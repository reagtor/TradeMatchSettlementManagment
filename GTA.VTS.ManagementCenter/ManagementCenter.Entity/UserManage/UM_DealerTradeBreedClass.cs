using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：品种类型权限表 实体类UM_DealerTradeBreedClass 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_DealerTradeBreedClass
	{
		public UM_DealerTradeBreedClass()
		{}
		#region Model
		private int? _breedclassid;
		private int? _userid;
		private int _dealertradebreedclassid;
		/// <summary>
		/// 品种标识
		/// </summary>
        [DataMember]
		public int? BreedClassID
		{
			set{ _breedclassid=value;}
			get{return _breedclassid;}
		}
		/// <summary>
		/// 用户的ID号
		/// </summary>
        [DataMember]
		public int? UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 交易员可交易品种列表标识
		/// </summary>
        [DataMember]
		public int DealerTradeBreedClassID
		{
			set{ _dealertradebreedclassid=value;}
			get{return _dealertradebreedclassid;}
		}
		#endregion Model

	}
}

