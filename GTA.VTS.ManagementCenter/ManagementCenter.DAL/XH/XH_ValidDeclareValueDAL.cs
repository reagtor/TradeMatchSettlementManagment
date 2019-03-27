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
    ///描述：有效申报取值 数据访问类XH_ValidDeclareValue。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class XH_ValidDeclareValueDAL
    {
        public XH_ValidDeclareValueDAL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(ValidDeclareValueID)+1 from XH_ValidDeclareValue";
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
        public bool Exists(int ValidDeclareValueID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_ValidDeclareValue where ValidDeclareValueID=@ValidDeclareValueID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ValidDeclareValueID", DbType.Int32, ValidDeclareValueID);
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
        public bool Add(ManagementCenter.Model.XH_ValidDeclareValue model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_ValidDeclareValue(");
            strSql.Append("UpperLimit,LowerLimit,NewDayUpperLimit,NewDayLowerLimit,BreedClassValidID)");

            strSql.Append(" values (");
            strSql.Append("@UpperLimit,@LowerLimit,@NewDayUpperLimit,@NewDayLowerLimit,@BreedClassValidID)");
            strSql.Append(";select @@IDENTITY");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if (model.UpperLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, model.UpperLimit);
            }
            if(model.UpperLimit==AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, model.LowerLimit);
            }
            //db.AddInParameter(dbCommand, "IsMarketNewDay", DbType.Int32, model.IsMarketNewDay);
            if (model.NewDayUpperLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewDayUpperLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewDayUpperLimit", DbType.Decimal, model.NewDayUpperLimit);
            }
            if (model.NewDayLowerLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewDayLowerLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewDayLowerLimit", DbType.Decimal, model.NewDayLowerLimit);
            }
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, model.BreedClassValidID);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
            return true;
            //  return obj;
        }

        /// <summary>
        /// 添加有效申报(重载,无事务)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.XH_ValidDeclareValue model)
        {
            return Add(model, null, null);
        }

        #region 更新有效申报取值

        /// <summary>
        /// 更新有效申报取值
        /// </summary>
        /// <param name="model">有效申报取值实体类</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_ValidDeclareValue model, DbTransaction tran,
                           Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_ValidDeclareValue set ");
            strSql.Append("UpperLimit=@UpperLimit,");
            strSql.Append("LowerLimit=@LowerLimit,");
            strSql.Append("NewDayUpperLimit=@NewDayUpperLimit,");
            strSql.Append("NewDayLowerLimit=@NewDayLowerLimit");
            strSql.Append(" where BreedClassValidID=@BreedClassValidID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if (model.UpperLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, model.UpperLimit);
            }
            if (model.UpperLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, model.LowerLimit);
            }
            if (model.NewDayUpperLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewDayUpperLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewDayUpperLimit", DbType.Decimal, model.NewDayUpperLimit);
            }
            if (model.NewDayLowerLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "NewDayLowerLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "NewDayLowerLimit", DbType.Decimal, model.NewDayLowerLimit);
            }
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, model.BreedClassValidID);
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

        #region 更新有效申报取值

        /// <summary>
        /// 更新有效申报取值
        /// </summary>
        /// <param name="model">有效申报取值实体类</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_ValidDeclareValue model)
        {
            return Update(model, null, null);
        }

        #endregion

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ValidDeclareValueID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_ValidDeclareValue ");
            strSql.Append(" where ValidDeclareValueID=@ValidDeclareValueID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ValidDeclareValueID", DbType.Int32, ValidDeclareValueID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_ValidDeclareValue GetModel(int ValidDeclareValueID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select ValidDeclareValueID,UpperLimit,LowerLimit,NewDayUpperLimit,NewDayLowerLimit,BreedClassValidID from XH_ValidDeclareValue ");
            strSql.Append(" where ValidDeclareValueID=@ValidDeclareValueID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ValidDeclareValueID", DbType.Int32, ValidDeclareValueID);
            ManagementCenter.Model.XH_ValidDeclareValue model = null;
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
            strSql.Append("select ValidDeclareValueID,UpperLimit,LowerLimit,NewDayUpperLimit,NewDayLowerLimit,BreedClassValidID ");
            strSql.Append(" FROM XH_ValidDeclareValue ");
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
        public List<ManagementCenter.Model.XH_ValidDeclareValue> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ValidDeclareValueID,UpperLimit,LowerLimit,NewDayUpperLimit,NewDayLowerLimit,BreedClassValidID ");
            strSql.Append(" FROM XH_ValidDeclareValue ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.XH_ValidDeclareValue> list =
                new List<ManagementCenter.Model.XH_ValidDeclareValue>();
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
        public ManagementCenter.Model.XH_ValidDeclareValue ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.XH_ValidDeclareValue model = new ManagementCenter.Model.XH_ValidDeclareValue();
            object ojb;
            ojb = dataReader["ValidDeclareValueID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ValidDeclareValueID = (int) ojb;
            }
            ojb = dataReader["UpperLimit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UpperLimit = (decimal) ojb;
            }
            ojb = dataReader["LowerLimit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LowerLimit = (decimal) ojb;
            }
            ojb = dataReader["NewDayUpperLimit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.NewDayUpperLimit = (decimal)ojb;
            }
            ojb = dataReader["NewDayLowerLimit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.NewDayLowerLimit = (decimal)ojb;
            }
            ojb = dataReader["BreedClassValidID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassValidID = (int) ojb;
            }
            return model;
        }

        #endregion  成员方法

        #region  根据品种有效申报标识删除有效申报取值

        /// <summary>
        /// 根据品种有效申报标识删除有效申报取值
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool DeleteVDeclareValue(int BreedClassValidID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_ValidDeclareValue ");
            strSql.Append(" where BreedClassValidID=@BreedClassValidID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, BreedClassValidID);
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

        #region 根据品种有效申报标识删除有效申报取值(无事务)

        /// <summary>
        /// 根据品种有效申报标识删除有效申报取值
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public bool DeleteVDeclareValueByBCValidID(int BreedClassValidID)
        {
            return DeleteVDeclareValue(BreedClassValidID, null, null);
        }

        #endregion

        #region 根据品种有效申报标识获取有效申报取值实体
        /// <summary>
       /// 根据品种有效申报标识获取有效申报取值实体
       /// </summary>
       /// <param name="BreedClassValidID">品种有效申报标识</param>
       /// <returns></returns>
        public ManagementCenter.Model.XH_ValidDeclareValue GetModelValidDeclareValue(int BreedClassValidID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select ValidDeclareValueID,UpperLimit,LowerLimit,NewDayUpperLimit,NewDayLowerLimit,BreedClassValidID from XH_ValidDeclareValue ");
            strSql.Append(" where BreedClassValidID=@BreedClassValidID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, BreedClassValidID);
            ManagementCenter.Model.XH_ValidDeclareValue model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }
        #endregion
    }
}