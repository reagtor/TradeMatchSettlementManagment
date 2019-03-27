using System.ServiceModel;
using ReckoningCounter.Entity;

namespace ReckoningCounter.BLL
{

    /// <summary>
    /// 成交及委托回报回调接口
    /// </summary> 
    /// 
    [ServiceContract]
    public interface IDoOrderCallback
    {
        /// <summary>
        /// 股票成交回报
        /// </summary>
        /// <param name="drsip"></param>
        /// 
        [OperationContract]
        void ProcessStockDealRpt(StockDealOrderPushBack drsip);

        /// <summary>
        /// 商品期货成交回报
        /// </summary>
        /// <param name="drmip"></param>
        /// 
        [OperationContract]
        void ProcessMercantileDealRpt(FutureDealOrderPushBack drmip);

        /// <summary>
        /// 股指期货成交回报
        /// </summary>
        /// <param name="drsifi"></param>
        /// 
        [OperationContract]
        void ProcessStockIndexFuturesDealRpt(FutureDealOrderPushBack drsifi);

        /// <summary>
        /// 港股成交回报
        /// </summary>
        /// <param name="drsip"></param>
        /// 
        [OperationContract]
        void ProcessHKDealRpt(HKDealOrderPushBack drsip);

        /// <summary>
        /// 港股改单回报
        /// </summary>
        /// <param name="mopb"></param>
        [OperationContract]
        void ProcessHKModifyOrderRpt(HKModifyOrderPushBack mopb);

    }
}
