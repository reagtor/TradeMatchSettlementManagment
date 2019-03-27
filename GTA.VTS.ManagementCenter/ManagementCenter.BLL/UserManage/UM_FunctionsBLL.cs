using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;
namespace ManagementCenter.BLL
{
	/// <summary>
    /// 描述：权限功能表 业务逻辑类UM_FunctionsBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
    /// </summary>
	public class UM_FunctionsBLL
	{
        private readonly ManagementCenter.DAL.UM_FunctionsDAL dal = new ManagementCenter.DAL.UM_FunctionsDAL();
		public UM_FunctionsBLL()
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
		public bool Exists(int FunctionID)
		{
			return dal.Exists(FunctionID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(ManagementCenter.Model.UM_Functions model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_Functions model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int FunctionID)
		{
			
			dal.Delete(FunctionID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_Functions GetModel(int FunctionID)
		{
			
			return dal.GetModel(FunctionID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.UM_Functions GetModelByCache(int FunctionID)
		{
			
			string CacheKey = "UM_FunctionsModel-" + FunctionID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(FunctionID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.UM_Functions)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}

        /// <summary>
        /// 根据查询条件返回功能列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>返回功能列表</returns>
        public List<ManagementCenter.Model.UM_Functions> GetListArray(string strWhere)
        {
            try
            {
                return dal.GetListArray(strWhere);
            }
            catch (Exception)
            {
                return null;
                throw;
            }     
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

		#endregion  成员方法
	}
}

