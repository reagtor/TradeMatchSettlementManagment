using System;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model.QH
{
    /// <summary>
    /// 柜台回推期货成交实体类
    /// Update BY:李健华
    /// Update Date:2009-08-15
    /// Desc.:修改注释
    /// Update By:李健华
    /// Update Date:2009-09-01
    /// Desc.:根据前台需求，增加回推数据TradeTypeId 成交类型ID
    /// </summary>
    [DataContract]
    public class FuturePushDealEntity
    {

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
