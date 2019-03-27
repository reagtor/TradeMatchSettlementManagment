using MatchCenter.Entity;
using System.ServiceModel;
using MatchCenter.Entity.HK;

namespace MatchCenter.BLL.interfaces
{
    /// <summary>
    /// 撮合中心下单撤单接口
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Desc.：添加港股相关的方法公开接口定义
    /// Update By:李健华
    /// Update Date:2009-10-19
    /// </summary>
    [ServiceContract]
    interface IDoOrder
    {
        #region 下单

        /// <summary>
        /// 现货下单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        ResultDataEntity DoStockOrder(StockOrderEntity dataOrder);
        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        ResultDataEntity DoFutureOrder(FutureOrderEntity dataOrder);
        /// <summary>
        /// 商品期货下单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        ResultDataEntity DoCommoditiesOrder(CommoditiesOrderEntity dataOrder);

        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        ResultDataEntity DoHKEntrustOrder(HKOrderEntity dataOrder);

        #endregion

        #region 撤单
        /// <summary>
        /// 现货撤单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        CancelResultEntity CancelStockOrder(CancelEntity dataOrder);

        /// <summary>
        /// 股指期货撤单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        CancelResultEntity CancelFutureOrder(CancelEntity dataOrder);

        /// <summary>
        /// 商品期货撤单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        CancelResultEntity CancelCommoditiesOrder(CancelEntity dataOrder);

        /// <summary>
        /// 港股撤单
        /// </summary>
        /// <param name="dataOrder">委托实体</param>
        /// <returns></returns>
        [OperationContract]
        CancelResultEntity CancelHKOrder(CancelEntity dataOrder);

        #endregion

        /// <summary>
        /// 港股改单
        /// </summary>
        /// <param name="dataOrder">港股改单实体</param>
        /// <returns></returns>
        [OperationContract]
        HKModifyResultEntity ModifyHKStockOrder(HKModifyEntity dataOrder);

        /// <summary>
        /// 检查通道
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string CheckChannel();



    }
}
