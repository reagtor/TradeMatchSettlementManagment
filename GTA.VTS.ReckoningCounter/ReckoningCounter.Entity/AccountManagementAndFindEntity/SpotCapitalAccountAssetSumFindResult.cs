using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:现货资金帐户下的某币种资产汇总查询结果
    /// Desc.:现货资金帐户下的某币种资产汇总查询结果
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public  class SpotCapitalAccountAssetSumFindResult
    {
        /// <summary>
        /// 资金账号
        /// </summary>
        [DataMember]
        public string CapitalAccount { get;  set; }

        /// <summary>
        /// 市值
        /// </summary>
        [DataMember]
        public decimal MarketValue { get;  set; }

        /// <summary>
        /// 资产总数
        /// </summary>
        [DataMember]
        public decimal AssetAmount { get;  set; }

        /// <summary>
        /// 币种名称
        /// </summary>
        [DataMember]
        public string CurrencyName { get; set; }
    }
}
