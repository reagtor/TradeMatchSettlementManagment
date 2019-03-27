#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.QueryFilter;

#endregion

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类XH_AcccountHoldFreezeTableDal。
    /// </summary>
    public class XH_AcccountHoldFreezeTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int HoldFreezeLogoId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_AcccountHoldFreezeTable where HoldFreezeLogoId=@HoldFreezeLogoId ");
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
        /// 得到一个对象实体
        /// </summary>
        public  XH_AcccountHoldFreezeTableInfo GetModel(int FreezeTypeLogo, string entrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HoldFreezeLogoId,FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber from XH_AcccountHoldFreezeTable ");
            strSql.Append(" where FreezeTypeLogo=@FreezeTypeLogo and EntrustNumber=@EntrustNumber");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            XH_AcccountHoldFreezeTableInfo model = null;
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
        /// 得到一个对象实体
        /// </summary>
        public  XH_AcccountHoldFreezeTableInfo GetModelWithNoLock(int FreezeTypeLogo, string entrustNumber)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HoldFreezeLogoId,FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber from XH_AcccountHoldFreezeTable WITH (NOLOCK)");
            strSql.Append(" where FreezeTypeLogo=@FreezeTypeLogo and EntrustNumber=@EntrustNumber");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, FreezeTypeLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, entrustNumber);
            XH_AcccountHoldFreezeTableInfo model = null;
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
        /// 增加一条数据
        /// </summary>
        public int Add(XH_AcccountHoldFreezeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AcccountHoldFreezeTable(");
            strSql.Append("FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber)");

            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@prepareFreezeAmount,@ThawTime,@FreezeTypeLogo,@AccountHoldLogo,@EntrustNumber)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "prepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
         
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
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
        public int Add(XH_AcccountHoldFreezeTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AcccountHoldFreezeTable(");
            strSql.Append("FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber)");

            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@prepareFreezeAmount,@ThawTime,@FreezeTypeLogo,@AccountHoldLogo,@EntrustNumber)");
            strSql.Append(";select @@IDENTITY");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "prepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
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
        /// 增加一条数据
        /// </summary>
        public bool AddRecord(XH_AcccountHoldFreezeTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AcccountHoldFreezeTable(");
            strSql.Append("FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber)");

            strSql.Append(" values (");
            strSql.Append("@FreezeTime,@prepareFreezeAmount,@ThawTime,@FreezeTypeLogo,@AccountHoldLogo,@EntrustNumber)");
            strSql.Append(";select @@IDENTITY");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "prepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
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
        public void Update(XH_AcccountHoldFreezeTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AcccountHoldFreezeTable set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("prepareFreezeAmount=@prepareFreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo,");
            strSql.Append("AccountHoldLogo=@AccountHoldLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber");
            strSql.Append("where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, model.HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "prepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 带事物更新数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="db">Database</param>
        /// <param name="transaction">DbTransaction</param>
        public void Update(XH_AcccountHoldFreezeTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AcccountHoldFreezeTable set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("prepareFreezeAmount=@prepareFreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo,");
            strSql.Append("AccountHoldLogo=@AccountHoldLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, model.HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "prepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
            db.ExecuteNonQuery(dbCommand, transaction);
        }
        /// <summary>
        /// 带事物更新数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="db">Database</param>
        /// <param name="transaction">DbTransaction</param>
        public bool UpdateRecord(XH_AcccountHoldFreezeTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AcccountHoldFreezeTable set ");
            strSql.Append("FreezeTime=@FreezeTime,");
            strSql.Append("prepareFreezeAmount=@prepareFreezeAmount,");
            strSql.Append("ThawTime=@ThawTime,");
            strSql.Append("FreezeTypeLogo=@FreezeTypeLogo,");
            strSql.Append("AccountHoldLogo=@AccountHoldLogo,");
            strSql.Append("EntrustNumber=@EntrustNumber");
            strSql.Append("where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, model.HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "FreezeTime", DbType.DateTime, model.FreezeTime);
            db.AddInParameter(dbCommand, "prepareFreezeAmount", DbType.Int32, model.PrepareFreezeAmount);
            db.AddInParameter(dbCommand, "ThawTime", DbType.DateTime, model.ThawTime);
            db.AddInParameter(dbCommand, "FreezeTypeLogo", DbType.Int32, model.FreezeTypeLogo);
            db.AddInParameter(dbCommand, "AccountHoldLogo", DbType.Int32, model.AccountHoldLogo);
            db.AddInParameter(dbCommand, "EntrustNumber", DbType.AnsiString, model.EntrustNumber);
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
        /// 清除冻结的持仓
        /// </summary>
        /// <param name="HoldFreezeLogoId"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        public void Clear(int HoldFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AcccountHoldFreezeTable set ");
            strSql.Append("prepareFreezeAmount=@prepareFreezeAmount");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            db.AddInParameter(dbCommand, "prepareFreezeAmount", DbType.Int32, 0);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int HoldFreezeLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_AcccountHoldFreezeTable ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int HoldFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_AcccountHoldFreezeTable ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            db.ExecuteNonQuery(dbCommand, transaction);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteRecord(int HoldFreezeLogoId, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_AcccountHoldFreezeTable ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
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
        /// </summary>
        public XH_AcccountHoldFreezeTableInfo GetModel(int HoldFreezeLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select HoldFreezeLogoId,FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber from XH_AcccountHoldFreezeTable ");
            strSql.Append(" where HoldFreezeLogoId=@HoldFreezeLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HoldFreezeLogoId", DbType.Int32, HoldFreezeLogoId);
            XH_AcccountHoldFreezeTableInfo model = null;
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
                "select sum(prepareFreezeAmount) from XH_AcccountHoldFreezeTable");
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
        /// </summary>
        public List<XH_AcccountHoldFreezeTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select HoldFreezeLogoId,FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber ");
            strSql.Append(" FROM XH_AcccountHoldFreezeTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_AcccountHoldFreezeTableInfo> list = new List<XH_AcccountHoldFreezeTableInfo>();
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
        public List<XH_AcccountHoldFreezeTableInfo> GetAllListArray()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select HoldFreezeLogoId,FreezeTime,prepareFreezeAmount,ThawTime,FreezeTypeLogo,AccountHoldLogo,EntrustNumber ");
            strSql.Append(" FROM XH_AcccountHoldFreezeTable ");
          
            List<XH_AcccountHoldFreezeTableInfo> list = new List<XH_AcccountHoldFreezeTableInfo>();
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
        public XH_AcccountHoldFreezeTableInfo ReaderBind(IDataReader dataReader)
        {
            XH_AcccountHoldFreezeTableInfo model = new XH_AcccountHoldFreezeTableInfo();
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
            ojb = dataReader["prepareFreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PrepareFreezeAmount = (int)ojb;
            }
            ojb = dataReader["ThawTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ThawTime = (DateTime) ojb;
            }
            ojb = dataReader["FreezeTypeLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeTypeLogo = (int) ojb;
            }
            ojb = dataReader["AccountHoldLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountHoldLogo = (int) ojb;
            }
            model.EntrustNumber = dataReader["EntrustNumber"].ToString();
            return model;
        }
        ///// <summary>
        ///// 根据条件分页查询
        ///// </summary>
        ///// <param name="pageProcInfo">分页存储过程过滤条件</param>
        ///// <param name="total">总页数</param>
        ///// <returns></returns>
        //public List<XH_AcccountHoldFreezeTableInfo> PagingXH_AcccountHoldFreezeByFilter(PagingProceduresInfo pageProcInfo, out int total)
        //{
        //    List<XH_AcccountHoldFreezeTableInfo> list = new List<XH_AcccountHoldFreezeTableInfo>();
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