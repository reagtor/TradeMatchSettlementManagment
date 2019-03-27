//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Amib.Threading;
//using MatchCenter.Entity;
//using MatchCenter.BLL.MatchData;
//using MatchCenter.BLL.RealTime;
//using CommonUtility;
//using CommonObject;
//using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;

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
using MatchCenter.DAL;
using MatchCenter.Entity;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.BLL.MatchRules;
using Amib.Threading;
using MatchCenter.BLL.Common;
using System.Threading;
using RealTime.Server.SModelData.HqData;

namespace MatchCenter.BLL.Match
{
    /// <summary>
    /// Title: 初始化撮合中心商品期货类
    /// Desc.:本类包括对应的商品代码（合约代码）初始化，对每个商品代码（合约代码）进下委托的接收
    ///       下单后撤单操作的接收，对应的所有委托的撮合，所撮合的驱动来源于行情的刷新推动撮合。
    ///        撮合成功交易记录回推柜台。
    /// Create BY：李健华
    /// Create Date：2010-01-21
    /// </summary>
    public class SPQHMatcher
    {
        #region 变量定义

        /// <summary>
        /// 线程池
        /// </summary>
        private static SmartThreadPool smartPool = new SmartThreadPool();
        #region 缓冲区定义
        /// <summary>
        /// 撤单存储区
        /// </summary>
        private QueueBufferBase<CommoditiesDataOrderEntity> CancelBufferEntity;
        /// <summary>
        /// 缓冲区
        /// </summary>
        public QueueBufferBase<CommoditiesDataOrderEntity> EntityBuffer;

        /// <summary>
        ///  委托单不在交易价格范围，发生熔断 （限废单）内部撤单事件
        /// </summary>
        private QueueBufferBase<CommoditiesDataOrderEntity> internalCancelOrader;

        #endregion

        #region 委托单缓存区

        /// <summary>
        /// 存储【买限（市）价】委托单
        /// </summary>
        private EntrustOrderData<CommoditiesDataOrderEntity> buyOrder;

        /// <summary>
        /// 存储【卖限（市）价】委托单
        /// </summary>
        private EntrustOrderData<CommoditiesDataOrderEntity> sellOrder;


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
        /// 买行情缓冲区
        /// </summary>
        private RealTimeDataCache realTimeDataBuy;
        /// <summary>
        /// 卖行情缓冲区
        /// </summary>
        private RealTimeDataCache realTimeDataSell;

        /// <summary>
        /// 代码
        /// </summary>
        private string matchCode;


        /// <summary>
        /// 交易所ID
        /// </summary>
        public int bourseTypeID = 0;

        #endregion

        #region 构造函数
        /// <summary>
        /// 静态构造函数 初始化线程池
        /// </summary>
        static SPQHMatcher()
        {
            smartPool.MaxThreads = 80;
            smartPool.MinThreads = 20;
            smartPool.Start();
        }

        /// <summary>
        /// 构造函数 初始相关回推成交和撤单等相关参数和事件
        /// </summary>
        /// <param name="code">代码</param>
        public SPQHMatcher(string code)
        {

            matchCode = code;

            realTimeDataBuy = new RealTimeDataCache();
            realTimeDataBuy.bufferRealTime.QueueItemProcessEvent += new EventHandler<QueueItemHandleEventArg<FutureMarketEntity>>(bufferRealTime_QueueItemProcessEventBuy);
            realTimeDataSell = new RealTimeDataCache();
            realTimeDataSell.bufferRealTime.QueueItemProcessEvent += new EventHandler<QueueItemHandleEventArg<FutureMarketEntity>>(bufferRealTime_QueueItemProcessEventSell);

            buyOrder = new EntrustOrderData<CommoditiesDataOrderEntity>(Types.TransactionDirection.Buying);

            sellOrder = new EntrustOrderData<CommoditiesDataOrderEntity>(Types.TransactionDirection.Selling);

            EntityBuffer = new QueueBufferBase<CommoditiesDataOrderEntity>();
            EntityBuffer.QueueItemProcessEvent += ProcessBussiness;

            // RealTimeData.QueueItemProcessEvent += ProcessMatch;
            //_realtimeService = RealtimeMarketService.GetRealtimeMark();
            //_realtimeService.FutRealtimeMarketChangeEvent += FutRealtimeMarketChangeEvent;
            //有先移除，是为了时间定时器执行每日开市重新初始化
            if (MatchCodeDictionary.spQHMatchCodeDic.ContainsKey(code))
            {
                MatchCodeDictionary.spQHMatchCodeDic[code] = null;
                MatchCodeDictionary.spQHMatchCodeDic.Remove(code);
            }

            MatchCodeDictionary.spQHMatchCodeDic.Add(matchCode, SPQHRealtimeMarketChangeEvent);


            CancelBufferEntity = new QueueBufferBase<CommoditiesDataOrderEntity>();
            CancelBufferEntity.QueueItemProcessEvent += ProcessCanceBussiness;
            internalCancelOrader = new QueueBufferBase<CommoditiesDataOrderEntity>();
            internalCancelOrader.QueueItemProcessEvent += InternalCancelOraderProcessEvent;

            //市场存量
            buyMarketVolume = new MarketVolumeData();
            sellMarketVolume = new MarketVolumeData();
        }
        /// <summary>
        /// 行情驱动撮合限价买委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bufferRealTime_QueueItemProcessEventBuy(object sender, QueueItemHandleEventArg<FutureMarketEntity> e)
        {
            // MatchBuy(e.Item);
            smartPool.QueueWorkItem(MatchBuy, e.Item);
        }
        /// <summary>
        /// 行情驱动撮合限价卖委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bufferRealTime_QueueItemProcessEventSell(object sender, QueueItemHandleEventArg<FutureMarketEntity> e)
        {
            //MatchSell(e.Item);
            smartPool.QueueWorkItem(MatchSell, e.Item);
        }
        #endregion

        #region 委托单接收和撤单接收
        /// <summary>
        /// 接收委托单 
        /// </summary>
        /// <param name="model">委托单</param>
        public void AcceptOrder(CommoditiesDataOrderEntity model)
        {
            try
            {

                //撮合中心委托单不能为空
                if (model == null)
                {
                    return;
                }

                #region 商品期货无市价委托
                if (model.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
                {
                    //////撮合中心行情数据
                    //VTFutData data = RealtimeMarketService.GetRealTimeFutureDataByContractCode(matchCode);
                    //if (data != null)
                    //{
                    //    model.OrderPrice = (decimal)data.OriginalData.Lasttrade;
                    //}
                    //else
                    //{
                    //    LogHelper.WriteError(matchCode, new Exception(GenerateInfo.CH_E007));
                    //}
                    //if (EntityBuffer != null)
                    //{
                    //    //撮合中心委托时间
                    //    model.ReachTime = System.DateTime.Now;
                    //    CommoditiesDataOrderAccess.Add(model);
                    //    EntityBuffer.InsertQueueItem(model);

                    //    LogHelper.WriteDebug("接收委托单成功[委托单号:" + model.OrderNo + " 委托代码=" + model.StockCode + ",委托价格=" + model.OrderPrice + ",接收时间=" + DateTime.Now + "]");
                    //}
                    return;
                }
                #endregion

                //FutureFuseEntity fuseEntity = null;
                //int fuse = 0;
                //bool isPrice = CostPriceCalculate.Instanse.QHComparePriceByMatchCodeReturnIsFuse(model.OrderPrice, matchCode, ref fuse);
                //if (isPrice == false)
                //{
                //    fuseEntity = new FutureFuseEntity();
                //    fuseEntity.FuseMark = fuse;
                //    fuseEntity.Model = model;
                //    internalCancelOrader.InsertQueueItem(fuseEntity);
                //    LogHelper.WriteDebug("接收期货委托单失败,价格判断失败[" + " 委托代码=" + model.StockCode + ",委托价格=" + model.OrderPrice + ",接收时间=" + DateTime.Now + "]");
                //    return;
                //}

                bool isPrice = CostPriceCalculate.Instanse.CommoditiesComparePriceByMatchCode(model.OrderPrice, matchCode);
                if (!isPrice)
                {
                    internalCancelOrader.InsertQueueItem(model);
                    LogHelper.WriteDebug("接收期货委托单失败,价格判断失败[" + " 委托代码=" + model.StockCode + ",委托价格=" + model.OrderPrice + ",接收时间=" + DateTime.Now + "]");
                    return;
                }


                //缓冲区不能为空
                if (EntityBuffer != null)
                {
                    model.ReachTime = System.DateTime.Now;
                    CommoditiesDataOrderAccess.Add(model);
                    EntityBuffer.InsertQueueItem(model);
                    LogHelper.WriteDebug("接收期货委托单成功[" + " 委托代码=" + model.StockCode + ",委托价格=" + model.OrderPrice + ",接收时间=" + DateTime.Now + "]");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("接收商品期货下单委托单操作异常" + ex.Message, ex);
            }
        }

        /// <summary>接收提交撤销委托单操作,并撤单和撤单回报</summary>
        /// <param name="model">委托实体</param>
        public int CancelOrder(CancelEntity model)
        {
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

            //撮合中心委托单号
            if (string.IsNullOrEmpty(model.OldOrderNo))
            {
                return 3;
            }
            decimal cancount = 0.00m;
            model.CancelCount = model.CancelCount + 1;

            #region  update ===2010-01-08
            string sholdercode = "";
            var entry = CommoditiesDataOrderAccess.GetFutureEntityByOrderNo(model.OldOrderNo.Trim());
            if (entry != null)
            {
                sholdercode = entry.SholderCode;
            }
            if (!string.IsNullOrEmpty(sholdercode))
            {
                //委托存储区不能为空
                if (buyOrder != null)
                {
                    cancel = buyOrder.Remove(model.OldOrderNo, sholdercode, ref cancount);
                }

                if (sellOrder != null && cancel == false)
                {
                    cancel = sellOrder.Remove(model.OldOrderNo, sholdercode, ref cancount);
                }
                ////因为期货分为市价和限价队列所以这里要多做操作，这个队列日后修改成现货相同只有卖买队列即可
                ////委托存储区不能为空
                //if (buyMarketPriceOrder != null && cancel == false)
                //{
                //    cancel = buyMarketPriceOrder.Remove(model.OldOrderNo, sholdercode, ref cancount);
                //}

                //if (sellMarketPriceOrder != null && cancel == false)
                //{
                //    cancel = sellMarketPriceOrder.Remove(model.OldOrderNo, sholdercode, ref cancount);
                //}
            }
            //如果撤单不成功
            if (!cancel && model.CancelCount >= RulesDefaultValue.DefaultCancelFailCount / 2)
            {
                if (entry != null && entry.OrderVolume > 0)
                {
                    LogHelper.WriteError("CH-0002:期货撤单失败", new Exception("直接从数据库中撤单" + model.OldOrderNo.Trim()));
                    cancel = true;
                    cancount = entry.OrderVolume;
                }
            }
            //============
            //委托存储区不能为空
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
            //撮合中心撤单成功
            if (cancel)
            {
                dataEntity.OrderVolume = cancount;
                dataEntity.IsSuccess = true;
                dataEntity.Message = "撤单成功。";
            }
            else
            {
                dataEntity.IsSuccess = false;
                dataEntity.Message = "委托单不存在。";
            }

            OperationContext context = null;
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelNo))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }
            }

            try
            {
                if (cancount == 0.00m)
                {
                    return 0;
                }
                dataEntity.OrderVolume = cancount;

                CommoditiesDataOrderAccess.Update(model.OldOrderNo);//更新数据库
                //上下文不能为空
                if (context == null)
                {
                    LogHelper.WriteError("CH-500:期货撤单时通道上下文为空,撤单回推缓存到回推队列中", new Exception(model.OldOrderNo));
                    //保存到缓存回推队列中 还没有实现
                    TradePushBackImpl.Instanse.SaveCFCancelBack(dataEntity);
                    return 1;
                }
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    string showMsg = string.Format(GenerateInfo.CH_D009, "期货", model.OldOrderNo, model.StockCode, DateTime.Now, model.OldOrderNo, cancount);
                    smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMsg);

                    //smartPool.QueueWorkItem(callback.CancelStockIndexFuturesOrderRpt, dataEntity);
                    //不能用异常，要不然捕捉不到异常
                    callback.CancelCommoditiesOrderRpt(dataEntity);
                }
                else
                {
                    //还没有实现
                    LogHelper.WriteError("CH-501:现货撤单时通道GetCallbackChannel为空,撤单回推缓存到回推队列中", new Exception(model.OldOrderNo));
                    //保存到缓存回推队列中
                    TradePushBackImpl.Instanse.SaveCFCancelBack(dataEntity);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                //还没有实现
                TradePushBackImpl.Instanse.SaveCFCancelBack(dataEntity);
                return 0;
            }
            return 1;
        }

        #region 接收委托单存储撮合买卖委托单缓冲数据
        /// <summary>
        /// 存储撮合买卖委托单缓冲数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessBussiness(object sender, QueueItemHandleEventArg<CommoditiesDataOrderEntity> e)
        {
            if (e.Item == null)
            {
                return;
            }
            e.Item.ReachTime = DateTime.Now;
            if (e.Item.TransactionDirection == (int)Types.TransactionDirection.Buying)
            {
                //if (e.Item.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
                //{
                //    if (buyMarketPriceOrder != null)
                //    {
                //        buyMarketPriceOrder.Add(e.Item);
                //    }
                //}
                //else
                //{
                if (buyOrder != null)
                {
                    //buyOrder.BufferStockEntity.InsertQueueItem(e.Item);
                    buyOrder.Add(e.Item);
                }
                //}
            }
            else if (e.Item.TransactionDirection == (int)Types.TransactionDirection.Selling)
            {
                //if (e.Item.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
                //{
                //    if (sellMarketPriceOrder != null)
                //    {
                //        sellMarketPriceOrder.Add(e.Item);
                //    }
                //}
                //else
                //{
                if (sellOrder != null)
                {
                    //sellOrder.BufferStockEntity.InsertQueueItem(e.Item);
                    sellOrder.Add(e.Item);
                }
                //}
            }
        }
        #endregion
        #endregion

        #region  行情驱动撮合方法

        #region 1.行情调度驱动撮合事件
        /// <summary>
        /// 1.行情调度驱动撮合事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SPQHRealtimeMarketChangeEvent(object sender, MercantileFutDataChangeEventArg e)
        {
            if (e == null || e.HqData == null)
            {
                return;
            }
            if (e.HqData.CodeKey == matchCode)
            {
                //string nowTime = DateTime.Now.ToString();
                //var data = e.HqData.OriginalData;
                //string msg = data.CodeKey + "行情到达：{0}  行情时间：{1}\r\n成交价：{2}\r\n";
                //msg += "卖一：{3}--{4}   卖五：{5}--{6}\r\n  ";
                //msg += "买一：{7}--{8}   买五：{9}--{10}\r\n  ";
                //string wrmsg = string.Format(msg, nowTime, data.Time, data.Lasttrade, data.Sellprice1, data.Sellvol1, data.Sellprice5, data.Sellvol5
                //                          , data.Buyprice1, data.Buyvol1, data.Buyprice5, data.Buyvol5);
                //LogHelper.WriteDebug(wrmsg);
                smartPool.QueueWorkItem(ProcessMatch, e.HqData);
            }
        }
        #endregion

        #region 2.行情驱动撮合证卷方法开始组装行情数据转换成内部执行实体

        /// <summary>2-1.行情驱动撮合证卷方法开始组装行情数据转换成内部执行实体</summary>
        /// <param name="hqData">行情实体</param>
        private void ProcessMatch(MerFutData hqData)
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
                FuseManager.Instanse.ModifyState(matchCode);
                FutureMarketEntity GlobalMarketEntity = GetMarketEntity(hqData);
                MatchStart(GlobalMarketEntity);
            }
        }

        /// <summary> 2-2.组装行情数据转换成内部执行实体</summary>
        /// <param name="hqExData">撮合中心实体</param>
        /// <returns></returns>
        private FutureMarketEntity GetMarketEntity(MerFutData hqExData)
        {
            if (hqExData == null)
            {
                return null;
            }
            var MarketEntity = new FutureMarketEntity();
            MarketEntity.CodeKey = hqExData.CodeKey;
            //撮合中心买一价
            MarketEntity.BuyFirstPrice = (decimal)hqExData.Buyprice1;
            //撮合中心买一数量
            MarketEntity.BuyFirstVolume = (decimal)hqExData.Buyvol1;
            //撮合中心买二价
            MarketEntity.BuySecondPrice = (decimal)hqExData.Buyprice2;
            //撮合中心买二数量
            MarketEntity.BuySecondVolume = (decimal)hqExData.Buyvol2;
            MarketEntity.BuyThirdPrice = (decimal)hqExData.Buyprice3;
            MarketEntity.BuyThirdVolume = (decimal)hqExData.Buyvol3;
            MarketEntity.BuyFourthPrice = (decimal)hqExData.Buyprice4;
            MarketEntity.BuyFourthVolume = (decimal)hqExData.Buyvol4;
            MarketEntity.BuyFivePrice = (decimal)hqExData.Buyprice5;
            MarketEntity.BuyFiveVolume = (decimal)hqExData.Buyvol5;
            //撮合中心成交价格
            MarketEntity.LastPrice = (decimal)hqExData.Lasttrade;
            //撮合中心成交数量
            MarketEntity.LastVolume = (decimal)hqExData.PTrans;
            MarketEntity.UpPrice = (decimal)hqExData.UpperLimit;//.HighPrice;//.UpB;
            MarketEntity.LowerPrice = (decimal)hqExData.LowerLimit;//.LowPrice;//.LowB;
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
            MarketEntity.HQReachTime = Convert.ToDateTime(hqExData.Time);
            return MarketEntity;
        }

        #endregion

        #region 3.行情驱动开始撮合【买、卖】委托单
        /// <summary>3.行情驱动开始撮合【买、卖】市价委托单</summary>
        /// <param name="hqData">行情数据</param>
        public void MatchStart(FutureMarketEntity hqData)
        {
            //撮合中心实体不能为空
            if (hqData == null)
            {
                return;
            }
            try
            {
                smartPool.QueueWorkItem(MatchMarkPriceBuy, hqData);
                smartPool.QueueWorkItem(MatchMarkPriceSell, hqData);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH--0003:撮合中心撮合异常", ex);
                return;
            }
        }
        #endregion

        #region 4.期货卖撮合

        #region 4-1.撮合卖市价的委托单，撮合完后把行情加入到缓存区撮合限价委托

        /// <summary>4-1.撮合卖市价的委托单，撮合完后把行情加入到缓存区撮合限价委托  </summary>
        /// <param name="hqData">行情数据</param>
        private void MatchMarkPriceSell(FutureMarketEntity hqData)
        {
            if (hqData == null)
            {
                return;
            }
            //#region update 2009-01-08
            ////撮合中心撮合实体
            //Dictionary<string, List<CommoditiesDataOrderEntity>> orderDic = sellMarketPriceOrder.GetAcceptEntitys();
            ////撮合中心撮合实体不能为空
            //if (orderDic != null)
            //{
            //    foreach (var item in orderDic)
            //    {
            //        if (item.Value != null)
            //        {
            //            foreach (CommoditiesDataOrderEntity entity in item.Value)
            //            {
            //                MatchMarkPriceBuy(entity, hqData);
            //            }
            //        }
            //    }
            //}

            ////List<CommoditiesDataOrderEntity> matchEntitys = sellMarketPriceOrder.GetAcceptEntitys();
            ////if (!Utils.IsNullOrEmpty(matchEntitys))
            ////{
            ////    foreach (CommoditiesDataOrderEntity entity in matchEntitys)
            ////    {
            ////        MatchMarkPriceSell(entity, hqData);
            ////    }
            ////}
            //#endregion
            realTimeDataSell.Add(hqData);//把当前行情加入到缓存区中撮合限价委托
        }

        /// <summary>4-1-1.卖的市价委托撮合五挡行情</summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情实体</param>
        private void MatchMarkPriceSell(CommoditiesDataOrderEntity orderEntity, FutureMarketEntity hqData)
        {
            //实体不能为空
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            if (orderEntity.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
            {
                #region 涨停 跌停 与行情成交市价相等委托直接撤单
                if (hqData.LastPrice == hqData.LowerPrice)
                {
                    CancelBufferEntity.InsertQueueItem(orderEntity);
                    return;
                }
                #endregion
            }

            decimal markVolume;

            #region 市价最新成交价撮合成交（市价不用与最新成交价撮合也即不用考虑市场存量）
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

            #region 市价买一量撮合成交
            if (hqData.BuyFirstVolume > 0.00m)
            {
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
                    return;
                }
            }
            #endregion

            #region 市价买二量撮合成交
            if (hqData.BuySecondVolume > 0.00m)
            {
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
                    return;
                }
            }
            #endregion

            #region 市价买三量撮合成交
            if (hqData.BuyThirdVolume > 0.00m)
            {
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
                    return;
                }
            }
            #endregion

            #region 市价买四量撮合成交
            if (hqData.BuyFourthVolume > 0.00m)
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
                    orderEntity.OrderVolume = 0.00m;
                    hqData.BuyFourthVolume = hqData.BuyFourthVolume - markVolume;
                    return;
                }
            }
            #endregion

            #region 市价买五量撮合成交
            if (hqData.BuyFiveVolume > 0.00m)
            {
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
                    return;
                }
            }
            #endregion

            #region 如果撮合五档行情后还有没有成交完，则直接撤单
            if (orderEntity.OrderVolume > 0.00m)
            {
                CancelBufferEntity.InsertQueueItem(orderEntity);
                return;
            }
            #endregion
        }

        #endregion

        #region 4-2.撮合卖的限价委托单

        /// <summary>
        /// 4-2.撮合卖的限价委托单
        /// </summary>
        /// <param name="hqData">行情数据</param>
        private void MatchSell(FutureMarketEntity hqData)
        {
            if (hqData == null)
            {
                return;
            }

            #region update 2010-01-08
            //撮合中心撮合实体
            Dictionary<string, List<CommoditiesDataOrderEntity>> orderDic = sellOrder.GetAcceptEntitys();
            //撮合中心撮合实体不能为空
            if (orderDic == null || orderDic.Count <= 0)
            {
                return;
            }

            //List<CommoditiesDataOrderEntity> matchEntitys = sellOrder.GetAcceptEntitys();
            //if (Utils.IsNullOrEmpty(matchEntitys))
            //{
            //    return;
            //}

            //Dictionary<string, List<CommoditiesDataOrderEntity>> orderDic = new Dictionary<string, List<CommoditiesDataOrderEntity>>();

            //#region 组合每个交易员的本次行情的所有委托列表
            //foreach (var model in matchEntitys)
            //{
            //    if (orderDic.ContainsKey(model.SholderCode))
            //    {
            //        orderDic[model.SholderCode].Add(model);
            //    }
            //    else
            //    {
            //        List<CommoditiesDataOrderEntity> list = new List<CommoditiesDataOrderEntity>();
            //        list.Add(model);
            //        orderDic[model.SholderCode] = list;
            //    }
            //}
            //#endregion
            #endregion

            #region 分发每个人的委托列表(对于每个交易员本次行情都是相同)

            foreach (var sholderCodeModel in orderDic)
            {
                List<CommoditiesDataOrderEntity> orderList = sholderCodeModel.Value;
                if (Utils.IsNullOrEmpty(orderList))
                {
                    continue;
                }
                FutureMarketEntity hqDataModel = new FutureMarketEntity();
                hqDataModel = hqData;
                //委托价格市场存量委托编号
                Dictionary<decimal, string> priceDic = new Dictionary<decimal, string>();
                foreach (CommoditiesDataOrderEntity item in orderList)
                {
                    //if (item.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
                    //{
                    //    MatchMarkPriceSell(item, hqDataModel);
                    //}
                    //else
                    //{
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
                    //}
                }
            }
            #endregion

        }

        /// <summary>
        /// 4-2-1限价委托撮合五档行情（撮合卖）
        /// </summary>
        /// <param name="orderEntity">委托单</param>
        /// <param name="hqData">行情实体</param>
        /// <param name="marketVolueOrderNo">市场存量关联编号</param>
        private void MatchSell(CommoditiesDataOrderEntity orderEntity, FutureMarketEntity hqData, ref string marketVolueOrderNo)
        {
            if (orderEntity == null || hqData == null)
            {
                return;
            }
            decimal markVolume;

            //LogHelper.WriteDebug("---->【商品期货】开始撮合卖");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 市价最新成交价撮合成交，如果是第一次（开始）撮合不能撮合最后成交价
            if (hqData.LastVolume > 0.00m)
            {
                #region 委托价格小于最后成交价
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
                    CommoditiesDataOrderAccess.Update(orderEntity.OrderNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);
                    CommoditiesDataOrderAccess.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);//更新关联的委托市场存量
                    if (orderEntity.OrderVolume <= 0)
                    {
                        return;
                    }
                }
                #endregion
            }
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.买一价=" + hqData.BuyFirstPrice + ",买一量=" + hqData.BuyFirstVolume + ",买二价=" + hqData.BuySecondPrice + ",买二量=" + hqData.BuySecondVolume + ",买三价=" + hqData.BuyThirdPrice + ",买三量=" + hqData.BuyThirdVolume + ",买四价=" + hqData.BuyFourthPrice + ",买四量=" + hqData.BuyFourthVolume + ",买五价=" + hqData.BuyFivePrice + ",买五量=" + hqData.BuyFiveVolume);

            #region 限价买一量撮合成交
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

            #region 限价买二量撮合成交
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

            #region 限价买三量撮合成交
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

            #region 限价买四量撮合成交
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

            #region 限价买五量撮合成交
            if (hqData.BuyFiveVolume > 0.00m)
            {
                if (orderEntity.OrderPrice <= hqData.BuyFivePrice)
                {
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

            #region 委托单还有没有撮合完的再添加到委托撮合列表中等待行情再撮合
            if (orderEntity.OrderVolume > 0.00m)
            {
                //LogHelper.WriteDebug("---->委托单还有没有撮合完的再添加到委托撮合列表中等待行情再撮合. OrderVolume=" + orderEntity.OrderVolume);
                if (Types.MatchCenterState.First == orderEntity.MatchState)
                {                    //用户编号+价格+委托编号

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
                    CommoditiesDataOrderAccess.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
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

        #endregion

        #region 5.期货买撮合

        #region 5-1.撮合买市价的委托单，撮合完后把行情加入到缓存区撮合限价委托

        /// <summary> 5-1.撮合买市价的委托单，撮合完后把行情加入到缓存区撮合限价委托 </summary>
        /// <param name="hqData">行情实体</param>
        private void MatchMarkPriceBuy(FutureMarketEntity hqData)
        {

            if (hqData == null)
            {
                return;
            }

            //#region update 2010-01-08
            ////撮合中心撮合实体
            //Dictionary<string, List<CommoditiesDataOrderEntity>> orderDic = buyMarketPriceOrder.GetAcceptEntitys();
            ////撮合中心撮合实体不能为空
            //if (orderDic != null)
            //{
            //    foreach (var item in orderDic)
            //    {
            //        if (item.Value != null)
            //        {
            //            foreach (CommoditiesDataOrderEntity entity in item.Value)
            //            {
            //                MatchMarkPriceBuy(entity, hqData);
            //            }
            //        }
            //    }
            //}


            ////List<CommoditiesDataOrderEntity> matchEntitys = buyMarketPriceOrder.GetAcceptEntitys();
            ////if (matchEntitys != null)
            ////{
            ////    foreach (CommoditiesDataOrderEntity entity in matchEntitys)
            ////    {
            ////        MatchMarkPriceBuy(entity, hqData);
            ////    }
            ////}
            //#endregion
            realTimeDataBuy.Add(hqData);
        }

        ///// <summary> 5-1-1买的市价委托撮合五档行情修改  </summary>
        ///// <param name="orderEntity">撮合队列实体</param>
        ///// <param name="hqData">行情数据</param>
        //private void MatchMarkPriceBuy(CommoditiesDataOrderEntity orderEntity, FutureMarketEntity hqData)
        //{
        //    //实体不能为空
        //    if (orderEntity == null || hqData == null)
        //    {
        //        return;
        //    }
        //    decimal markVolume;
        //    #region 市价最新成交价撮合买成交（市价不用与最新成交价撮合也即不用考虑市场存量）
        //    //if (hqData.LastVolume > 0.00m)
        //    //{
        //    //    if (orderEntity.OrderVolume > hqData.LastVolume)
        //    //    {
        //    //        markVolume = hqData.LastVolume;
        //    //        orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
        //    //        hqData.LastVolume = 0.00m;
        //    //        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
        //    //    }
        //    //    else
        //    //    {
        //    //        markVolume = orderEntity.OrderVolume;
        //    //        InsertDealEntity(orderEntity, hqData.LastPrice, markVolume);
        //    //        orderEntity.OrderVolume = 0.00m;
        //    //        hqData.LastVolume = hqData.LastVolume - markVolume;
        //    //        return;
        //    //    }
        //    //}
        //    #endregion

        //    #region 市价卖一量撮合买成交
        //    if (hqData.SellFirstVolume > 0.00m)
        //    {
        //        if (orderEntity.OrderVolume > hqData.SellFirstVolume)
        //        {
        //            markVolume = hqData.SellFirstVolume;
        //            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
        //            hqData.SellFirstVolume = 0.00m;
        //            InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
        //        }
        //        else
        //        {
        //            markVolume = orderEntity.OrderVolume;
        //            InsertDealEntity(orderEntity, hqData.SellFirstPrice, markVolume);
        //            orderEntity.OrderVolume = 0.00m;
        //            hqData.SellFirstVolume = hqData.SellFirstVolume - markVolume;
        //            return;
        //        }
        //    }
        //    #endregion

        //    #region 市价卖二量撮合买成交
        //    if (hqData.SellSecondVolume > 0.00m)
        //    {
        //        if (orderEntity.OrderVolume > hqData.SellSecondVolume)
        //        {
        //            markVolume = hqData.SellSecondVolume;
        //            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
        //            hqData.SellSecondVolume = 0.00m;
        //            InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
        //        }
        //        else
        //        {
        //            markVolume = orderEntity.OrderVolume;
        //            InsertDealEntity(orderEntity, hqData.SellSecondPrice, markVolume);
        //            orderEntity.OrderVolume = 0.00m;
        //            hqData.SellSecondVolume = hqData.SellSecondVolume - markVolume;
        //            return;
        //        }
        //    }
        //    #endregion

        //    #region 市价卖三量撮合买成交
        //    if (hqData.SellThirdVolume > 0.00m)
        //    {
        //        if (orderEntity.OrderVolume > hqData.SellThirdVolume)
        //        {
        //            markVolume = hqData.SellThirdVolume;
        //            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
        //            hqData.SellThirdVolume = 0.00m;
        //            InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
        //        }
        //        else
        //        {
        //            markVolume = orderEntity.OrderVolume;
        //            InsertDealEntity(orderEntity, hqData.SellThirdPrice, markVolume);
        //            orderEntity.OrderVolume = 0.00m;
        //            hqData.SellThirdVolume = hqData.SellThirdVolume - markVolume;
        //            return;
        //        }
        //    }
        //    #endregion

        //    #region 市价卖四量撮合买成交
        //    if (hqData.SellFourthVolume > 0.00m)
        //    {
        //        if (orderEntity.OrderVolume > hqData.SellFourthVolume)
        //        {
        //            markVolume = hqData.SellFourthVolume;
        //            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
        //            hqData.SellFourthVolume = 0.00m;
        //            InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
        //        }
        //        else
        //        {
        //            markVolume = orderEntity.OrderVolume;
        //            InsertDealEntity(orderEntity, hqData.SellFourthPrice, markVolume);
        //            orderEntity.OrderVolume = 0.00m;
        //            hqData.SellFourthVolume = hqData.SellFourthVolume - markVolume;
        //            return;
        //        }
        //    }
        //    #endregion

        //    #region 市价卖五量撮合买成交
        //    if (hqData.SellFiveVolume > 0.00m)
        //    {
        //        if (orderEntity.OrderVolume > hqData.SellFiveVolume)
        //        {
        //            markVolume = hqData.SellFiveVolume;
        //            orderEntity.OrderVolume = orderEntity.OrderVolume - markVolume;
        //            hqData.SellFiveVolume = 0.00m;
        //            InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
        //        }
        //        else
        //        {
        //            markVolume = orderEntity.OrderVolume;
        //            InsertDealEntity(orderEntity, hqData.SellFivePrice, markVolume);
        //            orderEntity.OrderVolume = 0.00m;
        //            hqData.SellFiveVolume = hqData.SellFiveVolume - markVolume;
        //            return;
        //        }
        //    }
        //    #endregion

        //    #region 市价撮合，如果五档量撮合不完直接当作撤单
        //    if (orderEntity.OrderVolume > 0.00m)
        //    {
        //        CancelBufferEntity.InsertQueueItem(orderEntity);
        //    }
        //    #endregion
        //}
        #endregion

        #region 5-2.撮合买限价的委托单
        /// <summary> 5-2.买限价委托撮合</summary>
        /// <param name="hqData">行情实体</param>
        private void MatchBuy(FutureMarketEntity hqData)
        {
            if (hqData == null)
            {
                return;
            }

            #region update 2010-01-08
            //撮合中心撮合实体
            Dictionary<string, List<CommoditiesDataOrderEntity>> orderDic = buyOrder.GetAcceptEntitys();
            //撮合中心撮合实体不能为空
            if (orderDic == null || orderDic.Count <= 0)
            {
                return;
            }


            //List<CommoditiesDataOrderEntity> matchEntitys = buyOrder.GetAcceptEntitys();
            //if (matchEntitys == null)
            //{
            //    return;
            //}
            //Dictionary<string, List<CommoditiesDataOrderEntity>> orderDic = new Dictionary<string, List<CommoditiesDataOrderEntity>>();

            //#region 组合每个交易员的本次行情的所有委托列表
            //foreach (var model in matchEntitys)
            //{
            //    if (orderDic.ContainsKey(model.SholderCode))
            //    {
            //        orderDic[model.SholderCode].Add(model);
            //    }
            //    else
            //    {
            //        List<CommoditiesDataOrderEntity> list = new List<CommoditiesDataOrderEntity>();
            //        list.Add(model);
            //        orderDic[model.SholderCode] = list;
            //    }
            //}
            //#endregion
            #endregion


            #region 分发每个人的委托列表(对于每个交易员本次行情都是相同)

            foreach (var sholderCodeModel in orderDic)
            {

                List<CommoditiesDataOrderEntity> orderList = sholderCodeModel.Value;
                if (Utils.IsNullOrEmpty(orderList))
                {
                    continue;
                }
                FutureMarketEntity hqDataModel = new FutureMarketEntity();
                hqDataModel = hqData;
                //委托价格市场存量委托编号
                Dictionary<decimal, string> priceDic = new Dictionary<decimal, string>();
                foreach (CommoditiesDataOrderEntity item in orderList)
                {
                    //if (item.IsMarketPrice == (int)Types.MarketPriceType.MarketPrice)
                    //{
                    //    MatchMarkPriceBuy(item, hqDataModel);
                    //}
                    //else
                    //{
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
                    //}
                }
            }
            #endregion

        }

        /// <summary> 5-2-1限价委托撮合五档行情（撮合买）</summary>
        /// <param name="orderEntity">委托实体</param>
        /// <param name="hqData">行情实体</param>
        /// <param name="marketVolueOrderNo">市场存量委托编号</param>
        private void MatchBuy(CommoditiesDataOrderEntity orderEntity, FutureMarketEntity hqData, ref string marketVolueOrderNo)
        {
            if (orderEntity == null || hqData == null)
            {
                return;
            }

            decimal markVolume;

            //LogHelper.WriteDebug("---->【商品期货】开始撮合买");
            //LogHelper.WriteDebug("----> 行情: LastVolume=" + hqData.LastVolume + ",LastPrice=" + hqData.LastPrice + ";委托: OrderVolume=" + orderEntity.OrderVolume + ",OrderPrice=" + orderEntity.OrderPrice);

            #region 市价撮合是否为其他撮合，如果是第一次（开始）撮合不能撮合最后成交价
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
                    CommoditiesDataOrderAccess.Update(orderEntity.OrderNo, marketVolueOrderNo, orderEntity.MarkLeft);
                    CommoditiesDataOrderAccess.Update(orderEntity.MarketVolumeNo, orderEntity.MarketVolumeNo, orderEntity.MarkLeft);//更新关联的委托市场存量
                    if (orderEntity.OrderVolume <= 0)
                    {
                        return;
                    }
                }
                #endregion
            }
            #endregion

            //LogHelper.WriteDebug("---->五档行情撮合.卖一价=" + hqData.SellFirstPrice + ",卖一量=" + hqData.SellFirstVolume + ",卖二价=" + hqData.SellSecondPrice + ",卖二量=" + hqData.SellSecondVolume + ",卖三价=" + hqData.SellThirdPrice + ",卖三量=" + hqData.SellThirdVolume + ",卖四价=" + hqData.SellFourthPrice + ",卖四量=" + hqData.SellFourthVolume + ",卖五价=" + hqData.SellFivePrice + ",卖五量=" + hqData.SellFiveVolume);

            #region 卖一价撮合买成交
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

            #region 卖二价撮合买成交
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

            #region 卖三价撮合买成交
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

            #region 卖四价撮合买成交
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

            #region 卖五价撮合买成交
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
                    CommoditiesDataOrderAccess.Update(orderEntity.OrderNo, marketVolueOrderNo, marketVolume);
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

        #endregion

        #region 6.撮合成交回报
        /// <summary>
        /// 6.撮合成交回报
        /// </summary>
        /// <param name="entity">成交回报实体</param>
        /// <param name="price">价格</param>
        /// <param name="volume">数量</param>
        private void InsertDealEntity(CommoditiesDataOrderEntity entity, decimal price, decimal volume)
        {

            //撮合中心实体不能为空
            if (entity == null)
            {
                return;
            }
            if (volume <= 0.00m)
            {
                return;
            }
            string code = Guid.NewGuid().ToString();
            var deal = new CommoditiesDealBackEntity();
            //id
            deal.Id = code;
            //撮合中心通道号码
            deal.ChannelID = entity.ChannelID;
            deal.OrderNo = entity.OrderNo;
            //撮合中心成交时间
            deal.DealTime = DateTime.Now;
            deal.DealAmount = volume;
            deal.DealPrice = price;
            if (CommoditiesDealOrderAccess.Add(deal) == 1)
            {
                //StockDealMessage dealData = new StockDealMessage();
                //dealData.FutureEtity = deal;
                //dealData.StockCode = matchCode;
                //ShowMatchMessageDeletate showMatchMessageDeletate = MatchCenterManager.Instance.ShowMatchMessage;
                //showMatchMessageDeletate.BeginInvoke(dealData, null, null);
                string showMesg = string.Format(GenerateInfo.CH_D002, GenerateInfo.CH_D04, deal.OrderNo, matchCode, deal.DealTime,
                    deal.DealAmount, entity.IsMarketPrice == 0 ? "市价" : "限价", deal.DealPrice);
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowMatchMessage, showMesg);

                CommoditiesDataOrderAccess.Update(entity.OrderNo, volume);

                MatchCenterManager.Instance.matchCenterBackService.commoditiesDealOrderCacheList.InsertQueueItem(deal);

                LogHelper.WriteDebug(showMesg);
                //LogHelper.WriteDebug(" 成交回报[" + "委托id=" + code + ", 委托代码=" + entity.StockCode + " ,回报时间=" + DateTime.Now +
                //                     ",委托单号码=" + entity.OldOrderNo + ", 成交价格=" + price + ",成交数量=" + volume + "]");
            }
            else
            {
                //插入不成功把之前的数据添加回到撮合单元中
            }

        }
        #endregion

        #endregion

        #region 7.委托单不在交易价格范围，发生内部撤单事件
        /// <summary>
        /// 7.委托单不在交易价格范围，发生内部撤单事件
        /// Create by 董鹏
        /// Crate date: 2010-01-22
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InternalCancelOraderProcessEvent(object sender, QueueItemHandleEventArg<CommoditiesDataOrderEntity> e)
        {
            if (e.Item == null)
            {
                return;
            }
            CommoditiesDataOrderEntity model = e.Item;
            //委托单不能为空
            if (model == null || string.IsNullOrEmpty(model.ChannelID))
            {
                return;
            }
            string code = Guid.NewGuid().ToString();
            var dataEntity = new ResultDataEntity();
            dataEntity.Id = code;
            dataEntity.OrderNo = model.OrderNo;    //委托单号码

            MerFutData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetMercantileFutData(matchCode);
            decimal entrustPrice = model.OrderPrice;
            if (data != null)
            {
                entrustPrice = (decimal)data.Lasttrade;
            }
            string messInfo = string.Format(GenerateInfo.CH_D007, entrustPrice);
            dataEntity.Message = messInfo;

            OperationContext context = null;
            if (model.ChannelID != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelID))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelID];
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
                    smartPool.QueueWorkItem(callback.ProcessCommoditiesOrderRpt, dataEntity);

                    string showMesg = string.Format(GenerateInfo.CH_D001, messInfo, model.OrderNo, model.StockCode, DateTime.Now, model.OrderVolume, "限价");
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

        #region 8.撮合五档价格撮合不完委托量,内部自动撤单
        /// <summary>
        /// 撮合五档价格撮合不完委托量,内部自动撤单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessCanceBussiness(object sender, QueueItemHandleEventArg<CommoditiesDataOrderEntity> e)
        {
            CommoditiesDataOrderEntity model = e.Item;
            if (model == null)
            {
                return;
            }
            //撮合中心委托实体
            var dataEntity = new CancelOrderEntity();
            dataEntity.OrderNo = model.OrderNo;
            dataEntity.IsSuccess = true;
            dataEntity.OrderVolume = model.OrderVolume;
            dataEntity.Message = "撤单成功!";
            OperationContext context = null;
            if (model.ChannelID != null)
            {
                //获取撮合中心上下文
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelID))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelID];
                }
            }
            if (context == null)
            {
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    smartPool.QueueWorkItem(callback.CancelCommoditiesOrderRpt, dataEntity);
                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteError("CH-0001:wcf服务通道阻塞", ex);
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                return;
            }
        }
        #endregion

        #region 9.卖买根据委托价格获取市场存量
        /// <summary>
        /// 买操作查找市场存量
        /// </summary>
        /// <param name="entrustPrice">委托价格</param>
        /// <param name="hqData">实体</param>
        /// <returns></returns>
        private decimal GetBuyMarkLeft(decimal entrustPrice, FutureMarketEntity hqData)
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
        private decimal GetSellMarkLeft(decimal entrustPrice, FutureMarketEntity hqData)
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

        #region  清除撮合买卖委托单缓冲数据
        /// <summary>
        /// 清除撮合买卖委托单缓冲数据
        /// </summary>
        public void Clear()
        {
            if (buyOrder != null)
            {
                buyOrder.Clear();
            }
            if (sellOrder != null)
            {
                sellOrder.Clear();
            }
            //if (buyMarketPriceOrder != null)
            //{
            //    buyMarketPriceOrder.Clear();
            //}
            //if (sellMarketPriceOrder != null)
            //{
            //    sellMarketPriceOrder.Clear();
            //}
        }
        #endregion
    }
}
