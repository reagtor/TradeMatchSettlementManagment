#region Using Namespace

using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Model.HK;

#endregion

namespace ReckoningCounter.BLL.Delegatevalidate.ManagementCenter
{
    /// <summary>
    /// 描述：委托规则检验接口
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    internal interface IMcValidater
    {
        /// <summary>
        /// 股票委托规则检验
        /// </summary>
        /// <param name="request">股票委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        bool ValidateStockTradeRule(StockOrderRequest request, ref string errMsg);

        /// <summary>
        /// 期货委托规则检验
        /// </summary>
        /// <param name="request">期货委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        bool ValidateFutureTradeRule(MercantileFuturesOrderRequest request, ref string errMsg);

        /// <summary>
        /// 股指期货委托规则检验
        /// </summary>
        /// <param name="request">股指期货委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        bool ValidateStockIndexFutureTradeRule(StockIndexFuturesOrderRequest request, ref string errMsg);

        /// <summary>
        /// 港股委托规则检验
        /// </summary>
        /// <param name="request">港股委托对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        bool ValidateHKStockTradeRule(HKOrderRequest request, ref string errMsg);

        /// <summary>
        /// 港股改单委托规则检验
        /// </summary>
        /// <param name="request">改单请求实体</param>
        /// <param name="entrusInfo">原委托实体对象</param>
        /// <param name="errMsg">返回错误检验信息</param>
        /// <returns>是否成功</returns>
        bool ValidateHKModifyOrderRule(HKModifyOrderRequest request, HK_TodayEntrustInfo entrusInfo, ref string errMsg);
    }
}