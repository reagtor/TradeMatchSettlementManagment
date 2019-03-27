using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.BLL.CommonTable;

namespace ManagementCenter.BLL.CommonTable
{
    /// <summary>
    /// 描述:期货代码初始化
    /// 作者：程序员：熊晓凌
    /// 日期：2009-01-04
    /// Desc: 使用代码规则管理器生成代码
    /// Update by:董鹏
    /// Update Date: 2010-03-10
    /// </summary>
    public class QHCodeInit
    {
        /// <summary>
        /// 添加期货品种时初始化一个周期的和约代码
        /// </summary>
        /// <param name="BreedClassID"></param>
        public void QHCommdityCodeInit(int BreedClassID)
        {
            CommodityCodeUpdate CCP = new CommodityCodeUpdate();
            //根据品种ID，找到品种的前缀代码
            string QH_PrefixCode = CCP.GetQH_PrefixCodeByID(BreedClassID);

            //根据品种ID，找到品种的名称
            string QH_breedclassName = CCP.GetBreedClassNameByID(BreedClassID);

            #region 创建代码规则管理器 add by 董鹏 2010-03-10
            //根据品种获取交易所
            CM_BreedClassBLL bc = new CM_BreedClassBLL();
            CM_BourseTypeBLL bt = new CM_BourseTypeBLL();
            var bourseType = bt.GetModel(bc.GetModel(BreedClassID).BourseTypeID.Value);

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
            #endregion

            CM_CommodityDAL CommodityDAL = new CM_CommodityDAL();
            CM_Commodity CM_Commodity = new CM_Commodity();
            CM_Commodity.BreedClassID = BreedClassID;

            CM_Commodity.GoerScale = decimal.MaxValue;
            CM_Commodity.LabelCommodityCode = null;
            CM_Commodity.StockPinYin = null;
            CM_Commodity.MarketDate = DateTime.Now.AddYears(-1);//暂时将日期定为一年前 DateTime.MaxValue;
            CM_Commodity.turnovervolume = null;
            CM_Commodity.IsExpired = (int)Types.IsYesOrNo.No;//初始化期货代码时，默认没有过期
            CM_Commodity.ISSysDefaultCode = (int)Types.IsYesOrNo.Yes;//新增期货代码时，默认代码没有过期

            QH_AgreementDeliveryMonthDAL AgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();
            List<QH_AgreementDeliveryMonth> L = AgreementDeliveryMonthDAL.GetListArray(string.Format("BreedClassID={0}", BreedClassID));

            foreach (QH_AgreementDeliveryMonth AgreementDeliveryMonth in L)
            {
                if ((int)AgreementDeliveryMonth.MonthID > System.DateTime.Now.Month && (int)AgreementDeliveryMonth.MonthID <= 12)
                {
                    //string Code = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) +
                    //    CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID);

                    //使用代码规则管理器生成代码
                    string Code = codeRule.GetCode(QH_PrefixCode, DateTime.Now.Year.ToString(), CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID));
                    CM_Commodity.CommodityCode = Code;
                    CM_Commodity.CommodityName = Code;// QH_breedclassName + Code.Substring(Code.Length - 4);
                    CommodityDAL.Add(CM_Commodity);
                }
                if ((int)AgreementDeliveryMonth.MonthID < System.DateTime.Now.Month)
                {
                    //string Code = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) +
                    //    CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID);

                    //使用代码规则管理器生成代码
                    string Code = codeRule.GetCode(QH_PrefixCode, DateTime.Now.AddYears(1).Year.ToString(), CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID));
                    CM_Commodity.CommodityCode = Code;
                    CM_Commodity.CommodityName = Code;// QH_breedclassName + Code.Substring(Code.Length - 4);
                    CommodityDAL.Add(CM_Commodity);
                }
                if ((int)AgreementDeliveryMonth.MonthID == System.DateTime.Now.Month)
                {
                    int lasttradingday = CCP.GetLastTradingDay(CCP.GetLastTradingDayEntity(BreedClassID), BreedClassID);
                    string Code = string.Empty;
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                      //  Code = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) +
                      //CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID);

                        //使用代码规则管理器生成代码
                        Code = codeRule.GetCode(QH_PrefixCode, DateTime.Now.Year.ToString(), CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID));
                    }
                    else
                    {
                     //   Code = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) +
                     //CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID);

                        //使用代码规则管理器生成代码
                        Code = codeRule.GetCode(QH_PrefixCode, DateTime.Now.AddYears(1).Year.ToString(), CommodityCodeUpdate.GetTwoLenMonth((int)AgreementDeliveryMonth.MonthID));
                    }
                    CM_Commodity.CommodityCode = Code;
                    CM_Commodity.CommodityName = Code;// QH_breedclassName + Code.Substring(Code.Length - 4);
                    CommodityDAL.Add(CM_Commodity);
                }
                if ((int)AgreementDeliveryMonth.MonthID == 13)
                {
                    SpecialQHCodeInit(BreedClassID, QH_PrefixCode, QH_breedclassName);
                }
            }
        }

        private void SpecialQHCodeInit(int BreedClassID, string QH_PrefixCode, string QH_breedclassName)
        {
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

            CommodityCodeUpdate CCP = new CommodityCodeUpdate();
            int lasttradingday = CCP.GetLastTradingDay(CCP.GetLastTradingDayEntity(BreedClassID), BreedClassID);
            string Code1 = string.Empty;
            string Code2 = string.Empty;
            string Code3 = string.Empty;
            string Code4 = string.Empty;

            #region
            switch (System.DateTime.Now.Month)
            {
                case 1:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "02";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "03";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "06";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "01";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    }
                    break;
                case 2:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "03";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "06";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "02";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "04";
                    }
                    break;
                case 3:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "04";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "06";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "03";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "05";
                    }
                    break;
                case 4:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "05";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "06";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "04";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    }
                    break;
                case 5:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "06";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "05";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "07";
                    }
                    break;
                case 6:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "07";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "06";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "08";
                    }
                    break;
                case 7:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "08";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "07";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "03";
                    }
                    break;
                case 8:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "03";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "08";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "10";
                    }
                    break;
                case 9:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "10";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "03";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "09";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "11";
                    }
                    break;
                case 10:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "11";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "03";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "10";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "06";
                    }
                    break;
                case 11:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "03";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "06";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "11";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "01";
                    }
                    break;
                case 12:
                    Code2 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "01";
                    Code3 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "03";
                    Code4 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "06";
                    if (System.DateTime.Now.Day < lasttradingday)
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now) + "12";
                    }
                    else
                    {
                        Code1 = QH_PrefixCode + CommodityCodeUpdate.GetTwoLenYear(System.DateTime.Now.AddYears(1)) + "02";
                    }
                    break;
            }
            #endregion

            CM_CommodityDAL CommodityDAL = new CM_CommodityDAL();
            CM_Commodity CM_Commodity = new CM_Commodity();

            CM_Commodity.BreedClassID = BreedClassID;
            CM_Commodity.GoerScale = decimal.MaxValue;
            CM_Commodity.LabelCommodityCode = null;
            CM_Commodity.StockPinYin = null;
            CM_Commodity.MarketDate = DateTime.MaxValue;
            CM_Commodity.turnovervolume = null;
            CM_Commodity.IsExpired = (int) Types.IsYesOrNo.No;//初始化期货代码时，默认代码没有过期
            CM_Commodity.ISSysDefaultCode = (int)Types.IsYesOrNo.Yes;//初始化期货代码时，默认是系统代码
            CM_Commodity.CommodityCode = Code1;
            CM_Commodity.CommodityName = Code1;// QH_breedclassName + Code1.Substring(Code1.Length - 4);
            CommodityDAL.Add(CM_Commodity);
            CM_Commodity.CommodityCode = Code2;
            CM_Commodity.CommodityName = Code2;// QH_breedclassName + Code2.Substring(Code2.Length - 4);
            CommodityDAL.Add(CM_Commodity);
            CM_Commodity.CommodityCode = Code3;
            CM_Commodity.CommodityName = Code3;// QH_breedclassName + Code3.Substring(Code3.Length - 4);
            CommodityDAL.Add(CM_Commodity);
            CM_Commodity.CommodityCode = Code4;
            CM_Commodity.CommodityName = Code4;// QH_breedclassName + Code4.Substring(Code4.Length - 4);
            CommodityDAL.Add(CM_Commodity);
        }


        /// <summary>
        /// 添加非交易日
        /// </summary>
        /// <param name="year"></param>
        public static void InitNotTrandingDay(int year)
        {
            try
            {
                CM_BourseTypeDAL BourseTypeDAL = new CM_BourseTypeDAL();
                List<CM_BourseType> l = BourseTypeDAL.GetListArray(string.Empty);
                DateTime startDate = DateTime.Parse(string.Format("{0}-01-01", year));
                DateTime endDate = DateTime.Parse(string.Format("{0}-12-31", year));
                CM_NotTradeDateDAL NotTradeDateDAL = new CM_NotTradeDateDAL();
                CM_NotTradeDate NotTradeDate = new CM_NotTradeDate();

                foreach (CM_BourseType type in l)
                {
                    for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                    {
                        DayOfWeek t = dt.DayOfWeek;
                        if (t == DayOfWeek.Sunday || t == DayOfWeek.Saturday)
                        {

                            List<ManagementCenter.Model.CM_NotTradeDate> l_NotTradeDate =
                                NotTradeDateDAL.GetListArray(string.Format("NotTradeDay='{0}' AND BourseTypeID={1}", dt, type.BourseTypeID));
                            if (l_NotTradeDate == null || l_NotTradeDate.Count < 1)
                            {
                                NotTradeDate.NotTradeDay = dt;
                                NotTradeDate.BourseTypeID = type.BourseTypeID;
                                NotTradeDateDAL.Add(NotTradeDate);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errMsg = "初始化非交易日失败!";
                LogHelper.WriteError(errMsg, ex);
            }
        }
    }
}
