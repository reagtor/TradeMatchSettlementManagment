using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：期货_持仓限制 实体类QH_PositionLimitValue 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-12-13
    /// Desc.:增加持仓限制类型PositionLimitType
    /// Update by：董鹏
    /// Update date:2010-01-19
    /// </summary>
    [DataContract]
	public class QH_PositionLimitValue
	{
		public QH_PositionLimitValue()
		{}
		#region Model
		private int? _start;
		private decimal? _positionvalue;
		private int _deliverymonthtypeid;
		private int _positionlimitvalueid;
		private int? _ends;
		private int? _breedclassid;
		private int? _upperlimitifequation;
		private int? _lowerlimitifequation;
		private int? _positionbailtypeid;
		private int? _positionvaluetypeid;
		/// <summary>
		/// 开始
		/// </summary>
        [DataMember]
		public int? Start
		{
			set{ _start=value;}
			get{return _start;}
		}
		/// <summary>
		/// 持仓
		/// </summary>
        [DataMember]
		public decimal? PositionValue
		{
			set{ _positionvalue=value;}
			get{return _positionvalue;}
		}
		/// <summary>
		/// 交割月份类型标识
        /// 1、交割月份 2、一般月份 3、交割月份前一个月 4、交割月份前二个月 5、交割月份前三个月
		/// </summary>
        [DataMember]
		public int DeliveryMonthType
		{
			set{ _deliverymonthtypeid=value;}
			get{return _deliverymonthtypeid;}
		}
		/// <summary>
		/// 期货-持仓限制标识
		/// </summary>
        [DataMember]
		public int PositionLimitValueID
		{
			set{ _positionlimitvalueid=value;}
			get{return _positionlimitvalueid;}
		}
		/// <summary>
		/// 结束
		/// </summary>
        [DataMember]
		public int? Ends
		{
			set{ _ends=value;}
			get{return _ends;}
		}
		/// <summary>
		/// 品种标识
		/// </summary>
        [DataMember]
		public int? BreedClassID
		{
			set{ _breedclassid=value;}
			get{return _breedclassid;}
		}
		/// <summary>
		/// 上限是否可相等
		/// </summary>
        [DataMember]
		public int? UpperLimitIfEquation
		{
			set{ _upperlimitifequation=value;}
			get{return _upperlimitifequation;}
		}
		/// <summary>
		/// 下限是否可相等
		/// </summary>
        [DataMember]
		public int? LowerLimitIfEquation
		{
			set{ _lowerlimitifequation=value;}
			get{return _lowerlimitifequation;}
		}
		/// <summary>
		/// 持仓和保证金控制类型标识
		/// </summary>
        [DataMember]
		public int? PositionBailTypeID
		{
			set{ _positionbailtypeid=value;}
			get{return _positionbailtypeid;}
		}
		/// <summary>
		/// 商品期货_持仓取值标识
		/// </summary>
        [DataMember]
		public int? PositionValueTypeID
		{
			set{ _positionvaluetypeid=value;}
			get{return _positionvaluetypeid;}
		}

        ///// <summary>
        ///// 持仓限制类型
        ///// 1、一般月份持仓限额 2、临近交割月持仓限额 3、交割月持仓限额 4、最小交割单位整数倍限额
        ///// </summary>
        //[DataMember]
        //public int PositionLimitType
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 最小交割单位整数倍限额
        /// </summary>
        [DataMember]
        public int? MinUnitLimit
        {
            get;
            set;
        }

		#endregion Model

	}
}

