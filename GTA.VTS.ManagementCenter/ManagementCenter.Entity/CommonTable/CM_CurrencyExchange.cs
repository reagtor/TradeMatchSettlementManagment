using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述:币种之间兑换类型表 实体类CM_CurrencyExchange 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_CurrencyExchange
	{
		public CM_CurrencyExchange()
		{}
		#region Model
		private int _currencyexchangeid;
		private string _describe;
		/// <summary>
		/// 币种之间兑换类型标识
		/// </summary>
        [DataMember]
		public int CurrencyExchangeID
		{
			set{ _currencyexchangeid=value;}
			get{return _currencyexchangeid;}
		}
		/// <summary>
		/// 兑换方式描述
		/// </summary>
        [DataMember]
		public string Describe
		{
			set{ _describe=value;}
			get{return _describe;}
		}
		#endregion Model

	}
}

