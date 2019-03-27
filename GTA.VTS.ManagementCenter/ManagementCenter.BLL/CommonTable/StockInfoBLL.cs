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
    /// 描述:股票（权证）代码表 业务逻辑类StockInfo 的摘要说明。错误编码范围:4320-4323
    /// 作者：熊晓凌     修改：刘书伟
    /// 日期：2008-11-20  2009-12-01
	/// </summary>
	public class StockInfoBLL
	{
		private readonly ManagementCenter.DAL.StockInfoDAL dal=new StockInfoDAL();
		public StockInfoBLL()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string StockCode)
		{
			return dal.Exists(StockCode);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(ManagementCenter.Model.StockInfo model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.StockInfo model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(string StockCode)
		{
			
			dal.Delete(StockCode);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.StockInfo GetModel(string StockCode)
		{
			
			return dal.GetModel(StockCode);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.StockInfo GetModelByCache(string StockCode)
		{
			
			string CacheKey = "StockInfoModel-" + StockCode;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(StockCode);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.StockInfo)objModel;
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
        /// 根据查询条件获取普通代码信息表(查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件（可为空）</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.StockInfo> GetStockInfoList(string strWhere)
        {
            try
            {
                StockInfoDAL stockInfoDAL=new StockInfoDAL();
                return stockInfoDAL.GetStockInfoList(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4320";
                string errMsg = "根据查询条件获取普通代码信息表(查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

		#endregion  成员方法
	}
}

