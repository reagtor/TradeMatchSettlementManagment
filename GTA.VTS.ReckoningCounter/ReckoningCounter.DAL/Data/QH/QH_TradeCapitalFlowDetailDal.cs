using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using ReckoningCounter.Model;

namespace ReckoningCounter.DAL.Data.QH
{
    /// <summary>
    /// 期货交易资金流水操作类
    /// </summary>
    public class QH_TradeCapitalFlowDetailDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QH_TradeCapitalFlowDetailDal()
        { }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string TradeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from QH_TradeCapitalFlowDetail where TradeID=@TradeID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.String, TradeID);
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
        public void Add(QH_TradeCapitalFlowDetailInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_TradeCapitalFlowDetail(");
            strSql.Append("TradeID,UserCapitalAccount,FlowTypes,Margin,TradeProceduresFee,ProfitLoss,OtherCose,CurrencyType,CreateDateTime)");

            strSql.Append(" values (");
            strSql.Append("@TradeID,@UserCapitalAccount,@FlowTypes,@Margin,@TradeProceduresFee,@ProfitLoss,@OtherCose,@CurrencyType,@CreateDateTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.String, model.TradeID);
            db.AddInParameter(dbCommand, "UserCapitalAccount", DbType.AnsiString, model.UserCapitalAccount);
            db.AddInParameter(dbCommand, "FlowTypes", DbType.Int32, model.FlowTypes);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OtherCose", DbType.Decimal, model.OtherCose);
            db.AddInParameter(dbCommand, "CurrencyType", DbType.Int32, model.CurrencyType);
            //db.AddInParameter(dbCommand, "FlowTotal", DbType.Decimal, model.FlowTotal);
            db.AddInParameter(dbCommand, "CreateDateTime", DbType.DateTime, model.CreateDateTime);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(QH_TradeCapitalFlowDetailInfo model, ReckoningTransaction tm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_TradeCapitalFlowDetail(");
            strSql.Append("TradeID,UserCapitalAccount,FlowTypes,Margin,TradeProceduresFee,ProfitLoss,OtherCose,CurrencyType,CreateDateTime)");

            strSql.Append(" values (");
            strSql.Append("@TradeID,@UserCapitalAccount,@FlowTypes,@Margin,@TradeProceduresFee,@ProfitLoss,@OtherCose,@CurrencyType,@CreateDateTime)");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.String, model.TradeID);
            db.AddInParameter(dbCommand, "UserCapitalAccount", DbType.AnsiString, model.UserCapitalAccount);
            db.AddInParameter(dbCommand, "FlowTypes", DbType.Int32, model.FlowTypes);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OtherCose", DbType.Decimal, model.OtherCose);
            db.AddInParameter(dbCommand, "CurrencyType", DbType.Int32, model.CurrencyType);
            //db.AddInParameter(dbCommand, "FlowTotal", DbType.Decimal, model.FlowTotal);
            db.AddInParameter(dbCommand, "CreateDateTime", DbType.DateTime, model.CreateDateTime);
            db.ExecuteNonQuery(dbCommand, tm.Transaction);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(QH_TradeCapitalFlowDetailInfo model, Database db, DbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QH_TradeCapitalFlowDetail(");
            strSql.Append("TradeID,UserCapitalAccount,FlowTypes,Margin,TradeProceduresFee,ProfitLoss,OtherCose,CurrencyType,CreateDateTime)");

            strSql.Append(" values (");
            strSql.Append("@TradeID,@UserCapitalAccount,@FlowTypes,@Margin,@TradeProceduresFee,@ProfitLoss,@OtherCose,@CurrencyType,@CreateDateTime)");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.String, model.TradeID);
            db.AddInParameter(dbCommand, "UserCapitalAccount", DbType.AnsiString, model.UserCapitalAccount);
            db.AddInParameter(dbCommand, "FlowTypes", DbType.Int32, model.FlowTypes);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OtherCose", DbType.Decimal, model.OtherCose);
            db.AddInParameter(dbCommand, "CurrencyType", DbType.Int32, model.CurrencyType);
            db.AddInParameter(dbCommand, "CreateDateTime", DbType.DateTime, model.CreateDateTime);
            try
            {
                db.ExecuteNonQuery(dbCommand, transaction);
                return true;
            }
            catch
            {

                return false;
            }

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(QH_TradeCapitalFlowDetailInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QH_TradeCapitalFlowDetail set ");
            strSql.Append("UserCapitalAccount=@UserCapitalAccount,");
            strSql.Append("FlowTypes=@FlowTypes,");
            strSql.Append("Margin=@Margin,");
            strSql.Append("TradeProceduresFee=@TradeProceduresFee,");
            strSql.Append("ProfitLoss=@ProfitLoss,");
            strSql.Append("OtherCose=@OtherCose,");
            strSql.Append("CurrencyType=@CurrencyType,");
            strSql.Append("FlowTotal=@FlowTotal");
            strSql.Append(" where TradeID=@TradeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.String, model.TradeID);
            db.AddInParameter(dbCommand, "UserCapitalAccount", DbType.AnsiString, model.UserCapitalAccount);
            db.AddInParameter(dbCommand, "FlowTypes", DbType.Int32, model.FlowTypes);
            db.AddInParameter(dbCommand, "Margin", DbType.Decimal, model.Margin);
            db.AddInParameter(dbCommand, "TradeProceduresFee", DbType.Decimal, model.TradeProceduresFee);
            db.AddInParameter(dbCommand, "ProfitLoss", DbType.Decimal, model.ProfitLoss);
            db.AddInParameter(dbCommand, "OtherCose", DbType.Decimal, model.OtherCose);
            db.AddInParameter(dbCommand, "CurrencyType", DbType.Int32, model.CurrencyType);
            db.AddInParameter(dbCommand, "FlowTotal", DbType.Decimal, model.FlowTotal);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string TradeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QH_TradeCapitalFlowDetail ");
            strSql.Append(" where TradeID=@TradeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.String, TradeID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QH_TradeCapitalFlowDetailInfo GetModel(string TradeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeID,UserCapitalAccount,FlowTypes,Margin,TradeProceduresFee,ProfitLoss,OtherCose,FlowTotal,CurrencyType,CreateDateTime from QH_TradeCapitalFlowDetail ");
            strSql.Append(" where TradeID=@TradeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "TradeID", DbType.String, TradeID);
            QH_TradeCapitalFlowDetailInfo model = null;
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
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<QH_TradeCapitalFlowDetailInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TradeID,UserCapitalAccount,FlowTypes,Margin,TradeProceduresFee,ProfitLoss,OtherCose,CurrencyType,FlowTotal,CreateDateTime ");
            strSql.Append(" FROM QH_TradeCapitalFlowDetail ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();
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
        public QH_TradeCapitalFlowDetailInfo ReaderBind(IDataReader dataReader)
        {
            QH_TradeCapitalFlowDetailInfo model = new QH_TradeCapitalFlowDetailInfo();
            object ojb;
            model.TradeID = dataReader["TradeID"].ToString();
            model.UserCapitalAccount = dataReader["UserCapitalAccount"].ToString();
            ojb = dataReader["FlowTypes"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FlowTypes = (int)ojb;
            }
            ojb = dataReader["Margin"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Margin = (decimal)ojb;
            }
            ojb = dataReader["TradeProceduresFee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TradeProceduresFee = (decimal)ojb;
            }
            ojb = dataReader["ProfitLoss"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProfitLoss = (decimal)ojb;
            }
            ojb = dataReader["OtherCose"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.OtherCose = (decimal)ojb;
            }
            ojb = dataReader["FlowTotal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FlowTotal = (decimal)ojb;
            }
            ojb = dataReader["CurrencyType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CurrencyType = (int)ojb;
            }
            ojb = dataReader["CreateDateTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreateDateTime = (DateTime)ojb;
            }
            return model;
        }

    }
}
