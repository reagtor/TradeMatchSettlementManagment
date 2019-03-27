#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateValidate.ManagementCenter;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.Entity;

#endregion

//using ReckoningCounter.DAL.DevolveVerifyCommonService;

namespace ReckoningCounter.BLL.Delegatevalidate.ManagementCenter
{
    /// <summary>
    /// 描述：期货货品种检验交易规则，错误码范围1350-1399
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public class FutureRuleContainer : RuleContainer<MercantileFuturesOrderRequest>
    {
        public FutureRuleContainer(int breedClassid)
            : base(breedClassid)
        {
        }

        #region Overrides of RuleContainer<MercantileFuturesOrderRequest>

        protected override void FillCommandList()
        {
            //0.交易员权限--交易员是否有此品种的交易权限
            FuturesTraderRightsCommand futuresTraderRightsCommand = new FuturesTraderRightsCommand(this.BreedClassID);
            AddCommand(futuresTraderRightsCommand);

            // 1.最小、大委托量检验命令
            FuturesMaxLeaveQuantityRangeValueCommand futuresMaxLeaveQuantityRangeValueCommand =
                new FuturesMaxLeaveQuantityRangeValueCommand(this.BreedClassID);
            AddCommand(futuresMaxLeaveQuantityRangeValueCommand);

            // 2.最小变动价位检验命令
            FuturesMinChangePriceValueCommand futuresMinChangePriceValueCommand = new FuturesMinChangePriceValueCommand(this.BreedClassID);
            AddCommand(futuresMinChangePriceValueCommand);

            //3.验证最小倍数(内部处理如果股指期货不用验证，这是为了适应于之前的方法),把此方法设置在此是因为适用于盘前检查时不用验证最小倍数据这个规则
            //所以设置在这里，而不象持仓限制
            // 2.最小变动价位检验命令
            FuturesMinMultipleValueCommand futuresMinMultipleValueCommand = new FuturesMinMultipleValueCommand(this.BreedClassID);
            AddCommand(futuresMinMultipleValueCommand);
        }

        #endregion
    }

    #region Validate Commands

    /// <summary>
    /// 期货验证命令的基类
    /// </summary>
    public abstract class FutureValidateCommand : ValidateCommand<MercantileFuturesOrderRequest>
    {
        /// <summary>
        /// 期货_品种_交易规则
        /// </summary>
        private QH_FuturesTradeRules m_FutureTradeRules;

        protected FutureValidateCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        /// <summary>
        /// 期货_品种_交易规则
        /// </summary>
        protected QH_FuturesTradeRules TradeRules
        {
            get
            {
                if (m_FutureTradeRules == null)
                    m_FutureTradeRules = MCService.FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(BreedClassID);
                return m_FutureTradeRules;
            }
        }
    }


    /// <summary>
    /// 0.交易员权限--交易员是否有此品种的交易权限
    /// </summary>
    public class FuturesTraderRightsCommand : FutureValidateCommand
    {
        public FuturesTraderRightsCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        public override bool Validate(MercantileFuturesOrderRequest request, ref string strMessage)
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
    /// 1.最小、大委托量检验命令
    /// </summary>
    public class FuturesMaxLeaveQuantityRangeValueCommand : FutureValidateCommand
    {
        /// <summary>
        /// 交易规则委托量
        /// </summary>
        private QH_ConsignQuantum consignQuantum;

        /// <summary>
        /// 单笔委托量
        /// </summary>
        private IList<QH_SingleRequestQuantity> singleRequestQuantityList;

        public FuturesMaxLeaveQuantityRangeValueCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        #region Overrides of ValidateCommand<StockOrderRequest>

        public override bool Validate(MercantileFuturesOrderRequest request, ref string strMessage)
        {
            //bool result = false;

            string errMsg = "期货最小委托量检验失败！";
            string errCode = "GT-1351";
            strMessage = errCode + ":" + errMsg;

            int? consignQuantumID = TradeRules.ConsignQuantumID;
            if (!consignQuantumID.HasValue)
                return false;

            if (consignQuantum == null)
                consignQuantum = MCService.FuturesTradeRules.GetConsignQuantumByConsignQuantumID(consignQuantumID.Value);

            if (consignQuantum == null)
                return false;

            if (!consignQuantum.MinConsignQuantum.HasValue)
                return false;

            Types.UnitType marketUnitType;
            try
            {
                marketUnitType = (Types.UnitType)TradeRules.MarketUnitID;
                ;
            }
            catch
            {
                return false;
            }

            decimal scale = MCService.GetTradeUnitScale(request.Code, request.OrderUnitType);
            decimal scale2 = MCService.GetTradeUnitScale(request.Code, marketUnitType);

            var orderAmount = (decimal)request.OrderAmount * scale;
            var minAmount = consignQuantum.MinConsignQuantum.Value * scale2;

            //进行最小委托量校验
            if (orderAmount < minAmount)
                return false;
            //最小委托量整数倍验证
            if (orderAmount % minAmount != 0)
            {
                return false;
            }

            string errMsg2 = "超过当前期货单笔最大委托量！";
            string errCode2 = "GT-1352";
            strMessage = errCode2 + ":" + errMsg2;

            singleRequestQuantityList =
                MCService.FuturesTradeRules.GetSingleRequestQuantityByConsignQuantumID(consignQuantumID.Value);

            QH_SingleRequestQuantity limit_SingleRequestQuantity = null;
            QH_SingleRequestQuantity market_SingleRequestQuantity = null;

            foreach (QH_SingleRequestQuantity singleRequestQuantity in singleRequestQuantityList)
            {
                if (singleRequestQuantity.ConsignInstructionTypeID.HasValue)
                {
                    int val = singleRequestQuantity.ConsignInstructionTypeID.Value;
                    if (val == (int)Entity.Contants.Types.OrderPriceType.OPTLimited)
                        limit_SingleRequestQuantity = singleRequestQuantity;
                    else if (val == (int)Entity.Contants.Types.OrderPriceType.OPTMarketPrice)
                        market_SingleRequestQuantity = singleRequestQuantity;
                }
            }

            //进行最大委托量校验
            switch (request.OrderWay)
            {
                case Entity.Contants.Types.OrderPriceType.OPTLimited:
                    if (limit_SingleRequestQuantity == null)
                        return false;
                    if (!limit_SingleRequestQuantity.MaxConsignQuanturm.HasValue)
                        return false;
                    var maxLimitAmount = limit_SingleRequestQuantity.MaxConsignQuanturm.Value * scale2;
                    if (orderAmount > maxLimitAmount)
                        return false;

                    break;

                case Entity.Contants.Types.OrderPriceType.OPTMarketPrice:
                    if (market_SingleRequestQuantity == null)
                        return false;
                    if (!market_SingleRequestQuantity.MaxConsignQuanturm.HasValue)
                        return false;
                    var maxMarketAmount = market_SingleRequestQuantity.MaxConsignQuanturm.Value * scale2;
                    if (orderAmount > maxMarketAmount)
                        return false;
                    break;
            }

            strMessage = "";
            return true;
        }

        #endregion
    }

    /// <summary>
    /// 2.最小变动价位检验命令
    /// </summary>
    public class FuturesMinChangePriceValueCommand : FutureValidateCommand
    {
        public FuturesMinChangePriceValueCommand(int breedClassID)
            : base(breedClassID)
        {
        }

        #region Overrides of ValidateCommand<StockOrderRequest>

        public override bool Validate(MercantileFuturesOrderRequest request, ref string strMessage)
        {
            bool result = false;

            string errMsg = "期货最小变动价位检验失败！";
            string errCode = "GT-1353";
            strMessage = errCode + ":" + errMsg;

            decimal? min = TradeRules.LeastChangePrice;
            if (!min.HasValue)
            {
                return false;
            }

            decimal minPrice = min.Value;
            decimal orderPrice = (decimal)request.OrderPrice;
            //是否满足最小变动价位
            if (orderPrice % minPrice == 0)
            {
                result = true;
                strMessage = "";
            }

            return result;
        }

        #endregion


    }

    /// <summary>
    /// 验证委托量最小倍数
    /// </summary>
    public class FuturesMinMultipleValueCommand : FutureValidateCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="breedClassID"></param>
        public FuturesMinMultipleValueCommand(int breedClassID)
            : base(breedClassID)
        {

        }
        /// <summary>
        /// 重写实现验证委托量最小倍数方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public override bool Validate(MercantileFuturesOrderRequest request, ref string strMessage)
        {
            //如果是开仓返回true，直接验证通过，因为开仓的时候已经在内部有处理
            if (request.OpenCloseType == ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType.OpenPosition)
            {
                return true;
            }

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByBreedClassID(BreedClassID);

            bool result = true;

            if (breedClass != null && breedClass.BreedClassTypeID.HasValue)
            {
                int breedClassTypeID = breedClass.BreedClassTypeID.Value;
                int breedClassID = breedClass.BreedClassID;
                //如果是股指期货直接返回true
                if (breedClassTypeID == (int)Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    return true;
                }

                //如果不是商品期货返回验证不通过
                if (breedClassTypeID != (int)Types.BreedClassTypeEnum.CommodityFuture)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            PositionLimitValueInfo posInfo = MCService.GetPositionLimit(request.Code);
            //如果是最小交割单位的整数倍时
            if (posInfo.IsMinMultiple)
            {
                if (((decimal)request.OrderAmount) % posInfo.MinMultipleValue != 0)
                {
                    LogHelper.WriteDebug("===商品期货验证开仓最小交割单位倍数据： (request.OrderAmount % posInfo.MinMultipleValue)=(" + request.OrderAmount + " % " + posInfo.MinMultipleValue + ")=" + (((decimal)request.OrderAmount) % posInfo.MinMultipleValue));
                    strMessage = "GT-1354:[商品期货委托持久化]开仓持仓检查,持仓限制不是最小交割单位倍数";
                    result = false;
                }
            }

            return result;

        }
    }
    #endregion
}