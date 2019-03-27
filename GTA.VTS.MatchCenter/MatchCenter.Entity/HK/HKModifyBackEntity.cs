using System;
using System.Runtime.Serialization;


namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心改单回报实体类
    /// Create BY：王伟
    /// Create Date：2009-10-20
    /// </summary>
    [DataContract]
    public class HKModifyBackEntity
    {
        private string message = string.Empty;
        private string orderNo = string.Empty;
        private bool isSuccess;
        //private decimal orderVolume;

        private string id = String.Empty;

        private string channelNo;

        private decimal modifyVolume;

        private decimal modifyCount;

        private CancelOrderEntity cancleOrderEntity;


        /// <summary>
        /// 改单撤单回报实体
        /// </summary>
        [DataMember]
        public CancelOrderEntity CanleOrderEntity
        {
            get
            {
                return cancleOrderEntity;

            }
            set
            {
                cancleOrderEntity = value;
            }
        }
        
        
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

        /// <summary>
        /// 改单数量
        /// </summary>
        [DataMember]
        public decimal ModifyVolume
        {
            get
            {
                return modifyVolume;
            }
            set
            {
                modifyVolume = value;
            }
        }

        /// <summary>
        /// 改单次数
        /// </summary>
        [DataMember]
        public decimal ModifyCount
        {
            get
            {
                return modifyCount;
            }
            set
            {
                modifyCount = value;
            }
        }
    }
}

