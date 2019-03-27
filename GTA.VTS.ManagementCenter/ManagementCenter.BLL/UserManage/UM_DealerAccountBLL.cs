using System;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;
namespace ManagementCenter.BLL
{
	/// <summary>
    /// 描述: 业务逻辑类UM_DealerAccount 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20
	/// </summary>
	public class UM_DealerAccountBLL
	{
        private readonly ManagementCenter.DAL.UM_DealerAccountDAL dal = new ManagementCenter.DAL.UM_DealerAccountDAL();
		public UM_DealerAccountBLL()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string DealerAccoutID)
		{
			return dal.Exists(DealerAccoutID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(ManagementCenter.Model.UM_DealerAccount model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_DealerAccount model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(string DealerAccoutID)
		{
			
			dal.Delete(DealerAccoutID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_DealerAccount GetModel(string DealerAccoutID)
		{
			
			return dal.GetModel(DealerAccoutID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.UM_DealerAccount GetModelByCache(string DealerAccoutID)
		{
			
			string CacheKey = "UM_DealerAccountModel-" + DealerAccoutID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(DealerAccoutID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.UM_DealerAccount)objModel;
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

        public ManagementCenter.Model.UM_DealerAccount GetModelByUserIDAndType(int UserID, int AccountAttributionType)
		{
		    try
		    {
                return dal.GetModelByUserIDAndType(UserID, AccountAttributionType);
		    }
		    catch (Exception)
		    {
		        return null;
		    }
		}

	    #endregion  成员方法
	}
}

