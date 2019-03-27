#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateValidate;
using ReckoningCounter.BLL.DelegateValidate.Cost;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.Entity;
using ReckoningCounter.DAL.HKTradingRulesService;
using RealTime.Server.SModelData.HqData;
using System.Timers;
using System.Threading;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Entity.Model.QH;

#endregion

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 管理中心服务类，错误编码范围8400-8480
    /// 作者：宋涛
    /// 日期：2008-12-10
    /// Desc.:增加了商品期货新品种、新合约、交割月判断方法
    /// Update By:董鹏
    /// Update Date:2010-01-26
    /// Desc.: 增加了股指期货"季月合约上市首日涨跌幅","合约最后交易日涨跌幅"的判断方法
    /// Update By:董鹏
    /// Update Date:2010-03-05
    /// Desc.: 修改清算所用的初始化今日结算价
    /// Update By:李健华
    /// Update Date:2010-03-16
    /// </summary>
    public static class MCService
    {
        public static CommonParaProxy CommonPara = CommonParaProxy.GetInstance();
        public static FuturesTradeRulesProxy FuturesTradeRules = FuturesTradeRulesProxy.GetInstance();
        private static SyncCache<string, decimal> futureTodayCloseCache = new SyncCache<string, decimal>();
        private static SyncCache<string, decimal> futureYesterdayCloseCache = new SyncCache<string, decimal>();
        /// <summary>
        /// 涨跌幅处理器实例
        /// </summary>
        public static HighLowRangeProcessor HLRangeProcessor = new HighLowRangeProcessor();
        //private static SyncCache<string, bool> newStockCache = new SyncCache<string, bool>();
        /// <summary>
        /// 现货交易规则单例实例
        /// </summary>
        public static SpotTradeRulesProxy SpotTradeRules = SpotTradeRulesProxy.GetInstance();

        //private static SyncCache<string, bool> zfCache = new SyncCache<string, bool>();
        ///// <summary>
        ///// 当前系统当前日期当前月的所有交易日(每日开盘初始化一次,目前只考虑所有的交易所共用，不考虑交易所不共用，如果这样考虑再使用SyncCache缓存)
        ///// </summary>
        //private static List<DateTime> CurrentMothDay = new List<DateTime>();

        /// <summary>
        /// 港股交易规则类实例
        /// </summary>
        public static HKStockTradeRulesProxy HKTradeRulesProxy = HKStockTradeRulesProxy.GetInstance();

        ///// <summary>
        ///// 是否已经全部获取到所有的昨日结算价
        ///// </summary>
        //private static bool isGetYesPrice = true;
        /// <summary>
        /// 定时获取期货昨日结算价
        /// </summary>
        private static System.Timers.Timer timer = null;

        #region 费用计算接口

        /// <summary>
        /// 获取现货交易费用
        /// </summary>
        /// <param name="request">现货委托</param>
        /// <returns>现货交易费用结果</returns>
        public static XHCostResult ComputeXHCost(StockOrderRequest request)
        {
            return CostCalculator.ComputeXHCost(request);
        }
        /// <summary>
        /// 获取现货交易费用
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="unitType">委托单位类型</param>
        /// <param name="buySell">买卖方向</param>
        /// <returns>现货交易费用结果</returns>
        public static XHCostResult ComputeXHCost(string code, float price, int amount, Types.UnitType unitType,
                                                 Types.TransactionDirection buySell)
        {
            return CostCalculator.ComputeXHCost(code, price, amount, unitType, buySell);
        }
        /// <summary>
        /// 获取港股交易费用
        /// </summary>
        /// <param name="request">港股委托</param>
        /// <returns>港股交易费用结果</returns>
        public static HKCostResult ComputeHKCost(HKOrderRequest request)
        {
            return CostCalculator.ComputeHKCost(request);
        }
        /// <summary>
        /// 获取港股交易费用
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="unitType">委托单位类型</param>
        /// <param name="buySell">买卖方向</param>
        /// <returns>港股交易费用结果</returns>
        public static HKCostResult ComputeHKCost(string code, float price, int amount, Types.UnitType unitType,
                                                 Types.TransactionDirection buySell)
        {
            return CostCalculator.ComputeHKCost(code, price, amount, unitType, buySell);
        }

        #region 商品期货
        /// <summary>
        /// 获取商品期货交易费用
        /// </summary>
        /// <param name="request">期货委托</param>
        /// <returns>期货交易费用结果</returns>
        public static QHCostResult ComputeSPQHCost(MercantileFuturesOrderRequest request)
        {
            return CostCalculator.ComputeSPQHCost(request);
        }

        /// <summary>
        /// 获取商品期货交易费用
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="unitType">委托单位类型</param>
        /// <param name="openCloseType">开平仓方向</param>
        /// <returns>期货交易费用结果</returns>
        public static QHCostResult ComputeSPQHCost(string code, float price, int amount, Types.UnitType unitType,
                                                 Entity.Contants.Types.FutureOpenCloseType openCloseType)
        {
            return CostCalculator.ComputeSPQHCost(code, price, amount, unitType, openCloseType);
        }
        #endregion

        #region 股指期货
        /// <summary>
        /// 获取股指期货交易费用
        /// </summary>
        /// <param name="request">股指期货委托</param>
        /// <returns>股指期货交易费用结果</returns>
        public static QHCostResult ComputeGZQHCost(StockIndexFuturesOrderRequest request)
        {
            return CostCalculator.ComputeGZQHCost(request);
        }

        /// <summary>
        /// 获取股指期货交易费用
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="unitType">委托单位类型</param>
        /// <param name="openCloseType">开平仓方向</param>
        /// <returns>股指期货交易费用结果</returns>
        public static QHCostResult ComputeGZQHCost(string code, float price, int amount, Types.UnitType unitType,
                                                   Entity.Contants.Types.FutureOpenCloseType openCloseType)
        {
            return CostCalculator.ComputeGZQHCost(code, price, amount, unitType, openCloseType);
        }
        #endregion

        /// <summary>
        /// 获取保本价
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="costPrice">成本价</param>
        /// <param name="amount">总持仓量</param>
        /// <param name="buySellType">买卖方向（仅期货有用）</param>
        /// <returns>保本价</returns>
        public static decimal GetHoldPrice(string code, decimal costPrice, decimal amount,
                                           Types.TransactionDirection buySellType)
        {
            return CostCalculator.GetHoldPrice(code, costPrice, amount, buySellType);
        }

        /// <summary>
        /// 获取保本价
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="costPrice">成本价</param>
        /// <param name="amount">总持仓量</param>
        /// <param name="buySellTypeId">买卖方向（仅期货有用）</param>
        /// <returns>保本价</returns>
        public static decimal GetHoldPrice(string code, decimal costPrice, decimal amount, int buySellTypeId)
        {
            Types.TransactionDirection buySellType = (Types.TransactionDirection)buySellTypeId;
            return CostCalculator.GetHoldPrice(code, costPrice, amount, buySellType);
        }

        /// <summary>
        /// 获取保本价(仅供现货使用)
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="costPrice">成本价</param>
        /// <param name="amount">总持仓量</param>
        /// <returns>保本价</returns>
        public static decimal GetHoldPrice(string code, decimal costPrice, decimal amount)
        {
            Types.TransactionDirection buySellType = Types.TransactionDirection.Buying;
            return CostCalculator.GetHoldPrice(code, costPrice, amount, buySellType);
        }

        #endregion

        //public static TransactionManageProxy TransactionManage = TransactionManageProxy.GetInstance();

        /// <summary>
        /// 获取指定代码的计价单位
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>计价单位</returns>
        public static int GetPriceUnit(string code)
        {
            string errCode = "GT-8400";
            string errMsg = "无法获取指定商品代码的计价单位。";

            int? ID = CommonPara.GetBreedClassIdByCommodityCode(code);
            int? type = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            if (type.HasValue)
            {
                int bType = type.Value;
                int bID = ID.Value;

                switch (bType)
                {
                    //现货
                    case 1:
                        XH_SpotTradeRules rules = SpotTradeRules.GetSpotTradeRulesByBreedClassID(bID);
                        if (rules == null)
                        {
                            throw new VTException(errCode, errMsg);
                        }

                        //获取计价单位
                        return rules.PriceUnit;

                    //期货
                    case 2:
                    //股指期货
                    case 3:
                        QH_FuturesTradeRules futureRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(bID);

                        return futureRules.PriceUnit;
                }
            }

            throw new VTException(errCode, errMsg);
        }

        /// <summary>
        /// 根据商品代码，以及指定交易单位类型获取指定交易单位和计价单位的倍数
        /// 传入000001，手，返回100（股）
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="unitType">交易单位类型</param>
        /// <returns>倍数</returns>
        public static decimal GetTradeUnitScale(string code, Types.UnitType unitType)
        {
            #region old code
            //string errCode = "GT-8401";
            //string errMsg = "无法获取指定商品代码的交易单位与计价单位之间的倍数。";

            //int? ID = CommonPara.GetBreedClassIdByCommodityCode(code);
            //int? type = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            //if (type.HasValue)
            //{
            //    int bType = type.Value;
            //    int bID = ID.Value;

            //    switch (bType)
            //    {
            //        //现货
            //        case 1:
            //            XH_SpotTradeRules rules = SpotTradeRules.GetSpotTradeRulesByBreedClassID(bID);
            //            if (rules == null)
            //            {
            //                throw new VTException(errCode, errMsg);
            //            }

            //            //获取计价单位
            //            int priceUnit = rules.PriceUnit;
            //            return CommonPara.GetUnitConversionByDetailUnits(bID, (int)unitType, priceUnit);

            //        //期货
            //        case 2:
            //        //股指期货
            //        case 3:
            //            QH_FuturesTradeRules futureRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(bID);
            //            //int unit = (int) unitType;

            //            //if (!futureRules.UnitsID.HasValue)
            //            //    throw new VTException(errCode, errMsg);

            //            //if (unit != futureRules.UnitsID.Value)
            //            //    throw new VTException(errCode, errMsg);

            //            //if(unit != futureRules.PriceUnit)
            //            //    throw new VTException(errCode, errMsg);

            //            if (!futureRules.UnitMultiple.HasValue)
            //                throw new VTException(errCode, errMsg);

            //            return futureRules.UnitMultiple.Value;
            //    }
            //}

            //throw new VTException(errCode, errMsg);
            #endregion
            return GetTradeUnitScale(Types.BreedClassTypeEnum.Stock, code, unitType);
        }

        /// <summary>
        /// 根据商品代码，以及指定交易单位类型获取指定交易单位和计价单位的倍数
        /// 传入000001，手，返回100（股）（注：这里即指定的交易单位）这里会通过代码获取交易规则的计价单位
        /// 返回的交易单位转成计价单位的倍数
        /// </summary>
        /// <param name="type">商品代码所属类型，这里只是为了区别港股</param>
        /// <param name="code">商品代码</param>
        /// <param name="unitType">交易单位类型</param>
        /// <returns>倍数</returns>
        public static decimal GetTradeUnitScale(Types.BreedClassTypeEnum type, string code, Types.UnitType unitType)
        {
            string errCode = "GT-8401";
            string errMsg = "无法获取指定商品代码的交易单位与计价单位之间的倍数。";

            CM_BreedClass cmBreed = CommonPara.GetBreedClassByCommodityCode(code, type);
            if (cmBreed != null || cmBreed.BreedClassTypeID.HasValue)
            {

                switch ((Types.BreedClassTypeEnum)cmBreed.BreedClassTypeID.Value)
                {
                    case Types.BreedClassTypeEnum.Stock:  //现货
                        #region 现货
                        XH_SpotTradeRules rules = SpotTradeRules.GetSpotTradeRulesByBreedClassID(cmBreed.BreedClassID);
                        if (rules == null)
                        {
                            throw new VTException(errCode, errMsg);
                        }
                        //获取计价单位
                        int priceUnit = rules.PriceUnit;
                        return CommonPara.GetUnitConversionByDetailUnits(cmBreed.BreedClassID, (int)unitType, priceUnit);
                        #endregion
                    //break;
                    case Types.BreedClassTypeEnum.CommodityFuture://商品期货
                    case Types.BreedClassTypeEnum.StockIndexFuture:  //股指期货
                        #region 股指（商品）期货
                        QH_FuturesTradeRules futureRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(cmBreed.BreedClassID);
                        if (!futureRules.UnitMultiple.HasValue)
                            throw new VTException(errCode, errMsg);

                        return futureRules.UnitMultiple.Value;
                        #endregion
                    //break;
                    case Types.BreedClassTypeEnum.HKStock:
                        #region 港股
                        HK_Commodity hkcom = HKTradeRulesProxy.GetHKCommodityByCommodityCode(code);
                        if (hkcom == null)
                        {
                            throw new VTException(errCode, errMsg);
                        }
                        else
                        {
                            //目前港股只有手与股的转换单位，所以之间的倍数为代码表中的值倍数值
                            //而代码表中的倍数值已经固定为1手=X股（X即为倍数）
                            if (unitType == Types.UnitType.Hand)
                            {
                                return hkcom.PerHandThighOrShare.Value;
                            }
                            else
                            {
                                return 1;
                            }
                        }

                        #endregion
                    //break;
                    default:
                        break;
                }
            }
            throw new VTException(errCode, errMsg);
        }

        /// <summary>
        /// 根据商品代码获取期货交易单位计价单位倍数-期货是合约乘数300
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>倍数</returns>
        public static decimal GetFutureTradeUntiScale(string code)
        {
            string errCode = "GT-8401";
            string errMsg = "无法获取指定商品代码的交易单位与计价单位之间的倍数。";

            int? ID = CommonPara.GetBreedClassIdByCommodityCode(code);
            int? type = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            if (type.HasValue)
            {
                int bID = ID.Value;
                QH_FuturesTradeRules futureRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(bID);

                if (!futureRules.UnitMultiple.HasValue)
                {
                    throw new VTException(errCode, errMsg);
                }

                return futureRules.UnitMultiple.Value;
            }

            return 1;
        }

        /// <summary>
        /// 根据商品代码获取期货交易单位
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>期货交易单位</returns>
        public static int GetFutureTradeUnit(string code)
        {
            string errCode = "GT-8401";
            string errMsg = "无法获取指定商品代码的交易单位与计价单位之间的倍数。";

            int? ID = CommonPara.GetBreedClassIdByCommodityCode(code);
            int? type = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            if (type.HasValue)
            {
                int bID = ID.Value;
                QH_FuturesTradeRules futureRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(bID);

                if (!futureRules.UnitMultiple.HasValue)
                    throw new VTException(errCode, errMsg);

                int? unit = futureRules.UnitsID;
                if (!unit.HasValue)
                    throw new VTException(errCode, errMsg);

                return unit.Value;
            }

            return 1;
        }

        /// <summary>
        /// 根据商品代码获取撮合单位和计价单位的倍数
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>倍数</returns>
        public static decimal GetMatchUnitScale(string code)
        {
            #region old code
            //Types.UnitType matchType = GetMatchUnitType(code);

            //return GetTradeUnitScale(code, matchType);
            #endregion
            return GetMatchUnitScale(Types.BreedClassTypeEnum.Stock, code);
        }

        /// <summary>
        /// 根据商品代码获取撮合单位和计价单位的倍数
        /// </summary>
        /// <param name="type">商品代码所属类型，这里只是为了区别港股</param>
        /// <param name="code">商品代码</param>
        /// <returns>倍数</returns>
        public static decimal GetMatchUnitScale(Types.BreedClassTypeEnum type, string code)
        {
            Types.UnitType matchType = GetMatchUnitType(code, type);

            return GetTradeUnitScale(type, code, matchType);
        }

        /// <summary>
        /// 根据商品代码返回其对应的撮合单位（不能用于港股查询）
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>撮合单位</returns>
        public static Types.UnitType GetMatchUnitType(string code)
        {
            return GetMatchUnitType(code, Types.BreedClassTypeEnum.Stock);
        }

        /// <summary>
        /// 根据商品代码返回其对应的撮合单位
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <returns>撮合单位</returns>
        public static Types.UnitType GetMatchUnitType(string code, Types.BreedClassTypeEnum type)
        {
            string errCode = "GT-8402";
            string errMsg = "无法根据商品代码获取其对应的撮合单位。";
            //int? breedClassType = null;
            #region old code
            //switch (type)
            //{
            //    case Types.BreedClassTypeEnum.Stock:
            //    case Types.BreedClassTypeEnum.CommodityFuture:
            //    case Types.BreedClassTypeEnum.StockIndexFuture:
            //        breedClassType = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            //        break;
            //    case Types.BreedClassTypeEnum.HKStock:
            //        breedClassType = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            //        break;
            //}

            //if (!breedClassType.HasValue)
            //    throw new VTException(errCode, errMsg);

            #endregion

            #region 获取商品所属品种类别 breedClassTypeEnum
            Types.BreedClassTypeEnum breedClassTypeEnum;
            int rulesbID;
            try
            {
                CM_BreedClass cm_BreedClass = CommonPara.GetBreedClassByCommodityCode(code, type);
                if (cm_BreedClass == null || !cm_BreedClass.BreedClassTypeID.HasValue)
                {
                    throw new VTException(errCode, "无法根据交易所编码从管理中心获取交易商品品种列表。");
                }
                breedClassTypeEnum = (Types.BreedClassTypeEnum)cm_BreedClass.BreedClassTypeID.Value;
                rulesbID = cm_BreedClass.BreedClassID;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                throw new VTException(errCode, errMsg);
            }
            #endregion

            //int? ID = CommonPara.GetBreedClassIdByCommodityCode(code);
            //if (!ID.HasValue)
            //    throw new VTException(errCode, errMsg);

            switch (breedClassTypeEnum)
            {
                case Types.BreedClassTypeEnum.Stock:
                    #region 获取现货撮合单位
                    XH_SpotTradeRules rules = SpotTradeRules.GetSpotTradeRulesByBreedClassID(rulesbID);
                    if (rules != null)
                    {
                        int macthTypeID = rules.MarketUnitID;
                        try
                        {
                            Types.UnitType matchType = (Types.UnitType)macthTypeID;
                            return matchType;
                        }
                        catch (Exception ex)
                        {
                            throw new VTException(errCode, errMsg, ex);
                        }
                    }
                    throw new VTException(errCode, errMsg);
                    #endregion
                case Types.BreedClassTypeEnum.CommodityFuture:
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    #region 获取期货撮合单位
                    QH_FuturesTradeRules futuresTradeRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(rulesbID);
                    if (futuresTradeRules != null)
                    {
                        //TODO:返回那个？MarketUnitID,UnitsID
                        int macthTypeID = futuresTradeRules.UnitsID.Value;
                        try
                        {
                            Types.UnitType matchType = (Types.UnitType)macthTypeID;
                            return matchType;
                        }
                        catch (Exception ex)
                        {
                            throw new VTException(errCode, errMsg, ex);
                        }
                    }
                    throw new VTException(errCode, errMsg);
                    #endregion
                case Types.BreedClassTypeEnum.HKStock:
                    #region 获取港股撮合单位
                    HK_SpotTradeRules hkSoptTradeRules = HKTradeRulesProxy.GetSpotTradeRulesByBreedClassID(rulesbID);
                    if (hkSoptTradeRules != null)
                    {
                        int macthTypeID = hkSoptTradeRules.MarketUnitID.Value;
                        try
                        {
                            Types.UnitType matchType = (Types.UnitType)macthTypeID;
                            return matchType;
                        }
                        catch (Exception ex)
                        {
                            throw new VTException(errCode, errMsg, ex);
                        }
                    }
                    throw new VTException(errCode, errMsg);
                    #endregion
            }

            throw new VTException(errCode, errMsg);
        }


        #region 新股与增发股
        /// <summary>
        /// 是否是新股
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>是否是新股</returns>
        public static bool InternalIsNewStock(string code)
        {
            bool result = false;

            result = CommonPara.GetNewCommodityByCommodityCode(code);
            //if (commodity != null)
            //{
            //    result = true;
            //}


            return result;
        }

        /// <summary>
        /// 是否是新股
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>是否是新股</returns>
        public static bool IsNewStock(string code)
        {
            bool result = false;

            //if (!newStockCache.TryGetValue(code, out result))
            //{
            LogHelper.WriteDebug("Debug_Test-003:当前股票：" + code + "缓存中没有找到是新股上市" + DateTime.Now);
            result = InternalIsNewStock(code);
            LogHelper.WriteDebug("Debug_Test-003:当前股票：" + code + "重新加入新股缓存列表" + result);
            //newStockCache.AddOrUpdate(code, result);
            //}

            return result;
        }
        /// <summary>
        /// 是否是增发新股
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>是否是增发新股</returns>
        public static bool InternalIsZF(string code)
        {
            bool result = false;

            result = CommonPara.GetZFCommodityByCommodityCode(code);
            //if (zfInfo != null)
            //    result = true;

            return result;
        }

        /// <summary>
        /// 是否是增发新股
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>是否是增发新股</returns>
        public static bool IsZF(string code)
        {
            bool result = false;

            //if (!zfCache.TryGetValue(code, out result))
            //{
            result = InternalIsZF(code);

            //    zfCache.AddOrUpdate(code, result);
            //}

            return result;
        }
        #endregion


        /// <summary>
        /// 每天收市需要进行的处理
        /// </summary>
        public static void DoMarketCloseJob()
        {
            InitializeFutureTodayPreSettlementPrice();
        }

        /// <summary>
        /// 用于清算时直接从数据库中获取数据初始化当日结算价
        /// </summary>
        public static void IniFutureTodayPreSettlementPriceRecovery()
        {
            futureTodayCloseCache.Reset();
            QH_TodaySettlementPriceDal dal = new QH_TodaySettlementPriceDal();
            List<QH_TodaySettlementPriceInfo> list = dal.GetListArray("");

            foreach (var item in list)
            {
                if (item.SettlementPrice > 0)
                {
                    if (futureTodayCloseCache.Contains(item.CommodityCode))
                    {
                        futureTodayCloseCache.Delete(item.CommodityCode);
                    }
                    futureTodayCloseCache.AddOrUpdate(item.CommodityCode, item.SettlementPrice);
                }
            }
        }

        /// <summary>
        /// 获取所有期货的今日收盘结算价
        /// </summary>
        public static void InitializeFutureTodayPreSettlementPrice()
        {
            List<int> bIdList = GetAllFuturesBreedClassId();

            foreach (var bId in bIdList)
            {
                var commodities = CommonPara.GetAllCommodityByBreedClass(bId);
                foreach (var commodity in commodities)
                {
                    //过期的不获取
                    if (commodity.IsExpired == (int)Types.IsYesOrNo.Yes)
                        continue;

                    decimal price = 0;
                    string strMsg = "";

                    bool isSuccess = GetFutureTodayPreSettlementPrice(commodity.CommodityCode, out price, ref strMsg);

                    //失败再获取一次
                    if (!isSuccess)
                        isSuccess = GetFutureTodayPreSettlementPrice(commodity.CommodityCode, out price, ref strMsg);

                    //失败再获取一次
                    if (!isSuccess)
                        isSuccess = GetFutureTodayPreSettlementPrice(commodity.CommodityCode, out price, ref strMsg);

                }
            }
        }
        /// <summary>
        /// 在缓存中获取期货的今日收盘结算价
        /// </summary>
        /// <param name="contract">合约名称</param>
        /// <param name="price">今日收盘结算价</param>
        /// <param name="msg"></param>
        /// <returns>是否能获得</returns>
        public static bool GetFutureTodayPreSettlementPriceByCache(string contract, out decimal price)
        {
            if (futureTodayCloseCache.TryGetValue(contract, out price))
            {
                if (price > 0)
                {
                    return true;
                }

                futureTodayCloseCache.Delete(contract);
            }
            return false;

        }
        /// <summary>
        /// 提供故障恢复清除今日结算价的缓存数据
        /// </summary>
        public static void ClearFuterTodayPreSettlemmentPrice()
        {
            futureTodayCloseCache.Reset();
        }

        /// <summary>
        /// 获取期货的今日收盘结算价
        /// </summary>
        /// <param name="contract">合约名称</param>
        /// <param name="price">今日收盘结算价</param>
        /// <param name="msg"></param>
        /// <returns>是否能获得</returns>
        public static bool GetFutureTodayPreSettlementPrice(string contract, out decimal price, ref string msg)
        {
            if (futureTodayCloseCache.TryGetValue(contract, out price))
            {
                if (price > 0)
                {
                    return true;
                }

                futureTodayCloseCache.Delete(contract);
            }

            string format = "无法获取合约{0}的今日收盘结算价SettlementPrice";
            msg = string.Format(format, contract);

            int enumType = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(contract).Value;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();

            switch ((Types.BreedClassTypeEnum)enumType)
            {

                case Types.BreedClassTypeEnum.CommodityFuture:
                    MerFutData merData = service.GetMercantileFutData(contract);
                    if (merData == null)
                    {
                        LogHelper.WriteInfo("MCService.GetFutureTodayPreSettlementPrice" + msg + "-futdata=null");
                        return false;
                    }
                    //今日收盘结算价
                    try
                    {
                        price = (decimal)merData.ClearPrice;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message, ex);
                    }

                    break;
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    FutData futData = service.GetFutData(contract);
                    if (futData == null)
                    {
                        LogHelper.WriteInfo("MCService.GetFutureTodayPreSettlementPrice" + msg + "-futdata=null");
                        return false;
                    }
                    //今日收盘结算价
                    try
                    {
                        price = (decimal)futData.SettlementPrice;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message, ex);
                    }
                    break;

            }

            if (price <= 0)
            {
                LogHelper.WriteInfo("MCService.GetFutureTodayPreSettlementPrice" + msg + "-price=" + price);

                return false;
            }

            string format2 = "获取合约{0}的今日收盘结算价SettlementPrice={1}";
            string strMessage = string.Format(format2, contract, price);
            LogHelper.WriteDebug(strMessage);

            futureTodayCloseCache.AddOrUpdate(contract, price);
            msg = "";

            return true;
        }

        /// <summary>
        /// 获取所有期货的昨日结算价
        /// </summary>
        private static void InitializeFutureYesterdayPreSettlementPrice()
        {
            List<int> bIdList = GetAllFuturesBreedClassId();
            List<string> noGetList = new List<string>();
            foreach (var bId in bIdList)
            {
                var commodities = CommonPara.GetAllCommodityByBreedClass(bId);
                foreach (var commodity in commodities)
                {
                    //过期的不获取
                    if (commodity.IsExpired == (int)Types.IsYesOrNo.Yes)
                        continue;

                    decimal price = 0;
                    string strMsg = "";

                    bool isSuccess = GetFutureYesterdayPreSettlementPrice(commodity.CommodityCode, out price, ref strMsg);

                    //失败再获取一次
                    if (!isSuccess)
                        isSuccess = GetFutureYesterdayPreSettlementPrice(commodity.CommodityCode, out price, ref strMsg);

                    //失败再获取一次
                    if (!isSuccess)
                        isSuccess = GetFutureYesterdayPreSettlementPrice(commodity.CommodityCode, out price, ref strMsg);

                    if (!isSuccess)
                    {
                        noGetList.Add(commodity.CommodityCode);
                    }
                }
            }
            if (!Utils.IsNullOrEmpty(noGetList))
            {
                GetQHYesterdayPriceTimer(noGetList);
            }
        }


        /// <summary>
        /// 获取期货昨日结算价事件不停的执行，执行到不每个代码都获取到为止
        /// </summary>
        private static void GetQHYesterdayPriceTimer(List<string> list)
        {
            try
            {
                if (timer != null)
                {
                    timer.Enabled = false;
                    timer = null;
                }


                timer = new System.Timers.Timer();
                timer.Interval = 10 * 60 * 1000;
                timer.Elapsed += delegate
                {
                    Thread thread = new Thread(delegate()
                    {
                        try
                        {
                            List<string> noGetList = new List<string>();
                            foreach (var item in list)
                            {
                                decimal price = 0;
                                string strMsg = "";
                                bool isSuccess = false;
                                isSuccess = GetFutureYesterdayPreSettlementPrice(item, out price, ref strMsg);
                                if (!isSuccess)
                                {
                                    CM_Commodity com = MCService.CommonPara.GetCommodityByCommodityCode(item);
                                    //如果代码过期不添加到要断续获取
                                    if (com != null && com.IsExpired != 1)
                                    {
                                        noGetList.Add(item);
                                    }
                                }
                            }
                            if (!Utils.IsNullOrEmpty(noGetList))
                            {
                                GetQHYesterdayPriceTimer(noGetList);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    });
                    thread.Start();
                    timer.Enabled = false;
                };

                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

        }

        /// <summary>
        /// 获取所有期货品种的BreedClassId
        /// </summary>
        /// <returns></returns>
        private static List<int> GetAllFuturesBreedClassId()
        {
            var bList = CommonPara.GetAllBreedClass();

            var bIDList = new List<int>();
            foreach (var breedClass in bList)
            {
                if (breedClass.BreedClassTypeID.HasValue)
                {
                    if (breedClass.BreedClassTypeID.Value == (int)Types.BreedClassTypeEnum.StockIndexFuture)
                    {
                        bIDList.Add(breedClass.BreedClassID);
                    }

                    if (breedClass.BreedClassTypeID.Value == (int)Types.BreedClassTypeEnum.CommodityFuture)
                    {
                        bIDList.Add(breedClass.BreedClassID);
                    }
                }
            }
            return bIDList;
        }


        /// <summary>
        /// 获取期货的昨日结算价(包括商品期货、股指期货)
        /// </summary>
        /// <param name="contract">合约名称</param>
        /// <param name="price">昨日结算价</param>
        /// <param name="msg">错误信息</param>
        /// <returns>是否能获得</returns>
        public static bool GetFutureYesterdayPreSettlementPrice(string contract, out decimal price, ref string msg)
        {
            if (futureYesterdayCloseCache.TryGetValue(contract, out price))
            {
                if (price > 0)
                {
                    return true;
                }

                futureYesterdayCloseCache.Delete(contract);
            }

            string format = "无法获取合约{0}的昨日结算价PreSettlementPrice";
            msg = string.Format(format, contract);

            int enumType = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(contract).Value;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();

            switch ((Types.BreedClassTypeEnum)enumType)
            {

                case Types.BreedClassTypeEnum.CommodityFuture:
                    MerFutData merData = service.GetMercantileFutData(contract);
                    if (merData == null)
                    {
                        LogHelper.WriteInfo("MCService.GetFutureYesterdayPreSettlementPrice" + msg + "-futdata=null");
                        return false;
                    }
                    //昨日结算价
                    price = (decimal)merData.PreClearPrice;

                    break;
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    FutData futData = service.GetFutData(contract);
                    if (futData == null)
                    {
                        LogHelper.WriteInfo("MCService.GetFutureYesterdayPreSettlementPrice" + msg + "-futdata=null");
                        return false;
                    }
                    //昨日结算价
                    price = (decimal)futData.PreSettlementPrice;
                    break;

            }

            if (price <= 0)
            {
                LogHelper.WriteInfo("MCService.GetFutureYesterdayPreSettlementPrice" + msg + "-price=" + price);
                return false;
            }

            string format2 = "获取合约{0}的昨日结算价PreSettlementPrice={1}";
            string strMessage = string.Format(format2, contract, price);
            LogHelper.WriteDebug(strMessage);

            futureYesterdayCloseCache.AddOrUpdate(contract, price);
            msg = "";

            return true;
        }

        public static void Reset()
        {
            SpotTradeRules.Reset();
            SpotTradeRules.Initialize();

            FuturesTradeRules.Reset();
            FuturesTradeRules.Initialize();

            //newStockCache.Reset();
            //zfCache.Reset();

            futureYesterdayCloseCache.Reset();
            futureTodayCloseCache.Reset();

            //港股规则
            HKTradeRulesProxy.Reset();
            HKTradeRulesProxy.Initialize();



            InitializeFutureYesterdayPreSettlementPrice();
        }




        /// <summary>
        /// 进行委托类型转换，目前期货委托和股指期货委托实际内容一样
        /// </summary>
        /// <param name="request">股指期货委托</param>
        /// <returns>期货委托</returns>
        public static MercantileFuturesOrderRequest GetFuturesOrderRequest(StockIndexFuturesOrderRequest request)
        {
            MercantileFuturesOrderRequest futuresOrderRequest = new MercantileFuturesOrderRequest();
            futuresOrderRequest.BuySell = request.BuySell;
            futuresOrderRequest.ChannelID = request.ChannelID;
            futuresOrderRequest.Code = request.Code;
            futuresOrderRequest.OpenCloseType = request.OpenCloseType;
            futuresOrderRequest.OrderAmount = request.OrderAmount;
            futuresOrderRequest.OrderPrice = request.OrderPrice;
            futuresOrderRequest.OrderUnitType = request.OrderUnitType;
            futuresOrderRequest.OrderWay = request.OrderWay;
            futuresOrderRequest.PortfoliosId = request.PortfoliosId;
            futuresOrderRequest.TraderId = request.TraderId;
            futuresOrderRequest.TraderPassword = request.TraderPassword;
            return futuresOrderRequest;
        }

        /// <summary>
        /// 根据当前日期获取期货保证金比例(百分比)
        /// </summary>
        /// <param name="code">期货代码</param>
        /// <returns>期货保证金比例(百分比)</returns>
        public static decimal GetFutureBailScale(string code)
        {
            string errCode = "GT-8403";
            string errMsg = "无法根据期货商品代码获取其对应的保证金比例。";

            CM_BreedClass breedClass = CommonPara.GetBreedClassByCommodityCode(code);

            int? breedClassType = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            if (!breedClassType.HasValue)
                throw new VTException(errCode, errMsg);

            Types.BreedClassTypeEnum breedClassTypeEnum;
            try
            {
                breedClassTypeEnum = (Types.BreedClassTypeEnum)breedClassType.Value;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                throw new VTException(errCode, errMsg);
            }

            switch (breedClassTypeEnum)
            {
                case Types.BreedClassTypeEnum.Stock:
                    throw new VTException(errCode, errMsg);
                case Types.BreedClassTypeEnum.CommodityFuture:
                    return FutureBailScaleService.GetFutureBailScale(code);
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    QH_SIFBail sifBail = FuturesTradeRules.GetSIFBailByBreedClassID(breedClass.BreedClassID);
                    return sifBail.BailScale;
            }

            return 0;
        }

        /// <summary>
        /// 根据商品代码获取持仓限制
        /// 现货返回股数，股指期货返回，商品期货返回手数
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>持仓限制</returns>
        public static PositionLimitValueInfo GetPositionLimit(string code)
        {
            #region old code
            //string errCode = "GT-8404";
            //string errMsg = "无法根据商品代码获取其对应的持仓限制。";

            //CM_BreedClass breedClass = CommonPara.GetBreedClassByCommodityCode(code);

            //int? breedClassType = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            //if (!breedClassType.HasValue)
            //    throw new VTException(errCode, errMsg);

            //Types.BreedClassTypeEnum breedClassTypeEnum;
            //try
            //{
            //    breedClassTypeEnum = (Types.BreedClassTypeEnum)breedClassType.Value;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.ToString(), ex);
            //    throw new VTException(errCode, errMsg);
            //}

            //switch (breedClassTypeEnum)
            //{
            //    case Types.BreedClassTypeEnum.Stock:
            //        ReckoningCounter.DAL.SpotTradingDevolveService.XH_SpotPosition xh_SpotPosition =
            //            SpotTradeRules.GetSpotPositionByBreedClassID(breedClass.BreedClassID);
            //        if (xh_SpotPosition != null)
            //        {
            //            //持仓比率
            //            if (xh_SpotPosition.Rate.HasValue)
            //            {
            //                var scale = xh_SpotPosition.Rate.Value / 100;
            //                //TODO:流通量
            //                CM_Commodity commodity = CommonPara.GetCommodityByCommodityCode(code);
            //                if (commodity == null)
            //                    throw new VTException(errCode, errMsg);

            //                if (!commodity.turnovervolume.HasValue)
            //                    throw new VTException(errCode, errMsg);

            //                var all = commodity.turnovervolume.Value;

            //                return (decimal)all * scale;
            //            }
            //        }
            //        break;
            //    case Types.BreedClassTypeEnum.CommodityFuture:
            //        Types.QHPositionValueType positionValueType;
            //        var val = FuturePositionLimitService.GetFuturePostionLimit(code, out positionValueType);
            //        if (positionValueType == Types.QHPositionValueType.Scales)
            //        {
            //            var all = GetFutureHold(code);
            //            return all * val;
            //        }
            //        return val;
            //    case Types.BreedClassTypeEnum.StockIndexFuture:
            //        QH_SIFPosition qh_SIFPosition =
            //            FuturesTradeRules.GetSIFPositionByBreedClassID(breedClass.BreedClassID);
            //        if (qh_SIFPosition != null)
            //        {
            //            //单边持仓量
            //            if (qh_SIFPosition.UnilateralPositions.HasValue)
            //            {
            //                return qh_SIFPosition.UnilateralPositions.Value;
            //            }
            //        }
            //        break;
            //}

            //return -1;
            #endregion
            return GetPositionLimit(code, Types.BreedClassTypeEnum.Stock);
        }
        /// <summary>
        /// 根据商品代码获取持仓限制
        /// 现货返回股数，股指期货返回，商品期货返回手数
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="classType">商品所属类别（这里主要是了为了与港股区分）</param>
        /// <returns>持仓限制</returns>
        public static PositionLimitValueInfo GetPositionLimit(string code, Types.BreedClassTypeEnum classType)
        {
            string errCode = "GT-8404";
            string errMsg = "无法根据商品代码获取其对应的持仓限制。";

            CM_BreedClass breedClass = CommonPara.GetBreedClassByCommodityCode(code, classType);

            //int? breedClassType = CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            //if (!breedClassType.HasValue)
            //    throw new VTException(errCode, errMsg);
            if (breedClass == null || !breedClass.BreedClassTypeID.HasValue)
            {
                throw new VTException(errCode, errMsg);
            }

            Types.BreedClassTypeEnum breedClassTypeEnum;
            try
            {

                //breedClassTypeEnum = (Types.BreedClassTypeEnum)breedClassType.Value;
                breedClassTypeEnum = (Types.BreedClassTypeEnum)breedClass.BreedClassTypeID.Value;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                throw new VTException(errCode, errMsg);
            }

            switch (breedClassTypeEnum)
            {
                case Types.BreedClassTypeEnum.Stock:
                    #region 现货
                    ReckoningCounter.DAL.SpotTradingDevolveService.XH_SpotPosition xh_SpotPosition =
                        SpotTradeRules.GetSpotPositionByBreedClassID(breedClass.BreedClassID);
                    if (xh_SpotPosition != null)
                    {
                        //持仓比率
                        if (xh_SpotPosition.Rate.HasValue)
                        {
                            var scale = xh_SpotPosition.Rate.Value / 100;
                            //TODO:流通量
                            CM_Commodity commodity = CommonPara.GetCommodityByCommodityCode(code);
                            if (commodity == null)
                                throw new VTException(errCode, errMsg);

                            if (!commodity.turnovervolume.HasValue)
                                throw new VTException(errCode, errMsg);

                            var all = commodity.turnovervolume.Value;
                            //return (decimal)all * scale;
                            PositionLimitValueInfo xhinfo = new PositionLimitValueInfo();
                            xhinfo.PositionValue = (decimal)all * scale;
                            return xhinfo;
                        }
                    }
                    #endregion
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    #region 港股
                    ReckoningCounter.DAL.SpotTradingDevolveService.XH_SpotPosition hkPosition =
                        SpotTradeRules.GetSpotPositionByBreedClassID(breedClass.BreedClassID);
                    if (hkPosition != null)
                    {
                        //持仓比率
                        if (hkPosition.Rate.HasValue)
                        {
                            var scale = hkPosition.Rate.Value / 100;
                            //TODO:流通量
                            HK_Commodity hkcommodity = HKTradeRulesProxy.GetHKCommodityByCommodityCode(code);
                            if (hkcommodity == null)
                                throw new VTException(errCode, errMsg);

                            if (!hkcommodity.turnovervolume.HasValue)
                                throw new VTException(errCode, errMsg);

                            var all = hkcommodity.turnovervolume.Value;

                            //return (decimal)all * scale;
                            PositionLimitValueInfo hkinfo = new PositionLimitValueInfo();
                            hkinfo.PositionValue = (decimal)all * scale;
                            return hkinfo;
                        }
                    }
                    #endregion
                    break;
                case Types.BreedClassTypeEnum.CommodityFuture:
                    #region 商品期货
                    Types.QHPositionValueType positionValueType;
                    var val = FuturePositionLimitService.GetFuturePostionLimit(code, out positionValueType);
                    if (val.PositionValue == -1 && val.IsNoComputer)
                    {
                        var levleCount = GetFutureHold(code);
                        val.PositionValue = levleCount;
                    }
                    else
                    {
                        if (positionValueType == Types.QHPositionValueType.Scales)
                        {
                            var all = GetFutureHold(code);
                            switch (val.QHPositionBailType)
                            {
                                case Types.QHPositionBailType.SinglePosition:
                                    all = all / 2;
                                    break;
                                case Types.QHPositionBailType.TwoPosition:
                                    break;
                                case Types.QHPositionBailType.ByDays:
                                    break;
                                default:
                                    break;
                            }
                            //return all * val;
                            decimal value = val.PositionValue / 100;
                            val.PositionValue = value * all;
                            return val;
                        }
                    }
                    return val;
                    #endregion
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    #region 股指期货
                    QH_SIFPosition qh_SIFPosition =
                        FuturesTradeRules.GetSIFPositionByBreedClassID(breedClass.BreedClassID);
                    if (qh_SIFPosition != null)
                    {
                        //单边持仓量
                        if (qh_SIFPosition.UnilateralPositions.HasValue)
                        {
                            //return qh_SIFPosition.UnilateralPositions.Value;
                            PositionLimitValueInfo gz_info = new PositionLimitValueInfo();
                            gz_info.PositionValue = qh_SIFPosition.UnilateralPositions.Value;
                            return gz_info;
                        }
                    }
                    break;
                    #endregion
            }
            PositionLimitValueInfo info = new PositionLimitValueInfo();
            info.PositionValue = -1;
            return info;
            //return -1;
        }

        /// <summary>
        /// 获取期货总持仓量（从行情中获取）
        /// </summary>
        /// <param name="code">期货合约</param>
        /// <returns>总持仓量</returns>
        private static int GetFutureHold(string code)
        {
            //TODO:总持仓量
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();
            //RealtimeMarketServiceFactory.GetService();
            var data = service.GetMercantileFutData(code);
            double hold = data.OpenInterest;
            return Convert.ToInt32(hold);
        }

        /// <summary>
        /// 根据字段id获取字段范围
        /// </summary>
        /// <param name="fieldRangeID">字段id</param>
        /// <returns>字段范围</returns>
        //public static CM_FieldRange GetFieldRange(int fieldRangeID)
        //{
        //    return CommonPara.GetFieldRangeByFieldRangeID(fieldRangeID);
        //}

        ///// <summary>
        ///// 输入值是否在当前字段范围内
        ///// </summary>
        ///// <param name="value">输入值</param>
        ///// <param name="fieldRange">字段范围</param>
        ///// <returns>是否在当前字段范围内</returns>
        //public static bool CheckFieldRange(decimal value, CM_FieldRange fieldRange)
        //{
        //    if (!fieldRange.LowerLimit.HasValue || !fieldRange.UpperLimit.HasValue)
        //        return false;

        //    bool isLowerEqual = fieldRange.LowerLimitIfEquation == (int)Types.IsYesOrNo.Yes ? true : false;
        //    bool result = isLowerEqual ? value >= fieldRange.LowerLimit : value > fieldRange.LowerLimit;

        //    if (result)
        //    {
        //        bool isUpperEqual = fieldRange.UpperLimitIfEquation == (int)Types.IsYesOrNo.Yes ? true : false;
        //        result = isUpperEqual ? value <= fieldRange.UpperLimit : value < fieldRange.UpperLimit;
        //    }

        //    return result;
        //}

        #region 期货通用方法 add by janyo at 090105

        /// <summary>
        /// 判断持仓表中的期货合约是否已经超过最后交易日
        /// Update by：李健华
        /// Update Date:2010-03-02
        /// Desc.:修改之前的判断是否过期合约错误判断，去年的合约一定是过期的，和上一个月的合约也是过期的，应分开判断
        /// </summary>
        /// <param name="contractCode">合约代码</param>
        public static bool IsExpireLastedTradeDate(string contractCode)
        {
            CM_BreedClass cmBreedClass = CommonPara.GetBreedClassByCommodityCode(contractCode);
            if (cmBreedClass == null)
            {
                throw new VTException("IsExpireLastedTradeDate", "无法获取期货合约代码对应的商品类型");
            }

            QH_FuturesTradeRules qhFuturesTradeRules =
                FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(cmBreedClass.BreedClassID);
            if (qhFuturesTradeRules == null)
            {
                throw new VTException("IsExpireLastedTradeDate", "无法获取期货商品类型对应的交易规则");
            }

            QH_LastTradingDay qhLastTradingDay =
                FuturesTradeRules.GetLastTradingDayByLastTradingDayID((int)qhFuturesTradeRules.LastTradingDayID);
            if (qhLastTradingDay == null)
            {
                throw new VTException("IsExpireLastedTradeDate", "无法获取期货商品类型对应的最后交易日");
            }

            if (contractCode.Length < 4)
            {
                throw new VTException("IsExpireLastedTradeDate", "非法的期货合约代码");
            }

            int year = 0, month = 0;
            FutureService.GetAgreementTime(contractCode, out year, out month);
            int day = GetLastTradingDay(qhLastTradingDay, contractCode);

            // update by 董鹏 2010-03-31
            if (year > DateTime.Now.Year)
            {
                return false;
            }

            //Update 李健华 2010-03-02==============
            //if (month < DateTime.Now.Month && year < DateTime.Now.Year) //把||改成&& 修改:刘书伟 2009-7-27
            //    return true; //去年的合约，上一个月的合约 肯定 不能交易了
            //-----------------------------
            if (year < DateTime.Now.Year)
            {
                return true;//去年的合约肯定不能交易
            }
            if (month < DateTime.Now.Month)
            {
                return true;//上一个月的合约肯定不能交易
            }
            //--------------------

            //==============================

            if (qhLastTradingDay.LastTradingDayTypeID == 4) //合约规定的月份的前一个月终止交易的合约
            {
                if (month == DateTime.Now.Month)
                {
                    return true;
                }
                else
                {
                    month--;
                }
            }

            if (day < DateTime.Now.Day && month == DateTime.Now.Month && year == DateTime.Now.Year)//去除=(因当天到期的合约也可以交易) 修改:刘书伟 2009-7-27
                return true; //小于等于今天的合约终止

            return false;
        }

        /// <summary>
        /// Desc: 获取指定代码的最后交易日
        /// Create by: 董鹏
        /// Create Date: 2010-03-05
        /// </summary>
        /// <param name="contractCode"></param>
        /// <returns></returns>
        public static int GetLastTradingDay(string contractCode)
        {
            CM_BreedClass cmBreedClass = CommonPara.GetBreedClassByCommodityCode(contractCode);
            if (cmBreedClass == null)
            {
                throw new VTException("GetLastTradingDay", "无法获取期货合约代码对应的商品类型");
            }

            QH_FuturesTradeRules qhFuturesTradeRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(cmBreedClass.BreedClassID);
            if (qhFuturesTradeRules == null)
            {
                throw new VTException("GetLastTradingDay", "无法获取期货商品类型对应的交易规则");
            }

            QH_LastTradingDay qhLastTradingDay = FuturesTradeRules.GetLastTradingDayByLastTradingDayID((int)qhFuturesTradeRules.LastTradingDayID);
            if (qhLastTradingDay == null)
            {
                throw new VTException("GetLastTradingDay", "无法获取期货商品类型对应的最后交易日");
            }

            return GetLastTradingDay(qhLastTradingDay, contractCode);
        }

        /// <summary>
        /// 根据交易日实体和品种得到最后交易日，当前月份为交割月份
        /// </summary>
        /// <param name="LastTradingDayEntity">交易日实体</param>
        /// <param name="contractCode">合约ID</param>
        /// <returns></returns>
        public static int GetLastTradingDay(QH_LastTradingDay LastTradingDayEntity, string contractCode)
        {
            //交易所代码
            CM_BourseType _CMBourseType = CommonPara.GetBourseTypeByCommodityCode(contractCode);
            int bourseTypeID = _CMBourseType.BourseTypeID;

            switch ((int)LastTradingDayEntity.LastTradingDayTypeID)
            {
                //第几天
                case (int)Types.QHLastTradingDayType.DeliMonthAndDay:
                    return DeliMonthOfDay(LastTradingDayEntity, contractCode);
                // return int.MaxValue;
                //倒数或者顺数第几个交易日
                case (int)Types.QHLastTradingDayType.DeliMonthAndDownOrShunAndWeek:
                    return DeliMonthOfTurnOrBackTrandingDay(LastTradingDayEntity, bourseTypeID);
                // return int.MaxValue;
                //交割月份的前一个月份的倒数或者顺数第几个交易日
                case (int)Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay:
                    return DeliMonthOfAgoMonthTradeDay(LastTradingDayEntity, bourseTypeID);
                // return int.MaxValue;
                //第几周的星期几
                case (int)Types.QHLastTradingDayType.DeliMonthAndWeek:
                    return DeliMonthOfWeekDay(LastTradingDayEntity, bourseTypeID);
                // return int.MaxValue;
            }

            return 0;
        }

        /// <summary>
        /// Desc: 获取指定品种的最后交易日
        /// Create by: 董鹏
        /// Create Date: 2010-03-18
        /// </summary>
        /// <param name="breedClassID"></param>
        /// <returns></returns>
        public static int GetLastTradingDayByBreedClassId(int breedClassID)
        {
            CM_BreedClass breedClass = CommonPara.GetBreedClassByBreedClassID(breedClassID);

            int bourseTypeID = breedClass.BourseTypeID.Value;

            if (breedClass == null)
            {
                throw new VTException("GetLastTradingDayByBreedClassId", "无法获取期货品种标识对应的品种");
            }

            QH_FuturesTradeRules qhFuturesTradeRules = FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(breedClass.BreedClassID);
            if (qhFuturesTradeRules == null)
            {
                throw new VTException("GetLastTradingDayByBreedClassId", "无法获取期货商品类型对应的交易规则");
            }

            QH_LastTradingDay qhLastTradingDay = FuturesTradeRules.GetLastTradingDayByLastTradingDayID((int)qhFuturesTradeRules.LastTradingDayID);
            if (qhLastTradingDay == null)
            {
                throw new VTException("GetLastTradingDayByBreedClassId", "无法获取期货商品类型对应的最后交易日");
            }

            switch ((int)qhLastTradingDay.LastTradingDayTypeID)
            {
                //第几天
                case (int)Types.QHLastTradingDayType.DeliMonthAndDay:
                    //return DeliMonthOfDay(qhLastTradingDay, contractCode);
                    return DeliMonthOfDay(qhLastTradingDay, bourseTypeID);
                // return int.MaxValue;
                //倒数或者顺数第几个交易日
                case (int)Types.QHLastTradingDayType.DeliMonthAndDownOrShunAndWeek:
                    return DeliMonthOfTurnOrBackTrandingDay(qhLastTradingDay, bourseTypeID);
                // return int.MaxValue;
                //交割月份的前一个月份的倒数或者顺数第几个交易日
                case (int)Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay:
                    return DeliMonthOfAgoMonthTradeDay(qhLastTradingDay, bourseTypeID);
                // return int.MaxValue;
                //第几周的星期几
                case (int)Types.QHLastTradingDayType.DeliMonthAndWeek:
                    return DeliMonthOfWeekDay(qhLastTradingDay, bourseTypeID);
                // return int.MaxValue;
            }

            return 0;
        }

        /// <summary>
        /// 类型为第几日（自然日），求最后交易日，如果当天为非交易日，往后顺延
        /// </summary>
        /// <param name="LastTradingDay">最后交易日实体</param>
        /// <param name="code">代码</param>
        /// <returns></returns>
        private static int DeliMonthOfDay(QH_LastTradingDay LastTradingDay, string code)
        {
            int day = (int)LastTradingDay.WhatDay;

            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;

            DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, day));
            while (!JudgmentIsTrandingDay(dt, code))
            {
                dt = dt.AddDays(1);
                if (dt.Month != CurrentMonth) break;
            }

            if (dt.Month == CurrentMonth) return dt.Day;

            return int.MaxValue;
        }

        /// <summary>
        /// Desc: 类型为第几日（自然日），求最后交易日，如果当天为非交易日，往后顺延
        /// Create by: 董鹏
        /// Create Date: 2010-03-18
        /// </summary>
        /// <param name="LastTradingDay">最后交易日实体</param>
        /// <param name="bourseTypeID">交易所ID</param>
        /// <returns></returns>
        private static int DeliMonthOfDay(QH_LastTradingDay LastTradingDay, int bourseTypeID)
        {
            int day = (int)LastTradingDay.WhatDay;

            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;

            //根据品种获取当前月份里面的非交易日列表
            IList<CM_NotTradeDate> List_CM_NotTradeDate = CommonPara.GetNotTradeDateByBourseType(bourseTypeID);

            DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, day));

            foreach (CM_NotTradeDate date in List_CM_NotTradeDate)
            {
                while (((DateTime)date.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                {
                    dt = dt.AddDays(1);
                    if (dt.Month != CurrentMonth) break;
                }
            }
            if (dt.Month == CurrentMonth) return dt.Day;

            return int.MaxValue;
        }

        /// <summary>
        /// 类型为第几周的星期几，求最后交易日
        /// </summary>
        /// <param name="LastTradingDay">最后交易日实体</param>
        /// <param name="bourseTypeID">交易所ID</param>
        /// <returns></returns>
        private static int DeliMonthOfWeekDay(QH_LastTradingDay LastTradingDay, int bourseTypeID)
        {
            if (LastTradingDay == null) return int.MaxValue;
            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;
            return getDay(CurrentYear, CurrentMonth, (int)LastTradingDay.WhatWeek, (int)LastTradingDay.Week);
        }

        /// <summary>
        /// 类型为倒数或者顺数第几个交易日，求最后交易日
        /// </summary>
        /// <param name="LastTradingDay">最后交易日实体</param>
        /// <param name="bourseTypeID">交易所ID</param>
        /// <returns></returns>
        private static int DeliMonthOfTurnOrBackTrandingDay(QH_LastTradingDay LastTradingDay, int bourseTypeID)
        {
            int day = (int)LastTradingDay.WhatDay;

            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;

            // CM_NotTradeDateDAL NotTradeDateDAL = new CM_NotTradeDateDAL();

            int temp = 0;

            //根据品种获取当前月份里面的非交易日列表
            IList<CM_NotTradeDate> List_CM_NotTradeDate = CommonPara.GetNotTradeDateByBourseType(bourseTypeID);

            #region 根据类型求出最后交易日

            if (LastTradingDay.Sequence == (int)Types.QHLastTradingDayIsSequence.Order)
            {
                for (int i = 1; i <= DateTime.DaysInMonth(CurrentYear, CurrentMonth); i++)
                {
                    DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, i));
                    bool falg = false;
                    foreach (CM_NotTradeDate date in List_CM_NotTradeDate)
                    {
                        if (((DateTime)date.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                        {
                            falg = true;
                            break;
                        }
                    }
                    if (!falg)
                    {
                        temp = temp + 1;
                        if (temp == day) return i;
                    }
                }
            }
            else
            {
                for (int i = DateTime.DaysInMonth(CurrentYear, CurrentMonth); i >= 1; i--)
                {
                    DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, i));
                    bool falg = false;
                    foreach (CM_NotTradeDate date in List_CM_NotTradeDate)
                    {
                        if (((DateTime)date.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                        {
                            falg = true;
                            break;
                        }
                    }
                    if (!falg)
                    {
                        temp = temp + 1;
                        if (temp == day) return i;
                    }
                }
            }

            #endregion

            return int.MaxValue;
        }

        /// <summary>
        /// 类型为交割月份的前一个月份的倒数或者顺数第几个交易日，求最后交易日
        /// </summary>
        /// <param name="LastTradingDay">最后交易日实体</param>
        /// <param name="bourseTypeID">交易所ID</param>
        /// <returns></returns>
        private static int DeliMonthOfAgoMonthTradeDay(QH_LastTradingDay LastTradingDay, int bourseTypeID)
        {
            return DeliMonthOfTurnOrBackTrandingDay(LastTradingDay, bourseTypeID);
        }


        /// <summary>
        /// 判断该天是否交易
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="breedclass"></param>
        /// <returns></returns>
        public static bool JudgmentIsTrandingDay(DateTime dt, string code)
        {
            return CommonPara.IsTradeDate(code, dt);
        }

        /// <summary>
        /// 根据某年某月第几周星期几得到为该月的几号
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="weekNO"></param>
        /// <param name="weekDay">星期日为0</param>
        /// <returns></returns>
        public static int getDay(int year, int month, int weekNO, int weekDay)
        {
            DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, 1));
            DayOfWeek t = dt.DayOfWeek;
            int days = 0;
            int temp = 0;
            switch (t)
            {
                case DayOfWeek.Sunday:
                    temp = 0;
                    break;
                case DayOfWeek.Monday:
                    temp = 1;
                    break;
                case DayOfWeek.Tuesday:
                    temp = 2;
                    break;
                case DayOfWeek.Wednesday:
                    temp = 3;
                    break;
                case DayOfWeek.Thursday:
                    temp = 4;
                    break;
                case DayOfWeek.Friday:
                    temp = 5;
                    break;
                case DayOfWeek.Saturday:
                    temp = 6;
                    break;
            }
            if (weekNO == 1)
            {
                return days = weekDay + 1 - temp;
            }
            //return days = 7 - temp + (weekNO - 2) * 7 + weekDay + 1;
            if (temp == 0 || temp == 6)
            {
                return days = 7 - temp + (weekNO - 2) * 7 + weekDay + 1 + 7;//当本月的第一天是星期六或星期天时，向后顺延一周

            }
            else
            {
                return days = 7 - temp + (weekNO - 2) * 7 + weekDay + 1;

            }
        }

        // }

        #endregion

        /// <summary>
        /// Title:判断当前时间是否为
        /// 当日00：00：00至当日开盘前--0,
        /// 当日开盘至当日收盘--1,
        /// 当日收盘后至当日23：59：59--2
        /// Create by:李健华
        /// Create Date:2009-12-01
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int IsNowTimeMarket(Types.BreedClassTypeEnum type, string code)
        {
            var bourseType = CommonPara.GetBourseTypeByCommodityCode(code, type);
            if (bourseType == null)
            {
                return 0;
            }
            DateTime isNowTime = DateTime.Now;
            string nowDate = isNowTime.ToString("yyyy-MM-dd");

            try
            {
                var tradeTimes = CommonPara.GetAllTradeTimeByBourseTypeID(bourseType.BourseTypeID);
                if (tradeTimes == null || tradeTimes.Count == 0)
                {
                    return 0;
                }
                #region 处理最先开市时间与结束时间
                DateTime stratDateTime = ConvertToNowDateTime(tradeTimes[0].StartTime.Value);
                DateTime endDateTime = ConvertToNowDateTime(tradeTimes[0].EndTime.Value);

                foreach (var item in tradeTimes)
                {
                    DateTime start = ConvertToNowDateTime(item.StartTime.Value);
                    if (stratDateTime > start)
                    {
                        stratDateTime = start;
                    }
                    DateTime end = ConvertToNowDateTime(item.EndTime.Value);
                    if (endDateTime < end)
                    {
                        endDateTime = end;
                    }
                }
                #endregion
                DateTime zeroHour = DateTime.Parse(nowDate + " 00:00:00");

                DateTime time = DateTime.Parse(nowDate + " 23:59:59");

                if (isNowTime >= stratDateTime && isNowTime <= endDateTime)
                {
                    return 1;
                }
                else if (isNowTime >= zeroHour && isNowTime <= stratDateTime)
                {
                    return 0;
                }
                else if (isNowTime >= endDateTime && isNowTime <= time)
                {
                    return 2;
                }

                return 0;
            }
            catch
            {
                return 0;
            }

            #region old code
            //try
            //{
            //    string startTimeStr = bourseType.CounterFromSubmitStartTime.Value.ToString("HH:mm:ss");
            //    DateTime stratDateTime = DateTime.Parse(nowDate + " " + startTimeStr);

            //    string endTimeStr = bourseType.CounterFromSubmitEndTime.Value.ToString("HH:mm:ss");
            //    DateTime endDateTime = DateTime.Parse(nowDate + " " + endTimeStr);

            //    DateTime zeroHour = DateTime.Parse(nowDate + " 00:00:00");

            //    DateTime time = DateTime.Parse(nowDate + " 23:59:59");

            //    if (isNowTime >= stratDateTime && isNowTime <= endDateTime)
            //    {
            //        return 1;
            //    }
            //    else if (isNowTime >= zeroHour && isNowTime <= stratDateTime)
            //    {
            //        return 0;
            //    }
            //    else if (isNowTime >= endDateTime && isNowTime <= time)
            //    {
            //        return 2;
            //    }

            //    return 0;
            //}
            //catch
            //{
            //    return 0;
            //}
            #endregion


        }


        /// <summary>
        /// 把传入的日期时间转为当前日期时间 如果转换有异常则返回当前时间DateTime.Now
        /// </summary>
        /// <param name="time">要转换的时间</param>
        /// <returns></returns>
        public static DateTime ConvertToNowDateTime(DateTime time)
        {
            string strDate = DateTime.Now.ToString("yyyy-MM-dd");
            string strTime = time.ToString("HH:mm:ss");

            DateTime tradeTime = DateTime.Parse(strDate + " " + strTime);
            return tradeTime;

        }

        #region 根据期货Code判断是否为新品种期货合约上市当日 add by 董鹏 2010-01-25
        /// <summary>
        /// 根据期货Code判断是否为新品种期货合约上市
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsNewBreedClassByCode(string code)
        {
            CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null || !commodity.BreedClassID.HasValue)
            {
                return false;
            }
            //IList<CM_Commodity> itemsAll = MCService.CommonPara.GetAllCommodity();
            IList<CM_Commodity> items = new List<CM_Commodity>();
            items = MCService.CommonPara.GetAllCommodityByBreedClass(commodity.BreedClassID.Value);
            //foreach (CM_Commodity item in itemsAll)
            //{
            //    if (item.BreedClassID == commodity.BreedClassID)
            //    {
            //        items.Add(item);
            //    }
            //}
            if (items.Count == 1)
            {
                if (commodity.MarketDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    return true;
                }
            }
            else
            {
                //是否有比当前更早的代码
                bool flag = false;
                foreach (CM_Commodity item in items)
                {
                    if (item.CommodityCode.CompareTo(commodity.CommodityCode) < 0)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    if (commodity.MarketDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 根据期货Code判断是否为新月份期货合约上市当日 add by 董鹏 2010-01-25
        /// <summary>
        /// 根据商品Code判断是否为新月份期货合约上市
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static bool IsNewMonthBreedClassByCode(string code)
        {
            CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null || !commodity.BreedClassID.HasValue)
            {
                return false;
            }
            //IList<CM_Commodity> itemsAll = MCService.CommonPara.GetAllCommodity();
            IList<CM_Commodity> items = new List<CM_Commodity>();
            items = MCService.CommonPara.GetAllCommodityByBreedClass(commodity.BreedClassID.Value);
            //foreach (CM_Commodity item in itemsAll)
            //{
            //    if (item.BreedClassID == commodity.BreedClassID)
            //    {
            //        items.Add(item);
            //    }
            //}
            //同品种有1个以上的代码，并且有比当前代码更早的
            if (items.Count > 1)
            {
                bool flag = false;
                foreach (CM_Commodity item in items)
                {
                    if (item.CommodityCode.CompareTo(commodity.CommodityCode) < 0)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    if (commodity.MarketDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 根据期货Code判断是否为交割月份 add by 董鹏 2010-01-26
        /// <summary>
        /// 判断当前月是否合约交割月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <returns></returns>
        public static bool IsDeliveryMonth(string code)
        {
            int year;
            int month;
            FutureService.GetAgreementTime(code, out year, out month);
            if (year == DateTime.Now.Year && month == DateTime.Now.Month)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 根据期货合约代码判断是否为最后交易日 add by 董鹏 2010-03-05
        /// <summary>
        /// 根据期货合约代码判断是否为最后交易日
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsLastTradingDayContract(string code)
        {
            int year;
            int month;
            FutureService.GetAgreementTime(code, out year, out month);
            int LastTradingDay = GetLastTradingDay(code);
            DateTime date = new DateTime(year, month, LastTradingDay);

            while (!JudgmentIsTrandingDay(date, code))
            {
                date = date.AddDays(1);
            }
            if (DateTime.Now.Date == date.Date)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 根据期货合约代码判断是否为季月合约上市首日 add by 董鹏 2010-03-05
        /// <summary>
        /// 根据期货合约代码判断是否为季月合约上市首日
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <returns></returns>
        public static bool IsNewQuarterMonthContract(string code)
        {
            //if (!IsNewMonthBreedClassByCode(code) && !IsNewBreedClassByCode(code))
            //{
            //    return false;
            //}

            CM_Commodity cm = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (cm.MarketDate.Date != DateTime.Now.Date)
            {
                return false;
            }

            int year;
            int month;
            FutureService.GetAgreementTime(code, out year, out month);
            switch (DateTime.Now.Month)
            {
                case 1:
                    if (year == DateTime.Now.Year && month == 9)
                    {
                        return true;
                    }
                    break;
                case 4:
                    if (year == DateTime.Now.Year && month == 12)
                    {
                        return true;
                    }
                    break;
                case 7:
                    if (year == DateTime.Now.AddYears(1).Year && month == 3)
                    {
                        return true;
                    }
                    break;
                case 10:
                    if (year == DateTime.Now.AddYears(1).Year && month == 6)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
        #endregion
    }
}