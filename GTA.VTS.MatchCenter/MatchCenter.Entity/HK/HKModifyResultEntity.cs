using System;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 改单结果实体类
    /// Create BY：王伟
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
    public class HKModifyResultEntity
    {
        private string orderNo = string.Empty;

        private bool isSuccess;

        private string message;

        /// <summary>
        /// 委托单号码
        /// </summary>
        [DataMember]
        public string OrderNo
        {
            get
            {
                return orderNo;
            }
            set
            {
                orderNo = value;
            }
        }

        /// <summary>
        /// 改单是否成功
        /// </summary>
        [DataMember]
        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
            set
            {
                isSuccess = value;
            }
        }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}

