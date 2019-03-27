using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述:行业表 实体类Profession 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
	/// </summary>
    [DataContract]
	public class Profession
	{
		public Profession()
		{}
		#region Model
		private string _nindcd;
		private string _nindnme;
		private string _ennindnme;
		/// <summary>
		/// 行业标识
		/// </summary>
        [DataMember]
		public string Nindcd
		{
			set{ _nindcd=value;}
			get{return _nindcd;}
		}
		/// <summary>
		/// 行业中文描述
		/// </summary>
        [DataMember]
		public string Nindnme
		{
			set{ _nindnme=value;}
			get{return _nindnme;}
		}
		/// <summary>
		/// 行业英文描述
		/// </summary>
        [DataMember]
		public string EnNindnme
		{
			set{ _ennindnme=value;}
			get{return _ennindnme;}
		}
		#endregion Model

	}
}

