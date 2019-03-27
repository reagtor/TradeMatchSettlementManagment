#region Using Namespace

using System.ServiceModel;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants;

#endregion

namespace ReckoningCounter.BLL
{
    /// <summary>
    /// 下单服务接口
    /// </summary>
    /// 
    [ServiceContract]
    public interface IDoOrder
    {
        #region 现货

        /// <summary>
        /// 下单个现货委托单
        /// </summary>
        /// <param name="stockorder">现货委托单数据</param>
        /// <returns>返回带委托单号的单个委托单</returns>
        /// 
        [OperationContract]
        OrderResponse DoStockOrder(StockOrderRequest stockorder);

        /// <summary>
        /// 撤消现货委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <param name="errorType"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelStockOrder2(string OrderId, ref string message, out Types.OrderStateType ost, out int errorType);

        /// <summary>
        /// 撤消现货委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelStockOrder(string OrderId, ref string message, out Types.OrderStateType ost);

        #endregion

        #region 股指期货

        /// <summary>
        /// 下单个股指期货委托单
        /// </summary>
        /// <param name="futuresorder">期货委托单数据</param>
        /// <param name="futuresorder">状态信息</param>
        /// <returns></returns>
        /// 
        [OperationContract]
        OrderResponse DoStockIndexFuturesOrder(StockIndexFuturesOrderRequest futuresorder);

        /// <summary>
        /// 撤消股指期货委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <param name="errorType"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelStockIndexFuturesOrder2(string OrderId, ref string message, out Types.OrderStateType ost,
                                           out int errorType);

        /// <summary>
        /// 撤消股指期货委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelStockIndexFuturesOrder(string OrderId, ref string message, out Types.OrderStateType ost);

        #endregion

        #region 商品期货

        /// <summary>
        /// 下单个商品期货委托单
        /// </summary>
        /// <param name="futuresorder">期货委托单数据</param>
        /// <returns></returns>
        [OperationContract]
        OrderResponse DoMercantileFuturesOrder(MercantileFuturesOrderRequest futuresorder);

        /// <summary>
        /// 撤消商品期货委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <param name="errorType"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelMercantileFuturesOrder2(string OrderId, ref string message, out Types.OrderStateType ost,
                                           out int errorType);

        /// <summary>
        /// 撤消商品期货委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelMercantileFuturesOrder(string OrderId, ref string message, out Types.OrderStateType ost);

        #endregion

        #region 港股

        /// <summary>
        /// 下单个港股委托单
        /// </summary>
        /// <param name="hkorder">港股委托单数据</param>
        /// <returns>返回带委托单号的单个委托单</returns>
        /// 
        [OperationContract]
        OrderResponse DoHKOrder(HKOrderRequest hkorder);

        /// <summary>
        /// 下单个港股改单委托单
        /// </summary>
        /// <param name="hkorder">港股改单委托单数据</param>
        /// <returns>返回带委托单号的单个委托单</returns>
        /// 
        [OperationContract]
        OrderResponse DoHKModifyOrder(HKModifyOrderRequest hkorder);

        /// <summary>
        /// 撤消港股委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <param name="errorType"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelHKOrder2(string OrderId, ref string message, out Types.OrderStateType ost, out int errorType);

        /// <summary>
        /// 撤消港股委托单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="message"></param>
        /// <param name="ost"></param>
        /// <returns>返回带委托单号的下单集合</returns>
        /// 
        [OperationContract]
        bool CancelHKOrder(string OrderId, ref string message, out Types.OrderStateType ost);

        #endregion
    }
}