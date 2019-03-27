using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace MatchCenter.DAL
{


    public class SqlHelper
    {

        //数据库连接字符串
        protected static string connectionString = ConfigurationManager.AppSettings["Connection"].ToString();

        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection CreateConnection()
        {

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            return conn;
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, string commandText, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand(commandText, conn);
            cmd.Transaction = trans;
            SqlDataReader reader = cmd.ExecuteReader();
            cmd.Dispose();
            return reader;
        }


        public static SqlDataReader ExecuteReader(string commandText)
        {
            SqlConnection connection = CreateConnection();
            SqlCommand cmd = new SqlCommand(commandText, connection);
            SqlDataReader reader;
            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();
                return reader;
            }
            catch
            {
                connection.Close();

            }
            return null;

        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, string commandText)
        {
            return ExecuteReader(conn, commandText, null);
        }


        public static int ExecuteNoneQuery(string conn, string strSQL)
        {

            using (SqlConnection connection = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, connection))
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
            //return 0;
        }


        public static object ExecuteScalar(string SQLString)
        {
            object obj = new object();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    connection.Open();
                    obj = cmd.ExecuteScalar();
                }
            }
            return obj;
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch
                    {
                        connection.Close();

                    }
                }
            }
            return 0;
        }




    }
}
