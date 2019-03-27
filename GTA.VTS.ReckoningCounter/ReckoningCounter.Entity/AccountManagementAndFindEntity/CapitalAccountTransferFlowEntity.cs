using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:资金账户转账流水实体
    /// Desc.:资金账户转账流水实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class CapitalATFE
    {
        /// <summary>
        /// 流水标识
        /// </summary>
        private string _FlowLogo = string.Empty;

        /// <summary>
        /// 流水标识
        /// </summary>
        [DataMember]
        public string FlowLogo
        {
            get { return _FlowLogo; }
            set { _FlowLogo = value; }
        }



        /// <summary>
        /// 转出资金账户
        /// </summary>
        private string _FromCapitalAccount = string.Empty;

        /// <summary>
        /// 转出资金账户
        /// </summary>
        [DataMember]
        public string FromCapitalAccount
        {
            get { return _FromCapitalAccount; }
            set { _FromCapitalAccount = value; }
        }


        /// <summary>
        /// 转入资金账户
        /// </summary>
        private string _ToCapitalAccount = string.Empty;

        /// <summary>
        /// 转入资金账户
        /// </summary>
        [DataMember]
        public string ToCapitalAccount
        {
            get { return _ToCapitalAccount; }
            set { _ToCapitalAccount = value; }
        }

        /// <summary>
        /// 资金数量
        /// </summary>
        private string _CapitalAmount = string.Empty;

        /// <summary>
        /// 资金数量
        /// </summary>
        [DataMember]
        public string CapitalAmount
        {
            get { return _CapitalAmount; }
            set { _CapitalAmount = value; }
        }


        /// <summary>
        /// 币种
        /// </summary>
        private int _CurrencyType = 0;

        /// <summary>
        /// 币种
        /// </summary>
        [DataMember]
        public int CurrencyType
        {
            get { return _CurrencyType; }
            set { _CurrencyType = value; }
        }


        /// <summary>
        /// 资金流水类型(包括：几个资金账户之间的同币种自由转账类型、银行账户中的三个币种间的自由兑换类型）
        /// </summary>
        private string _CapitalFlowType = string.Empty;

        /// <summary>
        ///  资金流水类型(包括：几个资金账户之间的同币种自由转账类型、银行账户中的三个币种间的自由兑换类型）
        /// </summary>
        [DataMember]
        public string CapitalFlowType
        {
            get { return _CapitalFlowType; }
            set { _CapitalFlowType = value; }
        }

    }
}