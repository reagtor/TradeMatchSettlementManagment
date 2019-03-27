using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：代码分配业务逻辑类。
    /// 作者：熊晓凌
    /// 日期：2008-12-15  异常编码范围 2241-2260
    /// </summary>
    public class RC_TradeCommodityAssignBLL
    {
        /// <summary>
        /// 代码分配DAL
        /// </summary>
        private readonly ManagementCenter.DAL.RC_TradeCommodityAssignDAL dal =
            new ManagementCenter.DAL.RC_TradeCommodityAssignDAL();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RC_TradeCommodityAssignBLL()
        {
        }
        #endregion
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
        public bool Exists(string CommodityCode, int MatchMachineID)
        {
            return dal.Exists(CommodityCode, MatchMachineID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.RC_TradeCommodityAssign model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public bool Update(int MatchMachineID, List<ManagementCenter.Model.RC_TradeCommodityAssign> ladd,
                           List<ManagementCenter.Model.RC_TradeCommodityAssign> ldel)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                foreach (var assign in ldel)
                {
                    dal.Delete(assign.CommodityCode, MatchMachineID, Tran, db);
                }
                foreach (var assign in ladd)
                {
                    assign.MatchMachineID = MatchMachineID;
                    dal.Add(assign, Tran, db);
                }
                Tran.Commit();
                return true;
            }
            catch (Exception)
            {
                Tran.Rollback();
                return false;
                throw;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string CommodityCode, int MatchMachineID)
        {
            //dal.Delete(CommodityCode,MatchMachineID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.RC_TradeCommodityAssign GetModel(string CommodityCode, int MatchMachineID)
        {
            return dal.GetModel(CommodityCode, MatchMachineID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.RC_TradeCommodityAssign GetModelByCache(string CommodityCode, int MatchMachineID)
        {
            string CacheKey = "RC_TradeCommodityAssignModel-" + CommodityCode + MatchMachineID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(CommodityCode, MatchMachineID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                                                      TimeSpan.Zero);
                    }
                }
                catch
                {}
            }
            return (ManagementCenter.Model.RC_TradeCommodityAssign) objModel;
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
        /// 获取商品分配到撮合机的列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.RC_TradeCommodityAssign> GetListArray(string strWhere)
        {
            return dal.GetListArray(strWhere);
        }


        /// <summary>
        /// 根据撮合机得到撮合机撮合的代码
        /// </summary>
        /// <param name="MatchMachineID"></param>
        /// <returns></returns>
        public DataSet GetCodeListByMatchMachineID(int MatchMachineID)
        {
            try
            {
                return dal.GetCodeListByMatchMachineID(MatchMachineID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-2241";
                string errMsg = "根据撮合机得到撮合机撮合的代码失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 得到交易所没有分配的商品代码
        /// </summary>
        /// <param name="BourseTypeID"></param>
        /// <returns></returns>
        public DataSet GetNotAssignCodeByBourseTypeID(int BourseTypeID)
        {
            try
            {
                return dal.GetNotAssignCodeByBourseTypeID(BourseTypeID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-2242";
                string errMsg = "得到交易所没有分配的商品代码失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}