namespace ReckoningCapabilityTest
{
    partial class QhForm
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
            this.btnFuteMax = new DevExpress.XtraEditors.SimpleButton();
            this.btnQhHold = new DevExpress.XtraEditors.SimpleButton();
            this.btnXHMaxAccount = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnFuteMax
            // 
            this.btnFuteMax.Location = new System.Drawing.Point(47, 42);
            this.btnFuteMax.Name = "btnFuteMax";
            this.btnFuteMax.Size = new System.Drawing.Size(176, 23);
            this.btnFuteMax.TabIndex = 15;
            this.btnFuteMax.Text = " 股指期最大委托量查询";
            this.btnFuteMax.Click += new System.EventHandler(this.btnFuteMax_Click);
            // 
            // btnQhHold
            // 
            this.btnQhHold.Location = new System.Drawing.Point(47, 104);
            this.btnQhHold.Name = "btnQhHold";
            this.btnQhHold.Size = new System.Drawing.Size(148, 23);
            this.btnQhHold.TabIndex = 16;
            this.btnQhHold.Text = "期货持仓查询";
            this.btnQhHold.Click += new System.EventHandler(this.btnQhHold_Click);
            // 
            // btnXHMaxAccount
            // 
            this.btnXHMaxAccount.Location = new System.Drawing.Point(47, 175);
            this.btnXHMaxAccount.Name = "btnXHMaxAccount";
            this.btnXHMaxAccount.Size = new System.Drawing.Size(148, 23);
            this.btnXHMaxAccount.TabIndex = 17;
            this.btnXHMaxAccount.Text = "现货最大委托量查询";
            this.btnXHMaxAccount.Click += new System.EventHandler(this.btnXHMaxAccount_Click);
            // 
            // QhForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.btnXHMaxAccount);
            this.Controls.Add(this.btnQhHold);
            this.Controls.Add(this.btnFuteMax);
            this.Name = "QhForm";
            this.Text = "QhForm";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnFuteMax;
        private DevExpress.XtraEditors.SimpleButton btnQhHold;
        private DevExpress.XtraEditors.SimpleButton btnXHMaxAccount;
    }
}