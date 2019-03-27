using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：期货_品种_交割月份 实体类QH_CFPositionMonth 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_CFPositionMonth
	{
		public QH_CFPositionMonth()
		{}
		#region Model
		private int _deliverymonthtypeid;
		private string _deliverymonthname;
		/// <summary>
		/// 交割月份类型标识
		/// </summary>
        [DataMember]
		public int DeliveryMonthTypeID
		{
			set{ _deliverymonthtypeid=value;}
			get{return _deliverymonthtypeid;}
		}
		/// <summary>
		/// 交割月份名称
		/// </summary>
        [DataMember]
		public string DeliveryMonthName
		{
			set{ _deliverymonthname=value;}
			get{return _deliverymonthname;}
		}
		#endregion Model

	}
}

