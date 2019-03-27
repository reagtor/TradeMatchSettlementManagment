using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity.HK
{
    /// <summary>
    /// Title:港股持仓查询条件实体
    /// Create by:李健华
    /// Create Date:2009-10-19
    ///</summary>
    [DataContract]
    public class HKHoldConditionFindEntity
    {
        #region 港股持仓股东代码 HKHoldAccount
        /// <summary>
        /// 港股股东代码
        /// </summary>
        [DataMember]
        public string HKHoldAccount { get; set; }
        #endregion

        #region 品种代码 VarietyCode
        /// <summary>
        /// 品种代码
        /// </summary>
        [DataMember]
        public string VarietyCode { get; set; }
        #endregion

        #region 品种名称 VarietyName
        /// <summary>
        /// 品种名称
        /// </summary>
        [DataMember]
        public string VarietyName { get; set; }
        #endregion

        #region 持仓总量 HoldTotal
        /// <summary>
        /// 持仓总量
        /// </summary>
        [DataMember]
        public int HoldTotal { get; set; }
        #endregion

        #region 冻结总量 FreezeTotal
        /// <summary>
        /// 冻结总量
        /// </summary>
        [DataMember]
        public int FreezeTotal { get; set; }
        #endregion

        #region 可用量 CouldAmount
        /// <summary>
        /// 可用量
        /// </summary>
        [DataMember]
        public int CouldAmount { get; set; }
        #endregion

        #region 持仓均价 HoldAveragePrice
        /// <summary>
        /// 持仓均价
        /// </summary>
        [DataMember]
        public decimal HoldAveragePrice { get; set; }
        #endregion

        #region 当前价格 CurrentPrice
        /// <summary>
        /// 当前价格
        /// </summary>
        [DataMember]
        public decimal CurrentPrice { get; set; }
        #endregion

        #region 港股代码 HKCode
        /// <summary>
        /// 港股代码
        /// </summary>
        [DataMember]
        public string HKCode { get; set; }

        #endregion

        #region 品种类别 VarietyType
        /// <summary>
        /// 品种类别
        /// </summary>
        [DataMember]
        public string VarietyType { get; set; }

        #endregion

        #region 币种 CurrencyType
        /// <summary>
        /// 币种（0为初始值，1为RMB，2为HK，3为US）
        /// </summary>
        [DataMember]
        public int CurrencyType { get; set; }
        #endregion

        #region 所属市场(0为初始值） BelongToMarket
        /// <summary>
        /// 所属市场(0为初始值）
        /// </summary>
        [DataMember]
        public int BelongToMarket { get; set; }
        #endregion
    }
}
