using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL;
using ManagementCenter.BLL.WCFService_Out;

namespace ManagementCenterService.UI
{
    /// <summary>
    /// 描述：虚拟交易系统后台管理中心服务界面
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// 描述：添加日志
    /// 修改作者：刘书伟
    /// 修改日期：2010-04-23
    /// </summary>
    /// </summary>
    public partial class Form1 : Form
    {

        //现货，期货公共服务Host
        ServiceHost commonParaHost;

        //现货服务Host
        ServiceHost spotTradeRulesHost;

        //期货服务Host
        ServiceHost futuresTradeRulesHost;

        //用户管理服务Host
        ServiceHost transactionManageHost;

        //港股服务Host
        ServiceHost hkTradeRulesHost;


        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region 主页面加载事件
        /// <summary>
        /// 主页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            ManagementCenter.BLL.WCFService_Out.TransactionManageService Tms = new TransactionManageService();
            Tms.StartUpdateService();//每次启动管理中心服务时更新同步数据
            try
            {
                commonParaHost = new ServiceHost(typeof(CommonParaService));
                commonParaHost.Open();


                spotTradeRulesHost = new ServiceHost(typeof(SpotTradeRulesService));
                spotTradeRulesHost.Open();


                futuresTradeRulesHost = new ServiceHost(typeof(FuturesTradeRulesService));
                futuresTradeRulesHost.Open();


                transactionManageHost = new ServiceHost(typeof(TransactionManageService));
                transactionManageHost.Open();

                hkTradeRulesHost = new ServiceHost(typeof(HKTradeRulesService));
                hkTradeRulesHost.Open();
            }
            catch (Exception ex)
            {
                this.Text = "管理中心服务开启失败";
                LogHelper.WriteError("管理中心服务开启失败", ex); //ex.InnerException);
                throw ex;
            }
            StartTestConn();
            StartUpdate();
        }
        #endregion

        #region  窗体关闭事件
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                commonParaHost.Close();
                spotTradeRulesHost.Close();
                futuresTradeRulesHost.Close();
                transactionManageHost.Close();
                hkTradeRulesHost.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("窗体关闭事件失败", ex.InnerException);
            }
        }
        #endregion

        #region  检测连接和更新
        /// <summary>
        /// 检测连接和更新
        /// </summary>
        public static void StartTestConn()
        {
            Thread _Thread = new Thread(new ThreadStart(TestConnState));
            _Thread.IsBackground = true;
            _Thread.Start();
        }

        /// <summary>
        /// 测试柜台服务和撮合服务连接状态
        /// </summary>
        private static void TestConnState()
        {
            while (true)
            {
                int timegap = 0;
                try
                {
                    ManagementCenter.BLL.CT_CounterBLL CounterBLL = new CT_CounterBLL();
                    CounterBLL.CenterTestConnection();
                    ManagementCenter.BLL.RC_MatchCenterBLL MatchCenterBLL = new RC_MatchCenterBLL();
                    MatchCenterBLL.CenterTestConnection();
                    if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TestConnGapTime"].ToString(),
                                 out timegap))
                    {
                        timegap = 600000;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("测试柜台和撮合服务是否连接的方法失败", ex.InnerException);
                }
                Thread.Sleep(timegap);
            }
        }

        /// <summary>
        /// 开始启动更新服务
        /// </summary>
        private static void StartUpdate()
        {
            ManagementCenter.BLL.WCFService_Out.TransactionManageService Tms = new TransactionManageService();
            Tms.Start();
        }
        #endregion

    }
}