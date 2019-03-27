using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.DAL.Data.HK
{
    /// <summary>
    /// 港股资金冻结数据访问类HK_CapitalAccountFreezeDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    public class HK_CapitalAccountFreezeDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_CapitalAccountFreezeDal()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// <param name="CapitalFreezeLogoId"></param>
        /// </summary>
        public bool Exists(int CapitalFreezeLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_CapitalAccountFreeze where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
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
        /// <param name="model"></param>
        /// </summary>
        public int Add(HK_CapitalAccountFreezeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_CapitalAccountFreeze(");
            strSql.Append("CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,FreezeCapitalAmount,FreezeCost,OweCosting,ThawTime,FreezeTime)");

            strSql.Append(" values (");
            strSql.Append("@CapitalAccountLogo,@FreezeTypeLogo,@EntrustNumber,@FreezeCapitalAmount,@FreezeCost,@OweCosting,@ThawTime,@FreezeTime)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "FreezeCapitalAmount", DbType.Decimal, model.FreezeCapitalAmount);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
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
        public int Add(HK_CapitalAccountFreezeInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_CapitalAccountFreeze(");
            strSql.Append("FreezeTime,FreezeCapitalAmount,ThawTime,FreezeTypeLogo,");
            strSql.Append(" EntrustNumber,CapitalAccountLogo,OweCosting,FreezeCost)");
            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@FreezeCapitalAmount,@ThawTime,@FreezeTypeLogo,");
            strSql.Append(" @EntrustNumber,@CapitalAccountLogo,@OweCosting,@FreezeCost)");
            strSql.Append(";select @@IDENTITY");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeCapitalAmount", DbType.Decimal, model.FreezeCapitalAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddRecord(HK_CapitalAccountFreezeInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_CapitalAccountFreeze(");
            strSql.Append("FreezeTime,FreezeCapitalAmount,ThawTime,FreezeTypeLogo,EntrustNumber, ");
            strSql.Append(" CapitalAccountLogo,OweCosting,FreezeCost)");
            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@FreezeCapitalAmount,@ThawTime,@FreezeTypeLogo,");
            strSql.Append(" @EntrustNumber,@CapitalAccountLogo,@OweCosting,@FreezeCost)");
            strSql.Append(";select @@IDENTITY");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeCapitalAmount", DbType.Decimal, model.FreezeCapitalAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return false;
            }

            model.CapitalFreezeLogoId = result;
            return true;
        }


        /// <summary>
        /// 更新一条数据
        /// <param name="model"></param>
        /// </summary>
        public void Update(HK_CapitalAccountFreezeInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_CapitalAccountFreeze set ");
            strSql.Append("CapitalAccountLogo=@CapitalAccountLogo,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("FreezeCapitalAmount=@FreezeCapitalAmount,");
            strSql.Append("FreezeCost=@FreezeCost,");
            strSql.Append("OweCosting=@OweCosting,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("FreezeTime=@FreezeTime");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, model.CapitalFreezeLogoId);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "FreezeCapitalAmount", DbType.Decimal, model.FreezeCapitalAmount);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(HK_CapitalAccountFreezeInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_CapitalAccountFreeze set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("FreezeCapitalAmount=@FreezeCapitalAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("CapitalAccountLogo=@CapitalAccountLogo,");
            strSql.Append("OweCosting=@OweCosting,");
            strSql.Append("FreezeCost=@FreezeCost");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeCapitalAmount", DbType.Decimal, model.FreezeCapitalAmount);
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, model.CapitalFreezeLogoId);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            db.ExecuteNonQuery(dbCommand, tm.Transaction);
        }

        /// <summary>
        /// 清除冻结的金额和费用
        /// </summary>
        public void Clear(int capitalFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_CapitalAccountFreeze set ");
            strSql.Append("FreezeCapitalAmount=@FreezeCapitalAmount,");
            strSql.Append("FreezeCost=@FreezeCost");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeCapitalAmount", DbType.Decimal, 0);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, 0);
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, capitalFreezeLogoId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// <param name="CapitalFreezeLogoId"></param>
        /// </summary>
        public void Delete(int CapitalFreezeLogoId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_CapitalAccountFreeze ");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="capitalFreezeLogoId">资金冻结Id</param>
        /// <param name="reckoningTransaction">事务封装对象</param>
        public void Delete(int capitalFreezeLogoId, ReckoningTransaction reckoningTransaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_CapitalAccountFreeze ");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = reckoningTransaction.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, capitalFreezeLogoId);
            db.ExecuteNonQuery(dbCommand, reckoningTransaction.Transaction);
        }

        /// <summary>
        /// 得到一个对象实体
        ///<param name="CapitalFreezeLogoId"></param>
        /// </summary>
        public HK_CapitalAccountFreezeInfo GetModel(int CapitalFreezeLogoId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalFreezeLogoId,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,FreezeCapitalAmount,FreezeCost,OweCosting,ThawTime,FreezeTime from HK_CapitalAccountFreeze ");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
            HK_CapitalAccountFreezeInfo model = null;
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
        public List<HK_CapitalAccountFreezeInfo> GetListArrayWithNoLock(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FreezeTime,FreezeCapitalAmount,CapitalFreezeLogoId,");
            strSql.Append(" ThawTime,FreezeTypeLogo,EntrustNumber,CapitalAccountLogo,OweCosting,FreezeCost");
            strSql.Append(" FROM HK_CapitalAccountFreeze  WITH (NOLOCK)");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HK_CapitalAccountFreezeInfo> list = new List<HK_CapitalAccountFreezeInfo>();
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
        /// 获取所有冻结的列表
        /// </summary>
        /// <returns></returns>
        public List<HK_CapitalAccountFreezeSum> GetAllFreezeMoney()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalAccountLogo,sum(freezecapitalamount+freezecost) FreezeCapitalSum ");
            strSql.Append("from HK_CapitalAccountFreeze ");
            strSql.Append("group by CapitalAccountLogo");

            List<HK_CapitalAccountFreezeSum> list = new List<HK_CapitalAccountFreezeSum>();
            Database db = DatabaseFactory.CreateDatabase();

            using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind2(dataReader));
                }
            }
            return list;
        }

        /// <summary>
        /// 获得数据列表
        /// <param name="strWhere"></param>
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalFreezeLogoId,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,FreezeCapitalAmount,FreezeCost,OweCosting,ThawTime,FreezeTime ");
            strSql.Append(" FROM HK_CapitalAccountFreeze ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表 
        /// <param name="strWhere"></param>
        /// </summary>
        public List<HK_CapitalAccountFreezeInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalFreezeLogoId,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,FreezeCapitalAmount,FreezeCost,OweCosting,ThawTime,FreezeTime ");
            strSql.Append(" FROM HK_CapitalAccountFreeze ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HK_CapitalAccountFreezeInfo> list = new List<HK_CapitalAccountFreezeInfo>();
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
        public HK_CapitalAccountFreezeInfo ReaderBind(IDataReader dataReader)
        {
            HK_CapitalAccountFreezeInfo model = new HK_CapitalAccountFreezeInfo();
            object ojb;
            ojb = dataReader["CapitalFreezeLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalFreezeLogoId = (int)ojb;
            }
            ojb = dataReader["CapitalAccountLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogo = (int)ojb;
            }
            ojb = dataReader["FreezeTypeLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTypeLogo = (int)ojb;
            }
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            ojb = dataReader["FreezeCapitalAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapitalAmount = (decimal)ojb;
            }
            ojb = dataReader["FreezeCost"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCost = (decimal)ojb;
            }
            ojb = dataReader["OweCosting"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OweCosting = (decimal)ojb;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private HK_CapitalAccountFreezeSum ReaderBind2(IDataReader dataReader)
        {
            HK_CapitalAccountFreezeSum model = new HK_CapitalAccountFreezeSum();
            object ojb;
            ojb = dataReader["CapitalAccountLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogo = (int)ojb;
            }
            ojb = dataReader["FreezeCapitalSum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapitalSum = (decimal)ojb;
            }
            return model;
        }
        #endregion  成员方法
    }
}
