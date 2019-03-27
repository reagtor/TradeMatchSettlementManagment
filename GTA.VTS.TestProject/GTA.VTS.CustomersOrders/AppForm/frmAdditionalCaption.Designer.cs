namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class frmAdditionalCaption
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdditionalCaption));
            this.lblTradeId = new System.Windows.Forms.Label();
            this.txtTradeID = new System.Windows.Forms.TextBox();
            this.txtRMB = new System.Windows.Forms.TextBox();
            this.lblRMB = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbCaption = new System.Windows.Forms.GroupBox();
            this.lblUSA = new System.Windows.Forms.Label();
            this.lblHK = new System.Windows.Forms.Label();
            this.txtUSA = new System.Windows.Forms.TextBox();
            this.txtHK = new System.Windows.Forms.TextBox();
            this.error = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnIsReckoningDone = new System.Windows.Forms.Button();
            this.btnIsReckoning = new System.Windows.Forms.Button();
            this.grbReckoning = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblIsReckoning = new System.Windows.Forms.Label();
            this.lblIsReckoningDone = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.grbCaption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.error)).BeginInit();
            this.grbReckoning.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTradeId
            // 
            this.lblTradeId.AutoSize = true;
            this.lblTradeId.Location = new System.Drawing.Point(88, 32);
            this.lblTradeId.Name = "lblTradeId";
            this.lblTradeId.Size = new System.Drawing.Size(59, 12);
            this.lblTradeId.TabIndex = 0;
            this.lblTradeId.Text = "交易员ID:";
            // 
            // txtTradeID
            // 
            this.txtTradeID.Location = new System.Drawing.Point(170, 27);
            this.txtTradeID.Name = "txtTradeID";
            this.txtTradeID.Size = new System.Drawing.Size(100, 21);
            this.txtTradeID.TabIndex = 1;
            // 
            // txtRMB
            // 
            this.txtRMB.Location = new System.Drawing.Point(170, 60);
            this.txtRMB.Name = "txtRMB";
            this.txtRMB.Size = new System.Drawing.Size(100, 21);
            this.txtRMB.TabIndex = 2;
            // 
            // lblRMB
            // 
            this.lblRMB.AutoSize = true;
            this.lblRMB.Location = new System.Drawing.Point(99, 63);
            this.lblRMB.Name = "lblRMB";
            this.lblRMB.Size = new System.Drawing.Size(47, 12);
            this.lblRMB.TabIndex = 3;
            this.lblRMB.Text = "人民币:";
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Location = new System.Drawing.Point(90, 163);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Location = new System.Drawing.Point(195, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // grbCaption
            // 
            this.grbCaption.Controls.Add(this.lblUSA);
            this.grbCaption.Controls.Add(this.lblHK);
            this.grbCaption.Controls.Add(this.txtUSA);
            this.grbCaption.Controls.Add(this.txtHK);
            this.grbCaption.Controls.Add(this.lblTradeId);
            this.grbCaption.Controls.Add(this.btnCancel);
            this.grbCaption.Controls.Add(this.txtTradeID);
            this.grbCaption.Controls.Add(this.btnOK);
            this.grbCaption.Controls.Add(this.txtRMB);
            this.grbCaption.Controls.Add(this.lblRMB);
            this.grbCaption.Location = new System.Drawing.Point(52, 53);
            this.grbCaption.Name = "grbCaption";
            this.grbCaption.Size = new System.Drawing.Size(394, 239);
            this.grbCaption.TabIndex = 6;
            this.grbCaption.TabStop = false;
            this.grbCaption.Text = "追加资金";
            // 
            // lblUSA
            // 
            this.lblUSA.AutoSize = true;
            this.lblUSA.Location = new System.Drawing.Point(109, 130);
            this.lblUSA.Name = "lblUSA";
            this.lblUSA.Size = new System.Drawing.Size(35, 12);
            this.lblUSA.TabIndex = 9;
            this.lblUSA.Text = "美元:";
            // 
            // lblHK
            // 
            this.lblHK.AutoSize = true;
            this.lblHK.Location = new System.Drawing.Point(109, 97);
            this.lblHK.Name = "lblHK";
            this.lblHK.Size = new System.Drawing.Size(35, 12);
            this.lblHK.TabIndex = 8;
            this.lblHK.Text = "港币:";
            // 
            // txtUSA
            // 
            this.txtUSA.Location = new System.Drawing.Point(170, 125);
            this.txtUSA.Name = "txtUSA";
            this.txtUSA.Size = new System.Drawing.Size(100, 21);
            this.txtUSA.TabIndex = 7;
            // 
            // txtHK
            // 
            this.txtHK.Location = new System.Drawing.Point(170, 92);
            this.txtHK.Name = "txtHK";
            this.txtHK.Size = new System.Drawing.Size(100, 21);
            this.txtHK.TabIndex = 6;
            // 
            // error
            // 
            this.error.ContainerControl = this;
            // 
            // btnIsReckoningDone
            // 
            this.btnIsReckoningDone.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnIsReckoningDone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIsReckoningDone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIsReckoningDone.Location = new System.Drawing.Point(618, 141);
            this.btnIsReckoningDone.Name = "btnIsReckoningDone";
            this.btnIsReckoningDone.Size = new System.Drawing.Size(125, 23);
            this.btnIsReckoningDone.TabIndex = 8;
            this.btnIsReckoningDone.Text = "清算是否完成";
            this.btnIsReckoningDone.UseVisualStyleBackColor = true;
            this.btnIsReckoningDone.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnIsReckoning
            // 
            this.btnIsReckoning.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnIsReckoning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIsReckoning.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIsReckoning.Location = new System.Drawing.Point(783, 141);
            this.btnIsReckoning.Name = "btnIsReckoning";
            this.btnIsReckoning.Size = new System.Drawing.Size(118, 23);
            this.btnIsReckoning.TabIndex = 9;
            this.btnIsReckoning.Text = "是否正在进行清算";
            this.btnIsReckoning.UseVisualStyleBackColor = true;
            this.btnIsReckoning.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // grbReckoning
            // 
            this.grbReckoning.Controls.Add(this.label6);
            this.grbReckoning.Controls.Add(this.label5);
            this.grbReckoning.Controls.Add(this.lblIsReckoning);
            this.grbReckoning.Controls.Add(this.lblIsReckoningDone);
            this.grbReckoning.Controls.Add(this.lblTime);
            this.grbReckoning.Location = new System.Drawing.Point(525, 52);
            this.grbReckoning.Name = "grbReckoning";
            this.grbReckoning.Size = new System.Drawing.Size(430, 240);
            this.grbReckoning.TabIndex = 11;
            this.grbReckoning.TabStop = false;
            this.grbReckoning.Text = "清算状态";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(376, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 12);
            this.label6.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(185, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 4;
            // 
            // lblIsReckoning
            // 
            this.lblIsReckoning.AutoSize = true;
            this.lblIsReckoning.Location = new System.Drawing.Point(263, 134);
            this.lblIsReckoning.Name = "lblIsReckoning";
            this.lblIsReckoning.Size = new System.Drawing.Size(107, 12);
            this.lblIsReckoning.TabIndex = 3;
            this.lblIsReckoning.Text = "是否正在进行清算:";
            // 
            // lblIsReckoningDone
            // 
            this.lblIsReckoningDone.AutoSize = true;
            this.lblIsReckoningDone.Location = new System.Drawing.Point(95, 131);
            this.lblIsReckoningDone.Name = "lblIsReckoningDone";
            this.lblIsReckoningDone.Size = new System.Drawing.Size(83, 12);
            this.lblIsReckoningDone.TabIndex = 2;
            this.lblIsReckoningDone.Text = "清算是否完成:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(17, 53);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(59, 12);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "清算时间:";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(617, 100);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(124, 21);
            this.txtTime.TabIndex = 10;
            // 
            // frmAdditionalCaption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1004, 549);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.btnIsReckoning);
            this.Controls.Add(this.btnIsReckoningDone);
            this.Controls.Add(this.grbCaption);
            this.Controls.Add(this.grbReckoning);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAdditionalCaption";
            this.Text = "追加资金";
            this.grbCaption.ResumeLayout(false);
            this.grbCaption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.error)).EndInit();
            this.grbReckoning.ResumeLayout(false);
            this.grbReckoning.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTradeId;
        private System.Windows.Forms.TextBox txtTradeID;
        private System.Windows.Forms.TextBox txtRMB;
        private System.Windows.Forms.Label lblRMB;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grbCaption;
        private System.Windows.Forms.Label lblUSA;
        private System.Windows.Forms.Label lblHK;
        private System.Windows.Forms.TextBox txtUSA;
        private System.Windows.Forms.TextBox txtHK;
        private System.Windows.Forms.ErrorProvider error;
        private System.Windows.Forms.Button btnIsReckoningDone;
        private System.Windows.Forms.Button btnIsReckoning;
        private System.Windows.Forms.GroupBox grbReckoning;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label lblIsReckoningDone;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblIsReckoning;
    }
}