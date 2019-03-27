#region Using Namespace

using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using Amib.Threading;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateAcceptOffer.OrderAccepter.Logic;
using ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer;
using ReckoningCounter.BLL.DelegateValidate;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.Reckoning.Instantaneous;
using ReckoningCounter.BLL.Reckoning.Logic;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.DAL.MatchCenterService;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.DelegateOffer;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;
using ResultDataEntity = ReckoningCounter.DAL.MatchCenterService.ResultDataEntity;
using GTA.VTS.Common.CommonObject;

#endregion

namespace ReckoningCounter.BLL.delegateoffer
{
    /// <summary>
    /// 报盘中心,错误编码2140-2199
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class OrderOfferCenter
    {
        #region == 静态属性 ==

        private static volatile OrderOfferCenter _instance;
        private static object lockObject = new object();

        /// <summary>
        /// 单例
        /// </summary>
        public static OrderOfferCenter Instance
        {
            get
            {
                //if (_instance == null)
                //    _instance = new OrderOfferCenter(10);
                //return _instance;

                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new OrderOfferCenter(10);
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region == 字段/属性 ==

        /// <summary>
        /// 柜台客户端ID
        /// </summary>
        public static string COUNT_CLIENT_ID = string.Empty;

        /// <summary>
        /// 并行工作报盘数
        /// </summary>
        private int _callbackDealWorkingThreadCount = 10;

        /// <summary>
        /// 柜台缓存对象
        /// </summary>
        protected CounterCache xhCounterCache = Common.XHCounterCache.Instance;

        protected CounterCache qhCounterCache = Common.QHCounterCache.Instance;

        protected CounterCache hkCounterCache = Common.HKCounterCache.Instance;

        /// <summary>
        /// 港股报盘工作容器
        /// </summary>
        private List<QueueBufferBase<HkTodayEntrustEx>> _hkQueueList;


        /// <summary>
        /// 商品期货报盘工作容器
        /// </summary>
        private List<QueueBufferBase<QhTodayEntrustTableEx>> _mercantileFutureQueueList;

        /// <summary>
        /// 委托暂存器
        /// </summary>
        internal OrderCache _orderCache;

        /// <summary>
        /// 下单服务通道管理器
        /// </summary>
        private SeviceChannelManager _seviceChannelManager;

        /// <summary>
        /// 股指期货报盘工作容器
        /// </summary>
        private List<QueueBufferBase<QhTodayEntrustTableEx>> _stockIndexFuturesFutureQueueList;

        /// <summary>
        /// 现货报盘工作容器
        /// </summary>
        private List<QueueBufferBase<XhTodayEntrustTableEx>> _stockQueueList;

        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };

        /// <summary>
        /// 成交回报事件
        /// </summary>
        public event EventHandler<RuntimeMessageEventArge> DoOfferOrderEvent = null;

        #endregion

        #region == 构造器 ==

        static OrderOfferCenter()
        {
            //COUNT_CLIENT_ID = ConfigurationManager.AppSettings[Utils.CounterID];
            COUNT_CLIENT_ID = ServerConfig.CounterID;

            if (string.IsNullOrEmpty(COUNT_CLIENT_ID))
            {
                //COUNT_CLIENT_ID = Dns.GetHostName() + Guid.NewGuid();
                //改为写MAC地址+时间
                COUNT_CLIENT_ID = CommUtils.GetMacAddress();

                //ConfigFile.SetKeyValue(Utils.CounterID, COUNT_CLIENT_ID);
                ServerConfig.CounterID = COUNT_CLIENT_ID;
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="callbackDealWorkingThreadCount">并行工作数</param>
        private OrderOfferCenter(int callbackDealWorkingThreadCount)
        {
            InitializeThreadPool();

            _seviceChannelManager = new SeviceChannelManager("NetTcpBinding_IDoOrder",
                                                             "NetTcpBinding_IOrderDealRpt", COUNT_CLIENT_ID);
            _callbackDealWorkingThreadCount = callbackDealWorkingThreadCount;
            _stockQueueList = new List<QueueBufferBase<XhTodayEntrustTableEx>>();
            _hkQueueList = new List<QueueBufferBase<HkTodayEntrustEx>>();
            _mercantileFutureQueueList = new List<QueueBufferBase<QhTodayEntrustTableEx>>();
            _stockIndexFuturesFutureQueueList = new List<QueueBufferBase<QhTodayEntrustTableEx>>();
            _orderCache = new OrderCache();
            for (int i = 0; i < _callbackDealWorkingThreadCount; i++)
            {
                var xhEntrustBuffer = new QueueBufferBase<XhTodayEntrustTableEx>();
                xhEntrustBuffer.Name = "XHBuffer-" + (i + 1);
                xhEntrustBuffer.QueueItemProcessEvent += DoXHOrderOffer;
                //xhEntrustBuffer.QueueItemProcessEvent +=
                //    (sender, e) => smartPool.QueueWorkItem(DoXHOrderOffer, sender, e);
                _stockQueueList.Add(xhEntrustBuffer);

                var hkEntrustBuffer = new QueueBufferBase<HkTodayEntrustEx>();
                hkEntrustBuffer.Name = "HKBuffer-" + (i + 1);
                hkEntrustBuffer.QueueItemProcessEvent += DoHKOrderOffer;
                //xhEntrustBuffer.QueueItemProcessEvent +=
                //    (sender, e) => smartPool.QueueWorkItem(DoXHOrderOffer, sender, e);
                _hkQueueList.Add(hkEntrustBuffer);

                var qhEntrustBuffer = new QueueBufferBase<QhTodayEntrustTableEx>();
                qhEntrustBuffer.Name = "SPQHBuffer-" + (i + 1);
                qhEntrustBuffer.QueueItemProcessEvent += DoSPQHOrderOffer;
                _mercantileFutureQueueList.Add(qhEntrustBuffer);

                var gzqhEntrustBuffer = new QueueBufferBase<QhTodayEntrustTableEx>();
                gzqhEntrustBuffer.Name = "GZQHBuffer-" + (i + 1);
                gzqhEntrustBuffer.QueueItemProcessEvent += DoGZQHOrderOffer;
                _stockIndexFuturesFutureQueueList.Add(gzqhEntrustBuffer);
            }

            _orderCache.XHOfferWakeupEvent += _orderCache_XHOfferWakeupEvent;
            _orderCache.GZQHOfferWakeupEvent += _orderCache_GZQHOfferWakeupEvent;
            _orderCache.QHOfferWakeupEvent += _orderCache_QHOfferWakeupEvent;
            _orderCache.HKOfferWakeupEvent += _orderCache_HKOfferWakeupEvent;
        }

        private void InitializeThreadPool()
        {
            int offerThreadCount = 200;
            string offerThread = ConfigurationManager.AppSettings["offerThread"];
            if (!string.IsNullOrEmpty(offerThread))
            {
                int count = 0;
                bool isSuccess = int.TryParse(offerThread.Trim(), out count);
                if (isSuccess)
                    offerThreadCount = count;
            }

            smartPool.MaxThreads = offerThreadCount;
            smartPool.MinThreads = 25;
            smartPool.Start();
        }

        public void DoClose()
        {
            this._seviceChannelManager.DoClose();
        }

        #endregion

        #region 唤醒通知

        /// <summary>
        /// 唤醒通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _orderCache_QHOfferWakeupEvent(object sender, WakeupEventArgs<QhTodayEntrustTableEx> e)
        {
            //this.OfferSPQHOrder(e.Item);
            DispatchSPQHOrder(e.Item);
        }

        /// <summary>
        /// 唤醒通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _orderCache_GZQHOfferWakeupEvent(object sender, WakeupEventArgs<QhTodayEntrustTableEx> e)
        {
            //this.OfferGZQHOrder(e.Item);
            DispatchGZQHOrder(e.Item);
        }

        /// <summary>
        /// 唤醒通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _orderCache_XHOfferWakeupEvent(object sender, WakeupEventArgs<XhTodayEntrustTableEx> e)
        {
            //this.OfferStockOrder(e.Item);
            DispatchStockOrder(e.Item);
        }

        /// <summary>
        /// 唤醒通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _orderCache_HKOfferWakeupEvent(object sender, WakeupEventArgs<HkTodayEntrustEx> e)
        {
            //this.OfferStockOrder(e.Item);
            DispatchHKOrder(e.Item);
        }

        #endregion

        #region 委托对象转换

        /// <summary>
        /// 报盘商品期货对象转换
        /// </summary>
        /// <param name="qHOrder"></param>
        /// <returns></returns>
        private CommoditiesOrderEntity ConvertSPQH(QhTodayEntrustTableEx qHOrder)
        {
            var result = new CommoditiesOrderEntity();
            //通道ID
            result.ChannelNo = COUNT_CLIENT_ID;
            //市价委托
            result.IsMarketPrice = qHOrder.OriginalEntity.IsMarketValue
                                      ? (TypesMarketPriceType)GTA.VTS.Common.CommonObject.Types.MarketPriceType.MarketPrice
                                      : (TypesMarketPriceType)GTA.VTS.Common.CommonObject.Types.MarketPriceType.otherPrice;
            //委托价格
            result.OrderPrice = qHOrder.OriginalEntity.EntrustPrice;
            //委托量
            result.OrderVolume = qHOrder.OriginalEntity.EntrustAmount;
            //委托代码
            result.StockCode = qHOrder.OriginalEntity.ContractCode;
            //买卖方向
            result.TransactionDirection = qHOrder.OriginalEntity.BuySellTypeId == 1
                                              ? (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                              : (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling;

            //股东代码
            result.SholderCode = qHOrder.OriginalEntity.TradeAccount;
            return result;
        }

        /// <summary>
        /// 报盘股指期货委托对象转换
        /// </summary>
        /// <param name="qHOrder"></param>
        /// <returns></returns>
        private FutureOrderEntity ConvertGZQH(QhTodayEntrustTableEx qHOrder)
        {
            var result = new FutureOrderEntity();
            //通道ID
            result.ChannelNo = COUNT_CLIENT_ID;
            //市价委托
            result.IsMarketPrice = qHOrder.OriginalEntity.IsMarketValue
                                   ? (TypesMarketPriceType)GTA.VTS.Common.CommonObject.Types.MarketPriceType.MarketPrice
                                   : (TypesMarketPriceType)GTA.VTS.Common.CommonObject.Types.MarketPriceType.otherPrice;
            //+
            result.OrderPrice = qHOrder.OriginalEntity.EntrustPrice;
            //委托量
            result.OrderVolume = qHOrder.OriginalEntity.EntrustAmount;
            //委托代码
            result.StockCode = qHOrder.OriginalEntity.ContractCode;
            //买卖方向
            
            result.TransactionDirection = qHOrder.OriginalEntity.BuySellTypeId == 1
                                              ? (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                              : (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling;

            //股东代码
            result.SholderCode = qHOrder.OriginalEntity.TradeAccount;

            return result;
        }

        /// <summary>
        /// 报盘现货委托对象转换
        /// </summary>
        /// <param name="xhOrder"></param>
        /// <returns></returns>
        private StockOrderEntity ConvertStock(XhTodayEntrustTableEx xhOrder)
        {
            var result = new StockOrderEntity();
            //通道ID
            result.ChannelNo = COUNT_CLIENT_ID;
            //市价委托
            result.IsMarketPrice = xhOrder.OriginalEntity.IsMarketValue
                                       ? (TypesMarketPriceType)GTA.VTS.Common.CommonObject.Types.MarketPriceType.MarketPrice
                                       : (TypesMarketPriceType)GTA.VTS.Common.CommonObject.Types.MarketPriceType.otherPrice;
            //委托价格
            result.OrderPrice = xhOrder.OriginalEntity.EntrustPrice;
            //委托量
            result.OrderVolume = xhOrder.OriginalEntity.EntrustAmount;
            //委托代码
            result.StockCode = xhOrder.OriginalEntity.SpotCode;
            //买卖方向
            result.TransactionDirection = xhOrder.OriginalEntity.BuySellTypeId == 1
                                              ? (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                              : (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling;
            //(CommonObject.Types.TransactionDirection) Enum.Parse(typeof (CommonObject.Types.TransactionDirection),
            //                                                     xhOrder.OriginalEntity.BuySellTypeId.ToString());
            //股东代码
            result.SholderCode = xhOrder.OriginalEntity.StockAccount;

            return result;
        }

        /// <summary>
        /// 报盘港股委托对象转换
        /// </summary>
        /// <param name="hkOrder"></param>
        /// <returns></returns>
        private HKOrderEntity ConvertHK(HkTodayEntrustEx hkOrder)
        {
            var result = new HKOrderEntity();
            //通道ID
            result.ChannelNo = COUNT_CLIENT_ID;

            //result.HKPriceType = hkOrder.PriceType;           
            result.HKPriceType = (TypesHKPriceType)hkOrder.OriginalEntity.OrderPriceType;


            //委托价格
            result.OrderPrice = hkOrder.OriginalEntity.EntrustPrice;
            //委托量
            result.OrderVolume = hkOrder.OriginalEntity.EntrustAmount;
            //委托代码
            result.Code = hkOrder.OriginalEntity.Code;
            //买卖方向
            result.TransactionDirection = hkOrder.OriginalEntity.BuySellTypeID == 1
                                              ? (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying
                                              : (TypesTransactionDirection)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling;
            //(CommonObject.Types.TransactionDirection) Enum.Parse(typeof (CommonObject.Types.TransactionDirection),
            //                                                     hkOrder.OriginalEntity.BuySellTypeId.ToString());
            //股东代码
            result.SholderCode = hkOrder.OriginalEntity.HoldAccount;

            return result;
        }

        #endregion

        #region 内部撤单失败流程

        /// <summary>
        /// 现货内部撤单，当报盘失败时执行（如无法通讯，报盘返回结果错误等）
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="strErrMessage"></param>
        public bool IntelnalCancelXHOrder(XH_TodayEntrustTableInfo tet, string strErrMessage)
        {
            //OrderLogicFlowBase<StockOrderRequest, XhTodayEntrustTable, StockDealBackEntity>
            //    _XHDataLogicProcessor =
            //        DataLogicProcessorFactory.GetXHDataLogicProcessor(
            //            (CommonObject.Types.TransactionDirection)
            //            Enum.Parse(typeof (CommonObject.Types.TransactionDirection),
            //                       tet.BuySellTypeId.ToString()));

            //if (_XHDataLogicProcessor != null)
            //{
            //    if (_XHDataLogicProcessor.DelegateOffer_CancelOrder(tet, strErrMessage))
            //        return true;
            //}

            XHAcceptLogic logic = new XHAcceptLogic();

            //内部撤单也要回推结果到前台
            //(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
            logic.EndCancelEvent += XhAcceptLogic_EndCancelEvent;

            if (logic.InternalCancelOrder(tet, strErrMessage))
                return true;

            return false;
        }

        private void XhAcceptLogic_EndCancelEvent(ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> obj)
        {
            CounterOrderService.Instance.AcceptStockDealOrder(obj);
        }

        /// <summary>
        /// 港股内部撤单，当报盘失败时执行（如无法通讯，报盘返回结果错误等）
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="strErrMessage"></param>
        public bool IntelnalCancelHKOrder(HK_TodayEntrustInfo tet, string strErrMessage)
        {
            HKAcceptLogic logic = new HKAcceptLogic();

            //内部撤单也要回推结果到前台
            //(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
            logic.EndCancelEvent += HkAcceptLogic_EndCancelEvent;

            if (logic.InternalCancelOrder(tet, strErrMessage))
                return true;

            return false;
        }

        private void HkAcceptLogic_EndCancelEvent(ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> obj)
        {
            CounterOrderService.Instance.AcceptHKDealOrder(obj);
        }

        /// <summary>
        /// 商品期货内部撤单，当报盘失败时执行（如无法通讯，报盘返回结果错误等）
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="strErrMessage"></param>
        public bool IntelnalCancelSPQHOrder(QH_TodayEntrustTableInfo tet, string strErrMessage)
        {
            //Types.FutureOpenCloseType openCloseType =
            //    (Types.FutureOpenCloseType)tet.OpenCloseTypeId;
            //OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
            //    _SPQHDataLogicProcessor =
            //        DataLogicProcessorFactory.GetSPQHDataLogicProcessor(openCloseType);
            //if (_SPQHDataLogicProcessor != null)
            //{
            //    if (_SPQHDataLogicProcessor.DelegateOffer_CancelOrder(tet, strErrMessage))
            //        return true;
            //}

            SPQHAcceptLogic logic = new SPQHAcceptLogic();

            //内部撤单结果也要回推到前台
            //(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
            logic.EndCancelEvent += SPQHAcceptLogic_EndCancelEvent;

            if (logic.InternalCancelOrder(tet, strErrMessage))
                return true;

            return false;
        }

        private void SPQHAcceptLogic_EndCancelEvent(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> obj)
        {
            CounterOrderService.Instance.AcceptMercantileFuturesOrderDealOrder(obj);
        }

        /// <summary>
        /// 股指期货内部撤单，当报盘失败时执行（如无法通讯，报盘返回结果错误等）
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="strErrMessage"></param>
        public bool IntelnalCancelGZQHOrder(QH_TodayEntrustTableInfo tet, string strErrMessage)
        {
            //Types.FutureOpenCloseType openCloseType =
            //    (Types.FutureOpenCloseType) tet.OpenCloseTypeId;
            //OrderLogicFlowBase<StockIndexFuturesOrderRequest, QH_TodayEntrustTableInfo, FutureDealBackEntity>
            //    _GZQHDataLogicProcessor =
            //        DataLogicProcessorFactory.GetGZQHDataLogicProcessor(openCloseType);
            //if (_GZQHDataLogicProcessor != null)
            //{
            //    if (_GZQHDataLogicProcessor.DelegateOffer_CancelOrder(tet, strErrMessage))
            //        return true;
            //}

            //内部撤单也要回推到前台
            //(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
            GZQHAcceptLogic logic = new GZQHAcceptLogic();
            logic.EndCancelEvent += GZQHAcceptLogic_EndCancelEvent;

            if (logic.InternalCancelOrder(tet, strErrMessage))
                return true;

            return false;
        }

        private void GZQHAcceptLogic_EndCancelEvent(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> obj)
        {
            CounterOrderService.Instance.AcceptStockIndexFuturesDealOrder(obj);
        }

        #endregion

        #region 内部报盘

        /// <summary>
        /// 真实商品期货报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoSPQHOrderOffer(object sender, QueueItemHandleEventArg<QhTodayEntrustTableEx> e)
        {

            DateTime t1 = DateTime.Now;
            QH_TodayEntrustTableInfo tet = e.Item.OriginalEntity;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(e.Item.OriginalEntity.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                string strMessage = string.Empty;

                //1.查找撮合中心信息
                if (mc == null)
                {
                    //IntelnalCancelXHOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2160:[商品期货报盘]交易代码未分配对应的撮合中心，委托作废，请检查管理中心相关的配置.";
                    IntelnalCancelSPQHOrder(tet, tet.OrderMessage);
                    LogHelper.WriteInfo(tet.OrderMessage);
                    goto EndProcess;
                }

                //2.第一次获取报盘通道
                DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);

                //3.第二次获取报盘通道，当第一次失败时
                if (doc == null)
                {
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);
                }

                //4.两次获取报盘通道都不正常时，委托作废
                tet.OrderMessage = strMessage;
                if (cs != ChannelState.CSNormal || doc == null)
                {
                    IntelnalCancelSPQHOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2161:[商品期货报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启";
                    goto EndProcess;
                }

                var mcOrder = ConvertSPQH(e.Item);

                LogHelper.WriteDebug("-------->商品期货内部报盘OrderOfferCenter.DoSPQHOrderOffer，" + mcOrder.DescInfo());
                ResultDataEntity rde = null;
                try
                {
                    //5.第一次报盘
                    //FireDoXHOfferOrderEvent(mcOrder);
                    DateTime dt1 = DateTime.Now;
                    rde = doc.DoCommoditiesOrder(mcOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoSPQHOfferOrderEvent(mcOrder, ts.TotalMilliseconds.ToString());
                }
                catch (CommunicationException ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    //6.获取报盘通道并第二次报盘，当第一次报盘失败时
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService);

                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);
                    //7.当获取的报盘通道不正常时，委托作废
                    if (doc == null)
                    {
                        IntelnalCancelSPQHOrder(tet, tet.OrderMessage);
                        tet.OrderMessage = "GT-2162:[商品期货报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启.";

                        goto EndProcess;
                    }
                    rde = doc.DoCommoditiesOrder(mcOrder);
                }

                if (rde == null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService);

                    tet.OrderMessage = "GT-2163:[商品期货报盘]撮合中心返回结果异常，委托作废，请检查撮合服务是否正常运行.";
                    IntelnalCancelSPQHOrder(tet, tet.OrderMessage);

                    LogHelper.WriteInfo("rde=null," + tet.OrderMessage + ", 报盘对象" + mcOrder.DescInfo());
                    goto EndProcess;
                }

                if (rde.OrderNo.Length > 0)
                {
                    //已报
                    tet.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    //撮合返回的委托单号存储
                    tet.McOrderId = rde.OrderNo;

                    LogHelper.WriteDebug("OrderOfferCenter.DoSPQHOrderOffer添加委托单号映射到ConterCache,OrderNo=" + rde.OrderNo);

                    //柜台缓存委托相关信息
                    var item = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount, tet.EntrustNumber,
                                                  (GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                  Enum.Parse(
                                                      typeof(
                                                          GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                      tet.BuySellTypeId.ToString()),
                                                  (ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType)
                                                  Enum.Parse(
                                                      typeof(
                                                          ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType),
                                                      tet.OpenCloseTypeId.ToString()));
                    item.EntrustAmount = tet.EntrustAmount;
                    item.Code = tet.ContractCode;
                    item.TraderId = e.Item.TraderId;

                    qhCounterCache.AddOrderMappingInfo(rde.OrderNo, item);
                    //0812：根据算法交易要求，报盘成功也要推一次
                    ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject = new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                    reckonEndObject.IsSuccess = true;
                    reckonEndObject.EntrustTable = tet;
                    reckonEndObject.TradeTableList = new List<QH_TodayTradeTableInfo>();
                    reckonEndObject.TradeID = e.Item.TraderId;
                    reckonEndObject.Message = "";

                    CounterOrderService.Instance.AcceptMercantileFuturesOrderDealOrder(reckonEndObject);
                }
                else
                {
                    IntelnalCancelSPQHOrder(tet, rde.Message);
                }

            EndProcess:
                //报盘结束
                ;
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2164:[商品期货报盘]无法报盘，委托作废，请检查撮合服务是否开启.";
                IntelnalCancelSPQHOrder(tet, tet.OrderMessage);
                LogHelper.WriteError(ex.Message, ex);

                if (mc != null)
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(tet.OrderMessage))
                OrderOfferDataLogic.UpdateFutureOrder(tet);
            }

#if(DEBUG)
            DateTime t2 = DateTime.Now;
            TimeSpan ts2 = t2 - t1;
            LogHelper.WriteDebug("报盘TotalTime=" + ts2.TotalMilliseconds);
#endif

            #region 旧报盘逻辑

            /*
            ----------------------------------------------
            QhTodayEntrustTable tet = e.Item.OriginalEntity;
            try
            {
                RC_MatchCenter mc =
                    MCService.CommonPara.GetMatchCenterByCommodityCode(e.Item.OriginalEntity.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                string strMessage = string.Empty;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                                mc.XiaDanService, mc.CuoHeService,
                                                                                ref cs, ref strMessage);
                    if (doc == null)
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                      mc.XiaDanService, mc.CuoHeService,
                                                                      ref cs, ref strMessage);

                    tet.OrderMessage = strMessage;
                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = ConvertSPQH(e.Item);
                        ResultDataEntity rde = doc.DoCommoditiesOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.OrderNo.Length > 0)
                            {
                                //已报
                                tet.OrderStatusId = (int) Types.OrderStateType.DOSIsRequired;
                                //委托单号存储
                                tet.McOrderId = rde.OrderNo;

                                LogHelper.WriteDebug("OrderOfferCenter.DoQHOrderOffer添加委托单号映射到ConterCache,OrderNo=" +
                                                     rde.OrderNo);
                                var item = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount,
                                                              tet.EntrustNumber,
                                                              (
                                                              CommonObject.Types.TransactionDirection)
                                                              Enum.Parse(
                                                                  typeof (
                                                                      CommonObject.Types.TransactionDirection),
                                                                  tet.BuySellId.Value.ToString()),
                                                              (Types.FutureOpenCloseType)
                                                              Enum.Parse(
                                                                  typeof (
                                                                      Types.FutureOpenCloseType),
                                                                  tet.OpenCloseId.Value.ToString()));
                                item.EntrustAmount = tet.EntrustMount.Value;
                                item.Code = tet.ContractCode;
                                _conterCache.AddOrderMappingInfo(rde.OrderNo,
                                                                 item);
                            }
                            else
                            {
                                //废单
                                tet.OrderStatusId = (int) Types.OrderStateType.DOSCanceled;
                                //tet.EntrustNumber = "";
                                //委托状态信息 
                                tet.OrderMessage = rde.Message;
                            }
                        }
                        else
                            tet.OrderMessage = "GT-2120:[商品期货报盘]撮合中心返回结果异常";
                    }
                    //else
                    //tet.OrderMessage = "GT-2503 [商品期货报盘]与撮合中心通道异常";
                }
                else
                    tet.OrderMessage = "GT-2122:[商品期货报盘]代码未分配撮合中心异常";
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2123:[股指期货报盘]报盘未知异常" + ex;
                LogHelper.WriteError(ex.Message, ex);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(tet.OrderMessage))
                OrderOfferDataLogic.UpdateFutureOrder(tet);
            }
 * */

            #endregion
        }
        /// <summary>
        /// 真实股指期货报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoGZQHOrderOffer(object sender, QueueItemHandleEventArg<QhTodayEntrustTableEx> e)
        {
            DateTime t1 = DateTime.Now;
            QH_TodayEntrustTableInfo tet = e.Item.OriginalEntity;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(e.Item.OriginalEntity.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                string strMessage = string.Empty;

                //1.查找撮合中心信息
                if (mc == null)
                {
                    //IntelnalCancelXHOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2180:[股指期货报盘]交易代码未分配对应的撮合中心，委托作废，请检查管理中心相关的配置.";
                    IntelnalCancelGZQHOrder(tet, tet.OrderMessage);
                    LogHelper.WriteInfo(tet.OrderMessage);
                    goto EndProcess;
                }

                //2.第一次获取报盘通道
                DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                            mc.XiaDanService, mc.CuoHeService,
                                                                            ref cs, ref strMessage);

                //3.第二次获取报盘通道，当第一次失败时
                if (doc == null)
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                //4.两次获取报盘通道都不正常时，委托作废
                tet.OrderMessage = strMessage;
                if (cs != ChannelState.CSNormal || doc == null)
                {
                    IntelnalCancelGZQHOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2181:[股指期货报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启";
                    goto EndProcess;
                }

                var mcOrder = ConvertGZQH(e.Item);

                LogHelper.WriteDebug("-------->股指期货内部报盘OrderOfferCenter.DoGZQHOrderOffer，" + mcOrder.DescInfo());
                ResultDataEntity rde = null;
                try
                {
                    //5.第一次报盘
                    //FireDoXHOfferOrderEvent(mcOrder);
                    DateTime dt1 = DateTime.Now;
                    rde = doc.DoFutureOrder(mcOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoGZQHOfferOrderEvent(mcOrder, ts.TotalMilliseconds.ToString());
                }
                catch (CommunicationException ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    //6.获取报盘通道并第二次报盘，当第一次报盘失败时
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                    //7.当获取的报盘通道不正常时，委托作废
                    if (doc == null)
                    {
                        IntelnalCancelGZQHOrder(tet, tet.OrderMessage);
                        tet.OrderMessage = "GT-2182:[股指期货报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启.";

                        goto EndProcess;
                    }

                    DateTime dt1 = DateTime.Now;
                    rde = doc.DoFutureOrder(mcOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoGZQHOfferOrderEvent(mcOrder, ts.TotalMilliseconds.ToString());
                }

                if (rde == null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);

                    tet.OrderMessage = "GT-2183:[股指期货报盘]撮合中心返回结果异常，委托作废，请检查撮合服务是否正常运行.";
                    IntelnalCancelGZQHOrder(tet, tet.OrderMessage);

                    LogHelper.WriteInfo("rde=null," + tet.OrderMessage + ", 报盘对象" + mcOrder.DescInfo());
                    goto EndProcess;
                }

                if (rde.OrderNo.Length > 0)
                {
                    //已报
                    tet.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    //撮合返回的委托单号存储
                    tet.McOrderId = rde.OrderNo;

                    string format =
                        "OrderOfferCenter.DoGZQHOrderOffer添加委托单号映射到ConterCache[OrderNo={0},EntrustNumber={1}]";
                    string desc = string.Format(format, rde.OrderNo, tet.EntrustNumber);
                    LogHelper.WriteDebug(desc);

                    //柜台缓存委托相关信息
                    var item = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount,
                                                  tet.EntrustNumber,
                                                  (
                                                  GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                  Enum.Parse(
                                                      typeof(
                                                          GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                      tet.BuySellTypeId.ToString()),
                                                  (ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType)
                                                  Enum.Parse(
                                                      typeof(
                                                          ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType),
                                                      tet.OpenCloseTypeId.ToString()));
                    item.EntrustAmount = tet.EntrustAmount;
                    item.Code = tet.ContractCode;
                    item.TraderId = e.Item.TraderId;

                    if (e.Item.IsOpenMarketCheckOrder)
                    {
                        item.IsOpenMarketCheckOrder = true;
                    }

                    qhCounterCache.AddOrderMappingInfo(rde.OrderNo,
                                                     item);

                    //0812：根据算法交易要求，报盘成功也要推一次
                    ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject =
                        new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                    reckonEndObject.IsSuccess = true;
                    reckonEndObject.EntrustTable = tet;
                    reckonEndObject.TradeTableList = new List<QH_TodayTradeTableInfo>();
                    reckonEndObject.TradeID = e.Item.TraderId;
                    reckonEndObject.Message = "";

                    CounterOrderService.Instance.AcceptStockIndexFuturesDealOrder(reckonEndObject);
                }
                else
                {
                    IntelnalCancelGZQHOrder(tet, rde.Message);
                }

            EndProcess:
                //报盘结束
                ;
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2184:[股指期货报盘]无法报盘，委托作废，请检查撮合服务是否开启.";
                IntelnalCancelGZQHOrder(tet, tet.OrderMessage);
                LogHelper.WriteError(ex.Message, ex);

                if (mc != null)
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(tet.OrderMessage))
                OrderOfferDataLogic.UpdateFutureOrder(tet);
            }

#if(DEBUG)
            DateTime t2 = DateTime.Now;
            TimeSpan ts2 = t2 - t1;
            LogHelper.WriteDebug("报盘TotalTime=" + ts2.TotalMilliseconds);
#endif

            #region 旧报盘逻辑

            /*
            QhTodayEntrustTable tet = e.Item.OriginalEntity;
            try
            {
                RC_MatchCenter mc =
                    MCService.CommonPara.GetMatchCenterByCommodityCode(e.Item.OriginalEntity.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                string strMessage = string.Empty;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                                mc.XiaDanService, mc.CuoHeService,
                                                                                ref cs, ref strMessage);
                    if (doc == null)
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                      mc.XiaDanService, mc.CuoHeService,
                                                                      ref cs, ref strMessage);
                    tet.OrderMessage = strMessage;
                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = ConvertGZQH(e.Item);
                        ResultDataEntity rde = doc.DoFutureOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.OrderNo.Length > 0)
                            {
                                //已报
                                tet.OrderStatusId = (int) Types.OrderStateType.DOSIsRequired;
                                //委托单号存储
                                tet.McOrderId = rde.OrderNo;
                                LogHelper.WriteDebug("OrderOfferCenter.DoGZQHOrderOffer添加委托单号映射到ConterCache,OrderNo=" +
                                                     rde.OrderNo);

                                var item = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount,
                                                              tet.EntrustNumber,
                                                              (
                                                              CommonObject.Types.TransactionDirection
                                                              )
                                                              Enum.Parse(
                                                                  typeof (
                                                                      CommonObject.Types.TransactionDirection),
                                                                  tet.BuySellId.Value.ToString()),
                                                              (Types.FutureOpenCloseType)
                                                              Enum.Parse(
                                                                  typeof (
                                                                      Types.FutureOpenCloseType),
                                                                  tet.OpenCloseId.Value.ToString()));
                                item.EntrustAmount = tet.EntrustMount.Value;
                                item.Code = tet.ContractCode;
                                _conterCache.AddOrderMappingInfo(rde.OrderNo,
                                                                 item);
                            }
                            else
                            {
                                //废单
                                tet.OrderStatusId = (int) Types.OrderStateType.DOSCanceled;
                                //tet.EntrustNumber = "";
                                //委托状态信息 
                                tet.OrderMessage = rde.Message;
                            }
                        }
                        else
                            tet.OrderMessage = "GT-2127:[股指期货报盘]撮合中心返回结果异常";
                    }
                    //else
                    // tet.OrderMessage = "GT-2803 [股指期货报盘]与撮合中心通道异常";
                }
                else
                    tet.OrderMessage = "GT-2128:[股指期货报盘]代码未分配撮合中心异常";
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2129:[股指期货报盘]报盘未知异常" + ex;
                LogHelper.WriteError(ex.Message, ex);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(tet.OrderMessage))
                OrderOfferDataLogic.UpdateFutureOrder(tet);
            }
             * */

            #endregion
        }

        /// <summary>
        /// 股指期货强制平仓假报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoGZQHCloseHoldOrderOffer(object sender, QueueItemHandleEventArg<QhTodayEntrustTableEx> e)
        {
            QH_TodayEntrustTableInfo tet = e.Item.OriginalEntity;

            ResultDataEntity rde = null;
            try
            {
                var mcOrder = ConvertGZQH(e.Item);

                LogHelper.WriteDebug("-------->股指期货强制平仓假报盘OrderOfferCenter.DoGZQHCloseHoldOrderOffer，" +
                                     mcOrder.DescInfo());
                try
                {
                    //模拟报盘
                    rde = new ResultDataEntity();
                    rde.Id = Guid.NewGuid().ToString();
                    rde.OrderNo = Guid.NewGuid().ToString();
                    //显示报盘信息到窗体listbox上
                    FireDoGZQHOfferOrderEvent(mcOrder, "0");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                if (rde.OrderNo.Length > 0)
                {
                    //已报
                    tet.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    //撮合返回的委托单号存储
                    tet.McOrderId = rde.OrderNo;
                    LogHelper.WriteDebug("OrderOfferCenter.DoGZQHCloseHoldOrderOffer添加委托单号映射到ConterCache,OrderNo=" + rde.OrderNo);
                    //柜台缓存委托相关信息
                    var item = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount, tet.EntrustNumber,
                                                  (GTA.VTS.Common.CommonObject.Types.TransactionDirection)Enum.Parse(typeof(GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                      tet.BuySellTypeId.ToString()),
                                                  (ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType)Enum.Parse(typeof(ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType),
                                                      tet.OpenCloseTypeId.ToString()));
                    item.EntrustAmount = tet.EntrustAmount;
                    item.Code = tet.ContractCode;
                    item.TraderId = e.Item.TraderId;
                    item.IsOpenMarketCheckOrder = e.Item.IsOpenMarketCheckOrder;//标识是否为盘前检查强行平仓
                    item.QHForcedCloseType = e.Item.QHForcedCloseType;//盘前检查强行平仓类型

                    qhCounterCache.AddOrderMappingInfo(rde.OrderNo, item);
                }
                else
                {
                    IntelnalCancelGZQHOrder(tet, rde.Message);
                }
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2184:[股指期货报盘]无法报盘，委托作废，请检查撮合服务是否开启.";
                IntelnalCancelGZQHOrder(tet, tet.OrderMessage);
                LogHelper.WriteError(ex.Message, ex);
            }
            finally
            {
                OrderOfferDataLogic.UpdateFutureOrder(tet);
            }

            //传回成交回报
            FutureDealBackEntity model = new FutureDealBackEntity();
            model.DealAmount = e.Item.OriginalEntity.EntrustAmount;
            model.DealPrice = e.Item.OriginalEntity.EntrustPrice;
            model.DealTime = DateTime.Now;
            model.Id = rde.Id;
            model.OrderNo = rde.OrderNo;

            var call = new DoCallbackProcess();
            call.ProcessStockIndexFuturesDealRpt(model);
        }
        /// <summary>
        /// 商品期货强制平仓假报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoSPQHCloseHoldOrderOffer(object sender, QueueItemHandleEventArg<QhTodayEntrustTableEx> e)
        {
            QH_TodayEntrustTableInfo tet = e.Item.OriginalEntity;

            ResultDataEntity rde = null;
            try
            {
                var mcOrder = ConvertSPQH(e.Item);

                LogHelper.WriteDebug("-------->商品期货强制平仓假报盘OrderOfferCenter.DoSPQHCloseHoldOrderOffer，" + mcOrder.DescInfo());
                try
                {
                    //模拟报盘
                    rde = new ResultDataEntity();
                    rde.Id = Guid.NewGuid().ToString();
                    rde.OrderNo = Guid.NewGuid().ToString();

                    FireDoSPQHOfferOrderEvent(mcOrder, "0");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                if (rde.OrderNo.Length > 0)
                {
                    //已报
                    tet.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    //撮合返回的委托单号存储
                    tet.McOrderId = rde.OrderNo;
                    LogHelper.WriteDebug("OrderOfferCenter.DoGZQHCloseHoldOrderOffer添加委托单号映射到ConterCache,OrderNo=" + rde.OrderNo);
                    //柜台缓存委托相关信息
                    var item = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount, tet.EntrustNumber,
                                                  (GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                  Enum.Parse(typeof(GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                      tet.BuySellTypeId.ToString()),
                                                  (ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType)
                                                  Enum.Parse(typeof(ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType),
                                                      tet.OpenCloseTypeId.ToString()));
                    item.EntrustAmount = tet.EntrustAmount;
                    item.Code = tet.ContractCode;
                    item.TraderId = e.Item.TraderId;
                    item.IsOpenMarketCheckOrder = e.Item.IsOpenMarketCheckOrder;//标识是否为盘前检查强行平仓
                    item.QHForcedCloseType = e.Item.QHForcedCloseType;//盘前检查强行平仓类型

                    qhCounterCache.AddOrderMappingInfo(rde.OrderNo, item);
                }
                else
                {
                    IntelnalCancelSPQHOrder(tet, rde.Message);
                }
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2184:[商品期货报盘]无法报盘，委托作废，请检查撮合服务是否开启.";
                IntelnalCancelSPQHOrder(tet, tet.OrderMessage);
                LogHelper.WriteError(ex.Message, ex);
            }
            finally
            {
                OrderOfferDataLogic.UpdateFutureOrder(tet);
            }

            //传回成交回报
            CommoditiesDealBackEntity model = new CommoditiesDealBackEntity();
            model.DealAmount = e.Item.OriginalEntity.EntrustAmount;
            model.DealPrice = e.Item.OriginalEntity.EntrustPrice;
            model.DealTime = DateTime.Now;
            model.Id = rde.Id;
            model.OrderNo = rde.OrderNo;

            var call = new DoCallbackProcess();
            call.ProcessMercantileDealRpt(model);
        }

        /// <summary>
        /// 真实现货报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoXHOrderOffer(object sender, QueueItemHandleEventArg<XhTodayEntrustTableEx> e)
        {
            DateTime t1 = DateTime.Now;
            XH_TodayEntrustTableInfo tet = e.Item.OriginalEntity;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(e.Item.OriginalEntity.SpotCode);
                ChannelState cs = ChannelState.CSNormal;
                string strMessage = string.Empty;

                //1.查找撮合中心信息
                if (mc == null)
                {
                    //IntelnalCancelXHOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2140:[现货报盘]交易代码未分配对应的撮合中心，委托作废，请检查管理中心相关的配置.";
                    IntelnalCancelXHOrder(tet, tet.OrderMessage);
                    LogHelper.WriteDebug(tet.OrderMessage);
                    goto EndProcess;
                }

                //2.第一次获取报盘通道
                DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                            mc.XiaDanService, mc.CuoHeService,
                                                                            ref cs, ref strMessage);

                //3.第二次获取报盘通道，当第一次失败时
                if (doc == null)
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                //4.两次获取报盘通道都不正常时，委托作废
                tet.OrderMessage = strMessage;
                if (cs != ChannelState.CSNormal || doc == null)
                {
                    IntelnalCancelXHOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2141:[现货报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启";
                    goto EndProcess;
                }

                var mcOrder = ConvertStock(e.Item);

                LogHelper.WriteDebug("-------->现货内部报盘OrderOfferCenter.DoXHOrderOffer，" + mcOrder.DescInfo());
                ResultDataEntity rde = null;
                try
                {
                    //5.第一次报盘
                    //FireDoXHOfferOrderEvent(mcOrder);
                    DateTime dt1 = DateTime.Now;
                    rde = doc.DoStockOrder(mcOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoXHOfferOrderEvent(mcOrder, ts.TotalMilliseconds.ToString());
                }
                catch (CommunicationException ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    //6.获取报盘通道并第二次报盘，当第一次报盘失败时
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                    //7.当获取的报盘通道不正常时，委托作废
                    if (doc == null)
                    {
                        IntelnalCancelXHOrder(tet, tet.OrderMessage);
                        tet.OrderMessage = "GT-2142:[现货报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启.";

                        goto EndProcess;
                    }

                    DateTime dt1 = DateTime.Now;
                    rde = doc.DoStockOrder(mcOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoXHOfferOrderEvent(mcOrder, ts.TotalMilliseconds.ToString());
                }

                if (rde == null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);

                    tet.OrderMessage = "GT-2143:[现货报盘]撮合中心返回结果异常，委托作废，请检查撮合服务是否正常运行.";
                    IntelnalCancelXHOrder(tet, tet.OrderMessage);

                    LogHelper.WriteInfo("rde=null," + tet.OrderMessage + ", 报盘对象" + mcOrder.DescInfo());
                    goto EndProcess;
                }

                if (rde.OrderNo.Length > 0)
                {
                    //已报
                    tet.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    //撮合返回的委托单号存储
                    tet.McOrderId = rde.OrderNo;

                    string format = "OrderOfferCenter.DoXHOrderOffer添加委托单号映射到ConterCache[OrderNo={0},EntrustNumber={1}]";
                    string desc = string.Format(format, rde.OrderNo, tet.EntrustNumber);
                    LogHelper.WriteDebug(desc);

                    //柜台缓存委托相关信息
                    var item = new OrderCacheItem(tet.CapitalAccount, tet.StockAccount,
                                                  tet.EntrustNumber,
                                                  (
                                                  GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                  Enum.Parse(
                                                      typeof(
                                                          GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                      tet.BuySellTypeId.ToString
                                                          ()));
                    item.EntrustAmount = tet.EntrustAmount;
                    item.Code = tet.SpotCode;
                    item.TraderId = e.Item.TraderId;

                    xhCounterCache.AddOrderMappingInfo(rde.OrderNo,
                                                     item);

                    //0812：根据算法交易要求，报盘成功也要推一次
                    ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject =
                        new ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>();
                    reckonEndObject.IsSuccess = true;
                    reckonEndObject.EntrustTable = tet;
                    reckonEndObject.TradeTableList = new List<XH_TodayTradeTableInfo>();
                    reckonEndObject.TradeID = e.Item.TraderId;
                    reckonEndObject.Message = "";

                    CounterOrderService.Instance.AcceptStockDealOrder(reckonEndObject);
                }
                else
                {
                    IntelnalCancelXHOrder(tet, rde.Message);
                }

            EndProcess:
                //报盘结束
                ;
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2144:[现货报盘]无法报盘，委托作废，请检查撮合服务是否开启.";
                IntelnalCancelXHOrder(tet, tet.OrderMessage);
                LogHelper.WriteError(ex.Message, ex);

                if (mc != null)
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(tet.OrderMessage))
                OrderOfferDataLogic.UpdateStockOrder(tet);
            }

#if(DEBUG)
            DateTime t2 = DateTime.Now;
            TimeSpan ts2 = t2 - t1;
            LogHelper.WriteDebug("报盘TotalTime=" + ts2.TotalMilliseconds);
#endif
        }

        /// <summary>
        /// 真实港股报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoHKOrderOffer(object sender, QueueItemHandleEventArg<HkTodayEntrustEx> e)
        {
            DateTime t1 = DateTime.Now;
            HK_TodayEntrustInfo tet = e.Item.OriginalEntity;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(e.Item.OriginalEntity.Code);
                ChannelState cs = ChannelState.CSNormal;
                string strMessage = string.Empty;

                //1.查找撮合中心信息
                if (mc == null)
                {
                    //IntelnalCancelXHOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2195:[港股报盘]交易代码未分配对应的撮合中心，委托作废，请检查管理中心相关的配置.";
                    IntelnalCancelHKOrder(tet, tet.OrderMessage);
                    LogHelper.WriteDebug(tet.OrderMessage);
                    goto EndProcess;
                }

                //2.第一次获取报盘通道
                DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                            mc.XiaDanService, mc.CuoHeService,
                                                                            ref cs, ref strMessage);

                //3.第二次获取报盘通道，当第一次失败时
                if (doc == null)
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                //4.两次获取报盘通道都不正常时，委托作废
                tet.OrderMessage = strMessage;
                if (cs != ChannelState.CSNormal || doc == null)
                {
                    IntelnalCancelHKOrder(tet, tet.OrderMessage);

                    tet.OrderMessage = "GT-2194:[港股报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启";
                    goto EndProcess;
                }

                var mcOrder = ConvertHK(e.Item);

                LogHelper.WriteDebug("-------->港股内部报盘OrderOfferCenter.DoHKOrderOffer，" + mcOrder.DescInfo());
                ResultDataEntity rde = null;
                HKModifyOrderRequest modifyRequest;

                try
                {
                    //5.第一次报盘
                    //报盘前检查是否有改单
                    if (ModifyOrderProcessor.Instance.IsExistType1Request(tet.EntrustNumber, out modifyRequest))
                    {
                        IntelnalCancelHKOrder(tet, tet.OrderMessage);
                        tet.OrderMessage = "报盘前改单，当前委托作废.";

                        ModifyOrderProcessor.Instance.ProcessType1_NewOrder(modifyRequest);
                        ModifyOrderProcessor.Instance.DeleteType1Reqeust(tet.EntrustNumber);

                        goto EndProcess;
                    }

                    //FireDoXHOfferOrderEvent(mcOrder);
                    DateTime dt1 = DateTime.Now;
                    rde = doc.DoHKEntrustOrder(mcOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoHKOfferOrderEvent(mcOrder, ts.TotalMilliseconds.ToString());
                }
                catch (CommunicationException ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    //6.获取报盘通道并第二次报盘，当第一次报盘失败时
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                    //7.当获取的报盘通道不正常时，委托作废
                    if (doc == null)
                    {
                        IntelnalCancelHKOrder(tet, tet.OrderMessage);
                        tet.OrderMessage = "GT-2193:[港股报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启.";

                        goto EndProcess;
                    }

                    //报盘前第二次检查是否有改单
                    if (ModifyOrderProcessor.Instance.IsExistType1Request(tet.EntrustNumber, out modifyRequest))
                    {
                        IntelnalCancelHKOrder(tet, tet.OrderMessage);
                        tet.OrderMessage = "报盘前改单，当前委托作废.";

                        ModifyOrderProcessor.Instance.ProcessType1_NewOrder(modifyRequest);
                        ModifyOrderProcessor.Instance.DeleteType1Reqeust(tet.EntrustNumber);

                        goto EndProcess;
                    }

                    DateTime dt1 = DateTime.Now;
                    rde = doc.DoHKEntrustOrder(mcOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoHKOfferOrderEvent(mcOrder, ts.TotalMilliseconds.ToString());
                }

                if (rde == null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);

                    tet.OrderMessage = "GT-2192:[港股报盘]撮合中心返回结果异常，委托作废，请检查撮合服务是否正常运行.";
                    IntelnalCancelHKOrder(tet, tet.OrderMessage);

                    LogHelper.WriteInfo("rde=null," + tet.OrderMessage + ", 报盘对象" + mcOrder.DescInfo());
                    goto EndProcess;
                }
                LogHelper.WriteDebug("-------->港股内部报盘OrderOfferCenter.DoHKOrderOffer==撮合ID：" + rde.OrderNo);

                if (rde.OrderNo.Length > 0)
                {
                    //已报
                    tet.OrderStatusID = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    //撮合返回的委托单号存储
                    tet.McOrderID = rde.OrderNo;

                    string format = "OrderOfferCenter.DoHKOrderOffer添加委托单号映射到ConterCache[OrderNo={0},EntrustNumber={1}]";
                    string desc = string.Format(format, rde.OrderNo, tet.EntrustNumber);
                    LogHelper.WriteDebug(desc);

                    //柜台缓存委托相关信息
                    var item = new OrderCacheItem(tet.CapitalAccount, tet.HoldAccount,
                                                  tet.EntrustNumber,
                                                  (
                                                  GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                  Enum.Parse(
                                                      typeof(
                                                          GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                      tet.BuySellTypeID.ToString
                                                          ()));
                    item.EntrustAmount = tet.EntrustAmount;
                    item.Code = tet.Code;
                    item.TraderId = e.Item.TraderId;

                    hkCounterCache.AddOrderMappingInfo(rde.OrderNo,
                                                     item);

                    //0812：根据算法交易要求，报盘成功也要推一次
                    ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject =
                        new ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo>();
                    reckonEndObject.IsSuccess = true;
                    reckonEndObject.EntrustTable = tet;
                    reckonEndObject.TradeTableList = new List<HK_TodayTradeInfo>();
                    reckonEndObject.TradeID = e.Item.TraderId;
                    reckonEndObject.Message = "";

                    CounterOrderService.Instance.AcceptHKDealOrder(reckonEndObject);
                }
                else
                {
                    IntelnalCancelHKOrder(tet, rde.Message);
                }

            EndProcess:
                //报盘结束
                ;
            }
            catch (Exception ex)
            {
                tet.OrderMessage = "GT-2191:[港股报盘]无法报盘，委托作废，请检查撮合服务是否开启.";
                IntelnalCancelHKOrder(tet, tet.OrderMessage);
                LogHelper.WriteError(ex.Message, ex);

                if (mc != null)
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(tet.OrderMessage))
                OrderOfferDataLogic.UpdateHKOrder(tet);
            }

#if(DEBUG)
            DateTime t2 = DateTime.Now;
            TimeSpan ts2 = t2 - t1;
            LogHelper.WriteDebug("报盘TotalTime=" + ts2.TotalMilliseconds);
#endif
        }


        /// <summary>
        /// 真实港股改单报盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoHKModifyOrderOffer(object sender, QueueItemHandleEventArg<HKModifyOrderRequest> e)
        {
            DateTime t1 = DateTime.Now;
            HKModifyOrderRequest hkOrder = e.Item;
            RC_MatchCenter mc = null;
            string strMessage = string.Empty;

            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(hkOrder.Code);
                ChannelState cs = ChannelState.CSNormal;

                //1.查找撮合中心信息
                if (mc == null)
                {
                    //IntelnalCancelXHOrder(tet, tet.OrderMessage);

                    strMessage = "GT-2195:[港股报盘]交易代码未分配对应的撮合中心，委托作废，请检查管理中心相关的配置.";
                    LogHelper.WriteDebug(strMessage);

                    DoHKModifyOrderOffer_FailureProcess(hkOrder, strMessage);

                    goto EndProcess;
                }

                //2.第一次获取报盘通道
                DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                            mc.XiaDanService, mc.CuoHeService,
                                                                            ref cs, ref strMessage);

                //3.第二次获取报盘通道，当第一次失败时
                if (doc == null)
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                //4.两次获取报盘通道都不正常时，委托作废
                if (cs != ChannelState.CSNormal || doc == null)
                {
                    strMessage = "GT-2194:[港股报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启";
                    LogHelper.WriteDebug(strMessage);

                    DoHKModifyOrderOffer_FailureProcess(hkOrder, strMessage);

                    goto EndProcess;
                }

                HKModifyEntity dataOrder = ConvertHKModifyOrder(hkOrder);

                LogHelper.WriteDebug("-------->港股改单内部报盘OrderOfferCenter.DoHKModifyOrderOffer，" + hkOrder);
                HKModifyResultEntity rde = null;
                //HKModifyOrderRequest modifyRequest;

                try
                {
                    //5.第一次报盘
                    //FireDoXHOfferOrderEvent(mcOrder);
                    DateTime dt1 = DateTime.Now;
                    rde = doc.ModifyHKStockOrder(dataOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoHKModifyOfferOrderEvent(dataOrder, ts.TotalMilliseconds.ToString());
                }
                catch (CommunicationException ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    //6.获取报盘通道并第二次报盘，当第一次报盘失败时
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
                    doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                  mc.XiaDanService, mc.CuoHeService,
                                                                  ref cs, ref strMessage);
                    //7.当获取的报盘通道不正常时，委托作废
                    if (doc == null)
                    {
                        strMessage = "GT-2193:[港股报盘]无法与撮合中心建立报盘通道，委托作废，请检查撮合服务是否开启.";
                        LogHelper.WriteDebug(strMessage);

                        DoHKModifyOrderOffer_FailureProcess(hkOrder, strMessage);

                        goto EndProcess;
                    }


                    DateTime dt1 = DateTime.Now;
                    rde = doc.ModifyHKStockOrder(dataOrder);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    FireDoHKModifyOfferOrderEvent(dataOrder, ts.TotalMilliseconds.ToString());
                }

                if (rde == null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);

                    strMessage = "GT-2192:[港股报盘]撮合中心返回结果异常，委托作废，请检查撮合服务是否正常运行.";
                    LogHelper.WriteDebug(strMessage);

                    DoHKModifyOrderOffer_FailureProcess(hkOrder, strMessage);

                    goto EndProcess;
                }

                if (rde.OrderNo.Length > 0)
                {
                    //HKModifyOrderPushBack pushBack = new HKModifyOrderPushBack();
                    //pushBack.IsSuccess = true;
                    //pushBack.Message = strMessage;
                    //pushBack.OriginalRequestNumber = hkOrder.EntrustNubmer;
                    //pushBack.TradeID = hkOrder.TraderId;
                    //pushBack.CallbackChannlId = hkOrder.ChannelID;

                    //CounterOrderService.Instance.AcceptHKModifyOrder(pushBack);

                    LogHelper.WriteDebug("OrderOfferCenter.DoHKModifyOrderoffer港股类型2改单报盘成功" + hkOrder);

                    //报盘成功，添加到类型2缓存中
                    ModifyOrderProcessor.Instance.AddType2Reqest(hkOrder);
                }
                else
                {
                    strMessage = "GT-2192:[港股报盘]撮合中心返回结果异常，委托作废，请检查撮合服务是否正常运行.";
                    LogHelper.WriteDebug(strMessage);

                    DoHKModifyOrderOffer_FailureProcess(hkOrder, strMessage);
                }

            EndProcess:
                //报盘结束
                ;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2191:[港股报盘]无法报盘，委托作废，请检查撮合服务是否开启.";
                LogHelper.WriteError(ex.Message, ex);

                DoHKModifyOrderOffer_FailureProcess(hkOrder, strMessage);


                if (mc != null)
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(tet.OrderMessage))
                //OrderOfferDataLogic.UpdateHKOrder(tet);
            }

#if(DEBUG)
            DateTime t2 = DateTime.Now;
            TimeSpan ts2 = t2 - t1;
            LogHelper.WriteDebug("报盘TotalTime=" + ts2.TotalMilliseconds);
#endif
        }

        /// <summary>
        /// 转换改单委托
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private HKModifyEntity ConvertHKModifyOrder(HKModifyOrderRequest request)
        {
            var result = new HKModifyEntity();
            //通道ID
            result.ChannelNo = COUNT_CLIENT_ID;

            result.ModifyVolume = (int)request.OrderAmount;

            var tet = HKDataAccess.GetTodayEntrustTable(request.EntrustNubmer);
            result.OldOrderNo = tet.McOrderID;

            result.StockCode = request.Code;

            return result;
        }

        /// <summary>
        /// 港股改单报盘失败处理
        /// </summary>
        /// <param name="hkOrder"></param>
        /// <param name="strMessage"></param>
        private void DoHKModifyOrderOffer_FailureProcess(HKModifyOrderRequest hkOrder, string strMessage)
        {
            HKModifyOrderPushBack pushBack = new HKModifyOrderPushBack();
            pushBack.Message = strMessage;
            pushBack.OriginalRequestNumber = hkOrder.EntrustNubmer;
            pushBack.TradeID = hkOrder.TraderId;
            pushBack.CallbackChannlId = hkOrder.ChannelID;

            CounterOrderService.Instance.AcceptHKModifyOrder(pushBack);
            hkOrder.Message = strMessage;
            HKDataAccess.UpdateModifyOrderRequest(hkOrder);
        }

        private void FireDoXHOfferOrderEvent(StockOrderEntity entity)
        {
            if (DoOfferOrderEvent != null)
            {
                string format =
                    "现货报盘[代码={0},方向={1},价格={2},下单量={3},是否市价单={4},报盘时间={5}]";
                string desc = String.Format(format, entity.StockCode, entity.TransactionDirection, entity.OrderPrice,
                                            entity.OrderVolume, entity.IsMarketPrice, DateTime.Now);
                RuntimeMessageEventArge args = new RuntimeMessageEventArge(desc);
                DoOfferOrderEvent(this, args);
            }
        }

        private void FireDoXHOfferOrderEvent(StockOrderEntity entity, string timespan)
        {
            if (DoOfferOrderEvent != null)
            {
                string format =
                    "现货报盘[代码={0},方向={1},价格={2},下单量={3},是否市价单={4},报盘时间={5},SendTime={6}]";
                string desc = String.Format(format, entity.StockCode, entity.TransactionDirection, entity.OrderPrice,
                                            entity.OrderVolume, entity.IsMarketPrice, DateTime.Now, timespan);
                RuntimeMessageEventArge args = new RuntimeMessageEventArge(desc);
                DoOfferOrderEvent(this, args);
            }
        }

        private void FireDoHKOfferOrderEvent(HKOrderEntity entity, string timespan)
        {
            if (DoOfferOrderEvent != null)
            {
                string format =
                    "港股报盘[代码={0},方向={1},价格={2},下单量={3},委托类型={4},报盘时间={5},SendTime={6}]";
                string desc = String.Format(format, entity.Code, entity.TransactionDirection, entity.OrderPrice,
                                            entity.OrderVolume, entity.HKPriceType, DateTime.Now, timespan);
                RuntimeMessageEventArge args = new RuntimeMessageEventArge(desc);
                DoOfferOrderEvent(this, args);
            }
        }

        private void FireDoHKModifyOfferOrderEvent(HKModifyEntity entity, string timespan)
        {
            if (DoOfferOrderEvent != null)
            {
                string format =
                    "港股改单报盘[代码={0},原始委托={1},新委托量={2},报盘时间={3},SendTime={4}]";
                string desc = String.Format(format, entity.StockCode, entity.OldOrderNo, entity.ModifyVolume
                                            , DateTime.Now, timespan);
                RuntimeMessageEventArge args = new RuntimeMessageEventArge(desc);
                DoOfferOrderEvent(this, args);
            }
        }

        private void FireDoSPQHOfferOrderEvent(CommoditiesOrderEntity entity, string timespan)
        {
            if (DoOfferOrderEvent != null)
            {
                string format =
                    "商品期货报盘[代码={0},方向={1},价格={2},下单量={3},是否市价单={4},报盘时间={5},SendTime={6}]";
                string desc = String.Format(format, entity.StockCode, entity.TransactionDirection, entity.OrderPrice,
                                            entity.OrderVolume, entity.IsMarketPrice, DateTime.Now, timespan);
                RuntimeMessageEventArge args = new RuntimeMessageEventArge(desc);
                DoOfferOrderEvent(this, args);
            }
        }

        private void FireDoGZQHOfferOrderEvent(FutureOrderEntity entity, string timespan)
        {
            if (DoOfferOrderEvent != null)
            {
                string format =
                    "股指期货报盘[代码={0},方向={1},价格={2},下单量={3},是否市价单={4},报盘时间={5},SendTime={6}]";
                string desc = String.Format(format, entity.StockCode, entity.TransactionDirection, entity.OrderPrice,
                                            entity.OrderVolume, entity.IsMarketPrice, DateTime.Now, timespan);
                RuntimeMessageEventArge args = new RuntimeMessageEventArge(desc);
                DoOfferOrderEvent(this, args);
            }
        }



        #endregion

        #region 分发委托

        /// <summary>
        /// 分发现货委托
        /// </summary>
        /// <param name="stockOrder"></param>
        private bool DispatchStockOrder(XhTodayEntrustTableEx stockOrder)
        {
            if (stockOrder == null)
            {
                LogHelper.WriteInfo("OrderOfferCenter.DispatchStockOrder委托为空.");
                return false;
            }

            ExecuteXHThreadWork(stockOrder);
            return true;

            #region 旧分发逻辑

            //string strMessage = string.Empty;
            //try
            //{
            //    QueueBufferBase<XhTodayEntrustTableEx> cache = null;
            //    foreach (var queueBufferBase in _stockQueueList)
            //    {
            //        if (cache == null || queueBufferBase.BufferedItemCount < cache.BufferedItemCount)
            //            cache = queueBufferBase;
            //    }
            //    if (cache == null)
            //    {
            //        LogHelper.WriteDebug("OrderOfferCenter.DispatchStockOrder无法找到报盘队列，分发现货委托失败.");
            //        return false;
            //    }

            //    cache.InsertQueueItem(stockOrder);
            //    LogHelper.WriteDebug("OrderOfferCenter.DispatchStockOrder分发现货委托[OfferBuffer=" + cache.Name + "]");
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    strMessage = "GT-2130:[现货报盘]分发现货委托异常," + ex.Message;
            //    LogHelper.WriteError(strMessage, ex);

            //    return false;
            //}

            #endregion
        }

        private void ExecuteXHThreadWork(XhTodayEntrustTableEx stockOrder)
        {
            try
            {
                smartPool.QueueWorkItem(DoXHOrderOffer, this,
                                        new QueueItemHandleEventArg<XhTodayEntrustTableEx>(stockOrder));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("OrderOfferCenter.ExecuteXHThreadWork", ex);
                DoXHOrderOffer(this, new QueueItemHandleEventArg<XhTodayEntrustTableEx>(stockOrder));
            }
        }

        /// <summary>
        /// 分发港股委托
        /// </summary>
        /// <param name="stockOrder"></param>
        private bool DispatchHKOrder(HkTodayEntrustEx stockOrder)
        {
            if (stockOrder == null)
            {
                LogHelper.WriteInfo("OrderOfferCenter.DispatchHKOrder委托为空.");
                return false;
            }

            ExecuteHKThreadWork(stockOrder);
            return true;
        }

        private void ExecuteHKThreadWork(HkTodayEntrustEx stockOrder)
        {
            try
            {
                smartPool.QueueWorkItem(DoHKOrderOffer, this,
                                        new QueueItemHandleEventArg<HkTodayEntrustEx>(stockOrder));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("OrderOfferCenter.ExecuteHKThreadWork", ex);
                DoHKOrderOffer(this, new QueueItemHandleEventArg<HkTodayEntrustEx>(stockOrder));
            }
        }

        /// <summary>
        /// 分发港股改单委托
        /// </summary>
        /// <param name="hkOrder"></param>
        private bool DispatchHKModifyOrder(HKModifyOrderRequest hkOrder)
        {
            if (hkOrder == null)
            {
                LogHelper.WriteInfo("OrderOfferCenter.DispatchHKModifyOrder委托为空.");
                return false;
            }

            ExecuteHKModifyThreadWork(hkOrder);
            return true;
        }

        private void ExecuteHKModifyThreadWork(HKModifyOrderRequest hkOrder)
        {
            try
            {
                smartPool.QueueWorkItem(DoHKModifyOrderOffer, this,
                                        new QueueItemHandleEventArg<HKModifyOrderRequest>(hkOrder));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("OrderOfferCenter.ExecuteHKModifyThreadWork", ex);
                DoHKModifyOrderOffer(this, new QueueItemHandleEventArg<HKModifyOrderRequest>(hkOrder));
            }
        }

        /// <summary>
        /// 分发商品期货委托
        /// </summary>
        /// <param name="futuresorder"></param>
        private bool DispatchSPQHOrder(QhTodayEntrustTableEx futuresorder)
        {
            if (futuresorder == null)
            {
                LogHelper.WriteInfo("OrderOfferCenter.DispatchMercantileFuturesOrder委托为空.");

                return false;
            }

            ExecuteSPQHThreadWork(futuresorder);
            return true;

            #region 旧分发逻辑

            //string strMessage = string.Empty;
            //try
            //{
            //    QueueBufferBase<QhTodayEntrustTableEx> cache = null;
            //    foreach (var queueBufferBase in _mercantileFutureQueueList)
            //    {
            //        if (cache == null || queueBufferBase.BufferedItemCount < cache.BufferedItemCount)
            //            cache = queueBufferBase;
            //    }
            //    if (cache == null)
            //    {
            //        LogHelper.WriteDebug("OrderOfferCenter.DispatchMercantileFuturesOrder无法找到报盘队列，分发商品期货委托失败.");

            //        return false;
            //    }

            //    cache.InsertQueueItem(futuresorder);
            //}
            //catch (Exception ex)
            //{
            //    strMessage = "GT-2131:[现货报盘]分发商品期货委托异常," + ex.Message;
            //    LogHelper.WriteError(strMessage, ex);
            //}

            #endregion
        }

        private void ExecuteSPQHThreadWork(QhTodayEntrustTableEx futuresorder)
        {
            try
            {
                smartPool.QueueWorkItem(DoSPQHOrderOffer, this,
                                        new QueueItemHandleEventArg<QhTodayEntrustTableEx>(futuresorder));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("OrderOfferCenter.ExecuteSPQHThreadWork", ex);
                DoSPQHOrderOffer(this, new QueueItemHandleEventArg<QhTodayEntrustTableEx>(futuresorder));
            }
        }

        /// <summary>
        /// 分发股指期货委托
        /// </summary>
        /// <param name="futuresorder"></param>
        private bool DispatchGZQHOrder(QhTodayEntrustTableEx futuresorder)
        {
            if (futuresorder == null)
            {
                LogHelper.WriteInfo("OrderOfferCenter.DispatchStockIndexFuturesOrder委托为空.");

                return false;
            }

            ExecuteGZQHThreadWork(futuresorder);
            return true;

            #region 旧分发逻辑

            //string strMessage = string.Empty;
            //try
            //{
            //    QueueBufferBase<QhTodayEntrustTableEx> cache = null;
            //    foreach (var queueBufferBase in _stockIndexFuturesFutureQueueList)
            //    {
            //        if (cache == null || queueBufferBase.BufferedItemCount < cache.BufferedItemCount)
            //            cache = queueBufferBase;
            //    }
            //    if (cache == null)
            //    {
            //        LogHelper.WriteDebug("OrderOfferCenter.DispatchStockIndexFuturesOrder无法找到报盘队列，分发股指期货委托失败.");

            //        return false;
            //    }

            //    cache.InsertQueueItem(futuresorder);
            //}
            //catch (Exception ex)
            //{
            //    strMessage = "GT-2132:[现货报盘]分发股指期货委托异常," + ex.Message;
            //    LogHelper.WriteError(strMessage, ex);
            //}

            #endregion
        }

        private void ExecuteGZQHThreadWork(QhTodayEntrustTableEx futuresorder)
        {
            try
            {
                smartPool.QueueWorkItem(DoGZQHOrderOffer, this,
                                        new QueueItemHandleEventArg<QhTodayEntrustTableEx>(futuresorder));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("OrderOfferCenter.ExecuteSPQHThreadWork", ex);
                DoGZQHOrderOffer(this, new QueueItemHandleEventArg<QhTodayEntrustTableEx>(futuresorder));
            }
        }

        #endregion

        #region 外部报盘

        /// <summary>
        /// 现货委托报盘
        /// </summary>
        /// <param name="stockOrder"></param>
        /// <returns></returns>
        public bool OfferStockOrder(XhTodayEntrustTableEx stockOrder)
        {
            LogHelper.WriteDebug("------>现货委托外部报盘OrderOfferCenter.OfferStockOrder," + stockOrder.OriginalRequest);
            if (stockOrder != null)
            {
                if (!ValidateCenter.IsMatchTradingTime(stockOrder.OriginalEntity.SpotCode))
                {
                    return _orderCache.CacheStockOrder(stockOrder);
                }
                else
                {
                    return DispatchStockOrder(stockOrder);
                }
            }

            return false;
        }

        /// <summary>
        /// 港股委托报盘
        /// </summary>
        /// <param name="hkOrder"></param>
        /// <returns></returns>
        public bool OfferHKOrder(HkTodayEntrustEx hkOrder)
        {
            LogHelper.WriteDebug("------>港股委托外部报盘OrderOfferCenter.OfferHKOrder," + hkOrder.OriginalRequest);
            if (hkOrder != null)
            {
                if (
                    !ValidateCenter.IsMatchTradingTime(GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.HKStock,
                                                       hkOrder.OriginalEntity.Code))
                {
                    return _orderCache.CacheHKOrder(hkOrder);
                }
                else
                {
                    return DispatchHKOrder(hkOrder);
                }
            }

            return false;
        }

        /// <summary>
        /// 港股委托报盘
        /// </summary>
        /// <param name="hkOrder"></param>
        /// <returns></returns>
        public bool OfferHKModifyOrder(HKModifyOrderRequest hkOrder)
        {
            LogHelper.WriteDebug("------>港股改单委托外部报盘OrderOfferCenter.OfferHKModifyOrder," + hkOrder);
            if (hkOrder != null)
            {
                return DispatchHKModifyOrder(hkOrder);
            }

            return false;
        }

        /// <summary>
        /// 商品期货委托报盘
        /// </summary>
        /// <param name="futuresorder"></param>
        /// <returns></returns>
        public void OfferSPQHOrder(QhTodayEntrustTableEx futuresorder)
        {
            LogHelper.WriteDebug("------>商品期货委托外部报盘OrderOfferCenter.OfferSPQHOrder," + futuresorder.OriginalEntity);

            if (futuresorder != null)
            {
                if (!ValidateCenter.IsMatchTradingTime(futuresorder.OriginalEntity.ContractCode))
                {
                    _orderCache.CacheMercantileFuturesOrder(futuresorder);
                }
                else
                {
                    DispatchSPQHOrder(futuresorder);
                }
            }
        }

        /// <summary>
        /// 股指期货委托报盘
        /// </summary>
        /// <param name="futuresorder"></param>
        /// <returns></returns>
        public void OfferGZQHOrder(QhTodayEntrustTableEx futuresorder)
        {
            LogHelper.WriteDebug("------>股指期货委托外部报盘OrderOfferCenter.OfferGZQHOrder," + futuresorder.OriginalEntity);

            if (futuresorder != null)
            {
                if (!ValidateCenter.IsMatchTradingTime(futuresorder.OriginalEntity.ContractCode))
                {
                    _orderCache.CacheStockIndexFuturesOrder(futuresorder);
                }
                else
                {
                    DispatchGZQHOrder(futuresorder);
                }
            }
        }

        /// <summary>
        /// 股指期货过期的合约需要强制平仓的委托进行内部假报盘，直接以结算价成交
        /// </summary>
        /// <param name="futuresorder"></param>
        public void OfferCloseHoldGZQHOrder(QhTodayEntrustTableEx futuresorder)
        {
            LogHelper.WriteDebug("------>股指期货委托强制平仓假报盘OrderOfferCenter.OfferCloseHoldGZQHOrder," + futuresorder.OriginalEntity);

            DoGZQHCloseHoldOrderOffer(this, new QueueItemHandleEventArg<QhTodayEntrustTableEx>(futuresorder));
        }

        /// <summary>
        /// 商品期货过期的合约需要强制平仓的委托进行内部假报盘，直接以结算价成交
        /// </summary>
        /// <param name="futuresorder"></param>
        public void OfferCloseHoldSPQHOrder(QhTodayEntrustTableEx futuresorder)
        {
            LogHelper.WriteDebug("------>股指期货委托强制平仓假报盘OrderOfferCenter.OfferCloseHoldGZQHOrder," + futuresorder.OriginalEntity);

            DoSPQHCloseHoldOrderOffer(this, new QueueItemHandleEventArg<QhTodayEntrustTableEx>(futuresorder));
        }

        /// <summary>
        /// 现货撤单报盘
        /// </summary>
        /// <param name="xhOrder"></param>
        /// <param name="strMessage"></param>
        /// <param name="ost"></param>
        /// <returns></returns>
        public bool CancelStockOrder(XH_TodayEntrustTableInfo xhOrder, ref string strMessage, out ReckoningCounter.Entity.Contants.Types.OrderStateType ost)
        {
            LogHelper.WriteDebug("------>开始现货撤单报盘OrderOfferCenter.CancelStockOrder,委托单号=" + xhOrder.EntrustNumber);
            bool result = false;
            ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.None;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(xhOrder.SpotCode);
                ChannelState cs = ChannelState.CSNormal;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                                mc.XiaDanService, mc.CuoHeService,
                                                                                ref cs, ref strMessage);
                    if (doc == null)
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                      mc.XiaDanService, mc.CuoHeService, ref cs,
                                                                      ref strMessage);

                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = new CancelEntity();
                        mcOrder.OldOrderNo = xhOrder.McOrderId;
                        mcOrder.StockCode = xhOrder.SpotCode;
                        mcOrder.ChannelNo = COUNT_CLIENT_ID;

                        var rde = doc.CancelStockOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.IsSuccess)
                            {
                                LogHelper.WriteDebug("------>现货撤单报盘成功OrderOfferCenter.CancelStockOrder，委托单号=" +
                                                     xhOrder.EntrustNumber);
                                if (xhOrder.OrderStatusId == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealed)
                                {
                                    xhOrder.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateStockOrderStatus_Cancel(xhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                }
                                else if (xhOrder.OrderStatusId == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired)
                                {
                                    xhOrder.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateStockOrderStatus_Cancel(xhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                }
                                else
                                {
                                    result = false;
                                    strMessage = "GT-2150:[现货撤单报盘]委托单状态异常,委托单状态不可撤";
                                }

                                //0827：撤单报盘成功也要推一次
                                string traderId = "";
                                var user = AccountManager.Instance.GetUserByAccount(xhOrder.CapitalAccount);
                                if (user != null)
                                {
                                    traderId = user.UserID;
                                }
                                //else
                                //{
                                //    traderId = xhCounterCache.GetTraderIdByFundAccount(xhOrder.CapitalAccount);
                                //    //TODO:需要根据资金账户获取用户ID
                                //}

                                ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject =
                                    new ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>();
                                reckonEndObject.IsSuccess = true;
                                reckonEndObject.EntrustTable = xhOrder;
                                reckonEndObject.TradeTableList = new List<XH_TodayTradeTableInfo>();
                                reckonEndObject.TradeID = traderId;
                                reckonEndObject.Message = "";

                                CounterOrderService.Instance.AcceptStockDealOrder(reckonEndObject);
                            }
                            else
                            {
                                LogHelper.WriteInfo("------>现货撤单报盘失败OrderOfferCenter.CancelStockOrder，委托单号=" +
                                                    xhOrder.EntrustNumber + ",Message=" + rde.Message);
                                result = false;
                                strMessage = rde.Message;
                            }
                        }
                        else
                            strMessage = "GT-2151:[现货撤单报盘]报盘返回结果异常";
                    }
                    //else
                    // strMessage = "GT-2308 [现货撤单报盘]报盘通道异常";
                }
                else
                    strMessage = "GT-2152:[现货撤单报盘]代码未分配撮合中心异常";
            }
            catch (Exception ex)
            {
                result = false;
                strMessage = "GT-2153:[现货撤单报盘]未知异常," + ex.Message;
                if (mc != null)
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(),
                                                                     mc.XiaDanService, mc.CuoHeService);
            }

            LogHelper.WriteDebug("------>结束现货撤单报盘OrderOfferCenter.CancelStockOrder，委托单号=" +
                                 xhOrder.EntrustNumber + ",最终消息=" + strMessage);
            return result;
        }

        /// <summary>
        /// 港股撤单报盘
        /// </summary>
        /// <param name="xhOrder"></param>
        /// <param name="strMessage"></param>
        /// <param name="ost"></param>
        /// <returns></returns>
        public bool CancelHKOrder(HK_TodayEntrustInfo xhOrder, ref string strMessage, out ReckoningCounter.Entity.Contants.Types.OrderStateType ost)
        {
            LogHelper.WriteDebug("------>开始港股撤单报盘OrderOfferCenter.CancelHKOrder,委托单号=" + xhOrder.EntrustNumber);
            bool result = false;
            ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.None;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(xhOrder.Code);
                ChannelState cs = ChannelState.CSNormal;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);
                    if (doc == null)
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);

                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = new CancelEntity();
                        mcOrder.OldOrderNo = xhOrder.McOrderID;
                        mcOrder.StockCode = xhOrder.Code;
                        mcOrder.ChannelNo = COUNT_CLIENT_ID;

                        var rde = doc.CancelHKOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.IsSuccess)
                            {
                                LogHelper.WriteDebug("------>港股撤单报盘成功OrderOfferCenter.CancelHKOrder，委托单号=" +
                                                     xhOrder.EntrustNumber + "  撮合单号=" + xhOrder.McOrderID);
                                if (xhOrder.OrderStatusID == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealed)
                                {
                                    xhOrder.OrderStatusID = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateHKOrderStatus_Cancel(xhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                }
                                else if (xhOrder.OrderStatusID == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired)
                                {
                                    xhOrder.OrderStatusID = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateHKOrderStatus_Cancel(xhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                }
                                else
                                {
                                    result = false;
                                    strMessage = "GT-2199:[港股撤单报盘]委托单状态异常,委托单状态不可撤";
                                }

                                //0827：撤单报盘成功也要推一次
                                string traderId = "";
                                var user = AccountManager.Instance.GetUserByAccount(xhOrder.CapitalAccount);
                                if (user != null)
                                {
                                    traderId = user.UserID;
                                }
                                //else
                                //{
                                //    traderId = hkCounterCache.GetTraderIdByFundAccount(xhOrder.CapitalAccount);
                                //    //TODO:需要根据资金账户获取用户ID
                                //}

                                ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject =
                                    new ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo>();
                                reckonEndObject.IsSuccess = true;
                                reckonEndObject.EntrustTable = xhOrder;
                                reckonEndObject.TradeTableList = new List<HK_TodayTradeInfo>();
                                reckonEndObject.TradeID = traderId;
                                reckonEndObject.Message = "";

                                CounterOrderService.Instance.AcceptHKDealOrder(reckonEndObject);
                            }
                            else
                            {
                                LogHelper.WriteInfo("------>港股撤单报盘失败OrderOfferCenter.CancelHKOrder，委托单号=" + xhOrder.EntrustNumber + ",Message=" + rde.Message);
                                result = false;
                                strMessage = rde.Message;
                            }
                        }
                        else
                            strMessage = "GT-2198:[港股撤单报盘]报盘返回结果异常";
                    }
                    //else
                    // strMessage = "GT-2308 [现货撤单报盘]报盘通道异常";
                }
                else
                    strMessage = "GT-2197:[港股撤单报盘]代码未分配撮合中心异常";
            }
            catch (Exception ex)
            {
                result = false;
                strMessage = "GT-2196:[港股撤单报盘]未知异常," + ex.Message;
                if (mc != null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService);
                }
            }

            LogHelper.WriteDebug("------>结束港股撤单报盘OrderOfferCenter.CancelHKOrder，委托单号=" + xhOrder.EntrustNumber + "  撮合单号=" + xhOrder.McOrderID + ",最终消息=" + strMessage);
            return result;
        }


        /// <summary>
        /// 商品期货撤单报盘
        /// </summary>
        /// <param name="qhOrder"></param>
        /// <param name="strMessage"></param>
        /// <param name="ost">撤单状态</param>
        /// <returns></returns>
        public bool CancelSPQHOrder(QH_TodayEntrustTableInfo qhOrder, ref string strMessage, out ReckoningCounter.Entity.Contants.Types.OrderStateType ost)
        {
            LogHelper.WriteDebug("------>开始商品期货撤单报盘OrderOfferCenter.CancelSPQHOrder,委托单号=" + qhOrder.EntrustNumber);
            bool result = false;
            ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.None;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(qhOrder.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);
                    if (doc == null)
                    {
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);
                    }

                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = new CancelEntity();
                        mcOrder.OldOrderNo = qhOrder.McOrderId;
                        mcOrder.StockCode = qhOrder.ContractCode;
                        mcOrder.ChannelNo = COUNT_CLIENT_ID;

                        var rde = doc.CancelCommoditiesOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.IsSuccess)
                            {
                                LogHelper.WriteDebug("------>商品期货撤单报盘成功OrderOfferCenter.CancelSPQHOrder，委托单号=" + qhOrder.EntrustNumber);

                                if (qhOrder.OrderStatusId == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealed)
                                {
                                    qhOrder.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateFutureOrderStatus_Cancel(qhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                }
                                else if (qhOrder.OrderStatusId == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired)
                                {
                                    qhOrder.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateFutureOrderStatus_Cancel(qhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                }
                                else
                                {
                                    result = false;
                                    strMessage = "GT-2170:[商品期货撤单报盘]委托单状态异常,委托单状态不可撤";
                                }

                                //0827：撤单报盘成功也要推一次
                                string traderId = "";
                                var user = AccountManager.Instance.GetUserByAccount(qhOrder.CapitalAccount);
                                if (user != null)
                                {
                                    traderId = user.UserID;
                                }
                                //else
                                //{
                                //    traderId = qhCounterCache.GetTraderIdByFundAccount(qhOrder.CapitalAccount);
                                //    //TODO:需要根据资金账户获取用户ID
                                //}

                                ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject =
                                    new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                                reckonEndObject.IsSuccess = true;
                                reckonEndObject.EntrustTable = qhOrder;
                                reckonEndObject.TradeTableList = new List<QH_TodayTradeTableInfo>();
                                reckonEndObject.TradeID = traderId;
                                reckonEndObject.Message = "";

                                //TODO:回推商品期货撤单
                                CounterOrderService.Instance.AcceptMercantileFuturesOrderDealOrder(reckonEndObject);
                            }
                            else
                            {
                                LogHelper.WriteInfo("------>商品期货撤单报盘失败OrderOfferCenter.CancelSPQHOrder，委托单号=" + qhOrder.EntrustNumber + ",Message=" + rde.Message);
                                result = false;
                                strMessage = rde.Message;
                            }
                        }
                        else
                        {
                            strMessage = "GT-2171:[商品期货撤单报盘]报盘返回结果异常";
                        }
                    }
                    //else
                    // strMessage = "GT-2308 [现货撤单报盘]报盘通道异常";
                }
                else
                {
                    strMessage = "GT-2172:[商品期货撤单报盘]代码未分配撮合中心异常";
                }
            }
            catch (Exception ex)
            {
                result = false;
                strMessage = "GT-2173:[商品期货撤单报盘]未知异常," + ex.Message;
                if (mc != null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService);
                }
            }

            LogHelper.WriteDebug("------>结束商品期货撤单报盘报盘OrderOfferCenter.CancelSPQHOrder，委托单号=" + qhOrder.EntrustNumber + ",最终消息=" + strMessage);
            return result;

            #region 旧撤单逻辑

            /*
            bool result = false;
            try
            {
                RC_MatchCenter mc = MCService.CommonPara.GetMatchCenterByCommodityCode(qhOrder.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                                mc.XiaDanService, mc.CuoHeService,
                                                                                ref cs, ref strMessage);
                    if (doc == null)
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                      mc.XiaDanService, mc.CuoHeService, ref cs,
                                                                      ref strMessage);
                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = new CancelEntity();
                        mcOrder.OldOrderNo = qhOrder.EntrustNumber;
                        mcOrder.StockCode = qhOrder.ContractCode;
                        mcOrder.ChannelNo = COUNT_CLIENT_ID;
                        var rde = doc.CancelCommoditiesOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.IsSuccess)
                            {
                                if (qhOrder.OrderStatusId == (int) Types.OrderStateType.DOSPartDealed)
                                {
                                    qhOrder.OrderStatusId = (int) Types.OrderStateType.DOSPartDealRemoveSoon;
                                    result = true;
                                    //TODO:并发问题，见现货
                                    OrderOfferDataLogic.UpdateFutureOrder(qhOrder);
                                }
                                else if (qhOrder.OrderStatusId == (int) Types.OrderStateType.DOSIsRequired)
                                {
                                    qhOrder.OrderStatusId = (int) Types.OrderStateType.DOSRequiredRemoveSoon;
                                    result = true;
                                    OrderOfferDataLogic.UpdateFutureOrder(qhOrder);
                                }
                                else
                                    strMessage = "GT-2137:[商品期货撤单报盘]委托单状态异常,委托单状态不可撤";
                            }
                            else
                                strMessage = rde.Message;
                        }
                        else
                            strMessage = "GT-2138:[商品期货撤单报盘]报盘返回结果异常";
                    }
                    // else
                    // strMessage = "GT-2508 [商品期货撤单报盘]报盘通道异常";
                }
                else
                    strMessage = "GT-2139:[商品期货撤单报盘]代码未分配撮合中心异常";
            }
            catch (Exception ex)
            {
                strMessage = "GT-2140:[商品期货撤单报盘]未知异常" + ex.Message;
            }
            return result;
             * */

            #endregion
        }

        /// <summary>
        /// 股指期货撤单报盘
        /// </summary>
        /// <param name="qhOrder"></param>
        /// <param name="strMessage"></param>
        /// <param name="ost">撤单状态</param>
        /// <returns></returns>
        public bool CancelGZQHOrder(QH_TodayEntrustTableInfo qhOrder, ref string strMessage, out ReckoningCounter.Entity.Contants.Types.OrderStateType ost)
        {
            LogHelper.WriteDebug("------>开始股指期货撤单报盘OrderOfferCenter.CancelGZQHOrder,委托单号=" + qhOrder.EntrustNumber);
            bool result = false;
            ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.None;
            RC_MatchCenter mc = null;
            try
            {
                mc = MCService.CommonPara.GetMatchCenterByCommodityCode(qhOrder.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);
                    if (doc == null)
                    {
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService, ref cs, ref strMessage);
                    }

                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = new CancelEntity();
                        mcOrder.OldOrderNo = qhOrder.McOrderId;
                        mcOrder.StockCode = qhOrder.ContractCode;
                        mcOrder.ChannelNo = COUNT_CLIENT_ID;

                        var rde = doc.CancelFutureOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.IsSuccess)
                            {
                                LogHelper.WriteDebug(
                                    "------>股指期货撤单报盘成功OrderOfferCenter.CancelGZQHOrder，委托单号=" + qhOrder.EntrustNumber);
                                if (qhOrder.OrderStatusId == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealed)
                                {
                                    qhOrder.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateFutureOrderStatus_Cancel(qhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon;
                                }
                                else if (qhOrder.OrderStatusId == (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSIsRequired)
                                {
                                    qhOrder.OrderStatusId = (int)ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                    result = true;
                                    //OrderOfferDataLogic.UpdateStockOrder(xhOrder);
                                    OrderOfferDataLogic.UpdateFutureOrderStatus_Cancel(qhOrder);
                                    ost = ReckoningCounter.Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon;
                                }
                                else
                                {
                                    result = false;
                                    strMessage = "GT-2190:[股指期货撤单报盘]委托单状态异常,委托单状态不可撤";
                                }

                                //0827：撤单报盘成功也要推一次
                                string traderId = "";
                                var user = AccountManager.Instance.GetUserByAccount(qhOrder.CapitalAccount);
                                if (user != null)
                                {
                                    traderId = user.UserID;
                                }
                                //else
                                //{
                                //    traderId = qhCounterCache.GetTraderIdByFundAccount(qhOrder.CapitalAccount);
                                //    //TODO:需要根据资金账户获取用户ID
                                //}

                                ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject =
                                    new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                                reckonEndObject.IsSuccess = true;
                                reckonEndObject.EntrustTable = qhOrder;
                                reckonEndObject.TradeTableList = new List<QH_TodayTradeTableInfo>();
                                reckonEndObject.TradeID = traderId;
                                reckonEndObject.Message = "";

                                CounterOrderService.Instance.AcceptStockIndexFuturesDealOrder(reckonEndObject);
                            }
                            else
                            {
                                LogHelper.WriteInfo("------>股指期货撤单报盘失败OrderOfferCenter.CancelSPQHOrder，委托单号=" + qhOrder.EntrustNumber + ",Message=" + rde.Message);
                                result = false;
                                strMessage = rde.Message;
                            }
                        }
                        else
                            strMessage = "GT-2191:[股指期货撤单报盘]报盘返回结果异常";
                    }
                    //else
                    // strMessage = "GT-2308 [现货撤单报盘]报盘通道异常";
                }
                else
                    strMessage = "GT-2192:[股指期货撤单报盘]代码未分配撮合中心异常";
            }
            catch (Exception ex)
            {
                result = false;
                strMessage = "GT-2193:[股指期货撤单报盘]未知异常," + ex.Message;
                if (mc != null)
                {
                    _seviceChannelManager.RemoveDoOrderAndRptChannel(mc.IP, mc.Port.Value.ToString(), mc.XiaDanService, mc.CuoHeService);
                }
            }

            LogHelper.WriteDebug("------>结束股指期货撤单报盘报盘OrderOfferCenter.CancelGZQHOrder，委托单号=" + qhOrder.EntrustNumber + ",最终消息=" + strMessage);
            return result;

            #region 旧撤单逻辑

            /*
            bool result = false;
            try
            {
                RC_MatchCenter mc = MCService.CommonPara.GetMatchCenterByCommodityCode(qhOrder.ContractCode);
                ChannelState cs = ChannelState.CSNormal;
                if (mc != null)
                {
                    DoOrderClient doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                                mc.XiaDanService, mc.CuoHeService,
                                                                                ref cs, ref strMessage);
                    if (doc == null)
                        doc = _seviceChannelManager.GetDoOrderChannel(mc.IP, mc.Port.Value.ToString(),
                                                                      mc.XiaDanService, mc.CuoHeService, ref cs,
                                                                      ref strMessage);
                    if (cs == ChannelState.CSNormal && doc != null)
                    {
                        var mcOrder = new CancelEntity();
                        mcOrder.OldOrderNo = qhOrder.EntrustNumber;
                        mcOrder.StockCode = qhOrder.ContractCode;
                        mcOrder.ChannelNo = COUNT_CLIENT_ID;
                        var rde = doc.CancelFutureOrder(mcOrder);
                        if (rde != null)
                        {
                            if (rde.IsSuccess)
                            {
                                if (qhOrder.OrderStatusId == (int) Types.OrderStateType.DOSPartDealed)
                                {
                                    qhOrder.OrderStatusId = (int) Types.OrderStateType.DOSPartDealRemoveSoon;
                                    result = true;
                                    //TODO:并发问题，见现货
                                    OrderOfferDataLogic.UpdateFutureOrder(qhOrder);
                                }
                                else if (qhOrder.OrderStatusId == (int) Types.OrderStateType.DOSIsRequired)
                                {
                                    qhOrder.OrderStatusId = (int) Types.OrderStateType.DOSRequiredRemoveSoon;
                                    result = true;
                                    OrderOfferDataLogic.UpdateFutureOrder(qhOrder);
                                }
                                else
                                    strMessage = "GT-2141:[股指期货撤单报盘]委托单状态异常,委托单状态不可撤";
                            }
                            else
                                strMessage = rde.Message;
                        }
                        else
                            strMessage = "GT-2142:[股指期货撤单报盘]报盘返回结果异常";
                    }
                    //else
                    //strMessage = "GT-2808 [股指期货撤单报盘]报盘通道异常";
                }
                else
                    strMessage = "GT-2143:[股指期货撤单报盘]代码未分配撮合中心异常";
            }
            catch (Exception ex)
            {
                strMessage = "GT-2144:[股指期货撤单报盘]未知异常," + ex.Message;
            }

            return result;
             * */

            #endregion
        }

        #endregion
    }
}