using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:，委托单状态描述表,实体类DB_OrderStatus 。(属性说明自动提取数据库字段的描述信息)
    /// Desc.: 委托单状态描述表与数据库对应
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class DB_OrderStatusInfo
	{
        /// <summary>
        /// 委托单状态描述表,实体类构造函数 
        /// </summary>
        public DB_OrderStatusInfo()
		{}
		#region Model
		private int _orderstatusid;
		private string _orderstatusname;
		/// <summary>
		/// 单据状态ID
		/// </summary>
		public int OrderStatusId
		{
			set{ _orderstatusid=value;}
			get{return _orderstatusid;}
		}
		/// <summary>
		/// 单据状态名称
		/// </summary>
		public string OrderStatusName
		{
			set{ _orderstatusname=value;}
			get{return _orderstatusname;}
		}
		#endregion Model

	}
}

