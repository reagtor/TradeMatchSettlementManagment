using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amib.Threading;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.Entity.HK;
using MatchCenter.BLL.MatchData;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
using System.ServiceModel;
//using CommonRealtimeMarket.entity;
using MatchCenter.BLL.RealTime;
using MatchCenter.BLL.Common;
using MatchCenter.DAL.HK;
using MatchCenter.Entity;
using MatchCenter.BLL.interfaces;
using MatchCenter.BLL.PushBack;
using RealTime.Server.SModelData.HqData;

namespace MatchCenter.BLL.Match
{
    /// <summary>
    /// 初始化撮合中心港股类
    /// Title: 初始化撮合中心港股证券类
    /// Desc.:本类包括对应的商品代码（合约代码）初始化，对每个商品代码（合约代码）进下委托的接收
    ///       下单后撤单操作的接收，对应的所有委托的撮合，所撮合的驱动来源于行情的刷新推动撮合。
    ///        撮合成功交易记录回推柜台。
    /// Create BY：李健华
    /// Create Date：2009-10-16
    /// Desc.:添加港股撮合检验方法和撮合方法定义
    /// Update by：李健华
    /// Update date:2009-10-19
    /// Desc.:实现限价盘、增强限价盘、特别限价盘流程
    /// Update by：王伟
    /// Update date:2009-10-19
    /// Desc.:增加改单流程
    /// Update by：王伟
    /// Update date:2009-10-21
    /// </summary>
    public class HKStockMatcher
    {
        #region 变量定义

        #region 缓冲区定义
        /// <summary>
        /// 港股下单委托缓冲区操作接口类
        /// </summary>
        public QueueBufferBase<HKEntrustOrderInfo> orderCache;

        /// <summary>
        /// 内部撤单缓冲区
        /// </summary>
        private QueueBufferBase<HKEntrustOrderInfo> internalCancelOrader;

        /// <summary>
        /// 限价盘未撮合完撤单缓冲区
        /// </summary>
        private QueueBufferBase<HKEntrustOrderInfo> CancelBufferEntity;

        #endregion

        #region 委托单缓存区
        /// <summary>
        /// 存储【买限价】委托单缓存区
        /// </summary>
        private EntrustOrderData<HKEntrustOrderInfo> buyOrder;

        /// <summary>
        /// 存储【卖限价】委托单缓存区
        /// </summary>
        private EntrustOrderData<HKEntrustOrderInfo> sellOrder;

        #endregion

        #region 委托市场存量缓存区
        /// <summary>
        /// 存储【买】市场存量
        /// </summary>
        public MarketVolumeData buyMarketVolume;

        /// <summary>
        /// 存储【卖】市场存量
        /// </summary>
        public MarketVolumeData sellMarketVolume;

        #endregion

        /// <summary>
        /// 港股商品代码
        /// </summary>
        private string matchCode;

        /// <summary>
        /// 港股撮合商品所属交易所ID
        /// </summary>
        public int bourseTypeID = 0;

        /// <summary>
        /// 线程池
        /// </summary>
        private static SmartThreadPool smartPool = new SmartThreadPool();

        #endregion

        #region 构造函数
        /// <summary>
        /// 静态方法构造函数
        /// </summary>
        static HKStockMatcher()
        {
            smartPool.MaxThreads = 80;
            smartPool.MinThreads = 20;
            smartPool.Start();
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="code"></param>
        public HKStockMatcher(string code)
        {
            matchCode = code;
            buyOrder = new EntrustOrderData<HKEntrustOrderInfo>(Types.TransactionDirection.Buying);
            sellOrder = new EntrustOrderData<HKEntrustOrderInfo>(Types.TransactionDirection.Selling);

            //有先移除，是为了时间定时器执行每日开市重新初始化
            if (MatchCodeDictionary.hkStockMatchCodeDic.ContainsKey(code))
            {
                MatchCodeDictionary.hkStockMatchCodeDic[code] = null;
                MatchCodeDictionary.hkStockMatchCodeDic.Remove(code);
            }

            MatchCodeDictionary.hkStockMatchCodeDic.Add(matchCode, HKRealtimeMarketChangeEvent);

            CancelBufferEntity = new QueueBufferBase<HKEntrustOrderInfo>();
            CancelBufferEntity.QueueItemProcessEvent += ProcessCanceBussiness;
            orderCache = new QueueBufferBase<HKEntrustOrderInfo>();
            orderCache.QueueItemProcessEvent += ProcessBussiness;
            internalCancelOrader = new QueueBufferBase<HKEntrustOrderInfo>();
            internalCancelOrader.QueueItemProcessEvent += InternalCancelOraderProcessEvent;
            //市场存量
            buyMarketVolume = new MarketVolumeData();
            sellMarketVolume = new MarketVolumeData();
        }

        #endregion

        #region 接收下单撤单改单

        /// <summary>接收下单委托单操作</summary>
        /// <param name="model">委托实体</param>
        public void AcceptOrder(HKEntrustOrderInfo model)
        {
            try
            {
                //实体不能为空
                if (model == null)
                {
                    return;
                }

                //获取港股行情
                HKStock data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetHKStockData(matchCode);
                if (data == null)
                {

                    LogHelper.WriteError(matchCode, new Exception(GenerateInfo.CH_E007));
                    return;
                }

                // 1判断报价是否有涨跌停等 (内部方法应该当包含三种类型的价格判断(限价盘，增强限价盘，特别限价盘)
                string highLowMsg = "";
                bool isPriceEntrust = CostPriceCalculate.Instanse.HKComparePriceByMatchCode(model, data, out highLowMsg);
                if (isPriceEntrust == false)
                {
                    model.OrderMessage = highLowMsg;
                    internalCancelOrader.InsertQueueItem(model);//不可以下单做内部撤单处理
                    return;
                }

                //缓冲区不能为空
                if (orderCache != null)
                {
                    model.ReceiveTime = DateTime.Now;
                    HKEntrustOrderDal.Add(model);
                    orderCache.InsertQueueItem(model);
                    LogHelper.WriteDebug("接收委托单成功[委托单号:" + model.OrderNo + " 委托代码=" + model.HKSecuritiesCode + ",委托价格=" + model.OrderPrice + ",接收时间=" + DateTime.Now + "]");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("接收港股下单委托单操作异常" + ex.Message, ex);
            }
        }

        /// <summary>接收提交撤销委托单操作</summary>
        /// <param name="model">委托实体</param>
        public int CancelOrder(CancelEntity model)
        {
            LogHelper.WriteDebug("[撮合单元运行港股撤单方法]");
            //委托实体为空
            if (model == null)
            {
                return 0;
            }
            bool cancel = false;
            string code = Guid.NewGuid().ToString();
            var dataEntity = new CancelOrderEntity();
            dataEntity.Id = code;//委托单号码
            dataEntity.OrderNo = model.OldOrderNo;
            dataEntity.ChannelNo = model.ChannelNo; //通道号码
            decimal cancount = 0.00m;
            model.CancelCount = model.CancelCount + 1;

            #region  update ===2010-01-08
            string sholdercode = "";
            var entry = HKEntrustOrderDal.GetModel(model.OldOrderNo.Trim());
            if (entry != null)
            {
                sholdercode = entry.SholderCode;
            }
            if (!string.IsNullOrEmpty(sholdercode))
            {
                //存储委托单不能为空
                if (buyOrder != null)
                {
                    cancel = buyOrder.Remove(model.OldOrderNo, sholdercode, ref cancount);
                }

                if (cancel == false)
                {
                    cancel = sellOrder.Remove(model.OldOrderNo, sholdercode, ref cancount);

                }
            }
            //如果撤单不成功
            if (!cancel && model.CancelCount >= RulesDefaultValue.DefaultCancelFailCount / 2)
            {
                if (entry != null && entry.OrderVolume > 0)
                {
                    LogHelper.WriteError("CH-0002:港股撤单失败", new Exception("直接从数据库中撤单" + model.OldOrderNo.Trim()));
                    cancel = true;
                    cancount = entry.OrderVolume;
                }
            }
            //===================
            //存储委托单不能为空
            //if (buyOrder != null)
            //{
            //    cancel = buyOrder.Remove(model.OldOrderNo, ref cancount);
            //}

            //if (cancel == false)
            //{
            //    cancel = sellOrder.Remove(model.OldOrderNo, ref cancount);

            //}
            //
            #endregion===

            if (cancel)
            {
                dataEntity.OrderVolume = cancount;
                dataEntity.IsSuccess = true;
                dataEntity.Message = "撤单成功";
            }
            else
            {
                dataEntity.IsSuccess = false;
                dataEntity.Message = "CH-0002:委托单不存在。";
                // LogHelper.WriteDebug(" 撤单失败委托单不存在[" + " 委托代码=" + model.StockCode + ", 撤单时间=" + DateTime.Now + ",委托单号码=" + model.OldOrderNo +"]");
                return 0;
            }

            OperationContext context = null;
            //通道号码不能为空
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey((model.ChannelNo)))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }
            }

            try
            {
                dataEntity.OrderVolume = cancount;

                HKEntrustOrderDal.Update(model.OldOrderNo);//更新港股委托数据库
                //上下文不能为空
                if (context == null)
                {
                    LogHelper.WriteError("CH-500:港股撤单时通道上下文为空,撤单回推缓存到回推队列中", new Exception(model.OldOrderNo));
                    //保存到缓存回推队列中
                    TradePushBackImpl.Instanse.SaveHKCancelBack(dataEntity);
                    return 1;
                }
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    string showMsg = string.Format(GenerateInfo.CH_D009, "港股", code, model.StockCode, DateTime.Now, model.OldOrderNo, cancount);
                    LogHelper.WriteDebug(showMsg);
                    smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMsg);

                    callback.CancelHKStockOrderRpt(dataEntity);//回报数据

                }
                else
                {
                    LogHelper.WriteError("CH-501:港股撤单时通道GetCallbackChannel为空,撤单回推缓存到回推队列中", new Exception(model.OldOrderNo));
                    //保存到缓存回推队列中
                    TradePushBackImpl.Instanse.SaveHKCancelBack(dataEntity);
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-100:港股撤单wcf服务通道异常", ex);
                TradePushBackImpl.Instanse.SaveHKCancelBack(dataEntity);
                return 1;
            }
        }

        /// <summary>处理港股减量改单操作</summary>
        /// <param name="model">委托实体</param>
        public int HKModifyOrder(HKModifyEntity model)
        {
            LogHelper.WriteDebug("[撮合单元运行改委托单方法]");
            //委托实体为空
            if (model == null)
            {
                return 1;
            }

            //0表示不回报再进行改单，1表示规则验证不通过回报，2改单成功回报
            int modify = 0;
            //改单量
            decimal modcount = model.ModifyVolume;
            //撤单量
            decimal cancleCount = 0.0m;
            //改单异常
            string errorMsg = "";
            //改单是否成功
            bool isModifySuccess = false;
            //当前改单成功后委托量
            decimal nowOrderVolue = 0;
            if (modcount <= 0)
            {
                errorMsg = "CH-0003:委托改单量不能小于等于0";
                isModifySuccess = false;
                cancleCount = 0;
                modify = 1;
            }
            else
            {
                #region  update ===2010-01-08
                string sholdercode = "";
                var entry = HKEntrustOrderDal.GetModel(model.OldOrderNo.Trim());
                if (entry != null)
                {
                    sholdercode = entry.SholderCode.Trim();
                }
                if (!string.IsNullOrEmpty(sholdercode))
                {
                    if (buyOrder != null)
                    {
                        modify = buyOrder.Modify(model.OldOrderNo, sholdercode, modcount, ref nowOrderVolue, ref cancleCount, ref errorMsg);

                    }
                    //如果在买委托单中查询不到改单不成功再在卖队列中改单查询
                    if (modify == 0)
                    {
                        modify = sellOrder.Modify(model.OldOrderNo, sholdercode, modcount, ref nowOrderVolue, ref cancleCount, ref errorMsg);
                    }
                }
                //==========
                //存储委托单不能为空
                //if (buyOrder != null)
                //{
                //    modify = buyOrder.Modify(model.OldOrderNo, modcount, ref nowOrderVolue, ref cancleCount, ref errorMsg);

                //}
                ////如果在买委托单中查询不到改单不成功再在卖队列中改单查询
                //if (modify == 0)
                //{
                //    modify = sellOrder.Modify(model.OldOrderNo, modcount, ref nowOrderVolue, ref cancleCount, ref errorMsg);
                //}
                //===
                #endregion
            }
            if (modify <= 0)
            {
                return 0;//返回0再改单
            }

            if (modify == 1)
            {
                isModifySuccess = false;
            }
            else
            {
                isModifySuccess = true;
            }
            //初始化改单回报实体
            HKModifyBackEntity modifyPushBack = new HKModifyBackEntity();
            ////初始化撤单回报实体
            modifyPushBack.CanleOrderEntity = new CancelOrderEntity();

            //改单Code
            string code = Guid.NewGuid().ToString();
            modifyPushBack.Id = code;//委托单单号（改单）
            modifyPushBack.CanleOrderEntity.Id = code;//撤单委托单号码

            modifyPushBack.OrderNo = model.OldOrderNo;
            modifyPushBack.CanleOrderEntity.OrderNo = model.OldOrderNo;
            //通道号码
            modifyPushBack.ChannelNo = model.ChannelNo;
            modifyPushBack.CanleOrderEntity.ChannelNo = model.ChannelNo;

            modifyPushBack.CanleOrderEntity.OrderVolume = cancleCount;//撤单量
            modifyPushBack.CanleOrderEntity.IsSuccess = isModifySuccess;//是否改单成功
            modifyPushBack.CanleOrderEntity.Message = errorMsg;//改单信息

            modifyPushBack.ModifyVolume = modcount;//改单量
            modifyPushBack.IsSuccess = isModifySuccess;//是否改单成功
            modifyPushBack.Message = errorMsg;//改单信息

            //回送改单回报和撤单回报
            OperationContext context = null;
            //通道号码不能为空
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey((model.ChannelNo)))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }
            }
            //上下文不能为空
            if (context == null)
            {
                //这里应是故障恢复动作留下再修改实现
                //如果有异常应该先象以下这方法一样把成交记录缓存起来等待通首无异常再回推
                //TradePushBackImpl.Instanse.SaveCanceBack(cancleEntity);
                //这里因为已经成交了记录等，所以不再返加0，即不再做相关同样的改单重试操作
                return 1;
            }
            try
            {

                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    string showMsg = string.Format(GenerateInfo.CH_D009, "港股【改单】", code, model.StockCode, DateTime.Now, model.OldOrderNo, cancleCount);
                    LogHelper.WriteDebug(showMsg);
                    smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMsg);

                    HKEntrustOrderDal.Update(model.OldOrderNo, modcount, nowOrderVolue);//更新港股委托数据库

                    callback.ModifyHKStockOrderRpt(modifyPushBack);//回报数据                    

                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-100:撤单wcf服务通道异常", ex);
                //这里应是故障恢复动作留下再修改实现
                //如果有异常应该先象以下这方法一样把成交记录缓存起来等待通首无异常再回推
                //TradePushBackImpl.Instanse.SaveCanceBack(cancleEntity);
                return 1;
            }
        }

        #region 接收委托单缓冲数据处理数据
        /// <summary>
        /// 撮合中心缓冲数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessBussiness(object sender, QueueItemHandleEventArg<HKEntrustOrderInfo> e)
        {
            //委托对象不能为空
            if (e.Item == null)
            {
                return;
            }
            e.Item.ReceiveTime = DateTime.Now;
            if (e.Item.TradeType == (int)Types.TransactionDirection.Buying)
            {
                if (buyOrder != null)
                {
                    buyOrder.Add(e.Item);
                }
            }
            else if (e.Item.TradeType == (int)Types.TransactionDirection.Selling)
            {
                if (sellOrder != null)
                {
                    sellOrder.Add(e.Item);
                }
            }
        }
        #endregion
        #endregion

        #region 撮合方法
        #region 1.行情调度驱动撮合事件
        /// <summary>1.行情调度驱动撮合事件</summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void HKRealtimeMarketChangeEvent(object sender, HKStockDataChangeEventArg e)
        {
            if (e == null || e.HqData == null)
            {
                return;
            }
            if (e.HqData.CodeKey == matchCode)
            {
                //港股实现
                smartPool.QueueWorkItem(ProcessMatch, e.HqData);
            }
        }
        #endregion

        #region 2.行情驱动撮合证卷方法开始组装行情数据转换成内部执行实体

        /// <summary>2-1.行情驱动撮合证卷方法开始组装行情数据转换成内部执行实体</summary>
        /// <param name="hqData">行情实体</param>
        private void ProcessMatch(HKStock hqData)
        {
            if (hqData.CodeKey == matchCode)
            {
                //港股交易时间判断可以要修改
                if (TradeTimeManager.Instanse.IsMarchTime(bourseTypeID, DateTime.Now) == false)
                {
                    return;
                }
                //港股交易日期判断可以要修改
                if (TradeTimeManager.Instanse.IsMarchDate(bourseTypeID, DateTime.Now) == false)
                {
                    return;
                }
                HqMarketEntity GlobalMarketEntity = GetMarketEntity(hqData);
                Match(GlobalMarketEntity);
            }
        }

        /// <summary> 2-2.组装行情数据转换成内部执行实体</summary>
        /// <param name="hqExData">撮合中心实体</param>
        /// <returns></returns>
        private HqMarketEntity GetMarketEntity(HKStock hqExData)
        {
            if (hqExData == null)
            {
                return null;
            }
            var MarketEntity = new HqMarketEntity();

            #region  撮合中心行情【买】一至买五价、量
            MarketEntity.BuyFirstPrice = (decimal)hqExData.Buyprice1;
            MarketEntity.BuyFirstVolume = (decimal)hqExData.Buyvol1;
            MarketEntity.BuySecondPrice = (decimal)hqExData.Buyprice2;
            MarketEntity.BuySecondVolume = (decimal)hqExData.Buyvol2;
            MarketEntity.BuyThirdPrice = (decimal)hqExData.Buyprice3;
            MarketEntity.BuyThirdVolume = (decimal)hqExData.Buyvol3;
            MarketEntity.BuyFourthPrice = (decimal)hqExData.Buyprice4;
            MarketEntity.BuyFourthVolume = (decimal)hqExData.Buyvol4;
            MarketEntity.BuyFivePrice = (decimal)hqExData.Buyprice5;
            MarketEntity.BuyFiveVolume = (decimal)hqExData.Buyvol5;
            #endregion

            #region   撮合中心行情【卖】一至买五价、量
            MarketEntity.SellFirstPrice = (decimal)hqExData.Sellprice1;
            MarketEntity.SellFirstVolume = (decimal)hqExData.Sellvol1;
            MarketEntity.SellSecondPrice = (decimal)hqExData.Sellprice2;
            MarketEntity.SellSecondVolume = (decimal)hqExData.Sellvol2;
            MarketEntity.SellThirdPrice = (decimal)hqExData.Sellprice3;
            MarketEntity.SellThirdVolume = (decimal)hqExData.Sellvol3;
            MarketEntity.SellFourthPrice = (decimal)hqExData.Sellprice4;
            MarketEntity.SellFourthVolume = (decimal)hqExData.Sellvol4;
            MarketEntity.SellFivePrice = (decimal)hqExData.Sellprice5;
            MarketEntity.SellFiveVolume = (decimal)hqExData.Sellvol5;
            #endregion

            #region 这里要修改，要询问行情组件那里个标记为最后最底价等，再计算我们要的涨跌停价

            //MarketEntity.UpPrice = (decimal) hqExData.UpB;
            //MarketEntity.LowerPrice = (decimal) hqExData.LowB;
            //撮合中心跌停价 
            //double lowb = hqExData. - hqExData.YClose * fTemp;
            //hqExData.LowB = Convert.ToSingle(lowb);
            //MarketEntity.UpPrice = Math.Round((decimal)upb, 2);
            //MarketEntity.LowerPrice = Math.Round((decimal)lowb, 2);
            #endregion

            MarketEntity.LastPrice = (decimal)hqExData.Lasttrade;
            MarketEntity.LastVolume = (decimal)hqExData.PTrans;

            return MarketEntity;
        }
        #endregion

        #region 3.行情驱动开始撮合【买、卖】委托单
        /// <summary>
        /// 行情驱动买卖事件
        /// </summary>
        /// <param name="data"></param>
        public void Match(HqMarketEntity data)
        {
            if (data == null)
            {
                return;
            }
            try
            {
                smartPool.QueueWorkItem(MatchBuy, data);
                smartPool.QueueWorkItem(MatchSell, data);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH--0003:撮合中心撮合异常", ex);
                return;
            }
        }
        #endregion

        #region 4.根据行情撮合卖【限价盘，增强限价盘，特别限价盘】委托单

        /// <summary>4. 根据行情撮合卖【限价盘，增强限价盘，特别限价盘】委托单</summary>
        /// <param name="hqData">行情数据</param>
        private void MatchSell(HqMarketEntity hqData)
        {
            #region update 2010-01-08
            //撮合中心撮合实体
            Dictionary<string, List<HKEntrustOrderInfo>> orderDic = sellOrder.GetAcceptEntitys();
            //撮合中心撮合实体不能为空
            if (orderDic == null || orderDic.Count <= 0)
            {
                return;
            }

            ////撮合中心撮合实体
            //List<HKEntrustOrderInfo> matchEntitys = sellOrder.GetAcceptEntitys();
            ////撮合中心撮合实体不能为空
            //if (Utils.IsNullOrEmpty(matchEntitys))
            //{
            //    return;
            //}
            //Dictionary<string, List<HKEntrustOrderInfo>> orderDic = new Dictionary<string, List<HKEntrustOrderInfo>>();

            //#region 组合每个交易员的本次行情的所有委托列表
            //foreach (var model in matchEntitys)
            //{
            //    if (orderDic.ContainsKey(model.SholderCode))
            //    {
            //        orderDic[model.SholderCode].Add(model);
            //    }
            //    else
            //    {
            //        List<HKEntrustOrderInfo> list = new List<HKEntrustOrderInfo>();
            //        list.Add(model);
            //        orderDic[model.SholderCode] = list;
            //    }
            //}
            //#endregion

            #endregion

            #region 分发每个人的委托列表(对于每个交易员本次行情都是相同)

            foreach (var sholderCodeModel in orderDic)
            {
                List<HKEntrustOrderInfo> orderList = sholderCodeModel.Value;
                if (Utils.IsNullOrEmpty(orderList))
                {
                    continue;
                }
                HqMarketEntity hqDataModel = new HqMarketEntity();
                hqDataModel = hqData;
                //委托价格市场存量委托编号
                Dictionary<decimal, string> priceDic = new Dictionary<decimal, string>();
                foreach (HKEntrustOrderInfo item in orderList)
                {
                    if (item.OrderType == (int)Types.HKPriceType.SLO)
                    {
                        //特别限价单不考虑市场存量
                        MatchSpecialPriceSell(item, hqData);
                    }
                    else
                    {
                        string marketVolueOrderNo = "";
                        Types.MatchCenterState nowMatchState = item.MatchState;
                        decimal orderPrice = item.OrderPrice;

                        if (nowMatchState == Types.MatchCenterState.First)
                        {
                            if (priceDic.ContainsKey(orderPrice))
                            {
                                marketVolueOrderNo = priceDic[orderPrice];
                            }
                            else
                            {
                                marketVolueOrderNo = item.OrderNo;
                            }
                        }
                        //开始撮合
                        switch ((Types.HKPriceType)item.OrderType)
                        {
                            case Types.HKPriceType.LO:
                                MatchFixingPriceSell(item, hqData, ref marketVolueOrderNo);
                                break;
                            case Types.HKPriceType.ELO:
                                MatchHardPriceSell(item, hqData, ref marketVolueOrderNo);
                                break;
                            //case Types.HKPriceType.SLO:
                            //    MatchSpecialPriceSell(item, hqData);
                            //break;
                            //default:
                            //    break;
                        }

                        if (nowMatchState == Types.MatchCenterState.First && !string.IsNullOrEmpty(marketVolueOrderNo))
                        {
                            if (!priceDic.ContainsKey(orderPrice))
                            {
                                priceDic.Add(orderPrice, item.OrderNo);
                            }
                            else
                            {
                                priceDic[orderPrice] = marketVolueOrderNo;
                            }
                        }
                    }

                }
            }
            #endregion

        }

        /// <summary>4-1 根据行情【卖的限价盘】委托撮合五挡行情
        /// </summary>
        /// <param name="orderEntity">委托单</param>
        /// <param name="hqData">行情数据</param>
        /// <param name="marketVolueOrderNo">市场存量关联编号</param>
        private void MatchFixingPriceSell(HKEntrustOrderInfo orderEntity, HqMarketEntity hqData, ref string marketVolueOrderNo)
        {

            //委托实体不能为空
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            decimal markVolume;

            //LogHelper.WriteDebug("---->【港股】开始撮合卖限价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 市价最新成交价撮合成交，如果是第一次（开始）撮合不能撮合最后成交价
            if (orderEntity.MatchState != Types.MatchCenterState.First)
            {
                if (hqData.LastVolume > 0.00m)
                {
                    #region 价格小于最后成交价
                    if (orderEntity.OrderPrice < hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格小于最后成交价,委托量与市场成交量撮合.");

                        if (orderEntity.OrderVolume > hqData.LastVolume)
                        {
                            markVolume = hqData.LastVolume;
                            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            hqData.LastVolume = 0.00m;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                        }
                        else
                        {
                            markVolume = orderEntity.OrderVolume;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                            orderEntity.OrderVolume = 0.00m;
                            hqData.LastVolume = hqData.LastVolume - markVolume;
                            return;
                        }
                    }
                    #endregion

                    #region 当成交价等于委托价时时要考虑市场存量
                    if (orderEntity.OrderPrice == hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格等于最后成交价,市场存量与市场成交量撮合.撮合不完市场存量再与委托量撮合.");

                        //用户编号+价格+委托编号
                        string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + orderEntity.MarketVolumeNo;
                        decimal currentMarketVolume = 0;
                        if (string.IsNullOrEmpty(orderEntity.MarketVolumeNo))
                        {
                            currentMarketVolume = 0;
                        }
                        else
                        {
                            currentMarketVolume = sellMarketVolume.GetMarketVolume(key);
                        }


                        decimal hqLastVolume = hqData.LastVolume;
                        //if (orderEntity.MarkLeft >= hqData.LastVolume)
                        //{
                        if (currentMarketVolume >= hqLastVolume)
                        {
                            hqData.LastVolume = 0.00m;
                            // orderEntity.MarkLeft = orderEntity.MarkLeft - hqData.LastVolume;
                            orderEntity.MarkLeft = currentMarketVolume - hqLastVolume;
                            sellMarketVolume.ModifyMarketVolume(key, hqLastVolume);
                        }
                        else
                        {
                            decimal volume = 0;
                            //volume = hqData.LastVolume - orderEntity.MarkLeft;
                            volume = hqLastVolume - currentMarketVolume;//撮合市场存量
                            //撮合市场存量后剩余的最后成交量再撮合委托量
                            if (volume >= orderEntity.OrderVolume)//剩余量大于委托量
                            {
                                markVolume = orderEntity.OrderVolume;
                                orderEntity.OrderVolume = 0;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = hqLastVolume - markVolume - currentMarketVolume;
                            }
                            else
                            {
                                markVolume = volume;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = 0.00m;
                                //orderEntity.MarkLeft = 0.00m;
                                orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            }
                            sellMarketVolume.ModifyMarketVolume(key, currentMarketVolume);

                            orderEntity.MarkLeft = 0.00m;
                        }
                        HKEntrustOrderDal.Update(orderEntity.OrderNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        HKEntrustOrderDal.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        if (orderEntity.OrderVolume <= 0)
                        {
                            return;
                        }
                    }
                    #endregion
                }
            }
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.买一价=" + hqData.BuyFirstPrice + ",买一量=" + hqData.BuyFirstVolume + ",买二价=" + hqData.BuySecondPrice + ",买二量=" + hqData.BuySecondVolume + ",买三价=" + hqData.BuyThirdPrice + ",买三量=" + hqData.BuyThirdVolume + ",买四价=" + hqData.BuyFourthPrice + ",买四量=" + hqData.BuyFourthVolume + ",买五价=" + hqData.BuyFivePrice + ",买五量=" + hqData.BuyFiveVolume);

            #region 买一量撮合成交
            if (hqData.BuyFirstVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFirstPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyFirstVolume)
                    {
                        markVolume = hqData.BuyFirstVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                        hqData.BuyFirstVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.BuyFirstVolume = hqData.BuyFirstVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 买二量撮合成交
            if (hqData.BuySecondVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuySecondPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuySecondVolume)
                    {
                        markVolume = hqData.BuySecondVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                        hqData.BuySecondVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.BuySecondVolume = hqData.BuySecondVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 买三量撮合成交
            if (hqData.BuyThirdVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyThirdPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyThirdVolume)
                    {
                        markVolume = hqData.BuyThirdVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                        hqData.BuyThirdVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                        hqData.BuyThirdVolume = hqData.BuyThirdVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 买四量撮合成交
            if (hqData.BuyFourthVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFourthPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyFourthVolume)
                    {
                        markVolume = hqData.BuyFourthVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.BuyFourthVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.BuyFourthPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFourthPrice, markVolume);
                        hqData.BuyFourthVolume = hqData.BuyFourthVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 买五量撮合成交
            if (hqData.BuyFiveVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFivePrice)
                {
                    //撮合中心数量比较
                    if (orderEntity.OrderVolume > hqData.BuyFiveVolume)
                    {
                        markVolume = hqData.BuyFiveVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                        hqData.BuyFiveVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                        hqData.BuyFiveVolume = hqData.BuyFiveVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 委托担还有没有撮合完的再添加到委托撮合列表中等待行情再撮合
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的再添加到委托撮合列表中等待行情再撮合. OrderVolume=" + orderEntity.OrderVolume);

                if (Types.MatchCenterState.First == orderEntity.MatchState)
                {
                    //用户编号+价格+委托编号
                    string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + marketVolueOrderNo;

                    //获取市场存量
                    decimal marketVolume = 0;
                    if (marketVolueOrderNo != orderEntity.OrderNo)
                    {
                        marketVolume = sellMarketVolume.GetMarketVolume(key);
                    }
                    else
                    {
                        marketVolume = GetSellMarkLeft(orderEntity.OrderPrice, hqData);
                    }

                    orderEntity.MarkLeft = marketVolume;
                    orderEntity.MarketVolumeNo = marketVolueOrderNo;

                    //把市场存量记录下来
                    sellMarketVolume.AddMarketVolume(key, marketVolume);
                    //更新数据库表中的数据市场存量和记录市场存量的关联并更新撮合状态
                    HKEntrustOrderDal.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
                }
                orderEntity.MatchState = Types.MatchCenterState.other;
                sellOrder.Add(orderEntity);
            }
            else
            {
                if (Types.MatchCenterState.First == orderEntity.MatchState && marketVolueOrderNo == orderEntity.OrderNo)
                {
                    marketVolueOrderNo = "";
                }
            }
            #endregion
        }

        /// <summary>4-2.根据行情【卖的增强限价盘】委托撮合五档行情撮合成交
        /// </summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情数据</param>
        /// <param name="marketVolueOrderNo">市场存量关联编号</param>
        private void MatchHardPriceSell(HKEntrustOrderInfo orderEntity, HqMarketEntity hqData, ref string marketVolueOrderNo)
        {
            //委托实体不能为空
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            decimal markVolume;

            //LogHelper.WriteDebug("---->【港股】开始撮合卖增强限价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 市价最新成交价撮合成交，如果是第一次（开始）撮合不能撮合最后成交价
            if (orderEntity.MatchState != Types.MatchCenterState.First)
            {
                if (hqData.LastVolume > 0.00m)
                {
                    #region 价格小于最后成交价
                    if (orderEntity.OrderPrice < hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格小于最后成交价,委托量与市场成交量撮合.");

                        if (orderEntity.OrderVolume > hqData.LastVolume)
                        {
                            markVolume = hqData.LastVolume;
                            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            hqData.LastVolume = 0.00m;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                        }
                        else
                        {
                            markVolume = orderEntity.OrderVolume;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                            orderEntity.OrderVolume = 0.00m;
                            hqData.LastVolume = hqData.LastVolume - markVolume;
                            return;
                        }
                    }
                    #endregion

                    #region 当成交价等于委托价时时要考虑市场存量
                    if (orderEntity.OrderPrice == hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格等于最后成交价,市场存量与市场成交量撮合.撮合不完市场存量再与委托量撮合.");

                        //用户编号+价格+委托编号
                        string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + orderEntity.MarketVolumeNo;
                        decimal currentMarketVolume = 0;
                        if (string.IsNullOrEmpty(orderEntity.MarketVolumeNo))
                        {
                            currentMarketVolume = 0;
                        }
                        else
                        {
                            currentMarketVolume = sellMarketVolume.GetMarketVolume(key);
                        }


                        decimal hqLastVolume = hqData.LastVolume;
                        //if (orderEntity.MarkLeft >= hqData.LastVolume)
                        //{
                        if (currentMarketVolume >= hqLastVolume)
                        {
                            hqData.LastVolume = 0.00m;
                            // orderEntity.MarkLeft = orderEntity.MarkLeft - hqData.LastVolume;
                            orderEntity.MarkLeft = currentMarketVolume - hqLastVolume;
                            sellMarketVolume.ModifyMarketVolume(key, hqLastVolume);
                        }
                        else
                        {
                            decimal volume = 0;
                            //volume = hqData.LastVolume - orderEntity.MarkLeft;
                            volume = hqLastVolume - currentMarketVolume;//撮合市场存量
                            //撮合市场存量后剩余的最后成交量再撮合委托量
                            if (volume >= orderEntity.OrderVolume)//剩余量大于委托量
                            {
                                markVolume = orderEntity.OrderVolume;
                                orderEntity.OrderVolume = 0;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = hqLastVolume - markVolume - currentMarketVolume;
                            }
                            else
                            {
                                markVolume = volume;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = 0.00m;
                                //orderEntity.MarkLeft = 0.00m;
                                orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            }
                            sellMarketVolume.ModifyMarketVolume(key, currentMarketVolume);

                            orderEntity.MarkLeft = 0.00m;
                        }
                        HKEntrustOrderDal.Update(orderEntity.OrderNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        HKEntrustOrderDal.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        if (orderEntity.OrderVolume <= 0)
                        {
                            return;
                        }
                    }
                    #endregion
                }
            }
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.买一价=" + hqData.BuyFirstPrice + ",买一量=" + hqData.BuyFirstVolume + ",买二价=" + hqData.BuySecondPrice + ",买二量=" + hqData.BuySecondVolume + ",买三价=" + hqData.BuyThirdPrice + ",买三量=" + hqData.BuyThirdVolume + ",买四价=" + hqData.BuyFourthPrice + ",买四量=" + hqData.BuyFourthVolume + ",买五价=" + hqData.BuyFivePrice + ",买五量=" + hqData.BuyFiveVolume);

            #region 买一量撮合成交
            if (hqData.BuyFirstVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFirstPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyFirstVolume)
                    {
                        markVolume = hqData.BuyFirstVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                        hqData.BuyFirstVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.BuyFirstVolume = hqData.BuyFirstVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 买二量撮合成交
            if (hqData.BuySecondVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuySecondPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuySecondVolume)
                    {
                        markVolume = hqData.BuySecondVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                        hqData.BuySecondVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.BuySecondVolume = hqData.BuySecondVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 买三量撮合成交
            if (hqData.BuyThirdVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyThirdPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyThirdVolume)
                    {
                        markVolume = hqData.BuyThirdVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                        hqData.BuyThirdVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                        hqData.BuyThirdVolume = hqData.BuyThirdVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 买四量撮合成交
            if (hqData.BuyFourthVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFourthPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyFourthVolume)
                    {
                        markVolume = hqData.BuyFourthVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.BuyFourthVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.BuyFourthPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFourthPrice, markVolume);
                        hqData.BuyFourthVolume = hqData.BuyFourthVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 买五量撮合成交
            if (hqData.BuyFiveVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFivePrice)
                {
                    //撮合中心数量比较
                    if (orderEntity.OrderVolume > hqData.BuyFiveVolume)
                    {
                        markVolume = hqData.BuyFiveVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                        hqData.BuyFiveVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                        hqData.BuyFiveVolume = hqData.BuyFiveVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 委托单还有没有撮合完的，类型改为限价单，重新加入队列
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的，改为限价单，再添加到委托撮合列表中等待行情再撮合. OrderVolume=" + orderEntity.OrderVolume);

                if (Types.MatchCenterState.First == orderEntity.MatchState)
                {
                    //用户编号+价格+委托编号
                    string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + marketVolueOrderNo;

                    //获取市场存量
                    decimal marketVolume = 0;
                    if (marketVolueOrderNo != orderEntity.OrderNo)
                    {
                        marketVolume = sellMarketVolume.GetMarketVolume(key);
                    }
                    else
                    {
                        marketVolume = GetSellMarkLeft(orderEntity.OrderPrice, hqData);
                    }

                    orderEntity.MarkLeft = marketVolume;
                    orderEntity.MarketVolumeNo = marketVolueOrderNo;

                    //把市场存量记录下来
                    sellMarketVolume.AddMarketVolume(key, marketVolume);
                    //更新数据库表中的数据市场存量和记录市场存量的关联并更新撮合状态
                    HKEntrustOrderDal.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
                }
                orderEntity.MatchState = Types.MatchCenterState.other;
                orderEntity.OrderType = (int)Types.HKPriceType.LO;
                sellOrder.Add(orderEntity);
            }
            else
            {
                if (Types.MatchCenterState.First == orderEntity.MatchState && marketVolueOrderNo == orderEntity.OrderNo)
                {
                    marketVolueOrderNo = "";
                }
            }
            #endregion
        }

        /// <summary>4-3.根据行情【卖的特别限价盘】委托撮合五档行情撮合成交
        /// </summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情数据</param>
        private void MatchSpecialPriceSell(HKEntrustOrderInfo orderEntity, HqMarketEntity hqData)
        {

            //委托实体不能为空
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            decimal markVolume;

            //LogHelper.WriteDebug("---->【港股】开始撮合卖特别限价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 特别限价盘和现货的市价相同
            //if (orderEntity.MatchState != Types.MatchCenterState.First)
            //{
            if (hqData.LastVolume > 0.00m)
            {
                #region 价格小于最后成交价
                if (orderEntity.OrderPrice <= hqData.LastPrice)
                {
                    //LogHelper.WriteDebug("---->价格小于等于最后成交价,委托量与市场成交量撮合.");

                    if (orderEntity.OrderVolume > hqData.LastVolume)
                    {
                        markVolume = hqData.LastVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.LastVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.LastVolume = hqData.LastVolume - markVolume;
                        return;
                    }
                }
                #endregion
            }
            //}
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.买一价=" + hqData.BuyFirstPrice + ",买一量=" + hqData.BuyFirstVolume + ",买二价=" + hqData.BuySecondPrice + ",买二量=" + hqData.BuySecondVolume + ",买三价=" + hqData.BuyThirdPrice + ",买三量=" + hqData.BuyThirdVolume + ",买四价=" + hqData.BuyFourthPrice + ",买四量=" + hqData.BuyFourthVolume + ",买五价=" + hqData.BuyFivePrice + ",买五量=" + hqData.BuyFiveVolume);

            #region 买一量撮合成交
            if (hqData.BuyFirstVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFirstPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyFirstVolume)
                    {
                        markVolume = hqData.BuyFirstVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                        hqData.BuyFirstVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.BuyFirstVolume = hqData.BuyFirstVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 买二量撮合成交
            if (hqData.BuySecondVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuySecondPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuySecondVolume)
                    {
                        markVolume = hqData.BuySecondVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                        hqData.BuySecondVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.BuySecondVolume = hqData.BuySecondVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 买三量撮合成交
            if (hqData.BuyThirdVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyThirdPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyThirdVolume)
                    {
                        markVolume = hqData.BuyThirdVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                        hqData.BuyThirdVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                        hqData.BuyThirdVolume = hqData.BuyThirdVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 买四量撮合成交
            if (hqData.BuyFourthVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFourthPrice)
                {
                    if (orderEntity.OrderVolume > hqData.BuyFourthVolume)
                    {
                        markVolume = hqData.BuyFourthVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.BuyFourthVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.BuyFourthPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFourthPrice, markVolume);
                        hqData.BuyFourthVolume = hqData.BuyFourthVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 买五量撮合成交
            if (hqData.BuyFiveVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFivePrice)
                {
                    //撮合中心数量比较
                    if (orderEntity.OrderVolume > hqData.BuyFiveVolume)
                    {
                        markVolume = hqData.BuyFiveVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                        hqData.BuyFiveVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                        hqData.BuyFiveVolume = hqData.BuyFiveVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 委托担还有没有撮合完的撤单
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的直接撤单. OrderVolume=" + orderEntity.OrderVolume);
                //sellOrder.Add(orderEntity);
                CancelBufferEntity.InsertQueueItem(orderEntity);
            }
            #endregion
        }

        #endregion

        #region 5.根据行情撮合买【限价盘，增强限价盘，特别限价盘】委托单

        /// <summary>5. 根据行情撮合买【限价盘，增强限价盘，特别限价盘】委托单</summary>
        /// <param name="hqData">行情数据</param>
        private void MatchBuy(HqMarketEntity hqData)
        {

            #region update 2010-01-08
            //撮合中心撮合实体
            Dictionary<string, List<HKEntrustOrderInfo>> orderDic = buyOrder.GetAcceptEntitys();
            //撮合中心撮合实体不能为空
            if (orderDic == null || orderDic.Count <= 0)
            {
                return;
            }


            //List<HKEntrustOrderInfo> matchBuyList = buyOrder.GetAcceptEntitys();
            //if (Utils.IsNullOrEmpty(matchBuyList))
            //{
            //    return;
            //}
            //Dictionary<string, List<HKEntrustOrderInfo>> orderDic = new Dictionary<string, List<HKEntrustOrderInfo>>();

            //#region 组合每个交易员的本次行情的所有委托列表
            //foreach (var model in matchBuyList)
            //{
            //    if (orderDic.ContainsKey(model.SholderCode))
            //    {
            //        orderDic[model.SholderCode].Add(model);
            //    }
            //    else
            //    {
            //        List<HKEntrustOrderInfo> list = new List<HKEntrustOrderInfo>();
            //        list.Add(model);
            //        orderDic[model.SholderCode] = list;
            //    }
            //}
            //#endregion
            #endregion


            #region 分发每个人的委托列表(对于每个交易员本次行情都是相同)

            foreach (var sholderCodeModel in orderDic)
            {
                List<HKEntrustOrderInfo> orderList = sholderCodeModel.Value;
                if (Utils.IsNullOrEmpty(orderList))
                {
                    continue;
                }

                HqMarketEntity hqDataModel = new HqMarketEntity();
                hqDataModel = hqData;
                //委托价格市场存量委托编号
                Dictionary<decimal, string> priceDic = new Dictionary<decimal, string>();
                foreach (HKEntrustOrderInfo item in orderList)
                {
                    if (item.OrderType == (int)Types.HKPriceType.SLO)
                    {
                        //特别限价单不考虑市场存量
                        MatchSpecialPriceBuy(item, hqData);
                    }
                    else
                    {
                        string marketVolueOrderNo = "";
                        Types.MatchCenterState nowMatchState = item.MatchState;
                        decimal orderPrice = item.OrderPrice;

                        if (nowMatchState == Types.MatchCenterState.First)
                        {
                            if (priceDic.ContainsKey(orderPrice))
                            {
                                marketVolueOrderNo = priceDic[orderPrice];
                            }
                            else
                            {
                                marketVolueOrderNo = item.OrderNo;
                            }
                        }
                        //开始撮合
                        switch ((Types.HKPriceType)item.OrderType)
                        {
                            case Types.HKPriceType.LO:
                                MatchFixingPriceBuy(item, hqData, ref marketVolueOrderNo);
                                break;
                            case Types.HKPriceType.ELO:
                                MatchHardPriceBuy(item, hqData, ref marketVolueOrderNo);
                                break;
                        }

                        if (nowMatchState == Types.MatchCenterState.First && !string.IsNullOrEmpty(marketVolueOrderNo))
                        {
                            if (!priceDic.ContainsKey(orderPrice))
                            {
                                priceDic.Add(orderPrice, item.OrderNo);
                            }
                            else
                            {
                                priceDic[orderPrice] = marketVolueOrderNo;
                            }
                        }
                    }
                }
            }
            #endregion

        }

        /// <summary>5-1 根据行情【买的限价盘】委托撮合五挡行情
        /// </summary>
        /// <param name="orderEntity">委托单</param>
        /// <param name="hqData">行情数据</param>
        /// <param name="marketVolueOrderNo">市场存量委托编号</param>
        private void MatchFixingPriceBuy(HKEntrustOrderInfo orderEntity, HqMarketEntity hqData, ref string marketVolueOrderNo)
        {
            if (orderEntity == null || hqData == null)
            {
                return;
            }

            decimal markVolume = 0;

            //LogHelper.WriteDebug("---->【港股】开始撮合买限价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 修改实现买的限价盘撮合流程

            #region 市价最新成交价撮合买成交，如果是第一次（开始）撮合不能撮合最后成交价
            if (orderEntity.MatchState != Types.MatchCenterState.First)
            {
                if (hqData.LastVolume > 0.00m)
                {
                    #region 价格大于最后成交价
                    if (orderEntity.OrderPrice > hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格大于最后成交价,委托量与市场成交量撮合.");

                        if (orderEntity.OrderVolume > hqData.LastVolume)
                        {
                            markVolume = hqData.LastVolume;
                            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            hqData.LastVolume = 0.00m;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                        }
                        else
                        {
                            markVolume = orderEntity.OrderVolume;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                            orderEntity.OrderVolume = 0.00m;
                            hqData.LastVolume = hqData.LastVolume - markVolume;
                            return;
                        }
                    }
                    #endregion

                    #region 当成交价等于委托价时时要考虑市场存量
                    if (orderEntity.OrderPrice == hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格等于最后成交价,市场存量与市场成交量撮合.撮合不完市场存量再与委托量撮合.");

                        //用户编号+价格+委托编号
                        string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + orderEntity.MarketVolumeNo;

                        decimal currentMarketVolume = 0;
                        if (string.IsNullOrEmpty(orderEntity.MarketVolumeNo))
                        {
                            currentMarketVolume = 0;
                        }
                        else
                        {
                            currentMarketVolume = buyMarketVolume.GetMarketVolume(key);
                        }
                        decimal hqLastVolume = hqData.LastVolume;
                        //if (orderEntity.MarkLeft >= hqData.LastVolume)
                        //{
                        if (currentMarketVolume >= hqLastVolume)
                        {
                            hqData.LastVolume = 0.00m;
                            // orderEntity.MarkLeft = orderEntity.MarkLeft - hqData.LastVolume;
                            orderEntity.MarkLeft = currentMarketVolume - hqLastVolume;
                            buyMarketVolume.ModifyMarketVolume(key, hqLastVolume);
                        }
                        else
                        {
                            decimal volume = 0;
                            //volume = hqData.LastVolume - orderEntity.MarkLeft;
                            volume = hqLastVolume - currentMarketVolume;//撮合市场存量
                            //撮合市场存量后剩余的最后成交量再撮合委托量
                            if (volume >= orderEntity.OrderVolume)//剩余量大于委托量
                            {
                                markVolume = orderEntity.OrderVolume;
                                orderEntity.OrderVolume = 0;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = hqLastVolume - markVolume - currentMarketVolume;
                            }
                            else
                            {
                                markVolume = volume;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = 0.00m;
                                //orderEntity.MarkLeft = 0.00m;
                                orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            }
                            buyMarketVolume.ModifyMarketVolume(key, currentMarketVolume);

                            orderEntity.MarkLeft = 0.00m;
                        }
                        HKEntrustOrderDal.Update(orderEntity.OrderNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        HKEntrustOrderDal.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        if (orderEntity.OrderVolume <= 0)
                        {
                            return;
                        }
                    }
                    #endregion
                }
            }
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.卖一价=" + hqData.SellFirstPrice + ",卖一量=" + hqData.SellFirstVolume + ",卖二价=" + hqData.SellSecondPrice + ",卖二量=" + hqData.SellSecondVolume + ",卖三价=" + hqData.SellThirdPrice + ",卖三量=" + hqData.SellThirdVolume + ",卖四价=" + hqData.SellFourthPrice + ",卖四量=" + hqData.SellFourthVolume + ",卖五价=" + hqData.SellFivePrice + ",卖五量=" + hqData.SellFiveVolume);

            #region 市价卖一量撮合成交
            if (hqData.SellFirstVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFirstPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFirstVolume)
                    {
                        markVolume = hqData.SellFirstVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFirstVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFirstVolume = hqData.SellFirstVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖二量撮合成交
            if (hqData.SellSecondVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellSecondPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellSecondVolume)
                    {
                        markVolume = hqData.SellSecondVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellSecondVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellSecondVolume = hqData.SellSecondVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖三量撮合成交
            if (hqData.SellThirdVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellThirdPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellThirdVolume)
                    {
                        markVolume = hqData.SellThirdVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellThirdVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellThirdVolume = hqData.SellThirdVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖四量撮合成交
            if (hqData.SellFourthVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFourthPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFourthVolume)
                    {
                        markVolume = hqData.SellFourthVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFourthVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFourthVolume = hqData.SellFourthVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖五量撮合成交
            if (hqData.SellFiveVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFivePrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFiveVolume)
                    {
                        markVolume = hqData.SellFiveVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFiveVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFiveVolume = hqData.SellFiveVolume - markVolume;
                        return;

                    }
                }
            }
            #endregion

            #region 买市撮合，如果五档量撮合不完加入缓冲区，等待下次撮合
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的再添加到委托撮合列表中等待行情再撮合.OrderVolume=" + orderEntity.OrderVolume);

                if (Types.MatchCenterState.First == orderEntity.MatchState)
                {                        //用户编号+价格+委托编号
                    string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + marketVolueOrderNo;

                    //获取市场存量
                    decimal marketVolume = 0;
                    if (marketVolueOrderNo != orderEntity.OrderNo)
                    {
                        marketVolume = buyMarketVolume.GetMarketVolume(key);
                    }
                    else
                    {
                        marketVolume = GetBuyMarkLeft(orderEntity.OrderPrice, hqData);
                    }

                    orderEntity.MarkLeft = marketVolume;
                    orderEntity.MarketVolumeNo = marketVolueOrderNo;

                    //把市场存量记录下来
                    buyMarketVolume.AddMarketVolume(key, marketVolume);
                    //更新数据库表中的数据市场存量和记录市场存量的关联并更新撮合状态
                    HKEntrustOrderDal.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
                }

                orderEntity.MatchState = Types.MatchCenterState.other;
                buyOrder.Add(orderEntity);
            }
            else
            {
                if (Types.MatchCenterState.First == orderEntity.MatchState && marketVolueOrderNo == orderEntity.OrderNo)
                {
                    marketVolueOrderNo = "";
                }
            }

            #endregion
            #endregion
        }

        /// <summary>5-2.根据行情【买的增强限价盘】委托撮合五档行情撮合成交
        /// </summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情数据</param>
        /// <param name="marketVolueOrderNo">市场存量委托编号</param>
        private void MatchHardPriceBuy(HKEntrustOrderInfo orderEntity, HqMarketEntity hqData, ref string marketVolueOrderNo)
        {
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            decimal markVolume;

            //LogHelper.WriteDebug("---->【港股】开始撮合买增强限价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 修改实现买的增强限价盘撮合流程
            #region 撮合最后成交价成交，如果是第一次（开始）撮合不能撮合最后成交价
            if (orderEntity.MatchState != Types.MatchCenterState.First)
            {

                if (hqData.LastVolume > 0.00m)
                {
                    #region 价格大于最后成交价
                    if (orderEntity.OrderPrice > hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格大于最后成交价,委托量与市场成交量撮合.");

                        if (orderEntity.OrderVolume > hqData.LastVolume)
                        {
                            markVolume = hqData.LastVolume;
                            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                            hqData.LastVolume = 0.00m;
                        }
                        else
                        {
                            markVolume = orderEntity.OrderVolume;
                            InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                            orderEntity.OrderVolume = 0.00m;
                            hqData.LastVolume = hqData.LastVolume - markVolume;
                            return;
                        }
                    }
                    #endregion

                    #region 当成交价等于委托价时时要考虑市场存量
                    if (orderEntity.OrderPrice == hqData.LastPrice)
                    {
                        //LogHelper.WriteDebug("---->价格等于最后成交价,市场存量与市场成交量撮合.撮合不完市场存量再与委托量撮合.");

                        //用户编号+价格+委托编号
                        string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + orderEntity.MarketVolumeNo;
                        decimal currentMarketVolume = 0;
                        if (string.IsNullOrEmpty(orderEntity.MarketVolumeNo))
                        {
                            currentMarketVolume = 0;
                        }
                        else
                        {
                            currentMarketVolume = buyMarketVolume.GetMarketVolume(key);
                        }

                        decimal hqLastVolume = hqData.LastVolume;
                        //if (orderEntity.MarkLeft >= hqData.LastVolume)
                        //{
                        if (currentMarketVolume >= hqLastVolume)
                        {
                            hqData.LastVolume = 0.00m;
                            // orderEntity.MarkLeft = orderEntity.MarkLeft - hqData.LastVolume;
                            orderEntity.MarkLeft = currentMarketVolume - hqLastVolume;
                            buyMarketVolume.ModifyMarketVolume(key, hqLastVolume);
                        }
                        else
                        {
                            decimal volume = 0;
                            //volume = hqData.LastVolume - orderEntity.MarkLeft;
                            volume = hqLastVolume - currentMarketVolume;//撮合市场存量
                            //撮合市场存量后剩余的最后成交量再撮合委托量
                            if (volume >= orderEntity.OrderVolume)//剩余量大于委托量
                            {
                                markVolume = orderEntity.OrderVolume;
                                orderEntity.OrderVolume = 0;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = hqLastVolume - markVolume - currentMarketVolume;
                            }
                            else
                            {
                                markVolume = volume;
                                InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                                hqData.LastVolume = 0.00m;
                                //orderEntity.MarkLeft = 0.00m;
                                orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                            }
                            buyMarketVolume.ModifyMarketVolume(key, currentMarketVolume);

                            orderEntity.MarkLeft = 0.00m;
                        }
                        HKEntrustOrderDal.Update(orderEntity.OrderNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        HKEntrustOrderDal.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        if (orderEntity.OrderVolume <= 0)
                        {
                            return;
                        }
                    }

                    #endregion
                }
            }
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.卖一价=" + hqData.SellFirstPrice + ",卖一量=" + hqData.SellFirstVolume + ",卖二价=" + hqData.SellSecondPrice + ",卖二量=" + hqData.SellSecondVolume + ",卖三价=" + hqData.SellThirdPrice + ",卖三量=" + hqData.SellThirdVolume + ",卖四价=" + hqData.SellFourthPrice + ",卖四量=" + hqData.SellFourthVolume + ",卖五价=" + hqData.SellFivePrice + ",卖五量=" + hqData.SellFiveVolume);

            #region 卖一量撮合成交
            if (hqData.SellFirstVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFirstPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFirstVolume)
                    {
                        markVolume = hqData.SellFirstVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
                        hqData.SellFirstVolume = 0.00m;
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFirstVolume = hqData.SellFirstVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 卖二量撮合成交
            if (hqData.SellSecondVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellSecondPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellSecondVolume)
                    {
                        markVolume = hqData.SellSecondVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellSecondVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
                        hqData.SellSecondVolume = hqData.SellSecondVolume - markVolume;
                        orderEntity.OrderVolume = 0.00m;
                        return;
                    }
                }
            }
            #endregion

            #region 卖三量撮合成交
            if (hqData.SellThirdVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellThirdPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellThirdVolume)
                    {
                        markVolume = hqData.SellThirdVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellThirdVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellThirdVolume = hqData.SellThirdVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 卖四量撮合成交
            if (hqData.SellFourthVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFourthPrice)
                {
                    //撮合中心数量比较
                    if (orderEntity.OrderVolume > hqData.SellFourthVolume)
                    {
                        markVolume = hqData.SellFourthVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFourthVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFourthVolume = hqData.SellFourthVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 卖五量撮合成交
            if (hqData.SellFiveVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFivePrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFiveVolume)
                    {
                        markVolume = hqData.SellFiveVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFiveVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFiveVolume = hqData.SellFiveVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 委托单还有没有撮合完的修改为限价盘，加入队列等待下次撮合。

            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的，改为限价单，再添加到委托撮合列表中等待行情再撮合. OrderVolume=" + orderEntity.OrderVolume);

                if (Types.MatchCenterState.First == orderEntity.MatchState)
                {
                    //用户编号+价格+委托编号
                    string key = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + marketVolueOrderNo;

                    //获取市场存量
                    decimal marketVolume = 0;
                    if (marketVolueOrderNo != orderEntity.OrderNo)
                    {
                        marketVolume = buyMarketVolume.GetMarketVolume(key);
                    }
                    else
                    {
                        marketVolume = GetBuyMarkLeft(orderEntity.OrderPrice, hqData);
                    }

                    orderEntity.MarkLeft = marketVolume;
                    orderEntity.MarketVolumeNo = marketVolueOrderNo;

                    //把市场存量记录下来
                    buyMarketVolume.AddMarketVolume(key, marketVolume);
                    //更新数据库表中的数据市场存量和记录市场存量的关联并更新撮合状态
                    HKEntrustOrderDal.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
                }

                orderEntity.MatchState = Types.MatchCenterState.other;
                //流程修改：增强限价盘
                orderEntity.OrderType = (int)Types.HKPriceType.LO;
                buyOrder.Add(orderEntity);
            }
            else
            {
                if (Types.MatchCenterState.First == orderEntity.MatchState && marketVolueOrderNo == orderEntity.OrderNo)
                {
                    marketVolueOrderNo = "";
                }
            }

            #endregion
            #endregion
        }
        /// <summary>5-3.根据行情【买的特别限价盘】委托撮合五档行情撮合成交
        /// </summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情数据</param>
        private void MatchSpecialPriceBuy(HKEntrustOrderInfo orderEntity, HqMarketEntity hqData)
        {
            if (orderEntity == null || hqData == null)
            {
                return;
            }

            decimal markVolume = 0;

            //LogHelper.WriteDebug("---->【港股】开始撮合买特别限价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 修改实现买的限价盘撮合流程

            #region 市价最新成交价撮合买成交
            if (hqData.LastVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.LastPrice)
                {
                    //LogHelper.WriteDebug("---->价格大于等于最后成交价,委托量与市场成交量撮合.");

                    if (orderEntity.OrderVolume > hqData.LastVolume)
                    {
                        markVolume = hqData.LastVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.LastVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.LastVolume = hqData.LastVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.卖一价=" + hqData.SellFirstPrice + ",卖一量=" + hqData.SellFirstVolume + ",卖二价=" + hqData.SellSecondPrice + ",卖二量=" + hqData.SellSecondVolume + ",卖三价=" + hqData.SellThirdPrice + ",卖三量=" + hqData.SellThirdVolume + ",卖四价=" + hqData.SellFourthPrice + ",卖四量=" + hqData.SellFourthVolume + ",卖五价=" + hqData.SellFivePrice + ",卖五量=" + hqData.SellFiveVolume);

            #region 市价卖一量撮合成交
            if (hqData.SellFirstVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFirstPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFirstVolume)
                    {
                        markVolume = hqData.SellFirstVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFirstVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFirstVolume = hqData.SellFirstVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖二量撮合成交
            if (hqData.SellSecondVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellSecondPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellSecondVolume)
                    {
                        markVolume = hqData.SellSecondVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellSecondVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellSecondVolume = hqData.SellSecondVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖三量撮合成交
            if (hqData.SellThirdVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellThirdPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellThirdVolume)
                    {
                        markVolume = hqData.SellThirdVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellThirdVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellThirdVolume = hqData.SellThirdVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖四量撮合成交
            if (hqData.SellFourthVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFourthPrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFourthVolume)
                    {
                        markVolume = hqData.SellFourthVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFourthVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFourthVolume = hqData.SellFourthVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 市价卖五量撮合成交
            if (hqData.SellFiveVolume > 0.00m)
            {
                if (orderEntity.OrderPrice >= hqData.SellFivePrice)
                {
                    if (orderEntity.OrderVolume > hqData.SellFiveVolume)
                    {
                        markVolume = hqData.SellFiveVolume;
                        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                        hqData.SellFiveVolume = 0.00m;
                        InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
                    }
                    else
                    {
                        markVolume = orderEntity.OrderVolume;
                        InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
                        orderEntity.OrderVolume = 0.00m;
                        hqData.SellFiveVolume = hqData.SellFiveVolume - markVolume;
                        return;
                    }
                }
            }
            #endregion

            #region 买市撮合，如果五档量撮合不完直接当作撤单
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的直接撤单. OrderVolume=" + orderEntity.OrderVolume);
                //orderEntity.MatchState = Types.MatchCenterState.other;
                CancelBufferEntity.InsertQueueItem(orderEntity);
            }

            #endregion
            #endregion

        }

        #endregion

        #endregion

        #region 6.撮合成交回报事件
        /// <summary>6.成交回报</summary>
        /// <param name="entity">实体</param>
        /// <param name="price">价格</param>
        /// <param name="volume">数量</param>
        private void InsertDealEntity(HKEntrustOrderInfo entity, decimal price, decimal volume)
        {
            //委托实体不能为空  //数量不能小于0//价格不能为负数
            if (entity == null || volume <= 0.00m || price <= 0.00m)
            {
                return;
            }

            HKDealBackEntity deal = new HKDealBackEntity();
            DateTime dealTime = DateTime.Now;
            string code = Guid.NewGuid().ToString();
            string showMesg = string.Format(GenerateInfo.CH_D002, "港股", entity.OrderNo, matchCode, dealTime, volume, entity.OrderType, price);

            deal.ChannelID = entity.BranchID; //通道号码
            deal.DealAmount = volume;//成交量
            // deal.DealMessage = showMesg;//成交单信息
            deal.ID = code;
            deal.DealPrice = price;//成交价格
            deal.DealTime = dealTime;//成交时间
            deal.DealType = true;//这个字段可以设计错误
            deal.OrderNo = entity.OrderNo;     //委托单号码
            if (HKDealOrderDal.Add(deal) == 1)
            {
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMesg);
                MatchCenterManager.Instance.matchCenterBackService.hkstockDealOrderCatheList.InsertQueueItem(deal);
                HKEntrustOrderDal.Update(entity.OrderNo, volume);
                LogHelper.WriteDebug(showMesg);
            }

        }
        #endregion

        #region  7.委托单不在交易价格范围之内撤单事件（限废单）
        /// <summary>
        ///  7.委托单不在交易价格范围之内撤单事件（限废单） 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InternalCancelOraderProcessEvent(object sender, QueueItemHandleEventArg<HKEntrustOrderInfo> e)
        {
            if (e.Item == null)
            {
                return;
            }
            HKEntrustOrderInfo model = e.Item;
            //委托单不能为空
            if (model == null || string.IsNullOrEmpty(model.BranchID))
            {
                return;
            }

            ResultDataEntity dataEntity = new ResultDataEntity();
            string code = Guid.NewGuid().ToString();
            dataEntity.Id = code;
            dataEntity.OrderNo = model.OrderNo;    //委托单号码

            //VTHKStockData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetHKStockData(matchCode);
            //decimal entrustPrice = model.OrderPrice;
            //if (data != null)
            //{
            //    entrustPrice = (decimal)data.Lasttrade;
            //}
            string messInfo = string.Format(GenerateInfo.CH_D007, model.OrderMessage);
            dataEntity.Message = messInfo;

            OperationContext context = null;
            if (model.BranchID != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.BranchID))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.BranchID];
                }
            }
            if (context == null)
            {
                LogHelper.WriteDebug(GenerateInfo.CH_D008);
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    smartPool.QueueWorkItem(callback.ProcessHKStockOrderRpt, dataEntity);

                    string showMesg = string.Format(GenerateInfo.CH_D001, messInfo, model.OrderNo, model.HKSecuritiesCode, DateTime.Now, model.OrderVolume, model.OrderType);
                    smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMesg);
                    LogHelper.WriteDebug(showMesg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                return;
            }
        }
        #endregion

        #region 8.限价盘撮合五档价格撮合不完内部自动内部撤单处理事件
        /// <summary>
        /// 限价盘撮合五档价格撮合不完内部自动内部撤单处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessCanceBussiness(object sender, QueueItemHandleEventArg<HKEntrustOrderInfo> e)
        {
            HKEntrustOrderInfo model = e.Item;
            if (model == null)
            {
                return;
            }
            CancelOrderEntity dataEntity = new CancelOrderEntity();
            string code = Guid.NewGuid().ToString();
            dataEntity.Id = code; //id
            dataEntity.OrderNo = model.OrderNo; //委托单号码
            dataEntity.IsSuccess = true;    //成功标志
            dataEntity.OrderVolume = model.OrderVolume;
            dataEntity.Message = "撤单成功!";
            dataEntity.ChannelNo = model.BranchID;   //通道号码
            OperationContext context = null;
            if (model.BranchID != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.BranchID))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.BranchID];
                }

            }
            if (context == null)
            {
                //如果通道不存在应象现货一样先缓存到队列中等待下次回推（即这里做故障恢复）
                TradePushBackImpl.Instanse.SaveHKCancelBack(dataEntity);
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    LogHelper.WriteDebug(" 撮合五档价格撮合不完委托量,内部自动撤单成功[" + "委托id=" + code + "委托代码=" + model.HKSecuritiesCode + ",撤单时间=" + DateTime.Now + ",委托单号码=" + model.OrderNo + ",撤单数量=" + model.OrderVolume + "]");
                    callback.CancelHKStockOrderRpt(dataEntity);
                    HKEntrustOrderDal.Update(model.OrderNo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                //如果通道异做故障恢复
                TradePushBackImpl.Instanse.SaveHKCancelBack(dataEntity);
                return;
            }
        }
        #endregion

        #region 9.卖买根据委托价格获取市场存量
        /// <summary>
        /// 买操作查找市场存量
        /// </summary>
        /// <param name="entrustPrice">委托价格</param>
        /// <param name="hqData">行情实体</param>
        /// <returns></returns>
        private decimal GetBuyMarkLeft(decimal entrustPrice, HqMarketEntity hqData)
        {
            //委托价格跟买一价比较
            if (entrustPrice == hqData.BuyFirstPrice)
            {
                return hqData.BuyFirstVolume;
            }
            if (entrustPrice == hqData.BuySecondPrice)
            {
                return hqData.BuySecondVolume;
            }
            //委托价格跟买三价比较
            if (entrustPrice == hqData.BuyThirdPrice)
            {
                return hqData.BuyThirdVolume;

            }
            //委托价格跟买四价比较
            if (entrustPrice == hqData.BuyFourthPrice)
            {
                return hqData.BuyFourthVolume;

            }
            //委托价格跟买五价比较
            if (entrustPrice == hqData.SellFivePrice)
            {
                return hqData.BuyFiveVolume;
            }
            return 0.00m;
        }

        /// <summary>
        /// 卖操作处理市场存量
        /// </summary>
        /// <param name="entrustPrice">委托价格</param>
        /// <param name="hqData">行情实体</param>
        /// <returns></returns>
        private decimal GetSellMarkLeft(decimal entrustPrice, HqMarketEntity hqData)
        {
            //委托价格判断
            if (entrustPrice == hqData.SellFirstPrice)
            {
                return hqData.SellFirstVolume;
            }
            //委托价格判断
            if (entrustPrice == hqData.SellSecondPrice)
            {
                return hqData.SellSecondVolume;
            }
            //委托价格判断
            if (entrustPrice == hqData.SellThirdPrice)
            {
                return hqData.SellThirdVolume;
            }
            if (entrustPrice == hqData.SellFourthPrice)
            {
                return hqData.SellFourthVolume;
            }
            if (entrustPrice == hqData.SellFivePrice)
            {
                return hqData.SellFiveVolume;

            }
            return 0.00m;
        }
        #endregion

        #region 清除卖买委托单数据
        /// <summary>
        /// 清除卖买委托单数据
        /// </summary>
        public void Clear()
        {
            //委托对象不能为空
            if (buyOrder != null)
            {
                buyOrder.Clear();
            }
            if (sellOrder != null)
            {
                sellOrder.Clear();
            }

        }
        #endregion
    }
}
