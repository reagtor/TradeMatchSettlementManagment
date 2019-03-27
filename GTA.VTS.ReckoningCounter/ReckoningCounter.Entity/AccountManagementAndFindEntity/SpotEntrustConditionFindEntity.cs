using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity
{
    /// <summary>
    /// Title:现货委托条件查询实体
    /// Desc.:现货委托条件查询实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class SpotEntrustConditionFindEntity
    {
        /// <summary>
        /// 现货资金帐户
        /// </summary>
        private string _SpotCapitalAccount = string.Empty;

        /// <summary>
        /// 现货资金帐户
        /// </summary>
        /// 
        [DataMember]
        public string SpotCapitalAccount
        {
            get { return _SpotCapitalAccount; }
            set { _SpotCapitalAccount = value; }
        }

        /// <summary>
        /// 现货代码(证券代码、港股代码）
        /// </summary>
        private string _SpotCode = string.Empty;

        /// <summary>
        /// 现货代码(证券代码、港股代码）
        /// </summary>
        /// 
        [DataMember]
        public string SpotCode
        {
            get { return _SpotCode; }
            set { _SpotCode = value; }
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
        private int _BuySellDirection=0;

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
        private int _BelongToMarket =0;

        /// <summary>
        /// 所属市场(-100为初始值）
        /// </summary>
        [DataMember]
        public int BelongToMarket
        {
            get { return _BelongToMarket; }
            set { _BelongToMarket = value; }
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
    }
}
