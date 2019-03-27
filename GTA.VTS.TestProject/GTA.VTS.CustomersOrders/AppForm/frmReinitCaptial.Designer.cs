namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class frmReinitCaptial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReinitCaptial));
            this.lblTestDesc = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.lblTestTradeID = new System.Windows.Forms.Label();
            this.cmbmoney = new System.Windows.Forms.ComboBox();
            this.lblTestCapitalCurrency = new System.Windows.Forms.Label();
            this.lblTestCapitalType = new System.Windows.Forms.Label();
            this.butPersonalization = new System.Windows.Forms.Button();
            this.cmbCapitalType = new System.Windows.Forms.ComboBox();
            this.butCleat = new System.Windows.Forms.Button();
            this.lblTestCapitalAmount = new System.Windows.Forms.Label();
            this.txtCapitals = new System.Windows.Forms.TextBox();
            this.errPro = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errPro)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTestDesc
            // 
            this.lblTestDesc.AutoSize = true;
            this.lblTestDesc.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblTestDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTestDesc.Location = new System.Drawing.Point(513, 125);
            this.lblTestDesc.Name = "lblTestDesc";
            this.lblTestDesc.Size = new System.Drawing.Size(197, 12);
            this.lblTestDesc.TabIndex = 12;
            this.lblTestDesc.Text = "注：输入的交易员ID之间用逗号分隔";
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(515, 97);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(121, 21);
            this.txtUserId.TabIndex = 11;
            // 
            // lblTestTradeID
            // 
            this.lblTestTradeID.AutoSize = true;
            this.lblTestTradeID.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblTestTradeID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTestTradeID.Location = new System.Drawing.Point(418, 100);
            this.lblTestTradeID.Name = "lblTestTradeID";
            this.lblTestTradeID.Size = new System.Drawing.Size(59, 12);
            this.lblTestTradeID.TabIndex = 1;
            this.lblTestTradeID.Text = "交易员ID:";
            // 
            // cmbmoney
            // 
            this.cmbmoney.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbmoney.FormattingEnabled = true;
            this.cmbmoney.Items.AddRange(new object[] {
            "所有币种",
            "人民币",
            "港币",
            "美元"});
            this.cmbmoney.Location = new System.Drawing.Point(515, 167);
            this.cmbmoney.Name = "cmbmoney";
            this.cmbmoney.Size = new System.Drawing.Size(121, 20);
            this.cmbmoney.TabIndex = 9;
            // 
            // lblTestCapitalCurrency
            // 
            this.lblTestCapitalCurrency.AutoSize = true;
            this.lblTestCapitalCurrency.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblTestCapitalCurrency.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTestCapitalCurrency.Location = new System.Drawing.Point(418, 172);
            this.lblTestCapitalCurrency.Name = "lblTestCapitalCurrency";
            this.lblTestCapitalCurrency.Size = new System.Drawing.Size(59, 12);
            this.lblTestCapitalCurrency.TabIndex = 8;
            this.lblTestCapitalCurrency.Text = "资金币种:";
            // 
            // lblTestCapitalType
            // 
            this.lblTestCapitalType.AutoSize = true;
            this.lblTestCapitalType.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblTestCapitalType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTestCapitalType.Location = new System.Drawing.Point(418, 144);
            this.lblTestCapitalType.Name = "lblTestCapitalType";
            this.lblTestCapitalType.Size = new System.Drawing.Size(59, 12);
            this.lblTestCapitalType.TabIndex = 2;
            this.lblTestCapitalType.Text = "资金类型:";
            // 
            // butPersonalization
            // 
            this.butPersonalization.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.butPersonalization.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butPersonalization.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butPersonalization.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.butPersonalization.Location = new System.Drawing.Point(547, 228);
            this.butPersonalization.Name = "butPersonalization";
            this.butPersonalization.Size = new System.Drawing.Size(75, 23);
            this.butPersonalization.TabIndex = 7;
            this.butPersonalization.Text = "个性化";
            this.butPersonalization.UseVisualStyleBackColor = true;
            this.butPersonalization.Click += new System.EventHandler(this.butPersonalization_Click);
            // 
            // cmbCapitalType
            // 
            this.cmbCapitalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCapitalType.FormattingEnabled = true;
            this.cmbCapitalType.Items.AddRange(new object[] {
            "所有资金",
            "银行卡资金",
            "现货资金",
            "期货资金",
            "港股资金",
            "除银行卡之外资金"});
            this.cmbCapitalType.Location = new System.Drawing.Point(515, 141);
            this.cmbCapitalType.Name = "cmbCapitalType";
            this.cmbCapitalType.Size = new System.Drawing.Size(121, 20);
            this.cmbCapitalType.TabIndex = 3;
            // 
            // butCleat
            // 
            this.butCleat.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.butCleat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butCleat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butCleat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.butCleat.Location = new System.Drawing.Point(434, 228);
            this.butCleat.Name = "butCleat";
            this.butCleat.Size = new System.Drawing.Size(75, 23);
            this.butCleat.TabIndex = 6;
            this.butCleat.Text = "清空数据";
            this.butCleat.UseVisualStyleBackColor = true;
            this.butCleat.Click += new System.EventHandler(this.butCleat_Click);
            // 
            // lblTestCapitalAmount
            // 
            this.lblTestCapitalAmount.AutoSize = true;
            this.lblTestCapitalAmount.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblTestCapitalAmount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTestCapitalAmount.Location = new System.Drawing.Point(417, 200);
            this.lblTestCapitalAmount.Name = "lblTestCapitalAmount";
            this.lblTestCapitalAmount.Size = new System.Drawing.Size(59, 12);
            this.lblTestCapitalAmount.TabIndex = 4;
            this.lblTestCapitalAmount.Text = "资金数目:";
            // 
            // txtCapitals
            // 
            this.txtCapitals.Location = new System.Drawing.Point(515, 193);
            this.txtCapitals.Name = "txtCapitals";
            this.txtCapitals.Size = new System.Drawing.Size(120, 21);
            this.txtCapitals.TabIndex = 5;
            // 
            // errPro
            // 
            this.errPro.ContainerControl = this;
            // 
            // frmReinitCaptial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1044, 642);
            this.Controls.Add(this.lblTestTradeID);
            this.Controls.Add(this.lblTestDesc);
            this.Controls.Add(this.cmbCapitalType);
            this.Controls.Add(this.cmbmoney);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.lblTestCapitalAmount);
            this.Controls.Add(this.lblTestCapitalType);
            this.Controls.Add(this.txtCapitals);
            this.Controls.Add(this.butPersonalization);
            this.Controls.Add(this.lblTestCapitalCurrency);
            this.Controls.Add(this.butCleat);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2000, 2000);
            this.MinimizeBox = false;
            this.Name = "frmReinitCaptial";
            this.Text = "重新初始化数据";
            ((System.ComponentModel.ISupportInitialize)(this.errPro)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTestDesc;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label lblTestTradeID;
        private System.Windows.Forms.ComboBox cmbmoney;
        private System.Windows.Forms.Label lblTestCapitalCurrency;
        private System.Windows.Forms.Label lblTestCapitalType;
        private System.Windows.Forms.Button butPersonalization;
        private System.Windows.Forms.ComboBox cmbCapitalType;
        private System.Windows.Forms.Button butCleat;
        private System.Windows.Forms.Label lblTestCapitalAmount;
        private System.Windows.Forms.TextBox txtCapitals;
        private System.Windows.Forms.ErrorProvider errPro;
    }
}