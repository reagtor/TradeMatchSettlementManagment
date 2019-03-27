using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：股票分红记录_现金 业务逻辑类CM_StockMelonCashBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_StockMelonCashBLL
    {
        private readonly ManagementCenter.DAL.CM_StockMelonCashDAL cM_StockMelonCashDAL =
            new ManagementCenter.DAL.CM_StockMelonCashDAL();

        public CM_StockMelonCashBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_StockMelonCashDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int StockMelonCuttingCashID)
        {
            return cM_StockMelonCashDAL.Exists(StockMelonCuttingCashID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.CM_StockMelonCash model)
        {
            return cM_StockMelonCashDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CM_StockMelonCash model)
        {
            cM_StockMelonCashDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int StockMelonCuttingCashID)
        {
            cM_StockMelonCashDAL.Delete(StockMelonCuttingCashID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_StockMelonCash GetModel(int StockMelonCuttingCashID)
        {
            return cM_StockMelonCashDAL.GetModel(StockMelonCuttingCashID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_StockMelonCash GetModelByCache(int StockMelonCuttingCashID)
        {
            string CacheKey = "CM_StockMelonCashModel-" + StockMelonCuttingCashID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_StockMelonCashDAL.GetModel(StockMelonCuttingCashID);
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
            return (ManagementCenter.Model.CM_StockMelonCash) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_StockMelonCashDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的股票分红记录_现金（查询条件可为空）
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_StockMelonCash> GetListArray(string strWhere)
        {
            return cM_StockMelonCashDAL.GetListArray(strWhere);
        }

        #endregion  成员方法
    }
}