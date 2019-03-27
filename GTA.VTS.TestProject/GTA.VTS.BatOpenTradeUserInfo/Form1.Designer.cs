namespace GTA.VTS.BatOpenTradeUserInfo
{
    partial class Form1
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
            this.btnOpenUserAccount = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOpenAmount = new System.Windows.Forms.TextBox();
            this.txtCapital = new System.Windows.Forms.TextBox();
            this.labCapital = new System.Windows.Forms.Label();
            this.labMessage = new System.Windows.Forms.Label();
            this.chkIsIniHoldAccount = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOpenUserAccount
            // 
            this.btnOpenUserAccount.Location = new System.Drawing.Point(153, 35);
            this.btnOpenUserAccount.Name = "btnOpenUserAccount";
            this.btnOpenUserAccount.Size = new System.Drawing.Size(75, 23);
            this.btnOpenUserAccount.TabIndex = 0;
            this.btnOpenUserAccount.Text = "OpenAccount";
            this.btnOpenUserAccount.UseVisualStyleBackColor = true;
            this.btnOpenUserAccount.Click += new System.EventHandler(this.btnOpenUserAccount_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Amount";
            // 
            // txtOpenAmount
            // 
            this.txtOpenAmount.Location = new System.Drawing.Point(60, 10);
            this.txtOpenAmount.Name = "txtOpenAmount";
            this.txtOpenAmount.Size = new System.Drawing.Size(87, 21);
            this.txtOpenAmount.TabIndex = 2;
            this.txtOpenAmount.Text = "100";
            // 
            // txtCapital
            // 
            this.txtCapital.Location = new System.Drawing.Point(60, 37);
            this.txtCapital.Name = "txtCapital";
            this.txtCapital.Size = new System.Drawing.Size(87, 21);
            this.txtCapital.TabIndex = 3;
            this.txtCapital.Text = "100000000000";
            // 
            // labCapital
            // 
            this.labCapital.AutoSize = true;
            this.labCapital.Location = new System.Drawing.Point(13, 40);
            this.labCapital.Name = "labCapital";
            this.labCapital.Size = new System.Drawing.Size(47, 12);
            this.labCapital.TabIndex = 4;
            this.labCapital.Text = "Capital";
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.ForeColor = System.Drawing.Color.Red;
            this.labMessage.Location = new System.Drawing.Point(28, 89);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(0, 12);
            this.labMessage.TabIndex = 5;
            // 
            // chkIsIniHoldAccount
            // 
            this.chkIsIniHoldAccount.AutoSize = true;
            this.chkIsIniHoldAccount.Location = new System.Drawing.Point(15, 64);
            this.chkIsIniHoldAccount.Name = "chkIsIniHoldAccount";
            this.chkIsIniHoldAccount.Size = new System.Drawing.Size(96, 16);
            this.chkIsIniHoldAccount.TabIndex = 6;
            this.chkIsIniHoldAccount.Text = "Is Init Hold";
            this.chkIsIniHoldAccount.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 110);
            this.Controls.Add(this.chkIsIniHoldAccount);
            this.Controls.Add(this.labMessage);
            this.Controls.Add(this.labCapital);
            this.Controls.Add(this.txtCapital);
            this.Controls.Add(this.txtOpenAmount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOpenUserAccount);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenUserAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOpenAmount;
        private System.Windows.Forms.TextBox txtCapital;
        private System.Windows.Forms.Label labCapital;
        private System.Windows.Forms.Label labMessage;
        private System.Windows.Forms.CheckBox chkIsIniHoldAccount;
    }
}

