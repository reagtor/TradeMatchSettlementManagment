using System;
using System.ServiceModel;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.PushBack;
using MatchCenter.BLL.interfaces;
using MatchCenter.Entity;

namespace MatchCenter.BLL.match
{
    /// <summary>
    /// 撮合中心港股撤单失败返回类
    /// Create BY：王伟
    /// Create Date：2009-10-20
    /// </summary>
    public class CanceFailHKBack : CanceFailBack
    {
        //撮合中心委托缓冲区
        private QueueBufferBase<CancelEntity> bufferCancel;
        /// <summary>
        /// 实例构造函数
        /// </summary>
        public CanceFailHKBack()
        {
            bufferCancel = new QueueBufferBase<CancelEntity>();
            bufferCancel.QueueItemProcessEvent += ProcessHKCancel;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="cancelEntity">撤单实体</param>
        public override void Add(CancelEntity cancelEntity)
        {
            bufferCancel.InsertQueueItem(cancelEntity);
        }

        /// <summary>
        /// 撮合中心撤单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessHKCancel(object sender, QueueItemHandleEventArg<CancelEntity> e)
        {
            CancelOrder(e.Item);
        }
        /// <summary>
        /// 撤消委托单
        /// </summary>
        /// <param name="model">委托实体</param>
        public void CancelOrder(CancelEntity model)
        {
            LogHelper.WriteDebug("[撮合单元运行港股撤单失败后回推失败信息方法]");
            //委托实体为空
            if (model == null)
            {
                return;
            }
            //bool cancel = false;
            string code = Guid.NewGuid().ToString();
            var dataEntity = new CancelOrderEntity();
            //id
            dataEntity.Id = code;
            //委托单号码
            dataEntity.OrderNo = model.OldOrderNo;
            //通道号码
            dataEntity.ChannelNo = model.ChannelNo;
            decimal cancount = 0.00m;
            dataEntity.IsSuccess = false;
            dataEntity.Message = "撤单失败，委托单不存在。";
            OperationContext context = null;
            //通道号码不能为空
            if (model.ChannelNo != null)
            {
                if (MatchCenterManager.Instance.OperationContexts.ContainsKey((model.ChannelNo)))
                {
                    context = MatchCenterManager.Instance.OperationContexts[model.ChannelNo];
                }

            }
            //上下文不能为空
            if (context == null)
            {
                return;
            }
            try
            {
                var callback = context.GetCallbackChannel<IDoOrderCallback>();

                if (callback != null)
                {
                    callback.CancelHKStockOrderRpt(dataEntity);
                }
                LogHelper.WriteDebug(" 撤单失败[" + "委托id=" + code + "委托代码=" + model.StockCode + ",撤单时间=" + DateTime.Now + ",委托单号码=" + model.OldOrderNo + ",撤单数量=" + cancount + "]");

            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-100:撤单wcf服务通道异常", ex);
                TradePushBackImpl.Instanse.SaveHKCancelBack(dataEntity);

            }
        }
    }
}
