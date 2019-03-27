using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:交易货币类型
    /// Desc.:交易货币类型实体(港币，人民币、等）BD_CurrencyType 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_CurrencyTypeInfo
	{
        /// <summary>
        /// 交易货币类型构造函数 
        /// </summary>
        public BD_CurrencyTypeInfo()
		{}
		#region Model
		private int _currencytypeid;
		private string _currencyname;
		/// <summary>
		/// 货币类型ID
		/// </summary>
		public int CurrencyTypeId
		{
			set{ _currencytypeid=value;}
			get{return _currencytypeid;}
		}
		/// <summary>
		/// 货币类型名称
		/// </summary>
		public string CurrencyName
		{
			set{ _currencyname=value;}
			get{return _currencyname;}
		}
		#endregion Model

	}
}

