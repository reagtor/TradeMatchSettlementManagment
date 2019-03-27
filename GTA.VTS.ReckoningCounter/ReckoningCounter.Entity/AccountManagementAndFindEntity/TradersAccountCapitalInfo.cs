using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:交易账户资金信息
    /// Desc.:交易账户资金信息实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class TradersAccountCapitalInfo
    {
        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderID
        {
            get;
            set;
        }
        /// <summary>
        /// 账号ID
        /// </summary>
        [DataMember]
        public string AccountID
        {
            get;
            set;
        }
        /// <summary>
        /// 帐号类型
        /// </summary>
        [DataMember]
        public int AccountType
        {
            get;
            set;
        }

        /// <summary>
        /// 资金币种类型
        /// </summary>
        [DataMember]
        public int CurrencyType
        {
            get;
            set;
        }
        /// <summary>
        /// 初始化资金
        /// </summary>
        [DataMember]
        public double InitCapital
        {
            get;
            set;
        }
        /// <summary>
        /// 冻结资金
        /// </summary>
        [DataMember]
        public decimal FreezeCapital
        {
            get;
            set;
        }
        /// <summary>
        /// 可用资金
        /// </summary>
        [DataMember]
        public decimal AvailableCapital
        {
            get;
            set;
        }

    }
}
