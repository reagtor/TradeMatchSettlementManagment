#region Using Namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.delegateoffer;
using ReckoningCounter.BLL.DelegateValidate.Local;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.BLL.MarketClose;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.MemoryData;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.ScheduleManagement
{
    /// <summary>
    /// 任务通知处理基类
    /// 作者：宋涛
    /// 日期：2008-11-25
    /// </summary>
    public abstract class MarketProcessJob : IDisposable
    {
        private bool doDetailLoop;
        protected ScheduleEventArgs processArgs;

        public MarketProcessJob()
            : this(true)
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="doDetailLoop">是否进行每一个商品的循环</param>
        public MarketProcessJob(bool doDetailLoop)
        {
            ScheduleManager.OnNotify += this.DoNotify;
            this.doDetailLoop = doDetailLoop;
        }

        #region IDisposable Members

        public void Dispose()
        {
            ScheduleManager.OnNotify -= this.DoNotify;
        }

        #endregion

        /// <summary>
        /// 总处理入口
        /// </summary>
        protected abstract void GlobalProcess(ScheduleEventArgs args);

        /// <summary>
        /// 每一个商品代码处理入口
        /// </summary>
        /// <param name="args">任务参数</param>
        /// <param name="commodity">商品</param>
        /// <param name="breedClassTypeEnum">品种类型名称 1.股票现货 2.商品期货 3.股指期货</param>
        protected abstract void DetailProcess(ScheduleEventArgs args, CM_Commodity commodity,
                                              Types.BreedClassTypeEnum breedClassTypeEnum);

        /// <summary>
        /// 关联到ScheduleManager的通知处理方法
        /// </summary>
        /// <param name="args"></param>
        public void DoNotify(ScheduleEventArgs args)
        {
            LogHelper.WriteDebug("---->MarketProcessJob.RecieveNotify" + args);
            this.processArgs = args;

            DoGlobalAction();

            if (this.doDetailLoop)
            {
                DoDetailLoopAction();
            }
        }

        /// <summary>
        /// 进行每一个交易品种类型的处理
        /// </summary>
        private void DoGlobalAction()
        {
            GlobalProcess(processArgs);
        }


        /// <summary>
        /// 进行每一种交易品种下的所有商品的循环
        /// </summary>
        private void DoDetailLoopAction()
        {
            IList<CM_BreedClass> breedClasses = MCService.CommonPara.GetAllBreedClass();

            foreach (CM_BreedClass breedClass in breedClasses)
            {
                Types.BreedClassTypeEnum breedClassTypeEnum = (Types.BreedClassTypeEnum)breedClass.BreedClassTypeID;

                IList<CM_Commodity> commodities =
                    MCService.CommonPara.GetAllCommodityByBreedClass(breedClass.BreedClassID);

                foreach (CM_Commodity commodity in commodities)
                {
                    DetailProcess(this.processArgs, commodity, breedClassTypeEnum);
                }
            }
        }
    }

    #region 现货(包括港股)开收市事操作

    #region 现货(包括港股)开市处理操作类
    /// <summary>
    /// 现货(包括港股)【开市】处理操作类
    /// </summary>
    public class StockOpenProcessJob : MarketProcessJob
    {
        //不进行DetailProcess处理
        public StockOpenProcessJob()
            : base(false)
        {
        }

        protected override void GlobalProcess(ScheduleEventArgs args)
        {
            if (args.TimeType != Entity.Contants.Types.TradingTimeType.MacthBeginWork)
            {
                return;
            }
            try
            {
                StockOpenCloseProcess.DoOpen();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("现货开市处理异常", ex);
            }
        }

        protected override void DetailProcess(ScheduleEventArgs args, CM_Commodity commodity,
                                              Types.BreedClassTypeEnum breedClassTypeEnum)
        {
        }
    }
    #endregion

    #region 现货分红  Crate 2010-06-11 李健华

    /// <summary>
    /// 现货分红 
    /// //解决问题：分红持仓用户分红当日排名虚高
    ///问题原因：后台分红时间为登记日当日（除权除息日前1日）收盘清算时，即保证在除权除息日开市前分红完毕，
    ///于收市清算时进行减少柜台的运行次数。但登记日当日收盘价还是未除权除息价，
    ///清算后把现金分红和股票分红过户，会导致前台显示的该用户股票市值虚增，
    ///当日排名偏高，除权除息日开市后，行情价格调整为除权除息价，
    ///用户市值即恢复正常，第二日排名也会恢复正常。

    ///解决方法：修改柜台分红执行时间。判断用户在股票登记日结束前就开始持有分红股票，
    ///柜台给用户转入现金分红金额以及股票分红数量时间为除权除息日最早的交易所开市前一小时
    /// ，且分红记录主动回推到前台。
    /// </summary>
    public class StockMelonCuttingJob : MarketProcessJob
    {
        //不进行DetailProcess处理
        public StockMelonCuttingJob()
            : base(false)
        {
        }
        protected override void GlobalProcess(ScheduleEventArgs args)
        {
            if (args.TimeType != Entity.Contants.Types.TradingTimeType.Open)
            {
                return;
            }
            try
            {
                //1.分红之前提交内存数据
                MemoryDataManager.End();

                StockOpenCloseProcess.DoMelonCutting();


            }
            catch (Exception ex)
            {
                LogHelper.WriteError("现货分红处理异常", ex);
            }
            try
            {
                //重新加载一次
                MemoryDataManager.Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("现货分红加载内存异常", ex);
            }
            //=============
        }

        protected override void DetailProcess(ScheduleEventArgs args, CM_Commodity commodity, Types.BreedClassTypeEnum breedClassTypeEnum)
        {

        }
    }
    #endregion

    #region 现货(包括港股)收市处理操作类
    /// <summary>
    /// 现货(包括港股)【收市】处理操作类
    /// </summary>
    public class StockCloseProcessJob : MarketProcessJob
    {
        //不进行DetailProcess处理
        public StockCloseProcessJob()
            : base(false)
        {
        }

        protected override void GlobalProcess(ScheduleEventArgs args)
        {
            if (args.TimeType != Entity.Contants.Types.TradingTimeType.MatchEndWork)
                return;

            //update 李健华 2009-12-15=====因为有关内存表的数据，所以在这里盘后清算时不作多线程操作
            //Thread stockCloseThread = new Thread(StockOpenCloseProcess.DoClose);
            //stockCloseThread.Start();
            StockOpenCloseProcess.DoClose();
            //=============
        }

        protected override void DetailProcess(ScheduleEventArgs args, CM_Commodity commodity,
                                              Types.BreedClassTypeEnum breedClassTypeEnum)
        {
        }
    }
    #endregion

    #region 现货(包括港股)收开市处理
    /// <summary>
    ///现货(包括港股)收开市处理
    /// </summary>
    public class StockOpenCloseProcess
    {
        public static void DoOpen()
        {
            LogHelper.WriteInfo("执行现货开市动作DoOpen");
            try
            {
                //关闭内存表管理器
                if (ScheduleManager.CanEndCache())
                {
                    MemoryDataManager.End();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("现货开市DoOpen处理异常", ex);
            }
            DoBaseData();

            //DoCacheOrder();

            ScheduleManager.HasDoneStockOpen = true;
            ScheduleManager.HasDoneOpenNotify();
        }

        /// <summary>
        /// 如果过了开市时间，那么立刻将缓存中的委托送出
        /// </summary>
        private static void DoCacheOrder()
        {
            //DateTime now = DateTime.Now;
            //if (Utils.CompareTime(ScheduleManager.FirstBeginTime, now) <= 0 &&
            //    Utils.CompareTime(ScheduleManager.LastEndTime, now) >= 0)
            //{
            //    LogHelper.WriteInfo("StockOpenCloseProcess.DoCacheOrder柜台启动时开市时间已过，立刻将缓存中的委托送出");
            //    OrderOfferCenter.Instance._orderCache.SendCache();
            //}

            LogHelper.WriteInfo("StockOpenCloseProcess.DoCacheOrder柜台启动时开市时间已过，立刻将缓存中的委托送出");
            OrderOfferCenter.Instance._orderCache.SendCache();
        }

        public static void DoClose()
        {
            //还没到收市，不处理
            if (DateTime.Now.Hour < ScheduleManager.LastEndTime.Hour)
                return;

            if (DateTime.Now.Hour == ScheduleManager.LastEndTime.Hour)
            {
                if (DateTime.Now.Minute < ScheduleManager.LastEndTime.Minute)
                    return;
            }

            #region update 2009-12-16 不在这里操作内存资金提交加载，在做完撤单等操作后
            //收市处理时在不这里马上提交内存的资金表，因为这里如果提交了，在后面还有要预下单再做撤单
            //时就会用到内存表的资金这样就无法同步,所以这里应该在做完撤单操作后延时等待撤单都做完时再提交
            //内存表资金

            ////过了收市时间，开始处理

            ////先关闭内存管理器
            //MemoryDataManager.End();

            ////add 李健华   2009-12-14 
            ////可能因为有预委托下单，后面清算时还要用到内存表的作内部撤单操作，所以这里先提交一次再打开
            ////内存表管理
            //MemoryDataManager.Start();
            //=====================
            #endregion

            LogHelper.WriteInfo("执行现货收市动作DoClose");

            //清算之前把所有柜台缓存的清算撮合ID的数据清除
            XHCounterCache.Instance.ResetDictionary();
            HKCounterCache.Instance.ResetDictionary();

            InternalDoClose();

            ScheduleManager.HasDoneStockReckoning = true;
            ScheduleManager.ReckoningDoneNotify();
        }

        private static void InternalDoClose()
        {
            //记录开始清算
            ScheduleManager.IsStockReckoning = true;

            bool isSuccess = DoReckoning();

            if (isSuccess)
            {
                #region update 李健华 2010-06-12  分红登记
                //isSuccess = DoMelonCutting();  
                isSuccess = MelonCutService.RegisterMeloncutTable();
                #endregion
                if (isSuccess)
                {
                    DoHistoryDataProcess();
                }
            }
            //这里分红处理后在这里就加载内存表
            ////清算完毕后再重新加载内存资金表内容,为后面其他清算做预委托下单的处理同样操作内存表的数据
            ////内存表管理
            MemoryDataManager.Start();

            ScheduleManager.IsStockReckoning = false;

            ScheduleManager.IsHKReckoning = true;
            isSuccess = DoHKReckoning();
            if (isSuccess)
            {
                DoHKHistoryDataProcess();
            }
            //这里港股完成后也重新加载一个，因为在内部清算的时候已经全过一交提交
            MemoryDataManager.Start();
            ScheduleManager.IsHKReckoning = false;
        }

        #region OpenProcess

        /// <summary>
        /// 读取交易规则及基础数据
        /// </summary>
        private static void DoBaseData()
        {
            //已经在ScheduleManger中初始化
            //MCService.CommonPara.Reset();
            //MCService.CommonPara.Initialize();

            LogHelper.WriteInfo("------------证券开市处理-DoBaseData");
            try
            {
                MCService.Reset();

                AccountManager.Instance.Reset();

                #region  old code  注释 --李健华 2010-06-09
                ////交易员工厂封装类进行重置
                //VTTradersFactory.Reset();
                //VTTradersFactory.GetStockTraders();
                //VTTradersFactory.GetFutureTraders();
                #endregion
                RealTimeMarketUtil.GetInstance().Reset();
                LocalCommonValidater.Reset();

                #region ===回推故障数据导入 Create by:李健华 Create date:2009-08-12===

                FailureRecoveryFactory.XHReaderToDB();

                #endregion ===回推故障数据导入 End===

                var rescueManager = RescueManager.Instance;

                LogHelper.WriteInfo("------------完成证券开市处理-DoBaseData");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------证券开市处理失败-DoBaseData");
            }
        }

        #endregion

        #region CloseProcess

        /// <summary>
        /// 分红处理
        /// </summary>
        public static bool DoMelonCutting()
        {
            if (StatusTableChecker.HasDoneMelonCut(DateTime.Now))
                return true;

            bool result = false;
            LogHelper.WriteInfo("------------开始证券收市处理-DoMelonCutting[分红处理]");

            try
            {
                result = MelonCutService.Process();
                if (!result)
                {
                    LogHelper.WriteInfo("***证券收市处理-DoMelonCutting[分红处理]第1次失败!进行第2次处理");
                    result = MelonCutService.Process();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***证券收市处理-DoMelonCutting[分红处理]第2次失败!进行第3次处理");
                    result = MelonCutService.Process();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***证券收市处理-DoMelonCutting[分红处理]第3次失败!退出");
                }
                else
                {
                    LogHelper.WriteInfo("------------完成证券收市处理-DoMelonCutting[分红处理]");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------证券收市处理失败-DoMelonCutting[分红处理]");
            }

            return result;
        }

        /// <summary>
        /// 历史数据处理 
        /// </summary>
        private static void DoHistoryDataProcess()
        {
            if (StatusTableChecker.HasDoneStockHistoryDataProcess(DateTime.Now))
                return;

            LogHelper.WriteInfo("------------开始证券收市处理-DoHistoryDataProcess[历史数据处理]");

            try
            {
                HistoryDataService.ProcessStock();
                LogHelper.WriteInfo("------------完成证券收市处理-DoHistoryDataProcess[历史数据处理]");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------证券收市处理失败-DoHistoryDataProcess[历史数据处理]");
            }
        }

        /// <summary>
        /// 港股历史数据处理 
        /// </summary>
        private static void DoHKHistoryDataProcess()
        {
            if (StatusTableChecker.HasDoneHKHistoryDataProcess(DateTime.Now))
                return;

            LogHelper.WriteInfo("------------开始港股收市处理-DoHKHistoryDataProcess[历史数据处理]");

            try
            {
                HistoryDataService.ProcessHK();
                LogHelper.WriteInfo("------------完成港股收市处理-DoHKHistoryDataProcess[历史数据处理]");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------港股收市处理失败-DoHKHistoryDataProcess[历史数据处理]");
            }
        }

        /// <summary>
        /// 证券盘后清算
        /// </summary>
        private static bool DoReckoning()
        {
            if (StatusTableChecker.HasDoneStockReckoning(DateTime.Now))
                return true;

            LogHelper.WriteInfo("------------开始证券收市处理-DoReckoning[证券盘后清算]");

            bool result = false;
            try
            {
                result = ReckoningService.ProcessStock();

                if (!result)
                {
                    LogHelper.WriteInfo("***证券收市处理-DoReckoning[证券盘后清算]第1次失败!进行第2次处理");
                    result = ReckoningService.ProcessStock();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***证券收市处理-DoReckoning[证券盘后清算]第2次失败!进行第3次处理");
                    result = ReckoningService.ProcessStock();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***证券收市处理-DoReckoning[证券盘后清算]第3次失败!!退出");
                }
                else
                {
                    LogHelper.WriteInfo("------------完成证券收市处理-DoReckoning[证券盘后清算]");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------证券收市处理失败-DoReckoning[证券盘后清算]");
            }

            return result;
        }

        /// <summary>
        /// 港股盘后清算
        /// </summary>
        private static bool DoHKReckoning()
        {
            if (StatusTableChecker.HasDoneHKReckoning(DateTime.Now))
                return true;

            LogHelper.WriteInfo("------------开始港股收市处理-DoHKReckoning[港股盘后清算]");

            bool result = false;
            try
            {
                result = ReckoningService.ProcessHK();

                if (!result)
                {
                    LogHelper.WriteInfo("***港股收市处理-DoHKReckoning[港股盘后清算]第1次失败!进行第2次处理");
                    result = ReckoningService.ProcessHK();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***港股收市处理-DoHKReckoning[港股盘后清算]第2次失败!进行第3次处理");
                    result = ReckoningService.ProcessHK();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***港股收市处理-DoHKReckoning[港股盘后清算]第3次失败!!退出");
                }
                else
                {
                    LogHelper.WriteInfo("------------完成港股收市处理-DoHKReckoning[港股盘后清算]");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------证券收市处理失败-DoHKReckoning[证券盘后清算]");
            }

            return result;
        }

        #endregion
    }
    #endregion

    #endregion

    #region  期货开收市处理
    /// <summary>
    /// 期货【开市】处理
    /// </summary>
    public class FutureOpenProcessJob : MarketProcessJob
    {
        //不进行DetailProcess处理
        public FutureOpenProcessJob()
            : base(false)
        {
        }

        protected override void GlobalProcess(ScheduleEventArgs args)
        {
            if (args.TimeType != Entity.Contants.Types.TradingTimeType.MacthBeginWork)
            {
                return;
            }

            try
            {
                FutureOpenCloseProcess.DoOpen();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("期货开市处理异常", ex);
            }
        }

        protected override void DetailProcess(ScheduleEventArgs args, CM_Commodity commodity,
                                              Types.BreedClassTypeEnum breedClassTypeEnum)
        {
        }
    }

    /// <summary>
    /// 期货【收市】处理
    /// </summary>
    public class FutureCloseProcessJob : MarketProcessJob
    {
        //不进行DetailProcess处理
        public FutureCloseProcessJob()
            : base(false)
        {
        }

        protected override void GlobalProcess(ScheduleEventArgs args)
        {
            if (args.TimeType != Entity.Contants.Types.TradingTimeType.MatchEndWork)
                return;
            //update 李健华 2009-12-15=====因为有关内存表的数据，所以在这里盘后清算时不作多线程操作
            //Thread futureCloseThread = new Thread(FutureOpenCloseProcess.DoClose);
            //futureCloseThread.Start();
            FutureOpenCloseProcess.DoClose();
            //=========
        }

        protected override void DetailProcess(ScheduleEventArgs args, CM_Commodity commodity,
                                              Types.BreedClassTypeEnum breedClassTypeEnum)
        {
        }
    }

    /// <summary>
    /// 执行期货开收市动作
    /// </summary>
    public class FutureOpenCloseProcess
    {
        public static void DoOpen()
        {
            LogHelper.WriteInfo("执行期货开市动作DoOpen");

            DoBaseData();

            try
            {
                //还没到开市，不处理
                if (CanDoOpen())
                {
                    if (ScheduleManager.CanEndCache())
                    {
                        MemoryDataManager.End();
                    }
                    DoFutureDayCheck();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("期货开市DoOpen处理异常", ex);
            }
            //add 2010-03-16 添加对期货的清算判断，如果发现前一日没有清算完成则设置期货是否清算异常系统暂停交易，使用期台不能下单
            string errMsg = "";
            DateTime ReckoningDateTime;
            bool isReckoning = false;
            isReckoning = StatusTableChecker.IsFutureReckoningFaultRecovery(out ReckoningDateTime, out errMsg);
            if (isReckoning)
            {
                IList<CM_BreedClass> list = MCService.CommonPara.GetAllBreedClass();
                foreach (var item in list)
                {
                    if (item.BreedClassTypeID.Value == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture ||
                        item.BreedClassTypeID.Value == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture)
                    {
                        ScheduleManager.IsFutureReckoningErrorStopTrade = true;
                        break;
                    }
                }

            }
            //else
            //{
            //    ScheduleManager.IsFutureReckoningErrorStopTrade = false;
            //}
            //==================


            ScheduleManager.HasDoneFutureOpen = true;
            ScheduleManager.HasDoneOpenNotify();
        }

        private static bool CanDoOpen()
        {
            if (DateTime.Now.Hour < ScheduleManager.FirstBeginTime.Hour)
                return false;

            if (DateTime.Now.Hour == ScheduleManager.FirstBeginTime.Hour)
            {
                if (DateTime.Now.Minute < ScheduleManager.FirstBeginTime.Minute)
                    return false;
            }

            return true;
        }

        public static void DoClose()
        {
            //还没到收市，不处理
            if (DateTime.Now.Hour < ScheduleManager.LastEndTime.Hour)
                return;

            if (DateTime.Now.Hour == ScheduleManager.LastEndTime.Hour)
            {
                if (DateTime.Now.Minute < ScheduleManager.LastEndTime.Minute)
                    return;
            }

            LogHelper.WriteInfo("执行期货收市动作DoClose");

            //===update 李健华  2009-12-14
            //这里已经在现货清算中提交过，也再得新加载过，在清算完成后会统一再提交一次
            //先关闭内存管理器
            // MemoryDataManager.End();
            //==========

            MCService.DoMarketCloseJob();

            //add 2010-03-16  
            //1.发现前一日没有清算完成今日也不能清算 (这个不用了，系统自动清算只要把当日或者前一日的清算完成即可，系统只要保证没有清算完成不可下单操作即可）
            //2.可以清算时，先初始化所有的今日结算价
            //3.检查当前持仓表中所有持仓合约都能获取得到结算价时再执行清算
            //===============
            //1.

            string errMsg = "";
            bool isReck = false;
            DateTime recktime;
            isReck = StatusTableChecker.IsFutureTodayReckoning(out recktime, out errMsg);
            //if (!isReck)
            //{
            //    LogHelper.WriteError("今日期货清算不能执行清算，时间获取为：" + recktime, new Exception(errMsg));
            //    return;
            //}
            //2.

            //3.
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            List<QH_HoldAccountTableInfo> models = dal.GetAllListArray();
            decimal price;
            foreach (var item in models)
            {
                //如果持仓量有就要获取，如果都已经没有了持仓量证明已经清算完毕不再再清算
                if (item.HistoryFreezeAmount + item.HistoryHoldAmount + item.TodayFreezeAmount + item.TodayHoldAmount > 0)
                {

                    if (!MCService.GetFutureTodayPreSettlementPriceByCache(item.Contract, out price))
                    {
                        //如果代码还是可以交易的代码则中止清算，已经过期忽略不理，内部清算的时候获取不到会不会修改持仓均价相关的内容
                        if (!MCService.IsExpireLastedTradeDate(item.Contract))
                        {
                            //但当代码当日为非交易日时也可以放过
                            //如果发现前一日没有清算完成今日获取不到价格不能放过
                            if (!isReck || MCService.CommonPara.IsTradeDate(item.Contract, DateTime.Now))
                            {
                                //为了防止每日重启程序时时间已经过了收市时间即到了清算时间所作的每次清算而已经清算成功的再设置为不清算成功
                                //如：20100402的五点时重启程序，这时已经四点半已经清算成功，那些这里再清算而又获取不到行情就会设置错误，而这里应该不设置，放过
                                //因为内部还会检查是否清算成功过
                                if (!StatusTableChecker.HasDoneFutureReckoning(DateTime.Now))
                                {

                                    LogHelper.WriteError("今日期货清算获取今日结算价无法获取：" + item.Contract, new Exception("清算中止"));
                                    //暂停交易
                                    ScheduleManager.IsFutureReckoningErrorStopTrade = true;
                                    return;
                                }
                            }
                            ////如果发现前一日没有清算完成今日获取不到价格不能放过
                            //else if (!isReck)
                            //{
                            //    LogHelper.WriteError("今日期货清算获取今日结算价无法获取：" + item.Contract, new Exception("前一日清算异常，今日清算中止"));
                            //    //暂停交易
                            //    ScheduleManager.IsFutureReckoningErrorStopTrade = true;
                            //    return;

                            //}

                        }
                    }
                }
            }
            //清算之前把所有柜台缓存的清算撮合ID的数据清除
            QHCounterCache.Instance.ResetDictionary();

            InternalDoClose();

            ScheduleManager.HasDoneFutureReckoning = true;
            ScheduleManager.ReckoningDoneNotify();

            ////过了收市时间，开始处理，延时5分钟，等现货做完后再做
            //timer = new Timer();
            //timer.Interval = 1*60*1000;
            //timer.Elapsed += delegate
            //                     {
            //                         timer.Enabled = false;
            //                         LogHelper.WriteInfo("执行期货收市动作DoClose");
            //                         InternalDoClose();
            //                         InternalDoClose();
            //                         InternalDoClose();
            //                     };
            //timer.Enabled = true;
            //期货清算完成后把资金管理的相关账号清除，释放内存
            AccountManager.Instance.Reset();
        }

        /// <summary>
        /// 用于故障恢复清算
        /// </summary>
        /// <param name="recktime">要清算的日期</param>
        public static void DoFaultRecoveryClose(string recktime)
        {

            //add 2010-03-16  
            //2.可以清算时，先初始化所有的今日结算价
            //3.检查当前持仓表中所有持仓合约都能获取得到结算价时再执行清算

            //2.
            MCService.IniFutureTodayPreSettlementPriceRecovery();
            //3.
            QH_HoldAccountTableDal dal = new QH_HoldAccountTableDal();
            List<QH_HoldAccountTableInfo> models = dal.GetAllListArray();
            decimal price;

            foreach (var item in models)
            {
                //如果持仓量有就要获取，如果都已经没有了持仓量证明已经清算完毕不再再清算
                if (item.HistoryFreezeAmount + item.HistoryHoldAmount + item.TodayFreezeAmount + item.TodayHoldAmount > 0)
                {
                    if (!MCService.GetFutureTodayPreSettlementPriceByCache(item.Contract, out price))
                    {
                        LogHelper.WriteError("故障恢复期货清算获取今日结算价无法获取：" + item.Contract, new Exception("清算中止"));
                        return;
                    }
                }
            }
            //把是否正在做故障恢复清算状态修改回来为true,正在进行
            ScheduleManager.IsFaultRecoveryFutureReckoning = true;
            ScheduleManager.CurrentFaultRecoveryFutureReckoningDate = DateTime.Parse(recktime);
            InternalDoClose();

            //现货也设置清算完成，为了后面能提交现货的相关数据内存表中的数据
            ScheduleManager.HasDoneStockReckoning = true;
            ScheduleManager.HasDoneFutureReckoning = true;

            ScheduleManager.ReckoningDoneNotify();

            //证明已经故障恢复清算已经成功那么更新清算记录表记录
            if (ScheduleManager.IsFutureReckoningErrorStopTrade == false)
            {
                //故障恢复清算完成修改清算日期标志，因为内部之前 的方法直接修改为当前的了，这里要修改回当招提交的数据日期
                recktime = DateTime.Parse(recktime).ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss");
                StatusTableChecker.UpdateFutureReckoningDate(recktime, null);
            }
            //清空当前所有的缓存当前清算时添加的缓存数据
            MCService.ClearFuterTodayPreSettlemmentPrice();

            //把是否正在做故障恢复清算状态修改回来为false
            ScheduleManager.IsFaultRecoveryFutureReckoning = false;
        }

        private static void InternalDoClose()
        {
            //记录开始清算
            ScheduleManager.IsFutureReckoning = true;

            bool isSuccess = DoReckoning();

            if (isSuccess)
            {
                DoHistoryDataProcess();
                //清算成功后把是否暂停交易设置为不再暂停交易，已经清算成功
                ScheduleManager.IsFutureReckoningErrorStopTrade = false;
            }
            else
            {
                IList<CM_BreedClass> list = MCService.CommonPara.GetAllBreedClass();
                foreach (var item in list)
                {
                    if (item.BreedClassTypeID.Value == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture ||
                        item.BreedClassTypeID.Value == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture)
                    {
                        //清算不成功设置为暂停交易
                        ScheduleManager.IsFutureReckoningErrorStopTrade = true;
                        break;
                    }
                }

            }

            ScheduleManager.IsFutureReckoning = false;
        }

        #region OpenProcess

        /// <summary>
        /// 读取交易规则及基础数据
        /// </summary>
        private static void DoBaseData()
        {
            LogHelper.WriteInfo("------------期货开市处理-DoBaseData");

            try
            {
                MCService.FuturesTradeRules.Reset();
                MCService.FuturesTradeRules.Initialize();

                #region ===回推故障数据导入 Create by:李健华 Create date:2009-08-12===

                FailureRecoveryFactory.QHReaderToDB();

                #endregion ===回推故障数据导入 End===

                LogHelper.WriteInfo("------------完成期货开市处理-DoBaseData");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------期货开市处理失败-DoBaseData");
            }
        }

        /// <summary>
        /// 进行期货每日开盘检查
        /// </summary>
        private static void DoFutureDayCheck()
        {
            if (StatusTableChecker.HasDoneFutureDayCheck(DateTime.Now))
                return;

            LogHelper.WriteInfo("------------期货开市处理-DoFutureDayCheck[进行期货每日开盘检查]");

            try
            {
                FutureDayChecker checker = new FutureDayChecker();
                checker.Process();
                LogHelper.WriteInfo("------------完成期货开市处理-DoFutureDayCheck[进行期货每日开盘检查]");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------期货开市处理失败-DoFutureDayCheck[进行期货每日开盘检查]");
            }
        }

        #endregion

        #region CloseProcess

        /// <summary>
        /// 历史数据处理 
        /// </summary>
        public static void DoHistoryDataProcess()
        {
            if (StatusTableChecker.HasDoneFutureHistoryDataProcess(DateTime.Now))
                return;

            LogHelper.WriteInfo("------------开始期货收市处理-DoHistoryDataProcess[历史数据处理]");

            try
            {
                HistoryDataService.ProcessFuture();
                HistoryDataService.ProcessUnReckonedTable();
                LogHelper.WriteInfo("------------完成期货收市处理-DoHistoryDataProcess[历史数据处理]");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------期货收市处理失败-DoHistoryDataProcess[历史数据处理]");
            }
        }

        /// <summary>
        /// 期货盘后清算
        /// </summary>
        public static bool DoReckoning()
        {
            if (StatusTableChecker.HasDoneFutureReckoning(DateTime.Now))
                return true;

            LogHelper.WriteInfo("------------开始期货收市处理-DoReckoning[期货盘后清算]");

            bool result = false;
            try
            {
                result = ReckoningService.ProcessFuture();
                if (!result)
                {
                    LogHelper.WriteInfo("***期货收市处理-DoReckoning[期货盘后清算]第1次失败!进行第2次处理");
                    result = ReckoningService.ProcessFuture();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***期货收市处理-DoReckoning[期货盘后清算]第2次失败!进行第3次处理");
                    result = ReckoningService.ProcessFuture();
                }

                if (!result)
                {
                    LogHelper.WriteInfo("***期货收市处理-DoReckoning[期货盘后清算]第3次失败!!退出");
                }
                else
                {
                    LogHelper.WriteInfo("------------完成期货收市处理-DoReckoning[期货盘后清算]");
                }

                LogHelper.WriteInfo("------------完成期货收市处理-DoReckoning[期货盘后清算]");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                LogHelper.WriteInfo("------------期货收市处理失败-DoReckoning[期货盘后清算]");
            }

            return result;
        }

        #endregion
    }
    #endregion


}