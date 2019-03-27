using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：品种表实体类CM_BreedClass 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
    public class CM_BreedClass
    {
        public CM_BreedClass()
        { }
        #region Model
        private int _breedclassid;
        private string _breedclassname;
        private int? _accounttypeidfund;
        private int? _breedclasstypeid;
        private int? _accounttypeidhold;
        private int? _boursetypeid;
        private int? _issysdefaultbreed;
        private int? _ishkbreedclasstype;
        private int? _deletestate;
        /// <summary>
        /// 品种ID
        /// </summary>
        [DataMember]
        public int BreedClassID
        {
            set { _breedclassid = value; }
            get { return _breedclassid; }
        }
        /// <summary>
        /// 品种名称
        /// </summary>
        [DataMember]
        public string BreedClassName
        {
            set { _breedclassname = value; }
            get { return _breedclassname; }
        }
        /// <summary>
        /// 资金帐号类型
        /// </summary>
        [DataMember]
        public int? AccountTypeIDFund
        {
            set { _accounttypeidfund = value; }
            get { return _accounttypeidfund; }
        }
        /// <summary>
        /// 品种类型ID
        /// </summary>
        [DataMember]
        public int? BreedClassTypeID
        {
            set { _breedclasstypeid = value; }
            get { return _breedclasstypeid; }
        }
        /// <summary>
        /// 持仓帐号类型
        /// </summary>
        [DataMember]
        public int? AccountTypeIDHold
        {
            set { _accounttypeidhold = value; }
            get { return _accounttypeidhold; }
        }
        /// <summary>
        /// 交易所ID
        /// </summary>
        [DataMember]
        public int? BourseTypeID
        {
            set { _boursetypeid = value; }
            get { return _boursetypeid; }
        }

        #region 2009.10.22 新增字段
        /// <summary>
        /// 是否是系统默认品种
        /// </summary>
        [DataMember]
        public int? ISSysDefaultBreed
        {
            set { _issysdefaultbreed = value; }
            get { return _issysdefaultbreed; }
        }

        /// <summary>
        /// 是否港股品种类型
        /// </summary>
        [DataMember]
        public int? ISHKBreedClassType
        {
            set { _ishkbreedclasstype = value; }
            get { return _ishkbreedclasstype; }
        }

        /// <summary>
        /// 删除状态
        /// </summary>
        [DataMember]
        public int? DeleteState
        {
            set { _deletestate = value; }
            get { return _deletestate; }
        }
        #endregion

        #endregion Model

    }
}

