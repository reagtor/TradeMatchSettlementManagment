using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：权限功能表 实体类UM_Functions 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_Functions
	{
		public UM_Functions()
		{}
		#region Model
		private int _functionid;
		private string _functionname;
		/// <summary>
		/// 用户可用的功能表ID号
		/// </summary>
        [DataMember]
		public int FunctionID
		{
			set{ _functionid=value;}
			get{return _functionid;}
		}
		/// <summary>
		/// 功能名称
		/// </summary>
        [DataMember]
		public string FunctionName
		{
			set{ _functionname=value;}
			get{return _functionname;}
		}
		#endregion Model

	}
}

