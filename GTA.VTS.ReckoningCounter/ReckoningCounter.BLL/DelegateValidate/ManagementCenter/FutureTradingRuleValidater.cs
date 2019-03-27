#region Using Namespace

using ReckoningCounter.BLL.Delegatevalidate.ManagementCenter;
using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 期货检验方法
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public class FutureTradingRuleValidater : TradingRuleValidater<int, FutureRuleContainer>
    {
        /// <summary>
        /// 获取交易品种并检验
        /// </summary>
        /// <param name="request">期货委托请求</param>
        /// <param name="iBreed">品种标识</param>
        /// <param name="strMessage">现货检验错误信息</param>
        public bool Validate(MercantileFuturesOrderRequest request, int iBreed, ref string strMessage)
        {
            bool bresult = false;
            FutureRuleContainer src = GetContainer(iBreed);

            if (src == null)
            {
                src = new FutureRuleContainer(iBreed);
                SetContainer(iBreed, src);
            }

            bresult = src.ValidateAllRules(request, ref strMessage); //调用现货检验总方法
            return bresult;
        }
    }
}