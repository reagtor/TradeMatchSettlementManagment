#region Using Namespace

using System;
using System.Collections.Generic;
using System.Threading;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;

#endregion

namespace ReckoningCounter.BLL.Reckoning.Logic
{
    /// <summary>
    /// 柜台委托单业务清算处理逻辑基类
    /// </summary>
    /// <typeparam name="TEntrustTable">当日委托表类型</typeparam>
    /// <typeparam name="TDealBack">成交回报类型</typeparam>
    /// <typeparam name="TTradeTable">当日成交表类型</typeparam>
    public abstract class ReckonUnitBase<TEntrustTable, TDealBack, TTradeTable> : IReckonUnit
    {
        #region 委托信息

        /// <summary>
        /// 交易员ID
        /// </summary>
        public string TradeID;

        /// <summary>
        /// 柜台委托单号
        /// </summary>
        public string EntrustNumber { get; protected set; }

        /// <summary>
        /// 商品代码
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        public decimal EntrustAmount { get; protected set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public decimal TradeAmount { get; protected set; }

        /// <summary>
        /// 撤单数量
        /// </summary>
        public decimal CancelAmount { get; protected set; }

        /// <summary>
        /// 资金帐户
        /// </summary>
        public string CapitalAccount { get; protected set; }

        private int capitalAccountId = -1;

        /// <summary>
        /// 资金账户id
        /// </summary>
        public int CapitalAccountId
        {
            get { return capitalAccountId; }
            protected set { capitalAccountId = value; }
        }

        /// <summary>
        /// 持仓帐户
        /// </summary>
        public string HoldingAccount { get; protected set; }

        private int holdingAccountId = -1;

        /// <summary>
        /// 持仓帐户id
        /// </summary>
        public int HoldingAccountId
        {
            get { return holdingAccountId; }
            protected set { holdingAccountId = value; }
        }

        private int currencyType = -1;

        /// <summary>
        /// 交易币种
        /// </summary>
        public int CurrencyType
        {
            get { return currencyType; }
            protected set { currencyType = value; }
        }

        /// <summary>
        /// 买卖方向
        /// </summary>
        public Types.TransactionDirection BuySellType { get; protected set; }

        private int captitalTradingRule = -1;

        /// <summary>
        /// 资金T+N规则
        /// </summary>
        public int CaptitalTradingRule
        {
            get { return captitalTradingRule; }
            protected set { captitalTradingRule = value; }
        }

        private int holdingTradingRule = -1;

        /// <summary>
        /// 资金T+N规则
        /// </summary>
        public int HoldingTradingRule
        {
            get { return holdingTradingRule; }
            protected set { holdingTradingRule = value; }
        }

        /// <summary>
        /// 委托缓存对象
        /// </summary>
        public OrderCacheItem OrderInfo { get; protected set; }

        #region 仅用于期货

        ////是否是强制平仓的过期合约生成的委托
        //protected bool IsExpiredContract { get; set; }

        ////是否是期货开盘检查中的资金检查Order
        //protected bool IsCheckCapitalOrder { get; set; }

        /// <summary>
        /// 是否是期货开盘前检查的强行平仓委托单(包括资金，持仓限制检查)
        /// </summary>
        public bool IsCheckForcedCloseOrder { get; set; }

        /// <summary>
        /// 期货强行平仓类型
        /// </summary>
        public Types.QHForcedCloseType QHForcedCloseType { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// 柜台缓存对象
        /// </summary>
        protected CounterCache counterCacher;// = CounterCache.Instance;

        /// <summary>
        /// 行情接口
        /// </summary>
        protected IRealtimeMarketService _realtimeService;

        /// <summary>
        /// 供外部插入消息的队列
        /// </summary>
        protected QueueBufferBase<object> externalMessageQueue = new QueueBufferBase<object>();

        /// <summary>
        /// 内部成交回报缓存列表
        /// </summary>
        protected SyncList<TDealBack> dealBackList = new SyncList<TDealBack>();

        /// <summary>
        /// 内部撤单回报缓存列表（应该只有一个）
        /// </summary>
        protected SyncList<CancelOrderEntity> cancelBackList = new SyncList<CancelOrderEntity>();

        /// <summary>
        /// 是否正在做定时清算提交
        /// </summary>
        protected bool isDoReckonCommitChecking;

        #region 成交回报校验字段

        /// <summary>
        /// 已经清算的id列表
        /// </summary>
        protected IList<string> hasReckonedIDList = new List<string>();

        private ReaderWriterLockSlim hasReckonedIDListLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 所有接收的id列表
        /// </summary>
        protected IList<string> idList = new List<string>();

        private ReaderWriterLockSlim idListLock = new ReaderWriterLockSlim();

        #endregion

        protected ReckonUnitBase()
        {
            CurrencyType = -1;
            CaptitalTradingRule = -1;
            HoldingTradingRule = -1;

            this.counterCacher = GetCounterCache();

            externalMessageQueue.QueueItemProcessEvent += ExternalMessageQueue_QueueItemProcessEvent;
        }

        /// <summary>
        /// 是否已经从数据库中加载清算过的ID
        /// </summary>
        protected bool HasLoadReckonedID { get; set; }

        #region IReckonUnit Members

        /// <summary>
        /// 清算通知检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void DoReckonCommitCheck(object sender, EventArgs args)
        {
            if (isDoReckonCommitChecking)
                return;

            try
            {
                SmartPool.QueueWorkItem(InternalDoReckonCommitCheck);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                Thread t = new Thread(InternalDoReckonCommitCheck);
                t.Start();
                //InternalDoReckonCommitCheck();
            }
        }

        public SmartThreadPool SmartPool
        {
            get;
            set;
        }

        private void InternalDoReckonCommitCheck()
        {
            isDoReckonCommitChecking = true;
            ReckonCommitCheck();
            isDoReckonCommitChecking = false;
        }

        #endregion

        private void ExternalMessageQueue_QueueItemProcessEvent(object sender, QueueItemHandleEventArg<object> e)
        {
            bool isLastMessage = InternalInsertMessage(e.Item);

            //最后一个消息时，马上进行清算，不再等待计时
            if (isLastMessage)
            {
                LogHelper.WriteInfo("InternalInsertMessage最后一个消息，马上进行清算[EntrustNumber=" + EntrustNumber + "]");
                DoReckonCommitCheck(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 内部成交回报接收处理方法,单线程逐个执行，由externalMessageQueue驱动
        /// </summary>
        /// <param name="message"></param>
        /// <returns>是否是最后一个消息</returns>
        protected abstract bool InternalInsertMessage(object message);

        /// <summary>
        /// 接收由撮合推回的成交回报
        /// </summary>
        /// <param name="message"></param>
        public void InsertMessage(object message)
        {
            externalMessageQueue.InsertQueueItem(message);
        }


        /// <summary>
        /// 成交回报清算完成通知事件
        /// </summary>
        public event Action<ReckonEndObject<TEntrustTable, TTradeTable>> EndReckoningEvent;

        protected virtual void OnEndReckoningEvent(ReckonEndObject<TEntrustTable, TTradeTable> reckonEndObject)
        {
            if (EndReckoningEvent != null)
                EndReckoningEvent(reckonEndObject);
        }

        /// <summary>
        /// 撤单回报清算完成通知事件
        /// </summary>
        public event Action<ReckonEndObject<TEntrustTable, TTradeTable>> EndCancelEvent;

        protected virtual void OnEndCancelEvent(ReckonEndObject<TEntrustTable, TTradeTable> cancelEndObject)
        {
            if (EndCancelEvent != null)
                EndCancelEvent(cancelEndObject);
        }

        protected abstract void ReckonCommitCheck();


        /// <summary>
        /// 添加需要清算的成交回报id到所有接收的id列表
        /// </summary>
        /// <param name="id">成交回报id</param>
        protected void AddID(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            idListLock.EnterUpgradeableReadLock();
            try
            {
                if (!idList.Contains(id))
                {
                    idListLock.EnterWriteLock();
                    try
                    {
                        idList.Add(id);
                    }
                    finally
                    {
                        idListLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                idListLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 从所有接收的id列表中移除成交回报id
        /// </summary>
        /// <param name="id">成交回报id</param>
        protected void RemoveID(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            idListLock.EnterUpgradeableReadLock();
            try
            {
                if (idList.Contains(id))
                {
                    idListLock.EnterWriteLock();
                    try
                    {
                        idList.Remove(id);
                    }
                    finally
                    {
                        idListLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                idListLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 从所有接收的id列表中移除成交回报id列表
        /// </summary>
        /// <param name="ids">成交回报id列表</param>
        protected void RemoveIDList(List<string> ids)
        {
            if (Utils.IsNullOrEmpty(ids))
                return;

            idListLock.EnterWriteLock();

            try
            {
                foreach (var id in ids)
                {
                    if (idList.Contains(id))
                    {
                        idList.Remove(id);
                    }
                }
            }
            finally
            {
                idListLock.ExitWriteLock();

            }
        }


        /// <summary>
        /// 判断是否已经添加过成交回报id
        /// </summary>
        /// <param name="id">成交回报id</param>
        protected bool HasAddId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            bool result = false;
            idListLock.EnterReadLock();
            try
            {
                if (idList.Contains(id))
                {
                    LogHelper.WriteInfo("ReckonUnitBase.HasAddId处理单元接收的回报ID重复,之前已接收过相同ID的回报，ID=" + id);
                    result = true;
                }
            }
            finally
            {
                idListLock.ExitReadLock();
            }

            return result;
        }

        /// <summary>
        /// 添加已经清算的成交回报id
        /// </summary>
        /// <param name="id">成交回报id</param>
        protected void AddReckonedID(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            hasReckonedIDListLock.EnterUpgradeableReadLock();
            try
            {
                if (!hasReckonedIDList.Contains(id))
                {
                    hasReckonedIDListLock.EnterWriteLock();
                    try
                    {
                        hasReckonedIDList.Add(id);
                    }
                    finally
                    {
                        hasReckonedIDListLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                hasReckonedIDListLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 添加已经清算的成交回报id
        /// </summary>
        /// <param name="id">成交回报id</param>
        protected void AddReckonedIDList(List<string> ids)
        {
            if (Utils.IsNullOrEmpty(ids))
                return;

            hasReckonedIDListLock.EnterWriteLock();

            try
            {
                foreach (var id in ids)
                {
                    if (!hasReckonedIDList.Contains(id))
                    {

                        hasReckonedIDList.Add(id);
                    }
                }
            }
            finally
            {
                hasReckonedIDListLock.ExitWriteLock();

            }
        }

        /// <summary>
        /// 从数据库中加载已经清算的成交回报id
        /// </summary>
        protected abstract void LoadReckonedIDList();

        /// <summary>
        /// 判断是否已经清算成交回报id
        /// </summary>
        /// <param name="id">成交回报id</param>
        protected bool HasAddReckoned(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            //if (hasReckonedIDList.Contains(id))
            //    return true;

            bool result = false;
            hasReckonedIDListLock.EnterReadLock();
            try
            {
                if (hasReckonedIDList.Contains(id))
                    result = true;
            }
            finally
            {
                hasReckonedIDListLock.ExitReadLock();
            }

            return result;
        }

        protected abstract CounterCache GetCounterCache();

        /// <summary>
        /// 初始化委托缓存信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        protected bool InitializeOrderCache(string orderNo, ref string strMessage)
        {
            //从缓存取撮合中心委托单与柜台委托单映射关系
            if (OrderInfo == null)
            {
                if (this.counterCacher == null)
                    this.counterCacher = GetCounterCache();

                OrderInfo = this.counterCacher.GetOrderMappingInfo(orderNo);
            }

            if (OrderInfo == null)
            {
                strMessage = "GT-2500:柜台撮合中心委托映射关系丢失";
                return false;
            }

            TradeID = OrderInfo.TraderId;

            //买卖方向
            BuySellType = OrderInfo.BuySellType;

            if (string.IsNullOrEmpty(CapitalAccount))
            {
                //资金帐户
                CapitalAccount = OrderInfo.CapitalAccount;
            }

            if (String.IsNullOrEmpty(HoldingAccount))
            {
                //持仓帐户
                HoldingAccount = OrderInfo.HoldingAccount;
            }

            if (String.IsNullOrEmpty(EntrustNumber))
            {
                //柜台委托单号
                EntrustNumber = OrderInfo.CounterOrderNo;

                if (!String.IsNullOrEmpty(EntrustNumber))
                {
                    //当发生故障恢复，加载已清算的id列表
                    if (TradeAmount == 0 && !HasLoadReckonedID)
                        LoadReckonedIDList();
                }
            }

            if (string.IsNullOrEmpty(Code))
            {
                Code = OrderInfo.Code;
            }

            if (EntrustAmount == 0)
            {
                EntrustAmount = OrderInfo.EntrustAmount;
            }

            //初始化是否为盘前检查强行平仓和强行平仓类型
            IsCheckForcedCloseOrder = OrderInfo.IsOpenMarketCheckOrder;
            QHForcedCloseType = OrderInfo.QHForcedCloseType;

            GetCurrencyType();
            if (CurrencyType == -1)
            {
                strMessage = "GT-2591:[清算初始化]无法获取交易币种";
                return false;
            }

            //获取账户ID
            GetAccountID();
            if (CapitalAccountId == -1)
            {
                strMessage = "GT-2592:[清算初始化]无法获取资金帐号ID";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取币种
        /// </summary>
        protected abstract void GetCurrencyType();

        /// <summary>
        /// 获取账户ID
        /// </summary>
        protected abstract void GetAccountID();
    }

    /// <summary>
    /// 清算提交完成通知对象
    /// </summary>
    /// <typeparam name="TEntrustTable">当日委托表</typeparam>
    /// <typeparam name="TTradeTable">当日成交</typeparam>
    public class ReckonEndObject<TEntrustTable, TTradeTable>
    {
        /// <summary>
        /// 清算对应的委托表
        /// </summary>
        public TEntrustTable EntrustTable;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess;

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Message;

        /// <summary>
        /// 交易员ID
        /// </summary>
        public string TradeID;

        /// <summary>
        /// 清算完的成交列表
        /// </summary>
        public IList<TTradeTable> TradeTableList;
    }
}