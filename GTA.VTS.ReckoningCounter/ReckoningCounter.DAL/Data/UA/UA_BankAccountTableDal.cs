#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类UA_BankAccountTableDal。
    /// Update by:董鹏
    /// Update date:2009-12-24
    /// Desc.: 添加个性化资金设置的方法
    /// </summary>
    public class UA_BankAccountTableDal
    {
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(TradeCurrencyTypeLogo)+1 from UA_BankAccountTable";
            Database db = DatabaseFactory.CreateDatabase();
            object obj = db.ExecuteScalar(CommandType.Text, strsql);
            if (obj != null && obj != DBNull.Value)
            {
                return int.Parse(obj.ToString());
            }
            return 1;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TradeCurrencyTypeLogo, string UserAccountDistributeLogo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from UA_BankAccountTable where TradeCurrencyTypeLogo=@TradeCurrencyTypeLogo and UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, TradeCurrencyTypeLogo);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, UserAccountDistributeLogo);
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
        /// 增加一条数据
        /// </summary>
        public void Add(UA_BankAccountTableInfo model)
        {
            Database db = DatabaseFactory.CreateDatabase();
            Add(model, db, null);

        }
        /// <summary>
        /// 开是否开启事务增加一条数据
        /// <param name="model">要插入的用户账号对象</param>
        /// <param name="db">操作数据对象</param>
        /// <param name="trm">开启事务对象，如果为null不开启</param>
        /// </summary>
        public void Add(UA_BankAccountTableInfo model, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UA_BankAccountTable(");
            strSql.Append(
                "CapitalRemainAmount,TradeCurrencyTypeLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapital,AvailableCapital)");

            strSql.Append(" values (");
            strSql.Append(
                "@CapitalRemainAmount,@TradeCurrencyTypeLogo,@UserAccountDistributeLogo,@BalanceOfTheDay,@TodayOutInCapital,@FreezeCapital,@AvailableCapital)");

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalRemainAmount", DbType.Decimal, model.CapitalRemainAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, model.TradeCurrencyTypeLogo);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapital", DbType.Decimal, model.FreezeCapital);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            if (trm == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, trm);
            }
        }

        /// <summary>
        /// 开启是否使用事务新数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        /// <param name="trm"></param>
        public void Update(UA_BankAccountTableInfo model, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UA_BankAccountTable set ");
            strSql.Append("CapitalRemainAmount=@CapitalRemainAmount,");
            strSql.Append("BalanceOfTheDay=@BalanceOfTheDay,");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("FreezeCapital=@FreezeCapital,");
            strSql.Append("AvailableCapital=@AvailableCapital");
            strSql.Append(
                " where TradeCurrencyTypeLogo=@TradeCurrencyTypeLogo and UserAccountDistributeLogo=@UserAccountDistributeLogo ");

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalRemainAmount", DbType.Decimal, model.CapitalRemainAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, model.TradeCurrencyTypeLogo);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapital", DbType.Decimal, model.FreezeCapital);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            if (trm != null)
            {
                db.ExecuteNonQuery(dbCommand, trm);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(UA_BankAccountTableInfo model)
        {
            Database db = DatabaseFactory.CreateDatabase();
            Update(model, db, null);

        }

        /// <summary>
        /// 开户时根据银行账号和交易货币类型增加资金
        /// Create BY:李健华
        /// Crate Date:2009-07-12
        /// </summary>
        /// <param name="addAmount">增加总数</param>
        /// <param name="bankAccount">银行账号</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="db"></param>
        /// <param name="trm">开启事务</param>
        public void AddCapital(decimal addAmount, string bankAccount, GTA.VTS.Common.CommonObject.Types.CurrencyType currencyType, Database db, DbTransaction trm)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE [UA_BankAccountTable]   SET ");
            strSql.Append(" CapitalRemainAmount = CapitalRemainAmount + @AddAmount");
            strSql.Append("  ,TodayOutInCapital = TodayOutInCapital + @AddAmount");
            strSql.Append("  ,AvailableCapital =AvailableCapital + @AddAmount");
            strSql.Append(" WHERE UserAccountDistributeLogo=@UserAccountDistributeLogo");
            strSql.Append("    And  TradeCurrencyTypeLogo=@TradeCurrencyTypeLogo");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AddAmount", DbType.Decimal, addAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, (int)currencyType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, bankAccount);
            db.ExecuteNonQuery(dbCommand, trm);
        }

        /// <summary>
        /// 转账时根据银行账号和交易货币类型增加或者减少资金（转出转入)
        /// Create BY:李健华
        /// Crate Date:2009-07-12
        /// </summary>
        /// <param name="addAmount">增加总数</param>
        /// <param name="isAdd">增加或者减少</param>
        /// <param name="bankAccount">银行账号</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="db"></param>
        /// <param name="trm">开启事务</param>
        public void AddOrSubCapital(decimal addAmount, bool isAdd, string bankAccount, GTA.VTS.Common.CommonObject.Types.CurrencyType currencyType, Database db, DbTransaction trm)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE [UA_BankAccountTable]   SET ");
            strSql.Append(" CapitalRemainAmount = CapitalRemainAmount {0} @AddAmount");
            strSql.Append("  ,TodayOutInCapital = TodayOutInCapital {0} @AddAmount");
            strSql.Append("  ,AvailableCapital =AvailableCapital {0} @AddAmount");
            strSql.Append("  ,BalanceOfTheDay =BalanceOfTheDay {0} @AddAmount");
            strSql.Append(" WHERE UserAccountDistributeLogo=@UserAccountDistributeLogo");
            strSql.Append("    And  TradeCurrencyTypeLogo=@TradeCurrencyTypeLogo");
            DbCommand dbCommand = db.GetSqlStringCommand(string.Format(strSql.ToString(), isAdd ? "+" : "-"));
            db.AddInParameter(dbCommand, "AddAmount", DbType.Decimal, addAmount);
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, (int)currencyType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, bankAccount);
            db.ExecuteNonQuery(dbCommand, trm);
        }



        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TradeCurrencyTypeLogo, string UserAccountDistributeLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UA_BankAccountTable ");
            strSql.Append(
                " where TradeCurrencyTypeLogo=@TradeCurrencyTypeLogo and UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, TradeCurrencyTypeLogo);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, UserAccountDistributeLogo);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UA_BankAccountTableInfo GetModel(int TradeCurrencyTypeLogo, string UserAccountDistributeLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalRemainAmount,TradeCurrencyTypeLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapital,AvailableCapital from UA_BankAccountTable ");
            strSql.Append(
                " where TradeCurrencyTypeLogo=@TradeCurrencyTypeLogo and UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, TradeCurrencyTypeLogo);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, UserAccountDistributeLogo);
            UA_BankAccountTableInfo model = null;
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
        public List<UA_BankAccountTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalRemainAmount,TradeCurrencyTypeLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapital,AvailableCapital ");
            strSql.Append(" FROM UA_BankAccountTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<UA_BankAccountTableInfo> list = new List<UA_BankAccountTableInfo>();
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
        public UA_BankAccountTableInfo ReaderBind(IDataReader dataReader)
        {
            UA_BankAccountTableInfo model = new UA_BankAccountTableInfo();
            object ojb;
            ojb = dataReader["CapitalRemainAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalRemainAmount = (decimal)ojb;
            }
            ojb = dataReader["TradeCurrencyTypeLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyTypeLogo = (int)ojb;
            }
            model.UserAccountDistributeLogo = dataReader["UserAccountDistributeLogo"].ToString();
            ojb = dataReader["BalanceOfTheDay"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BalanceOfTheDay = (decimal)ojb;
            }
            ojb = dataReader["TodayOutInCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayOutInCapital = (decimal)ojb;
            }
            ojb = dataReader["FreezeCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapital = (decimal)ojb;
            }
            ojb = dataReader["AvailableCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapital = (decimal)ojb;
            }
            return model;
        }

        #endregion  成员方法

        /// <summary>
        /// 个性化资金设置
        /// </summary>
        /// <param name="amount">设置金额</param>
        /// <param name="account">资金账号</param>
        /// <param name="currencyType">币种</param>
        /// <param name="db">数据连接对象</param>
        /// <param name="trm">开启事务</param>
        public void PersonalizationCapital(decimal amount, string account, GTA.VTS.Common.CommonObject.Types.CurrencyType currencyType, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE [UA_BankAccountTable]   SET ");
            strSql.Append(" AvailableCapital = @amount");
            strSql.Append(" WHERE UserAccountDistributeLogo=@UserAccountDistributeLogo");
            strSql.Append("    And  TradeCurrencyTypeLogo=@TradeCurrencyTypeLogo");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "amount", DbType.Decimal, amount);
            db.AddInParameter(dbCommand, "TradeCurrencyTypeLogo", DbType.Int32, (int)currencyType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, account);
            db.ExecuteNonQuery(dbCommand, trm);
        }
    }
}