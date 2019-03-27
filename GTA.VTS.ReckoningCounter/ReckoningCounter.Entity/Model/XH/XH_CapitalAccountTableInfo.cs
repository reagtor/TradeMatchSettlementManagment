using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货资金表实体类
    /// Desc: 现货资金表XH_CapitalAccountTable实体类 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class XH_CapitalAccountTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_CapitalAccountTableInfo()
        { }
        #region CapitalAccountLogo 现货资金账户表ID(主键)
        private int capitalAccountLogo;
        /// <summary>
        /// 现货资金账户表ID(主键)
        /// </summary>
        [DataMember]
        public int CapitalAccountLogo
        {
            get
            {
                return capitalAccountLogo;
            }
            set
            {
                capitalAccountLogo = value;
            }
        }
        #endregion

        #region UserAccountDistributeLogo 用户现货资金账号ID(外键UA_UserAccountAllocationTable)
        private string userAccountDistributeLogo;
        /// <summary>
        /// 用户现货资金账号ID(外键UA_UserAccountAllocationTable)
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

        #region FreezeCapitalTotal 今天冻结资金金额
        private decimal freezeCapitalTotal;
        /// <summary>
        /// 今天冻结资金金额
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

        #region HasDoneProfitLossTotal 累计已实现盈亏
        private decimal hasDoneProfitLossTotal;
        /// <summary>
        /// 累计已实现盈亏
        /// </summary>
        [DataMember]
        public decimal HasDoneProfitLossTotal
        {
            get
            {
                return hasDoneProfitLossTotal;
            }
            set
            {
                hasDoneProfitLossTotal = value;
            }
        }
        #endregion

        
    }
}

