using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL;
using ReckoningCounterService.UI.RuntimeMessage;

namespace ReckoningCounterService.UI
{
    

    public partial class CallbackMessageFrm : Form
    {

        private int dealcount;
        private int ordercount;
        private int cacelcount;

        public CallbackMessageFrm()
        {
            InitializeComponent();
        }

        private void CallbackMessageFrm_Load(object sender, EventArgs e)
        {
            ReckonCenter.Instace.CancelOrderCallbackEvent += Instace_CancelOrderCallbackEvent;
            ReckonCenter.Instace.DealRptCallbackEvent += Instace_DealRptCallbackEvent;
            ReckonCenter.Instace.OrderRptCallbackEvent += Instace_OrderRptCallbackEvent;
        }

        void Instace_OrderRptCallbackEvent(object sender, ReckoningCounter.BLL.Reckoning.Instantaneous.RuntimeMessageEventArge e)
        {
           MessageDisplayHelper.Event(e.RuntimeMessage,  lbOrderRpt);
            ordercount++;
            //textBox2.Text = ordercount.ToString();
            SetMessage(textBox2,ordercount.ToString());
        }

        void Instace_DealRptCallbackEvent(object sender, ReckoningCounter.BLL.Reckoning.Instantaneous.RuntimeMessageEventArge e)
        {
            MessageDisplayHelper.Event(e.RuntimeMessage,  lbDealRpt);
            dealcount++;
            //textBox1.Text = dealcount.ToString();
            SetMessage(textBox1, dealcount.ToString());
        }

        void Instace_CancelOrderCallbackEvent(object sender, ReckoningCounter.BLL.Reckoning.Instantaneous.RuntimeMessageEventArge e)
        {
            MessageDisplayHelper.Event(e.RuntimeMessage,  lbCancelOrderRpt);
            cacelcount++;
            //textBox3.Text = cacelcount.ToString();
            SetMessage(textBox3,cacelcount.ToString());
        }

        private void SetMessage(TextBox textBox, string message)
        {
            this.Invoke(new MethodInvoker(() => textBox.Text = message));
        }

        private void CallbackMessageFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ReckonCenter.Instace.CancelOrderCallbackEvent -= Instace_CancelOrderCallbackEvent;
            ReckonCenter.Instace.DealRptCallbackEvent -= Instace_DealRptCallbackEvent;
            ReckonCenter.Instace.OrderRptCallbackEvent -= Instace_OrderRptCallbackEvent;
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    lbDealRpt.Items.Clear();
                    dealcount = 0;
                    textBox1.Text = "0";
                    break;
                case 1:
                    lbOrderRpt.Items.Clear();
                    ordercount = 0;
                    textBox2.Text = "0";
                    break;
                case 2:
                    lbCancelOrderRpt.Items.Clear();
                    cacelcount = 0;
                    textBox3.Text = "0";
                    break;
            }
        }
    }
}
