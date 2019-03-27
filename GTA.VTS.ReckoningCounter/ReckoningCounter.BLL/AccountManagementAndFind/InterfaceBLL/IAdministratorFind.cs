using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Model;

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL
{
    /// <summary>
    /// 作用：柜台管理员查询交易员委托和成交查询情况（包括： 现货委托查询、现货成交查询、期货委托查询、期货成交查询）
    ///       以及各资金账户明细和各持仓账户明细（包括： 银行资金明细查询、现货资金明细查询、 期货资金明细查询、现货持仓查询、 期货持仓查询）
    ///       和各资金账户的资金流水情况
    /// 作者：李科恒
    /// 日期：2008-11-17
    /// </summary>
    public interface IAdministratorFind
    {
        #region  (NEW)管理员查询交易员银行资金明细情况
        /// <summary>
        ///  管理员查询交易员银行资金明细情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        List<UA_BankAccountTableInfo> AdminFindTraderBankCapital(string adminId, string adminPassword, string traderId, out string outMessage);
        #endregion

        #region  (NEW)管理员查询交易员现货资金明细情况
        /// <summary>
        ///  管理员查询交易员现货资金明细情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<SpotCapitalEntity> AdminFindTraderSpotCapital(string adminId, string adminPassword, string traderId, ref string strErrorMessage);
        #endregion

        #region  (NEW)管理员查询交易员期货资金明细情况
        /// <summary>
        ///  管理员查询交易员期货资金明细情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<FuturesCapitalEntity> AdminFindTraderFuturesCapital(string adminId, string adminPassword, string traderId,
                                                                 ref string strErrorMessage);
        #endregion

        #region (NEW)管理员查询交易员现货当日委托情况
        /// <summary>
        /// 管理员查询交易员现货当日委托情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<XH_TodayEntrustTableInfo> AdminFindTraderSpotTodayEntrust(string adminId, string adminPassword, string traderId,
                                                                   SpotEntrustConditionFindEntity findCondition,
                                                                   int start, int pageLength, out int count,
                                                                   ref string strErrorMessage);
        #endregion

        #region (NEW)管理员查询交易员期货当日委托情况
        /// <summary>
        /// 管理员查询交易员期货当日委托情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<QH_TodayEntrustTableInfo> AdminFindTraderFuturesTodayEntrust(string adminId, string adminPassword, string traderId,
                                                                      FuturesEntrustConditionFindEntity findCondition,
                                                                      int start, int pageLength, out int count,
                                                                      ref string strErrorMessage);
        #endregion

        #region （NEW）管理员查询交易员现货历史委托情况
        /// <summary>
        ///  管理员查询交易员现货历史委托情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<XH_HistoryEntrustTableInfo> AdminFindTraderSpotHistoryEntrust(string adminId, string adminPassword, string traderId,
                                                                       SpotEntrustConditionFindEntity findCondition,
                                                                       int start, int pageLength, out int count,
                                                                       ref string strErrorMessage);
        #endregion

        #region （NEW）管理员查询交易员期货历史委托情况
        /// <summary>
        ///  管理员查询交易员期货历史委托情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<QH_HistoryEntrustTableInfo> AdminFindTraderFuturesHistoryEntrust(string adminId, string adminPassword, string traderId,
                                                                          FuturesEntrustConditionFindEntity
                                                                              findCondition, int start, int pageLength,
                                                                          out int count, ref string strErrorMessage);
        #endregion

        #region  (NEW)管理员查询交易员现货当日成交情况
        /// <summary>
        /// 管理员查询交易员现货当日成交情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<XH_TodayTradeTableInfo> AdminFindTraderSpotTodayTrade(string adminId, string adminPassword, string traderId,
                                                               SpotTradeConditionFindEntity findCondition, int start,
                                                               int pageLength, out int count, out string strErrorMessage);
        #endregion

        #region  (NEW)管理员查询交易员期货当日成交情况
        /// <summary>
        /// 管理员查询交易员期货当日成交情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<QH_TodayTradeTableInfo> AdminFindTraderFuturesTodayTrade(string adminId, string adminPassword, string traderId,
                                                                  FuturesTradeConditionFindEntity findCondition,
                                                                  int start, int pageLength, out int count,
                                                                  out string strErrorMessage);
        #endregion

        #region  (NEW)管理员查询交易员现货历史成交情况
        /// <summary>
        /// 管理员查询交易员现货历史成交情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<XH_HistoryTradeTableInfo> AdminFindTraderSpotHistoryTrade(string adminId, string adminPassword, string traderId,
                                                                   SpotTradeConditionFindEntity findCondition, int start,
                                                                   int pageLength, out int count,
                                                                   ref string strErrorMessage);
        #endregion

        #region  (NEW)管理员查询交易员期货历史成交情况
        /// <summary>
        /// 管理员查询交易员期货历史成交情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<QH_HistoryTradeTableInfo> AdminFindTraderFuturesHistoryTrade(string adminId, string adminPassword, string traderId,
                                                                      FuturesTradeConditionFindEntity findCondition,
                                                                      int start, int pageLength, out int count,
                                                                      ref string strErrorMessage);
        #endregion

        #region  （NEW）管理员查询交易员现货持仓情况
        /// <summary>
        ///  管理员查询交易员现货持仓情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<SpotHoldFindResultEntity> AdminFindTraderSpotHold(string adminId, string adminPassword, string traderId,
                                                               SpotHoldConditionFindEntity findCondition, int start,
                                                               int pageLength, out int count, ref string strErrorMessage);

        #endregion

        #region  （NEW）管理员查询交易员期货持仓情况
        /// <summary>
        ///  管理员查询交易员期货持仓情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<FuturesHoldFindResultEntity> AdminFindTraderFuturesHold(string adminId, string adminPassword, string traderId,
                                                                     FuturesHoldConditionFindEntity findCondition,
                                                                     int start, int pageLength, out int count,
                                                                     ref string strErrorMessage);
        #endregion

        #region  （NEW）管理员查询交易员资产汇总情况
        /// <summary>
        ///  管理员查询交易员资产汇总情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        List<AssetSummaryEntity> AdminFindTraderAssetSummary(string adminId, string adminPassword, string traderId, out string outMessage);
        #endregion

        #region  (NEW)管理员查询交易员各资金账户转账流水

        /// <summary>
        /// 管理员查询交易员各资金账户转账流水
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        List<UA_CapitalFlowTableInfo> AdminFindTraderCapitalAccountTransferFlow(string adminId, string adminPassword, string traderId, int start,
                                                                            int pageLength, out int count,
                                                                            out string strErrorMessage);

        #endregion
       

    }
}