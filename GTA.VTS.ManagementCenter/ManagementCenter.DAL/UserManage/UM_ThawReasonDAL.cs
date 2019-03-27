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
    ///  描述：解冻原因表  数据访问类UM_ThawReasonDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
	public class UM_ThawReasonDAL
	{
		public UM_ThawReasonDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(ThawReasonID)+1 from UM_ThawReason";
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
		public bool Exists(int ThawReasonID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from UM_ThawReason where ThawReasonID=@ThawReasonID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ThawReasonID", DbType.Int32,ThawReasonID);
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
		public int Add(ManagementCenter.Model.UM_ThawReason model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UM_ThawReason(");
			strSql.Append("ThawReason,ThawReasonTime,DealerAccoutID)");

			strSql.Append(" values (");
			strSql.Append("@ThawReason,@ThawReasonTime,@DealerAccoutID)");
			strSql.Append(";select @@IDENTITY");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ThawReason", DbType.String, model.ThawReason);
			db.AddInParameter(dbCommand, "ThawReasonTime", DbType.DateTime, model.ThawReasonTime);
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String, model.DealerAccoutID);
			int result;
			object obj = db.ExecuteScalar(dbCommand);
			if(!int.TryParse(obj.ToString(),out result))
			{
				return 0;
			}
			return result;
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_ThawReason model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UM_ThawReason set ");
			strSql.Append("ThawReason=@ThawReason,");
			strSql.Append("ThawReasonTime=@ThawReasonTime,");
			strSql.Append("DealerAccoutID=@DealerAccoutID");
			strSql.Append(" where ThawReasonID=@ThawReasonID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ThawReasonID", DbType.Int32, model.ThawReasonID);
			db.AddInParameter(dbCommand, "ThawReason", DbType.String, model.ThawReason);
			db.AddInParameter(dbCommand, "ThawReasonTime", DbType.DateTime, model.ThawReasonTime);
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String, model.DealerAccoutID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ThawReasonID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_ThawReason ");
			strSql.Append(" where ThawReasonID=@ThawReasonID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ThawReasonID", DbType.Int32,ThawReasonID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_ThawReason GetModel(int ThawReasonID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ThawReasonID,ThawReason,ThawReasonTime,DealerAccoutID from UM_ThawReason ");
			strSql.Append(" where ThawReasonID=@ThawReasonID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ThawReasonID", DbType.Int32,ThawReasonID);
			ManagementCenter.Model.UM_ThawReason model=null;
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
			strSql.Append("select ThawReasonID,ThawReason,ThawReasonTime,DealerAccoutID ");
			strSql.Append(" FROM UM_ThawReason ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_ThawReason");
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
		public List<ManagementCenter.Model.UM_ThawReason> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ThawReasonID,ThawReason,ThawReasonTime,DealerAccoutID ");
			strSql.Append(" FROM UM_ThawReason ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.UM_ThawReason> list = new List<ManagementCenter.Model.UM_ThawReason>();
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
		public ManagementCenter.Model.UM_ThawReason ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.UM_ThawReason model=new ManagementCenter.Model.UM_ThawReason();
			object ojb; 
			ojb = dataReader["ThawReasonID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.ThawReasonID=(int)ojb;
			}
			model.ThawReason=dataReader["ThawReason"].ToString();
			ojb = dataReader["ThawReasonTime"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.ThawReasonTime=(DateTime)ojb;
			}
			model.DealerAccoutID=dataReader["DealerAccoutID"].ToString();
			return model;
		}

        /// <summary>
        /// 删除记录根据交易员ID
        /// </summary>
        public void DeleteByUserID(int UserID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete  from UM_ThawReason where DealerAccoutID in  ");
            strSql.Append(" (select DealerAccoutID from UM_DealerAccount where UserID=@UserID) ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }

		#endregion  成员方法
	}
}

