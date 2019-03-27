using System;
using System.Collections.Generic;
using System.Data;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using GTA.VTS.Common.CommonObject;
namespace ManagementCenter.BLL
{
	/// <summary>
    /// 描述：起始资金表 业务逻辑类UM_OriginationFundBLL 的摘要说明。
    /// 作者：熊晓凌
    /// 日期：2008-11-20   
    /// </summary>
	public class UM_OriginationFundBLL
	{
        private readonly ManagementCenter.DAL.UM_OriginationFundDAL dal = new ManagementCenter.DAL.UM_OriginationFundDAL();
		public UM_OriginationFundBLL()
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
		public bool Exists(int OriginationFundID)
		{
			return dal.Exists(OriginationFundID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(ManagementCenter.Model.UM_OriginationFund model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(ManagementCenter.Model.UM_OriginationFund model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int OriginationFundID)
		{
			
			dal.Delete(OriginationFundID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ManagementCenter.Model.UM_OriginationFund GetModel(int OriginationFundID)
		{
			
			return dal.GetModel(OriginationFundID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public ManagementCenter.Model.UM_OriginationFund GetModelByCache(int OriginationFundID)
		{
			
			string CacheKey = "UM_OriginationFundModel-" + OriginationFundID;
			object objModel = LTP.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(OriginationFundID);
					if (objModel != null)
					{
						int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
						LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (ManagementCenter.Model.UM_OriginationFund)objModel;
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


        #region
        /// <summary>
        /// 根据用户ID获取初始资金列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.UM_OriginationFund> GetListArrayByUserID(int UserID)
        {
            try
            {
                //ManagementCenter.DAL.UM_DealerAccountDAL DealerAccountDAL = new UM_DealerAccountDAL();
                ////DealerAccountDAL.DeleteByUserID(

                //ManagementCenter.DAL.UM_AccountTypeDAL AccountTypeDAL = new UM_AccountTypeDAL();
                //List<UM_AccountType> L_UM_AccountType =
                //    AccountTypeDAL.GetListArray(string.Format("AccountAttributionType={0}",
                //                                              (int) Types.AccountAttributionType.BankAccount));

                //if (L_UM_AccountType != null && L_UM_AccountType.Count == 1)
                //{
                //    List<UM_DealerAccount> L_UM_DealerAccount =
                //        DealerAccountDAL.GetListArray(string.Format("AccountTypeID={0} AND UserID={1}",
                //                                                    L_UM_AccountType[0].AccountTypeID, UserID));
                //    if (L_UM_DealerAccount != null && L_UM_DealerAccount.Count == 1)
                //    {
                //        List<ManagementCenter.Model.UM_OriginationFund> L_UM_OriginationFund =
                //            dal.GetListArray(string.Format("DealerAccoutID='{0}'", L_UM_DealerAccount[0].DealerAccoutID));
                //        return L_UM_OriginationFund;
                //    }
                //}
                //return null;
                return dal.GetListByUserID(UserID);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
  
        }

	    #endregion


        #endregion  成员方法
    }
}

