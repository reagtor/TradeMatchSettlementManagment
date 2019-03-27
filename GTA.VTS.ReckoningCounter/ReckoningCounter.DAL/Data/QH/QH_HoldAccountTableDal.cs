#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.QueryFilter;

#endregion

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类QH_HoldAccountTableDal。
    /// </summary>
    public class QH_HoldAccountTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int AccountHoldLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_HoldAccountTable where AccountHoldLogoId=@AccountHoldLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, AccountHoldLogoId);
            int cmdresult;
            object obj = db.ExecuteScalar(dbCommand);
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
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
        public  QH_HoldAccountTableInfo GetQhAccountHoldTableWithNoLock(string strHoldAccount, string strCode,
                                                                         int iCurrType, int BuySellType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AccountHoldLogoId,HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice from QH_HoldAccountTable WITH (NOLOCK)");
            strSql.Append(
                " where TradeCurrencyType=@TradeCurrencyType AND UserAccountDistributeLogo=@UserAccountDistributeLogo AND Contract=@Contract  AND BuySellTypeId=@BuySellTypeId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strHoldAccount);
            db.AddInParameter(dbCommand, "Contract", DbType.String, strCode);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, BuySellType);
            QH_HoldAccountTableInfo model = null;
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
        /// 获取某个持仓账户某个币种的保证金总额
        /// </summary>
        /// <param name="strHoldAccount"></param>
        /// <param name="iCurrType"></param>
        /// <returns></returns>
        public decimal GetMarginSum(string strHoldAccount, int iCurrType)
        {
            decimal result = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select sum(Margin) from QH_HoldAccountTable ");
            strSql.Append(
                " where TradeCurrencyType=@TradeCurrencyType AND UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strHoldAccount);

            try
            {
                object obj = db.ExecuteScalar(dbCommand);
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out result);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 获取保证金合计、按账号、币种分组的记录
        /// </summary>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> GetMarginSum()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select UserAccountDistributeLogo,TradeCurrencyType,sum(Margin) Margin from QH_HoldAccountTable ");
            sb.Append("group by UserAccountDistributeLogo,TradeCurrencyType ");
            sb.Append("order by UserAccountDistributeLogo");

            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
            Database db = DatabaseFactory.CreateDatabase();

            using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, sb.ToString()))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind2(dataReader));
                }
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public  QH_HoldAccountTableInfo GetQhAccountHoldTable(string strHoldAccount, string strCode, int iCurrType, int BuySellType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AccountHoldLogoId,HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice from QH_HoldAccountTable ");
            strSql.Append(
                " where TradeCurrencyType=@TradeCurrencyType AND UserAccountDistributeLogo=@UserAccountDistributeLogo AND Contract=@Contract AND BuySellTypeId=@BuySellTypeId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strHoldAccount);
            db.AddInParameter(dbCommand, "Contract", DbType.String, strCode);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, BuySellType);
            QH_HoldAccountTableInfo model = null;
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
        public int Add(QH_HoldAccountTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_HoldAccountTable(");
            strSql.Append(
                "HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice)");

            strSql.Append(" values (");
            strSql.Append(
                "@HistoryHoldAmount,@HistoryFreezeAmount,@HoldAveragePrice,@TodayHoldAmount,@TradeCurrencyType,@TodayHoldAveragePrice,@UserAccountDistributeLogo,@BuySellTypeId,@TodayFreezeAmount,@Contract,@CostPrice,@BreakevenPrice,@Margin,@ProfitLoss,@OpenAveragePrice)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HistoryHoldAmount", DbType.Decimal, model.HistoryHoldAmount);
            db.AddInParameter(dbCommand, "HistoryFreezeAmount", DbType.Decimal, model.HistoryFreezeAmount);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "TodayHoldAmount", DbType.Decimal, model.TodayHoldAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TodayHoldAveragePrice", DbType.Decimal, model.TodayHoldAveragePrice);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "TodayFreezeAmount", DbType.Decimal, model.TodayFreezeAmount);
            db.AddInParameter(dbCommand, "Contract", DbType.AnsiString, model.Contract);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OpenAveragePrice", DbType.Decimal, model.OpenAveragePrice);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(QH_HoldAccountTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_HoldAccountTable(");
            strSql.Append(
                "HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice)");

            strSql.Append(" values (");
            strSql.Append(
                "@HistoryHoldAmount,@HistoryFreezeAmount,@HoldAveragePrice,@TodayHoldAmount,@TradeCurrencyType,@TodayHoldAveragePrice,@UserAccountDistributeLogo,@BuySellTypeId,@TodayFreezeAmount,@Contract,@CostPrice,@BreakevenPrice,@Margin,@ProfitLoss,@OpenAveragePrice)");
            strSql.Append(";select @@IDENTITY");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HistoryHoldAmount", DbType.Decimal, model.HistoryHoldAmount);
            db.AddInParameter(dbCommand, "HistoryFreezeAmount", DbType.Decimal, model.HistoryFreezeAmount);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "TodayHoldAmount", DbType.Decimal, model.TodayHoldAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TodayHoldAveragePrice", DbType.Decimal, model.TodayHoldAveragePrice);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "TodayFreezeAmount", DbType.Decimal, model.TodayFreezeAmount);
            db.AddInParameter(dbCommand, "Contract", DbType.AnsiString, model.Contract);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OpenAveragePrice", DbType.Decimal, model.OpenAveragePrice);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddRecord(QH_HoldAccountTableInfo model,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_HoldAccountTable(");
            strSql.Append(
                "HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice)");

            strSql.Append(" values (");
            strSql.Append(
                "@HistoryHoldAmount,@HistoryFreezeAmount,@HoldAveragePrice,@TodayHoldAmount,@TradeCurrencyType,@TodayHoldAveragePrice,@UserAccountDistributeLogo,@BuySellTypeId,@TodayFreezeAmount,@Contract,@CostPrice,@BreakevenPrice,@Margin,@ProfitLoss,@OpenAveragePrice)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HistoryHoldAmount", DbType.Decimal, model.HistoryHoldAmount);
            db.AddInParameter(dbCommand, "HistoryFreezeAmount", DbType.Decimal, model.HistoryFreezeAmount);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "TodayHoldAmount", DbType.Decimal, model.TodayHoldAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TodayHoldAveragePrice", DbType.Decimal, model.TodayHoldAveragePrice);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "TodayFreezeAmount", DbType.Decimal, model.TodayFreezeAmount);
            db.AddInParameter(dbCommand, "Contract", DbType.AnsiString, model.Contract);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OpenAveragePrice", DbType.Decimal, model.OpenAveragePrice);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return false;
            }
            model.AccountHoldLogoId = result;
            return true;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_HoldAccountTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HoldAccountTable set ");
            strSql.Append("HistoryHoldAmount=@HistoryHoldAmount,");
            strSql.Append("HistoryFreezeAmount=@HistoryFreezeAmount,");
            strSql.Append("HoldAveragePrice=@HoldAveragePrice,");
            strSql.Append("TodayHoldAmount=@TodayHoldAmount,");
            strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("TodayHoldAveragePrice=@TodayHoldAveragePrice,");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("TodayFreezeAmount=@TodayFreezeAmount,");
            strSql.Append("Contract=@Contract,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("BreakevenPrice=@BreakevenPrice,");
            strSql.Append("Margin=@Margin,");
            strSql.Append("ProfitLoss=@ProfitLoss,");
            strSql.Append("OpenAveragePrice=@OpenAveragePrice");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "HistoryHoldAmount", DbType.Decimal, model.HistoryHoldAmount);
            db.AddInParameter(dbCommand, "HistoryFreezeAmount", DbType.Decimal, model.HistoryFreezeAmount);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "TodayHoldAmount", DbType.Decimal, model.TodayHoldAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TodayHoldAveragePrice", DbType.Decimal, model.TodayHoldAveragePrice);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "TodayFreezeAmount", DbType.Decimal, model.TodayFreezeAmount);
            db.AddInParameter(dbCommand, "Contract", DbType.AnsiString, model.Contract);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OpenAveragePrice", DbType.Decimal, model.OpenAveragePrice);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void AddUpdate(QH_HoldAccountTableInfo_Delta model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HoldAccountTable set ");
            strSql.Append("HistoryHoldAmount=HistoryHoldAmount+@HistoryHoldAmount,");
            strSql.Append("HistoryFreezeAmount=HistoryFreezeAmount+@HistoryFreezeAmount,");
            strSql.Append("TodayHoldAmount=TodayHoldAmount+@TodayHoldAmount,");
            strSql.Append("TodayFreezeAmount=TodayFreezeAmount+@TodayFreezeAmount,");
            strSql.Append("Margin=Margin+@Margin");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "HistoryHoldAmount", DbType.Decimal, model.HistoryHoldAmountDelta);
            db.AddInParameter(dbCommand, "HistoryFreezeAmount", DbType.Decimal, model.HistoryFreezeAmountDelta);
            db.AddInParameter(dbCommand, "TodayHoldAmount", DbType.Decimal, model.TodayHoldAmountDelta);
            db.AddInParameter(dbCommand, "TodayFreezeAmount", DbType.Decimal, model.TodayFreezeAmountDelta);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.MarginDelta);
            db.ExecuteNonQuery(dbCommand);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_HoldAccountTableInfo model,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HoldAccountTable set ");
            strSql.Append("HistoryHoldAmount=@HistoryHoldAmount,");
            strSql.Append("HistoryFreezeAmount=@HistoryFreezeAmount,");
            strSql.Append("HoldAveragePrice=@HoldAveragePrice,");
            strSql.Append("TodayHoldAmount=@TodayHoldAmount,");
            strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("TodayHoldAveragePrice=@TodayHoldAveragePrice,");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("BuySellTypeId=@BuySellTypeId,");
            strSql.Append("TodayFreezeAmount=@TodayFreezeAmount,");
            strSql.Append("Contract=@Contract,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("BreakevenPrice=@BreakevenPrice,");
            strSql.Append("Margin=@Margin,");
            strSql.Append("ProfitLoss=@ProfitLoss,");
            strSql.Append("OpenAveragePrice=@OpenAveragePrice");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = tm.Database;
            DbTransaction trans = tm.Transaction;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "HistoryHoldAmount", DbType.Decimal, model.HistoryHoldAmount);
            db.AddInParameter(dbCommand, "HistoryFreezeAmount", DbType.Decimal, model.HistoryFreezeAmount);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "TodayHoldAmount", DbType.Decimal, model.TodayHoldAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TodayHoldAveragePrice", DbType.Decimal, model.TodayHoldAveragePrice);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BuySellTypeId", DbType.Int32, model.BuySellTypeId);
            db.AddInParameter(dbCommand, "TodayFreezeAmount", DbType.Decimal, model.TodayFreezeAmount);
            db.AddInParameter(dbCommand, "Contract", DbType.AnsiString, model.Contract);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OpenAveragePrice", DbType.Decimal, model.OpenAveragePrice);
            db.ExecuteNonQuery(dbCommand, trans);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void AddUpdate(QH_HoldAccountTableInfo_Delta model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HoldAccountTable set ");
            strSql.Append("HistoryHoldAmount=HistoryHoldAmount+@HistoryHoldAmount,");
            strSql.Append("HistoryFreezeAmount=HistoryFreezeAmount+@HistoryFreezeAmount,");
            strSql.Append("TodayHoldAmount=TodayHoldAmount+@TodayHoldAmount,");
            strSql.Append("TodayFreezeAmount=TodayFreezeAmount+@TodayFreezeAmount,");
            strSql.Append("Margin=Margin+@Margin");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "HistoryHoldAmount", DbType.Decimal, model.HistoryHoldAmountDelta);
            db.AddInParameter(dbCommand, "HistoryFreezeAmount", DbType.Decimal, model.HistoryFreezeAmountDelta);
            db.AddInParameter(dbCommand, "TodayHoldAmount", DbType.Decimal, model.TodayHoldAmountDelta);
            db.AddInParameter(dbCommand, "TodayFreezeAmount", DbType.Decimal, model.TodayFreezeAmountDelta);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.MarginDelta);
            db.ExecuteNonQuery(dbCommand,tm.Transaction);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int AccountHoldLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_HoldAccountTable ");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, AccountHoldLogoId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_HoldAccountTableInfo GetModel(int AccountHoldLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AccountHoldLogoId,HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice from QH_HoldAccountTable ");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, AccountHoldLogoId);
            QH_HoldAccountTableInfo model = null;
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
        public List<QH_HoldAccountTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AccountHoldLogoId,HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice ");
            strSql.Append(" FROM QH_HoldAccountTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
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
        public List<QH_HoldAccountTableInfo> GetListArray(string strWhere, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AccountHoldLogoId,HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice ");
            strSql.Append(" FROM QH_HoldAccountTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
            Database db = tm.Database;
            using (IDataReader dataReader = db.ExecuteReader(tm.Transaction,CommandType.Text, strSql.ToString()))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

        /// <summary>
        /// 获得全部数据列表
        /// </summary>
        public List<QH_HoldAccountTableInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AccountHoldLogoId,HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice ");
            strSql.Append(" FROM QH_HoldAccountTable ");
           
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
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
        public QH_HoldAccountTableInfo ReaderBind(IDataReader dataReader)
        {
            QH_HoldAccountTableInfo model = new QH_HoldAccountTableInfo();
            object ojb;
            ojb = dataReader["AccountHoldLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountHoldLogoId = (int) ojb;
            }
            ojb = dataReader["HistoryHoldAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HistoryHoldAmount = (decimal) ojb;
            }
            ojb = dataReader["HistoryFreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HistoryFreezeAmount = (decimal) ojb;
            }
            ojb = dataReader["HoldAveragePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HoldAveragePrice = (decimal) ojb;
            }
            ojb = dataReader["TodayHoldAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayHoldAmount = (decimal) ojb;
            }
            ojb = dataReader["TradeCurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyType = (int) ojb;
            }
            ojb = dataReader["TodayHoldAveragePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayHoldAveragePrice = (decimal) ojb;
            }
            model.UserAccountDistributeLogo = dataReader["UserAccountDistributeLogo"].ToString();
            ojb = dataReader["BuySellTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BuySellTypeId = (int) ojb;
            }
            ojb = dataReader["TodayFreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayFreezeAmount = (decimal) ojb;
            }
            model.Contract = dataReader["Contract"].ToString();
            ojb = dataReader["CostPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CostPrice = (decimal) ojb;
            }
            ojb = dataReader["BreakevenPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreakevenPrice = (decimal) ojb;
            }
            ojb = dataReader["Margin"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Margin = (decimal) ojb;
            }
            ojb = dataReader["ProfitLoss"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProfitLoss = (decimal) ojb;
            }
            ojb = dataReader["OpenAveragePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OpenAveragePrice = (decimal) ojb;
            }
            return model;
        }


        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public QH_HoldAccountTableInfo ReaderBind2(IDataReader dataReader)
        {
            QH_HoldAccountTableInfo model = new QH_HoldAccountTableInfo();
            object ojb;
            
            model.UserAccountDistributeLogo = dataReader["UserAccountDistributeLogo"].ToString();

            ojb = dataReader["TradeCurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyType = (int)ojb;
            }

            ojb = dataReader["Margin"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Margin = (decimal)ojb;
            }
           
            return model;
        }


        #region 根据用户ID和密码查询用户所拥有的期货持仓账号明细
        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的期货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">密码</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> GetListByUserIDAndPwd(string userID, string pwd, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserAndPwd), userID, pwd));
        }
        #endregion

        #region 根据用户ID查询用户所拥有的期货持仓账号明细
        /// <summary>
        /// 根据用户ID查询用户所拥有的期货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> GetListByUserID(string userID, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserID), userID));
        }
        #endregion

        #region  根据期货持仓账号查询期货持仓账号明细
        /// <summary>
        /// 根据期货持仓账号查询期货持仓账号明细
        /// </summary>
        ///<param name="account">期货持仓账号</param>
        /// <param name="type">币种</param>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> GetListByAccount(string account, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByAccount), account));
        }
        #endregion

        #region 根据查询的货币类型和查询的条件类型构建查询的SQLScript
        /// <summary>
        /// Title:根据查询的货币类型和查询的条件类型构建查询的SQLScript
        /// Desc:默认返回根据主键查询
        /// </summary>
        /// <param name="type">要查询的货币类型</param>
        /// <param name="byType">查询条件类型</param>
        /// <returns>返回查询的条件脚本语句</returns>
        string BuildQueryWhere(QueryType.QueryCurrencyType type, QueryType.QueryWhereType byType)
        {
            string strByWhere = "";
            switch (byType)
            {
                case QueryType.QueryWhereType.ByUserID:
                    strByWhere = "  UserAccountDistributeLogo in( select useraccountdistributelogo from UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesHold + "')  and userid='{0}' )";
                    break;
                case QueryType.QueryWhereType.ByUserAndPwd:
                    strByWhere = "   UserAccountDistributeLogo in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesHold + "') and userid in (select userid from dbo.UA_UserBasicInformationTable where  userid='{1}' And  Password ='{0}' ))";
                    break;
                case QueryType.QueryWhereType.ByAccount:
                    strByWhere = " UserAccountDistributeLogo='{0}'  ";
                    break;
                default:
                    strByWhere = " AccountHoldLogoId ='{0}'";
                    break;
            }

            if (QueryType.QueryCurrencyType.ALL != type)
            {
                strByWhere += "  And  TradeCurrencyType='" + (int)type + "'";
            }
            return strByWhere;

        }
        #endregion
        #endregion  成员方法
    }
}