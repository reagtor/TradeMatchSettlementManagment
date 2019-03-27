namespace GTA.VTS.CustomersOrders.AppForm.UpdateAPP
{
    partial class FrmServer
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
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
            this.menuStripService = new System.Windows.Forms.MenuStrip();
            this.tSService = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemUpdateService = new System.Windows.Forms.ToolStripMenuItem();
            this.lstBoxService = new System.Windows.Forms.ListBox();
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
            // FrmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(392, 194);
            this.Controls.Add(this.lstBoxService);
            this.Controls.Add(this.menuStripService);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStripService;
            this.MaximizeBox = false;
            this.Name = "FrmServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App Update Server";
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
    }
}

