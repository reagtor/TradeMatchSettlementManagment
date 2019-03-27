#region Using Namespace

using System;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.Reckoning.Logic;

#endregion

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderAccepter.Logic
{
    /// <summary>
    /// 下单预处理逻辑
    /// </summary>
    /// <typeparam name="TRequest">原始请求</typeparam>
    /// <typeparam name="TEntrust">委托</typeparam>
    /// <typeparam name="TTrade">成交</typeparam>
    public abstract class AcceptLogic<TRequest, TEntrust, TTrade>
    {
        /// <summary>
        /// 交易员ID
        /// </summary>
        public string TradeID;

        /// <summary>
        /// 柜台缓存对象
        /// </summary>
        protected CounterCache counterCacher;// = CounterCache.Instance;

        /// <summary>
        /// 原始请求对象
        /// </summary>
        public TRequest Request { get; protected set; }

        /// <summary>
        /// 柜台委托单号
        /// </summary>
        public string EntrustNumber { get; protected set; }

        /// <summary>
        /// 商品代码
        /// </summary>
        public string Code { get; protected set; }

        //资金帐户
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
        /// 代码对应品种的交易币种
        /// </summary>
        public int CurrencyType
        {
            get { return currencyType; }
            protected set { currencyType = value; }
        }

        protected abstract CounterCache GetCounterCache();

        public AcceptLogic()
        {
            this.counterCacher = GetCounterCache();
        }

        /// <summary>
        /// 撤单校验-检查委托单当前状态是否可撤
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="tet">委托实体</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>校验是否通过</returns>
        public abstract bool CancelOrderValidate(string entrustNumber, out TEntrust tet,
                                                 ref string strMessage);

        /// <summary>
        /// 内部撤单
        /// </summary>
        /// <param name="tet">委托实体</param>
        /// <param name="strMcErrorMessage">错误信息</param>
        /// <returns>是否成功</returns>
        public abstract bool InternalCancelOrder(TEntrust tet, string strMcErrorMessage);

        /// <summary>
        /// 检验并持久化柜台委托单
        /// </summary>
        /// <param name="stockorder">原始委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <param name="outEntity">柜台委托</param>
        /// <returns>是否成功</returns>
        public abstract bool PersistentOrder(TRequest stockorder, ref TEntrust outEntity,
                                             ref string strMessage);

        /// <summary>
        /// 撤单回报清算完成通知事件
        /// </summary>
        public event Action<ReckonEndObject<TEntrust, TTrade>> EndCancelEvent;

        protected virtual void OnEndCancelEvent(ReckonEndObject<TEntrust, TTrade> cancelEndObject)
        {
            if (EndCancelEvent != null)
                EndCancelEvent(cancelEndObject);
        }
    }

    /// <summary>
    /// 用来标识接收委托持久化时检查资金或者持仓失败时触发的异常
    /// </summary>
    public class CheckException:Exception
    {
        public CheckException()
        {
            
        }

        public CheckException(string msg):base(msg)
        {
            
        }
    }
}