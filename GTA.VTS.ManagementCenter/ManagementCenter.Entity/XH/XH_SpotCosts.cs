using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：品种_现货_交易费用 实体类XH_SpotCosts 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class XH_SpotCosts
	{
		public XH_SpotCosts()
		{}
		#region Model
		private int? _getvaluetypeid;
        private decimal _stampduty;
        private decimal _stampdutystartingpoint;
		private decimal? _commision;
        private decimal _transfertoll;
        private decimal? _monitoringfee;
        private decimal? _transfertollstartingpoint;
        private decimal? _clearingfees;
        private decimal _commisionstartingpoint;
        private decimal? _poundagesinglevalue;
        private decimal? _systemtoll;
		private int _breedclassid;
		private int? _currencytypeid;
		private int? _stampdutytypeid;
	    private int _transferTollTypeID;
		/// <summary>
		/// 交易规则_取值类型标识(单值还是范围值)
		/// </summary>
        [DataMember]
		public int? GetValueTypeID
		{
			set{ _getvaluetypeid=value;}
			get{return _getvaluetypeid;}
		}
		/// <summary>
		/// 印花税
		/// </summary>
        [DataMember]
        public decimal StampDuty
		{
			set{ _stampduty=value;}
			get{return _stampduty;}
		}
		/// <summary>
		/// 印花税起点
		/// </summary>
        [DataMember]
        public decimal StampDutyStartingpoint
		{
			set{ _stampdutystartingpoint=value;}
			get{return _stampdutystartingpoint;}
		}
		/// <summary>
		/// 佣金
		/// </summary>
        [DataMember]
		public decimal? Commision
		{
			set{ _commision=value;}
			get{return _commision;}
		}
		/// <summary>
		/// 过户费
		/// </summary>
        [DataMember]
        public decimal TransferToll
		{
			set{ _transfertoll=value;}
			get{return _transfertoll;}
		}
		/// <summary>
		/// 监管费
		/// </summary>
        [DataMember]
        public decimal? MonitoringFee
		{
			set{ _monitoringfee=value;}
			get{return _monitoringfee;}
		}
		/// <summary>
		/// 过户费起点
		/// </summary>
        [DataMember]
        public decimal? TransferTollStartingpoint
		{
			set{ _transfertollstartingpoint=value;}
			get{return _transfertollstartingpoint;}
		}

        /// <summary>
        /// 过户费取值类型标识
        /// </summary>
        [DataMember]
	    public int TransferTollTypeID
	    {
            set { _transferTollTypeID = value; }
            get { return _transferTollTypeID; }
	    }

		/// <summary>
		/// 结算费
		/// </summary>
        [DataMember]
        public decimal? ClearingFees
		{
			set{ _clearingfees=value;}
			get{return _clearingfees;}
		}
		/// <summary>
		/// 佣金起点
		/// </summary>
        [DataMember]
        public decimal CommisionStartingpoint
		{
			set{ _commisionstartingpoint=value;}
			get{return _commisionstartingpoint;}
		}
		/// <summary>
		/// 交易手续费单值
		/// </summary>
        [DataMember]
        public decimal? PoundageSingleValue
		{
			set{ _poundagesinglevalue=value;}
			get{return _poundagesinglevalue;}
		}
		/// <summary>
		/// 交易系统使用费
		/// </summary>
        [DataMember]
        public decimal? SystemToll
		{
			set{ _systemtoll=value;}
			get{return _systemtoll;}
		}
		/// <summary>
		/// 品种标识
		/// </summary>
        [DataMember]
		public int BreedClassID
		{
			set{ _breedclassid=value;}
			get{return _breedclassid;}
		}
		/// <summary>
		/// 交易货币类型标识
		/// </summary>
        [DataMember]
		public int? CurrencyTypeID
		{
			set{ _currencytypeid=value;}
			get{return _currencytypeid;}
		}
		/// <summary>
		/// 印花税单边或者双边取值
		/// </summary>
        [DataMember]
		public int? StampDutyTypeID
		{
			set{ _stampdutytypeid=value;}
			get{return _stampdutytypeid;}
		}
		#endregion Model

	}
}

