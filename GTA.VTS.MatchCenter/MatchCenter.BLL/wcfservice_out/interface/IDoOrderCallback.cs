using System.ServiceModel;
using MatchCenter.Entity;
using MatchCenter.Entity.HK;

namespace MatchCenter.BLL.interfaces
{
    /// <summary>
    /// 撮合中心委托回报接口
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Desc.：添加港股相关的方法公开接口
    /// Update By:李健华
    /// Update Date:2009-10-19
    /// Desc.：添加商品期货相关的方法公开接口
    /// Update By:董鹏
    /// Update Date:2010-01-22
    /// </summary>
    [ServiceContract]
    public interface IDoOrderCallback
    {
        #region 成交回报
        /// <summary>
        /// 股票成交回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void ProcessStockDealRpt(StockDealBackEntity model);

        /// <summary>
        /// 商品期货成交回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void ProcessMercantileDealRpt(CommoditiesDealBackEntity model);

        /// <summary>
        /// 股指期货成交回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void ProcessStockIndexFuturesDealRpt(FutureDealBackEntity model);

        /// <summary>
        /// 港股成交回报
        /// </summary>
        /// <param name="model">港股成交回报实体</param>
        [OperationContract]
        void ProcessHKStockDealRpt(HKDealBackEntity model);
        #endregion

        #region 委托废单回报
        /// <summary>
        /// 股票委托回报（即委托废单回报）
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void ProcessStockOrderRpt(ResultDataEntity model);

        /// <summary>
        /// 商品期货委托回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void ProcessCommoditiesOrderRpt(ResultDataEntity model);

        /// <summary>
        /// 股指期货委托回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void ProcessStockIndexFuturesOrderRpt(ResultDataEntity model);
        /// <summary>
        /// 港股委托回报（即委托废单回报）
        /// </summary>
        /// <param name="model">港股委托回报实体</param>
        [OperationContract]
        void ProcessHKStockOrderRpt(ResultDataEntity model);
        #endregion

        #region 撤单回报
        /// <summary>
        /// 股票撤单回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void CancelStockOrderRpt(CancelOrderEntity model);

        #region  商品期货撤单回报 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撤单回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void CancelCommoditiesOrderRpt(CancelOrderEntity model);
        #endregion

        /// <summary>
        /// 股指期货撤单回报
        /// </summary>
        /// <param name="model">委托回报</param>
        [OperationContract]
        void CancelStockIndexFuturesOrderRpt(CancelOrderEntity model);
        /// <summary>
        /// 港股撤单回报
        /// </summary>
        /// <param name="model">港股撤单回报实体</param>
        [OperationContract]
        void CancelHKStockOrderRpt(CancelOrderEntity model);

        

        #endregion

        #region 改单回报接口
        /// <summary>
        /// 港股改单回报
        /// </summary>
        /// <param name="model">港股改单回报实体</param>
        [OperationContract]
        void ModifyHKStockOrderRpt(HKModifyBackEntity model);
        #endregion

    }
}

