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
    /// 数据访问类BD_UnReckonedDealTableDal。
    /// </summary>
    public class BD_UnReckonedDealTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from BD_UnReckonedDealTable where id=@id ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "id", DbType.String, id);
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
        public void Add(BD_UnReckonedDealTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BD_UnReckonedDealTable(");
            strSql.Append("id,EntityType,OrderNo,Price,Amount,Time,Message,IsSuccess,CounterID)");

            strSql.Append(" values (");
            strSql.Append("@id,@EntityType,@OrderNo,@Price,@Amount,@Time,@Message,@IsSuccess,@CounterID)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "id", DbType.String, model.id);
            db.AddInParameter(dbCommand, "EntityType", DbType.Int32, model.EntityType);
            db.AddInParameter(dbCommand, "OrderNo", DbType.String, model.OrderNo);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, model.Price);
            db.AddInParameter(dbCommand, "Amount", DbType.Int32, model.Amount);
            db.AddInParameter(dbCommand, "Time", DbType.DateTime, model.Time);
            db.AddInParameter(dbCommand, "Message", DbType.String, model.Message);
            db.AddInParameter(dbCommand, "IsSuccess", DbType.Boolean, model.IsSuccess);
            db.AddInParameter(dbCommand, "CounterID", DbType.String, model.CounterID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BD_UnReckonedDealTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BD_UnReckonedDealTable set ");
            strSql.Append("EntityType=@EntityType,");
            strSql.Append("OrderNo=@OrderNo,");
            strSql.Append("Price=@Price,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("Time=@Time,");
            strSql.Append("Message=@Message,");
            strSql.Append("IsSuccess=@IsSuccess,");
            strSql.Append("CounterID=@CounterID");
            strSql.Append(" where id=@id ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "id", DbType.String, model.id);
            db.AddInParameter(dbCommand, "EntityType", DbType.Int32, model.EntityType);
            db.AddInParameter(dbCommand, "OrderNo", DbType.String, model.OrderNo);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, model.Price);
            db.AddInParameter(dbCommand, "Amount", DbType.Int32, model.Amount);
            db.AddInParameter(dbCommand, "Time", DbType.DateTime, model.Time);
            db.AddInParameter(dbCommand, "Message", DbType.String, model.Message);
            db.AddInParameter(dbCommand, "IsSuccess", DbType.Boolean, model.IsSuccess);
            db.AddInParameter(dbCommand, "CounterID", DbType.String, model.CounterID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BD_UnReckonedDealTable ");
            strSql.Append(" where id=@id ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "id", DbType.String, id);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据(带事务）
        /// </summary>
        public void Delete(string id,Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BD_UnReckonedDealTable ");
            strSql.Append(" where id=@id ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "id", DbType.String, id);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BD_UnReckonedDealTableInfo GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select id,EntityType,OrderNo,Price,Amount,Time,Message,IsSuccess,CounterID from BD_UnReckonedDealTable ");
            strSql.Append(" where id=@id ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "id", DbType.String, id);
            BD_UnReckonedDealTableInfo model = null;
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
        public List<BD_UnReckonedDealTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,EntityType,OrderNo,Price,Amount,Time,Message,IsSuccess,CounterID ");
            strSql.Append(" FROM BD_UnReckonedDealTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<BD_UnReckonedDealTableInfo> list = new List<BD_UnReckonedDealTableInfo>();
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
        public BD_UnReckonedDealTableInfo ReaderBind(IDataReader dataReader)
        {
            BD_UnReckonedDealTableInfo model = new BD_UnReckonedDealTableInfo();
            object ojb;
            model.id = dataReader["id"].ToString();
            ojb = dataReader["EntityType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EntityType = (int) ojb;
            }
            model.OrderNo = dataReader["OrderNo"].ToString();
            ojb = dataReader["Price"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Price = (decimal) ojb;
            }
            ojb = dataReader["Amount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Amount = (int) ojb;
            }
            ojb = dataReader["Time"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Time = (DateTime) ojb;
            }
            model.Message = dataReader["Message"].ToString();
            ojb = dataReader["IsSuccess"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSuccess = (bool) ojb;
            }
            model.CounterID = dataReader["CounterID"].ToString();
            return model;
        }

        #endregion  成员方法
    }
}