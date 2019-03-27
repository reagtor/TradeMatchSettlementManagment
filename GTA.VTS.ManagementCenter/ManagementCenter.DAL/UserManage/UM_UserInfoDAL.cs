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
    /// 描述：用户基本信息表 数据访问类UM_UserInfoDAL。
    /// 作者：熊晓凌        
    /// 日期：2008-11-18         
    /// 修改：叶振东
    /// 修改日期：2009-12-23
    /// </summary>
    public class UM_UserInfoDAL
    {
        public UM_UserInfoDAL()
        { }
        //=================================================公共成员方法==========================================

        #region 得到最大ID
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            string strsql = "select max(UserID)+1 from UM_UserInfo";
            Database db = DatabaseFactory.CreateDatabase();
            object obj = db.ExecuteScalar(CommandType.Text, strsql);
            if (obj != null && obj != DBNull.Value)
            {
                return int.Parse(obj.ToString());
            }
            return 1;
        }
        #endregion

        #region 是否存在该记录
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UM_UserInfo where UserID=@UserID ");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
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
        #endregion

        #region 增加一条数据
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.UM_UserInfo model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UM_UserInfo(");
            strSql.Append("UserName,LoginName,Password,CertificateStyle,Postalcode,RoleID,CertificateNo,Telephone,Address,Email,QuestionID,Answer,CouterID,Remark,AddType,AddTime)");

            strSql.Append(" values (");
            strSql.Append("@UserName,@LoginName,@Password,@CertificateStyle,@Postalcode,@RoleID,@CertificateNo,@Telephone,@Address,@Email,@QuestionID,@Answer,@CouterID,@Remark,@AddType,@AddTime)");
            strSql.Append(";select @@IDENTITY");

            if (db == null) db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserName", DbType.String, model.UserName);
            db.AddInParameter(dbCommand, "LoginName", DbType.String, model.LoginName);
            db.AddInParameter(dbCommand, "Password", DbType.String, UtilityClass.DesEncrypt(model.Password, string.Empty));
            db.AddInParameter(dbCommand, "CertificateStyle", DbType.Int32, model.CertificateStyle);
            db.AddInParameter(dbCommand, "Postalcode", DbType.String, model.Postalcode);
            db.AddInParameter(dbCommand, "RoleID", DbType.Int32, model.RoleID);
            db.AddInParameter(dbCommand, "CertificateNo", DbType.String, model.CertificateNo);
            db.AddInParameter(dbCommand, "Telephone", DbType.String, model.Telephone);
            db.AddInParameter(dbCommand, "Address", DbType.String, model.Address);
            db.AddInParameter(dbCommand, "Email", DbType.String, model.Email);
            db.AddInParameter(dbCommand, "QuestionID", DbType.Int32, model.QuestionID);
            db.AddInParameter(dbCommand, "Answer", DbType.String, model.Answer);
            db.AddInParameter(dbCommand, "CouterID", DbType.Int32, model.CouterID);
            db.AddInParameter(dbCommand, "Remark", DbType.String, model.Remark);
            db.AddInParameter(dbCommand, "AddType", DbType.Int32, model.AddType);
            db.AddInParameter(dbCommand, "AddTime", DbType.DateTime, System.DateTime.Now);
            int result;
            object obj;
            if (tran == null)
            {
                obj = db.ExecuteScalar(dbCommand);
            }
            else
            {
                obj = db.ExecuteScalar(dbCommand, tran);
            }
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.UM_UserInfo model)
        {
            return Add(model, null, null);
        }
        #endregion

        #region 更新一条数据
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.UM_UserInfo model, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UM_UserInfo set ");
            strSql.Append("UserName=@UserName,");
            strSql.Append("LoginName=@LoginName,");
            strSql.Append("Password=@Password,");
            strSql.Append("CertificateStyle=@CertificateStyle,");
            strSql.Append("Postalcode=@Postalcode,");
            strSql.Append("RoleID=@RoleID,");
            strSql.Append("CertificateNo=@CertificateNo,");
            strSql.Append("Telephone=@Telephone,");
            strSql.Append("Address=@Address,");
            strSql.Append("Email=@Email,");
            strSql.Append("QuestionID=@QuestionID,");
            strSql.Append("Answer=@Answer,");
            strSql.Append("CouterID=@CouterID,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("AddType=@AddType,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where UserID=@UserID ");

            if (db == null) db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserName", DbType.String, model.UserName);
            db.AddInParameter(dbCommand, "LoginName", DbType.String, model.LoginName);
            db.AddInParameter(dbCommand, "Password", DbType.String, model.Password);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, model.UserID);
            db.AddInParameter(dbCommand, "CertificateStyle", DbType.Int32, model.CertificateStyle);
            db.AddInParameter(dbCommand, "Postalcode", DbType.String, model.Postalcode);
            db.AddInParameter(dbCommand, "RoleID", DbType.Int32, model.RoleID);
            db.AddInParameter(dbCommand, "CertificateNo", DbType.String, model.CertificateNo);
            db.AddInParameter(dbCommand, "Telephone", DbType.String, model.Telephone);
            db.AddInParameter(dbCommand, "Address", DbType.String, model.Address);
            db.AddInParameter(dbCommand, "Email", DbType.String, model.Email);
            db.AddInParameter(dbCommand, "QuestionID", DbType.Int32, model.QuestionID);
            db.AddInParameter(dbCommand, "Answer", DbType.String, model.Answer);
            db.AddInParameter(dbCommand, "CouterID", DbType.Int32, model.CouterID);
            db.AddInParameter(dbCommand, "Remark", DbType.String, model.Remark);
            db.AddInParameter(dbCommand, "AddType", DbType.Int32, model.AddType);
            db.AddInParameter(dbCommand, "AddTime", DbType.DateTime, model.AddTime);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        public void Update(ManagementCenter.Model.UM_UserInfo model)
        {
            Update(model, null, null);
        }

        /// <summary>
        /// 根据用户ID更新用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        public void UpdateDelState(int UserID, DbTransaction tran, Database db)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UM_UserInfo set ");
            strSql.Append("AddType=@AddType ");
            strSql.Append(" where UserID=@UserID AND RoleID=@RoleID AND AddType=@OldType");

            if (db == null) db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "AddType", DbType.Int32, (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.FrontTarnDelState);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "RoleID", DbType.Int32, (int)ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Transaction);
            db.AddInParameter(dbCommand, "OldType", DbType.Int32, (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.FrontTaransaction);

            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }
        #endregion

        #region 删除一条数据
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserID, DbTransaction tran, Database db)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete UM_UserInfo ");
            strSql.Append(" where UserID=@UserID ");
            if (db == null) db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            if (tran == null)
            {
                db.ExecuteNonQuery(dbCommand);
            }
            else
            {
                db.ExecuteNonQuery(dbCommand, tran);
            }
        }

        /// <summary>
        /// 根据用户ID删除用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        public void Delete(int UserID)
        {
            Delete(UserID, null, null);
        }
        #endregion

        #region 得到一个对象实体
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_UserInfo GetModel(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserName,LoginName,Password,UserID,CertificateStyle,Postalcode,RoleID,CertificateNo,Telephone,Address,Email,QuestionID,Answer,CouterID,Remark,AddType,AddTime from UM_UserInfo ");
            strSql.Append(" where UserID=@UserID ");
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            ManagementCenter.Model.UM_UserInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }
        #endregion

        #region
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserName,LoginName,Password,UserID,CertificateStyle,Postalcode,RoleID,CertificateNo,Telephone,Address,Email,QuestionID,Answer,CouterID,Remark,AddType,AddTime ");
            strSql.Append(" FROM UM_UserInfo ");
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
            db.AddInParameter(dbCommand, "tblName", DbType.AnsiString, "UM_UserInfo");
            db.AddInParameter(dbCommand, "fldName", DbType.AnsiString, "ID");
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddInParameter(dbCommand, "IsReCount", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "OrderType", DbType.Boolean, 0);
            db.AddInParameter(dbCommand, "strWhere", DbType.AnsiString, strWhere);
            return db.ExecuteDataSet(dbCommand);
        }*/
        #endregion

        #region 获得数据列表（比DataSet效率高，推荐使用）

        /// <summary>
        /// 获得数据列表（比DataSet效率高，推荐使用）
        /// </summary>
        public List<ManagementCenter.Model.UM_UserInfo> GetListArray(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserName,LoginName,Password,UserID,CertificateStyle,Postalcode,RoleID,CertificateNo,Telephone,Address,Email,QuestionID,Answer,CouterID,Remark,AddType,AddTime ");
            strSql.Append(" FROM UM_UserInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<ManagementCenter.Model.UM_UserInfo> list = new List<ManagementCenter.Model.UM_UserInfo>();
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
        #endregion

        #region 对象实体绑定数据
        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public ManagementCenter.Model.UM_UserInfo ReaderBind(IDataReader dataReader)
        {
            ManagementCenter.Model.UM_UserInfo model = new ManagementCenter.Model.UM_UserInfo();
            object ojb;
            model.UserName = dataReader["UserName"].ToString();
            model.LoginName = dataReader["LoginName"].ToString();
            model.Password = dataReader["Password"].ToString();
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
            }
            ojb = dataReader["CertificateStyle"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CertificateStyle = (int)ojb;
            }
            model.Postalcode = dataReader["Postalcode"].ToString();
            ojb = dataReader["RoleID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RoleID = (int)ojb;
            }
            model.CertificateNo = dataReader["CertificateNo"].ToString();
            model.Telephone = dataReader["Telephone"].ToString();
            model.Address = dataReader["Address"].ToString();
            model.Email = dataReader["Email"].ToString();
            ojb = dataReader["QuestionID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.QuestionID = (int)ojb;
            }
            model.Answer = dataReader["Answer"].ToString();
            ojb = dataReader["CouterID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CouterID = (int)ojb;
            }
            ojb = dataReader["AddTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AddTime = (DateTime)ojb;
            }
            ojb = dataReader["AddType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AddType = (int)ojb;
            }
            model.Remark = dataReader["Remark"].ToString();
            return model;
        }
        #endregion

        //=================================================交易员==========================================

        #region  交易员分页查询

        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="userInfo">查询条件 用户实体</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="rowCount">总页数</param>
        /// <returns></returns>
        public DataSet GetPagingUser(Model.UM_UserInfo userInfo, int pageNo, int pageSize, out int rowCount)
        {
            // string SQL_SELECT_CUSTOMER =
            //@"select UserName,LoginName,Password,UserID,CertificateStyle,Postalcode,RoleID,CertificateNo,Telephone,Address,Email,QuestionID,Answer,CouterID,Remark,AddType,AddTime  FROM UM_UserInfo where 1=1 ";
            string SQL_SELECT_CUSTOMER =
            @"select a.*,b.name from UM_UserInfo a,CT_Counter b where b.CouterID=a.CouterID ";
            if (userInfo.LoginName != null && !string.IsNullOrEmpty(userInfo.LoginName))
            {
                SQL_SELECT_CUSTOMER += "AND LoginName LIKE  '%' + @LoginName + '%' ";
            }
            if (userInfo.UserName != null && !string.IsNullOrEmpty(userInfo.UserName))
            {
                SQL_SELECT_CUSTOMER += "AND UserName LIKE  '%' + @UserName + '%' ";
            }
            //if (userInfo.CouterID != int.MaxValue)
            //{
            //    SQL_SELECT_CUSTOMER += "AND CouterID=@CouterID ";
            //}
            if (userInfo.UserID != int.MaxValue)
            {
                SQL_SELECT_CUSTOMER += "AND UserID=@UserID ";
            }
            if (userInfo.RoleID != int.MaxValue)
            {
                SQL_SELECT_CUSTOMER += "AND RoleID=@RoleID ";
            }
            if (userInfo.Name != null && !string.IsNullOrEmpty(userInfo.Name)) //柜台名称
            {
                SQL_SELECT_CUSTOMER += "AND name LIKE  '%' + @name + '%' ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_CUSTOMER);


            if (userInfo.LoginName != null && !string.IsNullOrEmpty(userInfo.LoginName))
            {
                database.AddInParameter(dbCommand, "LoginName", DbType.String, userInfo.LoginName);
            }
            if (userInfo.UserName != null && !string.IsNullOrEmpty(userInfo.UserName))
            {
                database.AddInParameter(dbCommand, "UserName", DbType.String, userInfo.UserName);
            }
            //if (userInfo.CouterID != int.MaxValue)
            //{
            //    database.AddInParameter(dbCommand, "CouterID", DbType.Int32, userInfo.CouterID);
            //}
            if (userInfo.UserID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "UserID", DbType.Int32, userInfo.UserID);
            }
            if (userInfo.RoleID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "RoleID", DbType.Int32, userInfo.RoleID);
            }
            if (userInfo.Name != null && !string.IsNullOrEmpty(userInfo.Name))  //柜台名称
            {
                database.AddInParameter(dbCommand, "name", DbType.String, userInfo.Name);
            }
            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_CUSTOMER, pageNo, pageSize, out rowCount, "TSM_CUSTOMER");

        }
        #endregion

        #region 交易员登陆验证

        /// <summary>
        /// 登陆验证
        /// </summary>
        public ManagementCenter.Model.UM_UserInfo TranLogin(int UserID, string Password)
        {
            string SQL_SELECT_Login =
           @"select * FROM UM_UserInfo where UserID=@UserID AND Password=@Password AND RoleID=@RoleID ";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_Login);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "Password", DbType.String, Password);
            db.AddInParameter(dbCommand, "RoleID", DbType.Int32, (int)Types.RoleTypeEnum.Transaction);
            ManagementCenter.Model.UM_UserInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }

        #endregion

        //=================================================管理员========================================== 

        #region 管理员分页查询
        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="managerQueryEntity">查询条件 海外用户实体</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="rowCount">总页数</param>
        /// <returns></returns>
        public DataSet GetPagingManager(ManagementCenter.Model.UserManage.ManagerQueryEntity managerQueryEntity, int pageNo, int pageSize, out int rowCount)
        {

            string SQL_SELECT_Manager = @"select UM_UserInfo.*,UM_ManagerBeloneToGroup.ManagerGroupID,UM_ManagerGroup.ManagerGroupName
                                                                        From UM_UserInfo,UM_ManagerBeloneToGroup,UM_ManagerGroup
                                                                        Where UM_UserInfo.UserID=UM_ManagerBeloneToGroup.UserID 
                                                                        AND UM_ManagerBeloneToGroup.ManagerGroupID=UM_ManagerGroup.ManagerGroupID  ";

            if (managerQueryEntity.LoginName != null && !string.IsNullOrEmpty(managerQueryEntity.LoginName))
            {
                SQL_SELECT_Manager += "AND LoginName LIKE  '%' + @LoginName + '%' ";
            }
            if (managerQueryEntity.UserName != null && !string.IsNullOrEmpty(managerQueryEntity.UserName))
            {
                SQL_SELECT_Manager += "AND UserName LIKE  '%' + @UserName + '%' ";
            }
            if (managerQueryEntity.UserID != int.MaxValue)
            {
                SQL_SELECT_Manager += "AND UM_UserInfo.UserID=@UserID ";
            }
            if (managerQueryEntity.RoleID != int.MaxValue)
            {
                SQL_SELECT_Manager += "AND RoleID=@RoleID ";
            }
            if (managerQueryEntity.ManagerGroupID != int.MaxValue)
            {
                SQL_SELECT_Manager += "AND ManagerGroupID=@ManagerGroupID ";
            }
            if (managerQueryEntity.ManagerGroupName != null && !string.IsNullOrEmpty(managerQueryEntity.ManagerGroupName))
            {
                SQL_SELECT_Manager += "AND ManagerGroupName LIKE  '%' + @ManagerGroupName + '%' ";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(SQL_SELECT_Manager);


            if (managerQueryEntity.LoginName != null && !string.IsNullOrEmpty(managerQueryEntity.LoginName))
            {
                database.AddInParameter(dbCommand, "LoginName", DbType.String, managerQueryEntity.LoginName);
            }
            if (managerQueryEntity.UserName != null && !string.IsNullOrEmpty(managerQueryEntity.UserName))
            {
                database.AddInParameter(dbCommand, "UserName", DbType.String, managerQueryEntity.UserName);
            }
            if (managerQueryEntity.UserID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "UserID", DbType.Int32, managerQueryEntity.UserID);
            }
            if (managerQueryEntity.RoleID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "RoleID", DbType.Int32, managerQueryEntity.RoleID);
            }
            if (managerQueryEntity.ManagerGroupID != int.MaxValue)
            {
                database.AddInParameter(dbCommand, "ManagerGroupID", DbType.Int32, managerQueryEntity.ManagerGroupID);
            }
            if (managerQueryEntity.ManagerGroupName != null && !string.IsNullOrEmpty(managerQueryEntity.ManagerGroupName))
            {
                database.AddInParameter(dbCommand, "ManagerGroupName", DbType.String, managerQueryEntity.ManagerGroupName);
            }

            return CommPager.QueryPager(database, dbCommand, SQL_SELECT_Manager, pageNo, pageSize, out rowCount, "UM_UserInfo");

        }
        #endregion

        #region 管理员登陆验证

        /// <summary>
        /// 管理员登陆验证
        /// </summary>
        public ManagementCenter.Model.UM_UserInfo ManagerLoginConfirm(string LoginName, string Password, int AddType)
        {
            string SQL_SELECT_Login =
           @"SELECT [UserName],[LoginName],[Password],[UserID],[CertificateStyle],[Postalcode],[RoleID],
     [CertificateNo],[Telephone],[Address],[Email],[QuestionID],[Answer],[CouterID],[Remark],[AddType],[AddTime]
 FROM UM_UserInfo where LoginName=@LoginName AND Password=@Password AND (RoleID=@MRoleID OR RoleID=@ARoleID) AND AddType=@AddType  ";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_Login);
            db.AddInParameter(dbCommand, "LoginName", DbType.String, LoginName);
            db.AddInParameter(dbCommand, "Password", DbType.String, Password);
            db.AddInParameter(dbCommand, "MRoleID", DbType.Int32, (int)Types.RoleTypeEnum.Manager);
            db.AddInParameter(dbCommand, "ARoleID", DbType.Int32, (int)Types.RoleTypeEnum.Admin);
            db.AddInParameter(dbCommand, "AddType", DbType.Int32, AddType);
            ManagementCenter.Model.UM_UserInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }
        #endregion

        #region 管理员找回密码

        /// <summary>
        /// 管理员找回密码
        /// </summary>
        public ManagementCenter.Model.UM_UserInfo SeekForPassword(string LoginName, string Answer, int QuestionID)
        {
            string SQL_SELECT_Login =
           @"SELECT [UserName],[LoginName],[Password],[UserID],[CertificateStyle],[Postalcode],[RoleID],
[CertificateNo],[Telephone],[Address],[Email],[QuestionID],[Answer],[CouterID],[Remark],[AddType],[AddTime]
 FROM UM_UserInfo where LoginName=@LoginName AND Answer=@Answer AND QuestionID=@QuestionID  ";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_Login);
            db.AddInParameter(dbCommand, "LoginName", DbType.String, LoginName);
            db.AddInParameter(dbCommand, "Answer", DbType.String, Answer);
            db.AddInParameter(dbCommand, "QuestionID", DbType.Int32, QuestionID);
            ManagementCenter.Model.UM_UserInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }
        #endregion

        #region 对管理员登陆名和密码进行验证
        /// <summary>
        /// 对管理员用户名和密码进行验证
        /// </summary>
        /// <param name="LoginName">管理员用户名</param>
        /// <param name="PassWord">密码</param>
        /// <returns>查询出的数据</returns>
        public ManagementCenter.Model.UM_UserInfo AdminLogin(string LoginName, string PassWord)
        {
            string SQL_SELECT_Login =
           @"SELECT [UserName],[LoginName],[Password],[UserID],[CertificateStyle],[Postalcode],[RoleID],
[CertificateNo],[Telephone],[Address],[Email],[QuestionID],[Answer],[CouterID],[Remark],[AddType],[AddTime]
 FROM UM_UserInfo where LoginName=@LoginName AND Password=@Password AND (RoleID=1 or RoleID=2)";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_Login);
            db.AddInParameter(dbCommand, "LoginName", DbType.String, LoginName);
            db.AddInParameter(dbCommand, "Password", DbType.String, PassWord);
            ManagementCenter.Model.UM_UserInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }
        #endregion 对管理员登陆名和密码进行验证

        #region 查询出交易员
        /// <summary>
        /// 获取所有管理员
        /// </summary>
        /// <returns>获取所有交易员</returns>
        public List<ManagementCenter.Model.UM_UserInfo> GetAllUser()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.*,b.name ");
            strSql.Append(" from UM_UserInfo a,CT_Counter b where b.CouterID=a.CouterID ");
            List<ManagementCenter.Model.UM_UserInfo> list = new List<UM_UserInfo>();
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
        #endregion 查询出交易员
    }
}

