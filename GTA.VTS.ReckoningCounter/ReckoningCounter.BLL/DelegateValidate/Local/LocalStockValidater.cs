#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.Local
{
    /// <summary>
    /// 证券买卖委托内部校验，错误码范围1200-1299
    /// 资金与持仓检查不再在这里做，在下单时会重复计算
    /// 作者：宋涛
    /// 日期：2008-11-24
    /// </summary>
    public class LocalStockValidater : LocalCommonValidater
    {
        private int currencyTypeID;
        private StockOrderRequest request;

        public LocalStockValidater(StockOrderRequest request)
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
            bool result = Initialize(request.TraderId, request.Code, out errMsg);

            //检查品种交易权限
            //if(result)
            //{
            //    result = CheckTradeRight(out errMsg);
            //}

            //基础验证，包括商品的基础验证，账户的基础验证
            if (result)
            {
                result = CheckTraderAvalible(out errMsg);
            }

            ////检查资金
            //if(result)
            //{
            //    result = CheckFuncTable(ref errMsg);
            //}

            ////检查持仓
            //if (result)
            //{
            //    result = CheckHoldTable(ref errMsg);
            //}

            //如果是在交易时间内，那么检查停牌，连续3次，预下单不检查
            if (ValidateCenter.IsMatchTradingTime(request.Code))
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
                currencyTypeID =
                    MCService.SpotTradeRules.GetCurrencyTypeByBreedClassID(breedClass.BreedClassID).
                        CurrencyTypeID;
            }


            return result;
        }

        /// <summary>
        /// 检查交易员_现货资金交易账户表(UA_UserAccountAllocationTable)是否可用
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
                        result = true;
                    else
                    {
                        errMsg = "交易员现货资金账户不可用。";
                        errCode = "GT-1200";
                        errMsg = errCode + ":" + errMsg;

                        LogHelper.WriteInfo(errMsg);
                    }
                }
                else
                {
                    errMsg = "交易员现货资金账户不存在。";
                    errCode = "GT-1201";
                    errMsg = errCode + ":" + errMsg;
                    LogHelper.WriteInfo(errMsg);
                }
            }
            catch (Exception ex)
            {
                errMsg = "无法进行交易员现货资金账户有效性验证。";
                errCode = "GT-1202";
                errMsg = errCode + ":" + errMsg;

                LogHelper.WriteError(errMsg, ex);
            }


            return result;
        }

        /// <summary>
        /// 检查交易员_现货持仓交易账户表(UA_UserAccountAllocationTable)是否可用
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
                        errMsg = "交易员现货持仓账户不可用。";
                        errCode = "GT-1203";
                        errMsg = errCode + ":" + errMsg;

                        LogHelper.WriteInfo(errMsg);
                    }
                }
                else
                {
                    errMsg = "交易员现货持仓账户不存在。";
                    errCode = "GT-1204";
                    errMsg = errCode + ":" + errMsg;
                    LogHelper.WriteInfo(errMsg);
                }
            }
            catch (Exception ex)
            {
                errMsg = "无法进行交易员现货持仓账户有效性验证。";
                errCode = "GT-1205";
                errMsg = errCode + ":" + errMsg;

                LogHelper.WriteError(errMsg, ex);
            }


            return result;
        }

        /// <summary>
        /// 检测是否停牌，如果没有停牌，代表校验通过
        /// 当获取不到行情表示停牌，当买一价、量，卖一价、量都为0时也表示停牌
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckStopTrading(ref string errMsg)
        {
            errMsg = "当前交易股票" + request.Code + "可能停牌，委托无效。";
            string errCode = "GT-1206";
            errMsg = errCode + ":" + errMsg;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            var hqdata = service.GetStockHqData(request.Code);
            if (hqdata == null)
                return false;

            var hqExData = hqdata;
            if (hqExData == null)
                return false;

            var hqData = hqExData.HqData;
            if (hqData == null)
                return false;

            if (hqData.Buyprice1 == 0 && hqData.Sellprice1 == 0 && hqData.Buyvol1 == 0 && hqData.Sellvol1 == 0)
                return false;

            errMsg = "";
            return true;
        }

        /// <summary>
        /// 检测交易员的品种交易权限
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private  bool CheckTradeRight(out string errMsg)
        {
            errMsg = "";
            try
            {
                IList<UM_DealerTradeBreedClass> list =
                MCService.CommonPara.GetTransactionRightTable(int.Parse(this.request.TraderId));
                CM_Commodity code = MCService.CommonPara.GetCommodityByCommodityCode(this.request.Code);

                foreach (UM_DealerTradeBreedClass tradeBreedClass in list)
                {
                    if ((int)tradeBreedClass.BreedClassID == (int)code.BreedClassID)
                    {
                        return true;
                    }
                }
                errMsg = "交易员没有该品种的交易权限！";
            }
            catch (Exception ex)
            {
                errMsg = "无法进行交易品种权限有效性验证。";
                string errCode = "GT-1207";
                errMsg = errCode + ":" + errMsg;
                LogHelper.WriteError(errMsg, ex);
            }
            return false;
        }


        #region 资金与持仓检查不再在本地校验做，在下单计算时顺便处理，避免重复计算

        /*
        /// <summary>
        /// 检测证券资金表是否正常
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckFuncTable(ref string errMsg)
        {
            bool result = false;
            errMsg = "";


            TList<XhCapitalAccountTable> list =
                DataRepository.XhCapitalAccountTableProvider.GetByUserAccountDistributeLogo(request.TraderId);
            foreach (XhCapitalAccountTable xhCapitalAccountTable in list)
            {
                if (xhCapitalAccountTable.TradeCurrencyType.Value == this.currencyTypeID)
                {
                    if (xhCapitalAccountTable.AvailableCapital > 0)
                        result = true;

                    break;
                }
            }

            ////查找现货对应币种的资金帐号
            //TList<XhCapitalAccountTable> cptAccount =
            //    DataRepository.XhCapitalAccountTableProvider.Find(string.Format("TradeCurrencyType={0} and UserAccountDistributeLogo='{1}'", iType, capitalAccount));


            return result;
        }

        /// <summary>
        /// 检测证券持仓表是否正常
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckHoldTable(ref string errMsg)
        {
            bool result = false;
            errMsg = "";

            if (request.buysell == Types.TransactionDirection.Selling)
            {
                TList<XhAccountHoldTable> list =
                    DataRepository.XhAccountHoldTableProvider.GetByUserAccountDistributeLogo(request.TraderId);
                foreach (XhAccountHoldTable xhHoldAccountTable in list)
                {
                    //if(xhHoldAccountTable.)
                }
            }
            else
            {
                result = true;
            }


            return result;
        }
         * */

        #endregion
    }
}