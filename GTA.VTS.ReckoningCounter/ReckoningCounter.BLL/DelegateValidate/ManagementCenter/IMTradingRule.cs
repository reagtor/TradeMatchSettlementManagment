#region Using Namespace

using ReckoningCounter.BLL.Delegatevalidate.ManagementCenter;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 描述：根据代码获取某一品种的数据接口
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// 描述：增加根据港股代码获取某一品种的数据接口
    /// 作者：李健华
    /// 日期：2009-10-20
    /// </summary>
    public interface IMTradingRule
    {
        /// <summary>
        /// 根据商品代码获取某一现货或期货商品的计算公式
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns></returns>
        StockRuleContainer GetStockRuleContainerByCode(string code);

        /// <summary>
        /// 根据商品代码，商品类型获取某一期货商品的费用计算公式
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns></returns>
        FutureRuleContainer GetFutureRuleContainerByCode(string code);
        /// <summary>
        /// 根据商品代码获取某一港股商品的计算公式
        /// </summary>
        /// <param name="code">港股商品代码</param>
        /// <returns></returns>
        HKStockRuleContainer GetHKStockRuleContainerByCode(string code);
    }
}