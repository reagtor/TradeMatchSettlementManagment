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
    /// 描述: 帐号类型 业务逻辑类UM_AccountTypeBLL 的摘要说明。
    /// 作者：熊晓凌  修改:刘书伟
    /// 日期：2008-11-20  修改日期:2009-11-03
    /// </summary>
    public class UM_AccountTypeBLL
    {
        private readonly ManagementCenter.DAL.UM_AccountTypeDAL dal = new ManagementCenter.DAL.UM_AccountTypeDAL();

        public UM_AccountTypeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int AccountTypeID)
        {
            return dal.Exists(AccountTypeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.UM_AccountType model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.UM_AccountType model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int AccountTypeID)
        {
            dal.Delete(AccountTypeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_AccountType GetModel(int AccountTypeID)
        {
            return dal.GetModel(AccountTypeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.UM_AccountType GetModelByCache(int AccountTypeID)
        {
            string CacheKey = "UM_AccountTypeModel-" + AccountTypeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(AccountTypeID);
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
            return (ManagementCenter.Model.UM_AccountType) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
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
        /// 获取所有帐户类型
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_AccountType> GetListArray(string strWhere)
        {
            //return dal.GetListArray(string.Empty);
            try
            {
                return dal.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-";
                string errMsg = "根据查询条件获取账户失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }

        }
        #endregion  成员方法
    }
}