using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：委托指令类型 业务逻辑类QH_ConsignInstructionTypeBLL 的摘要说明。
    /// 作者：刘书伟
    /// 日期:2008-12-13
    /// </summary>
    public class QH_ConsignInstructionTypeBLL
    {
        private readonly ManagementCenter.DAL.QH_ConsignInstructionTypeDAL qH_ConsignInstructionTypeDAL =
            new ManagementCenter.DAL.QH_ConsignInstructionTypeDAL();

        public QH_ConsignInstructionTypeBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_ConsignInstructionTypeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ConsignInstructionTypeID)
        {
            return qH_ConsignInstructionTypeDAL.Exists(ConsignInstructionTypeID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.QH_ConsignInstructionType model)
        {
            qH_ConsignInstructionTypeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.QH_ConsignInstructionType model)
        {
            qH_ConsignInstructionTypeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ConsignInstructionTypeID)
        {
            qH_ConsignInstructionTypeDAL.Delete(ConsignInstructionTypeID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_ConsignInstructionType GetModel(int ConsignInstructionTypeID)
        {
            return qH_ConsignInstructionTypeDAL.GetModel(ConsignInstructionTypeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_ConsignInstructionType GetModelByCache(int ConsignInstructionTypeID)
        {
            string CacheKey = "QH_ConsignInstructionTypeModel-" + ConsignInstructionTypeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_ConsignInstructionTypeDAL.GetModel(ConsignInstructionTypeID);
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
            return (ManagementCenter.Model.QH_ConsignInstructionType) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_ConsignInstructionTypeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的委托指令类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_ConsignInstructionType> GetListArray(string strWhere)
        {
            try
            {
                return qH_ConsignInstructionTypeDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
                //throw;
            }
        }

        #endregion  成员方法
    }
}