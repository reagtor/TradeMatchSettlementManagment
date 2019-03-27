using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.Entity;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MatchCenter.DAL
{
    /// <summary>
    /// 撮合中心现货成交回报数据访问代理类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    public class DealOrderDataAccess
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public static int Add(StockDealEntity model)
        {

            StringBuilder strSql = null;
            strSql = new StringBuilder();
            strSql.Append("insert into DealOrder(");
            strSql.Append("DealOrderNo,OrderNo,ChannelID,");
            strSql.Append("OrderPrice,DealAmount,DealTime");
            strSql.Append("");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + model.Id + "',");
            strSql.Append("'" + model.OrderNo + "',");
            strSql.Append("'" + model.ChannelNo + "',");
            strSql.Append("" + model.DealPrice + ",");
            strSql.Append("" + model.DealAmount + ",");
            strSql.Append("'" + model.DealTime + "'");
            strSql.Append(")");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                //int result;
                object obj = db.ExecuteNonQuery(dbCommand);
                return 1;
                //SqlHelper.ExecuteSql(strSql.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接受阻", ex);
                return 0;
            }

        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="dealNo">委托单号</param>
        public static int Delete(string dealNo)
        {
            if (string.IsNullOrEmpty(dealNo))
            {
                return 0;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete DealOrder ");
            strSql.Append(" where DealOrderNo='" + dealNo + "' ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            return db.ExecuteNonQuery(dbCommand);
            //return  SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        ///删除多余的数据
        /// </summary>
        public static void Delete()
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(" Delete from DealOrder where DATEDIFF(day, DealTime,'{0}') != 0", DateTime.Now.ToShortDateString());
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接受阻", ex);
                return;
            }
        }

        /// <summary>
        /// 查找成交回报实体
        /// </summary>
        /// <returns></returns>
        public static List<StockDealEntity> GetDealBackEntityList()
        {
            List<StockDealEntity> list = new List<StockDealEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from DealOrder where DATEDIFF(day, DealTime, GETDATE()) = 0 ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader == null)
                {
                    return null;
                }
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <returns></returns>
        public static StockDealEntity ReaderBind(IDataReader reader)
        {
            if (reader == null)
                return null;
            StockDealEntity model = new StockDealEntity();
            object obj;
            obj = reader["DealOrderNo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.Id = obj.ToString();
            }
            obj = reader["OrderNo"];
            if (obj != null && obj != DBNull.Value)
            {
                model.OrderNo = obj.ToString();
            }
            obj = reader["ChannelID"];
            if (obj != null && obj != DBNull.Value)
            {
                model.ChannelNo = obj.ToString();
            }
            obj = reader["OrderPrice"];
            if (obj != null && obj != DBNull.Value)
            {
                model.DealPrice = (decimal)obj;
            }
            obj = reader["DealAmount"];
            if (obj != null && obj != DBNull.Value)
            {
                model.DealAmount = (decimal)obj;
            }
            obj = reader["DealTime"];
            if (obj != null && obj != DBNull.Value)
            {
                model.DealTime = (DateTime)obj;
            }
            return model;
        }


    }
}
