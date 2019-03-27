using System;
using System.Collections.Generic;
using System.Data;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：字段_范围 业务逻辑类CM_FieldRangeBLL 的摘要说明。
    /// 错误编码范围:5340-5359
    ///作者：刘书伟
    ///日期:2008-11-27
    /// </summary>
    public class CM_FieldRangeBLL
    {
        private readonly ManagementCenter.DAL.CM_FieldRangeDAL cM_FieldRangeDAL =
            new ManagementCenter.DAL.CM_FieldRangeDAL();

        public CM_FieldRangeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_FieldRangeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int FieldRangeID)
        {
            return cM_FieldRangeDAL.Exists(FieldRangeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.CM_FieldRange model)
        {
            return cM_FieldRangeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CM_FieldRange model)
        {
            cM_FieldRangeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FieldRangeID)
        {
            cM_FieldRangeDAL.Delete(FieldRangeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_FieldRange GetModel(int FieldRangeID)
        {
            return cM_FieldRangeDAL.GetModel(FieldRangeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_FieldRange GetModelByCache(int FieldRangeID)
        {
            string CacheKey = "CM_FieldRangeModel-" + FieldRangeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_FieldRangeDAL.GetModel(FieldRangeID);
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
            return (ManagementCenter.Model.CM_FieldRange) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_FieldRangeDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 根据查询条件获取所有的字段_范围（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的字段_范围（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_FieldRange> GetListArray(string strWhere)

        {
            try
            {
                return cM_FieldRangeDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5340";
                string errMsg = "根据查询条件获取所有的字段_范围(查询条件可为空)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #endregion  成员方法
    }
}