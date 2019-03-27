using System;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Types=ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：用户基本信息表 业务逻辑类UM_UserInfoBLL 的摘要说明。
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-20
    /// </summary>
    public class UM_UserInfoBLL
    {
        private readonly ManagementCenter.DAL.UM_UserInfoDAL dal = new ManagementCenter.DAL.UM_UserInfoDAL();

        public UM_UserInfoBLL()
        {
        }

        //===========================================公共方法===============================================

        #region 得到最大ID

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        #endregion

        #region 是否存在该记录

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            return dal.Exists(UserID);
        }

        #endregion

        #region 增加一条数据

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.UM_UserInfo model)
        {
            return dal.Add(model);
        }

        #endregion

        #region 更新一条数据

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.UM_UserInfo model)
        {
            dal.Update(model);
        }

        #endregion

        #region 删除一条数据

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserID)
        {
            dal.Delete(UserID);
        }

        #endregion

        #region 得到一个对象实体

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_UserInfo GetModel(int UserID)
        {
            return dal.GetModel(UserID);
        }

        #endregion

        #region 得到一个对象实体，从缓存中

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.UM_UserInfo GetModelByCache(int UserID)
        {
            string CacheKey = "UM_UserInfoModel-" + UserID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(UserID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                                                      TimeSpan.Zero);
                    }
                }
                catch
                {}
            }
            return (ManagementCenter.Model.UM_UserInfo) objModel;
        }

        #endregion

        #region 获得数据列表

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_UserInfo> GetListArray(string strWhere)
        {
            return dal.GetListArray(strWhere);
        }

        #endregion

        #region

        /// <summary>
        /// 获得数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion

        //===========================================管理员===============================================

        #region 添加管理员

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RightGroupID"></param>
        /// <returns></returns>
        public bool ManagerAdd(ManagementCenter.Model.UM_UserInfo model, int RightGroupID)
        {
            ManagementCenter.DAL.UM_UserInfoDAL UserInfoDAL = new UM_UserInfoDAL();
            ManagementCenter.DAL.UM_ManagerBeloneToGroupDAL ManagerBeloneToGroupDAL = new UM_ManagerBeloneToGroupDAL();

            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                int UserID = UserInfoDAL.Add(model, Tran, db);
                if (UserID != 0)
                {
                    UM_ManagerBeloneToGroup ManagerBeloneToGroup = new UM_ManagerBeloneToGroup();
                    ManagerBeloneToGroup.UserID = UserID;
                    ManagerBeloneToGroup.ManagerGroupID = RightGroupID;
                    ManagerBeloneToGroupDAL.Add(ManagerBeloneToGroup, Tran, db);
                    Tran.Commit();
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-1100";
                string errMsg = "添加管理员失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
            return true;
        }

        #endregion

        #region 更新管理员

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <param name="RightGroupID">权限组id</param>
        /// <returns></returns>
        public bool ManagerUpdate(ManagementCenter.Model.UM_UserInfo model, int RightGroupID)
        {
            ManagementCenter.DAL.UM_UserInfoDAL UserInfoDAL = new UM_UserInfoDAL();
            ManagementCenter.DAL.UM_ManagerBeloneToGroupDAL ManagerBeloneToGroupDAL = new UM_ManagerBeloneToGroupDAL();

            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                UserInfoDAL.Update(model, Tran, db);

                UM_ManagerBeloneToGroup ManagerBeloneToGroup = new UM_ManagerBeloneToGroup();
                ManagerBeloneToGroup.UserID = model.UserID;
                ManagerBeloneToGroup.ManagerGroupID = RightGroupID;
                ManagerBeloneToGroupDAL.Update(ManagerBeloneToGroup, Tran, db);
                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-1101";
                string errMsg = "更新管理员失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
            return true;
        }

        #endregion

        #region 删除管理员

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ManagerDelete(int UserID)
        {
            ManagementCenter.DAL.UM_UserInfoDAL UserInfoDAL = new UM_UserInfoDAL();
            ManagementCenter.DAL.UM_ManagerBeloneToGroupDAL ManagerBeloneToGroupDAL = new UM_ManagerBeloneToGroupDAL();

            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                ManagerBeloneToGroupDAL.Delete(UserID, Tran, db);
                UserInfoDAL.Delete(UserID, Tran, db);
                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-1102";
                string errMsg = "删除管理员失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
            return true;
        }

        #endregion

        #region 管理员分页查询

        /// <summary>
        /// 管理员分页查询
        /// </summary>
        /// <param name="managerQueryEntity">查询实体</param>
        /// <param name="pageNo">第几页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rowCount">总记录条数</param>
        /// <returns></returns>
        public DataSet GetPagingManager(Model.UserManage.ManagerQueryEntity managerQueryEntity, int pageNo, int pageSize,
                                        out int rowCount)
        {
            try
            {
                return dal.GetPagingManager(managerQueryEntity, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                string errCode = "GL-1103";
                string errMsg = "管理员分页查询失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                rowCount = 0;
                return null;
            }
        }

        #endregion

        #region 判断管理员登录名是否存在
        /// <summary>
        /// 判断管理员登录名是否存在
        /// </summary>
        /// <param name="loginname">登陆名称</param>
        /// <returns></returns>
        public bool CheckLoginName(string loginname)
        {
            try
            {
                string Where = string.Format(" LoginName='{0}' AND (RoleID={1} OR RoleID={2})", loginname,
                                             (int) Types.RoleTypeEnum.Manager, (int) Types.RoleTypeEnum.Admin);
                DataSet ds = dal.GetList(Where);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0) return true;
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-1104";
                string errMsg = "判断管理员登录名是否存在失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }

        #endregion

        #region 管理员验证
        /// <summary>
        /// 管理员验证
        /// </summary>
        /// <param name="LoginName">登陆名</param>
        /// <param name="Password">密码</param>
        /// <param name="AddType">管理员的添加类型</param>
        /// <returns></returns>
        public UM_UserInfo ManagerLoginConfirm(string LoginName, string Password, int AddType)
        {
            try
            {
                UM_UserInfo UserInfo = dal.ManagerLoginConfirm(LoginName, Password, AddType);
                return UserInfo;
            }
            catch (Exception ex)
            {
                string errCode = "GL-1105";
                string errMsg = "管理员验证失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #region 找回密码方法
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="LoginName">登陆名</param>
        /// <param name="Answer">问题答案</param>
        /// <param name="QuestionID">问题类型</param>
        /// <returns></returns>
        public ManagementCenter.Model.UM_UserInfo SeekForPassword(string LoginName, string Answer, int QuestionID)
        {
            try
            {
                return dal.SeekForPassword(LoginName, Answer, QuestionID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-1106";
                string errMsg = "管理员验证失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }
        #endregion

        //===========================================交易员===============================================

        #region 交易员分页查询

        /// <summary>
        /// 交易员分页查询
        /// </summary>
        /// <param name="userInfo">用户查询实体</param>
        /// <param name="pageNo">第几页</param>
        /// <param name="pageSize">每页行数</param>
        /// <param name="rowCount">总记录数</param>
        /// <returns></returns>
        public DataSet GetPagingUser(Model.UM_UserInfo userInfo, int pageNo, int pageSize, out int rowCount)
        {
            try
            {
                return dal.GetPagingUser(userInfo, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0260";
                string errMsg = "交易员分页查询！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                rowCount = 0;
                return null;
            }
        }

        #endregion
    }
}