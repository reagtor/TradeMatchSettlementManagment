using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchCenter.BLL
{
    /// <summary>
    /// 此类作为单一实例模式泛型抽象基类
    /// 内部嵌套类实现单一实例模式,实现延迟初始化
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// </summary>
    public abstract class Singleton<T> where T : new()
    {
        static Singleton()
        {
        }
        /// <summary>
        /// 实体单一实例模式不延时加载内部实例静态变量
        /// </summary>
        protected static readonly T singletonInstance = new T();
    }
}
