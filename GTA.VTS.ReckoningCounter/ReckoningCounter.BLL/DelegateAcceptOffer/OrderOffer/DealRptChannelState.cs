using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer
{

    /// <summary>
    /// 成交回报通道状态
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public enum ChannelState
    {
        /// <summary>
        /// 正常
        /// </summary>
        CSNormal,
        /// <summary>
        /// 出错
        /// </summary>
        CSError,
        /// <summary>
        /// 正在检测
        /// </summary>
        CSChecking
    }

    /// <summary>
    /// 带状态的成交回报通道
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    class DealRptChannelState
    {
        /// <summary>
        /// 成交回报通道
        /// </summary>
        public OrderDealRptClient DealRptChannel { get; set; }
        /// <summary>
        /// 通道状态
        /// </summary>
        public ChannelState State { get; set; }

        /// <summary>
        /// 成交回报通道服务地址
        /// </summary>
        public string ServiceAddress { get; private set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="dealRptChannel"></param>
        /// <param name="state"></param>
        /// <param name="serviceAddress"></param>
        public DealRptChannelState(OrderDealRptClient dealRptChannel, ChannelState state, string serviceAddress)
        {
            DealRptChannel = dealRptChannel;
            State = state;
            ServiceAddress = serviceAddress;
        }


    }
}
