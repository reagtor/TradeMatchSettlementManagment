using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用
namespace ManagementCenter.DAL
{
	/// <summary>
    /// 描述：品种类型权限表 数据访问类UM_DealerTradeBreedClassDAL。
    /// 作者：熊晓凌  修改：刘书伟
    /// 日期：2008-11-18  修改日期：2009-11-04
    /// </summary>
	public class UM_DealerTradeBreedClassDAL
	{
		public UM_DealerTradeBreedClassDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(DealerTradeBreedClassID)+1 from UM_DealerTradeBreedClass";
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
		public bool Exists(int DealerTradeBreedClassID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from UM_DealerTradeBreedClass where DealerTradeBreedClassID=@DealerTradeBreedClassID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "DealerTradeBreedClassID", DbType.Int32,DealerTradeBreedClassID);
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
		public int Add(ManagementCenter.Model.UM_DealerTradeBreedClass model,DbTransaction tran, Database db)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UM_DealerTradeBreedClass(");
			strSql.Append("BreedClassID,UserID)");

			strSql.Append(" values (");
			strSql.Append("@BreedClassID,@UserID)");
			strSql.Append(";select @@IDENTITY");
			if(db==null) db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
			db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
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
        /// 添加品种类型权限
        /// </summary>
        /// <param name="model">品种类型权限实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.UM_DealerTradeBreedClass model)
        {
            return Add(model, null, null);
        }

	    /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_DealerTradeBreedClass model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UM_DealerTradeBreedClass set ");
			strSql.Append("BreedClassID=@BreedClassID,");
			strSql.Append("UserID=@UserID");
			strSql.Append(" where DealerTradeBreedClassID=@DealerTradeBreedClassID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
			db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
			db.AddInParameter(dbCommand, "DealerTradeBreedClassID", DbType.Int32, model.DealerTradeBreedClassID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int DealerTradeBreedClassID,DbTransaction tran, Database db)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_DealerTradeBreedClass ");
			strSql.Append(" where DealerTradeBreedClassID=@DealerTradeBreedClassID ");
            if (db==null) db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "DealerTradeBreedClassID", DbType.Int32,DealerTradeBreedClassID);
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
        /// 根据品种权限ID删除品种权限信息
        /// </summary>
        /// <param name="DealerTradeBreedClassID">品种权限ID</param>
        public void Delete(int DealerTradeBreedClassID)
        {
            Delete(DealerTradeBreedClassID,null,null);
        }

        /// <summary>
        /// 根据品种ID，删除品种权限信息
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool DeleteDealerTradeByBreedClassID(int BreedClassID, DbTransaction tran, Database db)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_DealerTradeBreedClass ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
            return true;
        }

        /// <summary>
        /// 根据品种ID，删除品种权限信息
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool DeleteDealerTradeByBreedClassID(int BreedClassID)
        {
            return DeleteDealerTradeByBreedClassID(BreedClassID, null, null);
        }

	    /// <summary>
        /// 删除数据根据用户ID
        /// </summary>
        public void DeleteByUserID(int UserID,DbTransaction tran, Database db)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_DealerTradeBreedClass ");
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
		public ManagementCenter.Model.UM_DealerTradeBreedClass GetModel(int DealerTradeBreedClassID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select BreedClassID,UserID,DealerTradeBreedClassID from UM_DealerTradeBreedClass ");
			strSql.Append(" where DealerTradeBreedClassID=@DealerTradeBreedClassID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "DealerTradeBreedClassID", DbType.Int32,DealerTradeBreedClassID);
			ManagementCenter.Model.UM_DealerTradeBreedClass model=null;
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
			strSql.Append("select BreedClassID,UserID,DealerTradeBreedClassID ");
			strSql.Append(" FROM UM_DealerTradeBreedClass ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_DealerTradeBreedClass");
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
		public List<ManagementCenter.Model.UM_DealerTradeBreedClass> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select BreedClassID,UserID,DealerTradeBreedClassID ");
			strSql.Append(" FROM UM_DealerTradeBreedClass ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.UM_DealerTradeBreedClass> list = new List<ManagementCenter.Model.UM_DealerTradeBreedClass>();
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
		public ManagementCenter.Model.UM_DealerTradeBreedClass ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.UM_DealerTradeBreedClass model=new ManagementCenter.Model.UM_DealerTradeBreedClass();
			object ojb; 
			ojb = dataReader["BreedClassID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.BreedClassID=(int)ojb;
			}
			ojb = dataReader["UserID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.UserID=(int)ojb;
			}
			ojb = dataReader["DealerTradeBreedClassID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.DealerTradeBreedClassID=(int)ojb;
			}
			return model;
		}

        /// <summary>
        /// 获取用户的品种交易权限
        /// </summary>
        /// <param name="UserID">交易员ID</param>
        /// <returns></returns>
        public DataSet GetUserBreedClassRight(int UserID)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("Select D.UserID,D.DealerTradeBreedClassID,B.BreedClassID,B.BreedClassName,C.BourseTypeName,c.BourseTypeID From CM_BreedClass as B ");
            strSql.Append("right Join CM_BourseType as C On C.BourseTypeID=B.BourseTypeID ");
            strSql.Append("Left Join ");
            //strSql.Append("(Select * From UM_DealerTradeBreedClass  where UserID=@UserID) AS D On D.BreedClassID=B.BreedClassID order by c.BourseTypeID");
            strSql.Append("(Select * From UM_DealerTradeBreedClass  where UserID=@UserID) AS D On D.BreedClassID=B.BreedClassID where  B.DELETESTATE<>1 order by c.BourseTypeID");

			Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            return db.ExecuteDataSet(dbCommand);
            
        }

	    #endregion  成员方法
	}
}

