using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述：港股行业表 实体类HKProfessionInfo 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：刘书伟
    /// 日期:2009-10-21
    /// </summary>
    [DataContract]
    public class HKProfessionInfo
    {
        public HKProfessionInfo()
        { }
        #region Model
        private string _nindcd;
        private string _nindnme;
        private string _ennindnme;
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
        /// 行业中文描述
        /// </summary>
        [DataMember]
        public string Nindnme
        {
            set { _nindnme = value; }
            get { return _nindnme; }
        }
        /// <summary>
        /// 行业英文描述
        /// </summary>
        [DataMember]
        public string EnNindnme
        {
            set { _ennindnme = value; }
            get { return _ennindnme; }
        }
        #endregion Model

    }
}
