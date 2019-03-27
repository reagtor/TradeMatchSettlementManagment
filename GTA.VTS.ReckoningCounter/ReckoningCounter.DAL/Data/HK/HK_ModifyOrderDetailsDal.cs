using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.DAL.Data.HK
{
    ///<summary>
    /// Title:港股改单记录明细数据层操作类
    /// Desc.:本实体类只对已经改单成功后的新委托与被修改的委托单号作一一对应的记录。
    ///       不成功的记录应在委托记录请求表中查询，而委托表中记录的改单记录只是对已当前的委托
    ///       最后一次的改单委托
    /// 数据访问类HK_ModifyOrderDetailsDal。
    /// Create By:李健华
    /// Create Date:2009-11-10
    /// </summary>
    public class HK_ModifyOrderDetailsDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_ModifyOrderDetailsDal()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from HK_ModifyOrderDetails where ID=@ID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
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
        public int Add(HK_ModifyOrderDetailsInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HK_ModifyOrderDetails(");
            strSql.Append("NewRequestNumber,OriginalRequestNumber,ModifyType,CreateDate)");

            strSql.Append(" values (");
            strSql.Append("@NewRequestNumber,@OriginalRequestNumber,@ModifyType,@CreateDate)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "NewRequestNumber", DbType.AnsiString, model.NewRequestNumber);
            db.AddInParameter(dbCommand, "OriginalRequestNumber", DbType.AnsiString, model.OriginalRequestNumber);
            db.AddInParameter(dbCommand, "ModifyType", DbType.Int32, model.ModifyType);
            db.AddInParameter(dbCommand, "CreateDate", DbType.DateTime, model.CreateDate);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(HK_ModifyOrderDetailsInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HK_ModifyOrderDetails set ");
            strSql.Append("NewRequestNumber=@NewRequestNumber,");
            strSql.Append("OriginalRequestNumber=@OriginalRequestNumber,");
            strSql.Append("ModifyType=@ModifyType,");
            strSql.Append("CreateDate=@CreateDate");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
            db.AddInParameter(dbCommand, "NewRequestNumber", DbType.AnsiString, model.NewRequestNumber);
            db.AddInParameter(dbCommand, "OriginalRequestNumber", DbType.AnsiString, model.OriginalRequestNumber);
            db.AddInParameter(dbCommand, "ModifyType", DbType.Int32, model.ModifyType);
            db.AddInParameter(dbCommand, "CreateDate", DbType.DateTime, model.CreateDate);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from HK_ModifyOrderDetails ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public HK_ModifyOrderDetailsInfo GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NewRequestNumber,OriginalRequestNumber,ModifyType,CreateDate from HK_ModifyOrderDetails ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            HK_ModifyOrderDetailsInfo model = null;
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
            strSql.Append("select ID,NewRequestNumber,OriginalRequestNumber,ModifyType,CreateDate ");
            strSql.Append(" FROM HK_ModifyOrderDetails ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }


        /// <summary>
        /// 获得数据列表 
        /// </summary>
        public List<HK_ModifyOrderDetailsInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NewRequestNumber,OriginalRequestNumber,ModifyType,CreateDate ");
            strSql.Append(" FROM HK_ModifyOrderDetails ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<HK_ModifyOrderDetailsInfo> list = new List<HK_ModifyOrderDetailsInfo>();
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
        public HK_ModifyOrderDetailsInfo ReaderBind(IDataReader dataReader)
        {
            HK_ModifyOrderDetailsInfo model = new HK_ModifyOrderDetailsInfo();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            model.NewRequestNumber = dataReader["NewRequestNumber"].ToString();
            model.OriginalRequestNumber = dataReader["OriginalRequestNumber"].ToString();
            ojb = dataReader["ModifyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifyType = (int)ojb;
            }
            ojb = dataReader["CreateDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreateDate = (DateTime)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}
