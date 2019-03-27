#region Using Namespace

using System.Collections.Generic;
using System.Threading;

#endregion

namespace GTA.VTS.Common.CommonObject
{
    /// <summary>
    /// 同步的缓存类
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SyncCache<K, V>
    {
        #region AddOrUpdateStatus enum

        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        } ;

        #endregion

        protected ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        protected Dictionary<K, V> innerCache = new Dictionary<K, V>();

        public V Get(K key)
        {
            V result = default(V);
            cacheLock.EnterReadLock();
            try
            {
                if (innerCache.ContainsKey(key))
                {
                    result = innerCache[key];
                }
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return result;
        }

        public ICollection<V> GetAll()
        {
            ICollection<V> result;
            cacheLock.EnterReadLock();
            try
            {
                result = innerCache.Values;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return result;
        }

        public bool Contains(K key)
        {
            bool result = false;
            cacheLock.EnterReadLock();
            try
            {
                result = innerCache.ContainsKey(key);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return result;
        }

        public bool TryGetValue(K key, out V value)
        {
            bool result = false;
            cacheLock.EnterReadLock();
            try
            {
                result = innerCache.TryGetValue(key, out value);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return result;
        }

        public bool Add(K key, V value)
        {
            bool result = false;
            cacheLock.EnterWriteLock();
            try
            {
                if (!innerCache.ContainsKey(key))
                {
                    innerCache.Add(key, value);
                    result = true;
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }

            return result;
        }

        public bool AddWithTimeout(K key, V value, int timeout)
        {
            bool result = false;
            if (cacheLock.TryEnterWriteLock(timeout))
            {
                try
                {
                    if (!innerCache.ContainsKey(key))
                    {
                        innerCache.Add(key, value);
                        result = true;
                    }
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }

            return result;
        }

        public AddOrUpdateStatus AddOrUpdate(K key, V value)
        {
            cacheLock.EnterUpgradeableReadLock();
            try
            {
                V result;
                if (innerCache.TryGetValue(key, out result))
                {
                    if (Equals(result, value))
                    {
                        return AddOrUpdateStatus.Unchanged;
                    }
                    else
                    {
                        cacheLock.EnterWriteLock();
                        try
                        {
                            innerCache[key] = value;
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    cacheLock.EnterWriteLock();
                    try
                    {
                        innerCache.Add(key, value);
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                    return AddOrUpdateStatus.Added;
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        public bool Delete(K key)
        {
            bool result = false;

            cacheLock.EnterWriteLock();
            try
            {
                if (innerCache.ContainsKey(key))
                {
                    innerCache.Remove(key);
                    result = true;
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }

            return result;
        }

        public void Reset()
        {
            cacheLock.EnterWriteLock();
            try
            {
                innerCache.Clear();
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
    }

    /// <summary>
    /// 同步缓存类2
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SyncCache2<K, V> : SyncCache<K, V> where V : class
    {
        /// <summary>
        /// 根据key获取value，当获取不到时，通过getBykey方法获取value，并插入缓存，
        /// 同时返回给调用者
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="getByKey">GetByKey类型的方法委托</param>
        /// <returns>value</returns>
        public V GetWithAdd(K key, GetByKey<K, V> getByKey)
        {
            cacheLock.EnterUpgradeableReadLock();
            V result;
            try
            {
                if (innerCache.ContainsKey(key))
                {
                    result = innerCache[key];
                }
                else
                {
                    result = getByKey(key);
                    if (result != null)
                    {
                        cacheLock.EnterWriteLock();
                        try
                        {
                            innerCache[key] = result;
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                    }
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }

            return result;
        }
    }

    public delegate V GetByKey<K, V>(K k);
}