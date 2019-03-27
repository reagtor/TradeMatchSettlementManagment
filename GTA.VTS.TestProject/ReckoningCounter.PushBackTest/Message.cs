using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.PushBackTest
{
    public class XHMessage
    {
        public string CapitalAccount { get; set; }
        public string Code { get; set; }
        public string BuySell { get; set; }

        /// <summary>
        /// 市价/限价    HK:LO,ELO,SLO
        /// </summary>
        public string EntrustType { get; set; }

        public string EntrustAmount { get; set; }

        public string EntrustPrice { get; set; }

        public string TradeAmount { get; set; }

        public string CancelAmount { get; set; }

        public string OrderMessage { get; set; }

        public string OrderStatus { get; set; }

        public string EntrustNumber { get; set; }

        public string TradeTime { get; set; }
    }

    public class QHMessage:XHMessage
    {
        public string OpenClose { get; set; }
    }

    public class HKMessage:XHMessage
    {
        public string IsModifyOrder { get; set; }

        public string ModifyOrderNumber { get; set; }
    }
    public class HKMarketValue
    {

        #region 货币名称 CurrencyName
        /// <summary>
        /// 货币名称
        /// </summary>
        
        public string CurrencyName { get; set; }
        #endregion

        #region 港股名称 HKName
        /// <summary>
        /// 港股名称
        /// </summary>
        
        public string HKName { get; set; }
        #endregion

        #region 持仓总量 HoldSumAmount
        /// <summary>
        /// 持仓总量
        /// </summary>
        
        public int HoldSumAmount { get; set; }
        #endregion

        #region 当前价 RealtimePrice
        /// <summary>
        /// 当前价
        /// </summary>
        
        public decimal RealtimePrice { get; set; }
        #endregion

        #region 市值 MarketValue
        /// <summary>
        /// 市值
        /// </summary>
        
        public decimal MarketValue { get; set; }
        #endregion

        #region 浮动盈亏 FloatProfitLoss
        /// <summary>
        /// 浮动盈亏
        /// </summary>
        
        public decimal FloatProfitLoss { get; set; }
        #endregion

        #region 错误号 ErroNumber
 
        public string ErroNumber { get; set; }
        #endregion

        #region 错误原因 ErroReason
        /// <summary>
        /// 错误原因
        /// </summary>
        
        public string ErroReason { get; set; }
        #endregion

        #region 交易员ID TraderId
        /// <summary>
        /// 交易员ID
        /// </summary>
        
        public string TraderId { get; set; }
        #endregion

        #region 所属市场 BelongMarket
        /// <summary>
        /// 所属市场
        /// </summary>
        
        public string BelongMarket { get; set; }
        #endregion

        #region 品种类别 VarietyCategories
        /// <summary>
        /// 品种类别
        /// </summary>
        
        public string VarietyCategories { get; set; }
        #endregion        

        #region 代码 Code
        /// <summary>
        /// 品种类别
        /// </summary>

        public string Code { get; set; }
        #endregion        

        /// <summary>
        /// 成本价格
        /// </summary>
        public decimal CostPrice
        {
            set;
            get;
        }
        /// <summary>
        /// 保本价格
        /// </summary>
        public decimal BreakevenPrice
        {
            set;
            get;
        }
        /// <summary>
        /// 持仓均价
        /// </summary>
        public decimal HoldAveragePrice
        {
            set;
            get;
        }
        /// <summary>
        /// 盯市盈亏
        /// </summary>
        public decimal MarketProfitLoss
        {
            set;
            get;
        }
    }
}
