using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.DAL.CommonTable
{
    /// <summary>
    ///描述：现货代码更新 数据访问类CommodityCodeUpdateDAL。
    ///作者：熊晓凌
    ///日期:2008-11-20
    /// </summary>
    public  class CommodityCodeUpdateDAL
    {
        /// <summary>
        /// 现货代码更新
        /// </summary>
        public static void CommodityCodeUpdate()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("CommodityCodeUpdataPrc");
            db.ExecuteNonQuery(dbCommand);
        }

    }
}
