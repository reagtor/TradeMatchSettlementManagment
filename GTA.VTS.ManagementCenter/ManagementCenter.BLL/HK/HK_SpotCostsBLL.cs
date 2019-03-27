using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：港股_交易费用 业务逻辑类HK_SpotCostsBLL 的摘要说明。错误编码范围:7850-7869
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_SpotCostsBLL
    {
        private readonly HK_SpotCostsDAL hK_SpotCostsDAL = new HK_SpotCostsDAL();
        public HK_SpotCostsBLL()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return hK_SpotCostsDAL.GetMaxId();
        }

        #region 是否存在港股交易费用记录
        /// <summary>
        /// 是否存在港股交易费用记录
        /// </summary>
        public bool ExistsHKSpotCosts(int BreedClassID)
        {
            try
            {
                HK_SpotCostsDAL hKSpotCostsDAL = new HK_SpotCostsDAL();
                return hKSpotCostsDAL.Exists(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7850";
                string errMsg = "判断港股_交易费用中品种名称是否已经存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region  添加港股交易费用
        /// <summary>
        /// 添加港股交易费用
        /// </summary>
        /// <param name="model">港股交易费用实体</param>
        /// <returns></returns>
        public bool AddHKSpotCosts(ManagementCenter.Model.HK_SpotCosts model)
        {
            try
            {
                return hK_SpotCostsDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7851";
                string errMsg = "添加港股交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 更新港股交易费用
        /// <summary>
        /// 更新港股交易费用
        /// </summary>
        /// <param name="model">港股交易费用实体</param>
        /// <returns></returns>
        public bool UpdateHKSpotCosts(ManagementCenter.Model.HK_SpotCosts model)
        {
            try
            {
                return hK_SpotCostsDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7852";
                string errMsg = " 更新港股交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 根据品种ID，删除港股交易费用

        /// <summary>
        ///根据品种ID，删除港股交易费用
        /// </summary>
        public bool DeleteHKSpotCosts(int BreedClassID)
        {
            try
            {
                return hK_SpotCostsDAL.Delete(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7853";
                string errMsg = "根据品种ID，删除港股交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据品种ID得到港股交易费用对象实体
        /// <summary>
        /// 根据品种ID得到港股交易费用对象实体
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.HK_SpotCosts GetModel(int BreedClassID)
        {
            try
            {
                return hK_SpotCostsDAL.GetModel(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7854";
                string errMsg = "根据品种ID得到港股交易费用对象实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 得到一个对象实体，从缓存中。
        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.HK_SpotCosts GetModelByCache(int BreedClassID)
        {

            string CacheKey = "HK_SpotCostsModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = hK_SpotCostsDAL.GetModel(BreedClassID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (ManagementCenter.Model.HK_SpotCosts)objModel;
        }
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return hK_SpotCostsDAL.GetList(strWhere);
        }
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region 根据查询条件获取所有的港股_交易费用（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的港股_交易费用（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HK_SpotCosts> GetListArray(string strWhere)
        {
            try
            {
                return hK_SpotCostsDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7855";
                string errMsg = "根据查询条件获取所有的港股_交易费用（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 获取所有港股交易费用

        /// <summary>
        /// 获取所有港股交易费用
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllHKSpotCosts(string BreedClassName, int pageNo, int pageSize,
                                       out int rowCount)
        {
            try
            {
                HK_SpotCostsDAL hKSpotCostsDAL = new HK_SpotCostsDAL();
                return hKSpotCostsDAL.GetAllHKSpotCosts(BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-7856";
                string errMsg = "获取所有港股交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据港股交易费用表中的品种ID获取品种名称

        /// <summary>
        /// 根据港股交易费用表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetHKSpotCostsBreedClassName()
        {
            try
            {
                HK_SpotCostsDAL hKSpotCostsDAL = new HK_SpotCostsDAL();
                return hKSpotCostsDAL.GetHKSpotCostsBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-7857";
                string errMsg = "根据港股交易费用表中的品种ID获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}
