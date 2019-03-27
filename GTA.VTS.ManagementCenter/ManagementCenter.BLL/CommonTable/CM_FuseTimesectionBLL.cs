using System;
using System.Collections.Generic;
using System.Data;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：熔断_时间段标识 业务逻辑类CM_FuseTimesection 的摘要说明。错误编码范围:6720-6739
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_FuseTimesectionBLL
    {
        private readonly ManagementCenter.DAL.CM_FuseTimesectionDAL cM_FuseTimesectionDAL = new CM_FuseTimesectionDAL();

        public CM_FuseTimesectionBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_FuseTimesectionDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TimesectionID)
        {
            return cM_FuseTimesectionDAL.Exists(TimesectionID);
        }

        #region 添加熔断_时间段标识

        /// <summary>
        /// 添加熔断_时间段标识
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <returns></returns>
        public int AddCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model)
        {
            try
            {
                return cM_FuseTimesectionDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6720";
                string errMsg = "添加熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        #endregion

        #region 添加熔断_时间段标识 重载

        /// <summary>
        /// 添加熔断_时间段标识
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <param name="msg">返回错误结果提示信息</param>
        /// <returns></returns>
        public int AddCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model, ref string msg)
        {
            try
            {
                string m_msg = string.Empty;
                if (model.EndTime < model.StartTime)
                {
                    msg += "起始时间不能大于截止时间!" + "\t\n";
                }
                else if (model.EndTime < model.StartTime)
                {
                    msg += "截止时间不能小于起始时间!" + "\t\n";
                }
                if (model.StartTime == model.EndTime)
                {
                    msg += "起始和截止时间不能相等!" + "\t\n";
                }
                m_msg = msg;
                if (!string.IsNullOrEmpty(m_msg))
                {
                    return AppGlobalVariable.INIT_INT;
                }
                return cM_FuseTimesectionDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6721";
                string errMsg = "添加熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        #endregion

        #region 修改熔断_时间段标识

        /// <summary>
        /// 修改熔断_时间段标识
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <returns></returns>
        public bool UpdateCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model)
        {
            try
            {
                return cM_FuseTimesectionDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6722";
                string errMsg = "修改熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 修改熔断_时间段标识 重载

        /// <summary>
        /// 修改熔断_时间段标识 重载
        /// </summary>
        /// <param name="model">熔断_时间段标识实体</param>
        /// <param name="msg">返回错误结果提示信息</param>
        /// <returns></returns>
        public bool UpdateCMFuseTimesection(ManagementCenter.Model.CM_FuseTimesection model, ref string msg)
        {
            try
            {
                string m_msg = string.Empty;
                if (model.StartTime > model.EndTime)
                {
                    msg += "起始时间不能大于截止时间!" + "\t\n";
                }
                else if (model.EndTime < model.StartTime)
                {
                    msg += "截止时间不能小于起始时间!" + "\t\n";
                }
                if (model.StartTime == model.EndTime)
                {
                    msg += "起始和截止时间不能相等!" + "\t\n";
                }
                m_msg = msg;
                if (!string.IsNullOrEmpty(m_msg))
                {
                    return false;
                }
                return cM_FuseTimesectionDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6723";
                string errMsg = "修改熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 删除熔断_时间段标识

        /// <summary>
        /// 删除熔断_时间段标识
        /// </summary>
        /// <param name="TimesectionID">熔断_时间段标识</param>
        /// <returns></returns>
        public bool DeleteCMFuseTimesection(int TimesectionID)
        {
            try
            {
                return cM_FuseTimesectionDAL.Delete(TimesectionID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6724";
                string errMsg = "删除熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据商品代码获取所有熔断_时间段标识

        /// <summary>
        /// 根据商品代码获取熔断_时间段标识
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public DataSet GetCMFuseTimesectionByCommodityCode(string CommodityCode)
        {
            try
            {
                CM_FuseTimesectionDAL cMFuseTimesectionDAL = new CM_FuseTimesectionDAL();
                return cMFuseTimesectionDAL.GetCMFuseTimesectionByCommodityCode(CommodityCode);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6725";
                string errMsg = "根据商品代码获取所有熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_FuseTimesection GetModel(int TimesectionID)
        {
            return cM_FuseTimesectionDAL.GetModel(TimesectionID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_FuseTimesection GetModelByCache(int TimesectionID)
        {
            string CacheKey = "CM_FuseTimesectionModel-" + TimesectionID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_FuseTimesectionDAL.GetModel(TimesectionID);
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
            return (ManagementCenter.Model.CM_FuseTimesection) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_FuseTimesectionDAL.GetList(strWhere);
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
        /// 根据条件获取所有的熔断_时间段标识(条件可为空)
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_FuseTimesection> GetListArray(string strWhere)
        {
            return cM_FuseTimesectionDAL.GetListArray(strWhere);
        }

        #endregion  成员方法
    }
}