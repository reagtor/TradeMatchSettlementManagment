using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：解冻原因表 实体类UM_ThawReason 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
	/// </summary>
    [DataContract]
	public class UM_ThawReason
	{
		public UM_ThawReason()
		{}
		#region Model
		private int _thawreasonid;
		private string _thawreason;
		private DateTime? _thawreasontime;
		private string _dealeraccoutid;
		/// <summary>
		/// 解冻原因标识
		/// </summary>
        [DataMember]
		public int ThawReasonID
		{
			set{ _thawreasonid=value;}
			get{return _thawreasonid;}
		}
		/// <summary>
		/// 解冻原因
		/// </summary>
        [DataMember]
		public string ThawReason
		{
			set{ _thawreason=value;}
			get{return _thawreason;}
		}
		/// <summary>
		/// 解冻时间
		/// </summary>
        [DataMember]
		public DateTime? ThawReasonTime
		{
			set{ _thawreasontime=value;}
			get{return _thawreasontime;}
		}
		/// <summary>
		/// 交易员账号分配表标识
		/// </summary>
        [DataMember]
		public string DealerAccoutID
		{
			set{ _dealeraccoutid=value;}
			get{return _dealeraccoutid;}
		}
		#endregion Model

	}
}

