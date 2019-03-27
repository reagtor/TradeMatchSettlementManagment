using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity.Model.QueryFilter
{
    /// <summary>
    /// Title: 分页存储过程信息实体
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class PagingProceduresInfo
    {
        #region Tables
        /// <summary>
        /// 表名,多表请使用 tableA a inner join tableB b On a.AID = b.AID
        /// </summary>
        public string Tables
        {
            get;
            set;
        }
        #endregion

        #region PK
        /// <summary>
        /// 主键，可以带表头 a.AID
        /// </summary>
        public string PK
        {
            get;
            set;
        }
        #endregion

        #region Sort
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort
        {
            get;
            set;
        }
        #endregion

        #region PageNumber
        /// <summary>
        /// 开始页码即要查询的页
        /// </summary>
        public int PageNumber
        {
            get;
            set;
        }
        #endregion

        #region PageSize
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }
        #endregion

        #region Fields
        /// <summary>
        /// 读取字段
        /// </summary>
        public string Fields
        {
            get;
            set;
        }
        #endregion

        #region Filter
        /// <summary>
        /// Where条件
        /// </summary>
        public string Filter
        {
            get;
            set;
        }
        #endregion

        #region IsCount
        /// <summary>
        /// 是否获得总记录数
        /// </summary>
        public bool IsCount
        {
            get;
            set;
        }
        #endregion

    }
}
