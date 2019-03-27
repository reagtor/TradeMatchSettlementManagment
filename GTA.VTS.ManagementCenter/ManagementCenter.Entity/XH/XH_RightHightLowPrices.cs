using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：权证涨跌幅价格 实体类XH_RightHightLowPrices 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：刘书伟
    /// 日期：2008-11-26
    /// </summary>
    [DataContract]
    public class XH_RightHightLowPrices
    {
        public XH_RightHightLowPrices()
        {
        }

        #region Model

        private int _righthightlowpriceid;
        private decimal? _rightfrontdaycloseprice;
        private decimal? _stockfrontdaycloseprice;
        private decimal? _setscale;
        private int? _hightlowid;

        /// <summary>
        /// 权证涨跌幅价格标识
        /// </summary>
        [DataMember]
        public int RightHightLowPriceID
        {
            set { _righthightlowpriceid = value; }
            get { return _righthightlowpriceid; }
        }

        /// <summary>
        /// 权证前一日收盘价格
        /// </summary>
        [DataMember]
        public decimal? RightFrontDayClosePrice
        {
            set { _rightfrontdaycloseprice = value; }
            get { return _rightfrontdaycloseprice; }
        }

        /// <summary>
        /// 标的证券前日收盘价
        /// </summary>
        [DataMember]
        public decimal? StockFrontDayClosePrice
        {
            set { _stockfrontdaycloseprice = value; }
            get { return _stockfrontdaycloseprice; }
        }

        /// <summary>
        /// 设置比例
        /// </summary>
        [DataMember]
        public decimal? SetScale
        {
            set { _setscale = value; }
            get { return _setscale; }
        }

        /// <summary>
        /// 涨跌幅标识
        /// </summary>
        [DataMember]
        public int? HightLowID
        {
            set { _hightlowid = value; }
            get { return _hightlowid; }
        }

        #endregion Model
    }
}