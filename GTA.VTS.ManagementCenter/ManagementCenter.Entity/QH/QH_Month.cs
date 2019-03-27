using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：月份 实体类QH_Month 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_Month
	{
		public QH_Month()
		{}
		#region Model
		private int _monthid;
		private string _monthname;
		/// <summary>
		/// 月份标识
		/// </summary>
        [DataMember]
		public int MonthID
		{
			set{ _monthid=value;}
			get{return _monthid;}
		}
		/// <summary>
		/// 月份描述
		/// </summary>
        [DataMember]
		public string MonthName
		{
			set{ _monthname=value;}
			get{return _monthname;}
		}
		#endregion Model

	}
}

