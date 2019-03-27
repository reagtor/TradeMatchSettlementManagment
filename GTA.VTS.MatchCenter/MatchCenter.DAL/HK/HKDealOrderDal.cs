using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.Entity.HK;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.DAL.HK
{
    /// <summary>
    /// 撮合中心港股撮合成交单数据访问类HKDealOrderDal。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// Desc.:添加相关的数据更新方法和字段个改
    /// Create BY：李健华
    /// Create Date：2009-10-19
    /// </summary>
    public class HKDealOrderDal
    {
        public HKDealOrderDal()
        { }


        /// <summary>
        /// 根据主键判断是否存在该记录
        /// <param name="DealOrderNo">主键成交编号</param>
        /// </summary>
        public static bool Exists(string dealOrderNo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HKDealOrder where DealOrderNo=@DealOrderNo ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "DealOrderNo", DbType.AnsiString, dealOrderNo);
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
        /// <param name="model">成交记录实体</param>
        /// </summary>
        public static int Add(HKDealBackEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into HKDealOrder(");
                strSql.Append("DealOrderNo,OrderNo,ChannelID,DealPrice,DealAmount,DealTime,DealType,DealMessage)");

                strSql.Append(" values (");
                strSql.Append("@DealOrderNo,@OrderNo,@ChannelID,@DealPrice,@DealAmount,@DealTime,@DealType,@DealMessage)");
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.AddInParameter(dbCommand, "DealOrderNo", DbType.AnsiString, model.ID);
                db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, model.OrderNo);
                db.AddInParameter(dbCommand, "ChannelID", DbType.AnsiString, model.ChannelID);
                db.AddInParameter(dbCommand, "DealPrice", DbType.Decimal, model.DealPrice);
                db.AddInParameter(dbCommand, "DealAmount", DbType.Decimal, model.DealAmount);
                db.AddInParameter(dbCommand, "DealTime", DbType.DateTime, model.DealTime);
                db.AddInParameter(dbCommand, "DealType", DbType.Boolean, model.DealType);
                db.AddInParameter(dbCommand, "DealMessage", DbType.AnsiString, model.DealMessage);
                db.ExecuteNonQuery(dbCommand);
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接受阻", ex);
                return 0;
            }
        }
        /// <summary>
        /// 根据主键更新一条数据
        /// <param name="model">成交记录实体</param>
        /// </summary>
        public static void Update(HKDealBackEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HKDealOrder set ");
            strSql.Append("OrderNo=@OrderNo,");
            strSql.Append("ChannelID=@ChannelID,");
            strSql.Append("DealPrice=@DealPrice,");
            strSql.Append("DealAmount=@DealAmount,");
            strSql.Append("DealTime=@DealTime,");
            strSql.Append("DealType=@DealType,");
            strSql.Append("DealMessage=@DealMessage");
            strSql.Append(" where DealOrderNo=@DealOrderNo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "DealOrderNo", DbType.AnsiString, model.ID);
            db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, model.OrderNo);
            db.AddInParameter(dbCommand, "ChannelID", DbType.AnsiString, model.ChannelID);
            db.AddInParameter(dbCommand, "DealPrice", DbType.Decimal, model.DealPrice);
            db.AddInParameter(dbCommand, "DealAmount", DbType.Decimal, model.DealAmount);
            db.AddInParameter(dbCommand, "DealTime", DbType.DateTime, model.DealTime);
            db.AddInParameter(dbCommand, "DealType", DbType.Boolean, model.DealType);
            db.AddInParameter(dbCommand, "DealMessage", DbType.AnsiString, model.DealMessage);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 根据主键删除一条数据
        /// <param name="DealOrderNo">成交编号(主键)</param>
        /// </summary>
        public static void Delete(string dealOrderNo)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HKDealOrder ");
            strSql.Append(" where DealOrderNo=@DealOrderNo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "DealOrderNo", DbType.AnsiString, dealOrderNo);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除不是当天的成交记录数据
        /// </summary>
        public static void DeleteDeal()
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(" Delete from HKDealOrder where DATEDIFF(day, DealTime, '{0}') != 0 ", DateTime.Now.ToShortDateString());
            SqlHelper.AsyncExecuteNonQuery(strSql.ToString(), SqlHelper.CallbackAsyncExecuteNonQuery);
        }

        /// <summary>
        /// 根据主键得到一个实体（记录）
        /// <param name="dealOrderNo">成交编号(主键)</param>
        /// </summary>
        public static HKDealBackEntity GetModel(string dealOrderNo)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DealOrderNo,OrderNo,ChannelID,DealPrice,DealAmount,DealTime,DealType,DealMessage from HKDealOrder ");
            strSql.Append(" where DealOrderNo=@DealOrderNo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "DealOrderNo", DbType.AnsiString, dealOrderNo);
            HKDealBackEntity model = null;
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
        /// 根据查询条件语句获得数据列表
        /// <param name="strWhere">查询条件SQL语句</param>
        /// </summary>
        public static DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DealOrderNo,OrderNo,ChannelID,DealPrice,DealAmount,DealTime,DealType,DealMessage ");
            strSql.Append(" FROM HKDealOrder ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }


        /// <summary>
        /// 根据查询条件语句获得数据列表
        /// <param name="strWhere">查询条件SQL语句</param>
        /// </summary>
        public static List<HKDealBackEntity> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DealOrderNo,OrderNo,ChannelID,DealPrice,DealAmount,DealTime,DealType,DealMessage ");
            strSql.Append(" FROM HKDealOrder ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HKDealBackEntity> list = new List<HKDealBackEntity>();
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
        /// <param name="dataReader">IDataReader对象</param>
        /// </summary>
        public static HKDealBackEntity ReaderBind(IDataReader dataReader)
        {
            HKDealBackEntity model = new HKDealBackEntity();
            object ojb;
            model.ID = dataReader["DealOrderNo"].ToString();
            model.OrderNo = dataReader["OrderNo"].ToString();
            model.ChannelID = dataReader["ChannelID"].ToString();
            ojb = dataReader["DealPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DealPrice = (decimal)ojb;
            }
            ojb = dataReader["DealAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DealAmount = (decimal)ojb;
            }
            ojb = dataReader["DealTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DealTime = (DateTime)ojb;
            }
            ojb = dataReader["DealType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DealType = (bool)ojb;
            }
            model.DealMessage = dataReader["DealMessage"].ToString();
            return model;
        }

        /// <summary>
        /// 查找成交回报实体
        /// </summary>
        /// <returns></returns>
        public static List<HKDealBackEntity> GetDealBackEntityList()
        {
            List<HKDealBackEntity> list = new List<HKDealBackEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from HKDealOrder where DATEDIFF(day, DealTime, '{0}') = 0 ", DateTime.Now.ToShortDateString());
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader == null)
                {
                    return null;
                }
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }
    }
}
