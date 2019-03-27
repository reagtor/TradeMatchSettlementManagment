using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：港股_品种_交易规则表 实体类HK_SpotTradeRules 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：刘书伟
    /// 日期:2009-10-21
    /// </summary>
    [DataContract]
    public class HK_SpotTradeRules
    {
        public HK_SpotTradeRules()
        { }
        #region Model
        private int? _funddeliveryinstitution;
        private int? _stockdeliveryinstitution;
        private int? _maxleavequantity;
        private int? _marketunitid;
        private int _priceunit;
        private int _breedclassid;
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
        /// 每笔最大委托量
        /// </summary>
        [DataMember]
        public int? MaxLeaveQuantity
        {
            set { _maxleavequantity = value; }
            get { return _maxleavequantity; }
        }
        /// <summary>
        /// 行情成交量单位
        /// </summary>
        [DataMember]
        public int? MarketUnitID
        {
            set { _marketunitid = value; }
            get { return _marketunitid; }
        }
        /// <summary>
        /// 计价单位
        /// </summary>
        [DataMember]
        public int PriceUnit
        {
            set { _priceunit = value; }
            get { return _priceunit; }
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
        #endregion Model

    }
}
