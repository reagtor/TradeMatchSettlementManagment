using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易所类型_非交易日期 实体类CM_NotTradeDate 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-11-26
    /// </summary>
    [DataContract]
	public class CM_NotTradeDate
	{
		public CM_NotTradeDate()
		{}
		#region Model
		private int _nottradedateid;
		//private int? _nottradedatemonth;
		private DateTime? _nottradeday;
		private int? _boursetypeid;
		/// <summary>
		/// 非交易日期标识
		/// </summary>
        [DataMember]
		public int NotTradeDateID
		{
			set{ _nottradedateid=value;}
			get{return _nottradedateid;}
		}
		/// <summary>
		/// 非交易月份
		/// </summary>
        //public int? NotTradeDateMonth
        //{
        //    set{ _nottradedatemonth=value;}
        //    get{return _nottradedatemonth;}
        //}
		/// <summary>
		/// 非交易日
		/// </summary>
        [DataMember]
		public DateTime? NotTradeDay
		{
			set{ _nottradeday=value;}
			get{return _nottradeday;}
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

