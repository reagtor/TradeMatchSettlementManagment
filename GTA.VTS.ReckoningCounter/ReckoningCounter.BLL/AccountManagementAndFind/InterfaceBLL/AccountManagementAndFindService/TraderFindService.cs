#region Using Namespace

using System;
using System.Collections.Generic;
using System.ServiceModel;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.DAL.Data;

#endregion

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService
{    
    /// <summary>
    /// 提交给ROE交易员查询的方法服务
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TraderFindService : ITraderFind
    {
        #region  -------------------按交易员查询 提供给ROE使用---------------
        # region  银行资金明细查询(OK)

        /// <summary>
        ///  银行资金明细查询
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bankAccount">银行帐户</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public List<UA_BankAccountTableInfo> BankCapitalFind(string userId, string bankAccount, out string outMessage)
        {
            List<UA_BankAccountTableInfo> result = null;
            var aa = new CapitalAndHoldFindBLL();
            outMessage = string.Empty;
            result = aa.BankCapitalFind(userId, bankAccount, out outMessage);
            return result;
        }

        # endregion

        #region 现货资金情况查询(根据资金账号 朱亮---OK）

        /// <summary>
        /// 根据资金账号查询现货资金情况
        /// </summary>
        /// <param name="strFundAccountId"></param>
        /// <param name="currencyType"></param>
        /// <param name="userPassword"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public SpotCapitalEntity SpotCapitalFind(string strFundAccountId, Types.CurrencyType currencyType,
                                                 string userPassword, ref string strErrorMessage)
        {
            var capitalAndHoldFindBLL = new CapitalAndHoldFindBLL();
            return capitalAndHoldFindBLL.SpotCapitalFind(strFundAccountId, currencyType, userPassword,
                                                         ref strErrorMessage);
        }

        #endregion

        #region 期货资金明细查询（通过资金账号和币种---OK）

        public FuturesCapitalEntity FuturesCapitalFind(string strFundAccountId, Types.CurrencyType currencyType,
                                                       string userPassword, ref string strErrorMessage)
        {
            FuturesCapitalEntity result = null;
            var aa = new CapitalAndHoldFindBLL();
            result = aa.FuturesCapitalFind(strFundAccountId, currencyType, userPassword, ref strErrorMessage);
            return result;
        }

        #endregion

        #region 现货当日委托查询(根据资金账号 朱亮----OK）

        /// <summary>
        /// 根据资金账号查询现货当日委托
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="strPassword"></param>
        /// <param name="findCondition"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_TodayEntrustTableInfo> SpotTodayEntrustFindByXhCapitalAccount(string capitalAccount,
                                                                                 string strPassword,
                                                                                 SpotEntrustConditionFindEntity
                                                                                     findCondition, int start,
                                                                                 int pageLength, out int count,
                                                                                 ref string strErrorMessage)
        {

            if (findCondition == null)
            {
                count = 1;
                var list = new List<XH_TodayEntrustTableInfo>();
                try
                {
                    XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
                    //list = DataRepository.XH_TodayEntrustTableInfoProvider.GetByCapitalAccount(capitalAccount);
                    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "'");
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                return list;
            }

            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();

            return entrustAndTradeFindBLL.SpotTodayEntrustFindByXhCapitalAccount(capitalAccount, strPassword,
                                                                                 findCondition, start,
                                                                                 pageLength, out count,
                                                                                 ref strErrorMessage);
        }

        #endregion

        # region 期货当日委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号----OK）

        /// <summary>
        /// 期货当日委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_TodayEntrustTableInfo> FuturesTodayEntrustFindByQhCapitalAccount(string capitalAccount,
                                                                                    string strPassword,
                                                                                    FuturesEntrustConditionFindEntity
                                                                                        findCondition, int start,
                                                                                    int pageLength, out int count,
                                                                                    ref string strErrorMessage)
        {
            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();

            return entrustAndTradeFindBLL.FuturesTodayEntrustFindByQhCapitalAccount(capitalAccount, strPassword,
                                                                                    findCondition, start,
                                                                                    pageLength, out count,
                                                                                    ref strErrorMessage);
        }

        # endregion

        # region 现货当日成交查询（根据资金帐户进行查询）(OK)

        /// <summary>
        /// 现货当日成交查询（根据资金帐户进行查询）
        /// </summary>
        /// <param name="capitalAccount">资金帐户</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="strErrorMessage"></param>
        /// <param name="findCondition">查询条件实体的一个对象</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <returns></returns>
        public List<XH_TodayTradeTableInfo> SpotTodayTradeFindByCapitalAccount(string capitalAccount, string userPassword,
                                                                           int start, int pageLength, out int count,
                                                                           out string strErrorMessage,
                                                                           SpotTradeConditionFindEntity findCondition)
        {
            strErrorMessage = string.Empty;
            XH_TodayTradeTableDal dal = new XH_TodayTradeTableDal();
            if (findCondition == null)
            {
                count = 1;

                var list = new List<XH_TodayTradeTableInfo>();

                try
                {

                    list = dal.GetListArray(" CapitalAccount='" + capitalAccount + "' ");
                    //list = DataRepository.XH_TodayTradeTableInfoProvider.GetByCapitalAccount(capitalAccount);
                }
                catch (Exception ex)
                {
                    strErrorMessage = ex.Message;
                    LogHelper.WriteError(ex.Message, ex);
                }

                return list;
            }

            if (!String.IsNullOrEmpty(findCondition.EntrustNumber))
            {
                count = 1;

                var list = new List<XH_TodayTradeTableInfo>();

                try
                {
                    list = dal.GetListArray(" EntrustNumber='" + findCondition.EntrustNumber.Trim() + "' ");

                    //list = DataRepository.XH_TodayTradeTableInfoProvider.GetByEntrustNumber(findCondition.EntrustNumber);
                }
                catch (Exception ex)
                {
                    strErrorMessage = ex.Message;
                    LogHelper.WriteError(ex.Message, ex);
                }

                return list;
            }

            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();


            return entrustAndTradeFindBLL.SpotTodayTradeFindByXhCapitalAccount(capitalAccount, userPassword,
                                                                               findCondition, start,
                                                                               pageLength, out count,
                                                                               out strErrorMessage);
        }

        # endregion

        # region 期货当日成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号----OK）

        /// <summary>
        /// 期货当日成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_TodayTradeTableInfo> FuturesTodayTradeFindByXhCapitalAccount(string capitalAccount,
                                                                                string strPassword,
                                                                                FuturesTradeConditionFindEntity
                                                                                    findCondition, int start,
                                                                                int pageLength, out int count,
                                                                                out string strErrorMessage)
        {
            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();
            return entrustAndTradeFindBLL.FuturesTodayTradeFindByXhCapitalAccount(capitalAccount, strPassword,
                                                                                  findCondition, start,
                                                                                  pageLength, out count,
                                                                                  out strErrorMessage);
        }

        # endregion

        # region 现货历史委托查询(根据资金帐户进行查询）(OK)

        /// <summary>
        /// 现货历史委托查询(根据资金帐户进行查询）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage"></param>
        /// <param name="findCondition">查询条件实体的一个对象</param>
        /// <param name="userPassword"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <returns></returns>
        public List<XH_HistoryEntrustTableInfo> SpotHistoryEntrustFind(string capitalAccount, string userPassword, int start,
                                                                   int pageLength, out int count,
                                                                   out string strErrorMessage,
                                                                   SpotEntrustConditionFindEntity findCondition)
        {
            count = 0;
            strErrorMessage = string.Empty;
            List<XH_HistoryEntrustTableInfo> result = null;
            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();
            result = entrustAndTradeFindBLL.SpotHistoryEntrustFindByXhCapitalAccount(capitalAccount, userPassword,
                                                                                     findCondition, start, pageLength,
                                                                                     out count, ref strErrorMessage);
            return result;
        }

        # endregion

        # region  期货历史委托查询（根据资金账号、密码及查询条件来查询---OK）

        /// <summary> 
        /// 期货历史委托查询（根据资金账号、密码及查询条件来查询）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_HistoryEntrustTableInfo> FuturesHistoryEntrustFindByQhCapitalAccount(string capitalAccount,
                                                                                        string strPassword,
                                                                                        FuturesEntrustConditionFindEntity
                                                                                            findCondition, int start,
                                                                                        int pageLength, out int count,
                                                                                        ref string strErrorMessage)
        {
            count = 0;
            strErrorMessage = string.Empty;
            List<QH_HistoryEntrustTableInfo> result = null;
            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();
            result = entrustAndTradeFindBLL.FuturesHistoryEntrustFindByQhCapitalAccount(capitalAccount, strPassword,
                                                                                        findCondition, start, pageLength,
                                                                                        out count, ref strErrorMessage);
            return result;
        }

        # endregion

        # region 现货历史成交查询(根据资金帐户进行查询）(OK)

        /// <summary>
        /// 现货历史成交查询(根据资金帐户进行查询）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage"></param>
        /// <param name="findCondition">查询条件实体的一个对象</param>
        /// <param name="userPassword"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="findCondition">查询条件实体的一个对象</param>
        /// <returns></returns>
        public List<XH_HistoryTradeTableInfo> SpotHistoryTradeFind(string capitalAccount, string userPassword, int start,
                                                               int pageLength, out int count, out string strErrorMessage,
                                                               SpotTradeConditionFindEntity findCondition)
        {
            count = 0;
            strErrorMessage = string.Empty;
            List<XH_HistoryTradeTableInfo> result = null;
            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();
            result = entrustAndTradeFindBLL.SpotHistoryTradeFindByXhCapitalAccount(capitalAccount, userPassword,
                                                                                   findCondition, start, pageLength,
                                                                                   out count, ref strErrorMessage);
            ;
            return result;
        }

        # endregion

        # region 期货历史成交查询（根据资金账号、密码及查询条件来查询----OK）

        /// <summary>
        /// 期货历史成交查询（根据资金账号、密码及查询条件来查询）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_HistoryTradeTableInfo> FuturesHistoryTradeFindByXhCapitalAccount(string capitalAccount,
                                                                                    string strPassword,
                                                                                    FuturesTradeConditionFindEntity
                                                                                        findCondition, int start,
                                                                                    int pageLength, out int count,
                                                                                    ref string strErrorMessage)
        {
            var entrustAndTradeFindBLL = new EntrustAndTradeFindBLL();
            return entrustAndTradeFindBLL.FuturesHistoryTradeFindByQHCapitalAccount(capitalAccount, strPassword,
                                                                                    findCondition, start, pageLength,
                                                                                    out count, ref strErrorMessage);
        }

        # endregion

        # region  现货持仓查询 （根据资金账号、密码及查询条件来查询-----OK）

        /// <summary>
        ///  现货持仓查询 （根据资金账号、密码及查询条件来查询）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage"></param>
        /// <param name="findCondition"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public List<SpotHoldFindResultEntity> SpotHoldFind(string capitalAccount, string strPassword,
                                                           SpotHoldConditionFindEntity findCondition,
                                                           int start, int pageLength, out int count,
                                                           ref string strErrorMessage)
        {
            count = 0;
            strErrorMessage = string.Empty;

            List<SpotHoldFindResultEntity> result = null;
            var _CapitalAndHoldFind = new CapitalAndHoldFindBLL();
            result = _CapitalAndHoldFind.SpotHoldFind(capitalAccount, strPassword,
                                                      findCondition,
                                                      start, pageLength, out count, ref strErrorMessage);

            return result;
        }

        # endregion

        # region 期货持仓查询 （根据资金账号、密码及查询条件来查询-----OK）

        /// <summary>
        /// 期货持仓查询 （根据资金账号、密码及查询条件来查询）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<FuturesHoldFindResultEntity> FuturesHoldFind(string capitalAccount, string strPassword,
                                                                 FuturesHoldConditionFindEntity findCondition, int start,
                                                                 int pageLength, out int count,
                                                                 ref string strErrorMessage)
        {
            //List<QH_HoldAccountTableInfo> tempt = null;
            var result = new List<FuturesHoldFindResultEntity>();
            count = 0;
            var aa = new CapitalAndHoldFindBLL();
            result = aa.FuturesHoldFind(capitalAccount, strPassword,
                                        findCondition, start, pageLength, out count, ref strErrorMessage);
            return result;
        }

        # endregion 现货持仓查询

        # region 查询银行资金账户的转账流水情况(OK)

        /// <summary>
        /// 查询银行资金账户的转账流水情况
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> BankCapitalAccountTransferFlowFind(string userId, string capitalAccount,
                                                                            string userPassword, DateTime startTime,
                                                                            DateTime endTime, int start, int pageLength,
                                                                            out int count, out string strErrorMessage)
        {
            List<UA_CapitalFlowTableInfo> result = null;

            count = 0;
            strErrorMessage = string.Empty;

            var _CapitalAccountFlowFind = new CapitalAccountFlowFindBLL();
            result = _CapitalAccountFlowFind.CapitalAccountTransferFlowFind(userId, capitalAccount,
                                                                            userPassword, startTime, endTime, start,
                                                                            pageLength,
                                                                            out count, out strErrorMessage);
            return result;
        }

        # endregion

        # region 查询现货资金账户的转账流水情况(OK)

        /// <summary>
        /// 查询现货资金账户的转账流水情况
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> SpotCapitalAccountTransferFlowFind(string userId, string capitalAccount,
                                                                            string userPassword, DateTime startTime,
                                                                            DateTime endTime, int start, int pageLength,
                                                                            out int count, out string strErrorMessage)
        {
            List<UA_CapitalFlowTableInfo> result = null;

            count = 0;
            strErrorMessage = string.Empty;

            var _CapitalAccountFlowFind = new CapitalAccountFlowFindBLL();
            result = _CapitalAccountFlowFind.CapitalAccountTransferFlowFind(userId, capitalAccount,
                                                                            userPassword, startTime, endTime, start,
                                                                            pageLength,
                                                                            out count, out strErrorMessage);
            return result;
        }

        # endregion

        # region 查询期货资金账户的转账流水情况(OK)

        /// <summary>
        /// 查询期货资金账户的转账流水情况
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> FuturesCapitalAccountTransferFlowFind(string userId, string capitalAccount,
                                                                               string userPassword, DateTime startTime,
                                                                               DateTime endTime, int start,
                                                                               int pageLength,
                                                                               out int count, out string strErrorMessage)
        {
            List<UA_CapitalFlowTableInfo> result = null;

            count = 0;
            strErrorMessage = string.Empty;

            var _CapitalAccountFlowFind = new CapitalAccountFlowFindBLL();
            result = _CapitalAccountFlowFind.CapitalAccountTransferFlowFind(userId, capitalAccount,
                                                                            userPassword, startTime, endTime, start,
                                                                            pageLength,
                                                                            out count, out strErrorMessage);
            return result;
        }

        # endregion

        # region 交易员的几个资金账户之间同币种两两自由转账(OK)

        /// <summary>
        /// 交易员的几个资金账户之间两两自由转账（同币种）
        /// </summary>
        /// <param name="freeTransfer"></param>
        /// <param name="currencyType"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public bool TwoAccountsFreeTransferFunds(FreeTransferEntity freeTransfer, Types.CurrencyType currencyType,
                                                 out string outMessage)
        {
            outMessage = string.Empty;
            //转帐是否成功
            bool _transferIsSuccessful = false;
            var aa = new CapitalManagementBLL();
            _transferIsSuccessful = aa.TwoAccountsFreeTransferFunds(freeTransfer, currencyType, out outMessage);
            return _transferIsSuccessful;
        }

        # endregion 交易员的几个资金账户之间同币种两两自由转账

        # region  资产汇总查询（根据交易员ID及密码查询）

        /// <summary>
        /// 资产汇总查询（根据交易员ID及密码查询）
        /// </summary>
        /// <param name="password"></param>
        /// <param name="outMessage"></param>
        /// <param name="traderId"></param>
        /// <returns></returns>
        public List<AssetSummaryEntity> AssetSummaryFind(string traderId, string password, out string outMessage)
        {
            outMessage = string.Empty;
            List<AssetSummaryEntity> result = null;
            var aa = new CapitalAndHoldFindBLL();
            var findAccount = new FindAccountEntity();
            findAccount.UserID = traderId;
            findAccount.UserPassword = password;
            result = aa.AssetSummaryFind(findAccount, out outMessage);
            return result;
        }

        # endregion
        #endregion  ----------------------------------

        #region -------------------按交易员查询 之前都是提供给前台使用，这里不再使用---------------

        //#region 银行资金明细查询
        ///// <summary>银行资金明细查询
        /////  银行资金明细查询
        ///// </summary>
        ///// <param name="userId">用户ID</param>
        ///// <param name="outMessage">输出信息</param>
        ///// <returns></returns>
        //public List<UA_BankAccountTableInfo> BankCapitalFind2(string userId, out string outMessage)
        //{
        //    return BankCapitalFind(userId, "", out outMessage);
        //}
        //#endregion

        //#region 现货/期货资金情况查询

        //#region 现货资金情况查询
        ///// <summary>现货资金情况查询（根据交易员和币种进行查询）
        /////  现货资金情况查询（根据交易员和币种进行查询）
        ///// </summary>
        ///// <param name="userId">用户ID</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="currencyType">币种</param>
        ///// <param name="userPassword">用户密码</param>
        ///// <param name="strErrorMessage">输出信息</param>
        ///// <returns></returns>
        //public SpotCapitalEntity SpotCapitalFind2(string userId, int AccountType, Types.CurrencyType currencyType,
        //                                          string userPassword, ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 1, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null) return null;
        //    return SpotCapitalFind(_AccountPair.CapitalAccount.UserAccountDistributeLogo, currencyType, userPassword,
        //                           ref strErrorMessage);
        //    //return null;
        //}
        //#endregion

        //#region 期货资金情况查询
        ///// <summary>期货资金情况查询
        ///// 期货资金情况查询
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="currencyType"></param>
        ///// <param name="userPassword"></param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //public FuturesCapitalEntity FuturesCapitalFind2(string userId, int AccountType, Types.CurrencyType currencyType,
        //                                                string userPassword, ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 0, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null) return null;
        //    return FuturesCapitalFind(_AccountPair.CapitalAccount.UserAccountDistributeLogo, currencyType, userPassword,
        //                              ref strErrorMessage);
        //}
        //#endregion
        //#endregion

        //#region 现货/期货持仓情况查询

        //#region 现货持仓查询
        ///// <summary>现货持仓查询
        /////  现货持仓查询
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strErrorMessage"></param>
        ///// <param name="strPassword"></param>
        ///// <param name="findCondition"></param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <returns></returns>
        //public List<SpotHoldFindResultEntity> SpotHoldFind2(string userId, int AccountType, string userPassword,
        //                                                    SpotHoldConditionFindEntity findCondition, int start,
        //                                                    int pageLength, out int count, ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 1, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return SpotHoldFind(_AccountPair.CapitalAccount.UserAccountDistributeLogo, userPassword, findCondition,
        //                        start, pageLength, out count, ref strErrorMessage);
        //}
        //#endregion

        //#region 现货持仓查询
        ///// <summary>
        ///// 期货持仓查询 （根据交易员、密码及查询条件来查询）
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strPassword">交易员密码</param>
        ///// <param name="findCondition">查询条件实体对象</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //public List<FuturesHoldFindResultEntity> FuturesHoldFind2(string userId, int AccountType, string userPassword,
        //                                                          FuturesHoldConditionFindEntity findCondition, int start,
        //                                                          int pageLength, out int count, ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 0, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return FuturesHoldFind(_AccountPair.CapitalAccount.UserAccountDistributeLogo, userPassword, findCondition,
        //                           start, pageLength, out count, ref strErrorMessage);
        //}
        //#endregion

        //#endregion

        //#region 现货委托/成交查询

        //#region 现货今日/历史委托查询
        ///// <summary>现货当日委托查询（根据交易员进行查询）
        ///// 现货当日委托查询（根据交易员进行查询）
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strPassword"></param>
        ///// <param name="findCondition"></param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //public List<XH_TodayEntrustTableInfo> SpotTodayEntrustFindByXhCapitalAccount2(string userId, int AccountType,
        //                                                                          string strPassword,
        //                                                                          SpotEntrustConditionFindEntity
        //                                                                              findCondition, int start,
        //                                                                          int pageLength, out int count,
        //                                                                          ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 1, ref strPassword);
        //    strErrorMessage = strPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return SpotTodayEntrustFindByXhCapitalAccount(_AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                                  strPassword, findCondition, start, pageLength, out count,
        //                                                  ref strErrorMessage);
        //}
        ///// <summary>现货历史委托查询
        ///// 现货历史委托查询
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strErrorMessage"></param>
        ///// <param name="findCondition">查询条件实体的一个对象</param>
        ///// <param name="userPassword"></param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="findCondition">查询条件实体的一个对象</param>
        ///// <returns></returns>
        //public List<XH_HistoryEntrustTableInfo> SpotHistoryEntrustFind2(string userId, int AccountType, string userPassword,
        //                                                            int start, int pageLength, out int count,
        //                                                            out string strErrorMessage,
        //                                                            SpotEntrustConditionFindEntity findCondition)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 1, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return SpotHistoryEntrustFind(_AccountPair.CapitalAccount.UserAccountDistributeLogo, userPassword, start,
        //                                  pageLength, out count, out strErrorMessage, findCondition);
        //}
        //#endregion

        //#region 现货今日/历史成交查询
        ///// <summary>现货当日成交查询
        ///// 现货当日成交查询（根据交易员进行查询）
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="userPassword">用户密码</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="strErrorMessage"></param>
        ///// <param name="findCondition">查询条件实体的一个对象</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <returns></returns>
        //public List<XH_TodayTradeTableInfo> SpotTodayTradeFindByCapitalAccount2(string userId, int AccountType,
        //                                                                    string userPassword, int start,
        //                                                                    int pageLength, out int count,
        //                                                                    out string strErrorMessage,
        //                                                                    SpotTradeConditionFindEntity findCondition)
        //{
        //    strErrorMessage = string.Empty;

        //    if (!String.IsNullOrEmpty(findCondition.EntrustNumber))
        //    {
        //        count = 1;

        //        var list = new List<XH_TodayTradeTableInfo>();

        //        try
        //        {
        //            XH_TodayTradeTableDal dal = new XH_TodayTradeTableDal();
        //            list = dal.GetListArray(" EntrustNumber='" + findCondition.EntrustNumber.Trim() + "' ");
        //            // list = DataRepository.XH_TodayTradeTableInfoProvider.GetByEntrustNumber(findCondition.EntrustNumber);
        //        }
        //        catch (Exception ex)
        //        {
        //            strErrorMessage = ex.Message;
        //            LogHelper.WriteError(ex.Message, ex);
        //        }

        //        return list;
        //    }

        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 1, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return SpotTodayTradeFindByCapitalAccount(_AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                              userPassword, start, pageLength, out count, out strErrorMessage,
        //                                              findCondition);
        //}

        ///// <summary>现货历史成交查询
        ///// 现货历史成交查询
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strErrorMessage"></param>
        ///// <param name="findCondition">查询条件实体的一个对象</param>
        ///// <param name="userPassword"></param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="findCondition">查询条件实体的一个对象</param>
        ///// <returns></returns>
        //public List<XH_HistoryTradeTableInfo> SpotHistoryTradeFind2(string userId, int AccountType, string userPassword,
        //                                                        int start, int pageLength, out int count,
        //                                                        out string strErrorMessage,
        //                                                        SpotTradeConditionFindEntity findCondition)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 1, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return SpotHistoryTradeFind(_AccountPair.CapitalAccount.UserAccountDistributeLogo, userPassword, start,
        //                                pageLength, out count, out strErrorMessage, findCondition);
        //}
        //#endregion

        //#endregion

        //#region 期货委托/成交查询

        //#region 期货今日/历史委托查询
        ///// <summary>
        ///// 期货当日委托查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strPassword">交易员密码</param>
        ///// <param name="findCondition">查询条件</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //public List<QH_TodayEntrustTableInfo> FuturesTodayEntrustFindByQhCapitalAccount2(string userId, int AccountType,
        //                                                                             string strPassword,
        //                                                                             FuturesEntrustConditionFindEntity
        //                                                                                 findCondition, int start,
        //                                                                             int pageLength, out int count,
        //                                                                             ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 0, ref strPassword);
        //    strErrorMessage = strPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return FuturesTodayEntrustFindByQhCapitalAccount(_AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                                     strPassword, findCondition, start, pageLength, out count,
        //                                                     ref strErrorMessage);
        //}
        ///// <summary> 期货历史委托查询
        ///// 期货历史委托查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strPassword">交易员密码</param>
        ///// <param name="findCondition">查询条件</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //public List<QH_HistoryEntrustTableInfo> FuturesHistoryEntrustFindByQhCapitalAccount2(string userId, int AccountType,
        //                                                                                 string userPassword,
        //                                                                                 FuturesEntrustConditionFindEntity
        //                                                                                     findCondition, int start,
        //                                                                                 int pageLength, out int count,
        //                                                                                 ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 0, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return FuturesHistoryEntrustFindByQhCapitalAccount(_AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                                       userPassword, findCondition, start, pageLength, out count,
        //                                                       ref strErrorMessage);
        //}
        //#endregion

        //#region 期货今日/历史成交查询
        ///// <summary>
        ///// 期货当日成交查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strPassword">交易员密码</param>
        ///// <param name="findCondition">查询条件实体对象</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //public List<QH_TodayTradeTableInfo> FuturesTodayTradeFindByXhCapitalAccount2(string userId, int AccountType, string userPassword,
        //                                                                         FuturesTradeConditionFindEntity findCondition, int start,
        //                                                                         int pageLength, out int count, out string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 0, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return FuturesTodayTradeFindByXhCapitalAccount(_AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                                   userPassword, findCondition, start, pageLength, out count,
        //                                                   out strErrorMessage);
        //}

        ///// <summary>
        ///// 期货历史成交查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// </summary>
        ///// <param name="userId">交易员</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strPassword">交易员密码</param>
        ///// <param name="findCondition">查询条件实体对象</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //public List<QH_HistoryTradeTableInfo> FuturesHistoryTradeFindByXhCapitalAccount2(string userId, int AccountType, string userPassword,
        //                                                                             FuturesTradeConditionFindEntity findCondition, int start,
        //                                                                             int pageLength, out int count, ref string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 0, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return FuturesHistoryTradeFindByXhCapitalAccount(_AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                                     userPassword, findCondition, start, pageLength, out count,
        //                                                     ref strErrorMessage);
        //}
        //#endregion

        //#endregion

        //#region 查询转账流水
        ///// <summary>查询银行资金账户的转账流水情况
        ///// 查询银行资金账户的转账流水情况
        ///// </summary>
        ///// <param name="userId">交易员ID</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strErrorMessage">输出信息</param>
        ///// <param name="userPassword">用户密码</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="startTime">开始时间</param>
        ///// <param name="endTime">结束时间</param>
        ///// <returns></returns>
        //public List<UA_CapitalFlowTableInfo> BankCapitalAccountTransferFlowFind2(string userId, int AccountType, string userPassword,
        //                                                                        DateTime startTime, DateTime endTime, int start, int pageLength,
        //                                                                     out int count, out string strErrorMessage)
        //{
        //    //AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 3, ref userPassword);
        //    //strErrorMessage = userPassword;
        //    //if (_AccountPair == null) { count = 0; return null; }

        //    //========================================modify  by  xiongxl===============================
        //    #region update 2009-07-08 李健华
        //    // string backaccount = CommonDataAgent.GetBackAccount(userId, userPassword, out strErrorMessage, AccountType);
        //    string backaccount = CommonDataAgent.GetUserAccountByType(userId, userPassword, out strErrorMessage, GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount);
        //    #endregion
        //    if (backaccount == string.Empty)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return BankCapitalAccountTransferFlowFind(userId, backaccount, userPassword, startTime, endTime, start,
        //                                              pageLength, out count, out strErrorMessage);
        //    //===========================================end============================================
        //}

        ///// <summary>查询现货某一个资金账户的转账流水情况
        /////  查询现货某一个资金账户的转账流水情况
        ///// </summary>
        ///// <param name="userId">交易员ID</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strErrorMessage">输出信息</param>
        ///// <param name="userPassword">用户密码</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="startTime">开始时间</param>
        ///// <param name="endTime">结束时间</param>
        ///// <returns></returns>
        //public List<UA_CapitalFlowTableInfo> SpotCapitalAccountTransferFlowFind2(string userId, int AccountType, string userPassword, DateTime startTime,
        //                                                                     DateTime endTime, int start, int pageLength,
        //                                                                     out int count, out string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 1, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return SpotCapitalAccountTransferFlowFind(userId, _AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                              userPassword, startTime, endTime, start, pageLength, out count,
        //                                              out strErrorMessage);
        //}

        ///// <summary>
        ///// 查询期货某一个资金账户的转账流水情况
        ///// </summary>
        ///// <param name="userId">交易员ID</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="strErrorMessage">输出信息</param>
        ///// <param name="userPassword">用户密码</param>
        ///// <param name="start">记录集页索引</param>
        ///// <param name="pageLength">页长</param>
        ///// <param name="count">查到的记录数</param>
        ///// <param name="startTime">开始时间</param>
        ///// <param name="endTime">结束时间</param>
        ///// <returns></returns>
        //public List<UA_CapitalFlowTableInfo> FuturesCapitalAccountTransferFlowFind2(string userId, int AccountType, string userPassword, DateTime startTime,
        //                                                                        DateTime endTime, int start, int pageLength,
        //                                                                        out int count, out string strErrorMessage)
        //{
        //    AccountPair _AccountPair = CommonDataAgent.GeyAccountPair(userId, AccountType, 0, ref userPassword);
        //    strErrorMessage = userPassword;
        //    if (_AccountPair == null)
        //    {
        //        count = 0;
        //        return null;
        //    }
        //    return FuturesCapitalAccountTransferFlowFind(userId, _AccountPair.CapitalAccount.UserAccountDistributeLogo,
        //                                                 userPassword, startTime, endTime, start, pageLength, out count,
        //                                                 out strErrorMessage);
        //}
        //#endregion

        //#region 查询现货某一个资金账户可交易某只股票的最大数量
        ///// <summary>
        ///// 查询现货某一个资金账户可交易某只股票的最大数量
        ///// </summary>
        ///// <param name="userId">交易员ID</param>
        ///// <param name="userPassword">用户密码</param>
        ///// <param name="stockCode">股票代码</param>
        ///// <returns></returns>
        //public int SpotCapitalTradeAmountFind(string userId, string userPassword, string stockCode)
        //{
        //    //TODO:查询现货某一个资金账户可交易某只股票的最大数量
        //    return 100000000;
        //}
        //#endregion

        #endregion


    }
}