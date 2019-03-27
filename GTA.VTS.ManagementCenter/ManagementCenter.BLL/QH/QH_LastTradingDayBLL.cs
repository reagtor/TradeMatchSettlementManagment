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
    ///描述：最后交易日 业务逻辑类QH_LastTradingDayBLL 的摘要说明。错误编码范围:6050-6059
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class QH_LastTradingDayBLL
    {
        private readonly ManagementCenter.DAL.QH_LastTradingDayDAL qH_LastTradingDayDAL =
            new ManagementCenter.DAL.QH_LastTradingDayDAL();

        public QH_LastTradingDayBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_LastTradingDayDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LastTradingDayID)
        {
            return qH_LastTradingDayDAL.Exists(LastTradingDayID);
        }

        /// <summary>
        /// 添加最后交易日
        /// </summary>
        /// <param name="model">最后交易日实体</param>
        /// <returns></returns>
        public int AddQHLastTradingDay(ManagementCenter.Model.QH_LastTradingDay model)
        {
            try
            {
                QH_LastTradingDayDAL qHLastTradingDayDAL = new QH_LastTradingDayDAL();
                return qHLastTradingDayDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6050";
                string errMsg = "添加最后交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        /// <summary>
        /// 更新最后交易日
        /// </summary>
        /// <param name="model">最后交易日实体</param>
        /// <returns></returns>
        public bool UpdateQHLastTradingDay(ManagementCenter.Model.QH_LastTradingDay model)
        {
            try
            {
                QH_LastTradingDayDAL qHLastTradingDayDAL = new QH_LastTradingDayDAL();
                return qHLastTradingDayDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6051";
                string errMsg = "更新最后交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 删除最后交易日
        /// </summary>
        /// <param name="LastTradingDayID">最后交易日ID</param>
        /// <returns></returns>
        public bool DeleteQHLastTradingDay(int LastTradingDayID)
        {
            try
            {
                QH_LastTradingDayDAL qHLastTradingDayDAL = new QH_LastTradingDayDAL();
                return qHLastTradingDayDAL.Delete(LastTradingDayID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6052";
                string errMsg = "删除最后交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #region 根据最后交易日ID，获取最后交易日实体
        /// <summary>
       /// 根据最后交易日ID，获取最后交易日实体
       /// </summary>
       /// <param name="LastTradingDayID">最后交易日ID</param>
       /// <returns></returns>
        public ManagementCenter.Model.QH_LastTradingDay GetQHLastTradingDayModel(int LastTradingDayID)
        {
            try
            {
                QH_LastTradingDayDAL qHLastTradingDayDAL = new QH_LastTradingDayDAL();
                return qHLastTradingDayDAL.GetModel(LastTradingDayID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6053";
                string errMsg = "根据最后交易日ID，获取最后交易日实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_LastTradingDay GetModelByCache(int LastTradingDayID)
        {
            string CacheKey = "QH_LastTradingDayModel-" + LastTradingDayID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_LastTradingDayDAL.GetModel(LastTradingDayID);
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
            return (ManagementCenter.Model.QH_LastTradingDay) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_LastTradingDayDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的最后交易日（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_LastTradingDay> GetListArray(string strWhere)
        {
            try
            {
                return qH_LastTradingDayDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6054";
                string errMsg = "根据查询条件获取所有的最后交易日（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}