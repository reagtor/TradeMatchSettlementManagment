namespace ManagementCenterConsole.UI.MatchCenterManage
{
    partial class LeftNavControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeftNavControl));
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.CenterManage = new DevExpress.XtraNavBar.NavBarItem();
            this.MachineManage = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup1;
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.CenterManage,
            this.MachineManage});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.Size = new System.Drawing.Size(233, 519);
            this.navBarControl1.TabIndex = 0;
            this.navBarControl1.Text = "navBarControl1";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "撮合中心管理";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.CenterManage),
            new DevExpress.XtraNavBar.NavBarItemLink(this.MachineManage)});
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // CenterManage
            // 
            this.CenterManage.Caption = "撮合中心配置";
            this.CenterManage.LargeImage = ((System.Drawing.Image)(resources.GetObject("CenterManage.LargeImage")));
            this.CenterManage.Name = "CenterManage";
            this.CenterManage.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.CenterManage_LinkClicked);
            // 
            // MachineManage
            // 
            this.MachineManage.Caption = "撮合机配置";
            this.MachineManage.LargeImage = ((System.Drawing.Image)(resources.GetObject("MachineManage.LargeImage")));
            this.MachineManage.Name = "MachineManage";
            this.MachineManage.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.MachineManage_LinkClicked);
            // 
            // LeftNavControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navBarControl1);
            this.Name = "LeftNavControl";
            this.Size = new System.Drawing.Size(233, 519);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem CenterManage;
        private DevExpress.XtraNavBar.NavBarItem MachineManage;

    }
}
