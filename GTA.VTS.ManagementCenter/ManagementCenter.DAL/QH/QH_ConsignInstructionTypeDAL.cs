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
    ///描述：委托指令类型 数据访问类QH_ConsignInstructionTypeDAL。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
	public class QH_ConsignInstructionTypeDAL
	{
		public QH_ConsignInstructionTypeDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(ConsignInstructionTypeID)+1 from QH_ConsignInstructionType";
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
		public bool Exists(int ConsignInstructionTypeID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from QH_ConsignInstructionType where ConsignInstructionTypeID=@ConsignInstructionTypeID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ConsignInstructionTypeID", DbType.Int32,ConsignInstructionTypeID);
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
		public void Add(ManagementCenter.Model.QH_ConsignInstructionType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into QH_ConsignInstructionType(");
			strSql.Append("ConsignInstructionTypeID,TypeName)");

			strSql.Append(" values (");
			strSql.Append("@ConsignInstructionTypeID,@TypeName)");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ConsignInstructionTypeID", DbType.Int32, model.ConsignInstructionTypeID);
			db.AddInParameter(dbCommand, "TypeName", DbType.AnsiString, model.TypeName);
			db.ExecuteNonQuery(dbCommand);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.QH_ConsignInstructionType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update QH_ConsignInstructionType set ");
			strSql.Append("TypeName=@TypeName");
			strSql.Append(" where ConsignInstructionTypeID=@ConsignInstructionTypeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ConsignInstructionTypeID", DbType.Int32, model.ConsignInstructionTypeID);
			db.AddInParameter(dbCommand, "TypeName", DbType.AnsiString, model.TypeName);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ConsignInstructionTypeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete QH_ConsignInstructionType ");
			strSql.Append(" where ConsignInstructionTypeID=@ConsignInstructionTypeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ConsignInstructionTypeID", DbType.Int32,ConsignInstructionTypeID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.QH_ConsignInstructionType GetModel(int ConsignInstructionTypeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ConsignInstructionTypeID,TypeName from QH_ConsignInstructionType ");
			strSql.Append(" where ConsignInstructionTypeID=@ConsignInstructionTypeID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "ConsignInstructionTypeID", DbType.Int32,ConsignInstructionTypeID);
			ManagementCenter.Model.QH_ConsignInstructionType model=null;
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
			strSql.Append("select ConsignInstructionTypeID,TypeName ");
			strSql.Append(" FROM QH_ConsignInstructionType ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "QH_ConsignInstructionType");
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
		public List<ManagementCenter.Model.QH_ConsignInstructionType> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ConsignInstructionTypeID,TypeName ");
			strSql.Append(" FROM QH_ConsignInstructionType ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.QH_ConsignInstructionType> list = new List<ManagementCenter.Model.QH_ConsignInstructionType>();
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
		public ManagementCenter.Model.QH_ConsignInstructionType ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.QH_ConsignInstructionType model=new ManagementCenter.Model.QH_ConsignInstructionType();
			object ojb; 
			ojb = dataReader["ConsignInstructionTypeID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.ConsignInstructionTypeID=(int)ojb;
			}
			model.TypeName=dataReader["TypeName"].ToString();
			return model;
		}

		#endregion  成员方法
	}
}

