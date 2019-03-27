using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 描述：界面数据绑定
    /// 作者：刘书伟
    /// 日期：2008-12-10
    /// </summary>
    public class BindData
    {
        #region 绑定数据，1是，2否

        /// <summary>
        ///  绑定数据，1是，2否
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListYesOrNo()
        {
            UComboItem _item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _item = new UComboItem("是", (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes);
            listUComboItem.Add(_item);
            _item = new UComboItem("否", (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No);
            listUComboItem.Add(_item);
            return listUComboItem;
        }

        #endregion

        #region 绑定品种类型

        /// <summary>
        /// 绑定品种类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListBreedClassType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("现货",//("股票现货",
                                   (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.Stock);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("商品期货",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.CommodityFuture);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("股指期货",
                                   (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("港股",
                                   (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.HKStock);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region  绑定股票性质

        /// <summary>
        /// 绑定股票性质
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListStockNature()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("正常股票",
                                   (int)GTA.VTS.Common.CommonObject.Types.StockNatureEnum.Normal);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("ST股票",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.StockNatureEnum.ST);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定最小变动价位范围值,交易费用(交易规则_取值类型表中的2个值)

        /// <summary>
        ///  绑定最小变动价位范围值,交易费用(交易规则_取值类型表中的2个值)
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListValueType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("单一值", (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Single);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("范围值",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Scope);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定交易规则_取值类型

        /// <summary>
        /// 绑定交易规则_取值类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListCMValueType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("单一值", (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Single);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("范围值",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Scope);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("成交额",
                                   (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Turnover);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("股", (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Thigh);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("单边买",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.SingleBuy);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("单边卖",
                                   (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.SingleSell);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("双边", (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.TwoEdge);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定印花税取值类型

        /// <summary>
        /// 绑定印花税取值类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListStampDutyType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("单边买",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.SingleBuy);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("单边卖",
                                   (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.SingleSell);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("双边", (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.TwoEdge);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定现货过户费取值类型

        /// <summary>
        /// 绑定现货过户费取值类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListXHTransferTollType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("成交额",
                                   (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Turnover);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("股", (int)GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Thigh);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定交易方向

        /// <summary>
        /// 绑定交易方向
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListTransDire()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("买", (int)GTA.VTS.Common.CommonObject.Types.TransactionDirection.Buying);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("卖",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.TransactionDirection.Selling);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定币种类型

        /// <summary>
        /// 绑定币种类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListCurrencyType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("人民币", (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("港币",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.CurrencyType.HK);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("美元",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.CurrencyType.US);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定账号所属类型

        /// <summary>
        /// 绑定账号所属类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListAccountType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("现货资金账号", (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotCapital);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("现货持仓账号",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.AccountAttributionType.SpotHold);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("期货资金账号",
                                   (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesCapital);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("期货持仓账号 ", (int)GTA.VTS.Common.CommonObject.Types.AccountAttributionType.FuturesHold);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("银行账号",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定单位类型

        /// <summary>
        /// 绑定单位类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListUnitType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("股", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Thigh);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("手",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.UnitType.Hand);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("张",
                                   (int)GTA.VTS.Common.CommonObject.Types.UnitType.Sheet);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("点 ", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Point);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("吨",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.UnitType.Ton);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("克",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.UnitType.Gram);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("指数点",
            //                       (int)
            //                       GTA.VTS.Common.CommonObject.Types.UnitType.IndexPoints);
            //listUComboItem.Add(_Item);
            _Item = new UComboItem("份",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.UnitType.Share);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定现货单笔最大委托量单位 GetBindListXHAboutUnit()

        /// <summary>
        /// 绑定现货相关单位(单笔最大委托量单位,计价单位,交易单位,行情成交量单位,最小交易单位) GetBindListXHAboutUnit()
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListXHAboutUnit()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("股", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Thigh);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("手", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Hand);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("份", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Share);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("张", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Sheet);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定港股相关单位(计价单位,行情成交量单位) GetBindListHKAboutUnit()

        /// <summary>
        /// 绑定港股相关单位(计价单位,行情成交量单位) GetBindListXHAboutUnit()
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListHKAboutUnit()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("股", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Thigh);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("手", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Hand);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region  绑定期货(计价单位)  GetBindListQHPriceUnit()

        /// <summary>
        /// 绑定期货(计价单位) 
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHPriceUnit()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("吨", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Ton);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("点", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Point);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("克", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Gram);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定期货相关单位 GetBindListQHAboutUnit()

        /// <summary>
        /// 绑定期货相关单位(交易单位,行情成交量单位) 
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHAboutUnit()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("手", (int)GTA.VTS.Common.CommonObject.Types.UnitType.Hand);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region  绑定现货有效申报类型

        /// <summary>
        /// 绑定现货有效申报类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListXHValidDeclareType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            //_Item = new UComboItem("最近成交价的上下百分比", (int) GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceUpperDownScale);
            _Item = new UComboItem("最近成交(%)", (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceUpperDownScale);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("不高于即时揭示的最低卖出价格的百分比且不低于即时揭示的最高买入价格的百分比",
            _Item = new UComboItem("即时揭示价(%)",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.NotHighSalePriceScaleAndNotLowBuyPriceScale);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("低于买一价的个价位与卖一价之间或低于买一价与高于卖一价的个价位之间",
            //_Item = new UComboItem("价位",
            //                       (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.DownBuyOneAndSaleOne);
            //listUComboItem.Add(_Item);
            //_Item = new UComboItem("最近成交价上下各多少元", (int) GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceOnDownMoney);
            _Item = new UComboItem("最近成交价(元)", (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceOnDownMoney);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region  绑定现货有效申报类型(显示所有文字)

        /// <summary>
        /// 绑定现货有效申报类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListXHValidDeclareTypeDisplayAll()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            // _Item = new UComboItem("最近成交价的上下百分比", (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceUpperDownScale);
            _Item = new UComboItem("最近成交(%)", (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceUpperDownScale);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("不高于即时揭示的最低卖出价格的百分比且不低于即时揭示的最高买入价格的百分比",
            _Item = new UComboItem("即时揭示价(%)",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.NotHighSalePriceScaleAndNotLowBuyPriceScale);
            listUComboItem.Add(_Item);
            // _Item = new UComboItem("低于买一价的个价位与卖一价之间或低于买一价与高于卖一价的个价位之间",
            //_Item = new UComboItem("价位",
            //                       (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.DownBuyOneAndSaleOne);
            //listUComboItem.Add(_Item);
            //_Item = new UComboItem("最近成交价上下各多少元", (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceOnDownMoney);
            _Item = new UComboItem("最近成交价(元)", (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.BargainPriceOnDownMoney);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定现货品种涨跌幅控制类型

        /// <summary>
        /// 绑定现货品种涨跌幅控制类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListXHHighLowType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            //_Item = new UComboItem("新股上市，增发上市，暂停后开始交易和其他日期",
            _Item = new UComboItem("股票",
                                   (int)GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NewThighAddFatStopAfterOrOtherDate);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("权证涨跌幅",
            _Item = new UComboItem("权证",
                                  (int)GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.RightPermitHighLow);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("新基金上市，增发上市，暂停后开始交易和其他日期",
            _Item = new UComboItem("基金",
                                   (int)GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NewFundAddFatStopAfterOrOtherDate);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("无涨跌幅限制",
            _Item = new UComboItem("债券",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NotHighLowControl);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("港股",
            //                      (int)
            //                      GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NotHighLowControl);
            //listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion


        #region 绑定现货品种涨跌幅控制类型(显示所有文字)

        /// <summary>
        /// 绑定现货品种涨跌幅控制类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListXHHighLowTypeDisplayAll()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            // _Item = new UComboItem("新股上市，增发上市，暂停后开始交易和其他日期",
            _Item = new UComboItem("股票",
                                   (int)GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NewThighAddFatStopAfterOrOtherDate);
            listUComboItem.Add(_Item);
            // _Item = new UComboItem("无涨跌幅限制",
            _Item = new UComboItem("债券",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NotHighLowControl);
            listUComboItem.Add(_Item);
            // _Item = new UComboItem("权证涨跌幅",
            _Item = new UComboItem("权证",
                                   (int)GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.RightPermitHighLow);
            listUComboItem.Add(_Item);
            // _Item = new UComboItem("新基金上市，增发上市，暂停后开始交易和其他日期",
            _Item = new UComboItem("基金",
                                   (int)GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NewFundAddFatStopAfterOrOtherDate);
            listUComboItem.Add(_Item);
            //_Item = new UComboItem("港股",
            //                       5);
            ////GTA.VTS.Common.CommonObject.Types.XHSpotHighLowControlType.NotHighLowControl);
            //listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion


        #region 绑定期货最后交易日类型

        /// <summary>
        /// 绑定期货最后交易日类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHLastTradingDayType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("交割月份+第几个自然日",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndDay);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("交割月份+倒数/顺数+第几个交易日",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndDownOrShunAndWeek);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("交割月份+第几周+星期几",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndWeek);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("交割月份前一个月份+倒数/顺数+第几个交易日",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 期货交割月份类型(针对期货持仓及保证金)

        /// <summary>
        /// 期货交割月份类型(针对期货持仓及保证金)
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHCFPositionMonthType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("交割月份",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHCFPositionMonthType.OnDelivery);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("一般月份",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHCFPositionMonthType.GeneralMonth);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("交割月份前一个月",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHCFPositionMonthType.OnDeliAgoMonth);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("交割月份前二个月",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHCFPositionMonthType.OnDeliAgoTwoMonth);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("交割月份前三个月",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHCFPositionMonthType.OnDeliAgoThreeMonth);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region  绑定委托指令类型

        /// <summary>
        /// 绑定委托指令类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListMarketPriceType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("市价", (int)GTA.VTS.Common.CommonObject.Types.MarketPriceType.MarketPrice);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("限价",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.MarketPriceType.otherPrice);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定期货涨跌停类型

        /// <summary>
        /// 绑定期货涨跌停类型(根据2009。05。15界面确认结果修改)
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHHighLowStopType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            //_Item = new UComboItem("不超过上一交易日结算价",
            //_Item = new UComboItem("不超过上一......",
            //                       (int) GTA.VTS.Common.CommonObject.Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice);
            _Item = new UComboItem("比例",
                       (int)GTA.VTS.Common.CommonObject.Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice);

            listUComboItem.Add(_Item);
            //_Item = new UComboItem("每吨不高于或低于上一交易日结算价格",
            _Item = new UComboItem("金额",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定期货涨跌停类型(显示所有文字)(根据2009。05。15界面确认结果修改)

        /// <summary>
        /// 绑定期货涨跌停类型(显示所有文字)(根据2009。05。15界面确认结果修改)
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHHighLowStopALLType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            //_Item = new UComboItem("不超过上一交易日结算价",
            //                       (int)GTA.VTS.Common.CommonObject.Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice);
            _Item = new UComboItem("比例",
           (int)GTA.VTS.Common.CommonObject.Types.QHHighLowStopScopeType.NoMoreAgoTradDayClearPrice);

            listUComboItem.Add(_Item);
            //_Item = new UComboItem("每吨不高于或低于上一交易日结算价格",
            //                       (int)
            //                       GTA.VTS.Common.CommonObject.Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice);
            _Item = new UComboItem("金额",
                       (int)
                       GTA.VTS.Common.CommonObject.Types.QHHighLowStopScopeType.TonNotHighOrLowAgoTradDayClearPrice);

            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 期货每周星期几

        /// <summary>
        /// 期货每周星期几
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHWeek()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("星期日",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHWeek.Sunday);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("星期一",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHWeek.Monday);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("星期二",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHWeek.Tuesday);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("星期三",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHWeek.Wednesday);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("星期四",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHWeek.Thursday);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("星期五",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHWeek.Friday);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("星期六",
                                   (int)GTA.VTS.Common.CommonObject.Types.QHWeek.Saturday);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定期货最后交易日是顺数或倒数

        /// <summary>
        /// 绑定期货最后交易日是顺数或倒数
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHLastTradDayIsSequence()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("顺数", (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayIsSequence.Order);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("倒数",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHLastTradingDayIsSequence.Reverse);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定商品期货_持仓取值类型

        /// <summary>
        /// 绑定商品期货_持仓取值类型
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHPositionValueType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("持仓量", (int)GTA.VTS.Common.CommonObject.Types.QHPositionValueType.Positions);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("百分比",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHPositionValueType.Scales);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定持仓和保证金控制类型

        /// <summary>
        /// 绑定持仓和保证金控制类型 
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListQHPositionBailType()
        {
            UComboItem _Item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _Item = new UComboItem("单边持仓", (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.SinglePosition);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("双边持仓",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHPositionBailType.TwoPosition);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("自然日",
                                   (int)
                                   GTA.VTS.Common.CommonObject.Types.QHPositionBailType.ByDays);
            listUComboItem.Add(_Item);
            _Item = new UComboItem("交易日",
                                  (int)
                                  GTA.VTS.Common.CommonObject.Types.QHPositionBailType.ByTradeDays);
            listUComboItem.Add(_Item);
            return listUComboItem;
        }

        #endregion

        #region 绑定期货交易费用类型，1(按成交量)交易单位手续费，2(按成交额)成交金额手续费

        /// <summary>
        /// 绑定期货交易费用类型，1(按成交量)交易单位手续费，2(按成交额)成交金额手续费
        /// </summary>
        /// <returns></returns>
        public static List<UComboItem> GetBindListFutrueCostType()
        {
            UComboItem _item;
            List<UComboItem> listUComboItem = new List<UComboItem>();
            _item = new UComboItem("按成交量", (int)GTA.VTS.Common.CommonObject.Types.FutrueCostType.TradeUnitCharge);
            listUComboItem.Add(_item);
            _item = new UComboItem("按成交额", (int)GTA.VTS.Common.CommonObject.Types.FutrueCostType.TurnoverRateOfSerCha);
            listUComboItem.Add(_item);
            return listUComboItem;
        }

        #endregion
       
    }
}