#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounter.DAL;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity;
using ReckoningCounter.Entity.DelegateOffer;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;
using System.Threading;

#endregion

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer
{
    /// <summary>
    /// 报盘唤醒事件参数,错误编码2111-2119
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    /// <typeparam name="ProcessItem"></typeparam>
    internal class WakeupEventArgs<ProcessItem> : EventArgs
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="_tem"></param>
        public WakeupEventArgs(ProcessItem _tem)
        {
            Item = _tem;
        }

        /// <summary>
        /// 处理元素
        /// </summary>
        public ProcessItem Item { get; private set; }
    }

    /// <summary>
    /// 柜台关健事件接收缓存
    /// </summary>
    internal class OrderCacheMessageAccpetItem
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="args"></param>
        /// <param name="commodity"></param>
        /// <param name="breedClassType"></param>
        public OrderCacheMessageAccpetItem(ScheduleEventArgs args, CM_Commodity commodity,
                                           Types.BreedClassTypeEnum breedClassType)
        {
            Args = args;
            Commodity = commodity;
            BreedClassType = breedClassType;
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public ScheduleEventArgs Args { get; private set; }

        /// <summary>
        /// 商品代码
        /// </summary>
        public CM_Commodity Commodity { get; private set; }

        /// <summary>
        /// 品种类型(现货,商品期货,股指期货)
        /// </summary>
        public Types.BreedClassTypeEnum BreedClassType { get; private set; }
    }

    /// <summary>
    /// 报盘委托缓存处理
    /// </summary>
    internal class OrderCache : MarketProcessJob
    {
        /// <summary>
        /// 股指期货容器
        /// </summary>
        private Dictionary<string, Dictionary<string, QhTodayEntrustTableEx>> _GZQHOrderContainer =
            new Dictionary<string, Dictionary<string, QhTodayEntrustTableEx>>();

        /// <summary>
        /// 商品期货容器
        /// </summary>
        private Dictionary<string, Dictionary<string, QhTodayEntrustTableEx>> _QHOrderContainer =
            new Dictionary<string, Dictionary<string, QhTodayEntrustTableEx>>();

        /// <summary>
        /// 现货容器
        /// </summary>
        private Dictionary<string, Dictionary<string, XhTodayEntrustTableEx>> _XHOrderContainer =
            new Dictionary<string, Dictionary<string, XhTodayEntrustTableEx>>();

        /// <summary>
        /// 港股容器
        /// </summary>
        private Dictionary<string, Dictionary<string, HkTodayEntrustEx>> _HKOrderContainer =
            new Dictionary<string, Dictionary<string, HkTodayEntrustEx>>();


        //private QueueBufferBase<OrderCacheMessageAccpetItem> ScheduleMessageBuffer;


        public OrderCache()
        {
            //ScheduleMessageBuffer = new QueueBufferBase<OrderCacheMessageAccpetItem>();
            //ScheduleMessageBuffer.QueueItemProcessEvent += ScheduleMessageBuffer_QueueItemProcessEvent;
        }

        /// <summary>
        /// 现货事件
        /// </summary>
        public event EventHandler<WakeupEventArgs<XhTodayEntrustTableEx>> XHOfferWakeupEvent = null;

        /// <summary>
        /// 港股事件
        /// </summary>
        public event EventHandler<WakeupEventArgs<HkTodayEntrustEx>> HKOfferWakeupEvent = null;

        /// <summary>
        /// 股指期货事件
        /// </summary>
        public event EventHandler<WakeupEventArgs<QhTodayEntrustTableEx>> GZQHOfferWakeupEvent = null;

        /// <summary>
        /// 商品期货事件
        /// </summary>
        public event EventHandler<WakeupEventArgs<QhTodayEntrustTableEx>> QHOfferWakeupEvent = null;


        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ScheduleMessageBuffer_QueueItemProcessEvent(object sender,
        //                                                         QueueItemHandleEventArg<OrderCacheMessageAccpetItem> e)
        //{
        //    //开市处理
        //    if (e.Item.Args.TimeType == Entity.Contants.Types.TradingTimeType.MacthBeginWork 
        //        //||e.Item.Args.TimeType == Types.TradingTimeType.Open
        //        )
        //    {
        //        string code = e.Item.Commodity.CommodityCode;

        //        //现货
        //        if (e.Item.BreedClassType == Types.BreedClassTypeEnum.Stock)
        //        {
        //            if (this._XHOrderContainer.ContainsKey(code))
        //            {
        //                if (IsTimeDone(code))
        //                {
        //                    var xHList = this._XHOrderContainer[code];
        //                    foreach (var value in xHList.Values)
        //                    {
        //                        SendXHValue(value);
        //                    }
        //                }
        //            }


        //            //商品期货
        //        }
        //        else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.CommodityFuture)
        //        {
        //            if (this._QHOrderContainer.ContainsKey(code))
        //            {
        //                if (IsTimeDone(code))
        //                {
        //                    var xHList = this._QHOrderContainer[code];
        //                    foreach (var value in xHList.Values)
        //                    {
        //                        SendSPQHValue(value);
        //                    }
        //                }
        //            }
        //        } //股指期货
        //        else if (e.Item.BreedClassType == Types.BreedClassTypeEnum.StockIndexFuture)
        //        {
        //            if (this._GZQHOrderContainer.ContainsKey(code))
        //            {
        //                if (IsTimeDone(code))
        //                {
        //                    var xHList = this._GZQHOrderContainer[code];
        //                    foreach (var value in xHList.Values)
        //                    {
        //                        SendGZQHValue(value);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //收市处理，清除委托缓存
        //    if (e.Item.Args.TimeType == Entity.Contants.Types.TradingTimeType.MatchEndWork)
        //    {
        //        //LogHelper.WriteDebug("OrderCache:收市处理，清除委托缓存");
        //        _XHOrderContainer.Clear();
        //        _QHOrderContainer.Clear();
        //        _GZQHOrderContainer.Clear();
        //    }
        //}

        #region Cache Order
        /// <summary>
        /// 缓存现货
        /// </summary>
        /// <param name="orderRequest"></param>
        public bool CacheStockOrder(XhTodayEntrustTableEx orderRequest)
        {
            bool result = false;
            if (orderRequest != null && orderRequest.OriginalEntity != null)
            {
                if (orderRequest.IsCacheOrder)
                    return true;

                try
                {
                    orderRequest.IsCacheOrder = true;
                    string strCode = orderRequest.OriginalEntity.SpotCode;
                    string strOrderId = orderRequest.OriginalEntity.EntrustNumber;
                    if (_XHOrderContainer.ContainsKey(strCode))
                    {
                        var levelOneItem = _XHOrderContainer[strCode];
                        if (levelOneItem == null)
                            levelOneItem = new Dictionary<string, XhTodayEntrustTableEx>();

                        if (levelOneItem.ContainsKey(strOrderId))
                        {
                            LogHelper.WriteDebug("OrderCache.CacheStockOrder:已经缓存委托单" + strOrderId);
                        }
                        else
                        {
                            levelOneItem.Add(strOrderId, orderRequest);
                        }
                    }
                    else
                    {
                        var levelOneItem = new Dictionary<string, XhTodayEntrustTableEx>();
                        levelOneItem.Add(strOrderId, orderRequest);
                        _XHOrderContainer.Add(strCode, levelOneItem);
                    }

                    result = true;
                }
                catch (Exception ex)
                {
                    string strMessage = "GT-2114:[现货报盘]缓存未开市委托异常。";
                    orderRequest.OriginalEntity.OrderMessage = strMessage;
                    OrderOfferDataLogic.UpdateStockOrderMessage(orderRequest.OriginalEntity);
                    LogHelper.WriteError(strMessage, ex);
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 删除缓存的港股委托
        /// </summary>
        /// <param name="code"></param>
        /// <param name="entrustNumber"></param>
        /// <returns></returns>
        public bool DeleteHKOrder(string code, string entrustNumber)
        {
            string strCode = code;
            string strOrderId = entrustNumber;

            if (_HKOrderContainer.ContainsKey(strCode))
            {
                var levelOneItem = _HKOrderContainer[strCode];
                if (levelOneItem != null)
                {
                    if (levelOneItem.ContainsKey(strOrderId))
                    {
                        LogHelper.WriteDebug("OrderCache.DeleteHKOrder:已经删除缓存的港股委托单" + strOrderId);
                        levelOneItem.Remove(strOrderId);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 删除缓存的现货委托
        /// </summary>
        /// <param name="code"></param>
        /// <param name="entrustNumber"></param>
        /// <returns></returns>
        public bool DeleteXHOrder(string code, string entrustNumber)
        {
            string strCode = code;
            string strOrderId = entrustNumber;

            if (_XHOrderContainer.ContainsKey(strCode))
            {
                var levelOneItem = _XHOrderContainer[strCode];
                if (levelOneItem != null)
                {
                    if (levelOneItem.ContainsKey(strOrderId))
                    {
                        LogHelper.WriteDebug("OrderCache.DeleteXHOrder:已经删除缓存的现货委托单" + strOrderId);
                        levelOneItem.Remove(strOrderId);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 删除缓存的股指期货委托
        /// </summary>
        /// <param name="code"></param>
        /// <param name="entrustNumber"></param>
        /// <returns></returns>
        public bool DeleteGZQHOrder(string code, string entrustNumber)
        {
            string strCode = code;
            string strOrderId = entrustNumber;

            if (_GZQHOrderContainer.ContainsKey(strCode))
            {
                var levelOneItem = _GZQHOrderContainer[strCode];
                if (levelOneItem != null)
                {
                    if (levelOneItem.ContainsKey(strOrderId))
                    {
                        LogHelper.WriteDebug("OrderCache.DeleteXHOrder:已经删除缓存的股指期货委托单" + strOrderId);
                        levelOneItem.Remove(strOrderId);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 删除缓存的期货委托
        /// </summary>
        /// <param name="code"></param>
        /// <param name="entrustNumber"></param>
        /// <returns></returns>
        public bool DeleteQHOrder(string code, string entrustNumber)
        {
            string strCode = code;
            string strOrderId = entrustNumber;

            if (_QHOrderContainer.ContainsKey(strCode))
            {
                var levelOneItem = _QHOrderContainer[strCode];
                if (levelOneItem != null)
                {
                    if (levelOneItem.ContainsKey(strOrderId))
                    {
                        LogHelper.WriteDebug("OrderCache.DeleteXHOrder:已经删除缓存的期货委托单" + strOrderId);
                        levelOneItem.Remove(strOrderId);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 缓存港股
        /// </summary>
        /// <param name="orderRequest"></param>
        public bool CacheHKOrder(HkTodayEntrustEx orderRequest)
        {
            bool result = false;
            if (orderRequest != null && orderRequest.OriginalEntity != null)
            {
                if (orderRequest.IsCacheOrder)
                    return true;

                try
                {
                    orderRequest.IsCacheOrder = true;
                    string strCode = orderRequest.OriginalEntity.Code;
                    string strOrderId = orderRequest.OriginalEntity.EntrustNumber;
                    if (_HKOrderContainer.ContainsKey(strCode))
                    {
                        var levelOneItem = _HKOrderContainer[strCode];
                        if (levelOneItem == null)
                            levelOneItem = new Dictionary<string, HkTodayEntrustEx>();

                        if (levelOneItem.ContainsKey(strOrderId))
                        {
                            LogHelper.WriteDebug("OrderCache.CacheHKOrder:已经缓存委托单" + strOrderId);
                        }
                        else
                        {
                            levelOneItem.Add(strOrderId, orderRequest);
                        }
                    }
                    else
                    {
                        var levelOneItem = new Dictionary<string, HkTodayEntrustEx>();
                        levelOneItem.Add(strOrderId, orderRequest);
                        _HKOrderContainer.Add(strCode, levelOneItem);
                    }

                    result = true;
                }
                catch (Exception ex)
                {
                    string strMessage = "GT-2114:[港股报盘]缓存未开市委托异常。";
                    orderRequest.OriginalEntity.OrderMessage = strMessage;
                    OrderOfferDataLogic.UpdateHKOrderMessage(orderRequest.OriginalEntity);
                    LogHelper.WriteError(strMessage, ex);
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 缓存商品期货
        /// </summary>
        /// <param name="futuresorder"></param>
        public void CacheMercantileFuturesOrder(QhTodayEntrustTableEx futuresorder)
        {
            if (futuresorder != null && futuresorder.OriginalEntity != null)
            {
                if (futuresorder.IsCacheOrder)
                    return;

                try
                {
                    futuresorder.IsCacheOrder = true;
                    string strCode = futuresorder.OriginalEntity.ContractCode;
                    string strOrderId = futuresorder.OriginalEntity.EntrustNumber;
                    if (_QHOrderContainer.ContainsKey(strCode))
                    {
                        var levelOneItem = _QHOrderContainer[strCode];
                        if (levelOneItem == null)
                            levelOneItem = new Dictionary<string, QhTodayEntrustTableEx>();

                        if (levelOneItem.ContainsKey(strOrderId))
                        {
                            LogHelper.WriteDebug("OrderCache.CacheMercantileFuturesOrder:已经缓存委托单" + strOrderId);
                        }
                        else
                        {
                            levelOneItem.Add(strOrderId, futuresorder);
                        }
                        //levelOneItem.Add(strOrderId, futuresorder);
                    }
                    else
                    {
                        var levelOneItem = new Dictionary<string, QhTodayEntrustTableEx>();
                        levelOneItem.Add(strOrderId, futuresorder);
                        _QHOrderContainer.Add(strCode, levelOneItem);
                    }
                }
                catch (Exception ex)
                {
                    string strMessage = "GT-2115:[商品期货报盘]缓存未开市委托异常" + ex.Message;
                    futuresorder.OriginalEntity.OrderMessage = strMessage;
                    OrderOfferDataLogic.UpdateFutureOrder(futuresorder.OriginalEntity);
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 缓存股指期货
        /// </summary>
        /// <param name="futuresorder"></param>
        public void CacheStockIndexFuturesOrder(QhTodayEntrustTableEx futuresorder)
        {
            if (futuresorder != null && futuresorder.OriginalEntity != null)
            {
                if (futuresorder.IsCacheOrder)
                    return;

                try
                {
                    futuresorder.IsCacheOrder = true;
                    string strCode = futuresorder.OriginalEntity.ContractCode;
                    string strOrderId = futuresorder.OriginalEntity.EntrustNumber;
                    if (_GZQHOrderContainer.ContainsKey(strCode))
                    {
                        var levelOneItem = _GZQHOrderContainer[strCode];
                        if (levelOneItem == null)
                            levelOneItem = new Dictionary<string, QhTodayEntrustTableEx>();

                        if (levelOneItem.ContainsKey(strOrderId))
                        {
                            LogHelper.WriteDebug("OrderCache.CacheStockIndexFuturesOrder:已经缓存委托单" + strOrderId);
                        }
                        else
                        {
                            levelOneItem.Add(strOrderId, futuresorder);
                        }

                        //levelOneItem.Add(strOrderId, futuresorder);
                    }
                    else
                    {
                        var levelOneItem = new Dictionary<string, QhTodayEntrustTableEx>();
                        levelOneItem.Add(strOrderId, futuresorder);
                        _GZQHOrderContainer.Add(strCode, levelOneItem);
                    }
                }
                catch (Exception ex)
                {
                    string strMessage = "GT-2116:[股指期货报盘]缓存未开市委托异常" + ex.Message;
                    futuresorder.OriginalEntity.OrderMessage = strMessage;
                    OrderOfferDataLogic.UpdateFutureOrder(futuresorder.OriginalEntity);
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }
        #endregion

        #region 定时触发
        protected override void GlobalProcess(ScheduleEventArgs args)
        {
            //因为开盘时间提前二分钟，所以这里做开盘时调用执行预委托发送时，要推迟二分钟，这里为了保证时间上的
            //送单，所以阴塞线程二分10秒
            Thread thread = new Thread(delegate() { ProcessMatchBeginTime(args); });
            thread.Start();
            //thread.Join(121000);
            //ProcessMatchBeginTime(args);
        }

        private void ProcessMatchBeginTime(ScheduleEventArgs args)
        {
           
            //开市处理
            if (args.TimeType == Entity.Contants.Types.TradingTimeType.MacthBeginWork
                //||e.Item.Args.TimeType == Types.TradingTimeType.Open
                )
            {
                ////因为开盘时间提前二分钟，所以这里做开盘时调用执行预委托发送时，要推迟二分钟，这里为了保证时间上的
                //送单，所以阴塞线程二分10秒
                Thread.CurrentThread.Join(121000);
                //====
                SendXHCache();
                SendHKCache();
                SendSPQHCache();
                SendGZQHCache();
            }

            //收市处理，清除委托缓存
            if (args.TimeType == Entity.Contants.Types.TradingTimeType.MatchEndWork)
            {
                //LogHelper.WriteDebug("OrderCache:收市处理，清除委托缓存");
                _XHOrderContainer.Clear();
                _QHOrderContainer.Clear();
                _GZQHOrderContainer.Clear();
            }
        }


        protected override void DetailProcess(ScheduleEventArgs args, CM_Commodity commodity,
                                              Types.BreedClassTypeEnum breedClassTypeEnum)
        {
            //ScheduleMessageBuffer.InsertQueueItem(new OrderCacheMessageAccpetItem(args, commodity, breedClassTypeEnum));
        }
        #endregion

        #region 报盘SendValue to OrderOffer

        private void SendGZQHValue(QhTodayEntrustTableEx value)
        {
            if (value.HasSendCacheOrder)
                return;

            //if (!IsTimeDone(value.OriginalEntity.ContractCode))
            //    return;

            try
            {
                value.HasSendCacheOrder = true;
                this.GZQHOfferWakeupEvent(this, new WakeupEventArgs<QhTodayEntrustTableEx>(value));
            }
            catch (Exception ex)
            {
                string strMessage = "GT-2113:[股指期货报盘]缓存未开市委托唤醒异常" + ex.Message;
                value.OriginalEntity.OrderMessage = strMessage;
                OrderOfferDataLogic.UpdateFutureOrder(value.OriginalEntity);
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void SendSPQHValue(QhTodayEntrustTableEx value)
        {
            if (value.HasSendCacheOrder)
                return;

            //if (!IsTimeDone(value.OriginalEntity.ContractCode))
            //    return;

            try
            {
                this.QHOfferWakeupEvent(this, new WakeupEventArgs<QhTodayEntrustTableEx>(value));
                value.HasSendCacheOrder = true;
            }
            catch (Exception ex)
            {
                string strMessage = "GT-2112:[商品期货报盘]缓存未开市委托唤醒异常" + ex.Message;
                value.OriginalEntity.OrderMessage = strMessage;
                OrderOfferDataLogic.UpdateFutureOrder(value.OriginalEntity);
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void SendXHValue(XhTodayEntrustTableEx value)
        {
            if (value.HasSendCacheOrder)
                return;

            //if(!IsTimeDone(value.OriginalEntity.SpotCode))
            //    return;

            try
            {
                value.HasSendCacheOrder = true;
                this.XHOfferWakeupEvent(this, new WakeupEventArgs<XhTodayEntrustTableEx>(value));
            }
            catch (Exception ex)
            {
                string strMessage = "GT-2111:[现货报盘]缓存未开市委托唤醒异常";
                value.OriginalEntity.OrderMessage = strMessage;
                OrderOfferDataLogic.UpdateStockOrderMessage(value.OriginalEntity);
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        private void SendHKValue(HkTodayEntrustEx value)
        {
            if (value.HasSendCacheOrder)
                return;

            //if(!IsTimeDone(value.OriginalEntity.SpotCode))
            //    return;

            try
            {
                value.HasSendCacheOrder = true;
                this.HKOfferWakeupEvent(this, new WakeupEventArgs<HkTodayEntrustEx>(value));
            }
            catch (Exception ex)
            {
                string strMessage = "GT-2111:[现货报盘]缓存未开市委托唤醒异常";
                value.OriginalEntity.OrderMessage = strMessage;
                OrderOfferDataLogic.UpdateHKOrderMessage(value.OriginalEntity);
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        /// <summary>
        /// Update Date:2009-10-26
        /// Update By:李健华
        /// Desc.:为了与港股应用多加代码所属于类别是标识
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private DateTime GetBourseBeginTime(string code, Types.BreedClassTypeEnum type)
        {
            DateTime now = DateTime.Now;

            var bourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(code, type);
            if (bourseType.ReceivingConsignStartTime.HasValue)
            {
                DateTime begin = bourseType.ReceivingConsignStartTime.Value;
                return new DateTime(now.Year, now.Month, now.Day, begin.Hour, begin.Minute, begin.Second);
            }

            return now;
        }
        /// <summary>
        /// Update Date:2009-10-26
        /// Update By:李健华
        /// Desc.:为了与港股应用调用重载方法
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool IsTimeDone(string code)
        {
            #region old code
            //DateTime start = GetBourseBeginTime(code);

            //DateTime now = DateTime.Now;

            //if (now >= start)
            //{
            //    return true;
            //}

            //return false;
            #endregion
            return IsTimeDone(code, Types.BreedClassTypeEnum.Stock);
        }
        /// <summary>
        /// Create Date:2009-10-26
        /// Create By:李健华
        /// Desc.:为了与港股应用重载方法
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsTimeDone(string code, Types.BreedClassTypeEnum type)
        {
            DateTime start = GetBourseBeginTime(code, type);

            DateTime now = DateTime.Now;

            if (now >= start)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region 加载当日未报的委托从数据库
        //当启动时，从数据库中加载今日未报的委托
        private void LoadOrderFromDB()
        {
            LoadXHOrderFromDB();
            LoadHKOrderFromDB();
            //LoadSPQHOrderFromDB();
            LoadGZQHOrderFromDB();
        }

        private void LoadSPQHOrderFromDB()
        {
            //TODO:LoadSPQHOrderFromDB
        }

        private void LoadGZQHOrderFromDB()
        {
            QH_TodayEntrustTableDal dal = new QH_TodayEntrustTableDal();
            string format = "OrderStatusId = '{0}'";
            string where = string.Format(format, (int)Entity.Contants.Types.OrderStateType.DOSUnRequired);
            List<QH_TodayEntrustTableInfo> list = null;
            try
            {
                list = dal.GetListArray(where);
                //list = DataRepository.QhTodayEntrustTableProvider.Find(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            foreach (QH_TodayEntrustTableInfo table in list)
            {


                //不再判断是否是当天
                var order = new QhTodayEntrustTableEx(table);
                CacheStockIndexFuturesOrder(order);
            }
        }

        private void LoadXHOrderFromDB()
        {
            string format = "OrderStatusId = '{0}'";
            string where = string.Format(format, (int)Entity.Contants.Types.OrderStateType.DOSUnRequired);
            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            List<XH_TodayEntrustTableInfo> list = null;
            try
            {
                //list = DataRepository.XhTodayEntrustTableProvider.Find(where);
                list = dal.GetListArray(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            DateTime now = DateTime.Now;
            foreach (XH_TodayEntrustTableInfo table in list)
            {


                //DateTime entrustTime = table.EntrustTime.Value;
                //if (entrustTime.Year == now.Year && entrustTime.Month == now.Month && entrustTime.Day == now.Day)
                //{
                //    CacheStockOrder(new XhTodayEntrustTableEx(table)); 
                //}

                //不再判断是否是当天
                var order = new XhTodayEntrustTableEx(table);
                CacheStockOrder(order);
            }
        }

        private void LoadHKOrderFromDB()
        {
            string format = "OrderStatusId = '{0}'";
            string where = string.Format(format, (int)Entity.Contants.Types.OrderStateType.DOSUnRequired);
            HK_TodayEntrustDal dal = new HK_TodayEntrustDal();
            List<HK_TodayEntrustInfo> list = null;
            try
            {
                //list = DataRepository.XhTodayEntrustTableProvider.Find(where);
                list = dal.GetListArray(where);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (Utils.IsNullOrEmpty(list))
                return;

            DateTime now = DateTime.Now;
            foreach (HK_TodayEntrustInfo table in list)
            {


                //DateTime entrustTime = table.EntrustTime.Value;
                //if (entrustTime.Year == now.Year && entrustTime.Month == now.Month && entrustTime.Day == now.Day)
                //{
                //    CacheStockOrder(new XhTodayEntrustTableEx(table)); 
                //}

                //不再判断是否是当天
                var order = new HkTodayEntrustEx(table);
                CacheHKOrder(order);
            }
        }
        #endregion

        #region 发送缓存

        /// <summary>
        /// 当重新启动时，如果过了开市时间，那么立刻发送缓存
        /// </summary>
        public void SendCache()
        {
            LoadOrderFromDB();

            SendXHCache();
            SendHKCache();
            //TODO:SendSPQHCache
            SendSPQHCache();
            SendGZQHCache();
        }

        private void SendXHCache()
        {
            foreach (KeyValuePair<string, Dictionary<string, XhTodayEntrustTableEx>> pair in _XHOrderContainer)
            {
                var dict = pair.Value;
                if (IsTimeDone(pair.Key))
                {
                    foreach (KeyValuePair<string, XhTodayEntrustTableEx> keyValuePair in dict)
                    {
                        var value = keyValuePair.Value;
                        SendXHValue(value);
                    }
                }
            }
        }
        /// <summary>
        /// Update Date:2009-10-26
        /// Update By:李健华
        /// Desc.:修改调用相应的重载方法IsTimeDone
        /// </summary>
        private void SendHKCache()
        {
            foreach (KeyValuePair<string, Dictionary<string, HkTodayEntrustEx>> pair in _HKOrderContainer)
            {
                var dict = pair.Value;
                if (IsTimeDone(pair.Key, Types.BreedClassTypeEnum.HKStock))
                {
                    foreach (KeyValuePair<string, HkTodayEntrustEx> keyValuePair in dict)
                    {
                        var value = keyValuePair.Value;
                        SendHKValue(value);
                    }
                }
            }
        }

        private void SendSPQHCache()
        {
            foreach (KeyValuePair<string, Dictionary<string, QhTodayEntrustTableEx>> pair in _QHOrderContainer)
            {
                var dict = pair.Value;
                if (IsTimeDone(pair.Key))
                {
                    foreach (KeyValuePair<string, QhTodayEntrustTableEx> keyValuePair in dict)
                    {
                        var value = keyValuePair.Value;
                        SendSPQHValue(value);
                    }
                }
            }
        }

        private void SendGZQHCache()
        {
            foreach (KeyValuePair<string, Dictionary<string, QhTodayEntrustTableEx>> pair in _GZQHOrderContainer)
            {
                var dict = pair.Value;
                if (IsTimeDone(pair.Key))
                {
                    foreach (KeyValuePair<string, QhTodayEntrustTableEx> keyValuePair in dict)
                    {
                        var value = keyValuePair.Value;
                        SendGZQHValue(value);
                    }
                }
            }
        }

        #endregion
    }
}