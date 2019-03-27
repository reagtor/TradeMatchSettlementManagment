using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    ///描述：现货_品种_涨跌幅_控制类型 实体类XH_SpotHighLowControlType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
	public class XH_SpotHighLowControlType
	{
        public XH_SpotHighLowControlType()
        { }
        #region Model
        private int _highlowtypeid;
        private int _breedclasshighlowid;
        /// <summary>
        /// 涨跌幅类型标识
        /// </summary>
        [DataMember]
        public int HighLowTypeID
        {
            set { _highlowtypeid = value; }
            get { return _highlowtypeid; }
        }
        /// <summary>
        /// 品种涨跌幅标识
        /// </summary>
        [DataMember]
        public int BreedClassHighLowID
        {
            set { _breedclasshighlowid = value; }
            get { return _breedclasshighlowid; }
        }
        #endregion Model

	}
}

