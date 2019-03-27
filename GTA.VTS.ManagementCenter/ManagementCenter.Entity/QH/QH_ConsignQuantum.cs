using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易规则委托量 实体类QH_ConsignQuantum 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_ConsignQuantum
	{
		public QH_ConsignQuantum()
		{}
		#region Model
		private int _consignquantumid;
		private int? _minconsignquantum;
		/// <summary>
		/// 交易规则委托量标识
		/// </summary>
        [DataMember]
		public int ConsignQuantumID
		{
			set{ _consignquantumid=value;}
			get{return _consignquantumid;}
		}
		/// <summary>
		/// 最小委托量
		/// </summary>
        [DataMember]
		public int? MinConsignQuantum
		{
			set{ _minconsignquantum=value;}
			get{return _minconsignquantum;}
		}
		#endregion Model

	}
}

