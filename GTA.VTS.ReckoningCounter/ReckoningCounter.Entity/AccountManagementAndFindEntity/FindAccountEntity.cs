using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:查询账户实体
    /// Desc.:查询账户实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FindAccountEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        private string _UserID = string.Empty;
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }


        /// <summary>
        ///  用户密码
        /// </summary>
        private string _UserPassword = string.Empty;
        /// <summary>
        ///  用户密码
        /// </summary>
        [DataMember]
        public string UserPassword
        {
            get { return _UserPassword; }
            set { _UserPassword = value; }
        }


        /// <summary>
        /// 角色
        /// </summary>
        private int _RoleNumber;
        /// <summary>
        /// 角色
        /// </summary>
        [DataMember]
        public int RoleNumber
        {
            get { return _RoleNumber; }
            set { _RoleNumber = value; }
        }

        /// <summary>
        /// 要查询的账号类型(目前只有9种账号类型）
        /// </summary>
        private int _accountType;
        /// <summary>
        /// 要查询的账号类型(目前只有9种账号类型）
        /// </summary>
        [DataMember]
        public int AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }


        /// <summary>
        ///账号所属类型(5种类型之一)
        /// </summary>
        private int _accountAttributionType;
        /// <summary>
        /// 账号所属类型(5种类型之一)
        /// </summary>
        [DataMember]
        public int AccountAttributionType
        {
            get { return _accountAttributionType; }
            set { _accountAttributionType = value; }
        }

    }
}