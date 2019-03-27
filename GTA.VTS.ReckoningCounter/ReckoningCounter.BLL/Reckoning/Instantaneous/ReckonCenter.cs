#region Using Namespace

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer;
using ReckoningCounter.BLL.DelegateValidate.ManagementCenter;
using ReckoningCounter.BLL.Reckoning.Instantaneous;
using ReckoningCounter.BLL.Reckoning.Logic;
using ReckoningCounter.BLL.Reckoning.Logic.GZQH;
using ReckoningCounter.BLL.Reckoning.Logic.HK;
using ReckoningCounter.BLL.Reckoning.Logic.SPQH;
using ReckoningCounter.BLL.Reckoning.Logic.XH;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;

//using ReckoningCounter.BLL.Common.DataLogicFlow;
//using ReckoningCounter.BLL.Common.DataLogicFlow.Factory;

#endregion

namespace ReckoningCounter.BLL
{
    /// <summary>
    /// 清算中心，错误编码2500-2599
    /// 作者：宋涛
    /// 时间：2008-07-16
    /// </summary>
    public class ReckonCenter
    {
        #region == 静态属性 ==

        private static ReckonCenter _instance;

        /// <summary>
        /// 单例
        /// </summary>
        public static ReckonCenter Instace
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReckonCenter(10);
                }
                return _instance;
            }
        }

        #endregion

        #region == 字段/属性 ==

        /// <summary>
        /// 现货撤单处理元素容器具
        /// </summary>
        //private QueueBufferBase<CancelOrderRptItem> _CancelOrderProcessHolder;
        /// <summary>
        /// 成交回报回送服务
        /// </summary>
        protected CounterOrderService _counterOrderService;

        /// <summary>
        /// 现货错误价格委托回报处理容器
        /// </summary>
        //private QueueBufferBase<OrderErrorItem> _errorPriceOrderHolder;
        /// <summary>
        /// 股指期货撤单处理元素容器具
        /// </summary>
        //private QueueBufferBase<CancelOrderRptItem> _GZQHCancelOrderProcessHolder;
        /// <summary>
        /// 股指期货错误价格委托回报处理容器
        /// </summary>
        //private QueueBufferBase<OrderErrorItem> _GZQHErrorPriceOrderHolder;
        /// <summary>
        /// 商品期货清算工作并行数
        /// </summary>
        //private int _iMercantileFuturekWorkThreadCount = 1;
        /// <summary>
        /// 管理中心品种，规则服务
        /// </summary>
        public IMTradingRule _IMTradingRule;

        /// <summary>
        /// 股指期货清算工作并行数
        /// </summary>
        //private int _iStockIndexFutureWorkThreadCount = 1;
        /// <summary>
        /// 现货清算工作并行数
        /// </summary>
        //private int _iStockWorkThreadCount = 1;
        /// <summary>
        /// 商品期货清算工作并行容器
        /// </summary>
        //protected List<QueueBufferBase<CommoditiesDealBackEntity>> _mercantileFutureQueueBuffers;
        /// <summary>
        /// 报盘数据逻辑
        /// </summary>
        //private OrderOfferDataLogic _orderOfferDataLogic = new OrderOfferDataLogic();
        /// <summary>
        /// 清算数据逻辑
        /// </summary>
        //private ReckonDataLogic _reckonDataLogic = new ReckonDataLogic();
        /// <summary>
        /// 商品期货撤单处理元素容器具
        /// </summary>
        //private QueueBufferBase<CancelOrderRptItem> _SPQHCancelOrderProcessHolder;
        /// <summary>
        /// 商品期货错误价格委托回报处理容器
        /// </summary>
        //private QueueBufferBase<OrderErrorItem> _SPQHErrorPriceOrderHolder;
        /// <summary>
        /// 股指期货清算工作并行容器
        /// </summary>
        //protected List<QueueBufferBase<FutureDealBackEntity>> _stockIndexFutureQueueBuffers;
        /// <summary>
        /// 现货清算工作并行容器
        /// </summary>
        //protected List<QueueBufferBase<StockDealBackEntity>> _stockQueueBuffers;
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };

        private CounterCache xhCounterCache = XHCounterCache.Instance;
        private CounterCache hkCounterCache = HKCounterCache.Instance;
        private CounterCache qhCounterCache = QHCounterCache.Instance;

        #region == 运行时业务消息 ==

        /// <summary>
        /// 撤单回报事件
        /// </summary>
        public event EventHandler<RuntimeMessageEventArge> CancelOrderCallbackEvent = null;

        /// <summary>
        /// 委托回报事件
        /// </summary>
        public event EventHandler<RuntimeMessageEventArge> OrderRptCallbackEvent = null;

        /// <summary>
        /// 成交回报事件
        /// </summary>
        public event EventHandler<RuntimeMessageEventArge> DealRptCallbackEvent = null;

        #endregion

        #endregion

        #region == 构造器 ==

        /// <summary>
        /// 构造器
        /// </summary>
        internal ReckonCenter(int threadCount)
        {
            InitializeThreadPool();

            //初始化工作线程数
            //_iMercantileFuturekWorkThreadCount = threadCount;
            //_iStockIndexFutureWorkThreadCount = threadCount;
            //_iStockWorkThreadCount = threadCount;


            _counterOrderService = CounterOrderService.Instance;

            #region 现货

            //_stockQueueBuffers = new List<QueueBufferBase<StockDealBackEntity>>();

            //_errorPriceOrderHolder = new QueueBufferBase<OrderErrorItem>();
            //_errorPriceOrderHolder.QueueItemProcessEvent += BeginXHErrorPriceCancelOrderReckoning;

            //_CancelOrderProcessHolder = new QueueBufferBase<CancelOrderRptItem>();
            //_CancelOrderProcessHolder.QueueItemProcessEvent += BeginXHCancelOrderReckoning;

            #endregion

            #region 商品期货

            //_mercantileFutureQueueBuffers = new List<QueueBufferBase<CommoditiesDealBackEntity>>();
            //_SPQHCancelOrderProcessHolder = new QueueBufferBase<CancelOrderRptItem>();
            //_SPQHCancelOrderProcessHolder.QueueItemProcessEvent += BeginSPQHCancelOrderReckoning;

            //_SPQHErrorPriceOrderHolder = new QueueBufferBase<OrderErrorItem>();
            //_SPQHErrorPriceOrderHolder.QueueItemProcessEvent += BeginSPQHErrorPriceCancelOrderReckoning;

            #endregion

            #region 股指期货

            //_stockIndexFutureQueueBuffers = new List<QueueBufferBase<FutureDealBackEntity>>();
            //_GZQHErrorPriceOrderHolder = new QueueBufferBase<OrderErrorItem>();
            //_GZQHErrorPriceOrderHolder.QueueItemProcessEvent += BeginGZQHErrorPriceCancelOrderReckoning;

            //_GZQHCancelOrderProcessHolder = new QueueBufferBase<CancelOrderRptItem>();
            //_GZQHCancelOrderProcessHolder.QueueItemProcessEvent += BeginGZQHCancelOrderReckoning;

            #endregion

            InitBuffers();
        }

        #endregion

        private void InitializeThreadPool()
        {
            int offerThreadCount = 200;
            try
            {
                string offerThread = ConfigurationManager.AppSettings["dealThread"];
                if (!string.IsNullOrEmpty(offerThread))
                {
                    int count = 0;
                    bool isSuccess = int.TryParse(offerThread.Trim(), out count);
                    if (isSuccess)
                        offerThreadCount = count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            smartPool.MaxThreads = offerThreadCount;
            smartPool.MinThreads = 25;
            smartPool.Start();

            //int cancelThreadCount = 200;
            //try
            //{
            //    string cancelThread = ConfigurationManager.AppSettings["cancelThread"];
            //    if (!string.IsNullOrEmpty(cancelThread))
            //    {
            //        int count = 0;
            //        bool isSuccess = int.TryParse(cancelThread.Trim(), out count);
            //        if (isSuccess)
            //            cancelThreadCount = count;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //}

            //smartPool2.MaxThreads = cancelThreadCount;
            //smartPool2.Start();
        }

        /// <summary>
        /// 初始化并行工作容器
        /// </summary>
        private void InitBuffers()
        {
            /*//现货
            for (int i = 0; i < this._iStockWorkThreadCount; i++)
            {
                var qbb = new QueueBufferBase<StockDealBackEntity>();
                qbb.Name = "XHDealBuffer-" + (i + 1);
                qbb.QueueItemProcessEvent += BeginXHReckoning2;
                _stockQueueBuffers.Add(qbb);
            }
            //股指期货
            for (int j = 0; j < this._iStockIndexFutureWorkThreadCount; j++)
            {
                var qbb = new QueueBufferBase<FutureDealBackEntity>();
                qbb.Name = "GZQHDealBuffer-" + (j + 1);
                qbb.QueueItemProcessEvent += BeginGZQHReckoning2;
                this._stockIndexFutureQueueBuffers.Add(qbb);
            }
            //商品期货
            for (int k = 0; k < this._iMercantileFuturekWorkThreadCount; k++)
            {
                var qbb = new QueueBufferBase<CommoditiesDealBackEntity>();
                qbb.Name = "SPQHDealBuffer-" + (k + 1);
                qbb.QueueItemProcessEvent += BeginSPQHReckoning2;
                this._mercantileFutureQueueBuffers.Add(qbb);
            }**/
        }

        #region 1.0清算逻辑

        /*
        #region 撤单清算入口

        #region 现货撤单


        /// <summary>
        /// 现货撤单异步回报处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginXHCancelOrderReckoning(object sender,
                                                                     QueueItemHandleEventArg<CancelOrderRptItem> e)
        {
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.Stock)
                {
                    var OrderCacheItem = CounterCache.Instance.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    XH_TodayEntrustTableInfo tet = null;

                    var coe = e.Item.RptItem;

                    if (coe.IsSuccess) //撤单成功
                    {
                        //var rde = new ResultDataEntity();
                        //rde.Message = e.Item.RptItem.Message;
                        //rde.OrderNo = e.Item.RptItem.OrderNo;

                        var logic = XHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(coe.OrderNo);
                        if (logic == null)
                        {
                            OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                                _XHDataLogicProcessor =
                                    DataLogicProcessorFactory.GetXHDataLogicProcessor(coe.OrderNo);

                            if (_XHDataLogicProcessor == null)
                                return;

                            logic = (XHOrderLogicFlow)_XHDataLogicProcessor;
                            logic.EndReckoningEvent += EndXHReckoning;
                            logic.EndCancelEvent += EndXHCancelReckoning;
                            XHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(coe.OrderNo, logic);
                        }

                        //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                        //    _XHDataLogicProcessor =
                        //        DataLogicProcessorFactory.GetXHDataLogicProcessor(rde.OrderNo);
                        //if (_XHDataLogicProcessor != null)
                        //{
                        /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                        /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                        /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                        /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                        //_XHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.RptItem, out tet, true);
                        //}

                        logic.InsertMessage(e.Item.RptItem);
                    }
                    else //撤单失败
                    {
                        if (OrderCacheItem != null)
                        {
                            tet = ReckonDataLogic.GetXHEntrustEntity(e.Item.RptItem.OrderNo);

                            if (tet != null)
                            {
                                if (tet.OrderStatusId ==
                                    (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                                else if (tet.OrderStatusId ==
                                         (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                                tet.OrderMessage = e.Item.RptItem.Message;

                                OrderOfferDataLogic.UpdateStockOrderStatusAndMessage(tet);
                            }
                            else
                            {
                                //TODO:tet=null
                            }
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    var OrderCacheItem = CounterCache.Instance.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;
                    // _reckonDataLogic.GetQHEntrustEntity(OrderCacheItem.CounterOrderNo);
                    if (tet != null)
                    {
                        if (e.Item.RptItem.IsSuccess) //撤单成功
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else //撤单失败
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                        }
                        OrderOfferDataLogic.UpdateFutureOrder(tet);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:撤单异步回报处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 有价格问题的现货委托信息处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginXHErrorPriceCancelOrderReckoning(object sender,
                                                                  QueueItemHandleEventArg<OrderErrorItem> e)
        {
            string mcOrder = string.Empty;
            string orderStatuid = string.Empty;
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.Stock)
                {
                    XH_TodayEntrustTableInfo tet;

                    ResultDataEntity rde = e.Item.ErrorItem;
                    string orderNo = rde.OrderNo;
                    var logic = XHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(orderNo);
                    if (logic == null)
                    {
                        OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                            _XHDataLogicProcessor =
                                DataLogicProcessorFactory.GetXHDataLogicProcessor(orderNo);

                        if (_XHDataLogicProcessor == null)
                            return;

                        logic = (XHOrderLogicFlow)_XHDataLogicProcessor;
                        logic.EndReckoningEvent += EndXHReckoning;
                        logic.EndCancelEvent += EndXHCancelReckoning;

                        XHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(orderNo, logic);
                    }

                    //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    //    _XHDataLogicProcessor =
                    //        DataLogicProcessorFactory.GetXHDataLogicProcessor(e.Item.ErrorItem.OrderNo);

                    /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                    /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                    /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                    /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                    CancelOrderEntity coe = new CancelOrderEntity();
                    coe.Message = rde.Message;
                    coe.OrderNo = rde.OrderNo;
                    coe.OrderVolume = -1;
                    coe.Id = rde.Id;

                    logic.InsertMessage(coe);

                    //_XHDataLogicProcessor.InstantReckon_CancelOrder(coe, out tet, false);
                    //if (tet != null)
                    //{
                    //    mcOrder = tet.McOrderId;
                    //    orderStatuid = tet.OrderStatusId.Value.ToString();
                    //}
                }
                else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    // OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                    //_SPQHDataLogicProcessor = DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);
                    // return _SPQHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.ErrorItem);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                //TODO:有价格问题的委托信息处理异常
                //_orderOfferDataLogic.UpdateFutureOrder(tet);
                LogHelper.WriteError(ex.Message, ex);
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
        }

        #endregion

        #region 商品期货撤单

        /// <summary>
        /// 商品期货撤单异步回报处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginSPQHCancelOrderReckoning(object sender,
                                                                         QueueItemHandleEventArg<CancelOrderRptItem> e)
        {
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    var OrderCacheItem = CounterCache.Instance.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;

                    var coe = e.Item.RptItem;

                    if (coe.IsSuccess) //撤单成功
                    {
                        //var rde = new ResultDataEntity();
                        //rde.Message = e.Item.RptItem.Message;
                        //rde.OrderNo = e.Item.RptItem.OrderNo;

                        var logic = SPQHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(coe.OrderNo);
                        if (logic == null)
                        {
                            OrderLogicFlowBase
                                <MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                                _XHDataLogicProcessor =
                                    DataLogicProcessorFactory.GetSPQHDataLogicProcessor(coe.OrderNo);

                            if (_XHDataLogicProcessor == null)
                                return;

                            logic = (SPQHOrderLogicFlow)_XHDataLogicProcessor;
                            logic.EndReckoningEvent += EndSPQHReckoning;
                            logic.EndCancelEvent += EndSPQHCancelReckoning;
                            SPQHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(coe.OrderNo, logic);
                        }

                        //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                        //    _XHDataLogicProcessor =
                        //        DataLogicProcessorFactory.GetXHDataLogicProcessor(rde.OrderNo);
                        //if (_XHDataLogicProcessor != null)
                        //{
                        /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                        /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                        /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                        /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                        //_XHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.RptItem, out tet, true);
                        //}

                        logic.InsertMessage(e.Item.RptItem);
                    }
                    else //撤单失败
                    {
                        if (OrderCacheItem != null)
                        {
                            tet = ReckonDataLogic.GetQHEntrustEntity(e.Item.RptItem.OrderNo);

                            if (tet != null)
                            {
                                if (tet.OrderStatusId ==
                                    (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                                else if (tet.OrderStatusId ==
                                         (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                                tet.OrderMessage = e.Item.RptItem.Message;

                                OrderOfferDataLogic.UpdateFutureOrder(tet);
                            }
                            else
                            {
                                //TODO:tet=null
                            }
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    var OrderCacheItem = CounterCache.Instance.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;
                    // _reckonDataLogic.GetQHEntrustEntity(OrderCacheItem.CounterOrderNo);
                    if (tet != null)
                    {
                        if (e.Item.RptItem.IsSuccess) //撤单成功
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else //撤单失败
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                        }
                        OrderOfferDataLogic.UpdateFutureOrder(tet);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:撤单异步回报处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }


        /// <summary>
        /// 有价格问题的商品期货货委托信息处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginSPQHErrorPriceCancelOrderReckoning(object sender,
                                                                      QueueItemHandleEventArg<OrderErrorItem> e)
        {
            string mcOrder = string.Empty;
            string orderStatuid = string.Empty;
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    QH_TodayEntrustTableInfo tet;

                    ResultDataEntity rde = e.Item.ErrorItem;
                    string orderNo = rde.OrderNo;
                    var logic = SPQHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(orderNo);
                    if (logic == null)
                    {
                        OrderLogicFlowBase
                            <MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                            _XHDataLogicProcessor =
                                DataLogicProcessorFactory.GetSPQHDataLogicProcessor(orderNo);

                        if (_XHDataLogicProcessor == null)
                            return;

                        logic = (SPQHOrderLogicFlow)_XHDataLogicProcessor;
                        logic.EndReckoningEvent += EndSPQHReckoning;
                        logic.EndCancelEvent += EndSPQHCancelReckoning;

                        SPQHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(orderNo, logic);
                    }

                    //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    //    _XHDataLogicProcessor =
                    //        DataLogicProcessorFactory.GetXHDataLogicProcessor(e.Item.ErrorItem.OrderNo);

                    /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                    /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                    /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                    /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                    CancelOrderEntity coe = new CancelOrderEntity();
                    coe.Message = rde.Message;
                    coe.OrderNo = rde.OrderNo;
                    coe.OrderVolume = -1;
                    coe.Id = rde.Id;

                    logic.InsertMessage(coe);

                    //_XHDataLogicProcessor.InstantReckon_CancelOrder(coe, out tet, false);
                    //if (tet != null)
                    //{
                    //    mcOrder = tet.McOrderId;
                    //    orderStatuid = tet.OrderStatusId.Value.ToString();
                    //}
                }
                else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    // OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                    //_SPQHDataLogicProcessor = DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);
                    // return _SPQHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.ErrorItem);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                //TODO:有价格问题的委托信息处理异常
                //_orderOfferDataLogic.UpdateFutureOrder(tet);
                LogHelper.WriteError(ex.Message, ex);
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
        }

        #endregion

        #region 股指期货撤单

        /// <summary>
        /// 股指期货撤单异步回报处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginGZQHCancelOrderReckoning(object sender,
                                                                         QueueItemHandleEventArg<CancelOrderRptItem> e)
        {
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    var OrderCacheItem = CounterCache.Instance.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;

                    var coe = e.Item.RptItem;

                    if (coe.IsSuccess) //撤单成功
                    {
                        //var rde = new ResultDataEntity();
                        //rde.Message = e.Item.RptItem.Message;
                        //rde.OrderNo = e.Item.RptItem.OrderNo;

                        var logic = GZQHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(coe.OrderNo);
                        if (logic == null)
                        {
                            OrderLogicFlowBase
                                <StockIndexFuturesOrderRequest, QH_TodayEntrustTableInfo, FutureDealBackEntity>
                                _XHDataLogicProcessor =
                                    DataLogicProcessorFactory.GetGZQHDataLogicProcessor(coe.OrderNo);

                            if (_XHDataLogicProcessor == null)
                                return;

                            logic = (GZQHOrderLogicFlow)_XHDataLogicProcessor;
                            logic.EndReckoningEvent += EndGZQHReckoning;
                            logic.EndCancelEvent += EndGZQHCancelReckoning;
                            GZQHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(coe.OrderNo, logic);
                        }

                        //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                        //    _XHDataLogicProcessor =
                        //        DataLogicProcessorFactory.GetXHDataLogicProcessor(rde.OrderNo);
                        //if (_XHDataLogicProcessor != null)
                        //{
                        /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                        /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                        /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                        /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                        //_XHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.RptItem, out tet, true);
                        //}

                        logic.InsertMessage(e.Item.RptItem);
                    }
                    else //撤单失败
                    {
                        if (OrderCacheItem != null)
                        {
                            tet = ReckonDataLogic.GetQHEntrustEntity(e.Item.RptItem.OrderNo);

                            if (tet != null)
                            {
                                if (tet.OrderStatusId ==
                                    (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                                else if (tet.OrderStatusId ==
                                         (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                                tet.OrderMessage = e.Item.RptItem.Message;

                                OrderOfferDataLogic.UpdateFutureOrder(tet);
                            }
                            else
                            {
                                //TODO:tet=null
                            }
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    var OrderCacheItem = CounterCache.Instance.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;
                    // _reckonDataLogic.GetQHEntrustEntity(OrderCacheItem.CounterOrderNo);
                    if (tet != null)
                    {
                        if (e.Item.RptItem.IsSuccess) //撤单成功
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else //撤单失败
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                        }
                        OrderOfferDataLogic.UpdateFutureOrder(tet);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:撤单异步回报处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }


        /// <summary>
        /// 有价格问题的股指期货货委托信息处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginGZQHErrorPriceCancelOrderReckoning(object sender,
                                                                      QueueItemHandleEventArg<OrderErrorItem> e)
        {
            string mcOrder = string.Empty;
            string orderStatuid = string.Empty;
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    QH_TodayEntrustTableInfo tet;

                    ResultDataEntity rde = e.Item.ErrorItem;
                    string orderNo = rde.OrderNo;
                    var logic = GZQHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(orderNo);
                    if (logic == null)
                    {
                        OrderLogicFlowBase
                            <StockIndexFuturesOrderRequest, QH_TodayEntrustTableInfo, FutureDealBackEntity>
                            _XHDataLogicProcessor =
                                DataLogicProcessorFactory.GetGZQHDataLogicProcessor(orderNo);

                        if (_XHDataLogicProcessor == null)
                            return;

                        logic = (GZQHOrderLogicFlow)_XHDataLogicProcessor;
                        logic.EndReckoningEvent += EndGZQHReckoning;
                        logic.EndCancelEvent += EndGZQHCancelReckoning;

                        GZQHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(orderNo, logic);
                    }

                    //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    //    _XHDataLogicProcessor =
                    //        DataLogicProcessorFactory.GetXHDataLogicProcessor(e.Item.ErrorItem.OrderNo);

                    /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                    /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                    /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                    /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                    CancelOrderEntity coe = new CancelOrderEntity();
                    coe.Message = rde.Message;
                    coe.OrderNo = rde.OrderNo;
                    coe.OrderVolume = -1;
                    coe.Id = rde.Id;

                    logic.InsertMessage(coe);

                    //_XHDataLogicProcessor.InstantReckon_CancelOrder(coe, out tet, false);
                    //if (tet != null)
                    //{
                    //    mcOrder = tet.McOrderId;
                    //    orderStatuid = tet.OrderStatusId.Value.ToString();
                    //}
                }
                else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    // OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                    //_SPQHDataLogicProcessor = DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);
                    // return _SPQHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.ErrorItem);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                //TODO:有价格问题的委托信息处理异常
                //_orderOfferDataLogic.UpdateFutureOrder(tet);
                LogHelper.WriteError(ex.Message, ex);
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
        }

        #endregion

        #endregion

        #region 成交清算入口

        #region 现货清算处理

        /// <summary>
        /// 开始现货清算处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginXHReckoning(object sender, QueueItemHandleEventArg<StockDealBackEntity> e)
        {
            try
            {
                if (e == null)
                    return;

                if (e.Item == null)
                    return;

                StockDealBackEntity ido = e.Item;
                if (ido == null)
                    return;

                XHOrderLogicFlow logic = InitializeXHReckonLogic(ido.OrderNo);

                //再获取一次
                if (logic == null)
                    logic = InitializeXHReckonLogic(ido.OrderNo);

                if (logic != null)
                    logic.InsertMessage(ido);
            }
            catch (Exception ex)
            {
                //TODO:现货清算处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        public XHOrderLogicFlow InitializeXHReckonLogic(string matchOrderNo)
        {
            bool canAdd = false;
            var logic = XHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(matchOrderNo);
            if (logic == null)
            {
                OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    _XHDataLogicProcessor =
                        DataLogicProcessorFactory.GetXHDataLogicProcessor(matchOrderNo);

                if (_XHDataLogicProcessor == null)
                    return logic;

                logic = (XHOrderLogicFlow)_XHDataLogicProcessor;


                canAdd = XHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(matchOrderNo, logic);
                if (canAdd)
                {
                    logic.EndReckoningEvent += EndXHReckoning;
                    logic.EndCancelEvent += EndXHCancelReckoning;
                }
                else
                {
                    logic = XHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(matchOrderNo);
                }
            }

            return logic;
        }

        /// <summary>
        /// 现货清算完后的通知事件接收方法
        /// </summary>
        /// <param name="stockReckoningResultObject"></param>
        private void EndXHReckoning(
            ReckoningResultObject<XH_TodayEntrustTableInfo, StockDealBackEntity> stockReckoningResultObject)
        {
            var item = stockReckoningResultObject.DealBackEntity;
            string strDealNo = stockReckoningResultObject.DealNo;
            var tet = stockReckoningResultObject.EntrustTable;

            if (stockReckoningResultObject.isSuccess)
            {
                //var rpt = this.XHDealRptConvert(item, strDealNo, tet);
                //_counterOrderService.AcceptStockDealOrder(rpt, tet.CallbackChannlId);
            }

            if (tet != null)
            {
                CounterCache.Instance.RemoveOrderMappingInfo(tet.McOrderId,
                                                             (Entity.Contants.Types.OrderStateType)
                                                             Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
                                                                        tet.OrderStatusId.ToString()));

                XHLogciFlowManager.GetInstance().RemoveLogicFlowByMatchOrderNo(item.OrderNo,
                                                                               (Entity.Contants.Types.OrderStateType)
                                                                               Enum.Parse(
                                                                                   typeof(
                                                                                       Entity.Contants.Types.
                                                                                       OrderStateType),
                                                                                   tet.OrderStatusId.ToString()));
            }
        }


        /// <summary>
        /// 现货撤单完后的通知事件接收方法
        /// </summary>
        /// <param name="cancelObject"></param>
        private void EndXHCancelReckoning(CancelResultObject<XH_TodayEntrustTableInfo> cancelObject)
        {
            CancelOrderEntity coe = cancelObject.CancelEntity;


            if (coe.OrderVolume == -1)
            {
                //有价格问题的委托信息处理
                var tet = cancelObject.EntrustTable;

                string mcOrder = string.Empty;
                string orderStatuid = string.Empty;

                if (tet != null)
                {
                    mcOrder = tet.McOrderId;
                    orderStatuid = tet.OrderStatusId.ToString();
                }

                if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
                    CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
                                                                 (Entity.Contants.Types.OrderStateType)
                                                                 Enum.Parse(
                                                                     typeof(Entity.Contants.Types.OrderStateType),
                                                                     orderStatuid));
            }
            else
            {
                //撤单异步回报处理
            }
        }

        #endregion

        #region 商品期货清算处理

        /// <summary>
        /// 开始商品期货清算处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginSPQHReckoning(object sender,
                                        QueueItemHandleEventArg<CommoditiesDealBackEntity> e)
        {
            try
            {
                CommoditiesDealBackEntity ido = e.Item;
                var logic = SPQHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(ido.OrderNo);
                if (logic == null)
                {
                    OrderLogicFlowBase
                        <MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                        _GZQHDataLogicProcessor =
                            DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);

                    if (_GZQHDataLogicProcessor == null)
                        return;

                    logic = (SPQHOrderLogicFlow)_GZQHDataLogicProcessor;
                    logic.EndReckoningEvent += EndSPQHReckoning;
                    logic.EndCancelEvent += EndSPQHCancelReckoning;

                    SPQHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(ido.OrderNo, logic);
                }

                logic.InsertMessage(ido);
            }
            catch (Exception ex)
            {
                //TODO:现货清算处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void EndSPQHCancelReckoning(CancelResultObject<QH_TodayEntrustTableInfo> cancelObject)
        {
            CancelOrderEntity coe = cancelObject.CancelEntity;


            if (coe.OrderVolume == -1)
            {
                //有价格问题的委托信息处理
                var tet = cancelObject.EntrustTable;

                string mcOrder = string.Empty;
                string orderStatuid = string.Empty;

                if (tet != null)
                {
                    mcOrder = tet.McOrderId;
                    orderStatuid = tet.OrderStatusId.ToString();
                }

                if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
                    CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
                                                                 (Entity.Contants.Types.OrderStateType)
                                                                 Enum.Parse(
                                                                     typeof(Entity.Contants.Types.OrderStateType),
                                                                     orderStatuid));
            }
            else
            {
                //撤单异步回报处理
            }
        }

        private void EndSPQHReckoning(
            ReckoningResultObject<QH_TodayEntrustTableInfo, CommoditiesDealBackEntity> qhReckoningResultObject)
        {
            var item = qhReckoningResultObject.DealBackEntity;
            string strDealNo = qhReckoningResultObject.DealNo;
            var tet = qhReckoningResultObject.EntrustTable;

            if (qhReckoningResultObject.isSuccess)
            {
                //var rpt = this.SPQHDealRptConvert(item, strDealNo, tet.EntrustNumber);
                //_counterOrderService.AcceptStockIndexFuturesDealOrder(rpt, tet.CallbackChannelId);
            }

            if (tet != null)
            {
                CounterCache.Instance.RemoveOrderMappingInfo(tet.McOrderId,
                                                             (Entity.Contants.Types.OrderStateType)
                                                             Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
                                                                        tet.OrderStatusId.ToString()));

                XHLogciFlowManager.GetInstance().RemoveLogicFlowByMatchOrderNo(item.OrderNo,
                                                                               (Entity.Contants.Types.OrderStateType)
                                                                               Enum.Parse(
                                                                                   typeof(
                                                                                       Entity.Contants.Types.
                                                                                       OrderStateType),
                                                                                   tet.OrderStatusId.ToString()));
            }
        }

        #endregion

        #region 股指期货清算处理

        /// <summary>
        /// 开始股指期货清算处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginGZQHReckoning(object sender,
                                        QueueItemHandleEventArg<FutureDealBackEntity> e)
        {
            try
            {
                FutureDealBackEntity ido = e.Item;
                var logic = GZQHLogciFlowManager.GetInstance().GetLogicFlowByMatchOrderNo(ido.OrderNo);
                if (logic == null)
                {
                    OrderLogicFlowBase<StockIndexFuturesOrderRequest, QH_TodayEntrustTableInfo, FutureDealBackEntity>
                        _GZQHDataLogicProcessor =
                            DataLogicProcessorFactory.GetGZQHDataLogicProcessor(ido.OrderNo);

                    if (_GZQHDataLogicProcessor == null)
                        return;

                    logic = (GZQHOrderLogicFlow)_GZQHDataLogicProcessor;
                    logic.EndReckoningEvent += EndGZQHReckoning;
                    logic.EndCancelEvent += EndGZQHCancelReckoning;

                    GZQHLogciFlowManager.GetInstance().AddLogicFlowByMatchOrderNo(ido.OrderNo, logic);
                }

                logic.InsertMessage(ido);
            }
            catch (Exception ex)
            {
                //TODO:现货清算处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void EndGZQHCancelReckoning(CancelResultObject<QH_TodayEntrustTableInfo> cancelObject)
        {
            CancelOrderEntity coe = cancelObject.CancelEntity;


            if (coe.OrderVolume == -1)
            {
                //有价格问题的委托信息处理
                var tet = cancelObject.EntrustTable;

                string mcOrder = string.Empty;
                string orderStatuid = string.Empty;

                if (tet != null)
                {
                    mcOrder = tet.McOrderId;
                    orderStatuid = tet.OrderStatusId.ToString();
                }

                if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
                    CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
                                                                 (Entity.Contants.Types.OrderStateType)
                                                                 Enum.Parse(
                                                                     typeof(Entity.Contants.Types.OrderStateType),
                                                                     orderStatuid));
            }
            else
            {
                //撤单异步回报处理
            }
        }

        private void EndGZQHReckoning(
            ReckoningResultObject<QH_TodayEntrustTableInfo, FutureDealBackEntity> qhReckoningResultObject)
        {
            var item = qhReckoningResultObject.DealBackEntity;
            string strDealNo = qhReckoningResultObject.DealNo;
            var tet = qhReckoningResultObject.EntrustTable;

            if (qhReckoningResultObject.isSuccess)
            {
                //var rpt = this.GZQHDealRptConvert(item, strDealNo, tet.EntrustNumber);
                //_counterOrderService.AcceptStockIndexFuturesDealOrder(rpt, tet.CallbackChannelId);
            }

            if (tet != null)
            {
                CounterCache.Instance.RemoveOrderMappingInfo(tet.McOrderId,
                                                             (Entity.Contants.Types.OrderStateType)
                                                             Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
                                                                        tet.OrderStatusId.ToString()));

                XHLogciFlowManager.GetInstance().RemoveLogicFlowByMatchOrderNo(item.OrderNo,
                                                                               (Entity.Contants.Types.OrderStateType)
                                                                               Enum.Parse(
                                                                                   typeof(
                                                                                       Entity.Contants.Types.
                                                                                       OrderStateType),
                                                                                   tet.OrderStatusId.ToString()));
            }
        }

        #endregion

        #endregion

        **/

        #endregion

        #region 清算功能方法

        /// <summary>
        /// 进行撮合-委托缓存校验
        /// 因为错误价格的回报返回非常快，从而会导致返回后报盘那里的缓存信息还没有加入，
        /// 这样就会导致清算失败，所以先进行判断，缓存中是否存在，如果不存在，那么暂停
        /// 线程后再取
        /// </summary>
        /// <param name="counterCache">缓存</param>
        /// <param name="orderNo">撮合对应的单号</param>
        private bool CanGetOrderCache(CounterCache counterCache, string orderNo)
        {
            //CounterCache counterCache = XHCounterCache.Instance;

            var cachItem = counterCache.GetOrderMappingInfo(orderNo);
            if (cachItem != null)
            {
                return true;
            }

            Thread.Sleep(1000);

            cachItem = counterCache.GetOrderMappingInfo(orderNo);
            if (cachItem != null)
            {
                return true;
            }

            Thread.Sleep(2000);

            cachItem = counterCache.GetOrderMappingInfo(orderNo);
            if (cachItem != null)
            {
                return true;
            }

            Thread.Sleep(3000);

            cachItem = counterCache.GetOrderMappingInfo(orderNo);
            if (cachItem == null)
            {
                string format = "ReconCenter.CanGetOrderCache无法查找[{0}]的缓存信息";
                string desc = string.Format(format, orderNo);
                LogHelper.WriteDebug(desc);
            }

            return false;
        }

        /// <summary>
        /// 获取现货清算单元
        /// </summary>
        /// <param name="matchOrderNo">撮合单号</param>
        /// <returns>现货清算单元</returns>
        private XHReckonUnit GetXHReckonUnit(string matchOrderNo)
        {
            var reckonUnitManager = ReckonUnitManagerFactory.GetXHReckonUnitManager();
            var reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(matchOrderNo);
            if (reckonUnit == null)
            {
                reckonUnit = new XHReckonUnit();
                bool canAdd = reckonUnitManager.AddReckonUnitByMatchOrderNo(matchOrderNo, reckonUnit);

                if (canAdd)
                {
                    reckonUnit.EndReckoningEvent += EndXHReckoning2;
                    reckonUnit.EndCancelEvent += EndXHCancelReckoning2;
                }
                else
                {
                    reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(matchOrderNo);
                }
            }
            return reckonUnit;
        }

        /// <summary>
        /// 获取港股清算单元
        /// </summary>
        /// <param name="matchOrderNo">撮合单号</param>
        /// <returns>现货清算单元</returns>
        private HKReckonUnit GetHKReckonUnit(string matchOrderNo)
        {
            var reckonUnitManager = ReckonUnitManagerFactory.GetHKReckonUnitManager();
            var reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(matchOrderNo);
            if (reckonUnit == null)
            {
                reckonUnit = new HKReckonUnit();
                bool canAdd = reckonUnitManager.AddReckonUnitByMatchOrderNo(matchOrderNo, reckonUnit);

                if (canAdd)
                {
                    reckonUnit.EndReckoningEvent += EndHKReckoning2;
                    reckonUnit.EndCancelEvent += EndHKCancelReckoning2;
                }
                else
                {
                    reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(matchOrderNo);
                }
            }
            return reckonUnit;
        }

        /// <summary>
        /// 获取股指期货清算单元
        /// </summary>
        /// <param name="orderNo">撮合单号</param>
        /// <returns>股指期货清算单元</returns>
        private GZQHReckonUnit GetGZQHReckonUnit(string orderNo)
        {
            var reckonUnitManager = ReckonUnitManagerFactory.GetGZQHReckonUnitManager();
            var reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(orderNo);

            if (reckonUnit == null)
            {
                reckonUnit = new GZQHReckonUnit();
                bool canAdd = reckonUnitManager.AddReckonUnitByMatchOrderNo(orderNo, reckonUnit);

                if (canAdd)
                {
                    reckonUnit.EndReckoningEvent += EndGZQHReckoning2;
                    reckonUnit.EndCancelEvent += EndGZQHCancelReckoning2;
                }
                else
                {
                    reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(orderNo);
                }
            }
            return reckonUnit;
        }

        /// <summary>
        /// 获取商品期货清算单元
        /// </summary>
        /// <param name="orderNo">撮合单号</param>
        /// <returns>商品期货清算单元</returns>
        private SPQHReckonUnit GetSPQHReckonUnit(string orderNo)
        {
            var reckonUnitManager = ReckonUnitManagerFactory.GetSPQHReckonUnitManager();
            var reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(orderNo);
            if (reckonUnit == null)
            {
                reckonUnit = new SPQHReckonUnit();
                bool canAdd = reckonUnitManager.AddReckonUnitByMatchOrderNo(orderNo, reckonUnit);

                if (canAdd)
                {
                    reckonUnit.EndReckoningEvent += EndSPQHReckoning2;
                    reckonUnit.EndCancelEvent += EndSPQHCancelReckoning2;
                }
                else
                {
                    reckonUnit = reckonUnitManager.GetReckonUnitByMatchOrderNo(orderNo);
                }
            }
            return reckonUnit;
        }

        #endregion

        #region 1.1清算逻辑

        #region 撤单清算入口

        #region 现货撤单清算

        /// <summary>
        /// 现货撤单异步回报处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginXHCancelOrderReckoning2(object sender,
                                                  QueueItemHandleEventArg<CancelOrderRptItem> e)
        {
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.Stock)
                {
                    var orderCacheItem = xhCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    XH_TodayEntrustTableInfo tet = null;

                    var coe = e.Item.RptItem;
                    var matchOrderNo = coe.OrderNo;

                    if (coe.IsSuccess) //撤单成功
                    {
                        //进行撮合-委托缓存校验
                        //因为撤单的回报可能返回非常快，从而会导致返回后报盘那里的缓存信息还没有加入，
                        //这样就会导致清算失败，所以先进行判断，缓存中是否存在，如果不存在，那么暂停
                        //线程后再取
                        bool canGet = CanGetOrderCache(xhCounterCache, matchOrderNo);
                        if (!canGet)
                        {
                            //TODO:
                        }

                        XHReckonUnit reckonUnit = GetXHReckonUnit(matchOrderNo);

                        //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                        //    _XHDataLogicProcessor =
                        //        DataLogicProcessorFactory.GetXHDataLogicProcessor(rde.OrderNo);
                        //if (_XHDataLogicProcessor != null)
                        //{
                        /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                        /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                        /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                        /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                        //_XHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.RptItem, out tet, true);
                        //}

                        reckonUnit.InsertMessage(e.Item.RptItem);
                    }
                    else //撤单失败
                    {
                        if (orderCacheItem != null)
                        {
                            tet = ReckonDataLogic.GetXHEntrustEntity(orderCacheItem.CounterOrderNo);

                            if (tet != null)
                            {
                                #region oldcode

                                //if (tet.OrderStatusId ==
                                //    (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                //    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                                //else if (tet.OrderStatusId ==
                                //         (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                //    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                                //tet.OrderMessage = e.Item.RptItem.Message;

                                //OrderOfferDataLogic.UpdateStockOrderStatusAndMessage(tet);

                                #endregion

                                //撤单失败，需要对委托进行校验，看是否是最终状态
                                tet.OrderMessage = coe.Message;
                                if (CheckXHCancelFailureEntrust(tet))
                                {
                                    XHDataAccess.UpdateEntrustTable(tet);
                                }
                                else
                                {
                                    XHDataAccess.UpdateEntrustOrderMessage(tet.EntrustNumber, tet.OrderMessage);
                                }

                                //撤单失败也要推给前台
                                var cancelEndObject =
                                    new ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>();
                                cancelEndObject.TradeID = orderCacheItem.TraderId;
                                cancelEndObject.EntrustTable = tet;
                                cancelEndObject.TradeTableList = new List<XH_TodayTradeTableInfo>();
                                cancelEndObject.IsSuccess = false;
                                cancelEndObject.Message = coe.Message;

                                CounterOrderService.Instance.AcceptStockDealOrder(cancelEndObject);
                            }

                            //如果是失败的撤单，那么这个撤单回报也要从故障恢复中删掉
                            CrashManager.GetInstance().DeleteEntity(coe.Id);
                        }
                    }
                }
                else
                {
                    var OrderCacheItem = xhCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;
                    // _reckonDataLogic.GetQHEntrustEntity(OrderCacheItem.CounterOrderNo);
                    if (tet != null)
                    {
                        if (e.Item.RptItem.IsSuccess) //撤单成功
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else //撤单失败
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                        }
                        OrderOfferDataLogic.UpdateFutureOrder(tet);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:撤单异步回报处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 当撤单失败时，对委托进行一个校验，看与成交是否匹配
        /// </summary>
        /// <param name="tet"></param>
        /// <returns>是否有修改</returns>
        private bool CheckXHCancelFailureEntrust(XH_TodayEntrustTableInfo tet)
        {
            bool result = false;
            try
            {
                var oldTradeAmount = tet.TradeAmount;
                var oldCancelAmount = tet.CancelAmount;
                var oldState = tet.OrderStatusId;

                var tradeList = XHDataAccess.GetTodayTradeListByEntrustNumber(tet.EntrustNumber);

                int tradeAmount = 0;
                int cancelAmount = 0;

                foreach (var trade in tradeList)
                {
                    //成交
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTDealed)
                    {
                        tradeAmount += trade.TradeAmount;
                        continue;
                    }

                    //撤单
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTCanceled)
                    {
                        cancelAmount += trade.TradeAmount;
                        continue;
                    }

                    //内部撤单
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTInternalCanceled)
                    {
                        cancelAmount += trade.TradeAmount;
                        continue;
                    }
                }

                tet.TradeAmount = tradeAmount;
                tet.CancelAmount = cancelAmount;

                //已报或已报待撤
                if (tet.TradeAmount == 0 && tet.CancelAmount == 0)
                {
                    //tet.OrderStatusId = (int) Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    goto end;
                }

                //委托量==成交量+撤单量 全部成交
                if (tet.EntrustAmount == (tet.TradeAmount + tet.CancelAmount))
                {
                    //如果撤单成功的量大于0
                    if (tet.CancelAmount > 0)
                    {
                        //如果撤单成功的量等于委托的量，那么代表全撤,否则是部撤
                        if (tet.EntrustAmount == tet.CancelAmount)
                        {
                            tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else
                        {
                            tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                        }
                    }
                    //否则是没有发生撤单，已成   
                    else
                    {
                        tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSDealed;
                    }
                }
                //委托量>成交量+撤单量 部分成交
                else if (tet.EntrustAmount > (tet.TradeAmount + tet.CancelAmount))
                {
                    if (tet.CancelAmount == 0)
                        tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                    else
                        tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                }

            end:
                if (tet.TradeAmount == oldTradeAmount && tet.CancelAmount == oldCancelAmount &&
                    tet.OrderStatusId == oldState)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }


        /// <summary>
        /// 有价格问题的现货委托信息处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginXHErrorPriceCancelOrderReckoning2(object sender,
                                                            QueueItemHandleEventArg<OrderErrorItem> e)
        {
            string mcOrder = string.Empty;
            string orderStatuid = string.Empty;
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.Stock)
                {
                    //XH_TodayEntrustTableInfo tet;

                    ResultDataEntity rde = e.Item.ErrorItem;
                    string orderNo = rde.OrderNo;

                    //进行撮合-委托缓存校验
                    //因为错误价格的回报返回非常快，从而会导致返回后报盘那里的缓存信息还没有加入，
                    //这样就会导致清算失败，所以先进行判断，缓存中是否存在，如果不存在，那么暂停
                    //线程后再取
                    bool canGet = CanGetOrderCache(xhCounterCache, orderNo);
                    if (!canGet)
                    {
                        //TODO:
                    }

                    XHReckonUnit reckonUnit = GetXHReckonUnit(orderNo);

                    //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    //    _XHDataLogicProcessor =
                    //        DataLogicProcessorFactory.GetXHDataLogicProcessor(e.Item.ErrorItem.OrderNo);

                    /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                    /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                    /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                    /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                    CancelOrderEntity coe = new CancelOrderEntity();
                    coe.Message = rde.Message;
                    coe.OrderNo = rde.OrderNo;
                    coe.OrderVolume = -1;
                    coe.Id = rde.Id;

                    reckonUnit.InsertMessage(coe);

                    //_XHDataLogicProcessor.InstantReckon_CancelOrder(coe, out tet, false);
                    //if (tet != null)
                    //{
                    //    mcOrder = tet.McOrderId;
                    //    orderStatuid = tet.OrderStatusId.Value.ToString();
                    //}
                }
                else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    // OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                    //_SPQHDataLogicProcessor = DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);
                    // return _SPQHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.ErrorItem);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                //TODO:有价格问题的委托信息处理异常
                //_orderOfferDataLogic.UpdateFutureOrder(tet);
                LogHelper.WriteError(ex.Message, ex);
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
        }

        #endregion

        #region 港股撤单清算

        /// <summary>
        /// 港股撤单异步回报处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginHKCancelOrderReckoning2(object sender,
                                                  QueueItemHandleEventArg<CancelOrderRptItem> e)
        {
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.HKStock)
                {
                    var orderCacheItem = hkCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    HK_TodayEntrustInfo tet = null;

                    var coe = e.Item.RptItem;
                    var matchOrderNo = coe.OrderNo;

                    if (coe.IsSuccess) //撤单成功
                    {
                        //进行撮合-委托缓存校验
                        //因为撤单的回报可能返回非常快，从而会导致返回后报盘那里的缓存信息还没有加入，
                        //这样就会导致清算失败，所以先进行判断，缓存中是否存在，如果不存在，那么暂停
                        //线程后再取
                        bool canGet = CanGetOrderCache(hkCounterCache, matchOrderNo);
                        if (!canGet)
                        {
                            //TODO:
                        }

                        HKReckonUnit reckonUnit = GetHKReckonUnit(matchOrderNo);

                        //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                        //    _XHDataLogicProcessor =
                        //        DataLogicProcessorFactory.GetXHDataLogicProcessor(rde.OrderNo);
                        //if (_XHDataLogicProcessor != null)
                        //{
                        /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                        /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                        /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                        /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                        //_XHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.RptItem, out tet, true);
                        //}

                        reckonUnit.InsertMessage(e.Item.RptItem);
                    }
                    else //撤单失败
                    {
                        if (orderCacheItem != null)
                        {
                            tet = ReckonDataLogic.GetHKEntrustEntity(orderCacheItem.CounterOrderNo);

                            if (tet != null)
                            {
                                #region oldcode

                                //if (tet.OrderStatusId ==
                                //    (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                //    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                                //else if (tet.OrderStatusId ==
                                //         (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                //    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                                //tet.OrderMessage = e.Item.RptItem.Message;

                                //OrderOfferDataLogic.UpdateStockOrderStatusAndMessage(tet);

                                #endregion

                                //撤单失败，需要对委托进行校验，看是否是最终状态
                                tet.OrderMessage = coe.Message;
                                if (CheckHKCancelFailureEntrust(tet))
                                {
                                    HKDataAccess.UpdateEntrustTable(tet);
                                }
                                else
                                {
                                    HKDataAccess.UpdateEntrustOrderMessage(tet.EntrustNumber, tet.OrderMessage);
                                }

                                //撤单失败也要推给前台
                                var cancelEndObject =
                                    new ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo>();
                                cancelEndObject.TradeID = orderCacheItem.TraderId;
                                cancelEndObject.EntrustTable = tet;
                                cancelEndObject.TradeTableList = new List<HK_TodayTradeInfo>();
                                cancelEndObject.IsSuccess = false;
                                cancelEndObject.Message = coe.Message;

                                CounterOrderService.Instance.AcceptHKDealOrder(cancelEndObject);
                            }

                            //如果是失败的撤单，那么这个撤单回报也要从故障恢复中删掉
                            CrashManager.GetInstance().DeleteEntity(coe.Id);
                        }
                    }
                }
                else
                {
                    var OrderCacheItem = hkCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;
                    // _reckonDataLogic.GetQHEntrustEntity(OrderCacheItem.CounterOrderNo);
                    if (tet != null)
                    {
                        if (e.Item.RptItem.IsSuccess) //撤单成功
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else //撤单失败
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                        }
                        OrderOfferDataLogic.UpdateFutureOrder(tet);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:撤单异步回报处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 当撤单失败时，对委托进行一个校验，看与成交是否匹配
        /// </summary>
        /// <param name="tet"></param>
        /// <returns>是否有修改</returns>
        private bool CheckHKCancelFailureEntrust(HK_TodayEntrustInfo tet)
        {
            bool result = false;
            try
            {
                var oldTradeAmount = tet.TradeAmount;
                var oldCancelAmount = tet.CancelAmount;
                var oldState = tet.OrderStatusID;

                var tradeList = HKDataAccess.GetTodayTradeListByEntrustNumber(tet.EntrustNumber);

                int tradeAmount = 0;
                int cancelAmount = 0;

                foreach (var trade in tradeList)
                {
                    //成交
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTDealed)
                    {
                        tradeAmount += trade.TradeAmount;
                        continue;
                    }

                    //撤单
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTCanceled)
                    {
                        cancelAmount += trade.TradeAmount;
                        continue;
                    }

                    //内部撤单
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTInternalCanceled)
                    {
                        cancelAmount += trade.TradeAmount;
                        continue;
                    }
                }

                tet.TradeAmount = tradeAmount;
                tet.CancelAmount = cancelAmount;

                //已报或已报待撤
                if (tet.TradeAmount == 0 && tet.CancelAmount == 0)
                {
                    //tet.OrderStatusId = (int) Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    goto end;
                }

                //委托量==成交量+撤单量 全部成交
                if (tet.EntrustAmount == (tet.TradeAmount + tet.CancelAmount))
                {
                    //如果撤单成功的量大于0
                    if (tet.CancelAmount > 0)
                    {
                        //如果撤单成功的量等于委托的量，那么代表全撤,否则是部撤
                        if (tet.EntrustAmount == tet.CancelAmount)
                        {
                            tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else
                        {
                            tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                        }
                    }
                    //否则是没有发生撤单，已成   
                    else
                    {
                        tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSDealed;
                    }
                }
                //委托量>成交量+撤单量 部分成交
                else if (tet.EntrustAmount > (tet.TradeAmount + tet.CancelAmount))
                {
                    if (tet.CancelAmount == 0)
                        tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                    else
                        tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                }

            end:
                if (tet.TradeAmount == oldTradeAmount && tet.CancelAmount == oldCancelAmount &&
                    tet.OrderStatusID == oldState)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }


        /// <summary>
        /// 有价格问题的港股委托信息处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginHKErrorPriceCancelOrderReckoning2(object sender,
                                                            QueueItemHandleEventArg<OrderErrorItem> e)
        {
            string mcOrder = string.Empty;
            string orderStatuid = string.Empty;
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.HKStock)
                {
                    //HK_TodayEntrustInfo tet;

                    ResultDataEntity rde = e.Item.ErrorItem;
                    string orderNo = rde.OrderNo;

                    //进行撮合-委托缓存校验
                    //因为错误价格的回报返回非常快，从而会导致返回后报盘那里的缓存信息还没有加入，
                    //这样就会导致清算失败，所以先进行判断，缓存中是否存在，如果不存在，那么暂停
                    //线程后再取
                    bool canGet = CanGetOrderCache(hkCounterCache, orderNo);
                    if (!canGet)
                    {
                        //TODO:
                    }

                    HKReckonUnit reckonUnit = GetHKReckonUnit(orderNo);

                    //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    //    _XHDataLogicProcessor =
                    //        DataLogicProcessorFactory.GetXHDataLogicProcessor(e.Item.ErrorItem.OrderNo);

                    /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                    /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                    /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                    /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                    CancelOrderEntity coe = new CancelOrderEntity();
                    coe.Message = rde.Message;
                    coe.OrderNo = rde.OrderNo;
                    coe.OrderVolume = -1;
                    coe.Id = rde.Id;

                    reckonUnit.InsertMessage(coe);

                    //_XHDataLogicProcessor.InstantReckon_CancelOrder(coe, out tet, false);
                    //if (tet != null)
                    //{
                    //    mcOrder = tet.McOrderId;
                    //    orderStatuid = tet.OrderStatusId.Value.ToString();
                    //}
                }
                else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    // OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                    //_SPQHDataLogicProcessor = DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);
                    // return _SPQHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.ErrorItem);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                //TODO:有价格问题的委托信息处理异常
                //_orderOfferDataLogic.UpdateFutureOrder(tet);
                LogHelper.WriteError(ex.Message, ex);
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
        }

        #endregion

        #region 商品期货撤单清算

        /// <summary>
        /// 商品期货撤单异步回报处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginSPQHCancelOrderReckoning2(object sender,
                                                    QueueItemHandleEventArg<CancelOrderRptItem> e)
        {
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    var OrderCacheItem = qhCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;

                    var coe = e.Item.RptItem;

                    if (coe.IsSuccess) //撤单成功
                    {
                        //var rde = new ResultDataEntity();
                        //rde.Message = e.Item.RptItem.Message;
                        //rde.OrderNo = e.Item.RptItem.OrderNo;
                        var orderNo = coe.OrderNo;

                        SPQHReckonUnit reckonUnit = GetSPQHReckonUnit(orderNo);

                        //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                        //    _XHDataLogicProcessor =
                        //        DataLogicProcessorFactory.GetXHDataLogicProcessor(rde.OrderNo);
                        //if (_XHDataLogicProcessor != null)
                        //{
                        /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                        /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                        /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                        /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                        //_XHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.RptItem, out tet, true);
                        //}

                        reckonUnit.InsertMessage(e.Item.RptItem);
                    }
                    else //撤单失败
                    {
                        if (OrderCacheItem != null)
                        {
                            tet = ReckonDataLogic.GetQHEntrustEntity(e.Item.RptItem.OrderNo);

                            if (tet != null)
                            {
                                if (tet.OrderStatusId ==
                                    (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                                else if (tet.OrderStatusId ==
                                         (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                                tet.OrderMessage = e.Item.RptItem.Message;

                                OrderOfferDataLogic.UpdateFutureOrder(tet);
                            }
                            else
                            {
                                //TODO:tet=null
                            }
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    var OrderCacheItem = qhCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;
                    // _reckonDataLogic.GetQHEntrustEntity(OrderCacheItem.CounterOrderNo);
                    if (tet != null)
                    {
                        if (e.Item.RptItem.IsSuccess) //撤单成功
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else //撤单失败
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                        }
                        OrderOfferDataLogic.UpdateFutureOrder(tet);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:撤单异步回报处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }


        /// <summary>
        /// 有价格问题的商品期货货委托信息处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginSPQHErrorPriceCancelOrderReckoning2(object sender,
                                                              QueueItemHandleEventArg<OrderErrorItem> e)
        {
            string mcOrder = string.Empty;
            string orderStatuid = string.Empty;
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    //QH_TodayEntrustTableInfo tet;

                    ResultDataEntity rde = e.Item.ErrorItem;
                    string orderNo = rde.OrderNo;

                    SPQHReckonUnit reckonUnit = GetSPQHReckonUnit(orderNo);


                    //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    //    _XHDataLogicProcessor =
                    //        DataLogicProcessorFactory.GetXHDataLogicProcessor(e.Item.ErrorItem.OrderNo);

                    /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                    /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                    /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                    /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                    CancelOrderEntity coe = new CancelOrderEntity();
                    coe.Message = rde.Message;
                    coe.OrderNo = rde.OrderNo;
                    coe.OrderVolume = -1;
                    coe.Id = rde.Id;

                    reckonUnit.InsertMessage(coe);

                    //_XHDataLogicProcessor.InstantReckon_CancelOrder(coe, out tet, false);
                    //if (tet != null)
                    //{
                    //    mcOrder = tet.McOrderId;
                    //    orderStatuid = tet.OrderStatusId.Value.ToString();
                    //}
                }
                else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    // OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                    //_SPQHDataLogicProcessor = DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);
                    // return _SPQHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.ErrorItem);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                //TODO:有价格问题的委托信息处理异常
                //_orderOfferDataLogic.UpdateFutureOrder(tet);
                LogHelper.WriteError(ex.Message, ex);
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
        }

        #endregion

        #region 股指期货撤单清算

        /// <summary>
        /// 股指期货撤单异步回报处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginGZQHCancelOrderReckoning2(object sender,
                                                    QueueItemHandleEventArg<CancelOrderRptItem> e)
        {
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    var orderCacheItem = qhCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;

                    var coe = e.Item.RptItem;

                    if (coe.IsSuccess) //撤单成功
                    {
                        //var rde = new ResultDataEntity();
                        //rde.Message = e.Item.RptItem.Message;
                        //rde.OrderNo = e.Item.RptItem.OrderNo;

                        string orderNo = coe.OrderNo;

                        //进行撮合-委托缓存校验
                        //因为撤单的回报可能返回非常快，从而会导致返回后报盘那里的缓存信息还没有加入，
                        //这样就会导致清算失败，所以先进行判断，缓存中是否存在，如果不存在，那么暂停
                        //线程后再取
                        bool canGet = CanGetOrderCache(qhCounterCache, orderNo);
                        if (!canGet)
                        {
                            //TODO:
                        }

                        GZQHReckonUnit reckonUnit = GetGZQHReckonUnit(orderNo);

                        //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                        //    _XHDataLogicProcessor =
                        //        DataLogicProcessorFactory.GetXHDataLogicProcessor(rde.OrderNo);
                        //if (_XHDataLogicProcessor != null)
                        //{
                        /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                        /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                        /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                        /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                        //_XHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.RptItem, out tet, true);
                        //}

                        reckonUnit.InsertMessage(e.Item.RptItem);
                    }
                    else //撤单失败
                    {
                        if (orderCacheItem != null)
                        {
                            tet = ReckonDataLogic.GetQHEntrustEntity(orderCacheItem.CounterOrderNo);

                            if (tet != null)
                            {
                                #region oldcode

                                //if (tet.OrderStatusId ==
                                //    (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                //    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                                //else if (tet.OrderStatusId ==
                                //         (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                //    tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                                //tet.OrderMessage = e.Item.RptItem.Message;

                                //OrderOfferDataLogic.UpdateFutureOrder(tet);

                                #endregion

                                //撤单失败，需要对委托进行校验，看是否是最终状态
                                tet.OrderMessage = coe.Message;
                                if (CheckGZQHCancelFailureEntrust(tet))
                                {
                                    QHDataAccess.UpdateEntrustTable(tet);
                                }
                                else
                                {
                                    QHDataAccess.UpdateEntrustOrderMessage(tet.EntrustNumber, tet.OrderMessage);
                                }

                                //撤单失败也要推给前台
                                var cancelEndObject =
                                    new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                                cancelEndObject.TradeID = orderCacheItem.TraderId;
                                cancelEndObject.EntrustTable = tet;
                                cancelEndObject.TradeTableList = new List<QH_TodayTradeTableInfo>();
                                cancelEndObject.IsSuccess = false;
                                cancelEndObject.Message = coe.Message;

                                CounterOrderService.Instance.AcceptStockIndexFuturesDealOrder(cancelEndObject);
                            }

                            //如果是失败的撤单，那么这个撤单回报也要从故障恢复中删掉
                            CrashManager.GetInstance().DeleteEntity(coe.Id);
                        }
                    }
                }
                else
                {
                    var OrderCacheItem = qhCounterCache.GetOrderMappingInfo(e.Item.RptItem.OrderNo);
                    QH_TodayEntrustTableInfo tet = null;
                    // _reckonDataLogic.GetQHEntrustEntity(OrderCacheItem.CounterOrderNo);
                    if (tet != null)
                    {
                        if (e.Item.RptItem.IsSuccess) //撤单成功
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else //撤单失败
                        {
                            if (tet.OrderStatusId == (int)Entity.Contants.Types.OrderStateType.DOSPartDealRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                            else if (tet.OrderStatusId ==
                                     (int)Entity.Contants.Types.OrderStateType.DOSRequiredRemoveSoon)
                                tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSIsRequired;
                        }
                        OrderOfferDataLogic.UpdateFutureOrder(tet);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:撤单异步回报处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 当撤单失败时，对委托进行一个校验，看与成交是否匹配
        /// </summary>
        /// <param name="tet"></param>
        private bool CheckGZQHCancelFailureEntrust(QH_TodayEntrustTableInfo tet)
        {
            bool result = false;
            try
            {
                var oldTradeAmount = tet.TradeAmount;
                var oldCancelAmount = tet.CancelAmount;
                var oldState = tet.OrderStatusId;

                var tradeList = QHDataAccess.GetTodayTradeListByEntrustNumber(tet.EntrustNumber);

                int tradeAmount = 0;
                int cancelAmount = 0;

                foreach (var trade in tradeList)
                {
                    //成交
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTDealed)
                    {
                        tradeAmount += trade.TradeAmount;
                        continue;
                    }

                    //撤单
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTCanceled)
                    {
                        cancelAmount += trade.TradeAmount;
                        continue;
                    }

                    //内部撤单
                    if (trade.TradeTypeId == (int)Entity.Contants.Types.DealRptType.DRTInternalCanceled)
                    {
                        cancelAmount += trade.TradeAmount;
                        continue;
                    }
                }

                tet.TradeAmount = tradeAmount;
                tet.CancelAmount = cancelAmount;

                //已报或已报待撤
                if (tet.TradeAmount == 0 && tet.CancelAmount == 0)
                {
                    //tet.OrderStatusId = (int) Entity.Contants.Types.OrderStateType.DOSIsRequired;
                    goto end;
                }

                //委托量==成交量+撤单量 全部成交
                if (tet.EntrustAmount == (tet.TradeAmount + tet.CancelAmount))
                {
                    //如果撤单成功的量大于0
                    if (tet.CancelAmount > 0)
                    {
                        //如果撤单成功的量等于委托的量，那么代表全撤,否则是部撤
                        if (tet.EntrustAmount == tet.CancelAmount)
                        {
                            tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSRemoved;
                        }
                        else
                        {
                            tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                        }
                    }
                    //否则是没有发生撤单，已成   
                    else
                    {
                        tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSDealed;
                    }
                }
                //委托量>成交量+撤单量 部分成交
                else if (tet.EntrustAmount > (tet.TradeAmount + tet.CancelAmount))
                {
                    if (tet.CancelAmount == 0)
                        tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartDealed;
                    else
                        tet.OrderStatusId = (int)Entity.Contants.Types.OrderStateType.DOSPartRemoved;
                }

            end:
                if (tet.TradeAmount == oldTradeAmount && tet.CancelAmount == oldCancelAmount &&
                    tet.OrderStatusId == oldState)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }


        /// <summary>
        /// 有价格问题的股指期货货委托信息处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginGZQHErrorPriceCancelOrderReckoning2(object sender,
                                                              QueueItemHandleEventArg<OrderErrorItem> e)
        {
            string mcOrder = string.Empty;
            string orderStatuid = string.Empty;
            try
            {
                if (e.Item.BreedClassType == Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    //QH_TodayEntrustTableInfo tet;

                    ResultDataEntity rde = e.Item.ErrorItem;
                    string orderNo = rde.OrderNo;

                    //进行撮合-委托缓存校验
                    //因为错误价格的回报返回非常快，从而会导致返回后报盘那里的缓存信息还没有加入，
                    //这样就会导致清算失败，所以先进行判断，缓存中是否存在，如果不存在，那么暂停
                    //线程后再取
                    bool canGet = CanGetOrderCache(qhCounterCache, orderNo);
                    if (!canGet)
                    {
                        //TODO:
                    }

                    GZQHReckonUnit reckonUnit = GetGZQHReckonUnit(orderNo);


                    //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity>
                    //    _XHDataLogicProcessor =
                    //        DataLogicProcessorFactory.GetXHDataLogicProcessor(e.Item.ErrorItem.OrderNo);

                    /// 因为原来撤单逻辑不对量进行判断，所以改为传递撤单回报对象。
                    /// 但是当为错误价格委托时，不需要量，所以coe.OrderVolume赋-1代表错误价格委托撤单
                    /// 这么做是因为正常撤单和错误价格撤单原来共用了一个方法，逻辑处理错误
                    /// 目前还是共用一个方法，但是内部通过量是否为-1来判断属于那种撤单
                    CancelOrderEntity coe = new CancelOrderEntity();
                    coe.Message = rde.Message;
                    coe.OrderNo = rde.OrderNo;
                    coe.OrderVolume = -1;
                    coe.Id = rde.Id;

                    reckonUnit.InsertMessage(coe);

                    //_XHDataLogicProcessor.InstantReckon_CancelOrder(coe, out tet, false);
                    //if (tet != null)
                    //{
                    //    mcOrder = tet.McOrderId;
                    //    orderStatuid = tet.OrderStatusId.Value.ToString();
                    //}
                }
                else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
                {
                    // OrderLogicFlowBase<MercantileFuturesOrderRequest, QH_TodayEntrustTableInfo, CommoditiesDealBackEntity>
                    //_SPQHDataLogicProcessor = DataLogicProcessorFactory.GetSPQHDataLogicProcessor(ido.OrderNo);
                    // return _SPQHDataLogicProcessor.InstantReckon_CancelOrder(e.Item.ErrorItem);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                //TODO:有价格问题的委托信息处理异常
                //_orderOfferDataLogic.UpdateFutureOrder(tet);
                LogHelper.WriteError(ex.Message, ex);
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
        }

        #endregion

        #endregion

        #region 成交清算入口

        #region 现货清算处理

        /// <summary>
        /// 开始现货清算处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginXHReckoning2(object sender, QueueItemHandleEventArg<StockDealBackEntity> e)
        {
            try
            {
                if (e == null)
                    return;

                if (e.Item == null)
                    return;

                StockDealBackEntity ido = e.Item;
                if (ido == null)
                    return;

                //进行撮合-委托缓存校验
                bool canGet = CanGetOrderCache(xhCounterCache, ido.OrderNo);
                if (!canGet)
                {
                    //TODO:
                }

                XHReckonUnit reckonUnit = GetXHReckonUnit(ido.OrderNo);

                reckonUnit.InsertMessage(ido);
            }
            catch (Exception ex)
            {
                //TODO:现货清算处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 现货清算完后的通知事件接收方法
        /// </summary>
        /// <param name="stockReckoningResultObject"></param>
        private void EndXHReckoning2(ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptStockDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                if (tet != null)
                {
                    xhCounterCache.RemoveOrderMappingInfo(tet.McOrderId,
                                                                 (Entity.Contants.Types.OrderStateType)
                                                                 Enum.Parse(
                                                                     typeof(Entity.Contants.Types.OrderStateType),
                                                                     tet.OrderStatusId.ToString()));

                    XHReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderId,
                                                                                     (
                                                                                     Entity.Contants.Types.
                                                                                         OrderStateType)
                                                                                     tet.OrderStatusId);
                }
            }


            //var item = stockReckoningResultObject.DealBackEntity;
            //string strDealNo = stockReckoningResultObject.DealNo;
            //var tet = stockReckoningResultObject.EntrustTable;

            //if (stockReckoningResultObject.isSuccess)
            //{
            //    var rpt = this.XHDealRptConvert(item, strDealNo, tet);
            //    _counterOrderService.AcceptStockDealOrder(rpt, tet.CallbackChannlId);
            //}

            //if (tet != null)
            //{
            //    CounterCache.Instance.RemoveOrderMappingInfo(tet.McOrderId,
            //                                                 (Entity.Contants.Types.OrderStateType)
            //                                                 Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
            //                                                            tet.OrderStatusId.ToString()));

            //    XHLogciFlowManager.GetInstance().RemoveLogicFlowByMatchOrderNo(item.OrderNo,
            //                                                                   (Entity.Contants.Types.OrderStateType)
            //                                                                   Enum.Parse(
            //                                                                       typeof(
            //                                                                           Entity.Contants.Types.
            //                                                                           OrderStateType),
            //                                                                       tet.OrderStatusId.ToString()));
            //}
        }


        /// <summary>
        /// 现货撤单完后的通知事件接收方法
        /// </summary>
        /// <param name="cancelObject"></param>
        private void EndXHCancelReckoning2(
            ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptStockDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                xhCounterCache.RemoveOrderMappingInfo(tet.McOrderId,
                                                             (Entity.Contants.Types.OrderStateType)
                                                             Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
                                                                        tet.OrderStatusId.ToString()));

                XHReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderId,
                                                                                 (Entity.Contants.Types.OrderStateType)
                                                                                 tet.OrderStatusId);
            }


            //CancelOrderEntity coe = cancelObject.CancelEntity;


            //if (coe.OrderVolume == -1)
            //{
            //    //有价格问题的委托信息处理
            //    var tet = cancelObject.EntrustTable;

            //    string mcOrder = string.Empty;
            //    string orderStatuid = string.Empty;

            //    if (tet != null)
            //    {
            //        mcOrder = tet.McOrderId;
            //        orderStatuid = tet.OrderStatusId.ToString();
            //    }

            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
            //else
            //{
            //    //撤单异步回报处理
            //}
        }

        #endregion

        #region 港股清算处理

        /// <summary>
        /// 开始港股清算处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginHKReckoning2(object sender, QueueItemHandleEventArg<HKDealBackEntity> e)
        {
            try
            {
                if (e == null)
                    return;

                if (e.Item == null)
                    return;

                HKDealBackEntity ido = e.Item;
                if (ido == null)
                    return;

                //进行撮合-委托缓存校验
                bool canGet = CanGetOrderCache(hkCounterCache, ido.OrderNo);
                if (!canGet)
                {
                    //TODO:
                }

                HKReckonUnit reckonUnit = GetHKReckonUnit(ido.OrderNo);

                reckonUnit.InsertMessage(ido);
            }
            catch (Exception ex)
            {
                //TODO:现货清算处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 港股清算完后的通知事件接收方法
        /// </summary>
        /// <param name="stockReckoningResultObject"></param>
        private void EndHKReckoning2(ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptHKDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                if (tet != null)
                {
                    hkCounterCache.RemoveOrderMappingInfo(tet.McOrderID,
                                                                 (Entity.Contants.Types.OrderStateType)
                                                                 Enum.Parse(
                                                                     typeof(Entity.Contants.Types.OrderStateType),
                                                                     tet.OrderStatusID.ToString()));

                    HKReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderID,
                                                                                     (
                                                                                     Entity.Contants.Types.
                                                                                         OrderStateType)
                                                                                     tet.OrderStatusID);
                }
            }


            //var item = stockReckoningResultObject.DealBackEntity;
            //string strDealNo = stockReckoningResultObject.DealNo;
            //var tet = stockReckoningResultObject.EntrustTable;

            //if (stockReckoningResultObject.isSuccess)
            //{
            //    var rpt = this.XHDealRptConvert(item, strDealNo, tet);
            //    _counterOrderService.AcceptStockDealOrder(rpt, tet.CallbackChannlId);
            //}

            //if (tet != null)
            //{
            //    CounterCache.Instance.RemoveOrderMappingInfo(tet.McOrderId,
            //                                                 (Entity.Contants.Types.OrderStateType)
            //                                                 Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
            //                                                            tet.OrderStatusId.ToString()));

            //    XHLogciFlowManager.GetInstance().RemoveLogicFlowByMatchOrderNo(item.OrderNo,
            //                                                                   (Entity.Contants.Types.OrderStateType)
            //                                                                   Enum.Parse(
            //                                                                       typeof(
            //                                                                           Entity.Contants.Types.
            //                                                                           OrderStateType),
            //                                                                       tet.OrderStatusId.ToString()));
            //}
        }


        /// <summary>
        /// 港股撤单完后的通知事件接收方法
        /// </summary>
        /// <param name="cancelObject"></param>
        private void EndHKCancelReckoning2(
            ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptHKDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                hkCounterCache.RemoveOrderMappingInfo(tet.McOrderID,
                                                             (Entity.Contants.Types.OrderStateType)
                                                             Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
                                                                        tet.OrderStatusID.ToString()));

                HKReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderID,
                                                                                 (Entity.Contants.Types.OrderStateType)
                                                                                 tet.OrderStatusID);
            }


            //CancelOrderEntity coe = cancelObject.CancelEntity;


            //if (coe.OrderVolume == -1)
            //{
            //    //有价格问题的委托信息处理
            //    var tet = cancelObject.EntrustTable;

            //    string mcOrder = string.Empty;
            //    string orderStatuid = string.Empty;

            //    if (tet != null)
            //    {
            //        mcOrder = tet.McOrderId;
            //        orderStatuid = tet.OrderStatusId.ToString();
            //    }

            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
            //else
            //{
            //    //撤单异步回报处理
            //}
        }

        #endregion

        #region 商品期货清算处理

        /// <summary>
        /// 开始商品期货清算处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginSPQHReckoning2(object sender,
                                         QueueItemHandleEventArg<CommoditiesDealBackEntity> e)
        {
            try
            {
                CommoditiesDealBackEntity ido = e.Item;
                string orderNo = ido.OrderNo;

                SPQHReckonUnit reckonUnit = GetSPQHReckonUnit(orderNo);

                reckonUnit.InsertMessage(ido);
            }
            catch (Exception ex)
            {
                //TODO:现货清算处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void EndSPQHCancelReckoning2(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptMercantileFuturesOrderDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                if (tet != null)
                {
                    qhCounterCache.RemoveOrderMappingInfo(tet.McOrderId, (Entity.Contants.Types.OrderStateType)Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
                                                                     tet.OrderStatusId.ToString()));
                    GZQHReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderId, (Entity.Contants.Types.OrderStateType)tet.OrderStatusId);
                }
            }

        }

        private void EndSPQHReckoning2(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptMercantileFuturesOrderDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                if (tet != null)
                {
                    qhCounterCache.RemoveOrderMappingInfo(tet.McOrderId, (Entity.Contants.Types.OrderStateType)Enum.Parse(typeof(Entity.Contants.Types.OrderStateType), tet.OrderStatusId.ToString()));
                    GZQHReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderId, (Entity.Contants.Types.OrderStateType)tet.OrderStatusId);
                }
            }

            #region old code
            //var item = qhReckoningResultObject.DealBackEntity;
            //string strDealNo = qhReckoningResultObject.DealNo;
            //var tet = qhReckoningResultObject.EntrustTable;

            //if (qhReckoningResultObject.isSuccess)
            //{
            //    var rpt = this.SPQHDealRptConvert(item, strDealNo, tet.EntrustNumber);
            //    _counterOrderService.AcceptStockIndexFuturesDealOrder(rpt, tet.CallbackChannelId);
            //}

            //if (tet != null)
            //{
            //    CounterCache.Instance.RemoveOrderMappingInfo(tet.McOrderId,
            //                                                 (Entity.Contants.Types.OrderStateType)
            //                                                 Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
            //                                                            tet.OrderStatusId.ToString()));

            //    XHLogciFlowManager.GetInstance().RemoveLogicFlowByMatchOrderNo(item.OrderNo,
            //                                                                   (Entity.Contants.Types.OrderStateType)
            //                                                                   Enum.Parse(
            //                                                                       typeof(
            //                                                                           Entity.Contants.Types.
            //                                                                           OrderStateType),
            //                                                                       tet.OrderStatusId.ToString()));
            //}
            #endregion
        }

        #endregion

        #region 股指期货清算处理

        /// <summary>
        /// 开始股指期货清算处理
        /// 使用异步处理的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginGZQHReckoning2(object sender,
                                         QueueItemHandleEventArg<FutureDealBackEntity> e)
        {
            try
            {
                FutureDealBackEntity ido = e.Item;

                string orderNo = ido.OrderNo;

                //进行撮合-委托缓存校验
                bool canGet = CanGetOrderCache(qhCounterCache, ido.OrderNo);
                if (!canGet)
                {
                    //TODO:
                }

                GZQHReckonUnit reckonUnit = GetGZQHReckonUnit(orderNo);

                reckonUnit.InsertMessage(ido);
            }
            catch (Exception ex)
            {
                //TODO:现货清算处理异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void EndGZQHCancelReckoning2(
            ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptStockIndexFuturesDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                if (tet != null)
                {
                    qhCounterCache.RemoveOrderMappingInfo(tet.McOrderId,
                                                                 (Entity.Contants.Types.OrderStateType)
                                                                 Enum.Parse(
                                                                     typeof(Entity.Contants.Types.OrderStateType),
                                                                     tet.OrderStatusId.ToString()));
                    GZQHReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderId,
                                                                                       (
                                                                                       Entity.Contants.Types.
                                                                                           OrderStateType)
                                                                                       tet.OrderStatusId);
                }
            }

            //CancelOrderEntity coe = cancelObject.CancelEntity;


            //if (coe.OrderVolume == -1)
            //{
            //    //有价格问题的委托信息处理
            //    var tet = cancelObject.EntrustTable;

            //    string mcOrder = string.Empty;
            //    string orderStatuid = string.Empty;

            //    if (tet != null)
            //    {
            //        mcOrder = tet.McOrderId;
            //        orderStatuid = tet.OrderStatusId.ToString();
            //    }

            //    if (!string.IsNullOrEmpty(mcOrder) && !string.IsNullOrEmpty(orderStatuid))
            //        CounterCache.Instance.RemoveOrderMappingInfo(mcOrder,
            //                                                     (Entity.Contants.Types.OrderStateType)
            //                                                     Enum.Parse(
            //                                                         typeof(Entity.Contants.Types.OrderStateType),
            //                                                         orderStatuid));
            //}
            //else
            //{
            //    //撤单异步回报处理
            //}
        }

        private void EndGZQHReckoning2(
            ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject.IsSuccess)
            {
                _counterOrderService.AcceptStockIndexFuturesDealOrder(reckonEndObject);

                var tet = reckonEndObject.EntrustTable;
                if (tet != null)
                {
                    qhCounterCache.RemoveOrderMappingInfo(tet.McOrderId,
                                                                 (Entity.Contants.Types.OrderStateType)
                                                                 Enum.Parse(
                                                                     typeof(Entity.Contants.Types.OrderStateType),
                                                                     tet.OrderStatusId.ToString()));
                    GZQHReckonUnitManager.GetInstance().RemoveReckonUnitByMatchOrderNo(tet.McOrderId,
                                                                                       (
                                                                                       Entity.Contants.Types.
                                                                                           OrderStateType)
                                                                                       tet.OrderStatusId);
                }
            }
            //var item = qhReckoningResultObject.DealBackEntity;
            //string strDealNo = qhReckoningResultObject.DealNo;
            //var tet = qhReckoningResultObject.EntrustTable;

            //if (qhReckoningResultObject.isSuccess)
            //{
            //    var rpt = this.GZQHDealRptConvert(item, strDealNo, tet.EntrustNumber);
            //    _counterOrderService.AcceptStockIndexFuturesDealOrder(rpt, tet.CallbackChannelId);
            //}

            //if (tet != null)
            //{
            //    CounterCache.Instance.RemoveOrderMappingInfo(tet.McOrderId,
            //                                                 (Entity.Contants.Types.OrderStateType)
            //                                                 Enum.Parse(typeof(Entity.Contants.Types.OrderStateType),
            //                                                            tet.OrderStatusId.ToString()));

            //    XHLogciFlowManager.GetInstance().RemoveLogicFlowByMatchOrderNo(item.OrderNo,
            //                                                                   (Entity.Contants.Types.OrderStateType)
            //                                                                   Enum.Parse(
            //                                                                       typeof(
            //                                                                           Entity.Contants.Types.
            //                                                                           OrderStateType),
            //                                                                       tet.OrderStatusId.ToString()));
            //}
        }

        #endregion

        #endregion

        #endregion

        #region == 回报类型转换 ==

        /*
        /// <summary>
        /// 构建柜台商品期货成交回报
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dealRptId"></param>
        /// <param name="strCounterOrderId"></param>
        /// <returns></returns>
        private FutureDealOrderPushBack SPQHDealRptConvert(CommoditiesDealBackEntity obj, string dealRptId,
                                                           string strCounterOrderId)
        {
            var result = new FutureDealOrderPushBack();
            if (obj != null)
            {
                result.DealAmount = Convert.ToInt32(obj.DealAmount);
                result.DealDatetime = obj.DealTime;
                result.DealId = dealRptId;
                result.DealPrice = obj.DealPrice;
                result.OrderId = strCounterOrderId;
            }
            return result;
        }

        /// <summary>
        /// 构造柜台股指期货成交回报
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dealRptId"></param>
        /// <param name="strCounterOrderId"></param>
        /// <returns></returns>
        private FutureDealOrderPushBack GZQHDealRptConvert(FutureDealBackEntity obj, string dealRptId,
                                                           string strCounterOrderId)
        {
            var result = new FutureDealOrderPushBack();
            if (obj != null)
            {
                result.DealAmount = Convert.ToInt32(obj.DealAmount);
                result.DealDatetime = obj.DealTime;
                result.DealId = dealRptId;
                result.DealPrice = obj.DealPrice;
                result.OrderId = strCounterOrderId;
            }
            return result;
        }

        /// <summary>
        /// 构造柜台现货成交回报
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dealRptId"></param>
        /// <param name="strCounterOrderId"></param>
        /// <returns></returns>
        private StockDealOrderPushBack XHDealRptConvert(StockDealBackEntity obj, string dealRptId,
                                                        XH_TodayEntrustTableInfo tet)
        {
            var result = new StockDealOrderPushBack();
            if (obj != null)
            {
                result.DealAmount = Convert.ToInt32(obj.DealAmount);
                result.DealDatetime = obj.DealTime;
                result.DealId = dealRptId;
                result.DealPrice = obj.DealPrice;
                result.OrderId = tet.EntrustNumber;
                result.OrderState =
                    (Entity.Contants.Types.OrderStateType) Enum.Parse(typeof (Entity.Contants.Types.OrderStateType),
                                                                      tet.OrderStatusId.ToString());
            }
            return result;
        }*/

        #endregion

        #region == 接收各种回报 ==

        #region == 接收成交回报 ==

        #region 接收现货成交回报并分发

        /// <summary>
        /// 接收报盘中心现货成交回报方法
        /// </summary>
        /// <param name="ido">成交回报</param>
        public void AcceptXHDealOrder(StockDealBackEntity ido)
        {
            DispatchXHOrder(ido);
            string message = "OrderID:" + ido.OrderNo + "-- DealPrice:" + ido.DealPrice
                             + "--DealAmount : " + ido.DealAmount + " -- Dealtime :" + ido.DealTime;

            LogHelper.WriteDebug("<------接收报盘中心现货成交回报方法ReckonCenter.AcceptXHDealOrder[" + message + "]");
            this.FireDealRptCallbackEvent(message);
        }

        /// <summary>
        /// 分发现货成交清算
        /// </summary>
        /// <param name="order"></param>
        private void DispatchXHOrder(StockDealBackEntity order)
        {
            if (order == null)
                return;

            ExecuteXHDealThreadWork(order);
            return;

            #region 旧分发逻辑

            /*QueueBufferBase<StockDealBackEntity> cache = null;
            foreach (var queueBufferBase in _stockQueueBuffers)
            {
                if (cache == null || queueBufferBase.BufferedItemCount < cache.BufferedItemCount)
                    cache = queueBufferBase;
            }
            if (cache == null)
            {
                LogHelper.WriteDebug("ReckonCenter.DispatchXHOrder无法找到报盘队列，分发现货成交清算失败.");
                return;
            }
            cache.InsertQueueItem(order);
            LogHelper.WriteDebug("ReckonCenter.DispatchXHOrder分发现货成交清算[DealBuffer=" + cache.Name + "]");
            */

            #endregion
        }

        private void ExecuteXHDealThreadWork(StockDealBackEntity order)
        {
            try
            {
                smartPool.QueueWorkItem(BeginXHReckoning2, this,
                                        new QueueItemHandleEventArg<StockDealBackEntity>(order));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteXHDealThreadWork", ex);
                BeginXHReckoning2(this, new QueueItemHandleEventArg<StockDealBackEntity>(order));
            }
        }

        #endregion

        #region 接收港股成交回报并分发

        /// <summary>
        /// 接收报盘中心港股成交回报方法
        /// </summary>
        /// <param name="ido">成交回报</param>
        public void AcceptHKDealOrder(HKDealBackEntity ido)
        {
            DispatchHKOrder(ido);
            string message = "OrderID:" + ido.OrderNo + "-- DealPrice:" + ido.DealPrice
                             + "--DealAmount : " + ido.DealAmount + " -- Dealtime :" + ido.DealTime;

            LogHelper.WriteDebug("<------接收报盘中心港股成交回报方法ReckonCenter.AcceptHKDealOrder[" + message + "]");
            this.FireDealRptCallbackEvent(message);
        }

        /// <summary>
        /// 分发港股成交清算
        /// </summary>
        /// <param name="order"></param>
        private void DispatchHKOrder(HKDealBackEntity order)
        {
            if (order == null)
                return;

            ExecuteHKDealThreadWork(order);
            return;

            #region 旧分发逻辑

            /*QueueBufferBase<StockDealBackEntity> cache = null;
            foreach (var queueBufferBase in _stockQueueBuffers)
            {
                if (cache == null || queueBufferBase.BufferedItemCount < cache.BufferedItemCount)
                    cache = queueBufferBase;
            }
            if (cache == null)
            {
                LogHelper.WriteDebug("ReckonCenter.DispatchXHOrder无法找到报盘队列，分发现货成交清算失败.");
                return;
            }
            cache.InsertQueueItem(order);
            LogHelper.WriteDebug("ReckonCenter.DispatchXHOrder分发现货成交清算[DealBuffer=" + cache.Name + "]");
            */

            #endregion
        }

        private void ExecuteHKDealThreadWork(HKDealBackEntity order)
        {
            try
            {
                smartPool.QueueWorkItem(BeginHKReckoning2, this,
                                        new QueueItemHandleEventArg<HKDealBackEntity>(order));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteXHDealThreadWork", ex);
                BeginHKReckoning2(this, new QueueItemHandleEventArg<HKDealBackEntity>(order));
            }
        }

        #endregion

        #region 接收股指期货成交回报并分发

        /// <summary>
        /// 接收报盘中心股指期货成交回报方法
        /// </summary>
        /// <param name="ido"></param>
        public void AcceptGZQHDealOrder(FutureDealBackEntity ido)
        {
            DispatchGZQHOrder(ido);
            string message = "OrderID:" + ido.OrderNo + "-- DealPrice:" + ido.DealPrice
                             + "--DealAmount : " + ido.DealAmount + " -- Dealtime :" + ido.DealTime;

            LogHelper.WriteDebug("<------接收报盘中心股指期货成交回报方法ReckonCenter.AcceptGZQHDealOrder[" + message + "]");
            this.FireDealRptCallbackEvent(message);
        }

        /// <summary>
        /// 分发股指期货成交清算
        /// </summary>
        /// <param name="order"></param>
        private void DispatchGZQHOrder(FutureDealBackEntity order)
        {
            if (order == null)
                return;

            ExecuteGZQHDealThreadWork(order);
            return;

            #region 旧分发逻辑

            /*
            QueueBufferBase<FutureDealBackEntity> cache = null;
            foreach (var queueBufferBase in _stockIndexFutureQueueBuffers)
            {
                if (cache == null || queueBufferBase.BufferedItemCount < cache.BufferedItemCount)
                    cache = queueBufferBase;
            }
            if (cache == null)
            {
                LogHelper.WriteDebug("ReckonCenter.DispatchGZQHOrder无法找到报盘队列，分发股指期货成交清算失败.");

                return;
            }
            cache.InsertQueueItem(order);**/

            #endregion
        }

        private void ExecuteGZQHDealThreadWork(FutureDealBackEntity order)
        {
            try
            {
                smartPool.QueueWorkItem(BeginGZQHReckoning2, this,
                                        new QueueItemHandleEventArg<FutureDealBackEntity>(order));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteSPQHDealThreadWork", ex);
                BeginGZQHReckoning2(this, new QueueItemHandleEventArg<FutureDealBackEntity>(order));
            }
        }

        #endregion

        #region 接收商品期货成交回报并分发

        /// <summary>
        ///  接收报盘中中商品期货成交回报方法
        /// </summary>
        /// <param name="ido"></param>
        public void AcceptSPQHDealOrder(CommoditiesDealBackEntity ido)
        {
            DispatchSPQHOrder(ido);
            string message = "OrderID:" + ido.OrderNo + "-- DealPrice:" + ido.DealPrice
                             + "--DealAmount : " + ido.DealAmount + " -- Dealtime :" + ido.DealTime;

            LogHelper.WriteDebug("<------接收报盘中心商品期货成交回报方法ReckonCenter.AcceptSPQHDealOrder[" + message + "]");
            this.FireDealRptCallbackEvent(message);
        }

        /// <summary>
        /// 分发商品期货成交清算
        /// </summary>
        /// <param name="order"></param>
        private void DispatchSPQHOrder(CommoditiesDealBackEntity order)
        {
            if (order == null)
                return;

            ExecuteSPQHDealThreadWork(order);
            return;

            #region 旧分发逻辑

            /*
            QueueBufferBase<CommoditiesDealBackEntity> cache = null;
            foreach (var queueBufferBase in _mercantileFutureQueueBuffers)
            {
                if (cache == null || queueBufferBase.BufferedItemCount < cache.BufferedItemCount)
                    cache = queueBufferBase;
            }
            if (cache == null)
            {
                LogHelper.WriteDebug("ReckonCenter.DispatchQHOrder无法找到报盘队列，分发商品期货成交清算失败.");

                return;
            }
            cache.InsertQueueItem(order);**/

            #endregion
        }

        private void ExecuteSPQHDealThreadWork(CommoditiesDealBackEntity order)
        {
            try
            {
                smartPool.QueueWorkItem(BeginSPQHReckoning2, this,
                                        new QueueItemHandleEventArg<CommoditiesDealBackEntity>(order));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteSPQHDealThreadWork", ex);
                BeginSPQHReckoning2(this, new QueueItemHandleEventArg<CommoditiesDealBackEntity>(order));
            }
        }

        #endregion

        #endregion

        #region == 接收委托回报(错误及废单) ==

        #region 接收现货委托回报(错误及废单)

        /// <summary>
        ///  现货委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void AcceptXHErrorOrderRpt(ResultDataEntity model)
        {
            OrderErrorItem item = new OrderErrorItem(model,
                                                     Types.BreedClassTypeEnum.Stock);
            DispatchXHErrorOrder(item);

            //this._errorPriceOrderHolder.InsertQueueItem(item);
            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message;

            LogHelper.WriteDebug("<------现货委托回报价格异常(废单)ReckonCenter.AcceptXHErrorOrderRpt[" + message + "]");
            this.FireOrderRptCallbackEvent(message);
        }

        private void DispatchXHErrorOrder(OrderErrorItem item)
        {
            if (item == null)
                return;

            ExecuteXHErrorThreadWork(item);
            return;
        }

        private void ExecuteXHErrorThreadWork(OrderErrorItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginXHErrorPriceCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteXHErrorThreadWork", ex);
                BeginXHErrorPriceCancelOrderReckoning2(this, new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
        }

        #endregion

        #region 接收港股委托回报(错误及废单)

        /// <summary>
        ///  港股委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void AcceptHKErrorOrderRpt(ResultDataEntity model)
        {
            OrderErrorItem item = new OrderErrorItem(model, Types.BreedClassTypeEnum.HKStock);
            DispatchHKErrorOrder(item);

            //this._errorPriceOrderHolder.InsertQueueItem(item);
            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message;

            LogHelper.WriteDebug("<------港股委托回报价格异常(废单)ReckonCenter.AcceptHKErrorOrderRpt[" + message + "]");
            this.FireOrderRptCallbackEvent(message);
        }

        private void DispatchHKErrorOrder(OrderErrorItem item)
        {
            if (item == null)
                return;

            ExecuteHKErrorThreadWork(item);
            return;
        }

        private void ExecuteHKErrorThreadWork(OrderErrorItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginHKErrorPriceCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteHKErrorThreadWork", ex);
                BeginHKErrorPriceCancelOrderReckoning2(this, new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
        }

        #endregion

        #region 接收股指期货委托回报(错误及废单)

        /// <summary>
        /// 股指期货委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void AcceptGZQHErrorOrderRpt(ResultDataEntity model)
        {
            OrderErrorItem item = new OrderErrorItem(model,
                                                     Types.BreedClassTypeEnum.
                                                         StockIndexFuture);
            DispatchGZQHErrorOrder(item);

            //this._GZQHErrorPriceOrderHolder.InsertQueueItem(item);
            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message;

            LogHelper.WriteDebug("<------股指期货委托回报价格异常(废单)ReckonCenter.AcceptGZQHErrorOrderRpt[" + message + "]");
            this.FireOrderRptCallbackEvent(message);
        }

        private void DispatchGZQHErrorOrder(OrderErrorItem item)
        {
            if (item == null)
                return;

            ExecuteGZQHErrorThreadWork(item);
            return;
        }

        private void ExecuteGZQHErrorThreadWork(OrderErrorItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginGZQHErrorPriceCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteGZQHErrorThreadWork", ex);
                BeginGZQHErrorPriceCancelOrderReckoning2(this, new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
        }

        #endregion

        #region 接收商品期货委托回报(错误及废单)

        /// <summary>
        /// 商品期货委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void AcceptSPQHErrorOrderRpt(ResultDataEntity model)
        {
            OrderErrorItem item = new OrderErrorItem(model,
                                                     Types.BreedClassTypeEnum.
                                                         CommodityFuture);
            DispatchSPQHErrorOrder(item);
            //this._SPQHErrorPriceOrderHolder.InsertQueueItem(item);

            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message;

            LogHelper.WriteDebug("<------商品期货委托回报价格异常(废单)ReckonCenter.AcceptSPQHErrorOrderRpt[" + message + "]");
            this.FireOrderRptCallbackEvent(message);
        }

        private void DispatchSPQHErrorOrder(OrderErrorItem item)
        {
            if (item == null)
                return;

            ExecuteSPQHErrorThreadWork(item);
            return;
        }

        private void ExecuteSPQHErrorThreadWork(OrderErrorItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginSPQHErrorPriceCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteSPQHErrorThreadWork", ex);
                BeginSPQHErrorPriceCancelOrderReckoning2(this, new QueueItemHandleEventArg<OrderErrorItem>(item));
            }
        }

        #endregion

        #endregion

        #region == 接收撤单回报 ==

        #region 接收现货撤单回报

        /// <summary>
        /// 现货撤单异步回报
        /// </summary>
        /// <param name="model"></param>
        public void AcceptCancelXHOrderRpt(CancelOrderEntity model)
        {
            CancelOrderRptItem item = new CancelOrderRptItem(model,
                                                             Types.BreedClassTypeEnum.
                                                                 Stock);

            DispatchXHCancelOrder(item);

            //this._CancelOrderProcessHolder.InsertQueueItem(item);

            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message +
                             "CancelSuccessed : " + model.IsSuccess;

            LogHelper.WriteDebug("<------现货撤单异步回报ReckonCenter.AcceptCancelXHOrderRpt[" + message + "]");
            this.FireCancelOrderCallbackEvent(message);
        }

        private void DispatchXHCancelOrder(CancelOrderRptItem item)
        {
            if (item == null)
                return;

            ExecuteXHCancelThreadWork(item);
            return;
        }

        private void ExecuteXHCancelThreadWork(CancelOrderRptItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginXHCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteXHCancelThreadWork", ex);
                BeginXHCancelOrderReckoning2(this,
                                             new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
        }

        #endregion

        #region 接收港股撤单回报

        /// <summary>
        /// 港股撤单异步回报
        /// </summary>
        /// <param name="model"></param>
        public void AcceptCancelHKOrderRpt(CancelOrderEntity model)
        {
            CancelOrderRptItem item = new CancelOrderRptItem(model, Types.BreedClassTypeEnum.HKStock);

            DispatchHKCancelOrder(item);

            //this._CancelOrderProcessHolder.InsertQueueItem(item);

            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message +
                             "CancelSuccessed : " + model.IsSuccess;

            LogHelper.WriteDebug("<------港股撤单异步回报ReckonCenter.AcceptCancelHKOrderRpt[" + message + "]");
            this.FireCancelOrderCallbackEvent(message);
        }

        private void DispatchHKCancelOrder(CancelOrderRptItem item)
        {
            if (item == null)
                return;

            ExecuteHKCancelThreadWork(item);
            return;
        }

        private void ExecuteHKCancelThreadWork(CancelOrderRptItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginHKCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteHKCancelThreadWork", ex);
                BeginHKCancelOrderReckoning2(this,
                                             new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
        }

        #endregion

        #region 接收商品期货撤单回报

        /// <summary>
        /// 商品期货撤单异步回报
        /// </summary>
        /// <param name="model"></param>
        public void AcceptCancelSPQHOrderRpt(CancelOrderEntity model)
        {
            CancelOrderRptItem item = new CancelOrderRptItem(model,
                                                             Types.BreedClassTypeEnum.
                                                                 CommodityFuture);

            DispatchSPQHCancelOrder(item);
            //this._SPQHCancelOrderProcessHolder.InsertQueueItem(item);
            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message +
                             "CancelSuccessed : " + model.IsSuccess;

            LogHelper.WriteDebug("<------商品期货撤单异步回报ReckonCenter.AcceptCancelSPQHOrderRpt[" + message + "]");
            this.FireCancelOrderCallbackEvent(message);
        }

        private void DispatchSPQHCancelOrder(CancelOrderRptItem item)
        {
            if (item == null)
                return;

            ExecuteSPQHCancelThreadWork(item);
            return;
        }

        private void ExecuteSPQHCancelThreadWork(CancelOrderRptItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginSPQHCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteSPQHCancelThreadWork", ex);
                BeginSPQHCancelOrderReckoning2(this,
                                               new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
        }

        #endregion

        #region 接收股指期货撤单回报

        /// <summary>
        /// 股指期货撤单异步回报
        /// </summary>
        /// <param name="model"></param>
        public void AcceptCancelGZQHOrderRpt(CancelOrderEntity model)
        {
            CancelOrderRptItem item = new CancelOrderRptItem(model,
                                                             Types.BreedClassTypeEnum.
                                                                 StockIndexFuture);
            DispatchGZQHCancelOrder(item);
            //this._GZQHCancelOrderProcessHolder.InsertQueueItem(item);
            string message = "OrderID:" + model.OrderNo + "-- Message:" + model.Message +
                             "CancelSuccessed : " + model.IsSuccess;

            LogHelper.WriteDebug("<------股指期货撤单异步回报ReckonCenter.AcceptCancelGZQHOrderRpt[" + message + "]");
            this.FireCancelOrderCallbackEvent(message);
        }

        private void DispatchGZQHCancelOrder(CancelOrderRptItem item)
        {
            if (item == null)
                return;

            ExecuteGZQHCancelThreadWork(item);
            return;
        }

        private void ExecuteGZQHCancelThreadWork(CancelOrderRptItem item)
        {
            try
            {
                smartPool.QueueWorkItem(BeginGZQHCancelOrderReckoning2, this,
                                        new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("ReckonCenter.ExecuteGZQHCancelThreadWork", ex);
                BeginGZQHCancelOrderReckoning2(this,
                                               new QueueItemHandleEventArg<CancelOrderRptItem>(item));
            }
        }

        #endregion

        #endregion

        #endregion

        #region 回报消息事件（用于UI显示）

        /// <summary>
        /// 成交回报消息事件
        /// </summary>
        /// <param name="strMessage"></param>
        private void FireDealRptCallbackEvent(string strMessage)
        {
            try
            {
                if (DealRptCallbackEvent != null)
                    DealRptCallbackEvent(this, new RuntimeMessageEventArge(strMessage));
            }
            catch (Exception ex)
            {
                //TODO:成交回报消息事件异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 委托回报消息事件
        /// </summary>
        /// <param name="strMessage"></param>
        private void FireOrderRptCallbackEvent(string strMessage)
        {
            try
            {
                if (OrderRptCallbackEvent != null)
                    OrderRptCallbackEvent(this, new RuntimeMessageEventArge(strMessage));
            }
            catch (Exception ex)
            {
                //TODO:委托回报消息事件异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 撤单回报消息事件
        /// </summary>
        /// <param name="strMessage"></param>
        private void FireCancelOrderCallbackEvent(string strMessage)
        {
            try
            {
                if (CancelOrderCallbackEvent != null)
                    CancelOrderCallbackEvent(this, new RuntimeMessageEventArge(strMessage));
            }
            catch (Exception ex)
            {
                //TODO:撤单回报消息事件异常
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        #endregion
    }
}