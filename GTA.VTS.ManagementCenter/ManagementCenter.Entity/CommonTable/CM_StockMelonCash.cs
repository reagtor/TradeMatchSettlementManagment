using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：股票分红记录_现金表 实体类CM_StockMelonCash 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_StockMelonCash
	{
		public CM_StockMelonCash()
		{}
		#region Model
		private int _stockmeloncuttingcashid;
		private DateTime? _stockrightregisterdate;
		private DateTime? _stockrightlogoutdatumdate;
		private string _commoditycode;
		private double? _ratioofcashdividend;
		/// <summary>
		/// 股票分红记录_现金标识
		/// </summary>
        [DataMember]
		public int StockMelonCuttingCashID
		{
			set{ _stockmeloncuttingcashid=value;}
			get{return _stockmeloncuttingcashid;}
		}
		/// <summary>
		/// 股权登记日
		/// </summary>
        [DataMember]
		public DateTime? StockRightRegisterDate
		{
			set{ _stockrightregisterdate=value;}
			get{return _stockrightregisterdate;}
		}
		/// <summary>
		/// 除权基准日
		/// </summary>
        [DataMember]
		public DateTime? StockRightLogoutDatumDate
		{
			set{ _stockrightlogoutdatumdate=value;}
			get{return _stockrightlogoutdatumdate;}
		}
		
		/// <summary>
		/// 商品代码
		/// </summary>
        [DataMember]
		public string CommodityCode
		{
			set{ _commoditycode=value;}
			get{return _commoditycode;}
		}
		/// <summary>
		/// 现金分红比例
		/// </summary>
        [DataMember]
        public double? RatioOfCashDividend
		{
			set{ _ratioofcashdividend=value;}
			get{return _ratioofcashdividend;}
		}
		#endregion Model

	}
}

