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
    /// 数据访问类QH_HistoryTradeTableDal。
    /// </summary>
    public class QH_HistoryTradeTableDal
    {
        #region  成员方法


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string TradeNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_HistoryTradeTable where TradeNumber=@TradeNumber ");
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
        public void Add(QH_HistoryTradeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_HistoryTradeTable(");
            strSql.Append("TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,EntrustPrice,TradeAmount,TradeProceduresFee,Margin,ContractCode,TradeAccount,CapitalAccount,BuySellTypeId,OpenCloseTypeId,TradeUnitId,TradeTypeId,CurrencyTypeId,TradeTime,MarketProfitLoss)");

            strSql.Append(" values (");
            strSql.Append("@TradeNumber,@EntrustNumber,@PortfolioLogo,@TradePrice,@EntrustPrice,@TradeAmount,@TradeProceduresFee,@Margin,@ContractCode,@TradeAccount,@CapitalAccount,@BuySellTypeId,@OpenCloseTypeId,@TradeUnitId,@TradeTypeId,@CurrencyTypeId,@TradeTime,@MarketProfitLoss)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "ContractCode", DbType.AnsiString, model.ContractCode);
            db.AddInParameter(dbCommand, "TradeAccount", DbType.AnsiString, model.TradeAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "OpenCloseTypeId", DbType.Int32, model.OpenCloseTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "TradeTypeId", DbType.Int32, model.TradeTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeTime", DbType.DateTime, model.TradeTime);
            db.AddInParameter(dbCommand, "MarketProfitLoss", DbType.Decimal, model.MarketProfitLoss);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_HistoryTradeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HistoryTradeTable set ");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("TradePrice=@TradePrice,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeProceduresFee=@TradeProceduresFee,");
            strSql.Append("Margin=@Margin,");
            strSql.Append("ContractCode=@ContractCode,");
            strSql.Append("TradeAccount=@TradeAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("OpenCloseTypeId=@OpenCloseTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("TradeTypeId=@TradeTypeId,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("TradeTime=@TradeTime,");
            strSql.Append("MarketProfitLoss=@MarketProfitLoss");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "ContractCode", DbType.AnsiString, model.ContractCode);
            db.AddInParameter(dbCommand, "TradeAccount", DbType.AnsiString, model.TradeAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "OpenCloseTypeId", DbType.Int32, model.OpenCloseTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "TradeTypeId", DbType.Int32, model.TradeTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeTime", DbType.DateTime, model.TradeTime);
            db.AddInParameter(dbCommand, "MarketProfitLoss", DbType.Decimal, model.MarketProfitLoss);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string TradeNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_HistoryTradeTable ");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, TradeNumber);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_HistoryTradeTableInfo GetModel(string TradeNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,EntrustPrice,TradeAmount,TradeProceduresFee,Margin,ContractCode,TradeAccount,CapitalAccount,BuySellTypeId,OpenCloseTypeId,TradeUnitId,TradeTypeId,CurrencyTypeId,TradeTime,MarketProfitLoss from QH_HistoryTradeTable ");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, TradeNumber);
            QH_HistoryTradeTableInfo model = null;
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
        public List<QH_HistoryTradeTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,EntrustPrice,TradeAmount,TradeProceduresFee,Margin,ContractCode,TradeAccount,CapitalAccount,BuySellTypeId,OpenCloseTypeId,TradeUnitId,TradeTypeId,CurrencyTypeId,TradeTime,MarketProfitLoss ");
            strSql.Append(" FROM QH_HistoryTradeTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_HistoryTradeTableInfo> list = new List<QH_HistoryTradeTableInfo>();
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
        public QH_HistoryTradeTableInfo ReaderBind(IDataReader dataReader)
        {
            QH_HistoryTradeTableInfo model = new QH_HistoryTradeTableInfo();
            object ojb;
            model.TradeNumber = dataReader["TradeNumber"].ToString();
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            model.PortfolioLogo = dataReader["PortfolioLogo"].ToString();
            ojb = dataReader["TradePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradePrice = (decimal)ojb;
            }
            ojb = dataReader["EntrustPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustPrice = (decimal)ojb;
            }
            ojb = dataReader["TradeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeAmount = (int)ojb;
            }
            ojb = dataReader["TradeProceduresFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeProceduresFee = (decimal)ojb;
            }
            ojb = dataReader["Margin"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Margin = (decimal)ojb;
            }
            model.ContractCode = dataReader["ContractCode"].ToString();
            model.TradeAccount = dataReader["TradeAccount"].ToString();
            model.CapitalAccount = dataReader["CapitalAccount"].ToString();
            ojb = dataReader["BuySellTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BuySellTypeId = (int)ojb;
            }
            ojb = dataReader["OpenCloseTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OpenCloseTypeId = (int)ojb;
            }
            ojb = dataReader["TradeUnitId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeUnitId = (int)ojb;
            }
            ojb = dataReader["TradeTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeTypeId = (int)ojb;
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
            ojb = dataReader["MarketProfitLoss"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketProfitLoss = (decimal)ojb;
            }
            return model;
        }

        ///// <summary>
        ///// 根据条件分页查询
        ///// </summary>
        ///// <param name="pageProcInfo">分页存储过程过滤条件</param>
        ///// <param name="total">总页数</param>
        ///// <returns></returns>
        //public List<QH_HistoryTradeTableInfo> PagingQH_HistoryTradeByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<QH_HistoryTradeTableInfo> list = new List<QH_HistoryTradeTableInfo>();
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