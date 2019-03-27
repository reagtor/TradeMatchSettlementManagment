using System.Collections.Generic;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;

namespace ReckoningCounter.BLL.Reckoning.Logic.XH
{
    /// <summary>
    ///现货成交回报汇总对象
    /// Create By:宋涛
    /// Create date:2009-01-26
    /// </summary>
    public class XHDealSum
    {
        /// <summary>
        /// 总成交数量
        /// </summary>
        public decimal AmountSum;

        /// <summary>
        /// 总成交金额
        /// </summary>
        public decimal CapitalSum;

        /// <summary>
        /// 总成交费用
        /// </summary>
        public decimal CostSum;
    }
}