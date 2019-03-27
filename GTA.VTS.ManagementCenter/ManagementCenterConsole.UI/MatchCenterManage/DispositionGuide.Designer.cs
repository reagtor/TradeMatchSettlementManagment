namespace ManagementCenterConsole.UI.MatchCenterManage
{
    partial class DispositionGuide
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelUp = new DevExpress.XtraEditors.PanelControl();
            this.labDescrip = new DevExpress.XtraEditors.LabelControl();
            this.panelDown = new DevExpress.XtraEditors.PanelControl();
            this.btn_CancelOrOk = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Next = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Back = new DevExpress.XtraEditors.SimpleButton();
            this.panelMid = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelUp)).BeginInit();
            this.panelUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelDown)).BeginInit();
            this.panelDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelMid)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUp
            // 
            this.panelUp.Controls.Add(this.labDescrip);
            this.panelUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUp.Location = new System.Drawing.Point(0, 0);
            this.panelUp.Name = "panelUp";
            this.panelUp.Size = new System.Drawing.Size(458, 51);
            this.panelUp.TabIndex = 0;
            this.panelUp.Text = "panelControl1";
            // 
            // labDescrip
            // 
            this.labDescrip.Location = new System.Drawing.Point(12, 16);
            this.labDescrip.Name = "labDescrip";
            this.labDescrip.Size = new System.Drawing.Size(70, 14);
            this.labDescrip.TabIndex = 0;
            this.labDescrip.Text = "labelControl1";
            // 
            // panelDown
            // 
            this.panelDown.Controls.Add(this.btn_CancelOrOk);
            this.panelDown.Controls.Add(this.btn_Next);
            this.panelDown.Controls.Add(this.btn_Back);
            this.panelDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDown.Location = new System.Drawing.Point(0, 290);
            this.panelDown.Name = "panelDown";
            this.panelDown.Size = new System.Drawing.Size(458, 58);
            this.panelDown.TabIndex = 5;
            this.panelDown.Text = "panelControl1";
            // 
            // btn_CancelOrOk
            // 
            this.btn_CancelOrOk.Location = new System.Drawing.Point(371, 17);
            this.btn_CancelOrOk.Name = "btn_CancelOrOk";
            this.btn_CancelOrOk.Size = new System.Drawing.Size(75, 23);
            this.btn_CancelOrOk.TabIndex = 7;
            this.btn_CancelOrOk.Text = "取消";
            this.btn_CancelOrOk.Click += new System.EventHandler(this.btn_CancelOrOk_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Location = new System.Drawing.Point(279, 17);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(75, 23);
            this.btn_Next.TabIndex = 6;
            this.btn_Next.Text = "下一步";
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // btn_Back
            // 
            this.btn_Back.Location = new System.Drawing.Point(198, 17);
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(75, 23);
            this.btn_Back.TabIndex = 5;
            this.btn_Back.Text = "上一步";
            this.btn_Back.Click += new System.EventHandler(this.btn_Back_Click);
            // 
            // panelMid
            // 
            this.panelMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMid.Location = new System.Drawing.Point(0, 51);
            this.panelMid.Name = "panelMid";
            this.panelMid.Size = new System.Drawing.Size(458, 239);
            this.panelMid.TabIndex = 4;
            this.panelMid.Text = "panelControl1";
            // 
            // DispositionGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 348);
            this.Controls.Add(this.panelMid);
            this.Controls.Add(this.panelDown);
            this.Controls.Add(this.panelUp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DispositionGuide";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "撮合中心配置向导";
            this.Load += new System.EventHandler(this.DispositionGuide_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelUp)).EndInit();
            this.panelUp.ResumeLayout(false);
            this.panelUp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelDown)).EndInit();
            this.panelDown.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelMid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelUp;
        private DevExpress.XtraEditors.PanelControl panelDown;
        private DevExpress.XtraEditors.SimpleButton btn_CancelOrOk;
        private DevExpress.XtraEditors.SimpleButton btn_Next;
        private DevExpress.XtraEditors.SimpleButton btn_Back;
        private DevExpress.XtraEditors.PanelControl panelMid;
        private DevExpress.XtraEditors.LabelControl labDescrip;
    }
}