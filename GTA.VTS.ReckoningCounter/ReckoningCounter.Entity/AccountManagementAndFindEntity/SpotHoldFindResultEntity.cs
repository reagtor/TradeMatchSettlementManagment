using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ReckoningCounter.Model;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
  /// <summary>
    /// Title:现货持仓查询结果实体
    /// Desc.:现货持仓查询结果实体
    /// Create by:李健华
    /// Create date:2009-11-2
  /// </summary>
  [DataContract]
  public  class SpotHoldFindResultEntity
  {
        /// <summary>
        /// 现货持仓帐户表对象
        /// </summary>
      private XH_AccountHoldTableInfo _holdFindResult = null;

        /// <summary>
        /// 现货持仓帐户表对象
        /// </summary>
        [DataMember]
      public XH_AccountHoldTableInfo HoldFindResult
        {
            get { return _holdFindResult; }
            set { _holdFindResult = value; }
        }

        /// <summary>
        /// 货币名称
        /// </summary>
        [DataMember]
        public string CurrencyName { get; set; }

        /// <summary>
        /// 现货名称
        /// </summary>
        [DataMember]
        public string SpotName { get; set; }

        /// <summary>
        /// 持仓总量
        /// </summary>
        [DataMember]
        public int HoldSumAmount { get; set; }

        /// <summary>
        /// 当前价
        /// </summary>
        [DataMember]
        public decimal RealtimePrice { get; set; }

        /// <summary>
        /// 市值
        /// </summary>
        [DataMember]
        public decimal MarketValue { get; set; }

        /// <summary>
        /// 浮动盈亏
        /// </summary>
        [DataMember]
        public decimal FloatProfitLoss { get; set; }

        /// <summary>
        /// 错误号
        /// </summary>
        [DataMember]
        public string ErroNumber { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        [DataMember]
        public string ErroReason { get; set; }

        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderId { get; set; }

        /// <summary>
        /// 所属市场
        /// </summary>
        [DataMember]
        public string BelongMarket { get; set; }

        /// <summary>
        /// 品种类别
        /// </summary>
        [DataMember]
        public string VarietyCategories { get; set; }
    }
}
