using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using Timer=System.Timers.Timer;

namespace ReckoningCounterService.UI
{
    public partial class OfferMessageFrm : Form
    {
        private int count = 0;
        private int lastCount = 0;

        private int receiveCount = 0;
        private int lastReceiveCount = 0;

        public OfferMessageFrm()
        {
            InitializeComponent();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += ComputeSpeed;
            timer.Enabled = true;

            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";

#if(DEBUG)
            label2.Visible = true;
            textBox2.Visible = true;
            textBox4.Visible = true;
#endif
        }

        void ComputeSpeed(object sender, System.Timers.ElapsedEventArgs e)
        {
            decimal offset = count - lastCount;
            decimal speed = offset/1;

            //textBox2.Text = speed.ToString();
            if(speed != 0)
                SetMessage(textBox2,speed.ToString());

            lastCount = count;

            SetMessage(textBox1, count.ToString());

            decimal receiveOffset = receiveCount - lastReceiveCount;
            decimal receiveSpeed = receiveOffset/1;

            if(receiveSpeed != 0)
                SetMessage(textBox4, receiveSpeed.ToString());

            lastReceiveCount = receiveCount;
            SetMessage(textBox3, receiveCount.ToString());
        }

        private void OfferMessageFrm_Load(object sender, EventArgs e)
        {
            ReckoningCounter.BLL.delegateoffer.OrderOfferCenter.Instance.DoOfferOrderEvent += Instance_DoOfferOrderEvent;
            ReckoningCounter.BLL.OrderAccepterService.Service.ReceiveOrderEvent += new EventHandler<ReckoningCounter.BLL.Reckoning.Instantaneous.RuntimeMessageEventArge>(Service_ReceiveOrderEvent);
        }

        void Service_ReceiveOrderEvent(object sender, ReckoningCounter.BLL.Reckoning.Instantaneous.RuntimeMessageEventArge e)
        {
            receiveCount++;
        }

        void Instance_DoOfferOrderEvent(object sender, ReckoningCounter.BLL.Reckoning.Instantaneous.RuntimeMessageEventArge e)
        {
            MessageDisplayHelper.Event(e.RuntimeMessage, listBox1);
            count++;

            //SetMessage(textBox1,count.ToString());
            //textBox1.Text = count.ToString();
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            count = 0;
            lastCount = 0;
            textBox1.Text = "";
            textBox2.Text = "";


            receiveCount = 0;
            lastReceiveCount = 0;
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void SetMessage(TextBox textBox,string message)
        {
            try
            {
                if (!this.IsHandleCreated)
                    return;

                if (this.IsDisposed)
                    return;

                if (textBox == null)
                {
                    LogHelper.WriteDebug("OfferMessageFrm.SetMessage[TextBox=null]");
                    return;
                }

                if(textBox.InvokeRequired)
                    this.Invoke(new MethodInvoker(() => textBox.Text = message));
                else
                {
                    textBox.Text = message;
                }
            }
            catch (InvalidOperationException ex)
            {
                //在创建窗口句柄之前，不能在控件上调用 Invoke 或 BeginInvoke。
                //MessageBox.Show(ex.ToString());
                LogHelper.WriteError(ex.Message, ex);
            }
            
        }
    }
}
