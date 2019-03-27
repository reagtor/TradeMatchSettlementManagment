using System;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model.QH
{
    /// <summary>
    /// 柜台回推期货委托单实体类
    /// 作者：李健华
    /// 日期：2009-08-05
    /// Update BY:李健华
    /// Update Date:2009-08-04
    /// Desc.:根据前台需求增加卖买类型，开平仓类型字段属性。
    /// Update BY:李健华
    /// Update Date:2009-08-15
    /// Desc.:添加委托单状态信息属性
    /// Update BY:李健华
    /// Update Date:2009-08-18
    /// Desc.:添加交易货币类型ID,资金账号ID
    /// </summary>
    [DataContract]
    public class FuturePushOrderEntity
    {
        #region EntrustNumber 委托单号(主键)
        private string entrustNumber;
        /// <summary>
        /// 委托单号(主键)
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

        #region ContractCode 委托合约(商品)ID(编码)
        private string contractCode;
        /// <summary>
        /// 委托合约(商品)ID(编码)
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

        #region CloseFloatProfitLoss 平仓盈亏（浮）
        private decimal closeFloatProfitLoss;
        /// <summary>
        /// 平仓盈亏（浮）
        /// </summary>
        [DataMember]
        public decimal CloseFloatProfitLoss
        {
            get
            {
                return closeFloatProfitLoss;
            }
            set
            {
                closeFloatProfitLoss = value;
            }
        }
        #endregion

        #region CloseMarketProfitLoss 平仓盈亏（盯）
        private decimal closeMarketProfitLoss;
        /// <summary>
        /// 平仓盈亏（盯）
        /// </summary>
        [DataMember]
        public decimal CloseMarketProfitLoss
        {
            get
            {
                return closeMarketProfitLoss;
            }
            set
            {
                closeMarketProfitLoss = value;
            }
        }
        #endregion

        #region EntrustTime 委托时间
        private DateTime entrustTime;
        /// <summary>
        /// 委托时间
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

        #region CapitalAccount 期货委托资金帐户(即股指期货资金帐号/商品期货资金帐号)(外键UA_UserAccountAllocationTable)
        private string capitalAccount;
        /// <summary>
        /// 期货委托资金帐户(即股指期货资金帐号/商品期货资金帐号)(外键UA_UserAccountAllocationTable)
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
