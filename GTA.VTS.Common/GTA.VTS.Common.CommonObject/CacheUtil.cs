using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace GTA.VTS.Common.CommonObject
{
    /// <summary>
    /// Create Date:2009-08-18
    /// Create By:李健华
    ///  缓存公共单元
    ///  本类所有方法都使用静态方法锁定类型，而实例方法锁定实例。
    ///  在任何实例函数中只能有一个线程执行，并且在任何类的静态函数中只能有一个线程执行加锁
    /// </summary>
    public class CacheUtil<K, V> where V : class
    {
        private Dictionary<K, V> dict;
        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public CacheUtil()
        {
            dict = new Dictionary<K, V>();
        }
        /// <summary>
        /// 是否有数据
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool HasValue()
        {
            return dict.Count > 0;
        }
        /// <summary>
        /// 当前缓存数据记录总数据
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Count()
        {
            return dict.Count;
        }

        /// <summary>
        /// 添加一条记录，如果已经存在则先删除该记录再添加
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">要添回的记录</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(K key, V value)
        {
            if (value == null)
                return;
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
            dict.Add(key, value);
        }
        /// <summary>
        /// 向记录中添加多条记录，如果其中有某条记录已经，则先删除再添加。
        /// </summary>
        /// <param name="key">key（记录key委托方法val=>val.id）</param>
        /// <param name="value">要添加的记录列表</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(Func<V, K> key, List<V> value)
        {
            if (value == null)
                return;
            foreach (V item in value)
            {
                K addkey = key(item);
                this.Add(addkey, item);
            }
        }
        /// <summary>
        /// 用一组数据列表初始化缓存数据,即内部先调用Reset()方法请空后再逐条添加
        /// </summary>
        /// <param name="key">key（记录key委托方法val=>val.id）</param>
        /// <param name="value">要添加的记录列表</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Fill(Func<V, K> key, List<V> value)
        {
            if (value == null)
                return;
            this.Reset();
            foreach (V item in value)
            {
                K addkey = key(item);
                this.Add(addkey, item);
            }
        }
        /// <summary>
        /// 删除一个一个特定记录
        /// </summary>
        /// <param name="key">记录key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove(K key)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
        }

        /// <summary>
        /// 是否存在指定key的记录
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>是否存在</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Contains(K key)
        {
            if (dict.ContainsKey(key))
                return true;

            return false;
        }

        /// <summary>
        /// 重置缓存(即清空所有缓存数据)
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Reset()
        {
            dict.Clear();
        }

        /// <summary>
        /// 根据key获取记录
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public V GetByKey(K k)
        {
            V v = default(V);
            if (dict.ContainsKey(k))
                v = dict[k];
            return v;
        }
        /// <summary>
        /// 根据获取当前所有缓存记录
        /// [MethodImpl(MethodImplOptions.Synchronized)]这里不用上锁，即使有其他线程在添加数据，
        /// 但添加数据已经是锁定当前实例对象，所以也不可以在使用获取
        /// </summary>
        /// <returns></returns>
        public List<V> GetAll()
        {
            List<V> v = new List<V>();
            foreach (var item in dict)
            {
                // v.Add(dict[(K)((DictionaryEntry)item).Key]);
                v.Add(item.Value);
            }
            return v;
        }

    }
}
