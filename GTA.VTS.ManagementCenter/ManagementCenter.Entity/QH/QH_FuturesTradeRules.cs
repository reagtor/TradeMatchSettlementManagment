using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：期货_品种_交易规则 实体类QH_FuturesTradeRules 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// Desc.: 增加交割月涨跌幅 DeliveryMonthHighLowStopValue
    /// Update by：董鹏
    /// Update date:2010-01-20
    /// </summary>
    [DataContract]
    public class QH_FuturesTradeRules
    {
        public QH_FuturesTradeRules()
        {
        }

        #region Model

        private decimal? _unitmultiple;
        private decimal? _leastchangeprice;
        private int _ifcontaincnewyear;
        private int? _agreementdeliveryinstitution;
        private int? _funddeliveryinstitution;
        //private int? _TradeUnit;
        private int _breedclassid;
        private int? _lasttradingdayid;
        private int? _consignquantumid;
        private int _isslew;
       
       // private int? _unitpricing;
        private int? _highlowstopscopeid;
        private decimal? _highlowstopscopevalue;
        private decimal _newBreedFuturesPactHighLowStopValue;
        private decimal _newMonthFuturesPactHighLowStopValue;
        private int? _unitsid;
        private int _priceUnit;
        private int _marketUnitID;
        private string _futruesCode;

        /// <summary>
        /// 交易单位计价单位倍数
        /// </summary>
        [DataMember]
        public decimal? UnitMultiple
        {
            set { _unitmultiple = value; }
            get { return _unitmultiple; }
        }

        /// <summary>
        /// 最小变动价位
        /// </summary>
        [DataMember]
        public decimal? LeastChangePrice
        {
            set { _leastchangeprice = value; }
            get { return _leastchangeprice; }
        }

        /// <summary>
        /// 合约交割月是否包含春节月份
        /// </summary>
        [DataMember]
        public int IfContainCNewYear
        {
            set { _ifcontaincnewyear = value; }
            get { return _ifcontaincnewyear; }
        }

        /// <summary>
        /// 合约的交割制度
        /// </summary>
        [DataMember]
        public int? AgreementDeliveryInstitution
        {
            set { _agreementdeliveryinstitution = value; }
            get { return _agreementdeliveryinstitution; }
        }

        /// <summary>
        /// 资金的交割制度
        /// </summary>
        [DataMember]
        public int? FundDeliveryInstitution
        {
            set { _funddeliveryinstitution = value; }
            get { return _funddeliveryinstitution; }
        }

      
        /// <summary>
        /// 品种标识
        /// </summary>
        [DataMember]
        public int BreedClassID
        {
            set { _breedclassid = value; }
            get { return _breedclassid; }
        }

        /// <summary>
        /// 最后交易日标识
        /// </summary>
        [DataMember]
        public int? LastTradingDayID
        {
            set { _lasttradingdayid = value; }
            get { return _lasttradingdayid; }
        }

        /// <summary>
        /// 交易规则委托量标识
        /// </summary>
        [DataMember]
        public int? ConsignQuantumID
        {
            set { _consignquantumid = value; }
            get { return _consignquantumid; }
        }

        /// <summary>
        /// 是否充许回转
        /// </summary>
        [DataMember]
        public int IsSlew
        {
            set { _isslew = value; }
            get { return _isslew; }
        }

        /// <summary>
        /// 涨跌停板幅度类型标识
        /// </summary>
        [DataMember]
        public int? HighLowStopScopeID
        {
            set { _highlowstopscopeid = value; }
            get { return _highlowstopscopeid; }
        }

        /// <summary>
        /// 涨跌停板幅度
        /// </summary>
        [DataMember]
        public decimal? HighLowStopScopeValue
        {
            set { _highlowstopscopevalue = value; }
            get { return _highlowstopscopevalue; }
        }

        /// <summary>
        /// 新品种期货合约上市当日涨跌停板幅度
        /// </summary>
        [DataMember]
        public decimal NewBreedFuturesPactHighLowStopValue
        {
            set { _newBreedFuturesPactHighLowStopValue = value; }
            get { return _newBreedFuturesPactHighLowStopValue; }
        }

        /// <summary>
        /// 新月份期货合约上市当日涨跌停板幅度
        /// </summary>
        [DataMember]
        public decimal NewMonthFuturesPactHighLowStopValue
        {
            set { _newMonthFuturesPactHighLowStopValue = value; }
            get { return _newMonthFuturesPactHighLowStopValue; }
        }

        /// <summary>
        /// 交割月涨跌幅
        /// </summary>
        [DataMember]
        public decimal? DeliveryMonthHighLowStopValue
        {
            get;
            set;
        }

        /// <summary>
        /// 交易单位
        /// </summary>
        [DataMember]
        public int? UnitsID
        {
            set { _unitsid = value; }
            get { return _unitsid; }
        }

        /// <summary>
        /// 计价单位
        /// </summary>
        [DataMember]
        public int PriceUnit
        {
            set { _priceUnit = value; }
            get { return _priceUnit; }
        }

        /// <summary>
        /// 行情成交量单位
        /// </summary>
        [DataMember]
        public int MarketUnitID
        {
            set { _marketUnitID = value; }
            get { return _marketUnitID; }
        }

        /// <summary>
        /// 期货交易代码
        /// </summary>
        [DataMember]
        public string FutruesCode
        {
            set { _futruesCode = value; }
            get {return  _futruesCode; }
        }

        #endregion Model
    }
}