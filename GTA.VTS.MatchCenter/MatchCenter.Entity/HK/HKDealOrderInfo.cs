using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using GTA.VTS.Common.CommonObject;

namespace MatchCenter.Entity.HK
{
    /// <summary>
    /// 港股撮合成交实体类HKDealOrderInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// Desc.:修类名为了与之前的回推命名规则一至
    /// Update BY：李健华
    /// Update Date：2009-10-15
    /// 
    /// </summary>
    [DataContract]
    public class HKDealBackEntity
    {
        public HKDealBackEntity()
        { }
        #region Model
        private string id;
        private string _orderno;
        private string _channelid;
        private decimal _dealprice;
        private decimal _dealamount;
        private DateTime _dealtime;
        private bool _dealtype;
        private string _dealmessage;
        /// <summary>
        /// 成交记录编号标识
        /// </summary>
        [DataMember]
        public string ID
        {
            set { id = value; }
            get { return id; }
        }
        /// <summary>
        /// 委托单编号
        /// </summary>
        [DataMember]
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// 成交回推通道
        /// </summary>
        [DataMember]
        public string ChannelID
        {
            set { _channelid = value; }
            get { return _channelid; }
        }
        /// <summary>
        /// 成交价格
        /// </summary>
        [DataMember]
        public decimal DealPrice
        {
            set { _dealprice = value; }
            get { return _dealprice; }
        }
        /// <summary>
        /// 成交量
        /// </summary>
        [DataMember]
        public decimal DealAmount
        {
            set { _dealamount = value; }
            get { return _dealamount; }
        }
        /// <summary>
        /// 成交时间
        /// </summary>
        [DataMember]
        public DateTime DealTime
        {
            set { _dealtime = value; }
            get { return _dealtime; }
        }
        /// <summary>
        /// 成交类型(0撮合成交，1还是撤单成交）
        /// </summary>
        [DataMember]
        public bool DealType
        {
            set { _dealtype = value; }
            get { return _dealtype; }
        }
        /// <summary>
        /// 成交单信息
        /// </summary>
        [DataMember]
        public string DealMessage
        {
            set { _dealmessage = value; }
            get { return _dealmessage; }
        }
        #endregion Model

    }
}
