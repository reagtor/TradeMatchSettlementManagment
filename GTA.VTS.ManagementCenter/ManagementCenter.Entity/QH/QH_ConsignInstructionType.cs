using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：委托指令类型 实体类QH_ConsignInstructionType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_ConsignInstructionType
	{
		public QH_ConsignInstructionType()
		{}
		#region Model
		private int _consigninstructiontypeid;
		private string _typename;
		/// <summary>
		/// 委托指令类型标识
		/// </summary>
        [DataMember] 
		public int ConsignInstructionTypeID
		{
			set{ _consigninstructiontypeid=value;}
			get{return _consigninstructiontypeid;}
		}
		/// <summary>
		/// 类型名称
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

