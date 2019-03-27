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
    ///描述：期货_品种_交易规则 数据访问类QH_FuturesTradeRulesDAL。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// Desc: 增加了字段DeliveryMonthHighLowStopValue相关处理
    /// Update By: 董鹏
    /// Update Date: 2010-01-21
    /// </summary>
    public class QH_FuturesTradeRulesDAL
    {
        public QH_FuturesTradeRulesDAL()
        {
        }

        #region SQL

        /// <summary>
        ///根据查询条件返回期货_品种_交易规则
        /// </summary>
        private string SQL_SELECT_QHFUTURESTRADERULES =
            @" SELECT A.BREEDCLASSNAME,B.*
                                                                FROM CM_BREEDCLASS A,QH_FUTURESTRADERULES B 
                                                                WHERE A.BREEDCLASSID=B.BREEDCLASSID ";

        /// <summary>
        /// 根据期货_品种_交易规则表中的品种标识获取品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAME_QHFUTURESTRADERULES =
            @" SELECT A.BREEDCLASSID,A.BREEDCLASSNAME 
                                                                    FROM CM_BREEDCLASS A,QH_FUTURESTRADERULES B 
                                                                    WHERE A.BREEDCLASSID=B.BREEDCLASSID AND A.BREEDCLASSTYPEID<>1";

        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from QH_FuturesTradeRules";
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
            strSql.Append("select count(1) from QH_FuturesTradeRules where BreedClassID=@BreedClassID ");
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

        #region 添加期货_品种_交易规则

        /// <summary>
        /// 添加期货_品种_交易规则
        /// </summary>
        /// <param name="model">期货_品种_交易规则实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.QH_FuturesTradeRules model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_FuturesTradeRules(");
            strSql.Append(
                "UnitMultiple,LeastChangePrice,IfContainCNewYear,AgreementDeliveryInstitution,FundDeliveryInstitution,BreedClassID,LastTradingDayID,ConsignQuantumID,IsSlew,HighLowStopScopeID,HighLowStopScopeValue,NewBreedFuturesPactHighLowStopValue,NewMonthFuturesPactHighLowStopValue,UnitsID,PriceUnit,MarketUnitID,FutruesCode,DeliveryMonthHighLowStopValue)");

            strSql.Append(" values (");
            strSql.Append(
                "@UnitMultiple,@LeastChangePrice,@IfContainCNewYear,@AgreementDeliveryInstitution,@FundDeliveryInstitution,@BreedClassID,@LastTradingDayID,@ConsignQuantumID,@IsSlew,@HighLowStopScopeID,@HighLowStopScopeValue,@NewBreedFuturesPactHighLowStopValue,@NewMonthFuturesPactHighLowStopValue,@UnitsID,@PriceUnit,@MarketUnitID,@FutruesCode,@DeliveryMonthHighLowStopValue)");

            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UnitMultiple", DbType.Decimal, model.UnitMultiple);
            db.AddInParameter(dbCommand, "LeastChangePrice", DbType.Currency, model.LeastChangePrice);
            db.AddInParameter(dbCommand, "IfContainCNewYear", DbType.Int32, model.IfContainCNewYear);
            db.AddInParameter(dbCommand, "AgreementDeliveryInstitution", DbType.Int32,
                              model.AgreementDeliveryInstitution);
            db.AddInParameter(dbCommand, "FundDeliveryInstitution", DbType.Int32, model.FundDeliveryInstitution);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "LastTradingDayID", DbType.Int32, model.LastTradingDayID);
            db.AddInParameter(dbCommand, "ConsignQuantumID", DbType.Int32, model.ConsignQuantumID);
            db.AddInParameter(dbCommand, "IsSlew", DbType.Int32, model.IsSlew);
            db.AddInParameter(dbCommand, "HighLowStopScopeID", DbType.Int32, model.HighLowStopScopeID);
            if (model.HighLowStopScopeValue == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "HighLowStopScopeValue", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "HighLowStopScopeValue", DbType.Decimal, model.HighLowStopScopeValue);
            }
            if (model.NewBreedFuturesPactHighLowStopValue == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewBreedFuturesPactHighLowStopValue", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewBreedFuturesPactHighLowStopValue", DbType.Decimal,
                                  model.NewBreedFuturesPactHighLowStopValue);
            }
            if (model.NewMonthFuturesPactHighLowStopValue == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewMonthFuturesPactHighLowStopValue", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewMonthFuturesPactHighLowStopValue", DbType.Decimal,
                                  model.NewMonthFuturesPactHighLowStopValue);
            }
            db.AddInParameter(dbCommand, "UnitsID", DbType.Int32, model.UnitsID);
            db.AddInParameter(dbCommand, "PriceUnit", DbType.Int32, model.PriceUnit);
            db.AddInParameter(dbCommand, "MarketUnitID", DbType.Int32, model.MarketUnitID);
            db.AddInParameter(dbCommand, "FutruesCode", DbType.String, model.FutruesCode);
            db.AddInParameter(dbCommand, "DeliveryMonthHighLowStopValue", DbType.Int32, model.DeliveryMonthHighLowStopValue);
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

        #endregion

        #region 添加期货_品种_交易规则

        /// <summary>
        /// 添加期货_品种_交易规则
        /// </summary>
        /// <param name="model">期货_品种_交易规则实体</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.QH_FuturesTradeRules model)
        {
            return Add(model, null, null);
        }

        #endregion

        #region 更新期货_品种_交易规则

        /// <summary>
        /// 更新期货_品种_交易规则
        /// </summary>
        /// <param name="model">期货_品种_交易规则实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_FuturesTradeRules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_FuturesTradeRules set ");
            strSql.Append("UnitMultiple=@UnitMultiple,");
            strSql.Append("LeastChangePrice=@LeastChangePrice,");
            strSql.Append("IfContainCNewYear=@IfContainCNewYear,");
            strSql.Append("AgreementDeliveryInstitution=@AgreementDeliveryInstitution,");
            strSql.Append("FundDeliveryInstitution=@FundDeliveryInstitution,");
            strSql.Append("LastTradingDayID=@LastTradingDayID,");
            strSql.Append("ConsignQuantumID=@ConsignQuantumID,");
            strSql.Append("IsSlew=@IsSlew,");
            strSql.Append("HighLowStopScopeID=@HighLowStopScopeID,");
            strSql.Append("HighLowStopScopeValue=@HighLowStopScopeValue,");
            strSql.Append("NewBreedFuturesPactHighLowStopValue=@NewBreedFuturesPactHighLowStopValue,");
            strSql.Append("NewMonthFuturesPactHighLowStopValue=@NewMonthFuturesPactHighLowStopValue,");
            strSql.Append("UnitsID=@UnitsID,");
            strSql.Append("PriceUnit=@PriceUnit,");
            strSql.Append("MarketUnitID=@MarketUnitID,");
            strSql.Append("FutruesCode=@FutruesCode,");
            strSql.Append("DeliveryMonthHighLowStopValue=@DeliveryMonthHighLowStopValue");            
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UnitMultiple", DbType.Decimal, model.UnitMultiple);
            db.AddInParameter(dbCommand, "LeastChangePrice", DbType.Currency, model.LeastChangePrice);
            db.AddInParameter(dbCommand, "IfContainCNewYear", DbType.Int32, model.IfContainCNewYear);
            db.AddInParameter(dbCommand, "AgreementDeliveryInstitution", DbType.Int32,
                              model.AgreementDeliveryInstitution);
            db.AddInParameter(dbCommand, "FundDeliveryInstitution", DbType.Int32, model.FundDeliveryInstitution);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "LastTradingDayID", DbType.Int32, model.LastTradingDayID);
            db.AddInParameter(dbCommand, "ConsignQuantumID", DbType.Int32, model.ConsignQuantumID);
            db.AddInParameter(dbCommand, "IsSlew", DbType.Int32, model.IsSlew);
            db.AddInParameter(dbCommand, "HighLowStopScopeID", DbType.Int32, model.HighLowStopScopeID);
            if (model.HighLowStopScopeValue == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "HighLowStopScopeValue", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "HighLowStopScopeValue", DbType.Decimal, model.HighLowStopScopeValue);
            }
            if (model.NewBreedFuturesPactHighLowStopValue == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewBreedFuturesPactHighLowStopValue", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewBreedFuturesPactHighLowStopValue", DbType.Decimal,
                                  model.NewBreedFuturesPactHighLowStopValue);
            }
            if (model.NewMonthFuturesPactHighLowStopValue == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewMonthFuturesPactHighLowStopValue", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewMonthFuturesPactHighLowStopValue", DbType.Decimal,
                                  model.NewMonthFuturesPactHighLowStopValue);
            }
            db.AddInParameter(dbCommand, "UnitsID", DbType.Int32, model.UnitsID);
            db.AddInParameter(dbCommand, "PriceUnit", DbType.Int32, model.PriceUnit);
            db.AddInParameter(dbCommand, "MarketUnitID", DbType.Int32, model.MarketUnitID);
            db.AddInParameter(dbCommand, "FutruesCode", DbType.String, model.FutruesCode);
            db.AddInParameter(dbCommand, "DeliveryMonthHighLowStopValue", DbType.Int32, model.DeliveryMonthHighLowStopValue);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #endregion

        #region 根据品种标识删除期货_品种_交易规则

        /// <summary>
        /// 根据品种标识删除期货_品种_交易规则
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_FuturesTradeRules ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
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

        #endregion

        #region 根据品种标识删除期货_品种_交易规则

        /// <summary>
        /// 根据品种标识删除期货_品种_交易规则
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID)
        {
            return Delete(BreedClassID);
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_FuturesTradeRules GetModel(int BreedClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select UnitMultiple,LeastChangePrice,IfContainCNewYear,AgreementDeliveryInstitution,FundDeliveryInstitution,BreedClassID,LastTradingDayID,ConsignQuantumID,IsSlew,HighLowStopScopeID,HighLowStopScopeValue,NewBreedFuturesPactHighLowStopValue,NewMonthFuturesPactHighLowStopValue,UnitsID,PriceUnit,MarketUnitID,FutruesCode,DeliveryMonthHighLowStopValue from QH_FuturesTradeRules ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            ManagementCenter.Model.QH_FuturesTradeRules model = null;
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
                "select UnitMultiple,LeastChangePrice,IfContainCNewYear,AgreementDeliveryInstitution,FundDeliveryInstitution,BreedClassID,LastTradingDayID,ConsignQuantumID,IsSlew,HighLowStopScopeID,HighLowStopScopeValue,NewBreedFuturesPactHighLowStopValue,NewMonthFuturesPactHighLowStopValue,UnitsID,PriceUnit,MarketUnitID,FutruesCode,DeliveryMonthHighLowStopValue ");
            strSql.Append(" FROM QH_FuturesTradeRules ");
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
        public List<ManagementCenter.Model.QH_FuturesTradeRules> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select UnitMultiple,LeastChangePrice,IfContainCNewYear,AgreementDeliveryInstitution,FundDeliveryInstitution,BreedClassID,LastTradingDayID,ConsignQuantumID,IsSlew,HighLowStopScopeID,HighLowStopScopeValue,NewBreedFuturesPactHighLowStopValue,NewMonthFuturesPactHighLowStopValue,UnitsID,PriceUnit,MarketUnitID,FutruesCode,DeliveryMonthHighLowStopValue ");
            strSql.Append(" FROM QH_FuturesTradeRules ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.QH_FuturesTradeRules> list =
                new List<ManagementCenter.Model.QH_FuturesTradeRules>();
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

        #region 获取所有期货_品种_交易规则

        /// <summary>
        /// 获取所有期货_品种_交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllFuturesTradeRules(string BreedClassName, int pageNo, int pageSize,
                                               out int rowCount)
        {
            //条件查询
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                SQL_SELECT_QHFUTURESTRADERULES += "AND (BreedClassName LIKE  '%' + @BreedClassName + '%') ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_QHFUTURESTRADERULES);
            if (BreedClassName != AppGlobalVariable.INIT_STRING && BreedClassName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BreedClassName", DbType.String, BreedClassName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_QHFUTURESTRADERULES, pageNo, pageSize,
                                        out rowCount, "QH_FuturesTradeRules");
        }

        #endregion

        #region 根据期货_品种_交易规则表中的品种标识获取品种名称
        /// <summary>
        /// 根据期货_品种_交易规则表中的品种标识获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHBreedClassNameByBreedClassID()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAME_QHFUTURESTRADERULES);
            return database.ExecuteDataSet(dbCommand);
        }
        #endregion


        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.QH_FuturesTradeRules ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.QH_FuturesTradeRules model = new ManagementCenter.Model.QH_FuturesTradeRules();
            object ojb;
            ojb = dataReader["UnitMultiple"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UnitMultiple = (decimal)ojb;
            }
            ojb = dataReader["LeastChangePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LeastChangePrice = (decimal)ojb;
            }
            ojb = dataReader["IfContainCNewYear"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IfContainCNewYear = (int)ojb;
            }
            ojb = dataReader["AgreementDeliveryInstitution"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AgreementDeliveryInstitution = (int)ojb;
            }
            ojb = dataReader["FundDeliveryInstitution"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FundDeliveryInstitution = (int)ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            ojb = dataReader["LastTradingDayID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LastTradingDayID = (int)ojb;
            }
            ojb = dataReader["ConsignQuantumID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ConsignQuantumID = (int)ojb;
            }
            ojb = dataReader["IsSlew"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSlew = (int)ojb;
            }
            ojb = dataReader["HighLowStopScopeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HighLowStopScopeID = (int)ojb;
            }
            ojb = dataReader["HighLowStopScopeValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HighLowStopScopeValue = (decimal)ojb;
            }
            ojb = dataReader["NewBreedFuturesPactHighLowStopValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.NewBreedFuturesPactHighLowStopValue = (decimal)ojb;
            }
            ojb = dataReader["NewMonthFuturesPactHighLowStopValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.NewMonthFuturesPactHighLowStopValue = (decimal)ojb;
            }
            ojb = dataReader["UnitsID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UnitsID = (int)ojb;
            }
            ojb = dataReader["PriceUnit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PriceUnit = (int)ojb;
            }
            ojb = dataReader["MarketUnitID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketUnitID = (int)ojb;
            }
            model.FutruesCode = dataReader["FutruesCode"].ToString();
            ojb = dataReader["DeliveryMonthHighLowStopValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DeliveryMonthHighLowStopValue = (decimal)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}