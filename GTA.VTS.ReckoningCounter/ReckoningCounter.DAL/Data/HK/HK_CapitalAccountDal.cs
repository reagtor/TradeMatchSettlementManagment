using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Entity.Model.QueryFilter;

namespace ReckoningCounter.DAL.Data.HK
{
    /// <summary>
    /// 港股资金账户数据访问类HK_CapitalAccountDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// Update by:董鹏
    /// Update date:2009-12-25
    /// Desc.: 添加个性化资金设置的方法
    /// </summary>
    public class HK_CapitalAccountDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_CapitalAccountDal()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// <param name="CapitalAccountLogo"></param>
        /// </summary>
        public bool Exists(int CapitalAccountLogo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_CapitalAccount where CapitalAccountLogo=@CapitalAccountLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, CapitalAccountLogo);
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
        public int Add(HK_CapitalAccountInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_CapitalAccount(");
            strSql.Append("UserAccountDistributeLogo,TradeCurrencyType,AvailableCapital,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,HasDoneProfitLossTotal)");

            strSql.Append(" values (");
            strSql.Append("@UserAccountDistributeLogo,@TradeCurrencyType,@AvailableCapital,@BalanceOfTheDay,@TodayOutInCapital,@FreezeCapitalTotal,@CapitalBalance,@HasDoneProfitLossTotal)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
            db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotal", DbType.Decimal, model.HasDoneProfitLossTotal);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 开是否开启事务增加一条数据
        /// <param name="model">要插入的用户账号对象</param>
        /// <param name="db">操作数据对象</param>
        /// <param name="trm">开启事务对象，如果为null不开启</param>
        /// </summary>
        public int Add(HK_CapitalAccountInfo model, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_CapitalAccount (");
            strSql.Append("AvailableCapital,UserAccountDistributeLogo,BalanceOfTheDay,");
            strSql.Append(" TodayOutInCapital,FreezeCapitalTotal,TradeCurrencyType,HasDoneProfitLossTotal)");
            strSql.Append(" values (");
            strSql.Append("@AvailableCapital,@UserAccountDistributeLogo,@BalanceOfTheDay,");
            strSql.Append("  @TodayOutInCapital,@FreezeCapitalTotal,@TradeCurrencyType,@HasDoneProfitLossTotal)");
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

 
        #region 更新一条数据
        /// <summary>
        /// 更新一条数据
        /// Update BY:李健华
        /// Update date:2009-10-20
        /// Desc.:更改重载是否开启事务操作，本方法调用是否开启事务方法，此为默认不开启事务
        /// </summary>
        public void Update(HK_CapitalAccountInfo model)
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
        public void Update(HK_CapitalAccountInfo model, Database db, DbTransaction trm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_CapitalAccount set ");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("AvailableCapital=@AvailableCapital,");
            strSql.Append("BalanceOfTheDay=@BalanceOfTheDay,");
            strSql.Append("TodayOutInCapital=@TodayOutInCapital,");
            strSql.Append("FreezeCapitalTotal=@FreezeCapitalTotal,");
            //strSql.Append("CapitalBalance=@CapitalBalance,");
            strSql.Append("HasDoneProfitLossTotal=@HasDoneProfitLossTotal");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "AvailableCapital", DbType.Decimal, model.AvailableCapital);
            db.AddInParameter(dbCommand, "BalanceOfTheDay", DbType.Decimal, model.BalanceOfTheDay);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "FreezeCapitalTotal", DbType.Decimal, model.FreezeCapitalTotal);
           // db.AddInParameter(dbCommand, "CapitalBalance", DbType.Decimal, model.CapitalBalance);
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
            strSql.Append("update HK_CapitalAccount set ");
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
            strSql.Append("update HK_CapitalAccount set ");
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

        /// <summary>
        /// 删除一条数据
        /// <param name="CapitalAccountLogo"></param>
        /// </summary>
        public void Delete(int CapitalAccountLogo)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_CapitalAccount ");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, CapitalAccountLogo);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// <param name="CapitalAccountLogo"></param>
        /// </summary>
        public HK_CapitalAccountInfo GetModel(int CapitalAccountLogo)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalAccountLogo,UserAccountDistributeLogo,TradeCurrencyType,AvailableCapital,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,HasDoneProfitLossTotal from HK_CapitalAccount ");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, CapitalAccountLogo);
            HK_CapitalAccountInfo model = null;
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
        /// <param name="strWhere"></param>
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalAccountLogo,UserAccountDistributeLogo,TradeCurrencyType,AvailableCapital,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,HasDoneProfitLossTotal ");
            strSql.Append(" FROM HK_CapitalAccount ");
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
        public List<HK_CapitalAccountInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalAccountLogo,UserAccountDistributeLogo,TradeCurrencyType,AvailableCapital,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,HasDoneProfitLossTotal ");
            strSql.Append(" FROM HK_CapitalAccount ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HK_CapitalAccountInfo> list = new List<HK_CapitalAccountInfo>();
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
        public List<HK_CapitalAccountInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalAccountLogo,UserAccountDistributeLogo,TradeCurrencyType,AvailableCapital,BalanceOfTheDay,TodayOutInCapital,FreezeCapitalTotal,CapitalBalance,HasDoneProfitLossTotal ");
            strSql.Append(" FROM HK_CapitalAccount ");
            //if (strWhere.Trim() != "")
            //{
            //    strSql.Append(" where " + strWhere);
            //}
            List<HK_CapitalAccountInfo> list = new List<HK_CapitalAccountInfo>();
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
        public HK_CapitalAccountInfo ReaderBind(IDataReader dataReader)
        {
            HK_CapitalAccountInfo model = new HK_CapitalAccountInfo();
            object ojb;
            ojb = dataReader["CapitalAccountLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogo = (int)ojb;
            }
            model.UserAccountDistributeLogo = dataReader["UserAccountDistributeLogo"].ToString();
            ojb = dataReader["TradeCurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyType = (int)ojb;
            }
            ojb = dataReader["AvailableCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapital = (decimal)ojb;
            }
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
            ojb = dataReader["HasDoneProfitLossTotal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HasDoneProfitLossTotal = (decimal)ojb;
            }
            return model;
        }


        #region 根据用户ID和密码查询用户所拥有的现货资金账号明细
        
        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的现货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">密码</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<HK_CapitalAccountInfo> GetListByUserIDAndPwd(string userID, string pwd, QueryType.QueryCurrencyType type)
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
        public List<HK_CapitalAccountInfo> GetListByUserID(string userID, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserID), userID));
        }
        #endregion

        #region  根据现货资金账号查询现货资金账号明细
       
        /// <summary>
        /// 根据现货资金账号查询现货资金账号明细
        /// </summary>
        /// <param name="account">现货资金账号</param>
        /// <param name="type">币种</param>
        /// <returns></returns>
        public List<HK_CapitalAccountInfo> GetListByAccount(string account, QueryType.QueryCurrencyType type)
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
            strSql.Append("UPDATE [HK_CapitalAccount]   SET ");
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
