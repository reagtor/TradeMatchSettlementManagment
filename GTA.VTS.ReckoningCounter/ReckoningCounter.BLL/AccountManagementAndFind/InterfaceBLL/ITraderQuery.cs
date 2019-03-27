using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.Entity;
using ReckoningCounter.DAL.FuturesDevolveService;
using GTA.VTS.Common.CommonObject;

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL
{
    /// <summary>
    /// Create By:李健华
    /// Create Date:2009-07-21
    /// Titel:交易员相关查询服务管理
    /// Desc.:主要是提供给前台查询，同时也适用于其他的客户端的查询，其体方法其体调用参考相关参数。
    ///       包括现货、期货资金、持仓查询，交易员资产汇总查询，现货、期货资金、持仓冻结查询
    ///       现货、期货今日、历史成交、委托查询，银行资金查询，转账流水查询
    /// </summary>
    [ServiceContract]
    public interface ITraderQuery
    {
        # region 现货资金明细查询（根据交易员和币种进行查询）
        /// <summary>
        ///  现货资金明细查询（根据交易员和币种进行查询）包含盈亏
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="currencyType">币种</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        [OperationContract]
        SpotCapitalEntity PagingQuerySpotCapital(string userId, int accountType, Types.CurrencyType currencyType, string userPassword,
                                          ref string strErrorMessage);
        # endregion

        #region 期货资金明细查询（通过交易员和币种）
        /// <summary>
        /// 期货资金明细查询包含盈亏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="currencyType">货币类型（RMB-1，HK-2，US-3）</param>
        /// <param name="userPassword"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        [OperationContract]
        FuturesCapitalEntity PagingQueryFuturesCapital(string userId, int accountType, Types.CurrencyType currencyType,
                                                string userPassword, ref string strErrorMessage);
        #endregion

        # region  现货持仓查询（根据交易员、密码及查询条件来查询）
        /// <summary>
        ///  现货持仓查询 包含盈亏
        /// </summary>
        /// <param name="userId">交易员</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strErrorMessage"></param>
        /// <param name="strPassword"></param>
        /// <param name="findCondition"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <returns></returns>
        [OperationContract]
        List<SpotHoldFindResultEntity> PagingQuerySpotHold(string userId, int accountType, string strPassword,
                                                      SpotHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage);
        # endregion

        # region 期货持仓查询 （根据交易员、密码及查询条件来查询）
        /// <summary>
        /// 期货持仓查询 （根据交易员、密码及查询条件来查询）
        /// </summary>
        /// <param name="userId">交易员</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        [OperationContract]
        List<FuturesHoldFindResultEntity> PagingQueryFuturesHold(string userId, int accountType, string strPassword,
                                                                 FuturesHoldConditionFindEntity findCondition, int start,
                                                                 int pageLength, out int count,
                                                                 ref string strErrorMessage);
        # endregion

        #region 查询现货某一个资金账户可交易某只股票的最大数量
        /// <summary>
        /// 查询现货某一个资金账户可交易某只股票的最大数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userPassword"></param>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        [OperationContract]
        int SpotCapitalTradeAmountFind(string userId, string userPassword, string stockCode);
        #endregion

        # region  资产汇总查询（根据交易员ID及密码查询）
        /// <summary>
        /// 资产汇总查询（根据交易员ID及密码查询）
        /// </summary>
        /// <param name="password">交易员密码</param>
        /// <param name="outMessage"></param>
        /// <param name="traderId">交易员ID</param>
        /// <returns></returns>
        [OperationContract]
        List<AssetSummaryEntity> QueryAssetSummary(string traderId, string password, out string outMessage);
        # endregion

        #region Create by:李健华 Create Date:2009-07-08

        #region 现货持仓、持仓冻结查询

        #region 现货持仓明细查询
        #region 根据用户ID和密码查询用户所拥有的【现货持仓账号】明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的现货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_AccountHoldTableInfo> QueryXH_AccountHoldTableByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencytype, out string errorMsg);
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的现货持仓账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的现货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_AccountHoldTableInfo> QueryXH_AccountHoldTableByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion

        #region 根据【现货持仓账号】查询现货持仓账号明细
        /// <summary>
        /// 根据【现货持仓账号】查询现货持仓账号明细
        /// </summary>
        ///<param name="xh_Cap_Account">现货持仓账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_AccountHoldTableInfo> QueryXH_AccountHoldTableByAccount(string xh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion
        #endregion

        #region 现货持仓冻结明细查询
        #region 根据委托编号查询【现货持仓冻结表】明细
        /// <summary>
        /// 根据委托编号查询【现货持仓冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        [OperationContract]
        List<XH_AcccountHoldFreezeTableInfo> QueryXH_AcccountHoldFreezeTableByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg);
        #endregion

        #region 根据用户持仓账号和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细信息
        /// <summary>
        /// Title：根据用户持仓账号和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="holdAccount">持仓账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_AcccountHoldFreezeTableInfo> PagingQueryXH_AcccountHoldFreezeTableByAccount(string holdAccount, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_AcccountHoldFreezeTableInfo> PagingQueryXH_AcccountHoldFreezeTableByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion
        #endregion

        #endregion

        #region 期货持仓、持仓冻结查询

        #region 期货持仓明细查询
        #region 根据用户ID和密码查询用户所拥有的【期货持仓账号】明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的期货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HoldAccountTableInfo> QueryQH_HoldAccountTableByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的期货持仓账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的期货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HoldAccountTableInfo> QueryQH_HoldAccountTableByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion

        #region 根据【期货持仓账号】查询期货持仓账号明细
        /// <summary>
        /// 根据【期货持仓账号】查询期货持仓账号明细
        /// </summary>
        ///<param name="xh_Cap_Account">期货持仓账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HoldAccountTableInfo> QueryQH_HoldAccountTableByAccount(string xh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion
        #endregion

        #region 期货持仓冻结明细查询
        #region 根据委托编号查询【期货持仓冻结表】明细
        /// <summary>
        /// 根据委托编号查询【期货持仓冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HoldAccountFreezeTableInfo> QueryQH_HoldAccountFreezeTableByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg);
        #endregion

        #region 根据用户持仓账号和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细信息
        /// <summary>
        /// Title：根据用户持仓账号和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="holdAccount">持仓账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HoldAccountFreezeTableInfo> PagingQueryQH_HoldAccountFreezeTableByAccount(string holdAccount, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="userID">持仓账号</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HoldAccountFreezeTableInfo> PagingQueryQH_HoldAccountFreezeTableByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion
        #endregion

        #endregion

        #region 期货资金、资金冻结查询

        #region 期货资金明细查询

        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的期货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_CapitalAccountTableInfo> QueryQH_CapitalAccountTableByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的期货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_CapitalAccountTableInfo> QueryQH_CapitalAccountTableByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        /// <summary>
        /// 根据期货资金账号查询期货资金账号明细
        /// </summary>
        ///<param name="qh_Cap_Account">期货资金账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_CapitalAccountTableInfo> QueryQH_CapitalAccountTableByAccount(string qh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg);

        #endregion

        #region 期货资金冻结明细查询
        /// <summary>
        /// 根据委托编号查询【期货资金冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">查询冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        [OperationContract]
        List<QH_CapitalAccountFreezeTableInfo> QueryQH_CapitalAccountFreezeTableByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg);
        /// <summary>
        /// Title：根据用户资金账号和查询的交易的货币类型、查询时间段查询【期货资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">查询冻结类型</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_CapitalAccountFreezeTableInfo> PagingQueryQH_CapitalAccountFreezeTableByAccount(string account, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【期货资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="userID">资金账号</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">查询冻结类型</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_CapitalAccountFreezeTableInfo> PagingQueryQH_CapitalAccountFreezeTableByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #endregion

        #region 现货资金、资金冻结查询

        #region 现货资金明细查询
        #region 根据用户ID和密码查询用户所拥有的【现货资金账号】明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的现货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_CapitalAccountTableInfo> QueryXH_CapitalAccountTableByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的现货资金账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的现货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_CapitalAccountTableInfo> QueryXH_CapitalAccountTableByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion

        #region 根据【现货资金账号】查询现货资金账号明细
        /// <summary>
        /// 根据【现货资金账号】查询现货资金账号明细
        /// </summary>
        ///<param name="xh_Cap_Account">现货资金账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_CapitalAccountTableInfo> QueryXH_CapitalAccountTableByAccount(string xh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg);
        #endregion
        #endregion

        #region 现货资金冻结明细查询
        #region 根据委托编号查询【现货资金冻结表】明细
        /// <summary>
        /// 根据委托编号查询【现货资金冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        [OperationContract]
        List<XH_CapitalAccountFreezeTableInfo> QueryXH_CapitalAccountFreezeTableByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg);
        #endregion

        #region 根据用户资金账号和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细信息
        /// <summary>
        /// Title：根据用户资金账号和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_CapitalAccountFreezeTableInfo> PagingQueryXH_CapitalAccountFreezeTableByAccount(string account, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="userID">资金账号</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_CapitalAccountFreezeTableInfo> PagingQueryXH_CapitalAccountFreezeTableByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion
        #endregion

        #endregion

        #region 现货今日/历史【委托】分页查询

        #region 现货【今日】委托分页查询
        /// <summary>
        /// Title:根据用户（交易员ID）、密码、查询条件实现分页查询现货当日委托信息,用户返回的是所拥有的资金账号
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pwd">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件对象</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_TodayEntrustTableInfo> PagingQueryXH_TodayEntrustInfoByFilterAndUserIDPwd(string userID, string pwd, int accountType, SpotEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 现货【历史】委托分页查询
        /// <summary>
        /// Title:根据用户（交易员ID）、密码、查询条件实现分页查询现货历史委托信息,用户返回的是所拥有的资金账号
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pwd">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件对象</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<XH_HistoryEntrustTableInfo> PagingQueryXH_HistoryEntrustInfoByFilterAndUserIDPwd(string userID, string pwd, int accountType, SpotEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);

        #endregion
        #endregion

        #region 现货今日/历史【成交】分页查询

        #region 现货【历史】成交信息分页查询-----根据用户和密码查询该用户所拥有的现货资金帐户历史现货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的现货资金帐户历史现货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：4-商品现货资金帐号,6-股指现货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回历史现货成交信息</returns>
        [OperationContract]
        List<XH_HistoryTradeTableInfo> PagingQueryXH_HistoryTradeByFilterAndUserIDPwd(string userID, string pwd, int accountType, SpotTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 现货【今日】成交信息分页查询 根据用户和密码查询该用户所拥有的现货资金帐户今日现货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的现货资金帐户当日现货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个现货资金账号(如：4-商品现货资金帐号,6-股指现货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日现货成交信息</returns>
        [OperationContract]
        List<XH_TodayTradeTableInfo> PagingQueryXH_TodayTradeByFilterAndUserIDPwd(string userID, string pwd, int accountType, SpotTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion
        #endregion

        #region 期货今日/历史【委托】成交分页查询

        #region 期货【当日】委托查询 根据用户（交易员ID）、密码、查询条件实现分页查询期货当日委托信息
        /// <summary>
        /// Title:根据用户（交易员ID）、密码、查询条件实现分页查询期货当日委托信息,用户返回的是所拥有的资金账号
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pwd">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件对象</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_TodayEntrustTableInfo> PagingQueryQH_TodayEntrustInfoByFilterAndUserIDPwd(string userID, string pwd, int accountType, FuturesEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 期货【历史】委托查询 根据用户（交易员ID）、密码、查询条件实现分页查询期货历史委托信息
        /// <summary>
        /// Title:根据用户（交易员ID）、密码、查询条件实现分页查询期货历史委托信息,用户返回的是所拥有的资金账号
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pwd">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件对象</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_HistoryEntrustTableInfo> PagingQueryQH_HistoryEntrustInfoByFilterAndUserIDPwd(string userID, string pwd, int accountType, FuturesEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #endregion

        #region 期货今日/历史【成交】分页查询

        #region 期货【历史】成交信息分页查询-----根据用户和密码查询该用户所拥有的期货资金帐户历史期货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的期货资金帐户历史期货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个期货资金账号(如：4-商品期货资金帐号,6-股指期货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货成交信息</returns>
        [OperationContract]
        List<QH_HistoryTradeTableInfo> PagingQueryQH_HistoryTradeByFilterAndUserIDPwd(string userID, string pwd, int accountType, FuturesTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 期货【今日】成交信息分页查询----根据用户和密码查询该用户所拥有的期货资金帐户当日期货成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的期货资金帐户当日期货成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个期货资金账号(如：4-商品期货资金帐号,6-股指期货资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货成交信息</returns>
        [OperationContract]
        List<QH_TodayTradeTableInfo> PagingQueryQH_TodayTradeByFilterAndUserIDPwd(string userID, string pwd, int accountType, FuturesTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #endregion

        #region 分页查询转账流水
        /// <summary>
        /// 分页查询转账流水
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="pwd">用户密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤的条件</param>
        /// <param name="pageInfo">分页的相关信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<UA_CapitalFlowTableInfo> PaginQueryUA_CapitalFlowTableByFilter(string userId, string pwd, int accountType, QueryUA_CapitalFlowFilter filter, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion

        #region 银行资金明细查询
        #region 根据用户ID查询银行资金明细
        /// <summary>
        /// 根据用户ID查询银行资金明细
        /// </summary>
        /// <param name="userId">要查询的交易员ID</param>
        /// <param name="errorMsg">查询失败异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<UA_BankAccountTableInfo> QueryUA_BankAccountByUserID(string userId, out string errorMsg);
        #endregion

        #region 根据银行账号查询银行资金明细
        /// <summary>
        /// 根据银行账号查询银行资金明细
        /// </summary>
        /// <param name="bankAccount">要查询的银行账号</param>
        /// <param name="errorMsg">查询失败异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<UA_BankAccountTableInfo> QueryUA_BankAccountByBankAccount(string bankAccount, out string errorMsg);
        #endregion
        #endregion

        #region 已经在管理中心获取
        //#region 根据品种ID（breedclassID）返回期货规则
        ///// <summary>
        ///// 根据品种ID（breedclassID）返回期货规则
        ///// </summary>
        ///// <param name="breedClassID">品种ID</param>
        ///// <returns>返回期货交易规则</returns>
        //[OperationContract]
        //QH_FuturesTradeRules QueryQH_FuturesTradeRulesByBreedClassID(int breedClassID);
        //#endregion

        //#region 根据品种ID（breedclassID）返回现货规则
        ///// <summary>
        ///// 根据品种ID（breedclassID）返回现货规则
        ///// </summary>
        ///// <param name="breedClassID">品种ID</param>
        ///// <returns>返回现货交易规则</returns>
        //[OperationContract]
        //XH_SpotTradeRules QueryXH_SpotTradeRulesByBreedClassID(int breedClassID);
        //#endregion

        //#region 单位查询
        ///// <summary>
        ///// 获取所有的单位
        ///// </summary>
        ///// <returns>返回所有的单位数据列表</returns>
        //[OperationContract]
        //IList<CM_Units> QueryAllCM_Units();
        //#endregion

        //#region 单位转换查询
        ///// <summary>
        ///// 获取所有的单位转换数据
        ///// </summary>
        ///// <returns>所有的单位转换数据实体列表</returns>
        //[OperationContract]
        //IList<CM_UnitConversion> QueryAllCM_UnitConversion();
        //#endregion
        #endregion

        #endregion
        #region 期货资金流水（盘后清算）查询
        /// <summary>
        /// Title:根据资金账号和密码查询期货相应盘后的资金清算流水
        /// Desc.:此方法查询返回的是盘后清算的资金流水方法.
        ///       如果开始日期和结束日期为null/"",默认查询当前日志前一个月内的所有资金流水，
        ///       如果开始日期和结束日期都为非法数据则同上默认查询当前日志前一个月内的所有资金流水，
        ///       为了提供查询效率，如果不是为了实现分页则在查询时传递默认的
        ///       是否返回分页是设置为false，不返回总页数
        /// Create By:李健华
        /// Create Date:2009-12-04
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币类型</param>
        /// <param name="pageInfo">分页信息实体</param>
        /// <param name="total">总页数据</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_TradeCapitalFlowDetailInfo> PagingQueryQH_TradeCapitalFlowDetailByAccount(string userID, string pwd, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, PagingInfo pageInfo, out int total, out string errorMsg);

        /// <summary>
        /// 描述：商品期货资金流水查询
        /// 作者：叶振东
        /// 时间：2010-01-28
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币类型</param>
        /// <param name="pageInfo">分页信息实体</param>
        /// <param name="total">总页数据</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<QH_TradeCapitalFlowDetailInfo> PagingQuerySPQH_TradeCapitalFlowDetailByAccount(string userID, string pwd, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, PagingInfo pageInfo, out int total, out string errorMsg);
        #endregion
    }
}
