using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity;
using ReckoningCounter.BLL.ManagementCenter;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.SpotTradingDevolveService;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using RealTime.Server.SModelData.HqData;
using ReckoningCounter.DAL.HKTradingRulesService;

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 描述：港股品种检验交易规则，错误码范围1250-1299
    /// 作者：李健华
    /// 日期：2009-10-20
    /// </summary>
    public class HKStockRuleContainer : RuleContainer<HKOrderRequest>
    {
        public HKStockRuleContainer(int BreedClassID)
            : base(BreedClassID)
        {
        }

        protected override void FillCommandList()
        {
            //0.交易员权限--交易员是否有此品种的交易权限
            HKStockTraderRightsCommand hkStockTraderRightsCommand = new HKStockTraderRightsCommand(this.BreedClassID);
            AddCommand(hkStockTraderRightsCommand);

            HKStockZeroVolumeOfBusinessCommand zeroVolumeOfBusinessCommand = new HKStockZeroVolumeOfBusinessCommand(this.BreedClassID);
            AddCommand(zeroVolumeOfBusinessCommand);

            //1.最小交易单位(计价单位）因为牵涉到持仓，所以分离出来，由外部单独进行调用
            //StockMinVolumeOfBusinessCommand minVolumeOfBusinessCommand = new StockMinVolumeOfBusinessCommand(this.BreedClassID);
            //AddCommand(minVolumeOfBusinessCommand);

            //2.最大委托量
            HKStockMaxLeaveQuantityRangeValueCommand stockMaxLeaveQuantityRangeValueCommand =
                new HKStockMaxLeaveQuantityRangeValueCommand(this.BreedClassID);
            AddCommand(stockMaxLeaveQuantityRangeValueCommand);

            //3.最小变动价位
            HKStockMinChangePriceValueCommand stockMinChangePriceValueCommand =
                new HKStockMinChangePriceValueCommand(this.BreedClassID);
            AddCommand(stockMinChangePriceValueCommand);

            //4.涨跌幅
            HKStockSpotHighLowCommand stockSpotHighLowCommand = new HKStockSpotHighLowCommand(this.BreedClassID);
            AddCommand(stockSpotHighLowCommand);
        }
    }
    #region Validate Commands

    #region 港股验证命令的基类
    /// <summary>
    /// 港股验证命令的基类
    /// </summary>
    public abstract class HKStockValidateCommand : ValidateCommand<HKOrderRequest>
    {
        /// <summary>
        /// 港股_品种_交易规则
        /// </summary>
        private HK_SpotTradeRules hk_SpotTradeRules;

        protected HKStockValidateCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        /// <summary>
        /// 港股_品种_交易规则
        /// </summary>
        protected HK_SpotTradeRules TradeRules
        {
            get
            {
                if (hk_SpotTradeRules == null)
                    hk_SpotTradeRules = MCService.HKTradeRulesProxy.GetSpotTradeRulesByBreedClassID(BreedClassID);
                return hk_SpotTradeRules;
            }
        }
    }
    #endregion

    #region 0.交易员权限--交易员是否有此品种的交易权限
    /// <summary>
    /// 0.交易员权限--交易员是否有此品种的交易权限
    /// </summary>
    public class HKStockTraderRightsCommand : HKStockValidateCommand
    {
        public HKStockTraderRightsCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        public override bool Validate(HKOrderRequest request, ref string strMessage)
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
    #endregion

    #region 0.零请求校验
    /// <summary>
    ///0.零请求校验
    /// </summary>
    public class HKStockZeroVolumeOfBusinessCommand : HKStockValidateCommand
    {
        public HKStockZeroVolumeOfBusinessCommand(int BreedClassID)
            : base(BreedClassID)
        {
        }

        #region Implementation of ValidateCommand<HKOrderRequest>

        public override bool Validate(HKOrderRequest request, ref string strMessage)
        {
            string errMsg = "港股委托数量必须大于零！";
            string errCode = "GT-1255";
            strMessage = errCode + ":" + errMsg;

            if (request.OrderAmount <= 0)
            {
                return false;
            }
            strMessage = "";
            return true;
        }

        #endregion
    }
    #endregion

    #region  1.最小交易单位检验命令
    /// <summary>
    /// 1.最小交易单位检验命令,因为牵涉到持仓，所以不再由HKStockRuleContainer调用，由外部单独进行调用
    /// </summary>
    public class HKStockMinVolumeOfBusinessCommand : HKStockValidateCommand
    {
        /// <summary>
        /// 交易规则_交易方向_交易单位_交易量(最小交易单位)集合（此中的集合用原来现货中的表数据）
        /// </summary>
        private IList<ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness> m_MinVolumeOfBusinessList;

        /// <summary>
        /// 持仓量(必须是计价单位)
        /// </summary>
        private int m_Position;

        public HKStockMinVolumeOfBusinessCommand(int BreedClassID, int position)
            : base(BreedClassID)
        {
            this.m_Position = position;
        }

        /// <summary>
        /// 交易规则_交易方向_交易单位_交易量(最小交易单位)属性
        /// </summary>
        public IList<ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness> MinVolumeOfBusinessList
        {
            get
            {
                if (Utils.IsNullOrEmpty(m_MinVolumeOfBusinessList))
                {
                    m_MinVolumeOfBusinessList =
                        MCService.SpotTradeRules.GetMinVolumeOfBusinessByBreedClassID(BreedClassID);
                }
                return m_MinVolumeOfBusinessList;
            }
        }

        #region Implementation of ValidateCommand<HKOrderRequest>

        public override bool Validate(HKOrderRequest request, ref string strMessage)
        {
            string errMsg = "港股最小单位检验失败！";
            string errCode = "GT-1250";

            int unitID = (int)request.OrderUnitType;
            int tradeWayID = (int)request.BuySell;

            if (Utils.IsNullOrEmpty(MinVolumeOfBusinessList))
            {
                strMessage = errCode + ":" + errMsg;
                return false;
            }

            //======update 2009-11-08 李健华======
            //港股只有手对股的转换，这里特殊处理，而进入此方法后的验证单位和量都转换成了股单位，
            //而对应的规则表（XH_MinVolumeOfBusiness表）中只有手的转换，因此这里要特殊转换

            //判断是否包含有股的转换 ,没有就自行模拟加入
            bool isThigh = false;
            List<ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness> minVolumesList = new List<ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness>(MinVolumeOfBusinessList);
            //minVolumesList = new List<ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness>(MinVolumeOfBusinessList);

            // minVolumesList =MinVolumeOfBusinessList;
            foreach (var item in MinVolumeOfBusinessList)
            {
                if (item.UnitID == (int)Types.UnitType.Thigh)
                {
                    isThigh = true;
                    break;
                }
            }
            //判断是否包含有股的转换 ,没有就自行模拟加入
            if (!isThigh)
            {
                HK_Commodity hkComm = MCService.HKTradeRulesProxy.GetHKCommodityByCommodityCode(request.Code);
                ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness itembuy = new ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness();
                itembuy.UnitID = (int)Types.UnitType.Thigh;
                itembuy.VolumeOfBusiness = hkComm.PerHandThighOrShare;
                itembuy.TradeWayID = (int)Types.TransactionDirection.Buying;
                minVolumesList.Add(itembuy);

                ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness itemSell = new ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness();
                itemSell.UnitID = (int)Types.UnitType.Thigh;
                itemSell.VolumeOfBusiness = hkComm.PerHandThighOrShare;
                itemSell.TradeWayID = (int)Types.TransactionDirection.Selling;
                minVolumesList.Add(itemSell);

            }

            var minVolumes = from minVolume in minVolumesList
                             where minVolume.UnitID.Value == unitID && minVolume.TradeWayID.Value == tradeWayID
                             select minVolume;

            //当此品种没有此交易单位时检验不通过。如权证 下单80股则检验失败(单位只有手和份)
            if (minVolumes.Count() == 0)
            {
                strMessage = errCode + ":" + errMsg;
                return false;
            }

            //==================

            if (request.BuySell == Types.TransactionDirection.Buying)
            {
                foreach (ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness volume in minVolumes)
                {
                    if ((int)request.OrderUnitType == volume.UnitID)
                    {
                        if (request.OrderAmount < volume.VolumeOfBusiness.Value ||
                            (request.OrderAmount % volume.VolumeOfBusiness.Value != 0))
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
                decimal scale = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, request.Code, request.OrderUnitType);
                var amount = (decimal)request.OrderAmount * scale;


                //持仓单位(默认为撮合单位)转为计价单位
                decimal scale2 = MCService.GetMatchUnitScale(Types.BreedClassTypeEnum.HKStock, request.Code);
                var position = m_Position * scale2;

                if (amount > position)
                {
                    strMessage = errCode + ":" + "超过可用持仓";
                    return false;
                }

                //港股最小单位零股检验
                foreach (ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness volume in minVolumes)
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

                //港股最小单位零股检验
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
    #endregion

    #region 2.最大委托量检验命令
    /// <summary>
    /// 2.最大委托量检验命令
    /// </summary>
    public class HKStockMaxLeaveQuantityRangeValueCommand : HKStockValidateCommand
    {
        /// <summary>
        /// 交易规则_最大委托量_范围_值 集合
        /// </summary>
        //private IList<XH_MaxLeaveQuantityRangeValue> m_MaxLeaveQuantityRangeValue;
        public HKStockMaxLeaveQuantityRangeValueCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        #region Overrides of ValidateCommand<HKOrderRequest>

        public override bool Validate(HKOrderRequest request, ref string strMessage)
        {
            bool result = false;

            string errMsg = "超过当前港股单笔最大委托量！";
            string errCode = "GT-1251";
            strMessage = errCode + ":" + errMsg;


            //int maxUnit = TradeRules.MaxLeaveQuantityUnit;
            int maxUnit = TradeRules.PriceUnit;//这与MaxLeaveQuantityUnit字段同相同的

            Types.UnitType maxUnitType;
            try
            {
                maxUnitType = (Types.UnitType)maxUnit;
            }
            catch
            {
                return false;
            }

            decimal scale = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, request.Code, request.OrderUnitType);
            decimal scale2 = MCService.GetTradeUnitScale(Types.BreedClassTypeEnum.HKStock, request.Code, maxUnitType);

            var orderAmount = (decimal)request.OrderAmount * scale;
            //===Update By;李健华 2009-11-10 ===
            // 因为港股规则里填写的最大委托量为手而之前的现货直接填写的为股，所以这里在要转换过来为股
            //，而在这里验证的时候传入的都为股单位
            //int? maxSingle = TradeRules.MaxLeaveQuantity;
            int? maxSingle = TradeRules.MaxLeaveQuantity;

            if (maxSingle.HasValue)
            {
                int perHand = MCService.HKTradeRulesProxy.GetHKPerHandThighOrShareByCode(request.Code, out strMessage);
                maxSingle = maxSingle.Value * perHand;

                //====================
                decimal maxAmount = maxSingle.Value * scale2;
                if (orderAmount <= maxAmount)
                {
                    result = true;
                    strMessage = "";
                }
                else
                {
                    if (string.IsNullOrEmpty(strMessage))
                    {
                        strMessage = errCode + ":" + errMsg;
                    }
                }
            }

            return result;
        }

        #endregion
    }
    #endregion

    #region  3.最小变动价位检验命令
    /// <summary>
    /// 3.最小变动价位检验命令
    /// </summary>
    public class HKStockMinChangePriceValueCommand : HKStockValidateCommand
    {
        /// <summary>
        ///交易规则_最小变动价位_范围_值 集合
        /// </summary>
        private IList<HK_MinPriceFieldRange> m_MinPriceFiledRange;

        /// <summary>
        ///
        /// </summary>
        /// <param name="breedClassID"></param>
        public HKStockMinChangePriceValueCommand(int breedClassID)
            : base(breedClassID)
        {
        }
        /// <summary>
        /// 交易规则_最小变动价位_范围_值 属性
        /// </summary>
        public IList<HK_MinPriceFieldRange> MinChangePriceValue
        {
            get
            {
                if (m_MinPriceFiledRange == null || m_MinPriceFiledRange.Count <= 0)
                {
                    m_MinPriceFiledRange = MCService.HKTradeRulesProxy.GetAllHKMinPriceFieldRange();
                }
                return m_MinPriceFiledRange;
            }
        }

        #region Overrides of ValidateCommand<HKOrderRequest>

        public override bool Validate(HKOrderRequest request, ref string strMessage)
        {
            bool result = false;

            string errMsg = "港股最小变动价位检验失败！";
            string errCode = "GT-1252";
            strMessage = errCode + ":" + errMsg;

            #region //////
            // 价位（靠后不靠前）（港元）	最小变动单位
            //0.01-0.25	0.001
            //0.25-0.50	0.005
            //0.50-10.00	0.010
            //10.00-20.00	0.020
            //20.00-100.00	 0.050
            //100-200.00	 0.100
            //200.00-500.00	 0.200
            //500.00-1,000.00	0.500
            //1,000.00-2,000.00	1.000
            //2,000.00-5,000.00	2.000
            //5,000.00-9,995.00	5.000

            ////从高到低判断
            //if (5000.00F < orderPrice && orderPrice >= 9995.00F)
            //{
            //    //	5.000
            //}
            //if (2000.00F < orderPrice && orderPrice >= 5000.00F)
            //{
            //    //	2.000
            //}
            //if (1000.00F < orderPrice && orderPrice >= 2000.00F)
            //{
            //    //	1.000
            //}
            //if (500.00F < orderPrice && orderPrice >= 1000.00F)
            //{
            //    //	0.500
            //}
            //if (200.00F < orderPrice && orderPrice >= 500.00F)
            //{
            //    //	 0.200
            //}
            //if (100.00F < orderPrice && orderPrice >= 200.00F)
            //{
            //    //	 0.100
            //}
            //if (20.00F < orderPrice && orderPrice >= 100.00F)
            //{
            //    //	 0.050
            //}
            //if (10.00F < orderPrice && orderPrice >= 20.00F)
            //{
            //    //	0.020
            //}
            //if (0.50F < orderPrice && orderPrice >= 10.00F)
            //{
            //    //	0.010
            //}
            //if (0.25F < orderPrice && orderPrice >= 0.50F)
            //{
            //    //0.005
            //}
            //if (0.01F < orderPrice && orderPrice >= 0.25F)
            //{
            //    //是否满足最小变动价位
            //    if (orderPrice % 0.001 == 0)
            //    {
            //        strMessage = "";
            //        return true;
            //    }
            //}
            #endregion

            decimal orderPrice = (decimal)request.OrderPrice;
            foreach (HK_MinPriceFieldRange item in MinChangePriceValue)
            {
                //价位（靠后不靠前）（港元）
                if (item.LowerLimit.Value < orderPrice && orderPrice <= item.UpperLimit.Value)
                {
                    if (orderPrice % item.Value == 0)
                    {
                        strMessage = "";
                        return true;
                    }
                }
            }

            #region old code
            //int? min = TradeRules.ValueTypeMinChangePrice;
            //if (!min.HasValue)
            //{
            //    return false;
            //}

            //int minValueType = min.Value;
            //decimal orderPrice = (decimal)request.OrderPrice;
            //switch (minValueType)
            //{
            //    case (int)Types.GetValueTypeEnum.Single:
            //        decimal? minSingle = TradeRules.MinChangePrice;
            //        if (minSingle.HasValue)
            //        {
            //            decimal minSingleValue = minSingle.Value;

            //            //是否满足最小变动价位
            //            if (orderPrice % minSingleValue == 0)
            //            {
            //                result = true;
            //                strMessage = "";
            //            }
            //        }

            //        break;
            //    case (int)Types.GetValueTypeEnum.Scope:
            //        foreach (XH_MinChangePriceValue changePriceValue in MinChangePriceValue)
            //        {
            //            if (changePriceValue.Value.HasValue)
            //            {
            //                decimal fValue = changePriceValue.Value.Value;
            //                CM_FieldRange fieldRange = MCService.GetFieldRange(changePriceValue.FieldRangeID);

            //                //是否在当前字段范围内
            //                result = MCService.CheckFieldRange(orderPrice, fieldRange);
            //                if (result)
            //                {
            //                    //是否满足最小变动价位
            //                    if (orderPrice % fValue == 0)
            //                    {
            //                        strMessage = "";

            //                        return true;
            //                    }
            //                }
            //            }
            //        }
            //        break;
            //}
            #endregion

            return result;
        }

        #endregion


    }
    #endregion

    #region  4.港股_品种_涨跌幅校验命令(有效报价)
    /// <summary>
    /// 4.港股_品种_涨跌幅校验命令(有效报价)
    /// </summary>
    public class HKStockSpotHighLowCommand : HKStockValidateCommand
    {
        public HKStockSpotHighLowCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        #region Overrides of ValidateCommand<HKOrderRequest>

        public override bool Validate(HKOrderRequest request, ref string strMessage)
        {
            //根据要求，柜台不再做涨跌幅判断，由撮合处理
            return true;

            #region 根据要求，柜台不再做涨跌幅判断，由撮合处理 以下注释
            //string errMsg = "港股涨跌幅检验失败！";
            //string errCode = "GT-1253";

            //decimal orderPrice = (decimal)request.OrderPrice;

            //decimal orderPriceD = (decimal)request.OrderPrice;
            //HighLowRangeValue value = MCService.HLRangeProcessor.GetHKStockHighLowRangeValueByCommodityCode(request.Code, orderPriceD, request.OrderWay, request.BuySell);
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
            #endregion
        }
        #endregion
    }
    #endregion
    #endregion
}
