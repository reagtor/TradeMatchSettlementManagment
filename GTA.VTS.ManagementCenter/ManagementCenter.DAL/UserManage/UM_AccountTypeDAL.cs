using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用
namespace ManagementCenter.DAL
{
	/// <summary>
    /// 描述：帐号类型 数据访问类UM_AccountTypeDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
	/// </summary>
	public class UM_AccountTypeDAL
	{
		public UM_AccountTypeDAL()
		{}
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(AccountTypeID)+1 from UM_AccountType";
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
        public bool Exists(int AccountTypeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UM_AccountType where AccountTypeID=@AccountTypeID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountTypeID", DbType.Int32, AccountTypeID);
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
        /// </summary>
        public void Add(ManagementCenter.Model.UM_AccountType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UM_AccountType(");
            strSql.Append("AccountTypeID,AccountName,AccountAttributionType,ReMark,ConnectHoldID)");

            strSql.Append(" values (");
            strSql.Append("@AccountTypeID,@AccountName,@AccountAttributionType,@ReMark,@ConnectHoldID)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountTypeID", DbType.Int32, model.AccountTypeID);
            db.AddInParameter(dbCommand, "AccountName", DbType.String, model.AccountName);
            db.AddInParameter(dbCommand, "AccountAttributionType", DbType.Int32, model.AccountAttributionType);
            db.AddInParameter(dbCommand, "ReMark", DbType.String, model.ReMark);
            db.AddInParameter(dbCommand, "ConnectHoldID", DbType.Int32, model.ConnectHoldID);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.UM_AccountType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UM_AccountType set ");
            strSql.Append("AccountName=@AccountName,");
            strSql.Append("AccountAttributionType=@AccountAttributionType,");
            strSql.Append("ReMark=@ReMark,");
            strSql.Append("ConnectHoldID=@ConnectHoldID ");
            strSql.Append(" where AccountTypeID=@AccountTypeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountTypeID", DbType.Int32, model.AccountTypeID);
            db.AddInParameter(dbCommand, "AccountName", DbType.String, model.AccountName);
            db.AddInParameter(dbCommand, "AccountAttributionType", DbType.Int32, model.AccountAttributionType);
            db.AddInParameter(dbCommand, "ReMark", DbType.String, model.ReMark);
            db.AddInParameter(dbCommand, "ConnectHoldID", DbType.Int32, model.ConnectHoldID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int AccountTypeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_AccountType ");
            strSql.Append(" where AccountTypeID=@AccountTypeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountTypeID", DbType.Int32, AccountTypeID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_AccountType GetModel(int AccountTypeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AccountTypeID,AccountName,AccountAttributionType,ReMark,ConnectHoldID from UM_AccountType ");
            strSql.Append(" where AccountTypeID=@AccountTypeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountTypeID", DbType.Int32, AccountTypeID);
            ManagementCenter.Model.UM_AccountType model = null;
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
            strSql.Append("select AccountTypeID,AccountName,AccountAttributionType,ReMark,ConnectHoldID ");
            strSql.Append(" FROM UM_AccountType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("UP_GetRecordByPage");
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_AccountType");
            db.AddInParameter(dbCommand, "fldName", DbType.AnsiString, "ID");
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddInParameter(dbCommand, "IsReCount", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "OrderType", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "strWhere", DbType.AnsiString, strWhere);
            return db.ExecuteDataSet(dbCommand);
        }*/

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<ManagementCenter.Model.UM_AccountType> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AccountTypeID,AccountName,AccountAttributionType,ReMark,ConnectHoldID ");
            strSql.Append(" FROM UM_AccountType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.UM_AccountType> list = new List<ManagementCenter.Model.UM_AccountType>();
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
        public ManagementCenter.Model.UM_AccountType ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.UM_AccountType model = new ManagementCenter.Model.UM_AccountType();
            object ojb;
            ojb = dataReader["AccountTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountTypeID = (int)ojb;
            }
            model.AccountName = dataReader["AccountName"].ToString();
            ojb = dataReader["AccountAttributionType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountAttributionType = (int)ojb;
            }
            model.ReMark = dataReader["ReMark"].ToString();
            ojb = dataReader["ConnectHoldID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ConnectHoldID = (int)ojb;
            }
            return model;
        }

        #endregion  成员方法
	}
}

