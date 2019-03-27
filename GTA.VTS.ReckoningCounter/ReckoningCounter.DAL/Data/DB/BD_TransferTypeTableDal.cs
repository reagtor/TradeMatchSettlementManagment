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
    /// 数据访问类BD_TransferTypeTableDal。
    /// </summary>
    public class BD_TransferTypeTableDal
    {
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(TransferTypeLogo)+1 from BD_TransferTypeTable";
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
        public bool Exists(int TransferTypeLogo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from BD_TransferTypeTable where TransferTypeLogo=@TransferTypeLogo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32, TransferTypeLogo);
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
        public void Add(BD_TransferTypeTableInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into BD_TransferTypeTable(");
            strSql.Append("TransferTypeLogo,TransferTypeName,Remarks)");

            strSql.Append(" values (");
            strSql.Append("@TransferTypeLogo,@TransferTypeName,@Remarks)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32, model.TransferTypeLogo);
            db.AddInParameter(dbCommand, "TransferTypeName", DbType.AnsiString, model.TransferTypeName);
            db.AddInParameter(dbCommand, "Remarks", DbType.AnsiString, model.Remarks);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BD_TransferTypeTableInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("update BD_TransferTypeTable set ");
            strSql.Append("TransferTypeName=@TransferTypeName,");
            strSql.Append("Remarks=@Remarks");
            strSql.Append(" where TransferTypeLogo=@TransferTypeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32, model.TransferTypeLogo);
            db.AddInParameter(dbCommand, "TransferTypeName", DbType.AnsiString, model.TransferTypeName);
            db.AddInParameter(dbCommand, "Remarks", DbType.AnsiString, model.Remarks);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TransferTypeLogo)
        {
			
            StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from BD_TransferTypeTable ");
            strSql.Append(" where TransferTypeLogo=@TransferTypeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32,TransferTypeLogo);
            db.ExecuteNonQuery(dbCommand);

        }*/

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BD_TransferTypeTableInfo GetModel(int TransferTypeLogo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TransferTypeLogo,TransferTypeName,Remarks from BD_TransferTypeTable ");
            strSql.Append(" where TransferTypeLogo=@TransferTypeLogo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TransferTypeLogo", DbType.Int32, TransferTypeLogo);
            BD_TransferTypeTableInfo model = null;
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
        public List<BD_TransferTypeTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TransferTypeLogo,TransferTypeName,Remarks ");
            strSql.Append(" FROM BD_TransferTypeTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<BD_TransferTypeTableInfo> list = new List<BD_TransferTypeTableInfo>();
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
        public BD_TransferTypeTableInfo ReaderBind(IDataReader dataReader)
        {
            BD_TransferTypeTableInfo model = new BD_TransferTypeTableInfo();
            object ojb;
            ojb = dataReader["TransferTypeLogo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TransferTypeLogo = (int) ojb;
            }
            model.TransferTypeName = dataReader["TransferTypeName"].ToString();
            model.Remarks = dataReader["Remarks"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}