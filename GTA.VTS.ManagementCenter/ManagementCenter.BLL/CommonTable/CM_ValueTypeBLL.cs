using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：交易规则_取值类型 业务逻辑类CM_ValueTypeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_ValueTypeBLL
    {
        private readonly ManagementCenter.DAL.CM_ValueTypeDAL cM_ValueTypeDAL =
            new ManagementCenter.DAL.CM_ValueTypeDAL();

        public CM_ValueTypeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_ValueTypeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ValueTypeID)
        {
            return cM_ValueTypeDAL.Exists(ValueTypeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.CM_ValueType model)
        {
            return cM_ValueTypeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CM_ValueType model)
        {
            cM_ValueTypeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ValueTypeID)
        {
            cM_ValueTypeDAL.Delete(ValueTypeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_ValueType GetModel(int ValueTypeID)
        {
            return cM_ValueTypeDAL.GetModel(ValueTypeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_ValueType GetModelByCache(int ValueTypeID)
        {
            string CacheKey = "CM_ValueTypeModel-" + ValueTypeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_ValueTypeDAL.GetModel(ValueTypeID);
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
            return (ManagementCenter.Model.CM_ValueType) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_ValueTypeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的交易规则_取值类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_ValueType> GetListArray(string strWhere)
        {
            return cM_ValueTypeDAL.GetListArray(strWhere);
        }

        #endregion  成员方法
    }
}