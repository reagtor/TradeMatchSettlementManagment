#region Using Namespace

using System;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.Entity;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.Local
{
    /// <summary>
    /// 期货开仓买卖委托内部校验，错误码范围1300-1399
    /// 作者：宋涛
    /// 日期：2008-11-24
    /// </summary>
    public class LocalFutureOpenValidater : LocalCommonValidater
    {
        private Types.TransactionDirection buysell;
        private string code;
        private int currencyTypeID;
        private string traderId;
        //private MercantileFuturesOrderRequest request;

        public LocalFutureOpenValidater(MercantileFuturesOrderRequest request)
        {
            //this.request = request;

            this.traderId = request.TraderId;
            this.code = request.Code;
            this.buysell = request.BuySell;
        }

        public LocalFutureOpenValidater(StockIndexFuturesOrderRequest request)
        {
            //this.request = request;

            this.traderId = request.TraderId;
            this.code = request.Code;
            this.buysell = request.BuySell;
        }

        /// <summary>
        /// 校验入口
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="breedType">品种类型（这里是因为这里两种获取的行情实体方法有区别）</param>
        /// <returns>是否成功</returns>
        public bool Check(out string errMsg, Types.BreedClassTypeEnum breedType)
        {
            bool result = Initialize(traderId, code, out errMsg);

            //基础验证，包括商品的基础验证，账户的基础验证
            if (result)
            {
                result = CheckTraderAvalible(out errMsg);
            }

            //检查资金
            //if (result)
            //{
            //    result = CheckFuncTable(ref errMsg);
            //}

            ////检查持仓
            //if (result)
            //{
            //    result = CheckHoldTable(ref errMsg);
            //}

            //如果是在交易时间内，那么检查停牌，连续3次，预下单不检查
            if (ValidateCenter.IsMatchTradingTime(code))
            {
                if (result)
                {
                    result = CheckStopTrading(ref errMsg, breedType);

                    if (!result)
                        result = CheckStopTrading(ref errMsg, breedType);

                    if (!result)
                        result = CheckStopTrading(ref errMsg, breedType);
                }
            }

            return result;
        }


        /// <summary>
        /// 检查交易账户是否有效
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        private bool CheckTraderAvalible(out string errMsg)
        {
            bool result = CheckFuncTraderAvalible(this.accountFundTypeId, out errMsg);

            if (result)
            {
                if (buysell == Types.TransactionDirection.Selling)
                {
                    result = CheckHoldTraderAvalible(this.accountHoldTypeId, out errMsg);
                }
            }


            if (result)
            {
                var currencyType = MCService.FuturesTradeRules.GetCurrencyTypeByBreedClassID(breedClass.BreedClassID);
                // update by 董鹏 2010-04-06 增加判断，避免currencyType为null时抛出异常
                if (currencyType != null)
                {
                    currencyTypeID = currencyType.CurrencyTypeID;
                }
            }


            return result;
        }

        /// <summary>
        /// 检查交易员_期货资金交易账户表(UA_UserAccountAllocationTable)是否可用
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
                UA_UserAccountAllocationTableInfo val = GetUserAccountAllocationTable(traderId, accTypeID);

                if (val != null)
                {
                    if (val.WhetherAvailable)
                        result = true;
                    else
                    {
                        errMsg = "交易员期货资金账户不可用[" + traderId + "]";
                        errCode = "GT-1300";
                        errMsg = errCode + ":" + errMsg;

                        LogHelper.WriteInfo(errMsg);
                    }
                }
                else
                {
                    errMsg = "交易员期货资金账户不存在[" + traderId + "]";
                    errCode = "GT-1301";
                    errMsg = errCode + ":" + errMsg;
                    LogHelper.WriteInfo(errMsg);
                }
            }
            catch (Exception ex)
            {
                errMsg = "无法进行交易员期货资金账户有效性验证[" + traderId + "]";
                errCode = "GT-1302";
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
                UA_UserAccountAllocationTableInfo val = GetUserAccountAllocationTable(traderId, accTypeID);

                if (val != null)
                {
                    if (val.WhetherAvailable)
                        result = true;
                    else
                    {
                        errMsg = "交易员期货持仓账户不可用。";
                        errCode = "GT-1303";
                        errMsg = errCode + ":" + errMsg;

                        LogHelper.WriteInfo(errMsg);
                    }
                }
                else
                {
                    errMsg = "交易员期货持仓账户不存在。";
                    errCode = "GT-1304";
                    errMsg = errCode + ":" + errMsg;
                    LogHelper.WriteInfo(errMsg);
                }
            }
            catch (Exception ex)
            {
                errMsg = "无法进行交易员期货持仓账户有效性验证。";
                errCode = "GT-1305";
                errMsg = errCode + ":" + errMsg;

                LogHelper.WriteError(errMsg, ex);
            }


            return result;
        }

        /// <summary>
        /// 检测是否停牌
        /// 当获取不到行情表示停牌，当买一价、量，卖一价、量都为0时也表示停牌
        /// </summary>
        /// <param name="errMsg"></param>
        /// <param name="breedType">品种类型（这里是因为这里两种获取的行情实体方法有区别）</param>
        /// <returns></returns>
        private bool CheckStopTrading(ref string errMsg, Types.BreedClassTypeEnum breedType)
        {
            errMsg = "当前交易商品" + code + "可能停牌，委托无效。";
            string errCode = "GT-1206";
            errMsg = errCode + ":" + errMsg;

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();

            switch (breedType)
            {
                case Types.BreedClassTypeEnum.Stock:
                    break;
                case Types.BreedClassTypeEnum.CommodityFuture:
                    var hqdata = service.GetMercantileFutData(code);
                    if (hqdata == null)
                    {
                        return false;
                    }
                    if (hqdata.Buyprice1 == 0 && hqdata.Sellprice1 == 0 && hqdata.Buyvol1 == 0 && hqdata.Sellvol1 == 0)
                        return false;
                    break;
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    var futdata = service.GetFutData(code);
                    if (futdata == null)
                    {
                        return false;
                    }

                    if (futdata.Buyprice1 == 0 && futdata.Sellprice1 == 0 && futdata.Buyvol1 == 0 && futdata.Sellvol1 == 0)
                    {
                        return false;
                    }
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    break;
                default:
                    break;
            }




            return true;
        }

        #region 资金与持仓检查不再在本地校验做，在下单计算时顺便处理，避免重复计算

        /*
        
        /// <summary>
        /// 检测期货资金表是否正常
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckFuncTable(ref string errMsg)
        {
            bool result = false;
            errMsg = "";

            TList<QhCapitalAccountTable> list =
                DataRepository.QhCapitalAccountTableProvider.GetByUserAccountDistributeLogo(traderId);
            foreach (QhCapitalAccountTable qhCapitalAccountTable in list)
            {
                 if(qhCapitalAccountTable.TradeCurrencyType.Value == this.currencyTypeID)
                 {
                     if(qhCapitalAccountTable.AvailableCapital > 0)
                         result = true;

                     break;
                 }
            }

            return result;
        }

        /// <summary>
        /// 检测期货持仓表是否正常
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool CheckHoldTable(ref string errMsg)
        {
            bool result = false;
            errMsg = "";

            if (buysell == Types.TransactionDirection.Selling)
            {
                TList<QhHoldAccountTable> list =
                    DataRepository.QhHoldAccountTableProvider.GetByUserAccountDistributeLogo(traderId);
                foreach (QhHoldAccountTable qhHoldAccountTable in list)
                {
                    
                }
            }


            return result;
        }
        */

        #endregion
    }
}