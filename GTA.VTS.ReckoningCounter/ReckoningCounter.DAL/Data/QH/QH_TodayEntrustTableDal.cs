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
    /// 数据访问类QH_TodayEntrustTableDal。
    /// </summary>
    public class QH_TodayEntrustTableDal
    {
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string EntrustNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_TodayEntrustTable where EntrustNumber=@EntrustNumber ");
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
        /// 得到一个对象实体
        /// </summary>
        public QH_TodayEntrustTableInfo GetModelWithNoLock(string EntrustNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select EntrustNumber,EntrustTime,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAccount,OfferTime,OwnershipMarket,ContractCode,ContractName,FreezeCapitalAmount,OwnershipMarketId,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,VarietyType,BuySellId,CapitalAccount,OpenCloseId,VarietyTypeID,OrderStatusId,CallbackChannelId,IsMarketValue,OrderMessage,CurrencyTypeId,TradeUnitId,OrderStatusMessage,McOrderId from QH_TodayEntrustTable WITH (NOLOCK)");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            QH_TodayEntrustTableInfo model = null;
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
        /// 增加一条数据
        /// </summary>
        public void Add(QH_TodayEntrustTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_TodayEntrustTable(");
            strSql.Append("EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PortfolioLogo,@EntrustPrice,@EntrustAmount,@TradeAmount,@TradeAveragePrice,@CancelAmount,@CancelLogo,@BuySellTypeId,@CurrencyTypeId,@OpenCloseTypeId,@TradeUnitId,@OrderStatusId,@ContractCode,@TradeAccount,@CapitalAccount,@IsMarketValue,@OrderMessage,@CallbackChannelId,@McOrderId,@CloseFloatProfitLoss,@CloseMarketProfitLoss,@OfferTime,@EntrustTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "OpenCloseTypeId", DbType.Int32, model.OpenCloseTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "ContractCode", DbType.AnsiString, model.ContractCode);
            db.AddInParameter(dbCommand, "TradeAccount", DbType.AnsiString, model.TradeAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CallbackChannelId", DbType.AnsiString, model.CallbackChannelId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "CloseFloatProfitLoss", DbType.Decimal, model.CloseFloatProfitLoss);
            db.AddInParameter(dbCommand, "CloseMarketProfitLoss", DbType.Decimal, model.CloseMarketProfitLoss);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(QH_TodayEntrustTableInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_TodayEntrustTable(");
            strSql.Append("EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PortfolioLogo,@EntrustPrice,@EntrustAmount,@TradeAmount,@TradeAveragePrice,@CancelAmount,@CancelLogo,@BuySellTypeId,@CurrencyTypeId,@OpenCloseTypeId,@TradeUnitId,@OrderStatusId,@ContractCode,@TradeAccount,@CapitalAccount,@IsMarketValue,@OrderMessage,@CallbackChannelId,@McOrderId,@CloseFloatProfitLoss,@CloseMarketProfitLoss,@OfferTime,@EntrustTime)");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "OpenCloseTypeId", DbType.Int32, model.OpenCloseTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "ContractCode", DbType.AnsiString, model.ContractCode);
            db.AddInParameter(dbCommand, "TradeAccount", DbType.AnsiString, model.TradeAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CallbackChannelId", DbType.AnsiString, model.CallbackChannelId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "CloseFloatProfitLoss", DbType.Decimal, model.CloseFloatProfitLoss);
            db.AddInParameter(dbCommand, "CloseMarketProfitLoss", DbType.Decimal, model.CloseMarketProfitLoss);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            try
            {
                db.ExecuteNonQuery(dbCommand, tm.Transaction);
                return true;
            }
            catch
            {

                return false;
            }

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_TodayEntrustTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_TodayEntrustTable set ");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("EntrustAmount=@EntrustAmount,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("CancelLogo=@CancelLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("OpenCloseTypeId=@OpenCloseTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("OrderStatusId=@OrderStatusId,");
            strSql.Append("ContractCode=@ContractCode,");
            strSql.Append("TradeAccount=@TradeAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("IsMarketValue=@IsMarketValue,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("CallbackChannelId=@CallbackChannelId,");
            strSql.Append("McOrderId=@McOrderId,");
            strSql.Append("CloseFloatProfitLoss=@CloseFloatProfitLoss,");
            strSql.Append("CloseMarketProfitLoss=@CloseMarketProfitLoss,");
            strSql.Append("OfferTime=@OfferTime,");
            strSql.Append("EntrustTime=@EntrustTime");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "OpenCloseTypeId", DbType.Int32, model.OpenCloseTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "ContractCode", DbType.AnsiString, model.ContractCode);
            db.AddInParameter(dbCommand, "TradeAccount", DbType.AnsiString, model.TradeAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CallbackChannelId", DbType.AnsiString, model.CallbackChannelId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "CloseFloatProfitLoss", DbType.Decimal, model.CloseFloatProfitLoss);
            db.AddInParameter(dbCommand, "CloseMarketProfitLoss", DbType.Decimal, model.CloseMarketProfitLoss);
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
            strSql.Append("update QH_TodayEntrustTable set ");
            strSql.Append("OrderMessage=@OrderMessage");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, orderMessage);
            db.ExecuteNonQuery(dbCommand);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateRecord(QH_TodayEntrustTableInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_TodayEntrustTable set ");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("EntrustAmount=@EntrustAmount,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("CancelLogo=@CancelLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("OpenCloseTypeId=@OpenCloseTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("OrderStatusId=@OrderStatusId,");
            strSql.Append("ContractCode=@ContractCode,");
            strSql.Append("TradeAccount=@TradeAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("IsMarketValue=@IsMarketValue,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("CallbackChannelId=@CallbackChannelId,");
            strSql.Append("McOrderId=@McOrderId,");
            strSql.Append("CloseFloatProfitLoss=@CloseFloatProfitLoss,");
            strSql.Append("CloseMarketProfitLoss=@CloseMarketProfitLoss,");
            strSql.Append("OfferTime=@OfferTime,");
            strSql.Append("EntrustTime=@EntrustTime");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "OpenCloseTypeId", DbType.Int32, model.OpenCloseTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "ContractCode", DbType.AnsiString, model.ContractCode);
            db.AddInParameter(dbCommand, "TradeAccount", DbType.AnsiString, model.TradeAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CallbackChannelId", DbType.AnsiString, model.CallbackChannelId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "CloseFloatProfitLoss", DbType.Decimal, model.CloseFloatProfitLoss);
            db.AddInParameter(dbCommand, "CloseMarketProfitLoss", DbType.Decimal, model.CloseMarketProfitLoss);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.ExecuteNonQuery(dbCommand, tm.Transaction);



        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(QH_TodayEntrustTableInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_TodayEntrustTable set ");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("EntrustAmount=@EntrustAmount,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("CancelLogo=@CancelLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("OpenCloseTypeId=@OpenCloseTypeId,");
            strSql.Append("TradeUnitId=@TradeUnitId,");
            strSql.Append("OrderStatusId=@OrderStatusId,");
            strSql.Append("ContractCode=@ContractCode,");
            strSql.Append("TradeAccount=@TradeAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("IsMarketValue=@IsMarketValue,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("CallbackChannelId=@CallbackChannelId,");
            strSql.Append("McOrderId=@McOrderId,");
            strSql.Append("CloseFloatProfitLoss=@CloseFloatProfitLoss,");
            strSql.Append("CloseMarketProfitLoss=@CloseMarketProfitLoss,");
            strSql.Append("OfferTime=@OfferTime,");
            strSql.Append("EntrustTime=@EntrustTime");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustAmount", DbType.Int32, model.EntrustAmount);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "CancelLogo", DbType.Boolean, model.CancelLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "OpenCloseTypeId", DbType.Int32, model.OpenCloseTypeId);
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "ContractCode", DbType.AnsiString, model.ContractCode);
            db.AddInParameter(dbCommand, "TradeAccount", DbType.AnsiString, model.TradeAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "IsMarketValue", DbType.Boolean, model.IsMarketValue);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "CallbackChannelId", DbType.AnsiString, model.CallbackChannelId);
            db.AddInParameter(dbCommand, "McOrderId", DbType.AnsiString, model.McOrderId);
            db.AddInParameter(dbCommand, "CloseFloatProfitLoss", DbType.Decimal, model.CloseFloatProfitLoss);
            db.AddInParameter(dbCommand, "CloseMarketProfitLoss", DbType.Decimal, model.CloseMarketProfitLoss);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            if (db.ExecuteNonQuery(dbCommand, tm.Transaction) != -1)
                return true;
            return false;


        }


        /// <summary>
        /// 根据委托单列表和要更新的通道号更新回推通道
        /// </summary>
        /// <param name="entrustNumber">委托编号</param>
        /// <param name="newClientID">通道号</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        public void UpdateChannel(string entrustNumber, string newClientID, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_TodayEntrustTable set ");
            strSql.AppendFormat("CallbackChannelId='{0}' ", newClientID);
            strSql.AppendFormat(" where EntrustNumber in( {0} )", entrustNumber);
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            //db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            //db.AddInParameter(dbCommand, "CallbackChannelId", DbType.AnsiString, newClientID);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string EntrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_TodayEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_TodayEntrustTableInfo GetModel(string EntrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime from QH_TodayEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            QH_TodayEntrustTableInfo model = null;
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
        public QH_TodayEntrustTableInfo GetModel(string EntrustNumber, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime from QH_TodayEntrustTable ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, EntrustNumber);
            QH_TodayEntrustTableInfo model = null;
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
        public List<QH_TodayEntrustTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime ");
            strSql.Append(" FROM QH_TodayEntrustTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_TodayEntrustTableInfo> list = new List<QH_TodayEntrustTableInfo>();
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
        public List<QH_TodayEntrustTableInfo> GetListArrayWithNoLock(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustAmount,TradeAmount,TradeAveragePrice,CancelAmount,CancelLogo,BuySellTypeId,CurrencyTypeId,OpenCloseTypeId,TradeUnitId,OrderStatusId,ContractCode,TradeAccount,CapitalAccount,IsMarketValue,OrderMessage,CallbackChannelId,McOrderId,CloseFloatProfitLoss,CloseMarketProfitLoss,OfferTime,EntrustTime ");
            strSql.Append(" FROM QH_TodayEntrustTable WITH (NOLOCK)");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_TodayEntrustTableInfo> list = new List<QH_TodayEntrustTableInfo>();
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
        public List<QH_TodayEntrustTableInfo> GetPushList(string channelID)
        {
            string where = string.Format("select Z.* from QH_TodayEntrustTable Z where Z.EntrustNumber IN (SELECT X.EntrustNumber FROM QH_TodayTradeTable X ,QH_PushBackOrderTable Y WHERE  X.TradeNumber = Y.TradeNumber and y.ChannelID = '{0}')", channelID);
            List<QH_TodayEntrustTableInfo> list = new List<QH_TodayEntrustTableInfo>();
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
        public QH_TodayEntrustTableInfo ReaderBind(IDataReader dataReader)
        {
            QH_TodayEntrustTableInfo model = new QH_TodayEntrustTableInfo();
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
            ojb = dataReader["CurrencyTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeId = (int)ojb;
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
            ojb = dataReader["OrderStatusId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderStatusId = (int)ojb;
            }
            model.ContractCode = dataReader["ContractCode"].ToString();
            model.TradeAccount = dataReader["TradeAccount"].ToString();
            model.CapitalAccount = dataReader["CapitalAccount"].ToString();
            ojb = dataReader["IsMarketValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsMarketValue = (bool)ojb;
            }
            model.OrderMessage = dataReader["OrderMessage"].ToString();
            model.CallbackChannelId = dataReader["CallbackChannelId"].ToString();
            model.McOrderId = dataReader["McOrderId"].ToString();
            ojb = dataReader["CloseFloatProfitLoss"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseFloatProfitLoss = (decimal)ojb;
            }
            ojb = dataReader["CloseMarketProfitLoss"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseMarketProfitLoss = (decimal)ojb;
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
        //public List<QH_TodayEntrustTableInfo> PagingQH_TodayEntrustByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<QH_TodayEntrustTableInfo> list = new List<QH_TodayEntrustTableInfo>();
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