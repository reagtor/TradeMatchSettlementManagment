#region Using Namespace

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#endregion

namespace GTA.VTS.Common.CommonUtility
{
    public class QueueItemHandleEventArg<TQueueItem> : EventArgs
    {
        public QueueItemHandleEventArg(TQueueItem itemPara)
        {
            this.Item = itemPara;
        }

        public TQueueItem Item { get; private set; }
    }


    public class QueueItemEx<TQueueItem>
    {
        public QueueItemEx(TQueueItem item, EventHandler<QueueItemHandleEventArg<TQueueItem>> handler)
        {
            this.Item = item;
            this.Handler = handler;
        }

        public TQueueItem Item { get; private set; }

        public EventHandler<QueueItemHandleEventArg<TQueueItem>> Handler { get; private set; }
    }

    /// <summary>
    /// 数据缓冲处理基类
    /// </summary>
    /// <typeparam name="TQueueItem"></typeparam>
    public class QueueBufferBase<TQueueItem>
    {
        private bool _isWorking;
        private Queue<TQueueItem> _itemBuffer = new Queue<TQueueItem>();

        /// <summary>
        /// 缓冲区元素个数
        /// </summary>
        public int BufferedItemCount
        {
            get { return _itemBuffer.Count; }
        }

        public string Name { get; set; }

        public event EventHandler<QueueItemHandleEventArg<TQueueItem>> QueueItemProcessEvent;

        /// <summary>
        /// 处理业务回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessBussiness(object sender, QueueItemHandleEventArg<TQueueItem> e)
        {
            try
            {
                if (this.QueueItemProcessEvent != null)
                    QueueItemProcessEvent(this, e);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 处理结束的回调的线程管理
        /// </summary>
        /// <param name="ar"></param>
        private void ProcessCallback(IAsyncResult ar)
        {
            ThreadPool.QueueUserWorkItem(this.CallBackProcess, ar);
        }

        /// <summary>
        /// 处理结束的回调
        /// </summary>
        protected void CallBackProcess(object state)
        {
            IAsyncResult ar = (IAsyncResult) state;
            QueueItemEx<TQueueItem> qie = (QueueItemEx<TQueueItem>) ar.AsyncState;
            qie.Handler.EndInvoke(ar);

            lock (((ICollection) this._itemBuffer).SyncRoot)
            {
                if (this._itemBuffer.Count > 0)
                {
                    TQueueItem item = this._itemBuffer.Dequeue();
                    EventHandler<QueueItemHandleEventArg<TQueueItem>> handler
                        = this.ProcessBussiness;
                    QueueItemEx<TQueueItem> itemEx = new QueueItemEx<TQueueItem>(item, handler);
                    handler.BeginInvoke(this, new QueueItemHandleEventArg<TQueueItem>(item), ProcessCallback, itemEx);
                }
                else
                {
                    this._isWorking = false;
                }
            }
        }

        /// <summary>
        /// 向缓冲区中加入数据
        /// </summary>
        /// <param name="item"></param>
        public virtual void InsertQueueItem(TQueueItem item)
        {
            lock (((ICollection) this._itemBuffer).SyncRoot)
            {
                if (this._isWorking)
                {
                    this._itemBuffer.Enqueue(item);
                }
                else
                {
                    EventHandler<QueueItemHandleEventArg<TQueueItem>> handler = this.ProcessBussiness;
                    QueueItemEx<TQueueItem> itemEx = new QueueItemEx<TQueueItem>(item, handler);
                    handler.BeginInvoke(this, new QueueItemHandleEventArg<TQueueItem>(item), ProcessCallback, itemEx);
                    this._isWorking = true;
                }
            }
        }
    }
}