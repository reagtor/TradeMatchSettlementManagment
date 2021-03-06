﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:期货成交实体
    /// Desc.:期货成交实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FuturesTradeEntity
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
        /// 委托单号
        /// </summary>
        private string _EntrustNumber = string.Empty;

        /// <summary>
        /// 委托单号
        /// </summary>
        [DataMember]
        public string EntrustNumber
        {
            get { return _EntrustNumber; }
            set { _EntrustNumber = value; }
        }


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
        /// 开平方向
        /// </summary>
        private string _OpenCloseDirection = string.Empty;

        /// <summary>
        /// 开平方向
        /// </summary>
        [DataMember]
        public string OpenCloseDirection
        {
            get { return _OpenCloseDirection; }
            set { _OpenCloseDirection = value; }
        }

        /// <summary>
        /// 买卖方向
        /// </summary>
        private string _BuySellDirection = string.Empty;

        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public string BuySellDirection
        {
            get { return _BuySellDirection; }
            set { _BuySellDirection = value; }
        }

        /// <summary>
        /// 成交类型
        /// </summary>
        private string _TradeType = string.Empty;

        /// <summary>
        /// 成交类型
        /// </summary>
        [DataMember]
        public string TradeType
        {
            get { return _TradeType; }
            set { _TradeType = value; }
        }



        /// <summary>
        /// 成交时间（格式为： yyyy-mm-dd hh:mm:ss格式)
        /// </summary>
        private string _TradeTime = string.Empty;

        /// <summary>
        /// 成交时间（格式为： yyyy-mm-dd hh:mm:ss格式)
        /// </summary>
        [DataMember]
        public string TradeTime
        {
            get { return _TradeTime; }
            set { _TradeTime = value; }
        }


        /// <summary>
        /// 成交数量
        /// </summary>
        private string _TradeAmount = string.Empty;

        /// <summary>
        /// 成交数量
        /// </summary>
        [DataMember]
        public string TradeAmount
        {
            get { return _TradeAmount; }
            set { _TradeAmount = value; }
        }

        /// <summary>
        /// 成交均价
        /// </summary>
        private string _TradeAveragePrice = string.Empty;

        /// <summary>
        /// 成交均价
        /// </summary>
        [DataMember]
        public string TradeAveragePrice
        {
            get { return _TradeAveragePrice; }
            set { _TradeAveragePrice = value; }
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

        /// <summary>
        /// 机构号
        /// </summary>
        private string _AgenciesNumber = string.Empty;

        /// <summary>
        /// 机构号
        /// </summary>
        [DataMember]
        public string AgenciesNumber
        {
            get { return _AgenciesNumber; }
            set { _AgenciesNumber = value; }
        }


        /// <summary>
        /// 投组标识
        /// </summary>
        private string _PortfolioLogo = string.Empty;

        /// <summary>
        /// 投组标识
        /// </summary>
        [DataMember]
        public string PortfolioLogo
        {
            get { return _PortfolioLogo; }
            set { _PortfolioLogo = value; }
        }
    }
}