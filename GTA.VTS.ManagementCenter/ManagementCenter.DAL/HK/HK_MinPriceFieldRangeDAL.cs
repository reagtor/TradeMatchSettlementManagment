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
    ///描述：港股交易规则_最小变动价位范围值 数据访问类HK_MinPriceFieldRangeDAL。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_MinPriceFieldRangeDAL
    {
        public HK_MinPriceFieldRangeDAL()
        {

        }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(FieldRangeID)+1 from HK_MinPriceFieldRange";
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
        public bool Exists(int FieldRangeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_MinPriceFieldRange where FieldRangeID=@FieldRangeID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
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
        public void Add(ManagementCenter.Model.HK_MinPriceFieldRange model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_MinPriceFieldRange(");
            strSql.Append("FieldRangeID,UpperLimit,LowerLimit,Value)");

            strSql.Append(" values (");
            strSql.Append("@FieldRangeID,@UpperLimit,@LowerLimit,@Value)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, model.FieldRangeID);
            db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, model.UpperLimit);
            db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, model.LowerLimit);
            db.AddInParameter(dbCommand, "Value", DbType.Decimal, model.Value);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.HK_MinPriceFieldRange model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_MinPriceFieldRange set ");
            strSql.Append("UpperLimit=@UpperLimit,");
            strSql.Append("LowerLimit=@LowerLimit,");
            strSql.Append("Value=@Value");
            strSql.Append(" where FieldRangeID=@FieldRangeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, model.FieldRangeID);
            db.AddInParameter(dbCommand, "UpperLimit", DbType.Decimal, model.UpperLimit);
            db.AddInParameter(dbCommand, "LowerLimit", DbType.Decimal, model.LowerLimit);
            db.AddInParameter(dbCommand, "Value", DbType.Decimal, model.Value);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FieldRangeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete HK_MinPriceFieldRange ");
            strSql.Append(" where FieldRangeID=@FieldRangeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.HK_MinPriceFieldRange GetModel(int FieldRangeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FieldRangeID,UpperLimit,LowerLimit,Value from HK_MinPriceFieldRange ");
            strSql.Append(" where FieldRangeID=@FieldRangeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FieldRangeID", DbType.Int32, FieldRangeID);
            ManagementCenter.Model.HK_MinPriceFieldRange model = null;
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
            strSql.Append("select FieldRangeID,UpperLimit,LowerLimit,Value ");
            strSql.Append(" FROM HK_MinPriceFieldRange ");
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
        public List<ManagementCenter.Model.HK_MinPriceFieldRange> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FieldRangeID,UpperLimit,LowerLimit,Value ");
            strSql.Append(" FROM HK_MinPriceFieldRange ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.HK_MinPriceFieldRange> list = new List<ManagementCenter.Model.HK_MinPriceFieldRange>();
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
        public ManagementCenter.Model.HK_MinPriceFieldRange ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.HK_MinPriceFieldRange model = new ManagementCenter.Model.HK_MinPriceFieldRange();
            object ojb;
            ojb = dataReader["FieldRangeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FieldRangeID = (int)ojb;
            }
            ojb = dataReader["UpperLimit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UpperLimit = (decimal)ojb;
            }
            ojb = dataReader["LowerLimit"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LowerLimit = (decimal)ojb;
            }
            ojb = dataReader["Value"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Value = (decimal)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}
