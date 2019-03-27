using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:现货委托实体
    /// Desc.:现货委托实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class SpotEntrustEntity
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
        /// 委托类型
        /// </summary>
        private string _EntrustType = string.Empty;

        /// <summary>
        /// 委托类型
        /// </summary>
        [DataMember]
        public string EntrustType
        {
            get { return _EntrustType; }
            set { _EntrustType = value; }
        }

        /// <summary>
        /// 申报时间（格式为： yyyy-mm-dd hh:mm:ss格式)
        /// </summary>
        private string _DeclareTime = string.Empty;

        /// <summary>
        /// 申报时间（格式为： yyyy-mm-dd hh:mm:ss格式)
        /// </summary>
        [DataMember]
        public string DeclareTime
        {
            get { return _DeclareTime; }
            set { _DeclareTime = value; }
        }


        /// <summary>
        /// 委托状态
        /// </summary>
        private string _EntrustState = string.Empty;

        /// <summary>
        /// 委托状态
        /// </summary>
        [DataMember]
        public string EntrustState
        {
            get { return _EntrustState; }
            set { _EntrustState = value; }
        }

        /// <summary>
        /// 可撤标识
        /// </summary>
        private string _CanBeWithdrawnLogo = string.Empty;

        /// <summary>
        /// 可撤标识
        /// </summary>
        [DataMember]
        public string CanBeWithdrawnLogo
        {
            get { return _CanBeWithdrawnLogo; }
            set { _CanBeWithdrawnLogo = value; }
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
        /// 委托价格
        /// </summary>
        private string _EntrustPrice = string.Empty;

        /// <summary>
        /// 委托价格
        /// </summary>
        [DataMember]
        public string EntrustPrice
        {
            get { return _EntrustPrice; }
            set { _EntrustPrice = value; }
        }

        /// <summary>
        /// 委托数量
        /// </summary>
        private string _EntrustAmount = string.Empty;

        /// <summary>
        /// 委托数量
        /// </summary>
        [DataMember]
        public string EntrustAmount
        {
            get { return _EntrustAmount; }
            set { _EntrustAmount = value; }
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