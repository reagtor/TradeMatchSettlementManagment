using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易规则_最大委托量_范围_值 实体类XH_MaxLeaveQuantityRangeValue 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class XH_MaxLeaveQuantityRangeValue
	{
		public XH_MaxLeaveQuantityRangeValue()
		{}
		#region Model
		private decimal? _value;
		private int _breedclassid;
		private int _fieldrangeid;
		/// <summary>
		/// 值
		/// </summary>
        [DataMember]
		public decimal? Value
		{
			set{ _value=value;}
			get{return _value;}
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
		/// 字段_范围标识
		/// </summary>
        [DataMember]
		public int FieldRangeID
		{
			set{ _fieldrangeid=value;}
			get{return _fieldrangeid;}
		}
		#endregion Model

	}
}

