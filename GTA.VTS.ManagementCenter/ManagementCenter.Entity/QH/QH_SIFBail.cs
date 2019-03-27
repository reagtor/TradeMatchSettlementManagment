using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：品种_股指期货_保证金 实体类QH_SIFBail 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_SIFBail
	{
		public QH_SIFBail()
		{}
		#region Model
		private decimal _bailscale;
		private int _breedclassid;
		/// <summary>
		/// 保证金比例
		/// </summary>
        [DataMember]
		public decimal BailScale
		{
			set{ _bailscale=value;}
			get{return _bailscale;}
		}
		/// <summary>
		/// 品种标识
		/// </summary>
        [DataMember]
		public int BreedClassID
		{
			set{ _breedclassid=value;}
			get{return _breedclassid;}
		}
		#endregion Model

	}
}

