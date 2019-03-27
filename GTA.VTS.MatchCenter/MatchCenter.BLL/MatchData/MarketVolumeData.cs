using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatchCenter.BLL.MatchData
{
    /// <summary>
    /// Title:市场存量数据缓存类
    /// Desc.: 市场存量缓存Dictionary数据key=用户编号+价格+委托编号
    /// Create by:李健华
    /// Create Date:2009-11-16
    /// </summary>
    public class MarketVolumeData
    {
        /// <summary>
        /// 市场存量缓存Dictionary数据key=用户编号+价格+委托编号
        /// </summary>
        private Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
        /// <summary>
        /// 资源锁
        /// </summary>
        private ReaderWriterLockSlim _dicLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 添加市场存量
        /// </summary>
        /// <param name="key">用户编号+价格+委托编号</param>
        /// <param name="volume">市场存量</param>
        public void AddMarketVolume(string key, decimal volume)
        {
            _dicLock.EnterReadLock();
            bool isContainskey = false;
            try
            {
                isContainskey = dic.ContainsKey(key);
            }
            finally
            {
                _dicLock.ExitReadLock();
            }

            if (!isContainskey)
            {
                _dicLock.EnterWriteLock();
                try
                {

                    dic.Add(key, volume);
                }
                finally
                {
                    _dicLock.ExitWriteLock();
                }
            }
        }
        /// <summary>
        /// 更新（减少）市场存量 如果市场存量为0 直接移除
        /// </summary>
        /// <param name="key">用户编号+价格+委托编号</param>
        /// <param name="volume">要减少的市场存量</param>
        public void ModifyMarketVolume(string key, decimal volume)
        {

            _dicLock.EnterWriteLock();
            try
            {
                if (dic.ContainsKey(key))
                {
                    decimal values = dic[key];
                    values = values - volume;
                    if (values > 0)
                    {
                        dic[key] = values;
                    }
                    else
                    {
                        dic.Remove(key);
                    }
                }
            }
            finally
            {
                _dicLock.ExitWriteLock();
            }

        }

        /// <summary>
        /// 根据Key获取市场存量key=用户编号+价格+委托编号
        /// </summary>
        /// <param name="key">key=用户编号+价格+委托编号</param>
        /// <returns>返回获取到的市存量</returns>
        public decimal GetMarketVolume(string key)
        {
            decimal volume = 0;
            _dicLock.EnterReadLock();
            try
            {
                if (dic.ContainsKey(key))
                {
                    volume = dic[key];
                }
            }
            finally
            {
                _dicLock.ExitReadLock();
            }
            return volume;
        }



    }
}
