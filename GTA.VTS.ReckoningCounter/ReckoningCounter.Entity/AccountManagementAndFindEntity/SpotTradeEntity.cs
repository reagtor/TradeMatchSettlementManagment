using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:现货成交实体
    /// Desc.:现货成交实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class SpotTradeEntity
    {

        /// <summary>
        /// 现货股东代码
        /// </summary>
        private string _SpotShareholdersCode = string.Empty;

        /// <summary>
        /// 现货股东代码
        /// </summary>
        [DataMember]
        public string SpotShareholdersCode
        {
            get { return _SpotShareholdersCode; }
            set { _SpotShareholdersCode = value; }
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
        /// 品种代码
        /// </summary>
        private string _VarietyCode = string.Empty;

        /// <summary>
        /// 品种代码
        /// </summary>
        [DataMember]
        public string VarietyCode
        {
            get { return _VarietyCode; }
            set { _VarietyCode = value; }
        }

        /// <summary>
        /// 品种名称
        /// </summary>
        private string _VarietyName = string.Empty;

        /// <summary>
        /// 品种名称
        /// </summary>
        [DataMember]
        public string VarietyName
        {
            get { return _VarietyName; }
            set { _VarietyName = value; }
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
        /// 成交查询起始时间
        /// </summary>
        private DateTime _StartTime = DateTime.Today;
        /// <summary>
        /// 成交查询起始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }


        /// <summary>
        /// 成交查询结束时间
        /// </summary>
        private DateTime _EndTime = DateTime.Today;
        /// <summary>
        /// 成交查询结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }



        /// <summary>
        /// 成交数量
        /// </summary>
        private int _TradeAmount;
        /// <summary>
        /// 成交数量
        /// </summary>
        [DataMember]
        public int TradeAmount
        {
            get { return _TradeAmount; }
            set { _TradeAmount = value; }
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