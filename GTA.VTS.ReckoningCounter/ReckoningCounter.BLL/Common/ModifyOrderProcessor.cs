#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.delegateoffer;
using ReckoningCounter.BLL.DelegateValidate;
using ReckoningCounter.BLL.Reckoning.Logic;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.BLL.DelegateValidate.ManagementCenter;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 港股改单处理器
    /// 作者：宋涛
    /// </summary>
    public class ModifyOrderProcessor
    {
        //类型1改单委托缓存
        public static ModifyOrderProcessor Instance = new ModifyOrderProcessor();
        private SyncCache<string, HKModifyOrderRequest> type1Cache = new SyncCache<string, HKModifyOrderRequest>();

        //类型2改单委托缓存
        private SyncCache<string, HKModifyOrderRequest> type2Cache = new SyncCache<string, HKModifyOrderRequest>();

        //类型3改单委托缓存
        private SyncCache<string, HKModifyOrderRequest> type3Cache = new SyncCache<string, HKModifyOrderRequest>();

        public OrderResponse Process(HKModifyOrderRequest hkOrder)
        {
            LogHelper.WriteDebug("M——>ModifyOrderProcessor.Process接收改单入口" + hkOrder);

            bool canModifyOrder = false;
            OrderResponse orResult = new OrderResponse();
            string strErrorMessage = "";

            //hkOrder.ID = HKCommonLogic.BuildHKOrderNo();
            hkOrder.ID = Guid.NewGuid().ToString();

            #region 原始单位及成交量转换

            var code = hkOrder.Code;
            var oriUnitType = hkOrder.OrderUnitType;
            decimal scale;
            Types.UnitType matchUnitType;

            bool canConvert = OrderAccepter.ConvertUnitType(Types.BreedClassTypeEnum.HKStock, code, oriUnitType, out scale, out matchUnitType);
            if (!canConvert)
            {
                strErrorMessage = @"GT-2450:[港股委托]无法进行行情单位转换.";
                goto EndProcess;
            }
            LogHelper.WriteDebug("委托单：" + hkOrder.EntrustNubmer + "当前改单获取到的转换倍数为: " + scale);
            hkOrder.OrderUnitType = matchUnitType;
            hkOrder.OrderAmount = hkOrder.OrderAmount * (float)scale;

            #endregion

            //记录改单操作记录数据
            bool canSave = HKDataAccess.AddHKModifyOrderRequest(hkOrder);
            if (!canSave)
            {
                strErrorMessage = @"GT-2451:[港股改单委托]无法保持改单委托，请查看错误日志.";
                LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);

                goto EndProcess;
            }

            #region 校验

            if ((string.IsNullOrEmpty(hkOrder.TraderId) && string.IsNullOrEmpty(hkOrder.FundAccountId)))
            {
                strErrorMessage = @"GT-2452:[港股改单委托]交易员ID或资金帐户无效.";
                LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);

                goto EndProcess;
            }

            //柜台委托时间判断
            if (!ValidateCenter.IsCountTradingTime(Types.BreedClassTypeEnum.HKStock, hkOrder.Code, ref strErrorMessage))
            {
                LogHelper.WriteInfo("ModifyOrderProcessor.Process" + strErrorMessage);
                string oriMsg = "当前时间不接受委托";
                if (strErrorMessage.Length > 0)
                    oriMsg = strErrorMessage;

                strErrorMessage = @"GT-2453:[港股改单委托]" + oriMsg;
                LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);
                goto EndProcess;
            }

            HK_TodayEntrustInfo tet = HKDataAccess.GetTodayEntrustTable(hkOrder.EntrustNubmer);
            if (tet == null)
            {
                strErrorMessage = "GT-2454:[港股改单委托]无委托对象信息";
                LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);

                goto EndProcess;
            }

            if ((float)tet.EntrustPrice == hkOrder.OrderPrice && tet.EntrustAmount == (int)hkOrder.OrderAmount)
            {
                strErrorMessage = "GT-2455:[港股改单委托]无效改单，改单量、改单价与原始委托相同";
                LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);

                goto EndProcess;
            }

            //港股改单规则检验，这里有些与真实下单有些重复，但这样可以提高改单的成功率
            if (!McValidater.GetInstance().ValidateHKModifyOrderRule(hkOrder, tet, ref strErrorMessage))
            {
                LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);
                goto EndProcess;
            }

            #endregion

            //未报
            if (tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSUnRequired)
            {
                ProcessType1(hkOrder);
                canModifyOrder = true;
                goto EndProcess;
            }

            #region 已报、部成

            if (tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSIsRequired ||
                tet.OrderStatusID == (int)Entity.Contants.Types.OrderStateType.DOSPartDealed)
            {
                //如果不在交易时间内，那么直接到缓存中删除原来的委托，生成新委托
                if (!ValidateCenter.IsMatchTradingTime(Types.BreedClassTypeEnum.HKStock, hkOrder.Code))
                {
                    strErrorMessage = "GT-2456:[港股改单委托]不在交易时间内";
                    LogHelper.WriteInfo(strErrorMessage + "," + hkOrder);

                    goto EndProcess;
                }

                if ((decimal)hkOrder.OrderPrice == tet.EntrustPrice && hkOrder.OrderAmount < tet.EntrustAmount)
                {
                    ProcessType2(hkOrder);
                    canModifyOrder = true;
                }
                else
                {
                    ProcessType3(hkOrder);
                    canModifyOrder = true;
                    goto EndProcess;
                }
            }

            #endregion

        EndProcess:
            //结束
            if (canModifyOrder)
            {
                hkOrder.ModifyOrderDateTime = DateTime.Now;
                orResult.OrderDatetime = DateTime.Now;
            }
            else
            {
                int type = 0;
                bool canGetErrorCode = Utils.GetErrorCode(strErrorMessage, out type);
                if (canGetErrorCode)
                    orResult.ErrorType = type;

                hkOrder.Message = strErrorMessage;
                orResult.OrderMessage = strErrorMessage;
            }

            //更新改单委托
            HKDataAccess.UpdateModifyOrderRequest(hkOrder);

            orResult.IsSuccess = canModifyOrder;
            orResult.OrderId = hkOrder.ID;
            return orResult;
        }

        #region Type1

        /// <summary>
        /// 处理Type1类型改单-未报
        /// </summary>
        /// <param name="request">改单委托</param>
        private void ProcessType1(HKModifyOrderRequest request)
        {
            LogHelper.WriteDebug("M————>ModifyOrderProcessor.ProcessType1接收类型1改单请求" + request);

            //如果不在交易时间内，那么直接到缓存中删除原来的委托，生成新委托
            if (!ValidateCenter.IsMatchTradingTime(Types.BreedClassTypeEnum.HKStock, request.Code))
            {
                LogHelper.WriteDebug("类型1改单请求" + request + "不在交易时间时间内，直接到缓存中删除原来的委托，生成新委托");
                //1.删除缓存中的委托信息
                OrderOfferCenter.Instance._orderCache.DeleteHKOrder(request.Code, request.EntrustNubmer);

                //2.进行内存撤单
                var tet = HKDataAccess.GetTodayEntrustTable(request.EntrustNubmer);
                OrderOfferCenter.Instance.IntelnalCancelHKOrder(tet, "预委托被改单，作废");
                //tet.OrderStatusID = (int)Entity.Contants.Types.OrderStateType.DOSCanceled;
                //tet.OrderMessage = "未报的委托被改单，作废";
                //HKDataAccess.UpdateEntrustTable(tet);

                //3.回推原始委托变化到前台
                ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject =
                    new ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo>();
                reckonEndObject.IsSuccess = true;
                reckonEndObject.EntrustTable = tet;
                reckonEndObject.TradeTableList = new List<HK_TodayTradeInfo>();
                reckonEndObject.TradeID = request.TraderId;
                reckonEndObject.Message = "";

                CounterOrderService.Instance.AcceptHKDealOrder(reckonEndObject);

                ProcessType1_NewOrder(request);
            }
            else
            {
                //否则的话，放入类型1改单缓存，等原始委托报盘时再处理
                LogHelper.WriteDebug("M————>ModifyOrderProcessor.ProcessType1类型1改单请求在交易时间内，放入缓存等待报盘时查找核对" + request);

                AddType1Reqest(request);
            }
        }

        /// <summary>
        /// 类型1改单——生成新委托，下单，并回推改单结果到前台
        /// </summary>
        /// <param name="request"></param>
        internal void ProcessType1_NewOrder(HKModifyOrderRequest request)
        {
            LogHelper.WriteDebug("M——————>ModifyOrderProcessor.ProcessType1_NewOrder生成新委托，下单，并回推改单结果到前台" + request);

            HKOrderRequest newRequest = CreateNewType1Request(request);
            HKModifyOrderPushBack pushBack = new HKModifyOrderPushBack();
            pushBack.OriginalRequestNumber = request.EntrustNubmer;
            pushBack.TradeID = request.TraderId;
            pushBack.CallbackChannlId = request.ChannelID;

            var res = OrderAccepterService.Service.DoHKOrder(newRequest);

            pushBack.IsSuccess = res.IsSuccess;
            pushBack.Message = res.OrderMessage;


            if (res.IsSuccess)
            {
                HKDataAccess.UpdateEntrustModifyOrderNumber(res.OrderId, request.EntrustNubmer);
                //记录成功改单委托记录明细，方便查询和关联查询
                HKDataAccess.AddModifyOrderSuccessDatils(res.OrderId, request.EntrustNubmer, 1);

                pushBack.NewRequestNumber = res.OrderId;
            }
            else
            {
                request.Message = res.OrderMessage;
                HKDataAccess.UpdateModifyOrderRequest(request);
            }

            //将改单结果推给前台
            CounterOrderService.Instance.AcceptHKModifyOrder(pushBack);
        }

        /// <summary>
        /// 添加类型1改单委托到缓存中
        /// </summary>
        /// <param name="request"></param>
        private void AddType1Reqest(HKModifyOrderRequest request)
        {
            type1Cache.Add(request.EntrustNubmer, request);
        }

        /// <summary>
        /// 类型1改单委托缓存是否存在对应的委托
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool IsExistType1Request(string entrustNumber, out HKModifyOrderRequest request)
        {
            request = null;

            if (type1Cache.Contains(entrustNumber))
            {
                request = type1Cache.Get(entrustNumber);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除类型1改单委托从缓存中
        /// </summary>
        /// <param name="entrustNumber"></param>
        public void DeleteType1Reqeust(string entrustNumber)
        {
            type1Cache.Delete(entrustNumber);
        }

        /// <summary>
        /// 根据港股改单委托创建新的委托
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private HKOrderRequest CreateNewType1Request(HKModifyOrderRequest request)
        {
            var tet = HKDataAccess.GetTodayEntrustTable(request.EntrustNubmer);

            HKOrderRequest newRequest = new HKOrderRequest();
            newRequest.BuySell = (Types.TransactionDirection)tet.BuySellTypeID;
            newRequest.ChannelID = tet.CallbackChannlID;
            newRequest.Code = request.Code;
            newRequest.FundAccountId = request.FundAccountId;
            newRequest.OrderAmount = request.OrderAmount;
            newRequest.OrderPrice = request.OrderPrice;
            newRequest.OrderUnitType = (Types.UnitType)tet.TradeUnitID;
            newRequest.OrderWay = Types.HKPriceType.LO;
            newRequest.PortfoliosId = tet.PortfolioLogo;
            newRequest.TraderId = request.TraderId;
            newRequest.TraderPassword = request.TraderPassword;

            return newRequest;
        }

        #endregion

        #region Type2

        /// <summary>
        /// 处理Type2类型改单-价不变量减
        /// </summary>
        /// <param name="request">改单委托</param>
        private void ProcessType2(HKModifyOrderRequest request)
        {
            LogHelper.WriteDebug("M————>ModifyOrderProcessor.ProcessType2接收类型2改单请求" + request);

            OrderOfferCenter.Instance.OfferHKModifyOrder(request);
        }

        /// <summary>
        /// 添加类型2改单委托到缓存中
        /// </summary>
        /// <param name="request"></param>
        public void AddType2Reqest(HKModifyOrderRequest request)
        {
            type2Cache.Add(request.EntrustNubmer, request);
        }

        /// <summary>
        /// 类型2改单委托缓存是否存在对应的委托
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool IsExistType2Request(string entrustNumber, out HKModifyOrderRequest request)
        {
            request = null;

            if (type2Cache.Contains(entrustNumber))
            {
                request = type2Cache.Get(entrustNumber);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除类型2改单委托从缓存中
        /// </summary>
        /// <param name="entrustNumber"></param>
        private void DeleteType2Reqeust(string entrustNumber)
        {
            type2Cache.Delete(entrustNumber);
        }

        /// <summary>
        /// 更新类型2改单的各种状态变化
        /// </summary>
        /// <param name="entrustNumber"></param>
        public void ProcessType2ModifyOrder(string entrustNumber)
        {
            string desc = "M<---——ModifyOrderProcessor.ProcessType2ModifyOrder类型2改单——原始委托清算完毕,更新状态";
            LogHelper.WriteDebug(desc);

            //===update 2009-11-08 李健华 
            //HK_TodayEntrustInfo tet = HKDataAccess.GetTodayEntrustTable(entrustNumber);

            //如果新单号与原始单号相同，那么代表是一个类型2的改单
            //tet.ModifyOrderNumber = entrustNumber;
            //tet.IsModifyOrder = true;
            //HKDataAccess.UpdateEntrustTable(tet);
            //为了不更新区他字段 和不用再查询一次再更新
            HKDataAccess.UpdateEntrustModifyOrderNumber(entrustNumber, entrustNumber);
            //记录成功改单委托记录明细，方便查询和关联查询
            HKDataAccess.AddModifyOrderSuccessDatils(entrustNumber, entrustNumber, 2);
            //=======================

            HK_ModifyOrderRequestDal dal = new HK_ModifyOrderRequestDal();
            HKModifyOrderRequest request = dal.GetModel(entrustNumber);
            if (request != null)
            {
                request.ModifyOrderDateTime = DateTime.Now;
                HKDataAccess.UpdateModifyOrderRequest(request);
            }

            DeleteType2Reqeust(entrustNumber);
        }

        #endregion

        #region Type3

        /// <summary>
        /// 处理Type3类型改单-价变量变
        /// </summary>
        /// <param name="request">改单委托</param>
        private void ProcessType3(HKModifyOrderRequest request)
        {
            LogHelper.WriteDebug("M————>ModifyOrderProcessor.ProcessType3接收类型3改单请求" + request);

            string strMessage = "";
            Entity.Contants.Types.OrderStateType ost;
            int errType;

            //先进行主动撤单
            bool result = OrderAccepterService.Service.CancelHKOrder(request.EntrustNubmer, ref strMessage, out ost,
                                                                     out errType);

            //如果发送撤单委托都进行不成功，那么整个改单委托直接作废
            if (!result)
            {
                HKModifyOrderPushBack pushBack = new HKModifyOrderPushBack();
                pushBack.IsSuccess = false;
                pushBack.Message = strMessage;
                pushBack.OriginalRequestNumber = request.EntrustNubmer;
                pushBack.TradeID = request.TraderId;
                pushBack.CallbackChannlId = request.ChannelID;

                request.Message = strMessage;
                HKDataAccess.UpdateModifyOrderRequest(request);

                LogHelper.WriteDebug("M————>ModifyOrderProcessor.ProcessType3发送撤单委托不成功，整个改单委托直接作废" + request +
                                     "Message=" + strMessage);

                //将类型3改单失败结果推给前台
                CounterOrderService.Instance.AcceptHKModifyOrder(pushBack);

                return;
            }

            //成功的话，放入类型3改单缓存，等撤单成交回报清算完毕时再处理
            AddType3Reqest(request);
        }

        /// <summary>
        /// 添加类型3改单委托到缓存中
        /// </summary>
        /// <param name="request"></param>
        private void AddType3Reqest(HKModifyOrderRequest request)
        {
            type3Cache.Add(request.EntrustNubmer, request);
        }

        /// <summary>
        /// 类型3改单委托缓存是否存在对应的委托
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool IsExistType3Request(string entrustNumber, out HKModifyOrderRequest request)
        {
            request = null;

            if (type3Cache.Contains(entrustNumber))
            {
                request = type3Cache.Get(entrustNumber);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除类型3改单委托从缓存中
        /// </summary>
        /// <param name="entrustNumber"></param>
        public void DeleteType3Reqeust(string entrustNumber)
        {
            type3Cache.Delete(entrustNumber);
        }

        /// <summary>
        /// 当原始委托撤单清算完毕后，由此方法进行后继处理
        /// 生成新的委托进行下单，并修改原始委托相关信息
        /// </summary>
        /// <param name="request"></param>
        public void ProcessType3NewOrder(HKModifyOrderRequest request)
        {
            string desc = "M<---——ModifyOrderProcessor.ProcessType3NewOrder类型3改单——原始委托撤单清算完毕,生成新的委托下单，改单对象" + request;
            LogHelper.WriteDebug(desc);

            HK_TodayEntrustInfo tet = HKDataAccess.GetTodayEntrustTable(request.EntrustNubmer);

            string strMessage = "";
            OrderResponse res = null;

            HKModifyOrderPushBack pushBack = new HKModifyOrderPushBack();
            pushBack.OriginalRequestNumber = request.EntrustNubmer;
            pushBack.TradeID = request.TraderId;
            pushBack.CallbackChannlId = request.ChannelID;

            //计算出需要下的量
            float amount = request.OrderAmount - tet.TradeAmount;

            //如果成交量已经大于等于当前改单量，那么无法再进行改单操作，改单失败
            if (amount <= 0)
            {
                string format = "GT-2456:[港股改单委托]当前委托已成交数量{0}大于等于改单量{1}";
                strMessage = string.Format(format, tet.TradeAmount, request.OrderAmount);
                LogHelper.WriteDebug("M<---——ModifyOrderProcessor.ProcessType3NewOrder类型3改单失败" + request + "Message=" +
                                     strMessage);

                pushBack.Message = strMessage;
                pushBack.IsSuccess = false;

                //将改单结果推给前台
                CounterOrderService.Instance.AcceptHKModifyOrder(pushBack);

                //更新改单委托记录表的信息
                request.Message = strMessage;
                HKDataAccess.UpdateModifyOrderRequest(request);
            }
            else
            {
                HKOrderRequest newRequest = CreateNewType3Request(request, tet, amount);

                res = OrderAccepterService.Service.DoHKOrder(newRequest);
                pushBack.NewRequestNumber = res.OrderId;
                pushBack.Message = res.OrderMessage;

                string txtMsg = "";
                if (res.IsSuccess)
                {
                    txtMsg = "类型3改单成功";
                    //更新改单后成功下单的委托单，--更新是一笔改单下单委托，和原来被改单的委托单号
                    HKDataAccess.UpdateEntrustModifyOrderNumber(res.OrderId, tet.EntrustNumber);
                    //记录成功改单委托记录明细，方便查询和关联查询
                    HKDataAccess.AddModifyOrderSuccessDatils(res.OrderId, tet.EntrustNumber, 3);

                }
                else
                {
                    string desc2 = "M<---——ModifyOrderProcessor.ProcessType3NewOrder类型3改单——生成新的委托下单失败，改单对象" + request +
                                                     "Message=" + res.OrderMessage;
                    LogHelper.WriteDebug(desc2);

                    txtMsg = "GT-2457:类型3改单生成新的委托下单失败" + res.OrderMessage;

                    //更新改单委托记录表的信息
                    request.Message = txtMsg;
                    HKDataAccess.UpdateModifyOrderRequest(request);
                }

                //将改单结果推给前台
                CounterOrderService.Instance.AcceptHKModifyOrder(pushBack);

                ////修改原始委托信息
                //tet.ModifyOrderNumber = request.EntrustNubmer;
                //tet.IsModifyOrder = true;
                //HKDataAccess.UpdateEntrustTable(tet);

                //request.ModifyOrderDateTime = DateTime.Now; 

                //=====add 李健华 2009-11-07=========
                //则更新原始委托单信息
                HKDataAccess.UpdateEntrustOrderMessage(tet.EntrustNumber, txtMsg);
                //===========
            }

            DeleteType3Reqeust(request.EntrustNubmer);
        }

        private HKOrderRequest CreateNewType3Request(HKModifyOrderRequest request, HK_TodayEntrustInfo tet, float amount)
        {
            HKOrderRequest newRequest = new HKOrderRequest();
            newRequest.BuySell = (Types.TransactionDirection)tet.BuySellTypeID;
            newRequest.ChannelID = tet.CallbackChannlID;
            newRequest.Code = request.Code;
            newRequest.FundAccountId = request.FundAccountId;
            newRequest.OrderAmount = amount;
            newRequest.OrderPrice = request.OrderPrice;
            newRequest.OrderUnitType = (Types.UnitType)tet.TradeUnitID;
            newRequest.OrderWay = Types.HKPriceType.LO;
            newRequest.PortfoliosId = tet.PortfolioLogo;
            newRequest.TraderId = request.TraderId;
            newRequest.TraderPassword = request.TraderPassword;

            return newRequest;
        }

        #endregion
    }
}