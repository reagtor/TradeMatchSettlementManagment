using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：品种_现货_交易费用_成交额_交易手续费 实体类XH_SpotRangeCost 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class XH_SpotRangeCost
	{
		public XH_SpotRangeCost()
		{}
		#region Model
		private decimal? _value;
        //private int _fieldrangeid;
		private int _breedclassid;
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
		/// 字段_范围标识
		/// </summary>
        //[DataMember]
        //public int FieldRangeID
        //{
        //    set{ _fieldrangeid=value;}
        //    get{return _fieldrangeid;}
        //}
		/// <summary>
		/// 品种标识
		/// </summary>
        [DataMember]
		public int BreedClassID
		{
			set{ _breedclassid=value;}
			get{return _breedclassid;}
		}
		#endregion Model

	}
}

