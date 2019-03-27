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
    ///描述：涨跌停板幅度类型 数据访问类QH_HighLowStopScopeTypeDAL。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
	public class QH_HighLowStopScopeTypeDAL
	{
		public QH_HighLowStopScopeTypeDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(HighLowStopScopeID)+1 from QH_HighLowStopScopeType";
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
		public bool Exists(int HighLowStopScopeID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from QH_HighLowStopScopeType where HighLowStopScopeID=@HighLowStopScopeID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "HighLowStopScopeID", DbType.Int32,HighLowStopScopeID);
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
		public void Add(ManagementCenter.Model.QH_HighLowStopScopeType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into QH_HighLowStopScopeType(");
			strSql.Append("HighLowStopScopeID,HighLowStopScopeName)");

			strSql.Append(" values (");
			strSql.Append("@HighLowStopScopeID,@HighLowStopScopeName)");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "HighLowStopScopeID", DbType.Int32, model.HighLowStopScopeID);
			db.AddInParameter(dbCommand, "HighLowStopScopeName", DbType.String, model.HighLowStopScopeName);
			db.ExecuteNonQuery(dbCommand);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.QH_HighLowStopScopeType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update QH_HighLowStopScopeType set ");
			strSql.Append("HighLowStopScopeName=@HighLowStopScopeName");
			strSql.Append(" where HighLowStopScopeID=@HighLowStopScopeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "HighLowStopScopeID", DbType.Int32, model.HighLowStopScopeID);
			db.AddInParameter(dbCommand, "HighLowStopScopeName", DbType.String, model.HighLowStopScopeName);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int HighLowStopScopeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete QH_HighLowStopScopeType ");
			strSql.Append(" where HighLowStopScopeID=@HighLowStopScopeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "HighLowStopScopeID", DbType.Int32,HighLowStopScopeID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.QH_HighLowStopScopeType GetModel(int HighLowStopScopeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select HighLowStopScopeID,HighLowStopScopeName from QH_HighLowStopScopeType ");
			strSql.Append(" where HighLowStopScopeID=@HighLowStopScopeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "HighLowStopScopeID", DbType.Int32,HighLowStopScopeID);
			ManagementCenter.Model.QH_HighLowStopScopeType model=null;
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
			strSql.Append("select HighLowStopScopeID,HighLowStopScopeName ");
			strSql.Append(" FROM QH_HighLowStopScopeType ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "QH_HighLowStopScopeType");
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
		public List<ManagementCenter.Model.QH_HighLowStopScopeType> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select HighLowStopScopeID,HighLowStopScopeName ");
			strSql.Append(" FROM QH_HighLowStopScopeType ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.QH_HighLowStopScopeType> list = new List<ManagementCenter.Model.QH_HighLowStopScopeType>();
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
		public ManagementCenter.Model.QH_HighLowStopScopeType ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.QH_HighLowStopScopeType model=new ManagementCenter.Model.QH_HighLowStopScopeType();
			object ojb; 
			ojb = dataReader["HighLowStopScopeID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.HighLowStopScopeID=(int)ojb;
			}
			model.HighLowStopScopeName=dataReader["HighLowStopScopeName"].ToString();
			return model;
		}

		#endregion  成员方法
	}
}

