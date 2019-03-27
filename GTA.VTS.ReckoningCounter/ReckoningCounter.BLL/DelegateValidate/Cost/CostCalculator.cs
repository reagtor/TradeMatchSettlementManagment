#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.Entity;
using ReckoningCounter.DAL.HKTradingRulesService;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.Cost
{
    /// <summary>
    /// 交易费用计算器，错误码范围1700-1799
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public static class CostCalculator
    {
        #region 现货费用计算

        /// <summary>
        /// 获取现货交易费用
        /// </summary>
        /// <param name="request">现货委托</param>
        /// <returns>现货交易费用结果</returns>
        public static XHCostResult ComputeXHCost(StockOrderRequest request)
        {
            int? bc = MCService.CommonPara.GetBreedClassIdByCommodityCode(request.Code);

            XHCostResult result = null;

            if (!bc.HasValue)
            {
                return null;
            }

            int breedClassID = bc.Value;

            XH_SpotCosts cost = MCService.SpotTradeRules.GetSpotCostsByBreedClassID(breedClassID);

            if (cost == null)
                return null;

            try
            {
                result = InternalComputeXHCost(request, cost);
            }
            catch (Exception ex)
            {
                string errCode = "GT-1701";
                string errMsg = "无法获取现货交易费用。";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception);
                throw exception;
            }

            return result;
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
            StockOrderRequest request = new StockOrderRequest();
            request.Code = code;
            request.OrderPrice = price;
            request.OrderAmount = amount;
            request.OrderUnitType = unitType;
            request.BuySell = buySell;

            return ComputeXHCost(request);
        }

        private static XHCostResult InternalComputeXHCost(StockOrderRequest request, XH_SpotCosts cost)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code);


            XHCostResult result = new XHCostResult();

            result.Code = request.Code;

            decimal orderPrice = (decimal)request.OrderPrice;
            //期货合约乘数300
            decimal scale = MCService.GetTradeUnitScale(request.Code, request.OrderUnitType);
            //以计价单位计算的委托量
            var orderAmount = (decimal)request.OrderAmount * scale;

            //成交额
            var dealAmount = orderPrice * orderAmount;

            /// 1.印花税
            decimal stamp = cost.StampDuty / 100;

            decimal stampStart = cost.StampDutyStartingpoint;

            if (cost.StampDutyTypeID.HasValue)
            {
                int stampType = cost.StampDutyTypeID.Value;

                #region old code
                //if (stampType == (int)Types.GetValueTypeEnum.SingleSell &&
                //    request.BuySell == Types.TransactionDirection.Selling)
                //{
                //    decimal stampVal = stamp * dealAmount;
                //    if (stampVal < stampStart)
                //        stampVal = stampStart;

                //    stampVal = Utils.Round(stampVal);

                //    result.StampDuty = stampVal;
                //}
                #endregion
                decimal stampVal = stamp * dealAmount;
                if (stampVal < stampStart)
                {
                    stampVal = stampStart;
                }
                stampVal = Utils.Round(stampVal);

                switch ((Types.GetValueTypeEnum)stampType)
                {
                    case Types.GetValueTypeEnum.Single:
                        break;
                    case Types.GetValueTypeEnum.Scope:
                        break;
                    case Types.GetValueTypeEnum.Turnover:
                        break;
                    case Types.GetValueTypeEnum.Thigh:
                        break;
                    case Types.GetValueTypeEnum.SingleBuy:
                        if (request.BuySell == Types.TransactionDirection.Buying)
                        {
                            result.StampDuty = stampVal;
                        }
                        break;
                    case Types.GetValueTypeEnum.SingleSell:
                        if (request.BuySell == Types.TransactionDirection.Selling)
                        {
                            result.StampDuty = stampVal;
                        }
                        break;
                    case Types.GetValueTypeEnum.TwoEdge:
                        result.StampDuty = stampVal;
                        break;
                    case Types.GetValueTypeEnum.No:
                        break;
                    default:
                        break;
                }

            }


            /// 2.佣金
            //public decimal Commision { get; set; }
            if (cost.Commision.HasValue)
            {
                decimal comm = cost.Commision.Value / 100;

                decimal commVal = comm * dealAmount;

                decimal commStart = cost.CommisionStartingpoint;

                if (commVal < commStart)
                    commVal = commStart;

                commVal = Utils.Round(commVal);

                result.Commision = commVal;
            }


            /// 3.过户费
            //public decimal TransferToll { get; set; }
            decimal trans = cost.TransferToll / 100;

            int transType = cost.TransferTollTypeID;

            decimal transVal = 0;

            //过户费类型 [按股] [按成交额]
            switch (transType)
            {
                case (int)Types.GetValueTypeEnum.Thigh:
                    transVal = trans * orderAmount;
                    break;
                case (int)Types.GetValueTypeEnum.Turnover:
                    transVal = trans * dealAmount;
                    break;
            }
            transVal = Utils.Round(transVal);

            if (cost.TransferTollStartingpoint.HasValue)
            {
                decimal transStart = cost.TransferTollStartingpoint.Value;
                if (transVal < transStart)
                    transVal = transStart;
            }

            transVal = Utils.Round(transVal);
            result.TransferToll = transVal;

            /// 4.交易手续费
            //public decimal PoundageSingleValue { get; set; }
            if (cost.GetValueTypeID.HasValue)
            {
                int poundType = cost.GetValueTypeID.Value;

                //交易手续费单值 single double
                switch (poundType)
                {
                    case (int)Types.GetValueTypeEnum.Single:
                        decimal pound = cost.PoundageSingleValue.Value / 100;
                        decimal poundValue = pound * dealAmount;
                        poundValue = Utils.Round(poundValue);
                        result.PoundageSingleValue = poundValue;
                        break;
                    case (int)Types.GetValueTypeEnum.Scope:
                        //IList<XH_SpotRangeCost> costs =
                        //    MCService.SpotTradeRules.GetSpotRangeCostByBreedClassID(breedClass.BreedClassID);

                        //foreach (XH_SpotRangeCost spotRangeCost in costs)
                        //{
                        //    //int fieldRangeID = spotRangeCost.FieldRangeID;
                        //    decimal pound2 = spotRangeCost.Value.Value;
                        //    pound2 = Utils.Round(pound2);

                        //    CM_FieldRange fieldRange = MCService.GetFieldRange(fieldRangeID);

                        //    //是否在当前字段范围内
                        //    bool isExist = MCService.CheckFieldRange(dealAmount, fieldRange);
                        //    if (isExist)
                        //    {
                        //        result.PoundageSingleValue = pound2;
                        //        break;
                        //    }
                        //}
                        break;
                }
            }


            /// 5.监管费
            //public decimal MonitoringFee { get; set; }
            if (cost.MonitoringFee.HasValue)
            {
                decimal monitor = cost.MonitoringFee.Value / 100;

                decimal monitorVal = monitor * dealAmount;
                monitorVal = Utils.Round(monitorVal);

                result.MonitoringFee = monitorVal;
            }


            /// 6.结算费
            if (cost.ClearingFees.HasValue)
            {
                decimal clear = cost.ClearingFees.Value / 100;
                decimal clearval = clear * dealAmount;
                clearval = Utils.Round(clearval);

                result.ClearingFees = clearval;
            }


            /// 7.交易系统使用费（港股）
            if (cost.SystemToll.HasValue)
                result.TradeSystemFees = cost.SystemToll.Value;

            string format = "现货费用计算[代码={0},价格={1},数量={2},单位={3},方向={4}]";
            string desc = string.Format(format, request.Code, request.OrderPrice, request.OrderAmount,
                                        request.OrderUnitType, request.BuySell);
            LogHelper.WriteDebug(desc + result);
            return result;
        }

        #endregion

        #region 港股费用计算

        /// <summary>
        /// 获取港股交易费用
        /// </summary>
        /// <param name="request">港股委托</param>
        /// <returns>港股交易费用结果</returns>
        public static HKCostResult ComputeHKCost(HKOrderRequest request)
        {
            int? bc = MCService.CommonPara.GetBreedClassIdByCommodityCode(request.Code, Types.BreedClassTypeEnum.HKStock);
            HKCostResult result = null;

            if (!bc.HasValue)
            {
                return null;
            }
            int breedClassID = bc.Value;

            HK_SpotCosts cost = MCService.HKTradeRulesProxy.GetSpotCostsByBreedClassID(breedClassID);

            if (cost == null)
            {
                return null;
            }
            try
            {
                result = InternalComputeHKCost(request, cost);
            }
            catch (Exception ex)
            {
                string errCode = "GT-1701";
                string errMsg = "无法获取港股交易费用。";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception);
                throw exception;
            }

            return result;
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
        public static HKCostResult ComputeHKCost(string code, float price, int amount, Types.UnitType unitType, Types.TransactionDirection buySell)
        {
            HKOrderRequest request = new HKOrderRequest();
            request.Code = code;
            request.OrderPrice = price;
            request.OrderAmount = amount;
            request.OrderUnitType = unitType;
            request.BuySell = buySell;

            return ComputeHKCost(request);
        }

        /// <summary>
        /// 根据港股委托单和港股交易费用规则统计交易费用实体
        /// </summary>
        /// <param name="request">委托单</param>
        /// <param name="cost">交易费用规则</param>
        /// <returns>返回港股交易费统计实体</returns>
        private static HKCostResult InternalComputeHKCost(HKOrderRequest request, HK_SpotCosts cost)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code, Types.BreedClassTypeEnum.HKStock);

            HKCostResult result = new HKCostResult();

            result.Code = request.Code;

            decimal orderPrice = (decimal)request.OrderPrice;

            decimal scale = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, request.Code, request.OrderUnitType);
            //以计价单位计算的委托量
            var orderAmount = (decimal)request.OrderAmount * scale;

            //成交额
            var dealAmount = orderPrice * orderAmount;

            #region 1.印花税
            /// 1.印花税
            decimal stamp = cost.StampDuty.Value / 100;

            decimal stampStart = cost.StampDutyStartingpoint.Value;

            if (cost.StampDutyTypeID.HasValue)
            {

                int stampType = cost.StampDutyTypeID.Value;

                decimal stampVal = stamp * dealAmount;
                if (stampVal < stampStart)
                {
                    stampVal = stampStart;
                }
                stampVal = Utils.Round(stampVal);

                switch ((Types.GetValueTypeEnum)stampType)
                {
                    case Types.GetValueTypeEnum.Single:
                        break;
                    case Types.GetValueTypeEnum.Scope:
                        break;
                    case Types.GetValueTypeEnum.Turnover:
                        break;
                    case Types.GetValueTypeEnum.Thigh:
                        break;
                    case Types.GetValueTypeEnum.SingleBuy:
                        if (request.BuySell == Types.TransactionDirection.Buying)
                        {
                            result.StampDuty = stampVal;
                        }
                        break;
                    case Types.GetValueTypeEnum.SingleSell:
                        if (request.BuySell == Types.TransactionDirection.Selling)
                        {
                            result.StampDuty = stampVal;
                        }
                        break;
                    case Types.GetValueTypeEnum.TwoEdge:
                        result.StampDuty = stampVal;
                        break;
                    case Types.GetValueTypeEnum.No:
                        break;
                    default:
                        break;
                }


            }
            #endregion

            #region 2.佣金
            /// 2.佣金
            if (cost.Commision.HasValue)
            {
                decimal comm = cost.Commision.Value / 100;
                decimal commVal = comm * dealAmount;
                decimal commStart = 0;
                if (cost.CommisionStartingpoint.HasValue)
                {
                    commStart = cost.CommisionStartingpoint.Value;
                }
                if (commVal < commStart)
                    commVal = commStart;
                commVal = Utils.Round(commVal);
                result.Commision = commVal;
            }
            #endregion

            #region 过户费
            /// 3.过户费
            if (cost.TransferToll.HasValue)
            {
                decimal transVal = cost.TransferToll.Value / 100;
                transVal = Utils.Round(transVal);
                result.TransferToll = transVal;
            }
            #endregion

            #region 4.交易手续费
            #region old code
            ///// 4.交易手续费
            ////public decimal PoundageSingleValue { get; set; }
            //if (cost.GetValueTypeID.HasValue)
            //{
            //    int poundType = cost.GetValueTypeID.Value;

            //    //交易手续费单值 single double
            //    switch (poundType)
            //    {
            //        case (int)Types.GetValueTypeEnum.Single:
            //            decimal pound = cost.PoundageValue.Value / 100;
            //            decimal poundValue = pound * dealAmount;
            //            poundValue = Utils.Round(poundValue);
            //            result.PoundageSingleValue = poundValue;
            //            break;
            //        case (int)Types.GetValueTypeEnum.Scope:
            //            IList<XH_SpotRangeCost> costs =
            //                MCService.SpotTradeRules.GetSpotRangeCostByBreedClassID(breedClass.BreedClassID);

            //            foreach (XH_SpotRangeCost spotRangeCost in costs)
            //            {
            //                int fieldRangeID = spotRangeCost.FieldRangeID;
            //                decimal pound2 = spotRangeCost.Value.Value;
            //                pound2 = Utils.Round(pound2);

            //                CM_FieldRange fieldRange = MCService.GetFieldRange(fieldRangeID);

            //                //是否在当前字段范围内
            //                bool isExist = MCService.CheckFieldRange(dealAmount, fieldRange);
            //                if (isExist)
            //                {
            //                    result.PoundageSingleValue = pound2;
            //                    break;
            //                }
            //            }
            //            break;
            //    }
            //}    
            #endregion

            decimal pound = cost.PoundageValue.Value / 100;
            decimal poundValue = dealAmount * pound;
            poundValue = Utils.Round(poundValue);
            result.PoundageSingleValue = poundValue;

            #endregion

            #region  5.监管费
            /// 5.监管费
            //public decimal MonitoringFee { get; set; }
            if (cost.MonitoringFee.HasValue)
            {
                decimal monitor = cost.MonitoringFee.Value / 100;
                decimal monitorVal = monitor * dealAmount;
                monitorVal = Utils.Round(monitorVal);

                result.MonitoringFee = monitorVal;
            }
            #endregion

            #region  6.结算费（无）
            /// 6.结算费
            result.ClearingFees = 0;
            #endregion

            #region 7.交易系统使用费
            /// 7.交易系统使用费
            if (cost.SystemToll.HasValue)
            {
                result.TradeSystemFees = cost.SystemToll.Value;
            }
            #endregion

            string format = "港股费用计算[代码={0},价格={1},数量={2},单位={3},方向={4}]";
            string desc = string.Format(format, request.Code, request.OrderPrice, request.OrderAmount,
                                        request.OrderUnitType, request.BuySell);
            LogHelper.WriteDebug(desc + result);
            return result;
        }

        #endregion

        #region 商品期货费用计算

        /// <summary>
        /// 获取商品期货交易费用
        /// </summary>
        /// <param name="request">期货委托</param>
        /// <returns>商品期货交易费用结果</returns>
        public static QHCostResult ComputeSPQHCost(MercantileFuturesOrderRequest request)
        {
            int? bc = MCService.CommonPara.GetBreedClassIdByCommodityCode(request.Code);

            QHCostResult result = null;

            if (!bc.HasValue)
                return null;

            int breedClassID = bc.Value;
            QH_FutureCosts cost = MCService.FuturesTradeRules.GetFutureCostsByBreedClassID(breedClassID);
            if (cost == null)
                return null;

            try
            {
                result = InternalComputeSPQHCost(request, cost);
            }
            catch (Exception ex)
            {
                string errCode = "GT-1702";
                string errMsg = "无法获取期货货交易费用。";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception);
                throw exception;
            }

            return result;
        }

        /// <summary>
        /// 获取商品期货交易费用
        /// </summary>
        /// <param name="code">商品期货商品代码</param>
        /// <param name="price">委托价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="unitType">委托单位类型</param>
        /// <param name="openCloseType">开平仓方向</param>
        /// <returns>商品期货交易费用结果</returns>
        public static QHCostResult ComputeSPQHCost(string code, float price, int amount, Types.UnitType unitType, Entity.Contants.Types.FutureOpenCloseType openCloseType)
        {
            MercantileFuturesOrderRequest request = new MercantileFuturesOrderRequest();
            request.Code = code;
            request.OrderPrice = price;
            request.OrderAmount = amount;
            request.OrderUnitType = unitType;
            request.OpenCloseType = openCloseType;

            return ComputeSPQHCost(request);
        }

        /// <summary>
        /// 获取商品期货交易费用内部方法
        /// </summary>
        /// <param name="request">期货委托</param>
        /// <param name="cost">费用数据实体</param>
        /// <returns>商品期货交易费用结果</returns>
        private static QHCostResult InternalComputeSPQHCost(MercantileFuturesOrderRequest request, QH_FutureCosts cost)
        {
            QHCostResult result = new QHCostResult();
            //result.Code = result.Code;
            result.Code = request.Code;
            decimal orderPrice = (decimal)request.OrderPrice;
            //期货合约乘数300
            decimal scale = MCService.GetTradeUnitScale(request.Code, request.OrderUnitType);
            //以计价单位计算的委托量
            var orderAmount = (decimal)request.OrderAmount * scale;

            //成交额
            var dealAmount = orderPrice * orderAmount;

            decimal cosing = 0;

            //按类型计算比例
            switch ((Types.FutrueCostType)Enum.Parse(typeof(Types.FutrueCostType), cost.CostType.ToString()))
            {
                case Types.FutrueCostType.TradeUnitCharge:
                    //与量有关的都是撮合单位，所以这里不用再把委托量*转换比例
                    //为了不改上面的代码这里直接再把原来转了的值再除
                    orderAmount = (decimal)orderAmount / scale;
                    cosing = orderAmount * cost.TurnoverRateOfServiceCharge;
                    break;
                case Types.FutrueCostType.TurnoverRateOfSerCha:
                    //比例
                    decimal cosingScale = cost.TurnoverRateOfServiceCharge / 100;
                    cosing = dealAmount * cosingScale;
                    break;
            }
            cosing = Utils.Round(cosing);

            #region old code
            //decimal tradeVal = 0;
            ////成交单位比率
            //if (cost.TradeUnitCharge.HasValue)
            //{
            //    decimal trade = cost.TradeUnitCharge.Value / 100;
            //    tradeVal = orderAmount * trade;
            //    tradeVal = Utils.Round(tradeVal);
            //}

            //decimal turnVal = 0;
            ////成交金额比率
            //if (cost.TurnoverRateOfServiceCharge.HasValue)
            //{
            //    decimal turn = cost.TurnoverRateOfServiceCharge.Value / 100;
            //    turnVal = turn * dealAmount;
            //    turnVal = Utils.Round(turnVal);
            //}
            //result.Cosing = tradeVal + turnVal;
            #endregion

            result.Cosing = cosing;

            string format = "商品期货费用计算[代码={0},价格={1},数量={2},单位={3},方向={4},合约乘数={5},商品期货交易费用类型={6},费用比率={7}]";
            string desc = string.Format(format, request.Code, request.OrderPrice, request.OrderAmount,
                                        request.OrderUnitType, request.BuySell, scale
                                        , cost.CostType + "--" + (cost.CostType == (int)Types.FutrueCostType.TradeUnitCharge ? "按成交量" : "按成交额")
                                        , cost.TurnoverRateOfServiceCharge);
            LogHelper.WriteDebug(desc + result);

            return result;
        }

        #endregion

        #region 股指期货费用计算

        /// <summary>
        /// 获取股指期货交易费用
        /// </summary>
        /// <param name="request">股指期货委托</param>
        /// <returns>股指期货交易费用结果</returns>
        public static QHCostResult ComputeGZQHCost(StockIndexFuturesOrderRequest request)
        {
            int? bc = MCService.CommonPara.GetBreedClassIdByCommodityCode(request.Code);

            QHCostResult result = null;

            if (!bc.HasValue)
                return result;

            int breedClassID = bc.Value;
            QH_FutureCosts cost = MCService.FuturesTradeRules.GetFutureCostsByBreedClassID(breedClassID);

            if (cost == null)
                return null;


            try
            {
                result = InternalComputeGZQHCost(request, cost);
            }
            catch (Exception ex)
            {
                string errCode = "GT-1703";
                string errMsg = "无法获取股指期货货交易费用。";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception);
                throw exception;
            }

            return result;
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
            StockIndexFuturesOrderRequest request = new StockIndexFuturesOrderRequest();
            request.Code = code;
            request.OrderPrice = price;
            request.OrderAmount = amount;
            request.OrderUnitType = unitType;
            request.OpenCloseType = openCloseType;

            return ComputeGZQHCost(request);
        }

        /// <summary>
        /// 获取股指期货交易费用的实际方法，目前使用商品期货的计算方法
        /// </summary>
        /// <param name="request">股指期货委托</param>
        /// <param name="cost">费用实体</param>
        /// <returns>股指期货交易费用结果</returns>
        private static QHCostResult InternalComputeGZQHCost(StockIndexFuturesOrderRequest request, QH_FutureCosts cost)
        {
            QHCostResult result = new QHCostResult();
            //result.Code = result.Code;
            result.Code = request.Code;
            decimal orderPrice = (decimal)request.OrderPrice;
            //期货合约乘数300
            decimal scale = MCService.GetTradeUnitScale(request.Code, request.OrderUnitType);
            //以计价单位计算的委托量
            var orderAmount = (decimal)request.OrderAmount * scale;

            //成交额
            var dealAmount = orderPrice * orderAmount;

            decimal cosing = 0;

            //按类型计算比例
            switch ((Types.FutrueCostType)Enum.Parse(typeof(Types.FutrueCostType), cost.CostType.ToString()))
            {
                case Types.FutrueCostType.TradeUnitCharge:
                    //与量有关的都是撮合单位，所以这里不用再把委托量*转换比例
                    //为了不改上面的代码这里直接再把原来转了的值再除
                    orderAmount = (decimal)orderAmount / scale;
                    cosing = orderAmount * cost.TurnoverRateOfServiceCharge;
                    break;
                case Types.FutrueCostType.TurnoverRateOfSerCha:
                    //比例
                    decimal cosingScale = cost.TurnoverRateOfServiceCharge / 100;
                    cosing = dealAmount * cosingScale;
                    break;
            }
            cosing = Utils.Round(cosing);

            #region old code
            //decimal tradeVal = 0;
            ////成交单位比率
            //if (cost.TradeUnitCharge.HasValue)
            //{
            //    decimal trade = cost.TradeUnitCharge.Value / 100;
            //    tradeVal = orderAmount * trade;
            //    tradeVal = Utils.Round(tradeVal);
            //}

            //decimal turnVal = 0;
            ////成交金额比率
            //if (cost.TurnoverRateOfServiceCharge.HasValue)
            //{
            //    decimal turn = cost.TurnoverRateOfServiceCharge.Value / 100;
            //    turnVal = turn * dealAmount;
            //    turnVal = Utils.Round(turnVal);
            //}
            //result.Cosing = tradeVal + turnVal;
            #endregion

            result.Cosing = cosing;


            string format = "股指期货费用计算[代码={0},价格={1},数量={2},单位={3},方向={4},合约乘数={5},股指期货交易费用类型={6},费用比率={7}]";
            string desc = string.Format(format, request.Code, request.OrderPrice, request.OrderAmount,
                                        request.OrderUnitType, request.BuySell, scale
                                        , cost.CostType + "--" + (cost.CostType == (int)Types.FutrueCostType.TradeUnitCharge ? "按成交量" : "按成交额")
                                        , cost.TurnoverRateOfServiceCharge);
            LogHelper.WriteDebug(desc + result);

            return result;
        }

        #endregion

        #region 获取保本价

        /// <summary>
        /// 获取保本价
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="costPrice">成本价</param>
        /// <param name="amount">总持仓量(计价单位)</param>
        /// <param name="buySellType">买卖方向（仅期货有用）</param>
        /// <returns>保本价</returns>
        public static decimal GetHoldPrice(string code, decimal costPrice, decimal amount, Types.TransactionDirection buySellType)
        {
            string errCode = "GT-1704";
            string errMsg = "无法获取指定商品的保本价。";


            int? breedClassType = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(code);
            if (!breedClassType.HasValue)
                return 0;

            Types.BreedClassTypeEnum breedClassTypeEnum;
            try
            {
                breedClassTypeEnum = (Types.BreedClassTypeEnum)breedClassType.Value;
            }
            catch (Exception ex)
            {
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception);
                throw exception;
            }

            try
            {
                //费用——过户费：沪A股，成交面个的1%,深A股免收。起点1元。你的600股，过户费=600×0.001=0.6＜1元，所以过户费是1元
                //印花税——单项收费，只有卖股票收。
                //佣金（5.93）=佣金费率×成交金额=佣金费率×6594  所以你的佣金费率≈万分之九
                //此次你买入股票总支出金额=股票成交金额+佣金+过户费+印花税=6594+5.930+1+0=6600.93
                //所以你的买入成本价=总金额/买入数量=6600.93/600=11.00155
                //设卖出价格P
                //卖出收入＞买入总支出
                //600×P-过户费（1）-印花税（600×P×0.001）-佣金（P×600×0.0009）＞6600.93
                //598.86×P-1＞6600.93
                //P＞11.02416  保本价11.024

                switch (breedClassTypeEnum)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return GetStockHoldPrice(code, costPrice, amount);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return GetFutureHoldPrice(code, costPrice, amount, buySellType);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return GetStockFutureHoldPrice(code, costPrice, amount, buySellType);
                }
            }
            catch (Exception ex)
            {
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception);
                throw exception;
            }
            return 0;
        }

        /// <summary>
        /// 获取现货保本价
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="costPrice">成本价</param>
        /// <param name="amount">总持仓量</param>
        /// <returns>保本价</returns>
        private static decimal GetStockHoldPrice(string code, decimal costPrice, decimal amount)
        {
            int? bc = MCService.CommonPara.GetBreedClassIdByCommodityCode(code);

            if (!bc.HasValue)
            {
                return -1;
            }

            int breedClassID = bc.Value;

            XH_SpotCosts cost = MCService.SpotTradeRules.GetSpotCostsByBreedClassID(breedClassID);


            /// 1.印花税
            decimal stamp = cost.StampDuty / 100;
            stamp = Utils.Round(stamp, 4);

            /// 2.佣金
            decimal comm = cost.Commision.Value / 100;
            comm = Utils.Round(comm, 4);

            /// 3.过户费
            decimal trans = cost.TransferToll / 100;
            trans = Utils.Round(trans, 4);
            int transType = cost.TransferTollTypeID;


            /// 4.交易手续费
            decimal pound = cost.PoundageSingleValue.Value / 100;
            pound = Utils.Round(pound, 4);

            /// 5.监管费
            decimal monitor = cost.MonitoringFee.Value / 100;
            monitor = Utils.Round(monitor, 4);

            /// 6.结算费
            decimal clear = cost.ClearingFees.Value / 100;
            clear = Utils.Round(clear, 4);

            /// 7.交易系统使用费（港股）--->与价格无关,每笔0.5港元
            decimal tradeSystemFees = cost.SystemToll.Value;
            tradeSystemFees = Utils.Round(tradeSystemFees, 4);

            //所有与价格有关的费用百分比之和
            decimal scale = stamp + comm + pound + monitor + clear;
            scale = Utils.Round(scale, 4);

            //所有与价格无关的费用（单笔）
            decimal others = 0;
            //过户费类型 [按股] [按成交额]
            switch (transType)
            {
                case (int)Types.GetValueTypeEnum.Thigh:
                    others = tradeSystemFees + trans * amount;
                    break;
                case (int)Types.GetValueTypeEnum.Turnover:
                    others = tradeSystemFees;
                    scale += trans;
                    break;
            }

            others = Utils.Round(others);

            //计算公式:保本价=［进价*（1+佣金）+过户费*2］/（1-佣金）基中佣金、过户费均为费率。
            //以上面成交为例计算，保本价=［5.08*(1+3‰)+0］/(1-3‰)=5.11057约为5.11或5.12

            //保本价holdPrice
            //单股费用=所有与价格有关的费用百分比之和 × holdPrice + 所有与价格无关的费用（单笔）/amount
            //singleCost = scale * holdPrice + others/amount
            //holdPrice - singleCost = costPrice => holdPrice -(scale*holdPrice + others/amount) = costPrice
            //=>holdPrice = (costPrice + others/amount)/(1-scale)
            decimal holdPrice = (costPrice + others / amount) / (1 - scale);
            holdPrice = Utils.Round(holdPrice);

            return holdPrice;
        }

        /// <summary>
        /// 获取期货保本价
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="costPrice">成本价</param>
        /// <param name="amount">总持仓量</param>
        /// <param name="buySellType">买卖方向</param>
        /// <returns>保本价</returns>
        private static decimal GetFutureHoldPrice(string code, decimal costPrice, decimal amount, Types.TransactionDirection buySellType)
        {
            int? bc = MCService.CommonPara.GetBreedClassIdByCommodityCode(code);

            //QHCostResult result = null;

            if (!bc.HasValue)
            {
                return 0;
            }

            int breedClassId = bc.Value;
            QH_FutureCosts cost = MCService.FuturesTradeRules.GetFutureCostsByBreedClassID(breedClassId);

            #region old code

            ////成交单位比率
            //decimal trade = 0;
            //if (cost.TradeUnitCharge.HasValue)
            //{
            //    trade = cost.TradeUnitCharge.Value / 100;
            //    trade = Utils.Round(trade);
            //}
            ////decimal tradeVal = trade;

            ////成交金额比率
            //decimal turn = 0;
            //if (cost.TurnoverRateOfServiceCharge.HasValue)
            //{
            //    turn = cost.TurnoverRateOfServiceCharge.Value / 100;
            //    turn = Utils.Round(turn);
            //}
            ////decimal turnVal = turn * orderPrice ;
            //decimal holdPrice = 0;

            //if (buySellType == Types.TransactionDirection.Buying)
            //{
            //    holdPrice = (costPrice + trade) / (1 - turn);
            //}
            //else
            //{
            //    holdPrice = (costPrice + trade) / (1 + turn);
            //}

            //holdPrice = Utils.Round(holdPrice);

            //return holdPrice;
            #endregion

            //单笔费用
            decimal cosing = 0;

            //按类型计算比例
            //计算笔费用=费用比例*成交量*成交价/成交量--->费用比例*成交价
            switch ((Types.FutrueCostType)Enum.Parse(typeof(Types.FutrueCostType), cost.CostType.ToString()))
            {
                case Types.FutrueCostType.TradeUnitCharge:
                    //cosing = cost.TurnoverRateOfServiceCharge * amount / amount;////按成交额计算笔费用=费用比例*总持仓量*成交价/总持仓量--->费用比例*成交价
                    cosing = cost.TurnoverRateOfServiceCharge;
                    break;
                case Types.FutrueCostType.TurnoverRateOfSerCha:
                    cosing = cost.TurnoverRateOfServiceCharge / 100;
                    //cosing = cosing * amount * costPrice / amount;//按成交额计算笔费用=费用比例*总持仓量*成交价/总持仓量--->费用比例*成交价
                    cosing = cosing * costPrice;
                    break;
            }

            decimal holdPrice = 0;

            //买
            //保本价=（成本价*持仓量+费用）/持仓量
            //卖
            //保本价=（成本价*持仓量-费用）/持仓量
            if (buySellType == Types.TransactionDirection.Buying)
            {
                //保本价=（成本价*持仓量+成交单位比率*持仓量<或者成交金额比率*成本价*持仓量>）/持仓量
                //==>保本价=（成本价+成交单位比率<或者成交金额比率*成本价>）
                holdPrice = costPrice + cosing;
            }
            else
            {
                //保本价=（成本价*持仓量-成交单位比率*持仓量<或者成交金额比率*成本价*持仓量>）/持仓量
                //==>保本价=（成本价-成交单位比率<或者成交金额比率*成本价>）
                holdPrice = costPrice - cosing;
            }

            //holdPrice = costPrice + cosing;

            holdPrice = Utils.Round(holdPrice);

            return holdPrice;
        }

        /// <summary>
        /// 获取股指期货保本价(目前使用商品期货的方法)
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="costPrice">成本价</param>
        /// <param name="amount">总持仓量</param>
        /// <param name="buySellType">买卖方向</param>
        /// <returns>保本价</returns>
        private static decimal GetStockFutureHoldPrice(string code, decimal costPrice, decimal amount, Types.TransactionDirection buySellType)
        {
            return GetFutureHoldPrice(code, costPrice, amount, buySellType);
        }

        #endregion
    }
}