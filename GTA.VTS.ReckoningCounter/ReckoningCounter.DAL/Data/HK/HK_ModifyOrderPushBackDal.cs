using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;

namespace ReckoningCounter.DAL.Data.HK
{
    /// <summary>
    /// Title:港股改单回推数据数据操作类
    /// Create by:李健华
    /// Create Date:2009-10-30
    /// </summary>
    public class HK_ModifyOrderPushBackDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_ModifyOrderPushBackDal()
        { }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_ModifyOrderPushBack where ID=@ID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.AnsiString, ID);
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
        public void Add(HKModifyOrderPushBack model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_ModifyOrderPushBack(");
            strSql.Append("ID,TradeID,Message,IsSuccess,OriginalRequestNumber,NewRequestNumber,CallbackChannlId)");

            strSql.Append(" values (");
            strSql.Append("@ID,@TradeID,@Message,@IsSuccess,@OriginalRequestNumber,@NewRequestNumber,@CallbackChannlId)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.AnsiString, model.ID);
            db.AddInParameter(dbCommand, "TradeID", DbType.AnsiString, model.TradeID);
            db.AddInParameter(dbCommand, "Message", DbType.AnsiString, model.Message);
            db.AddInParameter(dbCommand, "IsSuccess", DbType.Boolean, model.IsSuccess);
            db.AddInParameter(dbCommand, "OriginalRequestNumber", DbType.AnsiString, model.OriginalRequestNumber);
            db.AddInParameter(dbCommand, "NewRequestNumber", DbType.AnsiString, model.NewRequestNumber);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(HKModifyOrderPushBack model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_ModifyOrderPushBack set ");
            strSql.Append("TradeID=@TradeID,");
            strSql.Append("Message=@Message,");
            strSql.Append("IsSuccess=@IsSuccess,");
            strSql.Append("OriginalRequestNumber=@OriginalRequestNumber,");
            strSql.Append("NewRequestNumber=@NewRequestNumber,");
            strSql.Append("CallbackChannlId=@CallbackChannlId");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.AnsiString, model.TradeID);
            db.AddInParameter(dbCommand, "Message", DbType.AnsiString, model.Message);
            db.AddInParameter(dbCommand, "IsSuccess", DbType.Boolean, model.IsSuccess);
            db.AddInParameter(dbCommand, "OriginalRequestNumber", DbType.AnsiString, model.OriginalRequestNumber);
            db.AddInParameter(dbCommand, "NewRequestNumber", DbType.AnsiString, model.NewRequestNumber);
            db.AddInParameter(dbCommand, "CallbackChannlId", DbType.AnsiString, model.CallbackChannlId);
            db.AddInParameter(dbCommand, "ID", DbType.AnsiString, model.ID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_ModifyOrderPushBack ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.AnsiString, ID);
            db.ExecuteNonQuery(dbCommand);

        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public HKModifyOrderPushBack GetModel(string ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,TradeID,Message,IsSuccess,OriginalRequestNumber,NewRequestNumber,CallbackChannlId from HK_ModifyOrderPushBack ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.AnsiString, ID);
            HKModifyOrderPushBack model = null;
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
            strSql.Append("select ID,TradeID,Message,IsSuccess,OriginalRequestNumber,NewRequestNumber,CallbackChannlId ");
            strSql.Append(" FROM HK_ModifyOrderPushBack ");
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
        public List<HKModifyOrderPushBack> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,TradeID,Message,IsSuccess,OriginalRequestNumber,NewRequestNumber,CallbackChannlId ");
            strSql.Append(" FROM HK_ModifyOrderPushBack ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HKModifyOrderPushBack> list = new List<HKModifyOrderPushBack>();
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
        public HKModifyOrderPushBack ReaderBind(IDataReader dataReader)
        {
            HKModifyOrderPushBack model = new HKModifyOrderPushBack();
            object ojb;
            model.ID = dataReader["ID"].ToString();
            model.TradeID = dataReader["TradeID"].ToString();
            model.Message = dataReader["Message"].ToString();
            ojb = dataReader["IsSuccess"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSuccess = (bool)ojb;
            }
            model.OriginalRequestNumber = dataReader["OriginalRequestNumber"].ToString();
            model.NewRequestNumber = dataReader["NewRequestNumber"].ToString();
            model.CallbackChannlId = dataReader["CallbackChannlId"].ToString();
            return model;
        }

        #endregion  成员方法

    }

    /// <summary>
    /// Title:港股改单回推数据【故障恢复】数据操作类
    /// Create by:李健华
    /// Create Date:2009-10-30 
    /// </summary>
    public class HK_ModifyPushBackOrderDal
    {
        /// <summary>
        /// 增加一条数据
        /// <param name="channelId"></param>
        /// <param name="modifyOrderID">改单记录ID</param>
        /// </summary>
        public void Add(string modifyOrderID, string channelId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_ModifyPushBackOrder(");
            strSql.Append("ModifyOrderID,ChannelID");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + modifyOrderID + "',");
            strSql.Append("'" + channelId + "'");
            strSql.Append(")");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 删除一条数据
        /// <param name="modifyOrderID">Id</param>
        /// </summary>
        public void Delete(string modifyOrderID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HK_ModifyPushBackOrder ");
            strSql.Append(" where ModifyOrderID ='" + modifyOrderID + "'");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand);

        }
        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <returns></returns>
        public bool JudgeConnectionState()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("数据库连接异常", ex);
                return false;
            }
            return false;
        }

        /// <summary>
        /// 删除不是当前的回推记录
        /// <param name="callbackChannlId">回推通道ID</param>
        /// </summary>
        public int DeleteNotPushToday(string callbackChannlId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HK_ModifyPushBackOrder where ModifyOrderID ");
            strSql.Append("  in( select ID from [HK_ModifyOrderPushBack] where  ");
            strSql.Append(" CONVERT(varchar(100), CreateDate, 23)!=CONVERT(varchar(100), getdate(), 23) ");
            strSql.Append(" and CallbackChannlId='" + callbackChannlId + "' ) ");//删除故障恢复中不是当天的数据
            strSql.Append(" delete [HK_ModifyOrderPushBack] where  ");
            strSql.Append(" CONVERT(varchar(100), CreateDate, 23)!=CONVERT(varchar(100), getdate(), 23)  ");
            strSql.Append(" and CallbackChannlId='" + callbackChannlId + "' "); //删除回推数据表中的不是当天的数据
            strSql.Append(" select Count(1) from HK_ModifyPushBackOrder where ChannelID='" + callbackChannlId + "' ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            int cmdresult = 0;
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult;


        }
        /// <summary>
        /// 根据通道ID获取还没有回推的数据
        /// <param name="channleID">通道ID</param>
        /// </summary>
        public List<HKModifyOrderPushBack> GetListPushDataByChannleID(string channleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,TradeID,Message,IsSuccess,OriginalRequestNumber,NewRequestNumber,CallbackChannlId ");
            strSql.Append(" FROM HK_ModifyOrderPushBack ");
            strSql.Append(" where ID in( select ModifyOrderID from dbo.HK_ModifyPushBackOrder ");
            strSql.Append("  where ChannelID='" + channleID + "')");
            HK_ModifyOrderPushBackDal dal = new HK_ModifyOrderPushBackDal();

            List<HKModifyOrderPushBack> list = new List<HKModifyOrderPushBack>();
            Database db = DatabaseFactory.CreateDatabase();
            using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))
            {
                while (dataReader.Read())
                {
                    list.Add(dal.ReaderBind(dataReader));
                }
            }
            return list;
        }
    }
}
