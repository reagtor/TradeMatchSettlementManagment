#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.DevolveVerifyCommonService;
using ReckoningCounter.DAL.FuturesDevolveService;
using ReckoningCounter.BLL.Common;
using System.Linq;
using GTA.VTS.Common.CommonUtility;

#endregion

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 管理中心的FuturesTradeRules缓存包装类，错误编码8200-8299
    /// 作者：宋涛
    /// 日期：2008-12-07
    /// </summary>
    public class FuturesTradeRulesProxy
    {
        #region IFuturesTradeRulse List

        private WCFCacheObjectWithGetKey<int, IList<QH_AgreementDeliveryMonth>> agreementDeliveryMonthByBreedClassIDObj;

        private WCFCacheObjectWithGetAll<int, QH_AgreementDeliveryMonth> agreementDeliveryMonthObj;

        private WCFCacheObjectWithGetAll<int, QH_CFPositionMonth> allCFPostionMonthObj;

        private WCFCacheObjectWithGetAll<int, QH_ConsignInstructionType> allConsionInstructionTypeObj;

        private WCFCacheObjectWithGetAll<int, QH_PositionBailType> allPositionBailTypeObj;

        private WCFCacheObjectWithGetAll<int, QH_PositionLimitValue> allQHPositionLimitValueObj;

        private WCFCacheObjectWithGetKey<int, IList<QH_CFBailScaleValue>> cFBailScaleValueByBreedClassIDObj;

        private WCFCacheObjectWithGetAll<int, QH_CFBailScaleValue> cFBailScaleValueObj;

        private WCFCacheObjectWithGetAll<int, QH_ConsignQuantum> consignQuantumObj;

        private WCFCacheObjectWithGetAll<int, QH_FutureCosts> futureCostsObj;

        private WCFCacheObjectWithGetAll<int, QH_FuturesTradeRules> futureTradeRulsObj;

        private WCFCacheObjectWithGetAll<int, QH_HighLowStopScopeType> highLowStopScopeTypeObj;

        private WCFCacheObjectWithGetAll<int, QH_LastTradingDay> lastTradingDayObj;

        private WCFCacheObjectWithGetAll<int, QH_LastTradingDayType> lastTradingDayTypeObj;

        private WCFCacheObjectWithGetAll<int, QH_Month> monthObj;

        private WCFCacheObjectWithGetKey<int, IList<QH_PositionLimitValue>> positionLimitValueByBreedClassIDObj;

        private WCFCacheObjectWithGetAll<int, QH_SIFBail> sifBailObj;

        private WCFCacheObjectWithGetAll<int, QH_SIFPosition> sifPositionObj;

        private WCFCacheObjectWithGetKey<int, IList<QH_SingleRequestQuantity>> singleRequestQuantityByConsignQuantumIDObj;

        private WCFCacheObjectWithGetAll<int, QH_SingleRequestQuantity> singleRequestQuantityObj;

        #endregion

        #region SyncRoot

        #endregion

        private static FuturesTradeRulesProxy instance = new FuturesTradeRulesProxy();

        private FuturesTradeRulesProxy()
        {
            agreementDeliveryMonthByBreedClassIDObj
                =
                new WCFCacheObjectWithGetKey<int, IList<QH_AgreementDeliveryMonth>>(
                    GetAgreementDeliveryMonthByBreedClassIDFromWCF);

            agreementDeliveryMonthObj =
                new WCFCacheObjectWithGetAll<int, QH_AgreementDeliveryMonth>(GetAllAgreementDeliveryMonthFromWCF,
                                                                             val => val.AgreementDeliveryMonthID);

            allCFPostionMonthObj =
                new WCFCacheObjectWithGetAll<int, QH_CFPositionMonth>(GetAllCFPostionMonthFromWCF,
                                                                      val => val.DeliveryMonthTypeID);

            allConsionInstructionTypeObj =
                new WCFCacheObjectWithGetAll<int, QH_ConsignInstructionType>(GetAllConsionInstructionTypeFromWCF,
                                                                             val => val.ConsignInstructionTypeID);

            allPositionBailTypeObj =
                new WCFCacheObjectWithGetAll<int, QH_PositionBailType>(GetAllPositionBailTypeFromWCF,
                                                                       val => val.PositionBailTypeID);

            allQHPositionLimitValueObj =
                new WCFCacheObjectWithGetAll<int, QH_PositionLimitValue>(GetAllQHPositionLimitValueFromWCF,
                                                                         val => val.PositionLimitValueID);

            cFBailScaleValueByBreedClassIDObj =
                new WCFCacheObjectWithGetKey<int, IList<QH_CFBailScaleValue>>(GetCFBailScaleValueByBreedClassIDFromWCF);

            cFBailScaleValueObj =
                new WCFCacheObjectWithGetAll<int, QH_CFBailScaleValue>(GetAllCFBailScaleValueFromWCF,
                                                                       val => val.CFBailScaleValueID);

            consignQuantumObj =
                new WCFCacheObjectWithGetAll<int, QH_ConsignQuantum>(GetAllConsignQuantumFromWCF,
                                                                     val => val.ConsignQuantumID);

            futureCostsObj =
                new WCFCacheObjectWithGetAll<int, QH_FutureCosts>(GetAllFutureCostsFromWCF, val => val.BreedClassID);

            futureTradeRulsObj =
                new WCFCacheObjectWithGetAll<int, QH_FuturesTradeRules>(GetAllFuturesTradeRulesFromWCF,
                                                                        val => val.BreedClassID);

            highLowStopScopeTypeObj =
                new WCFCacheObjectWithGetAll<int, QH_HighLowStopScopeType>(GetAllHighLowStopScopeTypeFromWCF,
                                                                           val => val.HighLowStopScopeID);

            lastTradingDayObj =
                new WCFCacheObjectWithGetAll<int, QH_LastTradingDay>(GetAllLastTradingDayFromWCF,
                                                                     val => val.LastTradingDayID);

            lastTradingDayTypeObj =
                new WCFCacheObjectWithGetAll<int, QH_LastTradingDayType>(GetAllLastTradingDayTypeFromWCF,
                                                                         val => val.LastTradingDayTypeID);

            monthObj =
                new WCFCacheObjectWithGetAll<int, QH_Month>(GetAllMonthFromWCF, val => val.MonthID);

            positionLimitValueByBreedClassIDObj =
                new WCFCacheObjectWithGetKey<int, IList<QH_PositionLimitValue>>(
                    GetPositionLimitValueByBreedClassIDFromWCF);

            sifBailObj =
                new WCFCacheObjectWithGetAll<int, QH_SIFBail>(GetAllSIFBailFromWCF, val => val.BreedClassID);

            sifPositionObj =
                new WCFCacheObjectWithGetAll<int, QH_SIFPosition>(GetAllSIFPositionFromWCF, val => val.BreedClassID);


            singleRequestQuantityByConsignQuantumIDObj =
                new WCFCacheObjectWithGetKey<int, IList<QH_SingleRequestQuantity>>(
                    GetSingleRequestQuantityByConsignQuantumIDFromWCF);

            singleRequestQuantityObj =
                new WCFCacheObjectWithGetAll<int, QH_SingleRequestQuantity>(GetAllSingleRequestQuantityFromWCF,
                                                                            val => val.SingleRequestQuantityID);
        }

        public static FuturesTradeRulesProxy GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// 进行预加载
        /// </summary>
        public void Initialize()
        {
            GetAllFuturesTradeRules();
            GetAllFutureCosts();
            GetAllCFPostionMonth();
            GetAllPositionBailType();
            GetAllSIFBail();
            GetAllSIFPosition();
            GetAllSingleRequestQuantity();
            GetAllConsionInstructionType();
            GetAllCFBailScaleValue();
        }

        public void Reset()
        {
            #region IFuturesTrade

            agreementDeliveryMonthByBreedClassIDObj.Reset();

            agreementDeliveryMonthObj.Reset();

            allCFPostionMonthObj.Reset();

            allConsionInstructionTypeObj.Reset();

            allPositionBailTypeObj.Reset();

            allQHPositionLimitValueObj.Reset();

            cFBailScaleValueByBreedClassIDObj.Reset();
            cFBailScaleValueObj.Reset();

            consignQuantumObj.Reset();

            futureCostsObj.Reset();

            futureTradeRulsObj.Reset();

            highLowStopScopeTypeObj.Reset();

            lastTradingDayObj.Reset();

            lastTradingDayTypeObj.Reset();

            monthObj.Reset();

            positionLimitValueByBreedClassIDObj.Reset();

            sifBailObj.Reset();

            sifPositionObj.Reset();


            singleRequestQuantityByConsignQuantumIDObj.Reset();

            singleRequestQuantityObj.Reset();

            #endregion
        }

        #region IFuturesTradeRulse

        private FuturesTradeRulesClient GetClient()
        {
            FuturesTradeRulesClient client;
            try
            {
                client = new FuturesTradeRulesClient();
            }
            catch (Exception ex)
            {
                string errCode = "GT-8200";
                string errMsg = "无法获取管理中心提供的服务[IFuturesTradeRules]";
                throw new VTException(errCode, errMsg, ex);
            }

            return client;
        }

        /// <summary>
        /// 获取所有的合约交割月份
        /// </summary>
        /// <returns></returns>
        public IList<QH_AgreementDeliveryMonth> GetAllAgreementDeliveryMonth()
        {
            return GetAllAgreementDeliveryMonth(false);
        }

        private IList<QH_AgreementDeliveryMonth> GetAllAgreementDeliveryMonth(bool reLoad)
        {
            return agreementDeliveryMonthObj.GetAll(reLoad);
        }

        private IList<QH_AgreementDeliveryMonth> GetAllAgreementDeliveryMonthFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetALLAgreementDeliveryMonth();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8201";
                    string errMsg = "无法从管理中心获取所有的合约交割月份。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_AgreementDeliveryMonth GetAgreementDeliveryMonthByDeliveryMonthID(int agreementDeliveryMonthID)
        {
            return agreementDeliveryMonthObj.GetByKey(agreementDeliveryMonthID);
        }

        public IList<QH_AgreementDeliveryMonth> GetAgreementDeliveryMonthByBreedClassID(int breedClassID)
        {
            return agreementDeliveryMonthByBreedClassIDObj.GetByKey(breedClassID);
        }

        private IList<QH_AgreementDeliveryMonth> GetAgreementDeliveryMonthByBreedClassIDFromWCF(int breedClassID)
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAgreementDeliveryMonthByBreedClassID(breedClassID);
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8202";
                    string errMsg = "无法根据商品品种编码从管理中心获取所有的合约交割月份。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }


        public IList<QH_CFBailScaleValue> GetAllCFBailScaleValue()
        {
            return GetAllCFBailScaleValue(false);
        }

        private IList<QH_CFBailScaleValue> GetAllCFBailScaleValue(bool reLoad)
        {
            return cFBailScaleValueObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的品种_商品期货_保证金比例
        /// </summary>
        /// <returns></returns>
        private IList<QH_CFBailScaleValue> GetAllCFBailScaleValueFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllCFBailScaleValue();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8203";
                    string errMsg = "无法从管理中心获取所有的商品期货保证金列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_CFBailScaleValue GetCFBailScaleValueByCFBailScaleValueID(int bailScaleValueID)
        {
            return cFBailScaleValueObj.GetByKey(bailScaleValueID);
        }


        private IList<QH_CFBailScaleValue> GetCFBailScaleValueByBreedClassIDFromWCF(int breedClassID)
        {

            {
                IList<QH_CFBailScaleValue> list = null;
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                    {
                        list = client.GetCFBailScaleValueByBreedClassID(breedClassID);
                        try
                        {
                            list = AssemblingFilterCFBail(list);
                        }
                        catch (Exception ex)
                        {
                            string errCode = "GT-8203";
                            string errMsg = "重新过滤和组装保证金数据异常。";
                            throw new VTException(errCode, errMsg, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8203";
                    string errMsg = "无法从管理中心获取所有的商品期货保证金列表。";
                    throw new VTException(errCode, errMsg, ex);
                }

                return list;
            }
        }
        /// <summary>
        /// 重新过滤和组装保证金数据（这里是为了查找相关的最后交易日前第几日的问题）
        /// </summary>
        /// <param name="values">原保证金比例列表</param>
        /// <returns></returns>
        private static List<QH_CFBailScaleValue> AssemblingFilterCFBail(IList<QH_CFBailScaleValue> values)
        {
            string errCode = "GT-8485";
            string errMsg = "保证金比例数据设置有异常。";
            List<QH_CFBailScaleValue> list = new List<QH_CFBailScaleValue>();
            Dictionary<int, DateTime> modifyTime = new Dictionary<int, DateTime>();
            // List<QH_CFBailScaleValue> models = new List<QH_CFBailScaleValue>();
            //QH_CFBailScaleValue[] kd = new QH_CFBailScaleValue[values.Count];
            //values.CopyTo(kd, 0);
            //models = kd.ToList<QH_CFBailScaleValue>();



            try
            {
                foreach (var item in values)
                {
                    QH_CFBailScaleValue retList = new QH_CFBailScaleValue();

                    #region 重新定义一对象记录
                    //retList = item;//对象引用不能这样附值会把缓存的数据修改
                    retList.BailScale = item.BailScale;
                    retList.BreedClassID = item.BreedClassID;
                    retList.CFBailScaleValueID = item.CFBailScaleValueID;
                    retList.DeliveryMonthType = item.DeliveryMonthType;
                    retList.Ends = item.Ends;
                    retList.LowerLimitIfEquation = item.LowerLimitIfEquation;
                    retList.PositionBailTypeID = item.PositionBailTypeID;
                    retList.RelationScaleID = item.RelationScaleID;
                    retList.Start = item.Start;
                    retList.UpperLimitIfEquation = item.UpperLimitIfEquation;
                    #endregion

                    #region linq查询方法 查询相关记录
                    //查询是否在关联的保证金比例列表，有则要计算当前日志（即月份的交易日期）
                    var q = from c in values
                            where
                                 c.RelationScaleID == item.CFBailScaleValueID
                            select c;
                    #endregion


                    //如果关联的日期和类型交易日的记录才要转换
                    if (q != null && q.Count<QH_CFBailScaleValue>() > 0 && item.PositionBailTypeID == (int)Types.QHPositionBailType.ByTradeDays)
                    {
                        #region //如果关联的日期和类型交易日的记录才要转换
                        int end = 0;
                        //如果设置无有值则为非法记录，并且不可能小于零也不可以大于22，因为一个月最多的有效交易日也就是二十二天
                        if (!item.Ends.HasValue || item.Ends.Value >= 22 || item.Ends.Value <= 0)
                        {
                            throw new VTException(errCode, errMsg);
                        }
                        end = item.Ends.Value;

                        //获取当月共几天
                        int days = 0;
                        List<DateTime> timeList = Utils.GetCurrentMothDay(out days);


                        //最后交易日   //之前设置为1，没有考虑到记录不是交割月时而不用计算这个最后交易日的
                        int lastTrdeday = 31;
                        //当记录是交割月的记录时处理最后交易日的问题
                        if (item.DeliveryMonthType == (int)Types.QHCFPositionMonthType.OnDelivery)
                        {
                            lastTrdeday = MCService.GetLastTradingDayByBreedClassId(item.BreedClassID.Value);
                        }
                        //记录最后交易日之后的日期的所有日期
                        List<DateTime> lastDayed = new List<DateTime>();
                        foreach (var model in timeList)
                        {
                            if (model.Day > lastTrdeday)
                            {
                                lastDayed.Add(model);
                            }
                        }
                        //删除所有最后交易日之后的日期
                        foreach (var dayed in lastDayed)
                        {
                            timeList.Remove(dayed);
                        }

                        int countTradeDay = timeList.Count;
                        if (countTradeDay < end)
                        {
                            throw new VTException(errCode, errMsg);
                        }

                        //计算开始最后交易日的开始时间（最后交易日前几天的开始时间）
                        DateTime startTime = timeList[countTradeDay - end];
                        //关联的日期的结束时间
                        DateTime endTime = timeList[countTradeDay - 1 - end];
                        retList.Start = startTime.Day;
                        retList.Ends = days;
                        //item.Start = startTime.Day;
                        //item.Ends = days;
                        //记录有关联日期的最后日期记录，因为最后日期是由关联记录的开始日期决定
                        foreach (var modi in q)
                        {
                            if (!modifyTime.ContainsKey(modi.CFBailScaleValueID))
                            {
                                modifyTime.Add(modi.CFBailScaleValueID, endTime);
                            }
                        }
                        #endregion
                    }

                    else if (item.PositionBailTypeID == (int)Types.QHPositionBailType.ByTradeDays)
                    {
                        #region 否则如果是按交易日的记录要重新计算日期转为自然日


                        if (!item.Start.HasValue || !item.Ends.HasValue)
                        {
                            errMsg = "开始日期或者结束日期不能没有数值，不合规的日期设置";
                            throw new VTException(errCode, errMsg);

                        }
                        if (item.Start.Value > item.Ends.Value)
                        {
                            errMsg = "开始日期比结束日期大，不合规的日期设置";
                            throw new VTException(errCode, errMsg);
                        }

                        int start, end = 0;
                        end = item.Ends.Value;
                        start = item.Start.Value;

                        //获取当月共几天
                        int days = 0;
                        List<DateTime> timeList = Utils.GetCurrentMothDay(out days);


                        //最后交易日   //之前设置为1，没有考虑到记录不是交割月时而不用计算这个最后交易日的
                        int lastTrdeday = 31;
                        //当记录是交割月的记录时处理最后交易日的问题
                        if (item.DeliveryMonthType == (int)Types.QHCFPositionMonthType.OnDelivery)
                        {
                            lastTrdeday = MCService.GetLastTradingDayByBreedClassId(item.BreedClassID.Value);
                        }
                        //记录最后交易日之后的日期的所有日期
                        List<DateTime> lastDayed = new List<DateTime>();
                        foreach (var model in timeList)
                        {
                            if (model.Day > lastTrdeday)
                            {
                                lastDayed.Add(model);
                            }
                        }
                        //删除所有最后交易日之后的日期
                        foreach (var dayed in lastDayed)
                        {
                            timeList.Remove(dayed);
                        }

                        int countTradeDay = timeList.Count;
                        //计算开始最后交易日的开始时间（最后交易日前几天的开始时间）
                        DateTime startTime = DateTime.Now;
                        //关联的日期的结束时间
                        DateTime endTime = DateTime.Now;

                        //如果开始日期比当前所有交易日总数大那么开始日期直接以最后交易日为开始日
                        if (start > countTradeDay || start <= 0)
                        {
                            startTime = timeList[countTradeDay - 1];
                        }
                        else
                        {
                            //if (start <= 0)
                            //{
                            //    start = 1;
                            //}
                            startTime = timeList[start - 1];
                        }
                        retList.Start = startTime.Day;

                        //当结束日期比当前月所有交易日还大时，直接以当前月所拥有的自然最后日替换
                        if (end > countTradeDay || end <= 0)
                        {
                            //endTime = timeList[countTradeDay - 1];
                            retList.Ends = days;
                        }
                        else
                        {
                            endTime = timeList[end - 1];
                            retList.Ends = endTime.Day;
                        }
                        #endregion

                    }

                    list.Add(retList);

                }

                #region //修改已经在关联的日期
                //修改已经在关联的日期
                foreach (var item in modifyTime)
                {
                    //var q = from c in list
                    //        where
                    //             c.CFBailScaleValueID == item.Key
                    //        select c;
                    foreach (var model in list)
                    {
                        if (model.CFBailScaleValueID == item.Key)
                        {
                            //如果开始日期为null即为第一日，不为空则原始的日期
                            if (!model.Start.HasValue)
                            {
                                model.Start = 1;
                            }
                            model.Ends = item.Value.Day;
                        }

                    }
                }
                #endregion

                #region 写日志
                //日志字符串
                string txt = "品种类型：{0},控制类型：{1}，交割类型：{2}，开始：{3}，结束：{4}，比例：{5}";
                foreach (var model in list)
                {
                    //写日志
                  string  txtWriter = string.Format(txt, model.BreedClassID, model.PositionBailTypeID, model.DeliveryMonthType, model.Start, model.Ends, model.BailScale);
                  LogHelper.WriteDebug(txtWriter);
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                throw new VTException(errCode, errMsg);
            }

            return list;
        }



        public IList<QH_CFBailScaleValue> GetCFBailScaleValueByBreedClassID(int breedClassID)
        {
            return cFBailScaleValueByBreedClassIDObj.GetByKey(breedClassID);
        }


        public IList<QH_PositionBailType> GetAllPositionBailType()
        {
            return GetAllPositionBailType(false);
        }

        private IList<QH_PositionBailType> GetAllPositionBailType(bool reLoad)
        {
            return allPositionBailTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的期货交易保证金限制月份
        /// </summary>
        /// <returns></returns>
        public IList<QH_PositionBailType> GetAllPositionBailTypeFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllPositionBailType();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8204";
                    string errMsg = "无法从管理中心获取所有的期货交易保证金限制月份列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_PositionBailType GetPositionBailTypeByPositionBailTypeID(int id)
        {
            return allPositionBailTypeObj.GetByKey(id);
        }


        public IList<QH_PositionLimitValue> GetAllQHPositionLimitValue()
        {
            return GetAllQHPositionLimitValue(false);
        }

        private IList<QH_PositionLimitValue> GetAllQHPositionLimitValue(bool reLoad)
        {
            return allQHPositionLimitValueObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的期货_持仓限制
        /// </summary>
        /// <returns></returns>
        private IList<QH_PositionLimitValue> GetAllQHPositionLimitValueFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllQHPositionLimitValue();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8205";
                    string errMsg = "无法从管理中心获取所有的期货交易持仓限制列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }


        public QH_PositionLimitValue GetPositionLimitValueByPositionLimitValueID(int id)
        {
            return allQHPositionLimitValueObj.GetByKey(id);
        }


        private IList<QH_PositionLimitValue> GetPositionLimitValueByBreedClassIDFromWCF(int id)
        {
            try
            {
                using (FuturesTradeRulesClient client = GetClient())
                {
                    IList<QH_PositionLimitValue> list = client.GetPositionLimitValueByBreedClassID(id);

                    List<DateTime> timeList = GetCurrentMothDay(id);
                    foreach (var item in list)
                    {
                        //如果是交易日把它转为自然日存储，后面获取时候就可以直接判断与自然日即可
                        if (item.PositionBailTypeID == (int)Types.QHPositionBailType.ByTradeDays)
                        {
                            //如果设置的数据有异常则不作转换
                            if (item.Start.HasValue)
                            {
                                if (item.Start > timeList.Count)
                                {
                                    string errCode = "GT-8206";
                                    string errMsg = "商品类别Id=" + id + "：" + "数据设置有异常，持仓开始日期比当前系统月的有效交易日期还大";
                                    throw new VTException(errCode, errMsg, new Exception());
                                }
                                else
                                {
                                    item.Start = timeList[item.Start.Value - 1].Day;   //转为当前自然日期
                                }
                            }
                            if (item.Ends.HasValue)
                            {
                                //如果结束时间比交易日大那边最后一日所有交易日最后一日
                                if (item.Ends > timeList.Count)
                                {
                                    item.Ends = timeList[timeList.Count - 1].Day;
                                    //string errCode = "GT-8206";
                                    //string errMsg = "数据设置有异常，持仓结束日期比当前系统月的有效交易日期还大";
                                    //throw new VTException(errCode, errMsg, new Exception());
                                }
                                else
                                {
                                    item.Ends = timeList[item.Ends.Value - 1].Day;   //转为当前自然日期
                                }
                            }
                        }

                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                string errCode = "GT-8206";
                string errMsg = "商品类别Id=" + id + "：" + "无法根据商品类别从管理中心获取所有的期货交易持仓限制列表。";
                throw new VTException(errCode, errMsg, ex);
            }
        }

        /// <summary>
        /// 初始化当前系统当前日期当前月所有交易日(根据品种ID)
        /// </summary>
        public static List<DateTime> GetCurrentMothDay(int breedClassID)
        {
            //CurrentMothDay.Clear();
            int day = 0;
            List<DateTime> CurrentMothDay = new List<DateTime>();
            List<DateTime> list = Utils.GetCurrentMothDay(out day);
            CurrentMothDay.AddRange(list);
            CM_BreedClass cm_breedClass = MCService.CommonPara.GetBreedClassByBreedClassID(breedClassID);

            ////获取到要清算的日期再判断是否是非交易日期,如果是再向前推(while)
            //CM_BourseType cm_bourseList = MCService.CommonPara.GetBourseTypeByBourseTypeID(cm_breedClass.BourseTypeID.Value);
            bool isTradeDate = false;
            foreach (var model in list)
            {
                //foreach (var item in cm_bourseList)
                //{
                isTradeDate = MCService.CommonPara.IsTradeDate(cm_breedClass.BourseTypeID.Value, model);
                if (!isTradeDate)
                {
                    //如果是非交易日期那么把它删除
                    if (CurrentMothDay.Contains(model))
                    {
                        CurrentMothDay.Remove(model);
                    }
                }
                //    }
            }
            return CurrentMothDay;
        }


        public IList<QH_PositionLimitValue> GetPositionLimitValueByBreedClassID(int id)
        {
            return positionLimitValueByBreedClassIDObj.GetByKey(id);
        }

        public IList<QH_PositionLimitValue> GetPositionLimitValueByCommodityCode(string code)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            if (breedClass == null)
                return null;

            return GetPositionLimitValueByBreedClassID(breedClass.BreedClassID);
        }


        public IList<QH_CFPositionMonth> GetAllCFPostionMonth()
        {
            return GetAllCFPostionMonth(false);
        }

        private IList<QH_CFPositionMonth> GetAllCFPostionMonth(bool reLoad)
        {
            return allCFPostionMonthObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的期货_品种_交割月份
        /// </summary>
        /// <returns></returns>
        public IList<QH_CFPositionMonth> GetAllCFPostionMonthFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllCFPositionMonth();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8207";
                    string errMsg = "无法从管理中心获取所有的期货交割月份列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_CFPositionMonth GetCFPostionMonthByDeliveryMonthTypeID(int id)
        {
            return allCFPostionMonthObj.GetByKey(id);
        }


        public IList<QH_ConsignInstructionType> GetAllConsionInstructionType()
        {
            return GetAllConsionInstructionType(false);
        }

        private IList<QH_ConsignInstructionType> GetAllConsionInstructionType(bool reLoad)
        {
            return allConsionInstructionTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的委托指令类型
        /// </summary>
        /// <returns></returns>
        private IList<QH_ConsignInstructionType> GetAllConsionInstructionTypeFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllConsignInstructionType();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8208";
                    string errMsg = "无法从管理中心获取委托指令类型列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_ConsignInstructionType GetConsionInstructionTypeByID(int id)
        {
            return allConsionInstructionTypeObj.GetByKey(id);
        }


        public IList<QH_ConsignQuantum> GetAllConsignQuantum()
        {
            return GetAllConsignQuantum(false);
        }

        private IList<QH_ConsignQuantum> GetAllConsignQuantum(bool reLoad)
        {
            return consignQuantumObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的交易规则委托量
        /// </summary>
        /// <returns></returns>
        public IList<QH_ConsignQuantum> GetAllConsignQuantumFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllConsignQuantum();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8209";
                    string errMsg = "无法从管理中心获取所有的交易规则委托量列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_ConsignQuantum GetConsignQuantumByConsignQuantumID(int id)
        {
            return consignQuantumObj.GetByKey(id);
        }


        public IList<QH_FutureCosts> GetAllFutureCosts()
        {
            return GetAllFutureCosts(false);
        }

        private IList<QH_FutureCosts> GetAllFutureCosts(bool reLoad)
        {
            return futureCostsObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的品种_期货_交易费用
        /// </summary>
        /// <returns></returns>
        private IList<QH_FutureCosts> GetAllFutureCostsFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllFutureCosts();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8210";
                    string errMsg = "无法从管理中心获取所有的期货交易费用列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_FutureCosts GetFutureCostsByBreedClassID(int id)
        {
            return futureCostsObj.GetByKey(id);
        }

        /// <summary>
        /// 根据BreedClassID获取货币类型
        /// </summary>
        /// <param name="breedClassID">breedClassID</param>
        /// <returns>货币类型</returns>
        public CM_CurrencyType GetCurrencyTypeByBreedClassID(int breedClassID)
        {
            QH_FutureCosts costs = GetFutureCostsByBreedClassID(breedClassID);

            CM_CurrencyType currencyType = null;
            if (costs != null)
            {
                int? currencyTypeID = costs.CurrencyTypeID;

                if (currencyTypeID.HasValue)
                {
                    currencyType = MCService.CommonPara.GetCurrencyTypeByID(currencyTypeID.Value);
                }
            }

            return currencyType;
        }

        /// <summary>
        /// 根据商品代码获取货币类型
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns>货币类型</returns>
        public CM_CurrencyType GetCurrencyTypeByCommodityCode(string code)
        {
            CM_CurrencyType type = null;
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);

            if (breedClass != null)
            {
                type = GetCurrencyTypeByBreedClassID(breedClass.BreedClassID);
            }

            return type;
        }


        public IList<QH_FuturesTradeRules> GetAllFuturesTradeRules()
        {
            return GetAllFuturesTradeRules(false);
        }

        private IList<QH_FuturesTradeRules> GetAllFuturesTradeRules(bool reLoad)
        {
            return futureTradeRulsObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的期货_品种_交易规则
        /// </summary>
        /// <returns></returns>
        private IList<QH_FuturesTradeRules> GetAllFuturesTradeRulesFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllFuturesTradeRules();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8211";
                    string errMsg = "无法从管理中心获取所有的期货交易规则列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_FuturesTradeRules GetFuturesTradeRulesByBreedClassID(int id)
        {
            return futureTradeRulsObj.GetByKey(id);
        }

        /// <summary>
        /// 获取期货的交割制度
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <param name="fund">资金交割制度</param>
        /// <param name="agreement">股票交割制度</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>是否成功获取</returns>
        public bool GetDeliveryInstitution(string code, out int fund, out int agreement, ref string strMessage)
        {
            bool result = false;
            string errCode = "GT-8212";
            string errMsg = "无法根据商品编码从管理中心获取对于的交割制度。";
            strMessage = errCode + ":" + errMsg;

            fund = -1;
            agreement = -1;

            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            if (breedClass != null)
            {
                QH_FuturesTradeRules rules = GetFuturesTradeRulesByBreedClassID(breedClass.BreedClassID);
                if (rules != null)
                {
                    fund = rules.FundDeliveryInstitution.Value;
                    agreement = rules.AgreementDeliveryInstitution.Value;
                    result = true;
                }
            }

            return result;
        }


        public IList<QH_HighLowStopScopeType> GetAllHighLowStopScopeType()
        {
            return GetAllHighLowStopScopeType(false);
        }

        private IList<QH_HighLowStopScopeType> GetAllHighLowStopScopeType(bool reLoad)
        {
            return highLowStopScopeTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的涨跌停板幅度类型
        /// </summary>
        /// <returns></returns>
        private IList<QH_HighLowStopScopeType> GetAllHighLowStopScopeTypeFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllHighLowStopScopeType();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8213";
                    string errMsg = "无法从管理中心获取所有的涨跌停板幅度类型列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_HighLowStopScopeType GetHighLowStopScopeTypeByHighLowStopScopeID(int id)
        {
            return highLowStopScopeTypeObj.GetByKey(id);
        }


        public IList<QH_LastTradingDay> GetAllLastTradingDay()
        {
            return GetAllLastTradingDay(false);
        }

        private IList<QH_LastTradingDay> GetAllLastTradingDay(bool reLoad)
        {
            return lastTradingDayObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的最后交易日
        /// </summary>
        /// <returns></returns>
        private IList<QH_LastTradingDay> GetAllLastTradingDayFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllLastTradingDay();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8214";
                    string errMsg = "无法从管理中心获取所有的最后交易日列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_LastTradingDay GetLastTradingDayByLastTradingDayID(int id)
        {
            return lastTradingDayObj.GetByKey(id);
        }


        public IList<QH_LastTradingDayType> GetAllLastTradingDayType()
        {
            return GetAllLastTradingDayType(false);
        }

        private IList<QH_LastTradingDayType> GetAllLastTradingDayType(bool reLoad)
        {
            return lastTradingDayTypeObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的最后交易日类型
        /// </summary>
        /// <returns></returns>
        private IList<QH_LastTradingDayType> GetAllLastTradingDayTypeFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllLastTradingDayType();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8215";
                    string errMsg = "无法从管理中心获取所有的最后交易日类型列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_LastTradingDayType GetLastTradingDayTypeByLastTradingDayTypeID(int id)
        {
            return lastTradingDayTypeObj.GetByKey(id);
        }


        public IList<QH_Month> GetAllMonth()
        {
            return GetAllMonth(false);
        }

        private IList<QH_Month> GetAllMonth(bool reLoad)
        {
            return monthObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的月份
        /// </summary>
        /// <returns></returns>
        private IList<QH_Month> GetAllMonthFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllMonth();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8216";
                    string errMsg = "无法从管理中心获取所有的交易月份列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_Month GetMonthByMonthID(int id)
        {
            return monthObj.GetByKey(id);
        }


        public IList<QH_SIFBail> GetAllSIFBail()
        {
            return GetAllSIFBail(false);
        }

        private IList<QH_SIFBail> GetAllSIFBail(bool reLoad)
        {
            return sifBailObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的品种_股指期货_保证金
        /// </summary>
        /// <returns></returns>
        private IList<QH_SIFBail> GetAllSIFBailFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllSIFBail();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8217";
                    string errMsg = "无法从管理中心获取所有的股指期货保证金列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_SIFBail GetSIFBailByBreedClassID(int id)
        {
            return sifBailObj.GetByKey(id);
        }


        public IList<QH_SIFPosition> GetAllSIFPosition()
        {
            return GetAllSIFPosition(false);
        }

        private IList<QH_SIFPosition> GetAllSIFPosition(bool reLoad)
        {
            return sifPositionObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的股指期货持仓限制
        /// </summary>
        /// <returns></returns>
        private IList<QH_SIFPosition> GetAllSIFPositionFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllSIFPosition();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8218";
                    string errMsg = "无法从管理中心获取所有的股指期货持仓限制列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_SIFPosition GetSIFPositionByBreedClassID(int id)
        {
            return sifPositionObj.GetByKey(id);
        }

        public QH_SIFPosition GetSIFPositionByCommodityCode(string code)
        {
            CM_BreedClass breedClass = MCService.CommonPara.GetBreedClassByCommodityCode(code);
            if (breedClass == null)
                return null;

            return GetSIFPositionByBreedClassID(breedClass.BreedClassID);
        }


        public IList<QH_SingleRequestQuantity> GetAllSingleRequestQuantity()
        {
            return GetAllSingleRequestQuantity(false);
        }

        private IList<QH_SingleRequestQuantity> GetAllSingleRequestQuantity(bool reLoad)
        {
            return singleRequestQuantityObj.GetAll(reLoad);
        }

        /// <summary>
        /// 获取所有的单笔委托量
        /// </summary>
        /// <returns></returns>
        private IList<QH_SingleRequestQuantity> GetAllSingleRequestQuantityFromWCF()
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetAllSingleRequestQuantity();
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8219";
                    string errMsg = "无法从管理中心获取所有的单笔委托量列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }

        public QH_SingleRequestQuantity GetSingleRequestQuantityByID(int id)
        {
            return singleRequestQuantityObj.GetByKey(id);
        }


        public IList<QH_SingleRequestQuantity> GetSingleRequestQuantityByConsignQuantumID(int id)
        {
            return singleRequestQuantityByConsignQuantumIDObj.GetByKey(id);
        }

        private IList<QH_SingleRequestQuantity> GetSingleRequestQuantityByConsignQuantumIDFromWCF(int id)
        {

            {
                try
                {
                    using (FuturesTradeRulesClient client = GetClient())
                        return client.GetSingleRequestQuantityByConsignQuantumID(id);
                }
                catch (Exception ex)
                {
                    string errCode = "GT-8220";
                    string errMsg = "无法根据交易规则委托量标识从管理中心获取所有的单笔委托量列表。";
                    throw new VTException(errCode, errMsg, ex);
                }
            }
        }
        #endregion

    }
}