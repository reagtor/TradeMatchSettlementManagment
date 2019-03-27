#region Using Namespace

using System;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateAcceptOffer.OrderAccepter.Logic;
using ReckoningCounter.BLL.delegateoffer;
using ReckoningCounter.BLL.DelegateValidate;
using ReckoningCounter.BLL.DelegateValidate.Local;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.Reckoning.Instantaneous;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.DelegateOffer;
using ReckoningCounter.Model;
using ReckoningCounter.BLL.Reckoning.Logic;
using ReckoningCounter.Entity.Model.HK;

#endregion

namespace ReckoningCounter.BLL
{
    /// <summary>
    /// 委托接收流程控制及业务处理类，错误编码2100-2139
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// 作者：李健华
    /// 日期：2010-01-25
    /// Desc：添加商品期货
    /// </summary>
    public class OrderAccepter
    {
        #region == 字段/属性 ==

        /// <summary>
        /// 报盘中心实例
        /// </summary>
        private OrderOfferCenter _orderOfferCenter;

        private SmartThreadPool threadPool = new SmartThreadPool { MaxThreads = 100, MinThreads = 25 };

        /// <summary>
        /// 成交回报事件
        /// </summary>
        public event EventHandler<RuntimeMessageEventArge> ReceiveOrderEvent = null;

        #endregion

        #region == 构造器 ==

        /// <summary>
        /// 构造器
        /// </summary>
        public OrderAccepter()
        {
            _orderOfferCenter = OrderOfferCenter.Instance;
        }

        #endregion

        #region == 方法 ==

        #region 事件触发方法

        private void FireReceiveOrderEvent(StockOrderRequest stockOrder)
        {
            //if (ReceiveOrderEvent != null)
            //{
            //    ReceiveOrderEvent(this, new RuntimeMessageEventArge("stockorder"));
            //}

            threadPool.QueueWorkItem(ASyncFireEvent, new RuntimeMessageEventArge("stockorder"));
        }

        private void FireReceiveOrderEvent(StockIndexFuturesOrderRequest stockOrder)
        {
            //if (ReceiveOrderEvent != null)
            //{
            //    ReceiveOrderEvent(this, new RuntimeMessageEventArge("stockindexorder"));
            //}

            threadPool.QueueWorkItem(ASyncFireEvent, new RuntimeMessageEventArge("stockindexorder"));
        }

        /// <summary>
        /// 商品期货委托接收信息显示事件方法（用于显示到窗体）
        /// </summary>
        /// <param name="futuresorder"></param>
        private void FireReceiveOrderEvent(MercantileFuturesOrderRequest futuresorder)
        {
            //if (ReceiveOrderEvent != null)
            //{
            //    ReceiveOrderEvent(this, new RuntimeMessageEventArge("futuresorder"));
            //}

            threadPool.QueueWorkItem(ASyncFireEvent, new RuntimeMessageEventArge("SPQHOrder"));
        }

        private void FireReceiveOrderEvent(HKOrderRequest hkOrder)
        {
            threadPool.QueueWorkItem(ASyncFireEvent, new RuntimeMessageEventArge("hkorder"));
        }

        private void ASyncFireEvent(RuntimeMessageEventArge args)
        {
            if (ReceiveOrderEvent != null)
            {
                ReceiveOrderEvent(this, args);
            }
        }

        #endregion

        #region 现货
        /// <summary>
        /// 将单位转换为撮合单位，并返回比例
        /// </summary>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <param name="code">商品代码</param>
        /// <param name="oriUnitType">原始单位</param>
        /// <param name="scale">比例</param>
        /// <param name="matchUnitType">撮合单位</param>
        /// <returns></returns>
        public static bool ConvertUnitType(Types.BreedClassTypeEnum type, string code, Types.UnitType oriUnitType, out decimal scale, out Types.UnitType matchUnitType)
        {
            bool result = false;
            try
            {
                //获取撮合单位（行情单位）
                matchUnitType = MCService.GetMatchUnitType(code, type);

                if (matchUnitType == oriUnitType)
                {
                    scale = 1;
                    return true;
                }
                if (type == Types.BreedClassTypeEnum.HKStock)
                {
                    scale = MCService.CommonPara.GetHKUnitConversionByDetailUnits(code, oriUnitType, matchUnitType);
                }
                else
                {
                    var breedClass = MCService.CommonPara.GetBreedClassIdByCommodityCode(code, type);
                    scale = MCService.CommonPara.GetUnitConversionByDetailUnits(breedClass.Value, (int)oriUnitType, (int)matchUnitType);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);

                scale = 1;
                matchUnitType = oriUnitType;
            }

            return result;
        }

        /// <summary>
        /// 现货委托处理
        /// </summary>
        /// <param name="stockorder">原始委托对象</param>
        /// <returns></returns>
        public OrderResponse DoStockOrder(StockOrderRequest stockorder)
        {
            LogHelper.WriteDebug("---->接受现货委托OrderAccepter.DoStockOrder");

            var orResult = new OrderResponse();
            var xhTodayEntrustTable = new XH_TodayEntrustTableInfo();
            string strErrorMessage = string.Empty;
            //是否通过验证，可以报盘
            bool canOfferOrder = false;

            var code = stockorder.Code;

            #region 判断代码是否存在

            var commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null)
            {
                strErrorMessage = @"GT-2108:[现货委托]不存在对应代码的商品，Code=" + code;
                goto EndProcess2;
            }

            #endregion

            #region 判断委托价格不为0，不为0后面还会有最小变动价位判断
            // 因为价格为0时对后面的清算获取冻结的资金都为0所以不作清算，所以撮合回报价格不附合的错误价格回报
            //时清算就出问题，而在撤单会出现撤单不存在，并且因为冻结资金为0，而在盘后清算时做撤单再算，冻结资金为
            //0所以也不清算清算成功单一直以已报 
            if (stockorder.OrderPrice == 0 && stockorder.OrderWay == Entity.Contants.Types.OrderPriceType.OPTLimited)
            {
                strErrorMessage = @"GT-2109:[现货委托]委托价格不能为：0";
                goto EndProcess2;
            }

            #endregion
            #region 原始单位及成交量转换

            //var code = stockorder.Code;
            var oriUnitType = stockorder.OrderUnitType;
            decimal scale;
            Types.UnitType matchUnitType;

            bool canConvert = ConvertUnitType(Types.BreedClassTypeEnum.Stock, code, oriUnitType, out scale, out matchUnitType);
            if (!canConvert)
            {
                strErrorMessage = @"GT-2107:[现货委托]无法进行行情单位转换.";
                goto EndProcess2;
            }

            stockorder.OrderUnitType = matchUnitType;
            stockorder.OrderAmount = stockorder.OrderAmount * (float)scale;

            #endregion

            FireReceiveOrderEvent(stockorder);

            if (stockorder == null)
            {
                strErrorMessage = @"GT-2100:[现货委托]无委托对象信息.";
            }
            else if (stockorder.OrderWay == Entity.Contants.Types.OrderPriceType.OPTMarketPrice &&
                     !ValidateCenter.IsMatchTradingTime(stockorder.Code))
            {
                strErrorMessage = @"GT-2106:[现货委托]市价单不接受预下单.";
            }
            else
            {
                LogHelper.WriteDebug(stockorder.ToString());
                XHAcceptLogic xhAcceptLogic = new XHAcceptLogic();
                //OrderLogicFlowBase<StockOrderRequest, XH_TodayEntrustTableInfo, StockDealBackEntity> _XHDataLogicProcessor =
                //    DataLogicProcessorFactory.GetXHDataLogicProcessor(stockorder.BuySell);
                try
                {
                    #region  add by 董鹏 2010-04-06
                    if (!string.IsNullOrEmpty(stockorder.TraderId))
                    {
                        stockorder.TraderId = stockorder.TraderId.Trim();
                    }
                    if (!string.IsNullOrEmpty(stockorder.FundAccountId))
                    {
                        stockorder.FundAccountId = stockorder.FundAccountId.Trim();
                    }
                    #endregion

                    if ((string.IsNullOrEmpty(stockorder.TraderId) && string.IsNullOrEmpty(stockorder.FundAccountId)))
                    {
                        strErrorMessage = @"GT-2101:[现货委托]交易员ID或资金帐户无效.";
                        LogHelper.WriteInfo(strErrorMessage + "," + stockorder);
                        goto EndProcess;
                    }

                    //柜台委托时间判断
                    if (!ValidateCenter.IsCountTradingTime(stockorder.Code, ref strErrorMessage))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoStockOrder" + strErrorMessage);
                        string oriMsg = "当前时间不接受委托";
                        if (strErrorMessage.Length > 0)
                            oriMsg = strErrorMessage;

                        strErrorMessage = @"GT-2102:[现货委托]" + oriMsg;
                        LogHelper.WriteInfo(strErrorMessage + "," + stockorder);
                        goto EndProcess;
                    }

                    if (string.IsNullOrEmpty(stockorder.TraderId))
                    {
                        //stockorder.TraderId =
                        //    CounterCache.Instance.GetTraderIdByFundAccount(stockorder.FundAccountId);

                        var user = AccountManager.Instance.GetUserByAccount(stockorder.FundAccountId);
                        if (user != null)
                            stockorder.TraderId = user.UserID;
                    }

                    // 交易规则判断
                    if (!ValidateCenter.Validate(stockorder, ref strErrorMessage))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoStockOrder" + strErrorMessage);
                        LogHelper.WriteInfo("[现货委托]交易规则判断失败，" + stockorder);
                        goto EndProcess;
                    }

                    //资金,持仓判断
                    //if (!_XHDataLogicProcessor.PersistentOrder(stockorder, ref strErrorMessage,
                    //                                           ref xhTodayEntrustTable))
                    //{
                    //    LogHelper.WriteInfo("OrderAccepter.DoStockOrder" + strErrorMessage);
                    //    goto EndProcess;
                    //}

                    if (!xhAcceptLogic.PersistentOrder(stockorder, ref xhTodayEntrustTable, ref strErrorMessage))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoStockOrder" + strErrorMessage);
                        goto EndProcess;
                    }

                    canOfferOrder = true;
                    //报盘
                    XhTodayEntrustTableEx order = new XhTodayEntrustTableEx(xhTodayEntrustTable);
                    order.TraderId = stockorder.TraderId;

                    _orderOfferCenter.OfferStockOrder(order);

                EndProcess:
                    //结束
                    ;
                }
                catch (Exception ex)
                {
                    strErrorMessage = @"GT-2103:[现货委托]接收处理异常，请查看日志.";
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

        EndProcess2:
            //结束
            ;
            orResult.OrderMessage = strErrorMessage;

            //只有报盘才分配一个柜台的委托单号
            if (canOfferOrder)
            {
                orResult.OrderId = xhTodayEntrustTable.EntrustNumber;
                orResult.IsSuccess = true;
            }
            else
            {
                int type = 0;
                bool canGetErrorCode = Utils.GetErrorCode(strErrorMessage, out type);
                if (canGetErrorCode)
                    orResult.ErrorType = type;
            }

            return orResult;
        }

        public bool CancelStockOrder(string orderId, ref string message, out Entity.Contants.Types.OrderStateType ost, out int errorType)
        {
            errorType = 0;
            LogHelper.WriteDebug("---->现货撤单OrderAccepter.CancelStockOrder，OrderID=" + orderId);
            var result = false;
            message = string.Empty;
            ost = Entity.Contants.Types.OrderStateType.None;
            //var xhOrder = DataRepository.XhTodayEntrustTableProvider.GetByEntrustNumber(orderId);

            var tet = XHDataAccess.GetTodayEntrustTable(orderId);
            if (tet == null)
            {
                message = "GT-2104:[现货撤单委托]对应委托单不存在";
                return false;
            }

            ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);

            //var buySellFlag = (CommonObject.Types.TransactionDirection) xhOrder.BuySellTypeId;

            //var _XHDataLogicProcessor = DataLogicProcessorFactory.GetXHDataLogicProcessor(buySellFlag);
            XHAcceptLogic xhAcceptLogic = new XHAcceptLogic();

            //柜台委托时间判断
            if (!ValidateCenter.IsCountTradingTime(tet.SpotCode, ref message))
            {
               
                string oriMsg = @"GT-2102:[现货撤单委托]当前时间不接受委托.";
                if (message.Length > 0)
                {
                    oriMsg = message;
                }
                oriMsg = @"GT-2102:[现货撤单委托]" + oriMsg;
                LogHelper.WriteInfo("OrderAccepter.CancelStockOrder" + message);
                return false;
            }

            //委托单是否存在,委托单状态是否允许撤单判断
            if (!xhAcceptLogic.CancelOrderValidate(orderId, out tet, ref message))
            {
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
                return false;
            }

            //20090326：只有接受委托时间内、撮合时间外时才能进行内部撤单，其他时间统一走外部撤单
            //20091215：李健华===当在临界开市时间时可能存在还有未报的单，这样就会走外部撤单就会产生报到撮合后
            //找不到委托单，因为此单还在内部缓存单中
            if (ValidateCenter.IsMatchTradingTime(tet.SpotCode)
                 && ost != Entity.Contants.Types.OrderStateType.DOSUnRequired)
            {
                //撮合时间内，走外部撤单
                //报盘
                result = _orderOfferCenter.CancelStockOrder(tet, ref message, out ost);
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
            }
            else
            {
                //非撮合时间内，走内部撤单
                try
                {
                    //内部撤单也要回推结果到前台
                    //(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
                    xhAcceptLogic.EndCancelEvent += XhAcceptLogic_EndCancelEvent;
                    result = xhAcceptLogic.InternalCancelOrder(tet, "前台主动撤单");

                    if (result)
                        ost = Entity.Contants.Types.OrderStateType.DOSRemoved;
                    else
                    {
                        LogHelper.WriteInfo("OrderAccepter.CancelStockOrder撤单失败！");
                        message = "GT-2105:[现货撤单委托]撤单失败,请联系系统管理员。";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    LogHelper.WriteError(ex.Message, ex);
                    message = "GT-2103:[现货撤单委托]未报状态委托对象更新异常," + ex.Message; //TODO:
                }

                //预委托缓存中的也要删除
                OrderOfferCenter.Instance._orderCache.DeleteXHOrder(tet.SpotCode, orderId);
            }

            #region 旧撤单逻辑

            //if (xhOrder.OrderStatusId == (int) Types.OrderStateType.DOSUnRequired)
            //{
            //    try
            //    {
            //        result = _XHDataLogicProcessor.InternalCancelOrder(xhOrder, string.Empty);
            //        ost = Types.OrderStateType.DOSRemoved;
            //    }
            //    catch (Exception ex)
            //    {
            //        result = false;
            //        LogHelper.WriteError(ex.Message, ex);
            //        message = "GT-2103:[现货撤单委托]未报状态委托对象更新异常," + ex.Message; //TODO:
            //    }
            //}
            //else
            //{
            //    //报盘
            //    result = _orderOfferCenter.CancelStockOrder(xhOrder, ref message, out ost);
            //    ost = Types.GetOrderStateType(xhOrder.OrderStatusId.Value);
            //}

            #endregion

            if (!result)
            {
                int type = 0;
                bool canGetErrorCode = Utils.GetErrorCode(message, out type);
                if (canGetErrorCode)
                    errorType = type;
            }
            return result;
        }

        void XhAcceptLogic_EndCancelEvent(ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> obj)
        {
            CounterOrderService.Instance.AcceptStockDealOrder(obj);
        }
        #endregion

        #region 港股

        /// <summary>
        /// 港股下单请求
        /// </summary>
        /// <param name="hkOrder">原始委托对象</param>
        /// <returns></returns>
        public OrderResponse DoHKOrder(HKOrderRequest hkOrder)
        {
            LogHelper.WriteDebug("---->接受港股委托OrderAccepter.DoHKOrder");

            OrderResponse orResult = new OrderResponse();
            HK_TodayEntrustInfo todayEntrustInfo = new HK_TodayEntrustInfo();

            string strErrorMessage = string.Empty;
            bool canOfferOrder = false;  //是否通过验证，可以报盘

            var code = hkOrder.Code;

            #region 判断代码是否存在

            var commodity = MCService.HKTradeRulesProxy.GetHKCommodityByCommodityCode(code);
            if (commodity == null)
            {
                strErrorMessage = @"GT-2140:[港股委托]不存在对应代码的商品，Code=" + code;
                goto EndProcess2;
            }

            #endregion


            #region 判断委托价格不为0，不为0后面还会有最小变动价位判断
            // 因为价格为0时对后面的清算获取冻结的资金都为0所以不作清算，所以撮合回报价格不附合的错误价格回报
            //时清算就出问题，而在撤单会出现撤单不存在，并且因为冻结资金为0，而在盘后清算时做撤单再算，冻结资金为
            //0所以也不清算清算成功单一直以已报 
            if (hkOrder.OrderPrice == 0)
            {
                strErrorMessage = @"GT-2109:[港股委托]委托价格不能为：0";
                goto EndProcess2;
            }

            #endregion

            #region 原始单位及成交量转换

            //var code = hkOrder.Code;
            var oriUnitType = hkOrder.OrderUnitType;
            decimal scale;
            Types.UnitType matchUnitType;

            bool canConvert = ConvertUnitType(Types.BreedClassTypeEnum.HKStock, code, oriUnitType, out scale, out matchUnitType);
            if (!canConvert)
            {
                strErrorMessage = @"GT-2139:[港股委托]无法进行行情单位转换.";
                goto EndProcess2;
            }

            hkOrder.OrderUnitType = matchUnitType;
            hkOrder.OrderAmount = hkOrder.OrderAmount * (float)scale;

            #endregion

            FireReceiveOrderEvent(hkOrder);

            if (hkOrder == null)
            {
                strErrorMessage = @"GT-2138:[港股委托]无委托对象信息.";
            }
            //else if (!ValidateCenter.IsMatchTradingTime(hkOrder.Code))
            //{
            //    strErrorMessage = @"GT-2106:[港股委托]当前时间不接受预下单.";
            //}
            else
            {
                LogHelper.WriteDebug(hkOrder.ToString());
                HKAcceptLogic hkAcceptLogic = new HKAcceptLogic();
                try
                {
                    #region 基本信息字段完整性检验

                    #region  add by 董鹏 2010-04-06
                    if (!string.IsNullOrEmpty(hkOrder.TraderId))
                    {
                        hkOrder.TraderId = hkOrder.TraderId.Trim();
                    }
                    if (!string.IsNullOrEmpty(hkOrder.FundAccountId))
                    {
                        hkOrder.FundAccountId = hkOrder.FundAccountId.Trim();
                    }
                    #endregion

                    if ((string.IsNullOrEmpty(hkOrder.TraderId) && string.IsNullOrEmpty(hkOrder.FundAccountId)))
                    {
                        strErrorMessage = @"GT-2137:[港股委托]交易员ID或资金帐户无效.";
                        LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);
                        goto EndProcess;
                    }
                    #endregion

                    #region 柜台委托时间判断
                    //柜台委托时间判断
                    if (!ValidateCenter.IsCountTradingTime(Types.BreedClassTypeEnum.HKStock, hkOrder.Code, ref strErrorMessage))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoHKOrder" + strErrorMessage);
                        string oriMsg = "当前时间不接受委托";
                        if (strErrorMessage.Length > 0)
                        {
                            oriMsg = strErrorMessage;
                        }
                        strErrorMessage = @"GT-2136:[港股委托]" + oriMsg;
                        LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);
                        goto EndProcess;
                    }
                    #endregion

                    #region 如果交易员ID为null根据资金账户获取交易员ID，这间接验证资金账号是否为空等
                    //如果交易员ID为null根据资金账户获取交易员ID，这间接验证资金账号是否为空等
                    if (string.IsNullOrEmpty(hkOrder.TraderId))
                    {
                        var user = AccountManager.Instance.GetUserByAccount(hkOrder.FundAccountId);
                        if (user != null)
                        {
                            hkOrder.TraderId = user.UserID;
                        }
                    }
                    #endregion

                    #region 交易规则判断
                    // 交易规则判断
                    if (!ValidateCenter.Validate(hkOrder, ref strErrorMessage))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoHKOrder" + strErrorMessage);
                        LogHelper.WriteInfo("[港股委托]交易规则判断失败，" + hkOrder);
                        goto EndProcess;
                    }
                    #endregion

                    if (!hkAcceptLogic.PersistentOrder(hkOrder, ref todayEntrustInfo, ref strErrorMessage))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoHKOrder" + strErrorMessage);
                        goto EndProcess;
                    }
                    canOfferOrder = true;
                    HkTodayEntrustEx order = new HkTodayEntrustEx(todayEntrustInfo);
                    order.TraderId = hkOrder.TraderId;
                    //报盘
                    _orderOfferCenter.OfferHKOrder(order);
                EndProcess:  //结束
                    ;
                }
                catch (Exception ex)
                {
                    strErrorMessage = @"GT-2135:[港股委托]接收处理异常，请查看日志.";
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

        EndProcess2:
            //结束
            ;
            orResult.OrderMessage = strErrorMessage;

            //只有报盘才分配一个柜台的委托单号
            if (canOfferOrder)
            {
                orResult.OrderId = todayEntrustInfo.EntrustNumber;
                orResult.IsSuccess = true;
            }
            else
            {
                int type = 0;
                bool canGetErrorCode = Utils.GetErrorCode(strErrorMessage, out type);
                if (canGetErrorCode)
                    orResult.ErrorType = type;
            }

            return orResult;
        }

        /// <summary>
        /// 港股撤单操作
        /// </summary>
        /// <param name="orderID">委托单号</param>
        /// <param name="message">撤单返回信息</param>
        /// <param name="ost">返回委托单状态标识</param>
        /// <param name="errorType">撤单异常类型</param>
        /// <returns></returns>
        public bool CancelHKOrder(string orderID, ref string message, out Entity.Contants.Types.OrderStateType ost, out int errorType)
        {
            errorType = 0;
            LogHelper.WriteDebug("---->港股撤单OrderAccepter.CancelHKOrder，OrderID=" + orderID);
            bool result = false;
            message = string.Empty;
            ost = Entity.Contants.Types.OrderStateType.None;

            HK_TodayEntrustInfo tet = HKDataAccess.GetTodayEntrustTable(orderID);
            if (tet == null)
            {
                message = "GT-2134:[港股撤单委托]对应委托单不存在";
                return false;
            }

            ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusID);
            HKAcceptLogic hkAcceptLogic = new HKAcceptLogic();

            //柜台委托时间判断
            if (!ValidateCenter.IsCountTradingTime(Types.BreedClassTypeEnum.HKStock, tet.Code, ref message))
            {
                LogHelper.WriteInfo("OrderAccepter.CancelHKOrder" + message);
                message = @"GT-2133:[港股委托]当前时间不接受委托.";
                return false;
            }

            //委托单是否存在,委托单状态是否允许撤单判断
            if (!hkAcceptLogic.CancelOrderValidate(orderID, out tet, ref message))
            {
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusID);
                return false;
            }

            //20090326：只有接受委托时间内、撮合时间外时才能进行内部撤单，其他时间统一走外部撤单
            //20091215：李健华===当在临界开市时间时可能存在还有未报的单，这样就会走外部撤单就会产生报到撮合后
            //找不到委托单，因为此单还在内部缓存单中
            if (ValidateCenter.IsMatchTradingTime(Types.BreedClassTypeEnum.HKStock, tet.Code)
                && ost != Entity.Contants.Types.OrderStateType.DOSUnRequired)
            {
                //撮合时间内，走外部撤单
                //报盘
                result = _orderOfferCenter.CancelHKOrder(tet, ref message, out ost);
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusID);
            }
            else
            {
                //非撮合时间内，走内部撤单
                try
                {
                    ////内部撤单也要回推结果到前台
                    ////(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
                    //hkAcceptLogic.EndCancelEvent += HKAcceptLogic_EndCancelEvent;
                    result = hkAcceptLogic.InternalCancelOrder(tet, "前台主动撤单");

                    if (result)
                    {
                        ost = Entity.Contants.Types.OrderStateType.DOSRemoved;
                    }
                    else
                    {
                        LogHelper.WriteInfo("OrderAccepter.CancelHKOrder撤单失败！");
                        message = "GT-2132:[港股撤单委托]撤单失败,请联系系统管理员。";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    LogHelper.WriteError(ex.Message, ex);
                    message = "GT-2131:[港股撤单委托]未报状态委托对象更新异常," + ex.Message; //TODO:
                }

                //预委托缓存中的也要删除
                OrderOfferCenter.Instance._orderCache.DeleteHKOrder(tet.Code, orderID);
            }


            if (!result)
            {
                int type = 0;
                bool canGetErrorCode = Utils.GetErrorCode(message, out type);
                if (canGetErrorCode)
                {
                    errorType = type;
                }
            }
            return result;
        }

        /// <summary>
        /// 港股改单请求
        /// </summary>
        /// <param name="hkOrder"></param>
        /// <returns></returns>
        public OrderResponse ModifyHKOrder(HKModifyOrderRequest hkOrder)
        {
            //string strErrorMessage = "";

            return ModifyOrderProcessor.Instance.Process(hkOrder);
        }
        ///// <summary>
        ///// 港股接收委托
        ///// </summary>
        ///// <param name="obj"></param>
        //void HKAcceptLogic_EndCancelEvent(ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> obj)
        //{
        //     CounterOrderService.Instance.AcceptHKDealOrder(obj);
        //}
        #endregion

        #region 期货（商品、股指期货）
        /// <summary>
        /// 商品期货委托业务处理
        /// </summary>
        /// <param name="futuresorder">商品期货下单实体</param>
        /// <returns></returns>
        public OrderResponse DoMercantileFuturesOrder(MercantileFuturesOrderRequest futuresorder)
        {
            LogHelper.WriteDebug("---->接受商品期货委托OrderAccepter.DoMercantileFuturesOrder");
            FireReceiveOrderEvent(futuresorder);

            var orResult = new OrderResponse();
            var CounterOrderObject = new QH_TodayEntrustTableInfo();
            string strErrorMessage = string.Empty;

            //是否通过验证，可以报盘
            bool canOfferOrder = false;

            var code = futuresorder.Code;

            #region 判断代码是否存在

            var commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null)
            {
                strErrorMessage = @"GT-2118:[商品期货委托]不存在对应代码的商品，Code=" + code;
                goto EndProcess2;
            }
            #endregion

            #region 判断委托价格不为0，不为0后面还会有最小变动价位判断
            // 因为价格为0时对后面的清算获取冻结的资金都为0所以不作清算，所以撮合回报价格不附合的错误价格回报
            //时清算就出问题，而在撤单会出现撤单不存在，并且因为冻结资金为0，而在盘后清算时做撤单再算，冻结资金为
            //0所以也不清算清算成功单一直以已报 
            if (futuresorder.OrderPrice == 0 && futuresorder.OrderWay == Entity.Contants.Types.OrderPriceType.OPTLimited)
            {
                strErrorMessage = @"GT-2109:[商品委托]委托价格不能为：0";
                goto EndProcess2;
            }

            #endregion

            #region 原始单位及成交量转换

            //var code = futuresorder.Code;
            var oriUnitType = futuresorder.OrderUnitType;
            decimal scale;
            Types.UnitType matchUnitType;

            bool canConvert = ConvertUnitType(Types.BreedClassTypeEnum.Stock, code, oriUnitType, out scale, out matchUnitType);
            if (!canConvert)
            {
                strErrorMessage = @"GT-2117:[商品期货委托]无法进行撮合单位转换.";
                goto EndProcess2;
            }

            futuresorder.OrderUnitType = matchUnitType;
            futuresorder.OrderAmount = futuresorder.OrderAmount * (float)scale;

            #endregion

            FireReceiveOrderEvent(futuresorder);

            if (futuresorder == null)
            {
                strErrorMessage = @"GT-2120:[商品期货委托]无委托对象信息.";
            }
            else if (futuresorder.OrderWay == Entity.Contants.Types.OrderPriceType.OPTMarketPrice &&
                     !ValidateCenter.IsMatchTradingTime(futuresorder.Code))
            {
                strErrorMessage = @"GT-2126:[商品期货委托]市价单不接受预下单.";
            }
            else
            {

                LogHelper.WriteDebug(futuresorder.ToString());
                SPQHAcceptLogic spqhAcceptLogic = new SPQHAcceptLogic();

                #region 判断是否为开盘检查强制平仓
                ////是否是强制平仓的过期合约生成的委托
                //bool isExpiredContract = false;

                //是否是期货开盘检查中的盘前检查强行平仓
                bool isOpenMarketCheckOrder = false;
                //强行平仓类型（当是盘前检查强行平仓时才有效）
                Types.QHForcedCloseType qhforceCloseType = new Types.QHForcedCloseType();

                //bool isFutureDayCheckContract = false;
                if (futuresorder is MercantileFuturesOrderRequest2)
                {
                    // isExpiredContract = ((MercantileFuturesOrderRequest2)futuresorder).IsExpiredContract;
                    isOpenMarketCheckOrder = ((MercantileFuturesOrderRequest2)futuresorder).IsForcedCloseOrder;
                    qhforceCloseType = ((MercantileFuturesOrderRequest2)futuresorder).QHForcedCloseType;
                    //isFutureDayCheckContract = true;
                }
                #endregion
                try
                {
                    //不是开盘检查的强制平仓委托才进行检查
                    // if (!isFutureDayCheckContract)
                    if (!isOpenMarketCheckOrder)
                    {
                        #region  add by 董鹏 2010-04-06
                        if (!string.IsNullOrEmpty(futuresorder.TraderId))
                        {
                            futuresorder.TraderId = futuresorder.TraderId.Trim();
                        }
                        if (!string.IsNullOrEmpty(futuresorder.FundAccountId))
                        {
                            futuresorder.FundAccountId = futuresorder.FundAccountId.Trim();
                        }
                        #endregion

                        #region 资金账号交易员ID数据检验
                        if ((string.IsNullOrEmpty(futuresorder.TraderId) && string.IsNullOrEmpty(futuresorder.FundAccountId)))
                        {
                            strErrorMessage = @"GT-2121:[商品期货委托]交易员ID或资金帐户无效.";
                            LogHelper.WriteInfo(strErrorMessage + "," + futuresorder);
                            goto EndProcess;
                        }
                        #endregion

                        #region 柜台委托时间判断
                        if (!ValidateCenter.IsCountTradingTime(futuresorder.Code, ref strErrorMessage))
                        {
                            LogHelper.WriteInfo("OrderAccepter.DoMercantileFuturesOrder" + strErrorMessage);
                            string oriMsg   = @"GT-2122:[商品期货委托]当前时间不接受委托.";
                            if (strErrorMessage.Length > 0)
                            {
                                oriMsg = strErrorMessage;
                            }
                            strErrorMessage = @"GT-2136:[商品期货委托]" + oriMsg;
                            LogHelper.WriteInfo(strErrorMessage + "," + futuresorder);

                            goto EndProcess;
                        }
                        #endregion

                        #region 交易规则判断
                        if (!ValidateCenter.Validate(futuresorder, ref strErrorMessage))
                        {
                            LogHelper.WriteInfo("OrderAccepter.DoMercantileFuturesOrder" + strErrorMessage);
                            LogHelper.WriteInfo("[商品期货委托]交易规则判断失败，" + futuresorder);
                            goto EndProcess;
                        }
                        #endregion

                    }

                    #region 资金,持仓判断
                    if (!spqhAcceptLogic.PersistentOrder(futuresorder, ref CounterOrderObject, ref strErrorMessage))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoMercantileFuturesOrder" + strErrorMessage);
                        goto EndProcess;
                    }
                    #endregion

                    #region 报盘
                    canOfferOrder = true;
                    QhTodayEntrustTableEx tableEx = new QhTodayEntrustTableEx(CounterOrderObject);
                    tableEx.TraderId = futuresorder.TraderId;

                    // if (isExpiredContract)
                    if (isOpenMarketCheckOrder)
                    {
                        tableEx.IsOpenMarketCheckOrder = isOpenMarketCheckOrder;
                        tableEx.QHForcedCloseType = qhforceCloseType;
                        _orderOfferCenter.OfferCloseHoldSPQHOrder(tableEx);
                    }
                    else
                    {
                        var ex = tableEx;
                        ex.IsOpenMarketCheckOrder = isOpenMarketCheckOrder;
                        _orderOfferCenter.OfferSPQHOrder(ex);
                    }

                    #endregion

                EndProcess:
                    //结束
                    ;
                }
                catch (Exception ex)
                {
                    strErrorMessage = @"GT-2123:[商品期货委托]接收处理异常，请查看日志.";
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        EndProcess2:
            //结束
            ;

            orResult.OrderMessage = strErrorMessage;

            //只有报盘才分配一个柜台的委托单号
            if (canOfferOrder)
            {
                orResult.OrderId = CounterOrderObject.EntrustNumber;
                orResult.IsSuccess = true;
            }

            return orResult;
        }

        /// <summary>
        /// 股指期货委托业务处理
        /// </summary>
        /// <param name="futuresorder"></param>
        /// <returns></returns>
        public OrderResponse DoStockIndexFuturesOrder(StockIndexFuturesOrderRequest futuresorder)
        {
            LogHelper.WriteDebug("---->接受股指期货委托OrderAccepter.DoStockIndexFuturesOrder");


            var orResult = new OrderResponse();
            var qhTodayEntrustTable = new QH_TodayEntrustTableInfo();
            string strErrorMessage = string.Empty;

            //是否通过验证，可以报盘
            bool canOfferOrder = false;

            var code = futuresorder.Code;

            #region 判断代码是否存在

            var commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null)
            {
                strErrorMessage = @"GT-2118:[股指期货委托]不存在对应代码的商品，Code=" + code;
                goto EndProcess2;
            }

            #endregion

            #region 判断委托价格不为0，不为0后面还会有最小变动价位判断
            // 因为价格为0时对后面的清算获取冻结的资金都为0所以不作清算，所以撮合回报价格不附合的错误价格回报
            //时清算就出问题，而在撤单会出现撤单不存在，并且因为冻结资金为0，而在盘后清算时做撤单再算，冻结资金为
            //0所以也不清算清算成功单一直以已报 
            if (futuresorder.OrderPrice == 0 && futuresorder.OrderWay == Entity.Contants.Types.OrderPriceType.OPTLimited)
            {
                strErrorMessage = @"GT-2109:[期货委托]委托价格不能为：0";
                goto EndProcess2;
            }

            #endregion

            #region 原始单位及成交量转换

            //var code = futuresorder.Code;
            var oriUnitType = futuresorder.OrderUnitType;
            decimal scale;
            Types.UnitType matchUnitType;

            bool canConvert = ConvertUnitType(Types.BreedClassTypeEnum.Stock, code, oriUnitType, out scale, out matchUnitType);
            if (!canConvert)
            {
                strErrorMessage = @"GT-2117:[股指期货委托]无法进行行情单位转换.";
                goto EndProcess2;
            }


            futuresorder.OrderUnitType = matchUnitType;
            futuresorder.OrderAmount = futuresorder.OrderAmount * (float)scale;

            #endregion

            FireReceiveOrderEvent(futuresorder);

            if (futuresorder == null)
            {
                strErrorMessage = @"GT-2110:[股指期货委托]无委托对象信息.";
            }
            else if (futuresorder.OrderWay == Entity.Contants.Types.OrderPriceType.OPTMarketPrice &&
                     !ValidateCenter.IsMatchTradingTime(futuresorder.Code))
            {
                strErrorMessage = @"GT-2116:[股指期货委托]市价单不接受预下单.";
            }
            else
            {
                LogHelper.WriteDebug(futuresorder.ToString());
                GZQHAcceptLogic gzqhAcceptLogic = new GZQHAcceptLogic();
                //OrderLogicFlowBase<StockIndexFuturesOrderRequest, QhTodayEntrustTable, FutureDealBackEntity>
                //    _GZQHDataLogicProcessor
                //        = DataLogicProcessorFactory.GetGZQHDataLogicProcessor(futuresorder.OpenCloseType);

                #region 判断是否为开盘检查强制平仓
                ////是否是强制平仓的过期合约生成的委托
                //bool isExpiredContract = false;


                //是否是期货开盘检查中的盘前检查强行平仓
                bool isOpenMarketCheckOrder = false;
                //强行平仓类型（当是盘前检查强行平仓时才有效）
                Types.QHForcedCloseType qhforceCloseType = new Types.QHForcedCloseType();

                //bool isFutureDayCheckContract = false;
                if (futuresorder is StockIndexFuturesOrderRequest2)
                {
                    // isExpiredContract = ((MercantileFuturesOrderRequest2)futuresorder).IsExpiredContract;
                    isOpenMarketCheckOrder = ((StockIndexFuturesOrderRequest2)futuresorder).IsForcedCloseOrder;
                    qhforceCloseType = ((StockIndexFuturesOrderRequest2)futuresorder).QHForcedCloseType;
                    //isFutureDayCheckContract = true;
                }
                #endregion

                try
                {
                    //不是开盘检查的强制平仓委托才进行检查
                    if (!isOpenMarketCheckOrder)
                    {
                        #region  add by 董鹏 2010-04-06
                        if (!string.IsNullOrEmpty(futuresorder.TraderId))
                        {
                            futuresorder.TraderId = futuresorder.TraderId.Trim();
                        }
                        if (!string.IsNullOrEmpty(futuresorder.FundAccountId))
                        {
                            futuresorder.FundAccountId = futuresorder.FundAccountId.Trim();
                        }
                        #endregion

                        #region 资金账号交易员ID数据检验
                        if ((string.IsNullOrEmpty(futuresorder.TraderId) &&
                             string.IsNullOrEmpty(futuresorder.FundAccountId)))
                        {
                            strErrorMessage = @"GT-2111:[现货委托]交易员ID或资金帐户无效.";
                            LogHelper.WriteInfo(strErrorMessage + "," + futuresorder);
                            goto EndProcess;
                        }
                        #endregion

                        #region 柜台委托时间判断
                        //柜台委托时间判断
                        if (!ValidateCenter.IsCountTradingTime(futuresorder.Code, ref strErrorMessage))
                        {
                            LogHelper.WriteInfo("OrderAccepter.DoStockIndexFuturesOrder" + strErrorMessage);

                            string oriMsg = "当前时间不接受委托";
                            if (strErrorMessage.Length > 0)
                                oriMsg = strErrorMessage;

                            strErrorMessage = @"GT-2112:[股指期货委托]" + oriMsg;
                            LogHelper.WriteInfo(strErrorMessage + "," + futuresorder);
                            goto EndProcess;
                        }
                        #endregion

                        #region 如果交易员ID为空根据资金账号获取
                        if (string.IsNullOrEmpty(futuresorder.TraderId))
                        {
                            //stockorder.TraderId =
                            //    CounterCache.Instance.GetTraderIdByFundAccount(stockorder.FundAccountId);

                            var user = AccountManager.Instance.GetUserByAccount(futuresorder.FundAccountId);
                            if (user != null)
                                futuresorder.TraderId = user.UserID;
                        }
                        #endregion

                        #region  交易规则判断
                        //交易规则判断
                        if (!ValidateCenter.Validate(futuresorder, ref strErrorMessage))
                        {
                            LogHelper.WriteInfo("OrderAccepter.DoStockIndexFuturesOrder" + strErrorMessage);
                            LogHelper.WriteInfo("[股指期货委托]交易规则判断失败，" + futuresorder);
                            goto EndProcess;
                        }
                        #endregion
                    }

                    #region 资金,持仓判断
                    if (!gzqhAcceptLogic.PersistentOrder(futuresorder, ref qhTodayEntrustTable, ref strErrorMessage
                             ))
                    {
                        LogHelper.WriteInfo("OrderAccepter.DoStockIndexFuturesOrder" + strErrorMessage);
                        goto EndProcess;
                    }
                    #endregion

                    #region 报盘
                    //报盘
                    canOfferOrder = true;

                    //强制平仓的过期合约进行假报盘，直接撮合成功
                    QhTodayEntrustTableEx tableEx = new QhTodayEntrustTableEx(qhTodayEntrustTable);
                    tableEx.TraderId = futuresorder.TraderId;

                    if (isOpenMarketCheckOrder)
                    {
                        tableEx.IsOpenMarketCheckOrder = isOpenMarketCheckOrder;
                        tableEx.QHForcedCloseType = qhforceCloseType;
                        _orderOfferCenter.OfferCloseHoldGZQHOrder(tableEx);
                    }
                    else
                    {
                        var ex = tableEx;
                        ex.IsOpenMarketCheckOrder = isOpenMarketCheckOrder;
                        _orderOfferCenter.OfferGZQHOrder(ex);
                    }

                    #endregion

                EndProcess:
                    //结束
                    ;
                }
                catch (Exception ex)
                {
                    strErrorMessage = @"GT-2113:[股指期货委托]接收处理异常，请查看日志.";
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

        EndProcess2:
            //结束
            ;

            orResult.OrderMessage = strErrorMessage;

            //只有报盘才分配一个柜台的委托单号
            if (canOfferOrder)
            {
                orResult.OrderId = qhTodayEntrustTable.EntrustNumber;
                orResult.IsSuccess = true;
            }

            return orResult;
        }

        /// <summary>
        /// 商品期货撤单事件
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="message"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public bool CancelMercantileFuturesOrder(string orderId, ref string message, out int errorType)
        {
            errorType = 0;
            LogHelper.WriteDebug("---->商品期货撤单OrderAccepter.CancelSPQHOrder，OrderID=" + orderId);
            var result = false;
            message = string.Empty;

            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            var tet = dal.GetModel(orderId);

            if (tet == null)
            {
                message = "GT-2124:[商品期货撤单委托]对应委托单不存在";
                return false;
            }

            var ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
            SPQHAcceptLogic spqhAcceptLogic = new SPQHAcceptLogic();

            //柜台委托时间判断
            if (!ValidateCenter.IsCountTradingTime(tet.ContractCode, ref message))
            {
                LogHelper.WriteDebug("OrderAccepter.CancelSPQHOrder" + message);
                message = @"GT-2122:[商品期货撤单委托]当前时间不接受委托.";
                return false;
            }

            //委托单是否存在,委托单状态是否允许撤单判断
            if (!spqhAcceptLogic.CancelOrderValidate(orderId, out tet, ref message))
            {
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
                return false;
            }

            //20090326：只有接受委托时间内、撮合时间外时才能进行内部撤单，其他时间统一走外部撤单
            if (ValidateCenter.IsMatchTradingTime(tet.ContractCode) && ost != Entity.Contants.Types.OrderStateType.DOSUnRequired)
            {
                //撮合时间内，走外部撤单
                //报盘
                result = _orderOfferCenter.CancelSPQHOrder(tet, ref message, out ost);
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
            }
            else
            {
                //非撮合时间内，走内部撤单
                try
                {
                    //内部撤单结果也要回推到前台
                    //(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
                    //spqhAcceptLogic.EndCancelEvent += SPQHAcceptLogic_EndCancelEvent;
                    result = spqhAcceptLogic.InternalCancelOrder(tet, "前台主动撤单");
                    if (result)
                    {
                        ost = Entity.Contants.Types.OrderStateType.DOSRemoved;
                    }
                    else
                    {
                        LogHelper.WriteInfo("OrderAccepter.CancelMercantileFuturesOrder撤单失败！");
                        message = "GT-2132:[商品期货撤单委托]撤单失败,请联系系统管理员。";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    LogHelper.WriteError(ex.Message, ex);
                    message = "GT-2123:[商品期货撤单委托]未报状态委托对象更新异常," + ex.Message; //TODO:
                }

                //预委托缓存中的也要删除
                OrderOfferCenter.Instance._orderCache.DeleteQHOrder(tet.ContractCode, orderId);
            }

            return result;
        }

        /// <summary>
        /// 商品期货撤单回报事件(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
        /// </summary>
        /// <param name="obj"></param>
        void SPQHAcceptLogic_EndCancelEvent(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> obj)
        {
            CounterOrderService.Instance.AcceptMercantileFuturesOrderDealOrder(obj);
        }

        /// <summary>
        /// 股指期货撤单事件
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="message"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public bool CancelStockIndexFuturesOrder(string orderId, ref string message, out int errorType)
        {
            errorType = 0;
            LogHelper.WriteDebug("---->股指期货撤单OrderAccepter.CancelGZQHOrder，OrderID=" + orderId);
            var result = false;
            message = string.Empty;

            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            var tet = dal.GetModel(orderId);
            //QhTodayEntrustTable qhOrder = DataRepository.QhTodayEntrustTableProvider.GetByEntrustNumber(orderId);
            if (tet == null)
            {
                message = "GT-2114:[股指期货撤单委托]对应委托单不存在";
                return false;
            }

            var ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
            GZQHAcceptLogic gzqhAcceptLogic = new GZQHAcceptLogic();
            //OrderLogicFlowBase<StockIndexFuturesOrderRequest, QhTodayEntrustTable, FutureDealBackEntity>
            //    _GZQHDataLogicProcessor
            //        = DataLogicProcessorFactory.GetGZQHDataLogicProcessor();

            //柜台委托时间判断
            if (!ValidateCenter.IsCountTradingTime(tet.ContractCode, ref message))
            {
                LogHelper.WriteInfo("OrderAccepter.CancelGZQHOrder" + message);
                message = @"GT-2112:[股指期货撤单委托]当前时间不接受委托.";
                return false;
            }

            //委托单是否存在,委托单状态是否允许撤单判断
            if (!gzqhAcceptLogic.CancelOrderValidate(orderId, out tet, ref message))
            {
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
                return false;
            }

            //20090326：只有接受委托时间内、撮合时间外时才能进行内部撤单，其他时间统一走外部撤单
            //20091215：李健华===当在临界开市时间时可能存在还有未报的单，这样就会走外部撤单就会产生报到撮合后
            //找不到委托单，因为此单还在内部缓存单中
            if (ValidateCenter.IsMatchTradingTime(tet.ContractCode)
                && ost != Entity.Contants.Types.OrderStateType.DOSUnRequired)
            {
                //撮合时间内，走外部撤单
                //报盘
                result = _orderOfferCenter.CancelGZQHOrder(tet, ref message, out ost);
                ost = Entity.Contants.Types.GetOrderStateType(tet.OrderStatusId);
            }
            else
            {
                //非撮合时间内，走内部撤单
                try
                {
                    //内部撤单也要回推到前台
                    //(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
                    gzqhAcceptLogic.EndCancelEvent += GZQHAcceptLogic_EndCancelEvent;
                    result = gzqhAcceptLogic.InternalCancelOrder(tet, "前台主动撤单");

                    if (result)
                    {
                        ost = Entity.Contants.Types.OrderStateType.DOSRemoved;
                    }
                    else
                    {
                        LogHelper.WriteInfo("OrderAccepter.CancelStockIndexFuturesOrder撤单失败！");
                        message = "GT-2132:[股指期货撤单委托]撤单失败,请联系系统管理员。";
                    }
                    //ost = Types.OrderStateType.DOSRemoved;
                }
                catch (Exception ex)
                {
                    result = false;
                    LogHelper.WriteError(ex.Message, ex);
                    message = "GT-2113:[股指期货撤单委托]未报状态委托对象更新异常," + ex.Message; //TODO:
                }

                //预委托缓存中的也要删除
                OrderOfferCenter.Instance._orderCache.DeleteGZQHOrder(tet.ContractCode, orderId);
            }


            return result;
        }

        /// <summary>
        /// 股指期货撤单回报事件(已无效，AcceptLogic不会再触发，内部撤单与外部撤单统一，由ReckonUnit处理）
        /// </summary>
        /// <param name="obj"></param>
        void GZQHAcceptLogic_EndCancelEvent(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> obj)
        {
            CounterOrderService.Instance.AcceptStockIndexFuturesDealOrder(obj);
        }
        #endregion

        #endregion
    }
}