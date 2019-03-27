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
    /// 描述：帐号表 数据访问类UM_DealerAccountDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
	public class UM_DealerAccountDAL
	{
		public UM_DealerAccountDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string DealerAccoutID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from UM_DealerAccount where DealerAccoutID=@DealerAccoutID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String,DealerAccoutID);
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
		public void Add(ManagementCenter.Model.UM_DealerAccount model,DbTransaction tran,Database db)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UM_DealerAccount(");
			strSql.Append("IsDo,UserID,DealerAccoutID,AccountTypeID)");

			strSql.Append(" values (");
			strSql.Append("@IsDo,@UserID,@DealerAccoutID,@AccountTypeID)");

			if(db==null)db = DatabaseFactory.CreateDatabase();

			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "IsDo", DbType.Boolean, model.IsDo);
			db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String, model.DealerAccoutID);
			db.AddInParameter(dbCommand, "AccountTypeID", DbType.Int32, model.AccountTypeID);
            if (tran!=null)
            {
                db.ExecuteNonQuery(dbCommand,tran);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand);
            }
			
		}
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.UM_DealerAccount model)
        {
            Add(model, null, null);
        }


		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_DealerAccount model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UM_DealerAccount set ");
			strSql.Append("IsDo=@IsDo,");
			strSql.Append("UserID=@UserID,");
			strSql.Append("AccountTypeID=@AccountTypeID");
			strSql.Append(" where DealerAccoutID=@DealerAccoutID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "IsDo", DbType.Boolean, model.IsDo);
			db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String, model.DealerAccoutID);
			db.AddInParameter(dbCommand, "AccountTypeID", DbType.Int32, model.AccountTypeID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(string DealerAccoutID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_DealerAccount ");
			strSql.Append(" where DealerAccoutID=@DealerAccoutID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String,DealerAccoutID);
			db.ExecuteNonQuery(dbCommand);

		}

        /// <summary>
        /// 删除数据根据用户ID
        /// </summary>
        public void DeleteByUserID(int UserID,DbTransaction tran,Database db)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_DealerAccount ");
            strSql.Append(" where UserID=@UserID ");
            if(db==null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            if(tran==null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand,tran);
            }
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_DealerAccount GetModel(string DealerAccoutID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select IsDo,UserID,DealerAccoutID,AccountTypeID from UM_DealerAccount ");
			strSql.Append(" where DealerAccoutID=@DealerAccoutID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String,DealerAccoutID);
			ManagementCenter.Model.UM_DealerAccount model=null;
			using (IDataReader dataReader = db.ExecuteReader(dbCommand))
			{
				if(dataReader.Read())
				{
					model=ReaderBind(dataReader);
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select IsDo,UserID,DealerAccoutID,AccountTypeID ");
			strSql.Append(" FROM UM_DealerAccount ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_DealerAccount");
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
		public List<ManagementCenter.Model.UM_DealerAccount> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select IsDo,UserID,DealerAccoutID,AccountTypeID ");
			strSql.Append(" FROM UM_DealerAccount ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.UM_DealerAccount> list = new List<ManagementCenter.Model.UM_DealerAccount>();
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
		public ManagementCenter.Model.UM_DealerAccount ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.UM_DealerAccount model=new ManagementCenter.Model.UM_DealerAccount();
			object ojb; 
			ojb = dataReader["IsDo"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.IsDo=(bool)ojb;
			}
			ojb = dataReader["UserID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.UserID=(int)ojb;
			}
			model.DealerAccoutID=dataReader["DealerAccoutID"].ToString();
			ojb = dataReader["AccountTypeID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.AccountTypeID=(int)ojb;
			}
			return model;
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_DealerAccount GetModelByUserIDAndType(int UserID, int AccountAttributionType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select A.* FROM UM_DealerAccount as A,UM_AccountType as B ");
            strSql.Append(" WHERE A.AccountTypeID=B.AccountTypeID AND B.AccountAttributionType=@AccountAttributionType AND A.UserID=@UserID   ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountAttributionType", DbType.Int32, AccountAttributionType);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            ManagementCenter.Model.UM_DealerAccount model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }

		#endregion  成员方法
	}
}

