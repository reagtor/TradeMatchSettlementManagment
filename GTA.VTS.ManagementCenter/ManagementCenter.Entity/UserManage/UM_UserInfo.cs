using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：用户基本信息表 实体类UM_UserInfo 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
	/// </summary>
    [DataContract]
	public class UM_UserInfo
	{
		public UM_UserInfo()
		{}
		#region Model
		private string _username;
		private string _loginname;
		private string _password;
		private int _userid;
		private int? _certificatestyle;
		private string _postalcode;
		private int? _roleid;
		private string _certificateno;
		private string _telephone;
		private string _address;
		private string _email;
		private int? _questionid;
		private string _answer;
		private int? _couterid;
		private string _remark;
	    private int _addtype;
	    private DateTime _addtime;

        //柜台名称(此字段仅查询时用)
	    private string _name;
		/// <summary>
		/// 真实姓名
		/// </summary>
        [DataMember]
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 登陆名称
		/// </summary>
        [DataMember]
		public string LoginName
		{
			set{ _loginname=value;}
			get{return _loginname;}
		}
		/// <summary>
		/// 密码
		/// </summary>
        [DataMember]
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
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
		/// 证件类型
		/// </summary>
        [DataMember]
		public int? CertificateStyle
		{
			set{ _certificatestyle=value;}
			get{return _certificatestyle;}
		}
		/// <summary>
		/// 邮政编码
		/// </summary>
        [DataMember]
		public string Postalcode
		{
			set{ _postalcode=value;}
			get{return _postalcode;}
		}
		/// <summary>
		/// 角色信息表ID号
		/// </summary>
        [DataMember]
		public int? RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 证件号码
		/// </summary>
        [DataMember]
		public string CertificateNo
		{
			set{ _certificateno=value;}
			get{return _certificateno;}
		}
		/// <summary>
		/// 联系电话
		/// </summary>
        [DataMember]
		public string Telephone
		{
			set{ _telephone=value;}
			get{return _telephone;}
		}
		/// <summary>
		/// 通讯地址
		/// </summary>
        [DataMember]
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 电子邮箱
		/// </summary>
        [DataMember]
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 提示问题
		/// </summary>
        [DataMember]
		public int? QuestionID
		{
			set{ _questionid=value;}
			get{return _questionid;}
		}
		/// <summary>
		/// 提示问题答案
		/// </summary>
        [DataMember]
		public string Answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// 清算柜台ID号
		/// </summary>
        [DataMember]
		public int? CouterID
		{
			set{ _couterid=value;}
			get{return _couterid;}
		}
		/// <summary>
		/// 备注
		/// </summary>
        [DataMember]
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
	    /// <summary>
	    /// 添加类型
	    /// </summary>
        [DataMember]
	    public int AddType
	    {
            set { _addtype = value; }
            get { return _addtype; }
	    }
	    /// <summary>
	    /// 添加时间
	    /// </summary>
        [DataMember]
	    public DateTime AddTime
	    {
             set { _addtime = value; }
            get { return _addtime; }
	    }

        /// <summary>
        /// 柜台名称(此字段仅查询时用)
        /// </summary>
        [DataMember]
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
		#endregion Model

	}
}

