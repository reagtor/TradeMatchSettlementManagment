using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述：港股交易商品代码更新 数据访问类HKCommodityCodeUpdateDAL。
    ///作者：刘书伟
    ///日期:2009-10-31
    /// </summary>
    public class HKCommodityCodeUpdateDAL
    {
        /// <summary>
        /// 港股交易商品代码更新 
        /// </summary>
        public static void HKCommodityCodeUpdate()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HKCommodityCodeUpdataPrc");
            db.ExecuteNonQuery(dbCommand);
        }
    }
}
