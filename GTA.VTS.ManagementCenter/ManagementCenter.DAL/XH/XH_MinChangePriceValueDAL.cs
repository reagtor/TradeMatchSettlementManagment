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
    ///描述：交易规则_最小变动价位_范围_值  数据访问类XH_MinChangePriceValueDAL。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class XH_MinChangePriceValueDAL
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_MinChangePriceValueDAL()
        {
        }
        #endregion

        #region SQL

        /// <summary>
        /// 根据品种ID获取交易规则_最小变动价位_范围_值
        /// </summary>
        private string SQL_SELECT_MINCHANGEPRICEFIELDRANGE =
            @"SELECT A.*,B.[VALUE],B.BREEDCLASSID FROM 
                                                CM_FIELDRANGE A,XH_MINCHANGEPRICEVALUE B
                                                WHERE A.FIELDRANGEID=B.FIELDRANGEID ";

        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from XH_MinChangePriceValue";
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
        public bool Exists(int BreedClassID, int FieldRangeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from XH_MinChangePriceValue where BreedClassID=@BreedClassID and FieldRangeID=@FieldRangeID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
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
        public bool Add(ManagementCenter.Model.XH_MinChangePriceValue model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_MinChangePriceValue(");
            strSql.Append("Value,BreedClassID,FieldRangeID)");

            strSql.Append(" values (");
            strSql.Append("@Value,@BreedClassID,@FieldRangeID)");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Value", DbType.Currency, model.Value);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, model.FieldRangeID);
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

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(ManagementCenter.Model.XH_MinChangePriceValue model)
        {
            return Add(model, null, null);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ManagementCenter.Model.XH_MinChangePriceValue model, DbTransaction tran,
                           Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_MinChangePriceValue set ");
            strSql.Append("Value=@Value");
            strSql.Append(" where BreedClassID=@BreedClassID and FieldRangeID=@FieldRangeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Value", DbType.Currency, model.Value);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, model.FieldRangeID);
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

        #region 更新交易规则_最小变动价位_范围_值

        /// <summary>
        /// 更新交易规则_最小变动价位_范围_值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_MinChangePriceValue model)
        {
            return Update(model);
        }

        #endregion

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int BreedClassID, int FieldRangeID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_MinChangePriceValue ");
            strSql.Append(" where BreedClassID=@BreedClassID and FieldRangeID=@FieldRangeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
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

        #region 根据字段范围ID，品种ID删除最小变动价位_范围_值

        /// <summary>
        /// 根据字段范围ID，品种ID删除最小变动价位_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="FieldRangeID">字段范围ID</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, int FieldRangeID)
        {
            return Delete(BreedClassID, FieldRangeID, null, null);
        }

        #endregion


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int BreedClassID,DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_MinChangePriceValue ");
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


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_MinChangePriceValue GetModel(int BreedClassID, int FieldRangeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value,BreedClassID,FieldRangeID from XH_MinChangePriceValue ");
            strSql.Append(" where BreedClassID=@BreedClassID and FieldRangeID=@FieldRangeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
            ManagementCenter.Model.XH_MinChangePriceValue model = null;
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
            strSql.Append("select Value,BreedClassID,FieldRangeID ");
            strSql.Append(" FROM XH_MinChangePriceValue ");
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
        public List<ManagementCenter.Model.XH_MinChangePriceValue> GetListArray(string strWhere,DbTransaction tran,Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value,BreedClassID,FieldRangeID ");
            strSql.Append(" FROM XH_MinChangePriceValue ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.XH_MinChangePriceValue> list =
                new List<ManagementCenter.Model.XH_MinChangePriceValue>();
            if(db==null) db = DatabaseFactory.CreateDatabase();
            if (tran==null)
            { 
                using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))
                {
                    while (dataReader.Read())
                    {
                        list.Add(ReaderBind(dataReader));
                    }
                }
            }
            else
            {
                using (IDataReader dataReader = db.ExecuteReader(tran,CommandType.Text, strSql.ToString()))
                {
                    while (dataReader.Read())
                    {
                        list.Add(ReaderBind(dataReader));
                    }
                }
            }
           
            return list;
        }

        /// <summary>
        /// 根据查询条件获取交易规则_最小变动价位_范围_值
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_MinChangePriceValue> GetListArray(string strWhere)
        {
            return GetListArray(strWhere, null, null);
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.XH_MinChangePriceValue ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.XH_MinChangePriceValue model = new ManagementCenter.Model.XH_MinChangePriceValue();
            object ojb;
            ojb = dataReader["Value"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Value = (decimal) ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int) ojb;
            }
            ojb = dataReader["FieldRangeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FieldRangeID = (int) ojb;
            }
            return model;
        }

        #region 根据品种ID获取交易规则_最小变动价位_范围_值

        /// <summary>
        /// 根据品种ID获取交易规则_最小变动价位_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public DataSet GetMinChangePriceFieldRangeByBreedClassID(int BreedClassID)
        {
            if (BreedClassID != AppGlobalVariable.INIT_INT) 
            {
                SQL_SELECT_MINCHANGEPRICEFIELDRANGE += " AND (B.BreedClassID=@BreedClassID ) ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_MINCHANGEPRICEFIELDRANGE);
            database.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);

            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #endregion  成员方法
    }
}