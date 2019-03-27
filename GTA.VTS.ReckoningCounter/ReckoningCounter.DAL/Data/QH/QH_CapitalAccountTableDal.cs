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
    /// 数据访问类QH_CapitalAccountTableDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// Update by:董鹏
    /// Update date:2009-12-25
    /// Desc.: 添加个性化资金设置的方法
    /// </summary>
    public class QH_CapitalAccountTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CapitalAccountLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from QH_CapitalAccountTable where CapitalAccountLogoId=@CapitalAccountLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, CapitalAccountLogoId);
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
        /// 获取可以资金
        /// </summary>
        /// <param name="strCapitalAccount"></param>
        /// <param name="iCurrType"></param>
        /// <returns></returns>
        public decimal GetAvailableCapitalWithNoLock(string strCapitalAccount, int iCurrType)
        {
            decimal result = -1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AvailableCapital from QH_CapitalAccountTable WITH (NOLOCK)");
            strSql.Append(
                " where TradeCurrencyType=@TradeCurrencyType AND UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strCapitalAccount);

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
        /// 得到一个对象实体
        /// </summary>
        public QH_CapitalAccountTableInfo GetQHCapitalAccount(string strCapitalAccount, int iCurrType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogoId,AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,MarginTotal,TradeCurrencyType from QH_CapitalAccountTable ");
            strSql.Append(
                " where TradeCurrencyType=@TradeCurrencyType AND UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strCapitalAccount);
            QH_CapitalAccountTableInfo model = null;
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
        /// 开是否开启事务增加一条数据
        /// <param name="model">要插入的用户账号对象</param>
        /// <param name="db">操作数据对象</param>
        /// <param name="trm">开启事务对象，如果为null不开启</param>
        /// </summary>
        public int Add(QH_CapitalAccountTableInfo model, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_CapitalAccountTable(");
            strSql.Append(
                "AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,MarginTotal,TradeCurrencyType,CloseFloatProfitLossTotal,CloseMarketProfitLossTotal)");

            strSql.Append(" values (");
            strSql.Append(
                "@AvailableCapital,@UserAccountDistributeLogo,@BalanceOfTheDay,@TodayOutInCapital,@FreezeCapitalTotal,@MarginTotal,@TradeCurrencyType,@CloseFloatProfitLossTotal,@CloseMarketProfitLossTotal)");
            strSql.Append(";select @@IDENTITY");

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            // db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);//数据库统计约束字段
            db.AddInParameter(dbCommand, "MarginTotal", DbType.Decimal, model.MarginTotal);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotal", DbType.Decimal, model.CloseFloatProfitLossTotal);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotal", DbType.Decimal, model.CloseMarketProfitLossTotal);
            int result;
            object obj = null;
            if (trm == null)
            {
                obj = db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                obj = db.ExecuteNonQuery(dbCommand, trm);
            }
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;

        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(QH_CapitalAccountTableInfo model)
        {
            Database db = DatabaseFactory.CreateDatabase();
            return Add(model, db, null);
        }

        /// <summary>
        /// 转账时根据主键ID增加或者减少资金
        /// Create by:李健华
        /// Crate date:2009-07-19
        /// Desc.:增加或者减少由isdd来判断，增加即数据原有量加上要增加的值，如数据为10增加为100最后为110
        /// </summary>
        /// <param name="amount">要减少的资金总额</param>
        /// <param name="isAdd">增加/减少</param>
        /// <param name="id">主键</param>
        /// <param name="db">数据库对象</param>
        /// <param name="trm">开启事务</param>
        public void AddOrSubCapital(decimal amount, bool isAdd, int id, Database db, DbTransaction trm)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=AvailableCapital {0} @Amount,");
            strSql.Append("TodayOutInCapital=TodayOutInCapital {0} @Amount,");
            strSql.Append("BalanceOfTheDay=BalanceOfTheDay {0} @Amount,");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");

            DbCommand dbCommand = db.GetSqlStringCommand(string.Format(strSql.ToString(), isAdd ? "+" : "-"));
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, id);
            db.AddInParameter(dbCommand, "Amount", DbType.Decimal, amount);
            db.ExecuteNonQuery(dbCommand, trm);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_CapitalAccountTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=@AvailableCapital,");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("BalanceOfTheDay=@BalanceOfTheDay,");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("FreezeCapitalTotal=@FreezeCapitalTotal,");
            // strSql.Append("CapitalBalance=@CapitalBalance,");
            strSql.Append("MarginTotal=@MarginTotal,");
            strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("CloseFloatProfitLossTotal=@CloseFloatProfitLossTotal,");
            strSql.Append("CloseMarketProfitLossTotal=@CloseMarketProfitLossTotal");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, model.CapitalAccountLogoId);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            // db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);
            db.AddInParameter(dbCommand, "MarginTotal", DbType.Decimal, model.MarginTotal);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotal", DbType.Decimal, model.CloseFloatProfitLossTotal);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotal", DbType.Decimal, model.CloseMarketProfitLossTotal);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 仅更新变化字段
        /// </summary>
        public void Update(QH_CapitalAccountTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=@AvailableCapital,");
            //strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            //strSql.Append("BalanceOfTheDay=@BalanceOfTheDay,");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("FreezeCapitalTotal=@FreezeCapitalTotal,");
            //strSql.Append("CapitalBalance=@CapitalBalance,");
            strSql.Append("MarginTotal=@MarginTotal,");
            //strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("CloseFloatProfitLossTotal=@CloseFloatProfitLossTotal,");
            strSql.Append("CloseMarketProfitLossTotal=@CloseMarketProfitLossTotal");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, model.CapitalAccountLogoId);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            //db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            //db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            //db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);
            db.AddInParameter(dbCommand, "MarginTotal", DbType.Decimal, model.MarginTotal);
            //db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotal", DbType.Decimal, model.CloseFloatProfitLossTotal);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotal", DbType.Decimal, model.CloseMarketProfitLossTotal);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 仅更新变化字段
        /// </summary>
        public bool UpdateRecord(QH_CapitalAccountTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=@AvailableCapital,");
            //strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            //strSql.Append("BalanceOfTheDay=@BalanceOfTheDay,");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("FreezeCapitalTotal=@FreezeCapitalTotal,");
            //strSql.Append("CapitalBalance=@CapitalBalance,");
            strSql.Append("MarginTotal=@MarginTotal,");
            //strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("CloseFloatProfitLossTotal=@CloseFloatProfitLossTotal,");
            strSql.Append("CloseMarketProfitLossTotal=@CloseMarketProfitLossTotal");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, model.CapitalAccountLogoId);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            //db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            //db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            //db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);
            db.AddInParameter(dbCommand, "MarginTotal", DbType.Decimal, model.MarginTotal);
            //db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotal", DbType.Decimal, model.CloseFloatProfitLossTotal);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotal", DbType.Decimal, model.CloseMarketProfitLossTotal);
            try
            {
                db.ExecuteNonQuery(dbCommand, transaction);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }

        }

        /// <summary>
        /// 将新值累加到原来的字段上(+=)
        /// </summary>
        public void AddUpdate(QH_CapitalAccountTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=AvailableCapital+(@AvailableCapital),");
            //strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            //strSql.Append("BalanceOfTheDay=@BalanceOfTheDay,");
            strSql.Append("TodayOutInCapital=TodayOutInCapital+(@TodayOutInCapital),");
            strSql.Append("FreezeCapitalTotal=FreezeCapitalTotal+(@FreezeCapitalTotal),");
            //strSql.Append("CapitalBalance=@CapitalBalance,");
            strSql.Append("MarginTotal=MarginTotal+(@MarginTotal),");
            //strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("CloseFloatProfitLossTotal=CloseFloatProfitLossTotal+(@CloseFloatProfitLossTotal),");
            strSql.Append("CloseMarketProfitLossTotal=CloseMarketProfitLossTotal+(@CloseMarketProfitLossTotal)");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, model.CapitalAccountLogoId);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            //db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            //db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            //db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);
            db.AddInParameter(dbCommand, "MarginTotal", DbType.Decimal, model.MarginTotal);
            //db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotal", DbType.Decimal, model.CloseFloatProfitLossTotal);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotal", DbType.Decimal, model.CloseMarketProfitLossTotal);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CapitalAccountLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_CapitalAccountTable ");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, CapitalAccountLogoId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_CapitalAccountTableInfo GetModel(int CapitalAccountLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogoId,AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,MarginTotal,TradeCurrencyType,CloseFloatProfitLossTotal,CloseMarketProfitLossTotal from QH_CapitalAccountTable ");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, CapitalAccountLogoId);
            QH_CapitalAccountTableInfo model = null;
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
        public List<QH_CapitalAccountTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogoId,AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,MarginTotal,TradeCurrencyType,CloseFloatProfitLossTotal,CloseMarketProfitLossTotal ");
            strSql.Append(" FROM QH_CapitalAccountTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_CapitalAccountTableInfo> list = new List<QH_CapitalAccountTableInfo>();
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
        public List<QH_CapitalAccountTableInfo> GetListArray(Database db, DbTransaction transaction, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogoId,AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,MarginTotal,TradeCurrencyType,CloseFloatProfitLossTotal,CloseMarketProfitLossTotal ");
            strSql.Append(" FROM QH_CapitalAccountTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_CapitalAccountTableInfo> list = new List<QH_CapitalAccountTableInfo>();
            using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, strSql.ToString()))
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
        public List<QH_CapitalAccountTableInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogoId,AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,MarginTotal,TradeCurrencyType,CloseFloatProfitLossTotal,CloseMarketProfitLossTotal ");
            strSql.Append(" FROM QH_CapitalAccountTable ");

            List<QH_CapitalAccountTableInfo> list = new List<QH_CapitalAccountTableInfo>();
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
        public QH_CapitalAccountTableInfo ReaderBind(IDataReader dataReader)
        {
            QH_CapitalAccountTableInfo model = new QH_CapitalAccountTableInfo();
            object ojb;
            ojb = dataReader["CapitalAccountLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogoId = (int)ojb;
            }
            ojb = dataReader["AvailableCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapital = (decimal)ojb;
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
            ojb = dataReader["FreezeCapitalTotal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapitalTotal = (decimal)ojb;
            }
            ojb = dataReader["CapitalBalance"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalBalance = (decimal)ojb;
            }
            ojb = dataReader["MarginTotal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarginTotal = (decimal)ojb;
            }
            ojb = dataReader["TradeCurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyType = (int)ojb;
            }
            ojb = dataReader["CloseFloatProfitLossTotal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseFloatProfitLossTotal = (decimal)ojb;
            }
            ojb = dataReader["CloseMarketProfitLossTotal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseMarketProfitLossTotal = (decimal)ojb;
            }
            return model;
        }



        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的期货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">密码</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<QH_CapitalAccountTableInfo> GetListByUserIDAndPwd(string userID, string pwd, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserAndPwd), userID, pwd));
        }

        /// <summary>
        /// 根据用户ID查询用户所拥有的期货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<QH_CapitalAccountTableInfo> GetListByUserID(string userID, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserID), userID));
        }
        /// <summary>
        /// 根据期货资金账号查询期货资金账号明细
        /// </summary>
        ///<param name="account">期货资金账号</param>
        /// <param name="type">币种</param>
        /// <returns></returns>
        public List<QH_CapitalAccountTableInfo> GetListByAccount(string account, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByAccount), account));
        }
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
                    strByWhere = "  UserAccountDistributeLogo in( select useraccountdistributelogo from UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesCapital + "')  and userid='{0}' )";
                    break;
                case QueryType.QueryWhereType.ByUserAndPwd:
                    strByWhere = "   UserAccountDistributeLogo in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesCapital + "') and userid in (select userid from dbo.UA_UserBasicInformationTable where  userid='{1}' And  Password ='{0}' ))";
                    break;
                case QueryType.QueryWhereType.ByAccount:
                    strByWhere = " UserAccountDistributeLogo='{0}'  ";
                    break;
                default:
                    strByWhere = " CapitalAccountLogoId ='{0}'";
                    break;
            }

            if (QueryType.QueryCurrencyType.ALL != type)
            {
                strByWhere += "  And  TradeCurrencyType='" + (int)type + "'";
            }
            return strByWhere;

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
            strSql.Append("UPDATE [QH_CapitalAccountTable]   SET ");
            strSql.Append(" AvailableCapital = @amount");
            strSql.Append(" WHERE UserAccountDistributeLogo=@UserAccountDistributeLogo");
            strSql.Append("    And  TradeCurrencyType=@TradeCurrencyType");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "amount", DbType.Decimal, amount);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, (int)currencyType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, account);
            db.ExecuteNonQuery(dbCommand, trm);
        }
    }
}