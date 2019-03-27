using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:资金转账类型实体类
    /// Desc.:资金转账类型实体类BD_TransferTypeTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_TransferTypeTableInfo
	{
        /// <summary>
        /// 资金转账类型实体类构造函数 
        /// </summary>
        public BD_TransferTypeTableInfo()
		{}
		#region Model
		private int _transfertypelogo;
		private string _transfertypename;
		private string _remarks;
		/// <summary>
		/// 转账类型ID
		/// </summary>
		public int TransferTypeLogo
		{
			set{ _transfertypelogo=value;}
			get{return _transfertypelogo;}
		}
		/// <summary>
		/// 转账类型名称
		/// </summary>
		public string TransferTypeName
		{
			set{ _transfertypename=value;}
			get{return _transfertypename;}
		}
		/// <summary>
		/// 转账类型名称备注
		/// </summary>
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		#endregion Model

	}
}

