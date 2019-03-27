#region Using Namespace

using System;
using System.Runtime.Serialization;
using ReckoningCounter.Entity.Contants;

#endregion

namespace ReckoningCounter.Entity
{
    /// <summary>
    /// Title:期货下单数据契约
    /// Desc.:期货下单数据契约
    /// Create by:宋涛
    /// Create date:2008-12-2
    /// </summary>
    [DataContract]
    public class MercantileFuturesOrderRequest
    {
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
        /// 合约代码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

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
        /// 开平仓方向
        /// </summary>
        [DataMember]
        public Types.FutureOpenCloseType OpenCloseType { get; set; }

        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.TransactionDirection BuySell { get; set; }

        /// <summary>
        /// 委托类型(限、市价)
        /// </summary>
        [DataMember]
        public Types.OrderPriceType OrderWay { get; set; }

        /// <summary>
        /// 投组ID
        /// </summary>
        [DataMember]
        public string PortfoliosId { get; set; }

        /// <summary>
        /// 委托单位类型
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.UnitType OrderUnitType { get; set; }
    }


    /// <summary>
    /// Title:股指期货下单数据契约
    /// Desc.:股指期货下单数据契约
    /// Create by:宋涛
    /// Create date:2008-12-2
    /// </summary>
    [DataContract]
    public class StockIndexFuturesOrderRequest
    {
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
        /// 合约代码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

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
        /// 开平仓方向
        /// </summary>
        [DataMember]
        public Types.FutureOpenCloseType OpenCloseType { get; set; }

        /// <summary>
        /// 买卖方向
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.TransactionDirection BuySell { get; set; }

        /// <summary>
        /// 委托类型(限、市价)
        /// </summary>
        [DataMember]
        public Types.OrderPriceType OrderWay { get; set; }

        /// <summary>
        /// 投组ID
        /// </summary>
        [DataMember]
        public string PortfoliosId { get; set; }

        /// <summary>
        /// 委托单位类型
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.UnitType OrderUnitType { get; set; }

        /// <summary>
        /// 转换显示的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //string msg = "股指期货下单数据契约[]";
            return base.ToString();
        }
    }


    /// <summary>
    /// Title:现货下单请求
    /// Desc.:现货下单请求
    /// Create by:宋涛
    /// Create date:2008-12-2
    /// </summary>
    [DataContract]
    public class StockOrderRequest
    {
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
        /// 买卖方向
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.TransactionDirection BuySell { get; set; }

        /// <summary>
        /// 委托类型(限、市价)
        /// </summary>
        [DataMember]
        public Types.OrderPriceType OrderWay { get; set; }

        /// <summary>
        /// 投组ID
        /// </summary>
        [DataMember]
        public string PortfoliosId { get; set; }

        /// <summary>
        /// 委托单位类型
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.UnitType OrderUnitType { get; set; }

        /// <summary>
        /// 转换显示的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string format = "现货下单请求[交易员ID={0},资金帐号={1},商品代码={2},委托价格={3},委托数量={4},买卖方向={5},委托类型(限、市价)={6},委托单位类型={7}]";

            string desc = String.Format(format, this.TraderId, FundAccountId, Code, OrderPrice, OrderAmount, BuySell,
                                        OrderWay, OrderUnitType);
            return desc;
        }
    }

    /// <summary>
    /// Title:港股下单请求
    /// Desc.:港股下单请求
    /// Create by:宋涛
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class HKOrderRequest
    {
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
        /// 买卖方向
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.TransactionDirection BuySell { get; set; }

        /// <summary>
        /// 委托类型(LO: 限价盘；ELO：增强限价盘；SLO：特别限价盘)
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.HKPriceType OrderWay { get; set; }

        /// <summary>
        /// 投组ID
        /// </summary>
        [DataMember]
        public string PortfoliosId { get; set; }

        /// <summary>
        /// 委托单位类型
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.UnitType OrderUnitType { get; set; }

        /// <summary>
        /// 转换显示的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string format = "港股下单请求[交易员ID={0},资金帐号={1},商品代码={2},委托价格={3},委托数量={4},买卖方向={5},委托类型(限、市价)={6},委托单位类型={7}]";

            string desc = String.Format(format, this.TraderId, FundAccountId, Code, OrderPrice, OrderAmount, BuySell,
                                        OrderWay, OrderUnitType);
            return desc;
        }
    }

    /// <summary>
    /// Title:港股改单请求
    /// Desc.:港股改单请求
    /// Create by:宋涛
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class HKModifyOrderRequest
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
        ///// <summary>
        ///// 买卖方向
        ///// </summary>
        //[DataMember]
        //public CommonObject.Types.TransactionDirection BuySell { get; set; }

        ///// <summary>
        ///// 委托类型(LO: 限价盘；ELO：增强限价盘；SLO：特别限价盘)
        ///// </summary>
        //[DataMember]
        //public Types.HKOrderPriceType OrderWay { get; set; }

        ///// <summary>
        ///// 投组ID
        ///// </summary>
        //[DataMember]
        //public string PortfoliosId { get; set; }

        /// <summary>
        /// 委托单位类型
        /// </summary>
        [DataMember]
        public GTA.VTS.Common.CommonObject.Types.UnitType OrderUnitType { get; set; }

        /// <summary>
        /// 转换显示的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string format = "港股改单请求[交易员ID={0},资金帐号={1},商品代码={2},委托价格={3},委托数量={4}]";

            string desc = String.Format(format, this.TraderId, FundAccountId, Code, OrderPrice, OrderAmount);
            return desc;
        }
    }
}
