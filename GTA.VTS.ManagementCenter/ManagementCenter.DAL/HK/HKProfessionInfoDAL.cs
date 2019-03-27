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
    ///描述：港股行业 数据访问类HKProfessionInfoDAL。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HKProfessionInfoDAL
    {
        public HKProfessionInfoDAL()
        {

        }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string Nindcd)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HKProfessionInfo where Nindcd=@Nindcd ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Nindcd", DbType.String, Nindcd);
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
        public void Add(ManagementCenter.Model.HKProfessionInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HKProfessionInfo(");
            strSql.Append("Nindcd,Nindnme,EnNindnme)");

            strSql.Append(" values (");
            strSql.Append("@Nindcd,@Nindnme,@EnNindnme)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Nindcd", DbType.String, model.Nindcd);
            db.AddInParameter(dbCommand, "Nindnme", DbType.String, model.Nindnme);
            db.AddInParameter(dbCommand, "EnNindnme", DbType.String, model.EnNindnme);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.HKProfessionInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HKProfessionInfo set ");
            strSql.Append("Nindnme=@Nindnme,");
            strSql.Append("EnNindnme=@EnNindnme");
            strSql.Append(" where Nindcd=@Nindcd ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Nindcd", DbType.String, model.Nindcd);
            db.AddInParameter(dbCommand, "Nindnme", DbType.String, model.Nindnme);
            db.AddInParameter(dbCommand, "EnNindnme", DbType.String, model.EnNindnme);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string Nindcd)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HKProfessionInfo ");
            strSql.Append(" where Nindcd=@Nindcd ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Nindcd", DbType.String, Nindcd);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.HKProfessionInfo GetModel(string Nindcd)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Nindcd,Nindnme,EnNindnme from HKProfessionInfo ");
            strSql.Append(" where Nindcd=@Nindcd ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "Nindcd", DbType.String, Nindcd);
            ManagementCenter.Model.HKProfessionInfo model = null;
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
            strSql.Append("select Nindcd,Nindnme,EnNindnme ");
            strSql.Append(" FROM HKProfessionInfo ");
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
        public List<ManagementCenter.Model.HKProfessionInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Nindcd,Nindnme,EnNindnme ");
            strSql.Append(" FROM HKProfessionInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HKProfessionInfo> list = new List<ManagementCenter.Model.HKProfessionInfo>();
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
        public ManagementCenter.Model.HKProfessionInfo ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.HKProfessionInfo model = new ManagementCenter.Model.HKProfessionInfo();
            model.Nindcd = dataReader["Nindcd"].ToString();
            model.Nindnme = dataReader["Nindnme"].ToString();
            model.EnNindnme = dataReader["EnNindnme"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}
