using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：商品期货_保证金比例 实体类QH_CFBailScaleValue 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    [DataContract]
    public class QH_CFBailScaleValue
    {
        public QH_CFBailScaleValue()
        { }
        #region Model
        private int _cfbailscalevalueid;
        private int? _start;
        private decimal? _bailscale;
        private int? _ends;
        private int? _breedclassid;
        private int? _deliverymonthtype;
        private int? _upperlimitifequation;
        private int? _lowerlimitifequation;
        private int? _positionbailtypeid;
        /// <summary>
        /// 商品期货-保证金比例标识
        /// </summary>
        [DataMember]
        public int CFBailScaleValueID
        {
            set { _cfbailscalevalueid = value; }
            get { return _cfbailscalevalueid; }
        }
        /// <summary>
        /// 开始
        /// </summary>
        [DataMember]
        public int? Start
        {
            set { _start = value; }
            get { return _start; }
        }
        /// <summary>
        /// 保证金比例
        /// </summary>
        [DataMember]
        public decimal? BailScale
        {
            set { _bailscale = value; }
            get { return _bailscale; }
        }
        /// <summary>
        /// 结束
        /// </summary>
        [DataMember]
        public int? Ends
        {
            set { _ends = value; }
            get { return _ends; }
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
        /// 交割月份类型标识
        /// </summary>
        [DataMember]
        public int? DeliveryMonthType
        {
            set { _deliverymonthtype = value; }
            get { return _deliverymonthtype; }
        }
        /// <summary>
        /// 上限是否可相等
        /// </summary>
        [DataMember]
        public int? UpperLimitIfEquation
        {
            set { _upperlimitifequation = value; }
            get { return _upperlimitifequation; }
        }
        /// <summary>
        /// 下限是否可相等
        /// </summary>
        [DataMember]
        public int? LowerLimitIfEquation
        {
            set { _lowerlimitifequation = value; }
            get { return _lowerlimitifequation; }
        }
        /// <summary>
        /// 持仓和保证金控制类型标识
        /// </summary>
        [DataMember]
        public int? PositionBailTypeID
        {
            set { _positionbailtypeid = value; }
            get { return _positionbailtypeid; }
        }

        /// <summary>
        /// Title:关联保证金记录ID
        /// Desc: 用于记录关联的记录ID，该记录为最后交易日前n天起的保证金比例。
        /// Create by: 董鹏
        /// Create Date:2010-02-01
        /// </summary>
        [DataMember]
        public int? RelationScaleID
        {
            get;
            set;
        }

        #endregion Model

    }
}
