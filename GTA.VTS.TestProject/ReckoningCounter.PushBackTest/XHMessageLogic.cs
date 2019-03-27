#region Using Namespace

using System.Collections.Generic;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.PushBackTest.DoDealRptService;
using ReckoningCounter.PushBackTest.DoOrderService;

#endregion

namespace ReckoningCounter.PushBackTest
{
    public class XHMessageLogic
    {
        private SyncCache<string, XHMessage> xhMessageCache = new SyncCache<string, XHMessage>();
        private List<XHMessage> xhMessageList = new List<XHMessage>();
        protected ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();


        public List<XHMessage> MessageList
        {
            get { return xhMessageList; }
        }

        public bool HasChanged
        {
            get; set;
        }

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

        public List<XHMessage> ClearAllFinalStateOrder()
        {
            List<XHMessage> list = new List<XHMessage>();
            listLock.EnterWriteLock();
            try
            {
                for (int i = xhMessageList.Count - 1; i >=0; i--)
                {
                    var message = xhMessageList[i];
                    bool isFinalState = Utils.IsFinalState(message.OrderStatus);
                    if(isFinalState)
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

        public void ProcessPushBack(StockDealOrderPushBack drsip)
        {
            var tet = drsip.StockOrderEntity;
            var deals = drsip.StockDealList;

            string entrustNumber = tet.EntrustNumber;
            if (!xhMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = xhMessageCache.Get(entrustNumber);
            message.EntrustAmount = tet.EntrustAmount.ToString();
            message.TradeAmount = tet.TradeAmount.ToString();
            message.CancelAmount = tet.CancelAmount.ToString();
            message.OrderMessage = tet.OrderMessage;
            message.OrderStatus = Utils.GetOrderStateMsg(tet.OrderStatusId);

            if(deals.Count > 0)
            {
                message.TradeTime = deals[deals.Count - 1].TradeTime.ToString();
            }

            HasChanged = true;
        }

        public void UpdateMessage(string entrustNumber,string msg)
        {
            if(string.IsNullOrEmpty(entrustNumber))
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
    }
}