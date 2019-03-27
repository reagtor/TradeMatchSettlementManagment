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
    /// 数据访问类XH_CapitalAccountTableDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// Update by:董鹏
    /// Update date:2009-12-25
    /// Desc.: 添加个性化资金设置的方法
    /// </summary>
    public class XH_CapitalAccountTableDal
    {
        #region  成员方法
        #region 是否存在该记录
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CapitalAccountLogo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_CapitalAccountTable where CapitalAccountLogo=@CapitalAccountLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, CapitalAccountLogo);
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
        public XH_CapitalAccountTableInfo GetXHCapitalAccountWithNoLock(string strCapitalAccount, int iCurrType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AvailableCapital,CapitalAccountLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,TradeCurrencyType,HasDoneProfitLossTotal from XH_CapitalAccountTable WITH (NOLOCK)");
            strSql.Append(
                " where TradeCurrencyType=@TradeCurrencyType AND UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strCapitalAccount);
            XH_CapitalAccountTableInfo model = null;
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
        public XH_CapitalAccountTableInfo GetXHCapitalAccount(string strCapitalAccount, int iCurrType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AvailableCapital,CapitalAccountLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,TradeCurrencyType,HasDoneProfitLossTotal from XH_CapitalAccountTable ");
            strSql.Append(
                " where TradeCurrencyType=@TradeCurrencyType AND UserAccountDistributeLogo=@UserAccountDistributeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strCapitalAccount);
            XH_CapitalAccountTableInfo model = null;
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
        /// 获取可以资金
        /// </summary>
        /// <param name="strCapitalAccount">资金账号</param>
        /// <param name="iCurrType">币种</param>
        /// <returns></returns>
        public decimal GetAvailableCapitalWithNoLock(string strCapitalAccount, int iCurrType)
        {
            decimal result = -1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AvailableCapital from XH_CapitalAccountTable WITH (NOLOCK)");
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
        #endregion

        #region 增加一条数据
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(XH_CapitalAccountTableInfo model)
        {
            Database db = DatabaseFactory.CreateDatabase();
            return Add(model, db, null);

        }

        /// <summary>
        /// 开是否开启事务增加一条数据
        /// <param name="model">要插入的用户账号对象</param>
        /// <param name="db">操作数据对象</param>
        /// <param name="trm">开启事务对象，如果为null不开启</param>
        /// </summary>
        public int Add(XH_CapitalAccountTableInfo model, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_CapitalAccountTable(");
            strSql.Append(
                "AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,TradeCurrencyType,HasDoneProfitLossTotal)");

            strSql.Append(" values (");
            strSql.Append(
                "@AvailableCapital,@UserAccountDistributeLogo,@BalanceOfTheDay,@TodayOutInCapital,@FreezeCapitalTotal,@TradeCurrencyType,@HasDoneProfitLossTotal)");
            strSql.Append(";select @@IDENTITY");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            //db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);//数据库统计值不用附值
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotal", DbType.Decimal, model.HasDoneProfitLossTotal);
            int result;
            object obj = null;
            if (trm == null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand, trm);
            }
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;

        }


        #endregion

        #region 更新一条数据
        /// <summary>
        /// 更新一条数据
        /// Update BY:李健华
        /// Update date:2009-07-16
        /// Desc.:更改重载是否开启事务操作，本方法调用是否开启事务方法，此为默认不开启事务
        /// </summary>
        public void Update(XH_CapitalAccountTableInfo model)
        {
            Database db = DatabaseFactory.CreateDatabase();
            Update(model, db, null);
        }
        /// <summary>
        /// 开启事务更新数据
        /// Create BY:李健华
        /// Create date:2009-07-16
       /// </summary>
        /// <param name="model">要更新的实体</param>
        /// <param name="db"></param>
        /// <param name="trm">如果为null不开启事务</param>
        public void Update(XH_CapitalAccountTableInfo model, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=@AvailableCapital,");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("BalanceOfTheDay=@BalanceOfTheDay,");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("FreezeCapitalTotal=@FreezeCapitalTotal,");
            //strSql.Append("CapitalBalance=@CapitalBalance,");
            strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("HasDoneProfitLossTotal=@HasDoneProfitLossTotal");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            //db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotal", DbType.Decimal, model.HasDoneProfitLossTotal);
            if (trm != null)
            {
                db.ExecuteNonQuery(dbCommand, trm);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand);
            }
        }
        #endregion

        #region 仅更新变化字段
        /// <summary>
        /// 仅更新变化字段
        /// </summary>
        /// <param name="capitalAccountLogo"></param>
        /// <param name="availableCapital"></param>
        /// <param name="freezeCapitalTotal"></param>
        /// <param name="todayOutInCapital"></param>
        /// <param name="hasDoneProfitLossTotal"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void Update(int capitalAccountLogo, decimal availableCapital, decimal freezeCapitalTotal,
                           decimal todayOutInCapital, decimal hasDoneProfitLossTotal, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=@AvailableCapital,");
            strSql.Append("FreezeCapitalTotal=@FreezeCapitalTotal,");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("HasDoneProfitLossTotal=@HasDoneProfitLossTotal");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, availableCapital);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, capitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, freezeCapitalTotal);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, todayOutInCapital);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotal", DbType.Decimal, hasDoneProfitLossTotal);
            db.ExecuteNonQuery(dbCommand, transaction);
        }
        #endregion

        #region 将新值累加到原来的字段上(+=)
        /// <summary>
        /// 将新值累加到原来的字段上(+=)
        /// </summary>
        /// <param name="capitalAccountLogo"></param>
        /// <param name="availableCapital"></param>
        /// <param name="freezeCapitalTotal"></param>
        /// <param name="todayOutInCapital"></param>
        /// <param name="hasDoneProfitLossTotal"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void AddUpdate(int capitalAccountLogo, decimal availableCapital, decimal freezeCapitalTotal,
                           decimal todayOutInCapital, decimal hasDoneProfitLossTotal, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_CapitalAccountTable set ");
            strSql.Append("AvailableCapital=AvailableCapital+(@AvailableCapital),");
            strSql.Append("FreezeCapitalTotal=FreezeCapitalTotal+(@FreezeCapitalTotal),");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("HasDoneProfitLossTotal=HasDoneProfitLossTotal+(@HasDoneProfitLossTotal)");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, availableCapital);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, capitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, freezeCapitalTotal);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, todayOutInCapital);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotal", DbType.Decimal, hasDoneProfitLossTotal);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        #endregion

        #region 删除一条数据
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CapitalAccountLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_CapitalAccountTable ");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, CapitalAccountLogo);
            db.ExecuteNonQuery(dbCommand);
        }
        #endregion

        #region 得到一个对象实体
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_CapitalAccountTableInfo GetModel(int CapitalAccountLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AvailableCapital,CapitalAccountLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,TradeCurrencyType,HasDoneProfitLossTotal from XH_CapitalAccountTable ");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, CapitalAccountLogo);
            XH_CapitalAccountTableInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<XH_CapitalAccountTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AvailableCapital,CapitalAccountLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,TradeCurrencyType,HasDoneProfitLossTotal ");
            strSql.Append(" FROM XH_CapitalAccountTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();
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
        #endregion

        #region 获得所有数据列表
        /// <summary>
        /// 获得所有数据列表
        /// </summary>
        public List<XH_CapitalAccountTableInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select AvailableCapital,CapitalAccountLogo,UserAccountDistributeLogo,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,TradeCurrencyType,HasDoneProfitLossTotal ");
            strSql.Append(" FROM XH_CapitalAccountTable ");

            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();
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
        #endregion

        #region 对象实体绑定数据
        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public XH_CapitalAccountTableInfo ReaderBind(IDataReader dataReader)
        {
            XH_CapitalAccountTableInfo model = new XH_CapitalAccountTableInfo();
            object ojb;
            ojb = dataReader["AvailableCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapital = (decimal)ojb;
            }
            ojb = dataReader["CapitalAccountLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogo = (int)ojb;
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
            ojb = dataReader["TradeCurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyType = (int)ojb;
            }
            ojb = dataReader["HasDoneProfitLossTotal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HasDoneProfitLossTotal = (decimal)ojb;
            }
            return model;
        }
        #endregion

        #region 根据用户ID和密码查询用户所拥有的现货资金账号明细
        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的现货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">密码</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<XH_CapitalAccountTableInfo> GetListByUserIDAndPwd(string userID, string pwd, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserAndPwd), userID, pwd));
        }
        #endregion

        #region 根据用户ID查询用户所拥有的现货资金账号明细
        /// <summary>
        /// 根据用户ID查询用户所拥有的现货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<XH_CapitalAccountTableInfo> GetListByUserID(string userID, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserID), userID));
        }
        #endregion

        #region  根据现货资金账号查询现货资金账号明细
        /// <summary>
        /// 根据现货资金账号查询现货资金账号明细
        /// </summary>
        ///<param name="account">现货资金账号</param>
        /// <param name="type">币种</param>
        /// <returns></returns>
        public List<XH_CapitalAccountTableInfo> GetListByAccount(string account, QueryType.QueryCurrencyType type)
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
                    strByWhere = "  UserAccountDistributeLogo in( select useraccountdistributelogo from UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital + "')  and userid='{0}' )";
                    break;
                case QueryType.QueryWhereType.ByUserAndPwd:
                    strByWhere = "   UserAccountDistributeLogo in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital + "') and userid in (select userid from dbo.UA_UserBasicInformationTable where  userid='{1}' And  Password ='{0}' ))";
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
        #endregion

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
            strSql.Append("UPDATE [XH_CapitalAccountTable]   SET ");
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