#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using RealTime.Server.SModelData.HqData;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.DAL.HKTradingRulesService;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate
{
    /// <summary>
    /// 涨跌幅处理器,错误编码范围1800-1850
    /// 作者：宋涛
    /// 日期：2008-11-21
    /// Desc.:修改港股的验证上下限计算方法，按照新的需求做了分段计算处理
    /// Update By:董鹏
    /// Update Date:2009-12-11
    /// Desc.:增加了商品期货涨跌幅计算方法
    /// Update By:董鹏
    /// Update Date:2010-01-26
    /// Desc: 增加了股指期货"季月合约上市首日涨跌幅","合约最后交易日涨跌幅"的计算方法
    /// Update By:董鹏
    /// Update Date:2010-03-05
    /// </summary>
    public class HighLowRangeProcessor
    {
        private ObjectCache<string, HighLowRange> highLowRangeList = new ObjectCache<string, HighLowRange>();

        public void Reset()
        {
            highLowRangeList.Reset();
        }

        /// <summary>
        /// 根据商品代码获取涨跌幅值
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <returns>涨跌幅值</returns>
        public HighLowRangeValue GetHighLowRangeValueByCommodityCode(string code, decimal orderPrice)
        {
            int? breedClassType = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            if (!breedClassType.HasValue)
                return null;

            HighLowRangeValue value = null;

            Types.BreedClassTypeEnum breedClassTypeEnum;
            try
            {
                breedClassTypeEnum = (Types.BreedClassTypeEnum)breedClassType.Value;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return null;
            }

            switch (breedClassTypeEnum)
            {
                case Types.BreedClassTypeEnum.Stock:
                    value = GetStockHighLowRangeValueByCommodityCode(code, orderPrice);
                    break;
                case Types.BreedClassTypeEnum.CommodityFuture:
                    value = GetCommoditiesHighLowRangeValueByCommodityCode(code, orderPrice);
                    break;
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    value = GetFutureHighLowRangeValueByCommodityCode(code, orderPrice);
                    break;
            }

            if (value != null)
                ProcessMinChangeValue(code, orderPrice, value);

            return value;
        }



        /// <summary>
        /// 获取港股涨跌幅(有效申报)
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="priceType">报价类型</param>
        /// <param name="tranType">卖买方向</param>
        /// <returns></returns>
        public HighLowRangeValue GetHKStockHighLowRangeValueByCommodityCode(string code, decimal orderPrice, Types.HKPriceType priceType, Types.TransactionDirection tranType)
        {
            HighLowRangeValue value = null;
            try
            {
                HKRangeValue lr = new HKRangeValue();
                lr = GetHKBuyHighLowRangeValue(code, orderPrice, priceType);
                value = new HighLowRangeValue();
                value.RangeType = Types.HighLowRangeType.HongKongPrice;
                value.HongKongRangeValue = lr;
                switch (tranType)
                {
                    case Types.TransactionDirection.Buying:
                        value.HighRangeValue = lr.BuyHighRangeValue;
                        value.LowRangeValue = lr.BuyLowRangeValue;
                        break;
                    case Types.TransactionDirection.Selling:
                        value.HighRangeValue = lr.SellHighRangeValue;
                        value.LowRangeValue = lr.SellLowRangeValue;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return null;
            }
            return value;

        }
        /// <summary>
        /// 根据最小变动价位进行四舍五入处理
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="value">涨跌幅原始对象</param>
        private void ProcessMinChangeValue(string code, decimal orderPrice, HighLowRangeValue value)
        {
            decimal min = GetMinChangePrice(code, orderPrice);

            if (min == 0)
                return;

            string minStr = Convert.ToString(min);
            string[] strs = minStr.Split('.');
            if (strs.Length < 2)
                return;
            string round = strs[1];

            while (round[round.Length - 1] == '0')
            {
                round = round.Substring(0, round.Length - 1);
            }

            int len = round.Length;
            if (len <= 0)
                return;

            value.HighRangeValue = Math.Round(value.HighRangeValue, len, MidpointRounding.AwayFromZero);
            value.LowRangeValue = Math.Round(value.LowRangeValue, len, MidpointRounding.AwayFromZero);

            if (value.HongKongRangeValue != null)
            {
                value.HongKongRangeValue.BuyHighRangeValue = Math.Round(value.HongKongRangeValue.BuyHighRangeValue, len);
                value.HongKongRangeValue.BuyLowRangeValue = Math.Round(value.HongKongRangeValue.BuyLowRangeValue, len);
                value.HongKongRangeValue.SellHighRangeValue = Math.Round(value.HongKongRangeValue.SellHighRangeValue, len);
                value.HongKongRangeValue.SellLowRangeValue = Math.Round(value.HongKongRangeValue.SellLowRangeValue, len);
            }
        }

        #region 现货涨跌幅处理

        /// <summary>
        /// 根据现货商品代码获取涨跌幅值
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <returns>涨跌幅值</returns>
        private HighLowRangeValue GetStockHighLowRangeValueByCommodityCode(string code, decimal orderPrice)
        {
            HighLowRange highLowRange = GetStockHighLowRangeByCommodityCode(code);

            if (highLowRange == null)
                return null;
            LogHelper.WriteDebug("Debug_Test-005:当前股票：" + code + "涨跌幅类型:" + highLowRange.RangeType.ToString());

            switch (highLowRange.RangeType)
            {
                case Types.HighLowRangeType.YesterdayCloseScale:
                    return GetType1(code, highLowRange);
                case Types.HighLowRangeType.RecentDealScale:
                    return GetType2(code, highLowRange);
                case Types.HighLowRangeType.Buy1Sell1Scale:
                    return GetType3(code, highLowRange);
                case Types.HighLowRangeType.RightPermitHighLow:
                    return GetType4(highLowRange);
                case Types.HighLowRangeType.HongKongPrice:
                    return GetType5(code, orderPrice, highLowRange);
                case Types.HighLowRangeType.RecentDealNumber:
                    return GetType6(code, highLowRange);
            }

            return null;
        }

        /// <summary>
        /// 对现货指定代码进行涨跌幅计算
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange GetStockHighLowRangeByCommodityCode(string code)
        {
            HighLowRange result = null;

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            if (breedClass == null)
            {
                LogHelper.WriteInfo("代码" + code + "无法找到对应的BreedClass.");
                return null;
            }

            XH_SpotHighLowControlType controlType =
                MCService.SpotTradeRules.GetSpotHighLowControlTypeByBreedClassID(breedClass.BreedClassID);

            if (controlType == null)
            {
                LogHelper.WriteInfo("代码" + code + "无法找到对应的XH_SpotHighLowControlType.");
                return null;
            }

            XH_SpotTradeRules rules = MCService.SpotTradeRules.GetSpotTradeRulesByBreedClassID(breedClass.BreedClassID);
            if (rules == null)
            {
                LogHelper.WriteInfo("代码" + code + "无法找到对应的XH_SpotTradeRules.");
                return null;
            }

            if (!rules.BreedClassHighLowID.HasValue)
            {
                LogHelper.WriteInfo("代码" + code + "其对应的XH_SpotTradeRules.BreedClassHighLowID无值.");
                return null;
            }

            IList<XH_SpotHighLowValue> values =
                MCService.SpotTradeRules.GetSpotHighLowValueByBreedClassHighLowID(rules.BreedClassHighLowID.Value);

            if (Utils.IsNullOrEmpty(values))
            {
                LogHelper.WriteInfo("代码" + code + "其对应的XH_SpotHighLowValue无值.");
                return null;
            }

            int highLowTypeID = controlType.HighLowTypeID;

            Types.XHSpotHighLowControlType controlTypeEnum;
            try
            {
                controlTypeEnum = (Types.XHSpotHighLowControlType)highLowTypeID;
            }
            catch (Exception ex)
            {
                //string errCode = "GT-8101";
                //string errMsg = "无法根据交易商品品种编码从管理中心获取涨跌幅。";
                //throw new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(ex.ToString(), ex);
                return null;
            }
            LogHelper.WriteDebug("Debug_Test-001:当前股票：" + code + "品种涨跌幅控制类型:" + controlTypeEnum.ToString());

            switch (controlTypeEnum)
            {
                /// 新股上市，增发上市，暂停后开始交易和其他日期(正常股票,ST股票)
                case Types.XHSpotHighLowControlType.NewThighAddFatStopAfterOrOtherDate:
                    result = GetHighLowValueType1(code, values);
                    break;
                /// 无涨跌幅限制
                case Types.XHSpotHighLowControlType.NotHighLowControl:
                    result = GetHighLowValueType2(code, values);
                    break;
                /// 权证涨跌幅
                case Types.XHSpotHighLowControlType.RightPermitHighLow:
                    result = GetHighLowValueType3(code, values);
                    break;
                /// 新基金上市，增发上市，暂停后开始交易和其他日期
                case Types.XHSpotHighLowControlType.NewFundAddFatStopAfterOrOtherDate:
                    result = GetHighLowValueType4(code, values);
                    break;
            }


            return result;
        }

        /// <summary>
        /// 处理无涨跌幅限制的商品
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange ProcessNoLimit(string code)
        {
            HighLowRange result = new HighLowRange();

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            if (breedClass == null)
                return null;

            XH_ValidDeclareType validDeclareType =
                MCService.SpotTradeRules.GetValidDeclareTypeByBreedClassID(breedClass.BreedClassID);
            IList<XH_ValidDeclareValue> validDeclareValues =
                MCService.SpotTradeRules.GetValidDeclareValueByBreedClassID(breedClass.BreedClassID);

            if (Utils.IsNullOrEmpty(validDeclareValues))
            {
                LogHelper.WriteInfo("代码" + code + "其对应的XH_ValidDeclareValue无值.");
                return null;
            }

            Types.XHValidDeclareType declareType;
            try
            {
                declareType = (Types.XHValidDeclareType)validDeclareType.ValidDeclareTypeID;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return null;
            }

            switch (declareType)
            {
                //1 最近成交价的上下百分比
                case Types.XHValidDeclareType.BargainPriceUpperDownScale:
                    result = ProcessNoLimitType1(code, validDeclareValues);
                    break;
                //2 不高于即时揭示的最低卖出价格的百分比且不低于即时揭示的最高买入价格的百分比
                case Types.XHValidDeclareType.NotHighSalePriceScaleAndNotLowBuyPriceScale:
                    result = ProcessNoLimitType2(code, validDeclareValues);
                    break;
                //3 低于买一价的个价位与卖一价之间或低于买一价与高于卖一价的个价位之间
                case Types.XHValidDeclareType.DownBuyOneAndSaleOne:
                    result = ProcessNoLimitType3(code, validDeclareValues);
                    break;
                //4 最近成交价上下各多少元
                case Types.XHValidDeclareType.BargainPriceOnDownMoney:
                    result = ProcessNoLimitType4(code, validDeclareValues);
                    break;
            }
            LogHelper.WriteDebug("Debug_Test-02:当前股票：" + code + "处理无涨跌幅限制的商品" + validDeclareType.ValidDeclareTypeID);
            return result;
        }

        /// <summary>
        /// （已经废弃，已经没有范围值此表）获取港股在指定基准价位上下浮动多少个价位的值
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="basePrice">基准价位</param>
        /// <param name="priceCount">价位上下浮动的数量,正往上，负向下</param>
        /// <returns>价位</returns>
        public decimal GetHKRangeValue(string code, decimal orderPrice, decimal basePrice, decimal priceCount)
        {

            decimal result = -1;
            //CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            //if (breedClass == null)
            //    return -1;

            //IList<XH_MinChangePriceValue> m_MinChangePriceValue =
            //    MCService.SpotTradeRules.GetMinChangePriceValueByBreedClassID(breedClass.BreedClassID);
            //if (Utils.IsNullOrEmpty(m_MinChangePriceValue))
            //    return -1;

            //foreach (XH_MinChangePriceValue changePriceValue in m_MinChangePriceValue)
            //{
            //    if (changePriceValue.Value.HasValue)
            //    {
            //        decimal fValue = changePriceValue.Value.Value;
            //        CM_FieldRange fieldRange = GetFieldRange(changePriceValue.FieldRangeID);

            //        //是否在当前字段范围内
            //        bool isFind = MCService.CheckFieldRange(orderPrice, fieldRange);
            //        if (isFind)
            //        {
            //            decimal min = changePriceValue.Value.Value;
            //            decimal change = min * priceCount;

            //            return basePrice + change;
            //        }
            //    }
            //}


            return result;
        }

        //private CM_FieldRange GetFieldRange(int fieldRangeID)
        //{
        //    return MCService.CommonPara.GetFieldRangeByFieldRangeID(fieldRangeID);
        //}

        #region NoLimit Process

        /// <summary>
        /// 1 最近成交价的上下百分比RecentDealScale
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values">涨跌幅实体对象列表</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange ProcessNoLimitType1(string code, IList<XH_ValidDeclareValue> values)
        {
            if (values.Count == 0)
                return null;

            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            result.HighRange = values[0].UpperLimit.Value;
            result.LowRange = values[0].LowerLimit.Value;
            result.RangeType = Types.HighLowRangeType.RecentDealScale;

            highLowRangeList.Add(result, code);

            return result;
        }

        /// <summary>
        /// 2 不高于即时揭示的最低卖出价格的百分比且不低于即时揭示的最高买入价格的百分比Buy1Sell1Scale
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values">涨跌幅实体对象列表</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange ProcessNoLimitType2(string code, IList<XH_ValidDeclareValue> values)
        {
            if (values.Count == 0)
                return null;

            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            result.HighRange = values[0].UpperLimit.Value;
            result.LowRange = values[0].LowerLimit.Value;
            result.RangeType = Types.HighLowRangeType.Buy1Sell1Scale;

            highLowRangeList.Add(result, code);

            return result;
        }

        /// <summary>
        /// 3 低于买一价的个价位与卖一价之间或低于买一价与高于卖一价的个价位之间
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values">涨跌幅实体对象列表</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange ProcessNoLimitType3(string code, IList<XH_ValidDeclareValue> values)
        {
            if (values.Count == 0)
                return null;

            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            HKRange hkRange = new HKRange();

            //买入时的价位差取表中的UpperLimit字段
            //high:卖一价                                    highrange=0     ref=卖一价
            hkRange.BuyHighRange = 0;

            //low:买一价 - 24个价位(UpperLimit)          lowrange = 24;  ref=买一价
            hkRange.BuyLowRange = values[0].UpperLimit.Value;


            //卖出时的价位差取表中的LowerLimit字段
            //high:卖一价 + 24个价位(LowerLimit)     highrange = 24  ref=卖一价
            hkRange.SellHighRange = values[1].LowerLimit.Value;

            //low:买一价                                lowrange = 0    ref=买一价
            hkRange.SellLowRange = 0;

            result.HongKongRange = hkRange;
            result.RangeType = Types.HighLowRangeType.HongKongPrice;

            highLowRangeList.Add(result, code);

            return result;
        }

        /// <summary>
        /// 4 最近成交价上下各多少元
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values">涨跌幅实体对象列表</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange ProcessNoLimitType4(string code, IList<XH_ValidDeclareValue> values)
        {
            if (values.Count == 0)
                return null;

            bool isNew = MCService.IsNewStock(code);
            if (!isNew)
                isNew = MCService.IsZF(code);

            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            decimal high = 0;
            decimal low = 0;

            if (isNew)
            {
                //上市首日
                //var q = from value in values
                //        //where value.IsMarketNewDay.Value == (int)Types.IsYesOrNo.Yes
                //        select value;

                //if(q.Count() == 0 )
                //    return null;

                //high = q.First().NewDayUpperLimit.Value;
                //low = q.First().NewDayLowerLimit.Value;
                high = values[0].NewDayUpperLimit.Value;
                low = values[0].NewDayLowerLimit.Value;
            }
            else
            {
                //非上市首日
                //var q = from value in values
                //        //where value.IsMarketNewDay.Value == (int)Types.IsYesOrNo.No
                //        select value;

                //if (q.Count() == 0)
                //    return null;

                //high = q.First().UpperLimit.Value;
                //low = q.First().LowerLimit.Value;

                high = values[0].UpperLimit.Value;
                low = values[0].LowerLimit.Value;
            }

            result.HighRange = high;
            result.LowRange = low;
            result.RangeType = Types.HighLowRangeType.RecentDealNumber;

            highLowRangeList.Add(result, code);

            return result;
        }

        #endregion

        #region ControlType Process

        /// <summary>
        /// 1.新股上市，增发上市，暂停后开始交易和其他日期(正常股票,ST股票)
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values">涨跌幅实体对象列表</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange GetHighLowValueType1(string code, IList<XH_SpotHighLowValue> values)
        {
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();//RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(code);

            LogHelper.WriteDebug("Debug_Test-002:当前股票：" + code + "新股上市，增发上市" + DateTime.Now);

            //新股上市，增发上市，暂停后开始交易
            bool isNew = MCService.IsNewStock(code);
            if (!isNew)
            {
                LogHelper.WriteDebug("Debug_Test-004:当前股票：" + code + "判断新股上市，增发上市，再判断是否为增发上市");

                isNew = MCService.IsZF(code);
            }

            if (isNew)
            {
                LogHelper.WriteDebug("Debug_Test-01:当前股票：" + code + "新股上市" + DateTime.Now);
                return ProcessNoLimit(code);
            }

            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }
            bool isST = false;
            if (data.Name.ToLower().Contains("st"))
            {
                isST = true;
            }
            else
            {

                isST = false;
            }


            decimal scale = 0;
            if (isST)
            {
                scale = values[0].StValue.Value;
            }
            else
            {
                scale = values[0].NormalValue.Value;
            }

            result.HighRange = result.LowRange = scale;
            result.RangeType = Types.HighLowRangeType.YesterdayCloseScale;

            highLowRangeList.Add(result, code);
            return result;
        }

        /// <summary>
        /// 2.无涨跌幅限制（不缓存）
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values"></param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange GetHighLowValueType2(string code, IList<XH_SpotHighLowValue> values)
        {
            return ProcessNoLimit(code);
        }

        /// <summary>
        /// 3.权证涨跌幅
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values">涨跌幅实体对象列表</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange GetHighLowValueType3(string code, IList<XH_SpotHighLowValue> values)
        {
            HighLowRange result = new HighLowRange();

            bool isNew = MCService.IsNewStock(code);
            if (!isNew)
                isNew = MCService.IsZF(code);

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(code);
            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqExData exData = data;
            if (exData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqData hqData = exData.HqData;
            if (hqData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }


            decimal yesterDayClosePrice = 0;
            if (isNew)
            {
                yesterDayClosePrice = (decimal)hqData.Open;
            }
            else
            {
                yesterDayClosePrice = (decimal)exData.YClose;
            }



            CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null)
            {
                return null;
            }

            //计算公式中的比例（125%）
            decimal scale = values[0].RightHighLowScale.Value / 100;

            //行权比例
            decimal goerScale = commodity.GoerScale;

            //标的代码
            string code2 = commodity.LabelCommodityCode;

            HqExData data2 = service.GetStockHqData(code2);
            HqExData exData2 = data2;

            //标的代码昨日收盘价
            decimal yesterDayClosePrice2 = (decimal)exData2.YClose;

            HighLowRange highLowRange = GetStockHighLowRangeByCommodityCode(code2);
            if (highLowRange.RangeType != Types.HighLowRangeType.YesterdayCloseScale)
                return null;

            //标的代码涨跌幅比例
            decimal rangeScale = highLowRange.HighRange / 100;

            decimal num = yesterDayClosePrice2 * rangeScale * scale * goerScale;


            //上限=权证前一日收盘价格+(标的证券前日收盘价×标的证券价格涨跌幅比例×150%)×行权比例(作废)
            //下限=权证前一日收盘价格-(标的证券前日收盘价×标的证券价格涨跌幅比例×150%)×行权比例(作废)


            //20090310修改
            //权证涨幅价格＝权证前一日收盘价格＋(标的证券当日涨幅价格-标的证券前一日收盘价)×125%×行权比例；
            decimal numHigh = (yesterDayClosePrice2 * (1 + rangeScale) - yesterDayClosePrice2) * scale * goerScale;
            decimal highPrice = yesterDayClosePrice + numHigh;

            //权证跌幅价格＝权证前一日收盘价格-(标的证券前一日收盘价-标的证券当日跌幅价格)×125%×行权比例。
            decimal numLow = (yesterDayClosePrice2 - yesterDayClosePrice2 * (1 - rangeScale)) * scale * goerScale;
            decimal lowPrice = yesterDayClosePrice - numLow;

            result.HighRange = highPrice;
            result.LowRange = lowPrice;
            result.RangeType = Types.HighLowRangeType.RightPermitHighLow;

            return result;
        }

        /// <summary>
        /// 4.新基金上市，增发上市，暂停后开始交易和其他日期
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="values">涨跌幅实体对象列表</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange GetHighLowValueType4(string code, IList<XH_SpotHighLowValue> values)
        {
            bool isNew = MCService.IsNewStock(code);
            if (!isNew)
                isNew = MCService.IsZF(code);

            if (isNew)
                return ProcessNoLimit(code);

            //基于昨天收盘价的，那么今天一天内都不会变化，直接放入缓存，下次获取时不再计算
            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            result.HighRange = result.LowRange = values[0].FundYestClosePriceScale.Value;
            result.RangeType = Types.HighLowRangeType.YesterdayCloseScale;

            highLowRangeList.Add(result, code);
            return result;
        }

        #endregion

        #region 获取实际的涨跌幅值

        /// <summary>
        /// 1.昨日收盘价的上下百分比
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="highLowRange">涨跌幅对象</param>
        /// <returns>涨跌幅值对象</returns>
        private HighLowRangeValue GetType1(string code, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            /*IRealtimeMarketService service = RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(code);
            HqExData exData = data;

            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            if (exData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }*/



            //float yClose = exData.YClose;
            //decimal yestPrice = (decimal) yClose;

            decimal yestPrice = RealTimeMarketUtil.GetInstance().GetStockYClose(code);
            if (yestPrice == -1)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            decimal high = yestPrice * (1 + highRange / 100);
            decimal low = yestPrice * (1 - lowRange / 100);

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.YesterdayCloseScale;

            return value;
        }

        /// <summary>
        /// 2.最近成交价的上下百分比
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="highLowRange">涨跌幅对象</param>
        /// <returns>涨跌幅值对象</returns>
        private HighLowRangeValue GetType2(string code, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(code);

            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqExData exData = data;

            if (exData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqData hqData = exData.HqData;
            if (hqData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            float rPrice = hqData.Lasttrade;
            if (rPrice <= 0)
            {
                string errCode = "GT-1802";
                string errMsg = "无法获取现货最近成交价，code=" + code;

                LogHelper.WriteInfo(errMsg + ",LastTrade=" + rPrice);
                throw new VTException(errCode, errMsg);
            }

            decimal recentPrice = (decimal)rPrice;
            LogHelper.WriteDebug("Debug_Test-03:当前股票：" + code + "最后成交价:" + recentPrice);
            decimal high = recentPrice * (1 + highRange / 100);
            decimal low = recentPrice * (1 - lowRange / 100);

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.RecentDealScale;

            return value;
        }

        /// <summary>
        /// 3.买一卖一百分比
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="highLowRange">涨跌幅对象</param>
        /// <returns>涨跌幅值对象</returns>
        private HighLowRangeValue GetType3(string code, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(code);

            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqExData exData = data;

            if (exData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqData hqData = exData.HqData;

            if (hqData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }


            float b1 = hqData.Buyprice1;
            decimal buy1 = (decimal)b1;

            float s1 = hqData.Sellprice1;
            decimal sell1 = (decimal)s1;

            decimal high = sell1 * highRange / 100;
            decimal low = buy1 * lowRange / 100;

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.Buy1Sell1Scale;

            return value;
        }

        /// <summary>
        /// 4.权证涨跌幅
        /// </summary>
        /// <param name="highLowRange">涨跌幅对象</param>
        /// <returns>涨跌幅值对象</returns>
        private HighLowRangeValue GetType4(HighLowRange highLowRange)
        {
            decimal high = highLowRange.HighRange;
            decimal low = highLowRange.LowRange;

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.RightPermitHighLow;

            return value;
        }

        /// <summary>
        /// 5.港股买卖价位
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="highLowRange">涨跌幅对象</param>
        /// <returns>涨跌幅值对象</returns>
        private HighLowRangeValue GetType5(string code, decimal orderPrice, HighLowRange highLowRange)
        {
            HKRange hkRange = highLowRange.HongKongRange;

            decimal buyHighRange = hkRange.BuyHighRange;
            decimal buyLowRange = hkRange.BuyLowRange;
            decimal sellHighRange = hkRange.SellHighRange;
            decimal sellLowRange = hkRange.SellLowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(code);

            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqExData exData = data;

            if (exData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqData hqData = exData.HqData;

            if (hqData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }


            float b1 = hqData.Buyprice1;
            decimal buy1 = (decimal)b1;

            float s1 = hqData.Sellprice1;
            decimal sell1 = (decimal)s1;

            decimal buyH = sell1;
            decimal buyL = GetHKRangeValue(code, orderPrice, buy1, -buyLowRange);

            decimal sellH = GetHKRangeValue(code, orderPrice, sell1, sellHighRange);
            decimal sellL = buy1;

            //decimal high = 0;
            //decimal low = 0;

            HighLowRangeValue value = new HighLowRangeValue();

            HKRangeValue hkRangeValue = new HKRangeValue();
            hkRangeValue.BuyHighRangeValue = buyH;
            hkRangeValue.BuyLowRangeValue = buyL;
            hkRangeValue.SellHighRangeValue = sellH;
            hkRangeValue.SellLowRangeValue = sellL;

            value.HongKongRangeValue = hkRangeValue;
            value.RangeType = Types.HighLowRangeType.HongKongPrice;

            return value;
        }

        /// <summary>
        /// 6.最近成交价上下各多少元
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="highLowRange">涨跌幅对象</param>
        /// <returns>涨跌幅值对象</returns>
        private HighLowRangeValue GetType6(string code, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(code);

            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqExData exData = data;

            if (exData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            HqData hqData = exData.HqData;
            if (hqData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取现货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            float rPrice = hqData.Lasttrade;
            if (rPrice <= 0)
            {
                string errCode = "GT-1802";
                string errMsg = "无法获取现货最近成交价，code=" + code;

                LogHelper.WriteInfo(errMsg + ",LastTrade=" + rPrice);
                throw new VTException(errCode, errMsg);
            }

            decimal recentPrice = (decimal)rPrice;

            decimal high = recentPrice + highRange;
            decimal low = recentPrice - lowRange;

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.RecentDealNumber;

            return value;
        }

        #endregion

        #endregion

        #region 期货涨跌幅处理

        /// <summary>
        /// 根据股指期货代码获取涨跌幅值
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>涨跌幅值</returns>
        private HighLowRangeValue GetFutureHighLowRangeValueByCommodityCode(string code, decimal orderPrice)
        {
            HighLowRange highLowRange = GetFutureHighLowRangeByCommodityCode(code);
            switch (highLowRange.RangeType)
            {
                case Types.HighLowRangeType.YesterdayBalanceScale:
                    return GetType7(code, highLowRange);
                case Types.HighLowRangeType.YesterdayBalanceNumber:
                    return GetType8(code, highLowRange);
            }

            return null;
        }

        /// <summary>
        /// 对股指期货指定代码进行涨跌幅计算
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange GetFutureHighLowRangeByCommodityCode(string code)
        {
            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            if (breedClass == null)
                return null;

            QH_FuturesTradeRules tradeRules = MCService.FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(breedClass.BreedClassID);

            if (tradeRules == null)
            {
                return null;
            }

            int? hlID = tradeRules.HighLowStopScopeID;
            if (!hlID.HasValue)
            {
                return null;
            }

            #region "季月合约上市首日涨跌幅","合约最后交易日涨跌幅"判断， add by 董鹏 2010-03-05
            if (MCService.IsLastTradingDayContract(code))
            {
                //合约最后交易日涨跌幅
                result.HighRange = result.LowRange = tradeRules.HighLowStopScopeValue.Value * tradeRules.NewMonthFuturesPactHighLowStopValue;
            }
            else if (MCService.IsNewQuarterMonthContract(code))
            {
                //季月合约上市首日涨跌幅
                result.HighRange = result.LowRange = tradeRules.HighLowStopScopeValue.Value * tradeRules.NewBreedFuturesPactHighLowStopValue;
            }
            else
            {
                //一般情况
                result.HighRange = result.LowRange = tradeRules.HighLowStopScopeValue.Value;
            }
            #endregion

            switch (hlID.Value)
            {
                //YesterdayBalanceScale
                case (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice:
                    result.RangeType = Types.HighLowRangeType.YesterdayBalanceScale;
                    break;
                //YesterdayBalanceNumber
                case (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice:
                    result.RangeType = Types.HighLowRangeType.YesterdayBalanceNumber;
                    break;
            }

            highLowRangeList.Add(result, code);

            return result;
        }

        /// <summary>
        /// 7.(股指期货)上一交易日结算价的上下百分比
        /// </summary>
        /// <param name="code"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private HighLowRangeValue GetType7(string code, HighLowRange range)
        {
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            FutData futData = service.GetFutData(code);

            if (futData == null)
            {
                string errCode = "GT-1801";
                string errMsg = "无法获取期货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }


            //decimal yesterdayBalance = (decimal) futData.PreSettlementPrice;

            decimal yesterdayBalance = 0;
            string msg = "";
            bool canGetPrice = MCService.GetFutureYesterdayPreSettlementPrice(code, out yesterdayBalance, ref msg);
            if (!canGetPrice)
            {
                LogHelper.WriteDebug(msg);
                //return false;
            }
            LogHelper.WriteDebug(code + "昨日结算价=" + yesterdayBalance);

            decimal highRange = range.HighRange;
            decimal lowRange = range.LowRange;

            decimal high = yesterdayBalance * (1 + highRange / 100);
            decimal low = yesterdayBalance * (1 - lowRange / 100);

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.YesterdayBalanceScale;

            return value;
        }

        /// <summary>
        /// (股指期货)上一交易日结算价的上下限的增量(多少钱)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private HighLowRangeValue GetType8(string code, HighLowRange range)
        {
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            FutData futData = service.GetFutData(code);

            if (futData == null)
            {
                string errCode = "GT-1801";
                string errMsg = "无法获取期货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            decimal yesterdayBalance = (decimal)futData.PreSettlementPrice;

            decimal highRange = range.HighRange;
            decimal lowRange = range.LowRange;

            decimal high = yesterdayBalance + highRange;
            decimal low = yesterdayBalance - lowRange;

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.YesterdayBalanceNumber;

            return value;
        }

        /// <summary>
        /// Desc: 根据商品期货代码获取涨跌幅值
        /// Create by: 董鹏
        /// Create date: 2010-01-26
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>涨跌幅值</returns>
        private HighLowRangeValue GetCommoditiesHighLowRangeValueByCommodityCode(string code, decimal orderPrice)
        {
            HighLowRange highLowRange = GetCommoditiesHighLowRangeByCommodityCode(code);
            switch (highLowRange.RangeType)
            {
                case Types.HighLowRangeType.YesterdayBalanceScale:
                    return GetCommoditiesHighLowRangeValueByScale(code, highLowRange);
                case Types.HighLowRangeType.YesterdayBalanceNumber:
                    return GetCommoditiesHighLowRangeValueByAmount(code, highLowRange);
            }
            return null;
        }

        /// <summary>
        /// Desc: 对商品期货指定代码进行涨跌幅计算
        /// Create by: 董鹏
        /// Create date: 2010-01-26
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>涨跌幅对象</returns>
        private HighLowRange GetCommoditiesHighLowRangeByCommodityCode(string code)
        {
            HighLowRange result = highLowRangeList.GetByKey(code);
            if (result != null)
                return result;
            result = new HighLowRange();

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            if (breedClass == null)
                return null;

            QH_FuturesTradeRules tradeRules = MCService.FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(breedClass.BreedClassID);

            if (tradeRules == null)
            {
                return null;
            }

            int? hlID = tradeRules.HighLowStopScopeID;
            if (!hlID.HasValue)
            {
                return null;
            }

            //1、新品种合约上市当日涨跌幅
            if (MCService.IsNewBreedClassByCode(code))
            {
                result.HighRange = result.LowRange = (tradeRules.NewBreedFuturesPactHighLowStopValue * tradeRules.HighLowStopScopeValue.Value);
            }
            //2、新月份合约上市当日涨跌幅
            else if (MCService.IsNewMonthBreedClassByCode(code))
            {
                result.HighRange = result.LowRange = (tradeRules.NewMonthFuturesPactHighLowStopValue * tradeRules.HighLowStopScopeValue.Value);
            }
            //3、交割月涨跌幅
            else if (MCService.IsDeliveryMonth(code) && tradeRules.DeliveryMonthHighLowStopValue.HasValue)
            {
                result.HighRange = result.LowRange = tradeRules.DeliveryMonthHighLowStopValue.Value;
            }
            //4、涨跌幅
            else
            {
                result.HighRange = result.LowRange = tradeRules.HighLowStopScopeValue.Value;
            }

            switch (hlID.Value)
            {
                //YesterdayBalanceScale
                case (int)Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice:
                    result.RangeType = Types.HighLowRangeType.YesterdayBalanceScale;
                    break;
                //YesterdayBalanceNumber
                case (int)Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice:
                    result.RangeType = Types.HighLowRangeType.YesterdayBalanceNumber;
                    break;
            }

            highLowRangeList.Add(result, code);

            return result;
        }

        /// <summary>
        /// Desc: (商品期货)按百分比计算涨跌幅值
        /// Create by: 董鹏
        /// Create date: 2010-01-26
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="range">涨跌幅值</param>
        /// <returns></returns>
        private HighLowRangeValue GetCommoditiesHighLowRangeValueByScale(string code, HighLowRange range)
        {
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();
            MerFutData futData = service.GetMercantileFutData(code);

            if (futData == null)
            {
                string errCode = "GT-1801";
                string errMsg = "无法获取期货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            decimal yesterdayBalance = 0;
            yesterdayBalance = (decimal)futData.PreClearPrice;

            decimal highRange = range.HighRange;
            decimal lowRange = range.LowRange;

            decimal high = yesterdayBalance * (1 + highRange / 100);
            decimal low = yesterdayBalance * (1 - lowRange / 100);

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.YesterdayBalanceScale;

            return value;
        }

        /// <summary>
        /// Desc: (商品期货)按增量计算涨跌幅值
        /// Create by: 董鹏
        /// Create date: 2010-01-26
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="range">涨跌幅值</param>
        /// <returns></returns>
        private HighLowRangeValue GetCommoditiesHighLowRangeValueByAmount(string code, HighLowRange range)
        {
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();
            MerFutData futData = service.GetMercantileFutData(code);

            if (futData == null)
            {
                string errCode = "GT-1801";
                string errMsg = "无法获取期货行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }

            decimal yesterdayBalance = (decimal)futData.PreClearPrice;

            decimal highRange = range.HighRange;
            decimal lowRange = range.LowRange;

            decimal high = yesterdayBalance + highRange;
            decimal low = yesterdayBalance - lowRange;

            HighLowRangeValue value = new HighLowRangeValue();
            value.HighRangeValue = high;
            value.LowRangeValue = low;
            value.RangeType = Types.HighLowRangeType.YesterdayBalanceNumber;

            return value;
        }

        #endregion

        #region 港股涨跌幅（有效申报）处理

        /// <summary>
        /// 5.港股买价位（上下限--涨跌幅）
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <param name="priceType">委托价格类型</param>
        /// <returns>涨跌幅值对象</returns>
        private HKRangeValue GetHKBuyHighLowRangeValue(string code, decimal orderPrice, Types.HKPriceType priceType)
        {

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();
            HKStock data = service.GetHKStockData(code);

            if (data == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取港股行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }
            HKStock hqData = data;
            if (hqData == null)
            {
                string errCode = "GT-1800";
                string errMsg = "无法获取港股行情，code=" + code;
                throw new VTException(errCode, errMsg);
            }


            //买方向上限
            decimal buyHigh = 0.000M;
            //买方向下限
            decimal buyLow = 0.000M;
            //卖方向上限
            decimal sellHigh = 0.000M;
            //卖方向上限
            decimal sellLow = 0.000M;

            decimal buy1 = (decimal)hqData.Buyprice1;//行情买一价
            decimal buyvol1 = (decimal)hqData.Buyvol1;//行情买一量

            decimal sell1 = (decimal)hqData.Sellprice1;//行情卖一价
            decimal sellvol1 = (decimal)hqData.Sellvol1;//行情买一量
            decimal yesterClosePrice = (decimal)hqData.PreClosePrice;//上日收市价
            decimal highTradePrice = (decimal)hqData.High;//最高成交价
            decimal lowTradePrice = (decimal)hqData.Low;//最低成交价
            decimal lastTradePrice = (decimal)hqData.Lasttrade;//最后成交价(最新成交价）
            decimal nominalPrice = (decimal)hqData.NominalPrice; // 0;//按盘价

            #region new Create by:董鹏 2009-12-11
            //上限
            decimal upperLimit = nominalPrice * 9;
            //下限
            decimal lowerLimit = (decimal)nominalPrice / 9;
            #endregion

            Types.HKValidPriceType validPriceType = new Types.HKValidPriceType();

            //得到最小变动价位 都是用最新成交价来查找 石千松已经确认2009-10-30
            decimal minPrice;
            HK_MinPriceFieldRange orderMinPrice = GetHKMinPrice(lastTradePrice);
            if (orderMinPrice == null)
            {
                string errCode = "GT-1252";
                string errMsg = "获取港股最小变动价位检验失败，code=" + code;
                throw new VTException(errCode, errMsg);
            }
            minPrice = orderMinPrice.Value.Value;

            //写日志字符串
            string txt = "买一价：" + buy1 + "，卖一价：" + sell1 + "，上日收市价：" + yesterClosePrice + "，最高成交价：" + highTradePrice + "，最低成交价：" + lowTradePrice + "";
            txt += "最后成交价：" + lastTradePrice + "，按盘价：" + nominalPrice + "，最小变动价位:" + minPrice + "";
            string txtHL = "";
            //string errorMsg = "";

            #region new Create by:董鹏 2009-12-11
            string lowerLog = "";
            string upperLog = "";
            #endregion

            switch (priceType)
            {
                //所有的不得偏离按盘价的九倍或以上，用公式来表达就是：【-按盘价*9，按盘价*9】
                //如果是下限，就取 -按盘价*9，上限就取 按盘价*9
                //  【下限，上限】

                case Types.HKPriceType.LO://限价盘
                    #region 有现存买盘及沽盘的情况
                    //有现存买盘及沽盘的情况
                    if (buy1 > 0 && sell1 > 0)
                    {
                        txtHL += "==【限价盘】有现存买盘及沽盘的情况======";
                        #region 买盘
                        //买盘
                        //价介乎低于当时买盘价二十四个价位及当时卖盘价之间的价格
                        //【最佳买盘价-24价位，最佳卖盘价】

                        #region old Comment by:董鹏 2009-12-11
                        //buyLow = buy1 - 24 * minPrice;//下限
                        #endregion

                        #region new Create by:董鹏 2009-12-11
                        buyLow = GetSegmentedLowerPrice(buy1, 24, ref lowerLog);
                        #endregion

                        buyHigh = sell1;//上限

                        //txtHL += "买盘【最佳买盘价-24价位，最佳卖盘价】==下限：{0}=" + buy1 + " -  24*" + minPrice + "  上限：{1}=" + sell1;

                        #endregion

                        #region 卖盘
                        //卖盘
                        //价介乎高于当时卖盘价二十四个价位及当时买盘价之间的价格
                        //【最佳买盘价，最佳卖盘价+24价位】
                        sellLow = buy1;//下限 

                        #region old Comment by:董鹏 2009-12-11
                        //sellHigh = sell1 + 24 * minPrice;//上限
                        #endregion

                        #region new Create by:董鹏 2009-12-11
                        sellHigh = GetSegmentedUpperPrice(sell1, 24, ref upperLog);
                        #endregion

                        //txtHL += "卖盘【最佳买盘价，最佳卖盘价+24价位】==下限：{2}=" + buy1 + "  上限：{3}=" + sell1 + " + 24*" + minPrice;
                        HighLowIntersectionCalculate(ref buyLow, ref buyHigh, ref sellLow, ref sellHigh, ref lowerLimit, ref upperLimit);
                        txtHL += "买盘【最佳买盘价-24价位，最佳卖盘价】==下限：{0}=" + lowerLog + "  上限：{1}=" + buyHigh;
                        txtHL += "卖盘【最佳买盘价，最佳卖盘价+24价位】==下限：{2}=" + sellLow + "  上限：{3}=" + upperLog;

                        #endregion
                        validPriceType = Types.HKValidPriceType.SellAndBuy;
                    }
                    #endregion

                    #region 没有现存买盘的情况
                    //没有现存买盘的情况
                    else if (buy1 <= 0 && sell1 > 0)
                    {
                        txtHL += "==【限价盘】没有现存买盘的情况======";

                        #region 买盘
                        //买盘
                        //最佳卖盘价是高的，所以是上限， 最低价-24价位是小的，所以是下限
                        //价介乎当时卖盘价，与及当时卖盘价、上日收市价及当日最低成交价三者中的最低价再低二十四个价位之间的价格
                        //【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价】

                        #region 比较在个价格最低
                        decimal k = ComparePrice(yesterClosePrice, sell1, lowTradePrice, false);
                        #endregion

                        #region old Comment by:董鹏 2009-12-11
                        //buyLow = k - minPrice * 24;//买下限
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        //买下限
                        buyLow = GetSegmentedLowerPrice(k, 24, ref lowerLog);
                        #endregion
                        buyHigh = sell1;//买上限

                        //txtHL += "买盘【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价】==下限：{0}=" + k + " -  24*" + minPrice + "  上限：{1}=" + sell1;
                        #endregion

                        #region 卖盘
                        //卖盘
                        //价低于或等于当时卖盘价加二十四个价位的价格
                        //（0，最佳卖盘价+24价位】
                        //（按盘价/9倍，最佳卖盘价+24价位】价格不可偏移按盘价9倍

                        sellLow = nominalPrice / 9;//卖下限 
                        #region old Comment by:董鹏 2009-12-11
                        //sellHigh = sell1 + 24 * minPrice;//卖上限
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        //卖上限
                        sellHigh = GetSegmentedUpperPrice(sell1, 24, ref upperLog);
                        #endregion
                        //txtHL += "卖盘【按盘价/9倍，最佳卖盘价+24价位】==下限：{2}=" + nominalPrice + "/ 9   上限：{3}=" + sell1 + " +  24 *" + minPrice;
                        HighLowIntersectionCalculate(ref buyLow, ref buyHigh, ref sellLow, ref sellHigh, ref lowerLimit, ref upperLimit);
                        txtHL += "买盘【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价】==下限：{0}=" + lowerLog + "  上限：{1}=" + buyHigh;
                        txtHL += "卖盘【按盘价/9倍，最佳卖盘价+24价位】==下限：{2}=" + nominalPrice + "/ 9   上限：{3}=" + upperLog;
                        #endregion
                        validPriceType = Types.HKValidPriceType.NoBuy;
                    }
                    #endregion

                    #region 没有现存卖盘的情况
                    //没有现存卖盘的情况
                    else if (buy1 > 0 && sell1 <= 0)
                    {
                        txtHL += "==【限价盘】没有现存卖盘的情况======";
                        #region 买盘
                        //买盘
                        //价高于或等于当时买盘价减二十四个价位的价格
                        //【最佳买盘价-24价位，9*按盘价】
                        #region old Comment by:董鹏 2009-12-11
                        //buyLow = buy1 - minPrice * 24;
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        buyLow = GetSegmentedLowerPrice(buy1, 24, ref lowerLog);
                        #endregion
                        buyHigh = nominalPrice * 9;//9*按盘价
                        //txtHL += "买盘【最佳买盘价-24价位，9*按盘价】==下限：{0}=" + buy1 + " -  24*" + minPrice + "  上限：{1}=" + nominalPrice + "*" + 9;

                        #endregion

                        #region 卖盘
                        //卖盘
                        //价介乎当时买盘价，与及当时买盘价、上日收市价及当日最高成交价三者中的最高价再高二十四个价位之间的价格
                        //【最佳买盘价，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】
                        sellLow = buy1;//下限 
                        #region 比较在个价格最高
                        decimal k = ComparePrice(yesterClosePrice, buy1, highTradePrice, true);
                        #endregion
                        #region old Comment by:董鹏 2009-12-11
                        //sellHigh = k + minPrice * 24;//上限
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        sellHigh = GetSegmentedUpperPrice(k, 24, ref upperLog);
                        #endregion
                        //txtHL += "卖盘【最佳买盘价，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】==下限：{2}=" + buy1 + "  上限：{3}=" + k + " + 24*" + minPrice;
                        HighLowIntersectionCalculate(ref buyLow, ref buyHigh, ref sellLow, ref sellHigh, ref lowerLimit, ref upperLimit);
                        txtHL += "买盘【最佳买盘价-24价位，9*按盘价】==下限：{0}=" + lowerLog + "  上限：{1}=" + nominalPrice + "*" + 9;
                        txtHL += "卖盘【最佳买盘价，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】==下限：{2}=" + sellLow + "  上限：{3}=" + upperLog;

                        #endregion
                        validPriceType = Types.HKValidPriceType.NoSell;
                    }
                    #endregion

                    #region 没有现存买卖盘的情况
                    //没有现存买卖盘的情况
                    else if (buy1 <= 0 && sell1 <= 0)
                    {
                        txtHL += "==【限价盘】没有现存买卖盘的情况======";
                        #region 买盘
                        //买盘
                        //输入价高于或等于最后卖盘价、上日收市价及当日最低成交价三者中的最低价再低二十四个价位的价格。
                        //若没有上日收市价及当日最低成交价，输入价可高于或等于或低于最后卖盘价(即不受最后卖盘价限制）
                        //【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】
                        if (yesterClosePrice <= 0 && lowTradePrice <= 0)
                        {
                            buyLow = nominalPrice / 9;
                            txtHL += "买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==下限：{0}=" + nominalPrice + "/" + 9;
                        }
                        else
                        {
                            #region 比较在个价格最低
                            decimal k = ComparePrice(yesterClosePrice, lastTradePrice, lowTradePrice, false);
                            #endregion
                            #region old Comment by:董鹏 2009-12-11
                            //buyLow = k - minPrice * 24;
                            #endregion
                            #region new Create by:董鹏 2009-12-11
                            buyLow = GetSegmentedLowerPrice(k, 24, ref lowerLog);
                            #endregion
                            //txtHL += "买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==下限：{0}=" + k + " -  24 *" + minPrice;
                            txtHL += "买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==下限：{0}=" + lowerLog;
                        }
                        buyHigh = nominalPrice * 9;
                        txtHL += "  上限：{1}=" + nominalPrice * 9;

                        #endregion

                        #region 卖盘
                        //卖盘
                        //输入价低于或等于最后买盘价、上日收市价及当日最高成交价三者中最高价再高二十四个价位的价格。
                        //若没有上日收市价及当日最高成交价，输入价可低于或等于或高于最后买盘价
                        //（0，max(最后买盘价，上日收市价，当日最高成交价)+24价位】
                        sellLow = nominalPrice / 9;
                        txtHL += "卖盘（按盘价/9，max(最后买盘价，上日收市价，当日最高成交价)+24价位】==下限：{2}=" + nominalPrice / 9;

                        if (yesterClosePrice <= 0 && highTradePrice <= 0)
                        {
                            sellHigh = nominalPrice * 9;
                            txtHL += "  上限：{2}=" + nominalPrice + "*" + 9;

                        }
                        else
                        {
                            #region 比较在个价格最高
                            decimal t = ComparePrice(yesterClosePrice, lastTradePrice, highTradePrice, true);
                            #endregion
                            #region old Comment by:董鹏 2009-12-11
                            //sellHigh = t + minPrice * 24;
                            #endregion
                            #region new Create by:董鹏 2009-12-11
                            sellHigh = GetSegmentedUpperPrice(t, 24, ref upperLog);
                            #endregion
                            txtHL += "  上限：{3}=" + upperLog;
                        }

                        #endregion
                        validPriceType = Types.HKValidPriceType.NoSellAndBuy;
                    }
                    #endregion
                    break;
                case Types.HKPriceType.ELO:


                    #region 有现存买盘及沽盘的情况
                    //有现存买盘及沽盘的情况
                    if (buy1 > 0 && sell1 > 0)
                    {
                        txtHL += "==【增强限价盘】有现存买盘及沽盘的情况======";
                        #region 买盘
                        //买盘
                        //输入价介乎低于当时买盘价二十四个价位与及高于当时卖盘价四个价位
                        // 【最佳买盘价-24价位，最佳卖盘价+4价位】
                        #region old Comment by:董鹏 2009-12-11
                        //buyLow = buy1 - 24 * minPrice;
                        //buyHigh = sell1 + 4 * minPrice;
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        buyLow = GetSegmentedLowerPrice(buy1, 24, ref lowerLog);
                        buyHigh = GetSegmentedUpperPrice(sell1, 4, ref upperLog);
                        #endregion
                        //txtHL += "买盘【最佳买盘价-24价位，最佳卖盘价+4价位】==下限：{0}=" + buy1 + " -  24 *" + minPrice + "  上限：{1}=" + sell1 + "+  4*" + minPrice;
                        txtHL += "买盘【最佳买盘价-24价位，最佳卖盘价+4价位】==下限：{0}=" + lowerLog + "  上限：{1}=" + upperLog;

                        #endregion

                        #region 卖盘
                        //卖盘
                        //输入价介乎高于当时卖盘价二十四个价位与及低于当时买盘价四个价位
                        //【最佳买盘价-4价位，最佳卖盘价+24价位】
                        #region old Comment by:董鹏 2009-12-11
                        //sellLow = buy1 - minPrice * 4;
                        //sellHigh = sell1 + 24 * minPrice;
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        sellLow = GetSegmentedLowerPrice(buy1, 4, ref lowerLog);
                        sellHigh = GetSegmentedUpperPrice(sell1, 24, ref upperLog);
                        #endregion
                        //txtHL += "卖盘【最佳买盘价-4价位，最佳卖盘价+24价位】==下限：{2}=" + buy1 + " -  4 *" + minPrice + "  上限：{3}=" + sell1 + " +  24 *" + minPrice;
                        txtHL += "卖盘【最佳买盘价-4价位，最佳卖盘价+24价位】==下限：{2}=" + lowerLog + "  上限：{3}=" + upperLog;

                        #endregion
                        validPriceType = Types.HKValidPriceType.SellAndBuy;
                    }
                    #endregion

                    #region 没有现存买盘的情况
                    //没有现存买盘的情况
                    else if (buy1 <= 0 && sell1 > 0)
                    {
                        txtHL += "==【增强限价盘】没有现存买盘的情况======";
                        #region 买盘
                        //买盘
                        //输入价介乎高于当时卖盘价四个价位，与及当时卖盘价、上日收市价及当日最低成交价三者中的最低价再低二十四个价位之间的价格
                        //【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价+4价位】

                        #region 比较在个价格最低
                        decimal k = ComparePrice(yesterClosePrice, sell1, lowTradePrice, false);
                        #endregion
                        #region old Comment by:董鹏 2009-12-11
                        //buyLow = k - minPrice * 24;
                        //buyHigh = sell1 + minPrice * 4;
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        buyLow = GetSegmentedLowerPrice(k, 24, ref lowerLog);
                        buyHigh = GetSegmentedUpperPrice(sell1, 4, ref upperLog);
                        #endregion
                        //txtHL += "买盘【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价+4价位】==下限：{0}=" + k + " -  24 *" + minPrice + "  上限：{2}=" + sell1 + " +  4 *" + minPrice;
                        txtHL += "买盘【min(最佳卖盘价，上日收市价，当日最低成交价)-24价位，最佳卖盘价+4价位】==下限：{0}=" + lowerLog + "  上限：{2}=" + upperLog;

                        #endregion

                        #region 卖盘
                        //卖盘
                        //输入价低于或等于当时卖盘价加二十四个价位的价格
                        //（按盘价/9倍，最佳卖盘价+24价位】
                        sellLow = nominalPrice / 9;
                        #region old Comment by:董鹏 2009-12-11
                        //sellHigh = sell1 + 24 * minPrice;
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        sellHigh = GetSegmentedUpperPrice(sell1, 24, ref upperLog);
                        #endregion
                        //txtHL += "卖盘【按盘价/9倍，最佳卖盘价+24价位】==下限：{2}=" + nominalPrice + "/" + 9 + "  上限：{3}=" + sell1 + " +  24 *" + minPrice;
                        txtHL += "卖盘【按盘价/9倍，最佳卖盘价+24价位】==下限：{2}=" + nominalPrice + "/" + 9 + "  上限：{3}=" + upperLog;

                        #endregion
                        validPriceType = Types.HKValidPriceType.NoBuy;
                    }
                    #endregion

                    #region 没有现存卖盘的情况
                    //没有现存卖盘的情况
                    else if (buy1 > 0 && sell1 <= 0)
                    {
                        txtHL += "==【增强限价盘】没有现存卖盘的情况======";
                        #region 买盘
                        //买盘
                        //输入价高于或等于当时买盘价减二十四个价位的价格
                        //【最佳买盘价-24价位，9*按盘价】
                        #region old Comment by:董鹏 2009-12-11
                        //buyLow = buy1 - minPrice * 24;//下限
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        buyLow = GetSegmentedLowerPrice(buy1, 24, ref lowerLog);
                        #endregion
                        buyHigh = nominalPrice * 9;//上限
                        //txtHL += "买盘【最佳买盘价-24价位，9*按盘价】==下限：{0}=" + buy1 + " -  4*" + minPrice + "  上限：{1}=" + nominalPrice + "*" + 9;
                        txtHL += "买盘【最佳买盘价-24价位，9*按盘价】==下限：{0}=" + lowerLog + "  上限：{1}=" + nominalPrice + "*" + 9;

                        #endregion

                        #region 卖盘
                        //卖盘
                        //输入价介乎低于当时买盘价四个价位，与及当时买盘价、上日收市价及当日最高成交价三者中的最高价再高二十四个价位之间的价格
                        //【最佳买盘价-4价位，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】
                        //下限是 最佳买盘价 - 四个价位
                        //上限是 XXX最高价 + 24价位

                        #region 比较在个价格最高
                        decimal k = ComparePrice(yesterClosePrice, buy1, highTradePrice, true);
                        #endregion

                        #region old Comment by:董鹏 2009-12-11
                        //sellLow = buy1 - minPrice * 4;//卖下限
                        //sellHigh = k + minPrice * 24; //卖上限
                        #endregion
                        #region new Create by:董鹏 2009-12-11
                        sellLow = GetSegmentedLowerPrice(buy1, 4, ref lowerLog);
                        sellHigh = GetSegmentedUpperPrice(k, 24, ref upperLog);
                        #endregion
                        //txtHL += "卖盘【最佳买盘价-4价位，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】==下限：{2}=" + buy1 + " -  4*" + minPrice + "  上限：{3}=" + k + " +  24*" + minPrice;
                        txtHL += "卖盘【最佳买盘价-4价位，max(最佳买盘价，上日收市价，当日最高成交价)+24价位】==下限：{2}=" + lowerLog + "  上限：{3}=" + upperLog;

                        #endregion
                        validPriceType = Types.HKValidPriceType.NoSell;
                    }
                    #endregion

                    #region 没有现存买卖盘的情况
                    //没有现存买卖盘的情况(示）
                    else if (buy1 <= 0 && sell1 <= 0)
                    {
                        txtHL += "==【增强限价盘】没有现存买卖盘的情况======";
                        #region 买盘
                        //买盘
                        //输入价高于或等于最后沽盘价、上日收市价及当日最低成交价三者中的最低价再低二十四个价位的价格。
                        //若没有上日收市价及当日最低成交价，输入价可高于或等于或低于最后沽盘价(即不受最后卖盘价限制）
                        //【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】

                        if (yesterClosePrice <= 0 && lowTradePrice <= 0)
                        {
                            buyLow = nominalPrice / 9;
                            txtHL += "买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==下限：{0}=" + nominalPrice + "/" + 9;

                        }
                        else
                        {
                            #region 比较在个价格最低
                            decimal k = ComparePrice(yesterClosePrice, lastTradePrice, lowTradePrice, false);
                            #endregion
                            #region old Comment by:董鹏 2009-12-11
                            //buyLow = k - minPrice * 24;
                            #endregion
                            #region new Create by:董鹏 2009-12-11
                            buyLow = GetSegmentedLowerPrice(k, 24, ref lowerLog);
                            #endregion
                            //txtHL += "买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==下限：{0}=" + k + " -  24*" + minPrice;
                            txtHL += "买盘【min(最后沽盘价，上日收市价，当日最低成交价)-24价位，9*按盘价】==下限：{0}=" + lowerLog;

                        }
                        buyHigh = nominalPrice * 9;

                        txtHL += "  上限：{1}=" + nominalPrice + "*" + 9;
                        #endregion

                        #region 卖盘
                        //卖盘
                        //输入价低于或等于最后买盘价、上日收市价及当日最高成交价三者中最高价再高二十四个价位的价格。
                        // 若没有上日收市价及当日最高成交价，输入价可低于或等于或高于最后买盘价
                        //（按盘价/9，max(最后买盘价，上日收市价，当日最高成交价)+24价位】
                        sellLow = nominalPrice / 9; ;
                        txtHL += "卖盘（按盘价/9，max(最后买盘价，上日收市价，当日最高成交价)+24价位】==下限：{2}=" + nominalPrice + "/" + 9;
                        if (yesterClosePrice <= 0 && highTradePrice <= 0)
                        {
                            sellHigh = nominalPrice * 9;
                            txtHL += "  上限：{3}=" + nominalPrice + "*" + 9;
                        }
                        else
                        {
                            #region 比较在个价格最高
                            decimal t = ComparePrice(yesterClosePrice, lastTradePrice, highTradePrice, true);
                            #endregion
                            #region old Comment by:董鹏 2009-12-11
                            //sellHigh = t + minPrice * 24;
                            #endregion
                            #region new Create by:董鹏 2009-12-11
                            sellHigh = GetSegmentedUpperPrice(t, 24, ref upperLog);
                            #endregion
                            //txtHL += "  上限：{3}=" + t + " +  24*" + minPrice;
                            txtHL += "  上限：{3}=" + upperLog;
                        }
                        #endregion
                        validPriceType = Types.HKValidPriceType.NoSellAndBuy;
                    }
                    #endregion
                    break;
                case Types.HKPriceType.SLO:


                    #region 有现存买盘及沽盘的情况
                    //有现存买盘及沽盘的情况
                    if (buy1 > 0 && sell1 > 0)
                    {
                        txtHL += "==【特别限价盘】有现存买盘及沽盘的情况======";
                        #region 买盘
                        //买盘
                        // 【最佳卖盘价，9*按盘价】
                        buyLow = sell1;
                        buyHigh = nominalPrice * 9;
                        txtHL += "买盘【最佳卖盘价，9*按盘价】==下限：{0}=" + sell1 + "  上限：{1}=" + nominalPrice + "*" + 9; ;

                        #endregion

                        #region 卖盘
                        //卖盘
                        //(9/按盘价，最佳买盘价】
                        sellLow = nominalPrice / 9;
                        sellHigh = buy1;
                        txtHL += "买盘【9/按盘价，9*按盘价】==下限：{2}=" + nominalPrice + "/" + 9 + "  上限：{3}=" + buy1;

                        #endregion
                        validPriceType = Types.HKValidPriceType.SellAndBuy;
                    }
                    #endregion

                    #region 没有现存买盘的情况
                    //没有现存买盘的情况
                    else if (buy1 <= 0 && sell1 > 0)
                    {
                        txtHL += "==【特别限价盘】没有现存买盘的情况======";
                        #region 买盘
                        //买盘
                        //【最佳卖盘价，9*按盘价】
                        buyLow = sell1; ;
                        buyHigh = nominalPrice * 9;
                        txtHL += "买盘【最佳卖盘价，9*按盘价】==下限：{0}=" + sell1 + "  上限：{1}=" + nominalPrice + "*" + 9; ;

                        #endregion

                        #region 卖盘(无)
                        ////卖盘
                        ////按盘价上下9倍
                        sellLow = nominalPrice / 9;//下限
                        sellHigh = nominalPrice * 9;//上限
                        txtHL += "卖盘【9/按盘价，9*按盘价】==下限：{2}=" + nominalPrice + "/" + 9 + "  上限：{3}=" + nominalPrice + "*" + 9; ;
                        #endregion
                        validPriceType = Types.HKValidPriceType.NoBuy;
                    }
                    #endregion

                    #region 没有现存卖盘的情况
                    //没有现存卖盘的情况
                    else if (buy1 > 0 && sell1 <= 0)
                    {
                        txtHL += "==【特别限价盘】没有现存卖盘的情况======";
                        #region 买盘
                        //买盘
                        ////按盘价上下9倍
                        buyLow = nominalPrice / 9;//下限
                        buyHigh = nominalPrice * 9;//上限

                        txtHL += "买盘【9/按盘价，9*按盘价】==下限：{0}=" + nominalPrice + "/" + 9 + "  上限：{1}=" + nominalPrice + "*" + 9;
                        #endregion

                        #region 卖盘
                        //卖盘
                        //(9/按盘价，最佳买盘价】
                        sellLow = nominalPrice / 9;//卖下限 9/按盘价
                        sellHigh = buy1; //卖上限
                        txtHL += "卖盘【9/按盘价，9*按盘价】==下限：{2}=" + nominalPrice + "/" + 9 + "  上限：{3}=" + buy1;

                        #endregion
                        validPriceType = Types.HKValidPriceType.NoSell;
                    }
                    #endregion

                    #region 没有现存买卖盘的情况
                    ////没有现存买卖盘的情况
                    else if (buy1 <= 0 && sell1 <= 0)
                    {
                        txtHL += "==【特别限价盘】没有现存买卖盘的情况======";
                        validPriceType = Types.HKValidPriceType.NoSellAndBuy;
                        #region 买盘（无）
                        //买盘
                        ////按盘价上下9倍
                        buyLow = nominalPrice / 9;//下限
                        buyHigh = nominalPrice * 9;//上限
                        txtHL += "买盘【9/按盘价，9*按盘价】==下限：{0}=" + nominalPrice / 9 + "  上限：{1}=" + nominalPrice * 9; ;

                        #endregion

                        #region 卖盘
                        //卖盘
                        sellLow = nominalPrice / 9;//下限
                        sellHigh = nominalPrice * 9;//上限
                        txtHL += "卖盘【9/按盘价，9*按盘价】==下限：{2}=" + nominalPrice + "/" + 9 + "  上限：{3}=" + nominalPrice + "*" + 9; ;
                        #endregion

                    }
                    #endregion
                    break;
                default:
                    break;
            }

            //new Create by : 董鹏 2009-12-11
            //计算价格范围交集
            HighLowIntersectionCalculate(ref buyLow, ref buyHigh, ref sellLow, ref sellHigh, ref lowerLimit, ref upperLimit);

            HKRangeValue hkRangeValue = new HKRangeValue();
            hkRangeValue.BuyHighRangeValue = buyHigh;
            hkRangeValue.BuyLowRangeValue = buyLow;
            hkRangeValue.SellHighRangeValue = sellHigh;
            hkRangeValue.SellLowRangeValue = sellLow;
            hkRangeValue.HKValidPriceType = validPriceType;

            LogHelper.WriteDebug(DateTime.Now.ToString() + txt + string.Format(txtHL, buyLow, buyHigh, sellLow, sellHigh));

            return hkRangeValue;
        }

        /// <summary>
        /// 港股根据价格获取港股最小变动价位实体
        /// </summary>
        /// <param name="orderPrice"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        private HK_MinPriceFieldRange GetHKMinPrice(decimal price)
        {
            IList<HK_MinPriceFieldRange> m_MinPriceFiledRange = MCService.HKTradeRulesProxy.GetAllHKMinPriceFieldRange();

            foreach (HK_MinPriceFieldRange item in m_MinPriceFiledRange)
            {
                //价位（靠后不靠前）（港元）
                if (item.LowerLimit.Value < price && price <= item.UpperLimit.Value)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 比较三个数据大小返回最大或者最小(isLargest)
        /// </summary>
        /// <param name="yesterClosePrice"></param>
        /// <param name="price"></param>
        /// <param name="tradePrice"></param>
        /// <param name="isLargest">返回最大或者最小(true返回大，false返回小)</param>
        /// <returns></returns>
        private decimal ComparePrice(decimal yesterClosePrice, decimal price, decimal tradePrice, bool isLargest)
        {
            decimal k;
            if (isLargest)
            {
                if (price > yesterClosePrice)
                {
                    k = price;
                }
                else
                {
                    k = yesterClosePrice;
                }
                if (k < tradePrice)
                {
                    k = tradePrice;
                }
            }
            else
            {
                if (price < yesterClosePrice)
                {
                    k = price;
                }
                else
                {
                    k = yesterClosePrice;
                }
                if (k > tradePrice)
                {
                    k = tradePrice;
                }
            }
            return k;
        }

        #region 获取分段计算的价格上、下限



        /// <summary>
        /// 计算价格上下限的交集
        /// </summary>
        /// <param name="buyLow">买方向下限</param>
        /// <param name="buyHigh">买方向上限</param>
        /// <param name="sellLow">卖方向上限</param>
        /// <param name="sellHigh">卖方向上限</param>
        /// <param name="lowerLimit">9倍按盘价下限</param>
        /// <param name="upperLimit">9倍按盘价上限</param>
        public void HighLowIntersectionCalculate(ref decimal buyLow, ref decimal buyHigh, ref decimal sellLow, ref decimal sellHigh, ref decimal lowerLimit, ref decimal upperLimit)
        {
            buyLow = (buyLow > lowerLimit) ? buyLow : lowerLimit;
            buyHigh = (buyHigh < upperLimit) ? buyHigh : upperLimit;
            sellLow = (sellLow > lowerLimit) ? sellLow : lowerLimit;
            sellHigh = (sellHigh < upperLimit) ? sellHigh : upperLimit;
        }

        /// <summary>
        /// 获取分段计算的价格上限
        /// </summary>
        /// <param name="price">最佳价格</param>
        /// <param name="level">价位数</param>
        /// <param name="log">计算公式的日志文本</param>
        /// <returns>分段计算的价格上限</returns>
        /// 
        public decimal GetSegmentedUpperPrice(decimal price, decimal level, ref string log)
        {
            //当前价格区间
            HK_MinPriceFieldRange range = GetHKMinPrice(price);
            if (range == null)
            {
                log = "根据价格：" + price + "获取最小变动价位为null";
                return 0;
            }
            //下一个价格区间
            HK_MinPriceFieldRange rangeNext = GetHKMinPrice(range.UpperLimit.Value + range.Value.Value);
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
        /// Update By:李健华
        /// UPdate Date:2010-05-26
        /// Desc.:修改返回价格区间为空时作相应的处理
        /// </summary>
        /// <param name="price">最佳价格</param>
        /// <param name="level">价位数</param>
        /// <param name="log">计算公式的日志文本</param>
        /// <returns>分段计算的价格下限</returns>
        public decimal GetSegmentedLowerPrice(decimal price, decimal level, ref string log)
        {
            //当前价格区间
            HK_MinPriceFieldRange range = GetHKMinPrice(price);
            if (range == null)
            {
                log = "根据价格：" + price + "获取最小变动价位为null";
                return 0;
            }
            //上一个价格区间
            HK_MinPriceFieldRange rangeLast = GetHKMinPrice(range.LowerLimit.Value - range.Value.Value);

            if (rangeLast == null)
            {
                log = "根据价格：" + range.LowerLimit.Value + "-" + range.Value.Value + "获取最小变动价位为null";
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



        #region 功能函数

        /// <summary>
        /// 获取现货最小变动价位
        /// </summary>
        /// <param name="code">现货代码</param>
        /// <param name="orderPrice">委托价格</param>
        /// <returns>最小变动价位</returns>
        public decimal GetMinChangePrice(string code, decimal orderPrice)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            if (breedClass == null)
                return 0;

            XH_SpotHighLowControlType controlType =
                MCService.SpotTradeRules.GetSpotHighLowControlTypeByBreedClassID(breedClass.BreedClassID);

            if (controlType == null)
                return 0;

            XH_SpotTradeRules rules = MCService.SpotTradeRules.GetSpotTradeRulesByBreedClassID(breedClass.BreedClassID);
            if (rules == null)
                return 0;

            int? min = rules.ValueTypeMinChangePrice;
            if (!min.HasValue)
            {
                return 0;
            }

            int minValueType = min.Value;
            //decimal orderPrice = 0;
            switch (minValueType)
            {
                case (int)Types.GetValueTypeEnum.Single:
                    decimal? minSingle = rules.MinChangePrice;
                    if (minSingle.HasValue)
                    {
                        decimal minSingleValue = minSingle.Value;
                        return minSingleValue;
                        ////是否满足最小变动价位
                        //if (orderPrice % minSingleValue == 0)
                        //{
                        //    result = true;
                        //    strMessage = "";
                        //}
                    }

                    break;
                case (int)Types.GetValueTypeEnum.Scope:

                    //if (orderPrice == 0)
                    //    return 0;

                    //var minChangePriceValue = MCService.SpotTradeRules.GetMinChangePriceValueByBreedClassID(breedClass.BreedClassID);
                    //foreach (XH_MinChangePriceValue changePriceValue in minChangePriceValue)
                    //{
                    //    if (changePriceValue.Value.HasValue)
                    //    {
                    //        decimal fValue = changePriceValue.Value.Value;
                    //        CM_FieldRange fieldRange = MCService.GetFieldRange(changePriceValue.FieldRangeID);

                    //        //是否在当前字段范围内
                    //        bool isIn = MCService.CheckFieldRange(orderPrice, fieldRange);
                    //        if (isIn)
                    //        {
                    //            //是否满足最小变动价位
                    //            //if (orderPrice % fValue == 0)
                    //            //{
                    //            //    strMessage = "";

                    //            //    return true;
                    //            //}
                    //            return fValue;
                    //        }
                    //    }
                    //}
                    break;
            }


            return 0;
        }

        #endregion
    }
}
