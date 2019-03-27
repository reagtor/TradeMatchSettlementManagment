using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL.WCFService_Out.interfases;
using ManagementCenter.Model;
using Entity = ManagementCenter.Model;
using ManagementCenter.BLL;

namespace ManagementCenter.BLL.WCFService_Out
{
    /// <summary>
    /// 描述：公共参数服务 
    /// 作者：刘书伟
    /// 日期：2008-11-20 修改日期：2009-10-22
    /// 描述:添加Debug日志
    /// 修改作者：刘书伟
    /// 日期：2010-05-10
    /// </summary>
    // 错误编码 8200-8399
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CommonParaService : ICommonPara
    {
        #region ICommonPara 成员

        /// <summary>
        /// 获取所有的交易所类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_BourseType> GetAllBourseType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8200获取所有的交易所类型方法名称:GetAllBourseType()" + DateTime.Now);
                CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
                return cM_BourseTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8200";
                string errMsg = "获取所有的交易所类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据交易所类型标识返回交易所类型
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <returns></returns>
        public Entity.CM_BourseType GetBourseTypeByBourseTypeID(int bourseTypeID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8201根据交易所类型标识返回交易所类型方法名称:GetBourseTypeByBourseTypeID(int bourseTypeID)参数是:" + bourseTypeID + "时间是:" + DateTime.Now);
                CM_BourseTypeBLL cM_BourseTypeBLL = new CM_BourseTypeBLL();
                List<Entity.CM_BourseType> cM_BourseTypeList =
                cM_BourseTypeBLL.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID));
                if (cM_BourseTypeList.Count > 0)
                {
                    Entity.CM_BourseType cM_BourseType = cM_BourseTypeList[0];
                    if (cM_BourseType != null)
                    {
                        return cM_BourseType;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8201";
                string errMsg = "根据交易所类型标识返回交易所类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的交易商品品种
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_BreedClass> GetAllBreedClass()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8202获取所有的交易商品品种方法名称:GetAllBreedClass()" + DateTime.Now);
                CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
                return cM_BreedClassBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8202";
                string errMsg = "获取所有的交易商品品种失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回交易商品品种
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.CM_BreedClass GetBreedClassByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8203根据品种标识返回交易商品品种方法名称:GetBreedClassByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
                List<Entity.CM_BreedClass> cM_BreedClassList =
                   cM_BreedClassBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (cM_BreedClassList.Count > 0)
                {
                    Entity.CM_BreedClass cM_BreedClass = cM_BreedClassList[0];
                    if (cM_BreedClass != null)
                    {
                        return cM_BreedClass;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8203";
                string errMsg = "根据品种标识返回交易商品品种失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 根据交易所类型标识返回交易商品品种
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <returns></returns>
        public List<Entity.CM_BreedClass> GetBreedClassByBourseTypeID(int bourseTypeID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8204根据交易所类型标识返回交易商品品种方法名称:GetBreedClassByBourseTypeID(int bourseTypeID)" + DateTime.Now);
                CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
                return cM_BreedClassBLL.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8204";
                string errMsg = "根据交易所类型标识返回交易商品品种列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        ///  获取所有交易商品品种类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_BreedClassType> GetAllBreedClassType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8205获取所有交易商品品种类型方法名称:GetAllBreedClassType()" + DateTime.Now);
                CM_BreedClassTypeBLL cM_BourseTypeBLL = new CM_BreedClassTypeBLL();
                return cM_BourseTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8205";
                string errMsg = "获取所有交易商品品种类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的交易商品
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_Commodity> GetAllCommodity()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8206获取所有的交易商品方法名称:GetAllCommodity()" + DateTime.Now);
                CM_CommodityBLL cM_Commodity = new CM_CommodityBLL();
                return cM_Commodity.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8206";
                string errMsg = "获取所有的交易商品失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据商品代码返回交易商品
        /// </summary>
        /// <param name="commodityCode">商品代码</param>
        /// <returns></returns>
        public Entity.CM_Commodity GetCommodityByCommodityCode(string commodityCode)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8207根据商品代码返回交易商品方法名称:GetCommodityByCommodityCode(string commodityCode)参数是:" + commodityCode + "时间是:" + DateTime.Now);
                CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
                List<Entity.CM_Commodity> cM_CommodityList =
                     cM_CommodityBLL.GetListArray(string.Format("CommodityCode='{0}'", commodityCode));
                if (cM_CommodityList.Count > 0)
                {
                    Entity.CM_Commodity cM_Commodity = cM_CommodityList[0];
                    if (cM_Commodity != null)
                    {
                        return cM_Commodity;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8207";
                string errMsg = "根据商品代码返回交易商品实体失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回交易商品
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<Entity.CM_Commodity> GetCommodityByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8208根据品种标识返回交易商品方法名称:GetCommodityByBreedClassID(int breedClassID)参数是：" + breedClassID + "时间是:" + DateTime.Now);
                CM_CommodityBLL cM_CommodityBLL = new CM_CommodityBLL();
                return cM_CommodityBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8208";
                string errMsg = "根据品种标识返回交易商品列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的可交易商品_熔断
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_CommodityFuse> GetAllCommodityFuse()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8209获取所有的可交易商品_熔断方法名称:GetAllCommodityFuse()" + DateTime.Now);
                CM_CommodityFuseBLL cM_CommodityFuse = new CM_CommodityFuseBLL();
                return cM_CommodityFuse.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8209";
                string errMsg = "获取所有的可交易商品_熔断失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据商品代码返回可交易商品_熔断
        /// </summary>
        /// <param name="commodityCode">商品代码</param>
        /// <returns></returns>
        public Entity.CM_CommodityFuse GetCommodityFuseByCommodityCode(string commodityCode)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8210根据商品代码返回可交易商品_熔断方法名称:GetCommodityFuseByCommodityCode(string commodityCode)" + DateTime.Now);
                CM_CommodityFuseBLL cM_CommodityFuseBLL = new CM_CommodityFuseBLL();
                List<Entity.CM_CommodityFuse> cM_CommodityFuseList =
                    cM_CommodityFuseBLL.GetListArray(string.Format("CommodityCode='{0}'", commodityCode));
                if (cM_CommodityFuseList.Count > 0)
                {
                    Entity.CM_CommodityFuse cM_CommodityFuse = cM_CommodityFuseList[0];
                    if (cM_CommodityFuse != null)
                    {
                        return cM_CommodityFuse;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8210";
                string errMsg = "根据商品代码返回可交易商品_熔断失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的交易货币类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_CurrencyType> GetAllCurrencyType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8211获取所有的交易货币类型方法名称:GetAllCurrencyType()" + DateTime.Now);
                CM_CurrencyTypeBLL cM_CurrencyType = new CM_CurrencyTypeBLL();
                return cM_CurrencyType.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8211";
                string errMsg = "获取所有的交易货币类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的股票分红记录_现金
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_StockMelonCash> GetAllStockMelonCash()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8216获取所有的股票分红记录_现金方法名称:GetAllStockMelonCash()" + DateTime.Now);
                CM_StockMelonCashBLL cM_StockMelonCash = new CM_StockMelonCashBLL();
                return
                    cM_StockMelonCash.GetListArray(string.Format("(StockRightRegisterDate>='{0}' AND StockRightRegisterDate<'{1}') OR (StockRightLogoutDatumDate>='{1}' AND StockRightLogoutDatumDate<'{2}')",
                                                                 System.DateTime.Now.ToShortDateString(),
                                                                 System.DateTime.Now.AddDays(1).ToShortDateString(),
                                                                 System.DateTime.Now.AddDays(2).ToShortDateString()));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8216";
                string errMsg = "获取所有的股票分红记录_现金失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的股票分红记录_股票
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_StockMelonStock> GetAllStockMelonStock()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8217获取所有的股票分红记录_股票方法名称:GetAllStockMelonStock()" + DateTime.Now);
                CM_StockMelonStockBLL cM_StockMelonStock = new CM_StockMelonStockBLL();
                return
                    cM_StockMelonStock.GetListArray(string.Format("(StockRightRegisterDate>='{0}' AND StockRightRegisterDate<'{1}') OR (StockRightLogoutDatumDate>='{1}' AND  StockRightLogoutDatumDate<'{2}')",
                                                                  System.DateTime.Now.ToShortDateString(),
                                                                  System.DateTime.Now.AddDays(1).ToShortDateString(),
                                                                  System.DateTime.Now.AddDays(2).ToShortDateString()));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8217";
                string errMsg = "获取所有的股票分红记录_股票失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }



        /// <summary>
        /// 获取所有的交易所类型_交易时间
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_TradeTime> GetAllTradeTime()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8218获取所有的交易所类型_交易时间方法名称:GetAllTradeTime()" + DateTime.Now);
                CM_TradeTimeBLL cM_TradeTime = new CM_TradeTimeBLL();
                return cM_TradeTime.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8218";
                string errMsg = "获取所有的交易所类型_交易时间失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据交易所类型标识返回交易时间
        /// </summary>
        /// <param name="bourseTypeID">交易所类型</param>
        /// <returns></returns>
        public List<Entity.CM_TradeTime> GetTradeTimeByBourseTypeID(int bourseTypeID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8219根据交易所类型标识返回交易时间方法名称:GetTradeTimeByBourseTypeID(int bourseTypeID)" + DateTime.Now);
                CM_TradeTimeBLL cM_TradeTime = new CM_TradeTimeBLL();
                return cM_TradeTime.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8219";
                string errMsg = "根据交易所类型标识返回交易时间失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的交易方向
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_TradeWay> GetAllTradeWay()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8220获取所有的交易方向方法名称:GetAllTradeWay()" + DateTime.Now);
                CM_TradeWayBLL cM_TradeWay = new CM_TradeWayBLL();
                return cM_TradeWay.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8220";
                string errMsg = "获取所有的交易方向失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的现货_品种_交易单位换算
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_UnitConversion> GetAllUnitConversion()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8221获取所有的现货_品种_交易单位换算方法名称:GetAllUnitConversion()" + DateTime.Now);
                CM_UnitConversionBLL cM_UnitConversion = new CM_UnitConversionBLL();
                return cM_UnitConversion.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8221";
                string errMsg = "获取所有的现货_品种_交易单位换算失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识获取现货_品种_交易单位换算
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<Entity.CM_UnitConversion> GetUnitConversionByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8222根据品种标识获取现货_品种_交易单位换算方法名称:GetUnitConversionByBreedClassID(int breedClassID)" + DateTime.Now);
                CM_UnitConversionBLL cM_UnitConversion = new CM_UnitConversionBLL();
                return cM_UnitConversion.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8222";
                string errMsg = "根据品种标识获取现货_品种_交易单位换算失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的单位
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_Units> GetAllUnits()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8223获取所有的单位方法名称:GetAllUnits()" + DateTime.Now);
                CM_UnitsBLL cM_Units = new CM_UnitsBLL();
                return cM_Units.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8223";
                string errMsg = "获取所有的单位失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的交易规则_取值类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_ValueType> GetAllValueType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8224获取所有的交易规则_取值类型方法名称:GetAllValueType()" + DateTime.Now);
                CM_ValueTypeBLL cM_ValueType = new CM_ValueTypeBLL();
                return cM_ValueType.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8224";
                string errMsg = "获取所有的交易规则_取值类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取柜台列表
        /// </summary>
        /// <returns></returns>   
        public List<Entity.CT_Counter> GetAllCounter()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8225获取柜台列表方法名称:GetAllCounter()" + DateTime.Now);
                CT_CounterBLL CounterBLL = new CT_CounterBLL();
                return CounterBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8225";
                string errMsg = "获取柜台列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取撮合中心列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.RC_MatchCenter> GetAllMatchCenter()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8226获取撮合中心列表方法名称:GetAllMatchCenter()" + DateTime.Now);
                RC_MatchCenterBLL MatchCenterBLL = new RC_MatchCenterBLL();
                return MatchCenterBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8226";
                string errMsg = "获取撮合中心列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据IP和端口获取撮合中心
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public Entity.RC_MatchCenter GetMatchCenterByIpAndPort(string ip, int port)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8227根据IP和端口获取撮合中心方法名称:GetMatchCenterByIpAndPort(string ip, int port)" + DateTime.Now);
                RC_MatchCenterBLL MatchCenterBLL = new RC_MatchCenterBLL();
                List<Entity.RC_MatchCenter> rC_MatchCenterList =
                    MatchCenterBLL.GetListArray(string.Format("IP='{0}' AND Port={1}", ip, port));
                if (rC_MatchCenterList.Count > 0)
                {
                    Entity.RC_MatchCenter rC_MatchCenter = rC_MatchCenterList[0];
                    if (rC_MatchCenter != null)
                    {
                        return rC_MatchCenter;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                string errCode = "GL-8227";
                string errMsg = "根据IP和端口获取撮合中心失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取撮合机列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.RC_MatchMachine> GetAllMatchMachine()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8228获取撮合机列表方法名称:GetAllMatchMachine()" + DateTime.Now);
                RC_MatchMachineBLL RC_MatchMachine = new RC_MatchMachineBLL();
                return RC_MatchMachine.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8228";
                string errMsg = "获取撮合机列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据撮合中心获取撮合机列表
        /// </summary>
        /// <param name="MatchCenterID"></param>
        /// <returns></returns>
        public List<Entity.RC_MatchMachine> GetAllMatchMachineByMatchCenter(int MatchCenterID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8229根据撮合中心获取撮合机列表方法名称:GetAllMatchMachineByMatchCenter(int MatchCenterID)" + DateTime.Now);
                RC_MatchMachineBLL RC_MatchMachine = new RC_MatchMachineBLL();
                return RC_MatchMachine.GetListArray(string.Format("MatchCenterID={0}", MatchCenterID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8229";
                string errMsg = "根据撮合中心获取撮合机列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据撮合机得到所属的交易所类型和所属的撮合中心-即撮合机实体
        /// </summary>
        /// <returns></returns>
        public Entity.RC_MatchMachine GetMatchMachine(int MatchMachineID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8230根据撮合机得到所属的交易所类型和所属的撮合中心-即撮合机实体方法名称:GetMatchMachine(int MatchMachineID)" + DateTime.Now);
                RC_MatchMachineBLL RC_MatchMachine = new RC_MatchMachineBLL();
                List<Entity.RC_MatchMachine> rC_MatchMachineList =
                    RC_MatchMachine.GetListArray(string.Format("MatchMachineID={0}", MatchMachineID));
                if (rC_MatchMachineList.Count > 0)
                {
                    Entity.RC_MatchMachine rC_MatchMachine = rC_MatchMachineList[0];
                    if (rC_MatchMachine != null)
                    {
                        return rC_MatchMachine;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8230";
                string errMsg = "根据撮合机得到所属的交易所类型和所属的撮合中心-即撮合机实体失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取撮合机—代码分配列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.RC_TradeCommodityAssign> GetAllTradeCommodityAssign()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8231获取撮合机—代码分配列表方法名称:GetAllTradeCommodityAssign()" + DateTime.Now);
                RC_TradeCommodityAssignBLL RC_TradeCommodityAssign = new RC_TradeCommodityAssignBLL();
                return RC_TradeCommodityAssign.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8231";
                string errMsg = "获取撮合机—代码分配列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取一个撮合机分配的代码列表
        /// </summary>
        /// <param name="MatchMachineID"></param>
        /// <returns></returns>
        public List<Entity.RC_TradeCommodityAssign> GetCommodityAssignByMachineID(int MatchMachineID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8232获取一个撮合机分配的代码列表方法名称:GetCommodityAssignByMachineID(int MatchMachineID)" + DateTime.Now);
                RC_TradeCommodityAssignBLL RC_TradeCommodityAssign = new RC_TradeCommodityAssignBLL();
                return RC_TradeCommodityAssign.GetListArray(string.Format("MatchMachineID={0}", MatchMachineID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8232";
                string errMsg = "获取一个撮合机分配的代码列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取代码所属的撮合机ID
        /// </summary>
        /// <returns></returns>
        public Entity.RC_TradeCommodityAssign GetTradeCommodityAssign(string CommodityCode)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8233获取代码所属的撮合机ID方法名称:GetTradeCommodityAssign(string CommodityCode)" + DateTime.Now);
                //根据代码返回其它商品代码（普通现货和股指期货）
                RC_TradeCommodityAssignBLL RC_TradeCommodityAssignBLL = new RC_TradeCommodityAssignBLL();
                List<Entity.RC_TradeCommodityAssign> rC_TradeCommodityAssignList =
                    RC_TradeCommodityAssignBLL.GetListArray(string.Format("CommodityCode='{0}'", CommodityCode));
                if (rC_TradeCommodityAssignList.Count > 0)
                {
                    Entity.RC_TradeCommodityAssign rC_TradeCommodityAssign = rC_TradeCommodityAssignList[0];
                    //rC_TradeCommodityAssign = rC_TradeCommodityAssignList[0];
                    if (rC_TradeCommodityAssign != null)
                    {
                        return rC_TradeCommodityAssign;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8233";
                string errMsg = "获取代码所属的撮合机ID失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取代码所属的撮合机实体
        /// </summary>
        /// <returns></returns>
        public Entity.RC_MatchMachine GetMatchMachinebyCommodity(string CommodityCode)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8234获取代码所属的撮合机实体方法名称:GetMatchMachinebyCommodity(string CommodityCode) 参数：" + CommodityCode + "  " + DateTime.Now);
                //根据代码返回其它商品代码所属的撮合机实体（普通现货和股指期货）
                RC_TradeCommodityAssignBLL RC_TradeCommodityAssignBLL = new RC_TradeCommodityAssignBLL();
                List<Entity.RC_TradeCommodityAssign> rC_TradeCommodityAssignList =
                    RC_TradeCommodityAssignBLL.GetListArray(string.Format("CommodityCode='{0}'", CommodityCode));
                if (rC_TradeCommodityAssignList.Count > 0)
                {
                    Entity.RC_TradeCommodityAssign rC_TradeCommodityAssign = rC_TradeCommodityAssignList[0];
                    if (rC_TradeCommodityAssign != null)
                    {
                        return GetMatchMachine(rC_TradeCommodityAssign.MatchMachineID);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8234";
                string errMsg = "获取代码所属的撮合机实体失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有帐号类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.UM_AccountType> GetALLAccountType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8235获取所有帐号类型方法名称:GetALLAccountType()" + DateTime.Now);
                UM_AccountTypeBLL UM_AccountType = new UM_AccountTypeBLL();
                return UM_AccountType.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8235";
                string errMsg = "获取所有帐号类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的非交易日期
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_NotTradeDate> GetAllNotTradeDate()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8236获取所有的非交易日期方法名称:GetAllNotTradeDate()" + DateTime.Now);
                CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
                return cM_NotTradeDateBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8236";
                string errMsg = "获取所有的非交易日期失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据交易所类型标识返回非交易日期
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <returns></returns>
        public List<Entity.CM_NotTradeDate> GetNotTradeDateByBourseTypeID(int bourseTypeID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8237根据交易所类型标识返回非交易日期方法名称:GetNotTradeDateByBourseTypeID(int bourseTypeID)参数是:" + bourseTypeID + "时间是:" + DateTime.Now);
                CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
                return cM_NotTradeDateBLL.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8237";
                string errMsg = "根据交易所类型标识返回非交易日期失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的熔断_时间段标识
        /// </summary>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_FuseTimesection> GetAllFuseTimesection()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8238获取所有的熔断_时间段标识方法名称:GetAllFuseTimesection()" + DateTime.Now);
                CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
                return cM_FuseTimesectionBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8238";
                string errMsg = "获取所有的熔断_时间段标识失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据商品代码返回熔断_时间段标识
        /// </summary>
        /// <param name="commodityCode">商品代码</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_FuseTimesection> GetFuseTimesectionByCommodityCode(string commodityCode)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8239根据商品代码返回熔断_时间段标识方法名称:GetFuseTimesectionByCommodityCode(string commodityCode)" + DateTime.Now);
                CM_FuseTimesectionBLL cM_FuseTimesectionBLL = new CM_FuseTimesectionBLL();
                return cM_FuseTimesectionBLL.GetListArray(string.Format("CommodityCode='{0}'", commodityCode));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8239";
                string errMsg = "根据商品代码返回熔断_时间段标识失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有新股上市的商品代码
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_Commodity> GetNewCommodity()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8240获取所有新股上市的商品代码方法名称:GetNewCommodity()" + DateTime.Now);
                CM_CommodityBLL cM_Commodity = new CM_CommodityBLL();
                string strWhere = string.Format("MarketDate='{0}'", System.DateTime.Now.ToShortDateString());

                LogHelper.WriteDebug("获取新股的条件：" + strWhere);
                return cM_Commodity.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-8240";
                string errMsg = "获取所有新股上市的商品代码失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有增发上市的商品代码
        /// </summary>
        /// <returns></returns>
        public List<Entity.ZFInfo> GetZFCommodity()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8241获取所有增发上市的商品代码方法名称:GetZFCommodity()" + DateTime.Now);
                ZFInfoBLL ZFInfoBLL = new ZFInfoBLL();
                return ZFInfoBLL.GetListArray(string.Format("paydt='{0}'", System.DateTime.Now.ToString("yyyy-MM-dd")));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8241";
                string errMsg = "获取所有增发上市的商品代码失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #region  提供给前台的方法

        /// <summary>
        /// 获取所有商品代码
        /// </summary>
        /// <returns></returns>
        public List<Entity.CommonTable.OnstageCommodity> GetCommodityListArray()
        {
            try
            {
                CM_CommodityBLL cM_Commodity = new CM_CommodityBLL();
                return cM_Commodity.GetCommodityListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8242";
                string errMsg = "获取所有商品代码失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种找到币种
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public Entity.CM_CurrencyBreedClassType GetCurrencyByBreedClassID(int BreedClassID)
        {
            try
            {
                CM_CurrencyTypeBLL CurrencyTypeBLL = new CM_CurrencyTypeBLL();
                return CurrencyTypeBLL.GetCurrencyByBreedClassID(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8243";
                string errMsg = "根据品种找到币种失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有品种和币种的对应关系
        /// </summary>
        /// <returns></returns>
        public List<Entity.CM_CurrencyBreedClassType> GetListCurrencyBreedClass()
        {
            try
            {
                CM_CurrencyTypeBLL CurrencyTypeBLL = new CM_CurrencyTypeBLL();
                return CurrencyTypeBLL.GetListCurrencyBreedClass();
            }
            catch (Exception ex)
            {
                string errCode = "GL-8244";
                string errMsg = "获取所有品种和币种的对应关系失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取行业信息列表
        /// </summary>
        /// <returns>返回行业信息列表</returns>
        public List<ManagementCenter.Model.Profession> GetListProfessionArray()
        {
            try
            {
                Profession Profession = new Profession();
                return Profession.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8245";
                string errMsg = "获取行业信息列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #region add by 董鹏 2010-05-19

        /// <summary>
        /// 获取指定日期区间内(除权基准日)所有的股票分红记录_现金
        /// （开始或结束日期为空则取今天的日期）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>股票分红记录_现金</returns>
        public List<CM_StockMelonCash> GetStockMelonCashByDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
            {
                startDate = DateTime.Now;
            }
            if (endDate == null)
            {
                endDate = DateTime.Now;
            }
            try
            {
                LogHelper.WriteDebug("Debug-8216获取所有的股票分红记录_现金方法名称:GetStockMelonCashByDateRange()，开始日期=" + startDate.Value.ToShortDateString() + "，结束日期=" + endDate.Value.ToShortDateString());
                CM_StockMelonCashBLL cM_StockMelonCash = new CM_StockMelonCashBLL();
                return cM_StockMelonCash.GetListArray(string.Format("(StockRightLogoutDatumDate>='{0}' AND StockRightLogoutDatumDate<'{1}')",
                                                     startDate.Value.ToShortDateString(), endDate.Value.AddDays(1).ToShortDateString()));

            }
            catch (Exception ex)
            {
                string errCode = "GL-8216";
                string errMsg = "获取所有的股票分红记录_现金失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取指定日期区间内(除权基准日)所有的股票分红记录_股票
        /// （开始或结束日期为空则取今天的日期）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>股票分红记录_股票</returns>
        public List<CM_StockMelonStock> GetStockMelonStockByDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
            {
                startDate = DateTime.Now;
            }
            if (endDate == null)
            {
                endDate = DateTime.Now;
            }
            try
            {
                LogHelper.WriteDebug("Debug-8217获取所有的股票分红记录_股票方法名称:GetStockMelonStockByDateRange()，开始日期=" + startDate.Value.ToShortDateString() + "，结束日期=" + endDate.Value.ToShortDateString());
                CM_StockMelonStockBLL cM_StockMelonStock = new CM_StockMelonStockBLL();
                return cM_StockMelonStock.GetListArray(string.Format("(StockRightLogoutDatumDate>='{0}' AND  StockRightLogoutDatumDate<'{1}')",
                                                  startDate.Value.ToShortDateString(), endDate.Value.AddDays(1).ToShortDateString()));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8217";
                string errMsg = "获取所有的股票分红记录_股票失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 获取交易员的品种交易权限
        /// </summary>
        /// <returns></returns>
        public List<Entity.UM_DealerTradeBreedClass> TransactionRightTable(int UserID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8246获取交易员的品种交易权限方法名称:TransactionRightTable(int UserID)" + DateTime.Now);
                UM_DealerTradeBreedClassBLL DealerTradeBreedClassBLL = new UM_DealerTradeBreedClassBLL();
                return DealerTradeBreedClassBLL.GetBreedClassRightList(UserID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8246";
                string errMsg = "获取交易员的品种交易权限失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 根据交易所类型和日期判断是否为交易日
        /// </summary>
        /// <param name="bourseTypeID">交易所类型标识</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool JudgeIsTradeDateByBourseTypeID(int bourseTypeID, DateTime dt)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8247根据交易所类型和日期判断是否为交易日方法名称:JudgeIsTradeDateByBourseTypeID(int bourseTypeID, DateTime dt)" + DateTime.Now);
                CM_NotTradeDateBLL cM_NotTradeDateBLL = new CM_NotTradeDateBLL();
                List<Entity.CM_NotTradeDate> l = cM_NotTradeDateBLL.GetListArray(string.Format("BourseTypeID={0} and NotTradeDay='{1}'", bourseTypeID, dt));
                if (l == null || l.Count >= 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8247";
                string errMsg = "根据交易所类型和日期判断是否为交易日失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 获取所有的股票收盘价
        /// </summary>
        /// <returns></returns>
        public List<Entity.ClosePriceInfo> GetAllClosePriceInfo()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8248获取所有的股票收盘价方法名称:GetAllClosePriceInfo()" + DateTime.Now);
                ClosePriceInfoBLL closePriceInfoBLL = new ClosePriceInfoBLL();
                return closePriceInfoBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8248";
                string errMsg = "获取所有的股票收盘价失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种类型标识获取股票收盘价
        /// </summary>
        /// <param name="BreedClassTypeID">品种类型标识</param>
        /// <returns></returns>
        public List<Entity.ClosePriceInfo> GetAllClosePriceInfoByBreedClassTypeID(int BreedClassTypeID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8249根据品种类型标识获取股票收盘价方法名称:GetAllClosePriceInfoByBreedClassTypeID(int BreedClassTypeID)" + DateTime.Now);
                ClosePriceInfoBLL closePriceInfoBLL = new ClosePriceInfoBLL();
                return closePriceInfoBLL.GetListArray(string.Format("BreedClassTypeID={0}", BreedClassTypeID));

            }
            catch (Exception ex)
            {
                string errCode = "GL-8249";
                string errMsg = "根据品种类型标识获取股票收盘价失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


    }
}