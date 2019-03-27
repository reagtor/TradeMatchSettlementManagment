using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.PushBackTest.DoDealRptService;
using ReckoningCounter.PushBackTest.DoOrderService;

namespace ReckoningCounter.PushBackTest
{
    public class GZQHMessageLogic
    {
        private SyncCache<string, QHMessage> qhMessageCache = new SyncCache<string, QHMessage>();
        private List<QHMessage> qhMessageList = new List<QHMessage>();
        protected ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();

        public List<QHMessage> MessageList
        {
            get { return qhMessageList; }
        }

        public bool HasChanged
        {
            get;
            set;
        }

        public void ProcessDoOrder(OrderResponse response, StockIndexFuturesOrderRequest request)
        {
            QHMessage message = new QHMessage();
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
            message.OpenClose = Utils.GetFutureOpenCloseType(request.OpenCloseType);

            qhMessageList.Add(message);
            if (!response.IsSuccess)
            {
                message.EntrustNumber = Guid.NewGuid().ToString();
                message.OrderStatus = "废单06";
            }

            qhMessageCache.Add(message.EntrustNumber, message);

            HasChanged = true;
        }

        public void ProcessPushBack(FutureDealOrderPushBack drsifi)
        {
            var tet = drsifi.StockIndexFuturesOrde;
            var deals = drsifi.FutureDealList;

            string entrustNumber = tet.EntrustNumber;
            if (!qhMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = qhMessageCache.Get(entrustNumber);
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

        public void UpdateMessage(string entrustNumber,string msg)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            if (!qhMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = qhMessageCache.Get(entrustNumber);
            message.OrderMessage = msg;

            HasChanged = true;
        }

        public void ClearAll()
        {
            listLock.EnterWriteLock();
            try
            {
                qhMessageList.Clear();
                HasChanged = true;
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }
    }
}
