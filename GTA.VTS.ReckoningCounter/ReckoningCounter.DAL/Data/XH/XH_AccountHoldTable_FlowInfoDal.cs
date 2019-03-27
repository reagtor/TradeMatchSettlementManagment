using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;

namespace ReckoningCounter.DAL.Data.XH
{
    /// <summary>
    /// 数据访问类XH_AccountHoldTable_FlowInfoDal。
    /// </summary>
    public class XH_AccountHoldTable_FlowInfoDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_AccountHoldTable_FlowInfoDal()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_AccountHoldTable_Flow where ID=@ID ");
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
        public int Add(XH_AccountHoldTable_FlowInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AccountHoldTable_Flow(");
            strSql.Append("AccountHoldLogoId,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice,FlowTime)");

            strSql.Append(" values (");
            strSql.Append("@AccountHoldLogoId,@AvailableAmount,@FreezeAmount,@CostPrice,@BreakevenPrice,@HoldAveragePrice,@FlowTime)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "FlowTime", DbType.DateTime, model.FlowTime);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(XH_AccountHoldTable_FlowInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_AccountHoldTable_Flow(");
            strSql.Append("AccountHoldLogoId,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice,FlowTime)");

            strSql.Append(" values (");
            strSql.Append("@AccountHoldLogoId,@AvailableAmount,@FreezeAmount,@CostPrice,@BreakevenPrice,@HoldAveragePrice,@FlowTime)");
            strSql.Append(";select @@IDENTITY");
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "FlowTime", DbType.DateTime, model.FlowTime);
            int result;
            object obj = db.ExecuteScalar(dbCommand, transaction);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(XH_AccountHoldTable_FlowInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_AccountHoldTable_Flow set ");
            strSql.Append("AccountHoldLogoId=@AccountHoldLogoId,");
            strSql.Append("AvailableAmount=@AvailableAmount,");
            strSql.Append("FreezeAmount=@FreezeAmount,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("BreakevenPrice=@BreakevenPrice,");
            strSql.Append("HoldAveragePrice=@HoldAveragePrice,");
            strSql.Append("FlowTime=@FlowTime");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, model.ID);
            db.AddInParameter(dbCommand, "AccountHoldLogoId", DbType.Int32, model.AccountHoldLogoId);
            db.AddInParameter(dbCommand, "AvailableAmount", DbType.Decimal, model.AvailableAmount);
            db.AddInParameter(dbCommand, "FreezeAmount", DbType.Decimal, model.FreezeAmount);
            db.AddInParameter(dbCommand, "CostPrice", DbType.Decimal, model.CostPrice);
            db.AddInParameter(dbCommand, "BreakevenPrice", DbType.Decimal, model.BreakevenPrice);
            db.AddInParameter(dbCommand, "HoldAveragePrice", DbType.Decimal, model.HoldAveragePrice);
            db.AddInParameter(dbCommand, "FlowTime", DbType.DateTime, model.FlowTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_AccountHoldTable_Flow ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除全部数据
        /// </summary>
        public void Delete(Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_AccountHoldTable_Flow ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_AccountHoldTable_FlowInfo GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,AccountHoldLogoId,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice,FlowTime from XH_AccountHoldTable_Flow ");
            strSql.Append(" where ID=@ID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);
            XH_AccountHoldTable_FlowInfo model = null;
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
            strSql.Append("select ID,AccountHoldLogoId,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice,FlowTime ");
            strSql.Append(" FROM XH_AccountHoldTable_Flow ");
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
        public List<XH_AccountHoldTable_FlowInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,AccountHoldLogoId,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice,FlowTime ");
            strSql.Append(" FROM XH_AccountHoldTable_Flow ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_AccountHoldTable_FlowInfo> list = new List<XH_AccountHoldTable_FlowInfo>();
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
        /// 获取全部记录
        /// </summary>
        /// <returns></returns>
        public List<XH_AccountHoldTable_FlowInfo> GetAllLast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,AccountHoldLogoId,AvailableAmount,FreezeAmount,CostPrice,BreakevenPrice,HoldAveragePrice,FlowTime ");
            strSql.Append(" FROM XH_AccountHoldTable_Flow x");
            //strSql.Append("select * from XH_AccountHoldTable_Flow x");
            strSql.Append(" where ID =(");
            strSql.Append("select top 1 ID");
            strSql.Append(" from XH_AccountHoldTable_Flow y");
            strSql.Append(" where y.AccountHoldLogoId=x.AccountHoldLogoId");
            strSql.Append(" order by FlowTime desc)");

            List<XH_AccountHoldTable_FlowInfo> list = new List<XH_AccountHoldTable_FlowInfo>();
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
        public XH_AccountHoldTable_FlowInfo ReaderBind(IDataReader dataReader)
        {
            XH_AccountHoldTable_FlowInfo model = new XH_AccountHoldTable_FlowInfo();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["AccountHoldLogoId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AccountHoldLogoId = (int)ojb;
            }
            ojb = dataReader["AvailableAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AvailableAmount = (decimal)ojb;
            }
            ojb = dataReader["FreezeAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreezeAmount = (decimal)ojb;
            }
            ojb = dataReader["CostPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CostPrice = (decimal)ojb;
            }
            ojb = dataReader["BreakevenPrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BreakevenPrice = (decimal)ojb;
            }
            ojb = dataReader["HoldAveragePrice"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HoldAveragePrice = (decimal)ojb;
            }
            ojb = dataReader["FlowTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FlowTime = (DateTime)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}
