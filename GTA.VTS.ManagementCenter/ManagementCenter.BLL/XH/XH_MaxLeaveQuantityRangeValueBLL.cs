using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：交易规则_最大委托量_范围_值  业务逻辑类XH_MaxLeaveQuantityRangeValueBLL 的摘要说明。
    /// 说明：已没有交易规则_最大委托量_范围_值 功能
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class XH_MaxLeaveQuantityRangeValueBLL
    {
        private readonly ManagementCenter.DAL.XH_MaxLeaveQuantityRangeValueDAL xH_MaxLeaveQuantityRangeValueDAL =
            new ManagementCenter.DAL.XH_MaxLeaveQuantityRangeValueDAL();

        public XH_MaxLeaveQuantityRangeValueBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_MaxLeaveQuantityRangeValueDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID, int FieldRangeID)
        {
            return xH_MaxLeaveQuantityRangeValueDAL.Exists(BreedClassID, FieldRangeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.XH_MaxLeaveQuantityRangeValue model)
        {
            xH_MaxLeaveQuantityRangeValueDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.XH_MaxLeaveQuantityRangeValue model)
        {
            xH_MaxLeaveQuantityRangeValueDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int BreedClassID, int FieldRangeID)
        {
            xH_MaxLeaveQuantityRangeValueDAL.Delete(BreedClassID, FieldRangeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_MaxLeaveQuantityRangeValue GetModel(int BreedClassID, int FieldRangeID)
        {
            return xH_MaxLeaveQuantityRangeValueDAL.GetModel(BreedClassID, FieldRangeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_MaxLeaveQuantityRangeValue GetModelByCache(int BreedClassID, int FieldRangeID)
        {
            string CacheKey = "XH_MaxLeaveQuantityRangeValueModel-" + BreedClassID + FieldRangeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_MaxLeaveQuantityRangeValueDAL.GetModel(BreedClassID, FieldRangeID);
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
            return (ManagementCenter.Model.XH_MaxLeaveQuantityRangeValue) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_MaxLeaveQuantityRangeValueDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的交易规则_最大委托量_范围_值（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_MaxLeaveQuantityRangeValue> GetListArray(string strWhere)
        {
            try
            {
                return xH_MaxLeaveQuantityRangeValueDAL.GetListArray(strWhere);
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