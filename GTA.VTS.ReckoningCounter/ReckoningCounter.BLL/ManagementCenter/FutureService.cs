using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.DevolveVerifyCommonService;

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 提供期货的各种服务
    /// 作者：宋涛
    /// 日期：2008-12-06
    /// </summary>
    public class FutureService
    {
        /// <summary>
        /// 判断当前月是属于那种交割月份类型
        /// Update by:李健华
        /// Update date:2010-03-31
        /// desc.:增加比较的日期，不直接内部取当前日期，由外部传递，这为了清算时获取期货的保证金比例而用
        /// </summary>
        /// <param name="year">合约交割年</param>
        /// <param name="month">合约交割月</param>
        /// <param name="compareDate">要比较的日期</param>
        /// <returns>交割月份类型</returns>
        public static Types.QHCFPositionMonthType CheckMonthType(int year, int month, DateTime compareDate)
        {
            Types.QHCFPositionMonthType monthType = Types.QHCFPositionMonthType.GeneralMonth;

            //int nowYear = DateTime.Now.Year;
            //int nowMonth = DateTime.Now.Month;
            int nowYear = compareDate.Year;
            int nowMonth = compareDate.Month;

            int val = 0;
            if (year == nowYear)
            {
                val = month - nowMonth;
            }
            else if (year > nowYear)
            {
                val = month + (12 - nowMonth);
            }

            switch (val)
            {
                case 1:
                    monthType = Types.QHCFPositionMonthType.OnDeliAgoMonth;
                    break;
                case 2:
                    monthType = Types.QHCFPositionMonthType.OnDeliAgoTwoMonth;
                    break;
                case 3:
                    monthType = Types.QHCFPositionMonthType.OnDeliAgoThreeMonth;
                    break;
            }

            return monthType;
        }

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

        /// <summary>
        /// 检查期货合约是否过期
        /// Update by: 董鹏
        /// Update Date: 2010-03-29
        /// Desc: 此方法没有进行最后交易日判断，同时存在另一个相同的方法，此处改为调用该方法。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool CheckQHContractIsExpired(string code)
        {
            #region 没有做最后交易日的判断
            //int year;
            //int month;
            //GetAgreementTime(code, out year, out month);
            //DateTime now = DateTime.Now;
            ////合约已过期
            //if (year < now.Year)
            //{
            //    return true;
            //}

            ////本年度合约
            //if (year == now.Year)
            //{
            //    //合约已过期
            //    if (month < now.Month)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            #endregion

            return MCService.IsExpireLastedTradeDate(code);
        }

        /// <summary>
        /// 获取根据日期前后推一天的交易日日期与品种ID
        /// Create by: 李健华
        /// Crate Date: 2010-03-31
        /// Desc:获取根据日期前后推一天的交易日日期
        /// </summary>
        /// <param name="date">根据的日期</param>
        /// <param name="breedClassID">所属品种</param>
        /// <returns>返回交易日期</returns>
        public static DateTime GetNowDayBackwardTradeDay(DateTime date, int breedClassID)
        {
            DateTime backwardTradeDay = date;
            bool isTradeDate = false;
            while (!isTradeDate)
            {
                //判断是否是星期六星期日，如果是向后推清算日期
                for (int i = 1; i <= 3; i++)
                {
                    backwardTradeDay = backwardTradeDay.AddDays(1);   //向后推日期，如果是星期六星期日再向后推
                    if (backwardTradeDay.DayOfWeek == DayOfWeek.Sunday || backwardTradeDay.DayOfWeek == DayOfWeek.Saturday)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                //获取到要清算的日期再判断是否是非交易日期,如果是再向后推(while)

                #region old code
                ////获取 所有期货的交易所的ID
                ////IList<CM_BourseType> cm_bourseList = MCService.CommonPara.GetAllBourseType();
                //List<int> cm_bourseList = new List<int>();
                ////获取所有品种类别
                //IList<CM_BreedClass> cm_breedClass = MCService.CommonPara.GetAllBreedClass();

                ////过期不是期货的不用判断
                //foreach (var item in cm_breedClass)
                //{
                //    switch ((Types.BreedClassTypeEnum)item.BreedClassTypeID)
                //    {
                //        case Types.BreedClassTypeEnum.Stock:
                //            break;
                //        case Types.BreedClassTypeEnum.CommodityFuture:

                //        case Types.BreedClassTypeEnum.StockIndexFuture:
                //            if (!cm_bourseList.Contains(item.BourseTypeID.Value))
                //            {
                //                cm_bourseList.Add(item.BourseTypeID.Value);
                //            }
                //            break;
                //        case Types.BreedClassTypeEnum.HKStock:
                //            break;
                //        default:
                //            break;
                //    }

                //}
                //foreach (var item in cm_bourseList)
                //{

                //    isTradeDate = MCService.CommonPara.IsTradeDate(item, backwardTradeDay);
                //    if (!isTradeDate)
                //    {
                //        break;
                //    }
                //}
                #endregion
 
                //获取品种类别获取所属的交易所
                CM_BreedClass cm_breedClass = MCService.CommonPara.GetBreedClassByBreedClassID(breedClassID);
                if (cm_breedClass != null)
                {
                    isTradeDate = MCService.CommonPara.IsTradeDate(cm_breedClass.BourseTypeID.Value, backwardTradeDay);
                    if (!isTradeDate)
                    {
                        break;
                    }
                }
                else
                {
                    //为了不造成死循环直接退出
                    break;
                }
            }
            return backwardTradeDay;
        }
    }

}
