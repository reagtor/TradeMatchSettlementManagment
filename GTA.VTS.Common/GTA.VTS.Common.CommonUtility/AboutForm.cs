#region Using Namespace

using System;
using System.Windows.Forms;

#endregion

namespace GTA.VTS.Common.CommonUtility
{
    public partial class AboutForm : Form
    {
        public AboutForm(string name, string version)
        {
            InitializeComponent();
            this.AppName = name;
            this.AppVersion = version;
        }

        public string AppName { get; set; }

        public string AppVersion { get; set; }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            //AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            //string version = assemblyName.Version.ToString();
            //this.labelMessage.Text = AppName + "  " + AppVersion;
            labelMessage.Text = AppName;
            lbVersion.Text = AppVersion;
        }

        private void linkGtahttp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.gtafe.com");
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
    }
}