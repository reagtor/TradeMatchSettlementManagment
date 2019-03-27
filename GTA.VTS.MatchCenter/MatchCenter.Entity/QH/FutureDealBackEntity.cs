using System;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 期货成交/委托回报实体
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
   public class FutureDealBackEntity
    {
        #region Model

        private string orderid;

        private decimal dealprice;

        private decimal dealamount;

        private string id = String.Empty;

        private DateTime dealdatetime;

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
        /// 委托单号
        /// </summary>
        [DataMember]
        public string OrderNo
        {
            get { return orderid; }
            set { orderid = value; }
        }

        /// <summary>
        /// 通道号
        /// </summary>
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
        /// 成交价格
        /// </summary>
        [DataMember]
        public decimal DealPrice
        {
            get { return dealprice; }
            set { dealprice = value; }
        }

        /// <summary>
        /// 成交数量
        /// </summary>
        [DataMember]
        public decimal DealAmount
        {
            get { return dealamount; }
            set { dealamount = value; }
        }

        /// <summary>
        /// 成交时间
        /// </summary>
        [DataMember]
        public DateTime DealTime
        {
            get { return dealdatetime; }
            set { dealdatetime = value; }
        }

        #endregion Model
    }
}
