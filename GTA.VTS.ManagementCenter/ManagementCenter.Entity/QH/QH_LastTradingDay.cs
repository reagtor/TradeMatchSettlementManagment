using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：最后交易日 实体类QH_LastTradingDay 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_LastTradingDay
	{
		public QH_LastTradingDay()
		{}
		#region Model
		private int _lasttradingdayid;
		private int? _whatday;
		private int? _whatweek;
		private int? _week;
		private int  _sequence;
		private int? _lasttradingdaytypeid;
		/// <summary>
		/// 最后交易日标识
		/// </summary>
        [DataMember]
		public int LastTradingDayID
		{
			set{ _lasttradingdayid=value;}
			get{return _lasttradingdayid;}
		}
		/// <summary>
		/// 第几日
		/// </summary>
        [DataMember]
		public int? WhatDay
		{
			set{ _whatday=value;}
			get{return _whatday;}
		}
		/// <summary>
		/// 第几周
		/// </summary>
        [DataMember]
		public int? WhatWeek
		{
			set{ _whatweek=value;}
			get{return _whatweek;}
		}
		/// <summary>
		/// 星期几
		/// </summary>
        [DataMember]
		public int? Week
		{
			set{ _week=value;}
			get{return _week;}
		}
		/// <summary>
		/// 顺数或倒数
		/// </summary>
        [DataMember]
		public int Sequence
		{
			set{ _sequence=value;}
			get{return _sequence;}
		}
		/// <summary>
		/// 最后交易日类型标识
		/// </summary>
        [DataMember]
		public int? LastTradingDayTypeID
		{
			set{ _lasttradingdaytypeid=value;}
			get{return _lasttradingdaytypeid;}
		}
		#endregion Model

	}
}

