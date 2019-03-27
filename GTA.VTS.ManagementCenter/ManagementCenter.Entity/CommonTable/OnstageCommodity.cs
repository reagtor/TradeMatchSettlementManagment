using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model.CommonTable
{
    /// <summary>
    /// 描述:行业标识实体类
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
    public class OnstageCommodity:CM_Commodity
    {
        /// <summary>
        /// 行业标识
        /// </summary>
        private string _nindcd;
        [DataMember]
        public string Nindcd
        {
            set { _nindcd = value; }
            get { return _nindcd; }
        }
    }
}
