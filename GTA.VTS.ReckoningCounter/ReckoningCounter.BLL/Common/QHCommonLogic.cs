#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.BLL.Reckoning.Instantaneous;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.QH.Hold;
using ReckoningCounter.Model;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 期货货基本操作帮助类
    /// 作者：宋涛
    /// </summary>
    public static class QHCommonLogic
    {
        #region 撤单逻辑，供内、外部撤单使用

        public static void ProcessCancelOrderStatus(QH_TodayEntrustTableInfo tet)
        {
            //未报，待报，已报的单，做废单处理
            if (tet.OrderStatusId == (int)Types.OrderStateType.DOSUnRequired ||
                tet.OrderStatusId == (int)Types.OrderStateType.DOSRequiredSoon ||
                tet.OrderStatusId == (int)Types.OrderStateType.DOSIsRequired)
            {
                tet.OrderStatusId = (int)Types.OrderStateType.DOSCanceled;
            }
            //已报待撤的单：默认撤单成功，改状态为已撤
            else if (tet.OrderStatusId == (int)Types.OrderStateType.DOSRequiredRemoveSoon)
            {
                tet.OrderStatusId = (int)Types.OrderStateType.DOSRemoved;
            }
            //部成,部成待撤的单：默认撤单成功，改状态为部撤
            else if (tet.OrderStatusId == (int)Types.OrderStateType.DOSPartDealRemoveSoon ||
                     tet.OrderStatusId == (int)Types.OrderStateType.DOSPartDealed)
            {
                tet.OrderStatusId = (int)Types.OrderStateType.DOSPartRemoved;
            }

            //其他的保持原有状态
        }

        #endregion

        #region Persist方法

        /// <summary>
        /// 创建期货委托标识
        /// </summary>
        /// <returns></returns>
        public static string BuildQHOrderNo()
        {
            return ReckonDataLogic.BuildQHOrderNo();
        }

        /// <summary>
        /// 构建期货撤单回报
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="rde"></param>
        /// <param name="tm"></param>
        /// <param name="isInternalCancelOrder"></param>
        /// <returns></returns>
        public static QH_TodayTradeTableInfo BuildQHCancelRpt(QH_TodayEntrustTableInfo tet, CancelOrderEntity rde,
                                                              ReckoningTransaction tm, bool isInternalCancelOrder)
        {
            string result = string.Empty;

            //当为价格错误的撤单时，直接作为废单，不记录到数据库中。
            if (rde.OrderVolume == -1)
                return null;

            //成交回报实体
            var qhDealrpt = new QH_TodayTradeTableInfo();

            qhDealrpt.TradeNumber = rde.Id; //不再自己构建id，使用撤单回报的id，一一对应
            //成交时间
            qhDealrpt.TradeTime = DateTime.Now;
            //成交价
            qhDealrpt.TradePrice = 0;
            //成交量
            qhDealrpt.TradeAmount = Convert.ToInt32(rde.OrderVolume);
            //股东代码
            qhDealrpt.TradeAccount = tet.TradeAccount;
            //资金帐户
            qhDealrpt.CapitalAccount = tet.CapitalAccount;
            //成交回报类型
            if (isInternalCancelOrder)
            {
                qhDealrpt.TradeTypeId = (int)Types.DealRptType.DRTInternalCanceled;
            }
            else
            {
                qhDealrpt.TradeTypeId = (int)Types.DealRptType.DRTCanceled;
            }
            //现货名称
            qhDealrpt.ContractCode = tet.ContractCode;

            //交易手续费
            qhDealrpt.TradeProceduresFee = 0;
            //保证金
            qhDealrpt.Margin = 0;

            //委托价格
            qhDealrpt.EntrustPrice = tet.EntrustPrice;
            //委托单号
            qhDealrpt.EntrustNumber = tet.EntrustNumber;
            //投组标识
            qhDealrpt.PortfolioLogo = tet.PortfolioLogo;
            //货币类型
            qhDealrpt.CurrencyTypeId = tet.CurrencyTypeId;
            //开平方向
            qhDealrpt.OpenCloseTypeId = tet.OpenCloseTypeId;
            //买卖方向
            qhDealrpt.BuySellTypeId = tet.BuySellTypeId;
            //成交单位
            qhDealrpt.TradeUnitId = tet.TradeUnitId;
            //2009-12-03 add 李健华 
            qhDealrpt.MarketProfitLoss = 0;
            //==========
            QH_TodayTradeTableDal qh_TodayTradeTableDal = new QH_TodayTradeTableDal();
            // var provider = new SqlQhTodayTradeTableProvider(TransactionFactory.RC_ConnectionString, true, string.Empty);

            //provider.Insert(tm, qhDealrpt);
            qh_TodayTradeTableDal.Add(qhDealrpt, tm);

            return qhDealrpt;
        }

        /// <summary>
        /// 构建股指期货成交回报
        /// </summary>
        /// <param name="tet"></param>
        /// <param name="sdbe"></param>
        /// <param name="qhcr"></param>
        /// <param name="dMargin"></param>
        /// <param name="tm"></param>
        /// <param name="marketProfitLoss"></param>
        /// <param name="dealRptType">成交类型</param>
        /// <returns></returns>
        public static QH_TodayTradeTableInfo BuildGZQHDealRpt(QH_TodayEntrustTableInfo tet, FutureDealBackEntity sdbe,
                                                              QHCostResult qhcr, decimal dMargin, decimal marketProfitLoss,
                                                              ReckoningTransaction tm, Types.DealRptType dealRptType)
        {
            string result = string.Empty;
            //成交回报实体
            var qhDealrpt = new QH_TodayTradeTableInfo();

            //xhDealrpt.TradeNumber = this.BuildXHDealOrderNo();
            qhDealrpt.TradeNumber = sdbe.Id; //不再自己构建id，使用成交回报的id，一一对应
            //成交时间
            qhDealrpt.TradeTime = sdbe.DealTime;
            //成交价
            qhDealrpt.TradePrice = sdbe.DealPrice;
            //成交量
            qhDealrpt.TradeAmount = Convert.ToInt32(sdbe.DealAmount);
            //成交单位
            qhDealrpt.TradeUnitId = tet.TradeUnitId;
            //股东代码
            qhDealrpt.TradeAccount = tet.TradeAccount;
            //资金帐户
            qhDealrpt.CapitalAccount = tet.CapitalAccount;
            //成交回报类型
            // qhDealrpt.TradeTypeId = (int)Types.DealRptType.DRTDealed;
            qhDealrpt.TradeTypeId = (int)dealRptType;
            //现货名称
            qhDealrpt.ContractCode = tet.ContractCode;
            //现货代码
            //qhDealrpt = tet.ContractName;
            //交易手续费
            qhDealrpt.TradeProceduresFee = qhcr.Cosing;
            //保证金
            //qhDealrpt.Margin = 0; // dMargin;
            //update date 2009-12-03 不管开平仓都记录保证金
            //if (tet.OpenCloseTypeId == (int) Types.FutureOpenCloseType.OpenPosition)
            qhDealrpt.Margin = dMargin;
            //=============

            //委托价格
            qhDealrpt.EntrustPrice = tet.EntrustPrice;
            //委托单号
            qhDealrpt.EntrustNumber = tet.EntrustNumber;
            //投组标识
            qhDealrpt.PortfolioLogo = tet.PortfolioLogo;
            //货币类型
            qhDealrpt.CurrencyTypeId = tet.CurrencyTypeId;

            qhDealrpt.BuySellTypeId = tet.BuySellTypeId;

            qhDealrpt.OpenCloseTypeId = tet.OpenCloseTypeId;

            //add 李健华 增加每笔盯市盈亏
            qhDealrpt.MarketProfitLoss = marketProfitLoss;
            //==========

            QH_TodayTradeTableDal qhTodayTradeTableDal = new QH_TodayTradeTableDal();

            if (qhTodayTradeTableDal.Exists(qhDealrpt.TradeNumber))
            {
                string format = "BuildGZQHDealRpt数据库已经存在TradeNumber={0}";
                string desc = string.Format(format, qhDealrpt.TradeNumber);
                LogHelper.WriteDebug(desc);

                return null;
            }

            try
            {
                qhTodayTradeTableDal.Add(qhDealrpt, tm);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                qhDealrpt = null;
            }

            return qhDealrpt;
        }

        /// <summary>
        /// 构造股指期货委托单
        /// </summary>
        public static string BuildGZQHOrder(ref QH_TodayEntrustTableInfo order,
                                            StockIndexFuturesOrderRequest originalOrder,
                                            string strHoldingAccount, string strCapitalAccount,
                                            int iCurType, ref string strMessage)
        {
            //bool result = false;
            order = new QH_TodayEntrustTableInfo();

            order.EntrustNumber = BuildQHOrderNo();
            order.EntrustAmount = Convert.ToInt32(originalOrder.OrderAmount);
            order.EntrustPrice = Convert.ToDecimal(originalOrder.OrderPrice);
            order.EntrustTime = DateTime.Now;
            order.OfferTime = DateTime.Now;
            order.IsMarketValue = originalOrder.OrderWay == Types.OrderPriceType.OPTMarketPrice
                                      ? true
                                      : false;
            order.BuySellTypeId = (int)originalOrder.BuySell;
            order.OrderStatusId = (int)Types.OrderStateType.DOSUnRequired;
            if (strHoldingAccount == null)
            {
                strHoldingAccount = "";
            }
            order.TradeAccount = strHoldingAccount;
            if (strCapitalAccount == null)
            {
                strCapitalAccount = "";
            }
            order.CapitalAccount = strCapitalAccount;
            if (originalOrder.PortfoliosId == null)
            {
                originalOrder.PortfoliosId = "";
            }
            order.PortfolioLogo = originalOrder.PortfoliosId;
            if (originalOrder.Code == null)
            {
                originalOrder.Code = "";
            }
            order.ContractCode = originalOrder.Code;
            order.TradeAmount = 0;
            order.TradeAveragePrice = 0;
            order.CancelAmount = 0;
            order.CancelLogo = true;
            if (originalOrder.ChannelID == null)
            {
                originalOrder.ChannelID = "";
            }
            order.CallbackChannelId = originalOrder.ChannelID;
            order.IsMarketValue = originalOrder.OrderWay == Types.OrderPriceType.OPTMarketPrice
                                      ? true
                                      : false;
            order.OpenCloseTypeId = (int)originalOrder.OpenCloseType;
            order.TradeUnitId = (int)originalOrder.OrderUnitType;
            order.CurrencyTypeId = iCurType;

            order.OrderMessage = "";
            order.McOrderId = "";

            CheckEntrustLength(order);

            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            dal.Add(order);

            return order.EntrustNumber;
        }
        /// <summary>
        /// 检查是否有超过数据库限制长度的字段
        /// </summary>
        /// <param name="tet"></param>
        private static void CheckEntrustLength(QH_TodayEntrustTableInfo tet)
        {
            if (tet.PortfolioLogo.Length > 25)
            {
                string format1 = "CheckEntrustLength[PortfolioLogo={0}]";
                string desc1 = string.Format(format1, tet.PortfolioLogo);
                LogHelper.WriteDebug(desc1);
                tet.PortfolioLogo = tet.PortfolioLogo.Substring(0, 25);
            }

            if (tet.TradeAccount.Length > 20)
            {
                string format1 = "CheckEntrustLength[StockAccount={0}]";
                string desc1 = string.Format(format1, tet.TradeAccount);
                LogHelper.WriteDebug(desc1);
                tet.TradeAccount = tet.TradeAccount.Substring(0, 20);
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

            if (tet.CallbackChannelId.Length > 50)
            {
                string format1 = "CheckEntrustLength[CallbackChannlId={0}]";
                string desc1 = string.Format(format1, tet.CallbackChannelId);
                LogHelper.WriteDebug(desc1);
                tet.CallbackChannelId = tet.CallbackChannelId.Substring(0, 50);
            }

            if (tet.McOrderId.Length > 100)
            {
                string format1 = "CheckEntrustLength[McOrderId={0}]";
                string desc1 = string.Format(format1, tet.McOrderId);
                LogHelper.WriteDebug(desc1);
                tet.McOrderId = tet.McOrderId.Substring(0, 100);
            }
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
            QH_CapitalAccountFreezeTableInfo caf = new QH_CapitalAccountFreezeTableInfo();
            caf.CapitalAccountLogo = capitalAccountId;
            caf.EntrustNumber = entrustNumber;
            caf.FreezeTypeLogo = (int)Types.FreezeType.DelegateFreeze;
            caf.FreezeTime = DateTime.Now;
            caf.FreezeAmount = 0;
            caf.FreezeCost = 0;

            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            return dal.Add(caf);
        }

        #endregion

        /// <summary>
        /// 获取持仓内存表，如果内存中不存在，那么再到数据库查找
        /// </summary>
        /// <param name="holdAccount">持仓账户</param>
        /// <param name="code">代码</param>
        /// <param name="tradeCurrencyType">币种</param>
        /// <param name="buySellType">买卖方向</param>
        /// <returns>内存表</returns>
        public static QHHoldMemoryTable GetHoldMemoryTable(string holdAccount, string code, int tradeCurrencyType, int buySellType)
        {
            QHHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.QHHoldMemoryList.GetByHoldAccountAndCurrencyType(holdAccount, code,
                                                                                                buySellType,
                                                                                                tradeCurrencyType);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
                    var hold = dal.GetQhAccountHoldTable(holdAccount, code, tradeCurrencyType, buySellType);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        MemoryDataManager.QHHoldMemoryList.AddQHHoldAccountTableToMemeory(hold);
                    }
                    else
                    {
                        //如果数据库也没有，那么代表无持仓
                        return null;
                    }

                    holdMemory = MemoryDataManager.QHHoldMemoryList.GetByHoldAccountAndCurrencyType(holdAccount, code,
                                                                                                    buySellType,
                                                                                                    tradeCurrencyType);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("QHCommonLogic.GetHoldMemoryTable-" + ex.Message, ex);
            }

            return holdMemory;
        }

        /// <summary>
        /// 获取持仓内存表，如果内存中不存在，那么再到数据库查找
        /// </summary>
        /// <param name="accountHoldLogoId">持仓id</param>
        /// <returns></returns>
        public static QHHoldMemoryTable GetHoldMemoryTable(int accountHoldLogoId)
        {
            QHHoldMemoryTable holdMemory = null;
            try
            {
                holdMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(accountHoldLogoId);
                //如果持仓为空，那么先从数据库加载，看是不是内存表没有加载
                if (holdMemory == null)
                {
                    QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
                    var hold = dal.GetModel(accountHoldLogoId);
                    //如果数据库有，那么直接加载到内存表中
                    if (hold != null)
                    {
                        MemoryDataManager.QHHoldMemoryList.AddQHHoldAccountTableToMemeory(hold);
                    }
                    else
                    {
                        //如果数据库也没有，那么代表无持仓
                        return null;
                    }

                    holdMemory = MemoryDataManager.QHHoldMemoryList.GetByAccountHoldLogoId(accountHoldLogoId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("QHCommonLogic.GetHoldMemoryTable-" + ex.Message, ex);
            }

            return holdMemory;
        }


        /// <summary>
        /// 商品期货实体转换为股指期货实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static StockIndexFuturesOrderRequest SPQHEntryConversionGZQHEntry(MercantileFuturesOrderRequest model)
        {
            StockIndexFuturesOrderRequest futRequest = new StockIndexFuturesOrderRequest();
            futRequest.BuySell = model.BuySell;
            futRequest.ChannelID = model.ChannelID;
            futRequest.Code = model.Code;
            futRequest.FundAccountId = model.FundAccountId;
            futRequest.OpenCloseType = model.OpenCloseType;
            futRequest.OrderAmount = model.OrderAmount;
            futRequest.OrderPrice = model.OrderPrice;
            futRequest.OrderUnitType = model.OrderUnitType;
            futRequest.OrderWay = model.OrderWay;
            futRequest.PortfoliosId = model.PortfoliosId;
            futRequest.TraderId = model.TraderId;
            futRequest.TraderPassword = model.TraderPassword;
            return futRequest;
        }

        /// <summary>
        /// 商品期货成交实体转换为股指期货成交实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static FutureDealBackEntity SPQHDealEntryConversionGZQHDealEntry(CommoditiesDealBackEntity model)
        {
            FutureDealBackEntity dealEntry = new FutureDealBackEntity();
            dealEntry.DealAmount = model.DealAmount;
            dealEntry.DealPrice = model.DealPrice;
            dealEntry.DealTime = model.DealTime;
            dealEntry.Id = model.Id;
            dealEntry.OrderNo = model.OrderNo;

            return dealEntry;
        }

    }
}