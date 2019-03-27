using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:银行资金实体
    /// Desc.:银行资金实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class BankCapitalEntity
    {
        /// <summary>
        /// 人民币资金余额
        /// </summary>
        private string _RMBCapitalBalance = string.Empty;
        /// <summary>
        /// 人民币资金余额
        /// </summary>
        [DataMember]
        public string RMBCapitalBalance
        {
            get { return _RMBCapitalBalance; }
            set { _RMBCapitalBalance = value; }
        }


        /// <summary>
        ///  港币资金余额
        /// </summary>
        private string _HKCapitalBalance = string.Empty;
        /// <summary>
        /// 港币资金余额
        /// </summary>
        [DataMember]
        public string HKCapitalBalance
        {
            get { return _HKCapitalBalance; }
            set { _HKCapitalBalance = value; }
        }

        /// <summary>
        /// 美元资金余额
        /// </summary>
        private string _USCapitalBalance = string.Empty;
        /// <summary>
        /// 美元资金余额
        /// </summary>
        [DataMember]
        public string USCapitalBalance
        {
            get { return _USCapitalBalance; }
            set { _USCapitalBalance = value; }
        }
    }
}