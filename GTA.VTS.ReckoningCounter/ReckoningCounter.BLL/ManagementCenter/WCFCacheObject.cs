#region Using Namespace

using System;
using System.Collections.Generic;
using GTA.VTS.Common.CommonObject;

#endregion

namespace ReckoningCounter.BLL.ManagementCenter
{
    /// <summary>
    /// 泛型接口帮助类，适用于WCF接口同时提供GetAll和GetByKey方法的对象类
    /// 作者：宋涛
    /// 日期：2008-12-05
    /// </summary>
    /// <typeparam name="K">K</typeparam>
    /// <typeparam name="V">V</typeparam>
    public class WCFCacheObject<K, V> where V : class
    {
        private ObjectCache<K, V> cacheList = new ObjectCache<K, V>();
        private Func<IList<V>> getAllFromWCFFunc;
        private Func<K, V> getByKeyFromWCFFunc;
        private Func<V, K> GetKeyFunc;


        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="getAllFromWCF">获取全部对象的WCF接口方法</param>
        /// <param name="getByKeyFromWCF">根据Key获取对象的WCF接口方法</param>
        /// <param name="getKey">获取对象Key的谓词</param>
        public WCFCacheObject(Func<IList<V>> getAllFromWCF, Func<K, V> getByKeyFromWCF, Func<V, K> getKey)
        {
            this.getByKeyFromWCFFunc = getByKeyFromWCF;
            this.getAllFromWCFFunc = getAllFromWCF;
            this.GetKeyFunc = getKey;
        }

        /// <summary>
        /// 进行重置
        /// </summary>
        public void Reset()
        {
            if (cacheList == null)
                return;

            lock (cacheList)
            {
                cacheList.Reset();
            }
        }

        /// <summary>
        /// 获取全部对象，如果列表为空，那么从WCF加载
        /// </summary>
        /// <returns>对象列表</returns>
        public IList<V> GetAll()
        {
            return GetAll(false);
        }

        /// <summary>
        /// 获取全部对象，如果列表为空，那么从WCF加载；如果reload，那么重新从WCF加载
        /// </summary>
        /// <param name="reLoad">是否从WCF加载</param>
        /// <returns>对象列表</returns>
        public IList<V> GetAll(bool reLoad)
        {
            lock (cacheList)
            {
                if (reLoad || !cacheList.HasData)
                {
                    IList<V> list = getAllFromWCFFunc();

                    cacheList.Fill(list, GetKeyFunc);
                }
            }

            return cacheList.List;
        }

        /// <summary>
        /// 根据key获取对象，首先在cache列表中查找，然后再从WCF查找
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>对象</returns>
        public V GetByKey(K key)
        {
            V val = null;
            if (cacheList.HasData)
            {
                val = cacheList.GetByKey(key);

                if (val == null)
                {
                    val = getByKeyFromWCFFunc(key);

                    if (val != null)
                        cacheList.Add(val, key);
                }
            }
            else
            {
                GetAll();
                val = cacheList.GetByKey(key);
            }

            return val;
        }
    }

    /// <summary>
    /// 泛型接口帮助类，适用于WCF接口只提供GetAll方法的对象类,并暴露GetAll，GetByKey
    /// </summary>
    /// <typeparam name="K">K</typeparam>
    /// <typeparam name="V">V</typeparam>
    public class WCFCacheObjectWithGetAll<K, V> where V : class
    {
        private ObjectCache<K, V> cacheList = new ObjectCache<K, V>();
        private Func<IList<V>> getAllFromWCFFunc;
        private Func<V, K> GetKeyFunc;


        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="getAllFromWCF">获取全部对象的WCF接口方法</param>
        /// <param name="getKey">获取对象Key的谓词</param>
        public WCFCacheObjectWithGetAll(Func<IList<V>> getAllFromWCF, Func<V, K> getKey)
        {
            this.getAllFromWCFFunc = getAllFromWCF;
            this.GetKeyFunc = getKey;
        }

        /// <summary>
        /// 进行重置
        /// </summary>
        public void Reset()
        {
            if (cacheList == null)
                return;

            lock (cacheList)
            {
                cacheList.Reset();
            }
        }

        /// <summary>
        /// 获取全部对象，如果列表为空，那么从WCF加载
        /// </summary>
        /// <returns>对象列表</returns>
        public IList<V> GetAll()
        {
            return GetAll(false);
        }

        /// <summary>
        /// 获取全部对象，如果列表为空，那么从WCF加载；如果reload，那么重新从WCF加载
        /// </summary>
        /// <param name="reLoad">是否从WCF加载</param>
        /// <returns>对象列表</returns>
        public IList<V> GetAll(bool reLoad)
        {
            lock (cacheList)
            {
                if (reLoad || !cacheList.HasData)
                {
                    IList<V> list = getAllFromWCFFunc();

                    cacheList.Fill(list, GetKeyFunc);
                }
            }


            return cacheList.List;
        }


        /// <summary>
        /// 根据key获取对象，在cache列表中查找
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>对象</returns>
        public V GetByKey(K key)
        {
            V val = null;
            if (!cacheList.HasData)
            {
                GetAll();
            }

            val = cacheList.GetByKey(key);

            return val;
        }
    }

    /// <summary>
    /// 泛型接口帮助类，适用于WCF接口只提供GetByKey方法的对象类
    /// </summary>
    /// <typeparam name="K">K</typeparam>
    /// <typeparam name="V">V</typeparam>
    public class WCFCacheObjectWithGetKey<K, V> where V : class
    {
        private ObjectCache<K, V> cacheList = new ObjectCache<K, V>();
        private Func<K, V> getByKeyFromWCFFunc;


        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="getByKeyFromWCF">根据Key获取对象的WCF接口方法</param>
        public WCFCacheObjectWithGetKey(Func<K, V> getByKeyFromWCF)
        {
            this.getByKeyFromWCFFunc = getByKeyFromWCF;
        }

        /// <summary>
        /// 进行重置
        /// </summary>
        public void Reset()
        {
            if (cacheList == null)
                return;

            lock (cacheList)
            {
                cacheList.Reset();
            }
        }

        /// <summary>
        /// 根据key获取对象，首先在cache列表中查找，然后再从WCF查找
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>对象</returns>
        public V GetByKey(K key)
        {
            V val = null;

            lock (cacheList)
            {
                if (cacheList.HasData)
                {
                    val = cacheList.GetByKey(key);

                    if (val == null)
                    {
                        val = getByKeyFromWCFFunc(key);

                        if (val != null)
                            cacheList.Add(val, key);
                    }
                }
                else
                {
                    val = getByKeyFromWCFFunc(key);

                    if (val != null)
                        cacheList.Add(val, key);
                }
            }

            return val;
        }
    }

    /// <summary>
    /// 泛型接口帮助类，适用于WCF接口只提供GetAll方法的对象类,并且对象无key（即不能用字典）
    /// </summary>
    /// <typeparam name="V">V</typeparam>
    public class WCFCacheObjectWithGetAllNoKey<V> where V : class
    {
        private IList<V> cacheList = new List<V>();
        private Func<IList<V>> getAllFromWCFFunc;


        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="getAllFromWCF">获取全部对象的WCF接口方法</param>
        public WCFCacheObjectWithGetAllNoKey(Func<IList<V>> getAllFromWCF)
        {
            this.getAllFromWCFFunc = getAllFromWCF;
        }

        /// <summary>
        /// 进行重置
        /// </summary>
        public void Reset()
        {
            if(cacheList == null)
                return;

            lock (cacheList)
            {
                cacheList.Clear();
            }
        }

        /// <summary>
        /// 获取全部对象，如果列表为空，那么从WCF加载
        /// </summary>
        /// <returns>对象列表</returns>
        public IList<V> GetAll()
        {
            return GetAll(false);
        }

        /// <summary>
        /// 获取全部对象，如果列表为空，那么从WCF加载；如果reload，那么重新从WCF加载
        /// </summary>
        /// <param name="reLoad">是否从WCF加载</param>
        /// <returns>对象列表</returns>
        public IList<V> GetAll(bool reLoad)
        {
            lock(cacheList)
            {
                if(cacheList == null)
                    cacheList = new List<V>();

                if(reLoad || cacheList.Count == 0)
                {
                    cacheList = getAllFromWCFFunc();
                }
            }

            return cacheList;
        }
    }
}