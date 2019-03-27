using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：现货_品种_交易规则 实体类XH_SpotTradeRules 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟 修改
    ///日期:2008-11-25
    /// </summary>
    [DataContract]
    public class XH_SpotTradeRules
    {
        public XH_SpotTradeRules()
        {
        }

        #region Model

        private int _breedclassid;
        private decimal? _minchangeprice;
        private int? _funddeliveryinstitution;
        private int? _stockdeliveryinstitution;
        private int _isslew;
        private int? _maxleavequantity;
        private int? _valuetypeminchangeprice;
        private int _marketUnitID;
        private int _priceUnit;
        private int _maxLeaveQuantityUnit;
        private int _minVolumeMultiples;
        private int? _breedClassValidID;
        private int? _breedClassHighLowID;
        /// 品种标识
        /// </summary>
        [DataMember]
        public int BreedClassID
        {
            set { _breedclassid = value; }
            get { return _breedclassid; }
        }

        /// <summary>
        /// 最小变动价位
        /// </summary>
        [DataMember]
        public decimal? MinChangePrice
        {
            set { _minchangeprice = value; }
            get { return _minchangeprice; }
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
        /// 股票的交割制度
        /// </summary>
        [DataMember]
        public int? StockDeliveryInstitution
        {
            set { _stockdeliveryinstitution = value; }
            get { return _stockdeliveryinstitution; }
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
        /// 每笔最大委托量
        /// </summary>
        [DataMember]
        public int? MaxLeaveQuantity
        {
            set { _maxleavequantity = value; }
            get { return _maxleavequantity; }
        }

        /// <summary>
        /// 最小变动价位取值类型-单值,多值
        /// </summary>
        [DataMember]
        public int? ValueTypeMinChangePrice
        {
            set { _valuetypeminchangeprice = value; }
            get { return _valuetypeminchangeprice; }
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
        /// 计价单位
        /// </summary>
        [DataMember]
        public int PriceUnit
        {
            set { _priceUnit = value; }
            get { return _priceUnit; }
        }

        /// <summary>
        /// 每笔最大委托量单位
        /// </summary>
        [DataMember]
        public int MaxLeaveQuantityUnit
        {
            set { _maxLeaveQuantityUnit = value; }
            get { return _maxLeaveQuantityUnit; }
        }

        /// <summary>
        /// 最小交易单位倍数
        /// </summary>
        [DataMember]
        public int MinVolumeMultiples
        {
            set { _minVolumeMultiples = value; }
            get { return _minVolumeMultiples; }
        }

        /// <summary>
        /// 品种有效申报标识
        /// </summary>
        [DataMember]
        public int? BreedClassValidID
        {
            set { _breedClassValidID = value; }
            get { return _breedClassValidID; }
        }

        /// <summary>
        /// 品种涨跌幅标识
        /// </summary>
        [DataMember]
        public int? BreedClassHighLowID
        {
            set { _breedClassHighLowID = value; }
            get { return _breedClassHighLowID; }
        }


        #endregion Model
    }
}