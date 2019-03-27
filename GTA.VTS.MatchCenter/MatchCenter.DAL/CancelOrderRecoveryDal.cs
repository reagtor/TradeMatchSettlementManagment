using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using MatchCenter.Entity;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.DAL
{
    /// <summary>
    /// Tilte;撤单故障恢复数据操作DAL类
    /// Create BY：李健华
    /// Create date:2009-12-01
    /// </summary>
    public class CancelOrderRecoveryDal
    {
        public CancelOrderRecoveryDal()
        { }
        #region  成员方法


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void Add(CancelEntity model, int breedClassType)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CancelOrderRecovery(");
                strSql.Append("OrderNo,ChannelNo,CancelCount,Code,BreedClassType)");

                strSql.Append(" values (");
                strSql.Append("@OrderNo,@ChannelNo,@CancelCount,@Code,@BreedClassType)");
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                //db.AddInParameter(dbCommand, "ID", DbType.Guid, model.ID);
                db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, model.OldOrderNo);
                db.AddInParameter(dbCommand, "ChannelNo", DbType.AnsiString, model.ChannelNo);
                db.AddInParameter(dbCommand, "CancelCount", DbType.Int32, model.CancelCount);
                db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.StockCode);
                db.AddInParameter(dbCommand, "BreedClassType", DbType.Int32, breedClassType);
                //db.AddInParameter(dbCommand, "CreateDate", DbType.DateTime, model.CreateDate);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接受阻添加当日要恢复的撤单数据异常", ex);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public static void Delete(string orderNo)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CancelOrderRecovery ");
            strSql.Append(" where OrderNo=@OrderNo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, orderNo);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除数据库中所有数据
        /// </summary>
        public static void DeleteAll()
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CancelOrderRecovery ");
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:删除当日要恢复的撤单数据异常", ex);
            }
        }

        /// <summary>
        /// 删除所有不是今日的所有数据
        /// </summary>
        public static void Delete()
        {
            string deleteIsNotTodayData = " Delete from CancelOrderRecovery where DATEDIFF(day, CreateDate, '" + DateTime.Now.ToShortDateString() + "') != 0";
            SqlHelper.AsyncExecuteNonQuery(deleteIsNotTodayData, SqlHelper.CallbackAsyncExecuteNonQuery);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static CancelEntity GetModel(string orderNo)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OrderNo,ChannelNo,CancelCount,Code from CancelOrderRecovery ");
            strSql.Append(" where OrderNo=@orderNo ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OrderNo", DbType.AnsiString, orderNo);
            CancelEntity model = null;
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
        /// 获取所有当日要恢复的数据根据品种类型获取
        /// <param name="breedClassType">品种类型</param>
        /// </summary>
        public static List<CancelEntity> GetListTodayRecoveryByBreedClassTypeID(int breedClassType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OrderNo,ChannelNo,CancelCount,Code ");
            strSql.Append(" FROM CancelOrderRecovery ");
            strSql.AppendFormat(" where BreedClassType={0} And DATEDIFF(day, CreateDate, '{1}') = 0", breedClassType, DateTime.Now.ToShortDateString());

            List<CancelEntity> list = new List<CancelEntity>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))
                {
                    while (dataReader.Read())
                    {
                        list.Add(ReaderBind(dataReader));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-111:数据库连接受阻获取所有当日要恢复的撤单数据异常", ex);
            }
            return list;
        }

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public static List<CancelEntity> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,OrderNo,ChannelNo,CancelCount,Code,BreedClassType,CreateDate ");
            strSql.Append(" FROM CancelOrderRecovery ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<CancelEntity> list = new List<CancelEntity>();
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
        public static CancelEntity ReaderBind(IDataReader dataReader)
        {
            CancelEntity model = new CancelEntity();
            object ojb;
            model.OldOrderNo = dataReader["OrderNo"].ToString();
            model.ChannelNo = dataReader["ChannelNo"].ToString();
            ojb = dataReader["CancelCount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CancelCount = (int)ojb;
            }
            model.StockCode = dataReader["Code"].ToString();
            return model;
        }

        #endregion  成员方法

    }
}
