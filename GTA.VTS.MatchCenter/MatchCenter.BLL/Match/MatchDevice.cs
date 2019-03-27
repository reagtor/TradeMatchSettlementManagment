using System;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;
using CommonRealtimeMarket;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.PushBack;
using MatchCenter.BLL.match;
using MatchCenter.Entity;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.BLL.Common;
using Amib.Threading;
using MatchCenter.BLL.Match;
using MatchCenter.Entity.HK;
using GTA.VTS.Common.CommonObject;
using MatchCenter.DAL;
using MatchCenter.BLL.MatchData;
using MatchCenter.BLL.ManagementCenter;

namespace MatchCenter.BLL
{
    /// <summary>
    /// 初始化callback返回类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Des:增加ModifyAsyncResult
    /// Update By：王伟
    /// Update Date:2009-10-20
    /// Desc:增加商品期货相关内容
    /// Update By：董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public class CancelAsyncResult
    {
        /// <summary>
        /// 撤单委托实体
        /// </summary>
        private CancelEntity cancelObj;
        /// <summary>
        /// 撤单委托实体
        /// </summary>
        private CacelStockOrderDelegate cancelMd;

        /// <summary>
        /// 委托
        /// </summary>
        public CacelStockOrderDelegate CancelMd
        {
            get
            {
                return cancelMd;
            }
            set
            {
                cancelMd = value;
            }
        }
        /// <summary>
        /// 撤单委托实体对象
        /// </summary>
        public CancelEntity CancelObj
        {
            get
            {
                return cancelObj;
            }
            set
            {
                cancelObj = value;
            }
        }

    }

    /// <summary>
    /// 初始化Modify的callback返回类
    /// Create BY：王伟
    /// Create Date：2009-10-20
    /// </summary>
    public class ModifyAsyncResult
    {
        /// <summary>
        /// 撤单委托实体
        /// </summary>
        private HKModifyEntity modifyObj;
        /// <summary>
        /// 撤单委托实体
        /// </summary>
        private ModifyHKOrderDelegate modifyd;

        /// <summary>
        /// 委托
        /// </summary>
        public ModifyHKOrderDelegate ModifyHKD
        {
            get
            {
                return modifyd;
            }
            set
            {
                modifyd = value;
            }
        }
        /// <summary>
        /// 撤单委托实体对象
        /// </summary>
        public HKModifyEntity ModifyObj
        {
            get
            {
                return modifyObj;
            }
            set
            {
                modifyObj = value;
            }
        }

    }


    /// <summary>
    /// 初始化撮合中心撮合机类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Desc.:添加港股相关方法和定义
    /// Update by：李健华
    /// Update date:2009-10-19
    /// Desc.:港股下单、撤单
    /// Update by：王伟
    /// Update date:2009-10-19
    /// Update By:董鹏
    /// Update Date:2009-12-18
    /// Desc.: 修改进入撮合机下单方法日志，在其中加了货品类别名称
    /// Desc.：添加商品期货相关的内容
    /// Update By: 董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public class MatchDevice
    {
        #region 变量定义
        /// <summary>
        /// 撮合中心委托缓冲区
        /// </summary>
        private static SmartThreadPool smartPool = new SmartThreadPool();
        /// <summary>
        /// 撮合中心委托缓冲区
        /// </summary>
        private QueueBufferBase<StockDataOrderEntity> stockOrderCache;
        /// <summary>
        /// 撮合中心港股委托缓冲区
        /// </summary>
        private QueueBufferBase<HKEntrustOrderInfo> hkOrderCache;
        /// <summary>
        /// 撮合中心委托缓冲区
        /// </summary>
        private QueueBufferBase<FutureDataOrderEntity> bufferFutureEntity;
        /// <summary>
        /// 撮合中心委托缓冲区
        /// </summary>
        private QueueBufferBase<CancelEntity> bufferCancel;
        /// <summary>
        /// 撮合中心股指期货委托缓冲区
        /// </summary>
        private QueueBufferBase<CancelEntity> bufferFutureCancel;
        /// <summary>
        /// 撮合中心委托缓冲区
        /// </summary>
        private QueueBufferBase<CancelEntity> bufferHKCancel;
        /// <summary>
        /// 改单缓冲区
        /// </summary>
        private QueueBufferBase<HKModifyEntity> bufferHKModify;

        #region 商品期货撮合中心委托缓冲区 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撮合中心委托缓冲区
        /// </summary>
        private QueueBufferBase<CommoditiesDataOrderEntity> bufferCommoditiesEntity;
        #endregion
        #region 撮合中心商品期货委托缓冲区 add by 董鹏 2010-01-22
        /// <summary>
        /// 撮合中心商品期货撤单缓冲区
        /// </summary>
        private QueueBufferBase<CancelEntity> bufferCommoditiesCancel;
        #endregion

        /// <summary>
        /// 撮合中心实体对象
        /// </summary>
        private readonly object sryobj;

        #region 撤单委托存储区定义(包括港股改单）
        /// <summary>
        /// 现货撤单委托存储区
        /// </summary>
        public List<CancelEntity> CancelOrderEntity;
        /// <summary>
        /// 股指期货撤单委托存储区
        /// </summary>
        public List<CancelEntity> CancelFutureEntity;
        /// <summary>
        /// 港股撤单委托存储区
        /// </summary>
        public List<CancelEntity> CancelHKOrderEntity;
        /// <summary>
        /// 港股改单委托存储区
        /// </summary>
        public List<HKModifyEntity> ModifyHKOrderEntity;

        #region 商品期货撤单委托存储区 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撤单委托存储区
        /// </summary>
        public List<CancelEntity> CancelCommoditiesEntity;
        #endregion
        #endregion

        ///// <summary>
        ///// 默认值
        ///// </summary>
        //public int DefaultInternal = 1000;

        #region 撮合单元定义Dictionary
        /// <summary>
        /// 股指期货撮合单元
        /// </summary>
        public IDictionary<string, FutureMatcher> FutureMarkers = new Dictionary<string, FutureMatcher>();
        /// <summary>
        /// 现货撮合单元
        /// </summary>
        public IDictionary<string, StockMatcher> StockMarkers = new Dictionary<string, StockMatcher>();
        /// <summary>
        /// 港股撮合单元
        /// </summary>
        public IDictionary<string, HKStockMatcher> HKStockMarkers = new Dictionary<string, HKStockMatcher>();

        #region 商品期货撮合单元 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撮合单元
        /// </summary>
        public IDictionary<string, SPQHMatcher> CommoditiesMarkers = new Dictionary<string, SPQHMatcher>();
        #endregion

        #endregion

        /// <summary>
        /// 交易所ID
        /// </summary>
        public int bourseTypeID = 0;
        #endregion

        #region 时间定时器
        /// <summary>
        /// 港股撤单定时器
        /// </summary>
        Timer CanHKTimer = null;
        /// <summary>
        /// 港股改单定时器
        /// </summary>
        Timer ModifyHKTimer = null;
        /// <summary>
        /// 股指期货撤单定时器
        /// </summary>
        Timer CanFutureTimer = null;
        /// <summary>
        /// 现货撤单定时器
        /// </summary>
        Timer CanCelTimer = null;

        #region 商品期货撤单定时器 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撤单定时器
        /// </summary>
        Timer CanCommoditiesTimer = null;
        #endregion
        #endregion

        static MatchDevice()
        {
            smartPool.MaxThreads = 100;
            smartPool.MinThreads = 25;
            smartPool.Start();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatchDevice()
        {
            #region old code
            //CancelOrderEntity = new List<CancelEntity>();
            //CancelFutureEntity = new List<CancelEntity>();
            //CancelHKOrderEntity = new List<CancelEntity>();
            ////改单存储体
            //ModifyHKOrderEntity = new List<HKModifyEntity>();

            sryobj = new object();

            //stockOrderCache = new QueueBufferBase<StockDataOrderEntity>();
            //stockOrderCache.QueueItemProcessEvent += ProcessBussiness;

            //hkOrderCache = new QueueBufferBase<HKEntrustOrderInfo>();
            //hkOrderCache.QueueItemProcessEvent += ProcessHKBussiness;

            //bufferFutureEntity = new QueueBufferBase<FutureDataOrderEntity>();
            //bufferFutureEntity.QueueItemProcessEvent += ProcessFutureBussiness;

            //Timer CanCelTimer = new Timer();
            //CanCelTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
            //CanCelTimer.Elapsed += OnTimerElapsed;
            //CanCelTimer.Enabled = true;

            //Timer CanFutureTimer = new Timer();
            //CanFutureTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
            //CanFutureTimer.Elapsed += OnTimeFutureElased;
            //CanFutureTimer.Enabled = true;

            //Timer CanHKTimer = new Timer();
            //CanHKTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
            //CanHKTimer.Elapsed += OnTimerHKElapsed;
            //CanHKTimer.Enabled = true;

            //Timer ModifyHKTimer = new Timer();
            //ModifyHKTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
            //ModifyHKTimer.Elapsed += OnTimerHKModifyElapsed;
            //ModifyHKTimer.Enabled = true;


            //bufferCancel = new QueueBufferBase<CancelEntity>();
            //bufferCancel.QueueItemProcessEvent += ProcessCancel;

            //bufferFutureCancel = new QueueBufferBase<CancelEntity>();
            //bufferFutureCancel.QueueItemProcessEvent += ProcessFutureCancel;

            //bufferHKCancel = new QueueBufferBase<CancelEntity>();
            //bufferHKCancel.QueueItemProcessEvent += ProcessHKCancel;

            ////增加改单关联事件
            //bufferHKModify = new QueueBufferBase<HKModifyEntity>();
            //bufferHKModify.QueueItemProcessEvent += ProcessHKModify;

            #endregion
        }

        /// <summary>
        /// 初始化撮合中心管理器相关事件方法
        /// </summary>
        /// <param name="breedTypeID"></param>
        public void IniMatchDevice(Types.BreedClassTypeEnum breedTypeID)
        {

            switch (breedTypeID)
            {
                case Types.BreedClassTypeEnum.Stock:
                    if (CancelOrderEntity == null)
                    {
                        CancelOrderEntity = new List<CancelEntity>();
                        RecoveryCancelOrder(breedTypeID);
                    }
                    if (stockOrderCache == null)
                    {
                        stockOrderCache = new QueueBufferBase<StockDataOrderEntity>();
                        stockOrderCache.QueueItemProcessEvent += ProcessBussiness;
                    }
                    if (CanCelTimer == null)
                    {
                        CanCelTimer = new Timer();
                        CanCelTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
                        CanCelTimer.Elapsed += OnTimerElapsed;
                        CanCelTimer.Enabled = true;
                    }
                    if (bufferCancel == null)
                    {
                        bufferCancel = new QueueBufferBase<CancelEntity>();
                        bufferCancel.QueueItemProcessEvent += ProcessCancel;
                    }

                    break;
                case Types.BreedClassTypeEnum.CommodityFuture:
                    #region 初始化商品期货相关内容 add by 董鹏 2010-01-22
                    if (CancelCommoditiesEntity == null)
                    {
                        CancelCommoditiesEntity = new List<CancelEntity>();
                        RecoveryCancelOrder(breedTypeID);
                    }
                    if (bufferCommoditiesEntity == null)
                    {
                        bufferCommoditiesEntity = new QueueBufferBase<CommoditiesDataOrderEntity>();
                        bufferCommoditiesEntity.QueueItemProcessEvent += ProcessCommoditiesBussiness;
                    }

                    if (CanCommoditiesTimer == null)
                    {
                        CanCommoditiesTimer = new Timer();
                        CanCommoditiesTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
                        CanCommoditiesTimer.Elapsed += OnTimeCommoditiesElased;
                        CanCommoditiesTimer.Enabled = true;
                    }
                    if (bufferCommoditiesCancel == null)
                    {
                        bufferCommoditiesCancel = new QueueBufferBase<CancelEntity>();
                        bufferCommoditiesCancel.QueueItemProcessEvent += ProcessCommoditiesCancel;
                    }
                    #endregion
                    break;
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    if (CancelFutureEntity == null)
                    {
                        CancelFutureEntity = new List<CancelEntity>();
                        RecoveryCancelOrder(breedTypeID);
                    }
                    if (bufferFutureEntity == null)
                    {
                        bufferFutureEntity = new QueueBufferBase<FutureDataOrderEntity>();
                        bufferFutureEntity.QueueItemProcessEvent += ProcessFutureBussiness;
                    }

                    if (CanFutureTimer == null)
                    {
                        CanFutureTimer = new Timer();
                        CanFutureTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
                        CanFutureTimer.Elapsed += OnTimeFutureElased;
                        CanFutureTimer.Enabled = true;
                    }
                    if (bufferFutureCancel == null)
                    {
                        bufferFutureCancel = new QueueBufferBase<CancelEntity>();
                        bufferFutureCancel.QueueItemProcessEvent += ProcessFutureCancel;
                    }
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    if (CancelHKOrderEntity == null)
                    {
                        CancelHKOrderEntity = new List<CancelEntity>();
                        RecoveryCancelOrder(breedTypeID);
                    }
                    if (ModifyHKOrderEntity == null)
                    {
                        //改单存储体
                        ModifyHKOrderEntity = new List<HKModifyEntity>();
                    }
                    if (hkOrderCache == null)
                    {
                        hkOrderCache = new QueueBufferBase<HKEntrustOrderInfo>();
                        hkOrderCache.QueueItemProcessEvent += ProcessHKBussiness;
                    }
                    if (CanHKTimer == null)
                    {
                        CanHKTimer = new Timer();
                        CanHKTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
                        CanHKTimer.Elapsed += OnTimerHKElapsed;
                        CanHKTimer.Enabled = true;
                    }
                    if (ModifyHKTimer == null)
                    {
                        ModifyHKTimer = new Timer();
                        ModifyHKTimer.Interval = RulesDefaultValue.DefaultInternal / 50;//2秒钟
                        ModifyHKTimer.Elapsed += OnTimerHKModifyElapsed;
                        ModifyHKTimer.Enabled = true;
                    }
                    if (bufferHKCancel == null)
                    {
                        bufferHKCancel = new QueueBufferBase<CancelEntity>();
                        bufferHKCancel.QueueItemProcessEvent += ProcessHKCancel;
                    }
                    if (bufferHKModify == null)
                    {
                        //增加改单关联事件
                        bufferHKModify = new QueueBufferBase<HKModifyEntity>();
                        bufferHKModify.QueueItemProcessEvent += ProcessHKModify;
                    }
                    break;
                default:
                    break;
            }

        }


        #region 时间定时器撤单(包括港股改单)操作
        /// <summary>
        /// 现货撤单定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            //撮合中心时间判断
            if (TradeTimeManager.Instanse.IsEndTime(bourseTypeID))
            {
                Clear();
            }
            if (TradeTimeManager.Instanse.IsMarchTime(bourseTypeID, DateTime.Now)
                && TradeTimeManager.Instanse.IsMarchDate(bourseTypeID, DateTime.Now))
            {
                List<CancelEntity> entities;
                if (!Utils.IsNullOrEmpty(CancelOrderEntity))
                {
                    lock (sryobj)
                    {
                        entities = CancelOrderEntity;
                        CancelOrderEntity = new List<CancelEntity>();
                    }
                }
                else
                {
                    return;
                }
                //撮合中心实体不能为空
                if (Utils.IsNullOrEmpty(entities))
                {
                    return;
                }
                foreach (CancelEntity entity in entities)
                {
                    bufferCancel.InsertQueueItem(entity);

                }
            }
        }

        /// <summary>
        /// 港股撤单定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerHKElapsed(object sender, ElapsedEventArgs args)
        {
            //撮合中心时间判断
            if (TradeTimeManager.Instanse.IsEndTime(bourseTypeID))
            {
                HKClear();
            }
            if (TradeTimeManager.Instanse.IsMarchTime(bourseTypeID, DateTime.Now)
                && TradeTimeManager.Instanse.IsMarchDate(bourseTypeID, DateTime.Now))
            {
                List<CancelEntity> entities;
                if (!Utils.IsNullOrEmpty(CancelHKOrderEntity))
                {
                    lock (sryobj)
                    {
                        entities = CancelHKOrderEntity;
                        CancelHKOrderEntity = new List<CancelEntity>();
                    }
                }
                else
                {
                    return;
                }
                //撮合中心实体不能为空
                if (Utils.IsNullOrEmpty(entities))
                {
                    return;
                }
                foreach (CancelEntity entity in entities)
                {
                    bufferHKCancel.InsertQueueItem(entity);

                }
            }
        }

        /// <summary>
        /// 港股改单触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerHKModifyElapsed(object sender, ElapsedEventArgs args)
        {
            //撮合中心时间判断
            if (TradeTimeManager.Instanse.IsEndTime(bourseTypeID))
            {
                HKClear();
            }
            if (TradeTimeManager.Instanse.IsMarchTime(bourseTypeID, DateTime.Now)
                && TradeTimeManager.Instanse.IsMarchDate(bourseTypeID, DateTime.Now))
            {
                List<HKModifyEntity> entities;
                if (!Utils.IsNullOrEmpty(ModifyHKOrderEntity))
                {
                    lock (sryobj)
                    {
                        entities = ModifyHKOrderEntity;
                        ModifyHKOrderEntity = new List<HKModifyEntity>();
                    }
                }
                else
                {
                    return;
                }
                //撮合中心实体不能为空
                if (Utils.IsNullOrEmpty(entities))
                {
                    return;
                }
                foreach (HKModifyEntity entity in entities)
                {
                    bufferHKModify.InsertQueueItem(entity);

                }
            }
        }

        /// <summary>
        /// 撮合中心股指期货撤单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimeFutureElased(object sender, ElapsedEventArgs args)
        {
            //撮合中心时间判断
            if (TradeTimeManager.Instanse.IsEndTime(bourseTypeID))
            {
                FutureClear();
            }

            if (TradeTimeManager.Instanse.IsMarchTime(bourseTypeID, DateTime.Now)
                && TradeTimeManager.Instanse.IsMarchDate(bourseTypeID, DateTime.Now))
            {
                List<CancelEntity> entities;
                //撤单实体不能为空
                if (CancelFutureEntity != null && CancelFutureEntity.Count > 0)
                {
                    lock (sryobj)
                    {
                        entities = CancelFutureEntity;
                        CancelFutureEntity = new List<CancelEntity>();

                    }
                }
                else
                {
                    return;
                }
                //实体不能为空
                if (entities == null || entities.Count <= 0)
                {
                    return;
                }
                foreach (CancelEntity entity in entities)
                {
                    bufferFutureCancel.InsertQueueItem(entity);

                }
            }
        }

        #region 撮合中心商品期货撤单 add by 董鹏 2010-01-22
        /// <summary>
        /// 撮合中心商品期货撤单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimeCommoditiesElased(object sender, ElapsedEventArgs args)
        {
            //撮合中心时间判断
            if (TradeTimeManager.Instanse.IsEndTime(bourseTypeID))
            {
                CommoditiesClear();
            }

            if (TradeTimeManager.Instanse.IsMarchTime(bourseTypeID, DateTime.Now) && TradeTimeManager.Instanse.IsMarchDate(bourseTypeID, DateTime.Now))
            {
                List<CancelEntity> entities;
                //撤单实体不能为空
                if (CancelCommoditiesEntity != null && CancelCommoditiesEntity.Count > 0)
                {
                    lock (sryobj)
                    {
                        entities = CancelCommoditiesEntity;
                        CancelCommoditiesEntity = new List<CancelEntity>();

                    }
                }
                else
                {
                    return;
                }
                //实体不能为空
                if (entities == null || entities.Count <= 0)
                {
                    return;
                }
                foreach (CancelEntity entity in entities)
                {
                    bufferCommoditiesCancel.InsertQueueItem(entity);

                }
            }
        }

        #endregion
        #endregion

        #region 撤单事件
        /// <summary>
        /// 撮合中心撤单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessCancel(object sender, QueueItemHandleEventArg<CancelEntity> e)
        {
            CancelEntity entity = e.Item;
            //实体不能为空
            if (entity == null)
            {
                return;
            }
            //撮合中心证卷代码不能为空
            if (string.IsNullOrEmpty(entity.StockCode))
            {
                return;
            }
            if (!StockMarkers.ContainsKey(entity.StockCode))
            {
                return;
            }
            StockMatcher matcher = StockMarkers[entity.StockCode];
            if (matcher != null)
            {
                CancelAsyncResult result = new CancelAsyncResult();
                CacelStockOrderDelegate matchStockDelegate = matcher.CancelOrder;
                result.CancelObj = entity;
                result.CancelMd = matchStockDelegate;
                matchStockDelegate.BeginInvoke(entity, CallBackMethod, result);
            }


        }

        /// <summary>
        /// 股指期货撤单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessFutureCancel(object sender, QueueItemHandleEventArg<CancelEntity> e)
        {
            CancelEntity entity = e.Item;
            //撮合中心实体不能为空
            if (entity == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(entity.StockCode))
            {
                return;
            }
            if (!FutureMarkers.ContainsKey(entity.StockCode))
            {
                return;
            }
            FutureMatcher Fmatcher = FutureMarkers[entity.StockCode];
            //撮合中心实体不能为空
            if (Fmatcher != null)
            {
                CancelAsyncResult result = new CancelAsyncResult();
                CacelStockOrderDelegate matchStockDelegate = Fmatcher.CancelOrder;

                result.CancelObj = entity;
                result.CancelMd = matchStockDelegate;
                matchStockDelegate.BeginInvoke(entity, CallBackFutureMethod, result);

                //IWorkItemResult result1 = smartPool.QueueWorkItem(delegate(object state)
                //{
                //    return DateTime.Now;
                //}, null, delegate(IWorkItemResult _result)
                //{
                //    Console.WriteLine(_result.Result);
                //});
            }
        }

        /// <summary>
        /// 港股撤单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessHKCancel(object sender, QueueItemHandleEventArg<CancelEntity> e)
        {
            CancelEntity entity = e.Item;
            //实体不能为空
            if (entity == null)
            {
                return;
            }
            //撮合中心证卷代码不能为空
            if (string.IsNullOrEmpty(entity.StockCode))
            {
                return;
            }
            if (!HKStockMarkers.ContainsKey(entity.StockCode))
            {
                return;
            }
            HKStockMatcher HKmatcher = HKStockMarkers[entity.StockCode];
            if (HKmatcher != null)
            {

                CancelAsyncResult result = new CancelAsyncResult();
                CacelStockOrderDelegate matchStockDelegate = HKmatcher.CancelOrder;
                result.CancelObj = entity;
                result.CancelMd = matchStockDelegate;
                matchStockDelegate.BeginInvoke(entity, CallBackHKMethod, result);


            }


        }

        #region 商品期货撤单 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撤单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessCommoditiesCancel(object sender, QueueItemHandleEventArg<CancelEntity> e)
        {
            CancelEntity entity = e.Item;
            //撮合中心实体不能为空
            if (entity == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(entity.StockCode))
            {
                return;
            }
            if (!CommoditiesMarkers.ContainsKey(entity.StockCode))
            {
                return;
            }
            SPQHMatcher Fmatcher = CommoditiesMarkers[entity.StockCode];
            //撮合中心实体不能为空
            if (Fmatcher != null)
            {
                CancelAsyncResult result = new CancelAsyncResult();
                CacelStockOrderDelegate matchStockDelegate = Fmatcher.CancelOrder;

                result.CancelObj = entity;
                result.CancelMd = matchStockDelegate;
                matchStockDelegate.BeginInvoke(entity, CallBackCommoditiesMethod, result);
            }
        }

        #endregion
        #endregion

        #region 撤单不成功继续撤单回调检查事件
        /// <summary>
        /// 股指期货撤单不成功继续撤单
        /// </summary>
        /// <param name="ar">回调参数</param>
        protected void CallBackFutureMethod(IAsyncResult ar)
        {
            CancelAsyncResult asyncResult = (CancelAsyncResult)ar.AsyncState;
            if (asyncResult == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelObj == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelMd == null)
                return;
            int result = asyncResult.CancelMd.EndInvoke(ar);
            if (result == 0)
            {
                if (asyncResult.CancelObj.CancelCount <= RulesDefaultValue.DefaultCancelFailCount)
                {
                    FutureAdd(asyncResult.CancelObj);
                }
                else
                {
                    CanceFailBack.Futureinstanse.Add(asyncResult.CancelObj);
                }

            }

        }

        /// <summary>
        /// 撤单不成功继续撤单
        /// </summary>
        /// <param name="ar">回调参数</param>
        protected void CallBackMethod(IAsyncResult ar)
        {
            CancelAsyncResult asyncResult = (CancelAsyncResult)ar.AsyncState;
            //撮合中心实体不能为空
            if (asyncResult == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelObj == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelMd == null)
                return;
            int result = asyncResult.CancelMd.EndInvoke(ar);
            if (result == 0)
            {
                if (asyncResult.CancelObj.CancelCount <= RulesDefaultValue.DefaultCancelFailCount / 2)
                {
                    Add(asyncResult.CancelObj);
                }
                else
                {
                    CanceFailBack.Stockinstanse.Add(asyncResult.CancelObj);
                }

            }

        }

        /// <summary>
        /// 港股撤单不成功继续撤单
        /// </summary>
        /// <param name="ar">回调参数</param>
        protected void CallBackHKMethod(IAsyncResult ar)
        {
            CancelAsyncResult asyncResult = (CancelAsyncResult)ar.AsyncState;
            //撮合中心实体不能为空
            if (asyncResult == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelObj == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelMd == null)
                return;
            int result = asyncResult.CancelMd.EndInvoke(ar);
            if (result == 0)
            {
                if (asyncResult.CancelObj.CancelCount <= RulesDefaultValue.DefaultCancelFailCount / 2)
                {
                    HKAdd(asyncResult.CancelObj);
                }
                else
                {
                    CanceFailBack.HKInstance.Add(asyncResult.CancelObj);
                }

            }

        }

        /// <summary>
        /// 港股改单不成功继续改单
        /// </summary>
        /// <param name="ar">回调参数</param>
        protected void CallBackHKModifyMethod(IAsyncResult ar)
        {
            ModifyAsyncResult asyncResult = (ModifyAsyncResult)ar.AsyncState;
            //撮合中心实体不能为空
            if (asyncResult == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.ModifyObj == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.ModifyHKD == null)
                return;
            int result = asyncResult.ModifyHKD.EndInvoke(ar);
            if (result == 0)
            {
                //if (asyncResult.ModifyObj.ModCount <= RulesDefaultValue.DefaultCancelFailCount / 2)
                //{
                //    HKModifyAdd(asyncResult.ModifyObj);
                //}
                //else
                //{
                //    //CanceFailBack.HKInstance.Add(asyncResult.ModifyObj);
                ModifyFailHKStockBack.Instance.Add(asyncResult.ModifyObj);


                //}

            }

        }

        #region 商品期货撤单不成功继续撤单 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撤单不成功继续撤单
        /// </summary>
        /// <param name="ar">回调参数</param>
        protected void CallBackCommoditiesMethod(IAsyncResult ar)
        {
            CancelAsyncResult asyncResult = (CancelAsyncResult)ar.AsyncState;
            if (asyncResult == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelObj == null)
                return;
            //撮合中心实体不能为空
            if (asyncResult.CancelMd == null)
                return;
            int result = asyncResult.CancelMd.EndInvoke(ar);
            if (result == 0)
            {
                if (asyncResult.CancelObj.CancelCount <= RulesDefaultValue.DefaultCancelFailCount)
                {
                    CommoditiesAdd(asyncResult.CancelObj);
                }
                else
                {
                    CanceFailBack.CommoditiesInstanse.Add(asyncResult.CancelObj);
                }

            }

        }
        #endregion
        #endregion

        #region 异步接收委托单事件
        /// <summary>
        /// 接收现货委托单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessBussiness(object sender, QueueItemHandleEventArg<StockDataOrderEntity> e)
        {
            //撮合中心代码不能为空
            if (e.Item.StockCode != null)
            {
                StockMatcher matcher = StockMarkers[e.Item.StockCode];
                if (matcher != null)
                {
                    //MatchStockDelegate matchStockDelegate = matcher.AcceptOrder;
                    //matchStockDelegate.BeginInvoke(e.Item, null, null);
                    smartPool.QueueWorkItem(matcher.AcceptOrder, e.Item);
                }
            }
        }

        /// <summary>
        /// 接收港股委托单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessHKBussiness(object sender, QueueItemHandleEventArg<HKEntrustOrderInfo> e)
        {
            if (e.Item.HKSecuritiesCode != null)
            {
                HKStockMatcher matcher = HKStockMarkers[e.Item.HKSecuritiesCode];
                if (matcher != null)
                {
                    smartPool.QueueWorkItem(matcher.AcceptOrder, e.Item);
                }
            }
        }

        /// <summary>
        /// 接收委托单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessFutureBussiness(object sender, QueueItemHandleEventArg<FutureDataOrderEntity> e)
        {
            //撮合中心对象不能为空
            if (e.Item.StockCode != null)
            {
                FutureMatcher matcher = FutureMarkers[e.Item.StockCode];
                //撮合中心对象不能为空
                if (matcher != null)
                {
                    //MatchFutureOrderDelegate matchFutureOrderDelegate = matcher.AcceptOrder;
                    //matchFutureOrderDelegate.BeginInvoke(e.Item, null, null);
                    smartPool.QueueWorkItem(matcher.AcceptOrder, e.Item);
                }
            }
        }

        #region 接收商品期货委托单 add by 董鹏 2010-01-22

        /// <summary>
        /// 接收商品期货委托单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessCommoditiesBussiness(object sender, QueueItemHandleEventArg<CommoditiesDataOrderEntity> e)
        {
            //撮合中心对象不能为空
            if (e.Item.StockCode != null)
            {
                SPQHMatcher matcher = CommoditiesMarkers[e.Item.StockCode];
                //撮合中心对象不能为空
                if (matcher != null)
                {
                    smartPool.QueueWorkItem(matcher.AcceptOrder, e.Item);
                }
            }
        }
        #endregion
        #endregion

        #region 清除所有委托
        /// <summary>
        /// 清除现货所有委托
        /// </summary>
        private void Clear()
        {
            if (AppConfig.IsAppStartInitialize)
            {
                return;
            }

            if (StockMarkers != null && StockMarkers.Count > 0)
            {
                foreach (string strKey in StockMarkers.Keys)
                {
                    StockMatcher matcher = StockMarkers[strKey];
                    //撮合中心对象不能为空
                    if (matcher != null)
                    {
                        matcher.Clear();
                    }
                }
            }
            if (!Utils.IsNullOrEmpty(CancelOrderEntity))
            {
                CancelOrderEntity.Clear();
            }
        }
        /// <summary>
        /// 清除股指期货所有委托
        /// </summary>
        private void FutureClear()
        {

            if (AppConfig.IsAppStartInitialize)
            {
                return;
            }
            if (FutureMarkers != null && FutureMarkers.Count > 0)
            {
                foreach (string strKey in FutureMarkers.Keys)
                {
                    FutureMatcher matcher = FutureMarkers[strKey];
                    //撮合中心对象不能为空
                    if (matcher != null)
                    {
                        matcher.Clear();
                    }
                }
            }
            //if (CancelFutureEntity != null && CancelFutureEntity.Count > 0)
            if (!Utils.IsNullOrEmpty(CancelFutureEntity))
            {
                CancelFutureEntity.Clear();
            }
        }

        /// <summary>
        /// 清除港股所有委托
        /// </summary>
        private void HKClear()
        {

            if (AppConfig.IsAppStartInitialize)
            {
                return;
            }
            if (HKStockMarkers != null && HKStockMarkers.Count > 0)
            {
                foreach (string strKey in HKStockMarkers.Keys)
                {
                    HKStockMatcher matcher = HKStockMarkers[strKey];
                    //撮合中心对象不能为空
                    if (matcher != null)
                    {
                        matcher.Clear();
                    }
                }
            }
            if (!Utils.IsNullOrEmpty(CancelHKOrderEntity))
            {
                CancelHKOrderEntity.Clear();
                //如有改单也要清除
                ModifyHKOrderEntity.Clear();
            }
        }

        #region add by 董鹏 2010-01-22
        /// <summary>
        /// 清除商品期货所有委托
        /// </summary>
        private void CommoditiesClear()
        {

            if (AppConfig.IsAppStartInitialize)
            {
                return;
            }
            if (CommoditiesMarkers != null && CommoditiesMarkers.Count > 0)
            {
                foreach (string strKey in CommoditiesMarkers.Keys)
                {
                    SPQHMatcher matcher = CommoditiesMarkers[strKey];
                    //撮合中心对象不能为空
                    if (matcher != null)
                    {
                        matcher.Clear();
                    }
                }
            }
            //if (CancelFutureEntity != null && CancelFutureEntity.Count > 0)
            if (!Utils.IsNullOrEmpty(CancelCommoditiesEntity))
            {
                CancelCommoditiesEntity.Clear();
            }
        }
        #endregion
        #endregion

        #region 调度接收委托(包括港股改单)单下单
        /// <summary>
        /// 调度委托单
        /// </summary>
        /// <param name="dataOrder">现货委托单</param>
        /// <returns></returns>
        public ResultDataEntity DoStockOrder(StockDataOrderEntity dataOrder)
        {
            LogHelper.WriteDebug(string.Format(GenerateInfo.CH_D004, GenerateInfo.CH_D01));
            //撮合中心实体不能为空
            if (dataOrder == null)
            {
                return null;
            }
            string code = Guid.NewGuid().ToString();
            var result = new ResultDataEntity();
            //撮合中心对象标志
            result.Id = code;
            if (TradeTimeManager.Instanse.IsAcceptTime(DateTime.Now, dataOrder.StockCode) == false)
            {
                string mesg = string.Format(GenerateInfo.CH_D003, "委托单");
                LogHelper.WriteDebug(mesg);
                result.Message = mesg;
                return result;
            }
            //ShowRejuctMessageDeletate messageDeletate = MatchCenterManager.Instance.ShowDoWorkMessage;
            //messageDeletate.BeginInvoke(dataOrder, null, null);
            string showMesg = string.Format(GenerateInfo.CH_D001, GenerateInfo.CH_D01, code, dataOrder.StockCode, DateTime.Now, dataOrder.OrderVolume, dataOrder.IsMarketPrice == 0 ? "市价" : "限价");
            smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);

            result.Id = code;
            //委托单号码
            result.OrderNo = code;
            //委托单编号
            dataOrder.OrderNo = code;
            //每次下单添加到下单列表，为了过滤行情所用
            smartPool.QueueWorkItem(MatchCodeDictionary.AddXH_ActivityOrderDic, dataOrder.StockCode);
            stockOrderCache.InsertQueueItem(dataOrder);
            return result;
        }

        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="dataOrder">委托单</param>
        /// <returns></returns>
        public ResultDataEntity DoFutureOrder(FutureDataOrderEntity dataOrder)
        {

            LogHelper.WriteDebug(string.Format(GenerateInfo.CH_D004, GenerateInfo.CH_D02));
            if (dataOrder == null)    //撮合中心委托单不能为空
            {
                return null;
            }
            string code = Guid.NewGuid().ToString();
            var result = new ResultDataEntity();
            result.Id = code; //对象标志
            if (TradeTimeManager.Instanse.IsAcceptTime(DateTime.Now, dataOrder.StockCode) == false)
            {
                string mesg = string.Format(GenerateInfo.CH_D003, "委托单");
                LogHelper.WriteDebug(mesg);
                result.Message = mesg;
                return result;
            }
            //ShowFutureMessageDeletete messageDeletate = MatchCenterManager.Instance.ShowDoWorkMessage;
            //messageDeletate.BeginInvoke(dataOrder, null, null);
            string showMesg = string.Format(GenerateInfo.CH_D001, GenerateInfo.CH_D02, code, dataOrder.StockCode, DateTime.Now, dataOrder.OrderVolume, dataOrder.IsMarketPrice == 0 ? "市价" : "限价");
            smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);

            result.Id = code;
            result.OrderNo = code;    //委托单号码
            dataOrder.OrderNo = code;
            //每次下单添加到下单列表，为了过滤行情所用
            smartPool.QueueWorkItem(MatchCodeDictionary.AddQH_ActivityOrderDic, dataOrder.StockCode);
            bufferFutureEntity.InsertQueueItem(dataOrder);
            return result;

        }

        /// <summary>
        /// 调度港股委托下单
        /// </summary>
        /// <param name="model">港股委托单实体</param>
        /// <returns></returns>
        public ResultDataEntity DoHKStockOrder(HKEntrustOrderInfo model)
        {
            LogHelper.WriteDebug(string.Format(GenerateInfo.CH_D004, GenerateInfo.CH_D03));
            if (model == null)
            {
                return null;
            }
            string code = Guid.NewGuid().ToString();
            ResultDataEntity result = new ResultDataEntity();
            result.Id = code;
            if (TradeTimeManager.Instanse.IsHKAcceptTime(DateTime.Now, model.HKSecuritiesCode) == false)
            {
                string mesg = string.Format(GenerateInfo.CH_D003, "委托单");
                LogHelper.WriteDebug(mesg + code);
                result.Message = mesg;
                return result;
            }
            string showMesg = string.Format(GenerateInfo.CH_D001, GenerateInfo.CH_D03, code, model.HKSecuritiesCode, DateTime.Now, model.OrderVolume, model.OrderType);
            smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);

            result.Id = code;
            //委托单号码
            result.OrderNo = code;
            //委托单编号
            model.OrderNo = code;
            //每次下单添加到下单列表，为了过滤行情所用
            smartPool.QueueWorkItem(MatchCodeDictionary.AddHK_ActivityOrderDic, model.HKSecuritiesCode);
            hkOrderCache.InsertQueueItem(model);
            return result;
        }
        /// <summary>
        /// 港股改单
        /// </summary>
        /// <param name="model">委托</param>
        /// <returns></returns>
        public HKModifyResultEntity ModifyHKOrder(HKModifyEntity model)
        {
            //撮合中心委托不能为空
            if (model == null)
            {
                return null;
            }

            var entity = new HKModifyResultEntity();
            //撮合中心委托单号码
            entity.OrderNo = model.OldOrderNo;
            if (TradeTimeManager.Instanse.IsHKAcceptTime(DateTime.Now, model.StockCode) == false)
            {
                entity.IsSuccess = false;
                entity.Message = string.Format(GenerateInfo.CH_D003, "改单");
                return entity;
            }
            else
            {
                entity.IsSuccess = true;
                entity.Message = GenerateInfo.CH_D011;
            }
            //撮合中心代码不能为空
            if (model.StockCode != null)
            {
                string showMesg = string.Format(GenerateInfo.CH_D012, model.OldOrderNo, model.StockCode, DateTime.Now);

                HKModifyAdd(model);

                LogHelper.WriteDebug(showMesg);
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);
            }

            return entity;
        }

        #region
        /// <summary>
        /// 商品期货下单
        /// </summary>
        /// <param name="dataOrder">委托单</param>
        /// <returns></returns>
        public ResultDataEntity DoCommoditiesOrder(CommoditiesDataOrderEntity dataOrder)
        {

            LogHelper.WriteDebug(string.Format(GenerateInfo.CH_D004, GenerateInfo.CH_D04));
            if (dataOrder == null)    //撮合中心委托单不能为空
            {
                return null;
            }
            string code = Guid.NewGuid().ToString();
            var result = new ResultDataEntity();
            result.Id = code; //对象标志
            if (TradeTimeManager.Instanse.IsAcceptTime(DateTime.Now, dataOrder.StockCode) == false)
            {
                string mesg = string.Format(GenerateInfo.CH_D003, "委托单");
                LogHelper.WriteDebug(mesg);
                result.Message = mesg;
                return result;
            }
            string showMesg = string.Format(GenerateInfo.CH_D001, GenerateInfo.CH_D04, code, dataOrder.StockCode, DateTime.Now, dataOrder.OrderVolume, "限价");//dataOrder.IsMarketPrice == 0 ? "市价" : "限价");
            smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);

            result.Id = code;
            result.OrderNo = code;    //委托单号码
            dataOrder.OrderNo = code;
            //每次下单添加到下单列表，为了过滤行情所用
            smartPool.QueueWorkItem(MatchCodeDictionary.AddSPQH_ActivityOrderDic, dataOrder.StockCode);
            bufferCommoditiesEntity.InsertQueueItem(dataOrder);
            return result;
        }
        #endregion

        #endregion

        #region 调度撤单(包括港股改单)委托
        /// <summary>
        /// 现货撤单
        /// </summary>
        /// <param name="model">委托</param>
        /// <returns></returns>
        public CancelResultEntity CancelOrder(CancelEntity model)
        {
            //撮合中心委托不能为空
            if (model == null)
            {
                return null;
            }

            var entity = new CancelResultEntity();
            //撮合中心委托单号码
            entity.OrderNo = model.OldOrderNo;
            if (TradeTimeManager.Instanse.IsAcceptTime(DateTime.Now, model.StockCode) == false)
            {
                entity.IsSuccess = false;
                entity.Message = string.Format(GenerateInfo.CH_D003, "撤单");
                return entity;
            }
            else
            {
                entity.IsSuccess = true;
                entity.Message = GenerateInfo.CH_D006;
            }
            //撮合中心代码不能为空
            if (model.StockCode != null)
            {
                string showMesg = string.Format(GenerateInfo.CH_D005, model.OldOrderNo, model.StockCode, DateTime.Now);

                Add(model);

                LogHelper.WriteDebug(showMesg);
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);
            }

            return entity;
        }

        /// <summary>
        /// 股指期货撤单
        /// </summary>
        /// <param name="model">委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelFutureOrder(CancelEntity model)
        {
            //撮合中心委托单不能为空
            if (model == null)
            {
                return null;
            }
            var entity = new CancelResultEntity();
            //撮合中心委托单编号
            entity.OrderNo = model.OldOrderNo;
            if (TradeTimeManager.Instanse.IsAcceptTime(DateTime.Now, model.StockCode) == false)
            {
                entity.IsSuccess = false;
                entity.Message = "撤单不在交易时间之内。";
                return entity;
            }
            else
            {
                entity.IsSuccess = true;
                entity.Message = "撤单接收成功。";
            }
            //撮合中心号码不能为空
            if (model.StockCode != null)
            {
                string showMesg = "接收撤单委托单成功[" + "委托单编号=" + model.OldOrderNo + ", 委托代码=" + model.StockCode + ",接收时间=" +
                                    DateTime.Now + "]";

                FutureAdd(model);

                LogHelper.WriteDebug(showMesg);
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);

            }

            return entity;
        }

        /// <summary>
        /// 港股撤单
        /// </summary>
        /// <param name="model">委托</param>
        /// <returns></returns>
        public CancelResultEntity CancelHKOrder(CancelEntity model)
        {
            //撮合中心委托不能为空
            if (model == null)
            {
                return null;
            }

            var entity = new CancelResultEntity();
            //撮合中心委托单号码
            entity.OrderNo = model.OldOrderNo;
            if (TradeTimeManager.Instanse.IsHKAcceptTime(DateTime.Now, model.StockCode) == false)
            {
                entity.IsSuccess = false;
                entity.Message = string.Format(GenerateInfo.CH_D003, "撤单");
                return entity;
            }
            else
            {
                entity.IsSuccess = true;
                entity.Message = GenerateInfo.CH_D006;
            }
            //撮合中心代码不能为空
            if (model.StockCode != null)
            {
                string showMesg = string.Format(GenerateInfo.CH_D005, model.OldOrderNo, model.StockCode, DateTime.Now);

                HKAdd(model);

                LogHelper.WriteDebug(showMesg);
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);
            }

            return entity;
        }
        /// <summary>
        /// 港股改单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessHKModify(object sender, QueueItemHandleEventArg<HKModifyEntity> e)
        {
            HKModifyEntity entity = e.Item;
            //实体不能为空
            if (entity == null)
            {
                return;
            }
            //撮合中心证卷代码不能为空
            if (string.IsNullOrEmpty(entity.StockCode))
            {
                return;
            }
            HKStockMatcher HKmatcher = HKStockMarkers[entity.StockCode];
            if (HKmatcher != null)
            {
                ModifyAsyncResult result = new ModifyAsyncResult();
                ModifyHKOrderDelegate matchStockDelegate = HKmatcher.HKModifyOrder;

                result.ModifyObj = entity;
                result.ModifyHKD = matchStockDelegate;
                matchStockDelegate.BeginInvoke(entity, CallBackHKModifyMethod, result);

            }


        }

        /// <summary>
        /// 商品期货撤单
        /// </summary>
        /// <param name="model">委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelCommoditiesOrder(CancelEntity model)
        {
            //撮合中心委托单不能为空
            if (model == null)
            {
                return null;
            }
            var entity = new CancelResultEntity();
            //撮合中心委托单编号
            entity.OrderNo = model.OldOrderNo;
            if (TradeTimeManager.Instanse.IsAcceptTime(DateTime.Now, model.StockCode) == false)
            {
                entity.IsSuccess = false;
                entity.Message = "撤单不在交易时间之内。";
                return entity;
            }
            else
            {
                entity.IsSuccess = true;
                entity.Message = "撤单接收成功。";
            }
            //撮合中心号码不能为空
            if (model.StockCode != null)
            {
                string showMesg = "接收撤单委托单成功[" + "委托单编号=" + model.OldOrderNo + ", 委托代码=" + model.StockCode + ",接收时间=" +
                                    DateTime.Now + "]";

                CommoditiesAdd(model);

                LogHelper.WriteDebug(showMesg);
                smartPool.QueueWorkItem(ShowMessage.Instanse.ShowOfferMessage, showMesg);

            }

            return entity;
        }

        #endregion

        #region 添加撤单(包括港股改单)委托列表方法
        /// <summary>
        /// 添加委托到数据存储区
        /// </summary>
        /// <param name="dataX">委托单</param>
        private void Add(CancelEntity dataX)
        {
            //撮合中心实体不能为空
            if (CancelOrderEntity == null)
            {
                return;
            }
            lock (sryobj)
            {
                CancelOrderEntity.Add(dataX);

            }
        }
        /// <summary>
        /// 添加委托到数据存储区
        /// </summary>
        /// <param name="dataX">委托单</param>
        private void FutureAdd(CancelEntity dataX)
        {
            //撮合中心实体不能为空
            if (CancelFutureEntity == null)
            {
                return;
            }
            lock (sryobj)
            {
                CancelFutureEntity.Add(dataX);
            }
        }

        /// <summary>
        /// 添加委托到撤单缓冲区
        /// </summary>
        /// <param name="dataX"></param>
        private void HKAdd(CancelEntity dataX)
        {
            if (CancelHKOrderEntity == null)
            {
                return;
            }
            lock (sryobj)
            {
                CancelHKOrderEntity.Add(dataX);

            }

        }

        /// <summary>
        /// 添加委托到改单缓冲区
        /// </summary>
        /// <param name="dataX"></param>
        private void HKModifyAdd(HKModifyEntity dataX)
        {
            if (ModifyHKOrderEntity == null)
            {
                return;
            }
            lock (sryobj)
            {
                ModifyHKOrderEntity.Add(dataX);

            }

        }

        #region 添加商品期货委托到数据存储区 add by 董鹏 2010-01-22
        /// <summary>
        /// 添加商品期货委托到数据存储区
        /// </summary>
        /// <param name="dataX">委托单</param>
        private void CommoditiesAdd(CancelEntity dataX)
        {
            //撮合中心实体不能为空
            if (CancelCommoditiesEntity == null)
            {
                return;
            }
            lock (sryobj)
            {
                CancelCommoditiesEntity.Add(dataX);
            }
        }
        #endregion
        #endregion

        #region 每日重新开市或者重启（关闭）程序时使用
        /// <summary>
        /// 清空所有列表事件和时间定时器，这是为了每日重新开市时使用
        /// </summary>
        public void ClearAllTimerEvent()
        {

            if (stockOrderCache != null)
            {
                stockOrderCache.QueueItemProcessEvent -= ProcessBussiness;
            }
            if (CanCelTimer != null)
            {
                CanCelTimer.Elapsed -= OnTimerElapsed;
                CanCelTimer.Enabled = false;
            }
            if (bufferCancel != null)
            {
                bufferCancel.QueueItemProcessEvent -= ProcessCancel;
            }

            if (bufferFutureEntity != null)
            {
                bufferFutureEntity.QueueItemProcessEvent -= ProcessFutureBussiness;
            }

            if (CanFutureTimer != null)
            {
                CanFutureTimer.Elapsed -= OnTimeFutureElased;
                CanFutureTimer.Enabled = false;
            }
            if (bufferFutureCancel != null)
            {
                bufferFutureCancel.QueueItemProcessEvent -= ProcessFutureCancel;
            }

            if (hkOrderCache != null)
            {
                hkOrderCache.QueueItemProcessEvent -= ProcessHKBussiness;
            }
            if (CanHKTimer != null)
            {

                CanHKTimer.Elapsed -= OnTimerHKElapsed;
                CanHKTimer.Enabled = false;
            }
            if (ModifyHKTimer != null)
            {
                ModifyHKTimer.Elapsed -= OnTimerHKModifyElapsed;
                ModifyHKTimer.Enabled = false;
            }
            if (bufferHKCancel != null)
            {
                bufferHKCancel.QueueItemProcessEvent -= ProcessHKCancel;
            }
            if (bufferHKModify != null)
            {
                //增加改单关联事件
                bufferHKModify.QueueItemProcessEvent -= ProcessHKModify;
            }

            //释放所有线程资源 不能这样除非初始化改变方式
            //smartPool.Dispose();

        }

        /// <summary>
        /// 初始化撤单数据，故障恢复撤单列表，主要是为了程序在中午休市时重启程序
        /// </summary>
        /// <param name="breedClassType"></param>
        private void RecoveryCancelOrder(Types.BreedClassTypeEnum breedClassType)
        {
            List<CancelEntity> cancelList = CancelOrderRecoveryDal.GetListTodayRecoveryByBreedClassTypeID((int)breedClassType);

            #region old

            //switch (breedClassType)
            //{
            //    case Types.BreedClassTypeEnum.Stock:
            //        if (!Utils.IsNullOrEmpty(cancelList))
            //        {
            //            CancelOrderEntity.AddRange(cancelList);
            //        }
            //        break;
            //    case Types.BreedClassTypeEnum.CommodityFuture:
            //        if (!Utils.IsNullOrEmpty(cancelList))
            //        {
            //            CancelCommoditiesEntity.AddRange(cancelList);
            //        }
            //        break;
            //    case Types.BreedClassTypeEnum.StockIndexFuture:
            //        if (!Utils.IsNullOrEmpty(cancelList))
            //        {
            //            CancelFutureEntity.AddRange(cancelList);
            //        }
            //        break;
            //    case Types.BreedClassTypeEnum.HKStock:
            //        if (!Utils.IsNullOrEmpty(cancelList))
            //        {
            //            CancelHKOrderEntity.AddRange(cancelList);
            //        }
            //        break;
            //}

            #endregion

            #region 增加交易所不包含代码的过滤 add by 董鹏 2010-04-30

            RC_MatchMachine matchMachine = null;
            //获取交易所对应的撮合机
            List<RC_MatchMachine> matchMachines = CommonDataCacheProxy.Instanse.GetCacheRC_MatchMachine();
            foreach (var machine in matchMachines)
            {
                if (machine.BourseTypeID == this.bourseTypeID)
                {
                    matchMachine = machine;
                    break;
                }
            }

            if (matchMachine == null)
            {
                return;
            }

            //获取撮合机分配的代码
            List<RC_TradeCommodityAssign> commodityList = CommonDataManagerOperate.GetTradeCommodityAssignByMatchineID(matchMachine.MatchMachineID);
            if (commodityList==null || commodityList.Count == 0)
            {
                return;
            }

            //过滤不在撮合机代码分配表中的撤单记录
            List<CancelEntity> cancelListFilte = new List<CancelEntity>();
            foreach (var commodity in commodityList)
            {
                foreach (var cancelOrder in cancelList)
                {
                    if (commodity.CommodityCode.Trim() == cancelOrder.StockCode.Trim())
                    {
                        cancelListFilte.Add(cancelOrder);
                        continue;
                    }
                }
            }

            switch (breedClassType)
            {
                case Types.BreedClassTypeEnum.Stock:
                    if (!Utils.IsNullOrEmpty(cancelListFilte))
                    {
                        CancelOrderEntity.AddRange(cancelListFilte);
                    }
                    break;
                case Types.BreedClassTypeEnum.CommodityFuture:
                    if (!Utils.IsNullOrEmpty(cancelListFilte))
                    {
                        CancelCommoditiesEntity.AddRange(cancelListFilte);
                    }
                    break;
                case Types.BreedClassTypeEnum.StockIndexFuture:
                    if (!Utils.IsNullOrEmpty(cancelListFilte))
                    {
                        CancelFutureEntity.AddRange(cancelListFilte);
                    }
                    break;
                case Types.BreedClassTypeEnum.HKStock:
                    if (!Utils.IsNullOrEmpty(cancelListFilte))
                    {
                        CancelHKOrderEntity.AddRange(cancelListFilte);
                    }
                    break;
            }

            #endregion
        }

        /// <summary>
        /// 此方法只是为了在程序在中午休市中已经存
        /// </summary>
        public void SaveAllCancel()
        {
            List<CancelEntity> entities = new List<CancelEntity>();
            entities = CancelOrderEntity;
            //撮合中心实体不能为空
            if (!Utils.IsNullOrEmpty(entities))
            {
                foreach (CancelEntity entity in entities)
                {
                    CancelOrderRecoveryDal.Add(entity, (int)Types.BreedClassTypeEnum.Stock);
                }
            }
            entities = CancelFutureEntity;
            if (!Utils.IsNullOrEmpty(entities))
            {
                foreach (CancelEntity entity in entities)
                {
                    CancelOrderRecoveryDal.Add(entity, (int)Types.BreedClassTypeEnum.StockIndexFuture);
                }
            }
            entities = CancelHKOrderEntity;
            if (!Utils.IsNullOrEmpty(entities))
            {
                foreach (CancelEntity entity in entities)
                {
                    CancelOrderRecoveryDal.Add(entity, (int)Types.BreedClassTypeEnum.HKStock);
                }
            }
            entities = CancelCommoditiesEntity;
            if (!Utils.IsNullOrEmpty(entities))
            {
                foreach (CancelEntity entity in entities)
                {
                    CancelOrderRecoveryDal.Add(entity, (int)Types.BreedClassTypeEnum.CommodityFuture);
                }
            }
        }
        #endregion

    }
}