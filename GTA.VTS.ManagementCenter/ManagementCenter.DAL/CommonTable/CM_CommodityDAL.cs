using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using ManagementCenter.Model.CommonTable;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述:交易商品 数据访问类CM_CommodityDAL。
    ///作者：刘书伟
    ///日期:2008-11-20
    /// </summary>
    public class CM_CommodityDAL
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CM_CommodityDAL()
        { }
        #endregion

        #region SQL

        /// <summary>
        /// 获取交易商品
        /// </summary>
        private string SQL_SELECT_CMCOMMODITY =
            @"SELECT * FROM CM_COMMODITY WHERE 1=1 ";

        /// <summary>
        /// 获取品种类型股指期货的商品代码
        /// </summary>
        private string SQL_SELECTCOMMODITYCODE_CMCOMMODITY = @"SELECT B.COMMODITYCODE,B.COMMODITYNAME 
                                                            FROM CM_BREEDCLASS AS A,CM_COMMODITY AS B
                                                            WHERE A.BREEDCLASSID=B.BREEDCLASSID 
                                                            AND BREEDCLASSTYPEID=3 AND B.ISEXPIRED=2";

        #endregion
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string CommodityCode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CM_Commodity where CommodityCode=@CommodityCode ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
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
        public bool Add(ManagementCenter.Model.CM_Commodity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CM_Commodity(");
            strSql.Append("CommodityCode,CommodityName,BreedClassID,LabelCommodityCode,GoerScale,MarketDate,StockPinYin,turnovervolume,IsExpired,ISSysDefaultCode)");

            strSql.Append(" values (");
            strSql.Append("@CommodityCode,@CommodityName,@BreedClassID,@LabelCommodityCode,@GoerScale,@MarketDate,@StockPinYin,@turnovervolume,@IsExpired,@ISSysDefaultCode)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, model.CommodityCode);
            db.AddInParameter(dbCommand, "CommodityName", DbType.String, model.CommodityName);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            if (string.IsNullOrEmpty(model.LabelCommodityCode) || model.LabelCommodityCode == AppGlobalVariable.INIT_STRING)
            {
                db.AddInParameter(dbCommand, "LabelCommodityCode", DbType.String, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "LabelCommodityCode", DbType.String, model.LabelCommodityCode);
            }
            if (model.GoerScale == AppGlobalVariable.INIT_DECIMAL)
            {
                db.AddInParameter(dbCommand, "GoerScale", DbType.Decimal, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "GoerScale", DbType.Decimal, model.GoerScale);

            }
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
            if (model.IsExpired == AppGlobalVariable.INIT_INT)
            {
                model.IsExpired = (int)Types.IsYesOrNo.No;
                db.AddInParameter(dbCommand, "IsExpired", DbType.Int32, model.IsExpired);
            }
            else
            {
                db.AddInParameter(dbCommand, "IsExpired", DbType.Int32, model.IsExpired);

            }
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
        /// 更新交易商品(根据2。0需求，只允许修改品种类型，其它字段不允许修改)
        /// </summary>
        /// <param name="model">交易商品实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.CM_Commodity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CM_Commodity set ");
            //strSql.Append("CommodityName=@CommodityName,");
            strSql.Append("BreedClassID=@BreedClassID,");
            // strSql.Append("LabelCommodityCode=@LabelCommodityCode,");
            //strSql.Append("GoerScale=@GoerScale,");
            strSql.Append("MarketDate=@MarketDate,");
            //strSql.Append("StockPinYin=@StockPinYin,");
            //strSql.Append("turnovervolume=@turnovervolume,");
            strSql.Append("IsExpired=@IsExpired,");
            strSql.Append("ISSysDefaultCode=@ISSysDefaultCode");
            strSql.Append(" where CommodityCode=@CommodityCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, model.CommodityCode);
            //db.AddInParameter(dbCommand, "CommodityName", DbType.String, model.CommodityName);
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, model.BreedClassID);
            
            if (model.MarketDate == AppGlobalVariable.INIT_DATETIME)
            {
                db.AddInParameter(dbCommand, "MarketDate", DbType.DateTime, DBNull.Value);
            }
            else
            {
                db.AddInParameter(dbCommand, "MarketDate", DbType.DateTime, model.MarketDate);

            }
            //db.AddInParameter(dbCommand, "StockPinYin", DbType.String, model.StockPinYin);
            //db.AddInParameter(dbCommand, "turnovervolume", DbType.Double, model.turnovervolume);
            if (model.IsExpired == AppGlobalVariable.INIT_INT)
            {
                model.IsExpired = (int)Types.IsYesOrNo.No;
                db.AddInParameter(dbCommand, "IsExpired", DbType.Int32, model.IsExpired);
            }
            else
            {
                db.AddInParameter(dbCommand, "IsExpired", DbType.Int32, model.IsExpired);

            }
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

        #region 根据商品代码，更新期货代码是否更新状态
        /// <summary>
        /// 根据商品代码，更新期货代码是否更新状态
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="IsExpired">期货代码是否过期</param>
        /// <returns></returns>
        public bool Update(string CommodityCode, int IsExpired)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CM_Commodity set ");
            strSql.Append("IsExpired=@IsExpired");
            strSql.Append(" where CommodityCode=@CommodityCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
            db.AddInParameter(dbCommand, "IsExpired", DbType.Int32, IsExpired);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }
        #endregion

        #region  根据品种ID，更新交易商品表中的品种ID
        /// <summary>
        /// 根据品种ID，更新交易商品表中的品种ID
        /// </summary>
        /// <param name="OldBreedClassID">修改前的品种ID</param>
        /// <param name="NewBreedClassID">修改后的品种ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool UpdateBreedClassID(int OldBreedClassID, int NewBreedClassID, DbTransaction tran, Database db)
        {
            var strSql = new StringBuilder();
            strSql.Append("update CM_Commodity set ");
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

        #region  根据品种ID，更新交易商品表中的品种ID(无事务)
        /// <summary>
        /// 根据品种ID，更新交易商品表中的品种ID(无事务)
        /// </summary>
        /// <param name="OldBreedClassID">修改前的品种ID</param>
        /// <param name="NewBreedClassID">修改后的品种ID</param>
        /// <returns></returns>
        public bool UpdateBreedClassID(int OldBreedClassID, int NewBreedClassID)
        {
            return UpdateBreedClassID(OldBreedClassID, NewBreedClassID, null, null);
        }
        #endregion

        /// <summary>
        /// 根据代码删除代码记录
        /// </summary>
        /// <param name="CommodityCode"></param>
        /// <returns></returns>
        public bool Delete(string CommodityCode)
        {
            return Delete(CommodityCode, null, null);
        }

        /// <summary>
        /// 根据代码删除代码记录
        /// </summary>
        /// <param name="CommodityCode">代码</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(string CommodityCode, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete CM_Commodity ");
            strSql.Append(" where CommodityCode=@CommodityCode ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();

            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
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
        /// 根据品种ID删除交易品种
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="tran"></param>
        /// <param name="db">数据库</param>
        /// <returns></returns>
        public bool DeleteCommodityByBreedClassID(int BreedClassID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete CM_Commodity ");
            strSql.Append(" where BreedClassID=@BreedClassID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();

            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            // db.ExecuteNonQuery(dbCommand);
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                obj = db.ExecuteNonQuery(dbCommand, tran);
            }
            return true;
        }

        /// <summary>
        ///  根据品种ID删除交易品种
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool DeleteCommodityByBreedClassID(int BreedClassID)
        {
            return DeleteCommodityByBreedClassID(BreedClassID, null, null);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_Commodity GetModel(string CommodityCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CommodityCode,CommodityName,BreedClassID,LabelCommodityCode,GoerScale,MarketDate,StockPinYin,turnovervolume,IsExpired,ISSysDefaultCode from CM_Commodity ");
            strSql.Append(" where CommodityCode=@CommodityCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
            ManagementCenter.Model.CM_Commodity model = null;
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
            strSql.Append("select CommodityCode,CommodityName,BreedClassID,LabelCommodityCode,GoerScale,MarketDate,StockPinYin,turnovervolume,IsExpired,ISSysDefaultCode ");
            strSql.Append(" FROM CM_Commodity ");
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
        public List<ManagementCenter.Model.CM_Commodity> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CommodityCode,CommodityName,BreedClassID,LabelCommodityCode,GoerScale,MarketDate,StockPinYin,turnovervolume,IsExpired,ISSysDefaultCode ");
            strSql.Append(" FROM CM_Commodity ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.CM_Commodity> list = new List<ManagementCenter.Model.CM_Commodity>();
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
        /// 获取所有代码及昨日收盘价
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_Commodity> GetListCMCommodityAndClosePrice(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select CommodityCode,CommodityName,BreedClassID,LabelCommodityCode,GoerScale,MarketDate,StockPinYin,turnovervolume,IsExpired,ISSysDefaultCode ");
            //strSql.Append(" FROM CM_Commodity ");
            strSql.Append("select a.*,b.ClosePrice from CM_Commodity a left join ClosePriceInfo b ");
            strSql.Append(" on a.CommodityCode=b.StockCode and b.BreedClassTypeID<>4 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.CM_Commodity> list = new List<ManagementCenter.Model.CM_Commodity>();
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

        #region 获取所有交易商品

        /// <summary>
        /// 获取所有交易商品
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="CommodityName">商品名称</param>
        ///  <param name="BreedClassID">品种ID</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMCommodity(string CommodityCode, string CommodityName, int BreedClassID, int pageNo, int pageSize,
                                            out int rowCount)
        {
            //条件查询
            if ((CommodityCode != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(CommodityCode))
                || (CommodityName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(CommodityName)))
            {
                SQL_SELECT_CMCOMMODITY += " AND (CommodityCode LIKE  '%' + @CommodityCode + '%' ";
                //}
                //if (CommodityName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(CommodityName))
                //{
                SQL_SELECT_CMCOMMODITY += " OR CommodityName LIKE  '%' + @CommodityName + '%') ";
            }
            if (BreedClassID != AppGlobalVariable.INIT_INT)
            {
                SQL_SELECT_CMCOMMODITY += "AND (BreedClassID=@BreedClassID) ";
            }
            SQL_SELECT_CMCOMMODITY += " AND ISEXPIRED<>1  ";

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_CMCOMMODITY);

            if (CommodityCode != AppGlobalVariable.INIT_STRING && CommodityCode != string.Empty)
            {
                database.AddInParameter(dbCommand, "CommodityCode", DbType.String, CommodityCode);
            }
            if (CommodityName != AppGlobalVariable.INIT_STRING && CommodityName != string.Empty)
            {
                database.AddInParameter(dbCommand, "CommodityName", DbType.String, CommodityName);
            }
            if (BreedClassID != AppGlobalVariable.INIT_INT)
            {
                database.AddInParameter(dbCommand, "BreedClassID", DbType.Int32, BreedClassID);
            }
            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_CMCOMMODITY, pageNo, pageSize,
                                        out rowCount, "CM_Commodity");
        }

        #endregion

        #region 获取品种类型股指期货的商品代码
        /// <summary>
        /// 获取品种类型股指期货的商品代码
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHSIFCommodityCode()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTCOMMODITYCODE_CMCOMMODITY);
            return database.ExecuteDataSet(dbCommand);
        }
        #endregion

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.CM_Commodity ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.CM_Commodity model = new ManagementCenter.Model.CM_Commodity();
            object ojb;
            model.CommodityCode = dataReader["CommodityCode"].ToString();
            model.CommodityName = dataReader["CommodityName"].ToString();
            model.StockPinYin = dataReader["StockPinYin"].ToString();
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            model.LabelCommodityCode = dataReader["LabelCommodityCode"].ToString();
            ojb = dataReader["GoerScale"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.GoerScale = (decimal)ojb;
            }
            ojb = dataReader["MarketDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketDate = (DateTime)ojb;
            }
            ojb = dataReader["turnovervolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.turnovervolume = (double)ojb;
            }
            ojb = dataReader["IsExpired"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsExpired = (int)ojb;
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
        /// 根据查询条件获取行业标识数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>返回行业标识数据</returns>
        public List<ManagementCenter.Model.CommonTable.OnstageCommodity> GetCommodityListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CM_Commodity.*, Nindcd from CM_Commodity left join StockInfo on CommodityCode=StockCode ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.CommonTable.OnstageCommodity> list = new List<ManagementCenter.Model.CommonTable.OnstageCommodity>();
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
        /// 行业标识实体
        /// </summary>
        /// <param name="dataReader">读数据</param>
        /// <returns>返回行业标识实体</returns>
        public ManagementCenter.Model.CommonTable.OnstageCommodity _ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.CommonTable.OnstageCommodity model = new OnstageCommodity();
            object ojb;
            model.CommodityCode = dataReader["CommodityCode"].ToString();
            model.CommodityName = dataReader["CommodityName"].ToString();
            model.StockPinYin = dataReader["StockPinYin"].ToString();
            model.Nindcd = dataReader["Nindcd"].ToString();
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            model.LabelCommodityCode = dataReader["LabelCommodityCode"].ToString();
            ojb = dataReader["GoerScale"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.GoerScale = (decimal)ojb;
            }
            ojb = dataReader["MarketDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MarketDate = (DateTime)ojb;
            }
            ojb = dataReader["turnovervolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.turnovervolume = (double)ojb;
            }
            ojb = dataReader["IsExpired"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsExpired = (int)ojb;
            }
            return model;
        }
        #endregion
    }
}

