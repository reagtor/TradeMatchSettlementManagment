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
    /// 数据访问类DB_OrderStatusDal。
    /// </summary>
    public class DB_OrderStatusDal
    {
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(OrderStatusId)+1 from DB_OrderStatus";
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
        public bool Exists(int OrderStatusId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from DB_OrderStatus where OrderStatusId=@OrderStatusId ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, OrderStatusId);
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
        public void Add(DB_OrderStatusInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into DB_OrderStatus(");
            strSql.Append("OrderStatusId,OrderStatusName)");

            strSql.Append(" values (");
            strSql.Append("@OrderStatusId,@OrderStatusName)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "OrderStatusName", DbType.AnsiString, model.OrderStatusName);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(DB_OrderStatusInfo model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("update DB_OrderStatus set ");
            strSql.Append("OrderStatusName=@OrderStatusName");
            strSql.Append(" where OrderStatusId=@OrderStatusId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, model.OrderStatusId);
            db.AddInParameter(dbCommand, "OrderStatusName", DbType.AnsiString, model.OrderStatusName);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int OrderStatusId)
        {
			
            StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from DB_OrderStatus ");
            strSql.Append(" where OrderStatusId=@OrderStatusId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32,OrderStatusId);
            db.ExecuteNonQuery(dbCommand);

        }*/

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DB_OrderStatusInfo GetModel(int OrderStatusId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OrderStatusId,OrderStatusName from DB_OrderStatus ");
            strSql.Append(" where OrderStatusId=@OrderStatusId ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderStatusId", DbType.Int32, OrderStatusId);
            DB_OrderStatusInfo model = null;
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
        public List<DB_OrderStatusInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OrderStatusId,OrderStatusName ");
            strSql.Append(" FROM DB_OrderStatus ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<DB_OrderStatusInfo> list = new List<DB_OrderStatusInfo>();
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
        public DB_OrderStatusInfo ReaderBind(IDataReader dataReader)
        {
            DB_OrderStatusInfo model = new DB_OrderStatusInfo();
            object ojb;
            ojb = dataReader["OrderStatusId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OrderStatusId = (int) ojb;
            }
            model.OrderStatusName = dataReader["OrderStatusName"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}