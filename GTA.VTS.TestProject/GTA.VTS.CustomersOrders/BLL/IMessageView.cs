using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// Desc: 信息显示接口
    /// Create by: 董鹏
    /// Create Date: 2010-03-01
    /// </summary>
    public interface IMessageView
    {
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        void WriteMessage(string msg);
    }

    /// <summary>
    /// Desc: 回报处理方法接口
    /// Create by: 董鹏
    /// Create Date: 2010-03-01
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOrderCallBackView<T>
    {
        /// <summary>
        /// 回报处理
        /// </summary>
        /// <param name="pushBack"></param>
        void ProcessPushBack(T pushBack);
    }
}
