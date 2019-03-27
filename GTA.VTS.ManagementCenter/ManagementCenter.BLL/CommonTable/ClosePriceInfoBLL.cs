using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.DAL.CommonTable;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：股票收盘价 数据访问类ClosePriceInfoBLL。
    ///作者：刘书伟
    ///日期:2009-11-27
    /// </summary>
    public class ClosePriceInfoBLL
    {
        private readonly ClosePriceInfoDAL closePriceInfoDAL = new ClosePriceInfoDAL();

        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string StockCode)
        {
            return closePriceInfoDAL.Exists(StockCode);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.ClosePriceInfo model)
        {
            closePriceInfoDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.ClosePriceInfo model)
        {
            closePriceInfoDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string StockCode)
        {

            closePriceInfoDAL.Delete(StockCode);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.ClosePriceInfo GetModel(string StockCode)
        {

            return closePriceInfoDAL.GetModel(StockCode);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.ClosePriceInfo GetModelByCache(string StockCode)
        {

            string CacheKey = "ClosePriceInfoModel-" + StockCode;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = closePriceInfoDAL.GetModel(StockCode);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (ManagementCenter.Model.ClosePriceInfo)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return closePriceInfoDAL.GetList(strWhere);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<ManagementCenter.Model.ClosePriceInfo> GetModelList(string strWhere)
        {
            DataSet ds = closePriceInfoDAL.GetList(strWhere);
            List<ManagementCenter.Model.ClosePriceInfo> modelList = new List<ManagementCenter.Model.ClosePriceInfo>();
            int rowsCount = ds.Tables[0].Rows.Count;
            if (rowsCount > 0)
            {
                ManagementCenter.Model.ClosePriceInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new ManagementCenter.Model.ClosePriceInfo();
                    model.StockCode = ds.Tables[0].Rows[n]["StockCode"].ToString();
                    if (ds.Tables[0].Rows[n]["ClosePrice"].ToString() != "")
                    {
                        model.ClosePrice = decimal.Parse(ds.Tables[0].Rows[n]["ClosePrice"].ToString());
                    }
                    if (ds.Tables[0].Rows[n]["ClosePriceDate"].ToString() != "")
                    {
                        model.ClosePriceDate = DateTime.Parse(ds.Tables[0].Rows[n]["ClosePriceDate"].ToString());
                    }
                    if (ds.Tables[0].Rows[n]["BreedClassTypeID"].ToString() != "")
                    {
                        model.BreedClassTypeID = int.Parse(ds.Tables[0].Rows[n]["BreedClassTypeID"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 根据查询条件获取所有的股票收盘价（查询条件可为空）

        /// <summary>
        /// 根据查询条件获取所有的股票收盘价（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">条件查询</param>
        /// <returns></returns>
        public List<ClosePriceInfo> GetListArray(string strWhere)
        {
            try
            {
                return closePriceInfoDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4399";
                string errMsg = "根据查询条件获取所有的股票收盘价（查询条件可为空）失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}
