using System;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model.XH
{
    /// <summary>
    /// 柜台回推现货成交实体类
    /// Update BY:李健华
    /// Update Date:2009-08-04
    /// Desc.:修改注释
    /// Update By:李健华
    /// Update Date:2009-09-01
    /// Desc.:根据前台需求，增加回推数据TradeTypeId 成交类型ID
    /// </summary>
    [DataContract]
    public class StockPushDealEntity
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
    }
}
