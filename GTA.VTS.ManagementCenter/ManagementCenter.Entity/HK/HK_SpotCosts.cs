using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：港股_交易费用表 实体类HK_SpotCosts 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：刘书伟
    /// 日期:2009-10-21
    /// </summary>
    [DataContract]
    public class HK_SpotCosts
    {
        public HK_SpotCosts()
        { }
        #region Model
        private decimal? _stampduty;
        private decimal? _stampdutystartingpoint;
        private decimal? _commision;
        private decimal? _monitoringfee;
        private decimal? _commisionstartingpoint;
        private decimal? _poundagevalue;
        private decimal? _systemtoll;
        private int? _stampdutytypeid;
        private int _breedclassid;
        private int? _currencytypeid;
        private decimal? _transfertoll;
        /// <summary>
        /// 印花税
        /// </summary>
        [DataMember]
        public decimal? StampDuty
        {
            set { _stampduty = value; }
            get { return _stampduty; }
        }
        /// <summary>
        /// 印花税起点
        /// </summary>
        [DataMember]
        public decimal? StampDutyStartingpoint
        {
            set { _stampdutystartingpoint = value; }
            get { return _stampdutystartingpoint; }
        }
        /// <summary>
        /// 佣金
        /// </summary>
        [DataMember]
        public decimal? Commision
        {
            set { _commision = value; }
            get { return _commision; }
        }
        /// <summary>
        /// 监管费
        /// </summary>
        [DataMember]
        public decimal? MonitoringFee
        {
            set { _monitoringfee = value; }
            get { return _monitoringfee; }
        }
        /// <summary>
        /// 佣金起点
        /// </summary>
        [DataMember]
        public decimal? CommisionStartingpoint
        {
            set { _commisionstartingpoint = value; }
            get { return _commisionstartingpoint; }
        }
        /// <summary>
        /// 交易费
        /// </summary>
        [DataMember]
        public decimal? PoundageValue
        {
            set { _poundagevalue = value; }
            get { return _poundagevalue; }
        }
        /// <summary>
        /// 交易系统使用费
        /// </summary>
        [DataMember]
        public decimal? SystemToll
        {
            set { _systemtoll = value; }
            get { return _systemtoll; }
        }
        /// <summary>
        /// 印花税单边或者双边取值
        /// </summary>
        [DataMember]
        public int? StampDutyTypeID
        {
            set { _stampdutytypeid = value; }
            get { return _stampdutytypeid; }
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
        /// 交易货币类型标识
        /// </summary>
        [DataMember]
        public int? CurrencyTypeID
        {
            set { _currencytypeid = value; }
            get { return _currencytypeid; }
        }
        /// <summary>
        /// 过户费
        /// </summary>
        [DataMember]
        public decimal? TransferToll
        {
            set { _transfertoll = value; }
            get { return _transfertoll; }
        }
        #endregion Model

    }
}
