using System;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;
namespace ManagementCenter.BLL
{
	/// <summary>
    /// 描述：资金明细表 业务逻辑类UM_FundAddInfoBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
    /// </summary>
	public class UM_FundAddInfoBLL
	{
        private readonly ManagementCenter.DAL.UM_FundAddInfoDAL dal = new ManagementCenter.DAL.UM_FundAddInfoDAL();
		public UM_FundAddInfoBLL()
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
		public bool Exists(int AddFundID)
		{
			return dal.Exists(AddFundID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(ManagementCenter.Model.UM_FundAddInfo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_FundAddInfo model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int AddFundID)
		{
			
			dal.Delete(AddFundID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_FundAddInfo GetModel(int AddFundID)
		{
			
			return dal.GetModel(AddFundID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.UM_FundAddInfo GetModelByCache(int AddFundID)
		{
			
			string CacheKey = "UM_FundAddInfoModel-" + AddFundID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(AddFundID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.UM_FundAddInfo)objModel;
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

        #region 追加资金历史分页查询
        /// <summary>
        /// 追加资金历史分页查询
        /// </summary>
        /// <param name="fundAddQueryEntity"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataSet GetPagingFund(ManagementCenter.Model.UserManage.FundAddQueryEntity fundAddQueryEntity, int pageNo, int pageSize,
                                        out int rowCount)
		{
		    try
		    {
		        return  dal.GetPagingFund(fundAddQueryEntity, pageNo, pageSize, out rowCount);
		    }
		    catch (Exception)
		    {
		        rowCount = 0;
		        return null;
		    }
        }
        #endregion

        #endregion  成员方法
    }
}

