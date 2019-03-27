using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述：港股交易商品 数据访问类HK_CommodityDAL。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_CommodityDAL
    {
        public HK_CommodityDAL()
        {
        }

        #region SQL

        /// <summary>
        /// 获取港股交易商品
        /// </summary>
        private string SQL_SELECT_HKCOMMODITY =
            @"SELECT * FROM HK_COMMODITY WHERE 1=1 ";


        #endregion

        #region  成员方法

        /// <summary>
        /// 是否存在港股交易商品记录
        /// </summary>
        /// <param name="HKCommodityCode">港股交易商品代码</param>
        /// <returns></returns>
        public bool Exists(string HKCommodityCode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_Commodity where HKCommodityCode=@HKCommodityCode ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HKCommodityCode", DbType.String, HKCommodityCode);
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
        ///添加港股交易商品 
        /// </summary>
        /// <param name="model">港股交易商品实体</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.HK_Commodity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_Commodity(");
            strSql.Append("HKCommodityCode,HKCommodityName,BreedClassID,MarketDate,StockPinYin,turnovervolume,PerHandThighOrShare,IsSellNull,ISSysDefaultCode)");

            strSql.Append(" values (");
            strSql.Append("@HKCommodityCode,@HKCommodityName,@BreedClassID,@MarketDate,@StockPinYin,@turnovervolume,@PerHandThighOrShare,@IsSellNull,@ISSysDefaultCode)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HKCommodityCode", DbType.String, model.HKCommodityCode);
            db.AddInParameter(dbCommand, "HKCommodityName", DbType.String, model.HKCommodityName);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            //db.AddInParameter(dbCommand, "MarketDate", DbType.DateTime, model.MarketDate);
            if (model.MarketDate == AppGlobalVariable.INIT_DATETIME)
            {
                db.AddInParameter(dbCommand, "MarketDate", DbType.DateTime, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "MarketDate", DbType.DateTime, model.MarketDate);

            }
            db.AddInParameter(dbCommand, "StockPinYin", DbType.String, model.StockPinYin);
            db.AddInParameter(dbCommand, "turnovervolume", DbType.Double, model.turnovervolume);
            db.AddInParameter(dbCommand, "PerHandThighOrShare", DbType.Int32, model.PerHandThighOrShare);
            db.AddInParameter(dbCommand, "IsSellNull", DbType.Int32, model.IsSellNull);
            // db.AddInParameter(dbCommand, "ISSysDefaultCode", DbType.Int32, model.ISSysDefaultCode);
            if (model.ISSysDefaultCode == AppGlobalVariable.INIT_INT)
            {
                model.ISSysDefaultCode = (int)Types.IsYesOrNo.No;
                db.AddInParameter(dbCommand, "ISSysDefaultCode", DbType.Int32, model.ISSysDefaultCode);
            }
            else
            {
                db.AddInParameter(dbCommand, "ISSysDefaultCode", DbType.Int32, model.ISSysDefaultCode);

            }
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        /// <summary>
        /// 更新港股交易商品
        /// </summary>
        /// <param name="model">港股交易商品实体</param>
        public bool Update(ManagementCenter.Model.HK_Commodity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_Commodity set ");
            //strSql.Append("HKCommodityName=@HKCommodityName,");
            strSql.Append("BreedClassID=@BreedClassID,");
            //strSql.Append("MarketDate=@MarketDate,");
            //strSql.Append("StockPinYin=@StockPinYin,");
            // strSql.Append("turnovervolume=@turnovervolume,");
            //strSql.Append("PerHandThighOrShare=@PerHandThighOrShare,");
            //strSql.Append("IsSellNull=@IsSellNull,");
            strSql.Append("ISSysDefaultCode=@ISSysDefaultCode");
            strSql.Append(" where HKCommodityCode=@HKCommodityCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HKCommodityCode", DbType.String, model.HKCommodityCode);
            // db.AddInParameter(dbCommand, "HKCommodityName", DbType.String, model.HKCommodityName);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            //db.AddInParameter(dbCommand, "MarketDate", DbType.DateTime, model.MarketDate);
            //db.AddInParameter(dbCommand, "StockPinYin", DbType.String, model.StockPinYin);
            //db.AddInParameter(dbCommand, "turnovervolume", DbType.Double, model.turnovervolume);
            //db.AddInParameter(dbCommand, "PerHandThighOrShare", DbType.Int32, model.PerHandThighOrShare);
            //db.AddInParameter(dbCommand, "IsSellNull", DbType.Int32, model.IsSellNull);
            //db.AddInParameter(dbCommand, "ISSysDefaultCode", DbType.Int32, model.ISSysDefaultCode);
            if (model.ISSysDefaultCode == AppGlobalVariable.INIT_INT)
            {
                model.ISSysDefaultCode = (int)Types.IsYesOrNo.No;
                db.AddInParameter(dbCommand, "ISSysDefaultCode", DbType.Int32, model.ISSysDefaultCode);
            }
            else
            {
                db.AddInParameter(dbCommand, "ISSysDefaultCode", DbType.Int32, model.ISSysDefaultCode);

            }
            db.ExecuteNonQuery(dbCommand);
            return true;
        }

        #region  根据品种ID，更新港股交易商品表中的品种ID
        /// <summary>
        /// 根据品种ID，更新港股交易商品表中的品种ID
        /// </summary>
        /// <param name="OldBreedClassID">修改前的品种ID</param>
        /// <param name="NewBreedClassID">修改后的品种ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool UpdateHKBreedClassID(int OldBreedClassID, int NewBreedClassID, DbTransaction tran, Database db)
        {
            var strSql = new StringBuilder();
            strSql.Append("update HK_Commodity set ");
            strSql.Append("BreedClassID=@NewBreedClassID");
            strSql.Append(" where BreedClassID=@OldBreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "OldBreedClassID", DbType.Int32, OldBreedClassID);
            db.AddInParameter(dbCommand, "NewBreedClassID", DbType.Int32, NewBreedClassID);
            //db.ExecuteNonQuery(dbCommand);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
            return true;
        }
        #endregion

        #region  根据品种ID，更新港股交易商品表中的品种ID(无事务)
        /// <summary>
        /// 根据品种ID，更新港股交易商品表中的品种ID(无事务)
        /// </summary>
        /// <param name="OldBreedClassID">修改前的品种ID</param>
        /// <param name="NewBreedClassID">修改后的品种ID</param>
        /// <returns></returns>
        public bool UpdateHKBreedClassID(int OldBreedClassID, int NewBreedClassID)
        {
            return UpdateHKBreedClassID(OldBreedClassID, NewBreedClassID, null, null);
        }
        #endregion



        /// <summary>
        /// 根据港股交易商品代码删除港股交易商品
        /// </summary>
        /// <param name="HKCommodityCode">港股交易商品代码</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool Delete(string HKCommodityCode, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HK_Commodity ");
            strSql.Append(" where HKCommodityCode=@HKCommodityCode ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();

            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HKCommodityCode", DbType.String, HKCommodityCode);
            //db.ExecuteNonQuery(dbCommand);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);

            }
            return true;
        }

        /// <summary>
        /// 根据港股交易商品代码删除港股交易商品(无事务)
        /// </summary>
        /// <param name="HKCommodityCode">港股交易商品代码</param>
        /// <returns></returns>
        public bool Delete(string HKCommodityCode)
        {
            return Delete(HKCommodityCode, null, null);
        }

        /// <summary>
        /// 根据港股交易商品代码获取实体对象
        /// </summary>
        /// <param name="HKCommodityCode">港股交易商品代码</param>
        /// <returns></returns>
        public ManagementCenter.Model.HK_Commodity GetModel(string HKCommodityCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HKCommodityCode,HKCommodityName,BreedClassID,MarketDate,StockPinYin,turnovervolume,PerHandThighOrShare,IsSellNull,ISSysDefaultCode from HK_Commodity ");
            strSql.Append(" where HKCommodityCode=@HKCommodityCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "HKCommodityCode", DbType.String, HKCommodityCode);
            ManagementCenter.Model.HK_Commodity model = null;
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
        ///获取港股交易商品数据列表 
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HKCommodityCode,HKCommodityName,BreedClassID,MarketDate,StockPinYin,turnovervolume,PerHandThighOrShare,IsSellNull,ISSysDefaultCode ");
            strSql.Append(" FROM HK_Commodity ");
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
        public List<ManagementCenter.Model.HK_Commodity> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HKCommodityCode,HKCommodityName,BreedClassID,MarketDate,StockPinYin,turnovervolume,PerHandThighOrShare,IsSellNull,ISSysDefaultCode ");
            strSql.Append(" FROM HK_Commodity ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HK_Commodity> list = new List<ManagementCenter.Model.HK_Commodity>();
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
        /// 获取所有港股代码及昨日收盘价
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HK_Commodity> GetListHKCommodityAndClosePrice(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select HKCommodityCode,HKCommodityName,BreedClassID,MarketDate,StockPinYin,turnovervolume,PerHandThighOrShare,IsSellNull,ISSysDefaultCode ");
            //strSql.Append(" FROM HK_Commodity ");
            strSql.Append("select a.*,b.ClosePrice from HK_Commodity a left join ClosePriceInfo b ");
            strSql.Append(" on a.HKCommodityCode=b.StockCode and b.BreedClassTypeID=4 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HK_Commodity> list = new List<ManagementCenter.Model.HK_Commodity>();
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

        #region 获取所有港股交易商品

        /// <summary>
        /// 获取所有港股交易商品
        /// </summary>
        /// <param name="HKCommodityCode">港股商品代码</param>
        /// <param name="HKCommodityName">港股商品名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllHKCommodity(string HKCommodityCode, string HKCommodityName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            //条件查询
            if ((HKCommodityCode != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(HKCommodityCode))
                || (HKCommodityName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(HKCommodityName)))
            {
                SQL_SELECT_HKCOMMODITY += " AND (HKCommodityCode LIKE  '%' + @HKCommodityCode + '%' ";

                SQL_SELECT_HKCOMMODITY += " OR HKCommodityName LIKE  '%' + @HKCommodityName + '%') ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_HKCOMMODITY);

            if (HKCommodityCode != AppGlobalVariable.INIT_STRING && HKCommodityCode != string.Empty)
            {
                database.AddInParameter(dbCommand, "HKCommodityCode", DbType.String, HKCommodityCode);
            }
            if (HKCommodityName != AppGlobalVariable.INIT_STRING && HKCommodityName != string.Empty)
            {
                database.AddInParameter(dbCommand, "HKCommodityName", DbType.String, HKCommodityName);
            }
            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_HKCOMMODITY, pageNo, pageSize,
                                        out rowCount, "HK_Commodity");
        }

        #endregion

        /// <summary>
        /// 港股交易商品的对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.HK_Commodity ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.HK_Commodity model = new ManagementCenter.Model.HK_Commodity();
            object ojb;
            model.HKCommodityCode = dataReader["HKCommodityCode"].ToString();
            model.HKCommodityName = dataReader["HKCommodityName"].ToString();
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            ojb = dataReader["MarketDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketDate = (DateTime)ojb;
            }
            model.StockPinYin = dataReader["StockPinYin"].ToString();
            ojb = dataReader["turnovervolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.turnovervolume = (double)ojb;
            }
            ojb = dataReader["PerHandThighOrShare"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PerHandThighOrShare = (int)ojb;
            }
            ojb = dataReader["IsSellNull"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSellNull = (int)ojb;
            }
            ojb = dataReader["ISSysDefaultCode"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ISSysDefaultCode = (int)ojb;
            }
            return model;
        }

        #endregion  成员方法

        #region 提供前台服务方法

        /// <summary>
        /// 根据查询条件返回港股代码及所属行业
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.OnstageHK_Commodity> GetHKCommodityListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HK_Commodity.*, Nindcd from HK_Commodity left join HKStockInfo on HKCommodityCode=StockCode ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.OnstageHK_Commodity> list = new List<ManagementCenter.Model.OnstageHK_Commodity>();
            Database db = DatabaseFactory.CreateDatabase();
            using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))
            {
                while (dataReader.Read())
                {
                    list.Add(_ReaderBind(dataReader));
                }
            }
            return list;
        }

        /// <summary>
        /// 返回港股代码及所属行业实体
        /// </summary>
        /// <param name="dataReader">读数据</param>
        /// <returns></returns>
        public ManagementCenter.Model.OnstageHK_Commodity _ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.OnstageHK_Commodity model = new OnstageHK_Commodity();
            object ojb;
            model.HKCommodityCode = dataReader["HKCommodityCode"].ToString();
            model.HKCommodityName = dataReader["HKCommodityName"].ToString();
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            ojb = dataReader["MarketDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketDate = (DateTime)ojb;
            }
            model.StockPinYin = dataReader["StockPinYin"].ToString();
            ojb = dataReader["turnovervolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.turnovervolume = (double)ojb;
            }
            ojb = dataReader["PerHandThighOrShare"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PerHandThighOrShare = (int)ojb;
            }
            ojb = dataReader["IsSellNull"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSellNull = (int)ojb;
            }
            ojb = dataReader["ISSysDefaultCode"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ISSysDefaultCode = (int)ojb;
            }

            //model.HKCommodityCode = dataReader["HKCommodityCode"].ToString();
            //model.HKCommodityName = dataReader["HKCommodityName"].ToString();
            //model.StockPinYin = dataReader["StockPinYin"].ToString();
            model.Nindcd = dataReader["Nindcd"].ToString();
            //ojb = dataReader["BreedClassID"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.BreedClassID = (int)ojb;
            //}
            //ojb = dataReader["MarketDate"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.MarketDate = (DateTime)ojb;
            //}
            //ojb = dataReader["turnovervolume"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.turnovervolume = (double)ojb;
            //}
            return model;
        }
        #endregion

    }
}
