using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:交易方向类型实体
    /// Desc.:交易方向类型实体（卖、买）BD_BuySellType 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_BuySellTypeInfo
	{
        /// <summary>
        /// 易方向类型实体类构造函数 
        /// </summary>
        public BD_BuySellTypeInfo()
		{}
		#region Model
		private int _buysellid;
		private string _buysellname;
		/// <summary>
		/// 买卖类型ID
		/// </summary>
		public int BuysellId
		{
			set{ _buysellid=value;}
			get{return _buysellid;}
		}
		/// <summary>
		/// 买卖类型名称
		/// </summary>
		public string BuysellName
		{
			set{ _buysellname=value;}
			get{return _buysellname;}
		}
		#endregion Model

	}
}

