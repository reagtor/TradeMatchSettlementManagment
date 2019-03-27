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
using Types = GTA.VTS.Common.CommonObject.Types;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：交易商品品种 业务逻辑类CM_BreedClassBLL 的摘要说明。错误编码范围:4100-4130
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    public class CM_BreedClassBLL
    {
        private readonly ManagementCenter.DAL.CM_BreedClassDAL cM_BreedClassDAL =
            new ManagementCenter.DAL.CM_BreedClassDAL();

        public CM_BreedClassBLL()
        {
        }

        #region  成员方法

        /// <summary>
        ///获取交易商品品种表的最大ID 
        /// </summary>
        /// <returns></returns>
        public int GetCMBreedClassMaxId()
        {
            try
            {
                return cM_BreedClassDAL.GetMaxId();

            }
            catch (Exception ex)
            {
                string errCode = "GL-4116";
                string errMsg = "获取交易商品品种表的最大ID失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID)
        {
            return cM_BreedClassDAL.Exists(BreedClassID);
        }

        /// <summary>
        /// 添加交易商品品种
        /// </summary>
        /// <param name="model">交易商品品种实体</param>
        /// <returns></returns>
        //public int AddCMBreedClass(CM_BreedClass model)
        public bool AddCMBreedClass(CM_BreedClass model)
        {
            try
            {
                return cM_BreedClassDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4100";
                string errMsg = "添加交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                //return AppGlobalVariable.INIT_INT;
                return false;
            }
        }

        /// <summary>
        /// 更新交易商品品种
        /// </summary>
        /// <param name="model">交易商品品种实体</param>
        /// <returns></returns>
        public bool UpdateCMBreedClass(CM_BreedClass model)
        {
            try
            {
                return cM_BreedClassDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4102";
                string errMsg = "更新交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 删除交易商品品种
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool DeleteCMBreedClass(int BreedClassID)
        {
            try
            {
                return cM_BreedClassDAL.Delete(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4101";
                string errMsg = "删除交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #region 删除品种时，则根据品种ID，删除所有相关联的表
        /// <summary>
        /// 删除品种时，则根据品种ID，删除所有相关联的表
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool DeleteCMBreedClassALLAbout(int BreedClassID)
        {
            XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
            QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
            QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
            CM_CommodityDAL cM_CommodityDAL = new CM_CommodityDAL();
            CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
            CM_BreedClass cM_BreedClass = new CM_BreedClass();
            XH_SpotPositionDAL xH_SpotPositionDAL = new XH_SpotPositionDAL();
            UM_DealerTradeBreedClassDAL uM_DealerTradeBreedClassDAL = new UM_DealerTradeBreedClassDAL();
            UM_DealerTradeBreedClass uM_DealerTradeBreedClass = new UM_DealerTradeBreedClass();
            //QH_SIFPositionDAL qH_SIFPositionDAL=new QH_SIFPositionDAL();//股指期货持仓限制
            //QH_SIFPosition qH_SIFPosition=new QH_SIFPosition();
            QH_PositionLimitValueDAL qH_PositionLimitValueDAL = new QH_PositionLimitValueDAL();//期货持仓限制
            XH_SpotCostsDAL xH_SpotCostsDAL = new XH_SpotCostsDAL();//现货交易费用
            QH_FutureCostsDAL qH_FutureCostsDAL = new QH_FutureCostsDAL();//期货交易费用
            QH_CFBailScaleValueDAL qH_CFBailScaleValueDAL = new QH_CFBailScaleValueDAL();//商品期货_保证金比例

            HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();//港股交易规则BLL
            HK_SpotCostsDAL hK_SpotCostsDAL = new HK_SpotCostsDAL();//港股交易费用
            HK_CommodityDAL hK_CommodityDAL = new HK_CommodityDAL();//港股交易商品

            //期货保证金 add by 董鹏 2010-02-02
            QH_SIFBailDAL qh_SIFBailDal = new QH_SIFBailDAL();

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
                int NewBreedClassID = AppGlobalVariable.INIT_INT; //未分配品种ID
                //获取系统默认未分配品种的品种ID
                List<CM_BreedClass> cMBreedClassList =
                    cMBreedClassDAL.GetListArray(string.Format("DeleteState={0}", (int)Types.IsYesOrNo.Yes));
                if (cMBreedClassList.Count == 0)
                {
                    return false;
                }
                if (cMBreedClassList[0].ISSysDefaultBreed == (int)Types.IsYesOrNo.Yes)
                {
                    NewBreedClassID = cMBreedClassList[0].BreedClassID;
                }
                cM_BreedClass = cMBreedClassDAL.GetModel(BreedClassID);
                if (cM_BreedClass == null)
                {
                    return false;
                }

                //删除撮合机里的代码
                int ISHKBreedClassType = Convert.ToInt32(cM_BreedClass.ISHKBreedClassType);//是否港股类型
                if (ISHKBreedClassType == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes)//当品种类型是港股时
                {
                    List<ManagementCenter.Model.HK_Commodity> hkCommodityList =
                        hK_CommodityDAL.GetListArray(string.Format("BreedClassID={0}", BreedClassID));
                    if (hkCommodityList.Count > 0)
                    {
                        List<ManagementCenter.Model.RC_TradeCommodityAssign> rcTradeCommodityAssignList =
                            rC_TradeCommodityAssignDAL.GetListArray(string.Format("CodeFormSource={0}",
                                                                                  (int)Types.IsCodeFormSource.No));
                        if (rcTradeCommodityAssignList.Count > 0)
                        {
                            for (int i = 0; i < hkCommodityList.Count; i++)
                            {
                                for (int j = i; j < rcTradeCommodityAssignList.Count; j++)
                                {
                                    if (hkCommodityList[i].HKCommodityCode == rcTradeCommodityAssignList[j].CommodityCode)
                                    {
                                        if (!rC_TradeCommodityAssignDAL.DeleteRCByCommodityCode(hkCommodityList[i].HKCommodityCode, Tran, db))
                                        {
                                            Tran.Rollback();
                                            return false;
                                        }
                                        break;
                                    }
                                }
                            }

                        }

                    }

                }
                else if (ISHKBreedClassType == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No)//当其它品种类型是时)
                {
                    List<ManagementCenter.Model.CM_Commodity> cmCommodityList =
                       cM_CommodityDAL.GetListArray(string.Format("BreedClassID={0}", BreedClassID));
                    if (cmCommodityList.Count > 0)
                    {
                        List<ManagementCenter.Model.RC_TradeCommodityAssign> rcTradeCommodityAssignList =
                            rC_TradeCommodityAssignDAL.GetListArray(string.Format("CodeFormSource={0}",
                                                                                  (int)Types.IsCodeFormSource.Yes));
                        if (rcTradeCommodityAssignList.Count > 0)
                        {
                            for (int i = 0; i < cmCommodityList.Count; i++)
                            {
                                for (int j = i; j < rcTradeCommodityAssignList.Count; j++)
                                {
                                    if (cmCommodityList[i].CommodityCode == rcTradeCommodityAssignList[j].CommodityCode)
                                    {
                                        if (!rC_TradeCommodityAssignDAL.DeleteRCByCommodityCode(cmCommodityList[i].CommodityCode, Tran, db))
                                        {
                                            Tran.Rollback();
                                            return false;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                //品种类型ID
                int breedClassType = Convert.ToInt32(cM_BreedClass.BreedClassTypeID);
                if (breedClassType == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.Stock)
                {
                    if (!xH_SpotTradeRulesBLL.DeleteSpotTradeRulesAboutAll(BreedClassID))
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!xH_SpotPositionDAL.Delete(BreedClassID, Tran, db)) //删除持仓
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!xH_SpotCostsDAL.Delete(BreedClassID, Tran, db)) //删除现货交易费用
                    {
                        Tran.Rollback();
                        return false;
                    }

                }
                else if (breedClassType == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    if (!qH_FuturesTradeRulesBLL.DeleteFuturesTradeRulesAboutAll(BreedClassID))
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!qH_SIFPositionBLL.DeleteQHSIFPositionAndQHSIFBail(BreedClassID))
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!qH_PositionLimitValueDAL.DeletePositionLimitVByBreedClassID(BreedClassID, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!qH_FutureCostsDAL.Delete(BreedClassID, Tran, db)) //删除期货交易费用
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!qH_CFBailScaleValueDAL.DeleteCFBailScaleVByBreedClassID(BreedClassID, Tran, db))//删除商品期货_保证金比例
                    {
                        Tran.Rollback();
                        return false;
                    }
                }
                else if (breedClassType == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.HKStock)
                {
                    if (!hK_SpotTradeRulesBLL.DeleteHKSpotTradeRulesAbout(BreedClassID))
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!hK_SpotCostsDAL.Delete(BreedClassID, Tran, db)) //删除港股交易费用
                    {
                        Tran.Rollback();
                        return false;
                    }
                    if (!xH_SpotPositionDAL.Delete(BreedClassID, Tran, db)) //删除持仓
                    {
                        Tran.Rollback();
                        return false;
                    }
                    //根据品种ID，更新港股交易商品表中的品种ID(
                    if (!hK_CommodityDAL.UpdateHKBreedClassID(BreedClassID, NewBreedClassID, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }
                if (breedClassType != (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.HKStock)
                {
                    //根据品种ID，更新交易商品表中的品种ID(
                    if (!cM_CommodityDAL.UpdateBreedClassID(BreedClassID, NewBreedClassID, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }

                //删除品种权限表记录
                List<Model.UM_DealerTradeBreedClass> uMDealerTradeBreedClass =
                    uM_DealerTradeBreedClassDAL.GetListArray(string.Format("BreedClassID={0}", BreedClassID));
                foreach (Model.UM_DealerTradeBreedClass umDTradeBreedClass in uMDealerTradeBreedClass)
                {
                    if (!uM_DealerTradeBreedClassDAL.DeleteDealerTradeByBreedClassID(Convert.ToInt32(umDTradeBreedClass.BreedClassID), Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }
                //删除期货保证金记录 add by 董鹏 2010-02-02
                if (!qh_SIFBailDal.Delete(BreedClassID, Tran, db))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!cMBreedClassDAL.Delete(BreedClassID, Tran, db))
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
                string errCode = "GL-4115";
                string errMsg = " 删除品种时，则根据品种ID，删除所有相关联的表失败!";
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
        public ManagementCenter.Model.CM_BreedClass GetModel(int BreedClassID)
        {
            return cM_BreedClassDAL.GetModel(BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_BreedClass GetModelByCache(int BreedClassID)
        {
            string CacheKey = "CM_BreedClassModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_BreedClassDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.CM_BreedClass)objModel;
        }

        #region 根据查询条件获取品种数据列表
        /// <summary>
        /// 根据查询条件获取品种数据列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public DataSet GetList(string strWhere)
        {
            try
            {
                return cM_BreedClassDAL.GetList(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4114";
                string errMsg = "根据查询条件获取品种数据列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的交易商品品种（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_BreedClass> GetListArray(string strWhere)
        {
            try
            {
                return cM_BreedClassDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4103";
                string errMsg = "根据查询条件获取所有的交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #region 获取现货品种名称

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetBreedClassName()
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4104";
                string errMsg = "获取现货品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取所有交易商品品种

        /// <summary>
        /// 获取所有交易商品品种
        /// </summary>
        /// <param name="BreedClassTypeID">品种类型ID</param>
        ///  <param name="BourseTypeID">交易所类型ID</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMBreedClass(int BreedClassTypeID, int BourseTypeID, int pageNo, int pageSize,
                                          out int rowCount)
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetAllCMBreedClass(BreedClassTypeID, BourseTypeID, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-4105";
                string errMsg = "获取所有交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region  根据交易商品品种表中的交易所类型ID获取交易所类型名称

        /// <summary>
        /// 根据交易商品品种表中的交易所类型ID获取交易所类型名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetCMBreedClassBourseTypeName()
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetCMBreedClassBourseTypeName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4106";
                string errMsg = "根据交易商品品种表中的交易所类型ID获取交易所类型名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取所有品种名称

        /// <summary>
        /// 获取所有品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllBreedClassName()
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetAllBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4107";
                string errMsg = "获取所有品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取品种类型是商品期货的品种名称

        /// <summary>
        /// 获取品种类型是商品期货的品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetSpQhTypeBreedClassName()
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetSpQhTypeBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4108";
                string errMsg = "获取品种类型是商品期货的品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取品种类型是商品期货或股指期货的品种名称

        /// <summary>
        /// 获取品种类型是商品期货或股指期货的品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHFutureCostsBreedClassName()
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetQHFutureCostsBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4109";
                string errMsg = "获取品种类型是商品期货或股指期货的品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取品种类型是股指期货的品种名称

        /// <summary>
        /// 获取品种类型是股指期货的品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHSIFPositionAndBailBreedClassName()
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetQHSIFPositionAndBailBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4110";
                string errMsg = "获取品种类型是股指期货的品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region  判断品种名称是否已经存在
        /// <summary>
        /// 判断品种名称是否已经存在
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <returns></returns>
        public bool IsExistBreedClassName(string BreedClassName)
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                string strWhere = string.Format("BreedClassName='{0}'", BreedClassName);
                DataSet _ds = cMBreedClassDAL.GetList(strWhere);
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
                string errCode = "GL-4111";
                string errMsg = "判断交易商品品种名称是否已经存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;

            }
        }
        #endregion

        #region 获取现货普通和港股品种名称

        /// <summary>
        /// 获取现货普通和港股品种名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetXHAndHKBreedClassName()
        {
            try
            {
                CM_BreedClassDAL cMBreedClassDAL = new CM_BreedClassDAL();
                return cMBreedClassDAL.GetXHAndHKBreedClassName();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4113";
                string errMsg = "获取现货普通和港股品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 根据品种标识返回交易商品品种实体
        /// <summary>
        /// 根据品种标识返回交易商品品种实体
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public CM_BreedClass GetBreedClassByBClassID(int breedClassID)
        {
            try
            {
                CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
                List<CM_BreedClass> cM_BreedClassList =
                    cM_BreedClassBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (cM_BreedClassList.Count > 0)
                {
                    CM_BreedClass cM_BreedClass = cM_BreedClassList[0];
                    if (cM_BreedClass != null)
                    {
                        return cM_BreedClass;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4114";
                string errMsg = "根据品种标识返回交易商品品种实体失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}