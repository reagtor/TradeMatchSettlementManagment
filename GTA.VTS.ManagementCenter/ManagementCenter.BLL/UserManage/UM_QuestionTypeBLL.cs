using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.Model;
namespace ManagementCenter.BLL
{
	/// <summary>
    /// 描述：问题类型表 业务逻辑类UM_QuestionTypeBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
    /// </summary>
	public class UM_QuestionTypeBLL
	{
        private readonly ManagementCenter.DAL.UM_QuestionTypeDAL dal = new ManagementCenter.DAL.UM_QuestionTypeDAL();
		public UM_QuestionTypeBLL()
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
		public bool Exists(int QuestionID)
		{
			return dal.Exists(QuestionID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(ManagementCenter.Model.UM_QuestionType model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_QuestionType model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int QuestionID)
		{
			
			dal.Delete(QuestionID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_QuestionType GetModel(int QuestionID)
		{
			
			return dal.GetModel(QuestionID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.UM_QuestionType GetModelByCache(int QuestionID)
		{
			
			string CacheKey = "UM_QuestionTypeModel-" + QuestionID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(QuestionID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.UM_QuestionType)objModel;
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

        /// <summary>
        /// 获取实体列表方法
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_QuestionType> GetListArray(string strWhere)
		{
            try
            {
                return dal.GetListArray(strWhere);
            }
            catch (Exception)
            {
                //写日志
                return null;
            }
		}

	    #endregion  成员方法
	}
}

