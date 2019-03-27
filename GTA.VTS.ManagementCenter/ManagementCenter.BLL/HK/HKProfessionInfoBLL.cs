using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：港股行业 业务逻辑类HKProfessionInfoBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HKProfessionInfoBLL
    {
        private readonly HKProfessionInfoDAL hKProfessionInfoDAL = new HKProfessionInfoDAL();
        public HKProfessionInfoBLL()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string Nindcd)
        {
            return hKProfessionInfoDAL.Exists(Nindcd);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.HKProfessionInfo model)
        {
            hKProfessionInfoDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.HKProfessionInfo model)
        {
            hKProfessionInfoDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string Nindcd)
        {

            hKProfessionInfoDAL.Delete(Nindcd);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.HKProfessionInfo GetModel(string Nindcd)
        {

            return hKProfessionInfoDAL.GetModel(Nindcd);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.HKProfessionInfo GetModelByCache(string Nindcd)
        {

            string CacheKey = "HKProfessionInfoModel-" + Nindcd;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = hKProfessionInfoDAL.GetModel(Nindcd);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (ManagementCenter.Model.HKProfessionInfo)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return hKProfessionInfoDAL.GetList(strWhere);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<ManagementCenter.Model.HKProfessionInfo> GetModelList(string strWhere)
        {
            DataSet ds = hKProfessionInfoDAL.GetList(strWhere);
            List<ManagementCenter.Model.HKProfessionInfo> modelList = new List<ManagementCenter.Model.HKProfessionInfo>();
            int rowsCount = ds.Tables[0].Rows.Count;
            if (rowsCount > 0)
            {
                ManagementCenter.Model.HKProfessionInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new ManagementCenter.Model.HKProfessionInfo();
                    model.Nindcd = ds.Tables[0].Rows[n]["Nindcd"].ToString();
                    model.Nindnme = ds.Tables[0].Rows[n]["Nindnme"].ToString();
                    model.EnNindnme = ds.Tables[0].Rows[n]["EnNindnme"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的港股行业（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HKProfessionInfo> GetListArray(string strWhere)
        {
            try
            {
                return hKProfessionInfoDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "";
                string errMsg = "";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}
