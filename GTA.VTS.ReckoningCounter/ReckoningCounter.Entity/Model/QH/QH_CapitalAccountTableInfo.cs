using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 期货资金账户实体类
    /// Desc: 期货资金账户实体类实体类QH_CapitalAccountTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create By: 李健华
    /// Create Date: 2009-10-15
    /// </summary>
    [DataContract]
    public class QH_CapitalAccountTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QH_CapitalAccountTableInfo()
        { }

        #region CapitalAccountLogoId 期货资金账户ID(主键)
        private int capitalAccountLogoId;
        /// <summary>
        /// 期货资金账户ID(主键)
        /// </summary>
        [DataMember]
        public int CapitalAccountLogoId
        {
            get
            {
                return capitalAccountLogoId;
            }
            set
            {
                capitalAccountLogoId = value;
            }
        }
        #endregion

        #region UserAccountDistributeLogo 用户期货资金账号ID(外键UA_UserAccountAllocationTable)
        private string userAccountDistributeLogo;
        /// <summary>
        /// 用户期货资金账号ID(外键UA_UserAccountAllocationTable)
        /// </summary>
        [DataMember]
        public string UserAccountDistributeLogo
        {
            get
            {
                return userAccountDistributeLogo;
            }
            set
            {
                userAccountDistributeLogo = value;
            }
        }
        #endregion

        #region BalanceOfTheDay 上日结存金额
        private decimal balanceOfTheDay;
        /// <summary>
        /// 上日结存金额
        /// </summary>
        [DataMember]
        public decimal BalanceOfTheDay
        {
            get
            {
                return balanceOfTheDay;
            }
            set
            {
                balanceOfTheDay = value;
            }
        }
        #endregion

        #region TodayOutInCapital 今天收入/支出资金总额
        private decimal todayOutInCapital;
        /// <summary>
        /// 今天收入/支出资金总额
        /// </summary>
        [DataMember]
        public decimal TodayOutInCapital
        {
            get
            {
                return todayOutInCapital;
            }
            set
            {
                todayOutInCapital = value;
            }
        }
        #endregion

        #region AvailableCapital 可用资金
        private decimal availableCapital;
        /// <summary>
        /// 可用资金
        /// </summary>
        [DataMember]
        public decimal AvailableCapital
        {
            get
            {
                return availableCapital;
            }
            set
            {
                availableCapital = value;
            }
        }
        #endregion

        #region FreezeCapitalTotal 今天冻结资金总额
        private decimal freezeCapitalTotal;
        /// <summary>
        /// 今天冻结资金总额
        /// </summary>
        [DataMember]
        public decimal FreezeCapitalTotal
        {
            get
            {
                return freezeCapitalTotal;
            }
            set
            {
                freezeCapitalTotal = value;
            }
        }
        #endregion

        #region CapitalBalance 资金结存金额([FreezeCapitalTotal]+[AvailableCapital])，当前拥有总金额
        private decimal capitalBalance;
        /// <summary>
        /// 资金结存金额([FreezeCapitalTotal]+[AvailableCapital])，当前拥有总金额
        /// </summary>
        [DataMember]
        public decimal CapitalBalance
        {
            get
            {
                return capitalBalance;
            }
            set
            {
                capitalBalance = value;
            }
        }
        #endregion

        #region MarginTotal 保证金总额
        private decimal marginTotal;
        /// <summary>
        /// 保证金总额
        /// </summary>
        [DataMember]
        public decimal MarginTotal
        {
            get
            {
                return marginTotal;
            }
            set
            {
                marginTotal = value;
            }
        }
        #endregion

        #region TradeCurrencyType 当前资金账号的所成交(交易)货币类型(外键BD_CurrencyType)
        private int tradeCurrencyType;
        /// <summary>
        /// 当前资金账号的所成交(交易)货币类型(外键BD_CurrencyType)
        /// </summary>
        [DataMember]
        public int TradeCurrencyType
        {
            get
            {
                return tradeCurrencyType;
            }
            set
            {
                tradeCurrencyType = value;
            }
        }
        #endregion

        #region CloseFloatProfitLossTotal 总平仓盈亏（浮）
        private decimal closeFloatProfitLossTotal;
        /// <summary>
        /// 总平仓盈亏（浮）
        /// </summary>
        [DataMember]
        public decimal CloseFloatProfitLossTotal
        {
            get
            {
                return closeFloatProfitLossTotal;
            }
            set
            {
                closeFloatProfitLossTotal = value;
            }
        }
        #endregion

        #region CloseMarketProfitLossTotal 总平仓盈亏（盯）
        private decimal closeMarketProfitLossTotal;
        /// <summary>
        /// 总平仓盈亏（盯）
        /// </summary>
        [DataMember]
        public decimal CloseMarketProfitLossTotal
        {
            get
            {
                return closeMarketProfitLossTotal;
            }
            set
            {
                closeMarketProfitLossTotal = value;
            }
        }
        #endregion

    }
}

