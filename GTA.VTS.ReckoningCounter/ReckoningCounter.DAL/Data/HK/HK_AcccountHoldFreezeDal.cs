using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using ReckoningCounter.Entity.Model.HK;
using GTA.VTS.Common.CommonUtility;

namespace ReckoningCounter.DAL.Data.HK
{
    /// <summary>
    /// 港股持仓冻结数据访问类HK_AcccountHoldFreezeDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class HK_AcccountHoldFreezeDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_AcccountHoldFreezeDal()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// <param name="HoldFreezeLogoId">持仓冻结标识</param>
        /// </summary>
        public bool Exists(int HoldFreezeLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_AcccountHoldFreeze where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
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
        /// <param name="model">港股持仓冻结实体</param>
        /// </summary>
        public int Add(HK_AcccountHoldFreezeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_AcccountHoldFreeze(");
            strSql.Append("EntrustNumber,PrepareFreezeAmount,FreezeTypeID,AccountHoldLogo,ThawTime,FreezeTime)");

            strSql.Append(" values (");
            strSql.Append("@EntrustNumber,@PrepareFreezeAmount,@FreezeTypeID,@AccountHoldLogo,@ThawTime,@FreezeTime)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PrepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "FreezeTypeID", DbType.Int32, model.FreezeTypeID);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(HK_AcccountHoldFreezeInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_AcccountHoldFreeze(");
            strSql.Append("FreezeTime,PrepareFreezeAmount,ThawTime,FreezeTypeID,AccountHoldLogo,EntrustNumber)");

            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@prepareFreezeAmount,@ThawTime,@FreezeTypeID,@AccountHoldLogo,@EntrustNumber)");
            strSql.Append(";select @@IDENTITY");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "PrepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeID", DbType.Int32, model.FreezeTypeID);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 更新一条数据
        /// <param name="model">港股持仓冻结实体</param>
        /// </summary>
        public void Update(HK_AcccountHoldFreezeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_AcccountHoldFreeze set ");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("PrepareFreezeAmount=@PrepareFreezeAmount,");
            strSql.Append("FreezeTypeID=@FreezeTypeID,");
            strSql.Append("AccountHoldLogo=@AccountHoldLogo,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("FreezeTime=@FreezeTime");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, model.HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "PrepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "FreezeTypeID", DbType.Int32, model.FreezeTypeID);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 带事物更新数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="db">Database</param>
        /// <param name="transaction">DbTransaction</param>
        public void Update(HK_AcccountHoldFreezeInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_AcccountHoldFreeze set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("PrepareFreezeAmount=@prepareFreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("FreezeTypeID=@FreezeTypeID,");
            strSql.Append("AccountHoldLogo=@AccountHoldLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, model.HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "PrepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeID", DbType.Int32, model.FreezeTypeID);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 清除冻结的持仓
        /// </summary>
        /// <param name="holdFreezeLogoId">持仓冻结Id</param>
        /// <param name="db">数据库对象</param>
        /// <param name="transaction">事务对象</param>
        public void Clear(int holdFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_AcccountHoldFreeze set ");
            strSql.Append("PrepareFreezeAmount=@PrepareFreezeAmount");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, holdFreezeLogoId);
            db.AddInParameter(dbCommand, "PrepareFreezeAmount", DbType.Int32, 0);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 删除一条数据
        /// <param name="HoldFreezeLogoId">港股持仓冻结标识</param>
        /// </summary>
        public void Delete(int HoldFreezeLogoId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_AcccountHoldFreeze ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            db.ExecuteNonQuery(dbCommand);

        }
       
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteRecord(int holdFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_AcccountHoldFreeze ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, holdFreezeLogoId);
            try
            {
                db.ExecuteNonQuery(dbCommand, transaction);
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 得到一个对象实体
        /// <param name="HoldFreezeLogoId">港股持仓冻结标识</param>
        /// </summary>
        public HK_AcccountHoldFreezeInfo GetModel(int HoldFreezeLogoId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HoldFreezeLogoId,EntrustNumber,PrepareFreezeAmount,FreezeTypeID,AccountHoldLogo,ThawTime,FreezeTime from HK_AcccountHoldFreeze ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            HK_AcccountHoldFreezeInfo model = null;
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
        /// 获取某个持仓的全部冻结量
        /// </summary>
        /// <param name="accountHoldId"></param>
        /// <returns></returns>
        public int GetAllFreezeAmount(int accountHoldId)
        {
            int result = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select sum(PrepareFreezeAmount) from HK_AcccountHoldFreeze");
            strSql.Append(
                " where AccountHoldLogo=@AccountHoldLogo");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, accountHoldId);

            try
            {
                object obj = db.ExecuteScalar(dbCommand);
                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out result);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 获得数据列表
        /// <param name="strWhere">查询条件语句SQL</param>
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HoldFreezeLogoId,EntrustNumber,PrepareFreezeAmount,FreezeTypeID,AccountHoldLogo,ThawTime,FreezeTime ");
            strSql.Append(" FROM HK_AcccountHoldFreeze ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// <param name="strWhere">查询条件语句SQL</param>
        /// </summary>
        public List<HK_AcccountHoldFreezeInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HoldFreezeLogoId,EntrustNumber,PrepareFreezeAmount,FreezeTypeID,AccountHoldLogo,ThawTime,FreezeTime ");
            strSql.Append(" FROM HK_AcccountHoldFreeze ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HK_AcccountHoldFreezeInfo> list = new List<HK_AcccountHoldFreezeInfo>();
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
        /// 获得数据列表
        /// </summary>
        public List<HK_AcccountHoldFreezeInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HoldFreezeLogoId,EntrustNumber,PrepareFreezeAmount,FreezeTypeID,AccountHoldLogo,ThawTime,FreezeTime ");
            strSql.Append(" FROM HK_AcccountHoldFreeze ");
            
            List<HK_AcccountHoldFreezeInfo> list = new List<HK_AcccountHoldFreezeInfo>();
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
        /// <param name="dataReader"></param>
        /// </summary>
        public HK_AcccountHoldFreezeInfo ReaderBind(IDataReader dataReader)
        {
            HK_AcccountHoldFreezeInfo model = new HK_AcccountHoldFreezeInfo();
            object ojb;
            ojb = dataReader["HoldFreezeLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HoldFreezeLogoId = (int)ojb;
            }
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            ojb = dataReader["PrepareFreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PrepareFreezeAmount = (int)ojb;
            }
            ojb = dataReader["FreezeTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTypeID = (int)ojb;
            }
            ojb = dataReader["AccountHoldLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountHoldLogo = (int)ojb;
            }
            ojb = dataReader["ThawTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ThawTime = (DateTime)ojb;
            }
            ojb = dataReader["FreezeTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTime = (DateTime)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}
