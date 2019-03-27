using System;
using System.Collections.Generic;
using System.Text;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;
using ReckoningCounter.DAL.AccountManagementAndFindDAL;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.Entity.Model.QueryFilter;
using ReckoningCounter.Entity.Model;
using ReckoningCounter.MemoryData;
using ReckoningCounter.MemoryData.QH.Capital;
using ReckoningCounter.MemoryData.XH.Hold;
using ReckoningCounter.MemoryData.QH.Hold;
using ReckoningCounter.MemoryData.XH.Capital;
using ReckoningCounter.DAL.Data.QH;
using CommonObject = GTA.VTS.Common.CommonObject;
using RealTime.Server.SModelData.HqData;

namespace ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL
{
    /// <summary>
    /// 作用：柜台各资金账户明细查询和各持仓账户明细查询（包括： 银行资金明细查询、现货资金明细查询、 期货资金明细查询、现货持仓查询、 期货持仓查询）
    /// 作者：李科恒
    /// 日期：2008-10-30
    /// Update BY：李健华
    /// Update Date:2009-07-22
    /// Desc.:修改相关的DAL操作，相关方法的逻辑以及业务逻辑，并把相关一些数据从缓存中获取，如用户账号和资金、持仓等信息
    /// </summary>
    public class CapitalAndHoldFindBLL
    {
        # region 通过交易员ID获得该交易员的银行账号
        /// <summary>
        ///  通过交易员ID获得该交易员的银行账号
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <returns></returns>
        string GetBankAccountByUserId(string userId)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                #region oldcode
                // var userAccountAllocationTable = DataRepository.UaUserAccountAllocationTableProvider.Find(
                //   string.Format("UserID='{0}' AND AccountTypeLogo='1'", userId));
                #endregion
                #region 获取用户银行账号
                #region 从缓存中获取用户银行账号
                UA_UserAccountAllocationTableInfo account = AccountManager.Instance.GetAccountByUserIDAndAccountType(userId, (int)Types.AccountType.BankAccount);
                #endregion
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                //List<UA_UserAccountAllocationTableInfo> userAccountAllocationTable = dal.GetListArray(string.Format(" UserID='{0}' AND AccountTypeLogo='1'", userId));
                //foreach (UA_UserAccountAllocationTableInfo item in userAccountAllocationTable)
                //{
                //    result = item.UserAccountDistributeLogo;
                //}
                #endregion
            }
            return result;
        }
        # endregion 通过资金账号获得市值

        # region 通过交易员所拥有的交易账号获得该交易员ID
        /// <summary>
        ///  通过交易员所拥有的交易账号获得该交易员ID
        ///  Update Date:2009-07-15
        ///  Update By:李健华
        ///  Desc.:修改操作数据层方法和相关实体
        /// </summary>
        /// <param name="TradeAccount">交易账号</param>
        /// <returns></returns>
        public string GetUserIdByTradeAccount(string TradeAccount)
        {
            string userId = string.Empty;

            if (!string.IsNullOrEmpty(TradeAccount))
            {
                #region old code
                //var userAccountAllocationTable = DataRepository.UaUserAccountAllocationTableProvider.Find(
                //    string.Format("UserAccountDistributeLogo='{0}'", TradeAccount));
                #endregion
                #region 从缓存中获取用户账号信息
                UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetUserByAccount(TradeAccount);
                if (userInfo != null)
                {
                    userId = userInfo.UserID;
                }
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                //List<UA_UserAccountAllocationTableInfo> userAccountAllocationTable = dal.GetListArray(string.Format(" UserAccountDistributeLogo='{0}'", TradeAccount));
                //foreach (UA_UserAccountAllocationTableInfo item in userAccountAllocationTable)
                //{
                //    userId = item.UserID;
                //}
                #endregion
            }
            return userId;
        }
        # endregion

        # region 通过现货资金账号和币种获得该资金账号下的该币种的持仓市值
        /// <summary>
        ///  通过现货资金账号和币种获得该资金账号下的该币种的持仓市值
        ///  Update Date:2009-07-15
        ///  Update By:李健华
        ///  Desc.:修改操作数据层方法和相关实体,并增加out总浮动盈亏（未实现盈亏）统计参数
        /// </summary>
        /// <param name="strFundAccount">现货资金账户</param>
        /// <param name="currencyType">查询交易货币类型</param>
        /// <param name="floatProfitLoss">总浮动盈亏</param>
        /// <returns>返回所有持仓总市值</returns>
        decimal GetMarketValueByXhFundAccount(string strFundAccount, Types.CurrencyType currencyType, out decimal floatProfitLoss)
        {
            #region ==变量定义==
            decimal result = 0;
            floatProfitLoss = 0;
            //string errorMsg = "";

            #endregion
            #region 从缓存中根据现货资金账户获取关联的持仓账户信息
            UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(strFundAccount);
            //UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();
            ////根据现货资金账户获取关联的持仓账户信息
            //UA_UserAccountAllocationTableInfo userInfo = userDal.GetUserHoldAccountByUserCapitalAccount(strFundAccount);
            #endregion
            if (userInfo != null)
            {
                result = GetMarketValueByXH_HoldAccount(userInfo.UserAccountDistributeLogo, currencyType, out floatProfitLoss);
            }

            #region update 2009-07-14 李健华
            //string strHoldAccount = CommonDataAgent.GetRealtionAccountIdByAccountId(strFundAccount);
            //if (!string.IsNullOrEmpty(strHoldAccount))
            //{
            //    var holdList = DataRepository.XhAccountHoldTableProvider.Find(
            //        string.Format(" UserAccountDistributeLogo='{0}' AND CurrencyTypeId='{1}'", strHoldAccount, (int)currencyType));
            //    foreach (var item in holdList)
            //    {
            //        HqExData vhe = CommonDataAgent.RealtimeService.GetStockHqData(item.Code);
            //        if (vhe != null)
            //        {
            //            result += (item.AvailableAmount.Value + item.FreezeAmount.Value) *
            //                      Convert.ToDecimal(vhe.HqData.Lasttrade);
            //        }
            //    }
            //}
            #endregion

            return result;
        }
        /// <summary>
        /// 通过现货持仓账号和币种获得该现货持仓账号下的该币种的现货持仓市值和总浮动盈亏
        /// Create by:李健华
        /// Create Date:2009-07-15
        /// </summary>
        /// <param name="holdAccount">现货持仓账号</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="floatProfitLoss">总浮动盈亏</param>
        /// <returns></returns>
        decimal GetMarketValueByXH_HoldAccount(string holdAccount, Types.CurrencyType currencyType, out decimal floatProfitLoss)
        {
            decimal marketValue = 0;
            floatProfitLoss = 0;
            string errorMsg = "";
            #region 获取当前持仓账户下所有持仓
            List<XH_AccountHoldTableInfo> list = QueryXH_AccountHoldByAccount(holdAccount, (QueryType.QueryCurrencyType)((int)currencyType), out errorMsg);
            #endregion

            #region 编历所有持仓信息统计
            foreach (XH_AccountHoldTableInfo item in list)
            {
                string codeStr = item.Code;
                //通过行情服务器获取当前现货行情
                HqExData vhe = CommonDataAgent.RealtimeService.GetStockHqData(codeStr);

                #region 根据商品代码获取撮合（即交易单位）单位转换成计价单位的倍数
                //根据商品代码获取搓合单位
                Types.UnitType utMatch = MCService.GetMatchUnitType(codeStr);
                //根据搓合单位转换成计价单位获取得转换的倍数
                decimal unitMultiple = MCService.GetTradeUnitScale(codeStr, utMatch);
                #endregion

                # region 获取持仓总量并赋值
                decimal amount = item.AvailableAmount + item.FreezeAmount;
                //把撮合（即交易）单位持仓总量转换为计价单位的持仓总量，因为之前存储到数据库中的与持仓量有关
                //的都是交易单位量，价格相关的都是计价单位。
                amount = amount * unitMultiple;
                #endregion

                # region 获取当前价并赋值 如果获取不到行情数据最新成交价再以昨日收盘价计算如果昨日收盘价为0再把当前价以持仓均价来代替计算
                //获取当前价并赋值 如果获取不到行情数据最新成交价再以昨日收盘价计算
                //如果昨日收盘价为0再把当前价以持仓均价来代替计算
                decimal realtimePrice = 0.00M;
                if (vhe != null)
                {
                    if (vhe.HqData.Lasttrade == 0)
                    {
                        decimal yesterPrice = MCService.CommonPara.GetClosePriceByCode(codeStr);
                        if (yesterPrice <= 0)
                        {
                            errorMsg = "【现货资金统计】行情最新成交价为0,昨日收盘价也为0,当前记录使用持仓均价计算";
                            realtimePrice = item.HoldAveragePrice;
                        }
                        else
                        {
                            errorMsg = "【现货资金统计】行情最新成交价为0,当前记录使用昨日收盘价计算";
                            realtimePrice = yesterPrice;
                        }
                        LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + codeStr + errorMsg);
                    }
                    else
                    {
                        realtimePrice = Convert.ToDecimal(vhe.HqData.Lasttrade);
                    }
                }
                else
                {
                    decimal yesterPrice = MCService.CommonPara.GetClosePriceByCode(codeStr);
                    if (yesterPrice <= 0)
                    {
                        errorMsg = "【现货资金统计】未能获取行情,昨日收盘价也为0,当前记录使用持仓均价计算";
                        realtimePrice = item.HoldAveragePrice;
                    }
                    else
                    {
                        errorMsg = "【现货资金统计】未能获取行情,当前记录使用昨日收盘价计算";
                        realtimePrice = yesterPrice;
                    }
                    LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + codeStr + errorMsg);
                }
                #endregion

                #region 统计总浮动盈亏
                //浮动盈亏=持仓总量*（当前价-持仓均价）
                floatProfitLoss += amount * (realtimePrice - item.HoldAveragePrice);
                #endregion

                #region 总市值
                marketValue += amount * realtimePrice;
                #endregion

            }
            #endregion

            return marketValue;
        }
        # endregion

        # region 通过期货资金账号和币种获得该资金账号下的该币种的持仓市值
        /// <summary>
        ///  通过期货资金账号和币种获得该资金账号下的该币种的持仓市值
        ///  Update Date:2009-07-15
        ///  Update By:李健华
        ///  Desc.:修改操作数据层方法和相关实体,并增加out总持仓浮动盈亏统计,总持仓盯市盈亏参数
        /// </summary>
        /// <param name="strFundAccount">期货资金账户</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="floatProfitLossTotal">总持仓浮动盈亏</param>
        /// <param name="marketProfitLossTotal">总持仓盯市盈亏</param>
        /// <returns></returns>
        decimal GetMarketValueByQhFundAccount(string strFundAccount, Types.CurrencyType currencyType, out decimal floatProfitLossTotal, out decimal marketProfitLossTotal)
        {
            #region ==变量定义==
            decimal result = 0;
            floatProfitLossTotal = 0;//总持仓浮动盈亏
            marketProfitLossTotal = 0;//总持仓盯市盈亏
            //string errorMsg = "";
            #endregion
            #region 从缓存中根据现货资金账户获取关联的持仓账户信息
            UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(strFundAccount);
            //UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();
            ////根据现货资金账户获取关联的持仓账户信息
            // UA_UserAccountAllocationTableInfo userInfo = userDal.GetUserHoldAccountByUserCapitalAccount(strFundAccount);
            #endregion
            if (userInfo != null)
            {
                result = GetMarketValueByQH_HoldAccount(userInfo.UserAccountDistributeLogo, currencyType, out floatProfitLossTotal, out marketProfitLossTotal);
            }
            #region update 2009-07-14 李健华
            //string strHoldAccount = CommonDataAgent.GetRealtionAccountIdByAccountId(strFundAccount);
            //if (!string.IsNullOrEmpty(strHoldAccount))
            //{
            //    //=================modify by xiongxl ====================
            //    //修改字段名称
            //    var holdList = DataRepository.QhHoldAccountTableProvider.Find(
            //        string.Format(" UserAccountDistributeLogo='{0}' AND TradeCurrencyType='{1}'", strHoldAccount, (int)currencyType));
            //    //====================end================================
            //    foreach (var item in holdList)
            //    {
            //        VTFutData vte = CommonDataAgent.RealtimeService.GetFutData(item.Contract);
            //        if (vte != null)
            //        {
            //            result += (item.HistoryHoldAmount.Value + item.HistoryFreezeAmount.Value + item.TodayHoldAmount.Value + item.TodayFreezeAmount.Value) *
            //                      Convert.ToDecimal(vte.Lasttrade);//Lasttrade为当前价（市价）
            //        }
            //    }
            //}
            #endregion

            return result;
        }
        /// <summary>
        /// 通过期货持仓账号和币种获得该资金账号下的该币种的持仓市值，总浮动盈亏，总盯市盈亏
        /// Create by:李健华
        /// Create Date:2009-07-15
        /// </summary>
        /// <param name="holdAccount">持仓账号</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="floatProfitLossTotal">总浮动盈亏</param>
        /// <param name="marketProfitLossTotal">总盯市盈亏</param>
        /// <returns></returns>
        decimal GetMarketValueByQH_HoldAccount(string holdAccount, Types.CurrencyType currencyType, out decimal floatProfitLossTotal, out decimal marketProfitLossTotal)
        {
            #region ==变量定义==
            decimal marketValue = 0;
            floatProfitLossTotal = 0;//总持仓浮动盈亏
            marketProfitLossTotal = 0;//总持仓盯市盈亏
            string errorMsg = "";
            #endregion

            #region 获取当前持仓账户下所有持仓
            List<QH_HoldAccountTableInfo> list = QueryQH_HoldAccountByAccount(holdAccount, (QueryType.QueryCurrencyType)((int)currencyType), out errorMsg);
            #endregion

            #region 编历所有持仓信息统计
            foreach (QH_HoldAccountTableInfo item in list)
            {
                string contract = item.Contract;
                //通过行情服务器获取当前现货行情
                int? breedID = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(contract);
                //昨结算价
                double preSettlementPrice = 0;
                //今结算价
                double settlementPrice = 0;
                //最新价
                double lasttrade = 0;
                decimal realtimePrice = 0.00M;

                bool isGetRealtime = false;
                if (breedID.HasValue)
                {
                    switch ((CommonObject.Types.BreedClassTypeEnum)breedID.Value)
                    {

                        case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture:
                            FutData vte = CommonDataAgent.RealtimeService.GetFutData(contract);
                            if (vte == null)
                            {
                                errorMsg = "【股指期货资金统计】未获取到该股指期货代码的行情,当前记录使用持仓均价计算.";
                                LogHelper.WriteDebug("持仓账号:" + item.UserAccountDistributeLogo + " 代码：" + item.Contract + errorMsg);
                                realtimePrice = item.HoldAveragePrice;
                            }
                            else
                            {
                                preSettlementPrice = vte.PreSettlementPrice;
                                settlementPrice = vte.SettlementPrice;
                                lasttrade = vte.Lasttrade;
                                isGetRealtime = true;
                            }
                            break;
                        case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture:
                            MerFutData mfvte = CommonDataAgent.RealtimeService.GetMercantileFutData(contract);
                            if (mfvte == null)
                            {
                                errorMsg = "【商品期货资金统计】未获取到该商品期货代码的行情,当前记录使用持仓均价计算.";
                                LogHelper.WriteDebug("持仓账号:" + item.UserAccountDistributeLogo + " 代码：" + contract + errorMsg);
                                realtimePrice = item.HoldAveragePrice;
                            }
                            else
                            {
                                preSettlementPrice = mfvte.PreClearPrice;
                                settlementPrice = mfvte.ClearPrice;
                                lasttrade = mfvte.Lasttrade;
                                isGetRealtime = true;
                            }
                            break;
                    }

                }


                #region 根据商品代码获取撮合（即交易单位）单位转换成计价单位的倍数
                ////根据商品代码获取搓合单位
                //Types.UnitType utMatch = MCService.GetMatchUnitType(contract);
                ////根据搓合单位转换成计价单位获取得转换的倍数
                //decimal unitMultiple = MCService.GetTradeUnitScale(contract, utMatch);
                //根据商品代码获取期货交易单位计价单位倍数
                decimal scale = MCService.GetFutureTradeUntiScale(contract);
                #endregion

                # region 获取持仓总量并赋值
                decimal amount = item.HistoryHoldAmount + item.HistoryFreezeAmount + item.TodayHoldAmount + item.TodayFreezeAmount;
                //把撮合（即交易）单位持仓总量转换为计价单位的持仓总量，因为之前存储到数据库中的与持仓量有关
                //的都是交易单位量，价格相关的都是计价单位。
                amount = amount * scale;
                #endregion

                # region 获取当前价并赋值 如果获取不到行情数据把当前价以持仓均价来代替计算
                //  持仓盈亏（盯）：持仓合约的持仓均价与市场成交价之间的差额收益，
                //在当日收盘结算前此收益为理论收益，当日结算后，盯市盈亏转为实际的盈亏。
                //买入持仓
                //持仓盈亏（盯）=（市场成交价-买入持仓均价）*买入持仓总量*合约乘数
                //卖出持仓
                //持仓盈亏（盯）=（卖出持仓均价-市场成交价）*卖出持仓总量*合约乘数

                //当日00：00：00至当日开盘前：市场成交价=昨日结算价
                //当日开盘至当日收盘：市场成交价=最新的一笔成交价格，也称为当前价
                //当日收盘后至当日23：59：59：市场成交价=当日结算价
                //买入持仓均价与卖出持仓均价在收盘清算后，需要变成当日结算价。
              

               if(isGetRealtime)
                {
                    int k = MCService.IsNowTimeMarket(Types.BreedClassTypeEnum.StockIndexFuture, contract);

                    switch (k)
                    {
                        case 0:
                            #region 当前时间00：00：00至当日开盘前
                            if (preSettlementPrice == 0)
                            {
                                errorMsg = "【期货资金统计】获取该期货代码的行情昨日结算价为0,当前记录使用持仓均价计算";
                                LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + contract + errorMsg);
                                realtimePrice = item.HoldAveragePrice;
                            }
                            else
                            {
                                //昨结算 PreSettlementPrice  //今结算 SettlementPrice
                                realtimePrice = Convert.ToDecimal(preSettlementPrice);
                            }
                            #endregion
                            break;
                        case 1:
                            #region 当前时间开盘至当日收盘
                            if (lasttrade == 0)
                            {
                                errorMsg = "【期货资金统计】获取该期货代码的行情最新成交价为0,当前记录使用持仓均价计算";
                                LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + contract + errorMsg);
                                realtimePrice = item.HoldAveragePrice;
                            }
                            else
                            {
                                //昨结算 PreSettlementPrice  //今结算 SettlementPrice
                                realtimePrice = Convert.ToDecimal(lasttrade);
                            }
                            #endregion
                            break;
                        case 2:
                            #region 当日收盘后至当日23：59：59--2
                            if (settlementPrice == 0)
                            {
                                errorMsg = "【期货资金统计】获取该期货代码的行情今日结算价为0,当前记录使用持仓均价计算";
                                LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + contract + errorMsg);
                                realtimePrice = item.HoldAveragePrice;
                            }
                            else
                            {
                                //昨结算 PreSettlementPrice  //今结算 SettlementPrice
                                realtimePrice = Convert.ToDecimal(settlementPrice);
                            }
                            #endregion
                            break;
                    }
                }

                #region old code
                //if (vte != null)
                //{
                //    if (vte.Lasttrade == 0)
                //    {
               //        errorMsg = "【期货资金统计】获取该现货代码的行情最新成交价为0,当前记录使用持仓均价计算";
                //        LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + contract + errorMsg);
                //        realtimePrice = item.HoldAveragePrice;
                //    }
                //    else
                //    {
                //        //昨结算 PreSettlementPrice
                //        //今结算 SettlementPrice
                //        realtimePrice = Convert.ToDecimal(vte.Lasttrade);
                //    }
                //}
                //else
                //{
                //    errorMsg = "【期货资金统计】未获取到该现货代码的行情,当前记录使用持仓均价计算.";
                //    LogHelper.WriteDebug("持仓账号:" + holdAccount + " 代码：" + contract + errorMsg);
                //    realtimePrice = item.HoldAveragePrice;
                //}
                #endregion

                #endregion

                # region 获取【盯市盈亏、浮动盈亏】并赋值

                if (item.BuySellTypeId == (int)CommonObject.Types.TransactionDirection.Buying)
                {
                    // 买方向的盯市盈亏=买方向的盯市盈亏=[持仓总量={0}*(当前价={1}-持仓均价={2})*交易单位倍数={3}]
                    // marketProfitLossTotal += amount * (realtimePrice - item.HoldAveragePrice) * scale;
                    //前面总量已经转为计价单位总数，所以这里不再用*交易单位倍数
                    marketProfitLossTotal += amount * (realtimePrice - item.HoldAveragePrice);
                    //买方向的浮动盈亏=买方向的浮动盈亏=[持仓总量={0}*(当前价={1}-开仓均价={2})*交易单位倍数={3}]
                    floatProfitLossTotal += amount * (realtimePrice - item.OpenAveragePrice);
                }
                else
                {
                    //卖方向的盯市盈亏=卖方向的盯市盈亏=[持仓总量={0}*(持仓均价={1}-当前价={2})*交易单位倍数={3}]
                    marketProfitLossTotal += amount * (item.HoldAveragePrice - realtimePrice);
                    //卖方向的浮动盈亏=卖方向的浮动盈亏=[持仓总量={0}*(开仓均价={1}-当前价={2})*交易单位倍数={3}]
                    floatProfitLossTotal += amount * (item.OpenAveragePrice - realtimePrice);
                }
                # endregion

                #region 总市值
                marketValue += amount * realtimePrice;
                #endregion

            }
            #endregion

            return marketValue;
        }
        # endregion

        #region 现货资金明细查询(通过资金账号和币种）
        /// <summary>
        /// 现货资金明细查询(通过资金账号和币种）
        /// </summary>
        /// <param name="strFundAccountId">现资金账号</param>
        /// <param name="currencyType">货币类型</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="strErrorMessage">异常信息</param>
        /// <returns></returns>
        public SpotCapitalEntity SpotCapitalFind(string strFundAccountId, Types.CurrencyType currencyType, string userPassword, ref string strErrorMessage)
        {
            SpotCapitalEntity result = null;
            try
            {
                #region update 2009-07-14 李健华
                #region old Code
                //var capitalAccount = DataRepository.XhCapitalAccountTableProvider.Find(
                //    string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", strFundAccountId,
                //                  (int)currencyType));
                #endregion
                List<XH_CapitalAccountTableInfo> capitalAccount = QueryXH_CapitalAccountByAccount(strFundAccountId, (QueryType.QueryCurrencyType)((int)currencyType), out strErrorMessage);
                #endregion
                //if (capitalAccount != null && capitalAccount.Count > 0)
                if (!Utils.IsNullOrEmpty(capitalAccount))
                {
                    XH_CapitalAccountTableInfo ca = capitalAccount[0];

                    #region 获取交易货币类型名称
                    CM_CurrencyType cmCurrencyType = MCService.CommonPara.GetCurrencyTypeByID(ca.TradeCurrencyType);
                    string cName = "";
                    if (cmCurrencyType != null)
                        cName = cmCurrencyType.CurrencyName;
                    #endregion

                    decimal notDoneProfitLossTotal = 0;//未实现盈亏
                    decimal marketValue = GetMarketValueByXhFundAccount(strFundAccountId, currencyType, out notDoneProfitLossTotal);
                    result = new SpotCapitalEntity(ca, marketValue, cName, notDoneProfitLossTotal);
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                LogHelper.WriteError("现货资金明细查询SpotCapitalFind()异常：" + strFundAccountId + "   CurrencyType:" + (int)currencyType, ex);
            }
            return result;
        }

        #endregion 现货资金查询(通过资金账号和币种）

        #region 期货资金明细查询（通过资金账号和币种）
        public FuturesCapitalEntity FuturesCapitalFind(string strFundAccountId, Types.CurrencyType currencyType, string userPassword, ref string strErrorMessage)
        {
            FuturesCapitalEntity result = null;
            try
            {
                #region oldCode
                //var capitalAccount = DataRepository.QhCapitalAccountTableProvider.Find(
                //    string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", strFundAccountId,
                //                  (int)currencyType));
                #endregion
                List<QH_CapitalAccountTableInfo> capitalAccount = QueryQH_CapitalAccountByAccount(strFundAccountId, (QueryType.QueryCurrencyType)((int)currencyType), out strErrorMessage);

                //if (capitalAccount != null && capitalAccount.Count > 0)
                if (!Utils.IsNullOrEmpty(capitalAccount))
                {
                    QH_CapitalAccountTableInfo ca = capitalAccount[0];

                    #region 获取交易货币类型名称
                    CM_CurrencyType cmCurrencyType = MCService.CommonPara.GetCurrencyTypeByID(ca.TradeCurrencyType);
                    string cName = "";
                    if (cmCurrencyType != null)
                        cName = cmCurrencyType.CurrencyName;
                    #endregion
                    decimal floatProfitLossTotal = 0;//总持仓浮动盈亏
                    decimal marketProfitLossTotal = 0;//总持仓盯市盈亏 
                    decimal marketValue = GetMarketValueByQhFundAccount(strFundAccountId, currencyType, out  floatProfitLossTotal, out marketProfitLossTotal);
                    result = new FuturesCapitalEntity(ca, marketValue, cName, floatProfitLossTotal, marketProfitLossTotal);
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                LogHelper.WriteError("期货资金明细查询FuturesCapitalFind()异常：" + strFundAccountId + "   CurrencyType:" + (int)currencyType, ex);

            }
            return result;
        }
        #endregion

        # region 现货持仓查询过滤条件
        /// <summary>
        /// 现货持仓查询过滤条件
        ///  Update Date:2009-07-15
        ///  Update By:李健华
        ///  Desc.:修改操作组装条件方式
        /// </summary>
        /// <param name="_findConditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildXhHoldQueryWhere(SpotHoldConditionFindEntity _findConditions)
        {
            string result = "1=1 ";

            # region  0.股东代码
            //0.股东代码
            if (!string.IsNullOrEmpty(_findConditions.SpotHoldAccount))
            {
                result += string.Format(" And UserAccountDistributeLogo='{0}'", _findConditions.SpotHoldAccount);
            }
            # endregion

            # region 2.币种赋值
            if (_findConditions.CurrencyType != 0)
            {
                result += string.Format(" AND CurrencyTypeId='{0}'", _findConditions.CurrencyType);
            }
            # endregion

            # region 3.现货代码
            if (!string.IsNullOrEmpty(_findConditions.SpotCode))
            {
                result += string.Format(" AND Code='{0}'", _findConditions.SpotCode);
            }
            # endregion
            return result;
        }
        # endregion 现货持仓查询过滤条件

        # region （NEW）期货持仓查询过滤条件
        /// <summary>
        /// 期货持仓查询过滤条件
        /// </summary>
        /// <param name="_findCoditions">过滤条件实体对象</param>
        /// <returns></returns>
        string BuildQhHoldQueryWhere(FuturesHoldConditionFindEntity _findCoditions)
        {
            string result = "1=1 ";

            # region  1.期货交易编码
            if (!string.IsNullOrEmpty(_findCoditions.FuturesHoldAccount))
            {
                result += string.Format(" And  UserAccountDistributeLogo='{0}'", _findCoditions.FuturesHoldAccount);
            }
            # endregion

            # region 2.币种赋值
            if (_findCoditions.CurrencyType != 0)
            {

                result += string.Format(" AND CurrencyTypeId='{0}'", _findCoditions.CurrencyType);
            }
            # endregion

            # region 3.合约代码
            if (!string.IsNullOrEmpty(_findCoditions.ContractCode))
            {

                result += string.Format(" AND Contract='{0}'", _findCoditions.ContractCode);
            }
            # endregion
            return result;
        }
        # endregion 现货持仓查询过滤条件

        # region 现货持仓查询 （根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 现货持仓查询 （根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        ///  Update Date:2009-07-15
        ///  Update By:李健华
        ///  Desc.:修改操作数据层方法和相关实体
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<SpotHoldFindResultEntity> SpotHoldFind(string capitalAccount, string strPassword,
                                                      SpotHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            List<XH_AccountHoldTableInfo> tempt = null;
            List<SpotHoldFindResultEntity> result = new List<SpotHoldFindResultEntity>();
            //SpotHoldFindResultEntity
            count = 0;
            try
            {
                #region ==old code==
                // 由资金账号查找相对应的持仓账号
                ////public static  string GetRealtionAccountIdByAccountId(string strAccountId)
                // string XhHoldAccount = CommonDataAgent.GetRealtionAccountIdByAccountId(capitalAccount);
                #endregion

                #region 根据现货资金账户获取关联的持仓账户信息
                UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(capitalAccount);
                //UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();
                ////根据现货资金账户获取关联的持仓账户信息
                //UA_UserAccountAllocationTableInfo userInfo = userDal.GetUserHoldAccountByUserCapitalAccount(capitalAccount);
                if (userInfo == null)
                {
                    strErrorMessage = "查询不到相关账户信息";
                    return result;
                }
                #endregion

                #region 组装条件
                string whereCondition = "";
                if (findCondition != null)
                {
                    //将股东代码（持仓账号）加入查询条件中
                    findCondition.SpotHoldAccount = userInfo.UserAccountDistributeLogo;
                    whereCondition = BuildXhHoldQueryWhere(findCondition);
                    //tempt = DataRepository.XhAccountHoldTableProvider.GetPaged(whereCondition, "AccountHoldLogoId ASC", start, pageLength, out count);
                }
                else
                {
                    whereCondition = string.Format(" UserAccountDistributeLogo='{0}'", userInfo.UserAccountDistributeLogo);
                    //var holdAccount = Common.Utils.GetXHHoldAccountByCapitalAccount(capitalAccount);
                    //tempt = DataRepository.XhAccountHoldTableProvider.GetByUserAccountDistributeLogo(holdAccount.UserAccountDistributeLogo);
                }
                #endregion

                #region 分页查询相关信息
                PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
                if (start <= 1)
                {
                    prcoInfo.IsCount = false;
                }
                {
                    prcoInfo.IsCount = true;
                }
                prcoInfo.PageNumber = start;
                prcoInfo.PageSize = pageLength;
                prcoInfo.Fields = "  AccountHoldLogoId,AvailableAmount,FreezeAmount,UserAccountDistributeLogo,CostPrice,Code,BreakevenPrice,CurrencyTypeId,HoldAveragePrice ";
                prcoInfo.PK = "AccountHoldLogoId";
                prcoInfo.Sort = " AvailableAmount asc ";
                prcoInfo.Tables = " XH_AccountHoldTable ";

                #region 组装相关条件
                prcoInfo.Filter = whereCondition;
                #endregion


                #endregion

                XH_AccountHoldTableDal xh_AccHolDal = new XH_AccountHoldTableDal();
                CommonDALOperate<XH_AccountHoldTableInfo> com = new CommonDALOperate<XH_AccountHoldTableInfo>();
                // CommonDALOperate<XH_AccountHoldTableInfo>.DataReaderBind bind = xh_AccHolDal.ReaderBind;
                //tempt = com.PagingQueryProcedures(prcoInfo, out count, bind);
                tempt = com.PagingQueryProcedures(prcoInfo, out count, xh_AccHolDal.ReaderBind);


                foreach (XH_AccountHoldTableInfo _XhAccountHold in tempt)
                {
                    strErrorMessage = "";

                    SpotHoldFindResultEntity sfre = new SpotHoldFindResultEntity();
                    sfre.HoldFindResult = _XhAccountHold;

                    #region 根据商品代码获取商品对象实体
                    CM_Commodity _CM_Commodity = MCService.CommonPara.GetCommodityByCommodityCode(_XhAccountHold.Code);
                    #endregion

                    # region  获取品种类别并赋值
                    //品种类别 
                    #region old code
                    // int BreedClassId = Convert.ToInt32(MCService.CommonPara.GetBreedClassIdByCommodityCode(_XhAccountHold.Code));

                    //sfre.VarietyCategories = BreedClassId.ToString();
                    #endregion

                    if (_CM_Commodity != null)
                    {
                        if (findCondition != null && !string.IsNullOrEmpty(findCondition.VarietyCode))
                        {
                            if (_CM_Commodity.BreedClassID.Value.ToString().Trim() != findCondition.VarietyCode.Trim())
                            {
                                continue;
                            }
                        }
                        sfre.VarietyCategories = _CM_Commodity.BreedClassID.Value.ToString();
                    }

                    # endregion

                    # region  获取所属市场并赋值
                    //所属市场
                    CM_BourseType _CM_BourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(_XhAccountHold.Code);
                    if (_CM_BourseType != null)
                    {
                        sfre.BelongMarket = _CM_BourseType.BourseTypeName;
                    }
                    else
                    {
                        strErrorMessage = "根据商品代码未获取到商品所属市场 ！";
                    }
                    # endregion

                    # region  获取现货名称并赋值
                    //现货名称
                    if (_CM_Commodity != null)
                    {
                        sfre.SpotName = _CM_Commodity.CommodityName;
                    }
                    else
                    {
                        strErrorMessage = "根据商品代码未获取到现货名称 ！";
                    }
                    # endregion

                    # region 获取币种名称并赋值
                    string _currencyName = MCService.CommonPara.GetCurrencyTypeByID(_XhAccountHold.CurrencyTypeId).CurrencyName;
                    if (!string.IsNullOrEmpty(_currencyName))
                    {
                        sfre.CurrencyName = _currencyName;
                    }
                    else
                    {
                        strErrorMessage = "现货持仓表中的币种类型存储错误！";
                    }
                    # endregion

                    # region 获取持仓总量并赋值
                    sfre.HoldSumAmount = Convert.ToInt32(_XhAccountHold.AvailableAmount + _XhAccountHold.FreezeAmount);
                    # endregion

                    # region 获取当前价并赋值
                    HqExData vhe = CommonDataAgent.RealtimeService.GetStockHqData(_XhAccountHold.Code);
                    if (vhe != null)
                    {
                        if (vhe.HqData.Lasttrade == 0)
                        {
                            decimal yesterPrice = MCService.CommonPara.GetClosePriceByCode(_XhAccountHold.Code);
                            if (yesterPrice <= 0)
                            {
                                strErrorMessage = "【现货资金统计】行情最新成交价为0,昨日收盘价也为0,当前记录使用持仓均价计算";
                                sfre.RealtimePrice = _XhAccountHold.HoldAveragePrice;
                            }
                            else
                            {
                                strErrorMessage = "【现货资金统计】行情最新成交价为0,当前记录使用昨日收盘价计算";
                                sfre.RealtimePrice = yesterPrice;
                            }
                            LogHelper.WriteDebug("持仓账号:" + _XhAccountHold.UserAccountDistributeLogo + " 代码：" + _XhAccountHold.Code + strErrorMessage);
                        }
                        else
                        {
                            sfre.RealtimePrice = Convert.ToDecimal(vhe.HqData.Lasttrade);
                        }
                    }
                    else
                    {
                        decimal yesterPrice = MCService.CommonPara.GetClosePriceByCode(_XhAccountHold.Code);
                        if (yesterPrice <= 0)
                        {
                            strErrorMessage = "【现货资金统计】未能获取行情,昨日收盘价也为0,当前记录使用持仓均价计算";
                            sfre.RealtimePrice = _XhAccountHold.HoldAveragePrice;
                        }
                        else
                        {
                            strErrorMessage = "【现货资金统计】未能获取行情,当前记录使用昨日收盘价计算";
                            sfre.RealtimePrice = yesterPrice;
                        }
                        LogHelper.WriteDebug("持仓账号:" + _XhAccountHold.UserAccountDistributeLogo + " 代码：" + _XhAccountHold.Code + strErrorMessage);
                    }

                    # endregion

                    #region 根据商品代码获取撮合（即交易单位）单位转换成计价单位的倍数
                    //根据商品代码获取搓合单位
                    Types.UnitType utMatch = MCService.GetMatchUnitType(_XhAccountHold.Code);
                    //根据搓合单位转换成计价单位获取得转换的倍数
                    decimal unitMultiple = MCService.GetTradeUnitScale(_XhAccountHold.Code, utMatch);
                    #endregion

                    # region 获取市值并赋值
                    //因为持仓记录的是撮合单位量（即交易量）所以要转换成计价单位量才正确
                    //sfre.MarketValue = sfre.HoldSumAmount * sfre.RealtimePrice;
                    sfre.MarketValue = sfre.HoldSumAmount * unitMultiple * sfre.RealtimePrice;
                    # endregion

                    # region 获取浮动盈亏并赋值
                    //浮动盈亏=持仓总量*（当前价-持仓均价）
                    //因为持仓记录的是撮合单位量（即交易量）所以要转换成计价单位量才正确
                    //sfre.FloatProfitLoss = sfre.HoldSumAmount * (sfre.RealtimePrice - _XhAccountHold.HoldAveragePrice);
                    sfre.FloatProfitLoss = sfre.HoldSumAmount * unitMultiple * (sfre.RealtimePrice - _XhAccountHold.HoldAveragePrice);
                    # endregion

                    # region 获取交易员ID并赋值
                    sfre.TraderId = userInfo.UserID;
                    #region old code
                    //string _userId = string.Empty;
                    //_userId = GetUserIdByTradeAccount(capitalAccount);
                    //if (!string.IsNullOrEmpty(_userId))
                    //{
                    //    sfre.TraderId = _userId;
                    //}
                    //else
                    //    strErrorMessage = "未获取到该资金账号所对应的交易员ID ！";
                    #endregion
                    # endregion

                    # region 获取错误原因并赋值
                    if (!string.IsNullOrEmpty(strErrorMessage))
                    {
                        string errtxt = strErrorMessage;
                        sfre.ErroReason = errtxt;
                    }
                    # endregion

                    # region 获取错误号并赋值（还未实现，暂时赋为空值）
                    sfre.ErroNumber = string.Empty;
                    # endregion

                    result.Add(sfre);
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                LogHelper.WriteError("ReckoningCounter.BLL.AccountManagementAndFind.AccountManagementAndFindBLL--SpotHoldFind()查询异常", ex);
            }
            return result;
        }

        # endregion 现货持仓查询

        #region 获取当前行情价格
        /// <summary>
        /// 获取当前行情价格
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private decimal ReturnRealtimePrice(string code)
        {
            try
            {
                decimal realtimePrice = 0;
                FutData vte = CommonDataAgent.RealtimeService.GetFutData(code);
                if (vte != null)
                {
                    realtimePrice = Convert.ToDecimal(vte.Lasttrade);

                    if (realtimePrice <= 0) //从行情中取的当前价为0时，连续取3次
                    {
                        vte = CommonDataAgent.RealtimeService.GetFutData(code);
                        realtimePrice = Convert.ToDecimal(vte.Lasttrade);
                        if (realtimePrice <= 0)
                        {
                            vte = CommonDataAgent.RealtimeService.GetFutData(code);
                            realtimePrice = Convert.ToDecimal(vte.Lasttrade);

                        }
                        if (realtimePrice <= 0)
                        {
                            vte = CommonDataAgent.RealtimeService.GetFutData(code);
                            realtimePrice = Convert.ToDecimal(vte.Lasttrade);

                        }
                    }
                }
                else
                {
                    //当行情为空时，连续取3次行情
                    vte = CommonDataAgent.RealtimeService.GetFutData(code);
                    if (vte == null)
                    {
                        vte = CommonDataAgent.RealtimeService.GetFutData(code);
                    }
                    if (vte == null)
                    {
                        vte = CommonDataAgent.RealtimeService.GetFutData(code);

                    }
                    if (vte != null)
                    {
                        realtimePrice = Convert.ToDecimal(vte.Lasttrade);

                        if (realtimePrice <= 0) //从行情中取的当前价为0时，连续取3次
                        {
                            vte = CommonDataAgent.RealtimeService.GetFutData(code);
                            realtimePrice = Convert.ToDecimal(vte.Lasttrade);
                            if (realtimePrice <= 0)
                            {
                                vte = CommonDataAgent.RealtimeService.GetFutData(code);
                                realtimePrice = Convert.ToDecimal(vte.Lasttrade);

                            }
                            if (realtimePrice <= 0)
                            {
                                vte = CommonDataAgent.RealtimeService.GetFutData(code);
                                realtimePrice = Convert.ToDecimal(vte.Lasttrade);

                            }
                        }
                    }
                }
                return realtimePrice;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return 0;
            }

        }
        #endregion

        # region 期货持仓查询 （根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// <summary>
        /// 期货持仓查询 （根据资金账号、密码及查询条件来查询,其中查询条件中包含委托单号）
        /// </summary>
        /// <param name="capitalAccount">资金账号</param>
        /// <param name="strPassword">交易员密码</param>
        /// <param name="findCondition">查询条件实体对象</param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="count"></param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        public List<FuturesHoldFindResultEntity> FuturesHoldFind(string capitalAccount, string strPassword,
                                                      FuturesHoldConditionFindEntity findCondition, int start, int pageLength, out int count, ref string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            string format = "期货持仓查询[capitalAccount={0},strPassword={1},findCondition={2}]-错误信息：";
            string desc = string.Format(format, capitalAccount, strPassword, findCondition);
            List<QH_HoldAccountTableInfo> tempt = null;
            var result = new List<FuturesHoldFindResultEntity>();

            count = 0;
            try
            {
                #region ==old code==
                // 由资金账号查找相对应的持仓账号
                //public static  string GetRealtionAccountIdByAccountId(string strAccountId)
                //string QhHoldAccount = CommonDataAgent.GetRealtionAccountIdByAccountId(capitalAccount);
                #endregion

                #region 根据现货资金账户获取关联的持仓账户信息
                UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(capitalAccount);
                //UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();
                ////根据现货资金账户获取关联的持仓账户信息
                //UA_UserAccountAllocationTableInfo userInfo = userDal.GetUserHoldAccountByUserCapitalAccount(capitalAccount);
                if (userInfo == null)
                {
                    strErrorMessage = "查询不到相关账户信息";
                    return result;
                }
                #endregion

                #region 组装查询相关条件
                //将交易编码（持仓账号）加入查询条件中
                string whereCondition = "";
                if (findCondition != null)
                {
                    //将股东代码（持仓账号）加入查询条件中
                    findCondition.FuturesHoldAccount = userInfo.UserAccountDistributeLogo;
                    whereCondition = BuildQhHoldQueryWhere(findCondition);
                }
                else
                {
                    whereCondition = string.Format(" UserAccountDistributeLogo='{0}'", userInfo.UserAccountDistributeLogo);
                }
                #endregion

                #region 分页查询相关信息
                PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
                if (start <= 1)
                {
                    prcoInfo.IsCount = false;
                }
                {
                    prcoInfo.IsCount = true;
                }
                prcoInfo.PageNumber = start;
                prcoInfo.PageSize = pageLength;
                prcoInfo.Fields = "  AccountHoldLogoId,HistoryHoldAmount,HistoryFreezeAmount,HoldAveragePrice,TodayHoldAmount,TradeCurrencyType,TodayHoldAveragePrice,UserAccountDistributeLogo,BuySellTypeId,TodayFreezeAmount,Contract,CostPrice,BreakevenPrice,Margin,ProfitLoss,OpenAveragePrice ";
                prcoInfo.PK = "AccountHoldLogoId";
                prcoInfo.Sort = " TodayHoldAmount asc ";
                prcoInfo.Tables = " QH_HoldAccountTable ";

                #region 组装相关条件
                prcoInfo.Filter = whereCondition;
                #endregion


                #endregion

                #region 执行查询
                QH_HoldAccountTableDal xh_AccHolDal = new QH_HoldAccountTableDal();
                CommonDALOperate<QH_HoldAccountTableInfo> com = new CommonDALOperate<QH_HoldAccountTableInfo>();
                //CommonDALOperate<QH_HoldAccountTableInfo>.DataReaderBind bind = xh_AccHolDal.ReaderBind;
                //tempt = com.PagingQueryProcedures(prcoInfo, out count, bind);
                tempt = com.PagingQueryProcedures(prcoInfo, out count, xh_AccHolDal.ReaderBind);

                //  tempt = DataRepository.QhHoldAccountTableProvider.GetPaged(whereCondition, "AccountHoldLogoId ASC", start, pageLength, out count);
                if (tempt.Count == 0)
                {
                    strErrorMessage = "此交易员的持仓帐户记录不存在.";
                    LogHelper.WriteDebug(desc + strErrorMessage);
                    return null;
                }
                #endregion

                #region 遍历组装返回相关数据对象 并过滤相关的查询对应条件的记录 如品种类别
                foreach (QH_HoldAccountTableInfo _QhAccountHold in tempt)
                {
                    strErrorMessage = "";
                    FuturesHoldFindResultEntity sfre = new FuturesHoldFindResultEntity();

                    sfre.HoldFindResult = _QhAccountHold;

                    #region 根据商品代码获取商品对象实体
                    CM_Commodity _CM_Commodity = MCService.CommonPara.GetCommodityByCommodityCode(_QhAccountHold.Contract);
                    #endregion

                    # region  获取品种类别并赋值
                    //品种类别 
                    #region old code
                    //int BreedClassId = Convert.ToInt32(MCService.CommonPara.GetBreedClassIdByCommodityCode(_QhAccountHold.Contract));
                    //sfre.VarietyCategories = BreedClassId.ToString();
                    #endregion
                    if (_CM_Commodity != null)
                    {
                        if (findCondition != null && !string.IsNullOrEmpty(findCondition.VarietyCode))
                        {
                            if (_CM_Commodity.BreedClassID.Value.ToString().Trim() != findCondition.VarietyCode.Trim())
                            {
                                continue;
                            }
                        }
                        sfre.VarietyCategories = _CM_Commodity.BreedClassID.Value.ToString();
                    }
                    # endregion

                    # region  获取所属市场并赋值
                    //所属市场
                    CM_BourseType _CM_BourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(_QhAccountHold.Contract);
                    if (_CM_BourseType != null)
                    {
                        sfre.BelongMarket = _CM_BourseType.BourseTypeName;
                    }
                    else
                    {
                        strErrorMessage = "根据商品代码未获取到商品所属市场 ！";
                        LogHelper.WriteDebug(desc + strErrorMessage);
                    }
                    # endregion

                    # region  获取合约名称并赋值
                    //CM_Commodity _CM_Commodity = MCService.CommonPara.GetCommodityByCommodityCode(_QhAccountHold.Contract);
                    if (_CM_Commodity != null)
                    {
                        sfre.ContractName = _CM_Commodity.CommodityName;
                    }
                    else
                    {
                        strErrorMessage = "根据商品代码未获取到现货名称 ！";
                        LogHelper.WriteDebug(desc + strErrorMessage);

                    }
                    # endregion

                    # region 获取币种名称并赋值
                    string _currencyName = MCService.CommonPara.GetCurrencyTypeByID(_QhAccountHold.TradeCurrencyType).CurrencyName;
                    if (!string.IsNullOrEmpty(_currencyName))
                    {
                        sfre.CurrencyName = _currencyName;
                    }
                    else
                    {
                        strErrorMessage = "期货持仓表中的币种类型存储错误！";
                        LogHelper.WriteDebug(desc + strErrorMessage);
                    }
                    # endregion

                    # region 获取今开仓量并赋值
                    sfre.HoldSumAmount = Convert.ToInt32(_QhAccountHold.TodayHoldAmount);
                    # endregion

                    # region 获取持仓总量并赋值
                    //if (_QhAccountHold.BuySellTypeId ==1)
                    //{
                    //    sfre.HoldSumAmount = Convert.ToInt32(_QhAccountHold.HistoryHoldAmount + _QhAccountHold.HistoryFreezeAmount + _QhAccountHold.TodayHoldAmount  + _QhAccountHold.TodayFreezeAmount);
                    //}
                    sfre.HoldSumAmount = Convert.ToInt32(_QhAccountHold.HistoryHoldAmount + _QhAccountHold.HistoryFreezeAmount + _QhAccountHold.TodayHoldAmount + _QhAccountHold.TodayFreezeAmount);
                    # endregion

                    # region 获取可用总量并赋值
                    sfre.AvailableTotalAmount = Convert.ToInt32(_QhAccountHold.HistoryHoldAmount + _QhAccountHold.TodayHoldAmount);
                    # endregion

                    # region 获取冻结总量并赋值
                    sfre.FreezeTotalAmount = Convert.ToInt32(_QhAccountHold.HistoryFreezeAmount + _QhAccountHold.TodayFreezeAmount);
                    # endregion

                    # region 获取当前价并赋值
                    //获取股指期货行情
                    //VTFutData GetFutData(string strCode);
                    //VTFutData vte = CommonDataAgent.RealtimeService.GetFutData(_QhAccountHold.Contract);
                    //if (vte != null)
                    //{
                    //    sfre.RealtimePrice = Convert.ToDecimal(vte.Lasttrade);
                    //}
                    //else
                    //    strErrorMessage = "未获取到该期货代码的行情！";

                    //decimal realtimePrice = ReturnRealtimePrice(_QhAccountHold.Contract);
                    int? breedID = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(_QhAccountHold.Contract);
                    //昨结算价
                    double preSettlementPrice = 0;
                    //今结算价
                    double settlementPrice = 0;
                    //最新价
                    double lasttrade = 0;
                    bool isGetRealtime = false;
                    if (breedID.HasValue)
                    {
                        switch ((CommonObject.Types.BreedClassTypeEnum)breedID.Value)
                        {

                            case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture:
                                FutData vte = CommonDataAgent.RealtimeService.GetFutData(_QhAccountHold.Contract);
                                if (vte == null)
                                {
                                    strErrorMessage = "【股指期货资金统计】未获取到该股指期货代码的行情,当前记录使用持仓均价计算.";
                                    LogHelper.WriteDebug("持仓账号:" + _QhAccountHold.UserAccountDistributeLogo + " 代码：" + _QhAccountHold.Contract + strErrorMessage);
                                    sfre.RealtimePrice = _QhAccountHold.HoldAveragePrice;
                                }
                                else
                                {
                                    preSettlementPrice = vte.PreSettlementPrice;
                                    settlementPrice = vte.SettlementPrice;
                                    lasttrade = vte.Lasttrade;
                                    isGetRealtime = true;
                                }
                                break;
                            case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture:
                                MerFutData mfvte = CommonDataAgent.RealtimeService.GetMercantileFutData(_QhAccountHold.Contract);
                                if (mfvte == null)
                                {
                                    strErrorMessage = "【商品期货资金统计】未获取到该商品期货代码的行情,当前记录使用持仓均价计算.";
                                    LogHelper.WriteDebug("持仓账号:" + _QhAccountHold.UserAccountDistributeLogo + " 代码：" + _QhAccountHold.Contract + strErrorMessage);
                                    sfre.RealtimePrice = _QhAccountHold.HoldAveragePrice;
                                }
                                else
                                {
                                    preSettlementPrice = mfvte.PreClearPrice;
                                    settlementPrice = mfvte.ClearPrice;
                                    lasttrade = mfvte.Lasttrade;
                                    isGetRealtime = true;
                                }
                                break;
                        }

                    }


                    if (isGetRealtime)
                    {
                        int k = MCService.IsNowTimeMarket(Types.BreedClassTypeEnum.StockIndexFuture, _QhAccountHold.Contract);

                        switch (k)
                        {
                            case 0:
                                #region 当前时间00：00：00至当日开盘前
                                if (preSettlementPrice == 0)
                                {
                                    strErrorMessage = "【期货资金统计】获取该期货代码的行情昨日结算价为0,当前记录使用持仓均价计算";
                                    LogHelper.WriteDebug("持仓账号:" + _QhAccountHold.UserAccountDistributeLogo + " 代码：" + _QhAccountHold.Contract + strErrorMessage);
                                    sfre.RealtimePrice = _QhAccountHold.HoldAveragePrice;
                                }
                                else
                                {
                                    //昨结算 PreSettlementPrice  //今结算 SettlementPrice
                                    sfre.RealtimePrice = Convert.ToDecimal(preSettlementPrice);
                                }
                                #endregion
                                break;
                            case 1:
                                #region 当前时间开盘至当日收盘
                                if (lasttrade == 0)
                                {
                                    strErrorMessage = "【期货资金统计】获取该期货代码的行情最新成交价为0,当前记录使用持仓均价计算";
                                    LogHelper.WriteDebug("持仓账号:" + _QhAccountHold.UserAccountDistributeLogo + " 代码：" + _QhAccountHold.Contract + strErrorMessage);
                                    sfre.RealtimePrice = _QhAccountHold.HoldAveragePrice;
                                }
                                else
                                {
                                    //昨结算 PreSettlementPrice  //今结算 SettlementPrice
                                    sfre.RealtimePrice = Convert.ToDecimal(lasttrade);
                                }
                                #endregion
                                break;
                            case 2:
                                #region 当日收盘后至当日23：59：59--2
                                if (settlementPrice == 0)
                                {
                                    strErrorMessage = "【期货资金统计】获取该期货代码的行情今日结算价为0,当前记录使用持仓均价计算";
                                    LogHelper.WriteDebug("持仓账号:" + _QhAccountHold.UserAccountDistributeLogo + " 代码：" + _QhAccountHold.Contract + strErrorMessage);
                                    sfre.RealtimePrice = _QhAccountHold.HoldAveragePrice;
                                }
                                else
                                {
                                    //昨结算 PreSettlementPrice  //今结算 SettlementPrice
                                    sfre.RealtimePrice = Convert.ToDecimal(settlementPrice);
                                }
                                #endregion
                                break;
                        }
                    }

                    //if (vte != null)
                    //{
                    //    if (vte.Lasttrade == 0)
                    //    {
                    //        strErrorMessage = "【期货资金统计】获取该期货代码的行情最新成交价为0,当前记录使用持仓均价计算";
                    //        sfre.RealtimePrice = _QhAccountHold.HoldAveragePrice;
                    //        LogHelper.WriteDebug("持仓账号:" + _QhAccountHold.UserAccountDistributeLogo + " 代码：" + _QhAccountHold.Contract + strErrorMessage);

                    //    }
                    //    else
                    //    {
                    //        sfre.RealtimePrice = Convert.ToDecimal(vte.Lasttrade);
                    //    }
                    //}
                    //else
                    //{
                    //    strErrorMessage = "【期货资金统计】未获取到该期货代码的行情,当前记录使用持仓均价计算.";
                    //    sfre.RealtimePrice = _QhAccountHold.HoldAveragePrice;
                    //    LogHelper.WriteDebug("持仓账号:" + _QhAccountHold.UserAccountDistributeLogo + " 代码：" + _QhAccountHold.Contract + strErrorMessage);
                    //}
                    # endregion

                    # region 获取昨日结算价并赋值
                    if (isGetRealtime)
                    {
                        sfre.YesterdayClearingPrice = Convert.ToDecimal(preSettlementPrice);//Yclose);
                    }
                    else
                    {
                        strErrorMessage = "期货未获取到该期货代码的行情--昨日结算价！";
                        LogHelper.WriteDebug(desc + strErrorMessage);
                    }
                    # endregion

                    # region 获取买卖方向并赋值
                    if (_QhAccountHold.BuySellTypeId == 1)
                        sfre.BuySellDirection = "买";
                    else
                        sfre.BuySellDirection = "卖";
                    # endregion

                    //# region 获取盯市盈亏
                    // sfre.CloseMarketProfitLoss = sfre.HoldSumAmount * (sfre.RealtimePrice - sfre.HoldAveragePrice);
                    //# endregion

                    # region 获取盯市盈亏，浮动盈亏并赋值
                    //sfre.CloseFloatProfitLoss = sfre.HoldSumAmount * (sfre.RealtimePrice - _QhAccountHold.CostPrice.Value);

                    #region 根据商品代码获取撮合（即交易单位）单位转换成计价单位的倍数
                    //把撮合（即交易）单位持仓总量转换为计价单位的持仓总量，因为之前存储到数据库中的与持仓量有关
                    //的都是交易单位量，价格相关的都是计价单位。
                    decimal scale = MCService.GetFutureTradeUntiScale(_QhAccountHold.Contract);
                    #endregion

                    //买方向的盯市盈亏
                    string buyingMarketProfitLoss = "买方向的盯市盈亏=[持仓总量={0}*(当前价={1}-持仓均价={2})*交易单位倍数={3}]";
                    //买方向的浮动盈亏
                    string buyingFloatProfitLoss = "买方向的浮动盈亏=[持仓总量={0}*(当前价={1}-开仓均价={2})*交易单位倍数={3}]";
                    //卖方向的盯市盈亏
                    string sellMarketProfitLoss = "卖方向的盯市盈亏=[持仓总量={0}*(持仓均价={1}-当前价={2})*交易单位倍数={3}]";
                    //卖方向的浮动盈亏
                    string sellFloatProfitLoss = "卖方向的浮动盈亏=[持仓总量={0}*(开仓均价={1}-当前价={2})*交易单位倍数={3}]";
                    //计算计价持仓总量
                    decimal computeTotal = sfre.HoldSumAmount * scale;

                    if (_QhAccountHold.BuySellTypeId == (int)CommonObject.Types.TransactionDirection.Buying)
                    {
                        //sfre.CloseMarketProfitLoss = sfre.HoldSumAmount * (sfre.RealtimePrice - sfre.HoldAveragePrice) * scale;
                        //sfre.FloatProfitLoss = sfre.HoldSumAmount * (sfre.RealtimePrice - _QhAccountHold.CostPrice) * scale;
                        // 买方向的盯市盈亏=买方向的盯市盈亏=[持仓总量={0}*(当前价={1}-持仓均价={2})*交易单位倍数={3}]
                        //sfre.MarketProfitLoss = computeTotal * (sfre.RealtimePrice - _QhAccountHold.HoldAveragePrice) * scale;
                        //以上总量已经转为计价单位总量所以不用再乘以倍数
                        sfre.MarketProfitLoss = computeTotal * (sfre.RealtimePrice - _QhAccountHold.HoldAveragePrice);
                        //买方向的浮动盈亏=买方向的浮动盈亏=[持仓总量={0}*(当前价={1}-开仓均价={2})*交易单位倍数={3}]
                        sfre.FloatProfitLoss = computeTotal * (sfre.RealtimePrice - _QhAccountHold.OpenAveragePrice);

                        //打印买方向的盯市盈亏
                        string buyMarketP = string.Format(buyingMarketProfitLoss, computeTotal, sfre.RealtimePrice, _QhAccountHold.HoldAveragePrice, scale);
                        LogHelper.WriteDebug(buyMarketP);
                        //打印买方向的浮动盈亏
                        string buyFloatP = string.Format(buyingFloatProfitLoss, computeTotal, sfre.RealtimePrice, _QhAccountHold.OpenAveragePrice, scale);
                        LogHelper.WriteDebug(buyFloatP);
                    }
                    else
                    {
                        //卖方向的盯市盈亏=卖方向的盯市盈亏=[持仓总量={0}*(持仓均价={1}-当前价={2})*交易单位倍数={3}]
                        sfre.MarketProfitLoss = computeTotal * (_QhAccountHold.HoldAveragePrice - sfre.RealtimePrice);
                        //卖方向的浮动盈亏=卖方向的浮动盈亏=[持仓总量={0}*(开仓均价={1}-当前价={2})*交易单位倍数={3}]
                        sfre.FloatProfitLoss = computeTotal * (_QhAccountHold.OpenAveragePrice - sfre.RealtimePrice);

                        //打印卖方向的盯市盈亏
                        string sellMarketP = string.Format(sellMarketProfitLoss, computeTotal, _QhAccountHold.HoldAveragePrice, sfre.RealtimePrice, scale);
                        LogHelper.WriteDebug(sellMarketP);
                        //打印卖方向的浮动盈亏
                        string sellFloatP = string.Format(sellFloatProfitLoss, computeTotal, _QhAccountHold.OpenAveragePrice, sfre.RealtimePrice, scale);
                        LogHelper.WriteDebug(sellFloatP);

                    }

                    # endregion

                    # region 获取持仓均价并赋值
                    sfre.HoldAveragePrice = _QhAccountHold.HoldAveragePrice;
                    # endregion

                    # region 获取今日仓均价并赋值
                    sfre.TodayOpenAveragePrice = _QhAccountHold.TodayHoldAveragePrice;
                    # endregion

                    # region 获取交易员ID并赋值
                    string _userId = string.Empty;
                    _userId = GetUserIdByTradeAccount(capitalAccount);
                    if (!string.IsNullOrEmpty(_userId))
                    {
                        sfre.TraderId = _userId;
                    }
                    else
                    {
                        strErrorMessage = "未获取到该资金账号所对应的交易员ID ！";
                        LogHelper.WriteDebug(desc + strErrorMessage);
                    }
                    # endregion

                    # region 获取错误原因并赋值
                    if (!string.IsNullOrEmpty(strErrorMessage))
                    {
                        string errtxt = strErrorMessage;
                        sfre.ErroReason = errtxt;
                    }
                    # endregion

                    # region 获取错误号并赋值（还未实现，暂时赋为空值）
                    sfre.ErroNumber = string.Empty;
                    # endregion

                    #region 成本价,保证金,今日开仓量,保本价(liushuwei)
                    //成本价
                    sfre.CostPrice = Convert.ToDecimal(_QhAccountHold.CostPrice);
                    //保证金
                    sfre.MarginAmount = Convert.ToDecimal(_QhAccountHold.Margin);
                    //今日开仓量
                    sfre.TodayAOpenAmount = Convert.ToInt32(_QhAccountHold.TodayHoldAmount);
                    //保本价
                    sfre.BreakevenPrice = Convert.ToDecimal(_QhAccountHold.BreakevenPrice);
                   
                    #endregion
                    //交易单位计价单位倍数 add by 董鹏 2010-05-04
                    sfre.UnitMultiple = scale;

                    result.Add(sfre);
                }
                #endregion
            }
            catch (Exception ex)
            {
                strErrorMessage = "期货持仓查询,错误原因" + ex.ToString();
                // LogHelper.WriteDebug(desc + strErrorMessage);
                LogHelper.WriteError(strErrorMessage, ex);
            }
            return result;
        }

        # endregion 现货持仓查询

        # region 银行资金明细查询
        /// <summary>
        /// 银行资金明细查询
        /// </summary>
        /// <param name="userId">交易员ID</param>
        /// <param name="bankAccount">银行账号</param>
        /// <param name="outMessage">输出参数</param>
        /// <returns></returns>
        public List<UA_BankAccountTableInfo> BankCapitalFind(string userId, string bankAccount, out string outMessage)
        {
            List<UA_BankAccountTableInfo> list = null;
            outMessage = null;
            try
            {
                string _bankAccount = string.Empty;
                // 通过交易员ID获得该交易员的银行账号
                _bankAccount = GetBankAccountByUserId(userId);
                if (string.IsNullOrEmpty(bankAccount) || bankAccount == _bankAccount)
                {
                    #region old code
                    //string whereCondition = string.Format("UserAccountDistributeLogo ='{0}'", _bankAccount);
                    //result = DataRepository.UaBankAccountTableProvider.GetPaged(whereCondition, "TradeCurrencyTypeLogo ASC", 0, 15, out count);
                    #endregion
                    UA_BankAccountTableDal dal = new UA_BankAccountTableDal();
                    list = dal.GetListArray(string.Format("UserAccountDistributeLogo ='{0}'", _bankAccount));
                }
                else
                {
                    outMessage = "对不起，查询失败！失败原因为：交易员的银行账号输入不正确 ！";
                }
            }
            catch (Exception ex)
            {
                outMessage = ex.ToString();
            }
            return list;
        }
        # endregion 银行资金明细查询

        # region  （NEW）单个现货资金账号下的资产汇总查询（根据资金账号查询）
        /// <summary>
        /// 单个现货资金账号下的资产汇总查询（根据资金账号查询）
        /// Update By:李健华
        /// Update Date:2009-07-15
        /// Desc.:修改相关的操作逻辑，精简操作
        /// </summary>
        /// <returns></returns>
        public List<AssetSummaryEntity> SpotAssetSummaryFind(string capitalAccount, string strPassword, out string strErrorMessage)
        {
            #region 变量定义
            List<AssetSummaryEntity> list = new List<AssetSummaryEntity>();
            strErrorMessage = string.Empty;
            AssetSummaryEntity temp = new AssetSummaryEntity();
            #endregion

            # region try....catch 语句
            try
            {
                #region 根据现货资金账户获取关联的持仓账户信息
                UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(capitalAccount);
                //UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();
                //UA_UserAccountAllocationTableInfo userInfo = userDal.GetUserHoldAccountByUserCapitalAccount(capitalAccount);
                if (userInfo == null)
                {
                    strErrorMessage = "查询不到对应资金账号所对应的交易员相关信息！";
                    return list;
                }
                #endregion

                #region 根据现货资金账号返回现货资金账号所拥有的资金信息
                List<XH_CapitalAccountTableInfo> xh_AccountInfo_list = QueryXH_CapitalAccountByAccount(capitalAccount, QueryType.QueryCurrencyType.ALL, out strErrorMessage);
                #endregion

                #region 获取交易货币为【RMB】的总市值、总浮动盈亏(未实现盈亏)
                decimal RMBSum;
                decimal floatRMBSum;
                RMBSum = GetMarketValueByXH_HoldAccount(userInfo.UserAccountDistributeLogo, Types.CurrencyType.RMB, out floatRMBSum);
                #endregion

                #region 获取交易货币为【HK】的总市值、总浮动盈亏(未实现盈亏)
                decimal HKSum;
                decimal floatHKSum;
                HKSum = GetMarketValueByXH_HoldAccount(userInfo.UserAccountDistributeLogo, Types.CurrencyType.HK, out floatHKSum);
                #endregion

                #region 获取交易货币为【US】的总市值、总浮动盈亏(未实现盈亏)
                decimal USSum;
                decimal floatUSSum;
                USSum = GetMarketValueByXH_HoldAccount(userInfo.UserAccountDistributeLogo, Types.CurrencyType.US, out floatUSSum);
                #endregion

                #region 遍历资金账户信息查询相关交易货币分类汇总相关资产
                foreach (XH_CapitalAccountTableInfo item in xh_AccountInfo_list)
                {
                    switch ((Types.CurrencyType)item.TradeCurrencyType)
                    {
                        case Types.CurrencyType.RMB:
                            {
                                temp.RMBAvailable = RMBSum + item.AvailableCapital;
                                temp.RMBFreeze = item.FreezeCapitalTotal;
                                //// RMB未实现盈亏(浮动盈亏)=汇总所有持仓表中的浮动盈亏
                                //temp.RMBNotDoneProfitLossTotal = floatRMBSum;
                                ////RMB总盈亏=累计已实现盈亏+未实现盈亏
                                //temp.RMBProfitLossTotal = floatRMBSum + item.HasDoneProfitLossTotal;
                            }
                            break;
                        case Types.CurrencyType.HK:
                            {
                                temp.HKAvailable = HKSum + item.AvailableCapital;
                                temp.HKFreeze = item.FreezeCapitalTotal;
                                ////HK未实现盈亏(浮动盈亏)=汇总所有持仓表中的浮动盈亏
                                //temp.HKNotDoneProfitLossTotal = floatHKSum;
                                ////HK总盈亏=累计已实现盈亏+未实现盈亏
                                //temp.HKProfitLossTotal = floatHKSum + item.HasDoneProfitLossTotal;
                            }
                            break;
                        case Types.CurrencyType.US:
                            {
                                temp.USAvailable = USSum + item.AvailableCapital;
                                temp.USFreeze = item.FreezeCapitalTotal;
                                ////US未实现盈亏(浮动盈亏)=汇总所有持仓表中的浮动盈亏
                                //temp.USNotDoneProfitLossTotal = floatUSSum;
                                ////US总盈亏=累计已实现盈亏+未实现盈亏
                                //temp.USProfitLossTotal = floatUSSum + item.HasDoneProfitLossTotal;
                            }
                            break;
                        default:
                            break;
                    }

                }
                #endregion
                list.Add(temp);

                #region old Code

                ////SpotCapitalEntity _SpotCapitalEntity= new SpotCapitalEntity( );
                //var temp = new AssetSummaryEntity();
                ////temp.CapitalAccount = capitalAccount;

                ////1.获得该资金账号下的RMB(人民币）的持仓市值
                //decimal RMBSum;
                //decimal floatRMBSum;//浮动盈亏
                //RMBSum = GetMarketValueByXhFundAccount(capitalAccount, Types.CurrencyType.RMB, out floatRMBSum);

                ////现货资金明细查询(通过资金账号和币种）
                ////public SpotCapitalEntity SpotCapitalFind(string strFundAccountId,Types.CurrencyType currencyType, string userPassword,ref string strErrorMessage)
                //var _capitalAccountRMB = DataRepository.XhCapitalAccountTableProvider.Find(
                //    string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", capitalAccount, 1));
                //decimal rmb = _capitalAccountRMB[0].AvailableCapital.Value;
                //temp.RMBAvailable = RMBSum + rmb;
                //temp.RMBFreeze = _capitalAccountRMB[0].FreezeCapitalTotal.Value;

                ////2.获得该资金账号下的HK（港币）的持仓市值
                //decimal HKSum;
                //decimal floatHKSum;//浮动盈亏
                //HKSum = GetMarketValueByXhFundAccount(capitalAccount, Types.CurrencyType.HK, out floatHKSum);
                //var _capitalAccountHK = DataRepository.XhCapitalAccountTableProvider.Find(
                //   string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", capitalAccount, 2));
                //decimal hk = _capitalAccountHK[0].AvailableCapital.Value;
                //temp.HKAvailable = HKSum + hk;
                //temp.HKFreeze = _capitalAccountHK[0].FreezeCapitalTotal.Value;

                ////3.获得该资金账号下的US（港币）的持仓市值
                //decimal USSum;
                //decimal floatUSSum;//浮动盈亏
                //USSum = GetMarketValueByXhFundAccount(capitalAccount, Types.CurrencyType.US, out floatUSSum);
                //var _capitalAccountUS = DataRepository.XhCapitalAccountTableProvider.Find(
                //   string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", capitalAccount, 3));
                //decimal US = _capitalAccountUS[0].AvailableCapital.Value;
                //temp.USAvailable = USSum + US;
                //temp.USFreeze = _capitalAccountUS[0].FreezeCapitalTotal.Value;

                //list.Add(temp);
                #endregion
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
            }
            # endregion try....catch 语句
            return list;
        }
        # endregion

        # region  （NEW）单个期货资金账号下的资产汇总查询（根据资金账号查询）
        /// <summary>
        /// 单个期货资金账号下的资产汇总查询（根据资金账号查询）
        /// Update By:李健华
        /// Update Date:2009-07-15
        /// Desc.:修改相关的操作逻辑，精简操作
        /// </summary>
        /// <param name="capitalAccount">期货资金账号</param>
        /// <param name="strErrorMessage">统计异常</param>
        /// <param name="strPassword">用户密码</param>
        /// <returns></returns>
        public List<AssetSummaryEntity> FuturesAssetSummaryFind(string capitalAccount, string strPassword, out string strErrorMessage)
        {
            #region 变量定义
            List<AssetSummaryEntity> list = new List<AssetSummaryEntity>();
            strErrorMessage = string.Empty;
            AssetSummaryEntity temp = new AssetSummaryEntity();
            #endregion

            # region try....catch 语句
            try
            {

                #region 根据期货资金账户获取关联的持仓账户信息
                #region 从缓存中获取用户账号信息
                //UA_UserAccountAllocationTableDal userDal = new UA_UserAccountAllocationTableDal();
                //UA_UserAccountAllocationTableInfo userInfo = userDal.GetUserHoldAccountByUserCapitalAccount(capitalAccount);
                UA_UserAccountAllocationTableInfo userInfo = AccountManager.Instance.GetHoldAccountByCapitalAccount(capitalAccount);
                #endregion
                if (userInfo == null)
                {
                    strErrorMessage = "查询不到对应资金账号所对应的交易员相关信息！";
                    return list;
                }
                #endregion

                #region 根据期货资金账号返回期货资金账号所拥有的资金信息 这里内部方法已经实现从缓存中获取数据
                List<QH_CapitalAccountTableInfo> qh_AccountInfo_list = QueryQH_CapitalAccountByAccount(capitalAccount, QueryType.QueryCurrencyType.ALL, out strErrorMessage);
                #endregion

                #region 获取交易货币为【RMB】的总市值、总浮动盈亏、总盯市盈亏
                decimal RMBSum;//RMB货币类型的总市值
                decimal floatProfitLossTotalRMB = 0;//总浮动盈亏
                decimal marketProfitLossTotalRMB = 0;//总盯市盈亏
                RMBSum = GetMarketValueByQH_HoldAccount(userInfo.UserAccountDistributeLogo, Types.CurrencyType.RMB, out floatProfitLossTotalRMB, out marketProfitLossTotalRMB);
                #endregion

                #region 获取交易货币为【HK】的总市值、总浮动盈亏、总盯市盈亏
                decimal HKSum;//HK货币类型的总市值
                decimal floatProfitLossTotalHK = 0;
                decimal marketProfitLossTotalHK = 0;
                HKSum = GetMarketValueByQH_HoldAccount(userInfo.UserAccountDistributeLogo, Types.CurrencyType.HK, out floatProfitLossTotalHK, out marketProfitLossTotalHK);
                #endregion

                #region 获取交易货币为【US】的总市值、总浮动盈亏、总盯市盈亏
                decimal USSum;//US货币类型的总市值
                decimal floatProfitLossTotalUS = 0;
                decimal marketProfitLossTotalUS = 0;
                USSum = GetMarketValueByQH_HoldAccount(userInfo.UserAccountDistributeLogo, Types.CurrencyType.US, out floatProfitLossTotalUS, out marketProfitLossTotalUS);
                #endregion

                #region 遍历资金账户信息查询相关交易货币分类汇总相关资产
                foreach (QH_CapitalAccountTableInfo item in qh_AccountInfo_list)
                {
                    switch ((Types.CurrencyType)item.TradeCurrencyType)
                    {
                        case Types.CurrencyType.RMB:
                            {
                                temp.RMBAvailable = RMBSum + item.AvailableCapital;
                                temp.RMBFreeze = item.FreezeCapitalTotal;
                                ////货币RMB类型的总浮动盈亏
                                //temp.FloatProfitLossTotalRMB = floatProfitLossTotalRMB;
                                ////货币RMB类型的总盯市盈亏
                                //temp.MarketProfitLossTotalRMB = marketProfitLossTotalRMB;
                            }
                            break;
                        case Types.CurrencyType.HK:
                            {
                                temp.HKAvailable = HKSum + item.AvailableCapital;
                                temp.HKFreeze = item.FreezeCapitalTotal;
                                ////货币HK类型的总浮动盈亏
                                //temp.FloatProfitLossTotalHK = floatProfitLossTotalHK;
                                ////货币HK类型的总盯市盈亏
                                //temp.MarketProfitLossTotalHK = marketProfitLossTotalHK;
                            }
                            break;
                        case Types.CurrencyType.US:
                            {
                                temp.USAvailable = USSum + item.AvailableCapital;
                                temp.USFreeze = item.FreezeCapitalTotal;
                                ////货币US类型的总浮动盈亏
                                //temp.FloatProfitLossTotalUS = floatProfitLossTotalUS;
                                ////货币US类型的总盯市盈亏
                                //temp.MarketProfitLossTotalUS = marketProfitLossTotalUS;
                            }
                            break;
                        default:
                            break;
                    }

                }
                #endregion

                list.Add(temp);

                #region old Cdoe
                //var temp = new AssetSummaryEntity();
                ////temp.CapitalAccount = capitalAccount;

                ////1.获得该资金账号下的RMB(人民币）的持仓市值
                //decimal RMBSum;
                //decimal floatProfitLossTotalRMB = 0;
                //decimal marketProfitLossTotalRMB = 0;
                //RMBSum = GetMarketValueByQhFundAccount(capitalAccount, Types.CurrencyType.RMB, out floatProfitLossTotalRMB, out marketProfitLossTotalRMB);

                ////现货资金明细查询(通过资金账号和币种）
                ////public SpotCapitalEntity SpotCapitalFind(string strFundAccountId,Types.CurrencyType currencyType, string userPassword,ref string strErrorMessage)
                //var _capitalAccountRMB = DataRepository.QhCapitalAccountTableProvider.Find(
                //    string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", capitalAccount, 1));
                //decimal rmb = _capitalAccountRMB[0].AvailableCapital.Value;
                //temp.RMBAvailable = RMBSum + rmb;
                //temp.RMBFreeze = _capitalAccountRMB[0].FreezeCapitalTotal.Value;

                ////2.获得该资金账号下的HK（港币）的持仓市值
                //decimal HKSum;
                //decimal floatProfitLossTotalHK = 0;
                //decimal marketProfitLossTotalHK = 0;
                //HKSum = GetMarketValueByQhFundAccount(capitalAccount, Types.CurrencyType.HK, out floatProfitLossTotalHK, out marketProfitLossTotalHK);
                //var _capitalAccountHK = DataRepository.QhCapitalAccountTableProvider.Find(
                //   string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", capitalAccount, 2));
                //decimal hk = _capitalAccountHK[0].AvailableCapital.Value;
                //temp.HKAvailable = HKSum + hk;
                //temp.HKFreeze = _capitalAccountHK[0].FreezeCapitalTotal.Value;

                ////3.获得该资金账号下的US（港币）的持仓市值
                //decimal USSum;
                //decimal floatProfitLossTotalUS = 0;
                //decimal marketProfitLossTotalUS = 0;
                //USSum = GetMarketValueByQhFundAccount(capitalAccount, Types.CurrencyType.US, out floatProfitLossTotalUS, out marketProfitLossTotalUS);
                //var _capitalAccountUS = DataRepository.QhCapitalAccountTableProvider.Find(
                //   string.Format("UserAccountDistributeLogo='{0}' AND TradeCurrencyType ='{1}'", capitalAccount, 3));
                //decimal US = _capitalAccountUS[0].AvailableCapital.Value;
                //temp.USAvailable = USSum + US;
                //temp.USFreeze = _capitalAccountUS[0].FreezeCapitalTotal.Value;

                //result.Add(temp);
                #endregion
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.ToString();
            }
            # endregion
            return list;
        }
        # endregion

        # region  （NEW）资产汇总查询（根据交易员ID查询）
        /// <summary>
        /// 资产汇总查询
        /// Update by:李健华
        /// Update date:2009-07-28
        /// Desc.:修改一些DAL操作和相关的Bug,以及代码逻辑规范,并修改获取用户信息从数据缓存中获取
        /// </summary>
        /// <param name="findAccount">要汇总资产查询账户的实体</param>
        /// <returns></returns>
        public List<AssetSummaryEntity> AssetSummaryFind(FindAccountEntity findAccount, out  string outMessage)
        {
            outMessage = string.Empty;

            #region 变量定义
            List<AssetSummaryEntity> list = new List<AssetSummaryEntity>();
            AssetSummaryEntity model = new AssetSummaryEntity();
            AccountManager am = AccountManager.Instance;
            decimal RMBAvailble = 0;
            decimal HKAvailble = 0;
            decimal USAvailble = 0;
            decimal RBMFreeze = 0;
            decimal HKFreeze = 0;
            decimal USFreeze = 0;
            #endregion

            #region 获得该交易员的所有资金账号
            #region 从缓存中获取数据
            //获得该交易员的银行资金账号
            var _bankCapitalAccount = am.GetAccountByUserIDAndAccountTypeClass(findAccount.UserID, Types.AccountAttributionType.BankAccount);

            //获得该交易员的所有现货资金账号
            var _XH_CapitalAccount = am.GetAccountByUserIDAndAccountTypeClass(findAccount.UserID, Types.AccountAttributionType.SpotCapital);

            //获得该交易员的所有期货资金账号
            var _QH_CapitalAccount = am.GetAccountByUserIDAndAccountTypeClass(findAccount.UserID, Types.AccountAttributionType.FuturesCapital);
            #endregion

            #region 从数据库中获取数据
            ////获得该交易员的银行资金账号
            //string _bankCapitalAccount = aa.Find_UserBankCapitalAccount(findAccount.UserID);

            ////获得该交易员的所有现货资金账号
            //string[] _XH_CapitalAccount = aa.Find_UserXHCapitalAccount(findAccount.UserID);

            ////获得该交易员的所有期货资金账号
            //string[] _QH_CapitalAccount = aa.Find_UserQHCapitalAccount(findAccount.UserID);
            #endregion
            #endregion

            # region 银行资金汇总
            if (!Utils.IsNullOrEmpty(_bankCapitalAccount))
            {
                var Bank = BankCapitalFind(findAccount.UserID, _bankCapitalAccount[0].UserAccountDistributeLogo, out outMessage);
                foreach (UA_BankAccountTableInfo item in Bank)
                {
                    if (item.TradeCurrencyTypeLogo == 1)
                    {
                        RMBAvailble += item.AvailableCapital;
                        RBMFreeze += item.FreezeCapital;
                    }
                    else if (item.TradeCurrencyTypeLogo == 2)
                    {
                        HKAvailble += item.AvailableCapital;
                        HKFreeze += item.FreezeCapital;
                    }
                    else
                    {
                        USAvailble += item.AvailableCapital;
                        USFreeze += item.FreezeCapital;
                    }
                }
            }
            # endregion

            # region 现货资金汇总
            foreach (UA_UserAccountAllocationTableInfo xhcapitalAccount in _XH_CapitalAccount)
            {
                var Xh = SpotAssetSummaryFind(xhcapitalAccount.UserAccountDistributeLogo, "", out outMessage);
                foreach (AssetSummaryEntity item in Xh)
                {
                    RMBAvailble += item.RMBAvailable;
                    RBMFreeze += item.RMBFreeze;

                    HKAvailble += item.HKAvailable;
                    HKFreeze += item.HKFreeze;

                    USAvailble += item.USAvailable;
                    USFreeze += item.USFreeze;
                }
            }
            # endregion

            # region 期货资金汇总
            foreach (UA_UserAccountAllocationTableInfo qhCapitalAccount in _QH_CapitalAccount)
            {
                var Qh = FuturesAssetSummaryFind(qhCapitalAccount.UserAccountDistributeLogo, "", out outMessage);
                foreach (AssetSummaryEntity item in Qh)
                {
                    RMBAvailble += item.RMBAvailable;
                    RBMFreeze += item.RMBFreeze;

                    HKAvailble += item.HKAvailable;
                    HKFreeze += item.HKFreeze;

                    USAvailble += item.USAvailable;
                    USFreeze += item.USFreeze;
                }
            }
            # endregion

            #region 汇总相关数据组装实体返回
            //交易员ID
            model.UserID = findAccount.UserID;

            //RMB的汇总情况
            model.RMBAvailable = RMBAvailble;
            model.RMBFreeze = RBMFreeze;

            //港币的汇总情况
            model.HKAvailable = HKAvailble;
            model.HKFreeze = HKFreeze;

            //美元的汇总情况
            model.USAvailable = USAvailble;
            model.USFreeze = USFreeze;

            list.Add(model);
            #endregion
            return list;
        }
        # endregion

        #region Create by :李健华 2009-07-12

        #region 现货持仓、持仓冻结查询

        #region 现货持仓明细查询

        #region 根据【用户ID查询】用户所拥有的现货持仓账号明细
        /// <summary>
        /// 根据【用户ID查询】用户所拥有的public货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">要查询的货币类型</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_AccountHoldTableInfo> QueryXH_AccountHoldByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencytype, out string errorMsg)
        {
            errorMsg = "";
            List<XH_AccountHoldTableInfo> list = new List<XH_AccountHoldTableInfo>();

            #region 从数据库中取数据
            //try
            //{ 
            //    XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            //    list = dal.GetListByUserID(userID, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 先通过用户ID取得用户的现货持仓账号
                // UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();

                #region 如果为0就查询类别下的所有可能有两个账号

                #region 从缓存中获取账号
                userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 1);
                #endregion

                #region 直接从数据库中获取
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotHold);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion
                #endregion

                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetXH_AccountHoldInfoListFromMemory(item.UserAccountDistributeLogo, currencytype, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;

        }
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的现货持仓账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的现货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">要查询的货币类型</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_AccountHoldTableInfo> QueryXH_AccountHoldByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencytype, out string errorMsg)
        {
            errorMsg = "";
            List<XH_AccountHoldTableInfo> list = new List<XH_AccountHoldTableInfo>();

            #region 从数据库中获取数据
            //try
            //{
            //    XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            //    list = dal.GetListByUserIDAndPwd(userID, pwd, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 检查用户密码
                //update 2009-11-25 李健华
                //var userDal = new UA_UserBasicInformationTableDal();
                //if (!userDal.Exists(userID, pwd))
                //{
                //    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                //    return list;
                //}
                //==============
                //====new===
                if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
                {
                    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                    return list;
                }
                //==========end==========

                #endregion

                #region 先通过用户ID取得用户的现货持仓资金账号
                // UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 如果为0就查询类别下的所有可能有两个账号
                #region 从缓存中获取账号
                userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 1);
                #endregion
                #endregion

                #region 直接从数据库中获取
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotHold);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetXH_AccountHoldInfoListFromMemory(item.UserAccountDistributeLogo, currencytype, out errorMsg));
                }
                #endregion

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return list;
        }

        #endregion

        #region 根据【现货持仓账号】查询现货持仓账号明细
        /// <summary>
        /// 根据【现货持仓账号】查询现货持仓账号明细
        /// </summary>
        ///<param name="xh_hold_Account">期货持仓账号</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_AccountHoldTableInfo> QueryXH_AccountHoldByAccount(string xh_hold_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_AccountHoldTableInfo> list = new List<XH_AccountHoldTableInfo>();

            #region 从数据库中获取数据
            //try
            //{ 
            //    XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
            //    list = dal.GetListByAccount(xh_Cap_Account, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            #region 从内存表中获取数据
            list = GetXH_AccountHoldInfoListFromMemory(xh_hold_Account, currencyType, out errorMsg);
            #endregion
            return list;
        }
        #endregion

        #region 根据现货持仓账号和查询交易货币类型从内存表中获取现货持仓表数据列表
        /// <summary>
        /// 根据现货持仓账号和查询交易货币类型从内存表中获取现货持仓表数据列表
        /// </summary>
        /// <param name="xh_Hold_Account">现货持仓账号</param>
        /// <param name="currencyType">交易货币类型</param>
        /// <param name="errorMsg">查询异常</param>
        /// <returns></returns>
        List<XH_AccountHoldTableInfo> GetXH_AccountHoldInfoListFromMemory(string xh_Hold_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_AccountHoldTableInfo> list = new List<XH_AccountHoldTableInfo>();
            if (!ServerConfig.IsLoadAllData)
            {
                #region 没有加载缓存持仓，从数据库中查询
                try
                {
                    //string messStr = "GetXH_AccountHoldInfoListFromMemory===正在查询现货账号" + xh_Hold_Account + "的持仓,因为没有加载持仓表数据正在从数据库中查询，查询时间" + DateTime.Now.ToString();
                    //LogHelper.WriteInfo(messStr);

                    XH_AccountHoldTableDal dal = new XH_AccountHoldTableDal();
                    list = dal.GetListByAccount(xh_Hold_Account, currencyType);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            else
            {
                #region 直接从缓存中查询
                var manager = MemoryDataManager.XHHoldMemoryList;
                if (manager == null)
                {
                    errorMsg = "还没有初始化缓存数据对象！";
                    return list;
                }
                IList<int> listID = manager.GetAccountHoldLogoID(xh_Hold_Account);
                if (Utils.IsNullOrEmpty(listID))
                {
                    return list;
                }
                if (currencyType == QueryType.QueryCurrencyType.ALL)
                {
                    foreach (int item in listID)
                    {
                        XHHoldMemoryTable table = manager.GetByAccountHoldLogoId(item);
                        if (table != null)
                        {
                            var info = table.Data;
                            list.Add(info);
                        }
                    }
                }
                else
                {
                    foreach (int item in listID)
                    {
                        XHHoldMemoryTable table = manager.GetByAccountHoldLogoId(item);

                        if (table != null)
                        {
                            var info = table.Data;
                            if (info.CurrencyTypeId == (int)currencyType)
                            {
                                list.Add(info);
                            }
                        }
                    }
                }
                #endregion
            }

            return list;
        }
        #endregion
        #endregion

        #region 现货持仓冻结明细查询

        #region 根据委托编号查询【现货持仓冻结表】明细
        /// <summary>
        /// 根据委托编号查询【现货持仓冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<XH_AcccountHoldFreezeTableInfo> QueryXH_AcccountHoldFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_AcccountHoldFreezeTableInfo> list = null;
            if (string.IsNullOrEmpty(entrustNo))
            {
                errorMsg = "委托编号不能为空！";
                return list;
            }
            XH_AcccountHoldFreezeTableDal dal = new XH_AcccountHoldFreezeTableDal();
            try
            {
                StringBuilder sb = new StringBuilder("  EntrustNumber='" + entrustNo.Trim() + "'");
                //如果查询的冻结类型不是查询所有时加上条件
                if (freezeType != QueryType.QueryFreezeType.ALL)
                {
                    sb.AppendFormat("  AND FreezeTypeLogo='{0}'", (int)freezeType);
                }
                list = dal.GetListArray(sb.ToString());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据用户持仓账号和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细信息
        /// <summary>
        /// Title：根据用户持仓账号和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="account">持仓账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_AcccountHoldFreezeTableInfo> PagingQueryXH_AcccountHoldFreezeByAccount(string holdAccount, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<XH_AcccountHoldFreezeTableInfo> list = null;
            XH_AcccountHoldFreezeTableDal dal = new XH_AcccountHoldFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果持仓账号或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(holdAccount))
            {
                errorMsg = "持仓账号不能为空！";
                return list;
            }
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = "  f.HoldFreezeLogoId,f.EntrustNumber,f.prepareFreezeAmount,f.FreezeTypeLogo,f.AccountHoldLogo,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.HoldFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "XH_AcccountHoldFreezeTable as f  inner join XH_AccountHoldTable as a on a.AccountHoldLogoId=f.AccountHoldLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            //1=1 and a.UserAccountDistributeLogo='010000000406' and a.TradeCurrencyType=1
            sb.Append(" a.UserAccountDistributeLogo='" + holdAccount + "'  ");

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.CurrencyTypeId='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<XH_AcccountHoldFreezeTableInfo> com = new CommonDALOperate<XH_AcccountHoldFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingXH_AcccountHoldFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion

        }

        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【现货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="userID">持仓账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_AcccountHoldFreezeTableInfo> PagingQueryXH_AcccountHoldFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<XH_AcccountHoldFreezeTableInfo> list = null;
            XH_AcccountHoldFreezeTableDal dal = new XH_AcccountHoldFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "用户ID不能为空！";
                return list;
            }
            //如果日后要加上用户和密码验证在此加上
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = "  f.HoldFreezeLogoId,f.EntrustNumber,f.prepareFreezeAmount,f.FreezeTypeLogo,f.AccountHoldLogo,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.HoldFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "XH_AcccountHoldFreezeTable as f  inner join XH_AccountHoldTable as a on a.AccountHoldLogoId=f.AccountHoldLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();

            #region 从缓存中获取账号

            List<UA_UserAccountAllocationTableInfo> userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 1);
            #region 添加数据
            string userIDstr = "";
            foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
            {
                userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
            }
            if (!string.IsNullOrEmpty(userIDstr))
            {
                userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
            }

            #endregion
            #endregion

            #region 直接从数据库中获取
            if (!string.IsNullOrEmpty(userIDstr))
            {
                if (userIDstr.Split(',').Length > 1)
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo  in ( {0} )", userIDstr);
                }
                else
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo = {0} ", userIDstr);
                }
            }
            else //如果在缓存中获取不到数据直接从数据库中获取数据
            {

                //找不到用户不在数据库中查找了,这里不再管了
                sb.Append("  a.UserAccountDistributeLogo=''  ");//这里加这个是为了后面的条件
                //if (accountType == 0)
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.SpotHold + "')  and userid='" + userID + "' )");
                //}
                //else
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "' and userid='" + userID + "' )");
                //}
            }
            #endregion

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.CurrencyTypeId='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<XH_AcccountHoldFreezeTableInfo> com = new CommonDALOperate<XH_AcccountHoldFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingXH_AcccountHoldFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion
        }
        #endregion
        #endregion

        #endregion

        #region 期货持仓、持仓冻结查询

        #region 期货持仓明细查询

        #region 根据【用户ID查询】用户所拥有的期货持仓账号明细
        /// <summary>
        /// 根据【用户ID查询】用户所拥有的public货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> QueryQH_HoldAccountByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();

            #region 从数据库中获取数据
            //try
            //{
            //    QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            //    list = dal.GetListByUserID(userID, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            try
            {
                #region 先通过用户ID取得用户的期货持仓资金账号
                // UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取账号
                userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 0);
                #endregion
                #region 从数据库中获取账号
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesHold);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetQH_HoldAccountInfoListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;

        }
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的期货持仓账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的期货持仓账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> QueryQH_HoldAccountByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
            #region 从数据库中获取数据
            //try
            //{
            //    QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            //    list = dal.GetListByUserIDAndPwd(userID, pwd, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 检查用户密码

                //update 2009-11-25 李健华
                //var userDal = new UA_UserBasicInformationTableDal();
                //if (!userDal.Exists(userID, pwd))
                //{
                //    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                //    return list;
                //}
                //==============
                //====new===
                if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
                {
                    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                    return list;
                }
                //==========end==========

                #endregion

                #region 先通过用户ID取得用户的期货持仓资金账号
                // UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取账号
                userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 0);
                #endregion
                #region 从数据库中获取账号
                //if (accountType == 0) //为0时可能有相关多个账户港股或者证券
                //{
                //    dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesHold);
                //}
                //else //不为0时查询一个账户的相关信息
                //{
                //    dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetQH_HoldAccountInfoListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        #endregion

        #region 根据【期货持仓账号】查询期货持仓账号明细
        /// <summary>
        /// 根据【期货持仓账号】查询期货持仓账号明细
        /// </summary>
        ///<param name="qh_Cap_Account">期货持仓账号</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_HoldAccountTableInfo> QueryQH_HoldAccountByAccount(string qh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
            #region 从数据库中取数据
            //try
            //{
            //    QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            //    list = dal.GetListByAccount(xh_Cap_Account, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            #region 从内存表中获取数据

            list = GetQH_HoldAccountInfoListFromMemory(qh_Cap_Account, currencyType, out errorMsg);

            #endregion
            return list;
        }
        #endregion

        #region 根据期货持仓账号和查询交易货币类型从内存表中获取期货持仓表数据列表
        /// <summary>
        /// 根据期货持仓账号和查询交易货币类型从内存表中获取期货持仓表数据列表
        /// </summary>
        /// <param name="qh_Hold_Account">期货持仓账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常</param>
        /// <returns></returns>
        List<QH_HoldAccountTableInfo> GetQH_HoldAccountInfoListFromMemory(string qh_Hold_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_HoldAccountTableInfo> list = new List<QH_HoldAccountTableInfo>();
            if (!ServerConfig.IsLoadAllData)
            {
                #region 没有加载缓存持仓，从数据库中查询
                try
                {
                    //string messStr = "GetQH_HoldAccountInfoListFromMemory===正在查询期货账号" + qh_Hold_Account + "的持仓,因为没有加载持仓表数据正在从数据库中查询，查询时间" + DateTime.Now.ToString();
                    //LogHelper.WriteInfo(messStr);

                    QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
                    list = dal.GetListByAccount(qh_Hold_Account, currencyType);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                #endregion
            }
            else
            {

                #region 直接从缓存中查询
                var manager = MemoryDataManager.QHHoldMemoryList;
                if (manager == null)
                {
                    errorMsg = "还没有初始化缓存数据对象！";
                    return list;
                }
                IList<int> listID = manager.GetAccountHoldLogoID(qh_Hold_Account);
                if (Utils.IsNullOrEmpty(listID))
                {
                    return list;
                }
                if (currencyType == QueryType.QueryCurrencyType.ALL)
                {
                    foreach (int item in listID)
                    {
                        QHHoldMemoryTable table = manager.GetByAccountHoldLogoId(item);
                        if (table != null)
                        {
                            var info = table.Data;
                            list.Add(info);
                        }
                    }
                }
                else
                {
                    foreach (int item in listID)
                    {
                        QHHoldMemoryTable table = manager.GetByAccountHoldLogoId(item);
                        if (table != null)
                        {
                            var info = table.Data;
                            if (info.TradeCurrencyType == (int)currencyType)
                            {
                                list.Add(info);
                            }
                        }
                    }
                }
                #endregion
            }
            return list;
        }
        #endregion
        #endregion

        #region 期货持仓冻结明细查询
        #region 根据委托编号查询【期货持仓冻结表】明细
        /// <summary>
        /// 根据委托编号查询【期货持仓冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<QH_HoldAccountFreezeTableInfo> QueryQH_HoldAccountFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_HoldAccountFreezeTableInfo> list = null;
            if (string.IsNullOrEmpty(entrustNo))
            {
                errorMsg = "委托编号不能为空！";
                return list;
            }
            QH_HoldAccountFreezeTableDal dal = new QH_HoldAccountFreezeTableDal();
            try
            {
                StringBuilder sb = new StringBuilder("  EntrustNumber='" + entrustNo.Trim() + "'");
                //如果查询的冻结类型不是查询所有时加上条件
                if (freezeType != QueryType.QueryFreezeType.ALL)
                {
                    sb.AppendFormat("  AND FreezeTypeLogo='{0}'", (int)freezeType);
                }
                list = dal.GetListArray(sb.ToString());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据用户持仓账号和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细信息
        /// <summary>
        /// Title：根据用户持仓账号和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="account">持仓账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_HoldAccountFreezeTableInfo> PagingQueryQH_HoldAccountFreezeByAccount(string holdAccount, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<QH_HoldAccountFreezeTableInfo> list = null;
            QH_HoldAccountFreezeTableDal dal = new QH_HoldAccountFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果持仓账号或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(holdAccount))
            {
                errorMsg = "持仓账号不能为空！";
                return list;
            }
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.HoldFreezeLogoId,f.EntrustNumber,f.FreezeAmount,f.AccountHoldLogo,f.FreezeTypeLogo,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.HoldFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "QH_HoldAccountFreezeTable as f  inner join QH_HoldAccountTable as a on a.AccountHoldLogoId=f.AccountHoldLogo ";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            //1=1 and a.UserAccountDistributeLogo='010000000406' and a.TradeCurrencyType=1
            sb.Append(" a.UserAccountDistributeLogo='" + holdAccount + "'  ");

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<QH_HoldAccountFreezeTableInfo> com = new CommonDALOperate<QH_HoldAccountFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                // list = dal.PagingQH_HoldAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion

        }
        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【期货持仓冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_HoldAccountFreezeTableInfo> PagingQueryQH_HoldAccountFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<QH_HoldAccountFreezeTableInfo> list = null;
            QH_HoldAccountFreezeTableDal dal = new QH_HoldAccountFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "用户ID不能为空！";
                return list;
            }
            //如果日后要加上用户和密码验证在此加上
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.HoldFreezeLogoId,f.EntrustNumber,f.FreezeAmount,f.AccountHoldLogo,f.FreezeTypeLogo,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.HoldFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "QH_HoldAccountFreezeTable as f  inner join QH_HoldAccountTable as a on a.AccountHoldLogoId=f.AccountHoldLogo ";


            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            #region 从缓存中获取持仓账号
            List<UA_UserAccountAllocationTableInfo> userAccountInfo = AccountManager.Instance.GetUserHoldAccountFormUserCache(userID, accountType, 0);
            #region 添加数据
            string userIDstr = "";
            foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
            {
                userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
            }
            if (!string.IsNullOrEmpty(userIDstr))
            {
                userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
            }

            #endregion
            #endregion

            #region 直接从数据库中获取
            if (!string.IsNullOrEmpty(userIDstr))
            {
                if (userIDstr.Split(',').Length > 1)
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo  in ( {0} )", userIDstr);
                }
                else
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo = {0} ", userIDstr);
                }
            }
            else //如果在缓存中获取不到数据直接从数据库中获取数据
            {
                sb.Append("  a.UserAccountDistributeLogo =''");
                //if (accountType == 0)
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.FuturesHold + "')  and userid='" + userID + "' )");
                //}
                //else
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "'  and userid='" + userID + "' )");
                //}
            }
            #endregion

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<QH_HoldAccountFreezeTableInfo> com = new CommonDALOperate<QH_HoldAccountFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingQH_HoldAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion
        }
        #endregion
        #endregion

        #endregion

        #region 期货资金、资金冻结查询

        #region 期货资金明细查询

        #region 根据用户ID查询用户所拥有的期货资金账号明细
        /// <summary>
        /// 根据用户ID查询用户所拥有的期货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_CapitalAccountTableInfo> QueryQH_CapitalAccountByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_CapitalAccountTableInfo> list = new List<QH_CapitalAccountTableInfo>();
            #region 取数据库表数据
            //QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            //try
            //{
            //    list = dal.GetListByUserID(userID, type);
            //    //if (!Utils.IsNullOrEmpty(list))
            //    //{
            //    //    errorMsg = "此用户没有相关数据请检查相关的用户ID是否正确！";
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 先通过用户ID取得用户的期货资金账号
                // UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取资金账号
                userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 0);
                #endregion
                #region 从数据库中获取资金账号
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesCapital);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetQH_CapitalAccountInfoListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return list;
        }
        #endregion

        #region 根据用户ID和密码查询用户所拥有的期货资金账号明细
        /// <summary>
        /// 根据用户ID和密码查询用户所拥有的期货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_CapitalAccountTableInfo> QueryQH_CapitalAccountByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_CapitalAccountTableInfo> list = new List<QH_CapitalAccountTableInfo>();

            #region 从数据库存表中获取数据
            //try
            //{
            //    QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            //    list = dal.GetListByUserIDAndPwd(userID, pwd, type);
            //    //if (!Utils.IsNullOrEmpty(list))
            //    //{
            //    //    errorMsg = "此用户没有相关数据请检查相关的用户ID是否正确！";
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            try
            {
                #region 检查用户密码
                //update 2009-11-25 李健华
                //var userDal = new UA_UserBasicInformationTableDal();
                //if (!userDal.Exists(userID, pwd))
                //{
                //    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                //    return list;
                //}
                //==============
                //====new===
                if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
                {
                    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                    return list;
                }
                //==========end==========

                #endregion

                #region 先通过用户ID取得用户的期货资金账号
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取资金账号
                userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 0);
                #endregion
                #region 从数据库中获取资金账号
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesCapital);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据

                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetQH_CapitalAccountInfoListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据期货资金账号查询期货资金账号明细
        /// <summary>
        /// 根据期货资金账号查询期货资金账号明细
        /// </summary>
        ///<param name="qh_Cap_Account">期货资金账号</param>
        /// <param name="currencytype">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_CapitalAccountTableInfo> QueryQH_CapitalAccountByAccount(string qh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_CapitalAccountTableInfo> list = new List<QH_CapitalAccountTableInfo>();

            #region 从数据库中获取数据
            //try
            //{
            //    QH_CapitalAccountTableDal dal = new QH_CapitalAccountTableDal();
            //    list = dal.GetListByAccount(qh_Cap_Account, type);
            //    //if (!Utils.IsNullOrEmpty(list))
            //    //{
            //    //    errorMsg = "此用户没有相关数据请检查相关的用户ID是否正确！";
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion

            #region 从内存表中获取数据
            list = GetQH_CapitalAccountInfoListFromMemory(qh_Cap_Account, currencyType, out errorMsg);
            #endregion
            return list;
        }
        #endregion

        #region 根据资金账号和查询交易货币类型从内存表中获取期货资金数据列表
        /// <summary>
        /// 根据资金账号和查询交易货币类型从内存表中获取期货资金数据列表
        /// </summary>
        /// <param name="qh_Cap_Account">资金账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常</param>
        /// <returns></returns>
        List<QH_CapitalAccountTableInfo> GetQH_CapitalAccountInfoListFromMemory(string qh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_CapitalAccountTableInfo> list = new List<QH_CapitalAccountTableInfo>();
            var manager = MemoryDataManager.QHCapitalMemoryList;
            if (manager == null)
            {
                errorMsg = "还没有初始化缓存数据对象！";
                return list;
            }
            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                QHCapitalMemoryTable table = manager.GetByCapitalAccountAndCurrencyType(qh_Cap_Account, (int)currencyType);
                if (table != null)
                {
                    var cap = table.Data;
                    list.Add(cap);
                }
            }
            else
            {
                for (int i = 1; i < 4; i++)
                {
                    QHCapitalMemoryTable table = manager.GetByCapitalAccountAndCurrencyType(qh_Cap_Account, i);
                    if (table != null)
                    {
                        var cap = table.Data;
                        list.Add(cap);
                    }
                }
            }
            return list;
        }
        #endregion

        #endregion

        #region 期货资金冻结明细查询
        #region 根据委托编号查询冻结资金明细
        /// <summary>
        /// 根据委托编号查询冻结资金明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">查询冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<QH_CapitalAccountFreezeTableInfo> QueryQH_CapitalAccountFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg)
        {
            errorMsg = "";
            List<QH_CapitalAccountFreezeTableInfo> list = null;
            if (string.IsNullOrEmpty(entrustNo))
            {
                errorMsg = "委托编号不能为空！";
                return list;
            }
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            try
            {
                StringBuilder sb = new StringBuilder(" EntrustNumber='" + entrustNo.Trim() + "'");
                //如果查询的冻结类型不是查询所有时加上条件
                if (freezeType != QueryType.QueryFreezeType.ALL)
                {
                    sb.Append("  AND FreezeTypeLogo='" + (int)freezeType + "'");
                }
                list = dal.GetListArray(sb.ToString());
                //if (!Utils.IsNullOrEmpty(list))
                //{
                //    errorMsg = "此委托编号没有相关数据请检查相关的委托编号是否正确！";
                //}
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据用户资金账号和查询的交易的货币类型查询资金冻结表明细信息
        /// <summary>
        /// Title：根据用户资金账号和查询的交易的货币类型查询资金冻结表明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">查询冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_CapitalAccountFreezeTableInfo> PagingQueryQH_CapitalAccountFreezeByAccount(string account, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<QH_CapitalAccountFreezeTableInfo> list = null;
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果资金账号或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(account))
            {
                errorMsg = "资金账号不能为空！";
                return list;
            }
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.CapitalFreezeLogoId,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,FreezeAmount,OweCosting,FreezeCost,ThawTime,FreezeTime ";
            prcoInfo.PK = "f.CapitalFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "dbo.QH_CapitalAccountFreezeTable as f  inner join QH_CapitalAccountTable as a on a.CapitalAccountLogoId=f.CapitalAccountLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            //1=1 and a.UserAccountDistributeLogo='010000000406' and a.TradeCurrencyType=1
            sb.Append(" a.UserAccountDistributeLogo='" + account + "'  ");
            //如果查询的货币类型不是查询所有时加上条件
            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<QH_CapitalAccountFreezeTableInfo> com = new CommonDALOperate<QH_CapitalAccountFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingQH_CapitalAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion

        }
        #endregion

        #region 根据用户资金账号和查询的交易的货币类型查询资金冻结表明细信息
        /// <summary>
        /// Title：根据用户资金账号和查询的交易的货币类型查询资金冻结表明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">查询冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_CapitalAccountFreezeTableInfo> PagingQueryQH_CapitalAccountFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<QH_CapitalAccountFreezeTableInfo> list = null;
            QH_CapitalAccountFreezeTableDal dal = new QH_CapitalAccountFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "用户ID不能为空！";
                return list;
            }
            //如果日后要加上用户和密码验证在此加上
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.CapitalFreezeLogoId,CapitalAccountLogo,FreezeTypeLogo,EntrustNumber,FreezeAmount,OweCosting,FreezeCost,ThawTime,FreezeTime ";
            prcoInfo.PK = "f.CapitalFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "dbo.QH_CapitalAccountFreezeTable as f  inner join QH_CapitalAccountTable as a on a.CapitalAccountLogoId=f.CapitalAccountLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            #region 从缓存中获取持仓账号
            List<UA_UserAccountAllocationTableInfo> userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 0);
            #region 添加数据
            string userIDstr = "";
            foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
            {
                userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
            }
            if (!string.IsNullOrEmpty(userIDstr))
            {
                userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
            }

            #endregion
            #endregion

            #region 直接从数据库中获取
            if (!string.IsNullOrEmpty(userIDstr))
            {
                if (userIDstr.Split(',').Length > 1)
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo  in ( {0} )", userIDstr);
                }
                else
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo ={0} ", userIDstr);
                }
            }
            else //如果在缓存中获取不到数据直接从数据库中获取数据
            {
                //这里不再查找数据库
                sb.Append("  a.UserAccountDistributeLogo ='' ");
                //if (accountType == 0)
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.FuturesCapital + "')  and userid='" + userID + "' )");
                //}
                //else
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "'  and userid='" + userID + "' )");
                //}
            }

            #endregion

            //如果查询的货币类型不是查询所有时加上条件
            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<QH_CapitalAccountFreezeTableInfo> com = new CommonDALOperate<QH_CapitalAccountFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingQH_CapitalAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion

        }
        #endregion
        #endregion

        #endregion

        #region 现货资金、资金冻结查询

        #region 现货资金明细查询
        #region 根据【用户ID查询】用户所拥有的现货资金账号明细
        /// <summary>
        /// 根据【用户ID查询】用户所拥有的public货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_CapitalAccountTableInfo> QueryXH_CapitalAccountByUserID(string userID, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();
            #region 从数据库中获取数据
            //try
            //{
            //    XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            //    list = dal.GetListByUserID(userID, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 先通过用户ID取得用户的现货资金账号
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取资金账号
                userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 1);
                #endregion
                #region 从数据库中获取资金账号
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotCapital);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion
                #region 从内存表中获取数据
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetXH_CapitalAccountInfoListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;

        }
        #endregion

        #region 根据【用户ID和密码】查询用户所拥有的现货资金账号明细
        /// <summary>
        /// 根据【用户ID和密码】查询用户所拥有的现货资金账号明细
        /// </summary>
        /// <param name="userID">用户（交易员）ID</param>
        /// <param name="pwd">用户（交易员）密码</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_CapitalAccountTableInfo> QueryXH_CapitalAccountByUserIDAndPwd(string userID, string pwd, int accountType, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();
            #region 从数据库中获取数据
            //try
            // { 
            //     XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            //     list = dal.GetListByUserIDAndPwd(userID, pwd, type);
            // }
            // catch (Exception ex)
            // {
            //     errorMsg = ex.Message;
            //     LogHelper.WriteError(ex.ToString(), ex);
            // }
            #endregion

            try
            {
                #region 检查用记密码
                //update 2009-11-25 李健华
                //var userDal = new UA_UserBasicInformationTableDal();
                //if (!userDal.Exists(userID, pwd))
                //{
                //    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                //    return list;
                //}
                //==============
                //====new===
                if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
                {
                    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                    return list;
                }
                //==========end==========

                #endregion

                #region 先通过用户ID取得用户的现货资金账号
                //UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 从缓存中获取资金账号
                userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 1);
                #endregion
                #region 从数据库中获取资金账号
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.SpotCapital);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion

                #region 从内存表中获取数据

                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    list.AddRange(GetXH_CapitalAccountInfoListFromMemory(item.UserAccountDistributeLogo, currencyType, out errorMsg));
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }

        #endregion

        #region 根据【现货资金账号】查询现货资金账号明细
        /// <summary>
        /// 根据【现货资金账号】查询现货资金账号明细
        /// </summary>
        ///<param name="xh_Cap_Account">现货资金账号</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_CapitalAccountTableInfo> QueryXH_CapitalAccountByAccount(string xh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();

            #region 从数据库中获取数据
            //try
            //{
            //    XH_CapitalAccountTableDal dal = new XH_CapitalAccountTableDal();
            //    list = dal.GetListByAccount(xh_Cap_Account, type);
            //}
            //catch (Exception ex)
            //{
            //    errorMsg = ex.Message;
            //    LogHelper.WriteError(ex.ToString(), ex);
            //}
            #endregion
            try
            {
                #region 从内存表中获取数据
                list = GetXH_CapitalAccountInfoListFromMemory(xh_Cap_Account, currencyType, out errorMsg);
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据资金账号和查询交易货币类型从内存表中获取现货资金数据列表
        /// <summary>
        /// 根据资金账号和查询交易货币类型从内存表中获取数据列表
        /// </summary>
        /// <param name="qh_Cap_Account">资金账号</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="errorMsg">查询异常</param>
        /// <returns></returns>
        List<XH_CapitalAccountTableInfo> GetXH_CapitalAccountInfoListFromMemory(string xh_Cap_Account, QueryType.QueryCurrencyType currencyType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_CapitalAccountTableInfo> list = new List<XH_CapitalAccountTableInfo>();
            var manager = MemoryDataManager.XHCapitalMemoryList;
            if (manager == null)
            {
                errorMsg = "还没有初始化缓存数据对象！";
                return list;
            }
            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                XHCapitalMemoryTable table = manager.GetByCapitalAccountAndCurrencyType(xh_Cap_Account, (int)currencyType);
                if (table != null)
                {
                    var cap = table.Data;
                    list.Add(cap);
                }
            }
            else
            {
                for (int i = 1; i < 4; i++)
                {
                    XHCapitalMemoryTable table = manager.GetByCapitalAccountAndCurrencyType(xh_Cap_Account, i);
                    if (table != null)
                    {
                        var cap = table.Data;
                        list.Add(cap);
                    }
                }
            }
            return list;
        }
        #endregion

        #endregion

        #region 现货资金冻结明细查询

        #region 根据委托编号查询【现货资金冻结表】明细
        /// <summary>
        /// 根据委托编号查询【现货资金冻结表】明细
        /// </summary>
        /// <param name="entrustNo">委托编号</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<XH_CapitalAccountFreezeTableInfo> QueryXH_CapitalAccountFreezeByEntrustNo(string entrustNo, QueryType.QueryFreezeType freezeType, out string errorMsg)
        {
            errorMsg = "";
            List<XH_CapitalAccountFreezeTableInfo> list = null;
            if (string.IsNullOrEmpty(entrustNo))
            {
                errorMsg = "委托编号不能为空！";
                return list;
            }
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            try
            {
                StringBuilder sb = new StringBuilder("  EntrustNumber='" + entrustNo.Trim() + "'");
                //如果查询的冻结类型不是查询所有时加上条件
                if (freezeType != QueryType.QueryFreezeType.ALL)
                {
                    sb.AppendFormat("  AND FreezeTypeLogo='{0}'", (int)freezeType);
                }
                list = dal.GetListArray(sb.ToString());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
        }
        #endregion

        #region 根据用户资金账号和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细信息
        /// <summary>
        /// Title：根据用户资金账号和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="account">资金账号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_CapitalAccountFreezeTableInfo> PagingQueryXH_CapitalAccountFreezeByAccount(string account, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<XH_CapitalAccountFreezeTableInfo> list = null;
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果资金账号或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(account))
            {
                errorMsg = "资金账号不能为空！";
                return list;
            }
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.CapitalFreezeLogoId,f.CapitalAccountLogo,f.FreezeTypeLogo,f.EntrustNumber,f.FreezeCapitalAmount,f.FreezeCost,f.OweCosting,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.CapitalFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "dbo.XH_CapitalAccountFreezeTable as f  inner join XH_CapitalAccountTable as a on a.CapitalAccountLogo=f.CapitalAccountLogo";

            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            //1=1 and a.UserAccountDistributeLogo='010000000406' and a.TradeCurrencyType=1
            sb.Append(" a.UserAccountDistributeLogo='" + account + "'  ");

            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<XH_CapitalAccountFreezeTableInfo> com = new CommonDALOperate<XH_CapitalAccountFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                //list = dal.PagingXH_CapitalAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion

        }

        #endregion

        #region 根据用户ID和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细
        /// <summary>
        /// Title：根据用户ID和查询的交易的货币类型、查询时间段查询【现货资金冻结表】明细信息
        /// Desc.：如果开始时间或者结束时间为null即返回当前时间向前一个月（30天）的时间段查询
        ///        如果开始时间大于结束时间即返回当前时间向前一个月（30天）的时间段查询
        /// </summary>
        /// <param name="userID">资金账号</param>
        /// <param name="accountType">账号类型(数据库中BD_AccountType的九种) 如果传为0时查询对应的现货持仓账号类别(3--证券股东代码,9--港股股东代码)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币查询类型（ALL-0，RMB-1，HK-2，US-3）</param>
        /// <param name="freezeType">冻结类型</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="total">总页数</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<XH_CapitalAccountFreezeTableInfo> PagingQueryXH_CapitalAccountFreezeByUserID(string userID, int accountType, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, QueryType.QueryFreezeType freezeType, PagingInfo pageInfo, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<XH_CapitalAccountFreezeTableInfo> list = null;
            XH_CapitalAccountFreezeTableDal dal = new XH_CapitalAccountFreezeTableDal();
            errorMsg = "";
            total = 0;
            #endregion

            #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
            if (string.IsNullOrEmpty(userID))
            {
                errorMsg = "用户ID不能为空！";
                return list;
            }
            //如果日后要加上用户和密码验证在此加上
            if (pageInfo == null)
            {
                errorMsg = "分页信息不能为空!";
                return null;
            }
            #endregion

            #region 分页查询相关信息
            PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
            prcoInfo.IsCount = pageInfo.IsCount;
            prcoInfo.PageNumber = pageInfo.CurrentPage;
            prcoInfo.PageSize = pageInfo.PageLength;
            prcoInfo.Fields = " f.CapitalFreezeLogoId,f.CapitalAccountLogo,f.FreezeTypeLogo,f.EntrustNumber,f.FreezeCapitalAmount,f.FreezeCost,f.OweCosting,f.ThawTime,f.FreezeTime ";
            prcoInfo.PK = "f.CapitalFreezeLogoId";
            if (pageInfo.Sort == 0)
            {
                prcoInfo.Sort = " f.FreezeTime asc ";
            }
            else
            {
                prcoInfo.Sort = " f.FreezeTime desc ";
            }
            prcoInfo.Tables = "dbo.XH_CapitalAccountFreezeTable as f  inner join XH_CapitalAccountTable as a on a.CapitalAccountLogo=f.CapitalAccountLogo";


            #region 组装相关条件
            StringBuilder sb = new StringBuilder();
            #region 从缓存中获取持仓账号
            List<UA_UserAccountAllocationTableInfo> userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 1);
            #region 添加数据
            string userIDstr = "";
            foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
            {
                userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
            }
            if (!string.IsNullOrEmpty(userIDstr))
            {
                userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
            }

            #endregion
            #endregion

            #region 直接从数据库中获取
            if (!string.IsNullOrEmpty(userIDstr))
            {
                if (userIDstr.Split(',').Length > 1)
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo  in ( {0} )", userIDstr);
                }
                else
                {
                    sb.AppendFormat("  a.UserAccountDistributeLogo ={0} ", userIDstr);
                }
            }
            else //如果在缓存中获取不到数据直接从数据库中获取数据
            {
                //这里不再查找数据库
                sb.Append("  a.UserAccountDistributeLogo ='' ");
                //if (accountType == 0)
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.SpotCapital + "')  and userid='" + userID + "' )");
                //}
                //else
                //{
                //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "'  and userid='" + userID + "' )");
                //}
            }
            #endregion


            if (currencyType != QueryType.QueryCurrencyType.ALL)
            {
                sb.Append(" and a.TradeCurrencyType='" + (int)currencyType + "'");
            }
            sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "f.FreezeTime");
            //如果查询的冻结类型不是查询所有时加上条件
            if (freezeType != QueryType.QueryFreezeType.ALL)
            {
                sb.AppendFormat("  AND f.FreezeTypeLogo='{0}'", (int)freezeType);
            }
            #endregion

            prcoInfo.Filter = sb.ToString();
            #endregion

            #region 执行查询
            try
            {
                CommonDALOperate<XH_CapitalAccountFreezeTableInfo> com = new CommonDALOperate<XH_CapitalAccountFreezeTableInfo>();
                list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                // list = dal.PagingXH_CapitalAccountFreezeByFilter(prcoInfo, out pageTotal);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }
            return list;
            #endregion
        }
        #endregion
        #endregion

        #endregion

        #region 根据用户ID查询银行明细
        /// <summary>
        /// 根据用户ID查询银行明细 
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="errorMsg">查询不到数据提示信息</param>
        /// <returns></returns>
        public List<UA_BankAccountTableInfo> UA_BankAccountByUserID(string userId, out string errorMsg)
        {
            List<UA_BankAccountTableInfo> result = null;
            errorMsg = null;
            try
            {
                StringBuilder sb = new StringBuilder("");
                if (string.IsNullOrEmpty(userId))
                {
                    errorMsg = "请输入要查询的用户ID！";
                }
                else
                {
                    #region 从缓存中获取用户银行账号
                    UA_UserAccountAllocationTableInfo account = AccountManager.Instance.GetAccountByUserIDAndAccountType(userId, (int)Types.AccountType.BankAccount);
                    #endregion

                    #region 从数据库中查询
                    if (account != null)
                    {
                        result = UA_BankAccountByBankAccount(account.UserAccountDistributeLogo, out errorMsg);
                        //sb.AppendFormat("   [UserAccountDistributeLogo] ='{0}' ", account.UserAccountDistributeLogo);
                    }
                    else
                    {
                        errorMsg = "对不起，查询失败！失败原因为：交易员信息不存在！";
                        return null;
                    }
                    //sb.Append("   [UserAccountDistributeLogo] in( ");
                    //sb.Append(" select useraccountdistributelogo from UA_UserAccountAllocationTable ");
                    //sb.Append("  where accounttypelogo=1 and userid='" + userId.Trim() + "')   ");
                    //UA_BankAccountTableDal dal = new UA_BankAccountTableDal();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 根据银行账号查询银行明细
        /// <summary>
        /// 根据银行账号查询银行明细 
        /// </summary>
        /// <param name="bankAccount">银行账号</param>
        /// <param name="errorMsg">查询不到数据提示信息</param>
        /// <returns></returns>
        public List<UA_BankAccountTableInfo> UA_BankAccountByBankAccount(string bankAccount, out string errorMsg)
        {
            List<UA_BankAccountTableInfo> result = null;
            errorMsg = null;
            try
            {
                if (string.IsNullOrEmpty(bankAccount))
                {
                    errorMsg = "请输入要查询的银行账户！";
                }
                else
                {
                    UA_BankAccountTableDal dal = new UA_BankAccountTableDal();
                    result = dal.GetListArray("  UserAccountDistributeLogo='" + bankAccount.Trim() + "'");
                    if (Utils.IsNullOrEmpty(result))
                    {
                        errorMsg = "对不起，查询失败！失败原因为：交易员的不存在银行账号信息  ！";
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
            }
            return result;
        }
        #endregion

        #endregion

        #region 期货资金流水（盘后清算）查询
        /// <summary>
        /// Title:根据用户ID和密码查询期货相应盘后的资金清算流水
        /// Desc.:此方法查询返回的是盘后清算的资金流水方法.
        ///       如果开始日期和结束日期为null/"",默认查询当前日志前一个月内的所有资金流水，
        ///       如果开始日期和结束日期都为非法数据则同上默认查询当前日志前一个月内的所有资金流水，
        ///       为了提供查询效率，如果不是为了实现分页则在查询时传递默认的
        ///       是否返回分页是设置为false，不返回总页数
        /// Create By:李健华
        /// Create Date:2009-12-04
        /// Desc: 增加参数账户类型ID，用于区分股指期货和商品期货
        /// Update by: 董鹏
        /// Update Date: 2010-03-09
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="currencyType">货币类型</param>
        /// <param name="pageInfo">分页信息实体</param>
        /// <param name="accountType">账户类型ID</param>
        /// <param name="total">总页数据</param>
        /// <param name="errorMsg">查询异常信息</param>
        /// <returns></returns>
        public List<QH_TradeCapitalFlowDetailInfo> PagingQueryQH_CapitalFlowDetailByAccount(string userID, string pwd, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, PagingInfo pageInfo, int accountType, out int total, out string errorMsg)
        {
            #region 初始货变量
            List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();
            errorMsg = "";
            total = 0;
            #endregion

            try
            {
                #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
                if (string.IsNullOrEmpty(userID))
                {
                    errorMsg = "用户ID不能为空！";
                    return list;
                }
                //如果日后要加上用户和密码验证在此加上
                if (pageInfo == null)
                {
                    errorMsg = "分页信息不能为空!";
                    return null;
                }
                #endregion

                #region 检查用户密码
                if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
                {
                    errorMsg = "用户密码不正确，或者没有此用户相关信息！";
                    return list;
                }
                #endregion

                #region 先通过用户ID取得用户的期货资金账号
                List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
                #region 如果为0就查询类别下的所有可能有两个账号
                #region 从缓存中获取账号
                //userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, (int)Types.AccountType.StockFuturesCapital, 0);
                userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, accountType, 0);
                #endregion
                #endregion

                #region 直接从数据库中获取
                //if (accountType == 0)
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesCapital);
                //}
                //else
                //{
                //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
                //}
                #endregion
                #endregion


                #region 分页查询相关信息
                PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
                prcoInfo.IsCount = pageInfo.IsCount;
                prcoInfo.PageNumber = pageInfo.CurrentPage;
                prcoInfo.PageSize = pageInfo.PageLength;
                prcoInfo.Fields = " TradeID,UserCapitalAccount,FlowTypes,Margin,TradeProceduresFee,ProfitLoss,OtherCose,FlowTotal,CurrencyType,CreateDateTime ";
                prcoInfo.PK = "TradeID";
                if (pageInfo.Sort == 0)
                {
                    prcoInfo.Sort = " CreateDateTime asc ";
                }
                else
                {
                    prcoInfo.Sort = " CreateDateTime desc ";
                }
                prcoInfo.Tables = " dbo.QH_TradeCapitalFlowDetail ";

                #region 组装相关条件
                StringBuilder sb = new StringBuilder();
                string userIDstr = "";
                foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
                {
                    userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
                }
                if (!string.IsNullOrEmpty(userIDstr))
                {
                    userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
                }



                if (!string.IsNullOrEmpty(userIDstr))
                {
                    if (userIDstr.Split(',').Length > 1)
                    {
                        sb.AppendFormat("  UserCapitalAccount  in ( {0} )", userIDstr);
                    }
                    else
                    {
                        sb.AppendFormat("  UserCapitalAccount ={0} ", userIDstr);
                    }
                }
                else //如果在缓存中获取不到数据直接从数据库中获取数据
                {
                    //这里不再查找数据库
                    sb.Append("  UserCapitalAccount ='' ");
                    //if (accountType == 0)
                    //{
                    //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.SpotCapital + "')  and userid='" + userID + "' )");
                    //}
                    //else
                    //{
                    //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "'  and userid='" + userID + "' )");
                    //}
                }

                if (currencyType != QueryType.QueryCurrencyType.ALL)
                {
                    sb.Append(" and CurrencyType='" + (int)currencyType + "'");
                }
                sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "CreateDateTime");

                prcoInfo.Filter = sb.ToString();
                #endregion

                #endregion

                #region 执行查询
                try
                {
                    QH_TradeCapitalFlowDetailDal dal = new QH_TradeCapitalFlowDetailDal();
                    CommonDALOperate<QH_TradeCapitalFlowDetailInfo> com = new CommonDALOperate<QH_TradeCapitalFlowDetailInfo>();
                    list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteError(ex.ToString(), ex);
                }
                return list;
                #endregion

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.WriteError(ex.ToString(), ex);
            }

            return list;
        }

        ///// <summary>
        ///// 描述：商品期货资金流水查询
        ///// 作者：叶振东
        ///// 时间：2010-01-28
        ///// </summary>
        ///// <param name="userID">用户ID</param>
        ///// <param name="startTime">开始时间</param>
        ///// <param name="endTime">结束时间</param>
        ///// <param name="currencyType">货币类型</param>
        ///// <param name="pageInfo">分页信息实体</param>
        ///// <param name="total">总页数据</param>
        ///// <param name="errorMsg">查询异常信息</param>
        ///// <returns></returns>
        //public List<QH_TradeCapitalFlowDetailInfo> PagingQuerySPQH_CapitalFlowDetailByAccount(string userID, string pwd, DateTime? startTime, DateTime? endTime, QueryType.QueryCurrencyType currencyType, PagingInfo pageInfo, out int total, out string errorMsg)
        //{
        //    #region 初始货变量
        //    List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();
        //    errorMsg = "";
        //    total = 0;
        //    #endregion

        //    try
        //    {
        //        #region 如果用户ID或者分页对象实体为空直接返错误传递参数提示 列表为null
        //        if (string.IsNullOrEmpty(userID))
        //        {
        //            errorMsg = "用户ID不能为空！";
        //            return list;
        //        }
        //        //如果日后要加上用户和密码验证在此加上
        //        if (pageInfo == null)
        //        {
        //            errorMsg = "分页信息不能为空!";
        //            return null;
        //        }
        //        #endregion

        //        #region 检查用户密码
        //        if (!AccountManager.Instance.ExistsBasicUserPWDByUserId(userID, pwd))
        //        {
        //            errorMsg = "用户密码不正确，或者没有此用户相关信息！";
        //            return list;
        //        }
        //        #endregion

        //        #region 先通过用户ID取得用户的期货资金账号
        //        List<UA_UserAccountAllocationTableInfo> userAccountInfo = new List<UA_UserAccountAllocationTableInfo>();
        //        #region 如果为0就查询类别下的所有可能有两个账号
        //        #region 从缓存中获取账号
        //        userAccountInfo = AccountManager.Instance.GetUserCapitalAccountFormUserCache(userID, (int)Types.AccountType.CommodityFuturesCapital, 0);
        //        #endregion
        //        #endregion

        //        #region 直接从数据库中获取
        //        //if (accountType == 0)
        //        //{
        //        //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountTypeClass(userID, Types.AccountAttributionType.FuturesCapital);
        //        //}
        //        //else
        //        //{
        //        //    userAccountInfo = dal.GetUserAccountByUserIDAndPwdAndAccountType(userID, accountType);
        //        //}
        //        #endregion
        //        #endregion


        //        #region 分页查询相关信息
        //        PagingProceduresInfo prcoInfo = new PagingProceduresInfo();
        //        prcoInfo.IsCount = pageInfo.IsCount;
        //        prcoInfo.PageNumber = pageInfo.CurrentPage;
        //        prcoInfo.PageSize = pageInfo.PageLength;
        //        prcoInfo.Fields = " TradeID,UserCapitalAccount,FlowTypes,Margin,TradeProceduresFee,ProfitLoss,OtherCose,FlowTotal,CurrencyType,CreateDateTime ";
        //        prcoInfo.PK = "TradeID";
        //        if (pageInfo.Sort == 0)
        //        {
        //            prcoInfo.Sort = " CreateDateTime asc ";
        //        }
        //        else
        //        {
        //            prcoInfo.Sort = " CreateDateTime desc ";
        //        }
        //        prcoInfo.Tables = " dbo.QH_TradeCapitalFlowDetail ";

        //        #region 组装相关条件
        //        StringBuilder sb = new StringBuilder();
        //        string userIDstr = "";
        //        foreach (UA_UserAccountAllocationTableInfo item in userAccountInfo)
        //        {
        //            userIDstr += ",   '" + item.UserAccountDistributeLogo + "'";
        //        }
        //        if (!string.IsNullOrEmpty(userIDstr))
        //        {
        //            userIDstr = userIDstr.Substring(userIDstr.IndexOf(',') + 1);
        //        }



        //        if (!string.IsNullOrEmpty(userIDstr))
        //        {
        //            if (userIDstr.Split(',').Length > 1)
        //            {
        //                sb.AppendFormat("  UserCapitalAccount  in ( {0} )", userIDstr);
        //            }
        //            else
        //            {
        //                sb.AppendFormat("  UserCapitalAccount ={0} ", userIDstr);
        //            }
        //        }
        //        else //如果在缓存中获取不到数据直接从数据库中获取数据
        //        {
        //            //这里不再查找数据库
        //            sb.Append("  UserCapitalAccount ='' ");
        //            //if (accountType == 0)
        //            //{
        //            //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo in ( select accounttypelogo from BD_AccountType where atcid='" + (int)CommonObject.Types.AccountAttributionType.SpotCapital + "')  and userid='" + userID + "' )");
        //            //}
        //            //else
        //            //{
        //            //    sb.Append("  a.UserAccountDistributeLogo  in ( select useraccountdistributelogo from dbo.UA_UserAccountAllocationTable  where accounttypelogo='" + accountType + "'  and userid='" + userID + "' )");
        //            //}
        //        }

        //        if (currencyType != QueryType.QueryCurrencyType.ALL)
        //        {
        //            sb.Append(" and CurrencyType='" + (int)currencyType + "'");
        //        }
        //        sb.AppendFormat(CommonDALBulidSQLScript.BuildWhereQueryBetwennTime(startTime, endTime, 30), "CreateDateTime");

        //        prcoInfo.Filter = sb.ToString();
        //        #endregion

        //        #endregion

        //        #region 执行查询
        //        try
        //        {
        //            QH_TradeCapitalFlowDetailDal dal = new QH_TradeCapitalFlowDetailDal();
        //            CommonDALOperate<QH_TradeCapitalFlowDetailInfo> com = new CommonDALOperate<QH_TradeCapitalFlowDetailInfo>();
        //            list = com.PagingQueryProcedures(prcoInfo, out total, dal.ReaderBind);
        //        }
        //        catch (Exception ex)
        //        {
        //            errorMsg = ex.Message;
        //            LogHelper.WriteError(ex.ToString(), ex);
        //        }
        //        return list;
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message;
        //        LogHelper.WriteError(ex.ToString(), ex);
        //    }

        //    return list;
        //}
        #endregion


    }
}