using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;
namespace ManagementCenter.BLL
{
	/// <summary>
    ///描述：持仓和保证金控制类型 业务逻辑类QH_PositionBailTypeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
	public class QH_PositionBailTypeBLL
	{
        private readonly ManagementCenter.DAL.QH_PositionBailTypeDAL qH_PositionBailTypeDAL = new ManagementCenter.DAL.QH_PositionBailTypeDAL();
        public QH_PositionBailTypeBLL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
            return qH_PositionBailTypeDAL.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int PositionBailTypeID)
		{
            return qH_PositionBailTypeDAL.Exists(PositionBailTypeID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(ManagementCenter.Model.QH_PositionBailType model)
		{
            qH_PositionBailTypeDAL.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.QH_PositionBailType model)
		{
            qH_PositionBailTypeDAL.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int PositionBailTypeID)
		{

            qH_PositionBailTypeDAL.Delete(PositionBailTypeID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.QH_PositionBailType GetModel(int PositionBailTypeID)
		{

            return qH_PositionBailTypeDAL.GetModel(PositionBailTypeID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.QH_PositionBailType GetModelByCache(int PositionBailTypeID)
		{
			
			string CacheKey = "QH_PositionBailTypeModel-" + PositionBailTypeID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
                    objModel = qH_PositionBailTypeDAL.GetModel(PositionBailTypeID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.QH_PositionBailType)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
            return qH_PositionBailTypeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的持仓和保证金控制类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_PositionBailType> GetListArray(string strWhere)
        {
            try
            {
                return qH_PositionBailTypeDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

		#endregion  成员方法
	}
}

