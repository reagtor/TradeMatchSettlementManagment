﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity.HK
{
    /// <summary>
    /// Title:港股成交查询条件实体
    /// Create by:李健华
    /// Create Date:2009-10-19
    ///</summary>
    [DataContract]
    public class HKTradeConditionFindEntity
    {
        /// <summary>
        /// 港股资金帐户
        /// </summary>
        private string _hkCapitalAccount = string.Empty;

        /// <summary>
        /// 港股资金帐户
        /// </summary>
        /// 
        [DataMember]
        public string HKCapitalAccount
        {
            get { return _hkCapitalAccount; }
            set { _hkCapitalAccount = value; }
        }

        /// <summary>
        /// 委托查询起始时间
        /// </summary>
        private DateTime _StartTime = DateTime.Today;
        /// <summary>
        /// 委托查询起始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
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
        /// 委托查询结束时间
        /// </summary>
        private DateTime _EndTime = DateTime.Today;
        /// <summary>
        /// 委托查询结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        /// <summary>
        /// 成交类型（-100为初始值，1买卖成交，2撤单成交，3分红成交，4开仓成交，5平仓成交，6平今成交，7 废单）
        /// </summary>
        private int _TradeType = 0;

        /// <summary>
        /// 成交类型 成交类型（-100为初始值，1买卖成交，2撤单成交，3分红成交，4开仓成交，5平仓成交，6平今成交，7 废单）
        /// </summary>
        [DataMember]
        public int TradeType
        {
            get { return _TradeType; }
            set { _TradeType = value; }
        }


        /// <summary>
        /// 港股代码(证券代码、港股代码）
        /// </summary>
        private string _HKCode = string.Empty;

        /// <summary>
        /// 港股代码(证券代码、港股代码）
        /// </summary>
        [DataMember]
        public string HKCode
        {
            get { return _HKCode; }
            set { _HKCode = value; }
        }



        /// <summary>
        /// 品种类别
        /// </summary>
        private string _VarietyType = string.Empty;
        /// <summary>
        /// 品种类别
        /// </summary>
        [DataMember]
        public string VarietyType
        {
            get { return _VarietyType; }
            set { _VarietyType = value; }
        }

        /// <summary>
        /// 币种（-100为初始值，1为RMB，2为HK，3为US）
        /// </summary>
        private int _CurrencyType = 0;
        /// <summary>
        /// 币种（-100为初始值，1为RMB，2为HK，3为US）
        /// </summary>
        [DataMember]
        public int CurrencyType
        {
            get { return _CurrencyType; }
            set { _CurrencyType = value; }
        }

        /// <summary>
        /// 买卖方向
        /// </summary>
        private int _BuySellDirection = 0;

        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public int BuySellDirection
        {
            get { return _BuySellDirection; }
            set { _BuySellDirection = value; }
        }


        /// <summary>
        /// 委托状态(有9种状态，-100为初始值）
        /// </summary>
        private int _EntrustState = 0;

        /// <summary>
        /// 委托状态(有9种状态，-100为初始值）
        /// </summary>
        [DataMember]
        public int EntrustState
        {
            get { return _EntrustState; }
            set { _EntrustState = value; }
        }

        /// <summary>
        /// 可撤标识(1为可撤，0为不可撤, -100为初始值）
        /// </summary>
        private int _CanBeWithdrawnLogo = 0;

        /// <summary>
        /// 可撤标识(1为可撤，0为不可撤, -100为初始值）
        /// </summary>
        [DataMember]
        public int CanBeWithdrawnLogo
        {
            get { return _CanBeWithdrawnLogo; }
            set { _CanBeWithdrawnLogo = value; }
        }


        /// <summary>
        /// 所属市场(-100为初始值）
        /// </summary>
        private int _BelongToMarket = 0;

        /// <summary>
        /// 所属市场(-100为初始值）
        /// </summary>
        [DataMember]
        public int BelongToMarket
        {
            get { return _BelongToMarket; }
            set { _BelongToMarket = value; }
        }
    }
}
