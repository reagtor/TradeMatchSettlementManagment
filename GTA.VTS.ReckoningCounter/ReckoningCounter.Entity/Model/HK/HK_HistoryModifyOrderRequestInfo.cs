using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// Title:港股改单历史表实体
    /// Create by:李健华
    /// Create Date:2009-10-29
    /// </summary>
    [DataContract]
    public class HK_HistoryModifyOrderRequestInfo
    {
        /// <summary>
        /// 改单ID，由柜台赋予，供前台查询
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// 回送通道ID
        /// </summary>
        [DataMember]
        public string ChannelID { get; set; }

        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderId { get; set; }


        /// <summary>
        /// 资金帐号
        /// </summary>
        [DataMember]
        public string FundAccountId { get; set; }

        /// <summary>
        /// 交易员密码
        /// </summary>
        [DataMember]
        public string TraderPassword { get; set; }


        /// <summary>
        /// 商品代码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 被修改的委托单号
        /// </summary>
        [DataMember]
        public string EntrustNubmer { get; set; }

        /// <summary>
        /// 委托价格
        /// </summary>
        [DataMember]
        public float OrderPrice { get; set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        [DataMember]
        public float OrderAmount { get; set; }

        /// <summary>
        /// 委托信息（供柜台处理改单失败时填写，下单时不需要）
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 改单时间
        /// </summary>
        [DataMember]
        public DateTime ModifyOrderDateTime { get; set; }
    }
}
