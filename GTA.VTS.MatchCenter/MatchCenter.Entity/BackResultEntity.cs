using System;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心回报实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
    public class ResultDataEntity
    {
        private string message = string.Empty;
        private string orderNo = string.Empty;

        private string id = String.Empty;

        /// <summary>
        /// 对象标识
        /// </summary>
        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }     
        /// <summary>
        /// 返回异常信息CH--0001、委托单不是在交易时间之内；
        /// CH--0002、委托单价格不在涨跌幅之内；
        /// CH-- 0003、证卷停牌；CH-- 0004、其他；CH-- 0004
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

        /// <summary>
        /// 委托单号
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

      

    }
}
