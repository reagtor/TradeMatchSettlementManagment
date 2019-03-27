using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:资产汇总查询实体
    /// Desc.:资产汇总查询实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class AssetSummaryEntity
    {
        ///// <summary>
        ///// 资金账号
        ///// </summary>
        //private string _capitalAccount;
        ///// <summary>
        ///// 资金账号
        ///// </summary>
        //[DataMember]
        //public string CapitalAccount
        //{
        //    get { return _capitalAccount; }
        //    set { _capitalAccount = value; }
        //}

        /// <summary>
        /// 交易员ID
        /// </summary>
        private string _UserID;
        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        /// <summary>
        /// 人民币可用资金
        /// </summary>
        private decimal _RMBAvailable;
        /// <summary>
        ///人民币可用资金
        /// </summary>
        [DataMember]
        public decimal RMBAvailable
        {
            get { return _RMBAvailable; }
            set { _RMBAvailable = value; }
        }


        /// <summary>
        /// 人民币冻结资金
        /// </summary>
        private decimal _RMBFreeze;
        /// <summary>
        ///人民币冻结资金
        /// </summary>
        [DataMember]
        public decimal RMBFreeze
        {
            get { return _RMBFreeze; }
            set { _RMBFreeze = value; }
        }


        /// <summary>
        ///港币可用资金
        /// </summary>
        private decimal _HKAvailable;
        /// <summary>
        /// 港币可用资金
        /// </summary>
        [DataMember]
        public decimal HKAvailable
        {
            get { return _HKAvailable; }
            set { _HKAvailable = value; }
        }


        /// <summary>
        /// 港币冻结资金
        /// </summary>
        private decimal _HKFreeze;
        /// <summary>
        ///港币冻结资金
        /// </summary>
        [DataMember]
        public decimal HKFreeze
        {
            get { return _HKFreeze; }
            set { _HKFreeze = value; }
        }


        /// <summary>
        ///美元可用资金
        /// </summary>
        private decimal _USAvailable;
        /// <summary>
        /// 美元可用资金
        /// </summary>
        [DataMember]
        public decimal USAvailable
        {
            get { return _USAvailable; }
            set { _USAvailable = value; }
        }

        /// <summary>
        /// 美元冻结资金
        /// </summary>
        private decimal _USFreeze;
        /// <summary>
        ///美元冻结资金
        /// </summary>
        [DataMember]
        public decimal USFreeze
        {
            get { return _USFreeze; }
            set { _USFreeze = value; }
        }  
    }
}