using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model.UserManage
{
    /// <summary>
    /// 描述：管理员查询 实体类
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
    public class ManagerQueryEntity:UM_UserInfo
    {
        private string _managergroupname;
        private int _managergroupid;

        /// <summary>
        /// 权限组名称
        /// </summary>
        [DataMember]
        public string ManagerGroupName
        {
            set { _managergroupname = value; }
            get { return _managergroupname; }
        }
        /// <summary>
        /// 权限组ID
        /// </summary>
        [DataMember]
        public int ManagerGroupID
        {
            set { _managergroupid = value; }
            get { return _managergroupid; }
        }
    }
}
