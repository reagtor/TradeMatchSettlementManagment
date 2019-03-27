using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title: 账户实体
    /// Desc.:账户实体此实体主要用于开户使用
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class AccountEntity
    {
        /// <summary>
        /// 交易员ID
        /// </summary>
        private string _traderID = string.Empty;
        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderID
        {
            get { return _traderID; }
            set { _traderID = value; }
        }




        /// <summary>
        /// 交易员密码
        /// </summary>
        private string _traderPassWord = string.Empty;
        /// <summary>
        /// 交易员密码
        /// </summary>
        [DataMember]
        public string TraderPassWord
        {
            get { return _traderPassWord; }
            set { _traderPassWord = value; }
        }



        /// <summary>
        /// 用户角色编号
        /// </summary>
        private int _roleNumber;
        /// <summary>
        /// 用户角色编号
        /// </summary>
        [DataMember]
        public int RoleNumber
        {
            get { return _roleNumber; }
            set { _roleNumber = value; }
        }


        /// <summary>
        ///  账号
        /// </summary>
        private string _account = string.Empty;
        /// <summary>
        /// 账号
        /// </summary>
        [DataMember]
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }




        /// <summary>
        /// 账号类型
        /// </summary>
        private int _accountType;
        /// <summary>
        /// 账号类型
        /// </summary>
        [DataMember]
        public int AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }



        /// <summary>
        /// 账号所属类型（现货资金，现货持仓，期货资金，期货持仓）
        /// </summary>
        private int _accountAAttribution;
        /// <summary>
        /// 账号所属类型（现货资金，现货持仓，期货资金，期货持仓）
        /// </summary>
        [DataMember]
        public int AccountAAttribution
        {
            get { return _accountAAttribution; }
            set { _accountAAttribution = value; }
        }



        /// <summary>
        /// 人民币
        /// </summary>
        private decimal _currencyRMB;
        /// <summary>
        /// 人民币
        /// </summary>
        [DataMember]
        public decimal CurrencyRMB
        {
            get { return _currencyRMB; }
            set { _currencyRMB = value; }
        }



        /// <summary>
        ///港币
        /// </summary>
        private decimal _currencyHK;
        /// <summary>
        /// 港币
        /// </summary>
        [DataMember]
        public decimal CurrencyHK
        {
            get { return _currencyHK; }
            set { _currencyHK = value; }
        }



        /// <summary>
        ///美元
        /// </summary>
        private decimal _currencyUS;
        /// <summary>
        /// 美元
        /// </summary>
        [DataMember]
        public decimal CurrencyUS
        {
            get { return _currencyUS; }
            set { _currencyUS = value; }
        }
    }
}