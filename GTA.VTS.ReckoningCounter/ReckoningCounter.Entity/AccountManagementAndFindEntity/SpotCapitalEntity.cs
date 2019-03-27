using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ReckoningCounter.Model;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:现货资金实体
    /// Desc.:现货汇总实体包括可用资金和持仓市值等
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class SpotCapitalEntity
    {
        /// <summary>
        /// 现货资金账户实体
        /// </summary>
        [DataMember]
        public XH_CapitalAccountTableInfo GTCapitalObj { get; private set; }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="gTCapitalObj">现货资金账号信息实体</param>
        /// <param name="dMarketValue">现所有持仓总市值</param>
        /// <param name="currencyName">当前交易货币类型</param>
        /// <param name="notDoneProfitLossTotal">未实现盈亏</param>
        public SpotCapitalEntity(XH_CapitalAccountTableInfo gTCapitalObj, decimal dMarketValue, string currencyName, decimal notDoneProfitLossTotal)
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
        public SpotCapitalEntity()
        {
            GTCapitalObj = null;
            MarketValue = 0;
            AssetAmount = 0;
            NotDoneProfitLossTotal = 0;
            CurrencyName = string.Empty;
        }

        /// <summary>
        /// 市值
        /// </summary>
        [DataMember]
        public decimal MarketValue { get; private set; }

        /// <summary>
        /// 资产总数
        /// </summary>
        [DataMember]
        public decimal AssetAmount { get; private set; }

        /// <summary>
        /// 币种名称
        /// </summary>
        [DataMember]
        public string CurrencyName { get; private set; }

        #region ====统计而增加的属性===
        #region 总盈亏
        /// <summary>
        /// 总盈亏=累计已实现盈亏+未实现盈亏
        /// </summary>
        [DataMember]
        public decimal ProfitLossTotal { get; private set; }
        #endregion

        #region 未实现盈亏
        /// <summary>
        /// 未实现盈亏=汇总所有持仓表中的浮动盈亏
        /// </summary>
        [DataMember]
        public decimal NotDoneProfitLossTotal { get; private set; }
        #endregion
        #endregion
    }
}