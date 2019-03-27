using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility; //请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述：现货_品种_交易规则 数据访问类XH_SpotTradeRulesDAL。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class XH_SpotTradeRulesDAL
    {
        public XH_SpotTradeRulesDAL()
        {
        }

        #region SQL

        /// <summary>
        /// 根据现货规则表中的品种标识获取品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAME_XHSPOTTRADERULES =
            @" SELECT A.BREEDCLASSID,A.BREEDCLASSNAME 
                                                                    FROM CM_BREEDCLASS A,XH_SPOTTRADERULES B 
                                                                    WHERE A.BREEDCLASSID=B.BREEDCLASSID";

        /// <summary>
        /// 根据查询条件返加现货规则数据
        /// </summary>
        private string SQL_SELECT_XHSPOTTRADERULES =
            @"SELECT B.BreedClassName,a.* FROM XH_SPOTTRADERULES as a,CM_BreedClass as b 
                                                            WHERE a.BreedClassID=B.BreedClassID ";


        /// <summary>
        /// 根据品种ID返回现货规则明细(此品种的涨跌幅和有效申报)数据
        /// </summary>
        private string SQL_SELECT_XHSPOTTRADERULESDetail =
            @"SELECT A.BREEDCLASSID, B.HIGHLOWTYPEID,C.FUNDYESTCLOSEPRICESCALE,C.RIGHTHIGHLOWSCALE,
                C.NORMALVALUE,C.STVALUE,D.VALIDDECLARETYPEID,E.UPPERLIMIT,
                E.LOWERLIMIT,E.NEWDAYUPPERLIMIT,E.NEWDAYLOWERLIMIT
                FROM XH_SPOTTRADERULES A 
                RIGHT JOIN XH_SPOTHIGHLOWCONTROLTYPE B ON A.BREEDCLASSHIGHLOWID=B.BREEDCLASSHIGHLOWID
                RIGHT JOIN XH_SPOTHIGHLOWVALUE C ON A.BREEDCLASSHIGHLOWID=C.BREEDCLASSHIGHLOWID
                LEFT JOIN XH_VALIDDECLARETYPE D ON A.BREEDCLASSVALIDID=D.BREEDCLASSVALIDID
                LEFT JOIN XH_VALIDDECLAREVALUE E ON A.BREEDCLASSVALIDID=E.BREEDCLASSVALIDID
                WHERE A.BREEDCLASSID=@BREEDCLASSID ";

        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from XH_SpotTradeRules";
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
            strSql.Append("select count(1) from XH_SpotTradeRules where BreedClassID=@BreedClassID ");
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


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(ManagementCenter.Model.XH_SpotTradeRules model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_SpotTradeRules(");
            strSql.Append(
                "MinChangePrice,FundDeliveryInstitution,StockDeliveryInstitution,BreedClassID,IsSlew,MaxLeaveQuantity,ValueTypeMinChangePrice,MarketUnitID,PriceUnit,MaxLeaveQuantityUnit,MinVolumeMultiples,BreedClassValidID,BreedClassHighLowID)");
            strSql.Append(" values (");
            strSql.Append(
                "@MinChangePrice,@FundDeliveryInstitution,@StockDeliveryInstitution,@BreedClassID,@IsSlew,@MaxLeaveQuantity,@ValueTypeMinChangePrice,@MarketUnitID,@PriceUnit,@MaxLeaveQuantityUnit,@MinVolumeMultiples,@BreedClassValidID,@BreedClassHighLowID)");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinChangePrice", DbType.Currency, model.MinChangePrice);
            db.AddInParameter(dbCommand, "FundDeliveryInstitution", DbType.Int32, model.FundDeliveryInstitution);
            db.AddInParameter(dbCommand, "StockDeliveryInstitution", DbType.Int32, model.StockDeliveryInstitution);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "IsSlew", DbType.Int32, model.IsSlew);
            db.AddInParameter(dbCommand, "MaxLeaveQuantity", DbType.Int32, model.MaxLeaveQuantity);
            db.AddInParameter(dbCommand, "ValueTypeMinChangePrice", DbType.Int32, model.ValueTypeMinChangePrice);
            //  db.AddInParameter(dbCommand, "ValueTypeMaxLeaveQuantity", DbType.Int32, model.ValueTypeMaxLeaveQuantity);
            db.AddInParameter(dbCommand, "MarketUnitID", DbType.Int32, model.MarketUnitID);
            db.AddInParameter(dbCommand, "PriceUnit", DbType.Int32, model.PriceUnit);
            db.AddInParameter(dbCommand, "MaxLeaveQuantityUnit", DbType.Int32, model.MaxLeaveQuantityUnit);
            db.AddInParameter(dbCommand, "MinVolumeMultiples", DbType.Int32, model.MinVolumeMultiples);
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, model.BreedClassValidID);
            db.AddInParameter(dbCommand, "BreedClassHighLowID", DbType.Int32, model.BreedClassHighLowID);

            // db.ExecuteNonQuery(dbCommand);
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                obj = db.ExecuteNonQuery(dbCommand, tran);
            }

            return true;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(ManagementCenter.Model.XH_SpotTradeRules model)
        {
            return Add(model, null, null);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateSpotTradeRules(ManagementCenter.Model.XH_SpotTradeRules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_SpotTradeRules set ");
            strSql.Append("MinChangePrice=@MinChangePrice,");
            strSql.Append("FundDeliveryInstitution=@FundDeliveryInstitution,");
            strSql.Append("StockDeliveryInstitution=@StockDeliveryInstitution,");
            strSql.Append("IsSlew=@IsSlew,");
            strSql.Append("MaxLeaveQuantity=@MaxLeaveQuantity,");
            strSql.Append("ValueTypeMinChangePrice=@ValueTypeMinChangePrice,");
            // strSql.Append("ValueTypeMaxLeaveQuantity=@ValueTypeMaxLeaveQuantity,");
            strSql.Append("MarketUnitID=@MarketUnitID,");
            strSql.Append("PriceUnit=@PriceUnit,");
            strSql.Append("MaxLeaveQuantityUnit=@MaxLeaveQuantityUnit,");
            strSql.Append("MinVolumeMultiples=@MinVolumeMultiples,");
            strSql.Append("BreedClassValidID=@BreedClassValidID,");
            strSql.Append("BreedClassHighLowID=@BreedClassHighLowID");
            strSql.Append(" where BreedClassID=@BreedClassID");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinChangePrice", DbType.Currency, model.MinChangePrice);
            db.AddInParameter(dbCommand, "FundDeliveryInstitution", DbType.Int32, model.FundDeliveryInstitution);
            db.AddInParameter(dbCommand, "StockDeliveryInstitution", DbType.Int32, model.StockDeliveryInstitution);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "IsSlew", DbType.Int32, model.IsSlew);
            db.AddInParameter(dbCommand, "MaxLeaveQuantity", DbType.Int32, model.MaxLeaveQuantity);
            db.AddInParameter(dbCommand, "ValueTypeMinChangePrice", DbType.Int32, model.ValueTypeMinChangePrice);
            // db.AddInParameter(dbCommand, "ValueTypeMaxLeaveQuantity", DbType.Int32, model.ValueTypeMaxLeaveQuantity);
            db.AddInParameter(dbCommand, "MarketUnitID", DbType.Int32, model.MarketUnitID);
            db.AddInParameter(dbCommand, "PriceUnit", DbType.Int32, model.PriceUnit);
            db.AddInParameter(dbCommand, "MaxLeaveQuantityUnit", DbType.Int32, model.MaxLeaveQuantityUnit);
            db.AddInParameter(dbCommand, "MinVolumeMultiples", DbType.Int32, model.MinVolumeMultiples);
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, model.BreedClassValidID);
            db.AddInParameter(dbCommand, "BreedClassHighLowID", DbType.Int32, model.BreedClassHighLowID);

            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #region 根据品种标识删除现货品种交易规则

        /// <summary>
        /// 根据品种标识删除现货品种交易规则
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_SpotTradeRules ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            //db.ExecuteNonQuery(dbCommand);
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                obj = db.ExecuteNonQuery(dbCommand, tran);
            }
            return true;
        }

        #endregion

        #region 根据品种标识删除现货品种交易规则(无事务)

        /// <summary>
        /// 根据品种标识删除现货品种交易规则
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
        public ManagementCenter.Model.XH_SpotTradeRules GetModel(int BreedClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select MinChangePrice,FundDeliveryInstitution,StockDeliveryInstitution,BreedClassID,IsSlew,MaxLeaveQuantity,ValueTypeMinChangePrice,MarketUnitID,PriceUnit,MaxLeaveQuantityUnit,MinVolumeMultiples,BreedClassValidID,BreedClassHighLowID from XH_SpotTradeRules ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            ManagementCenter.Model.XH_SpotTradeRules model = null;
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
            strSql.Append(
                "select MinChangePrice,FundDeliveryInstitution,StockDeliveryInstitution,BreedClassID,IsSlew,MaxLeaveQuantity,ValueTypeMinChangePrice,MarketUnitID,PriceUnit,MaxLeaveQuantityUnit,MinVolumeMultiples,BreedClassValidID,BreedClassHighLowID");
            strSql.Append(" FROM XH_SpotTradeRules ");
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
        public List<ManagementCenter.Model.XH_SpotTradeRules> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select MinChangePrice,FundDeliveryInstitution,StockDeliveryInstitution,BreedClassID,IsSlew,MaxLeaveQuantity,ValueTypeMinChangePrice,MarketUnitID,PriceUnit,MaxLeaveQuantityUnit,MinVolumeMultiples,BreedClassValidID,BreedClassHighLowID");
            strSql.Append(" FROM XH_SpotTradeRules ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.XH_SpotTradeRules> list = new List<ManagementCenter.Model.XH_SpotTradeRules>();
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

        #region 获取所有现货交易规则

        /// <summary>
        /// 获取所有现货交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllSpotTradeRules(string BreedClassName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            //条件查询
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                SQL_SELECT_XHSPOTTRADERULES += "AND (BreedClassName LIKE  '%' + @BreedClassName + '%') ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_XHSPOTTRADERULES);
            if (BreedClassName != AppGlobalVariable.INIT_STRING && BreedClassName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BreedClassName", DbType.String, BreedClassName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_XHSPOTTRADERULES, pageNo, pageSize,
                                        out rowCount, "XH_SpotTradeRules");
        }

        #endregion

        #region 根据品种ID返回现货规则明细(此品种的涨跌幅和有效申报)数据
        /// <summary>
        /// 根据品种ID返回现货规则明细(此品种的涨跌幅和有效申报)数据
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public DataSet GetSpotTradeRulesDetail(int BreedClassID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_XHSPOTTRADERULESDetail);
            database.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);

            return database.ExecuteDataSet(dbCommand);
        }
        #endregion

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.XH_SpotTradeRules ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.XH_SpotTradeRules model = new ManagementCenter.Model.XH_SpotTradeRules();
            object ojb;
            ojb = dataReader["MinChangePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MinChangePrice = (decimal) ojb;
            }
            ojb = dataReader["FundDeliveryInstitution"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FundDeliveryInstitution = (int) ojb;
            }
            ojb = dataReader["StockDeliveryInstitution"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StockDeliveryInstitution = (int) ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int) ojb;
            }
            ojb = dataReader["IsSlew"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSlew = (int) ojb;
            }
            ojb = dataReader["MaxLeaveQuantity"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MaxLeaveQuantity = (int) ojb;
            }
            ojb = dataReader["ValueTypeMinChangePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ValueTypeMinChangePrice = (int) ojb;
            }

            ojb = dataReader["MarketUnitID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketUnitID = (int) ojb;
            }

            ojb = dataReader["PriceUnit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PriceUnit = (int) ojb;
            }
            ojb = dataReader["MaxLeaveQuantityUnit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MaxLeaveQuantityUnit = (int) ojb;
            }

            ojb = dataReader["MinVolumeMultiples"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MinVolumeMultiples = (int) ojb;
            }

            ojb = dataReader["BreedClassValidID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassValidID = (int) ojb;
            }
            ojb = dataReader["BreedClassHighLowID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassHighLowID = (int) ojb;
            }
            return model;
        }

        /// <summary>
        /// 根据现货规则表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetBreedClassNameByBreedClassID()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAME_XHSPOTTRADERULES);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion  成员方法
    }
}