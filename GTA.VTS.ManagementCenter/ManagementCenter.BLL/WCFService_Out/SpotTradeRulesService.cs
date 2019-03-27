using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL.WCFService_Out.interfases;
using Entity = ManagementCenter.Model;


namespace ManagementCenter.BLL.WCFService_Out
{
    /// <summary>
    /// 描述：现货交易规则对外服务
    /// 作者：刘书伟
    /// 日期：2008-11-20
    /// 描述:添加Debug日志
    /// 修改作者：刘书伟
    /// 日期：2010-05-10
    /// </summary>
    // 错误编码 8400-8499
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SpotTradeRulesService : ISpotTradeRules
    {
        #region ISpotTradeRules 成员

        /// <summary>
        /// 获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_MinVolumeOfBusiness> GetAllMinVolumeOfBusiness()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8402获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)方法名称:GetAllMinVolumeOfBusiness()" + DateTime.Now);
                XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
                return xH_MinVolumeOfBusinessBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8402";
                string errMsg = "获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<Entity.XH_MinVolumeOfBusiness> GetMinVolumeOfBusinessByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8403根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)方法名称:GetMinVolumeOfBusinessByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
                return xH_MinVolumeOfBusinessBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8403";
                string errMsg = "根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        ///  获取所有的品种_现货_交易费用
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_SpotCosts> GetAllSpotCosts()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8404获取所有的品种_现货_交易费用方法名称:GetAllSpotCosts()" + DateTime.Now);
                XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
                return xH_SpotCostsBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8404";
                string errMsg = "获取所有的品种_现货_交易费用失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回品种_现货_交易费用
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.XH_SpotCosts GetSpotCostsByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8405根据品种标识返回品种_现货_交易费用方法名称:GetSpotCostsByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                XH_SpotCostsBLL xH_SpotCostsBLL = new XH_SpotCostsBLL();
                //Entity.XH_SpotCosts xH_SpotCosts =
                //    xH_SpotCostsBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID))[0];
                List<Entity.XH_SpotCosts> xH_SpotCostsList =
                    xH_SpotCostsBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (xH_SpotCostsList.Count > 0)
                {
                    Entity.XH_SpotCosts xH_SpotCosts = xH_SpotCostsList[0];
                    if (xH_SpotCosts != null)
                    {
                        return xH_SpotCosts;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8405";
                string errMsg = "根据品种标识返回品种_现货_交易费用失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的现货_品种_涨跌幅_控制类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_SpotHighLowControlType> GetAllSpotHighLowControlType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8406获取所有的现货_品种_涨跌幅_控制类型方法名称:GetAllSpotHighLowControlType()" + DateTime.Now);
                XH_SpotHighLowControlTypeBLL xH_SpotHighLowControlTypeBLL = new XH_SpotHighLowControlTypeBLL();
                return xH_SpotHighLowControlTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8406";
                string errMsg = "获取所有的现货_品种_涨跌幅_控制类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的现货_交易商品品种_持仓限制
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_SpotPosition> GetAllSpotPosition()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8407获取所有的现货_交易商品品种_持仓限制方法名称:GetAllSpotPosition()" + DateTime.Now);
                XH_SpotPositionBLL xH_SpotPositionBLL = new XH_SpotPositionBLL();
                return xH_SpotPositionBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8407";
                string errMsg = "获取所有的现货_交易商品品种_持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.XH_SpotPosition GetSpotPositionByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8408根据品种标识返回现货_交易商品品种_持仓限制方法名称:GetSpotPositionByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                XH_SpotPositionBLL xH_SpotPositionBLL = new XH_SpotPositionBLL();
                List<Entity.XH_SpotPosition> xH_SpotPositionList =
                xH_SpotPositionBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (xH_SpotPositionList.Count > 0)
                {
                    Entity.XH_SpotPosition xH_SpotPosition = xH_SpotPositionList[0];
                    if (xH_SpotPosition != null)
                    {
                        return xH_SpotPosition;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8408";
                string errMsg = "根据品种标识返回现货_交易商品品种_持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的现货_品种_交易规则
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_SpotTradeRules> GetAllSpotTradeRules()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8411获取所有的现货_品种_交易规则方法名称:GetAllSpotTradeRules()" + DateTime.Now);
                XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
                return xH_SpotTradeRulesBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8411";
                string errMsg = "获取所有的现货_品种_交易规则失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回现货_品种_交易规则
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.XH_SpotTradeRules GetSpotTradeRulesByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8412根据品种标识返回现货_品种_交易规则方法名称:GetSpotTradeRulesByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                XH_SpotTradeRulesBLL xH_SpotTradeRulesBLL = new XH_SpotTradeRulesBLL();
                var list = xH_SpotTradeRulesBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));

                if (list.Count > 0)
                {
                    Entity.XH_SpotTradeRules xH_SpotTradeRules = list[0];
                    if (xH_SpotTradeRules != null)
                    {
                        return xH_SpotTradeRules;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8412";
                string errMsg = "根据品种标识返回现货_品种_交易规则失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的现货_品种_涨跌幅
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_SpotHighLowValue> GetAllSpotHighLowValue()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8413获取所有的现货_品种_涨跌幅方法名称:GetAllSpotHighLowValue()" + DateTime.Now);
                XH_SpotHighLowValueBLL xH_SpotHighLowValueBLL = new XH_SpotHighLowValueBLL();
                return xH_SpotHighLowValueBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8413";
                string errMsg = "获取所有的现货_品种_涨跌幅失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据涨跌幅标识返回现货_品种_涨跌幅
        /// </summary>
        /// <param name="hightLowValueID">涨跌幅取值标识</param>
        /// <returns></returns>
        public Entity.XH_SpotHighLowValue GetSpotHighLowValueByHightLowID(int hightLowValueID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8414根据涨跌幅标识返回现货_品种_涨跌幅方法名称:GetSpotHighLowValueByHightLowID(int hightLowValueID)参数是:" + hightLowValueID + "时间是:" + DateTime.Now);
                XH_SpotHighLowValueBLL xH_SpotHighLowValueBLL = new XH_SpotHighLowValueBLL();
                List<Entity.XH_SpotHighLowValue> xH_SpotHighLowValueList =
                xH_SpotHighLowValueBLL.GetListArray(string.Format("HightLowValueID={0}", hightLowValueID));

                if (xH_SpotHighLowValueList.Count > 0)
                {
                    Entity.XH_SpotHighLowValue xH_SpotHighLowValue = xH_SpotHighLowValueList[0];
                    if (xH_SpotHighLowValue != null)
                    {
                        return xH_SpotHighLowValue;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8414";
                string errMsg = "根据涨跌幅标识返回现货_品种_涨跌幅失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据涨跌幅类型标识返回现货_品种_涨跌幅
        /// </summary>
        /// <param name="breedClassHighLowID">品种涨跌幅类型标识</param>
        /// <returns></returns>
        public List<Entity.XH_SpotHighLowValue> GetSpotHighLowValueByBreedClassHighLowID(int breedClassHighLowID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8415根据涨跌幅类型标识返回现货_品种_涨跌幅方法名称:GetSpotHighLowValueByBreedClassHighLowID(int breedClassHighLowID)参数是:" + breedClassHighLowID + "时间是:" + DateTime.Now);
                XH_SpotHighLowValueBLL xH_SpotHighLowValueBLL = new XH_SpotHighLowValueBLL();
                return xH_SpotHighLowValueBLL.GetListArray(string.Format("BreedClassHighLowID={0}", breedClassHighLowID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8415";
                string errMsg = "根据涨跌幅类型标识返回现货_品种_涨跌幅失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据涨跌幅标识获取权证涨跌幅价格
        /// </summary>
        /// <param name="hightLowID">涨跌幅标识</param>
        /// <returns></returns>
        public List<Entity.XH_RightHightLowPrices> GetRightHightLowPricesByHightLowID(int hightLowID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8417根据涨跌幅标识获取权证涨跌幅价格方法名称:GetRightHightLowPricesByHightLowID(int hightLowID)参数是:" + hightLowID + "时间是:" + DateTime.Now);
                XH_RightHightLowPricesBLL xH_RightHightLowPricesBLL = new XH_RightHightLowPricesBLL();
                return xH_RightHightLowPricesBLL.GetListArray(string.Format("ByHightLowID={0}", hightLowID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8417";
                string errMsg = "根据涨跌幅标识获取权证涨跌幅价格失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的有效申报类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_ValidDeclareType> GetAllValidDeclareType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8418获取所有的有效申报类型方法名称:GetAllValidDeclareType()" + DateTime.Now);
                XH_ValidDeclareTypeBLL xH_ValidDeclareTypeBLL = new XH_ValidDeclareTypeBLL();
                return xH_ValidDeclareTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8418";
                string errMsg = "获取所有的有效申报类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种有效申报标识获取有效申报类型
        /// </summary>
        /// <param name="breedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public Entity.XH_ValidDeclareType GetValidDeclareTypeByBreedClassValidID(int breedClassValidID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8419根据品种有效申报标识获取有效申报类型方法名称:GetValidDeclareTypeByBreedClassValidID(int breedClassValidID)参数是:" + breedClassValidID + "时间是:" + DateTime.Now);
                XH_ValidDeclareTypeBLL xH_ValidDeclareTypeBLL = new XH_ValidDeclareTypeBLL();
                List<Entity.XH_ValidDeclareType> xH_ValidDeclareTypeList =
                    xH_ValidDeclareTypeBLL.GetListArray(string.Format("BreedClassValidID={0}", breedClassValidID));
                if (xH_ValidDeclareTypeList.Count > 0)
                {
                    Entity.XH_ValidDeclareType xH_ValidDeclareType = xH_ValidDeclareTypeList[0];
                    if (xH_ValidDeclareType != null)
                    {
                        return xH_ValidDeclareType;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8419";
                string errMsg = "根据品种有效申报标识获取有效申报类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的有效申报取值
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_ValidDeclareValue> GetAllValidDeclareValue()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8420获取所有的有效申报取值方法名称:GetAllValidDeclareValue()" + DateTime.Now);
                XH_ValidDeclareValueBLL xH_ValidDeclareValueBLL = new XH_ValidDeclareValueBLL();
                return xH_ValidDeclareValueBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8420";
                string errMsg = "获取所有的有效申报取值失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据有效申报取值标识获取有效申报取值
        /// </summary>
        /// <param name="validDeclareValueID">有效申报取值标识</param>
        /// <returns></returns>
        public Entity.XH_ValidDeclareValue GetValidDeclareValueByValidDeclareValueID(int validDeclareValueID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8421根据有效申报取值标识获取有效申报取值方法名称:GetValidDeclareValueByValidDeclareValueID(int validDeclareValueID)参数是:" + validDeclareValueID + "时间是:" + DateTime.Now);
                XH_ValidDeclareValueBLL xH_ValidDeclareValueBLL = new XH_ValidDeclareValueBLL();
                List<Entity.XH_ValidDeclareValue> xH_ValidDeclareValueList =
                    xH_ValidDeclareValueBLL.GetListArray(string.Format("ValidDeclareValueID={0}", validDeclareValueID));
                if (xH_ValidDeclareValueList.Count > 0)
                {
                    Entity.XH_ValidDeclareValue xH_ValidDeclareValue = xH_ValidDeclareValueList[0];
                    if (xH_ValidDeclareValue != null)
                    {
                        return xH_ValidDeclareValue;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8421";
                string errMsg = "根据有效申报取值标识获取有效申报取值失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种有效申报标识获取有效申报取值
        /// </summary>
        /// <param name="breedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public List<Entity.XH_ValidDeclareValue> GetValidDeclareValueByBreedClassValidID(int breedClassValidID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8422根据有效申报取值标识获取有效申报取值方法名称:GetValidDeclareValueByBreedClassValidID(int breedClassValidID)参数是:" + breedClassValidID + "时间是:" + DateTime.Now);
                XH_ValidDeclareValueBLL xH_ValidDeclareValueBLL = new XH_ValidDeclareValueBLL();
                return xH_ValidDeclareValueBLL.GetListArray(string.Format("BreedClassValidID={0}", breedClassValidID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8422";
                string errMsg = "根据品种有效申报标识获取有效申报取值失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion
    }
}