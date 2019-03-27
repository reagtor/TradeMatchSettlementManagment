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
    /// 数据访问类XH_TodayEntrustTableDal。
    /// </summary>
    public class XH_TodayEntrustTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string EntrustNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_TodayEntrustTable where EntrustNumber=@EntrustNumber ");
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
        /// 获得数据列表
        /// </summary>
        public List<XH_TodayEntrustTableInfo> GetListArrayWithNoLock(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime ");
            strSql.Append(" FROM XH_TodayEntrustTable WITH (NOLOCK)");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_TodayEntrustTableInfo> list = new List<XH_TodayEntrustTableInfo>();
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
        /// 增加一条数据
        /// </summary>
        public void Add(XH_TodayEntrustTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_TodayEntrustTable(");
            strSql.Append("EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PortfolioLogo,@EntrustPrice,@EntrustAmount,@SpotCode,@TradeAmount,@TradeAveragePrice,@CancelAmount,@CancelLogo,@BuySellTypeId,@StockAccount,@CapitalAccount,@OrderStatusId,@IsMarketValue,@OrderMessage,@CurrencyTypeId,@TradeUnitId,@CallbackChannlId,@McOrderId,@HasDoneProfit,@OfferTime,@EntrustTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(XH_TodayEntrustTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_TodayEntrustTable(");
            strSql.Append("EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PortfolioLogo,@EntrustPrice,@EntrustAmount,@SpotCode,@TradeAmount,@TradeAveragePrice,@CancelAmount,@CancelLogo,@BuySellTypeId,@StockAccount,@CapitalAccount,@OrderStatusId,@IsMarketValue,@OrderMessage,@CurrencyTypeId,@TradeUnitId,@CallbackChannlId,@McOrderId,@HasDoneProfit,@OfferTime,@EntrustTime)");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddRecord(XH_TodayEntrustTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_TodayEntrustTable(");
            strSql.Append("EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PortfolioLogo,@EntrustPrice,@EntrustAmount,@SpotCode,@TradeAmount,@TradeAveragePrice,@CancelAmount,@CancelLogo,@BuySellTypeId,@StockAccount,@CapitalAccount,@OrderStatusId,@IsMarketValue,@OrderMessage,@CurrencyTypeId,@TradeUnitId,@CallbackChannlId,@McOrderId,@HasDoneProfit,@OfferTime,@EntrustTime)");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);

            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            if (db.ExecuteNonQuery(dbCommand, transaction) != -1)
                return true;
            return false;
        }



        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(XH_TodayEntrustTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_TodayEntrustTable set ");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("EntrustAmount=@EntrustAmount,");
            strSql.Append("SpotCode=@SpotCode,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("CancelLogo=@CancelLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("StockAccount=@StockAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("OrderStatusId=@OrderStatusId,");
            strSql.Append("IsMarketValue=@IsMarketValue,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("CallbackChannlId=@CallbackChannlId,");
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
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 更新OrderMessage
        /// </summary>
        public void UpdateOrderMessage(string entrustNumber, string orderMessage)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_TodayEntrustTable set ");
            strSql.Append("OrderMessage=@OrderMessage");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, orderMessage);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_TodayEntrustTableInfo GetModelWithNoLock(string EntrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime from XH_TodayEntrustTable WITH (NOLOCK) ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            XH_TodayEntrustTableInfo model = null;
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
        /// 更新一条数据
        /// </summary>
        public void Update(XH_TodayEntrustTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_TodayEntrustTable set ");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("EntrustAmount=@EntrustAmount,");
            strSql.Append("SpotCode=@SpotCode,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("CancelLogo=@CancelLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("StockAccount=@StockAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("OrderStatusId=@OrderStatusId,");
            strSql.Append("IsMarketValue=@IsMarketValue,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("CallbackChannlId=@CallbackChannlId,");
            strSql.Append("McOrderId=@McOrderId,");
            strSql.Append("HasDoneProfit=@HasDoneProfit,");
            strSql.Append("OfferTime=@OfferTime,");
            strSql.Append("EntrustTime=@EntrustTime");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 根据委托单列表和要更新的通道号更新回推通道
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="newClientID">通道号</param>
        /// <param name="db">数据库</param>
        /// <param name="transaction">事务</param>
        public void UpdateChannel(string entrustNumber, string newClientID, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_TodayEntrustTable set ");
            strSql.AppendFormat("CallbackChannlId='{0}'", newClientID);
            strSql.AppendFormat(" where EntrustNumber in( {0} ) ", entrustNumber);
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            //db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            //db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, newClientID);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpRecord(XH_TodayEntrustTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_TodayEntrustTable set ");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("EntrustAmount=@EntrustAmount,");
            strSql.Append("SpotCode=@SpotCode,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("CancelLogo=@CancelLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("StockAccount=@StockAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("OrderStatusId=@OrderStatusId,");
            strSql.Append("IsMarketValue=@IsMarketValue,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("CallbackChannlId=@CallbackChannlId,");
            strSql.Append("McOrderId=@McOrderId,");
            strSql.Append("HasDoneProfit=@HasDoneProfit,");
            strSql.Append("OfferTime=@OfferTime,");
            strSql.Append("EntrustTime=@EntrustTime");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "SpotCode", DbType.AnsiString, model.SpotCode);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "StockAccount", DbType.AnsiString, model.StockAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            if (db.ExecuteNonQuery(dbCommand, transaction) != -1)
                return true;
            return false;
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string EntrustNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_TodayEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_TodayEntrustTableInfo GetModel(string EntrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime from XH_TodayEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            XH_TodayEntrustTableInfo model = null;
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
        /// 得到一个对象实体
        /// </summary>
        public XH_TodayEntrustTableInfo GetModel(string EntrustNumber, ReckoningTransaction tm)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime from XH_TodayEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            XH_TodayEntrustTableInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand, tm.Transaction))
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
        public List<XH_TodayEntrustTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,SpotCode,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,StockAccount,CapitalAccount,OrderStatusId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,CallbackChannlId,McOrderId,HasDoneProfit,OfferTime,EntrustTime ");
            strSql.Append(" FROM XH_TodayEntrustTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_TodayEntrustTableInfo> list = new List<XH_TodayEntrustTableInfo>();
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
        /// 获得数据列表 
        /// </summary>
        public List<XH_TodayEntrustTableInfo> GetPushList(string channelID)
        {
            string where = string.Format("select Z.* from XH_TodayEntrustTable Z where Z.EntrustNumber IN (SELECT X.EntrustNumber FROM XH_TodayTradeTable X ,XH_PushBackOrderTable Y WHERE  X.TradeNumber = Y.TradeNumber and y.ChannelID = '{0}')", channelID);
            List<XH_TodayEntrustTableInfo> list = new List<XH_TodayEntrustTableInfo>();
            Database db = DatabaseFactory.CreateDatabase();
            using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, where.ToString()))
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
        public XH_TodayEntrustTableInfo ReaderBind(IDataReader dataReader)
        {
            XH_TodayEntrustTableInfo model = new XH_TodayEntrustTableInfo();
            object ojb;
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            model.PortfolioLogo = dataReader["PortfolioLogo"].ToString();
            ojb = dataReader["EntrustPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustPrice = (decimal)ojb;
            }
            ojb = dataReader["EntrustAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntrustAmount = (int)ojb;
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
            ojb = dataReader["CancelAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CancelAmount = (int)ojb;
            }
            ojb = dataReader["CancelLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CancelLogo = (bool)ojb;
            }
            ojb = dataReader["BuySellTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BuySellTypeId = (int)ojb;
            }
            model.StockAccount = dataReader["StockAccount"].ToString();
            model.CapitalAccount = dataReader["CapitalAccount"].ToString();
            ojb = dataReader["OrderStatusId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderStatusId = (int)ojb;
            }
            ojb = dataReader["IsMarketValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsMarketValue = (bool)ojb;
            }
            model.OrderMessage = dataReader["OrderMessage"].ToString();
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
            model.CallbackChannlId = dataReader["CallbackChannlId"].ToString();
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
        //public List<XH_TodayEntrustTableInfo> PagingXH_TodayEntrustByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<XH_TodayEntrustTableInfo> list = new List<XH_TodayEntrustTableInfo>();
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