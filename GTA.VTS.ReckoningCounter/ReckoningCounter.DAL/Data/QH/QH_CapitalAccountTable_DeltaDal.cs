#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类QH_CapitalAccountTable_DeltaDal。
    /// </summary>
    public class QH_CapitalAccountTable_DeltaDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_CapitalAccountTable_Delta where ID=@ID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
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
        public int Add(QH_CapitalAccountTable_DeltaInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_CapitalAccountTable_Delta(");
            strSql.Append(
                "CapitalAccountLogoId,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapitalDelta,MarginTotalDelta,CloseFloatProfitLossTotalDelta,CloseMarketProfitLossTotalDelta,DeltaTime)");

            strSql.Append(" values (");
            strSql.Append(
                "@CapitalAccountLogoId,@AvailableCapitalDelta,@FreezeCapitalTotalDelta,@TodayOutInCapitalDelta,@MarginTotalDelta,@CloseFloatProfitLossTotalDelta,@CloseMarketProfitLossTotalDelta,@DeltaTime)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, model.CapitalAccountLogoId);
            db.AddInParameter(dbCommand, "AvailableCapitalDelta", DbType.Decimal, model.AvailableCapitalDelta);
            db.AddInParameter(dbCommand, "FreezeCapitalTotalDelta", DbType.Decimal, model.FreezeCapitalTotalDelta);
            db.AddInParameter(dbCommand, "TodayOutInCapitalDelta", DbType.Decimal, model.TodayOutInCapitalDelta);
            db.AddInParameter(dbCommand, "MarginTotalDelta", DbType.Decimal, model.MarginTotalDelta);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotalDelta", DbType.Decimal,
                              model.CloseFloatProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotalDelta", DbType.Decimal,
                              model.CloseMarketProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "DeltaTime", DbType.DateTime, model.DeltaTime);
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
        public int Add(QH_CapitalAccountTable_DeltaInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_CapitalAccountTable_Delta(");
            strSql.Append(
                "CapitalAccountLogoId,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapitalDelta,MarginTotalDelta,CloseFloatProfitLossTotalDelta,CloseMarketProfitLossTotalDelta,DeltaTime)");

            strSql.Append(" values (");
            strSql.Append(
                "@CapitalAccountLogoId,@AvailableCapitalDelta,@FreezeCapitalTotalDelta,@TodayOutInCapitalDelta,@MarginTotalDelta,@CloseFloatProfitLossTotalDelta,@CloseMarketProfitLossTotalDelta,@DeltaTime)");
            strSql.Append(";select @@IDENTITY");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, model.CapitalAccountLogoId);
            db.AddInParameter(dbCommand, "AvailableCapitalDelta", DbType.Decimal, model.AvailableCapitalDelta);
            db.AddInParameter(dbCommand, "FreezeCapitalTotalDelta", DbType.Decimal, model.FreezeCapitalTotalDelta);
            db.AddInParameter(dbCommand, "TodayOutInCapitalDelta", DbType.Decimal, model.TodayOutInCapitalDelta);
            db.AddInParameter(dbCommand, "MarginTotalDelta", DbType.Decimal, model.MarginTotalDelta);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotalDelta", DbType.Decimal,
                              model.CloseFloatProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotalDelta", DbType.Decimal,
                              model.CloseMarketProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "DeltaTime", DbType.DateTime, model.DeltaTime);
            int result;
            object obj = db.ExecuteScalar(dbCommand,transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 得到某个资金帐号的汇总增量
        /// </summary>
        public QH_CapitalAccountTable_DeltaInfo GetSum(int capitalAccountLogoId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogoId,sum(AvailableCapitalDelta) AvailableCapitalDelta,sum(FreezeCapitalTotalDelta) FreezeCapitalTotalDelta,sum(TodayOutInCapitalDelta) TodayOutInCapitalDelta,sum(MarginTotalDelta) MarginTotalDelta,sum(CloseFloatProfitLossTotalDelta) CloseFloatProfitLossTotalDelta,sum(CloseMarketProfitLossTotalDelta) CloseMarketProfitLossTotalDelta from QH_CapitalAccountTable_Delta ");
            strSql.Append(" where CapitalAccountLogoId=@CapitalAccountLogoId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, capitalAccountLogoId);
            QH_CapitalAccountTable_DeltaInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind2(dataReader);
                }
            }
            return model;
        }

        /// <summary>
        /// 得到所有汇总增量（以资金帐号区分）
        /// </summary>
        public List<QH_CapitalAccountTable_DeltaInfo> GetAllSum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogoId,sum(AvailableCapitalDelta) AvailableCapitalDelta,sum(FreezeCapitalTotalDelta) FreezeCapitalTotalDelta,sum(TodayOutInCapitalDelta) TodayOutInCapitalDelta,sum(MarginTotalDelta) MarginTotalDelta,sum(CloseFloatProfitLossTotalDelta) CloseFloatProfitLossTotalDelta,sum(CloseMarketProfitLossTotalDelta) CloseMarketProfitLossTotalDelta from QH_CapitalAccountTable_Delta group by CapitalAccountLogoId");

            List<QH_CapitalAccountTable_DeltaInfo> list = new List<QH_CapitalAccountTable_DeltaInfo>();
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
        /// 删除全部数据
        /// </summary>
        public void Delete(Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_CapitalAccountTable_Delta ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 清空数据表
        /// </summary>
        public void Truncate()
        {
            string sql = "truncate table QH_CapitalAccountTable_Delta";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_CapitalAccountTable_DeltaInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_CapitalAccountTable_Delta set ");
            strSql.Append("CapitalAccountLogoId=@CapitalAccountLogoId,");
            strSql.Append("AvailableCapitalDelta=@AvailableCapitalDelta,");
            strSql.Append("FreezeCapitalTotalDelta=@FreezeCapitalTotalDelta,");
            strSql.Append("TodayOutInCapitalDelta=@TodayOutInCapitalDelta,");
            strSql.Append("MarginTotalDelta=@MarginTotalDelta,");
            strSql.Append("CloseFloatProfitLossTotalDelta=@CloseFloatProfitLossTotalDelta,");
            strSql.Append("CloseMarketProfitLossTotalDelta=@CloseMarketProfitLossTotalDelta,");
            strSql.Append("DeltaTime=@DeltaTime");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
            db.AddInParameter(dbCommand, "CapitalAccountLogoId", DbType.Int32, model.CapitalAccountLogoId);
            db.AddInParameter(dbCommand, "AvailableCapitalDelta", DbType.Decimal, model.AvailableCapitalDelta);
            db.AddInParameter(dbCommand, "FreezeCapitalTotalDelta", DbType.Decimal, model.FreezeCapitalTotalDelta);
            db.AddInParameter(dbCommand, "TodayOutInCapitalDelta", DbType.Decimal, model.TodayOutInCapitalDelta);
            db.AddInParameter(dbCommand, "MarginTotalDelta", DbType.Decimal, model.MarginTotalDelta);
            db.AddInParameter(dbCommand, "CloseFloatProfitLossTotalDelta", DbType.Decimal,
                              model.CloseFloatProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "CloseMarketProfitLossTotalDelta", DbType.Decimal,
                              model.CloseMarketProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "DeltaTime", DbType.DateTime, model.DeltaTime);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_CapitalAccountTable_Delta ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_CapitalAccountTable_DeltaInfo GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select ID,CapitalAccountLogoId,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapitalDelta,MarginTotalDelta,CloseFloatProfitLossTotalDelta,CloseMarketProfitLossTotalDelta,DeltaTime from QH_CapitalAccountTable_Delta ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            QH_CapitalAccountTable_DeltaInfo model = null;
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
                "select ID,CapitalAccountLogoId,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapitalDelta,MarginTotalDelta,CloseFloatProfitLossTotalDelta,CloseMarketProfitLossTotalDelta,DeltaTime ");
            strSql.Append(" FROM QH_CapitalAccountTable_Delta ");
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
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "QH_CapitalAccountTable_Delta");
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
        public List<QH_CapitalAccountTable_DeltaInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select ID,CapitalAccountLogoId,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapitalDelta,MarginTotalDelta,CloseFloatProfitLossTotalDelta,CloseMarketProfitLossTotalDelta,DeltaTime ");
            strSql.Append(" FROM QH_CapitalAccountTable_Delta ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_CapitalAccountTable_DeltaInfo> list = new List<QH_CapitalAccountTable_DeltaInfo>();
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
        public QH_CapitalAccountTable_DeltaInfo ReaderBind(IDataReader dataReader)
        {
            QH_CapitalAccountTable_DeltaInfo model = new QH_CapitalAccountTable_DeltaInfo();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int) ojb;
            }
            ojb = dataReader["CapitalAccountLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogoId = (int) ojb;
            }
            ojb = dataReader["AvailableCapitalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapitalDelta = (decimal) ojb;
            }
            ojb = dataReader["FreezeCapitalTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapitalTotalDelta = (decimal) ojb;
            }
            ojb = dataReader["TodayOutInCapitalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayOutInCapitalDelta = (decimal) ojb;
            }
            ojb = dataReader["MarginTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarginTotalDelta = (decimal) ojb;
            }
            ojb = dataReader["CloseFloatProfitLossTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseFloatProfitLossTotalDelta = (decimal) ojb;
            }
            ojb = dataReader["CloseMarketProfitLossTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseMarketProfitLossTotalDelta = (decimal) ojb;
            }
            ojb = dataReader["DeltaTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DeltaTime = (DateTime) ojb;
            }
            return model;
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public QH_CapitalAccountTable_DeltaInfo ReaderBind2(IDataReader dataReader)
        {
            QH_CapitalAccountTable_DeltaInfo model = new QH_CapitalAccountTable_DeltaInfo();
            object ojb;
            //ojb = dataReader["ID"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.ID = (int)ojb;
            //}
            ojb = dataReader["CapitalAccountLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogoId = (int) ojb;
            }
            ojb = dataReader["AvailableCapitalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapitalDelta = (decimal) ojb;
            }
            ojb = dataReader["FreezeCapitalTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapitalTotalDelta = (decimal) ojb;
            }
            ojb = dataReader["TodayOutInCapitalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayOutInCapitalDelta = (decimal) ojb;
            }
            ojb = dataReader["MarginTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarginTotalDelta = (decimal) ojb;
            }
            ojb = dataReader["CloseFloatProfitLossTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseFloatProfitLossTotalDelta = (decimal) ojb;
            }
            ojb = dataReader["CloseMarketProfitLossTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CloseMarketProfitLossTotalDelta = (decimal) ojb;
            }
            //ojb = dataReader["DeltaTime"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.DeltaTime = (DateTime)ojb;
            //}
            return model;
        }

        #endregion  成员方法
    }
}