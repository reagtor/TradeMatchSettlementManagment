using System;
using System.Collections.Generic;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.DAL;
using ReckoningCounter.DAL.AccountManagementAndFindDAL;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL.HK;

namespace ReckoningCounter.BLL
{
    /// <summary>
    /// 作用：柜台管理员查询交易员委托和成交查询情况（包括： 现货委托查询、现货成交查询、期货委托查询、期货成交查询）
    ///       以及各资金账户明细和各持仓账户明细（包括： 银行资金明细查询、现货资金明细查询、 期货资金明细查询、现货持仓查询、 期货持仓查询）
    ///       和各资金账户的资金流水情况
    /// 作者：李科恒
    /// 日期：2008-11-18
    /// Update By:李健华
    /// Update Date:2009-07-28
    /// Desc.:修改相关的逻辑和精简之前的方法并把一些数据从缓存数据中获取
    /// Update by:李健华
    /// Update Date:2009-10-19
    /// Desc.:把未有实现的方法或者无用的方法删除掉
    /// </summary>
    public class AdministratorFindTraderBLL
    {
        # region 验证管理员密码是否正确
        /// <summary>
        ///  验证管理员密码是否正确
        /// </summary>
        /// <param name="userId">管理ID</param>
        /// <param name="password">管理员密码</param>
        /// <param name="outMessage">验证异常信息</param>
        /// <returns></returns>
        public bool AdminIsExist(string userId, string password, out string outMessage)
        {
            bool result = false;
            outMessage = string.Empty;
            #region try ---catch
            try
            {
                #region old code
                //将资金账号加入查询条件中
                //findCondition.SpotCapitalAccount = capitalAccount;
                //List<UA_UserBasicInformationTableInfo> tempt = null;
                //string whereCondition = string.Format("UserID='{0}' AND Password='{1}' AND RoleNumber='1'", userId, password);
                //tempt = DataRepository.UaUserBasicInformationTableProvider.GetPaged(whereCondition, "RoleNumber ASC", 0, 10, out count);
                #endregion

                #region 数据库验证
                UA_UserBasicInformationTableDal dal = new UA_UserBasicInformationTableDal();
                if (dal.Exists(userId, password, GTA.VTS.Common.CommonObject.Types.UserId.Manager))
                {
                    result = true;
                }
                else
                {
                    outMessage = "查询失败，该管理员不存在或密码错误！";
                }
                #endregion
            }
            catch (Exception ex)
            {
                outMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            #endregion
            return result;
        }
        # endregion

        #region  (NEW)管理员查询交易员银行资金明细情况
        /// <summary>
        ///  管理员查询交易员银行资金明细情况
        /// </summary>
        /// <param name="admin">查询实体过滤条件</param>
        /// <param name="outMessage">查询异常信息</param>
        /// <returns></returns>
        public List<UA_BankAccountTableInfo> AdminFindTraderBankCapital(AdministratorFindEntity admin, out string outMessage)
        {
            outMessage = string.Empty;
            List<UA_BankAccountTableInfo> list = null;
            // CapitalAndHoldFindBLL aa = new CapitalAndHoldFindBLL();
            // AccountManagementDAL bb = new AccountManagementDAL();
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out outMessage);

                #region 若管理员密码正确
                if (findResult)
                {
                    #region 从缓存中获取交易员的银行账号
                    var bankAccount = AccountManager.Instance.GetAccountByUserIDAndAccountType(admin.TraderID, (int)Types.AccountType.BankAccount);
                    //string bankAccount = bb.Find_UserBankCapitalAccount(admin.TraderID);
                    #endregion

                    #region 获取数据
                    if (bankAccount != null)
                    {
                        UA_BankAccountTableDal dal = new UA_BankAccountTableDal();
                        list = dal.GetListArray(string.Format("UserAccountDistributeLogo ='{0}'", bankAccount.UserAccountDistributeLogo.Trim()));
                    }
                    else
                    {
                        outMessage = "对不起，查询失败！失败原因为：查询不到该交易员的没有相应的银行账号 ！";
                    }
                    #endregion
                    //这里不调用以下方法为了不重复验证银行账号
                    //result = aa.BankCapitalFind(admin.TraderID, bankAccount, out outMessage);
                }
                #endregion
            }
            catch (Exception ex)
            {
                outMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region (NEW)管理员查询交易员现货当日委托情况
        /// <summary>
        /// 管理员查询交易员现货当日委托情况
        /// </summary>
        /// <param name="admin">管理员查实体对象</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_TodayEntrustTableInfo> AdminFindTraderSpotTodayEntrust(AdministratorFindEntity admin, SpotEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<XH_TodayEntrustTableInfo> list = new List<XH_TodayEntrustTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有现货资金账号
                    var XhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.SpotCapital);
                    // string[] XhCapitalAccount = bb.Find_UserXHCapitalAccount(admin.TraderID);
                    #endregion

                    #region 篇历交易员的资金账户查询相关数据（这里是按之前的分页方法查询，这是不正确的）
                    foreach (UA_UserAccountAllocationTableInfo item in XhCapitalAccount)
                    {
                        //这里按理分页是不正确的
                        var tempt = aa.SpotTodayEntrustFindByXhCapitalAccount(item.UserAccountDistributeLogo.Trim(), "", findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            list.AddRange(tempt);
                        }
                        //foreach (XH_TodayEntrustTableInfo _XhTodayEntrustTable in tempt)
                        //{
                        //    result.Add(_XhTodayEntrustTable);
                        //}
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return list;
        }
        #endregion

        #region (NEW)管理员查询交易员期货当日委托情况
        /// <summary>
        /// 管理员查询交易员期货当日委托情况
        /// </summary>
        /// <param name="admin">管理员查实体对象</param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_TodayEntrustTableInfo> AdminFindTraderFuturesTodayEntrust(AdministratorFindEntity admin, FuturesEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<QH_TodayEntrustTableInfo> list = new List<QH_TodayEntrustTableInfo>();
            //List<QH_TodayEntrustTableInfo> tempt = new List<QH_TodayEntrustTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    //查出交易员所拥有的所有期货资金账号
                    #region 从缓存中获取交易员所拥有的所有期货资金账号
                    var QhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.FuturesCapital);
                    // string[] QhCapitalAccount = bb.Find_UserQHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in QhCapitalAccount)
                    {
                        var tempt = aa.FuturesTodayEntrustFindByQhCapitalAccount(item.UserAccountDistributeLogo, "", findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            list.AddRange(tempt);
                        }
                        //foreach (QH_TodayEntrustTableInfo _QhTodayEntrustTable in tempt)
                        //{
                        //    result.Add(_QhTodayEntrustTable);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return list;
        }
        #endregion

        #region （NEW）管理员查询交易员现货历史委托情况
        /// <summary>
        ///  管理员查询交易员现货历史委托情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<XH_HistoryEntrustTableInfo> AdminFindTraderSpotHistoryEntrust(AdministratorFindEntity admin, SpotEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<XH_HistoryEntrustTableInfo> result = new List<XH_HistoryEntrustTableInfo>();
            //List<XH_HistoryEntrustTableInfo> tempt = new List<XH_HistoryEntrustTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有货资金账号
                    var XhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.SpotCapital);
                    // string[] XhCapitalAccount = bb.Find_UserXHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in XhCapitalAccount)
                    {
                        var tempt = aa.SpotHistoryEntrustFindByXhCapitalAccount(item.UserAccountDistributeLogo, "",
                         findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (XH_HistoryEntrustTableInfo _XhHistoryEntrustTable in tempt)
                        //{
                        //    result.Add(_XhHistoryEntrustTable);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return result;
        }
        #endregion

        #region （NEW）管理员查询交易员期货历史委托情况
        /// <summary>
        ///  管理员查询交易员期货历史委托情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<QH_HistoryEntrustTableInfo> AdminFindTraderFuturesHistoryEntrust(AdministratorFindEntity admin, FuturesEntrustConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<QH_HistoryEntrustTableInfo> result = new List<QH_HistoryEntrustTableInfo>();
            //List<QH_HistoryEntrustTableInfo> tempt = new List<QH_HistoryEntrustTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有期货资金账号
                    var QhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.FuturesCapital);
                    //string[] QhCapitalAccount = bb.Find_UserQHCapitalAccount(admin.TraderID);
                    #endregion



                    foreach (var item in QhCapitalAccount)
                    {
                        var tempt = aa.FuturesHistoryEntrustFindByQhCapitalAccount(item.UserAccountDistributeLogo, "",
                        findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (QH_HistoryEntrustTableInfo _QhHistoryEntrustTable in tempt)
                        //{
                        //    result.Add(_QhHistoryEntrustTable);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return result;
        }
        #endregion

        #region  (NEW)管理员查询交易员现货当日成交情况
        /// <summary>
        /// 管理员查询交易员现货当日成交情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_TodayTradeTableInfo> AdminFindTraderSpotTodayTrade(AdministratorFindEntity admin, SpotTradeConditionFindEntity findCondition, int start, int pageLength, out int count, out string strErrorMessage)
        {
            List<XH_TodayTradeTableInfo> result = new List<XH_TodayTradeTableInfo>();
            //List<XH_TodayTradeTableInfo> tempt = new List<XH_TodayTradeTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {

                    #region 从缓存中获取交易员所拥有的所有现货资金账号
                    var XhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.SpotCapital);
                    //string[] XhCapitalAccount = bb.Find_UserXHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in XhCapitalAccount)
                    {
                        var tempt = aa.SpotTodayTradeFindByXhCapitalAccount(item.UserAccountDistributeLogo, "",
                          findCondition, start, pageLength, out  count, out strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (XH_TodayTradeTableInfo _XH_TodayTradeTableInfo in tempt)
                        //{
                        //    result.Add(_XH_TodayTradeTableInfo);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        #region  (NEW)管理员查询交易员期货当日成交情况
        /// <summary>
        /// 管理员查询交易员期货当日成交情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_TodayTradeTableInfo> AdminFindTraderFuturesTodayTrade(AdministratorFindEntity admin, FuturesTradeConditionFindEntity findCondition, int start, int pageLength, out int count, out string strErrorMessage)
        {
            List<QH_TodayTradeTableInfo> result = new List<QH_TodayTradeTableInfo>();
            //List<QH_TodayTradeTableInfo> tempt = new List<QH_TodayTradeTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();

            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有现货资金账号
                    var QhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.FuturesCapital);
                    //string[] QhCapitalAccount = bb.Find_UserQHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in QhCapitalAccount)
                    {
                        var tempt = aa.FuturesTodayTradeFindByXhCapitalAccount(item.UserAccountDistributeLogo, "",
                         findCondition, start, pageLength, out  count, out strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (QH_TodayTradeTableInfo _QH_TodayTradeTableInfo in tempt)
                        //{
                        //    result.Add(_QH_TodayTradeTableInfo);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        #region  (NEW)管理员查询交易员现货历史成交情况
        /// <summary>
        /// 管理员查询交易员现货历史成交情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<XH_HistoryTradeTableInfo> AdminFindTraderSpotHistoryTrade(AdministratorFindEntity admin, SpotTradeConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<XH_HistoryTradeTableInfo> result = new List<XH_HistoryTradeTableInfo>();
            //List<XH_HistoryTradeTableInfo> tempt = new List<XH_HistoryTradeTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();

            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有现货资金账号
                    var XhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.SpotCapital);
                    // string[] XhCapitalAccount = bb.Find_UserXHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in XhCapitalAccount)
                    {
                        var tempt = aa.SpotHistoryTradeFindByXhCapitalAccount(item.UserAccountDistributeLogo, "",
                         findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (XH_HistoryTradeTableInfo _XH_HistoryTradeTableInfo in tempt)
                        //{
                        //    result.Add(_XH_HistoryTradeTableInfo);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;

        }
        #endregion

        #region  (NEW)管理员查询交易员期货历史成交情况
        /// <summary>
        /// 管理员查询交易员期货历史成交情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<QH_HistoryTradeTableInfo> AdminFindTraderFuturesHistoryTrade(AdministratorFindEntity admin, FuturesTradeConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<QH_HistoryTradeTableInfo> result = new List<QH_HistoryTradeTableInfo>();
            //List<QH_HistoryTradeTableInfo> tempt = new List<QH_HistoryTradeTableInfo>();
            EntrustAndTradeFindBLL aa = new EntrustAndTradeFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();

            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有期货资金账号
                    var QhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.FuturesCapital);
                    // string[] QhCapitalAccount = bb.Find_UserQHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in QhCapitalAccount)
                    {
                        var tempt = aa.FuturesHistoryTradeFindByQHCapitalAccount(item.UserAccountDistributeLogo, "",
                          findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (QH_HistoryTradeTableInfo _QH_HistoryTradeTableInfo in tempt)
                        //{
                        //    result.Add(_QH_HistoryTradeTableInfo);
                        //}
                    }
                }
                else // 若管理员密码错误
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;

        }
        #endregion

        #region  (NEW)管理员查询交易员现货资金明细情况
        /// <summary>
        ///  管理员查询交易员现货资金明细情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<SpotCapitalEntity> AdminFindTraderSpotCapital(AdministratorFindEntity admin, ref string strErrorMessage)
        {
            var result = new List<SpotCapitalEntity>();
            //var tempt = new SpotCapitalEntity();
            var aa = new CapitalAndHoldFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();

            strErrorMessage = string.Empty;
            //bool findResult = false;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有现货资金账号
                    var XhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.SpotCapital);
                    // string[] XhCapitalAccount = bb.Find_UserXHCapitalAccount(admin.TraderID);
                    #endregion
                    //查出交易员所拥有的所有现货资金账号
                    foreach (var item in XhCapitalAccount)
                    {
                        //这里因为有可以返回港股账号，因为港股账号也属现货账号类别，这里只是查询原来现货的
                        if (item.AccountTypeLogo == (int)Types.AccountType.HKSpotCapital)
                        {
                            continue;
                        }
                        //查出该现货资金帐户人民币的情况
                        var temptRMB = aa.SpotCapitalFind(item.UserAccountDistributeLogo, Types.CurrencyType.RMB, "", ref strErrorMessage);
                        if (temptRMB != null)
                        {
                            result.Add(temptRMB);
                        }
                        //查出该现货资金帐户港币的情况
                        var temptHK = aa.SpotCapitalFind(item.UserAccountDistributeLogo, Types.CurrencyType.HK, "", ref strErrorMessage);
                        if (temptHK != null)
                        {
                            result.Add(temptHK);
                        }
                        //查出该现货资金帐户美元的情况
                        var temptUS = aa.SpotCapitalFind(item.UserAccountDistributeLogo, Types.CurrencyType.US, "", ref strErrorMessage);
                        if (temptUS != null)
                        {
                            result.Add(temptUS);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        #region  (NEW)管理员查询交易员期货资金明细情况
        /// <summary>
        ///  管理员查询交易员期货资金明细情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<FuturesCapitalEntity> AdminFindTraderFuturesCapital(AdministratorFindEntity admin, ref string strErrorMessage)
        {

            var result = new List<FuturesCapitalEntity>();
            //var tempt = new FuturesCapitalEntity();
            var aa = new CapitalAndHoldFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();

            strErrorMessage = string.Empty;
            //bool findResult = false;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有期货资金账号
                    var QhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.FuturesCapital);
                    // string[] QhCapitalAccount = bb.Find_UserQHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in QhCapitalAccount)
                    {
                        //查出该现货资金帐户人民币的情况
                        var temptRMB = aa.FuturesCapitalFind(item.UserAccountDistributeLogo, Types.CurrencyType.RMB, "", ref strErrorMessage);
                        if (temptRMB != null)
                        {
                            result.Add(temptRMB);
                        }

                        //查出该现货资金帐户港币的情况
                        var temptHK = aa.FuturesCapitalFind(item.UserAccountDistributeLogo, Types.CurrencyType.HK, "", ref strErrorMessage);
                        if (temptHK != null)
                        {
                            result.Add(temptHK);
                        }

                        //查出该现货资金帐户美元的情况
                        var temptUS = aa.FuturesCapitalFind(item.UserAccountDistributeLogo, Types.CurrencyType.US, "", ref strErrorMessage);
                        if (temptUS != null)
                        {
                            result.Add(temptUS);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        #region  （NEW）管理员查询交易员现货持仓情况
        /// <summary>
        ///  管理员查询交易员现货持仓情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<SpotHoldFindResultEntity> AdminFindTraderSpotHold(AdministratorFindEntity admin, SpotHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<SpotHoldFindResultEntity> result = new List<SpotHoldFindResultEntity>();
            //var tempt = new List<SpotHoldFindResultEntity>();
            var aa = new CapitalAndHoldFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有现货资金账号
                    var XhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.SpotCapital);
                    //   string[] XhCapitalAccount = bb.Find_UserXHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in XhCapitalAccount)
                    {
                        var tempt = aa.SpotHoldFind(item.UserAccountDistributeLogo, "",
                         findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (SpotHoldFindResultEntity _SpotHoldFindResultEntity in tempt)
                        //{
                        //    result.Add(_SpotHoldFindResultEntity);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        #region  （NEW）管理员查询交易员期货持仓情况
        /// <summary>
        ///  管理员查询交易员期货持仓情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="findCondition"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<FuturesHoldFindResultEntity> AdminFindTraderFuturesHold(AdministratorFindEntity admin, FuturesHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            var result = new List<FuturesHoldFindResultEntity>();
            //var tempt = new List<FuturesHoldFindResultEntity>();
            var aa = new CapitalAndHoldFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取交易员所拥有的所有期货资金账号
                    var QhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.FuturesCapital);
                    //  string[] QhCapitalAccount = bb.Find_UserQHCapitalAccount(admin.TraderID);
                    #endregion

                    foreach (var item in QhCapitalAccount)
                    {
                        var tempt = aa.FuturesHoldFind(item.UserAccountDistributeLogo, "",
                         findCondition, start, pageLength, out  count, ref strErrorMessage);
                        if (!Utils.IsNullOrEmpty(tempt))
                        {
                            result.AddRange(tempt);
                        }
                        //foreach (FuturesHoldFindResultEntity _FuturesHoldFindResultEntity in tempt)
                        //{
                        //    result.Add(_FuturesHoldFindResultEntity);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        #region  （NEW）管理员查询交易员资产汇总情况
        /// <summary>
        ///  管理员查询交易员资产汇总情况
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public List<AssetSummaryEntity> AdminFindTraderAssetSummary(AdministratorFindEntity admin, out string outMessage)
        {
            var result = new List<AssetSummaryEntity>();
            //var findAccount = new FindAccountEntity();
            //var tempt = new List<FuturesHoldFindResultEntity>();
            var aa = new CapitalAndHoldFindBLL();
            outMessage = string.Empty;
            //bool findResult = false;
            //int count = 0;
            try
            {
                if (!string.IsNullOrEmpty(admin.TraderID))
                {
                    // 验证管理员密码是否正确
                    bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out outMessage);

                    if (findResult)// 若管理员密码正确
                    {
                        var findAccount = new FindAccountEntity();
                        findAccount.UserID = admin.TraderID;
                        result = aa.AssetSummaryFind(findAccount, out outMessage);
                    }
                }
                else
                {
                    outMessage = "查询失败，交易员ID不能为空！";
                }
            }
            catch (Exception ex)
            {
                outMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        # region (NEW)查询指定资金账户的转出流水情况（只查转出帐户，防止数据重复）
        /// <summary>
        /// 查询指定资金账户的转出流水情况（只查转出帐户，防止数据重复）
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strErrorMessage">输出信息</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<UA_CapitalFlowTableInfo> TransferFlowFind(string userId, string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage)
        {
            count = 0;
            strErrorMessage = string.Empty;


            List<UA_CapitalFlowTableInfo> result = null;
            UA_CapitalFlowTableDal dal = new UA_CapitalFlowTableDal();
            //CapitalAndHoldFindBLL _CapitalAndHoldFind = new CapitalAndHoldFindBLL();
            try
            {
                if (!string.IsNullOrEmpty(capitalAccount)) //先判断是否输入了资金帐户
                {
                    #region 从缓存中获取账号对应的用户信息 通过资金账号获得该资金账号所对应的交易员ID
                    var userInfo = AccountManager.Instance.GetUserByAccount(capitalAccount);
                    //  _userID = _CapitalAndHoldFind.GetUserIdByTradeAccount(capitalAccount);
                    #endregion

                    //再判断“通过资金账号获得该资金账号所对应的交易员ID”与参数“输入的交易员ID”是否相同
                    //if (!string.IsNullOrEmpty(_userID) && userId == _userID)
                    if (userInfo != null && userId.Trim() == userInfo.UserID.Trim())
                    {
                        string whereCondition = string.Format(" FromCapitalAccount='{0}'", capitalAccount);
                        #region 分页存储过程相关信息组装
                        PagingProceduresInfo ppInfo = new PagingProceduresInfo();
                        ppInfo.IsCount = true;
                        ppInfo.PageNumber = start;
                        ppInfo.PageSize = pageLength;
                        ppInfo.Fields = "CapitalFlowLogo,FromCapitalAccount,ToCapitalAccount,TransferAmount,TransferTime,TradeCurrencyType,TransferTypeLogo ";
                        ppInfo.PK = "CapitalFlowLogo";
                        ppInfo.Sort = " TransferTime desc ";
                        ppInfo.Tables = "UA_CapitalFlowTable";
                        #endregion

                        #region 过滤条件组装
                        ppInfo.Filter = whereCondition;
                        #endregion
                        CommonDALOperate<UA_CapitalFlowTableInfo> com = new CommonDALOperate<UA_CapitalFlowTableInfo>();
                        result = com.PagingQueryProcedures(ppInfo, out count, dal.ReaderBind);
                        // result = DataRepository.UaCapitalFlowTableProvider.GetPaged(whereCondition, "CapitalFlowLogo ASC", start, pageLength, out count);
                    }
                    else
                        strErrorMessage = "查询失败！失败原因为：交易员ID或资金账号输入错误 ！";
                }
                else
                    strErrorMessage = "查询失败！失败原因为：没有输入资金账号 ！";
            }
            catch (Exception ex)
            {

                LogHelper.WriteError(ex.ToString(), ex);
            }

            return result;
        }
        # endregion

        #region  (NEW)管理员查询交易员各资金账户转账流水
        /// <summary>
        /// 管理员查询交易员各资金账户转账流水
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<UA_CapitalFlowTableInfo> AdminFindTraderCapitalAccountTransferFlow(AdministratorFindEntity admin, int start, int pageLength, out int count, out string strErrorMessage)
        {
            List<UA_CapitalFlowTableInfo> result = new List<UA_CapitalFlowTableInfo>();
            count = 0;
            strErrorMessage = string.Empty;
            //List<UaCapitalFlowTable> CapitalAccountTransferFlowFind(string userId, string capitalAccount, string userPassword, int start, int pageLength, out int count, out string strErrorMessage)
            //List<UA_CapitalFlowTableInfo> tempt = new List<UA_CapitalFlowTableInfo>();
            CapitalAndHoldFindBLL aa = new CapitalAndHoldFindBLL();
            //AccountManagementDAL bb = new AccountManagementDAL();
            strErrorMessage = string.Empty;
            //bool findResult = false;
            count = 0;
            try
            {
                // 验证管理员密码是否正确
                bool findResult = AdminIsExist(admin.AdministratorID, admin.AdministratorPassword, out strErrorMessage);

                if (findResult)// 若管理员密码正确
                {
                    #region 从缓存中获取相关账号
                    //查出该交易员的银行帐号
                    //string bankAccount = bb.Find_UserBankCapitalAccount(admin.TraderID);
                    var bankAccount = AccountManager.Instance.GetAccountByUserIDAndAccountType(admin.TraderID, (int)Types.AccountType.BankAccount);

                    //查出交易员所拥有的所有现货资金账号
                    //string[] XhCapitalAccount = bb.Find_UserXHCapitalAccount(admin.TraderID);
                    var XhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.SpotCapital);
                    //查出交易员所拥有的所有期货资金账号
                    // string[] QhCapitalAccount = bb.Find_UserQHCapitalAccount(admin.TraderID);
                    var QhCapitalAccount = AccountManager.Instance.GetAccountByUserIDAndAccountTypeClass(admin.TraderID, Types.AccountAttributionType.FuturesCapital);
                    #endregion

                    # region 银行资金账号转出流水情况
                    if (bankAccount != null)
                    {
                        //银行资金账号转出流水情况
                        var temptBank = TransferFlowFind(admin.TraderID, bankAccount.UserAccountDistributeLogo, "", start, pageLength, out count, out strErrorMessage);
                        if (!Utils.IsNullOrEmpty(temptBank))
                        {
                            result.AddRange(temptBank);
                        }
                        //if (tempt.Count > 0)//只有当该资金帐户有转出流水时才执行此代码块
                        //{
                        //    foreach (UA_CapitalFlowTableInfo _UaCapitalFlowTable in tempt)
                        //    {
                        //        result.Add(_UaCapitalFlowTable);
                        //    }
                        //}
                    }
                    # endregion

                    # region 现货资金帐户转出流水情况
                    //现货资金帐户转出流水情况
                    //if (XhCapitalAccount.Length > 0)
                    if (!Utils.IsNullOrEmpty(XhCapitalAccount))
                    {
                        foreach (var item in XhCapitalAccount)
                        {
                            var temptCap = TransferFlowFind(admin.TraderID, item.UserAccountDistributeLogo, "", start, pageLength, out count, out strErrorMessage);

                            if (!Utils.IsNullOrEmpty(temptCap))
                            {
                                result.AddRange(temptCap);
                            }
                            //if (tempt.Count > 0)//只有当该资金帐户有转出流水时才执行此代码块
                            //{
                            //    foreach (UA_CapitalFlowTableInfo _UaCapitalFlowTable in tempt)
                            //    {
                            //        result.Add(_UaCapitalFlowTable);
                            //    }
                            //}
                        }
                    }

                    # endregion

                    # region 期货资金帐户转出流水情况
                    //期货资金帐户转出流水情况
                    //if (QhCapitalAccount.Length > 0)
                    if (!Utils.IsNullOrEmpty(QhCapitalAccount))
                    {
                        foreach (var item in QhCapitalAccount)
                        {
                            var temptQh = TransferFlowFind(admin.TraderID, item.UserAccountDistributeLogo, "", start, pageLength, out count,
                                                      out strErrorMessage);
                            if (!Utils.IsNullOrEmpty(temptQh))
                            {
                                result.AddRange(temptQh);
                            }
                            //if (tempt.Count > 0) //只有当该资金帐户有转出流水时才执行此代码块
                            //{
                            //    foreach (UA_CapitalFlowTableInfo _UaCapitalFlowTable in tempt)
                            //    {
                            //        result.Add(_UaCapitalFlowTable);
                            //    }
                            //}
                        }
                    }
                    # endregion
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return result;
        }
        #endregion

        #region 管理员查询根据交易员查询交易员各资金账户相关信息
        /// <summary>
        /// Title:管理员查询根据交易员查询交易员各资金账户相关信息
        /// Create By:李健华
        /// Create Date:2009-11-02
        /// Desc: 增加了商品期货资金账号处理
        /// Update by:董鹏
        /// Update Date: 2010-01-22
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="traderId">交易员ID</param>
        /// <param name="strErrorMessage">查询相关异常信息</param>
        /// <returns></returns>
        public List<TradersAccountCapitalInfo> AdminFindTraderCapitalAccountInfoByID(string adminId, string adminPassword, string traderId, out string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            List<TradersAccountCapitalInfo> list = null;
            //bool findResult = AdminIsExist(adminId, adminPassword, out strErrorMessage);
            //if (!findResult)
            //{
            //    return list;
            //}

            try
            {
                #region 从缓存中获取交易员的所有账号信息
                List<UA_UserAccountAllocationTableInfo> userAccountList = AccountManager.Instance.GetUserAllAccounts(traderId);
                if (Utils.IsNullOrEmpty(userAccountList))
                {
                    strErrorMessage = "对不起，查询失败！失败原因为：查询不到该交易员的相应的账号信息 ！";
                    return list;
                }
                #endregion
                CapitalAndHoldFindBLL capitalFind = new CapitalAndHoldFindBLL();
                list = new List<TradersAccountCapitalInfo>();
                foreach (var item in userAccountList)
                {
                    switch ((Types.AccountType)item.AccountTypeLogo)
                    {
                        case Types.AccountType.BankAccount:
                            #region 获取数据银行账号
                            List<UA_BankAccountTableInfo> bank_list = null;
                            UA_BankAccountTableDal dal = new UA_BankAccountTableDal();
                            bank_list = dal.GetListArray(string.Format("UserAccountDistributeLogo ='{0}'", item.UserAccountDistributeLogo.Trim()));
                            if (Utils.IsNullOrEmpty(bank_list))
                            {
                                strErrorMessage = "对不起，查询失败！失败原因为：查询不到该交易员的没有相应的银行账号 ！";
                            }
                            else
                            {
                                foreach (var bankItem in bank_list)
                                {
                                    TradersAccountCapitalInfo bankCapitalInfo = new TradersAccountCapitalInfo();
                                    bankCapitalInfo.AccountID = bankItem.UserAccountDistributeLogo.Trim();
                                    bankCapitalInfo.AccountType = (int)Types.AccountType.BankAccount;
                                    bankCapitalInfo.AvailableCapital = bankItem.AvailableCapital;
                                    bankCapitalInfo.CurrencyType = (int)bankItem.TradeCurrencyTypeLogo;
                                    bankCapitalInfo.FreezeCapital = bankItem.FreezeCapital;
                                    bankCapitalInfo.InitCapital = 0;
                                    list.Add(bankCapitalInfo);
                                }
                            }
                            #endregion
                            break;
                        case Types.AccountType.StockSpotCapital:
                            #region 从缓存中获取交易员所拥有的所有现货资金账号
                            List<XH_CapitalAccountTableInfo> xh_capitalAccountList = capitalFind.QueryXH_CapitalAccountByAccount(item.UserAccountDistributeLogo, QueryType.QueryCurrencyType.ALL, out strErrorMessage);
                            if (Utils.IsNullOrEmpty(xh_capitalAccountList))
                            {
                                strErrorMessage = "对不起，查询失败！失败原因为：查询不到该交易员的没有相应的现货账号 ！";
                            }
                            else
                            {
                                foreach (var xhItem in xh_capitalAccountList)
                                {
                                    TradersAccountCapitalInfo xhCapitalInfo = new TradersAccountCapitalInfo();
                                    xhCapitalInfo.AccountID = xhItem.UserAccountDistributeLogo.Trim();
                                    xhCapitalInfo.AccountType = (int)Types.AccountType.StockSpotCapital;
                                    xhCapitalInfo.AvailableCapital = xhItem.AvailableCapital;
                                    xhCapitalInfo.CurrencyType = (int)xhItem.TradeCurrencyType;
                                    xhCapitalInfo.FreezeCapital = xhItem.FreezeCapitalTotal;
                                    xhCapitalInfo.InitCapital = 0;
                                    list.Add(xhCapitalInfo);
                                }
                            }
                            #endregion
                            break;
                        //case Types.AccountType.StockSpotHoldCode:
                        //    break;
                        case Types.AccountType.CommodityFuturesCapital:
                            #region 从缓存中获取交易员所拥有的所有商品期货资金账号 add by 董鹏 2010-01-22
                            List<QH_CapitalAccountTableInfo> cfqh_capitalAccountList = capitalFind.QueryQH_CapitalAccountByAccount(item.UserAccountDistributeLogo, QueryType.QueryCurrencyType.ALL, out strErrorMessage);
                            if (Utils.IsNullOrEmpty(cfqh_capitalAccountList))
                            {
                                strErrorMessage = "对不起，查询失败！失败原因为：查询不到该交易员的没有相应的期货账号 ！";
                            }
                            else
                            {
                                foreach (var qhItem in cfqh_capitalAccountList)
                                {
                                    TradersAccountCapitalInfo qhCapitalInfo = new TradersAccountCapitalInfo();
                                    qhCapitalInfo.AccountID = qhItem.UserAccountDistributeLogo.Trim();
                                    qhCapitalInfo.AccountType = (int)Types.AccountType.CommodityFuturesCapital;
                                    qhCapitalInfo.AvailableCapital = qhItem.AvailableCapital;
                                    qhCapitalInfo.CurrencyType = (int)qhItem.TradeCurrencyType;
                                    qhCapitalInfo.FreezeCapital = qhItem.FreezeCapitalTotal;
                                    qhCapitalInfo.InitCapital = 0;
                                    list.Add(qhCapitalInfo);
                                }
                            }
                            #endregion
                            break;
                        //case Types.AccountType.CommodityFuturesHoldCode:
                        //    break;
                        case Types.AccountType.StockFuturesCapital:
                            #region 从缓存中获取交易员所拥有的所有期货资金账号
                            List<QH_CapitalAccountTableInfo> qh_capitalAccountList = capitalFind.QueryQH_CapitalAccountByAccount(item.UserAccountDistributeLogo, QueryType.QueryCurrencyType.ALL, out strErrorMessage);
                            if (Utils.IsNullOrEmpty(qh_capitalAccountList))
                            {
                                strErrorMessage = "对不起，查询失败！失败原因为：查询不到该交易员的没有相应的期货账号 ！";
                            }
                            else
                            {
                                foreach (var qhItem in qh_capitalAccountList)
                                {
                                    TradersAccountCapitalInfo qhCapitalInfo = new TradersAccountCapitalInfo();
                                    qhCapitalInfo.AccountID = qhItem.UserAccountDistributeLogo.Trim();
                                    qhCapitalInfo.AccountType = (int)Types.AccountType.StockFuturesCapital;
                                    qhCapitalInfo.AvailableCapital = qhItem.AvailableCapital;
                                    qhCapitalInfo.CurrencyType = (int)qhItem.TradeCurrencyType;
                                    qhCapitalInfo.FreezeCapital = qhItem.FreezeCapitalTotal;
                                    qhCapitalInfo.InitCapital = 0;
                                    list.Add(qhCapitalInfo);
                                }
                            }
                            #endregion
                            break;
                        //case Types.AccountType.StockFuturesHoldCode:
                        //    break;
                        case Types.AccountType.HKSpotCapital:
                            #region 从缓存中获取交易员所拥有的所有港股资金账号
                            HKCapitalAndHoldQuery hkFind = new HKCapitalAndHoldQuery();
                            List<HK_CapitalAccountInfo> hk_capitalAccountList = hkFind.QueryHK_CapitalAccountByAccount(item.UserAccountDistributeLogo, QueryType.QueryCurrencyType.ALL, out strErrorMessage);
                            if (Utils.IsNullOrEmpty(hk_capitalAccountList))
                            {
                                strErrorMessage = "对不起，查询失败！失败原因为：查询不到该交易员的没有相应的港股账号 ！";
                            }
                            else
                            {
                                foreach (var hkItem in hk_capitalAccountList)
                                {
                                    TradersAccountCapitalInfo hkCapitalInfo = new TradersAccountCapitalInfo();
                                    hkCapitalInfo.AccountID = hkItem.UserAccountDistributeLogo.Trim();
                                    hkCapitalInfo.AccountType = (int)Types.AccountType.HKSpotCapital;
                                    hkCapitalInfo.AvailableCapital = hkItem.AvailableCapital;
                                    hkCapitalInfo.CurrencyType = (int)hkItem.TradeCurrencyType;
                                    hkCapitalInfo.FreezeCapital = hkItem.FreezeCapitalTotal;
                                    hkCapitalInfo.InitCapital = 0;
                                    list.Add(hkCapitalInfo);
                                }
                            }
                            #endregion
                            break;
                        //case Types.AccountType.HKSpotHoldCode:
                        //    break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
                return null;
            }
            return list;

        }
        #endregion
        //#region  【未实现方法待删除】管理员查询交易员现货资金账户交易费用流水
        ///// <summary>
        ///// 管理员查询交易员现货资金账户交易费用流水 
        ///// </summary>
        ///// <param name="admin"></param>
        ///// <returns></returns>
        //public List<SpotTradeFeeFlowEntity> AdminFindTraderSpotCapitalAccountTradeFeeFlow(AdministratorFindEntity admin)
        //{
        //    var _AdministratorFindTrader = new AdministratorFindTraderDAL();
        //    return _AdministratorFindTrader.AdminFindTraderSpotCapitalAccountTradeFeeFlow(admin);
        //    throw new NotImplementedException();
        //}
        //#endregion

        //#region 【未实现方法待删除】 管理员查询交易员期货资金账户交易费用流水
        ///// <summary>
        /////  管理员查询交易员期货资金账户交易费用流水
        ///// </summary>
        ///// <param name="admin"></param>
        ///// <returns></returns>
        //public List<FuturesTradeFeeFlowEntity> AdminFindTraderFuturesCapitalAccountTradeFeeFlow(AdministratorFindEntity admin)
        //{
        //    var _AdministratorFindTrader = new AdministratorFindTraderDAL();
        //    return _AdministratorFindTrader.AdminFindTraderFuturesCapitalAccountTradeFeeFlow(admin);
        //    throw new NotImplementedException();
        //}

        //#endregion
    }
}

