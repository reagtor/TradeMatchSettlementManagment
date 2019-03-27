using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;

namespace ReckoningCounterService.UI
{
    public partial class CounterIDConfig : Form
    {
        public CounterIDConfig()
        {
            InitializeComponent();
        }

        private void CounterIDConfig_Load(object sender, EventArgs e)
        {
            string counterID = ServerConfig.CounterID;

            if (string.IsNullOrEmpty(counterID))
            {
                //COUNT_CLIENT_ID = Dns.GetHostName() + Guid.NewGuid();
                //改为写MAC地址
                counterID = CommUtils.GetMacAddress();
            }

            textBox1.Text = counterID;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string counterID = textBox1.Text.Trim();
            if ( string.IsNullOrEmpty(counterID))
            {
                MessageBox.Show("请输入柜台ID！", "信息");
                textBox1.Focus();
            }
            else
            {
                if (counterID != ServerConfig.CounterID)
                {
                    ServerConfig.CounterID = counterID;
                    MessageBox.Show("保存成功！","信息");
                }

                this.Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
