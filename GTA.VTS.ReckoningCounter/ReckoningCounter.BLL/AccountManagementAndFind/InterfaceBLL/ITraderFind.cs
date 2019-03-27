using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.Entity.Model.QueryFilter;

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL
{
    /// <summary>
    /// 交易员查询接口
    /// 
    ///090203 修改:虚拟交易前台的调用基本采用第二个重载函数(形如xxx2()）
    ///Update by:李健华
    ///Update Date:2009-10-19
    ///Desc.:删除无用的已经注释掉的接口定义
    /// </summary>
    [ServiceContract]
    public interface ITraderFind
    {
        #region  ------------------- 按交易员查询  提供给ROE使用---------------
        # region  银行资金明细查询
        /// <summary>
        ///  银行资金明细查询
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bankAccount">银行账号</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        [OperationContract]
        List<UA_BankAccountTableInfo> BankCapitalFind(string userId, string bankAccount, out string outMessage);
        # endregion

        # region 现货资金明细查询（根据现货资金账号和币种进行查询）
        /// <summary>
        ///  现货资金明细查询（根据现货资金账号和币种进行查询）
        /// </summary>
        /// <param name="strFundAccountId">用户ID</param>
        /// <param name="currencyType">币种</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        [OperationContract]
        SpotCapitalEntity SpotCapitalFind(string strFundAccountId, Types.CurrencyType currencyType, string userPassword,
                                          ref string strErrorMessage);
        # endregion

        #region 期货资金明细查询（通过资金账号和币种）
        /// <summary>
        /// 期货资金明细查询
        /// </summary>
        /// <param name="strFundAccountId"></param>
        /// <param name="currencyType"></param>
        /// <param name="userPassword"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        [OperationContract]
        FuturesCapitalEntity FuturesCapitalFind(string strFundAccountId, Types.CurrencyType currencyType,
                                                string userPassword, ref string strErrorMessage);
        #endregion

        # region 现货当日委托查询（根据现货资金账号进行查询）
        /// <summary>
        /// 现货当日委托查询（根据现货资金账号进行查询）
        /// </summary>
        /// <param name="capitalAccount"></param>
        /// <param name="strPassword"></param>
        /// <param name="findCondition"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        [OperationContract]
        List<XH_TodayEntrustTableInfo> SpotTodayEntrustFindByXhCapitalAccount(string capitalAccount, string strPassword,
                                                                          SpotEntrustConditionFindEntity findCondition,
                                                                          int start, int pageLength, out int count,
                                                                        ref string strErrorMessage);
        # endregion

        # region 期货当日委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
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
        [OperationContract]
        List<QH_TodayEntrustTableInfo> FuturesTodayEntrustFindByQhCapitalAccount(string capitalAccount,
                                                                                    string strPassword,
                                                                                    FuturesEntrustConditionFindEntity
                                                                                        findCondition, int start,
                                                                                    int pageLength, out int count,
                                                                                    ref string strErrorMessage);
        # endregion

        # region 现货当日成交查询（根据资金帐户进行查询）

        /// <summary>
        /// 现货当日成交查询（根据资金帐户进行查询）
        /// </summary>
        /// <param name="capitalAccount">资金帐户</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <param name="findCondition">查询条件实体的一个对象</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_TodayTradeTableInfo> SpotTodayTradeFindByCapitalAccount(string capitalAccount, string userPassword,
                                                                    int start, int pageLength, out int count,
                                                                    out string strErrorMessage,
                                                                    SpotTradeConditionFindEntity findCondition);
        # endregion

        # region 期货当日成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）

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
        [OperationContract]
        List<QH_TodayTradeTableInfo> FuturesTodayTradeFindByXhCapitalAccount(string capitalAccount, string strPassword,
                                                                         FuturesTradeConditionFindEntity findCondition,
                                                                         int start, int pageLength, out int count,
                                                                         out string strErrorMessage);


        # endregion 现货当日成交查询

        # region 现货历史委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 现货历史委托查询
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
        [OperationContract]
        List<XH_HistoryEntrustTableInfo> SpotHistoryEntrustFind(string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, SpotEntrustConditionFindEntity findCondition);
        # endregion

        # region  期货历史委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary> 
        /// 期货历史委托查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HistoryEntrustTableInfo> FuturesHistoryEntrustFindByQhCapitalAccount(string capitalAccount,
                                                                                 string strPassword,
                                                                                 FuturesEntrustConditionFindEntity
                                                                                     findCondition, int start,
                                                                                 int pageLength, out int count,
                                                                                 ref string strErrorMessage);

        # endregion

        # region 现货历史成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 现货历史成交查询
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
        [OperationContract]
        List<XH_HistoryTradeTableInfo> SpotHistoryTradeFind(string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, SpotTradeConditionFindEntity findCondition);
        # endregion

        # region 期货历史成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 期货历史成交查询（根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HistoryTradeTableInfo> FuturesHistoryTradeFindByXhCapitalAccount(string capitalAccount, string strPassword,
                                                                             FuturesTradeConditionFindEntity
                                                                                 findCondition, int start,
                                                                             int pageLength, out int count,
                                                                             ref string strErrorMessage);


        # endregion

        # region  现货持仓查询（根据资金账号、密码及查询条件来查询）
        /// <summary>
        ///  现货持仓查询
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage"></param>
        /// <param name="strPassword"></param>
        /// <param name="findCondition"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <returns></returns>
        [OperationContract]
        List<SpotHoldFindResultEntity> SpotHoldFind(string capitalAccount, string strPassword,
                                                      SpotHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage);
        # endregion

        # region 期货持仓查询 （根据资金账号、密码及查询条件来查询）
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
        [OperationContract]
        List<FuturesHoldFindResultEntity> FuturesHoldFind(string capitalAccount, string strPassword,
                                                                 FuturesHoldConditionFindEntity findCondition, int start,
                                                                 int pageLength, out int count,
                                                                 ref string strErrorMessage);
        # endregion

        # region 查询银行资金账户的转账流水情况
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
        [OperationContract]
        List<UA_CapitalFlowTableInfo> BankCapitalAccountTransferFlowFind(string userId, string capitalAccount,
                                                                 string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength,
                                                                 out int count, out string strErrorMessage);
        # endregion

        # region 查询现货某一个资金账户的转账流水情况
        /// <summary>
        ///  查询现货某一个资金账户的转账流水情况
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
        [OperationContract]
        List<UA_CapitalFlowTableInfo> SpotCapitalAccountTransferFlowFind(string userId, string capitalAccount,
                                                                 string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength,
                                                                 out int count, out string strErrorMessage);
        # endregion

        # region 查询期货某一个资金账户的转账流水情况
        /// <summary>
        /// 查询期货某一个资金账户的转账流水情况
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
        [OperationContract]
        List<UA_CapitalFlowTableInfo> FuturesCapitalAccountTransferFlowFind(string userId, string capitalAccount,
                                                                 string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength,
                                                                 out int count, out string strErrorMessage);
        # endregion

        # region 交易员的几个资金账户之间同币种两两自由转账

        /// <summary>
        /// 交易员的几个资金账户之间两两自由转账（同币种）
        /// </summary>
        /// <param name="freeTransfer"></param>
        /// <param name="currencyType"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        [OperationContract]
        bool TwoAccountsFreeTransferFunds(FreeTransferEntity freeTransfer, Types.CurrencyType currencyType,
                                          out string outMessage);
        # endregion 交易员的几个资金账户之间同币种两两自由转账

        # region  资产汇总查询（根据交易员ID及密码查询）
        /// <summary>
        /// 资产汇总查询（根据交易员ID及密码查询）
        /// </summary>
        /// <param name="password"></param>
        /// <param name="outMessage"></param>
        /// <param name="traderId"></param>
        /// <returns></returns>
        [OperationContract]
        List<AssetSummaryEntity> AssetSummaryFind(string traderId, string password, out string outMessage);
        # endregion
        #endregion

        #region -------------------按交易员查询   之前都是提供给前台使用，这里不再使用---------------

        //# region  银行资金明细查询
        ///// <summary>
        /////  银行资金明细查询
        ///// </summary>
        ///// <param name="userId">用户ID</param>
        ///// <param name="outMessage">输出信息</param>
        ///// <returns></returns>
        //[OperationContract]
        //List<UA_BankAccountTableInfo> BankCapitalFind2(string userId, out string outMessage);
        //# endregion

        //# region 现货资金明细查询（根据交易员和币种进行查询）
        ///// <summary>
        /////  现货资金明细查询（根据交易员和币种进行查询）
        ///// </summary>
        ///// <param name="userId">用户ID</param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="currencyType">币种</param>
        ///// <param name="userPassword">用户密码</param>
        ///// <param name="strErrorMessage">输出信息</param>
        ///// <returns></returns>
        //[OperationContract]
        //SpotCapitalEntity SpotCapitalFind2(string userId, int AccountType, Types.CurrencyType currencyType, string userPassword,
        //                                  ref string strErrorMessage);
        //# endregion

        //#region 期货资金明细查询（通过交易员和币种）
        ///// <summary>
        ///// 期货资金明细查询
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="currencyType"></param>
        ///// <param name="userPassword"></param>
        ///// <param name="strErrorMessage"></param>
        ///// <returns></returns>
        //[OperationContract]
        //FuturesCapitalEntity FuturesCapitalFind2(string userId, int AccountType, Types.CurrencyType currencyType,
        //                                        string userPassword, ref string strErrorMessage);
        //#endregion

        //# region 现货当日委托查询（根据交易员进行查询）
        ///// <summary>
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
        //[OperationContract]
        //List<XH_TodayEntrustTableInfo> SpotTodayEntrustFindByXhCapitalAccount2(string userId, int AccountType, string strPassword,
        //                                                                  SpotEntrustConditionFindEntity findCondition,
        //                                                                  int start, int pageLength, out int count,
        //                                                                ref string strErrorMessage);
        //# endregion

        //# region 期货当日委托查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
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
        //[OperationContract]
        //List<QH_TodayEntrustTableInfo> FuturesTodayEntrustFindByQhCapitalAccount2(string userId, int AccountType,
        //                                                                            string strPassword,
        //                                                                            FuturesEntrustConditionFindEntity
        //                                                                                findCondition, int start,
        //                                                                            int pageLength, out int count,
        //                                                                            ref string strErrorMessage);
        //# endregion

        //# region 现货当日成交查询 & 现货当日资金明细查询（根据交易员进行查询）

        ///// <summary>
        ///// 现货当日成交查询 & 现货当日资金明细查询（根据交易员进行查询）
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
        //[OperationContract]
        //List<XH_TodayTradeTableInfo> SpotTodayTradeFindByCapitalAccount2(string userId, int AccountType, string userPassword,
        //                                                            int start, int pageLength, out int count,
        //                                                            out string strErrorMessage,
        //                                                            SpotTradeConditionFindEntity findCondition);
        //# endregion

        //# region 期货当日成交查询& 期货当日资金明细查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）

        ///// <summary>
        ///// 期货当日成交查询& 期货当日资金明细查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
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
        //[OperationContract]
        //List<QH_TodayTradeTableInfo> FuturesTodayTradeFindByXhCapitalAccount2(string userId, int AccountType, string strPassword,
        //                                                                 FuturesTradeConditionFindEntity findCondition,
        //                                                                 int start, int pageLength, out int count,
        //                                                                 out string strErrorMessage);


        //# endregion 现货当日成交查询

        //# region 现货历史委托查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// <summary>
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
        //[OperationContract]
        //List<XH_HistoryEntrustTableInfo> SpotHistoryEntrustFind2(string userId, int AccountType, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, SpotEntrustConditionFindEntity findCondition);
        //# endregion

        //# region  期货历史委托查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// <summary> 
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
        //[OperationContract]
        //List<QH_HistoryEntrustTableInfo> FuturesHistoryEntrustFindByQhCapitalAccount2(string userId, int AccountType,
        //                                                                         string strPassword,
        //                                                                         FuturesEntrustConditionFindEntity
        //                                                                             findCondition, int start,
        //                                                                         int pageLength, out int count,
        //                                                                         ref string strErrorMessage);

        //# endregion

        //# region 现货历史成交查询& 现货历史资金明细查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// <summary>
        ///// 现货历史成交查询& 现货历史资金明细查询
        ///// 注：清算前，在此不能查询到当日资金明细
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
        //[OperationContract]
        //List<XH_HistoryTradeTableInfo> SpotHistoryTradeFind2(string userId, int AccountType, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, SpotTradeConditionFindEntity findCondition);
        //# endregion

        //# region 期货历史成交查询& 现货历史资金明细查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// <summary>
        ///// 期货历史成交查询& 现货历史资金明细查询（根据交易员、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///// 注：清算前，在此不能查询到当日资金明细
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
        //[OperationContract]
        //List<QH_HistoryTradeTableInfo> FuturesHistoryTradeFindByXhCapitalAccount2(string userId, int AccountType, string strPassword,
        //                                                                     FuturesTradeConditionFindEntity
        //                                                                         findCondition, int start,
        //                                                                     int pageLength, out int count,
        //                                                                     ref string strErrorMessage);


        //# endregion

        //# region  现货持仓查询（根据交易员、密码及查询条件来查询）
        ///// <summary>
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
        //[OperationContract]
        //List<SpotHoldFindResultEntity> SpotHoldFind2(string userId, int AccountType, string strPassword,
        //                                              SpotHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage);
        //# endregion

        //# region 期货持仓查询 （根据交易员、密码及查询条件来查询）
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
        //[OperationContract]
        //List<FuturesHoldFindResultEntity> FuturesHoldFind2(string userId, int AccountType, string strPassword,
        //                                                         FuturesHoldConditionFindEntity findCondition, int start,
        //                                                         int pageLength, out int count,
        //                                                         ref string strErrorMessage);
        //# endregion

        //# region 查询银行资金账户的转账流水情况
        ///// <summary>
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
        //[OperationContract]
        //List<UA_CapitalFlowTableInfo> BankCapitalAccountTransferFlowFind2(string userId, int AccountType,
        //                                                         string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength,
        //                                                         out int count, out string strErrorMessage);
        //# endregion

        //# region 查询现货某一个资金账户的转账流水情况
        ///// <summary>
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
        //[OperationContract]
        //List<UA_CapitalFlowTableInfo> SpotCapitalAccountTransferFlowFind2(string userId, int AccountType,
        //                                                         string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength,
        //                                                         out int count, out string strErrorMessage);
        //# endregion

        //# region 查询期货某一个资金账户的转账流水情况
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
        //[OperationContract]
        //List<UA_CapitalFlowTableInfo> FuturesCapitalAccountTransferFlowFind2(string userId, int AccountType,
        //                                                         string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength,
        //                                                         out int count, out string strErrorMessage);
        //# endregion

        //#region 查询现货某一个资金账户可交易某只股票的最大数量

        //[OperationContract]
        //int SpotCapitalTradeAmountFind(string userId, string userPassword, string stockCode);
        //#endregion

        #endregion---------------------------------------------

    }
}