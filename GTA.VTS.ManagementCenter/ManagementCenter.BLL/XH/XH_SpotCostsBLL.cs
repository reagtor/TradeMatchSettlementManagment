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
    ///描述：品种_现货_交易费用 业务逻辑类XH_SpotCostsBLL 的摘要说明。
    /// 错误编码范围:5500-5519
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class XH_SpotCostsBLL
    {
        private readonly ManagementCenter.DAL.XH_SpotCostsDAL xH_SpotCostsDAL =
            new ManagementCenter.DAL.XH_SpotCostsDAL();

        public XH_SpotCostsBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_SpotCostsDAL.GetMaxId();
        }

        /// <summary>
        /// 判断品种_现货_交易费用中品种名称是否已经存在
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool Exists(int BreedClassID)
        {
            try
            {
                XH_SpotCostsDAL xHSpotCostsDAL = new XH_SpotCostsDAL();
                return xHSpotCostsDAL.Exists(BreedClassID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5507";
                string errMsg = "判断品种_现货_交易费用中品种名称是否已经存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #region 添加现货交易费用
        /// <summary>
        /// 添加现货交易费用
        /// </summary>
        /// <param name="model">现货交易费用实体</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.XH_SpotCosts model)
        {
            try
            {
                return xH_SpotCostsDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5500";
                string errMsg = " 添加现货交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 更新现货交易费用
        /// <summary>
        /// 更新现货交易费用
        /// </summary>
        /// <param name="model">现货交易费用实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_SpotCosts model)
        {
            try
            {
                return xH_SpotCostsDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5502";
                string errMsg = " 更新现货交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 根据品种ID，删除现货交易费用

        /// <summary>
        ///根据品种ID，删除现货交易费用
        /// </summary>
        public bool DeleteSpotCosts(int BreedClassID)
        {
            try
            {
                return xH_SpotCostsDAL.Delete(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5501";
                string errMsg = " 根据品种ID，删除现货交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据品种ID得到现货交易费用对象实体

        /// <summary>
        /// 根据品种ID得到现货交易费用对象实体
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.XH_SpotCosts GetModel(int BreedClassID)
        {
            try
            {
                return xH_SpotCostsDAL.GetModel(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5505";
                string errMsg = " 根据品种ID得到现货交易费用对象实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 暂不用的公共代码

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_SpotCosts GetModelByCache(int BreedClassID)
        {
            string CacheKey = "XH_SpotCostsModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_SpotCostsDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.XH_SpotCosts) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_SpotCostsDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #endregion

        #region 根据查询条件获取所有的品种_现货_交易费用（查询条件可为空）

        /// <summary>
        /// 根据查询条件获取所有的品种_现货_交易费用（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_SpotCosts> GetListArray(string strWhere)
        {
            try
            {
                return xH_SpotCostsDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5504";
                string errMsg = " 根据查询条件获取所有的品种_现货_交易费用（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取所有现货交易费用

        /// <summary>
        /// 获取所有现货交易费用
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllSpotCosts(int BreedClassID, string BreedClassName, int pageNo, int pageSize,
                                       out int rowCount)
        {
            try
            {
                XH_SpotCostsDAL xHSpotCostsDAL = new XH_SpotCostsDAL();
                return xHSpotCostsDAL.GetAllSpotCosts(BreedClassID, BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-5503";
                string errMsg = " 获取所有现货交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据现货交易费用表中的品种ID获取品种名称

        /// <summary>
        /// 根据现货交易费用表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetSpotCostsBreedClassName()
        {
            try
            {
                XH_SpotCostsDAL xHSpotCostsDAL = new XH_SpotCostsDAL();
                return xHSpotCostsDAL.GetSpotCostsBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5506";
                string errMsg = "根据现货交易费用表中的品种ID获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}