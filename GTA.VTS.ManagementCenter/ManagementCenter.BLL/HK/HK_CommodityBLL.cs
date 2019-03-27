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
    ///描述：港股交易商品 业务逻辑类HK_CommodityBLL 的摘要说明。错误编码范围:7750-7769
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_CommodityBLL
    {
        private readonly HK_CommodityDAL hK_CommodityDAL = new HK_CommodityDAL();
        public HK_CommodityBLL()
        { }
        #region  成员方法

        #region 是否存在该记录
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string HKCommodityCode)
        {
            return hK_CommodityDAL.Exists(HKCommodityCode);
        }
        #endregion

        #region 添加港股交易商品

        /// <summary>
        /// 添加港股交易商品
        /// </summary>
        /// <param name="model">港股交易商品实体</param>
        /// <returns></returns>
        public bool AddHKCommodity(ManagementCenter.Model.HK_Commodity model)
        {
            try
            {
                return hK_CommodityDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7750";
                string errMsg = "添加港股交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 更新港股交易商品
        /// <summary>
        /// 更新港股交易商品
        /// </summary>
        /// <param name="model">港股交易商品实体</param>
        /// <returns></returns>
        public bool UpdateHKCommodity(ManagementCenter.Model.HK_Commodity model)
        {
            try
            {
                return hK_CommodityDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7751";
                string errMsg = "更新港股交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据港股交易商品代码删除港股交易商品（相关表的记录同时删除）

        /// <summary>
        /// 根据港股交易商品代码删除港股交易商品（相关表的记录同时删除）
        /// </summary>
        /// <param name="HKCommodityCode">港股商品代码</param>
        /// <returns></returns>
        public bool DeleteHKCommodity(string HKCommodityCode)
        {
            HK_CommodityDAL hKCommodityDAL = new HK_CommodityDAL();
            RC_TradeCommodityAssignDAL TradeCommodityAssignDAL = new RC_TradeCommodityAssignDAL();
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
                TradeCommodityAssignDAL.DeleteByCommodityCode(HKCommodityCode, Tran, db);
                if (!hKCommodityDAL.Delete(HKCommodityCode, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-7752";
                string errMsg = "删除港股交易商品失败!";
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

        #region 得到港股交易商品的对象实体
        /// <summary>
        /// 得到港股交易商品的对象实体
        /// </summary>
        /// <param name="HKCommodityCode">港股交易商品的实体</param>
        /// <returns></returns>
        public ManagementCenter.Model.HK_Commodity GetModel(string HKCommodityCode)
        {
            return hK_CommodityDAL.GetModel(HKCommodityCode);
        }
        #endregion

        #region 得到港股交易商品的对象实体，从缓存中。
        /// <summary>
        /// 得到港股交易商品的对象实体，从缓存中。
        /// </summary>
        /// <param name="HKCommodityCode">港股交易商品的实体</param>
        /// <returns></returns>
        public ManagementCenter.Model.HK_Commodity GetModelByCache(string HKCommodityCode)
        {

            string CacheKey = "HK_CommodityModel-" + HKCommodityCode;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = hK_CommodityDAL.GetModel(HKCommodityCode);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (ManagementCenter.Model.HK_Commodity)objModel;
        }
        #endregion

        #region 根据查询条件获得港股交易商品的数据列表
        /// <summary>
        /// 根据查询条件获得港股交易商品的数据列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetList(string strWhere)
        {
            return hK_CommodityDAL.GetList(strWhere);
        }
        #endregion

        #region 获取所有港股交易商品
        /// <summary>
        /// 获取所有港股交易商品
        /// </summary>
        /// <param name="HKCommodityCode">港股商品代码</param>
        /// <param name="HKCommodityName">港股商品名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllHKCommodity(string HKCommodityCode, string HKCommodityName, int pageNo,
                                         int pageSize,
                                         out int rowCount)
        {
            try
            {
                HK_CommodityDAL hKCommodityDAL = new HK_CommodityDAL();
                return hKCommodityDAL.GetAllHKCommodity(HKCommodityCode, HKCommodityName, pageNo, pageSize,
                                                        out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-7753";
                string errMsg = "获取所有港股交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获得港股交易商品的数据列表
        /// <summary>
        /// 获得港股交易商品的数据列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region 根据查询条件获取所有的港股交易商品（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的港股交易商品（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HK_Commodity> GetListArray(string strWhere)
        {
            try
            {
                return hK_CommodityDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7754";
                string errMsg = "根据查询条件获取所有的港股交易商品（查询条件可为空）";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region  获取所有港股代码及昨日收盘价
        /// <summary>
        /// 获取所有港股代码及昨日收盘价
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HK_Commodity> GetListHKCommodityAndClosePrice(string strWhere)
        {
            try
            {
                return hK_CommodityDAL.GetListHKCommodityAndClosePrice(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7758";
                string errMsg = "获取所有港股代码及昨日收盘价（查询条件可为空）";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 判断港股交易商品代码是否已经存在

        /// <summary>
        /// 判断港股交易商品代码是否已经存在
        /// </summary>
        /// <param name="HKCommodityCode">港股商品代码</param>
        /// <returns></returns>
        public bool IsExistHKCommodityCode(string HKCommodityCode)
        {
            try
            {
                HK_CommodityDAL hKCommodityDAL = new HK_CommodityDAL();
                string strWhere = string.Format("HKCommodityCode='{0}'", HKCommodityCode);
                DataSet _ds = hKCommodityDAL.GetList(strWhere);
                if (_ds != null)
                {
                    if (_ds.Tables[0].Rows.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7755";
                string errMsg = "判断港股交易商品代码是否已经存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 判断港股交易商品名称是否已经存在

        /// <summary>
        /// 判断港股交易商品名称是否已经存在
        /// </summary>
        /// <param name="HKCommodityName">港股商品名称</param>
        /// <returns></returns>
        public bool IsExistHKCommodityName(string HKCommodityName)
        {
            try
            {
                HK_CommodityDAL hKCommodityDAL = new HK_CommodityDAL();
                string strWhere = string.Format("HKCommodityName='{0}'", HKCommodityName);
                DataSet _ds = hKCommodityDAL.GetList(strWhere);
                if (_ds != null)
                {
                    if (_ds.Tables[0].Rows.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7756";
                string errMsg = "判断港股交易商品名称是否已经存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 提供前台获取港股商品代码的方法

        /// <summary>
        /// 提供前台获取港股商品代码的方法
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.OnstageHK_Commodity> GetHKCommodityListArray(string strWhere)
        {
            try
            {
                return hK_CommodityDAL.GetHKCommodityListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7757";
                string errMsg = "提供前台获取港股商品代码的方法失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}
