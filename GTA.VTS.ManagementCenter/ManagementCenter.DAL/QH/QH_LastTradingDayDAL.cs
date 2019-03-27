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
    ///描述：最后交易日 数据访问类QH_LastTradingDayDAL。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_LastTradingDayDAL
    {
        public QH_LastTradingDayDAL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(LastTradingDayID)+1 from QH_LastTradingDay";
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
        public bool Exists(int LastTradingDayID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_LastTradingDay where LastTradingDayID=@LastTradingDayID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "LastTradingDayID", DbType.Int32, LastTradingDayID);
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

        #region  添加最后交易日

        /// <summary>
        /// 添加最后交易日
        /// </summary>
        /// <param name="model">最后交易日实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.QH_LastTradingDay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_LastTradingDay(");
            strSql.Append("WhatDay,WhatWeek,Week,Sequence,LastTradingDayTypeID)");

            strSql.Append(" values (");
            strSql.Append("@WhatDay,@WhatWeek,@Week,@Sequence,@LastTradingDayTypeID)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "WhatDay", DbType.Int32, model.WhatDay);
            db.AddInParameter(dbCommand, "WhatWeek", DbType.Int32, model.WhatWeek);
            db.AddInParameter(dbCommand, "Week", DbType.Int32, model.Week);
            db.AddInParameter(dbCommand, "Sequence", DbType.Int32, model.Sequence);
            db.AddInParameter(dbCommand, "LastTradingDayTypeID", DbType.Int32, model.LastTradingDayTypeID);
            int result;
            object obj = db.ExecuteScalar(dbCommand);

            if (!int.TryParse(obj.ToString(), out result))
            {
                return AppGlobalVariable.INIT_INT;
            }
            return result;
        }

        #endregion

        #region 更新最后交易日

        /// <summary>
        /// 更新最后交易日
        /// </summary>
        /// <param name="model">最后交易日实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.QH_LastTradingDay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_LastTradingDay set ");
            strSql.Append("WhatDay=@WhatDay,");
            strSql.Append("WhatWeek=@WhatWeek,");
            strSql.Append("Week=@Week,");
            strSql.Append("Sequence=@Sequence,");
            strSql.Append("LastTradingDayTypeID=@LastTradingDayTypeID");
            strSql.Append(" where LastTradingDayID=@LastTradingDayID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "LastTradingDayID", DbType.Int32, model.LastTradingDayID);
            db.AddInParameter(dbCommand, "WhatDay", DbType.Int32, model.WhatDay);
            db.AddInParameter(dbCommand, "WhatWeek", DbType.Int32, model.WhatWeek);
            db.AddInParameter(dbCommand, "Week", DbType.Int32, model.Week);
            db.AddInParameter(dbCommand, "Sequence", DbType.Int32, model.Sequence);
            db.AddInParameter(dbCommand, "LastTradingDayTypeID", DbType.Int32, model.LastTradingDayTypeID);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #endregion

        #region 根据最后交易日ID，删除最后交易日

        /// <summary>
        /// 根据最后交易日ID，删除最后交易日
        /// </summary>
        /// <param name="LastTradingDayID">最后交易日ID</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int LastTradingDayID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_LastTradingDay ");
            strSql.Append(" where LastTradingDayID=@LastTradingDayID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "LastTradingDayID", DbType.Int32, LastTradingDayID);
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

        #region 根据最后交易日ID，删除最后交易日

        /// <summary>
        /// 根据最后交易日ID，删除最后交易日
        /// </summary>
        /// <param name="LastTradingDayID">最后交易日ID</param>
        /// <returns></returns>
        public bool Delete(int LastTradingDayID)
        {
            return Delete(LastTradingDayID, null, null);
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_LastTradingDay GetModel(int LastTradingDayID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select LastTradingDayID,WhatDay,WhatWeek,Week,Sequence,LastTradingDayTypeID from QH_LastTradingDay ");
            strSql.Append(" where LastTradingDayID=@LastTradingDayID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "LastTradingDayID", DbType.Int32, LastTradingDayID);
            ManagementCenter.Model.QH_LastTradingDay model = null;
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
            strSql.Append("select LastTradingDayID,WhatDay,WhatWeek,Week,Sequence,LastTradingDayTypeID ");
            strSql.Append(" FROM QH_LastTradingDay ");
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
        public List<ManagementCenter.Model.QH_LastTradingDay> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select LastTradingDayID,WhatDay,WhatWeek,Week,Sequence,LastTradingDayTypeID ");
            strSql.Append(" FROM QH_LastTradingDay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.QH_LastTradingDay> list = new List<ManagementCenter.Model.QH_LastTradingDay>();
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
        public ManagementCenter.Model.QH_LastTradingDay ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.QH_LastTradingDay model = new ManagementCenter.Model.QH_LastTradingDay();
            object ojb;
            ojb = dataReader["LastTradingDayID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LastTradingDayID = (int) ojb;
            }
            ojb = dataReader["WhatDay"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WhatDay = (int) ojb;
            }
            ojb = dataReader["WhatWeek"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WhatWeek = (int) ojb;
            }
            ojb = dataReader["Week"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Week = (int) ojb;
            }
            ojb = dataReader["Sequence"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Sequence = (int) ojb;
            }
            ojb = dataReader["LastTradingDayTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LastTradingDayTypeID = (int) ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}