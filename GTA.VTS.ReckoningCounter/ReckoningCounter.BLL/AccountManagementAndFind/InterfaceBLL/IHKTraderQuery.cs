using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.Entity.AccountManagementAndFindEntity.HK;
using ReckoningCounter.Entity;

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL
{
    /// <summary>
    /// Create By:李健华
    /// Create Date:2009-10-19
    /// Titel:交易员港股相关查询服务管理接口
    /// Desc.:主要是提供给前台查询，同时也适用于其他的客户端的查询，其体方法其体调用参考相关参数。
    ///       包括港股资金、持仓查询，交易员资产汇总查询，港股资金、持仓冻结查询
    ///       港股今日、历史成交、委托查询
    /// </summary>
    [ServiceContract]
    public interface IHKTraderQuery
    {
        # region 港股资金明细查询（根据交易员和币种进行查询）
        /// <summary>
        ///  港股资金明细查询（根据交易员和币种进行查询）包含盈亏
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="currencyType">币种</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        [OperationContract]
        HKCapitalEntity PagingQueryHKCapitalDetail(string userId, string userPassword, int accountType, Types.CurrencyType currencyType, ref string strErrorMessage);
        # endregion

        # region  港股持仓查询（根据交易员、密码及查询条件来查询）

        /// <summary>
        /// 港股持仓查询 包含盈亏
        /// </summary>
        /// <param name="userId">交易员</param>
        /// <param name="pwd"></param>
        /// <param name="accountType">账号类型（注：这是账户类型不是类别，这里是有九种类型，而类别是五种）</param>
        /// <param name="findCondition"></param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="errMesg"></param>
        /// <returns></returns>
        [OperationContract]
        List<HKHoldFindResultyEntity> PagingQueryHKHold(string userId, string pwd, int accountType, HKHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string errMesg);
        # endregion

        #region 港股持仓、持仓冻结查询

        #region 港股持仓明细查询
        #region 根据用户ID和密码查询用户所拥有的【港股持仓账号】明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的港股持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_AccountHoldInfo> QueryHK_AccountHoldByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencytype, out string errMsg);
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的港股持仓账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的港股持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_AccountHoldInfo> QueryHK_AccountHoldByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errMsg);
        #endregion

        #region 根据【港股持仓账号】查询港股持仓账号明细
        /// <summary>
        /// 根据【港股持仓账号】查询港股持仓账号明细
        /// </summary>
        ///<param name="hk_Cap_Acc">港股持仓账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_AccountHoldInfo> QueryHK_AccountHoldByAccount(string hk_Cap_Acc, QueryType.QueryCurrencyType currencyType, out string errMsg);
        #endregion
        #endregion

        #region 港股持仓冻结明细查询
        #region 根据委托编号查询【港股持仓冻结表】明细
        /// <summary>
        /// 根据委托编号查询【港股持仓冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        [OperationContract]
        List<HK_AcccountHoldFreezeInfo> QueryHK_AcccountHoldFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errMsg);
        #endregion

        #region 根据用户持仓账号和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细信息
        /// <summary>
        /// Title：根据用户持仓账号和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细信息
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
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_AcccountHoldFreezeInfo> PagingQueryHK_AcccountHoldFreezeByAccount(string holdAccount, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【港股持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_AcccountHoldFreezeInfo> PagingQueryHK_AcccountHoldFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion
        #endregion

        #endregion

        #region 港股资金、资金冻结查询

        #region 港股资金明细查询
        #region 根据用户ID和密码查询用户所拥有的【港股资金账号】明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的港股资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_CapitalAccountInfo> QueryHK_CapitalAccountTableByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errMsg);
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的港股资金账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的港股资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_CapitalAccountInfo> QueryHK_CapitalAccountByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errMsg);
        #endregion

        #region 根据【港股资金账号】查询港股资金账号明细
        /// <summary>
        /// 根据【港股资金账号】查询港股资金账号明细
        /// </summary>
        ///<param name="xh_Cap_Account">港股资金账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_CapitalAccountInfo> QueryHK_CapitalAccountByAccount(string xh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errMsg);
        #endregion
        #endregion

        #region 港股资金冻结明细查询
        #region 根据委托编号查询【港股资金冻结表】明细
        /// <summary>
        /// 根据委托编号查询【港股资金冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        [OperationContract]
        List<HK_CapitalAccountFreezeInfo> QueryHK_CapitalAccountFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errMsg);
        #endregion

        #region 根据用户资金账号和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细信息
        /// <summary>
        /// Title：根据用户资金账号和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细信息
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
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_CapitalAccountFreezeInfo> PagingQueryHK_CapitalAccountFreezeByAccount(string account, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【港股资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        ///        如果要查询某一天的，开始和结束时间相等(即要查2009-01-05这一天时，开始和结束时间传相同值2009-01-05)
        /// </summary>
        /// <param name="userID">资金账号</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_CapitalAccountFreezeInfo> PagingQueryHK_CapitalAccountFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion
        #endregion

        #endregion

        #region 港股今日/历史【委托】分页查询

        #region 港股【今日】委托分页查询
        /// <summary>
        /// Title:根据用户（交易员ID）、密码、查询条件实现分页查询港股当日委托信息,用户返回的是所拥有的资金账号
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pwd">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件对象</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_TodayEntrustInfo> PagingQueryHK_TodayEntrustByFilterAndUserIDPwd(string userID, string pwd, int accountType, HKEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion

        #region 港股【历史】委托分页查询
        /// <summary>
        /// Title:根据用户（交易员ID）、密码、查询条件实现分页查询港股历史委托信息,用户返回的是所拥有的资金账号
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pwd">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件对象</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns></returns>
        [OperationContract]
        List<HK_HistoryEntrustInfo> PagingQueryHK_HistoryEntrustByFilterAndUserIDPwd(string userID, string pwd, int accountType, HKEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errMsg);

        #endregion
        #endregion

        #region 港股今日/历史【成交】分页查询

        #region 港股【历史】成交信息分页查询-----根据用户和密码查询该用户所拥有的港股资金帐户历史港股成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的港股资金帐户历史港股成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个港股资金账号(如：4-商品港股资金帐号,6-股指港股资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>返回历史港股成交信息</returns>
        [OperationContract]
        List<HK_HistoryTradeInfo> PagingQueryHK_HistoryTradeByFilterAndUserIDPwd(string userID, string pwd, int accountType, HKTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion

        #region 港股【今日】成交信息分页查询 根据用户和密码查询该用户所拥有的港股资金帐户今日港股成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的港股资金帐户当日港股成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个港股资金账号(如：4-商品港股资金帐号,6-股指港股资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>返回当日港股成交信息</returns>
        [OperationContract]
        List<HK_TodayTradeInfo> PagingQueryHK_TodayTradeByFilterAndUserIDPwd(string userID, string pwd, int accountType, HKTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion
        #endregion

        #region 港股改单记录查询
        /// <summary>
        /// Title:根据用户ID和委托编号查询当日改单请求记录
        /// Desc.:此方法UserID和EntrustNumber,dateTime为空时忽略不当作查询条件，但条件不能同时为空，
        ///       当日查询自动会忽略时间条件查询。
        ///       如果查询历史时不传递时间或者传递的时间有误将查询当前时间前一个月内的条件查询
        /// Create By:李健华
        /// Create Date:2009-11-12
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="entrustNumber">委托单编号（被改单编号）</param>
        /// <param name="startTime">查询开始时间(如果为查询当日的可以为空)</param>
        /// <param name="endTime">查询结束时间(如果为查询当日的可以为空)</param>
        /// <param name="selectType">查询类型(0-查询所有即历史和当日，1-表时当日，2-表时历史)</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回查询到的总页数</param>
        /// <param name="errMsg">查询异常信息</param>
        /// <returns>返回查询到的当日当单记录</returns>
        [OperationContract]
        List<HK_HistoryModifyOrderRequestInfo> PagingQueryHK_ModifyRequertByUserIDOrEntrustNo(string userID, string entrustNumber, DateTime? startTime, DateTime? endTime, int selectType, PagingInfo pageInfo, out int total, out string errMsg);
        #endregion
    }
}
