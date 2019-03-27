using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：最后交易日类型 业务逻辑类QH_LastTradingDayTypeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_LastTradingDayTypeBLL
    {
        private readonly ManagementCenter.DAL.QH_LastTradingDayTypeDAL qH_LastTradingDayTypeDAL =
            new ManagementCenter.DAL.QH_LastTradingDayTypeDAL();

        public QH_LastTradingDayTypeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_LastTradingDayTypeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LastTradingDayTypeID)
        {
            return qH_LastTradingDayTypeDAL.Exists(LastTradingDayTypeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.QH_LastTradingDayType model)
        {
            qH_LastTradingDayTypeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.QH_LastTradingDayType model)
        {
            qH_LastTradingDayTypeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LastTradingDayTypeID)
        {
            qH_LastTradingDayTypeDAL.Delete(LastTradingDayTypeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_LastTradingDayType GetModel(int LastTradingDayTypeID)
        {
            return qH_LastTradingDayTypeDAL.GetModel(LastTradingDayTypeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_LastTradingDayType GetModelByCache(int LastTradingDayTypeID)
        {
            string CacheKey = "QH_LastTradingDayTypeModel-" + LastTradingDayTypeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_LastTradingDayTypeDAL.GetModel(LastTradingDayTypeID);
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
            return (ManagementCenter.Model.QH_LastTradingDayType) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_LastTradingDayTypeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的最后交易日类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_LastTradingDayType> GetListArray(string strWhere)
        {
            try
            {
                return qH_LastTradingDayTypeDAL.GetListArray(strWhere);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion  成员方法
    }
}