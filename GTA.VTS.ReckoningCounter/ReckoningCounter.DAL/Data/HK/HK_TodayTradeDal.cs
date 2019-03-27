using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity.Model.HK;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace ReckoningCounter.DAL.Data.HK
{
    /// <summary>
    /// 港股今日成交数据访问类HK_TodayTradeDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class HK_TodayTradeDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_TodayTradeDal()
        { }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        ///<param name="tradeNumber"></param>
        /// </summary>
        public bool Exists(string tradeNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_TodayTrade where TradeNumber=@TradeNumber ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, tradeNumber);
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
        ///<param name="model"></param>
        /// </summary>
        public void Add(HK_TodayTradeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_TodayTrade(");
            strSql.Append("TradeNumber,PortfolioLogo,EntrustNumber,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,HoldAccount,CapitalAccount,Code,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime)");

            strSql.Append(" values (");
            strSql.Append("@TradeNumber,@PortfolioLogo,@EntrustNumber,@TradePrice,@TradeAmount,@EntrustPrice,@StampTax,@Commission,@TransferAccountFee,@TradeProceduresFee,@MonitoringFee,@TradingSystemUseFee,@TradeCapitalAmount,@ClearingFee,@HoldAccount,@CapitalAccount,@Code,@TradeTypeId,@TradeUnitId,@BuySellTypeId,@CurrencyTypeId,@TradeTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "StampTax", DbType.Decimal, model.StampTax);
            db.AddInParameter(dbCommand, "Commission", DbType.Decimal, model.Commission);
            db.AddInParameter(dbCommand, "TransferAccountFee", DbType.Decimal, model.TransferAccountFee);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            db.AddInParameter(dbCommand, "TradingSystemUseFee", DbType.Decimal, model.TradingSystemUseFee);
            db.AddInParameter(dbCommand, "TradeCapitalAmount", DbType.Decimal, model.TradeCapitalAmount);
            db.AddInParameter(dbCommand, "ClearingFee", DbType.Decimal, model.ClearingFee);
            db.AddInParameter(dbCommand, "HoldAccount", DbType.AnsiString, model.HoldAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
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
        public void Add(HK_TodayTradeInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_TodayTrade(");
            strSql.Append("TradeNumber,PortfolioLogo,EntrustNumber,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,HoldAccount,CapitalAccount,Code,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime)");

            strSql.Append(" values (");
            strSql.Append("@TradeNumber,@PortfolioLogo,@EntrustNumber,@TradePrice,@TradeAmount,@EntrustPrice,@StampTax,@Commission,@TransferAccountFee,@TradeProceduresFee,@MonitoringFee,@TradingSystemUseFee,@TradeCapitalAmount,@ClearingFee,@HoldAccount,@CapitalAccount,@Code,@TradeTypeId,@TradeUnitId,@BuySellTypeId,@CurrencyTypeId,@TradeTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "StampTax", DbType.Decimal, model.StampTax);
            db.AddInParameter(dbCommand, "Commission", DbType.Decimal, model.Commission);
            db.AddInParameter(dbCommand, "TransferAccountFee", DbType.Decimal, model.TransferAccountFee);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            db.AddInParameter(dbCommand, "TradingSystemUseFee", DbType.Decimal, model.TradingSystemUseFee);
            db.AddInParameter(dbCommand, "TradeCapitalAmount", DbType.Decimal, model.TradeCapitalAmount);
            db.AddInParameter(dbCommand, "ClearingFee", DbType.Decimal, model.ClearingFee);
            db.AddInParameter(dbCommand, "HoldAccount", DbType.AnsiString, model.HoldAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "TradeTypeId", DbType.Int32, model.TradeTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeTime", DbType.DateTime, model.TradeTime);
            db.ExecuteNonQuery(dbCommand,tm.Transaction);
        }
        /// <summary>
        /// 更新一条数据
        ///<param name="model"></param>
        /// </summary>
        public void Update(HK_TodayTradeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_TodayTrade set ");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("TradePrice=@TradePrice,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("StampTax=@StampTax,");
            strSql.Append("Commission=@Commission,");
            strSql.Append("TransferAccountFee=@TransferAccountFee,");
            strSql.Append("TradeProceduresFee=@TradeProceduresFee,");
            strSql.Append("MonitoringFee=@MonitoringFee,");
            strSql.Append("TradingSystemUseFee=@TradingSystemUseFee,");
            strSql.Append("TradeCapitalAmount=@TradeCapitalAmount,");
            strSql.Append("ClearingFee=@ClearingFee,");
            strSql.Append("HoldAccount=@HoldAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("Code=@Code,");
            strSql.Append("TradeTypeId=@TradeTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("TradeTime=@TradeTime");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, model.TradeNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "TradePrice", DbType.Decimal, model.TradePrice);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "StampTax", DbType.Decimal, model.StampTax);
            db.AddInParameter(dbCommand, "Commission", DbType.Decimal, model.Commission);
            db.AddInParameter(dbCommand, "TransferAccountFee", DbType.Decimal, model.TransferAccountFee);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            db.AddInParameter(dbCommand, "TradingSystemUseFee", DbType.Decimal, model.TradingSystemUseFee);
            db.AddInParameter(dbCommand, "TradeCapitalAmount", DbType.Decimal, model.TradeCapitalAmount);
            db.AddInParameter(dbCommand, "ClearingFee", DbType.Decimal, model.ClearingFee);
            db.AddInParameter(dbCommand, "HoldAccount", DbType.AnsiString, model.HoldAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "TradeTypeId", DbType.Int32, model.TradeTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeTime", DbType.DateTime, model.TradeTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        ///<param name="tradeNumber"></param>
        /// </summary>
        public void Delete(string tradeNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_TodayTrade ");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, tradeNumber);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        ///<param name="tradeNumber"></param>
        /// </summary>
        public HK_TodayTradeInfo GetModel(string tradeNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeNumber,PortfolioLogo,EntrustNumber,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,HoldAccount,CapitalAccount,Code,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime from HK_TodayTrade ");
            strSql.Append(" where TradeNumber=@TradeNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeNumber", DbType.AnsiString, tradeNumber);
            HK_TodayTradeInfo model = null;
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
        ///<param name="strWhere"></param>
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeNumber,PortfolioLogo,EntrustNumber,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,HoldAccount,CapitalAccount,Code,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ");
            strSql.Append(" FROM HK_TodayTrade ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        ///<param name="strWhere"></param>
        /// </summary>
        public List<HK_TodayTradeInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeNumber,PortfolioLogo,EntrustNumber,TradePrice,TradeAmount,EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee,MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,HoldAccount,CapitalAccount,Code,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ");
            strSql.Append(" FROM HK_TodayTrade ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HK_TodayTradeInfo> list = new List<HK_TodayTradeInfo>();
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
        ///<param name="dataReader"></param>
        /// </summary>
        public HK_TodayTradeInfo ReaderBind(IDataReader dataReader)
        {
            HK_TodayTradeInfo model = new HK_TodayTradeInfo();
            object ojb;
            model.TradeNumber = dataReader["TradeNumber"].ToString();
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            model.PortfolioLogo = dataReader["PortfolioLogo"].ToString();
            ojb = dataReader["TradePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradePrice = (decimal)ojb;
            }
            ojb = dataReader["TradeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeAmount = (int)ojb;
            }
            ojb = dataReader["EntrustPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustPrice = (decimal)ojb;
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
            ojb = dataReader["TradeCapitalAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCapitalAmount = (decimal)ojb;
            }
            ojb = dataReader["ClearingFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ClearingFee = (decimal)ojb;
            }
            model.HoldAccount = dataReader["HoldAccount"].ToString();
            model.CapitalAccount = dataReader["CapitalAccount"].ToString();
            model.Code = dataReader["Code"].ToString();
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

        #endregion  成员方法
    }
}
