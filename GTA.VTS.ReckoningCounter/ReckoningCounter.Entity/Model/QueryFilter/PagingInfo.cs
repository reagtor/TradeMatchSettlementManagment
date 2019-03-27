using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model
{
    /// <summary>
    /// Title: 分页信息
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class PagingInfo
    {

        #region 是否返回总页数
        /// <summary>
        /// 是否返回总页数，（设置否false）这样可以提高查询速度
        /// </summary>
        [DataMember]
        public bool IsCount
        {
            get;
            set;
        }

        #endregion

        #region 当前页
        /// <summary>
        /// 当前页 以1开始为第一页
        /// </summary>
        [DataMember]
        public int CurrentPage
        {
            get;
            set;
        }
        #endregion

        #region 每页长
        /// <summary>
        /// 每页长
        /// </summary>
        [DataMember]
        public int PageLength
        {
            get;
            set;
        }
        #endregion

        #region 排序方向
        /// <summary>
        /// 排序方向 0--asc ,1 --desc
        /// </summary>
        [DataMember]
        public int Sort
        {
            get;
            set;
        }
        #endregion
    }

}
