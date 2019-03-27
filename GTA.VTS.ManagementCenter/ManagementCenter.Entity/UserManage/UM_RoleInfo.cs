using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：角色信息表 实体类UM_RoleInfo 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
	/// </summary>
    [DataContract]
	public class UM_RoleInfo
	{
		public UM_RoleInfo()
		{}
		#region Model
		private int _roleid;
		private string _rolename;
		/// <summary>
		/// 角色信息表ID号
		/// </summary>
        [DataMember]
		public int RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 角色名称
		/// </summary>
        [DataMember]
		public string RoleName
		{
			set{ _rolename=value;}
			get{return _rolename;}
		}
		#endregion Model

	}
}

