using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 期货历史成交实体类
    /// Desc: 期货历史成交实体类QH_HistoryTradeTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class QH_HistoryTradeTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QH_HistoryTradeTableInfo()
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
        #region Margin 保证金
        private decimal margin;
        /// <summary>
        /// 保证金
        /// </summary>
        [DataMember]
        public decimal Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
            }
        }
        #endregion
        #region ContractCode 成交合约(商品)编号(这与管理中心的CM_Commodity对应)
        private string contractCode;
        /// <summary>
        /// 成交合约(商品)编号(这与管理中心的CM_Commodity对应)
        /// </summary>
        [DataMember]
        public string ContractCode
        {
            get
            {
                return contractCode;
            }
            set
            {
                contractCode = value;
            }
        }
        #endregion
        #region TradeAccount 用于成交的交易账户(即期货持仓帐户--即或证券股东代码/商品期货交易编码))(外键UA_UserAccountAllocationTable)
        private string tradeAccount;
        /// <summary>
        /// 用于成交的交易账户(即期货持仓帐户--即或证券股东代码/商品期货交易编码))(外键UA_UserAccountAllocationTable)
        /// </summary>
        [DataMember]
        public string TradeAccount
        {
            get
            {
                return tradeAccount;
            }
            set
            {
                tradeAccount = value;
            }
        }
        #endregion
        #region CapitalAccount 期货成交资金帐户(即股指期货资金帐号/商品期货资金帐号)
        private string capitalAccount;
        /// <summary>
        /// 期货成交资金帐户(即股指期货资金帐号/商品期货资金帐号)
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
        #region OpenCloseTypeId 开平仓类型ID(外键BD_OpenCloseType)
        private int openCloseTypeId;
        /// <summary>
        /// 开平仓类型ID(外键BD_OpenCloseType)
        /// </summary>
        [DataMember]
        public int OpenCloseTypeId
        {
            get
            {
                return openCloseTypeId;
            }
            set
            {
                openCloseTypeId = value;
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
        #region MarketProfitLoss 盯市盈亏
        private decimal marketProfitLoss;
        /// <summary>
        /// 盯市盈亏
        /// </summary>
        [DataMember]
        public decimal MarketProfitLoss
        {
            get
            {
                return marketProfitLoss;
            }
            set
            {
                marketProfitLoss = value;
            }
        }
        #endregion
    }
}

