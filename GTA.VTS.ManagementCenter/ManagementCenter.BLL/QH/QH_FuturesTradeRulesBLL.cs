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
    ///描述：期货_品种_交易规则 业务逻辑类QH_FuturesTradeRulesBLL 的摘要说明。 错误编码范围:6000-6019
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class QH_FuturesTradeRulesBLL
    {
        private readonly ManagementCenter.DAL.QH_FuturesTradeRulesDAL qH_FuturesTradeRulesDAL =
            new ManagementCenter.DAL.QH_FuturesTradeRulesDAL();

        public QH_FuturesTradeRulesBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_FuturesTradeRulesDAL.GetMaxId();
        }

        #region 根据品种ID，判断期货交易规则是否已存在
        /// <summary>
        /// 根据品种ID，判断期货交易规则是否已存在
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool ExistsFuturesTradeRules(int BreedClassID)
        {
            try
            {
                QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                return qHFuturesTradeRulesDAL.Exists(BreedClassID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6000";
                string errMsg = "根据品种ID，判断期货交易规则是否已存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 添加期货交易规则
        /// </summary>
        /// <param name="model">期货_品种_交易规则实体</param>
        /// <returns></returns>
        public bool AddFuturesTradeRules(ManagementCenter.Model.QH_FuturesTradeRules model)
        {
            try
            {
                QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                return qH_FuturesTradeRulesDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6001";
                string errMsg = "添加期货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 更新期货交易规则
        /// </summary>
        /// <param name="model">期货_品种_交易规则实体</param>
        /// <returns></returns>
        public bool UpdateFuturesTradeRules(ManagementCenter.Model.QH_FuturesTradeRules model)
        {
            try
            {
                QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                return qH_FuturesTradeRulesDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6002";
                string errMsg = "更新期货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 删除期货_品种_交易规则
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        public bool DeleteFuturesTradeRules(int BreedClassID)
        {
            try
            {
                QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                return qH_FuturesTradeRulesDAL.Delete(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6003";
                string errMsg = "删除期货_品种_交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #region 根据品种标识,删除期货品种交易规则(规则相关表全部删除)

        /// <summary>
        ///根据品种标识,删除期货品种交易规则(规则相关表全部删除) 
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <returns></returns>
        public bool DeleteFuturesTradeRulesAboutAll(int BreedClassID)
        {
            QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL=new QH_FuturesTradeRulesDAL();
            QH_AgreementDeliveryMonthDAL qH_AgreementDeliveryMonthDAL=new QH_AgreementDeliveryMonthDAL();
            QH_ConsignQuantumDAL qH_ConsignQuantumDAL=new QH_ConsignQuantumDAL();
            QH_SingleRequestQuantityDAL qH_SingleRequestQuantityDAL=new QH_SingleRequestQuantityDAL();
            QH_LastTradingDayDAL qH_LastTradingDayDAL=new QH_LastTradingDayDAL();

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
                int ConsignQuantumID = AppGlobalVariable.INIT_INT;
                int LastTradingDayID = AppGlobalVariable.INIT_INT;
                QH_FuturesTradeRules qHFuturesTradeRules = new QH_FuturesTradeRules();
                qHFuturesTradeRules = qHFuturesTradeRulesDAL.GetModel(BreedClassID);
                if (qHFuturesTradeRules != null)
                {
                    if (!string.IsNullOrEmpty(qHFuturesTradeRules.ConsignQuantumID.ToString()))
                    {
                        ConsignQuantumID = Convert.ToInt32(qHFuturesTradeRules.ConsignQuantumID);
                    }
                    if (!string.IsNullOrEmpty(qHFuturesTradeRules.LastTradingDayID.ToString()))
                    {
                        LastTradingDayID = Convert.ToInt32(qHFuturesTradeRules.LastTradingDayID);
                    }
                    if (ConsignQuantumID != AppGlobalVariable.INIT_INT)
                    {
                        if (!qH_SingleRequestQuantityDAL.DeleteSingleRQByConsignQuantumID(ConsignQuantumID, Tran, db))
                        {
                            Tran.Rollback();
                            return false;
                        }

                    }


                    List<Model.QH_AgreementDeliveryMonth> qHAgreementDeliveryM =
                        qH_AgreementDeliveryMonthDAL.GetListArray(string.Format("BreedClassID={0}", BreedClassID), Tran,
                                                                  db);
                    foreach (Model.QH_AgreementDeliveryMonth MonthID in qHAgreementDeliveryM)
                    {

                        if (!qH_AgreementDeliveryMonthDAL.Delete((Int32) MonthID.MonthID, BreedClassID)) //?处理
                        {
                            Tran.Rollback();
                            return false;
                        }
                    }

                    if (!qHFuturesTradeRulesDAL.Delete(BreedClassID, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }

                    if (ConsignQuantumID != AppGlobalVariable.INIT_INT)
                    {
                        if (!qH_ConsignQuantumDAL.Delete(ConsignQuantumID, Tran, db))
                        {
                            Tran.Rollback();
                            return false;
                        }
                    }
                    if (LastTradingDayID != AppGlobalVariable.INIT_INT)
                    {
                        if (!qH_LastTradingDayDAL.Delete(LastTradingDayID, Tran, db))
                        {
                            Tran.Rollback();
                            return false;
                        }
                    }
                }
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6004";
                string errMsg = " 根据品种标识,删除期货品种交易规则(规则相关表全部删除)失败!";
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
        /// 根据品种ID，获取期货交易规则对象实体
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public ManagementCenter.Model.QH_FuturesTradeRules GetFuturesTradeRulesModel(int BreedClassID)
        {
            try
            {
                QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                return qHFuturesTradeRulesDAL.GetModel(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6005";
                string errMsg = " 根据品种ID，获取期货交易规则对象实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_FuturesTradeRules GetModelByCache(int BreedClassID)
        {
            string CacheKey = "QH_FuturesTradeRulesModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_FuturesTradeRulesDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.QH_FuturesTradeRules) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_FuturesTradeRulesDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的期货_品种_交易规则（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_FuturesTradeRules> GetListArray(string strWhere)
        {
            try
            {
                return qH_FuturesTradeRulesDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6006";
                string errMsg = " 根据查询条件获取所有的期货_品种_交易规则（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #region 获取所有期货_品种_交易规则

        /// <summary>
        /// 获取所有期货_品种_交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllFuturesTradeRules(string BreedClassName, int pageNo, int pageSize,
                                               out int rowCount)
        {
            try
            {
                QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                return qH_FuturesTradeRulesDAL.GetAllFuturesTradeRules(BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-6007";
                string errMsg = "获取所有期货_品种_交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法

        #region 根据期货_品种_交易规则表中的品种标识获取品种名称

        /// <summary>
        /// 根据期货_品种_交易规则表中的品种标识获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHBreedClassNameByBreedClassID()
        {
            try
            {
                QH_FuturesTradeRulesDAL qHFuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                return qH_FuturesTradeRulesDAL.GetQHBreedClassNameByBreedClassID();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6008";
                string errMsg = "根据期货_品种_交易规则表中的品种标识获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion
    }
}