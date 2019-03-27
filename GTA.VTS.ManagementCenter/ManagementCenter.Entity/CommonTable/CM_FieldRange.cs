using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：字段_范围表 实体类CM_FieldRange 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_FieldRange
	{
		public CM_FieldRange()
		{}
		#region Model
        private int _fieldrangeid;
		private decimal? _upperlimit;
		private decimal? _lowerlimit;
		private int _upperlimitifequation;
		private int _lowerlimitifequation;
        //private string _propertymarket;
		/// <summary>
		/// 字段_范围标识
		/// </summary>
        [DataMember]
		public int FieldRangeID
		{
			set{ _fieldrangeid=value;}
			get{return _fieldrangeid;}
		}
		/// <summary>
		/// 上限
		/// </summary>
        [DataMember]
		public decimal? UpperLimit
		{
			set{ _upperlimit=value;}
			get{return _upperlimit;}
		}
		/// <summary>
		/// 下限
		/// </summary>
        [DataMember]
		public decimal? LowerLimit
		{
			set{ _lowerlimit=value;}
			get{return _lowerlimit;}
		}
		/// <summary>
		/// 上限是否可相等
		/// </summary>
        [DataMember]
		public int UpperLimitIfEquation
		{
			set{ _upperlimitifequation=value;}
			get{return _upperlimitifequation;}
		}
		/// <summary>
		/// 下限是否可相等
		/// </summary>
        [DataMember]
		public int LowerLimitIfEquation
		{
			set{ _lowerlimitifequation=value;}
			get{return _lowerlimitifequation;}
		}
	
		#endregion Model

	}
}

