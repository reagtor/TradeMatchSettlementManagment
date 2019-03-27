using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：熔断_时间段标识 实体类CM_FuseTimesection 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_FuseTimesection
	{
		public CM_FuseTimesection()
		{}
		#region Model
		private int _timesectionid;
		private string _commoditycode;
		private DateTime? _starttime;
		private DateTime? _endtime;
		/// <summary>
		/// 时间段表示
		/// </summary>
        [DataMember]
		public int TimesectionID
		{
			set{ _timesectionid=value;}
			get{return _timesectionid;}
		}
		/// <summary>
		/// 商品代码
		/// </summary>
        [DataMember]
		public string CommodityCode
		{
			set{ _commoditycode=value;}
			get{return _commoditycode;}
		}
		/// <summary>
		/// 允许起始时间
		/// </summary>
        [DataMember]
		public DateTime? StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// 允许截止时间
		/// </summary>
        [DataMember]
		public DateTime? EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

