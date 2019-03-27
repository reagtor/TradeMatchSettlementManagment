using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

//请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述：交易商品品种 数据访问类CM_BreedClassDAL。
    ///作者：刘书伟
    ///日期:2008-11-20
    /// </summary>
    public class CM_BreedClassDAL
    {
        #region SQL

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        private string SQL_SELECT_CMBREEDCLASS =
            @"SELECT BREEDCLASSID,BREEDCLASSNAME 
                                FROM CM_BREEDCLASS 
                                WHERE BREEDCLASSTYPEID=1 AND DELETESTATE IS NOT NULL AND DELETESTATE<>1 ";


        /// <summary>
        /// 根据查询条件获取交易商品品种 
        /// </summary>
        private string SQL_SELECTALL_CMBREEDCLASS = @"SELECT * FROM CM_BREEDCLASS WHERE DELETESTATE IS NOT NULL AND DELETESTATE<>1 ";//1=1

        /// <summary>
        ///  根据交易商品品种表中的交易所类型ID获取交易所类型名称
        /// </summary>
        private string SQL_SELECTBOURSETYPENAME_CMBREEDCLASS =
            @"SELECT A.BOURSETYPEID,A.BOURSETYPENAME 
                                                            FROM CM_BOURSETYPE A,CM_BREEDCLASS B 
                                                            WHERE A.BOURSETYPEID=B.BOURSETYPEID ";

        /// <summary>
        /// 获取(除港股品种外的)交易商品品种表中的品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAME_CM_BREEDCLASS = @"SELECT BREEDCLASSID,BREEDCLASSNAME FROM CM_BREEDCLASS WHERE BREEDCLASSTYPEID<>4 ";

        /// <summary>
        /// 获取品种类型是商品期货或股指期货的品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAMEQHFUTURECOSTS_CMBREEDCLASS =
            @"SELECT BREEDCLASSID,BREEDCLASSNAME 
                                                    FROM CM_BREEDCLASS 
                                                    WHERE BREEDCLASSTYPEID<>1 AND BREEDCLASSTYPEID<>4 ";

        /// <summary>
        /// 获取品种类型是商品期货的品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAMESPQH_CMBREEDCLASS =
            @"SELECT BREEDCLASSID,BREEDCLASSNAME 
                                                    FROM CM_BREEDCLASS 
                                                    WHERE BREEDCLASSTYPEID=2 ";

        /// <summary>
        ///获取品种类型是股指期货的品种名称
        /// </summary>
        private string SQL_SELECTQHSIF_CMBREEDCLASS =
            @"SELECT BREEDCLASSID,BREEDCLASSNAME 
                                                        FROM CM_BREEDCLASS 
                                                        WHERE BREEDCLASSTYPEID=3 ";

        /// <summary>
        /// 获取现货普通和港股品种名称
        /// </summary>
        private string SQL_SELECT_XHANDHKCMBREEDCLASS =
            @"SELECT BREEDCLASSID,BREEDCLASSNAME 
                                FROM CM_BREEDCLASS 
                                WHERE BREEDCLASSTYPEID=1 OR BREEDCLASSTYPEID=4 AND DELETESTATE IS NOT NULL AND DELETESTATE<>1 ";


        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BreedClassID)+1 from CM_BreedClass";
            Database db = DatabaseFactory.CreateDatabase();
            object obj = db.ExecuteScalar(CommandType.Text, strsql);
            if (obj != null && obj != DBNull.Value)
            {
                return int.Parse(obj.ToString());
            }
            return AppGlobalVariable.INIT_INT;//1;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            var strSql = new StringBuilder();
            strSql.Append("select count(1) from CM_BreedClass where BreedClassID=@BreedClassID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            int cmdresult;
            object obj = db.ExecuteScalar(dbCommand);
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
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
        public bool Add(CM_BreedClass model)
        {
            var strSql = new StringBuilder();
            strSql.Append("SET IDENTITY_INSERT [CM_BreedClass] ON; ");//关闭自动增长列(因为系统默认前1500ID属于系统ID)
            strSql.Append("insert into CM_BreedClass(");
            //strSql.Append("BreedClassName,AccountTypeIDFund,BreedClassTypeID,AccountTypeIDHold,BourseTypeID,ISSysDefaultBreed,ISHKBreedClassType,DeleteState)");
            strSql.Append("BreedClassID,BreedClassName,AccountTypeIDFund,BreedClassTypeID,AccountTypeIDHold,BourseTypeID,ISSysDefaultBreed,ISHKBreedClassType,DeleteState)");
            strSql.Append(" values (");
            strSql.Append("@BreedClassID,@BreedClassName,@AccountTypeIDFund,@BreedClassTypeID,@AccountTypeIDHold,@BourseTypeID,@ISSysDefaultBreed,@ISHKBreedClassType,@DeleteState)");
            //strSql.Append(";select @@IDENTITY");
            strSql.Append(";SET IDENTITY_INSERT [CM_BreedClass] OFF");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "BreedClassName", DbType.String, model.BreedClassName);
            db.AddInParameter(dbCommand, "AccountTypeIDFund", DbType.Int32, model.AccountTypeIDFund);
            db.AddInParameter(dbCommand, "BreedClassTypeID", DbType.Int32, model.BreedClassTypeID);
            db.AddInParameter(dbCommand, "AccountTypeIDHold", DbType.Int32, model.AccountTypeIDHold);
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            db.AddInParameter(dbCommand, "ISSysDefaultBreed", DbType.Int32, model.ISSysDefaultBreed);
            db.AddInParameter(dbCommand, "ISHKBreedClassType", DbType.Int32, model.ISHKBreedClassType);
            model.DeleteState = (int)Types.IsYesOrNo.No;//添加时默认状态为非删除
            db.AddInParameter(dbCommand, "DeleteState", DbType.Int32, model.DeleteState);
            //int result;
            //object obj = db.ExecuteScalar(dbCommand);
            //if (!int.TryParse(obj.ToString(), out result))
            //{
            //    return AppGlobalVariable.INIT_INT;
            //}
            //return result;
            db.ExecuteScalar(dbCommand);
            return true;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(CM_BreedClass model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update CM_BreedClass set ");
            strSql.Append("BreedClassName=@BreedClassName,");
            strSql.Append("AccountTypeIDFund=@AccountTypeIDFund,");
            strSql.Append("BreedClassTypeID=@BreedClassTypeID,");
            strSql.Append("AccountTypeIDHold=@AccountTypeIDHold,");
            strSql.Append("BourseTypeID=@BourseTypeID");
            // strSql.Append("UnitID=@UnitID");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.AddInParameter(dbCommand, "BreedClassName", DbType.String, model.BreedClassName);
            db.AddInParameter(dbCommand, "AccountTypeIDFund", DbType.Int32, model.AccountTypeIDFund);
            db.AddInParameter(dbCommand, "BreedClassTypeID", DbType.Int32, model.BreedClassTypeID);
            db.AddInParameter(dbCommand, "AccountTypeIDHold", DbType.Int32, model.AccountTypeIDHold);
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            // db.AddInParameter(dbCommand,"UnitID",DbType.Int32,model.UnitID);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #region  根据交易所类型ID，更新交易商品品种中的交易所类型ID
        /// <summary>
        /// 根据交易所类型ID，更新交易商品品种中的交易所类型ID
        /// </summary>
        /// <param name="OldBourseTypeID">修改前的交易所类型ID</param>
        /// <param name="NewBourseTypeID">修改后的交易所类型ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool UpdateBourseTypeID(int OldBourseTypeID, int NewBourseTypeID, DbTransaction tran, Database db)
        {
            var strSql = new StringBuilder();
            strSql.Append("update CM_BreedClass set ");
            strSql.Append("BourseTypeID=@NewBourseTypeID");
            strSql.Append(" where BourseTypeID=@OldBourseTypeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OldBourseTypeID", DbType.Int32, OldBourseTypeID);
            db.AddInParameter(dbCommand, "NewBourseTypeID", DbType.Int32, NewBourseTypeID);
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

        #region  根据交易所类型ID，更新交易商品品种中的交易所类型ID
        /// <summary>
        /// 根据交易所类型ID，更新交易商品品种中的交易所类型ID
        /// </summary>
        /// <param name="OldBourseTypeID">修改前的交易所类型ID</param>
        /// <param name="NewBourseTypeID">修改后的交易所类型ID</param>
        /// <returns></returns>
        public bool UpdateBourseTypeID(int OldBourseTypeID, int NewBourseTypeID)
        {
            return UpdateBourseTypeID(OldBourseTypeID, NewBourseTypeID, null, null);
        }
        #endregion

        /// <summary>
        /// 根据品种ID，删除品种
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int BreedClassID, DbTransaction tran, Database db)
        {
            var strSql = new StringBuilder();
            strSql.Append("delete CM_BreedClass ");
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
        ///  根据品种ID，删除品种
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
        public CM_BreedClass GetModel(int BreedClassID)
        {
            var strSql = new StringBuilder();
            strSql.Append(
                "select BreedClassID,BreedClassName,AccountTypeIDFund,BreedClassTypeID,AccountTypeIDHold,BourseTypeID,ISSysDefaultBreed,ISHKBreedClassType,DeleteState from CM_BreedClass ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            CM_BreedClass model = null;
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
            var strSql = new StringBuilder();
            strSql.Append(
                "select BreedClassID,BreedClassName,AccountTypeIDFund,BreedClassTypeID,AccountTypeIDHold,BourseTypeID,ISSysDefaultBreed,ISHKBreedClassType,DeleteState");
            strSql.Append(" FROM CM_BreedClass ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        #region  获得交易商品品种数据列表（比DataSet效率高，推荐使用）
        /// <summary>
        ///  获得交易商品品种数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public List<CM_BreedClass> GetListArray(string strWhere, DbTransaction tran, Database db)
        {
            var strSql = new StringBuilder();
            strSql.Append(
                "select BreedClassID,BreedClassName,AccountTypeIDFund,BreedClassTypeID,AccountTypeIDHold,BourseTypeID,ISSysDefaultBreed,ISHKBreedClassType,DeleteState");
            strSql.Append(" FROM CM_BreedClass ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            var list = new List<CM_BreedClass>();
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            if (tran == null)
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
                using (IDataReader dataReader = db.ExecuteReader(tran, CommandType.Text, strSql.ToString()))
                {
                    while (dataReader.Read())
                    {
                        list.Add(ReaderBind(dataReader));
                    }
                }
            }
            return list;
        }
        #endregion

        #region  获得交易商品品种数据列表（比DataSet效率高，推荐使用）
        /// <summary>
        ///  获得交易商品品种数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<CM_BreedClass> GetListArray(string strWhere)
        {
            return GetListArray(strWhere, null, null);
        }
        #endregion

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public CM_BreedClass ReaderBind(IDataReader dataReader)
        {
            var model = new CM_BreedClass();
            object ojb;
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            model.BreedClassName = dataReader["BreedClassName"].ToString();
            ojb = dataReader["AccountTypeIDFund"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountTypeIDFund = (int)ojb;
            }
            ojb = dataReader["BreedClassTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassTypeID = (int)ojb;
            }
            ojb = dataReader["AccountTypeIDHold"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountTypeIDHold = (int)ojb;
            }
            ojb = dataReader["BourseTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BourseTypeID = (int)ojb;
            }
            ojb = dataReader["ISSysDefaultBreed"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ISSysDefaultBreed = (int)ojb;
            }
            ojb = dataReader["ISHKBreedClassType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ISHKBreedClassType = (int)ojb;
            }
            ojb = dataReader["DeleteState"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DeleteState = (int)ojb;
            }

            return model;
        }

        #endregion  成员方法

        #region 获取现货品种名称

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetBreedClassName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_CMBREEDCLASS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #region 获取现货普通和港股品种名称

        /// <summary>
        /// 获取现货普通和港股品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetXHAndHKBreedClassName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_XHANDHKCMBREEDCLASS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #region 获取所有交易商品品种

        /// <summary>
        /// 获取所有交易商品品种
        /// </summary>
        /// <param name="BreedClassTypeID">品种类型ID</param>
        ///  <param name="BourseTypeID">交易所类型ID</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMBreedClass(int BreedClassTypeID, int BourseTypeID, int pageNo, int pageSize,
                                          out int rowCount)
        {
            //条件查询
            if (BreedClassTypeID != AppGlobalVariable.INIT_INT)
            {
                SQL_SELECTALL_CMBREEDCLASS += "AND (BreedClassTypeID=@BreedClassTypeID) ";
            }
            if (BourseTypeID != AppGlobalVariable.INIT_INT)
            {
                SQL_SELECTALL_CMBREEDCLASS += "AND (BourseTypeID=@BourseTypeID) ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTALL_CMBREEDCLASS);
            if (BreedClassTypeID != AppGlobalVariable.INIT_INT)
            {
                database.AddInParameter(dbCommand, "BreedClassTypeID", DbType.Int32, BreedClassTypeID);
            }
            if (BourseTypeID != AppGlobalVariable.INIT_INT)
            {
                database.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, BourseTypeID);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECTALL_CMBREEDCLASS, pageNo, pageSize,
                                        out rowCount, "CM_BreedClass");
        }

        #endregion

        #region  根据交易商品品种表中的交易所类型ID获取交易所类型名称

        /// <summary>
        /// 根据交易商品品种表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetCMBreedClassBourseTypeName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBOURSETYPENAME_CMBREEDCLASS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #region 获取所有品种名称

        /// <summary>
        /// 获取所有品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllBreedClassName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAME_CM_BREEDCLASS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #region 获取品种类型是商品期货的品种名称

        /// <summary>
        /// 获取品种类型是商品期货的品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetSpQhTypeBreedClassName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAMESPQH_CMBREEDCLASS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #region 获取品种类型是商品期货或股指期货的品种名称

        /// <summary>
        /// 获取品种类型是商品期货或股指期货的品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHFutureCostsBreedClassName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAMEQHFUTURECOSTS_CMBREEDCLASS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #region 获取品种类型是股指期货的品种名称

        /// <summary>
        /// 获取品种类型是股指期货的品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHSIFPositionAndBailBreedClassName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTQHSIF_CMBREEDCLASS);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

    }
}