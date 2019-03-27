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
    ///描述：品种_股指期货_保证金 数据访问类QH_SIFBailDAL。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_SIFBailDAL
    {
        public QH_SIFBailDAL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from QH_SIFBail";
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
            strSql.Append("select count(1) from QH_SIFBail where BreedClassID=@BreedClassID ");
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
        /// 添加品种_股指期货_保证金
        /// </summary>
        /// <param name="model">品种_股指期货_保证金实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.QH_SIFBail model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_SIFBail(");
            strSql.Append("BailScale,BreedClassID)");

            strSql.Append(" values (");
            strSql.Append("@BailScale,@BreedClassID)");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if (model.BailScale != AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "BailScale", DbType.Decimal, model.BailScale);
            }
            else
            {
                db.AddInParameter(dbCommand, "BailScale", DbType.Decimal, DBNull.Value);
            }
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
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
        ///  添加品种_股指期货_保证金
        /// </summary>
        /// <param name="model"> 品种_股指期货_保证金实体</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.QH_SIFBail model)
        {
            return Add(model, null, null);
        }


        /// <summary>
        /// 更新品种_股指期货_保证金
        /// </summary>
        /// <param name="model">品种_股指期货_保证金实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_SIFBail model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_SIFBail set ");
            strSql.Append("BailScale=@BailScale");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if (model.BailScale != AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "BailScale", DbType.Decimal, model.BailScale);
            }
            else
            {
                db.AddInParameter(dbCommand, "BailScale", DbType.Decimal, DBNull.Value);
            } db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            //	db.ExecuteNonQuery(dbCommand);
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
        /// 更新品种_股指期货_保证金
        /// </summary>
        /// <param name="model">品种_股指期货_保证金实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_SIFBail model)
        {
            return Update(model, null, null);
        }

        /// <summary>
        /// 删除品种_股指期货_保证金
        /// </summary>
        /// <param name="BreedClassID">品种标识(品种_股指期货_保证金)</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_SIFBail ");
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
        /// 删除品种_股指期货_保证金
        /// </summary>
        /// <param name="BreedClassID">品种标识(品种_股指期货_保证金)</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID)
        {
            return Delete(BreedClassID, null, null);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_SIFBail GetModel(int BreedClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BailScale,BreedClassID from QH_SIFBail ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            ManagementCenter.Model.QH_SIFBail model = null;
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
            strSql.Append("select BailScale,BreedClassID ");
            strSql.Append(" FROM QH_SIFBail ");
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
        public List<ManagementCenter.Model.QH_SIFBail> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BailScale,BreedClassID ");
            strSql.Append(" FROM QH_SIFBail ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.QH_SIFBail> list = new List<ManagementCenter.Model.QH_SIFBail>();
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
        public ManagementCenter.Model.QH_SIFBail ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.QH_SIFBail model = new ManagementCenter.Model.QH_SIFBail();
            object ojb;
            ojb = dataReader["BailScale"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BailScale = (decimal) ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int) ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}