namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    partial class MinVolumeOfBusinessManageUI
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gdMinVolumeOfBusResult = new DevExpress.XtraGrid.GridControl();
            this.gdvMinVolumeOfBusSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcTradeWayID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlTradeWayID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcBreedClassID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBreedClassID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcUnitID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlUnitID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcVolumeOfBusiness = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtBreedClassName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.txtVolumeOfBusiness = new DevExpress.XtraEditors.TextEdit();
            this.cmbTradeWayID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbUnitID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbBreedClassID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdMinVolumeOfBusResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvMinVolumeOfBusSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTradeWayID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUnitID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolumeOfBusiness.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeWayID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnitID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdMinVolumeOfBusResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 45);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(792, 398);
            this.panelControl1.TabIndex = 44;
            this.panelControl1.Text = "panelControl1";
            // 
            // gdMinVolumeOfBusResult
            // 
            this.gdMinVolumeOfBusResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdMinVolumeOfBusResult.EmbeddedNavigator.Name = "";
            this.gdMinVolumeOfBusResult.Location = new System.Drawing.Point(2, 2);
            this.gdMinVolumeOfBusResult.MainView = this.gdvMinVolumeOfBusSelect;
            this.gdMinVolumeOfBusResult.Name = "gdMinVolumeOfBusResult";
            this.gdMinVolumeOfBusResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlTradeWayID,
            this.ddlBreedClassID,
            this.ddlUnitID});
            this.gdMinVolumeOfBusResult.Size = new System.Drawing.Size(788, 394);
            this.gdMinVolumeOfBusResult.TabIndex = 0;
            this.gdMinVolumeOfBusResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvMinVolumeOfBusSelect});
            this.gdMinVolumeOfBusResult.DoubleClick += new System.EventHandler(this.gdMinVolumeOfBusResult_DoubleClick);
            // 
            // gdvMinVolumeOfBusSelect
            // 
            this.gdvMinVolumeOfBusSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcTradeWayID,
            this.gdcBreedClassID,
            this.gdcUnitID,
            this.gdcVolumeOfBusiness});
            this.gdvMinVolumeOfBusSelect.GridControl = this.gdMinVolumeOfBusResult;
            this.gdvMinVolumeOfBusSelect.Name = "gdvMinVolumeOfBusSelect";
            this.gdvMinVolumeOfBusSelect.OptionsBehavior.Editable = false;
            this.gdvMinVolumeOfBusSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcTradeWayID
            // 
            this.gdcTradeWayID.Caption = "交易方向";
            this.gdcTradeWayID.ColumnEdit = this.ddlTradeWayID;
            this.gdcTradeWayID.FieldName = "TradeWayID";
            this.gdcTradeWayID.Name = "gdcTradeWayID";
            this.gdcTradeWayID.Visible = true;
            this.gdcTradeWayID.VisibleIndex = 1;
            this.gdcTradeWayID.Width = 115;
            // 
            // ddlTradeWayID
            // 
            this.ddlTradeWayID.AutoHeight = false;
            this.ddlTradeWayID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlTradeWayID.Name = "ddlTradeWayID";
            this.ddlTradeWayID.NullText = "";
            // 
            // gdcBreedClassID
            // 
            this.gdcBreedClassID.Caption = "品种名称";
            this.gdcBreedClassID.ColumnEdit = this.ddlBreedClassID;
            this.gdcBreedClassID.FieldName = "BreedClassID";
            this.gdcBreedClassID.Name = "gdcBreedClassID";
            this.gdcBreedClassID.Visible = true;
            this.gdcBreedClassID.VisibleIndex = 0;
            this.gdcBreedClassID.Width = 153;
            // 
            // ddlBreedClassID
            // 
            this.ddlBreedClassID.AutoHeight = false;
            this.ddlBreedClassID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBreedClassID.Name = "ddlBreedClassID";
            this.ddlBreedClassID.NullText = "";
            // 
            // gdcUnitID
            // 
            this.gdcUnitID.Caption = "交易单位";
            this.gdcUnitID.ColumnEdit = this.ddlUnitID;
            this.gdcUnitID.FieldName = "UnitID";
            this.gdcUnitID.Name = "gdcUnitID";
            this.gdcUnitID.Visible = true;
            this.gdcUnitID.VisibleIndex = 3;
            this.gdcUnitID.Width = 128;
            // 
            // ddlUnitID
            // 
            this.ddlUnitID.AutoHeight = false;
            this.ddlUnitID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlUnitID.Name = "ddlUnitID";
            this.ddlUnitID.NullText = "";
            // 
            // gdcVolumeOfBusiness
            // 
            this.gdcVolumeOfBusiness.Caption = "最小交易量";
            this.gdcVolumeOfBusiness.FieldName = "VolumeOfBusiness";
            this.gdcVolumeOfBusiness.Name = "gdcVolumeOfBusiness";
            this.gdcVolumeOfBusiness.Visible = true;
            this.gdcVolumeOfBusiness.VisibleIndex = 2;
            this.gdcVolumeOfBusiness.Width = 93;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.txtBreedClassName);
            this.panelControl3.Controls.Add(this.labelControl9);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.UCPageNavig);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(792, 45);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
            // 
            // txtBreedClassName
            // 
            this.txtBreedClassName.Location = new System.Drawing.Point(70, 9);
            this.txtBreedClassName.Name = "txtBreedClassName";
            this.txtBreedClassName.Size = new System.Drawing.Size(100, 21);
            this.txtBreedClassName.TabIndex = 0;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(12, 12);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(52, 14);
            this.labelControl9.TabIndex = 16;
            this.labelControl9.Text = "品种名称:";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(176, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // UCPageNavig
            // 
            this.UCPageNavig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UCPageNavig.CurrentPage = 0;
            this.UCPageNavig.Location = new System.Drawing.Point(468, 9);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 2;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.panel1);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(0, 443);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(792, 123);
            this.panelControl4.TabIndex = 1;
            this.panelControl4.Text = "panelControl4";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.labelControl14);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.txtVolumeOfBusiness);
            this.panel1.Controls.Add(this.cmbTradeWayID);
            this.panel1.Controls.Add(this.cmbUnitID);
            this.panel1.Controls.Add(this.cmbBreedClassID);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Location = new System.Drawing.Point(5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 112);
            this.panel1.TabIndex = 13;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(707, 82);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(68, 23);
            this.btnOK.TabIndex = 25;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.Location = new System.Drawing.Point(564, 20);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(7, 14);
            this.labelControl14.TabIndex = 24;
            this.labelControl14.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(707, 51);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(635, 82);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(561, 82);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(68, 23);
            this.btnModify.TabIndex = 5;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(489, 82);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtVolumeOfBusiness
            // 
            this.txtVolumeOfBusiness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVolumeOfBusiness.Location = new System.Drawing.Point(458, 13);
            this.txtVolumeOfBusiness.Name = "txtVolumeOfBusiness";
            this.txtVolumeOfBusiness.Size = new System.Drawing.Size(100, 21);
            this.txtVolumeOfBusiness.TabIndex = 2;
            // 
            // cmbTradeWayID
            // 
            this.cmbTradeWayID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTradeWayID.Location = new System.Drawing.Point(266, 48);
            this.cmbTradeWayID.Name = "cmbTradeWayID";
            this.cmbTradeWayID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTradeWayID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbTradeWayID.Size = new System.Drawing.Size(100, 21);
            this.cmbTradeWayID.TabIndex = 1;
            // 
            // cmbUnitID
            // 
            this.cmbUnitID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUnitID.Location = new System.Drawing.Point(458, 48);
            this.cmbUnitID.Name = "cmbUnitID";
            this.cmbUnitID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUnitID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbUnitID.Size = new System.Drawing.Size(100, 21);
            this.cmbUnitID.TabIndex = 3;
            // 
            // cmbBreedClassID
            // 
            this.cmbBreedClassID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBreedClassID.Location = new System.Drawing.Point(266, 13);
            this.cmbBreedClassID.Name = "cmbBreedClassID";
            this.cmbBreedClassID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBreedClassID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBreedClassID.Size = new System.Drawing.Size(100, 21);
            this.cmbBreedClassID.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl4.Location = new System.Drawing.Point(208, 51);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(52, 14);
            this.labelControl4.TabIndex = 19;
            this.labelControl4.Text = "交易方向:";
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Location = new System.Drawing.Point(388, 51);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 14);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "交易单位:";
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(388, 16);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(64, 14);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "最小交易量:";
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Location = new System.Drawing.Point(208, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "品种名称:";
            // 
            // MinVolumeOfBusinessManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl4);
            this.Name = "MinVolumeOfBusinessManageUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "最小交易单位管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MinVolumeOfBusinessManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdMinVolumeOfBusResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvMinVolumeOfBusSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTradeWayID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUnitID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolumeOfBusiness.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeWayID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnitID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gdMinVolumeOfBusResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvMinVolumeOfBusSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTradeWayID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcUnitID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcVolumeOfBusiness;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlTradeWayID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBreedClassID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlUnitID;
        private DevExpress.XtraEditors.TextEdit txtBreedClassName;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.TextEdit txtVolumeOfBusiness;
        private DevExpress.XtraEditors.ComboBoxEdit cmbTradeWayID;
        private DevExpress.XtraEditors.ComboBoxEdit cmbUnitID;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBreedClassID;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}