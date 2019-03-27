#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.SpotTradingDevolveService;

#endregion

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 对现货交易规则对外接口的缓存包装，错误编码8100-8199
    /// 作者：宋涛
    /// 日期：2008-12-05
    /// </summary>
    public class SpotTradeRulesProxy
    {
        #region ISpotTradeRules Fields

        private static SpotTradeRulesProxy instance = new SpotTradeRulesProxy();

        //private WCFCacheObjectWithGetAllNoKey<XH_MinChangePriceValue> minChangeObj;

        //private WCFCacheObjectWithGetKey<int, IList<XH_MinChangePriceValue>> minChangeObjByBreedClassID;

        private WCFCacheObjectWithGetKey<int, IList<XH_MinVolumeOfBusiness>> minVolumeByBreedClassIDObj;

        private WCFCacheObjectWithGetAllNoKey<XH_MinVolumeOfBusiness> minVolumeObj;

        //private WCFCacheObjectWithGetKey<int, IList<XH_RightHightLowPrices>> rightHighLowPricesByHightLowIDObj;

        //private WCFCacheObjectWithGetAllNoKey<XH_RightHightLowPrices> rightHightLowPricesObj;

        private WCFCacheObject<int, XH_SpotCosts> spotCostsObj;

        private WCFCacheObjectWithGetAll<int, XH_SpotHighLowControlType> spotHighLowControlTypeObj;

        private WCFCacheObjectWithGetKey<int, IList<XH_SpotHighLowValue>> spotHighLowValueByBreedClassHighLowIDObj;

        private WCFCacheObject<int, XH_SpotHighLowValue> spotHighLowValueObj;

        private WCFCacheObject<int, XH_SpotPosition> spotPositionObj;

        //private WCFCacheObjectWithGetAllNoKey<XH_SpotRangeCost> spotRangCostObj;
        //private WCFCacheObjectWithGetKey<int, IList<XH_SpotRangeCost>> spotRangeCostByBreedClassID;

        private WCFCacheObject<int, XH_SpotTradeRules> spotTradeRulesObj;

        private WCFCacheObject<int, XH_ValidDeclareType> validDeclareTypeObj;

        #endregion

        #region SyncRoot

        #endregion

        private SpotTradeRulesProxy()
        {
            //minChangeObj =
            //    new WCFCacheObjectWithGetAllNoKey<XH_MinChangePriceValue>(GetAllMinChangePriceValueFromWCF);

            //minChangeObjByBreedClassID =
            //    new WCFCacheObjectWithGetKey<int, IList<XH_MinChangePriceValue>>(
            //        GetMinChangePriceValueByBreedClassIDFromWCF);

            minVolumeByBreedClassIDObj =
                new WCFCacheObjectWithGetKey<int, IList<XH_MinVolumeOfBusiness>>(
                    GetMinVolumeOfBusinessByBreedClassIDFromWCF);

            minVolumeObj =
                new WCFCacheObjectWithGetAllNoKey<XH_MinVolumeOfBusiness>(GetAllMinVolumeOfBusinessFromWCF);

            //rightHighLowPricesByHightLowIDObj =
            //    new WCFCacheObjectWithGetKey<int, IList<XH_RightHightLowPrices>>(
            //        GetRightHighLowPricesByHightLowIDFromWCF);

            //rightHightLowPricesObj =
            //    new WCFCacheObjectWithGetAllNoKey<XH_RightHightLowPrices>(GetAllRightHighLowPricesFromWCF);

            spotCostsObj =
                new WCFCacheObject<int, XH_SpotCosts>(GetAllSpotCostsFromWCF, GetSpotCostsByBreedClassIDFromWCF,
                                                      val => val.BreedClassID);

            spotHighLowControlTypeObj =
                new WCFCacheObjectWithGetAll<int, XH_SpotHighLowControlType>(GetAllSpotHighLowControlTypeFromWCF,
                                                                             val => val.BreedClassHighLowID);

            spotHighLowValueByBreedClassHighLowIDObj =
                new WCFCacheObjectWithGetKey<int, IList<XH_SpotHighLowValue>>(
                    GetSpotHighLowValueByBreedClassHighLowIDFromWCF);

            spotHighLowValueObj =
                new WCFCacheObject<int, XH_SpotHighLowValue>(GetAllSpotHighLowValueFromWCF,
                                                             GetSpotHighLowByHightLowValueIDFromWCF,
                                                             val => val.HightLowValueID);

            spotPositionObj =
                new WCFCacheObject<int, XH_SpotPosition>(GetAllSpotPositionFromWCF, GetSpotPositionByBreedClassIDFromWCF,
                                                         Val => Val.BreedClassID);

            //spotRangCostObj =
            //    new WCFCacheObjectWithGetAllNoKey<XH_SpotRangeCost>(GetAllSpotRangeCostFromWCF);

            //spotRangeCostByBreedClassID =
            //    new WCFCacheObjectWithGetKey<int, IList<XH_SpotRangeCost>>(GetSpotRangeCostByBreedClassIDFromWCF);

            spotTradeRulesObj =
                new WCFCacheObject<int, XH_SpotTradeRules>(GetAllSpotTradeRulesFromWCF,
                                                           GetSpotTradeRulesByBreedClassIDFromWCF,
                                                           val => val.BreedClassID);

            validDeclareTypeObj =
                new WCFCacheObject<int, XH_ValidDeclareType>(GetAllValidDeclareTypeFromWCF,
                                                             GetValidDeclareTypeByBreedClassValidIDFromWCF,
                                                             val => val.BreedClassValidID);
        }

        public static SpotTradeRulesProxy GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// 进行预加载
        /// </summary>
        public void Initialize()
        {
            GetAllSpotCosts();
            GetAllSpotTradeRules();
            GetAllSpotPosition();
            //GetAllSpotRangeCost();
            GetAllSpotHighLowControlType();
            GetAllSpotHighLowValue();
            //GetAllRightHighLowPrices();
            GetAllValidDeclareType();
            //GetAllMinChangePriceValue();
            GetAllMinVolumeOfBusiness();
        }

        public void Reset()
        {
            //minChangeObj.Reset();
            //minChangeObjByBreedClassID.Reset();
            minVolumeByBreedClassIDObj.Reset();
            minVolumeObj.Reset();
            //rightHighLowPricesByHightLowIDObj.Reset();
            //rightHightLowPricesObj.Reset();
            spotCostsObj.Reset();
            spotHighLowControlTypeObj.Reset();
            spotHighLowValueByBreedClassHighLowIDObj.Reset();
            spotHighLowValueObj.Reset();
            spotPositionObj.Reset();
            //spotRangCostObj.Reset();
            //spotRangeCostByBreedClassID.Reset();
            spotTradeRulesObj.Reset();
            validDeclareTypeObj.Reset();
        }

        private SpotTradeRulesClient GetClient()
        {
            SpotTradeRulesClient client;
            try
            {
                client = new SpotTradeRulesClient();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8100";
                string errMsg = "无法获取管理中心提供的服务[IStockTradeRules]。";
                throw new VTException(errCode, errMsg, ex);
            }

            return client;
        }

        #region ISportTradeRules

        #region 交易规则_最小变动价位_范围_值
        ///// <summary>
        ///// 获取所有的交易规则_最小变动价位_范围_值
        ///// </summary>
        ///// <returns></returns>
        //public IList<XH_MinChangePriceValue> GetAllMinChangePriceValue()
        //{
        //    return GetAllMinChangePriceValue(false);
        //}

        ///// <summary>
        ///// 获取所有的交易规则_最小变动价位_范围_值
        ///// </summary>
        ///// <returns></returns>
        //public IList<XH_MinChangePriceValue> GetAllMinChangePriceValue(bool reLoad)
        //{
        //    return minChangeObj.GetAll(reLoad);
        //}

        //private IList<XH_MinChangePriceValue> GetAllMinChangePriceValueFromWCF()
        //{
        //    try
        //    {
        //        using (SpotTradeRulesClient client = GetClient())
        //            return client.GetAllMinChangePriceValue();
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8101";
        //        string errMsg = "无法从管理中心获取交易规则最小变动价位范围值列表。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}

        //private IList<XH_MinChangePriceValue> GetMinChangePriceValueByBreedClassIDFromWCF(int id)
        //{
        //    try
        //    {
        //        using (SpotTradeRulesClient client = GetClient())
        //            return client.GetMinChangePriceValueByBreedClassID(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8102";
        //        string errMsg = "无法根据交易商品品种编码从管理中心获取交易规则最小变动价位范围值列表。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}

        //public IList<XH_MinChangePriceValue> GetMinChangePriceValueByBreedClassID(int id)
        //{
        //    return minChangeObjByBreedClassID.GetByKey(id);
        //}
        #endregion

        #region 交易方向_交易单位_交易量
        /// <summary>
        /// 获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <returns></returns>
        public IList<XH_MinVolumeOfBusiness> GetAllMinVolumeOfBusiness()
        {
            return GetAllMinVolumeOfBusiness(false);
        }

        private IList<XH_MinVolumeOfBusiness> GetAllMinVolumeOfBusiness(bool reLoad)
        {
            return minVolumeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <returns></returns>
        private IList<XH_MinVolumeOfBusiness> GetAllMinVolumeOfBusinessFromWCF()
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetAllMinVolumeOfBusiness();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8103";
                string errMsg = "无法从管理中心获取交易规则最小交易单位列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<XH_MinVolumeOfBusiness> GetMinVolumeOfBusinessByBreedClassID(int id)
        {
            return minVolumeByBreedClassIDObj.GetByKey(id);
        }

        /// <summary>
        /// 根据品种标识返回交易规则_交易方向_交易单位_交易量(最小交易单位)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IList<XH_MinVolumeOfBusiness> GetMinVolumeOfBusinessByBreedClassIDFromWCF(int id)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetMinVolumeOfBusinessByBreedClassID(id);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8104";
                string errMsg = "无法根据交易商品品种编码从管理中心获取交易规则最小交易单位列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }
        #endregion

        #region 品种_现货_交易费用

        public IList<XH_SpotCosts> GetAllSpotCosts()
        {
            return GetAllSpotCosts(false);
        }

        /// <summary>
        /// 获取所有的品种_现货_交易费用
        /// </summary>
        /// <param name="reLoad"></param>
        /// <returns></returns>
        public IList<XH_SpotCosts> GetAllSpotCosts(bool reLoad)
        {
            return spotCostsObj.GetAll(reLoad);
        }

        private IList<XH_SpotCosts> GetAllSpotCostsFromWCF()
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetAllSpotCosts();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8105";
                string errMsg = "无法从管理中心获取现货交易费用列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        private XH_SpotCosts GetSpotCostsByBreedClassIDFromWCF(int breedClassID)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetSpotCostsByBreedClassID(breedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8106";
                string errMsg = "无法从管理中心获取现货交易费用。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public XH_SpotCosts GetSpotCostsByBreedClassID(int breedClassID)
        {
            return spotCostsObj.GetByKey(breedClassID);
        }
        #endregion

        #region 交易币种
        /// <summary>
        ///Title:根据BreedClassID获取交易费用所属货币类型
        ///Desc.:此方法内部是先根据商品类别查询到现货的交易费用实体然后再通过费用所属的货币类型返回相应的货币实体.
        ///      因为现在有港股加入，费用表不在现货所以此方法使用时要与注意与港股分开
        /// </summary>
        /// <param name="breedClassID">breedClassID</param>
        /// <returns>货币类型</returns>
        public CM_CurrencyType GetCurrencyTypeByBreedClassID(int breedClassID)
        {
            XH_SpotCosts spotCosts = GetSpotCostsByBreedClassID(breedClassID);

            CM_CurrencyType currencyType = null;
            if (spotCosts != null)
            {
                int? currencyTypeID = spotCosts.CurrencyTypeID;

                if (currencyTypeID.HasValue)
                {
                    currencyType = MCService.CommonPara.GetCurrencyTypeByID(currencyTypeID.Value);
                }
            }

            return currencyType;
        }

        /// <summary>
        /// 根据商品代码获取货币类型
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>货币类型</returns>
        public CM_CurrencyType GetCurrencyTypeByCommodityCode(string code)
        {
            CM_CurrencyType type = null;
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            if (breedClass != null)
            {
                type = GetCurrencyTypeByBreedClassID(breedClass.BreedClassID);
            }

            return type;
        }
        #endregion

        #region 现货_品种_涨跌幅_控制类型
        /// <summary>
        /// 获取所有的现货_品种_涨跌幅_控制类型
        /// </summary>
        /// <returns></returns>
        public IList<XH_SpotHighLowControlType> GetAllSpotHighLowControlType()
        {
            return GetAllSpotHighLowControlType(false);
        }

        private IList<XH_SpotHighLowControlType> GetAllSpotHighLowControlType(bool reLoad)
        {
            return spotHighLowControlTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的现货_品种_涨跌幅_控制类型
        /// </summary>
        /// <returns></returns>
        private IList<XH_SpotHighLowControlType> GetAllSpotHighLowControlTypeFromWCF()
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetAllSpotHighLowControlType();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8107";
                string errMsg = "无法从管理中心获取现货品种涨跌幅控制类型列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public XH_SpotHighLowControlType GetSpotHighLowControlTypeByBreedClassHighLowID(int id)
        {
            return spotHighLowControlTypeObj.GetByKey(id);
        }

        public XH_SpotHighLowControlType GetSpotHighLowControlTypeByBreedClassID(int breedClassID)
        {
            XH_SpotHighLowControlType result = null;
            XH_SpotTradeRules rules = GetSpotTradeRulesByBreedClassID(breedClassID);
            if (rules != null)
            {
                if (rules.BreedClassHighLowID.HasValue)
                    result = GetSpotHighLowControlTypeByBreedClassHighLowID(rules.BreedClassHighLowID.Value);
            }

            return result;
        }
        #endregion

        #region 现货_品种_涨跌幅
        public IList<XH_SpotHighLowValue> GetAllSpotHighLowValue()
        {
            return GetAllSpotHighLowValue(false);
        }

        public IList<XH_SpotHighLowValue> GetAllSpotHighLowValue(bool reLoad)
        {
            return spotHighLowValueObj.GetAll(reLoad);
        }

        public XH_SpotHighLowValue GetSpotHighLowByHightLowValueID(int id)
        {
            return spotHighLowValueObj.GetByKey(id);
        }

        /// <summary>
        /// 获取所有的现货_品种_涨跌幅
        /// </summary>
        /// <returns></returns>
        private IList<XH_SpotHighLowValue> GetAllSpotHighLowValueFromWCF()
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetAllSpotHighLowValue();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8115";
                string errMsg = "无法从管理中心获取现货涨跌幅列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        private XH_SpotHighLowValue GetSpotHighLowByHightLowValueIDFromWCF(int id)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetSpotHighLowValueByHightLowID(id);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8116";
                string errMsg = "无法从管理中心获取现货涨跌幅。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<XH_SpotHighLowValue> GetSpotHighLowValueByBreedClassHighLowID(int breedClassHighLowID)
        {
            return spotHighLowValueByBreedClassHighLowIDObj.GetByKey(breedClassHighLowID);
        }

        private IList<XH_SpotHighLowValue> GetSpotHighLowValueByBreedClassHighLowIDFromWCF(int breedClassHighLowID)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetSpotHighLowValueByBreedClassHighLowID(breedClassHighLowID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8115";
                string errMsg = "无法从管理中心获取现货涨跌幅列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }
        #endregion

        #region 现货_交易商品品种_持仓限制
        /// <summary>
        /// 获取所有的现货_交易商品品种_持仓限制
        /// </summary>
        /// <returns></returns>
        public IList<XH_SpotPosition> GetAllSpotPosition()
        {
            return GetAllSpotPosition(false);
        }

        private IList<XH_SpotPosition> GetAllSpotPosition(bool reLoad)
        {
            return spotPositionObj.GetAll(reLoad);
        }

        public XH_SpotPosition GetSpotPositionByBreedClassID(int id)
        {
            return spotPositionObj.GetByKey(id);
        }

        /// <summary>
        /// 获取所有的现货_交易商品品种_持仓限制
        /// </summary>
        /// <returns></returns>
        private IList<XH_SpotPosition> GetAllSpotPositionFromWCF()
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetAllSpotPosition();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8108";
                string errMsg = "无法从管理中心获取现货品种持仓限制列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        private XH_SpotPosition GetSpotPositionByBreedClassIDFromWCF(int id)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetSpotPositionByBreedClassID(id);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8109";
                string errMsg = "无法从管理中心获取现货品种持仓限制。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public XH_SpotPosition GetSpotPositionByCommodityCode(string code)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            if (breedClass == null)
                return null;

            return GetSpotPositionByBreedClassID(breedClass.BreedClassID);
        }
        #endregion

        #region 现货现货_交易费用_成交额_交易手续费范围值
        ///// <summary>
        ///// 获取所有的品种_现货_交易费用_成交额_交易手续费
        ///// </summary>
        ///// <returns></returns>
        //public IList<XH_SpotRangeCost> GetAllSpotRangeCost()
        //{
        //    return GetAllSpotRangeCost(false);
        //}

        //private IList<XH_SpotRangeCost> GetAllSpotRangeCost(bool reLoad)
        //{
        //    return spotRangCostObj.GetAll(reLoad);
        //}

        ///// <summary>
        ///// 获取所有的品种_现货_交易费用_成交额_交易手续费
        ///// </summary>
        ///// <returns></returns>
        //private IList<XH_SpotRangeCost> GetAllSpotRangeCostFromWCF()
        //{
        //    try
        //    {
        //        using (SpotTradeRulesClient client = GetClient())
        //            return client.GetAllSpotRangeCost();
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8110";
        //        string errMsg = "无法从管理中心获取现货交易手续费列表。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}


        //public IList<XH_SpotRangeCost> GetSpotRangeCostByBreedClassID(int id)
        //{
        //    return spotRangeCostByBreedClassID.GetByKey(id);
        //}

        ///// <summary>
        ///// 根据品种标识返回品种_现货_交易费用_成交额_交易手续费
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //private IList<XH_SpotRangeCost> GetSpotRangeCostByBreedClassIDFromWCF(int id)
        //{
        //    try
        //    {
        //        using (SpotTradeRulesClient client = GetClient())
        //            return client.GetSpotRangeCostByBreedClassID(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8111";
        //        string errMsg = "无法根据商品品种编码从管理中心获取现货交易手续费列表。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}
        #endregion

        #region 现货_品种_交易规则
        public IList<XH_SpotTradeRules> GetAllSpotTradeRules()
        {
            return GetAllSpotTradeRules(false);
        }

        private IList<XH_SpotTradeRules> GetAllSpotTradeRules(bool reLoad)
        {
            return spotTradeRulesObj.GetAll(reLoad);
        }

        public XH_SpotTradeRules GetSpotTradeRulesByBreedClassID(int id)
        {
            return spotTradeRulesObj.GetByKey(id);
        }

        /// <summary>
        /// 获取所有的现货_品种_交易规则
        /// </summary>
        /// <returns></returns>
        private IList<XH_SpotTradeRules> GetAllSpotTradeRulesFromWCF()
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetAllSpotTradeRules();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8112";
                string errMsg = "无法从管理中心获取现货交易规则列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        private XH_SpotTradeRules GetSpotTradeRulesByBreedClassIDFromWCF(int id)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetSpotTradeRulesByBreedClassID(id);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8113";
                string errMsg = "无法从管理中心获取现货交易规则。";
                throw new VTException(errCode, errMsg, ex);
            }
        }
        #endregion

        #region 现货有效申报类型
        public IList<XH_ValidDeclareType> GetAllValidDeclareType()
        {
            return GetAllValidDeclareType(false);
        }

        public IList<XH_ValidDeclareType> GetAllValidDeclareType(bool reLoad)
        {
            return validDeclareTypeObj.GetAll(reLoad);
        }

        private IList<XH_ValidDeclareType> GetAllValidDeclareTypeFromWCF()
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetAllValidDeclareType();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8119";
                string errMsg = "无法从管理中心获取涨跌幅类型列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public XH_ValidDeclareType GetValidDeclareTypeByBreedClassValidID(int id)
        {
            return validDeclareTypeObj.GetByKey(id);
        }

        private XH_ValidDeclareType GetValidDeclareTypeByBreedClassValidIDFromWCF(int id)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetValidDeclareTypeByBreedClassValidID(id);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8120";
                string errMsg = "无法根据涨跌幅类型编码从管理中心获取涨跌幅类型。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public XH_ValidDeclareType GetValidDeclareTypeByBreedClassID(int id)
        {
            XH_SpotTradeRules tradeRules = GetSpotTradeRulesByBreedClassID(id);
            if (tradeRules == null)
                return null;

            return GetValidDeclareTypeByBreedClassValidID(tradeRules.BreedClassValidID.Value);
        }
        #endregion

        #region 有效申报类型值
        private IList<XH_ValidDeclareValue> GetValidDeclareValueByBreedClassValidIDFromWCF(int breedClassValidID)
        {
            try
            {
                using (SpotTradeRulesClient client = GetClient())
                    return client.GetValidDeclareValueByBreedClassValidID(breedClassValidID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8121";
                string errMsg = "无法根据涨跌幅验证编码从管理中心获取涨跌幅验证列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<XH_ValidDeclareValue> GetValidDeclareValueByBreedClassID(int id)
        {
            XH_SpotTradeRules tradeRules = GetSpotTradeRulesByBreedClassID(id);
            if (tradeRules != null)
            {
                return GetValidDeclareValueByBreedClassValidIDFromWCF(tradeRules.BreedClassValidID.Value);
            }

            return null;
        }
        #endregion

        /// <summary>
        /// 获取现货的交割制度
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="fund">资金交割制度</param>
        /// <param name="stock">股票交割制度</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>是否成功获取</returns>
        public bool GetDeliveryInstitution(string code, out int fund, out int stock, ref string strMessage)
        {
            bool result = false;
            string errCode = "GT-8114";
            string errMsg = "无法根据商品编码从管理中心获取对于的交割制度。";
            strMessage = errCode + ":" + errMsg;

            fund = -1;
            stock = -1;

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            if (breedClass != null)
            {
                XH_SpotTradeRules rules = GetSpotTradeRulesByBreedClassID(breedClass.BreedClassID);
                if (rules != null)
                {
                    fund = rules.FundDeliveryInstitution.Value;
                    stock = rules.StockDeliveryInstitution.Value;
                    result = true;
                    strMessage = "";
                }
            }

            return result;
        }

        //public IList<XH_RightHightLowPrices> GetAllRightHighLowPrices()
        //{
        //    return GetAllRightHighLowPrices(false);
        //}

        //private IList<XH_RightHightLowPrices> GetAllRightHighLowPrices(bool reLoad)
        //{
        //    return rightHightLowPricesObj.GetAll(reLoad);
        //}

        ///// <summary>
        ///// 获取所有的权证涨跌幅价格
        ///// </summary>
        ///// <returns></returns>
        //private IList<XH_RightHightLowPrices> GetAllRightHighLowPricesFromWCF()
        //{
        //    try
        //    {
        //        using (SpotTradeRulesClient client = GetClient())
        //            return client.GetAllRightHightLowPrices();
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8117";
        //        string errMsg = "无法从管理中心获取权证涨跌幅价格列表。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}

        //public IList<XH_RightHightLowPrices> GetRightHighLowPricesByHightLowID(int id)
        //{
        //    return rightHighLowPricesByHightLowIDObj.GetByKey(id);
        //}

        /// <summary>
        /// 根据涨跌幅标识获取权证涨跌幅价格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //private IList<XH_RightHightLowPrices> GetRightHighLowPricesByHightLowIDFromWCF(int id)
        //{
        //    try
        //    {
        //        using (SpotTradeRulesClient client = GetClient())
        //            return client.GetRightHightLowPricesByHightLowID(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8118";
        //        string errMsg = "无法根据涨跌幅编码从管理中心获取权证涨跌幅价格。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}

        #endregion
    }
}