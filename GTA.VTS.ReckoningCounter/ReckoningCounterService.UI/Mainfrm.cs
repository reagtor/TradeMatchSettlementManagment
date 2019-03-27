#region Using Namespace

using System;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using GTAMarketSocket;
//using RealtimeMarket.factory;
using ReckoningCounter.BLL;
using ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.ScheduleManagement;
using ReckoningCounterService.UI;
using ReckoningCounterService.UI.RuntimeMessage;
using Timer=System.Timers.Timer;
//using RealTime.Server.Handler;
using RealTime.Common.CommonClass;
using System.Diagnostics;

#endregion

namespace CounterServiceManager
{
    /// <summary>
    /// Title:清算柜台主窗体
    /// Create by:李健华
    /// Create date :2009-10-10
    /// </summary>
    public partial class mainfrm : Form
    {
        #region == 窗体相关 ==

        private HostManager hostManager = new HostManager();
        private string title = "瑞尔格特清算柜台";

        /// <summary>
        /// 构造函数
        /// </summary>
        public mainfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainfrm_Load(object sender, EventArgs e)
        {
            ThreadPool.SetMaxThreads(200, 200);
            Thread t = new Thread(DoStart);
            t.Start();
            //DoStart();

            Timer timer = new Timer();
            timer.Interval = 30*1000;
            timer.Elapsed += delegate
                                 {
                                     GC.Collect();
                                     GC.Collect();
                                 };
            timer.Enabled = true;
        }


        private void DoStart()
        {
            Application.ThreadException += Application_ThreadException;

            this.Invoke(new MethodInvoker(() => this.Text = title + "[初始化中，请稍候……]"));
            this.Invoke(new MethodInvoker(() => this.menuStrip1.Enabled = false));
            this.Invoke(new MethodInvoker(() => this.Cursor = Cursors.WaitCursor));

            string errMsg = string.Empty;

            //1.启动行情服务
            try
            {
                this.InitRealtimeMarketComponent();
            }
            catch (Exception ex)
            {
                string msg = "行情组件初始化失败！请检查配置文件。";
                string strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;
                MessageBox.Show(strErrorMsg, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(Environment.ExitCode);
                return;
            }

            //2.业务层进行初始化
            bool isScheduleStart = ScheduleManager.Start(ref errMsg);
            if (!isScheduleStart)
            {
                string msg = errMsg.Length > 0 ? errMsg : "业务逻辑初始化失败！请检查配置文件。";

                MessageBox.Show(msg, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

                LogHelper.WriteInfo("*******************Start Failure*********************");
                Environment.Exit(Environment.ExitCode);
                return;
            }

            //3.开启柜台WCF服务
            bool canOpenHost = hostManager.RegisterWcfService(ref errMsg);
            if (!canOpenHost)
            {
                string msg = "远程服务启动失败！请检查配置文件。";
                string strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + errMsg;

                MessageBox.Show(strErrorMsg, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteInfo(errMsg);
                Environment.Exit(Environment.ExitCode);
                return;
            }

            this.Invoke(new MethodInvoker(() => this.Text = title + "[初始化成功！]"));
            this.Invoke(new MethodInvoker(() => this.menuStrip1.Enabled = true));
            this.Invoke(new MethodInvoker(() => this.Cursor = Cursors.Default));

            Thread.Sleep(2000);


            this.Invoke(new MethodInvoker(() => this.Text = title));
            this.Invoke(new MethodInvoker(() => this.Text = GetCounterIDString()));
        }

        private string GetCounterIDString()
        {
            string counterID = ServerConfig.CounterID;
            return this.title + "[柜台ID：" + counterID + "]";
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {

                if (!EventLog.SourceExists("ReckoningCounterService"))
                {
                    EventLog.CreateEventSource("ReckoningCounterService", "ReckoningCounterServiceLog");
                }

                EventLog myLog = new EventLog();
                myLog.Source = "ReckoningCounterService";

                myLog.WriteEntry(e.Exception.Message + " : " + e.Exception.StackTrace);

            }
            catch (Exception ex1)
            {
                LogHelper.WriteError("********************清算柜台异常:*******************\r\n", ex1);
            }

            LogHelper.WriteError(e.Exception.Message, e.Exception);
        }

        #endregion

        #region == 行情初始化 ==

        //private ISocket _socketService;

        /// <summary>
        /// 消息打印事件
        /// </summary>
        public OnEventDelegate FEvent;

        //private IRealtimeMarketService rms;

        /// <summary>
        /// 瑞尔格特下单服务
        /// </summary>
        private GTASocket SocketService;

        private void InitRealtimeMarketComponent()
        {
            int mode = Utils.GetRealTimeMode();

            if (mode == 1)
            {
                InitRealtimeMarketComponent1();
            }
            else if (mode == 2)
            {
                InitRealtimeMarketComponent2();
            }
        }

        /// <summary>
        /// 初始化行情组件1
        /// </summary>
        private void InitRealtimeMarketComponent1()
        {
            //登陆处理
            //string username = "rtuser";
            //string password = "11";
            string username = Utils.GetRealTimeUserName();
            string password = Utils.GetRealTimePassword();
            GTASocketSingletonForRealTime.StartLogin(username, password, ShowConnectMessage);
            //数据接受注册事件
            this.FEvent += Event;
            SocketService = GTASocketSingletonForRealTime.SocketService;
            GTASocketSingletonForRealTime.SocketStateChanged += GTASocketSingleton_SocketStateChanged;
            SocketService.AddEventDelegate(this.FEvent);
            GTASocketSingletonForRealTime.SocketStatusHandle(SocketStatusChange);
            IRealtimeMarketService rms = RealtimeMarketServiceFactory.GetService();
        }

        /// <summary>
        /// 初始化行情组件2
        /// </summary>
        private void InitRealtimeMarketComponent2()
        {
            ////登陆处理
            ////string username = "rtuser";
            ////string password = "11";
            //string username = Utils.GetRealTimeUserName();
            //string password = Utils.GetRealTimePassword();
            //this.rms = RealtimeMarketServiceFactory2.GetService();
            //rms.Login(username, password, ShowConnectMessage);
            //rms.InitializeService();

            ////数据接受注册事件
            //this.FEvent += Event;
            //_socketService = rms.SocketService;
            //_socketService.AddEventDelegate(this.FEvent);
            //rms.GetGTASingleton.SocketStatusHandle(SocketStatusChange);
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
                    int mode = Utils.GetRealTimeMode();
                    this.StatusText.Text = e.Message;
                    if (MessageBox.Show("连接中断，是否重新连接？", "系统信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                        DialogResult.Yes)
                    {
                        //if (mode == 1)
                        //{
                        GTASocketSingletonForRealTime.BeginAResetCycle();
                        //}
                        //else if (mode == 2)
                        //{
                        //    this.rms.GetGTASingleton.BeginAResetCycle();
                        //}
                    }
                    else
                    {
                        if (mode == 1)
                        {
                            SocketService.SocketStatus = GTASocketStatus.SSIsManualStoped;
                        }
                        else
                        {
                            //_socketService.SocketStatus = GTASocketStatus.SSIsManualStoped;
                        }
                    }
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
            lock (this.lstMessages)
            {
                this.lstMessages.Items.Clear();
            }
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="eventMessage"></param>
        public void Event(string eventMessage)
        {
            MessageDisplayHelper.Event(eventMessage, lstMessages);
        }

        /// <summary>
        /// Socket状态改变触发事件
        /// </summary>
        /// <param name="_e"></param>
        private delegate void DelegateSocketStatusChange(SocketServiceStatusEventArg _e);

        #endregion

        #region == 菜单按钮事件 ==

        private void rtMessageRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null)
                if (!this.FindForm((sender as ToolStripMenuItem).Tag.ToString()))
                {
                    var frm = new RealtimeFrm();
                    this.lstMessages = frm.DisplayList;
                    //frm.Text = (sender as ToolStripMenuItem).Text;
                    frm.MdiParent = this;
                    frm.Show();
                }
        }

        private bool FindForm(string strFrmName)
        {
            foreach (var c in this.MdiChildren)
            {
                if (c.Text.ToLower() == strFrmName.ToLower())
                {
                    c.BringToFront();
                    return true;
                }
            }
            return false;
        }

        private void OfferMessageOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null)
                if (!this.FindForm((sender as ToolStripMenuItem).Tag.ToString()))
                {
                    var frm = new OfferMessageFrm();
                    //this.lstMessages = frm.DisplayList;
                    //frm.Text = (sender as ToolStripMenuItem).Text;
                    frm.MdiParent = this;
                    frm.Show();
                }
        }

        private void McRptMessageMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null)
                if (!this.FindForm((sender as ToolStripMenuItem).Tag.ToString()))
                {
                    var frm = new CallbackMessageFrm();
                    //this.lstMessages = frm.DisplayList;
                    //frm.Text = (sender as ToolStripMenuItem).Text;
                    frm.MdiParent = this;
                    frm.Show();
                }
        }

        #endregion

        private void mainfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Text = title + "[正在关闭，请稍候……]";
            LogHelper.WriteInfo("开始关闭柜台WCF服务");
            hostManager.UnRegisterWcfService();
            LogHelper.WriteInfo("柜台WCF服务已关闭！");

            ScheduleManager.Shutdown();
        }

        private void tsmiBreakConnection_Click(object sender, EventArgs e)
        {
        }

        private void tsmtReconnect_Click(object sender, EventArgs e)
        {
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            string version = assemblyName.Version.ToString();

            AboutForm aboutForm = new AboutForm("虚拟交易所清算柜台服务程序", version);
            aboutForm.ShowDialog();
            aboutForm.Close();
        }

        private void mainfrm_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void CounterIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CounterIDConfig config = new CounterIDConfig();
            config.ShowDialog();

            this.Text = GetCounterIDString();
        }

        private void tsmiQuit_Click(object sender, EventArgs e)
        {
        }
    }

    /// <summary>
    /// 服务管理
    /// </summary>
    public class HostManager
    {
        /// <summary>
        /// 帐户管理
        /// </summary>
        private ServiceHost _commonParaHost;

        /// <summary>
        /// 公共查询服务
        /// </summary>
        private ServiceHost _doCommonQuery;

        /// <summary>
        /// 回推服务单例
        /// </summary>
        private ServiceHost _doDealRptHost;

        /// <summary>
        /// 港股公共查询服务
        /// </summary>
        private ServiceHost _doHKCommonQuery;
        /// <summary>
        /// 港股Find公共查询服务(些Find服务的查询方法应用于ROE)
        /// </summary>
        private ServiceHost _doHKQuery;


        /// <summary>
        /// 下单服务
        /// </summary>
        private ServiceHost _doOrderHost;

        /// <summary>
        /// 下单查询单例
        /// </summary>
        private ServiceHost _doQueryTrader;

        private Timer timer;

        /// <summary>
        /// 定时检查WCF服务
        /// </summary>
        public HostManager()
        {
            int interval = 30*1000;
            timer = new Timer(interval);
            timer.Elapsed += DoHostCheck;
            //timer.Enabled = true;
        }

        #region Host Check

        /// <summary>
        /// 检查WCF服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoHostCheck(object sender, ElapsedEventArgs e)
        {
            CheckDoOrderHost();
            CheckDoDealRptHost();
            CheckDoQueryHost();
            CheckDoCommonQueryHost();
            CheckDoHKCommonQueryHost();
            CheckDoHKQueryHost();
            CheckManagementHost();
        }

        /// <summary>
        /// 检查下单服务
        /// </summary>
        private void CheckDoOrderHost()
        {
            if (_doOrderHost == null)
                return;

            if (_doOrderHost.State == CommunicationState.Closed || _doOrderHost.State == CommunicationState.Faulted)
            {
                LogHelper.WriteInfo("_doOrderHost has been Closed!");
                string errMsg = string.Empty;
                OpenDoOrderHost(ref errMsg);
            }
        }

        /// <summary>
        /// 检查成交回报服务
        /// </summary>
        private void CheckDoDealRptHost()
        {
            if (_doDealRptHost == null)
                return;

            if (_doDealRptHost.State == CommunicationState.Closed || _doDealRptHost.State == CommunicationState.Faulted)
            {
                LogHelper.WriteInfo("_doDealRptHost has been Closed!");
                string errMsg = string.Empty;
                OpenDoDealRptHost(ref errMsg);
            }
        }

        /// <summary>
        /// 检查交易员下单服务
        /// </summary>
        private void CheckDoQueryHost()
        {
            if (_doQueryTrader == null)
                return;

            if (_doQueryTrader.State == CommunicationState.Closed || _doQueryTrader.State == CommunicationState.Faulted)
            {
                LogHelper.WriteInfo("_doQueryTrader has been Closed!");
                string errMsg = string.Empty;
                OpenDoQueryHost(ref errMsg);
            }
        }

        /// <summary>
        /// 检查公共查询服务
        /// </summary>
        private void CheckDoCommonQueryHost()
        {
            if (_doCommonQuery == null)
                return;

            if (_doCommonQuery.State == CommunicationState.Closed || _doCommonQuery.State == CommunicationState.Faulted)
            {
                LogHelper.WriteInfo("_doQueryService has been Closed!");
                string errMsg = string.Empty;
                OpenDoCommonQueryHost(ref errMsg);
            }
        }

        /// <summary>
        /// 检查港股公共查询服务
        /// </summary>
        private void CheckDoHKCommonQueryHost()
        {
            if (_doHKCommonQuery == null)
                return;

            if (_doHKCommonQuery.State == CommunicationState.Closed || _doHKCommonQuery.State == CommunicationState.Faulted)
            {
                LogHelper.WriteInfo("_doHKQueryService has been Closed!");
                string errMsg = string.Empty;
                OpenDoHKCommonQueryHost(ref errMsg);
            }
        }
        /// <summary>
        /// 检查港股Find公共查询服务
        /// </summary>
        private void CheckDoHKQueryHost()
        {
            if (_doHKQuery == null)
                return;

            if (_doHKQuery.State == CommunicationState.Closed || _doHKQuery.State == CommunicationState.Faulted)
            {
                LogHelper.WriteInfo("_doHKFindService has been Closed!");
                string errMsg = string.Empty;
                OpenDoHKQueryHost(ref errMsg);
            }
        }

        /// <summary>
        /// 检查账户管理服务
        /// </summary>
        private void CheckManagementHost()
        {
            if (_commonParaHost == null)
                return;

            if (_commonParaHost.State == CommunicationState.Closed ||
                _commonParaHost.State == CommunicationState.Faulted)
            {
                LogHelper.WriteInfo("_commonParaHost has been Closed!");
                string errMsg = string.Empty;
                OpenManagementHost(ref errMsg);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 注册WCF服务
        /// </summary>
        public bool RegisterWcfService(ref string strErrorMsg)
        {
            bool result = OpenDoOrderHost(ref strErrorMsg);

            if (!result)
                return false;

            result = OpenDoDealRptHost(ref strErrorMsg);
            if (!result)
                return false;

            result = OpenDoQueryHost(ref strErrorMsg);
            if (!result)
                return false;
            result = OpenDoCommonQueryHost(ref strErrorMsg);
            if (!result)
                return false;

            result = OpenDoHKCommonQueryHost(ref strErrorMsg);
            if (!result)
                return false;

            result = OpenDoHKQueryHost(ref strErrorMsg);
            if (!result)
                return false;

            result = OpenManagementHost(ref strErrorMsg);
            if (!result)
                return false;

            //TraderFindService _TraderFindService = new TraderFindService(typeof(CommonParaService));

            //ServiceHost commonParaHost = new ServiceHost(typeof(TraderFindService));
            //commonParaHost.Open();

            timer.Enabled = true;

            return true;
        }

        /// <summary>
        /// 取消注册WCF服务
        /// </summary>
        public void UnRegisterWcfService()
        {
            timer.Enabled = false;

            CloseDoOrderHost();

            CloseDoDealRptHost();

            CloseDoQueryHost();

            CloseDoCommonQueryServiceHost();

            CloseDoHKCommonQueryServiceHost();

            CloseDoHKQueryServiceHost();

            CloseManagementHost();
        }

        #endregion

        #region Close Service

        /// <summary>
        /// 关闭账户管理服务
        /// </summary>
        private void CloseManagementHost()
        {
            LogHelper.WriteDebug("begin CloseManagementHost");
            try
            {
                if (_commonParaHost != null && _commonParaHost.State != CommunicationState.Closed)
                    _commonParaHost.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            LogHelper.WriteDebug("end CloseManagementHost");
        }

        /// <summary>
        /// 关闭交易员查询服务
        /// </summary>
        private void CloseDoQueryHost()
        {
            LogHelper.WriteDebug("begin CloseDoQueryHost");
            try
            {
                if (_doQueryTrader != null && _doQueryTrader.State != CommunicationState.Closed)
                    _doQueryTrader.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            LogHelper.WriteDebug("end CloseDoQueryHost");
        }

        /// <summary>
        /// 关闭公共查询服务
        /// </summary>
        private void CloseDoCommonQueryServiceHost()
        {
            LogHelper.WriteDebug("begin CloseDoQueryServiceHost");
            try
            {
                if (_doCommonQuery != null && _doCommonQuery.State != CommunicationState.Closed)
                    _doCommonQuery.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            LogHelper.WriteDebug("end CloseDoQueryServiceHost");
        }

        /// <summary>
        /// 关闭港股公共查询服务
        /// </summary>
        private void CloseDoHKCommonQueryServiceHost()
        {
            LogHelper.WriteDebug("begin CloseHKDoQueryServiceHost");
            try
            {
                if (_doHKCommonQuery != null && _doHKCommonQuery.State != CommunicationState.Closed)
                    _doHKCommonQuery.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            LogHelper.WriteDebug("end CloseDoHKQueryServiceHost");
        }

        /// <summary>
        /// 关闭港股Find公共查询服务
        /// </summary>
        private void CloseDoHKQueryServiceHost()
        {
            LogHelper.WriteDebug("begin CloseDoHKFindServiceHost");
            try
            {
                if (_doHKQuery != null && _doHKQuery.State != CommunicationState.Closed)
                    _doHKQuery.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            LogHelper.WriteDebug("end CloseDoHKFindServiceHost");
        }

        /// <summary>
        /// 关闭成交回报服务
        /// </summary>
        private void CloseDoDealRptHost()
        {
            LogHelper.WriteDebug("begin CloseDoDealRptHost");
            try
            {
                if (_doDealRptHost != null && _doDealRptHost.State != CommunicationState.Closed)
                    _doDealRptHost.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            LogHelper.WriteDebug("end CloseDoDealRptHost");
        }

        /// <summary>
        /// 关闭下单服务
        /// </summary>
        private void CloseDoOrderHost()
        {
            LogHelper.WriteDebug("begin CloseDoOrderHost");
            try
            {
                if (_doOrderHost != null && _doOrderHost.State != CommunicationState.Closed)
                    _doOrderHost.Abort();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            LogHelper.WriteDebug("end CloseDoOrderHost");
        }

        #endregion

        #region Open Service

        /// <summary>
        /// 启动账号管理服务
        /// </summary>
        /// <param name="strErrorMsg">异常信息</param>
        /// <returns>是否启动</returns>
        private bool OpenManagementHost(ref string strErrorMsg)
        {
            bool result = false;
            try
            {
                //帐户管理
                _commonParaHost = new ServiceHost(typeof (AccountAndCapitalManagementService));
                _commonParaHost.Faulted += _commonParaHost_Faulted;
                _commonParaHost.Open();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = "无法启动帐户管理服务，请检查配置后重新启动程序！";
                strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;
                LogHelper.WriteError(msg, ex);
            }

            return result;
        }

        /// <summary>
        /// 启动交易员查询服务
        /// </summary>
        /// <param name="strErrorMsg">异常信息</param>
        /// <returns>是否启动</returns>
        private bool OpenDoQueryHost(ref string strErrorMsg)
        {
            bool result = false;
            try
            {
                //公布查询服务
                _doQueryTrader = new ServiceHost(typeof (TraderFindService));
                _doQueryTrader.Faulted += _doQueryTrader_Faulted;
                _doQueryTrader.Open();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = "无法启动查询服务，请检查配置后重新启动程序！";
                strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;

                LogHelper.WriteError(msg, ex);
            }
            return result;
        }

        /// <summary>
        /// 开启公共查询服务
        /// </summary>
        /// <param name="strErrorMsg">开启相关异常</param>
        /// <returns></returns>
        private bool OpenDoCommonQueryHost(ref string strErrorMsg)
        {
            bool result = false;
            try
            {
                //开启公共查询服务
                _doCommonQuery = new ServiceHost(typeof (TraderQueryService));
                _doCommonQuery.Faulted += _doCommonQuery_Faulted;
                _doCommonQuery.Open();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = "无法启动查询服务，请检查配置后重新启动程序！";
                strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;

                LogHelper.WriteError(msg, ex);
            }
            return result;
        }

        /// <summary>
        /// 开启港股公共查询服务
        /// </summary>
        /// <param name="strErrorMsg">开启相关异常</param>
        /// <returns></returns>
        private bool OpenDoHKCommonQueryHost(ref string strErrorMsg)
        {
            bool result = false;
            try
            {
                //开启公共查询服务
                _doHKCommonQuery = new ServiceHost(typeof (HKTraderQueryService));
                _doHKCommonQuery.Faulted += _doHKCommonQuery_Faulted;
                _doHKCommonQuery.Open();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = "无法启动港股查询服务，请检查配置后重新启动程序！";
                strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;

                LogHelper.WriteError(msg, ex);
            }
            return result;
        }
        /// <summary>
        /// 开启港股Find公共查询服务(些Find服务的查询方法应用于ROE)
        /// </summary>
        /// <param name="strErrorMsg">开启相关异常</param>
        /// <returns></returns>
        private bool OpenDoHKQueryHost(ref string strErrorMsg)
        {
            bool result = false;
            try
            {
                //开启公共查询服务
                _doHKQuery = new ServiceHost(typeof(HKTraderFindService));
                _doHKQuery.Faulted += _doHKQuery_Faulted;
                _doHKQuery.Open();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = "无法启动港股Find公共查询服务，请检查配置后重新启动程序！";
                strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;

                LogHelper.WriteError(msg, ex);
            }
            return result;
        }

        /// <summary>
        /// 启动下单服务
        /// </summary>
        /// <param name="strErrorMsg">异常信息</param>
        /// <returns>是否启动</returns>
        private bool OpenDoOrderHost(ref string strErrorMsg)
        {
            bool result = false;
            //公布下单服务
            try
            {
                _doOrderHost = new ServiceHost(typeof (DoOrderService));
                //_doOrderHost = new ServiceHost(new DoOrderService());
                _doOrderHost.Faulted += _doOrderHost_Faulted;
                _doOrderHost.Open();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = "无法启动下单服务，请检查配置后重新启动程序！";
                strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;

                LogHelper.WriteError(msg, ex);
            }
            return result;
        }

        /// <summary>
        /// 启动成交回报服务
        /// </summary>
        /// <param name="strErrorMsg">异常信息</param>
        /// <returns>是否启动</returns>
        private bool OpenDoDealRptHost(ref string strErrorMsg)
        {
            bool result = false;
            //公布回推服务
            try
            {
                _doDealRptHost = new ServiceHost(CounterOrderService.Instance);
                _doDealRptHost.Faulted += _doDealRptHost_Faulted;
                _doDealRptHost.Open();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = "无法启动回推服务，请检查配置后重新启动程序！";
                strErrorMsg = msg + Environment.NewLine + "错误信息如下：" + Environment.NewLine + ex.Message;

                LogHelper.WriteError(msg, ex);
            }
            return result;
        }

        #endregion

        #region Faulted Event

        private void _commonParaHost_Faulted(object sender, EventArgs e)
        {
            string msg = "******************_commonParaHost_Faulted******************";
            LogHelper.WriteInfo(msg);
            //OpenManagementHost();
        }

        private void _doQueryTrader_Faulted(object sender, EventArgs e)
        {
            string msg = "******************_doQueryTrader_Faulted******************";
            LogHelper.WriteInfo(msg);
            //OpenDoQueryHost();
        }

        /// <summary>
        /// 委托记录开启服务异常信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _doCommonQuery_Faulted(object sender, EventArgs e)
        {
            string msg = "******************_doCommonQuery_Faulted******************";
            LogHelper.WriteInfo(msg);
            //OpenDoCommonQueryHost();
        }

        private void _doHKCommonQuery_Faulted(object sender, EventArgs e)
        {
            string msg = "******************_doHKCommonQuery_Faulted******************";
            LogHelper.WriteInfo(msg);
            //OpenDoHKCommonQueryHost();
        }
        /// <summary>
        /// 开启港股Find共公查询方法异常处理事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _doHKQuery_Faulted(object sender, EventArgs e)
        {
            string msg = "******************_doHKQuery_Faulted******************";
            LogHelper.WriteInfo(msg);
            //OpenDoHKCommonQueryHost();
        }

        private void _doOrderHost_Faulted(object sender, EventArgs e)
        {
            string msg = "******************_doOrderHost_Faulted******************";
            LogHelper.WriteInfo(msg);
            //OpenDoOrderHost();
        }

        private void _doDealRptHost_Faulted(object sender, EventArgs e)
        {
            string msg = "******************_doDealRptHost_Faulted******************";
            LogHelper.WriteInfo(msg);
            //OpenDoDealRptHost();
        }

        #endregion
    }
}