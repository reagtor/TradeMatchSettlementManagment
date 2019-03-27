using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Collections;
using GTA.VTS.UpdateServerRemoting;
using System.Configuration;



namespace GTA.VTS.AutomaticProgramUpdatesServer
{
    public class APPUpdateServer
    {
        /// <summary>
        /// 更新程序开启的服务器端口
        /// </summary>
        public static int UpdatePort
        {
            get
            {
                int result = 9999;
                try
                {
                    string str = ConfigurationManager.AppSettings["UpdatePort"];
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (int.TryParse(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            return 9999;
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }

        public void RegistryUpdateService()
        {
            TcpServerChannel channel = new TcpServerChannel(UpdatePort);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(UpdateServer),
                "UpdateServer",
                WellKnownObjectMode.SingleCall);
        }
    }
}
