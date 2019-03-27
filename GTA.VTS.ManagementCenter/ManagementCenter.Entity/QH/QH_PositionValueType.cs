using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：商品期货_持仓取值类型 实体类QH_PositionValueType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    [DataContract]
    public class QH_PositionValueType
    {
        public QH_PositionValueType()
        {
        }

        #region Model

        private int _positionvaluetypeid;
        private string _positionvaluename;

        /// <summary>
        /// 商品期货_持仓取值标识
        /// </summary>
        [DataMember]
        public int PositionValueTypeID
        {
            set { _positionvaluetypeid = value; }
            get { return _positionvaluetypeid; }
        }

        /// <summary>
        /// 取值类型名称
        /// </summary>
        [DataMember]
        public string PositionValueName
        {
            set { _positionvaluename = value; }
            get { return _positionvaluename; }
        }

        #endregion Model
    }
}