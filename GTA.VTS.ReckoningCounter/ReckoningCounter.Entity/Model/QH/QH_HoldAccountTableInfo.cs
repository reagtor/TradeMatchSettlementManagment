using System;
using System.Runtime.Serialization;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title: 期货持仓实体类
    /// Desc: QH_HoldAccountTable期货持仓账户的所拥有的持仓信息明细实体类 。(属性说明自动提取数据库字段的描述信息)
    /// Create By: 李健华
    /// Create Date: 2009-10-15
    /// </summary>
    [DataContract]
    public class QH_HoldAccountTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QH_HoldAccountTableInfo()
        { }
        #region AccountHoldLogoId 持仓ID主键
        private int accountHoldLogoId;
        /// <summary>
        /// 持仓ID主键
        /// </summary>
        [DataMember]
        public int AccountHoldLogoId
        {
            get
            {
                return accountHoldLogoId;
            }
            set
            {
                accountHoldLogoId = value;
            }
        }
        #endregion

        #region UserAccountDistributeLogo 持仓账号(外键UA_UserAccountAllocationTable)
        private string userAccountDistributeLogo;
        /// <summary>
        /// 持仓账号(外键UA_UserAccountAllocationTable)
        /// </summary>
        [DataMember]
        public string UserAccountDistributeLogo
        {
            get
            {
                return userAccountDistributeLogo;
            }
            set
            {
                userAccountDistributeLogo = value;
            }
        }
        #endregion

        #region TradeCurrencyType 持仓交易货币类型(外键BD_CurrencyType)
        private int tradeCurrencyType;
        /// <summary>
        /// 持仓交易货币类型(外键BD_CurrencyType)
        /// </summary>
        [DataMember]
        public int TradeCurrencyType
        {
            get
            {
                return tradeCurrencyType;
            }
            set
            {
                tradeCurrencyType = value;
            }
        }
        #endregion

        #region BuySellTypeId 买卖类型(外键BD_BuySellType)
        private int buySellTypeId;
        /// <summary>
        /// 买卖类型(外键BD_BuySellType)
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

        #region HistoryHoldAmount 历史持仓总量即历史可用量
        private decimal historyHoldAmount;
        /// <summary>
        /// 历史可用持仓总量
        /// </summary>
        [DataMember]
        public decimal HistoryHoldAmount
        {
            get
            {
                return historyHoldAmount;
            }
            set
            {
                historyHoldAmount = value;
            }
        }
        #endregion

        #region HistoryFreezeAmount 历史冻结总量
        private decimal historyFreezeAmount;
        /// <summary>
        /// 历史冻结总量
        /// </summary>
        [DataMember]
        public decimal HistoryFreezeAmount
        {
            get
            {
                return historyFreezeAmount;
            }
            set
            {
                historyFreezeAmount = value;
            }
        }
        #endregion

        #region HoldAveragePrice 持仓平均价格
        private decimal holdAveragePrice;
        /// <summary>
        /// 持仓平均价格
        /// </summary>
        [DataMember]
        public decimal HoldAveragePrice
        {
            get
            {
                return holdAveragePrice;
            }
            set
            {
                holdAveragePrice = value;
            }
        }
        #endregion

        #region TodayHoldAmount 今天持仓总量
        private decimal todayHoldAmount;
        /// <summary>
        /// 今天可用持仓总量
        /// </summary>
        [DataMember]
        public decimal TodayHoldAmount
        {
            get
            {
                return todayHoldAmount;
            }
            set
            {
                todayHoldAmount = value;
            }
        }
        #endregion

        #region TodayHoldAveragePrice 今天持仓平均价格
        private decimal todayHoldAveragePrice;
        /// <summary>
        /// 今天持仓平均价格
        /// </summary>
        [DataMember]
        public decimal TodayHoldAveragePrice
        {
            get
            {
                return todayHoldAveragePrice;
            }
            set
            {
                todayHoldAveragePrice = value;
            }
        }
        #endregion

        #region TodayFreezeAmount 今天冻结总量
        private decimal todayFreezeAmount;
        /// <summary>
        /// 今天冻结总量
        /// </summary>
        [DataMember]
        public decimal TodayFreezeAmount
        {
            get
            {
                return todayFreezeAmount;
            }
            set
            {
                todayFreezeAmount = value;
            }
        }
        #endregion

        #region Contract 持仓合约(商品)编码(这与管理中心CM_Commodity的ID对应)
        private string contract;
        /// <summary>
        /// 持仓合约(商品)编码(这与管理中心CM_Commodity的ID对应)
        /// </summary>
        [DataMember]
        public string Contract
        {
            get
            {
                return contract;
            }
            set
            {
                contract = value;
            }
        }
        #endregion

        #region CostPrice 成本价格
        private decimal costPrice;
        /// <summary>
        /// 成本价格
        /// </summary>
        [DataMember]
        public decimal CostPrice
        {
            get
            {
                return costPrice;
            }
            set
            {
                costPrice = value;
            }
        }
        #endregion

        #region BreakevenPrice 保本价格
        private decimal breakevenPrice;
        /// <summary>
        /// 保本价格
        /// </summary>
        [DataMember]
        public decimal BreakevenPrice
        {
            get
            {
                return breakevenPrice;
            }
            set
            {
                breakevenPrice = value;
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

        #region ProfitLoss 最后利润
        private decimal profitLoss;
        /// <summary>
        /// 最后利润
        /// </summary>
        [DataMember]
        public decimal ProfitLoss
        {
            get
            {
                return profitLoss;
            }
            set
            {
                profitLoss = value;
            }
        }
        #endregion

        #region OpenAveragePrice 开仓均价
        private decimal openAveragePrice;
        /// <summary>
        /// 开仓均价
        /// </summary>
        [DataMember]
        public decimal OpenAveragePrice
        {
            get
            {
                return openAveragePrice;
            }
            set
            {
                openAveragePrice = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 期货持仓变化表
    /// </summary>
    public class QH_HoldAccountTableInfo_Delta
    {
        /// <summary>
        /// 期货持仓实体
        /// </summary>
        public QH_HoldAccountTableInfo Data;

        /// <summary>
        /// 持仓账户Id
        /// </summary>
        public int AccountHoldLogoId;

        /// <summary>
        /// 历史持仓变化量
        /// </summary>
        public decimal HistoryHoldAmountDelta;

        /// <summary>
        /// 历史冻结变化量
        /// </summary>
        public decimal HistoryFreezeAmountDelta;

        /// <summary>
        /// 当日持仓变化量
        /// </summary>
        public decimal TodayHoldAmountDelta;

        /// <summary>
        /// 当日冻结变化量
        /// </summary>
        public decimal TodayFreezeAmountDelta;

        /// <summary>
        /// 保证金变化量
        /// </summary>
        public decimal MarginDelta;
    }
}

