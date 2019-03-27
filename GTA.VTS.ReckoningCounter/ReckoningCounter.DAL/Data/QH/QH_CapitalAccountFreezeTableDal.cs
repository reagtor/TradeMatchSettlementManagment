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
    /// 数据访问类QH_CapitalAccountFreezeTableDal。
    /// </summary>
    public class QH_CapitalAccountFreezeTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CapitalFreezeLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select count(1) from QH_CapitalAccountFreezeTable where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
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
        /// 获得数据列表
        /// </summary>
        public  List<QH_CapitalAccountFreezeTableInfo> GetListArrayWithNoLock(string enTrustNumber, int freezeType)
        {
            string format = "EntrustNumber='{0}' AND FreezeTypeLogo={1}";
            string where = string.Format(format, enTrustNumber, freezeType);

            return GetListArrayWithNoLock(where);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public  List<QH_CapitalAccountFreezeTableInfo> GetListArrayWithNoLock(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalFreezeLogoId,FreezeTime,FreezeAmount,ThawTime,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,OweCosting,FreezeCost ");
            strSql.Append(" FROM QH_CapitalAccountFreezeTable WITH (NOLOCK)");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_CapitalAccountFreezeTableInfo> list = new List<QH_CapitalAccountFreezeTableInfo>();
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
        public  List<QH_CapitalAccountFreezeTableInfo> GetListArray(string enTrustNumber, int freezeType)
        {
            string format = "EntrustNumber='{0}' AND FreezeTypeLogo={1}";
            string where = string.Format(format, enTrustNumber, freezeType);

            return GetListArray(where);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(QH_CapitalAccountFreezeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_CapitalAccountFreezeTable(");
            strSql.Append(
                "FreezeTime,FreezeAmount,ThawTime,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,OweCosting,FreezeCost)");

            strSql.Append(" values (");
            strSql.Append(
                "@FreezeTime,@FreezeAmount,@ThawTime,@CapitalAccountLogo,@FreezeTypeLogo,@EntrustNumber,@OweCosting,@FreezeCost)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
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
        public int Add(QH_CapitalAccountFreezeTableInfo model,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_CapitalAccountFreezeTable(");
            strSql.Append(
                "FreezeTime,FreezeAmount,ThawTime,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,OweCosting,FreezeCost)");

            strSql.Append(" values (");
            strSql.Append(
                "@FreezeTime,@FreezeAmount,@ThawTime,@CapitalAccountLogo,@FreezeTypeLogo,@EntrustNumber,@OweCosting,@FreezeCost)");
            strSql.Append(";select @@IDENTITY");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
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
        public bool AddRecord(QH_CapitalAccountFreezeTableInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_CapitalAccountFreezeTable(");
            strSql.Append(
                "FreezeTime,FreezeAmount,ThawTime,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,OweCosting,FreezeCost)");

            strSql.Append(" values (");
            strSql.Append(
                "@FreezeTime,@FreezeAmount,@ThawTime,@CapitalAccountLogo,@FreezeTypeLogo,@EntrustNumber,@OweCosting,@FreezeCost)");
            strSql.Append(";select @@IDENTITY");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            int result;
            object obj = db.ExecuteScalar(dbCommand, tm.Transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return false;
            }
            model.CapitalFreezeLogoId = result;
            return true;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_CapitalAccountFreezeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountFreezeTable set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("CapitalAccountLogo=@CapitalAccountLogo,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("OweCosting=@OweCosting,");
            strSql.Append("FreezeCost=@FreezeCost");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, model.CapitalFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 清除冻结的金额和费用
        /// </summary>
        public void Clear(int capitalFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountFreezeTable set ");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("FreezeCost=@FreezeCost");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, capitalFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, 0);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, 0);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_CapitalAccountFreezeTableInfo model,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountFreezeTable set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("CapitalAccountLogo=@CapitalAccountLogo,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber,");
            strSql.Append("OweCosting=@OweCosting,");
            strSql.Append("FreezeCost=@FreezeCost");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = tm.Database;
            DbTransaction trans = tm.Transaction;   
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, model.CapitalFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.AddInParameter(dbCommand, "OweCosting", DbType.Decimal, model.OweCosting);
            db.AddInParameter(dbCommand, "FreezeCost", DbType.Decimal, model.FreezeCost);
            db.ExecuteNonQuery(dbCommand, trans);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CapitalFreezeLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_CapitalAccountFreezeTable ");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CapitalFreezeLogoId,ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_CapitalAccountFreezeTable ");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
            db.ExecuteNonQuery(dbCommand, tm.Transaction);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteRecord(int CapitalFreezeLogoId, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_CapitalAccountFreezeTable ");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = tm.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
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
        public QH_CapitalAccountFreezeTableInfo GetModel(int CapitalFreezeLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalFreezeLogoId,FreezeTime,FreezeAmount,ThawTime,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,OweCosting,FreezeCost from QH_CapitalAccountFreezeTable ");
            strSql.Append(" where CapitalFreezeLogoId=@CapitalFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFreezeLogoId", DbType.Int32, CapitalFreezeLogoId);
            QH_CapitalAccountFreezeTableInfo model = null;
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
        public List<QH_CapitalAccountFreezeTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalFreezeLogoId,FreezeTime,FreezeAmount,ThawTime,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,OweCosting,FreezeCost ");
            strSql.Append(" FROM QH_CapitalAccountFreezeTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_CapitalAccountFreezeTableInfo> list = new List<QH_CapitalAccountFreezeTableInfo>();
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
        /// 获取按账户汇总的冻结资金
        /// </summary>
        /// <returns></returns>
        public List<QH_CapitalAccountFreezeSum> GetAllFreezeMoney()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CapitalAccountLogo,sum(freezeamount+freezecost) FreezeCapitalSum ");
            strSql.Append("from qh_capitalaccountfreezetable ");
            //strSql.Append("where freezetypelogo=1 ");
            strSql.Append("group by CapitalAccountLogo");

            List<QH_CapitalAccountFreezeSum> list = new List<QH_CapitalAccountFreezeSum>();
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

        private QH_CapitalAccountFreezeSum ReaderBind2(IDataReader dataReader)
        {
            QH_CapitalAccountFreezeSum model = new QH_CapitalAccountFreezeSum();
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

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public QH_CapitalAccountFreezeTableInfo ReaderBind(IDataReader dataReader)
        {
            QH_CapitalAccountFreezeTableInfo model = new QH_CapitalAccountFreezeTableInfo();
            object ojb;
            ojb = dataReader["CapitalFreezeLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalFreezeLogoId = (int) ojb;
            }
            ojb = dataReader["FreezeTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTime = (DateTime) ojb;
            }
            ojb = dataReader["FreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeAmount = (decimal) ojb;
            }
            ojb = dataReader["ThawTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ThawTime = (DateTime) ojb;
            }
            ojb = dataReader["CapitalAccountLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogo = (int) ojb;
            }
            ojb = dataReader["FreezeTypeLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTypeLogo = (int) ojb;
            }
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            ojb = dataReader["OweCosting"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OweCosting = (decimal) ojb;
            }
            ojb = dataReader["FreezeCost"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCost = (decimal) ojb;
            }
            return model;
        }

        ///// <summary>
        ///// 根据条件分页查询
        ///// </summary>
        ///// <param name="pageProcInfo">分页存储过程过滤条件</param>
        ///// <param name="total">总页数</param>
        ///// <returns></returns>
        //public List<QH_CapitalAccountFreezeTableInfo> PagingQH_CapitalAccountFreezeByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<QH_CapitalAccountFreezeTableInfo> list = new List<QH_CapitalAccountFreezeTableInfo>();
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