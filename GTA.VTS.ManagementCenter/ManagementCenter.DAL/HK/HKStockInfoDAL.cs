using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述：港股股票代码 数据访问类HKStockInfoDAL。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HKStockInfoDAL
    {
        public HKStockInfoDAL()
        {

        }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string StockCode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HKStockInfo where StockCode=@StockCode ");
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
        public void Add(ManagementCenter.Model.HKStockInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HKStockInfo(");
            strSql.Append("StockCode,StockName,StockPinYin,turnovervolume,Paydt,Nindcd,PerHandThighOrShare,IsSellNull)");

            strSql.Append(" values (");
            strSql.Append("@StockCode,@StockName,@StockPinYin,@turnovervolume,@Paydt,@Nindcd,@PerHandThighOrShare,@IsSellNull)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, model.StockCode);
            db.AddInParameter(dbCommand, "StockName", DbType.String, model.StockName);
            db.AddInParameter(dbCommand, "StockPinYin", DbType.String, model.StockPinYin);
            db.AddInParameter(dbCommand, "turnovervolume", DbType.Double, model.turnovervolume);
            db.AddInParameter(dbCommand, "Paydt", DbType.String, model.Paydt);
            db.AddInParameter(dbCommand, "Nindcd", DbType.String, model.Nindcd);
            db.AddInParameter(dbCommand, "PerHandThighOrShare", DbType.Int32, model.PerHandThighOrShare);
            db.AddInParameter(dbCommand, "IsSellNull", DbType.Int32, model.IsSellNull);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.HKStockInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HKStockInfo set ");
            strSql.Append("StockName=@StockName,");
            strSql.Append("StockPinYin=@StockPinYin,");
            strSql.Append("turnovervolume=@turnovervolume,");
            strSql.Append("Paydt=@Paydt,");
            strSql.Append("Nindcd=@Nindcd,");
            strSql.Append("PerHandThighOrShare=@PerHandThighOrShare,");
            strSql.Append("IsSellNull=@IsSellNull");
            strSql.Append(" where StockCode=@StockCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, model.StockCode);
            db.AddInParameter(dbCommand, "StockName", DbType.String, model.StockName);
            db.AddInParameter(dbCommand, "StockPinYin", DbType.String, model.StockPinYin);
            db.AddInParameter(dbCommand, "turnovervolume", DbType.Double, model.turnovervolume);
            db.AddInParameter(dbCommand, "Paydt", DbType.String, model.Paydt);
            db.AddInParameter(dbCommand, "Nindcd", DbType.String, model.Nindcd);
            db.AddInParameter(dbCommand, "PerHandThighOrShare", DbType.Int32, model.PerHandThighOrShare);
            db.AddInParameter(dbCommand, "IsSellNull", DbType.Int32, model.IsSellNull);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string StockCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HKStockInfo ");
            strSql.Append(" where StockCode=@StockCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, StockCode);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.HKStockInfo GetModel(string StockCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StockCode,StockName,StockPinYin,turnovervolume,Paydt,Nindcd,PerHandThighOrShare,IsSellNull from HKStockInfo ");
            strSql.Append(" where StockCode=@StockCode ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "StockCode", DbType.String, StockCode);
            ManagementCenter.Model.HKStockInfo model = null;
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
            strSql.Append("select StockCode,StockName,StockPinYin,turnovervolume,Paydt,Nindcd,PerHandThighOrShare,IsSellNull ");
            strSql.Append(" FROM HKStockInfo ");
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
        public List<ManagementCenter.Model.HKStockInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StockCode,StockName,StockPinYin,turnovervolume,Paydt,Nindcd,PerHandThighOrShare,IsSellNull ");
            strSql.Append(" FROM HKStockInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HKStockInfo> list = new List<ManagementCenter.Model.HKStockInfo>();
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
        /// 根据查询条件获取新增的港股代码信息(查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件（可为空）</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HKStockInfo> GetHKStockInfoList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StockCode,StockName,StockPinYin,turnovervolume,Paydt,Nindcd,PerHandThighOrShare,IsSellNull,BreedClassID,[CodeFromSource]=2 ");
            strSql.Append(" FROM HKStockInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HKStockInfo> list = new List<ManagementCenter.Model.HKStockInfo>();
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
        public ManagementCenter.Model.HKStockInfo ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.HKStockInfo model = new ManagementCenter.Model.HKStockInfo();
            object ojb;
            model.StockCode = dataReader["StockCode"].ToString();
            model.StockName = dataReader["StockName"].ToString();
            model.StockPinYin = dataReader["StockPinYin"].ToString();
            ojb = dataReader["turnovervolume"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.turnovervolume = (double)ojb;
            }
            model.Paydt = dataReader["Paydt"].ToString();
            model.Nindcd = dataReader["Nindcd"].ToString();
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
            ojb = dataReader["BreedClassID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreedClassID = (int)ojb;
            }
            //ojb = dataReader["CodeFormSource"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.CodeFromSource = (int)ojb;
            //}
            return model;
        }

        #endregion  成员方法
    }
}
