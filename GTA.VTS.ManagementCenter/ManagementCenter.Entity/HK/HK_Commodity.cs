using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：港股交易商品表 实体类HK_Commodity 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-10-21
    /// </summary>
    [DataContract]
    public class HK_Commodity
    {
        public HK_Commodity()
        { }
        #region Model
        private string _hkcommoditycode;
        private string _hkcommodityname;
        private int? _breedclassid;
        private DateTime? _marketdate;
        private string _stockpinyin;
        private double? _turnovervolume;
        private int? _perhandthighorshare;
        private int? _issellnull;
        private int _iSSysDefaultCode;
        /// <summary>
        /// 港股商品代码
        /// </summary>
        [DataMember]
        public string HKCommodityCode
        {
            set { _hkcommoditycode = value; }
            get { return _hkcommoditycode; }
        }
        /// <summary>
        /// 港股商品名称
        /// </summary>
        [DataMember]
        public string HKCommodityName
        {
            set { _hkcommodityname = value; }
            get { return _hkcommodityname; }
        }

        /// <summary>
        /// 品种标识
        /// </summary>
        [DataMember]
        public int? BreedClassID
        {
            set { _breedclassid = value; }
            get { return _breedclassid; }
        }

        /// <summary>
        /// 上市日期
        /// </summary>
        [DataMember]
        public DateTime? MarketDate
        {
            set { _marketdate = value; }
            get { return _marketdate; }
        }
        /// <summary>
        /// 股票简称的拼音
        /// </summary>
        [DataMember]
        public string StockPinYin
        {
            set { _stockpinyin = value; }
            get { return _stockpinyin; }
        }
        /// <summary>
        /// 流通股数
        /// </summary>
        [DataMember]
        public double? turnovervolume
        {
            set { _turnovervolume = value; }
            get { return _turnovervolume; }
        }
        /// <summary>
        /// 每手股份数
        /// </summary>
        [DataMember]
        public int? PerHandThighOrShare
        {
            set { _perhandthighorshare = value; }
            get { return _perhandthighorshare; }
        }
        /// <summary>
        /// 是否卖空-本版本中没数据
        /// </summary>
        [DataMember]
        public int? IsSellNull
        {
            set { _issellnull = value; }
            get { return _issellnull; }
        }
       
        /// <summary>
        /// 是否是系统默认代码
        /// </summary>
        [DataMember]
        public int ISSysDefaultCode
        {
            set { _iSSysDefaultCode = value; }
            get { return _iSSysDefaultCode; }
        }
        #endregion Model

    }
}
