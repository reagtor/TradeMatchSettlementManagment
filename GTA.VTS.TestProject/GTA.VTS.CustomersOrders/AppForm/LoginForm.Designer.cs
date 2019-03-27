namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.gpbLoginInfo = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labLanguage = new System.Windows.Forms.Label();
            this.cmbLanguageType = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblTradePassword = new System.Windows.Forms.Label();
            this.txtTradeID = new System.Windows.Forms.TextBox();
            this.lblTradeID = new System.Windows.Forms.Label();
            this.error = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.gpbLoginInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.error)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbLoginInfo
            // 
            this.gpbLoginInfo.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gpbLoginInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gpbLoginInfo.Controls.Add(this.linkLabel1);
            this.gpbLoginInfo.Controls.Add(this.labLanguage);
            this.gpbLoginInfo.Controls.Add(this.cmbLanguageType);
            this.gpbLoginInfo.Controls.Add(this.txtPassword);
            this.gpbLoginInfo.Controls.Add(this.lblTradePassword);
            this.gpbLoginInfo.Controls.Add(this.txtTradeID);
            this.gpbLoginInfo.Controls.Add(this.lblTradeID);
            this.gpbLoginInfo.Location = new System.Drawing.Point(9, 7);
            this.gpbLoginInfo.Name = "gpbLoginInfo";
            this.gpbLoginInfo.Size = new System.Drawing.Size(319, 91);
            this.gpbLoginInfo.TabIndex = 0;
            this.gpbLoginInfo.TabStop = false;
            this.gpbLoginInfo.Text = "Login Info";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(277, 69);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(29, 12);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "配置";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // labLanguage
            // 
            this.labLanguage.AutoSize = true;
            this.labLanguage.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labLanguage.Location = new System.Drawing.Point(24, 68);
            this.labLanguage.Name = "labLanguage";
            this.labLanguage.Size = new System.Drawing.Size(83, 12);
            this.labLanguage.TabIndex = 23;
            this.labLanguage.Text = "Language Type";
            // 
            // cmbLanguageType
            // 
            this.cmbLanguageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguageType.FormattingEnabled = true;
            this.cmbLanguageType.Items.AddRange(new object[] {
            "中文",
            "English"});
            this.cmbLanguageType.Location = new System.Drawing.Point(159, 63);
            this.cmbLanguageType.Name = "cmbLanguageType";
            this.cmbLanguageType.Size = new System.Drawing.Size(112, 20);
            this.cmbLanguageType.TabIndex = 4;
            this.cmbLanguageType.SelectedIndexChanged += new System.EventHandler(this.cmbLanguageType_SelectedIndexChanged);
            this.cmbLanguageType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbLanguageType_KeyPress);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(159, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(112, 21);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // lblTradePassword
            // 
            this.lblTradePassword.AutoSize = true;
            this.lblTradePassword.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblTradePassword.Location = new System.Drawing.Point(24, 42);
            this.lblTradePassword.Name = "lblTradePassword";
            this.lblTradePassword.Size = new System.Drawing.Size(95, 12);
            this.lblTradePassword.TabIndex = 20;
            this.lblTradePassword.Text = "Trade Password:";
            // 
            // txtTradeID
            // 
            this.txtTradeID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTradeID.Location = new System.Drawing.Point(159, 14);
            this.txtTradeID.Name = "txtTradeID";
            this.txtTradeID.Size = new System.Drawing.Size(112, 21);
            this.txtTradeID.TabIndex = 1;
            this.txtTradeID.Text = "9";
            this.txtTradeID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTradeID_KeyPress);
            // 
            // lblTradeID
            // 
            this.lblTradeID.AutoSize = true;
            this.lblTradeID.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblTradeID.Location = new System.Drawing.Point(24, 20);
            this.lblTradeID.Name = "lblTradeID";
            this.lblTradeID.Size = new System.Drawing.Size(59, 12);
            this.lblTradeID.TabIndex = 13;
            this.lblTradeID.Text = "Trade ID:";
            // 
            // error
            // 
            this.error.ContainerControl = this;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnCancel.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Location = new System.Drawing.Point(185, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLogin.Location = new System.Drawing.Point(62, 104);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnLogin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnLogin_KeyPress);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(338, 137);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gpbLoginInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customers Orders--Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.gpbLoginInfo.ResumeLayout(false);
            this.gpbLoginInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.error)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbLoginInfo;
        private System.Windows.Forms.TextBox txtTradeID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblTradePassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider error;
        private System.Windows.Forms.Label labLanguage;
        private System.Windows.Forms.ComboBox cmbLanguageType;
        private System.Windows.Forms.Label lblTradeID;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}