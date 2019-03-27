using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：持仓和保证金控制类型 实体类QH_PositionBailType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    [DataContract]
	public class QH_PositionBailType
	{
		public QH_PositionBailType()
		{}
		#region Model
		private int _positionbailtypeid;
		private string _positionbailtypename;
		/// <summary>
		/// 持仓和保证金控制类型标识
		/// </summary>
        [DataMember]
		public int PositionBailTypeID
		{
			set{ _positionbailtypeid=value;}
			get{return _positionbailtypeid;}
		}
		/// <summary>
		/// 持仓和保证金类型名称
		/// </summary>
        [DataMember]
		public string PositionBailTypeName
		{
			set{ _positionbailtypename=value;}
			get{return _positionbailtypename;}
		}
		#endregion Model

	}
}

