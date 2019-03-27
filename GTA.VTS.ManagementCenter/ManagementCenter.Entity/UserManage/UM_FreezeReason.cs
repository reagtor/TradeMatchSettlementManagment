using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：资金冻结表 实体类UM_FreezeReason 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_FreezeReason
	{
		public UM_FreezeReason()
		{}
		#region Model
		private int _freezereasonid;
		private string _freezereason;
		private DateTime? _freezereasontime;
		private DateTime? _thawreasontime;
		private string _dealeraccoutid;
		private int? _isauto;
		/// <summary>
		/// 冻结原因标识
		/// </summary>
        [DataMember]
		public int FreezeReasonID
		{
			set{ _freezereasonid=value;}
			get{return _freezereasonid;}
		}
		/// <summary>
		/// 冻结原因
		/// </summary>
        [DataMember]
		public string FreezeReason
		{
			set{ _freezereason=value;}
			get{return _freezereason;}
		}
		/// <summary>
		/// 冻结时间
		/// </summary>
        [DataMember]
		public DateTime? FreezeReasonTime
		{
			set{ _freezereasontime=value;}
			get{return _freezereasontime;}
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
		/// <summary>
		/// 是否自动解冻
		/// </summary>
        [DataMember]
		public int? IsAuto
		{
			set{ _isauto=value;}
			get{return _isauto;}
		}
		#endregion Model

	}
}

