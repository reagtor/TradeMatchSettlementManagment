#region Using Namespace

using System.Collections.Generic;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// StockTradingRuleValidater，FutureTradingRuleValidater的泛型基类
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    /// <typeparam name="K">Key</typeparam>
    /// <typeparam name="V">Value</typeparam>
    public class TradingRuleValidater<K, V>
    {
        private Dictionary<K, V> containers = new Dictionary<K, V>();

        /// <summary>
        /// 根据Key获取Value
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public V GetContainer(K key)
        {
            V src = default(V);

            if (containers.ContainsKey(key))
                src = containers[key];

            return src;
        }

        /// <summary>
        /// 设置Value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="val">Value</param>
        public void SetContainer(K key, V val)
        {
            containers[key] = val;
        }
    }
}