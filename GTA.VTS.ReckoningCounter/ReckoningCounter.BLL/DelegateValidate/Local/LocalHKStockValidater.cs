using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.Entity;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.Model;
using GTA.VTS.Common.CommonUtility;
using CommonRealtimeMarket;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.HKTradingRulesService;
//using CommonRealtimeMarket.entity;
using RealTime.Server.SModelData.HqData;

namespace ReckoningCounter.BLL.DelegateValidate.Local
{
    /// <summary>
    /// 港股买卖委托内部校验，错误码范围1200-1299
    /// 资金与持仓检查不再在这里做，在下单时会重复计算
    /// Create By：李健华
    /// Create Date：2009-10-20
    /// </summary>
    public class LocalHKStockValidater : LocalCommonValidater
    {
        private int currencyTypeID;
        private HKOrderRequest request;

        public LocalHKStockValidater(HKOrderRequest request)
        {
            this.request = request;
        }


        /// <summary>
        /// 校验入口
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public bool Check(out string errMsg)
        {
            bool result = Initialize(Types.BreedClassTypeEnum.HKStock, request.TraderId, request.Code, out errMsg);

            //基础验证，包括商品的基础验证，账户的基础验证
            if (result)
            {
                result = CheckTraderAvalible(out errMsg);
            }

            //如果是在交易时间内，那么检查停牌，连续3次，预下单不检查
            if (ValidateCenter.IsMatchTradingTime(Types.BreedClassTypeEnum.HKStock, request.Code))
            {
                if (result)
                {
                    result = CheckStopTrading(ref errMsg);

                    if (!result)
                        result = CheckStopTrading(ref errMsg);

                    if (!result)
                        result = CheckStopTrading(ref errMsg);
                }
            }


            return result;
        }


        /// <summary>
        /// 检查交易账户是否有效
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckTraderAvalible(out string errMsg)
        {
            bool result = CheckFuncTraderAvalible(this.accountFundTypeId, out errMsg);

            if (result)
            {
                if (request.BuySell == Types.TransactionDirection.Selling)
                {
                    result = CheckHoldTraderAvalible(this.accountHoldTypeId, out errMsg);
                }
            }

            if (result)
            {
                HK_SpotCosts hkSpotCots = MCService.HKTradeRulesProxy.GetSpotCostsByBreedClassID(breedClass.BreedClassID);
                if (hkSpotCots != null)
                {
                    currencyTypeID = hkSpotCots.CurrencyTypeID.Value;
                }
                else
                {
                    errMsg = "GT-1207:无法获取港股交易费用实体，获取不到交易币种";
                    LogHelper.WriteInfo(errMsg);
                }
            }

            return result;
        }

        /// <summary>
        /// 检查交易员_港股资金交易账户表(UA_UserAccountAllocationTable)是否可用
        /// </summary>
        /// <param name="accTypeID"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        private bool CheckFuncTraderAvalible(int accTypeID, out string errMsg)
        {
            bool result = false;
            errMsg = "";
            string errCode = "";

            try
            {
                UA_UserAccountAllocationTableInfo val = GetUserAccountAllocationTable(request.TraderId, accTypeID);

                if (val != null)
                {
                    if (val.WhetherAvailable)
                    {
                        result = true;
                    }
                    else
                    {
                        errMsg = "交易员港股资金账户不可用。";
                        errCode = "GT-1200";
                        errMsg = errCode + ":" + errMsg;

                        LogHelper.WriteInfo(errMsg);
                    }
                }
                else
                {
                    errMsg = "交易员港股资金账户不存在。";
                    errCode = "GT-1201";
                    errMsg = errCode + ":" + errMsg;

                    LogHelper.WriteInfo(errMsg);
                }
            }
            catch (Exception ex)
            {
                errMsg = "无法进行交易员港股资金账户有效性验证。";
                errCode = "GT-1202";
                errMsg = errCode + ":" + errMsg;

                LogHelper.WriteError(errMsg, ex);
            }


            return result;
        }

        /// <summary>
        /// 检查交易员_港股持仓交易账户表(UA_UserAccountAllocationTable)是否可用
        /// </summary>
        /// <param name="accTypeID"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        private bool CheckHoldTraderAvalible(int accTypeID, out string errMsg)
        {
            bool result = false;
            errMsg = "";
            string errCode = "";

            try
            {
                UA_UserAccountAllocationTableInfo val = GetUserAccountAllocationTable(request.TraderId, accTypeID);

                if (val != null)
                {
                    if (val.WhetherAvailable)
                        result = true;
                    else
                    {
                        errMsg = "交易员港股持仓账户不可用。";
                        errCode = "GT-1203";
                        errMsg = errCode + ":" + errMsg;

                        LogHelper.WriteInfo(errMsg);
                    }
                }
                else
                {
                    errMsg = "交易员港股持仓账户不存在。";
                    errCode = "GT-1204";
                    errMsg = errCode + ":" + errMsg;
                    LogHelper.WriteInfo(errMsg);
                }
            }
            catch (Exception ex)
            {
                errMsg = "无法进行交易员港股持仓账户有效性验证。";
                errCode = "GT-1205";
                errMsg = errCode + ":" + errMsg;

                LogHelper.WriteError(errMsg, ex);
            }


            return result;
        }

        /// <summary>
        /// 检测是否停牌，如果没有停牌，代表校验通过
        /// 当获取不到行情盘前数据表示停牌，当行情盘前数据标识时为停牌则为停牌
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckStopTrading(ref string errMsg)
        {
            errMsg = "当前交易股票" + request.Code + "可能停牌，委托无效。";
            string errCode = "GT-1206";
            errMsg = errCode + ":" + errMsg;

            #region 旧逻辑，港股要以静态数据来判断
            //IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            //VTHKStockData hqdata = service.GetHKStockData(request.Code);
            //if (hqdata == null)
            //    return false;

            //HKStock hqExData = hqdata.OriginalData;
            //if (hqExData == null)
            //    return false;


            //if (hqExData.Buyprice1 == 0 && hqExData.Sellprice1 == 0 && hqExData.Buyvol1 == 0 && hqExData.Sellvol1 == 0)
            //    return false;
            #endregion

            #region 新逻辑

            //读行情组件静态信息列表
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService();
            HKStaticData hqdata = service.GetHKPreMarketData(request.Code);
            if (hqdata == null)
            {
                hqdata = service.GetHKPreMarketData(request.Code);
            }
            if (hqdata == null)
            {
                hqdata = service.GetHKPreMarketData(request.Code);
            }
            if (hqdata == null)
            {
                errMsg = errCode + ":" + "当前交易股票" + request.Code + "获取不到行情可能停牌，委托无效。";
                return false;
            }
            //如果停牌验证不通过
            if (hqdata.SuspensionFlag == true)
            {
                return false;
            }
            #endregion
            errMsg = "";
            return true;
        }



    }
}
