using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;

namespace GTA.VTS.CustomersOrders.BLL
{

    /// <summary>
    /// 模块编号:
    /// 作用：期货消息逻辑类
    /// 作者：叶振东
    /// 编写日期：2010-01-25
    /// 作用：添加自动撤单功能
    /// 作者：李健华
    /// 编写日期：2010-05-11
    ///  </summary>
    public class SPQHMessageLogic : AutoCancleOperater
    {
        /// <summary>
        /// 商品期货委托缓存列表
        /// </summary>
        private SyncCache<string, QHMessage> qhMessageCache = new SyncCache<string, QHMessage>();
        /// <summary>
        /// 商品期货相关信息列表
        /// </summary>
        private List<QHMessage> qhMessageList = new List<QHMessage>();
        /// <summary>
        /// 读取委托单数据锁
        /// </summary>
        protected ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();




        /// <summary>
        /// 所有信息列表
        /// </summary>
        public List<QHMessage> MessageList
        {
            get { return qhMessageList; }
        }

        /// <summary>
        /// 缓存数据信息是否被改变过
        /// </summary>
        public bool HasChanged
        {
            get;
            set;
        }

        /// <summary>
        /// 下单委托操作相关转换信息并缓存相关信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="request"></param>
        public void ProcessDoOrder(OrderResponse response, MercantileFuturesOrderRequest request)
        {
            QHMessage message = new QHMessage();
            message.BuySell = request.BuySell == GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying ? "买" : "卖";
            message.CapitalAccount = request.FundAccountId;
            message.Code = request.Code;
            message.EntrustAmount = request.OrderAmount.ToString();
            message.EntrustNumber = response.OrderId;
            message.EntrustPrice = request.OrderPrice.ToString();
            message.EntrustType = request.OrderWay == TypesOrderPriceType.OPTLimited ? "限价" : "市价";
            message.OrderMessage = response.OrderMessage;
            message.OrderStatus = "未报02";
            message.TradeAmount = "0";
            message.TradeTime = "";
            message.OpenClose = Utils.GetFutureOpenCloseType(request.OpenCloseType);

            qhMessageList.Add(message);
            if (!response.IsSuccess)
            {
                message.EntrustNumber = Guid.NewGuid().ToString();
                message.OrderStatus = "废单06";
            }

            qhMessageCache.Add(message.EntrustNumber, message);

            HasChanged = true;
        }

        /// <summary>
        /// 成交回推处理相关信息刷新相关数据,并能显示相关信息
        /// </summary>
        /// <param name="drsifi"></param>
        public void ProcessPushBack(FutureDealOrderPushBack drsifi)
        {
            var tet = drsifi.StockIndexFuturesOrde;
            var deals = drsifi.FutureDealList;

            string entrustNumber = tet.EntrustNumber;
            if (!qhMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("[商品期货]委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = qhMessageCache.Get(entrustNumber);
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

        /// <summary>
        ///  更新委托单信息
        /// </summary>
        /// <param name="entrustNumber"></param>
        /// <param name="msg"></param>
        public void UpdateMessage(string entrustNumber, string msg)
        {
            if (string.IsNullOrEmpty(entrustNumber))
            {
                return;
            }

            if (!qhMessageCache.Contains(entrustNumber))
            {
                LogHelper.WriteDebug("委托" + entrustNumber + "不存在缓存中");
                return;
            }

            var message = qhMessageCache.Get(entrustNumber);
            message.OrderMessage = msg;

            HasChanged = true;
        }

        /// <summary>
        /// 清除所有相关委托列表缓存的信息
        /// </summary>
        public void ClearAll()
        {
            listLock.EnterWriteLock();
            try
            {
                qhMessageList.Clear();
                //缓存同步信息也清除
                qhMessageCache.Reset();
                //同样清除缓存自动撤单列表
                autoCanceOrder.Reset();
                HasChanged = true;
            }
            finally
            {
                listLock.ExitWriteLock();
            }
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
