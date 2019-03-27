using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：有效申报类型表 实体类XH_ValidDeclareType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2008-12-2
    /// </summary>
    [DataContract]
    public class XH_ValidDeclareType
    {
        public XH_ValidDeclareType()
        {
        }

        #region Model

        private int _breedClassValidID;
        private int _validDeclareTypeID;

        /// <summary>
        /// 有效申报类型标识
        /// </summary>
        [DataMember]
        public int ValidDeclareTypeID
        {
            set { _validDeclareTypeID = value; }
            get { return _validDeclareTypeID; }
        }

        /// <summary>
        /// 品种有效申报标识
        /// </summary>
        [DataMember]
        public int BreedClassValidID
        {
            set { _breedClassValidID = value; }
            get { return _breedClassValidID; }
        }

        #endregion Model
    }
}