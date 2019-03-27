#region Using Namespace

using System.Runtime.Serialization;

#endregion

namespace GTA.VTS.Common.CommonObject
{
    public class Types
    {
        #region AccountAttributionType enum

        /// <summary>
        /// 账号所属类别
        /// </summary>
        [DataContract]
        public enum AccountAttributionType
        {
            /// <summary>
            /// 现货资金账号
            /// </summary>
            [EnumMember]
            SpotCapital = 1,
            /// <summary>
            /// 现货持仓账号
            /// </summary>
            [EnumMember]
            SpotHold = 2,
            /// <summary>
            /// 期货资金账号
            /// </summary>
            [EnumMember]
            FuturesCapital = 3,
            /// <summary>
            /// 期货持仓账号 
            /// </summary>
            [EnumMember]
            FuturesHold = 4,
            /// <summary>
            /// 银行账号
            /// </summary>
            [EnumMember]
            BankAccount = 5
        }

        #endregion
        /// <summary>
        /// 账号所属类型对应表DB_AccountType
        /// Create By:李健华
        /// Create Date:2009-07-28
        /// Desc.:添加账号所属类型对应表DB_AccountType
        /// </summary>
        [DataContract]
        public enum AccountType
        {
            /// <summary>
            /// 1	银行帐号	test	5	NULL
            /// </summary>
            [EnumMember]
            BankAccount = 1,
            /// <summary>
            /// 2	证券资金帐户	NULL	1	3
            /// </summary>
            [EnumMember]
            StockSpotCapital = 2,
            /// <summary>
            /// 3	证券股东代码	NULL	2	NULL
            /// </summary>
            [EnumMember]
            StockSpotHoldCode = 3,
            /// <summary>
            /// 4	商品期货资金帐号	NULL	3	5
            /// </summary>
            [EnumMember]
            CommodityFuturesCapital = 4,
            /// <summary>
            /// 5	商品期货交易编码	NULL	4	NULL
            /// </summary>
            [EnumMember]
            CommodityFuturesHoldCode = 5,
            /// <summary>
            /// 6	股指期货资金帐号	NULL	3	7
            /// </summary>
            [EnumMember]
            StockFuturesCapital = 6,
            /// <summary>
            /// 7	证券股东代码	NULL	4	NULL
            /// </summary>
            [EnumMember]
            StockFuturesHoldCode = 7,
            /// <summary>
            /// 8	港股资金帐户	NULL	1	9
            /// </summary>
            [EnumMember]
            HKSpotCapital = 8,
            /// <summary>
            /// 9	港股股东代码	NULL	2	NULL
            /// </summary>
            [EnumMember]
            HKSpotHoldCode = 9
        }

        #region BreedClassTypeEnum enum

        /// <summary>
        /// 品种类型
        /// Update by:李健华
        /// Update date:2009-10-16
        /// Desc.：增加港股品种类型枚举
        /// </summary>
        public enum BreedClassTypeEnum
        {
            /// <summary>
            /// 现货(把股票现货改成现货09.06.11)
            /// </summary>
            Stock = 1,
            /// <summary>
            /// 商品期货
            /// </summary>
            CommodityFuture = 2,
            /// <summary>
            /// 股指期货
            /// </summary>
            StockIndexFuture = 3,
            /// <summary>
            /// 港股
            /// </summary>
            HKStock = 4,

        }

        #endregion

        #region 用户角色

        /// <summary>
        /// 用户角色
        /// </summary>
        public enum UserId
        {
            /// <summary>
            /// 管理员
            /// </summary>
            Manager = 1,
            /// <summary>
            /// 交易员
            /// </summary>
            Trader = 2
        }

        #endregion

        #region CurrencyType enum

        /// <summary>
        /// 币种
        /// </summary>
        public enum CurrencyType
        {
            /// <summary>
            /// 人民币
            /// </summary>
            RMB = 1,
            /// <summary>
            /// 港币
            /// </summary>
            HK = 2,
            /// <summary>
            /// 美元
            /// </summary>
            US = 3
        }

        #endregion

        #region GetValueTypeEnum enum

        /// <summary>
        /// 取值类型
        /// </summary>
        public enum GetValueTypeEnum
        {
            /// <summary>
            /// 单一值
            /// </summary>
            Single = 1,
            /// <summary>
            /// 范围值
            /// </summary>
            Scope = 2,
            /// <summary>
            ///  成交额
            /// </summary>
            Turnover = 3,
            /// <summary>
            /// 股
            /// </summary>
            Thigh = 4,
            /// <summary>
            /// 单边买
            /// </summary>
            SingleBuy = 5,
            /// <summary>
            /// 单边卖
            /// </summary>
            SingleSell = 6,
            /// <summary>
            /// 双边
            /// </summary>
            TwoEdge = 7,
            /// <summary>
            /// 无
            /// </summary>
            No = 8
        }

        #endregion

        #region MarketPriceType enum

        /// <summary>
        /// 市价类型判断0
        /// </summary>
        [DataContract]
        public enum MarketPriceType
        {
            /// <summary>
            /// 市价委托
            /// </summary>
            [EnumMember]
            MarketPrice = 0,

            /// <summary>
            /// 其他
            /// </summary>
            [EnumMember]
            otherPrice = 1
        }

        #endregion

        #region HKPriceType enum

        /// <summary>
        /// 港股价格类型
        /// Create by:李健华
        /// Create date:2009-10-16
        /// </summary>
        [DataContract]
        public enum HKPriceType
        {
            /// <summary>
            /// 限价盘
            /// </summary>
            [EnumMember]
            LO = 1,

            /// <summary>
            /// 增强限价盘
            /// </summary>
            [EnumMember]
            ELO = 2,

            /// <summary>
            /// 特别限价盘
            /// </summary>
            [EnumMember]
            SLO = 3
        }

        #endregion

        #region  [(刘书伟)]

        #region HKValidPriceType enum

        /// <summary>
        /// 港股有效报价类型
        /// Create By:李健华
        /// Create Date:2009-10-23
        /// </summary>
        public enum HKValidPriceType
        {
            /// <summary>
            /// 有现存买盘及沽盘的情况
            /// </summary>
            SellAndBuy = 1,

            /// <summary>
            /// 没有现存买盘的情况
            /// </summary>
            NoBuy = 2,

            /// <summary>
            /// 没有现存沽盘的情况
            /// </summary>
            NoSell = 3,

            /// <summary>
            /// 没有现存买卖盘的情况
            /// </summary>
            NoSellAndBuy = 4
        }

        #endregion

        #region HighLowRangeType enum

        /// <summary>
        /// 涨跌幅的实际逻辑分类
        /// 根据需求，由XHValidDeclareType和XHSpotHighLowControlType综合而来
        /// 另外附加期货的两种涨跌停类型
        /// </summary>
        [DataContract]
        public enum HighLowRangeType
        {
            /// <summary>
            /// 昨日收盘价的上下百分比
            /// </summary>
            [EnumMember]
            YesterdayCloseScale = 1,

            /// <summary>
            /// 最近成交价的上下百分比
            /// </summary>
            [EnumMember]
            RecentDealScale = 2,

            /// <summary>
            /// 买一卖一百分比(不高于即时揭示的最低卖出价格的百分比且不低于即时揭示的最高买入价格的百分比)
            /// 上限：卖一价的110%			下限：买一价的90%
            /// </summary>
            [EnumMember]
            Buy1Sell1Scale = 3,

            /// <summary>
            /// 权证涨跌幅=>返回上下限价格
            /// </summary>
            [EnumMember]
            RightPermitHighLow = 4,

            /// <summary>
            /// 港股买卖价位([低于买一价的n个价位]与卖一价之间或低于买一价与[高于卖一价的n个价位]之间)
            /// =>返回上下限价格
            /// 买	上限：卖一价								下限：低于买一价的24个价位（买1-24）
            /// 卖 	上限：高于卖一价的24个价位(卖1+24)			下限：买一价
            /// </summary>
            [EnumMember]
            HongKongPrice = 5,

            /// <summary>
            /// 最近成交价上下各多少元=>返回上下限的增量（多少元）
            /// </summary>
            [EnumMember]
            RecentDealNumber = 6,

            /// <summary>
            /// (期货)上一交易日结算价的上下百分比
            /// </summary>
            [EnumMember]
            YesterdayBalanceScale = 7,

            /// <summary>
            /// (期货)上一交易日结算价的上下限的增量(多少钱)
            /// </summary>
            [EnumMember]
            YesterdayBalanceNumber = 8
        }

        #endregion

        #region IsYesOrNo enum

        /// <summary>
        /// 判断是否
        /// </summary>
        public enum IsYesOrNo
        {
            /// <summary>
            /// 是
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 否
            /// </summary>
            No = 2
        }

        #endregion

        #region IsCodeFormSource enum

        /// <summary>
        /// 判断可交易商品_撮合机_分配表中的撮合代码来源于哪个表
        /// </summary>
        public enum IsCodeFormSource
        {
            /// <summary>
            /// 来源于CM_Commodity(交易商品)
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 来源于HK_Commodity(港股交易商品)
            /// </summary>
            No = 2
        }

        #endregion

        #region QHCFPositionMonthType enum

        /// <summary>
        /// 期货交割月份类型(针对期货持仓及保证金)
        /// 1、交割月份 2、一般月份 3、交割月份前一个月 4、交割月份前二个月 5、交割月份前三个月
        /// </summary>
        public enum QHCFPositionMonthType
        {
            /// <summary>
            /// 交割月份
            /// </summary>
            OnDelivery = 1,
            /// <summary>
            /// 一般月份
            /// </summary>
            GeneralMonth = 2,
            /// <summary>
            /// 交割月份前一个月
            /// </summary>
            OnDeliAgoMonth = 3,
            /// <summary>
            /// 交割月份前二个月
            /// </summary>
            OnDeliAgoTwoMonth = 4,
            /// <summary>
            /// 交割月份前三个月
            /// </summary>
            OnDeliAgoThreeMonth = 5
        }

        #endregion

        #region QHHighLowStopScopeType enum

        /// <summary>
        /// 期货涨跌停板类型(根据2009。05。15界面确认结果修改)
        /// </summary>
        public enum QHHighLowStopScopeType
        {
            /// <summary>
            /// 比例(不超过上一交易日结算价的)
            /// </summary>
            NoMoreAgoTradDayClearPrice = 1,
            /// <summary>
            /// 金额(每吨不高于或低于上一交易日结算价格的)
            /// </summary>
            TonNotHighOrLowAgoTradDayClearPrice = 2
        }

        #endregion

        #region QHLastTradingDayIsSequence enum

        /// <summary>
        /// 顺数或倒数(最后交易日)
        /// </summary>
        public enum QHLastTradingDayIsSequence
        {
            /// <summary>
            /// 顺数
            /// </summary>
            Order = 1,
            /// <summary>
            /// 倒数
            /// </summary>
            Reverse = 2
        }

        #endregion

        #region QHLastTradingDayType enum

        /// <summary>
        /// 最后交易日类型
        /// </summary>
        public enum QHLastTradingDayType
        {
            /// <summary>
            /// 交割月份+第几个自然日
            /// </summary>
            DeliMonthAndDay = 1,
            /// <summary>
            /// 交割月份+倒数/顺数+第几个交易日
            /// </summary>
            DeliMonthAndDownOrShunAndWeek = 2,
            /// <summary>
            /// 交割月份+第几周+星期几
            /// </summary>
            DeliMonthAndWeek = 3,

            // 原来：交割月份+前一个月份+最后一个交易日
            // 现改为：交割月份前一个月份+倒数/顺数+第几个交易日，因为代码和2是相同的，所以统一成类似的
            // update by 董鹏 2010-03-31
            /// <summary>
            ///交割月份前一个月份+倒数/顺数+第几个交易日
            /// </summary>
            DeliMonthAgoMonthLastTradeDay = 4
        }

        #endregion

        #region QHPositionBailType enum

        /// <summary>
        ///持仓和保证金控制类型 
        /// </summary>
        public enum QHPositionBailType
        {
            /// <summary>
            ///单边持仓 
            /// </summary>
            SinglePosition = 1,
            /// <summary>
            ///双边持仓 
            /// </summary>
            TwoPosition = 2,
            /// <summary>
            ///按自然日天数 
            /// </summary>
            ByDays = 3,
            /// <summary>
            /// 按交易日天数
            /// </summary>
            ByTradeDays = 4
        }

        #endregion

        #region QHPositionValueType enum

        /// <summary>
        /// 商品期货_持仓取值类型
        /// </summary>
        public enum QHPositionValueType
        {
            /// <summary>
            /// 持仓量
            /// </summary>
            Positions = 1,
            /// <summary>
            /// 百分比
            /// </summary>
            Scales = 2
        }

        #endregion

        #region QHWeek enum

        /// <summary>
        /// 每周星期几
        /// </summary>
        public enum QHWeek
        {
            /// <summary>
            /// 星期日
            /// </summary>
            Sunday = 0,
            /// <summary>
            /// 星期一
            /// </summary>
            Monday = 1,

            /// <summary>
            /// 星期二
            /// </summary>
            Tuesday = 2,

            /// <summary>
            /// 星期三
            /// </summary>
            Wednesday = 3,

            /// <summary>
            /// 星期四
            /// </summary>
            Thursday = 4,

            /// <summary>
            /// 星期五
            /// </summary>
            Friday = 5,

            /// <summary>
            /// 星期六
            /// </summary>
            Saturday = 6
        }

        #endregion

        #region UnitType enum

        /// <summary>
        /// 单位类型
        /// </summary>
        [DataContract]
        public enum UnitType
        {
            /// <summary>
            /// 股
            /// </summary>
            [EnumMember]
            Thigh = 1,

            /// <summary>
            /// 手
            /// </summary>
            [EnumMember]
            Hand = 2,

            /// <summary>
            /// 张
            /// </summary>
            [EnumMember]
            Sheet = 3,

            /// <summary>
            /// 点
            /// </summary>
            [EnumMember]
            Point = 4,
            /// <summary>
            /// 吨
            /// </summary>
            [EnumMember]
            Ton = 5,

            /// <summary>
            /// 克
            /// </summary>
            [EnumMember]
            Gram = 6,
            ///// <summary>
            ///// 指数点
            ///// </summary>
            //[EnumMember] IndexPoints = 7,
            //(说明:2009.03.19与小廖确认结果:点=指数点，保留点)
            /// <summary>
            /// 份
            /// </summary>
            [EnumMember]
            Share = 8
        }

        #endregion

        #region XHSpotHighLowControlType enum

        /// <summary>
        /// 现货品种涨跌幅控制类型
        /// </summary>
        public enum XHSpotHighLowControlType
        {
            /// <summary>
            /// 新股上市，增发上市，暂停后开始交易和其他日期(正常股票,ST股票)
            /// </summary>
            NewThighAddFatStopAfterOrOtherDate = 1,
            /// <summary>
            /// 无涨跌幅限制
            /// </summary>
            NotHighLowControl = 2,
            /// <summary>
            /// 权证涨跌幅
            /// </summary>
            RightPermitHighLow = 3,
            /// <summary>
            /// 新基金上市，增发上市，暂停后开始交易和其他日期
            /// </summary>
            NewFundAddFatStopAfterOrOtherDate = 4
        }

        #endregion

        #region XHValidDeclareType enum

        /// <summary>
        /// 有效申报类型
        /// </summary>
        public enum XHValidDeclareType
        {
            /// <summary>
            /// 最近成交价的上下百分比
            /// </summary>
            BargainPriceUpperDownScale = 1,
            /// <summary>
            /// 不高于即时揭示的最低卖出价格的百分比且不低于即时揭示的最高买入价格的百分比
            /// </summary>
            NotHighSalePriceScaleAndNotLowBuyPriceScale = 2,
            /// <summary>
            /// 低于买一价的个价位与卖一价之间或低于买一价与高于卖一价的个价位之间
            /// </summary>
            DownBuyOneAndSaleOne = 3,
            /// <summary>
            /// 最近成交价上下各多少元
            /// </summary>
            BargainPriceOnDownMoney = 4
        }

        #endregion

        #region 期货交易费用类型
        /// <summary>
        /// 期货交易费用类型
        /// </summary>
        public enum FutrueCostType
        {
            /// <summary>
            /// 按成交量（即交易单位手续费）
            /// </summary>
            TradeUnitCharge = 1,
            /// <summary>
            /// 按成交额（即成交金额手续费）
            /// </summary>
            TurnoverRateOfSerCha = 2

        }
        #endregion

        #endregion

        #region StockNatureEnum enum

        /// <summary>
        /// 股票性质
        /// </summary>
        public enum StockNatureEnum
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1,
            /// <summary>
            /// ST
            /// </summary>
            ST = 2
        }

        #endregion

        #region TransactionDirection enum

        /// <summary>
        /// 交易方向
        /// </summary>
        [DataContract]
        public enum TransactionDirection
        {
            /// <summary>
            /// 买
            /// </summary>
            [EnumMember]
            Buying = 1,
            /// <summary>
            /// 卖
            /// </summary>
            [EnumMember]
            Selling = 2,
        }

        #endregion

        #region [(陈武民)]

        [DataContract]
        public enum MatchCenterState
        {
            /// <summary>
            /// 开始撮合
            /// </summary>
            [EnumMember]
            First = 1,

            /// <summary>
            /// 其他撮合
            /// </summary>
            [EnumMember]
            other = 2,
        }

        #endregion

        #region QHForcedCloseType enum
        /// <summary>
        /// 期货强行平仓类型
        /// Create By:董鹏
        /// Create Date:2010-02-24
        /// Desc.:商品期货强行平仓类型
        /// Update By:李健华
        /// Update Date:2010-02-24
        /// Desc.:修改检举名称，因为股指期货也要有所用到
        /// </summary>
        public enum QHForcedCloseType
        {
            /// <summary>
            /// 过期合约
            /// </summary>
            Expired = 1,
            /// <summary>
            /// 资金不足
            /// </summary>
            CapitalCheck = 2,
            /// <summary>
            /// 超过持仓限制
            /// </summary>
            OverHoldLimit = 3,
            /// <summary>
            /// 不是最小交割单位整数倍
            /// </summary>
            NotModMinUnit = 4
        }
        #endregion

        /// <summary>
        /// Desc: 代码规则类型：1、6位数字，2、5位数字，3、品种代码+2位年份+2位月份，4、品种代码+1位年份+2位月份
        /// Create By: 董鹏
        /// Create Date: 2010-03-09
        /// </summary>
        public enum CodeRulesType
        {
            /// <summary>
            /// 6位数字，如600000（上交所、深交所适用）
            /// </summary>
            SixFigures = 1,
            /// <summary>
            /// 5位数字，如00001（港交所适用）
            /// </summary>
            FiveFigures = 2,
            /// <summary>
            /// 品种代码+2位年份+2位月份，如IF1006（中金所、上期所、大商所适用）
            /// </summary>
            PrefixWithTwoDigitYearTwoDigitMonth = 3,
            /// <summary>
            /// 品种代码+1位年份+2位月份，如WT009（郑商所适用）
            /// </summary>
            PrefixWithOneDigitYearTwoDigitMonth = 4
        }

        /// <summary>
        /// Desc: 上市类型：1、默认类型，2、新股上市，3、增发上市
        /// Create By: 董鹏
        /// Create Date: 2010-03-23
        /// </summary>
        public enum MarketType
        {
            /// <summary>
            /// 默认类型
            /// </summary>
            Default = 1,
            /// <summary>
            /// 新股上市
            /// </summary>
            New = 2,
            /// <summary>
            /// 增发上市
            /// </summary>
            Increase = 3
        }
    }
}