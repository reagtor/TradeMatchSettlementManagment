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
    ///描述：现货_品种_交易单位换算 业务逻辑类CM_UnitConversionBLL 的摘要说明。
    /// 错误编码范围:5750-5769
    ///作者：刘书伟
    ///日期:2008-11-27
    /// </summary>
    public class CM_UnitConversionBLL
    {
        private readonly ManagementCenter.DAL.CM_UnitConversionDAL cM_UnitConversionDAL =
            new ManagementCenter.DAL.CM_UnitConversionDAL();

        public CM_UnitConversionBLL()
        {
        }

        #region  成员方法

        #region 添加现货_品种_交易单位换算
        /// <summary>
        /// 添加现货_品种_交易单位换算
        /// </summary>
        /// <param name="model">现货_品种_交易单位换算实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.CM_UnitConversion model)
        {
            try
            {
                return cM_UnitConversionDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5750";
                string errMsg = "添加现货_品种_交易单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }
        #endregion

        #region 更新现货_品种_交易单位换算
        /// <summary>
        /// 更新现货_品种_交易单位换算
        /// </summary>
        /// <param name="model">现货_品种_交易单位换算实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.CM_UnitConversion model)
        {
            try
            {
                return cM_UnitConversionDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5752";
                string errMsg = "更新现货_品种_交易单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 根据现货_品种_交易单位换算ID删除现货_品种_交易单位换算
        /// <summary>
        /// 根据现货_品种_交易单位换算ID删除现货_品种_交易单位换算
        /// </summary>
        /// <param name="UnitConversionID">现货_品种_交易单位换算ID</param>
        /// <returns></returns>
        public bool Delete(int UnitConversionID)
        {
            try
            {
                return cM_UnitConversionDAL.Delete(UnitConversionID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5751";
                string errMsg = "根据现货_品种_交易单位换算ID删除现货_品种_交易单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 暂不需要的公共方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_UnitConversionDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UnitConversionID)
        {
            return cM_UnitConversionDAL.Exists(UnitConversionID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_UnitConversion GetModel(int UnitConversionID)
        {
            return cM_UnitConversionDAL.GetModel(UnitConversionID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_UnitConversion GetModelByCache(int UnitConversionID)
        {
            string CacheKey = "CM_UnitConversionModel-" + UnitConversionID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_UnitConversionDAL.GetModel(UnitConversionID);
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
            return (ManagementCenter.Model.CM_UnitConversion) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_UnitConversionDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region 根据查询条件获取所有的现货_品种_交易单位换算 （查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的现货_品种_交易单位换算 （查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_UnitConversion> GetListArray(string strWhere)
        {
            try
            {
                return cM_UnitConversionDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5754";
                string errMsg = "根据查询条件获取所有的现货_品种_交易单位换算 （查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
                }
        }
        #endregion

        #region 获取所有现货_品种_交易单位换算

        /// <summary>
        /// 获取所有现货_品种_交易单位换算
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMUnitConversion(string BreedClassName, int pageNo, int pageSize,
                                              out int rowCount)
        {
            try
            {
                CM_UnitConversionDAL cMUnitConversionDAL = new CM_UnitConversionDAL();
                return cMUnitConversionDAL.GetAllCMUnitConversion(BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-5753";
                string errMsg = "获取所有现货_品种_交易单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据现货_品种_交易单位换算表中的品种ID获取品种名称

        /// <summary>
        /// 根据现货_品种_交易单位换算表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetCMUnitConversionBreedClassName()
        {
            try
            {
                CM_UnitConversionDAL cMUnitConversionDAL = new CM_UnitConversionDAL();
                return cMUnitConversionDAL.GetCMUnitConversionBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5755";
                string errMsg = "根据现货_品种_交易单位换算表中的品种ID获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}