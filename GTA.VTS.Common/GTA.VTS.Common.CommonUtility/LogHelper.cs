using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace GTA.VTS.Common.CommonUtility
{
    /// <summary>
    /// 日志入口
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class LogHelper
    {

         /// <summary>
        /// 日志
        /// </summary>
        private static ILog clientLog;

        /// <summary>
        /// 初始化
        /// </summary>
        static LogHelper()
        {
            XmlConfigurator.Configure();
            if (clientLog == null)
            {
                //clientLog = LogManager.GetLogger(typeof(LogHelper));
                clientLog = LogManager.GetLogger("Server");
            }
        }

        /// <summary>
        /// 写普通信息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void WriteInfo(string msg)
        {
            clientLog.Info(msg);
        }

        /// <summary>
        /// 写调试信息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void WriteDebug(string msg)
        {
            clientLog.Debug(msg);
        }

        /// <summary>
        /// 写错误信息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        public static void WriteError(string msg, Exception ex)
        {
            clientLog.Error(msg, ex);
        }
        
    }
}
