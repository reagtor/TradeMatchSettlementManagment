using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.BLL.DelegateValidate.Cost
{
    /// <summary>
    /// 港股交易费用结果
    /// Create By：李健华
    /// Create Date：2009-10-20
    /// </summary>
    public class HKCostResult
    {
        /// <summary>
        /// 商品代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 1.印花税
        /// </summary>
        public decimal StampDuty { get; set; }

        /// <summary>
        /// 2.佣金
        /// </summary>
        public decimal Commision { get; set; }

        /// <summary>
        /// 3.过户费
        /// </summary>
        public decimal TransferToll { get; set; }

        /// <summary>
        /// 4.交易手续费
        /// </summary>
        public decimal PoundageSingleValue { get; set; }

        /// <summary>
        /// 5.监管费
        /// </summary>
        public decimal MonitoringFee { get; set; }

        /// <summary>
        /// 6.结算费
        /// </summary>
        public decimal ClearingFees { get; set; }

        /// <summary>
        /// 7.交易系统使用费（港股）
        /// </summary>
        public decimal TradeSystemFees { get; set; }


        /// <summary>
        /// 总费用
        /// </summary>
        public decimal CoseSum
        {
            get
            {
                return StampDuty + Commision + TransferToll + PoundageSingleValue + MonitoringFee + ClearingFees +
                       TradeSystemFees;
            }
        }

        public override string ToString()
        {
            string format = "港股交易费用[商品代码={0},印花税={1},佣金={2},过户费={3},交易手续费={4},监管费={5},结算费={6},交易系统使用费={7},合计={8}]";
            string desc = string.Format(format, Code, StampDuty, Commision, TransferToll, PoundageSingleValue, MonitoringFee,
                                        ClearingFees, TradeSystemFees, CoseSum);
            return desc;
        }
    }
}
