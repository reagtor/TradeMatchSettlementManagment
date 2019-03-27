#region Using Namespace

using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using Amib.Threading;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.Reckoning.Logic;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.QH;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.QH;
using ReckoningCounter.Entity.Model.XH;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.BLL.ManagementCenter;

#endregion

namespace ReckoningCounter.BLL
{
    public delegate void PushChannelDelegate(string channel);

    /// <summary>
    /// 缓冲元素
    /// </summary>
    /// <typeparam name="TBizItem"></typeparam>
    internal class BufferItem<TBizItem>
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="item"></param>
        /// <param name="strChannelId"></param>
        public BufferItem(TBizItem item, string strChannelId)
        {
            this.Item = item;
            this.ChannelId = strChannelId;
        }

        /// <summary>
        /// 实体
        /// </summary>
        public TBizItem Item { get; private set; }

        /// <summary>
        /// 通道ID
        /// </summary>
        public string ChannelId { get; private set; }
    }

    /// <summary>
    /// 委托单接收单一进一静态类实例
    /// </summary>
    public static class OrderAccepterService
    {
        private static OrderAccepter accepter = new OrderAccepter();

        public static OrderAccepter Service
        {
            get { return accepter; }
        }
    }

    /// <summary>
    /// 下单通道服务实例
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DoOrderService : IDoOrder
    {
        #region 现货
        /// <summary>
        /// 现货下单方法
        /// </summary>
        /// <param name="stockorder">现货下单实体</param>
        /// <returns></returns>
        public OrderResponse DoStockOrder(StockOrderRequest stockorder)
        {
            //PerfMonitor.ReceiveOrder();
            //var beginTime = PerfMonitor.Begin();
            var result = OrderAccepterService.Service.DoStockOrder(stockorder);
            //PerfMonitor.End(beginTime,"WcfOrderService.DoStockOrder");

            return result;
        }

        /// <summary>
        ///  商品期货撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托ID</param>
        /// <param name="message">撤单返回信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <param name="errorType">错误类型（编码）</param>
        /// <returns></returns>
        public bool CancelStockOrder2(string orderId, ref string message, out Types.OrderStateType ost, out int errorType)
        {
            //PerfMonitor.ReceiveOrder();
            //var beginTime = PerfMonitor.Begin();
            var result = OrderAccepterService.Service.CancelStockOrder(orderId, ref message, out ost, out errorType);
            //PerfMonitor.End(beginTime, "WcfOrderService.CancelStockOrder2");

            return result;
        }

        /// <summary>
        ///  商品期货撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托ID</param>
        /// <param name="message">撤单返回信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <returns></returns>
        public bool CancelStockOrder(string orderId, ref string message, out Types.OrderStateType ost)
        {
            //PerfMonitor.ReceiveOrder();
            //var beginTime = PerfMonitor.Begin();
            int errorType = 0;
            var result = OrderAccepterService.Service.CancelStockOrder(orderId, ref message, out ost, out errorType);
            //PerfMonitor.End(beginTime, "WcfOrderService.CancelStockOrder");

            return result;
        }

        #endregion

        #region 商品期货

        /// <summary>
        /// 商品期货下单方法
        /// </summary>
        /// <param name="futuresorder">商品期货下单实体</param>
        /// <returns></returns>
        public OrderResponse DoMercantileFuturesOrder(MercantileFuturesOrderRequest futuresorder)
        {
            return OrderAccepterService.Service.DoMercantileFuturesOrder(futuresorder);
        }

        /// <summary>
        ///  商品期货撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托ID</param>
        /// <param name="message">撤单返回信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <param name="errorType">错误类型（编码）</param>
        /// <returns></returns>
        public bool CancelMercantileFuturesOrder2(string orderId, ref string message, out Types.OrderStateType ost, out int errorType)
        {
            ost = Types.OrderStateType.None;
            return OrderAccepterService.Service.CancelMercantileFuturesOrder(orderId, ref message, out errorType);
        }

        /// <summary>
        ///  商品期货撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托ID</param>
        /// <param name="message">撤单返回信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <returns></returns>
        public bool CancelMercantileFuturesOrder(string orderId, ref string message, out Types.OrderStateType ost)
        {
            ost = Types.OrderStateType.None;
            int errorType = 0;
            return OrderAccepterService.Service.CancelMercantileFuturesOrder(orderId, ref message, out errorType);
        }

        #endregion

        #region 股指期货
        /// <summary>
        /// 股指期货下单方法
        /// </summary>
        /// <param name="futuresorder">股指期货委托单实体</param>
        /// <returns></returns>
        public OrderResponse DoStockIndexFuturesOrder(StockIndexFuturesOrderRequest futuresorder)
        {
            return OrderAccepterService.Service.DoStockIndexFuturesOrder(futuresorder);
        }

        /// <summary>
        ///  股指期货撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托ID</param>
        /// <param name="message">撤单返回信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <param name="errorType">错误类型（编码）</param>
        /// <returns></returns>
        public bool CancelStockIndexFuturesOrder2(string orderId, ref string message, out Types.OrderStateType ost, out int errorType)
        {
            ost = Types.OrderStateType.None;
            return OrderAccepterService.Service.CancelStockIndexFuturesOrder(orderId, ref message, out errorType);
        }

        /// <summary>
        ///  股指期货撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托ID</param>
        /// <param name="message">撤单返回信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <returns></returns>
        public bool CancelStockIndexFuturesOrder(string orderId, ref string message, out Types.OrderStateType ost)
        {
            ost = Types.OrderStateType.None;
            int errorType = 0;
            return OrderAccepterService.Service.CancelStockIndexFuturesOrder(orderId, ref message, out errorType);
        }

        #endregion

        #region 港股
        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="hkorder">港股下单实体</param>
        /// <returns></returns>
        public OrderResponse DoHKOrder(HKOrderRequest hkorder)
        {
            var result = OrderAccepterService.Service.DoHKOrder(hkorder);
            return result;
        }
        /// <summary>
        /// 港股委托改单
        /// </summary>
        /// <param name="hkorder">改单实体</param>
        /// <returns></returns>
        public OrderResponse DoHKModifyOrder(HKModifyOrderRequest hkorder)
        {
            var result = OrderAccepterService.Service.ModifyHKOrder(hkorder);
            return result;
        }

        /// <summary>
        /// 港股撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托单号</param>
        /// <param name="message">撤单信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <param name="errorType">撤单错误类型</param>
        /// <returns></returns>
        public bool CancelHKOrder2(string orderId, ref string message, out Types.OrderStateType ost, out int errorType)
        {
            var result = OrderAccepterService.Service.CancelHKOrder(orderId, ref message, out ost, out errorType);

            return result;
        }
        /// <summary>
        /// 港股撤单方法
        /// </summary>
        /// <param name="orderId">撤单委托单号</param>
        /// <param name="message">撤单信息</param>
        /// <param name="ost">撤单后委托单状态</param>
        /// <returns></returns>
        public bool CancelHKOrder(string orderId, ref string message, out Types.OrderStateType ost)
        {
            int errorType = 0;
            var result = OrderAccepterService.Service.CancelHKOrder(orderId, ref message, out ost, out errorType);

            return result;
        }

        #endregion

        //private OrderAccepter _orderAccepterInstance = OrderAccepterService.Service;
    }

    /// <summary>
    /// 柜台服务wcf远程对象
    /// 职责
    /// 1.管理回送通道
    /// 2.接收wcf过程调用,并把服务传递为实际的OrderServiceBase服务对象
    /// 作者 ; 朱 亮
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CounterOrderService : /*IDoOrder,*/ IOrderDealRpt
    {
        #region 变量定义

        private static volatile CounterOrderService _instance;
        private static object lockObject = new object();

        /// <summary>
        /// 线程池
        /// </summary>
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 100 };

        #endregion

        #region 单例 入口

        /// <summary>
        /// 单例
        /// </summary>
        public static CounterOrderService Instance
        {
            get
            {
                //if (_instance == null)
                //    _instance = new CounterOrderService(1);
                //return _instance;

                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new CounterOrderService();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region == 字段/属性 ==

        ///// <summary>
        ///// 回报并行工作数（每种类型，如现货，股指期货，商品期货）
        ///// /// </summary>      
        //private int _callbackDealWorkingThreadCount = 3;

        ///// <summary>
        ///// 商品期货成回回调工作容器
        ///// </summary>
        //private List<QueueBufferBase<BufferItem<FutureDealOrderPushBack>>> _mercantileDealFutureCallbackQueueList;

        /// <summary>
        /// 上下文缓存
        /// </summary>
        private Dictionary<string, OperationContext> _operationContexts = new Dictionary<string, OperationContext>();

        ///// <summary>
        ///// 委托接收业务处理
        ///// </summary>
        //private OrderAccepter _orderAccepterInstance;

        ///// <summary>
        ///// 现货成回回调工作容器
        ///// </summary>
        //private List<QueueBufferBase<BufferItem<StockDealOrderPushBack>>> _stockDealCallbackQueueList;

        ///// <summary>
        ///// 股指期货成回回调工作容器
        ///// </summary>
        //private List<QueueBufferBase<BufferItem<FutureDealOrderPushBack>>> _stockDealIndexFuturesFutureCallbackQueueList;

        /// <summary>
        /// 分发现货实体
        /// </summary>
        public QueueBufferBase<ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>> DispachXHEntity =
            new QueueBufferBase<ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>>();

        #endregion

        #region == 构造器 ==

        /// <summary>
        /// 构造器
        /// </summary>
        private CounterOrderService()
        {
        }

        #endregion

        #region 回推成交回报信息

        #region 调用回推方法实现回推，回推失败进行故障恢复

        /// <summary>股指期货成交回报
        ///  股指期货成交回报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        private void GZQHDealBuffer_QueueItemProcessEvent(object sender, BufferItem<FutureDealOrderPushBack> item)
        {
            #region 没有必要检查记录数据库状态 ,如果要检查这里放在数据插入时候再做日志记录或者记录到文件中再做故障恢复

            //QH_PushBackOrderTableDal qh_PushBackOrderTableDal = new QH_PushBackOrderTableDal();
            //if (qh_PushBackOrderTableDal.JudgeConnectionState() == false)
            //    return;

            #endregion

            #region 如果对象或者通道 为空直接返回不实现回推

            if (item == null || item.Item == null)
            {
                return;
            }
            //如果通道为空就走以下看是否为分红数据不然以下走之前的类型
            if (string.IsNullOrEmpty(item.ChannelId))
            {
                //如果回推的数据有成交记录看看是不是分红成交
                if (item.Item.FutureDealList != null && item.Item.FutureDealList.Count > 0)
                {
                    #region 如果是分红对所有当前注册的通道都回推一次
                    //如果是分红对所有当前注册的通道都回推一次
                    if (item.Item.FutureDealList[0].TradeTypeId == (int)Types.DealRptType.DRTMargin
                        || item.Item.FutureDealList[0].TradeTypeId == (int)Types.DealRptType.DRTTradeDated
                        || item.Item.FutureDealList[0].TradeTypeId == (int)Types.DealRptType.DRTViolateLimit)
                    {
                        try
                        {
                            List<string> channeIDList = new List<string>();
                            foreach (var key in this._operationContexts.Keys)
                            {
                                channeIDList.Add(key);
                            }
                            //对所有的通道都回推相关数据
                            foreach (var chKey in channeIDList)
                            {
                                var newItem = new BufferItem<FutureDealOrderPushBack>(item.Item, chKey);
                                if (!this.CallbackStockIndexFuturesDealRpt(newItem))
                                {
                                    SaveQHBack(newItem.Item.FutureDealList, chKey);
                                }
                            }
                            //回推完直接返回
                            return;
                        }
                        catch (Exception ex)
                        {
                            //这里有可能会线程冲突
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    #endregion

                    #region 如果不为分空不处理直接返回
                    else
                    {
                        //不为分红的直接返回
                        return;
                    }
                    #endregion
                }
                else
                {
                    return;
                }
            }
            #endregion

            #region 如果存在回推通道证明是要回推的记录 如果回推不成功记录保存到数据库中下做下次回推

            if (!this.CallbackStockIndexFuturesDealRpt(item))
            {
                SaveQHBack(item.Item.FutureDealList, item.ChannelId);

                #region oldcode

                //try
                //{
                //    DeleteQHBack(item.Item.FutureDealList);
                //}
                //catch
                //{
                //    DeleteQHBack(item.Item.FutureDealList);
                //}

                #endregion
            }

            #endregion
        }

        /// <summary>商品期货成交回报
        /// 商品期货成交回报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SPQHDealItem_QueueItemProcessEvent(object sender, BufferItem<FutureDealOrderPushBack> item)
        {


            #region 如果对象或者通道 为空直接返回不实现回推

            if (item == null || item.Item == null)
            {
                return;
            }
            //如果通道为空就走以下看是否为分红数据不然以下走之前的类型
            if (string.IsNullOrEmpty(item.ChannelId))
            {
                //如果回推的数据有成交记录看看是不是分红成交
                if (item.Item.FutureDealList != null && item.Item.FutureDealList.Count > 0)
                {
                    #region 如果是分红对所有当前注册的通道都回推一次
                    //如果是分红对所有当前注册的通道都回推一次
                    if (item.Item.FutureDealList[0].TradeTypeId == (int)Types.DealRptType.DRTMargin
                        || item.Item.FutureDealList[0].TradeTypeId == (int)Types.DealRptType.DRTTradeDated
                        || item.Item.FutureDealList[0].TradeTypeId == (int)Types.DealRptType.DRTViolateLimit)
                    {
                        try
                        {
                            List<string> channeIDList = new List<string>();
                            foreach (var key in this._operationContexts.Keys)
                            {
                                channeIDList.Add(key);
                            }
                            //对所有的通道都回推相关数据
                            foreach (var chKey in channeIDList)
                            {
                                var newItem = new BufferItem<FutureDealOrderPushBack>(item.Item, chKey);
                                if (!this.CallbackStockIndexFuturesDealRpt(newItem))
                                {
                                    SaveQHBack(newItem.Item.FutureDealList, chKey);
                                }
                            }
                            //回推完直接返回
                            return;
                        }
                        catch (Exception ex)
                        {
                            //这里有可能会线程冲突
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    #endregion

                    #region 如果不为分空不处理直接返回
                    else
                    {
                        //不为分红的直接返回
                        return;
                    }
                    #endregion
                }
                else
                {
                    return;
                }
            }
            #endregion
            #region 如果存在回推通道证明是要回推的记录 如果回推不成功记录保存到数据库中下做下次回推

            if (!this.CallbackMercantileDealRpt(item))
            {
                SaveQHBack(item.Item.FutureDealList, item.ChannelId);
            }

            #endregion
        }

        /// <summary>股票成交回报
        /// 股票成交回报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        private void sDealItem_QueueItemProcessEvent(object sender, BufferItem<StockDealOrderPushBack> item)
        {
            #region 没有必要检查记录数据库状态 ,如果要检查这里放在数据插入时候再做日志记录或者记录到文件中再做故障恢复

            //if (xh_PushBackOrderTableDal.JudgeConnectionState() == false)
            //    return;

            #endregion

            #region 如果对象或者通道为空直接返回不实现回推。也不记录数据
            if (item == null || item.Item == null)
            {
                return;
            }

            //如果通道为空就走以下看是否为分红数据不然以下走之前的类型
            if (string.IsNullOrEmpty(item.ChannelId))
            {
                //如果回推的数据有成交记录看看是不是分红成交
                if (item.Item.StockDealList != null && item.Item.StockDealList.Count > 0)
                {
                    #region 如果是分红对所有当前注册的通道都回推一次
                    //如果是分红对所有当前注册的通道都回推一次
                    if (item.Item.StockDealList[0].TradeTypeId == (int)Types.DealRptType.DRTTransfer)
                    {
                        try
                        {
                            List<string> channeIDList = new List<string>();
                            foreach (var key in this._operationContexts.Keys)
                            {
                                channeIDList.Add(key);
                            }
                            //对所有的通道都回推相关数据
                            foreach (var chKey in channeIDList)
                            {
                                var newItem = new BufferItem<StockDealOrderPushBack>(item.Item, chKey);
                                if (!this.CallbackStockDealRpt(newItem))
                                {
                                    SaveXHBack(newItem.Item.StockDealList, chKey);
                                }
                            }
                            //回推完直接返回
                            return;
                        }
                        catch (Exception ex)
                        {
                            //这里有可能会线程冲突
                            LogHelper.WriteError(ex.Message, ex);
                        }
                    }
                    #endregion

                    #region 如果不为分空不处理直接返回
                    else
                    {
                        //不为分红的直接返回
                        return;
                    }
                    #endregion
                }
                else
                {
                    return;
                }
            }

            #endregion

            #region 如果存在回推通道证明是要回推的记录 如果回推不成功记录保存到数据库中下做下次回推

            if (!this.CallbackStockDealRpt(item))
            {
                SaveXHBack(item.Item.StockDealList, item.ChannelId.Trim());

                #region oldcode

                //try
                //{
                //    //一次删除
                //    DeleteXHBack(item.Item.StockDealList);
                //}
                //catch
                //{
                //    //删除不成功
                //    DeleteXHBack(item.Item.StockDealList);
                //}

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 港股成交回报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        private void HKDealItem_QueueItemProcessEvent(object sender, BufferItem<HKDealOrderPushBack> item)
        {

            #region 如果对象或者通道为空直接返回不实现回推。也不记录数据

            if (item == null || string.IsNullOrEmpty(item.ChannelId) || item.Item == null)
            {
                return;
            }
            #endregion

            #region 如果存在回推通道证明是要回推的记录 如果回推不成功记录保存到数据库中下做下次回推

            if (!this.CallbackHKDealRpt(item))
            {
                SaveHKBack(item.Item.HKDealList, item.ChannelId.Trim());
            }

            #endregion
        }

        /// <summary>
        /// 港股改单回报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        private void HKModifyOrderItem_QueueItemProcessEvent(object sender, BufferItem<HKModifyOrderPushBack> item)
        {

            #region 如果对象或者通道为空直接返回不实现回推。也不记录数据

            if (item == null || string.IsNullOrEmpty(item.ChannelId) || item.Item == null)
            {
                return;
            }
            #endregion

            #region 如果存在回推通道证明是要回推的记录 如果回推不成功记录保存到数据库中下做下次回推

            if (!this.CallbackHKModifyOrderRpt(item))//推不成功时保存到故障恢复中
            {
                SaveHKModifyOrderBack(item.Item.ID, item.ChannelId.Trim());
            }
            else
            {
                DeleteHKModifyBack(item.Item.ID);
            }
            #endregion
        }

        #endregion

        #region 故障恢复

        /// <summary>现货故障恢复
        /// 现货故障恢复
        /// </summary>
        /// <param name="channelID">通道ID</param>
        private void PushXHRegain(string channelID)
        {
            try
            {
                //注册时把文件中的数据导入到数据库中
                FailureRecoveryFactory.XHReaderToDB();
                //注册通道调用用故障恢复即开始检查数据
                //首先把不是当日要回推的数据清除
                XH_PushBackOrderTableDal dal = new XH_PushBackOrderTableDal();
                if (dal.DeleteNotPushToday(channelID) > 0) //如果还有要回推的数据执行数据查询等相关操作再回推
                {
                    #region 然后根据通道获取所要加推的成交数据所对应的不重复今日委托数据

                    //然后根据通道获取所要加推的成交数据所对应的不重复今日委托数据
                    //历史数据在这里已经自动转换成今日委托
                    //List<XH_TodayEntrustTableInfo> entrustList = dal.GetEntrustListByChannleID(channelID);
                    //这里为了减少数据连合查询带来的压力所以分两步完成
                    string entrusNo = dal.GetDistinctEntrustNumberByChannelID(channelID);
                    List<XH_TodayEntrustTableInfo> entrustList = dal.GetEntrustListByEnturstNo(entrusNo);

                    #endregion

                    #region 然后再根据通道返回所要回推的今日成交数据

                    //然后再根据通道返回所要回推的今日成交数据
                    List<XH_TodayTradeTableInfo> tradeList = new List<XH_TodayTradeTableInfo>();
                    tradeList = dal.GetTodayTradeListByChannleID(channelID);
                    ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> pushOrderEntry;

                    #endregion

                    #region 开始组装数据回推

                    //开始组装数据回推
                    AccountManager am = AccountManager.Instance;
                    foreach (var item in entrustList)
                    {
                        #region 分委托单号分次回推数据

                        pushOrderEntry = new ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>();
                        pushOrderEntry.EntrustTable = item;
                        var pushTradeList = new List<XH_TodayTradeTableInfo>();

                        #endregion

                        #region 查询同通道下的同一委托单号作一次回推

                        foreach (var tradeItem in tradeList)
                        {
                            if (tradeItem.EntrustNumber.Trim() == item.EntrustNumber.Trim())
                            {
                                pushTradeList.Add(tradeItem);
                            }
                        }

                        #endregion

                        #region  这里通过委托单的资金账号查找对应的用户ID，的转换回推数据时不再对委托单外部的交易员ID再转换

                        UA_UserAccountAllocationTableInfo ua = am.GetUserByAccount(item.CapitalAccount);
                        if (ua != null)
                        {
                            pushOrderEntry.TradeID = ua.UserID;
                        }

                        #endregion

                        pushOrderEntry.TradeTableList = pushTradeList;
                        AcceptStockPushOrder(pushOrderEntry);
                    }

                    #endregion
                }
            }

            catch (Exception ex)
            {
                LogHelper.WriteError("现货故障恢复数据获取失败", ex);
            }

            #region old code

            //ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject;
            //List<XH_TodayTradeTableInfo> xh_TodayTradeTableInfos;
            //List<XH_TodayTradeTableInfo> xh_TodayTradeTableInfoEntity;
            //List<XH_TodayEntrustTableInfo> xh_TodayEntrustTableInfos;
            //XH_TodayTradeTableDal xh_TodayTradeTableDal = new XH_TodayTradeTableDal();
            //XH_TodayEntrustTableDal xh_TodayEntrustTableDal = new XH_TodayEntrustTableDal();
            //xh_TodayTradeTableInfos = xh_TodayTradeTableDal.GetPushList(channelID);
            //xh_TodayEntrustTableInfos = xh_TodayEntrustTableDal.GetPushList(channelID);
            //foreach (var entrustTableInfo in xh_TodayEntrustTableInfos)
            //{
            //    reckonEndObject = new ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo>();
            //    reckonEndObject.EntrustTable = entrustTableInfo;
            //    xh_TodayTradeTableInfoEntity = new List<XH_TodayTradeTableInfo>();
            //    foreach (var tradeInfo in xh_TodayTradeTableInfos)
            //    {
            //        if (entrustTableInfo.EntrustNumber == tradeInfo.EntrustNumber)
            //        {
            //            xh_TodayTradeTableInfoEntity.Add(tradeInfo);
            //        }
            //    }
            //    UA_UserAccountAllocationTableInfo obj = AccountManager.Instance.GetUserByAccount(entrustTableInfo.StockAccount);
            //    reckonEndObject.TradeID = obj.UserID;
            //    reckonEndObject.TradeTableList = xh_TodayTradeTableInfoEntity;
            //    AcceptStockPushOrder(reckonEndObject);
            //}

            #endregion
        }

        /// <summary>期货故障恢复
        /// 期货故障恢复
        /// </summary>
        /// <param name="channelID"></param>
        private void PushQHRegain(string channelID)
        {
            try
            {
                //注册时把文件中的数据导入到数据库中
                FailureRecoveryFactory.QHReaderToDB();
                //注册通道调用用故障恢复即开始检查数据
                //首先把不是当日要回推的数据清除
                QH_PushBackOrderTableDal dal = new QH_PushBackOrderTableDal();
                if (dal.DeleteNotPushToday(channelID) > 0) //如果还有要回推的数据执行数据查询等相关操作再回推
                {
                    #region 然后根据通道获取所要加推的成交数据所对应的不重复今日委托数据

                    //然后根据通道获取所要加推的成交数据所对应的不重复今日委托数据
                    //历史数据在这里已经自动转换成今日委托
                    //List<XH_TodayEntrustTableInfo> entrustList = dal.GetEntrustListByChannleID(channelID);
                    //这里为了减少数据连合查询带来的压力所以分两步完成
                    string entrusNo = dal.GetDistinctEntrustNumberByChannelID(channelID);
                    List<QH_TodayEntrustTableInfo> entrustList = dal.GetEntrustListByEnturstNo(entrusNo);

                    #endregion

                    #region 因为商品期货和股指期货都在同一表中，所以这里根据委托代码分解出来以后面回推使用
                    //股指期货列表
                    List<QH_TodayEntrustTableInfo> gzQHEntrustList = new List<QH_TodayEntrustTableInfo>();
                    //商品期货委托列表
                    List<QH_TodayEntrustTableInfo> spQHEntrustList = new List<QH_TodayEntrustTableInfo>();
                    foreach (var item in entrustList)
                    {
                        int? breedID = MCService.CommonPara.GetBreedClassTypeEnumByCommodityCode(item.ContractCode);

                        if (breedID.HasValue)
                        {
                            switch ((GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum)breedID.Value)
                            {
                                case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture:
                                    gzQHEntrustList.Add(item);
                                    break;
                                case GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture:
                                    spQHEntrustList.Add(item);
                                    break;
                            }
                        }
                    }

                    #endregion

                    #region 然后再根据通道返回所要回推的今日成交数据

                    //然后再根据通道返回所要回推的今日成交数据
                    List<QH_TodayTradeTableInfo> tradeList = new List<QH_TodayTradeTableInfo>();
                    tradeList = dal.GetTodayTradeListByChannleID(channelID);
                    ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> pushOrderEntry;

                    #endregion

                    //开始组装数据回推
                    AccountManager am = AccountManager.Instance;
                    #region 开始组装股指期货数据回推
                    foreach (var item in gzQHEntrustList)
                    {
                        #region 分委托单号分次回推数据

                        pushOrderEntry = new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                        pushOrderEntry.EntrustTable = item;
                        var pushTradeList = new List<QH_TodayTradeTableInfo>();

                        #endregion

                        #region 查询同通道下的同一委托单号作一次回推

                        foreach (var tradeItem in tradeList)
                        {
                            if (tradeItem.EntrustNumber.Trim() == item.EntrustNumber.Trim())
                            {
                                pushTradeList.Add(tradeItem);
                            }
                        }

                        #endregion

                        #region  这里通过委托单的资金账号查找对应的用户ID，的转换回推数据时不再对委托单外部的交易员ID再转换

                        UA_UserAccountAllocationTableInfo ua = am.GetUserByAccount(item.CapitalAccount);
                        if (ua != null)
                        {
                            pushOrderEntry.TradeID = ua.UserID;
                        }

                        #endregion

                        pushOrderEntry.TradeTableList = pushTradeList;
                        AcceptStockIndexFuturesPushOrder(pushOrderEntry);
                    }

                    #endregion

                    #region 开始组装商品期货数据回推
                    foreach (var item in spQHEntrustList)
                    {
                        #region 分委托单号分次回推数据

                        pushOrderEntry = new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
                        pushOrderEntry.EntrustTable = item;
                        var pushTradeList = new List<QH_TodayTradeTableInfo>();

                        #endregion

                        #region 查询同通道下的同一委托单号作一次回推

                        foreach (var tradeItem in tradeList)
                        {
                            if (tradeItem.EntrustNumber.Trim() == item.EntrustNumber.Trim())
                            {
                                pushTradeList.Add(tradeItem);
                            }
                        }

                        #endregion

                        #region  这里通过委托单的资金账号查找对应的用户ID，的转换回推数据时不再对委托单外部的交易员ID再转换

                        UA_UserAccountAllocationTableInfo ua = am.GetUserByAccount(item.CapitalAccount);
                        if (ua != null)
                        {
                            pushOrderEntry.TradeID = ua.UserID;
                        }

                        #endregion

                        pushOrderEntry.TradeTableList = pushTradeList;
                        AcceptMercantileFuturesOrderPushOrder(pushOrderEntry);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("期货故障恢复数据获取失败", ex);
            }

            #region old code

            //ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject;
            //List<QH_TodayTradeTableInfo> xh_TodayTradeTableInfos;
            //List<QH_TodayTradeTableInfo> xh_TodayTradeTableInfoEntity;
            //List<QH_TodayEntrustTableInfo> xh_TodayEntrustTableInfos;
            //QH_TodayTradeTableDal xh_TodayTradeTableDal = new QH_TodayTradeTableDal();
            //QH_TodayEntrustTableDal xh_TodayEntrustTableDal = new QH_TodayEntrustTableDal();
            //xh_TodayTradeTableInfos = xh_TodayTradeTableDal.GetPushList(channelID);
            //xh_TodayEntrustTableInfos = xh_TodayEntrustTableDal.GetPushList(channelID);
            //foreach (var entrustTableInfo in xh_TodayEntrustTableInfos)
            //{
            //    reckonEndObject = new ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo>();
            //    reckonEndObject.EntrustTable = entrustTableInfo;
            //    xh_TodayTradeTableInfoEntity = new List<QH_TodayTradeTableInfo>();
            //    foreach (var tradeInfo in xh_TodayTradeTableInfos)
            //    {
            //        if (entrustTableInfo.EntrustNumber == tradeInfo.EntrustNumber)
            //        {
            //            xh_TodayTradeTableInfoEntity.Add(tradeInfo);
            //        }
            //    }
            //    //AccountManager
            //    reckonEndObject.TradeTableList = xh_TodayTradeTableInfoEntity;

            //    UA_UserAccountAllocationTableInfo obj = AccountManager.Instance.GetUserByAccount(entrustTableInfo.TradeAccount);
            //    reckonEndObject.TradeID = obj.UserID;
            //    AcceptStockIndexFuturesPushOrder(reckonEndObject);

            //}

            #endregion
        }


        /// <summary>港股故障恢复
        /// 港股故障恢复
        /// </summary>
        /// <param name="channelID">通道ID</param>
        private void PushHKRegain(string channelID)
        {
            try
            {
                //注册时把文件中的数据导入到数据库中
                FailureRecoveryFactory.HKReaderToDB();
                //注册通道调用用故障恢复即开始检查数据
                //首先把不是当日要回推的数据清除
                HK_PushBackOrderDal dal = new HK_PushBackOrderDal();
                if (dal.DeleteNotPushToday(channelID) > 0) //如果还有要回推的数据执行数据查询等相关操作再回推
                {
                    #region 然后根据通道获取所要加推的成交数据所对应的不重复今日委托数据

                    //然后根据通道获取所要加推的成交数据所对应的不重复今日委托数据
                    //历史数据在这里已经自动转换成今日委托
                    //List<XH_TodayEntrustTableInfo> entrustList = dal.GetEntrustListByChannleID(channelID);
                    //这里为了减少数据连合查询带来的压力所以分两步完成
                    string entrusNo = dal.GetDistinctEntrustNumberByChannelID(channelID);
                    List<HK_TodayEntrustInfo> entrustList = dal.GetEntrustListByEnturstNo(entrusNo);

                    #endregion

                    #region 然后再根据通道返回所要回推的今日成交数据

                    //然后再根据通道返回所要回推的今日成交数据
                    List<HK_TodayTradeInfo> tradeList = new List<HK_TodayTradeInfo>();
                    tradeList = dal.GetTodayTradeListByChannleID(channelID);
                    ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> pushOrderEntry;

                    #endregion

                    #region 开始组装数据回推

                    //开始组装数据回推
                    AccountManager am = AccountManager.Instance;
                    foreach (var item in entrustList)
                    {
                        #region 分委托单号分次回推数据

                        pushOrderEntry = new ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo>();
                        pushOrderEntry.EntrustTable = item;
                        List<HK_TodayTradeInfo> pushTradeList = new List<HK_TodayTradeInfo>();

                        #endregion

                        #region 查询同通道下的同一委托单号作一次回推

                        foreach (var tradeItem in tradeList)
                        {
                            if (tradeItem.EntrustNumber.Trim() == item.EntrustNumber.Trim())
                            {
                                pushTradeList.Add(tradeItem);
                            }
                        }

                        #endregion

                        #region  这里通过委托单的资金账号查找对应的用户ID，的转换回推数据时不再对委托单外部的交易员ID再转换

                        UA_UserAccountAllocationTableInfo ua = am.GetUserByAccount(item.CapitalAccount);
                        if (ua != null)
                        {
                            pushOrderEntry.TradeID = ua.UserID;
                        }

                        #endregion

                        pushOrderEntry.TradeTableList = pushTradeList;
                        AcceptHKPushOrder(pushOrderEntry);
                    }

                    #endregion
                }
            }

            catch (Exception ex)
            {
                LogHelper.WriteError("港股故障恢复数据获取失败", ex);
            }

        }

        /// <summary>港股改单数据故障恢复
        /// 港股改单数据故障恢复
        /// </summary>
        /// <param name="channelID">通道ID</param>
        private void PushHKModifyRegain(string channelID)
        {
            try
            {
                //注册时把文件中的数据导入到数据库中
                FailureRecoveryFactory.HKModifyReaderToDB();
                //注册通道调用用故障恢复即开始检查数据
                //首先把不是当日要回推的数据清除
                HK_ModifyPushBackOrderDal dal = new HK_ModifyPushBackOrderDal();
                if (dal.DeleteNotPushToday(channelID) > 0) //如果还有要回推的数据执行数据查询等相关操作再回推
                {
                    //获取所有要回推的当前通道的改单数据
                    List<HKModifyOrderPushBack> modifyPushList = dal.GetListPushDataByChannleID(channelID);
                    #region 开始组装数据回推

                    //开始组装数据回推

                    foreach (var item in modifyPushList)
                    {
                        ////然后再回推
                        AcceptHKModifyPushOrder(item);
                    }
                    #endregion
                }
            }

            catch (Exception ex)
            {
                LogHelper.WriteError("港股故障恢复数据获取失败", ex);
            }

        }

        /// <summary>故障恢复注册委托事件
        /// 故障恢复注册委托事件
        /// </summary>
        /// <param name="channelID"></param>
        private void PushRegain(string channelID)
        {
            if (string.IsNullOrEmpty(channelID))
            {
                return;
            }

            try
            {
                PushChannelDelegate pushXH = PushXHRegain;
                PushChannelDelegate pushQH = PushQHRegain;
                PushChannelDelegate pushHK = PushHKRegain;
                PushChannelDelegate pushHKModify = PushHKModifyRegain;
                pushXH.BeginInvoke(channelID, null, null);
                pushQH.BeginInvoke(channelID, null, null);
                pushHK.BeginInvoke(channelID, null, null);
                pushHKModify.BeginInvoke(channelID, null, null);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("PushRegain", ex);
                return;
            }
        }

        /// <summary>
        /// 接收现货故障恢复
        /// </summary>
        /// <param name="reckonEndObject"></param>
        private void AcceptStockPushOrder(ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject)
        {
            var item = new BufferItem<StockDealOrderPushBack>(GetXHObject(reckonEndObject),
                                                              reckonEndObject.EntrustTable.CallbackChannlId);
            if (item == null)
            {
                return;
            }
            try
            {
                //这里故障恢复回推时先把数据从数据库删除，如果再回推不正确则后面自动再保存。
                DeleteXHBack(item.Item.StockDealList);
                smartPool.QueueWorkItem(sDealItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("回推数据失败", ex);
            }
        }
        /// <summary>
        /// 港股故障恢复回报事件
        /// </summary>
        /// <param name="reckonEndObject"></param>
        private void AcceptHKPushOrder(ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject)
        {
            var item = new BufferItem<HKDealOrderPushBack>(GetHKObject(reckonEndObject), reckonEndObject.EntrustTable.CallbackChannlID);
            if (item == null)
            {
                return;
            }
            try
            {
                //这里故障恢复回推时先把数据从数据库删除，如果再回推不正确则后面自动再保存。
                DeleteHKBack(item.Item.HKDealList);
                smartPool.QueueWorkItem(HKDealItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("港股故障恢复回推数据失败", ex);
            }
        }
        /// <summary>
        /// 接收股指期货故障恢复成交回报
        /// </summary>
        /// <param name="drsip"></param>
        private void AcceptStockIndexFuturesPushOrder(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> drsip)
        {
            var item = new BufferItem<FutureDealOrderPushBack>(GetQHObject(drsip), drsip.EntrustTable.CallbackChannelId);
            if (item == null)
            {
                return;
            }
            try
            {
                //这里故障恢复回推时先把数据从数据库删除，如果再回推不正确则后面自动再保存。
                DeleteQHBack(item.Item.FutureDealList);
                smartPool.QueueWorkItem(GZQHDealBuffer_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("AcceptStockIndexFuturesDealOrder", ex);
            }
        }

        /// <summary>
        /// 接收商品期货故障恢复成交回报
        /// </summary>
        /// <param name="drsip"></param>
        private void AcceptMercantileFuturesOrderPushOrder(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> drsip)
        {
            var item = new BufferItem<FutureDealOrderPushBack>(GetQHObject(drsip), drsip.EntrustTable.CallbackChannelId);
            if (item == null)
            {
                return;
            }
            try
            {
                //这里故障恢复回推时先把数据从数据库删除，如果再回推不正确则后面自动再保存。
                DeleteQHBack(item.Item.FutureDealList);
                smartPool.QueueWorkItem(SPQHDealItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("AcceptMercantileFuturesOrderDealOrder", ex);
            }
        }

        /// <summary>
        /// 港股改单故障恢复回报事件
        /// </summary>
        /// <param name="reckonEndObject"></param>
        private void AcceptHKModifyPushOrder(HKModifyOrderPushBack mopb)
        {
            var item = new BufferItem<HKModifyOrderPushBack>(mopb, mopb.CallbackChannlId);
            if (item == null)
            {
                return;
            }
            try
            {
                //这里故障恢复回推时先把数据从数据库删除，如果再回推不正确则后面自动再保存。
                DeleteHKModifyBack(mopb.ID);
                smartPool.QueueWorkItem(HKModifyOrderItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("港股故障恢复回推数据失败", ex);
            }
        }

        /// <summary>保存还没有实现回推的现货数据
        /// 保存还没有实现回推的现货数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="channelID"></param>
        private void SaveXHBack(List<StockPushDealEntity> list, string channelID)
        {
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            LogHelper.WriteError("保存现货成交回推失败数据", new Exception(list[0].TradeNumber));
            try
            {
                XH_PushBackOrderTableDal dal = new XH_PushBackOrderTableDal();
                //这里不开启事务，插入一条成功算一条。
                Database db = DatabaseFactory.CreateDatabase();
                foreach (var item in list)
                {
                    try
                    {
                        dal.Add(item.TradeNumber, channelID, db);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message, ex);
                        //保存不到数据库直接保存到文件中
                        FailureRecoveryFactory.XHWriter(item.TradeNumber, channelID);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("保存数据失败", ex);
            }

            #region old code

            //ReckoningTransaction tranManger = new ReckoningTransaction();
            //Database db = DatabaseFactory.CreateDatabase();
            //XH_PushBackOrderTableDal dal = new XH_PushBackOrderTableDal();
            //try
            //{
            //    using (DbConnection conn = db.CreateConnection())
            //    {
            //        conn.Open();
            //        tranManger.Database = db;
            //        DbTransaction tran = conn.BeginTransaction();
            //        tranManger.Transaction = tran;
            //        foreach (var item in list)
            //        {
            //            dal.Add(item.TradeNumber, channelID, tranManger.Database, tranManger.Transaction);
            //        }
            //        tranManger.Transaction.Commit();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    tranManger.Transaction.Rollback();
            //    LogHelper.WriteError("保存数据失败", ex);
            //}

            #endregion
        }

        /// <summary>保存还没有实现回推的期货数据
        /// 保存还没有实现回推的期货数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="channelID"></param>
        private void SaveQHBack(List<FuturePushDealEntity> list, string channelID)
        {
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            LogHelper.WriteError("保存商品期货成交回推失败数据", new Exception(list[0].TradeNumber));

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                QH_PushBackOrderTableDal dal = new QH_PushBackOrderTableDal();
                foreach (var item in list)
                {
                    try
                    {
                        dal.Add(item.TradeNumber, channelID, db);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message, ex);
                        //保存不到数据库直接保存到文件中
                        FailureRecoveryFactory.QHWriter(item.TradeNumber, channelID);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("保存数据失败", ex);
            }

            #region old code

            //ReckoningTransaction tranManger = new ReckoningTransaction();
            //Database db = DatabaseFactory.CreateDatabase();
            //QH_PushBackOrderTableDal dal = new QH_PushBackOrderTableDal();
            //try
            //{
            //    using (DbConnection conn = db.CreateConnection())
            //    {
            //        conn.Open();
            //        tranManger.Database = db;
            //        DbTransaction transaction = conn.BeginTransaction();
            //        tranManger.Transaction = transaction;
            //        foreach (var item in list)
            //        {
            //            dal.Add(item.TradeNumber, channelID, tranManger.Database, tranManger.Transaction);
            //        }
            //        tranManger.Transaction.Commit();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    tranManger.Transaction.Rollback();
            //    LogHelper.WriteError("保存数据失败", ex);
            //}

            #endregion
        }

        /// <summary>保存还没有实现回推的港股数据
        /// 保存还没有实现回推的港股数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="channelID"></param>
        private void SaveHKBack(List<HKPushDealEntity> list, string channelID)
        {
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            LogHelper.WriteError("保存港股成交回推失败数据", new Exception(list[0].TradeNumber));
            try
            {
                HK_PushBackOrderDal dal = new HK_PushBackOrderDal();
                //这里不开启事务，插入一条成功算一条。
                Database db = DatabaseFactory.CreateDatabase();
                foreach (var item in list)
                {
                    try
                    {
                        dal.Add(item.TradeNumber, channelID, db);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message, ex);
                        //保存不到数据库直接保存到文件中
                        FailureRecoveryFactory.HKWriter(item.TradeNumber, channelID);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("保存港股成交回推数据失败", ex);
            }

        }

        /// <summary>保存还没有实现回推的港股改单数据
        /// 保存还没有实现回推的港股改单数据
        /// </summary>
        /// <param name="id">回推记录ID</param>
        /// <param name="channelID"></param>
        private void SaveHKModifyOrderBack(string id, string channelID)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(channelID))
            {
                return;
            }
            LogHelper.WriteError("保存港股改单回推失败数据", new Exception(id));

            HK_ModifyPushBackOrderDal dal = new HK_ModifyPushBackOrderDal();
            try
            {
                dal.Add(id, channelID);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                //保存不到数据库直接保存到文件中
                FailureRecoveryFactory.HKModifyWriter(id, channelID);
            }
        }

        /// <summary>先保存要回推的港股改单数据
        /// 先保存要回推的港股改单数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="channelID"></param>
        /// <returns>返回保存的主键ID</returns>
        private string SaveHKModifyOrderPushBack(HKModifyOrderPushBack model)
        {
            string id = Guid.NewGuid().ToString();
            if (model == null)
            {
                return "";
            }
            try
            {
                if (string.IsNullOrEmpty(model.ID))
                {
                    model.ID = id;
                }
                else
                {
                    id = model.ID;
                }
                HK_ModifyOrderPushBackDal dal = new HK_ModifyOrderPushBackDal();
                dal.Add(model);
            }
            catch (Exception ex)
            {
                string str = "通道ID：" + model.CallbackChannlId;
                str += "是否成功" + model.IsSuccess;
                str += "信息" + model.Message;
                str += "新委托的单号" + model.NewRequestNumber;
                str += "原始委托的单号" + model.OriginalRequestNumber;
                str += "交易员" + model.TradeID;
                LogHelper.WriteError("保存港股改单回推数据失败" + str, ex);
                return "";
            }
            return id;
        }

        /// <summary>删除已经回推成功能或者没有必要回推的现货数据 
        /// 删除已经回推成功能或者没有必要回推的现货数据 
        /// </summary>
        /// <param name="list"></param>
        private void DeleteXHBack(List<StockPushDealEntity> list)
        {
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            try
            {
                XH_PushBackOrderTableDal dal = new XH_PushBackOrderTableDal();
                Database db = DatabaseFactory.CreateDatabase();
                foreach (var item in list)
                {
                    dal.Delete(item.TradeNumber, db);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("DeleteXHBack", ex);
            }

            #region old code

            //ReckoningTransaction tranManger = new ReckoningTransaction();
            //Database db = DatabaseFactory.CreateDatabase();
            //XH_PushBackOrderTableDal dal = new XH_PushBackOrderTableDal();
            //try
            //{
            //    using (DbConnection connection = db.CreateConnection())
            //    {
            //        connection.Open();
            //        tranManger.Database = db;
            //        DbTransaction tran = connection.BeginTransaction();
            //        tranManger.Transaction = tran;
            //        foreach (var item in list)
            //        {
            //            dal.Delete(item.TradeNumber, tranManger.Database, tranManger.Transaction);
            //        }
            //        tranManger.Transaction.Commit();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    tranManger.Transaction.Rollback();
            //    LogHelper.WriteError("DeleteXHBack", ex);
            //}

            #endregion
        }

        /// <summary>删除已经回推成功能或者没有必要回推的期货数据
        /// 删除已经回推成功能或者没有必要回推的期货数据 
        /// </summary>
        /// <param name="list"></param>
        private void DeleteQHBack(List<FuturePushDealEntity> list)
        {
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                QH_PushBackOrderTableDal dal = new QH_PushBackOrderTableDal();
                foreach (var item in list)
                {
                    dal.Delete(item.TradeNumber, db);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("DeleteXHBack", ex);
            }

            #region old code

            //ReckoningTransaction tranManger = new ReckoningTransaction();
            //Database db = DatabaseFactory.CreateDatabase();
            //QH_PushBackOrderTableDal dal = new QH_PushBackOrderTableDal();
            //try
            //{
            //    using (DbConnection conn = db.CreateConnection())
            //    {
            //        conn.Open();
            //        tranManger.Database = db;
            //        DbTransaction tran = conn.BeginTransaction();
            //        tranManger.Transaction = tran;
            //        foreach (var item in list)
            //        {
            //            dal.Delete(item.TradeNumber, tranManger.Database, tranManger.Transaction);
            //        }

            //        tranManger.Transaction.Commit();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    tranManger.Transaction.Rollback();
            //    LogHelper.WriteError("DeleteXHBack", ex);
            //}

            #endregion
        }

        /// <summary>删除已经回推成功能或者没有必要回推的港股数据 
        /// 删除已经回推成功能或者没有必要回推的港股数据 
        /// </summary>
        /// <param name="list"></param>
        private void DeleteHKBack(List<HKPushDealEntity> list)
        {
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            try
            {
                HK_PushBackOrderDal dal = new HK_PushBackOrderDal();
                Database db = DatabaseFactory.CreateDatabase();
                foreach (var item in list)
                {
                    dal.Delete(item.TradeNumber, db);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("DeleteHKBack", ex);
            }
        }

        /// <summary>删除已经回推成功能或者没有必要回推的港股改单数据 
        /// 删除已经回推成功能或者没有必要回推的港股改单数据 
        /// </summary>
        /// <param name="id">改单记录ID</param>
        private void DeleteHKModifyBack(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            try
            {
                HK_ModifyPushBackOrderDal dal = new HK_ModifyPushBackOrderDal();
                dal.Delete(id);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("DeleteHKModifyBack", ex);
            }
        }

        #endregion

        #region 获取调用当前操作的客户端实例的通道【实现回推方法】

        /// <summary>
        /// 获取调用当前操作的客户端实例的通道实现【回推股票成交回报信息】
        /// </summary>
        /// <param name="item"></param>
        internal bool CallbackStockDealRpt(BufferItem<StockDealOrderPushBack> item)
        {
            if (item != null)
            {
                try
                {
                    string strChannelId = item.ChannelId;
                    if (string.IsNullOrEmpty(strChannelId))
                        return false;

                    StockDealOrderPushBack drsip = item.Item;
                    if (drsip != null)
                    {
                        if (this._operationContexts.ContainsKey(strChannelId))
                        {
                            var oc = this._operationContexts[strChannelId];
                            if (oc != null)
                            {
                                var callback = oc.GetCallbackChannel<IDoOrderCallback>();

                                if (callback != null)
                                {
                                    try
                                    {
                                        callback.ProcessStockDealRpt(drsip);
                                        return true;
                                        //DeleteXHBack(drsip.StockDealList);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.WriteError(ex.Message, ex);

                                        lock (((ICollection)this._operationContexts).SyncRoot)
                                        {
                                            this._operationContexts.Remove(strChannelId);
                                        }
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("回推现货成交回报信息异常==" + item.Item.StockOrderEntity.EntrustNumber, ex);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取调用当前操作的客户端实例的通道实现【回推商品期货成交回报信息】
        /// </summary>
        /// <param name="item"></param>
        internal bool CallbackMercantileDealRpt(BufferItem<FutureDealOrderPushBack> item)
        {
            if (item != null)
            {
                try
                {
                    string strChannelId = item.ChannelId;
                    if (string.IsNullOrEmpty(strChannelId))
                    {
                        return false;
                    }

                    FutureDealOrderPushBack drsip = item.Item;
                    if (drsip != null)
                    {
                        if (this._operationContexts.ContainsKey(strChannelId))
                        {
                            var oc = this._operationContexts[strChannelId];
                            if (oc != null)
                            {
                                var callback = oc.GetCallbackChannel<IDoOrderCallback>();

                                if (callback != null)
                                {
                                    try
                                    {
                                        callback.ProcessMercantileDealRpt(drsip);
                                        return true;
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.WriteError(ex.Message, ex);
                                        lock (((ICollection)this._operationContexts).SyncRoot)
                                        {
                                            this._operationContexts.Remove(strChannelId);
                                        }
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("回推商品期货成交回报信息异常==" + item.Item.StockIndexFuturesOrde.EntrustNumber, ex);
                    return false;
                }

            }
            return false;
        }

        /// <summary>
        /// 获取调用当前操作的客户端实例的通道实现【回推股指期货成交回报信息】
        /// </summary>
        /// <param name="item"></param>
        internal bool CallbackStockIndexFuturesDealRpt(BufferItem<FutureDealOrderPushBack> item)
        {
            if (item != null)
            {
                try
                {
                    string strChannelId = item.ChannelId;
                    if (string.IsNullOrEmpty(strChannelId))
                        return false;

                    FutureDealOrderPushBack drsip = item.Item;
                    if (drsip != null)
                    {
                        if (this._operationContexts.ContainsKey(strChannelId))
                        {
                            var oc = this._operationContexts[strChannelId];
                            if (oc != null)
                            {
                                var callback = oc.GetCallbackChannel<IDoOrderCallback>();

                                if (callback != null)
                                {
                                    try
                                    {
                                        callback.ProcessStockIndexFuturesDealRpt(drsip);
                                        return true;
                                        //DeleteQHBack(drsip.FutureDealList);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.WriteError(ex.Message, ex);
                                        lock (((ICollection)this._operationContexts).SyncRoot)
                                        {
                                            this._operationContexts.Remove(strChannelId);
                                        }
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("回推股指期货成交回报信息异常==" + item.Item.StockIndexFuturesOrde.EntrustNumber, ex);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取调用当前操作的客户端实例的通道实现【回推港股成交回报信息】
        /// </summary>
        /// <param name="item"></param>
        internal bool CallbackHKDealRpt(BufferItem<HKDealOrderPushBack> item)
        {

            if (item != null)
            {
                try
                {
                    string strChannelId = item.ChannelId;
                    if (string.IsNullOrEmpty(strChannelId))
                        return false;

                    HKDealOrderPushBack drsip = item.Item;
                    if (drsip != null)
                    {
                        if (this._operationContexts.ContainsKey(strChannelId))
                        {
                            var oc = this._operationContexts[strChannelId];
                            if (oc != null)
                            {
                                var callback = oc.GetCallbackChannel<IDoOrderCallback>();

                                if (callback != null)
                                {
                                    try
                                    {
                                        callback.ProcessHKDealRpt(drsip);
                                        return true;
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.WriteError(ex.Message, ex);
                                        lock (((ICollection)this._operationContexts).SyncRoot)
                                        {
                                            this._operationContexts.Remove(strChannelId);
                                        }
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("回推港股成交回报信息异常==" + item.Item.HKOrderEntity.EntrustNumber, ex);
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取调用当前操作的客户端实例的通道实现【回推港股改单回报信息】
        /// </summary>
        /// <param name="item"></param>
        internal bool CallbackHKModifyOrderRpt(BufferItem<HKModifyOrderPushBack> item)
        {
            if (item != null)
            {
                try
                {
                    string strChannelId = item.ChannelId;
                    if (string.IsNullOrEmpty(strChannelId))
                        return false;

                    HKModifyOrderPushBack drsip = item.Item;
                    if (drsip != null)
                    {
                        if (this._operationContexts.ContainsKey(strChannelId))
                        {
                            var oc = this._operationContexts[strChannelId];
                            if (oc != null)
                            {
                                var callback = oc.GetCallbackChannel<IDoOrderCallback>();

                                if (callback != null)
                                {
                                    try
                                    {
                                        callback.ProcessHKModifyOrderRpt(drsip);
                                        return true;
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.WriteError(ex.Message, ex);
                                        lock (((ICollection)this._operationContexts).SyncRoot)
                                        {
                                            this._operationContexts.Remove(strChannelId);
                                        }
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("回推港股改单成交回报信息异常==" + item.Item.OriginalRequestNumber, ex);
                    return false;
                }
            }
            return false;
        }
        #endregion

        #endregion

        #region 接收成交回报

        /// <summary>
        /// 接收现货成交回报
        /// </summary>
        /// <param name="reckonEndObject"></param>
        public void AcceptStockDealOrder(ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject)
        {
            var item = new BufferItem<StockDealOrderPushBack>(GetXHObject(reckonEndObject),
                                                              reckonEndObject.EntrustTable.CallbackChannlId);
            if (item == null)
            {
                return;
            }
            try
            {
                //这里开始加推回推成功后删除，所以这里不先保存，在回推时如果回推不成功再保存。
                //SaveXHBack(item.Item.StockDealList, item.ChannelId);
                smartPool.QueueWorkItem(sDealItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("回推数据失败", ex);
            }
        }

        /// <summary>
        /// 接收港股成交回报
        /// </summary>
        /// <param name="reckonEndObject"></param>
        public void AcceptHKDealOrder(ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> reckonEndObject)
        {
            var item = new BufferItem<HKDealOrderPushBack>(GetHKObject(reckonEndObject), reckonEndObject.EntrustTable.CallbackChannlID);
            if (item == null)
            {
                return;
            }
            try
            {
                smartPool.QueueWorkItem(HKDealItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("回推数据失败", ex);
            }
        }

        /// <summary>
        /// 接收港股改单回报
        /// </summary>
        /// <param name="mopb"></param>
        public void AcceptHKModifyOrder(HKModifyOrderPushBack mopb)
        {
            //接收到回推数据先保存到数据库中
            string id = SaveHKModifyOrderPushBack(mopb);
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            mopb.ID = id;

            //然后再回推
            var item = new BufferItem<HKModifyOrderPushBack>(mopb, mopb.CallbackChannlId);
            if (item == null)
            {
                return;
            }
            try
            {
                smartPool.QueueWorkItem(HKModifyOrderItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("回推数据失败", ex);
            }
        }

        /// <summary>
        /// 接收股指期货成交回报
        /// </summary>
        /// <param name="drsip"></param>
        public void AcceptStockIndexFuturesDealOrder(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> drsip)
        {
            var item = new BufferItem<FutureDealOrderPushBack>(GetQHObject(drsip), drsip.EntrustTable.CallbackChannelId);
            if (item == null)
            {
                return;
            }
            try
            {
                //SaveQHBack(item.Item.FutureDealList, item.ChannelId);
                smartPool.QueueWorkItem(GZQHDealBuffer_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("AcceptStockIndexFuturesDealOrder", ex);
            }
        }

        /// <summary>
        /// 接收商品期货成交回报
        /// </summary>
        /// <param name="drsip"></param>
        public void AcceptMercantileFuturesOrderDealOrder(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> drsip)
        {
            var item = new BufferItem<FutureDealOrderPushBack>(GetQHObject(drsip), drsip.EntrustTable.CallbackChannelId);
            if (item == null)
            {
                return;
            }
            try
            {
                smartPool.QueueWorkItem(SPQHDealItem_QueueItemProcessEvent, this, item);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("AcceptMercantileFuturesOrderDealOrder", ex);
            }
        }

        #region 回推数据组装转换

        /// <summary>返回期货回推对象
        /// 返回期货回推对象
        /// </summary>
        /// <param name="reckonEndObject"></param>
        /// <returns></returns>
        private FutureDealOrderPushBack GetQHObject(ReckonEndObject<QH_TodayEntrustTableInfo, QH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject == null)
            {
                return null;
            }
            //回推实体base
            FutureDealOrderPushBack pushBackEntry = new FutureDealOrderPushBack();
            //回推中包含的实体
            FuturePushOrderEntity pushEntrustEntity = new FuturePushOrderEntity();
            List<FuturePushDealEntity> pushTradeList = new List<FuturePushDealEntity>();
            //要转换的实体
            QH_TodayEntrustTableInfo entrustInfoList = reckonEndObject.EntrustTable;
            if (entrustInfoList == null)
            {
                return null;
            }
            IList<QH_TodayTradeTableInfo> tradeTableInfos = reckonEndObject.TradeTableList;
            //账号管理定义 
            AccountManager am = AccountManager.Instance;
            pushEntrustEntity.CancelAmount = entrustInfoList.CancelAmount;
            pushEntrustEntity.CancelLogo = entrustInfoList.CancelLogo;
            pushEntrustEntity.BuySellTypeId = entrustInfoList.BuySellTypeId;
            pushEntrustEntity.OpenCloseTypeId = entrustInfoList.OpenCloseTypeId;
            pushEntrustEntity.CloseFloatProfitLoss = entrustInfoList.CloseFloatProfitLoss;
            pushEntrustEntity.CloseMarketProfitLoss = entrustInfoList.CloseMarketProfitLoss;
            pushEntrustEntity.ContractCode = entrustInfoList.ContractCode;
            pushEntrustEntity.EntrustAmount = entrustInfoList.EntrustAmount;
            pushEntrustEntity.EntrustNumber = entrustInfoList.EntrustNumber;
            pushEntrustEntity.EntrustPrice = entrustInfoList.EntrustPrice;
            pushEntrustEntity.EntrustTime = entrustInfoList.EntrustTime;
            pushEntrustEntity.OrderStatusId = entrustInfoList.OrderStatusId;
            pushEntrustEntity.OrderMessage = entrustInfoList.OrderMessage;
            pushEntrustEntity.TradeAmount = entrustInfoList.TradeAmount;
            pushEntrustEntity.TradeAveragePrice = entrustInfoList.TradeAveragePrice;
            pushEntrustEntity.CurrencyTypeId = entrustInfoList.CurrencyTypeId;
            pushEntrustEntity.CapitalAccount = entrustInfoList.CapitalAccount;
            foreach (var item in tradeTableInfos)
            {
                FuturePushDealEntity model = new FuturePushDealEntity();
                model.Margin = item.Margin;
                model.TradeAmount = item.TradeAmount;
                model.TradeNumber = item.TradeNumber;
                model.TradePrice = item.TradePrice;
                model.TradeProceduresFee = item.TradeProceduresFee;
                model.TradeTime = item.TradeTime;
                model.TradeTypeId = item.TradeTypeId;
                model.MarketProfitLoss = item.MarketProfitLoss;
                pushTradeList.Add(model);
            }

            pushBackEntry.StockIndexFuturesOrde = pushEntrustEntity;
            pushBackEntry.FutureDealList = pushTradeList;

            #region  这里通过委托单的资金账号查找对应的用户ID

            if (string.IsNullOrEmpty(reckonEndObject.TradeID))
            {
                UA_UserAccountAllocationTableInfo ua = am.GetUserByAccount(entrustInfoList.CapitalAccount.Trim());
                if (ua != null)
                {
                    pushBackEntry.TradeID = ua.UserID;
                }
            }
            else
            {
                pushBackEntry.TradeID = reckonEndObject.TradeID; //委托交易员ID
            }

            #endregion

            return pushBackEntry;
        }

        /// <summary>返回现货柜台实体
        /// 返回柜台实体
        /// </summary>
        /// <param name="reckonEndObject"></param>
        /// <returns></returns>
        private StockDealOrderPushBack GetXHObject(ReckonEndObject<XH_TodayEntrustTableInfo, XH_TodayTradeTableInfo> reckonEndObject)
        {
            if (reckonEndObject == null)
            {
                return null;
            }
            StockDealOrderPushBack pushBackEntry = new StockDealOrderPushBack();

            StockPushOrderEntity orderEntity = new StockPushOrderEntity();
            List<StockPushDealEntity> stockDealList = new List<StockPushDealEntity>();

            XH_TodayEntrustTableInfo entrustInfoList = reckonEndObject.EntrustTable;
            if (entrustInfoList == null)
            {
                return null;
            }
            IList<XH_TodayTradeTableInfo> tradeInfoList = reckonEndObject.TradeTableList;

            AccountManager am = AccountManager.Instance;
            orderEntity.BuySellTypeId = entrustInfoList.BuySellTypeId;
            orderEntity.CancelAmount = entrustInfoList.CancelAmount;
            orderEntity.CancelLogo = entrustInfoList.CancelLogo;
            orderEntity.EntrustAmount = entrustInfoList.EntrustAmount;
            orderEntity.EntrustNumber = entrustInfoList.EntrustNumber;
            orderEntity.EntrustPrice = entrustInfoList.EntrustPrice;
            orderEntity.HasDoneProfit = entrustInfoList.HasDoneProfit;
            orderEntity.OfferTime = entrustInfoList.OfferTime;
            orderEntity.OrderMessage = entrustInfoList.OrderMessage;
            orderEntity.OrderStatusId = entrustInfoList.OrderStatusId;
            orderEntity.SpotCode = entrustInfoList.SpotCode;
            orderEntity.TradeAmount = entrustInfoList.TradeAmount;
            orderEntity.TradeAveragePrice = entrustInfoList.TradeAveragePrice;
            orderEntity.CurrencyTypeId = entrustInfoList.CurrencyTypeId;
            orderEntity.CapitalAccount = entrustInfoList.CapitalAccount;
            foreach (var item in tradeInfoList)
            {
                StockPushDealEntity entity = new StockPushDealEntity();
                entity.ClearingFee = item.ClearingFee;
                entity.Commission = item.Commission;
                entity.MonitoringFee = item.MonitoringFee;
                entity.TradeAmount = item.TradeAmount;
                entity.TradeNumber = item.TradeNumber;
                entity.StampTax = item.StampTax;
                entity.TradeProceduresFee = item.TradeProceduresFee;
                entity.TradeTime = item.TradeTime;
                entity.TradingSystemUseFee = item.TradingSystemUseFee;
                entity.TransferAccountFee = item.TransferAccountFee;
                entity.TradePrice = item.TradePrice;
                entity.TradeTypeId = item.TradeTypeId;
                stockDealList.Add(entity);
            }
            pushBackEntry.StockOrderEntity = orderEntity;
            pushBackEntry.StockDealList = stockDealList;

            #region  这里通过委托单的资金账号查找对应的用户ID

            if (string.IsNullOrEmpty(reckonEndObject.TradeID))
            {
                UA_UserAccountAllocationTableInfo ua = am.GetUserByAccount(entrustInfoList.CapitalAccount.Trim());
                if (ua != null)
                {
                    pushBackEntry.TradeID = ua.UserID;
                }
            }
            else
            {
                pushBackEntry.TradeID = reckonEndObject.TradeID; //委托交易员ID
            }

            #endregion

            return pushBackEntry;
        }

        /// <summary>返回现货柜台实体
        /// 返回柜台实体
        /// </summary>
        /// <param name="reckonEndObject"></param>
        /// <returns></returns>
        private HKDealOrderPushBack GetHKObject(ReckonEndObject<HK_TodayEntrustInfo, HK_TodayTradeInfo> obj)
        {
            if (obj == null)
            {
                return null;
            }
            HKDealOrderPushBack pushBackEntry = new HKDealOrderPushBack();

            HKPushOrderEntity orderEntity = new HKPushOrderEntity();
            List<HKPushDealEntity> stockDealList = new List<HKPushDealEntity>();

            HK_TodayEntrustInfo entrustInfoList = obj.EntrustTable;
            if (entrustInfoList == null)
            {
                return null;
            }
            IList<HK_TodayTradeInfo> tradeInfoList = obj.TradeTableList;

            AccountManager am = AccountManager.Instance;
            orderEntity.BuySellTypeId = entrustInfoList.BuySellTypeID;
            orderEntity.CancelAmount = entrustInfoList.CancelAmount;
            orderEntity.CancelLogo = entrustInfoList.CancelLogo;
            orderEntity.EntrustAmount = entrustInfoList.EntrustAmount;
            orderEntity.EntrustNumber = entrustInfoList.EntrustNumber;
            orderEntity.EntrustPrice = entrustInfoList.EntrustPrice;
            orderEntity.HasDoneProfit = entrustInfoList.HasDoneProfit;
            orderEntity.OfferTime = entrustInfoList.OfferTime;
            orderEntity.OrderMessage = entrustInfoList.OrderMessage;
            orderEntity.OrderStatusId = entrustInfoList.OrderStatusID;
            orderEntity.Code = entrustInfoList.Code;
            orderEntity.TradeAmount = entrustInfoList.TradeAmount;
            orderEntity.TradeAveragePrice = entrustInfoList.TradeAveragePrice;
            orderEntity.CurrencyTypeId = entrustInfoList.CurrencyTypeID;
            orderEntity.CapitalAccount = entrustInfoList.CapitalAccount;
            orderEntity.IsModifyOrder = entrustInfoList.IsModifyOrder;
            orderEntity.ModifyOrderNumber = entrustInfoList.ModifyOrderNumber;
            foreach (var item in tradeInfoList)
            {
                HKPushDealEntity entity = new HKPushDealEntity();
                entity.ClearingFee = item.ClearingFee;
                entity.Commission = item.Commission;
                entity.MonitoringFee = item.MonitoringFee;
                entity.TradeAmount = item.TradeAmount;
                entity.TradeNumber = item.TradeNumber;
                entity.StampTax = item.StampTax;
                entity.TradeProceduresFee = item.TradeProceduresFee;
                entity.TradeTime = item.TradeTime;
                entity.TradingSystemUseFee = item.TradingSystemUseFee;
                entity.TransferAccountFee = item.TransferAccountFee;
                entity.TradePrice = item.TradePrice;
                entity.TradeTypeId = item.TradeTypeId;
                stockDealList.Add(entity);
            }
            pushBackEntry.HKOrderEntity = orderEntity;
            pushBackEntry.HKDealList = stockDealList;

            #region  这里通过委托单的资金账号查找对应的用户ID

            if (string.IsNullOrEmpty(obj.TradeID))
            {
                UA_UserAccountAllocationTableInfo ua = am.GetUserByAccount(entrustInfoList.CapitalAccount.Trim());
                if (ua != null)
                {
                    pushBackEntry.TradeID = ua.UserID;
                }
            }
            else
            {
                pushBackEntry.TradeID = obj.TradeID; //委托交易员ID
            }

            #endregion

            return pushBackEntry;
        }
        #endregion

        #endregion

        #region 回推WCF公开方法 回推契约

        /// <summary>
        /// 注释回推通道
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool RegisterChannel(string clientId)
        {
            var result = false;

            lock (((ICollection)this._operationContexts).SyncRoot)
            {
                LogHelper.WriteDebug("WcfOrderService.RegisterChannel[ClientID=" + clientId + "]");
                OperationContext context = OperationContext.Current;
                this._operationContexts[clientId] = context;
                context.Channel.Faulted += Channel_Faulted;
                PushRegain(clientId);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 修改委托回推通道
        /// </summary>
        /// <param name="entrustNumberList"></param>
        /// <param name="newClientId"></param>
        /// <param name="numberType">编号类型1--现货，2-期货,3-港股</param>
        /// <returns></returns>
        public bool ChangeEntrustChannel(List<string> entrustNumberList, string newClientId, int numberType)
        {
            bool isSuccess = false;
            try
            {
                DoChangeEntrustChannel(entrustNumberList, newClientId, numberType);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("更新回推通道号异常：" + ex.Message + "新通道号：" + newClientId, ex);
            }

            return isSuccess;
        }

        public bool UnRegisterChannel(string clientId)
        {
            try
            {
                if (this._operationContexts.ContainsKey(clientId))
                {
                    LogHelper.WriteDebug("WcfOrderService.UnRegisterChannel:ClientID[" + clientId + "]");
                    OperationContext context = this._operationContexts[clientId];

                    lock (((ICollection)this._operationContexts).SyncRoot)
                        this._operationContexts.Remove(clientId);

                    try
                    {
                        context.Channel.DelayDoClose();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return true;
        }

        /// <summary>
        /// 检查通道
        /// </summary>
        /// <returns></returns>
        public string CheckChannel()
        {
            return DateTime.Now.ToString();
        }

        /// <summary>
        /// 【内部私有方法】异常处理方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channel_Faulted(object sender, EventArgs e)
        {
            LogHelper.WriteInfo("******************WCF通道发生异常WcfOrderService.Channel_Faulted******************");

            IContextChannel channel = sender as IContextChannel;
            if (channel == null)
                return;

            //意外检查
            channel.DoClose();
        }

        /// <summary>
        /// 根据委托单号和要更新的类型更新回推通道
        /// </summary>
        /// <param name="number">委托单列表</param>
        /// <param name="newClientId">新的通道号</param>
        /// <param name="numberType">编号类型1--现货，2-期货,3-港股</param>
        private void DoChangeEntrustChannel(List<string> number, string newClientId, int numberType)
        {
            if (string.IsNullOrEmpty(newClientId))
                return;
            switch (numberType)
            {
                case 1:
                    XH_PushBackOrderTableDal xh_dal = new XH_PushBackOrderTableDal();
                    xh_dal.UpdateEntrustPushBackChannelID(number, newClientId);
                    break;
                case 2:
                    QH_PushBackOrderTableDal qh_dal = new QH_PushBackOrderTableDal();
                    qh_dal.UpdateEntrustPushBackChannelID(number, newClientId);
                    break;
                case 3:
                    HK_PushBackOrderDal hk_dal = new HK_PushBackOrderDal();
                    hk_dal.UpdateEntrustPushBackChannelID(number, newClientId);
                    break;
            }
        }

        #endregion

        #region 之前下单相关方法

        //public OrderResponse DoStockOrder(StockOrderRequest stockorder)
        //{
        //    return _orderAccepterInstance.DoStockOrder(stockorder);
        //}

        //public OrderResponse DoMercantileFuturesOrder(MercantileFuturesOrderRequest futuresorder)
        //{
        //    return _orderAccepterInstance.DoMercantileFuturesOrder(futuresorder);
        //}

        //public OrderResponse DoStockIndexFuturesOrder(StockIndexFuturesOrderRequest futuresorder)
        //{
        //    return _orderAccepterInstance.DoStockIndexFuturesOrder(futuresorder);
        //}

        //public bool CancelStockOrder(string OrderId, ref string message, out Types.OrderStateType ost, out int errorType)
        //{
        //    return _orderAccepterInstance.CancelStockOrder(OrderId, ref message, out ost, out errorType);
        //}

        //public bool CancelMercantileFuturesOrder(string OrderId, ref string message, out Types.OrderStateType ost,
        //                                         out int errorType)
        //{
        //    ost = Types.OrderStateType.None;
        //    return _orderAccepterInstance.CancelMercantileFuturesOrder(OrderId, ref message, out errorType);
        //}

        //public bool CancelStockIndexFuturesOrder(string OrderId, ref string message, out Types.OrderStateType ost,
        //                                         out int errorType)
        //{
        //    ost = Types.OrderStateType.None;
        //    return _orderAccepterInstance.CancelStockIndexFuturesOrder(OrderId, ref message, out errorType);
        //}

        #endregion
    }
}