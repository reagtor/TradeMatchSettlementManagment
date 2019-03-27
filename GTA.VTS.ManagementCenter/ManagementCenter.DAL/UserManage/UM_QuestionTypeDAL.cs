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
    /// 描述：问题类型表 数据访问类UM_QuestionTypeDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
	public class UM_QuestionTypeDAL
	{
		public UM_QuestionTypeDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(QuestionID)+1 from UM_QuestionType";
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
		public bool Exists(int QuestionID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from UM_QuestionType where QuestionID=@QuestionID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "QuestionID", DbType.Int32,QuestionID);
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
		public void Add(ManagementCenter.Model.UM_QuestionType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UM_QuestionType(");
			strSql.Append("QuestionID,Content)");

			strSql.Append(" values (");
			strSql.Append("@QuestionID,@Content)");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "QuestionID", DbType.Int32, model.QuestionID);
			db.AddInParameter(dbCommand, "Content", DbType.String, model.Content);
			db.ExecuteNonQuery(dbCommand);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_QuestionType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UM_QuestionType set ");
			strSql.Append("Content=@Content");
			strSql.Append(" where QuestionID=@QuestionID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "QuestionID", DbType.Int32, model.QuestionID);
			db.AddInParameter(dbCommand, "Content", DbType.String, model.Content);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int QuestionID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_QuestionType ");
			strSql.Append(" where QuestionID=@QuestionID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "QuestionID", DbType.Int32,QuestionID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_QuestionType GetModel(int QuestionID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select QuestionID,Content from UM_QuestionType ");
			strSql.Append(" where QuestionID=@QuestionID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "QuestionID", DbType.Int32,QuestionID);
			ManagementCenter.Model.UM_QuestionType model=null;
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
			strSql.Append("select QuestionID,Content ");
			strSql.Append(" FROM UM_QuestionType ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_QuestionType");
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
		public List<ManagementCenter.Model.UM_QuestionType> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select QuestionID,Content ");
			strSql.Append(" FROM UM_QuestionType ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.UM_QuestionType> list = new List<ManagementCenter.Model.UM_QuestionType>();
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
		public ManagementCenter.Model.UM_QuestionType ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.UM_QuestionType model=new ManagementCenter.Model.UM_QuestionType();
			object ojb; 
			ojb = dataReader["QuestionID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.QuestionID=(int)ojb;
			}
			model.Content=dataReader["Content"].ToString();
			return model;
		}

		#endregion  成员方法
	}
}

