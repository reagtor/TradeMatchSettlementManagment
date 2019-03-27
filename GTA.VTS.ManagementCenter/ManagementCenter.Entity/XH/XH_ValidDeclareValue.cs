using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：有效申报取值 实体类XH_ValidDeclareValue 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-12-2  修改日期：2009-7-23
    /// </summary>
    [DataContract]
	public class XH_ValidDeclareValue
	{
		public XH_ValidDeclareValue()
		{}
		#region Model
		private int _validdeclarevalueid;
		private decimal? _upperlimit;
		private decimal? _lowerlimit;
        //private int? _ismarketnewday;
		private int? _breedclassvalidid;
	    private decimal? _newDayUpperLimit;
	    private decimal? _newDayLowerLimit;
		/// <summary>
		/// 有效申报取值标识
		/// </summary>
        [DataMember]
		public int ValidDeclareValueID
		{
			set{ _validdeclarevalueid=value;}
			get{return _validdeclarevalueid;}
		}
		/// <summary>
		/// 上限
		/// </summary>
        [DataMember]
		public decimal? UpperLimit
		{
			set{ _upperlimit=value;}
			get{return _upperlimit;}
		}
		/// <summary>
		/// 下限
		/// </summary>
        [DataMember]
		public decimal? LowerLimit
		{
			set{ _lowerlimit=value;}
			get{return _lowerlimit;}
		}
        ///// <summary>
        ///// 是否是上市首日
        ///// </summary>
        //[DataMember]
        //public int? IsMarketNewDay
        //{
        //    set{ _ismarketnewday=value;}
        //    get{return _ismarketnewday;}
        //}
        /// <summary>
        /// 上市首日上限
        /// </summary>
        [DataMember]
        public decimal? NewDayUpperLimit
        {
            set { _newDayUpperLimit = value; }
            get { return _newDayUpperLimit; }
        }
        /// <summary>
        /// 上市首日下限
        /// </summary>
        [DataMember]
        public decimal? NewDayLowerLimit
        {
            set { _newDayLowerLimit = value; }
            get { return _newDayLowerLimit; }
        }
		/// <summary>
		/// 品种有效申报标识
		/// </summary>
        [DataMember]
		public int? BreedClassValidID
		{
			set{ _breedclassvalidid=value;}
			get{return _breedclassvalidid;}
		}
		#endregion Model

	}
}

