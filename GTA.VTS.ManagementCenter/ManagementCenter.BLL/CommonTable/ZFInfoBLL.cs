using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：增发股票代码 业务逻辑类ZFInfo 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20
    /// </summary>
    public class ZFInfoBLL
    {
        private readonly ManagementCenter.DAL.ZFInfoDAL dal = new ZFInfoDAL();

        public ZFInfoBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string stkcd)
        {
            return dal.Exists(stkcd);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.ZFInfo model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.ZFInfo model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string stkcd)
        {
            dal.Delete(stkcd);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.ZFInfo GetModel(string stkcd)
        {
            return dal.GetModel(stkcd);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.ZFInfo GetModelByCache(string stkcd)
        {
            string CacheKey = "ZFInfoModel-" + stkcd;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(stkcd);
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
            return (ManagementCenter.Model.ZFInfo) objModel;
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
        /// 获取实体列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.ZFInfo> GetListArray(string strWhere)
        {
            return dal.GetListArray(strWhere);
        }

        #endregion  成员方法
    }
}