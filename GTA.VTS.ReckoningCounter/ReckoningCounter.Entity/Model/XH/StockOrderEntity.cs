using System;
using System.Runtime.Serialization;

namespace ReckoningCounter.Model
{
    /// <summary>
    /// 柜台回推现货委托实体类
    /// Update BY:李健华
    /// Update Date:2009-08-15
    /// Desc.:添加委托单信息字段
    /// Update BY:李健华
    /// Update Date:2009-08-18
    /// Desc.:添加交易货币类型ID,资金账户ID
    /// </summary>
    [DataContract]
    public class StockPushOrderEntity
    {

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
        [DataMember]
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


       
    }
}
