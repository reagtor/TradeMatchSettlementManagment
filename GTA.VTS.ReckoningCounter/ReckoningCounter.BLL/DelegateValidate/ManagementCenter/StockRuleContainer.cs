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
using ReckoningCounter.DAL.SpotTradingDevolveService;
using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 描述：现货品种检验交易规则，错误码范围1250-1299
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public class StockRuleContainer : RuleContainer<StockOrderRequest>
    {
        public StockRuleContainer(int BreedClassID) : base(BreedClassID)
        {
        }

        protected override void FillCommandList()
        {
            //0.交易员权限--交易员是否有此品种的交易权限
            StockTraderRightsCommand stockTraderRightsCommand = new StockTraderRightsCommand(this.BreedClassID);
            AddCommand(stockTraderRightsCommand);

            StockZeroVolumeOfBusinessCommand zeroVolumeOfBusinessCommand = new StockZeroVolumeOfBusinessCommand(this.BreedClassID);
            AddCommand(zeroVolumeOfBusinessCommand);

            //1.最小交易单位(计价单位）因为牵涉到持仓，所以分离出来，由外部单独进行调用
            //StockMinVolumeOfBusinessCommand minVolumeOfBusinessCommand = new StockMinVolumeOfBusinessCommand(this.BreedClassID);
            //AddCommand(minVolumeOfBusinessCommand);

            //2.最大委托量
            StockMaxLeaveQuantityRangeValueCommand stockMaxLeaveQuantityRangeValueCommand =
                new StockMaxLeaveQuantityRangeValueCommand(this.BreedClassID);
            AddCommand(stockMaxLeaveQuantityRangeValueCommand);

            //3.最小变动价位
            StockMinChangePriceValueCommand stockMinChangePriceValueCommand =
                new StockMinChangePriceValueCommand(this.BreedClassID);
            AddCommand(stockMinChangePriceValueCommand);

            //4.涨跌幅
            StockSpotHighLowCommand stockSpotHighLowCommand = new StockSpotHighLowCommand(this.BreedClassID);
            AddCommand(stockSpotHighLowCommand);
        }
    }

    #region Validate Commands

    /// <summary>
    /// 现货验证命令的基类
    /// </summary>
    public abstract class StockValidateCommand : ValidateCommand<StockOrderRequest>
    {
        /// <summary>
        /// 现货_品种_交易规则
        /// </summary>
        private XH_SpotTradeRules m_SpotTradeRules;

        protected StockValidateCommand(int breedClassID) : base(breedClassID)
        {
        }

        /// <summary>
        /// 现货_品种_交易规则
        /// </summary>
        protected XH_SpotTradeRules TradeRules
        {
            get
            {
                if (m_SpotTradeRules == null)
                    m_SpotTradeRules = MCService.SpotTradeRules.GetSpotTradeRulesByBreedClassID(BreedClassID);
                return m_SpotTradeRules;
            }
        }
    }

    /// <summary>
    /// 0.交易员权限--交易员是否有此品种的交易权限
    /// </summary>
    public class StockTraderRightsCommand : StockValidateCommand
    {
        public StockTraderRightsCommand(int breedClassID) : base(breedClassID)
        {
        }

        public override bool Validate(StockOrderRequest request, ref string strMessage)
        {
            string errMsg = "交易员没有此品种的交易权限！";
            string errCode = "GT-1254";

            string tradeID = request.TraderId;
            if (string.IsNullOrEmpty(tradeID))
            {
                strMessage = errCode + ":" + errMsg;
                return false;
            }

            int userid = 0;
            try
            {
                userid = int.Parse(tradeID.Trim());
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                strMessage = errCode + ":" + errMsg;
                return false;
            }

            var rightList = MCService.CommonPara.GetTransactionRightTable(userid);
            if (Utils.IsNullOrEmpty(rightList))
            {
                strMessage = errCode + ":" + errMsg;
                return false;
            }

            foreach (UM_DealerTradeBreedClass tradeBreedClass in rightList)
            {
                if (!tradeBreedClass.BreedClassID.HasValue)
                    continue;

                if (this.BreedClassID == tradeBreedClass.BreedClassID.Value)
                    return true;
            }

            strMessage = errCode + ":" + errMsg;
            return false;
        }
    }

    /// <summary>
    /// 11.零请求校验
    /// </summary>
    public class StockZeroVolumeOfBusinessCommand : StockValidateCommand
    {
        public StockZeroVolumeOfBusinessCommand(int BreedClassID)
            : base(BreedClassID)
        {
        }

        #region Implementation of ValidateCommand<StockOrderRequest>

        public override bool Validate(StockOrderRequest request, ref string strMessage)
        {
            string errMsg = "现货委托数量必须大于零！";
            string errCode = "GT-1255";
            strMessage = errCode + ":" + errMsg;

            if(request.OrderAmount <= 0)
            {
                return false;
            }
            strMessage = "";
            return true;
        }

        #endregion
    }

    /// <summary>
    /// 1.最小交易单位检验命令,因为牵涉到持仓，所以不再由StockRuleContainer调用，由外部单独进行调用
    /// </summary>
    public class StockMinVolumeOfBusinessCommand : StockValidateCommand
    {
        /// <summary>
        /// 交易规则_交易方向_交易单位_交易量(最小交易单位)集合
        /// </summary>
        private IList<XH_MinVolumeOfBusiness> m_MinVolumeOfBusinessList;

        /// <summary>
        /// 持仓量(必须是计价单位)
        /// </summary>
        private int m_Position;

        public StockMinVolumeOfBusinessCommand(int BreedClassID, int position) : base(BreedClassID)
        {
            this.m_Position = position;
        }

        /// <summary>
        /// 交易规则_交易方向_交易单位_交易量(最小交易单位)属性
        /// </summary>
        public IList<XH_MinVolumeOfBusiness> MinVolumeOfBusinessList
        {
            get
            {
                if (m_MinVolumeOfBusinessList == null)
                {
                    m_MinVolumeOfBusinessList =
                        MCService.SpotTradeRules.GetMinVolumeOfBusinessByBreedClassID(BreedClassID);
                }
                return m_MinVolumeOfBusinessList;
            }
        }

        #region Implementation of ValidateCommand<StockOrderRequest>

        public override bool Validate(StockOrderRequest request, ref string strMessage)
        {
            string errMsg = "现货最小单位检验失败！";
            string errCode = "GT-1250";

            int unitID = (int) request.OrderUnitType;
            int tradeWayID = (int) request.BuySell;

            if (MinVolumeOfBusinessList == null)
            {
                strMessage = errCode + ":" + errMsg;
                return false;
            }

            var minVolumes = from minVolume in MinVolumeOfBusinessList
                             where minVolume.UnitID.Value == unitID && minVolume.TradeWayID.Value == tradeWayID
                             select minVolume;
            ;
            //当此品种没有此交易单位时检验不通过。如权证 下单80股则检验失败(单位只有手和份)
            if (minVolumes.Count() == 0)
            {
                strMessage = errCode + ":" + errMsg;
                return false;
            }

            if (request.BuySell == Types.TransactionDirection.Buying)
            {
                foreach (XH_MinVolumeOfBusiness volume in minVolumes)
                {
                    if ((int) request.OrderUnitType == volume.UnitID)
                    {
                        if (request.OrderAmount < volume.VolumeOfBusiness.Value ||
                            (request.OrderAmount%volume.VolumeOfBusiness.Value != 0))
                        {
                            strMessage = errCode + ":" + errMsg;
                            return false;
                        }
                    }
                }
            }
                //卖的话需要进行零股处理
            else
            {
                //委托单位转换为计价单位
                decimal scale = MCService.GetTradeUnitScale(request.Code, request.OrderUnitType);
                var amount = (decimal) request.OrderAmount*scale;


                //持仓单位(默认为撮合单位)转为计价单位
                decimal scale2 = MCService.GetMatchUnitScale(request.Code);
                var position = m_Position*scale2;

                if (amount > position)
                {
                    strMessage = errCode + ":" + "超过可用持仓";
                    return false;
                }

                //现货最小单位零股检验
                foreach (XH_MinVolumeOfBusiness volume in minVolumes)
                {
                    if ((int)request.OrderUnitType == volume.UnitID)
                    {
                        if (request.OrderAmount < volume.VolumeOfBusiness.Value ||
                            (request.OrderAmount % volume.VolumeOfBusiness.Value != 0))
                        {
                            if (amount != position)
                            {
                                strMessage = errCode + ":" + errMsg;
                                return false;
                            }
                        }
                    }
                }

                //现货最小单位零股检验
                //int zeroScale = TradeRules.MinVolumeMultiples;

                //if (amount%zeroScale != 0)
                //{
                //    if (amount != position)
                //    {
                //        strMessage = errCode + ":" + errMsg;
                //        return false;
                //    }
                //}
            }


            return true;
        }

        #endregion
    }

    /// <summary>
    /// 2.最大委托量检验命令
    /// </summary>
    public class StockMaxLeaveQuantityRangeValueCommand : StockValidateCommand
    {
        /// <summary>
        /// 交易规则_最大委托量_范围_值 集合
        /// </summary>
        //private IList<XH_MaxLeaveQuantityRangeValue> m_MaxLeaveQuantityRangeValue;
        public StockMaxLeaveQuantityRangeValueCommand(int breedClassID) : base(breedClassID)
        {
        }

        #region Overrides of ValidateCommand<StockOrderRequest>

        public override bool Validate(StockOrderRequest request, ref string strMessage)
        {
            bool result = false;

            string errMsg = "超过当前现货单笔最大委托量！";
            string errCode = "GT-1251";
            strMessage = errCode + ":" + errMsg;


            int maxUnit = TradeRules.MaxLeaveQuantityUnit;

            Types.UnitType maxUnitType;
            try
            {
                maxUnitType = (Types.UnitType) maxUnit;
            }
            catch
            {
                return false;
            }

            decimal scale = MCService.GetTradeUnitScale(request.Code, request.OrderUnitType);
            decimal scale2 = MCService.GetTradeUnitScale(request.Code, maxUnitType);

            var orderAmount = (decimal) request.OrderAmount*scale;

            int? maxSingle = TradeRules.MaxLeaveQuantity;
            if (maxSingle.HasValue)
            {
                decimal maxAmount = maxSingle.Value*scale2;
                if (orderAmount <= maxAmount)
                {
                    result = true;
                    strMessage = "";
                }
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// 3.最小变动价位检验命令
    /// </summary>
    public class StockMinChangePriceValueCommand : StockValidateCommand
    {
        ///// <summary>
        /////交易规则_最小变动价位_范围_值 集合
        ///// </summary>
        //private IList<XH_MinChangePriceValue> m_MinChangePriceValue;

        public StockMinChangePriceValueCommand(int breedClassID) : base(breedClassID)
        {
        }

        #region Overrides of ValidateCommand<StockOrderRequest>

        public override bool Validate(StockOrderRequest request, ref string strMessage)
        {
            bool result = false;

            string errMsg = "现货最小变动价位检验失败！";
            string errCode = "GT-1252";
            strMessage = errCode + ":" + errMsg;

            int? min = TradeRules.ValueTypeMinChangePrice;
            if (!min.HasValue)
            {
                return false;
            }

            int minValueType = min.Value;
            decimal orderPrice = (decimal) request.OrderPrice;
            switch (minValueType)
            {
                case (int) Types.GetValueTypeEnum.Single:
                    decimal? minSingle = TradeRules.MinChangePrice;
                    if (minSingle.HasValue)
                    {
                        decimal minSingleValue = minSingle.Value;

                        //是否满足最小变动价位
                        if (orderPrice%minSingleValue == 0)
                        {
                            result = true;
                            strMessage = "";
                        }
                    }

                    break;
                case (int) Types.GetValueTypeEnum.Scope:
                    //foreach (XH_MinChangePriceValue changePriceValue in MinChangePriceValue)
                    //{
                    //    if (changePriceValue.Value.HasValue)
                    //    {
                    //        decimal fValue = changePriceValue.Value.Value;
                    //        CM_FieldRange fieldRange = MCService.GetFieldRange(changePriceValue.FieldRangeID);

                    //        //是否在当前字段范围内
                    //        result = MCService.CheckFieldRange(orderPrice, fieldRange);
                    //        if (result)
                    //        {
                    //            //是否满足最小变动价位
                    //            if (orderPrice%fValue == 0)
                    //            {
                    //                strMessage = "";

                    //                return true;
                    //            }
                    //        }
                    //    }
                    //}
                    break;
            }


            return result;
        }

        #endregion

        ///// <summary>
        ///// 交易规则_最小变动价位_范围_值 属性
        ///// </summary>
        //public IList<XH_MinChangePriceValue> MinChangePriceValue
        //{
        //    get
        //    {
        //        if (m_MinChangePriceValue == null)
        //        {
        //            m_MinChangePriceValue = MCService.SpotTradeRules.GetMinChangePriceValueByBreedClassID(BreedClassID);
        //        }
        //        return m_MinChangePriceValue;
        //    }
        //}
    }

    /// <summary>
    /// 4.现货_品种_涨跌幅校验命令
    /// </summary>
    public class StockSpotHighLowCommand : StockValidateCommand
    {
        public StockSpotHighLowCommand(int breedClassID) : base(breedClassID)
        {
        }

        #region Overrides of ValidateCommand<StockOrderRequest>

        public override bool Validate(StockOrderRequest request, ref string strMessage)
        {
            //根据要求，柜台不再做涨跌幅判断，由撮合处理
            return true;

            #region  根据要求，柜台不再做涨跌幅判断，由撮合处理 所以以下注释
            //string errMsg = "现货涨跌幅检验失败！";
            //string errCode = "GT-1253";

            ////如果是市价单，那么委托价格是0，不再校验
            //if (request.OrderWay == Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
            //{
            //    return true;
            //}

            //decimal orderPrice = (decimal) request.OrderPrice;

            //decimal orderPriceD = (decimal) request.OrderPrice;
            //HighLowRangeValue value = MCService.HLRangeProcessor.GetHighLowRangeValueByCommodityCode(request.Code,
            //                                                                                         orderPriceD);
            //if (value == null)
            //{
            //    return true;
            //}

            //if (value.RangeType != Types.HighLowRangeType.HongKongPrice)
            //{
            //    decimal high = value.HighRangeValue;
            //    decimal low = value.LowRangeValue;

            //    if (orderPrice >= low && orderPrice <= high)
            //    {
            //        return true;
            //    }

            //    strMessage = errCode + ":" + errMsg;
            //    return false;
            //}
            //else
            //{
            //    HKRangeValue hkRangeValue = value.HongKongRangeValue;
            //    decimal buyH = hkRangeValue.BuyHighRangeValue;
            //    decimal buyL = hkRangeValue.BuyLowRangeValue;

            //    decimal sellH = hkRangeValue.SellHighRangeValue;
            //    decimal sellL = hkRangeValue.SellLowRangeValue;

            //    decimal high = 0;
            //    decimal low = 0;

            //    if (request.BuySell == Types.TransactionDirection.Buying)
            //    {
            //        low = buyL;
            //        high = buyH;
            //    }
            //    else
            //    {
            //        low = sellL;
            //        high = sellH;
            //    }

            //    if (orderPrice >= low && orderPrice <= high)
            //    {
            //        return true;
            //    }

            //    strMessage = errCode + ":" + errMsg;
            //    return false;
            //}
            ////HighLowRange highLowRange = MCService.HLRangeProcessor.GetStockHighLowRangeByCommodityCode(request.Code);
            ////switch (highLowRange.RangeType)
            ////{
            ////    case Types.HighLowRangeType.YesterdayCloseScale:
            ////        return CheckType1(request, highLowRange);
            ////    case Types.HighLowRangeType.RecentDealScale:
            ////        return CheckType2(request, highLowRange);
            ////    case Types.HighLowRangeType.Buy1Sell1Scale:
            ////        return CheckType3(request, highLowRange);
            ////    case Types.HighLowRangeType.RightPermitHighLow:
            ////        return CheckType4(request, highLowRange);
            ////    case Types.HighLowRangeType.HongKongPrice:
            ////        return CheckType5(request, highLowRange);
            ////    case Types.HighLowRangeType.RecentDealNumber:
            ////        return CheckType6(request, highLowRange);
            ////}
            #endregion
        }

        #region HighLowRangeType

        /// <summary>
        /// 1.昨日收盘价的上下百分比
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool CheckType1(StockOrderRequest request, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(request.Code);
            HqExData exData = data;


            float yClose = exData.YClose;
            decimal yestPrice = (decimal) yClose;

            decimal high = yestPrice*(1 + highRange);
            decimal low = yestPrice*(1 - lowRange);

            decimal orderPrice = (decimal) request.OrderPrice;

            if (orderPrice >= low && orderPrice <= high)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 2.最近成交价的上下百分比
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool CheckType2(StockOrderRequest request, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(request.Code);
            HqExData exData = data;

            float rPrice = exData.LastVolume;
            decimal recentPrice = (decimal) rPrice;

            decimal high = recentPrice*(1 + highRange);
            decimal low = recentPrice*(1 - lowRange);

            decimal orderPrice = (decimal) request.OrderPrice;

            if (orderPrice >= low && orderPrice <= high)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 3.买一卖一百分比
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool CheckType3(StockOrderRequest request, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(request.Code);
            HqExData exData = data;
            HqData hqData = exData.HqData;

            float b1 = hqData.Buyprice1;
            decimal buy1 = (decimal) b1;

            float s1 = hqData.Sellprice1;
            decimal sell1 = (decimal) s1;

            decimal high = sell1*highRange;
            decimal low = buy1*lowRange;


            decimal orderPrice = (decimal) request.OrderPrice;

            if (orderPrice >= low && orderPrice <= high)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 4.权证涨跌幅
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool CheckType4(StockOrderRequest request, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            decimal orderPrice = (decimal) request.OrderPrice;

            if (orderPrice >= lowRange && orderPrice <= highRange)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 5.港股买卖价位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool CheckType5(StockOrderRequest request, HighLowRange highLowRange)
        {
            HKRange hkRange = highLowRange.HongKongRange;

            decimal buyHighRange = hkRange.BuyHighRange;
            decimal buyLowRange = hkRange.BuyLowRange;
            decimal sellHighRange = hkRange.SellHighRange;
            decimal sellLowRange = hkRange.SellLowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(request.Code);
            HqExData exData = data;
            HqData hqData = exData.HqData;

            float b1 = hqData.Buyprice1;
            decimal buy1 = (decimal) b1;

            float s1 = hqData.Sellprice1;
            decimal sell1 = (decimal) s1;

            decimal buyH = sell1;
            decimal buyL = MCService.HLRangeProcessor.GetHKRangeValue(request.Code, (decimal) request.OrderPrice, buy1,
                                                                      -buyLowRange);

            decimal sellH = MCService.HLRangeProcessor.GetHKRangeValue(request.Code, (decimal) request.OrderPrice, sell1,
                                                                       sellHighRange);
            decimal sellL = buy1;

            decimal high = 0;
            decimal low = 0;

            if (request.BuySell == Types.TransactionDirection.Buying)
            {
                low = buyL;
                high = buyH;
            }
            else
            {
                low = sellL;
                high = sellH;
            }

            decimal orderPrice = (decimal) request.OrderPrice;

            if (orderPrice >= low && orderPrice <= high)
            {
                return true;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 6.最近成交价上下各多少元
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool CheckType6(StockOrderRequest request, HighLowRange highLowRange)
        {
            decimal highRange = highLowRange.HighRange;
            decimal lowRange = highLowRange.LowRange;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            HqExData data = service.GetStockHqData(request.Code);
            HqExData exData = data;

            float rPrice = exData.LastVolume;
            decimal recentPrice = (decimal) rPrice;

            decimal high = recentPrice + highRange;
            decimal low = recentPrice - lowRange;

            decimal orderPrice = (decimal) request.OrderPrice;

            if (orderPrice >= low && orderPrice <= high)
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }

    #endregion
}