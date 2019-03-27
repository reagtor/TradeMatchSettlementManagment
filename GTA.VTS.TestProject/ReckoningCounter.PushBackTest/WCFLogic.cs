#region Using Namespace

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Timers;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.PushBackTest.DoAccountService;
using ReckoningCounter.PushBackTest.DoDealRptService;
using ReckoningCounter.PushBackTest.DoOrderService;
using ReckoningCounter.PushBackTest.HKTraderFindService;
using ReckoningCounter.PushBackTest.TraderFindService;
using ReckoningCounter.PushBackTest.TraderQueryService;
using TypesOrderPriceType = ReckoningCounter.PushBackTest.DoAccountService.TypesOrderPriceType;
using ReckoningCounter.PushBackTest.HKTraderQuerySevice;

#endregion

namespace ReckoningCounter.PushBackTest
{
    public class WCFLogic
    {
        private AccountAndCapitalManagementClient accountClient;
        private string Channelid;

        private DoOrderClient doOrderClient;
        private string gzqhAccount = "";
        private string hkAccount = "";
        private HKTraderFindClient hkTraderFindClient;
        public bool IsServiceOk;

        private OrderDealRptClient rptClient;
        private string ServiceErrorMsg = "无法连接柜台服务！请检查配置没有问题后重新连接！";
        private string spqhAccount = "";
        private Timer timer;
        private TraderFindClient traderFindClient;
        //private HKTraderQueryClient hkTraderQueryClient;
        TraderQueryClient commonQueryClient;

        //Create date:2009-11-12
        /// <summary>
        /// 港股交易查询服务
        /// </summary>
        private HKTraderQuerySevice.HKTraderQueryClient hkTraderQueryClient;
        //========

        private string traderId = "";
        private string xhAccount = "";

        public void LoadTraderInfo()
        {
            traderId = ServerConfig.TraderID;
            xhAccount = ServerConfig.XHCapitalAccount;
            hkAccount = ServerConfig.HKCapitalAccount;
            gzqhAccount = ServerConfig.GZQHCapitalAccount;
            spqhAccount = ServerConfig.SPQHCapitalAccount;
        }

        public bool Initialize(string _channelID)
        {
            bool result = false;

            try
            {
                doOrderClient = new DoOrderClient();
                ICommunicationObject co1 = doOrderClient;
                co1.Faulted += CO_Faulted;

                traderFindClient = new TraderFindClient();
                ICommunicationObject co2 = traderFindClient;
                co2.Faulted += CO2_Faulted;

                OrderCallBack callBack = new OrderCallBack();
                rptClient = new OrderDealRptClient(new InstanceContext(callBack));
                ICommunicationObject co3 = rptClient;
                co3.Faulted += CO3_Faulted;

                accountClient = new AccountAndCapitalManagementClient();
                ICommunicationObject co4 = accountClient;
                co4.Faulted += co4_Faulted;

                //hkTraderQueryClient = new HKTraderQueryClient();
                //ICommunicationObject co5 = hkTraderQueryClient;
                //co5.Faulted += co5_Faulted;

                hkTraderFindClient = new HKTraderFindClient();
                ICommunicationObject co6 = hkTraderFindClient;
                co6.Faulted += co6_Faulted;

                hkTraderQueryClient = new HKTraderQuerySevice.HKTraderQueryClient();

                commonQueryClient = new TraderQueryClient();

                IsServiceOk = true;
                if (string.IsNullOrEmpty(_channelID))
                {
                    Channelid = ServerConfig.ChannelID;
                    if (string.IsNullOrEmpty(Channelid))
                    {
                        Channelid = CommUtils.GetMacAddress();
                        ServerConfig.ChannelID = Channelid;
                    }
                }
                else
                {
                    Channelid = _channelID;
                }
                result = rptClient.RegisterChannel(Channelid);

                if (result)
                {
                    string msg = "WCF Service [DoOrderService] is connected! " + DateTime.Now;
                    WriteMsg(msg);

                    string msg2 = "WCF Service [TradeFindService] is connected! " + DateTime.Now;
                    WriteMsg(msg2);

                    string msg3 = "WCF Service [DoDealRptService] is connected! " + DateTime.Now;
                    WriteMsg(msg3);

                    WriteMsg("");
                }
                else
                {
                    IsServiceOk = false;
                }

                if (IsServiceOk)
                {
                    timer = new Timer();
                    timer.Interval = 10 * 1000;
                    timer.Elapsed += CheckRptChannel;
                    timer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        private void CheckRptChannel(object sender, ElapsedEventArgs e)
        {
            try
            {
                string date = rptClient.CheckChannel();
                //WriteMsg(date);
            }
            catch (Exception ex)
            {
                timer.Enabled = false;
                ShutDown();
            }
        }

        private void co6_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [HKTraderFindService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void co5_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [HKTraderQueryService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void co4_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [DoAccountAndCapitalService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void CO3_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [DoDealRptService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void CO2_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [TradeFindService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        private void CO_Faulted(object sender, EventArgs e)
        {
            IsServiceOk = false;

            string msg = "<————WCF Service [DoOrderService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);
        }

        public void WriteMsg(string msg)
        {
            Program.MainForm.WriteWCFMsg(msg);
        }

        public void ShutDown()
        {
            IsServiceOk = false;

            try
            {
                rptClient.UnRegisterChannel(Channelid);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteError(ex.Message, ex);
            }

            rptClient.DoClose();
            traderFindClient.DoClose();
            doOrderClient.DoClose();
            accountClient.DoClose();

            string msg = "<————WCF Service [DoOrderService] is disconnected! " + DateTime.Now;
            WriteMsg(msg);

            string msg2 = "<————WCF Service [TradeFindService] is disconnected! " + DateTime.Now;
            WriteMsg(msg2);

            string msg3 = "<————WCF Service [DoDealRptService] is disconnected! " + DateTime.Now;
            WriteMsg(msg3);

            string msg4 = "<————WCF Service [DoAccountAndCapitalService] is disconnected! " + DateTime.Now;
            WriteMsg(msg4);

            WriteMsg("");
        }

        public HighLowRangeValue GetHighLowRangeValueByCommodityCode(string code, decimal orderPrice)
        {
            return accountClient.GetHighLowRangeValueByCommodityCode(code, orderPrice);
        }

        public HighLowRangeValue GetHKHighLowRangeValueByCommodityCode(string code, decimal orderPrice, Types.HKPriceType priceType, Types.TransactionDirection tranType)
        {
            return accountClient.GetHKHighLowRangeValueByCommodityCode(code, orderPrice, priceType, tranType);
        }
        /// <summary>
        /// 根据代码和代码类型获取最后成交价
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="breedClassType">所属商品类型（1-现货,2-商品期货,3-股指期货,4-港股)</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>最新成交价</returns>
        public decimal GetLastPricByCommodityCode(string code, int breedClassType, out string errMsg)
        {
            return accountClient.GetMarketDataInfoByCode(out errMsg, code, breedClassType).LastPrice;
        }

        #region XH

        public List<TraderQueryService.XH_CapitalAccountTableInfo> XHCapital
        {
            get
            {
                string msg = "";

                var cap = QueryXHCapital(xhAccount, ref msg);
                List<TraderQueryService.XH_CapitalAccountTableInfo> list = new List<TraderQueryService.XH_CapitalAccountTableInfo>();
                if (cap != null)
                    list.Add(cap);

                return list;
            }
        }

        public List<TraderFindService.XH_AccountHoldTableInfo> XHHold
        {
            get
            {
                string msg = "";

                var list = QueryXHHold(xhAccount, ref msg);

                List<TraderFindService.XH_AccountHoldTableInfo> holds = new List<TraderFindService.XH_AccountHoldTableInfo>();

                if (list != null)
                {
                    foreach (var entity in list)
                    {
                        holds.Add(entity.HoldFindResult);
                    }
                }

                return holds;
            }
        }

        public List<TraderFindService.XH_TodayEntrustTableInfo> XHTodayEntrust
        {
            get
            {
                string msg = "";

                return QueryXHTodayEntrust(xhAccount, ref msg);
            }
        }

        public List<TraderFindService.XH_TodayTradeTableInfo> XHTodayTrade
        {
            get
            {
                string msg = "";

                return QueryXHTodayTrade(xhAccount, ref msg);
            }
        }


        public OrderResponse DoStockOrder(StockOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoStockOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <param name="channelID"></param>
        /// <param name="type">1现货，2期货</param>
        /// <returns></returns>
        public bool ChangeEntrustChannel(List<string> entrustNumber, string channelID, int type)
        {
            return rptClient.ChangeEntrustChannel(entrustNumber, channelID, type);
        }

        public long GetXHMaxCount(string traderId, string code, decimal price, TypesOrderPriceType orderPriceType,
                                  out string errMsg)
        {
            return accountClient.GetSpotMaxOrderAmount(out errMsg, traderId, (float)price, code, orderPriceType);
        }

        public long GetQHMaxCount(string traderId, string code, decimal price, TypesOrderPriceType orderPriceType,
                                  out string errMsg)
        {
            return accountClient.GetFutureMaxOrderAmount(out errMsg, traderId, (float)price, code, orderPriceType);
        }

        public bool CancelStockOrder(string entrustNumber, ref string errMsg)
        {
            if (!IsServiceOk)
            {
                errMsg = ServiceErrorMsg;
                return false;
            }

            TypesOrderStateType statetype;
            return doOrderClient.CancelStockOrder(entrustNumber, ref errMsg, out statetype);
        }

        public TraderQueryService.XH_CapitalAccountTableInfo QueryXHCapital(string capitalAccount, ref string msg)
        {
            if (!IsServiceOk)
                return null;

            #region 这里因为只是获取只是资金没有必要再用这个方法也计算市值
            //var capital = traderFindClient.SpotCapitalFind(capitalAccount, Types.CurrencyType.RMB, "", ref msg);
            //if (capital == null)
            //    return null;

            //if (capital.GTCapitalObj != null)
            //{
            //    var cap = capital.GTCapitalObj;

            //    return cap;
            //}
            #endregion

            var capital = commonQueryClient.QueryXH_CapitalAccountTableByAccount(out msg, capitalAccount, ReckoningCounter.PushBackTest.TraderQueryService.QueryTypeQueryCurrencyType.RMB);
            if (capital != null && capital.Count > 0)
            {
                return capital[0];
            }
            return null;

        }

        public List<TraderFindService.SpotHoldFindResultEntity> QueryXHHold(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new TraderFindService.SpotHoldConditionFindEntity();
            var holds = traderFindClient.SpotHoldFind(out icount, capitalAccount, "", null,
                                                      0, int.MaxValue, ref strMessage);

            return holds;
        }

        public List<TraderFindService.XH_TodayEntrustTableInfo> QueryXHTodayEntrust(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<TraderFindService.XH_TodayEntrustTableInfo>();

            int icount;
            var secfe = new TraderFindService.SpotEntrustConditionFindEntity();
            var tets = traderFindClient.SpotTodayEntrustFindByXhCapitalAccount(out icount, capitalAccount,
                                                                               "", null,
                                                                               0, int.MaxValue,
                                                                               ref strMessage);

            return tets;
        }

        public List<TraderFindService.XH_TodayTradeTableInfo> QueryXHTodayTrade(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<TraderFindService.XH_TodayTradeTableInfo>();

            int icount;
            var secfe = new TraderFindService.SpotTradeConditionFindEntity();

            var trades = traderFindClient.SpotTodayTradeFindByCapitalAccount(out icount,
                                                                             out strMessage, capitalAccount,
                                                                             "",
                                                                             0, int.MaxValue, null);

            return trades;
        }

        public TraderFindService.SpotCapitalEntity QueryXHTotalCapital(Types.CurrencyType type, ref string msg)
        {
            if (!IsServiceOk)
                return null;

            return traderFindClient.SpotCapitalFind(xhAccount, type, "", ref msg);

        }

        public List<HKMarketValue> QuerymarketValueXHHold(string code, ref string strMessage)
        {
            List<HKMarketValue> list = new List<HKMarketValue>();

            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new TraderFindService.SpotHoldConditionFindEntity();
            if (!string.IsNullOrEmpty(code))
            {
                shcfe.SpotCode = code;
            }
            var holds = traderFindClient.SpotHoldFind(out icount, xhAccount, "", shcfe, 0, int.MaxValue, ref strMessage);
            if (holds == null)
            {
                return list;
            }
            foreach (var item in holds)
            {
                HKMarketValue hkmare = new HKMarketValue();
                hkmare.BelongMarket = item.BelongMarket;
                hkmare.BreakevenPrice = item.HoldFindResult.BreakevenPrice;
                hkmare.Code = item.HoldFindResult.Code;
                hkmare.CostPrice = item.HoldFindResult.CostPrice;
                hkmare.CurrencyName = item.CurrencyName;
                hkmare.ErroNumber = item.ErroNumber;
                hkmare.ErroReason = item.ErroReason;
                hkmare.FloatProfitLoss = item.FloatProfitLoss;
                hkmare.HKName = item.SpotName;
                hkmare.HoldAveragePrice = item.HoldFindResult.HoldAveragePrice;
                hkmare.HoldSumAmount = item.HoldSumAmount;
                hkmare.MarketValue = item.MarketValue;
                hkmare.RealtimePrice = item.RealtimePrice;
                hkmare.TraderId = item.TraderId;
                hkmare.VarietyCategories = item.VarietyCategories;
                list.Add(hkmare);
            }
            return list;
        }

        #endregion

        #region HK

        public List<HKTraderQuerySevice.HK_CapitalAccountInfo> HKCapital
        {
            get
            {
                string msg = "";

                var cap = QueryHKCapital(hkAccount, ref msg);
                List<HKTraderQuerySevice.HK_CapitalAccountInfo> list = new List<HKTraderQuerySevice.HK_CapitalAccountInfo>();
                if (cap != null)
                    list.Add(cap);

                return list;
            }
        }

        public List<HKTraderQuerySevice.HK_AccountHoldInfo> HKHold
        {
            get
            {
                string msg = "";

                var list = QueryHK_AccountHoldByAccount("", hkAccount, ref msg);

                //List<HKTraderFindService.HK_AccountHoldInfo> holds = new List<HKTraderFindService.HK_AccountHoldInfo>();

                //if (list != null)
                //{
                //    foreach (var entity in list)
                //    {
                //        holds.Add(entity.HoldFindResult);
                //    }
                //}

                return list;
            }
        }


        public List<HKTraderFindService.HK_TodayEntrustInfo> HKTodayEntrust
        {
            get
            {
                string msg = "";

                return QueryHKTodayEntrust(hkAccount, ref msg);
            }
        }

        public List<HKTraderFindService.HK_TodayTradeInfo> HKTodayTrade
        {
            get
            {
                string msg = "";

                return QueryHKTodayTrade(hkAccount, ref msg);
            }
        }


        public OrderResponse DoHKOrder(HKOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoHKOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }

        public OrderResponse ModifyHKOrder(HKModifyOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoHKModifyOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }

        public long GetHKMaxCount(string traderId, string code, decimal price, Types.HKPriceType orderPriceType,
                                  out string errMsg)
        {
            return accountClient.GetHKMaxOrderAmount(out errMsg, traderId, (float)price, code, orderPriceType);
        }

        public bool CancelHKOrder(string entrustNumber, ref string errMsg)
        {
            if (!IsServiceOk)
            {
                errMsg = ServiceErrorMsg;
                return false;
            }

            TypesOrderStateType statetype;
            //return doOrderClient.CancelStockOrder(entrustNumber, ref errMsg, out statetype);
            return doOrderClient.CancelHKOrder(entrustNumber, ref errMsg, out statetype);
        }

        public HKTraderFindService.HKCapitalEntity QueryHKTotalCapital(Types.CurrencyType type, ref string msg)
        {
            if (!IsServiceOk)
                return null;

            var capital = hkTraderFindClient.HKCapitalFind(hkAccount, type, "", ref msg);

            return capital;

        }

        public HKTraderQuerySevice.HK_CapitalAccountInfo QueryHKCapital(string capitalAccount, ref string msg)
        {
            if (!IsServiceOk)
                return null;
            #region old code
            //var capital = hkTraderFindClient.HKCapitalFind(capitalAccount, Types.CurrencyType.HK, "", ref msg);
            //if (capital == null)
            //    return null;

            //if (capital.GTCapitalObj != null)
            //{
            //    var cap = capital.GTCapitalObj;

            //    return cap;
            //}
            #endregion

            var capital = hkTraderQueryClient.QueryHK_CapitalAccountByAccount(out msg, capitalAccount, HKTraderQuerySevice.QueryTypeQueryCurrencyType.HK);
            if (capital != null && capital.Count > 0)
            {
                return capital[0];
            }
            return null;


        }
        public List<HKTraderQuerySevice.HK_AccountHoldInfo> QueryHK_AccountHoldByAccount(string code, string hkAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return null;
            //特殊处理持仓账号
            string hkHoldAccount = hkAccount.Substring(0, hkAccount.Length - 1) + "9"; ;

            var holds = hkTraderQueryClient.QueryHK_AccountHoldByAccount(out strMessage, hkHoldAccount, HKTraderQuerySevice.QueryTypeQueryCurrencyType.HK);

            return holds;

        }

        public List<HKTraderFindService.HKHoldFindResultyEntity> QueryHKHold(string code, string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new HKTraderFindService.HKHoldConditionFindEntity();
            if (!string.IsNullOrEmpty(code))
            {
                shcfe.HKCode = code;
            }
            var holds = hkTraderFindClient.HKHoldFind(out icount, capitalAccount, "", shcfe, 0, int.MaxValue, ref strMessage);

            return holds;
        }
        public List<HKTraderFindService.HKHoldFindResultyEntity> QueryHKHold(string code, ref string strMessage)
        {
            return QueryHKHold(code, hkAccount, ref strMessage);
        }

        public List<HKMarketValue> QuerymarketValueHKHold(string code, ref string strMessage)
        {
            List<HKMarketValue> list = new List<HKMarketValue>();

            List<HKTraderFindService.HKHoldFindResultyEntity> holdMarket = QueryHKHold(code, hkAccount, ref strMessage);
            if (holdMarket == null)
            {
                return list;
            }
            foreach (var item in holdMarket)
            {
                HKMarketValue hkmare = new HKMarketValue();
                hkmare.BelongMarket = item.BelongMarket;
                hkmare.BreakevenPrice = item.HoldFindResult.BreakevenPrice;
                hkmare.Code = item.HoldFindResult.Code;
                hkmare.CostPrice = item.HoldFindResult.CostPrice;
                hkmare.CurrencyName = item.CurrencyName;
                hkmare.ErroNumber = item.ErroNumber;
                hkmare.ErroReason = item.ErroReason;
                hkmare.FloatProfitLoss = item.FloatProfitLoss;
                hkmare.HKName = item.HKName;
                hkmare.HoldAveragePrice = item.HoldFindResult.HoldAveragePrice;
                hkmare.HoldSumAmount = item.HoldSumAmount;
                hkmare.MarketValue = item.MarketValue;
                hkmare.RealtimePrice = item.RealtimePrice;
                hkmare.TraderId = item.TraderId;
                hkmare.VarietyCategories = item.VarietyCategories;
                list.Add(hkmare);
            }
            return list;
        }
        public List<HKTraderFindService.HK_TodayEntrustInfo> QueryHKTodayEntrust(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<HKTraderFindService.HK_TodayEntrustInfo>();
            List<HKTraderFindService.HK_TodayEntrustInfo> tets = new List<HKTraderFindService.HK_TodayEntrustInfo>();
            int icount;
            var secfe = new HKTraderFindService.HKEntrustConditionFindEntity();
            if (!string.IsNullOrEmpty(CurrentQueryValue.QueryHKEnNO))
            {
                tets = QueryHKTodayEntrust(capitalAccount, CurrentQueryValue.QueryHKEnNO, ref   strMessage);
            }
            else
            {
                tets = hkTraderFindClient.HKTodayEntrustFindByHKCapitalAccount(out icount, out strMessage,
                                                                                     capitalAccount, "",
                                                                                     0, int.MaxValue, null
                      );
            }
            return tets;
        }

        public List<HKTraderFindService.HK_TodayEntrustInfo> QueryHKTodayEntrust(string capitalAccount, string entrustNumber, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<HKTraderFindService.HK_TodayEntrustInfo>();

            int icount;
            var secfe = new HKTraderFindService.HKEntrustConditionFindEntity();
            secfe.HKCapitalAccount = capitalAccount;
            secfe.EntrustNumber = entrustNumber;
            var tets = hkTraderFindClient.HKTodayEntrustFindByHKCapitalAccount(out icount, out strMessage,
                                                                               capitalAccount,
                                                                               "",
                                                                               0, int.MaxValue, secfe
                );
            return tets;
        }

        public List<HKTraderFindService.HK_TodayTradeInfo> QueryHKTodayTrade(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<HKTraderFindService.HK_TodayTradeInfo>();

            int icount;
            HKTraderFindService.HKTradeConditionFindEntity filter = new HKTraderFindService.HKTradeConditionFindEntity();
            if (!string.IsNullOrEmpty(CurrentQueryValue.QueryHKTradeNO))
            {
                filter.EntrustNumber = CurrentQueryValue.QueryHKTradeNO;
            }
            else
            {
                filter = null;
            }
            var trades = hkTraderFindClient.HKTodayTradeFindByCapitalAccount(out icount,
                                                                             out strMessage, capitalAccount,
                                                                             "",
                                                                             0, int.MaxValue, filter);

            return trades;
        }

        public List<HKTraderFindService.HK_HistoryEntrustInfo> QueryHKHisotryEntrust(string entrustNumber, DateTime? startTime, DateTime? endTime, ref string strMessage)
        {

            if (!IsServiceOk)
                return new List<HKTraderFindService.HK_HistoryEntrustInfo>();
            List<HKTraderFindService.HK_HistoryEntrustInfo> tets = new List<HKTraderFindService.HK_HistoryEntrustInfo>();
            int icount;
            var secfe = new HKTraderFindService.HKEntrustConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value;
                secfe.EndTime = endTime.Value;
            }


            tets = hkTraderFindClient.HKHistoryEntrustFind(out icount, out strMessage, hkAccount, "", 0, int.MaxValue, secfe);

            return tets;
        }
        public List<HKTraderFindService.HK_HistoryTradeInfo> QueryHKHisotryTrade(string entrustNumber, DateTime? startTime, DateTime? endTime, ref string strMessage)
        {

            if (!IsServiceOk)
                return new List<HKTraderFindService.HK_HistoryTradeInfo>();
            List<HKTraderFindService.HK_HistoryTradeInfo> tets = new List<HKTraderFindService.HK_HistoryTradeInfo>();
            int icount;
            var secfe = new HKTraderFindService.HKTradeConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value;
                secfe.EndTime = endTime.Value;
            }


            tets = hkTraderFindClient.HKHistoryTradeFind(out icount, out strMessage, hkAccount, "", 0, int.MaxValue, secfe);

            return tets;
        }
        public List<HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo> QueryHKModifyOrderRequest(string entrustNumber, DateTime? startTime, DateTime? endTime, ref string strMessage
              , int selectType)
        {

            if (!IsServiceOk)
                return new List<HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo>();
            HKTraderQuerySevice.PagingInfo pageInfo = new HKTraderQuerySevice.PagingInfo();
            pageInfo.CurrentPage = 0;
            pageInfo.IsCount = false;
            pageInfo.PageLength = int.MaxValue;

            List<HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo> tets = new List<HKTraderQuerySevice.HK_HistoryModifyOrderRequestInfo>();
            int icount;
            try
            {
                HKTraderQuerySevice.HKTraderQueryClient cline = new HKTraderQuerySevice.HKTraderQueryClient();

                tets = cline.PagingQueryHK_ModifyRequertByUserIDOrEntrustNo(out icount, out strMessage, traderId, entrustNumber, startTime, endTime, selectType, pageInfo);
            }
            catch (Exception ex)
            {

            }
            return tets;
        }

        /// <summary>
        /// 查询持仓包括统计有盈亏
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<HKTraderQuerySevice.HKHoldFindResultyEntity> QueryHKHoldMarketValue(string code, string pwd)
        {
            List<HKTraderQuerySevice.HKHoldFindResultyEntity> list = new List<HKTraderQuerySevice.HKHoldFindResultyEntity>();
            int count = 0;
            HKTraderQuerySevice.HKHoldConditionFindEntity findConndition = new HKTraderQuerySevice.HKHoldConditionFindEntity();
            findConndition.HKCode = code;
            string errMesg = "";
            list = hkTraderQueryClient.PagingQueryHKHold(out count, traderId, pwd, 9, findConndition, 0, 50, ref errMesg);
            return list;
        }
        #endregion

        #region GZQH

        public List<TraderQueryService.QH_CapitalAccountTableInfo> GZQHCapital
        {
            get
            {
                string msg = "";

                var cap = QueryQHCapital(gzqhAccount, ref msg);
                List<TraderQueryService.QH_CapitalAccountTableInfo> list = new List<TraderQueryService.QH_CapitalAccountTableInfo>();
                if (cap != null)
                    list.Add(cap);

                return list;
            }
        }

        public List<TraderFindService.QH_HoldAccountTableInfo> GZQHHold
        {
            get
            {
                string msg = "";

                var list = QueryQHHold(gzqhAccount, ref msg);

                List<TraderFindService.QH_HoldAccountTableInfo> holds = new List<TraderFindService.QH_HoldAccountTableInfo>();

                if (list != null)
                {
                    foreach (var entity in list)
                    {
                        holds.Add(entity.HoldFindResult);
                    }
                }

                return holds;
            }
        }

        public List<TraderFindService.QH_TodayEntrustTableInfo> GZQHTodayEntrust
        {
            get
            {
                string msg = "";

                return QueryQHTodayEntrust(gzqhAccount, ref msg);
            }
        }

        public List<TraderFindService.QH_TodayTradeTableInfo> GZQHTodayTrade
        {
            get
            {
                string msg = "";

                return QueryQHTodayTrade(gzqhAccount, ref msg);
            }
        }


        public bool CancelGZQHOrder(string entrustNumber, ref string errMsg)
        {
            if (!IsServiceOk)
            {
                errMsg = ServiceErrorMsg;
                return false;
            }

            TypesOrderStateType statetype;
            return doOrderClient.CancelStockIndexFuturesOrder(entrustNumber, ref errMsg, out statetype);
        }


        public OrderResponse DoGZQHOrder(StockIndexFuturesOrderRequest order)
        {
            if (!IsServiceOk)
                return new OrderResponse { OrderMessage = ServiceErrorMsg };

            order.ChannelID = Channelid;

            OrderResponse response = null;
            try
            {
                response = doOrderClient.DoStockIndexFuturesOrder(order);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                response = new OrderResponse();
                response.IsSuccess = false;
                response.OrderMessage = ServiceErrorMsg;
                IsServiceOk = false;
            }

            return response;
        }

        #endregion

        #region QH

        public TraderQueryService.QH_CapitalAccountTableInfo QueryQHCapital(string capitalAccount, ref string msg)
        {
            if (!IsServiceOk)
                return null;

            //var capital = traderFindClient.FuturesCapitalFind(capitalAccount, Types.CurrencyType.RMB, "", ref msg);

            //if (capital == null)
            //    return null;

            //if (capital.GTCapitalObj != null)
            //{
            //    var cap = capital.GTCapitalObj;

            //    return cap;
            //}
            var capital = commonQueryClient.QueryQH_CapitalAccountTableByAccount(out msg, capitalAccount, ReckoningCounter.PushBackTest.TraderQueryService.QueryTypeQueryCurrencyType.RMB);
            if (capital != null && capital.Count > 0)
            {
                return capital[0];
            }
            return null;
        }

        public List<TraderFindService.FuturesHoldFindResultEntity> QueryQHHold(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new TraderFindService.FuturesHoldConditionFindEntity();
            var holds = traderFindClient.FuturesHoldFind(out icount, capitalAccount, "", null,
                                                         0, int.MaxValue, ref strMessage);

            return holds;
        }

        public List<TraderFindService.QH_TodayEntrustTableInfo> QueryQHTodayEntrust(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<TraderFindService.QH_TodayEntrustTableInfo>();

            int icount;
            var secfe = new TraderFindService.FuturesEntrustConditionFindEntity();
            var tets = traderFindClient.FuturesTodayEntrustFindByQhCapitalAccount(out icount, capitalAccount,
                                                                                  "", null,
                                                                                  0, int.MaxValue,
                                                                                  ref strMessage);
            return tets;
        }

        public List<TraderFindService.QH_TodayTradeTableInfo> QueryQHTodayTrade(string capitalAccount, ref string strMessage)
        {
            if (!IsServiceOk)
                return new List<TraderFindService.QH_TodayTradeTableInfo>();

            int icount;
            var secfe = new TraderFindService.FuturesTradeConditionFindEntity();

            var trades = traderFindClient.FuturesTodayTradeFindByXhCapitalAccount(out icount,
                                                                                  out strMessage, capitalAccount,
                                                                                  "",
                                                                                  null, 0, int.MaxValue);

            return trades;
        }

        public List<HKMarketValue> QueryMarketValueQHHold(string code, ref string strMessage)
        {
            List<HKMarketValue> list = new List<HKMarketValue>();

            if (!IsServiceOk)
                return null;

            int icount;
            var shcfe = new TraderFindService.FuturesHoldConditionFindEntity();
            if (!string.IsNullOrEmpty(code))
            {
                shcfe.ContractCode = code;
            }
            var holds = traderFindClient.FuturesHoldFind(out icount, gzqhAccount, "", shcfe, 0, int.MaxValue, ref strMessage);
            if (holds == null)
            {
                return list;
            }
            foreach (var item in holds)
            {
                HKMarketValue hkmare = new HKMarketValue();
                hkmare.BelongMarket = item.BelongMarket;
                hkmare.BreakevenPrice = item.HoldFindResult.BreakevenPrice;
                hkmare.Code = item.HoldFindResult.Contract;
                hkmare.CostPrice = item.HoldFindResult.CostPrice;
                hkmare.CurrencyName = item.CurrencyName;
                hkmare.ErroNumber = item.ErroNumber;
                hkmare.ErroReason = item.ErroReason;
                hkmare.FloatProfitLoss = item.FloatProfitLoss;
                hkmare.HKName = item.ContractName;
                hkmare.HoldAveragePrice = item.HoldFindResult.HoldAveragePrice;
                hkmare.HoldSumAmount = item.HoldSumAmount;
                // 买方向的盯市盈亏=买方向的盯市盈亏=[持仓总量={0}*(当前价={1}-持仓均价={2})*交易单位倍数={3}]
                //sfre.MarketProfitLoss = computeTotal * (sfre.RealtimePrice - _QhAccountHold.HoldAveragePrice);
                //decimal holdSum = (decimal)item.FloatProfitLoss / (item.RealtimePrice - item.HoldFindResult.HoldAveragePrice);
                //if (holdSum == 0)
                //{
                hkmare.MarketValue = item.HoldSumAmount * item.RealtimePrice * 300;//因后台期货持仓查询返回没有计算市值，所以这里按后台公式自行计算，这里默认300
                //这里应该乘上数值倍数
                //}
                //else
                //{
                //  hkmare.MarketValue = holdSum * item.RealtimePrice;//因后台期货持仓查询返回没有计算市值，所以这里按后台公式自行计算
                //}
                hkmare.RealtimePrice = item.RealtimePrice;
                hkmare.MarketProfitLoss = item.MarketProfitLoss;

                hkmare.TraderId = item.TraderId;
                hkmare.VarietyCategories = item.VarietyCategories;
                list.Add(hkmare);
            }
            return list;
        }

        public TraderFindService.FuturesCapitalEntity QueryQHTotalCapital(Types.CurrencyType type, ref string msg)
        {
            if (!IsServiceOk)
                return null;

            return traderFindClient.FuturesCapitalFind(gzqhAccount, type, "", ref msg);

        }

        public List<QH_TradeCapitalFlowDetailInfo> QueryQHCapitalFlowDetail(TraderQueryService.QueryTypeQueryCurrencyType type, string pwd, out string msg)
        {
            msg = "";
            if (!IsServiceOk)
                return null;
            int total = 0;
            TraderQueryService.PagingInfo pageInfo = new TraderQueryService.PagingInfo();
            pageInfo.CurrentPage = 1;
            pageInfo.IsCount = false;
            pageInfo.PageLength = int.MaxValue;

            return commonQueryClient.PagingQueryQH_TradeCapitalFlowDetailByAccount(out total, out msg, traderId, pwd, null, null, type, pageInfo);

        }

        #endregion

        /// <summary>
        /// 现货历史委托查询
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="strMessage">信息</param>
        /// <returns></returns>
        internal List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo> QueryXHHisotryEntrust(out int icount,int currentPage, int pageLength, string entrustNumber, DateTime? startTime, DateTime? endTime, string code, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
                return new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo>();

            List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo> tets = new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryEntrustTableInfo>();
            //int icount;
            var secfe = new TraderFindService.SpotEntrustConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value.Date;
                secfe.EndTime = endTime.Value.Date.AddDays(1).AddSeconds(-1);
            }
            secfe.SpotCode = code;

            tets = traderFindClient.SpotHistoryEntrustFind(out icount, out strMessage, xhAccount, "", currentPage, pageLength, secfe);
            return tets;
        }

        /// <summary>
        /// 现货历史成交查询
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="strMessage">信息</param>
        /// <returns></returns>
        internal List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo> QueryXHHisotryTrade(out int icount, int currentPage, int pageLength, string entrustNumber, DateTime? startTime, DateTime? endTime, string code, ref string strMessage)
        {
            icount = 0;
            if (!IsServiceOk)
                return new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo>();

            List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo> tets = new List<ReckoningCounter.PushBackTest.TraderFindService.XH_HistoryTradeTableInfo>();
            //int icount;
            var secfe = new TraderFindService.SpotTradeConditionFindEntity();
            if (!string.IsNullOrEmpty(entrustNumber))
            {
                secfe.EntrustNumber = entrustNumber;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                secfe.StartTime = startTime.Value.Date;
                secfe.EndTime = endTime.Value.Date.AddDays(1).AddSeconds(-1);
            }
            secfe.SpotCode = code;

            tets = traderFindClient.SpotHistoryTradeFind(out icount, out strMessage, xhAccount, "", currentPage, pageLength, secfe);

            return tets;
        }
    }

    public class OrderCallBack : IOrderDealRptCallback
    {
        #region Implementation of IOrderDealRptCallback

        public void ProcessStockDealRpt(StockDealOrderPushBack drsip)
        {
            Program.MainForm.ProcessXHBack(drsip);
        }

        public void ProcessMercantileDealRpt(FutureDealOrderPushBack drmip)
        {
            Program.MainForm.ProcessSPQHBack(drmip);
        }

        public void ProcessStockIndexFuturesDealRpt(FutureDealOrderPushBack drsifi)
        {
            Program.MainForm.ProcessGZQHBack(drsifi);
        }

        public void ProcessHKDealRpt(HKDealOrderPushBack drsip)
        {
            Program.MainForm.ProcessHKBack(drsip);
        }

        public void ProcessHKModifyOrderRpt(HKModifyOrderPushBack mopb)
        {
            Program.MainForm.ProcessHKModifyOrderBack(mopb);
        }

        #endregion
    }


    public class CurrentQueryValue
    {
        /// <summary>
        /// 港股查询今日委托的委托编号
        /// </summary>
        public static string QueryHKEnNO = "";
        /// <summary>
        /// 港股查今日成交的委托编号
        /// </summary>
        public static string QueryHKTradeNO = "";
        /// <summary>
        /// 港股查询历史委托的委托编号
        /// </summary>
        public static string QueryHKHistoryEnNO = "";
        /// <summary>
        /// 港股查历史成交的委托编号
        /// </summary>
        public static string QueryHKHistoryTradeNO = "";

        /// <summary>
        /// 现货查询今日委托的委托编号
        /// </summary>
        public static string QueryXHEntrustNO = "";
        /// <summary>
        /// 现货查今日成交的委托编号
        /// </summary>
        public static string QueryXHTradeNO = "";


    }
}