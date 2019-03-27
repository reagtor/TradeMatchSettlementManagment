using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.DAL
{
    /// <summary>
    /// 执行SQL帮助类
    /// Create BY：李健华
    /// Create Date： 2009-08-19
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        /// 私有变量
        /// </summary>
        private static string connString = "";
        /// <summary>
        /// 获取数据连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connString))
            {
                Database db = DatabaseFactory.CreateDatabase();
                using (var conn = db.CreateConnection())
                {
                    connString = conn.ConnectionString;
                }
                if (!connString.EndsWith(";"))
                    connString += ";";
                connString += "Asynchronous Processing=true";
            }
            return connString;
        }
        /// <summary>
        /// 异步执行SQL。
        /// 内部没有关闭连接，所以在执行后要提供callBack方法回调用监控关闭数据库连接
        /// </summary>
        /// <param name="sqlText">要执行的SQLText</param>
        /// <param name="callBack">回执行监控事件</param>
        public static void AsyncExecuteNonQuery(string sqlText, AsyncCallback callBack)
        {
            //这只是为了获取配置文件中的数据连接字符串，为了不再添加key值，
            //所以这里为了方便一起使用。其可以自行设置key获取也可以
            //Database db = DatabaseFactory.CreateDatabase();
            //string connString = "";
            //using (var conn = db.CreateConnection())
            //{
            //    connString = conn.ConnectionString;

            //}
            //if (!connString.EndsWith(";"))
            //    connString += ";";
            //connString += "Asynchronous Processing=true";
            //关闭数据库连接要在callback中关闭，因为是异步操作
            //using (SqlConnection connection = new SqlConnection())
            //{
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = GetConnectionString();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlText;
                connection.Open();
                cmd.BeginExecuteNonQuery(callBack, cmd);
            }
            catch (Exception ex)
            {
                if (connection != null)
                {
                    connection.Close();
                }
                LogHelper.WriteError(ex.Message, ex);
            }
            //}
        }
        /// <summary>
        /// 默认执行异步SQL后关闭连接回调方法，
        /// 方法内不执行相关的查询返回的记录相关操作
        /// </summary>
        /// <param name="callBack"></param>
        public static void CallbackAsyncExecuteNonQuery(IAsyncResult callBack)
        {
            SqlCommand cmm = null;
            try
            {
                cmm = (SqlCommand)callBack.AsyncState;
                if (cmm == null)
                {
                    return;
                }
                cmm.EndExecuteNonQuery(callBack);
            }
            catch (Exception ex)
            {
                if (cmm != null)
                {
                    LogHelper.WriteError("CH-0001:异步执行SQL异常" + cmm.CommandText, ex);
                    cmm.Dispose();
                }
                else
                {
                    LogHelper.WriteError("CH-0001:异步执行SQL异常", ex);
                }
            }
            finally
            {
                if (cmm != null && cmm.Connection != null && cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Dispose();
                    cmm.Connection.Close();
                }
            }

            #region old code
            //try
            //{
            //    int i = 0;
            //    using (cmm = callBack.AsyncState as SqlCommand)
            //    {

            //        while (!callBack.IsCompleted)
            //        {
            //            LogHelper.WriteDebug("CH-0001:异步执行还没有成功正在等待" + i + "次");
            //            System.Threading.Thread.CurrentThread.Join(500);
            //            if (i > 5)
            //            {
            //                break;
            //            }
            //            i++;
            //        }
            //        //BeginExecuteNonQuery 方法會開始以非同步方式執行不傳回資料列之 Transact-SQL 
            //        //陳述式或預存程序的處理序，以便執行陳述式時可同時執行其他工作。
            //        //完成陳述式時，開發人員必須呼叫 EndExecuteNonQuery 方法以完成作業。
            //        //BeginExecuteNonQuery 方法會立即傳回，但在程式碼執行對應的 EndExecuteNonQuery 
            //        //方法呼叫之前，都不可以執行會根據相同 SqlCommand 物件而開始同步或非同步執行的
            //        //其任何呼叫。在命令執行完成之前呼叫 EndExecuteNonQuery，
            //        //將造成 SqlCommand 物件在執行完成之前都處於封鎖狀態。
            //        if (callBack.IsCompleted)
            //        {
            //            cmm.EndExecuteNonQuery(callBack);
            //        }
            //        else
            //        {
            //            LogHelper.WriteError("CH-0001:异步执行SQL不成功" + cmm.CommandText, new Exception(""));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (cmm != null)
            //    {
            //        LogHelper.WriteError("CH-0001:异步执行SQL异常" + cmm.CommandText, ex);
            //    }
            //    else
            //    {
            //        LogHelper.WriteError("CH-0001:异步执行SQL异常", ex);
            //    }
            //}
            #endregion
        }


    }
}
