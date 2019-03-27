using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：管理员组 实体类UM_ManagerGroup 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_ManagerGroup
	{
		public UM_ManagerGroup()
		{}
		#region Model
		private int _managergroupid;
		private string _managergroupname;
		/// <summary>
		/// 管理员组ID号
		/// </summary>
        [DataMember]
		public int ManagerGroupID
		{
			set{ _managergroupid=value;}
			get{return _managergroupid;}
		}
		/// <summary>
		/// 管理员组名称
		/// </summary>
        [DataMember]
		public string ManagerGroupName
		{
			set{ _managergroupname=value;}
			get{return _managergroupname;}
		}
		#endregion Model

	}
}

