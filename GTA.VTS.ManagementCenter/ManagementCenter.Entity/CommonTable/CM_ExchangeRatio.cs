using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：汇率表 实体类CM_ExchangeRatio 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_ExchangeRatio
	{
		public CM_ExchangeRatio()
		{}
		#region Model
		private decimal? _ratio;
		private string _remarks;
		private int _currencyexchangeid;
		/// <summary>
		/// 汇率
		/// </summary>
		[DataMember]
		public decimal? Ratio
		{
			set{ _ratio=value;}
			get{return _ratio;}
		}
		/// <summary>
		/// 备注
		/// </summary>
        [DataMember]
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 币种之间兑换类型标识
		/// </summary>
        [DataMember]
		public int CurrencyExchangeID
		{
			set{ _currencyexchangeid=value;}
			get{return _currencyexchangeid;}
		}
		#endregion Model

	}
}

