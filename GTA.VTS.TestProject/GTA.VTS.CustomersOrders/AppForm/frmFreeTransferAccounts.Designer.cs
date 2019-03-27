namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class ConvertTransfer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertTransfer));
            this.lblFromCaption = new System.Windows.Forms.Label();
            this.lblToCaption = new System.Windows.Forms.Label();
            this.lblCurrencyType = new System.Windows.Forms.Label();
            this.lblTransferAmount = new System.Windows.Forms.Label();
            this.cmbFromCaption = new System.Windows.Forms.ComboBox();
            this.cmbToCaption = new System.Windows.Forms.ComboBox();
            this.cmbCurrencyType = new System.Windows.Forms.ComboBox();
            this.txtCaptionNum = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dagCaption = new System.Windows.Forms.DataGridView();
            this.AvailableCapitalFie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BalanceOfTheDayFie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapitalRemainAmountFie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FreezeCapitalFie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TodayOutInCapitalFie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TradeCurrencyTypeLogoFie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrencyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserAccountDistributeLogoFie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnQuery = new System.Windows.Forms.Button();
            this.error = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dagCaption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.error)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFromCaption
            // 
            this.lblFromCaption.AutoSize = true;
            this.lblFromCaption.Location = new System.Drawing.Point(390, 28);
            this.lblFromCaption.Name = "lblFromCaption";
            this.lblFromCaption.Size = new System.Drawing.Size(59, 12);
            this.lblFromCaption.TabIndex = 2;
            this.lblFromCaption.Text = "转出帐号:";
            // 
            // lblToCaption
            // 
            this.lblToCaption.AutoSize = true;
            this.lblToCaption.Location = new System.Drawing.Point(393, 58);
            this.lblToCaption.Name = "lblToCaption";
            this.lblToCaption.Size = new System.Drawing.Size(59, 12);
            this.lblToCaption.TabIndex = 3;
            this.lblToCaption.Text = "转入帐号:";
            // 
            // lblCurrencyType
            // 
            this.lblCurrencyType.AutoSize = true;
            this.lblCurrencyType.Location = new System.Drawing.Point(416, 84);
            this.lblCurrencyType.Name = "lblCurrencyType";
            this.lblCurrencyType.Size = new System.Drawing.Size(35, 12);
            this.lblCurrencyType.TabIndex = 4;
            this.lblCurrencyType.Text = "币种:";
            // 
            // lblTransferAmount
            // 
            this.lblTransferAmount.AutoSize = true;
            this.lblTransferAmount.Location = new System.Drawing.Point(390, 111);
            this.lblTransferAmount.Name = "lblTransferAmount";
            this.lblTransferAmount.Size = new System.Drawing.Size(59, 12);
            this.lblTransferAmount.TabIndex = 5;
            this.lblTransferAmount.Text = "转账金额:";
            // 
            // cmbFromCaption
            // 
            this.cmbFromCaption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromCaption.FormattingEnabled = true;
            this.cmbFromCaption.Items.AddRange(new object[] {
            "银行帐号",
            "证券资金帐号",
            "商品期货资金帐号",
            "股指期货资金帐号",
            "港股资金帐号"});
            this.cmbFromCaption.Location = new System.Drawing.Point(502, 23);
            this.cmbFromCaption.Name = "cmbFromCaption";
            this.cmbFromCaption.Size = new System.Drawing.Size(121, 20);
            this.cmbFromCaption.TabIndex = 6;
            this.cmbFromCaption.SelectedIndexChanged += new System.EventHandler(this.cmbFromCaption_SelectedIndexChanged);
            // 
            // cmbToCaption
            // 
            this.cmbToCaption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToCaption.FormattingEnabled = true;
            this.cmbToCaption.Location = new System.Drawing.Point(502, 52);
            this.cmbToCaption.Name = "cmbToCaption";
            this.cmbToCaption.Size = new System.Drawing.Size(121, 20);
            this.cmbToCaption.TabIndex = 7;
            // 
            // cmbCurrencyType
            // 
            this.cmbCurrencyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrencyType.FormattingEnabled = true;
            this.cmbCurrencyType.Items.AddRange(new object[] {
            "人民币",
            "港币",
            "美元"});
            this.cmbCurrencyType.Location = new System.Drawing.Point(501, 80);
            this.cmbCurrencyType.Name = "cmbCurrencyType";
            this.cmbCurrencyType.Size = new System.Drawing.Size(121, 20);
            this.cmbCurrencyType.TabIndex = 8;
            // 
            // txtCaptionNum
            // 
            this.txtCaptionNum.Location = new System.Drawing.Point(501, 108);
            this.txtCaptionNum.MaxLength = 12;
            this.txtCaptionNum.Name = "txtCaptionNum";
            this.txtCaptionNum.Size = new System.Drawing.Size(121, 21);
            this.txtCaptionNum.TabIndex = 9;
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Location = new System.Drawing.Point(406, 143);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Location = new System.Drawing.Point(536, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.txtCaptionNum);
            this.groupBox1.Controls.Add(this.cmbCurrencyType);
            this.groupBox1.Controls.Add(this.cmbToCaption);
            this.groupBox1.Controls.Add(this.cmbFromCaption);
            this.groupBox1.Controls.Add(this.lblTransferAmount);
            this.groupBox1.Controls.Add(this.lblCurrencyType);
            this.groupBox1.Controls.Add(this.lblToCaption);
            this.groupBox1.Controls.Add(this.lblFromCaption);
            this.groupBox1.Location = new System.Drawing.Point(1, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1054, 470);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "自由转账";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dagCaption);
            this.groupBox2.Controls.Add(this.btnQuery);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1054, 163);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "资金查询";
            // 
            // dagCaption
            // 
            this.dagCaption.AllowUserToAddRows = false;
            this.dagCaption.AllowUserToDeleteRows = false;
            this.dagCaption.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dagCaption.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dagCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dagCaption.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dagCaption.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dagCaption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dagCaption.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AvailableCapitalFie,
            this.BalanceOfTheDayFie,
            this.CapitalRemainAmountFie,
            this.FreezeCapitalFie,
            this.TodayOutInCapitalFie,
            this.TradeCurrencyTypeLogoFie,
            this.CurrencyType,
            this.UserAccountDistributeLogoFie});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dagCaption.DefaultCellStyle = dataGridViewCellStyle3;
            this.dagCaption.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dagCaption.Location = new System.Drawing.Point(8, 43);
            this.dagCaption.Name = "dagCaption";
            this.dagCaption.RowTemplate.Height = 23;
            this.dagCaption.Size = new System.Drawing.Size(1040, 114);
            this.dagCaption.TabIndex = 1;
            // 
            // AvailableCapitalFie
            // 
            this.AvailableCapitalFie.DataPropertyName = "AvailableCapital";
            this.AvailableCapitalFie.HeaderText = "AvailableCapital";
            this.AvailableCapitalFie.Name = "AvailableCapitalFie";
            this.AvailableCapitalFie.Width = 150;
            // 
            // BalanceOfTheDayFie
            // 
            this.BalanceOfTheDayFie.DataPropertyName = "BalanceOfTheDay";
            this.BalanceOfTheDayFie.HeaderText = "BalanceOfTheDay";
            this.BalanceOfTheDayFie.Name = "BalanceOfTheDayFie";
            this.BalanceOfTheDayFie.Width = 125;
            // 
            // CapitalRemainAmountFie
            // 
            this.CapitalRemainAmountFie.DataPropertyName = "CapitalRemainAmount";
            this.CapitalRemainAmountFie.HeaderText = "CapitalRemainAmount";
            this.CapitalRemainAmountFie.Name = "CapitalRemainAmountFie";
            this.CapitalRemainAmountFie.Width = 125;
            // 
            // FreezeCapitalFie
            // 
            this.FreezeCapitalFie.DataPropertyName = "FreezeCapital";
            this.FreezeCapitalFie.HeaderText = "FreezeCapital";
            this.FreezeCapitalFie.Name = "FreezeCapitalFie";
            this.FreezeCapitalFie.Width = 125;
            // 
            // TodayOutInCapitalFie
            // 
            this.TodayOutInCapitalFie.DataPropertyName = "TodayOutInCapital";
            this.TodayOutInCapitalFie.HeaderText = "TodayOutInCapital";
            this.TodayOutInCapitalFie.Name = "TodayOutInCapitalFie";
            this.TodayOutInCapitalFie.Width = 150;
            // 
            // TradeCurrencyTypeLogoFie
            // 
            this.TradeCurrencyTypeLogoFie.DataPropertyName = "TradeCurrencyTypeLogo";
            this.TradeCurrencyTypeLogoFie.HeaderText = "TradeCurrencyTypeLogo";
            this.TradeCurrencyTypeLogoFie.Name = "TradeCurrencyTypeLogoFie";
            this.TradeCurrencyTypeLogoFie.Visible = false;
            this.TradeCurrencyTypeLogoFie.Width = 150;
            // 
            // CurrencyType
            // 
            this.CurrencyType.HeaderText = "TradeCurrencyTypeLogo";
            this.CurrencyType.Name = "CurrencyType";
            this.CurrencyType.Width = 150;
            // 
            // UserAccountDistributeLogoFie
            // 
            this.UserAccountDistributeLogoFie.DataPropertyName = "UserAccountDistributeLogo";
            this.UserAccountDistributeLogoFie.HeaderText = "UserAccountDistributeLogo";
            this.UserAccountDistributeLogoFie.Name = "UserAccountDistributeLogoFie";
            this.UserAccountDistributeLogoFie.Width = 150;
            // 
            // btnQuery
            // 
            this.btnQuery.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnQuery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnQuery.Location = new System.Drawing.Point(9, 16);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "资金查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.button3_Click);
            // 
            // error
            // 
            this.error.ContainerControl = this;
            // 
            // ConvertTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1054, 641);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(20000, 20000);
            this.MinimizeBox = false;
            this.Name = "ConvertTransfer";
            this.Text = "资金帐号自由转账";
            this.Load += new System.EventHandler(this.ConvertTransfer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dagCaption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.error)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFromCaption;
        private System.Windows.Forms.Label lblToCaption;
        private System.Windows.Forms.Label lblCurrencyType;
        private System.Windows.Forms.Label lblTransferAmount;
        private System.Windows.Forms.ComboBox cmbFromCaption;
        private System.Windows.Forms.ComboBox cmbToCaption;
        private System.Windows.Forms.ComboBox cmbCurrencyType;
        private System.Windows.Forms.TextBox txtCaptionNum;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dagCaption;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableCapitalFie;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalanceOfTheDayFie;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalRemainAmountFie;
        private System.Windows.Forms.DataGridViewTextBoxColumn FreezeCapitalFie;
        private System.Windows.Forms.DataGridViewTextBoxColumn TodayOutInCapitalFie;
        private System.Windows.Forms.DataGridViewTextBoxColumn TradeCurrencyTypeLogoFie;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrencyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserAccountDistributeLogoFie;
        private System.Windows.Forms.ErrorProvider error;
    }
}