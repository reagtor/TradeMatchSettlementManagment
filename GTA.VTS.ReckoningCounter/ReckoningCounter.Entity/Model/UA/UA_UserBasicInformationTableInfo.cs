using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title: 用户基础信息实体
    /// Desc: 用户基础信息实体类UA_UserBasicInformationTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
	/// </summary>
	[Serializable]
	public class UA_UserBasicInformationTableInfo
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UA_UserBasicInformationTableInfo()
		{}
		#region Model
		private string _userid;
		private string _password;
		private int _rolenumber;
		/// <summary>
		/// 用户ID(主键)
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 用户密码
		/// </summary>
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 用户角色(外键BD_UserRoleType)
		/// </summary>
		public int RoleNumber
		{
			set{ _rolenumber=value;}
			get{return _rolenumber;}
		}
		#endregion Model

	}
}

