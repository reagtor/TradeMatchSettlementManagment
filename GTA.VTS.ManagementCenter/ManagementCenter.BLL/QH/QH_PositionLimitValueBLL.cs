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
    ///描述：期货_持仓限制 业务逻辑类QH_PositionLimitValueBLL 的摘要说明。错误编码范围:6620-6639
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_PositionLimitValueBLL
    {
        private readonly ManagementCenter.DAL.QH_PositionLimitValueDAL qH_PositionLimitValueDAL =
            new ManagementCenter.DAL.QH_PositionLimitValueDAL();

        public QH_PositionLimitValueBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_PositionLimitValueDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PositionLimitValueID)
        {
            return qH_PositionLimitValueDAL.Exists(PositionLimitValueID);
        }

        #region 添加(商品)期货_持仓限制

        /// <summary>
        /// 添加(商品)期货_持仓限制
        /// </summary>
        /// <param name="model">(商品)期货_持仓限制实体</param>
        /// <returns></returns>
        public int AddQHPositionLimitValue(ManagementCenter.Model.QH_PositionLimitValue model)
        {
            try
            {
                QH_PositionLimitValueDAL qHPositionLimitValueDAL = new QH_PositionLimitValueDAL();
                return qHPositionLimitValueDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6620";
                string errMsg = "添加(商品)期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        #endregion

        #region 修改(商品)期货_持仓限制

        /// <summary>
        /// 修改(商品)期货_持仓限制
        /// </summary>
        /// <param name="model">(商品)期货_持仓限制实体</param>
        /// <returns></returns>
        public bool UpdateQHPositionLimitValue(ManagementCenter.Model.QH_PositionLimitValue model)
        {
            try
            {
                QH_PositionLimitValueDAL qHPositionLimitValueDAL = new QH_PositionLimitValueDAL();
                return qHPositionLimitValueDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6621";
                string errMsg = "修改(商品)期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region  删除(商品)期货_持仓限制

        /// <summary>
        /// 删除(商品)期货_持仓限制
        /// </summary>
        /// <param name="PositionLimitValueID">(商品)期货_持仓限制ID</param>
        /// <returns></returns>
        public bool DeleteQHPositionLimitValue(int PositionLimitValueID)
        {
            try
            {
                QH_PositionLimitValueDAL qHPositionLimitValueDAL = new QH_PositionLimitValueDAL();
                return qHPositionLimitValueDAL.Delete(PositionLimitValueID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6622";
                string errMsg = "删除(商品)期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据期货-持仓限制标识返回实体

        /// <summary>
        /// 根据期货-持仓限制标识返回实体
        /// </summary>
        /// <param name="PositionLimitValueID">期货-持仓限制标识</param>
        /// <returns></returns>
        public ManagementCenter.Model.QH_PositionLimitValue GetQHPositionLimitValueModel(int PositionLimitValueID)
        {
            try
            {
                return qH_PositionLimitValueDAL.GetModel(PositionLimitValueID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6623";
                string errMsg = "根据期货-持仓限制标识返回实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_PositionLimitValue GetModelByCache(int PositionLimitValueID)
        {
            string CacheKey = "QH_PositionLimitValueModel-" + PositionLimitValueID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_PositionLimitValueDAL.GetModel(PositionLimitValueID);
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
            return (ManagementCenter.Model.QH_PositionLimitValue) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_PositionLimitValueDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 获取所有(商品)期货_持仓限制

        /// <summary>
        ///获取所有(商品)期货_持仓限制 
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="DeliveryMonthTypeID">交割月份类型标识</param>
        /// <param name="PositionBailTypeID">持仓和保证金控制类型标识</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllQHPositionLimitValue(string BreedClassName, int DeliveryMonthTypeID, int PositionBailTypeID,
                                                  int pageNo, int pageSize,
                                                  out int rowCount)
        {
            try
            {
                QH_PositionLimitValueDAL qHPositionLimitValueDAL = new QH_PositionLimitValueDAL();
                return qHPositionLimitValueDAL.GetAllQHPositionLimitValue(BreedClassName, DeliveryMonthTypeID,
                                                                          PositionBailTypeID, pageNo, pageSize,
                                                                          out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-6624";
                string errMsg = "获取所有(商品)期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 根据查询条件获取所有的期货_持仓限制（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_PositionLimitValue> GetListArray(string strWhere)
        {
            try
            {
                return qH_PositionLimitValueDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6625";
                string errMsg = "根据查询条件获取所有的期货_持仓限制（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}