using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：撮合机管理业务逻辑类。
    /// 作者：熊晓凌
    /// 日期：2008-12-15  异常编码范围 2221-2240
    /// </summary>
    public class RC_MatchMachineBLL
    {
        /// <summary>
        /// 撮合机DAL
        /// </summary>
        private readonly ManagementCenter.DAL.RC_MatchMachineDAL dal = new ManagementCenter.DAL.RC_MatchMachineDAL();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RC_MatchMachineBLL()
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
        public bool Exists(int MatchMachineID)
        {
            return dal.Exists(MatchMachineID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.RC_MatchMachine model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.RC_MatchMachine model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int MatchMachineID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                ManagementCenter.DAL.RC_TradeCommodityAssignDAL TradeCommodityAssignDAL =
                    new RC_TradeCommodityAssignDAL();

                TradeCommodityAssignDAL.DeleteByMachineID(MatchMachineID, Tran, db);
                dal.Delete(MatchMachineID, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-2221";
                string errMsg = "删除撮合机失败 ID=" + MatchMachineID.ToString();
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.RC_MatchMachine GetModel(int MatchMachineID)
        {
            return dal.GetModel(MatchMachineID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.RC_MatchMachine GetModelByCache(int MatchMachineID)
        {
            string CacheKey = "RC_MatchMachineModel-" + MatchMachineID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(MatchMachineID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                                                      TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return (ManagementCenter.Model.RC_MatchMachine) objModel;
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
        /// 根据条件获取所有撮合机列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>返回撮合机实体</returns>
        public List<ManagementCenter.Model.RC_MatchMachine> GetListArray(string strWhere)
        {
            return dal.GetListArray(strWhere);
        }

        /// <summary>
        /// 分页查询撮合机
        /// </summary>
        /// <param name="machineQueryEntity">查询实体</param>
        /// <param name="pageNo">查询页号</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rowCount">总条数</param>
        /// <returns></returns>
        public DataSet GetPagingMachine(ManagementCenter.Model.RC_MatchMachine machineQueryEntity, int pageNo,
                                        int pageSize,
                                        out int rowCount)
        {
            try
            {
                return dal.GetPagingMachine(machineQueryEntity, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = 0;
                string errCode = "GL-2222";
                string errMsg = "分页查询撮合机失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #region  根据撮合中心_撮合机表中的交易所类型ID获取交易所类型名称

        /// <summary>
        /// 根据撮合中心_撮合机表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetRCMatchMachineBourseTypeName()
        {
            try
            {
                return dal.GetRCMatchMachineBourseTypeName();
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        #endregion

        #region  根据撮合中心_撮合机表中的撮合中心ID获取撮合中心名称

        /// <summary>
        /// 根据撮合中心_撮合机表中的撮合中心ID获取撮合中心名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetRCMatchMachineMatchCenterName()
        {
            try
            {
                return dal.GetRCMatchMachineMatchCenterName();
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}