using System;
using System.ServiceModel;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.interfaces;
using MatchCenter.Entity;

namespace MatchCenter.BLL.match
{
    /// <summary>
    /// 撮合中心期货撤单失败返回类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
    public class CanceFailFutureBack : CanceFailBack
    {
      
        //撮合中心委托缓冲区
        private  QueueBufferBase<CancelEntity> bufferFutureCancel;

        /// <summary>
        /// 构造函数 初始化撮合中心撤单委托缓冲区
        /// </summary>
        public CanceFailFutureBack()
        {
           
            bufferFutureCancel = new QueueBufferBase<CancelEntity>();
            bufferFutureCancel.QueueItemProcessEvent += ProcessFutureCancel;
        }

        /// <summary>
        /// 添加撤单实体
        /// </summary>
        /// <param name="cancelEntity">撤单实体</param>
        public override void Add(CancelEntity cancelEntity)
        {
            bufferFutureCancel.InsertQueueItem(cancelEntity);
        }

        /// <summary>
        /// 撤单方法
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
          private void ProcessFutureCancel(object sender, QueueItemHandleEventArg<CancelEntity> e)
          {
              CancelOrder(e.Item);
          }

        /// <summary>
        /// 撤销委托单
        /// </summary>
        /// <param name="model">委托单</param>
        public void CancelOrder(CancelEntity model)
        {
            if (model == null)
            {
                return ;
            }
            //bool cancel = false;
            var dataEntity = new CancelOrderEntity();
            dataEntity.OrderNo = model.OldOrderNo;
            string code = Guid.NewGuid().ToString();
            //id
            dataEntity.Id = code;
            //撮合中心委托单号
            if (string.IsNullOrEmpty(model.OldOrderNo))
            {
                return ;
            }
            decimal cancount = 0.00m;
           dataEntity.IsSuccess = false;
           dataEntity.Message = "撤单失败，委托单不存在。";
            OperationContext context = null;
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey(model.ChannelNo))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }

            }
            //撮合中心上下文不能为空
            if (context == null)
            {
                return ;
            }
            try
            {

                dataEntity.OrderVolume = 0.00m;
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    //var md = new CancelOrderDelegate(callback.CancelStockIndexFuturesOrderRpt);
                    //md.BeginInvoke(dataEntity, null, null);
                    callback.CancelStockIndexFuturesOrderRpt(dataEntity);
                }
                LogHelper.WriteDebug(" 撤单失败[" + "委托id=" + code + "委托代码=" + model.StockCode + ",撤单时间=" + DateTime.Now + ",委托单号码=" + model.OldOrderNo + ",撤单数量=" + cancount + "]");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-0001:撤单失败wcf服务通道阻塞", ex);
                return ;
            }
          
        }
    }
}
