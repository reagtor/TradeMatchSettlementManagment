using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：交易货币类型表 实体类CM_CurrencyType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_CurrencyType
	{
		public CM_CurrencyType()
		{}
		#region Model
		private int _currencytypeid;
		private string _currencyname;
		/// <summary>
		/// 交易货币类型标识
		/// </summary>
        [DataMember]
		public int CurrencyTypeID
		{
			set{ _currencytypeid=value;}
			get{return _currencytypeid;}
		}
		/// <summary>
		/// 货币名称
		/// </summary>
        [DataMember]
		public string CurrencyName
		{
			set{ _currencyname=value;}
			get{return _currencyname;}
		}
		#endregion Model

	}


    [DataContract]
    public class CM_CurrencyBreedClassType
    {
        private int? _currencytypeid;
        private int? _breedclassid;

        /// <summary>
        /// 交易货币类型标识
        /// </summary>
        [DataMember]
        public int? CurrencyTypeID
        {
            set { _currencytypeid = value; }
            get { return _currencytypeid; }
        }

        /// <summary>
        /// 交易货币类型标识
        /// </summary>
        [DataMember]
        public int? BreedClassID
        {
            set { _breedclassid = value; }
            get { return _breedclassid; }
        }

    }
}

