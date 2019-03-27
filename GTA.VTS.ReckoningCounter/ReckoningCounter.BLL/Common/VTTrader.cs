#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.DAL;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 交易员管理列表工厂
    /// 作者：宋涛
    /// 日期：2008-11-25
    /// Update By:李健华
    /// Update date:2010-06-08
    /// Desc.:修改此类不再缓存内部的相关交易员因为此方法中数据只有在盘后清算用到，直接在数据库中操作即可
    /// </summary>
    public static class VTTradersFactory
    {
        private static object stockRoot = new object();

        #region  old code  注释 --李健华 2010-06-09
        //private static VTTraders stockTraders;

        //private static object futureRoot = new object();
        //private static VTTraders futureTraders;

        //public static VTTraders GetStockTraders()
        //{
        //    lock (stockRoot)
        //    {
        //        if (stockTraders == null)
        //        {
        //            stockTraders = new VTTraders(Types.BreedClassTypeEnum.Stock);
        //        }

        //        return stockTraders;
        //    }
        //}

        //public static VTTraders GetFutureTraders()
        //{
        //    lock (futureRoot)
        //    {
        //        if (futureTraders == null)
        //        {
        //            futureTraders = new VTTraders(Types.BreedClassTypeEnum.CommodityFuture);
        //        }

        //        return futureTraders;
        //    }
        //}
        #endregion
        /// <summary>
        /// 重置
        /// </summary>
        public static void Reset()
        {
            #region  old code  注释 --李健华 2010-06-09
            //stockTraders = null;
            //futureTraders = null;

            //VTTraderUtil.Reset();
            #endregion
        }

        /// <summary>
        /// Create by:李健华
        /// Create date:2010-06-08
        /// Desc.: 初始货配对资金账号与持仓账号
        /// </summary>
        /// <param name="capitalAccountType"></param>
        /// <param name="userID">交易员ID</param>
        public static List<AccountPair> InitializeAccountPair(BD_AccountTypeInfo capitalAccountType)
        {

            List<AccountPair> AccountPairList = new List<AccountPair>();

            try
            {
                List<UA_UserBasicInformationTableInfo> userBasicList = new List<UA_UserBasicInformationTableInfo>();
                UA_UserBasicInformationTableDal ubDal = new UA_UserBasicInformationTableDal();
                userBasicList = ubDal.GetAll();
                if (userBasicList == null)
                {
                    return AccountPairList;
                }
                UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                foreach (var item in userBasicList)
                {
                    string where = string.Format("UserID = '{0}' AND AccountTypeLogo= '{1}'", item.UserID, capitalAccountType.AccountTypeLogo);

                    List<UA_UserAccountAllocationTableInfo> capitalAccountList = dal.GetListArray(where);

                    if (Utils.IsNullOrEmpty(capitalAccountList))
                    {
                        continue;
                    }

                    //对应资金账户类型的只能有一个资金交易账户
                    UA_UserAccountAllocationTableInfo capitalAccount = capitalAccountList[0];

                    #region  查找对应的持仓交易账户
                    //查找对应的持仓交易账户
                    int? relationID = capitalAccountType.RelationAccountId;
                    if (!relationID.HasValue)
                    {
                        continue;
                    }

                    int holdAccountTypeID = relationID.Value;
                    where = string.Format("UserID = '{0}' AND AccountTypeLogo= '{1}'", item.UserID, holdAccountTypeID);

                    List<UA_UserAccountAllocationTableInfo> HoldAccountList = dal.GetListArray(where);

                    if (Utils.IsNullOrEmpty(HoldAccountList))
                    {
                        //IsInitializeSuccess = false;
                        continue;
                    }

                    //对应持仓账户类型的只能有一个持仓交易账户
                    UA_UserAccountAllocationTableInfo holdAccount = HoldAccountList[0];
                    #endregion

                    AccountPair accountPair = new AccountPair { CapitalAccount = capitalAccount, HoldAccount = holdAccount };
                    AccountPairList.Add(accountPair);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("期货盘前检查初始货配对资金账号与持仓账号异常" + ex.Message, ex);
                AccountPairList = null;
            }
            return AccountPairList;

        }

    }

    #region old code  注释 --李健华 2010-06-09
    ///// <summary>
    ///// 交易员管理列表，现货或者期货分开处理
    ///// </summary>
    //public class VTTraders
    //{
    //    private IList<VTTrader> traderList = new List<VTTrader>();
    //    private IDictionary<string, VTTrader> traderDic = new Dictionary<string, VTTrader>();
    //    private IDictionary<string, VTTrader> accountDic = new Dictionary<string, VTTrader>();
    //    private Types.BreedClassTypeEnum breedClassTypeEnum;

    //    public VTTraders(Types.BreedClassTypeEnum breedClassTypeEnum)
    //    {
    //        this.breedClassTypeEnum = breedClassTypeEnum;
    //        List<UA_UserBasicInformationTableInfo> users = VTTraderUtil.GetAllTraders();
    //        if (Utils.IsNullOrEmpty(users))
    //            return;

    //        foreach (UA_UserBasicInformationTableInfo user in users)
    //        {
    //            AddVTTrader(breedClassTypeEnum, user);
    //        }

    //        IsInitializeSuccess = true;
    //    }

    //    private void AddVTTrader(Types.BreedClassTypeEnum bcTypeEnum, UA_UserBasicInformationTableInfo user)
    //    {
    //        try
    //        {
    //            VTTrader trader = new VTTrader(bcTypeEnum, user);

    //            if (trader.IsInitializeSuccess)
    //            {
    //                if (traderDic.ContainsKey(trader.Trader.UserID))
    //                {
    //                    traderDic.Remove(trader.Trader.UserID);
    //                }
    //                if (traderList.Contains(trader))
    //                {
    //                    traderList.Remove(trader);
    //                }

    //                TraderList.Add(trader);
    //                traderDic.Add(trader.Trader.UserID, trader);

    //                foreach (var pair in trader.AccountPairList)
    //                {
    //                    if (accountDic.ContainsKey(pair.CapitalAccount.UserAccountDistributeLogo))
    //                    {
    //                        accountDic.Remove(pair.CapitalAccount.UserAccountDistributeLogo);
    //                    }
    //                    if (accountDic.ContainsKey(pair.HoldAccount.UserAccountDistributeLogo))
    //                    {
    //                        accountDic.Remove(pair.HoldAccount.UserAccountDistributeLogo);
    //                    }

    //                    accountDic.Add(pair.CapitalAccount.UserAccountDistributeLogo, trader);
    //                    accountDic.Add(pair.HoldAccount.UserAccountDistributeLogo, trader);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.WriteError(ex.Message, ex);
    //        }
    //    }

    //    public IList<VTTrader> TraderList
    //    {
    //        get { return traderList; }
    //    }

    //    public VTTrader GetByUserID(string UserID)
    //    {
    //        VTTrader trader = null;

    //        if (traderDic.ContainsKey(UserID))
    //            trader = traderDic[UserID];

    //        return trader;
    //    }

    //    public VTTrader GetByAccount(string account)
    //    {
    //        VTTrader trader = null;

    //        if (accountDic.ContainsKey(account))
    //            trader = accountDic[account];

    //        return trader;
    //    }

    //    public bool IsInitializeSuccess { get; private set; }

    //    public Types.BreedClassTypeEnum BreedClassType
    //    {
    //        get
    //        {
    //            return this.breedClassTypeEnum;
    //        }
    //    }
    //}

    ///// <summary>
    ///// 交易员管理类，集中管理某个交易员下的所有配对的资金和持仓对象，现货或者期货分开处理
    ///// </summary>
    //public class VTTrader
    //{
    //    private IList<AccountPair> accountPairList = new List<AccountPair>();

    //    private UA_UserBasicInformationTableInfo trader;

    //    public UA_UserBasicInformationTableInfo Trader
    //    {
    //        get { return trader; }
    //    }

    //    public VTTrader(Types.BreedClassTypeEnum breedClassTypeEnum, UA_UserBasicInformationTableInfo user)
    //    {
    //        //AccountTypeClass账户类型分类，如期货资金分类
    //        Types.AccountAttributionType capitalATCID = Types.AccountAttributionType.SpotCapital;
    //        switch (breedClassTypeEnum)
    //        {
    //            case Types.BreedClassTypeEnum.Stock:
    //                capitalATCID = Types.AccountAttributionType.SpotCapital;
    //                break;
    //            case Types.BreedClassTypeEnum.CommodityFuture:
    //            case Types.BreedClassTypeEnum.StockIndexFuture:
    //                capitalATCID = Types.AccountAttributionType.FuturesCapital;
    //                break;
    //        }

    //        //资金账户类型AccountType，如1.商品期货资金帐号 2.股指期货资金帐号
    //        //BD_AccountTypeDal bd_AccountTypeDal = new BD_AccountTypeDal();
    //        //ATCId
    //        //string where = string.Format("ATCId = '{0}'", (int)capitalATCID);
    //        //List<BD_AccountTypeInfo> capitalAccountTypes = bd_AccountTypeDal.GetListArray(where);

    //        List<BD_AccountTypeInfo> capitalAccountTypes = AccountManager.Instance.GetAccoutTypeByACTID((int)capitalATCID);

    //        if (Utils.IsNullOrEmpty(capitalAccountTypes))
    //        {
    //            return;
    //        }

    //        this.trader = user;

    //        //每一个资金账户类型，对应一个持仓账户类型
    //        foreach (BD_AccountTypeInfo capitalAccountType in capitalAccountTypes)
    //        {
    //            //根据资金和持仓账户类型，获取对应的资金账户和持仓账户，放入一个Pair
    //            InitializeAccountPair(capitalAccountType);
    //        }

    //        IsInitializeSuccess = true;
    //    }

    //    public IList<AccountPair> AccountPairList
    //    {
    //        get { return accountPairList; }
    //    }

    //    public bool IsInitializeSuccess { get; private set; }


    //    private void InitializeAccountPair(BD_AccountTypeInfo capitalAccountType)
    //    {
    //        List<UA_UserAccountAllocationTableInfo> capitalAccountList = VTTraderUtil.GetAccountList(trader,
    //                                                                                capitalAccountType.AccountTypeLogo);

    //        if (Utils.IsNullOrEmpty(capitalAccountList))
    //        {
    //            IsInitializeSuccess = false;
    //            return;
    //        }

    //        //对应资金账户类型的只能有一个资金交易账户
    //        UA_UserAccountAllocationTableInfo capitalAccount = capitalAccountList[0];

    //        //查找对应的持仓交易账户
    //        int? relationID = capitalAccountType.RelationAccountId;
    //        if (!relationID.HasValue)
    //        {
    //            IsInitializeSuccess = false;
    //            return;
    //        }

    //        int holdAccountTypeID = relationID.Value;

    //        List<UA_UserAccountAllocationTableInfo> HoldAccountList = VTTraderUtil.GetAccountList(trader, holdAccountTypeID);
    //        if (Utils.IsNullOrEmpty(HoldAccountList))
    //        {
    //            IsInitializeSuccess = false;
    //            return;
    //        }

    //        //对应持仓账户类型的只能有一个持仓交易账户
    //        UA_UserAccountAllocationTableInfo holdAccount = HoldAccountList[0];


    //        AccountPair accountPair = new AccountPair { CapitalAccount = capitalAccount, HoldAccount = holdAccount };
    //        AccountPairList.Add(accountPair);
    //    }

    //    //private static TList<UaUserAccountAllocationTable> GetAccountList(UaUserBasicInformationTable user,
    //    //                                                                  int accountTypeID)
    //    //{
    //    //    string where = string.Format("UserID = '{0}' AND AccountTypeLogo= '{1}'",
    //    //                                 user.UserId, accountTypeID);
    //    //    return DataRepository.UaUserAccountAllocationTableProvider.Find(where);
    //    //}
    //}

    ///// <summary>
    ///// 数据库缓存类
    ///// </summary>
    //internal static class VTTraderUtil
    //{
    //    internal static void Reset()
    //    {
    //        users = GetUsers();
    //        accountList.Reset();
    //    }

    //    static List<UA_UserBasicInformationTableInfo> GetUsers()
    //    {
    //        //List<UA_UserBasicInformationTableInfo> list = new List<UA_UserBasicInformationTableInfo>();
    //        //try
    //        //{
    //        //    UA_UserBasicInformationTableDal dal = new UA_UserBasicInformationTableDal();
    //        //    //list = DataRepository.UaUserBasicInformationTableProvider.GetAll();
    //        //    //list = dal.GetListArray(string.Empty);
    //        //    list = dal.GetAll();
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    LogHelper.WriteError(ex.Message, ex);
    //        //}

    //        //return list;

    //        return AccountManager.Instance.GetAllBasicUsers();
    //    }

    //    public static UA_UserBasicInformationTableInfo GetUser(string traderID)
    //    {
    //        //UA_UserBasicInformationTableInfo result = null;
    //        //try
    //        //{
    //        //    UA_UserBasicInformationTableDal userBasicInformationTableDal = new UA_UserBasicInformationTableDal();
    //        //    result = userBasicInformationTableDal.GetModel(traderID);
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    LogHelper.WriteError(ex.Message, ex);
    //        //}

    //        //return result;
    //        return AccountManager.Instance.GetBasicUserByUserId(traderID);
    //    }

    //    private static List<UA_UserBasicInformationTableInfo> users = GetUsers();

    //    /// <summary>
    //    /// 获取所有的交易员
    //    /// </summary>
    //    /// <returns></returns>
    //    internal static List<UA_UserBasicInformationTableInfo> GetAllTraders()
    //    {
    //        if (Utils.IsNullOrEmpty(users))
    //        {
    //            return null;
    //        }

    //        lock (users)
    //        {
    //            return users;
    //        }
    //    }

    //    private static ObjectCache<string, List<UA_UserAccountAllocationTableInfo>> accountList =
    //        new ObjectCache<string, List<UA_UserAccountAllocationTableInfo>>();

    //    /// <summary>
    //    /// 根据交易员和账户类型返回账户
    //    /// </summary>
    //    /// <param name="user">交易员</param>
    //    /// <param name="accountTypeID">账户类型</param>
    //    /// <returns>账户</returns>
    //    internal static List<UA_UserAccountAllocationTableInfo> GetAccountList(UA_UserBasicInformationTableInfo user,
    //                                                                      int accountTypeID)
    //    {
    //        List<UA_UserAccountAllocationTableInfo> result = null;
    //        lock (accountList)
    //        {
    //            string key = user.UserID.Trim() + accountTypeID;

    //            result = accountList.GetByKey(key);

    //            if (result == null)
    //            {
    //                result = GetAccountListFromDB(user, accountTypeID);

    //                if (result != null)
    //                    accountList.Add(result, key);
    //            }
    //        }

    //        return result;
    //    }

    //    private static List<UA_UserAccountAllocationTableInfo> GetAccountListFromDB(UA_UserBasicInformationTableInfo user,
    //                                                                      int accountTypeID)
    //    {
    //        List<UA_UserAccountAllocationTableInfo> result = new List<UA_UserAccountAllocationTableInfo>();
    //        var acc = AccountManager.Instance.GetAccountByUserIDAndAccountType(user.UserID, accountTypeID);
    //        if (acc != null)
    //        {
    //            result.Add(acc);

    //            return result;
    //        }

    //        string where = string.Format("UserID = '{0}' AND AccountTypeLogo= '{1}'",
    //                                     user.UserID, accountTypeID);
    //        UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
    //        return dal.GetListArray(where);
    //        //return DataRepository.UaUserAccountAllocationTableProvider.Find(where);

    //    }
    //}

    #endregion

    /// <summary>
    ///  账号配对实体
    /// </summary>
    public class AccountPair
    {
        /// <summary>
        /// 资金账号实体
        /// </summary>
        public UA_UserAccountAllocationTableInfo CapitalAccount { get; set; }
        /// <summary>
        /// 持仓资金账号实体
        /// </summary>
        public UA_UserAccountAllocationTableInfo HoldAccount { get; set; }
    }

}