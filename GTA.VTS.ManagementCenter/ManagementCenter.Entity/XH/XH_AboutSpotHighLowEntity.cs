using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model.XH
{
    /// <summary>
    ///描述：现货相关涨跌幅实体
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
   public class XH_AboutSpotHighLowEntity
    {
        #region Model

        private int? _breedclasshighlowid;
        private int? _breedclassvalidid;
        /// <summary>
        /// 品种涨跌幅标识
        /// </summary>
        [DataMember]
        public int? BreedClassHighLowID
        {
            set { _breedclasshighlowid = value; }
            get { return _breedclasshighlowid; }
        }

        /// <summary>
        /// 品种有效申报标识
        /// </summary>
        [DataMember]
        public int? BreedClassValidID
        {
            set { _breedclassvalidid = value; }
            get { return _breedclassvalidid; }
        }
        #endregion
    }
}
