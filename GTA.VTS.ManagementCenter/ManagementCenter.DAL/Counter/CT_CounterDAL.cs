using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

//请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    /// 描述：柜台表 数据访问类CT_Counter。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public class CT_CounterDAL
    {
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(CouterID)+1 from CT_Counter";
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
        public bool Exists(int CouterID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            var strSql = new StringBuilder();
            strSql.Append("select count(1) from CT_Counter where CouterID=@CouterID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CouterID", DbType.Int32, CouterID);
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
        public int Add(CT_Counter model)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into CT_Counter(");
            strSql.Append(
                "name,IP,XiaDanServicePort,MaxValues,State,XiaDanServiceName,QueryServiceName,AccountServiceName,AccountServicePort,QueryServicePort,SendServiceName,SendPort)");

            strSql.Append(" values (");
            strSql.Append(
                "@name,@IP,@XiaDanServicePort,@MaxValues,@State,@XiaDanServiceName,@QueryServiceName,@AccountServiceName,@AccountServicePort,@QueryServicePort,@SendServiceName,@SendPort)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "name", DbType.String, model.name);
            db.AddInParameter(dbCommand, "IP", DbType.String, model.IP);
            db.AddInParameter(dbCommand, "XiaDanServicePort", DbType.Int32, model.XiaDanServicePort);
            db.AddInParameter(dbCommand, "MaxValues", DbType.Int32, model.MaxValues);
            db.AddInParameter(dbCommand, "State", DbType.Byte, model.State);
            db.AddInParameter(dbCommand, "XiaDanServiceName", DbType.String, model.XiaDanServiceName);
            db.AddInParameter(dbCommand, "QueryServiceName", DbType.String, model.QueryServiceName);
            db.AddInParameter(dbCommand, "AccountServiceName", DbType.String, model.AccountServiceName);
            db.AddInParameter(dbCommand, "AccountServicePort", DbType.Int32, model.AccountServicePort);
            db.AddInParameter(dbCommand, "QueryServicePort", DbType.Int32, model.QueryServicePort);
            db.AddInParameter(dbCommand, "SendServiceName", DbType.String, model.SendServiceName);
            db.AddInParameter(dbCommand, "SendPort", DbType.Int32, model.SendPort);

            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(CT_Counter model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update CT_Counter set ");
            strSql.Append("name=@name,");
            strSql.Append("IP=@IP,");
            strSql.Append("XiaDanServicePort=@XiaDanServicePort,");
            strSql.Append("MaxValues=@MaxValues,");
            strSql.Append("State=@State,");
            strSql.Append("XiaDanServiceName=@XiaDanServiceName,");
            strSql.Append("QueryServiceName=@QueryServiceName,");
            strSql.Append("AccountServiceName=@AccountServiceName,");
            strSql.Append("AccountServicePort=@AccountServicePort,");
            strSql.Append("QueryServicePort=@QueryServicePort,");
            strSql.Append("SendServiceName=@SendServiceName,");
            strSql.Append("SendPort=@SendPort ");
            strSql.Append(" where CouterID=@CouterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CouterID", DbType.Int32, model.CouterID);
            db.AddInParameter(dbCommand, "name", DbType.String, model.name);
            db.AddInParameter(dbCommand, "IP", DbType.String, model.IP);
            db.AddInParameter(dbCommand, "XiaDanServicePort", DbType.Int32, model.XiaDanServicePort);
            db.AddInParameter(dbCommand, "MaxValues", DbType.Int32, model.MaxValues);
            db.AddInParameter(dbCommand, "State", DbType.Byte, model.State);
            db.AddInParameter(dbCommand, "XiaDanServiceName", DbType.String, model.XiaDanServiceName);
            db.AddInParameter(dbCommand, "AccountServiceName", DbType.String, model.AccountServiceName);
            db.AddInParameter(dbCommand, "QueryServiceName", DbType.String, model.QueryServiceName);
            db.AddInParameter(dbCommand, "AccountServicePort", DbType.Int32, model.AccountServicePort);
            db.AddInParameter(dbCommand, "QueryServicePort", DbType.Int32, model.QueryServicePort);
            db.AddInParameter(dbCommand, "SendServiceName", DbType.String, model.SendServiceName);
            db.AddInParameter(dbCommand, "SendPort", DbType.Int32, model.SendPort);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CouterID)
        {
            var strSql = new StringBuilder();
            strSql.Append("delete CT_Counter ");
            strSql.Append(" where CouterID=@CouterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CouterID", DbType.Int32, CouterID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public CT_Counter GetModel(int CouterID)
        {
            var strSql = new StringBuilder();
            strSql.Append(
                "select CouterID,name,IP,XiaDanServicePort,MaxValues,State,XiaDanServiceName,AccountServiceName,QueryServiceName,AccountServicePort,QueryServicePort,SendServiceName,SendPort from CT_Counter ");
            strSql.Append(" where CouterID=@CouterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CouterID", DbType.Int32, CouterID);
            CT_Counter model = null;
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
            var strSql = new StringBuilder();
            strSql.Append(
                "select CouterID,name,IP,XiaDanServicePort,MaxValues,State,XiaDanServiceName,QueryServiceName,AccountServiceName,AccountServicePort,QueryServicePort,SendServiceName,SendPort ");
            strSql.Append(" FROM CT_Counter ");
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
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "CT_Counter");
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
        public List<CT_Counter> GetListArray(string strWhere)
        {
            var strSql = new StringBuilder();
            strSql.Append(
                "select CouterID,name,IP,XiaDanServicePort,MaxValues,State,XiaDanServiceName,QueryServiceName,AccountServiceName,AccountServicePort,QueryServicePort,SendServiceName,SendPort ");
            strSql.Append(" FROM CT_Counter ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            var list = new List<CT_Counter>();
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
        public CT_Counter ReaderBind(IDataReader dataReader)
        {
            var model = new CT_Counter();
            object ojb;
            ojb = dataReader["CouterID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CouterID = (int) ojb;
            }
            model.name = dataReader["name"].ToString();
            model.IP = dataReader["IP"].ToString();
            ojb = dataReader["XiaDanServicePort"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.XiaDanServicePort = (int)ojb;
            }
            ojb = dataReader["MaxValues"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MaxValues = (int) ojb;
            }
            ojb = dataReader["State"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.State = (int) ojb;
            }
            model.AccountServiceName = dataReader["AccountServiceName"].ToString();
            model.QueryServiceName = dataReader["QueryServiceName"].ToString();
            model.XiaDanServiceName = dataReader["XiaDanServiceName"].ToString();
            ojb = dataReader["AccountServicePort"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountServicePort = (int)ojb;
            }
            ojb = dataReader["QueryServicePort"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.QueryServicePort = (int) ojb;
            }
            model.SendServiceName = dataReader["SendServiceName"].ToString();

            ojb = dataReader["SendPort"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SendPort = (int) ojb;
            }
            return model;
        }

        #region 清算柜台分页查询

        /// <summary>
        /// 清算柜台分页查询
        /// </summary>
        /// <param name="counterQueryEntity">清算柜台实体</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">当前要显示的记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetPagingCounter(CT_Counter counterQueryEntity, int pageNo, int pageSize,
                                        out int rowCount)
        {
            string SQL_SELECT_Counter =
                @" select CouterID,name,IP,XiaDanServicePort,MaxValues,State,XiaDanServiceName,QueryServiceName,AccountServiceName,AccountServicePort,QueryServicePort,SendServiceName,SendPort  FROM CT_Counter where 1=1 ";

            if (counterQueryEntity.name != null && !string.IsNullOrEmpty(counterQueryEntity.name))
            {
                SQL_SELECT_Counter += "AND name LIKE  '%' + @name + '%' ";
            }
            if (counterQueryEntity.IP != null && !string.IsNullOrEmpty(counterQueryEntity.IP))
            {
                SQL_SELECT_Counter += "AND IP=@IP ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_Counter);


            if (counterQueryEntity.name != null && !string.IsNullOrEmpty(counterQueryEntity.name))
            {
                database.AddInParameter(dbCommand, "name", DbType.String, counterQueryEntity.name);
            }
            if (counterQueryEntity.IP != null && !string.IsNullOrEmpty(counterQueryEntity.IP))
            {
                database.AddInParameter(dbCommand, "IP", DbType.String, counterQueryEntity.IP);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_Counter, pageNo, pageSize, out rowCount,
                                        "UM_Counter");
        }

        #endregion

        #endregion  成员方法
    }
}