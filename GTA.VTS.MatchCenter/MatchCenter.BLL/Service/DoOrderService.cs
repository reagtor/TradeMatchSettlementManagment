using System;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.interfaces;
using MatchCenter.Entity;
using System.ServiceModel;
using System.Collections;
using GTA.VTS.Common.CommonObject;
using MatchCenter.Entity.HK;

namespace MatchCenter.BLL.Service
{
    /// <summary>
    /// 撮合中心下单撤单实现契约
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Desc.：添加港股相关的方法公开接口和相关实现
    /// Update By:李健华
    /// Update Date:2009-10-19 
    /// Desc.：添加商品期货相关的方法公开接口和相关实现
    /// Update By: 董鹏
    /// Update Date:2010-01-22
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DoOrderService : IDoOrder
    {
        #region 下单
        #region 现货下单报盘
        /// <summary>
        /// 现货下单
        /// </summary>
        /// <param name="dataOrder">委托单</param>
        /// <returns></returns>
        public ResultDataEntity DoStockOrder(StockOrderEntity dataOrder)
        {
            //下单通道
            LogHelper.WriteDebug("[调用现货下单通道服务]");
            if (dataOrder == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(dataOrder.SholderCode))
            {
                return null;
            }
            //撮合中心委托单
            StockDataOrderEntity data = new StockDataOrderEntity();
            data.ChannelNo = dataOrder.ChannelNo;
            data.IsMarketPrice = (int)dataOrder.IsMarketPrice;
            data.StockCode = dataOrder.StockCode;
            data.OrderPrice = dataOrder.OrderPrice;
            data.ReachTime = System.DateTime.Now;
            data.OrderVolume = dataOrder.OrderVolume;
            data.TransactionDirection = (int)dataOrder.TransactionDirection;
            data.SholderCode = dataOrder.SholderCode;
            return MatchCenterManager.Instance.DoStockOrder(data);
        }
        #endregion

        #region  股指期货下单报盘
        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="dataOrder">股指期货下单实体</param>
        /// <returns></returns>
        public ResultDataEntity DoFutureOrder(FutureOrderEntity dataOrder)
        {
            if (dataOrder == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(dataOrder.SholderCode))
            {
                return null;
            }
            FutureDataOrderEntity data = new FutureDataOrderEntity();
            data.ChannelNo = dataOrder.ChannelNo;
            data.IsMarketPrice = (int)dataOrder.IsMarketPrice;
            data.StockCode = dataOrder.StockCode;
            data.OrderPrice = dataOrder.OrderPrice;
            data.ReachTime = System.DateTime.Now;
            data.OrderVolume = dataOrder.OrderVolume;
            data.TransactionDirection = (int)dataOrder.TransactionDirection;
            data.SholderCode = dataOrder.SholderCode;
            data.Direction = dataOrder.Direction;
            return MatchCenterManager.Instance.DoFutureOrder(data);
        }
        #endregion

        #region 商品期货下单 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货下单
        /// </summary>
        /// <param name="dataOrder">商品期货下单实体</param>
        /// <returns></returns>
        public ResultDataEntity DoCommoditiesOrder(CommoditiesOrderEntity dataOrder)
        {
            if (dataOrder == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(dataOrder.SholderCode))
            {
                return null;
            }
            CommoditiesDataOrderEntity data = new CommoditiesDataOrderEntity();
            data.ChannelID = dataOrder.ChannelNo;
            data.IsMarketPrice = (int)dataOrder.IsMarketPrice;
            data.StockCode = dataOrder.StockCode;
            data.OrderPrice = dataOrder.OrderPrice;
            data.ReachTime = System.DateTime.Now;
            data.OrderVolume = dataOrder.OrderVolume;
            data.TransactionDirection = (int)dataOrder.TransactionDirection;
            data.SholderCode = dataOrder.SholderCode;
            data.Direction = dataOrder.Direction;
            return MatchCenterManager.Instance.DoCommoditiesOrder(data);
        }
        #endregion

        #region  港股下单报盘
        /// <summary>
        /// 港股下单报盘
        /// </summary>
        /// <param name="model">港股下单实体</param>
        /// <returns></returns>
        public ResultDataEntity DoHKEntrustOrder(HKOrderEntity model)
        {

            //下单通道
            LogHelper.WriteDebug("[调用港股下单通道服务]");
            if (model == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(model.SholderCode))
            {
                return null;
            }
            //撮合中心委托单
            HKEntrustOrderInfo data = new HKEntrustOrderInfo();
            data.BranchID = model.ChannelNo;
            data.OrderType = (int)model.HKPriceType;
            data.HKSecuritiesCode = model.Code;
            data.OrderPrice = model.OrderPrice;
            data.ReceiveTime = System.DateTime.Now;
            data.OrderVolume = model.OrderVolume;
            data.OldVolume = model.OrderVolume;
            data.TradeType = (int)model.TransactionDirection;
            data.SholderCode = model.SholderCode;
            return MatchCenterManager.Instance.DoHKStockOrder(data);
        }
        #endregion

        #endregion

        #region 撤单
       
        #region 现货撤单
        /// <summary>
        /// 现货撤单
        /// </summary>
        /// <param name="dataOrder">撤销委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelStockOrder(CancelEntity dataOrder)
        {
            if (dataOrder == null)
            {
                return null;
            }
            return MatchCenterManager.Instance.CancelOrder(dataOrder);
        }
        #endregion

        #region 股指期货撤单
        /// <summary>
        /// 股指期货撤单
        /// </summary>
        /// <param name="dataOrder">委托单</param>
        /// <returns></returns>
        public CancelResultEntity CancelFutureOrder(CancelEntity dataOrder)
        {
            //委托单不能为空
            if (dataOrder == null)
            {
                return null;
            }
            return MatchCenterManager.Instance.CancelFutureOrder(dataOrder);
        }
        #endregion

        #region 商品期货委托撤单 add by 董鹏 2010-01-22
        /// <summary>
        ///  商品期货委托撤单
        /// </summary>
        /// <param name="dataOrder"></param>
        /// <returns></returns>
        public CancelResultEntity CancelCommoditiesOrder(CancelEntity dataOrder)
        {
            //委托单不能为空
            if (dataOrder == null)
            {
                return null;
            }
            return MatchCenterManager.Instance.CancelCommoditiesOrder(dataOrder);
        }
        #endregion

        #region 港股撤单
        /// <summary>
        /// 港股撤单
        /// </summary>
        /// <param name="dataOrder">撤单实体</param>
        /// <returns></returns>
        public CancelResultEntity CancelHKOrder(CancelEntity dataOrder)
        {

            if (dataOrder == null)
            {
                return null;
            }

            return MatchCenterManager.Instance.CancelHKOrder(dataOrder);

        }
        #endregion

        #endregion

        #region 检查通道
        /// <summary>
        /// 检查通道
        /// </summary>
        /// <returns></returns>
        public string CheckChannel()
        {
            return System.DateTime.Now.ToString();
        }
        #endregion

        #region 改单
        /// <summary>
        /// 港股改单
        /// </summary>
        /// <param name="dataOrder">港股改单实体</param>
        /// <returns></returns>
        public HKModifyResultEntity ModifyHKStockOrder(HKModifyEntity dataOrder)
        {
            if (dataOrder == null)
            {
                return null;
            }
            return MatchCenterManager.Instance.ModifyHKOrder(dataOrder);
        
        }
        #endregion

        #region 未实现或者废弃
        //private object synObj = new object();
        ///// <summary>
        ///// 注销通道
        ///// </summary>
        ///// <param name="channelId">通道ID</param>
        //public void RemoveCallbackChannlId(string channelId)
        //{
        //    LogHelper.WriteDebug("[修改通道服务]");
        //    if (MatchCenterManager.Instance.OperationContexts.ContainsKey(channelId))
        //    {
        //        lock (((ICollection)MatchCenterManager.Instance.OperationContexts).SyncRoot)
        //        {
        //            OperationContext context = MatchCenterManager.Instance.OperationContexts[channelId];

        //            if (context != null)
        //            {
        //                context.Channel.Faulted += Channel_Faulted;
        //            }
        //            lock (((ICollection)MatchCenterManager.Instance.OperationContexts).SyncRoot)
        //                MatchCenterManager.Instance.OperationContexts.Remove(channelId);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 通道失败异常
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Channel_Faulted(object sender, EventArgs e)
        //{
        //    LogHelper.WriteDebug("******************WCF通道发生异常DoOrderService.Channel_Faulted******************");

        //    IContextChannel channel = sender as IContextChannel;
        //    if (channel == null)
        //        return;

        //    //意外检查
        //    channel.DoClose();
        //}
        ///// <summary>
        ///// 注册通道
        ///// </summary>
        ///// <returns></returns>
        //public string GetCallbackChannelId()
        //{
        //    string id = OperationContext.Current.SessionId;
        //    if (!MatchCenterManager.Instance.OperationContexts.ContainsKey(id))
        //    {
        //        lock (((ICollection)MatchCenterManager.Instance.OperationContexts).SyncRoot)
        //        {
        //            OperationContext context = OperationContext.Current;
        //            MatchCenterManager.Instance.OperationContexts[id] = context;
        //            context.Channel.Faulted += Channel_Faulted;
        //        }
        //    }
        //    return id;
        //}
      

        ///// <summary>
        ///// 未实现方法-接收商品期货委托下单
        ///// </summary>
        ///// <param name="dataOrder"></param>
        ///// <returns></returns>
        //public ResultDataEntity DoCommoditiesOrder(CommoditiesDataOrderEntity dataOrder)
        //{
        //    throw new System.NotImplementedException();
        //}
        ///// <summary>
        /////  未实现方法-接收商品期货委托撤单
        ///// </summary>
        ///// <param name="dataOrder"></param>
        ///// <returns></returns>
        //public CancelResultEntity CancelCommoditiesOrder(CancelEntity dataOrder)
        //{
        //    throw new System.NotImplementedException();
        //}
        #endregion

 
    }
}
