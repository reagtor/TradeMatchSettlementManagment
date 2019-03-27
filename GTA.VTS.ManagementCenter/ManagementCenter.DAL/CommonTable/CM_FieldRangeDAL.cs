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
    ///描述：字段_范围 数据访问类CM_FieldRangeDAL。
    ///作者：刘书伟
    ///日期:2008-11-20
    /// </summary>
    public class CM_FieldRangeDAL
    {
        public CM_FieldRangeDAL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(FieldRangeID)+1 from CM_FieldRange";
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
        public bool Exists(int FieldRangeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CM_FieldRange where FieldRangeID=@FieldRangeID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
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
        public int Add(ManagementCenter.Model.CM_FieldRange model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CM_FieldRange(");
            strSql.Append("UpperLimit,LowerLimit,UpperLimitIfEquation,LowerLimitIfEquation)");

            strSql.Append(" values (");
            strSql.Append("@UpperLimit,@LowerLimit,@UpperLimitIfEquation,@LowerLimitIfEquation)");
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
            if (model.LowerLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, model.LowerLimit);
            }
            db.AddInParameter(dbCommand, "UpperLimitIfEquation", DbType.Int32, model.UpperLimitIfEquation);
            db.AddInParameter(dbCommand, "LowerLimitIfEquation", DbType.Int32, model.LowerLimitIfEquation);
            int result;
            //object obj = db.ExecuteScalar(dbCommand);
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand, tran);
            }
            if (!int.TryParse(obj.ToString(), out result))
            {
                //return 0;
                return AppGlobalVariable.INIT_INT;
            }
            return result;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.CM_FieldRange model)
        {
            try
            {
                return Add(model, null, null);
            }
            catch (Exception)
            {
                return AppGlobalVariable.INIT_INT;
                //throw;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ManagementCenter.Model.CM_FieldRange model, DbTransaction tran,
                           Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CM_FieldRange set ");
            strSql.Append("UpperLimit=@UpperLimit,");
            strSql.Append("LowerLimit=@LowerLimit,");
            strSql.Append("UpperLimitIfEquation=@UpperLimitIfEquation,");
            strSql.Append("LowerLimitIfEquation=@LowerLimitIfEquation");
            strSql.Append(" where FieldRangeID=@FieldRangeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, model.FieldRangeID);
            if (model.UpperLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, model.UpperLimit);
            }
            if (model.LowerLimit == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, model.LowerLimit);
            }
            db.AddInParameter(dbCommand, "UpperLimitIfEquation", DbType.Int32, model.UpperLimitIfEquation);
            db.AddInParameter(dbCommand, "LowerLimitIfEquation", DbType.Int32, model.LowerLimitIfEquation);
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
        /// 更新字段范围
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.CM_FieldRange model)
        {
            return Update(model, null, null);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int FieldRangeID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete CM_FieldRange ");
            strSql.Append(" where FieldRangeID=@FieldRangeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
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

        #region 根据字段范围ID，删除字段范围值

        /// <summary>
        /// 根据字段范围ID，删除字段范围值
        /// </summary>
        /// <param name="FieldRangeID">字段范围ID</param>
        /// <returns></returns>
        public bool Delete(int FieldRangeID)
        {
            return Delete(FieldRangeID, null, null);
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_FieldRange GetModel(int FieldRangeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select FieldRangeID,UpperLimit,LowerLimit,UpperLimitIfEquation,LowerLimitIfEquation from CM_FieldRange ");
            strSql.Append(" where FieldRangeID=@FieldRangeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
            ManagementCenter.Model.CM_FieldRange model = null;
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
                "select FieldRangeID,UpperLimit,LowerLimit,UpperLimitIfEquation,LowerLimitIfEquation ");
            strSql.Append(" FROM CM_FieldRange ");
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
        public List<ManagementCenter.Model.CM_FieldRange> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select FieldRangeID,UpperLimit,LowerLimit,UpperLimitIfEquation,LowerLimitIfEquation ");
            strSql.Append(" FROM CM_FieldRange ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.CM_FieldRange> list = new List<ManagementCenter.Model.CM_FieldRange>();
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
        public ManagementCenter.Model.CM_FieldRange ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.CM_FieldRange model = new ManagementCenter.Model.CM_FieldRange();
            object ojb;
            ojb = dataReader["FieldRangeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FieldRangeID = (int) ojb;
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
            ojb = dataReader["UpperLimitIfEquation"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UpperLimitIfEquation = (int) ojb;
            }
            ojb = dataReader["LowerLimitIfEquation"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LowerLimitIfEquation = (int) ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}