using System;
using System.ServiceModel;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.BLL.PushBack;
using MatchCenter.BLL.interfaces;
using MatchCenter.Entity;

namespace MatchCenter.BLL.Match
{
   /// <summary>
   /// 撮合中心改单返回类
   /// Create By:王伟
   /// Create Date:2009-10-21
   /// </summary>
    public class ModifyFailHKStockBack
    {
        /// <summary>
        /// 改单失败类实例
        /// </summary>
        private static ModifyFailHKStockBack instance;
        
        /// <summary>
        /// 撮合中心委托缓冲区
        /// </summary>
        private QueueBufferBase<HKModifyEntity> bufferModify;

        /// <summary>
        /// 实例构造函数
        /// </summary>
        private ModifyFailHKStockBack()
        {
            bufferModify = new QueueBufferBase<HKModifyEntity>();
            bufferModify.QueueItemProcessEvent += ProcessModify;
        }

        /// <summary>
        /// 改单失败类单一静态类实例
        /// </summary>
        public static ModifyFailHKStockBack Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ModifyFailHKStockBack();
                }
                return instance;
            }
        }
        
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="modifyEntity">撤单实体</param>
        public  void Add(HKModifyEntity modifyEntity)
        {
            bufferModify.InsertQueueItem(modifyEntity);
        }

        /// <summary>
        /// 撮合中心改单
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ProcessModify(object sender, QueueItemHandleEventArg<HKModifyEntity> e)
        {
            ModifyOrder(e.Item);
        }

        /// <summary>
        /// 撤消委托单
        /// </summary>
        /// <param name="model">委托实体</param>
        public void ModifyOrder(HKModifyEntity model)
        {
            LogHelper.WriteDebug("[撮合单元运行撤销委托单方法]");
            //委托实体为空
            if (model == null)
            {
                return;
            }
            //bool cancel = false;
            string code = Guid.NewGuid().ToString();
            var dataEntity = new HKModifyBackEntity();
            //id
            dataEntity.Id = code;
            //委托单号码
            dataEntity.OrderNo = model.OldOrderNo;
            //通道号码
            dataEntity.ChannelNo = model.ChannelNo;
            decimal cancount = 0.00m;
            dataEntity.IsSuccess = false;
            dataEntity.Message = "改单失败，委托单不存在。";
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
                    callback.ModifyHKStockOrderRpt(dataEntity);
                }
                LogHelper.WriteDebug(" 改单单失败[" + "委托id=" + code + "委托代码=" + model.StockCode + ",撤单时间=" + DateTime.Now + ",委托单号码=" + model.OldOrderNo + ",撤单数量=" + cancount + "]");

            }
            catch (Exception ex)
            {
                LogHelper.WriteError("CH-100:撤单wcf服务通道异常", ex);
                //TradePushBackImpl.Instanse.SaveCanceBack(dataEntity);

            }
        }
    }

    }

