#region Using Namespace

using System;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.Reckoning.Instantaneous;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.Entity;

#endregion

namespace ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer
{
    /// <summary>
    /// 撮合中心报盘回调处理类
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class DoCallbackProcess : IOrderDealRptCallback
    {
        #region IOrderDealRptCallback Members

        /// <summary>
        /// 现货成交回报接收
        /// </summary>
        /// <param name="model"></param>
        public void ProcessStockDealRpt(StockDealBackEntity model)
        {
            string desc = CrashManager.GetInstance().GetXHDesc(model);
            LogHelper.WriteDebug("<---现货成交回报接收DoCallbackProcess.ProcessStockDealRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertXHDealBackEntity(model))
            {
                //即时清算
                ReckonCenter.Instace.AcceptXHDealOrder(model);
            }
        }

        /// <summary>
        /// 商品期货成交回报接收
        /// </summary>
        /// <param name="model"></param>
        public void ProcessMercantileDealRpt(CommoditiesDealBackEntity model)
        {
            string desc = CrashManager.GetInstance().GetQHDesc(model);
            LogHelper.WriteDebug("<---商品期货成交回报接收DoCallbackProcess.ProcessMercantileDealRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertQHDealBackEntity(model))
            {
                //即时清算
                ReckonCenter.Instace.AcceptSPQHDealOrder(model);
            }
        }

        /// <summary>
        /// 股指期货成交回报接收
        /// </summary>
        /// <param name="model"></param>
        public void ProcessStockIndexFuturesDealRpt(FutureDealBackEntity model)
        {
            string desc = CrashManager.GetInstance().GetGZQHDesc(model);
            LogHelper.WriteDebug("<---股指期货成交回报接收DoCallbackProcess.ProcessStockIndexFuturesDealRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertGZQHDealBackEntity(model))
            {
                //即时清算
                ReckonCenter.Instace.AcceptGZQHDealOrder(model);
            }
        }

        /// <summary>
        /// 现货异步委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void ProcessStockOrderRpt(ResultDataEntity model)
        {
            string desc = CrashManager.GetInstance().GetResultDesc(model);
            LogHelper.WriteDebug("<---现货异步委托回报价格异常(废单)DoCallbackProcess.AcceptXHErrorOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertXHResultDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptXHErrorOrderRpt(model);
            }
        }

        /// <summary>
        /// 商品期货异步委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void ProcessCommoditiesOrderRpt(ResultDataEntity model)
        {
            string desc = CrashManager.GetInstance().GetResultDesc(model);
            LogHelper.WriteDebug("<---商品期货异步委托回报价格异常(废单)DoCallbackProcess.AcceptSPQHErrorOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertQHResultDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptSPQHErrorOrderRpt(model);
            }
        }

        /// <summary>
        /// 股指期货异步委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void ProcessStockIndexFuturesOrderRpt(ResultDataEntity model)
        {
            string desc = CrashManager.GetInstance().GetResultDesc(model);
            LogHelper.WriteDebug("<---股指期货异步委托回报价格异常(废单)DoCallbackProcess.AcceptGZQHErrorOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertGZQHResultDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptGZQHErrorOrderRpt(model);
            }
        }

        /// <summary>
        /// 现货撤单异步回报
        /// </summary>
        /// <param name="model"></param>
        public void CancelStockOrderRpt(CancelOrderEntity model)
        {
            string desc = CrashManager.GetInstance().GetCancelDesc(model);
            LogHelper.WriteDebug("<---现货撤单异步回报DoCallbackProcess.CancelStockOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertXhCancelDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptCancelXHOrderRpt(model);
            }
        }

        /// <summary>
        /// 商品期货撤单异步回报
        /// </summary>
        /// <param name="model"></param>
        public void CancelCommoditiesOrderRpt(CancelOrderEntity model)
        {
            string desc = CrashManager.GetInstance().GetCancelDesc(model);
            LogHelper.WriteDebug("<---商品期货撤单异步回报DoCallbackProcess.AcceptCancelSPQHOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertQhCancelDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptCancelSPQHOrderRpt(model);
            }
        }

        /// <summary>
        /// 股指期货撤单异步回报
        /// </summary>
        /// <param name="model"></param>
        public void CancelStockIndexFuturesOrderRpt(CancelOrderEntity model)
        {
            string desc = CrashManager.GetInstance().GetCancelDesc(model);
            LogHelper.WriteDebug("<---股指期货撤单异步回报DoCallbackProcess.AcceptCancelGZQHOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertGZQHCancelDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptCancelGZQHOrderRpt(model);
            }
        }

        #endregion

        #region 港股接口成员

        /// <summary>
        /// 港股成交回报接收
        /// </summary>
        /// <param name="model"></param>
        public void ProcessHKStockDealRpt(HKDealBackEntity model)
        {
            string desc = CrashManager.GetInstance().GetHKDesc(model);
            LogHelper.WriteDebug("<---港股成交回报接收DoCallbackProcess.ProcessHKStockDealRpt" + desc);

            if (string.IsNullOrEmpty(model.ID))
                model.ID = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertHKDealBackEntity(model))
            {
                //即时清算
                ReckonCenter.Instace.AcceptHKDealOrder(model);
            }
        }
        /// <summary>
        /// 港股异步委托回报价格异常(废单)
        /// </summary>
        /// <param name="model"></param>
        public void ProcessHKStockOrderRpt(ResultDataEntity model)
        {
            string desc = CrashManager.GetInstance().GetResultDesc(model);
            LogHelper.WriteDebug("<---港股异步委托回报价格异常(废单)DoCallbackProcess.ProcessHKStockOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertHKResultDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptHKErrorOrderRpt(model);
            }
        }
       /// <summary>
       /// 港股撤单回报
       /// </summary>
       /// <param name="model"></param>
        public void CancelHKStockOrderRpt(CancelOrderEntity model)
        {
            string desc = CrashManager.GetInstance().GetCancelDesc(model);
            LogHelper.WriteDebug("<---港股撤单异步回报DoCallbackProcess.CancelHKStockOrderRpt" + desc);

            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            if (CrashManager.GetInstance().InsertHkCancelDealBackEntity(model))
            {
                ReckonCenter.Instace.AcceptCancelHKOrderRpt(model);
            }
        }
        /// <summary>
        /// 港股改单减量委托回报
        /// </summary>
        /// <param name="model"></param>
        public void ModifyHKStockOrderRpt(HKModifyBackEntity model)
        {
            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();

            string format = "M<---港股改单回报接收(类型2改单)DoCallbackProcess.ModifyHKStockOrderRpt[ID={0},IsSuccess={1},OrderNo={2}]";
            string desc = string.Format(format, model.Id, model.IsSuccess, model.OrderNo);
            LogHelper.WriteDebug(desc);

            //回推改单结果到前台
            HKModifyOrderPushBack pushBack = new HKModifyOrderPushBack();
            pushBack.IsSuccess = model.IsSuccess;
            pushBack.Message = model.Message;
            //这是撮合队列的委托ID
            //pushBack.OriginalRequestNumber = model.OrderNo;
            //这里撮合回推回来的回推通道ID不是前台的ID
            //pushBack.CallbackChannlId = model.ChannelNo;
            string entrustNumber = "";
            string channelID = HKDataAccess.GetModifyOrderChannelIDByMacID(model.OrderNo, out entrustNumber);
            pushBack.OriginalRequestNumber = entrustNumber;
            pushBack.NewRequestNumber = entrustNumber;
            pushBack.CallbackChannlId = channelID;

            //pushBack.TradeID = model.

            CounterOrderService.Instance.AcceptHKModifyOrder(pushBack);

            string message = "";
            //如果改单成功的话，那么发送内部的撤单回报
            if (model.IsSuccess)
            {
                var cancelEntity = model.CanleOrderEntity;
                message = "类型2改单成功-" + cancelEntity.Message;

                cancelEntity.Message = "";
                CancelHKStockOrderRpt(cancelEntity);
            }
            else
            {
                message = "类型2改单失败-" + model.Message;
            }

            // var tet = HKDataAccess.GetTodayEntrustTable(model.OrderNo);


            //更新委托信息
            //if (tet != null)
            //{
            //    tet.OrderMessage = message;
            //    HKDataAccess.UpdateEntrustTable(tet);
            //}
            HKDataAccess.UpdateEntrustOrderMessage(entrustNumber, message);
        }

        #endregion



        //void IOrderDealRptCallback.ProcessStockDealRpt(StockDealBackEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessStockDealRpt(StockDealBackEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessStockDealRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ProcessMercantileDealRpt(CommoditiesDealBackEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessMercantileDealRpt(CommoditiesDealBackEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessMercantileDealRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ProcessStockIndexFuturesDealRpt(FutureDealBackEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessStockIndexFuturesDealRpt(FutureDealBackEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessStockIndexFuturesDealRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ProcessHKStockDealRpt(HKDealBackEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessHKStockDealRpt(HKDealBackEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessHKStockDealRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ProcessStockOrderRpt(ResultDataEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessStockOrderRpt(ResultDataEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessStockOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ProcessCommoditiesOrderRpt(ResultDataEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessCommoditiesOrderRpt(ResultDataEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessCommoditiesOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ProcessStockIndexFuturesOrderRpt(ResultDataEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessStockIndexFuturesOrderRpt(ResultDataEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessStockIndexFuturesOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ProcessHKStockOrderRpt(ResultDataEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginProcessHKStockOrderRpt(ResultDataEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndProcessHKStockOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.CancelStockOrderRpt(CancelOrderEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginCancelStockOrderRpt(CancelOrderEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndCancelStockOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.CancelCommoditiesOrderRpt(CancelOrderEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginCancelCommoditiesOrderRpt(CancelOrderEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndCancelCommoditiesOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.CancelStockIndexFuturesOrderRpt(CancelOrderEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginCancelStockIndexFuturesOrderRpt(CancelOrderEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndCancelStockIndexFuturesOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.CancelHKStockOrderRpt(CancelOrderEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginCancelHKStockOrderRpt(CancelOrderEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndCancelHKStockOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //void IOrderDealRptCallback.ModifyHKStockOrderRpt(HKModifyBackEntity model)
        //{
        //    throw new NotImplementedException();
        //}

        IAsyncResult IOrderDealRptCallback.BeginModifyHKStockOrderRpt(HKModifyBackEntity model, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IOrderDealRptCallback.EndModifyHKStockOrderRpt(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}