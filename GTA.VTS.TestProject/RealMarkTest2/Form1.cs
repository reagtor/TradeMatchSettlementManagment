using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonRealtimeMarket;
using RealTime.Server.SModelData.HqData;
//using RealtimeMarket.factory;
//using RealtimeMarket.FastService;
using GTAMarketSocket;
using RealTime.Common.CommonClass;


namespace RealTimeMarketTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public OnEventDelegate FEvent;
        private IRealtimeMarketService rms = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitRealtimeMarketComponent();
            //if (rms.IsWcf)
            //{
            //    button2.Enabled = true;
            //    button2.Visible = true;
            //    button3.Enabled = true;
            //    button3.Visible = true;
            //}
            //else
            //{
                button2.Enabled = false;
                button2.Visible = false;
                button3.Enabled = false;
                button3.Visible = false;
           //}

        }


        private void ShowMessage(string message)
        {
            
        }

        #region 刷新数据
        delegate void RefreshQHDelegate(FutData futData);

        private delegate void RefreshXHDelegate(HqData haData);
        private void RefreshQHData(object sender, CommonRealtimeMarket.FutDataChangeEventArg e)
        {
            var vifFutdata = e.HqData;
            if (vifFutdata == null)
                return;

            var hutExData = vifFutdata;
            if (hutExData == null)
                return;
            if (hutExData.Stockno != textBox1.Text.Trim())
                return;
            if(propertyGrid1.InvokeRequired)
            {
                propertyGrid1.Invoke(new RefreshQHDelegate(RefreshQHView), hutExData);
                return;
            }
            propertyGrid1.SelectedObject = hutExData; 
        }
        private void RefreshQHView(FutData futData)
        {
            propertyGrid1.SelectedObject = futData; 
        }
        private void RefreshXHData(object sender, CommonRealtimeMarket.StockHqDataChangeEventArg e)
        {
            var hqdata = e.HqData;
            if (hqdata == null)
                return;

            var hqExData = hqdata;
            if (hqExData == null)
                return;

            var hqData = hqExData.HqData;
            if (hqData == null)
                return;
            if (hqData.Stockno != textBox1.Text.Trim())
                return;
            if (propertyGrid1.InvokeRequired)
            {
                propertyGrid1.Invoke(new RefreshXHDelegate(RefreshXHView), hqData);
                return;
            }
            propertyGrid1.SelectedObject = hqData; 
        }
        private void RefreshXHView(HqData hqData)
        {
            propertyGrid1.SelectedObject = hqData;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            string code = textBox1.Text;
            if (code.Length == 0)
            {
                MessageBox.Show("股票代码不能为空");
                return;
            }

            if (code.ToUpper().IndexOf("IF") >= 0)
            {
                var vifFutdata = rms.GetFutData(code);
                if (vifFutdata == null)
                    return;

                var hutExData = vifFutdata;
                if (hutExData == null)
                    return;

                propertyGrid1.SelectedObject = hutExData;
                rms.StockRealtimeMarketChangeEvent -= RefreshXHData;
                rms.FutRealtimeMarketChangeEvent += RefreshQHData;
            }
            else
            {
                var hqdata = rms.GetStockHqData(code);
                if (hqdata == null)
                    return;

                var hqExData = hqdata;
                if (hqExData == null)
                    return;
                var hqData = hqExData.HqData;
                if (hqData == null)
                    return;

                propertyGrid1.SelectedObject = hqData;
                rms.FutRealtimeMarketChangeEvent -= RefreshQHData;
                rms.StockRealtimeMarketChangeEvent += RefreshXHData;

            }
        }

        private ISocket _socketService = null;

        /// <summary>
        /// 初始化行情组件
        /// </summary>
        private void InitRealtimeMarketComponent()
        {

            ////登陆处理

            //this.rms = RealtimeMarketServiceFactory2.GetService();
            //rms.Login("rtuser", "11", ShowConnectMessage);
            //rms.InitializeService();

            ////数据接受注册事件
            //this.FEvent += Event;
            //_socketService = rms.SocketService;
            //_socketService.AddEventDelegate(this.FEvent);
            //rms.GetGTASingleton.SocketStatusHandle(SocketStatusChange);
            ////rms.StockRealtimeMarketChangeEvent+=

        }

        /// <summary>
        /// 状态改变触发事件
        /// </summary>
        /// <param name="e"></param>
        private void SocketStatusChange(SocketServiceStatusEventArg e)
        {
            if (this.statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new DelegateSocketStatusChange(delegate { SocketStatusChange(e); }), e);
            }
            else
            {
                if (e.SocketStatus == SocketServiceStatus.SSSConnected || e.SocketStatus == SocketServiceStatus.SSSLogin)
                {
                    this.StatusText.Text = "已连接";

                    //this.MenuResetState("中断连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSDisConnectedNeedReconnect)
                {
                    this.StatusText.Text = "已断开,启动自动重连接...";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSDisConnected)
                {
                    this.StatusText.Text = "已断开";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSException)
                {
                    this.StatusText.Text = "连接异常";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSConnecting)
                {
                    this.StatusText.Text = e.Message; //"正在重新连接……";
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSResetCycleIsEnding)
                {
                    this.StatusText.Text = e.Message;
                    //if (MessageBox.Show("连接中断，是否重新连接？", "系统信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                    //    DialogResult.Yes)
                    //{
                    //    this.rms.GetGTASingleton.BeginAResetCycle();
                    //}
                    //else
                        _socketService.SocketStatus = GTASocketStatus.SSIsManualStoped;
                }
            }
        }

        /// <summary>
        /// 状态处理（暂不使用)
        /// </summary>
        /// <param name="state"></param>
        private void GTASocketSingleton_SocketStateChanged(EnumSocketResetState state)
        {
        }

        /// <summary>
        /// 显示登陆信息
        /// </summary>
        /// <param name="e"></param>
        private void ShowConnectMessage(SocketServiceStatusEventArg e)
        {
            if (this.InvokeRequired)
            {
                SocketServiceStatusHandler showConnectMessage = ShowConnectMessage;
                this.Invoke(showConnectMessage, new object[] { e });
            }
            else
            {
                Application.DoEvents();
                switch (e.SocketStatus)
                {
                    case  SocketServiceStatus.SSSLogin:
                        //更新程序是否需要更新
                        this.StatusText.Text = "检测更新程序是否需要更新。";
                        Application.DoEvents();
                        // UpdateUpdateSoftSelf();
                        //检测是否需要更新
                        this.StatusText.Text = "异步启动检测客户端是否需要更新。";
                        Application.DoEvents();
                        this.StatusText.Text = "已登陆行情服务器.";
                        Application.DoEvents();
                        this.DialogResult = DialogResult.OK;
                        break;
                    case SocketServiceStatus.SSSErrorUser:


                        MessageBox.Show(e.Message, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.StatusText.Text = "登陆失败，请重新登陆！";
                        break;
                    case SocketServiceStatus.SSSException:
                        break;
                    case SocketServiceStatus.SSSResetCycleIsEnding:
                        this.StatusText.Text = e.Message;

                        break;
                    case SocketServiceStatus.SSSDisConnected:
                        this.StatusText.Text = e.Message;

                        break;
                    case SocketServiceStatus.SSSSocksProxyError:
                        this.StatusText.Text = e.Message;

                        break;
                    default:
                        this.StatusText.Text = e.Message;
                        Application.DoEvents();
                        break;
                }
            }
        }


        /// <summary>
        /// 清除消息
        /// </summary>
        public void ClearEchoInfo()
        {
            //lock (this.lstMessages)
            //{
            //    this.lstMessages.Items.Clear();
            //}
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="eventMessage"></param>
        public void Event(string eventMessage)
        {
            // MessageDisplayHelper.Event(eventMessage, lstMessages);
        }

        /// <summary>
        /// Socket状态改变触发事件
        /// </summary>
        /// <param name="_e"></param>
        private delegate void DelegateSocketStatusChange(SocketServiceStatusEventArg _e);

        private void button2_Click(object sender, EventArgs e)
        {
            //ClientListForm.GetInstance().Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //new frmRealTimeDataQuery().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            string code = textBox1.Text;
            if (code.Length == 0)
            {
                MessageBox.Show("商品期货代码不能为空");
                return;
            }
            var hqdata = rms.GetMercantileFutData(code);
            if (hqdata == null)
                return;

            var hqExData = hqdata;
            if (hqExData == null)
                return;

            propertyGrid1.SelectedObject = hqExData;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            string code = textBox1.Text;
            if (code.Length == 0)
            {
                MessageBox.Show("商品期货代码不能为空");
                return;
            }
            var hqdata = rms.GetHKStockData(code);
            if (hqdata == null)
                return;

            var hqExData = hqdata;
            if (hqExData == null)
                return;

            propertyGrid1.SelectedObject = hqExData;
        }
    }
}