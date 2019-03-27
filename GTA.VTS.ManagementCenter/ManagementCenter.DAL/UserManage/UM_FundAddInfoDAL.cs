using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Maticsoft.DBUtility;//请先添加引用
namespace ManagementCenter.DAL
{
	/// <summary>
    /// 描述：资金明细表 数据访问类UM_FundAddInfoDAL。
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
	public class UM_FundAddInfoDAL
	{
		public UM_FundAddInfoDAL()
		{}
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(AddFundID)+1 from UM_FundAddInfo";
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
        public bool Exists(int AddFundID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UM_FundAddInfo where AddFundID=@AddFundID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AddFundID", DbType.Int32, AddFundID);
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
        public int Add(ManagementCenter.Model.UM_FundAddInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UM_FundAddInfo(");
            strSql.Append("ManagerID,UserID,USNumber,RMBNumber,AddTime,HKNumber,Remark)");

            strSql.Append(" values (");
            strSql.Append("@ManagerID,@UserID,@USNumber,@RMBNumber,@AddTime,@HKNumber,@Remark)");
            strSql.Append(";select @@IDENTITY");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "ManagerID", DbType.Int32, model.ManagerID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
            db.AddInParameter(dbCommand, "USNumber", DbType.Decimal, model.USNumber);
            db.AddInParameter(dbCommand, "RMBNumber", DbType.Decimal, model.RMBNumber);
            db.AddInParameter(dbCommand, "AddTime", DbType.DateTime, model.AddTime);
            db.AddInParameter(dbCommand, "HKNumber", DbType.Decimal, model.HKNumber);
            db.AddInParameter(dbCommand, "Remark", DbType.String, model.Remark);
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
        public void Update(ManagementCenter.Model.UM_FundAddInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UM_FundAddInfo set ");
            strSql.Append("ManagerID=@ManagerID,");
            strSql.Append("UserID=@UserID,");
            strSql.Append("USNumber=@USNumber,");
            strSql.Append("RMBNumber=@RMBNumber,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("HKNumber=@HKNumber,");
            strSql.Append("Remark=@Remark");
            strSql.Append(" where AddFundID=@AddFundID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AddFundID", DbType.Int32, model.AddFundID);
            db.AddInParameter(dbCommand, "ManagerID", DbType.Int32, model.ManagerID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
            db.AddInParameter(dbCommand, "USNumber", DbType.Decimal, model.USNumber);
            db.AddInParameter(dbCommand, "RMBNumber", DbType.Decimal, model.RMBNumber);
            db.AddInParameter(dbCommand, "AddTime", DbType.DateTime, model.AddTime);
            db.AddInParameter(dbCommand, "HKNumber", DbType.Decimal, model.HKNumber);
            db.AddInParameter(dbCommand, "Remark", DbType.String, model.Remark);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int AddFundID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_FundAddInfo ");
            strSql.Append(" where AddFundID=@AddFundID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AddFundID", DbType.Int32, AddFundID);
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_FundAddInfo GetModel(int AddFundID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AddFundID,ManagerID,UserID,USNumber,RMBNumber,AddTime,HKNumber,Remark from UM_FundAddInfo ");
            strSql.Append(" where AddFundID=@AddFundID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AddFundID", DbType.Int32, AddFundID);
            ManagementCenter.Model.UM_FundAddInfo model = null;
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
            strSql.Append("select AddFundID,ManagerID,UserID,USNumber,RMBNumber,AddTime,HKNumber,Remark ");
            strSql.Append(" FROM UM_FundAddInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteDataSet(CommandType.Text, strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("UP_GetRecordByPage");
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_FundAddInfo");
            db.AddInParameter(dbCommand, "fldName", DbType.AnsiString, "ID");
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddInParameter(dbCommand, "IsReCount", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "OrderType", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "strWhere", DbType.AnsiString, strWhere);
            return db.ExecuteDataSet(dbCommand);
        }*/

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<ManagementCenter.Model.UM_FundAddInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AddFundID,ManagerID,UserID,USNumber,RMBNumber,AddTime,HKNumber,Remark ");
            strSql.Append(" FROM UM_FundAddInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.UM_FundAddInfo> list = new List<ManagementCenter.Model.UM_FundAddInfo>();
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
        public ManagementCenter.Model.UM_FundAddInfo ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.UM_FundAddInfo model = new ManagementCenter.Model.UM_FundAddInfo();
            object ojb;
            ojb = dataReader["AddFundID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AddFundID = (int)ojb;
            }
            ojb = dataReader["ManagerID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ManagerID = (int)ojb;
            }
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
            }
            ojb = dataReader["USNumber"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.USNumber = (decimal)ojb;
            }
            ojb = dataReader["RMBNumber"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RMBNumber = (decimal)ojb;
            }
            ojb = dataReader["AddTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AddTime = (DateTime)ojb;
            }
            ojb = dataReader["HKNumber"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.HKNumber = (decimal)ojb;
            }
            model.Remark = dataReader["Remark"].ToString();
            return model;
        }


        #region 追加资金历史分页查询
        /// <summary>
        /// 追加资金历史分页查询
        /// </summary>
        /// <param name="fundAddQueryEntity">追加资金实体</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetPagingFund(ManagementCenter.Model.UserManage.FundAddQueryEntity fundAddQueryEntity, int pageNo, int pageSize,
                                        out int rowCount)
        {
            string SQL_SELECT_Fund =
                @" select AddFundID,ManagerID,UM_FundAddInfo.UserID,USNumber,RMBNumber,UM_FundAddInfo.AddTime,
                                        HKNumber,UM_FundAddInfo.Remark,LoginName 
                                                    FROM UM_FundAddInfo,UM_UserInfo
                                                    WHERE UM_FundAddInfo.ManagerID=UM_UserInfo.UserID ";
            if (fundAddQueryEntity.UserID != int.MaxValue)
            {
                SQL_SELECT_Fund += "AND UM_FundAddInfo.UserID=@UserID ";
            }
            //if (fundAddQueryEntity.ManagerID != int.MaxValue)
            //{
            //    SQL_SELECT_Fund += "AND ManagerID=@ManagerID ";
            //}
            if (fundAddQueryEntity.loginName != null && !string.IsNullOrEmpty(fundAddQueryEntity.loginName))
            {
                SQL_SELECT_Fund += "AND LoginName LIKE  '%' + @LoginName + '%' ";
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_Fund);
            if (fundAddQueryEntity.UserID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "UserID", DbType.Int32, fundAddQueryEntity.UserID);
            }
            //if (fundAddQueryEntity.ManagerID != int.MaxValue)
            //{
            //    database.AddInParameter(dbCommand, "ManagerID", DbType.Int32, fundAddQueryEntity.ManagerID);
            //}
            //登陆名称(根据要求把按管理员编号查询改成按管理员登陆名称查询)
            if (fundAddQueryEntity.loginName != null && !string.IsNullOrEmpty(fundAddQueryEntity.loginName))
            {
                database.AddInParameter(dbCommand, "LoginName", DbType.String, fundAddQueryEntity.loginName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_Fund, pageNo, pageSize, out rowCount,
                                        "UM_FundAddInfo");
        }

        #endregion

        #endregion  成员方法
	}
}

