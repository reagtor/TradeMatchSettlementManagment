using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用
namespace ManagementCenter.DAL
{
    /// <summary>
    /// 描述:撮合中心表 数据访问类RC_MatchCenter。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public class RC_MatchCenterDAL
    {
        public RC_MatchCenterDAL()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(MatchCenterID)+1 from RC_MatchCenter";
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
        public bool Exists(int MatchCenterID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from RC_MatchCenter where MatchCenterID=@MatchCenterID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchCenterID", DbType.Int32, MatchCenterID);
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
        /// </summary>
        public int Add(ManagementCenter.Model.RC_MatchCenter model,DbTransaction tran,Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into RC_MatchCenter(");
            strSql.Append("MatchCenterName,IP,Port,State,XiaDanService,CuoHeService)");

            strSql.Append(" values (");
            strSql.Append("@MatchCenterName,@IP,@Port,@State,@XiaDanService,@CuoHeService)");
            strSql.Append(";select @@IDENTITY");
            if(db==null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchCenterName", DbType.String, model.MatchCenterName);
            db.AddInParameter(dbCommand, "IP", DbType.String, model.IP);
            db.AddInParameter(dbCommand, "Port", DbType.Int32, model.Port);
            db.AddInParameter(dbCommand, "State", DbType.Byte, model.State);
            db.AddInParameter(dbCommand, "XiaDanService", DbType.String, model.XiaDanService);
            db.AddInParameter(dbCommand, "CuoHeService", DbType.String, model.CuoHeService);
            int result;
            object obj;
            if(tran==null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand,tran);
            }
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 添加撮合中心
        /// </summary>
        /// <param name="model">撮合中心实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.RC_MatchCenter model)
        {
            return Add(model, null, null);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.RC_MatchCenter model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RC_MatchCenter set ");
            strSql.Append("MatchCenterName=@MatchCenterName,");
            strSql.Append("IP=@IP,");
            strSql.Append("Port=@Port,");
            strSql.Append("State=@State,");
            strSql.Append("XiaDanService=@XiaDanService,");
            strSql.Append("CuoHeService=@CuoHeService");
            strSql.Append(" where MatchCenterID=@MatchCenterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchCenterID", DbType.Int32, model.MatchCenterID);
            db.AddInParameter(dbCommand, "MatchCenterName", DbType.String, model.MatchCenterName);
            db.AddInParameter(dbCommand, "IP", DbType.String, model.IP);
            db.AddInParameter(dbCommand, "Port", DbType.Int32, model.Port);
            db.AddInParameter(dbCommand, "State", DbType.Byte, model.State);
            db.AddInParameter(dbCommand, "XiaDanService", DbType.String, model.XiaDanService);
            db.AddInParameter(dbCommand, "CuoHeService", DbType.String, model.CuoHeService);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int MatchCenterID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete RC_MatchCenter ");
            strSql.Append(" where MatchCenterID=@MatchCenterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchCenterID", DbType.Int32, MatchCenterID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public void DeleteAll(DbTransaction tran,Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete RC_MatchCenter ");
            if(db==null) db= DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            if(tran==null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                 db.ExecuteNonQuery(dbCommand,tran);
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.RC_MatchCenter GetModel(int MatchCenterID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MatchCenterID,MatchCenterName,IP,Port,State,XiaDanService,CuoHeService from RC_MatchCenter ");
            strSql.Append(" where MatchCenterID=@MatchCenterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MatchCenterID", DbType.Int32, MatchCenterID);
            ManagementCenter.Model.RC_MatchCenter model = null;
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
            strSql.Append("select MatchCenterID,MatchCenterName,IP,Port,State,XiaDanService,CuoHeService ");
            strSql.Append(" FROM RC_MatchCenter ");
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
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "RC_MatchCenter");
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
        public List<ManagementCenter.Model.RC_MatchCenter> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MatchCenterID,MatchCenterName,IP,Port,State,XiaDanService,CuoHeService ");
            strSql.Append(" FROM RC_MatchCenter ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.RC_MatchCenter> list = new List<ManagementCenter.Model.RC_MatchCenter>();
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
        public ManagementCenter.Model.RC_MatchCenter ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.RC_MatchCenter model = new ManagementCenter.Model.RC_MatchCenter();
            object ojb;
            ojb = dataReader["MatchCenterID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MatchCenterID = (int)ojb;
            }
            model.MatchCenterName = dataReader["MatchCenterName"].ToString();
            model.IP = dataReader["IP"].ToString();
            ojb = dataReader["Port"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Port = (int)ojb;
            }
            ojb = dataReader["State"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.State = (int)ojb;
            }
            model.CuoHeService = dataReader["CuoHeService"].ToString();
            model.XiaDanService = dataReader["XiaDanService"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}

