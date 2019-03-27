using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易商品品种类型表 实体类CM_BreedClassType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_BreedClassType
	{
		public CM_BreedClassType()
		{}
		#region Model
		private int _breedclasstypeid;
		private string _breedclasstypename;
		/// <summary>
		/// 品种类型标识
		/// </summary>
        [DataMember]
		public int BreedClassTypeID
		{
			set{ _breedclasstypeid=value;}
			get{return _breedclasstypeid;}
		}
		/// <summary>
		/// 品种类型名称
		/// </summary>
        [DataMember]
		public string BreedClassTypeName
		{
			set{ _breedclasstypename=value;}
			get{return _breedclasstypename;}
		}
		#endregion Model

	}
}

