using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：交易货币类型 业务逻辑类CM_CurrencyTypeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_CurrencyTypeBLL
    {
        private readonly ManagementCenter.DAL.CM_CurrencyTypeDAL cM_CurrencyTypeDAL =
            new ManagementCenter.DAL.CM_CurrencyTypeDAL();

        public CM_CurrencyTypeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_CurrencyTypeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CurrencyTypeID)
        {
            return cM_CurrencyTypeDAL.Exists(CurrencyTypeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.CM_CurrencyType model)
        {
            cM_CurrencyTypeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CM_CurrencyType model)
        {
            cM_CurrencyTypeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CurrencyTypeID)
        {
            cM_CurrencyTypeDAL.Delete(CurrencyTypeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_CurrencyType GetModel(int CurrencyTypeID)
        {
            return cM_CurrencyTypeDAL.GetModel(CurrencyTypeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_CurrencyType GetModelByCache(int CurrencyTypeID)
        {
            string CacheKey = "CM_CurrencyTypeModel-" + CurrencyTypeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_CurrencyTypeDAL.GetModel(CurrencyTypeID);
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
            return (ManagementCenter.Model.CM_CurrencyType) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_CurrencyTypeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的交易货币类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_CurrencyType> GetListArray(string strWhere)

        {
            return cM_CurrencyTypeDAL.GetListArray(strWhere);
        }

        /// <summary>
        /// 根据品种找到币种
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public ManagementCenter.Model.CM_CurrencyBreedClassType GetCurrencyByBreedClassID(int BreedClassID)
        {
            return cM_CurrencyTypeDAL.GetCurrencyByBreedClassID(BreedClassID);
        }

        /// <summary>
        /// 获取所有品种和币种的对应关系
        /// </summary>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_CurrencyBreedClassType> GetListCurrencyBreedClass()
        {
            return cM_CurrencyTypeDAL.GetListCurrencyBreedClass();
        }

        #endregion  成员方法
    }
}