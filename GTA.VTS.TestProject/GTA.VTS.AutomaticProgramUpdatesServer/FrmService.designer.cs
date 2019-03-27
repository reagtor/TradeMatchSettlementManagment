namespace GTA.VTS.AutomaticProgramUpdatesServer
{
    partial class FrmServer
    {
        /// <summary>
        ///  
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。

        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmServer));
            this.menuStripService = new System.Windows.Forms.MenuStrip();
            this.tSService = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemUpdateService = new System.Windows.Forms.ToolStripMenuItem();
            this.lstBoxService = new System.Windows.Forms.ListBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStripService.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripService
            // 
            this.menuStripService.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSService});
            this.menuStripService.Location = new System.Drawing.Point(0, 0);
            this.menuStripService.Name = "menuStripService";
            this.menuStripService.Size = new System.Drawing.Size(392, 24);
            this.menuStripService.TabIndex = 0;
            this.menuStripService.Text = "menuStrip1";
            // 
            // tSService
            // 
            this.tSService.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemUpdateService});
            this.tSService.Name = "tSService";
            this.tSService.Size = new System.Drawing.Size(59, 20);
            this.tSService.Text = "&Service";
            // 
            // menuItemUpdateService
            // 
            this.menuItemUpdateService.Name = "menuItemUpdateService";
            this.menuItemUpdateService.Size = new System.Drawing.Size(190, 22);
            this.menuItemUpdateService.Text = "Start Update Service";
            this.menuItemUpdateService.Click += new System.EventHandler(this.menuItemUpdateService_Click);
            // 
            // lstBoxService
            // 
            this.lstBoxService.FormattingEnabled = true;
            this.lstBoxService.ItemHeight = 14;
            this.lstBoxService.Location = new System.Drawing.Point(0, 27);
            this.lstBoxService.Name = "lstBoxService";
            this.lstBoxService.Size = new System.Drawing.Size(392, 172);
            this.lstBoxService.TabIndex = 1;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "双击显示自动更新服务端窗体";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // FrmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(392, 194);
            this.Controls.Add(this.lstBoxService);
            this.Controls.Add(this.menuStripService);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripService;
            this.MaximizeBox = false;
            this.Name = "FrmServer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Automatic update server program";
            this.Load += new System.EventHandler(this.FrmServer_Load);
            this.Resize += new System.EventHandler(this.FrmServer_Resize);
            this.menuStripService.ResumeLayout(false);
            this.menuStripService.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripService;
        private System.Windows.Forms.ToolStripMenuItem tSService;
        private System.Windows.Forms.ToolStripMenuItem menuItemUpdateService;
        private System.Windows.Forms.ListBox lstBoxService;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

