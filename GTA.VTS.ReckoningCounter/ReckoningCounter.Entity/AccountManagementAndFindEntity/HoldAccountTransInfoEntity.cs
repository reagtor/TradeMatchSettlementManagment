using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;
using GTA.VTS.Common.CommonObject;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Desc: 记录可用于内存持仓回滚的信息
    /// Create by : 董鹏
    /// Create Date:2010-02-01
    /// </summary>
    public struct HoldAccountTransInfoEntity
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
        /// 可用量
        /// </summary>
        public decimal AvailableAmountDelta { get; set; }

        /// <summary>
        /// 冻结量
        /// </summary>
        public decimal FreezeAmountDelta { get; set; }

        /// <summary>
        /// 历史冻结量
        /// </summary>
        public decimal HistoryFreezeAmountDelta { get; set; }

        /// <summary>
        /// 历史持仓量
        /// </summary>
        public decimal HistoryHoldAmountDelta { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal MarginDelta { get; set; }

        /// <summary>
        /// 当日冻结量
        /// </summary>
        public decimal TodayFreezeAmountDelta { get; set; }

        /// <summary>
        /// 当日持仓量
        /// </summary>
        public decimal TodayHoldAmountDelta { get; set; }

        /// <summary>
        /// 现货持仓信息实体
        /// </summary>
        public XH_AccountHoldTableInfo xhData { get; set; }

        /// <summary>
        /// 期货持仓信息实体
        /// </summary>
        public QH_HoldAccountTableInfo qhData { get; set; }

        /// <summary>
        /// 港股持仓信息实体
        /// </summary>
        public HK_AccountHoldInfo hkData { get; set; }
    }
}
