#region Using Namespace

using System;
using System.Windows.Forms;
using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using GTAMarketSocket;
using RealTime.Common.CommonClass;
using GTAStockBusinessFactory;

#endregion

namespace RealMarkTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitRealtimeMarketComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            string code = textBox1.Text;
            if (code.Length == 0)
            {
                MessageBox.Show("股票代码不能为空");
                return;
            }
            IRealtimeMarketService service = RealtimeMarketServiceFactory.GetService();
            if (code.ToUpper().IndexOf("IF") >= 0)
            {
                var vifFutdata = service.GetFutData(code);
                if (vifFutdata == null)
                    return;

                var hutExData = vifFutdata;
                if (hutExData == null)
                    return;


                propertyGrid1.SelectedObject = hutExData;
            }
            else
            {
                var hqdata = service.GetStockHqData(code);
                if (hqdata == null)
                    return;

                var hqExData = hqdata;
                if (hqExData == null)
                    return;

                var hqData = hqExData.HqData;
                if (hqData == null)
                    return;

                propertyGrid1.SelectedObject = hqData;
            }
        }

        #region == 行情初始化 ==

        /// <summary>
        /// 消息打印事件
        /// </summary>
        public OnEventDelegate FEvent;

        /// <summary>
        /// 瑞尔格特下单服务
        /// </summary>
        private GTASocket SocketService;

        /// <summary>
        /// 初始化行情组件
        /// </summary>
        private void InitRealtimeMarketComponent()
        {
            //登陆处理
            
            GTASocketSingletonForRealTime.StartLogin("Apex", "Apex", ShowConnectMessage);
            //数据接受注册事件
            this.FEvent += Event;
            SocketService = GTASocketSingletonForRealTime.SocketService;
            GTASocketSingletonForRealTime.SocketStateChanged += GTASocketSingleton_SocketStateChanged;
            SocketService.AddEventDelegate(this.FEvent);
            GTASocketSingletonForRealTime.SocketStatusHandle(SocketStatusChange);
            IRealtimeMarketService rms = RealtimeMarketServiceFactory.GetService();
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
                    if (MessageBox.Show("连接中断，是否重新连接？", "系统信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                        DialogResult.Yes)
                    {
                        GTASocketSingletonForRealTime.BeginAResetCycle();
                    }
                    else
                        SocketService.SocketStatus = GTASocketStatus.SSIsManualStoped;
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
                this.Invoke(showConnectMessage, new object[] {e});
            }
            else
            {
                Application.DoEvents();
                switch (e.SocketStatus)
                {
                    case SocketServiceStatus.SSSLogin:
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

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            string code = textBox1.Text;
            if (code.Length == 0)
            {
                MessageBox.Show("股票代码不能为空");
                return;
            }
            IRealtimeMarketService service = RealtimeMarketServiceFactory.GetService();
                var vtHkStock = service.GetHKStockData(code);
                if (vtHkStock == null)
                    return;

                var hkStock = vtHkStock;
                //if (hkStock == null)
                //    return;


                propertyGrid1.SelectedObject = hkStock;
            }
        
}
    }
