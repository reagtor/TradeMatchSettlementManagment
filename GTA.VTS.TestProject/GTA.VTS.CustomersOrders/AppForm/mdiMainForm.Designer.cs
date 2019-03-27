namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class mdiMainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mdiMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXHOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHKOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGZQHOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSPQHOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXHQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHKQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGZQHQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSPQHQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFlowQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Update = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReInitData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddCaption = new System.Windows.Forms.ToolStripMenuItem();
            this.toolOrder = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolQuery = new System.Windows.Forms.ToolStrip();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLable = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel13 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel12 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel16 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel14 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel15 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsmAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolOrder.SuspendLayout();
            this.toolQuery.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOrder,
            this.menuQuery,
            this.menuSettings,
            this.menuReInitData,
            this.ToolStripMenuItemAddCaption});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1108, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuOrder
            // 
            this.menuOrder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuXHOrder,
            this.menuHKOrder,
            this.menuGZQHOrder,
            this.menuSPQHOrder});
            this.menuOrder.Name = "menuOrder";
            this.menuOrder.Size = new System.Drawing.Size(41, 20);
            this.menuOrder.Text = "下单";
            this.menuOrder.Click += new System.EventHandler(this.menuOrder_Click);
            // 
            // menuXHOrder
            // 
            this.menuXHOrder.Name = "menuXHOrder";
            this.menuXHOrder.Size = new System.Drawing.Size(142, 22);
            this.menuXHOrder.Text = "现货下单";
            this.menuXHOrder.Click += new System.EventHandler(this.menuXHOrder_Click);
            // 
            // menuHKOrder
            // 
            this.menuHKOrder.Name = "menuHKOrder";
            this.menuHKOrder.Size = new System.Drawing.Size(142, 22);
            this.menuHKOrder.Text = "港股下单";
            this.menuHKOrder.Click += new System.EventHandler(this.menuHKOrder_Click);
            // 
            // menuGZQHOrder
            // 
            this.menuGZQHOrder.Name = "menuGZQHOrder";
            this.menuGZQHOrder.Size = new System.Drawing.Size(142, 22);
            this.menuGZQHOrder.Text = "股指期货下单";
            this.menuGZQHOrder.Click += new System.EventHandler(this.menuSIOrder_Click);
            // 
            // menuSPQHOrder
            // 
            this.menuSPQHOrder.Name = "menuSPQHOrder";
            this.menuSPQHOrder.Size = new System.Drawing.Size(142, 22);
            this.menuSPQHOrder.Text = "商品期货下单";
            this.menuSPQHOrder.Click += new System.EventHandler(this.menuCFOrder_Click);
            // 
            // menuQuery
            // 
            this.menuQuery.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuXHQuery,
            this.menuHKQuery,
            this.menuGZQHQuery,
            this.menuSPQHQuery,
            this.menuFlowQuery});
            this.menuQuery.Name = "menuQuery";
            this.menuQuery.Size = new System.Drawing.Size(41, 20);
            this.menuQuery.Text = "查询";
            this.menuQuery.Click += new System.EventHandler(this.menuQuery_Click);
            // 
            // menuXHQuery
            // 
            this.menuXHQuery.Name = "menuXHQuery";
            this.menuXHQuery.Size = new System.Drawing.Size(142, 22);
            this.menuXHQuery.Text = "现货查询";
            this.menuXHQuery.Click += new System.EventHandler(this.menuXHQuery_Click);
            // 
            // menuHKQuery
            // 
            this.menuHKQuery.Name = "menuHKQuery";
            this.menuHKQuery.Size = new System.Drawing.Size(142, 22);
            this.menuHKQuery.Text = "港股查询";
            this.menuHKQuery.Click += new System.EventHandler(this.menuHKQuery_Click);
            // 
            // menuGZQHQuery
            // 
            this.menuGZQHQuery.Name = "menuGZQHQuery";
            this.menuGZQHQuery.Size = new System.Drawing.Size(142, 22);
            this.menuGZQHQuery.Text = "股指期货查询";
            this.menuGZQHQuery.Click += new System.EventHandler(this.menuSIQuery_Click);
            // 
            // menuSPQHQuery
            // 
            this.menuSPQHQuery.Name = "menuSPQHQuery";
            this.menuSPQHQuery.Size = new System.Drawing.Size(142, 22);
            this.menuSPQHQuery.Text = "商品期货查询";
            this.menuSPQHQuery.Click += new System.EventHandler(this.menuCFQuery_Click);
            // 
            // menuFlowQuery
            // 
            this.menuFlowQuery.Name = "menuFlowQuery";
            this.menuFlowQuery.Size = new System.Drawing.Size(142, 22);
            this.menuFlowQuery.Text = "资金流水查询";
            this.menuFlowQuery.Click += new System.EventHandler(this.menuFlowQuery_Click);
            // 
            // menuSettings
            // 
            this.menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuConnection,
            this.ToolStripMenuItem,
            this.tsmAnalysis,
            this.Update,
            this.menuExit});
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(41, 20);
            this.menuSettings.Text = "设置";
            // 
            // menuConnection
            // 
            this.menuConnection.Name = "menuConnection";
            this.menuConnection.Size = new System.Drawing.Size(152, 22);
            this.menuConnection.Text = "柜台连接";
            this.menuConnection.Click += new System.EventHandler(this.menuConnection_Click);
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem.Text = "自由转账";
            this.ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // Update
            // 
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(152, 22);
            this.Update.Text = "更新程序";
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(152, 22);
            this.menuExit.Text = "退出";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuReInitData
            // 
            this.menuReInitData.Name = "menuReInitData";
            this.menuReInitData.Size = new System.Drawing.Size(101, 20);
            this.menuReInitData.Text = "数据重新初始化";
            this.menuReInitData.Click += new System.EventHandler(this.menuReInitData_Click);
            // 
            // ToolStripMenuItemAddCaption
            // 
            this.ToolStripMenuItemAddCaption.Name = "ToolStripMenuItemAddCaption";
            this.ToolStripMenuItemAddCaption.Size = new System.Drawing.Size(65, 20);
            this.ToolStripMenuItemAddCaption.Text = "添加资金";
            this.ToolStripMenuItemAddCaption.Click += new System.EventHandler(this.ToolStripMenuItemAddCaption_Click);
            // 
            // toolOrder
            // 
            this.toolOrder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolOrder.Location = new System.Drawing.Point(0, 24);
            this.toolOrder.Name = "toolOrder";
            this.toolOrder.Size = new System.Drawing.Size(936, 25);
            this.toolOrder.TabIndex = 1;
            this.toolOrder.Text = "toolStrip1";
            this.toolOrder.Visible = false;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton1.Text = "现货下单";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton2.Text = "港股下单";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton3.Text = "股指期货下单";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton4.Text = "商品期货下单";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolQuery
            // 
            this.toolQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripButton8,
            this.toolStripButton9});
            this.toolQuery.Location = new System.Drawing.Point(0, 24);
            this.toolQuery.Name = "toolQuery";
            this.toolQuery.Size = new System.Drawing.Size(1108, 25);
            this.toolQuery.TabIndex = 2;
            this.toolQuery.Text = "toolStrip2";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton5.Text = "现货查询";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton6.Text = "港股查询";
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton7.Text = "股指期货查询";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton8.Text = "商品期货查询";
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton9.Image")));
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton9.Text = "资金流水查询";
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 49);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1108, 26);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.Visible = false;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel9,
            this.toolStripStatusLabel10,
            this.toolStripStatusLabel8,
            this.StatusLable,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel13,
            this.toolStripStatusLabel11,
            this.toolStripStatusLabel12,
            this.toolStripStatusLabel16,
            this.toolStripStatusLabel14,
            this.toolStripStatusLabel15});
            this.statusStrip1.Location = new System.Drawing.Point(0, 551);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1108, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(53, 17);
            this.toolStripStatusLabel2.Text = "Version:";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(47, 17);
            this.toolStripStatusLabel3.Text = "1.0.0.0";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel6.Text = "  ";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(95, 17);
            this.toolStripStatusLabel9.Text = "当前登陆交易员:";
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel10.Text = "9";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel8.Text = "  ";
            // 
            // StatusLable
            // 
            this.StatusLable.Name = "StatusLable";
            this.StatusLable.Size = new System.Drawing.Size(83, 17);
            this.StatusLable.Text = "柜台连接状态:";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(41, 17);
            this.toolStripStatusLabel1.Text = "已连接";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel7.Text = "  ";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(119, 17);
            this.toolStripStatusLabel4.Text = "管理中心服务器地址:";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel5.Text = "127.0.0.1";
            // 
            // toolStripStatusLabel13
            // 
            this.toolStripStatusLabel13.Name = "toolStripStatusLabel13";
            this.toolStripStatusLabel13.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel13.Text = "  ";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(107, 17);
            this.toolStripStatusLabel11.Text = "柜台服务连接地址:";
            // 
            // toolStripStatusLabel12
            // 
            this.toolStripStatusLabel12.ActiveLinkColor = System.Drawing.Color.Blue;
            this.toolStripStatusLabel12.ForeColor = System.Drawing.Color.Blue;
            this.toolStripStatusLabel12.Name = "toolStripStatusLabel12";
            this.toolStripStatusLabel12.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel12.Text = "127.0.0.1";
            // 
            // toolStripStatusLabel16
            // 
            this.toolStripStatusLabel16.Name = "toolStripStatusLabel16";
            this.toolStripStatusLabel16.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel16.Text = "  ";
            // 
            // toolStripStatusLabel14
            // 
            this.toolStripStatusLabel14.Name = "toolStripStatusLabel14";
            this.toolStripStatusLabel14.Size = new System.Drawing.Size(47, 17);
            this.toolStripStatusLabel14.Text = "通道号:";
            // 
            // toolStripStatusLabel15
            // 
            this.toolStripStatusLabel15.ActiveLinkColor = System.Drawing.Color.BlueViolet;
            this.toolStripStatusLabel15.ForeColor = System.Drawing.Color.BlueViolet;
            this.toolStripStatusLabel15.Name = "toolStripStatusLabel15";
            this.toolStripStatusLabel15.Size = new System.Drawing.Size(107, 17);
            this.toolStripStatusLabel15.Text = "00:E0:6F:03:B4:42";
            // 
            // tsmAnalysis
            // 
            this.tsmAnalysis.Name = "tsmAnalysis";
            this.tsmAnalysis.Size = new System.Drawing.Size(152, 22);
            this.tsmAnalysis.Text = "交易统计分析";
            this.tsmAnalysis.Click += new System.EventHandler(this.tsmAnalysis_Click);
            // 
            // mdiMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 573);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolQuery);
            this.Controls.Add(this.toolOrder);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(20000, 20000);
            this.Name = "mdiMainForm";
            this.Text = "User Front Order System";
            this.Load += new System.EventHandler(this.mdiMainForm_Load);
            this.MdiChildActivate += new System.EventHandler(this.mdiMainForm_MdiChildActivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mdiMainForm_FormClosing);
            this.Resize += new System.EventHandler(this.mdiMainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolOrder.ResumeLayout(false);
            this.toolOrder.PerformLayout();
            this.toolQuery.ResumeLayout(false);
            this.toolQuery.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuOrder;
        private System.Windows.Forms.ToolStripMenuItem menuQuery;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuReInitData;
        private System.Windows.Forms.ToolStrip toolOrder;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStrip toolQuery;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLable;
        private System.Windows.Forms.ToolStripMenuItem menuXHOrder;
        private System.Windows.Forms.ToolStripMenuItem menuHKOrder;
        private System.Windows.Forms.ToolStripMenuItem menuGZQHOrder;
        private System.Windows.Forms.ToolStripMenuItem menuSPQHOrder;
        private System.Windows.Forms.ToolStripMenuItem menuXHQuery;
        private System.Windows.Forms.ToolStripMenuItem menuHKQuery;
        private System.Windows.Forms.ToolStripMenuItem menuGZQHQuery;
        private System.Windows.Forms.ToolStripMenuItem menuSPQHQuery;
        private System.Windows.Forms.ToolStripMenuItem menuFlowQuery;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripMenuItem menuConnection;
        private System.Windows.Forms.ToolStripMenuItem Update;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddCaption;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel12;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel13;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel14;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel15;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel16;
        private System.Windows.Forms.ToolStripMenuItem tsmAnalysis;
    }
}

