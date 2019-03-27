using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：交易所类型 业务逻辑类CM_BourseTypeBLL 的摘要说明。 错误编码范围:4700-4719
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_BourseTypeBLL
    {
        private readonly CM_BourseTypeDAL cM_BourseTypeDAL =
            new CM_BourseTypeDAL();

        #region  成员方法

        #region 获取交易所类型的最大ID
        /// <summary>
        /// 获取交易所类型的最大ID
        /// </summary>
        public int GetCMBourseTypeMaxId()
        {
            try
            {
                return cM_BourseTypeDAL.GetMaxId();

            }
            catch (Exception ex)
            {
                string errCode = "GL-4709";
                string errMsg = "获取交易所类型的最大ID失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }
        #endregion

        #region 是否存在该记录
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BourseTypeID)
        {
            return cM_BourseTypeDAL.Exists(BourseTypeID);
        }
        #endregion

        #region 添加交易所类型和交易时间
        /// <summary>
        /// 添加交易所类型和交易时间
        /// </summary>
        /// <param name="cmBourseType">交易所类型实体</param>
        /// <param name="cmTradeTimeList">交易时间实体集合</param>
        /// <returns></returns>
        public int AddCMBourseTypeAndTradeTime(CM_BourseType cmBourseType, List<CM_TradeTime> cmTradeTimeList)
        {
            var cmBourseTypeDal = new CM_BourseTypeDAL();
            var cmTradeTimeDal = new CM_TradeTimeDAL();
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction tran = Conn.BeginTransaction();
            //交易所类型ID
            int BourseTypeID = AppGlobalVariable.INIT_INT;
            //交易时间ID
            int TradeTimeID = AppGlobalVariable.INIT_INT;
            try
            {
                if (cmBourseType != null && cmTradeTimeList.Count > 0)
                {
                    BourseTypeID = cmBourseTypeDal.Add(cmBourseType, tran, db);
                    if (BourseTypeID == AppGlobalVariable.INIT_INT)
                    {
                        tran.Rollback();
                    }
                    foreach (CM_TradeTime _cmTradeTime in cmTradeTimeList)
                    {
                        _cmTradeTime.BourseTypeID = BourseTypeID;
                        TradeTimeID = cmTradeTimeDal.Add(_cmTradeTime, tran, db);
                        if (TradeTimeID == AppGlobalVariable.INIT_INT)
                        {
                            tran.Rollback();
                            break;
                        }
                    }
                }
                //根据交易所类型ID获取并查找最早的交易开始时间和最晚的交易结束时间
                List<CM_TradeTime> cmTTimeList =
                    cmTradeTimeDal.GetListArray(string.Format("BourseTypeID={0}", BourseTypeID), tran, db);
                if (cmTTimeList.Count > 0)
                {
                    DateTime _tempStartTime = AppGlobalVariable.INIT_DATETIME;
                    DateTime _tempEndTime = AppGlobalVariable.INIT_DATETIME;
                    bool isStartTime = true;//标志开始值是否改变
                    bool isEndTime = true;//标志结束值是否改变
                    for (int i = 0; i < cmTTimeList.Count; i++)
                    {
                        if (isStartTime)
                        {
                            _tempStartTime = (DateTime)cmTTimeList[0].StartTime;
                        }
                        if (isEndTime)
                        {
                            _tempEndTime = (DateTime)cmTTimeList[0].EndTime;
                        }
                        if (_tempStartTime > (DateTime)cmTTimeList[i].StartTime)
                        {
                            _tempStartTime = (DateTime)cmTTimeList[i].StartTime;
                            isStartTime = false;
                        }
                        if (_tempEndTime < (DateTime)cmTTimeList[i].EndTime)
                        {
                            _tempEndTime = (DateTime)cmTTimeList[i].EndTime;
                            isEndTime = false;
                        }
                    }
                    bool result = cmBourseTypeDal.Update(BourseTypeID, _tempStartTime, _tempEndTime, tran, db);
                    if (!result)
                    {
                        tran.Rollback();
                    }
                    tran.Commit();
                }
                return BourseTypeID;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                string errCode = "GL-4708";
                string errMsg = "添加交易所类型和交易时间失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        #endregion

        #region 更新交易所类型的撮合接收委托开始时间和撮合接收委托结束时间（根据交易开始和结束时间更新）
        /// <summary>
        /// 更新交易所类型的撮合接收委托开始时间和撮合接收委托结束时间（根据交易开始和结束时间更新）
        /// </summary>
        /// <param name="bourseTypeID">交易所类型ID</param>
        /// <param name="receivingConsignStartTime">撮合接收委托开始时间</param>
        /// <param name="receivingConsignEndTime">撮合接收委托结束时间</param>
        /// <returns></returns>
        public bool UpdateBourseTypeConsignTime(int bourseTypeID, DateTime receivingConsignStartTime, DateTime receivingConsignEndTime)
        {
            try
            {
                var cmBourseTypeDal = new CM_BourseTypeDAL();
                cmBourseTypeDal.Update(bourseTypeID, receivingConsignStartTime, receivingConsignEndTime);
                return true;
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return false;
            }
        }
        #endregion

        #region 得到一个对象实体，从缓存中。
        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public CM_BourseType GetModelByCache(int BourseTypeID)
        {
            string CacheKey = "CM_BourseTypeModel-" + BourseTypeID;
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_BourseTypeDAL.GetModel(BourseTypeID);
                    if (objModel != null)
                    {
                        int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                                           TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return (CM_BourseType)objModel;
        }
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_BourseTypeDAL.GetList(strWhere);
        }
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region 根据查询条件获取所有的交易所类型（查询条件可为空）

        /// <summary>
        /// 根据查询条件获取所有的交易所类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">条件查询</param>
        /// <returns></returns>
        public List<CM_BourseType> GetListArray(string strWhere)
        {
            try
            {
                return cM_BourseTypeDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4705";
                string errMsg = "根据查询条件获取所有的交易所类型（查询条件可为空）失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取所有交易所类型

        /// <summary>
        /// 获取所有交易所类型
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMBourseType(string BourseTypeName, int pageNo, int pageSize,
                                          out int rowCount)
        {
            try
            {
                var cMBourseTypeDAL = new CM_BourseTypeDAL();
                return cMBourseTypeDAL.GetAllCMBourseType(BourseTypeName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-4703";
                string errMsg = "获取所有交易所类型失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取交易所类型名称

        /// <summary>
        /// 获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetBourseTypeName()
        {
            try
            {
                var cMBourseTypeDAL = new CM_BourseTypeDAL();
                return cMBourseTypeDAL.GetBourseTypeName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4704";
                string errMsg = "获取交易所类型名称失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 判断交易所名称是否已经存在

        /// <summary>
        /// 判断交易所名称是否已经存在
        /// </summary>
        /// <param name="BourseTypeName">交易所名称</param>
        /// <returns></returns>
        public bool IsExistBourseTypeName(string BourseTypeName)
        {
            try
            {
                var cMBourseTypeDAL = new CM_BourseTypeDAL();
                string strWhere = string.Format("BourseTypeName='{0}'", BourseTypeName);
                DataSet _ds = cMBourseTypeDAL.GetList(strWhere);
                if (_ds != null)
                {
                    if (_ds.Tables[0].Rows.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4706";
                string errMsg = "判断交易所名称是否已经存在失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 删除交易所类型及相关联的表

        /// <summary>
        /// 删除交易所类型及相关联的表
        /// </summary>
        /// <param name="BourseTypeID">交易所类型ID</param>
        public bool DeleteCMBourseTypeAbout(int BourseTypeID)
        {
            CM_NotTradeDateDAL cM_NotTradeDateDAL = new CM_NotTradeDateDAL();
            CM_TradeTimeDAL cM_TradeTimeDAL = new CM_TradeTimeDAL();
            CM_BreedClassDAL cM_BreedClassDAL = new CM_BreedClassDAL();
            RC_MatchMachineDAL rC_MatchMachineDAL = new RC_MatchMachineDAL();
            RC_TradeCommodityAssignDAL rC_TradeCommodityAssignDAL = new RC_TradeCommodityAssignDAL();

            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                int NewBourseTypeID = 5;//未分配交易所的ID=5，固定值
                //获得撮合机数据列表
                List<RC_MatchMachine> rCMatchMachineList =
                    rC_MatchMachineDAL.GetListArray(string.Format("BourseTypeID={0}", BourseTypeID), Tran, db);
                if (rCMatchMachineList.Count > 0)
                {
                    foreach (RC_MatchMachine rCMatchMode in rCMatchMachineList)
                    {
                        // List<RC_TradeCommodityAssign> rCTradeCommodityAssignList =
                        //rC_TradeCommodityAssignDAL.GetListArray(string.Format("MatchMachineID={0}", rCMatchMode.MatchMachineID), Tran, db);
                        //删除撮合机代码分配表中同一个撮合机ID的代码
                        if (!rC_TradeCommodityAssignDAL.DeleteRCTradeCommodityAByMachineID(rCMatchMode.MatchMachineID))
                        {
                            Tran.Rollback();
                            return false;
                        }

                    }
                    //根据交易所类型ID，删除撮合机
                    if (!rC_MatchMachineDAL.DeleteByBourseTypeID(BourseTypeID, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }

                }
                //根据交易所类型删除交易日
                if (!cM_TradeTimeDAL.DeleteCMTradeTimeByBourseTypeID(BourseTypeID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                //根据交易所类型删除非交易日
                if (!cM_NotTradeDateDAL.DeleteByBourseTypeID(BourseTypeID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }

                //根据交易所类型ID，更新交易商品品种中的交易所类型ID
                if (!cM_BreedClassDAL.UpdateBourseTypeID(BourseTypeID, NewBourseTypeID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!cM_BourseTypeDAL.Delete(BourseTypeID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-4701";
                string errMsg = "删除交易所类型失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #endregion

        #region 根据交易所类型ID获取交易所类型实体

        /// <summary>
        /// 根据交易所类型ID获取交易所类型实体
        /// </summary>
        /// <param name="BourseTypeID"></param>
        /// <returns></returns>
        public CM_BourseType GetModel(int BourseTypeID)
        {
            try
            {
                return cM_BourseTypeDAL.GetModel(BourseTypeID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4707";
                string errMsg = "删除交易所类型失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region  更新交易所类型

        /// <summary>
        /// 更新交易所类型
        /// </summary>
        /// <param name="model">交易所类型实体</param>
        /// <returns></returns>
        public bool UpdateCMBourseType(CM_BourseType model)
        {
            try
            {
                return cM_BourseTypeDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4702";
                string errMsg = "更新交易所类型失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 添加交易所类型

        /// <summary>
        /// 添加交易所类型
        /// </summary>
        /// <param name="model">交易所类型实体</param>
        /// <returns></returns>
        public int AddCMBourseType(CM_BourseType model)
        {
            try
            {
                return cM_BourseTypeDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4700";
                string errMsg = "添加交易所类型失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        #endregion

        #endregion  成员方法
    }
}