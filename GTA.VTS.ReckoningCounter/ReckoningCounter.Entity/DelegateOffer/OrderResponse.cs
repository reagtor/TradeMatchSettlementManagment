using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using ReckoningCounter.Entity.Contants;
using ReckoningCounter.Entity.Model.QH;
using ReckoningCounter.Entity.Model.XH;
using ReckoningCounter.Model;
using ReckoningCounter.Entity.Model.HK;


namespace ReckoningCounter.Entity
{

    /// <summary>
    /// Title:委托返回信息
    /// Desc.:委托返回信息
    /// Create by:宋涛
    /// Create date:2008-11-2
    /// </summary>
    [DataContract]
    public class OrderResponse
    {
        /// <summary>
        /// 委托单号ID
        /// </summary>
        [DataMember]
        public string OrderId { get; set; }

        /// <summary>
        /// 委托受理时间
        /// </summary>
        [DataMember]
        public DateTime OrderDatetime { get; set; }

        /// <summary>
        /// 委托信息
        /// </summary>
        [DataMember]
        public string OrderMessage { get; set; }

        /// <summary>
        /// 委托是否通过
        /// </summary>
        [DataMember]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        [DataMember]
        public int ErrorType { get; set; }

    }

    /// <summary>
    /// Title:现货回推的委托数据
    /// Desc.:现货回推的委托数据
    /// Create by:宋涛
    /// Create date:2008-11-2
    /// </summary>
    [DataContract]
    public class StockDealOrderPushBack
    {
        /// <summary>
        /// 委托实体
        /// </summary>
        [DataMember]
        public StockPushOrderEntity StockOrderEntity;

        /// <summary>
        /// 现货成交回报实体
        /// </summary>
        [DataMember]
        public List<StockPushDealEntity> StockDealList;

        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TradeID;

    }

    /// <summary>
    /// Title:港股回推的委托数据
    /// Desc.:港股回推的委托数据
    /// Create by:宋涛
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class HKDealOrderPushBack
    {
        /// <summary>
        /// 港股委托实体
        /// </summary>
        [DataMember]
        public HKPushOrderEntity HKOrderEntity;

        /// <summary>
        /// 港股成交回报实体
        /// </summary>
        [DataMember]
        public List<HKPushDealEntity> HKDealList;

        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TradeID;
    }

    /// <summary>
    /// Title:港股改单回报
    /// Desc.:港股改单回报
    /// Create by:宋涛
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class HKModifyOrderPushBack
    {
        /// <summary>
        /// 记录ID（主键）
        /// </summary>
        [DataMember]
        public string ID;

        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TradeID;

        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public string Message;

        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public bool IsSuccess;

        /// <summary>
        /// 原始委托的单号
        /// </summary>
        [DataMember]
        public string OriginalRequestNumber;

        /// <summary>
        /// 新委托的单号（可以为空因为改单规则不附合时还没有生成单号)
        /// </summary>
        [DataMember]
        public string NewRequestNumber;

        /// <summary>
        /// 回调信息注册通道
        /// </summary>
        [DataMember]
        public string CallbackChannlId;

    }

    /// <summary>
    /// Title:期货回推的委托数据
    /// Desc.:期货回推的委托数据
    /// Create by:宋涛
    /// Create date:2008-12-2
    /// </summary>
    [DataContract]
    public class FutureDealOrderPushBack
    {
        /// <summary>
        /// 回推期货委托
        /// </summary>
        [DataMember]
        public FuturePushOrderEntity StockIndexFuturesOrde;

        /// <summary>
        /// 期货成交回报实体
        /// </summary>
        [DataMember]
        public List<FuturePushDealEntity> FutureDealList;

        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TradeID;

    }

}
