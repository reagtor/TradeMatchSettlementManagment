using System;
using System.Collections.Generic;
using System.Data;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：交易规则_交易方向_交易单位_交易量(最小交易单位) 业务逻辑类XH_MinVolumeOfBusinessBLL 的摘要说明。
    ///错误编码范围:5300-5319
    ///作者：刘书伟
    ///日期：2008-12-15
    /// </summary>
    public class XH_MinVolumeOfBusinessBLL
    {
        private readonly ManagementCenter.DAL.XH_MinVolumeOfBusinessDAL xH_MinVolumeOfBusinessDAL =
            new ManagementCenter.DAL.XH_MinVolumeOfBusinessDAL();

        public XH_MinVolumeOfBusinessBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_MinVolumeOfBusinessDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int MinVolumeOfBusinessID)
        {
            return xH_MinVolumeOfBusinessDAL.Exists(MinVolumeOfBusinessID);
        }

        #region 添加交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// <summary>
        /// 添加交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="model">交易规则_交易方向_交易单位_交易量(最小交易单位)实体</param>
        /// <returns></returns>
        public int Add(ManagementCenter.Model.XH_MinVolumeOfBusiness model)
        {
            try
            {
                return xH_MinVolumeOfBusinessDAL.Add(model);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5300";
                string errMsg = "添加交易规则_交易方向_交易单位_交易量(最小交易单位)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return AppGlobalVariable.INIT_INT;
            }
        }
        #endregion

        #region 暂不用的公共方法
        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public void Update(ManagementCenter.Model.XH_MinVolumeOfBusiness model)
        //{
        //    xH_MinVolumeOfBusinessDAL.Update(model);
        //}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        //public void Delete(int MinVolumeOfBusinessID)
        //{
        //    xH_MinVolumeOfBusinessDAL.Delete(MinVolumeOfBusinessID);
        //}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_MinVolumeOfBusiness GetModel(int MinVolumeOfBusinessID)
        {
            return xH_MinVolumeOfBusinessDAL.GetModel(MinVolumeOfBusinessID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_MinVolumeOfBusiness GetModelByCache(int MinVolumeOfBusinessID)
        {
            string CacheKey = "XH_MinVolumeOfBusinessModel-" + MinVolumeOfBusinessID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_MinVolumeOfBusinessDAL.GetModel(MinVolumeOfBusinessID);
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
            return (ManagementCenter.Model.XH_MinVolumeOfBusiness)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_MinVolumeOfBusinessDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region 根据查询条件获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_MinVolumeOfBusiness> GetListArray(string strWhere)
        {
            try
            {
                return xH_MinVolumeOfBusinessDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5304";
                string errMsg = "根据查询条件获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位（查询条件可为空)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return null;
            }
        }
        #endregion

        #endregion  成员方法

        #region 获取所有交易规则_交易方向_交易单位_交易量(最小交易单位)

        /// <summary>
        /// 获取所有交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllXHMinVolumeOfBusiness(string BreedClassName, int pageNo, int pageSize, out int rowCount)
        {
            try
            {
                return xH_MinVolumeOfBusinessDAL.GetAllXHMinVolumeOfBusiness(BreedClassName, pageNo, pageSize,
                                                                             out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-5303";
                string errMsg = "获取所有交易规则_交易方向_交易单位_交易量(最小交易单位)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return null;

            }
        }

        #endregion

        #region 根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID，删除此数据

        /// <summary>
        /// 根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID，删除此数据
        /// </summary>
        /// <param name="minVolumeOfBusinessID">根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID</param>
        /// <returns></returns>
        public bool DeleteXHMinVolumeOfBusByID(int minVolumeOfBusinessID)
        {
            try
            {
                return xH_MinVolumeOfBusinessDAL.DeleteXHMinVolumeOfBusByID(minVolumeOfBusinessID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5301";
                string errMsg = "根据交易规则_交易方向_交易单位_交易量(最小交易单位)ID，删除此数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;

            }
        }

        #endregion

        #region 更新交易规则_交易方向_交易单位_交易量(最小交易单位)数据

        /// <summary>
        /// 更新交易规则_交易方向_交易单位_交易量(最小交易单位)数据
        /// </summary>
        /// <param name="model">交易规则_交易方向_交易单位_交易量(最小交易单位)实体</param>
        public bool UpdateXHMinVolumeOfBus(ManagementCenter.Model.XH_MinVolumeOfBusiness model)
        {
            try
            {
                return xH_MinVolumeOfBusinessDAL.UpdateXHMinVolumeOfBus(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5302";
                string errMsg = "更新交易规则_交易方向_交易单位_交易量(最小交易单位)数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion


        #region 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// <summary>
        /// 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetXHAndHKBreedClassNameByBreedClassID()
        {
            try
            {
                return xH_MinVolumeOfBusinessDAL.GetXHAndHKBreedClassNameByBreedClassID();
            }
            catch (Exception ex)
            {
                string errCode = "GL-";
                string errMsg = "根据现货规则表和港股规则表中的品种标识获取品种名称数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

    }
}