using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// 港股资金账户实体类HK_CapitalAccountInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    //[Serializable]
    public class HK_CapitalAccountInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_CapitalAccountInfo()
        { }
        #region Model
        private int _capitalAccountLogo;
        private string _userAccountDistributeLogo;
        private int _tradeCurrencyType;
        private decimal _availablecapital;
        private decimal _balanceoftheday;
        private decimal _todayoutincapital;
        private decimal _freezecapitaltotal;
        private decimal _capitalbalance;
        private decimal _hasdoneprofitlosstotal;
        /// <summary>
        /// 港股资金账户表ID(主键)
        /// </summary>
        public int CapitalAccountLogo
        {
            set { _capitalAccountLogo = value; }
            get { return _capitalAccountLogo; }
        }
        /// <summary>
        /// 用户港股资金账号ID(外键UA_UserAccountAllocationTable)
        /// </summary>
        public string UserAccountDistributeLogo
        {
            set { _userAccountDistributeLogo = value; }
            get { return _userAccountDistributeLogo; }
        }
        /// <summary>
        /// 当前资金账号的所成交(交易)货币类型(外键BD_CurrencyType)
        /// </summary>
        public int TradeCurrencyType
        {
            set { _tradeCurrencyType = value; }
            get { return _tradeCurrencyType; }
        }
        /// <summary>
        /// 可用资金
        /// </summary>
        public decimal AvailableCapital
        {
            set { _availablecapital = value; }
            get { return _availablecapital; }
        }
        /// <summary>
        /// 上日结存金额
        /// </summary>
        public decimal BalanceOfTheDay
        {
            set { _balanceoftheday = value; }
            get { return _balanceoftheday; }
        }
        /// <summary>
        /// 今天收入/支出资金总额
        /// </summary>
        public decimal TodayOutInCapital
        {
            set { _todayoutincapital = value; }
            get { return _todayoutincapital; }
        }
        /// <summary>
        /// 今天冻结资金金额
        /// </summary>
        public decimal FreezeCapitalTotal
        {
            set { _freezecapitaltotal = value; }
            get { return _freezecapitaltotal; }
        }
        /// <summary>
        /// 资金结存金额([FreezeCapitalTotal]+[AvailableCapital])，当前拥有总金额
        /// </summary>
        public decimal CapitalBalance
        {
            set { _capitalbalance = value; }
            get { return _capitalbalance; }
        }
        /// <summary>
        /// 累计已实现盈亏
        /// </summary>
        public decimal HasDoneProfitLossTotal
        {
            set { _hasdoneprofitlosstotal = value; }
            get { return _hasdoneprofitlosstotal; }
        }
        #endregion Model

    }
}
