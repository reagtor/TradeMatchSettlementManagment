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
    /// 描述：现货_交易商品品种_持仓限制 业务逻辑类XH_SpotPositionBLL 的摘要说明。
    /// 错误编码范围:5650-5669
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class XH_SpotPositionBLL
    {
        private readonly ManagementCenter.DAL.XH_SpotPositionDAL xH_SpotPositionDAL =
            new ManagementCenter.DAL.XH_SpotPositionDAL();

        public XH_SpotPositionBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_SpotPositionDAL.GetMaxId();
        }

        #region 根据品种ID，判断现货_交易商品品种_持仓限制记录是否已存在

        /// <summary>
        /// 根据品种ID，判断现货_交易商品品种_持仓限制记录是否已存在
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public bool ExistsXHSpotPosition(int BreedClassID)
        {
            try
            {
                XH_SpotPositionDAL xHSpotPositionDAL = new XH_SpotPositionDAL();
                return xHSpotPositionDAL.Exists(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5650";
                string errMsg = "根据品种ID，判断现货_交易商品品种_持仓限制记录是否已存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 添加现货_交易商品品种_持仓限制
        /// <summary>
        /// 添加现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="model">现货_交易商品品种_持仓限制实体</param>
        /// <returns></returns>
        public bool Add(ManagementCenter.Model.XH_SpotPosition model)
        {
            try
            {
                return xH_SpotPositionDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5651";
                string errMsg = "添加现货_交易商品品种_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 更新现货_交易商品品种_持仓限制
        /// <summary>
        /// 更新现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="model">现货_交易商品品种_持仓限制实体</param>
        /// <returns></returns>
        public bool Update(ManagementCenter.Model.XH_SpotPosition model)
        {
            try
            {
                return xH_SpotPositionDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5653";
                string errMsg = "更新现货_交易商品品种_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 删除现货_交易商品品种_持仓限制
        /// <summary>
        /// 删除现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool Delete(int BreedClassID)
        {
            try
            {
                return xH_SpotPositionDAL.Delete(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5652";
                string errMsg = "删除现货_交易商品品种_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 暂不用的公共方法
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_SpotPosition GetModel(int BreedClassID)
        {
            return xH_SpotPositionDAL.GetModel(BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_SpotPosition GetModelByCache(int BreedClassID)
        {
            string CacheKey = "XH_SpotPositionModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_SpotPositionDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.XH_SpotPosition) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_SpotPositionDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region 根据查询条件获取所有的现货_交易商品品种_持仓限制（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的现货_交易商品品种_持仓限制（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_SpotPosition> GetListArray(string strWhere)
        {
            try
            {
                return xH_SpotPositionDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5655";
                string errMsg = "根据查询条件获取所有的现货_交易商品品种_持仓限制(查询条件可为空)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 获取所有现货_交易商品品种_持仓限制

        /// <summary>
        /// 获取所有现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllXHSpotPosition(string BreedClassName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            try
            {
                XH_SpotPositionDAL xHSpotPositionDAL = new XH_SpotPositionDAL();
                return xHSpotPositionDAL.GetAllXHSpotPosition(BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-5654";
                string errMsg = "获取所有现货_交易商品品种_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据现货交易商品品种_持仓限制表中的品种ID获取品种名称

        /// <summary>
        /// 根据现货交易商品品种_持仓限制表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetSpotPositionBreedClassName()
        {
            try
            {
                XH_SpotPositionDAL xHSpotPositionDAL = new XH_SpotPositionDAL();
                return xHSpotPositionDAL.GetSpotPositionBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5656";
                string errMsg = "根据现货交易商品品种_持仓限制表中的品种ID获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}