using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：管理员组 业务逻辑类UM_ManagerGroupBLL 的摘要说明。
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-20
    /// </summary>
    public class UM_ManagerGroupBLL
    {
        private readonly ManagementCenter.DAL.UM_ManagerGroupDAL dal = new ManagementCenter.DAL.UM_ManagerGroupDAL();

        public UM_ManagerGroupBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ManagerGroupID)
        {
            return dal.Exists(ManagerGroupID);
        }

        /// <summary>
        /// 添加权限组
        /// </summary>
        /// <param name="model">权限组实体</param>
        /// <param name="L_UM_ManagerGroupFunctions">权限组功能</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.UM_ManagerGroup model,
                        List<UM_ManagerGroupFunctions> L_UM_ManagerGroupFunctions)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                ManagementCenter.DAL.UM_ManagerGroupDAL UM_ManagerGroup = new UM_ManagerGroupDAL();
                ManagementCenter.DAL.UM_ManagerGroupFunctionsDAL UM_ManagerGroupFunctions =
                    new UM_ManagerGroupFunctionsDAL();

                int ManagerGroupID = UM_ManagerGroup.Add(model, Tran, db);
                if (ManagerGroupID != 0)
                {
                    foreach (UM_ManagerGroupFunctions ManagerGroupFunction in L_UM_ManagerGroupFunctions)
                    {
                        ManagerGroupFunction.ManagerGroupID = ManagerGroupID;
                        UM_ManagerGroupFunctions.Add(ManagerGroupFunction, Tran, db);
                    }
                }
                Tran.Commit();
            }
            catch (Exception ex)
            {
                string errCode = "GL-1300";
                string errMsg = "添加权限组失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                Tran.Rollback();
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
            return true;
        }

        /// <summary>
        /// 更新权限组
        /// </summary>
        /// <param name="model">权限组实体</param>
        /// <param name="L_UM_ManagerGroupFunctions">权限组功能</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.UM_ManagerGroup model,
                           List<UM_ManagerGroupFunctions> L_UM_ManagerGroupFunctions)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                ManagementCenter.DAL.UM_ManagerGroupDAL UM_ManagerGroup = new UM_ManagerGroupDAL();
                ManagementCenter.DAL.UM_ManagerGroupFunctionsDAL UM_ManagerGroupFunctions =
                    new UM_ManagerGroupFunctionsDAL();

                UM_ManagerGroup.Update(model, Tran, db);
                UM_ManagerGroupFunctions.DeleteByManagerGroupID(model.ManagerGroupID, Tran, db);
                foreach (UM_ManagerGroupFunctions ManagerGroupFunction in L_UM_ManagerGroupFunctions)
                {
                    ManagerGroupFunction.ManagerGroupID = model.ManagerGroupID;
                    UM_ManagerGroupFunctions.Add(ManagerGroupFunction, Tran, db);
                }

                Tran.Commit();
            }
            catch (Exception ex)
            {
                string errCode = "GL-1301";
                string errMsg = "更新权限组失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                Tran.Rollback();
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
            return true;
        }

        /// <summary>
        /// 删除权限组
        /// </summary>
        /// <param name="ManagerGroupID">权限组ID</param>
        /// <returns></returns>
        public bool Delete(int ManagerGroupID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                ManagementCenter.DAL.UM_ManagerGroupDAL UM_ManagerGroup = new UM_ManagerGroupDAL();
                ManagementCenter.DAL.UM_ManagerGroupFunctionsDAL UM_ManagerGroupFunctions =
                    new UM_ManagerGroupFunctionsDAL();

                UM_ManagerBeloneToGroupDAL UM_ManagerBeloneToGroup = new UM_ManagerBeloneToGroupDAL();
                List<UM_ManagerBeloneToGroup> l =
                    UM_ManagerBeloneToGroup.GetListArray(string.Format("ManagerGroupID={0}", ManagerGroupID));
                if (l != null && l.Count == 0)
                {
                    UM_ManagerGroupFunctions.DeleteByManagerGroupID(ManagerGroupID, Tran, db);
                    UM_ManagerGroup.Delete(ManagerGroupID, Tran, db);
                    Tran.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-1302";
                string errMsg = "删除权限组失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                Tran.Rollback();
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_ManagerGroup GetModel(int ManagerGroupID)
        {
            return dal.GetModel(ManagerGroupID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.UM_ManagerGroup GetModelByCache(int ManagerGroupID)
        {
            string CacheKey = "UM_ManagerGroupModel-" + ManagerGroupID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ManagerGroupID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                                                      TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return (ManagementCenter.Model.UM_ManagerGroup) objModel;
        }

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
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}
        /// <summary>
        /// 权限组分页查询方法
        /// </summary>
        /// <param name="strwhere"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataSet GetPagingManagerGroup(string strwhere, int pageNo, int pageSize, out int rowCount)
        {
            try
            {
                return dal.GetPagingManagerGroup(strwhere, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                string errCode = "GL-1303";
                string errMsg = "权限组分页查询失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                rowCount = 0;
                return null;
            }
        }

        /// <summary>
        /// 获取权限组列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_ManagerGroup> GetListArray(string strWhere)
        {
            try
            {
                return dal.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-1304";
                string errMsg = "获取权限组列表失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}