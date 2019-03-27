using System;
using GTA.VTS.Common.CommonObject;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心撤单实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
    public partial class CancelEntity
    {
        private string stockCode = string.Empty;
        private string oldOrderNo;
        private string channelNo;
        private int cancelCount = 0;

        /// <summary>
        /// 证卷代码
        /// </summary>
        [DataMember]
        public string StockCode
        {
            set { stockCode = value; }
            get { return stockCode; }
        }

        /// <summary>
        ///撤单次数 
        /// </summary>
        [DataMember]
        public int CancelCount
        {
            set { cancelCount = value; }
            get { return cancelCount; }
        }

        /// <summary>
        /// 原委托单号
        /// </summary>
        [DataMember]
        public string OldOrderNo
        {
            get { return oldOrderNo; }
            set { oldOrderNo = value; }
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



    }
}