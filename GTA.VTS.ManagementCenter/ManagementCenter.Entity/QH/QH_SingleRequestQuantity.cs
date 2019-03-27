using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：单笔委托量  实体类QH_SingleRequestQuantity 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_SingleRequestQuantity
	{
		public QH_SingleRequestQuantity()
		{}
		#region Model
		private int _singlerequestquantityid;
		private int? _MaxConsignQuanturm;
		private int? _consignquantumid;
		private int? _consigninstructiontypeid;
		/// <summary>
		/// 单笔委托量标识
		/// </summary>
        [DataMember]
		public int SingleRequestQuantityID
		{
			set{ _singlerequestquantityid=value;}
			get{return _singlerequestquantityid;}
		}
		/// <summary>
		/// 委托量
		/// </summary>
        [DataMember]
		public int? MaxConsignQuanturm
		{
			set{ _MaxConsignQuanturm=value;}
			get{return _MaxConsignQuanturm;}
		}
		/// <summary>
		/// 交易规则委托量标识
		/// </summary>
        [DataMember]
		public int? ConsignQuantumID
		{
			set{ _consignquantumid=value;}
			get{return _consignquantumid;}
		}
		/// <summary>
		/// 委托指令类型标识
		/// </summary>
        [DataMember]
		public int? ConsignInstructionTypeID
		{
			set{ _consigninstructiontypeid=value;}
			get{return _consigninstructiontypeid;}
		}
		#endregion Model

	}
}

