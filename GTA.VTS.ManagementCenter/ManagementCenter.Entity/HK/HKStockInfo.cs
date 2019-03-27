using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：港股股票代码表 实体类HKStockInfo 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：刘书伟
    /// 日期:2009-10-21
    /// </summary>
    [DataContract]
    public class HKStockInfo
    {
        public HKStockInfo()
        { }
        #region Model
        private string _stockcode;
        private string _stockname;
        private string _stockpinyin;
        private double? _turnovervolume;
        private string _paydt;
        private string _nindcd;
        private int? _perhandthighorshare;
        private int? _issellnull;
        private int _breedClassID;
        private int _codeFromSource;

        /// <summary>
        /// 股票代码
        /// </summary>
        [DataMember]
        public string StockCode
        {
            set { _stockcode = value; }
            get { return _stockcode; }
        }
        /// <summary>
        /// 股票名称
        /// </summary>
        [DataMember]
        public string StockName
        {
            set { _stockname = value; }
            get { return _stockname; }
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
        /// 股票上市日期
        /// </summary>
        [DataMember]
        public string Paydt
        {
            set { _paydt = value; }
            get { return _paydt; }
        }
        /// <summary>
        /// 行业标识
        /// </summary>
        [DataMember]
        public string Nindcd
        {
            set { _nindcd = value; }
            get { return _nindcd; }
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
        /// 是否卖空
        /// </summary>
        [DataMember]
        public int? IsSellNull
        {
            set { _issellnull = value; }
            get { return _issellnull; }
        }

        /// <summary>
        /// 品种标识
        /// </summary>
        [DataMember]
        public int BreedClassID
        {
            set { _breedClassID = value; }
            get { return _breedClassID; }
        }

        /// <summary>
        /// 代码来源哪个表（仅在分配撮合机代码时用）
        /// </summary>
        [DataMember]
        public int CodeFromSource
        {
            set { _codeFromSource = value; }
            get { return _codeFromSource; }
        }
        #endregion Model

    }
}
