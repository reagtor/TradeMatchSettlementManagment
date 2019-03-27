using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：权证涨跌幅价格 业务逻辑类XH_RightHightLowPrices 的摘要说明。
    /// 作者：刘书伟
    /// 日期：2008-11-26
    /// </summary>
    public class XH_RightHightLowPricesBLL
    {
        private readonly ManagementCenter.DAL.XH_RightHightLowPricesDAL dal = new XH_RightHightLowPricesDAL();

        public XH_RightHightLowPricesBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int RightHightLowPriceID)
        {
            return dal.Exists(RightHightLowPriceID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.XH_RightHightLowPrices model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.XH_RightHightLowPrices model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int RightHightLowPriceID)
        {
            dal.Delete(RightHightLowPriceID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_RightHightLowPrices GetModel(int RightHightLowPriceID)
        {
            return dal.GetModel(RightHightLowPriceID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_RightHightLowPrices GetModelByCache(int RightHightLowPriceID)
        {
            string CacheKey = "XH_RightHightLowPricesModel-" + RightHightLowPriceID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(RightHightLowPriceID);
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
            return (ManagementCenter.Model.XH_RightHightLowPrices) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}
        /// <summary>
        /// 获取所有的权证涨跌幅价格（查询条件可为空）
        /// <param name="strWhere">查询条件</param>
        /// </summary>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_RightHightLowPrices> GetListArray(string strWhere)
        {
            XH_RightHightLowPricesDAL xH_RightHightLowPricesDAL = new XH_RightHightLowPricesDAL();
            return xH_RightHightLowPricesDAL.GetListArray(strWhere);
        }

        #endregion  成员方法
    }
}