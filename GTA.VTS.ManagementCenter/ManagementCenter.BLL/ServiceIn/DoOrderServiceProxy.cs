using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.DAL.DoOrderService;

namespace ManagementCenter.BLL.ServiceIn
{
    /// <summary>
    /// 描述:撮合中心代理类
    /// 作者：熊晓凌
    /// 日期：2008-11-20
    /// </summary>
    public class DoOrderServiceProxy
    {
        private static DoOrderServiceProxy instance;

        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static DoOrderServiceProxy GetInstance()
        {
            if(instance==null)
                instance = new DoOrderServiceProxy();
            return instance;
        }

        private static string endpiontConfigurationName = "NetTcpBinding_IDoOrder";

        #region 撮合中心

        /// <summary>
        /// 获取客户端连接
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="port">服务器端口</param>
        /// <param name="name">服务器名称</param>
        /// <returns></returns>
        private DoOrderClient GetClient(string ip,int port,string name)
        {
            DoOrderClient doOrderclient;

            try
            {
                EndpointAddress tcpAddress = new EndpointAddress(string.Format("net.tcp://{0}:{1}/{2}",ip,port,name));
                doOrderclient = new DoOrderClient(endpiontConfigurationName, tcpAddress);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8000";
                string errMsg = "无法获取撮合中心提供的服务[IDoOrder],IP为：" + ip;
                VTException vte= new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
            return doOrderclient;
        }
        #endregion

        /// <summary>
        /// 测试服务连接方法
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="port">服务器端口</param>
        /// <param name="name">服务器名称</param>
        /// <returns></returns>
        public bool TestConnection(string ip, int port, string name)
        {
            bool falg = false;
            try
            {
                using (DoOrderClient doOrderclient = GetClient(ip, port, name))
                {
                    string s = doOrderclient.CheckChannel();
                    if(!string.IsNullOrEmpty(s)) falg = true;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-8001";
                string errMsg = "检测撮合中心连接失败！ip为" + ip;
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
            return falg;
        }

    }




   

}
