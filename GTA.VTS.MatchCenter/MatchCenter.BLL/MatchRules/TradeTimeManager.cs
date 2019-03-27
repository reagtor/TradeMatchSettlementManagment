using System;
using System.Collections.Generic;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.BLL.Common;
using MatchCenter.BLL.ManagementCenter;
using MatchCenter.Entity;
using GTA.VTS.Common.CommonUtility;

namespace MatchCenter.BLL
{
    /// <summary>
    /// 交易时间管理类
    /// Create BY：李健华
    /// Create Date：2009-08-24
    /// Update By:董鹏
    /// Update Date:2009-12-16
    /// Desc.:增加了新的关市操作时间区间判断方法
    /// </summary>
    public class TradeTimeManager : Singleton<TradeTimeManager>
    {
        #region 单一进入模式
        /// <summary>
        /// 费用价格计算功能类功能单一进入模式
        /// </summary>
        public static TradeTimeManager Instanse
        {
            get
            {
                return singletonInstance;
            }
        }
        #endregion

        #region 根据在交易时间列表中判断该时间是否在撮合时间内
        /// <summary>
        /// 判断时间是否在交易撮合时间内
        /// 根据在交易时间列表中判断该时间是否在撮合时间内
        /// </summary>
        /// <param name="tradeTimes">交易时间对象</param>
        /// <param name="marchTime">撮合时间</param>
        /// <returns></returns>
        public bool IsMarchTime(List<CM_TradeTime> tradeTimes, DateTime marchTime)
        {
            if (Utils.IsNullOrEmpty(tradeTimes))
            {
                return false;
            }
            foreach (var time in tradeTimes)
            {
                if (marchTime >= Utils.ConvertToNowDateTime((DateTime)time.StartTime) && marchTime <= Utils.ConvertToNowDateTime((DateTime)time.EndTime))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 根据交易所ID和撮合时间判断是否在交易撮合时间内
        /// <summary>
        /// 根据交易所ID和撮合时间判断是否在交易撮合时间内
        /// </summary>
        /// <param name="bourseTypeID">交易所ID</param>
        /// <param name="marchTime">撮合时间</param>
        /// <returns></returns>
        public bool IsMarchTime(int bourseTypeID, DateTime marchTime)
        {
            List<CM_TradeTime> tradeTimes = CommonDataCacheProxy.Instanse.GetCacheCM_TradeTimeByBourseID(bourseTypeID);
            if (Utils.IsNullOrEmpty(tradeTimes))
            {
                return false;
            }
            foreach (var time in tradeTimes)
            {
                if (marchTime >= Utils.ConvertToNowDateTime((DateTime)time.StartTime) && marchTime <= Utils.ConvertToNowDateTime((DateTime)time.EndTime))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 根据交易所ID和撮合日期判断是否在交易撮合日期内
        /// <summary>
        /// 根据交易所ID和撮合日期判断是否在交易撮合日期内
        /// </summary>
        /// <param name="bourseTypeID">交易所ID</param>
        /// <param name="marchDate">撮合日期</param>
        /// <returns></returns>
        public bool IsMarchDate(int bourseTypeID, DateTime marchDate)
        {
            string key = bourseTypeID.ToString() + "@" + marchDate.ToString("yyyy-MM-dd");
            CM_NotTradeDate notTradeDate = CommonDataCacheProxy.Instanse.GetCacheNotTradeDateByKey(key);
            if (notTradeDate != null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 根据商品代码获取商品所属交易所的接收委托时间

        /// <summary>
        /// 根据商品代码返回所属交易所
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public AcceptTradeTime GetAcceptTradeTimeByCommodityCode(string code)
        {
            AcceptTradeTime accetpTime = new AcceptTradeTime();
            CM_BourseType cm_bourseType = CommonDataManagerOperate.GetCM_BourseTypeByCommodityCode(code);
            if (cm_bourseType == null || !cm_bourseType.ReceivingConsignStartTime.HasValue || !cm_bourseType.ReceivingConsignEndTime.HasValue)
            {
                return null;
            }
            accetpTime.AcceptStartTime = cm_bourseType.ReceivingConsignStartTime.Value;
            accetpTime.AcceptEndTime = cm_bourseType.ReceivingConsignEndTime.Value;

            return accetpTime;
        }
        #endregion

        #region 判断某交易代码所属的交易所是否在某时间接收委托
        /// <summary>
        /// 判断某交易代码所属的交易所是否在某时间接收委托
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="code">交易代码</param>
        /// <returns></returns>
        public bool IsAcceptTime(DateTime time, string code)
        {
            AcceptTradeTime acceptTime = GetAcceptTradeTimeByCommodityCode(code);
            if (acceptTime == null)
            {
                LogHelper.WriteError(string.Format(GenerateInfo.CH_E008, code), new Exception());
                return false;
            }
            if (Utils.ConvertToNowDateTime(acceptTime.AcceptStartTime) > time)
            {
                return false;
            }
            if (Utils.ConvertToNowDateTime(acceptTime.AcceptEndTime) < time)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 判断当前时间是否清空当前撮合机中的委托
        /// <summary>
        /// 判断当前时间是否清空当前撮合机中的委托
        /// </summary>
        /// <returns></returns>
        public bool IsEndTime(int bourseTypeID)
        {
            CM_BourseType cm_bourseType = CommonDataCacheProxy.Instanse.GetCacheCM_BourseTypeByKey(bourseTypeID);
            if (cm_bourseType == null || cm_bourseType.ReceivingConsignEndTime == null)
            {
                return false;
            }
            DateTime endTime = ((DateTime)cm_bourseType.ReceivingConsignEndTime);

            DateTime tmpTime = endTime.AddHours(AppConfig.GetConfigClearTime());
            //当收市时间超过当天时，取当天最后时间
            if ((tmpTime.Date - endTime.Date).Days > 0)
            {
                tmpTime = Utils.ConvertToDateTime(DateTime.Now.ToShortDateString(), "23:59:59");
                return (DateTime.Now >= tmpTime) ? true : false;
            }

            if (DateTime.Now >= Utils.ConvertToNowDateTime(tmpTime))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 判断港股是否在某时间接收委托

        /// <summary>
        /// 判断某港股交易代码所属的交易所是否在某时间接收委托
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="code">交易代码</param>
        /// <returns></returns>
        public bool IsHKAcceptTime(DateTime time, string code)
        {
            AcceptTradeTime acceptTime = GetHKAcceptTradeTimeByCommodityCode(code);
            if (acceptTime == null)
            {
                LogHelper.WriteError(string.Format(GenerateInfo.CH_E008, code), new Exception());
                return false;
            }
            if (Utils.ConvertToNowDateTime(acceptTime.AcceptStartTime) > time)
            {
                return false;
            }
            if (Utils.ConvertToNowDateTime(acceptTime.AcceptEndTime) < time)
            {
                return false;
            }
            return true;
        }


        #region 根据港股商品代码获取商品所属交易所的接收委托时间

        /// <summary>
        /// 根据港股商品代码返回所属交易所
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public AcceptTradeTime GetHKAcceptTradeTimeByCommodityCode(string code)
        {
            AcceptTradeTime accetpTime = new AcceptTradeTime();
            CM_BourseType cm_bourseType = CommonDataManagerOperate.GetHK_BourseTypeByCommodityCode(code);
            if (cm_bourseType == null || !cm_bourseType.ReceivingConsignStartTime.HasValue || !cm_bourseType.ReceivingConsignEndTime.HasValue)
            {
                return null;
            }

            accetpTime.AcceptStartTime = cm_bourseType.ReceivingConsignStartTime.Value;
            accetpTime.AcceptEndTime = cm_bourseType.ReceivingConsignEndTime.Value;

            return accetpTime;
        }
        #endregion

        #endregion

        #region  获取所有交易所的交易时间中最先开市的和最后收市时间
        /// <summary>
        /// 获取所有交易所的交易时间中最先开市的和最后收市时间
        /// </summary>
        /// <returns></returns>
        public AcceptTradeTime GetAllTradeTimeOpenCloseTime()
        {
            List<CM_TradeTime> tradeTimes = CommonDataCacheProxy.Instanse.GetCacheALLCM_TradeTime();
            if (Utils.IsNullOrEmpty(tradeTimes))
            {
                return null;
            }
            AcceptTradeTime acc = new AcceptTradeTime();
            acc.AcceptStartTime = Utils.ConvertToNowDateTime(tradeTimes[0].StartTime.Value);
            acc.AcceptEndTime = Utils.ConvertToNowDateTime(tradeTimes[0].EndTime.Value);
            foreach (var time in tradeTimes)
            {
                DateTime start = Utils.ConvertToNowDateTime(time.StartTime.Value);
                if (acc.AcceptStartTime > start)
                {
                    acc.AcceptStartTime = start;
                }
                DateTime end = Utils.ConvertToNowDateTime(time.EndTime.Value);
                if (acc.AcceptEndTime < end)
                {
                    acc.AcceptEndTime = end;
                }
            }
            return acc;
        }
        #endregion

        #region 判断当前时间是否是开市时间的前半小时之内
        /// <summary>
        /// 判断当前时间是否是开市时间的前半小时之内
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsOpenMarketTime(DateTime time)
        {
            try
            {
                AcceptTradeTime accTime = GetAllTradeTimeOpenCloseTime();
                if (accTime == null)
                {
                    LogHelper.WriteError("CH-1108: 无法获取交易时间中最先开市的和最后收市时间", new Exception(""));
                    return false;
                }
                DateTime accstartTime = Utils.ConvertToNowDateTime((DateTime)accTime.AcceptStartTime);
                TimeSpan timespan = new TimeSpan(0, 30, 0);
                if (time >= accstartTime.Subtract(timespan) && time < accstartTime)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-1108: 判断当前时间是否是市时间的前十五分钟时间异常", ex);
                return false;
            }
        }
        #endregion

        #region 判断当前时间比最后收市时间还要大于30分钟和小于1小时之间内执行关市操作
        /// <summary>
        /// 判断当前时间比最后收市时间还要大于30分钟和小于1小时之间内执行关市操作
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsCloseMarketTime(DateTime time)
        {
            try
            {
                AcceptTradeTime accTime = GetAllTradeTimeOpenCloseTime();
                if (accTime == null)
                {
                    LogHelper.WriteError("CH-1218: 无法获取交易时间中最先开市的和最后收市时间", new Exception(""));
                    return false;
                }
                DateTime accendTime = Utils.ConvertToNowDateTime((DateTime)accTime.AcceptEndTime);
                //当前时间比最后收市时间还要大于30分钟和小于1小时之间内执行关市操作
                if (time >= accendTime.AddMinutes(30) && time < accendTime.AddMinutes(60))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-1218: 判断当前时间是否是关市时间异常", ex);
                return false;
            }
        }

        #region new Create by:董鹏 2009-12-16
        /// <summary>
        /// 判断当前时间是否在最先开市前半小时和最后收市后半小时之间的时间段内
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <returns></returns>
        public bool IsMarketTime(DateTime time)
        {
            try
            {
                AcceptTradeTime accTime = GetAllTradeTimeOpenCloseTime();
                if (accTime == null)
                {
                    LogHelper.WriteError("CH-1108: 无法获取交易时间中最先开市的和最后收市时间", new Exception(""));
                    return false;
                }
                //最先开始时间
                DateTime accstartTime = Utils.ConvertToNowDateTime((DateTime)accTime.AcceptStartTime);
                //最后收市时间
                DateTime accendTime = Utils.ConvertToNowDateTime((DateTime)accTime.AcceptEndTime);

                //当日第一秒时间
                DateTime lowerLimitTime = Utils.ConvertToDateTime(DateTime.Now.ToString("yyyy-MM-dd"), "00:00:00");
                //当日最后一秒时间
                DateTime upperLimitTime = Utils.ConvertToDateTime(DateTime.Now.ToString("yyyy-MM-dd"), "23:59:59");

                if ((time >= lowerLimitTime && time < accstartTime.AddMinutes(0 - 30)) || (time >= accendTime.AddMinutes(30) && time <= upperLimitTime))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-1218: 判断当前时间是否是关市时间异常", ex);
                return false;
            }
        }

        #endregion

        #endregion


    }
}
