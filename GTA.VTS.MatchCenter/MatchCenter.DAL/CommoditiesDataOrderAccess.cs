using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.Entity;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace MatchCenter.DAL
{
    /// <summary>
    /// 撮合中心商品期货委托数据访问代理类
    /// Create BY：李健华
    /// Create Date：2010-01-21
    /// </summary>
    public class CommoditiesDataOrderAccess
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Add(CommoditiesDataOrderEntity model)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into CommoditiesDataOrder(");
            strSql.Append("OrderNo,ChannelID,ReachTime,StockCode,OrderPrice,");
            strSql.Append("OrderVolume,TransactionDirection,SholderCode,MarketPrice,OrderMark,Direction");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + model.OrderNo + "',");
            strSql.Append("'" + model.ChannelID + "',");
            strSql.Append("'" + model.ReachTime + "',");
            strSql.Append("'" + model.StockCode + "',");
            strSql.Append("" + model.OrderPrice + ",");
            strSql.Append("" + model.OrderVolume + ",");
            strSql.Append("" + model.TransactionDirection + ",");
            strSql.Append("'" + model.SholderCode + "',");
            strSql.Append("" + model.IsMarketPrice + ",");
            strSql.Append("" + model.OrderMark + ",");
            strSql.Append("" + (int)model.Direction + "");
            strSql.Append(")");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                object obj = db.ExecuteNonQuery(dbCommand);
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接开启受阻", ex);
                return 0;
            }
        }

        /// <summary>
        /// 更新委托量
        /// </summary>
        /// <param name="orderNo">委托单号码</param>
        /// <param name="volume">成交数量</param>
        public static void Update(string orderNo, decimal volume)
        {
            if (string.IsNullOrEmpty(orderNo))
                return;
            var strSql = new StringBuilder();
            strSql.Append("update CommoditiesDataOrder set OrderVolume = OrderVolume - ");
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
        /// 更新委托量，撤单成交
        /// </summary>
        /// <param name="orderNo">委托单号码</param>
        /// <param name="volume">成交数量</param>
        public static void Update(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
                return;
            var strSql = new StringBuilder();
            strSql.Append("update CommoditiesDataOrder set OrderVolume = 0.00 ");

            strSql.Append("  where OrderNo ='" + orderNo + "'");
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
            strSql.AppendFormat("update CommoditiesDataOrder set MarketVolumeNo ='{0}'", marketVolumeNo);
            strSql.AppendFormat(" , MarkVolume ='{0}'", marketVolume);
            strSql.AppendFormat(" , MatchState ='{0}'", (int)Types.MatchCenterState.other);
            strSql.AppendFormat(" where OrderNo ='{0}'", orderNo);
            strSql.AppendFormat("  OR OrderNo ='{0}'", marketVolumeNo);
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
        ///删除多余的数据
        /// </summary>
        public static void Delete()
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(" Delete from CommoditiesDataOrder where DATEDIFF(day, ReachTime, '{0}') != 0", DateTime.Now.ToShortDateString());
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
        /// 根据委托商品代码获取所有撮合委托列表
        /// </summary>
        /// <param name="code">委托商品编号</param>
        /// <returns></returns>
        public static List<CommoditiesDataOrderEntity> GetFutureEntityList(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var stockDataOrderEntities = new List<CommoditiesDataOrderEntity>();
            var strSql = new StringBuilder();
            strSql.Append("select   [OrderNo],[ChannelID],[ReachTime],[StockCode],[OrderPrice],[OrderVolume],[TransactionDirection],");
            strSql.Append("[SholderCode],[MarketPrice],[OrderMark],[Direction],[MatchState],[MarketVolumeNo],");
            strSql.Append("[MarkVolume] from CommoditiesDataOrder ");
            strSql.AppendFormat(" where DATEDIFF(day, ReachTime, '{0}') = 0 and OrderVolume >0.00 ", DateTime.Now.ToShortDateString());
            strSql.Append(" and StockCode ='" + code + "'");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader == null)
                        return null;
                    while (dataReader.Read())
                    {
                        stockDataOrderEntities.Add(ReaderBind(dataReader));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接开启受阻", ex);
            }
            return stockDataOrderEntities;
        }
        /// <summary>
        /// 根据委托单号获取所有撮合委托实体
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public static CommoditiesDataOrderEntity GetFutureEntityByOrderNo(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
            {
                return null;
            }
            var stockDataOrderEntities = new CommoditiesDataOrderEntity();
            var strSql = new StringBuilder();
            strSql.Append("select   [OrderNo],[ChannelID],[ReachTime],[StockCode],[OrderPrice],[OrderVolume],[TransactionDirection],");
            strSql.Append("[SholderCode],[MarketPrice],[OrderMark],[Direction],[MatchState],[MarketVolumeNo],");
            strSql.Append("[MarkVolume] from CommoditiesDataOrder  where  ");
            strSql.Append("   OrderNo ='" + orderNo + "'");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader == null)
                        return null;
                    while (dataReader.Read())
                    {
                        stockDataOrderEntities = ReaderBind(dataReader);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接开启受阻", ex);
            }
            return stockDataOrderEntities;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static CommoditiesDataOrderEntity ReaderBind(IDataReader dataReader)
        {
            if (dataReader == null)
                return null;
            var model = new CommoditiesDataOrderEntity();
            object ojb;
            if (dataReader["OrderNo"] != null)
            {
                model.OrderNo = dataReader["OrderNo"].ToString();
            }
            if (dataReader["ChannelID"] != null)
            {
                model.ChannelID = dataReader["ChannelID"].ToString();
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
            ojb = dataReader["TransactionDirection"];
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
            ojb = dataReader["OrderMark"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderMark = (int)ojb;
            }
            ojb = dataReader["Direction"];

            if (ojb != null && ojb != DBNull.Value)
            {

                model.Direction = (CHDirection)ojb;
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
