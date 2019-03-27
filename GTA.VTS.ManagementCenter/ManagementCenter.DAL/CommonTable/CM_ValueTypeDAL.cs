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
    ///描述：交易规则_取值类型 数据访问类CM_ValueTypeDAL。
    ///作者：刘书伟
    ///日期:2008-11-26
	/// </summary>
	public class CM_ValueTypeDAL
	{
		public CM_ValueTypeDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(ValueTypeID)+1 from CM_ValueType";
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
		public bool Exists(int ValueTypeID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from CM_ValueType where ValueTypeID=@ValueTypeID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ValueTypeID", DbType.Int32,ValueTypeID);
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
		public int Add(ManagementCenter.Model.CM_ValueType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CM_ValueType(");
			strSql.Append("Name)");

			strSql.Append(" values (");
			strSql.Append("@Name)");
			strSql.Append(";select @@IDENTITY");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "Name", DbType.String, model.Name);
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
		public void Update(ManagementCenter.Model.CM_ValueType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CM_ValueType set ");
			strSql.Append("Name=@Name");
			strSql.Append(" where ValueTypeID=@ValueTypeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "Name", DbType.String, model.Name);
			db.AddInParameter(dbCommand, "ValueTypeID", DbType.Int32, model.ValueTypeID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ValueTypeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete CM_ValueType ");
			strSql.Append(" where ValueTypeID=@ValueTypeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ValueTypeID", DbType.Int32,ValueTypeID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.CM_ValueType GetModel(int ValueTypeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Name,ValueTypeID from CM_ValueType ");
			strSql.Append(" where ValueTypeID=@ValueTypeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ValueTypeID", DbType.Int32,ValueTypeID);
			ManagementCenter.Model.CM_ValueType model=null;
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
			strSql.Append("select Name,ValueTypeID ");
			strSql.Append(" FROM CM_ValueType ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "CM_ValueType");
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
		public List<ManagementCenter.Model.CM_ValueType> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Name,ValueTypeID ");
			strSql.Append(" FROM CM_ValueType ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.CM_ValueType> list = new List<ManagementCenter.Model.CM_ValueType>();
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
		public ManagementCenter.Model.CM_ValueType ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.CM_ValueType model=new ManagementCenter.Model.CM_ValueType();
			object ojb; 
			model.Name=dataReader["Name"].ToString();
			ojb = dataReader["ValueTypeID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.ValueTypeID=(int)ojb;
			}
			return model;
		}

		#endregion  成员方法
	}
}

