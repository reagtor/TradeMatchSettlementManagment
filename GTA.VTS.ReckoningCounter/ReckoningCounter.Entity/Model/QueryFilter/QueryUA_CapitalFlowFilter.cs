using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ReckoningCounter.Entity.Model.QueryFilter;

namespace ReckoningCounter.Entity.Model
{
    /// <summary>
    /// Title: 资金账户转账流水实体
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    [DataContract]
    public class QueryUA_CapitalFlowFilter
    {

        /// <summary>
        /// 流水标识
        /// </summary>
        [DataMember]
        public string FlowLogo
        {
            get;
            set;
        }

        /// <summary>
        /// 转出资金账户 当是以用户查询时不要传入，忽略就以用户关联的账号查询
        /// </summary>
        [DataMember]
        public string FromCapitalAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 转入资金账户
        /// </summary>
        [DataMember]
        public string ToCapitalAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime? StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime? EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 资金数量
        /// </summary>
        [DataMember]
        public decimal CapitalAmount
        {
            get;
            set;
        }


        /// <summary>
        /// 币种
        /// </summary>
        [DataMember]
        public QueryType.QueryCurrencyType CurrencyType
        {
            get;
            set;
        }

        /// <summary>
        ///  资金流水类型(包括：几个资金账户之间的同币种自由转账类型、银行账户中的三个币种间的自由兑换类型）
        /// 1---自由转账,2---分红转账,3--追加资金 ,其他为全部
        ///</summary>
        [DataMember]
        public int CapitalFlowType
        {
            get;
            set;
        }

    }
}
