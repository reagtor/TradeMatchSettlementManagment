using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title: 银行账户信息实体
    /// Desc: 银行账户信息实体类UA_BankAccountTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
	/// </summary>
    [DataContract]
	public class UA_BankAccountTableInfo
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UA_BankAccountTableInfo()
		{}
		#region Model
		private decimal _capitalremainamount;
		private int _tradecurrencytypelogo;
		private string _useraccountdistributelogo;
		private decimal _balanceoftheday;
		private decimal _todayoutincapital;
		private decimal _freezecapital;
		private decimal _availablecapital;
		/// <summary>
		/// 剩余资金总额
		/// </summary>
        [DataMember]
		public decimal CapitalRemainAmount
		{
			set{ _capitalremainamount=value;}
			get{return _capitalremainamount;}
		}
		/// <summary>
		/// 交易货币类型(外键BD_CurrencyType)
		/// </summary>
        [DataMember]
		public int TradeCurrencyTypeLogo
		{
			set{ _tradecurrencytypelogo=value;}
			get{return _tradecurrencytypelogo;}
		}
		/// <summary>
		/// 银行账号ID(外键UA_UserAccountAllocationTable)
		/// </summary>
        [DataMember]
		public string UserAccountDistributeLogo
		{
			set{ _useraccountdistributelogo=value;}
			get{return _useraccountdistributelogo;}
		}
		/// <summary>
		/// 上日结存
		/// </summary>
        [DataMember]
		public decimal BalanceOfTheDay
		{
			set{ _balanceoftheday=value;}
			get{return _balanceoftheday;}
		}
		/// <summary>
		/// 当日出入金额
		/// </summary>
        [DataMember]
		public decimal TodayOutInCapital
		{
			set{ _todayoutincapital=value;}
			get{return _todayoutincapital;}
		}
		/// <summary>
		/// 冻结总额
		/// </summary>
        [DataMember]
		public decimal FreezeCapital
		{
			set{ _freezecapital=value;}
			get{return _freezecapital;}
		}
		/// <summary>
		/// 可用资金
		/// </summary>
        [DataMember]
		public decimal AvailableCapital
		{
			set{ _availablecapital=value;}
			get{return _availablecapital;}
		}
		#endregion Model

	}
}

