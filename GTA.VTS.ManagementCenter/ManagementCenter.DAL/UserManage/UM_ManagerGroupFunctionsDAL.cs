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
    /// 描述：管理员组可用功能表 数据访问类UM_ManagerGroupFunctionsDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
	public class UM_ManagerGroupFunctionsDAL
	{
		public UM_ManagerGroupFunctionsDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(ManageGroupFuntctiosID)+1 from UM_ManagerGroupFunctions";
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
		public bool Exists(int ManageGroupFuntctiosID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from UM_ManagerGroupFunctions where ManageGroupFuntctiosID=@ManageGroupFuntctiosID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ManageGroupFuntctiosID", DbType.Int32,ManageGroupFuntctiosID);
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
		public int Add(ManagementCenter.Model.UM_ManagerGroupFunctions model,DbTransaction tran,Database db)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UM_ManagerGroupFunctions(");
			strSql.Append("FunctionID,ManagerGroupID)");

			strSql.Append(" values (");
			strSql.Append("@FunctionID,@ManagerGroupID)");
			strSql.Append(";select @@IDENTITY");
			if(db==null) db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "FunctionID", DbType.Int32, model.FunctionID);
			db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, model.ManagerGroupID);
			int result;
			object obj;
            if(tran==null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand,tran);
            }
			if(!int.TryParse(obj.ToString(),out result))
			{
				return 0;
			}
			return result;
		}
        /// <summary>
        /// 添加管理员组可用功能
        /// </summary>
        /// <param name="model">管理员组可用功能 实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.UM_ManagerGroupFunctions model)
        {
            return Add(model, null, null);
        }

	    /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_ManagerGroupFunctions model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UM_ManagerGroupFunctions set ");
			strSql.Append("FunctionID=@FunctionID,");
			strSql.Append("ManagerGroupID=@ManagerGroupID");
			strSql.Append(" where ManageGroupFuntctiosID=@ManageGroupFuntctiosID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ManageGroupFuntctiosID", DbType.Int32, model.ManageGroupFuntctiosID);
			db.AddInParameter(dbCommand, "FunctionID", DbType.Int32, model.FunctionID);
			db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, model.ManagerGroupID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ManageGroupFuntctiosID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_ManagerGroupFunctions ");
			strSql.Append(" where ManageGroupFuntctiosID=@ManageGroupFuntctiosID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ManageGroupFuntctiosID", DbType.Int32,ManageGroupFuntctiosID);
			db.ExecuteNonQuery(dbCommand);

		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteByManagerGroupID(int ManagerGroupID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_ManagerGroupFunctions ");
            strSql.Append(" where ManagerGroupID=@ManagerGroupID ");
            if(db==null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, ManagerGroupID);
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
		public ManagementCenter.Model.UM_ManagerGroupFunctions GetModel(int ManageGroupFuntctiosID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select ManageGroupFuntctiosID,FunctionID,ManagerGroupID,FunctionName='' from UM_ManagerGroupFunctions ");
			strSql.Append(" where ManageGroupFuntctiosID=@ManageGroupFuntctiosID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ManageGroupFuntctiosID", DbType.Int32,ManageGroupFuntctiosID);
			ManagementCenter.Model.UM_ManagerGroupFunctions model=null;
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
			strSql.Append("select ManageGroupFuntctiosID,FunctionID,ManagerGroupID ");
			strSql.Append(" FROM UM_ManagerGroupFunctions ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_ManagerGroupFunctions");
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
		public List<ManagementCenter.Model.UM_ManagerGroupFunctions> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select ManageGroupFuntctiosID,UM_ManagerGroupFunctions.FunctionID,ManagerGroupID,FunctionName ");
            strSql.Append(" FROM UM_ManagerGroupFunctions,UM_Functions where UM_ManagerGroupFunctions.FunctionID=UM_Functions.FunctionID ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" AND "+strWhere);
			}
			List<ManagementCenter.Model.UM_ManagerGroupFunctions> list = new List<ManagementCenter.Model.UM_ManagerGroupFunctions>();
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
		public ManagementCenter.Model.UM_ManagerGroupFunctions ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.UM_ManagerGroupFunctions model=new ManagementCenter.Model.UM_ManagerGroupFunctions();
			object ojb; 
			ojb = dataReader["ManageGroupFuntctiosID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.ManageGroupFuntctiosID=(int)ojb;
			}
			ojb = dataReader["FunctionID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.FunctionID=(int)ojb;
			}
			ojb = dataReader["ManagerGroupID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.ManagerGroupID=(int)ojb;
			}
            model.FunctionName = dataReader["FunctionName"].ToString();
			return model;
		}


        /// <summary>
        ///根据管理员ID获取权限列表
        /// </summary>
        public List<ManagementCenter.Model.UM_ManagerGroupFunctions> GetRightListByManagerID(int ManagerID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("select A.ManageGroupFuntctiosID,A.FunctionID,A.ManagerGroupID,FunctionName='' ");
            strSql.Append(" FROM UM_ManagerGroupFunctions as A,UM_ManagerBeloneToGroup as B ");
            strSql.Append(" WHERE A.ManagerGroupID=B.ManagerGroupID AND B.UserID=@UserID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, ManagerID);
            List<ManagementCenter.Model.UM_ManagerGroupFunctions> list = new List<ManagementCenter.Model.UM_ManagerGroupFunctions>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

		#endregion  成员方法
	}
}

