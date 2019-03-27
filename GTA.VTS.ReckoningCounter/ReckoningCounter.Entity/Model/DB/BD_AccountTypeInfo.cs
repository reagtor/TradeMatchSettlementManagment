using System;
namespace ReckoningCounter.Model
{
	/// <summary>
    /// Title:帐户类型实体类
    /// Desc.:帐户类型实体类BD_AccountType 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
	/// </summary>
	[Serializable]
	public class BD_AccountTypeInfo
	{
        /// <summary>
        ///帐户类型实体类构造函数 
        /// </summary>
		public BD_AccountTypeInfo()
		{}
		#region Model
		private int _accounttypelogo;
		private string _accounttypename;
		private string _remarks;
		private int? _atcid;
		private int? _relationaccountid;
		/// <summary>
		/// 账户类型主键ID
		/// </summary>
		public int AccountTypeLogo
		{
			set{ _accounttypelogo=value;}
			get{return _accounttypelogo;}
		}
		/// <summary>
		/// 账户类型名称
		/// </summary>
		public string AccountTypeName
		{
			set{ _accounttypename=value;}
			get{return _accounttypename;}
		}
		/// <summary>
		/// 账户类型备注
		/// </summary>
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 账户类型所属分类ID（外键DB_AccountTypeClass）
		/// </summary>
		public int? ATCId
		{
			set{ _atcid=value;}
			get{return _atcid;}
		}
		/// <summary>
		/// 账户关联的账号ID（如证券资金账户会关联证卷股东代码）
		/// </summary>
		public int? RelationAccountId
		{
			set{ _relationaccountid=value;}
			get{return _relationaccountid;}
		}
		#endregion Model

	}
}

