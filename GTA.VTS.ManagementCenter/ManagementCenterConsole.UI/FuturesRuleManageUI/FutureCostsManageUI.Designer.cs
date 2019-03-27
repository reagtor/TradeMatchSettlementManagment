namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    partial class FutureCostsManageUI
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
            this.gdFutureCostsResult = new DevExpress.XtraGrid.GridControl();
            this.gdvFutureCostsSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBreedClassID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBreedClassID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcCost = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcCurrencyTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlCurrencyTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcTurnoverRateOfSerCha = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcCostType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlCostType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtBreedClassID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labCostTypeUnit = new DevExpress.XtraEditors.LabelControl();
            this.txtCost = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.cmbCostType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.cmbCurrencyTypeID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbBreedClassID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gdFutureCostsResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvFutureCostsSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCurrencyTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCostType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCostType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyTypeID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gdFutureCostsResult
            // 
            this.gdFutureCostsResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdFutureCostsResult.EmbeddedNavigator.Name = "";
            this.gdFutureCostsResult.Location = new System.Drawing.Point(2, 2);
            this.gdFutureCostsResult.MainView = this.gdvFutureCostsSelect;
            this.gdFutureCostsResult.Name = "gdFutureCostsResult";
            this.gdFutureCostsResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBreedClassID,
            this.ddlCurrencyTypeID,
            this.ddlCostType});
            this.gdFutureCostsResult.Size = new System.Drawing.Size(788, 388);
            this.gdFutureCostsResult.TabIndex = 0;
            this.gdFutureCostsResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvFutureCostsSelect});
            this.gdFutureCostsResult.DoubleClick += new System.EventHandler(this.gdFutureCostsResult_DoubleClick);
            // 
            // gdvFutureCostsSelect
            // 
            this.gdvFutureCostsSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBreedClassID,
            this.gdcCost,
            this.gdcCurrencyTypeID,
            this.gdcTurnoverRateOfSerCha,
            this.gdcCostType});
            this.gdvFutureCostsSelect.GridControl = this.gdFutureCostsResult;
            this.gdvFutureCostsSelect.Name = "gdvFutureCostsSelect";
            this.gdvFutureCostsSelect.OptionsBehavior.Editable = false;
            this.gdvFutureCostsSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcBreedClassID
            // 
            this.gdcBreedClassID.Caption = "品种名称";
            this.gdcBreedClassID.ColumnEdit = this.ddlBreedClassID;
            this.gdcBreedClassID.FieldName = "BreedClassID";
            this.gdcBreedClassID.Name = "gdcBreedClassID";
            this.gdcBreedClassID.Visible = true;
            this.gdcBreedClassID.VisibleIndex = 0;
            this.gdcBreedClassID.Width = 85;
            // 
            // ddlBreedClassID
            // 
            this.ddlBreedClassID.AutoHeight = false;
            this.ddlBreedClassID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBreedClassID.Name = "ddlBreedClassID";
            this.ddlBreedClassID.NullText = "";
            // 
            // gdcCost
            // 
            this.gdcCost.Caption = "手续费(元/手)或(%)";
            this.gdcCost.FieldName = "TurnoverRateOfServiceCharge";
            this.gdcCost.Name = "gdcCost";
            this.gdcCost.Visible = true;
            this.gdcCost.VisibleIndex = 3;
            this.gdcCost.Width = 80;
            // 
            // gdcCurrencyTypeID
            // 
            this.gdcCurrencyTypeID.Caption = "交易货币类型";
            this.gdcCurrencyTypeID.ColumnEdit = this.ddlCurrencyTypeID;
            this.gdcCurrencyTypeID.FieldName = "CurrencyTypeID";
            this.gdcCurrencyTypeID.Name = "gdcCurrencyTypeID";
            this.gdcCurrencyTypeID.Visible = true;
            this.gdcCurrencyTypeID.VisibleIndex = 1;
            this.gdcCurrencyTypeID.Width = 77;
            // 
            // ddlCurrencyTypeID
            // 
            this.ddlCurrencyTypeID.AutoHeight = false;
            this.ddlCurrencyTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlCurrencyTypeID.Name = "ddlCurrencyTypeID";
            this.ddlCurrencyTypeID.NullText = "";
            // 
            // gdcTurnoverRateOfSerCha
            // 
            this.gdcTurnoverRateOfSerCha.Caption = "成交金额手续费(%)";
            this.gdcTurnoverRateOfSerCha.FieldName = "TurnoverRateOfServiceCharge";
            this.gdcTurnoverRateOfSerCha.Name = "gdcTurnoverRateOfSerCha";
            this.gdcTurnoverRateOfSerCha.Width = 93;
            // 
            // gdcCostType
            // 
            this.gdcCostType.Caption = "手续费类型";
            this.gdcCostType.ColumnEdit = this.ddlCostType;
            this.gdcCostType.FieldName = "CostType";
            this.gdcCostType.Name = "gdcCostType";
            this.gdcCostType.Visible = true;
            this.gdcCostType.VisibleIndex = 2;
            // 
            // ddlCostType
            // 
            this.ddlCostType.AutoHeight = false;
            this.ddlCostType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlCostType.Name = "ddlCostType";
            this.ddlCostType.NullText = "";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdFutureCostsResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 39);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(792, 392);
            this.panelControl1.TabIndex = 38;
            this.panelControl1.Text = "panelControl1";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.txtBreedClassID);
            this.panelControl3.Controls.Add(this.labelControl9);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.UCPageNavig);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(792, 39);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
            // 
            // txtBreedClassID
            // 
            this.txtBreedClassID.Location = new System.Drawing.Point(71, 10);
            this.txtBreedClassID.Name = "txtBreedClassID";
            this.txtBreedClassID.Size = new System.Drawing.Size(100, 21);
            this.txtBreedClassID.TabIndex = 0;
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
            this.btnQuery.Location = new System.Drawing.Point(177, 9);
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
            this.UCPageNavig.Location = new System.Drawing.Point(461, 5);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 2;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panel1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 431);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(792, 135);
            this.panelControl2.TabIndex = 2;
            this.panelControl2.Text = "panelControl2";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.labelControl21);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.labCostTypeUnit);
            this.panel1.Controls.Add(this.txtCost);
            this.panel1.Controls.Add(this.labelControl7);
            this.panel1.Controls.Add(this.cmbCostType);
            this.panel1.Controls.Add(this.labelControl6);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.cmbCurrencyTypeID);
            this.panel1.Controls.Add(this.cmbBreedClassID);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 125);
            this.panel1.TabIndex = 47;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(716, 95);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 23);
            this.btnOK.TabIndex = 58;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelControl21
            // 
            this.labelControl21.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl21.Appearance.Options.UseForeColor = true;
            this.labelControl21.Location = new System.Drawing.Point(617, 55);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(7, 14);
            this.labelControl21.TabIndex = 57;
            this.labelControl21.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(715, 55);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labCostTypeUnit
            // 
            this.labCostTypeUnit.Location = new System.Drawing.Point(582, 53);
            this.labCostTypeUnit.Name = "labCostTypeUnit";
            this.labCostTypeUnit.Size = new System.Drawing.Size(29, 14);
            this.labCostTypeUnit.TabIndex = 55;
            this.labCostTypeUnit.Text = "元/手";
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(473, 48);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(100, 21);
            this.txtCost.TabIndex = 3;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(403, 51);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(40, 14);
            this.labelControl7.TabIndex = 53;
            this.labelControl7.Text = "手续费:";
            // 
            // cmbCostType
            // 
            this.cmbCostType.Location = new System.Drawing.Point(473, 13);
            this.cmbCostType.Name = "cmbCostType";
            this.cmbCostType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCostType.Size = new System.Drawing.Size(100, 21);
            this.cmbCostType.TabIndex = 2;
            this.cmbCostType.SelectedIndexChanged += new System.EventHandler(this.cmbCostType_SelectedIndexChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(403, 16);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(64, 14);
            this.labelControl6.TabIndex = 51;
            this.labelControl6.Text = "手续费类型:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(517, 95);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(649, 95);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(583, 95);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(60, 23);
            this.btnModify.TabIndex = 5;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // cmbCurrencyTypeID
            // 
            this.cmbCurrencyTypeID.Location = new System.Drawing.Point(264, 48);
            this.cmbCurrencyTypeID.Name = "cmbCurrencyTypeID";
            this.cmbCurrencyTypeID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCurrencyTypeID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbCurrencyTypeID.Size = new System.Drawing.Size(120, 21);
            this.cmbCurrencyTypeID.TabIndex = 1;
            // 
            // cmbBreedClassID
            // 
            this.cmbBreedClassID.Location = new System.Drawing.Point(264, 13);
            this.cmbBreedClassID.Name = "cmbBreedClassID";
            this.cmbBreedClassID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBreedClassID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBreedClassID.Size = new System.Drawing.Size(120, 21);
            this.cmbBreedClassID.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(182, 51);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(76, 14);
            this.labelControl4.TabIndex = 41;
            this.labelControl4.Text = "交易货币类型:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(182, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 38;
            this.labelControl1.Text = "品种名称:";
            // 
            // FutureCostsManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl3);
            this.MaximizeBox = false;
            this.Name = "FutureCostsManageUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "期货交易费用管理";
            this.Load += new System.EventHandler(this.FutureCostsManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdFutureCostsResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvFutureCostsSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCurrencyTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCostType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCostType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyTypeID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gdFutureCostsResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvFutureCostsSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCost;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCurrencyTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTurnoverRateOfSerCha;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.TextEdit txtBreedClassID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBreedClassID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlCurrencyTypeID;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.ComboBoxEdit cmbCurrencyTypeID;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBreedClassID;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtCost;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit cmbCostType;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labCostTypeUnit;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCostType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlCostType;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}