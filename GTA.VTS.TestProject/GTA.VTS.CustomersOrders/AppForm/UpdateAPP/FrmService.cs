using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Collections;

namespace GTA.VTS.CustomersOrders.AppForm.UpdateAPP
{
    public partial class FrmServer : Form
    {
        public FrmServer()
        {
            InitializeComponent();
            gimsServer = new APPUpdateServer();
        }
        private APPUpdateServer gimsServer = null;

        public APPUpdateServer GIMSServer
        {
            get { return gimsServer; }
            set { gimsServer = value; }
        }

        private void menuItemUpdateService_Click(object sender, EventArgs e)
        {
            GIMSServer.RegistryUpdateService();
            RefreshService();
        }

        public void RefreshService()
        {
            lstBoxService.Items.Clear();
            foreach (WellKnownServiceTypeEntry w in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
                lstBoxService.Items.Add(w);
 
        }

        
    }
}