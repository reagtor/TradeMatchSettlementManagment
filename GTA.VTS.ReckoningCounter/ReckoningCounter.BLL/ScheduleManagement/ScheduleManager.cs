#region Using Namespace

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using GTA.VTS.Common.CommonUtility;
using Quartz;
using Quartz.Impl;
using ReckoningCounter.BLL.delegateoffer;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.MemoryData;
using Timer = System.Timers.Timer;

#endregion

namespace ReckoningCounter.BLL.ScheduleManagement
{
    /// <summary>
    /// 任务调度管理类
    /// 作者：宋涛
    /// 日期：2008-11-25
    /// </summary>
    public static class ScheduleManager
    {
        private const string SCHEDULE_ARGS = "extArgs";
        /// <summary>
        /// 开市的最先时间
        /// </summary>
        public static DateTime FirstBeginTime = new DateTime(2000, 1, 1, 0, 0, 0);
        private static FutureCloseProcessJob futureCloseJob;
        private static FutureOpenProcessJob futureOpenJob;
        /// <summary>
        /// 是否开启成功
        /// </summary>
        public static bool IsStartSuccess;
        /// <summary>
        /// 最后收市时间
        /// </summary>
        public static DateTime LastEndTime = new DateTime(2000, 1, 1, 0, 0, 0);
        private static HashSet<string> matchTimes = new HashSet<string>();
        private static IScheduler scheduler;
        /// <summary>
        /// 
        /// </summary>
        private static StockCloseProcessJob stockCloseJob;
        private static StockOpenProcessJob stockOpenJob;

        private static StockMelonCuttingJob stockMelonCutting;

        /// <summary>
        /// 现货是否在清算
        /// </summary>
        public static bool IsStockReckoning { get; set; }

        /// <summary>
        /// 港股是否在清算
        /// </summary>
        public static bool IsHKReckoning { get; set; }

        /// <summary>
        /// 期货是否在清算
        /// </summary>
        public static bool IsFutureReckoning { get; set; }

        /// <summary>
        /// 期货是否正在做故障恢复清算
        /// </summary>
        public static bool IsFaultRecoveryFutureReckoning { get; set; }

        /// <summary>
        /// 期货是否清算异常系统暂停交易
        /// </summary>
        public static bool IsFutureReckoningErrorStopTrade { get; set; }

        /// <summary>
        /// 当前正在做期货故障恢复清算的日期
        /// </summary>
        public static DateTime CurrentFaultRecoveryFutureReckoningDate { get; set; }


        internal static bool HasDoneFutureReckoning;
        internal static bool HasDoneStockReckoning;

        /// <summary>
        /// 现货正在开市初始化
        /// </summary>
        public static bool XHIniting { get; set; }
        /// <summary>
        /// 期货正在开市初始化 
        /// </summary>
        public static bool QHIniting { get; set; }
        /// <summary>
        /// 完成清算后操作内存表和更新清算当前的清算状态
        /// </summary>
        public static void ReckoningDoneNotify()
        {
            if (HasDoneFutureReckoning && HasDoneStockReckoning)
            {
                //清算完成
                LogHelper.WriteInfo("清算完成，启动内存表管理器");
                try
                {
                    //先提交再开启
                    MemoryDataManager.End();

                    MemoryDataManager.Start();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("清算完成，启动管理器异常", ex);
                }
                HasDoneStockReckoning = false;
                HasDoneFutureReckoning = false;
            }
        }

        internal static bool HasDoneStockOpen { get; set; }
        internal static bool HasDoneFutureOpen { get; set; }
        /// <summary>
        /// 是否每天开市时执行过内存数据初始化过
        /// </summary>
        internal static bool IsEverydayOpenCommitMemoryData { get; set; }
        /// <summary>
        ///  开市完成后更新清算状态设置为false以及加载内存表数据
        /// </summary>
        public static void HasDoneOpenNotify()
        {
            if (HasDoneStockOpen && HasDoneFutureOpen)
            {
                //开盘动作完成
                LogHelper.WriteInfo("开盘动作完成，启动内存表管理器");
                try
                {
                    MemoryDataManager.Start();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("开盘动作完成，启动管理器异常", ex);
                }
                HasDoneStockReckoning = false;
                HasDoneFutureReckoning = false;
            }
        }
        /// <summary>
        /// 任务触发通知时间
        /// </summary>
        public static event Action<ScheduleEventArgs> OnNotify;

        private static void DoNotify(ScheduleEventArgs args)
        {
            if (OnNotify != null)
            {
                LogHelper.WriteDebug("DoNotify" + args);
                OnNotify(args);
            }
        }

        /// <summary>
        /// 开始启动，进行初始化
        /// </summary>
        public static bool Start(ref string errMsg)
        {
            if (scheduler != null)
            {
                if (scheduler.IsStarted)
                    return true;
            }

            //DaoUtil.Initialize();
            //删除正常退出标志
            //DaoUtil.DeleteNormalFlag();
            bool isConnectionOk = DaoUtil.TestConnection();
            if (!isConnectionOk)
            {
                errMsg = "无法连接数据库，请检查配置文件！";
                LogHelper.WriteDebug("ScheduleManager.Start" + errMsg);
                return false;
            }

            stockOpenJob = new StockOpenProcessJob();
            stockCloseJob = new StockCloseProcessJob();
            futureOpenJob = new FutureOpenProcessJob();
            futureCloseJob = new FutureCloseProcessJob();

            stockMelonCutting = new StockMelonCuttingJob();
            try
            {
                //启动内存表管理器
                //MemoryDataManager.Start();

                //===回推故障数据导入 Start===
                FailureRecoveryFactory.QHReaderToDB();
                FailureRecoveryFactory.XHReaderToDB();
                FailureRecoveryFactory.HKReaderToDB();
                //===回推故障数据导入 End===

                ISchedulerFactory schedFact = new StdSchedulerFactory();
                scheduler = schedFact.GetScheduler();

                MCService.CommonPara.Reset();
                MCService.CommonPara.Initialize();

                InitializeTriggers();

                LogHelper.WriteInfo("ScheduleManager.Start");
                scheduler.Start();

                DoOpenProcess();
                DoCloseProcess();

                #region 已经在DoOpenProcess中期货中有执行不再处理
                ////add 2010-03-16 添加对期货的清算判断，如果发现前一日没有清算完成则设置期货是否清算异常系统暂停交易，使用期台不能下单
                //DateTime ReckoningDateTime;
                //bool isReckoning = false;
                //isReckoning = StatusTableChecker.IsFutureReckoningFaultRecovery(out ReckoningDateTime, out errMsg);
                //if (isReckoning)
                //{
                //    IList<CM_BreedClass> list = MCService.CommonPara.GetAllBreedClass();
                //    foreach (var item in list)
                //    {
                //        if (item.BreedClassTypeID.Value == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture ||
                //            item.BreedClassTypeID.Value == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture)
                //        {
                //            IsFutureReckoningErrorStopTrade = true;
                //            break;
                //        }
                //    }
                //}
                #endregion
                //==================

                IsStartSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfo("ScheduleManger start failure!");
                LogHelper.WriteError(ex.ToString(), ex);

                IsStartSuccess = false;
                return false;
            }

            LogHelper.WriteInfo("ScheduleManger start success!");
            return true;
        }

        private static void DoOpenProcess()
        {
            InternalDoOpen();
            //Thread thread = new Thread(InternalDoOpen);
            //thread.Start();
        }

        private static void InternalDoOpen()
        {
            StockOpenCloseProcess.DoOpen();
            FutureOpenCloseProcess.DoOpen();

            DoCacheOrder();
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




        /// <summary>
        /// 关闭调度管理
        /// </summary>
        public static void Shutdown()
        {
            if (scheduler != null)
            {
                if (scheduler.IsShutdown)
                    return;

                LogHelper.WriteInfo("ScheduleManager.Close");
                scheduler.Shutdown();
            }



            if (IsStartSuccess)
            {
                //DoCloseProcess();
                //add 李健华 这里是为了调用关闭程序时调用后面闭市清算操作不用做延时操作
                IsStartSuccess = false;
                InternalDoClose();
            }
            //因为清算时会先提交内存，所以放在最后再提交（如果不是清算时间的）
            //关闭内存管理器
            MemoryDataManager.End();

            //添加正常退出标志
            //DaoUtil.AddNormalFlag();

            OrderOfferCenter.Instance.DoClose();
        }



        private static void DoCloseProcess()
        {
            Timer timer = new Timer();
            timer.Interval = 1 * 60 * 1000;
            timer.Elapsed += delegate
                                 {
                                     Thread thread = new Thread(InternalDoClose);
                                     thread.Start();
                                     timer.Enabled = false;
                                 };

            timer.Enabled = true;
        }

        private static void InternalDoClose()
        {
            StockOpenCloseProcess.DoClose();
            FutureOpenCloseProcess.DoClose();
        }

        /// <summary>
        /// 初始化触发器
        /// </summary>
        private static void InitializeTriggers()
        {
            IList<CM_BourseType> bourseTypes = null;

            bourseTypes = MCService.CommonPara.GetAllBourseType();

            foreach (CM_BourseType bourseType in bourseTypes)
            {
                if (bourseType.ReceivingConsignStartTime.HasValue)
                {
                    // 撮合接收委托开始时间
                    DateTime matchBeginTime = bourseType.ReceivingConsignStartTime.Value;
                    //SetTrigger(bourseType, matchBeginTime, Types.TradingTimeType.MacthBeginWork, "matchBeginTime");
                    //SetTrigger(matchBeginTime, Types.TradingTimeType.MacthBeginWork, "Begin-" + bourseType.BourseTypeName);

                    SetBeginTime(matchBeginTime, bourseType, Types.TradingTimeType.MacthBeginWork);
                }

                if (bourseType.ReceivingConsignEndTime.HasValue)
                {
                    // 撮合接收委托结束时间
                    DateTime matchEndTime = bourseType.ReceivingConsignEndTime.Value;
                    //SetTrigger(bourseType, matchEndTime, Types.TradingTimeType.MatchEndWork, "matchEndTime");
                    SetEndTime(matchEndTime, bourseType, Types.TradingTimeType.MatchEndWork);
                }
            }

            //设置清算延时
            SetReckonDealy();
            ProcessTriggers();
        }

        private static void SetReckonDealy()
        {
            LastEndTime = LastEndTime.AddMinutes(GetConfigTime());
        }

        private static int GetConfigTime()
        {
            int mins = 30;

            string key = "reckondelaytime";
            string value = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    mins = int.Parse(value.Trim());
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
            return mins;

        }

        private static void ProcessTriggers()
        {
            DateTime now = DateTime.Now;
            FirstBeginTime = new DateTime(now.Year, now.Month, now.Day, FirstBeginTime.Hour, FirstBeginTime.Minute, 0);
            SetTrigger(FirstBeginTime, Types.TradingTimeType.MacthBeginWork, "Begin-First");

            string sTime = GetKeyTime(FirstBeginTime);
            if (matchTimes.Contains(sTime))
            {
                matchTimes.Remove(sTime);
            }

            foreach (var t in matchTimes)
            {
                int hour = 0;
                int minute = 0;
                GetKeyHourMinute(t, out hour, out minute);

                DateTime beginTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
                //update 李健华 2009-12-10 所有的开始初始化时间都提前二分钟秒
                DateTime iniStartTime = beginTime.AddMinutes(-2);
                //SetTrigger(beginTime, Types.TradingTimeType.MacthBeginWork, "Begin-" + beginTime);
                SetTrigger(iniStartTime, Types.TradingTimeType.MacthBeginWork, "Begin-" + beginTime);
            }

            SetTrigger(LastEndTime, Types.TradingTimeType.MatchEndWork, "End");

            //add 李健华 2010-06-10  设置分红时间
            DateTime melonCutTime = FirstBeginTime.AddMinutes(-GetConfigTime());
            SetTrigger(melonCutTime, Types.TradingTimeType.Open, "MelonCut");

        }

        private static void SetBeginTime(DateTime time, CM_BourseType type, Types.TradingTimeType timeType)
        {
            string sTime = GetKeyTime(time);
            if (!matchTimes.Contains(sTime))
            {
                matchTimes.Add(sTime);
            }

            if (FirstBeginTime.Year == 2000)
            {
                FirstBeginTime = time;
                return;
            }

            if (FirstBeginTime.Hour > time.Hour)
            {
                FirstBeginTime = time;

                return;
            }

            if (FirstBeginTime.Hour == time.Hour)
            {
                if (FirstBeginTime.Minute > time.Minute)
                {
                    FirstBeginTime = time;
                }
            }
        }

        #region MatchTime 帮助方法
        private static string GetKeyTime(DateTime time)
        {
            string hour = time.Hour.ToString();
            if (hour.Length == 1)
                hour = "0" + hour;

            string minute = time.Minute.ToString();
            if (minute.Length == 1)
                minute = "0" + minute;

            return hour + minute;
        }

        private static void GetKeyHourMinute(string keyTime, out int hour, out int minute)
        {
            string shour = keyTime.Substring(0, 2);
            if (shour.StartsWith("0"))
                shour = shour.Substring(1, 1);

            string sminute = keyTime.Substring(2, 2);
            if (sminute.StartsWith("0"))
                sminute = sminute.Substring(1, 1);

            hour = int.Parse(shour);
            minute = int.Parse(sminute);
        }

        #endregion


        private static void SetEndTime(DateTime time, CM_BourseType type, Types.TradingTimeType timeType)
        {
            if (LastEndTime.Year == 2000)
            {
                LastEndTime = time;

                return;
            }

            if (LastEndTime.Hour < time.Hour)
            {
                LastEndTime = time;
                return;
            }

            if (LastEndTime.Hour == time.Hour)
            {
                if (LastEndTime.Minute < time.Minute)
                {
                    LastEndTime = time;
                }
            }
        }

        private static void SetTrigger(DateTime triggerTime, Types.TradingTimeType timeType, string id)
        {
            DateTime now = DateTime.Now;
            DateTime time = new DateTime(now.Year, now.Month, now.Day, triggerTime.Hour, triggerTime.Minute,
                                         triggerTime.Second);

            string expFormat = "{0} {1} {2} ? * *";
            string exp = String.Format(expFormat, time.Second, time.Minute, time.Hour);

            ScheduleEventArgs args = new ScheduleEventArgs
                                         {
                                             //BourseTypeID = bourseType.BourseTypeID,
                                             TimeType = timeType,
                                             Time = time
                                         };

            string newTriggerName = id + "trigger";
            string newGroupName = id + "Group";

            CronTrigger trigger = new CronTrigger(newTriggerName, newGroupName, exp);
            trigger.JobDataMap.SetScheduleEventArgs(args);

            string newJobName = id + "job";
            JobDetail job = new JobDetail(newJobName, null, typeof(ScheduleJob));
            scheduler.ScheduleJob(job, trigger);

            string format = "ScheduleManager.SetTrigger[JobName={0},TiggerName={1},GroupName={2}]";
            string desc = string.Format(format, newJobName, newTriggerName, newGroupName);

            //LogHelper.WriteDebug(desc + " Args=" + args);
            LogHelper.WriteDebug("ScheduleManager.SetTrigger" + args);
        }

        private static ScheduleEventArgs GetScheduleEventArgs(this IDictionary dataMap)
        {
            return dataMap[SCHEDULE_ARGS] as ScheduleEventArgs;
        }

        private static void SetScheduleEventArgs(this IDictionary dataMap, ScheduleEventArgs args)
        {
            dataMap[SCHEDULE_ARGS] = args;
        }

        #region Nested type: ScheduleJob

        /// <summary>
        /// 任务包装类
        /// </summary>
        private class ScheduleJob : IJob
        {
            #region IJob Members

            /// <summary>
            /// 任务调度入口
            /// </summary>
            /// <param name="context">调度上下文</param>
            public void Execute(JobExecutionContext context)
            {
                ScheduleEventArgs args = context.Trigger.JobDataMap.GetScheduleEventArgs();

                if (args == null)
                {
                    args = new ScheduleEventArgs();
                    args.Time = context.FireTimeUtc.Value.ToLocalTime();

                    LogHelper.WriteDebug(string.Format("ScheduleJob[args=null,Time={0}]", args.Time));
                }
                else
                {
                    LogHelper.WriteDebug(string.Format("ScheduleJob[TimeType={0},Time={1}]",
                                                       args.TimeType, args.Time));

                    //if (CheckTradeDate(args))
                    DoNotify(args);
                }
            }

            #endregion
        }

        #endregion


        /// <summary>
        /// Create by:李健华
        /// Create date:2010-06-04
        /// 是否要处理开盘重新初始化内存表数据
        /// 每天只处理一次不再根据交易所处理，只拿当前所有交易所交易最开始的时候初始化即可
        /// 最开始时间的前五分钟或者十二分钟内处理
        /// </summary>
        /// <returns></returns>
        public static bool CanEndCache()
        {
            //小于或者大于最先开市时间不用提交初始化
            if (DateTime.Now.Hour != ScheduleManager.FirstBeginTime.Hour)
            {
                return false;
            }

            if (ScheduleManager.FirstBeginTime.Minute - 5 < DateTime.Now.Minute && DateTime.Now.Minute < ScheduleManager.FirstBeginTime.Minute + 12)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 任务事件信息
    /// </summary>
    [Serializable]
    public class ScheduleEventArgs : EventArgs
    {
        /// <summary>
        /// 交易所类型id
        /// </summary>
        //public int BourseTypeID;
        /// <summary>
        /// 事件触发时间
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// 时间类型
        /// </summary>
        public Types.TradingTimeType TimeType;

        public override string ToString()
        {
            string format = "[时间={0},时间类型={1}]";
            string desc = string.Format(format, Time, TimeType);

            return desc;
        }
    }
}