using System;
using System.Runtime.Serialization;
using GTA.VTS.Common.CommonObject;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心委托实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
    public class StockDataOrderEntity
    {
        #region Model
        private string channelNo;
        private string orderNo = string.Empty;
        private DateTime reachTime;
        private string stockCode = string.Empty;
        private decimal orderPrice;
        private decimal orderVolume;
        private int transactionDirection;
        private int isMarketPrice;
        private string oldOrderNo;
        private decimal markLeft;
        private string sholderCode;
        private string marketVolumeNo;
        private Types.MatchCenterState matchState = Types.MatchCenterState.First;


        /// <summary>
        /// 撮合状态
        /// </summary>
        public Types.MatchCenterState MatchState
        {
            get { return matchState; }
            set { matchState = value; }
        }

       /// <summary>
        /// 市场存量
       /// </summary>
        [DataMember]
        public decimal MarkLeft
        {
            get
            {
                return markLeft;
            }
            set
            {
                markLeft = value;
            }
        }

        /// <summary>
        /// 股东代码
        /// </summary>
        [DataMember]
        public string SholderCode
        {
            get
            {
                return sholderCode;
            }
            set
            {
                sholderCode = value;
            }
        }
       


         /// <summary>
        /// 通道号
        /// </summary>
        [DataMember]
        public string  ChannelNo
        {
            set
            {
                channelNo = value;
            }
            get
            {
                return channelNo;
            }
           
        }

        /// <summary>
        ///委托单编号 
        /// </summary>
        [DataMember]
        public string OrderNo
        {
            set { orderNo = value; }
            get { return orderNo; }
        }

      

        /// <summary>
        /// 委托到达时间
        /// </summary>
        [DataMember]
        public DateTime ReachTime
        {
            set { reachTime = value; }
            get { return reachTime; }
        }

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
        /// 委托价格
        /// </summary>
        [DataMember]
        public decimal OrderPrice
        {
            set { orderPrice = value; }
            get { return orderPrice; }
        }

        /// <summary>
        /// 委托数量
        /// </summary>
        [DataMember]
        public decimal OrderVolume
        {
            set { orderVolume = value; }
            get { return orderVolume; }
        }

       
        /// <summary>
        /// 是否是市价委托
        /// </summary>
        [DataMember]
        public int IsMarketPrice
        {
            get
            {
                return isMarketPrice;
            }
            set
            {
                isMarketPrice = value;
            }
        }

        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public  int TransactionDirection
        {
            get
            {
                return transactionDirection;
            }
            set
            {
                transactionDirection = value;
            }
        }

        /// <summary>
        /// 原委托单号
        /// </summary>
        [DataMember]
        public  string OldOrderNo
        {
            get
            {
                return oldOrderNo;
            }
            set
            {
                oldOrderNo = value;
            }
        }

      
        /// <summary>
        /// 市场存量关联委托单编号
        /// </summary>
        [DataMember]
        public  string MarketVolumeNo
        {
            get
            {
                return marketVolumeNo;
            }
            set
            {
                marketVolumeNo = value;
            }
        }

      
        #endregion Model


    }
}
