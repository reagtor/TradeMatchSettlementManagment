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
    /// 数据访问类XH_HistoryTradeTableDal。
    /// </summary>
    public class XH_HistoryTradeTableDal
    {
        #region  成员方法


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string TradeNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_HistoryTradeTable where TradeNumber=@TradeNumber ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, TradeNumber);
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
        public void Add(XH_HistoryTradeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_HistoryTradeTable(");
            strSql.Append("TradeNumber,EntrustNumber,EntrustPrice,TradePrice,PortfolioLogo,TradeAmount,StampTax,Commission,SpotCode,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,ClearingFee,StockAccount,CapitalAccount,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime)");

            strSql.Append(" values (");
            strSql.Append("@TradeNumber,@EntrustNumber,@EntrustPrice,@TradePrice,@PortfolioLogo,@TradeAmount,@StampTax,@Commission,@SpotCode,@TransferAccountFee,@TradeProceduresFee,@MonitoringFee,@TradingSystemUseFee,@ClearingFee,@StockAccount,@CapitalAccount,@TradeTypeId,@TradeUnitId,@BuySellTypeId,@CurrencyTypeId,@TradeTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "StampTax", DbType.Decimal, model.StampTax);
            db.AddInParameter(dbCommand, "Commission", DbType.Decimal, model.Commission);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TransferAccountFee", DbType.Decimal, model.TransferAccountFee);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            db.AddInParameter(dbCommand, "TradingSystemUseFee", DbType.Decimal, model.TradingSystemUseFee);
            db.AddInParameter(dbCommand, "ClearingFee", DbType.Decimal, model.ClearingFee);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "TradeTypeId", DbType.Int32, model.TradeTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeTime", DbType.DateTime, model.TradeTime);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(XH_HistoryTradeTableInfo model,Database db,DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_HistoryTradeTable(");
            strSql.Append("TradeNumber,EntrustNumber,EntrustPrice,TradePrice,PortfolioLogo,TradeAmount,StampTax,Commission,SpotCode,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,ClearingFee,StockAccount,CapitalAccount,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime)");

            strSql.Append(" values (");
            strSql.Append("@TradeNumber,@EntrustNumber,@EntrustPrice,@TradePrice,@PortfolioLogo,@TradeAmount,@StampTax,@Commission,@SpotCode,@TransferAccountFee,@TradeProceduresFee,@MonitoringFee,@TradingSystemUseFee,@ClearingFee,@StockAccount,@CapitalAccount,@TradeTypeId,@TradeUnitId,@BuySellTypeId,@CurrencyTypeId,@TradeTime)");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "StampTax", DbType.Decimal, model.StampTax);
            db.AddInParameter(dbCommand, "Commission", DbType.Decimal, model.Commission);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TransferAccountFee", DbType.Decimal, model.TransferAccountFee);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            db.AddInParameter(dbCommand, "TradingSystemUseFee", DbType.Decimal, model.TradingSystemUseFee);
            db.AddInParameter(dbCommand, "ClearingFee", DbType.Decimal, model.ClearingFee);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "TradeTypeId", DbType.Int32, model.TradeTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeTime", DbType.DateTime, model.TradeTime);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(XH_HistoryTradeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_HistoryTradeTable set ");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("TradePrice=@TradePrice,");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("StampTax=@StampTax,");
            strSql.Append("Commission=@Commission,");
            strSql.Append("SpotCode=@SpotCode,");
            strSql.Append("TransferAccountFee=@TransferAccountFee,");
            strSql.Append("TradeProceduresFee=@TradeProceduresFee,");
            strSql.Append("MonitoringFee=@MonitoringFee,");
            strSql.Append("TradingSystemUseFee=@TradingSystemUseFee,");
            strSql.Append("ClearingFee=@ClearingFee,");
            strSql.Append("StockAccount=@StockAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("TradeTypeId=@TradeTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("TradeTime=@TradeTime");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "StampTax", DbType.Decimal, model.StampTax);
            db.AddInParameter(dbCommand, "Commission", DbType.Decimal, model.Commission);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TransferAccountFee", DbType.Decimal, model.TransferAccountFee);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            db.AddInParameter(dbCommand, "TradingSystemUseFee", DbType.Decimal, model.TradingSystemUseFee);
            db.AddInParameter(dbCommand, "ClearingFee", DbType.Decimal, model.ClearingFee);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "TradeTypeId", DbType.Int32, model.TradeTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeTime", DbType.DateTime, model.TradeTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string TradeNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_HistoryTradeTable ");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, TradeNumber);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_HistoryTradeTableInfo GetModel(string TradeNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeNumber,EntrustNumber,EntrustPrice,TradePrice,PortfolioLogo,TradeAmount,StampTax,Commission,SpotCode,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,ClearingFee,StockAccount,CapitalAccount,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime from XH_HistoryTradeTable ");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, TradeNumber);
            XH_HistoryTradeTableInfo model = null;
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
        public List<XH_HistoryTradeTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeNumber,EntrustNumber,EntrustPrice,TradePrice,PortfolioLogo,TradeAmount,StampTax,Commission,SpotCode,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,ClearingFee,StockAccount,CapitalAccount,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ");
            strSql.Append(" FROM XH_HistoryTradeTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_HistoryTradeTableInfo> list = new List<XH_HistoryTradeTableInfo>();
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
        public XH_HistoryTradeTableInfo ReaderBind(IDataReader dataReader)
        {
            XH_HistoryTradeTableInfo model = new XH_HistoryTradeTableInfo();
            object ojb;
            model.TradeNumber = dataReader["TradeNumber"].ToString();
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            ojb = dataReader["EntrustPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustPrice = (decimal)ojb;
            }
            ojb = dataReader["TradePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradePrice = (decimal)ojb;
            }
            model.PortfolioLogo = dataReader["PortfolioLogo"].ToString();
            ojb = dataReader["TradeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeAmount = (int)ojb;
            }
            ojb = dataReader["StampTax"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StampTax = (decimal)ojb;
            }
            ojb = dataReader["Commission"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Commission = (decimal)ojb;
            }
            model.SpotCode = dataReader["SpotCode"].ToString();
            ojb = dataReader["TransferAccountFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TransferAccountFee = (decimal)ojb;
            }
            ojb = dataReader["TradeProceduresFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeProceduresFee = (decimal)ojb;
            }
            ojb = dataReader["MonitoringFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MonitoringFee = (decimal)ojb;
            }
            ojb = dataReader["TradingSystemUseFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradingSystemUseFee = (decimal)ojb;
            }
            ojb = dataReader["ClearingFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ClearingFee = (decimal)ojb;
            }
            model.StockAccount = dataReader["StockAccount"].ToString();
            model.CapitalAccount = dataReader["CapitalAccount"].ToString();
            ojb = dataReader["TradeTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeTypeId = (int)ojb;
            }
            ojb = dataReader["TradeUnitId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeUnitId = (int)ojb;
            }
            ojb = dataReader["BuySellTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BuySellTypeId = (int)ojb;
            }
            ojb = dataReader["CurrencyTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeId = (int)ojb;
            }
            ojb = dataReader["TradeTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeTime = (DateTime)ojb;
            }
            return model;
        }

        ///// <summary>
        ///// 根据条件分页查询
        ///// </summary>
        ///// <param name="pageProcInfo">分页存储过程过滤条件</param>
        ///// <param name="total">总页数</param>
        ///// <returns></returns>
        //public List<XH_HistoryTradeTableInfo> PagingXH_HistoryTradeByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<XH_HistoryTradeTableInfo> list = new List<XH_HistoryTradeTableInfo>();
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