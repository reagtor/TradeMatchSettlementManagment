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
    ///描述：品种_股指期货_保证金 业务逻辑类QH_SIFBailBLL 的摘要说明。错误编码范围:6640-6659
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_SIFBailBLL
    {
        private readonly ManagementCenter.DAL.QH_SIFBailDAL qH_SIFBailDAL = new ManagementCenter.DAL.QH_SIFBailDAL();

        public QH_SIFBailBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_SIFBailDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID)
        {
            return qH_SIFBailDAL.Exists(BreedClassID);
        }

       /// <summary>
       /// 添加品种_股指期货_保证金
       /// </summary>
        /// <param name="model">品种_股指期货_保证金实体</param>
        public void Add(ManagementCenter.Model.QH_SIFBail model)
        {
            try
            {
                qH_SIFBailDAL.Add(model);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6640";
                string errMsg = "添加品种_股指期货_保证金失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

       /// <summary>
       /// 更新品种_股指期货_保证金
       /// </summary>
       /// <param name="model">品种_股指期货_保证金实体</param>
        public void Update(ManagementCenter.Model.QH_SIFBail model)
        {
            try
            {
                qH_SIFBailDAL.Update(model);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6641";
                string errMsg = "更新品种_股指期货_保证金失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        /// <summary>
        /// 根据品种ID删除品种_股指期货_保证金
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        public void Delete(int BreedClassID)
        {
            try
            {
                qH_SIFBailDAL.Delete(BreedClassID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6642";
                string errMsg = "根据品种ID删除品种_股指期货_保证金失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_SIFBail GetModel(int BreedClassID)
        {
            return qH_SIFBailDAL.GetModel(BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_SIFBail GetModelByCache(int BreedClassID)
        {
            string CacheKey = "QH_SIFBailModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_SIFBailDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.QH_SIFBail) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_SIFBailDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的品种_股指期货_保证金（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_SIFBail> GetListArray(string strWhere)
        {
            try
            {
                return qH_SIFBailDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6643";
                string errMsg = "根据查询条件获取所有的品种_股指期货_保证金（查询条件可为空失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}