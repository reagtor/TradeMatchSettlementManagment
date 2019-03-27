using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ReckoningCounter.Model;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:期货资金实体
    /// Desc.:期货资金实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FuturesCapitalEntity
    {
        /// <summary>
        /// 期货资金账户实体
        /// </summary>
        [DataMember]
        public QH_CapitalAccountTableInfo GTCapitalObj { get; private set; }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="gTCapitalObj">期货资金表实体</param>
        /// <param name="dMarketValue">总市值</param>
        /// <param name="currencyName">交易货币名称</param>
        /// <param name="floatProfitLossTotal">总的持仓浮动盈亏</param>
        /// <param name="marketProfitLossTotal">总的持仓盯市盈亏</param>
        public FuturesCapitalEntity(QH_CapitalAccountTableInfo gTCapitalObj, decimal dMarketValue, string currencyName, decimal floatProfitLossTotal, decimal marketProfitLossTotal)
        {
            GTCapitalObj = gTCapitalObj;
            MarketValue = dMarketValue;
            //AssetAmount = MarketValue + GTCapitalObj.CapitalBalance.Value;
            //AssetAmount = MarketValue + GTCapitalObj.AvailableCapital + GTCapitalObj.FreezeCapitalTotal;
            AssetAmount = GTCapitalObj.CapitalBalance + gTCapitalObj.MarginTotal;
            CurrencyName = currencyName;
            FloatProfitLossTotal = floatProfitLossTotal;
            MarketProfitLossTotal = marketProfitLossTotal;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FuturesCapitalEntity()
        {
            GTCapitalObj = null;
            MarketValue = 0;
            AssetAmount = 0;
            CurrencyName = string.Empty;
        }
        #region 增加统计盈亏
        /// <summary>
        /// 总的持仓浮动盈亏
        /// </summary>
        [DataMember]
        public decimal FloatProfitLossTotal { get; private set; }
        /// <summary>
        /// 总的持仓盯市盈亏 
        /// </summary>
        [DataMember]
        public decimal MarketProfitLossTotal { get; private set; }
        #endregion

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


        //////////////////////////////////////////////////////////////
        /// <summary>
        /// 人民币上日结存
        /// </summary>
        private string _RMBBalanceOfTheDay = string.Empty;
        /// <summary>
        /// 人民币上日结存
        /// </summary>
        [DataMember]
        public string RMBBalanceOfTheDay
        {
            get { return _RMBBalanceOfTheDay; }
            set { _RMBBalanceOfTheDay = value; }
        }


        /// <summary>
        /// 港币上日结存
        /// </summary>
        private string _HKBalanceOfTheDay = string.Empty;
        /// <summary>
        ///  港币上日结存
        /// </summary>
        [DataMember]
        public string HKBalanceOfTheDay
        {
            get { return _HKBalanceOfTheDay; }
            set { _HKBalanceOfTheDay = value; }
        }


        /// <summary>
        /// 美元上日结存
        /// </summary>
        private string _USBalanceOfTheDay = string.Empty;
        /// <summary>
        /// 美元上日结存
        /// </summary>
        [DataMember]
        public string USBalanceOfTheDay
        {
            get { return _USBalanceOfTheDay; }
            set { _USBalanceOfTheDay = value; }
        }


        /// <summary>
        /// 人民币当日出入金
        /// </summary>
        private string _RMBTodayOutInCapital = string.Empty;
        /// <summary>
        /// 人民币当日出入金
        /// </summary>
        [DataMember]
        public string RMBTodayOutInCapital
        {
            get { return _RMBTodayOutInCapital; }
            set { _RMBTodayOutInCapital = value; }
        }

        /// <summary>
        /// 港币当日出入金
        /// </summary>
        private string _HKTodayOutInCapital = string.Empty;
        /// <summary>
        /// 港币当日出入金
        /// </summary>
        [DataMember]
        public string HKTodayOutInCapital
        {
            get { return _HKTodayOutInCapital; }
            set { _HKTodayOutInCapital = value; }
        }


        /// <summary>
        /// 美元当日出入金
        /// </summary>
        private string _USTodayOutInCapital = string.Empty;
        /// <summary>
        /// 美元当日出入金
        /// </summary>
        [DataMember]
        public string USTodayOutInCapital
        {
            get { return _USTodayOutInCapital; }
            set { _USTodayOutInCapital = value; }
        }


        /// <summary>
        /// 人民币保证金总额
        /// </summary>
        private string _RMBMarginTotal = string.Empty;
        /// <summary>
        /// 人民币保证金总额
        /// </summary>
        [DataMember]
        public string RMBMarginTotal
        {
            get { return _RMBMarginTotal; }
            set { _RMBMarginTotal = value; }
        }


        /// <summary>
        /// 港币保证金总额
        /// </summary>
        private string _HKMarginTotal = string.Empty;
        /// <summary>
        /// 港币保证金总额
        /// </summary>
        [DataMember]
        public string HKMarginTotal
        {
            get { return _HKMarginTotal; }
            set { _HKMarginTotal = value; }
        }


        /// <summary>
        ///  美元保证金总额
        /// </summary>
        private string _USMarginTotal = string.Empty;
        /// <summary>
        /// 美元保证金总额
        /// </summary>
        [DataMember]
        public string USMarginTotal
        {
            get { return _USMarginTotal; }
            set { _USMarginTotal = value; }
        }


        /// <summary>
        /// 人民币冻结总额
        /// </summary>
        private string _RMBFreezeTotal = string.Empty;
        /// <summary>
        /// 人民币冻结总额
        /// </summary>
        [DataMember]
        public string RMBFreezeTotal
        {
            get { return _RMBFreezeTotal; }
            set { _RMBFreezeTotal = value; }
        }


        /// <summary>
        /// 港币冻结总额
        /// </summary>
        private string _HKFreezeTotal = string.Empty;
        /// <summary>
        /// 港币冻结总额
        /// </summary>
        [DataMember]
        public string HKFreezeTotal
        {
            get { return _HKFreezeTotal; }
            set { _HKFreezeTotal = value; }
        }


        /// <summary>
        /// 美元冻结总额
        /// </summary>
        private string _USFreezeTotal = string.Empty;
        /// <summary>
        /// 美元冻结总额
        /// </summary>
        [DataMember]
        public string USFreezeTotal
        {
            get { return _USFreezeTotal; }
            set { _USFreezeTotal = value; }
        }



        /// <summary>
        /// 人民币可用资金
        /// </summary>
        private string _RMBAvailableCapital = string.Empty;
        /// <summary>
        /// 人民币可用资金
        /// </summary>
        [DataMember]
        public string RMBAvailableCapital
        {
            get { return _RMBAvailableCapital; }
            set { _RMBAvailableCapital = value; }
        }

        /// <summary>
        /// 港币可用资金
        /// </summary>
        private string _HKAvailableCapital = string.Empty;
        /// <summary>
        /// 港币可用资金
        /// </summary>
        [DataMember]
        public string HKAvailableCapital
        {
            get { return _HKAvailableCapital; }
            set { _HKAvailableCapital = value; }
        }


        /// <summary>
        /// 美元可用资金
        /// </summary>
        private string _USAvailableCapital = string.Empty;
        /// <summary>
        ///  美元可用资金
        /// </summary>
        [DataMember]
        public string USAvailableCapital
        {
            get { return _USAvailableCapital; }
            set { _USAvailableCapital = value; }
        }

        ///// <summary>
        /////人民币可交易资金 
        ///// </summary>
        //private string _RMBTradableCapital = string.Empty;
        ///// <summary>
        ///// 人民币可交易资金
        ///// </summary>
        //public string RMBTradableCapital
        //{
        //    get { return _RMBTradableCapital; }
        //    set { _RMBTradableCapital = value; }
        //}

        ///// <summary>
        /////  港币可交易资金
        ///// </summary>
        //private string _HKTradableCapital = string.Empty;
        ///// <summary>
        ///// 港币可交易资金
        ///// </summary>
        //public string HKTradableCapital
        //{
        //    get { return _HKTradableCapital; }
        //    set { _HKTradableCapital = value; }
        //}

        ///// <summary>
        ///// 美元可交易资金
        ///// </summary>
        //private string _USTradableCapital = string.Empty;
        ///// <summary>
        ///// 美元可交易资金
        ///// </summary>
        //public string USTradableCapital
        //{
        //    get { return _USTradableCapital; }
        //    set { _USTradableCapital = value; }
        //}


        ///// <summary>
        /////  人民币剩余冻结总额
        ///// </summary>
        //private string _RMBRemainFreezeTotal = string.Empty;
        ///// <summary>
        ///// 人民币剩余冻结总额
        ///// </summary>
        //public string RMBRemainFreezeTotal
        //{
        //    get { return _RMBRemainFreezeTotal; }
        //    set { _RMBRemainFreezeTotal = value; }
        //}


        ///// <summary>
        ///// 港币剩余冻结总额
        ///// </summary>
        //private string _HKRemainFreezeTotal = string.Empty;
        ///// <summary>
        ///// 港币剩余冻结总额
        ///// </summary>
        //public string HKRemainFreezeTotal
        //{
        //    get { return _HKRemainFreezeTotal; }
        //    set { _HKRemainFreezeTotal = value; }
        //}


        ///// <summary>
        /////  美元剩余冻结总额
        ///// </summary>
        //private string _USRemainFreezeTotal = string.Empty;
        ///// <summary>
        ///// 美元剩余冻结总额
        ///// </summary>
        //public string USRemainFreezeTotal
        //{
        //    get { return _USRemainFreezeTotal; }
        //    set { _USRemainFreezeTotal = value; }
        //}
    }
}