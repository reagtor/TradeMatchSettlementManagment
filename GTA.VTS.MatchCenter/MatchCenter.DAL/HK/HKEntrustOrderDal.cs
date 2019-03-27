using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using MatchCenter.Entity.HK;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.DAL.HK
{

    /// <summary>
    /// 撮合中心数据访问类HKEntrustOrderDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// Desc.:添加相关的数据更新方法和字段个改
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class HKEntrustOrderDal
    {
        public HKEntrustOrderDal()
        { }


        /// <summary>
        /// 根据主键判断是否存在该记录
        /// <param name="orderNo">主键委托编号</param>
        /// </summary>
        public static bool Exists(string orderNo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HKEntrustOrder where OrderNo=@OrderNo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, orderNo);
            int cmdresult;
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 增加一条数据
        /// <param name="model">委托记录实体</param>
        /// </summary>
        public static void Add(HKEntrustOrderInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HKEntrustOrder(");
            strSql.Append("SholderCode,OrderNo,BranchID,HKSecuritiesCode,OldVolume,ReceiveTime,OrderPrice,OrderVolume,OrderType,TradeType,OrderMessage)");

            strSql.Append(" values (");
            strSql.Append("@SholderCode,@OrderNo,@BranchID,@HKSecuritiesCode,@OldVolume,@ReceiveTime,@OrderPrice,@OrderVolume,@OrderType,@TradeType,@OrderMessage)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "SholderCode", DbType.AnsiString, model.SholderCode);
            db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, model.OrderNo);
            db.AddInParameter(dbCommand, "BranchID", DbType.AnsiString, model.BranchID);
            db.AddInParameter(dbCommand, "HKSecuritiesCode", DbType.AnsiString, model.HKSecuritiesCode);
            db.AddInParameter(dbCommand, "OldVolume", DbType.Decimal, model.OldVolume);
            db.AddInParameter(dbCommand, "ReceiveTime", DbType.DateTime, model.ReceiveTime);
            db.AddInParameter(dbCommand, "OrderPrice", DbType.Decimal, model.OrderPrice);
            db.AddInParameter(dbCommand, "OrderVolume", DbType.Decimal, model.OrderVolume);
            db.AddInParameter(dbCommand, "OrderType", DbType.Int32, model.OrderType);
            db.AddInParameter(dbCommand, "TradeType", DbType.Int32, model.TradeType);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        ///  根据主键更新一条数据
        /// <param name="model">委托记录实体</param>
        /// </summary>
        public static void Update(HKEntrustOrderInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HKEntrustOrder set ");
            strSql.Append("SholderCode=@SholderCode,");
            strSql.Append("BranchID=@BranchID,");
            strSql.Append("HKSecuritiesCode=@HKSecuritiesCode,");
            strSql.Append("OldVolume=@OldVolume,");
            strSql.Append("ReceiveTime=@ReceiveTime,");
            strSql.Append("OrderPrice=@OrderPrice,");
            strSql.Append("OrderVolume=@OrderVolume,");
            strSql.Append("OrderType=@OrderType,");
            strSql.Append("TradeType=@TradeType,");
            strSql.Append("OrderMessage=@OrderMessage");
            strSql.Append(" where OrderNo=@OrderNo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "SholderCode", DbType.AnsiString, model.SholderCode);
            db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, model.OrderNo);
            db.AddInParameter(dbCommand, "BranchID", DbType.AnsiString, model.BranchID);
            db.AddInParameter(dbCommand, "HKSecuritiesCode", DbType.AnsiString, model.HKSecuritiesCode);
            db.AddInParameter(dbCommand, "OldVolume", DbType.Decimal, model.OldVolume);
            db.AddInParameter(dbCommand, "ReceiveTime", DbType.DateTime, model.ReceiveTime);
            db.AddInParameter(dbCommand, "OrderPrice", DbType.Decimal, model.OrderPrice);
            db.AddInParameter(dbCommand, "OrderVolume", DbType.Decimal, model.OrderVolume);
            db.AddInParameter(dbCommand, "OrderType", DbType.Int32, model.OrderType);
            db.AddInParameter(dbCommand, "TradeType", DbType.Int32, model.TradeType);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.ExecuteNonQuery(dbCommand);

        }
        /// <summary>
        /// 更新委托，根据委托单号把委托量置为0即委托量已交交完
        /// </summary>
        /// <param name="orderNo">委托编号</param>
        public static void Update(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
                return;
            var strSql = new StringBuilder();
            strSql.Append("update HKEntrustOrder set OrderVolume =0.00 ");
            strSql.Append(" where OrderNo ='" + orderNo + "'");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch
            {
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
            strSql.AppendFormat("update HKEntrustOrder set MarketVolumeNo ='{0}'", marketVolumeNo);
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
        /// 改单更新原委托信息
        /// </summary>
        /// <param name="orderNo">委托单号码</param>
        /// <param name="volume">委托数量</param>
        /// <param name="nowVolume">当前委托剩余量</param>
        public static void Update(string orderNo, decimal volume, decimal nowVolume)
        {
            if (string.IsNullOrEmpty(orderNo))
                return;
            var strSql = new StringBuilder();
            strSql.Append("update HKEntrustOrder set OldVolume ='" + volume + "',  OrderVolume ='" + nowVolume + "'   ");
            strSql.Append(" where OrderNo ='" + orderNo + "'");
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库操作异常", ex);
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
            strSql.Append("update HKEntrustOrder set OrderVolume = OrderVolume - ");
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
                LogHelper.WriteError("CH-111:数据库操作异常", ex);
            }
        }

        /// <summary>
        /// 根据主键删除一条数据
        /// <param name="orderNo">委托编号(主键)</param>
        /// </summary>
        public static void Delete(string orderNo)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from HKEntrustOrder ");
                strSql.Append(" where OrderNo=@OrderNo ");
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, orderNo);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库操作异常", ex);
            }

        }

        /// <summary>
        ///删除多余的数据,即删除不是今天的数据
        /// </summary>
        public static void Delete()
        {
            string deleteIsNotTodayData = " Delete from HKEntrustOrder where DATEDIFF(day, ReceiveTime, '" + DateTime.Now.ToShortDateString() + "') != 0";
            SqlHelper.AsyncExecuteNonQuery(deleteIsNotTodayData, SqlHelper.CallbackAsyncExecuteNonQuery);
        }

        /// <summary>
        /// 根据主键得到一个实体（记录）
        /// <param name="orderNo">委托编号(主键)</param>
        /// </summary>
        public static HKEntrustOrderInfo GetModel(string orderNo)
        {
            HKEntrustOrderInfo model = null;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select SholderCode,OrderNo,BranchID,HKSecuritiesCode,OldVolume,ReceiveTime,OrderPrice,OrderVolume,OrderType,TradeType,OrderMessage,MatchState,MarketVolumeNo,MarkVolume from HKEntrustOrder ");
                strSql.Append(" where OrderNo=@OrderNo ");
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, orderNo);

                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        model = ReaderBind(dataReader);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库操作异常", ex);
            }
            return model;
        }

        /// <summary>
        /// 根据查询条件语句获得数据列表
        /// <param name="strWhere">查询条件SQL语句</param>
        /// </summary>
        public static DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SholderCode,OrderNo,BranchID,HKSecuritiesCode,OldVolume,ReceiveTime,OrderPrice,OrderVolume,OrderType,TradeType,OrderMessage,MatchState,MarketVolumeNo,MarkVolume ");
            strSql.Append(" FROM HKEntrustOrder ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 根据查询条件语句获得数据列表
        /// <param name="strWhere">查询条件SQL语句</param>
        /// </summary>
        public static List<HKEntrustOrderInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SholderCode,OrderNo,BranchID,HKSecuritiesCode,OldVolume,ReceiveTime,OrderPrice,OrderVolume,OrderType,TradeType,OrderMessage,MatchState,MarketVolumeNo,MarkVolume ");
            strSql.Append(" FROM HKEntrustOrder ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HKEntrustOrderInfo> list = new List<HKEntrustOrderInfo>();
            Database db = DatabaseFactory.CreateDatabase();
            using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

        /// <summary>
        /// 对象实体绑定数据
        /// <param name="dataReader">IDataReader对象</param>
        /// </summary>
        public static HKEntrustOrderInfo ReaderBind(IDataReader dataReader)
        {
            HKEntrustOrderInfo model = new HKEntrustOrderInfo();
            object ojb;
            model.SholderCode = dataReader["SholderCode"].ToString();
            model.OrderNo = dataReader["OrderNo"].ToString();
            model.BranchID = dataReader["BranchID"].ToString();
            model.HKSecuritiesCode = dataReader["HKSecuritiesCode"].ToString();
            ojb = dataReader["OldVolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OldVolume = (decimal)ojb;
            }
            ojb = dataReader["ReceiveTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ReceiveTime = (DateTime)ojb;
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
                model.OrderType = (int)ojb;
            }
            ojb = dataReader["OrderType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeType = (int)ojb;
            }
            model.OrderMessage = dataReader["OrderMessage"].ToString();

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

        /// <summary>
        /// 根据代码获取所有撮合委托
        /// </summary>
        /// <param name="code">委托商品代码</param>
        /// <returns></returns>
        public static List<HKEntrustOrderInfo> GetHKEntrustOrderList(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var list = new List<HKEntrustOrderInfo>();
            var strSql = new StringBuilder();
            strSql.Append("select SholderCode,OrderNo,BranchID,HKSecuritiesCode,OldVolume,ReceiveTime,OrderPrice,OrderVolume,OrderType,TradeType,OrderMessage ,MatchState,MarketVolumeNo,MarkVolume");
            strSql.AppendFormat("  FROM HKEntrustOrder where DATEDIFF(day, ReceiveTime,'{0}') = 0 and OrderVolume >0.00 ", DateTime.Now.ToShortDateString());
            strSql.Append(" and HKSecuritiesCode ='" + code + "'");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader == null)
                    return null;
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

    }
}
