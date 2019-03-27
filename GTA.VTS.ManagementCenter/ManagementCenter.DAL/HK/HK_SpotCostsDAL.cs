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
    ///描述：港股_交易费用 数据访问类HK_SpotCostsDAL。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_SpotCostsDAL
    {
        public HK_SpotCostsDAL()
        {

        }

        #region SQL

        /// <summary>
        /// 根据查询条件返回港股交易费用数据
        /// </summary>
        private string SQL_SELECT_HKSPOTCOSTS =
            @"SELECT B.BREEDCLASSNAME,A.* FROM 
                                                 HK_SPOTCOSTS AS A,CM_BREEDCLASS AS B 
                                                 WHERE A.BREEDCLASSID=B.BREEDCLASSID ";

        /// <summary>
        /// 根据港股交易费用表中的品种标识获取品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAME_HKSPOTCOSTS =
            @"SELECT A.BREEDCLASSID,A.BREEDCLASSNAME 
			                                                    FROM CM_BREEDCLASS A,HK_SPOTCOSTS B 
			                                                    WHERE A.BREEDCLASSID=B.BREEDCLASSID";

        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from HK_SpotCosts";
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
            strSql.Append("select count(1) from HK_SpotCosts where BreedClassID=@BreedClassID ");
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
        /// 添加港股交易费用
        /// </summary>
        /// <param name="model">港股交易费用实体</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.HK_SpotCosts model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_SpotCosts(");
            strSql.Append("StampDuty,StampDutyStartingpoint,Commision,MonitoringFee,CommisionStartingpoint,PoundageValue,SystemToll,StampDutyTypeID,BreedClassID,CurrencyTypeID,TransferToll)");

            strSql.Append(" values (");
            strSql.Append("@StampDuty,@StampDutyStartingpoint,@Commision,@MonitoringFee,@CommisionStartingpoint,@PoundageValue,@SystemToll,@StampDutyTypeID,@BreedClassID,@CurrencyTypeID,@TransferToll)");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StampDuty", DbType.Decimal, model.StampDuty);
            //db.AddInParameter(dbCommand, "StampDutyStartingpoint", DbType.Decimal, model.StampDutyStartingpoint);
            //if (model.StampDutyStartingpoint == AppGlobalVariable.INIT_DECIMAL)
            //{
            //    db.AddInParameter(dbCommand, "StampDutyStartingpoint", DbType.Decimal, DBNull.Value);
            //}
            //else
            //{
            db.AddInParameter(dbCommand, "StampDutyStartingpoint", DbType.Decimal, model.StampDutyStartingpoint);//界面上传值默认为0
            //}
            db.AddInParameter(dbCommand, "Commision", DbType.Decimal, model.Commision);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            db.AddInParameter(dbCommand, "CommisionStartingpoint", DbType.Decimal, model.CommisionStartingpoint);//界面上传值默认为0
            db.AddInParameter(dbCommand, "PoundageValue", DbType.Decimal, model.PoundageValue);
            db.AddInParameter(dbCommand, "SystemToll", DbType.Decimal, model.SystemToll);
            db.AddInParameter(dbCommand, "StampDutyTypeID", DbType.Int32, model.StampDutyTypeID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            // db.AddInParameter(dbCommand, "TransferToll", DbType.Decimal, model.TransferToll);
            if (model.TransferToll == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "TransferToll", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "TransferToll", DbType.Decimal, model.TransferToll);
            }
            // db.ExecuteNonQuery(dbCommand);
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
        /// 添加港股交易费用
        /// </summary>
        /// <param name="model">港股交易费用实体</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.HK_SpotCosts model)
        {
            return Add(model, null, null);
        }

        /// <summary>
        /// 更新港股交易费用
        /// </summary>
        /// <param name="model">港股交易费用实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.HK_SpotCosts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_SpotCosts set ");
            strSql.Append("StampDuty=@StampDuty,");
            strSql.Append("StampDutyStartingpoint=@StampDutyStartingpoint,");
            strSql.Append("Commision=@Commision,");
            strSql.Append("MonitoringFee=@MonitoringFee,");
            strSql.Append("CommisionStartingpoint=@CommisionStartingpoint,");
            strSql.Append("PoundageValue=@PoundageValue,");
            strSql.Append("SystemToll=@SystemToll,");
            strSql.Append("StampDutyTypeID=@StampDutyTypeID,");
            strSql.Append("CurrencyTypeID=@CurrencyTypeID,");
            strSql.Append("TransferToll=@TransferToll");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StampDuty", DbType.Decimal, model.StampDuty);
            //db.AddInParameter(dbCommand, "StampDutyStartingpoint", DbType.Decimal, model.StampDutyStartingpoint);
            db.AddInParameter(dbCommand, "StampDutyStartingpoint", DbType.Decimal, model.StampDutyStartingpoint);//界面上传值默认为0
            db.AddInParameter(dbCommand, "Commision", DbType.Decimal, model.Commision);
            db.AddInParameter(dbCommand, "MonitoringFee", DbType.Decimal, model.MonitoringFee);
            //db.AddInParameter(dbCommand, "CommisionStartingpoint", DbType.Decimal, model.CommisionStartingpoint);
            db.AddInParameter(dbCommand, "CommisionStartingpoint", DbType.Decimal, model.CommisionStartingpoint);//界面上传值默认为0
            db.AddInParameter(dbCommand, "PoundageValue", DbType.Decimal, model.PoundageValue);
            db.AddInParameter(dbCommand, "SystemToll", DbType.Decimal, model.SystemToll);
            db.AddInParameter(dbCommand, "StampDutyTypeID", DbType.Int32, model.StampDutyTypeID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            //db.AddInParameter(dbCommand, "TransferToll", DbType.Decimal, model.TransferToll);
            if (model.TransferToll == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "TransferToll", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "TransferToll", DbType.Decimal, model.TransferToll);
            }
            db.ExecuteNonQuery(dbCommand);
            return true;

        }

        /// <summary>
        /// 根据品种ID删除港股交易费用
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HK_SpotCosts ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            // db.ExecuteNonQuery(dbCommand);
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
        /// 根据品种ID删除港股交易费用
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
        public ManagementCenter.Model.HK_SpotCosts GetModel(int BreedClassID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StampDuty,StampDutyStartingpoint,Commision,MonitoringFee,CommisionStartingpoint,PoundageValue,SystemToll,StampDutyTypeID,BreedClassID,CurrencyTypeID,TransferToll from HK_SpotCosts ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            ManagementCenter.Model.HK_SpotCosts model = null;
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
            strSql.Append("select StampDuty,StampDutyStartingpoint,Commision,MonitoringFee,CommisionStartingpoint,PoundageValue,SystemToll,StampDutyTypeID,BreedClassID,CurrencyTypeID,TransferToll ");
            strSql.Append(" FROM HK_SpotCosts ");
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
        public List<ManagementCenter.Model.HK_SpotCosts> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StampDuty,StampDutyStartingpoint,Commision,MonitoringFee,CommisionStartingpoint,PoundageValue,SystemToll,StampDutyTypeID,BreedClassID,CurrencyTypeID,TransferToll ");
            strSql.Append(" FROM HK_SpotCosts ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HK_SpotCosts> list = new List<ManagementCenter.Model.HK_SpotCosts>();
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

        #region 获取所有港股交易费用

        /// <summary>
        /// 获取所有港股交易费用
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllHKSpotCosts(string BreedClassName, int pageNo, int pageSize,
                                       out int rowCount)
        {
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                SQL_SELECT_HKSPOTCOSTS += "AND (BreedClassName LIKE  '%' + @BreedClassName + '%') ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_HKSPOTCOSTS);

            if (BreedClassName != AppGlobalVariable.INIT_STRING && BreedClassName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BreedClassName", DbType.String, BreedClassName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_HKSPOTCOSTS, pageNo, pageSize,
                                        out rowCount, "HK_SpotCosts");
        }

        #endregion

        #region 根据港股交易费用表中的品种ID获取品种名称

        /// <summary>
        /// 根据港股交易费用表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetHKSpotCostsBreedClassName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAME_HKSPOTCOSTS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion


        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.HK_SpotCosts ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.HK_SpotCosts model = new ManagementCenter.Model.HK_SpotCosts();
            object ojb;
            ojb = dataReader["StampDuty"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StampDuty = (decimal)ojb;
            }
            ojb = dataReader["StampDutyStartingpoint"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StampDutyStartingpoint = (decimal)ojb;
            }
            ojb = dataReader["Commision"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Commision = (decimal)ojb;
            }
            ojb = dataReader["MonitoringFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MonitoringFee = (decimal)ojb;
            }
            ojb = dataReader["CommisionStartingpoint"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CommisionStartingpoint = (decimal)ojb;
            }
            ojb = dataReader["PoundageValue"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PoundageValue = (decimal)ojb;
            }
            ojb = dataReader["SystemToll"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SystemToll = (decimal)ojb;
            }
            ojb = dataReader["StampDutyTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StampDutyTypeID = (int)ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            ojb = dataReader["CurrencyTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeID = (int)ojb;
            }
            ojb = dataReader["TransferToll"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TransferToll = (decimal)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}
