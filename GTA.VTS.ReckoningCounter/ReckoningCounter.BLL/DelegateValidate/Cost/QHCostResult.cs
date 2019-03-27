using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.BLL.DelegateValidate.Cost
{
    /// <summary>
    /// 期货交易费用结果
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public class QHCostResult
    {
        /// <summary>
        /// 商品代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 交易费用
        /// </summary>
        public decimal Cosing { get; set; }

        public override string ToString()
        {
            string format = "期货交易费用[商品代码={0},交易费用={1}]";
            string desc = string.Format(format, Code, Cosing);
            return desc;
        }
    }
}
