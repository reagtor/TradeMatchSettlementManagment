using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:追加资金实体
    /// Desc.:追加资金实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class AddCapitalEntity
    {
        /// <summary>
        /// 交易员ID
        /// </summary>
        private string _TraderID = string.Empty;
        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderID
        {
            get { return _TraderID; }
            set { _TraderID = value; }
        }

        /// <summary>
        /// 银行资金账号
        /// </summary>
        private string _BankCapitalAccount = string.Empty;
        /// <summary>
        /// 银行资金账号
        /// </summary>
        [DataMember]
        public string BankCapitalAccount
        {
            get { return _BankCapitalAccount; }
            set { _BankCapitalAccount = value; }
        }



        /// <summary>
        /// 追加人民币的数额
        /// </summary>
        private Decimal _AddRMBAmount = -100;
        /// <summary>
        /// 追加人民币的数额
        /// </summary>
        [DataMember]
        public Decimal AddRMBAmount
        {
            get { return _AddRMBAmount; }
            set { _AddRMBAmount = value; }
        }


        /// <summary>
        ///  追加港币的数额
        /// </summary>
        private Decimal _AddHKAmount = -100;
        /// <summary>
        /// 追加港币的数额
        /// </summary>
        [DataMember]
        public Decimal AddHKAmount
        {
            get { return _AddHKAmount; }
            set { _AddHKAmount = value; }
        }


        /// <summary>
        /// 追加美元的数额
        /// </summary>
        private Decimal _AddUSAmount = -100;
        /// <summary>
        /// 追加美元的数额
        /// </summary>
        [DataMember]
        public Decimal AddUSAmount
        {
            get { return _AddUSAmount; }
            set { _AddUSAmount = value; }
        }
    }
}