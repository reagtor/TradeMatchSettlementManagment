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
    ///描述：现货_品种_交易规则 业务逻辑类XH_SpotTradeRulesBLL 的摘要说明。错误编码范围:5200-5219
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class XH_SpotTradeRulesBLL
    {
        private readonly ManagementCenter.DAL.XH_SpotTradeRulesDAL xH_SpotTradeRulesDAL =
            new ManagementCenter.DAL.XH_SpotTradeRulesDAL();

        public XH_SpotTradeRulesBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_SpotTradeRulesDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID)
        {
            return xH_SpotTradeRulesDAL.Exists(BreedClassID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(ManagementCenter.Model.XH_SpotTradeRules model)
        {
            try
            {
                return xH_SpotTradeRulesDAL.Add(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
                //throw;
            }
        }

        #region 更新现货交易规则

        /// <summary>
        /// 更新现货交易规则
        /// </summary>
        /// <param name="model">现货_品种_交易规则实体</param>
        /// <returns></returns>
        public bool UpdateSpotTradeRules(ManagementCenter.Model.XH_SpotTradeRules model)
        {
            try
            {
                return xH_SpotTradeRulesDAL.UpdateSpotTradeRules(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5203";
                string errMsg = "更新现货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int BreedClassID)
        {
            xH_SpotTradeRulesDAL.Delete(BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_SpotTradeRules GetModel(int BreedClassID)
        {
            return xH_SpotTradeRulesDAL.GetModel(BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_SpotTradeRules GetModelByCache(int BreedClassID)
        {
            string CacheKey = "XH_SpotTradeRulesModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_SpotTradeRulesDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.XH_SpotTradeRules) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_SpotTradeRulesDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 根据查询条件获取所有的现货_品种_交易规则（查询条件可为空）

        /// <summary>
        /// 根据查询条件获取所有的现货_品种_交易规则（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_SpotTradeRules> GetListArray(string strWhere)
        {
            try
            {
                return xH_SpotTradeRulesDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5205";
                string errMsg = "根据查询条件获取所有的现货_品种_交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 根据现货规则表中的品种ID获取品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetBreedClassNameByBreedClassID()
        {
            try
            {
                return xH_SpotTradeRulesDAL.GetBreedClassNameByBreedClassID();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5206";
                string errMsg = "根据现货规则表中的品种ID获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法

        #region 添加现货交易规则

        /// <summary>
        ///  添加现货交易规则
        /// </summary>
        /// <param name="xHSpotTradeRules">现货交易规则实体类</param>
        /// <returns></returns>
        public bool AddXHSpotTradeRules(ManagementCenter.Model.XH_SpotTradeRules xHSpotTradeRules)
        {
            try
            {
                XH_SpotTradeRulesDAL xHSpotTradeRulesDAL = new XH_SpotTradeRulesDAL();
                return xHSpotTradeRulesDAL.Add(xHSpotTradeRules);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5200";
                string errMsg = "添加现货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            //return true;
        }

        #endregion

        #region 根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则

        /// <summary>
        ///根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则 
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public bool DeleteSpotTradeRules(int BreedClassID, int BreedClassHighLowID, int BreedClassValidID)
        {
            XH_SpotTradeRulesDAL xHSpotTradeRulesDAL = new XH_SpotTradeRulesDAL();
            XH_SpotHighLowControlTypeBLL xHSpotHighLowControlTypeBLL = new XH_SpotHighLowControlTypeBLL();
            XH_ValidDeclareTypeBLL xHValidDeclareTypeBLL = new XH_ValidDeclareTypeBLL();

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
                //?先删除交易规则_交易方向_交易单位_交易量(最小交易单位)
                xHSpotTradeRulesDAL.Delete(BreedClassID, Tran, db);
                if (BreedClassHighLowID != AppGlobalVariable.INIT_INT)
                {
                    if (xHSpotHighLowControlTypeBLL.DeleteSpotHighLowValue(BreedClassHighLowID, Tran, db, true))
                    {
                        if (BreedClassValidID != AppGlobalVariable.INIT_INT)
                        {
                            if (xHValidDeclareTypeBLL.DeleteValidDeclareValue(BreedClassValidID, Tran, db, true))
                            {
                                Tran.Commit();
                                return true;
                            }
                        }
                    }
                }
                Tran.Rollback();
                return false;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-5201";
                string errMsg = "根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return false;
            }
            finally
            {
                if (Conn != null && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #endregion

        #region 根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则(规则相关表全部删除)

        /// <summary>
        ///根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则(规则相关表全部删除) 
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        ///// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        ///// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public bool DeleteSpotTradeRulesAboutAll(int BreedClassID)
        {
            XH_SpotTradeRulesDAL xHSpotTradeRulesDAL = new XH_SpotTradeRulesDAL();
            XH_SpotPositionDAL xHSpotPositionDAL = new XH_SpotPositionDAL();
            XH_SpotHighLowValueDAL xHSpotHighLowValueDAL = new XH_SpotHighLowValueDAL();
            XH_ValidDeclareValueDAL xHValidDeclareValueDAL = new XH_ValidDeclareValueDAL();
            //XH_MinChangePriceValueDAL xHMinChangePriceValueDAL = new XH_MinChangePriceValueDAL();
            CM_FieldRangeDAL cMFieldRangeDAL = new CM_FieldRangeDAL();
            XH_ValidDeclareTypeDAL xHValidDeclareTypeDAL = new XH_ValidDeclareTypeDAL();
            XH_MinVolumeOfBusinessDAL xHMinVolumeOfBusinessDAL = new XH_MinVolumeOfBusinessDAL();

            XH_SpotHighLowControlTypeDAL xHSpotHighLowControlTypeDAL = new XH_SpotHighLowControlTypeDAL();
            CM_UnitConversionDAL cM_UnitConversionDAL=new CM_UnitConversionDAL();


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
                int BreedClassHighLowID = AppGlobalVariable.INIT_INT;
                int BreedClassValidID = AppGlobalVariable.INIT_INT;
                XH_SpotTradeRules xHSpotTradeRules = new XH_SpotTradeRules();
                xHSpotTradeRules = xHSpotTradeRulesDAL.GetModel(BreedClassID);
                if (xHSpotTradeRules != null)
                {
                    if (!string.IsNullOrEmpty(xHSpotTradeRules.BreedClassHighLowID.ToString()))
                {
                    BreedClassHighLowID = Convert.ToInt32(xHSpotTradeRules.BreedClassHighLowID);
                }
                if (!string.IsNullOrEmpty(xHSpotTradeRules.BreedClassValidID.ToString()))
                {
                    BreedClassValidID = Convert.ToInt32(xHSpotTradeRules.BreedClassValidID);
                }
                if (BreedClassHighLowID != AppGlobalVariable.INIT_INT)
                {
                    if (!xHSpotHighLowValueDAL.DeleteSpotHighLowValue(BreedClassHighLowID, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }
                if (BreedClassValidID != AppGlobalVariable.INIT_INT)
                {
                    if (!xHValidDeclareValueDAL.DeleteVDeclareValue(BreedClassValidID, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }
                //if (!xHMinChangePriceValueDAL.Delete(BreedClassID, Tran, db))
                //{
                //    Tran.Rollback();
                //    return false;
                //}

                //List<Model.XH_MinChangePriceValue> xhMinCHangePriceV =
                //    xHMinChangePriceValueDAL.GetListArray(string.Format("BreedClassID={0}", BreedClassID), Tran, db);
                //foreach (Model.XH_MinChangePriceValue FieldRangeID in xhMinCHangePriceV)
                //{
                //    if (!cMFieldRangeDAL.Delete(FieldRangeID.FieldRangeID))
                //    {
                //        Tran.Rollback();
                //        return false;
                //    }
                //}
                if (!xHMinVolumeOfBusinessDAL.DeleteXHMinVolumeOfBusByBreedClassID(BreedClassID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                //添加删除现货单位换算
                List<Model.CM_UnitConversion> cMUnitC =
                    cM_UnitConversionDAL.GetListArray(string.Format("BreedClassID={0}", BreedClassID));
                foreach (Model.CM_UnitConversion unitConversion in cMUnitC)
                {
                    if (!cM_UnitConversionDAL.DeleteUnitConversionByBreedClassID(Convert.ToInt32(unitConversion.BreedClassID),Tran,db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }

                if (!xHSpotTradeRulesDAL.Delete(BreedClassID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!xHValidDeclareTypeDAL.DeleteValidDeclareType(BreedClassValidID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!xHSpotHighLowControlTypeDAL.Delete(BreedClassHighLowID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                }
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-5202";
                string errMsg = "根据品种标识,品种涨跌幅标识,品种有效申报标识,删除现货品种交易规则(规则相关表全部删除)失败!";
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

        #region 获取所有现货交易规则

        /// <summary>
        /// 获取所有现货交易规则
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllSpotTradeRules(string BreedClassName, int pageNo, int pageSize,
                                            out int rowCount)
        {
            try
            {
                XH_SpotTradeRulesDAL xHSpotTradeRulesDAL = new XH_SpotTradeRulesDAL();
                return xHSpotTradeRulesDAL.GetAllSpotTradeRules(BreedClassName, pageNo, pageSize,
                                                                out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-5204";
                string errMsg = "获取所有现货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return null;
            }
        }

        #endregion

        #region 根据品种ID返回现货规则明细(此品种的涨跌幅和有效申报)数据

        /// <summary>
        /// 根据品种ID返回现货规则明细(此品种的涨跌幅和有效申报)数据
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public DataSet GetSpotTradeRulesDetail(int BreedClassID)
        {
            try
            {
                XH_SpotTradeRulesDAL xHSpotTradeRulesDAL = new XH_SpotTradeRulesDAL();
                return xHSpotTradeRulesDAL.GetSpotTradeRulesDetail(BreedClassID);
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        #endregion
    }
}