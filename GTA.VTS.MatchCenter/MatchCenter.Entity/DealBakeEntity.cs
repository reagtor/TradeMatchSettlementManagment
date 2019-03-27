using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心成交回报实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
   public class DealBakeEntity
    {
        #region Model

        private string dealOrderNo;

        private string orderid;

        private decimal dealprice;

        private decimal dealamount;

        private DateTime dealdatetime;

        private string channelNo;

        private BreedClassTypeEnum classType;

       /// <summary>
       /// 成交回报编号
       /// </summary>
       public string DealOrderNo
       {
           get { return dealOrderNo; }
           set { dealOrderNo = value; }
       }
        /// <summary>
        /// 委托单号
        /// </summary>
        [DataMember]
        public string OrderNo
        {
            get
            {
                return orderid;
            }
            set
            {
                orderid = value;
            }
        }

        /// <summary>
        /// 成交价格
        /// </summary>
        [DataMember]
        public decimal DealPrice
        {
            get
            {
                return dealprice;
            }
            set
            {
                dealprice = value;
            }
        }

        /// <summary>
        /// 成交数量
        /// </summary>
        [DataMember]
        public decimal DealAmount
        {
            get
            {
                return dealamount;
            }
            set
            {
                dealamount = value;
            }
        }

        /// <summary>
        /// 成交时间
        /// </summary>
        [DataMember]
        public DateTime DealTime
        {
            get
            {
                return dealdatetime;
            }
            set
            {
                dealdatetime = value;
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
        /// 品种类型
        /// </summary>
        [DataMember]
        public BreedClassTypeEnum ClassType
        {
            get
            {
                return classType;
            }
            set
            {
                classType = value;
            }
        }
        #endregion Model
    }
}
