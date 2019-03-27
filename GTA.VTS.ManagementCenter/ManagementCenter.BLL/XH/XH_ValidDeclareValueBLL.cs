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
    ///描述：有效申报取值 业务逻辑类XH_ValidDeclareValue 的摘要说明。错误编码范围:5280-5299
    ///作者：刘书伟
    ///日期:2008-11-27
    /// </summary>
    public class XH_ValidDeclareValueBLL
    {
        private readonly ManagementCenter.DAL.XH_ValidDeclareValueDAL xH_ValidDeclareValueDAL =
            new ManagementCenter.DAL.XH_ValidDeclareValueDAL();

        public XH_ValidDeclareValueBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_ValidDeclareValueDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ValidDeclareValueID)
        {
            return xH_ValidDeclareValueDAL.Exists(ValidDeclareValueID);
        }

        #region 暂没用到的公共方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        //public void Add(ManagementCenter.Model.XH_ValidDeclareValue model)
        //{
        //    xH_ValidDeclareValueDAL.Add(model);
        //}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public void Update(ManagementCenter.Model.XH_ValidDeclareValue model)
        //{
        //    xH_ValidDeclareValueDAL.Update(model);
        //}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        //public void Delete(int ValidDeclareValueID)
        //{
        //    xH_ValidDeclareValueDAL.Delete(ValidDeclareValueID);
        //}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_ValidDeclareValue GetModel(int ValidDeclareValueID)
        {
            return xH_ValidDeclareValueDAL.GetModel(ValidDeclareValueID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_ValidDeclareValue GetModelByCache(int ValidDeclareValueID)
        {
            string CacheKey = "XH_ValidDeclareValueModel-" + ValidDeclareValueID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_ValidDeclareValueDAL.GetModel(ValidDeclareValueID);
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
            return (ManagementCenter.Model.XH_ValidDeclareValue) objModel;
        }
      

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_ValidDeclareValueDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region 根据查询条件获取所有的有效申报取值（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的有效申报取值（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_ValidDeclareValue> GetListArray(string strWhere)
        {
            try
            {
                return xH_ValidDeclareValueDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5280";
                string errMsg = "根据查询条件获取所有的有效申报取值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #endregion  成员方法
    }
}