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
    ///描述：交易货币类型 数据访问类CM_CurrencyType。
    ///作者：刘书伟
    ///日期:2008-11-20
    /// </summary>
    public class CM_CurrencyTypeDAL
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CM_CurrencyTypeDAL()
        { }
        #endregion
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(CurrencyTypeID)+1 from CM_CurrencyType";
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
        public bool Exists(int CurrencyTypeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CM_CurrencyType where CurrencyTypeID=@CurrencyTypeID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, CurrencyTypeID);
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
        public void Add(ManagementCenter.Model.CM_CurrencyType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CM_CurrencyType(");
            strSql.Append("CurrencyTypeID,CurrencyName)");

            strSql.Append(" values (");
            strSql.Append("@CurrencyTypeID,@CurrencyName)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            db.AddInParameter(dbCommand, "CurrencyName", DbType.String, model.CurrencyName);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CM_CurrencyType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CM_CurrencyType set ");
            strSql.Append("CurrencyName=@CurrencyName");
            strSql.Append(" where CurrencyTypeID=@CurrencyTypeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, model.CurrencyTypeID);
            db.AddInParameter(dbCommand, "CurrencyName", DbType.String, model.CurrencyName);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CurrencyTypeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete CM_CurrencyType ");
            strSql.Append(" where CurrencyTypeID=@CurrencyTypeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, CurrencyTypeID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_CurrencyType GetModel(int CurrencyTypeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CurrencyTypeID,CurrencyName from CM_CurrencyType ");
            strSql.Append(" where CurrencyTypeID=@CurrencyTypeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CurrencyTypeID", DbType.Int32, CurrencyTypeID);
            ManagementCenter.Model.CM_CurrencyType model = null;
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
            strSql.Append("select CurrencyTypeID,CurrencyName ");
            strSql.Append(" FROM CM_CurrencyType ");
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
        public List<ManagementCenter.Model.CM_CurrencyType> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CurrencyTypeID,CurrencyName ");
            strSql.Append(" FROM CM_CurrencyType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.CM_CurrencyType> list = new List<ManagementCenter.Model.CM_CurrencyType>();
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
        public ManagementCenter.Model.CM_CurrencyType ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.CM_CurrencyType model = new ManagementCenter.Model.CM_CurrencyType();
            object ojb;
            ojb = dataReader["CurrencyTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeID = (int)ojb;
            }
            model.CurrencyName = dataReader["CurrencyName"].ToString();
            return model;
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.CM_CurrencyBreedClassType _ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.CM_CurrencyBreedClassType model = new ManagementCenter.Model.CM_CurrencyBreedClassType();
            object ojb;
            ojb = dataReader["CurrencyTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeID = (int)ojb;
            }
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            return model;
        }

        /// <summary>
        /// 根据品种ID获取当前品种的币种类型
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.CM_CurrencyBreedClassType GetCurrencyByBreedClassID(int BreedClassID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("CurrencyTypeIDPrc");
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);

            ManagementCenter.Model.CM_CurrencyBreedClassType model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = _ReaderBind(dataReader);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取当前品种的币种类型
        /// </summary>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_CurrencyBreedClassType> GetListCurrencyBreedClass()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("CurrencyTypeIDPrc");
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, -100);

            List<ManagementCenter.Model.CM_CurrencyBreedClassType> list = new List<ManagementCenter.Model.CM_CurrencyBreedClassType>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(_ReaderBind(dataReader));
                }
            }
            return list;
        }


        #endregion  成员方法
    }
}

