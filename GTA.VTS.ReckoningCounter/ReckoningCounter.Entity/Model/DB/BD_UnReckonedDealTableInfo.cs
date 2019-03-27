using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:未清算的成交记录
    /// Desc.:未清算的成交记录实体类BD_UnReckonedDealTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_UnReckonedDealTableInfo
	{
        /// <summary>
        /// 未清算的成交记录构造函数 
        /// </summary>
        public BD_UnReckonedDealTableInfo()
		{}
		#region Model
		private string _id;
		private int _entitytype;
		private string _orderno;
		private decimal? _price;
		private int? _amount;
		private DateTime? _time;
		private string _message;
		private bool _issuccess;
		private string _counterid;
		/// <summary>
		/// 记录对象标识(这来自撮合中心回传的实体ID标识)
		/// </summary>
		public string id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 撮合回报类型(详见代码定义)
		/// </summary>
		public int EntityType
		{
			set{ _entitytype=value;}
			get{return _entitytype;}
		}
		/// <summary>
		/// 委托单号
		/// </summary>
		public string OrderNo
		{
			set{ _orderno=value;}
			get{return _orderno;}
		}
		/// <summary>
		/// 成交价格
		/// </summary>
		public decimal? Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 成交数量
		/// </summary>
		public int? Amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 成交时间
		/// </summary>
		public DateTime? Time
		{
			set{ _time=value;}
			get{return _time;}
		}
		/// <summary>
		/// 返回的信息
		/// </summary>
		public string Message
		{
			set{ _message=value;}
			get{return _message;}
		}
		/// <summary>
		/// 撮合是否成功(目前好象没有什么意义)
		/// </summary>
		public bool IsSuccess
		{
			set{ _issuccess=value;}
			get{return _issuccess;}
		}
		/// <summary>
		/// 撮合中心回送通道号或者柜台号
		/// </summary>
		public string CounterID
		{
			set{ _counterid=value;}
			get{return _counterid;}
		}
		#endregion Model

	}
}

