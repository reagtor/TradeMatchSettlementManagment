#region Using Namespace

using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 描述：现货检验方法
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public class StockTradingRuleValidater : TradingRuleValidater<int, StockRuleContainer>
    {
        /// <summary>
        /// 获取交易品种并检验
        /// </summary>
        /// <param name="request">股票委托请求</param>
        /// <param name="strMessage">现货检验错误信息</param>
        /// <param name="iBreed">品种标识</param>
        public bool Validate(StockOrderRequest request, ref string strMessage, int iBreed)
        {
            bool bresult = false;

            StockRuleContainer src = GetContainer(iBreed);

            if (src == null)
            {
                src = new StockRuleContainer(iBreed);
                SetContainer(iBreed, src);
            }
            bresult = src.ValidateAllRules(request, ref strMessage); //调用现货检验总方法

            return bresult;
        }
    }
}