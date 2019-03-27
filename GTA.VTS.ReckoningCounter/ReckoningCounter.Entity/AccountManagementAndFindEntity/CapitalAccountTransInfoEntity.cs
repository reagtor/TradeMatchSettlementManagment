using GTA.VTS.Common.CommonObject;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Desc: 记录可用于回滚资金内存表的信息
    /// Create by : 董鹏
    /// Create Date:2009-12-23
    /// </summary>
    public struct CapitalAccountTransInfoEntity
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        public Types.AccountType accountType { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public Types.CurrencyType currencyType { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// 可用资金增量
        /// </summary>
        public decimal AvailableCapitalDelta { get; set; }

        /// <summary>
        /// 冻结资金金额增量
        /// </summary>
        public decimal FreezeCapitalTotalDelta { get; set; }
        /// <summary>
        /// 当日出入金增量
        /// </summary>
        public decimal TodayOutInCapital { get; set; }
        /// <summary>
        /// 累计已实现盈亏增量
        /// </summary>
        public decimal HasDoneProfitLossTotalDelta { get; set; }

        /// <summary>
        /// 今天收入/支出资金总额增量
        /// </summary>
        public decimal TodayOutInCapitalDelta { get; set; }
        /// <summary>
        /// 保证金总额增量
        /// </summary>
        public decimal MarginTotalDelta { get; set; }
        /// <summary>
        /// 总平仓盈亏（浮)增量
        /// </summary>
        public decimal CloseFloatProfitLossTotalDelta { get; set; }
        /// <summary>
        /// 总平仓盈亏（盯）增量
        /// </summary>
        public decimal CloseMarketProfitLossTotalDelta { get; set; }
    }

}
