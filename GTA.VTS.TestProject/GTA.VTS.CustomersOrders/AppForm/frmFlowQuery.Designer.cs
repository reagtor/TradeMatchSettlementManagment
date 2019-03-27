namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class frmFlowQuery
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFlowQuery));
            this.grpQueryTerm = new System.Windows.Forms.GroupBox();
            this.dateTimePickerendTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerstartTime = new System.Windows.Forms.DateTimePicker();
            this.cmbaccountType = new System.Windows.Forms.ComboBox();
            this.lblaccountType = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblCurrencyType = new System.Windows.Forms.Label();
            this.lblCapitalAmount = new System.Windows.Forms.Label();
            this.cmbCurrType = new System.Windows.Forms.ComboBox();
            this.txtTransferAmount = new System.Windows.Forms.TextBox();
            this.lblCapitalFlowType = new System.Windows.Forms.Label();
            this.cmbTransferType = new System.Windows.Forms.ComboBox();
            this.grpQueryresult = new System.Windows.Forms.GroupBox();
            this.dagCapitalFlow = new System.Windows.Forms.DataGridView();
            this.butCapitalFlowQuery = new System.Windows.Forms.Button();
            this.CapitalFlowFromCapitalAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapitalFlowToCapitalAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapitalFlowTransferAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapitalFlowTradeCurrencyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrencyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapitalFlowTransferTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapitalFlowTransferTypeLogo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransferTypeLogo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapitalFlowCapitalFlowLogo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpQueryTerm.SuspendLayout();
            this.grpQueryresult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dagCapitalFlow)).BeginInit();
            this.SuspendLayout();
            // 
            // grpQueryTerm
            // 
            this.grpQueryTerm.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grpQueryTerm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grpQueryTerm.Controls.Add(this.dateTimePickerendTime);
            this.grpQueryTerm.Controls.Add(this.dateTimePickerstartTime);
            this.grpQueryTerm.Controls.Add(this.cmbaccountType);
            this.grpQueryTerm.Controls.Add(this.lblaccountType);
            this.grpQueryTerm.Controls.Add(this.lblEndTime);
            this.grpQueryTerm.Controls.Add(this.lblStartTime);
            this.grpQueryTerm.Controls.Add(this.lblCurrencyType);
            this.grpQueryTerm.Controls.Add(this.lblCapitalAmount);
            this.grpQueryTerm.Controls.Add(this.cmbCurrType);
            this.grpQueryTerm.Controls.Add(this.txtTransferAmount);
            this.grpQueryTerm.Controls.Add(this.lblCapitalFlowType);
            this.grpQueryTerm.Controls.Add(this.cmbTransferType);
            this.grpQueryTerm.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpQueryTerm.Location = new System.Drawing.Point(0, 0);
            this.grpQueryTerm.Name = "grpQueryTerm";
            this.grpQueryTerm.Size = new System.Drawing.Size(1044, 126);
            this.grpQueryTerm.TabIndex = 16;
            this.grpQueryTerm.TabStop = false;
            this.grpQueryTerm.Text = "查询条件";
            // 
            // dateTimePickerendTime
            // 
            this.dateTimePickerendTime.Location = new System.Drawing.Point(598, 64);
            this.dateTimePickerendTime.Name = "dateTimePickerendTime";
            this.dateTimePickerendTime.Size = new System.Drawing.Size(166, 21);
            this.dateTimePickerendTime.TabIndex = 6;
            this.dateTimePickerendTime.ValueChanged += new System.EventHandler(this.dateTimePickerendTime_ValueChanged);
            // 
            // dateTimePickerstartTime
            // 
            this.dateTimePickerstartTime.Location = new System.Drawing.Point(152, 64);
            this.dateTimePickerstartTime.Name = "dateTimePickerstartTime";
            this.dateTimePickerstartTime.Size = new System.Drawing.Size(166, 21);
            this.dateTimePickerstartTime.TabIndex = 5;
            this.dateTimePickerstartTime.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerstartTime.ValueChanged += new System.EventHandler(this.dateTimePickerstartTime_ValueChanged);
            // 
            // cmbaccountType
            // 
            this.cmbaccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbaccountType.FormattingEnabled = true;
            this.cmbaccountType.Items.AddRange(new object[] {
            "银行帐号",
            "证券资金帐户",
            "证券持仓帐户",
            "商品期货资金帐户",
            "商品期货持仓帐户",
            "股指期货资金帐号",
            "股指期货持仓帐户",
            "港股资金帐户",
            "港股持仓帐户"});
            this.cmbaccountType.Location = new System.Drawing.Point(152, 11);
            this.cmbaccountType.Name = "cmbaccountType";
            this.cmbaccountType.Size = new System.Drawing.Size(166, 20);
            this.cmbaccountType.TabIndex = 1;
            // 
            // lblaccountType
            // 
            this.lblaccountType.AutoSize = true;
            this.lblaccountType.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblaccountType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblaccountType.Location = new System.Drawing.Point(53, 21);
            this.lblaccountType.Name = "lblaccountType";
            this.lblaccountType.Size = new System.Drawing.Size(59, 12);
            this.lblaccountType.TabIndex = 16;
            this.lblaccountType.Text = "账号类型:";
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblEndTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblEndTime.Location = new System.Drawing.Point(500, 72);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(59, 12);
            this.lblEndTime.TabIndex = 14;
            this.lblEndTime.Text = "结束时间:";
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblStartTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblStartTime.Location = new System.Drawing.Point(53, 69);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(59, 12);
            this.lblStartTime.TabIndex = 12;
            this.lblStartTime.Text = "开始时间:";
            // 
            // lblCurrencyType
            // 
            this.lblCurrencyType.AutoSize = true;
            this.lblCurrencyType.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblCurrencyType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCurrencyType.Location = new System.Drawing.Point(500, 14);
            this.lblCurrencyType.Name = "lblCurrencyType";
            this.lblCurrencyType.Size = new System.Drawing.Size(59, 12);
            this.lblCurrencyType.TabIndex = 4;
            this.lblCurrencyType.Text = "货币类型:";
            // 
            // lblCapitalAmount
            // 
            this.lblCapitalAmount.AutoSize = true;
            this.lblCapitalAmount.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblCapitalAmount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCapitalAmount.Location = new System.Drawing.Point(500, 41);
            this.lblCapitalAmount.Name = "lblCapitalAmount";
            this.lblCapitalAmount.Size = new System.Drawing.Size(59, 12);
            this.lblCapitalAmount.TabIndex = 11;
            this.lblCapitalAmount.Text = "转账金额:";
            // 
            // cmbCurrType
            // 
            this.cmbCurrType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrType.FormattingEnabled = true;
            this.cmbCurrType.Items.AddRange(new object[] {
            "ALL",
            "RMB",
            "HK",
            "US"});
            this.cmbCurrType.Location = new System.Drawing.Point(598, 11);
            this.cmbCurrType.Name = "cmbCurrType";
            this.cmbCurrType.Size = new System.Drawing.Size(166, 20);
            this.cmbCurrType.TabIndex = 2;
            // 
            // txtTransferAmount
            // 
            this.txtTransferAmount.Location = new System.Drawing.Point(598, 39);
            this.txtTransferAmount.Name = "txtTransferAmount";
            this.txtTransferAmount.Size = new System.Drawing.Size(166, 21);
            this.txtTransferAmount.TabIndex = 4;
            // 
            // lblCapitalFlowType
            // 
            this.lblCapitalFlowType.AutoSize = true;
            this.lblCapitalFlowType.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblCapitalFlowType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCapitalFlowType.Location = new System.Drawing.Point(51, 44);
            this.lblCapitalFlowType.Name = "lblCapitalFlowType";
            this.lblCapitalFlowType.Size = new System.Drawing.Size(59, 12);
            this.lblCapitalFlowType.TabIndex = 6;
            this.lblCapitalFlowType.Text = "转账类型:";
            // 
            // cmbTransferType
            // 
            this.cmbTransferType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransferType.FormattingEnabled = true;
            this.cmbTransferType.Items.AddRange(new object[] {
            "自由转账",
            "分红转账",
            "追加资金",
            "全部"});
            this.cmbTransferType.Location = new System.Drawing.Point(152, 38);
            this.cmbTransferType.Name = "cmbTransferType";
            this.cmbTransferType.Size = new System.Drawing.Size(166, 20);
            this.cmbTransferType.TabIndex = 3;
            // 
            // grpQueryresult
            // 
            this.grpQueryresult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpQueryresult.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grpQueryresult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grpQueryresult.Controls.Add(this.dagCapitalFlow);
            this.grpQueryresult.Controls.Add(this.butCapitalFlowQuery);
            this.grpQueryresult.Location = new System.Drawing.Point(0, 126);
            this.grpQueryresult.Name = "grpQueryresult";
            this.grpQueryresult.Size = new System.Drawing.Size(1044, 517);
            this.grpQueryresult.TabIndex = 18;
            this.grpQueryresult.TabStop = false;
            this.grpQueryresult.Text = "查询结果";
            // 
            // dagCapitalFlow
            // 
            this.dagCapitalFlow.AllowUserToAddRows = false;
            this.dagCapitalFlow.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dagCapitalFlow.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dagCapitalFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dagCapitalFlow.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dagCapitalFlow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dagCapitalFlow.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dagCapitalFlow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dagCapitalFlow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CapitalFlowFromCapitalAccount,
            this.CapitalFlowToCapitalAccount,
            this.CapitalFlowTransferAmount,
            this.CapitalFlowTradeCurrencyType,
            this.CurrencyType,
            this.CapitalFlowTransferTime,
            this.CapitalFlowTransferTypeLogo,
            this.TransferTypeLogo,
            this.CapitalFlowCapitalFlowLogo});
            this.dagCapitalFlow.Location = new System.Drawing.Point(6, 50);
            this.dagCapitalFlow.Name = "dagCapitalFlow";
            this.dagCapitalFlow.ReadOnly = true;
            this.dagCapitalFlow.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Info;
            this.dagCapitalFlow.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dagCapitalFlow.RowTemplate.Height = 23;
            this.dagCapitalFlow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dagCapitalFlow.Size = new System.Drawing.Size(1032, 462);
            this.dagCapitalFlow.TabIndex = 16;
            // 
            // butCapitalFlowQuery
            // 
            this.butCapitalFlowQuery.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.butCapitalFlowQuery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butCapitalFlowQuery.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butCapitalFlowQuery.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.butCapitalFlowQuery.Location = new System.Drawing.Point(6, 20);
            this.butCapitalFlowQuery.Name = "butCapitalFlowQuery";
            this.butCapitalFlowQuery.Size = new System.Drawing.Size(75, 23);
            this.butCapitalFlowQuery.TabIndex = 7;
            this.butCapitalFlowQuery.Text = "查询";
            this.butCapitalFlowQuery.UseVisualStyleBackColor = true;
            this.butCapitalFlowQuery.Click += new System.EventHandler(this.butCapitalFlowQuery_Click);
            // 
            // CapitalFlowFromCapitalAccount
            // 
            this.CapitalFlowFromCapitalAccount.DataPropertyName = "FromCapitalAccount";
            this.CapitalFlowFromCapitalAccount.HeaderText = "FromCapitalAccount";
            this.CapitalFlowFromCapitalAccount.Name = "CapitalFlowFromCapitalAccount";
            this.CapitalFlowFromCapitalAccount.ReadOnly = true;
            this.CapitalFlowFromCapitalAccount.Width = 150;
            // 
            // CapitalFlowToCapitalAccount
            // 
            this.CapitalFlowToCapitalAccount.DataPropertyName = "ToCapitalAccount";
            this.CapitalFlowToCapitalAccount.HeaderText = "ToCapitalAccount";
            this.CapitalFlowToCapitalAccount.Name = "CapitalFlowToCapitalAccount";
            this.CapitalFlowToCapitalAccount.ReadOnly = true;
            this.CapitalFlowToCapitalAccount.Width = 150;
            // 
            // CapitalFlowTransferAmount
            // 
            this.CapitalFlowTransferAmount.DataPropertyName = "TransferAmount";
            this.CapitalFlowTransferAmount.HeaderText = "TransferAmount";
            this.CapitalFlowTransferAmount.Name = "CapitalFlowTransferAmount";
            this.CapitalFlowTransferAmount.ReadOnly = true;
            // 
            // CapitalFlowTradeCurrencyType
            // 
            this.CapitalFlowTradeCurrencyType.DataPropertyName = "TradeCurrencyType";
            this.CapitalFlowTradeCurrencyType.HeaderText = "TradeCurrencyType";
            this.CapitalFlowTradeCurrencyType.Name = "CapitalFlowTradeCurrencyType";
            this.CapitalFlowTradeCurrencyType.ReadOnly = true;
            this.CapitalFlowTradeCurrencyType.Visible = false;
            // 
            // CurrencyType
            // 
            this.CurrencyType.HeaderText = "TradeCurrencyType";
            this.CurrencyType.Name = "CurrencyType";
            this.CurrencyType.ReadOnly = true;
            // 
            // CapitalFlowTransferTime
            // 
            this.CapitalFlowTransferTime.DataPropertyName = "TransferTime";
            this.CapitalFlowTransferTime.HeaderText = "TransferTime";
            this.CapitalFlowTransferTime.Name = "CapitalFlowTransferTime";
            this.CapitalFlowTransferTime.ReadOnly = true;
            // 
            // CapitalFlowTransferTypeLogo
            // 
            this.CapitalFlowTransferTypeLogo.DataPropertyName = "TransferTypeLogo";
            this.CapitalFlowTransferTypeLogo.HeaderText = "TransferTypeLogo";
            this.CapitalFlowTransferTypeLogo.Name = "CapitalFlowTransferTypeLogo";
            this.CapitalFlowTransferTypeLogo.ReadOnly = true;
            this.CapitalFlowTransferTypeLogo.Visible = false;
            // 
            // TransferTypeLogo
            // 
            this.TransferTypeLogo.HeaderText = "TransferTypeLogo";
            this.TransferTypeLogo.Name = "TransferTypeLogo";
            this.TransferTypeLogo.ReadOnly = true;
            // 
            // CapitalFlowCapitalFlowLogo
            // 
            this.CapitalFlowCapitalFlowLogo.DataPropertyName = "CapitalFlowLogo";
            this.CapitalFlowCapitalFlowLogo.HeaderText = "CapitalFlowLogo";
            this.CapitalFlowCapitalFlowLogo.Name = "CapitalFlowCapitalFlowLogo";
            this.CapitalFlowCapitalFlowLogo.ReadOnly = true;
            this.CapitalFlowCapitalFlowLogo.Width = 150;
            // 
            // frmFlowQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 642);
            this.Controls.Add(this.grpQueryresult);
            this.Controls.Add(this.grpQueryTerm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2000, 2000);
            this.MinimizeBox = false;
            this.Name = "frmFlowQuery";
            this.Text = "资金流水查询";
            this.Load += new System.EventHandler(this.frmFlowQuery_Load);
            this.grpQueryTerm.ResumeLayout(false);
            this.grpQueryTerm.PerformLayout();
            this.grpQueryresult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dagCapitalFlow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpQueryTerm;
        private System.Windows.Forms.DateTimePicker dateTimePickerendTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerstartTime;
        private System.Windows.Forms.ComboBox cmbaccountType;
        private System.Windows.Forms.Label lblaccountType;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblCurrencyType;
        private System.Windows.Forms.Label lblCapitalAmount;
        private System.Windows.Forms.ComboBox cmbCurrType;
        private System.Windows.Forms.TextBox txtTransferAmount;
        private System.Windows.Forms.Label lblCapitalFlowType;
        private System.Windows.Forms.ComboBox cmbTransferType;
        private System.Windows.Forms.GroupBox grpQueryresult;
        private System.Windows.Forms.DataGridView dagCapitalFlow;
        private System.Windows.Forms.Button butCapitalFlowQuery;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalFlowFromCapitalAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalFlowToCapitalAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalFlowTransferAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalFlowTradeCurrencyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrencyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalFlowTransferTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalFlowTransferTypeLogo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferTypeLogo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapitalFlowCapitalFlowLogo;
    }
}