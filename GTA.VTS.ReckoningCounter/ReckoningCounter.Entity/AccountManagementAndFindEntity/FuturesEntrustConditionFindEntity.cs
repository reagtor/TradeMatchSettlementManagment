using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:期货委托条件查询实体
    /// Desc.:期货委托条件查询实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FuturesEntrustConditionFindEntity
    {
        /// <summary>
        /// 期货资金帐户
        /// </summary>
        private string _CapitalAccount = string.Empty;

        /// <summary>
        /// 期货资金帐户
        /// </summary>
        /// 
        [DataMember]
        public string CapitalAccount
        {
            get { return _CapitalAccount; }
            set { _CapitalAccount = value; }
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
        /// 开平方向
        /// </summary>
        private int _OpenCloseDirection =0;

        /// <summary>
        /// 开平方向
        /// </summary>
        [DataMember]
        public int OpenCloseDirection
        {
            get { return _OpenCloseDirection; }
            set { _OpenCloseDirection = value; }
        }

        /// <summary>
        /// 买卖方向
        /// </summary>
        private int _BuySellDirection =0;

        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public int BuySellDirection
        {
            get { return _BuySellDirection; }
            set { _BuySellDirection = value; }
        }

        ///// <summary>
        ///// 委托类型
        ///// </summary>
        //private string _EntrustType = string.Empty;

        ///// <summary>
        ///// 委托类型
        ///// </summary>
        //[DataMember]
        //public string EntrustType
        //{
        //    get { return _EntrustType; }
        //    set { _EntrustType = value; }
        //}

       

        /// <summary>
        /// 委托状态（-100为初始值，1无状态，2未报，3待报，4已报待撤，5已报，6废单，7已撤，8部撤，9部成，10已成）
        /// </summary>
        private int _EntrustState = 0;

        /// <summary>
        /// 委托状态（-100为初始值，1无状态，2未报，3待报，4已报待撤，5已报，6废单，7已撤，8部撤，9部成，10已成）
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
        /// 币种（-100为初始值，1为RMB，2为HK，3为US）
        /// </summary>
        private int _CurrencyTypeId = 0;
        /// <summary>
        /// 币种（-100为初始值，1为RMB，2为HK，3为US）
        /// </summary>
        [DataMember]
        public int CurrencyTypeId
        {
            get { return _CurrencyTypeId; }
            set { _CurrencyTypeId = value; }
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
        /// 品种类别
        /// </summary>
        private int _VarietyType = 0;
        /// <summary>
        /// 品种类别
        /// </summary>
        [DataMember]
        public int VarietyType
        {
            get { return _VarietyType; }
            set { _VarietyType = value; }
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