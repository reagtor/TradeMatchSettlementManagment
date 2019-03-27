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
    /// 描述：管理员_所属组表 数据访问类UM_ManagerBeloneToGroupDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
	public class UM_ManagerBeloneToGroupDAL
	{
		public UM_ManagerBeloneToGroupDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(UserID)+1 from UM_ManagerBeloneToGroup";
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
		public bool Exists(int UserID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from UM_ManagerBeloneToGroup where UserID=@UserID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "UserID", DbType.Int32,UserID);
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
        public void Add(ManagementCenter.Model.UM_ManagerBeloneToGroup model,DbTransaction tran,Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UM_ManagerBeloneToGroup(");
            strSql.Append("UserID,ManagerGroupID)");

            strSql.Append(" values (");
            strSql.Append("@UserID,@ManagerGroupID)");

            if(db==null) db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
            db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, model.ManagerGroupID);
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
        /// 添加管理员所属组信息
        /// </summary>
        /// <param name="model">管理员所属组实体</param>
        public void Add(ManagementCenter.Model.UM_ManagerBeloneToGroup model)
        {
            Add(model, null, null);
        }

	    /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_ManagerBeloneToGroup model,DbTransaction tran,Database db)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UM_ManagerBeloneToGroup set ");
			strSql.Append("ManagerGroupID=@ManagerGroupID");
			strSql.Append(" where UserID=@UserID ");
			if(db==null) db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
			db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, model.ManagerGroupID);
            if (tran==null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand,tran);
            }
		}

        /// <summary>
        /// 更新管理员所属组
        /// </summary>
        /// <param name="model">管理员所属组实体</param>
        public void Update(ManagementCenter.Model.UM_ManagerBeloneToGroup model)
        {
            Update(model,null,null);
        }

	    /// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int UserID,DbTransaction tran,Database db)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_ManagerBeloneToGroup ");
			strSql.Append(" where UserID=@UserID ");
			if(db==null) db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "UserID", DbType.Int32,UserID);

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
        /// 根据用户ID删除管理员所属组
        /// </summary>
        /// <param name="UserID"></param>
        public void  Delete(int UserID)
        {
            Delete(UserID, null, null);
        }

	    /// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_ManagerBeloneToGroup GetModel(int UserID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserID,ManagerGroupID from UM_ManagerBeloneToGroup ");
			strSql.Append(" where UserID=@UserID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "UserID", DbType.Int32,UserID);
			ManagementCenter.Model.UM_ManagerBeloneToGroup model=null;
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
			strSql.Append("select UserID,ManagerGroupID ");
			strSql.Append(" FROM UM_ManagerBeloneToGroup ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_ManagerBeloneToGroup");
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
		public List<ManagementCenter.Model.UM_ManagerBeloneToGroup> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserID,ManagerGroupID ");
			strSql.Append(" FROM UM_ManagerBeloneToGroup ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.UM_ManagerBeloneToGroup> list = new List<ManagementCenter.Model.UM_ManagerBeloneToGroup>();
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
		public ManagementCenter.Model.UM_ManagerBeloneToGroup ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.UM_ManagerBeloneToGroup model=new ManagementCenter.Model.UM_ManagerBeloneToGroup();
			object ojb; 
			ojb = dataReader["UserID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.UserID=(int)ojb;
			}
			ojb = dataReader["ManagerGroupID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.ManagerGroupID=(int)ojb;
			}
			return model;
		}

		#endregion  成员方法
	}
}

