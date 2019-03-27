using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.DAL.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using System.Data;
using ReckoningCounter.Model;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.BLL.ManagementCenter;
using GTA.VTS.Common.CommonObject;

namespace ReckoningCounter.BLL.ScheduleManagement
{
    /// <summary>
    /// Title:状态表检查管理类
    /// Desc.:主要是记录管理是否清算和检查资金的状态记录等
    /// Create By:宋涛
    /// Create date:2009-05-10
    /// Update by:李健华
    /// Update Date:2010-03-15
    /// Desc.:修改增加相应的方法，为了期货故障恢复使用
    /// </summary>
    public class StatusTableChecker
    {
        private const string FutureDayCheckDate = "FutureDayCheckDate";
        private const string FutureHistoryDataProcessDate = "FutureHistoryDataProcessDate";
        private const string FutureReckoningDate = "FutureReckoningDate";

        private const string MelonCutDate = "MelonCutDate";
        private const string StockHistoryDataProcessDate = "StockHistoryDataProcessDate";
        private const string StockReckoningDate = "StockReckoningDate";

        private const string HKHistoryDataProcessDate = "HKHistoryDataProcessDate";
        private const string HKReckoningDate = "HKReckoningDate";

        private const string RegistMelonCutDate = "RegistMelonCutDate";



        /// <summary>
        /// 是否已经做了现货历史数据处理
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneStockHistoryDataProcess(DateTime checkDate)
        {
            //get { return CheckDate(StockHistoryDataProcessDate); }
            return CheckDate(StockHistoryDataProcessDate, checkDate);
        }
        /// <summary>
        /// 是否已经做了现货分红登记
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneRegistMelonCutDate(DateTime checkDate)
        {

            return CheckDate(RegistMelonCutDate, checkDate);
        }
        /// <summary>
        /// 是否已经做了现货清算
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneStockReckoning(DateTime checkDate)
        {
            // get { return CheckDate(StockReckoningDate); }
            return CheckDate(StockReckoningDate, checkDate);
        }

        /// <summary>
        /// 是否已经做了港股历史数据处理
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneHKHistoryDataProcess(DateTime checkDate)
        {
            // get { return CheckDate(HKHistoryDataProcessDate); }
            return CheckDate(HKHistoryDataProcessDate, checkDate);
        }

        /// <summary>
        /// 是否已经做了港股清算
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneHKReckoning(DateTime checkDate)
        {
            // get { return CheckDate(HKReckoningDate); }
            return CheckDate(HKReckoningDate, checkDate);
        }

        /// <summary>
        /// 是否已经做了期货每日开盘检查
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneFutureDayCheck(DateTime checkDate)
        {
            bool rtn = CheckDate(FutureDayCheckDate, checkDate);
            //get { return CheckDate(FutureDayCheckDate); }

            LogHelper.WriteDebug("期货每日开盘检查时间:StatusName=" + FutureDayCheckDate + ", lastDate=" + GetByName(FutureDayCheckDate) + ", checkDate=" + checkDate.ToString() + ", HasDoneFutureDayCheck=" + rtn);
            return rtn;
        }

        /// <summary>
        /// 是否已经做了期货历史数据处理
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneFutureHistoryDataProcess(DateTime checkDate)
        {
            //get { return CheckDate(FutureHistoryDataProcessDate); }
            return CheckDate(FutureHistoryDataProcessDate, checkDate);
        }

        /// <summary>
        /// 是否已经做了期货清算
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneFutureReckoning(DateTime checkDate)
        {
            //get { return CheckDate(FutureReckoningDate); }
            return CheckDate(FutureReckoningDate, checkDate);
        }



        /// <summary>
        /// 是否已经做过分红
        /// </summary>
        /// <param name="checkDate">要检查的日期</param>
        /// <returns>返回是否成功</returns>
        public static bool HasDoneMelonCut(DateTime checkDate)
        {
            //get { return CheckDate(MelonCutDate); }
            return CheckDate(MelonCutDate, checkDate);
        }

        /// <summary>
        /// 获取期货清算最后一次成功的时间
        /// </summary>
        /// <returns></returns>
        public static string GetFutureReckoningHasDone()
        {
            return GetByName(FutureReckoningDate);
        }

        #region 更新方法

        /// <summary>
        /// 更新数据库中现货盘后历史数据处理日期
        /// </summary>
        public static void UpdateStockHistoryDataProcessDate(ReckoningTransaction tm)
        {
            DoUpdate(StockHistoryDataProcessDate, tm);
        }

        /// <summary>
        /// 更新数据库中港股盘后历史数据处理日期
        /// </summary>
        public static void UpdateHKHistoryDataProcessDate(ReckoningTransaction tm)
        {
            DoUpdate(HKHistoryDataProcessDate, tm);
        }

        /// <summary>
        /// 更新数据库中期货每日开盘检查日期
        /// </summary>
        public static void UpdateFutureDayCheckDate(ReckoningTransaction tm)
        {
            DoUpdate(FutureDayCheckDate, tm);
        }

        /// <summary>
        /// 更新数据库中期货盘后历史数据处理日期
        /// </summary>
        public static void UpdateFutureHistoryDataProcessDate(ReckoningTransaction tm)
        {
            DoUpdate(FutureHistoryDataProcessDate, tm);
        }

        /// <summary>
        /// 更新数据库中现货清算日期
        /// </summary>
        public static void UpdateStockReckoningDate(ReckoningTransaction tm)
        {
            DoUpdate(StockReckoningDate, tm);
        }
        /// <summary>
        /// 更新数据库中现货清算日期
        /// </summary>
        public static void UpdateRegistMelonCutDate(ReckoningTransaction tm)
        {
            DoUpdate(RegistMelonCutDate, tm);
        }
        /// <summary>
        /// 更新数据库中港股清算日期
        /// </summary>
        public static void UpdateHKReckoningDate(ReckoningTransaction tm)
        {
            DoUpdate(HKReckoningDate, tm);
        }

        /// <summary>
        /// 更新数据库中期货清算日期
        /// </summary>
        public static void UpdateFutureReckoningDate(ReckoningTransaction tm)
        {
            DoUpdate(FutureReckoningDate, tm);
        }
        /// <summary>
        /// 更新数据库中期货清算日期和移动历史数据日期
        /// </summary>
        /// <param name="tm">事务</param>
        /// <param name="value">日期</param>
        public static void UpdateFutureReckoningDate(string value, ReckoningTransaction tm)
        {
            DoUpdate(FutureReckoningDate, value, tm);
            DoUpdate(FutureHistoryDataProcessDate, value, tm);
        }
        //更新数据库中的分红日期
        public static void UpdateMelonCutDate()
        {
            ReckoningTransaction reckoningTransaction = new ReckoningTransaction();
            Database database = DatabaseFactory.CreateDatabase();
            DbConnection connection = database.CreateConnection();

            //TransactionManager tm = TransactionFactory.GetTransactionManager();
            //tm.BeginTransaction();

            try
            {
                connection.Open();
                reckoningTransaction.Database = database;
                DbTransaction transaction = connection.BeginTransaction();
                reckoningTransaction.Transaction = transaction;

                DoUpdate(MelonCutDate, reckoningTransaction);
                reckoningTransaction.Transaction.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("UpdateMelonCutDate", ex);
                reckoningTransaction.Transaction.Rollback();
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        #endregion

        #region 功能方法

        private static void DoUpdate(string name, string value, ReckoningTransaction tm)
        {
            BD_StatusTableDal dal = new BD_StatusTableDal();
            BD_StatusTableInfo statusTable = dal.GetModel(name);
            if (statusTable != null)
            {
                if (tm == null)
                    dal.Delete(name);
                else
                    dal.Delete(name, tm);
            }


            statusTable = new BD_StatusTableInfo();
            statusTable.name = name;
            statusTable.value = value;

            if (tm != null)
            {
                dal.Add(statusTable, tm.Database, tm.Transaction);
            }
            else
            {
                dal.Add(statusTable);
            }

            string format = "更新系统状态表[Name={0},Date={1}]";
            string desc = string.Format(format, name, statusTable.value);
            LogHelper.WriteInfo(desc);
        }
        /// <summary>
        /// 更新当前日期到数据库状态表中
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tm"></param>
        private static void DoUpdate(string name, ReckoningTransaction tm)
        {
            BD_StatusTableDal dal = new BD_StatusTableDal();
            BD_StatusTableInfo statusTable = dal.GetModel(name);
            if (statusTable != null)
            {
                if (tm == null)
                    dal.Delete(name);
                else
                    dal.Delete(name, tm);
            }


            statusTable = new BD_StatusTableInfo();
            statusTable.name = name;
            statusTable.value = DateTime.Now.ToString();

            if (tm != null)
            {
                dal.Add(statusTable, tm.Database, tm.Transaction);
            }
            else
            {
                dal.Add(statusTable);
            }

            string format = "更新系统状态表[Name={0},Date={1}]";
            string desc = string.Format(format, name, statusTable.value);
            LogHelper.WriteInfo(desc);
        }

        /// <summary>
        /// 检查今日是否已经做过动作
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool CheckDate(string name)
        {
            string lastDate = "";
            return CheckDate(name, out lastDate);
        }

        /// <summary>
        /// 检查今日是否已经做过动作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastDate">最后记录的日期</param>
        /// <returns></returns>
        private static bool CheckDate(string name, out string lastDate)
        {
            string dateStr = GetByName(name);
            lastDate = dateStr.Trim();

            if (lastDate == "")
            {
                return false;
            }

            try
            {
                DateTime dateTime = DateTime.Parse(lastDate);

                if (dateTime.Day == DateTime.Now.Day)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// 根据时间和日期检查是否已经做过动作
        /// </summary>
        /// <param name="name">要检查的内容</param>
        /// <param name="lastDate">日期</param>
        /// <returns></returns>
        private static bool CheckDate(string name, DateTime lastDate)
        {
            string dateStr = GetByName(name);
            string lastDateStr = dateStr.Trim();

            if (lastDateStr == "")
            {
                return false;
            }

            try
            {
                DateTime dateTime = DateTime.Parse(lastDateStr);

                if (dateTime.Day == lastDate.Day)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }
        }
        /// <summary>
        /// 根据name获取日期
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetByName(string name)
        {
            BD_StatusTableInfo statusTable = null;
            BD_StatusTableDal bd_StatusTableDal = new BD_StatusTableDal();
            try
            {
                statusTable = bd_StatusTableDal.GetModel(name);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (statusTable == null)
                return "";

            return statusTable.value;
        }

        #endregion


        /// <summary>
        /// 期货是否要执行期货清算故障恢复
        /// </summary>
        /// <param name="faultRecoveryTime">要执行故障恢复清算的日期</param>
        /// <param name="errMsg">查询返回异常或者提示信息</param>
        /// <returns></returns>
        public static bool IsFutureReckoningFaultRecovery(out DateTime faultRecoveryTime, out string errMsg)
        {

            errMsg = "";

            //要执行清算的日期
            faultRecoveryTime = DateTime.Now;
            try
            {
                string dateTime = StatusTableChecker.GetFutureReckoningHasDone();
                if (string.IsNullOrEmpty(dateTime))
                {
                    errMsg = "清算状态表记录无法获取，有异常请检查!";
                    return false;
                }
                DateTime lastDate = DateTime.Parse(dateTime);

                if (lastDate.Day == DateTime.Now.Day)
                {
                    errMsg = "今日已经执行清算完成！";
                    return false;
                }
                else if (lastDate.Date.AddDays(1).Date == DateTime.Now.Date)
                {
                    DateTime nowDate = DateTime.Now;

                    DateTime firstBeginTime = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, ScheduleManager.FirstBeginTime.Hour, ScheduleManager.FirstBeginTime.Minute, ScheduleManager.FirstBeginTime.Second);
                    DateTime lastEndTime = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, ScheduleManager.LastEndTime.Hour, ScheduleManager.LastEndTime.Minute, ScheduleManager.LastEndTime.Second);
                    if (nowDate >= firstBeginTime && nowDate <= lastEndTime)
                    {
                        errMsg = "今日还没有到清算的时间，请稍后再试！";
                        return false;
                    }
                    //如果当前时间是零晨到开市之前的也应该不用执行清算
                    if (nowDate >= DateTime.Now.Date && nowDate <= firstBeginTime)
                    {
                         errMsg = "今日还没有到清算的时间，请稍后再试！";
                        return false;
                    }
                    //如果不在交易时间内了，还没有清算完成那么返回要执行清算
                    //这里判断已经保证是在当前了
                    //这里再加二十分钟是因为清算时要时间（设置二十分钟因为测试过有一万用户的时间期货清算大约要五分钟）
                    if (nowDate > lastEndTime.AddMinutes(20))
                    {
                        return true;
                    }
                }

                //处理星期五清算不成功，然后星期一处理的手动执行清算后

                #region update 2010-03-17 old code
                //bool isTradeDate = false;
                //while (!isTradeDate)
                //{
                //    //判断是否是星期六星期日，如果是向后推清算日期
                //    for (int i = 1; i <= 3; i++)
                //    {
                //        faultRecoveryTime = lastDate.AddDays(i);
                //        if (faultRecoveryTime.DayOfWeek == DayOfWeek.Sunday || faultRecoveryTime.DayOfWeek == DayOfWeek.Saturday)
                //        {
                //            continue;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //    //获取到要清算的日期再判断是否是非交易日期,如果是再向后推
                //    IList<CM_BourseType> cm_bourseList = MCService.CommonPara.GetAllBourseType();

                //    foreach (var item in cm_bourseList)
                //    {
                //        isTradeDate = MCService.CommonPara.IsTradeDate(item.BourseTypeID, faultRecoveryTime);
                //        if (!isTradeDate)
                //        {
                //            break;
                //        }
                //    }
                //}
                #endregion

                #region 2010-03-17 new
                //这里返回的，我们只要补完当前日期的前一天的记录即可
                bool isTradeDate = false;
                while (!isTradeDate)
                {
                    #region 判断是否是星期六星期日，如果是向后推清算日期
                    //判断是否是星期六星期日，如果是向后推清算日期
                    for (int i = 1; i <= 3; i++)
                    {
                        faultRecoveryTime = faultRecoveryTime.AddDays(-1);   //向前推日期，如果是星期六星期日再向前推
                        if (faultRecoveryTime.DayOfWeek == DayOfWeek.Sunday || faultRecoveryTime.DayOfWeek == DayOfWeek.Saturday)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion

                    //如果向前推到最后还是和原来的清算日期相等那么就不用再清算
                    if (faultRecoveryTime.ToShortDateString() == lastDate.ToShortDateString())
                    {
                        errMsg = "今日向前再推算日期与之前正常清算的日期相符，不用再清算！";
                        return false;
                    }

                    #region  过期不是期货的不用判断
                    //获取到要清算的日期再判断是否是非交易日期,如果是再向后推(while)

                    //获取 所有期货的交易所的ID
                    //IList<CM_BourseType> cm_bourseList = MCService.CommonPara.GetAllBourseType();
                    List<int> cm_bourseList = new List<int>();
                    //获取所有品种类别
                    IList<CM_BreedClass> cm_breedClass = MCService.CommonPara.GetAllBreedClass();


                    //过期不是期货的不用判断
                    foreach (var item in cm_breedClass)
                    {
                        switch ((Types.BreedClassTypeEnum)item.BreedClassTypeID)
                        {
                            case Types.BreedClassTypeEnum.Stock:
                                break;
                            case Types.BreedClassTypeEnum.CommodityFuture:
                            case Types.BreedClassTypeEnum.StockIndexFuture:
                                if (!cm_bourseList.Contains(item.BourseTypeID.Value))
                                {
                                    cm_bourseList.Add(item.BourseTypeID.Value);
                                }
                                break;
                            case Types.BreedClassTypeEnum.HKStock:
                                break;
                            default:
                                break;
                        }

                    }
                    #endregion

                    //要每个交易所都是非交易时间才可以进行清算故障恢复
                    foreach (var item in cm_bourseList)
                    {
                        isTradeDate = MCService.CommonPara.IsTradeDate(item, faultRecoveryTime);
                        if (isTradeDate)
                        {
                            #region 再处理向前推日期，解决星期五清算不成功，星期一进行清算时再获取清算的日志返回故障恢复清算错误，即把日期再返回未清算紧跟的非交易日后
                            DateTime requestDate = faultRecoveryTime;
                            //只处理星期六星期日
                            for (int i = 1; i <= 3; i++)
                            {
                                requestDate = requestDate.AddDays(1);
                                if (requestDate.DayOfWeek == DayOfWeek.Sunday || requestDate.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    faultRecoveryTime = requestDate;
                                    //如果当前时间是星期六那么就直接退出用星期六即可
                                    if (requestDate.Date == DateTime.Now.Date)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            #endregion
                            return true;
                        }
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return true;
        }

        /// <summary>
        /// 今日期货是否可以执行清算（内部即检查当前时间与记录比较前一日是否请算完成）
        /// </summary>
        /// <param name="faultRecoveryTime">能够清算的时间</param>
        /// <param name="errMsg">查询返回异常或者提示信息</param>
        /// <returns></returns>
        public static bool IsFutureTodayReckoning(out DateTime faultRecoveryTime, out string errMsg)
        {
            errMsg = "";

            //要执行清算的日期
            faultRecoveryTime = DateTime.Now;
            string dateTime = StatusTableChecker.GetFutureReckoningHasDone();
            if (string.IsNullOrEmpty(dateTime))
            {
                errMsg = "清算状态表无记录，可以执行清算!";
                return true;
            }
            DateTime lastDate = DateTime.Parse(dateTime);

            if (lastDate.Day == DateTime.Now.Day)
            {
                errMsg = "今日已经执行清算完成！";
                return false;
            }
            else if (lastDate.Date.AddDays(1).Date == DateTime.Now.Date)
            {
                //DateTime nowDate = DateTime.Now;

                //DateTime firstBeginTime = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, ScheduleManager.FirstBeginTime.Hour, ScheduleManager.FirstBeginTime.Minute, ScheduleManager.FirstBeginTime.Second);
                //DateTime lastEndTime = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, ScheduleManager.LastEndTime.Hour, ScheduleManager.LastEndTime.Minute, ScheduleManager.LastEndTime.Second);
                //if (nowDate >= firstBeginTime && nowDate <= lastEndTime)
                //{
                //    errMsg = "今日还没有到清算的时间，请稍后再试！";
                //    return false;
                //}
                return true;
            }
            faultRecoveryTime = lastDate;
            bool isTradeDate = false;
            while (!isTradeDate)
            {
                //判断是否是星期六星期日，如果是向后推清算日期
                for (int i = 1; i <= 3; i++)
                {
                    faultRecoveryTime = faultRecoveryTime.AddDays(i);
                    if (faultRecoveryTime.DayOfWeek == DayOfWeek.Sunday || faultRecoveryTime.DayOfWeek == DayOfWeek.Saturday)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                //获取到要清算的日期再判断是否是非交易日期,如果是再向后推(while)

                //获取 所有期货的交易所的ID
                //IList<CM_BourseType> cm_bourseList = MCService.CommonPara.GetAllBourseType();
                List<int> cm_bourseList = new List<int>();
                //获取所有品种类别
                IList<CM_BreedClass> cm_breedClass = MCService.CommonPara.GetAllBreedClass();

                //过期不是期货的不用判断
                foreach (var item in cm_breedClass)
                {
                    switch ((Types.BreedClassTypeEnum)item.BreedClassTypeID)
                    {
                        case Types.BreedClassTypeEnum.Stock:
                            break;
                        case Types.BreedClassTypeEnum.CommodityFuture:
                        case Types.BreedClassTypeEnum.StockIndexFuture:
                            if (!cm_bourseList.Contains(item.BourseTypeID.Value))
                            {
                                cm_bourseList.Add(item.BourseTypeID.Value);
                            }
                            break;
                        case Types.BreedClassTypeEnum.HKStock:
                            break;
                        default:
                            break;
                    }

                }
                //要所有交易所都为非交易日时才可以清算
                foreach (var item in cm_bourseList)
                {
                    isTradeDate = MCService.CommonPara.IsTradeDate(item, faultRecoveryTime);
                    if (isTradeDate)
                    {
                        break;
                    }
                }
            }

            if (faultRecoveryTime.Date == DateTime.Now.Date)
            {
                return true;
            }
            return false;
        }
    }
}
