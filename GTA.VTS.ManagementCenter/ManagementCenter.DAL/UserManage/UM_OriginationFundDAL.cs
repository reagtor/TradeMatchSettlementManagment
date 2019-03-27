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
    /// 描述：起始资金表 数据访问类UM_OriginationFundDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
	public class UM_OriginationFundDAL
	{
		public UM_OriginationFundDAL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(OriginationFundID)+1 from UM_OriginationFund";
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
		public bool Exists(int OriginationFundID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from UM_OriginationFund where OriginationFundID=@OriginationFundID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "OriginationFundID", DbType.Int32,OriginationFundID);
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
        public int Add(ManagementCenter.Model.UM_OriginationFund model, DbTransaction tran, Database db)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UM_OriginationFund(");
			strSql.Append("FundMoney,Remark,TransactionCurrencyTypeID,DealerAccoutID)");

			strSql.Append(" values (");
			strSql.Append("@FundMoney,@Remark,@TransactionCurrencyTypeID,@DealerAccoutID)");
			strSql.Append(";select @@IDENTITY");

			if(db==null) db = DatabaseFactory.CreateDatabase();

			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "FundMoney", DbType.Currency, model.FundMoney);
			db.AddInParameter(dbCommand, "Remark", DbType.String, model.Remark);
			db.AddInParameter(dbCommand, "TransactionCurrencyTypeID", DbType.Int32, model.TransactionCurrencyTypeID);
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String, model.DealerAccoutID);
			int result;
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand, tran);
            }
			if(!int.TryParse(obj.ToString(),out result))
			{
				return 0;
			}
			return result;
		}
        /// <summary>
        /// 添加初始化资金
        /// </summary>
        /// <param name="model">初始化资金实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.UM_OriginationFund model)
        {
            return Add(model, null,null);
        }

	    /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_OriginationFund model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UM_OriginationFund set ");
			strSql.Append("FundMoney=@FundMoney,");
			strSql.Append("Remark=@Remark,");
			strSql.Append("TransactionCurrencyTypeID=@TransactionCurrencyTypeID,");
			strSql.Append("DealerAccoutID=@DealerAccoutID");
			strSql.Append(" where OriginationFundID=@OriginationFundID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "FundMoney", DbType.Currency, model.FundMoney);
			db.AddInParameter(dbCommand, "Remark", DbType.String, model.Remark);
			db.AddInParameter(dbCommand, "OriginationFundID", DbType.Int32, model.OriginationFundID);
			db.AddInParameter(dbCommand, "TransactionCurrencyTypeID", DbType.Int32, model.TransactionCurrencyTypeID);
			db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String, model.DealerAccoutID);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int OriginationFundID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_OriginationFund ");
			strSql.Append(" where OriginationFundID=@OriginationFundID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "OriginationFundID", DbType.Int32,OriginationFundID);
			db.ExecuteNonQuery(dbCommand);

		}

        /// <summary>
		/// 删除数据根据帐号
		/// </summary>
        public void DeleteByDealerAccoutID(string DealerAccoutID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete UM_OriginationFund ");
            strSql.Append(" where DealerAccoutID=@DealerAccoutID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "DealerAccoutID", DbType.String, DealerAccoutID);
			db.ExecuteNonQuery(dbCommand);

		}
        
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_OriginationFund GetModel(int OriginationFundID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select FundMoney,Remark,OriginationFundID,TransactionCurrencyTypeID,DealerAccoutID from UM_OriginationFund ");
			strSql.Append(" where OriginationFundID=@OriginationFundID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "OriginationFundID", DbType.Int32,OriginationFundID);
			ManagementCenter.Model.UM_OriginationFund model=null;
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
			strSql.Append("select FundMoney,Remark,OriginationFundID,TransactionCurrencyTypeID,DealerAccoutID ");
			strSql.Append(" FROM UM_OriginationFund ");
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
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_OriginationFund");
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
		public List<ManagementCenter.Model.UM_OriginationFund> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select FundMoney,Remark,OriginationFundID,TransactionCurrencyTypeID,DealerAccoutID ");
			strSql.Append(" FROM UM_OriginationFund ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.UM_OriginationFund> list = new List<ManagementCenter.Model.UM_OriginationFund>();
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
		public ManagementCenter.Model.UM_OriginationFund ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.UM_OriginationFund model=new ManagementCenter.Model.UM_OriginationFund();
			object ojb; 
			ojb = dataReader["FundMoney"];
			if(ojb != null && ojb != DBNull.Value)
			{ 
                model.FundMoney = (decimal)ojb;
			}
			model.Remark=dataReader["Remark"].ToString();
			ojb = dataReader["OriginationFundID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.OriginationFundID=(int)ojb;
			}
			ojb = dataReader["TransactionCurrencyTypeID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.TransactionCurrencyTypeID=(int)ojb;
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
            strSql.Append("delete from UM_OriginationFund  where DealerAccoutID in  ");
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

        /// <summary>
        /// 根据交易员ID获取初始资金列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_OriginationFund> GetListByUserID(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select * from UM_OriginationFund  where DealerAccoutID in  ");
            strSql.Append(" (select DealerAccoutID from UM_DealerAccount where UserID=@UserID) ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            List<ManagementCenter.Model.UM_OriginationFund> list = new List<ManagementCenter.Model.UM_OriginationFund>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }
		#endregion  成员方法
	}
}

