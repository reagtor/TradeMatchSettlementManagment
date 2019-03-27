using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：管理员_所属组表 实体类UM_ManagerBeloneToGroup 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
	/// </summary>
    [DataContract]
	public class UM_ManagerBeloneToGroup
	{
		public UM_ManagerBeloneToGroup()
		{}
		#region Model
		private int _userid;
		private int? _managergroupid;
		/// <summary>
		/// 用户的ID号
		/// </summary>
        [DataMember]
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 管理员组ID号
		/// </summary>
        [DataMember]
		public int? ManagerGroupID
		{
			set{ _managergroupid=value;}
			get{return _managergroupid;}
		}
		#endregion Model

	}
}

