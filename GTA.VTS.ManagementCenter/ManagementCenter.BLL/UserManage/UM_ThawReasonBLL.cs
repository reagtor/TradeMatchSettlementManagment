using System;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;
namespace ManagementCenter.BLL
{
	/// <summary>
    /// 描述：解冻原因表 业务逻辑类UM_ThawReasonBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
    /// </summary>
	public class UM_ThawReasonBLL
	{
        private readonly ManagementCenter.DAL.UM_ThawReasonDAL dal = new ManagementCenter.DAL.UM_ThawReasonDAL();
		public UM_ThawReasonBLL()
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
		public bool Exists(int ThawReasonID)
		{
			return dal.Exists(ThawReasonID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(ManagementCenter.Model.UM_ThawReason model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_ThawReason model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ThawReasonID)
		{
			
			dal.Delete(ThawReasonID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_ThawReason GetModel(int ThawReasonID)
		{
			
			return dal.GetModel(ThawReasonID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.UM_ThawReason GetModelByCache(int ThawReasonID)
		{
			
			string CacheKey = "UM_ThawReasonModel-" + ThawReasonID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ThawReasonID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.UM_ThawReason)objModel;
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

		#endregion  成员方法
	}
}

