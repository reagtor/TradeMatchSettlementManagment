using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：交易规则_最小变动价位,品种_现货_交易费用的范围_值 实体类XHFieldRangeEntity 
    ///作者：刘书伟
    ///日期:2008-12-23
    /// </summary>
    [DataContract]
    public class XHFieldRangeEntity
    {
        #region Model
        //交易规则_最小变动价位_范围_值,品种_现货_交易费用表中的字段
        private decimal? _value;
        private int _breedclassid;
        //字段范围表中的字段
        private int _fieldrangeid;
        private decimal? _upperlimit;
        private decimal? _lowerlimit;
        private int _upperlimitifequation;
        private int _lowerlimitifequation;
        private string _propertymarket;

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public decimal? Value
        {
            set { _value = value; }
            get { return _value; }
        }

        /// <summary>
        /// 品种标识
        /// </summary>
        [DataMember]
        public int BreedClassID
        {
            set { _breedclassid = value; }
            get { return _breedclassid; }
        }

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
        /// 上限是否可相等
        /// </summary>
        [DataMember]
        public int UpperLimitIfEquation
        {
            set { _upperlimitifequation = value; }
            get { return _upperlimitifequation; }
        }

        /// <summary>
        /// 下限是否可相等
        /// </summary>
        [DataMember]
        public int LowerLimitIfEquation
        {
            set { _lowerlimitifequation = value; }
            get { return _lowerlimitifequation; }
        }

        /// <summary>
        /// 行情属性
        /// </summary>
        [DataMember]
        public string PropertyMarket
        {
            set { _propertymarket = value; }
            get { return _propertymarket; }
        }

        #endregion Model
    }
}