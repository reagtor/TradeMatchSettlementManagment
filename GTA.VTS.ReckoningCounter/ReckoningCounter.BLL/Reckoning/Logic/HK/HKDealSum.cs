using System.Collections.Generic;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;

namespace ReckoningCounter.BLL.Reckoning.Logic.HK
{
    /// <summary>
    ///港股成交回报汇总对象
    /// Create By:李健华
    /// Create date:2009-10-26
    /// </summary>
    public class HKDealSum
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