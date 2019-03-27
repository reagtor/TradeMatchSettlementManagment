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
    /// 描述：管理员_所属表 业务逻辑类UM_ManagerBeloneToGroupBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
	/// </summary>
	public class UM_ManagerBeloneToGroupBLL
	{
        private readonly ManagementCenter.DAL.UM_ManagerBeloneToGroupDAL dal = new ManagementCenter.DAL.UM_ManagerBeloneToGroupDAL();
		public UM_ManagerBeloneToGroupBLL()
		{}
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
		public bool Exists(int UserID)
		{
			return dal.Exists(UserID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void  Add(ManagementCenter.Model.UM_ManagerBeloneToGroup model)
		{
			 dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_ManagerBeloneToGroup model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int UserID)
		{
			
			dal.Delete(UserID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_ManagerBeloneToGroup GetModel(int UserID)
		{
			
			return dal.GetModel(UserID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.UM_ManagerBeloneToGroup GetModelByCache(int UserID)
		{
			
			string CacheKey = "UM_ManagerBeloneToGroupModel-" + UserID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(UserID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.UM_ManagerBeloneToGroup)objModel;
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
        /// 获取管理员所属权限组列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_ManagerBeloneToGroup> GetListArray(string strWhere)
        {
            try
            {
                return dal.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-1121";
                string errMsg = "获取管理员所属权限组列表！";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  成员方法
	}
}

