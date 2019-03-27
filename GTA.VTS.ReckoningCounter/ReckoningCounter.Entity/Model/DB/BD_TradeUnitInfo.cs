using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:交易单位(如手-Hand，股-Thigh，等）
    /// Desc.:交易单位实体类(如手-Hand，股-Thigh，等）BD_TradeUnit 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_TradeUnitInfo
	{
        /// <summary>
        /// 交易单位(如手-Hand，股-Thigh，等）构造函数 
        /// </summary>
        public BD_TradeUnitInfo()
		{}
		#region Model
		private int _tradeunitid;
		private string _tradeunitname;
		/// <summary>
		/// 成交单位ID
		/// </summary>
		public int TradeUnitId
		{
			set{ _tradeunitid=value;}
			get{return _tradeunitid;}
		}
		/// <summary>
		/// 成交单位名称
		/// </summary>
		public string TradeUnitName
		{
			set{ _tradeunitname=value;}
			get{return _tradeunitname;}
		}
		#endregion Model

	}
}

