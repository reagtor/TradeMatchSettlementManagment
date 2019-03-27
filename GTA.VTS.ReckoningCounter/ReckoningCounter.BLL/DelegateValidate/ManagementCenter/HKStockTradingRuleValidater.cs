using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity;

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 描述：港股检验方法
    /// 作者：李健华
    /// 日期：2009-10-20
    /// </summary>
    public class HKStockTradingRuleValidater : TradingRuleValidater<int, HKStockRuleContainer>
    {
        /// <summary>
        /// 获取交易品种并检验
        /// </summary>
        /// <param name="request">股票委托请求</param>
        /// <param name="strMessage">现货检验错误信息</param>
        /// <param name="iBreed">品种标识</param>
        public bool Validate(HKOrderRequest request, ref string strMessage, int iBreed)
        {
            bool bresult = false;

            HKStockRuleContainer src = GetContainer(iBreed);

            if (src == null)
            {
                src = new HKStockRuleContainer(iBreed);
                SetContainer(iBreed, src);
            }
            bresult = src.ValidateAllRules(request, ref strMessage); //调用港股检验总方法

            return bresult;
        }
    }
}
