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
    /// 描述：权证涨跌幅价格 数据访问类XH_RightHightLowPrices。
    /// 作者：刘书伟
    /// 日期：2008-11-26
    /// </summary>
	public class XH_RightHightLowPricesDAL
	{
		public XH_RightHightLowPricesDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(RightHightLowPriceID)+1 from XH_RightHightLowPrices";
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
		public bool Exists(int RightHightLowPriceID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from XH_RightHightLowPrices where RightHightLowPriceID=@RightHightLowPriceID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "RightHightLowPriceID", DbType.Int32,RightHightLowPriceID);
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
		public void Add(ManagementCenter.Model.XH_RightHightLowPrices model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into XH_RightHightLowPrices(");
			strSql.Append("RightHightLowPriceID,RightFrontDayClosePrice,StockFrontDayClosePrice,SetScale,HightLowID)");

			strSql.Append(" values (");
			strSql.Append("@RightHightLowPriceID,@RightFrontDayClosePrice,@StockFrontDayClosePrice,@SetScale,@HightLowID)");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "RightHightLowPriceID", DbType.Int32, model.RightHightLowPriceID);
			db.AddInParameter(dbCommand, "RightFrontDayClosePrice", DbType.Double, model.RightFrontDayClosePrice);
			db.AddInParameter(dbCommand, "StockFrontDayClosePrice", DbType.Double, model.StockFrontDayClosePrice);
			db.AddInParameter(dbCommand, "SetScale", DbType.Double, model.SetScale);
			db.AddInParameter(dbCommand, "HightLowID", DbType.Int32, model.HightLowID);
			db.ExecuteNonQuery(dbCommand);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.XH_RightHightLowPrices model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update XH_RightHightLowPrices set ");
			strSql.Append("RightFrontDayClosePrice=@RightFrontDayClosePrice,");
			strSql.Append("StockFrontDayClosePrice=@StockFrontDayClosePrice,");
			strSql.Append("SetScale=@SetScale,");
			strSql.Append("HightLowID=@HightLowID");
			strSql.Append(" where RightHightLowPriceID=@RightHightLowPriceID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "RightHightLowPriceID", DbType.Int32, model.RightHightLowPriceID);
			db.AddInParameter(dbCommand, "RightFrontDayClosePrice", DbType.Double, model.RightFrontDayClosePrice);
			db.AddInParameter(dbCommand, "StockFrontDayClosePrice", DbType.Double, model.StockFrontDayClosePrice);
			db.AddInParameter(dbCommand, "SetScale", DbType.Double, model.SetScale);
			db.AddInParameter(dbCommand, "HightLowID", DbType.Int32, model.HightLowID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int RightHightLowPriceID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete XH_RightHightLowPrices ");
			strSql.Append(" where RightHightLowPriceID=@RightHightLowPriceID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "RightHightLowPriceID", DbType.Int32,RightHightLowPriceID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.XH_RightHightLowPrices GetModel(int RightHightLowPriceID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select RightHightLowPriceID,RightFrontDayClosePrice,StockFrontDayClosePrice,SetScale,HightLowID from XH_RightHightLowPrices ");
			strSql.Append(" where RightHightLowPriceID=@RightHightLowPriceID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "RightHightLowPriceID", DbType.Int32,RightHightLowPriceID);
			ManagementCenter.Model.XH_RightHightLowPrices model=null;
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
			strSql.Append("select RightHightLowPriceID,RightFrontDayClosePrice,StockFrontDayClosePrice,SetScale,HightLowID ");
			strSql.Append(" FROM XH_RightHightLowPrices ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "XH_RightHightLowPrices");
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
		public List<ManagementCenter.Model.XH_RightHightLowPrices> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select RightHightLowPriceID,RightFrontDayClosePrice,StockFrontDayClosePrice,SetScale,HightLowID ");
			strSql.Append(" FROM XH_RightHightLowPrices ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.XH_RightHightLowPrices> list = new List<ManagementCenter.Model.XH_RightHightLowPrices>();
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
		public ManagementCenter.Model.XH_RightHightLowPrices ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.XH_RightHightLowPrices model=new ManagementCenter.Model.XH_RightHightLowPrices();
			object ojb; 
			ojb = dataReader["RightHightLowPriceID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.RightHightLowPriceID=(int)ojb;
			}
			ojb = dataReader["RightFrontDayClosePrice"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.RightFrontDayClosePrice=(decimal)ojb;
			}
			ojb = dataReader["StockFrontDayClosePrice"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.StockFrontDayClosePrice=(decimal)ojb;
			}
			ojb = dataReader["SetScale"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.SetScale=(decimal)ojb;
			}
			ojb = dataReader["HightLowID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.HightLowID=(int)ojb;
			}
			return model;
		}

		#endregion  成员方法
	}
}

