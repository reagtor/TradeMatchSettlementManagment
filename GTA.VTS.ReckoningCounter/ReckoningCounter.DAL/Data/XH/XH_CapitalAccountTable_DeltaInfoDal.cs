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
    /// 数据访问类XH_CapitalAccountTable_DeltaInfoDal。
    /// </summary>
    public class XH_CapitalAccountTable_DeltaInfoDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_CapitalAccountTable_Delta where ID=@ID ");
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
        public int Add(XH_CapitalAccountTable_DeltaInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_CapitalAccountTable_Delta(");
            strSql.Append("CapitalAccountLogo,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapital,HasDoneProfitLossTotalDelta,DeltaTime)");

            strSql.Append(" values (");
            strSql.Append("@CapitalAccountLogo,@AvailableCapitalDelta,@FreezeCapitalTotalDelta,@TodayOutInCapital,@HasDoneProfitLossTotalDelta,@DeltaTime)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "AvailableCapitalDelta", DbType.Decimal, model.AvailableCapitalDelta);
            db.AddInParameter(dbCommand, "FreezeCapitalTotalDelta", DbType.Decimal, model.FreezeCapitalTotalDelta);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotalDelta", DbType.Decimal, model.HasDoneProfitLossTotalDelta);
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
        public int Add(XH_CapitalAccountTable_DeltaInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_CapitalAccountTable_Delta(");
            strSql.Append("CapitalAccountLogo,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapital,HasDoneProfitLossTotalDelta,DeltaTime)");

            strSql.Append(" values (");
            strSql.Append("@CapitalAccountLogo,@AvailableCapitalDelta,@FreezeCapitalTotalDelta,@TodayOutInCapital,@HasDoneProfitLossTotalDelta,@DeltaTime)");
            strSql.Append(";select @@IDENTITY");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, model.CapitalAccountLogo);
            db.AddInParameter(dbCommand, "AvailableCapitalDelta", DbType.Decimal, model.AvailableCapitalDelta);
            db.AddInParameter(dbCommand, "FreezeCapitalTotalDelta", DbType.Decimal, model.FreezeCapitalTotalDelta);
            db.AddInParameter(dbCommand, "TodayOutInCapital", DbType.Decimal, model.TodayOutInCapital);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotalDelta", DbType.Decimal, model.HasDoneProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "DeltaTime", DbType.DateTime, model.DeltaTime);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 得到某个资金帐号的汇总增量
        /// </summary>
        public XH_CapitalAccountTable_DeltaInfo GetSum(int capitalAccountLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogo,sum(AvailableCapitalDelta) AvailableCapitalDelta,sum(FreezeCapitalTotalDelta) FreezeCapitalTotalDelta,sum(TodayOutInCapital) TodayOutInCapital,sum(HasDoneProfitLossTotalDelta) HasDoneProfitLossTotalDelta from XH_CapitalAccountTable_Delta ");
            strSql.Append(" where CapitalAccountLogo=@CapitalAccountLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CapitalAccountLogo", DbType.Int32, capitalAccountLogo);
            XH_CapitalAccountTable_DeltaInfo model = null;
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
        public List<XH_CapitalAccountTable_DeltaInfo> GetAllSum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select CapitalAccountLogo,sum(AvailableCapitalDelta) AvailableCapitalDelta,sum(FreezeCapitalTotalDelta) FreezeCapitalTotalDelta,sum(TodayOutInCapital) TodayOutInCapital,sum(HasDoneProfitLossTotalDelta) HasDoneProfitLossTotalDelta from XH_CapitalAccountTable_Delta group by CapitalAccountLogo");

            List<XH_CapitalAccountTable_DeltaInfo> list = new List<XH_CapitalAccountTable_DeltaInfo>();
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
            strSql.Append("delete from XH_CapitalAccountTable_Delta ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 清空数据表
        /// </summary>
        public void Truncate()
        {
            string sql = "truncate table XH_CapitalAccountTable_Delta";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(dbCommand);
        }

        /*/// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(XH_CapitalAccountTable_DeltaInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_CapitalAccountTable_Delta set ");
            strSql.Append("AvailableCapitalDelta=@AvailableCapitalDelta,");
            strSql.Append("FreezeCapitalTotalDelta=@FreezeCapitalTotalDelta,");
            strSql.Append("HasDoneProfitLossTotalDelta=@HasDoneProfitLossTotalDelta,");
            strSql.Append("DeltaTime=@DeltaTime");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
            db.AddInParameter(dbCommand, "AvailableCapitalDelta", DbType.Decimal, model.AvailableCapitalDelta);
            db.AddInParameter(dbCommand, "FreezeCapitalTotalDelta", DbType.Decimal, model.FreezeCapitalTotalDelta);
            db.AddInParameter(dbCommand, "HasDoneProfitLossTotalDelta", DbType.Decimal,
                              model.HasDoneProfitLossTotalDelta);
            db.AddInParameter(dbCommand, "DeltaTime", DbType.DateTime, model.DeltaTime);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_CapitalAccountTable_Delta ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            db.ExecuteNonQuery(dbCommand);
        }
        */
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_CapitalAccountTable_DeltaInfo GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select ID,CapitalAccountLogo,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapital,HasDoneProfitLossTotalDelta,DeltaTime from XH_CapitalAccountTable_Delta ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            XH_CapitalAccountTable_DeltaInfo model = null;
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
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<XH_CapitalAccountTable_DeltaInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select ID,CapitalAccountLogo,AvailableCapitalDelta,FreezeCapitalTotalDelta,TodayOutInCapital,HasDoneProfitLossTotalDelta,DeltaTime ");
            strSql.Append(" FROM XH_CapitalAccountTable_Delta ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_CapitalAccountTable_DeltaInfo> list = new List<XH_CapitalAccountTable_DeltaInfo>();
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
        public XH_CapitalAccountTable_DeltaInfo ReaderBind(IDataReader dataReader)
        {
            XH_CapitalAccountTable_DeltaInfo model = new XH_CapitalAccountTable_DeltaInfo();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["CapitalAccountLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogo = (int)ojb;
            }
            ojb = dataReader["AvailableCapitalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapitalDelta = (decimal)ojb;
            }
            ojb = dataReader["FreezeCapitalTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapitalTotalDelta = (decimal)ojb;
            }
            ojb = dataReader["TodayOutInCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayOutInCapital = (decimal)ojb;
            }
            ojb = dataReader["HasDoneProfitLossTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HasDoneProfitLossTotalDelta = (decimal)ojb;
            }
            ojb = dataReader["DeltaTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DeltaTime = (DateTime)ojb;
            }
            return model;
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public XH_CapitalAccountTable_DeltaInfo ReaderBind2(IDataReader dataReader)
        {
            XH_CapitalAccountTable_DeltaInfo model = new XH_CapitalAccountTable_DeltaInfo();
            object ojb;
            //ojb = dataReader["ID"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.ID = (int)ojb;
            //}
            ojb = dataReader["CapitalAccountLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CapitalAccountLogo = (int)ojb;
            }
            ojb = dataReader["AvailableCapitalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableCapitalDelta = (decimal)ojb;
            }
            ojb = dataReader["FreezeCapitalTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeCapitalTotalDelta = (decimal)ojb;
            }
            ojb = dataReader["TodayOutInCapital"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TodayOutInCapital = (decimal)ojb;
            }
            ojb = dataReader["HasDoneProfitLossTotalDelta"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HasDoneProfitLossTotalDelta = (decimal)ojb;
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