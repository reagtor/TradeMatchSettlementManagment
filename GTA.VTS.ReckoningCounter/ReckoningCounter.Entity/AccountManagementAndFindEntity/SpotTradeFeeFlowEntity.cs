using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:现货交易费用流水实体
    /// Desc.:现货交易费用流水实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class SpotTradeFeeFlowEntity
    {
        /// <summary>
        /// 成交单号
        /// </summary>
        private string _TradeNumber = string.Empty;
        /// <summary>
        /// 成交单号
        /// </summary>
        [DataMember]
        public string TradeNumber
        {
            get { return _TradeNumber; }
            set { _TradeNumber = value; }
        }


        /// <summary>
        /// 现货代码
        /// </summary>
        private string _SpotCode = string.Empty;
        /// <summary>
        /// 现货代码
        /// </summary>
        [DataMember]
        public string SpotCode
        {
            get { return _SpotCode; }
            set { _SpotCode = value; }
        }


        /// <summary>
        /// 现货名称
        /// </summary>
        private string _SpotName = string.Empty;
        /// <summary>
        /// 现货名称
        /// </summary>
        [DataMember]
        public string SpotName
        {
            get { return _SpotName; }
            set { _SpotName = value; }
        }


        /// <summary>
        /// 成交价格
        /// </summary>
        private decimal _TradePrice;
        /// <summary>
        /// 成交价格
        /// </summary>
        [DataMember]
        public decimal TradePrice
        {
            get { return _TradePrice; }
            set { _TradePrice = value; }
        }

        


        /// <summary>
        /// 交易费用
        /// </summary>
        private decimal _TradeFee;
        /// <summary>
        /// 交易费用
        /// </summary>
        [DataMember]
        public decimal TradeFee
        {
            get { return _TradeFee; }
            set { _TradeFee = value; }
        }
        


        /// <summary>
        /// 成交时间
        /// </summary>
        private DateTime _TradeTime;
        /// <summary>
        /// 成交时间
        /// </summary>
        [DataMember]
        public DateTime TradeTime
        {
            get { return _TradeTime; }
            set { _TradeTime = value; }
        }
        


        /// <summary>
        /// 币种
        /// </summary>
        private int _CurrencyType;
        /// <summary>
        /// 币种
        /// </summary>
        [DataMember]
        public int CurrencyType
        {
            get { return _CurrencyType; }
            set { _CurrencyType = value; }
        }
        
    }
}