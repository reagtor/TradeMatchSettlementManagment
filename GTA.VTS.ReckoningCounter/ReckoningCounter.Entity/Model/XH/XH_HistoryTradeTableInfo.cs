using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 现货历史成交实体类
    /// Desc: 现货历史成交实体类XH_HistoryTradeTableInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class XH_HistoryTradeTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_HistoryTradeTableInfo()
        { }

        #region TradeNumber 成交单号主键
        private string tradeNumber;
        /// <summary>
        /// 成交单号主键
        /// </summary>
        [DataMember]
        public string TradeNumber
        {
            get
            {
                return tradeNumber;
            }
            set
            {
                tradeNumber = value;
            }
        }
        #endregion
        #region EntrustNumber 委托单号
        private string entrustNumber;
        /// <summary>
        /// 委托单号
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
        #region TradePrice 成交价格
        private decimal tradePrice;
        /// <summary>
        /// 成交价格
        /// </summary>
        [DataMember]
        public decimal TradePrice
        {
            get
            {
                return tradePrice;
            }
            set
            {
                tradePrice = value;
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
        #region StampTax 印花税
        private decimal stampTax;
        /// <summary>
        /// 印花税
        /// </summary>
        [DataMember]
        public decimal StampTax
        {
            get
            {
                return stampTax;
            }
            set
            {
                stampTax = value;
            }
        }
        #endregion
        #region Commission 佣金
        private decimal commission;
        /// <summary>
        /// 佣金
        /// </summary>
        [DataMember]
        public decimal Commission
        {
            get
            {
                return commission;
            }
            set
            {
                commission = value;
            }
        }
        #endregion
        #region SpotCode 成交商品编号(这与管理中心的CM_Commodity对应)
        private string spotCode;
        /// <summary>
        /// 成交商品编号(这与管理中心的CM_Commodity对应)
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
        #region TransferAccountFee 过户费
        private decimal transferAccountFee;
        /// <summary>
        /// 过户费
        /// </summary>
        [DataMember]
        public decimal TransferAccountFee
        {
            get
            {
                return transferAccountFee;
            }
            set
            {
                transferAccountFee = value;
            }
        }
        #endregion
        #region TradeProceduresFee 交易手续费
        private decimal tradeProceduresFee;
        /// <summary>
        /// 交易手续费
        /// </summary>
        [DataMember]
        public decimal TradeProceduresFee
        {
            get
            {
                return tradeProceduresFee;
            }
            set
            {
                tradeProceduresFee = value;
            }
        }
        #endregion
        #region MonitoringFee 监管费
        private decimal monitoringFee;
        /// <summary>
        /// 监管费
        /// </summary>
        [DataMember]
        public decimal MonitoringFee
        {
            get
            {
                return monitoringFee;
            }
            set
            {
                monitoringFee = value;
            }
        }
        #endregion
        #region TradingSystemUseFee 交易系统使用费
        private decimal tradingSystemUseFee;
        /// <summary>
        /// 交易系统使用费
        /// </summary>
        [DataMember]
        public decimal TradingSystemUseFee
        {
            get
            {
                return tradingSystemUseFee;
            }
            set
            {
                tradingSystemUseFee = value;
            }
        }
        #endregion
        #region ClearingFee 结算费
        private decimal clearingFee;
        /// <summary>
        /// 结算费
        /// </summary>
        [DataMember]
        public decimal ClearingFee
        {
            get
            {
                return clearingFee;
            }
            set
            {
                clearingFee = value;
            }
        }
        #endregion
        #region StockAccount 用于成交的交易账户(即现货持仓帐户--即或证券股东代码/港股股东代码))(外键UA_UserAccountAllocationTable)
        private string stockAccount;
        /// <summary>
        /// 用于成交的交易账户(即现货持仓帐户--即或证券股东代码/港股股东代码))(外键UA_UserAccountAllocationTable)
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
        #region CapitalAccount 现货成交资金帐户(即证券资金帐户/港股资金帐户)
        private string capitalAccount;
        /// <summary>
        /// 现货成交资金帐户(即证券资金帐户/港股资金帐户)
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
        #region TradeTypeId 成交类型ID(外键BD_TradeType)
        private int tradeTypeId;
        /// <summary>
        /// 成交类型ID(外键BD_TradeType)
        /// </summary>
        [DataMember]
        public int TradeTypeId
        {
            get
            {
                return tradeTypeId;
            }
            set
            {
                tradeTypeId = value;
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
        #region TradeTime 成交时间
        private DateTime tradeTime;
        /// <summary>
        /// 成交时间
        /// </summary>
        [DataMember]
        public DateTime TradeTime
        {
            get
            {
                return tradeTime;
            }
            set
            {
                tradeTime = value;
            }
        }
        #endregion

    }
}

