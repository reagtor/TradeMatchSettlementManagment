using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：可交易商品_熔断表 实体类CM_CommodityFuse 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
	/// </summary>
    [DataContract]
	public class CM_CommodityFuse
	{
		public CM_CommodityFuse()
		{}
		#region Model
        //private int? _noallowstarttime;
        private decimal _triggeringscale;
		private int _triggeringduration;
		private int _fusedurationlimit;
		private int _fusetimeofday;
		private string _commoditycode;
		/// <summary>
		/// 不允许启动时间休市时隔(分钟)
		/// </summary>
        //public int? NoAllowStartTime
        //{
        //    set{ _noallowstarttime=value;}
        //    get{return _noallowstarttime;}
        //}
		/// <summary>
		/// 触发比例
		/// </summary>
        [DataMember]
		public decimal TriggeringScale
		{
			set{ _triggeringscale=value;}
			get{return _triggeringscale;}
		}
		/// <summary>
		/// 触发持续时间限制(分钟)
		/// </summary>
        [DataMember]
		public int TriggeringDuration
		{
			set{ _triggeringduration=value;}
			get{return _triggeringduration;}
		}
		/// <summary>
		/// 熔断持续时间限制(分钟)
		/// </summary>
        [DataMember]
		public int FuseDurationLimit
		{
			set{ _fusedurationlimit=value;}
			get{return _fusedurationlimit;}
		}
		/// <summary>
		/// 熔断次数(次/天)
		/// </summary>
        [DataMember]
		public int FuseTimeOfDay
		{
			set{ _fusetimeofday=value;}
			get{return _fusetimeofday;}
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
		#endregion Model

	}
}

