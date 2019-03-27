using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：帐号表 实体类UM_DealerAccount 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_DealerAccount
	{
		public UM_DealerAccount()
		{}
		#region Model
		private bool _isdo;
		private int _userid;
		private string _dealeraccoutid;
		private int? _accounttypeid;

	    private int? _accountattributiontype;
		/// <summary>
		/// 是否可用
		/// </summary>
        [DataMember]
		public bool IsDo
		{
			set{ _isdo=value;}
			get{return _isdo;}
		}
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
		/// 交易员账号分配表标识
		/// </summary>
        [DataMember]
		public string DealerAccoutID
		{
			set{ _dealeraccoutid=value;}
			get{return _dealeraccoutid;}
		}
		/// <summary>
		/// 账号类型标识
		/// </summary>
        [DataMember]
		public int? AccountTypeID
		{
			set{ _accounttypeid=value;}
			get{return _accounttypeid;}
		}

        /// <summary>
        /// 帐户所属类型，现货资金和持仓，期货资金和持仓，银行帐号
        /// </summary>
        [DataMember]
        public int? AccountAttributionType
        {
            set { _accountattributiontype = value; }
            get { return _accountattributiontype; }
        }
		#endregion Model

	}
}

