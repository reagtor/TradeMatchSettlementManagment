using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
	/// 描述：撮合机表 实体类RC_MatchMachine 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class RC_MatchMachine
	{
		public RC_MatchMachine()
		{}
		#region Model
		private int _matchmachineid;
		private string _matchmachinename;
		private int? _boursetypeid;
		private int? _matchcenterid;
		/// <summary>
		/// 撮合机编号
		/// </summary>
        [DataMember]
		public int MatchMachineID
		{
			set{ _matchmachineid=value;}
			get{return _matchmachineid;}
		}
		/// <summary>
		/// 撮合机名称
		/// </summary>
        [DataMember]
		public string MatchMachineName
		{
			set{ _matchmachinename=value;}
			get{return _matchmachinename;}
		}
		/// <summary>
		/// 交易所类型标识
		/// </summary>
        [DataMember]
		public int? BourseTypeID
		{
			set{ _boursetypeid=value;}
			get{return _boursetypeid;}
		}
		/// <summary>
		/// 撮合中心编号
		/// </summary>
        [DataMember]
		public int? MatchCenterID
		{
			set{ _matchcenterid=value;}
			get{return _matchcenterid;}
		}
		#endregion Model

	}
}

