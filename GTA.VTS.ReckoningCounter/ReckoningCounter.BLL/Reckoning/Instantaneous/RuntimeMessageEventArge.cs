using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.BLL.Reckoning.Instantaneous
{

    /// <summary>
    /// 运行时业务消息事件参数
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class RuntimeMessageEventArge : EventArgs
    {
        /// <summary>
        /// 业务消息
        /// </summary>
        public string RuntimeMessage { get; private set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="strMessage"></param>
        public RuntimeMessageEventArge( string strMessage )
        {
            RuntimeMessage = strMessage;
        }
    }
}
