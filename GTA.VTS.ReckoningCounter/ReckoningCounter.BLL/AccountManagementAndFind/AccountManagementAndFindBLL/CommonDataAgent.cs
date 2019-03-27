using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Model;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL
{
    /// <summary>
    /// 公共数据方法管理
    /// Update BY:李健华
    /// Update Date:2010-06-08
    /// </summary>
    public class CommonDataAgent
    {
        /// <summary>
        /// 行情实例
        /// </summary>
        public static IRealtimeMarketService _realtimeService;
        /// <summary>
        /// 返回行情操作实例
        /// </summary>
        public static IRealtimeMarketService RealtimeService
        {
            get
            {
                if (_realtimeService == null)
                {
                    //_realtimeService = RealtimeMarketServiceFactory.GetService();
                    _realtimeService = RealTimeMarketUtil.GetRealMarketService();
                }
                return _realtimeService;
            }
        }

        /// <summary>
        /// 根据帐户类型和交易员ID找出对应的帐户和帐户所属类型
        /// </summary>
        /// <param name="freeTransfer">转帐实体</param>
        /// <param name="outMessage"></param>
        public static bool AccountTransferEntityChange(ref FreeTransferEntity freeTransfer, ref string outMessage)
        {
            string where = string.Format("UserID = '{0}' AND AccountTypeLogo= '{1}'", freeTransfer.TraderID, freeTransfer.FromCapitalAccountType);
            UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
            List<UA_UserAccountAllocationTableInfo> AccountListFrom = dal.GetListArray(where);
            if (AccountListFrom == null || AccountListFrom.Count < 1)
            {
                outMessage = "转出资金帐号不存在";
                return false;
            }
            if (AccountListFrom[0].WhetherAvailable == false)
            {
                outMessage = "转出资金帐户被冻结，不能转帐";
                return false;
            }

            where = string.Format("UserID = '{0}' AND AccountTypeLogo= '{1}'", freeTransfer.TraderID, freeTransfer.ToCapitalAccountType);
            List<UA_UserAccountAllocationTableInfo> AccountListTo = dal.GetListArray(where);
            if (AccountListTo == null || AccountListTo.Count < 1)
            {
                outMessage = "转入资金帐号不存在";
                return false;
            }
            if (AccountListTo[0].WhetherAvailable == false)
            {
                outMessage = "转入资金帐户被冻结，不能转帐";
                return false;
            }

            BD_AccountTypeDal acTypeDal = new BD_AccountTypeDal();
            BD_AccountTypeInfo FrombdAccountType = acTypeDal.GetModel(freeTransfer.FromCapitalAccountType);
            BD_AccountTypeInfo TobdAccountType = acTypeDal.GetModel(freeTransfer.ToCapitalAccountType);
            if (FrombdAccountType == null || TobdAccountType == null)
            {
                outMessage = "帐号所属类型匹配失败";
                return false;
            }
            freeTransfer.FromCapitalAccount = AccountListFrom[0].UserAccountDistributeLogo;
            freeTransfer.ToCapitalAccount = AccountListTo[0].UserAccountDistributeLogo;
            //freeTransfer.FromCapitalAccountType = (int)FrombdAccountType.ATCId;
            //freeTransfer.ToCapitalAccountType = (int)TobdAccountType.ATCId;
            return true;
        }

        #region  old code 无用注释掉 update 2010-06-08
        // /// <summary>
        // /// 根据资金账号返回交易员ID和账号所属账号类型
        // /// </summary>
        // /// <param name="strAccountId"></param>
        // /// <param name="traderId"></param>
        // /// <returns></returns>
        //private static int GetAccountTypeIdByAccountId(string strAccountId, ref string traderId)
        //{
        //    int result = 0;
        //    // var acObj = DataRepository.UaUserAccountAllocationTableProvider.GetByUserAccountDistributeLogo(strAccountId);
        //    UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
        //    var acObj = dal.GetModel(strAccountId);
        //    result = acObj.AccountTypeLogo;
        //    traderId = acObj.UserID;
        //    return result;
        //}


        ///// <summary>
        ///// 根据交易员，帐户类型查找对应的 资金持仓帐户对
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="AccountType">帐户类型</param>
        ///// <param name="futureSpotFlag">期货和现货标志 ：0=期货，1=现货</param>
        ///// <param name="userPassword">传0将不进行验证</param>
        ///// <returns>如果交易员存在且验证通过则返回accountPair否则通过userPassword带出消息</returns>
        //public static AccountPair GeyAccountPair(string userId, Int32 AccountType, byte futureSpotFlag, ref string userPassword)
        //{
        //    VTTraders _VTTraders;
        //    bool IsFrist = true;
        //Reset:
        //    if (futureSpotFlag == 1)
        //        _VTTraders = VTTradersFactory.GetStockTraders();
        //    else
        //        _VTTraders = VTTradersFactory.GetFutureTraders();
        //    foreach (var trader in _VTTraders.TraderList)
        //    {
        //        if (userId == trader.Trader.UserID)
        //        {
        //            if (userPassword != null && userPassword != trader.Trader.Password)
        //            {
        //                userPassword = "交易员密码错误";
        //                return null;
        //            }
        //            // trader.AccountPairList//可能会有多个 资金帐户 对象 
        //            foreach (var accountpair in trader.AccountPairList)
        //            {
        //                if (accountpair.CapitalAccount.AccountTypeLogo == AccountType || accountpair.HoldAccount.AccountTypeLogo == AccountType)
        //                {
        //                    return accountpair;
        //                }
        //            }
        //            userPassword = "交易员对应类型的帐号不存在";
        //            return null;
        //        }
        //    }
        //    if (IsFrist)
        //    {
        //        VTTradersFactory.Reset();
        //        IsFrist = false;
        //        goto Reset;
        //    }
        //    userPassword = "交易员不存在";
        //    return null;
        //}
        //# region 由资金账号查找相对应的持仓账号
        ///// <summary>
        ///// 由资金账号查找相对应的持仓账号
        ///// </summary>
        ///// <param name="strAccountId">资金账号</param>
        ///// <returns></returns>
        //public static string GetRealtionAccountIdByAccountId(string strAccountId)
        //{
        //    #region old code
        //    string result = string.Empty;
        //    //string userid = string.Empty;
        //    //var accountTypeLogo = GetAccountTypeIdByAccountId(strAccountId, ref userid);
        //    //var relationAccountId = DataRepository.BdAccountTypeProvider.GetByAccountTypeLogo(accountTypeLogo).RelationAccountId;
        //    //var acObj = DataRepository.UaUserAccountAllocationTableProvider.Find(
        //    //    string.Format("UserID='{0}' AND AccountTypeLogo='{1}'", userid, relationAccountId));
        //    //if (acObj != null && acObj.Count > 0)
        //    //    result = acObj[0].UserAccountDistributeLogo;
        //    #endregion

        //    UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
        //    var userAccountInfo = dal.GetUserHoldAccountByUserCapitalAccount(strAccountId);
        //    if (userAccountInfo != null)
        //    {
        //        result = userAccountInfo.UserAccountDistributeLogo;
        //    }
        //    return result;

        //}
        //# endregion 由资金账号查找相对应的持仓账号

        //#region  据用户ID，和用户类型，期货和现货标志 ：0=期货，1=现货，从缓存数据中查询用户相关的资金账户或者持仓账号信息
        ///// <summary>
        ///// 根据用户ID，和用户类型，期货和现货标志 ：0=期货，1=现货，从缓存数据中查询用户相关的持仓账号信息
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="accountType"></param>
        ///// <param name="futureSpotFlag"></param>
        ///// <param name="errorMsg">返回查询异常</param>
        ///// <returns></returns>
        //public static List<UA_UserAccountAllocationTableInfo> GetUserHoldAccountFormAccountPair(string userID, int accountType, byte futureSpotFlag, ref string errorMsg)
        //{
        //    List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
        //    errorMsg = null;
        //    AccountPair _AccountPair1 = null;
        //    AccountPair _AccountPair = null;
        //    #region 从缓存中获取账号
        //    if (accountType == 0)
        //    {
        //        #region 这里从数据缓存中获取数据，如果数据库添加新的类别品种类型又得增加代码或者修改，不利于扩展,便这为了速度优先所以从数据缓存中获取
        //        if (futureSpotFlag == 1)
        //        {
        //            _AccountPair = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.StockSpotHoldCode, futureSpotFlag, ref errorMsg);
        //            _AccountPair1 = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.HKSpotHoldCode, futureSpotFlag, ref errorMsg);
        //        }
        //        else
        //        {
        //            _AccountPair = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.CommodityFuturesHoldCode, futureSpotFlag, ref errorMsg);
        //            _AccountPair1 = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.StockFuturesHoldCode, futureSpotFlag, ref errorMsg);
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        _AccountPair = CommonDataAgent.GeyAccountPair(userID, accountType, futureSpotFlag, ref errorMsg);
        //    }
        //    #region 添加数据

        //    if (_AccountPair != null)
        //    {
        //        userAccountInfo.Add(_AccountPair.HoldAccount);
        //    }
        //    if (_AccountPair1 != null)
        //    {
        //        userAccountInfo.Add(_AccountPair1.HoldAccount);
        //    }
        //    #endregion
        //    #endregion
        //    return userAccountInfo;
        //}
        ///// <summary>
        ///// 根据用户ID，和用户类型，期货和现货标志 ：0=期货，1=现货，从缓存数据中查询用户相关的持仓账号信息
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="accountType"></param>
        ///// <param name="futureSpotFlag"></param>
        ///// <param name="errorMsg">返回查询异常</param>
        ///// <returns></returns>
        //public static List<UA_UserAccountAllocationTableInfo> GetUserCapitalAccountFormAccountPair(string userID, int accountType, byte futureSpotFlag, ref string errorMsg)
        //{
        //    List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
        //    errorMsg = null;
        //    AccountPair _AccountPair1 = null;
        //    AccountPair _AccountPair = null;
        //    #region 从缓存中获取账号
        //    if (accountType == 0)
        //    {
        //        #region 这里从数据缓存中获取数据，如果数据库添加新的类别品种类型又得增加代码或者修改，不利于扩展,便这为了速度优先所以从数据缓存中获取
        //        if (futureSpotFlag == 1)
        //        {
        //            _AccountPair = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.StockSpotCapital, futureSpotFlag, ref errorMsg);
        //            _AccountPair1 = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.HKSpotCapital, futureSpotFlag, ref errorMsg);
        //        }
        //        else
        //        {
        //            _AccountPair = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.CommodityFuturesCapital, futureSpotFlag, ref errorMsg);
        //            _AccountPair1 = CommonDataAgent.GeyAccountPair(userID, (int)Types.AccountType.StockFuturesCapital, futureSpotFlag, ref errorMsg);
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        _AccountPair = CommonDataAgent.GeyAccountPair(userID, accountType, futureSpotFlag, ref errorMsg);
        //    }
        //    #region 添加数据

        //    if (_AccountPair != null)
        //    {
        //        userAccountInfo.Add(_AccountPair.CapitalAccount);
        //    }
        //    if (_AccountPair1 != null)
        //    {
        //        userAccountInfo.Add(_AccountPair1.CapitalAccount);
        //    }
        //    #endregion
        //    #endregion
        //    return userAccountInfo;
        //}
        //#endregion

        ///// <summary>
        ///// 根据类型得到具体银行帐号
        ///// </summary>
        ///// <param name="Userid"></param>
        ///// <param name="password"></param>
        ///// <param name="mess"></param>
        ///// <param name="AccountType"></param>
        ///// <returns></returns>
        //public static string GetBackAccount(string Userid, string password, out string mess, int AccountType)
        //{
        //    //mess = "";
        //    #region update date:2009-07-09 update by:李健华
        //    //判断密码
        //    //var UserEntity = DataRepository.UaUserBasicInformationTableProvider.GetByUserId(Userid);
        //    //if (UserEntity == null || UserEntity.Password != password)
        //    //{
        //    //    mess = "用户密码验证失败";
        //    //    return string.Empty;
        //    //}
        //    ////查找银行帐户
        //    //var BackAccountList = DataRepository.UaUserAccountAllocationTableProvider.Find(string.Format("UserID='{0}' AND AccountTypeLogo='{1}'", Userid,
        //    //                                                        AccountType));

        //    //if (BackAccountList == null || BackAccountList.Count < 1)
        //    //{
        //    //    mess = "查找银行帐户失败";
        //    //    return string.Empty;
        //    //}
        //    //return BackAccountList[0].UserAccountDistributeLogo;
        //    #endregion

        //    return GetUserAccountByType(Userid, password, out mess, GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount);
        //}



        //#region 根据用户ID，密码，账号类型查询账号 Create by:李健华 Create date:2009-07-09
        ///// <summary>
        ///// 根据用户ID，密码，账号类型查询账号
        ///// 因为一个用户可能拥有多个账号（如：现货资金帐户-->证券资金帐户,港股资金帐户)而这里只返回第一个
        ///// </summary>
        ///// <param name="userId">用户ID</param>
        ///// <param name="pwd">用户密码</param>
        ///// <param name="mess">异常信息</param>
        ///// <param name="type">账号类型</param>
        ///// <returns>返回第一个账号</returns>
        //public static string GetUserAccountByType(string userId, string pwd, out string mess, GTA.VTS.Common.CommonObject.Types.AccountAttributionType type)
        //{
        //    mess = "";
        //    UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
        //    List<UA_UserAccountAllocationTableInfo> accountList = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userId, pwd, GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount);
        //    if (accountList == null || accountList.Count <= 0)
        //    {
        //        mess = "查找银行帐户失败";
        //        return string.Empty;
        //    }
        //    return accountList[0].UserAccountDistributeLogo;
        //}
        //#endregion   
        #endregion
    }
}
