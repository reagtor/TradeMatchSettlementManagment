using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易规则_取值类型表 实体类CM_ValueType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_ValueType
	{
		public CM_ValueType()
		{}
		#region Model
		private string _name;
		private int _valuetypeid;
		/// <summary>
		/// 名称
		/// </summary>
        [DataMember]
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 交易规则_取值类型标识
		/// </summary>
		[DataMember]
		public int ValueTypeID
		{
			set{ _valuetypeid=value;}
			get{return _valuetypeid;}
		}
		#endregion Model

	}
}

