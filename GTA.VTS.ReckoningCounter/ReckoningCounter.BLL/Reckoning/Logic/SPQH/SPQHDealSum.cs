using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.BLL.Reckoning.Logic.SPQH
{
    /// <summary>
    /// 股指期货成交回报汇总对象
    /// Create By:李健华
    /// Create date:2010-01-26
    /// </summary>
    public class SPQHDealSum
    {
        /// <summary>
        /// 总成交数量
        /// </summary>
        public decimal AmountSum;

        /// <summary>
        /// 真正的总成交金额（保证金）=委托量*委托价*期货合约乘数300-unitMultiple*保证金比例futureBail
        /// </summary>
        public decimal CapitalSum;

        /// <summary>
        /// 总成交金额（量价的简单汇总）=委托量*委托价
        /// </summary>
        public decimal CapitalSumNoScale;

        /// <summary>
        /// 总成交费用
        /// </summary>
        public decimal CostSum;
    }
}
