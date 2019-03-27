using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// Tilte; 现货消息实体
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    public class XHMessage
    {
        /// <summary>
        /// 资金账户
        /// </summary>
        public string CapitalAccount { get; set; }

        /// <summary>
        /// 商品代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 买卖方向
        /// </summary>
        public string BuySell { get; set; }

        /// <summary>
        /// 市价/限价    HK:LO,ELO,SLO
        /// </summary>
        public string EntrustType { get; set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        public string EntrustAmount { get; set; }

        /// <summary>
        /// 委托价格
        /// </summary>
        public string EntrustPrice { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public string TradeAmount { get; set; }

        /// <summary>
        /// 退单数量
        /// </summary>
        public string CancelAmount { get; set; }

        /// <summary>
        /// 消息文本
        /// </summary>
        public string OrderMessage { get; set; }

        /// <summary>
        /// 委托状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 委托编号
        /// </summary>
        public string EntrustNumber { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TradeTime { get; set; }
    }

    /// <summary>
    /// Tilte; 期货消息实体
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    public class QHMessage:XHMessage
    {
        /// <summary>
        /// 开平方向
        /// </summary>
        public string OpenClose { get; set; }
    }

    /// <summary>
    /// Tilte; 港股消息实体
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    public class HKMessage:XHMessage
    {
        /// <summary>
        /// 是否改单
        /// </summary>
        public string IsModifyOrder { get; set; }

        /// <summary>
        /// 改单数量
        /// </summary>
        public string ModifyOrderNumber { get; set; }
    }

    /// <summary>
    /// Tilte; 港股查询数据汇总实体
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
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

        #region 成本价格 CostPrice
        /// <summary>
        /// 成本价格
        /// </summary>
        public decimal CostPrice
        {
            set;
            get;
        }
        #endregion

        #region 保本价格 BreakevenPrice
        /// <summary>
        /// 保本价格
        /// </summary>
        public decimal BreakevenPrice
        {
            set;
            get;
        }
        #endregion

        #region 持仓均价 HoldAveragePrice
        /// <summary>
        /// 持仓均价
        /// </summary>
        public decimal HoldAveragePrice
        {
            set;
            get;
        }
        #endregion

        #region 盯市盈亏 MarketProfitLoss
        /// <summary>
        /// 盯市盈亏
        /// </summary>
        public decimal MarketProfitLoss
        {
            set;
            get;
        }
        #endregion
    }
}
