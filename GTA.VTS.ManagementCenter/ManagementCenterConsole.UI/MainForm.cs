using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagementCenter.BLL;
using ManagementCenterConsole.UI.CommonClass;
using ManagementCenterConsole.UI.CommonParameterSet;
using ManagementCenterConsole.UI.CounterManage;
using ManagementCenterConsole.UI.FuturesRuleManageUI;
using ManagementCenterConsole.UI.HKRuleManageUI;
using ManagementCenterConsole.UI.ManagerManage;
using ManagementCenterConsole.UI.MatchCenterManage;
using ManagementCenterConsole.UI.SpotRuleManageUI;
using ManagementCenter.Model;

namespace ManagementCenterConsole.UI
{
    /// <summary>
    /// 虚拟交易系统后台管理中心主界面
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region 变量及属性

        #region 主窗体属性
        /// <summary>
        /// 主窗体属性
        /// </summary>
        private static MainForm instance;
        /// <summary>
        /// 主窗体属性
        /// </summary>
        public static MainForm Instance
        {
            get
            {
                if (instance == null) instance = new MainForm();
                return instance;
            }
        }
        #endregion

        #region 窗体成员变量

        /// <summary>
        /// 交易员管理主页面
        /// </summary>
        private TransactionManage.TransactionManageForm transactionManageForm;

        /// <summary>
        /// 管理员管理界面
        /// </summary>
        private ManagerManage.ManagerManage managerManageForm;

        /// <summary>
        /// 权限管理界面
        /// </summary>
        private ManagerManage.RightManage rightManageForm;

        /// <summary>
        /// 撮合管理界面
        /// </summary>
        private MatchCenterManage.MatchCenterManage matchCenterManageFrom;
        //清算柜台管理界面
        private CounterManage.CounterManger counterManger;

        /// <summary>
        /// 撮合配置向导窗体
        /// </summary>
        private MatchCenterManage.DispositionGuide m_DispositionGuide;

        #region 公共参数管理菜单项
        /// <summary>
        /// 交易所类型管理窗体
        /// </summary>
        private BourseManagerUI m_BourseManagerUI = null;

        /// <summary>
        /// 交易商品品种管理
        /// </summary>
        private BreedClassManagerUI m_BreedClassManagerUI = null;

        /// <summary>
        /// 商品代码管理
        /// </summary>
        private CommodityManagerUI m_CommodityManagerUI = null;

        /// <summary>
        /// 结算价管理
        /// </summary>
        private TodaySettlementPriceManagerUI m_TodaySettlementPriceManagerUI = null;

        #endregion

        #region 现货管理菜单项
        /// <summary>
        /// 最小交易单位管理窗体
        /// </summary>
        private MinVolumeOfBusinessManageUI m_MinVolumeOfBusinessManageUI = null;

        /// <summary>
        ///现货交易费用管理窗体
        /// </summary>
        private SpotCostsManageUI m_SpotCostsManageUI = null;

        /// <summary>
        /// 现货交易规则管理窗体
        /// </summary>
        private SpotTradeRulesManageUI m_SpotTradeRulesManageUI = null;

        /// <summary>
        /// 现货持仓限制管理窗体
        /// </summary>
        private SpotPositionManageUI m_SpotPositionManageUI = null;

        /// <summary>
        /// 现货单位换算管理
        /// </summary>
        private SpotUnitConversionManageUI m_SpotUnitConversionManageUI = null;

        #endregion


        #region 港股管理菜单项

        /// <summary>
        /// 港股代码管理
        /// </summary>
        private HKCommodityManagerUI m_HKCommodityManagerUI = null;

        /// <summary>
        /// 港股交易费用管理
        /// </summary>
        private HKCostsManageUI m_HKCostsManageUI = null;

        /// <summary>
        /// 港股交易规则管理
        /// </summary>
        private HKTradeRulesManageUI m_HKTradeRulesManageUI = null;

        #endregion

        #region 期货管理菜单项

        /// <summary>
        /// (商品)期货_持仓限制管理UI
        /// </summary>
        private PositionLimitValueManageUI m_PositionLimitValueManageUI = null;

        /// <summary>
        /// 商品期货_保证金比例管理UI
        /// </summary>
        private CFBailScaleValueManageUI m_CFBailScaleValueManageUI = null;

        /// <summary>
        /// 品种_期货_交易费用管理UI
        /// </summary>
        private FutureCostsManageUI m_FutureCostsManageUI = null;

        /// <summary>
        /// 熔断管理UI(股指期货)
        /// </summary>
        private CommodityFuseManageUI m_CommodityFuseManageUI = null;

        /// <summary>
        /// 股指期货持仓限制和保证金管理UI
        /// </summary>
        private SIFPositionAndBailManageUI m_SIFPositionAndBailManageUI = null;

        /// <summary>
        /// 期货交易规则管理UI
        /// </summary>
        private FuturesTradeRulesManageUI m_FuturesTradeRulesManageUI = null;
        #endregion

        #endregion

        #endregion

        //================================  私有  方法 ================================
        #region 菜单按纽事件 barButton_ItemClick
        /// <summary>
        /// 菜单按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pictureEdit1.SendToBack();//图片置于底层
                switch (e.Item.Name)
                {
                    case "AccountM_TransactionM":
                        if (transactionManageForm == null || transactionManageForm.IsDisposed)
                        {
                            transactionManageForm =
                                new ManagementCenterConsole.UI.TransactionManage.TransactionManageForm();
                            transactionManageForm.MdiParent = this;
                            transactionManageForm.Dock = DockStyle.Fill;
                            transactionManageForm.WindowState = FormWindowState.Maximized;
                            transactionManageForm.Show();
                            transactionManageForm.Closed += new EventHandler(ChildForm_Closed);
                        }
                        transactionManageForm.BringToFront();
                        break;
                    case "AccountM_ManagerM":
                        if (managerManageForm == null || managerManageForm.IsDisposed)
                        {
                            managerManageForm = new ManagerManage.ManagerManage();
                            managerManageForm.MdiParent = this;
                            managerManageForm.Dock = DockStyle.Fill;
                            managerManageForm.WindowState = FormWindowState.Maximized;
                            managerManageForm.Show();
                            managerManageForm.Closed += new EventHandler(ChildForm_Closed);

                        }
                        managerManageForm.BringToFront();
                        break;
                    case "AccountM_RightM":
                        if (rightManageForm == null || rightManageForm.IsDisposed)
                        {
                            rightManageForm = new RightManage();
                            rightManageForm.MdiParent = this;
                            rightManageForm.Dock = DockStyle.Fill;
                            rightManageForm.WindowState = FormWindowState.Maximized;
                            rightManageForm.Show();
                            rightManageForm.Closed += new EventHandler(ChildForm_Closed);

                        }
                        rightManageForm.BringToFront();
                        break;
                    case "CounterM_ConfigM":
                        // new CounterManage.CounterManger().ShowDialog();
                        if (counterManger == null || counterManger.IsDisposed)
                        {
                            counterManger = new CounterManger();
                            counterManger.MdiParent = this;
                            counterManger.Dock = DockStyle.Fill;
                            counterManger.WindowState = FormWindowState.Maximized;
                            counterManger.Show();
                            counterManger.Closed += new EventHandler(ChildForm_Closed);

                        }
                        counterManger.BringToFront();
                        break;

                    case "MatchM_CenterM":
                        if (matchCenterManageFrom == null || matchCenterManageFrom.IsDisposed)
                        {
                            matchCenterManageFrom = new MatchCenterManage.MatchCenterManage();
                            matchCenterManageFrom.MdiParent = this;
                            matchCenterManageFrom.Dock = DockStyle.Fill;
                            matchCenterManageFrom.WindowState = FormWindowState.Maximized;
                            matchCenterManageFrom.Show();
                            matchCenterManageFrom.Closed += new EventHandler(ChildForm_Closed);
                        }
                        matchCenterManageFrom.BringToFront();
                        break;
                    case "MatchM_GuideM":
                        //new MatchCenterManage.DispositionGuide().ShowDialog();
                        this.pictureEdit1.BringToFront(); //弹出窗体时图片显示
                        m_DispositionGuide = new DispositionGuide();
                        m_DispositionGuide.ShowDialog();
                        break;
                    case "PersonalM":
                        ManagerManage.ManagerEdit managerEdit = new ManagerEdit();
                        UM_UserInfo UserInfo = new UM_UserInfo();
                        ManagementCenter.Model.CommonClass.UtilityClass.CopyEntityToEntity(CommonClass.ParameterSetting.Mananger, UserInfo);
                        managerEdit.UserInfo = UserInfo;
                        managerEdit.EditType = 2;
                        managerEdit.ispersonedit = true;
                        this.pictureEdit1.BringToFront(); //弹出窗体时图片显示
                        managerEdit.ShowDialog();
                        break;
                    case "MenuHelpM_About":
                        this.pictureEdit1.BringToFront(); //弹出窗体时图片显示
                        FrmAbout frmAbout = new FrmAbout();
                        frmAbout.ShowDialog();
                        break;
                    //工具栏项
                    case "Menutool_TransactionM":
                        if (transactionManageForm == null || transactionManageForm.IsDisposed)
                        {
                            transactionManageForm =
                                new ManagementCenterConsole.UI.TransactionManage.TransactionManageForm();
                            transactionManageForm.MdiParent = this;
                            transactionManageForm.Dock = DockStyle.Fill;
                            transactionManageForm.WindowState = FormWindowState.Maximized;
                            transactionManageForm.Show();
                            transactionManageForm.Closed += new EventHandler(ChildForm_Closed);

                        }
                        transactionManageForm.BringToFront();
                        break;

                    case "Menutool_ManagerM":
                        if (managerManageForm == null || managerManageForm.IsDisposed)
                        {
                            managerManageForm = new ManagerManage.ManagerManage();
                            managerManageForm.MdiParent = this;
                            managerManageForm.Dock = DockStyle.Fill;
                            managerManageForm.WindowState = FormWindowState.Maximized;
                            managerManageForm.Show();
                            managerManageForm.Closed += new EventHandler(ChildForm_Closed);

                        }
                        managerManageForm.BringToFront();
                        break;
                    case "Menutool_RightM":
                        if (rightManageForm == null || rightManageForm.IsDisposed)
                        {
                            rightManageForm = new RightManage();
                            rightManageForm.MdiParent = this;
                            rightManageForm.Dock = DockStyle.Fill;
                            rightManageForm.WindowState = FormWindowState.Maximized;
                            rightManageForm.Show();
                            rightManageForm.Closed += new EventHandler(ChildForm_Closed);

                        }
                        rightManageForm.BringToFront();
                        break;
                    case "Menutool_MatchM":
                        if (matchCenterManageFrom == null || matchCenterManageFrom.IsDisposed)
                        {
                            matchCenterManageFrom = new MatchCenterManage.MatchCenterManage();
                            matchCenterManageFrom.MdiParent = this;
                            matchCenterManageFrom.Dock = DockStyle.Fill;
                            matchCenterManageFrom.WindowState = FormWindowState.Maximized;
                            matchCenterManageFrom.Show();
                            matchCenterManageFrom.Closed += new EventHandler(ChildForm_Closed);

                        }
                        matchCenterManageFrom.BringToFront();
                        break;
                    case "Menutool_CounterM":
                        // new CounterManage.CounterManger().ShowDialog();
                        if (counterManger == null || counterManger.IsDisposed)
                        {
                            counterManger = new CounterManger();
                            counterManger.MdiParent = this;
                            counterManger.Dock = DockStyle.Fill;
                            counterManger.WindowState = FormWindowState.Maximized;
                            counterManger.Show();
                            counterManger.Closed += new EventHandler(ChildForm_Closed);

                        }
                        counterManger.BringToFront();
                        break;

                    case "Menutool_BreedClassM": //商品品种管理
                        if (m_BreedClassManagerUI == null || m_BreedClassManagerUI.MdiParent == null)
                        {
                            m_BreedClassManagerUI = new BreedClassManagerUI();
                            m_BreedClassManagerUI.MdiParent = this;
                            m_BreedClassManagerUI.Show();
                            m_BreedClassManagerUI.Closed += new EventHandler(ChildForm_Closed);

                        }
                        m_BreedClassManagerUI.Activate();
                        break;
                    case "Menutool_CommodityM": //商品代码管理
                        if (m_CommodityManagerUI == null || m_CommodityManagerUI.MdiParent == null)
                        {
                            m_CommodityManagerUI = new CommodityManagerUI();
                            m_CommodityManagerUI.MdiParent = this;
                            m_CommodityManagerUI.Show();
                            m_CommodityManagerUI.Closed += new EventHandler(ChildForm_Closed);

                        }
                        m_CommodityManagerUI.Activate();
                        break;
                    case "Menutool_BourseM": //交易所类型管理
                        if (m_BourseManagerUI == null || m_BourseManagerUI.MdiParent == null)
                        {
                            m_BourseManagerUI = new BourseManagerUI();
                            m_BourseManagerUI.MdiParent = this;
                            m_BourseManagerUI.Show();
                            m_BourseManagerUI.Closed += new EventHandler(ChildForm_Closed);

                        }
                        m_BourseManagerUI.Activate();
                        break;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 重写MDI子窗体关闭事件 ChildForm_Closed
        /// <summary>
        /// MDI子窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChildForm_Closed(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() == 1)
            {
                this.pictureEdit1.BringToFront();
            }
        }
        #endregion

        #region 加载指定控件

        /// <summary>
        /// 加载指定控件
        /// </summary>
        /// <param name="_tag">控件标示</param>
        public void LoadUserControl(string _tag)
        {
            this.pictureEdit1.SendToBack();//图片置于底层
            switch (_tag)
            {
                case "MenuCommParaM_BreedClassM": //商品品种管理
                    if (m_BreedClassManagerUI == null || m_BreedClassManagerUI.MdiParent == null)
                    {
                        m_BreedClassManagerUI = new BreedClassManagerUI();
                        m_BreedClassManagerUI.MdiParent = this;
                        m_BreedClassManagerUI.Show();
                        m_BreedClassManagerUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_BreedClassManagerUI.Activate();
                    break;
                case "MenuCommParaM_CommodityM": //商品代码管理
                    if (m_CommodityManagerUI == null || m_CommodityManagerUI.MdiParent == null)
                    {
                        m_CommodityManagerUI = new CommodityManagerUI();
                        m_CommodityManagerUI.MdiParent = this;
                        m_CommodityManagerUI.Show();
                        m_CommodityManagerUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_CommodityManagerUI.Activate();
                    break;
                case "MenuCommParaM_BourseM": //交易所类型管理
                    if (m_BourseManagerUI == null || m_BourseManagerUI.MdiParent == null)
                    {
                        m_BourseManagerUI = new BourseManagerUI();
                        m_BourseManagerUI.MdiParent = this;
                        m_BourseManagerUI.Show();
                        m_BourseManagerUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_BourseManagerUI.Activate();
                    break;

                case "MenuSpotM_MinVolumeOfBusM": //最小交易单位管理
                    if (m_MinVolumeOfBusinessManageUI == null || m_MinVolumeOfBusinessManageUI.MdiParent == null)
                    {
                        m_MinVolumeOfBusinessManageUI = new MinVolumeOfBusinessManageUI();
                        m_MinVolumeOfBusinessManageUI.MdiParent = this;
                        m_MinVolumeOfBusinessManageUI.Show();
                        m_MinVolumeOfBusinessManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_MinVolumeOfBusinessManageUI.Activate();
                    break;
                case "MenuSpotM_SpotCostsM": //现货交易费用管理
                    if (m_SpotCostsManageUI == null || m_SpotCostsManageUI.MdiParent == null)
                    {
                        m_SpotCostsManageUI = new SpotCostsManageUI();
                        m_SpotCostsManageUI.MdiParent = this;
                        m_SpotCostsManageUI.Show();
                        m_SpotCostsManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_SpotCostsManageUI.Activate();
                    break;
                case "MenuSpotM_SpotTradeRulesM": //现货交易规则管理
                    if (m_SpotTradeRulesManageUI == null || m_SpotTradeRulesManageUI.MdiParent == null)
                    {
                        m_SpotTradeRulesManageUI = new SpotTradeRulesManageUI();
                        m_SpotTradeRulesManageUI.MdiParent = this;
                        m_SpotTradeRulesManageUI.Show();
                        m_SpotTradeRulesManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_SpotTradeRulesManageUI.Activate();
                    break;
                case "MenuSpotM_SpotPositionM": //现货持仓限制管理
                    if (m_SpotPositionManageUI == null || m_SpotPositionManageUI.MdiParent == null)
                    {
                        m_SpotPositionManageUI = new SpotPositionManageUI();
                        m_SpotPositionManageUI.MdiParent = this;
                        m_SpotPositionManageUI.Show();
                        m_SpotPositionManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_SpotPositionManageUI.Activate();
                    break;
                case "MenuSpotM_UnitConversionM": //现货单位换算管理
                    if (m_SpotUnitConversionManageUI == null || m_SpotUnitConversionManageUI.MdiParent == null)
                    {
                        m_SpotUnitConversionManageUI = new SpotUnitConversionManageUI();
                        m_SpotUnitConversionManageUI.MdiParent = this;
                        m_SpotUnitConversionManageUI.Show();
                        m_SpotUnitConversionManageUI.Closed += new EventHandler(ChildForm_Closed);

                    }
                    m_SpotUnitConversionManageUI.Activate();
                    break;
                //港股
                case "MenuHKM_HKCommodityM": //港股代码管理
                    if (m_HKCommodityManagerUI == null || m_HKCommodityManagerUI.MdiParent == null)
                    {
                        m_HKCommodityManagerUI = new HKCommodityManagerUI();
                        m_HKCommodityManagerUI.MdiParent = this;
                        m_HKCommodityManagerUI.Show();
                        m_HKCommodityManagerUI.Closed += new EventHandler(ChildForm_Closed);

                    }
                    m_HKCommodityManagerUI.Activate();
                    break;

                case "MenuHKM_HKTradeRulesM": //港股交易规则管理
                    if (m_HKTradeRulesManageUI == null || m_HKTradeRulesManageUI.MdiParent == null)
                    {
                        m_HKTradeRulesManageUI = new HKTradeRulesManageUI();
                        m_HKTradeRulesManageUI.MdiParent = this;
                        m_HKTradeRulesManageUI.Show();
                        m_HKTradeRulesManageUI.Closed += new EventHandler(ChildForm_Closed);

                    }
                    m_HKTradeRulesManageUI.Activate();
                    break;
                case "MenuHKM_HKCostsM": //港股交易费用管理
                    if (m_HKCostsManageUI == null || m_HKCostsManageUI.MdiParent == null)
                    {
                        m_HKCostsManageUI = new HKCostsManageUI();
                        m_HKCostsManageUI.MdiParent = this;
                        m_HKCostsManageUI.Show();
                        m_HKCostsManageUI.Closed += new EventHandler(ChildForm_Closed);

                    }
                    m_HKCostsManageUI.Activate();
                    break;
                case "MenuHKM_MinVolumeOfBusM": //最小交易单位管理(共用)
                    if (m_MinVolumeOfBusinessManageUI == null || m_MinVolumeOfBusinessManageUI.MdiParent == null)
                    {
                        m_MinVolumeOfBusinessManageUI = new MinVolumeOfBusinessManageUI();
                        m_MinVolumeOfBusinessManageUI.MdiParent = this;
                        m_MinVolumeOfBusinessManageUI.Show();
                        m_MinVolumeOfBusinessManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_MinVolumeOfBusinessManageUI.Activate();
                    break;
                case "MenuHKM_SpotPositionM": //现货持仓限制管理（共用）
                    if (m_SpotPositionManageUI == null || m_SpotPositionManageUI.MdiParent == null)
                    {
                        m_SpotPositionManageUI = new SpotPositionManageUI();
                        m_SpotPositionManageUI.MdiParent = this;
                        m_SpotPositionManageUI.Show();
                        m_SpotPositionManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_SpotPositionManageUI.Activate();
                    break;
                //期货
                case "MenuFuturesM_PositionLimitV": //(商品)期货_持仓限制管理UI
                    if (m_PositionLimitValueManageUI == null || m_PositionLimitValueManageUI.MdiParent == null)
                    {
                        m_PositionLimitValueManageUI = new PositionLimitValueManageUI();
                        m_PositionLimitValueManageUI.MdiParent = this;
                        m_PositionLimitValueManageUI.Show();
                        m_PositionLimitValueManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_PositionLimitValueManageUI.Activate();
                    break;
                case "MenuFuturesM_CFBailScaleValue": //商品期货_保证金比例管理UI
                    if (m_CFBailScaleValueManageUI == null || m_CFBailScaleValueManageUI.MdiParent == null)
                    {
                        m_CFBailScaleValueManageUI = new CFBailScaleValueManageUI();
                        m_CFBailScaleValueManageUI.MdiParent = this;
                        m_CFBailScaleValueManageUI.Show();
                        m_CFBailScaleValueManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_CFBailScaleValueManageUI.Activate();
                    break;
                case "MenuFuturesM_FutureCostsResultM": //品种_期货_交易费用管理UI
                    if (m_FutureCostsManageUI == null || m_FutureCostsManageUI.MdiParent == null)
                    {
                        m_FutureCostsManageUI = new FutureCostsManageUI();
                        m_FutureCostsManageUI.MdiParent = this;
                        m_FutureCostsManageUI.Show();
                        m_FutureCostsManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_FutureCostsManageUI.Activate();
                    break;

                case "MenuFuturesM_CommodityFuseM": // 熔断管理UI(股指期货)
                    if (m_CommodityFuseManageUI == null || m_CommodityFuseManageUI.MdiParent == null)
                    {
                        m_CommodityFuseManageUI = new CommodityFuseManageUI();
                        m_CommodityFuseManageUI.MdiParent = this;
                        m_CommodityFuseManageUI.Show();
                        m_CommodityFuseManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_CommodityFuseManageUI.Activate();
                    break;
                case "MenuFuturesM_SIFPositionAndBailM": // 股指期货持仓限制和保证金管理UI
                    if (m_SIFPositionAndBailManageUI == null || m_SIFPositionAndBailManageUI.MdiParent == null)
                    {
                        m_SIFPositionAndBailManageUI = new SIFPositionAndBailManageUI();
                        m_SIFPositionAndBailManageUI.MdiParent = this;
                        m_SIFPositionAndBailManageUI.Show();
                        m_SIFPositionAndBailManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_SIFPositionAndBailManageUI.Activate();
                    break;
                case "MenuFuturesM_FuturesTradeRulesM": // 期货交易规则管理UI
                    if (m_FuturesTradeRulesManageUI == null || m_FuturesTradeRulesManageUI.MdiParent == null)
                    {
                        m_FuturesTradeRulesManageUI = new FuturesTradeRulesManageUI();
                        m_FuturesTradeRulesManageUI.MdiParent = this;
                        m_FuturesTradeRulesManageUI.Show();
                        m_FuturesTradeRulesManageUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    m_FuturesTradeRulesManageUI.Activate();
                    break;
                case "MenuFuturesM_TodaySettlementPriceM":
                    //结算价管理
                    if (m_TodaySettlementPriceManagerUI == null || m_TodaySettlementPriceManagerUI.MdiParent == null)
                    {
                        m_TodaySettlementPriceManagerUI = new TodaySettlementPriceManagerUI();
                        m_TodaySettlementPriceManagerUI.MdiParent = this;
                        m_TodaySettlementPriceManagerUI.Show();
                        m_TodaySettlementPriceManagerUI.Closed += new EventHandler(ChildForm_Closed);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 设置菜单权限
        /// <summary>
        /// 设置菜单权限
        /// </summary>
        public void SetRight()
        {
            try
            {
                //为 兼顾招商订制版的功能，又不影响V3。0的版本功能，所以把判断超级管理员代码注释掉
                //if (CommonClass.ParameterSetting.Mananger.RoleID == (int)ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Admin)
                    //return;
                ManagementCenter.BLL.UM_ManagerGroupFunctionsBLL ManagerGroupFunctionsBLL = new UM_ManagerGroupFunctionsBLL();
                List<UM_ManagerGroupFunctions> lr =
                ManagerGroupFunctionsBLL.GetRightListByManagerID(CommonClass.ParameterSetting.Mananger.UserID);
                if (lr == null || lr.Count < 1)
                {
                    //CommonClass.ShowMessageBox.ShowInformation("获取权限列表失败！");
                    return;
                }
                this.AccountM_ManagerM.Enabled = this.AccountM_RightM.Enabled = this.AccountM_TransactionM.Enabled =
                                                                                this.CounterM_ConfigM.Enabled =
                                                                                this.MatchM_CenterM.Enabled =
                                                                                this.MatchM_GuideM.Enabled =
                                                                                this.MenuCommParaM_BreedClassM.Enabled =
                                                                                this.MenuCommParaM_CommodityM.Enabled =
                                                                                this.MenuCommParaM_BourseM.Enabled =
                                                                                MenuFuturesM_TodaySettlementPriceM.Enabled = true;

                foreach (DevExpress.XtraBars.LinkPersistInfo item in this.MenuSpotM.LinksPersistInfo)
                {
                    item.Item.Enabled = true;
                }
                foreach (DevExpress.XtraBars.LinkPersistInfo item in this.MenuFuturesM.LinksPersistInfo)
                {
                    item.Item.Enabled = true;
                }
                foreach (DevExpress.XtraBars.LinkPersistInfo item in this.MenuHKM.LinksPersistInfo)
                {
                    item.Item.Enabled = true;
                }
                foreach (UM_ManagerGroupFunctions functions in lr)
                {
                    switch ((int)functions.FunctionID)
                    {
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.AccountM_ManagerM:
                            this.AccountM_ManagerM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.AccountM_RightM:
                            this.AccountM_RightM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.AccountM_TransactionM:
                            this.AccountM_TransactionM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.CounterM_ConfigM:
                            this.CounterM_ConfigM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MatchM_CenterM:
                            this.MatchM_CenterM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MatchM_GuideM:
                            this.MatchM_GuideM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MenuCommParaM_BreedClassM:
                            this.MenuCommParaM_BreedClassM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MenuCommParaM_CommodityM:
                            this.MenuCommParaM_CommodityM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MenuCommParaM_BourseM:
                            this.MenuCommParaM_BourseM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MenuFuturesM_TodaySettlementPriceM:
                            this.MenuFuturesM_TodaySettlementPriceM.Enabled = true;
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MenuSpotM:
                            this.MenuSpotM.Enabled = true;
                            foreach (DevExpress.XtraBars.LinkPersistInfo item in this.MenuSpotM.LinksPersistInfo)
                            {
                                item.Item.Enabled = true;
                            }
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MenuFuturesM:
                            this.MenuFuturesM.Enabled = true;
                            foreach (DevExpress.XtraBars.LinkPersistInfo item in this.MenuFuturesM.LinksPersistInfo)
                            {
                                item.Item.Enabled = true;
                                //this.MenuFuturesM_PositionLimitV.Enabled = false;
                                //this.MenuFuturesM_CFBailScaleValue.Enabled = false;
                            }
                            break;
                        case (int)ManagementCenter.Model.CommonClass.Types.MenuTypeEnum.MenuHKM:
                            this.MenuHKM.Enabled = true;
                            foreach (DevExpress.XtraBars.LinkPersistInfo item in this.MenuHKM.LinksPersistInfo)
                            {
                                item.Item.Enabled = true;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
        //================================  事件 ================================

        #region 主页面加载事件
        /// <summary>
        /// 主页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetRight();
            //设置状态栏
            this.barStaticItemLoginName.Caption = "当前用户:" + CommonClass.ParameterSetting.Mananger.LoginName;
            this.barStaticItemSyTime.Caption = "系统当前时间:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.timerDisplayStime.Interval = 1000;
            this.timerDisplayStime.Start();

        }
        #endregion

        #region 加载菜单事件 Item_ItemClick

        /// <summary>
        /// 加载菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadUserControl(e.Item.Name);
            }
            catch
            {
            }
        }

        #endregion

        #region 退出菜单项事件 AccountM_Exits_ItemClick
        /// <summary>
        /// 退出菜单项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccountM_Exits_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region 用来刷新系统时间的时钟控件
        /// <summary>
        /// 用来刷新系统时间的时钟控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDisplayStime_Tick(object sender, EventArgs e)
        {
            this.barStaticItemSyTime.Caption = "系统当前时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }
        #endregion

        #region 调用帮助文件事件 MenuHelpM_Help_ItemClick
        /// <summary>
        /// 调用帮助文件事件 MenuHelpM_Help_ItemClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuHelpM_Help_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string helpFileName = "ManagementCenterConsoleHelp.chm"; //帮助文件名
                System.Diagnostics.Process process = new Process();
                process.StartInfo.FileName = "ManagementCenterConsoleHelp.chm";
                process.StartInfo.Arguments = Application.StartupPath + "/" + helpFileName;
                if (helpFileName != null)
                {
                    process.Start();
                }
                else
                {
                    ShowMessageBox.ShowInformation("帮助文件还没有挂接!");
                }
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }
        #endregion

    }
}