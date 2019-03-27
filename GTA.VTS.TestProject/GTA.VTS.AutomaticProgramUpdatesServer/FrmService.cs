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

namespace GTA.VTS.AutomaticProgramUpdatesServer
{
    public partial class FrmServer : Form
    {
        Timer timer = new Timer();
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
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmServer_Load(object sender, EventArgs e)
        {
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
        }
        int k = 0;
        /// <summary>
        /// 时间事事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            switch (k % 3)
            {
                case 0:
                    this.notifyIcon1.Icon = ((System.Drawing.Icon)(GTA.VTS.AutomaticProgramUpdatesServer.Properties.Resources.Update));
                    break;
                case 1:
                    this.notifyIcon1.Icon = ((System.Drawing.Icon)(GTA.VTS.AutomaticProgramUpdatesServer.Properties.Resources.Auto_up_1));
                    break;
                case 2:
                    this.notifyIcon1.Icon = ((System.Drawing.Icon)(GTA.VTS.AutomaticProgramUpdatesServer.Properties.Resources.Auto_up_2));
                    break;
                default:
                    this.notifyIcon1.Icon = ((System.Drawing.Icon)(GTA.VTS.AutomaticProgramUpdatesServer.Properties.Resources.Update));
                    break;
            }
            k++;
            if (k > 1000)
            {
                k = 0;
            }

        }
        /// <summary>
        /// 点击托盘中最小化图标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            timer.Enabled = false;
            this.notifyIcon1.Visible = false;

        }
        /// <summary>
        /// 窗体大小发生变化时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmServer_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
                timer.Enabled = true;
            }
        }


    }
}