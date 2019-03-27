using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity.Model.QH;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace ReckoningCounter.DAL.Data
{

    /// <summary>
    /// 数据访问类QH_TodaySettlementPrice。
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// Desc.: 记录期货今日结算价数据访问类
    /// </summary>
    public class QH_TodaySettlementPriceDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QH_TodaySettlementPriceDal()
        { }

 
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string CommodityCode, int TradingDate)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_TodaySettlementPrice where CommodityCode=@CommodityCode and TradingDate=@TradingDate ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
            db.AddInParameter(dbCommand, "TradingDate", DbType.Int32, TradingDate);
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
        public void Add(QH_TodaySettlementPriceInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_TodaySettlementPrice(");
            strSql.Append("CommodityCode,TradingDate,SettlementPrice)");

            strSql.Append(" values (");
            strSql.Append("@CommodityCode,@TradingDate,@SettlementPrice)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, model.CommodityCode);
            db.AddInParameter(dbCommand, "TradingDate", DbType.Int32, model.TradingDate);
            db.AddInParameter(dbCommand, "SettlementPrice", DbType.Decimal, model.SettlementPrice);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_TodaySettlementPriceInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_TodaySettlementPrice set ");
            strSql.Append("SettlementPrice=@SettlementPrice");
            strSql.Append(" where CommodityCode=@CommodityCode and TradingDate=@TradingDate ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, model.CommodityCode);
            db.AddInParameter(dbCommand, "TradingDate", DbType.Int32, model.TradingDate);
            db.AddInParameter(dbCommand, "SettlementPrice", DbType.Decimal, model.SettlementPrice);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string CommodityCode, int TradingDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_TodaySettlementPrice ");
            strSql.Append(" where CommodityCode=@CommodityCode and TradingDate=@TradingDate ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
            db.AddInParameter(dbCommand, "TradingDate", DbType.Int32, TradingDate);
            db.ExecuteNonQuery(dbCommand);

        }
        /// <summary>
        /// 删除清空今日结算价表中所有的数据
        /// </summary>
        public void Delete()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete QH_TodaySettlementPrice ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand);

        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_TodaySettlementPriceInfo GetModel(string CommodityCode, int TradingDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CommodityCode,TradingDate,SettlementPrice from QH_TodaySettlementPrice ");
            strSql.Append(" where CommodityCode=@CommodityCode and TradingDate=@TradingDate ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
            db.AddInParameter(dbCommand, "TradingDate", DbType.Int32, TradingDate);
            QH_TodaySettlementPriceInfo model = null;
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
            strSql.Append("select CommodityCode,TradingDate,SettlementPrice ");
            strSql.Append(" FROM QH_TodaySettlementPrice ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("UP_GetRecordByPage");
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "QH_TodaySettlementPrice");
            db.AddInParameter(dbCommand, "fldName", DbType.AnsiString, "ID");
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddInParameter(dbCommand, "IsReCount", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "OrderType", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "strWhere", DbType.AnsiString, strWhere);
            return db.ExecuteDataSet(dbCommand);
        }*/

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<QH_TodaySettlementPriceInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CommodityCode,TradingDate,SettlementPrice ");
            strSql.Append(" FROM QH_TodaySettlementPrice ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_TodaySettlementPriceInfo> list = new List<QH_TodaySettlementPriceInfo>();
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
        public QH_TodaySettlementPriceInfo ReaderBind(IDataReader dataReader)
        {
            QH_TodaySettlementPriceInfo model = new QH_TodaySettlementPriceInfo();
            object ojb;
            model.CommodityCode = dataReader["CommodityCode"].ToString();
            ojb = dataReader["TradingDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradingDate = (int)ojb;
            }
            ojb = dataReader["SettlementPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SettlementPrice = (decimal)ojb;
            }
            return model;
        }
     
       
    }
}
