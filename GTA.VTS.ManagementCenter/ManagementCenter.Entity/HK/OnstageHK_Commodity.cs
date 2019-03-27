using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：提供前台港股行业标识实体类
    ///作者：刘书伟
    ///日期:2009-10-24
    /// </summary>
    [DataContract]
    public class OnstageHK_Commodity : HK_Commodity
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
