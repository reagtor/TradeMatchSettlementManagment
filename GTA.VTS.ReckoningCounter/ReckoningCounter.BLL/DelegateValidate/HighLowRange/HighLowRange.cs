using GTA.VTS.Common.CommonObject;
using System.Runtime.Serialization;

namespace ReckoningCounter.BLL.DelegateValidate
{
    /// <summary>
    /// 涨跌幅对象（代表上下限比例）
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    public class HighLowRange
    {
        /// <summary>
        /// 上限
        /// </summary>
        public decimal HighRange;

        /// <summary>
        /// 下限
        /// </summary>
        public decimal LowRange;

        /// <summary>
        /// 港股涨跌幅对象，只有在RangeType=HongKongPrice时才有效
        /// </summary>
        public HKRange HongKongRange;

        /// <summary>
        /// 涨跌幅的类型
        /// </summary>
        public Types.HighLowRangeType RangeType;
    }

    /// <summary>
    /// 涨跌幅值对象（代表上下限的实际值）
    /// </summary>
    [DataContract]
    public class HighLowRangeValue
    {
        /// <summary>
        /// 上限实际值
        /// </summary>
        [DataMember]
        public decimal HighRangeValue;

        /// <summary>
        /// 下限实际值
        /// </summary>
        [DataMember]
        public decimal LowRangeValue;

        /// <summary>
        /// 港股涨跌幅值对象，只有在RangeType=HongKongPrice时才有效
        /// </summary>
        [DataMember]
        public HKRangeValue HongKongRangeValue;

        /// <summary>
        /// 涨跌幅的类型
        /// </summary>
        [DataMember]
        public Types.HighLowRangeType RangeType;
    }

    /// <summary>
    /// 港股涨跌幅对象
    /// </summary>
    [DataContract]
    public class HKRange
    {
        /// <summary>
        /// 买方向上限
        /// </summary>
        [DataMember]
        public decimal BuyHighRange;

        /// <summary>
        /// 买方向下限
        /// </summary>
        [DataMember]
        public decimal BuyLowRange;

        /// <summary>
        /// 卖方向上限
        /// </summary>
        [DataMember]
        public decimal SellHighRange;

        /// <summary>
        /// 卖方向下限
        /// </summary>
        [DataMember]
        public decimal SellLowRange;
    }

    /// <summary>
    /// 港股涨跌幅值对象
    /// </summary>
    public class HKRangeValue
    {
        /// <summary>
        /// 买方向上限实际值
        /// </summary>
        [DataMember]
        public decimal BuyHighRangeValue;

        /// <summary>
        /// 买方向下限实际值
        /// </summary>
        [DataMember]
        public decimal BuyLowRangeValue;

        /// <summary>
        /// 卖方向上限实际值
        /// </summary>
        [DataMember]
        public decimal SellHighRangeValue;

        /// <summary>
        /// 卖方向下限实际值
        /// </summary>
        [DataMember]
        public decimal SellLowRangeValue;
        /// <summary>
        /// //港股有效报价类型
        /// </summary>
        [DataMember]
        public Types.HKValidPriceType HKValidPriceType;
        
    }

    
}