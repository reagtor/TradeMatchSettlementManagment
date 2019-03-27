using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易所类型_交易时间表 实体类CM_TradeTime 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_TradeTime
	{
		public CM_TradeTime()
		{}
		#region Model
		private int _tradetimeid;
		private DateTime? _starttime;
		private DateTime? _endtime;
		private int? _boursetypeid;
		/// <summary>
		/// 交易所类型_交易时间标识
		/// </summary>
        [DataMember]
		public int TradeTimeID
		{
			set{ _tradetimeid=value;}
			get{return _tradetimeid;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
        [DataMember]
		public DateTime? StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// 结束时间
		/// </summary>
        [DataMember]
		public DateTime? EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 交易所类型标识
		/// </summary>
        [DataMember]
		public int? BourseTypeID
		{
			set{ _boursetypeid=value;}
			get{return _boursetypeid;}
		}
		#endregion Model

	}
}

