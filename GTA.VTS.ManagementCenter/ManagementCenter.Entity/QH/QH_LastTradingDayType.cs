using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：最后交易日类型  实体类QH_LastTradingDayType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_LastTradingDayType
	{
		public QH_LastTradingDayType()
		{}
		#region Model
		private int _lasttradingdaytypeid;
		private string _typename;
		/// <summary>
		/// 最后交易日类型标识
		/// </summary>
        [DataMember]
		public int LastTradingDayTypeID
		{
			set{ _lasttradingdaytypeid=value;}
			get{return _lasttradingdaytypeid;}
		}
		/// <summary>
		/// 类型名
		/// </summary>
        [DataMember]
		public string TypeName
		{
			set{ _typename=value;}
			get{return _typename;}
		}
		#endregion Model

	}
}

