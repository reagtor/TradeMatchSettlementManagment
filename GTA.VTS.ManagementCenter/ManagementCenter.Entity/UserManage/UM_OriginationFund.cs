using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：起始资金表 实体类UM_OriginationFund 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_OriginationFund
	{
		public UM_OriginationFund()
		{}
		#region Model
        private decimal   _fundmoney;
		private string _remark;
		private int _originationfundid;
		private int? _transactioncurrencytypeid;
		private string _dealeraccoutid;
		/// <summary>
		/// 资金金额
		/// </summary>
        [DataMember]
        public decimal FundMoney
		{
			set{ _fundmoney=value;}
			get{return _fundmoney;}
		}
		/// <summary>
		/// 备注
		/// </summary>
        [DataMember]
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 起始资金标识
		/// </summary>
        [DataMember]
		public int OriginationFundID
		{
			set{ _originationfundid=value;}
			get{return _originationfundid;}
		}
		/// <summary>
		/// 交易货币类型标识
		/// </summary>
        [DataMember]
		public int? TransactionCurrencyTypeID
		{
			set{ _transactioncurrencytypeid=value;}
			get{return _transactioncurrencytypeid;}
		}
		/// <summary>
		/// 交易员账号分配表标识
		/// </summary>
        [DataMember]
		public string DealerAccoutID
		{
			set{ _dealeraccoutid=value;}
			get{return _dealeraccoutid;}
		}
		#endregion Model

	}
}

