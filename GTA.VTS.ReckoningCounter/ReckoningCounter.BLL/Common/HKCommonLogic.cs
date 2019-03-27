#region Using Namespace

using System;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.BLL.Reckoning.Instantaneous;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.HK.Hold;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 港股基本操作帮助类
    /// 作者：宋涛
    /// </summary>
    public static class HKCommonLogic
    {
        #region 撤单逻辑，供内、外部撤单使用

        /// <summary>
        /// 处理撤单委托状态
        /// </summary>
        /// <param name="tet">委托表</param>
        public static void ProcessCancelOrderStatus(HK_TodayEntrustInfo tet)
        {
            //未报，待报，已报的单，做废单处理
            if (tet.OrderStatusID == (int) Types.OrderStateType.DOSUnRequired ||
                tet.OrderStatusID == (int) Types.OrderStateType.DOSRequiredSoon ||
                tet.OrderStatusID == (int) Types.OrderStateType.DOSIsRequired)
            {
                tet.OrderStatusID = (int) Types.OrderStateType.DOSCanceled;
            }
                //已报待撤的单：默认撤单成功，改状态为已撤
            else if (tet.OrderStatusID == (int) Types.OrderStateType.DOSRequiredRemoveSoon)
            {
                tet.OrderStatusID = (int) Types.OrderStateType.DOSRemoved;
            }
                //部成,部成待撤的单：默认撤单成功，改状态为部撤
            else if (tet.OrderStatusID == (int) Types.OrderStateType.DOSPartDealRemoveSoon ||
                     tet.OrderStatusID == (int) Types.OrderStateType.DOSPartDealed)
            {
                tet.OrderStatusID = (int) Types.OrderStateType.DOSPartRemoved;
            }

            //其他的保持原有状态
        }

        #endregion

        #region Persist方法

        /// <summary>
        /// 创建港股委托标识
        /// </summary>
        /// <returns>委托单号</returns>
        public static string BuildHKOrderNo()
        {
            return ReckonDataLogic.BuildHKOrderNo();
        }

        /// <summary>
        /// 构建港股委托单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="originalOrder"></param>
        /// <param name="strHoldingAccount"></param>
        /// <param name="strCapitalAccount"></param>
        /// <param name="iCurType"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public static string BuildHKOrder(ref HK_TodayEntrustInfo order, HKOrderRequest originalOrder,
                                         string strHoldingAccount, string strCapitalAccount, int iCurType,
                                         ref string strMessage)
        {
            return BuildHKOrder(ref order, originalOrder, strHoldingAccount, strCapitalAccount, iCurType, false, "",
                                ref strMessage);
        }

        /// <summary>
        /// 构建港股委托单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="originalOrder"></param>
        /// <param name="strHoldingAccount"></param>
        /// <param name="strCapitalAccount"></param>
        /// <param name="iCurType"></param>
        /// <param name="modifyOrderNumber">原始委托单号</param>
        /// <param name="strMessage"></param>
        /// <param name="isModifyOrder">是否是改单</param>
        /// <returns></returns>
        public static string BuildHKOrder(ref HK_TodayEntrustInfo order, HKOrderRequest originalOrder,
                                          string strHoldingAccount, string strCapitalAccount, int iCurType,
                                          bool isModifyOrder, string modifyOrderNumber, ref string strMessage)
        {
            if (order == null)
                order = new HK_TodayEntrustInfo();

            order.EntrustNumber = BuildHKOrderNo();
            order.CurrencyTypeID = iCurType;
            order.TradeUnitID = (int) originalOrder.OrderUnitType;
            order.EntrustAmount = (int) originalOrder.OrderAmount;
            order.EntrustPrice = Convert.ToDecimal(originalOrder.OrderPrice);
            order.EntrustTime = DateTime.Now;
            order.OfferTime = DateTime.Now;
            order.OrderPriceType = (int) originalOrder.OrderWay;
            order.BuySellTypeID = (int) originalOrder.BuySell;
            order.OrderStatusID = (int) Types.OrderStateType.DOSUnRequired;

            if (strHoldingAccount == null)
                strHoldingAccount = "";
            order.HoldAccount = strHoldingAccount.Trim();

            if (strCapitalAccount == null)
                strCapitalAccount = "";
            order.CapitalAccount = strCapitalAccount.Trim();

            if (originalOrder.PortfoliosId == null)
                originalOrder.PortfoliosId = "";
            order.PortfolioLogo = originalOrder.PortfoliosId.Trim();

            if (originalOrder.Code == null)
                originalOrder.Code = "";
            order.Code = originalOrder.Code.Trim();

            order.TradeAmount = 0;
            order.TradeAveragePrice = 0;
            order.CancelAmount = 0;
            order.CancelLogo = true;

            if (originalOrder.ChannelID == null)
                originalOrder.ChannelID = "";
            order.CallbackChannlID = originalOrder.ChannelID.Trim();

            order.IsModifyOrder = isModifyOrder;
            order.ModifyOrderNumber = modifyOrderNumber;

            order.OrderMessage = "";
            order.McOrderID = "";
            CheckEntrustLength(order);

#if(DEBUG)
            LogHelper.WriteDebug("HKCommonLogic.BuildHKOrder:" + order);
#endif

            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
            dal.Add(order);

            return order.EntrustNumber;
        }

        /// <summary>
        /// 检查是否有超过数据库限制长度的字段
        /// </summary>
        /// <param name="tet"></param>
        private static void CheckEntrustLength(HK_TodayEntrustInfo tet)
        {
            if (tet.PortfolioLogo.Length > 25)
            {
                string format1 = "CheckEntrustLength[PortfolioLogo={0}]";
                string desc1 = string.Format(format1, tet.PortfolioLogo);
                LogHelper.WriteDebug(desc1);
                tet.PortfolioLogo = tet.PortfolioLogo.Substring(0, 25);
            }

            if (tet.HoldAccount.Length > 20)
            {
                string format1 = "CheckEntrustLength[StockAccount={0}]";
                string desc1 = string.Format(format1, tet.HoldAccount);
                LogHelper.WriteDebug(desc1);
                tet.HoldAccount = tet.HoldAccount.Substring(0, 20);
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

            if (tet.CallbackChannlID.Length > 50)
            {
                string format1 = "CheckEntrustLength[CallbackChannlId={0}]";
                string desc1 = string.Format(format1, tet.CallbackChannlID);
                LogHelper.WriteDebug(desc1);
                tet.CallbackChannlID = tet.CallbackChannlID.Substring(0, 50);
            }

            if (tet.McOrderID.Length > 100)
            {
                string format1 = "CheckEntrustLength[McOrderId={0}]";
                string desc1 = string.Format(format1, tet.McOrderID);
                LogHelper.WriteDebug(desc1);
                tet.McOrderID = tet.McOrderID.Substring(0, 100);
            }
        }

        /// <summary>
        /// 获取持仓内存表，如果内存中不存在，那么再到数据库查找
        /// </summary>
        /// <param name="holdAccount">持仓账户</param>
        /// <param name="code">代码</param>
        /// <param name="tradeCurrencyType">币种</param>
        /// <returns>内存表</returns>
        public static HKHoldMemoryTable GetHoldMemoryTable(string holdAccount, string code, int tradeCurrencyType)
        {
            HKHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(holdAccount, code,
                                                                                                tradeCurrencyType);
                //holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    HK_AccountHoldDal dal = new HK_AccountHoldDal();
                    var hold = dal.GetHKAccountHoldInfo(holdAccount, code, tradeCurrencyType);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        MemoryDataManager.HKHoldMemoryList.AddAccountHoldTableToMemory(hold);
                    }
                    else
                    {
                        //如果数据库也没有，那么代表无持仓
                        return null;
                    }

                    holdMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(holdAccount, code,
                                                                                                    tradeCurrencyType);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("HKCommonLogic.GetHoldMemoryTable-" + ex.Message, ex);
            }

            return holdMemory;
        }

        /// <summary>
        /// 获取持仓内存表，如果内存中不存在，那么再到数据库查找
        /// </summary>
        /// <param name="accountHoldLogoId">持仓id</param>
        /// <returns></returns>
        public static HKHoldMemoryTable GetHoldMemoryTable(int accountHoldLogoId)
        {
            HKHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(accountHoldLogoId);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    HK_AccountHoldDal dal = new HK_AccountHoldDal();
                    var hold = dal.GetModel(accountHoldLogoId);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        MemoryDataManager.HKHoldMemoryList.AddAccountHoldTableToMemory(hold);
                    }
                    else
                    {
                        //如果数据库也没有，那么代表无持仓
                        return null;
                    }

                    holdMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(accountHoldLogoId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("HKCommonLogic.GetHoldMemoryTable-" + ex.Message, ex);
            }

            return holdMemory;
        }

        /// <summary>
        /// 构建港股撤单成交回报
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="rde"></param>
        /// <param name="tm"></param>
        /// <param name="trade"></param>
        /// <param name="isInternalCancel"></param>
        /// <returns></returns>
        public static string BuildHKCancelRpt(HK_TodayEntrustInfo tet, CancelOrderEntity rde,
                                              ReckoningTransaction tm, out HK_TodayTradeInfo trade,
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
            var hkDealrpt = new HK_TodayTradeInfo();

            //xhDealrpt.TradeNumber = this.BuildXHDealOrderNo();
            hkDealrpt.TradeNumber = rde.Id; //不再自己构建id，使用撤单回报的id，一一对应
            //成交时间
            hkDealrpt.TradeTime = DateTime.Now;
            //成交价
            hkDealrpt.TradePrice = 0;
            //成交单位
            hkDealrpt.TradeUnitId = tet.TradeUnitID;
            //成交量
            hkDealrpt.TradeAmount = Convert.ToInt32(rde.OrderVolume);
            //股东代码
            hkDealrpt.HoldAccount = tet.HoldAccount;
            //资金帐户
            hkDealrpt.CapitalAccount = tet.CapitalAccount;
            //成交回报类型
            if (isInternalCancel)
            {
                hkDealrpt.TradeTypeId = (int) Types.DealRptType.DRTInternalCanceled;
            }
            else
            {
                hkDealrpt.TradeTypeId = (int) Types.DealRptType.DRTCanceled;
            }
            //现货名称
            hkDealrpt.Code = tet.Code;

            //印花税
            hkDealrpt.StampTax = 0;
            //佣金
            hkDealrpt.Commission = 0;
            //过户费
            hkDealrpt.TransferAccountFee = 0;
            //交易系统使用费
            hkDealrpt.TradingSystemUseFee = 0;
            //监管费
            hkDealrpt.MonitoringFee = 0;
            hkDealrpt.ClearingFee = 0;

            //委托价格
            hkDealrpt.EntrustPrice = tet.EntrustPrice;
            //成交金额
            hkDealrpt.TradeCapitalAmount = hkDealrpt.TradePrice*hkDealrpt.TradeAmount;
            //投组标识
            hkDealrpt.PortfolioLogo = tet.PortfolioLogo;
            //货币类型
            hkDealrpt.CurrencyTypeId = tet.CurrencyTypeID;
            //买卖方向
            hkDealrpt.BuySellTypeId = tet.BuySellTypeID;

            hkDealrpt.EntrustNumber = tet.EntrustNumber;
            HK_TodayTradeDal hkTodayTradeDal = new HK_TodayTradeDal();
            hkTodayTradeDal.Add(hkDealrpt, tm);
            result = hkDealrpt.TradeNumber;

            trade = hkDealrpt;
            return result;
        }

        /// <summary>
        /// 构建港股成交回报
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="sdbe"></param>
        /// <param name="xhcr"></param>
        /// <param name="dealCapital"></param>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static HK_TodayTradeInfo BuildHKDealRpt(HK_TodayEntrustInfo tet, HKDealBackEntity sdbe,
                                                       HKCostResult xhcr, decimal dealCapital,
                                                       ReckoningTransaction tm)
        {
            string result = string.Empty;

            //成交回报实体
            var hkDealRpt = new HK_TodayTradeInfo();

            //xhDealrpt.TradeNumber = this.BuildXHDealOrderNo();
            hkDealRpt.TradeNumber = sdbe.ID; //不再自己构建id，使用成交回报的id，一一对应
            //成交时间
            hkDealRpt.TradeTime = sdbe.DealTime;
            //成交价
            hkDealRpt.TradePrice = sdbe.DealPrice;
            //成交单位
            hkDealRpt.TradeUnitId = tet.TradeUnitID;
            //成交量
            hkDealRpt.TradeAmount = Convert.ToInt32(sdbe.DealAmount);
            //股东代码
            hkDealRpt.HoldAccount = tet.HoldAccount;
            //资金帐户
            hkDealRpt.CapitalAccount = tet.CapitalAccount;
            //成交回报类型
            hkDealRpt.TradeTypeId = (int) Types.DealRptType.DRTDealed;
            //现货名称
            hkDealRpt.Code = tet.Code;
            //印花税
            hkDealRpt.StampTax = xhcr.StampDuty;
            //佣金
            hkDealRpt.Commission = xhcr.Commision;
            //过户费
            hkDealRpt.TransferAccountFee = xhcr.TransferToll;
            //交易系统使用费
            hkDealRpt.TradingSystemUseFee = xhcr.TradeSystemFees;
            //交易手续费用（只有港股才有）
            hkDealRpt.TradeProceduresFee = xhcr.PoundageSingleValue;
            //监管费
            hkDealRpt.MonitoringFee = xhcr.MonitoringFee;
            //结算费
            hkDealRpt.ClearingFee = xhcr.ClearingFees;
            //委托价格
            hkDealRpt.EntrustPrice = tet.EntrustPrice;
            //成交金额
            hkDealRpt.TradeCapitalAmount = dealCapital; // xhDealrpt.TradePrice*xhDealrpt.TradeAmount;//TODO:是否正确？没有算比例
            //投组标识
            hkDealRpt.PortfolioLogo = tet.PortfolioLogo;
            //货币类型
            hkDealRpt.CurrencyTypeId = tet.CurrencyTypeID;
            //买卖方向
            hkDealRpt.BuySellTypeId = tet.BuySellTypeID;

            hkDealRpt.EntrustNumber = tet.EntrustNumber;
            HK_TodayTradeDal hkTodayTradeDal = new HK_TodayTradeDal();

            if (hkTodayTradeDal.Exists(hkDealRpt.TradeNumber))
            {
                string format = "BuildHKDealRpt数据库已经存在TradeNumber={0}";
                string desc = string.Format(format, hkDealRpt.TradeNumber);
                LogHelper.WriteDebug(desc);

                //xhDealrpt = xhTodayTradeTableDal.GetModel(xhDealrpt.TradeNumber);
                return null;
            }

            try
            {
                hkTodayTradeDal.Add(hkDealRpt, tm);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                hkDealRpt = null;
            }

            return hkDealRpt;
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
            HK_CapitalAccountFreezeInfo caf = new HK_CapitalAccountFreezeInfo();
            caf.CapitalAccountLogo = capitalAccountId;
            caf.EntrustNumber = entrustNumber;
            caf.FreezeTypeLogo = (int) Types.FreezeType.DelegateFreeze;
            caf.FreezeTime = DateTime.Now;
            caf.FreezeCapitalAmount = 0;
            caf.FreezeCost = 0;

            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            return dal.Add(caf);
        }

        #endregion
    }
}