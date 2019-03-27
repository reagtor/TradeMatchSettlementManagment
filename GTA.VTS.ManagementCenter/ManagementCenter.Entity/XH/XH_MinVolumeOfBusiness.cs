using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易规则_交易方向_交易单位_交易量(最小交易单位) 实体类XH_MinVolumeOfBusiness 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class XH_MinVolumeOfBusiness
	{
		public XH_MinVolumeOfBusiness()
		{}
		#region Model
		private int _minvolumeofbusinessid;
		private int? _volumeofbusiness;
		private int? _unitid;
		private int? _tradewayid;
		private int? _breedclassid;
		/// <summary>
		/// 最小交易单位标识
		/// </summary>
        [DataMember]
		public int MinVolumeOfBusinessID
		{
			set{ _minvolumeofbusinessid=value;}
			get{return _minvolumeofbusinessid;}
		}
		/// <summary>
		/// 可交易量
		/// </summary>
        [DataMember]
		public int? VolumeOfBusiness
		{
			set{ _volumeofbusiness=value;}
			get{return _volumeofbusiness;}
		}
		/// <summary>
		/// 单位标识
		/// </summary>
        [DataMember]
		public int? UnitID
		{
			set{ _unitid=value;}
			get{return _unitid;}
		}
		/// <summary>
		/// 交易方向标识
		/// </summary>
        [DataMember]
		public int? TradeWayID
		{
			set{ _tradewayid=value;}
			get{return _tradewayid;}
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
		#endregion Model

	}
}

