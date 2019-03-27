using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Collections;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.UpdateServerRemoting;



namespace GTA.VTS.CustomersOrders.AppForm.UpdateAPP
{
    public class APPUpdateServer
    {
        public void RegistryUpdateService()
        {
            TcpServerChannel channel = new TcpServerChannel(ServerConfig.UpdatePort);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(UpdateServer),
                "UpdateServer",
                WellKnownObjectMode.SingleCall);
        }
    }
}
