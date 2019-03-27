using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.DAL.SpotTradingDevolveService;
using MatchCenter.BLL.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.DAL.FuturesDevolveService;
//增加港股引用
using MatchCenter.DAL.HKTradingRulesService;

namespace MatchCenter.BLL.ManagementCenter
{
    /// <summary>
    /// 公共数据管理操作
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// Desc: 增加商品期货新月份合约判断
    /// Update By: 董鹏
    /// Update Desc: 2010-01-25
    /// Desc: 增加了股指期货"季月合约上市首日涨跌幅","合约最后交易日涨跌幅"的判断方法
    /// Update By:董鹏
    /// Update Date:2010-03-05
    /// </summary>
    public static class CommonDataManagerOperate
    {
        #region 1.根据商品Code判断是否为新股上市
        /// <summary>
        /// 根据商品Code判断是否为新股上市,这里只要是比较上市时间是否为当前时间
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static bool IsNewMarketyByCode(string code)
        {
            CM_Commodity item = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (item == null)
            {
                return false;
            }
            else if (item.MarketDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region 2.根据商品代码判断是否是增发上市
        /// <summary>
        /// 根据商品代码判断是否是增发上市
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static bool IsIncreaseMarketByCode(string code)
        {
            if (CommonDataCacheProxy.Instanse.GetCacheZFInfoByCode(code) != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 3.根据代码判断是否有涨跌幅
        /// <summary>
        /// 根据代码判断是否有涨跌幅 
        /// 即如果是新股上市或者是增发上市即有涨跌幅
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static bool IsLowHightPriceByCode(string code)
        {
            if (IsNewMarketyByCode(code) || IsIncreaseMarketByCode(code))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 4.根据品种有效申报标识标识(即有效申报类型主键)获取有效申报类型值实体
        /// <summary>
        /// 根据品种有效申报标识标识(即有效申报类型主键)获取有效申报类型值实体
        /// </summary>
        /// <param name="breedClassValidID">有效申报品种有效申报标识标识ID</param>
        /// <returns></returns>
        public static XH_ValidDeclareValue GetValidDeclareValueByBreedClassValidID(int breedClassValidID)
        {
            List<XH_ValidDeclareValue> list = CommonDataCacheProxy.Instanse.GetCacheValidDeclareValues();
            if (Utils.IsNullOrEmpty(list))
                return null;
            foreach (XH_ValidDeclareValue item in list)
            {
                if (item.BreedClassValidID == breedClassValidID)
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region 5.根据现货_品种_涨跌幅ID标识(即有现货涨跌幅控制类型主键)获取现货_品种_涨跌幅实体
        /// <summary>
        /// 根据现货_品种_涨跌幅ID标识(即有现货涨跌幅控制类型主键)获取现货_品种_涨跌幅实体
        /// </summary>
        /// <param name="breedClassHighLowID">现货_品种_涨跌幅id</param>
        /// <returns></returns>
        public static XH_SpotHighLowValue GetXH_SpotHighLowValueByBreedClassHighLowID(int breedClassHighLowID)
        {
            List<XH_SpotHighLowValue> list = CommonDataCacheProxy.Instanse.GetCacheXH_SpotHighLowValue();
            if (Utils.IsNullOrEmpty(list))
            {
                return null;
            }
            foreach (XH_SpotHighLowValue item in list)
            {
                if (item.BreedClassHighLowID == breedClassHighLowID)
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region 6.根据商品编号返回现货交易规则
        /// <summary>
        /// 根据商品编号返回现货交易规则
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static XH_SpotTradeRules GetXH_SpotTradeRulesByCommodityCode(string code)
        {
            CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (commodity == null)
            {
                return null;
            }
            var breedClassID = (int)commodity.BreedClassID;
            return CommonDataCacheProxy.Instanse.GetCacheXH_SpotTradeRulesByKey(breedClassID);
        }
        #endregion

        #region 7.根据商品编号获取所属的商品类别
        /// <summary>
        /// 7.根据商品编号获取所属的商品类别
        /// </summary>
        /// <param name="commodityCode">商品代码</param>
        /// <param name="type">是不是港股</param>
        /// <returns></returns>
        public static CM_BreedClass GetBreedClassByCommodityCode(string commodityCode, int type)
        {
            if (string.IsNullOrEmpty(commodityCode))
            {
                return null;
            }
            if (type == 1)
            {//非港股
                CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(commodityCode);
                if (commodity == null || !commodity.BreedClassID.HasValue)
                {
                    return null;
                }
                return CommonDataCacheProxy.Instanse.GetCacheCM_BreedClassByKey(commodity.BreedClassID.Value);
            }
            else
            {//港股
                HK_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheHKCommodityByCode(commodityCode);
                if (commodity == null || !commodity.BreedClassID.HasValue)
                {
                    return null;
                }
                return CommonDataCacheProxy.Instanse.GetCacheCM_BreedClassByKey(commodity.BreedClassID.Value);

            }
        }
        #endregion

        #region 8.根据范围值返回范围实体
        ///// <summary>
        ///// 根据范围值返回范围实体
        ///// 返回的实体是当前值小于上限值和大于下限值的实体,此方法会在所有的范围值里查找，
        ///// 只要查找到介于此值间的记录即返回
        ///// </summary>
        ///// <param name="value">范围值</param>
        ///// <returns></returns>
        //public static CM_FieldRange GetFieldRangeByRangeValue(decimal value)
        //{
        //    List<CM_FieldRange> list = CommonDataCacheProxy.Instanse.GetCacheCM_FileldRange();
        //    if (Utils.IsNullOrEmpty(list))
        //    {
        //        return null;
        //    }
        //    bool up = false;
        //    bool dowm = false;
        //    foreach (CM_FieldRange item in list)
        //    {
        //        up = Utils.TheValueCompareOtherValue(value, (decimal)item.UpperLimit, (Types.IsYesOrNo)item.UpperLimitIfEquation == Types.IsYesOrNo.Yes ? true : false);
        //        dowm = Utils.TheValueCompareOtherValue((decimal)item.LowerLimit, value, (Types.IsYesOrNo)item.LowerLimitIfEquation == Types.IsYesOrNo.Yes ? true : false);

        //        if (up && dowm)
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}
        #endregion

        #region 9.根据撮合机ID取得撮合机所分配置的撮合代码
        /// <summary>
        /// 根据撮合机ID取得撮合机所分配置的撮合代码
        /// </summary>
        /// <param name="matchMachineId">ID</param>
        /// <returns></returns>
        public static List<RC_TradeCommodityAssign> GetTradeCommodityAssignByMatchineID(int matchMachineId)
        {
            List<RC_TradeCommodityAssign> list = CommonDataCacheProxy.Instanse.GetCacheTradeCommodityAssign();
            List<RC_TradeCommodityAssign> commodityAssignDevice = new List<RC_TradeCommodityAssign>();

            #region linq语法
            //List<RC_TradeCommodityAssign> list = from item in commodityAssigns
            //                 from id in item.MatchMachineID
            //                 where id = matchMachineId
            //                 select item;
            //foreach (var mode in list)
            //{
            //    mode.MatchMachineID 
            //}
            //List students = new List();
            //from student in students
            //from score in student.Scores
            //where score > 90
            //select new { Last = student.LastName, score };　 
            #endregion

            if (Utils.IsNullOrEmpty(list))
            {
                return null;
            }
            foreach (var assign in list)
            {
                if (assign.MatchMachineID == matchMachineId)
                {
                    commodityAssignDevice.Add(assign);
                }
            }
            return commodityAssignDevice;

        }
        #endregion

        #region 10.根据商品代码获取熔断时间实体
        /// <summary>
        /// 根据商品代码获取熔断时间实体
        /// </summary>
        /// <param name="commondityCode">代码</param>
        /// <returns></returns>
        public static List<CM_FuseTimesection> GetFuseTimeEntitysByCommondityCode(string commondityCode)
        {
            List<CM_FuseTimesection> cm_FuseTimesections = new List<CM_FuseTimesection>();
            List<CM_FuseTimesection> list = CommonDataCacheProxy.Instanse.GetCacheCM_FuseTimesection();
            //撮合中心熔断实体对象
            if (Utils.IsNullOrEmpty(list))
            {
                return null;
            }

            foreach (CM_FuseTimesection timesection in list)
            {
                if (timesection.CommodityCode == commondityCode)
                {
                    cm_FuseTimesections.Add(timesection);
                }
            }
            return cm_FuseTimesections;
        }
        #endregion

        #region 11.根据期货商品代码返回期货交易规则
        /// <summary>
        /// 返回期货规则
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static QH_FuturesTradeRules GetQH_FuturesTradeRulesByCommodityCode(string code)
        {
            CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (commodity == null)
            {
                return null;
            }
            var breedClassID = (int)commodity.BreedClassID;
            return CommonDataCacheProxy.Instanse.GetCacheQH_FuturesTradeRulesByKey(breedClassID);
        }
        #endregion

        #region 12.根据商品代码返回所属交易所
        /// <summary>
        /// 根据港股商品代码返回所属交易所
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static CM_BourseType GetHK_BourseTypeByCommodityCode(string code)
        {
            HK_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheHKCommodityByCode(code);
            if (commodity == null || commodity.BreedClassID.HasValue == false)
            {
                return null;
            }
            CM_BreedClass breedClassEntry = CommonDataCacheProxy.Instanse.GetCacheCM_BreedClassByKey((int)commodity.BreedClassID);

            if (breedClassEntry == null || breedClassEntry.BourseTypeID.HasValue == false)
            {
                return null;
            }
            return CommonDataCacheProxy.Instanse.GetCacheCM_BourseTypeByKey((int)breedClassEntry.BourseTypeID);
        }
        #endregion

        #region 12.根据商品代码返回所属交易所
        /// <summary>
        /// 根据商品代码返回所属交易所
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static CM_BourseType GetCM_BourseTypeByCommodityCode(string code)
        {
            CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (commodity == null || commodity.BreedClassID.HasValue == false)
            {
                return null;
            }
            CM_BreedClass breedClassEntry = CommonDataCacheProxy.Instanse.GetCacheCM_BreedClassByKey((int)commodity.BreedClassID);

            if (breedClassEntry == null || breedClassEntry.BourseTypeID.HasValue == false)
            {
                return null;
            }
            return CommonDataCacheProxy.Instanse.GetCacheCM_BourseTypeByKey((int)breedClassEntry.BourseTypeID);
        }
        #endregion

        #region 13.根据价格获取最小变动价位
        /// <summary>
        /// 根据价格获取最小变动价位
        /// </summary>
        /// <param name="price">价格</param>
        /// <returns></returns>
        public static HK_MinPriceFieldRange GetMinPriceFieldByPrice(decimal price)
        {

            List<HK_MinPriceFieldRange> minrange = CommonDataCacheProxy.Instanse.GetAllHKMinChangePriceFieldRange();
            foreach (HK_MinPriceFieldRange item in minrange)
            {
                //价位（靠后不靠前）（港元）
                if (price > item.LowerLimit.Value && price <= item.UpperLimit.Value)
                {
                    return item;
                }
            }
            return null;

        }
        #endregion

        #region 14.根据期货Code判断是否为新品种期货合约上市当日 add by 董鹏 2010-01-25
        /// <summary>
        /// 根据期货Code判断是否为新品种期货合约上市
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsNewBreedClassByCode(string code)
        {
            CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (commodity == null && !commodity.BreedClassID.HasValue)
            {
                return false;
            }
            //List<CM_Commodity> itemsAll =CommonDataCacheProxy.Instanse.GetAllCommodity();
            List<CM_Commodity> items = new List<CM_Commodity>();
            items = CommonDataCacheProxy.Instanse.GetCacheCommodityByBreedClassID(commodity.BreedClassID.Value);
            //foreach (CM_Commodity item in itemsAll)
            //{
            //    if (item.BreedClassID == commodity.BreedClassID)
            //    {
            //        items.Add(item);
            //    }
            //}
            //为了防止items==null
            if (items != null && items.Count == 1)
            {
                if (commodity.MarketDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    return true;
                }
            }
            else
            {
                //是否有比当前更早的代码
                bool flag = false;
                //为了防止items==null
                if (items != null)
                {
                    foreach (CM_Commodity item in items)
                    {
                        if (item.CommodityCode.CompareTo(commodity.CommodityCode) < 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    if (commodity.MarketDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 15.根据期货Code判断是否为新月份期货合约上市当日 add by 董鹏 2010-01-25
        /// <summary>
        /// 根据商品Code判断是否为新月份期货合约上市
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static bool IsNewMonthBreedClassByCode(string code)
        {
            CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (commodity == null)
            {
                return false;
            }
            //List<CM_Commodity> itemsAll = GetAllCommonDataFromManagerCenter.GetAllCommodity();
            List<CM_Commodity> items = new List<CM_Commodity>();
            items = CommonDataCacheProxy.Instanse.GetCacheCommodityByBreedClassID(commodity.BreedClassID.Value);
            //foreach (CM_Commodity item in itemsAll)
            //{
            //    if (item.BreedClassID == commodity.BreedClassID)
            //    {
            //        items.Add(item);
            //    }
            //}
            //为了防止items==null
            //同品种有1个以上的代码，并且有比当前代码更早的
            if (items != null && items.Count > 1)
            {
                bool flag = false;
                foreach (CM_Commodity item in items)
                {
                    if (item.CommodityCode.CompareTo(commodity.CommodityCode) < 0)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    if (commodity.MarketDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 16.根据期货Code判断是否为交割月份 add by 董鹏 2010-01-26
        /// <summary>
        /// 判断当前月是否合约交割月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <returns></returns>
        public static bool IsDeliveryMonth(string code)
        {
            int year;
            int month;
            GetAgreementTime(code, out year, out month);
            if (year == DateTime.Now.Year && month == DateTime.Now.Month)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 17.根据期货Code获取合约年月 add by 董鹏 2010-01-26
        /// <summary>
        /// 获取合约的年，月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public static void GetAgreementTime(string code, out int year, out int month)
        {
            string str = code.Substring(code.Length - 4);
            if (!int.TryParse(str, out year))
            {
                //对1位年份的代码进行处理
                str = DateTime.Now.Year.ToString().Substring(2, 1) + str.Substring(1, 3);
            }
            string yearStr = str.Substring(0, 2);
            yearStr = "20" + yearStr;
            string monthStr = str.Substring(2, 2);

            year = int.Parse(yearStr);
            month = int.Parse(monthStr);
        }
        #endregion

        #region 18.根据期货合约代码判断是否为最后交易日 add by 董鹏 2010-03-05
        /// <summary>
        /// 根据期货合约代码判断是否为最后交易日
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsLastTradingDayContract(string code)
        {
            int year;
            int month;
            GetAgreementTime(code, out year, out month);
            int LastTradingDay = GetLastTradingDay(code);
            DateTime date = new DateTime(year, month, LastTradingDay);

            while (!JudgmentIsTrandingDay(date, code))
            {
                date = date.AddDays(1);
            }
            if (DateTime.Now.Date == date.Date)
            {
                return true;
            }
            return false;
        }


        #endregion

        #region 19.根据期货合约代码判断是否为季月合约上市首日 add by 董鹏 2010-03-05
        /// <summary>
        /// 根据期货合约代码判断是否为季月合约上市首日
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <returns></returns>
        public static bool IsNewQuarterMonthContract(string code)
        {
            //if (!IsNewMonthBreedClassByCode(code) && !IsNewBreedClassByCode(code))
            //{
            //    return false;
            //}

            CM_Commodity cm = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            if (cm.MarketDate.Date != DateTime.Now.Date)
            {
                return false;
            }

            int year;
            int month;
            GetAgreementTime(code, out year, out month);
            switch (DateTime.Now.Month)
            {
                case 1:
                    if (year == DateTime.Now.Year && month == 9)
                    {
                        return true;
                    }
                    break;
                case 4:
                    if (year == DateTime.Now.Year && month == 12)
                    {
                        return true;
                    }
                    break;
                case 7:
                    if (year == DateTime.Now.AddYears(1).Year && month == 3)
                    {
                        return true;
                    }
                    break;
                case 10:
                    if (year == DateTime.Now.AddYears(1).Year && month == 6)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
        #endregion

        #region 20.获取最后交易日  add by 董鹏 2010-03-05

        /// <summary>
        /// 获取当前月份最后交易日
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int GetLastTradingDay(string code)
        {
            //CM_Commodity commodity = CommonDataCacheProxy.Instanse.GetCacheCommodityByCode(code);
            //if (commodity == null || commodity.BreedClassID.HasValue == false)
            //{
            //    throw new VTException("IsExpireLastedTradeDate", "无法获取期货合约代码对应的商品");
            //}          
            //CM_BreedClass cmBreedClass = CommonDataCacheProxy.Instanse.GetCacheCM_BreedClassByKey((int)commodity.BreedClassID);
            //if (cmBreedClass == null)
            //{
            //    throw new VTException("IsExpireLastedTradeDate", "无法获取期货合约代码对应的商品类型");
            //}

            QH_FuturesTradeRules qhFuturesTradeRules = GetQH_FuturesTradeRulesByCommodityCode(code);// FuturesTradeRules.GetFuturesTradeRulesByBreedClassID(cmBreedClass.BreedClassID);
            if (qhFuturesTradeRules == null)
            {
                throw new VTException("IsExpireLastedTradeDate", "无法获取期货商品类型对应的交易规则");
            }

            // QH_LastTradingDay qhLastTradingDay = ManagementCenterDataAgent.Instanse.GetFutureTradeRulesInstanse().GetLastTradingDayByLastTradingDayID((int)qhFuturesTradeRules.LastTradingDayID);
            QH_LastTradingDay qhLastTradingDay = CommonDataCacheProxy.Instanse.GetCacheQH_LastTradingDayByID((int)qhFuturesTradeRules.LastTradingDayID);
            if (qhLastTradingDay == null)
            {
                throw new VTException("IsExpireLastedTradeDate", "无法获取期货商品类型对应的最后交易日");
            }

            return GetLastTradingDay(qhLastTradingDay, code);
        }

        /// <summary>
        /// 获取当前月份最后交易日
        /// </summary>
        /// <param name="LastTradingDayEntity"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static int GetLastTradingDay(QH_LastTradingDay LastTradingDayEntity, string code)
        {
            //交易所代码
            CM_BourseType _CMBourseType = GetCM_BourseTypeByCommodityCode(code);
            int bourseTypeID = _CMBourseType.BourseTypeID;

            switch ((int)LastTradingDayEntity.LastTradingDayTypeID)
            {
                //第几天
                case (int)Types.QHLastTradingDayType.DeliMonthAndDay:
                    return DeliMonthOfDay(LastTradingDayEntity, code);
                // return int.MaxValue;
                //倒数或者顺数第几个交易日
                case (int)Types.QHLastTradingDayType.DeliMonthAndDownOrShunAndWeek:
                    return DeliMonthOfTurnOrBackTrandingDay(LastTradingDayEntity, bourseTypeID);
                // return int.MaxValue;
                //交割月份的前一个月份的倒数或者顺数第几个交易日
                case (int)Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay:
                    return DeliMonthOfAgoMonthTradeDay(LastTradingDayEntity, bourseTypeID);
                // return int.MaxValue;
                //第几周的星期几
                case (int)Types.QHLastTradingDayType.DeliMonthAndWeek:
                    return DeliMonthOfWeekDay(LastTradingDayEntity, bourseTypeID);
                // return int.MaxValue;
            }

            return 0;
        }

        /// <summary>
        /// 类型为第几日(自然日)，求最后交易日，如果当天为非交易日，往后顺延
        /// </summary>
        /// <param name="LastTradingDay"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static int DeliMonthOfDay(QH_LastTradingDay LastTradingDay, string code)
        {
            int day = (int)LastTradingDay.WhatDay;

            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;

            DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, day));
            while (!JudgmentIsTrandingDay(dt, code))
            {
                dt = dt.AddDays(1);
                if (dt.Month != CurrentMonth) break;
            }

            if (dt.Month == CurrentMonth) return dt.Day;

            return int.MaxValue;
        }

        /// <summary>
        /// 类型为第几周的星期几，求最后交易日
        /// </summary>
        /// <param name="LastTradingDay"></param>
        /// <param name="bourseTypeID"></param>
        /// <returns></returns>
        private static int DeliMonthOfWeekDay(QH_LastTradingDay LastTradingDay, int bourseTypeID)
        {
            if (LastTradingDay == null) return int.MaxValue;
            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;
            return getDay(CurrentYear, CurrentMonth, (int)LastTradingDay.WhatWeek, (int)LastTradingDay.Week);
        }

        /// <summary>
        /// 类型为倒数或者顺数第几个交易日，求最后交易日
        /// </summary>
        /// <param name="LastTradingDay"></param>
        /// <param name="bourseTypeID"></param>
        /// <returns></returns>
        private static int DeliMonthOfTurnOrBackTrandingDay(QH_LastTradingDay LastTradingDay, int bourseTypeID)
        {
            int day = (int)LastTradingDay.WhatDay;

            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;

            // CM_NotTradeDateDAL NotTradeDateDAL = new CM_NotTradeDateDAL();

            int temp = 0;


            #region 已经有缓存了
            ////根据品种获取当前月份里面的非交易日列表
            //CM_BreedClass breedClassEntry = CommonDataCacheProxy.Instanse.GetCacheCM_BreedClassByKey(bourseTypeID);
            //IList<CM_NotTradeDate> List_CM_NotTradeDate = ManagementCenterDataAgent.Instanse.GetComonParaInstanse().GetNotTradeDateByBourseTypeID(breedClassEntry.BourseTypeID.Value);
            #endregion

            #region 根据类型求出最后交易日

            if (LastTradingDay.Sequence == (int)Types.QHLastTradingDayIsSequence.Order)
            {
                for (int i = 1; i <= DateTime.DaysInMonth(CurrentYear, CurrentMonth); i++)
                {
                    DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, i));
                    bool falg = false;
                    #region 从缓存中查询
                    //foreach (CM_NotTradeDate date in List_CM_NotTradeDate)
                    //{
                    //    if (((DateTime)date.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                    //    {
                    //        falg = true;
                    //        break;
                    //    }
                    //}
                    CM_NotTradeDate cm_noDate = CommonDataCacheProxy.Instanse.GetCacheNotTradeDateByKey(bourseTypeID + "@" + dt.ToString("yyyy-MM-dd"));
                    if (cm_noDate != null)
                    {
                        falg = true;
                    }
                    #endregion

                    if (!falg)
                    {
                        temp = temp + 1;
                        if (temp == day) return i;
                    }
                }
            }
            else
            {
                for (int i = DateTime.DaysInMonth(CurrentYear, CurrentMonth); i >= 1; i--)
                {
                    DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, i));
                    bool falg = false;
                    #region 从缓存中查询
                    //foreach (CM_NotTradeDate date in List_CM_NotTradeDate)
                    //{
                    //    if (((DateTime)date.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                    //    {
                    //        falg = true;
                    //        break;
                    //    }
                    //}  
                    CM_NotTradeDate cm_noDate = CommonDataCacheProxy.Instanse.GetCacheNotTradeDateByKey(bourseTypeID + "@" + dt.ToString("yyyy-MM-dd"));
                    if (cm_noDate != null)
                    {
                        falg = true;
                    }
                    #endregion
                    if (!falg)
                    {
                        temp = temp + 1;
                        if (temp == day) return i;
                    }
                }
            }

            #endregion

            return int.MaxValue;
        }

        /// <summary>
        /// 类型为交割月份的前一个月份的倒数或者顺数第几个交易日，求最后交易日
        /// </summary>
        /// <param name="LastTradingDay"></param>
        /// <param name="bourseTypeID"></param>
        /// <returns></returns>
        private static int DeliMonthOfAgoMonthTradeDay(QH_LastTradingDay LastTradingDay, int bourseTypeID)
        {
            return DeliMonthOfTurnOrBackTrandingDay(LastTradingDay, bourseTypeID);
        }

        /// <summary>
        /// 判断该天是否交易
        /// </summary>
        /// <param name="dt">指定的日期</param>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static bool JudgmentIsTrandingDay(DateTime dt, string code)
        {
            return CommonDataManagerOperate.IsTradeDate(code, dt);
        }

        /// <summary>
        /// 根据某年某月第几周星期几得到为该月的几号
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="weekNO"></param>
        /// <param name="weekDay">星期日为0</param>
        /// <returns></returns>
        public static int getDay(int year, int month, int weekNO, int weekDay)
        {
            DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, 1));
            DayOfWeek t = dt.DayOfWeek;
            int days = 0;
            int temp = 0;
            switch (t)
            {
                case DayOfWeek.Sunday:
                    temp = 0;
                    break;
                case DayOfWeek.Monday:
                    temp = 1;
                    break;
                case DayOfWeek.Tuesday:
                    temp = 2;
                    break;
                case DayOfWeek.Wednesday:
                    temp = 3;
                    break;
                case DayOfWeek.Thursday:
                    temp = 4;
                    break;
                case DayOfWeek.Friday:
                    temp = 5;
                    break;
                case DayOfWeek.Saturday:
                    temp = 6;
                    break;
            }
            if (weekNO == 1)
            {
                return days = weekDay + 1 - temp;
            }
            //return days = 7 - temp + (weekNO - 2) * 7 + weekDay + 1;
            if (temp == 0 || temp == 6)
            {
                return days = 7 - temp + (weekNO - 2) * 7 + weekDay + 1 + 7;//当本月的第一天是星期六或星期天时，向后顺延一周

            }
            else
            {
                return days = 7 - temp + (weekNO - 2) * 7 + weekDay + 1;

            }
        }

        /// <summary>
        /// 指定时间是否在交易日内
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="aDate">指定时间</param>
        /// <returns>是否在交易日内</returns>
        public static bool IsTradeDate(string code, DateTime aDate)
        {
            bool result = false;
            CM_BourseType bourseType = GetCM_BourseTypeByCommodityCode(code);
            if (bourseType != null)
            {
                result = IsTradeDate(bourseType.BourseTypeID, aDate);
            }

            return result;
        }

        /// <summary>
        /// 指定时间是否在交易日内
        /// </summary>
        /// <param name="bourseTypeID"></param>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public static bool IsTradeDate(int bourseTypeID, DateTime aDate)
        {
            bool result = true;
            try
            {
                #region 从缓存中查询即可，指定时间不在非交易日期中就是交易日了
                // IList<CM_NotTradeDate> notTradeDates = ManagementCenterDataAgent.Instanse.GetComonParaInstanse().GetNotTradeDateByBourseTypeID(bourseTypeID);

                //if (notTradeDates == null || notTradeDates.Count == 0)
                //{
                //    //string errCode = "GT-8033";
                //    //string errMsg = "无法从管理中心获取非交易日列表。";
                //    //throw new VTException(errCode, errMsg);
                //    return result;
                //}

                //var dates = from date in notTradeDates
                //            where date.NotTradeDay.HasValue
                //            select date.NotTradeDay.Value;

                //var ds = from d in dates
                //         where d.Year == aDate.Year && d.Month == aDate.Month && d.Day == aDate.Day
                //         select d;

                //if (ds.Count() > 0)
                //    result = false;

                CM_NotTradeDate cm_noDate = CommonDataCacheProxy.Instanse.GetCacheNotTradeDateByKey(bourseTypeID + "@" + aDate.ToString("yyyy-MM-dd"));
                if (cm_noDate != null)
                {
                    result = false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        #endregion


    }
}
