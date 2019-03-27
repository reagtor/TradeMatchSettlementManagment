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
    ///描述：单笔委托量 业务逻辑类QH_SingleRequestQuantityBLL 的摘要说明。错误编码范围:6040-6049
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class QH_SingleRequestQuantityBLL
    {
        private readonly ManagementCenter.DAL.QH_SingleRequestQuantityDAL qH_SingleRequestQuantityDAL =
            new ManagementCenter.DAL.QH_SingleRequestQuantityDAL();

        public QH_SingleRequestQuantityBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_SingleRequestQuantityDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int SingleRequestQuantityID)
        {
            return qH_SingleRequestQuantityDAL.Exists(SingleRequestQuantityID);
        }

        ///// <summary>
        ///// 增加一条数据
        ///// </summary>
        //public int Add(ManagementCenter.Model.QH_SingleRequestQuantity model)
        //{
        //    return qH_SingleRequestQuantityDAL.Add(model);
        //}

        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        //public void Update(ManagementCenter.Model.QH_SingleRequestQuantity model)
        //{
        //    qH_SingleRequestQuantityDAL.Update(model);
        //}

        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public void Delete(int SingleRequestQuantityID)
        //{
        //    qH_SingleRequestQuantityDAL.Delete(SingleRequestQuantityID);
        //}

        #region 根据单笔委托量标识获取单笔委托量实体
        /// <summary>
        /// 根据单笔委托量标识获取单笔委托量实体
       /// </summary>
        /// <param name="SingleRequestQuantityID">单笔委托量标识</param>
       /// <returns></returns>
        public ManagementCenter.Model.QH_SingleRequestQuantity GetQHSingleRequestQuantityModel(int SingleRequestQuantityID)
        {
            try
            {
                QH_SingleRequestQuantityDAL qHSingleRequestQuantityDAL=new QH_SingleRequestQuantityDAL();
                return qH_SingleRequestQuantityDAL.GetModel(SingleRequestQuantityID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6040";
                string errMsg = "根据单笔委托量标识获取单笔委托量实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

         #region 根据交易规则委托量ID获取单笔委托量
        /// <summary>
        /// 根据交易规则委托量ID获取单笔委托量
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.QH_SingleRequestQuantity GetQHSingleRequestQuantityModelByConsignQuantumID(int ConsignQuantumID)
        {
            try
            {
                QH_SingleRequestQuantityDAL qHSingleRequestQuantityDAL = new QH_SingleRequestQuantityDAL();
                return qH_SingleRequestQuantityDAL.GetQHSingleRequestQuantityModelByConsignQuantumID(ConsignQuantumID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6041";
                string errMsg = "根据交易规则委托量ID获取单笔委托量失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
         #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_SingleRequestQuantity GetModelByCache(int SingleRequestQuantityID)
        {
            string CacheKey = "QH_SingleRequestQuantityModel-" + SingleRequestQuantityID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_SingleRequestQuantityDAL.GetModel(SingleRequestQuantityID);
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
            return (ManagementCenter.Model.QH_SingleRequestQuantity) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_SingleRequestQuantityDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的单笔委托量（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_SingleRequestQuantity> GetListArray(string strWhere)
        {
            try
            {
                return qH_SingleRequestQuantityDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6042";
                string errMsg = "根据查询条件获取所有的单笔委托量（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}