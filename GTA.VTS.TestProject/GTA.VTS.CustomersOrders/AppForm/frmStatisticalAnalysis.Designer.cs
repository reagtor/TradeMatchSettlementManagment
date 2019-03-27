namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class frmStatisticalAnalysis
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStatisticalAnalysis));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbHistoryTradeAnalysis = new System.Windows.Forms.GroupBox();
            this.labMessage = new System.Windows.Forms.Label();
            this.dgvCodeListAnalysis = new System.Windows.Forms.DataGridView();
            this.dgvcL_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_OpenCloseType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_Dealed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_Removed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_FloatProfitLoss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_MarketProfitLoss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_Buy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvL_Sell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnQueryHistoryAnalysis = new System.Windows.Forms.Button();
            this.labEnd = new System.Windows.Forms.Label();
            this.labStart = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dgvHistoryTradeAnals = new System.Windows.Forms.DataGridView();
            this.dgvTxtCH_BreedClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtCH_Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtCH_Dealed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtCH_Canceled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtCH_Removed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtCH_PartRemoved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtCH_MarketProfitLoss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtCH_FloatProfitLoss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnTodayQueryAnalysis = new System.Windows.Forms.Button();
            this.gbTodayTradeAnalysis = new System.Windows.Forms.GroupBox();
            this.dgvTodayTradeAnals = new System.Windows.Forms.DataGridView();
            this.dgvTxtC_BreedClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_Dealed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_PartDealed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_Canceled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_Removed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_RequiredRemoveSoon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_PartRemoved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_IsRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_PartDealRemoveSoon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_RequiredSoon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_UnRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtC_None = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.gbHistoryTradeAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeListAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryTradeAnals)).BeginInit();
            this.gbTodayTradeAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodayTradeAnals)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.gbHistoryTradeAnalysis);
            this.panel1.Controls.Add(this.btnTodayQueryAnalysis);
            this.panel1.Controls.Add(this.gbTodayTradeAnalysis);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1031, 646);
            this.panel1.TabIndex = 0;
            // 
            // gbHistoryTradeAnalysis
            // 
            this.gbHistoryTradeAnalysis.Controls.Add(this.labMessage);
            this.gbHistoryTradeAnalysis.Controls.Add(this.dgvCodeListAnalysis);
            this.gbHistoryTradeAnalysis.Controls.Add(this.btnQueryHistoryAnalysis);
            this.gbHistoryTradeAnalysis.Controls.Add(this.labEnd);
            this.gbHistoryTradeAnalysis.Controls.Add(this.labStart);
            this.gbHistoryTradeAnalysis.Controls.Add(this.dtpEndDate);
            this.gbHistoryTradeAnalysis.Controls.Add(this.dtpStartDate);
            this.gbHistoryTradeAnalysis.Controls.Add(this.dgvHistoryTradeAnals);
            this.gbHistoryTradeAnalysis.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbHistoryTradeAnalysis.Location = new System.Drawing.Point(0, 182);
            this.gbHistoryTradeAnalysis.Name = "gbHistoryTradeAnalysis";
            this.gbHistoryTradeAnalysis.Size = new System.Drawing.Size(1031, 447);
            this.gbHistoryTradeAnalysis.TabIndex = 5;
            this.gbHistoryTradeAnalysis.TabStop = false;
            this.gbHistoryTradeAnalysis.Text = "历史交易统计分析";
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Location = new System.Drawing.Point(416, 409);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(0, 12);
            this.labMessage.TabIndex = 9;
            // 
            // dgvCodeListAnalysis
            // 
            this.dgvCodeListAnalysis.AllowUserToAddRows = false;
            this.dgvCodeListAnalysis.AllowUserToDeleteRows = false;
            this.dgvCodeListAnalysis.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgvCodeListAnalysis.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCodeListAnalysis.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dgvCodeListAnalysis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCodeListAnalysis.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcL_Code,
            this.dgvL_OpenCloseType,
            this.dgvL_Count,
            this.dgvL_Dealed,
            this.dgvL_Removed,
            this.dgvL_FloatProfitLoss,
            this.dgvL_MarketProfitLoss,
            this.dgvL_Buy,
            this.dgvL_Sell});
            this.dgvCodeListAnalysis.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvCodeListAnalysis.Location = new System.Drawing.Point(3, 170);
            this.dgvCodeListAnalysis.MultiSelect = false;
            this.dgvCodeListAnalysis.Name = "dgvCodeListAnalysis";
            this.dgvCodeListAnalysis.ReadOnly = true;
            this.dgvCodeListAnalysis.RowHeadersWidth = 51;
            this.dgvCodeListAnalysis.RowTemplate.Height = 23;
            this.dgvCodeListAnalysis.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCodeListAnalysis.Size = new System.Drawing.Size(1025, 225);
            this.dgvCodeListAnalysis.TabIndex = 8;
            this.dgvCodeListAnalysis.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvCodeListAnalysis_RowPostPaint);
            // 
            // dgvcL_Code
            // 
            this.dgvcL_Code.DataPropertyName = "BreedClassName";
            this.dgvcL_Code.HeaderText = "代码";
            this.dgvcL_Code.Name = "dgvcL_Code";
            this.dgvcL_Code.ReadOnly = true;
            // 
            // dgvL_OpenCloseType
            // 
            this.dgvL_OpenCloseType.DataPropertyName = "None";
            this.dgvL_OpenCloseType.HeaderText = "开平类型";
            this.dgvL_OpenCloseType.Name = "dgvL_OpenCloseType";
            this.dgvL_OpenCloseType.ReadOnly = true;
            // 
            // dgvL_Count
            // 
            this.dgvL_Count.DataPropertyName = "Count";
            this.dgvL_Count.HeaderText = "委托总数";
            this.dgvL_Count.Name = "dgvL_Count";
            this.dgvL_Count.ReadOnly = true;
            // 
            // dgvL_Dealed
            // 
            this.dgvL_Dealed.DataPropertyName = "Dealed";
            this.dgvL_Dealed.HeaderText = "成交量";
            this.dgvL_Dealed.Name = "dgvL_Dealed";
            this.dgvL_Dealed.ReadOnly = true;
            // 
            // dgvL_Removed
            // 
            this.dgvL_Removed.DataPropertyName = "Removed";
            this.dgvL_Removed.HeaderText = "撤单量";
            this.dgvL_Removed.Name = "dgvL_Removed";
            this.dgvL_Removed.ReadOnly = true;
            // 
            // dgvL_FloatProfitLoss
            // 
            this.dgvL_FloatProfitLoss.DataPropertyName = "FloatProfitLoss";
            this.dgvL_FloatProfitLoss.HeaderText = "浮动盈亏";
            this.dgvL_FloatProfitLoss.Name = "dgvL_FloatProfitLoss";
            this.dgvL_FloatProfitLoss.ReadOnly = true;
            // 
            // dgvL_MarketProfitLoss
            // 
            this.dgvL_MarketProfitLoss.DataPropertyName = "MarketProfitLoss";
            this.dgvL_MarketProfitLoss.HeaderText = "盯市盈亏";
            this.dgvL_MarketProfitLoss.Name = "dgvL_MarketProfitLoss";
            this.dgvL_MarketProfitLoss.ReadOnly = true;
            this.dgvL_MarketProfitLoss.Width = 110;
            // 
            // dgvL_Buy
            // 
            this.dgvL_Buy.DataPropertyName = "PartRemoved";
            this.dgvL_Buy.HeaderText = "买笔数";
            this.dgvL_Buy.Name = "dgvL_Buy";
            this.dgvL_Buy.ReadOnly = true;
            // 
            // dgvL_Sell
            // 
            this.dgvL_Sell.DataPropertyName = "Canceled";
            this.dgvL_Sell.HeaderText = "卖笔数";
            this.dgvL_Sell.Name = "dgvL_Sell";
            this.dgvL_Sell.ReadOnly = true;
            // 
            // btnQueryHistoryAnalysis
            // 
            this.btnQueryHistoryAnalysis.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnQueryHistoryAnalysis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnQueryHistoryAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnQueryHistoryAnalysis.Location = new System.Drawing.Point(331, 399);
            this.btnQueryHistoryAnalysis.Name = "btnQueryHistoryAnalysis";
            this.btnQueryHistoryAnalysis.Size = new System.Drawing.Size(75, 23);
            this.btnQueryHistoryAnalysis.TabIndex = 7;
            this.btnQueryHistoryAnalysis.Text = "历史统计";
            this.btnQueryHistoryAnalysis.UseVisualStyleBackColor = true;
            this.btnQueryHistoryAnalysis.Click += new System.EventHandler(this.btnQueryHistoryAnalysis_Click);
            // 
            // labEnd
            // 
            this.labEnd.AutoSize = true;
            this.labEnd.Location = new System.Drawing.Point(161, 406);
            this.labEnd.Name = "labEnd";
            this.labEnd.Size = new System.Drawing.Size(17, 12);
            this.labEnd.TabIndex = 6;
            this.labEnd.Text = "至";
            // 
            // labStart
            // 
            this.labStart.AutoSize = true;
            this.labStart.Location = new System.Drawing.Point(4, 406);
            this.labStart.Name = "labStart";
            this.labStart.Size = new System.Drawing.Size(29, 12);
            this.labStart.TabIndex = 5;
            this.labStart.Text = "开始";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(198, 400);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(108, 21);
            this.dtpEndDate.TabIndex = 4;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(47, 400);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(110, 21);
            this.dtpStartDate.TabIndex = 3;
            // 
            // dgvHistoryTradeAnals
            // 
            this.dgvHistoryTradeAnals.AllowUserToAddRows = false;
            this.dgvHistoryTradeAnals.AllowUserToDeleteRows = false;
            this.dgvHistoryTradeAnals.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgvHistoryTradeAnals.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvHistoryTradeAnals.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dgvHistoryTradeAnals.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvHistoryTradeAnals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistoryTradeAnals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTxtCH_BreedClassName,
            this.dgvTxtCH_Count,
            this.dgvTxtCH_Dealed,
            this.dgvTxtCH_Canceled,
            this.dgvTxtCH_Removed,
            this.dgvTxtCH_PartRemoved,
            this.dgvTxtCH_MarketProfitLoss,
            this.dgvTxtCH_FloatProfitLoss});
            this.dgvHistoryTradeAnals.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvHistoryTradeAnals.Location = new System.Drawing.Point(3, 17);
            this.dgvHistoryTradeAnals.MultiSelect = false;
            this.dgvHistoryTradeAnals.Name = "dgvHistoryTradeAnals";
            this.dgvHistoryTradeAnals.ReadOnly = true;
            this.dgvHistoryTradeAnals.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Info;
            this.dgvHistoryTradeAnals.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvHistoryTradeAnals.RowTemplate.Height = 23;
            this.dgvHistoryTradeAnals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistoryTradeAnals.Size = new System.Drawing.Size(1025, 153);
            this.dgvHistoryTradeAnals.TabIndex = 0;
            this.dgvHistoryTradeAnals.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistoryTradeAnals_CellClick);
            // 
            // dgvTxtCH_BreedClassName
            // 
            this.dgvTxtCH_BreedClassName.DataPropertyName = "BreedClassName";
            this.dgvTxtCH_BreedClassName.HeaderText = "品种类别";
            this.dgvTxtCH_BreedClassName.Name = "dgvTxtCH_BreedClassName";
            this.dgvTxtCH_BreedClassName.ReadOnly = true;
            // 
            // dgvTxtCH_Count
            // 
            this.dgvTxtCH_Count.DataPropertyName = "Count";
            this.dgvTxtCH_Count.HeaderText = "总委托量";
            this.dgvTxtCH_Count.Name = "dgvTxtCH_Count";
            this.dgvTxtCH_Count.ReadOnly = true;
            // 
            // dgvTxtCH_Dealed
            // 
            this.dgvTxtCH_Dealed.DataPropertyName = "Dealed";
            this.dgvTxtCH_Dealed.HeaderText = "已成";
            this.dgvTxtCH_Dealed.Name = "dgvTxtCH_Dealed";
            this.dgvTxtCH_Dealed.ReadOnly = true;
            // 
            // dgvTxtCH_Canceled
            // 
            this.dgvTxtCH_Canceled.DataPropertyName = "Canceled";
            this.dgvTxtCH_Canceled.HeaderText = "废单";
            this.dgvTxtCH_Canceled.Name = "dgvTxtCH_Canceled";
            this.dgvTxtCH_Canceled.ReadOnly = true;
            // 
            // dgvTxtCH_Removed
            // 
            this.dgvTxtCH_Removed.DataPropertyName = "Removed";
            this.dgvTxtCH_Removed.HeaderText = "已撤";
            this.dgvTxtCH_Removed.Name = "dgvTxtCH_Removed";
            this.dgvTxtCH_Removed.ReadOnly = true;
            // 
            // dgvTxtCH_PartRemoved
            // 
            this.dgvTxtCH_PartRemoved.DataPropertyName = "PartRemoved";
            this.dgvTxtCH_PartRemoved.HeaderText = "部撤";
            this.dgvTxtCH_PartRemoved.Name = "dgvTxtCH_PartRemoved";
            this.dgvTxtCH_PartRemoved.ReadOnly = true;
            // 
            // dgvTxtCH_MarketProfitLoss
            // 
            this.dgvTxtCH_MarketProfitLoss.DataPropertyName = "MarketProfitLoss";
            this.dgvTxtCH_MarketProfitLoss.HeaderText = "盯市盈亏";
            this.dgvTxtCH_MarketProfitLoss.Name = "dgvTxtCH_MarketProfitLoss";
            this.dgvTxtCH_MarketProfitLoss.ReadOnly = true;
            this.dgvTxtCH_MarketProfitLoss.Width = 130;
            // 
            // dgvTxtCH_FloatProfitLoss
            // 
            this.dgvTxtCH_FloatProfitLoss.DataPropertyName = "FloatProfitLoss";
            this.dgvTxtCH_FloatProfitLoss.HeaderText = "浮动盈亏";
            this.dgvTxtCH_FloatProfitLoss.Name = "dgvTxtCH_FloatProfitLoss";
            this.dgvTxtCH_FloatProfitLoss.ReadOnly = true;
            // 
            // btnTodayQueryAnalysis
            // 
            this.btnTodayQueryAnalysis.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTodayQueryAnalysis.Location = new System.Drawing.Point(0, 158);
            this.btnTodayQueryAnalysis.Name = "btnTodayQueryAnalysis";
            this.btnTodayQueryAnalysis.Size = new System.Drawing.Size(1031, 24);
            this.btnTodayQueryAnalysis.TabIndex = 4;
            this.btnTodayQueryAnalysis.Text = "统计分析";
            this.btnTodayQueryAnalysis.UseVisualStyleBackColor = true;
            this.btnTodayQueryAnalysis.Click += new System.EventHandler(this.btnQueryAnalysis_Click);
            // 
            // gbTodayTradeAnalysis
            // 
            this.gbTodayTradeAnalysis.Controls.Add(this.dgvTodayTradeAnals);
            this.gbTodayTradeAnalysis.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTodayTradeAnalysis.Location = new System.Drawing.Point(0, 0);
            this.gbTodayTradeAnalysis.Name = "gbTodayTradeAnalysis";
            this.gbTodayTradeAnalysis.Size = new System.Drawing.Size(1031, 158);
            this.gbTodayTradeAnalysis.TabIndex = 0;
            this.gbTodayTradeAnalysis.TabStop = false;
            this.gbTodayTradeAnalysis.Text = "今日委托交易统计";
            // 
            // dgvTodayTradeAnals
            // 
            this.dgvTodayTradeAnals.AllowUserToAddRows = false;
            this.dgvTodayTradeAnals.AllowUserToDeleteRows = false;
            this.dgvTodayTradeAnals.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgvTodayTradeAnals.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTodayTradeAnals.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dgvTodayTradeAnals.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTodayTradeAnals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTodayTradeAnals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTxtC_BreedClassName,
            this.dgvTxtC_Count,
            this.dgvTxtC_Dealed,
            this.dgvTxtC_PartDealed,
            this.dgvTxtC_Canceled,
            this.dgvTxtC_Removed,
            this.dgvTxtC_RequiredRemoveSoon,
            this.dgvTxtC_PartRemoved,
            this.dgvTxtC_IsRequired,
            this.dgvTxtC_PartDealRemoveSoon,
            this.dgvTxtC_RequiredSoon,
            this.dgvTxtC_UnRequired,
            this.dgvTxtC_None});
            this.dgvTodayTradeAnals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTodayTradeAnals.Location = new System.Drawing.Point(3, 17);
            this.dgvTodayTradeAnals.MultiSelect = false;
            this.dgvTodayTradeAnals.Name = "dgvTodayTradeAnals";
            this.dgvTodayTradeAnals.ReadOnly = true;
            this.dgvTodayTradeAnals.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Info;
            this.dgvTodayTradeAnals.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTodayTradeAnals.RowTemplate.Height = 23;
            this.dgvTodayTradeAnals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTodayTradeAnals.Size = new System.Drawing.Size(1025, 138);
            this.dgvTodayTradeAnals.TabIndex = 0;
            // 
            // dgvTxtC_BreedClassName
            // 
            this.dgvTxtC_BreedClassName.DataPropertyName = "BreedClassName";
            this.dgvTxtC_BreedClassName.HeaderText = "品种类别";
            this.dgvTxtC_BreedClassName.Name = "dgvTxtC_BreedClassName";
            this.dgvTxtC_BreedClassName.ReadOnly = true;
            // 
            // dgvTxtC_Count
            // 
            this.dgvTxtC_Count.DataPropertyName = "Count";
            this.dgvTxtC_Count.HeaderText = "总委托量";
            this.dgvTxtC_Count.Name = "dgvTxtC_Count";
            this.dgvTxtC_Count.ReadOnly = true;
            // 
            // dgvTxtC_Dealed
            // 
            this.dgvTxtC_Dealed.DataPropertyName = "Dealed";
            this.dgvTxtC_Dealed.HeaderText = "已成";
            this.dgvTxtC_Dealed.Name = "dgvTxtC_Dealed";
            this.dgvTxtC_Dealed.ReadOnly = true;
            // 
            // dgvTxtC_PartDealed
            // 
            this.dgvTxtC_PartDealed.DataPropertyName = "PartDealed";
            this.dgvTxtC_PartDealed.HeaderText = "部成";
            this.dgvTxtC_PartDealed.Name = "dgvTxtC_PartDealed";
            this.dgvTxtC_PartDealed.ReadOnly = true;
            // 
            // dgvTxtC_Canceled
            // 
            this.dgvTxtC_Canceled.DataPropertyName = "Canceled";
            this.dgvTxtC_Canceled.HeaderText = "废单";
            this.dgvTxtC_Canceled.Name = "dgvTxtC_Canceled";
            this.dgvTxtC_Canceled.ReadOnly = true;
            // 
            // dgvTxtC_Removed
            // 
            this.dgvTxtC_Removed.DataPropertyName = "Removed";
            this.dgvTxtC_Removed.HeaderText = "已撤";
            this.dgvTxtC_Removed.Name = "dgvTxtC_Removed";
            this.dgvTxtC_Removed.ReadOnly = true;
            // 
            // dgvTxtC_RequiredRemoveSoon
            // 
            this.dgvTxtC_RequiredRemoveSoon.DataPropertyName = "RequiredRemoveSoon";
            this.dgvTxtC_RequiredRemoveSoon.HeaderText = "已报待撤";
            this.dgvTxtC_RequiredRemoveSoon.Name = "dgvTxtC_RequiredRemoveSoon";
            this.dgvTxtC_RequiredRemoveSoon.ReadOnly = true;
            // 
            // dgvTxtC_PartRemoved
            // 
            this.dgvTxtC_PartRemoved.DataPropertyName = "PartRemoved";
            this.dgvTxtC_PartRemoved.HeaderText = "部撤";
            this.dgvTxtC_PartRemoved.Name = "dgvTxtC_PartRemoved";
            this.dgvTxtC_PartRemoved.ReadOnly = true;
            // 
            // dgvTxtC_IsRequired
            // 
            this.dgvTxtC_IsRequired.DataPropertyName = "IsRequired";
            this.dgvTxtC_IsRequired.HeaderText = "已报";
            this.dgvTxtC_IsRequired.Name = "dgvTxtC_IsRequired";
            this.dgvTxtC_IsRequired.ReadOnly = true;
            // 
            // dgvTxtC_PartDealRemoveSoon
            // 
            this.dgvTxtC_PartDealRemoveSoon.DataPropertyName = "PartDealRemoveSoon";
            this.dgvTxtC_PartDealRemoveSoon.HeaderText = "部成待撤";
            this.dgvTxtC_PartDealRemoveSoon.Name = "dgvTxtC_PartDealRemoveSoon";
            this.dgvTxtC_PartDealRemoveSoon.ReadOnly = true;
            this.dgvTxtC_PartDealRemoveSoon.Width = 130;
            // 
            // dgvTxtC_RequiredSoon
            // 
            this.dgvTxtC_RequiredSoon.DataPropertyName = "RequiredSoon";
            this.dgvTxtC_RequiredSoon.HeaderText = "待报";
            this.dgvTxtC_RequiredSoon.Name = "dgvTxtC_RequiredSoon";
            this.dgvTxtC_RequiredSoon.ReadOnly = true;
            this.dgvTxtC_RequiredSoon.Width = 130;
            // 
            // dgvTxtC_UnRequired
            // 
            this.dgvTxtC_UnRequired.DataPropertyName = "UnRequired";
            this.dgvTxtC_UnRequired.HeaderText = "未报";
            this.dgvTxtC_UnRequired.Name = "dgvTxtC_UnRequired";
            this.dgvTxtC_UnRequired.ReadOnly = true;
            // 
            // dgvTxtC_None
            // 
            this.dgvTxtC_None.DataPropertyName = "None";
            this.dgvTxtC_None.HeaderText = "无状态";
            this.dgvTxtC_None.Name = "dgvTxtC_None";
            this.dgvTxtC_None.ReadOnly = true;
            // 
            // frmStatisticalAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1031, 646);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmStatisticalAnalysis";
            this.Text = "交易统计分析";
            this.Load += new System.EventHandler(this.frmTodayStatisticalAnalysis_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStatisticalAnalysis_FormClosing);
            this.panel1.ResumeLayout(false);
            this.gbHistoryTradeAnalysis.ResumeLayout(false);
            this.gbHistoryTradeAnalysis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeListAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryTradeAnals)).EndInit();
            this.gbTodayTradeAnalysis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodayTradeAnals)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbTodayTradeAnalysis;
        private System.Windows.Forms.DataGridView dgvTodayTradeAnals;
        private System.Windows.Forms.Button btnTodayQueryAnalysis;
        private System.Windows.Forms.GroupBox gbHistoryTradeAnalysis;

        private System.Windows.Forms.DataGridView dgvHistoryTradeAnals;
        private System.Windows.Forms.Label labEnd;
        private System.Windows.Forms.Label labStart;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Button btnQueryHistoryAnalysis;
        private System.Windows.Forms.DataGridView dgvCodeListAnalysis;
        private System.Windows.Forms.Label labMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_BreedClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_Dealed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_PartDealed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_Canceled;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_Removed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_RequiredRemoveSoon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_PartRemoved;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_IsRequired;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_PartDealRemoveSoon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_RequiredSoon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_UnRequired;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtC_None;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_BreedClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_Dealed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_Canceled;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_Removed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_PartRemoved;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_MarketProfitLoss;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtCH_FloatProfitLoss;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcL_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_OpenCloseType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_Dealed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_Removed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_FloatProfitLoss;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_MarketProfitLoss;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_Buy;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvL_Sell;

    }
}