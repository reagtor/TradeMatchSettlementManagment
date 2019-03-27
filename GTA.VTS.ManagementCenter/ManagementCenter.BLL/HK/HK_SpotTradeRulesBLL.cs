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
    ///描述：港股_品种_交易规则 业务逻辑类HK_SpotTradeRulesBLL 的摘要说明。
    ///错误编码范围:7950-7919
    ///作者：刘书伟
    ///日期:2009-10-22
    /// </summary>
    public class HK_SpotTradeRulesBLL
    {
        private readonly HK_SpotTradeRulesDAL hK_SpotTradeRulesDAL = new HK_SpotTradeRulesDAL();
        public HK_SpotTradeRulesBLL()
        { }
        #region  成员方法

        #region 得到最大ID
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return hK_SpotTradeRulesDAL.GetMaxId();
        }
        #endregion

        #region 根据品种ID判断港股交易规则是否存在记录
        /// <summary>
        /// 根据品种ID判断港股交易规则是否存在记录
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool ExistsHKSpotTradeRules(int BreedClassID)
        {
            try
            {
                return hK_SpotTradeRulesDAL.Exists(BreedClassID);

            }
            catch (Exception ex)
            {
                string errCode = "GL-7950";
                string errMsg = "根据品种ID判断港股交易规则是否存在记录失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;

            }
        }
        #endregion

        #region 添加港股交易规则
        /// <summary>
        ///添加港股交易规则 
        /// </summary>
        /// <param name="model">港股交易规则实体</param>
        /// <returns></returns>
        public bool AddHKSpotTradeRules(ManagementCenter.Model.HK_SpotTradeRules model)
        {
            try
            {
                return hK_SpotTradeRulesDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7951";
                string errMsg = "添加港股交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region  更新港股交易规则
        /// <summary>
        /// 更新港股交易规则
        /// </summary>
        /// <param name="model">港股交易规则实体</param>
        /// <returns></returns>
        public bool UpdateHKSpotTradeRules(ManagementCenter.Model.HK_SpotTradeRules model)
        {
            try
            {
                return hK_SpotTradeRulesDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7952";
                string errMsg = "更新港股交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }
        #endregion

        #region 根据品种ID删除港股交易规则(相关)
        /// <summary>
        /// 根据品种ID删除港股交易规则(相关)
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool DeleteHKSpotTradeRulesAbout(int BreedClassID)
        {
            XH_MinVolumeOfBusinessDAL xHMinVolumeOfBusinessDAL = new XH_MinVolumeOfBusinessDAL();

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
                //return hK_SpotTradeRulesDAL.Delete(BreedClassID);
                if (!xHMinVolumeOfBusinessDAL.DeleteXHMinVolumeOfBusByBreedClassID(BreedClassID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!hK_SpotTradeRulesDAL.Delete(BreedClassID, Tran, db))
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
                string errCode = "GL-7953";
                string errMsg = "根据品种ID删除港股交易规则(相关)失败!";
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

        #region 得到港股交易规则的一个对象实体
        /// <summary>
        /// 得到港股交易规则的一个对象实体
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.HK_SpotTradeRules GetModel(int BreedClassID)
        {

            return hK_SpotTradeRulesDAL.GetModel(BreedClassID);
        }
        #endregion

        #region 得到港股交易规则的一个对象实体，从缓存中。
        /// <summary>
        /// 得到港股交易规则的一个对象实体，从缓存中。
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.HK_SpotTradeRules GetModelByCache(int BreedClassID)
        {

            string CacheKey = "HK_SpotTradeRulesModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = hK_SpotTradeRulesDAL.GetModel(BreedClassID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (ManagementCenter.Model.HK_SpotTradeRules)objModel;
        }
        #endregion

        #region 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return hK_SpotTradeRulesDAL.GetList(strWhere);
        }
        #endregion

        #region  获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

        #region  根据查询条件获取所有的港股_品种_交易规则（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的港股_品种_交易规则（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.HK_SpotTradeRules> GetListArray(string strWhere)
        {
            try
            {
                return hK_SpotTradeRulesDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7954";
                string errMsg = "根据查询条件获取所有的港股_品种_交易规则（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 获取所有港股交易规则

        /// <summary>
        /// 获取所有港股交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllHKSpotTradeRules(string BreedClassName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            try
            {
                HK_SpotTradeRulesDAL hKSpotTradeRulesDAL = new HK_SpotTradeRulesDAL();
                return hKSpotTradeRulesDAL.GetAllHKSpotTradeRules(BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-7955";
                string errMsg = "获取所有港股交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion


        #region 根据港股规则表中的品种ID获取品种名称
        /// <summary>
        /// 根据港股规则表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetHKBreedClassNameByBreedClassID()
        {
            try
            {
                return hK_SpotTradeRulesDAL.GetHKBreedClassNameByBreedClassID();
            }
            catch (Exception ex)
            {
                string errCode = "GL-7956";
                string errMsg = "根据港股规则表中的品种ID获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #endregion  成员方法
    }
}
