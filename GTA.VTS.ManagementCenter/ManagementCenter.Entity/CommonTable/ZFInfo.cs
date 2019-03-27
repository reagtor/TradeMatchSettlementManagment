using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述: 实体类ZFInfo 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class ZFInfo
	{
		public ZFInfo()
		{}
		#region Model
		private string _stkcd;
		private double? _roprc;
        private double? _zfgs;
		private string _paydt;
		/// <summary>
		/// 股票代码
		/// </summary>
        [DataMember]
		public string stkcd
		{
			set{ _stkcd=value;}
			get{return _stkcd;}
		}
		/// <summary>
		/// 增发比例
		/// </summary>
        [DataMember]
        public double? roprc
		{
			set{ _roprc=value;}
			get{return _roprc;}
		}
		/// <summary>
		/// 增发数量
		/// </summary>
        [DataMember]
        public double? zfgs
		{
			set{ _zfgs=value;}
			get{return _zfgs;}
		}
		/// <summary>
		/// 增发日期
		/// </summary>
        [DataMember]
		public string paydt
		{
			set{ _paydt=value;}
			get{return _paydt;}
		}
		#endregion Model

	}
}

