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
    ///描述：增发股票代码 数据访问类ZFInfo。
    ///作者：熊晓凌
    ///日期:2008-11-20
	/// </summary>
	public class ZFInfoDAL
	{
        public ZFInfoDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string stkcd)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from ZFInfo where stkcd=@stkcd ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "stkcd", DbType.String,stkcd);
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
		public void Add(ManagementCenter.Model.ZFInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ZFInfo(");
			strSql.Append("stkcd,roprc,zfgs,paydt)");

			strSql.Append(" values (");
			strSql.Append("@stkcd,@roprc,@zfgs,@paydt)");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "stkcd", DbType.String, model.stkcd);
			db.AddInParameter(dbCommand, "roprc", DbType.Double, model.roprc);
			db.AddInParameter(dbCommand, "zfgs", DbType.Double, model.zfgs);
			db.AddInParameter(dbCommand, "paydt", DbType.String, model.paydt);
			db.ExecuteNonQuery(dbCommand);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.ZFInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ZFInfo set ");
			strSql.Append("roprc=@roprc,");
			strSql.Append("zfgs=@zfgs,");
			strSql.Append("paydt=@paydt");
			strSql.Append(" where stkcd=@stkcd ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "stkcd", DbType.String, model.stkcd);
			db.AddInParameter(dbCommand, "roprc", DbType.Double, model.roprc);
			db.AddInParameter(dbCommand, "zfgs", DbType.Double, model.zfgs);
			db.AddInParameter(dbCommand, "paydt", DbType.String, model.paydt);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(string stkcd)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete ZFInfo ");
			strSql.Append(" where stkcd=@stkcd ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "stkcd", DbType.String,stkcd);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.ZFInfo GetModel(string stkcd)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select stkcd,roprc,zfgs,paydt from ZFInfo ");
			strSql.Append(" where stkcd=@stkcd ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "stkcd", DbType.String,stkcd);
			ManagementCenter.Model.ZFInfo model=null;
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
			strSql.Append("select stkcd,roprc,zfgs,paydt ");
			strSql.Append(" FROM ZFInfo ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "ZFInfo");
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
		public List<ManagementCenter.Model.ZFInfo> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select stkcd,roprc,zfgs,paydt ");
			strSql.Append(" FROM ZFInfo ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.ZFInfo> list = new List<ManagementCenter.Model.ZFInfo>();
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
		public ManagementCenter.Model.ZFInfo ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.ZFInfo model=new ManagementCenter.Model.ZFInfo();
			object ojb; 
			model.stkcd=dataReader["stkcd"].ToString();
			ojb = dataReader["roprc"];
			if(ojb != null && ojb != DBNull.Value)
			{
                model.roprc = (double)ojb;
			}
			ojb = dataReader["zfgs"];
			if(ojb != null && ojb != DBNull.Value)
			{
                model.zfgs = (double)ojb;
			}
			model.paydt=dataReader["paydt"].ToString();
			return model;
		}

		#endregion  成员方法
	}
}

