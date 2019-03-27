using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用
namespace ManagementCenter.DAL
{
	/// <summary>
    ///描述：品种_期货_交易费用 数据访问类QH_FutureCostsDAL。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
	public class QH_FutureCostsDAL
	{
		public QH_FutureCostsDAL()
		{}

        #region SQL
        /// <summary>
        /// 根据查询条件返回品种_期货_交易费用
        /// </summary>
        private string SQL_SELECT_QHFUTURECOSTS = @"SELECT B.BREEDCLASSNAME,A.* 
                                            FROM QH_FUTURECOSTS AS A,CM_BREEDCLASS AS B
                                            WHERE A.BREEDCLASSID=B.BREEDCLASSID AND B.BREEDCLASSTYPEID<>1 ";


        #endregion

		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(BreedClassID)+1 from QH_FutureCosts";
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
		public bool Exists(int BreedClassID)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from QH_FutureCosts where BreedClassID=@BreedClassID ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32,BreedClassID);
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
		public bool Add(ManagementCenter.Model.QH_FutureCosts model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into QH_FutureCosts(");
            strSql.Append("TurnoverRateOfServiceCharge,CurrencyTypeID,CostType,BreedClassID)");

			strSql.Append(" values (");
            strSql.Append("@TurnoverRateOfServiceCharge,@CurrencyTypeID,@CostType,@BreedClassID)");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            //if (model.TradeUnitCharge == AppGlobalVariable.INIT_DECIMAL)
            //{
            //    db.AddInParameter(dbCommand, "TradeUnitCharge", DbType.Decimal, DBNull.Value);
            //}
            //else
            //{
            //    db.AddInParameter(dbCommand, "TradeUnitCharge", DbType.Decimal, model.TradeUnitCharge);
            //}
            if (model.TurnoverRateOfServiceCharge == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "TurnoverRateOfServiceCharge", DbType.Decimal,DBNull.Value);

            }
            else
            {
                db.AddInParameter(dbCommand, "TurnoverRateOfServiceCharge", DbType.Decimal, model.TurnoverRateOfServiceCharge);
            }
			db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            db.AddInParameter(dbCommand, "CostType",DbType.Int32,model.CostType);
			db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
			db.ExecuteNonQuery(dbCommand);
		    return true;
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(ManagementCenter.Model.QH_FutureCosts model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update QH_FutureCosts set ");
            //strSql.Append("TradeUnitCharge=@TradeUnitCharge,");
			strSql.Append("TurnoverRateOfServiceCharge=@TurnoverRateOfServiceCharge,");
			strSql.Append("CurrencyTypeID=@CurrencyTypeID,");
            strSql.Append("CostType=@CostType");
			strSql.Append(" where BreedClassID=@BreedClassID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            //if (model.TradeUnitCharge == AppGlobalVariable.INIT_DECIMAL)
            //{
            //    db.AddInParameter(dbCommand, "TradeUnitCharge", DbType.Decimal, DBNull.Value);
            //}
            //else
            //{
            //    db.AddInParameter(dbCommand, "TradeUnitCharge", DbType.Decimal, model.TradeUnitCharge);
            //}
            if (model.TurnoverRateOfServiceCharge == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "TurnoverRateOfServiceCharge", DbType.Decimal, DBNull.Value);

            }
            else
            {
                db.AddInParameter(dbCommand, "TurnoverRateOfServiceCharge", DbType.Decimal, model.TurnoverRateOfServiceCharge);
            }
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            db.AddInParameter(dbCommand, "CostType", DbType.Int32, model.CostType);
			db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
			db.ExecuteNonQuery(dbCommand);
		    return true;
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete QH_FutureCosts ");
			strSql.Append(" where BreedClassID=@BreedClassID ");
            if(db==null)
            {
                 db = DatabaseFactory.CreateDatabase();
            }
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32,BreedClassID);
			//db.ExecuteNonQuery(dbCommand);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
		    return true;
		}
        /// <summary>
        /// 根据品种ID，删除品种_期货_交易费用
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
         public bool Delete(int BreedClassID)
         {
             return Delete(BreedClassID, null, null);
         }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.QH_FutureCosts GetModel(int BreedClassID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select TurnoverRateOfServiceCharge,CurrencyTypeID,CostType,BreedClassID from QH_FutureCosts ");
			strSql.Append(" where BreedClassID=@BreedClassID ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32,BreedClassID);
			ManagementCenter.Model.QH_FutureCosts model=null;
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
            strSql.Append("select TurnoverRateOfServiceCharge,CurrencyTypeID,CostType,BreedClassID ");
			strSql.Append(" FROM QH_FutureCosts ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			Database db = DatabaseFactory.CreateDatabase();
			return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
		}

		/// <summary>
		/// 获得数据列表（比DataSet效率高，推荐使用）
		/// </summary>
		public List<ManagementCenter.Model.QH_FutureCosts> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select TurnoverRateOfServiceCharge,CurrencyTypeID,CostType,BreedClassID ");
			strSql.Append(" FROM QH_FutureCosts ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			List<ManagementCenter.Model.QH_FutureCosts> list = new List<ManagementCenter.Model.QH_FutureCosts>();
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

        #region 获取所有品种_期货_交易费用

        /// <summary>
        ///获取所有品种_期货_交易费用
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllQHFutureCosts(string BreedClassName,
                                                  int pageNo, int pageSize,
                                                  out int rowCount)
        {
            //条件查询
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                SQL_SELECT_QHFUTURECOSTS += "AND (BreedClassName LIKE  '%' + @BreedClassName + '%') ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_QHFUTURECOSTS);

            if (BreedClassName != AppGlobalVariable.INIT_STRING && BreedClassName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BreedClassName", DbType.String, BreedClassName);
            }
            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_QHFUTURECOSTS, pageNo, pageSize,
                                        out rowCount, "QH_FutureCosts");
        }

        #endregion


		/// <summary>
		/// 对象实体绑定数据
		/// </summary>
		public ManagementCenter.Model.QH_FutureCosts ReaderBind(IDataReader dataReader)
		{
			ManagementCenter.Model.QH_FutureCosts model=new ManagementCenter.Model.QH_FutureCosts();
			object ojb; 
            //ojb = dataReader["TradeUnitCharge"];
            //if(ojb != null && ojb != DBNull.Value)
            //{
            //    model.TradeUnitCharge=(decimal)ojb;
            //}
			ojb = dataReader["TurnoverRateOfServiceCharge"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.TurnoverRateOfServiceCharge=(decimal)ojb;
			}
			ojb = dataReader["CurrencyTypeID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.CurrencyTypeID=(int)ojb;
			}
            ojb = dataReader["CostType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CostType = (int)ojb;
            }
			ojb = dataReader["BreedClassID"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.BreedClassID=(int)ojb;
			}
			return model;
		}

		#endregion  成员方法
	}
}

