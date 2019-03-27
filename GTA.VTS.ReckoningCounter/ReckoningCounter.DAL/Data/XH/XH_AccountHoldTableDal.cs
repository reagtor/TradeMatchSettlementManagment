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
    /// 数据访问类XH_AccountHoldTableDal。
    /// </summary>
    public class XH_AccountHoldTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int AccountHoldLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_AccountHoldTable where AccountHoldLogoId=@AccountHoldLogoId ");
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
        public  XH_AccountHoldTableInfo GetXhAccountHoldTableWithNoLock(string strHoldAccount, string strCode, int iCurrType)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(
            //    "select AccountHoldLogoId,AvailableAmount,FreezeAmount,UserAccountDistributeLogo,CostPrice,Code,BreakevenPrice,CurrencyTypeId from XH_AccountHoldTable WITH (NOLOCK)");
            strSql.Append("select AccountHoldLogoId,UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice from XH_AccountHoldTable WITH (NOLOCK)");

            strSql.Append(
                " where CurrencyTypeId=@CurrencyTypeId AND UserAccountDistributeLogo=@UserAccountDistributeLogo AND Code=@Code");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strHoldAccount);
            db.AddInParameter(dbCommand, "Code", DbType.String, strCode);
            XH_AccountHoldTableInfo model = null;
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
        public int Add(XH_AccountHoldTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AccountHoldTable(");
            strSql.Append("UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice)");

            strSql.Append(" values (");
            strSql.Append("@UserAccountDistributeLogo,@CurrencyTypeId,@Code,@AvailableAmount,@FreezeAmount,@CostPrice,@BreakevenPrice,@HoldAveragePrice)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
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
        public int Add(XH_AccountHoldTableInfo model, Database db ,DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AccountHoldTable(");
            strSql.Append("UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice)");

            strSql.Append(" values (");
            strSql.Append("@UserAccountDistributeLogo,@CurrencyTypeId,@Code,@AvailableAmount,@FreezeAmount,@CostPrice,@BreakevenPrice,@HoldAveragePrice)");
            strSql.Append(";select @@IDENTITY");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_AccountHoldTableInfo GetXhAccountHoldTable(string strHoldAccount, string strCode, int iCurrType)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(
            //    "select AccountHoldLogoId,AvailableAmount,FreezeAmount,UserAccountDistributeLogo,CostPrice,Code,BreakevenPrice,CurrencyTypeId from XH_AccountHoldTable ");
            strSql.Append("select AccountHoldLogoId,UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice from XH_AccountHoldTable ");

            strSql.Append(
                " where CurrencyTypeId=@CurrencyTypeId AND UserAccountDistributeLogo=@UserAccountDistributeLogo AND Code=@Code");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, iCurrType);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.String, strHoldAccount);
            db.AddInParameter(dbCommand, "Code", DbType.String, strCode);
            XH_AccountHoldTableInfo model = null;
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
        public bool AddRecord(XH_AccountHoldTableInfo model,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AccountHoldTable(");
            strSql.Append(
                "AvailableAmount,FreezeAmount,UserAccountDistributeLogo,CostPrice,Code,BreakevenPrice,CurrencyTypeId,HoldAveragePrice)");

            strSql.Append(" values (");
            strSql.Append(
                "@AvailableAmount,@FreezeAmount,@UserAccountDistributeLogo,@CostPrice,@Code,@BreakevenPrice,@CurrencyTypeId,@HoldAveragePrice)");
            strSql.Append(";select @@IDENTITY");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            int result;
            object obj = db.ExecuteScalar(dbCommand,tm.Transaction);
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
        public void Update(XH_AccountHoldTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AccountHoldTable set ");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("Code=@Code,");
            strSql.Append("AvailableAmount=@AvailableAmount,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("BreakevenPrice=@BreakevenPrice,");
            strSql.Append("HoldAveragePrice=@HoldAveragePrice");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void AddUpdate(XH_AccountHoldTableInfo_Delta model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AccountHoldTable set ");
            strSql.Append("AvailableAmount=AvailableAmount+@AvailableAmount,");
            strSql.Append("FreezeAmount=FreezeAmount+@FreezeAmount");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmountDelta);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmountDelta);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 带事物更新数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="db">Database</param>
        /// <param name="transaction">DbTransaction</param>
        public void Update(XH_AccountHoldTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AccountHoldTable set ");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("Code=@Code,");
            strSql.Append("AvailableAmount=@AvailableAmount,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("BreakevenPrice=@BreakevenPrice,");
            strSql.Append("HoldAveragePrice=@HoldAveragePrice");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void AddUpdate(XH_AccountHoldTableInfo_Delta model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AccountHoldTable set ");
            strSql.Append("AvailableAmount=AvailableAmount+@AvailableAmount,");
            strSql.Append("FreezeAmount=FreezeAmount+@FreezeAmount");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmountDelta);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmountDelta);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 带事物更新数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="db">Database</param>
        /// <param name="transaction">DbTransaction</param>
        public bool UpdateRecord(XH_AccountHoldTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AccountHoldTable set ");
            strSql.Append("AvailableAmount=@AvailableAmount,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("Code=@Code,");
            strSql.Append("BreakevenPrice=@BreakevenPrice,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId,");
            strSql.Append("HoldAveragePrice=@HoldAveragePrice");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            try
            {
                db.ExecuteNonQuery(dbCommand, transaction);
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int AccountHoldLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_AccountHoldTable ");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, AccountHoldLogoId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_AccountHoldTableInfo GetModel(int AccountHoldLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AccountHoldLogoId,UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice from XH_AccountHoldTable ");
            strSql.Append(" where AccountHoldLogoId=@AccountHoldLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, AccountHoldLogoId);
            XH_AccountHoldTableInfo model = null;
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
        public List<XH_AccountHoldTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AccountHoldLogoId,UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice ");
            strSql.Append(" FROM XH_AccountHoldTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_AccountHoldTableInfo> list = new List<XH_AccountHoldTableInfo>();
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
        /// 获得全部数据列表
        /// </summary>
        public List<XH_AccountHoldTableInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AccountHoldLogoId,UserAccountDistributeLogo,CurrencyTypeId,Code,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice ");
            strSql.Append(" FROM XH_AccountHoldTable ");
            
            List<XH_AccountHoldTableInfo> list = new List<XH_AccountHoldTableInfo>();
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
        public XH_AccountHoldTableInfo ReaderBind(IDataReader dataReader)
        {
            XH_AccountHoldTableInfo model = new XH_AccountHoldTableInfo();
            object ojb;
            ojb = dataReader["AccountHoldLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountHoldLogoId = (int)ojb;
            }
            model.UserAccountDistributeLogo = dataReader["UserAccountDistributeLogo"].ToString();
            ojb = dataReader["CurrencyTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeId = (int)ojb;
            }
            model.Code = dataReader["Code"].ToString();
            ojb = dataReader["AvailableAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableAmount = (decimal)ojb;
            }
            ojb = dataReader["FreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeAmount = (decimal)ojb;
            }
            ojb = dataReader["CostPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CostPrice = (decimal)ojb;
            }
            ojb = dataReader["BreakevenPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreakevenPrice = (decimal)ojb;
            }
            ojb = dataReader["HoldAveragePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HoldAveragePrice = (decimal)ojb;
            }
            return model;
        }

        #region 根据用户ID和密码查询用户所拥有的现货持仓账号明细
        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的现货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">密码</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<XH_AccountHoldTableInfo> GetListByUserIDAndPwd(string userID, string pwd, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserAndPwd), userID, pwd));
        }
        #endregion

        #region 根据用户ID查询用户所拥有的现货持仓账号明细
        /// <summary>
        /// 根据用户ID查询用户所拥有的现货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="type">要查询的货币类型</param>
        /// <returns></returns>
        public List<XH_AccountHoldTableInfo> GetListByUserID(string userID, QueryType.QueryCurrencyType type)
        {
            return GetListArray(string.Format(BuildQueryWhere(type, QueryType.QueryWhereType.ByUserID), userID));
        }
        #endregion

        #region  根据现货持仓账号查询现货持仓账号明细
        /// <summary>
        /// 根据现货持仓账号查询现货持仓账号明细
        /// </summary>
        ///<param name="account">现货持仓账号</param>
        /// <param name="type">币种</param>
        /// <returns></returns>
        public List<XH_AccountHoldTableInfo> GetListByAccount(string account, QueryType.QueryCurrencyType type)
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
                    strByWhere = "  UserAccountDistributeLogo in( select useraccountdistributelogo from UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotHold + "')  and userid='{0}' )";
                    break;
                case QueryType.QueryWhereType.ByUserAndPwd:
                    strByWhere = "   UserAccountDistributeLogo in( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in (select accounttypelogo from BD_AccountType where atcid='" + (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotHold + "') and userid in (select userid from dbo.UA_UserBasicInformationTable where  userid='{1}' And  Password ='{0}' ))";
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
                strByWhere += "  And  CurrencyTypeId='" + (int)type + "'";
            }
            return strByWhere;

        }
        #endregion
 
        #endregion  成员方法
    }
}