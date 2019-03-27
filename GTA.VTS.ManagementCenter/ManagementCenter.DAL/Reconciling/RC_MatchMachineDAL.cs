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
    /// 描述：撮合机表 数据访问类RC_MatchMachineDAL。
    /// 作者：熊晓凌 修改：刘书伟
    /// 日期：2008-11-18 2009-10-28
    /// </summary>
    public class RC_MatchMachineDAL
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RC_MatchMachineDAL()
        {
        }
        #endregion

        #region SQL

        /// <summary>
        ///  根据撮合中心_撮合机表中的交易所类型ID获取交易所类型名称
        /// </summary>
        private string SQL_SELECTBOURSETYPENAME_RCMATCHMACHINE =
            @"SELECT A.BOURSETYPEID,A.BOURSETYPENAME 
                                                                FROM CM_BOURSETYPE A,RC_MatchMachine B 
                                                                WHERE A.BOURSETYPEID=B.BOURSETYPEID ";

        /// <summary>
        ///  根据撮合中心_撮合机表中的撮合中心ID获取撮合中心名称
        /// </summary>
        private string SQL_SELECTMATCHCENTERNAME_RCMATCHMACHINE =
            @"SELECT A.MATCHCENTERID,A.MATCHCENTERNAME 
                                                                    FROM RC_MATCHCENTER A,RC_MATCHMACHINE B 
                                                                    WHERE A.MATCHCENTERID=B.MATCHCENTERID ";

        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(MatchMachineID)+1 from RC_MatchMachine";
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
        public bool Exists(int MatchMachineID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from RC_MatchMachine where MatchMachineID=@MatchMachineID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchMachineID", DbType.Int32, MatchMachineID);
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
        public int Add(ManagementCenter.Model.RC_MatchMachine model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into RC_MatchMachine(");
            strSql.Append("MatchMachineName,BourseTypeID,MatchCenterID)");

            strSql.Append(" values (");
            strSql.Append("@MatchMachineName,@BourseTypeID,@MatchCenterID)");
            strSql.Append(";select @@IDENTITY");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchMachineName", DbType.String, model.MatchMachineName);
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            db.AddInParameter(dbCommand, "MatchCenterID", DbType.Int32, model.MatchCenterID);
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
        /// 添加撮合机
        /// </summary>
        /// <param name="model">撮合机实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.RC_MatchMachine model)
        {
            return Add(model, null, null);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.RC_MatchMachine model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RC_MatchMachine set ");
            strSql.Append("MatchMachineName=@MatchMachineName,");
            strSql.Append("BourseTypeID=@BourseTypeID,");
            strSql.Append("MatchCenterID=@MatchCenterID");
            strSql.Append(" where MatchMachineID=@MatchMachineID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchMachineID", DbType.Int32, model.MatchMachineID);
            db.AddInParameter(dbCommand, "MatchMachineName", DbType.String, model.MatchMachineName);
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            db.AddInParameter(dbCommand, "MatchCenterID", DbType.Int32, model.MatchCenterID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int MatchMachineID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete RC_MatchMachine ");
            strSql.Append(" where MatchMachineID=@MatchMachineID ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchMachineID", DbType.Int32, MatchMachineID);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public void DeleteAll(DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete RC_MatchMachine ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }

        #region 根据交易所类型ID，删除撮合机
        /// <summary>
        /// 根据交易所类型ID，删除撮合机
        /// </summary>
        /// <param name="BourseTypeID">交易所类型ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool DeleteByBourseTypeID(int BourseTypeID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete RC_MatchMachine ");
            strSql.Append(" where BourseTypeID=@BourseTypeID ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
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

        #region 根据交易所类型ID，删除撮合机
        /// <summary>
        /// 根据交易所类型ID，删除撮合机
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
        public ManagementCenter.Model.RC_MatchMachine GetModel(int MatchMachineID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MatchMachineID,MatchMachineName,BourseTypeID,MatchCenterID from RC_MatchMachine ");
            strSql.Append(" where MatchMachineID=@MatchMachineID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchMachineID", DbType.Int32, MatchMachineID);
            ManagementCenter.Model.RC_MatchMachine model = null;
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
            strSql.Append("select MatchMachineID,MatchMachineName,BourseTypeID,MatchCenterID ");
            strSql.Append(" FROM RC_MatchMachine ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        #region 获得撮合机数据列表（比DataSet效率高，推荐使用）
        /// <summary>
        /// 获得撮合机数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.RC_MatchMachine> GetListArray(string strWhere, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MatchMachineID,MatchMachineName,BourseTypeID,MatchCenterID ");
            strSql.Append(" FROM RC_MatchMachine ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.RC_MatchMachine> list = new List<ManagementCenter.Model.RC_MatchMachine>();
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

        #region 获得撮合机数据列表（比DataSet效率高，推荐使用）
        /// <summary>
        /// 获得撮合机数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.RC_MatchMachine> GetListArray(string strWhere)
        {
            return GetListArray(strWhere, null, null);
        }
        #endregion

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.RC_MatchMachine ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.RC_MatchMachine model = new ManagementCenter.Model.RC_MatchMachine();
            object ojb;
            ojb = dataReader["MatchMachineID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MatchMachineID = (int)ojb;
            }
            model.MatchMachineName = dataReader["MatchMachineName"].ToString();
            ojb = dataReader["BourseTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BourseTypeID = (int)ojb;
            }
            ojb = dataReader["MatchCenterID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MatchCenterID = (int)ojb;
            }
            return model;
        }

        #region 撮合机分页查询
        /// <summary>
        /// 撮合机分页查询
        /// </summary>
        /// <param name="machineQueryEntity">撮合机实休</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>

        public DataSet GetPagingMachine(ManagementCenter.Model.RC_MatchMachine machineQueryEntity, int pageNo,
                                        int pageSize,
                                        out int rowCount)
        {
            string SQL_SELECT_Machine =
                @" select MatchMachineID,MatchMachineName,BourseTypeID,MatchCenterID FROM RC_MatchMachine where 1=1 ";

            if (machineQueryEntity.BourseTypeID != int.MaxValue)
            {
                SQL_SELECT_Machine += "AND BourseTypeID=@BourseTypeID ";
            }
            if (machineQueryEntity.MatchCenterID != int.MaxValue)
            {
                SQL_SELECT_Machine += "AND MatchCenterID=@MatchCenterID ";
            }
            if (machineQueryEntity.MatchMachineID != int.MaxValue)
            {
                SQL_SELECT_Machine += "AND MatchMachineID=@MatchMachineID ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_Machine);

            if (machineQueryEntity.BourseTypeID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, machineQueryEntity.BourseTypeID);
            }
            if (machineQueryEntity.MatchCenterID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "MatchCenterID", DbType.Int32, machineQueryEntity.MatchCenterID);
            }
            if (machineQueryEntity.MatchMachineID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "MatchMachineID", DbType.Int32, machineQueryEntity.MatchMachineID);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_Machine, pageNo, pageSize, out rowCount,
                                        "UM_Machine");
        }

        #endregion

        #region  根据撮合中心_撮合机表中的交易所类型ID获取交易所类型名称

        /// <summary>
        /// 根据撮合中心_撮合机表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetRCMatchMachineBourseTypeName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBOURSETYPENAME_RCMATCHMACHINE);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #region  根据撮合中心_撮合机表中的撮合中心ID获取撮合中心名称

        /// <summary>
        /// 根据撮合中心_撮合机表中的撮合中心ID获取撮合中心名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetRCMatchMachineMatchCenterName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTMATCHCENTERNAME_RCMATCHMACHINE);
            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #endregion  成员方法
    }
}