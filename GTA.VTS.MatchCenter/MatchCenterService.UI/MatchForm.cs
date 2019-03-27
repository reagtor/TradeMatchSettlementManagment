using System;
using System.Configuration;
using System.ServiceModel;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using CommonRealtimeMarket;
using GTAMarketSocket;
using MatchCenter.BLL;
using GTA.VTS.Common.CommonUtility;
using System.Xml;
using MatchCenter.BLL.Common;
using MatchCenter.BLL.Service;
using System.Timers;
using Timer = System.Timers.Timer;
using MatchCenter.BLL.ManagementCenter;
using MatchCenter.BLL.PushBack;
using MatchCenter.BLL.RealTime;
using MatchCenter.BLL.MatchRules;
//using RealTime.Server.Handler;
using RealTime.Common.CommonClass;
using GTA.VTS.Common.CommonObject;

namespace MatchServiceManager
{
    /// <summary>
    /// Title:撮合中心主窗体
    /// Create by:李健华
    /// Create date :2009-10-10
    /// Update By:董鹏
    /// Update Date:2010-02-25
    /// Desc.:增加连接管理中心失败后，重新连接的功能
    /// </summary>
    public partial class MatchForm : Form, IRealtimeMarketView
    {
        #region comment date:2009-08-18
        //private MatchDevice device = null;
        //private DateTime startTime;
        //private DateTime endTime;
        #endregion

        #region 变量定义
        /// <summary>
        /// 当前撮合成交总数据（即接收到的委托总数）
        /// </summary>
        private int matchCount = 0;
        /// <summary>
        /// 上次计算撮合成交总数
        /// </summary>
        private int lastComputerMachtCount;
        /// <summary>
        /// 当前报盘总数
        /// </summary>
        private int offerCount;
        /// <summary>
        /// 上次计算报盘总数
        /// </summary>
        private int lastComputeOfferCount;
        /// <summary>
        /// 用户等待的毫秒数才能提交条件
        /// </summary>
        private const int MillTime = 10000;
        /// <summary>
        /// 当前时间
        /// </summary>
        private DateTime m_CurTime = DateTime.Now;

        private DateTime StartTime;
        ///// <summary>
        ///// 定时检查服务器
        ///// </summary>
        //private Timer CheckServiceTimer;
        ///// <summary>
        ///// 计时器
        ///// </summary>
        //private System.Timers.Timer MatchTime = new System.Timers.Timer();
        ///// <summary>
        ///// 定时器--检查管理中心连接是否正常
        ///// </summary>
        //private System.Timers.Timer CheckManagerCenterConnTimer;

        //private ISocket _socketService;

        private string strTime = string.Empty;
        /// <summary>
        /// 下单服务宿主
        /// </summary>
        private ServiceHost doServiceHost;
        /// <summary>
        ///  成交回报服务宿主
        /// </summary>
        private ServiceHost OrderDealRptHost;
        #endregion

        #region 构造函数 此包括加载相关的定时器
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatchForm()
        {
            OnLoadInitializeTimer();
            InitializeComponent();
        }
        #endregion

        #region 加载相关定时器事件
        /// <summary>
        /// 加载相关定时器事件
        /// </summary>
        private void OnLoadInitializeTimer()
        {
            #region 加载统计撮合成交速度定时器
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += TestSpeed;
            timer.Enabled = true;
            #endregion

            #region 加载统计报盘速度定时器
            System.Timers.Timer timerWork = new System.Timers.Timer();
            timerWork.Interval = 1000;
            timerWork.Elapsed += TestWorkSpeed;
            timerWork.Enabled = true;
            #endregion

            #region 加载定时检查与管理中心连接服务时间器
            Timer CheckManagerCenterConnTimer = new Timer();
            CheckManagerCenterConnTimer.Interval = 60 * 60 * 1000;
            CheckManagerCenterConnTimer.Elapsed += CheckManagerCenterConnTimer_Elapsed;
            CheckManagerCenterConnTimer.Enabled = true;
            #endregion

            #region 加载定时检查服务时间器
            Timer CheckServiceTimer = new Timer();
            CheckServiceTimer.Interval = 60 * 1000;
            CheckServiceTimer.Elapsed += OnTimerCheckServiceElapsed;
            CheckServiceTimer.Enabled = true;
            #endregion
        }
        #endregion

        #region 窗体相关事件
        /// <summary>
        /// 窗体加载时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            ShowMarketSimulate();

            //开启初始化线程
            Thread thread = new Thread(InitInitializeThread);
            thread.Start();

            #region old
            //if (DoStartSevice())
            //{
            //    try
            //    {
            //        InitRealtimeMarketComponent();
            //    }
            //    catch (Exception ex)
            //    {
            //        string errMsgRe = "===撮合中心初始化行情服务失败";
            //        LogHelper.WriteError(errMsgRe, ex);
            //        this.Invoke(new MethodInvoker(() => this.Text = RulesDefaultValue.title_ZH + errMsgRe));
            //    }
            //    //this.lblMessage.Text = "";
            //    this.label6.Visible = false;
            //    //  lblMessage.Visible = false;
            //    Thread thread = new Thread(InitMatch);
            //    thread.Start();
            //    //InitMatch();
            //}
            //else
            //{
            //    string errMsg = "撮合中心服务不能启动!";
            //    MessageBox.Show(errMsg, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //    Application.Exit();
            //}
            #endregion
        }

        /// <summary>
        /// 初始化线程
        /// </summary>
        private void InitInitializeThread()
        {
            this.label6.Visible = false;
            try
            {
                //初始化行情
                InitRealtimeMarketComponent();

                //初始化撮合
                InitMatch();

                //启动WCF服务
                DoStartSevice();
            }
            catch (Exception ee)
            {
                LogHelper.WriteError("[撮合中心启动失败]：" + ee.Message, ee);
            }
        }

        /// <summary>
        /// 窗体正在关闭时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogHelper.WriteDebug(DateTime.Now + "撮合中心正在退出.....");
            LogHelper.WriteDebug(DateTime.Now + "==正在做关闭前的撤单数据故障恢复保存(请勿强制关闭)……");
            //ShowMessage.Instanse.ShowFormTitleMessage("==正在做关闭前的撤单数据故障恢复保存(请勿强制关闭)……");
            MatchCenterManager.Instance.SaveAllCancleData();
            //ShowMessage.Instanse.ShowFormTitleMessage("==撤单数据故障恢复保存完毕……");
            LogHelper.WriteDebug(DateTime.Now + "==撤单数据故障恢复保存完毕……");

            CloseDoServiceHost();
            LogHelper.WriteDebug(DateTime.Now + "撮合中心退出成功.....");
        }
        #endregion

        #region 服务开启、关闭、检查
        #region 服务检查 如果服务为null或者关闭内存自动再开启

        #region 下单服务检查
        /// <summary>
        /// 下单服务检查
        /// </summary>
        private void CheckServiceHost()
        {
            if (doServiceHost == null)
            {
                LogHelper.WriteDebug("doServiceHost is not open !");
                OpendoServiceHost();
                return;
            }
            if (doServiceHost.State == CommunicationState.Closed || doServiceHost.State == CommunicationState.Faulted)
            {
                LogHelper.WriteDebug("doServiceHost has been Closed!");
                OpendoServiceHost();

            }
        }
        #endregion

        #region 成交回报服务检查
        /// <summary>
        /// 成交回报服务检查
        /// </summary>
        private void CheckDealRptHost()
        {
            if (OrderDealRptHost == null)
            {

                LogHelper.WriteDebug("OrderDealRptHost is not open !");
                OpenDealRptHost();
                return;
            }
            if (OrderDealRptHost.State == CommunicationState.Closed || OrderDealRptHost.State == CommunicationState.Faulted)
            {
                LogHelper.WriteDebug("OrderDealRptHost has been Closed!");
                OpenDealRptHost();

            }
        }
        #endregion

        #region 检查服务
        /// <summary>
        /// 检查服务
        /// </summary>
        private void CheckService()
        {
            try
            {
                CheckDealRptHost();
                CheckServiceHost();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(DateTime.Now + "检查服务失败：", ex);
            }
        }
        #endregion

        #endregion

        #region 服务开启
        #region 启动服务
        /// <summary>
        /// 启动服务
        /// </summary>
        private bool DoStartSevice()
        {
            try
            {
                #region update 2009-08-18 update以下===
                //doServiceHost = new ServiceHost(typeof(DoOrderService));
                //OrderDealRptHost = new ServiceHost(typeof(OrderDealRpt));
                //doServiceHost.Open();
                //OrderDealRptHost.Open();
                #endregion
                //===
                OpendoServiceHost();
                OpenDealRptHost();
                //====
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("[撮合中心启动失败]：" + ex.Message, ex);
                //增加异常抛出，去掉了返回false，by 董鹏 2010-03-17
                throw ex;
                //return false;
            }
        }
        #endregion

        #region 打开下单服务
        /// <summary>
        /// 打开下单服务
        /// </summary>
        private void OpendoServiceHost()
        {
            try
            {
                doServiceHost = new ServiceHost(typeof(DoOrderService));
                doServiceHost.Open();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("[撮合中心下单服务开启失败]：", ex);
                throw ex;
            }
        }
        #endregion

        #region 打开成交回报服务
        /// <summary>
        /// 打开成交回报服务
        /// </summary>
        private void OpenDealRptHost()
        {
            try
            {
                OrderDealRptHost = new ServiceHost(typeof(OrderDealRpt)); //update date2008-08-18                
                OrderDealRptHost.Open();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("[撮合中心成交回报服务开启失败]：", ex);
                throw ex;
            }
        }
        #endregion
        #endregion

        #region  定期检查服务事件
        /// <summary>
        /// 定期检查服务事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerCheckServiceElapsed(object sender, ElapsedEventArgs args)
        {
            CheckService();
        }
        #endregion

        #region 关闭所有服务包括与管理中心的客户端连接工厂对象
        /// <summary>
        /// 关闭所有服务包括与管理中心的客户端连接工厂对象
        /// </summary>
        private void CloseDoServiceHost()
        {
            try
            {
                //ManagementCenterDataAgent.Instanse.MatchClost();
                if (doServiceHost != null && doServiceHost.State != CommunicationState.Closed)
                    doServiceHost.Close();
                if (OrderDealRptHost != null && OrderDealRptHost.State != CommunicationState.Closed)
                    OrderDealRptHost.Close();
                //CloseConnManagerCenterClinet();
                // ManagementCenterDataAgent.Instanse.ClearService();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        #region 定时检查与管理中心连接是否正常事件，如果不正常即操作关闭撮合中心
        /// <summary>
        /// 定时检查与管理中心连接是否正常事件，如果不正常即操作关闭撮合中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckManagerCenterConnTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ////if (InitCenterParmeterInstanse.Instanse.IsSuccess() == false)
            //if (!GetAllCommonDataFromManagerCenter.IsConnManagerCenterSuccess())
            //{
            //    LogHelper.WriteError("管理中心连接不上，请检查管理中心是否开启！", new Exception(""));
            //    return;
            //}
            //// CloseConnManagerCenterClinet();

            #region 增加连接管理中心失败后，重新连接的功能 modify by 董鹏 2010-02-25
            int retryTimes = 0;
            while (!CommonDataCacheProxy.Instanse.TestConnManagerCenterSuccess())
            {
                LogHelper.WriteError("管理中心连接不上，请检查管理中心是否开启！", new Exception(""));
                Thread.Sleep(5000);
                retryTimes++;
                ShowMessage.Instanse.ShowFormTitleMessage("==连接管理中心失败，正在尝试第" + retryTimes + "次重新连接……");
            }
            ShowMessage.Instanse.ShowFormTitleMessage(AppConfig.MatchCenterName);
            #endregion
        }
        #endregion
        #endregion

        ///// <summary>
        ///// 启动撮合进程
        ///// </summary>
        //private void DoMatchWork()
        //{
        //    MatchTime.Elapsed += this.OnTimerElapsed;
        //    MatchTime.Interval = MillTime;
        //    MatchTime.Enabled = false;
        //    InitMatch();
        //}

        ///// <summary>
        ///// 定时刷新服务
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //private void OnTimerElapsed(object sender, ElapsedEventArgs args)
        //{
        //    if (string.IsNullOrEmpty(strTime))
        //    {
        //        return;
        //    }
        //    //StartTime = GetStartTime();
        //    StartTime = Utils.ConvertToNowDateTime(AppConfig.GetConfigBeginTime());
        //    //strTime = System.Configuration.ConfigurationManager.AppSettings["BeginTime"];
        //    if (((TimeSpan)DateTime.Now.Subtract(StartTime)).TotalMilliseconds <= MillTime && ((TimeSpan)DateTime.Now.Subtract(StartTime)).TotalMilliseconds >= 0)
        //    {
        //        InitMatch();
        //    }

        //}

        /// <summary>
        /// 启动撮合
        /// </summary>
        private void InitMatch()
        {
            try
            {
                ShowMessage.Instanse.AppForm = this;
                //this.Invoke(new MethodInvoker(() => this.Text = RulesDefaultValue.title_ZH + "==系统正在初始化中，请稍候……"));
                ShowMessage.Instanse.ShowFormTitleMessage("==系统正在初始化中，请稍候……");

                #region old
                //// if (InitCenterParmeterInstanse.Instanse.IsSuccess() == false)
                //if (!GetAllCommonDataFromManagerCenter.IsConnManagerCenterSuccess())
                //{
                //    ShowMessage.Instanse.ShowFormTitleMessage("==无法连接管理中心，初始化失败……");
                //    //this.Invoke(new MethodInvoker(() => this.Text = RulesDefaultValue.title_ZH + "==无法连接管理中心，初始化失败……"));
                //    return;
                //}
                #endregion

                #region 增加连接管理中心失败后，重新连接的功能 modify by 董鹏 2010-02-25
                int retryTimes = 0;
                while (!CommonDataCacheProxy.Instanse.TestConnManagerCenterSuccess())
                {
                    Thread.Sleep(5000);
                    retryTimes++;
                    ShowMessage.Instanse.ShowFormTitleMessage("==连接管理中心失败，正在尝试第" + retryTimes + "次重新连接……");
                }
                #endregion

                RealtimeMarketService.ListQHHQWork = this.listQHHQ;
                RealtimeMarketService.ListXHHQWork = this.listXHHQ;
                RealtimeMarketService.ListHKHQWork = this.listHKHQ;
                RealtimeMarketService.ListCFHQWork = this.listCFHQ;

                #region 显示信息ListBox控件注册
                //MatchCenterManager.Instance.ListMessage = this.ltbMatchMsg;
                //MatchCenterManager.Instance.ListWork = this.ltbSpeedMsg;    
                //MatchCenterManager.Instance.ProcessEvent -= Match_Back;
                //MatchCenterManager.Instance.ProcessWorkEvent -= Work_Back;
                //MatchCenterManager.Instance.ProcessEvent += Match_Back;
                //MatchCenterManager.Instance.ProcessWorkEvent += Work_Back;

                ShowMessage.Instanse.ProcessEvent -= Match_Back;
                ShowMessage.Instanse.ProcessWorkEvent -= Work_Back;
                ShowMessage.Instanse.ProcessEvent += Match_Back;
                ShowMessage.Instanse.ProcessWorkEvent += Work_Back;
                ShowMessage.Instanse.ListBoxShowMatchMessage = this.ltbMatchMsg;
                ShowMessage.Instanse.ListBoxShowOfferMessage = this.ltbSpeedMsg;
                #endregion


                InitMatchCenter.Instanse.InitMatchStart();
                InitDealBack();
                ShowMessage.Instanse.ShowFormTitleMessage("==初始化完成……");
                ShowMessage.Instanse.ShowFormTitleMessage(AppConfig.MatchCenterName);

                //加载每日开关市时间事件
                MarketOpenClose.StartOpenTimerInit();



            }
            catch (Exception ex)
            {
                LogHelper.WriteError("启动撮合异常", ex);
                //增加异常抛出，by 董鹏 2010-03-17
                throw ex;
            }
        }

        #region == 行情初始化 ==

        private GTASocket SocketService = null;

        //private IRealtimeMarketService rms = null;
        /// <summary>
        ///初始化行情
        /// </summary>
        void InitRealtimeMarketComponent()
        {
            //增加了try部分，by 董鹏 2010-03-17
            try
            {
                SocketService = null;
                //rms = null;
                int mode = AppConfig.GetConfigRealTimeMode();

                if (mode == 2)
                {
                    InitRealtimeMarketComponent2();
                    return;
                }

                InitRealtimeMarketComponent1();
            }
            catch (Exception ex)
            {
                string errMsgRe = "===撮合中心初始化行情服务失败";
                LogHelper.WriteError(errMsgRe, ex);
                this.Invoke(new MethodInvoker(() => this.Text = RulesDefaultValue.title_ZH + errMsgRe));
                throw ex;
            }
        }

        /// <summary>
        /// 初始化行情组件1
        /// </summary>
        private void InitRealtimeMarketComponent1()
        {
            string userName = AppConfig.GetConfigRealTimeUserName();
            string passWord = AppConfig.GetConfigRealTimePassword();
            //登陆处理
            // GTASocketSingleton.StartLogin("rtuser", "11", ShowConnectMessage);
            GTASocketSingletonForRealTime.StartLogin(userName, passWord, ShowConnectMessage);
            //数据接受注册事件
            this.FEvent += Event;
            SocketService = GTASocketSingletonForRealTime.SocketService;
            GTASocketSingletonForRealTime.SocketStateChanged += GTASocketSingleton_SocketStateChanged;
            SocketService.AddEventDelegate(this.FEvent);
            GTASocketSingletonForRealTime.SocketStatusHandle(SocketStatusChange);

        }

        /// <summary>
        /// 初始化行情组件2
        /// </summary>
        private void InitRealtimeMarketComponent2()
        {
            ////登陆处理
            //rms = RealtimeMarket.factory.RealtimeMarketServiceFactory2.GetService();
            //string userName = AppConfig.GetConfigRealTimeUserName();
            //string passWord = AppConfig.GetConfigRealTimePassword();
            ////登陆处理
            //// GTASocketSingleton.StartLogin("rtuser", "11", ShowConnectMessage);

            //rms.Login(userName, passWord, ShowConnectMessage);
            //rms.InitializeService();

            ////数据接受注册事件
            //this.FEvent += Event;
            //_socketService = rms.SocketService;
            //_socketService.AddEventDelegate(this.FEvent);
            //rms.GetGTASingleton.SocketStatusHandle(SocketStatusChange);
        }

        /// <summary>
        /// Socket状态改变触发事件
        /// </summary>
        /// <param name="_e"></param>
        private delegate void DelegateSocketStatusChange(SocketServiceStatusEventArg _e);

        private void SocketStatusChange(SocketServiceStatusEventArg e)
        {
            if (this.statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new DelegateSocketStatusChange(delegate (SocketServiceStatusEventArg _e) { SocketStatusChange(e); }), e);
            }
            else
            {
                if (e.SocketStatus == SocketServiceStatus.SSSConnected || e.SocketStatus == SocketServiceStatus.SSSLogin)
                {
                    this.StatusLinkState.Text = "Socket服务连接状态：" + "已连接  ";
                    //this.StatusLinkState.Text += "数据库连接状态：" + "已连接";
                    //this.MenuResetState("中断连接");


                }
                else if (e.SocketStatus == SocketServiceStatus.SSSDisConnectedNeedReconnect)
                {
                    this.StatusLinkState.Text = "Socket服务连接状态：" + "已断开,启动自动重连接...";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSDisConnected)
                {

                    this.StatusLinkState.Text = "Socket服务连接状态：" + "已断开";
                    //this.MenuResetState("重新连接");

                }
                else if (e.SocketStatus == SocketServiceStatus.SSSException)
                {
                    this.StatusLinkState.Text = "Socket服务连接状态：" + "连接异常";
                    //this.MenuResetState("重新连接");
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSConnecting)
                {
                    this.StatusLinkState.Text = "Socket服务连接状态：" + e.Message.ToString();//"正在重新连接……";
                }
                else if (e.SocketStatus == SocketServiceStatus.SSSResetCycleIsEnding)
                {

                    int mode = AppConfig.GetConfigRealTimeMode();
                    this.StatusLinkState.Text = "Socket服务连接状态：" + e.Message.ToString();
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

        void GTASocketSingleton_SocketStateChanged(EnumSocketResetState state)
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
                    case SocketServiceStatus.SSSLogin:
                        //更新程序是否需要更新
                        //this.lblMessage.Text = "检测更新程序是否需要更新。";
                        this.StatusLinkState.Text = "检测更新程序是否需要更新。";
                        Application.DoEvents();
                        // UpdateUpdateSoftSelf();
                        //检测是否需要更新
                        //this.lblMessage.Text = "异步启动检测客户端是否需要更新。";
                        this.StatusLinkState.Text = "异步启动检测客户端是否需要更新。";
                        Application.DoEvents();
                        // this.lblMessage.Text = "已登陆行情服务器.";
                        this.StatusLinkState.Text = "已登陆行情服务器.";
                        Application.DoEvents();
                        this.DialogResult = DialogResult.OK;
                        break;
                    case SocketServiceStatus.SSSErrorUser:


                        MessageBox.Show(e.Message.ToString(), "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //this.lblMessage.Text = "登陆失败，请重新登陆！";
                        this.StatusLinkState.Text = "登陆失败，请重新登陆！";
                        break;
                    case SocketServiceStatus.SSSException:
                        break;
                    case SocketServiceStatus.SSSResetCycleIsEnding:
                        //this.lblMessage.Text = e.Message;
                        this.StatusLinkState.Text = e.Message;
                        break;

                    case SocketServiceStatus.SSSDisConnected:
                        //  this.lblMessage.Text = e.Message;
                        this.StatusLinkState.Text = e.Message;
                        break;
                    case SocketServiceStatus.SSSSocksProxyError:
                        //this.lblMessage.Text = e.Message;
                        this.StatusLinkState.Text = e.Message;
                        break;
                    default:
                        //this.lblMessage.Text = e.Message;
                        this.StatusLinkState.Text = e.Message;
                        Application.DoEvents();
                        break;
                }
            }
        }

        /// <summary>
        /// 消息打印事件
        /// </summary>
        public OnEventDelegate FEvent;


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
            if (chkShowRealTimeMsg.Checked)
            {
                MessageDisplayHelper.Event(eventMessage, lstMessages);
            }
        }


        #region 从数据库获取数据初始化成交回报数据
        /// <summary>
        /// 从数据库获取数据初始化成交回报数据
        /// </summary>
        public void InitDealBack()
        {
            try
            {
                //InitDealBackStart.Instanse.InitDealBackData();
                //InitDealBackStart.Instanse.InitFutureDealBackData();
                TradePushBackImpl.Instanse.InitDealPushBackFromDataBase();

            }
            catch (Exception ex)
            {
                LogHelper.WriteDebug("[撮合中心初始化成交回报失败]：" + ex.Message);
                return;
            }
        }
        #endregion

        #endregion

        #region 关闭与管理中心连接的相关客户端操作对象和工厂设置为null
        ///// <summary>
        ///// 关闭与管理中心连接的相关客户端操作对象和工厂设置为null
        ///// </summary>
        //private void CloseConnManagerCenterClinet()
        //{
        //    try
        //    {
        //        //ManagementCenterDataAgent.Instanse.MatchClost();
        //        //ManagementCenterDataAgent.Instanse.ClearService();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(ex.Message, ex);
        //    }
        //}
        #endregion

        #region 计算速度事件
        #region 把委托报盘数量/成交数量显示到相应的TextBox中
        /// <summary>
        /// 委托报盘数量/成交数量
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="message"></param>
        private void SetMessage(TextBox textBox, string message)
        {
            try
            {
                if (!this.IsHandleCreated)
                    return;

                if (this.IsDisposed)
                    return;

                this.Invoke(new MethodInvoker(() => textBox.Text = message));
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        #region  定时计算报盘速度事件 并显示到相应的TextBox
        /// <summary>
        /// 测试测试报盘速度事件 并显示到相应的TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TestWorkSpeed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                decimal offset = offerCount - lastComputeOfferCount;
                decimal speed = offset / 1;

                if (speed != 0)
                    SetMessage(txtOfferSpeed, speed.ToString());

                lastComputeOfferCount = offerCount;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        #region 定时计算撮合成交速度事件 并显示到相应的TextBox
        /// <summary>
        /// 测试撮合成交速度事件并显示到相应的TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TestSpeed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                decimal offset = matchCount - lastComputerMachtCount;
                decimal speed = offset / 1;
                if (speed != 0)
                {
                    SetMessage(txtMatchSpeed, speed.ToString());
                }
                lastComputerMachtCount = matchCount;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        /// <summary>
        /// 成交回报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Match_Back(object sender, EventArgs e)
        {
            matchCount++;
            SetMessage(txtMatchCount, matchCount.ToString());
        }

        /// <summary>
        /// 实时计算委托报盘速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Work_Back(object sender, EventArgs e)
        {
            offerCount++;
            if (offerCount == 1)
            {
                StartTime = DateTime.Now;
            }
            if (offerCount > 1000)
            {
                CalculationTime(offerCount);
            }

            SetMessage(txtOfferCount, offerCount.ToString());
        }

        #region 实时计算报盘速度
        /// <summary>
        /// 实时计算报盘速度
        /// </summary>
        /// <returns></returns>
        private void CalculationTime(int sumcount)
        {
            int interal = 0;
            decimal average;
            decimal sum = sumcount * 1.0m;
            //TimeSpan span = DateTime.Now - StartTime;
            //interal = span.Hours * 3600 + span.Minutes * 60 + span.Seconds;
            interal = (int)Utils.TimeSpanSecondsToNowDateTime(StartTime);
            if (interal != 0)
            {
                average = sum / interal;
                this.label6.Text = "报盘平均速度: " + average.ToString();
            }
        }
        #endregion
        #endregion

        #region 清除报盘和委托显示列表信息按钮事件
        /// <summary>
        /// 清除报盘和委托显示列表信息按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        #region 清空事件把委托报盘等都设置为0
        /// <summary>
        /// 清空事件
        /// </summary>
        private void Clear()
        {
            matchCount = 0;
            lastComputerMachtCount = 0;
            offerCount = 0;
            lastComputeOfferCount = 0;
            this.txtMatchCount.Text = "0";
            this.txtMatchSpeed.Text = "0";
            this.txtOfferCount.Text = "0";
            this.txtOfferSpeed.Text = "0";
            this.ltbSpeedMsg.Items.Clear();
            this.ltbMatchMsg.Items.Clear();
        }
        #endregion

        private void chShowRealTimeMsg_CheckedChanged(object sender, EventArgs e)
        {
            if (chShowRealTimeMsg.Checked)
            {
                RealtimeMarketService.isShowRealTimeMsg = true;
            }
            else
            {
                RealtimeMarketService.isShowRealTimeMsg = false;
            }
        }

        #endregion

        #region 重启行情连接
        /// <summary>
        /// 重启行情连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRelTime_Click(object sender, EventArgs e)
        {
            try
            {
                InitRealtimeMarketComponent();
            }
            catch (Exception ex)
            {
                string errMsgRe = "===撮合中心重启行情服务失败";
                LogHelper.WriteError(errMsgRe, ex);
                this.Invoke(new MethodInvoker(() => this.Text = RulesDefaultValue.title_ZH + errMsgRe));
            }
        }
        #endregion

        #region 行情模拟

        /// <summary>
        /// 发送现货模拟行情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHSimulate_Click(object sender, EventArgs e)
        {
            btnXHSimulate.Enabled = false;
            try
            {
                RealtimeMarketSimulate.SendStockQuotes(this, Convert.ToInt32(txtXHBatch.Text), Convert.ToInt32(txtXHInterval.Text));
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }
            finally
            {
                btnXHSimulate.Enabled = true;
            }
        }

        /// <summary>
        /// 发送港股模拟行情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHKSimulate_Click(object sender, EventArgs e)
        {
            btnHKSimulate.Enabled = false;
            try
            {
                RealtimeMarketSimulate.SendHKQuotes(this, Convert.ToInt32(txtHKBatch.Text), Convert.ToInt32(txtHKInterval.Text));
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }
            finally
            {
                btnHKSimulate.Enabled = true;
            }
        }

        /// <summary>
        /// 发送股指期货模拟行情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSISimulate_Click(object sender, EventArgs e)
        {
            btnSISimulate.Enabled = false;
            try
            {
                RealtimeMarketSimulate.SendFutureQuotes(this, Convert.ToInt32(txtSIBatch.Text), Convert.ToInt32(txtSIInterval.Text));
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }
            finally
            {
                btnSISimulate.Enabled = true;
            }
        }

        /// <summary>
        /// 发送商品期货模拟行情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCFSimulate_Click(object sender, EventArgs e)
        {
            btnCFSimulate.Enabled = false;
            try
            {
                RealtimeMarketSimulate.SendCommoditiesQuotes(this, Convert.ToInt32(txtCFBatch.Text), Convert.ToInt32(txtCFInterval.Text));
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }
            finally
            {
                btnCFSimulate.Enabled = true;
            }
        }

        /// <summary>
        /// 获取商品期货实际行情作为模拟行情值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCFCode_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                RealtimeMarketSimulate.GetRealTimeCommditiesData(this, txtCFCode.Text);
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }

        }

        /// <summary>
        /// 获取现货实际行情作为模拟行情值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtXHCode_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                RealtimeMarketSimulate.GetRealTimeStockData(this, txtXHCode.Text);
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }
        }

        /// <summary>
        /// 获取股指期货实际行情作为模拟行情值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSICode_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                RealtimeMarketSimulate.GetRealTimeFutureData(this, txtSICode.Text);
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }
        }

        /// <summary>
        /// 获取港股实际行情作为模拟行情值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHKCode_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                RealtimeMarketSimulate.GetRealTimeHKData(this, txtHKCode.Text);
            }
            catch (Exception ee)
            {
                LogHelper.WriteError(ee.Message, ee);
            }
        }

        /// <summary>
        /// 是否启用模拟行情
        /// </summary>
        private void ShowMarketSimulate()
        {
            if (!AppConfig.GetConfigMarketSimulate())
            {
                tabControl1.TabPages.Remove(tabPage8);
            }
        }

        #region IRealtimeMarketView 成员
        /// <summary>品种类型</summary>
        public Types.BreedClassTypeEnum BreedClassType
        {
            get;
            set;
        }

        /// <summary>代码</summary>
        public string hqCode
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHCode.Text = value;
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKCode.Text = value;
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSICode.Text = value;
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFCode.Text = value;
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return txtXHCode.Text;
                    case Types.BreedClassTypeEnum.HKStock:
                        return txtHKCode.Text;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return txtSICode.Text;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return txtCFCode.Text;
                }
                return null;
            }
        }

        /// <summary>名称</summary>
        public string hqName
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHName.Text = value;
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKName.Text = value;
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIName.Text = value;
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFName.Text = value;
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return txtXHName.Text;
                    case Types.BreedClassTypeEnum.HKStock:
                        return txtHKName.Text;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return txtSIName.Text;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return txtCFName.Text;
                }
                return null;
            }
        }

        /// <summary>买1价</summary>
        public decimal hqBuyFirstPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyFirstPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyFirstPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyFirstPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyFirstPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyFirstPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyFirstPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyFirstPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyFirstPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>买1量</summary>
        public decimal hqBuyFirstVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyFirstVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyFirstVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyFirstVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyFirstVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyFirstVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyFirstVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyFirstVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyFirstVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>买2价</summary>
        public decimal hqBuySecondPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuySecondPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuySecondPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuySecondPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuySecondPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuySecondPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuySecondPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuySecondPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuySecondPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>买2量</summary>
        public decimal hqBuySecondVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuySecondVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuySecondVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuySecondVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuySecondVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuySecondVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuySecondVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuySecondVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuySecondVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>买3价</summary>
        public decimal hqBuyThirdPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyThirdPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyThirdPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyThirdPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyThirdPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyThirdPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyThirdPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyThirdPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyThirdPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>买3量</summary>
        public decimal hqBuyThirdVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyThirdVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyThirdVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyThirdVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyThirdVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyThirdVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyThirdVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyThirdVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyThirdVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>买4价</summary>
        public decimal hqBuyFourthPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyFourthPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyFourthPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyFourthPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyFourthPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyFourthPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyFourthPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyFourthPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyFourthPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>买4量</summary>
        public decimal hqBuyFourthVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyFourthVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyFourthVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyFourthVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyFourthVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyFourthVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyFourthVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyFourthVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyFourthVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>买5价</summary>
        public decimal hqBuyFivePrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyFivePrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyFivePrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyFivePrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyFivePrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyFivePrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyFivePrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyFivePrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyFivePrice.Text);
                }
                return 0;
            }
        }

        /// <summary>买5量</summary>
        public decimal hqBuyFiveVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHBuyFiveVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKBuyFiveVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIBuyFiveVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFBuyFiveVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHBuyFiveVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKBuyFiveVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIBuyFiveVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFBuyFiveVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>卖1价</summary>
        public decimal hqSellFirstPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellFirstPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellFirstPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellFirstPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellFirstPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellFirstPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellFirstPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellFirstPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellFirstPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>卖1量</summary>
        public decimal hqSellFirstVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellFirstVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellFirstVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellFirstVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellFirstVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellFirstVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellFirstVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellFirstVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellFirstVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>卖2价</summary>
        public decimal hqSellSecondPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellSecondPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellSecondPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellSecondPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellSecondPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellSecondPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellSecondPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellSecondPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellSecondPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>卖2量</summary>
        public decimal hqSellSecondVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellSecondVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellSecondVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellSecondVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellSecondVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellSecondVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellSecondVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellSecondVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellSecondVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>卖3价</summary>
        public decimal hqSellThirdPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellThirdPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellThirdPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellThirdPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellThirdPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellThirdPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellThirdPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellThirdPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellThirdPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>卖3量</summary>
        public decimal hqSellThirdVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellThirdVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellThirdVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellThirdVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellThirdVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellThirdVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellThirdVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellThirdVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellThirdVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>卖4价</summary>
        public decimal hqSellFourthPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellFourthPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellFourthPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellFourthPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellFourthPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellFourthPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellFourthPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellFourthPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellFourthPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>卖4量</summary>
        public decimal hqSellFourthVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellFourthVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellFourthVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellFourthVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellFourthVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellFourthVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellFourthVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellFourthVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellFourthVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>卖5价</summary>
        public decimal hqSellFivePrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellFivePrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellFivePrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellFivePrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellFivePrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellFivePrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellFivePrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellFivePrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellFivePrice.Text);
                }
                return 0;
            }
        }

        /// <summary>卖5量</summary>
        public decimal hqSellFiveVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHSellFiveVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKSellFiveVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSISellFiveVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFSellFiveVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHSellFiveVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKSellFiveVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSISellFiveVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFSellFiveVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>成交价</summary>
        public decimal hqLastPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHLastPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKLastPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSILastPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFLastPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHLastPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKLastPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSILastPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFLastPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>成交量</summary>
        public decimal hqLastVolume
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHLastVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKLastVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSILastVolume.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFLastVolume.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHLastVolume.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKLastVolume.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSILastVolume.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFLastVolume.Text);
                }
                return 0;
            }
        }

        /// <summary>跌停价</summary>
        public decimal hqLowerPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHLowerPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKLowerPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSILowerPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFLowerPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHLowerPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKLowerPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSILowerPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFLowerPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>涨停价</summary>
        public decimal hqUpPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHUpPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKUpPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIUpPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFUpPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHUpPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKUpPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIUpPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFUpPrice.Text);
                }
                return 0;
            }
        }

        /// <summary>昨日收盘价</summary>
        public decimal hqYesterPrice
        {
            set
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        txtXHYesterPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        txtHKYesterPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        txtSIYesterPrice.Text = value.ToString();
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        txtCFYesterPrice.Text = value.ToString();
                        break;
                }
            }
            get
            {
                switch (this.BreedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        return Convert.ToDecimal(txtXHYesterPrice.Text);
                    case Types.BreedClassTypeEnum.HKStock:
                        return Convert.ToDecimal(txtHKYesterPrice.Text);
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        return Convert.ToDecimal(txtSIYesterPrice.Text);
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        return Convert.ToDecimal(txtCFYesterPrice.Text);
                }
                return 0;
            }
        }

        #endregion



        #endregion

        #region comment date:2009-08-18
        //comment date:2009-08-18
        //private string GetCode(int iloop)
        // {
        //     string strResult = string.Empty;

        //     strResult = iloop.ToString();

        //     while (strResult.Length <= 6)
        //     {
        //         strResult = "0" + strResult;
        //     }

        //     return strResult;
        // } 
        //private void InitDevice()
        //{
        //    MatchDevice matchDevice = new MatchDevice();
        //    matchDevice.AcceptStartTime = System.DateTime.Now.AddDays(-1.00);
        //    matchDevice.AcceptEndTime = System.DateTime.Now.AddDays(1.00);
        //    List<string> listCode = GetList();
        //    if (listCode == null)
        //    {
        //        return;
        //    }
        //    foreach (var s in listCode)
        //    {
        //        StockMatcher stockMatcher = new StockMatcher(s);
        //        stockMatcher.MarkPartication = 0.5m;
        //        stockMatcher.RiseFallPric = 0.1m;
        //        matchDevice.StockMarkers.Add(s, stockMatcher);
        //        MatchCenter.BLL.MatchCenter.Instance.MatcherDevice.Add(s, matchDevice);
        //    }
        //}

        // private List<string> GetList()
        //{
        //    List<string> listCode = new List<string>();
        //   XmlDocument document = new XmlDocument();
        //   document.Load("SzMarket.xml");
        //   XmlNodeList nodeList = document.SelectNodes("Root//Item");
        //    if (nodeList==null)
        //    {
        //        return null;
        //    }
        //    foreach(XmlNode xn  in nodeList)
        //    {
        //        XmlNode node = xn.SelectSingleNode("CodeKey");
        //        if (node !=null)
        //        {
        //            listCode.Add(node.InnerXml);
        //        }
        //    }
        //    return listCode;
        //}
        ///// <summary>
        ///// 获取实时行情组件用户名
        ///// </summary>
        ///// <returns>实时行情组件用户名</returns>
        //public static string AppConfig.GetConfigRealTimeUserName()
        //{
        //    string userName = "rtuser";
        //    try
        //    {
        //        string name = ConfigurationManager.AppSettings["ServerUserName"];
        //        if (!string.IsNullOrEmpty(name))
        //        {
        //            userName = name;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(ex.Message, ex);
        //    }

        //    return userName;
        //}

        ///// <summary>
        ///// 获取实时行情组
        ///// </summary>
        ///// <returns>实时行情组件密码</returns>
        //public static string AppConfig.GetConfigRealTimePassword()
        //{
        //    string password = "11";
        //    try
        //    {
        //        string ps = ConfigurationManager.AppSettings["ServerPassword"];
        //        if (!string.IsNullOrEmpty(ps))
        //        {
        //            password = ps;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(ex.Message, ex);
        //    }

        //    return password;
        //}
        #endregion

    }
}
