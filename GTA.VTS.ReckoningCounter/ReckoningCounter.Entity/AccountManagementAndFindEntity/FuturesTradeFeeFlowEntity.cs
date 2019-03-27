using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:期货交易费用流水实体
    /// Desc.:期货交易费用流水实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FuturesTradeFeeFlowEntity
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
        /// 合约代码
        /// </summary>
        private string _ContractCode = string.Empty;
        /// <summary>
        /// 合约代码
        /// </summary>
        [DataMember]
        public string ContractCode
        {
            get { return _ContractCode; }
            set { _ContractCode = value; }
        }
        


        /// <summary>
        /// 合约名称
        /// </summary>
        private string _ContractName = string.Empty;
        /// <summary>
        /// 合约名称
        /// </summary>
        [DataMember]
        public string ContractName
        {
            get { return _ContractName; }
            set { _ContractName = value; }
        }

        

        /// <summary>
        /// 成交价格
        /// </summary>
        private string _TradePrice = string.Empty;
        /// <summary>
        /// 成交价格
        /// </summary>
        [DataMember]
        public string TradePrice
        {
            get { return _TradePrice; }
            set { _TradePrice = value; }
        }


        /// <summary>
        /// 交易费用
        /// </summary>
        private string _TradeFee = string.Empty;
        /// <summary>
        /// 交易费用
        /// </summary>
        [DataMember]
        public string TradeFee
        {
            get { return _TradeFee; }
            set { _TradeFee = value; }
        }


        /// <summary>
        /// 成交时间
        /// </summary>
        private string _TradeTime = string.Empty;
        /// <summary>
        ///  成交时间
        /// </summary>
        [DataMember]
        public string TradeTime
        {
            get { return _TradeTime; }
            set { _TradeTime = value; }
        }


        /// <summary>
        /// 币种
        /// </summary>
        private string _CurrencyType = string.Empty;
        /// <summary>
        /// 币种
        /// </summary>
        [DataMember]
        public string CurrencyType
        {
            get { return _CurrencyType; }
            set { _CurrencyType = value; }
        }
    }
}