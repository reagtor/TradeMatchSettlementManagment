using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL;
using ReckoningCounter.Model;

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService
{
    class AdministratorFindService : IAdministratorFind
    {
        public ReckoningCounter.BLL.AdministratorFindTraderBLL AdministratorFindTraderBLL
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        
        #region IAdministratorFind 成员

        #region  管理员查询交易员银行资金明细情况
        /// <summary>
        ///  管理员查询交易员银行资金明细情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public List<UA_BankAccountTableInfo> AdminFindTraderBankCapital(string adminId, string adminPassword, string traderId, out string outMessage)
        {
            List<UA_BankAccountTableInfo> result = null;
            outMessage = string.Empty;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result=aa.AdminFindTraderBankCapital(admin, out outMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员现货当日委托情况
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
        public List<XH_TodayEntrustTableInfo> AdminFindTraderSpotTodayEntrust(string adminId, string adminPassword, string traderId,
                                                                          SpotEntrustConditionFindEntity findCondition,
                                                                          int start, int pageLength, out int count,
                                                                          ref string strErrorMessage)
        {
            List<XH_TodayEntrustTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderSpotTodayEntrust(admin,findCondition,
                                                                          start, pageLength, out  count,
                                                                          ref  strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员期货当日委托情况
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
        public List<QH_TodayEntrustTableInfo> AdminFindTraderFuturesTodayEntrust(string adminId, string adminPassword, string traderId,
                                                                             FuturesEntrustConditionFindEntity findCondition,
                                                                             int start, int pageLength, out int count,
                                                                             ref string strErrorMessage)
        {
            List<QH_TodayEntrustTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderFuturesTodayEntrust(admin,findCondition,start, pageLength, out count,
                                                                             ref  strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员现货历史委托情况
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
        public List<XH_HistoryEntrustTableInfo> AdminFindTraderSpotHistoryEntrust(string adminId, string adminPassword, string traderId,
                                                                              SpotEntrustConditionFindEntity findCondition,
                                                                              int start, int pageLength, out int count,
                                                                              ref string strErrorMessage)
        {
            List<XH_HistoryEntrustTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderSpotHistoryEntrust(admin,findCondition,
                                                                             start,pageLength, out count,
                                                                              ref strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员期货历史委托情况
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
        public List<QH_HistoryEntrustTableInfo> AdminFindTraderFuturesHistoryEntrust(string adminId, string adminPassword, string traderId,
                                                                                 FuturesEntrustConditionFindEntity
                                                                                     findCondition, int start, int pageLength,
                                                                                 out int count, ref string strErrorMessage)
        {
            List<QH_HistoryEntrustTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderFuturesHistoryEntrust(admin,findCondition,start,pageLength,
                                                                                 out  count, ref strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员现货当日成交情况
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
        public List<XH_TodayTradeTableInfo> AdminFindTraderSpotTodayTrade(string adminId, string adminPassword, string traderId,
                                                                      SpotTradeConditionFindEntity findCondition, int start,
                                                                      int pageLength, out int count, out string strErrorMessage)
        {
            List<XH_TodayTradeTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderSpotTodayTrade(admin,  findCondition, start,
                                                                      pageLength, out count, out strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员期货当日成交情况
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
        public List<QH_TodayTradeTableInfo> AdminFindTraderFuturesTodayTrade(string adminId, string adminPassword, string traderId,
                                                                         FuturesTradeConditionFindEntity findCondition,
                                                                         int start, int pageLength, out int count,
                                                                         out string strErrorMessage)
        {
            List<QH_TodayTradeTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderFuturesTodayTrade(admin,findCondition,start, pageLength, out  count,
                                                                         out  strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员现货历史成交情况
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
        public List<XH_HistoryTradeTableInfo> AdminFindTraderSpotHistoryTrade(string adminId, string adminPassword, string traderId,
                                                                          SpotTradeConditionFindEntity findCondition, int start,
                                                                          int pageLength, out int count,
                                                                          ref string strErrorMessage)
        {
            List<XH_HistoryTradeTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderSpotHistoryTrade(admin, findCondition,start,pageLength, out  count,
                                                                          ref strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员期货历史成交情况
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
        public List<QH_HistoryTradeTableInfo> AdminFindTraderFuturesHistoryTrade(string adminId, string adminPassword, string traderId,
                                                                             FuturesTradeConditionFindEntity findCondition,
                                                                             int start, int pageLength, out int count,
                                                                             ref string strErrorMessage)
        {
            List<QH_HistoryTradeTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderFuturesHistoryTrade(admin,findCondition,start, pageLength, out  count,
                                                                             ref strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员现货资金明细情况
        /// <summary>
        ///  管理员查询交易员现货资金明细情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        public List<SpotCapitalEntity> AdminFindTraderSpotCapital(string adminId, string adminPassword, string traderId, ref string strErrorMessage)
        {
            List<SpotCapitalEntity> result = null;
            strErrorMessage = string.Empty;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderSpotCapital(admin, ref  strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员期货资金明细情况
        /// <summary>
        ///  管理员查询交易员期货资金明细情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        public List<FuturesCapitalEntity> AdminFindTraderFuturesCapital(string adminId, string adminPassword, string traderId,
                                                                        ref string strErrorMessage)
        {
            List<FuturesCapitalEntity>  result = null;
            strErrorMessage = string.Empty;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderFuturesCapital(admin,ref strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员现货持仓情况
        /// <summary>
        ///  管理员查询交易员现货持仓情况
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
        public List<SpotHoldFindResultEntity> AdminFindTraderSpotHold(string adminId, string adminPassword, string traderId,
                                                                      SpotHoldConditionFindEntity findCondition, int start,
                                                                      int pageLength, out int count, ref string strErrorMessage)
        {
            List<SpotHoldFindResultEntity>  result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderSpotHold(admin,findCondition, start,
                                                                      pageLength, out count, ref strErrorMessage);
            return result;
        }

        #endregion

        #region 管理员查询交易员期货持仓情况
        /// <summary>
        ///  管理员查询交易员期货持仓情况
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
        public List<FuturesHoldFindResultEntity> AdminFindTraderFuturesHold(string adminId, string adminPassword, string traderId,
                                                                            FuturesHoldConditionFindEntity findCondition,
                                                                            int start, int pageLength, out int count,
                                                                            ref string strErrorMessage)
        {
            List<FuturesHoldFindResultEntity>  result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderFuturesHold(admin, findCondition,start, pageLength, out count,
                                                                            ref  strErrorMessage);
            return result;
        }
        #endregion

        #region 管理员查询交易员资产汇总情况
        /// <summary>
        ///  管理员查询交易员资产汇总情况
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public List<AssetSummaryEntity> AdminFindTraderAssetSummary(string adminId, string adminPassword, string traderId, out string outMessage)
        {
            List<AssetSummaryEntity>  result = null;
            outMessage= string.Empty;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderAssetSummary(admin, out outMessage);
            return result;
        }
        #endregion


        #region 管理员查询交易员各资金账户转账流水
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
        public List<UA_CapitalFlowTableInfo> AdminFindTraderCapitalAccountTransferFlow(string adminId, string adminPassword, string traderId, 
                                                                                    int start,int pageLength, out int count,out string strErrorMessage)
        {
            List<UA_CapitalFlowTableInfo> result = null;
            strErrorMessage = string.Empty;
            count = 0;
            AdministratorFindTraderBLL aa = new AdministratorFindTraderBLL();
            AdministratorFindEntity admin = new AdministratorFindEntity();
            admin.AdministratorID = adminId;
            admin.AdministratorPassword = adminPassword;
            admin.TraderID = traderId;
            result = aa.AdminFindTraderCapitalAccountTransferFlow(admin, start,pageLength, out count,out strErrorMessage);
            return result;
        }
        #endregion

      

        #endregion
    }
}