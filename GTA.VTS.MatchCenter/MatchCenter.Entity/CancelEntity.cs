using System;
using System.Runtime.Serialization;


namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心撤单实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
   public  class CancelOrderEntity
    {
        private string message = string.Empty;
        private string orderNo = string.Empty;
        private bool isSuccess;
        private decimal orderVolume;

        private string id = String.Empty;

        private string channelNo;

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
        /// 撤单数量
        /// </summary>
        [DataMember]
        public decimal OrderVolume
        {
            get
            {
                return orderVolume;
            }
            set
            {
                orderVolume = value;
            }
        }
        /// <summary>
        /// 通道号
        /// </summary>
        [DataMember]
        public string ChannelNo
        {
            get
            {
                return channelNo;
            }
            set
            {
                channelNo = value;
            }
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

       /// <summary>
       /// 是否成功
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
    }
}
