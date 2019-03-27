using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:期货开平仓类型实体
    /// Desc.:期货开平仓类型BD_OpenCloseType 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_OpenCloseTypeInfo
	{
        /// <summary>
        /// 期货开平仓类型实体构造函数 
        /// </summary>
        public BD_OpenCloseTypeInfo()
		{}
		#region Model
		private int _octid;
		private string _octname;
		/// <summary>
		/// 期货买卖类型（平开仓）ID
		/// </summary>
		public int OCTId
		{
			set{ _octid=value;}
			get{return _octid;}
		}
		/// <summary>
		/// 期货买卖类型名称
		/// </summary>
		public string OCTName
		{
			set{ _octname=value;}
			get{return _octname;}
		}
		#endregion Model

	}
}

