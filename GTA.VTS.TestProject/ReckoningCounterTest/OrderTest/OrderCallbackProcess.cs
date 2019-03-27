using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounterTest.CounterOrderService;

namespace ReckoningCounterTest.OrderTest
{
    class OrderCallbackProcess : IOrderDealRptCallback
    {
        #region IOrderDealRptCallback Members

        public event EventHandler Handler = null;


        public void ProcessStockDealRpt(StockDealOrderPushBack drsip)
        {
            if (Handler != null)
                Handler("DealRpt: OrderId" + drsip.OrderId + " OrderStatus:" + drsip.OrderState.ToString(), null);
        }

        public void ProcessMercantileDealRpt(FutureDealOrderPushBack drmip)
        {
            if (Handler != null)
                Handler("DealRpt: OrderId" + drmip.OrderId + " OrderStatus:" + drmip.OrderState.ToString(), null);
        }

        public void ProcessStockIndexFuturesDealRpt(FutureDealOrderPushBack drsifi)
        {
            if (Handler != null)
                Handler("DealRpt: OrderId" + drsifi.OrderId + " OrderStatus:" + drsifi.OrderState.ToString(), null);
            
        }

        #endregion
    }
}
