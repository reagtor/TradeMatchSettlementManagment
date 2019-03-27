using GTA.VTS.CustomersOrders.DoDealRptService;
using Amib.Threading;


namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// Tilte;成交及委托回报回调    
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    public class OrderCallBack : IOrderDealRptCallback
    {

        /// <summary>
        /// 线程池
        /// </summary>
        private static SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 200, MinThreads = 25 };

        static OrderCallBack()
        {
            smartPool.Start();
        }

        /// <summary>
        /// 现货成交回报方法接口
        /// </summary>
        public static IOrderCallBackView<StockDealOrderPushBack> XHView
        {
            get;
            set;
        }

        /// <summary>
        /// 股指期货成交回报方法接口
        /// </summary>
        public static IOrderCallBackView<FutureDealOrderPushBack> SIView
        {
            get;
            set;
        }

        /// <summary>
        /// 商品期货成交回报方法接口
        /// </summary>
        public static IOrderCallBackView<FutureDealOrderPushBack> CFView
        {
            get;
            set;
        }

        /// <summary>
        /// 港股成交回报方法接口
        /// </summary>
        public static IOrderCallBackView<HKDealOrderPushBack> HKView
        {
            get;
            set;
        }

        /// <summary>
        /// 港股改单回报方法接口
        /// </summary>
        public static IOrderCallBackView<HKModifyOrderPushBack> HKModifyView
        {
            get;
            set;
        }

        #region Implementation of IOrderDealRptCallback

        /// <summary>
        /// 现货成交回报处理
        /// </summary>
        /// <param name="drsip">现货回推的委托数据</param>
        public void ProcessStockDealRpt(StockDealOrderPushBack drsip)
        {
            //Program.mainForm.ProcessXHBack(drsip);
            if (XHView != null)
            {
                smartPool.QueueWorkItem(XHView.ProcessPushBack, drsip);
            }
        }

        /// <summary>
        /// 商品期货成交回报处理
        /// </summary>
        /// <param name="drmip">商品期货回推的委托数据</param>
        public void ProcessMercantileDealRpt(FutureDealOrderPushBack drmip)
        {
            //Program.mainForm.ProcessSPQHBack(drmip);
            if (CFView != null)
            {
                smartPool.QueueWorkItem(CFView.ProcessPushBack, drmip);
            }
        }

        /// <summary>
        /// 股指期货成交回报处理
        /// </summary>
        /// <param name="drsifi">股指期货回推的委托数据</param>
        public void ProcessStockIndexFuturesDealRpt(FutureDealOrderPushBack drsifi)
        {
            //Program.mainForm.ProcessGZQHBack(drsifi);
            if (SIView != null)
            {
                smartPool.QueueWorkItem(SIView.ProcessPushBack, drsifi);
            }
        }

        /// <summary>
        /// 港股成交回报处理
        /// </summary>
        /// <param name="drsip">港股回推的委托数据</param>
        public void ProcessHKDealRpt(HKDealOrderPushBack drsip)
        {
            //Program.mainForm.ProcessHKBack(drsip);
            if (HKView != null)
            {
                smartPool.QueueWorkItem(HKView.ProcessPushBack, drsip);
            }
        }

        /// <summary>
        /// 港股改单回报处理
        /// </summary>
        /// <param name="mopb">港股改单回推的委托数据</param>
        public void ProcessHKModifyOrderRpt(HKModifyOrderPushBack mopb)
        {
            //Program.mainForm.ProcessHKModifyOrderBack(mopb);
            if (HKModifyView != null)
            {
                smartPool.QueueWorkItem(HKModifyView.ProcessPushBack, mopb);
            }
        }

        #endregion
    }
}
