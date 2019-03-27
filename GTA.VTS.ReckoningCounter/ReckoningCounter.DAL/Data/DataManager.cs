using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据层管理器
    /// </summary>
    public static class DataManager
    {
        /// <summary>
        /// 获取企业库Database
        /// </summary>
        /// <returns></returns>
        public static Database GetDatabase()
        {
            return DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 数据库事务执行委托
        /// </summary>
        /// <param name="database"></param>
        /// <param name="transaction"></param>
        public delegate void ActionInTrans(Database database, DbTransaction transaction);

        /// <summary>
        /// 数据库事务执行委托
        /// </summary>
        /// <param name="reckoningTransaction"></param>
        public delegate void ActionInTrans2(ReckoningTransaction reckoningTransaction);

        /// <summary>
        /// 执行多表事务的帮助方法,外部调用需要捕获异常
        /// 使用方法如下
        /// try
        /// {
        ///     DataManager.ExecuteInTransaction((database, transaction) =>
        ///                               {
        ///                                   database.ExecuteNonQuery(dbCommand1, transaction);
        ///                                   database.ExecuteNonQuery(dbCommand2, transaction);
        ///                               });
        /// }
        /// catch (Exception ex)
        /// {
        ///    //做异常处理
        /// }
        /// </summary>
        /// <param name="actionInTrans"></param>
        public static void ExecuteInTransaction(ActionInTrans actionInTrans)
        {
            Database database = DatabaseFactory.CreateDatabase();

            using(DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    actionInTrans(database, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 执行多表事务的帮助方法,外部调用需要捕获异常
        /// 使用方法如下
        /// try
        /// {
        ///     DataManager.ExecuteInTransaction((tm) =>
        ///                               {
        ///                                   Database database = tm.Database;
        ///                                   DbTransaction transaction = tm.Transaction;
        ///                                   database.ExecuteNonQuery(dbCommand1, transaction);
        ///                                   database.ExecuteNonQuery(dbCommand2, transaction);
        ///                                     
        ///                                   //或者直接传入下一层
        ///                                   MyDataLogic.Update(tm);
        ///                               });
        /// }
        /// catch (Exception ex)
        /// {
        ///    //做异常处理
        /// }
        /// </summary>
        /// <param name="actionInTrans2"></param>
        public static void ExecuteInTransaction(ActionInTrans2 actionInTrans2)
        {
            Database database = DatabaseFactory.CreateDatabase();

            using(DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
                    reckoningTransaction.Database = database;
                    reckoningTransaction.Transaction = transaction;
                    actionInTrans2(reckoningTransaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// 事务封装类
    /// </summary>
    public class ReckoningTransaction
    {
        /// <summary>
        /// 数据库对象
        /// </summary>
        public Database Database;

        /// <summary>
        /// 事务对象
        /// </summary>
        public DbTransaction Transaction;
    }
}
