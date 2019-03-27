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
    ///描述：港股交易规则_最小变动价位范围值 业务逻辑类HK_MinPriceFieldRangeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_MinPriceFieldRangeBLL
    {
        private readonly HK_MinPriceFieldRangeDAL hK_MinPriceFieldRangeDAL = new HK_MinPriceFieldRangeDAL();
        public HK_MinPriceFieldRangeBLL()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return hK_MinPriceFieldRangeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int FieldRangeID)
        {
            return hK_MinPriceFieldRangeDAL.Exists(FieldRangeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.HK_MinPriceFieldRange model)
        {
            hK_MinPriceFieldRangeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.HK_MinPriceFieldRange model)
        {
            hK_MinPriceFieldRangeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FieldRangeID)
        {

            hK_MinPriceFieldRangeDAL.Delete(FieldRangeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.HK_MinPriceFieldRange GetModel(int FieldRangeID)
        {

            return hK_MinPriceFieldRangeDAL.GetModel(FieldRangeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.HK_MinPriceFieldRange GetModelByCache(int FieldRangeID)
        {

            string CacheKey = "HK_MinPriceFieldRangeModel-" + FieldRangeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = hK_MinPriceFieldRangeDAL.GetModel(FieldRangeID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (ManagementCenter.Model.HK_MinPriceFieldRange)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return hK_MinPriceFieldRangeDAL.GetList(strWhere);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<ManagementCenter.Model.HK_MinPriceFieldRange> GetModelList(string strWhere)
        {
            DataSet ds = hK_MinPriceFieldRangeDAL.GetList(strWhere);
            List<ManagementCenter.Model.HK_MinPriceFieldRange> modelList = new List<ManagementCenter.Model.HK_MinPriceFieldRange>();
            int rowsCount = ds.Tables[0].Rows.Count;
            if (rowsCount > 0)
            {
                ManagementCenter.Model.HK_MinPriceFieldRange model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new ManagementCenter.Model.HK_MinPriceFieldRange();
                    if (ds.Tables[0].Rows[n]["FieldRangeID"].ToString() != "")
                    {
                        model.FieldRangeID = int.Parse(ds.Tables[0].Rows[n]["FieldRangeID"].ToString());
                    }
                    if (ds.Tables[0].Rows[n]["UpperLimit"].ToString() != "")
                    {
                        model.UpperLimit = decimal.Parse(ds.Tables[0].Rows[n]["UpperLimit"].ToString());
                    }
                    if (ds.Tables[0].Rows[n]["LowerLimit"].ToString() != "")
                    {
                        model.LowerLimit = decimal.Parse(ds.Tables[0].Rows[n]["LowerLimit"].ToString());
                    }
                    if (ds.Tables[0].Rows[n]["Value"].ToString() != "")
                    {
                        model.Value = decimal.Parse(ds.Tables[0].Rows[n]["Value"].ToString());
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

        /// <summary>
        /// 根据查询条件获取所有的港股交易规则_最小变动价位范围值（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HK_MinPriceFieldRange> GetListArray(string strWhere)
        {
            try
            {
                return hK_MinPriceFieldRangeDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "";
                string errMsg = "";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}
