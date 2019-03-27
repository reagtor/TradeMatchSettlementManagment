using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：品种_期货_交易费用 实体类QH_FutureCosts 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
    public class QH_FutureCosts
    {
        public QH_FutureCosts()
        { }
        #region Model
        //private decimal? _TradeUnitCharge;
        private decimal _turnoverrateofservicecharge;
        private int? _currencytypeid;
        private int _costType;
        private int _breedclassid;
        /// <summary>
        /// 每手交易手续费
        /// </summary>
        //[DataMember]
        //public decimal? TradeUnitCharge
        //{
        //    set { _TradeUnitCharge = value; }
        //    get { return _TradeUnitCharge; }
        //}
        /// <summary>
        /// 单位或成交金额比率手续费
        /// </summary>
        [DataMember]
        public decimal TurnoverRateOfServiceCharge
        {
            set { _turnoverrateofservicecharge = value; }
            get { return _turnoverrateofservicecharge; }
        }
        /// <summary>
        /// 费用类型
        /// </summary>
        [DataMember]
        public int CostType
        {
            set { _costType = value; }
            get { return _costType; }
        }

        /// <summary>
        /// 货币类型
        /// </summary>
        [DataMember]
        public int? CurrencyTypeID
        {
            set { _currencytypeid = value; }
            get { return _currencytypeid; }
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

