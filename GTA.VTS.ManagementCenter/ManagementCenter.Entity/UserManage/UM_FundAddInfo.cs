using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：资金明细表 实体类UM_FundAddInfo 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_FundAddInfo
	{
		public UM_FundAddInfo()
		{}
        #region Model
        private int _addfundid;
        private int? _managerid;
        private int? _userid;
        private decimal? _usnumber;
        private decimal? _rmbnumber;
        private DateTime? _addtime;
        private decimal? _hknumber;
        private string _remark;
        /// <summary>
        /// 追加资金明细标识
        /// </summary>
        [DataMember]
        public int AddFundID
        {
            set { _addfundid = value; }
            get { return _addfundid; }
        }
        /// <summary>
        /// 管理员标识
        /// </summary>
        [DataMember]
        public int? ManagerID
        {
            set { _managerid = value; }
            get { return _managerid; }
        }
        /// <summary>
        /// 交易员标识
        /// </summary>
        [DataMember]
        public int? UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 美元
        /// </summary>
        [DataMember]
        public decimal? USNumber
        {
            set { _usnumber = value; }
            get { return _usnumber; }
        }
        /// <summary>
        /// 人民币
        /// </summary>
        [DataMember]
        public decimal? RMBNumber
        {
            set { _rmbnumber = value; }
            get { return _rmbnumber; }
        }
        /// <summary>
        /// 追加时间
        /// </summary>
        [DataMember]
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 港元
        /// </summary>
        [DataMember]
        public decimal? HKNumber
        {
            set { _hknumber = value; }
            get { return _hknumber; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

	}
}

