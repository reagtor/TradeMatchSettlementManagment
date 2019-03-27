using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:系统状态类型管理表
    /// Desc.:系统状态类型管理表BD_StatusTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_StatusTableInfo
	{
        /// <summary>
        /// 系统状态类型管理表构造函数 
        /// </summary>
        public BD_StatusTableInfo()
		{}
		#region Model
		private string _name;
		private string _value;
		/// <summary>
		/// 状态数据名称（主键）
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 状态数据值
		/// </summary>
		public string value
		{
			set{ _value=value;}
			get{return _value;}
		}
		#endregion Model

	}
}

