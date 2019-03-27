using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：涨跌停板幅度类型 业务逻辑类QH_HighLowStopScopeTypeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_HighLowStopScopeTypeBLL
    {
        private readonly ManagementCenter.DAL.QH_HighLowStopScopeTypeDAL qH_HighLowStopScopeTypeDAL =
            new ManagementCenter.DAL.QH_HighLowStopScopeTypeDAL();

        public QH_HighLowStopScopeTypeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_HighLowStopScopeTypeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int HighLowStopScopeID)
        {
            return qH_HighLowStopScopeTypeDAL.Exists(HighLowStopScopeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.QH_HighLowStopScopeType model)
        {
            qH_HighLowStopScopeTypeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.QH_HighLowStopScopeType model)
        {
            qH_HighLowStopScopeTypeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int HighLowStopScopeID)
        {
            qH_HighLowStopScopeTypeDAL.Delete(HighLowStopScopeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_HighLowStopScopeType GetModel(int HighLowStopScopeID)
        {
            return qH_HighLowStopScopeTypeDAL.GetModel(HighLowStopScopeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_HighLowStopScopeType GetModelByCache(int HighLowStopScopeID)
        {
            string CacheKey = "QH_HighLowStopScopeTypeModel-" + HighLowStopScopeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_HighLowStopScopeTypeDAL.GetModel(HighLowStopScopeID);
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
            return (ManagementCenter.Model.QH_HighLowStopScopeType) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_HighLowStopScopeTypeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的涨跌停板幅度类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_HighLowStopScopeType> GetListArray(string strWhere)
        {
            try
            {
                return qH_HighLowStopScopeTypeDAL.GetListArray(strWhere);
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