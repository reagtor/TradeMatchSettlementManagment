using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:冻结类型实体类
    /// Desc.:冻结类型实体类BD_FreezeType 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_FreezeTypeInfo
	{
        /// <summary>
        /// 冻结类型实体类构造函数 
        /// </summary>
        public BD_FreezeTypeInfo()
		{}
		#region Model
		private int _freezetypelogo;
		private string _freezedescribe;
		/// <summary>
		/// 冻结类型主键ID
		/// </summary>
		public int FreezeTypeLogo
		{
			set{ _freezetypelogo=value;}
			get{return _freezetypelogo;}
		}
		/// <summary>
		/// 冻结类型说明（名称）
		/// </summary>
		public string FreezeDescribe
		{
			set{ _freezedescribe=value;}
			get{return _freezedescribe;}
		}
		#endregion Model

	}
}

