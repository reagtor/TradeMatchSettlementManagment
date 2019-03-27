#region Using Namespace

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Timers;
using Amib.Threading;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.Reckoning.Logic.GZQH;
using ReckoningCounter.BLL.Reckoning.Logic.HK;
using ReckoningCounter.BLL.Reckoning.Logic.SPQH;
using ReckoningCounter.BLL.Reckoning.Logic.XH;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.MemoryData;
using ReckoningCounter.Model;
using Timer = System.Timers.Timer;

#endregion

namespace ReckoningCounter.BLL.Reckoning.Logic
{
    /// <summary>
    /// 清算处理单元管理器
    /// 作者：宋涛
    /// 日期：20090210
    /// </summary>
    public abstract class ReckonUnitManager<T> where T : IReckonUnit
    {
        private IDictionary<string, T> cache = new Dictionary<string, T>();
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 用于总冻结校验的计数器
        /// </summary>
        private int i;

        //private ReckonCommitTimer timer;
        /// <summary>
        /// 是否正在做定时清算
        /// </summary>
        private bool isChecking;

        /// <summary>
        /// 是否正在做定时总冻结资金校验
        /// </summary>
        private bool isCheckingFreeze;

        protected string name = "ReckonUnitManager";

        protected SmartThreadPool smartPool = new SmartThreadPool();

        /// <summary>
        /// 定时清算的定时器
        /// </summary>
        private Timer timer;

        /// <summary>
        /// 定时进行总冻结资金校验的定时器
        /// </summary>
        private Timer timer2;

        protected ReckonUnitManager()
        {
            InitializeThreadPool();

            //timer = new ReckonCommitTimer(Utils.GetReckonCommitInterval());
            //timer.Start();
            timer = new Timer();
            timer.Interval = Utils.GetReckonCommitInterval();
            timer.Elapsed += TimeCheck;
            timer.Enabled = true;

            timer2 = new Timer();
            timer2.Interval = Utils.GetReckonCommitInterval() * 5 + 500;
            timer2.Elapsed += TimeCheck2;
        }

        /// <summary>
        /// 进行定时清算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeCheck(object sender, ElapsedEventArgs e)
        {
            if (isChecking)
                return;

            //如果正在盘后清算，不校验
            if (ScheduleManager.IsFutureReckoning || ScheduleManager.IsHKReckoning || ScheduleManager.IsStockReckoning)
            {
                return;
            }

            //如果内存表没有启动，不校验
            if (MemoryDataManager.Status == MemoryDataManagerStatus.Close)
                return;

            isChecking = true;
            cacheLock.EnterReadLock();
            try
            {
                if (cache.Count > 0)
                {
                    foreach (var value in cache.Values)
                    {
                        value.DoReckonCommitCheck(this, EventArgs.Empty);
                    }
                }
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            isChecking = false;
        }

        /// <summary>
        /// 进行定时总冻结资金校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeCheck2(object sender, ElapsedEventArgs e)
        {
            if (isCheckingFreeze)
                return;

            //如果正在盘后清算，不校验
            if (ScheduleManager.IsFutureReckoning || ScheduleManager.IsHKReckoning || ScheduleManager.IsStockReckoning)
            {
                return;
            }

            //如果内存表没有启动，不校验
            if (MemoryDataManager.Status == MemoryDataManagerStatus.Close)
                return;

            isCheckingFreeze = true;
            DoCheck();

            cacheLock.EnterReadLock();
            try
            {
                if (cache.Count == 0)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            //如果连续5次检查，清算单元列表都为空，那么停止冻结资金检查
            if (i == 5)
            {
                i = 0;
                LogHelper.WriteDebug("$$$$$" + name + "停止冻结资金检查");
                timer2.Enabled = false;
            }

            isCheckingFreeze = false;
        }

        /// <summary>
        /// 进行资金总冻结资金\持仓总冻结持仓\保证金校验
        /// </summary>
        protected abstract void DoCheck();

        private void InitializeThreadPool()
        {
            int offerThreadCount = 200;
            try
            {
                string offerThread = ConfigurationManager.AppSettings["dealThread"];
                if (!string.IsNullOrEmpty(offerThread))
                {
                    int count = 0;
                    bool isSuccess = int.TryParse(offerThread.Trim(), out count);
                    if (isSuccess)
                        offerThreadCount = count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            smartPool.MaxThreads = offerThreadCount;
            smartPool.MinThreads = 25;
            smartPool.Start();
        }


        /// <summary>
        /// 根据撮合中心的委托单号获取一个处理单元
        /// </summary>
        /// <param name="matchOrderNo">撮合中心的委托单号</param>
        /// <returns>处理单元</returns>
        public T GetReckonUnitByMatchOrderNo(string matchOrderNo)
        {
            if (string.IsNullOrEmpty(matchOrderNo))
                return default(T);

            T result = default(T);
            cacheLock.EnterReadLock();
            try
            {
                if (cache.ContainsKey(matchOrderNo))
                {
                    result = cache[matchOrderNo];
                }
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return result;
        }

        /// <summary>
        /// 根据撮合中心的委托单号添加一个处理单元
        /// </summary>
        /// <param name="matchOrderNo">撮合中心的委托单号</param>
        /// <param name="unit">处理单元</param>
        /// <returns>是否成功</returns>
        public bool AddReckonUnitByMatchOrderNo(string matchOrderNo, T unit)
        {
            if (string.IsNullOrEmpty(matchOrderNo))
                return false;

            bool result = false;
            cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (!cache.ContainsKey(matchOrderNo))
                {
                    cacheLock.EnterWriteLock();
                    try
                    {
                        LogHelper.WriteDebug("ReckonUnitManager.AddReckonUnitByMatchOrderNo根据撮合中心的委托单号添加一个处理单元,,委托单号=" +
                                             matchOrderNo);
                        cache[matchOrderNo] = unit;
                        unit.SmartPool = smartPool;
                        //timer.Check += unit.DoReckonCommitCheck;
                        result = true;

                        //只要有清算单元进来，就开启冻结资金检查
                        if (!timer2.Enabled)
                        {
                            LogHelper.WriteDebug("$$$$$" + name + "开启冻结资金检查");
                            timer2.Enabled = true;
                            i = 0;
                        }
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }

            return result;
        }

        /// <summary>
        /// 根据撮合中心的委托单号移除一个处理单元
        /// </summary>
        /// <param name="matchOrderNo">撮合中心的委托单号</param>
        /// <param name="ost">当前委托状态</param>
        /// <returns>是否成功</returns>
        public bool RemoveReckonUnitByMatchOrderNo(string matchOrderNo, Types.OrderStateType ost)
        {
            if (string.IsNullOrEmpty(matchOrderNo))
                return false;

            bool result = false;
            cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(matchOrderNo))
                {
                    if (ost == Types.OrderStateType.DOSCanceled ||
                        ost == Types.OrderStateType.DOSDealed ||
                        ost == Types.OrderStateType.DOSPartRemoved ||
                        ost == Types.OrderStateType.DOSRemoved)
                    {
                        cacheLock.EnterWriteLock();
                        try
                        {
                            LogHelper.WriteDebug(
                                "ReckonUnitManager.RemoveReckonUnitByMatchOrderNo根据撮合中心的委托单号移除一个处理单元,委托单号=" +
                                matchOrderNo);
                            T unit = cache[matchOrderNo];
                            cache.Remove(matchOrderNo);
                            //timer.Check -= unit.DoReckonCommitCheck;
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                    }
                    result = true;
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }

            return result;
        }
    }

    /// <summary>
    /// 定时清算提交通知类
    /// </summary>
    internal class ReckonCommitTimer
    {
        private readonly Object mEventLock = new Object();
        private readonly Timer timer = new Timer();
        private EventHandler eventHandler;

        internal ReckonCommitTimer(int interval)
        {
            timer.Interval = interval;
            timer.Elapsed += delegate
                                 {
                                     EventArgs e = new EventArgs();
                                     OnCheck(e);
                                 };
        }

        internal void Start()
        {
            timer.Enabled = true;
        }

        internal void Stop()
        {
            timer.Enabled = false;
        }

        internal event EventHandler Check
        {
            //显式实现'add'方法
            add
            {
                //加私有锁，并向委托链表增加一个处理程序(以'value'为参数)
                lock (mEventLock)
                {
                    eventHandler += value;
                }
            }
            //显式实现'remove'方法
            remove
            {
                //加私有锁，并从委托链表从中移除处理程序(以'value'为参数)
                lock (mEventLock)
                {
                    eventHandler -= value;
                }
            }
        }

        protected virtual void OnCheck(EventArgs e)
        {
            //出于线程安全的考虑，将委托字段保存到一个临时字段中
            EventHandler temp = eventHandler;

            //通知所有已订阅事件的对象
            if (temp != null)
                temp(this, e);
        }
    }

    /// <summary>
    /// 现货清算单元管理器
    /// </summary>
    public class XHReckonUnitManager : ReckonUnitManager<XHReckonUnit>
    {
        private static XHReckonUnitManager instance = new XHReckonUnitManager { name = "XHReckonUnitManager" };

        public static XHReckonUnitManager GetInstance()
        {
            return instance;
        }

        #region Overrides of ReckonUnitManager<XHReckonUnit>

        protected override void DoCheck()
        {
            DoCapitalFreezeCheck();
            //DoHoldFreezeCheck();
        }

        private void DoHoldFreezeCheck()
        {
            var holdMemoryTables = MemoryDataManager.XHHoldMemoryList.GetAll();
            foreach (var holdMemoryTable in holdMemoryTables)
            {
                var hold = holdMemoryTable.Data;
                try
                {
                    int accountHoldId = hold.AccountHoldLogoId;

                    XH_AcccountHoldFreezeTableDal freezeTableDal = new XH_AcccountHoldFreezeTableDal();
                    int sum = freezeTableDal.GetAllFreezeAmount(accountHoldId);
                    holdMemoryTable.ReadAndWrite(h =>
                                                     {
                                                         if (h.FreezeAmount != sum)
                                                         {
                                                             string format =
                                                                 "XHReckonUnitManager.DoHoldFreezeCheck[AccountHoldLogoId={0},FreezeAmount={1},TrueSum={2}]";
                                                             string desc = string.Format(format, accountHoldId,
                                                                                         h.FreezeAmount, sum);
                                                             LogHelper.WriteDebug(desc);

                                                             h.FreezeAmount = sum;
                                                         }

                                                         return h;
                                                     });
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        private void DoCapitalFreezeCheck()
        {
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            var list = dal.GetAllFreezeMoney();
            foreach (var freezeSum in list)
            {
                //InternalDoCheck(freezeSum);
                smartPool.QueueWorkItem(InternalDoCheck, freezeSum);
            }
        }

        private void InternalDoCheck(XH_CapitalAccountFreezeSum freezeSum)
        {
            int capitalAccountId = freezeSum.CapitalAccountLogo;
            decimal sum = freezeSum.FreezeCapitalSum;

            var capMemory = MemoryDataManager.XHCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountId);
            if (capMemory == null)
            {
                return;
            }

            var capital = capMemory.Data;

            if (capital.FreezeCapitalTotal != sum)
            {
                decimal oldSum = capital.FreezeCapitalTotal;

                XH_CapitalAccountTable_DeltaInfo deltaInfo = new XH_CapitalAccountTable_DeltaInfo();
                deltaInfo.CapitalAccountLogo = capitalAccountId;
                deltaInfo.FreezeCapitalTotalDelta = sum - oldSum;
                capMemory.AddDelta(deltaInfo);

                string format2 = "XHReckonUnitManager.DoCapitalFreezeCheck修正现货总冻结资金[初始总冻结资金={0},实际总冻结资金={1},资金账户ID={2}]";
                string desc = string.Format(format2, oldSum, sum, capitalAccountId);
                LogHelper.WriteDebug(desc);
            }
        }

        #endregion
    }

    /// <summary>
    /// 现货清算单元管理器
    /// </summary>
    public class HKReckonUnitManager : ReckonUnitManager<HKReckonUnit>
    {
        private static HKReckonUnitManager instance = new HKReckonUnitManager { name = "HKReckonUnitManager" };

        public static HKReckonUnitManager GetInstance()
        {
            return instance;
        }

        #region Overrides of ReckonUnitManager<XHReckonUnit>

        protected override void DoCheck()
        {
            DoCapitalFreezeCheck();
            //DoHoldFreezeCheck();
        }

        private void DoHoldFreezeCheck()
        {
            var holdMemoryTables = MemoryDataManager.HKHoldMemoryList.GetAll();
            foreach (var holdMemoryTable in holdMemoryTables)
            {
                var hold = holdMemoryTable.Data;
                try
                {
                    int accountHoldId = hold.AccountHoldLogoID;

                    HK_AcccountHoldFreezeDal freezeTableDal = new HK_AcccountHoldFreezeDal();
                    int sum = freezeTableDal.GetAllFreezeAmount(accountHoldId);
                    holdMemoryTable.ReadAndWrite(h =>
                                                     {
                                                         if (h.FreezeAmount != sum)
                                                         {
                                                             string format =
                                                                 "HKReckonUnitManager.DoHoldFreezeCheck[AccountHoldLogoId={0},FreezeAmount={1},TrueSum={2}]";
                                                             string desc = string.Format(format, accountHoldId,
                                                                                         h.FreezeAmount, sum);
                                                             LogHelper.WriteDebug(desc);

                                                             h.FreezeAmount = sum;
                                                         }

                                                         return h;
                                                     });
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        private void DoCapitalFreezeCheck()
        {
            HK_CapitalAccountFreezeDal dal = new HK_CapitalAccountFreezeDal();
            var list = dal.GetAllFreezeMoney();
            foreach (var freezeSum in list)
            {
                //InternalDoCheck(freezeSum);
                smartPool.QueueWorkItem(InternalDoCheck, freezeSum);
            }
        }

        private void InternalDoCheck(HK_CapitalAccountFreezeSum freezeSum)
        {
            int capitalAccountId = freezeSum.CapitalAccountLogo;
            decimal sum = freezeSum.FreezeCapitalSum;

            var capMemory = MemoryDataManager.HKCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountId);
            if (capMemory == null)
            {
                return;
            }

            var capital = capMemory.Data;

            if (capital.FreezeCapitalTotal != sum)
            {
                decimal oldSum = capital.FreezeCapitalTotal;

                HK_CapitalAccount_DeltaInfo deltaInfo = new HK_CapitalAccount_DeltaInfo();
                deltaInfo.CapitalAccountLogo = capitalAccountId;
                deltaInfo.FreezeCapitalTotalDelta = sum - oldSum;
                capMemory.AddDelta(deltaInfo);

                string format2 = "HKReckonUnitManager.DoCapitalFreezeCheck修正现货总冻结资金[初始总冻结资金={0},实际总冻结资金={1},资金账户ID={2}]";
                string desc = string.Format(format2, oldSum, sum, capitalAccountId);
                LogHelper.WriteDebug(desc);
            }
        }

        #endregion
    }


    /// <summary>
    /// 股指期货清算单元管理器
    /// </summary>
    public class GZQHReckonUnitManager : ReckonUnitManager<GZQHReckonUnit>
    {
        private static GZQHReckonUnitManager instance = new GZQHReckonUnitManager { name = "GZQHReckonUnitManager" };

        public static GZQHReckonUnitManager GetInstance()
        {
            return instance;
        }

        #region Overrides of ReckonUnitManager<GZQHReckonUnit>

        protected override void DoCheck()
        {
            DoCapitalFreezeCheck();

            DoMarginCheck();
        }

        private void DoCapitalFreezeCheck()
        {
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            var list = dal.GetAllFreezeMoney();

            foreach (var freezeSum in list)
            {
                //InternalDoCapitalFreezeCheck(freezeSum);
                smartPool.QueueWorkItem(InternalDoCapitalFreezeCheck, freezeSum);
            }
        }

        private void InternalDoCapitalFreezeCheck(QH_CapitalAccountFreezeSum freezeSum)
        {
            int capitalAccountId = freezeSum.CapitalAccountLogo;
            decimal sum = freezeSum.FreezeCapitalSum;

            var capMemory = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountLogo(capitalAccountId);
            if (capMemory == null)
            {
                return;
            }

            var capital = capMemory.Data;

            if (capital.FreezeCapitalTotal != sum)
            {
                decimal oldSum = capital.FreezeCapitalTotal;

                QH_CapitalAccountTable_DeltaInfo deltaInfo = new QH_CapitalAccountTable_DeltaInfo();
                deltaInfo.CapitalAccountLogoId = capitalAccountId;
                deltaInfo.FreezeCapitalTotalDelta = sum - oldSum;
                capMemory.AddDelta(deltaInfo);

                string format2 =
                    "GZQHReckonUnitManager.DoCapitalFreezeCheck修正股指期货总冻结资金[初始总冻结资金={0},实际总冻结资金={1},资金账户ID={2}]";
                string desc = string.Format(format2, oldSum, sum, capitalAccountId);
                LogHelper.WriteDebug(desc);
            }
        }

        /// <summary>
        /// 检查资金表中的保证金总值是否等于所有持仓的保证金累加
        /// </summary>
        private void DoMarginCheck()
        {
            QH_HoldAccountTableDal holdDal = new QH_HoldAccountTableDal();
            var list = holdDal.GetMarginSum();
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            foreach (var hold in list)
            {
                //InternalDoMarginCheck(hold);
                smartPool.QueueWorkItem(InternalDoMarginCheck, hold);
            }
        }

        private void InternalDoMarginCheck(QH_HoldAccountTableInfo hold)
        {
            try
            {
                var capitalAccountInfo = AccountManager.Instance.GetCapitalAccountByHoldAccount(hold.UserAccountDistributeLogo);

                string capitalAccount = capitalAccountInfo.UserAccountDistributeLogo;
                int currencyType = hold.TradeCurrencyType;

                var capMem = MemoryDataManager.QHCapitalMemoryList.GetByCapitalAccountAndCurrencyType(capitalAccount, currencyType);
                var capital = capMem.Data;

                decimal realMarginTotal = hold.Margin;
                decimal marginTotal = capital.MarginTotal;

                if (marginTotal != realMarginTotal)
                {
                    QH_CapitalAccountTable_DeltaInfo delta = new QH_CapitalAccountTable_DeltaInfo();

                    delta.CapitalAccountLogoId = capital.CapitalAccountLogoId;
                    delta.MarginTotalDelta = realMarginTotal - marginTotal;
                    capMem.AddDelta(delta);

                    string format2 =
                        "GZQHReckonUnitManager.DoMarginCheck修正股指期货总保证金[初始总保证金={0},实际总保证金={1},资金账户ID={2}]";
                    string desc = string.Format(format2, marginTotal, realMarginTotal, capital.CapitalAccountLogoId);
                    LogHelper.WriteDebug(desc);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// 商品期货清算单元管理器
    /// </summary>
    public class SPQHReckonUnitManager : ReckonUnitManager<SPQHReckonUnit>
    {
        private static SPQHReckonUnitManager instance = new SPQHReckonUnitManager { name = "SPQHReckonUnitManager" };

        public static SPQHReckonUnitManager GetInstance()
        {
            return instance;
        }

        #region Overrides of ReckonUnitManager<SPQHReckonUnit>

        protected override void DoCheck()
        {
        }

        #endregion
    }
}