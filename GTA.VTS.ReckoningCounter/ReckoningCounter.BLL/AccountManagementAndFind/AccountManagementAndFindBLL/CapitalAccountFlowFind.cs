using System;
using System.Collections.Generic;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.DAL;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.DAL.Data;
using System.Data;
using ReckoningCounter.BLL.Common;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL
{
    /// <summary>
    /// 作用：柜台各账户的资金流水查询（包括： 银行资金账户转账流水查询、现货资金账户转账流水查询、期货资金账户转账流水查询、 现货资金账户交易费用流水查询、 期货资金账户交易费用流水查询）
    /// 作者：李科恒
    /// 日期：2008-10-30
    /// Update by:李健华
    /// Update Date:2009-10-19
    /// Desc.:把未有实现的方法或者无用的方法删除掉
    /// </summary>
    public class CapitalAccountFlowFindBLL
    {

        //# region 时间查询条件
        ///// <summary>
        ///// 时间查询条件
        ///// </summary>
        ///// <param name="startTime">开始时间</param>
        ///// <param name="endTime">结束时间</param>
        ///// <returns></returns>
        //public string findTime(DateTime startTime, DateTime endTime)
        //{
        //    string result = string.Empty;
        //    if (DateTime.Compare(startTime, endTime) < 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
        //    {
        //        #region update 2009-07-09 李健华
        //        //result += string.Format("AND TransferTime BETWEEN '{0}' AND '{1}'", startTime, endTime);
        //        result += string.Format("AND TransferTime>= '{0}' AND  TransferTime<'{1}'", startTime.ToString("yyyy-MM-dd"), endTime.AddDays(1).ToString("yyyy-MM-dd"));
        //        #endregion
        //    }
        //    return result;
        //}
        //# endregion 查询时间

        # region (NEW)查询指定资金账户的转账流水情况
        /// <summary>
        /// (NEW)查询指定资金账户的转账流水情况
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> CapitalAccountTransferFlowFind(string userId, string capitalAccount, string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength, out int count, out string strErrorMessage)
        {
            count = 0;
            strErrorMessage = string.Empty;

            List<UA_CapitalFlowTableInfo> result = null;
            UA_CapitalFlowTableDal dal = new UA_CapitalFlowTableDal();
            try
            {
                if (!string.IsNullOrEmpty(capitalAccount)) //先判断是否输入了资金帐户
                {
                    //通过资金账号获得该资金账号所对应的交易员ID
                    #region 从缓存中获取用户账号信息
                    UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetUserByAccount(capitalAccount);
                    #endregion

                    if (userInfo != null && userInfo.UserID.Trim() == userId.Trim())
                    {
                        //string tempt = findTime(startTime, endTime);
                        string tempt = string.Empty;
                        if (DateTime.Compare(startTime, endTime) <= 0) //起始时间小于结束时间（比如2008-05-06小于2008-05-28）
                        {
                            tempt= string.Format("AND TransferTime>= '{0}' AND  TransferTime<'{1}'", startTime.ToString("yyyy-MM-dd"), endTime.AddDays(1).ToString("yyyy-MM-dd"));
                        }
                        string whereCondition = string.Format("(FromCapitalAccount='{0}' OR ToCapitalAccount='{1}')", capitalAccount, capitalAccount);
                        whereCondition = whereCondition + tempt;

                        #region update 2009-07-09 李健华
                        PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                        ppInfo.IsCount = true;
                        ppInfo.PageNumber = start;
                        ppInfo.PageSize = pageLength;
                        ppInfo.Fields = "CapitalFlowLogo,FromCapitalAccount,ToCapitalAccount,uc.TransferAmount,TransferTime,TradeCurrencyType,TransferTypeLogo";
                        ppInfo.PK = "CapitalFlowLogo";
                        ppInfo.Filter = whereCondition;

                        CommonDALOperate<UA_CapitalFlowTableInfo> com = new CommonDALOperate<UA_CapitalFlowTableInfo>();
                        result = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);

                        #endregion
                    }
                    else
                        strErrorMessage = "查询失败！失败原因为：交易员ID或资金账号输入错误 ！";
                }
                else
                {
                    strErrorMessage = "查询失败！失败原因为：没有输入资金账号 ！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return result;
        }
        # endregion

        # region 查询指定资金账户的转账流水情况 Create by 李健华 Create date: 2009-07-09
        /// <summary>
        /// 根据用户ID和密码和过滤条件分页查询转账流水并以时间降序排列
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pwd">用户密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> CapitalAccountTransferFlowFind(string userId, string pwd, int accountType, QueryUA_CapitalFlowFilter filter, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            List<UA_CapitalFlowTableInfo> result = null;
            UA_CapitalFlowTableDal dal = new UA_CapitalFlowTableDal();
            errorMsg = "";
            total = 0;
            #region 如果条件根据主键查询直接返回一条记录
            int flowLogo = 0;
            if (filter != null && int.TryParse(filter.FlowLogo, out flowLogo))
            {
                if (flowLogo > 0)
                {
                    result = new List<UA_CapitalFlowTableInfo>();
                    result.Add(dal.GetModel(flowLogo));
                    total = 1;
                    return result;
                }
            }
            #endregion

            #region 非主键查询条件过滤
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            PagingProceduresInfo ppInfo = new PagingProceduresInfo();
            ppInfo.IsCount = pageInfo.IsCount;
            ppInfo.PageNumber = pageInfo.CurrentPage;
            ppInfo.PageSize = pageInfo.PageLength;
            ppInfo.Fields = " uc.[CapitalFlowLogo],uc.[FromCapitalAccount],uc.[ToCapitalAccount],uc.[TransferAmount],uc.[TransferTime],uc.[TradeCurrencyType],uc.[TransferTypeLogo]";
            ppInfo.PK = "uc.[CapitalFlowLogo]";
            if (pageInfo.Sort == 0)
            {
                ppInfo.Sort = " uc.TransferTime asc ";
            }
            else
            {
                ppInfo.Sort = " uc.TransferTime desc ";
            }

            //inner join dbo.UA_UserBasicInformationTable as u on u.Password=''2343''目前不要密
            ppInfo.Tables = "UA_UserAccountAllocationTable as ua  inner join dbo.UA_CapitalFlowTable as uc on uc.FromCapitalaccount in (ua.UserAccountDistributeLogo) or uc.toCapitalaccount in (ua.UserAccountDistributeLogo)";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            if (accountType == 0)
            {
                sb.Append("ua.userid='" + userId + "'  and ua.accounttypelogo in ( select accounttypelogo from BD_AccountType )  ");
            }
            else
            {
                sb.Append("ua.userid='" + userId + "'  and ua.accounttypelogo ='" + (int)accountType + "'   ");
            }


            #region 如果条件对象不为null时
            if (filter != null)
            {
                //如果要查询转账金额有值加上条件，查询为大于等于这个值
                // int amount = 0;
                //if (int.TryParse(filter.CapitalAmount, out amount))
                //{
                if (filter.CapitalAmount > 0)
                {
                    sb.Append("  And uc.TransferAmount>='" + filter.CapitalAmount + "'");
                }
                //}

                //时间查询条件
                if (filter.StartTime.HasValue && filter.EndTime.HasValue)
                {
                    sb.Append(" and  (uc.TransferTime >= '" + filter.StartTime.Value.ToString("yyyy-MM-dd") + "' And  uc.TransferTime <'" + filter.EndTime.Value.AddDays(1).ToString("yyyy-MM-dd") + "')");
                }

                //转账类型 1---自由转账,2---分红转账,3--追加资金 ,其他为全部
                if (filter.CapitalFlowType >= 1 && filter.CapitalFlowType <= 3)
                {
                    sb.Append(" and uc.TransferTypeLogo='" + filter.CapitalFlowType + "'");
                }
                //转账货币类型
                if (filter.CurrencyType != QueryType.QueryCurrencyType.ALL)
                {
                    sb.Append(" and uc.TradeCurrencyType='" + (int)filter.CurrencyType + "'");
                }
                //如果指定转出账号
                if (!string.IsNullOrEmpty(filter.FromCapitalAccount))
                {
                    sb.Append(" and uc.FromCapitalAccount='" + filter.FromCapitalAccount.Trim() + "'");
                }
                //如果指定转入账号
                if (!string.IsNullOrEmpty(filter.ToCapitalAccount))
                {
                    sb.Append(" and uc.ToCapitalAccount='" + filter.ToCapitalAccount.Trim() + "'");
                }
            }
            #endregion

            #endregion

            ppInfo.Filter = sb.ToString();
            #endregion

            try
            {
                CommonDALOperate<UA_CapitalFlowTableInfo> com = new CommonDALOperate<UA_CapitalFlowTableInfo>();
                result = com.PagingQueryProcedures(ppInfo, out total, dal.ReaderBind);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }

        # endregion

    }
}