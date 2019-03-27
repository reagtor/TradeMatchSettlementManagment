using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility; //请先添加引用

namespace ManagementCenter.DAL
{
    /// <summary>
    ///描述：交易所类型 数据访问类CM_BourseTypeDAL。
    ///作者：刘书伟
    ///日期:2008-11-20
    ///Desc: 增加了代码规则字段 CodeRulesType
    ///Update By: 董鹏
    ///Update Date: 2010-03-10
    /// </summary>
    public class CM_BourseTypeDAL
    {
        public CM_BourseTypeDAL()
        {
        }

        #region SQL
        /// <summary>
        /// 返回交易所信息
        /// </summary>
        private string SQL_SELECT_CMBOURSETYPE = @"SELECT * FROM CM_BOURSETYPE WHERE DELETESTATE IS NOT NULL AND DELETESTATE<>1 ";// 1=1

        /// <summary>
        /// 获取交易所类型名称
        /// </summary>
        private string SQL_SELECTBOURSETYPENAME_CMBOURSETYPE = @"SELECT BOURSETYPEID,BOURSETYPENAME FROM CM_BOURSETYPE 
                                                                 WHERE DELETESTATE IS NOT NULL AND DELETESTATE<>1 ";

        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(BourseTypeID)+1 from CM_BourseType";
            Database db = DatabaseFactory.CreateDatabase();
            object obj = db.ExecuteScalar(CommandType.Text, strsql);
            if (obj != null && obj != DBNull.Value)
            {
                return int.Parse(obj.ToString());
            }
            return AppGlobalVariable.INIT_INT;//1;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BourseTypeID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CM_BourseType where BourseTypeID=@BourseTypeID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, BourseTypeID);
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
        /// 添加交易所类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.CM_BourseType model)
        {
            return Add(model, null, null);
        }

        /// <summary>
        /// 添加交易所类型
        /// </summary>
        /// <param name="model">交易所类型实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.CM_BourseType model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SET IDENTITY_INSERT [CM_BourseType] ON; ");//关闭自动增长列(因为系统默认前100ID属于系统ID)
            strSql.Append("insert into CM_BourseType(");
            strSql.Append("BourseTypeID,BourseTypeName,ReceivingConsignStartTime,ReceivingConsignEndTime,CounterFromSubmitStartTime,CounterFromSubmitEndTime,ISSysDefaultBourseType,DeleteState,CodeRulesType)");
            strSql.Append(" values (");
            strSql.Append("@BourseTypeID,@BourseTypeName,@ReceivingConsignStartTime,@ReceivingConsignEndTime,@CounterFromSubmitStartTime,@CounterFromSubmitEndTime,@ISSysDefaultBourseType,@DeleteState,@CodeRulesType)");
            //strSql.Append(";select @@IDENTITY");
            strSql.Append(";SET IDENTITY_INSERT [CM_BourseType] OFF");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            db.AddInParameter(dbCommand, "BourseTypeName", DbType.String, model.BourseTypeName);
            db.AddInParameter(dbCommand, "ReceivingConsignStartTime", DbType.DateTime, model.ReceivingConsignStartTime);
            db.AddInParameter(dbCommand, "ReceivingConsignEndTime", DbType.DateTime, model.ReceivingConsignEndTime);
            db.AddInParameter(dbCommand, "CounterFromSubmitStartTime", DbType.DateTime, model.CounterFromSubmitStartTime);
            db.AddInParameter(dbCommand, "CounterFromSubmitEndTime", DbType.DateTime, model.CounterFromSubmitEndTime);
            db.AddInParameter(dbCommand, "ISSysDefaultBourseType", DbType.Int32, model.ISSysDefaultBourseType);
            model.DeleteState = (int)Types.IsYesOrNo.No;//添加时默认状态为非删除
            db.AddInParameter(dbCommand, "DeleteState", DbType.Int32, model.DeleteState);
            db.AddInParameter(dbCommand, "CodeRulesType", DbType.Int32, model.CodeRulesType);
            
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand, tran);
            }
            //if (!int.TryParse(obj.ToString(), out result))
            //{
            //    return AppGlobalVariable.INIT_INT;
            //}
           // return result;
            return  model.BourseTypeID;
        }

        /// <summary>
        /// 更新交易所类型
        /// </summary>
        /// <param name="model">交易所类型实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.CM_BourseType model)
        {
            return Update(model, null, null);
        }

        /// <summary>
        /// 更新交易所类型
        /// </summary>
        /// <param name="model">交易所类型实体</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.CM_BourseType model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CM_BourseType set ");
            strSql.Append("BourseTypeName=@BourseTypeName,");
            //strSql.Append("ReceivingConsignStartTime=@ReceivingConsignStartTime,");
            //strSql.Append("ReceivingConsignEndTime=@ReceivingConsignEndTime,");
            strSql.Append("CounterFromSubmitStartTime=@CounterFromSubmitStartTime,");
            strSql.Append("CounterFromSubmitEndTime=@CounterFromSubmitEndTime");
            strSql.Append(" where BourseTypeID=@BourseTypeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, model.BourseTypeID);
            db.AddInParameter(dbCommand, "BourseTypeName", DbType.String, model.BourseTypeName);
            //db.AddInParameter(dbCommand, "ReceivingConsignStartTime", DbType.DateTime, model.ReceivingConsignStartTime);
            //db.AddInParameter(dbCommand, "ReceivingConsignEndTime", DbType.DateTime, model.ReceivingConsignEndTime);
            db.AddInParameter(dbCommand, "CounterFromSubmitStartTime", DbType.DateTime, model.CounterFromSubmitStartTime);
            db.AddInParameter(dbCommand, "CounterFromSubmitEndTime", DbType.DateTime, model.CounterFromSubmitEndTime);
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
        /// 更新交易所类型的撮合接收委托开始时间和撮合接收委托结束时间（根据交易开始和结束时间更新）
        /// </summary>
        /// <param name="bourseTypeID">交易所类型ID</param>
        /// <param name="receivingConsignStartTime">撮合接收委托开始时间</param>
        /// <param name="receivingConsignEndTime">撮合接收委托结束时间</param>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool Update(int bourseTypeID, DateTime receivingConsignStartTime, DateTime receivingConsignEndTime, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CM_BourseType set ");
            strSql.Append("ReceivingConsignStartTime=@receivingConsignStartTime,");
            strSql.Append("ReceivingConsignEndTime=@receivingConsignEndTime");
            strSql.Append(" where BourseTypeID=@bourseTypeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, bourseTypeID);
            db.AddInParameter(dbCommand, "ReceivingConsignStartTime", DbType.DateTime, receivingConsignStartTime);
            db.AddInParameter(dbCommand, "ReceivingConsignEndTime", DbType.DateTime, receivingConsignEndTime);
            // db.ExecuteNonQuery(dbCommand);
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
        /// 更新交易所类型的撮合接收委托开始时间和撮合接收委托结束时间（根据交易开始和结束时间更新）
        /// </summary>
        /// <param name="bourseTypeID">交易所类型ID</param>
        /// <param name="receivingConsignStartTime">撮合接收委托开始时间</param>
        /// <param name="receivingConsignEndTime">撮合接收委托结束时间</param>
        /// <returns></returns>
        public bool Update(int bourseTypeID, DateTime receivingConsignStartTime, DateTime receivingConsignEndTime)
        {
            return Update(bourseTypeID, receivingConsignStartTime, receivingConsignEndTime, null, null);
        }

        #region 根据交易所类型ID，删除交易所(有事务)
        /// <summary>
        /// 根据交易所类型ID，删除交易所(有事务)
        /// </summary>
        /// <param name="BourseTypeID">交易所类型ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <returns></returns>
        public bool Delete(int BourseTypeID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete CM_BourseType ");
            strSql.Append(" where BourseTypeID=@BourseTypeID ");
            if (db == null)
            {
                db = DatabaseFactory.CreateDatabase();
            }
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, BourseTypeID);
            // db.ExecuteNonQuery(dbCommand);
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


        #region 根据交易所类型ID，删除交易所(无事务)
        /// <summary>
        /// 根据交易所类型ID，删除交易所(无事务)
        /// </summary>
        /// <param name="BourseTypeID">交易所类型ID</param>
        /// <returns></returns>
        public bool Delete(int BourseTypeID)
        {
            return Delete(BourseTypeID, null, null);
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_BourseType GetModel(int BourseTypeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BourseTypeID,BourseTypeName,ReceivingConsignStartTime,ReceivingConsignEndTime,CounterFromSubmitStartTime,CounterFromSubmitEndTime,ISSysDefaultBourseType,DeleteState,CodeRulesType from CM_BourseType ");
            strSql.Append(" where BourseTypeID=@BourseTypeID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "BourseTypeID", DbType.Int32, BourseTypeID);
            ManagementCenter.Model.CM_BourseType model = null;
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
            strSql.Append("select BourseTypeID,BourseTypeName,ReceivingConsignStartTime,ReceivingConsignEndTime,CounterFromSubmitStartTime,CounterFromSubmitEndTime,ISSysDefaultBourseType,DeleteState,CodeRulesType ");
            strSql.Append(" FROM CM_BourseType ");
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
        public List<ManagementCenter.Model.CM_BourseType> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BourseTypeID,BourseTypeName,ReceivingConsignStartTime,ReceivingConsignEndTime,CounterFromSubmitStartTime,CounterFromSubmitEndTime,ISSysDefaultBourseType,DeleteState,CodeRulesType ");
            strSql.Append(" FROM CM_BourseType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.CM_BourseType> list = new List<ManagementCenter.Model.CM_BourseType>();
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

        #region 获取所有交易所类型

        /// <summary>
        /// 获取所有交易所类型
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMBourseType(string BourseTypeName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            //条件查询
            if (BourseTypeName != AppGlobalVariable.INIT_STRING && !string.IsNullOrEmpty(BourseTypeName))
            {
                SQL_SELECT_CMBOURSETYPE += "AND (BourseTypeName LIKE  '%' + @BourseTypeName + '%') ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_CMBOURSETYPE);

            if (BourseTypeName != AppGlobalVariable.INIT_STRING && BourseTypeName != string.Empty)
            {
                database.AddInParameter(dbCommand, "BourseTypeName", DbType.String, BourseTypeName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_CMBOURSETYPE, pageNo, pageSize,
                                        out rowCount, "CM_BourseType");
        }

        #endregion

        #region 获取交易所类型名称
        /// <summary>
        /// 获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetBourseTypeName()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECTBOURSETYPENAME_CMBOURSETYPE);
            return database.ExecuteDataSet(dbCommand);
        }
        #endregion


        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.CM_BourseType ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.CM_BourseType model = new ManagementCenter.Model.CM_BourseType();
            object ojb;
            ojb = dataReader["BourseTypeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BourseTypeID = (int)ojb;
            }
            model.BourseTypeName = dataReader["BourseTypeName"].ToString();
            ojb = dataReader["ReceivingConsignStartTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ReceivingConsignStartTime = (DateTime)ojb;
            }
            ojb = dataReader["ReceivingConsignEndTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ReceivingConsignEndTime = (DateTime)ojb;
            }
            ojb = dataReader["CounterFromSubmitStartTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CounterFromSubmitStartTime = (DateTime)ojb;
            }
            ojb = dataReader["CounterFromSubmitEndTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CounterFromSubmitEndTime = (DateTime)ojb;
            }
            ojb = dataReader["ISSysDefaultBourseType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ISSysDefaultBourseType = (int)ojb;
            }
            ojb = dataReader["DeleteState"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DeleteState = (int)ojb;
            }
            ojb = dataReader["CodeRulesType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CodeRulesType = (int)ojb;
            }
            return model;
        }

        #endregion  成员方法
    }
}