using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：现货_交易商品品种_持仓限制 实体类XH_SpotPosition 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class XH_SpotPosition
	{
		public XH_SpotPosition()
		{}
		#region Model
		private decimal? _rate;
		private int _breedclassid;
		/// <summary>
		/// 持仓比率
		/// </summary>
        [DataMember]
		public decimal? Rate
		{
			set{ _rate=value;}
			get{return _rate;}
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
		#endregion Model

	}
}

