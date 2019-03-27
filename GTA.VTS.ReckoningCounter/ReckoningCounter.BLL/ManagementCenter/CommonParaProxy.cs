#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.HKTradingRulesService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 管理中心ICommonPara的缓存包装类，错误编码8000-8099
    /// 作者：宋涛
    /// 日期：2008-11-26
    /// 增加港股相关数据缓存数据操作
    /// 作者：李健华
    /// 日期：2009-10-20
    /// </summary>
    public class CommonParaProxy
    {
        #region CommonOject Fields

        private WCFCacheObjectWithGetAll<int, UM_AccountType> accountTypeObj;
        private WCFCacheObject<int, CM_BourseType> bourseTypeObj;
        private WCFCacheObjectWithGetKey<int, IList<CM_BreedClass>> breedClassByBourseTypeObj;
        private WCFCacheObject<int, CM_BreedClass> breedClassObj;
        private WCFCacheObjectWithGetAll<int, CM_BreedClassType> breedClassTypeObj;
        private WCFCacheObjectWithGetKey<int, IList<CM_Commodity>> commodityByBreedClassObj;
        private WCFCacheObject<string, CM_Commodity> commodityObj;
        private WCFCacheObject<string, CM_CommodityFuse> commondityFuseObj;
        private WCFCacheObjectWithGetAll<int, CM_CurrencyType> currencyTypeObj;
        // private WCFCacheObject<int, CM_FieldRange> fieldRangeObj;
        private WCFCacheObjectWithGetAll<int, CM_FuseTimesection> fuseTimeSectionObj;
        // private WCFCacheObject<int, CM_MarketParticipation> marketParticipationObj;
        private WCFCacheObjectWithGetAll<int, RC_MatchCenter> matchCenterObj;
        private WCFCacheObjectWithGetKey<string, RC_MatchMachine> matchineByCommodityObj;
        private WCFCacheObjectWithGetAll<int, RC_MatchMachine> matchMachineObj;

        //private WCFCacheObjectWithGetAll<string, CM_Commodity> newCommodityObj;
        //private WCFCacheObjectWithGetAll<string, ZFInfo> zfCommodityObj;
        //add 李健华 2010-03-15 为了解决是否为薪股的判断出现计算涨跌幅处理,直接缓存在此
        private static SyncCache<string, bool> newCommodityObj;
        private static SyncCache<string, bool> zfCommodityObj;

        private WCFCacheObjectWithGetKey<int, IList<CM_NotTradeDate>> notTradeDateByBourseTypeObj;
        private WCFCacheObjectWithGetAll<string, CM_StockMelonCash> stockMelonCashObj;
        private WCFCacheObjectWithGetAll<string, CM_StockMelonStock> stockMelonStockObj;
        private WCFCacheObjectWithGetKey<int, IList<CM_TradeTime>> tradeTimeByBourseTypeIDObj;
        private WCFCacheObjectWithGetAll<int, CM_TradeTime> tradeTimeObj;
        private WCFCacheObjectWithGetAll<int, CM_TradeWay> tradeWayObj;
        private WCFCacheObjectWithGetAll<int, CM_UnitConversion> unitConversionObj;
        private WCFCacheObjectWithGetKey<int, IList<CM_UnitConversion>> unitConvertionByBreedClassIDObj;
        private WCFCacheObjectWithGetAll<int, CM_Units> unitsObj;
        private WCFCacheObjectWithGetAll<int, CM_ValueType> valueTypeObj;
        private WCFCacheObjectWithGetKey<int, IList<UM_DealerTradeBreedClass>> dealerTraderObj;
        private WCFCacheObjectWithGetAll<string, YesterdayClosePriceInfo> closePriceInfoObj;





        //
        #endregion

        private static CommonParaProxy instance = new CommonParaProxy();

        private CommonParaProxy()
        {
            #region WCFCacheObject

            bourseTypeObj =
                new WCFCacheObject<int, CM_BourseType>(GetAllBourseTypeFromWCF, GetBourseTypeByBourseTypeIDFromWCF,
                                                       val => val.BourseTypeID);
           commodityObj = new WCFCacheObject<string, CM_Commodity>(GetAllCommodityFromWCF, GetCommodityByCommodityCodeFromWCF, Val => Val.CommodityCode);
 

            breedClassObj = new WCFCacheObject<int, CM_BreedClass>(GetAllBreedClassFromWCF,
                                                                   GetBreedClassByBreedClassIDFromWCF,
                                                                   val => val.BreedClassID);

            commondityFuseObj = new WCFCacheObject<string, CM_CommodityFuse>(GetAllCommodityFuseFromWCF,
                                                                             GetCommodityFuseByCommodityCodeFromWCF,
                                                                             val => val.CommodityCode);

            //fieldRangeObj = new WCFCacheObject<int, CM_FieldRange>(GetAllFieldRangeFromWCF,
            //                                                       GetFieldRangeByFieldRangeIDFromWCF,
            //                                                       val => val.FieldRangeID);

            //marketParticipationObj = new WCFCacheObject<int, CM_MarketParticipation>(
            //    GetAllMarketParticipationFromWCF, GetMarketParticipationByBreedClassIDFromWCF, val => val.BreedClassID);
            #endregion

            #region WCFCacheObjectWithGetAll

            stockMelonCashObj = new WCFCacheObjectWithGetAll<string, CM_StockMelonCash>(GetAllStockMelonCashFromWCF, val => val.CommodityCode);
            stockMelonStockObj = new WCFCacheObjectWithGetAll<string, CM_StockMelonStock>(GetAllStockMelonStockFromWCF, val => val.CommodityCode);
            breedClassTypeObj = new WCFCacheObjectWithGetAll<int, CM_BreedClassType>(GetAllBreedClassTypeFromWCF, val => val.BreedClassTypeID);
            currencyTypeObj = new WCFCacheObjectWithGetAll<int, CM_CurrencyType>(GetAllCurrencyTypeFromWCF, val => val.CurrencyTypeID);
            tradeWayObj = new WCFCacheObjectWithGetAll<int, CM_TradeWay>(GetAllTradeWayFromWCF, val => val.TradeWayID);
            unitsObj = new WCFCacheObjectWithGetAll<int, CM_Units>(GetAllUnitsFromWCF, val => val.UnitsID);
            valueTypeObj = new WCFCacheObjectWithGetAll<int, CM_ValueType>(GetAllValueTypeFromWCF, val => val.ValueTypeID);
            fuseTimeSectionObj = new WCFCacheObjectWithGetAll<int, CM_FuseTimesection>(GetAllFuseTimeSectionFromWCF, val => val.TimesectionID);
            tradeTimeObj = new WCFCacheObjectWithGetAll<int, CM_TradeTime>(GetAllTradeTimeFromWCF, val => val.TradeTimeID);
            unitConversionObj = new WCFCacheObjectWithGetAll<int, CM_UnitConversion>(GetAllUnitConversionFromWCF, val => val.UnitConversionID);
            matchCenterObj = new WCFCacheObjectWithGetAll<int, RC_MatchCenter>(GetAllMatchCenterFromWCF, val => val.MatchCenterID);
            matchMachineObj = new WCFCacheObjectWithGetAll<int, RC_MatchMachine>(GetAllMatchMachineFromWCF, val => val.MatchMachineID);
            accountTypeObj = new WCFCacheObjectWithGetAll<int, UM_AccountType>(GetAllAccountTypeFromWCF, val => val.AccountTypeID);

            //newCommodityObj = new WCFCacheObjectWithGetAll<string, CM_Commodity>(GetNewCommodityFromWCF, val => val.CommodityCode);
            //zfCommodityObj = new WCFCacheObjectWithGetAll<string, ZFInfo>(GetZFCommodityFromWCF, val => val.stkcd);
            newCommodityObj = new SyncCache<string, bool>();
            zfCommodityObj = new SyncCache<string, bool>();

            closePriceInfoObj = new WCFCacheObjectWithGetAll<string, YesterdayClosePriceInfo>(GetClosePriceFromWCF, val => val.Code);
            #endregion

            #region WCFCacheObjectWithGetKey

            tradeTimeByBourseTypeIDObj =
                new WCFCacheObjectWithGetKey<int, IList<CM_TradeTime>>(GetTradeTimeByBourseTypeIDFromWCF);

            commodityByBreedClassObj =
                new WCFCacheObjectWithGetKey<int, IList<CM_Commodity>>(GetAllCommodityByBreedClassFromWCF);

            unitConvertionByBreedClassIDObj =
                new WCFCacheObjectWithGetKey<int, IList<CM_UnitConversion>>(GetUnitConversionByBreedClassIDFromWCF);

            matchineByCommodityObj =
                new WCFCacheObjectWithGetKey<string, RC_MatchMachine>(GetMatchMachineByCommodityCodeFromWCF);

            notTradeDateByBourseTypeObj =
                new WCFCacheObjectWithGetKey<int, IList<CM_NotTradeDate>>(GetNotTradeDateByBourseTypeIDFromWCF);

            breedClassByBourseTypeObj =
                new WCFCacheObjectWithGetKey<int, IList<CM_BreedClass>>(GetAllBreedClassByBourseTypeFromWCF);

            dealerTraderObj = new WCFCacheObjectWithGetKey<int, IList<UM_DealerTradeBreedClass>>(GetTransactionRightTableFromWCF);

            #endregion

        }
        /// <summary>
        /// 获取单一实例
        /// </summary>
        /// <returns></returns>
        public static CommonParaProxy GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// 进行预加载
        /// </summary>
        public void Initialize()
        {
           GetAllCommodity();
            GetAllBreedClass();
            GetAllAccountType();
            GetAllBreedClassType();
            GetAllBourseType();
            GetAllCommodityFuse();
            GetAllCurrencyType();
            GetAllMatchCenter();
            GetAllMatchMachine();
            GetAllTradeTime();
            GetAllTradeWay();
            GetAllUnitConversion();
            GetAllUnits();
            //获取新股，增发股
            GetALLNewCommodity();//这里不管如何都进行重新初始化
            GetALLZFCommodity();
        }

        public void Reset()
        {
            accountTypeObj.Reset();
            bourseTypeObj.Reset();
            breedClassObj.Reset();
            breedClassTypeObj.Reset();
            commodityObj.Reset();
            commondityFuseObj.Reset();
            currencyTypeObj.Reset();
            //fieldRangeObj.Reset();
            fuseTimeSectionObj.Reset();
            //marketParticipationObj.Reset();
            matchCenterObj.Reset();
            matchMachineObj.Reset();
            stockMelonCashObj.Reset();
            stockMelonStockObj.Reset();
            tradeTimeObj.Reset();
            tradeWayObj.Reset();
            unitConversionObj.Reset();
            unitsObj.Reset();
            valueTypeObj.Reset();
            newCommodityObj.Reset();
            zfCommodityObj.Reset();

            commodityByBreedClassObj.Reset();
            tradeTimeByBourseTypeIDObj.Reset();
            unitConvertionByBreedClassIDObj.Reset();
            matchineByCommodityObj.Reset();
            notTradeDateByBourseTypeObj.Reset();
            breedClassByBourseTypeObj.Reset();
        }

        private CommonParaClient GetClient()
        {
            CommonParaClient client;
            try
            {
                client = new CommonParaClient();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8000";
                string errMsg = "无法获取管理中心提供的服务[ICommonPara]。";
                throw new VTException(errCode, errMsg, ex);
            }

            return client;
        }

        /// <summary>
        /// 获取所有新上市的商品
        /// </summary>
        /// <returns>商品列表</returns>
        private IList<CM_Commodity> GetNewCommodityFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                {
                    List<CM_Commodity> list = client.GetNewCommodity();
                    if (!Utils.IsNullOrEmpty(list))
                    {

                        string debugStr = "";
                        foreach (var item in list)
                        {
                            debugStr += item.CommodityCode + "   ";
                        }
                        LogHelper.WriteDebug("获取到新股代码" + debugStr);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GT-8001";
                string errMsg = "无法从管理中心获取新股列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }


        /// <summary>
        /// 获取所有新上市的商品初始化缓存数据
        /// </summary>
        /// <returns>商品列表</returns>
        private void GetALLNewCommodity()
        {
            IList<CM_Commodity> list = GetNewCommodityFromWCF();
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (!newCommodityObj.Contains(item.CommodityCode))
                    {
                        newCommodityObj.Add(item.CommodityCode, true);
                    }
                }
            }
        }

        ///// <summary>
        ///// 获取所有新上市的商品
        ///// </summary>
        ///// <returns>商品列表</returns>
        //public IList<CM_Commodity> GetALLNewCommodity(bool reLoad)
        //{
        //    return newCommodityObj.GetAll(reLoad);
        //}

        /// <summary>
        /// 根据商品代码获取新上市的商品
        /// </summary>
        /// <returns>商品</returns>
        public bool GetNewCommodityByCommodityCode(string code)
        {
            bool result = false;
            bool value = false;
            result = newCommodityObj.TryGetValue(code, out value);
            return result;
        }

        /// <summary>
        /// 获取增发上市的商品列表
        /// </summary>
        /// <returns>商品列表</returns>
        private IList<ZFInfo> GetZFCommodityFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())

                    return client.GetZFCommodity();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8002";
                string errMsg = "无法从管理中心获取增发列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取增发上市的商品列表
        /// </summary>
        /// <returns>商品列表</returns>
        private void GetALLZFCommodity()
        {
            IList<ZFInfo> list = GetZFCommodityFromWCF();
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (!zfCommodityObj.Contains(item.stkcd))
                    {
                        zfCommodityObj.Add(item.stkcd, true);
                    }
                }
            }
        }

        ///// <summary>
        ///// 获取增发上市的商品列表
        ///// </summary>
        ///// <returns>商品列表</returns>
        //public IList<ZFInfo> GetALLZFCommodity(bool reLoad)
        //{
        //    return zfCommodityObj.GetAll(reLoad);
        //}

        /// <summary>
        /// 根据商品代码获取增发上市的商品
        /// </summary>
        /// <returns>商品</returns>
        public bool GetZFCommodityByCommodityCode(string code)
        {
            bool result = false;
            bool value = false;
            result = zfCommodityObj.TryGetValue(code, out value);
            return result;
        }

        #region 功能方法

        /// <summary>
        /// 根据交易所id获取属于此交易所的所有交易时间
        /// </summary>
        /// <param name="bourseTypeID">bourseTypeID</param>
        /// <returns>交易时间列表</returns>
        public IList<CM_TradeTime> GetAllTradeTimeByBourseTypeID(int bourseTypeID)
        {
            return tradeTimeByBourseTypeIDObj.GetByKey(bourseTypeID);
        }

        /// <summary>
        /// 根据商品代码获取交易所类型
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>交易所类型</returns>
        public CM_BourseType GetBourseTypeByCommodityCode(string code)
        {
            #region old code
            //CM_BourseType bourseType = null;

            //CM_BreedClass breedClass = GetBreedClassByCommodityCode(code);
            //if (breedClass != null)
            //{
            //    bourseType = GetBourseTypeByBourseTypeID(breedClass.BourseTypeID.Value);
            //}

            //return bourseType;
            #endregion

            return GetBourseTypeByCommodityCode(code, Types.BreedClassTypeEnum.Stock);
        }
        /// <summary>
        /// 根据商品代码获取交易所类型
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <returns>交易所类型</returns>
        public CM_BourseType GetBourseTypeByCommodityCode(string code, Types.BreedClassTypeEnum type)
        {
            CM_BourseType bourseType = null;

            CM_BreedClass breedClass = GetBreedClassByCommodityCode(code, type);
            if (breedClass != null)
            {
                bourseType = GetBourseTypeByBourseTypeID(breedClass.BourseTypeID.Value);
            }
            return bourseType;
        }


        #endregion

        #region 管理中心相关

        #region Commodity

        private IList<CM_Commodity> GetAllCommodityFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllCommodity();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8003";
                string errMsg = "无法从管理中心获取所有商品列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        private CM_Commodity GetCommodityByCommodityCodeFromWCF(string code)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetCommodityByCommodityCode(code);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8004";
                string errMsg = "无法根据商品代码从管理中心获取指定商品。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的交易商品
        /// </summary>
        /// <returns>交易商品列表</returns>
        public IList<CM_Commodity> GetAllCommodity()
        {
            return commodityObj.GetAll();
        }

        /// <summary>
        /// 获取所有的交易商品
        /// </summary>
        /// <param name="reLoad">是否从WCF重新加载</param>
        /// <returns>交易商品列表</returns>
        public IList<CM_Commodity> GetAllCommodity(bool reLoad)
        {
            return commodityObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据商品代码获取商品
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>商品</returns>
        public CM_Commodity GetCommodityByCommodityCode(string code)
        {
            return commodityObj.GetByKey(code);
        }

        private IList<CM_Commodity> GetAllCommodityByBreedClassFromWCF(int breedClassID)
        {
            IList<CM_Commodity> list;
            try
            {
                using (CommonParaClient client = GetClient())
                    list = client.GetCommodityByBreedClassID(breedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8005";
                string errMsg = "无法根据商品品种从管理中心获取指定商品。";
                throw new VTException(errCode, errMsg, ex);
            }

            return list;
        }


        /// <summary>
        /// 根据交易品种id获取属于此交易品种的所有商品
        /// </summary>
        /// <param name="breedClassID">breedClassID</param>
        /// <returns>商品列表</returns>
        public IList<CM_Commodity> GetAllCommodityByBreedClass(int breedClassID)
        {
            return commodityByBreedClassObj.GetByKey(breedClassID);
        }

        /// <summary>
        /// 根据品种代码返回品种标识
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>商品</returns>
        public int? GetBreedClassIdByCommodityCode(string code)
        {
            #region old code
            //CM_Commodity commdity = GetCommodityByCommodityCode(code);

            //int? result = null;
            //if (commdity != null)
            //    result = commdity.BreedClassID;

            //return result;
            #endregion

            return GetBreedClassIdByCommodityCode(code, Types.BreedClassTypeEnum.Stock);
        }
        /// <summary>
        /// 根据品种代码返回品种标识
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <returns>商品</returns>
        public int? GetBreedClassIdByCommodityCode(string code, Types.BreedClassTypeEnum type)
        {
            int? breedClassID = null;
            switch (type)
            {
                case Types.BreedClassTypeEnum.Stock:
                case Types.BreedClassTypeEnum.CommodityFuture:
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    CM_Commodity commodity = GetCommodityByCommodityCode(code);
                    if (commodity != null)
                    {
                        breedClassID = commodity.BreedClassID;
                    }
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    HK_Commodity hkCom = HKStockTradeRulesProxy.GetInstance().GetHKCommodityByCommodityCode(code);
                    if (hkCom != null)
                    {
                        breedClassID = hkCom.BreedClassID.Value;
                    }
                    break;
            }
            return breedClassID;
        }

        #endregion

        #region CommodityFuse 可交易商品_熔断

        private IList<CM_CommodityFuse> GetAllCommodityFuseFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllCommodityFuse();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8006";
                string errMsg = "无法从管理中心获取可交易商品熔断列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 根据商品代码获取可交易商品_熔断
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>CM_CommodityFuse</returns>
        private CM_CommodityFuse GetCommodityFuseByCommodityCodeFromWCF(string code)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetCommodityFuseByCommodityCode(code);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8007";
                string errMsg = "无法根据商品代码从管理中心获取可交易商品熔断对象。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的可交易商品_熔断
        /// </summary>
        /// <returns>可交易商品_熔断列表</returns>
        public IList<CM_CommodityFuse> GetAllCommodityFuse()
        {
            return commondityFuseObj.GetAll();
        }

        /// <summary>
        /// 获取所有的可交易商品_熔断
        /// </summary>
        /// <returns>可交易商品_熔断列表</returns>
        public IList<CM_CommodityFuse> GetAllCommodityFuse(bool reLoad)
        {
            return commondityFuseObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据商品代码获取可交易商品_熔断
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>CM_CommodityFuse</returns>
        public CM_CommodityFuse GetCommodityFuseByCommodityCode(string code)
        {
            return commondityFuseObj.GetByKey(code);
        }

        #endregion

        #region FuseTimeSection 熔断-时间段标识

        private IList<CM_FuseTimesection> GetAllFuseTimeSectionFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllFuseTimesection();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8008";
                string errMsg = "无法从管理中心获取熔断时间段标识列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<CM_FuseTimesection> GetAllFuseTimeSection()
        {
            return fuseTimeSectionObj.GetAll();
        }

        public IList<CM_FuseTimesection> GetAllFuseTimeSection(bool reLoad)
        {
            return fuseTimeSectionObj.GetAll(reLoad);
        }

        public CM_FuseTimesection GetFuseTimeSectionByTimesectionIDField(int timesectionIDField)
        {
            return fuseTimeSectionObj.GetByKey(timesectionIDField);
        }

        //TODO:Nocache
        public IList<CM_FuseTimesection> GetFuseTimeSectionByCommodityCode(string code)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetFuseTimesectionByCommodityCode(code);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8009";
                string errMsg = "无法从根据商品代码管理中心获取熔断时间段标识列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        #endregion

        #region CurrencyType 货币类型

        private IList<CM_CurrencyType> GetAllCurrencyTypeFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllCurrencyType();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8010";
                string errMsg = "无法从管理中心获取货币类型列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的交易货币类型
        /// </summary>
        /// <returns>交易货币类型列表</returns>
        public IList<CM_CurrencyType> GetAllCurrencyType()
        {
            return currencyTypeObj.GetAll();
        }

        /// <summary>
        /// 获取所有的交易货币类型
        /// </summary>
        /// <returns>交易货币类型列表</returns>
        public IList<CM_CurrencyType> GetAllCurrencyType(bool reLoad)
        {
            return currencyTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据CurrencyTypeID获取校验仪的货币类型
        /// </summary>
        /// <param name="currencyTypeID">CurrencyTypeID</param>
        /// <returns>货币类型</returns>
        public CM_CurrencyType GetCurrencyTypeByID(int currencyTypeID)
        {
            return currencyTypeObj.GetByKey(currencyTypeID);
        }

        /// <summary>
        /// 根据商品代码获取货币类型
        /// </summary>
        /// <param name="type">商品所属类型这里主要是为了区别于港股</param>
        /// <param name="code">商品代码</param>
        /// <returns>货币类型</returns>
        public CM_CurrencyType GetCurrencyTypeByCommodityCode(Types.BreedClassTypeEnum type, string code)
        {
            CM_CurrencyType cutype = null;
            CM_BreedClass breedClass = GetBreedClassByCommodityCode(code, type);
            int? currencyTypeID = null;
            switch ((Types.BreedClassTypeEnum)breedClass.BreedClassTypeID.Value)
            {
                case Types.BreedClassTypeEnum.Stock:
                case Types.BreedClassTypeEnum.CommodityFuture:
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    XH_SpotCosts xhspotCosts = MCService.SpotTradeRules.GetSpotCostsByBreedClassID(breedClass.BreedClassID);
                    if (xhspotCosts != null)
                    {
                        currencyTypeID = xhspotCosts.CurrencyTypeID;
                    }
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    HK_SpotCosts hkspotCosts = MCService.HKTradeRulesProxy.GetSpotCostsByBreedClassID(breedClass.BreedClassID);
                    if (hkspotCosts != null)
                    {
                        currencyTypeID = hkspotCosts.CurrencyTypeID;
                    }
                    break;
                default:
                    break;
            }

            if (currencyTypeID.HasValue)
            {
                cutype = GetCurrencyTypeByID(currencyTypeID.Value);
            }
            return cutype;
        }

        /// <summary>
        ///Title:根据BreedClassID获取获取交易费用所属货币类型
        /// </summary>
        /// <param name="breedClassID">breedClassID</param>
        /// <returns>货币类型</returns>
        public CM_CurrencyType GetCurrencyTypeByBreedClassID(int breedClassID)
        {
            CM_CurrencyType currencyType = null;

            CM_BreedClass cm_BreedClass = GetBreedClassByBreedClassID(breedClassID);
            if (cm_BreedClass == null || !cm_BreedClass.BreedClassTypeID.HasValue)
            {
                return currencyType;
            }
            int? currencyTypeID = null;
            switch ((Types.BreedClassTypeEnum)cm_BreedClass.BreedClassTypeID.Value)
            {
                case Types.BreedClassTypeEnum.Stock:
                case Types.BreedClassTypeEnum.CommodityFuture:
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    XH_SpotCosts xhspotCosts = MCService.SpotTradeRules.GetSpotCostsByBreedClassID(breedClassID);
                    if (xhspotCosts != null)
                    {
                        currencyTypeID = xhspotCosts.CurrencyTypeID;
                    }
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    HK_SpotCosts hkspotCosts = MCService.HKTradeRulesProxy.GetSpotCostsByBreedClassID(breedClassID);
                    if (hkspotCosts != null)
                    {
                        currencyTypeID = hkspotCosts.CurrencyTypeID;
                    }
                    break;
                default:
                    break;
            }

            if (currencyTypeID.HasValue)
            {
                currencyType = MCService.CommonPara.GetCurrencyTypeByID(currencyTypeID.Value);
            }

            return currencyType;
        }
        #endregion

        #region StockMelonCash StockMelonStock 分红

        private IList<CM_StockMelonCash> GetAllStockMelonCashFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllStockMelonCash();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8011";
                string errMsg = "无法从管理中心获取分红列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }


        /// <summary>
        /// 获取所有的股票分红记录_现金
        /// </summary>
        /// <returns>股票分红记录_现金列表</returns>
        public IList<CM_StockMelonCash> GetAllStockMelonCash()
        {
            return stockMelonCashObj.GetAll();
        }

        /// <summary>
        /// 获取所有的股票分红记录_现金
        /// </summary>
        /// <returns>股票分红记录_现金列表</returns>
        public IList<CM_StockMelonCash> GetAllStockMelonCash(bool reLoad)
        {
            return stockMelonCashObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据商品代码获取股票分红记录_现金
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>股票分红记录_现金</returns>
        public CM_StockMelonCash GetStockMelonCashByCommodityCode(string code)
        {
            return stockMelonCashObj.GetByKey(code);
        }

        private IList<CM_StockMelonStock> GetAllStockMelonStockFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllStockMelonStock();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8011";
                string errMsg = "无法从管理中心获取股票分红记录。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的股票分红记录_股票
        /// </summary>
        /// <returns>股票分红记录_股票列表</returns>
        public IList<CM_StockMelonStock> GetAllStockMelonStock()
        {
            return stockMelonStockObj.GetAll();
        }

        /// <summary>
        /// 获取所有的股票分红记录_股票
        /// </summary>
        /// <returns>股票分红记录_股票列表</returns>
        public IList<CM_StockMelonStock> GetAllStockMelonStock(bool reLoad)
        {
            return stockMelonStockObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据商品代码获取股票分红记录_股票
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>股票分红记录_股票</returns>
        public CM_StockMelonStock GetStockMelonStockByCommodityCode(string code)
        {
            return stockMelonStockObj.GetByKey(code);
        }

        #endregion

        #region TradeTime 交易时间

        private IList<CM_TradeTime> GetAllTradeTimeFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllTradeTime();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8013";
                string errMsg = "无法从管理中心获取交易时间。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的交易所类型_交易时间
        /// </summary>
        /// <returns>交易所类型_交易时间列表</returns>
        public IList<CM_TradeTime> GetAllTradeTime()
        {
            return tradeTimeObj.GetAll();
        }

        /// <summary>
        /// 获取所有的交易所类型_交易时间
        /// </summary>
        /// <returns>交易所类型_交易时间列表</returns>
        public IList<CM_TradeTime> GetAllTradeTime(bool reLoad)
        {
            return tradeTimeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据TradeTimeID获取交易所类型_交易时间
        /// </summary>
        /// <param name="tradeTimeID">TradeTimeID</param>
        /// <returns>交易所类型_交易时间</returns>
        public CM_TradeTime GetTradeTimeByTradeTimeID(int tradeTimeID)
        {
            return tradeTimeObj.GetByKey(tradeTimeID);
        }

        private IList<CM_TradeTime> GetTradeTimeByBourseTypeIDFromWCF(int bourseTypeID)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetTradeTimeByBourseTypeID(bourseTypeID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8014";
                string errMsg = "无法从管理中心获取交易所交易时间。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        #endregion

        #region TradeWay 交易方向

        private IList<CM_TradeWay> GetAllTradeWayFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllTradeWay();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8015";
                string errMsg = "无法从管理中心获取交易方向。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的交易方向
        /// </summary>
        /// <returns>交易方向列表</returns>
        public IList<CM_TradeWay> GetAllTradeWay()
        {
            return tradeWayObj.GetAll();
        }

        /// <summary>
        /// 获取所有的交易方向
        /// </summary>
        /// <returns>交易方向列表</returns>
        public IList<CM_TradeWay> GetAllTradeWay(bool reLoad)
        {
            return tradeWayObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据TradeWayID获取交易方向
        /// </summary>
        /// <param name="tradeWayID">TradeWayID</param>
        /// <returns>交易方向</returns>
        public CM_TradeWay GetTradeWayByID(int tradeWayID)
        {
            return tradeWayObj.GetByKey(tradeWayID);
        }

        #endregion

        #region AccountType 账户类型

        private IList<UM_AccountType> GetAllAccountTypeFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetALLAccountType();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8016";
                string errMsg = "无法从管理中心获取账户类型列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的账户类型
        /// </summary>
        /// <returns>账户类型</returns>
        public IList<UM_AccountType> GetAllAccountType()
        {
            return accountTypeObj.GetAll();
        }

        /// <summary>
        /// 获取所有的账户类型
        /// </summary>
        /// <returns>账户类型</returns>
        public IList<UM_AccountType> GetAllAccountType(bool reLoad)
        {
            return accountTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据AccountTypeID获取账户类型
        /// </summary>
        /// <param name="accountTypeID">accountTypeID</param>
        /// <returns>账户类型</returns>
        public UM_AccountType GetAccountTypeByID(int accountTypeID)
        {
            return accountTypeObj.GetByKey(accountTypeID);
        }

        #endregion

        #region UnitConversion 现货_品种_交易单位换算

        private IList<CM_UnitConversion> GetAllUnitConversionFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllUnitConversion();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8017";
                string errMsg = "无法从管理中心获取现货品种交易单位换算关系。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的现货_品种_交易单位换算
        /// </summary>
        /// <returns>现货_品种_交易单位换算列表</returns>
        public IList<CM_UnitConversion> GetAllUnitConversion()
        {
            return unitConversionObj.GetAll();
        }

        /// <summary>
        /// 获取所有的现货_品种_交易单位换算
        /// </summary>
        /// <returns>现货_品种_交易单位换算列表</returns>
        public IList<CM_UnitConversion> GetAllUnitConversion(bool reLoad)
        {
            return unitConversionObj.GetAll(reLoad);
        }

        public CM_UnitConversion GetUnitConversionByUnitConversionID(int unitConversionID)
        {
            return unitConversionObj.GetByKey(unitConversionID);
        }

        private IList<CM_UnitConversion> GetUnitConversionByBreedClassIDFromWCF(int breedClassID)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetUnitConversionByBreedClassID(breedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8018";
                string errMsg = "无法根据商品品种编码从管理中心获取现货品种交易单位换算关系。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 根据商品品种编码从管理中心获取现货品种交易单位换算关系
        /// </summary>
        /// <param name="breedClassID"></param>
        /// <returns></returns>
        public IList<CM_UnitConversion> GetUnitConversionByBreedClassID(int breedClassID)
        {
            return unitConvertionByBreedClassIDObj.GetByKey(breedClassID);
        }

        public decimal GetUnitConversionByDetailUnits(int breedClassID, int unitFrom, int unitTo)
        {

            string errCode = "GT-8019";
            string format = "无法从管理中心获取指定的现货品种交易单位换算关系[UntiFrom={0},UnitTo={1}]";
            string errMsg = string.Format(format, unitFrom, unitTo);

            if (unitFrom == unitTo)
                return 1;

            IList<CM_UnitConversion> unitConversions = GetUnitConversionByBreedClassID(breedClassID);

            if (Utils.IsNullOrEmpty(unitConversions))
            {
                throw new VTException(errCode, errMsg);
            }

            //var unitConversions_valid = from unit in unitConversions
            //                            where unit.Value.HasValue && unit.UnitIDFrom.HasValue && unit.UnitIDTo.HasValue
            //                            select unit;

            IList<CM_UnitConversion> unitConversions_valid = new List<CM_UnitConversion>();
            foreach (CM_UnitConversion conversion in unitConversions)
            {
                if (conversion.Value.HasValue && conversion.UnitIDFrom.HasValue && conversion.UnitIDTo.HasValue)
                    unitConversions_valid.Add(conversion);
            }

            if (Utils.IsNullOrEmpty(unitConversions_valid))
            {
                throw new VTException(errCode, errMsg);
            }

            decimal result;
            try
            {

                var query = from unit in unitConversions_valid
                            where unit.UnitIDFrom.Value == unitFrom && unit.UnitIDTo.Value == unitTo
                            select unit.Value;

                if (query.Count() == 0)
                {
                    query = from unit in unitConversions_valid
                            where unit.UnitIDTo.Value == unitFrom && unit.UnitIDFrom.Value == unitTo
                            select unit.Value;

                    if (query.Count() == 0)
                    {
                        throw new VTException(errCode, errMsg);
                    }

                    result = query.First().Value;
                    result = 1 / result;
                }
                else
                {
                    result = query.First().Value;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                throw new VTException(errCode, errMsg);
            }

            return result;

        }
        /// <summary>
        /// 根据港股商品代码和从unitFrom（原则为交易单位）单位转到unitTo（原则为撮合单位）单位的转换倍数据
        /// 如：unitFrom=手,unitTo=股 如某代码倍数为500时则返回500
        /// 如：unitFrom=股,unitTo=手 如某代码倍数为500时则返回1/500
        /// </summary>
        /// <param name="code">港股商品代码</param>
        /// <param name="unitFrom">从单位（如手）交易单位</param>
        /// <param name="unitTo">到单位（股）撮合单位</param>
        /// <returns>返回倍数</returns>
        public decimal GetHKUnitConversionByDetailUnits(string code, Types.UnitType unitFrom, Types.UnitType unitTo)
        {
            string errCode = "GT-8019";
            string format = "无法从管理中心获取指定的港股商品交易单位换算关系[UntiFrom={0},UnitTo={1}]";
            string errMsg = string.Format(format, unitFrom, unitTo);

            if (unitFrom == unitTo)
            {
                return 1;
            }
            #region 港股
            HK_Commodity hkcom = HKStockTradeRulesProxy.GetInstance().GetHKCommodityByCommodityCode(code);
            if (hkcom == null)
            {
                throw new VTException(errCode, errMsg);
            }
            else
            {
                //目前港股只有手与股的转换单位
                if (unitFrom == Types.UnitType.Hand)
                {
                    return hkcom.PerHandThighOrShare.Value;
                }
                else
                {
                    return (decimal)1 / hkcom.PerHandThighOrShare.Value;
                }
            }

            #endregion
        }

        #endregion

        #region Units 单位

        private IList<CM_Units> GetAllUnitsFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllUnits();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8020";
                string errMsg = "无法从管理中心获取交易单位。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的单位
        /// </summary>
        /// <returns>单位列表</returns>
        public IList<CM_Units> GetAllUnits()
        {
            return unitsObj.GetAll();
        }

        /// <summary>
        /// 获取所有的单位
        /// </summary>
        /// <returns>单位列表</returns>
        public IList<CM_Units> GetAllUnits(bool reLoad)
        {
            return unitsObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据UnitsID获取单位
        /// </summary>
        /// <param name="unitsID">UnitsID</param>
        /// <returns>单位</returns>
        public CM_Units GetUnitsByUnitsID(int unitsID)
        {
            return unitsObj.GetByKey(unitsID);
        }

        #endregion

        #region ValueType 交易规则_取值类型

        private IList<CM_ValueType> GetAllValueTypeFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllValueType();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8021";
                string errMsg = "无法从管理中心获取交易规则取值类型。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的交易规则_取值类型
        /// </summary>
        /// <returns>交易规则_取值类型列表</returns>
        public IList<CM_ValueType> GetAllValueType()
        {
            return valueTypeObj.GetAll();
        }

        /// <summary>
        /// 获取所有的交易规则_取值类型
        /// </summary>
        /// <returns>交易规则_取值类型列表</returns>
        public IList<CM_ValueType> GetAllValueType(bool reLoad)
        {
            return valueTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据ValueTypeID获取交易规则_取值类型
        /// </summary>
        /// <param name="valueTypeID">ValueTypeID</param>
        /// <returns>交易规则_取值类型</returns>
        public CM_ValueType GetValueTypeByValueTypeID(int valueTypeID)
        {
            return valueTypeObj.GetByKey(valueTypeID);
        }

        #endregion

        #region BreedClassType 品种类型分类

        private IList<CM_BreedClassType> GetAllBreedClassTypeFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllBreedClassType();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8022";
                string errMsg = "无法从管理中心获取品种类型分类列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }


        /// <summary>
        /// 根据商品代码获取品种类型分类
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>品种类型分类</returns>
        public int? GetBreedClassTypeEnumByCommodityCode(string code)
        {
            #region old code
            //CM_BreedClass breedClass = GetBreedClassByCommodityCode(code);

            //int? result = null;

            //if (breedClass != null)
            //{
            //    result = breedClass.BreedClassTypeID;
            //}

            //return result;
            #endregion
            return GetBreedClassTypeEnumByCommodityCode(code, Types.BreedClassTypeEnum.Stock);
        }

        /// <summary>
        /// 根据商品代码获取品种类型分类
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <returns>品种类型分类</returns>
        public int? GetBreedClassTypeEnumByCommodityCode(string code, Types.BreedClassTypeEnum type)
        {
            CM_BreedClass breedClass = GetBreedClassByCommodityCode(code, type);

            int? result = null;

            if (breedClass != null)
            {
                result = breedClass.BreedClassTypeID;
            }

            return result;
        }

        /// <summary>
        /// 获取所有交易商品品种类型
        /// </summary>
        /// <returns>交易商品品种类型列表</returns>
        public IList<CM_BreedClassType> GetAllBreedClassType()
        {
            return breedClassTypeObj.GetAll();
        }

        /// <summary>
        /// 获取所有交易商品品种类型
        /// </summary>
        /// <returns>交易商品品种类型列表</returns>
        public IList<CM_BreedClassType> GetAllBreedClassType(bool reLoad)
        {
            return breedClassTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据BreedClassTypeID获取BreedClassType
        /// </summary>
        /// <param name="breedClassTypeID">BreedClassTypeID</param>
        /// <returns>BreedClassType</returns>
        public CM_BreedClassType GetBreedClassTypeByBreedClassTypeID(int breedClassTypeID)
        {
            return breedClassTypeObj.GetByKey(breedClassTypeID);
        }

        #endregion

        #region 获取交易所类型CM_BourseType

        private IList<CM_BourseType> GetAllBourseTypeFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllBourseType();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8023";
                string errMsg = "无法从管理中心获取交易所类型列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        private CM_BourseType GetBourseTypeByBourseTypeIDFromWCF(int bourseTypeID)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetBourseTypeByBourseTypeID(bourseTypeID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8024";
                string errMsg = "无法根据交易所类型编码从管理中心获取交易所类型。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的交易所类型
        /// </summary>
        /// <returns>交易所类型列表</returns>
        public IList<CM_BourseType> GetAllBourseType()
        {
            return bourseTypeObj.GetAll();
        }

        /// <summary>
        /// 获取所有的交易所类型
        /// </summary>
        /// <returns>交易所类型列表</returns>
        public IList<CM_BourseType> GetAllBourseType(bool reLoad)
        {
            return bourseTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据交易所类型BourseTypeID获取交易所类型，首先在缓存中查找，找不到则通过WCF服务获取
        /// </summary>
        /// <param name="bourseTypeID">BourseTypeID</param>
        /// <returns>交易所类型</returns>
        public CM_BourseType GetBourseTypeByBourseTypeID(int bourseTypeID)
        {
            return bourseTypeObj.GetByKey(bourseTypeID);
        }

        #endregion

        #region BreedClass 商品品种

        /// <summary>
        /// 获取所有的交易商品品种
        /// </summary>
        /// <returns>交易商品品种列表</returns>
        private IList<CM_BreedClass> GetAllBreedClassFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllBreedClass();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8025";
                string errMsg = "无法从管理中心获取交易商品品种列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 根据商品品种标识breedclassid获取商品品种breedclass
        /// </summary>
        /// <param name="breedClassID">breedclassid</param>
        /// <returns>交易商品品种</returns>
        private CM_BreedClass GetBreedClassByBreedClassIDFromWCF(int breedClassID)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetBreedClassByBreedClassID(breedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8026";
                string errMsg = "无法根据交易商品品种编码从管理中心获取交易商品品种。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 获取所有的交易商品品种
        /// </summary>
        /// <returns>交易商品品种列表</returns>
        public IList<CM_BreedClass> GetAllBreedClass()
        {
            return breedClassObj.GetAll();
        }

        /// <summary>
        /// 获取所有的交易商品品种
        /// </summary>
        /// <returns>交易商品品种列表</returns>
        public IList<CM_BreedClass> GetAllBreedClass(bool reLoad)
        {
            return breedClassObj.GetAll(reLoad);
        }

        /// <summary>
        /// 根据商品品种标识breedclassid获取商品品种breedclass
        /// </summary>
        /// <param name="breedClassID">breedclassid</param>
        /// <returns>交易商品品种</returns>
        public CM_BreedClass GetBreedClassByBreedClassID(int breedClassID)
        {
            return breedClassObj.GetByKey(breedClassID);
        }

        private IList<CM_BreedClass> GetAllBreedClassByBourseTypeFromWCF(int bourseTypeID)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetBreedClassByBourseTypeID(bourseTypeID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8027";
                string errMsg = "无法根据交易所编码从管理中心获取交易商品品种列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 根据交易所类型获取属于此类型的所有交易商品品种
        /// </summary>
        /// <param name="bourseTypeID">bourseTypeID</param>
        /// <returns>交易商品品种列表</returns>
        public IList<CM_BreedClass> GetAllBreedClassByBourseType(int bourseTypeID)
        {
            return breedClassByBourseTypeObj.GetByKey(bourseTypeID);
        }

        /// <summary>
        /// 根据商品代码获取商品品种
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>商品品种</returns>
        public CM_BreedClass GetBreedClassByCommodityCode(string code)
        {
            #region old code
            //CM_Commodity commodity = GetCommodityByCommodityCode(code);

            //CM_BreedClass result = null;

            //if (commodity != null)
            //{
            //    result = GetBreedClassByBreedClassID(commodity.BreedClassID.Value);
            //}

            //return result;
            #endregion

            return GetBreedClassByCommodityCode(code, Types.BreedClassTypeEnum.Stock);
        }

        /// <summary>
        /// 根据商品代码获取商品品种
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <returns>商品品种</returns>
        public CM_BreedClass GetBreedClassByCommodityCode(string code, Types.BreedClassTypeEnum type)
        {
            CM_BreedClass result = null;
            int breedClassID = 0;
            switch (type)
            {
                case Types.BreedClassTypeEnum.Stock:
                case Types.BreedClassTypeEnum.CommodityFuture:
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    CM_Commodity commodity = GetCommodityByCommodityCode(code);
                    if (commodity != null)
                    {
                        breedClassID = commodity.BreedClassID.Value;
                    }
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    HK_Commodity hkCom = MCService.HKTradeRulesProxy.GetHKCommodityByCommodityCode(code);
                    if (hkCom != null)
                    {
                        breedClassID = hkCom.BreedClassID.Value;
                    }
                    break;
            }
            result = GetBreedClassByBreedClassID(breedClassID);
            return result;
        }
        #endregion

        #region FieldRange 字段_范围

        //private IList<CM_FieldRange> GetAllFieldRangeFromWCF()
        //{
        //    try
        //    {
        //        using (CommonParaClient client = GetClient())
        //            return client.GetAllFieldRange();
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8028";
        //        string errMsg = "无法从管理中心获取交易商品字段范围。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}

        //private CM_FieldRange GetFieldRangeByFieldRangeIDFromWCF(int fieldRangeID)
        //{
        //    try
        //    {
        //        using (CommonParaClient client = GetClient())
        //            return client.GetFieldRangeByFieldRangeID(fieldRangeID);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8029";
        //        string errMsg = "无法根据字段范围编码从管理中心获取交易商品字段范围。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}

        ///// <summary>
        ///// 获取所有的字段_范围
        ///// </summary>
        ///// <returns>字段_范围列表</returns>
        //public IList<CM_FieldRange> GetAllFieldRange()
        //{
        //    return fieldRangeObj.GetAll();
        //}

        ///// <summary>
        ///// 获取所有的字段_范围
        ///// </summary>
        ///// <returns>字段_范围列表</returns>
        //public IList<CM_FieldRange> GetAllFieldRange(bool reLoad)
        //{
        //    return fieldRangeObj.GetAll(reLoad);
        //}

        ///// <summary>
        ///// 根据FieldRangeID获取字段_范围
        ///// </summary>
        ///// <param name="fieldRangeID">FieldRangeID</param>
        ///// <returns>字段_范围</returns>
        //public CM_FieldRange GetFieldRangeByFieldRangeID(int fieldRangeID)
        //{
        //    return fieldRangeObj.GetByKey(fieldRangeID);
        //}

        #endregion

        #region MarketParticipation 市场参与度

        //private IList<CM_MarketParticipation> GetAllMarketParticipationFromWCF()
        //{
        //    try
        //    {
        //        using (CommonParaClient client = GetClient())
        //            return client.GetAllMarketParticipation();
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8030";
        //        string errMsg = "无法从管理中心获取市场参与度列表。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}

        //private CM_MarketParticipation GetMarketParticipationByBreedClassIDFromWCF(int breedClassID)
        //{
        //    try
        //    {
        //        using (CommonParaClient client = GetClient())
        //            return client.GetMarketParticipationByBreedClassID(breedClassID);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errCode = "GT-8031";
        //        string errMsg = "无法根据商品类别从管理中心获取市场参与度。";
        //        throw new VTException(errCode, errMsg, ex);
        //    }
        //}


        ///// <summary>
        ///// 获取所有的市场参与度
        ///// </summary>
        ///// <returns>市场参与度列表</returns>
        //public IList<CM_MarketParticipation> GetAllMarketParticipation()
        //{
        //    return marketParticipationObj.GetAll();
        //}

        ///// <summary>
        ///// 获取所有的市场参与度
        ///// </summary>
        ///// <returns>市场参与度列表</returns>
        //public IList<CM_MarketParticipation> GetAllMarketParticipation(bool reLoad)
        //{
        //    return marketParticipationObj.GetAll(reLoad);
        //}

        ///// <summary>
        ///// 根据BreedClassID获取市场参与度
        ///// </summary>
        ///// <param name="breedClassID">BreedClassID</param>
        ///// <returns>市场参与度</returns>
        //public CM_MarketParticipation GetMarketParticipationByBreedClassID(int breedClassID)
        //{
        //    return marketParticipationObj.GetByKey(breedClassID);
        //}

        #endregion

        #region 非交易日

        private IList<CM_NotTradeDate> GetNotTradeDateByBourseTypeIDFromWCF(int bourseTypeID)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetNotTradeDateByBourseTypeID(bourseTypeID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8032";
                string errMsg = "无法从管理中心获取非交易日列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<CM_NotTradeDate> GetNotTradeDateByBourseType(int bourseTypeID)
        {
            return notTradeDateByBourseTypeObj.GetByKey(bourseTypeID);
        }

        /// <summary>
        /// 当前时间是否在交易日内
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>是否在交易日内</returns>
        public bool IsTradeDate(string code)
        {
            return IsTradeDate(code, DateTime.Now);
        }

        /// <summary>
        /// 指定时间是否在交易日内
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="aDate">指定时间</param>
        /// <returns>是否在交易日内</returns>
        public bool IsTradeDate(string code, DateTime aDate)
        {
            bool result = false;
            CM_BourseType bourseType = GetBourseTypeByCommodityCode(code);
            if (bourseType != null)
            {
                result = IsTradeDate(bourseType.BourseTypeID, aDate);
            }

            return result;
        }

        public bool IsTradeDate(int bourseTypeID)
        {
            return IsTradeDate(bourseTypeID, DateTime.Now);
        }

        public bool IsTradeDate(int bourseTypeID, DateTime aDate)
        {
            bool result = true;
            try
            {
                IList<CM_NotTradeDate> notTradeDates = GetNotTradeDateByBourseType(bourseTypeID);

                if (Utils.IsNullOrEmpty(notTradeDates))
                {
                    //string errCode = "GT-8033";
                    //string errMsg = "无法从管理中心获取非交易日列表。";
                    //throw new VTException(errCode, errMsg);

                    return result;
                }

                var dates = from date in notTradeDates
                            where date.NotTradeDay.HasValue
                            select date.NotTradeDay.Value;

                var ds = from d in dates
                         where d.Year == aDate.Year && d.Month == aDate.Month && d.Day == aDate.Day
                         select d;

                if (ds.Count() > 0)
                    result = false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        #endregion

        #region 收盘价
        /// <summary>
        /// 获取所有收盘价目前只是获取现货的
        /// </summary>
        /// <returns>收盘价列表</returns>
        private IList<YesterdayClosePriceInfo> GetClosePriceFromWCF()
        {
            try
            {
                IList<YesterdayClosePriceInfo> data = new List<YesterdayClosePriceInfo>();
                using (CommonParaClient client = GetClient())
                {
                    List<ClosePriceInfo> list = client.GetAllClosePriceInfoByBreedClassTypeID((int)Types.BreedClassTypeEnum.Stock);
                    if (list == null)
                    {
                        return data;
                    }
                    foreach (var item in list)
                    {
                        YesterdayClosePriceInfo model = new YesterdayClosePriceInfo();
                        model.Code = item.StockCode;
                        model.ClosePrice = item.ClosePrice;
                        data.Add(model);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                string errCode = "GT-8001";
                string errMsg = "无法从管理中心获取收盘价列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }
        /// <summary>
        /// 根据商品代码获取昨日收盘价
        /// </summary>
        /// <returns>收盘价</returns>
        public decimal GetClosePriceByCode(string code)
        {
            decimal price = 0;
            YesterdayClosePriceInfo model = closePriceInfoObj.GetByKey(code);
            if (model != null)
            {
                price = model.ClosePrice;
            }
            return price;
        }

        #endregion

        /// <summary>
        /// 根据交易员获取拥有的交易品种权限
        /// </summary>
        /// <param name="userID">交易员</param>
        /// <returns>交易品种权限</returns>
        private IList<UM_DealerTradeBreedClass> GetTransactionRightTableFromWCF(int userID)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.TransactionRightTable(userID);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8028";
                string errMsg = "无法根据交易员从管理中心获取对应的品种权限。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<UM_DealerTradeBreedClass> GetTransactionRightTable(int userID)
        {
            return dealerTraderObj.GetByKey(userID);
        }

        #endregion

        #region 撮合相关

        private IList<RC_MatchCenter> GetAllMatchCenterFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllMatchCenter();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8034";
                string errMsg = "无法从管理中心获取撮合中心列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<RC_MatchCenter> GetAllMatchCenter()
        {
            return matchCenterObj.GetAll();
        }

        public IList<RC_MatchCenter> GetAllMatchCenter(bool reLoad)
        {
            return matchCenterObj.GetAll(reLoad);
        }

        public RC_MatchCenter GetMatchCenterByMatchCenterID(int id)
        {
            return matchCenterObj.GetByKey(id);
        }

        private IList<RC_MatchMachine> GetAllMatchMachineFromWCF()
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetAllMatchMachine();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8035";
                string errMsg = "无法从管理中心获取撮合机列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        public IList<RC_MatchMachine> GetAllMatchMachine()
        {
            return matchMachineObj.GetAll();
        }

        public IList<RC_MatchMachine> GetAllMatchMachine(bool reLoad)
        {
            return matchMachineObj.GetAll(reLoad);
        }

        public RC_MatchMachine GetMatchMachineByMatchMachinerID(int id)
        {
            return matchMachineObj.GetByKey(id);
        }

        private RC_MatchMachine GetMatchMachineByCommodityCodeFromWCF(string code)
        {
            try
            {
                using (CommonParaClient client = GetClient())
                    return client.GetMatchMachinebyCommodity(code);
            }
            catch (Exception ex)
            {
                string errCode = "GT-8036";
                string errMsg = "无法根据商品代码从管理中心获取其所属撮合机。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 根据商品代码获取撮合机
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public RC_MatchMachine GetMatchMachineByCommodityCode(string code)
        {
            return matchineByCommodityObj.GetByKey(code);
        }

        /// <summary>
        /// 根据商品代码获取对应的撮合中心
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>撮合中心</returns>
        public RC_MatchCenter GetMatchCenterByCommodityCode(string code)
        {
            RC_MatchCenter result = null;

            RC_MatchMachine machine = GetMatchMachineByCommodityCode(code);
            if (machine != null)
            {
                result = GetMatchCenterByMatchCenterID(machine.MatchCenterID.Value);
            }

            return result;
        }

        #endregion
    }
}