using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述:币种之间兑换类型 业务逻辑类CM_CurrencyExchangeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_CurrencyExchangeBLL
    {
        private readonly ManagementCenter.DAL.CM_CurrencyExchangeDAL cM_CurrencyExchangeDAL =
            new ManagementCenter.DAL.CM_CurrencyExchangeDAL();

        public CM_CurrencyExchangeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_CurrencyExchangeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CurrencyExchangeID)
        {
            return cM_CurrencyExchangeDAL.Exists(CurrencyExchangeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.CM_CurrencyExchange model)
        {
            return cM_CurrencyExchangeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CM_CurrencyExchange model)
        {
            cM_CurrencyExchangeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CurrencyExchangeID)
        {
            cM_CurrencyExchangeDAL.Delete(CurrencyExchangeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_CurrencyExchange GetModel(int CurrencyExchangeID)
        {
            return cM_CurrencyExchangeDAL.GetModel(CurrencyExchangeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_CurrencyExchange GetModelByCache(int CurrencyExchangeID)
        {
            string CacheKey = "CM_CurrencyExchangeModel-" + CurrencyExchangeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_CurrencyExchangeDAL.GetModel(CurrencyExchangeID);
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
            return (ManagementCenter.Model.CM_CurrencyExchange) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_CurrencyExchangeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的币种之间兑换类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_CurrencyExchange> GetListArray(string strWhere)
        {
            try
            {
                return cM_CurrencyExchangeDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
                //throw;
            }
        }

        #endregion  成员方法
    }
}