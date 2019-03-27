#region Using Namespace

using System.Collections.Generic;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;

#endregion

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// 委托单号获取事件委托
    /// </summary>
    /// <param name="entrustNumber">委托单号</param>
    public delegate void EntrustNoEventHandler(string entrustNumber);

    /// <summary>
    /// Title: 现货消息逻辑类
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    public class XHMessageLogic : AutoCancleOperater
    {
        private SyncCache<string, XHMessage> xhMessageCache = new SyncCache<string, XHMessage>();
        private List<XHMessage> xhMessageList = new List<XHMessage>();
        protected ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 消息列表
        /// </summary>
        public List<XHMessage> MessageList
        {
            get { return xhMessageList; }
        }

        /// <summary>
        /// 消息内容是否改变
        /// </summary>
        public bool HasChanged
        {
            get;
            set;
        }

        /// <summary>
        /// 下单消息处理
        /// </summary>
        /// <param name="response">下单返回信息</param>
        /// <param name="request">下单请求</param>
        public void ProcessDoOrder(OrderResponse response, StockOrderRequest request)
        {
            XHMessage message = new XHMessage();
            message.BuySell = request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying ? "买" : "卖";
            message.CapitalAccount = request.FundAccountId;
            message.Code = request.Code;
            message.EntrustAmount = request.OrderAmount.ToString();
            message.EntrustNumber = response.OrderId;
            message.EntrustPrice = request.OrderPrice.ToString();
            message.EntrustType = request.OrderWay == TypesOrderPriceType.OPTLimited ? "限价" : "市价";
            message.OrderMessage = response.OrderMessage;
            message.OrderStatus = "未报02";
            message.TradeAmount = "0";
            message.TradeTime = "";

            listLock.EnterWriteLock();
            try
            {
                xhMessageList.Add(message);
            }
            finally
            {
                listLock.ExitWriteLock();
            }

            if (!response.IsSuccess)
            {
                message.EntrustNumber = System.Guid.NewGuid().ToString();
                message.OrderStatus = "废单06";
            }

            xhMessageCache.Add(message.EntrustNumber, message);

            HasChanged = true;
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        public void ClearAll()
        {
            listLock.EnterWriteLock();
            try
            {
                xhMessageList.Clear();
                HasChanged = true;
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 清空最终状态的委托消息
        /// </summary>
        /// <returns>消息列表</returns>
        public List<XHMessage> ClearAllFinalStateOrder()
        {
            List<XHMessage> list = new List<XHMessage>();
            listLock.EnterWriteLock();
            try
            {
                for (int i = xhMessageList.Count - 1; i >= 0; i--)
                {
                    var message = xhMessageList[i];
                    bool isFinalState = Utils.IsFinalState(message.OrderStatus);
                    if (isFinalState)
                    {
                        xhMessageList.Remove(message);
                        list.Add(message);
                        HasChanged = true;
                    }
                }
            }
            finally
            {
                listLock.ExitWriteLock();
            }

            return list;
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="drsip"></param>
        public void ProcessPushBack(StockDealOrderPushBack drsip)
        {
            var tet = drsip.StockOrderEntity;
            var deals = drsip.StockDealList;

            string entrustNumber = tet.EntrustNumber;
            if (!xhMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("[现货]委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = xhMessageCache.Get(entrustNumber);
            message.EntrustAmount = tet.EntrustAmount.ToString();
            message.TradeAmount = tet.TradeAmount.ToString();
            message.CancelAmount = tet.CancelAmount.ToString();
            message.OrderMessage = tet.OrderMessage;
            message.OrderStatus = Utils.GetOrderStateMsg(tet.OrderStatusId);

            if (deals.Count > 0)
            {
                message.TradeTime = deals[deals.Count - 1].TradeTime.ToString();
            }

            HasChanged = true;
        }

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="msg">消息文本</param>
        public void UpdateMessage(string entrustNumber, string msg)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            if (!xhMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = xhMessageCache.Get(entrustNumber);
            message.OrderMessage = msg;

            HasChanged = true;
        }

        /// <summary>
        /// 委托单号获取事件
        /// </summary>
        public static event EntrustNoEventHandler OnEntrustSelected;

        /// <summary>
        /// 触发委托单号获取事件
        /// </summary>
        /// <param name="entrustNumber"></param>
        public static void FireEntrustSelectedEvent(string entrustNumber)
        {
            if (OnEntrustSelected != null)
            {
                OnEntrustSelected(entrustNumber);
            }
        }
    }
}