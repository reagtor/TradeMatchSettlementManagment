#region Using Namespace

using System;
using System.Linq;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.HK;
using System.Collections.Generic;
using ReckoningCounter.DAL.HKTradingRulesService;
using CommonObject = GTA.VTS.Common.CommonObject;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 描述：港股改单品种检验交易规则，错误码范围2250-2299
    /// 作者：李健华
    /// 日期：2009-11-10
    /// </summary>
    public class HKModifyOrderRuleContainer
    {
        private int BreedClassID;

        /// <summary>
        /// 根据改单请求和改单类型，改单委托的买卖方向，检验相关验证方法
        /// </summary>
        /// <param name="hkModifyOrder"></param>
        /// <param name="modifyType"></param>
        /// <param name="entrusInfo"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public bool Validate(HKModifyOrderRequest hkModifyOrder, HK_TodayEntrustInfo entrusInfo, int breedClassID,
                             ref string strMessage)
        {
            BreedClassID = breedClassID;
            bool reust = false;
            int modifyType = 0; //改单类型

            #region 判断改单类型

            //未注报
            if (entrusInfo.OrderStatusID == (int)Types.OrderStateType.DOSUnRequired)
            {
                modifyType = 1;
            }


            //已报、部成
            if (entrusInfo.OrderStatusID == (int)Types.OrderStateType.DOSIsRequired ||
                entrusInfo.OrderStatusID == (int)Types.OrderStateType.DOSPartDealed)
            {
                if ((decimal)hkModifyOrder.OrderPrice == entrusInfo.EntrustPrice &&
                    hkModifyOrder.OrderAmount < entrusInfo.EntrustAmount)
                {
                    modifyType = 2;
                }
                else
                {
                    modifyType = 3;
                }
            }

            #endregion

            if (modifyType == 0)
            {
                strMessage = "GT-2250:[港股改单委托验证]当前委托无法改单";
                return false;
            }

            #region 为适用之前的方法组装下单请求实体

            HKOrderRequest request = new HKOrderRequest();
            request.Code = hkModifyOrder.Code;
            request.ChannelID = hkModifyOrder.ChannelID;
            request.BuySell = (CommonObject.Types.TransactionDirection)entrusInfo.BuySellTypeID;
            request.OrderAmount = hkModifyOrder.OrderAmount;
            request.OrderPrice = hkModifyOrder.OrderPrice;
            request.FundAccountId = entrusInfo.CapitalAccount;
            request.OrderUnitType = (GTA.VTS.Common.CommonObject.Types.UnitType)entrusInfo.TradeUnitID;
            request.OrderWay = (GTA.VTS.Common.CommonObject.Types.HKPriceType)entrusInfo.OrderPriceType;

            #endregion

            #region 1.零股检验

            //零股检验
            HKStockZeroVolumeOfBusinessCommand zerovolume = new HKStockZeroVolumeOfBusinessCommand(BreedClassID);
            reust = zerovolume.Validate(request, ref strMessage);
            if (!reust)
            {
                return false;
            }

            #endregion

            #region 2.单笔最大委托量检验命令

            //单笔最大委托量检验命令
            HKStockMaxLeaveQuantityRangeValueCommand maxLeave =
                new HKStockMaxLeaveQuantityRangeValueCommand(BreedClassID);
            reust = maxLeave.Validate(request, ref strMessage);
            if (!reust)
            {
                return false;
            }

            #endregion

            #region 3.不是第二种类型量减的并且价格有改动的验证最小变动价格

            //不是第二种类型量减的并且价格有改动的验证最小变动价格
            if (modifyType != 2 && (decimal)hkModifyOrder.OrderPrice != entrusInfo.EntrustPrice)
            {
                HKStockMinChangePriceValueCommand minChange = new HKStockMinChangePriceValueCommand(BreedClassID);
                reust = minChange.Validate(request, ref strMessage);
                if (!reust)
                {
                    return false;
                }
            }

            #endregion

            #region 委托量有所改变则验证相关持仓(宋涛1116：此处不需要进行校验，等待原委托撤单成功后，由新单校验处检验)

            ////if (modifyType != 2)
            //{
            //if (hkModifyOrder.OrderAmount != entrusInfo.EntrustAmount)
            //{
            //    if (entrusInfo.BuySellTypeID == (int)Types.TransactionDirection.Buying)
            //    {
            //        reust = PO_HoldValidate_Buy(ref strMessage, entrusInfo, request);
            //    }
            //    else
            //    {
            //        reust = PO_HoldValidate_Sell(ref strMessage, entrusInfo, request);
            //    }
            //    if (!reust)
            //    {
            //        return false;
            //    }
            //}
            ////}

            //不考虑持仓量，只考虑委托最小单位的验证
            HKModifyStockMinVolumeOfBusinessCommand minVolume = new HKModifyStockMinVolumeOfBusinessCommand(BreedClassID);
            reust = minVolume.Validate(request, ref strMessage);
            if (!reust)
            {
                return false;
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 买持仓检查
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="hkEntrustInfo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool PO_HoldValidate_Buy(ref string strMessage, HK_TodayEntrustInfo hkEntrustInfo,
                                         HKOrderRequest request)
        {
            bool result = false;
            strMessage = "";

            int position = 0;
            decimal freezeAmount = 0;

            //var ahtMemory = MemoryDataManager.HKHoldMemoryList.GetByAccountHoldLogoId(HoldingAccountId);
            //if (ahtMemory == null)
            //{
            var ahtMemory = HKCommonLogic.GetHoldMemoryTable(hkEntrustInfo.HoldAccount, hkEntrustInfo.Code,
                                                             hkEntrustInfo.CurrencyTypeID);
            //}

            if (ahtMemory != null && ahtMemory.Data != null)
            {
                var aht = ahtMemory.Data;

                position = Convert.ToInt32(aht.AvailableAmount);
                freezeAmount = aht.FreezeAmount;
            }
            else
            {
                position = 0;
                freezeAmount = 0;
            }


            HKStockMinVolumeOfBusinessCommand command = new HKStockMinVolumeOfBusinessCommand(BreedClassID, position);
            result = command.Validate(request, ref strMessage);
            // if (ValidateCenter.ValidateHKMinVolumeOfBusiness(request, position, ref strMessage))
            if (result)
            {
                //获取持仓限制
                Decimal pLimit = MCService.GetPositionLimit(request.Code, CommonObject.Types.BreedClassTypeEnum.HKStock).PositionValue;
                //可用持仓+冻结量+委托量<持仓限制            //这里是改单，所以要把之前的委托减去
                result = pLimit >=
                         position + freezeAmount + Convert.ToDecimal(request.OrderAmount - hkEntrustInfo.EntrustAmount);
                if (!result)
                {
                    strMessage = "GT-2419:[港股改单委托验证]可用持仓+冻结量+委托量<持仓限制--港股改单委托失败";
                }
            }
            else
            {
                strMessage += "--港股改单委托失败";
            }


            //成功时需要清空错误信息。
            if (result)
                strMessage = "";

            return result;
        }

        /// <summary>
        /// 卖持仓检查
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="hkEntrustInfo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool PO_HoldValidate_Sell(ref string strMessage, HK_TodayEntrustInfo hkEntrustInfo,
                                          HKOrderRequest request)
        {
            bool result = false;

            strMessage = "GT-2420:[港股改单委托检验]卖持仓检查,无持仓--港股改单委托失败";

            //var ahtMemory = MemoryDataManager.HKHoldMemoryList.GetByHoldAccountAndCurrencyType(HoldingAccount, Code,
            //                                                                                   CurrencyType);

            //if (ahtMemory == null)
            //{
            var ahtMemory = HKCommonLogic.GetHoldMemoryTable(hkEntrustInfo.HoldAccount, hkEntrustInfo.Code,
                                                             hkEntrustInfo.CurrencyTypeID);
            //}

            if (ahtMemory == null || ahtMemory.Data == null)
            {
                return false;
            }

            var aht = ahtMemory.Data;

            int position = Convert.ToInt32(aht.AvailableAmount);
            //真正的可用量是要减去真正的改单量增加或者减少量
            //而真正的改单量增加量为=现在改单委托量-原委托量;
            int nowAddAmount = Convert.ToInt32(request.OrderAmount) - hkEntrustInfo.EntrustAmount;
            //不能直接把冻结量加上，因为冻结量可能不为本单的冻结量，可能还有别的单下的冻结量
            position = position + hkEntrustInfo.EntrustAmount - hkEntrustInfo.TradeAmount;
            //则当前的真正的可用持仓量为
            position = position - nowAddAmount;

            //持仓帐户是否存在判断
            //result = ValidateCenter.ValidateHKMinVolumeOfBusiness(request, position, ref strMessage);
            HKStockMinVolumeOfBusinessCommand command = new HKStockMinVolumeOfBusinessCommand(BreedClassID, position);
            result = command.Validate(request, ref strMessage);
            if (!result)
            {
                strMessage += "--港股改单委托失败";
            }

            if (result)
            {
                strMessage = "";
            }

            return result;
        }
    }
    #region  1.最小交易单位检验命令
    /// <summary>
    /// 1.最小交易单位检验命令
    /// </summary>
    public class HKModifyStockMinVolumeOfBusinessCommand : HKStockValidateCommand
    {
        /// <summary>
        /// 交易规则_交易方向_交易单位_交易量(最小交易单位)集合（此中的集合用原来现货中的表数据）
        /// </summary>
        private IList<ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness> m_MinVolumeOfBusinessList;



        public HKModifyStockMinVolumeOfBusinessCommand(int BreedClassID)
            : base(BreedClassID)
        {
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
                if (item.UnitID == (int)CommonObject.Types.UnitType.Thigh)
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
                itembuy.UnitID = (int)CommonObject.Types.UnitType.Thigh;
                itembuy.VolumeOfBusiness = hkComm.PerHandThighOrShare;
                itembuy.TradeWayID = (int)CommonObject.Types.TransactionDirection.Buying;
                minVolumesList.Add(itembuy);

                ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness itemSell = new ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness();
                itemSell.UnitID = (int)CommonObject.Types.UnitType.Thigh;
                itemSell.VolumeOfBusiness = hkComm.PerHandThighOrShare;
                itemSell.TradeWayID = (int)CommonObject.Types.TransactionDirection.Selling;
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

            //if (request.BuySell == CommonObject.Types.TransactionDirection.Buying)
            //{
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
            #region 目前只验证所有的改单不分卖买验证，其他的都作为下单后验证
            //}
            ////卖的话需要进行零股处理
            //else
            //{
            //    //委托单位转换为计价单位
            //    decimal scale = MCService.GetTradeUnitScale(CommonObject.Types.BreedClassTypeEnum.HKStock, request.Code, request.OrderUnitType);
            //    var amount = (decimal)request.OrderAmount * scale;


            //    //持仓单位(默认为撮合单位)转为计价单位
            //    decimal scale2 = MCService.GetMatchUnitScale(CommonObject.Types.BreedClassTypeEnum.HKStock, request.Code);
            //    var position = m_Position * scale2;

            //    if (amount > position)
            //    {
            //        strMessage = errCode + ":" + "超过可用持仓";
            //        return false;
            //    }

            //    //港股最小单位零股检验
            //    foreach (ReckoningCounter.DAL.SpotTradingDevolveService.XH_MinVolumeOfBusiness volume in minVolumes)
            //    {
            //        if ((int)request.OrderUnitType == volume.UnitID)
            //        {
            //            if (request.OrderAmount < volume.VolumeOfBusiness.Value ||
            //                (request.OrderAmount % volume.VolumeOfBusiness.Value != 0))
            //            {
            //                if (amount != position)
            //                {
            //                    strMessage = errCode + ":" + errMsg;
            //                    return false;
            //                }
            //            }
            //        }
            //    }

            //}
            #endregion

            return true;
        }

        #endregion
    }
    #endregion
}