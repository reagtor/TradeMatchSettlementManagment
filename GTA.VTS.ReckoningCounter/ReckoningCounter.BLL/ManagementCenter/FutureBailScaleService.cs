#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using CommonRealtimeMarket.factory;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.FuturesDevolveService;
using RealTime.Server.SModelData.HqData;
using GTA.VTS.Common.CommonUtility;
using System.Linq;
#endregion

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 商品期货保证金比例服务类,错误编码范围8481-8490
    /// 作者：宋涛
    /// 日期：2008-12-05
    /// Update By:李健华
    /// Update Date:2010-03-02
    /// Desc.:修改保证金的相关逻辑，根据业务逻辑编码
    /// </summary>
    public static class FutureBailScaleService
    {
        /// <summary>
        /// 获取合约保证金
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <returns>保证金比例</returns>
        public static decimal GetFutureBailScale(string code)
        {
            string errCode = "GT-8481";
            string errMsg = code + "商品期货商品代码不存在。";

            CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null)
            {
                throw new VTException(errCode, errMsg);
            }

            if (commodity.BreedClassID.HasValue == false)
            {
                errMsg = "商品期货商品代码品种类型为null。";
                throw new VTException(errCode, errMsg);
            }

            errCode = "GT-8482";
            errMsg = "无法根据期货商品代码获取其对应的保证金比例。";

            int year;
            int month;

            try
            {
                FutureService.GetAgreementTime(code, out year, out month);
            }
            catch (Exception ex)
            {
                throw new VTException(errCode, errMsg, ex);
            }


            DateTime now = DateTime.Now;

            errCode = "GT-8483";
            errMsg = code + "合约已经过期。";

            decimal bailScace = -1;
            ////合约已过期
            //if (year < now.Year)
            //{
            //    throw new VTException(errCode, errMsg);
            //}

            //合约已经过期
            if (FutureService.CheckQHContractIsExpired(code))
            {
                LogHelper.WriteDebug("Debug_Test_Bail_001:合约已经过期正在按交割月份比例获取" + code + "保证金比例");
                //过期合约直接返回交易割月份的其中一记录的比例,即按交易月份获取
                //这是为了盘后清算在这样的需要
                bailScace = ProcessCurrentYear(code, year, now.Month, now, commodity.BreedClassID.Value);

                return bailScace;
            }

            #region 如果是正在清算的时候要调整这个区间值所以要把日期向后推
            if (ScheduleManagement.ScheduleManager.IsFutureReckoning)
            {
                //每日收盘后，如果某期货合约在T+1日所处期间符合调整交易保证金要求的，
                //该期货合约的所有持仓在T+0日收盘结算时按照新的交易保证金标准收取相应的交易保证金。
                //例如合约SR1011自交割月份前一个月第11日交易保证金由8%调整到15%，
                //则在第11日的前一个交易日收盘结算时就按照15%的保证金比例收取交易保证金。
                #region     如果是正在清算的时候要调整这个区间值即
                //如果当前正在清算并且是正在做故障恢复清算那么时间应该是做故障恢复清算的时间
                if (ScheduleManagement.ScheduleManager.IsFaultRecoveryFutureReckoning)
                {
                    now = ScheduleManagement.ScheduleManager.CurrentFaultRecoveryFutureReckoningDate;
                }
                now = FutureService.GetNowDayBackwardTradeDay(now, commodity.BreedClassID.Value);
                //当计算的日期回来是下一年的时候如当前2010-12-31那么1012这个合约的推算下一个交易日为11年1月份
                //或者月份也超过了交割月也不用调整
                //那么这时就不能再调整这个日期，直接按交割月当前日期计算即可
                if (now.Year > year || now.Month > month)
                {
                    now = DateTime.Now;
                }
                #endregion

            }
            #endregion

            //本年度合约
            if (year == now.Year)
            {
                //合约已过期
                if (month < now.Month)
                {
                    throw new VTException(errCode, errMsg);
                }
                LogHelper.WriteDebug("Debug_Test_Bail_01:正在获取商品期货代码本年度合约" + code + "保证金比例");

                bailScace = ProcessCurrentYear(code, year, month, now, commodity.BreedClassID.Value);
                LogHelper.WriteDebug("Debug_Test_Bail_End:获取到合约" + code + "保证金比例" + bailScace);
                return bailScace;

            }

            //下年度合约
            if (year > now.Year)
            {
                LogHelper.WriteDebug("Debug_Test_Bail_01:正在获取商品期货代码下年度合约" + code + "保证金比例");
                bailScace = ProcessNextYear(code, year, month, now, commodity.BreedClassID.Value);
                LogHelper.WriteDebug("Debug_Test_Bail_End:获取到合约" + code + "保证金比例" + bailScace);
                return bailScace;

            }
            LogHelper.WriteDebug("Debug_Test_Bail_End:合约" + code + "无类型保证金比例");
            return -1;
        }

        /// <summary>
        /// 处理本年度合约保证金
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">合约年</param>
        /// <param name="month">合约月</param>
        /// <param name="compareDate">要比较的日期</param>
        /// <param name="breedClassID">合约代码所属品种类型</param>
        /// <returns>保证金比例</returns>
        private static decimal ProcessCurrentYear(string code, int year, int month, DateTime compareDate, int breedClassID)
        {
            //DateTime now = DateTime.Now;
            DateTime now = compareDate;

            //当前合约处于交割月
            if (month == now.Month)
            {
                LogHelper.WriteDebug("Debug_Test_Bail_02:当前合约处于交割月" + code);

                return ProcessDeliveryMonth(code, now, breedClassID);
            }

            //当前合约还未到交割月
            if (month > now.Month)
            {
                LogHelper.WriteDebug("Debug_Test_Bail_02:当前合约还未到交割月" + code);

                return ProcessNonDeliveryMonth(code, year, month, now, breedClassID);
            }

            return -1;
        }

        /// <summary>
        /// 处理下年度合约保证金
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">合约年</param>
        /// <param name="month">合约月</param>
        /// <param name="compareDate">要比较的日期</param>
        /// <param name="breedClassID">合约代码所属品种类型</param>
        /// <returns>保证金比例</returns>
        private static decimal ProcessNextYear(string code, int year, int month, DateTime compareDate, int breedClassID)
        {
            //当前合约还未到交割月
            return ProcessNonDeliveryMonth(code, year, month, compareDate, breedClassID);
        }

        /// <summary>
        /// 处理交割月合约保证金
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="compareDate">要比较的日期</param>
        /// <param name="breedClassID">合约代码所属品种类型</param>
        /// <returns>保证金比例</returns>
        private static decimal ProcessDeliveryMonth(string code, DateTime compareDate, int breedClassID)
        {
            string errCode = "GT-8482";
            string errMsg = "无法根据期货商品代码获取其对应的保证金比例。";


            IList<QH_CFBailScaleValue> values = GetBailScaleValues(code, breedClassID);
            if (values == null)
            {
                throw new VTException(errCode, errMsg);
            }



            //如果是过期合约不用获取行情的持仓量，这是为了盘前检查，因为交割月的比例计算没有用到行情持仓量
            //持仓量----这里为了不用修改后面的代码设置如果合约是过期时直接给予市场持仓量为最大值，这是为了盘前检查时要计算一些强制平仓过期合约而设置
            //因为过期后的合约直接取交割月的其中一个比例，而这里设置这个持仓量是为了不知道此代码所设置的保证金比例计算方式是以天计算还是双边、单边计算
            decimal openInterest = int.MaxValue;
            if (!FutureService.CheckQHContractIsExpired(code))
            {
                IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
                //FutData futData = service.GetFutData(code);
                MerFutData merFutData = service.GetMercantileFutData(code);
                if (merFutData == null)
                {
                    errMsg = "处理交割月合约保证金时无法获取行情。";
                    throw new VTException(errCode, errMsg);
                }
                //持仓量
                openInterest = (decimal)merFutData.OpenInterest;
            }


            Types.QHCFPositionMonthType monthType = Types.QHCFPositionMonthType.OnDelivery;
            IList<QH_CFBailScaleValue> scaleValues = FindScaleValuesByMonthType(values, monthType);
            if (scaleValues.Count != 0)
            {
                return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
            }

            return -1;
        }

        /// <summary>
        /// 通过持仓和保证金控制类型来获取具体的保证金比例
        /// </summary>
        /// <param name="scaleValues">保证金比例列表</param>
        /// <param name="openInterest">持仓</param>
        /// <param name="compareDate">要比较的日期</param>
        /// <param name="breedClassID">合约代码所属品种类型</param>
        /// <returns>保证金比例</returns>
        private static decimal GetScaleByBailType(IList<QH_CFBailScaleValue> scaleValues, decimal openInterest, DateTime compareDate, int breedClassID)
        {
            if (scaleValues != null && scaleValues[0].PositionBailTypeID.HasValue)
            {
                LogHelper.WriteDebug("Debug_Test_Bail_04:保证金控制类型" + scaleValues[0].PositionBailTypeID.Value);
            }

            switch (scaleValues[0].PositionBailTypeID.Value)
            {
                case (int)Types.QHPositionBailType.SinglePosition:
                    return GetScale(scaleValues, openInterest / 2, breedClassID);

                case (int)Types.QHPositionBailType.TwoPosition:
                    return GetScale(scaleValues, openInterest, breedClassID);
                case (int)Types.QHPositionBailType.ByDays:
                case (int)Types.QHPositionBailType.ByTradeDays:
                    #region
                    //{
                    //int day = DateTime.Now.Day;
                    ////如果是正在清算的时候要调整这个区间值
                    //if (ScheduleManagement.ScheduleManager.IsFutureReckoning)
                    //{
                    //    //每日收盘后，如果某期货合约在T+1日所处期间符合调整交易保证金要求的，
                    //    //该期货合约的所有持仓在T+0日收盘结算时按照新的交易保证金标准收取相应的交易保证金。
                    //    //例如合约SR1011自交割月份前一个月第11日交易保证金由8%调整到15%，
                    //    //则在第11日的前一个交易日收盘结算时就按照15%的保证金比例收取交易保证金。
                    //    #region     如果是正在清算的时候要调整这个区间值即
                    //    DateTime faultRecoveryTime = DateTime.Now;
                    //    faultRecoveryTime = FutureService.GetNowDayBackwardTradeDay(faultRecoveryTime);
                    //    day = faultRecoveryTime.Day;
                    //    #endregion
                    //}
                    #endregion
                    return GetScale(scaleValues, compareDate.Day, breedClassID);
                //}
            }

            return -1;
        }


        /// <summary>
        /// 通过持仓和保证金控制类型来获取具体的保证金比例
        /// </summary>
        /// <param name="values">保证金比例列表</param>
        /// <param name="findVal">类型值（持仓或者日期）</param>
        /// <param name="breedClassID">合约代码所属品种类型</param>
        /// <returns>保证金比例</returns>
        private static decimal GetScale(IList<QH_CFBailScaleValue> values, decimal findVal, int breedClassID)
        {

            QH_CFBailScaleValue scaleValue = null;
            //不用再过滤直接，因为之前的缓存已经作过处理
            //List<QH_CFBailScaleValue> lists = AssemblingFilterCFBail(values);
            // foreach (QH_CFBailScaleValue value in lists)
            foreach (QH_CFBailScaleValue value in values)
            {
                if (CheckScaleFieldRange(findVal, value))
                {
                    scaleValue = value;
                    break;
                }
            }
            QH_SIFBail sifBail = MCService.FuturesTradeRules.GetSIFBailByBreedClassID(breedClassID);
            if (sifBail == null)
            {
                LogHelper.WriteDebug("Debug_Test_Bail_05:无法获取最低交易保证金比例");
                return -1;
            }
            LogHelper.WriteDebug("Debug_Test_Bail_05:获取最低交易保证金比例" + sifBail.BailScale);

            if (scaleValue == null)
            {
                LogHelper.WriteDebug("Debug_Test_Bail_06:无法获取保证金比例区间范围值,返回最低保证金");
                return sifBail.BailScale;
            }
            if (scaleValue.BailScale.Value < sifBail.BailScale)
            {
                LogHelper.WriteDebug("Debug_Test_Bail_06:获取保证金比例区间范围值比最低保证金比例还低直接返回最低保证金比例");
            }

            return scaleValue.BailScale.Value;
        }

        /// <summary>
        /// 输入值是否在当前字段范围内
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="fieldRange">字段范围</param>
        /// <returns>是否在当前字段范围内</returns>
        public static bool CheckScaleFieldRange(decimal value, QH_CFBailScaleValue fieldRange)
        {

            bool result = false;
            if (!fieldRange.Start.HasValue && !fieldRange.Ends.HasValue)
            {
                return result;
            }
            if (fieldRange.Start.HasValue)
            {
                result = fieldRange.LowerLimitIfEquation.Value == (int)Types.IsYesOrNo.Yes
                                                  ? value >= fieldRange.Start
                                                  : value > fieldRange.Start;
                if (result)
                {
                    if (fieldRange.Ends.HasValue)
                    {
                        result = fieldRange.UpperLimitIfEquation.Value == (int)Types.IsYesOrNo.Yes
                                     ? value <= fieldRange.Ends
                                     : value < fieldRange.Ends;
                    }
                }
            }
            else if (fieldRange.Ends.HasValue)
            {
                result = fieldRange.UpperLimitIfEquation.Value == (int)Types.IsYesOrNo.Yes
                             ? value <= fieldRange.Ends
                             : value < fieldRange.Ends;
            }
            return result;
        }

        /// <summary>
        /// 根据交割月份类型来获取对应的保证金比例列表
        /// </summary>
        /// <param name="values">保证金比例列表</param>
        /// <param name="monthType">交割月份类型</param>
        /// <returns>对应交割月份类型的保证金比例列表</returns>
        private static IList<QH_CFBailScaleValue> FindScaleValuesByMonthType(IList<QH_CFBailScaleValue> values, Types.QHCFPositionMonthType monthType)
        {
            IList<QH_CFBailScaleValue> scaleValues = new List<QH_CFBailScaleValue>();
            foreach (QH_CFBailScaleValue value in values)
            {
                if (value.DeliveryMonthType.Value == (int)monthType)
                {
                    scaleValues.Add(value);
                }
            }
            LogHelper.WriteDebug("Debug_Test_Bail_03:根据交割月份类型来获取对应的保证金比例列表" + monthType.ToString() + "获取列表" + scaleValues.Count);
            return scaleValues;
        }

        /// <summary>
        /// 处理费交割月份的合约
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">合约年</param>
        /// <param name="month">合约月</param>
        /// <param name="compareDate">要比较的日期</param>
        /// <param name="breedClassID">合约代码所属品种类型</param>
        /// <returns>保证金比例</returns>
        private static decimal ProcessNonDeliveryMonth(string code, int year, int month, DateTime compareDate, int breedClassID)
        {
            string errCode = "GT-8482";
            string errMsg = "无法根据期货商品代码获取其对应的保证金比例。";


            IList<QH_CFBailScaleValue> values = GetBailScaleValues(code, breedClassID);
            if (values == null || values.Count <= 0)
            {
                throw new VTException(errCode, errMsg);
            }

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            //FutData futData = service.GetFutData(code);
            MerFutData merFutData = service.GetMercantileFutData(code);
            if (merFutData == null)
            {
                errMsg = "处理交割月合约保证金时无法获取行情。";
                throw new VTException(errCode, errMsg);
            }

            //持仓量
            decimal openInterest = (decimal)merFutData.OpenInterest;

            //判断当前月份是那种交割月份类型
            Types.QHCFPositionMonthType monthType = FutureService.CheckMonthType(year, month, compareDate);

            IList<QH_CFBailScaleValue> scaleValues;

            #region  前三个月交割月份
            //如果是前三
            if (monthType == Types.QHCFPositionMonthType.OnDeliAgoThreeMonth)
            {
                scaleValues = FindScaleValuesByMonthType(values, monthType);

                //是前三
                if (scaleValues.Count > 0)
                {
                    return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
                }

                //否则就是一般月份
                scaleValues = FindScaleValuesByMonthType(values, Types.QHCFPositionMonthType.GeneralMonth);
                if (scaleValues.Count > 0)
                {
                    return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
                }
            }
            #endregion

            #region 前二
            //如果是前二
            if (monthType == Types.QHCFPositionMonthType.OnDeliAgoTwoMonth)
            {
                scaleValues = FindScaleValuesByMonthType(values, monthType);

                //是前二
                if (scaleValues.Count > 0)
                {
                    return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
                }

                //否则就是一般月份
                scaleValues = FindScaleValuesByMonthType(values, Types.QHCFPositionMonthType.GeneralMonth);
                if (scaleValues.Count > 0)
                {
                    return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
                }
            }
            #endregion

            #region 前一

            //如果是前一
            if (monthType == Types.QHCFPositionMonthType.OnDeliAgoMonth)
            {
                scaleValues = FindScaleValuesByMonthType(values, monthType);

                //是前一
                if (scaleValues.Count > 0)
                {
                    return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
                }

                //否则就是一般月份
                scaleValues = FindScaleValuesByMonthType(values, Types.QHCFPositionMonthType.GeneralMonth);
                if (scaleValues.Count > 0)
                {
                    return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
                }
            }
            #endregion

            #region 一般月份
            //如果是一般月份
            if (monthType == Types.QHCFPositionMonthType.GeneralMonth)
            {
                scaleValues = FindScaleValuesByMonthType(values, monthType);

                //是一般月份
                if (scaleValues.Count > 0)
                {
                    return GetScaleByBailType(scaleValues, openInterest, compareDate, breedClassID);
                }
            }
            #endregion

            #region 最低保证金
            QH_SIFBail sifBail = MCService.FuturesTradeRules.GetSIFBailByBreedClassID(breedClassID);
            if (sifBail == null)
            {
                LogHelper.WriteError("Debug_Test_Bail_ProcessNonDeliveryMonth_01:无法获取最低交易保证金比例", new Exception(""));
                return -1;
            }
            else
            {
                LogHelper.WriteDebug("Debug_Test_Bail_ProcessNonDeliveryMonth_02:无对应的保证金比例区间范围值获取最低交易保证金比例" + sifBail.BailScale);

                return sifBail.BailScale;
            }
            #endregion

            //return -1;
        }




        /// <summary>
        /// 根据合约代码获取其保证金比例列表
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="breedClassID">合约代码所属品种类型</param>
        /// <returns>保证金比例列表</returns>
        private static IList<QH_CFBailScaleValue> GetBailScaleValues(string code, int breedClassID)
        {
            //CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            //if (breedClass == null)
            //    return null;

            return MCService.FuturesTradeRules.GetCFBailScaleValueByBreedClassID(breedClassID);
        }



    }
}