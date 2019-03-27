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
    /// 数据访问类BD_StatusTableDal。
    /// </summary>
    public class BD_StatusTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string name)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from BD_StatusTable where name='{0}' ");
            string sql = string.Format(strSql.ToString(), name);

            DbCommand dbCommand = db.GetSqlStringCommand(sql);
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
        /// 增加一条数据（不带事务）
        /// </summary>
        public void Add(BD_StatusTableInfo model)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BD_StatusTable(");
            strSql.Append("name,value");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + model.name + "',");
            strSql.Append("'" + model.value + "'");
            strSql.Append(")");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 增加一条数据(带事务）
        /// </summary>
        public void Add(BD_StatusTableInfo model, Database db,DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BD_StatusTable(");
            strSql.Append("name,value");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + model.name + "',");
            strSql.Append("'" + model.value + "'");
            strSql.Append(")");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BD_StatusTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BD_StatusTable set ");
            strSql.Append("value='" + model.value + "'");
            strSql.Append(" where name='" + model.name + "' ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
           
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BD_StatusTable ");
            strSql.Append(" where name='{0}' ");
            string sql = string.Format(strSql.ToString(), name);

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            
            db.ExecuteNonQuery(dbCommand);
        }
        
        /// <summary>
        /// 根据名称删除记录
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="reckoningTransaction">事物</param>
        public void Delete(string name,ReckoningTransaction reckoningTransaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BD_StatusTable ");
            strSql.Append(" where name='{0}' ");
            string sql = string.Format(strSql.ToString(), name);

            Database db = reckoningTransaction.Database;
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
           
            db.ExecuteNonQuery(dbCommand, reckoningTransaction.Transaction);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BD_StatusTableInfo GetModel(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select name,value from BD_StatusTable ");
            strSql.Append(" where name='{0}' ");
            string sql = string.Format(strSql.ToString(), name);

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            BD_StatusTableInfo model = null;
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
        public List<BD_StatusTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select name,value ");
            strSql.Append(" FROM BD_StatusTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<BD_StatusTableInfo> list = new List<BD_StatusTableInfo>();
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
        public BD_StatusTableInfo ReaderBind(IDataReader dataReader)
        {
            BD_StatusTableInfo model = new BD_StatusTableInfo();
            model.name = dataReader["name"].ToString();
            model.value = dataReader["value"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}