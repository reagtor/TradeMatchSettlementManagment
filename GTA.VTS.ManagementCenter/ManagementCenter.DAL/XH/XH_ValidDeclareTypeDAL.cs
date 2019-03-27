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
    ///描述：有效申报类型 数据访问类XH_ValidDeclareTypeDAL。
    ///作者：刘书伟
    ///日期:2008-12-2
    /// </summary>
    public class XH_ValidDeclareTypeDAL
    {
        public XH_ValidDeclareTypeDAL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassValidID)+1 from XH_ValidDeclareType";
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
        public bool Exists(int ValidDeclareTypeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_ValidDeclareType where BreedClassValidID=@BreedClassValidID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, ValidDeclareTypeID);
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
        public void Add(ManagementCenter.Model.XH_ValidDeclareType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_ValidDeclareType(");
            strSql.Append("ValidDeclareTypeID)");

            strSql.Append(" values (");
            strSql.Append("@ValidDeclareTypeID)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            //db.AddInParameter(dbCommand, "ValidDeclareTypeID", DbType.Int32, model.ValidDeclareTypeID);
            db.AddInParameter(dbCommand, "ValidDeclareTypeID", DbType.Int32, model.ValidDeclareTypeID);
            db.ExecuteNonQuery(dbCommand);
        }

        #region 根据品种有效申报标识更新有效申报类型

        /// <summary>
        /// 根据品种有效申报标识更新有效申报类型
        /// </summary>
        /// <param name="model">有效申报类型实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_ValidDeclareType model, DbTransaction tran,
                           Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_ValidDeclareType set ");
            strSql.Append("ValidDeclareTypeID=@ValidDeclareTypeID");
            strSql.Append(" where BreedClassValidID=@BreedClassValidID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, model.BreedClassValidID);
            db.AddInParameter(dbCommand, "ValidDeclareTypeID", DbType.Int32, model.ValidDeclareTypeID);
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

        #region 根据品种有效申报标识更新有效申报类型

        /// <summary>
        /// 根据品种有效申报标识更新有效申报类型
        /// </summary>
        /// <param name="model">有效申报类型实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_ValidDeclareType model)
        {
            return Update(model, null, null);
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_ValidDeclareType GetModel(int BreedClassValidID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BreedClassValidID,ValidDeclareTypeID from XH_ValidDeclareType ");
            strSql.Append(" where BreedClassValidID=@BreedClassValidID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, BreedClassValidID);
            ManagementCenter.Model.XH_ValidDeclareType model = null;
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
            strSql.Append("select BreedClassValidID,ValidDeclareTypeID ");
            strSql.Append(" FROM XH_ValidDeclareType ");
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
        public List<ManagementCenter.Model.XH_ValidDeclareType> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BreedClassValidID,ValidDeclareTypeID ");
            strSql.Append(" FROM XH_ValidDeclareType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.XH_ValidDeclareType> list =
                new List<ManagementCenter.Model.XH_ValidDeclareType>();
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
        public ManagementCenter.Model.XH_ValidDeclareType ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.XH_ValidDeclareType model = new ManagementCenter.Model.XH_ValidDeclareType();
            object ojb;
            ojb = dataReader["BreedClassValidID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassValidID = (int) ojb;
            }

            ojb = dataReader["ValidDeclareTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ValidDeclareTypeID = (int) ojb;
            }
            return model;
        }

        #endregion  成员方法

        #region 增加一条有效申报类型

        /// <summary>
        /// 增加一条有效申报类型
        /// </summary>
        public int AddValidDeclareType(ManagementCenter.Model.XH_ValidDeclareType model, DbTransaction tran,
                                       Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_ValidDeclareType(");
            strSql.Append("ValidDeclareTypeID)");

            strSql.Append(" values (");
            strSql.Append("@ValidDeclareTypeID)");
            strSql.Append(";select @@IDENTITY");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ValidDeclareTypeID", DbType.Int32, model.ValidDeclareTypeID);
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

            // db.ExecuteNonQuery(dbCommand);
        }

        #endregion

        #region 增加一条有效申报类型(重载,没有事务)

        /// <summary>
        /// 增加一条有效申报类型(重载,没有事务)
        /// </summary>
        public int AddValidDeclareType(ManagementCenter.Model.XH_ValidDeclareType model)
        {
            return AddValidDeclareType(model, null, null);
        }

        #endregion

        #region 根据品种有效申报标识删除有效申报类型

        /// <summary>
        /// 根据品种有效申报标识删除有效申报类型
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <param name="tran">事务</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool DeleteValidDeclareType(int BreedClassValidID, DbTransaction tran,
                                           Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_ValidDeclareType ");
            strSql.Append(" where BreedClassValidID=@BreedClassValidID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassValidID", DbType.Int32, BreedClassValidID);
            // db.ExecuteNonQuery(dbCommand);
            //int result;
            //object obj;
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
            //if (!int.TryParse(obj.ToString(), out result))
            //{
            //    return 0;
            //}
            return true;
        }

        #endregion

        #region 根据品种有效申报标识删除有效申报类型(无事务)

        /// <summary>
        /// 根据品种有效申报标识删除有效申报类型
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public bool DeleteValidDeclareType(int BreedClassValidID)
        {
            return DeleteValidDeclareType(BreedClassValidID, null, null);
        }

        #endregion
    }
}