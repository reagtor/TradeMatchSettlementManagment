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
    ///描述：品种_现货_交易费用_成交额_交易手续费  数据访问类XH_SpotRangeCostDAL。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class XH_SpotRangeCostDAL
    {
        public XH_SpotRangeCostDAL()
        {
        }

        #region SQL

        /// <summary>
        /// 根据品种ID获取现货交易费用交易手续费_范围_值
        /// </summary>
        private string SQL_SELECT_XHSPOTRANGECOST =
            @"SELECT A.*,B.[VALUE],B.BREEDCLASSID FROM 
CM_FIELDRANGE A,XH_SpotRangeCost B
WHERE A.FIELDRANGEID=B.FIELDRANGEID 
AND B.BREEDCLASSID=@BREEDCLASSID";

        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(FieldRangeID)+1 from XH_SpotRangeCost";
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
        public bool Exists(int FieldRangeID, int BreedClassID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from XH_SpotRangeCost where FieldRangeID=@FieldRangeID and BreedClassID=@BreedClassID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
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
        /// 增加一条数据
        /// </summary>
        public bool Add(ManagementCenter.Model.XH_SpotRangeCost model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_SpotRangeCost(");
            strSql.Append("Value,FieldRangeID,BreedClassID)");

            strSql.Append(" values (");
            strSql.Append("@Value,@FieldRangeID,@BreedClassID)");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Value", DbType.Decimal, model.Value);
            //db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, model.FieldRangeID);
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

        #region 添加现货交易费用交易手续费

        /// <summary>
        /// 添加现货交易费用交易手续费
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.XH_SpotRangeCost model)
        {
            return Add(model, null, null);
        }

        #endregion

        #region  更新现货交易手续费

        /// <summary>
        /// 更新现货交易手续费
        /// </summary>
        /// <param name="model">现货交易手续费实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_SpotRangeCost model, DbTransaction tran,
                           Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_SpotRangeCost set ");
            strSql.Append("Value=@Value");
            strSql.Append(" where FieldRangeID=@FieldRangeID and BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Value", DbType.Decimal, model.Value);
            //db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, model.FieldRangeID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
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

        #region 更新现货交易手续费

        /// <summary>
        /// 更新现货交易手续费
        /// </summary>
        /// <param name="model">现货交易手续费实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_SpotRangeCost model)
        {
            return Update(model, null, null);
        }

        #endregion

        #region 根据字段范围ID，品种ID，删除现货交易费用手续费

        /// <summary>
        /// 根据字段范围ID，品种ID，删除现货交易费用手续费
        /// </summary>
        /// <param name="FieldRangeID">字段范围ID</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int FieldRangeID, int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_SpotRangeCost ");
            strSql.Append(" where FieldRangeID=@FieldRangeID and BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
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

        #endregion

        #region 根据字段范围ID，品种ID，删除现货交易费用手续费

        /// <summary>
        /// 根据字段范围ID，品种ID，删除现货交易费用手续费
        /// </summary>
        /// <param name="FieldRangeID">字段范围ID</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool Delete(int FieldRangeID, int BreedClassID)
        {
            return Delete(FieldRangeID, BreedClassID);
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_SpotRangeCost GetModel(int FieldRangeID, int BreedClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value,FieldRangeID,BreedClassID from XH_SpotRangeCost ");
            strSql.Append(" where FieldRangeID=@FieldRangeID and BreedClassID=@BreedClassID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            ManagementCenter.Model.XH_SpotRangeCost model = null;
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
            strSql.Append("select Value,FieldRangeID,BreedClassID ");
            strSql.Append(" FROM XH_SpotRangeCost ");
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
        public List<ManagementCenter.Model.XH_SpotRangeCost> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select Value,FieldRangeID,BreedClassID ");
            strSql.Append("select Value,BreedClassID ");
            strSql.Append(" FROM XH_SpotRangeCost ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.XH_SpotRangeCost> list = new List<ManagementCenter.Model.XH_SpotRangeCost>();
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
        public ManagementCenter.Model.XH_SpotRangeCost ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.XH_SpotRangeCost model = new ManagementCenter.Model.XH_SpotRangeCost();
            object ojb;
            ojb = dataReader["Value"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Value = (decimal) ojb;
            }
            //ojb = dataReader["FieldRangeID"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.FieldRangeID = (int) ojb;
            //}
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int) ojb;
            }
            return model;
        }

        #region 根据品种ID获取现货交易费用交易手续费_范围_值

        /// <summary>
        /// 根据品种ID获取现货交易费用交易手续费_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public DataSet GetXHSpotRangeCostByBreedClassID(int BreedClassID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_XHSPOTRANGECOST);
            database.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);

            return database.ExecuteDataSet(dbCommand);
        }

        #endregion

        #endregion  成员方法
    }
}