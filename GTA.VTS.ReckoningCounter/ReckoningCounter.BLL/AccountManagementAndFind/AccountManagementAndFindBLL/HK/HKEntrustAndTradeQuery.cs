using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.DAL.Data;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.AccountManagementAndFindEntity.HK;
using ReckoningCounter.Entity.Model;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity;
using ReckoningCounter.Model;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL.HK
{
    /// <summary>
    /// Title:港股委托成交查询类（包括今日委托、成交，历史委托、成交,改单记录）
    /// Create By:李健华
    /// Create Date:2009-10-19
    /// </summary>
    public class HKEntrustAndTradeQuery
    {
        #region 港股今日/历史【委托】分页查询

        #region 港股【今日】委托查询 根据用户和密码、过滤条件查询该用户所拥有的港股资金帐户今日港股委托信息

        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的港股资金帐户今日期货委托信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个港股资金账号(如：-商品港股资金帐号,-股指港股资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货委托信息</returns>
        public List<HK_TodayEntrustInfo> PagingHK_TodayEntrustByFilter(string userID, string pwd, int accountType, HKEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<HK_TodayEntrustInfo> list = null;
            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                #region 从数据库中判断
                //UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                //if (!usDal.Exists(userID, pwd))
                //{
                //    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                //    return list;
                //}
                #endregion

                #region 从缓存中判断
                UA_UserBasicInformationTableInfo userInfo = AccountManager.Instance.GetBasicUserByUserId(userID);
                if (userInfo == null)
                {
                    errorMsg = "交易员对应类型的帐号不存在";
                    return list;
                }
                if (userInfo.Password != pwd)
                {
                    errorMsg = "交易员密码错误";
                    return list;
                }
                #endregion
            }
            #endregion

            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                try
                {
                    list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                    if (!Utils.IsNullOrEmpty(list))
                    {
                        total = 1;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = "  EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,Code,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeID,HoldAccount,CapitalAccount,OrderStatusID,OrderPriceType,OrderMessage,CurrencyTypeID,TradeUnitID,CallbackChannlID,McOrderID,HasDoneProfit,OfferTime,EntrustTime,IsModifyOrder,ModifyOrderNumber";

                ppInfo.PK = "EntrustNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " EntrustTime asc ";
                }
                else
                {
                    ppInfo.Sort = " EntrustTime desc ";
                }
                ppInfo.Tables = "HK_TodayEntrust";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildHKEntrustQueryWhere(filter, userID, accountType, true);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<HK_TodayEntrustInfo> com = new CommonDALOperate<HK_TodayEntrustInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingXH_TodayEntrustByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }

        #endregion

        #region 港股【历史】委托查询 根据用户和密码、过滤条件查询该用户所拥有的港股资金帐户历史港股委托信息

        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的港股资金帐户历史期货委托信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个港股资金账号(如：-商品港股资金帐号,-股指港股资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日期货委托信息</returns>
        public List<HK_HistoryEntrustInfo> PagingHK_HistoryEntrustByFilter(string userID, string pwd, int accountType, HKEntrustConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<HK_HistoryEntrustInfo> list = null;
            HK_HistoryEntrustDal dal = new HK_HistoryEntrustDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                #region 从数据库中判断
                //UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                //if (!usDal.Exists(userID, pwd))
                //{
                //    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                //    return list;
                //}
                #endregion

                #region 从缓存中判断
                UA_UserBasicInformationTableInfo userInfo = AccountManager.Instance.GetBasicUserByUserId(userID);
                if (userInfo == null)
                {
                    errorMsg = "交易员对应类型的帐号不存在";
                    return list;
                }
                if (userInfo.Password != pwd)
                {
                    errorMsg = "交易员密码错误";
                    return list;
                }
                #endregion
            }
            #endregion

            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = "  EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,Code,TradeAmount,TradeAveragePrice,OrderPriceType,CancelAmount,BuySellTypeID,HoldAccount,CapitalAccount,CurrencyTypeID,TradeUnitID,OrderStatusID,OrderMessage,McOrderID,HasDoneProfit,OfferTime,EntrustTime,IsModifyOrder,ModifyOrderNumber  ";

                ppInfo.PK = "EntrustNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " EntrustTime asc ";
                }
                else
                {
                    ppInfo.Sort = " EntrustTime desc ";
                }
                ppInfo.Tables = "HK_HistoryEntrust";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildHKEntrustQueryWhere(filter, userID, accountType, false);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<HK_HistoryEntrustInfo> com = new CommonDALOperate<HK_HistoryEntrustInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }

        #endregion

        #region 组装港股委托过滤条件
        /// <summary>
        /// 组装港股委托过滤条件
        /// </summary>
        /// <param name="filter">港股委托过滤条件实体</param>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="isToday">是否为查询今日委托</param>
        /// <returns></returns>
        string BuildHKEntrustQueryWhere(HKEntrustConditionFindEntity filter, string userID, int accountType, bool isToday)
        {
            #region 过滤条件组装
            StringBuilder sbFilter = new StringBuilder("1=1 ");
            if (filter != null)
            {
                # region  0.资金账号
                if (!string.IsNullOrEmpty(filter.HKCapitalAccount))
                {
                    sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.HKCapitalAccount.Trim());
                }
                else //不指定可能有多个账号
                {
                    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                    if (accountType == 0)
                    {
                        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                    }
                    else
                    {
                        sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (filter.BuySellDirection != 0)
                {
                    sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                }

                # endregion

                # region 2.可撤标识
                if (isToday) //历史已经没有这个字段
                {
                    if (filter.CanBeWithdrawnLogo != 0)
                    {
                        sbFilter.AppendFormat(" AND CancelLogo='{0}'", filter.CanBeWithdrawnLogo);
                    }
                }
                # endregion

                # region 3.委托状态
                if (filter.EntrustState != 0)
                {
                    sbFilter.AppendFormat(" AND OrderStatusId='{0}'", filter.EntrustState);
                }
                # endregion

                # region 4.币种赋值
                if (filter.CurrencyType != 0)
                {
                    sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyType);
                }
                # endregion

                # region 5.港股代码
                if (!string.IsNullOrEmpty(filter.HKCode))
                {
                    sbFilter.AppendFormat(" AND Code='{0}'", filter.HKCode);
                }
                # endregion

                # region 6.查询开始时间和结束时间
                if (!isToday) //为当日成交就不分时间查询
                {
                    sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(filter.StartTime, filter.EndTime, 30), "EntrustTime");
                }
                #endregion

            }
            else
            {
                //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                if (accountType == 0)
                {
                    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                }
                else
                {
                    sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                }
            }
            #endregion
            return sbFilter.ToString();
        }
        #endregion

        #endregion

        #region 港股今日/历史成交分页查询

        #region 港股资金帐户历史港股成交信息分页查询--根据用户和密码查询该用户所拥有的港股资金帐户历史港股成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的港股资金帐户历史港股成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个港股资金账号(如：4-商品港股资金帐号,6-股指港股资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日港股成交信息</returns>
        public List<HK_HistoryTradeInfo> PagingHK_HistoryTradeByFilter(string userID, string pwd, int accountType, HKTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<HK_HistoryTradeInfo> list = null;
            HK_HistoryTradeDal dal = new HK_HistoryTradeDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                #region 从数据库中判断
                //UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                //if (!usDal.Exists(userID, pwd))
                //{
                //    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                //    return list;
                //}
                #endregion

                #region 从缓存中判断
                UA_UserBasicInformationTableInfo userInfo = AccountManager.Instance.GetBasicUserByUserId(userID);
                if (userInfo == null)
                {
                    errorMsg = "交易员对应类型的帐号不存在";
                    return list;
                }
                if (userInfo.Password != pwd)
                {
                    errorMsg = "交易员密码错误";
                    return list;
                }
                #endregion
            }
            #endregion

            //string strFilter = "";
            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                list = dal.GetListArray(" EntrustNumber='" + filter.EntrustNumber + "'");
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = "  TradeNumber,PortfolioLogo,EntrustNumber,EntrustPrice,TradePrice,TradeAmount,StampTax,Commission,Code,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,ClearingFee,HoldAccount,CapitalAccount,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ";

                ppInfo.PK = "TradeNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " TradeTime asc ";
                }
                else
                {
                    ppInfo.Sort = " TradeTime desc ";
                }
                ppInfo.Tables = " HK_HistoryTrade";
                #endregion

                #region 过滤条件组装
                ppInfo.Filter = BuildHKTradeQueryWhere(filter, userID, accountType, false);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<HK_HistoryTradeInfo> com = new CommonDALOperate<HK_HistoryTradeInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }
        #endregion

        #region 港股资金帐户当日港股成交信息分页查询--根据用户和密码查询该用户所拥有的港股资金帐户当日港股成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的港股资金帐户当日港股成交信息
        ///  Des.:此方法因为一个用户（交易员）可能拥有多个港股资金账号(如：4-商品港股资金帐号,6-股指港股资金帐号)
        ///  所以返回的数据会回多个资金账号的数据
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码--如果密码为空时不验证密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件数据</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">返回数页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>返回当日港股成交信息</returns>
        public List<HK_TodayTradeInfo> PagingHK_TodayTradeByFilter(string userID, string pwd, int accountType, HKTradeConditionFindEntity filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<HK_TodayTradeInfo> list = null;
            HK_TodayTradeDal dal = new HK_TodayTradeDal();
            errorMsg = "";
            total = 0;

            #region 密码不为空时先验证用户
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "查询失败！失败原因为：交易员ID不能为空！";
                return list;
            }
            if (!string.IsNullOrEmpty(pwd))
            {
                #region 从数据库中判断
                //UA_UserBasicInformationTableDal usDal = new UA_UserBasicInformationTableDal();
                //if (!usDal.Exists(userID, pwd))
                //{
                //    errorMsg = "查询失败！失败原因为：交易员ID或密码输入错误 ！";
                //    return list;
                //}
                #endregion
                #region 从缓存中判断
                UA_UserBasicInformationTableInfo userInfo = AccountManager.Instance.GetBasicUserByUserId(userID);
                if (userInfo == null)
                {
                    errorMsg = "交易员对应类型的帐号不存在";
                    return list;
                }
                if (userInfo.Password != pwd)
                {
                    errorMsg = "交易员密码错误";
                    return list;
                }
                #endregion
            }
            #endregion

            string strFilter = "";
            if (filter != null && !string.IsNullOrEmpty(filter.EntrustNumber))  //委托单号（当只根据委托单号查询时）
            {
                #region 如果有委托单号直接查询唯一记录
                strFilter += string.Format(" EntrustNumber='{0}'", filter.EntrustNumber);
                list = dal.GetListArray(strFilter);
                if (!Utils.IsNullOrEmpty(list))
                {
                    total = 1;
                }
                #endregion
            }
            else //当不带委托单号时
            {
                #region 如果分页信息为空返回异常
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 分页存储过程相关信息组装
                PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                ppInfo.IsCount = pageInfo.IsCount;
                ppInfo.PageNumber = pageInfo.CurrentPage;
                ppInfo.PageSize = pageInfo.PageLength;
                ppInfo.Fields = " TradeNumber,PortfolioLogo,EntrustNumber,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,HoldAccount,CapitalAccount,Code,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ";

                ppInfo.PK = "TradeNumber";
                if (pageInfo.Sort == 0)
                {
                    ppInfo.Sort = " TradeTime asc ";
                }
                else
                {
                    ppInfo.Sort = " TradeTime desc ";
                }
                ppInfo.Tables = " HK_TodayTrade";
                #endregion

                #region 过滤条件组装

                ppInfo.Filter = BuildHKTradeQueryWhere(filter, userID, accountType, true);
                #endregion

                #region 执行查询
                try
                {
                    CommonDALOperate<HK_TodayTradeInfo> com = new CommonDALOperate<HK_TodayTradeInfo>();
                    list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
                    //list = dal.PagingXH_TodayTradeByFilter(ppInfo, out total);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            return list;
        }
        #endregion

        #region 组装港股成交过滤条件
        /// <summary>
        /// 组装港股成交过滤条件
        /// </summary>
        /// <param name="filter">过滤条件实体</param>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的港股持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="isToday">是否为当日查询</param>
        /// <returns></returns>
        string BuildHKTradeQueryWhere(HKTradeConditionFindEntity filter, string userID, int accountType, bool isToday)
        {
            #region 过滤条件组装
            StringBuilder sbFilter = new StringBuilder("1=1 ");
            // strFilter = "1=1 ";
            if (filter != null)
            {
                # region 0.资金帐户
                if (!string.IsNullOrEmpty(filter.HKCapitalAccount))
                {
                    sbFilter.AppendFormat(" AND CapitalAccount='{0}'", filter.HKCapitalAccount);
                }
                else //不指定可能有多个账号
                {
                    sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                    if (accountType == 0)
                    {
                        sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where  atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                    }
                    else
                    {
                        sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                    }
                }
                # endregion

                # region  1.买卖方向

                if (filter.BuySellDirection != 0)
                {
                    sbFilter.AppendFormat(" AND BuySellTypeId='{0}'", filter.BuySellDirection);
                }
                # endregion

                # region 2.成交类型
                if (filter.TradeType != 0)
                {
                    sbFilter.AppendFormat(" AND TradeTypeId='{0}'", filter.TradeType);
                }
                # endregion

                # region 3.币种赋值
                if (filter.CurrencyType != 0)
                {
                    sbFilter.AppendFormat(" AND CurrencyTypeId='{0}'", filter.CurrencyType);
                }
                # endregion

                # region 4.合约代码
                if (!string.IsNullOrEmpty(filter.HKCode))
                {
                    sbFilter.AppendFormat(" AND Code='{0}'", filter.HKCode);
                }
                # endregion

                # region 5.查询开始时间和结束时间
                if (!isToday) //为当日成交就不分时间查询
                {
                    sbFilter.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(filter.StartTime, filter.EndTime, 30), "TradeTime");


                }
                #endregion

            }
            else
            {
                //条件为空直接查询用户所对应拥有的资金账户下的今日成交
                sbFilter.Append(" And CapitalAccount in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable   ");
                if (accountType == 0)
                {
                    sbFilter.AppendFormat("  where accounttypelogo in (select accounttypelogo from BD_AccountType where  atcid='{0}')  and userid='{1}')", (int)Types.AccountAttributionType.SpotCapital, userID);
                }
                else
                {
                    sbFilter.AppendFormat("  where accounttypelogo='{0}'  and userid='{1}')", accountType, userID);
                }
            }
            #endregion
            return sbFilter.ToString();
        }
        #endregion

        #endregion

        #region 港股改单记录查询
        /// <summary>
        /// Title:根据用户ID和委托编号查询当日改单请求记录
        /// Desc.:此方法UserID和EntrustNumber,dateTime为空时忽略不当作查询条件，但条件不能同时为空，
        ///       查询时间当查询类型为查询历史时应提供时间提高查询效率。
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
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns>返回查询到的当日当单记录</returns>
        public List<HK_HistoryModifyOrderRequestInfo> PagingHK_ModifyRequertByUserIDOrEntrustNo(string userID, string entrustNumber, DateTime? startTime, DateTime? endTime, int selectType, PagingInfo pageInfo, out int total, out string errorMsg)
        {

            List<HK_HistoryModifyOrderRequestInfo> list = null;
            HK_HistoryModifyOrderRequestDal dal = new HK_HistoryModifyOrderRequestDal();
            errorMsg = "";
            total = 0;

            #region 基本字段验证
            if (string.IsNullOrEmpty(userID) && string.IsNullOrEmpty(entrustNumber))
            {
                errorMsg = "查询失败！失败原因为：交易员ID和委托编号不能同时为空！";
                return list;
            }
            #endregion

            string whereSql = " 1=1   ";
            //组装用户ID
            if (!string.IsNullOrEmpty(userID))
            {
                whereSql += " And  TraderId='" + userID.Trim() + "'  ";
            }
            //组装委托编号
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                whereSql += " And  EntrustNubmer='" + entrustNumber.Trim() + "'  ";
            }

            //改单时间条件
            string modifyOrderDateTimeWhere = string.Format(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "ModifyOrderDateTime");



            #region 如果分页信息为空返回异常
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页存储过程相关信息组装
            PagingProceduresInfo ppInfo = new PagingProceduresInfo();
            ppInfo.IsCount = pageInfo.IsCount;
            ppInfo.PageNumber = pageInfo.CurrentPage;
            ppInfo.PageSize = pageInfo.PageLength;
            ppInfo.Fields = " ID,ChannelID,TraderId,FundAccountId,TraderPassword,Code,EntrustNubmer,OrderPrice,OrderAmount,Message,ModifyOrderDateTime  ";

            ppInfo.PK = " EntrustNubmer";
            if (pageInfo.Sort == 0)
            {
                ppInfo.Sort = " ModifyOrderDateTime asc ";
            }
            else
            {
                ppInfo.Sort = " ModifyOrderDateTime desc ";
            }

            string tables = " HK_ModifyOrderRequest ";
            switch (selectType)
            {
                case 1://当日查询
                    tables = " HK_ModifyOrderRequest ";
                    ppInfo.Filter = whereSql;
                    break;
                case 2://历史查询
                    tables = " HK_HistoryModifyOrderRequest ";
                    ppInfo.Filter = whereSql + modifyOrderDateTimeWhere;
                    break;
                default://默认查询所有(包括历史和当日)
                    tables = " ( select [ID],[ChannelID],[TraderId],[FundAccountId],[TraderPassword],[Code],[EntrustNubmer],[OrderPrice]";
                    tables += ",[OrderAmount],[Message],[ModifyOrderDateTime]  from  HK_ModifyOrderRequest ";
                    tables += " Where " + whereSql;
                    tables += "  union all ";
                    tables += " SELECT [ID],[ChannelID],[TraderId],[FundAccountId],[TraderPassword],[Code],[EntrustNubmer],[OrderPrice]";
                    tables += ",[OrderAmount],[Message],[ModifyOrderDateTime]  FROM HK_HistoryModifyOrderRequest ";
                    tables += " Where " + whereSql + "  " + modifyOrderDateTimeWhere + " ) as t ";
                    ppInfo.Filter = "";
                    break;
            }

            ppInfo.Tables = tables;

            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<HK_HistoryModifyOrderRequestInfo> com = new CommonDALOperate<HK_HistoryModifyOrderRequestInfo>();
                list = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            #endregion

            return list;
        }
        #endregion
    }
}
