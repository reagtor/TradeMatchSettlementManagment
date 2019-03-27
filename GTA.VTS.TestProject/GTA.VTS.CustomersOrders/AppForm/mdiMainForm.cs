using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using GTA.VTS.CustomersOrders.DoAccountManager;
using GTA.VTS.CustomersOrders.DoCommonQuery;
using GTA.VTS.CustomersOrders.HKCommonQuery;
using Timer = System.Timers.Timer;
using GTA.VTS.CustomersOrders.DoDealRptService;
using GTA.VTS.CustomersOrders.DoOrderService;
using GTA.VTS.CustomersOrders.TransactionManageService;
using Amib.Threading;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.BLL;

using System.Text.RegularExpressions;
using TypesOrderPriceType = GTA.VTS.CustomersOrders.DoOrderService.TypesOrderPriceType;
using System.Data.OleDb;
using System.Data;
using System.ComponentModel;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// Desc: 多窗体界面主窗体
    /// Create by: 董鹏
    /// Create Date: 2010-03-01
    /// </summary>
    public partial class mdiMainForm : Form
    {
        #region 变量

        /// <summary>
        /// WCF服务访问对象
        /// </summary>
        internal WCFServices wcfLogic;

        /// <summary>
        /// 子窗体集合
        /// </summary>
        Dictionary<Type, Form> MdiForms;
        ///// <summary>
        ///// 选中的菜单
        ///// </summary>
        //ToolStripMenuItem selectedMenu;
        ///// <summary>
        ///// 选中的工具按钮
        ///// </summary>
        //ToolStripButton selectedTool;

        /// <summary>
        /// 工具按钮对应窗体类型
        /// </summary>
        Dictionary<ToolStripButton, Type> ToolForms;

        /// <summary>
        /// Tabpage对应窗体类型
        /// </summary>
        Dictionary<TabPage, Type> TabForms;

        /// <summary>
        /// 选中按钮颜色
        /// </summary>
        Color focusColor = SystemColors.ButtonShadow;//GrayText;

        /// <summary>
        /// 是否更新程序
        /// </summary>
        public bool UpdataOK = false;

        /// <summary>
        /// 定时检查服务接口连接
        /// </summary>
        // private System.Timers.Timer timer = new System.Timers.Timer();

        /// <summary>
        /// 定时连接服务接口
        /// </summary>
        private System.Timers.Timer InitializeTimer = new System.Timers.Timer();
        /// <summary>
        /// 时时检查服务连接状态
        /// </summary>
        private System.Timers.Timer UpdateTimer = new System.Timers.Timer();
        string title;

        #endregion

        #region 构造函数

        public mdiMainForm()
        {
            //timer.Interval = 900 * 1000;
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //timer.Enabled = true;
            MdiForms = new Dictionary<Type, Form>();
            ToolForms = new Dictionary<ToolStripButton, Type>();
            TabForms = new Dictionary<TabPage, Type>();
            InitializeComponent();
            wcfLogic = WCFServices.Instance;

            UpdateTimer.Interval = 3 * 1000;
            UpdateTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateTimer_Elapsed);
            UpdateTimer.Enabled = true;
        }

        #endregion

        #region 窗体事件
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mdiMainForm_Load(object sender, EventArgs e)
        {
            string DnsSafeHost = wcfLogic.DnsSafeHost;
            LocalhostResourcesFormText();
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Click += new EventHandler(MenuItem_Click);
            }
            foreach (ToolStripButton item in toolOrder.Items)
            {
                item.Click += new EventHandler(ToolBarItem_Click);
            }
            MenuItem_Click(menuOrder, EventArgs.Empty);
            toolQuery.Visible = false;
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.Left = 0;
            this.Top = 0;
            tabControl1.MouseDoubleClick += new MouseEventHandler(tabControl1_MouseDoubleClick);
            MainFormLoad();
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            title = this.Text;
            this.toolStripStatusLabel10.Text = ServerConfig.TraderID;
            string ManagementIP = ServerConfig.ManagementIP;
            string ReckoningIP = ServerConfig.ReckoningIP;
            this.toolStripStatusLabel3.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (ManagementIP.Equals("localhost") || ManagementIP.Equals("127.0.0.1") || ReckoningIP.Equals("localhost") || ReckoningIP.Equals("127.0.0.1"))
            {
                this.toolStripStatusLabel5.Text = ResourceOperate.Instanse.GetResourceByKey("DnsSafeHost");
                this.toolStripStatusLabel12.Text = ResourceOperate.Instanse.GetResourceByKey("DnsSafeHost");
            }
            else
            {
                this.toolStripStatusLabel5.Text = ManagementIP;
                this.toolStripStatusLabel12.Text = ReckoningIP;
            }
            this.toolStripStatusLabel15.Text = ServerConfig.ChannelID;
        }
        #region timer
        /// <summary>
        /// 定时刷新柜台连接状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //bool IsServiceOk = wcfLogic.IsServiceOk;
            //bool ServiceOk = wcfLogic.ServiceOk;
            //if (IsServiceOk == true)
            //{
            //    this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Connected");
            //    this.toolStripStatusLabel1.ForeColor = Color.Green;
            //}
            //else if (IsServiceOk == false && ServiceOk == true)
            //{
            //    UpdateTimer.Enabled = false;
            //    //this.toolStripStatusLabel1.Text = "断开连接";
            //    this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Reconnect") + "...";
            //    this.toolStripStatusLabel1.ForeColor = Color.Red;
            //    bool isSuccess = wcfLogic.Initialize(txtChannelID.Text.Trim());
            //    if (isSuccess)
            //    {
            //        UpdateTimer.Enabled = true;
            //        this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Connected");
            //        this.toolStripStatusLabel1.ForeColor = Color.Green;
            //    }
            //    else
            //    {
            //        this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Failed");
            //        this.toolStripStatusLabel1.ForeColor = Color.Red;
            //        InitializeTimer.Interval = 6 * 1000;
            //        InitializeTimer.Elapsed += new System.Timers.ElapsedEventHandler(InitializeTimer_Elapsed);
            //        InitializeTimer.Enabled = true;
            //    }
            //}
        }
        #endregion timer
        /// <summary>
        /// 定时连接柜台服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Reconnect") + "...";
            this.toolStripStatusLabel1.ForeColor = Color.Red;
            string errorMsg = "";
            bool isSuccess = wcfLogic.Initialize(ServerConfig.TraderID, "", out errorMsg);
            if (isSuccess)
            {
                this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Connected");
                this.toolStripStatusLabel1.ForeColor = Color.Green;
                InitializeTimer.Enabled = false;
                UpdateTimer.Enabled = true;
            }
            else
            {
                this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Failed");
                this.toolStripStatusLabel1.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 三秒钟刷新一次服务连接状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool IsServiceOk = wcfLogic.IsServiceOk;
            bool ServiceOk = wcfLogic.ServiceOk;
            if (IsServiceOk == true)
            {
                this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Connected");
                this.toolStripStatusLabel1.ForeColor = Color.Green;
            }
            else
            {
                //this.toolStripStatusLabel1.Text = "断开连接";
                this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Disconnect");
                this.toolStripStatusLabel1.ForeColor = Color.Red;
                if (IsServiceOk == false && ServiceOk == true)
                {
                    UpdateTimer.Enabled = false;
                    //this.toolStripStatusLabel1.Text = "断开连接";
                    this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Reconnect") + "...";
                    this.toolStripStatusLabel1.ForeColor = Color.Red;
                    string errorMsg = "";
                    bool isSuccess = wcfLogic.Initialize(ServerConfig.TraderID, "", out errorMsg);
                    if (isSuccess)
                    {
                        UpdateTimer.Enabled = true;
                        this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Connected");
                        this.toolStripStatusLabel1.ForeColor = Color.Green;
                    }
                    else
                    {
                        this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Failed");
                        this.toolStripStatusLabel1.ForeColor = Color.Red;
                        InitializeTimer.Interval = 6 * 1000;
                        InitializeTimer.Elapsed += new System.Timers.ElapsedEventHandler(InitializeTimer_Elapsed);
                        InitializeTimer.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// 双击Tab控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                MdiForms[TabForms[tabControl1.SelectedTab]].Close();
            }
        }

        /// <summary>
        /// 菜单项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuItem_Click(object sender, EventArgs e)
        {
            //ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            //if (selectedMenu != null)
            //{
            //    selectedMenu.BackColor = System.Drawing.SystemColors.Control;
            //}
            //if (selectedMenu != menu)
            //{
            //    menu.BackColor = focusColor;
            //    selectedMenu = menu;
            //    HideMdiForms();
            //}
            //if (sender.Equals(menuOrder))
            //{
            //    if (toolOrder.Items.Contains(selectedTool))
            //    {
            //        OpenMdiForm(ToolForms[selectedTool]);
            //    }
            //}
            //if (sender.Equals(menuQuery))
            //{
            //    if (toolQuery.Items.Contains(selectedTool))
            //    {
            //        OpenMdiForm(ToolForms[selectedTool]);
            //    }
            //}
        }

        /// <summary>
        /// 工具按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolBarItem_Click(object sender, EventArgs e)
        {
            //ToolStripButton button = (ToolStripButton)sender;
            //if (selectedTool != null)
            //{
            //    selectedTool.BackColor = System.Drawing.SystemColors.Control;
            //}
            //if (selectedTool != button)
            //{
            //    button.BackColor = focusColor;
            //    selectedTool = button;
            //}
        }

        /// <summary>
        /// 子窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MdiForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Type type = sender.GetType();
            if (MdiForms.ContainsKey(type))
            {
                MdiForms.Remove(type);
            }

            if (tabControl1.SelectedTab != null)
            {
                TabForms.Remove(tabControl1.SelectedTab);
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            if (MdiForms.Count == 0)
            {
                tabControl1.Visible = false;
                this.Text = title;
            }
        }

        /// <summary>
        /// 子窗体激活事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mdiMainForm_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                return;
            }
            this.ActiveMdiChild.Text = "";
            this.Icon = this.ActiveMdiChild.Icon;
            //if (this.ActiveMdiChild.WindowState != FormWindowState.Maximized)
            //{
            //    this.ActiveMdiChild.WindowState = FormWindowState.Maximized;
            //}
            return;
            #region old
            Form frm = this.ActiveMdiChild;
            frm.Visible = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Bounds = new Rectangle(new Point(0, 0), new Size(this.ClientRectangle.Width, this.ClientRectangle.Height));// new Rectangle(new Point(0, 0), this.ViewInfo.PageClientBounds.Size);
            frm.WindowState = FormWindowState.Maximized;
            frm.Visible = true;
            return;

            //this.ActiveMdiChild.MaximizeBox = false;
            //this.ActiveMdiChild.MinimizeBox = false;
            //this.ActiveMdiChild.Dock = DockStyle.Fill;
            this.ActiveMdiChild.WindowState = FormWindowState.Maximized;

            Form activeMdiChild = this.ActiveMdiChild;
            if (activeMdiChild != null)
            {
                this.PatchMaximized(activeMdiChild);
                if (activeMdiChild.Dock != DockStyle.Fill)
                {
                    Size size = this.ClientSize;//.ViewInfo.PageClientBounds.Size;

                    Point location = new Point(0, 0);
                    Rectangle rect = new Rectangle(location, size);
                    if (activeMdiChild.Bounds.IntersectsWith(rect))
                    {
                        activeMdiChild.Bounds = rect;
                    }
                    else
                    {
                        Point point2 = new Point(-size.Width, -size.Height);
                        Rectangle rectangle2 = new Rectangle(point2, size);
                        activeMdiChild.Bounds = rectangle2;
                    }
                    activeMdiChild.Dock = DockStyle.Fill;
                }
            }
            //foreach (XtraMdiTabPage page in this.Pages)

            foreach (Form mdiChild in this.MdiForms.Values)
            {
                //Form mdiChild = page.MdiChild;
                if ((mdiChild != null) && !object.ReferenceEquals(mdiChild, activeMdiChild))
                {
                    if (mdiChild.Dock == DockStyle.Fill)
                    {
                        mdiChild.SuspendLayout();
                        try
                        {
                            Rectangle bounds = mdiChild.Bounds;
                            mdiChild.Dock = DockStyle.None;
                            mdiChild.Bounds = bounds;
                        }
                        finally
                        {
                            mdiChild.ResumeLayout();
                        }
                    }
                    this.PatchMaximized(mdiChild);
                }
            }
            #endregion
        }

        /// <summary>
        /// 切换选中TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                OpenMdiForm(TabForms[tabControl1.SelectedTab]);
            }
        }

        /// <summary>
        /// 点击下单菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOrder_Click(object sender, EventArgs e)
        {
            //toolOrder.Visible = true;
            //toolQuery.Visible = false;
        }

        /// <summary>
        /// 点击查询菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuQuery_Click(object sender, EventArgs e)
        {
            //toolOrder.Visible = false;
            //toolQuery.Visible = true;
        }

        ///// <summary>
        ///// 点击连接菜单
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void menuConnection_Click(object sender, EventArgs e)
        //{
        //    OpenMdiForm(typeof(frmConnection));
        //}

        /// <summary>
        /// 点击重新初始化数据菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReInitData_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmReinitCaptial));
        }

        /// <summary>
        /// 现货下单按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton1, typeof(frmXHOrder));
            OpenMdiForm(typeof(frmXHOrder));
        }

        /// <summary>
        /// 港股下单按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton2, typeof(frmHKOrder));
            OpenMdiForm(typeof(frmHKOrder));
        }

        /// <summary>
        /// 股指期货下单按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton3, typeof(frmSIOrder));
            OpenMdiForm(typeof(frmSIOrder));
        }

        /// <summary>
        /// 商品期货下单按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton4, typeof(frmCFOrder));
            OpenMdiForm(typeof(frmCFOrder));
        }

        /// <summary>
        /// 现货下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuXHOrder_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton1, typeof(frmXHOrder));
            OpenMdiForm(typeof(frmXHOrder));
        }

        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHKOrder_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton2, typeof(frmHKOrder));
            OpenMdiForm(typeof(frmHKOrder));
        }

        /// <summary>
        /// 股指期货下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSIOrder_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton3, typeof(frmSIOrder));
            OpenMdiForm(typeof(frmSIOrder));
        }

        /// <summary>
        /// 商品期货下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCFOrder_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton4, typeof(frmCFOrder));
            OpenMdiForm(typeof(frmCFOrder));
        }

        /// <summary>
        /// 现货查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuXHQuery_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton1, typeof(frmXHQuery));
            OpenMdiForm(typeof(frmXHQuery));
        }

        /// <summary>
        /// 港股查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHKQuery_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton1, typeof(frmHKQuery));
            OpenMdiForm(typeof(frmHKQuery));
        }

        /// <summary>
        /// 股指期货查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSIQuery_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton1, typeof(frmSIQuery));
            OpenMdiForm(typeof(frmSIQuery));
        }

        /// <summary>
        /// 商品期货查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCFQuery_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton1, typeof(frmCFQuery));
            OpenMdiForm(typeof(frmCFQuery));
        }

        /// <summary>
        /// 资金流水查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFlowQuery_Click(object sender, EventArgs e)
        {
            SetToolFormRelation(toolStripButton1, typeof(frmFlowQuery));
            OpenMdiForm(typeof(frmFlowQuery));
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mdiMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!UpdataOK)
            {
                string Exit = ResourceOperate.Instanse.GetResourceByKey("Exit");
                string Prompted = ResourceOperate.Instanse.GetResourceByKey("Prompted");
                DialogResult result = MessageBox.Show(Exit, Prompted, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    //先隐藏所有字窗体，不然出现关闭时闪动
                    HideMdiForms();
                    // return;
                    // timer.Enabled = false;
                    InitializeTimer.Enabled = false;
                    UpdateTimer.Enabled = false;
                    Application.ExitThread();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {

                //timer.Enabled = false;
                InitializeTimer.Enabled = false;
                UpdateTimer.Enabled = false;
                //先隐藏所有字窗体，不然出现关闭时闪动
                HideMdiForms();
                Application.ExitThread();
            }
        }

        /// <summary>
        /// 点击连接菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuConnection_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmConnection));
        }

        /// <summary>
        /// 点击更新程序菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, EventArgs e)
        {
            UpdataOK = true;
            try
            {
                string ExitUpdate = ResourceOperate.Instanse.GetResourceByKey("ExitUpdate");
                string Prompted = ResourceOperate.Instanse.GetResourceByKey("Prompted");
                DialogResult result = MessageBox.Show(ExitUpdate, Prompted, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    UpdataOK = false;
                    return;
                }
                string helpFileName = "GTA.VTS.UpdateClient.exe";
                System.Diagnostics.Process process = new Process();
                process.StartInfo.FileName = "GTA.VTS.UpdateClient.exe";
                process.StartInfo.Arguments = Application.StartupPath + "/" + helpFileName;
                if (helpFileName != null)
                {
                    process.Start();
                }
                //先把所有的字窗体都隐藏，本也可以关闭，但之前在窗体加载之前对了窗体加载了关闭事件所以这样程序还没有退出完毕更新程序已经开始了
                //否则也可以把字窗体之前加载的关闭事件一一移除即可
                HideMdiForms();
                Application.Exit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                UpdataOK = false;
                return;
            }
        }

        /// <summary>
        /// 点击退出菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExit_Click(object sender, EventArgs e)
        {
            string Exit = ResourceOperate.Instanse.GetResourceByKey("Exit");
            string Prompted = ResourceOperate.Instanse.GetResourceByKey("Prompted");
            DialogResult result = MessageBox.Show(Exit, Prompted, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.ExitThread();
            }
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 打开子窗体
        /// </summary>
        /// <param name="type"></param>
        private void OpenMdiForm(Type type)
        {
            this.Text = title;
            if (!MdiForms.ContainsKey(type))
            {
                Form frm = (Form)System.Reflection.Assembly.GetAssembly(type).CreateInstance(type.ToString());
                frm.FormClosed += new FormClosedEventHandler(MdiForm_FormClosed);
                MdiForms.Add(type, frm);
                frm.MdiParent = this;


                TabPage tp = new TabPage();
                tp.Parent = tabControl1;
                tp.Text = frm.Text;
                tp.Show();
                frm.Show();

                TabForms.Add(tp, type);

                tabControl1.SelectedTab = tp;
            }
            else
            {
                MdiForms[type].Activate();
                foreach (TabPage key in TabForms.Keys)
                {
                    if (TabForms[key] == type)
                    {
                        tabControl1.SelectedTab = key;
                        this.Text = title + " - " + key.Text;
                        break;
                    }
                }
            }
            if (MdiForms.Count != 0)
            {
                tabControl1.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        protected void PatchMaximized(Form form)
        {
            if ((form != null) && (form.WindowState != FormWindowState.Normal))
            {
                form.WindowState = FormWindowState.Normal;
            }
            //form.Dock = DockStyle.None;
        }

        /// <summary>
        /// 窗体尺寸改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mdiMainForm_Resize(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 隐藏所有子窗体
        /// </summary>
        private void HideMdiForms()
        {
            foreach (Form mdiChild in this.MdiForms.Values)
            {
                mdiChild.Hide();
            }
        }

        /// <summary>
        /// 设置工具按钮与窗体类型的关联
        /// </summary>
        /// <param name="tool"></param>
        /// <param name="type"></param>
        private void SetToolFormRelation(ToolStripButton tool, Type type)
        {
            if (!ToolForms.ContainsKey(tool))
            {
                ToolForms.Add(tool, type);
            }
        }

        /// <summary>
        /// 窗体加载执行方法
        /// </summary>
        private void MainFormLoad()
        {
            string LoginName = ServerConfig.TraderID;
            string passWord = ServerConfig.PassWord;
            string message;
            //将TraderId和passWord通过管理中心服务进行验证
            bool result = wcfLogic.AdminConfirmation(LoginName, passWord, out message);

            //如果验证通过则说明登陆的是管理员，下单界面禁用
            if (result)
            {
                menuOrder.Visible = false;
                menuQuery.Visible = false;
                menuSettings.Visible = false;
                toolOrder.Visible = false;
            }
            //验证不通过则是交易员，试玩界面禁用
            else
            {
                menuReInitData.Visible = false;
                ToolStripMenuItemAddCaption.Visible = false;
            }

            wcfLogic.LoadTraderInfo();
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.StatusLable.Text = ResourceOperate.Instanse.GetResourceByKey("Connection");
            this.toolStripStatusLabel1.Text = ResourceOperate.Instanse.GetResourceByKey("Connected");
            this.toolStripStatusLabel9.Text = ResourceOperate.Instanse.GetResourceByKey("Current");
            this.Text = ResourceOperate.Instanse.GetResourceByKey("UserFront");
            this.toolStripStatusLabel4.Text = ResourceOperate.Instanse.GetResourceByKey("ManagementAddress");
            this.toolStripStatusLabel11.Text = ResourceOperate.Instanse.GetResourceByKey("ReckoningAddress");
            this.toolStripStatusLabel14.Text = ResourceOperate.Instanse.GetResourceByKey("Channelnumber");
            // string x = menuStrip1.Items.Count.ToString();
            for (int i = 0; i < menuStrip1.Items.Count; i++) //遍历MenuStrip组件中的一级菜单项
            {
                string StripName = menuStrip1.Items[i].Name;
                // this.menuStrip1.Items[i].Text = x;
                this.menuStrip1.Items[i].Text = ResourceOperate.Instanse.GetResourceByKey(StripName);
                //将一级菜单中数据添加到ToolStripDropDownItem中
                ToolStripDropDownItem newmenu = (ToolStripDropDownItem)menuStrip1.Items[i];
                //循环二级菜单项
                for (int j = 0; j < newmenu.DropDownItems.Count; j++) //遍历二级菜单项
                {
                    string ToolName = newmenu.DropDownItems[j].Name;
                    newmenu.DropDownItems[j].Text = ResourceOperate.Instanse.GetResourceByKey(ToolName);
                }
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化WCF连接
        /// </summary>
        /// <param name="traderID">交易员ID</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>是否成功</returns>
        public bool Initialize(string traderID, ref string errMsg)
        {
            errMsg = "";
            try
            {
                bool isSuccess = wcfLogic.Initialize(traderID, "", out errMsg);
                if (!isSuccess)
                {
                    LogHelper.WriteDebug(errMsg);
                    return false;
                }

                string channleid = ServerConfig.ChannelID;
                string title = " [ChannelID: " + channleid + "]";
                StatusLable.Text = "Version:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "    " + StatusLable.Text;
                this.Text += title;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            return true;
        }

        #endregion

        #region 为兼容旧版本，改完后要去掉的
        //internal void ProcessXHBack(StockDealOrderPushBack drsip)
        //{

        //}

        //internal void ProcessSPQHBack(FutureDealOrderPushBack drmip)
        //{

        //}

        //internal void ProcessGZQHBack(FutureDealOrderPushBack drsifi)
        //{

        //}

        //internal void ProcessHKBack(HKDealOrderPushBack drsip)
        //{

        //}

        //internal void ProcessHKModifyOrderBack(HKModifyOrderPushBack mopb)
        //{

        //}

        //internal void WriteWCFMsg(string msg)
        //{

        //}
        #endregion


        /// <summary>
        /// 自由转账
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(ConvertTransfer));
        }

        private void ToolStripMenuItemAddCaption_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmAdditionalCaption));
        }

        private void tsmAnalysis_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmStatisticalAnalysis));
        }
    }
}
