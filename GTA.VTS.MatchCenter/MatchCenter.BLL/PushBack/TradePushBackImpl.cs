using MatchCenter.BLL.PushBack;
using System.Collections;
using MatchCenter.Entity;
using System.Collections.Generic;
using MatchCenter.DAL;
using MatchCenter.BLL.Common;
using MatchCenter.Entity.HK;
using MatchCenter.DAL.HK;
using System;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.BLL.PushBack
{
    /// <summary>
    /// Title:撮合中心相关信息成交处理类
    /// sealed以阻止发生派生
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Des:增加港股数据回推方法
    /// Update BY：王伟
    /// Update Date:2009-10-22 
    /// Desc:增加商品期货数据回推方法
    /// Update BY：董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public sealed class TradePushBackImpl : Singleton<TradePushBackImpl>
    {

        #region 单一进入模式
        /// <summary>
        /// 撮合中心相关信息成交处理类进入单一模式
        /// </summary>
        public static TradePushBackImpl Instanse
        {
            get
            {
                return singletonInstance;
            }
        }

        #endregion

        #region 要回推的现货成交记录保存到回推缓冲区中等待回推
        /// <summary>
        /// 保存成交回报
        /// 把要回推的现货成交记录保存到回推缓冲区中等待回推
        /// </summary>
        /// <param name="model">成交回报实体</param>
        public void SaveDealBack(StockDealEntity model)
        {
            #region 成交回报实体或者通道号不能为空
            //成交回报实体判断,成交回报实体不能为空。
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            #endregion

            #region 同步撮合中心回报
            lock (((ICollection)MatchCenterManager.Instance.DealBackEntitys).SyncRoot)
            {
                //撮合中心成交回报实体通道号判断
                if (MatchCenterManager.Instance.DealBackEntitys.ContainsKey(model.ChannelNo))
                {
                    Queue<StockDealEntity> queue = MatchCenterManager.Instance.DealBackEntitys[model.ChannelNo];
                    queue.Enqueue(model);
                    //这是一个类引用类型已经对以上改变了，不改再删除又添加进去，这样会再打乱之前的排序
                    //和增加系统开销
                    #region old code
                    //MatchCenter.Instance.DealBackEntitys.Remove(model.ChannelNo);
                    //MatchCenter.Instance.DealBackEntitys[model.ChannelNo] = queue;
                    #endregion
                }
                else
                {
                    var queue = new Queue<StockDealEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.DealBackEntitys[model.ChannelNo] = queue;
                }
            }
            #endregion
        }

        /// <summary>
        /// 保存成交回报
        /// </summary>
        /// <param name="model">成交回报实体</param>
        public void SaveDealBack(FutureDealBackEntity model)
        {
            #region 通道号或者成交实体不能为空
            //实体判断,成交回报实体不能为空。
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            #endregion

            #region 同步撮合中心委托回报实体
            lock (((ICollection)MatchCenterManager.Instance.DealFutureBackEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.DealFutureBackEntitys.ContainsKey(model.ChannelNo))
                {
                    Queue<FutureDealBackEntity> queue = MatchCenterManager.Instance.DealFutureBackEntitys[model.ChannelNo];
                    queue.Enqueue(model);
                    //MatchCenter.Instance.DealFutureBackEntitys.Remove(model.ChannelNo);
                    //MatchCenter.Instance.DealFutureBackEntitys[model.ChannelNo] = queue;
                }
                else
                {
                    //队列实体
                    var queue = new Queue<FutureDealBackEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.DealFutureBackEntitys[model.ChannelNo] = queue;
                }
            }
            #endregion
        }
        /// <summary>
        /// 保存港股成交回报
        /// </summary>
        /// <param name="model"></param>
        public void SaveDealBack(HKDealBackEntity model)
        {
            #region 通道号或者成交实体不能为空
            //实体判断,成交回报实体不能为空。
            if (model == null || string.IsNullOrEmpty(model.ChannelID))
            {
                return;
            }
            #endregion

            #region 同步撮合中心委托回报实体
            lock (((ICollection)MatchCenterManager.Instance.DealBackHKEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.DealBackHKEntitys.ContainsKey(model.ChannelID))
                {
                    Queue<HKDealBackEntity> queue = MatchCenterManager.Instance.DealBackHKEntitys[model.ChannelID];
                    queue.Enqueue(model);
                    //MatchCenter.Instance.DealFutureBackEntitys.Remove(model.ChannelNo);
                    //MatchCenter.Instance.DealFutureBackEntitys[model.ChannelNo] = queue;
                }
                else
                {
                    //队列实体
                    var queue = new Queue<HKDealBackEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.DealBackHKEntitys[model.ChannelID] = queue;
                }
            }
            #endregion
        }

        /// <summary>
        /// Desc: 保存商品期货成交回报
        /// Create by: 董鹏
        /// Create Date: 2010-01-25
        /// </summary>
        /// <param name="model">成交回报实体</param>
        public void SaveDealBack(CommoditiesDealBackEntity model)
        {
            #region 通道号或者成交实体不能为空
            //实体判断,成交回报实体不能为空。
            if (model == null || string.IsNullOrEmpty(model.ChannelID))
            {
                return;
            }
            #endregion

            #region 同步撮合中心委托回报实体
            lock (((ICollection)MatchCenterManager.Instance.DealCommoditiesBackEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.DealCommoditiesBackEntitys.ContainsKey(model.ChannelID))
                {
                    Queue<CommoditiesDealBackEntity> queue = MatchCenterManager.Instance.DealCommoditiesBackEntitys[model.ChannelID];
                    queue.Enqueue(model);
                }
                else
                {
                    //队列实体
                    var queue = new Queue<CommoditiesDealBackEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.DealCommoditiesBackEntitys[model.ChannelID] = queue;
                }
            }
            #endregion
        }

        /// <summary>
        /// 保存撮合中心现货撤单回报
        /// </summary>
        /// <param name="model">回报实体</param>
        public void SaveXHCancelBack(CancelOrderEntity model)
        {
            #region 通道号或者成交实体不能为空
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            #endregion

            #region 同步撤单实体
            lock (((ICollection)MatchCenterManager.Instance.CancelBackXHEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackXHEntitys.ContainsKey(model.ChannelNo))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackXHEntitys[model.ChannelNo];
                    queue.Enqueue(model);
                }
                else
                {
                    var queue = new Queue<CancelOrderEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.CancelBackXHEntitys[model.ChannelNo] = queue;
                }
            }
            #endregion
        }

        #region add by 董鹏 2010-04-30
        /// <summary>
        /// 保存撮合中心港股撤单回报
        /// </summary>
        /// <param name="model">回报实体</param>
        public void SaveHKCancelBack(CancelOrderEntity model)
        {
            #region 通道号或者成交实体不能为空
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            #endregion

            #region 同步撤单实体
            lock (((ICollection)MatchCenterManager.Instance.CancelBackHKEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackHKEntitys.ContainsKey(model.ChannelNo))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackHKEntitys[model.ChannelNo];
                    queue.Enqueue(model);
                }
                else
                {
                    var queue = new Queue<CancelOrderEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.CancelBackHKEntitys[model.ChannelNo] = queue;
                }
            }
            #endregion
        }

        /// <summary>
        /// 保存撮合中心股指期货撤单回报
        /// </summary>
        /// <param name="model">回报实体</param>
        public void SaveSICancelBack(CancelOrderEntity model)
        {
            #region 通道号或者成交实体不能为空
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            #endregion

            #region 同步撤单实体
            lock (((ICollection)MatchCenterManager.Instance.CancelBackSIEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackSIEntitys.ContainsKey(model.ChannelNo))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackSIEntitys[model.ChannelNo];
                    queue.Enqueue(model);
                }
                else
                {
                    var queue = new Queue<CancelOrderEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.CancelBackSIEntitys[model.ChannelNo] = queue;
                }
            }
            #endregion
        }

        /// <summary>
        /// 保存撮合中心商品期货撤单回报
        /// </summary>
        /// <param name="model">回报实体</param>
        public void SaveCFCancelBack(CancelOrderEntity model)
        {
            #region 通道号或者成交实体不能为空
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            #endregion

            #region 同步撤单实体
            lock (((ICollection)MatchCenterManager.Instance.CancelBackCFEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackCFEntitys.ContainsKey(model.ChannelNo))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackCFEntitys[model.ChannelNo];
                    queue.Enqueue(model);
                }
                else
                {
                    var queue = new Queue<CancelOrderEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.CancelBackCFEntitys[model.ChannelNo] = queue;
                }
            }
            #endregion
        }

        #endregion

        /// <summary>
        /// 保存撮合中心回报
        /// </summary>
        /// <param name="model">回报实体</param>
        public void SaveModifyBack(HKModifyBackEntity model)
        {
            #region 通道号或者成交实体不能为空
            if (model == null || string.IsNullOrEmpty(model.ChannelNo))
            {
                return;
            }
            #endregion

            #region 同步撤单实体
            lock (((ICollection)MatchCenterManager.Instance.ModifyBackEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.ModifyBackEntitys.ContainsKey(model.ChannelNo))
                {
                    Queue<HKModifyBackEntity> queue = MatchCenterManager.Instance.ModifyBackEntitys[model.ChannelNo];
                    queue.Enqueue(model);
                    //MatchCenter.Instance.CancelBackEntitys.Remove(model.ChannelNo);
                    //MatchCenter.Instance.CancelBackEntitys[model.ChannelNo] = queue;
                }
                else
                {
                    var queue = new Queue<HKModifyBackEntity>();
                    queue.Enqueue(model);
                    MatchCenterManager.Instance.ModifyBackEntitys[model.ChannelNo] = queue;
                }
            }
            #endregion
        }

        #endregion

        #region 实现回推相关队列插入方法开始实现回推
        #region 根据通道号期货成交回推方法
        /// <summary>
        /// 现货成交回推方法
        /// 根据通道号获取所要回推的数据队列实现异步回推
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertEntity(string channelId)
        {
            //通道是否为空
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            #region 同步撮合中心回报
            lock (((ICollection)MatchCenterManager.Instance.DealBackEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.DealBackEntitys.ContainsKey(channelId))
                {
                    Queue<StockDealEntity> queue = MatchCenterManager.Instance.DealBackEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.stockDealOrderCacheList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferEntityList.InsertQueueItem(entity);
                    //}
                    ////删除撮合中心回报
                    //MatchCenter.Instance.DealBackEntitys.Remove(channelId);
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        #region 根据通道号股指期货成交回推方法
        /// <summary>
        /// 根据通道号股指期货成交回推方法
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertFuture(string channelId)
        {
            //通道判断
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            //成交回报判断
            lock (((ICollection)MatchCenterManager.Instance.DealFutureBackEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.DealFutureBackEntitys.ContainsKey(channelId))
                {
                    Queue<FutureDealBackEntity> queue = MatchCenterManager.Instance.DealFutureBackEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.futureDealOrderCacheList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code 这里把通道号删除并不是取队列中数据发送即删除
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferFutureEntityList.InsertQueueItem(entity);
                    //}
                    ////删除成交回报
                    //MatchCenter.Instance.DealFutureBackEntitys.Remove(channelId);
                    #endregion
                }
            }
        }
        #endregion

        #region 根据通道号港股成交回推方法
        /// <summary>
        /// 港股成交回推方法
        /// 根据通道号获取所要回推的数据队列实现异步回推
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertHKEntity(string channelId)
        {
            //通道是否为空
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            #region 同步撮合中心回报
            lock (((ICollection)MatchCenterManager.Instance.DealBackHKEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.DealBackHKEntitys.ContainsKey(channelId))
                {
                    Queue<HKDealBackEntity> queue = MatchCenterManager.Instance.DealBackHKEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.hkstockDealOrderCatheList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferEntityList.InsertQueueItem(entity);
                    //}
                    ////删除撮合中心回报
                    //MatchCenter.Instance.DealBackEntitys.Remove(channelId);
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        #region 根据通道号商品期货成交回推方法
        /// <summary>
        /// 根据通道号商品期货成交回推方法
        /// </summary>
        /// <param name="channelId"></param>
        public void InsertCommodities(string channelId)
        {
            //通道判断
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            //成交回报判断
            lock (((ICollection)MatchCenterManager.Instance.DealCommoditiesBackEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.DealCommoditiesBackEntitys.ContainsKey(channelId))
                {
                    Queue<CommoditiesDealBackEntity> queue = MatchCenterManager.Instance.DealCommoditiesBackEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.commoditiesDealOrderCacheList.InsertQueueItem(queue.Dequeue());
                    }
                }
            }
        }
        #endregion

        #region 根据通道号撤单回推方法
        /// <summary>
        /// 现货——根据通道号撤单回推方法 
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertCancelXHEntity(string channelId)
        {
            //通道判断
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            //锁定撤单回报
            lock (((ICollection)MatchCenterManager.Instance.CancelBackXHEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackXHEntitys.ContainsKey(channelId))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackXHEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.BufferXHCancelEntityList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferCancelEntityList.InsertQueueItem(entity);
                    //}
                    ////删除撤单回报
                    //MatchCenter.Instance.CancelBackEntitys.Remove(channelId);
                    #endregion
                }
            }
        }

        #region add by 董鹏 2010-04-30       

        /// <summary>
        /// 股指期货——根据通道号撤单回推方法 
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertCancelSIEntity(string channelId)
        {
            //通道判断
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            //锁定撤单回报
            lock (((ICollection)MatchCenterManager.Instance.CancelBackSIEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackSIEntitys.ContainsKey(channelId))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackSIEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.BufferSICancelEntityList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferCancelEntityList.InsertQueueItem(entity);
                    //}
                    ////删除撤单回报
                    //MatchCenter.Instance.CancelBackEntitys.Remove(channelId);
                    #endregion
                }
            }
        }

        /// <summary>
        /// 商品期货——根据通道号撤单回推方法 
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertCancelCFEntity(string channelId)
        {
            //通道判断
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            //锁定撤单回报
            lock (((ICollection)MatchCenterManager.Instance.CancelBackCFEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackCFEntitys.ContainsKey(channelId))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackCFEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.BufferCFCancelEntityList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferCancelEntityList.InsertQueueItem(entity);
                    //}
                    ////删除撤单回报
                    //MatchCenter.Instance.CancelBackEntitys.Remove(channelId);
                    #endregion
                }
            }
        }
        #endregion
        #endregion

        #region 根据通道号撤单回推方法
        /// <summary>
        /// 根据通道号港股撤单回推方法 
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertCancelHKEntity(string channelId)
        {
            //通道判断
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            //锁定撤单回报
            lock (((ICollection)MatchCenterManager.Instance.CancelBackHKEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.CancelBackHKEntitys.ContainsKey(channelId))
                {
                    Queue<CancelOrderEntity> queue = MatchCenterManager.Instance.CancelBackHKEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.BufferHKCancelEntityList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferCancelEntityList.InsertQueueItem(entity);
                    //}
                    ////删除撤单回报
                    //MatchCenter.Instance.CancelBackEntitys.Remove(channelId);
                    #endregion
                }
            }
        }
        #endregion

        #region 根据通道号港股改单回推方法
        /// <summary>
        /// 根据通道号港股改单回推方法 
        /// </summary>
        /// <param name="channelId">通道号</param>
        public void InsertHKModifyEntity(string channelId)
        {
            //通道判断
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }
            //锁定撤单回报
            lock (((ICollection)MatchCenterManager.Instance.ModifyBackEntitys).SyncRoot)
            {
                if (MatchCenterManager.Instance.ModifyBackEntitys.ContainsKey(channelId))
                {
                    Queue<HKModifyBackEntity> queue = MatchCenterManager.Instance.ModifyBackEntitys[channelId];
                    while (queue.Count > 0)
                    {
                        MatchCenterManager.Instance.matchCenterBackService.BufferHKModifyEntityList.InsertQueueItem(queue.Dequeue());
                    }
                    #region old code
                    //foreach (var entity in queue)
                    //{
                    //    MatchCenter.Instance.MatchCenterBackService.BufferCancelEntityList.InsertQueueItem(entity);
                    //}
                    ////删除撤单回报
                    //MatchCenter.Instance.CancelBackEntitys.Remove(channelId);
                    #endregion
                }
            }
        }
        #endregion


        #endregion

        #region 初始化成交回推数据
        #region 从数据库中初始化现货和期货回推成交数据
        /// <summary>
        /// 从数据库中初始化现货和期货回推成交数据
        /// </summary>
        public void InitDealPushBackFromDataBase()
        {
            try
            {
                //根据配置初始化要撮合的商品代码
                string matchTypeStr = AppConfig.GetConfigMatchBreedClassType();
                if (matchTypeStr.Substring(3, 1) == "1")
                {
                    InitXH_DealPushBackFromDataBase();
                }
                if (matchTypeStr.Substring(2, 1) == "1")
                {
                    InitQH_DealPushBackFromDataBase();
                }
                if (matchTypeStr.Substring(1, 1) == "1")
                {
                    InitHK_DealPushBackFromDataBase();
                }
                if (matchTypeStr.Substring(0, 1) == "1")
                {
                    //add by 董鹏 2010-01-25
                    InitSPQH_DealPushBackFromDataBase();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-0023:从数据库中初始化现货和期货回推成交数据异常", ex);

            }
        }
        #endregion

        #region 初始化现货成交回报数据
        /// <summary>
        /// 初始化现货成交回报数据
        /// </summary>
        public void InitXH_DealPushBackFromDataBase()
        {
            List<StockDealEntity> list = DealOrderDataAccess.GetDealBackEntityList();
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            foreach (StockDealEntity model in list)
            {
                SaveDealBack(model);
            }
        }
        #endregion

        #region 初始化期货成交回报
        /// <summary>
        /// 初始化期货成交回报
        /// </summary>
        public void InitQH_DealPushBackFromDataBase()
        {
            List<FutureDealBackEntity> list = FutureDealOrderDataAccess.GetDealBackEntityList();
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            foreach (FutureDealBackEntity entity in list)
            {
                SaveDealBack(entity);
            }
        }
        #endregion

        #region 初始化港股成交回报数据
        /// <summary>
        /// 初始化港股成交回报数据
        /// </summary>
        public void InitHK_DealPushBackFromDataBase()
        {
            List<HKDealBackEntity> list = HKDealOrderDal.GetDealBackEntityList();

            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            foreach (HKDealBackEntity model in list)
            {
                SaveDealBack(model);
            }
        }

        #endregion

        #region 初始化商品期货成交回报 add by 董鹏 2010-01-25
        /// <summary>
        /// 初始化商品期货成交回报
        /// </summary>
        public void InitSPQH_DealPushBackFromDataBase()
        {
            List<CommoditiesDealBackEntity> list = CommoditiesDealOrderAccess.GetDealBackEntityList();
            if (Utils.IsNullOrEmpty(list))
            {
                return;
            }
            foreach (CommoditiesDealBackEntity entity in list)
            {
                SaveDealBack(entity);
            }
        }
        #endregion

        #endregion
    }
}
