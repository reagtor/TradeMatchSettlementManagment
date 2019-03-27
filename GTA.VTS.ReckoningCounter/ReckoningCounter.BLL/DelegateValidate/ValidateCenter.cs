#region Using Namespace

using System;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateValidate.Local;
using ReckoningCounter.BLL.Delegatevalidate.ManagementCenter;
using ReckoningCounter.BLL.DelegateValidate.ManagementCenter;
using ReckoningCounter.BLL.ManagementCenter;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate
{
    /// <summary>
    /// 柜台校验的入口，包含内部和外部校验
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public static class ValidateCenter
    {
        #region 委托验证方法

        /// <summary>
        /// 股票校验
        /// </summary>
        /// <param name="request">股票委托</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验成功失败结果</returns>
        public static bool Validate(StockOrderRequest request, ref string errMsg)
        {
            try
            {
                //内部校验
                LocalStockValidater local = new LocalStockValidater(request);
                bool result = local.Check(out errMsg);

                //外部校验
                if (result)
                {
                    result = McValidater.GetInstance().ValidateStockTradeRule(request, ref errMsg);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }
        /// <summary>
        /// 港股校验
        /// </summary>
        /// <param name="request">港股委托</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验成功失败结果</returns>
        public static bool Validate(HKOrderRequest request, ref string errMsg)
        {
            try
            {
                //内部校验
                LocalHKStockValidater local = new LocalHKStockValidater(request);
                bool result = local.Check(out errMsg);

                //外部校验
                if (result)
                {
                    result = McValidater.GetInstance().ValidateHKStockTradeRule(request, ref errMsg);
                }

                return result;
                //return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }

        /// <summary>
        /// 期货校验
        /// </summary>
        /// <param name="request">期货合约</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验成功失败结果</returns>
        public static bool Validate(MercantileFuturesOrderRequest request, ref string errMsg)
        {
            try
            {
                //内部校验
                LocalFutureOpenValidater localFutureOpenValidater = new LocalFutureOpenValidater(request);
                bool result = localFutureOpenValidater.Check(out errMsg, Types.BreedClassTypeEnum.CommodityFuture);

                //外部校验
                if (result)
                {
                    result = McValidater.GetInstance().ValidateFutureTradeRule(request, ref errMsg);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }

        /// <summary>
        /// 股指期货校验
        /// </summary>
        /// <param name="request">股指期货合约</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验成功失败结果</returns>
        public static bool Validate(StockIndexFuturesOrderRequest request, ref string errMsg)
        {
            try
            {
                //内部校验
                LocalFutureOpenValidater localFutureOpenValidater = new LocalFutureOpenValidater(request);
                bool result = localFutureOpenValidater.Check(out errMsg, Types.BreedClassTypeEnum.StockIndexFuture);

                //外部校验
                if (result)
                {
                    result = McValidater.GetInstance().ValidateStockIndexFutureTradeRule(request, ref errMsg);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }

        #endregion

        #region 撤单验证方法(不再使用，由下单实现)

        /// <summary>
        /// 股票撤单校验
        /// </summary>
        /// <param name="orderNo">委托单号</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验成功失败结果</returns>
        [Obsolete("不再使用，由下单实现")]
        public static bool ValidateStockCancel(string orderNo, ref string errMsg)
        {
            errMsg = "";

            LocalStockCacelValidater validater = new LocalStockCacelValidater(orderNo);
            bool result = validater.Check(ref errMsg);

            return result;
        }

        /// <summary>
        /// 期货撤单校验
        /// </summary>
        /// <param name="orderNo">委托单号</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验成功失败结果</returns>
        [Obsolete("不再使用，由下单实现")]
        public static bool ValidateFutureCancel(string orderNo, ref string errMsg)
        {
            errMsg = "";

            LocalFutureCacelValidater validater = new LocalFutureCacelValidater(orderNo);
            bool result = validater.Check(ref errMsg);

            return result;
        }

        /// <summary>
        /// 股指期货撤单校验
        /// </summary>
        /// <param name="orderNo">委托单号</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>校验成功失败结果</returns>
        [Obsolete("不再使用，由下单实现")]
        public static bool ValidateStockIndexCancel(string orderNo, ref string errMsg)
        {
            errMsg = "";

            LocalFutureCacelValidater validater = new LocalFutureCacelValidater(orderNo);
            bool result = validater.Check(ref errMsg);

            return result;
        }

        #endregion

        #region 交易日期、时间判断

        /// <summary>
        /// 验证是否在柜台接受委托时间内（不能用于港股代码的查询）
        /// </summary>
        /// <param name="code">具体的交易代码</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否在接受委托时间内</returns>
        public static bool IsCountTradingTime(string code, ref string errMsg)
        {
            #region old code
            //CM_BourseType bourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(code);

            //if(bourseType == null)
            //{
            //    errMsg = "无法获取相关交易所信息。";
            //    return false;
            //}

            //if(!bourseType.CounterFromSubmitStartTime.HasValue || !bourseType.CounterFromSubmitEndTime.HasValue)
            //{
            //    errMsg = "无法获取交易所开收市时间。";
            //    return false;
            //}

            //// 柜台接收委托开始时间
            //DateTime countBeginTime = bourseType.CounterFromSubmitStartTime.Value;

            //// 柜台接收委托结束时间
            //DateTime countEndTime = bourseType.CounterFromSubmitEndTime.Value;

            //DateTime now = DateTime.Now;

            //errMsg = "";

            ////是否正在清算，在清算时不能接受委托
            //bool isReckoning = ScheduleManagement.ScheduleManager.IsStockReckoning ||
            //                   ScheduleManagement.ScheduleManager.IsFutureReckoning;

            //if(isReckoning)
            //    return false;

            ////然后判断是否在柜台的接受委托时间内
            //if(Utils.CompareTime(countBeginTime,now)<0 && Utils.CompareTime(countEndTime,now)>0)
            //{
            //    return true;
            //}

            //return false;
            #endregion

            return IsCountTradingTime(Types.BreedClassTypeEnum.Stock, code, ref errMsg);
        }

        /// <summary>
        /// 验证是否在柜台接受委托时间内
        /// </summary>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <param name="code">具体的交易代码</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否在接受委托时间内</returns>
        public static bool IsCountTradingTime(Types.BreedClassTypeEnum type, string code, ref string errMsg)
        {

            CM_BourseType bourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(code, type);
            if (bourseType == null)
            {
                errMsg = "无法获取相关交易所信息。";
                return false;
            }

            if (!bourseType.CounterFromSubmitStartTime.HasValue || !bourseType.CounterFromSubmitEndTime.HasValue)
            {
                errMsg = "无法获取交易所开收市时间。";
                return false;
            }

            // 柜台接收委托开始时间
            DateTime countBeginTime = bourseType.CounterFromSubmitStartTime.Value;

            // 柜台接收委托结束时间
            DateTime countEndTime = bourseType.CounterFromSubmitEndTime.Value;

            DateTime now = DateTime.Now;

            errMsg = "";

            //是否正在清算，在清算时不能接受委托
            bool isReckoning = ScheduleManagement.ScheduleManager.IsStockReckoning ||
                               ScheduleManagement.ScheduleManager.IsFutureReckoning ||
                               ScheduleManagement.ScheduleManager.IsHKReckoning;

            if (isReckoning)
            {
                errMsg = "当前柜台正在执行清算";
                return false;
            }

            if (ScheduleManagement.ScheduleManager.IsFutureReckoningErrorStopTrade)
            {
                errMsg = "当前柜台上一日清算异常，暂停交易";
                return false;
            }

            //然后判断是否在柜台的接受委托时间内
            if (Utils.CompareTime(countBeginTime, now) < 0 && Utils.CompareTime(countEndTime, now) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 验证是否在撮合中心接受委托时间内（注：所有相关于根据商品代码查询相关的公共参数数据
        /// 的方法，当港股时应使用相应的重载方法，而些原来的方法会默认为原来的查询方法即CM_Commodity的数据返回相关的关系键值）
        /// </summary>
        /// <param name="code">具体的交易代码</param>
        /// <returns>是否在接受委托时间内</returns>
        public static bool IsMatchTradingTime(string code)
        {
            #region old code
            //CM_BourseType bourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(code);

            //// 撮合接收委托开始时间
            //DateTime matchBeginTime = bourseType.ReceivingConsignStartTime.Value;
            //matchBeginTime = SetNowDate(matchBeginTime);

            //// 撮合接收委托结束时间
            //DateTime matchEndTime = bourseType.ReceivingConsignEndTime.Value;
            //matchEndTime = SetNowDate(matchEndTime);

            //DateTime now = DateTime.Now;

            //if (Utils.CompareTime(matchBeginTime, now) <= 0 && Utils.CompareTime(matchEndTime, now) >= 0)
            //{
            //    return true;
            //}

            //return false;
            #endregion
            return IsMatchTradingTime(Types.BreedClassTypeEnum.Stock, code);
        }

        /// <summary>
        /// 验证是否在撮合中心接受委托时间内
        /// </summary>
        /// <param name="type">商品所属类别用来为了区分查询港股代码</param>
        /// <param name="code">具体的交易代码</param>
        /// <returns>是否在接受委托时间内</returns>
        public static bool IsMatchTradingTime(Types.BreedClassTypeEnum type, string code)
        {
            CM_BourseType bourseType = MCService.CommonPara.GetBourseTypeByCommodityCode(code, type);
            if (bourseType == null)
            {
                return false;
            }
            // 撮合接收委托开始时间
            DateTime matchBeginTime = bourseType.ReceivingConsignStartTime.Value;
            matchBeginTime = SetNowDate(matchBeginTime);

            // 撮合接收委托结束时间
            DateTime matchEndTime = bourseType.ReceivingConsignEndTime.Value;
            matchEndTime = SetNowDate(matchEndTime);

            DateTime now = DateTime.Now;

            if (Utils.CompareTime(matchBeginTime, now) <= 0 && Utils.CompareTime(matchEndTime, now) >= 0)
            {
                return true;
            }

            return false;
        }

        private static DateTime SetNowDate(DateTime time)
        {
            DateTime newTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hour,
                                            time.Minute, time.Second);
            return newTime;
        }

        /// <summary>
        /// 是否在交易日内
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>是否在交易日内</returns>
        public static bool IsTradeDate(string code)
        {
            return MCService.CommonPara.IsTradeDate(code);
        }

        #endregion

        #region 费用公式

        /// <summary>
        /// 根据商品代码，商品类型获取某一现货商品的费用计算公式
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>StockRuleContainer</returns>
        public static StockRuleContainer GetStockRuleContainerByCode(string code)
        {
            return McValidater.GetInstance().GetStockRuleContainerByCode(code);
        }

        /// <summary>
        /// 根据商品代码，商品类型获取某一期货商品的费用计算公式
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>FutureRuleContainer</returns>
        public static FutureRuleContainer GetFutureRuleContainerByCode(string code)
        {
            return McValidater.GetInstance().GetFutureRuleContainerByCode(code);
        }

        #endregion

        #region 最小交易单位校验
        //TODO:最小交易单位校验
        /// <summary>
        /// 最小交易单位校验，原来在外部校验中，但是为了避免与调用方重复调用数据库读取持仓，现在抽离出来
        /// 供调用者直接调用
        /// </summary>
        /// <param name="request">委托</param>
        /// <param name="position">当前委托对应的商品的持仓量</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否校验成功</returns>
        public static bool ValidateStockMinVolumeOfBusiness(StockOrderRequest request, int position, ref string errMsg)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code);

            bool result = false;

            if (breedClass != null)
            {
                int breedClassTypeID = breedClass.BreedClassTypeID.Value;
                int breedClassID = breedClass.BreedClassID;

                if (breedClassTypeID != (int)Types.BreedClassTypeEnum.Stock)
                    return false;

                StockMinVolumeOfBusinessCommand command = new StockMinVolumeOfBusinessCommand(breedClassID, position);
                result = command.Validate(request, ref errMsg);
            }

            return result;
        }

        #endregion

        #region 港股最小交易单位校验
        /// <summary>
        /// 港股最小交易单位校验
        /// 供调用者直接调用
        /// </summary>
        /// <param name="request">委托</param>
        /// <param name="position">当前委托对应的商品的持仓量</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否校验成功</returns>
        public static bool ValidateHKMinVolumeOfBusiness(HKOrderRequest request, int position, ref string errMsg)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(request.Code, Types.BreedClassTypeEnum.HKStock);

            bool result = false;

            if (breedClass != null)
            {
                int breedClassTypeID = breedClass.BreedClassTypeID.Value;
                int breedClassID = breedClass.BreedClassID;

                if (breedClassTypeID != (int)Types.BreedClassTypeEnum.HKStock)
                    return false;

                HKStockMinVolumeOfBusinessCommand command = new HKStockMinVolumeOfBusinessCommand(breedClassID, position);
                result = command.Validate(request, ref errMsg);
            }

            return result;
        }

        #endregion

        #region
        /// <summary>
        /// 港股改单校验
        /// </summary>
        /// <param name="request">改单请求对象</param>
        /// <param name="entrusInfo">原委托单对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>返回检验是否成功</returns>
        public static bool Validate(HKModifyOrderRequest request, ReckoningCounter.Entity.Model.HK.HK_TodayEntrustInfo entrusInfo, ref string errMsg)
        {
            try
            {
                bool result = McValidater.GetInstance().ValidateHKModifyOrderRule(request, entrusInfo, ref errMsg);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString(), ex);
                return false;
            }
        }
        #endregion
    }
}