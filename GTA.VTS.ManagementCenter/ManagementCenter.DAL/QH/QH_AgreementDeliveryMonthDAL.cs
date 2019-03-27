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
    ///描述：合约交割月份 数据访问类QH_AgreementDeliveryMonthDAL。
    ///作者：刘书伟
    ///日期:2008-11-21
    ///修改：叶振东
    ///日期：2010-01-20
    ///描述：添加通过交割月份标识和品种标识查询此交割月份是否存在
    /// </summary>
    public class QH_AgreementDeliveryMonthDAL
    {
        public QH_AgreementDeliveryMonthDAL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(AgreementDeliveryMonthID)+1 from QH_AgreementDeliveryMonth";
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
        public bool Exists(int AgreementDeliveryMonthID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from QH_AgreementDeliveryMonth where AgreementDeliveryMonthID=@AgreementDeliveryMonthID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AgreementDeliveryMonthID", DbType.Int32, AgreementDeliveryMonthID);
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

        #region 添加合约交割月份

        /// <summary>
        /// 添加合约交割月份
        /// </summary>
        /// <param name="model">合约交割月份实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.QH_AgreementDeliveryMonth model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_AgreementDeliveryMonth(");
            strSql.Append("MonthID,BreedClassID)");

            strSql.Append(" values (");
            strSql.Append("@MonthID,@BreedClassID)");
            strSql.Append(";select @@IDENTITY");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MonthID", DbType.Int32, model.MonthID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            //int result;
            //object obj = db.ExecuteScalar(dbCommand);
            //if(!int.TryParse(obj.ToString(),out result))
            //{
            //    return 0;
            //}
            //return result;
            object obj = null;

            if (tran == null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand, tran);
            }
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return AppGlobalVariable.INIT_INT;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        #endregion

        #region 添加合约交割月份

        /// <summary>
        /// 添加合约交割月份
        /// </summary>
        /// <param name="model">合约交割月份实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.QH_AgreementDeliveryMonth model)
        {
            return Add(model, null, null);
        }

        #endregion

        #region 更新合约交割月份

        /// <summary>
        /// 更新合约交割月份
        /// </summary>
        /// <param name="model">合约交割月份实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_AgreementDeliveryMonth model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_AgreementDeliveryMonth set ");
            strSql.Append("MonthID=@MonthID,");
            strSql.Append("BreedClassID=@BreedClassID");
            strSql.Append(" where AgreementDeliveryMonthID=@AgreementDeliveryMonthID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AgreementDeliveryMonthID", DbType.Int32, model.AgreementDeliveryMonthID);
            db.AddInParameter(dbCommand, "MonthID", DbType.Int32, model.MonthID);
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

        #region 更新合约交割月份

        /// <summary>
        /// 更新合约交割月份
        /// </summary>
        /// <param name="model">合约交割月份实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_AgreementDeliveryMonth model)
        {
            return Update(model, null, null);
        }

        #endregion

        #region 根据合约交割月份ID，删除合约交割月份

        /// <summary>
        /// 根据合约交割月份ID，删除合约交割月份
        /// </summary>
        /// <param name="AgreementDeliveryMonthID">合约交割月份ID</param>
        /// <returns></returns>
        public void Delete(int AgreementDeliveryMonthID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_AgreementDeliveryMonth ");
            strSql.Append(" where AgreementDeliveryMonthID=@AgreementDeliveryMonthID ");

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AgreementDeliveryMonthID", DbType.Int32, AgreementDeliveryMonthID);
            db.ExecuteNonQuery(dbCommand);
        }

        #endregion

        #region  根据合约月份ID和品种ID，删除合约交割月份

        /// <summary>
        ///  根据合约月份ID和品种ID，删除合约交割月份
        /// </summary>
        /// <param name="MonthID">月份ID</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int MonthID, int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_AgreementDeliveryMonth ");
            strSql.Append(" where MonthID=@MonthID and BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MonthID", DbType.Int32, MonthID);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);

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

        #region 根据合约月份ID和品种ID，删除合约交割月份

        /// <summary>
        ///  根据合约月份ID和品种ID，删除合约交割月份
        /// </summary>
        /// <param name="MonthID">月份ID</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool Delete(int MonthID, int BreedClassID)
        {
            return Delete(MonthID, BreedClassID, null, null);
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_AgreementDeliveryMonth GetModel(int AgreementDeliveryMonthID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AgreementDeliveryMonthID,MonthID,BreedClassID from QH_AgreementDeliveryMonth ");
            strSql.Append(" where AgreementDeliveryMonthID=@AgreementDeliveryMonthID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AgreementDeliveryMonthID", DbType.Int32, AgreementDeliveryMonthID);
            ManagementCenter.Model.QH_AgreementDeliveryMonth model = null;
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
            strSql.Append("select AgreementDeliveryMonthID,MonthID,BreedClassID ");
            strSql.Append(" FROM QH_AgreementDeliveryMonth ");
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
        public List<ManagementCenter.Model.QH_AgreementDeliveryMonth> GetListArray(string strWhere, DbTransaction tran,
                                                                                   Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AgreementDeliveryMonthID,MonthID,BreedClassID ");
            strSql.Append(" FROM QH_AgreementDeliveryMonth ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.QH_AgreementDeliveryMonth> list =
                new List<ManagementCenter.Model.QH_AgreementDeliveryMonth>();
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

        
        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<ManagementCenter.Model.QH_AgreementDeliveryMonth> GetListArray(string strWhere)
        {
            return GetListArray(strWhere, null, null);
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.QH_AgreementDeliveryMonth ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.QH_AgreementDeliveryMonth model =
                new ManagementCenter.Model.QH_AgreementDeliveryMonth();
            object ojb;
            ojb = dataReader["AgreementDeliveryMonthID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AgreementDeliveryMonthID = (int) ojb;
            }
            ojb = dataReader["MonthID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MonthID = (int) ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int) ojb;
            }
            return model;
        }
        /// <summary>
        /// 根据品种标识和月份标识查询是否存在此交割月份
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <param name="monthid">交割月份标识</param>
        /// <returns>是否存在此交割月份</returns>
        public ManagementCenter.Model.QH_AgreementDeliveryMonth GetQHAgreementDeliveryBreedClassID(int breedClassID, int monthid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AgreementDeliveryMonthID,MonthID,BreedClassID from QH_AgreementDeliveryMonth ");
            strSql.Append(" where MonthID=@monthid and  BreedClassID=@breedClassID");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "monthid", DbType.Int32, monthid);
            db.AddInParameter(dbCommand, "breedClassID", DbType.Int32, breedClassID);
            ManagementCenter.Model.QH_AgreementDeliveryMonth model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }

        #endregion  成员方法

      
    }
}