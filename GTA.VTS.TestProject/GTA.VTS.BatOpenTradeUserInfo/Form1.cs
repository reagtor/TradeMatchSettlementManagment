using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.BatOpenTradeUserInfo.TransactionManagerService;
using System.Threading;

namespace GTA.VTS.BatOpenTradeUserInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnOpenUserAccount_Click(object sender, EventArgs e)
        {
            btnOpenUserAccount.Enabled = false;
            WCFManager.lableMsg = this.labMessage;
            WCFManager.btnOpen = this.btnOpenUserAccount;
            // labMessage.Text = "开户开始稍后.......";
            WCFManager.ShowMesg("开户开始稍后......");

            int amount = 100;
            int.TryParse(txtOpenAmount.Text.Trim(), out amount);
            decimal capital = 100000000000;
            decimal.TryParse(txtCapital.Text.Trim(), out capital);
            if (amount > 100)
            {
                //MessageBox.Show("批量开户不能大于100");
                labMessage.Text = "批量开户不能大于100";
                btnOpenUserAccount.Enabled = true;
                return;
            }
            if (amount == 0)
            {
                amount = 100;
            }
            //开始初始化
            if(WCFManager.StartIni(capital, amount, chkIsIniHoldAccount.Checked))
            {
                WCFManager.ShowMesg("开户成功结束......");
            }
            else
            {
                WCFManager.ShowMesg("开户失败......");
            }

            // Thread.CurrentThread.Join(100000);
            //if (string.IsNullOrEmpty(message))
            //{
            //    message = "开户成功";
            //}
            // MessageBox.Show(message);
            //  btnOpenUserAccount.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WCFManager.CountThread() > 0)
            {
                e.Cancel = true;
                return;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WCFManager.testcurrcy();
        }
    }
}
