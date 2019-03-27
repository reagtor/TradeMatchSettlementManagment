using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTA.VTS.Common.CommonUtility;
using MatchCenter.Entity;

namespace MatchCenter.BLL.RealTime
{
    /// <summary>
    /// Title:行情缓存类
    /// Create By:李健华
    /// Create Date:2009-09-16
    /// </summary>
    public class RealTimeDataCache
    {

        #region 定义
        /// <summary>
        ///  行情缓存区
        /// </summary>
        private List<FutureMarketEntity> realTimeCache;
        /// <summary>
        /// 撮合中心缓冲区
        /// </summary>
        public QueueBufferBase<FutureMarketEntity> bufferRealTime;

        /// <summary>
        /// 读写锁
        /// 用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问。
        /// </summary>
        private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public RealTimeDataCache()
        {
            realTimeCache = new List<FutureMarketEntity>();
            bufferRealTime = new QueueBufferBase<FutureMarketEntity>();
        }
        #region 添加、删除操作
        /// <summary>
        /// 撮合中心清除数据
        /// </summary>
        public void Clear()
        {
            rwLock.EnterWriteLock();
            try
            {
                if (realTimeCache != null)
                {
                    realTimeCache.Clear();
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 添加委托到数据存储区
        /// </summary>
        /// <param name="dataX">委托单</param>
        public void Add(FutureMarketEntity dataX)
        {
            if (realTimeCache == null)
            {
                return;
            }
            rwLock.EnterWriteLock();
            try
            {
                realTimeCache.Add(dataX);
                realTimeCache.Sort(Compare);
                FutureMarketEntity item = new FutureMarketEntity();
                item = realTimeCache[0];
                realTimeCache.Remove(dataX);
                bufferRealTime.InsertQueueItem(item);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 按行情时间排序
        /// </summary>
        /// <param name="dataX"></param>
        /// <param name="dataY"></param>
        /// <returns></returns>
        private int Compare(FutureMarketEntity dataX, FutureMarketEntity dataY)
        {
            return dataX.HQReachTime.CompareTo(dataY.HQReachTime);
        }

        #endregion
    }
}
