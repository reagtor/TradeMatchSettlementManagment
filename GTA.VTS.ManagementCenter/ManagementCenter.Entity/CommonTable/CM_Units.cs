using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：单位表 实体类CM_Units 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
	/// </summary>
    [DataContract]
	public class CM_Units
	{
		public CM_Units()
		{}
		#region Model
		private int _unitsid;
		private string _unitsname;
		/// <summary>
		/// 单位标识
		/// </summary>
        [DataMember]
		public int UnitsID
		{
			set{ _unitsid=value;}
			get{return _unitsid;}
		}
		/// <summary>
		/// 单位名称
		/// </summary>
        [DataMember]
		public string UnitsName
		{
			set{ _unitsname=value;}
			get{return _unitsname;}
		}
		#endregion Model

	}
}

