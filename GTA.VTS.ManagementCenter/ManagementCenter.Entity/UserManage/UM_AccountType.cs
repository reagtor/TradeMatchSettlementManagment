using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：帐号类型 实体类UM_AccountType 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
    public class UM_AccountType
    {
        public UM_AccountType()
        { }
        #region Model
        private int _accounttypeid;
        private string _accountname;
        private int? _accountattributiontype;
        private string _remark;
        private int? _connectholdid; 
        /// <summary>
        /// 帐户类型ID
        /// </summary>
        [DataMember]
        public int AccountTypeID
        {
            set { _accounttypeid = value; }
            get { return _accounttypeid; }
        }
        /// <summary>
        /// 帐户名称
        /// </summary>
        [DataMember]
        public string AccountName
        {
            set { _accountname = value; }
            get { return _accountname; }
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
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string ReMark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// 对应持仓帐号类型ID
        /// </summary>
        [DataMember]
        public int? ConnectHoldID
        {
            set { _connectholdid = value; }
            get { return _connectholdid; }
        }
        #endregion Model

    }
}

