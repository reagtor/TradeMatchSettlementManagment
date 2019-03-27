using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述: 品种类型权限表 业务逻辑类UM_DealerTradeBreedClassBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
    /// </summary>
    public class UM_DealerTradeBreedClassBLL
    {
        /// <summary>
        /// 品种类型权限DAL
        /// </summary>
        private readonly ManagementCenter.DAL.UM_DealerTradeBreedClassDAL dal =
            new ManagementCenter.DAL.UM_DealerTradeBreedClassDAL();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UM_DealerTradeBreedClassBLL()
        {
        }
        #endregion

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
        public bool Exists(int DealerTradeBreedClassID)
        {
            return dal.Exists(DealerTradeBreedClassID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.UM_DealerTradeBreedClass model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.UM_DealerTradeBreedClass model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int DealerTradeBreedClassID)
        {
            dal.Delete(DealerTradeBreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.UM_DealerTradeBreedClass GetModel(int DealerTradeBreedClassID)
        {
            return dal.GetModel(DealerTradeBreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.UM_DealerTradeBreedClass GetModelByCache(int DealerTradeBreedClassID)
        {
            string CacheKey = "UM_DealerTradeBreedClassModel-" + DealerTradeBreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(DealerTradeBreedClassID);
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
            return (ManagementCenter.Model.UM_DealerTradeBreedClass)objModel;
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
        /// 获取用户的品种交易权限
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetUserBreedClassRight(int UserID)
        {
            return dal.GetUserBreedClassRight(UserID);
        }

        /// <summary>
        /// 根据用户ID获取品种权限列表
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns>返回品种权限列表</returns>
        public List<ManagementCenter.Model.UM_DealerTradeBreedClass> GetBreedClassRightList(int UserID)
        {
            List<ManagementCenter.Model.UM_DealerTradeBreedClass> l =
                dal.GetListArray(string.Format("UserID={0}", UserID));
            return l;
        }

        #endregion  成员方法
    }
}