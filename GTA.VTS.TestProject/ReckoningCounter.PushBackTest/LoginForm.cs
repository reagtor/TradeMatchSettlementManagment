using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ReckoningCounter.BLL.Common;

namespace ReckoningCounter.PushBackTest
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string traderID = ServerConfig.TraderID;
            string xhaccount = ServerConfig.XHCapitalAccount;
            string hkaccount = ServerConfig.HKCapitalAccount;
            string gzqhaccount = ServerConfig.GZQHCapitalAccount;
            string spqhaccount = ServerConfig.SPQHCapitalAccount;

            if(!string.IsNullOrEmpty(traderID))
                txtTradeID.Text = traderID;

            if (!string.IsNullOrEmpty(xhaccount))
                txtXH.Text = xhaccount;

            if (!string.IsNullOrEmpty(hkaccount))
                txtHK.Text = hkaccount;

            if (!string.IsNullOrEmpty(gzqhaccount))
                txtGZQH.Text = gzqhaccount;

            if (!string.IsNullOrEmpty(spqhaccount))
                txtSPQH.Text = spqhaccount;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ServerConfig.TraderID = txtTradeID.Text.Trim();
            ServerConfig.XHCapitalAccount = txtXH.Text.Trim();
            ServerConfig.HKCapitalAccount = txtHK.Text.Trim();
            ServerConfig.GZQHCapitalAccount = txtGZQH.Text.Trim();
            ServerConfig.SPQHCapitalAccount = txtSPQH.Text.Trim();

            ServerConfig.Refresh();
            this.Close();
        }
    }
}
