using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.AccountManagementAndFindDAL;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.MemoryData;
using ReckoningCounter.Model;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL
{
    /// <summary>
    /// 作用：柜台帐户管理（包括：非批量开户、批量开户、 非批量销户、批量销户、冻结账户、 解冻账户、查询账户、修改密码）
    /// 作者：李科恒
    /// 日期：2008-11-29
    /// Update By:李健华
    /// Update Date:2009-07-20
    /// Desc.:修改之前的调用DAL操作方法如事务，以及删除一些已经注释掉的方法我语句
    ///       修改方法操作成功后不返回OutMessage任何信息，直接附值Null/""
    /// Update By:李健华
    /// Update Date:2009-10-19
    /// Desc.:把AccountManagementDAL.cs中的冻结或者解冻账户方法移到本类中把原来的AccountManagementDAL.cs的删除
    /// </summary>
    public class AccountManagementBLL
    {

        # region (NEW)单个交易员开户
        /// <summary>
        /// 单个开户
        /// </summary>
        /// <param name="accounts">帐户实体列表</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool SingleTraderOpenAccount(List<AccountEntity> accounts, out string outMessage)
        {
            outMessage = string.Empty;
            Database db = DatabaseFactory.CreateDatabase();
            bool IsOpenSuccess = true;
            # region try ....catch 语句
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trm = conn.BeginTransaction();
                try
                {
                    TraderAccountManagement tam = new TraderAccountManagement();
                    //写交易员基本信息表
                    if (!tam.AddUserToUserBasicInformation(accounts[0], out outMessage, db, trm))
                    {
                        trm.Rollback();
                        return false;
                    }
                    foreach (AccountEntity _account in accounts)
                    {
                        //写帐户信息表
                        if (!tam.AddTraderAccountToUserAccountAllocation(_account, out outMessage, db, trm))
                        {
                            IsOpenSuccess = false;
                            break;
                        }
                        //初始资金帐户
                        if (!tam.AccountInformationInsertToRelevantAccount(_account, out outMessage, db, trm))
                        {
                            IsOpenSuccess = false;
                            break;
                        }
                    }
                    if (!IsOpenSuccess)
                    {
                        trm.Rollback();
                        return false;
                    }
                    outMessage = "";
                    trm.Commit();
                    //IsOpenSuccess = true;
                }
                catch (Exception ex)
                {
                    outMessage = ex.Message.ToString();
                    LogHelper.WriteError(ex.ToString(), ex);
                    trm.Rollback();
                    return false;
                }
                finally
                {
                    trm.Dispose();
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                #region 以上成功后向缓存数据表中初始化相关数据
                List<AccountEntity> mList = new List<AccountEntity>();
                mList.Add(accounts[0]);
                UpdateUserBasicInfoMemoryList(mList);
                UpdateUserAccountMemoryList(accounts);
                UpdateCapitalAccountMemoryList(accounts);
                #endregion
                return true;
            }
            # endregion
        }
        # endregion

        # region (NEW)批量开户
        /// <summary>
        /// 批量开户      
        /// </summary>
        /// <param name="accounts">帐户实体列表</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool VolumeTraderOpenAccount(List<AccountEntity> accounts, out string outMessage)
        {
            outMessage = "";
            Database db = DatabaseFactory.CreateDatabase();

            # region try....catch语句
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction tm = conn.BeginTransaction();
                bool IsOpenSuccess = true;
                //更新用户内存数据
                List<AccountEntity> mList = new List<AccountEntity>();
                try
                {
                    //交易员ID
                    string _traderID = string.Empty;
                    //临时交易员ID
                    string _tempTraderID = string.Empty;
                    //===================================== modity by xiongxl   ====================================



                    TraderAccountManagement tam = new TraderAccountManagement();
                    foreach (AccountEntity _account in accounts)
                    {
                        if (_account.TraderID != _tempTraderID)
                        {
                            //写交易员基本信息表
                            if (!tam.AddUserToUserBasicInformation(_account, out outMessage, db, tm))
                            {
                                tm.Rollback();
                                return false;
                            }
                            mList.Add(_account);
                        }
                        //写帐户信息表
                        if (!tam.AddTraderAccountToUserAccountAllocation(_account, out outMessage, db, tm))
                        {
                            IsOpenSuccess = false;
                            break;
                        }
                        //初始资金帐户
                        if (!tam.AccountInformationInsertToRelevantAccount(_account, out outMessage, db, tm))
                        {
                            IsOpenSuccess = false;
                            break;
                        }

                        _tempTraderID = _account.TraderID;
                    }
                    if (!IsOpenSuccess)
                    {
                        tm.Rollback();
                        return false;
                    }

                    //==========================================end================================================

                    //outMessage = "开户成功！";
                    outMessage = "";
                    tm.Commit();

                }
                catch (Exception ex)
                {
                    outMessage = ex.Message.ToString();
                    LogHelper.WriteError(ex.ToString(), ex);
                    tm.Rollback();
                    return false;
                }
                finally
                {
                    tm.Dispose();
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                #region 以上成功后向缓存数据表中初始化相关数据
                UpdateUserBasicInfoMemoryList(mList);
                UpdateUserAccountMemoryList(accounts);
                UpdateCapitalAccountMemoryList(accounts);
                #endregion
                return true;
            }
            # endregion
        }
        # endregion

        # region （NEW）单个销户
        /// <summary>
        ///  单个销户
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool DeleteSingleTraderAccount(string userId, out string outMessage)
        {
            outMessage = string.Empty;
            return true;

            //TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            //# region try.....catch语句
            //try
            //{
            //    bool result = false;
            //    var _findAccount = new FindAccountEntity();
            //    _findAccount.UserID = userId;
            //    var _AccountManagement = new AccountManagementDAL();
            //    result = _AccountManagement.DeleteSingleTraderAccount(_findAccount, out outMessage);
            //    if(result)
            //    {
            //        outMessage = "恭喜您，单个销户成功！";
            //        return true;
            //    }
            //    tm.Commit();
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    outMessage = "对不起，单个销户失败，失败原因为：" + ex;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //    tm.Rollback();
            //    return false;
            //}
            //# endregion
        }
        # endregion

        # region  （NEW）批量销户
        /// <summary>
        /// 批量销户
        /// </summary>
        /// <param name="userIDs">交易员ID</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool DeleteVolumeTraderAccount(string[] userIDs, out string outMessage)
        {
            outMessage = string.Empty;
            return true;

            //TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();
            //# region try.....catch语句
            //try
            //{
            //    outMessage = string.Empty;
            //    var _AccountManagement = new AccountManagementDAL();
            //    for (int i = 0; i <= userIDs.Length; i++)
            //    {
            //        //调用BLL层的单个销户方法
            //        DeleteSingleTraderAccount(userIDs[i], out  outMessage);
            //    }    
            //    outMessage = "恭喜您，批量销户成功！";
            //    tm.Commit();
            //    return true; 
            //}
            //catch (Exception ex)
            //{
            //    outMessage = "对不起，批量销户失败，失败原因为：" + ex;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //    tm.Rollback();
            //    return false;
            //}
            //# endregion try.....catch语句
        }
        # endregion

        # region  (NEW)冻结账户
        /// <summary>
        /// 冻结账户
        /// </summary>
        /// <param name="accounts">查询帐户对象列表</param>
        /// <param name="outMessage">输出信息</param>
        /// <returns></returns>
        public bool FreezeAccount(List<FindAccountEntity> accounts, out string outMessage)
        {
            # region try.....catch语句

            outMessage = string.Empty;
            try
            {
                bool result = false;
                # region 先判断准备冻结的帐户中有没有银行帐户
                foreach (FindAccountEntity _FindAccount in accounts)
                {
                    if (_FindAccount.AccountType == 1)
                    {
                        outMessage = "银行账户不能被冻结！";
                        return false;
                    }
                }
                # endregion

                //AccountManagementDAL amDal = new AccountManagementDAL();
                DataManager.ExecuteInTransaction((db, tran) =>
                {
                    // result = amDal.FreezeAccount(accounts, db, tran);
                    result = FreezeAccount(accounts, db, tran);
                });
                //result = amDal.FreezeAccount(accounts);
                //更新内存表中数据状态
                if (result)
                {
                    foreach (var item in accounts)
                    {
                        AccountManager.Instance.UpdateUser(item.UserID, item.AccountType, false);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
            //AccountManager.Instance.AddUser();
            # endregion
        }
        # endregion

        # region  (NEW)解冻账户
        /// <summary>
        /// 解冻账户
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public bool ThawAccount(List<FindAccountEntity> accounts, out string outMessage)
        {
            # region try.....catch 语句

            outMessage = string.Empty;
            try
            {
                bool result = false;

                # region 先判断准备冻结的帐户中有没有银行帐户
                foreach (FindAccountEntity _FindAccount in accounts)
                {
                    if (_FindAccount.AccountType == 1)
                    {
                        outMessage = "银行账户不能执行冻结和解冻操作！";
                        return false;
                    }
                }
                # endregion 先判断准备冻结的帐户中有没有银行帐户

                //AccountManagementDAL amDal = new AccountManagementDAL();
                DataManager.ExecuteInTransaction((db, tran) =>
                {
                    // result = amDal.ThawAccount(accounts, db, tran);
                    result = ThawAccount(accounts, db, tran);
                });
                //更新内存表中数据状态
                if (result)
                {
                    foreach (var item in accounts)
                    {
                        AccountManager.Instance.UpdateUser(item.UserID, item.AccountType, true);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
            # endregion
        }
        # endregion

        # region （NEW）查询账户
        /// <summary>
        ///  查询账户
        ///  Update BY:李健华
        ///  Update Date:2009-09-22
        ///  Desc.:修改使用查询的方法把AccountManagementDAL的方法去掉直接调用UA_UserAccountAllocationTableDal中的方法
        /// </summary>
        /// <param name="findAccount"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public List<AccountFindResultEntity> FindAccount(FindAccountEntity findAccount, out string outMessage)
        {
            List<AccountFindResultEntity> list = null;
            outMessage = string.Empty;
            // AccountManagementDAL amdal = new AccountManagementDAL();
            try
            {
                if (findAccount == null)
                {
                    outMessage = "查询失败，查询帐户对象为空！";
                    return list;
                }
                UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                list = dal.GetUserAccountByFindFilter(findAccount);
                // list = amdal.FindAccount(findAccount, out outMessage);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        # endregion

        # region （NEW）查询交易权限（OK）
        /// <summary>
        ///  查询交易权限
        /// </summary>
        /// <param name="findAccount"></param>
        /// <param name="outMessage"></param>
        /// <returns></returns>
        public List<CM_BreedClass> FindTradePrivileges(FindAccountEntity findAccount, out string outMessage)
        {
            List<CM_BreedClass> result = null;
            CM_BreedClass tempt = null;
            IList<UM_DealerTradeBreedClass> tempt2 = new List<UM_DealerTradeBreedClass>();
            outMessage = string.Empty;
            try
            {
                tempt2 = MCService.CommonPara.GetTransactionRightTable(Convert.ToInt32(findAccount.UserID));
                if (tempt2.Count > 0)
                {
                    foreach (UM_DealerTradeBreedClass _UM_DealerTradeBreedClass in tempt2)
                    {
                        tempt = MCService.CommonPara.GetBreedClassByBreedClassID(_UM_DealerTradeBreedClass.BreedClassID.Value);
                        if (tempt.BreedClassName != null)
                            result.Add(tempt);
                    }
                }
            }
            catch (Exception ex)
            {
                outMessage = ex.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return result;
        }
        # endregion

        #region (NEW)修改用户密码
        /// <summary>
        /// 修改用户密码
        /// Update BY:李健华
        /// Update Date:2009-07-20
        /// Desc.:修改操作相关的业务操作逻辑直接更新密码
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="oldPassword">原始密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="outMessage">输出信息</param>
        public bool UpdateUserPassword(string userID, string oldPassword, string newPassword, out string outMessage)
        {
            outMessage = string.Empty;
            try
            {
                UA_UserBasicInformationTableDal dal = new UA_UserBasicInformationTableDal();
                if (dal.Exists(userID, oldPassword))
                {
                    dal.UpdatePwdByUserID(newPassword, userID);
                    AccountManager.Instance.UpdateUserPWD(userID, newPassword);//更新缓存中的密码
                    return true;
                }
                else
                {
                    outMessage = "查询不到该用户信息或者用户密码不正确！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                outMessage = ex.Message.ToString();
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }
        # endregion

        #region 开户成功后更新内存表数据 Create By:李健华 Create Date:2009-08-11
        #region 开户成功后更新各资金账户内存表数据
        /// <summary>
        /// 开户成功后更新各资金账户内存表数据
        /// </summary>
        /// <param name="accounts">帐户实体列表</param>
        public void UpdateCapitalAccountMemoryList(List<AccountEntity> accounts)
        {
            try
            {
                foreach (AccountEntity item in accounts)
                {
                    #region Old 因港股不也现货账户一起所以不能这样去判断初始化
                    //int accountType = Convert.ToInt32(item.AccountAAttribution);
                    //switch (accountType)
                    //{
                    //    case (int)CommonObject.Types.AccountAttributionType.SpotCapital:
                    //        //初始化现货资金账户
                    //        InitializeXHCapitalMemoryList(item);
                    //        break;

                    //    case (int)CommonObject.Types.AccountAttributionType.SpotHold:
                    //        break;

                    //    case (int)CommonObject.Types.AccountAttributionType.FuturesCapital:
                    //        //初始化期货资金账户
                    //        InitializeQHCapitalMemoryList(item);
                    //        break;

                    //    case (int)CommonObject.Types.AccountAttributionType.FuturesHold:
                    //        break;

                    //    case (int)CommonObject.Types.AccountAttributionType.BankAccount:
                    //        //初始化银行资金账户
                    //        break;
                    //}
                    #endregion

                    switch ((Types.AccountType)item.AccountType)
                    {
                        case Types.AccountType.BankAccount:
                            break;
                        case Types.AccountType.StockSpotCapital:
                            //初始化现货资金账户
                            InitializeXHCapitalMemoryList(item);
                            break;
                        case Types.AccountType.StockSpotHoldCode:
                            break;
                        case Types.AccountType.CommodityFuturesCapital:
                            //初始化期货资金账户(商品期货）
                            InitializeQHCapitalMemoryList(item);
                            break;
                        case Types.AccountType.CommodityFuturesHoldCode:
                            break;
                        case Types.AccountType.StockFuturesCapital:
                            //初始化期货资金账户（股指期货)
                            InitializeQHCapitalMemoryList(item);
                            break;
                        case Types.AccountType.StockFuturesHoldCode:
                            break;
                        case Types.AccountType.HKSpotCapital:
                            //初始化港股资金账户
                            InitializeHKCapitalMemoryList(item);
                            break;
                        case Types.AccountType.HKSpotHoldCode:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("开户成功后更新各资金账户内存表数据异常", ex);
            }
        }
        #endregion

        #region 更新用户基本信息表内存数据
        /// <summary>
        /// 更新用户基本信息表内存数据
        /// </summary>
        /// <param name="accounts"></param>
        private void UpdateUserBasicInfoMemoryList(List<AccountEntity> accounts)
        {
            try
            {
                foreach (var item in accounts)
                {
                    UA_UserBasicInformationTableInfo model = new UA_UserBasicInformationTableInfo();
                    model.UserID = item.TraderID;
                    model.Password = item.TraderPassWord;
                    model.RoleNumber = item.RoleNumber;
                    AccountManager.Instance.AddUserBasicInfo(model);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("开户成功后初始化用户基本表信息数据异常", ex);
            }
        }
        #endregion

        #region 更新用户账号信息表内存数据
        /// <summary>
        /// 更新用户账号信息表内存数据
        /// </summary>
        /// <param name="accounts"></param>
        private void UpdateUserAccountMemoryList(List<AccountEntity> accounts)
        {
            try
            {
                foreach (var item in accounts)
                {
                    UA_UserAccountAllocationTableInfo model = new UA_UserAccountAllocationTableInfo();
                    model.UserAccountDistributeLogo = item.Account;
                    model.UserID = item.TraderID;
                    model.WhetherAvailable = true;
                    model.AccountTypeLogo = Convert.ToInt32(item.AccountType);
                    AccountManager.Instance.AddUser(model);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("开户成功后初始化用户账号表信息数据异常", ex);
            }
        }
        #endregion

        #region  初始化现货资金账户缓存
        /// <summary>
        /// 初始化现货资金账户缓存
        /// </summary>
        private void InitializeXHCapitalMemoryList(AccountEntity account)
        {
            if (account == null)
            {
                return;
            }
            #region  *************************
            //XH_CapitalAccountTableInfo model = new XH_CapitalAccountTableInfo();
            //model.AvailableCapital = 0;
            //model.BalanceOfTheDay = 0;
            //model.FreezeCapitalTotal = 0;
            //model.TodayOutInCapital = 0;
            //model.UserAccountDistributeLogo = account.Account;
            //model.CapitalBalance = 0;
            //model.TradeCurrencyType = (int)CommonObject.Types.CurrencyType.RMB;
            //MemoryDataManager.XHCapitalMemoryList.AddXHCapitalTable(model);
            //model.TradeCurrencyType = (int)CommonObject.Types.CurrencyType.HK;
            //MemoryDataManager.XHCapitalMemoryList.AddXHCapitalTable(model);
            //model.TradeCurrencyType = (int)CommonObject.Types.CurrencyType.US;
            //MemoryDataManager.XHCapitalMemoryList.AddXHCapitalTable(model);
            #endregion    *****************

            XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            List<XH_CapitalAccountTableInfo> list = dal.GetListArray(" UserAccountDistributeLogo='" + account.Account + "'");
            foreach (var item in list)
            {
                MemoryDataManager.XHCapitalMemoryList.AddXHCapitalTable(item);
            }
        }

        #endregion

        #region 初始化期货资金账户缓存

        /// <summary>
        /// 初始化期货资金账户缓存
        /// </summary>
        private void InitializeQHCapitalMemoryList(AccountEntity account)
        {
            if (account == null)
            {
                return;
            }
            #region ==============
            //QH_CapitalAccountTableInfo model = new QH_CapitalAccountTableInfo();
            //model.AvailableCapital = 0;
            //model.BalanceOfTheDay = 0;
            //model.CapitalBalance = 0;
            //model.FreezeCapitalTotal = 0;
            //model.TodayOutInCapital = 0;
            //model.UserAccountDistributeLogo = account.Account;
            //model.TradeCurrencyType = (int)CommonObject.Types.CurrencyType.RMB;
            //MemoryDataManager.QHCapitalMemoryList.AddQHCapitalTable(model);
            //model.TradeCurrencyType = (int)CommonObject.Types.CurrencyType.HK;
            //MemoryDataManager.QHCapitalMemoryList.AddQHCapitalTable(model);
            //model.TradeCurrencyType = (int)CommonObject.Types.CurrencyType.US;
            //MemoryDataManager.QHCapitalMemoryList.AddQHCapitalTable(model);
            #endregion =================
            QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            List<QH_CapitalAccountTableInfo> list = dal.GetListArray(" UserAccountDistributeLogo='" + account.Account + "'");
            foreach (var item in list)
            {
                MemoryDataManager.QHCapitalMemoryList.AddQHCapitalTable(item);
            }
        }

        #endregion

        #region  初始化港股资金账户缓存
        /// <summary>
        /// 初始化港股资金账户缓存
        /// </summary>
        private void InitializeHKCapitalMemoryList(AccountEntity account)
        {
            if (account == null)
            {
                return;
            }

            HK_CapitalAccountDal dal = new HK_CapitalAccountDal();
            List<HK_CapitalAccountInfo> list = dal.GetListArray(" UserAccountDistributeLogo='" + account.Account + "'");
            foreach (var item in list)
            {
                MemoryDataManager.HKCapitalMemoryList.AddCapitalTable(item);
            }
        }

        #endregion
        #endregion

        #region 冻结或者解冻账户
        /// <summary>
        /// 冻结或者解冻账户
        /// Create by:李健华
        /// Create date:2009-07-19 
        /// Desc.:合并之前的冻结或者解冻两个方法
        /// </summary>
        /// <param name="list">要冻结或者解冻账户实体列表</param>
        /// <param name="db">数据操作句柄</param>
        /// <param name="tran">开启事务对象，如果为null不开启</param>
        /// <param name="state">冻结(false)-不可用或者解冻(true)-可用</param>
        /// <returns></returns>
        private bool FreezeOrThawAccount(List<FindAccountEntity> list, Database db, DbTransaction tran, bool state)
        {
            try
            {
                //这里要理解list中的实体内容是不是只包含一个用户ID的内容
                //如果是这里可以实现批理提交更新不用单条更新
                foreach (FindAccountEntity model in list)
                {
                    UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                    dal.Update(model.UserID, model.AccountType, state, db, tran);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }


        #region  冻结账户
        /// <summary>
        /// 冻结账户 外部调用时记得要对事务的commint或者RollBack
        /// </summary>
        /// <param name="accounts">查询账户实体列表</param>
        /// <param name="db">数据操作句柄</param>
        /// <param name="tran">开启事务对象，如果为null不开启</param>
        /// <returns></returns>
        public bool FreezeAccount(List<FindAccountEntity> accounts, Database db, DbTransaction tran)
        {
            return FreezeOrThawAccount(accounts, db, tran, false);
        }
        #endregion

        #region 解冻账户
        /// <summary>
        ///解冻账户
        /// </summary>
        /// <param name="accounts">查询账户实体列表</param>
        /// <returns></returns>
        public bool ThawAccount(List<FindAccountEntity> accounts, Database db, DbTransaction tran)
        {
            return FreezeOrThawAccount(accounts, db, tran, true);
        }
        #endregion
        #endregion
    }
}