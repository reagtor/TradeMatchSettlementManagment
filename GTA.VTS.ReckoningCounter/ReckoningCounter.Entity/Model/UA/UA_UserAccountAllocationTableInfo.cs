using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title: 账户分配实体
    /// Desc: 账户分配实体UA_UserAccountAllocationTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
	/// </summary>
	[Serializable]
	public class UA_UserAccountAllocationTableInfo
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UA_UserAccountAllocationTableInfo()
		{}
		#region Model
		private string _useraccountdistributelogo;
		private bool _whetheravailable;
		private string _userid;
		private int _accounttypelogo;
		/// <summary>
		/// 用户账户ID(主键)
		/// </summary>
		public string UserAccountDistributeLogo
		{
			set{ _useraccountdistributelogo=value;}
			get{return _useraccountdistributelogo;}
		}
		/// <summary>
		/// 账户是否可用
		/// </summary>
		public bool WhetherAvailable
		{
			set{ _whetheravailable=value;}
			get{return _whetheravailable;}
		}
		/// <summary>
		/// 用户ID(外键UA_UserBasicInformationTable)
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 账户类型标识ID(外键BD_AccountType)
		/// </summary>
		public int AccountTypeLogo
		{
			set{ _accounttypelogo=value;}
			get{return _accounttypelogo;}
		}
		#endregion Model

	}
}

