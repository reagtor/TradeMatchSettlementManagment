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
    ///描述：交易所类型_交易时间 业务逻辑类CM_TradeTimeBLL 的摘要说明。错误编码范围:4740-4759
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_TradeTimeBLL
    {
        /// <summary>
        /// 交易所类型_交易时间 DAL
        /// </summary>
        private readonly ManagementCenter.DAL.CM_TradeTimeDAL cM_TradeTimeDAL =
            new ManagementCenter.DAL.CM_TradeTimeDAL();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CM_TradeTimeBLL()
        {
        }
        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return cM_TradeTimeDAL.GetMaxId();
        }

        /// <summary>
        /// 根据交易时间ID判断是否已存在交易时间记录
        /// </summary>
        /// <param name="TradeTimeID">交易时间ID</param>
        /// <returns></returns>
        public bool ExistsCMTradeTime(int TradeTimeID)
        {
            try
            {
                return cM_TradeTimeDAL.Exists(TradeTimeID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-4747";
                string errMsg = " 根据交易时间ID判断是否已存在交易时间记录失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        ///比较交易时间 
        /// </summary>
        /// <param name="cMTradeTimeList">交易时间集合</param>
        /// <returns></returns>
        public CM_TradeTime CompareTradeTime(List<CM_TradeTime> cMTradeTimeList)
        {
            try
            {
                CM_TradeTime cM_TradeTime = new CM_TradeTime();
                DateTime _tempStartTime = AppGlobalVariable.INIT_DATETIME;
                DateTime _tempEndTime = AppGlobalVariable.INIT_DATETIME;
                DateTime _changeStartTime = AppGlobalVariable.INIT_DATETIME;//某条记录的开始时间
                DateTime _changeEndTime = AppGlobalVariable.INIT_DATETIME;//某条记录的结束时间
                string timeFormat = string.Empty;//存时间格式
                bool isStartTime = true; //标志开始值是否改变
                bool isEndTime = true; //标志结束值是否改变
                for (int i = 0; i < cMTradeTimeList.Count; i++)
                {
                    timeFormat = ((DateTime)cMTradeTimeList[i].StartTime).ToString("HH:mm");
                    _changeStartTime = Convert.ToDateTime(timeFormat);
                    timeFormat = ((DateTime)cMTradeTimeList[i].EndTime).ToString("HH:mm");
                    _changeEndTime = Convert.ToDateTime(timeFormat);
                    if (isStartTime)
                    {
                        timeFormat = ((DateTime)cMTradeTimeList[0].StartTime).ToString("HH:mm");
                        _tempStartTime = Convert.ToDateTime(timeFormat); //转换成当前日期的时间
                        cM_TradeTime.StartTime = _tempStartTime;
                    }
                    if (isEndTime)
                    {
                        timeFormat = ((DateTime)cMTradeTimeList[0].EndTime).ToString("HH:mm");
                        _tempEndTime = Convert.ToDateTime(timeFormat); //转换成当前日期的时间
                        cM_TradeTime.EndTime = _tempEndTime;
                    }
                    if (_tempStartTime > _changeStartTime)
                    {
                        _tempStartTime = _changeStartTime;
                        cM_TradeTime.StartTime = _tempStartTime;
                        isStartTime = false;
                    }
                    if (_tempEndTime < _changeEndTime)
                    {
                        _tempEndTime = _changeEndTime;
                        cM_TradeTime.EndTime = _tempEndTime;
                        isEndTime = false;
                    }
                }
                return cM_TradeTime;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4749";
                string errMsg = "比较交易时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #region 添加交易时间

        /// <summary>
        /// 添加交易时间
        /// </summary>
        /// <param name="model">交易所类型_交易时间实体</param>
        /// <returns></returns>
        public int AddCMTradeTime(ManagementCenter.Model.CM_TradeTime model)
        {
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction tran = Conn.BeginTransaction();
            try
            {
                CM_TradeTimeDAL cM_TradeTimeDAL = new CM_TradeTimeDAL();
                CM_BourseTypeDAL cM_BourseTypeDAL = new CM_BourseTypeDAL();
                //int tradeTimeID = (int)model.TradeTimeID;
                int addTTimeResult = AppGlobalVariable.INIT_INT;
                //交易所类型ID
                int bourseTypeID = AppGlobalVariable.INIT_INT;
                bourseTypeID = (int)model.BourseTypeID;
                addTTimeResult = cM_TradeTimeDAL.Add(model, tran, db);
                if (addTTimeResult == AppGlobalVariable.INIT_INT)
                {
                    tran.Rollback();
                    return AppGlobalVariable.INIT_INT;
                }
                if (bourseTypeID != AppGlobalVariable.INIT_INT)
                {
                    //根据交易所类型ID获取并查找最早的交易开始时间和最晚的交易结束时间
                    List<CM_TradeTime> cmTTimeList =
                        cM_TradeTimeDAL.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID), tran, db);
                    if (cmTTimeList.Count > 0)
                    {
                        CM_TradeTime cmTradeTime = CompareTradeTime(cmTTimeList);
                        if (cmTradeTime != null)
                        {
                            bool result = cM_BourseTypeDAL.Update(bourseTypeID, (DateTime)cmTradeTime.StartTime,
                                                                  (DateTime)cmTradeTime.EndTime, tran,
                                                                  db);
                            if (!result)
                            {
                                tran.Rollback();
                                return AppGlobalVariable.INIT_INT;
                            }
                        }
                        tran.Commit();
                    }
                }
                return addTTimeResult;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                string errCode = "GL-4740";
                string errMsg = "添加交易时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
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

        #region 添加交易时间 重载
        /// <summary>
        /// 添加交易时间
        /// </summary>
        /// <param name="model">交易所类型_交易时间实体</param>
        /// <param name="msg">返回错误结果提示信息</param>
        /// <returns></returns>
        //public int AddCMTradeTime(ManagementCenter.Model.CM_TradeTime model, ref string msg)
        //{
        //    try
        //    {
        //        string m_msg = string.Empty;
        //        if (model.StartTime > model.EndTime)
        //        {
        //            m_msg += "交易开始时间不能大于交易结束时间!" + "\t\n";
        //        }
        //        if (model.EndTime < model.StartTime)
        //        {
        //            m_msg += "交易结束时间不能小于交易开始时间!" + "\t\n";
        //        }
        //        if (model.StartTime == model.EndTime)
        //        {
        //            m_msg += "交易开始时间和交易结束时间不能相等!" + "\t\n";
        //        }
        //        msg = m_msg;
        //        if (!string.IsNullOrEmpty(msg))
        //        {
        //            return AppGlobalVariable.INIT_INT;
        //        }
        //        return cM_TradeTimeDAL.Add(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GL-4740";
        //        string errMsg = "添加交易时间失败!";
        //        VTException exception = new VTException(errCode, errMsg, ex);
        //        LogHelper.WriteError(exception.ToString(), exception.InnerException);
        //        return AppGlobalVariable.INIT_INT;
        //    }
        //}
        #endregion

        #region 更新交易时间

        /// <summary>
        /// 更新交易时间
        /// </summary>
        /// <param name="model">交易所类型_交易时间实体</param>
        /// <returns></returns>
        public bool UpdateCMTradeTime(ManagementCenter.Model.CM_TradeTime model)
        {
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction tran = Conn.BeginTransaction();
            try
            {
                // return cM_TradeTimeDAL.Update(model);
                CM_TradeTimeDAL cM_TradeTimeDAL = new CM_TradeTimeDAL();
                CM_BourseTypeDAL cM_BourseTypeDAL = new CM_BourseTypeDAL();
                //交易所类型ID
                int bourseTypeID = AppGlobalVariable.INIT_INT;
                bourseTypeID = (int)model.BourseTypeID;
                bool updateTTimeResult = cM_TradeTimeDAL.Update(model, tran, db);
                if (!updateTTimeResult)
                {
                    tran.Rollback();
                    return false;
                }
                if (bourseTypeID != AppGlobalVariable.INIT_INT)
                {
                    //根据交易所类型ID获取并查找最早的交易开始时间和最晚的交易结束时间
                    List<CM_TradeTime> cmTTimeList =
                        cM_TradeTimeDAL.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID), tran, db);
                    if (cmTTimeList.Count > 0)
                    {
                       CM_TradeTime cmTradeTime = CompareTradeTime(cmTTimeList);
                       if (cmTradeTime != null)
                       {
                           bool result = cM_BourseTypeDAL.Update(bourseTypeID, (DateTime) cmTradeTime.StartTime,
                                                                 (DateTime) cmTradeTime.EndTime, tran,
                                                                 db);
                           if (!result)
                           {
                               tran.Rollback();
                               return false;
                           }
                       }
                        tran.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                string errCode = "GL-4742";
                string errMsg = "更新交易时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
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

        #region 根据交易时间ID，删除交易时间

        /// <summary>
        /// 根据交易时间ID，删除交易时间
        /// </summary>
        /// <param name="TradeTimeID">交易时间ID</param>
        /// <returns></returns>
        public bool DeleteCMTradeTime(int TradeTimeID)
        {
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction tran = Conn.BeginTransaction();
            try
            {
                //return cM_TradeTimeDAL.Delete(TradeTimeID);
                CM_TradeTimeDAL cM_TradeTimeDAL = new CM_TradeTimeDAL();
                CM_BourseTypeDAL cM_BourseTypeDAL = new CM_BourseTypeDAL();
                CM_TradeTime cMTradeTime = cM_TradeTimeDAL.GetModel(TradeTimeID);
                if (cMTradeTime != null)
                {
                    //交易所类型ID
                    int bourseTypeID = AppGlobalVariable.INIT_INT;
                    bourseTypeID = (int)cMTradeTime.BourseTypeID;
                    bool deleTTimeResult = cM_TradeTimeDAL.Delete(TradeTimeID);
                    if (!deleTTimeResult)
                    {
                        tran.Rollback();
                        return false;
                    }
                    if (bourseTypeID != AppGlobalVariable.INIT_INT)
                    {
                        //根据交易所类型ID获取并查找最早的交易开始时间和最晚的交易结束时间
                        List<CM_TradeTime> cmTTimeList =
                            cM_TradeTimeDAL.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID), tran, db);
                        if (cmTTimeList.Count > 0)
                        {
                           CM_TradeTime cmTradeTime = CompareTradeTime(cmTTimeList);
                           if (cmTradeTime != null)
                           {
                               bool result = cM_BourseTypeDAL.Update(bourseTypeID, (DateTime) cmTradeTime.StartTime,
                                                                     (DateTime) cmTradeTime.EndTime, tran,
                                                                     db);
                               if (!result)
                               {
                                   tran.Rollback();
                                   return false;
                               }
                           }
                            tran.Commit();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                string errCode = "GL-4741";
                string errMsg = " 根据交易时间ID，删除交易时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
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

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_TradeTime GetModel(int TradeTimeID)
        {
            return cM_TradeTimeDAL.GetModel(TradeTimeID);
        }

        #region 获取所有交易所类型_交易时间

        /// <summary>
        /// 获取所有交易所类型_交易时间
        /// </summary>
        /// <param name="BourseTypeName">交易所类型名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMTradeTime(string BourseTypeName, int pageNo, int pageSize,
                                         out int rowCount)
        {
            try
            {
                CM_TradeTimeDAL cMTradeTimeDAL = new CM_TradeTimeDAL();
                return cMTradeTimeDAL.GetAllCMTradeTime(BourseTypeName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-4743";
                string errMsg = " 获取所有交易所类型_交易时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region  根据交易所类型_交易时间表中的交易所类型ID获取交易所类型名称

        /// <summary>
        /// 根据交易所类型_交易时间表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetBourseTypeNameByBourseTypeID()
        {
            try
            {
                CM_TradeTimeDAL cMTradeTimeDAL = new CM_TradeTimeDAL();
                return cMTradeTimeDAL.GetBourseTypeNameByBourseTypeID();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4744";
                string errMsg = " 根据交易所类型_交易时间表中的交易所类型ID获取交易所类型名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_TradeTime GetModelByCache(int TradeTimeID)
        {
            string CacheKey = "CM_TradeTimeModel-" + TradeTimeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_TradeTimeDAL.GetModel(TradeTimeID);
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
            return (ManagementCenter.Model.CM_TradeTime)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_TradeTimeDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 根据查询条件获取所有的交易所类型_交易时间（查询条件可为空）

        /// <summary>
        /// 根据查询条件获取所有的交易所类型_交易时间（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_TradeTime> GetListArray(string strWhere)
        {
            try
            {
                return cM_TradeTimeDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4745";
                string errMsg = " 根据查询条件获取所有的交易所类型_交易时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据交易所类型返回交易所时间

        /// <summary>
        /// 根据交易所类型返回交易所时间
        /// </summary>
        /// <param name="BourseTypeID">交易所类型</param>
        /// <returns></returns>
        public DataSet GetTradeTimeByBourseTypeID(int BourseTypeID)
        {
            try
            {
                return cM_TradeTimeDAL.GetList(string.Format("BourseTypeID={0}", BourseTypeID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-4746";
                string errMsg = " 根据交易所类型返回交易所时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据交易所类型ID返回交易时间
        /// <summary>
        /// 根据交易所类型ID返回交易时间
        /// </summary>
        /// <param name="bourseTypeID">交易所类型ID</param>
        /// <returns></returns>
        public DataSet GetTradeTimeAndBourseTypeList(int bourseTypeID)
        {
            try
            {
                CM_TradeTimeDAL cmTradeTimeDAL = new CM_TradeTimeDAL();
                return cmTradeTimeDAL.GetTradeTimeAndBourseTypeList(bourseTypeID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4748";
                string errMsg = " 根据交易所类型ID返回交易时间失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #endregion  成员方法
    }
}