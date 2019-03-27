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
using ReckoningCounter.Entity;
using GTA.VTS.Common.CommonUtility;

#endregion

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 商品期货持仓限制服务类,错误编码范围8491-8499
    /// 作者：宋涛
    /// 日期：2008-12-05
    /// Update By：李健华
    /// Update Date:2010-01-28
    /// Desc.:修改持仓限获取相关的方法, 需求改变重新编码修改
    /// </summary>
    public static class FuturePositionLimitService
    {
        /// <summary>
        /// 获取合约持仓限制
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="positionValueType">持仓限制类型</param>
        /// <returns>持仓限制</returns>
        public static PositionLimitValueInfo GetFuturePostionLimit(string code, out Types.QHPositionValueType positionValueType)
        {
            string errCode = "GT-8491";
            string errMsg = "期货商品代码不存在。";

            CM_Commodity commodity = MCService.CommonPara.GetCommodityByCommodityCode(code);
            if (commodity == null)
                throw new VTException(errCode, errMsg);

            errCode = "GT-8492";
            errMsg = "无法根据期货商品代码获取其对应的持仓限制。";

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

            errCode = "GT-8493";
            errMsg = code + "合约已经过期。";
            positionValueType = Types.QHPositionValueType.Scales;

            //合约已过期
            if (year < now.Year)
            {
                throw new VTException(errCode, errMsg);
            }

            //本年度合约
            if (year == now.Year)
            {
                //合约已过期
                if (month < now.Month)
                {
                    throw new VTException(errCode, errMsg);
                }

                return ProcessCurrentYear(code, year, month, out positionValueType);
            }

            //下年度合约
            if (year > now.Year)
            {
                return ProcessNextYear(code, year, month, out positionValueType);
            }
            PositionLimitValueInfo info = new PositionLimitValueInfo();
            info.PositionValue = -1;
            info.IsMinMultiple = false;
            info.MinMultipleValue = 0;
            //return -1;
            return info;
        }
        #region 本年度合约持仓限制
        /// <summary>
        /// 处理本年度合约持仓限制
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">合约年</param>
        /// <param name="month">合约月</param>
        /// <param name="positionValueType">持仓限制类型</param>
        /// <returns>持仓限制</returns>
        private static PositionLimitValueInfo ProcessCurrentYear(string code, int year, int month, out Types.QHPositionValueType positionValueType)
        {
            DateTime now = DateTime.Now;
            positionValueType = Types.QHPositionValueType.Scales;


            //当前合约处于交割月
            if (month == now.Month)
            {
                return ProcessDeliveryMonth(code, out positionValueType);
            }

            //当前合约还未到交割月
            if (month > now.Month)
            {
                return ProcessNonDeliveryMonth(code, year, month, out positionValueType);
            }

            PositionLimitValueInfo info = new PositionLimitValueInfo();
            info.PositionValue = -1;
            info.IsMinMultiple = false;
            info.MinMultipleValue = 0;
            //return -1;
            return info;
        }
        #endregion
        /// <summary>
        /// 处理下年度合约持仓限制
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">合约年</param>
        /// <param name="month">合约月</param>
        /// <param name="positionValueType">持仓限制类型</param>
        /// <returns>持仓限制</returns>
        private static PositionLimitValueInfo ProcessNextYear(string code, int year, int month, out Types.QHPositionValueType positionValueType)
        {
            //当前合约还未到交割月
            return ProcessNonDeliveryMonth(code, year, month, out positionValueType);
        }

        /// <summary>
        /// 处理交割月合约持仓限制
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="positionValueType">持仓限制类型</param>
        /// <returns>持仓限制</returns>
        private static PositionLimitValueInfo ProcessDeliveryMonth(string code, out Types.QHPositionValueType positionValueType)
        {
            string errCode = "GT-8492";
            string errMsg = "无法根据商品期货商品代码获取其对应的持仓限制。";


            //根据合约代码获取其持仓限制列表
            IList<QH_PositionLimitValue> values = GetPositionLimitValues(code);
            if (values == null)
            {
                throw new VTException(errCode, errMsg);
            }

            #region 获取行情持仓量
            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            //FutData futData = service.GetFutData(code);
            MerFutData merFutData = service.GetMercantileFutData(code);
            if (merFutData == null)
            {
                errMsg = "处理交割月合约保证金时无法获取行情-持仓限制。";
                throw new VTException(errCode, errMsg);
            }

            //持仓量
            decimal openInterest = (decimal)merFutData.OpenInterest;
            LogHelper.WriteDebug("交割月份的合约当前获取到" + code + "行情持仓量：" + openInterest);
            #endregion

            #region 过滤所属持仓限制类型记录
            Types.QHCFPositionMonthType monthType = Types.QHCFPositionMonthType.OnDelivery;
            IList<QH_PositionLimitValue> limitValues = FindLimitValuesByMonthType(values, monthType);
            #endregion

            positionValueType = Types.QHPositionValueType.Scales;
            if (limitValues.Count != 0)
            {
                return GetLimitByBailType(limitValues, openInterest, out positionValueType);
            }

            throw new VTException(errCode, errMsg);
            // return -1;
        }




        /// <summary>
        /// 通过持仓限制列表和市场持仓量、持仓控制类型来获取具体的持仓限制
        /// </summary>
        /// <param name="limitValues">持仓限制列表</param>
        /// <param name="openInterest">市场持仓量</param>
        /// <param name="positionValueType">持仓限制类型</param>
        /// <returns>持仓限制</returns>
        private static PositionLimitValueInfo GetLimitByBailType(IList<QH_PositionLimitValue> limitValues, decimal openInterest, out Types.QHPositionValueType positionValueType)
        {
            PositionLimitValueInfo info = new PositionLimitValueInfo();
            switch (limitValues[0].PositionBailTypeID.Value)
            {
                //单边
                case (int)Types.QHPositionBailType.SinglePosition:
                    {
                        info = GetLimit(limitValues, openInterest / 2, out positionValueType);
                        info.QHPositionBailType = Types.QHPositionBailType.SinglePosition;
                        return info;
                    }
                //又边
                case (int)Types.QHPositionBailType.TwoPosition:
                    {
                        info = GetLimit(limitValues, openInterest, out positionValueType);
                        info.QHPositionBailType = Types.QHPositionBailType.TwoPosition;
                        return info;
                    }
                //按交易日天数据
                case (int)Types.QHPositionBailType.ByTradeDays:
                //按自然日天数据
                case (int)Types.QHPositionBailType.ByDays:
                    {
                        //如果是按天的话就先把当前系统日期的当前月中所有的交易日获取
                        //然后对这些交易日再查询 (然而这里已经在获取管理中心数据缓存时已经转换过来，所以这里不用再转换，
                        //因为缓存了的每次获取就不用再计算转换
                        info = GetLimit(limitValues, DateTime.Now.Day, out positionValueType);
                        info.QHPositionBailType = Types.QHPositionBailType.ByDays;
                        return info;
                    }
            }

            positionValueType = Types.QHPositionValueType.Scales;


            info.PositionValue = -1;
            info.IsMinMultiple = false;
            info.MinMultipleValue = 0;
            //return -1;
            return info;
        }

        /// <summary>
        /// 通过持仓和保证金控制类型来获取具体的持仓限制
        /// </summary>
        /// <param name="values">持仓限制列表</param>
        /// <param name="findVal">类型值（持仓或者日期）</param>
        /// <param name="positionValueType">持仓限制取值类型(百分比还是值）</param>
        /// <returns>持仓限制</returns>
        /// 
        private static PositionLimitValueInfo GetLimit(IList<QH_PositionLimitValue> values, decimal findVal, out Types.QHPositionValueType positionValueType)
        {
            QH_PositionLimitValue limitValue = null;
            PositionLimitValueInfo info = new PositionLimitValueInfo();
            info.PositionValue = -1;
            info.IsMinMultiple = false;
            info.MinMultipleValue = 0;

            foreach (QH_PositionLimitValue value in values)
            {
                if (CheckLimitFieldRange(findVal, value))
                {
                    limitValue = value;
                    break;
                }
            }

            positionValueType = Types.QHPositionValueType.Scales;
            if (limitValue == null)
            {
                //return -1;
                return info;
            }

            positionValueType = (Types.QHPositionValueType)limitValue.PositionValueTypeID.Value;

            //如果有值即为要对本记录作最小交割单位倍数判断
            if (limitValue.MinUnitLimit.HasValue && limitValue.MinUnitLimit.Value != 0)
            {
                info.IsMinMultiple = true;
                info.MinMultipleValue = (decimal)limitValue.MinUnitLimit.Value;
            }
            info.PositionValue = limitValue.PositionValue.Value;
            return info;
        }

        /// <summary>
        /// 输入值是否在当前字段范围内
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="fieldRange">字段范围</param>
        /// <returns>是否在当前字段范围内</returns>
        public static bool CheckLimitFieldRange(decimal value, QH_PositionLimitValue fieldRange)
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
        /// 根据交割月份类型来获取对应的持仓限制列表
        /// </summary>
        /// <param name="values">持仓限制列表</param>
        /// <param name="monthType">交割月份类型</param>
        /// <returns>对应交割月份类型的持仓限制列表</returns>
        private static IList<QH_PositionLimitValue> FindLimitValuesByMonthType(IList<QH_PositionLimitValue> values, Types.QHCFPositionMonthType monthType)
        {
            IList<QH_PositionLimitValue> limitValues = new List<QH_PositionLimitValue>();
            foreach (QH_PositionLimitValue value in values)
            {
                if (value.DeliveryMonthType == (int)monthType)
                {
                    limitValues.Add(value);
                }
            }

            return limitValues;
        }

        /// <summary>
        /// 处理非交割月份的合约
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">合约年</param>
        /// <param name="month">合约月</param>
        /// <param name="positionValueType">持仓限制类型</param>
        /// <returns>保证金比例</returns>
        private static PositionLimitValueInfo ProcessNonDeliveryMonth(string code, int year, int month, out Types.QHPositionValueType positionValueType)
        {
            string errCode = "GT-8492";
            string errMsg = "无法根据期货商品代码获取其对应的持仓限制。";


            IList<QH_PositionLimitValue> values = GetPositionLimitValues(code);
            if (values == null)
                throw new VTException(errCode, errMsg);

            IRealtimeMarketService service = RealTimeMarketUtil.GetRealMarketService(); //RealtimeMarketServiceFactory.GetService();
            //FutData futData = service.GetFutData(code);
            MerFutData merFutData = service.GetMercantileFutData(code);
            if (merFutData == null)
            {
                errMsg = "处理交割月合约保证金时无法获取行情-持仓限制。";
                throw new VTException(errCode, errMsg);
            }

            //持仓量
            decimal openInterest = (decimal)merFutData.OpenInterest;

            LogHelper.WriteDebug("非交割月份的合约当前获取到" + code + "行情持仓量：" + openInterest);

            //判断当前月份是那种交割月份类型
            Types.QHCFPositionMonthType monthType = FutureService.CheckMonthType(year, month, DateTime.Now);

            IList<QH_PositionLimitValue> limitValues;
            positionValueType = Types.QHPositionValueType.Scales;

            #region 交割月份前三个月
            //如果是前三
            if (monthType == Types.QHCFPositionMonthType.OnDeliAgoThreeMonth)
            {
                limitValues = FindLimitValuesByMonthType(values, monthType);

                //是前三
                if (limitValues.Count > 0)
                {
                    return GetLimitByBailType(limitValues, openInterest, out positionValueType);
                }
                errMsg = "代码" + code + " 无法获取交割前三个月的对应的持仓限制列表参数正在以一般月份查询。";
                LogHelper.WriteDebug(errCode + errMsg);

                //否则就是一般月份
                limitValues = FindLimitValuesByMonthType(values, Types.QHCFPositionMonthType.GeneralMonth);
                if (limitValues.Count > 0)
                {
                    PositionLimitValueInfo generOnThree = GetLimitByBailType(limitValues, openInterest, out positionValueType);
                    if (generOnThree.PositionValue == -1)
                    {
                        generOnThree.IsNoComputer = true;
                    }
                    return generOnThree;

                }
                errMsg = "代码" + code + " 无法获取交割月份前三个月的一般月份的对应的持仓限制列表参数。";
                LogHelper.WriteDebug(errCode + errMsg);
            }
            #endregion

            #region 交割月份前二个月
            //如果是前二
            if (monthType == Types.QHCFPositionMonthType.OnDeliAgoTwoMonth)
            {
                limitValues = FindLimitValuesByMonthType(values, monthType);

                //是前二
                if (limitValues.Count > 0)
                {
                    return GetLimitByBailType(limitValues, openInterest, out positionValueType);
                }
                errMsg = "代码" + code + " 无法获取交割前两个月的对应的持仓限制列表参数。";
                LogHelper.WriteDebug(errCode + errMsg);
                //否则就是一般月份
                limitValues = FindLimitValuesByMonthType(values, Types.QHCFPositionMonthType.GeneralMonth);
                if (limitValues.Count > 0)
                {
                    PositionLimitValueInfo generOnDeliAgo = GetLimitByBailType(limitValues, openInterest, out positionValueType);
                    if (generOnDeliAgo.PositionValue == -1)
                    {
                        generOnDeliAgo.IsNoComputer = true;
                    }
                    return generOnDeliAgo;
                }
                errMsg = "代码" + code + " 无法获取交割月份前两个月的一般月份的对应的持仓限制列表参数。";
                LogHelper.WriteDebug(errCode + errMsg);

            }
            #endregion

            #region 交割月份前一个月
            //如果是前一
            if (monthType == Types.QHCFPositionMonthType.OnDeliAgoMonth)
            {
                limitValues = FindLimitValuesByMonthType(values, monthType);

                //是前一
                if (limitValues.Count > 0)
                {
                    return GetLimitByBailType(limitValues, openInterest, out positionValueType);
                }
                errMsg = "代码" + code + " 无法获取交割前一个月的对应的持仓限制列表参数正在以一般月份查询。";
                LogHelper.WriteDebug(errCode + errMsg);



                //否则就是一般月份
                limitValues = FindLimitValuesByMonthType(values, Types.QHCFPositionMonthType.GeneralMonth);
                if (limitValues.Count > 0)
                {
                    PositionLimitValueInfo generOnDeli = GetLimitByBailType(limitValues, openInterest, out positionValueType);
                    if (generOnDeli.PositionValue == -1)
                    {
                        generOnDeli.IsNoComputer = true;
                    }
                    return generOnDeli;
                }
                errMsg = "代码" + code + " 无法获取交割月份前一个月的一般月份的对应的持仓限制列表参数。";
                LogHelper.WriteDebug(errCode + errMsg);

            }
            #endregion

            #region 交割月份一般月份
            //如果是一般月份
            if (monthType == Types.QHCFPositionMonthType.GeneralMonth)
            {
                limitValues = FindLimitValuesByMonthType(values, monthType);

                //是一般月份
                if (limitValues.Count > 0)
                {
                    PositionLimitValueInfo gener = GetLimitByBailType(limitValues, openInterest, out positionValueType);
                    if (gener.PositionValue == -1)
                    {
                        gener.IsNoComputer = true;
                    }
                    return gener;
                }
                errMsg = "代码" + code + " 无法获取一般月份的对应的持仓限制列表参数。";
                LogHelper.WriteDebug(errCode + errMsg);
            }
            #endregion

            PositionLimitValueInfo info = new PositionLimitValueInfo();
            info.PositionValue = -1;
            info.IsMinMultiple = false;
            info.MinMultipleValue = 0;
            //return -1;
            return info;
        }


        /// <summary>
        /// 根据合约代码获取其持仓限制列表
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <returns>持仓限制列表</returns>
        private static IList<QH_PositionLimitValue> GetPositionLimitValues(string code)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            if (breedClass == null)
                return null;

            return
                MCService.FuturesTradeRules.GetPositionLimitValueByBreedClassID(breedClass.BreedClassID);
        }
    }
}