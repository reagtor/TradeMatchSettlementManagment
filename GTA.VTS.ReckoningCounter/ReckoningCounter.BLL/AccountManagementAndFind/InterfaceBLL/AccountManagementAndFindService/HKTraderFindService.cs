using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL.HK;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.AccountManagementAndFindEntity.HK;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL;

namespace ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService
{
    /// <summary>
    ///Title:交易员港股查询接口服务实现
    ///Desc.:此类接口的所有方法为了适用于ROE相关而开启的
    ///Create by:李健华
    ///Create Date:2009-10-26
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class HKTraderFindService : IHKTraderFind
    {

        # region 港股资金明细查询（根据港股资金账号，用户密码和币种进行查询）
        /// <summary>
        ///  港股资金明细查询（根据港股资金账号，用户密码和币种进行查询）
        /// </summary>
        /// <param name="capitalAccount">港股资金账号</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="currencyType">币种</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <returns></returns>
        public HKCapitalEntity HKCapitalFind(string capitalAccount, GTA.VTS.Common.CommonObject.Types.CurrencyType currencyType, string userPassword, ref string strErrorMessage)
        {
            HKCapitalAndHoldQuery hkQuery = new HKCapitalAndHoldQuery();
            return hkQuery.QueryHKCapitalDetail(capitalAccount, currencyType, userPassword, ref strErrorMessage);

        }
        # endregion

        # region  港股持仓查询（根据港股资金账号、密码及查询条件来查询）

        /// <summary>
        /// 港股持仓查询（根据港股资金账号、密码及查询条件来查询）
        /// </summary>
        /// <param name="hk_Capi_Acc">港股账号</param>
        /// <param name="userPassword"></param>
        /// <param name="findCondition">查询过滤条件</param>
        /// <param name="start">记录集页索引</param>
        /// <param name="pageLength">页长</param>
        /// <param name="count">查到的记录数</param>
        /// <param name="errMesg"></param>
        /// <returns></returns>
        public List<HKHoldFindResultyEntity> HKHoldFind(string capitalAccount, string userPassword, HKHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string errMesg)
        {
            HKCapitalAndHoldQuery hkQuery = new HKCapitalAndHoldQuery();
            return hkQuery.QueryHKHoldSummary(capitalAccount, userPassword, findCondition, start, pageLength, out count, ref errMesg);

        }
        # endregion

        #region 港股今日/历史【委托】分页查询

        #region 港股【今日】委托分页查询
        /// <summary>
        /// Title:根据港股资金帐号、密码、查询条件实现分页查询港股当日委托信息
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="hk_Capi_Acc">港股资金帐号</param>
        /// <param name="userPassword">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="findCondition">过滤条件对象</param>
        /// <param name="start">起码页（查询那一页）页码</param>
        /// <param name="pageLength">每页长</param>
        /// <param name="count">总页数</param>
        /// <param name="strErrorMessage">查询异常信息</param>
        /// <returns></returns>
        public List<HK_TodayEntrustInfo> HKTodayEntrustFindByHKCapitalAccount(string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, HKEntrustConditionFindEntity findCondition)
        {
            UA_UserAccountAllocationTableInfo ua_acc_Model = AccountManager.Instance.GetUserByAccount(capitalAccount);
            if (ua_acc_Model == null)
            {
                strErrorMessage = "查询不到该港股资金账户相关信息!";
                count = 0;
                return null;
            }

            HKEntrustAndTradeQuery etf = new HKEntrustAndTradeQuery();
            PagingInfo pagInfo = new PagingInfo();
            pagInfo.CurrentPage = start;
            pagInfo.IsCount = true;
            pagInfo.PageLength = pageLength;
            return etf.PagingHK_TodayEntrustByFilter(ua_acc_Model.UserID, userPassword, (int)Types.AccountType.HKSpotCapital, findCondition, pagInfo, out count, out strErrorMessage);

        }
        #endregion

        #region 港股【历史】委托分页查询
        /// <summary>
        /// Title:根据港股资金帐号、密码、查询条件实现分页查询港股历史委托信息
        /// Desc.:过滤条件中如果条件为string型：当传入为(""|null)时条件忽略。 如果为int型：当传入为(0)时条件忽略。
        /// </summary>
        /// <param name="capitalAccount">港股资金帐号</param>
        /// <param name="userPassword">用户密码(如果为空时不检查用户密码有效性)</param>
        /// <param name="findCondition">过滤条件对象</param>
        /// <param name="start">起码页（查询那一页）页码</param>
        /// <param name="pageLength">每页长</param>
        /// <param name="count">总页数</param>
        /// <param name="strErrorMessage">查询异常信息</param>
        /// <returns></returns>
        public List<HK_HistoryEntrustInfo> HKHistoryEntrustFind(string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, HKEntrustConditionFindEntity findCondition)
        {
            UA_UserAccountAllocationTableInfo ua_acc_Model = AccountManager.Instance.GetUserByAccount(capitalAccount);
            if (ua_acc_Model == null)
            {
                strErrorMessage = "查询不到该港股资金账户相关信息!";
                count = 0;
                return null;
            }

            HKEntrustAndTradeQuery etf = new HKEntrustAndTradeQuery();
            PagingInfo pagInfo = new PagingInfo();
            pagInfo.CurrentPage = start;
            pagInfo.IsCount = true;
            pagInfo.PageLength = pageLength;
            return etf.PagingHK_HistoryEntrustByFilter(ua_acc_Model.UserID, userPassword, (int)Types.AccountType.HKSpotCapital, findCondition, pagInfo, out count, out strErrorMessage);
        }
        #endregion

        #endregion

        #region 港股今日/历史【成交】分页查询

        #region 港股【历史】成交信息分页查询-----根据用户和密码查询该用户所拥有的港股资金帐户历史港股成交信息
        /// <summary>
        ///  Title:根据用户和密码查询该用户所拥有的港股资金帐户历史港股成交信息
        /// </summary>
        /// <param name="capitalAccount">港股资金账号</param>
        /// <param name="userPassword">用户（交易员）密码</param>
        /// <param name="findCondition">过滤条件对象</param>
        /// <param name="start">起码页（查询那一页）页码</param>
        /// <param name="pageLength">每页长</param>
        /// <param name="count">总页数</param>
        /// <param name="strErrorMessage">查询异常信息</param>
        /// <returns>返回当日港股成交信息</returns>
        public List<HK_HistoryTradeInfo> HKHistoryTradeFind(string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, HKTradeConditionFindEntity findCondition)
        {
            UA_UserAccountAllocationTableInfo ua_acc_Model = AccountManager.Instance.GetUserByAccount(capitalAccount);
            if (ua_acc_Model == null)
            {
                strErrorMessage = "查询不到该港股资金账户相关信息!";
                count = 0;
                return null;
            }

            HKEntrustAndTradeQuery etf = new HKEntrustAndTradeQuery();
            PagingInfo pagInfo = new PagingInfo();
            pagInfo.CurrentPage = start;
            pagInfo.IsCount = true;
            pagInfo.PageLength = pageLength;
            return etf.PagingHK_HistoryTradeByFilter(ua_acc_Model.UserID, userPassword, (int)Types.AccountType.HKSpotCapital, findCondition, pagInfo, out count, out strErrorMessage);

        }

        #endregion

        #region 港股【今日】成交信息分页查询 根据港股资金账号和密码查询该用户所拥有的港股资金帐户今日港股成交信息
        /// <summary>
        ///  Title:根据港股资金账号和密码查询该用户所拥有的港股资金帐户当日港股成交信息
        ///  Des.: 根据港股资金账号和密码查询该用户所拥有的港股资金帐户当日港股成交信息
        /// </summary>
        /// <param name="capitalAccount">港股资金账号</param>
        /// <param name="userPassword">用户（交易员）密码</param>
        /// <param name="findCondition">过滤条件对象</param>
        /// <param name="start">起码页（查询那一页）页码</param>
        /// <param name="pageLength">每页长</param>
        /// <param name="count">总页数</param>
        /// <param name="strErrorMessage">查询异常信息</param>
        /// <returns>返回当日港股成交信息</returns>
        public List<HK_TodayTradeInfo> HKTodayTradeFindByCapitalAccount(string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage, HKTradeConditionFindEntity findCondition)
        {
            UA_UserAccountAllocationTableInfo ua_acc_Model = AccountManager.Instance.GetUserByAccount(capitalAccount);
            if (ua_acc_Model == null)
            {
                strErrorMessage = "查询不到该港股资金账户相关信息!";
                count = 0;
                return null;
            }

            HKEntrustAndTradeQuery etf = new HKEntrustAndTradeQuery();
            PagingInfo pagInfo = new PagingInfo();
            pagInfo.CurrentPage = start;
            pagInfo.IsCount = true;
            pagInfo.PageLength = pageLength;
            return etf.PagingHK_TodayTradeByFilter(ua_acc_Model.UserID, userPassword, (int)Types.AccountType.HKSpotCapital, findCondition, pagInfo, out count, out strErrorMessage);

        }
        #endregion
        #endregion

        # region 查询港股某一个资金账户的转账流水情况
        /// <summary>
        ///  查询港股某一个资金账户的转账流水情况
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
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> HKCapitalAccountTransferFlowFind(string userId, string capitalAccount, string userPassword, DateTime startTime, DateTime endTime, int start, int pageLength, out int count, out string strErrorMessage)
        {
            CapitalAccountFlowFindBLL cp = new CapitalAccountFlowFindBLL();
            return cp.CapitalAccountTransferFlowFind(userId, capitalAccount, userPassword, startTime, endTime, start, pageLength, out count, out strErrorMessage);
        }
        # endregion


 
    }
}
