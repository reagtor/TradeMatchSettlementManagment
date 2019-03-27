using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:账户查询结果实体
    /// Desc.:账户查询结果实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class AccountFindResultEntity
    {
        /// <summary>
        /// 交易员ID
        /// </summary>
        private string _TraderID = string.Empty;
        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderID
        {
            get { return _TraderID; }
            set { _TraderID = value; }
        }


        /// <summary>
        /// 账号ID
        /// </summary>
        private string _AccountID = string.Empty;
        /// <summary>
        /// 账号ID
        /// </summary>
        [DataMember]
        public string AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }


        /// <summary>
        ///  账号名称
        /// </summary>
        private string _AccountName = string.Empty;
        /// <summary>
        ///  账号名称
        /// </summary>
        [DataMember]
        public string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

 

        /// <summary>
        /// 账号可用与否状态(“可用”表示可用，“被冻结”表示被冻结不可用）
        /// </summary>
        private string _AccountState = string.Empty ;
        /// <summary>
        /// 账号可用与否状态(true表示可用，false表示被冻结不可用）
        /// </summary>
        [DataMember]
        public string AccountState
        {
            get { return _AccountState; }
            set { _AccountState = value; }
        }

    }
}