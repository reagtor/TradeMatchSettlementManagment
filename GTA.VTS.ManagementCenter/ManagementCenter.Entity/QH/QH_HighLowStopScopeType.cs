using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：涨跌停板幅度类型 实体类QH_HighLowStopScopeType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_HighLowStopScopeType
	{
		public QH_HighLowStopScopeType()
		{}
		#region Model
		private int _highlowstopscopeid;
		private string _highlowstopscopename;
		/// <summary>
		/// 涨跌停板幅度类型标识
		/// </summary>
        [DataMember]
		public int HighLowStopScopeID
		{
			set{ _highlowstopscopeid=value;}
			get{return _highlowstopscopeid;}
		}
		/// <summary>
		/// 涨跌停板幅度类型描述
		/// </summary>
        [DataMember]
		public string HighLowStopScopeName
		{
			set{ _highlowstopscopename=value;}
			get{return _highlowstopscopename;}
		}
		#endregion Model

	}
}

