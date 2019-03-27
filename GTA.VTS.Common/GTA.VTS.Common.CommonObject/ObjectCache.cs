#region Using Namespace

using System;
using System.Collections.Generic;

#endregion

namespace GTA.VTS.Common.CommonObject
{
    /// <summary>
    /// 列表对象缓存类，速度优先
    /// 作者：宋涛
    /// 日期：2008-11-23
    /// 
    /// 使用方法：
    /// 假设某个接口返回一系列对象，而这些对象并不是频繁更新的，那么我们可以将这些对象缓存到本地，
    /// 并提供快速访问的接口。
    /// 例：
    /// class MyObject
    /// {
    ///     public int ID{get;set;}
    ///     public string Name {get;set;}
    /// }
    /// 
    /// 使用本类：
    /// 1.初始化
    /// ObjectCache<int,MyObject> cache = new ObjectCache<int,Cache>();
    /// IList<MyObject> list = GetList();...
    /// cache.Fill(list,val=>val.ID);
    /// cache.FillWithNoReset(list,val=>val.ID);
    /// 
    /// 2.调用
    /// 获取所有对象：
    /// IList<MyObject> list = cache.List;
    /// 根据key获取对象：
    /// MyObject val = cache.GetByKey(1);
    /// 
    /// 3.单个对象操作
    /// cache.Add(myObject,val.ID);
    /// cache.Remove(myObject,val.ID);
    /// 
    /// 4.获取对象key的谓词
    /// 此表达式是为了设置内部的字典以什么key来进行查找，可以使用对象的id，也可以单独指定
    /// 如：
    /// val=>val.ID  //以MyObject.ID字段作为key
    /// 
    /// val=>1      //以1为key（这里可以使用自定义的序列）
    /// </summary>
    /// <typeparam name="K">对象的key</typeparam>
    /// <typeparam name="V">对象</typeparam>
    public class ObjectCache<K, V> where V : class
    {
        public IDictionary<K, V> dict;

        /// 是否正在Fill（调用Fill方法）
        private bool isFilling;

        public IList<V> valuesList;

        public ObjectCache()
        {
            valuesList = new List<V>();
            dict = new Dictionary<K, V>();
        }

        public IList<V> List
        {
            get { return this.valuesList; }
        }

        /// <summary>
        /// 是否有数据
        /// </summary>
        public bool HasData
        {
            get
            {
                return dict.Count > 0 && valuesList.Count > 0;
            }
        }

        /// <summary>
        /// 批量填充内部列表，使用：Fill(someObjects, someObject=>someObject.Key);
        /// 只能第一次初始化时使用，再次使用将会把原来的对象全部清除
        /// </summary>
        /// <param name="values">对象列表</param>
        /// <param name="getKey">获取对象key的谓词</param>
        public void Fill(IList<V> values, Func<V, K> getKey)
        {
            if (isFilling)
            {
                return;
            }



            if (values == null)
            {
                return;
            }

            if (values.Count == 0)
            {
                return;
            }
            isFilling = true;

            Reset();


            this.valuesList = values;

            for (int i = 0; i < values.Count; i++)
            {
                V val = values[i];
                K key = getKey(val);
                dict[key] = val;
            }

            isFilling = false;
        }

        ///// <summary>
        ///// 批量填充内部列表，不会清除原来的列表，可以在任何时间使用
        ///// </summary>
        ///// <param name="values">对象列表</param>
        ///// <param name="getKey">获取对象key的谓词</param>
        //public void FillWithNoReset(IList<V> values, Func<V, K> getKey)
        //{
        //    if (values == null)
        //        return;


        //    for (int i = 0; i < values.Count; i++)
        //    {
        //        V val = values[i];
        //        K key = getKey(val);

        //        //add to list
        //        if (valuesList.Contains(val))
        //            valuesList.Remove(val);

        //        valuesList.Add(val);

        //        //add to dict
        //        dict[key] = val;
        //    }
        //}

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="val">对象</param>
        /// <param name="key">key</param>
        public void Add(V val, K key)
        {
            if (val == null)
                return;

            //add to list
            if (valuesList.Contains(val))
                valuesList.Remove(val);

            valuesList.Add(val);

            dict[key] = val;
        }

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="val">对象</param>
        /// <param name="key">对象key</param>
        public void Remove(V val, K key)
        {
            if (val == null)
                return;


            this.valuesList.Remove(val);

            if (dict.ContainsKey(key))
                dict.Remove(key);
        }

        /// <summary>
        /// 是否存在指定key的对象
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>是否存在</returns>
        public bool Contains(K key)
        {
            if (dict.ContainsKey(key))
                return true;

            return false;
        }

        /// <summary>
        /// 重置缓存
        /// </summary>
        public void Reset()
        {
            dict.Clear();
            valuesList.Clear();
        }

        /// <summary>
        /// 根据key获取对象
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
    }
}