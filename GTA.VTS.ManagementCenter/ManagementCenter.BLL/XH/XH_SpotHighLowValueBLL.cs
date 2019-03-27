using System;
using System.Collections.Generic;
using System.Data;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述:现货_品种_涨跌幅 业务逻辑类XH_SpotHighLow 的摘要说明。
    /// 错误编码范围:5240-5259
    ///作者：刘书伟
    ///日期:2008-12-27
    /// </summary>
    public class XH_SpotHighLowValueBLL
    {
        private readonly ManagementCenter.DAL.XH_SpotHighLowValueDAL xH_SpotHighLowValueDAL =
            new XH_SpotHighLowValueDAL();

        public XH_SpotHighLowValueBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_SpotHighLowValueDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int HightLowValueID)
        {
            return xH_SpotHighLowValueDAL.Exists(HightLowValueID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        //public void Add(ManagementCenter.Model.XH_SpotHighLowValue model)
        //{
        //    xH_SpotHighLowValueDAL.Add(model);
        //}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public void Update(ManagementCenter.Model.XH_SpotHighLowValue model)
        //{
        //    xH_SpotHighLowValueDAL.Update(model);
        //}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_SpotHighLowValue GetModel(int HightLowValueID)
        {
            return xH_SpotHighLowValueDAL.GetModel(HightLowValueID);
        }

        #region  根据品种涨跌幅标识得到一个对象实体

        /// <summary>
        /// 根据品种涨跌幅标识得到一个对象实体
        /// </summary>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <returns></returns>
        public ManagementCenter.Model.XH_SpotHighLowValue GetModelByBCHighLowID(int BreedClassHighLowID)
        {
            try
            {
                XH_SpotHighLowValueDAL xHSpotHighLowValueDAL = new XH_SpotHighLowValueDAL();
                return xHSpotHighLowValueDAL.GetModelByBCHighLowID(BreedClassHighLowID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5240";
                string errMsg = "根据品种涨跌幅标识得到一个对象实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_SpotHighLowValue GetModelByCache(int HightLowValueID)
        {
            string CacheKey = "XH_SpotHighLowValueModel-" + HightLowValueID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_SpotHighLowValueDAL.GetModel(HightLowValueID);
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
            return (ManagementCenter.Model.XH_SpotHighLowValue) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_SpotHighLowValueDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region  根据查询条件获取所有的现货_品种_涨跌幅（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的现货_品种_涨跌幅（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_SpotHighLowValue> GetListArray(string strWhere)
        {
            try
            {
                return xH_SpotHighLowValueDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5241";
                string errMsg = "根据查询条件获取所有的现货_品种_涨跌幅失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
                
            }
        }
        #endregion

        #endregion  成员方法
    }
}