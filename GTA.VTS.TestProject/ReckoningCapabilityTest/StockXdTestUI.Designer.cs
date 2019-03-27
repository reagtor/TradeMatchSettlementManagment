namespace ReckoningCapabilityTest
{
    partial class StockXdTestUI
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnXHOrderService = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labXhXDTime = new DevExpress.XtraEditors.LabelControl();
            this.labAgeTime = new DevExpress.XtraEditors.LabelControl();
            this.btnDisXhXdAgeTime = new DevExpress.XtraEditors.SimpleButton();
            this.labAllTimeS = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(28, 63);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(564, 604);
            this.listBox1.TabIndex = 6;
            // 
            // btnXHOrderService
            // 
            this.btnXHOrderService.Location = new System.Drawing.Point(28, 12);
            this.btnXHOrderService.Name = "btnXHOrderService";
            this.btnXHOrderService.Size = new System.Drawing.Size(108, 23);
            this.btnXHOrderService.TabIndex = 7;
            this.btnXHOrderService.Text = "现货下单测试";
            this.btnXHOrderService.Click += new System.EventHandler(this.btnXHOrderService_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(142, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(76, 14);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "下单请求时间:";
            // 
            // labXhXDTime
            // 
            this.labXhXDTime.Location = new System.Drawing.Point(224, 12);
            this.labXhXDTime.Name = "labXhXDTime";
            this.labXhXDTime.Size = new System.Drawing.Size(70, 14);
            this.labXhXDTime.TabIndex = 9;
            this.labXhXDTime.Text = "labelControl1";
            // 
            // labAgeTime
            // 
            this.labAgeTime.Location = new System.Drawing.Point(509, 21);
            this.labAgeTime.Name = "labAgeTime";
            this.labAgeTime.Size = new System.Drawing.Size(70, 14);
            this.labAgeTime.TabIndex = 11;
            this.labAgeTime.Text = "labelControl3";
            // 
            // btnDisXhXdAgeTime
            // 
            this.btnDisXhXdAgeTime.Location = new System.Drawing.Point(329, 12);
            this.btnDisXhXdAgeTime.Name = "btnDisXhXdAgeTime";
            this.btnDisXhXdAgeTime.Size = new System.Drawing.Size(174, 23);
            this.btnDisXhXdAgeTime.TabIndex = 12;
            this.btnDisXhXdAgeTime.Text = "显示现货下单平均速度笔/秒";
            this.btnDisXhXdAgeTime.Click += new System.EventHandler(this.btnDisXhXdAgeTime_Click);
            // 
            // labAllTimeS
            // 
            this.labAllTimeS.Location = new System.Drawing.Point(668, 21);
            this.labAllTimeS.Name = "labAllTimeS";
            this.labAllTimeS.Size = new System.Drawing.Size(61, 14);
            this.labAllTimeS.TabIndex = 13;
            this.labAllTimeS.Text = "labAllTimeS";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(622, 21);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 14);
            this.labelControl3.TabIndex = 13;
            this.labelControl3.Text = "总秒数:";
            // 
            // StockXdTestUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 695);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labAllTimeS);
            this.Controls.Add(this.btnDisXhXdAgeTime);
            this.Controls.Add(this.labAgeTime);
            this.Controls.Add(this.labXhXDTime);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnXHOrderService);
            this.Controls.Add(this.listBox1);
            this.Name = "StockXdTestUI";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private DevExpress.XtraEditors.SimpleButton btnXHOrderService;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labXhXDTime;
        private DevExpress.XtraEditors.LabelControl labAgeTime;
        private DevExpress.XtraEditors.SimpleButton btnDisXhXdAgeTime;
        private DevExpress.XtraEditors.LabelControl labAllTimeS;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}

