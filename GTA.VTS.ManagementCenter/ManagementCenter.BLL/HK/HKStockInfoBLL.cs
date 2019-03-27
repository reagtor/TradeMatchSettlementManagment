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
    ///描述：港股股票代码 业务逻辑类HKStockInfoBLL 的摘要说明。错误编码范围:7767-7769
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HKStockInfoBLL
    {
        private readonly HKStockInfoDAL hKStockInfoDAL = new HKStockInfoDAL();
        public HKStockInfoBLL()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string StockCode)
        {
            return hKStockInfoDAL.Exists(StockCode);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.HKStockInfo model)
        {
            hKStockInfoDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.HKStockInfo model)
        {
            hKStockInfoDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string StockCode)
        {

            hKStockInfoDAL.Delete(StockCode);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.HKStockInfo GetModel(string StockCode)
        {

            return hKStockInfoDAL.GetModel(StockCode);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.HKStockInfo GetModelByCache(string StockCode)
        {

            string CacheKey = "HKStockInfoModel-" + StockCode;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = hKStockInfoDAL.GetModel(StockCode);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (ManagementCenter.Model.HKStockInfo)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return hKStockInfoDAL.GetList(strWhere);
        }
        
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的港股股票代码（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HKStockInfo> GetListArray(string strWhere)
        {
            try
            {
                return hKStockInfoDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7767";
                string errMsg = "根据查询条件获取所有的港股股票代码（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据查询条件获取新增的港股代码信息(查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件（可为空）</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HKStockInfo> GetHKStockInfoList(string strWhere)
        {
            try
            {
                HKStockInfoDAL hKStockInfoDAL = new HKStockInfoDAL();
                return hKStockInfoDAL.GetHKStockInfoList(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7768";
                string errMsg = "根据查询条件获取新增的港股代码信息(查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }


        #endregion  成员方法
    }
}
