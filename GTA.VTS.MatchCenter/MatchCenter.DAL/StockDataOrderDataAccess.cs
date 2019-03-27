using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using GTA.VTS.Common.CommonObject;

namespace MatchCenter.DAL
{
    /// <summary>
    /// 撮合中心委托数据访问类
    /// Create BY：李健华
    /// Create Date： 2009-08-19
    /// </summary>
    public class StockDataOrderDataAccess
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Add(StockDataOrderEntity model)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into StockDataOrder(");
            strSql.Append("OrderNo,BranchID,ReachTime,StockCode,OrderPrice,");
            strSql.Append("OrderVolume,OrderType,SholderCode,MarketPrice");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + model.OrderNo + "',");
            strSql.Append("'" + model.ChannelNo + "',");
            strSql.Append("'" + model.ReachTime + "',");
            strSql.Append("'" + model.StockCode + "',");
            strSql.Append("" + model.OrderPrice + ",");
            strSql.Append("" + model.OrderVolume + ",");
            strSql.Append("" + model.TransactionDirection + ",");
            strSql.Append("'" + model.SholderCode + "',");
            strSql.Append("" + model.IsMarketPrice + "");
            strSql.Append(")");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                //int result;
                object obj = db.ExecuteNonQuery(dbCommand);
                return 1;

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新成交回报
        /// </summary>
        /// <param name="orderNo">委托单号码</param>
        /// <param name="volume">委托数量</param>
        public static void Update(string orderNo, decimal volume)
        {
            if (string.IsNullOrEmpty(orderNo))
                return;
            var strSql = new StringBuilder();
            strSql.Append("update StockDataOrder set OrderVolume = OrderVolume - ");
            strSql.Append(volume);
            strSql.Append(" where OrderNo ='" + orderNo + "'");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接开启受阻", ex);
                return;
            }
        }
        /// <summary>
        /// 更新成交回报
        /// </summary>
        /// <param name="orderNo">委托编号</param>
        public static void Update(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
                return;
            var strSql = new StringBuilder();
            strSql.Append("update StockDataOrder set OrderVolume =0.00 ");
            strSql.Append(" where OrderNo ='" + orderNo + "'");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接开启受阻", ex);
                return;
            }
        }
        /// <summary>
        /// 更市场存量和关联市场存量委托编号和更新是担合状态，这里直接更新撮合状态为其他
        /// </summary>
        /// <param name="orderNo">委托编号</param>
        /// <param name="marketVolumeNo">关联市场存量委托编号</param>
        /// <param name="marketVolume">市场存量</param>
        public static void Update(string orderNo, string marketVolumeNo, decimal marketVolume)
        {
            if (string.IsNullOrEmpty(orderNo))
                return;
            var strSql = new StringBuilder();
            strSql.AppendFormat("update StockDataOrder set MarketVolumeNo ='{0}'", marketVolumeNo);
            strSql.AppendFormat(" , MarkVolume ='{0}'", marketVolume);
            strSql.AppendFormat(" , MatchState ='{0}'", (int)Types.MatchCenterState.other);
            strSql.AppendFormat(" where OrderNo ='{0}'", orderNo);
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接开启受阻", ex);
                return;
            }
        }
        //private static readonly string deleteIsNotTodayData = " Delete from StockDataOrder where DATEDIFF(day, ReachTime, '{0}') != 0";

        /// <summary>
        ///删除多余的数据,即删除不是今天的数据
        /// </summary>
        public static void Delete()
        {
            string deleteIsNotTodayData = " Delete from StockDataOrder where DATEDIFF(day, ReachTime, '" + DateTime.Now.ToShortDateString() + "') != 0";
            SqlHelper.AsyncExecuteNonQuery(deleteIsNotTodayData, SqlHelper.CallbackAsyncExecuteNonQuery);
            #region 改用异步执行
            //var strSql = new StringBuilder();
            //strSql.Append(" Delete from StockDataOrder where DATEDIFF(day, ReachTime, GETDATE()) != 0");
            //try
            //{
            //    Database db = DatabaseFactory.CreateDatabase();
            //    DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());

            //     db.ExecuteNonQuery(dbCommand);
            //}
            //catch(Exception ex)
            //{
            //    LogHelper.WriteError("CH-111:数据库连接受阻", ex);
            //    return;
            //}
            #endregion
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public static void DeleteDeal()
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(" Delete from DealOrder where DATEDIFF(day, ReachTime, '{0}') != 0 ", DateTime.Now.ToShortDateString());
            SqlHelper.AsyncExecuteNonQuery(strSql.ToString(), SqlHelper.CallbackAsyncExecuteNonQuery);
            //try
            //{
            //    Database db = DatabaseFactory.CreateDatabase();
            //    DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            //    db.ExecuteNonQuery(dbCommand);
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError("CH-111:数据库连接受阻", ex);
            //    return;
            //}
        }


        /// <summary>
        /// 根据委托商品代码获取所有撮合委托列表
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns></returns>
        public static List<StockDataOrderEntity> GetStockEntityList(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            try
            {
                var stockDataOrderEntities = new List<StockDataOrderEntity>();
                var strSql = new StringBuilder();
                strSql.Append(
                    "select OrderNo,BranchID,ReachTime,StockCode,OrderPrice,OrderVolume,OrderType,SholderCode,MarketPrice,MatchState,MarketVolumeNo,MarkVolume from StockDataOrder where DATEDIFF(day, ReachTime, GETDATE()) = 0 and OrderVolume >0.00 ");
                strSql.Append(" and StockCode ='" + code + "'");
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        stockDataOrderEntities.Add(ReaderBind(dataReader));
                    }
                }
                return stockDataOrderEntities;
            }
            catch (Exception ex)
            {
                //处理有可能超时的动作
                LogHelper.WriteError("CH-111:数据库连接开启受阻StockDataOrderDataAccess.GetStockEntityByOrderNo()", ex);
                return null;

            }
        }

        /// <summary>
        /// 根据委托单号获取所有撮合委托实体
        /// </summary>
        /// <param name="orderNo">商品代码</param>
        /// <returns></returns>
        public static StockDataOrderEntity GetStockEntityByOrderNo(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
            {
                return null;
            }
            var stockDataOrderEntities = new StockDataOrderEntity();
            var strSql = new StringBuilder();
            strSql.Append(
                "select OrderNo,BranchID,ReachTime,StockCode,OrderPrice,OrderVolume,OrderType,SholderCode,MarketPrice,MatchState,MarketVolumeNo,MarkVolume from StockDataOrder where  ");
            strSql.Append("  OrderNo ='" + orderNo + "'");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    stockDataOrderEntities = ReaderBind(dataReader);
                }
            }
            return stockDataOrderEntities;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="dataReader">dataReader</param>
        /// <returns></returns>
        public static StockDataOrderEntity ReaderBind(IDataReader dataReader)
        {
            if (dataReader == null)
                return null;
            var model = new StockDataOrderEntity();
            object ojb;
            if (dataReader["OrderNo"] != null)
            {
                model.OrderNo = dataReader["OrderNo"].ToString();
            }
            if (dataReader["BranchID"] != null)
            {
                model.ChannelNo = dataReader["BranchID"].ToString();
            }
            ojb = dataReader["ReachTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ReachTime = (DateTime)ojb;
            }
            if (dataReader["StockCode"] != null)
            {
                model.StockCode = dataReader["StockCode"].ToString();
            }
            ojb = dataReader["OrderPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderPrice = (decimal)ojb;
            }
            ojb = dataReader["OrderVolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderVolume = (decimal)ojb;
            }
            ojb = dataReader["OrderType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TransactionDirection = (int)ojb;
            }
            if (dataReader["SholderCode"] != null)
            {
                model.SholderCode = dataReader["SholderCode"].ToString();
            }

            ojb = dataReader["MarketPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsMarketPrice = (int)ojb;
            }

            ojb = dataReader["MatchState"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MatchState = (Types.MatchCenterState)ojb;
            }
            if (dataReader["MarketVolumeNo"] != null)
            {
                model.MarketVolumeNo = dataReader["MarketVolumeNo"].ToString();
            }
            ojb = dataReader["MarkVolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarkLeft = (decimal)ojb;
            }

            return model;
        }
    }


}