using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.BLL.ManagementCenter;
using MatchCenter.DAL.SpotTradingDevolveService;
using MatchCenter.BLL.Common;
//using CommonRealtimeMarket.entity;
using MatchCenter.BLL.RealTime;
using MatchCenter.Entity;
using GTA.VTS.Common.CommonObject;
using MatchCenter.DAL.DevolveVerifyCommonService;
//增加港股引用
using MatchCenter.DAL.HKTradingRulesService;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.DAL.FuturesDevolveService;
using MatchCenter.BLL.MatchRules;
using RealTime.Server.SModelData.HqData;
using MatchCenter.Entity.HK;

namespace MatchCenter.BLL
{
    /// <summary>
    /// 费用价格计算功能类
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// Update By:李健华
    /// Update Date:2009-10-29
    /// Desc.:修改港股的限价盘的验证上下限计算价位获取的方法
    /// Update By:董鹏
    /// Update Date:2009-12-10
    /// Desc.:修改港股的验证上下限计算方法，按照新的需求做了分段计算处理
    /// Update By:董鹏
    /// Update Date:2009-12-18
    /// Desc.:修改港股的验证上下限日志内容
    /// Update By:董鹏
    /// Update Date:2010-03-05
    /// Desc: 增加了股指期货"季月合约上市首日涨跌幅","合约最后交易日涨跌幅"的计算方法
    /// </summary>
    public class CostPriceCalculate : Singleton<CostPriceCalculate>
    {
        #region 单一进入模式
        /// <summary>
        /// 费用价格计算功能类功能单一进入模式
        /// </summary>
        public static CostPriceCalculate Instanse
        {
            get
            {
                return singletonInstance;
            }
        }
        #endregion

        #region 根据商品代码获取现货交易规则中最小价格小数位长度
        /// <summary>
        /// 根据商品代码获取现货交易规则中最小价格小数位长度
        /// 去除小数点最后为0无意义的位数.如果数据没有小数位（如：100-->2）返回默认有效小数位长度
        /// 如果所有的小数位都为0则返回默认有效小数为长度（如：100.000-->2）
        /// 如果有有效位数则返回有效位数（如：3.1010-->3，5.00002-->5,3.200-->1）
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns></returns>
        public int GetXHMinChangePriceDecimalDigit(string code)
        {
            XH_SpotTradeRules rule = CommonDataManagerOperate.GetXH_SpotTradeRulesByCommodityCode(code);
            if (rule == null)
                return RulesDefaultValue.DefaultLength;
            return Utils.GetDecimalDigit((decimal)rule.MinChangePrice, true);
        }
        #endregion

        #region 根据交易商品代码和交易方法以现现货品种涨跌幅控制类型计算相关涨跌幅
        /// <summary>
        /// Title:根据交易商品代码和交易方法以现现货品种涨跌幅控制类型计算相关涨跌幅
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="trans">买卖类型</param>
        /// <returns></returns>
        public PriceEntity GetPriceEntityForHighLowControlType(string code, Types.TransactionDirection trans)
        {
            LogHelper.WriteDebug("进入计算价格实体方法：");

            #region 获取商品，交易规则，行情数据
            //价格实体
            var entity = new PriceEntity();
            CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (commodity == null)
            {
                return null;
            }
            //var xh_tradeRules = CommonDataManagerOperate.GetSpotTradeRulesByCommondityCode(code);
            var xh_tradeRules = CommonDataCacheProxy.Instanse.GetCacheXH_SpotTradeRulesByKey((int)commodity.BreedClassID);
            if (xh_tradeRules == null)
            {
                return null;
            }

            HqExData vtHqExData = RealtimeMarketService.GetRealTimeStockDataByCommdityCode(code);
            if (vtHqExData == null)
            {
                return null;
            }
            var controlType = CommonDataCacheProxy.Instanse.GetCacheXH_SpotHighLowControlTypeByKey((int)xh_tradeRules.BreedClassHighLowID);
            var highLowValue = CommonDataManagerOperate.GetXH_SpotHighLowValueByBreedClassHighLowID((int)xh_tradeRules.BreedClassHighLowID);

            //昨日收盘价
            decimal yclose = (decimal)vtHqExData.YClose;
            #endregion

            #region 新股上市，增发上市，暂停后开始交易和其他日期(正常股票,ST股票)Types.XHSpotHighLowControlType.NewThighAddFatStopAfterOrOtherDate
            //新股上市，增发上市，暂停后开始交易和其他日期(正常股票,ST股票)
            if (controlType.HighLowTypeID == (int)Types.XHSpotHighLowControlType.NewThighAddFatStopAfterOrOtherDate)
            {
                //if (vtHqExData.StockLevel == Types.StockNatureEnum.ST)
                if (vtHqExData.Name.ToLower().Contains("st"))
                {
                    entity.LowerPrice = yclose * (1.0m - (decimal)highLowValue.StValue / 100);
                    entity.CeilingPrice = yclose * (1.0m + (decimal)highLowValue.StValue / 100);
                    return entity;
                }
                entity.LowerPrice = yclose * (1.0m - (decimal)highLowValue.NormalValue / 100);
                entity.CeilingPrice = yclose * (1.0m + (decimal)highLowValue.NormalValue / 100);
                return entity;
            }
            #endregion

            #region 无涨跌幅  Types.XHSpotHighLowControlType.NotHighLowControl
            //无涨跌幅
            if (controlType.HighLowTypeID == (int)Types.XHSpotHighLowControlType.NotHighLowControl)
            {
                return GetPriceEntityForValidDeclareType(code, trans, Types.MarketType.Default);
            }
            #endregion

            #region 权证的涨跌幅 Types.XHSpotHighLowControlType.RightPermitHighLow
            //权证的涨跌幅
            if (controlType.HighLowTypeID == (int)Types.XHSpotHighLowControlType.RightPermitHighLow)
            {
                var toOpenPrice = yclose;
                if (toOpenPrice <= 0.00m)
                {
                    toOpenPrice = (decimal)vtHqExData.HqData.Open;
                }
                HqExData labelHqExData = RealtimeMarketService.GetRealTimeStockDataByCommdityCode(commodity.LabelCommodityCode);

                decimal upPrice = (decimal)labelHqExData.YClose * (XHHighLowScale(commodity.LabelCommodityCode));
                //权证(上限)涨幅价格＝权证前一日收盘价格＋标的证券昨日价格*涨跌比例×行权比例×125%((decimal)highLowValue.RightHighLowScale / 100)；
                decimal risePrice = yclose + upPrice * commodity.GoerScale * (decimal)highLowValue.RightHighLowScale / 100;
                //权证(上限)涨幅价格＝权证前一日收盘价格-标的证券昨日价格*涨跌比例×行权比例×125%((decimal)highLowValue.RightHighLowScale / 100)；
                decimal lowPrice = yclose - upPrice * commodity.GoerScale * (decimal)highLowValue.RightHighLowScale / 100;
                if (risePrice <= 0.00m || lowPrice <= 0.00m)
                {
                    int length = Utils.GetDecimalDigit((decimal)xh_tradeRules.MinChangePrice, true);
                    if (risePrice <= 0.00m)
                    {
                        risePrice = Utils.XPowerYCountdown(10, (double)length);
                    }
                    if (lowPrice <= 0.00m)
                    {
                        lowPrice = Utils.XPowerYCountdown(10, (double)length);
                    }
                }
                entity.CeilingPrice = risePrice;
                entity.LowerPrice = lowPrice;
                return entity;
            }
            #endregion

            #region 新基金上市，增发上市，暂停后开始交易和其他日期 Types.XHSpotHighLowControlType.NewFundAddFatStopAfterOrOtherDate
            //新基金上市，增发上市，暂停后开始交易和其他日期
            if (controlType.HighLowTypeID == (int)Types.XHSpotHighLowControlType.NewFundAddFatStopAfterOrOtherDate)
            {
                //是否为新上市
                bool isNew = CommonDataManagerOperate.IsNewMarketyByCode(code);
                if (isNew)
                {
                    return GetPriceEntityForValidDeclareType(code, trans, Types.MarketType.New);
                }
                //是否为增发上市
                bool isAdd = CommonDataManagerOperate.IsIncreaseMarketByCode(code);
                if (isAdd)
                {
                    return GetPriceEntityForValidDeclareType(code, trans, Types.MarketType.Increase);
                }
                entity.CeilingPrice = yclose * (RulesDefaultValue.DefaultValue + (decimal)highLowValue.FundYestClosePriceScale);
                entity.LowerPrice = yclose * (RulesDefaultValue.DefaultValue - (decimal)highLowValue.FundYestClosePriceScale);
                return entity;
            }
            #endregion

            return null;
        }
        #endregion

        #region 现货涨跌幅比例值
        /// <summary>
        /// 现货涨跌幅比例值
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        private decimal XHHighLowScale(string code)
        {

            HqExData vtHqExData = RealtimeMarketService.GetRealTimeStockDataByCommdityCode(code);
            XH_SpotTradeRules tradeRule = CommonDataManagerOperate.GetXH_SpotTradeRulesByCommodityCode(code);
            XH_SpotHighLowValue higValue = CommonDataManagerOperate.GetXH_SpotHighLowValueByBreedClassHighLowID((int)tradeRule.BreedClassHighLowID);
            if (higValue == null)
                return 0.00m;
            //ST企业
            //if (vtHqExData.StockLevel == Types.StockNatureEnum.ST)
            if (vtHqExData.Name.ToLower().Contains("st"))
            {
                return (decimal)(higValue.StValue / 100);
            }
            //正常企业
            return (decimal)higValue.NormalValue / 100;

        }
        #endregion

        #region 根据交易商品代码和交易方法以有效申报类型计算相关涨跌幅
        /// <summary>
        ///  Title:根据交易商品代码和交易方法以有效申报类型计算相关涨跌幅
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="trans">交易方向</param>
        /// <param name="marketType">上市类型</param>
        /// <returns></returns>
        public PriceEntity GetPriceEntityForValidDeclareType(string code, Types.TransactionDirection trans, Types.MarketType marketType)
        {
            var xh_tradeRules = CommonDataManagerOperate.GetXH_SpotTradeRulesByCommodityCode(code);
            if (xh_tradeRules == null)
            {
                return null;
            }
            XH_ValidDeclareType ValidType = CommonDataCacheProxy.Instanse.GetCacheXH_ValideDeclareTypeByID((int)xh_tradeRules.BreedClassValidID);
            XH_ValidDeclareValue ruleValue = CommonDataManagerOperate.GetValidDeclareValueByBreedClassValidID((int)xh_tradeRules.BreedClassValidID);

            CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            HqExData vtHqExData = RealtimeMarketService.GetRealTimeStockDataByCommdityCode(code);

            if (vtHqExData == null)
                return null;
            var Valid = (Types.XHValidDeclareType)ValidType.ValidDeclareTypeID;
            //价格实体
            var entity = new PriceEntity();
            //行情数据
            HqData hqData = vtHqExData.HqData;

            #region  create by 董鹏 2010-03-23

            decimal uLimit = 0;
            decimal lLimit = 0;
            switch (marketType)
            {
                case Types.MarketType.Default:
                    uLimit = ruleValue.UpperLimit.Value;
                    lLimit = ruleValue.LowerLimit.Value;
                    break;
                case Types.MarketType.New:
                    if (Valid == Types.XHValidDeclareType.BargainPriceOnDownMoney)
                    {
                        uLimit = ruleValue.NewDayUpperLimit.Value;
                        lLimit = ruleValue.NewDayLowerLimit.Value;
                    }
                    else
                    {
                        uLimit = ruleValue.UpperLimit.Value;
                        lLimit = ruleValue.LowerLimit.Value;
                    }
                    break;
                case Types.MarketType.Increase:
                    if (Valid == Types.XHValidDeclareType.BargainPriceOnDownMoney)
                    {
                        uLimit = ruleValue.NewDayUpperLimit.Value;
                        lLimit = ruleValue.NewDayLowerLimit.Value;
                    }
                    else
                    {
                        uLimit = ruleValue.UpperLimit.Value;
                        lLimit = ruleValue.LowerLimit.Value;
                    }
                    break;
            }

            //最近成交的百分比
            if (Valid == Types.XHValidDeclareType.BargainPriceUpperDownScale)
            {
                entity.CeilingPrice = (decimal)(hqData.Lasttrade) * (1 + uLimit / 100);
                entity.LowerPrice = (decimal)(hqData.Lasttrade) * (1 - lLimit / 100);
                return entity;
            }
            //不高于即时揭示的最低卖出价格的110%且不低于即时揭示的最高买入价格的90%
            if (Valid == Types.XHValidDeclareType.NotHighSalePriceScaleAndNotLowBuyPriceScale)
            {
                entity.CeilingPrice = (decimal)hqData.Sellprice1 * uLimit / 100;
                entity.LowerPrice = (decimal)hqData.Buyprice1 * lLimit / 100;
                return entity;
            }
            if (Valid == Types.XHValidDeclareType.BargainPriceOnDownMoney)
            {
                entity.CeilingPrice = (decimal)hqData.Lasttrade + uLimit;
                entity.LowerPrice = (decimal)hqData.Lasttrade - lLimit;
                return entity;
            }
            #endregion

            #region old
            ////最近成交的百分比
            //if (Valid == Types.XHValidDeclareType.BargainPriceUpperDownScale)
            //{
            //    entity.CeilingPrice = (decimal)(hqData.Lasttrade) * (decimal)(1 + ruleValue.UpperLimit / 100);
            //    entity.LowerPrice = (decimal)(hqData.Lasttrade) * ((decimal)(1 - ruleValue.LowerLimit / 100));
            //    return entity;
            //}
            ////不高于即时揭示的最低卖出价格的110%且不低于即时揭示的最高买入价格的90%
            //if (Valid == Types.XHValidDeclareType.NotHighSalePriceScaleAndNotLowBuyPriceScale)
            //{
            //    entity.CeilingPrice = (decimal)hqData.Sellprice1 * (decimal)ruleValue.UpperLimit / 100;
            //    entity.LowerPrice = (decimal)hqData.Buyprice1 * (decimal)ruleValue.LowerLimit / 100;
            //    return entity;
            //}
            ////买入申报:低于买一价24个价位与卖一价之间卖出申报：买一价与高于卖一价24个价位之间
            ////if (Valid == Types.XHValidDeclareType.DownBuyOneAndSaleOne)
            ////{
            ////CM_FieldRange range;
            ////XH_MinChangePriceValue minChangePriceValue;
            ////if (trans == Types.TransactionDirection.Buying)
            ////{
            ////    range = CommonDataManagerOperate.GetFieldRangeByRangeValue((decimal)hqData.Buyprice1);
            ////    minChangePriceValue = CommonDataCacheProxy.Instanse.GetCacheMinChangePriceValueByKey((int)commodity.BreedClassID, range.FieldRangeID);
            ////    entity.CeilingPrice = (decimal)hqData.Sellprice1;
            ////    entity.LowerPrice = (decimal)hqData.Buyprice1 - (decimal)ruleValue.UpperLimit * (decimal)minChangePriceValue.Value;
            ////    return entity;
            ////}
            ////if (trans == Types.TransactionDirection.Selling)
            ////{
            ////    range = CommonDataManagerOperate.GetFieldRangeByRangeValue((decimal)hqData.Sellprice1);
            ////    minChangePriceValue = CommonDataCacheProxy.Instanse.GetCacheMinChangePriceValueByKey((int)commodity.BreedClassID, range.FieldRangeID);
            ////    entity.LowerPrice = (decimal)hqData.Buyprice1;
            ////    entity.CeilingPrice = (decimal)hqData.Sellprice1 + (decimal)ruleValue.LowerLimit * (decimal)minChangePriceValue.Value;
            ////    return entity;
            ////}
            ////}
            ////最近成交价的上下百分比
            //if (Valid == Types.XHValidDeclareType.BargainPriceOnDownMoney)
            //{
            //    entity.CeilingPrice = (decimal)hqData.Lasttrade + (decimal)ruleValue.UpperLimit;
            //    entity.LowerPrice = (decimal)hqData.Lasttrade - (decimal)ruleValue.LowerLimit;
            //    return entity;
            //}
            #endregion
            return null;
        }
        #endregion

        #region 根据交易商品编号和交易方向计算【现货】涨跌幅度
        /// <summary>
        /// Tiele：根据交易商品编号和交易方向计算涨跌幅度
        /// Desc.:如果是亲股上市或者是新增股直接调用以有效申报计算相关涨跌幅
        ///       如果以上两都不是则调用以现现货品种涨跌幅控制类型计算相关涨跌幅
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="transactionType">交易方向类型</param>
        /// <returns></returns>
        public PriceEntity GetPriceEntity(string code, Types.TransactionDirection transactionType)
        {
            try
            {
                //是否是新股上市
                bool isNew = CommonDataManagerOperate.IsNewMarketyByCode(code);
                if (isNew)
                {
                    return GetPriceEntityForValidDeclareType(code, transactionType, Types.MarketType.New);
                }
                //是否是新增股上市
                bool isAdd = CommonDataManagerOperate.IsIncreaseMarketByCode(code);
                if (isAdd)
                {
                    return GetPriceEntityForValidDeclareType(code, transactionType, Types.MarketType.Increase);
                }
                return GetPriceEntityForHighLowControlType(code, transactionType);
            }
            catch
            {
                return GetInitPriceEntity(code);
            }
        }
        #endregion

        #region 根据交易商品编号和交易方向计算【期货】涨跌幅度
        /// <summary>
        /// 根据交易商品编号和交易方向计算【期货】涨跌幅度
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public PriceEntity GetPriceEntity(string code)
        {
            //futData.PreSettlementPrice
            //VTFutData vtFutData = RealtimeMarketService.GetRealtimeMark().GetFutData(code);
            FutData vtFutData = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetFutData(code);
            PriceEntity priceEntity = new PriceEntity();
            //实体不能为空
            if (vtFutData == null)
                return null;
            //期货交易规则
            QH_FuturesTradeRules tradeRules = CommonDataManagerOperate.GetQH_FuturesTradeRulesByCommodityCode(code);
            //期货交易规则不能为空
            if (tradeRules == null)
            {
                return null;
            }

            #region "季月合约上市首日涨跌幅","合约最后交易日涨跌幅"判断， add by 董鹏 2010-03-05

            decimal scope;

            if (CommonDataManagerOperate.IsLastTradingDayContract(code))
            {
                //合约最后交易日涨跌幅
                scope = tradeRules.NewMonthFuturesPactHighLowStopValue * tradeRules.HighLowStopScopeValue.Value;
            }
            else if (CommonDataManagerOperate.IsNewQuarterMonthContract(code))
            {
                //季月合约上市首日涨跌幅
                scope = tradeRules.NewBreedFuturesPactHighLowStopValue * tradeRules.HighLowStopScopeValue.Value;
            }
            else
            {
                //一般情况
                scope = tradeRules.HighLowStopScopeValue.Value;
            }

            if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice)
            {

                priceEntity.CeilingPrice = (decimal)vtFutData.PreSettlementPrice * (1.00m + scope / 100);
                priceEntity.LowerPrice = (decimal)vtFutData.PreSettlementPrice * (1.00m - scope / 100);
                return priceEntity;
            }
            if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice)
            {
                priceEntity.CeilingPrice = (decimal)vtFutData.PreSettlementPrice + scope;
                priceEntity.CeilingPrice = (decimal)vtFutData.PreSettlementPrice - scope;
                return priceEntity;
            }

            #endregion

            //if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice)
            //{

            //    priceEntity.CeilingPrice = (decimal)vtFutData.PreSettlementPrice * (1.00m + (decimal)tradeRules.HighLowStopScopeValue / 100);
            //    priceEntity.LowerPrice = (decimal)vtFutData.PreSettlementPrice * (1.00m - (decimal)tradeRules.HighLowStopScopeValue / 100);
            //    return priceEntity;
            //}
            //if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice)
            //{
            //    priceEntity.CeilingPrice = (decimal)vtFutData.PreSettlementPrice + (decimal)tradeRules.HighLowStopScopeValue;
            //    priceEntity.CeilingPrice = (decimal)vtFutData.PreSettlementPrice - (decimal)tradeRules.HighLowStopScopeValue;
            //    return priceEntity;
            //}
            return null;
        }
        #endregion

        #region 获取【现货】初始默认值的涨跌幅价格实体
        /// <summary>
        /// 获取【现货】初始默认值的涨跌幅价格实体
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        private PriceEntity GetInitPriceEntity(string code)
        {
            HqExData vtHqExData = RealtimeMarketService.GetRealTimeStockDataByCommdityCode(code);
            if (vtHqExData == null)
                return null;
            var entity = new PriceEntity();
            //上限价=最后瞬时量*默认值（1）+默认限制值（0.05）
            entity.CeilingPrice = (decimal)(vtHqExData.LastVolume) * (RulesDefaultValue.DefaultValue + RulesDefaultValue.DefaultLimit);
            //下限价=最后瞬时量*默认值（1）-默认限制值（0.05）
            entity.LowerPrice = (decimal)(vtHqExData.LastVolume) * (RulesDefaultValue.DefaultValue - RulesDefaultValue.DefaultLimit);
            return entity;
        }
        #endregion

        #region 判断【期货】下单价格是否在涨跌幅之间（即是否可以下单）对其价格判断，并返回是否熔断
        /// <summary>
        /// 判断【期货】下单价格是否在涨跌幅之间（即是否可以下单）对其价格判断，并返回是否熔断 
        /// </summary>
        /// <param name="codePrice">代码价格</param>
        /// <param name="matchCode">撮合的期货商品代码</param>
        /// <param name="fuse">是否熔断标志(0不熔断，1熔断)</param>
        /// <returns></returns>
        public bool QHComparePriceByMatchCodeReturnIsFuse(decimal codePrice, string matchCode, ref int fuse)
        {
            fuse = 0;
            if (matchCode != null)
            {
                //VTFutData data = RealtimeMarketService.GetRealtimeMark().GetFutData(matchCode);
                FutData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetFutData(matchCode);
                if (data == null)
                {
                    return false;
                }
                bool isFuse = FuseManager.Instanse.IsFuse(matchCode);
                //if (data.OriginalData != null)
                //{
                if (isFuse)
                {
                    decimal triScale = FuseManager.Instanse.GetFuseTriggeringScaleByCommodityCode(matchCode);
                    fuse = 1;
                    var price = (decimal)data.PreSettlementPrice;
                    if (codePrice > price * (1 + triScale / 100))
                    {
                        return false;
                    }
                    if (codePrice < price * ((1 - triScale / 100)))
                    {
                        return false;
                    }
                }
                else
                {
                    fuse = 0;
                    //撮合中心计算价格类,获取跌涨幅
                    PriceEntity priceEntity = CostPriceCalculate.Instanse.GetPriceEntity(matchCode);
                    if (codePrice > priceEntity.CeilingPrice)
                    {
                        return false;
                    }
                    if (codePrice < priceEntity.LowerPrice)
                    {
                        return false;
                    }
                }
                //}
            }

            return true;
        }
        #endregion

        #region  判断【现货】下单价格是否在涨跌幅之间（即是否可以下单）对其价格判断
        /// <summary>
        /// 判断【现货】下单价格是否在涨跌幅之间（即是否可以下单）对其价格判断
        /// </summary>
        /// <param name="codePrice">价格</param>
        /// <param name="matchCode">撮合的商品代码</param>
        /// <param name="buySellDirection">买卖方式</param>
        /// <returns></returns>
        public bool XHComparePriceByMatchCode(decimal codePrice, string matchCode, Types.TransactionDirection buySellDirection)
        {
            PriceEntity priceEntity = GetPriceEntity(matchCode, buySellDirection);
            if (priceEntity != null)
            {
                int length = GetXHMinChangePriceDecimalDigit(matchCode);
                priceEntity.CeilingPrice = Math.Round(priceEntity.CeilingPrice, length, MidpointRounding.AwayFromZero);
                priceEntity.LowerPrice = Math.Round(priceEntity.LowerPrice, length, MidpointRounding.AwayFromZero);
                if (codePrice >= priceEntity.LowerPrice && codePrice <= priceEntity.CeilingPrice)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region  港股规则价格计算Update by:李健华 Update Date:2009-10-30

        #region 判断【港股】价格判断
        /// <summary>
        /// 港股价格规则判断入口
        /// </summary>
        /// <param name="hkorder">委托主体</param>
        /// <param name="data">行情主体</param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>       
        public bool HKComparePriceByMatchCode(HKEntrustOrderInfo hkorder, HKStock data, out string highLowStr)
        {
            highLowStr = "";
            if (hkorder == null || data == null)
            {
                return false;
            }

            try
            {
                if ((Types.TransactionDirection)hkorder.TradeType == Types.TransactionDirection.Buying)
                {
                    return ValidateHKBuy(hkorder, data, out highLowStr);
                }
                else
                {
                    return ValidateHKSell(hkorder, data, out highLowStr);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("港股价格规则异常" + hkorder.HKSecuritiesCode + "  " + hkorder.OrderPrice, ex);
                return false;
            }
        }

        #endregion

        #region 根据交易商品编号和交易方向计算【港股】涨跌幅度

        /// <summary>
        /// Tiele：根据交易商品编号和交易方向计算涨跌幅度
        /// Desc.:如果是新股上市或者是新增股直接调用
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="transactionType">交易方向类型</param>
        /// <returns></returns>
        public PriceEntity GetHKPriceEntity(string code, Types.TransactionDirection transactionType)
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 根据商品代码获取港股交易规则中最小价格小数位长度
        /// <summary>
        /// 根据商品代码获取港股交易规则中最小价格小数位长度
        /// 去除小数点最后为0无意义的位数.如果数据没有小数位（如：100-->2）返回默认有效小数位长度
        /// 如果所有的小数位都为0则返回默认有效小数为长度（如：100.000-->2）
        /// 如果有有效位数则返回有效位数（如：3.1010-->3，5.00002-->5,3.200-->1）
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns></returns>
        public int GetHKMinChangePriceDecimalDigit(string code)
        {
            //XH_SpotTradeRules rule = CommonDataManagerOperate.GetXH_SpotTradeRulesByCommodityCode(code);
            //if (rule == null)
            //    return RulesDefaultValue.DefaultLength;
            //return Utils.GetDecimalDigit((decimal)rule.MinChangePrice, true);
            return 0;
        }
        #endregion

        #region 限价盘、增强限价盘、特别限价盘规则验证

        /// <summary>
        /// 港股买方向规则校验：是否符合限价盘、增强限价盘、特别限价盘规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKBuy(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            highLowStr = "";
            if (orderEntity == null || hqData == null)
            {
                return false;
            }
            switch ((Types.HKPriceType)orderEntity.OrderType)
            {
                case Types.HKPriceType.LO:
                    return ValidateHKFixedBuy(orderEntity, hqData, out highLowStr);
                //break;
                case Types.HKPriceType.ELO:
                    return ValidateHKHardBuy(orderEntity, hqData, out highLowStr);
                //break;
                case Types.HKPriceType.SLO:
                    return ValidateHKSpecialBuy(orderEntity, hqData, out highLowStr);
                //break;
            }

            return false;
        }

        /// <summary>
        /// 港股买方向限价盘校验规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKFixedBuy(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            #region 相关定义
            highLowStr = "";
            //得到最佳卖盘价
            decimal bestSell = (decimal)hqData.Sellprice1;
            //得到最佳买盘价
            decimal bestBuy = (decimal)hqData.Buyprice1;
            //得到委托价格
            decimal orderPrice = orderEntity.OrderPrice;
            //得到最新成交价
            decimal lastTradePrice = (decimal)hqData.Lasttrade;
            //得到最小变动价位 都是用最新成交价来查找 石千松已经确认2009-10-30
            //计算最小变动价位不能以最新成交价来判断，以当前公式取到的来计算 石千松、王光辉已经确认2009-12-4
            // decimal least = GetMinPriceField(lastTradePrice);
            //得到按盘价
            decimal nominal = (decimal)hqData.NominalPrice;

            //最后沽盘价、上日收市价及当日最低成交价三者中的最低价
            decimal minSell = Math.Min(bestSell, Math.Min((decimal)hqData.PreClosePrice, (decimal)hqData.Low));

            //上限
            decimal upperLimit = nominal * 9;
            //下限
            decimal lowerLimit = (decimal)nominal / 9;

            #region new Create by:董鹏 2009-12-10
            //分段计算的下限价格
            decimal lowerPrice = 0.00m;
            #endregion
            #region new Create by:董鹏 2009-12-18
            string lowerLog = "";
            #endregion

            //写日志字符串
            string txt = "买一价：" + bestBuy + "，卖一价：" + bestSell + "，上日收市价：" + hqData.PreClosePrice + "，最高成交价：" + hqData.High + "，最低成交价：" + hqData.Low + "";
            txt += "最后成交价：" + lastTradePrice + "，按盘价：" + nominal;
            string txtHL = "";
            #endregion

            #region new Create by:李健华 2009-10-30
            #region  有现存买盘及沽盘
            if (bestBuy > 0.0m && bestSell > 0.0m)
            {
                //有现存买盘及沽盘
                //【最佳买盘价-24价位，最佳卖盘价】

                //内部获取最小变动价位
                decimal least = GetMinPriceField(bestBuy);
                txt += "，最小变动价位:" + least;
                txtHL += "==【限价盘】有现存买盘及沽盘的情况======买盘【最佳买盘价-24价位，最佳卖盘价】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(bestBuy, 24, ref lowerLog);

                #endregion

                #region 获取取值的交集

                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < bestBuy - least * 24.0m)
                //{
                //    highLowStr += "下限:=" + bestBuy + " -  24*" + least;

                //    lowerLimit = bestBuy - least * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #endregion

                if (upperLimit > bestSell)
                {
                    highLowStr += "   上限：" + bestSell;

                    upperLimit = bestSell;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }

                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                //highLowStr = "下限:=" + bestBuy + " -  24*" + least + "  上限：" + bestSell;
                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region  无现存买盘
            else if (bestBuy <= 0.0m && bestSell > 0.0m)
            {
                //无现存买盘 
                //【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价】

                //内部获取最小变动价位
                decimal least = GetMinPriceField(minSell);
                txt += "，最小变动价位:" + least;
                txtHL += "==【限价盘】没有现存买盘的情况===买盘【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(minSell, 24, ref lowerLog);

                #endregion

                #region 获取取值的交集
                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < minSell - least * 24.0m)
                //{
                //    highLowStr += "下限:=" + minSell + "-24*" + least;

                //    lowerLimit = minSell - least * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #endregion

                if (upperLimit > bestSell)
                {
                    highLowStr += "   上限：" + bestSell;

                    upperLimit = bestSell;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }

                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region  无现存卖盘
            else if (bestBuy > 0.0m && bestSell <= 0.0m)
            {
                //无现存卖盘
                //【最佳买盘价-24价位，9*按盘价】

                //内部获取最小变动价位
                decimal least = GetMinPriceField(bestBuy);
                txt += "，最小变动价位:" + least;
                txtHL += "==【限价盘】没有现存卖盘的情况====买盘【最佳买盘价-24价位，9*按盘价】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(bestBuy, 24, ref lowerLog);

                #endregion

                #region 获取取值的交集

                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < bestBuy - least * 24.0m)
                //{
                //    highLowStr += "下限:=" + bestBuy + "-24*" + least;

                //    lowerLimit = bestBuy - least * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #endregion

                highLowStr += "   上限：" + nominal + "*9";
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region  无现存买卖盘
            else if (bestBuy <= 0.0M && bestSell <= 0)
            {
                //无现存买卖盘
                //若没有上日收市价及当日最低成交价，输入价可高于或等于或低于最后卖盘价(即不受最后卖盘价限制）
                //【min(最后卖盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】

                //内部获取最小变动价位
                decimal least = GetMinPriceField(minSell);
                txt += "，最小变动价位:" + least;
                txtHL += "==【限价盘】没有现存买卖盘的情况===买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(minSell, 24, ref lowerLog);

                #endregion

                #region 获取取值的交集

                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < minSell - least * 24.0m)
                //{
                //    highLowStr += "下限:=" + minSell + "-24*" + least;

                //    lowerLimit = minSell - least * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #endregion

                highLowStr += "   上限：" + nominal + "*9";
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #endregion

            return false;
        }

        /// <summary>
        /// 港股买方向增强限价盘校验规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKHardBuy(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            #region 相关定义
            highLowStr = "";
            //得到最佳卖盘价
            decimal bestSell = (decimal)hqData.Sellprice1;
            //得到最佳买盘价
            decimal bestBuy = (decimal)hqData.Buyprice1;
            //得到委托价格
            decimal orderPrice = orderEntity.OrderPrice;
            //得到最新成交价
            decimal lastTradePrice = (decimal)hqData.Lasttrade;
            //计算最小变动价位不能以最新成交价来判断，2009-10-30以当前公式取到的来计算 石千松、王光辉已经确认2009-12-4
            // decimal least = GetMinPriceField(lastTradePrice);
            //得到按盘价
            decimal nominal = (decimal)hqData.NominalPrice;

            //最后沽盘价、上日收市价及当日最低成交价三者中的最低价
            decimal minSell = Math.Min(bestSell, Math.Min((decimal)hqData.PreClosePrice, (decimal)hqData.Low));
            //上限
            decimal upperLimit = nominal * 9;
            //下限
            decimal lowerLimit = (decimal)nominal / 9;

            #region new Create by:董鹏 2009-12-10
            //分段计算的下限价格
            decimal lowerPrice = 0.00m;
            //分段计算的上限价格
            decimal upperPrice = 0.00m;
            #endregion
            #region new Create by:董鹏 2009-12-18
            string lowerLog = "";
            string upperLog = "";
            #endregion

            //写日志字符串
            string txt = "买一价：" + bestBuy + "，卖一价：" + bestSell + "，上日收市价：" + hqData.PreClosePrice + "，最高成交价：" + hqData.High + "，最低成交价：" + hqData.Low + "";
            txt += "最后成交价：" + lastTradePrice + "，按盘价：" + nominal;
            string txtHL = "";
            #endregion

            #region new Create by:李健华 2009-10-30

            #region 有现存买盘及沽盘
            if (bestBuy > 0.0m && bestSell > 0.0m)
            {
                //有现存买盘及沽盘
                //委托价格<=最佳卖盘价以上4个价位；委托价格>=最佳买盘价以下24个价位
                // 【最佳买盘价-24价位，最佳卖盘价+4价位】

                //内部获取买最小变动价位
                decimal leastBuy = GetMinPriceField(bestBuy);
                //内部获取f卖最小变动价位
                decimal leastSell = GetMinPriceField(bestSell);

                txt += "，最小变动价位买：" + leastBuy + " 卖：" + leastSell;
                txtHL += "==【增强限价盘】有现存买盘及沽盘的情况===买盘【最佳买盘价-24价位，最佳卖盘价+4价位】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(bestBuy, 24, ref lowerLog);
                upperPrice = GetSegmentedUpperPrice(bestSell, 4, ref upperLog);

                #endregion

                #region 获取取值的交集

                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < bestBuy - leastBuy * 24.0m)
                //{
                //    highLowStr += "下限:=" + bestBuy + " -  24*" + leastBuy;

                //    lowerLimit = bestBuy - leastBuy * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                //if (upperLimit > bestSell + leastSell * 4.0m)
                //{
                //    highLowStr += "   上限：" + bestSell + "+" + leastSell + "*4";

                //    upperLimit = bestSell + leastSell * 4.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买盘
            else if (bestBuy <= 0.0m && bestSell > 0.0m)
            {
                //无现存买盘 
                //输入价介乎高于当时沽盘价四个价位，与及当时沽盘价、上日收市价及当日最低成交价三者中的最低价再低二十四个价位之间的价格
                //【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价+4价位】

                //内部获取最小变动价位
                decimal least = GetMinPriceField(minSell);
                //内部获取卖最小变动价位
                decimal leastSell = GetMinPriceField(bestSell);
                txt += "，最小变动价位：" + least + "卖:" + leastSell;
                txtHL += "==【增强限价盘】没有现存买盘的情况===买盘【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价+4价位】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(minSell, 24, ref lowerLog);
                upperPrice = GetSegmentedUpperPrice(bestSell, 4, ref upperLog);

                #endregion

                #region 获取取值的交集
                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < minSell - least * 24.0m)
                //{
                //    highLowStr += "下限:=" + minSell + "-24*" + least;

                //    lowerLimit = minSell - least * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                //if (upperLimit > bestSell + leastSell * 4.0m)
                //{
                //    highLowStr += "   上限：" + bestSell + "+" + leastSell + "*4";

                //    upperLimit = bestSell + leastSell * 4.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存卖盘
            else if (bestBuy > 0.0m && bestSell <= 0.0m)
            {
                //无现存卖盘
                //输入价高于或等于当时买盘价减二十四个价位的价格
                //【最佳买盘价-24价位，9*按盘价】

                //内部获取最小变动价位
                decimal least = GetMinPriceField(bestBuy);
                txt += "，最小变动价位：" + least;
                txtHL += "==【增强限价盘】没有现存卖盘的情况====买盘【最佳买盘价-24价位，9*按盘价】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(bestBuy, 24, ref lowerLog);

                #endregion

                #region 获取取值的交集
                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < bestBuy - least * 24.0m)
                //{
                //    highLowStr += "下限:=" + bestBuy + "-24*" + least;

                //    lowerLimit = bestBuy - least * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #endregion
                highLowStr += "   上限：" + nominal + "*9";
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买卖盘
            else if (bestBuy <= 0.0M && bestSell <= 0)
            {
                //无现存买卖盘
                //输入价高于或等于最后沽盘价、上日收市价及当日最低成交价三者中的最低价再低二十四个价位的价格
                //【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】

                //内部获取最小变动价位
                decimal least = GetMinPriceField(minSell);
                txt += "，最小变动价位：" + least;
                txtHL += "==【增强限价盘】没有现存买卖盘的情况===买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==";

                #region new Create by:董鹏 2009-12-10

                lowerPrice = GetSegmentedLowerPrice(minSell, 24, ref lowerLog);

                #endregion

                #region 获取取值的交集

                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < minSell - least * 24.0m)
                //{
                //    highLowStr += "下限:=" + minSell + "-24*" + least;

                //    lowerLimit = minSell - least * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #endregion

                highLowStr += "   上限：" + nominal + "*9";
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);
                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #endregion
            return false;
        }

        /// <summary>
        /// 港股买方向特别限价盘校验规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKSpecialBuy(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            #region 相关定义
            highLowStr = "";
            //得到最佳卖盘价
            decimal bestSell = (decimal)hqData.Sellprice1;
            //得到最佳买盘价
            decimal bestBuy = (decimal)hqData.Buyprice1;
            //得到委托价格
            decimal orderPrice = orderEntity.OrderPrice;

            decimal lastTradePrice = (decimal)hqData.Lasttrade;
            //得到最小变动价位 都是用最新成交价来查找 石千松已经确认2009-10-30
            //decimal least = GetMinPriceField(lastTradePrice); ;
            //得到按盘价
            decimal nominal = (decimal)hqData.NominalPrice;

            //最后沽盘价、上日收市价及当日最低成交价三者中的最低价
            decimal minSell = Math.Min(bestSell, Math.Min((decimal)hqData.PreClosePrice, (decimal)hqData.Low));
            //上限
            decimal upperLimit = nominal * 9;
            //下限
            decimal lowerLimit = (decimal)nominal / 9;

            //写日志字符串
            string txt = "买一价：" + bestBuy + "，卖一价：" + bestSell + "，上日收市价：" + hqData.PreClosePrice + "，最高成交价：" + hqData.High + "，最低成交价：" + hqData.Low + "";
            txt += "最后成交价：" + lastTradePrice + "，按盘价：" + nominal;
            string txtHL = "";
            #endregion

            #region new Create by:李健华 2009-10-30

            #region 有现存买盘及沽盘
            if (bestBuy > 0.0m && bestSell > 0.0m)
            {
                //有现存买盘及沽盘
                //输入价高于或等于当时沽盘价
                txtHL += "==【特别限价盘】有现存买盘及沽盘的情况===买盘【最佳卖盘价，9*按盘价】==";

                #region 获取取值的交集
                if (lowerLimit < bestSell)
                {
                    highLowStr += "下限:=" + bestSell;

                    lowerLimit = bestSell;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                highLowStr += "   上限：" + nominal + "*9";


                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
            }
            #endregion

            #region  无现存买盘
            else if (bestBuy <= 0.0m && bestSell > 0.0m)
            {
                //无现存买盘 
                //输入价高于或等于当时沽盘价
                txtHL += "==【特别限价盘】没有现存买盘的情况==买盘【最佳卖盘价，9*按盘价】==";
                #region 获取取值的交集
                if (lowerLimit < bestSell)
                {
                    highLowStr += "下限:=" + bestSell;

                    lowerLimit = bestSell;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                highLowStr += "   上限：" + nominal + "*9";


                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
            }
            #endregion

            #region 无现在卖盘
            else if (bestBuy > 0.0m && bestSell <= 0.0m)
            {
                txtHL += "==【特别限价盘】没有现存卖盘的情况===买盘【9/按盘价，9*按盘价】==";
                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";

                highLowStr += "   上限：" + nominal + "*9";

                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
            }
            #endregion

            #region 无现存买卖盘或者无现存卖盘
            else if (bestBuy <= 0.0m && bestSell <= 0.0m)
            {
                //无现存买卖盘或者无现存卖盘，无条件限制
                txtHL += "==【特别限价盘】没有现存买卖盘的情况===买盘【9/按盘价，9*按盘价】==";
                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";

                highLowStr += "   上限：" + nominal + "*9";

                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
            }

            #endregion

            #endregion

            return false;
        }

        /// <summary>
        /// 港股卖方向规则校验：是否符合限价盘、增强限价盘、特别限价盘规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKSell(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            highLowStr = "";
            if (orderEntity == null || hqData == null)
            {
                return false;
            }
            switch ((Types.HKPriceType)orderEntity.OrderType)
            {
                case Types.HKPriceType.LO:
                    return ValidateHKFixedSell(orderEntity, hqData, out highLowStr);
                //break;
                case Types.HKPriceType.ELO:
                    return ValidateHKHardSell(orderEntity, hqData, out highLowStr);
                //break;
                case Types.HKPriceType.SLO:
                    return ValidateHKSpecialSell(orderEntity, hqData, out highLowStr);
                //break;
            }

            return false;
        }

        /// <summary>
        /// 港股卖方向限价盘校验规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKFixedSell(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            #region 相关定义
            highLowStr = "";
            //得到最佳卖盘价
            decimal bestSell = (decimal)hqData.Sellprice1;
            //得到最佳买盘价
            decimal bestBuy = (decimal)hqData.Buyprice1;
            //得到委托价格
            decimal orderPrice = orderEntity.OrderPrice;
            //得到最新成交价
            decimal lastTradePrice = (decimal)hqData.Lasttrade;
            //得到最小变动价位 都是用最新成交价来查找 石千松已经确认2009-10-30
            //decimal least = GetMinPriceField(lastTradePrice); ;
            //得到按盘价
            decimal nominal = (decimal)hqData.NominalPrice;//按盘价

            decimal maxBuy = Math.Max(bestBuy, Math.Max((decimal)hqData.PreClosePrice, (decimal)hqData.High));

            //上限
            decimal upperLimit = nominal * 9;
            //下限
            decimal lowerLimit = (decimal)nominal / 9;

            #region new Create by:董鹏 2009-12-10
            //分段计算的上限价格
            decimal upperPrice = 0.00m;
            #endregion
            #region new Create by:董鹏 2009-12-18
            string upperLog = "";
            #endregion

            //写日志字符串
            string txt = "买一价：" + bestBuy + "，卖一价：" + bestSell + "，上日收市价：" + hqData.PreClosePrice + "，最高成交价：" + hqData.High + "，最低成交价：" + hqData.Low + "";
            txt += "最后成交价：" + lastTradePrice + "，按盘价：" + nominal;
            string txtHL = "";
            #endregion

            #region new Create by:李健华 2009-10-30
            #region 有现存买盘及沽盘
            if (bestBuy > 0.0m && bestSell > 0.0m)
            {
                //有现存买盘及沽盘
                //委托价格<=最佳卖盘价；委托价格>=最佳买盘价以下24个价位；按盘价/9=<委托价格<=按盘价*9
                //【最佳买盘价，最佳卖盘价+24价位】
                txtHL += "==【限价盘】有现存买盘及沽盘的情况==卖盘【最佳买盘价，最佳卖盘价+24价位】==";

                //内部获取f卖最小变动价位
                decimal leastSell = GetMinPriceField(bestSell);
                txt += "，最小变动价位卖：" + leastSell;

                #region new Create by:董鹏 2009-12-10

                upperPrice = GetSegmentedUpperPrice(bestSell, 24, ref upperLog);

                #endregion

                #region 获取取值的交集
                if (lowerLimit < bestBuy)
                {
                    highLowStr += "下限:=" + bestBuy;

                    lowerLimit = bestBuy;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #region old Comment by:董鹏 2009-12-10
                //if (upperLimit > bestSell + leastSell * 24.0m)
                //{
                //    highLowStr += "   上限：" + bestSell + "+" + leastSell + "*24";

                //    upperLimit = bestSell + leastSell * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10

                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买盘
            else if (bestBuy <= 0.0m && bestSell > 0.0m)
            {
                //无现存买盘 
                //输入价低于或等于当时沽盘价加二十四个价位的价格
                //（0，最佳卖盘价+24价位】
                txtHL += "==【限价盘】没有现存买盘的情况===卖盘【按盘价/9倍，最佳卖盘价+24价位】==";

                //内部获取f卖最小变动价位
                decimal leastSell = GetMinPriceField(bestSell);
                txt += "，最小变动价位卖：" + leastSell;

                #region new Create by:董鹏 2009-12-10

                upperPrice = GetSegmentedUpperPrice(bestSell, 24, ref upperLog);

                #endregion

                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";

                #region old Comment by:董鹏 2009-12-10
                //if (upperLimit > bestSell + leastSell * 24.0m)
                //{
                //    highLowStr += "   上限：" + bestSell + "+" + leastSell + "*24";

                //    upperLimit = bestSell + leastSell * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);
                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存卖盘
            else if (bestBuy > 0.0m && bestSell <= 0.0m)
            {
                //无现存卖盘
                //输入价介乎当时买盘价，与及当时买盘价、上日收市价及当日最高成交价三者中的最高价再高二十四个价位之间的价格
                //【最佳买盘价，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】
                txtHL += "==【限价盘】没有现存卖盘的情况==卖盘【最佳买盘价，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】";
                //内部获取f卖最小变动价位
                decimal leastMax = GetMinPriceField(maxBuy);
                txt += "，最小变动价位：" + leastMax;

                #region new Create by:董鹏 2009-12-10

                upperPrice = GetSegmentedUpperPrice(maxBuy, 24, ref upperLog);

                #endregion

                #region 获取取值的交集
                if (lowerLimit < bestBuy)
                {
                    highLowStr += "下限:=" + bestBuy;

                    lowerLimit = bestBuy;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                #region old Comment by:董鹏 2009-12-10
                //if (upperLimit > maxBuy + leastMax * 24.0m)
                //{
                //    highLowStr += "   上限：" + maxBuy + "+" + leastMax + "*24";

                //    upperLimit = maxBuy + leastMax * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买卖盘
            else if (bestBuy <= 0.0m && bestSell <= 0.0m)
            {
                //无现存买卖盘
                //输入价低于或等于最后买盘价、上日收市价及当日最高成交价三者中最高价再高二十四个价位的价格。
                //（0，max(最后买盘价，上日收市价，当日最高成交价)+24价位】
                //若没有上日收市价及当日最高成交价，输入价可低于或等于或高于最后买盘价
                txtHL += "==【限价盘】没有现存买卖盘的情况===卖盘（按盘价/9，max(最后买盘价，上日收市价，当日最高成交价)+24价位】==";
                //内部获取f卖最小变动价位
                decimal leastMax = GetMinPriceField(maxBuy);
                txt += "，最小变动价位：" + leastMax;

                #region new Create by:董鹏 2009-12-10

                upperPrice = GetSegmentedUpperPrice(maxBuy, 24, ref upperLog);

                #endregion

                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";

                #region old Comment by:董鹏 2009-12-10
                //if (upperLimit > maxBuy + leastMax * 24.0m)
                //{
                //    highLowStr += "   上限：" + maxBuy + "+" + leastMax + "*24";

                //    upperLimit = maxBuy + leastMax * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice > lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #endregion
            return false;
        }

        /// <summary>
        /// 港股卖方向增强限价盘校验规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKHardSell(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            #region 相关定义
            highLowStr = "";
            //得到最佳卖盘价
            decimal bestSell = (decimal)hqData.Sellprice1;
            //得到最佳买盘价
            decimal bestBuy = (decimal)hqData.Buyprice1;
            //得到委托价格
            decimal orderPrice = orderEntity.OrderPrice;
            //得到最新成交价
            decimal lastTradePrice = (decimal)hqData.Lasttrade;
            //得到最小变动价位 都是用最新成交价来查找 石千松已经确认2009-10-30
            //decimal least = GetMinPriceField(lastTradePrice);
            //得到按盘价
            decimal nominal = (decimal)hqData.NominalPrice;//按盘价

            decimal maxBuy = Math.Max(bestBuy, Math.Max((decimal)hqData.PreClosePrice, (decimal)hqData.High));

            //上限
            decimal upperLimit = nominal * 9;
            //下限
            decimal lowerLimit = (decimal)nominal / 9;

            #region new Create by:董鹏 2009-12-10
            //分段计算的下限价格
            decimal lowerPrice = 0.00m;
            //分段计算的上限价格
            decimal upperPrice = 0.00m;
            #endregion
            #region new Create by:董鹏 2009-12-18
            string lowerLog = "";
            string upperLog = "";
            #endregion

            //写日志字符串
            string txt = "买一价：" + bestBuy + "，卖一价：" + bestSell + "，上日收市价：" + hqData.PreClosePrice + "，最高成交价：" + hqData.High + "，最低成交价：" + hqData.Low + "";
            txt += "最后成交价：" + lastTradePrice + "，按盘价：" + nominal;
            string txtHL = "";
            #endregion

            #region new Create by:李健华 2009-10-30

            #region 有现存买盘及沽盘
            if (bestBuy > 0.0m && bestSell > 0.0m)
            {
                //有现存买盘及沽盘
                //输入价介乎高于当时沽盘价二十四个价位与及低于当时买盘价四个价位
                //【最佳买盘价-4价位，最佳卖盘价+24价位】
                txtHL += "==【增强限价盘】有现存买盘及沽盘的情况==卖盘【最佳买盘价-4价位，最佳卖盘价+24价位】";

                //内部获取f买最小变动价位
                decimal leastBuy = GetMinPriceField(bestBuy);
                //内部获取f卖最小变动价位
                decimal leastSell = GetMinPriceField(bestSell);
                txt += "，最小变动价位买：" + leastBuy + "卖：" + leastSell;

                #region new Create by:董鹏 2009-12-10
                lowerPrice = GetSegmentedLowerPrice(bestBuy, 4, ref lowerLog);
                upperPrice = GetSegmentedUpperPrice(bestSell, 24, ref upperLog);
                #endregion

                #region 获取取值的交集
                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < bestBuy - 4 * leastBuy)
                //{
                //    highLowStr += "下限:=" + bestBuy + "-4*" + leastBuy;

                //    lowerLimit = bestBuy - 4 * leastBuy;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                //if (upperLimit > bestSell + leastSell * 24.0m)
                //{
                //    highLowStr += "   上限：" + bestSell + "+" + leastSell + "*24";

                //    upperLimit = bestSell + leastSell * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买盘
            else if (bestBuy <= 0.0m && bestSell > 0.0m)
            {
                //无现存买盘 
                //输入价介乎当时沽盘价，与及当时沽盘价、上日收市价及当日最低成交价三者中的最低价再低二十四个价位之间的价格
                //（0，最佳卖盘价+24价位】
                txtHL += "==【增强限价盘】没有现存买盘的情况===卖盘【按盘价/9倍，最佳卖盘价+24价位】";

                //内部获取f卖最小变动价位
                decimal leastSell = GetMinPriceField(bestSell);
                txt += "，最小变动价位卖：" + leastSell;

                #region new Create by:董鹏 2009-12-10
                upperPrice = GetSegmentedUpperPrice(bestSell, 24, ref upperLog);
                #endregion

                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";

                #region old Comment by:董鹏 2009-12-10
                //if (upperLimit > bestSell + leastSell * 24.0m)
                //{
                //    highLowStr += "   上限：" + bestSell + "+" + leastSell + "*24";

                //    upperLimit = bestSell + leastSell * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region  无现存卖盘
            else if (bestBuy > 0.0m && bestSell <= 0.0m)
            {
                //无现存卖盘
                //输入价介乎低于当时买盘价四个价位，与及当时买盘价、上日收市价及当日最高成交价三者中的最高价再高二十四个价位之间的价格
                //【最佳买盘价-4价位，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】
                txtHL += "==【增强限价盘】没有现存卖盘的情况==卖盘【最佳买盘价-4价位，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】";
                //内部获取f买最小变动价位
                decimal leastBuy = GetMinPriceField(bestBuy);
                //内部获取最小变动价位
                decimal leastMax = GetMinPriceField(maxBuy);
                txt += "，最小变动价位买：" + leastBuy + "三个最大：" + leastMax;

                #region new Create by:董鹏 2009-12-10
                lowerPrice = GetSegmentedLowerPrice(bestBuy, 4, ref lowerLog);
                upperPrice = GetSegmentedUpperPrice(maxBuy, 24, ref upperLog);
                #endregion

                #region 获取取值的交集

                #region old Comment by:董鹏 2009-12-10
                //if (lowerLimit < bestBuy - 4 * leastBuy)
                //{
                //    highLowStr += "下限:=" + bestBuy + "-4*" + leastBuy;

                //    lowerLimit = bestBuy - 4 * leastBuy;
                //}
                //else
                //{
                //    highLowStr += "下限:=" + nominal + "/9";
                //}
                //if (upperLimit > maxBuy + leastMax * 24.0m)
                //{
                //    highLowStr += "   上限：" + maxBuy + "+" + leastMax + "*24";

                //    upperLimit = maxBuy + leastMax * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion

                #region new Create by:董鹏 2009-12-10
                if (lowerLimit < lowerPrice)
                {
                    highLowStr += "下限:=" + lowerLog + "=" + lowerPrice;

                    lowerLimit = lowerPrice;
                }
                else
                {
                    highLowStr += "下限:=" + nominal + "/9";
                }
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买卖盘
            else if (bestBuy <= 0.0M && bestSell <= 0)
            {
                //无现存买卖盘
                //输入价低于或等于最后买盘价、上日收市价及当日最高成交价三者中最高价再高二十四个价位的价格。
                //（0，max(最后买盘价，上日收市价，当日最高成交价)+24价位】
                txtHL += "==【增强限价盘】没有现存买卖盘的情况==卖盘（按盘价/9，max(最后买盘价，上日收市价，当日最高成交价)+24价位】";
                //内部获取最小变动价位
                decimal leastMax = GetMinPriceField(maxBuy);

                #region new Create by:董鹏 2009-12-10
                upperPrice = GetSegmentedUpperPrice(maxBuy, 24, ref upperLog);
                #endregion

                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";

                #region old Comment by:董鹏 2009-12-10
                //if (upperLimit > maxBuy + leastMax * 24.0m)
                //{
                //    highLowStr += "   上限：" + maxBuy + "+" + leastMax + "*24";

                //    upperLimit = maxBuy + leastMax * 24.0m;
                //}
                //else
                //{
                //    highLowStr += "   上限：" + nominal + "*9";
                //}
                #endregion
                #region new Create by:董鹏 2009-12-10
                if (upperLimit > upperPrice)
                {
                    highLowStr += "   上限：" + upperLog + "=" + upperPrice;

                    upperLimit = upperPrice;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice > lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion
            #endregion
            return false;
        }

        /// <summary>
        /// 港股卖方向特别限价盘校验规则
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="hqData"></param>
        /// <param name="highLowStr">上下限计算式值</param>
        /// <returns></returns>
        private bool ValidateHKSpecialSell(HKEntrustOrderInfo orderEntity, HKStock hqData, out string highLowStr)
        {
            #region 相关定义
            highLowStr = "";
            //得到最佳卖盘价
            decimal bestSell = (decimal)hqData.Sellprice1;
            //得到最佳买盘价
            decimal bestBuy = (decimal)hqData.Buyprice1;
            //得到委托价格
            decimal orderPrice = orderEntity.OrderPrice;

            //得到按盘价
            decimal nominal = (decimal)hqData.NominalPrice;//按盘价
            //得到最新成交价
            decimal lastTradePrice = (decimal)hqData.Lasttrade;

            //得到最小变动价位 都是用最新成交价来查找 石千松已经确认2009-10-30
            //decimal least = GetMinPriceField(lastTradePrice);
            decimal maxBuy = Math.Max(bestBuy, Math.Max((decimal)hqData.PreClosePrice, (decimal)hqData.High));

            //上限
            decimal upperLimit = nominal * 9;
            //下限
            decimal lowerLimit = (decimal)nominal / 9;

            //写日志字符串
            string txt = "买一价：" + bestBuy + "，卖一价：" + bestSell + "，上日收市价：" + hqData.PreClosePrice + "，最高成交价：" + hqData.High + "，最低成交价：" + hqData.Low + "";
            txt += "最后成交价：" + lastTradePrice + "，按盘价：" + nominal;
            string txtHL = "";
            #endregion

            #region new Create by:李健华 2009-10-30

            #region 有现存买盘及沽盘
            if (bestBuy > 0.0m && bestSell > 0.0m)
            {
                //有现存买盘及沽盘
                //输入价低于或等于当时买盘价
                //(0，最佳买盘价】
                txtHL += "==【特别限价盘】有现存买盘及沽盘的情况==买盘【9/按盘价，最价买盘】";

                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";

                if (upperLimit > bestBuy)
                {
                    highLowStr += "   上限：" + bestBuy;

                    upperLimit = bestBuy;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买盘
            else if (bestBuy <= 0.0m && bestSell > 0.0m)
            {
                //无现存买盘 
                //无特殊限制
                txtHL += "==【特别限价盘】没有现存买盘的情况==卖盘【9/按盘价，9*按盘价】";
                txtHL += "==下限：=" + nominal + "/9  上限：=" + nominal + "*9";

                #region 获取取值的交集
                highLowStr += "下限:=" + nominal + "/9";
                highLowStr += "   上限：" + nominal + "*9";
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存卖盘
            else if (bestBuy > 0.0m && bestSell <= 0.0m)
            {
                //无现存卖盘
                //【9/按盘价，最佳买盘价】
                txtHL += "==【特别限价盘】没有现存卖盘的情况==卖盘【9/按盘价，最佳买盘价】";

                #region 获取取值的交集

                highLowStr += "下限:=" + nominal + "/9";
                if (upperLimit > bestBuy)
                {
                    highLowStr += "   上限：" + bestBuy;

                    upperLimit = bestBuy;
                }
                else
                {
                    highLowStr += "   上限：" + nominal + "*9";
                }
                #endregion

                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);
                highLowStr = "下限：=" + nominal + "/9  上限：=" + bestBuy;

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #region 无现存买卖盘
            else if (bestBuy <= 0.0M && bestSell <= 0)
            {
                //无现存买卖盘
                txtHL += "==【特别限价盘】没有现存买卖盘的情况===卖盘【9/按盘价，9*按盘价】";

                #region 获取取值的交集
                highLowStr += "下限:=" + nominal + "/9";
                highLowStr += "   上限：" + nominal + "*9";
                #endregion
                LogHelper.WriteDebug(DateTime.Now.ToString() + txt + txtHL + highLowStr);

                if (orderPrice >= lowerLimit && orderPrice <= upperLimit)
                {
                    return true;
                }
                return false;
            }
            #endregion

            #endregion

            return false;
        }

        #endregion

        #region 根据当前价格获取最小变动价位

        /// <summary>
        /// 根据价格获取最小变动价位
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        private decimal GetMinPriceField(decimal price)
        {

            HK_MinPriceFieldRange minrange = CommonDataManagerOperate.GetMinPriceFieldByPrice(price);
            if (minrange != null)
            {
                return minrange.Value.Value;
            }

            //此处处理再好好考虑一下
            return 0.0m;

        }

        #endregion

        #region 获取分段计算的价格上、下限

        #region new Create by : 董鹏 2009-12-10

        /// <summary>
        /// 获取分段计算的价格上限
        /// </summary>
        /// <param name="price">最佳价格</param>
        /// <param name="level">价位数</param>
        /// <param name="log">计算公式提示语</param>
        /// <returns>分段计算的价格上限</returns>
        public decimal GetSegmentedUpperPrice(decimal price, decimal level, ref string log)
        {
            //当前价格区间
            HK_MinPriceFieldRange range = CommonDataManagerOperate.GetMinPriceFieldByPrice(price);
            if (range == null)
            {
                log = "根据价格：" + price + "获取最小变动价位为null";
                return 0;
            }
            //下一个价格区间
            HK_MinPriceFieldRange rangeNext = CommonDataManagerOperate.GetMinPriceFieldByPrice(range.UpperLimit.Value + range.Value.Value);

            if (rangeNext == null)
            {
                log = "根据价格：" + range.LowerLimit.Value + "-" + range.Value.Value + "获取最小变动价位为null";
                return 0;
            }
            //计算当前区间可减去的价位数
            decimal n1 = (range.UpperLimit.Value - price) / range.Value.Value;
            if (level <= n1)
            {
                log = price + " + " + level + " * " + range.Value.Value;
                return price + level * range.Value.Value;
            }
            else
            {
                log = price + " + " + n1 + " * " + range.Value.Value + " + (" + level + " - " + n1 + ") * " + rangeNext.Value.Value;
                return price + n1 * range.Value.Value + (level - n1) * rangeNext.Value.Value;
            }
        }

        /// <summary>
        /// 获取分段计算的价格下限
        /// </summary>
        /// <param name="price">最佳价格</param>
        /// <param name="level">价位数</param>
        /// <param name="log">计算提示语</param>
        /// <returns>分段计算的价格下限</returns>
        public decimal GetSegmentedLowerPrice(decimal price, decimal level, ref string log)
        {
            //当前价格区间
            HK_MinPriceFieldRange range = CommonDataManagerOperate.GetMinPriceFieldByPrice(price);
            //update 2010-05-26 李健华
            if (range == null)
            {
                log = "根据价：" + price + "获取最小变动价位为null";
                return 0;
            }
            //上一个价格区间
            HK_MinPriceFieldRange rangeLast = CommonDataManagerOperate.GetMinPriceFieldByPrice(range.LowerLimit.Value - range.Value.Value);
            if (rangeLast == null)
            {
                log = "根据价：" + range.LowerLimit.Value + "-" + range.Value.Value + "获取最小变动价位为null";
                return 0;
            }
            //计算当前区间可减去的价位数
            decimal n1 = (price - range.LowerLimit.Value) / range.Value.Value;
            if (level <= n1)
            {
                log = price + " - " + level + " * " + range.Value.Value;
                return price - level * range.Value.Value;
            }
            else
            {
                log = price + " - " + n1 + " * " + range.Value.Value + " - (" + level + " - " + n1 + ") * " + rangeLast.Value.Value;
                return price - n1 * range.Value.Value - (level - n1) * rangeLast.Value.Value;
            }
        }

        #endregion

        #endregion

        #endregion

        #region 商品期货价格涨跌幅验证 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货价格涨跌幅验证
        /// </summary>
        /// <param name="codePrice">代码价格</param>
        /// <param name="matchCode">撮合的期货商品代码</param>
        /// <returns></returns>
        public bool CommoditiesComparePriceByMatchCode(decimal codePrice, string matchCode)
        {

            if (matchCode != null)
            {
                MerFutData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetMercantileFutData(matchCode);
                if (data == null)
                {
                    return false;
                }

                //if (data.OriginalData != null)
                //{
                //撮合中心计算价格类,获取跌涨幅
                PriceEntity priceEntity = GetCommoditiesPriceEntity(matchCode);
                if (codePrice > priceEntity.CeilingPrice)
                {
                    return false;
                }
                if (codePrice < priceEntity.LowerPrice)
                {
                    return false;
                }
                //}
            }
            return true;
        }

        /// <summary>
        /// 根据交易商品编号计算【商品期货】涨跌幅度
        /// </summary>
        /// <param name="matchCode"></param>
        /// <returns></returns>
        public PriceEntity GetCommoditiesPriceEntity(string matchCode)
        {
            MerFutData vtFutData = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetMercantileFutData(matchCode);
            PriceEntity priceEntity = new PriceEntity();
            //实体不能为空
            if (vtFutData == null)
            {
                return null;
            }
            //期货交易规则
            QH_FuturesTradeRules tradeRules = CommonDataManagerOperate.GetQH_FuturesTradeRulesByCommodityCode(matchCode);

            //期货交易规则不能为空
            if (tradeRules == null)
            {
                return null;
            }

            //1、新品种合约上市当日涨跌幅
            if (CommonDataManagerOperate.IsNewBreedClassByCode(matchCode))
            {
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice)
                {
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice * (1.00m + (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewBreedFuturesPactHighLowStopValue) / 100);
                    priceEntity.LowerPrice = (decimal)vtFutData.PreClearPrice * (1.00m - (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewBreedFuturesPactHighLowStopValue) / 100);
                    return priceEntity;
                }
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice)
                {
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice + (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewBreedFuturesPactHighLowStopValue);
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice - (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewBreedFuturesPactHighLowStopValue);
                    return priceEntity;
                }
            }
            //2、新月份合约上市当日涨跌幅
            else if (CommonDataManagerOperate.IsNewMonthBreedClassByCode(matchCode))
            {
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice)
                {
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice * (1.00m + (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewMonthFuturesPactHighLowStopValue) / 100);
                    priceEntity.LowerPrice = (decimal)vtFutData.PreClearPrice * (1.00m - (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewMonthFuturesPactHighLowStopValue) / 100);
                    return priceEntity;
                }
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice)
                {
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice + (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewMonthFuturesPactHighLowStopValue);
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice - (decimal)(tradeRules.HighLowStopScopeValue.Value * tradeRules.NewMonthFuturesPactHighLowStopValue);
                    return priceEntity;
                }
            }
            //3、交割月涨跌幅
            else if (CommonDataManagerOperate.IsDeliveryMonth(matchCode) && tradeRules.DeliveryMonthHighLowStopValue.HasValue)
            {
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice)
                {
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice * (1.00m + (decimal)tradeRules.DeliveryMonthHighLowStopValue / 100);
                    priceEntity.LowerPrice = (decimal)vtFutData.PreClearPrice * (1.00m - (decimal)tradeRules.DeliveryMonthHighLowStopValue / 100);
                    return priceEntity;
                }
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice)
                {
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice + (decimal)tradeRules.DeliveryMonthHighLowStopValue;
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice - (decimal)tradeRules.DeliveryMonthHighLowStopValue;
                    return priceEntity;
                }
            }
            //4、涨跌幅
            else
            {
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice)
                {

                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice * (1.00m + (decimal)tradeRules.HighLowStopScopeValue / 100);
                    priceEntity.LowerPrice = (decimal)vtFutData.PreClearPrice * (1.00m - (decimal)tradeRules.HighLowStopScopeValue / 100);
                    return priceEntity;
                }
                if (tradeRules.HighLowStopScopeID == (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice)
                {
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice + (decimal)tradeRules.HighLowStopScopeValue;
                    priceEntity.CeilingPrice = (decimal)vtFutData.PreClearPrice - (decimal)tradeRules.HighLowStopScopeValue;
                    return priceEntity;
                }
            }
            return null;
        }
        #endregion
    }
}