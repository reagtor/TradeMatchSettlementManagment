using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    ///描述：交易所类型表 实体类CM_BourseType 。(属性说明自动提取数据库字段的描述信息)
    ///作者：刘书伟
    ///日期:2009-07-08
    /// </summary>
    [DataContract]
    public class CM_BourseType
    {
        public CM_BourseType()
        { }
        #region Model
        private int _boursetypeid;
        private string _boursetypename;
        private DateTime? _receivingconsignstarttime;
        private DateTime? _receivingconsignendtime;
        private DateTime? _counterfromsubmitstarttime;
        private DateTime? _counterfromsubmitendtime;
        private int? _issysdefaultboursetype;
        private int? _deletestate;
        /// <summary>
        /// 交易所类型标识
        /// </summary>
        [DataMember]
        public int BourseTypeID
        {
            set { _boursetypeid = value; }
            get { return _boursetypeid; }
        }
        /// <summary>
        /// 类型描述
        /// </summary>
        [DataMember]
        public string BourseTypeName
        {
            set { _boursetypename = value; }
            get { return _boursetypename; }
        }
        /// <summary>
        /// 撮合接收委托开始时间
        /// </summary>
        [DataMember]
        public DateTime? ReceivingConsignStartTime
        {
            set { _receivingconsignstarttime = value; }
            get { return _receivingconsignstarttime; }
        }
        /// <summary>
        /// 撮合接收委托结束时间
        /// </summary>
        [DataMember]
        public DateTime? ReceivingConsignEndTime
        {
            set { _receivingconsignendtime = value; }
            get { return _receivingconsignendtime; }
        }
        /// <summary>
        /// 柜台接收委托开始时间
        /// </summary>
        [DataMember]
        public DateTime? CounterFromSubmitStartTime
        {
            set { _counterfromsubmitstarttime = value; }
            get { return _counterfromsubmitstarttime; }
        }
        /// <summary>
        /// 柜台接收委托结束时间
        /// </summary>
        [DataMember]
        public DateTime? CounterFromSubmitEndTime
        {
            set { _counterfromsubmitendtime = value; }
            get { return _counterfromsubmitendtime; }
        }

        #region 2009.10.22 新增字段
        /// <summary>
        /// 是否是系统默认交易所
        /// </summary>
        [DataMember]
        public int? ISSysDefaultBourseType
        {
            set { _issysdefaultboursetype = value; }
            get { return _issysdefaultboursetype; }
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

        /// <summary>
        /// 代码规则： 1、6位数字，2、5位数字，3、品种代码+2位年份+2位月份，4、品种代码+1位年份+2位月份
        /// </summary>
        [DataMember]
        public int? CodeRulesType
        {
            get;
            set;
        }
        #endregion
        #endregion Model

    }
}

