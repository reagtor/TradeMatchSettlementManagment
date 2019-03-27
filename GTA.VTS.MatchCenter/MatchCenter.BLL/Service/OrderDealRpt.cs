using System;
using System.Collections;
using System.ServiceModel;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.PushBack;
using MatchCenter.BLL.interfaces;

namespace MatchCenter.BLL.Service
{
    /// <summary>
    /// 撮合中心成交回报通道处理类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class OrderDealRpt : IOrderDealRpt
    {
        #region IOrderDealRpt 成员

        /// <summary>
        /// 注册通道
        /// </summary>
        /// <returns></returns>
        public void RegisterChannel(string channelId)
        {
            //通道不能为空
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }

            lock (((ICollection)MatchCenterManager.Instance.OperationContexts).SyncRoot)
            {
                OperationContext context = OperationContext.Current;
                if (context == null)
                {
                    LogHelper.WriteDebug("OrderDealRpt.RegisterChannel注册获取或设置当前线程的执行上下文[ClientID=" + channelId + "]");
                    return;
                }
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(channelId))
                {
                    LogHelper.WriteDebug("OrderDealRpt.RegisterChannel重置[ClientID=" + channelId + "]");
                    MatchCenterManager.Instance.OperationContexts[channelId] = context;
                }
                else
                {
                    MatchCenterManager.Instance.OperationContexts.Add(channelId, context);
                    LogHelper.WriteDebug("OrderDealRpt.RegisterChannel新注册[ClientID=" + channelId + "]");
                }

                TradePushBackImpl.Instanse.InsertEntity(channelId);
                TradePushBackImpl.Instanse.InsertFuture(channelId);
                TradePushBackImpl.Instanse.InsertCommodities(channelId);
                //增加港股
                TradePushBackImpl.Instanse.InsertHKEntity(channelId);
                TradePushBackImpl.Instanse.InsertHKModifyEntity(channelId);
                TradePushBackImpl.Instanse.InsertCancelXHEntity(channelId);
                #region add by 董鹏 2010-04-30
                TradePushBackImpl.Instanse.InsertCancelHKEntity(channelId);
                TradePushBackImpl.Instanse.InsertCancelSIEntity(channelId);
                TradePushBackImpl.Instanse.InsertCancelCFEntity(channelId);
                #endregion

                context.Channel.Faulted += Channel_Faulted;
            }
        }


        /// <summary>
        /// 注销通道
        /// </summary>
        /// <param name="channelId">通道ID</param>
        public void UnRegisterChannel(string channelId)
        {
            if (MatchCenterManager.Instance.OperationContexts.ContainsKey(channelId))
            {
                LogHelper.WriteDebug("OrderDealRpt.UnRegisterChannel[ClientID=" + channelId + "]");
                OperationContext context = MatchCenterManager.Instance.OperationContexts[channelId];
                lock (((ICollection)MatchCenterManager.Instance.OperationContexts).SyncRoot)
                    MatchCenterManager.Instance.OperationContexts.Remove(channelId);
                if (context != null)
                {
                    var channel = context.Channel;
                    if (channel == null)
                    {
                        return;
                    }
                    channel.DelayDoClose();
                }
            }
        }

        /// <summary>
        /// 检查通道
        /// </summary>
        /// <returns></returns>
        public string CheckChannel()
        {
            return DateTime.Now.ToString();
        }

        /// <summary>
        /// 通道失败异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channel_Faulted(object sender, EventArgs e)
        {
            LogHelper.WriteDebug("******************WCF通道发生异常OrderDealRpt.Channel_Faulted******************");

            var channel = sender as IContextChannel;
            //channel.State = 
            if (channel == null)
                return;

            //意外检查
            channel.DoClose();
        }

        #endregion
    }
}