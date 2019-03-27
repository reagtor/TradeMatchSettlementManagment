using System;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心期货实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [DataContract]
    public class FutureDealEntity
    {
        #region Model

        /// <summary>
        /// 委托单号ID
        /// </summary>
        [DataMember]
        public string orderid;

        /// <summary>
        /// 成交价格
        /// </summary>
        [DataMember]
        public decimal dealprice;

        /// <summary>
        /// 成交数量
        /// </summary>
        [DataMember]
        public decimal dealamount;

        /// <summary>
        /// 成交时间
        /// </summary>
        [DataMember]
        public DateTime dealdatetime;

        /// <summary>
        /// 通道号
        /// </summary>
        [DataMember]
        public string channelNo;

        #endregion Model
    }
}
