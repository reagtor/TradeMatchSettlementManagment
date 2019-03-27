using System;
using System.Runtime.Serialization;


namespace ManagementCenter.Model.QH
{
    /// <summary>
    /// Desc: 今日结算价表
    /// Create By: 董鹏
    /// Create Date: 2010-03-12
    /// </summary>
    public class QH_TodaySettlementPriceInfoEx
    { /// <summary>
        /// 合约代码
        /// </summary>
        [DataMember]
        public string CommodityCode
        {
            get;
            set;
        }

        /// <summary>
        /// 交易日期
        /// </summary>
        [DataMember]
        public int TradingDate
        {
            get;
            set;
        }

        /// <summary>
        /// 今日结算价
        /// </summary>
        [DataMember]
        public decimal SettlementPrice
        {
            get;
            set;
        }
    }
}
