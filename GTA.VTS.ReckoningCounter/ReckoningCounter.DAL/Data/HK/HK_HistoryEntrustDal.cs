using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.DAL.Data.HK
{
    /// <summary>
    /// 港股历史委托数据访问类HK_HistoryEntrustDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class HK_HistoryEntrustDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_HistoryEntrustDal()
        { }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// <param name="entrustNumber"></param>
        /// </summary>
        public bool Exists(string entrustNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_HistoryEntrust where EntrustNumber=@EntrustNumber ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
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
        /// <param name="model"></param>
        /// </summary>
        public void Add(HK_HistoryEntrustInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_HistoryEntrust(");
            strSql.Append("EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,Code,TradeAmount,TradeAveragePrice,OrderPriceType,CancelAmount,BuySellTypeID,HoldAccount,CapitalAccount,CurrencyTypeID,TradeUnitID,OrderStatusID,OrderMessage,McOrderID,HasDoneProfit,OfferTime,EntrustTime,IsModifyOrder,ModifyOrderNumber)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PortfolioLogo,@EntrustPrice,@EntrustMount,@Code,@TradeAmount,@TradeAveragePrice,@OrderPriceType,@CancelAmount,@BuySellTypeID,@HoldAccount,@CapitalAccount,@CurrencyTypeID,@TradeUnitID,@OrderStatusID,@OrderMessage,@McOrderID,@HasDoneProfit,@OfferTime,@EntrustTime,@IsModifyOrder,@ModifyOrderNumber)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustMount", DbType.Int32, model.EntrustMount);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "OrderPriceType", DbType.Int32, model.OrderPriceType);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "BuySellTypeID", DbType.Int32, model.BuySellTypeID);
            db.AddInParameter(dbCommand, "HoldAccount", DbType.AnsiString, model.HoldAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            db.AddInParameter(dbCommand, "TradeUnitID", DbType.Int32, model.TradeUnitID);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, model.OrderStatusID);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "McOrderID", DbType.AnsiString, model.McOrderID);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.AddInParameter(dbCommand, "IsModifyOrder", DbType.Boolean, model.IsModifyOrder);
            db.AddInParameter(dbCommand, "ModifyOrderNumber", DbType.AnsiString, model.ModifyOrderNumber);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// <param name="model"></param>
        /// </summary>
        public void Update(HK_HistoryEntrustInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_HistoryEntrust set ");
            strSql.Append("EntrustPrice=@EntrustPrice,");
            strSql.Append("PortfolioLogo=@PortfolioLogo,");
            strSql.Append("EntrustMount=@EntrustMount,");
            strSql.Append("Code=@Code,");
            strSql.Append("TradeAmount=@TradeAmount,");
            strSql.Append("TradeAveragePrice=@TradeAveragePrice,");
            strSql.Append("OrderPriceType=@OrderPriceType,");
            strSql.Append("CancelAmount=@CancelAmount,");
            strSql.Append("BuySellTypeID=@BuySellTypeID,");
            strSql.Append("HoldAccount=@HoldAccount,");
            strSql.Append("CapitalAccount=@CapitalAccount,");
            strSql.Append("CurrencyTypeID=@CurrencyTypeID,");
            strSql.Append("TradeUnitID=@TradeUnitID,");
            strSql.Append("OrderStatusID=@OrderStatusID,");
            strSql.Append("OrderMessage=@OrderMessage,");
            strSql.Append("McOrderID=@McOrderID,");
            strSql.Append("HasDoneProfit=@HasDoneProfit,");
            strSql.Append("OfferTime=@OfferTime,");
            strSql.Append("EntrustTime=@EntrustTime,");
            strSql.Append("IsModifyOrder=@IsModifyOrder,");
            strSql.Append("ModifyOrderNumber=@ModifyOrderNumber");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PortfolioLogo", DbType.AnsiString, model.PortfolioLogo);
            db.AddInParameter(dbCommand, "EntrustPrice", DbType.Decimal, model.EntrustPrice);
            db.AddInParameter(dbCommand, "EntrustMount", DbType.Int32, model.EntrustMount);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "TradeAmount", DbType.Int32, model.TradeAmount);
            db.AddInParameter(dbCommand, "TradeAveragePrice", DbType.Decimal, model.TradeAveragePrice);
            db.AddInParameter(dbCommand, "OrderPriceType", DbType.Int32, model.OrderPriceType);
            db.AddInParameter(dbCommand, "CancelAmount", DbType.Int32, model.CancelAmount);
            db.AddInParameter(dbCommand, "BuySellTypeID", DbType.Int32, model.BuySellTypeID);
            db.AddInParameter(dbCommand, "HoldAccount", DbType.AnsiString, model.HoldAccount);
            db.AddInParameter(dbCommand, "CapitalAccount", DbType.AnsiString, model.CapitalAccount);
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            db.AddInParameter(dbCommand, "TradeUnitID", DbType.Int32, model.TradeUnitID);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, model.OrderStatusID);
            db.AddInParameter(dbCommand, "OrderMessage", DbType.AnsiString, model.OrderMessage);
            db.AddInParameter(dbCommand, "McOrderID", DbType.AnsiString, model.McOrderID);
            db.AddInParameter(dbCommand, "HasDoneProfit", DbType.Decimal, model.HasDoneProfit);
            db.AddInParameter(dbCommand, "OfferTime", DbType.DateTime, model.OfferTime);
            db.AddInParameter(dbCommand, "EntrustTime", DbType.DateTime, model.EntrustTime);
            db.AddInParameter(dbCommand, "IsModifyOrder", DbType.Boolean, model.IsModifyOrder);
            db.AddInParameter(dbCommand, "ModifyOrderNumber", DbType.AnsiString, model.ModifyOrderNumber);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// <param name="entrustNumber"></param>
        /// </summary>
        public void Delete(string entrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_HistoryEntrust ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// <param name="entrustNumber"></param>
        /// </summary>
        public HK_HistoryEntrustInfo GetModel(string entrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,Code,TradeAmount,TradeAveragePrice,OrderPriceType,CancelAmount,BuySellTypeID,HoldAccount,CapitalAccount,CurrencyTypeID,TradeUnitID,OrderStatusID,OrderMessage,McOrderID,HasDoneProfit,OfferTime,EntrustTime,IsModifyOrder,ModifyOrderNumber from HK_HistoryEntrust ");
            strSql.Append(" where EntrustNumber=@EntrustNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            HK_HistoryEntrustInfo model = null;
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
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,Code,TradeAmount,TradeAveragePrice,OrderPriceType,CancelAmount,BuySellTypeID,HoldAccount,CapitalAccount,CurrencyTypeID,TradeUnitID,OrderStatusID,OrderMessage,McOrderID,HasDoneProfit,OfferTime,EntrustTime,IsModifyOrder,ModifyOrderNumber ");
            strSql.Append(" FROM HK_HistoryEntrust ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }
     
        /// <summary>
        /// 获得数据列表
        /// <param name="strWhere"></param>
        /// </summary>
        public List<HK_HistoryEntrustInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EntrustNumber,PortfolioLogo,EntrustPrice,EntrustMount,Code,TradeAmount,TradeAveragePrice,OrderPriceType,CancelAmount,BuySellTypeID,HoldAccount,CapitalAccount,CurrencyTypeID,TradeUnitID,OrderStatusID,OrderMessage,McOrderID,HasDoneProfit,OfferTime,EntrustTime,IsModifyOrder,ModifyOrderNumber ");
            strSql.Append(" FROM HK_HistoryEntrust ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HK_HistoryEntrustInfo> list = new List<HK_HistoryEntrustInfo>();
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
        /// <param name="dataReader"></param>
        /// </summary>
        public HK_HistoryEntrustInfo ReaderBind(IDataReader dataReader)
        {
            HK_HistoryEntrustInfo model = new HK_HistoryEntrustInfo();
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
            model.Code = dataReader["Code"].ToString();
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
            ojb = dataReader["OrderPriceType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderPriceType = (int)ojb;
            }
            ojb = dataReader["CancelAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CancelAmount = (int)ojb;
            }
            ojb = dataReader["BuySellTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BuySellTypeID = (int)ojb;
            }
            model.HoldAccount = dataReader["HoldAccount"].ToString();
            model.CapitalAccount = dataReader["CapitalAccount"].ToString();
            ojb = dataReader["CurrencyTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeID = (int)ojb;
            }
            ojb = dataReader["TradeUnitID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeUnitID = (int)ojb;
            }
            ojb = dataReader["OrderStatusID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderStatusID = (int)ojb;
            }
            model.OrderMessage = dataReader["OrderMessage"].ToString();
            model.McOrderID = dataReader["McOrderID"].ToString();
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
            ojb = dataReader["IsModifyOrder"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsModifyOrder = (bool)ojb;
            }
            model.ModifyOrderNumber = dataReader["ModifyOrderNumber"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}
