using System;
using System.Collections.Generic;
using System.Data;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：管理员组可用功能表 业务逻辑类UM_ManagerGroupFunctionsBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
    /// </summary>
    public class UM_ManagerGroupFunctionsBLL
    {
        private readonly ManagementCenter.DAL.UM_ManagerGroupFunctionsDAL dal =
            new ManagementCenter.DAL.UM_ManagerGroupFunctionsDAL();

        public UM_ManagerGroupFunctionsBLL()
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
        public bool Exists(int ManageGroupFuntctiosID)
        {
            return dal.Exists(ManageGroupFuntctiosID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.UM_ManagerGroupFunctions model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.UM_ManagerGroupFunctions model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ManageGroupFuntctiosID)
        {
            dal.Delete(ManageGroupFuntctiosID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_ManagerGroupFunctions GetModel(int ManageGroupFuntctiosID)
        {
            return dal.GetModel(ManageGroupFuntctiosID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.UM_ManagerGroupFunctions GetModelByCache(int ManageGroupFuntctiosID)
        {
            string CacheKey = "UM_ManagerGroupFunctionsModel-" + ManageGroupFuntctiosID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ManageGroupFuntctiosID);
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
            return (ManagementCenter.Model.UM_ManagerGroupFunctions) objModel;
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_ManagerGroupFunctions> GetListArray(string strWhere)
        {
            try
            {
                return dal.GetListArray(strWhere);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
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
        /// 检查管理员权限
        /// </summary>
        /// <param name="ManagerID"></param>
        /// <param name="Function"></param>
        /// <returns></returns>
        public bool CheckRight(int ManagerID, int Function)
        {
            try
            {
                List<UM_ManagerGroupFunctions> l =
                    dal.GetRightListByManagerID(ManagerID);
                if (l == null) return false;
                foreach (var functions in l)
                {
                    if (functions.FunctionID == Function)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-1305";
                string errMsg = "检查管理员权限失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }

        /// <summary>
        ///根据管理员ID获取权限列表
        /// </summary>
        public List<ManagementCenter.Model.UM_ManagerGroupFunctions> GetRightListByManagerID(int ManagerID)
        {
            try
            {
                return dal.GetRightListByManagerID(ManagerID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-1306";
                string errMsg = "根据管理员ID获取权限列表失败！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}