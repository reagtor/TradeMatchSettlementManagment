using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ReckoningCounter.Entity.Model.HK;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity.HK
{
    /// <summary>
    /// Title:港股资金明细查询返回实体
    /// Create by:李健华
    /// Create Date:2009-10-19
    /// </summary>
    [DataContract]
    public class HKCapitalEntity
    {

        #region 构造函数
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="gTCapitalObj">港股资金账号信息实体</param>
        /// <param name="dMarketValue">现所有持仓总市值</param>
        /// <param name="currencyName">当前交易货币类型</param>
        /// <param name="notDoneProfitLossTotal">未实现盈亏</param>
        public HKCapitalEntity(HK_CapitalAccountInfo gTCapitalObj, decimal dMarketValue, string currencyName, decimal notDoneProfitLossTotal)
        {
            GTCapitalObj = gTCapitalObj;
            MarketValue = dMarketValue;
            AssetAmount = MarketValue + GTCapitalObj.CapitalBalance;
            NotDoneProfitLossTotal = notDoneProfitLossTotal;
            ProfitLossTotal = gTCapitalObj.HasDoneProfitLossTotal + notDoneProfitLossTotal;
            CurrencyName = currencyName;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public HKCapitalEntity()
        {
            GTCapitalObj = null;
            MarketValue = 0;
            AssetAmount = 0;
            NotDoneProfitLossTotal = 0;
            CurrencyName = string.Empty;
        }
        #endregion

        #region 港股资金帐户实体 GTCapitalObj
        /// <summary>
        /// 港股资金帐户实体
        /// </summary>
        [DataMember]
        public HK_CapitalAccountInfo GTCapitalObj { get; private set; }
        #endregion

        #region 市值 MarketValue
        /// <summary>
        /// 市值
        /// </summary>
        [DataMember]
        public decimal MarketValue { get; private set; }
        #endregion

        #region 资产总数 AssetAmount
        /// <summary>
        /// 资产总数
        /// </summary>
        [DataMember]
        public decimal AssetAmount { get; private set; }
        #endregion

        #region 币种名称 CurrencyName
        /// <summary>
        /// 币种名称
        /// </summary>
        [DataMember]
        public string CurrencyName { get; private set; }
        #endregion

        #region 总盈亏 ProfitLossTotal
        /// <summary>
        /// 总盈亏=累计已实现盈亏+未实现盈亏
        /// </summary>
        [DataMember]
        public decimal ProfitLossTotal { get; private set; }
        #endregion

        #region 未实现盈亏 NotDoneProfitLossTotal
        /// <summary>
        /// 未实现盈亏=汇总所有持仓表中的浮动盈亏
        /// </summary>
        [DataMember]
        public decimal NotDoneProfitLossTotal { get; private set; }
        #endregion
    }
}
