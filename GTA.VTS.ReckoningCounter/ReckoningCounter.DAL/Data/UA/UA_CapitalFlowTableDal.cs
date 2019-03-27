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
    /// 数据访问类UA_CapitalFlowTableDal。
    /// </summary>
    public class UA_CapitalFlowTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CapitalFlowLogo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UA_CapitalFlowTable where CapitalFlowLogo=@CapitalFlowLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFlowLogo", DbType.Int32, CapitalFlowLogo);
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
        public int Add(UA_CapitalFlowTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UA_CapitalFlowTable(");
            strSql.Append(
                "FromCapitalAccount,ToCapitalAccount,TransferAmount,TransferTime,TradeCurrencyType,TransferTypeLogo)");

            strSql.Append(" values (");
            strSql.Append(
                "@FromCapitalAccount,@ToCapitalAccount,@TransferAmount,@TransferTime,@TradeCurrencyType,@TransferTypeLogo)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FromCapitalAccount", DbType.AnsiString, model.FromCapitalAccount);
            db.AddInParameter(dbCommand, "ToCapitalAccount", DbType.AnsiString, model.ToCapitalAccount);
            db.AddInParameter(dbCommand, "TransferAmount", DbType.Decimal, model.TransferAmount);
            db.AddInParameter(dbCommand, "TransferTime", DbType.DateTime, model.TransferTime);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32, model.TransferTypeLogo);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 带事物更新数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="db">Database</param>
        /// <param name="transaction">DbTransaction</param>
        public void Add(UA_CapitalFlowTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UA_CapitalFlowTable(");
            strSql.Append(
                "FromCapitalAccount,ToCapitalAccount,TransferAmount,TransferTime,TradeCurrencyType,TransferTypeLogo)");

            strSql.Append(" values (");
            strSql.Append(
                "@FromCapitalAccount,@ToCapitalAccount,@TransferAmount,@TransferTime,@TradeCurrencyType,@TransferTypeLogo)");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FromCapitalAccount", DbType.AnsiString, model.FromCapitalAccount);
            db.AddInParameter(dbCommand, "ToCapitalAccount", DbType.AnsiString, model.ToCapitalAccount);
            db.AddInParameter(dbCommand, "TransferAmount", DbType.Decimal, model.TransferAmount);
            db.AddInParameter(dbCommand, "TransferTime", DbType.DateTime, model.TransferTime);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32, model.TransferTypeLogo);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(UA_CapitalFlowTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UA_CapitalFlowTable set ");
            strSql.Append("FromCapitalAccount=@FromCapitalAccount,");
            strSql.Append("ToCapitalAccount=@ToCapitalAccount,");
            strSql.Append("TransferAmount=@TransferAmount,");
            strSql.Append("TransferTime=@TransferTime,");
            strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("TransferTypeLogo=@TransferTypeLogo");
            strSql.Append(" where CapitalFlowLogo=@CapitalFlowLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFlowLogo", DbType.Int32, model.CapitalFlowLogo);
            db.AddInParameter(dbCommand, "FromCapitalAccount", DbType.AnsiString, model.FromCapitalAccount);
            db.AddInParameter(dbCommand, "ToCapitalAccount", DbType.AnsiString, model.ToCapitalAccount);
            db.AddInParameter(dbCommand, "TransferAmount", DbType.Decimal, model.TransferAmount);
            db.AddInParameter(dbCommand, "TransferTime", DbType.DateTime, model.TransferTime);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32, model.TransferTypeLogo);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CapitalFlowLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UA_CapitalFlowTable ");
            strSql.Append(" where CapitalFlowLogo=@CapitalFlowLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFlowLogo", DbType.Int32, CapitalFlowLogo);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UA_CapitalFlowTableInfo GetModel(int CapitalFlowLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalFlowLogo,FromCapitalAccount,ToCapitalAccount,TransferAmount,TransferTime,TradeCurrencyType,TransferTypeLogo from UA_CapitalFlowTable ");
            strSql.Append(" where CapitalFlowLogo=@CapitalFlowLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalFlowLogo", DbType.Int32, CapitalFlowLogo);
            UA_CapitalFlowTableInfo model = null;
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
            strSql.Append(
                "select CapitalFlowLogo,FromCapitalAccount,ToCapitalAccount,TransferAmount,TransferTime,TradeCurrencyType,TransferTypeLogo ");
            strSql.Append(" FROM UA_CapitalFlowTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetStoredProcCommand("UP_GetRecordByPage");
			db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UA_CapitalFlowTable");
			db.AddInParameter(dbCommand, "fldName", DbType.AnsiString, "ID");
			db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
			db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
			db.AddInParameter(dbCommand, "IsReCount", DbType.Boolean, 0);
			db.AddInParameter(dbCommand, "OrderType", DbType.Boolean, 0);
			db.AddInParameter(dbCommand, "strWhere", DbType.AnsiString, strWhere);
			return db.ExecuteDataSet(dbCommand);
		}*/

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<UA_CapitalFlowTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalFlowLogo,FromCapitalAccount,ToCapitalAccount,TransferAmount,TransferTime,TradeCurrencyType,TransferTypeLogo ");
            strSql.Append(" FROM UA_CapitalFlowTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<UA_CapitalFlowTableInfo> list = new List<UA_CapitalFlowTableInfo>();
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
        public UA_CapitalFlowTableInfo ReaderBind(IDataReader dataReader)
        {
            UA_CapitalFlowTableInfo model = new UA_CapitalFlowTableInfo();
            object ojb;
            ojb = dataReader["CapitalFlowLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalFlowLogo = (int)ojb;
            }
            model.FromCapitalAccount = dataReader["FromCapitalAccount"].ToString();
            model.ToCapitalAccount = dataReader["ToCapitalAccount"].ToString();
            ojb = dataReader["TransferAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TransferAmount = (decimal)ojb;
            }
            ojb = dataReader["TransferTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TransferTime = (DateTime)ojb;
            }
            ojb = dataReader["TradeCurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyType = (int)ojb;
            }
            ojb = dataReader["TransferTypeLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TransferTypeLogo = (int)ojb;
            }
            return model;
        }

        ///// <summary>
        ///// 根据条件分页查询转账资金明细
        ///// </summary>
        ///// <param name="pageProcInfo">分页存储过程过滤条件</param>
        ///// <param name="total">总页数</param>
        ///// <returns></returns>
        //public List<UA_CapitalFlowTableInfo> PaginQueryUA_CapitalFlowTableByFilter(PagingProceduresInfo pageProcInfo,out int total)
        //{
        //    List<UA_CapitalFlowTableInfo> list = new List<UA_CapitalFlowTableInfo>();
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