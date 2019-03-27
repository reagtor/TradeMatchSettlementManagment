using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用
namespace ManagementCenter.DAL
{
    /// <summary>
    /// 描述：管理员组 数据访问类UM_ManagerGroupDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public class UM_ManagerGroupDAL
    {
        public UM_ManagerGroupDAL()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(ManagerGroupID)+1 from UM_ManagerGroup";
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
        public bool Exists(int ManagerGroupID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UM_ManagerGroup where ManagerGroupID=@ManagerGroupID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, ManagerGroupID);
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
        public int Add(ManagementCenter.Model.UM_ManagerGroup model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UM_ManagerGroup(");
            strSql.Append("ManagerGroupName)");

            strSql.Append(" values (");
            strSql.Append("@ManagerGroupName)");
            strSql.Append(";select @@IDENTITY");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ManagerGroupName", DbType.String, model.ManagerGroupName);
            int result;
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand, tran);
            }
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 添加管理员组
        /// </summary>
        /// <param name="model">管理员组实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.UM_ManagerGroup model)
        {
            return Add(model, null, null);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.UM_ManagerGroup model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UM_ManagerGroup set ");
            strSql.Append("ManagerGroupName=@ManagerGroupName");
            strSql.Append(" where ManagerGroupID=@ManagerGroupID ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, model.ManagerGroupID);
            db.AddInParameter(dbCommand, "ManagerGroupName", DbType.String, model.ManagerGroupName);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }
        /// <summary>
        /// 更新管理员组
        /// </summary>
        /// <param name="model">管理员组实体</param>
        public void Update(ManagementCenter.Model.UM_ManagerGroup model)
        {
            Update(model, null, null);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ManagerGroupID, DbTransaction tran, Database db)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_ManagerGroup ");
            strSql.Append(" where ManagerGroupID=@ManagerGroupID ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, ManagerGroupID);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }
        /// <summary>
        /// 根据管理员组组ID删除
        /// </summary>
        /// <param name="ManagerGroupID">管理员组ID</param>
        public void Delete(int ManagerGroupID)
        {
            Delete(ManagerGroupID, null, null);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_ManagerGroup GetModel(int ManagerGroupID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ManagerGroupID,ManagerGroupName from UM_ManagerGroup ");
            strSql.Append(" where ManagerGroupID=@ManagerGroupID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, ManagerGroupID);
            ManagementCenter.Model.UM_ManagerGroup model = null;
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
            strSql.Append("select ManagerGroupID,ManagerGroupName ");
            strSql.Append(" FROM UM_ManagerGroup ");
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
        public List<ManagementCenter.Model.UM_ManagerGroup> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ManagerGroupID,ManagerGroupName ");
            strSql.Append(" FROM UM_ManagerGroup ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.UM_ManagerGroup> list = new List<ManagementCenter.Model.UM_ManagerGroup>();
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
        public ManagementCenter.Model.UM_ManagerGroup ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.UM_ManagerGroup model = new ManagementCenter.Model.UM_ManagerGroup();
            object ojb;
            ojb = dataReader["ManagerGroupID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ManagerGroupID = (int)ojb;
            }
            model.ManagerGroupName = dataReader["ManagerGroupName"].ToString();
            return model;
        }

        #endregion  成员方法


        #region 分页查询权限组

        /// <summary>
        /// 分页查询权限组
        /// </summary>
        /// <param name="strwhere">条件</param>
        /// <param name="pageNo">第几页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rowCount">总记录数</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetPagingManagerGroup(string strwhere, int pageNo, int pageSize, out int rowCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ManagerGroupID,ManagerGroupName ");
            strSql.Append(" FROM UM_ManagerGroup ");
            if (strwhere.Trim() != "")
            {
                strSql.Append(" where " + strwhere);
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(strSql.ToString());

            return CommPager.QueryPager(database, dbCommand, strSql.ToString(), pageNo, pageSize, out rowCount, "UM_ManagerGroup");

        }
        #endregion

    }
}

