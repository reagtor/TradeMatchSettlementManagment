using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：现货_品种_涨跌幅 实体类XH_SpotHighLowValue 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class XH_SpotHighLowValue
	{
		public XH_SpotHighLowValue()
		{}
		#region Model
		private int _hightlowvalueid;
        private decimal? _fundYestclosepricescale;
		private decimal? _righthighlowscale;
		private decimal? _normalvalue;
		private decimal? _stvalue;
		private int? _breedclasshighlowid;
		/// <summary>
		/// 涨跌幅取值标识
		/// </summary>
        [DataMember]
		public int HightLowValueID
		{
			set{ _hightlowvalueid=value;}
			get{return _hightlowvalueid;}
		}
		/// <summary>
		/// 基金昨日收盘价的上下百分比例
		/// </summary>
        [DataMember]
        public decimal? FundYestClosePriceScale
		{
			set{ _fundYestclosepricescale=value;}
			get{return _fundYestclosepricescale;}
		}
		/// <summary>
		/// 权证涨跌幅比例
		/// </summary>
        [DataMember]
		public decimal? RightHighLowScale
		{
			set{ _righthighlowscale=value;}
			get{return _righthighlowscale;}
		}
		/// <summary>
		/// 正常企业
		/// </summary>
        [DataMember]
		public decimal? NormalValue
		{
			set{ _normalvalue=value;}
			get{return _normalvalue;}
		}
		/// <summary>
		/// ST企业
		/// </summary>
        [DataMember]
		public decimal? StValue
		{
			set{ _stvalue=value;}
			get{return _stvalue;}
		}
		/// <summary>
		/// 品种涨跌幅标识
		/// </summary>
        [DataMember]
		public int? BreedClassHighLowID
		{
			set{ _breedclasshighlowid=value;}
			get{return _breedclasshighlowid;}
		}
		#endregion Model

	}
}

