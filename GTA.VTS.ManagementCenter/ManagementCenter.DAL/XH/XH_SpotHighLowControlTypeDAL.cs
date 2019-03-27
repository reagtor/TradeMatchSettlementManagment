using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility; //请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述：现货_品种_涨跌幅_控制类型 数据访问类XH_SpotHighLowControlTypeDAL。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class XH_SpotHighLowControlTypeDAL
    {
        public XH_SpotHighLowControlTypeDAL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassHighLowID)+1 from XH_SpotHighLowControlType";
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
        public bool Exists(int BreedClassHighLowID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from XH_SpotHighLowControlType where BreedClassHighLowID=@BreedClassHighLowID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassHighLowID", DbType.Int32, BreedClassHighLowID);
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
        /// 增加涨跌幅控制类型
        /// </summary>
        public int Add(ManagementCenter.Model.XH_SpotHighLowControlType model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_SpotHighLowControlType(");
            strSql.Append("HighLowTypeID)");

            strSql.Append(" values (");
            strSql.Append("@HighLowTypeID)");
            strSql.Append(";select @@IDENTITY");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HighLowTypeID", DbType.Int32, model.HighLowTypeID);
            //int result;
            //object obj = db.ExecuteScalar(dbCommand);
            //if (!int.TryParse(obj.ToString(), out result))
            //{
            //    return 0;
            //}

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
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 增加涨跌幅控制类型(重载,无事务)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.XH_SpotHighLowControlType model)
        {
            return Add(model, null, null);
        }

        #region  更新涨跌幅控制类型

        /// <summary>
        /// 更新涨跌幅控制类型
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_SpotHighLowControlType model, DbTransaction tran,
                           Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_SpotHighLowControlType set ");
            strSql.Append("HighLowTypeID=@HighLowTypeID");
            strSql.Append(" where BreedClassHighLowID=@BreedClassHighLowID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HighLowTypeID", DbType.Int32, model.HighLowTypeID);
            db.AddInParameter(dbCommand, "BreedClassHighLowID", DbType.Int32, model.BreedClassHighLowID);
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

        #endregion

        #region  更新涨跌幅控制类型

        /// <summary>
        /// 更新涨跌幅控制类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_SpotHighLowControlType model)
        {
            return Update(model, null, null);
        }

        #endregion

        #region 根据品种涨跌幅标识删除涨跌幅控制类型

        /// <summary>
        /// 根据品种涨跌幅标识删除涨跌幅控制类型
        /// </summary>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int BreedClassHighLowID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_SpotHighLowControlType ");
            strSql.Append(" where BreedClassHighLowID=@BreedClassHighLowID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassHighLowID", DbType.Int32, BreedClassHighLowID);
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

        #region 根据品种涨跌幅标识删除涨跌幅控制类型(无事务)

        /// <summary>
        /// 根据品种涨跌幅标识删除涨跌幅控制类型
        /// </summary>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <returns></returns>
        public bool Delete(int BreedClassHighLowID)
        {
            return Delete(BreedClassHighLowID, null, null);
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_SpotHighLowControlType GetModel(int BreedClassHighLowID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HighLowTypeID,BreedClassHighLowID from XH_SpotHighLowControlType ");
            strSql.Append(" where BreedClassHighLowID=@BreedClassHighLowID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassHighLowID", DbType.Int32, BreedClassHighLowID);
            ManagementCenter.Model.XH_SpotHighLowControlType model = null;
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
            strSql.Append("select HighLowTypeID,BreedClassHighLowID ");
            strSql.Append(" FROM XH_SpotHighLowControlType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "XH_SpotHighLowControlType");
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
        public List<ManagementCenter.Model.XH_SpotHighLowControlType> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HighLowTypeID,BreedClassHighLowID ");
            strSql.Append(" FROM XH_SpotHighLowControlType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.XH_SpotHighLowControlType> list =
                new List<ManagementCenter.Model.XH_SpotHighLowControlType>();
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
        public ManagementCenter.Model.XH_SpotHighLowControlType ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.XH_SpotHighLowControlType model =
                new ManagementCenter.Model.XH_SpotHighLowControlType();
            object ojb;
            ojb = dataReader["HighLowTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HighLowTypeID = (int) ojb;
            }
            ojb = dataReader["BreedClassHighLowID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassHighLowID = (int) ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}