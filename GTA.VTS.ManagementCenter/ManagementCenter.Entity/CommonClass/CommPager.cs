using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace ManagementCenter.Model.CommonClass
{
    /// <summary>
    /// 描述:分页方法
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public static class CommPager
    {
        /// <summary>
        /// DAL层分页方法
        /// </summary>
        /// <param name="dataBase">数据对象</param>
        /// <param name="dbCommand">命令对象</param>
        /// <param name="sqlText">SQL语句</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页多少行数据</param>
        /// <param name="rowCount">数据行数量</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static DataSet QueryPager(Database dataBase, DbCommand dbCommand, string sqlText, int pageNo, int pageSize, out int rowCount, string tableName)
        {
            int startRecord = (pageNo - 1) * pageSize;
            int maxRecord = pageSize;
            try
            {
                dbCommand.Connection = dataBase.CreateConnection();
                DbDataAdapter dataAdapter = dataBase.GetDataAdapter();
                dataAdapter.SelectCommand = dbCommand;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, startRecord, maxRecord, tableName);
                if (pageNo == 1)
                {

                    string SQL = "SELECT COUNT(*) FROM (" + sqlText + ") AS A";
                    dbCommand.CommandText = SQL;
                    rowCount = Convert.ToInt32(dataBase.ExecuteScalar(dbCommand).ToString());
                }
                else
                {
                    rowCount = int.MaxValue;
                }

                dbCommand.Connection.Close();
                return dataSet;
            }
            finally
            {
                dbCommand.Connection.Close();
                //rowCount = 0;
                //return null;
            }
        }
    }
}
