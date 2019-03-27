using System;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货当日委托实体类
    /// Desc: 现货当日委托实体类XH_TodayEntrustTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class XH_TodayEntrustTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_TodayEntrustTableInfo()
        { }

        #region EntrustNumber 现货委托单号(主键)
        private string entrustNumber;
        /// <summary>
        /// 现货委托单号(主键)
        /// </summary>
        [DataMember]
        public string EntrustNumber
        {
            get
            {
                return entrustNumber;
            }
            set
            {
                entrustNumber = value;
            }
        }
        #endregion

        #region PortfolioLogo 投组标识
        private string portfolioLogo;
        /// <summary>
        /// 投组标识
        /// </summary>
        [DataMember]
        public string PortfolioLogo
        {
            get
            {
                return portfolioLogo;
            }
            set
            {
                portfolioLogo = value;
            }
        }
        #endregion

        #region EntrustPrice 委托价格
        private decimal entrustPrice;
        /// <summary>
        /// 委托价格
        /// </summary>
        [DataMember]
        public decimal EntrustPrice
        {
            get
            {
                return entrustPrice;
            }
            set
            {
                entrustPrice = value;
            }
        }
        #endregion

        #region EntrustAmount 委托数量
        private int entrustAmount;
        /// <summary>
        /// 委托数量
        /// </summary>
        [DataMember]
        public int EntrustAmount
        {
            get
            {
                return entrustAmount;
            }
            set
            {
                entrustAmount = value;
            }
        }
        #endregion

        #region SpotCode 委托商品ID(编码)
        private string spotCode;
        /// <summary>
        /// 委托商品ID(编码)
        /// </summary>
        [DataMember]
        public string SpotCode
        {
            get
            {
                return spotCode;
            }
            set
            {
                spotCode = value;
            }
        }
        #endregion

        #region TradeAmount 成交总量
        private int tradeAmount;
        /// <summary>
        /// 成交总量
        /// </summary>
        [DataMember]
        public int TradeAmount
        {
            get
            {
                return tradeAmount;
            }
            set
            {
                tradeAmount = value;
            }
        }
        #endregion

        #region TradeAveragePrice 委托成交平均价格
        private decimal tradeAveragePrice;
        /// <summary>
        /// 委托成交平均价格
        /// </summary>
        [DataMember]
        public decimal TradeAveragePrice
        {
            get
            {
                return tradeAveragePrice;
            }
            set
            {
                tradeAveragePrice = value;
            }
        }
        #endregion

        #region CancelAmount 撤单总量
        private int cancelAmount;
        /// <summary>
        /// 撤单总量
        /// </summary>
        [DataMember]
        public int CancelAmount
        {
            get
            {
                return cancelAmount;
            }
            set
            {
                cancelAmount = value;
            }
        }
        #endregion

        #region CancelLogo 可撤标识(1为可撤，0为不可撤, -100为初始值)
        private bool cancelLogo;
        /// <summary>
        /// 可撤标识(1为可撤，0为不可撤, -100为初始值)
        /// </summary>
        [DataMember]
        public bool CancelLogo
        {
            get
            {
                return cancelLogo;
            }
            set
            {
                cancelLogo = value;
            }
        }
        #endregion

        #region BuySellTypeId 卖买类型(外键BD_BuySellType)
        private int buySellTypeId;
        /// <summary>
        /// 卖买类型(外键BD_BuySellType)
        /// </summary>
        [DataMember]
        public int BuySellTypeId
        {
            get
            {
                return buySellTypeId;
            }
            set
            {
                buySellTypeId = value;
            }
        }
        #endregion

        #region StockAccount 用于委托的交易账户(即现货持仓帐户--即或证券股东代码/港股股东代码))(外键UA_UserAccountAllocationTable)
        private string stockAccount;
        /// <summary>
        /// 用于委托的交易账户(即现货持仓帐户--即或证券股东代码/港股股东代码))(外键UA_UserAccountAllocationTable)
        /// </summary>
        [DataMember]
        public string StockAccount
        {
            get
            {
                return stockAccount;
            }
            set
            {
                stockAccount = value;
            }
        }
        #endregion

        #region CapitalAccount 委托现货资金帐户(即证券资金帐户/港股资金帐户)(外键UA_UserAccountAllocationTable)
        private string capitalAccount;
        /// <summary>
        /// 委托现货资金帐户(即证券资金帐户/港股资金帐户)(外键UA_UserAccountAllocationTable)
        /// </summary>
        [DataMember]
        public string CapitalAccount
        {
            get
            {
                return capitalAccount;
            }
            set
            {
                capitalAccount = value;
            }
        }
        #endregion

        #region OrderStatusId 委托单状态(外键DB_OrderStatus)
        private int orderStatusId;
        /// <summary>
        /// 委托单状态(外键DB_OrderStatus)
        /// </summary>
        [DataMember]
        public int OrderStatusId
        {
            get
            {
                return orderStatusId;
            }
            set
            {
                orderStatusId = value;
            }
        }
        #endregion

        #region IsMarketValue 是否是市价
        private bool isMarketValue;
        /// <summary>
        /// 是否是市价
        /// </summary>
        [DataMember]
        public bool IsMarketValue
        {
            get
            {
                return isMarketValue;
            }
            set
            {
                isMarketValue = value;
            }
        }
        #endregion

        #region OrderMessage 委托单信息
        private string orderMessage;
        /// <summary>
        /// 委托单信息
        /// </summary>
        [DataMember]
        public string OrderMessage
        {
            get
            {
                return orderMessage;
            }
            set
            {
                orderMessage = value;
            }
        }
        #endregion

        #region CurrencyTypeId 委托单交易货币类型ID(外键BD_CurrencyType)
        private int currencyTypeId;
        /// <summary>
        /// 委托单交易货币类型ID(外键BD_CurrencyType)
        /// </summary>
        [DataMember]
        public int CurrencyTypeId
        {
            get
            {
                return currencyTypeId;
            }
            set
            {
                currencyTypeId = value;
            }
        }
        #endregion

        #region TradeUnitId 委托单交易单位ID(外键BD_TradeUnit)
        private int tradeUnitId;
        /// <summary>
        /// 委托单交易单位ID(外键BD_TradeUnit)
        /// </summary>
        [DataMember]
        public int TradeUnitId
        {
            get
            {
                return tradeUnitId;
            }
            set
            {
                tradeUnitId = value;
            }
        }
        #endregion

        #region CallbackChannlId 回调信息注册通道
        private string callbackChannlId;
        /// <summary>
        /// 回调信息注册通道
        /// </summary>
        [DataMember]
        public string CallbackChannlId
        {
            get
            {
                return callbackChannlId;
            }
            set
            {
                callbackChannlId = value;
            }
        }
        #endregion

        #region McOrderId 委托单的MC机器随机编码单号
        private string mcOrderId;
        /// <summary>
        /// 委托单的MC机器随机编码单号
        /// </summary>
        [DataMember]
        public string McOrderId
        {
            get
            {
                return mcOrderId;
            }
            set
            {
                mcOrderId = value;
            }
        }
        #endregion

        #region HasDoneProfit 已实现盈亏
        private decimal hasDoneProfit;
        /// <summary>
        /// 已实现盈亏
        /// </summary>
        [DataMember]
        public decimal HasDoneProfit
        {
            get
            {
                return hasDoneProfit;
            }
            set
            {
                hasDoneProfit = value;
            }
        }
        #endregion

        #region OfferTime 委托报盘时间
        private DateTime? offerTime;
        /// <summary>
        /// 委托报盘时间
        /// </summary>
        public DateTime? OfferTime
        {
            get
            {
                return offerTime;
            }
            set
            {
                offerTime = value;
            }
        }
        #endregion

        #region EntrustTime 现货委托时间
        private DateTime entrustTime;
        /// <summary>
        /// 现货委托时间
        /// </summary>
        [DataMember]
        public DateTime EntrustTime
        {
            get
            {
                return entrustTime;
            }
            set
            {
                entrustTime = value;
            }
        }
        #endregion

        /// <summary>
        /// 转换显示的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("现货今日委托实体[");
            sb.Append("EntrustNumber={0},");
            sb.Append("PortfolioLogo={1},");
            sb.Append("EntrustPrice={2},");
            sb.Append("EntrustAmount={3},");
            sb.Append("SpotCode={4},");
            sb.Append("TradeAmount={5},");
            sb.Append("TradeAveragePrice={6},");
            sb.Append("CancelAmount={7},");
            sb.Append("CancelLogo={8},");
            sb.Append("BuySellTypeId={9},");
            sb.Append("StockAccount={10},");
            sb.Append("CapitalAccount={11},");
            sb.Append("OrderStatusId={12},");
            sb.Append("IsMarketValue={13},");
            sb.Append("OrderMessage={14},");
            sb.Append("CurrencyTypeId={15},");
            sb.Append("TradeUnitId={16},");
            sb.Append("CallbackChannlId={17},");
            sb.Append("McOrderId={18},");
            sb.Append("HasDoneProfit={19},");
            sb.Append("OfferTime={20},");
            sb.Append("EntrustTime={20}");
            sb.Append("]");

            string format = sb.ToString();

            string desc = string.Format(format, EntrustNumber, PortfolioLogo, EntrustPrice, EntrustAmount, SpotCode,
                                        TradeAmount, TradeAveragePrice, CancelAmount, CancelLogo, BuySellTypeId,
                                        StockAccount, CapitalAccount,
                                        OrderStatusId, IsMarketValue, OrderMessage, CurrencyTypeId, TradeUnitId,
                                        CallbackChannlId, McOrderId,
                                        HasDoneProfit, OfferTime, EntrustTime);
            return desc;
        }
    }
}

