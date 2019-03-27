using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:现货持仓查询实体
    /// Desc.:现货持仓查询实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class SpotHoldConditionFindEntity
    {
        /// <summary>
        /// 现货股东代码
        /// </summary>
        private string _SpotHoldAccount = string.Empty;

        /// <summary>
        /// 现货股东代码
        /// </summary>
        [DataMember]
        public string SpotHoldAccount
        {
            get { return _SpotHoldAccount; }
            set { _SpotHoldAccount = value; }
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
        /// 持仓总量
        /// </summary>
        private int _HoldTotal;
        /// <summary>
        /// 持仓总量
        /// </summary>
        [DataMember]
        public int HoldTotal
        {
            get { return _HoldTotal; }
            set { _HoldTotal = value; }
        }
        
        /// <summary>
        /// 冻结总量
        /// </summary>
        private int _FreezeTotal;
        /// <summary>
        /// 冻结总量
        /// </summary>
        [DataMember]
        public int FreezeTotal
        {
            get { return _FreezeTotal; }
            set { _FreezeTotal = value; }
        }
        
        /// <summary>
        /// 可用量
        /// </summary>
        private int _CouldAmount;
        /// <summary>
        /// 可用量
        /// </summary>
        [DataMember]
        public int CouldAmount
        {
            get { return _CouldAmount; }
            set { _CouldAmount = value; }
        }
        
        /// <summary>
        /// 持仓均价
        /// </summary>
        private decimal _HoldAveragePrice;
        /// <summary>
        /// 持仓均价
        /// </summary>
        [DataMember]
        public decimal HoldAveragePrice
        {
            get { return _HoldAveragePrice; }
            set { _HoldAveragePrice = value; }
        }
        
        /// <summary>
        /// 当前价格
        /// </summary>
        private decimal _CurrentPrice;
        /// <summary>
        /// 当前价格
        /// </summary>
        [DataMember]
        public decimal CurrentPrice
        {
            get { return _CurrentPrice; }
            set { _CurrentPrice = value; }
        }

       

        /// <summary>
        /// 现货代码(证券代码、港股代码）
        /// </summary>
        private string _SpotCode = string.Empty;

        /// <summary>
        /// 现货代码(证券代码、港股代码）
        /// </summary>
        [DataMember]
        public string SpotCode
        {
            get { return _SpotCode; }
            set { _SpotCode = value; }
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