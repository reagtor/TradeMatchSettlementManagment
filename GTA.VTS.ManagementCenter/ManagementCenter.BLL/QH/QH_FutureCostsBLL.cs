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
    ///描述：品种_期货_交易费用 业务逻辑类QH_FutureCostsBLL 的摘要说明。错误编码范围:6300-6319
    ///作者：刘书伟
    ///日期：2008-11-22
    /// </summary>
    public class QH_FutureCostsBLL
    {
        private readonly ManagementCenter.DAL.QH_FutureCostsDAL qH_FutureCostsDAL =
            new ManagementCenter.DAL.QH_FutureCostsDAL();

        public QH_FutureCostsBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_FutureCostsDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID)
        {
            try
            {
                return qH_FutureCostsDAL.Exists(BreedClassID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6300";
                string errMsg = "判断是否存在期货费用记录失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #region 添加品种_期货_交易费用

        /// <summary>
        /// 添加品种_期货_交易费用
        /// </summary>
        /// <param name="model">品种_期货_交易费用实体</param>
        /// <returns></returns>
        public bool AddQHFutureCosts(ManagementCenter.Model.QH_FutureCosts model)
        {
            try
            {
                return qH_FutureCostsDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6301";
                string errMsg = "添加品种_期货_交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 修改品种_期货_交易费用

        /// <summary>
        /// 修改品种_期货_交易费用
        /// </summary>
        /// <param name="model">品种_期货_交易费用实体</param>
        /// <returns></returns>
        public bool UpdateQHFutureCosts(ManagementCenter.Model.QH_FutureCosts model)
        {
            try
            {
                return qH_FutureCostsDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6302";
                string errMsg = "修改品种_期货_交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 删除品种_期货_交易费用

        /// <summary>
        /// 删除品种_期货_交易费用
        /// </summary>
        /// <param name="BreedClassID">品种_期货_交易费用ID</param>
        /// <returns></returns>
        public bool DeleteQHFutureCosts(int BreedClassID)
        {
            try
            {
                return qH_FutureCostsDAL.Delete(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6303";
                string errMsg = "删除品种_期货_交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_FutureCosts GetModel(int BreedClassID)
        {
            return qH_FutureCostsDAL.GetModel(BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_FutureCosts GetModelByCache(int BreedClassID)
        {
            string CacheKey = "QH_FutureCostsModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_FutureCostsDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.QH_FutureCosts) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_FutureCostsDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的品种_期货_交易费用（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_FutureCosts> GetListArray(string strWhere)
        {
            try
            {
                return qH_FutureCostsDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6304";
                string errMsg = "根据查询条件获取所有的品种_期货_交易费用（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #region 获取所有品种_期货_交易费用

        /// <summary>
        ///获取所有品种_期货_交易费用
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllQHFutureCosts(string BreedClassName,
                                           int pageNo, int pageSize,
                                           out int rowCount)
        {
            try
            {
                QH_FutureCostsDAL qHFutureCostsDAL = new QH_FutureCostsDAL();
                return qHFutureCostsDAL.GetAllQHFutureCosts(BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-6305";
                string errMsg = "获取所有品种_期货_交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}