using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;

namespace ManagementCenter.DAL.CommonTable
{
    /// <summary>
    ///描述：股票收盘价 数据访问类ClosePriceInfoDAL。
    ///作者：刘书伟
    ///日期:2009-11-27
    /// </summary>
    public class ClosePriceInfoDAL
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string StockCode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ClosePriceInfo where StockCode=@StockCode ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, StockCode);
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
        public void Add(ManagementCenter.Model.ClosePriceInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ClosePriceInfo(");
            strSql.Append("StockCode,ClosePrice,ClosePriceDate,BreedClassTypeID)");

            strSql.Append(" values (");
            strSql.Append("@StockCode,@ClosePrice,@ClosePriceDate,@BreedClassTypeID)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, model.StockCode);
            db.AddInParameter(dbCommand, "ClosePrice", DbType.Decimal, model.ClosePrice);
            db.AddInParameter(dbCommand, "ClosePriceDate", DbType.DateTime, model.ClosePriceDate);
            db.AddInParameter(dbCommand, "BreedClassTypeID", DbType.Int32, model.BreedClassTypeID);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.ClosePriceInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ClosePriceInfo set ");
            strSql.Append("ClosePrice=@ClosePrice,");
            strSql.Append("ClosePriceDate=@ClosePriceDate,");
            strSql.Append("BreedClassTypeID=@BreedClassTypeID ");
            strSql.Append(" where StockCode=@StockCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, model.StockCode);
            db.AddInParameter(dbCommand, "ClosePrice", DbType.Decimal, model.ClosePrice);
            db.AddInParameter(dbCommand, "ClosePriceDate", DbType.DateTime, model.ClosePriceDate);
            db.AddInParameter(dbCommand, "BreedClassTypeID", DbType.Int32, model.BreedClassTypeID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string StockCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete ClosePriceInfo ");
            strSql.Append(" where StockCode=@StockCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, StockCode);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.ClosePriceInfo GetModel(string StockCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StockCode,ClosePrice,ClosePriceDate,BreedClassTypeID from ClosePriceInfo ");
            strSql.Append(" where StockCode=@StockCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, StockCode);
            ManagementCenter.Model.ClosePriceInfo model = null;
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
            strSql.Append("select StockCode,ClosePrice,ClosePriceDate,BreedClassTypeID ");
            strSql.Append(" FROM ClosePriceInfo ");
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
        public List<ManagementCenter.Model.ClosePriceInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StockCode,ClosePrice,ClosePriceDate,BreedClassTypeID ");
            strSql.Append(" FROM ClosePriceInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.ClosePriceInfo> list = new List<ManagementCenter.Model.ClosePriceInfo>();
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
        public ManagementCenter.Model.ClosePriceInfo ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.ClosePriceInfo model = new ManagementCenter.Model.ClosePriceInfo();
            object ojb;
            model.StockCode = dataReader["StockCode"].ToString();
            ojb = dataReader["ClosePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ClosePrice = (decimal)ojb;
            }
            ojb = dataReader["ClosePriceDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ClosePriceDate = (DateTime)ojb;
            }
            ojb = dataReader["BreedClassTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassTypeID = (int)ojb;
            }
            return model;
        }

        #endregion  成员方法


    }
}
