#region Using Namespace

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.DAL.MatchCenterService;
using ReckoningCounter.Entity;
using ReckoningCounter.Model;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 功能类
    /// 作者：宋涛
    /// 日期：2008-11-25
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 判断一个List是否为空或者Count=0
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>是否为空</returns>
        public static bool IsNullOrEmpty<T>(IEnumerable<T> list)
        {
            if (list == null)
                return true;

            if (list.Count() == 0)
                return true;

            return false;
        }

        /// <summary>
        /// 比较两个时间
        /// 当time1早于time2时，返回负数；
        /// 当time1等于time2时，返回0；
        /// 当time1晚于time2时，返回正数；
        /// </summary>
        /// <param name="time1">time1</param>
        /// <param name="time2">time2</param>
        /// <returns>时间差</returns>
        public static int CompareTime(DateTime time1, DateTime time2)
        {
            int h = time1.Hour - time2.Hour;
            int m = time1.Minute - time2.Minute;
            int s = time1.Second - time2.Second;

            if (h != 0)
                return h;

            if (m != 0)
                return m;

            if (s != 0)
                return s;

            return 0;
        }

        public static string DescInfo(this StockOrderEntity entity)
        {
            string format =
                "StockOrderEntity[StockCode={0},TransactionDirection={1},OrderPrice={2},OrderVolume={3},IsMarketPrice={4}]";
            string desc = String.Format(format, entity.StockCode, entity.TransactionDirection, entity.OrderPrice,
                                        entity.OrderVolume, entity.IsMarketPrice);

            return desc;
        }

        public static string DescInfo(this HKOrderEntity entity)
        {
            string format =
                "HKOrderEntity[StockCode={0},TransactionDirection={1},OrderPrice={2},OrderVolume={3},PriceType={4}]";
            string desc = String.Format(format, entity.Code, entity.TransactionDirection, entity.OrderPrice,
                                        entity.OrderVolume, entity.HKPriceType);

            return desc;
        }

        public static string DescInfo(this CommoditiesOrderEntity entity)
        {
            string format =
                "CommoditiesOrderEntity[StockCode={0},TransactionDirection={1},OrderPrice={2},OrderVolume={3},IsMarketPrice={4}]";
            string desc = String.Format(format, entity.StockCode, entity.TransactionDirection, entity.OrderPrice,
                                        entity.OrderVolume, entity.IsMarketPrice);

            return desc;
        }

        public static string DescInfo(this FutureOrderEntity entity)
        {
            string format =
                "FutureOrderEntity[StockCode={0},TransactionDirection={1},OrderPrice={2},OrderVolume={3},IsMarketPrice={4}]";
            string desc = String.Format(format, entity.StockCode, entity.TransactionDirection, entity.OrderPrice,
                                        entity.OrderVolume, entity.IsMarketPrice);

            return desc;
        }

        public static string DescInfo(this StockDealBackEntity entity)
        {
            string format = "StockDealBackEntity[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3},ID={4}]";
            string desc = string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime,
                                        entity.Id);
            return desc;
        }

        public static string DescInfo(this HKDealBackEntity entity)
        {
            string format = "HKDealBackEntity[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3},ID={4}]";
            string desc = string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime,
                                        entity.ID);
            return desc;
        }


        public static string DescInfo(this FutureDealBackEntity entity)
        {
            string format = "FutureDealBackEntity[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3},ID={4}]";
            string desc = string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime,
                                        entity.Id);
            return desc;
        }

        public static string DescInfo(this CommoditiesDealBackEntity entity)
        {
            string format = "CommoditiesDealBackEntity[OrderNo={0},DealPrice={1},DealAmount={2},DealTime={3},ID={4}]";
            string desc = string.Format(format, entity.OrderNo, entity.DealPrice, entity.DealAmount, entity.DealTime,
                                        entity.Id);
            return desc;
        }

        public static string DescInfo(this CancelOrderEntity entity)
        {
            string format = "CancelOrderEntity[OrderNo={0},OrderVolume={1},IsSuccess={2},Message={3},ID={4}]";
            string desc = string.Format(format, entity.OrderNo, entity.OrderVolume, entity.IsSuccess, entity.Message,
                                        entity.Id);
            return desc;
        }

        /// <summary>
        /// 依据交易员ID,帐户类型取帐户信息
        /// </summary>
        /// <param name="strTraderId"></param>
        /// <param name="iAccountTypeId"></param>
        /// <returns></returns>
        public static UA_UserAccountAllocationTableInfo GetAccountByTraderInfo(string strTraderId, int iAccountTypeId)
        {
            //陈武民修改 2009年7月9日
            UA_UserAccountAllocationTableInfo result = null;
            UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
            string where = string.Format(" UserID = '{0}' ", strTraderId);
            List<UA_UserAccountAllocationTableInfo> list = dal.GetListArray(where);
            // DataRepository.UaUserAccountAllocationTableProvider.GetByUserId(strTraderId);

            foreach (UA_UserAccountAllocationTableInfo accountAllocationTable in list)
            {
                if (accountAllocationTable.AccountTypeLogo == iAccountTypeId)
                {
                    result = accountAllocationTable;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据交易员ID和商品代码获取资金账户
        /// </summary>
        /// <param name="strTraderId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static UA_UserAccountAllocationTableInfo GetCapitalAccount(string strTraderId, string code)
        {
            CM_BreedClass cmbc = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            return GetAccountByTraderInfo(strTraderId, cmbc.AccountTypeIDFund.Value);
        }

        /// <summary>
        /// 根据交易员ID和商品代码获取持仓账户
        /// </summary>
        /// <param name="strTraderId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static UA_UserAccountAllocationTableInfo GetHoldAccount(string strTraderId, string code)
        {
            CM_BreedClass cmbc = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            return GetAccountByTraderInfo(strTraderId, cmbc.AccountTypeIDHold.Value);
        }

        /// <summary>
        /// 根据持仓账户和商品代码获取资金账户
        /// </summary>
        /// <param name="holdAccount"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static UA_UserAccountAllocationTableInfo GetCapitalAccountByHoldAccount(string holdAccount, string code)
        {
            UA_UserAccountAllocationTableDal dal = new UA_UserAccountAllocationTableDal();
            string where = string.Format(" UserAccountDistributeLogo = '{0}' ", holdAccount);
            //var account = DataRepository.UaUserAccountAllocationTableProvider.GetByUserAccountDistributeLogo(holdAccount);
            var account = dal.GetModel(holdAccount);
            if (account != null)
            {
                string id = account.UserID;

                return GetCapitalAccount(id, code);
            }

            return null;
        }

        #region  注释2010-06-08 李健华
        ///// <summary>
        ///// 根据现货资金账户获取持仓账户
        ///// </summary>
        ///// <param name="capitalAccount"></param>
        ///// <returns></returns>
        //public static UA_UserAccountAllocationTableInfo GetXHHoldAccountByCapitalAccount(string capitalAccount)
        //{
        //    VTTraders traders = VTTradersFactory.GetStockTraders();
        //    VTTrader trader = traders.GetByAccount(capitalAccount);

        //    foreach (var list in trader.AccountPairList)
        //    {
        //        if (list.CapitalAccount.UserAccountDistributeLogo == capitalAccount)
        //            return list.HoldAccount;
        //    }

        //    return null;
        //}
        #endregion

        /// <summary>
        /// 根据错误信息获取错误编码
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool GetErrorCode(string message, out int type)
        {
            type = 0;
            bool result = false;
            message = message.Trim().ToUpper();

            if (!string.IsNullOrEmpty(message))
            {
                int i = message.IndexOf("GT-");
                if (i != -1)
                {
                    try
                    {
                        string code = message.Substring(i + 3, 4);

                        bool isSuccess = int.TryParse(code, out type);
                        if (isSuccess)
                            result = true;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message, ex);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取实时行情组件使用模式
        /// </summary>
        /// <returns>1.socket 2.fast</returns>
        public static int GetRealTimeMode()
        {
            int realTimeMode = 1;
            try
            {
                string setting = ConfigurationManager.AppSettings["realTimeMode"];
                if (!string.IsNullOrEmpty(setting))
                {
                    int count;
                    bool isSuccess = int.TryParse(setting.Trim(), out count);
                    if (isSuccess)
                        realTimeMode = count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return realTimeMode;
        }

        /// <summary>
        /// 获取实时行情组件用户名
        /// </summary>
        /// <returns>实时行情组件用户名</returns>
        public static string GetRealTimeUserName()
        {
            string userName = "rtuser";
            try
            {
                string name = ConfigurationManager.AppSettings["ServerUserName"];
                if (!string.IsNullOrEmpty(name))
                {
                    userName = name;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return userName;
        }

        /// <summary>
        /// 获取实时行情组件密码
        /// </summary>
        /// <returns>实时行情组件密码</returns>
        public static string GetRealTimePassword()
        {
            string password = "11";
            try
            {
                string ps = ConfigurationManager.AppSettings["ServerPassword"];
                if (!string.IsNullOrEmpty(ps))
                {
                    password = ps;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return password;
        }

        /// <summary>
        /// 获取定时清算提交间隔时间（毫秒）
        /// </summary>
        /// <returns></returns>
        public static int GetReckonCommitInterval()
        {
            int commitInterval = 1000;
            string value = ConfigurationManager.AppSettings["ReckonCommitInterval"];
            if (!string.IsNullOrEmpty(value))
            {
                int count = 0;
                bool isSuccess = int.TryParse(value.Trim(), out count);
                if (isSuccess)
                    commitInterval = count;
            }

            return commitInterval;
        }

        public static decimal Round(decimal val)
        {
            int len = 3;
            return Math.Round(val, len, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// 把数据根据要转换为四舍五入长度转换
        /// </summary>
        /// <param name="val"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static decimal Round(decimal val, int len)
        {
            return Math.Round(val, len, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 获取当前日期的当前月的天数并返回当月所有非周未的时间列表
        /// </summary>
        /// <param name="days">返回当前月的天数</param>
        /// <returns></returns>
        public static List<DateTime> GetCurrentMothDay(out int days)
        {
            days = 30;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            //得到当前月的天数
            switch (month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    if (DateTime.IsLeapYear(year))
                    {
                        //闰年二月为29天
                        days = 29;
                    }
                    else
                    {
                        //不是闰年，二月为28天
                        days = 28;
                    }
                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }
            DateTime startDate = DateTime.Parse(string.Format("{0}-{1}-01", year, month));
            DateTime endDate = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, days));

            List<DateTime> list = new List<DateTime>();

            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                DayOfWeek t = dt.DayOfWeek;
                if (t != DayOfWeek.Sunday && t != DayOfWeek.Saturday)
                {
                    list.Add(dt);
                }
            }


            return list;

        }
    }
}