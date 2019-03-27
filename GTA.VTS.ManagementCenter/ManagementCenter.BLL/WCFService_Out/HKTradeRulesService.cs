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
    /// 描述：港股交易规则对外服务
    /// 作者：刘书伟
    /// 日期：2009-10-22
    /// 描述:添加Debug日志
    /// 修改作者：刘书伟
    /// 日期：2010-05-10
    /// </summary>
    /// 错误编码 8600-8699 
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class HKTradeRulesService : IHKTradeRules
    {

        #region IHKTradeRules 成员

        /// <summary>
        /// 获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <returns></returns>
        public List<Entity.XH_MinVolumeOfBusiness> GetAllMinVolumeOfBusiness()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8600获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)方法名称:GetAllMinVolumeOfBusiness()" + DateTime.Now);
                XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
                return xH_MinVolumeOfBusinessBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8600";
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
                LogHelper.WriteDebug("Debug-8601根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)方法名称:GetMinVolumeOfBusinessByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                XH_MinVolumeOfBusinessBLL xH_MinVolumeOfBusinessBLL = new XH_MinVolumeOfBusinessBLL();
                return xH_MinVolumeOfBusinessBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8601";//"GL-8403";
                string errMsg = "根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)失败";
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
                LogHelper.WriteDebug("Debug-8602获取所有的现货_交易商品品种_持仓限制方法名称:GetAllSpotPosition()" + DateTime.Now);
                XH_SpotPositionBLL xH_SpotPositionBLL = new XH_SpotPositionBLL();
                return xH_SpotPositionBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8602";
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
                LogHelper.WriteDebug("Debug-8603根据品种标识返回现货_交易商品品种_持仓限制方法名称:GetSpotPositionByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
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
                string errCode = "GL-8603";
                string errMsg = "根据品种标识返回现货_交易商品品种_持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的港股交易商品
        /// </summary>
        /// <returns></returns>
        public List<Entity.HK_Commodity> GetAllHKCommodity()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8604获取所有的港股交易商品方法名称:GetAllHKCommodity()" + DateTime.Now);
                HK_CommodityBLL hK_CommodityBLL = new HK_CommodityBLL();
                return hK_CommodityBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8604";
                string errMsg = "获取所有的港股交易商品失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据港股商品代码返回交易商品
        /// </summary>
        /// <param name="hkcommodityCode">港股商品代码</param>
        /// <returns>返回港股代码实体</returns>
        public Entity.HK_Commodity GetHKCommodityByHKCommodityCode(string hkcommodityCode)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8605根据港股商品代码返回交易商品方法名称:GetHKCommodityByHKCommodityCode(string hkcommodityCode)参数是:" + hkcommodityCode + "时间是:" + DateTime.Now);
                HK_CommodityBLL hK_CommodityBLL = new HK_CommodityBLL();
                List<Entity.HK_Commodity> hK_CommodityList =
                     hK_CommodityBLL.GetListArray(string.Format("HKCommodityCode='{0}'", hkcommodityCode));
                if (hK_CommodityList.Count > 0)
                {
                    Entity.HK_Commodity hK_Commodity = hK_CommodityList[0];
                    if (hK_Commodity != null)
                    {
                        return hK_Commodity;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8605";// "GL-8207";
                string errMsg = "根据商品代码返回港股交易商品实体失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回港股交易商品
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<Entity.HK_Commodity> GetHKCommodityByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8606根据品种标识返回港股交易商品方法名称:GetHKCommodityByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                HK_CommodityBLL hK_CommodityBLL = new HK_CommodityBLL();
                return hK_CommodityBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8606";// "GL-8208";
                string errMsg = "根据品种标识返回港股交易商品列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有港股新股上市的商品代码
        /// </summary>
        /// <returns></returns>
        public List<Entity.HK_Commodity> GetNewHKCommodity()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8607获取所有港股新股上市的商品代码方法名称:GetNewHKCommodity()" + DateTime.Now);
                HK_CommodityBLL hK_CommodityBLL = new HK_CommodityBLL();
                return
                    hK_CommodityBLL.GetListArray(string.Format("MarketDate='{0}'", System.DateTime.Now.ToShortDateString()));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8607";//"GL-8240";
                string errMsg = "获取所有港股新股上市的商品代码失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的港股_交易费用
        /// </summary>
        /// <returns></returns>
        public List<Entity.HK_SpotCosts> GetAllHKSpotCosts()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8608获取所有的港股_交易费用方法名称:GetAllHKSpotCosts()" + DateTime.Now);
                HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
                return hK_SpotCostsBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8608";// "GL-8404";
                string errMsg = "获取所有的港股_交易费用失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回港股_交易费用
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.HK_SpotCosts GetHKSpotCostsByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8609根据品种标识返回港股_交易费用方法名称:GetHKSpotCostsByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                HK_SpotCostsBLL hK_SpotCostsBLL = new HK_SpotCostsBLL();
                List<Entity.HK_SpotCosts> hK_SpotCostsList =
                    hK_SpotCostsBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (hK_SpotCostsList.Count > 0)
                {
                    Entity.HK_SpotCosts hK_SpotCosts = hK_SpotCostsList[0];
                    if (hK_SpotCosts != null)
                    {
                        return hK_SpotCosts;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8609";// "GL-8405";
                string errMsg = "根据品种标识返回港股_交易费用失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的港股_品种_交易规则
        /// </summary>
        /// <returns></returns>
        public List<Entity.HK_SpotTradeRules> GetAllHKSpotTradeRules()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8610获取所有的港股_品种_交易规则方法名称:GetAllHKSpotTradeRules()" + DateTime.Now);
                HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
                return hK_SpotTradeRulesBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8610";// "GL-8411";
                string errMsg = "获取所有的港股_品种_交易规则失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回港股_品种_交易规则
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.HK_SpotTradeRules GetHKSpotTradeRulesByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8611根据品种标识返回港股_品种_交易规则方法名称:GetHKSpotTradeRulesByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                HK_SpotTradeRulesBLL hK_SpotTradeRulesBLL = new HK_SpotTradeRulesBLL();
                var list = hK_SpotTradeRulesBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (list.Count > 0)
                {
                    Entity.HK_SpotTradeRules hK_SpotTradeRules = list[0];
                    if (hK_SpotTradeRules != null)
                    {
                        return hK_SpotTradeRules;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8611";// "GL-8412";
                string errMsg = "根据品种标识返回港股_品种_交易规则失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的港股交易规则_最小变动价位范围值
        /// </summary>
        /// <returns></returns>
        public List<Entity.HK_MinPriceFieldRange> GetAllHKMinPriceFieldRange()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8612获取所有的港股交易规则_最小变动价位范围值方法名称:GetAllHKMinPriceFieldRange()" + DateTime.Now);
                HK_MinPriceFieldRangeBLL hK_MinPriceFieldRangeBLL = new HK_MinPriceFieldRangeBLL();
                return hK_MinPriceFieldRangeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8612";
                string errMsg = "获取所有的港股交易规则_最小变动价位范围值失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion


        #region  提供给前台的方法

        /// <summary>
        ///  提供前台获取港股商品代码列表
        /// </summary>
        /// <returns></returns>
        public List<ManagementCenter.Model.OnstageHK_Commodity> GetHKCommodityListArray()
        {
            try
            {
                HK_CommodityBLL hK_Commodity = new HK_CommodityBLL();
                return hK_Commodity.GetHKCommodityListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8613";
                string errMsg = "获取所有港股商品代码失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 提供前台获取港股行业列表
        /// </summary>
        /// <returns></returns>
        public List<ManagementCenter.Model.HKProfessionInfo> GetListHKProfessionInfoArray()
        {
            try
            {
                HKProfessionInfoBLL hKProfessionInfoBLL = new HKProfessionInfoBLL();
                return hKProfessionInfoBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8614";
                string errMsg = "获取港股行业信息列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion



    }
}
