using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：股指期货持仓限制 实体类QH_SIFPosition 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期: 2008-12-13
    /// </summary>
    [DataContract]
	public class QH_SIFPosition
	{
		public QH_SIFPosition()
		{}
		#region Model
		private int? _unilateralpositions;
		private int _breedclassid;
		/// <summary>
		/// 单边持仓量
		/// </summary>
        [DataMember]
		public int? UnilateralPositions
		{
			set{ _unilateralpositions=value;}
			get{return _unilateralpositions;}
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

