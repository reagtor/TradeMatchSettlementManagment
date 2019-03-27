using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：合约交割月份 实体类QH_AgreementDeliveryMonth 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-11-21
    /// </summary>
    [DataContract]
	public class QH_AgreementDeliveryMonth
	{
		public QH_AgreementDeliveryMonth()
		{}
		#region Model
		private int _agreementdeliverymonthid;
		private int? _monthid;
		private int? _breedclassid;
		/// <summary>
		/// 合约交割月份标识
		/// </summary>
        [DataMember]
		public int AgreementDeliveryMonthID
		{
			set{ _agreementdeliverymonthid=value;}
			get{return _agreementdeliverymonthid;}
		}
		/// <summary>
		/// 月份标识
		/// </summary>
        [DataMember]
		public int? MonthID
		{
			set{ _monthid=value;}
			get{return _monthid;}
		}
		/// <summary>
		/// 品种标识
		/// </summary>
        [DataMember]
		public int? BreedClassID
		{
			set{ _breedclassid=value;}
			get{return _breedclassid;}
		}
		#endregion Model

	}
}

