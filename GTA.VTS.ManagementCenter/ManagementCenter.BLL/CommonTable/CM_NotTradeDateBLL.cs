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
    /// 描述：交易所类型_非交易日期 业务逻辑类CM_NotTradeDate 的摘要说明。错误编码范围:4720-4739
    /// 作者：刘书伟
    /// 日期:2008-11-26
    /// 修改：叶振东
    /// 时间：2010-04-07
    /// 描述：添加根据交易所类型和非交易日时间来查是否存在记录方法
    /// </summary>
    public class CM_NotTradeDateBLL
    {
        private readonly ManagementCenter.DAL.CM_NotTradeDateDAL cM_NotTradeDateDAL = new CM_NotTradeDateDAL();

        public CM_NotTradeDateBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_NotTradeDateDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int NotTradeDateID)
        {
            return cM_NotTradeDateDAL.Exists(NotTradeDateID);
        }

        /// <summary>
        /// 添加非交易日
        /// </summary>
        /// <param name="model">非交易日实体</param>
        /// <returns></returns>
        public int AddCMNotTradeDate(ManagementCenter.Model.CM_NotTradeDate model)
        {
            try
            {
                return cM_NotTradeDateDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4720";
                string errMsg = "添加非交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        /// <summary>
        /// 更新非交易日
        /// </summary>
        /// <param name="model">非交易日实体</param>
        /// <returns></returns>
        public bool UpdateCMNotTradeDate(ManagementCenter.Model.CM_NotTradeDate model)
        {
            try
            {
                return cM_NotTradeDateDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4722";
                string errMsg = "修改非交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 删除非交易日
        /// </summary>
        /// <param name="NotTradeDateID">非交易日ID</param>
        /// <returns></returns>
        public bool DeleteCMNotTradeDate(int NotTradeDateID)
        {
            try
            {
                return cM_NotTradeDateDAL.Delete(NotTradeDateID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4721";
                string errMsg = "删除非交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_NotTradeDate GetModel(int NotTradeDateID)
        {
            return cM_NotTradeDateDAL.GetModel(NotTradeDateID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_NotTradeDate GetModelByCache(int NotTradeDateID)
        {
            string CacheKey = "CM_NotTradeDateModel-" + NotTradeDateID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_NotTradeDateDAL.GetModel(NotTradeDateID);
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
            return (ManagementCenter.Model.CM_NotTradeDate) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_NotTradeDateDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 获取所有交易所类型_非交易日期

        /// <summary>
        /// 获取所有交易所类型_非交易日期
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMNotTradeDate(string BourseTypeName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            try
            {
                CM_NotTradeDateDAL cMNotTradeDateDAL = new CM_NotTradeDateDAL();
                return cMNotTradeDateDAL.GetAllCMNotTradeDate(BourseTypeName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-4723";
                string errMsg = "获取所有交易所类型_非交易日期失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称

        /// <summary>
        ///根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetCMNotTradeDateBourseTypeName()
        {
            try
            {
                CM_NotTradeDateDAL cMNotTradeDateDAL = new CM_NotTradeDateDAL();
                return cMNotTradeDateDAL.GetCMNotTradeDateBourseTypeName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4724";
                string errMsg = "根据交易所类型_非交易日期表中的交易所类型ID获取交易所类型名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 根据查询条件获取所有的非交易日期（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">条件查询</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_NotTradeDate> GetListArray(string strWhere)
        {
            try
            {
                return cM_NotTradeDateDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-4725";
                string errMsg = "根据查询条件获取所有的非交易日期失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }

        }
        /// <summary>
        ///  根据交易所类型和非交易日时间来查是否存在记录
        /// </summary>
        /// <param name="BourseTypeID">交易所类型</param>
        /// <param name="NotTradeDay">非交易日时间</param>
        /// <returns></returns>
        public ManagementCenter.Model.CM_NotTradeDate GetNotTradeDate(int BourseTypeID, DateTime NotTradeDay)
        {
            try
            {
                return cM_NotTradeDateDAL.GetNotTradeDate(BourseTypeID,NotTradeDay);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4726";
                string errMsg = "根据交易所类型和非交易日查询失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion  成员方法
    }
}