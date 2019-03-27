using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:期货持仓实体
    /// Desc.:期货持仓实体（主要用于ROE查询）
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FuturesHoldEntity
    {
        /// <summary>
        /// 期货交易编码
        /// </summary>
        private string _FuturesTradeCode = string.Empty;

        /// <summary>
        /// 期货交易编码
        /// </summary>
        [DataMember]
        public string FuturesTradeCode
        {
            get { return _FuturesTradeCode; }
            set { _FuturesTradeCode = value; }
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
        /// 总买入持仓量
        /// </summary>
        private string _TotalBuyHoldAmount = string.Empty;
        /// <summary>
        ///  总买入持仓量
        /// </summary>
        [DataMember]
        public string TotalBuyHoldAmount
        {
            get { return _TotalBuyHoldAmount; }
            set { _TotalBuyHoldAmount = value; }
        }



        /// <summary>
        /// 今买入持仓量
        /// </summary>
        private string _TodayBuyHoldAmount = string.Empty;
        /// <summary>
        /// 今买入持仓量
        /// </summary>
        [DataMember]
        public string TodayBuyHoldAmount
        {
            get { return _TodayBuyHoldAmount; }
            set { _TodayBuyHoldAmount = value; }
        }


        /// <summary>
        /// 总卖出持仓量
        /// </summary>
        private string _TotalSellHoldAmount = string.Empty;
        /// <summary>
        /// 总卖出持仓量
        /// </summary>
        [DataMember]
        public string TotalSellHoldAmount
        {
            get { return _TotalSellHoldAmount; }
            set { _TotalSellHoldAmount = value; }
        }


        /// <summary>
        ///  今卖出持仓量
        /// </summary>
        private string _TodaySellHoldAmount = string.Empty;
        /// <summary>
        ///  今卖出持仓量
        /// </summary>
        [DataMember]
        public string TodaySellHoldAmount
        {
            get { return _TodaySellHoldAmount; }
            set { _TodaySellHoldAmount = value; }
        }


        /// <summary>
        /// 买入持仓冻结总量
        /// </summary>
        private string _BuyHoldFreezeTotalAmount = string.Empty;
        /// <summary>
        /// 买入持仓冻结总量
        /// </summary>
        [DataMember]
        public string BuyHoldFreezeTotalAmount
        {
            get { return _BuyHoldFreezeTotalAmount; }
            set { _BuyHoldFreezeTotalAmount = value; }
        }


        /// <summary>
        /// 卖出持仓冻结总量
        /// </summary>
        private string _SellHoldFreezeTotalAmount = string.Empty;
        /// <summary>
        /// 卖出持仓冻结总量
        /// </summary>
        [DataMember]
        public string SellHoldFreezeTotalAmount
        {
            get { return _SellHoldFreezeTotalAmount; }
            set { _SellHoldFreezeTotalAmount = value; }
        }



        /// <summary>
        /// 可用买入持仓量
        /// </summary>
        private string _AvailableBuyHoldAmount = string.Empty;
        /// <summary>
        /// 可用买入持仓量
        /// </summary>
        [DataMember]
        public string AvailableBuyHoldAmount
        {
            get { return _AvailableBuyHoldAmount; }
            set { _AvailableBuyHoldAmount = value; }
        }


        /// <summary>
        /// 可用卖出持仓量
        /// </summary>
        private string _AvailableSellHoldAmount = string.Empty;
        /// <summary>
        /// 可用卖出持仓量
        /// </summary>
        [DataMember]
        public string AvailableSellHoldAmount
        {
            get { return _AvailableSellHoldAmount; }
            set { _AvailableSellHoldAmount = value; }
        }


        /// <summary>
        /// 昨日结算价
        /// </summary>
        private string _YesterdayClearingPrice = string.Empty;
        /// <summary>
        /// 昨日结算价
        /// </summary>
        [DataMember]
        public string YesterdayClearingPrice
        {
            get { return _YesterdayClearingPrice; }
            set { _YesterdayClearingPrice = value; }
        }


        /// <summary>
        /// 买入持仓均价
        /// </summary>
        private string _BuyHoldAveragePrice = string.Empty;
        /// <summary>
        /// 买入持仓均价
        /// </summary>
        [DataMember]
        public string BuyHoldAveragePrice
        {
            get { return _BuyHoldAveragePrice; }
            set { _BuyHoldAveragePrice = value; }
        }


        /// <summary>
        /// 卖出持仓均价
        /// </summary>
        private string _SellHoldAveragePrice = string.Empty;
        /// <summary>
        ///  卖出持仓均价
        /// </summary>
        [DataMember]
        public string SellHoldAveragePrice
        {
            get { return _SellHoldAveragePrice; }
            set { _SellHoldAveragePrice = value; }
        }


        /// <summary>
        /// 所属市场
        /// </summary>
        private string _BelongToMarket = string.Empty;

        /// <summary>
        /// 所属市场
        /// </summary>
        [DataMember]
        public string BelongToMarket
        {
            get { return _BelongToMarket; }
            set { _BelongToMarket = value; }
        }


    }
}