using System;
using System.Collections.Generic;
using System.ServiceModel;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.PushBack;
using MatchCenter.BLL.interfaces;
using MatchCenter.BLL.MatchData;
using MatchCenter.BLL.RealTime;
using MatchCenter.Entity;
using MatchCenter.DAL;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.BLL.Common;
using Amib.Threading;
using RealTime.Server.SModelData.HqData;

namespace MatchCenter.BLL
{
    /// <summary>
    /// 初始化撮合中心现货类
    /// Title: 初始化撮合中心股指现货类
    /// Desc.:本类包括对应的商品代码（合约代码）初始化，对每个商品代码（合约代码）进下委托的接收
    ///       下单后撤单操作的接收，对应的所有委托的撮合，所撮合的驱动来源于行情的刷新推动撮合。
    ///        撮合成功交易记录回推柜台。
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Desc.:修改变量定义
    /// Update by：李健华
    /// Update date:2009-10-19
    /// </summary>
    public class StockMatcher
    {
        #region 变量定义

        #region 缓冲区定义
        /// <summary>
        /// 现货市价委托撮合五档价格量撮合不完委托量自动撤单缓冲区
        /// </summary>
        private QueueBufferBase<StockDataOrderEntity> CancelBufferEntity;

        /// <summary>
        /// 现货下单委托缓冲区
        /// </summary>
        public QueueBufferBase<StockDataOrderEntity> EntityBuffer;

        /// <summary>
        /// 委托单不在交易价格范围之内撤单事件（限废单） 
        /// </summary>
        private QueueBufferBase<StockDataOrderEntity> internalCancelOrader;
        #endregion

        #region 委托单缓存区
        /// <summary>
        /// 存储【买限价】委托单
        /// </summary>
        private EntrustOrderData<StockDataOrderEntity> buyOrder;

        /// <summary>
        /// 存储【卖限价】委托单
        /// </summary>
        private EntrustOrderData<StockDataOrderEntity> sellOrder;

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
        /// 撮合商品代码
        /// </summary>
        private string matchCode;

        /// <summary>
        /// 现货撮合商品所属交易所ID
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
        static StockMatcher()
        {
            smartPool.MaxThreads = 80;
            smartPool.MinThreads = 20;
            smartPool.Start();
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="code"></param>
        public StockMatcher(string code)
        {
            matchCode = code;

            buyOrder = new EntrustOrderData<StockDataOrderEntity>(Types.TransactionDirection.Buying);
            sellOrder = new EntrustOrderData<StockDataOrderEntity>(Types.TransactionDirection.Selling);

            EntityBuffer = new QueueBufferBase<StockDataOrderEntity>();
            EntityBuffer.QueueItemProcessEvent += ProcessBussiness;

            //RealTimeData.QueueItemProcessEvent += ProcessMatch;

            //_realtimeService = RealtimeMarketService.GetRealtimeMark();
            //_realtimeService.StockRealtimeMarketChangeEvent += StockRealtimeMarketChangeEvent;
            //有先移除，是为了时间定时器执行每日开市重新初始化
            if (MatchCodeDictionary.stockMatchCodeDic.ContainsKey(code))
            {
                MatchCodeDictionary.stockMatchCodeDic[code] = null;
                MatchCodeDictionary.stockMatchCodeDic.Remove(code);
            }
            MatchCodeDictionary.stockMatchCodeDic.Add(matchCode, StockRealtimeMarketChangeEvent);

            CancelBufferEntity = new QueueBufferBase<StockDataOrderEntity>();
            CancelBufferEntity.QueueItemProcessEvent += ProcessCanceBussiness;
            internalCancelOrader = new QueueBufferBase<StockDataOrderEntity>();
            internalCancelOrader.QueueItemProcessEvent += InternalCancelOraderProcessEvent;

            //市场存量
            buyMarketVolume = new MarketVolumeData();
            sellMarketVolume = new MarketVolumeData();
        }
        #endregion

        #region 接收下单撤单
        /// <summary>接收下单委托单操作</summary>
        /// <param name="model">委托实体</param>
        public void AcceptOrder(StockDataOrderEntity model)
        {
            try
            {
            //LogHelper.WriteDebug("撮合单元运行接收委托方法：" );
            //实体不能为空
            if (model == null)
            {
                return;
            }
            if (model.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
            {
                // VTHqExData data = _realtimeService.GetStockHqData(matchCode);
                HqExData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetStockHqData(matchCode);

                if (data != null)
                {
                    model.OrderPrice = (decimal)data.HqData.Lasttrade;
                }
                else
                {
                    LogHelper.WriteError(matchCode, new Exception(GenerateInfo.CH_E007));
                }
            }
            // bool isPrice = JudgePrice(model.OrderPrice, (Types.TransactionDirection)model.TransactionDirection);
            bool isPrice = CostPriceCalculate.Instanse.XHComparePriceByMatchCode(model.OrderPrice, matchCode, (Types.TransactionDirection)model.TransactionDirection);
            if (isPrice == false)
            {
                internalCancelOrader.InsertQueueItem(model);
                return;
            }
            //缓冲区不能为空
            if (EntityBuffer != null)
            {
                model.ReachTime = DateTime.Now;
                StockDataOrderDataAccess.Add(model);
                EntityBuffer.InsertQueueItem(model);
                LogHelper.WriteDebug("接收委托单成功[委托单号:" + model.OrderNo + " 委托代码=" + model.StockCode + ",委托价格=" + model.OrderPrice + ",接收时间=" + DateTime.Now + "]");
            }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("接收现货下单委托单操作异常" + ex.Message, ex);
            }
        }

        /// <summary>接收提交撤销委托单操作</summary>
        /// <param name="model">委托实体</param>
        public int CancelOrder(CancelEntity model)
        {
            LogHelper.WriteDebug("[撮合单元运行撤销委托单方法]");
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
            var entry = StockDataOrderDataAccess.GetStockEntityByOrderNo(model.OldOrderNo.Trim());
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
            //-============
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
            #endregion

            if (cancel)
            {
                dataEntity.OrderVolume = cancount;
                dataEntity.IsSuccess = true;
                dataEntity.Message = "撤单成功";
            }
            else
            {
                dataEntity.IsSuccess = false;
                dataEntity.Message = "委托单不存在。";
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

                StockDataOrderDataAccess.Update(model.OldOrderNo);//更新数据库

                //上下文不能为空
                if (context == null)
                {
                    LogHelper.WriteError("CH-500:现货撤单时通道上下文为空,撤单回推缓存到回推队列中", new Exception(model.OldOrderNo));
                    //保存到缓存回推队列中
                    TradePushBackImpl.Instanse.SaveXHCancelBack(dataEntity);
                    return 1;
                }
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    //LogHelper.WriteDebug(" 撤单成功[" + "委托id=" + code + "委托代码=" + model.StockCode + ",撤单时间=" + DateTime.Now + ",委托单号码=" + model.OldOrderNo + ",撤单数量=" + cancount + "]");
                    string showMsg = string.Format(GenerateInfo.CH_D009, "现货", code, model.StockCode, DateTime.Now, model.OldOrderNo, cancount);
                    LogHelper.WriteDebug(showMsg);
                    smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMsg);

                    callback.CancelStockOrderRpt(dataEntity);//回报数据
                }
                else
                {
                    LogHelper.WriteError("CH-501:现货撤单时通道GetCallbackChannel为空,撤单回推缓存到回推队列中", new Exception(model.OldOrderNo));
                    //保存到缓存回推队列中
                    TradePushBackImpl.Instanse.SaveXHCancelBack(dataEntity);
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-100:现货撤单wcf服务通道异常", ex);
                TradePushBackImpl.Instanse.SaveXHCancelBack(dataEntity);
                return 1;
            }
        }

        #region 接收委托单缓冲数据处理数据
        /// <summary>
        /// 撮合中心缓冲数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessBussiness(object sender, QueueItemHandleEventArg<StockDataOrderEntity> e)
        {
            //委托对象不能为空
            if (e.Item == null)
            {
                return;
            }
            e.Item.ReachTime = DateTime.Now;
            if (e.Item.TransactionDirection == (int)Types.TransactionDirection.Buying)
            {
                if (buyOrder != null)
                {
                    // matchBuyStock.BufferStockEntity.InsertQueueItem(e.Item);
                    buyOrder.Add(e.Item);
                }
            }
            else if (e.Item.TransactionDirection == (int)Types.TransactionDirection.Selling)
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
        private void StockRealtimeMarketChangeEvent(object sender, StockHqDataChangeEventArg e)
        {
            if (e == null || e.HqData == null)
            {
                return;
            }
            if (e.HqData.CodeKey == matchCode)
            {
                //RealTimeData.InsertQueueItem(e.HqData);
                smartPool.QueueWorkItem(ProcessMatch, e.HqData);
            }
        }
        #endregion

        #region 2.行情驱动撮合证卷方法开始组装行情数据转换成内部执行实体

        /// <summary>2-1.行情驱动撮合证卷方法开始组装行情数据转换成内部执行实体</summary>
        /// <param name="hqData">行情实体</param>
        private void ProcessMatch(HqExData hqData)
        {
            if (hqData.CodeKey == matchCode)
            {
                if (TradeTimeManager.Instanse.IsMarchTime(bourseTypeID, DateTime.Now) == false)
                {
                    return;
                }
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
        private HqMarketEntity GetMarketEntity(HqExData hqExData)
        {
            if (hqExData == null)
            {
                return null;
            }
            var MarketEntity = new HqMarketEntity();
            //撮合中心行情买一价
            MarketEntity.BuyFirstPrice = (decimal)hqExData.HqData.Buyprice1;
            MarketEntity.BuyFirstVolume = (decimal)hqExData.HqData.Buyvol1;
            //撮合中心行情买二价
            MarketEntity.BuySecondPrice = (decimal)hqExData.HqData.Buyprice2;
            MarketEntity.BuySecondVolume = (decimal)hqExData.HqData.Buyvol2;
            //撮合中心行情买三价
            MarketEntity.BuyThirdPrice = (decimal)hqExData.HqData.Buyprice3;
            MarketEntity.BuyThirdVolume = (decimal)hqExData.HqData.Buyvol3;
            MarketEntity.BuyFourthPrice = (decimal)hqExData.HqData.Buyprice4;
            MarketEntity.BuyFourthVolume = (decimal)hqExData.HqData.Buyvol4;
            MarketEntity.BuyFivePrice = (decimal)hqExData.HqData.Buyprice5;
            MarketEntity.BuyFiveVolume = (decimal)hqExData.HqData.Buyvol5;
            MarketEntity.LastPrice = (decimal)hqExData.HqData.Lasttrade;
            MarketEntity.LastVolume = (decimal)hqExData.LastVolume;
            //MarketEntity.UpPrice = (decimal) hqExData.OriginalData.UpB;
            //MarketEntity.LowerPrice = (decimal) hqExData.OriginalData.LowB;
            var fTemp = 0.1;
            if (false == string.IsNullOrEmpty(hqExData.Name))
            {
                if (hqExData.Name.ToUpper().IndexOf("ST") > -1)
                {//ST股票涨跌幅限制：5％ 
                    fTemp = 0.05;
                }
            }
            //涨停价
            double upb = hqExData.YClose + hqExData.YClose * fTemp;
            hqExData.UpB = Convert.ToSingle(upb);

            //撮合中心跌停价 
            double lowb = hqExData.YClose - hqExData.YClose * fTemp;
            hqExData.LowB = Convert.ToSingle(lowb);
            MarketEntity.UpPrice = Math.Round((decimal)upb, 2);
            MarketEntity.LowerPrice = Math.Round((decimal)lowb, 2);

            //撮合中心行情卖一价
            MarketEntity.SellFirstPrice = (decimal)hqExData.HqData.Sellprice1;
            MarketEntity.SellFirstVolume = (decimal)hqExData.HqData.Sellvol1;
            //撮合中心行情卖二价
            MarketEntity.SellSecondPrice = (decimal)hqExData.HqData.Sellprice2;
            MarketEntity.SellSecondVolume = (decimal)hqExData.HqData.Sellvol2;
            MarketEntity.SellThirdPrice = (decimal)hqExData.HqData.Sellprice3;
            MarketEntity.SellThirdVolume = (decimal)hqExData.HqData.Sellvol3;
            MarketEntity.SellFourthPrice = (decimal)hqExData.HqData.Sellprice4;
            MarketEntity.SellFourthVolume = (decimal)hqExData.HqData.Sellvol4;
            MarketEntity.SellFivePrice = (decimal)hqExData.HqData.Sellprice5;
            MarketEntity.SellFiveVolume = (decimal)hqExData.HqData.Sellvol5;
            return MarketEntity;
        }
        #endregion

        #region 3.行情驱动开始撮合【买、卖】委托单
        /// <summary>3.行情驱动开始撮合【买、卖】委托单</summary>
        /// <param name="data">行情数据</param>
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

        #region 4.根据行情撮合卖【市价、限价】委托单

        /// <summary>4. 根据行情撮合卖【市价、限价】委托单</summary>
        /// <param name="haData">行情数据</param>
        private void MatchSell(HqMarketEntity haData)
        {
            #region update 2010-01-08
            //撮合中心撮合实体
            Dictionary<string, List<StockDataOrderEntity>> orderDic = sellOrder.GetAcceptEntitys();
            //撮合中心撮合实体不能为空
            if (orderDic == null || orderDic.Count <= 0)
            {
                return;
            }

            ////撮合中心撮合实体
            //List<StockDataOrderEntity> matchEntitys = sellOrder.GetAcceptEntitys();
            ////撮合中心撮合实体不能为空
            //if (Utils.IsNullOrEmpty(matchEntitys))
            //{
            //    return;
            //}
            //Dictionary<string, List<StockDataOrderEntity>> orderDic = new Dictionary<string, List<StockDataOrderEntity>>();

            //#region 组合每个交易员的本次行情的所有委托列表
            //foreach (var model in matchEntitys)
            //{
            //    if (orderDic.ContainsKey(model.SholderCode))
            //    {
            //        orderDic[model.SholderCode].Add(model);
            //    }
            //    else
            //    {
            //        List<StockDataOrderEntity> list = new List<StockDataOrderEntity>();
            //        list.Add(model);
            //        orderDic[model.SholderCode] = list;
            //    }
            //}
            //#endregion
            #endregion


            #region 分发每个人的委托列表(对于每个交易员本次行情都是相同)

            foreach (var sholderCodeModel in orderDic)
            {
                List<StockDataOrderEntity> orderList = sholderCodeModel.Value;
                if (Utils.IsNullOrEmpty(orderList))
                {
                    continue;
                }
                HqMarketEntity hqDataModel = new HqMarketEntity();
                hqDataModel = haData;
                //委托价格市场存量委托编号
                Dictionary<decimal, string> priceDic = new Dictionary<decimal, string>();
                foreach (StockDataOrderEntity item in orderList)
                {
                    if (item.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
                    {
                        MatchMarkPriceSell(item, hqDataModel);
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
                        MatchSell(item, hqDataModel, ref marketVolueOrderNo);

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

        /// <summary>4-1 根据行情【卖的市价】委托撮合五挡行情
        /// </summary>
        /// <param name="orderEntity">委托单</param>
        /// <param name="hqData">行情数据</param>
        private void MatchMarkPriceSell(StockDataOrderEntity orderEntity, HqMarketEntity hqData)
        {
            //委托单不能为空或者行情实体
            if (orderEntity == null || hqData == null)
            {
                return;
            }

            decimal markVolume;

            //LogHelper.WriteDebug("---->【现货】开始撮合卖市价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 市价最新成交价撮合成交 （市价不用与最新成交价撮合也即不用考虑市场存量）
            //if (hqData.LastVolume > 0.00m)
            //{
            //    //撮合中心成交数量比较
            //    if (orderEntity.OrderVolume > hqData.LastVolume)
            //    {
            //        markVolume = hqData.LastVolume;
            //        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
            //        hqData.LastVolume = 0.00m;
            //        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
            //    }
            //    else
            //    {
            //        markVolume = orderEntity.OrderVolume;
            //        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
            //        orderEntity.OrderVolume = 0.00m;
            //        hqData.LastVolume = hqData.LastVolume - markVolume;
            //        return;
            //    }
            //}
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.买一价=" + hqData.BuyFirstPrice + ",买一量=" + hqData.BuyFirstVolume + ",买二价=" + hqData.BuySecondPrice + ",买二量=" + hqData.BuySecondVolume + ",买三价=" + hqData.BuyThirdPrice + ",买三量=" + hqData.BuyThirdVolume + ",买四价=" + hqData.BuyFourthPrice + ",买四量=" + hqData.BuyFourthVolume + ",买五价=" + hqData.BuyFivePrice + ",买五量=" + hqData.BuyFiveVolume);

            #region 市价卖撮合买一量成交
            if (hqData.BuyFirstVolume > 0.00m)
            {
                //撮合中心数量比较
                if (orderEntity.OrderVolume > hqData.BuyFirstVolume)
                {
                    markVolume = hqData.BuyFirstVolume;
                    orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                    hqData.BuyFirstVolume = 0.00m;
                    InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                }
                else
                {
                    markVolume = orderEntity.OrderVolume;
                    InsertDealEntity(orderEntity, hqData.BuyFirstPrice, markVolume);
                    orderEntity.OrderVolume = 0.00m;
                    hqData.BuyFirstVolume = hqData.BuyFirstVolume - markVolume;
                    // ModifyMarketSellEntity(orderEntity.SholderCode, hqData);
                    return;
                }
            }
            #endregion

            #region 市价卖撮合买二量成交
            if (hqData.BuySecondVolume > 0.00m)
            {
                //撮合中心数量比较
                if (orderEntity.OrderVolume > hqData.BuySecondVolume)
                {
                    markVolume = hqData.BuySecondVolume;
                    orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                    hqData.BuySecondVolume = 0.00m;
                    InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                }
                else
                {
                    markVolume = orderEntity.OrderVolume;
                    InsertDealEntity(orderEntity, hqData.BuySecondPrice, markVolume);
                    orderEntity.OrderVolume = 0.00m;
                    hqData.BuySecondVolume = hqData.BuySecondVolume - markVolume;
                    //ModifyMarketSellEntity(orderEntity.SholderCode, hqData);
                    return;
                }
            }
            #endregion

            #region 市价卖撮合买三量成交
            if (hqData.BuyThirdVolume > 0.00m)
            {
                //撮合中心数量比较
                if (orderEntity.OrderVolume > hqData.BuyThirdVolume)
                {
                    markVolume = hqData.BuyThirdVolume;
                    orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                    hqData.BuyThirdVolume = 0.00m;
                    InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                }
                else
                {
                    markVolume = orderEntity.OrderVolume;
                    InsertDealEntity(orderEntity, hqData.BuyThirdPrice, markVolume);
                    orderEntity.OrderVolume = 0.00m;
                    hqData.BuyThirdVolume = hqData.BuyThirdVolume - markVolume;
                    // ModifyMarketSellEntity(orderEntity.SholderCode, hqData);
                    return;
                }
            }
            #endregion

            #region 市价卖撮合买四量成交
            if (hqData.BuyFourthVolume > 0.00m)
            {
                //撮合中心数量比较
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
                    orderEntity.OrderVolume = 0.00m;
                    hqData.BuyFourthVolume = hqData.BuyFourthVolume - markVolume;
                    //ModifyMarketSellEntity(orderEntity.SholderCode, hqData);
                    return;
                }
            }
            #endregion

            #region 市价卖撮合买五量成交
            if (hqData.BuyFiveVolume > 0.00m)
            {
                //撮合中心数量比较
                if (orderEntity.OrderVolume > hqData.BuyFiveVolume)
                {
                    markVolume = hqData.BuyFiveVolume;
                    orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
                    hqData.BuyFiveVolume = 0.00m;
                    InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                }
                else
                {
                    markVolume = orderEntity.OrderVolume;
                    InsertDealEntity(orderEntity, hqData.BuyFivePrice, markVolume);
                    orderEntity.OrderVolume = 0.00m;
                    hqData.BuyFiveVolume = hqData.BuyFiveVolume - markVolume;
                    //ModifyMarketSellEntity(orderEntity.SholderCode, hqData);
                    //if (orderEntity.OrderVolume > 0.00m)
                    //{
                    //    CancelBufferEntity.InsertQueueItem(orderEntity);
                    //    return;
                    //}
                    return;
                }
            }
            #endregion

            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的直接撤单.OrderVolume=" + orderEntity.OrderVolume);

                CancelBufferEntity.InsertQueueItem(orderEntity);
                return;
            }
        }

        /// <summary>4-2.根据行情【卖的限价】委托撮合五档行情撮合成交
        /// </summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情数据</param>
        /// <param name="marketVolueOrderNo">市场存量关联编号</param>
        private void MatchSell(StockDataOrderEntity orderEntity, HqMarketEntity hqData, ref string marketVolueOrderNo)
        {
            //委托实体不能为空
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            decimal markVolume;

            //LogHelper.WriteDebug("---->【现货】开始撮合卖限价委托");
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
                        string marketKey = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + orderEntity.MarketVolumeNo;
                        decimal currentMarketVolume = 0;
                        if (string.IsNullOrEmpty(orderEntity.MarketVolumeNo))
                        {
                            currentMarketVolume = 0;
                        }
                        else
                        {
                            currentMarketVolume = buyMarketVolume.GetMarketVolume(marketKey);
                        }

                        decimal hqLastVolume = hqData.LastVolume;
                        //if (orderEntity.MarkLeft >= hqData.LastVolume)
                        //{
                        if (currentMarketVolume >= hqLastVolume)
                        {
                            hqData.LastVolume = 0.00m;
                            // orderEntity.MarkLeft = orderEntity.MarkLeft - hqData.LastVolume;
                            orderEntity.MarkLeft = currentMarketVolume - hqLastVolume;
                            sellMarketVolume.ModifyMarketVolume(marketKey, hqLastVolume);
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
                            sellMarketVolume.ModifyMarketVolume(marketKey, currentMarketVolume);

                            orderEntity.MarkLeft = 0.00m;
                        }
                        StockDataOrderDataAccess.Update(orderEntity.OrderNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                        StockDataOrderDataAccess.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);//更新关联的委托市场存量
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
                    StockDataOrderDataAccess.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
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

        #endregion

        #region 5.根据行情撮合买【市价、限价】委托单

        /// <summary>5. 根据行情撮合买【市价、限价】委托单</summary>
        /// <param name="hqData">行情数据</param>
        private void MatchBuy(HqMarketEntity hqData)
        {
            #region update 2010-01-08
            //撮合中心撮合实体
            Dictionary<string, List<StockDataOrderEntity>> orderDic = buyOrder.GetAcceptEntitys();
            //撮合中心撮合实体不能为空
            if (orderDic == null || orderDic.Count <= 0)
            {
                return;
            }


            //List<StockDataOrderEntity> matchBuyList = buyOrder.GetAcceptEntitys();
            //if (Utils.IsNullOrEmpty(matchBuyList))
            //{
            //    return;
            //}
            //Dictionary<string, List<StockDataOrderEntity>> orderDic = new Dictionary<string, List<StockDataOrderEntity>>();

            //#region 组合每个交易员的本次行情的所有委托列表
            //foreach (var model in matchBuyList)
            //{
            //    if (orderDic.ContainsKey(model.SholderCode))
            //    {
            //        orderDic[model.SholderCode].Add(model);
            //    }
            //    else
            //    {
            //        List<StockDataOrderEntity> list = new List<StockDataOrderEntity>();
            //        list.Add(model);
            //        orderDic[model.SholderCode] = list;
            //    }
            //}
            //#endregion
            #endregion

            #region 分发每个人的委托列表(对于每个交易员本次行情都是相同)

            foreach (var sholderCodeModel in orderDic)
            {
                List<StockDataOrderEntity> orderList = sholderCodeModel.Value;
                if (Utils.IsNullOrEmpty(orderList))
                {
                    continue;
                }

                HqMarketEntity hqDataModel = new HqMarketEntity();
                hqDataModel = hqData;
                //委托价格市场存量委托编号
                Dictionary<decimal, string> priceDic = new Dictionary<decimal, string>();
                foreach (StockDataOrderEntity item in orderList)
                {
                    if (item.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
                    {
                        MatchMarkPriceBuy(item, hqDataModel);
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
                        MatchBuy(item, hqDataModel, ref marketVolueOrderNo);

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

        /// <summary>5-1 根据行情【买的市价】委托撮合五挡行情
        /// </summary>
        /// <param name="orderEntity">委托单</param>
        /// <param name="hqData">行情数据</param>
        private void MatchMarkPriceBuy(StockDataOrderEntity orderEntity, HqMarketEntity hqData)
        {
            if (orderEntity == null || hqData == null)
            {
                return;
            }

            decimal markVolume;

            //LogHelper.WriteDebug("---->【现货】开始撮合买市价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 市价最新成交价撮合买成交 （市价不用与最新成交价撮合也即不用考虑市场存量）
            //if (hqData.LastVolume > 0.00m)
            //{
            //    if (orderEntity.OrderVolume > hqData.LastVolume)
            //    {
            //        markVolume = hqData.LastVolume;
            //        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
            //        hqData.LastVolume = 0.00m;
            //        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
            //    }
            //    else
            //    {
            //        markVolume = orderEntity.OrderVolume;
            //        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
            //        orderEntity.OrderVolume = 0.00m;
            //        hqData.LastVolume = hqData.LastVolume - markVolume;
            //        return;
            //    }
            //}
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.卖一价=" + hqData.SellFirstPrice + ",卖一量=" + hqData.SellFirstVolume + ",卖二价=" + hqData.SellSecondPrice + ",卖二量=" + hqData.SellSecondVolume + ",卖三价=" + hqData.SellThirdPrice + ",卖三量=" + hqData.SellThirdVolume + ",卖四价=" + hqData.SellFourthPrice + ",卖四量=" + hqData.SellFourthVolume + ",卖五价=" + hqData.SellFivePrice + ",卖五量=" + hqData.SellFiveVolume);

            #region 市价卖一量撮合成交
            if (hqData.SellFirstVolume > 0.00m)
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
            #endregion

            #region 市价卖二量撮合成交
            if (hqData.SellSecondVolume > 0.00m)
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
            #endregion

            #region 市价卖三量撮合成交
            if (hqData.SellThirdVolume > 0.00m)
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
            #endregion

            #region 市价卖四量撮合成交
            if (hqData.SellFourthVolume > 0.00m)
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
            #endregion

            #region 市价卖五量撮合成交
            if (hqData.SellFiveVolume > 0.00m)
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
            #endregion

            #region 买市撮合，如果五档量撮合不完直接当作撤单
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的直接撤单.OrderVolume=" + orderEntity.OrderVolume);

                CancelBufferEntity.InsertQueueItem(orderEntity);
            }
            #endregion
        }

        /// <summary>5-2.根据行情【买的限价】委托撮合五档行情撮合成交
        /// </summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情数据</param>
        /// <param name="marketVolueOrderNo">市场存量委托编号</param>
        private void MatchBuy(StockDataOrderEntity orderEntity, HqMarketEntity hqData, ref string marketVolueOrderNo)
        {
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            decimal markVolume;

            //LogHelper.WriteDebug("---->【现货】开始撮合买限价委托");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

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
                            volume = hqLastVolume - currentMarketVolume;//撮合市场存量再与委托量撮合
                            //撮合市场存量后剩余的最后成交量再撮合委托量
                            if (volume >= orderEntity.OrderVolume)//剩余量大于委托量
                            {
                                markVolume = orderEntity.OrderVolume;
                                orderEntity.OrderVolume = 0.00m;
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
                        StockDataOrderDataAccess.Update(orderEntity.OrderNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);//更新本委托的市场存量
                        StockDataOrderDataAccess.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);//更新关联的委托市场存量

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

            #region 委托单还有没有撮合完的再添加到委托撮合列表中等待行情再撮合
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的再添加到委托撮合列表中等待行情再撮合.OrderVolume=" + orderEntity.OrderVolume);

                if (Types.MatchCenterState.First == orderEntity.MatchState)
                {
                    //用户编号+价格+委托编号
                    string fistKey = orderEntity.SholderCode + "@" + orderEntity.OrderPrice + "@" + marketVolueOrderNo;
                    //获取市场存量
                    decimal marketVolume = 0;
                    if (marketVolueOrderNo != orderEntity.OrderNo)
                    {
                        marketVolume = buyMarketVolume.GetMarketVolume(fistKey);
                    }
                    else
                    {
                        marketVolume = GetBuyMarkLeft(orderEntity.OrderPrice, hqData);
                    }

                    orderEntity.MarkLeft = marketVolume;
                    orderEntity.MarketVolumeNo = marketVolueOrderNo;

                    //把市场存量记录下来
                    buyMarketVolume.AddMarketVolume(fistKey, marketVolume);
                    //更新数据库表中的数据市场存量和记录市场存量的关联并更新撮合状态
                    StockDataOrderDataAccess.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
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
        }

        #endregion

        #region 6.撮合成交回报事件
        /// <summary>6.成交回报</summary>
        /// <param name="entity">实体</param>
        /// <param name="price">价格</param>
        /// <param name="volume">数量</param>
        private void InsertDealEntity(StockDataOrderEntity entity, decimal price, decimal volume)
        {
            //委托实体不能为空
            if (entity == null)
            {
                return;
            }
            //数量不能小于0
            if (volume <= 0.00m)
            {
                return;
            }
            //价格不能为负数
            if (price <= 0.00m)
            {
                return;
            }
            string code = Guid.NewGuid().ToString();
            var deal = new StockDealEntity();
            //对象标志
            deal.Id = code;
            //通道号码
            deal.ChannelNo = entity.ChannelNo;
            //委托单号码
            deal.OrderNo = entity.OrderNo;
            //成交时间
            deal.DealTime = DateTime.Now;
            deal.DealAmount = volume;
            deal.ClassType = BreedClassTypeEnum.Stock;
            deal.DealPrice = price;
            if (DealOrderDataAccess.Add(deal) == 1)
            {
                string showMesg = string.Format(GenerateInfo.CH_D002, "现货", deal.OrderNo, matchCode, deal.DealTime,
                 deal.DealAmount, entity.IsMarketPrice == 0 ? "市价" : "限价", deal.DealPrice);
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMesg);

                MatchCenterManager.Instance.matchCenterBackService.stockDealOrderCacheList.InsertQueueItem(deal);
                StockDataOrderDataAccess.Update(entity.OrderNo, volume);

                LogHelper.WriteDebug(showMesg);
                //LogHelper.WriteDebug(" 成交回报[" + "委托id=" + code + ", 委托代码=" + entity.StockCode + " ,回报时间=" + DateTime.Now +
                //                   ",委托单号码=" + entity.OldOrderNo + ", 成交价格=" + price + ",成交数量=" + volume + "]");
            }

        }
        #endregion
        #endregion

        #region 7. 委托单不在交易价格范围之内撤单事件（限废单）
        /// <summary>
        ///  委托单不在交易价格范围之内撤单事件（限废单） 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InternalCancelOraderProcessEvent(object sender, QueueItemHandleEventArg<StockDataOrderEntity> e)
        {
            if (e.Item == null)
            {
                return;
            }
            StockDataOrderEntity model = e.Item;
            //委托单不能为空
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            string code = Guid.NewGuid().ToString();
            var dataEntity = new ResultDataEntity();
            dataEntity.Id = code;
            dataEntity.OrderNo = model.OrderNo;    //委托单号码

            HqExData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetStockHqData(matchCode);
            decimal entrustPrice = model.OrderPrice;
            if (data != null)
            {
                entrustPrice = (decimal)data.HqData.Lasttrade;
            }
            string messInfo = string.Format(GenerateInfo.CH_D007, entrustPrice);
            dataEntity.Message = messInfo;

            OperationContext context = null;
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelNo))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
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
                    smartPool.QueueWorkItem(callback.ProcessStockOrderRpt, dataEntity);

                    string showMesg = string.Format(GenerateInfo.CH_D001, messInfo, model.OrderNo, model.StockCode, DateTime.Now, model.OrderVolume, model.IsMarketPrice == 0 ? "市价" : "限价");
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

        #region 8.市价撮合五档价格撮合不完内部自动撤单处理事件
        /// <summary>
        /// 市价撮合五档价格撮合不完内部自动撤单处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessCanceBussiness(object sender, QueueItemHandleEventArg<StockDataOrderEntity> e)
        {
            StockDataOrderEntity model = e.Item;
            if (model == null)
            {
                return;
            }
            string code = Guid.NewGuid().ToString();
            var dataEntity = new CancelOrderEntity();
            //id
            dataEntity.Id = code;
            //委托单号码
            dataEntity.OrderNo = model.OrderNo;
            //成功标志
            dataEntity.IsSuccess = true;
            dataEntity.OrderVolume = model.OrderVolume;
            dataEntity.Message = "撤单成功!";
            //通道号码
            dataEntity.ChannelNo = model.ChannelNo;
            OperationContext context = null;
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelNo))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }

            }
            if (context == null)
            {
                TradePushBackImpl.Instanse.SaveXHCancelBack(dataEntity);
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    LogHelper.WriteDebug(" 撮合五档价格撮合不完委托量,内部自动撤单成功[" + "委托id=" + code + "委托代码=" + model.StockCode + ",撤单时间=" + DateTime.Now + ",委托单号码=" + model.OrderNo + ",撤单数量=" + model.OrderVolume + "]");
                    callback.CancelStockOrderRpt(dataEntity);
                    StockDataOrderDataAccess.Update(model.OrderNo);
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteError("CH-0001:wcf服务通道阻塞", ex);
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                TradePushBackImpl.Instanse.SaveXHCancelBack(dataEntity);
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