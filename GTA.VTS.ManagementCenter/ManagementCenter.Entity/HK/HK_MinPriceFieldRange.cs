using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：港股交易规则_最小变动价位范围值表 实体类HK_MinPriceFieldRange 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：刘书伟
    /// 日期:2009-10-21
    /// </summary>
    [DataContract]
    public class HK_MinPriceFieldRange
    {
        public HK_MinPriceFieldRange()
        { }
        #region Model
        private int _fieldrangeid;
        private decimal? _upperlimit;
        private decimal? _lowerlimit;
        private decimal? _value;
        /// <summary>
        /// 字段_范围标识
        /// </summary>
        [DataMember]
        public int FieldRangeID
        {
            set { _fieldrangeid = value; }
            get { return _fieldrangeid; }
        }
        /// <summary>
        /// 上限
        /// </summary>
        [DataMember]
        public decimal? UpperLimit
        {
            set { _upperlimit = value; }
            get { return _upperlimit; }
        }
        /// <summary>
        /// 下限
        /// </summary>
        [DataMember]
        public decimal? LowerLimit
        {
            set { _lowerlimit = value; }
            get { return _lowerlimit; }
        }
        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public decimal? Value
        {
            set { _value = value; }
            get { return _value; }
        }
        #endregion Model

    }
}
