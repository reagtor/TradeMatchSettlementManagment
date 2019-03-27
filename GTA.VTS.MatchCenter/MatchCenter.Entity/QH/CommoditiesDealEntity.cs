using System;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 商品期货成交实体类(与数据库应)
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    public class CommoditiesDealEntity
    {

        private string _dealorderno;
        private string _orderno;
        private string _channelid;
        private decimal _orderprice;
        private decimal _dealamount;
        private DateTime _dealtime;
        private string _dealtype;
        private string _dealmessage;
        /// <summary>
        /// 撮合中心期货成交回报主键
        /// </summary>
        public string DealOrderNo
        {
            set { _dealorderno = value; }
            get { return _dealorderno; }
        }
        /// <summary>
        /// 委托单号
        /// </summary>
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// 通道号
        /// </summary>
        public string ChannelID
        {
            set { _channelid = value; }
            get { return _channelid; }
        }
        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal OrderPrice
        {
            set { _orderprice = value; }
            get { return _orderprice; }
        }
        /// <summary>
        /// 成交数量
        /// </summary>
        public decimal DealAmount
        {
            set { _dealamount = value; }
            get { return _dealamount; }
        }
        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime DealTime
        {
            set { _dealtime = value; }
            get { return _dealtime; }
        }
        /// <summary>
        /// 成交类型
        /// </summary>
        public string DealType
        {
            set { _dealtype = value; }
            get { return _dealtype; }
        }
        /// <summary>
        /// 成交单信息
        /// </summary>
        public string DealMessage
        {
            set { _dealmessage = value; }
            get { return _dealmessage; }
        }
    }
}
