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
    ///描述：股指期货持仓限制 数据访问类QH_SIFPositionDAL。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_SIFPositionDAL
    {
        public QH_SIFPositionDAL()
        {
        }
        #region SQL
        /// <summary>
        /// 获取股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        private string SQL_SELECT_QHSIFPOSITIONANDQHSIFBAIL = @"SELECT A.BREEDCLASSID,A.BREEDCLASSNAME,B.UnilateralPositions,C.BailScale 
                                                                FROM CM_BREEDCLASS AS A,
                                                                QH_SIFPOSITION AS B,QH_SIFBAIL AS C 
                                                                WHERE A.BREEDCLASSTYPEID=3 
                                                                AND A.BREEDCLASSID=B.BREEDCLASSID
                                                                AND A.BREEDCLASSID=C.BREEDCLASSID ";
        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from QH_SIFPosition";
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
            strSql.Append("select count(1) from QH_SIFPosition where BreedClassID=@BreedClassID ");
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
        /// 添加股指期货持仓限制
        /// </summary>
        /// <param name="model">股指期货持仓限制实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.QH_SIFPosition model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_SIFPosition(");
            strSql.Append("UnilateralPositions,BreedClassID)");

            strSql.Append(" values (");
            strSql.Append("@UnilateralPositions,@BreedClassID)");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if (model.UnilateralPositions != AppGlobalVariable.INIT_INT)
            {
                db.AddInParameter(dbCommand, "UnilateralPositions", DbType.Int32, model.UnilateralPositions);
            }
            else
            {
                db.AddInParameter(dbCommand, "UnilateralPositions", DbType.Int32,DBNull.Value);
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
        /// 添加股指期货持仓限制 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.QH_SIFPosition model)
        {
            return Add(model, null, null);
        }

        /// <summary>
        /// 更新股指期货持仓限制 
        /// </summary>
        /// <param name="model">股指期货持仓限制 实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_SIFPosition model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_SIFPosition set ");
            strSql.Append("UnilateralPositions=@UnilateralPositions");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if (model.UnilateralPositions != AppGlobalVariable.INIT_INT)
            {
                db.AddInParameter(dbCommand, "UnilateralPositions", DbType.Int32, model.UnilateralPositions);
            }
            else
            {
                db.AddInParameter(dbCommand, "UnilateralPositions", DbType.Int32, DBNull.Value);
            } db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
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
        /// 更新股指期货持仓限制 
        /// </summary>
        /// <param name="model">股指期货持仓限制 实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_SIFPosition model)
        {
            return Update(model, null, null);
        }

        /// <summary>
        /// 删除股指期货持仓限制 
        /// </summary>
        /// <param name="BreedClassID">品种标识(股指期货持仓限制)</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_SIFPosition ");
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
        ///  删除股指期货持仓限制 
        /// </summary>
        /// <param name="BreedClassID">品种标识(股指期货持仓限制)</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID)
        {
            return Delete(BreedClassID, null, null);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_SIFPosition GetModel(int BreedClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UnilateralPositions,BreedClassID from QH_SIFPosition ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            ManagementCenter.Model.QH_SIFPosition model = null;
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
            strSql.Append("select UnilateralPositions,BreedClassID ");
            strSql.Append(" FROM QH_SIFPosition ");
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
        public List<ManagementCenter.Model.QH_SIFPosition> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UnilateralPositions,BreedClassID ");
            strSql.Append(" FROM QH_SIFPosition ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.QH_SIFPosition> list = new List<ManagementCenter.Model.QH_SIFPosition>();
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

        #region  获取所有股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        ///  获取所有股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllQHSIFPositionAndQHSIFBail(string BreedClassName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            //条件查询
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                SQL_SELECT_QHSIFPOSITIONANDQHSIFBAIL += "AND (BreedClassName LIKE  '%' + @BreedClassName + '%') ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_QHSIFPOSITIONANDQHSIFBAIL);
            if (BreedClassName != AppGlobalVariable.INIT_STRING && BreedClassName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BreedClassName", DbType.String, BreedClassName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_QHSIFPOSITIONANDQHSIFBAIL, pageNo, pageSize,
                                        out rowCount, "QHSIFPositionAndQHSIFBail");
        }

        #endregion

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.QH_SIFPosition ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.QH_SIFPosition model = new ManagementCenter.Model.QH_SIFPosition();
            object ojb;
            ojb = dataReader["UnilateralPositions"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UnilateralPositions = (int) ojb;
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