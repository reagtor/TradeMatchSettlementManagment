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
    ///描述：港股_品种_交易规则 数据访问类HK_SpotTradeRulesDAL。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_SpotTradeRulesDAL
    {
        public HK_SpotTradeRulesDAL()
        {

        }

        #region SQL

        /// <summary>
        /// 根据查询条件返回港股品种规则数据
        /// </summary>
        private string SQL_SELECT_HKSPOTTRADERULES =
            @"SELECT B.BREEDCLASSNAME,A.* FROM HK_SPOTTRADERULES AS A,CM_BREEDCLASS AS B 
                                                            WHERE A.BREEDCLASSID=B.BREEDCLASSID ";


        /// <summary>
        /// 根据港股规则表中的品种标识获取品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAME_HKSPOTTRADERULES =
            @" SELECT A.BREEDCLASSID,A.BREEDCLASSNAME 
                                                                    FROM CM_BREEDCLASS A,HK_SPOTTRADERULES B 
                                                                    WHERE A.BREEDCLASSID=B.BREEDCLASSID";



        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from HK_SpotTradeRules";
            Database db = DatabaseFactory.CreateDatabase();
            object obj = db.ExecuteScalar(CommandType.Text, strsql);
            if (obj != null && obj != DBNull.Value)
            {
                return int.Parse(obj.ToString());
            }
            return 1;
        }

        /// <summary>
        /// 根据品种ID判断港股交易规则是否存在记录
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool Exists(int BreedClassID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_SpotTradeRules where BreedClassID=@BreedClassID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
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

        #region  添加港股交易规则
        /// <summary>
        /// 添加港股交易规则
        /// </summary>
        /// <param name="model">港股交易规则实体</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.HK_SpotTradeRules model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_SpotTradeRules(");
            strSql.Append("FundDeliveryInstitution,StockDeliveryInstitution,MaxLeaveQuantity,MarketUnitID,PriceUnit,BreedClassID)");

            strSql.Append(" values (");
            strSql.Append("@FundDeliveryInstitution,@StockDeliveryInstitution,@MaxLeaveQuantity,@MarketUnitID,@PriceUnit,@BreedClassID)");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FundDeliveryInstitution", DbType.Int32, model.FundDeliveryInstitution);
            db.AddInParameter(dbCommand, "StockDeliveryInstitution", DbType.Int32, model.StockDeliveryInstitution);
            db.AddInParameter(dbCommand, "MaxLeaveQuantity", DbType.Int32, model.MaxLeaveQuantity);
            db.AddInParameter(dbCommand, "MarketUnitID", DbType.Int32, model.MarketUnitID);
            db.AddInParameter(dbCommand, "PriceUnit", DbType.Int32, model.PriceUnit);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
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
        #endregion

        #region  添加港股交易规则
        /// <summary>
        /// 添加港股交易规则
        /// </summary>
        /// <param name="model">港股交易规则实体</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.HK_SpotTradeRules model)
        {
            return Add(model, null, null);
        }
        #endregion

        /// <summary>
        /// 更新港股交易规则
        /// </summary>
        /// <param name="model">港股交易规则实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.HK_SpotTradeRules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_SpotTradeRules set ");
            strSql.Append("FundDeliveryInstitution=@FundDeliveryInstitution,");
            strSql.Append("StockDeliveryInstitution=@StockDeliveryInstitution,");
            strSql.Append("MaxLeaveQuantity=@MaxLeaveQuantity,");
            strSql.Append("MarketUnitID=@MarketUnitID,");
            strSql.Append("PriceUnit=@PriceUnit");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FundDeliveryInstitution", DbType.Int32, model.FundDeliveryInstitution);
            db.AddInParameter(dbCommand, "StockDeliveryInstitution", DbType.Int32, model.StockDeliveryInstitution);
            db.AddInParameter(dbCommand, "MaxLeaveQuantity", DbType.Int32, model.MaxLeaveQuantity);
            db.AddInParameter(dbCommand, "MarketUnitID", DbType.Int32, model.MarketUnitID);
            db.AddInParameter(dbCommand, "PriceUnit", DbType.Int32, model.PriceUnit);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #region 根据品种标识删除港股交易规则

        /// <summary>
        /// 根据品种标识删除港股交易规则
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HK_SpotTradeRules ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();

            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
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
        #endregion

        #region 根据品种标识删除港股交易规则

        /// <summary>
        /// 根据品种标识删除港股交易规则
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID)
        {
            return Delete(BreedClassID, null, null);
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.HK_SpotTradeRules GetModel(int BreedClassID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FundDeliveryInstitution,StockDeliveryInstitution,MaxLeaveQuantity,MarketUnitID,PriceUnit,BreedClassID from HK_SpotTradeRules ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            ManagementCenter.Model.HK_SpotTradeRules model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FundDeliveryInstitution,StockDeliveryInstitution,MaxLeaveQuantity,MarketUnitID,PriceUnit,BreedClassID ");
            strSql.Append(" FROM HK_SpotTradeRules ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<ManagementCenter.Model.HK_SpotTradeRules> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FundDeliveryInstitution,StockDeliveryInstitution,MaxLeaveQuantity,MarketUnitID,PriceUnit,BreedClassID ");
            strSql.Append(" FROM HK_SpotTradeRules ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HK_SpotTradeRules> list = new List<ManagementCenter.Model.HK_SpotTradeRules>();
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

        #region 获取所有港股交易规则

        /// <summary>
        /// 获取所有港股交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllHKSpotTradeRules(string BreedClassName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            //条件查询
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                SQL_SELECT_HKSPOTTRADERULES += "AND (BreedClassName LIKE  '%' + @BreedClassName + '%') ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_HKSPOTTRADERULES);
            if (BreedClassName != AppGlobalVariable.INIT_STRING && BreedClassName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BreedClassName", DbType.String, BreedClassName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_HKSPOTTRADERULES, pageNo, pageSize,
                                        out rowCount, "HK_SpotTradeRules");
        }

        #endregion

        #region 根据港股规则表中的品种ID获取品种名称
        /// <summary>
        /// 根据港股规则表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetHKBreedClassNameByBreedClassID()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAME_HKSPOTTRADERULES);
            return database.ExecuteDataSet(dbCommand);
        }
        #endregion


        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.HK_SpotTradeRules ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.HK_SpotTradeRules model = new ManagementCenter.Model.HK_SpotTradeRules();
            object ojb;
            ojb = dataReader["FundDeliveryInstitution"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FundDeliveryInstitution = (int)ojb;
            }
            ojb = dataReader["StockDeliveryInstitution"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StockDeliveryInstitution = (int)ojb;
            }
            ojb = dataReader["MaxLeaveQuantity"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MaxLeaveQuantity = (int)ojb;
            }
            ojb = dataReader["MarketUnitID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketUnitID = (int)ojb;
            }
            ojb = dataReader["PriceUnit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PriceUnit = (int)ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}
