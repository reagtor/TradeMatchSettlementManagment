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
    ///描述：行业表 数据访问类Profession。
    ///作者：熊晓凌
    ///日期:2008-11-20
    /// </summary>
	public class ProfessionDAL
	{
        public ProfessionDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string Nindcd)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ProfessionInfo where Nindcd=@Nindcd ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "Nindcd", DbType.String,Nindcd);
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
		public void Add(ManagementCenter.Model.Profession model)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into ProfessionInfo(");
			strSql.Append("Nindcd,Nindnme,EnNindnme)");

			strSql.Append(" values (");
			strSql.Append("@Nindcd,@Nindnme,@EnNindnme)");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "Nindcd", DbType.String, model.Nindcd);
			db.AddInParameter(dbCommand, "Nindnme", DbType.String, model.Nindnme);
			db.AddInParameter(dbCommand, "EnNindnme", DbType.String, model.EnNindnme);
			db.ExecuteNonQuery(dbCommand);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.Profession model)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("update ProfessionInfo set ");
			strSql.Append("Nindnme=@Nindnme,");
			strSql.Append("EnNindnme=@EnNindnme");
			strSql.Append(" where Nindcd=@Nindcd ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "Nindcd", DbType.String, model.Nindcd);
			db.AddInParameter(dbCommand, "Nindnme", DbType.String, model.Nindnme);
			db.AddInParameter(dbCommand, "EnNindnme", DbType.String, model.EnNindnme);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(string Nindcd)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("delete ProfessionInfo ");
			strSql.Append(" where Nindcd=@Nindcd ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "Nindcd", DbType.String,Nindcd);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.Profession GetModel(string Nindcd)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select Nindcd,Nindnme,EnNindnme from ProfessionInfo ");
			strSql.Append(" where Nindcd=@Nindcd ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "Nindcd", DbType.String,Nindcd);
			ManagementCenter.Model.Profession model=null;
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
			strSql.Append("select Nindcd,Nindnme,EnNindnme ");
            strSql.Append(" FROM ProfessionInfo ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "Profession");
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
		public List<ManagementCenter.Model.Profession> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Nindcd,Nindnme,EnNindnme ");
            strSql.Append(" FROM ProfessionInfo ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.Profession> list = new List<ManagementCenter.Model.Profession>();
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
		public ManagementCenter.Model.Profession ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.Profession model=new ManagementCenter.Model.Profession();
			model.Nindcd=dataReader["Nindcd"].ToString();
			model.Nindnme=dataReader["Nindnme"].ToString();
			model.EnNindnme=dataReader["EnNindnme"].ToString();
			return model;
		}

		#endregion  成员方法
	}
}

