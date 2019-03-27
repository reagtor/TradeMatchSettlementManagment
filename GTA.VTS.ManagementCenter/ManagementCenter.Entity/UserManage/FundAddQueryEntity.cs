using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model.UserManage
{
    /// <summary>
    /// 描述：资金查询 实体类
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
    public class FundAddQueryEntity : UM_FundAddInfo
    {
        private string _longinname;
        //private int _managerid;

        /// <summary>
        /// 管理员ID
        /// </summary>
        //public int ManagerID
        //{
        //    set { _managerid = value; }
        //    get { return _managerid; }
        //}

        /// <summary>
        /// 管理员登陆名称
        /// </summary>
        [DataMember]
        public string loginName
        {
            set { _longinname = value; }
            get { return _longinname; }
        }
    }
}
