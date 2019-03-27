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
    /// 柜台管理业务逻辑类CT_CounterBLL。错误编码3100-3120
    /// 作者：熊晓凌
    /// 日期：2008-12-20
    /// </summary>
    public class CT_CounterBLL
    {
        private readonly ManagementCenter.DAL.CT_CounterDAL dal = new ManagementCenter.DAL.CT_CounterDAL();

        public CT_CounterBLL()
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
        public bool Exists(int CouterID)
        {
            return dal.Exists(CouterID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.CT_Counter model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.CT_Counter model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int CouterID)
        {
            dal.Delete(CouterID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CT_Counter GetModel(int CouterID)
        {
            return dal.GetModel(CouterID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CT_Counter GetModelByCache(int CouterID)
        {
            string CacheKey = "CT_CounterModel-" + CouterID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(CouterID);
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
            return (ManagementCenter.Model.CT_Counter) objModel;
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
        /// 获取所有的柜台列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CT_Counter> GetListArray(string strWhere)
        {
            return dal.GetListArray(strWhere);
        }

        /// <summary>
        /// 柜台分页查询失败
        /// </summary>
        /// <param name="counterQueryEntity">查询实体</param>
        /// <param name="pageNo">查询页号</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rowCount">总记录数</param>
        /// <returns></returns>
        public DataSet GetPagingCounter(ManagementCenter.Model.CT_Counter counterQueryEntity, int pageNo, int pageSize,
                                        out int rowCount)
        {
            try
            {
                return dal.GetPagingCounter(counterQueryEntity, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = 0;
                string errCode = "GL-3100";
                string errMsg = "柜台分页查询失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 检测清算柜台连接
        /// </summary>
        public void CenterTestConnection()
        {
            try
            {
                List<ManagementCenter.Model.CT_Counter> l_Counter = dal.GetListArray(string.Empty);
                foreach (CT_Counter CT in l_Counter)
                {
                    bool flag = false;
                    try
                    {
                        flag = ServiceIn.AccountManageServiceProxy.GetInstance().TestConnection(CT.IP, (int)CT.AccountServicePort,
                                                                                                CT.AccountServiceName);
                    }
                    catch (VTException ex)
                    {
                        //写异常日志
                        flag = false;
                        GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                    }
                    if (CT.State == null)
                    {
                        if (flag)
                        {
                            CT.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnSuccess;
                        }
                        else
                        {
                            CT.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat;
                        }
                        dal.Update(CT);
                    }
                    else if (CT.State == (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat)
                    {
                        if (flag)
                        {
                            CT.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnSuccess;
                            dal.Update(CT);
                        }
                    }
                    else
                    {
                        if (!flag)
                        {
                            CT.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat;
                            dal.Update(CT);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-3101";
                string errMsg = "检测清算柜台连接失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }


        /// <summary>
        /// 检测清算柜台连接
        /// </summary>
        public bool TestCenterConnection()
        {
            try
            {
                List<ManagementCenter.Model.CT_Counter> l_Counter = dal.GetListArray(string.Empty);
                foreach (CT_Counter CT in l_Counter)
                {
                    try
                    {
                        ServiceIn.AccountManageServiceProxy.GetInstance().TestConnection(CT.IP, (int)CT.AccountServicePort,
                                                                                                CT.AccountServiceName);
                    }
                    catch (VTException ee)
                    {
                        GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ee.Message, ee);
                        return false;
                    }
                   
                }
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-3101";
                string errMsg = "检测清算柜台连接失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion  成员方法
    }
}