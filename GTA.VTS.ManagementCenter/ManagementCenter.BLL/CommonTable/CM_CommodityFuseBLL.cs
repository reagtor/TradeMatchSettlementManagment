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
    ///描述：可交易商品_熔断表 业务逻辑类CM_CommodityFuseBLL 的摘要说明。错误编码范围:6700-6719
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_CommodityFuseBLL
    {
        private readonly CM_CommodityFuseDAL cM_CommodityFuseDAL =
            new CM_CommodityFuseDAL();

        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsCommodityCode(string CommodityCode)
        {
            try
            {
                return cM_CommodityFuseDAL.Exists(CommodityCode);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6700";
                string errMsg = "判断可交易商品_熔断中的记录是否存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #region 添加可交易商品_熔断

        /// <summary>
        /// 添加可交易商品_熔断
        /// </summary>
        /// <param name="model">可交易商品_熔断实体</param>
        /// <returns></returns>
        public bool AddCMCommodityFuse(CM_CommodityFuse model)
        {
            try
            {
                return cM_CommodityFuseDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6701";
                string errMsg = " 添加可交易商品_熔断失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 修改可交易商品_熔断

        /// <summary>
        /// 修改可交易商品_熔断
        /// </summary>
        /// <param name="model">可交易商品_熔断实体</param>
        /// <returns></returns>
        public bool UpdateCMCommodityFuse(CM_CommodityFuse model)
        {
            try
            {
                return cM_CommodityFuseDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6702";
                string errMsg = " 修改可交易商品_熔断失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 删除可交易商品_熔断

        /// <summary>
        /// 删除可交易商品_熔断
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public bool DeleteCMCommodityFuse(string CommodityCode)
        {
            try
            {
                return cM_CommodityFuseDAL.Delete(CommodityCode);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6703";
                string errMsg = "删除可交易商品_熔断失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 删除可交易商品_熔断(同时删除同一商品代码的熔断_时间段标识表中的记录)

        /// <summary>
        /// 删除可交易商品_熔断(同时删除同一商品代码的熔断_时间段标识表中的记录)
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public bool DeleteCMCommodityFuseAbout(string CommodityCode)
        {
            CM_CommodityFuseDAL cMCommodityFuseDAL = new CM_CommodityFuseDAL();
            CM_FuseTimesectionDAL cMFuseTimesectionDAL = new CM_FuseTimesectionDAL();
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                if (!string.IsNullOrEmpty(CommodityCode))
                {
                    if (cMFuseTimesectionDAL.DeleteByCommodityCode(CommodityCode, Tran, db))
                    {
                        if (cMCommodityFuseDAL.Delete(CommodityCode, Tran, db))
                        {
                            Tran.Commit();
                            return true;
                        }
                    }
                }
                Tran.Rollback();
                return false;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6704";
                string errMsg = "删除可交易商品_熔断(同时删除同一商品代码的熔断_时间段标识表中的记录)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public CM_CommodityFuse GetModel(string CommodityCode)
        {
            return cM_CommodityFuseDAL.GetModel(CommodityCode);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public CM_CommodityFuse GetModelByCache(string CommodityCode)
        {
            string CacheKey = "CM_CommodityFuseModel-" + CommodityCode;
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_CommodityFuseDAL.GetModel(CommodityCode);
                    if (objModel != null)
                    {
                        int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                                           TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return (CM_CommodityFuse) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_CommodityFuseDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的可交易商品_熔断表（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<CM_CommodityFuse> GetListArray(string strWhere)
        {
            return cM_CommodityFuseDAL.GetListArray(strWhere);
        }

        #region 获取所有可交易商品_熔断

        /// <summary>
        /// 获取所有可交易商品_熔断
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMCommodityFuse(string CommodityCode, int pageNo, int pageSize,
                                             out int rowCount)
        {
            try
            {
                CM_CommodityFuseDAL cMCommodityFuseDAL = new CM_CommodityFuseDAL();
                return cMCommodityFuseDAL.GetAllCMCommodityFuse(CommodityCode, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-6705";
                string errMsg = "获取所有可交易商品_熔断失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}