using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DbHelperSQL
    {
        /// <summary>
        /// 执行一条sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        public static void ExecuteSql(string strSql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand);
        }



        /// <summary>
        /// 执行一条sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="reckoningTransaction">事务</param>
        /// <returns></returns>
        public static int ExecuteCountSql(string strSql, ReckoningTransaction reckoningTransaction)
        {
            Database db = reckoningTransaction.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            return db.ExecuteNonQuery(dbCommand, reckoningTransaction.Transaction);
        }
        
        /// <summary>
        /// 执行一条sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="reckoningTransaction">事务</param>
        /// <returns></returns>
        public static bool ExecuteBollSql(string strSql, ReckoningTransaction reckoningTransaction)
        {
            Database db = reckoningTransaction.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            try
            {
                db.ExecuteNonQuery(dbCommand, reckoningTransaction.Transaction);
                return true;
            }
            catch
            {

                return false;
            }

        }
        /// <summary>
        /// 执行一条sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        public static int ExecuteCountSql(string strSql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            return db.ExecuteNonQuery(dbCommand);
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        public static void RunProcedure(string storedProcName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 异步执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="callBack"></param>
        public static void BeginRunProcedure(string storedProcName, AsyncCallback callBack)
        {
            Database db = DatabaseFactory.CreateDatabase();
            var conn = db.CreateConnection();
            string connString = conn.ConnectionString;
            conn.Close();

            if (!connString.EndsWith(";"))
                connString += ";";
            connString += "Asynchronous Processing=true";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connString;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;

            connection.Open();
            cmd.BeginExecuteNonQuery(callBack, connection);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="reckoningTransaction"></param>
        public static void RunProcedure(string storedProcName, ReckoningTransaction reckoningTransaction)
        {
            Database db = reckoningTransaction.Database;
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName);
            db.ExecuteNonQuery(dbCommand, reckoningTransaction.Transaction);
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="reckoningTransaction"></param>
        public static void RunProcedure(string storedProcName, int commandTimeout, ReckoningTransaction reckoningTransaction)
        {
            Database db = reckoningTransaction.Database;
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName);

            if (commandTimeout < 0)
                commandTimeout = 30;
            dbCommand.CommandTimeout = commandTimeout;

            db.ExecuteNonQuery(dbCommand, reckoningTransaction.Transaction);
        }

        /// <summary>
        /// 获取表某个字段的最大值
        /// </summary>

        public static Object ExecuteSqlScalar(string strSql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            return db.ExecuteScalar(dbCommand);
        }

        /// <summary>
        /// 带有事物执行sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="reckoningTransaction">事物</param>
        public static void ExecuteSql(string strSql, ReckoningTransaction reckoningTransaction)
        {
            Database db = reckoningTransaction.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            db.ExecuteNonQuery(dbCommand, reckoningTransaction.Transaction);
        }


        /// <summary>
        /// 分红处理
        /// </summary>
        /// <param name="accountList"></param>
        /// <param name="flowTableInfos"></param>
        /// <param name="registerTableInfos"></param>
        public void ExecuteSql(List<XH_CapitalAccountTableInfo> accountList, List<UA_CapitalFlowTableInfo> flowTableInfos, List<XH_MelonCutRegisterTableInfo> registerTableInfos)
        {
            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    if (accountList != null && accountList.Count > 0)
                    {
                        XH_CapitalAccountTableDal xh_AccountTableDal = new XH_CapitalAccountTableDal();
                        foreach (var accountTableInfo in accountList)
                        {
                            xh_AccountTableDal.Update(accountTableInfo, db, transaction);
                        }
                    }
                    if (flowTableInfos != null && flowTableInfos.Count > 0)
                    {
                        UA_CapitalFlowTableDal flowTableDal = new UA_CapitalFlowTableDal();
                        foreach (var historyTradeTableInfo in flowTableInfos)
                        {
                            flowTableDal.Add(historyTradeTableInfo, db, transaction);
                        }
                    }
                    if (registerTableInfos != null && registerTableInfos.Count > 0)
                    {
                        XH_MelonCutRegisterTableDal melonCutRegisterTableDal = new XH_MelonCutRegisterTableDal();
                        foreach (var registerTableInfo in registerTableInfos)
                        {
                            melonCutRegisterTableDal.Delete(registerTableInfo, db, transaction);
                        }
                    }
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="modelList">对某个股票进行登记</param>
        public static void Add(List<XH_MelonCutRegisterTableInfo> modelList)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbTransaction transaction;
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    foreach (var model in modelList)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into XH_MelonCutRegisterTable(");
                        strSql.Append(
                            "RegisterDate,CutDate,UserAccountDistributeLogo,TradeCurrencyType,Code,RegisterAmount,CutType,CurrencyTypeId)");

                        strSql.Append(" values (");
                        strSql.Append(
                            "@RegisterDate,@CutDate,@UserAccountDistributeLogo,@TradeCurrencyType,@Code,@RegisterAmount,@CutType,@CurrencyTypeId)");
                        DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                        dbCommand.Transaction = transaction;
                        db.AddInParameter(dbCommand, "RegisterDate", DbType.DateTime, model.RegisterDate);
                        db.AddInParameter(dbCommand, "CutDate", DbType.DateTime, model.CutDate);
                        db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
                        db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
                        db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
                        db.AddInParameter(dbCommand, "RegisterAmount", DbType.Decimal, model.RegisterAmount);
                        db.AddInParameter(dbCommand, "CutType", DbType.Int32, model.CutType);
                        db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
                        //dbCommand.ExecuteNonQuery();
                        db.ExecuteNonQuery(dbCommand, transaction);

                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }



        }
    }
}
