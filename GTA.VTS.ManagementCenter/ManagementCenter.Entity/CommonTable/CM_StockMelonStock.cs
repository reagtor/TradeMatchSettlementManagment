using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：股票分红记录_股票表 实体类CM_StockMelonStock 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class CM_StockMelonStock
	{
		public CM_StockMelonStock()
		{}
		#region Model
		private int _stockmelonstockid;
		private DateTime? _stockrightregisterdate;
		private DateTime? _stockrightlogoutdatumdate;
        private double? _sentstockratio;
		private string _commoditycode;
		/// <summary>
		/// 股票分红记录_股票标识
		/// </summary>
        [DataMember]
		public int StockMelonStockID
		{
			set{ _stockmelonstockid=value;}
			get{return _stockmelonstockid;}
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
		/// 送股比例
		/// </summary>
        [DataMember]
        public double? SentStockRatio
		{
			set{ _sentstockratio=value;}
			get{return _sentstockratio;}
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
		#endregion Model

	}
}

