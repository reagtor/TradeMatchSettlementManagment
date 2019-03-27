using System;
using GTA.VTS.Common.CommonObject;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心改单实体类
    /// Create BY：王伟
    /// Create Date：2009-10-20
    /// </summary>
    [DataContract]
    public partial class HKModifyEntity
    {
        private string stockCode = string.Empty;
        private string oldOrderNo;
        private string channelNo;
        //private int moCount = 0;
        private int modifyVolume = 0;

        /// <summary>
        /// 证卷代码
        /// </summary>
        [DataMember]
        public string StockCode
        {
            set { stockCode = value; }
            get { return stockCode; }
        }

        ///// <summary>
        /////改单次数 
        ///// </summary>
        //[DataMember]
        //public int ModCount
        //{
        //    set { moCount = value; }
        //    get { return moCount; }
        //}

        /// <summary>
        ///改单数量 
        /// </summary>
        [DataMember]
        public int ModifyVolume
        {
            set
            {
                modifyVolume = value;
                
            }
            get { return modifyVolume; }
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