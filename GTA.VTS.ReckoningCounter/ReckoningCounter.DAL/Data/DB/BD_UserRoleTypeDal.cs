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
    /// 数据访问类BD_UserRoleTypeDal。
    /// </summary>
    public class BD_UserRoleTypeDal
    {
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(RoleNumber)+1 from BD_UserRoleType";
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
        public bool Exists(int RoleNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from BD_UserRoleType where RoleNumber=@RoleNumber ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "RoleNumber", DbType.Int32, RoleNumber);
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

        /*
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BD_UserRoleTypeInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into BD_UserRoleType(");
            strSql.Append("RoleNumber,RoleName,Remarks)");

            strSql.Append(" values (");
            strSql.Append("@RoleNumber,@RoleName,@Remarks)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "RoleNumber", DbType.Int32, model.RoleNumber);
            db.AddInParameter(dbCommand, "RoleName", DbType.AnsiString, model.RoleName);
            db.AddInParameter(dbCommand, "Remarks", DbType.AnsiString, model.Remarks);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BD_UserRoleTypeInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("update BD_UserRoleType set ");
            strSql.Append("RoleName=@RoleName,");
            strSql.Append("Remarks=@Remarks");
            strSql.Append(" where RoleNumber=@RoleNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "RoleNumber", DbType.Int32, model.RoleNumber);
            db.AddInParameter(dbCommand, "RoleName", DbType.AnsiString, model.RoleName);
            db.AddInParameter(dbCommand, "Remarks", DbType.AnsiString, model.Remarks);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int RoleNumber)
        {
			
            StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from BD_UserRoleType ");
            strSql.Append(" where RoleNumber=@RoleNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "RoleNumber", DbType.Int32,RoleNumber);
            db.ExecuteNonQuery(dbCommand);

        }*/

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BD_UserRoleTypeInfo GetModel(int RoleNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RoleNumber,RoleName,Remarks from BD_UserRoleType ");
            strSql.Append(" where RoleNumber=@RoleNumber ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "RoleNumber", DbType.Int32, RoleNumber);
            BD_UserRoleTypeInfo model = null;
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
        public List<BD_UserRoleTypeInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RoleNumber,RoleName,Remarks ");
            strSql.Append(" FROM BD_UserRoleType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<BD_UserRoleTypeInfo> list = new List<BD_UserRoleTypeInfo>();
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
        public BD_UserRoleTypeInfo ReaderBind(IDataReader dataReader)
        {
            BD_UserRoleTypeInfo model = new BD_UserRoleTypeInfo();
            object ojb;
            ojb = dataReader["RoleNumber"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RoleNumber = (int) ojb;
            }
            model.RoleName = dataReader["RoleName"].ToString();
            model.Remarks = dataReader["Remarks"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}