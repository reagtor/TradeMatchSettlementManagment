using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：股票分红记录_股票 业务逻辑类CM_StockMelonStockBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_StockMelonStockBLL
    {
        private readonly ManagementCenter.DAL.CM_StockMelonStockDAL cM_StockMelonStockDAL =
            new ManagementCenter.DAL.CM_StockMelonStockDAL();

        public CM_StockMelonStockBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_StockMelonStockDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int StockMelonStockID)
        {
            return cM_StockMelonStockDAL.Exists(StockMelonStockID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.CM_StockMelonStock model)
        {
            return cM_StockMelonStockDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CM_StockMelonStock model)
        {
            cM_StockMelonStockDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int StockMelonStockID)
        {
            cM_StockMelonStockDAL.Delete(StockMelonStockID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_StockMelonStock GetModel(int StockMelonStockID)
        {
            return cM_StockMelonStockDAL.GetModel(StockMelonStockID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_StockMelonStock GetModelByCache(int StockMelonStockID)
        {
            string CacheKey = "CM_StockMelonStockModel-" + StockMelonStockID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_StockMelonStockDAL.GetModel(StockMelonStockID);
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
            return (ManagementCenter.Model.CM_StockMelonStock) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_StockMelonStockDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的股票分红记录_股票（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_StockMelonStock> GetListArray(string strWhere)
        {
            return cM_StockMelonStockDAL.GetListArray(strWhere);
        }

        #endregion  成员方法
    }
}