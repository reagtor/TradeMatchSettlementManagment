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
    ///描述：交易所类型_非交易日期 数据访问类CM_NotTradeDate。
    ///作者：刘书伟
    ///日期: 2008-11-26
    ///修改：叶振东
    ///时间：2010-04-07
    ///描述：添加根据交易所类型和非交易日时间来查是否存在记录方法
    /// </summary>
    public class CM_NotTradeDateDAL
    {
        public CM_NotTradeDateDAL()
        { }
        #region SQL
        /// <summary>
        /// 根据查询条件返回交易所类型_非交易日期数据
        /// </summary>
        private string SQL_SELECT_CMNOTTRADEDATE = @"SELECT B.BOURSETYPENAME,A.* FROM CM_NOTTRADEDATE AS A,CM_BOURSETYPE AS B 
                                                    WHERE A.BOURSETYPEID=B.BOURSETYPEID ";

        /// <summary>
        ///  根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称
        /// </summary>
        private string SQL_SELECTBOURSETYPENAME_CMNOTTRADEDATE = @"SELECT DISTINCT A.BOURSETYPEID,A.BOURSETYPENAME 
                                                                FROM CM_BOURSETYPE A,CM_NOTTRADEDATE B 
                                                                WHERE A.BOURSETYPEID=B.BOURSETYPEID ";
        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(NotTradeDateID)+1 from CM_NotTradeDate";
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
        public bool Exists(int NotTradeDateID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CM_NotTradeDate where NotTradeDateID=@NotTradeDateID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "NotTradeDateID", DbType.Int32, NotTradeDateID);
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
        public int Add(ManagementCenter.Model.CM_NotTradeDate model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CM_NotTradeDate(");
            strSql.Append("NotTradeDay,BourseTypeID)");

            strSql.Append(" values (");
            strSql.Append("@NotTradeDay,@BourseTypeID)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            //db.AddInParameter(dbCommand, "NotTradeDateID", DbType.Int32, model.NotTradeDateID);
            db.AddInParameter(dbCommand, "NotTradeDay", DbType.DateTime, model.NotTradeDay);
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            //db.ExecuteNonQuery(dbCommand);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return AppGlobalVariable.INIT_INT;
            }
            return result;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ManagementCenter.Model.CM_NotTradeDate model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CM_NotTradeDate set ");
            //strSql.Append("NotTradeDateMonth=@NotTradeDateMonth,");
            strSql.Append("NotTradeDay=@NotTradeDay,");
            strSql.Append("BourseTypeID=@BourseTypeID");
            strSql.Append(" where NotTradeDateID=@NotTradeDateID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "NotTradeDateID", DbType.Int32, model.NotTradeDateID);
            //db.AddInParameter(dbCommand, "NotTradeDateMonth", DbType.Int32, model.NotTradeDateMonth);
            db.AddInParameter(dbCommand, "NotTradeDay", DbType.DateTime, model.NotTradeDay);
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #region 根据非交易日ID，删除非交易日
        /// <summary>
        /// 根据非交易日ID，删除非交易日
        /// </summary>
        /// <param name="NotTradeDateID">非交易日ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool Delete(int NotTradeDateID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete CM_NotTradeDate ");
            strSql.Append(" where NotTradeDateID=@NotTradeDateID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "NotTradeDateID", DbType.Int32, NotTradeDateID);
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

        #region 根据非交易日ID，删除非交易日
        /// <summary>
        /// 根据非交易日ID，删除非交易日
        /// </summary>
        /// <param name="NotTradeDateID">非交易日ID</param>
        /// <returns></returns>
        public bool Delete(int NotTradeDateID)
        {
            return Delete(NotTradeDateID, null, null);
        }
        #endregion

        #region 根据交易所类型ID，删除非交易日
        /// <summary>
        /// 根据交易所类型ID，删除非交易日
        /// </summary>
        /// <param name="BourseTypeID">交易所类型ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool DeleteByBourseTypeID(int BourseTypeID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete CM_NotTradeDate ");
            strSql.Append(" where BourseTypeID=@BourseTypeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, BourseTypeID);
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

        #region 根据交易所类型ID，删除非交易日
        /// <summary>
        /// 根据交易所类型ID，删除非交易日
        /// </summary>
        /// <param name="BourseTypeID">交易所类型ID</param>
        /// <returns></returns>
        public bool DeleteByBourseTypeID(int BourseTypeID)
        {
            return DeleteByBourseTypeID(BourseTypeID, null, null);
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_NotTradeDate GetModel(int NotTradeDateID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NotTradeDateID,NotTradeDay,BourseTypeID from CM_NotTradeDate ");
            strSql.Append(" where NotTradeDateID=@NotTradeDateID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "NotTradeDateID", DbType.Int32, NotTradeDateID);
            ManagementCenter.Model.CM_NotTradeDate model = null;
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
        ///  根据交易所类型和非交易日时间来查是否存在记录
        /// </summary>
        /// <param name="BourseTypeID">交易所类型</param>
        /// <param name="NotTradeDay">非交易日时间</param>
        /// <returns></returns>
        public ManagementCenter.Model.CM_NotTradeDate GetNotTradeDate(int BourseTypeID, DateTime NotTradeDay)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NotTradeDateID,NotTradeDay,BourseTypeID from CM_NotTradeDate ");
            strSql.Append(" where BourseTypeID=@BourseTypeID and NotTradeDay=@NotTradeDay");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, BourseTypeID);
            db.AddInParameter(dbCommand, "NotTradeDay", DbType.DateTime, NotTradeDay);
            ManagementCenter.Model.CM_NotTradeDate model = null;
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
            strSql.Append("select NotTradeDateID,NotTradeDay,BourseTypeID ");
            strSql.Append(" FROM CM_NotTradeDate ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        #region 获得交易所类型_非交易日期数据列表（比DataSet效率高，推荐使用）
        /// <summary>
        /// 获得交易所类型_非交易日期数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_NotTradeDate> GetListArray(string strWhere, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NotTradeDateID,NotTradeDay,BourseTypeID ");
            strSql.Append(" FROM CM_NotTradeDate ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.CM_NotTradeDate> list = new List<ManagementCenter.Model.CM_NotTradeDate>();
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

        #region 获得交易所类型_非交易日期数据列表（比DataSet效率高，推荐使用）
        /// <summary>
        /// 获得交易所类型_非交易日期数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_NotTradeDate> GetListArray(string strWhere)
        {
            return GetListArray(strWhere, null, null);
        }
        #endregion

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<ManagementCenter.Model.CM_NotTradeDate> GetListArrayByBreedClassID(int BreedClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select A.NotTradeDateID,A.NotTradeDay,A.BourseTypeID ");
            strSql.Append(" FROM CM_NotTradeDate as A,CM_BreedClass as B ");
            strSql.Append(" WHERE A.BourseTypeID=B.BourseTypeID ");
            strSql.Append(" AND B.BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            List<ManagementCenter.Model.CM_NotTradeDate> list = new List<ManagementCenter.Model.CM_NotTradeDate>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

        #region 获取所有交易所类型_非交易日期
        /// <summary>
        /// 获取所有交易所类型_非交易日期
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMNotTradeDate(string BourseTypeName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            //条件查询

            if (BourseTypeName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BourseTypeName))
            {
                SQL_SELECT_CMNOTTRADEDATE += "AND (BourseTypeName LIKE  '%' + @BourseTypeName + '%') ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_CMNOTTRADEDATE);

            if (BourseTypeName != AppGlobalVariable.INIT_STRING && BourseTypeName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BourseTypeName", DbType.String, BourseTypeName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_CMNOTTRADEDATE, pageNo, pageSize,
                                        out rowCount, "CM_NotTradeDate");
        }

        #endregion

        #region 根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称
        /// <summary>
        ///根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetCMNotTradeDateBourseTypeName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBOURSETYPENAME_CMNOTTRADEDATE);
            return database.ExecuteDataSet(dbCommand);
        }
        #endregion


        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.CM_NotTradeDate ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.CM_NotTradeDate model = new ManagementCenter.Model.CM_NotTradeDate();
            object ojb;
            ojb = dataReader["NotTradeDateID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.NotTradeDateID = (int)ojb;
            }
            ojb = dataReader["NotTradeDay"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.NotTradeDay = (DateTime)ojb;
            }
            ojb = dataReader["BourseTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BourseTypeID = (int)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}

