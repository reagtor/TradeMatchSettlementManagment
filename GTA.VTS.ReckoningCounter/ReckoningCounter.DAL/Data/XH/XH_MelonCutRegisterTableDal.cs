#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类XH_MelonCutRegisterTableDal。
    /// </summary>
    public class XH_MelonCutRegisterTableDal
    {
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int MelonCutRegisterID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from XH_MelonCutRegisterTable where MelonCutRegisterID=@MelonCutRegisterID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MelonCutRegisterID", DbType.Int32, MelonCutRegisterID);
            int cmdresult;
            object obj = db.ExecuteScalar(dbCommand);
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
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
        public int Add(XH_MelonCutRegisterTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_MelonCutRegisterTable(");
            strSql.Append(
                "RegisterDate,CutDate,UserAccountDistributeLogo,TradeCurrencyType,Code,RegisterAmount,CutType,CurrencyTypeId)");

            strSql.Append(" values (");
            strSql.Append(
                "@RegisterDate,@CutDate,@UserAccountDistributeLogo,@TradeCurrencyType,@Code,@RegisterAmount,@CutType,@CurrencyTypeId)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "RegisterDate", DbType.DateTime, model.RegisterDate);
            db.AddInParameter(dbCommand, "CutDate", DbType.DateTime, model.CutDate);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "RegisterAmount", DbType.Decimal, model.RegisterAmount);
            db.AddInParameter(dbCommand, "CutType", DbType.Int32, model.CutType);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
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
        public void Update(XH_MelonCutRegisterTableInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XH_MelonCutRegisterTable set ");
            strSql.Append("RegisterDate=@RegisterDate,");
            strSql.Append("CutDate=@CutDate,");
            strSql.Append("UserAccountDistributeLogo=@UserAccountDistributeLogo,");
            strSql.Append("TradeCurrencyType=@TradeCurrencyType,");
            strSql.Append("Code=@Code,");
            strSql.Append("RegisterAmount=@RegisterAmount,");
            strSql.Append("CutType=@CutType,");
            strSql.Append("CurrencyTypeId=@CurrencyTypeId");
            strSql.Append(" where MelonCutRegisterID=@MelonCutRegisterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MelonCutRegisterID", DbType.Int32, model.MelonCutRegisterID);
            db.AddInParameter(dbCommand, "RegisterDate", DbType.DateTime, model.RegisterDate);
            db.AddInParameter(dbCommand, "CutDate", DbType.DateTime, model.CutDate);
            db.AddInParameter(dbCommand, "UserAccountDistributeLogo", DbType.AnsiString, model.UserAccountDistributeLogo);
            db.AddInParameter(dbCommand, "TradeCurrencyType", DbType.Int32, model.TradeCurrencyType);
            db.AddInParameter(dbCommand, "Code", DbType.AnsiString, model.Code);
            db.AddInParameter(dbCommand, "RegisterAmount", DbType.Decimal, model.RegisterAmount);
            db.AddInParameter(dbCommand, "CutType", DbType.Int32, model.CutType);
            db.AddInParameter(dbCommand, "CurrencyTypeId", DbType.Int32, model.CurrencyTypeId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int MelonCutRegisterID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_MelonCutRegisterTable ");
            strSql.Append(" where MelonCutRegisterID=@MelonCutRegisterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MelonCutRegisterID", DbType.Int32, MelonCutRegisterID);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="db">Database</param>
        /// <param name="transaction">DbTransaction</param>
        public void Delete(XH_MelonCutRegisterTableInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XH_MelonCutRegisterTable ");
            strSql.Append(" where MelonCutRegisterID=@MelonCutRegisterID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MelonCutRegisterID", DbType.Int32, model.MelonCutRegisterID);
            db.ExecuteNonQuery(dbCommand, transaction);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XH_MelonCutRegisterTableInfo GetModel(int MelonCutRegisterID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select MelonCutRegisterID,RegisterDate,CutDate,UserAccountDistributeLogo,TradeCurrencyType,Code,RegisterAmount,CutType,CurrencyTypeId from XH_MelonCutRegisterTable ");
            strSql.Append(" where MelonCutRegisterID=@MelonCutRegisterID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "MelonCutRegisterID", DbType.Int32, MelonCutRegisterID);
            XH_MelonCutRegisterTableInfo model = null;
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
        public List<XH_MelonCutRegisterTableInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                "select MelonCutRegisterID,RegisterDate,CutDate,UserAccountDistributeLogo,TradeCurrencyType,Code,RegisterAmount,CutType,CurrencyTypeId ");
            strSql.Append(" FROM XH_MelonCutRegisterTable ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<XH_MelonCutRegisterTableInfo> list = new List<XH_MelonCutRegisterTableInfo>();
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
        public XH_MelonCutRegisterTableInfo ReaderBind(IDataReader dataReader)
        {
            XH_MelonCutRegisterTableInfo model = new XH_MelonCutRegisterTableInfo();
            object ojb;
            ojb = dataReader["MelonCutRegisterID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MelonCutRegisterID = (int) ojb;
            }
            ojb = dataReader["RegisterDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RegisterDate = (DateTime) ojb;
            }
            ojb = dataReader["CutDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CutDate = (DateTime) ojb;
            }
            model.UserAccountDistributeLogo = dataReader["UserAccountDistributeLogo"].ToString();
            ojb = dataReader["TradeCurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeCurrencyType = (int) ojb;
            }
            model.Code = dataReader["Code"].ToString();
            ojb = dataReader["RegisterAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RegisterAmount = (decimal) ojb;
            }
            ojb = dataReader["CutType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CutType = (int) ojb;
            }
            ojb = dataReader["CurrencyTypeId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyTypeId = (int) ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}