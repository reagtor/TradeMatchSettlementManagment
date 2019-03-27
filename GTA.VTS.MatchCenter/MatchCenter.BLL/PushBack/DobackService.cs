using System;
using System.Collections.Generic;
using System.ServiceModel;
using Amib.Threading;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.PushBack;
using MatchCenter.BLL.interfaces;
using MatchCenter.BLL.Service;
using MatchCenter.DAL;
using MatchCenter.Entity;
using MatchCenter.BLL.Common;
using MatchCenter.Entity.HK;
using MatchCenter.DAL.HK;

namespace MatchCenter.BLL.PushBack
{
    /// <summary>
    /// Title:成交回报处理类
    /// Create BY：李健华
    /// Create Date：2008-05-14
    /// Title:添加港股相关操作方法定义和部分实现
    /// Update By：李健华
    /// Update Date:2009-10-18
    /// Desc:添加商品期货相关操作方法定义和部分实现
    /// Update By：董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public class DobackService
    {
        #region 定义
        /// <summary>
        /// 撮合中心现货撮合成交回报缓冲区
        /// </summary>
        public QueueBufferBase<StockDealEntity> stockDealOrderCacheList;

        /// <summary>
        /// 撮合中心股指期货撮合成交回报委托缓冲区
        /// </summary>
        public QueueBufferBase<FutureDealBackEntity> futureDealOrderCacheList;
        /// <summary>
        /// 撮合中心港股撮合成交回报缓冲区
        /// </summary>
        public QueueBufferBase<HKDealBackEntity> hkstockDealOrderCatheList;

        #region 撮合中心商品期货撮合成交回报委托缓冲区 add by 董鹏 2010-01-22
        /// <summary>
        /// 撮合中心商品期货撮合成交回报委托缓冲区
        /// </summary>
        public QueueBufferBase<CommoditiesDealBackEntity> commoditiesDealOrderCacheList;
        #endregion

        /// <summary>
        /// 撮合中心现货撤单回报缓冲区
        /// </summary>
        public QueueBufferBase<CancelOrderEntity> BufferXHCancelEntityList;

        /// <summary>
        /// 撮合中心港股撤单回报缓冲区
        /// </summary>
        public QueueBufferBase<CancelOrderEntity> BufferHKCancelEntityList;

        #region add by 董鹏 2010-04-30

        /// <summary>
        /// 撮合中心股指期货撤单回报缓冲区
        /// </summary>
        public QueueBufferBase<CancelOrderEntity> BufferSICancelEntityList;
        
        /// <summary>
        /// 撮合中心商品期货撤单回报缓冲区
        /// </summary>
        public QueueBufferBase<CancelOrderEntity> BufferCFCancelEntityList;

        #endregion

        /// <summary>
        /// 撮合中心改单委托缓冲区
        /// </summary>
        public QueueBufferBase<HKModifyBackEntity> BufferHKModifyEntityList;

        /// <summary>
        /// 线程池
        /// </summary>
        private SmartThreadPool smartPool = new SmartThreadPool { MaxThreads = 100, MinThreads = 25 };

        /// <summary>
        /// 现货报盘工作容器
        /// </summary>
        private List<QueueBufferBase<StockDealEntity>> StockDealList;

        /// <summary>
        /// 期货回报工作容器
        /// </summary>
        private List<QueueBufferBase<FutureDealBackEntity>> FutureDealList;

        /// <summary>
        /// 港股回报工作容器
        /// </summary>
        private List<QueueBufferBase<HKDealBackEntity>> HKDealList;

        #region 商品期货回报工作容器 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货回报工作容器
        /// </summary>
        private List<QueueBufferBase<CommoditiesDealBackEntity>> CommoditiesDealList;
        #endregion
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="backCount"></param>
        public DobackService(int backCount)
        {

            //撮合中心缓冲区处理
            BufferXHCancelEntityList = new QueueBufferBase<CancelOrderEntity>();
            BufferXHCancelEntityList.QueueItemProcessEvent += ProcessXHCancel;

            BufferHKCancelEntityList = new QueueBufferBase<CancelOrderEntity>();
            BufferHKCancelEntityList.QueueItemProcessEvent += ProcessHKCancel;

            #region add by 董鹏 2010-04-30

            BufferSICancelEntityList = new QueueBufferBase<CancelOrderEntity>();
            BufferSICancelEntityList.QueueItemProcessEvent += ProcessSICancel;

            BufferCFCancelEntityList = new QueueBufferBase<CancelOrderEntity>();
            BufferCFCancelEntityList.QueueItemProcessEvent += ProcessCFCancel;

            #endregion

            BufferHKModifyEntityList = new QueueBufferBase<HKModifyBackEntity>();
            BufferHKModifyEntityList.QueueItemProcessEvent += ProcessHKModify;


            stockDealOrderCacheList = new QueueBufferBase<StockDealEntity>();
            stockDealOrderCacheList.QueueItemProcessEvent += DispatchStockProcess;

            futureDealOrderCacheList = new QueueBufferBase<FutureDealBackEntity>();
            futureDealOrderCacheList.QueueItemProcessEvent += DispatchFutureProcess;

            hkstockDealOrderCatheList = new QueueBufferBase<HKDealBackEntity>();
            hkstockDealOrderCatheList.QueueItemProcessEvent += DispatchHKStockProcess;

            #region commoditiesDealOrderCacheList add by 董鹏 2010-01-22
            commoditiesDealOrderCacheList = new QueueBufferBase<CommoditiesDealBackEntity>();
            commoditiesDealOrderCacheList.QueueItemProcessEvent += DispatchCommoditiesProcess;
            #endregion

            smartPool.Start();

            StockDealList = new List<QueueBufferBase<StockDealEntity>>();
            FutureDealList = new List<QueueBufferBase<FutureDealBackEntity>>();
            HKDealList = new List<QueueBufferBase<HKDealBackEntity>>();
            #region CommoditiesDealList add by 董鹏 2010-01-22
            CommoditiesDealList = new List<QueueBufferBase<CommoditiesDealBackEntity>>();
            #endregion

            for (int i = 0; i < backCount; i++)
            {
                QueueBufferBase<StockDealEntity> stockEntity = new QueueBufferBase<StockDealEntity>();
                stockEntity.QueueItemProcessEvent += (sender, e) => smartPool.QueueWorkItem(ProcessBussiness, sender, e);
                StockDealList.Add(stockEntity);
                QueueBufferBase<FutureDealBackEntity> futureEntity = new QueueBufferBase<FutureDealBackEntity>();
                futureEntity.QueueItemProcessEvent += (sender, e) => smartPool.QueueWorkItem(ProcessFutureBussiness, sender, e);
                FutureDealList.Add(futureEntity);
                QueueBufferBase<HKDealBackEntity> hkstockEntity = new QueueBufferBase<HKDealBackEntity>();
                hkstockEntity.QueueItemProcessEvent += (sender, e) => smartPool.QueueWorkItem(ProcessHKBussiness, sender, e);
                HKDealList.Add(hkstockEntity);
                #region
                QueueBufferBase<CommoditiesDealBackEntity> commoditiesEntity = new QueueBufferBase<CommoditiesDealBackEntity>();
                commoditiesEntity.QueueItemProcessEvent += (sender, e) => smartPool.QueueWorkItem(ProcessCommoditiesBussiness, sender, e);
                CommoditiesDealList.Add(commoditiesEntity);
                #endregion
            }
        }

        /// <summary>
        /// 现货撤单接口
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessXHCancel(object sender, QueueItemHandleEventArg<CancelOrderEntity> e)
        {
            OperationContext context = null;
            //实体通道不能为空
            if (e.Item.ChannelNo != null)
            {
                context = MatchCenterManager.Instance.OperationContexts[e.Item.ChannelNo];
            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                return;
            }
            try
            {
                //获取撮合中心通道
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {                    
                    //撤单处理
                    callback.CancelStockOrderRpt(e.Item);
                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteError("CH-0001:wcf服务通道阻塞", ex);
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                TradePushBackImpl.Instanse.SaveXHCancelBack(e.Item);
                return;
            }
        }

        /// <summary>
        /// 港股撤单接口
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessHKCancel(object sender, QueueItemHandleEventArg<CancelOrderEntity> e)
        {
            OperationContext context = null;
            //实体通道不能为空
            if (e.Item.ChannelNo != null)
            {
                context = MatchCenterManager.Instance.OperationContexts[e.Item.ChannelNo];
            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                return;
            }
            try
            {
                //获取撮合中心通道
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    //撤单处理
                    callback.CancelHKStockOrderRpt(e.Item);
                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteError("CH-0001:wcf服务通道阻塞", ex);
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                TradePushBackImpl.Instanse.SaveHKCancelBack(e.Item);
                return;
            }
        }

        #region add by 董鹏 2010-04-30

        /// <summary>
        /// 股指期货撤单接口
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessSICancel(object sender, QueueItemHandleEventArg<CancelOrderEntity> e)
        {
            OperationContext context = null;
            //实体通道不能为空
            if (e.Item.ChannelNo != null)
            {
                context = MatchCenterManager.Instance.OperationContexts[e.Item.ChannelNo];
            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                return;
            }
            try
            {
                //获取撮合中心通道
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    //撤单处理
                    callback.CancelStockIndexFuturesOrderRpt(e.Item);
                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteError("CH-0001:wcf服务通道阻塞", ex);
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                TradePushBackImpl.Instanse.SaveSICancelBack(e.Item);
                return;
            }
        }

        /// <summary>
        /// 商品期货撤单接口
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessCFCancel(object sender, QueueItemHandleEventArg<CancelOrderEntity> e)
        {
            OperationContext context = null;
            //实体通道不能为空
            if (e.Item.ChannelNo != null)
            {
                context = MatchCenterManager.Instance.OperationContexts[e.Item.ChannelNo];
            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                return;
            }
            try
            {
                //获取撮合中心通道
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    //撤单处理
                    callback.CancelCommoditiesOrderRpt(e.Item);
                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteError("CH-0001:wcf服务通道阻塞", ex);
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                TradePushBackImpl.Instanse.SaveCFCancelBack(e.Item);
                return;
            }
        }
        #endregion

        /// <summary>
        /// 港股改单接口
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessHKModify(object sender, QueueItemHandleEventArg<HKModifyBackEntity> e)
        {
            OperationContext context = null;
            //实体通道不能为空
            if (e.Item.ChannelNo != null)
            {
                context = MatchCenterManager.Instance.OperationContexts[e.Item.ChannelNo];
            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                return;
            }
            try
            {
                //获取撮合中心通道
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    //撤单处理
                    callback.ModifyHKStockOrderRpt(e.Item);

                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteError("CH-0001:wcf服务通道阻塞", ex);
                LogHelper.WriteError(GenerateInfo.CH_E001, ex);
                TradePushBackImpl.Instanse.SaveModifyBack(e.Item);
                return;
            }
        }

        /// <summary>
        /// 分发方法
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void DispatchStockProcess(object sender, QueueItemHandleEventArg<StockDealEntity> e)
        {
            //回报不能为空
            if (e.Item == null)
            {
                LogHelper.WriteDebug("委托回报不能为空");
            }
            try
            {
                QueueBufferBase<StockDealEntity> bufferDeal = null;
                foreach (var queueBufferBase in StockDealList)
                {
                    if (bufferDeal == null || queueBufferBase.BufferedItemCount < bufferDeal.BufferedItemCount)
                        bufferDeal = queueBufferBase;
                }
                //委托回报失败
                if (bufferDeal == null)
                {
                    LogHelper.WriteDebug("委托回报中心.DispatchStockProcess无法找到回报队列，委托回报失败.");
                    return;
                }
                bufferDeal.InsertQueueItem(e.Item);
            }
            //委托回报分发异常
            catch (Exception ex)
            {
                string strMessage = "CH-2000:[委托回报分发异常]" + ex.Message;
                LogHelper.WriteError(strMessage, ex);
                return;

            }
        }

        /// <summary>
        /// 分发方法
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void DispatchFutureProcess(object sender, QueueItemHandleEventArg<FutureDealBackEntity> e)
        {
            //撮合中心委托回报为空判断
            if (e.Item == null)
            {
                LogHelper.WriteDebug("委托回报不能为空");
            }
            try
            {
                QueueBufferBase<FutureDealBackEntity> bufferDeal = null;
                foreach (var queueBufferBase in FutureDealList)
                {
                    if (bufferDeal == null || queueBufferBase.BufferedItemCount < bufferDeal.BufferedItemCount)
                        bufferDeal = queueBufferBase;
                }
                //撮合中心委托回报失败
                if (bufferDeal == null)
                {
                    LogHelper.WriteDebug("委托回报中心.DispatchStockProcess无法找到回报队列，委托回报失败.");
                    return;
                }
                bufferDeal.InsertQueueItem(e.Item);
            }
            //撮合中心委托回报异常
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E004, ex);
                return;

            }
        }
        /// <summary>
        /// 分发港股成交回报方法
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void DispatchHKStockProcess(object sender, QueueItemHandleEventArg<HKDealBackEntity> e)
        {
            #region 按要求实现港股委托成交回报
            if (e.Item == null)
            {
                LogHelper.WriteDebug("委托回报不能为空");
            }
            try
            {
                QueueBufferBase<HKDealBackEntity> bufferDeal = null;
                foreach (var queueBufferBase in HKDealList)
                {
                    if (bufferDeal == null || queueBufferBase.BufferedItemCount < bufferDeal.BufferedItemCount)
                        bufferDeal = queueBufferBase;
                }
                //委托回报失败
                if (bufferDeal == null)
                {
                    LogHelper.WriteDebug("委托回报中心.DispatchStockProcess无法找到回报队列，委托回报失败.");
                    return;
                }
                bufferDeal.InsertQueueItem(e.Item);
            }
            //委托回报分发异常
            catch (Exception ex)
            {
                string strMessage = "CH-2000:[委托回报分发异常]" + ex.Message;
                LogHelper.WriteError(strMessage, ex);
                return;

            }
            #endregion
        }
        
        /// <summary>
        /// 分发商品期货成交回报方法
        /// Create by 董鹏 2010-01-22
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void DispatchCommoditiesProcess(object sender, QueueItemHandleEventArg<CommoditiesDealBackEntity> e)
        {
            //撮合中心委托回报为空判断
            if (e.Item == null)
            {
                LogHelper.WriteDebug("委托回报不能为空");
            }
            try
            {
                QueueBufferBase<CommoditiesDealBackEntity> bufferDeal = null;
                foreach (var queueBufferBase in CommoditiesDealList)
                {
                    if (bufferDeal == null || queueBufferBase.BufferedItemCount < bufferDeal.BufferedItemCount)
                        bufferDeal = queueBufferBase;
                }
                //撮合中心委托回报失败
                if (bufferDeal == null)
                {
                    LogHelper.WriteDebug("委托回报中心.DispatchStockProcess无法找到回报队列，委托回报失败.");
                    return;
                }
                bufferDeal.InsertQueueItem(e.Item);
            }
            //撮合中心委托回报异常
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E004, ex);
                return;

            }
        }

        /// <summary>
        /// 删除成交记录
        /// </summary>
        /// <param name="id"></param>
        private void DeleteBackStock(string id)
        {
            try
            {
                //删除数据库
                DealOrderDataAccess.Delete(id);
            }
            //成交回报异常返回
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E003, ex);
                return;
            }
        }

        /// <summary>
        /// 删除港股成交记录
        /// </summary>
        /// <param name="id"></param>
        private void DeleteHKBackStock(string id)
        {
            try
            {
                //删除数据库                
                HKDealOrderDal.Delete(id);
            }
            //成交回报异常返回
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E003, ex);
                return;
            }
        }

        /// <summary>
        /// 删除成交记录
        /// </summary>
        /// <param name="id">撮合中心委托id</param>
        private void DeleteFuture(string id)
        {
            try
            {
                FutureDealOrderDataAccess.Delete(id);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E003, ex);
                return;
            }
        }

        /// <summary>
        /// 删除商品期货成交记录
        /// Create by 董鹏 2010-01-22
        /// </summary>
        /// <param name="id">撮合中心委托id</param>
        private void DeleteCommodities(string id)
        {
            try
            {
                CommoditiesDealOrderAccess.Delete(id);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(GenerateInfo.CH_E003, ex);
                return;
            }
        }

        /// <summary>
        /// 缓冲数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessBussiness(object sender, QueueItemHandleEventArg<StockDealEntity> e)
        {
            if (e.Item == null)
            {
                return;
            }
            ProcessStock(e.Item);

        }

        /// <summary>
        /// 现货成交回报
        /// </summary>
        /// <param name="model">委托单</param>
        private void ProcessStock(StockDealEntity model)
        {
            //撮合中心委托不能为空
            if (model == null)
            {
                return;
            }
            model.DealTime = DateTime.Now;
            var backEntity = new StockDealBackEntity();

            backEntity.Id = model.Id;  //对象标志
            backEntity.DealAmount = model.DealAmount;    //成交数量
            backEntity.DealPrice = model.DealPrice;      //成交价格
            backEntity.DealTime = model.DealTime;    //成交时间
            backEntity.OrderNo = model.OrderNo;  //委托单号码
            OperationContext context = null;
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelNo))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }
            }
            if (context == null)   //撮合中心上下文不能为空
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            var channel = context.Channel;
            //撮合中心保存通道，撮合中心通道不能为空
            if (channel == null)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            //回推通道状态判断
            if (channel.State != CommunicationState.Opened)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    var md = new DoBackStockDealDeletate(callback.ProcessStockDealRpt);
                    md.BeginInvoke(backEntity, null, null);
                }
                DeleteBackStock(model.Id);//数据库删除
            }
            catch (Exception ex)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                LogHelper.WriteError(GenerateInfo.CH_E002, ex);
                return;
            }
        }

        /// <summary>
        /// 撮合中心缓冲数据
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessFutureBussiness(object sender, QueueItemHandleEventArg<FutureDealBackEntity> e)
        {
            if (e.Item == null)
            {
                return;
            }
            ProcessFutureStock(e.Item);
        }

        /// <summary>
        /// 股指期货成交回报
        /// </summary>
        /// <param name="model">委托单</param>
        private void ProcessFutureStock(FutureDealBackEntity model)
        {
            //期货成交实体不能为空
            if (model == null)
            {
                return;
            }
            model.DealTime = DateTime.Now;
            OperationContext context = null;
            if (model.ChannelNo != null)    //上下文通道不能为空
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelNo))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }
                else
                {
                    LogHelper.WriteError("股指期货成交回报，在缓存通道号列表中找不到指定的" + model.ChannelNo +
                                  "通道号，回报数据以缓存到回推队列中!", new Exception("股指期货成交回报获取回推通道!"));
                }
            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            var channel = context.Channel;
            //撮合中心通道为空
            if (channel == null)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            if (channel.State != CommunicationState.Opened)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            try
            {
                
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    callback.ProcessStockIndexFuturesDealRpt(model);
                    DeleteFuture(model.Id);
                }
            }
            //成交回报异常处理
            catch (Exception ex)
            {
                
                string ip = ";" + channel.RemoteAddress.ToString() + ";" + channel.LocalAddress.ToString();
                TradePushBackImpl.Instanse.SaveDealBack(model);
                LogHelper.WriteError(GenerateInfo.CH_E002 + ip, ex);
                return;
            }
        }

        /// <summary>
        /// 缓冲数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessHKBussiness(object sender, QueueItemHandleEventArg<HKDealBackEntity> e)
        {
            if (e.Item == null)
            {
                return;
            }
            ProcessHKStock(e.Item);

        }

        /// <summary>
        /// 现货成交回报
        /// </summary>
        /// <param name="model">委托单</param>
        private void ProcessHKStock(HKDealBackEntity model)
        {
            //撮合中心委托不能为空
            if (model == null)
            {
                return;
            }
            model.DealTime = DateTime.Now;

            OperationContext context = null;
            if (model.ChannelID != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelID))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelID];
                }
            }
            if (context == null)   //撮合中心上下文不能为空
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            var channel = context.Channel;
            //撮合中心保存通道，撮合中心通道不能为空
            if (channel == null)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            //回推通道状态判断
            if (channel.State != CommunicationState.Opened)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    var md = new DoBackHKDealDeletate(callback.ProcessHKStockDealRpt);
                    md.BeginInvoke(model, null, null);
                }
                DeleteHKBackStock(model.ID);//数据库删除
            }
            catch (Exception ex)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                LogHelper.WriteError(GenerateInfo.CH_E002, ex);
                return;
            }
        }
        
        /// <summary>
        /// 撮合中心缓冲数据
        /// Create by 董鹏 2010-01-22
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessCommoditiesBussiness(object sender, QueueItemHandleEventArg<CommoditiesDealBackEntity> e)
        {
            if (e.Item == null)
            {
                return;
            }
            ProcessCommoditiesStock(e.Item);
        }

        /// <summary>
        /// 商品期货成交回报
        /// Create by 董鹏 2010-01-22
        /// </summary>
        /// <param name="model">委托单</param>
        private void ProcessCommoditiesStock(CommoditiesDealBackEntity model)
        {
            //期货成交实体不能为空
            if (model == null)
            {
                return;
            }
            model.DealTime = DateTime.Now;
            OperationContext context = null;
            if (model.ChannelID != null)    //上下文通道不能为空
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelID))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelID];
                }
                else
                {
                    LogHelper.WriteError("商品期货成交回报，在缓存通道号列表中找不到指定的" + model.ChannelID +
                                  "通道号，回报数据以缓存到回推队列中!", new Exception("商品期货成交回报获取回推通道!"));
                }
            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            var channel = context.Channel;
            //撮合中心通道为空
            if (channel == null)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            if (channel.State != CommunicationState.Opened)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();
                if (callback != null)
                {
                    var md = new DoBackCommoditiesStockDealDeletate(callback.ProcessMercantileDealRpt);
                    md.BeginInvoke(model, null, null);
                }
                DeleteBackStock(model.Id);//数据库删除


                //var callback = context.GetCallbackChannel<IDoOrderCallback>();
                //if (callback != null)
                //{
                //    callback.ProcessMercantileDealRpt(model);
                //    DeleteCommodities(model.Id);
                //}
            }
            //成交回报异常处理
            catch (Exception ex)
            {
                TradePushBackImpl.Instanse.SaveDealBack(model);
                LogHelper.WriteError(GenerateInfo.CH_E002, ex);
                return;
            }
        }
        
    }
}