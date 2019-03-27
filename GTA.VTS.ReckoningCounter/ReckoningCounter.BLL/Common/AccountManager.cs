#region Using Namespace

using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Model;
using System.Configuration;
using System;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 账户缓存管理器
    /// 作者：宋涛
    /// </summary>
    public class AccountManager
    {
        /// <summary>
        /// 账号管理类静态单列
        /// </summary>
        public static AccountManager Instance = new AccountManager();

        #region List
        ///// <summary>
        /////   用户账号信息列表
        ///// </summary>
        //private List<UA_UserAccountAllocationTableInfo> userList;
        ///// <summary>
        ///// 用户基本信息列表
        ///// </summary>
        //private List<UA_UserBasicInformationTableInfo> basicUserList;
        /// <summary>
        ///  账号类型列表
        /// </summary>
        private List<BD_AccountTypeInfo> accountTypeLsit;

        #endregion

        #region Cache
        /// <summary>
        /// 根据用户账号缓存相关账号每一个账号ID为Key
        /// </summary>
        private readonly SyncCache<string, UA_UserAccountAllocationTableInfo> accountCache = new SyncCache<string, UA_UserAccountAllocationTableInfo>();

        /// <summary>
        /// 根据每个用户ID为Key缓存当前用户所有的相关所有账号（包括港股、期货、现货资金\持仓账号）
        /// </summary>
        private SyncCache<string, SyncList<UA_UserAccountAllocationTableInfo>> userCache = new SyncCache<string, SyncList<UA_UserAccountAllocationTableInfo>>();
        /// <summary>
        /// 缓存每个用户的基本信息
        /// </summary>
        private SyncCache<string, UA_UserBasicInformationTableInfo> basicUserCahce = new SyncCache<string, UA_UserBasicInformationTableInfo>();

        #endregion

        #region DAL

        private readonly BD_AccountTypeDal bdAccountTypeDal = new BD_AccountTypeDal();
        private readonly UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();
        private readonly UA_UserBasicInformationTableDal basicUserDal = new UA_UserBasicInformationTableDal();

        #endregion

        private Dictionary<int, BD_AccountTypeInfo> accountTypeDic = new Dictionary<int, BD_AccountTypeInfo>();

        private AccountManager()
        {
        }

        private void Initialize()
        {
            //如果不全部加载设置就在每次调用的时候再加载
            if (GetLoadConfig())
            {
                List<UA_UserAccountAllocationTableInfo> userList = userDal.GetAll();

                foreach (var user in userList)
                {
                    AddUser(user);
                }

                List<UA_UserBasicInformationTableInfo> basicUserList = basicUserDal.GetAll();
                foreach (var basicUser in basicUserList)
                {
                    basicUserCahce.Add(basicUser.UserID, basicUser);
                }
            }

            //不管以后修改是否初始化缓存都以缓存以下数据
            accountTypeLsit = bdAccountTypeDal.GetAll();
            foreach (var accountTypeInfo in accountTypeLsit)
            {
                accountTypeDic[accountTypeInfo.AccountTypeLogo] = accountTypeInfo;
            }
        }
        /// <summary>
        /// 重置账号管理实体
        /// </summary>
        public void Reset()
        {
            lock (this)
            {
                //if (userList != null)
                //    userList.Clear();
                userCache.Reset();

                //if (basicUserList != null)
                //    basicUserList.Clear();
                basicUserCahce.Reset();

                accountTypeDic.Clear();
                accountCache.Reset();

                try
                {
                    Initialize();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }
        /// <summary>
        /// Create By:李健华
        /// Create Date:2009-08-12
        /// Title:更新缓存表中的数据用户账号状态
        /// Desc.:因为用户账户信息主要有的是解冻和冻结状态的改变所以这里的更新方法主要是对
        ///       解冻和冻结的更新
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型</param>
        /// <param name="state">状态</param>
        public void UpdateUser(string userID, int accountType, bool state)
        {
            UA_UserAccountAllocationTableInfo ua = GetAccountByUserIDAndAccountType(userID, accountType);
            if (ua != null)
            {
                ua.WhetherAvailable = state;
                accountCache.Delete(ua.UserAccountDistributeLogo);
                accountCache.Add(ua.UserAccountDistributeLogo, ua);
            }
            if (userCache.Contains(userID))
            {
                var accountList = userCache.Get(userID).GetAll();
                foreach (var item in accountList)
                {
                    if (item.AccountTypeLogo == accountType)
                    {
                        item.WhetherAvailable = state;
                    }
                }
            }
        }

        /// <summary>
        /// 向缓存数据添加一条用户账号信息记录
        /// </summary>
        /// <param name="user">要添加的用户账号实体</param>
        public void AddUser(UA_UserAccountAllocationTableInfo user)
        {
            try
            {
                if (!accountCache.Contains(user.UserAccountDistributeLogo))
                {
                    accountCache.Add(user.UserAccountDistributeLogo, user);
                    LogHelper.WriteDebug("添加一条账号信息记录当前用户ID：" + user.UserID + " 添加账号：" + user.UserAccountDistributeLogo + " 账号列表数：" + accountCache.GetAll().Count);
                }

                SyncList<UA_UserAccountAllocationTableInfo> accountList = null;
                if (userCache.Contains(user.UserID))
                {
                    accountList = userCache.Get(user.UserID);
                }
                else
                {
                    accountList = new SyncList<UA_UserAccountAllocationTableInfo>();
                    userCache.Add(user.UserID, accountList);
                    LogHelper.WriteDebug("【第一次】添加一条【用户拥有账号记录】ID：" + user.UserID + " 添加账号：" + user.UserAccountDistributeLogo + "  账号类型： " + user.AccountTypeLogo + " 账号列表数：" + accountList.GetAll().Count);
                }
                //如果存在不再添加  这样判断好象有问题用forearch
                //if (!accountList.Contains(user))
                //{
                bool isAdd = true;
                List<UA_UserAccountAllocationTableInfo> models = accountList.GetAll();
                foreach (var item in accountList.GetAll())
                {
                    if (item.AccountTypeLogo == user.AccountTypeLogo
                        && item.UserAccountDistributeLogo == user.UserAccountDistributeLogo
                        && item.UserID == user.UserID)
                    {
                        isAdd = false;
                        break;
                    }
                }
                if (isAdd)
                {
                    accountList.Add(user);
                    LogHelper.WriteDebug("添加一条【用户拥有账号记录】ID：" + user.UserID + " 添加账号：" + user.UserAccountDistributeLogo + "  账号类型： " + user.AccountTypeLogo + " 账号列表数：" + accountList.GetAll().Count);
                }
                //}

            }
            catch (Exception ex)
            {
                LogHelper.WriteError("向缓存数据添加一条用户账号信息记录异常" + ex.Message, ex);
            }
        }
        /// <summary>
        /// 获取加载配置信息
        /// </summary>
        /// <returns></returns>
        protected bool GetLoadConfig()
        {
            bool result = true;
            string key = "loadAllData";
            try
            {
                string str = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.Trim() == "2")
                        result = false;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 向缓存数据添加一条用户基本信息记录
        /// </summary>
        /// <param name="userBasicInfo">要添加的用户基本信息实体</param>
        public void AddUserBasicInfo(UA_UserBasicInformationTableInfo userBasicInfo)
        {
            #region old code update 2009-11-25 李健华
            ////object obj = new object();
            ////lock (obj)
            ////{
            //if (basicUserCahce.Contains(userBasicInfo.UserID))
            //{

            //    basicUserCahce.Add(userBasicInfo.UserID, userBasicInfo);
            //    basicUserList.Add(userBasicInfo);
            //}
            ////}
            #endregion
            if (basicUserCahce.Contains(userBasicInfo.UserID))
            {
                basicUserCahce.Delete(userBasicInfo.UserID);
                //basicUserList.Remove(userBasicInfo);

                basicUserCahce.Add(userBasicInfo.UserID, userBasicInfo);
                //basicUserList.Add(userBasicInfo);
            }
            else
            {
                basicUserCahce.Add(userBasicInfo.UserID, userBasicInfo);
                //basicUserList.Add(userBasicInfo);
            }
        }

        #region 用户基本信息
        /// <summary>
        /// Create By:李健华
        /// Create Date:2010-05-21
        /// Title:更新缓存表中的数据用户账号密码
        /// Desc.:因为用户账户修改密码后如果缓存中不更新会出现不能同步查询一样
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="newPwd">新密码</param>
        public void UpdateUserPWD(string userID, string newPwd)
        {
            UA_UserBasicInformationTableInfo ua = GetBasicUserByUserId(userID);
            if (ua != null)
            {
                ua.Password = newPwd;
            }
        }
        /// <summary>
        /// 根据用户ID获取基础用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UA_UserBasicInformationTableInfo GetBasicUserByUserId(string userId)
        {
            if (basicUserCahce.Contains(userId))
                return basicUserCahce.Get(userId);

            UA_UserBasicInformationTableInfo basicUser = null;

            try
            {
                basicUser = basicUserDal.GetModel(userId);
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (basicUser != null)
            {
                AddUserBasicInfo(basicUser);
                //basicUserCahce.Add(userId, basicUser);
                //basicUserList.Add(basicUser);
            }

            return basicUser;
        }

        /// <summary>
        /// 根据用户ID，判断密码是否正确
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public bool ExistsBasicUserPWDByUserId(string userId, string pwd)
        {
            UA_UserBasicInformationTableInfo basicUser = null;
            if (basicUserCahce.Contains(userId))
            {
                basicUser = basicUserCahce.Get(userId);
            }
            if (basicUser == null)
            {
                try
                {
                    basicUser = basicUserDal.GetModel(userId);
                }
                catch (System.Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }

                if (basicUser != null)
                {
                    AddUserBasicInfo(basicUser);
                }
            }
            if (basicUser != null)
            {
                if (basicUser.Password == pwd)
                {
                    return true;
                }
            }

            return false;

        }

        ///// <summary>
        ///// 获取所有的用户基础信息对象 
        ///// 注意：因为这里可能是没有全部加载的所以不一定是返回与数据库同步的所有用户列表所以这里注释此方法--  注释 --李健华 2010-06-09
        ///// </summary>
        ///// <returns></returns>
        //public List<UA_UserBasicInformationTableInfo> GetAllBasicUsers()
        //{
        //    return basicUserList;
        //}
        #endregion

        #region 用户账号信息
        /// <summary>
        /// 根据任意账户获取账户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public UA_UserAccountAllocationTableInfo GetUserByAccount(string account)
        {

            try
            {
                account = account.Trim();
                if (accountCache.Contains(account))
                {
                    return accountCache.Get(account);
                }

                string format = "UserAccountDistributeLogo='{0}'";
                string where = string.Format(format, account);
                var userList = userDal.GetModel(account);
                if (userList != null)
                {
                    //var user = userList[0];
                    // AddUser(user);
                    #region 附值返回
                    UA_UserAccountAllocationTableInfo model = new UA_UserAccountAllocationTableInfo();
                    model.AccountTypeLogo = userList.AccountTypeLogo;
                    model.UserAccountDistributeLogo = userList.UserAccountDistributeLogo;
                    model.UserID = userList.UserID;
                    model.WhetherAvailable = userList.WhetherAvailable;
                    #endregion

                    AddUser(userList);
                    return model;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("根据任意账户获取账户信息" + ex.Message, ex);
            }

            return null;
        }

        /// <summary>
        /// 获取一个用户的所有账户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<UA_UserAccountAllocationTableInfo> GetUserAllAccounts(string userID)
        {
            try
            {
                if (userCache.Contains(userID))
                {
                    //如果缓存的数据没有够账号类型的列表数那么从新在数据库中获取再缓存过
                    //这是因为防止先根据账号查询而缓存先的数据就会出现资金账号不存在的问题
                    List<UA_UserAccountAllocationTableInfo> returList = userCache.Get(userID).GetAll();
                    if (returList.Count == accountTypeLsit.Count)
                    {
                        return returList;
                    }
                }

                string format = "UserID='{0}'";
                string where = string.Format(format, userID);
                var userList = userDal.GetListArray(where);
                string wiritStr = "获取一个用户ID获取的所有账户并加入缓存中" + where;
                if (!Utils.IsNullOrEmpty(userList))
                {
                    wiritStr += "获取记录数：" + userList.Count;
                    foreach (var user in userList)
                    {
                        AddUser(user);
                    }
                    return userList;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return null;
        }

        /// <summary>
        /// 根据用户的资金账户返回关联的用户持仓账户信息(也即根据相关账号查询返回相关联的账号)
        /// </summary>
        /// <param name="capitalAccount">资金账户</param>
        /// <returns></returns>
        public UA_UserAccountAllocationTableInfo GetHoldAccountByCapitalAccount(string capitalAccount)
        {
            var user = GetUserByAccount(capitalAccount);

            if (user == null)
                return null;

            var capitalAccountType = user.AccountTypeLogo;

            int holdAccountType = -1;

            if (!accountTypeDic.ContainsKey(capitalAccountType))
            {
                return null;
            }

            var accountTypeInfo = accountTypeDic[capitalAccountType];

            if (accountTypeInfo.AccountTypeLogo == capitalAccountType)
            {
                //如果是银行账号没有关联的持仓账号
                if (accountTypeInfo.ATCId.Value == (int)Types.AccountAttributionType.BankAccount)
                {
                    return null;
                }

                if (accountTypeInfo.ATCId.Value == (int)Types.AccountAttributionType.SpotHold ||
                    accountTypeInfo.ATCId.Value == (int)Types.AccountAttributionType.FuturesHold)
                {
                    return user;
                }

                //这里如果没有关联的返回为null不再查找
                if (!accountTypeInfo.RelationAccountId.HasValue)
                {
                    return null;
                }
                holdAccountType = accountTypeInfo.RelationAccountId.Value;

            }


            var useList = GetUserAllAccounts(user.UserID);
            if (Utils.IsNullOrEmpty(useList))
                return null;

            foreach (var user2 in useList)
            {
                if (user2.AccountTypeLogo == holdAccountType)
                    return user2;
            }

            return null;
        }

        /// <summary>
        /// 根据用户的持仓账户返回关联的用户资金账户信息(也即根据相关账号查询返回相关联的账号)
        /// </summary>
        /// <param name="holdAccount">持仓账户</param>
        /// <returns></returns>
        public UA_UserAccountAllocationTableInfo GetCapitalAccountByHoldAccount(string holdAccount)
        {
            var user = GetUserByAccount(holdAccount);

            if (user == null)
                return null;

            var accountType = user.AccountTypeLogo;

            int capitalAccountType = -1;

            if (!accountTypeDic.ContainsKey(accountType))
                return null;

            var accountTypeInfo = accountTypeDic[accountType];

            if (accountTypeInfo.AccountTypeLogo == accountType)
            {
                if (accountTypeInfo.ATCId.Value == (int)Types.AccountAttributionType.BankAccount)
                    return null;

                if (accountTypeInfo.ATCId.Value == (int)Types.AccountAttributionType.SpotCapital ||
                    accountTypeInfo.ATCId.Value == (int)Types.AccountAttributionType.FuturesCapital)
                {
                    return user;
                }
                List<BD_AccountTypeInfo> list = GetAllAccountType();
                foreach (var item in list)
                {
                    if (item.RelationAccountId.HasValue && item.RelationAccountId.Value == accountTypeInfo.AccountTypeLogo)
                    {
                        capitalAccountType = item.AccountTypeLogo;
                        break;
                    }
                }
                //capitalAccountType = accountTypeInfo.RelationAccountId.Value;
            }


            var useList = GetUserAllAccounts(user.UserID);
            if (Utils.IsNullOrEmpty(useList))
                return null;

            foreach (var user2 in useList)
            {
                if (user2.AccountTypeLogo == capitalAccountType)
                    return user2;
            }

            return null;
        }

        /// <summary>
        /// 根据用户ID，账号类别查询账号信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="accountTypeClass">账号类别，这里类别不是类型（类型目前数据库有九个，类别有五个，这两个值要区别）</param>
        /// <returns></returns>
        public List<UA_UserAccountAllocationTableInfo> GetAccountByUserIDAndAccountTypeClass(string userId, Types.AccountAttributionType accountTypeClass)
        {
            return GetAccountByUserIDAndPwdAndAccountTypeClass(userId, "", accountTypeClass);
        }

        /// <summary>
        /// 根据用户ID，密码，账号类别查询账号信息
        /// 因为一个用户可能拥有多个账号所以返回list（如：现货资金帐户-->证券资金帐户,港股资金帐户)
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pwd">用户密码</param>
        /// <param name="accountTypeClass">账号类别，这里类别不是类型（类型目前数据库有九个，类别有五个，这两个值要区别）</param>
        /// <returns></returns>
        public List<UA_UserAccountAllocationTableInfo> GetAccountByUserIDAndPwdAndAccountTypeClass(string userId, string pwd, Types.AccountAttributionType accountTypeClass)
        {
            List<UA_UserAccountAllocationTableInfo> list = new List<UA_UserAccountAllocationTableInfo>();

            var userList = GetUserAllAccounts(userId);
            if (Utils.IsNullOrEmpty(userList))
                return list;

            foreach (var user in userList)
            {
                int accountType = user.AccountTypeLogo;
                if (!accountTypeDic.ContainsKey(accountType))
                    return list;

                var accountTypeInfo = accountTypeDic[accountType];
                if (accountTypeInfo.ATCId.Value == (int)accountTypeClass)
                    list.Add(user);
            }

            return list;
        }
        /// <summary>
        /// 根据用户ID，账号类类查询账号信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="accountType">账号类型（类型目前数据库有九个）</param>
        /// <returns></returns>
        public UA_UserAccountAllocationTableInfo GetAccountByUserIDAndAccountType(string userId, int accountType)
        {
            var userList = GetUserAllAccounts(userId);
            if (Utils.IsNullOrEmpty(userList))
                return null;
            foreach (var user in userList)
            {
                if (user.AccountTypeLogo == accountType)
                    return user;
            }
            return null;
        }

        /// <summary>
        /// 根据用户ID，和用户类型，期货和现货标志 ：0=期货，1=现货，从缓存数据中查询用户相关的持仓账号信息
        /// </summary>
        /// <param name="userID">交易员ID</param>
        /// <param name="accountType">查询的账号类型0是查询所有</param>
        /// <param name="futureSpotFlag">期货和现货标志 ：0=期货，1=现货</param>
        /// <returns></returns>
        public List<UA_UserAccountAllocationTableInfo> GetUserHoldAccountFormUserCache(string userID, int accountType, byte futureSpotFlag)
        {
            List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
            #region 从缓存中获取账号 这里只是再封装，内部如果在内存没有查询到会先到数据库中查询后再返回再缓存
            if (accountType == 0)
            {
                if (futureSpotFlag == 1)
                {
                    userAccountInfo = GetAccountByUserIDAndAccountTypeClass(userID, Types.AccountAttributionType.SpotHold);
                }
                else if (futureSpotFlag == 0)
                {
                    userAccountInfo = GetAccountByUserIDAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesHold);
                }
            }
            else
            {
                UA_UserAccountAllocationTableInfo account = GetAccountByUserIDAndAccountType(userID, accountType);
                if (account != null)
                {
                    userAccountInfo.Add(account);
                }
            }
            #endregion
            return userAccountInfo;
        }
        /// <summary>
        /// 根据用户ID，和用户类型，期货和现货标志 ：0=期货，1=现货，从缓存数据中查询用户相关的资金账号信息
        /// </summary>
        /// <param name="userID">交易员ID</param>
        /// <param name="accountType">查询的账号类型0是查询所有</param>
        /// <param name="futureSpotFlag">期货和现货标志 ：0=期货，1=现货</param>
        /// <returns></returns>
        public List<UA_UserAccountAllocationTableInfo> GetUserCapitalAccountFormUserCache(string userID, int accountType, byte futureSpotFlag)
        {
            List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
            #region 从缓存中获取账号 这里只是再封装，内部如果在内存没有查询到会先到数据库中查询后再返回再缓存
            if (accountType == 0)
            {
                if (futureSpotFlag == 1)
                {
                    userAccountInfo = GetAccountByUserIDAndAccountTypeClass(userID, Types.AccountAttributionType.SpotCapital);
                }
                else if (futureSpotFlag == 0)
                {
                    userAccountInfo = GetAccountByUserIDAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesCapital);
                }
            }
            else
            {
                UA_UserAccountAllocationTableInfo account = GetAccountByUserIDAndAccountType(userID, accountType);
                if (account != null)
                {
                    userAccountInfo.Add(account);
                }
            }
            #endregion
            return userAccountInfo;
        }
        #endregion

        #region 账号类型
        /// <summary>
        /// 获取所有的账户类型
        /// </summary>
        /// <returns></returns>
        public List<BD_AccountTypeInfo> GetAllAccountType()
        {
            return accountTypeLsit;
        }
        /// <summary>
        /// 根据账号类别返回所有账号类型实体列表
        /// </summary>
        /// <param name="atcid"></param>
        /// <returns></returns>
        public List<BD_AccountTypeInfo> GetAccoutTypeByACTID(int atcid)
        {
            List<BD_AccountTypeInfo> list = new List<BD_AccountTypeInfo>();
            foreach (var accountTypeInfo in accountTypeLsit)
            {
                if (accountTypeInfo.ATCId.Value == atcid)
                {
                    list.Add(accountTypeInfo);
                }
            }

            return list;
        }
        /// <summary>
        /// 根据账号类型ID返回账号类型实体
        /// </summary>
        /// <param name="accountTypeLogo">账号类型ID</param>
        /// <returns></returns>
        public BD_AccountTypeInfo GetAccountType(int accountTypeLogo)
        {
            if (accountTypeDic.ContainsKey(accountTypeLogo))
            {
                return accountTypeDic[accountTypeLogo];
            }

            var info = bdAccountTypeDal.GetModel(accountTypeLogo);
            accountTypeLsit.Add(info);
            accountTypeDic[info.AccountTypeLogo] = info;

            return info;
        }
        #endregion


    }
}