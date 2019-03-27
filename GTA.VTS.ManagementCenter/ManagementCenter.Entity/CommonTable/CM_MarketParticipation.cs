using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：市场参与度表 实体类CM_MarketParticipation 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_MarketParticipation
	{
		public CM_MarketParticipation()
		{}
		#region Model
		private float _participation;
		private int _breedclassid;
		/// <summary>
		/// 参与度
		/// </summary>
        [DataMember]
		public float Participation
		{
			set{ _participation=value;}
			get{return _participation;}
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

