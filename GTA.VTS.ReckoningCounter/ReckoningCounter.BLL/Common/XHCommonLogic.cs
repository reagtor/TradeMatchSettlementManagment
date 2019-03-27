#region Using Namespace

using System;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.BLL.Reckoning.Instantaneous;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.XH.Hold;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 现货基本操作帮助类
    /// 作者：宋涛
    /// </summary>
    public static class XHCommonLogic
    {
        #region 撤单逻辑，供内、外部撤单使用

        /// <summary>
        /// 处理撤单委托状态
        /// </summary>
        /// <param name="tet">委托表</param>
        public static void ProcessCancelOrderStatus(XH_TodayEntrustTableInfo tet)
        {
            //未报，待报，已报的单，做废单处理
            if (tet.OrderStatusId == (int) Types.OrderStateType.DOSUnRequired ||
                tet.OrderStatusId == (int) Types.OrderStateType.DOSRequiredSoon ||
                tet.OrderStatusId == (int) Types.OrderStateType.DOSIsRequired)
            {
                tet.OrderStatusId = (int) Types.OrderStateType.DOSCanceled;
            }
                //已报待撤的单：默认撤单成功，改状态为已撤
            else if (tet.OrderStatusId == (int) Types.OrderStateType.DOSRequiredRemoveSoon)
            {
                tet.OrderStatusId = (int) Types.OrderStateType.DOSRemoved;
            }
                //部成,部成待撤的单：默认撤单成功，改状态为部撤
            else if (tet.OrderStatusId == (int) Types.OrderStateType.DOSPartDealRemoveSoon ||
                     tet.OrderStatusId == (int) Types.OrderStateType.DOSPartDealed)
            {
                tet.OrderStatusId = (int) Types.OrderStateType.DOSPartRemoved;
            }

            //其他的保持原有状态
        }

        #endregion

        #region Persist方法

        /// <summary>
        /// 创建现货委托标识
        /// </summary>
        /// <returns>委托单号</returns>
        public static string BuildXHOrderNo()
        {
            return ReckonDataLogic.BuildXHOrderNo();
        }

        /// <summary>
        /// 构建现货委托单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="originalOrder"></param>
        /// <param name="strHoldingAccount"></param>
        /// <param name="strCapitalAccount"></param>
        /// <param name="iCurType"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public static string BuildXhOrder(ref XH_TodayEntrustTableInfo order, StockOrderRequest originalOrder,
                                          string strHoldingAccount, string strCapitalAccount, int iCurType,
                                          ref string strMessage)
        {
            if (order == null)
                order = new XH_TodayEntrustTableInfo();

            order.EntrustNumber = BuildXHOrderNo();
            order.CurrencyTypeId = iCurType;
            order.TradeUnitId = (int) originalOrder.OrderUnitType;
            order.EntrustAmount = (int) originalOrder.OrderAmount;
            order.EntrustPrice = Convert.ToDecimal(originalOrder.OrderPrice);
            order.EntrustTime = DateTime.Now;
            order.OfferTime = DateTime.Now;
            order.IsMarketValue = originalOrder.OrderWay == Types.OrderPriceType.OPTMarketPrice
                                      ? true
                                      : false;
            order.BuySellTypeId = (int) originalOrder.BuySell;
            order.OrderStatusId = (int) Types.OrderStateType.DOSUnRequired;

            if (strHoldingAccount == null)
                strHoldingAccount = "";
            order.StockAccount = strHoldingAccount.Trim();

            if (strCapitalAccount == null)
                strCapitalAccount = "";
            order.CapitalAccount = strCapitalAccount.Trim();

            if (originalOrder.PortfoliosId == null)
                originalOrder.PortfoliosId = "";
            order.PortfolioLogo = originalOrder.PortfoliosId.Trim();

            if (originalOrder.Code == null)
                originalOrder.Code = "";
            order.SpotCode = originalOrder.Code.Trim();

            order.TradeAmount = 0;
            order.TradeAveragePrice = 0;
            order.CancelAmount = 0;
            order.CancelLogo = true;

            if (originalOrder.ChannelID == null)
                originalOrder.ChannelID = "";
            order.CallbackChannlId = originalOrder.ChannelID.Trim();

            order.IsMarketValue = originalOrder.OrderWay == Types.OrderPriceType.OPTMarketPrice
                                      ? true
                                      : false;
            order.OrderMessage = "";
            order.McOrderId = "";
            CheckEntrustLength(order);

#if(DEBUG)
            LogHelper.WriteDebug("XHCommonLogic.BuildXhOrder:" + order);
#endif

            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            dal.Add(order);

            return order.EntrustNumber;
        }

        /// <summary>
        /// 检查是否有超过数据库限制长度的字段
        /// </summary>
        /// <param name="tet"></param>
        private static void CheckEntrustLength(XH_TodayEntrustTableInfo tet)
        {
            if (tet.PortfolioLogo.Length > 25)
            {
                string format1 = "CheckEntrustLength[PortfolioLogo={0}]";
                string desc1 = string.Format(format1, tet.PortfolioLogo);
                LogHelper.WriteDebug(desc1);
                tet.PortfolioLogo = tet.PortfolioLogo.Substring(0, 25);
            }

            if (tet.StockAccount.Length > 20)
            {
                string format1 = "CheckEntrustLength[StockAccount={0}]";
                string desc1 = string.Format(format1, tet.StockAccount);
                LogHelper.WriteDebug(desc1);
                tet.StockAccount = tet.StockAccount.Substring(0, 20);
            }

            if (tet.CapitalAccount.Length > 20)
            {
                string format1 = "CheckEntrustLength[CapitalAccount={0}]";
                string desc1 = string.Format(format1, tet.CapitalAccount);
                LogHelper.WriteDebug(desc1);
                tet.CapitalAccount = tet.CapitalAccount.Substring(0, 20);
            }

            if (tet.OrderMessage.Length > 100)
            {
                string format1 = "CheckEntrustLength[OrderMessage={0}]";
                string desc1 = string.Format(format1, tet.OrderMessage);
                LogHelper.WriteDebug(desc1);
                tet.OrderMessage = tet.OrderMessage.Substring(0, 100);
            }

            if (tet.CallbackChannlId.Length > 50)
            {
                string format1 = "CheckEntrustLength[CallbackChannlId={0}]";
                string desc1 = string.Format(format1, tet.CallbackChannlId);
                LogHelper.WriteDebug(desc1);
                tet.CallbackChannlId = tet.CallbackChannlId.Substring(0, 50);
            }

            if (tet.McOrderId.Length > 100)
            {
                string format1 = "CheckEntrustLength[McOrderId={0}]";
                string desc1 = string.Format(format1, tet.McOrderId);
                LogHelper.WriteDebug(desc1);
                tet.McOrderId = tet.McOrderId.Substring(0, 100);
            }
        }

        /// <summary>
        /// 获取持仓内存表，如果内存中不存在，那么再到数据库查找
        /// </summary>
        /// <param name="holdAccount">持仓账户</param>
        /// <param name="code">代码</param>
        /// <param name="tradeCurrencyType">币种</param>
        /// <returns>内存表</returns>
        public static XHHoldMemoryTable GetHoldMemoryTable(string holdAccount, string code, int tradeCurrencyType)
        {
            XHHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.XHHoldMemoryList.GetByHoldAccountAndCurrencyType(holdAccount, code,
                                                                                                tradeCurrencyType);
                //holdMemory = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
                    var hold = dal.GetXhAccountHoldTable(holdAccount, code, tradeCurrencyType);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        MemoryDataManager.XHHoldMemoryList.AddXHAccountHoldTableToMemory(hold);
                    }
                    else
                    {
                        //如果数据库也没有，那么代表无持仓
                        return null;
                    }

                    holdMemory = MemoryDataManager.XHHoldMemoryList.GetByHoldAccountAndCurrencyType(holdAccount, code,
                                                                                                    tradeCurrencyType);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("XHCommonLogic.GetHoldMemoryTable-" + ex.Message, ex);
            }

            return holdMemory;
        }

        /// <summary>
        /// 获取持仓内存表，如果内存中不存在，那么再到数据库查找
        /// </summary>
        /// <param name="accountHoldLogoId">持仓id</param>
        /// <returns></returns>
        public static XHHoldMemoryTable GetHoldMemoryTable(int accountHoldLogoId)
        {
            XHHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(accountHoldLogoId);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
                    var hold = dal.GetModel(accountHoldLogoId);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        MemoryDataManager.XHHoldMemoryList.AddXHAccountHoldTableToMemory(hold);
                    }
                    else
                    {
                        //如果数据库也没有，那么代表无持仓
                        return null;
                    }

                    holdMemory = MemoryDataManager.XHHoldMemoryList.GetByAccountHoldLogoId(accountHoldLogoId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("XHCommonLogic.GetHoldMemoryTable-" + ex.Message, ex);
            }

            return holdMemory;
        }

        /// <summary>
        /// 构建现货撤单成交回报
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="rde"></param>
        /// <param name="tm"></param>
        /// <param name="trade"></param>
        /// <param name="isInternalCancel"></param>
        /// <returns></returns>
        public static string BuildXHCancelRpt(XH_TodayEntrustTableInfo tet, CancelOrderEntity rde,
                                              ReckoningTransaction tm, out XH_TodayTradeTableInfo trade,
                                              bool isInternalCancel)
        {
            string result = string.Empty;

            //当为价格错误的撤单时，直接作为废单，不记录到数据库中。
            if (rde.OrderVolume == -1)
            {
                trade = null;
                return result;
            }

            //成交回报实体
            var xhDealrpt = new XH_TodayTradeTableInfo();

            //xhDealrpt.TradeNumber = this.BuildXHDealOrderNo();
            xhDealrpt.TradeNumber = rde.Id; //不再自己构建id，使用撤单回报的id，一一对应
            //成交时间
            xhDealrpt.TradeTime = DateTime.Now;
            //成交价
            xhDealrpt.TradePrice = 0;
            //成交单位
            xhDealrpt.TradeUnitId = tet.TradeUnitId;
            //成交量
            xhDealrpt.TradeAmount = Convert.ToInt32(rde.OrderVolume);
            //股东代码
            xhDealrpt.StockAccount = tet.StockAccount;
            //资金帐户
            xhDealrpt.CapitalAccount = tet.CapitalAccount;
            //成交回报类型
            if (isInternalCancel)
            {
                xhDealrpt.TradeTypeId = (int) Types.DealRptType.DRTInternalCanceled;
            }
            else
            {
                xhDealrpt.TradeTypeId = (int) Types.DealRptType.DRTCanceled;
            }
            //现货名称
            xhDealrpt.SpotCode = tet.SpotCode;

            //印花税
            xhDealrpt.StampTax = 0;
            //佣金
            xhDealrpt.Commission = 0;
            //过户费
            xhDealrpt.TransferAccountFee = 0;
            //交易系统使用费
            xhDealrpt.TradingSystemUseFee = 0;
            //监管费
            xhDealrpt.MonitoringFee = 0;
            xhDealrpt.ClearingFee = 0;

            //委托价格
            xhDealrpt.EntrustPrice = tet.EntrustPrice;
            //成交金额
            xhDealrpt.TradeCapitalAmount = xhDealrpt.TradePrice*xhDealrpt.TradeAmount;
            //投组标识
            xhDealrpt.PortfolioLogo = tet.PortfolioLogo;
            //货币类型
            xhDealrpt.CurrencyTypeId = tet.CurrencyTypeId;
            //买卖方向
            xhDealrpt.BuySellTypeId = tet.BuySellTypeId;

            xhDealrpt.EntrustNumber = tet.EntrustNumber;
            XH_TodayTradeTableDal xhTodayTradeTableDal = new XH_TodayTradeTableDal();
            xhTodayTradeTableDal.Add(xhDealrpt, tm);
            result = xhDealrpt.TradeNumber;

            trade = xhDealrpt;
            return result;
        }

        /// <summary>
        /// 构建现货成交回报
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="sdbe"></param>
        /// <param name="xhcr"></param>
        /// <param name="dealCapital"></param>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static XH_TodayTradeTableInfo BuildXHDealRpt(XH_TodayEntrustTableInfo tet, StockDealBackEntity sdbe,
                                                            XHCostResult xhcr, decimal dealCapital,
                                                            ReckoningTransaction tm)
        {
            string result = string.Empty;

            //成交回报实体
            var xhDealrpt = new XH_TodayTradeTableInfo();

            //xhDealrpt.TradeNumber = this.BuildXHDealOrderNo();
            xhDealrpt.TradeNumber = sdbe.Id; //不再自己构建id，使用成交回报的id，一一对应
            //成交时间
            xhDealrpt.TradeTime = sdbe.DealTime;
            //成交价
            xhDealrpt.TradePrice = sdbe.DealPrice;
            //成交单位
            xhDealrpt.TradeUnitId = tet.TradeUnitId;
            //成交量
            xhDealrpt.TradeAmount = Convert.ToInt32(sdbe.DealAmount);
            //股东代码
            xhDealrpt.StockAccount = tet.StockAccount;
            //资金帐户
            xhDealrpt.CapitalAccount = tet.CapitalAccount;
            //成交回报类型
            xhDealrpt.TradeTypeId = (int) Types.DealRptType.DRTDealed;
            //现货名称
            xhDealrpt.SpotCode = tet.SpotCode;
            //印花税
            xhDealrpt.StampTax = xhcr.StampDuty;
            //佣金
            xhDealrpt.Commission = xhcr.Commision;
            //过户费
            xhDealrpt.TransferAccountFee = xhcr.TransferToll;
            //交易系统使用费
            xhDealrpt.TradingSystemUseFee = xhcr.TradeSystemFees;
            //监管费
            xhDealrpt.MonitoringFee = xhcr.MonitoringFee;
            //结算费
            xhDealrpt.ClearingFee = xhcr.ClearingFees;
            //委托价格
            xhDealrpt.EntrustPrice = tet.EntrustPrice;
            //成交金额
            xhDealrpt.TradeCapitalAmount = dealCapital; // xhDealrpt.TradePrice*xhDealrpt.TradeAmount;//TODO:是否正确？没有算比例
            //投组标识
            xhDealrpt.PortfolioLogo = tet.PortfolioLogo;
            //货币类型
            xhDealrpt.CurrencyTypeId = tet.CurrencyTypeId;
            //买卖方向
            xhDealrpt.BuySellTypeId = tet.BuySellTypeId;

            xhDealrpt.EntrustNumber = tet.EntrustNumber;
            XH_TodayTradeTableDal xhTodayTradeTableDal = new XH_TodayTradeTableDal();

            if (xhTodayTradeTableDal.Exists(xhDealrpt.TradeNumber))
            {
                string format = "BuildXHDealRpt数据库已经存在TradeNumber={0}";
                string desc = string.Format(format, xhDealrpt.TradeNumber);
                LogHelper.WriteDebug(desc);

                //xhDealrpt = xhTodayTradeTableDal.GetModel(xhDealrpt.TradeNumber);
                return null;
            }

            try
            {
                xhTodayTradeTableDal.Add(xhDealrpt, tm);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                xhDealrpt = null;
            }

            return xhDealrpt;
        }

        #endregion

        #region 功能方法

        /// <summary>
        /// 插入一条空的资金冻结记录
        /// </summary>
        /// <param name="capitalAccountId"></param>
        /// <param name="entrustNumber"></param>
        /// <returns></returns>
        public static int InsertNullCapitalFreeze(int capitalAccountId, string entrustNumber)
        {
            XH_CapitalAccountFreezeTableInfo caf = new XH_CapitalAccountFreezeTableInfo();
            caf.CapitalAccountLogo = capitalAccountId;
            caf.EntrustNumber = entrustNumber;
            caf.FreezeTypeLogo = (int) Types.FreezeType.DelegateFreeze;
            caf.FreezeTime = DateTime.Now;
            caf.FreezeCapitalAmount = 0;
            caf.FreezeCost = 0;

            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            return dal.Add(caf);
        }

        #endregion
    }
}