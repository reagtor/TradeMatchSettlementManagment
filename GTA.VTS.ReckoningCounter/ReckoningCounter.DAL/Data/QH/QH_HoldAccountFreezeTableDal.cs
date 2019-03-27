#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.QueryFilter;

#endregion

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类QH_HoldAccountFreezeTableDal。
    /// </summary>
    public class QH_HoldAccountFreezeTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int HoldFreezeLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_HoldAccountFreezeTable where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
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
        public int Add(QH_HoldAccountFreezeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_HoldAccountFreezeTable(");
            strSql.Append("FreezeTime,FreezeAmount,ThawTime,EntrustNumber,AccountHoldLogo,FreezeTypeLogo)");

            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@FreezeAmount,@ThawTime,@EntrustNumber,@AccountHoldLogo,@FreezeTypeLogo)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Int32, model.FreezeAmount);
         
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
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
        public int Add(QH_HoldAccountFreezeTableInfo model,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_HoldAccountFreezeTable(");
            strSql.Append("FreezeTime,FreezeAmount,ThawTime,EntrustNumber,AccountHoldLogo,FreezeTypeLogo)");

            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@FreezeAmount,@ThawTime,@EntrustNumber,@AccountHoldLogo,@FreezeTypeLogo)");
            strSql.Append(";select @@IDENTITY");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Int32, model.FreezeAmount);

            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            int result;
            object obj = db.ExecuteScalar(dbCommand,tm.Transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddRecoud(QH_HoldAccountFreezeTableInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_HoldAccountFreezeTable(");
            strSql.Append("FreezeTime,FreezeAmount,ThawTime,EntrustNumber,AccountHoldLogo,FreezeTypeLogo)");

            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@FreezeAmount,@ThawTime,@EntrustNumber,@AccountHoldLogo,@FreezeTypeLogo)");
            strSql.Append(";select @@IDENTITY");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Int32, model.FreezeAmount);

            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            int result;
            object obj = db.ExecuteScalar(dbCommand, tm.Transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return false;

            }
            model.HoldFreezeLogoId = result;
            return true;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_HoldAccountFreezeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HoldAccountFreezeTable set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("AccountHoldLogo=@AccountHoldLogo,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, model.HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Int32, model.FreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 清除冻结的持仓
        /// </summary>
        /// <param name="HoldFreezeLogoId"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void Clear(int HoldFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HoldAccountFreezeTable set ");
            strSql.Append("FreezeAmount=@FreezeAmount");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Int32, 0);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_HoldAccountFreezeTableInfo model,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_HoldAccountFreezeTable set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("AccountHoldLogo=@AccountHoldLogo,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = tm.Database;
            DbTransaction trans = tm.Transaction;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, model.HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Int32, model.FreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.ExecuteNonQuery(dbCommand, trans);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int HoldFreezeLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_HoldAccountFreezeTable ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteRecord(int HoldFreezeLogoId,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_HoldAccountFreezeTable ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            try
            {
                db.ExecuteNonQuery(dbCommand, tm.Transaction);
                return true;
            }
            catch
            {
                return false;
                
            }
            
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_HoldAccountFreezeTableInfo GetModel(int HoldFreezeLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select HoldFreezeLogoId,FreezeTime,FreezeAmount,ThawTime,EntrustNumber,AccountHoldLogo,FreezeTypeLogo from QH_HoldAccountFreezeTable ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            QH_HoldAccountFreezeTableInfo model = null;
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
        public List<QH_HoldAccountFreezeTableInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select HoldFreezeLogoId,FreezeTime,FreezeAmount,ThawTime,EntrustNumber,AccountHoldLogo,FreezeTypeLogo ");
            strSql.Append(" FROM QH_HoldAccountFreezeTable ");
            
            List<QH_HoldAccountFreezeTableInfo> list = new List<QH_HoldAccountFreezeTableInfo>();
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
        public List<QH_HoldAccountFreezeTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select HoldFreezeLogoId,FreezeTime,FreezeAmount,ThawTime,EntrustNumber,AccountHoldLogo,FreezeTypeLogo ");
            strSql.Append(" FROM QH_HoldAccountFreezeTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_HoldAccountFreezeTableInfo> list = new List<QH_HoldAccountFreezeTableInfo>();
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
        public List<QH_HoldAccountFreezeTableInfo> GetListArray(string strWhere,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select HoldFreezeLogoId,FreezeTime,FreezeAmount,ThawTime,EntrustNumber,AccountHoldLogo,FreezeTypeLogo ");
            strSql.Append(" FROM QH_HoldAccountFreezeTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_HoldAccountFreezeTableInfo> list = new List<QH_HoldAccountFreezeTableInfo>();
            Database db = tm.Database;
            using (IDataReader dataReader = db.ExecuteReader(tm.Transaction,CommandType.Text, strSql.ToString()))
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
        public QH_HoldAccountFreezeTableInfo ReaderBind(IDataReader dataReader)
        {
            QH_HoldAccountFreezeTableInfo model = new QH_HoldAccountFreezeTableInfo();
            object ojb;
            ojb = dataReader["HoldFreezeLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HoldFreezeLogoId = (int) ojb;
            }
            ojb = dataReader["FreezeTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTime = (DateTime) ojb;
            }
            ojb = dataReader["FreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeAmount = (int) ojb;
            }
            ojb = dataReader["ThawTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ThawTime = (DateTime) ojb;
            }
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            ojb = dataReader["AccountHoldLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountHoldLogo = (int) ojb;
            }
            ojb = dataReader["FreezeTypeLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTypeLogo = (int) ojb;
            }
            return model;
        }
        ///// <summary>
        ///// 根据条件分页查询
        ///// </summary>
        ///// <param name="pageProcInfo">分页存储过程过滤条件</param>
        ///// <param name="total">总页数</param>
        ///// <returns></returns>
        //public List<QH_HoldAccountFreezeTableInfo> PagingQH_HoldAccountFreezeByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<QH_HoldAccountFreezeTableInfo> list = new List<QH_HoldAccountFreezeTableInfo>();
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = CommonDALOperate.PagingProceduresDbCommand(db, pageProcInfo);
        //    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
        //    {
        //        while (dataReader.Read())
        //        {
        //            list.Add(ReaderBind(dataReader));
        //        }

        //    }
        //    total = db.GetParameterValue(dbCommand, "@Total") != null ? (int)db.GetParameterValue(dbCommand, "@Total") : 0;
        //    return list;
        //}
        #endregion  成员方法
    }
}