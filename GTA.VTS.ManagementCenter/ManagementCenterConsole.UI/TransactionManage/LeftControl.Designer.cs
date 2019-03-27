namespace ManagementCenterConsole.UI.TransactionManage
{
    partial class LeftControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeftControl));
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.AccountManage = new DevExpress.XtraNavBar.NavBarItem();
            this.AddFundManage = new DevExpress.XtraNavBar.NavBarItem();
            this.FreezeManage = new DevExpress.XtraNavBar.NavBarItem();
            this.TransferManageUI = new DevExpress.XtraNavBar.NavBarItem();
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
            this.AccountManage,
            this.AddFundManage,
            this.FreezeManage,
            this.TransferManageUI});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.Size = new System.Drawing.Size(217, 546);
            this.navBarControl1.TabIndex = 0;
            this.navBarControl1.Text = "navBarControl1";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "交易员管理";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.AccountManage),
            new DevExpress.XtraNavBar.NavBarItemLink(this.AddFundManage),
            new DevExpress.XtraNavBar.NavBarItemLink(this.FreezeManage),
            new DevExpress.XtraNavBar.NavBarItemLink(this.TransferManageUI)});
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // AccountManage
            // 
            this.AccountManage.Caption = "账号管理";
            this.AccountManage.LargeImage = ((System.Drawing.Image)(resources.GetObject("AccountManage.LargeImage")));
            this.AccountManage.Name = "AccountManage";
            this.AccountManage.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.AccountManage_LinkClicked);
            // 
            // AddFundManage
            // 
            this.AddFundManage.Caption = "追加资金";
            this.AddFundManage.LargeImage = ((System.Drawing.Image)(resources.GetObject("AddFundManage.LargeImage")));
            this.AddFundManage.Name = "AddFundManage";
            this.AddFundManage.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.AddFundManage_LinkClicked);
            // 
            // FreezeManage
            // 
            this.FreezeManage.Caption = "账号冻结/解冻";
            this.FreezeManage.LargeImage = ((System.Drawing.Image)(resources.GetObject("FreezeManage.LargeImage")));
            this.FreezeManage.Name = "FreezeManage";
            this.FreezeManage.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.FreezeManage_LinkClicked);
            // 
            // TransferManageUI
            // 
            this.TransferManageUI.Caption = "转账";
            this.TransferManageUI.LargeImage = ((System.Drawing.Image)(resources.GetObject("TransferManageUI.LargeImage")));
            this.TransferManageUI.Name = "TransferManageUI";
            this.TransferManageUI.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.TransferManage_LinkClicked);
            // 
            // LeftControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navBarControl1);
            this.Name = "LeftControl";
            this.Size = new System.Drawing.Size(217, 546);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem AccountManage;
        private DevExpress.XtraNavBar.NavBarItem AddFundManage;
        private DevExpress.XtraNavBar.NavBarItem FreezeManage;
        private DevExpress.XtraNavBar.NavBarItem TransferManageUI;
    }
}
