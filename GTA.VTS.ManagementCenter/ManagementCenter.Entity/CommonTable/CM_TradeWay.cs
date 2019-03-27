using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易方向表 实体类CM_TradeWay 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_TradeWay
	{
		public CM_TradeWay()
		{}
		#region Model
		private int _tradewayid;
		private string _tradewayname;
		/// <summary>
		/// 交易方向标识
		/// </summary>
        [DataMember]
		public int TradeWayID
		{
			set{ _tradewayid=value;}
			get{return _tradewayid;}
		}
		/// <summary>
		/// 交易方向名称
		/// </summary>
        [DataMember]
		public string TradeWayName
		{
			set{ _tradewayname=value;}
			get{return _tradewayname;}
		}
		#endregion Model

	}
}

