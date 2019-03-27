using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;
namespace ManagementCenter.BLL
{
	/// <summary>
    ///描述：商品期货_持仓取值类型 业务逻辑类QH_PositionValueTypeBLL 的摘要说明。
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
	public class QH_PositionValueTypeBLL
	{
        private readonly ManagementCenter.DAL.QH_PositionValueTypeDAL qH_PositionValueTypeDAL = new ManagementCenter.DAL.QH_PositionValueTypeDAL();
        public QH_PositionValueTypeBLL()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
            return qH_PositionValueTypeDAL.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int PositionValueTypeID)
		{
            return qH_PositionValueTypeDAL.Exists(PositionValueTypeID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(ManagementCenter.Model.QH_PositionValueType model)
		{
            qH_PositionValueTypeDAL.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.QH_PositionValueType model)
		{
            qH_PositionValueTypeDAL.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int PositionValueTypeID)
		{

            qH_PositionValueTypeDAL.Delete(PositionValueTypeID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.QH_PositionValueType GetModel(int PositionValueTypeID)
		{

            return qH_PositionValueTypeDAL.GetModel(PositionValueTypeID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.QH_PositionValueType GetModelByCache(int PositionValueTypeID)
		{
			
			string CacheKey = "QH_PositionValueTypeModel-" + PositionValueTypeID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
                    objModel = qH_PositionValueTypeDAL.GetModel(PositionValueTypeID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.QH_PositionValueType)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
            return qH_PositionValueTypeDAL.GetList(strWhere);
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
        /// 根据查询条件获取所有的商品期货_持仓取值类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_PositionValueType> GetListArray(string strWhere)
        {
            try
            {
                return qH_PositionValueTypeDAL.GetListArray(strWhere);
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

