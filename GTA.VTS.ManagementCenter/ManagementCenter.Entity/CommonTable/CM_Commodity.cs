using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述:交易商品表 实体类CM_Commodity 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:修改日期：2009-7-22
    /// </summary>
    [DataContract]
    public class CM_Commodity
    {
        public CM_Commodity()
        {
        }

        #region Model

        private string _commoditycode;
        private string _commodityname;
        private int? _breedclassid;
        private string _labelcommoditycode;
        private decimal _goerScale;
        private DateTime _marketdate;
        private string _stockpinyin;
        private double? _turnovervolume;
        private int _isExpired;
        private int _iSSysDefaultCode;
        /// <summary>
        /// 商品代码
        /// </summary>
        [DataMember]
        public string CommodityCode
        {
            set { _commoditycode = value; }
            get { return _commoditycode; }
        }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string CommodityName
        {
            set { _commodityname = value; }
            get { return _commodityname; }
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
        /// 标的商品代码
        /// </summary>
        [DataMember]
        public string LabelCommodityCode
        {
            set { _labelcommoditycode = value; }
            get { return _labelcommoditycode; }
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
        /// 上市时间
        /// </summary>
        [DataMember]
        public DateTime MarketDate
        {
            set { _marketdate = value; }
            get { return _marketdate; }
        }

        /// <summary>
        /// 股票权证拼音简称
        /// </summary>
        [DataMember]
        public string StockPinYin
        {
            set { _stockpinyin = value; }
            get { return _stockpinyin; }
        }

        /// <summary>
        /// 流通股数，总持仓量
        /// </summary>
        [DataMember]
        public double? turnovervolume
        {
            set { _turnovervolume = value; }
            get { return _turnovervolume; }
        }

        /// <summary>
        /// 期货代码是否过期
        /// </summary>
        [DataMember]
        public int IsExpired
        {
            set { _isExpired = value; }
            get { return _isExpired; }
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