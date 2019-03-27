using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.DAL;
using ManagementCenter.Model;

namespace ManagementCenter.BLL.CommonTable
{
    /// <summary>
    /// 描述：现货商品代码更新和期货和约代码生成 错误编码范围:7000-7049
    /// 作者：程序员：熊晓凌  修改：刘书伟
    /// 日期：2009-01-04      2009-12-02
    /// </summary>
    public class CommodityCodeUpdate
    {
        /// <summary>
        /// 从StockInfo表中更新新增和修改的现货代码
        /// </summary>
        public void CodeUpdata()
        {
            try
            {
                ManagementCenter.DAL.CommonTable.CommodityCodeUpdateDAL.CommodityCodeUpdate();
                LogHelper.WriteDebug("从StockInfo表中更新新增和修改的现货代码方法CommodityCodeUpdate()结束");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message.ToString(), ex);
            }
        }

        /// <summary>
        /// 从HKStockInfo表中更新新增和修改的港股交易商品代码更新
        /// </summary>
        public void HKCodeUpdata()
        {
            try
            {
                ManagementCenter.DAL.HKCommodityCodeUpdateDAL.HKCommodityCodeUpdate();
                LogHelper.WriteDebug("从HKStockInfo表中更新新增和修改的港股代码更新方法HKCommodityCodeUpdate()结束");

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message.ToString(), ex);
            }
        }

        /// <summary>
        /// 把新增的现货代码(StockInfo表和HKStockInfo表中)新增的代码自动添加到交易商品_撮合机_分配表中
        /// </summary>
        public void XHCodeAutoRCTradeCommodityAssign()
        {
            try
            {
                //交易所类型ID
                int bourseTypeID = 0;
                StockInfoBLL stockInfoBLL = new StockInfoBLL();
                HKStockInfoBLL hKStockInfoBLL = new HKStockInfoBLL();

                CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
                RC_MatchCenterBLL rcMatchCenterBll = new RC_MatchCenterBLL();
                RC_MatchMachineBLL rcMatchMachineBll = new RC_MatchMachineBLL();
                RC_TradeCommodityAssignBLL rcTradeCommodityAssignBll = new RC_TradeCommodityAssignBLL();
                RC_MatchMachine rcMatchMachine = new RC_MatchMachine();
                RC_MatchCenter rcMatchCenterModel = new RC_MatchCenter();
                RC_TradeCommodityAssign rcTradeCommodityAssignModel = new RC_TradeCommodityAssign();

                //获取撮合中心记录
                List<ManagementCenter.Model.RC_MatchCenter> rcMatchCenters = rcMatchCenterBll.GetListArray(string.Empty);
                if (rcMatchCenters.Count == 0)
                {
                    string errCode = "GL-7005";
                    string errMsg = "撮合中心记录不存在!";
                    LogHelper.WriteDebug(errCode + errMsg);
                    VTException exception = new VTException(errCode, errMsg);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                //获取所有新添加的普通代码
                string strWhereStock = " StockCode not in (select CommodityCode from CM_Commodity) ";
                List<StockInfo> stockInfoList = stockInfoBLL.GetStockInfoList(strWhereStock);
                //获取所有新添加的港股代码
                string strWhereHKStock = " StockCode not in (select HKCommodityCode from HK_Commodity) ";
                List<HKStockInfo> hKStockInfoList = hKStockInfoBLL.GetHKStockInfoList(strWhereHKStock);

                if (stockInfoList.Count > 0)
                {
                    int breedClassID = 0;//品种标识
                    foreach (StockInfo info in stockInfoList)
                    {
                        if (breedClassID != info.BreedClassID)
                        {
                            breedClassID = info.BreedClassID;
                            //获取相同品种ID的所有代码
                            string strWhereStock1 = " StockCode not in (select CommodityCode from CM_Commodity) and BreedClassID={0} ";
                            List<StockInfo> _stockInfoList =
                                stockInfoBLL.GetStockInfoList(string.Format(strWhereStock1, breedClassID));//(string.Format(" and BreedClassID={0}", breedClassID));
                            if (_stockInfoList == null || _stockInfoList.Count == 0)
                            {
                                string errCode = "GL-7006";
                                string errMsg = "相同品种ID的所有普通代码为空!";
                                LogHelper.WriteDebug(errCode + errMsg);
                                VTException exception = new VTException(errCode, errMsg);
                                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                                continue;
                            }
                            //根据品种ID获取交易所ID
                            CM_BreedClass cmBreedClass = cM_BreedClassBLL.GetModel(breedClassID);
                            if (cmBreedClass != null)
                            {
                                bourseTypeID = (int)cmBreedClass.BourseTypeID;
                                //根据交易所类型ID，获取撮合机记录
                                List<ManagementCenter.Model.RC_MatchMachine> rcMatchMachines =
                                    rcMatchMachineBll.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID));
                                if (rcMatchMachines.Count == 0)
                                {
                                    string errCode = "GL-7007";
                                    string errMsg = "获取撮合机记录不存在!";
                                    LogHelper.WriteDebug(errCode + errMsg);
                                    VTException exception = new VTException(errCode, errMsg);
                                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                                    return;
                                }
                                //把同一品种ID的所有代码添加到交易商品_撮合机_分配表中
                                foreach (StockInfo _stockInfo in _stockInfoList)
                                {
                                    rcTradeCommodityAssignModel.CommodityCode = _stockInfo.StockCode;
                                    rcTradeCommodityAssignModel.MatchMachineID = rcMatchMachines[0].MatchMachineID;
                                    rcTradeCommodityAssignModel.CodeFormSource = (int)GTA.VTS.Common.CommonObject.Types.IsCodeFormSource.Yes; //_stockInfo.CodeFromSource;
                                    rcTradeCommodityAssignBll.Add(rcTradeCommodityAssignModel);

                                }
                            }
                        }
                        continue;

                    }
                }
                if (hKStockInfoList.Count > 0)
                {
                    int breedClassID = 0;//品种标识
                    foreach (HKStockInfo hKInfo in hKStockInfoList)
                    {
                        if (breedClassID != hKInfo.BreedClassID)
                        {
                            breedClassID = hKInfo.BreedClassID;
                            //获取相同品种ID的所有港股代码
                            string strWhereHKStock1 = " StockCode not in (select HKCommodityCode from HK_Commodity) and BreedClassID={0} ";

                            List<HKStockInfo> _hKStockInfoList =
                                hKStockInfoBLL.GetHKStockInfoList(string.Format(strWhereHKStock1, breedClassID));//(string.Format(" and BreedClassID={0}", breedClassID));

                            if (_hKStockInfoList == null || _hKStockInfoList.Count == 0)
                            {
                                string errCode = "GL-7008";
                                string errMsg = "相同品种ID的所有港股代码为空!";
                                LogHelper.WriteDebug(errCode + errMsg);
                                VTException exception = new VTException(errCode, errMsg);
                                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                                continue;
                            }
                            //根据品种ID获取交易所ID
                            CM_BreedClass cmBreedClass = cM_BreedClassBLL.GetModel(breedClassID);
                            if (cmBreedClass != null)
                            {
                                bourseTypeID = (int)cmBreedClass.BourseTypeID;
                                //根据交易所类型ID，获取撮合机记录
                                List<ManagementCenter.Model.RC_MatchMachine> rcMatchMachines =
                                    rcMatchMachineBll.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID));
                                if (rcMatchMachines.Count == 0)
                                {
                                    string errCode = "GL-7009";
                                    string errMsg = "获取撮合机记录不存在!";
                                    LogHelper.WriteDebug(errCode + errMsg);
                                    VTException exception = new VTException(errCode, errMsg);
                                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                                    return;
                                }
                                //把同一品种ID的所有港股代码添加到交易商品_撮合机_分配表中
                                foreach (HKStockInfo _hKStockInfo in _hKStockInfoList)
                                {
                                    rcTradeCommodityAssignModel.CommodityCode = _hKStockInfo.StockCode;
                                    rcTradeCommodityAssignModel.MatchMachineID = rcMatchMachines[0].MatchMachineID;
                                    rcTradeCommodityAssignModel.CodeFormSource = (int)GTA.VTS.Common.CommonObject.Types.IsCodeFormSource.No; //_hKStockInfo.CodeFromSource;
                                    rcTradeCommodityAssignBll.Add(rcTradeCommodityAssignModel);

                                }
                            }
                        }
                        continue;

                    }
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-7010";
                string errMsg = "把新增的现货代码(StockInfo表和HKStockInfo表中)新增的代码添加到交易商品_撮合机_分配表中失败!";
                LogHelper.WriteDebug(errCode + errMsg);
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        /// <summary>
        /// 检测期货代码是否到期
        /// </summary>
        public void CheckCodeIsExpired()
        {
            try
            {
                QH_AgreementDeliveryMonthDAL AgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();

                //一般品种，交割月为1~12月份
                QH_FuturesTradeRulesDAL FuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
                //获取交易规则
                List<QH_FuturesTradeRules> L = FuturesTradeRulesDAL.GetListArray(string.Empty);
                if (L != null)
                {
                    foreach (QH_FuturesTradeRules FuturesTradeRules in L)
                    {
                        #region old
                        //List<QH_AgreementDeliveryMonth> listQHAgreDeMonth = AgreementDeliveryMonthDAL.GetListArray(string.Format("BreedClassID={0}", FuturesTradeRules.BreedClassID));

                        //foreach (QH_AgreementDeliveryMonth AgreementDeliveryMonth in listQHAgreDeMonth)
                        //{
                        //    if ((int)AgreementDeliveryMonth.MonthID != 13)
                        //    {
                        //        Production(FuturesTradeRules.BreedClassID);

                        //    }
                        //}
                        #endregion
                        //交割月份要么等于13，要么不等于，因此不需循环，造成重复执行多遍
                        //update by 董鹏 2010-03-31
                        //获取交易规则中的交割月份
                        List<QH_AgreementDeliveryMonth> listQHAgreDeMonth = AgreementDeliveryMonthDAL.GetListArray(string.Format("BreedClassID={0} and MonthID!={1}", FuturesTradeRules.BreedClassID, 13));
                        if (listQHAgreDeMonth.Count != 0)
                        {
                            Production(FuturesTradeRules.BreedClassID);
                        }
                    }
                }
                else
                {
                    LogHelper.WriteDebug("期货交易规则数据为空");
                }

                #region old
                ////特殊处理，本月下月随后的两个季度月份
                ////QH_AgreementDeliveryMonthDAL AgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();
                //List<QH_AgreementDeliveryMonth> L_m = AgreementDeliveryMonthDAL.GetListArray(string.Empty);
                //if (L_m != null)
                //{
                //    foreach (QH_AgreementDeliveryMonth AgreementDeliveryMonth in L_m)
                //    {
                //        if ((int)AgreementDeliveryMonth.MonthID == 13)
                //        {
                //            QH_LastTradingDay LastTradingDayEntity =
                //                GetLastTradingDayEntity((int)AgreementDeliveryMonth.BreedClassID);
                //            if (System.DateTime.Now.Day ==
                //                GetLastTradingDay(LastTradingDayEntity, (int)AgreementDeliveryMonth.BreedClassID))
                //            {
                //                SpecialQHCodeUpdate((int)AgreementDeliveryMonth.BreedClassID);
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    LogHelper.WriteDebug("期货合约交割月份数据为空");
                //}
                #endregion
                //update by 董鹏 2010-03-31
                //特殊处理，本月下月随后的两个季度月份
                //QH_AgreementDeliveryMonthDAL AgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();
                List<QH_AgreementDeliveryMonth> L_m = AgreementDeliveryMonthDAL.GetListArray(string.Format("MonthID={0}", 13));
                if (L_m != null)
                {
                    foreach (QH_AgreementDeliveryMonth AgreementDeliveryMonth in L_m)
                    {
                        QH_LastTradingDay LastTradingDayEntity = GetLastTradingDayEntity((int)AgreementDeliveryMonth.BreedClassID);
                        if (System.DateTime.Now.Day == GetLastTradingDay(LastTradingDayEntity, (int)AgreementDeliveryMonth.BreedClassID))
                        {
                            SpecialQHCodeUpdate((int)AgreementDeliveryMonth.BreedClassID);
                        }
                    }
                }
                else
                {
                    LogHelper.WriteDebug("期货合约交割月份数据为空");
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7000";
                string errMsg = "更新期货和约代码失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        /// <summary>
        /// 当天为最后交易日，更新和约代码
        /// </summary>
        /// <param name="breedclass">品种ID</param>
        public void Production(int breedclass)
        {
            QH_LastTradingDay LastTradingDayEntity = GetLastTradingDayEntity(breedclass);

            int LastTradingDay = GetLastTradingDay(LastTradingDayEntity, breedclass);

            if (System.DateTime.Now.Day == LastTradingDay)
            {
                QHCodeUpdata(breedclass, LastTradingDayEntity);
            }
        }

        /// <summary>
        /// 根据交易日实体和品种得到最后交易日，当前月份为交割月份
        /// </summary>
        /// <param name="LastTradingDayEntity">交易日实体</param>
        /// <param name="breedclass">品种ID</param>
        /// <returns></returns>
        public int GetLastTradingDay(QH_LastTradingDay LastTradingDayEntity, int breedclass)
        {
            try
            {
                switch ((int)LastTradingDayEntity.LastTradingDayTypeID)
                {
                    //第几天
                    case (int)Types.QHLastTradingDayType.DeliMonthAndDay:
                        if (IsTradingMonth(System.DateTime.Now.Month, breedclass))
                        {
                            return DeliMonthOfDay(LastTradingDayEntity, breedclass);
                        }
                        return int.MaxValue;
                    //倒数或者顺数第几个交易日
                    case (int)Types.QHLastTradingDayType.DeliMonthAndDownOrShunAndWeek:
                        if (IsTradingMonth(System.DateTime.Now.Month, breedclass))
                        {
                            return DeliMonthOfTurnOrBackTrandingDay(LastTradingDayEntity, breedclass);
                        }
                        return int.MaxValue;
                    //交割月份的前一个月份的倒数或者顺数第几个交易日
                    case (int)Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay:
                        if (IsTradingMonth(System.DateTime.Now.AddMonths(1).Month, breedclass))
                        {
                            return DeliMonthOfAgoMonthTradeDay(LastTradingDayEntity, breedclass);
                        }
                        return int.MaxValue;
                    //第几周的星期几
                    case (int)Types.QHLastTradingDayType.DeliMonthAndWeek:
                        if (IsTradingMonth(System.DateTime.Now.Month, breedclass))
                        {
                            return DeliMonthOfWeekDay(LastTradingDayEntity, breedclass);
                        }
                        return int.MaxValue;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7001";
                string errMsg = "根据类型求和约的最后交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }

            return int.MaxValue;
        }

        /// <summary>
        /// 类型为第几日，求最后交易日，如果当天为非交易日，往后顺延
        /// </summary>
        /// <param name="LastTradingDay"></param>
        /// <param name="breedclass"></param>
        /// <returns></returns>
        private int DeliMonthOfDay(QH_LastTradingDay LastTradingDay, int breedclass)
        {
            int day = (int)LastTradingDay.WhatDay;

            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;

            DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, day));
            while (!JudgmentIsTrandingDay(dt, breedclass))
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
        /// <param name="breedclass"></param>
        /// <returns></returns>
        private int DeliMonthOfWeekDay(QH_LastTradingDay LastTradingDay, int breedclass)
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
        /// <param name="breedclass"></param>
        /// <returns></returns>
        private int DeliMonthOfTurnOrBackTrandingDay(QH_LastTradingDay LastTradingDay, int breedclass)
        {
            int day = (int)LastTradingDay.WhatDay;

            DateTime now = DateTime.Now;
            int CurrentYear = now.Year;
            int CurrentMonth = now.Month;

            CM_NotTradeDateDAL NotTradeDateDAL = new CM_NotTradeDateDAL();

            int temp = 0;

            //根据品种获取当前月份里面的非交易日列表
            List<CM_NotTradeDate> List_CM_NotTradeDate = NotTradeDateDAL.GetListArrayByBreedClassID(breedclass);

            #region 根据类型求出最后交易日
            if ((int)LastTradingDay.Sequence == (int)Types.QHLastTradingDayIsSequence.Order)
            {
                for (int i = 1; i <= System.DateTime.DaysInMonth(CurrentYear, CurrentMonth); i++)
                {
                    DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, i));
                    bool falg = false;
                    foreach (CM_NotTradeDate date in List_CM_NotTradeDate)
                    {
                        if (((DateTime)date.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                        {
                            falg = true;
                            break;
                        }
                    }
                    if (!falg)
                    {
                        temp = temp + 1;
                        if (temp == day) return i;
                    }
                }
            }
            else
            {
                for (int i = System.DateTime.DaysInMonth(CurrentYear, CurrentMonth); i >= 1; i--)
                {
                    DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", CurrentYear, CurrentMonth, i));
                    bool falg = false;
                    foreach (CM_NotTradeDate date in List_CM_NotTradeDate)
                    {
                        if (((DateTime)date.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                        {
                            falg = true;
                            break;
                        }
                    }
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
        /// <param name="breedclass"></param>
        /// <returns></returns>
        private int DeliMonthOfAgoMonthTradeDay(QH_LastTradingDay LastTradingDay, int breedclass)
        {
            return DeliMonthOfTurnOrBackTrandingDay(LastTradingDay, breedclass);
        }


        /// <summary>
        /// 判断该天是否交易
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="breedclass"></param>
        /// <returns></returns>
        public bool JudgmentIsTrandingDay(DateTime dt, int breedclass)
        {
            CM_NotTradeDateDAL NotTradeDateDAL = new CM_NotTradeDateDAL();
            List<CM_NotTradeDate> List_CM_NotTradeDate = NotTradeDateDAL.GetListArrayByBreedClassID(breedclass);
            bool falg = true;
            foreach (CM_NotTradeDate NotTradeDate in List_CM_NotTradeDate)
            {
                if (((DateTime)NotTradeDate.NotTradeDay).ToShortDateString() == dt.ToShortDateString())
                {
                    falg = false;
                    break;
                }
            }
            return falg;
        }

        /// <summary>
        /// 根据品种获取最后交易日实体
        /// </summary>
        /// <param name="breedclass"></param>
        /// <returns></returns>
        public QH_LastTradingDay GetLastTradingDayEntity(int breedclass)
        {
            QH_FuturesTradeRulesDAL FuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
            QH_LastTradingDayDAL LastTradingDayDAL = new QH_LastTradingDayDAL();
            QH_FuturesTradeRules FuturesTradeRules = FuturesTradeRulesDAL.GetModel(breedclass);
            QH_LastTradingDay LastTradingDay = LastTradingDayDAL.GetModel((int)FuturesTradeRules.LastTradingDayID);
            return LastTradingDay;
        }


        /// <summary>
        /// 期货代码更新
        /// Update by: 董鹏
        /// Update Date: 2010-03-10
        /// Desc: 使用代码规则管理器生成代码
        /// </summary>
        /// <param name="breedclass">品种ID</param>
        /// <param name="LastTradingDayEntity">期货最后交易日实体</param>
        public void QHCodeUpdata(int breedclass, QH_LastTradingDay LastTradingDayEntity)
        {
            try
            {
                //根据品种ID，找到品种的代码名称
                string QH_Prefixname = GetQH_PrefixCodeByID(breedclass);

                //根据品种ID，找到品种的名称
                string QH_breedclassName = GetBreedClassNameByID(breedclass);

                //根据品种获取交易所
                CM_BreedClassBLL bc = new CM_BreedClassBLL();
                CM_BourseTypeBLL bt = new CM_BourseTypeBLL();
                var bourseType = bt.GetModel(bc.GetModel(breedclass).BourseTypeID.Value);

                //创建代码规则管理
                CodeRulesManager codeRule;
                if (bourseType.CodeRulesType.HasValue)
                {
                    LogHelper.WriteDebug("===开始自动生成代码，代码规则类型：" + bourseType.CodeRulesType.ToString());
                    codeRule = new CodeRulesManager((Types.CodeRulesType)bourseType.CodeRulesType.Value);
                }
                else
                {
                    LogHelper.WriteDebug("===开始自动生成代码，没有获取到交易所对应的代码规则，交易所：" + bourseType.BourseTypeName);
                    codeRule = new CodeRulesManager(null);
                }

                CM_CommodityDAL CommodityDAL = new CM_CommodityDAL();
                RC_TradeCommodityAssignDAL TradeCommodityAssignDAL = new RC_TradeCommodityAssignDAL();

                string Old_QH_codename;
                string New_QH_codename;

                if ((int)LastTradingDayEntity.LastTradingDayTypeID ==
                    (int)Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay)
                {
                    //Old_QH_codename = QH_Prefixname + GetTwoLenYear(DateTime.Now) +
                    //                  GetTwoLenMonth(DateTime.Now.AddMonths(1));
                    //New_QH_codename = QH_Prefixname + GetTwoLenYear(DateTime.Now.AddYears(1)) +
                    //                  GetTwoLenMonth(DateTime.Now.AddMonths(1));

                    //使用代码规则管理器生成代码
                    Old_QH_codename = codeRule.GetCode(QH_Prefixname, DateTime.Now.Year.ToString(), DateTime.Now.AddMonths(1).Month.ToString().PadLeft(2, '0'));
                    New_QH_codename = codeRule.GetCode(QH_Prefixname, DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddMonths(1).Month.ToString().PadLeft(2, '0'));
                }
                else
                {
                    //Old_QH_codename = QH_Prefixname + GetTwoLenYear(DateTime.Now) + GetTwoLenMonth(DateTime.Now);
                    //New_QH_codename = QH_Prefixname + GetTwoLenYear(DateTime.Now.AddYears(1)) +
                    //                  GetTwoLenMonth(DateTime.Now);

                    //使用代码规则管理器生成代码
                    Old_QH_codename = codeRule.GetCode(QH_Prefixname, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'));
                    New_QH_codename = codeRule.GetCode(QH_Prefixname, DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'));
                }

                CM_Commodity CM_Commodity = CommodityDAL.GetModel(Old_QH_codename);

                if (CM_Commodity == null)
                {
                    CM_Commodity = new CM_Commodity();
                }
                CM_Commodity.CommodityCode = New_QH_codename;
                CM_Commodity.MarketDate = DateTime.Parse(System.DateTime.Now.AddDays(1).ToShortDateString());
                //若第二天是非交易日则顺延一天 add by 董鹏 2010-03-31
                while (!JudgmentIsTrandingDay(CM_Commodity.MarketDate, breedclass))
                {
                    CM_Commodity.MarketDate = CM_Commodity.MarketDate.AddDays(1);
                }
                //根据确认结果 期货名称等于期货代码
                CM_Commodity.CommodityName = New_QH_codename;// QH_breedclassName + New_QH_codename.Substring(New_QH_codename.Length - 4);

                CM_Commodity.BreedClassID = breedclass;
                CM_Commodity.GoerScale = decimal.MaxValue;
                CM_Commodity.LabelCommodityCode = null;
                CM_Commodity.StockPinYin = null;
                CM_Commodity.turnovervolume = null;
                CM_Commodity.IsExpired = (int)Types.IsYesOrNo.No;//新增期货代码时，默认代码没有过期
                CM_Commodity.ISSysDefaultCode = (int)Types.IsYesOrNo.Yes;//新增期货代码时，默认是系统代码

                #region old
                //bool _result = CommodityDAL.Add(CM_Commodity);
                //if (!_result)
                //{
                //    LogHelper.WriteDebug("添加代码失败");
                //    return;
                //}
                ////更新代码分配表
                //TradeCommodityAssignDAL.Update(Old_QH_codename, New_QH_codename);
                ////CommodityDAL.Delete(Old_QH_codename);
                ////根据2009-7-22需求过期代码添加标识
                //int isExpired = (int)Types.IsYesOrNo.Yes;//旧代码状态设置为过期
                //bool _resultUpdate = CommodityDAL.Update(Old_QH_codename, isExpired);
                //if (!_resultUpdate)
                //{
                //    LogHelper.WriteDebug("更新代码失败");
                //}
                ////调用把期货新增的代码自动添加到可交易商品_撮合机_分配表中的方法
                //QHCodeAutoRCTradeCommodityAssign(New_QH_codename, breedclass);
                #endregion

                //前面的bool值实际上没有用处，改为直接抛出异常，并增加日志记录 update by 董鹏 2010-03-31
                LogHelper.WriteDebug("===生成新代码：" + New_QH_codename);
                //添加新代码
                CommodityDAL.Add(CM_Commodity);

                LogHelper.WriteDebug("===更新代码分配表");
                //更新代码分配表
                TradeCommodityAssignDAL.Update(Old_QH_codename, New_QH_codename);

                LogHelper.WriteDebug("===设置代码过期：" + Old_QH_codename);
                //旧代码状态设置为过期
                int isExpired = (int)Types.IsYesOrNo.Yes;
                CommodityDAL.Update(Old_QH_codename, isExpired);

                LogHelper.WriteDebug("===将代码添加到撮合机分配表中");
                //如果新代码未分配，将代码添加到撮合机分配表中
                QHCodeAutoRCTradeCommodityAssign(New_QH_codename, breedclass);

                LogHelper.WriteDebug("===新代码：" + New_QH_codename + "完成生成并分配到撮合机，旧代码：" + Old_QH_codename + "完成从撮合机分配表中移除并设为过期。");
            }
            catch (Exception ex)
            {
                string errCode = "GL-7002";
                string errMsg = "执行更新方法QHCodeUpdata()失败";
                //VTException exception = new VTException(errCode, errMsg, ex);
                //LogHelper.WriteError(exception.ToString(), exception.InnerException);
                LogHelper.WriteError(errCode + ":" + errMsg, ex);
            }
        }


        /// <summary>
        /// 得到两位年份
        /// </summary>
        /// <param name="dt">当前时间</param>
        /// <returns></returns>
        public static string GetTwoLenYear(DateTime dt)
        {
            string year = dt.Year.ToString();
            int lenth = year.Length;
            return year.Substring(lenth - 2);
        }

        /// <summary>
        /// 得到两位月份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetTwoLenMonth(DateTime dt)
        {
            string month = dt.Month.ToString();
            int Lenth = month.Length;
            if (Lenth == 1) month = "0" + month;
            return month;
        }

        /// <summary>
        /// 得到两位数的月份
        /// </summary>
        /// <param name="m">月份ID</param>
        /// <returns></returns>
        public static string GetTwoLenMonth(int m)
        {
            string month = m.ToString();
            int Lenth = month.Length;
            if (Lenth == 1) month = "0" + month;
            return month;
        }


        /// <summary>
        /// 根据某年某月第几周星期几得到为该月的几号
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="weekNO"></param>
        /// <param name="weekDay">星期日为0</param>
        /// <returns></returns>
        public int getDay(int year, int month, int weekNO, int weekDay)
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
        /// 判断月份是否为交割月
        /// </summary>
        /// <param name="month"></param>
        /// <param name="breedclass"></param>
        /// <returns></returns>
        public bool IsTradingMonth(int month, int breedclass)
        {
            QH_AgreementDeliveryMonthDAL AgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();
            List<QH_AgreementDeliveryMonth> L = AgreementDeliveryMonthDAL.GetListArray(string.Format("BreedClassID={0}", breedclass));
            bool falg = false;
            foreach (QH_AgreementDeliveryMonth AgreementDeliveryMonth in L)
            {
                if ((int)AgreementDeliveryMonth.MonthID == month || (int)AgreementDeliveryMonth.MonthID == 13)
                {
                    falg = true;
                    break;
                }
            }
            return falg;
        }


        /// <summary>
        /// 交易月份类型为本月下月随后的两个季度月份的代码更新
        /// </summary>
        /// <param name="breedclass"></param>
        private void SpecialQHCodeUpdate(int breedclass)
        {
            try
            {
                //根据品种ID，找到品种的前缀代码
                string QH_PrefixCode = GetQH_PrefixCodeByID(breedclass);

                //根据品种ID，找到品种的名称
                string QH_breedclassName = GetBreedClassNameByID(breedclass);

                string OldCode = string.Empty;
                string NewCode = string.Empty;

                //1,2,3,  4,5,6,  7,8,9  ,10,11,12

                //1,2,3,      6,
                //  2,3,      6,      9,
                //    3,  4,  6,      9,
                //        4,5,6,      9,
                //          5,6,      9,        12,
                //            6,  7,  9,        12,
                //                7,8,9,	    12,
                //    3,            8,9,        12,
                //    3,              9,  10,   12,
                //    3,                  10,11,12,
                //    3,      6,             11,12,
                //1,  3,      6,                12,
                //1,2,3,      6,
                //add 李健华 2010-01-15
                //比如今天之前是，1001，1002，1003，1006，今天之后是1002，1003，1006，1009，
                //下一个月1002到期后就是，1004，1006，1009，1010
                //保证3，6，9，12中有连续2个的季月，以及连续的两个自然月，总共4个合约是不变的。
                //==============
                switch (System.DateTime.Now.Month)
                {
                    case 1:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "01";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "09";
                        break;
                    case 2:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "02";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "04";
                        break;
                    case 3:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "03";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "05";
                        break;
                    case 4:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "04";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "12";
                        break;
                    case 5:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "05";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "07";
                        break;
                    case 6:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "06";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "08";
                        break;
                    case 7:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "07";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "03";
                        break;
                    case 8:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "08";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "10";
                        break;
                    case 9:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "09";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "11";
                        break;
                    case 10:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "10";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "06";
                        break;
                    case 11:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "11";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "01";
                        break;
                    case 12:
                        OldCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now) + "12";
                        NewCode = QH_PrefixCode + GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "02";
                        break;
                }

                CM_CommodityDAL CommodityDAL = new CM_CommodityDAL();

                RC_TradeCommodityAssignDAL TradeCommodityAssignDAL = new RC_TradeCommodityAssignDAL();

                CM_Commodity CM_Commodity = CommodityDAL.GetModel(OldCode);
                if (CM_Commodity == null) CM_Commodity = new CM_Commodity();

                CM_Commodity.CommodityCode = NewCode;
                CM_Commodity.MarketDate = DateTime.Parse(System.DateTime.Now.AddDays(1).ToShortDateString());
                //若第二天是非交易日则顺延一天 add by 董鹏 2010-03-31
                while (!JudgmentIsTrandingDay(CM_Commodity.MarketDate, breedclass))
                {
                    CM_Commodity.MarketDate = CM_Commodity.MarketDate.AddDays(1);
                }
                CM_Commodity.CommodityName = NewCode; //QH_breedclassName + NewCode.Substring(NewCode.Length - 4);

                CM_Commodity.BreedClassID = breedclass;
                CM_Commodity.GoerScale = decimal.MaxValue;
                CM_Commodity.LabelCommodityCode = null;
                CM_Commodity.StockPinYin = null;
                CM_Commodity.turnovervolume = null;
                CM_Commodity.IsExpired = (int)Types.IsYesOrNo.No;//新增期货代码时，默认代码没有过期
                CM_Commodity.ISSysDefaultCode = (int)Types.IsYesOrNo.Yes;//新增期货代码时，默认是系统代码

                bool _result = CommodityDAL.Add(CM_Commodity);
                if (!_result)
                {
                    LogHelper.WriteDebug("SpecialQHCodeUpdate()方法中的添加代码失败");
                    return;
                }
                //更新代码分配表
                TradeCommodityAssignDAL.Update(OldCode, NewCode);
                // CommodityDAL.Delete(OldCode);
                //根据2009-7-22需求过期代码添加标识
                int isExpired = (int)Types.IsYesOrNo.Yes;//旧代码状态设置为过期
                bool _resultUpdate = CommodityDAL.Update(OldCode, isExpired);
                if (!_resultUpdate)
                {
                    LogHelper.WriteDebug("SpecialQHCodeUpdate()方法中的更新代码失败");
                }
                //调用把期货新增的代码自动添加到可交易商品_撮合机_分配表中的方法
                QHCodeAutoRCTradeCommodityAssign(NewCode, breedclass);
            }
            catch (Exception ex)
            {
                string errCode = "GL-7003";
                string errMsg = "执行更新方法SpecialQHCodeUpdate()失败";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        /// <summary>
        /// 根据品种ID获取品种名称
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public string GetBreedClassNameByID(int BreedClassID)
        {
            CM_BreedClassDAL BreedClassDAL = new CM_BreedClassDAL();
            CM_BreedClass BreedClass = BreedClassDAL.GetModel(BreedClassID);
            if (BreedClass == null) return string.Empty;
            return BreedClass.BreedClassName;
        }

        /// <summary>
        /// 得到期货前缀代号
        /// </summary>
        /// <param name="BreedClassID"></param>
        /// <returns></returns>
        public string GetQH_PrefixCodeByID(int BreedClassID)
        {
            QH_FuturesTradeRulesDAL FuturesTradeRulesDAL = new QH_FuturesTradeRulesDAL();
            QH_FuturesTradeRules FuturesTradeRules = FuturesTradeRulesDAL.GetModel(BreedClassID);
            if (FuturesTradeRules == null) return string.Empty;
            return FuturesTradeRules.FutruesCode;
        }

        /// <summary>
        /// 把期货新增的代码自动添加到可交易商品_撮合机_分配表中
        /// </summary>
        /// <param name="CommodityName">商品代码</param>
        /// <param name="BreedClassID">品种ID</param>
        public void QHCodeAutoRCTradeCommodityAssign(string CommodityName, int BreedClassID)
        {
            try
            {
                //交易所类型ID
                int bourseTypeID = 0;
                CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
                RC_MatchCenterBLL rcMatchCenterBll = new RC_MatchCenterBLL();
                RC_MatchMachineBLL rcMatchMachineBll = new RC_MatchMachineBLL();
                RC_TradeCommodityAssignBLL rcTradeCommodityAssignBll = new RC_TradeCommodityAssignBLL();
                RC_MatchMachine rcMatchMachine = new RC_MatchMachine();
                RC_MatchCenter rcMatchCenterModel = new RC_MatchCenter();
                RC_TradeCommodityAssign rcTradeCommodityAssignModel = new RC_TradeCommodityAssign();
                //根据品种ID获取交易所ID
                CM_BreedClass cmBreedClass = cM_BreedClassBLL.GetModel(BreedClassID);
                if (cmBreedClass != null)
                {
                    bourseTypeID = (int)cmBreedClass.BourseTypeID;
                }
                //获取撮合中心记录
                List<ManagementCenter.Model.RC_MatchCenter> rcMatchCenters = rcMatchCenterBll.GetListArray(string.Empty);
                //撮合中心记录存在
                if (rcMatchCenters.Count > 0)
                {
                    //根据交易所类型ID，获取撮合机记录
                    List<ManagementCenter.Model.RC_MatchMachine> rcMatchMachines = rcMatchMachineBll.GetListArray(string.Format("BourseTypeID={0}", bourseTypeID));

                    //有撮合机存在，则向可交易商品_撮合机_分配表中添加新代码
                    if (rcMatchMachines.Count > 0)
                    {
                        //判断代码分配记录已存在则退出 add by 董鹏 2010-03-31
                        if (rcTradeCommodityAssignBll.Exists(CommodityName, rcMatchMachines[0].MatchMachineID))
                        {
                            return;
                        }
                        rcTradeCommodityAssignModel.CommodityCode = CommodityName;
                        rcTradeCommodityAssignModel.MatchMachineID = rcMatchMachines[0].MatchMachineID;
                        rcTradeCommodityAssignBll.Add(rcTradeCommodityAssignModel);
                    }
                    else
                    {
                        //撮合机不存在，则添加撮合机
                        rcMatchMachine.MatchCenterID = rcMatchCenters[0].MatchCenterID;
                        rcMatchMachine.MatchMachineName = "自动添加的撮合机名称";//保证不重复
                        rcMatchMachine.BourseTypeID = bourseTypeID;
                        int result = rcMatchMachineBll.Add(rcMatchMachine);
                        if (result > 0)
                        {
                            rcTradeCommodityAssignModel.CommodityCode = CommodityName;
                            rcTradeCommodityAssignModel.MatchMachineID = rcMatchMachines[0].MatchMachineID;
                            rcTradeCommodityAssignBll.Add(rcTradeCommodityAssignModel);
                        }
                        else
                        {
                            //写错误日志
                            LogHelper.WriteDebug("添加撮合机失败");
                        }

                    }
                }
                else
                {
                    //添加撮合中心记录
                    rcMatchCenterModel.MatchCenterName = "自动添加的撮合中心名称";
                    rcMatchCenterModel.IP = "127.0.0.1";
                    rcMatchCenterModel.Port = 9281;
                    rcMatchCenterModel.CuoHeService = "OrderDealRpt";
                    rcMatchCenterModel.XiaDanService = "DoOrderService";
                    int addrcMatchCResultID = rcMatchCenterBll.Add(rcMatchCenterModel);
                    if (addrcMatchCResultID > 0)
                    {
                        //撮合中心添加成功，则添加撮合机
                        rcMatchMachine.MatchCenterID = addrcMatchCResultID;// rcMatchCenterslist[0].MatchCenterID; 
                        rcMatchMachine.MatchMachineName = "自动添加的撮合机名称";//保证不重复
                        rcMatchMachine.BourseTypeID = bourseTypeID;
                        int resultMatchMachineID = rcMatchMachineBll.Add(rcMatchMachine);
                        //撮合机添加成功，则向可交易商品_撮合机_分配表中添加新代码
                        if (resultMatchMachineID > 0)
                        {
                            rcTradeCommodityAssignModel.CommodityCode = CommodityName;
                            rcTradeCommodityAssignModel.MatchMachineID = resultMatchMachineID;// rcMatchMachineslist[0].MatchMachineID;
                            rcTradeCommodityAssignBll.Add(rcTradeCommodityAssignModel);
                        }
                        else
                        {
                            //写错误日志
                            LogHelper.WriteDebug("向可交易商品_撮合机_分配表中添加新代码失败");
                        }
                    }
                    else
                    {
                        //写错误日志
                        LogHelper.WriteDebug("添加撮合中心失败");
                    }

                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-7004";
                string errMsg = "执行期货新增代码自动添加到可交易商品撮合机分配表中失败";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

    }
}
