
#region Using Namespace

using System;
using System.Runtime.Serialization;

#endregion

namespace MatchCenter.Entity
{
    /// <summary>
    /// 现货成交回报实体
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
    public class StockDealBackEntity
    {
        #region Model

        private decimal dealamount;

        private DateTime dealdatetime;
        private decimal dealprice;
        private string id = String.Empty;
        private string orderid;

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