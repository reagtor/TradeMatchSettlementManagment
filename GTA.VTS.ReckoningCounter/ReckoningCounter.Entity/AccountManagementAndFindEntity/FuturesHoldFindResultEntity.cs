using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ReckoningCounter.Model;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:期货持仓查询结果实体
    /// Desc.:期货持仓查询结果实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FuturesHoldFindResultEntity
    {

        /// <summary>
        /// 期货持仓帐户表对象
        /// </summary>
        private QH_HoldAccountTableInfo _holdFindResult = null;

        /// <summary>
        /// 期货持仓帐户表对象
        /// </summary>
        [DataMember]
        public QH_HoldAccountTableInfo HoldFindResult
        {
            get { return _holdFindResult; }
            set { _holdFindResult = value; }
        }

        /// <summary>
        /// 货币名称
        /// </summary>
        [DataMember]
        public string CurrencyName { get; set; }

        /// <summary>
        /// 合约名称
        /// </summary>
        [DataMember]
        public string ContractName { get; set; }


        /// <summary>
        /// 持仓总量
        /// </summary>
        [DataMember]
        public int HoldSumAmount { get; set; }


        /// <summary>
        /// 今开仓量
        /// </summary>
        [DataMember]
        public int TodayAOpenAmount { get; set; }

        ///// <summary>
        ///// 买入持仓总量
        ///// </summary>
        //[DataMember]
        //public int BuyHoldSumAmount { get; set; }

        ///// <summary>
        ///// 卖出持仓总量
        ///// </summary>
        //[DataMember]
        //public int SellHoldSumAmount { get; set; }

        /// <summary>
        /// 持仓均价
        /// </summary>
        [DataMember]
        public decimal HoldAveragePrice { get; set; }

        /// <summary>
        /// 今日开仓均价
        /// </summary>
        [DataMember]
        public decimal TodayOpenAveragePrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        [DataMember]
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 当前价
        /// </summary>
        [DataMember]
        public decimal RealtimePrice { get; set; }

        /// <summary>
        /// 保本价
        /// </summary>
        [DataMember]
        public decimal BreakevenPrice { get; set; }

        /// <summary>
        /// 昨日结算价
        /// </summary>
        [DataMember]
        public decimal YesterdayClearingPrice { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        [DataMember]
        public decimal MarginAmount { get; set; }

        ///// <summary>
        ///// 市值
        ///// </summary>
        //[DataMember]
        //public decimal MarketValue { get; set; }

        /// <summary>
        /// 浮动盈亏
        /// </summary>
        [DataMember]
        public decimal FloatProfitLoss { get; set; }

        /// <summary>
        /// 盯市盈亏
        /// </summary>
        [DataMember]
        public decimal MarketProfitLoss { get; set; }

        
        /// <summary>
        /// 错误号
        /// </summary>
        [DataMember]
        public string ErroNumber { get; set; }

        /// <summary>
        /// 冻结总量
        /// </summary>
        [DataMember]
        public int FreezeTotalAmount { get; set; }

        /// <summary>
        /// 可用总量
        /// </summary>
        [DataMember]
        public int AvailableTotalAmount { get; set; }
        
        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public string BuySellDirection { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        [DataMember]
        public string ErroReason { get; set; }

        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderId { get; set; }

        /// <summary>
        /// 所属市场
        /// </summary>
        [DataMember]
        public string BelongMarket { get; set; }

        /// <summary>
        /// 品种类别
        /// </summary>
        [DataMember]
        public string VarietyCategories { get; set; }

        /// <summary>
        /// Title: 交易单位计价单位倍数 
        /// Create by: 董鹏 
        /// Create date: 2010-05-04
        /// </summary>
        [DataMember]
        public decimal? UnitMultiple { set; get; }
    }
}
