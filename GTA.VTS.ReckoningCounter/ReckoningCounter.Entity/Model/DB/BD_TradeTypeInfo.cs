using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:成交类型
    /// Desc.:成交类型实体类(如买卖成交、撤单成交等）BD_TradeType 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_TradeTypeInfo
	{
        /// <summary>
        /// 成交类型构造函数 
        /// </summary>
        public BD_TradeTypeInfo()
		{}
		#region Model
		private int _tradetypeid;
		private string _tradetypename;
		/// <summary>
		/// 成交类型ID
		/// </summary>
		public int TradeTypeId
		{
			set{ _tradetypeid=value;}
			get{return _tradetypeid;}
		}
		/// <summary>
		/// 成交类型名称
		/// </summary>
		public string TradeTypeName
		{
			set{ _tradetypename=value;}
			get{return _tradetypename;}
		}
		#endregion Model

	}
}

