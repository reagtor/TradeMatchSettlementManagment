
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心业务异常情况
    /// Create BY：李健华
    /// Create Date：2009-08-21
    /// </summary>
    [DataContract]
    public enum CHExceptionType
    {
        /// <summary>
        /// 没异常
        /// </summary>
        [EnumMember]
        NoException = 1,

        /// <summary>
        /// 委托单不是在交易时间之内
        /// </summary>
        [EnumMember]
        TimeException = 2,

        /// <summary>
        /// 委托单价格不在涨跌幅内
        /// </summary>
        [EnumMember]
        PriceException = 3,

        /// <summary>
        /// 停牌
        /// </summary>
        [EnumMember]
        PauseException = 4,

        /// <summary>
        /// 其他
        /// </summary>
        [EnumMember]
        OtherException = 5

    }

    /// <summary>
    /// 开平方向
    /// </summary>
    [DataContract]
    public enum CHDirection
    { 
        /// <summary>
        /// 开
        /// </summary>
        [EnumMember]
        Open = 0,

        /// <summary>
        /// 平
        /// </summary>
        [EnumMember]
        Shut = 1,

    }

    /// <summary>
    /// 品种类型
    /// </summary>
    public enum BreedClassTypeEnum
    {
        /// <summary>
        /// 股票现货
        /// </summary>
        Stock = 1,
        /// <summary>
        /// 商品期货
        /// </summary>
        CommodityFuture = 2,
        /// <summary>
        /// 股指期货
        /// </summary>
        StockIndexFuture = 3
    }


}
