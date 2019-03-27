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
    /// 数据访问类BD_TradeUnitDal。
    /// </summary>
    public class BD_TradeUnitDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TradeUnitId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from BD_TradeUnit where TradeUnitId=@TradeUnitId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, TradeUnitId);
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
        public int Add(BD_TradeUnitInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into BD_TradeUnit(");
            strSql.Append("TradeUnitName)");

            strSql.Append(" values (");
            strSql.Append("@TradeUnitName)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeUnitName", DbType.AnsiString, model.TradeUnitName);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if(!int.TryParse(obj.ToString(),out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BD_TradeUnitInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("update BD_TradeUnit set ");
            strSql.Append("TradeUnitName=@TradeUnitName");
            strSql.Append(" where TradeUnitId=@TradeUnitId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, model.TradeUnitId);
            db.AddInParameter(dbCommand, "TradeUnitName", DbType.AnsiString, model.TradeUnitName);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TradeUnitId)
        {
			
            StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from BD_TradeUnit ");
            strSql.Append(" where TradeUnitId=@TradeUnitId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32,TradeUnitId);
            db.ExecuteNonQuery(dbCommand);

        }*/

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BD_TradeUnitInfo GetModel(int TradeUnitId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeUnitId,TradeUnitName from BD_TradeUnit ");
            strSql.Append(" where TradeUnitId=@TradeUnitId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeUnitId", DbType.Int32, TradeUnitId);
            BD_TradeUnitInfo model = null;
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
        public List<BD_TradeUnitInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeUnitId,TradeUnitName ");
            strSql.Append(" FROM BD_TradeUnit ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<BD_TradeUnitInfo> list = new List<BD_TradeUnitInfo>();
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
        public BD_TradeUnitInfo ReaderBind(IDataReader dataReader)
        {
            BD_TradeUnitInfo model = new BD_TradeUnitInfo();
            object ojb;
            ojb = dataReader["TradeUnitId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeUnitId = (int) ojb;
            }
            model.TradeUnitName = dataReader["TradeUnitName"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}