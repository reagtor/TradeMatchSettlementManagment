#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.QueryFilter;

#endregion

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类XH_HistoryEntrustTableDal。
    /// </summary>
    public class XH_HistoryEntrustTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string EntrustNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_HistoryEntrustTable where EntrustNumber=@EntrustNumber ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
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
        /// </summary>
        public void Add(XH_HistoryEntrustTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_HistoryEntrustTable(");
            strSql.Append("EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,SpotCode,TradeAmount,TradeAveragePrice,IsMarketValue,CancelAmount,BuySellTypeId,StockAccount,CapitalAccount,CurrencyTypeId,TradeUnitId,OrderStatusId,OrderMessage,McOrderId,HasDoneProfit,OfferTime,EntrustTime)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PortfolioLogo,@EntrustPrice,@EntrustMount,@SpotCode,@TradeAmount,@TradeAveragePrice,@IsMarketValue,@CancelAmount,@BuySellTypeId,@StockAccount,@CapitalAccount,@CurrencyTypeId,@TradeUnitId,@OrderStatusId,@OrderMessage,@McOrderId,@HasDoneProfit,@OfferTime,@EntrustTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustMount", DbType.Int32, model.EntrustMount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(XH_HistoryEntrustTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_HistoryEntrustTable set ");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("EntrustMount=@EntrustMount,");
            strSql.Append("SpotCode=@SpotCode,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("IsMarketValue=@IsMarketValue,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("StockAccount=@StockAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("OrderStatusId=@OrderStatusId,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("McOrderId=@McOrderId,");
            strSql.Append("HasDoneProfit=@HasDoneProfit,");
            strSql.Append("OfferTime=@OfferTime,");
            strSql.Append("EntrustTime=@EntrustTime");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustMount", DbType.Int32, model.EntrustMount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string EntrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_HistoryEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_HistoryEntrustTableInfo GetModel(string EntrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,SpotCode,TradeAmount,TradeAveragePrice,IsMarketValue,CancelAmount,BuySellTypeId,StockAccount,CapitalAccount,CurrencyTypeId,TradeUnitId,OrderStatusId,OrderMessage,McOrderId,HasDoneProfit,OfferTime,EntrustTime from XH_HistoryEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            XH_HistoryEntrustTableInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<XH_HistoryEntrustTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,SpotCode,TradeAmount,TradeAveragePrice,IsMarketValue,CancelAmount,BuySellTypeId,StockAccount,CapitalAccount,CurrencyTypeId,TradeUnitId,OrderStatusId,OrderMessage,McOrderId,HasDoneProfit,OfferTime,EntrustTime ");
            strSql.Append(" FROM XH_HistoryEntrustTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_HistoryEntrustTableInfo> list = new List<XH_HistoryEntrustTableInfo>();
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
        /// </summary>
        public XH_HistoryEntrustTableInfo ReaderBind(IDataReader dataReader)
        {
            XH_HistoryEntrustTableInfo model = new XH_HistoryEntrustTableInfo();
            object ojb;
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            model.PortfolioLogo = dataReader["PortfolioLogo"].ToString();
            ojb = dataReader["EntrustPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustPrice = (decimal)ojb;
            }
            ojb = dataReader["EntrustMount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustMount = (int)ojb;
            }
            model.SpotCode = dataReader["SpotCode"].ToString();
            ojb = dataReader["TradeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeAmount = (int)ojb;
            }
            ojb = dataReader["TradeAveragePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeAveragePrice = (decimal)ojb;
            }
            ojb = dataReader["IsMarketValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsMarketValue = (bool)ojb;
            }
            ojb = dataReader["CancelAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CancelAmount = (int)ojb;
            }
            ojb = dataReader["BuySellTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BuySellTypeId = (int)ojb;
            }
            model.StockAccount = dataReader["StockAccount"].ToString();
            model.CapitalAccount = dataReader["CapitalAccount"].ToString();
            ojb = dataReader["CurrencyTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeId = (int)ojb;
            }
            ojb = dataReader["TradeUnitId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeUnitId = (int)ojb;
            }
            ojb = dataReader["OrderStatusId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderStatusId = (int)ojb;
            }
            model.OrderMessage = dataReader["OrderMessage"].ToString();
            model.McOrderId = dataReader["McOrderId"].ToString();
            ojb = dataReader["HasDoneProfit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HasDoneProfit = (decimal)ojb;
            }
            ojb = dataReader["OfferTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OfferTime = (DateTime)ojb;
            }
            ojb = dataReader["EntrustTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustTime = (DateTime)ojb;
            }
            return model;
        }


        ///// <summary>
        ///// 根据条件分页查询
        ///// </summary>
        ///// <param name="pageProcInfo">分页存储过程过滤条件</param>
        ///// <param name="total">总页数</param>
        ///// <returns></returns>
        //public List<XH_HistoryEntrustTableInfo> PagingXH_HistoryEntrustByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<XH_HistoryEntrustTableInfo> list = new List<XH_HistoryEntrustTableInfo>();
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = CommonDALOperate.PagingProceduresDbCommand(db, pageProcInfo);
        //    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
        //    {
        //        while (dataReader.Read())
        //        {
        //            list.Add(ReaderBind(dataReader));
        //        }

        //    }
        //    total = db.GetParameterValue(dbCommand, "@Total") != null ? (int)db.GetParameterValue(dbCommand, "@Total") : 0;
        //    return list;
        //}
        #endregion  成员方法
    }
}