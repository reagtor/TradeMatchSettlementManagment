#region Using Namespace

using System.Runtime.Serialization;

#endregion

namespace ReckoningCounter.Entity.Contants
{
    /// <summary>
    /// Title:相关类型枚举类
    /// Desc.:
    /// 系统相关
    /// 柜台所用枚举及对应表 类及名字空间                                               对应数据库表
    /// 
    /// 1.交易员帐户类型     不需要，动态添加                                           BD_AccountType
    /// 2.交易员帐户类型分类 CommonObject.Types.AccountAttributionType                  DB_AccountTypeClass
    /// 3.委托买卖类型       CommonObject.Types.TransactionDirection                    BD_BuySellType
    /// 4.货币类型           CommonObject.Types.CurrencyType                            BD_CurrencyType
    /// 5.冻结类型       ReckoningCounter.Entity.Contants.Types.FreezeType          BD_FreezeType
    /// 6.期货委托开平仓类型 ReckoningCounter.Entity.Contants.Types.FutureOpenCloseType BD_OpenCloseType
    /// 7 柜台用户类型       ReckoningCounter.Entity.Contants.Types.UserRoleType        BD_UserRoleType
    /// 8.委托单状态         CommonObject.Types.OrderStateType                          DB_OrderStatus
    /// Create By：宋涛
    /// Create date:2008-10-25
    /// </summary>
    public class Types
    {
        #region == 系统相关 ==

        #region DealRptType enum

        /// <summary>
        /// 成交回报类型
        /// Update By:李健华
        /// Update date:2010-01-25
        /// Desc.:添加相应的期货需求添加的成交类型
        /// </summary>
        public enum DealRptType
        {
            /// <summary>
            /// 成交
            /// </summary>
            DRTDealed = 1,

            /// <summary>
            /// 撤单(外部)
            /// </summary>
            DRTCanceled = 2,

            /// <summary>
            /// 分红过户
            /// </summary>
            DRTTransfer = 3,

            /// <summary>
            /// 撤单(内部)
            /// </summary>
            DRTInternalCanceled = 4,

            # region 期货成交类型
            /// <summary>
            /// 废单
            /// </summary>
            DRTDiscard = 5,
            /// <summary>
            /// 保证金不足强行平仓成交
            /// </summary>
            DRTMargin = 6,
            /// <summary>
            /// 超出最后交易日强行平仓
            /// </summary>
            DRTTradeDated = 7,
            /// <summary>
            /// 违反持仓限制强行平仓
            /// </summary>
            DRTViolateLimit = 8
            #endregion
        }

        #endregion

        #region FreezeType enum

        /// <summary>
        /// 5.冻结类型
        /// </summary>
        public enum FreezeType : int
        {
            /// <summary>
            /// 委托冻结
            /// </summary>
            DelegateFreeze = 1,

            /// <summary>
            /// 清算冻结
            /// </summary>
            ReckoningFreeze = 2,

            /// <summary>
            /// 今日开仓平仓冻结
            /// </summary>
            TodayHoldingCloseFreeze = 3,

            /// <summary>
            /// 历史开仓平仓冻结
            /// </summary>
            HistoryHoldingCloseFreeze = 4,
        }

        #endregion

        #region FutureOpenCloseType enum

        /// <summary>
        /// 期货开平仓类型
        /// </summary>
        [DataContract]
        public enum FutureOpenCloseType
        {
            /// <summary>
            /// 开仓
            /// </summary>
            /// 
            [EnumMember]
            OpenPosition = 1,

            /// <summary>
            /// 平仓（历史）
            /// </summary>
            [EnumMember]
            ClosePosition = 2,

            /// <summary>
            /// 平今
            /// </summary>
            [EnumMember]
            CloseTodayPosition = 3
        }

        #endregion

        #region MeloncutType enum

        /// <summary>
        /// 现货分红类型
        /// </summary>
        public enum MeloncutType
        {
            /// <summary>
            /// 现金分红
            /// </summary>
            Cash = 1,

            /// <summary>
            /// 股票分红
            /// </summary>
            Stock = 2
        }

        #endregion

        #region OrderStateType enum

        /// <summary>
        /// 委托单状态枚举 
        /// </summary>
        /// 
        [DataContract]
        public enum OrderStateType
        {
            /// <summary>
            /// 无状态
            /// </summary>
            [EnumMember]
            None = 1,
            /// <summary>
            /// 未报
            /// </summary>
            [EnumMember]
            DOSUnRequired = 2,
            /// <summary>
            /// 待报
            /// </summary>
            [EnumMember]
            DOSRequiredSoon = 3,
            /// <summary>
            /// 已报待撤
            /// </summary>
            [EnumMember]
            DOSRequiredRemoveSoon = 4,
            /// <summary>
            /// 已报
            /// </summary>
            [EnumMember]
            DOSIsRequired = 5,
            /// <summary>
            /// 废单
            /// </summary>
            [EnumMember]
            DOSCanceled = 6,
            /// <summary>
            /// 已撤
            /// </summary>
            [EnumMember]
            DOSRemoved = 7,
            /// <summary>
            /// 部撤
            /// </summary>    
            [EnumMember]
            DOSPartRemoved = 8,

            /// <summary>
            /// 部成
            /// </summary>
            [EnumMember]
            DOSPartDealed = 9,

            /// <summary>
            /// 已成
            /// </summary>
            [EnumMember]
            DOSDealed = 10,

            /// <summary>
            /// 部成待撤
            /// </summary>
            [EnumMember]
            DOSPartDealRemoveSoon = 11,
        }

        #endregion

        #region UserRoleType enum

        /// <summary>
        /// 7.交易员帐户类型
        /// </summary>
        [DataContract]
        public enum UserRoleType
        {
            /// <summary>
            /// 管理员
            /// </summary>
            [EnumMember]
            Administrator = 1,

            /// <summary>
            /// 交易员
            /// </summary>
            [EnumMember]
            Trader = 2
        }

        #endregion

        /// <summary>
        /// 根据委托单类型int值转换成枚举类型
        /// </summary>
        /// <param name="type">要转换的int类型</param>
        /// <returns>委托单状态枚举类型</returns>
        public static OrderStateType GetOrderStateType(int type)
        {
            OrderStateType state = OrderStateType.None;

            switch (type)
            {
                case 1:
                    state = OrderStateType.None;
                    break;
                case 2:
                    state = OrderStateType.DOSUnRequired;
                    break;
                case 3:
                    state = OrderStateType.DOSRequiredSoon;
                    break;
                case 4:
                    state = OrderStateType.DOSRequiredRemoveSoon;
                    break;
                case 5:
                    state = OrderStateType.DOSIsRequired;
                    break;
                case 6:
                    state = OrderStateType.DOSCanceled;
                    break;
                case 7:
                    state = OrderStateType.DOSRemoved;
                    break;
                case 8:
                    state = OrderStateType.DOSPartRemoved;
                    break;
                case 9:
                    state = OrderStateType.DOSPartDealed;
                    break;
                case 10:
                    state = OrderStateType.DOSDealed;
                    break;
                case 11:
                    state = OrderStateType.DOSPartDealRemoveSoon;
                    break;
            }

            return state;
        }

        /// <summary>
        /// 根据委托单类型int值转换成中文描述
        /// </summary>
        /// <param name="type">要转换的int类型</param>
        /// <returns>委托单状态中文描述</returns>
        public static string GetOrderStateMsg(int type)
        {
            //OrderStateType state = OrderStateType.None;
            string msg = "无状态";

            switch (type)
            {
                case 1:
                    //state = OrderStateType.None;
                    msg = "无状态";
                    break;
                case 2:
                    //state = OrderStateType.DOSUnRequired;
                    msg = "未报";
                    break;
                case 3:
                    //state = OrderStateType.DOSRequiredSoon;
                    msg = "待报";
                    break;
                case 4:
                    //state = OrderStateType.DOSRequiredRemoveSoon;
                    msg = "已报待撤";
                    break;
                case 5:
                    //state = OrderStateType.DOSIsRequired;
                    msg = "已报";
                    break;
                case 6:
                    //state = OrderStateType.DOSCanceled;
                    msg = "废单";
                    break;
                case 7:
                    //state = OrderStateType.DOSRemoved;
                    msg = "已撤";
                    break;
                case 8:
                    //state = OrderStateType.DOSPartRemoved;
                    msg = "部撤";
                    break;
                case 9:
                    //state = OrderStateType.DOSPartDealed;
                    msg = "部成";
                    break;
                case 10:
                    //state = OrderStateType.DOSDealed;
                    msg = "已成";
                    break;
                case 11:
                    //state = OrderStateType.DOSPartDealRemoveSoon;
                    msg = "部成待撤";
                    break;
            }

            return msg;
        }

        /// <summary>
        /// 根据期货委托单开平平仓类型转换成中文描述
        /// </summary>
        /// <param name="type">期货开平仓枚举类型</param>
        /// <returns>期货开平仓状态中文描述</returns>
        public static string GetFutureOpenCloseType(FutureOpenCloseType type)
        {
            string result = "";
            switch (type)
            {
                case FutureOpenCloseType.OpenPosition:
                    result = "开仓";
                    break;
                case FutureOpenCloseType.ClosePosition:
                    result = "平仓(历史)";
                    break;
                case FutureOpenCloseType.CloseTodayPosition:
                    result = "平仓";
                    break;
            }

            return result;
        }

        #endregion

        #region == 转帐相关 ==

        /// <summary>
        /// 转账类型
        /// </summary>
        [DataContract]
        public enum TransferType
        {
            /// <summary>
            /// 自由转账
            /// </summary>
            [EnumMember]
            FreeTransfer = 1,
            /// <summary>
            /// 分红转账
            /// </summary>
            [EnumMember]
            DividendTransfer = 2,
            /// <summary>
            /// 追加资金
            /// </summary>
            [EnumMember]
            AddCapital = 3
        }

        #endregion

        #region == 委托接收相关 ==

        /// <summary>
        /// 委托价格类型
        /// </summary>
        [DataContract]
        public enum OrderPriceType
        {
            ///// <summary>
            ///// 指定价格
            ///// </summary>
            //[EnumMember] OPTLimited,
            ///// <summary>
            ///// 市价
            ///// </summary>
            //[EnumMember] OPTMarketPrice

            /// <summary>
            /// 市价
            /// </summary>
            [EnumMember]
            OPTMarketPrice,
            /// <summary>
            /// 指定价格
            /// </summary>
            [EnumMember]
            OPTLimited
        }

        ///// <summary>
        ///// 港股委托价格类型
        ///// </summary>
        //[DataContract]
        //public enum HKOrderPriceType
        //{
        //    /// <summary>
        //    /// 限价盘
        //    /// </summary>
        //    [EnumMember]
        //    LO,

        //    /// <summary>
        //    /// 增强限价盘
        //    /// </summary>
        //    [EnumMember]
        //    ELO,

        //    /// <summary>
        //    /// 特别限价盘
        //    /// </summary>
        //    [EnumMember]
        //    SLO
        //}

        #endregion

        #region == 调度相关 ==

        /// <summary>
        /// 交易时间类型
        /// </summary>
        public enum TradingTimeType
        {
            /// <summary>
            /// 开始时间（开市-开始撮合）
            /// </summary>
            Open,

            /// <summary>
            /// 结束时间（收市-结束撮合）
            /// </summary>
            Close,

            /// <summary>
            /// 撮合中心开始接受委托
            /// </summary>
            MacthBeginWork,

            /// <summary>
            /// 撮合中心结束接受委托
            /// </summary>
            MatchEndWork,

            /// <summary>
            /// 柜台开始接受委托
            /// </summary>
            CounterBeginWork,

            /// <summary>
            /// 柜台结束接受委托
            /// </summary>
            CounterEndWork,
        }

        #endregion
    }
}