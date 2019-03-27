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
    ///描述：股票分红记录_股票 数据访问类CM_StockMelonStockDAL。
    ///作者：熊晓凌
    ///日期:2008-11-20
	/// </summary>
	public class CM_StockMelonStockDAL
	{
		public CM_StockMelonStockDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(StockMelonStockID)+1 from StockMelonStock";
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
		public bool Exists(int StockMelonStockID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from StockMelonStock where StockMelonStockID=@StockMelonStockID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "StockMelonStockID", DbType.Int32,StockMelonStockID);
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
		public int Add(ManagementCenter.Model.CM_StockMelonStock model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into StockMelonStock(");
            strSql.Append("StockRightRegisterDate,StockRightLogoutDatumDate,SentStockRatio,StockCode)");

			strSql.Append(" values (");
            strSql.Append("@StockRightRegisterDate,@StockRightLogoutDatumDate,@SentStockRatio,@StockCode)");
			strSql.Append(";select @@IDENTITY");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "StockRightRegisterDate", DbType.DateTime, model.StockRightRegisterDate);
			db.AddInParameter(dbCommand, "StockRightLogoutDatumDate", DbType.DateTime, model.StockRightLogoutDatumDate);
			db.AddInParameter(dbCommand, "SentStockRatio", DbType.Double, model.SentStockRatio);
            db.AddInParameter(dbCommand, "StockCode", DbType.String, model.CommodityCode);
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
		public void Update(ManagementCenter.Model.CM_StockMelonStock model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update StockMelonStock set ");
			strSql.Append("StockRightRegisterDate=@StockRightRegisterDate,");
			strSql.Append("StockRightLogoutDatumDate=@StockRightLogoutDatumDate,");
			strSql.Append("SentStockRatio=@SentStockRatio,");
            strSql.Append("StockCode=@StockCode");
			strSql.Append(" where StockMelonStockID=@StockMelonStockID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "StockMelonStockID", DbType.Int32, model.StockMelonStockID);
			db.AddInParameter(dbCommand, "StockRightRegisterDate", DbType.DateTime, model.StockRightRegisterDate);
			db.AddInParameter(dbCommand, "StockRightLogoutDatumDate", DbType.DateTime, model.StockRightLogoutDatumDate);
			db.AddInParameter(dbCommand, "SentStockRatio", DbType.Double, model.SentStockRatio);
            db.AddInParameter(dbCommand, "StockCode", DbType.String, model.CommodityCode);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int StockMelonStockID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete StockMelonStock ");
			strSql.Append(" where StockMelonStockID=@StockMelonStockID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "StockMelonStockID", DbType.Int32,StockMelonStockID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.CM_StockMelonStock GetModel(int StockMelonStockID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select StockMelonStockID,StockRightRegisterDate,StockRightLogoutDatumDate,SentStockRatio,StockCode from StockMelonStock ");
			strSql.Append(" where StockMelonStockID=@StockMelonStockID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "StockMelonStockID", DbType.Int32,StockMelonStockID);
			ManagementCenter.Model.CM_StockMelonStock model=null;
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
            strSql.Append("select StockMelonStockID,StockRightRegisterDate,StockRightLogoutDatumDate,SentStockRatio,StockCode ");
			strSql.Append(" FROM StockMelonStock ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "CM_StockMelonStock");
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
		public List<ManagementCenter.Model.CM_StockMelonStock> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select StockMelonStockID,StockRightRegisterDate,StockRightLogoutDatumDate,SentStockRatio,StockCode ");
			strSql.Append(" FROM StockMelonStock ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.CM_StockMelonStock> list = new List<ManagementCenter.Model.CM_StockMelonStock>();
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
		public ManagementCenter.Model.CM_StockMelonStock ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.CM_StockMelonStock model=new ManagementCenter.Model.CM_StockMelonStock();
			object ojb; 
			ojb = dataReader["StockMelonStockID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.StockMelonStockID=(int)ojb;
			}
			ojb = dataReader["StockRightRegisterDate"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.StockRightRegisterDate=(DateTime)ojb;
			}
			ojb = dataReader["StockRightLogoutDatumDate"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.StockRightLogoutDatumDate=(DateTime)ojb;
			}
			ojb = dataReader["SentStockRatio"];
			if(ojb != null && ojb != DBNull.Value)
			{
                model.SentStockRatio = (double)ojb;
			}
            model.CommodityCode = dataReader["StockCode"].ToString();
			return model;
		}

		#endregion  成员方法
	}
}

