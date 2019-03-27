using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述:代码信息表 实体类StockInfo 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：程序员：熊晓凌 修改：刘书伟
    /// 日期：2008-11-18  2009-12-01
    /// </summary>
    [DataContract]
    public class StockInfo
    {
        public StockInfo()
        { }
        #region Model
        private string _stockcode;
        private string _stockname;
        private string _paydt;
        private string _labelcommoditycode;
        private string _nindcd;

        private string _stockpinyin;
        private double _turnovervolume;
        private decimal _goerScale;
        private int _breedClassID;
        private int _codeFromSource;

        /// <summary>
        /// 股票（权证）代码
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
        /// 股票上市日期
        /// </summary>
        [DataMember]
        public string Paydt
        {
            set { _paydt = value; }
            get { return _paydt; }
        }
        /// <summary>
        /// 权证标的代码
        /// </summary>
        [DataMember]
        public string LabelCommodityCode
        {
            set { _labelcommoditycode = value; }
            get { return _labelcommoditycode; }
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
        /// 简称拼音
        /// </summary>
        [DataMember]
        public string StockPinYin
        {
            set { _stockpinyin = value; }
            get { return _stockpinyin; }
        }

        /// <summary>
        /// 流通股数/总持仓量（期货）
        /// </summary>
        [DataMember]
        public double turnovervolume
        {
            set { _turnovervolume = value; }
            get { return _turnovervolume; }
        }

        /// <summary>
        /// 行权比例
        /// </summary>
        [DataMember]
        public decimal GoerScale
        {
            set { _goerScale = value; }
            get { return _goerScale; }
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

