using System;
using GTA.VTS.Common.CommonObject;
using System.Runtime.Serialization;


namespace MatchCenter.Entity
{
    /// <summary>
    /// 股指期货（下单）委托单实体类 
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
    public class FutureOrderEntity
    {
        #region Model
        private string channelNo;
        private string stockCode = string.Empty;
        private decimal orderPrice;
        private decimal orderVolume;
        private Types.TransactionDirection transactionDirection;
        private Types.MarketPriceType MarketPriceType;
        private CHDirection direction;
        private string sholderCode;

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
        public string ChannelNo
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
        public Types.MarketPriceType IsMarketPrice
        {
            get
            {
                return MarketPriceType;
            }
            set
            {
                MarketPriceType = value;
            }
        }

        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public Types.TransactionDirection TransactionDirection
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
        /// 开平方向
        /// </summary>
        [DataMember]
        public CHDirection Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }



        #endregion Model
    }
}
