#region Using Namespace

using System;
using System.Collections.Generic;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Model;
using Types = ReckoningCounter.Entity.Contants.Types;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 柜台交易员相关信息缓冲处理
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public abstract class CounterCache
    {
        #region == 静态属性 ==

        //protected static volatile CounterCache _instance;

        //protected static object lockObject = new object();

        ///// <summary>
        ///// 单例
        ///// </summary>
        //public static CounterCache Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (lockObject)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new CounterCache();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        #endregion

        #region == 属性/字段 ==

        /// <summary>
        /// 锁超时
        /// </summary>
        //private const int _lockTimeout = 5000;
        #region update date:2009-08-21 Update By:李健华
        /// <summary>
        /// 成交最大编号---这里只能设置为十万，因为数据库只有20位，
        /// 所以这里超出这个值后与前面的时间加起来的字符串超出20位，数据库就溢出
        /// </summary>
        private const int DealOrderMaxValue = 10000;

        /// <summary>
        /// 委托最大编号
        /// </summary>
        private const int OrderMaxValue = 10000;
        #endregion
        private static ReaderWriterLockSlim _orderMappingBufferLock = new ReaderWriterLockSlim();
        #region 注释 2010-06-08 李健华
        //private static ReaderWriterLockSlim _fundTraderIdMappingLock = new ReaderWriterLockSlim();
        //private static ReaderWriterLockSlim _userAccountCacheLock = new ReaderWriterLockSlim();
        #endregion

        /// <summary>
        /// 成交单计数器
        /// </summary>
        private int _dealOrderCounter;

        /// <summary>
        /// 成交锁对象
        /// </summary>
        private object _dealOrderLocker = new object();

        //private Dictionary<string, string> _fundTraderIdMapping;

        /// <summary>
        /// 委托单计数器
        /// </summary>
        private int _orderCounter;

        /// <summary>
        /// 委托锁对象
        /// </summary>
        private object _orderLocker = new object();

        /// <summary>
        /// 委托对象信息缓存
        /// </summary>
        private Dictionary<string, OrderCacheItem> _orderMappingBuffer;

        #region 注释 2010-06-08 李健华
        ///// <summary>
        ///// 用户帐户信息缓存
        ///// </summary>
        //private ObjectCache<string, List<UA_UserAccountAllocationTableInfo>> _userAccountCache;

        //private VTTraders futureTraders;
        //private VTTraders stockTraders;
        #endregion

        /// <summary>
        /// 委托单计数器
        /// </summary>
        public int NewOrderNo
        {
            get
            {
                lock (_orderLocker)
                {
                    if (_orderCounter >= OrderMaxValue)
                        _orderCounter = 0;
                    _orderCounter += 1;

                    return _orderCounter;
                }
            }
        }

        /// <summary>
        /// 成交单计数器
        /// </summary>
        public int NewDealOrderNo
        {
            get
            {
                lock (_dealOrderLocker)
                {
                    if (_dealOrderCounter >= DealOrderMaxValue)
                        _dealOrderCounter = 0;
                    _dealOrderCounter += 1;
                    return _dealOrderCounter;
                }
            }
        }

        #endregion

        #region == 构造器 ==

        /// <summary>
        /// 构造器
        /// </summary>
        protected CounterCache()
        {
            #region 注释 2010-06-08 李健华
            //_userAccountCache = new ObjectCache<string, List<UA_UserAccountAllocationTableInfo>>();
            #endregion

            _orderMappingBuffer = new Dictionary<string, OrderCacheItem>();


            #region 注释 2010-06-08 李健华
            //_fundTraderIdMapping = new Dictionary<string, string>();
            //stockTraders = VTTradersFactory.GetStockTraders();
            //futureTraders = VTTradersFactory.GetFutureTraders();
            #endregion
        }

        #endregion

        #region == 方法 ==
        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        public void ResetDictionary()
        {
            _orderMappingBufferLock.EnterWriteLock();
            try
            {
                _orderMappingBuffer.Clear();
            }
            finally
            {
                _orderMappingBufferLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 添加委托映射关系
        /// </summary>
        /// <param name="strMcOrderNo"></param>
        /// <param name="item"></param>
        public void AddOrderMappingInfo(string strMcOrderNo, OrderCacheItem item)
        {
            //lock (((ICollection) _orderMappingBuffer).SyncRoot)
            //{
            //    if (!_orderMappingBuffer.ContainsKey(strMcOrderNo))
            //        _orderMappingBuffer.Add(strMcOrderNo, item);
            //}

            //_orderMappingBufferLock.TryEnterWriteLock(_lockTimeout);
            //try
            //{
            //    if (!_orderMappingBuffer.ContainsKey(strMcOrderNo))
            //        _orderMappingBuffer.Add(strMcOrderNo, item);
            //}
            //finally
            //{
            //    _orderMappingBufferLock.ExitWriteLock();
            //}

            _orderMappingBufferLock.EnterUpgradeableReadLock();
            try
            {
                if (!_orderMappingBuffer.ContainsKey(strMcOrderNo))
                {
                    _orderMappingBufferLock.EnterWriteLock();
                    try
                    {
                        _orderMappingBuffer.Add(strMcOrderNo, item);
                    }
                    finally
                    {
                        _orderMappingBufferLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _orderMappingBufferLock.ExitUpgradeableReadLock();
            }
        }


        /// <summary>
        /// 移除委托映射关系
        /// </summary>
        /// <param name="strMcOrderNo"></param>
        /// <param name="ost"></param>
        public void RemoveOrderMappingInfo(string strMcOrderNo, Types.OrderStateType ost)
        {
            LogHelper.WriteDebug("CounterCache.RemoveOrderMappingInfo从ConterCache移除委托单号映射,OrderNo=" + strMcOrderNo);

            if (!string.IsNullOrEmpty(strMcOrderNo))
            {
                if (ost == Types.OrderStateType.DOSCanceled ||
                    ost == Types.OrderStateType.DOSDealed ||
                    //ost == Types.OrderStateType.DOSPartRemoved ||
                    ost == Types.OrderStateType.DOSRemoved)
                {
                    _orderMappingBufferLock.EnterUpgradeableReadLock();
                    try
                    {
                        if (!_orderMappingBuffer.ContainsKey(strMcOrderNo))
                        {
                            _orderMappingBufferLock.EnterWriteLock();
                            try
                            {
                                _orderMappingBuffer.Remove(strMcOrderNo);
                            }
                            finally
                            {
                                _orderMappingBufferLock.ExitWriteLock();
                            }
                        }
                    }
                    finally
                    {
                        _orderMappingBufferLock.ExitUpgradeableReadLock();
                    }

                    //lock (((ICollection) _orderMappingBuffer).SyncRoot)
                    //{
                    //    if (_orderMappingBuffer.ContainsKey(strMcOrderNo))
                    //    {
                    //        _orderMappingBuffer.Remove(strMcOrderNo);
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// 依据撮合中心返回的委托单号获取
        /// </summary>
        /// <param name="strMcOrderNo"></param>
        /// <returns></returns>
        public OrderCacheItem GetOrderMappingInfo(string strMcOrderNo)
        {
            OrderCacheItem result = null;
            if (string.IsNullOrEmpty(strMcOrderNo))
                return result;

            _orderMappingBufferLock.EnterReadLock();
            try
            {
                if (_orderMappingBuffer.ContainsKey(strMcOrderNo))
                    result = _orderMappingBuffer[strMcOrderNo];
            }
            finally
            {
                _orderMappingBufferLock.ExitReadLock();
            }

            //lock (((ICollection) _orderMappingBuffer).SyncRoot)
            //{
            //    if (_orderMappingBuffer.ContainsKey(strMcOrderNo))
            //        result = _orderMappingBuffer[strMcOrderNo];
            //}

            try
            {
                //如果当前缓存中找不到，那么从数据库查找
                if (result == null)
                {
                    result = LoadCacheItemFromDB(strMcOrderNo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 从数据库加载缓存项
        /// </summary>
        protected abstract OrderCacheItem LoadCacheItemFromDB(string strMcOrderNo);

        ///// <summary>
        ///// 获取缓存的交易员ID
        ///// </summary>
        ///// <param name="strFundAccountId"></param>
        ///// <returns></returns>
        //public string GetTraderIdByFundAccount(string strFundAccountId)
        //{
        //    string strResult = string.Empty;

        //    _fundTraderIdMappingLock.EnterUpgradeableReadLock();
        //    try
        //    {
        //        if (!_fundTraderIdMapping.TryGetValue(strFundAccountId, out strResult))
        //        {
        //            _fundTraderIdMappingLock.EnterWriteLock();
        //            try
        //            {
        //                if (!string.IsNullOrEmpty(strFundAccountId))
        //                    _fundTraderIdMapping[strFundAccountId] =
        //                        strResult = OrderOfferDataLogic.GetTraderIdByFundAccount(strFundAccountId);
        //            }
        //            finally
        //            {
        //                _fundTraderIdMappingLock.ExitWriteLock();
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        _fundTraderIdMappingLock.ExitUpgradeableReadLock();
        //    }

        //    //lock (((ICollection) _fundTraderIdMapping).SyncRoot)
        //    //{
        //    //    if (!_fundTraderIdMapping.ContainsKey(strFundAccountId))
        //    //    {
        //    //        if (!string.IsNullOrEmpty(strFundAccountId))
        //    //            _fundTraderIdMapping[strFundAccountId] =
        //    //                strResult = OrderOfferDataLogic.GetTraderIdByFundAccount(strFundAccountId);
        //    //    }
        //    //    else
        //    //        strResult = _fundTraderIdMapping[strFundAccountId];
        //    //}

        //    return strResult;
        //}

        ///// <summary>
        ///// 获取缓存的交易员ID
        ///// </summary>
        ///// <param name="strFundAccountId"></param>
        ///// <returns></returns>
        //public string GetTraderIdByXHFundAccountFromVTTraders(string strFundAccountId)
        //{
        //    string strResult = string.Empty;

        //    var trader = stockTraders.GetByAccount(strFundAccountId);
        //    if (trader == null)
        //    {
        //        trader = futureTraders.GetByAccount(strFundAccountId);
        //    }

        //    if (trader == null)
        //    {
        //        LogHelper.WriteInfo("CounterCache.GetTraderIdByXHFundAccount无法获取Trader，strFundAccountId=" +
        //                            strFundAccountId);
        //        return "";
        //    }

        //    strResult = trader.Trader.UserID;
        //    return strResult;
        //}

        /// <summary>
        /// 依据交易员ID,代码获取资金,持仓帐户信息
        /// Create By:李健华
        /// Create Date:2009-10-26
        /// Desc.:为了应用于港股 重载方法
        /// </summary>
        /// <param name="strTraderId"></param>
        /// <param name="strCode"></param>
        /// <param name="codeType">为了区别于港股(商品代码所属类别)</param>
        /// <param name="HoldingAccount"></param>
        /// <param name="HoldingAccountIsAvaliable"></param>
        /// <param name="CapitalAccount"></param>
        /// <param name="CapitalAccountIsAvaliable"></param>
        /// <returns></returns>
        public void GetHoldingAccountByTraderInfo(string strTraderId, string strCode, GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum codeType,
                                                  out string HoldingAccount, out bool HoldingAccountIsAvaliable,
                                                  out string CapitalAccount, out bool CapitalAccountIsAvaliable)
        {
            HoldingAccount = string.Empty;
            CapitalAccount = string.Empty;
            HoldingAccountIsAvaliable = false;
            CapitalAccountIsAvaliable = false;
            CM_BreedClass cmbc = MCService.CommonPara.GetBreedClassByCommodityCode(strCode, codeType);
            if (cmbc != null)
            {
                HoldingAccount = this.GetAccountByTraderInfo(strTraderId, cmbc.AccountTypeIDHold.Value, out HoldingAccountIsAvaliable);
                CapitalAccount = this.GetAccountByTraderInfo(strTraderId, cmbc.AccountTypeIDFund.Value, out CapitalAccountIsAvaliable);
            }
        }
        /// <summary>
        /// 依据交易员ID,代码获取资金,持仓帐户信息
        /// </summary>
        /// <param name="strTraderId"></param>
        /// <param name="strCode"></param>
        /// <param name="HoldingAccount"></param>
        /// <param name="HoldingAccountIsAvaliable"></param>
        /// <param name="CapitalAccount"></param>
        /// <param name="CapitalAccountIsAvaliable"></param>
        /// <returns></returns>
        public void GetHoldingAccountByTraderInfo(string strTraderId, string strCode,
                                                  out string HoldingAccount, out bool HoldingAccountIsAvaliable,
                                                  out string CapitalAccount, out bool CapitalAccountIsAvaliable)
        {
            #region old code update by 李健华 2009-10-26
            //HoldingAccount = string.Empty;
            //CapitalAccount = string.Empty;
            //HoldingAccountIsAvaliable = false;
            //CapitalAccountIsAvaliable = false;
            //CM_BreedClass cmbc = MCService.CommonPara.GetBreedClassByCommodityCode(strCode, codeType);
            //if (cmbc != null)
            //{
            //    HoldingAccount = this.GetAccountByTraderInfo(strTraderId, cmbc.AccountTypeIDHold.Value,
            //                                                 out HoldingAccountIsAvaliable);
            //    CapitalAccount = this.GetAccountByTraderInfo(strTraderId, cmbc.AccountTypeIDFund.Value,
            //                                                 out CapitalAccountIsAvaliable);
            //}
            #endregion

            GetHoldingAccountByTraderInfo(strTraderId, strCode, GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.Stock,
                                            out HoldingAccount, out HoldingAccountIsAvaliable,
                                            out CapitalAccount, out CapitalAccountIsAvaliable);
        }

        #region  注释 2010-06-08 李健华
        ///// <summary>
        ///// 依据交易员ID,帐户类型取帐户信息
        ///// </summary>
        ///// <param name="strTraderId"></param>
        ///// <param name="iAccountTypeId"></param>
        ///// <param name="AccountIsAvaliable"></param>
        ///// <returns></returns>
        //private string GetAccountByTraderInfoFromVTTraders(string strTraderId, int iAccountTypeId, out bool AccountIsAvaliable)
        //{
        //    string result = string.Empty;
        //    AccountIsAvaliable = false;
        //    try
        //    {
        //        var trader = stockTraders.GetByUserID(strTraderId);
        //        if (trader == null)
        //        {
        //            trader = futureTraders.GetByUserID(strTraderId);
        //        }

        //        if (trader == null)
        //        {
        //            LogHelper.WriteInfo("CounterCache.GetAccountByTraderInfo无法获取Trader，TraderID=" + strTraderId);
        //            return "";
        //        }

        //        var list = trader.AccountPairList;

        //        if (list != null)
        //        {
        //            foreach (var pair in list)
        //            {
        //                if (pair.CapitalAccount.AccountTypeLogo == iAccountTypeId)
        //                {
        //                    result = pair.CapitalAccount.UserAccountDistributeLogo;
        //                    AccountIsAvaliable = pair.CapitalAccount.WhetherAvailable;
        //                }
        //                else if (pair.HoldAccount.AccountTypeLogo == iAccountTypeId)
        //                {
        //                    result = pair.HoldAccount.UserAccountDistributeLogo;
        //                    AccountIsAvaliable = pair.HoldAccount.WhetherAvailable;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(ex.Message, ex);
        //    }

        //    return result;
        //}

        #endregion

        /// <summary>
        /// 依据交易员ID,帐户类型取帐户信息
        /// </summary>
        /// <param name="strTraderId"></param>
        /// <param name="iAccountTypeId"></param>
        /// <param name="AccountIsAvaliable"></param>
        /// <returns></returns>
        private string GetAccountByTraderInfo(string strTraderId, int iAccountTypeId, out bool AccountIsAvaliable)
        {
            #region new  2010-06-08 by 李健华
            string result = string.Empty;
            AccountIsAvaliable = false;
            try
            {
                UA_UserAccountAllocationTableInfo model = AccountManager.Instance.GetAccountByUserIDAndAccountType(strTraderId, iAccountTypeId);
                if (model != null)
                {
                    result = model.UserAccountDistributeLogo;
                    AccountIsAvaliable = model.WhetherAvailable;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;

            #endregion

            #region old code 2010-06-08 update by 李健华
            //string result = string.Empty;
            //AccountIsAvaliable = false;
            //try
            //{
            //    List<UA_UserAccountAllocationTableInfo> list = null;

            //    _userAccountCacheLock.EnterReadLock();
            //    try
            //    {
            //        list = _userAccountCache.GetByKey(strTraderId);
            //    }
            //    finally
            //    {
            //        _userAccountCacheLock.ExitReadLock();
            //    }

            //    if (list == null)
            //    {
            //        UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
            //        string where = string.Format("UserID = '{0}' ", strTraderId);
            //        list = dal.GetListArray(where);

            //        _userAccountCacheLock.EnterWriteLock();
            //        try
            //        {
            //            _userAccountCache.Add(list, strTraderId);
            //        }
            //        finally
            //        {
            //            _userAccountCacheLock.ExitWriteLock();
            //        }
            //    }

            //    if (list != null)
            //    {
            //        foreach (UA_UserAccountAllocationTableInfo accountAllocationTable in list)
            //        {
            //            if (accountAllocationTable.AccountTypeLogo == iAccountTypeId)
            //            {
            //                result = accountAllocationTable.UserAccountDistributeLogo;
            //                AccountIsAvaliable = accountAllocationTable.WhetherAvailable;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //}

            //return result;
            #endregion
        }

        #endregion
    }
    /// <summary>
    /// 交易员现货柜台缓存信息类
    /// </summary>
    public class XHCounterCache : CounterCache
    {
        private static volatile CounterCache _instance;

        private static object lockObject = new object();

        #region Overrides of CounterCache
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMcOrderNo"></param>
        /// <returns></returns>
        protected override OrderCacheItem LoadCacheItemFromDB(string strMcOrderNo)
        {
            OrderCacheItem result = null;
            string where = string.Format("McOrderId = '{0}' ", strMcOrderNo);

            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            var tets = dal.GetListArrayWithNoLock(where);

            if (!Utils.IsNullOrEmpty(tets))
            {
                try
                {
                    var tet = tets[0];
                    var cacheItem = new OrderCacheItem(tet.CapitalAccount, tet.StockAccount,
                                                       tet.EntrustNumber,
                                                       (
                                                       GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                       Enum.Parse(
                                                           typeof(
                                                               GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                           tet.BuySellTypeId.ToString
                                                               ()));
                    cacheItem.EntrustAmount = tet.EntrustAmount;
                    cacheItem.Code = tet.SpotCode;
                    var user = AccountManager.Instance.GetUserByAccount(tet.CapitalAccount);
                    if (user != null)
                    {
                        cacheItem.TraderId = user.UserID;
                    }
                    //else
                    //{
                    //    cacheItem.TraderId = GetTraderIdByFundAccount(tet.CapitalAccount); //TODO:需要根据资金账户获取用户ID
                    //}

                    AddOrderMappingInfo(tet.McOrderId, cacheItem);
                    result = cacheItem;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 单例
        /// </summary>
        public static CounterCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new XHCounterCache();
                        }
                    }
                }
                return _instance;
            }
        }
    }
    /// <summary>
    ///  交易员期货柜台缓存信息类
    /// </summary>
    public class QHCounterCache : CounterCache
    {
        private static volatile CounterCache _instance;

        private static object lockObject = new object();

        #region Overrides of CounterCache
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMcOrderNo"></param>
        /// <returns></returns>
        protected override OrderCacheItem LoadCacheItemFromDB(string strMcOrderNo)
        {
            OrderCacheItem result = null;
            string where = string.Format("McOrderId = '{0}' ", strMcOrderNo);

            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            var tets = dal.GetListArrayWithNoLock(where);

            if (!Utils.IsNullOrEmpty(tets))
            {
                try
                {
                    var tet = tets[0];
                    var cacheItem = new OrderCacheItem(tet.CapitalAccount, tet.TradeAccount,
                                                       tet.EntrustNumber,
                                                       (
                                                       GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                       Enum.Parse(
                                                           typeof(
                                                               GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                           tet.BuySellTypeId.ToString
                                                               ()));
                    cacheItem.EntrustAmount = tet.EntrustAmount;
                    cacheItem.Code = tet.ContractCode;
                    var user = AccountManager.Instance.GetUserByAccount(tet.CapitalAccount);
                    if (user != null)
                    {
                        cacheItem.TraderId = user.UserID;
                    }
                    //else
                    //{
                    //    cacheItem.TraderId = GetTraderIdByFundAccount(tet.CapitalAccount); //TODO:需要根据资金账户获取用户ID
                    //}

                    AddOrderMappingInfo(tet.McOrderId, cacheItem);
                    result = cacheItem;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 单例
        /// </summary>
        public static CounterCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new QHCounterCache();
                        }
                    }
                }
                return _instance;
            }
        }
    }
    /// <summary>
    ///  交易员港股柜台缓存信息类
    /// </summary>
    public class HKCounterCache : CounterCache
    {
        private static volatile CounterCache _instance;

        private static object lockObject = new object();

        #region Overrides of CounterCache
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMcOrderNo"></param>
        /// <returns></returns>
        protected override OrderCacheItem LoadCacheItemFromDB(string strMcOrderNo)
        {
            OrderCacheItem result = null;
            string where = string.Format("McOrderId = '{0}' ", strMcOrderNo);

            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
            var tets = dal.GetListArrayWithNoLock(where);

            if (!Utils.IsNullOrEmpty(tets))
            {
                try
                {
                    var tet = tets[0];
                    var cacheItem = new OrderCacheItem(tet.CapitalAccount, tet.HoldAccount,
                                                       tet.EntrustNumber,
                                                       (
                                                       GTA.VTS.Common.CommonObject.Types.TransactionDirection)
                                                       Enum.Parse(
                                                           typeof(
                                                               GTA.VTS.Common.CommonObject.Types.TransactionDirection),
                                                           tet.BuySellTypeID.ToString
                                                               ()));
                    cacheItem.EntrustAmount = tet.EntrustAmount;
                    cacheItem.Code = tet.Code;
                    var user = AccountManager.Instance.GetUserByAccount(tet.CapitalAccount);
                    if (user != null)
                    {
                        cacheItem.TraderId = user.UserID;
                    }
                    //else
                    //{
                    //    cacheItem.TraderId = GetTraderIdByFundAccount(tet.CapitalAccount); //TODO:需要根据资金账户获取用户ID
                    //}

                    AddOrderMappingInfo(tet.McOrderID, cacheItem);
                    result = cacheItem;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 单例
        /// </summary>
        public static CounterCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new HKCounterCache();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}