using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：现货_品种_交易单位换算表 实体类CM_UnitConversion 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_UnitConversion
	{
		public CM_UnitConversion()
		{}
		#region Model

	    private int _unitConversionID;
		private int? _breedclassid;
		private int? _value;
		private int? _unitidto;
		private int? _unitidfrom;

        /// <summary>
        /// 交易单位换算标识
        /// </summary>
        [DataMember]
	    public int UnitConversionID
	    {
            set { _unitConversionID = value; }
            get { return _unitConversionID; }
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
		/// 量
		/// </summary>
        [DataMember]
		public int? Value
		{
			set{ _value=value;}
			get{return _value;}
		}
		/// <summary>
		/// 单位标识2
		/// </summary>
        [DataMember]
		public int? UnitIDTo
		{
			set{ _unitidto=value;}
			get{return _unitidto;}
		}
		/// <summary>
		/// 单位标识1
		/// </summary>
        [DataMember]
		public int? UnitIDFrom
		{
			set{ _unitidfrom=value;}
			get{return _unitidfrom;}
		}
		#endregion Model

	}
}

