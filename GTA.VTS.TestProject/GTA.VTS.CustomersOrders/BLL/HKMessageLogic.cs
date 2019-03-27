#region Using Namespace
using System;
using System.Collections.Generic;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;
using GTA.VTS.CustomersOrders.HKCommonQuery;
#endregion

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// 模块编号:
    /// 作用：港股消息逻辑类
    /// 作者：叶振东
    /// 编写日期：2009-12-28
    /// </summary>
    public class HKMessageLogic : AutoCancleOperater
    {
        private SyncCache<string, HKMessage> hkMessageCache = new SyncCache<string, HKMessage>();
        private List<HKMessage> hkMessageList = new List<HKMessage>();
        protected ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();


        public List<HKMessage> MessageList
        {
            get { return hkMessageList; }
        }

        public bool HasChanged
        {
            get;
            set;
        }

        public void ProcessDoOrder(OrderResponse response, HKOrderRequest request)
        {
            HKMessage message = new HKMessage();
            message.BuySell = request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying ? "买" : "卖";
            message.CapitalAccount = request.FundAccountId;
            message.Code = request.Code;
            message.EntrustAmount = request.OrderAmount.ToString();
            message.EntrustNumber = response.OrderId;
            message.EntrustPrice = request.OrderPrice.ToString();
            //message.EntrustType = request.OrderWay == TypesOrderPriceType.OPTLimited ? "限价" : "市价";
            switch (request.OrderWay)
            {
                case Types.HKPriceType.LO:
                    message.EntrustType = "限价单";
                    break;
                case Types.HKPriceType.ELO:
                    message.EntrustType = "增强限价单";
                    break;
                case Types.HKPriceType.SLO:
                    message.EntrustType = "特别限价单";
                    break;
            }

            message.OrderMessage = response.OrderMessage;
            message.OrderStatus = "未报02";
            message.TradeAmount = "0";
            message.TradeTime = "";

            listLock.EnterWriteLock();
            try
            {
                hkMessageList.Add(message);
            }
            finally
            {
                listLock.ExitWriteLock();
            }

            if (!response.IsSuccess)
            {
                message.EntrustNumber = System.Guid.NewGuid().ToString();
                message.OrderStatus = "废单06";
            }

            hkMessageCache.Add(message.EntrustNumber, message);

            HasChanged = true;
        }

        public void ClearAll()
        {
            listLock.EnterWriteLock();
            try
            {
                hkMessageList.Clear();
                HasChanged = true;
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        public List<HKMessage> ClearAllFinalStateOrder()
        {
            List<HKMessage> list = new List<HKMessage>();
            listLock.EnterWriteLock();
            try
            {
                for (int i = hkMessageList.Count - 1; i >= 0; i--)
                {
                    var message = hkMessageList[i];
                    bool isFinalState = Utils.IsFinalState(message.OrderStatus);
                    if (isFinalState)
                    {
                        hkMessageList.Remove(message);
                        list.Add(message);
                        HasChanged = true;
                    }
                }
            }
            finally
            {
                listLock.ExitWriteLock();
            }

            return list;
        }

        public void ProcessPushBack(HKDealOrderPushBack drsip)
        {
            var tet = drsip.HKOrderEntity;
            var deals = drsip.HKDealList;

            string entrustNumber = tet.EntrustNumber;

            //因为Type3的改单的改单回报有可能比这个pushback要迟，所以稍微延迟一下
            //进行多次查找
            bool canFind = false;
            for (int i = 0; i < 3; i++)
            {
                if (hkMessageCache.Contains(entrustNumber))
                {
                    canFind = true;
                    break;
                }

                Thread.Sleep(1000);
            }

            if (!canFind)
            {
                if (!hkMessageCache.Contains(entrustNumber))
                {
                    LogHelper.WriteDebug("[港股]委托" + entrustNumber + "不存在缓存中");
                    return;
                }
            }

            var message = hkMessageCache.Get(entrustNumber);
            message.EntrustAmount = tet.EntrustAmount.ToString();
            message.TradeAmount = tet.TradeAmount.ToString();
            message.CancelAmount = tet.CancelAmount.ToString();
            message.OrderMessage = tet.OrderMessage;
            message.OrderStatus = Utils.GetOrderStateMsg(tet.OrderStatusId);

            if (deals.Count > 0)
            {
                message.TradeTime = deals[deals.Count - 1].TradeTime.ToString();
            }

            HasChanged = true;
        }

        public void UpdateMessage(string entrustNumber, string msg)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            if (!hkMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = hkMessageCache.Get(entrustNumber);
            message.OrderMessage = msg;

            HasChanged = true;
        }

        public void ProcessModifyBack(HKModifyOrderPushBack back)
        {
            string entrustNumber = back.OriginalRequestNumber;
            if (!hkMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = hkMessageCache.Get(entrustNumber);
            message.OrderMessage = back.Message;

            if (back.OriginalRequestNumber != back.NewRequestNumber && !string.IsNullOrEmpty(back.NewRequestNumber))
            {
                //改单成功，并且有一个新委托(Type1,Type3)

                //1.旧委托修改状态


                //2.生成新委托对象到列表中显示
                ShowNewEntrust(back.NewRequestNumber);
            }

            HasChanged = true;
        }

        /// <summary>
        /// 为了方便直接根据新的委托单号直接从柜台查询
        /// </summary>
        /// <param name="number"></param>
        private void ShowNewEntrust(string number)
        {
            //string desc = "HKMessageLogic.ShowNewEntrust显示新委托到UI中，EntrustNumber=" + number;
            //LogHelper.WriteDebug(desc);

            string strMessage = "";
            string capitalAccount = ServerConfig.HKCapitalAccount;
            var tets = WCFServices.Instance.QueryHKTodayEntrust(capitalAccount, number, ref strMessage);

            if (tets == null)
            {
                return;
            }

            if (tets.Count == 0)
            {
                return;
            }

            var tet = tets[0];

            InsertModifyBack(tet);
        }

        public void InsertModifyBack(HK_TodayEntrustInfo tet)
        {
            HKMessage message = new HKMessage();
            message.BuySell = tet.BuySellTypeID == (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying ? "买" : "卖";
            message.CapitalAccount = tet.CapitalAccount;
            message.Code = tet.Code;
            message.EntrustAmount = tet.EntrustAmount.ToString();
            message.EntrustNumber = tet.EntrustNumber;
            message.EntrustPrice = tet.EntrustPrice.ToString();
            message.IsModifyOrder = "是";
            message.ModifyOrderNumber = tet.ModifyOrderNumber;

            switch (tet.OrderPriceType)
            {
                case (int)Types.HKPriceType.LO:
                    message.EntrustType = "限价单";
                    break;
                case (int)Types.HKPriceType.ELO:
                    message.EntrustType = "增强限价单";
                    break;
                case (int)Types.HKPriceType.SLO:
                    message.EntrustType = "特别限价单";
                    break;
            }

            message.OrderMessage = tet.OrderMessage;
            message.OrderStatus = Utils.GetOrderStateMsg(tet.OrderStatusID);
            message.TradeAmount = tet.TradeAmount.ToString();
            message.TradeTime = "";

            listLock.EnterWriteLock();
            try
            {
                hkMessageList.Add(message);
            }
            finally
            {
                listLock.ExitWriteLock();
            }

            hkMessageCache.Add(message.EntrustNumber, message);

            HasChanged = true;
        }

        /// <summary>
        /// 委托单号获取事件
        /// </summary>
        public static event EntrustNoEventHandler OnEntrustSelected;

        /// <summary>
        /// 触发委托单号获取事件
        /// </summary>
        /// <param name="entrustNumber"></param>
        public static void FireEntrustSelectedEvent(string entrustNumber)
        {
            if (OnEntrustSelected != null)
            {
                OnEntrustSelected(entrustNumber);
            }
        }
    }
}