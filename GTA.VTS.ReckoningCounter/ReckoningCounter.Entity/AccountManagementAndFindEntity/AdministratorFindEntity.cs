using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:管理员实体
    /// Desc.:管理员实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class AdministratorFindEntity
    {
        /// <summary>
        ///  交易员ID
        /// </summary>
        private string _TraderID = string.Empty;
        /// <summary>
        ///  交易员ID
        /// </summary>
        [DataMember]
        public string TraderID
        {
            get { return _TraderID; }
            set { _TraderID = value; }
        }


        /// <summary>
        /// 管理员ID 
        /// </summary>
        private string _AdministratorID = string.Empty;
        /// <summary>
        /// 管理员ID 
        /// </summary>
        [DataMember]
        public string AdministratorID
        {
            get { return _AdministratorID; }
            set { _AdministratorID = value; }
        }


        /// <summary>
        /// 管理员密码
        /// </summary>
        private string _AdministratorPassword = string.Empty;
        /// <summary>
        /// 管理员密码
        /// </summary>
        [DataMember]
        public string AdministratorPassword
        {
            get { return _AdministratorPassword; }
            set { _AdministratorPassword = value; }
        }




        /// <summary>
        /// 是否为现货（true表示现货， false表示期货)
        /// </summary>
        private bool _WhetherSpot = true;
        /// <summary>
        /// 是否为现货（true表示现货， false表示期货)
        /// </summary>
        [DataMember]
        public bool WhetherSpot
        {
            get { return _WhetherSpot; }
            set { _WhetherSpot = value; }
        }
    }
}