using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：期货_品种_交割月份 业务逻辑类QH_CFPositionMonthBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_CFPositionMonthBLL
    {
        private readonly ManagementCenter.DAL.QH_CFPositionMonthDAL qH_CFPositionMonthDAL =
            new ManagementCenter.DAL.QH_CFPositionMonthDAL();

        public QH_CFPositionMonthBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_CFPositionMonthDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int DeliveryMonthTypeID)
        {
            return qH_CFPositionMonthDAL.Exists(DeliveryMonthTypeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.QH_CFPositionMonth model)
        {
            qH_CFPositionMonthDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.QH_CFPositionMonth model)
        {
            qH_CFPositionMonthDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int DeliveryMonthTypeID)
        {
            qH_CFPositionMonthDAL.Delete(DeliveryMonthTypeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_CFPositionMonth GetModel(int DeliveryMonthTypeID)
        {
            return qH_CFPositionMonthDAL.GetModel(DeliveryMonthTypeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_CFPositionMonth GetModelByCache(int DeliveryMonthTypeID)
        {
            string CacheKey = "QH_CFPositionMonthModel-" + DeliveryMonthTypeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_CFPositionMonthDAL.GetModel(DeliveryMonthTypeID);
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
            return (ManagementCenter.Model.QH_CFPositionMonth) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_CFPositionMonthDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的期货_品种_交割月份（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_CFPositionMonth> GetListArray(string strWhere)
        {
            try
            {
                return qH_CFPositionMonthDAL.GetListArray(strWhere);
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