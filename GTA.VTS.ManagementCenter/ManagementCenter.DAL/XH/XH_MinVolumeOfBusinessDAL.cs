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
    ///描述：交易规则_交易方向_交易单位_交易量(最小交易单位)  数据访问类XH_MinVolumeOfBusinessDAL。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class XH_MinVolumeOfBusinessDAL
    {
        public XH_MinVolumeOfBusinessDAL()
        {
        }

        #region SQL

        private string SQL_SELECT_XHMINVOLUMEOFBUSINESS = @"SELECT B.BREEDCLASSNAME,A.* FROM XH_MINVOLUMEOFBUSINESS AS A,CM_BREEDCLASS AS B 
                                                            WHERE A.BREEDCLASSID=B.BREEDCLASSID ";

        /// <summary>
        /// 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// </summary>
        private string SQL_SELECTBREEDCLASSNAME_XHANDHKSPOTTRADERULES = @" SELECT A.BREEDCLASSID,A.BREEDCLASSNAME FROM CM_BREEDCLASS AS A
                                                                            INNER JOIN XH_SPOTTRADERULES    B ON  A.BREEDCLASSID=B.BREEDCLASSID
                                                                            UNION 
                                                                            SELECT A.BREEDCLASSID,A.BREEDCLASSNAME FROM CM_BREEDCLASS AS A
                                                                            INNER JOIN HK_SPOTTRADERULES   C ON  A.BREEDCLASSID=C.BREEDCLASSID ";


        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(MinVolumeOfBusinessID)+1 from XH_MinVolumeOfBusiness";
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
        public bool Exists(int MinVolumeOfBusinessID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from XH_MinVolumeOfBusiness where MinVolumeOfBusinessID=@MinVolumeOfBusinessID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinVolumeOfBusinessID", DbType.Int32, MinVolumeOfBusinessID);
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
        public int Add(ManagementCenter.Model.XH_MinVolumeOfBusiness model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_MinVolumeOfBusiness(");
            strSql.Append("VolumeOfBusiness,UnitID,TradeWayID,BreedClassID)");

            strSql.Append(" values (");
            strSql.Append("@VolumeOfBusiness,@UnitID,@TradeWayID,@BreedClassID)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "VolumeOfBusiness", DbType.Int32, model.VolumeOfBusiness);
            db.AddInParameter(dbCommand, "UnitID", DbType.Int32, model.UnitID);
            db.AddInParameter(dbCommand, "TradeWayID", DbType.Int32, model.TradeWayID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.XH_MinVolumeOfBusiness model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_MinVolumeOfBusiness set ");
            strSql.Append("VolumeOfBusiness=@VolumeOfBusiness,");
            strSql.Append("UnitID=@UnitID,");
            strSql.Append("TradeWayID=@TradeWayID,");
            strSql.Append("BreedClassID=@BreedClassID");
            strSql.Append(" where MinVolumeOfBusinessID=@MinVolumeOfBusinessID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinVolumeOfBusinessID", DbType.Int32, model.MinVolumeOfBusinessID);
            db.AddInParameter(dbCommand, "VolumeOfBusiness", DbType.Int32, model.VolumeOfBusiness);
            db.AddInParameter(dbCommand, "UnitID", DbType.Int32, model.UnitID);
            db.AddInParameter(dbCommand, "TradeWayID", DbType.Int32, model.TradeWayID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int MinVolumeOfBusinessID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_MinVolumeOfBusiness ");
            strSql.Append(" where MinVolumeOfBusinessID=@MinVolumeOfBusinessID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinVolumeOfBusinessID", DbType.Int32, MinVolumeOfBusinessID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_MinVolumeOfBusiness GetModel(int MinVolumeOfBusinessID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select MinVolumeOfBusinessID,VolumeOfBusiness,UnitID,TradeWayID,BreedClassID from XH_MinVolumeOfBusiness ");
            strSql.Append(" where MinVolumeOfBusinessID=@MinVolumeOfBusinessID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinVolumeOfBusinessID", DbType.Int32, MinVolumeOfBusinessID);
            ManagementCenter.Model.XH_MinVolumeOfBusiness model = null;
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
            strSql.Append("select MinVolumeOfBusinessID,VolumeOfBusiness,UnitID,TradeWayID,BreedClassID ");
            strSql.Append(" FROM XH_MinVolumeOfBusiness ");
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
        public List<ManagementCenter.Model.XH_MinVolumeOfBusiness> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MinVolumeOfBusinessID,VolumeOfBusiness,UnitID,TradeWayID,BreedClassID ");
            strSql.Append(" FROM XH_MinVolumeOfBusiness ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.XH_MinVolumeOfBusiness> list =
                new List<ManagementCenter.Model.XH_MinVolumeOfBusiness>();
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
        public ManagementCenter.Model.XH_MinVolumeOfBusiness ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.XH_MinVolumeOfBusiness model = new ManagementCenter.Model.XH_MinVolumeOfBusiness();
            object ojb;
            ojb = dataReader["MinVolumeOfBusinessID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MinVolumeOfBusinessID = (int)ojb;
            }
            ojb = dataReader["VolumeOfBusiness"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.VolumeOfBusiness = (int)ojb;
            }
            ojb = dataReader["UnitID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UnitID = (int)ojb;
            }
            ojb = dataReader["TradeWayID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeWayID = (int)ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            return model;
        }

        #endregion  成员方法

        #region 获取所有交易规则_交易方向_交易单位_交易量(最小交易单位)

        /// <summary>
        /// 获取所有交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllXHMinVolumeOfBusiness(string BreedClassName, int pageNo, int pageSize, out int rowCount)
        {
            //条件查询
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                SQL_SELECT_XHMINVOLUMEOFBUSINESS += "AND (BreedClassName LIKE  '%' + @BreedClassName + '%') ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_XHMINVOLUMEOFBUSINESS);
            if (BreedClassName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BreedClassName))
            {
                database.AddInParameter(dbCommand, "BreedClassName", DbType.String, BreedClassName);
            }
            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_XHMINVOLUMEOFBUSINESS, pageNo, pageSize,
                                        out rowCount, "XH_MinVolumeOfBusiness");
        }

        #endregion

        #region 根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID，删除此数据

        /// <summary>
        /// 根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID，删除此数据
        /// </summary>
        /// <param name="minVolumeOfBusinessID">根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID</param>
        /// <returns></returns>
        public bool DeleteXHMinVolumeOfBusByID(int minVolumeOfBusinessID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_MinVolumeOfBusiness ");
            strSql.Append(" where MinVolumeOfBusinessID=@MinVolumeOfBusinessID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinVolumeOfBusinessID", DbType.Int32, minVolumeOfBusinessID);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        /// <summary>
        /// 根据品种ID删除交易规则_交易方向_交易单位_交易量
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns>返回false或true</returns>
        public bool DeleteXHMinVolumeOfBusByBreedClassID(int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_MinVolumeOfBusiness ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            db.ExecuteNonQuery(dbCommand, tran);
            return true;
        }


        #endregion

        #region 更新交易规则_交易方向_交易单位_交易量(最小交易单位)数据

        /// <summary>
        /// 更新交易规则_交易方向_交易单位_交易量(最小交易单位)数据
        /// </summary>
        /// <param name="model">交易规则_交易方向_交易单位_交易量(最小交易单位)实体</param>
        public bool UpdateXHMinVolumeOfBus(ManagementCenter.Model.XH_MinVolumeOfBusiness model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_MinVolumeOfBusiness set ");
            strSql.Append("VolumeOfBusiness=@VolumeOfBusiness,");
            strSql.Append("UnitID=@UnitID,");
            strSql.Append("TradeWayID=@TradeWayID,");
            strSql.Append("BreedClassID=@BreedClassID");
            strSql.Append(" where MinVolumeOfBusinessID=@MinVolumeOfBusinessID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MinVolumeOfBusinessID", DbType.Int32, model.MinVolumeOfBusinessID);
            db.AddInParameter(dbCommand, "VolumeOfBusiness", DbType.Int32, model.VolumeOfBusiness);
            db.AddInParameter(dbCommand, "UnitID", DbType.Int32, model.UnitID);
            db.AddInParameter(dbCommand, "TradeWayID", DbType.Int32, model.TradeWayID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #endregion

        #region 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// <summary>
        /// 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetXHAndHKBreedClassNameByBreedClassID()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBREEDCLASSNAME_XHANDHKSPOTTRADERULES);
            return database.ExecuteDataSet(dbCommand);
        }
        #endregion

    }
}