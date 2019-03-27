using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity.HK
{
    /// <summary>
    /// Title:港股持仓查询汇总返回实体
    /// Create By:李健华
    /// Create Date:2009-10-19
    /// </summary>
    [DataContract]
    public class HKHoldFindResultyEntity
    {
        #region 港股持仓表对象 _holdFindResult
        /// <summary>
        /// 港股持仓表对象
        /// </summary>
        private HK_AccountHoldInfo _holdFindResult = null;

        /// <summary>
        /// 港股持仓帐户表对象
        /// </summary>
        [DataMember]
        public HK_AccountHoldInfo HoldFindResult
        {
            get { return _holdFindResult; }
            set { _holdFindResult = value; }
        }
        #endregion

        #region 货币名称 CurrencyName
        /// <summary>
        /// 货币名称
        /// </summary>
        [DataMember]
        public string CurrencyName { get; set; }
        #endregion

        #region 港股名称 HKName
        /// <summary>
        /// 港股名称
        /// </summary>
        [DataMember]
        public string HKName { get; set; }
        #endregion

        #region 持仓总量 HoldSumAmount
        /// <summary>
        /// 持仓总量
        /// </summary>
        [DataMember]
        public int HoldSumAmount { get; set; }
        #endregion

        #region 当前价 RealtimePrice
        /// <summary>
        /// 当前价
        /// </summary>
        [DataMember]
        public decimal RealtimePrice { get; set; }
        #endregion

        #region 市值 MarketValue
        /// <summary>
        /// 市值
        /// </summary>
        [DataMember]
        public decimal MarketValue { get; set; }
        #endregion

        #region 浮动盈亏 FloatProfitLoss
        /// <summary>
        /// 浮动盈亏
        /// </summary>
        [DataMember]
        public decimal FloatProfitLoss { get; set; }
        #endregion

        #region 错误号 ErroNumber
        /// <summary>
        /// 错误号
        /// </summary>
        [DataMember]
        public string ErroNumber { get; set; }
        #endregion

        #region 错误原因 ErroReason
        /// <summary>
        /// 错误原因
        /// </summary>
        [DataMember]
        public string ErroReason { get; set; }
        #endregion

        #region 交易员ID TraderId
        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderId { get; set; }
        #endregion

        #region 所属市场 BelongMarket
        /// <summary>
        /// 所属市场
        /// </summary>
        [DataMember]
        public string BelongMarket { get; set; }
        #endregion

        #region 品种类别 VarietyCategories
        /// <summary>
        /// 品种类别
        /// </summary>
        [DataMember]
        public string VarietyCategories { get; set; }
        #endregion        
    }
}
