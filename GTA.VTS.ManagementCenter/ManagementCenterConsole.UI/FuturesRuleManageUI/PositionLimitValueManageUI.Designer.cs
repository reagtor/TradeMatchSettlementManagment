namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    partial class PositionLimitValueManageUI
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
            this.gdcStart = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtBreedClassName = new DevExpress.XtraEditors.TextEdit();
            this.cmbPositionBailTypeID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbDeliveryMonthTypeID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.gdPositionLimitValueSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBreedClassID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBreedClassID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcDeliveryMonthTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlDeliveryMonthTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcPositionValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcEnds = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcUpperLimitIfEquation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlUpperLimitIfEquation = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcLowerLimitIfEquation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlLowerLimitIfEquation = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcPositionBailTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlPositionBailTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcPositionValueTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlPositionValueTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdPositionLimitValueResult = new DevExpress.XtraGrid.GridControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPositionBailTypeID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDeliveryMonthTypeID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdPositionLimitValueSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDeliveryMonthTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUpperLimitIfEquation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlLowerLimitIfEquation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlPositionBailTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlPositionValueTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdPositionLimitValueResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gdcStart
            // 
            this.gdcStart.Caption = "起始(范围/天)";
            this.gdcStart.FieldName = "Start";
            this.gdcStart.Name = "gdcStart";
            this.gdcStart.Visible = true;
            this.gdcStart.VisibleIndex = 3;
            this.gdcStart.Width = 83;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.txtBreedClassName);
            this.panelControl3.Controls.Add(this.cmbPositionBailTypeID);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.cmbDeliveryMonthTypeID);
            this.panelControl3.Controls.Add(this.labelControl3);
            this.panelControl3.Controls.Add(this.labelControl9);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.UCPageNavig);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(792, 82);
            this.panelControl3.TabIndex = 65;
            this.panelControl3.Text = "panelControl3";
            // 
            // txtBreedClassName
            // 
            this.txtBreedClassName.Location = new System.Drawing.Point(94, 9);
            this.txtBreedClassName.Name = "txtBreedClassName";
            this.txtBreedClassName.Size = new System.Drawing.Size(135, 21);
            this.txtBreedClassName.TabIndex = 22;
            // 
            // cmbPositionBailTypeID
            // 
            this.cmbPositionBailTypeID.Location = new System.Drawing.Point(317, 9);
            this.cmbPositionBailTypeID.Name = "cmbPositionBailTypeID";
            this.cmbPositionBailTypeID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPositionBailTypeID.Size = new System.Drawing.Size(100, 21);
            this.cmbPositionBailTypeID.TabIndex = 21;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(235, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(76, 14);
            this.labelControl1.TabIndex = 20;
            this.labelControl1.Text = "持仓控制类型:";
            // 
            // cmbDeliveryMonthTypeID
            // 
            this.cmbDeliveryMonthTypeID.Location = new System.Drawing.Point(94, 45);
            this.cmbDeliveryMonthTypeID.Name = "cmbDeliveryMonthTypeID";
            this.cmbDeliveryMonthTypeID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDeliveryMonthTypeID.Size = new System.Drawing.Size(135, 21);
            this.cmbDeliveryMonthTypeID.TabIndex = 19;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 48);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(76, 14);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "交割月份类型:";
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
            this.btnQuery.Location = new System.Drawing.Point(235, 44);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(76, 23);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // UCPageNavig
            // 
            this.UCPageNavig.CurrentPage = 0;
            this.UCPageNavig.Location = new System.Drawing.Point(444, 5);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 0;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // gdPositionLimitValueSelect
            // 
            this.gdPositionLimitValueSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBreedClassID,
            this.gdcDeliveryMonthTypeID,
            this.gdcPositionValue,
            this.gdcStart,
            this.gdcEnds,
            this.gdcUpperLimitIfEquation,
            this.gdcLowerLimitIfEquation,
            this.gdcPositionBailTypeID,
            this.gdcPositionValueTypeID,
            this.gridColumn1});
            this.gdPositionLimitValueSelect.GridControl = this.gdPositionLimitValueResult;
            this.gdPositionLimitValueSelect.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gdPositionLimitValueSelect.Name = "gdPositionLimitValueSelect";
            this.gdPositionLimitValueSelect.OptionsBehavior.Editable = false;
            this.gdPositionLimitValueSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcBreedClassID
            // 
            this.gdcBreedClassID.Caption = "品种名称";
            this.gdcBreedClassID.ColumnEdit = this.ddlBreedClassID;
            this.gdcBreedClassID.FieldName = "BreedClassID";
            this.gdcBreedClassID.Name = "gdcBreedClassID";
            this.gdcBreedClassID.Visible = true;
            this.gdcBreedClassID.VisibleIndex = 0;
            this.gdcBreedClassID.Width = 71;
            // 
            // ddlBreedClassID
            // 
            this.ddlBreedClassID.AutoHeight = false;
            this.ddlBreedClassID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBreedClassID.Name = "ddlBreedClassID";
            this.ddlBreedClassID.NullText = "";
            // 
            // gdcDeliveryMonthTypeID
            // 
            this.gdcDeliveryMonthTypeID.Caption = "交割月份类型";
            this.gdcDeliveryMonthTypeID.ColumnEdit = this.ddlDeliveryMonthTypeID;
            this.gdcDeliveryMonthTypeID.FieldName = "DeliveryMonthType";
            this.gdcDeliveryMonthTypeID.Name = "gdcDeliveryMonthTypeID";
            this.gdcDeliveryMonthTypeID.Visible = true;
            this.gdcDeliveryMonthTypeID.VisibleIndex = 1;
            this.gdcDeliveryMonthTypeID.Width = 83;
            // 
            // ddlDeliveryMonthTypeID
            // 
            this.ddlDeliveryMonthTypeID.AutoHeight = false;
            this.ddlDeliveryMonthTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlDeliveryMonthTypeID.Name = "ddlDeliveryMonthTypeID";
            this.ddlDeliveryMonthTypeID.NullText = "";
            // 
            // gdcPositionValue
            // 
            this.gdcPositionValue.Caption = "持仓";
            this.gdcPositionValue.FieldName = "PositionValue";
            this.gdcPositionValue.Name = "gdcPositionValue";
            this.gdcPositionValue.Visible = true;
            this.gdcPositionValue.VisibleIndex = 8;
            this.gdcPositionValue.Width = 39;
            // 
            // gdcEnds
            // 
            this.gdcEnds.Caption = "结束(范围/天)";
            this.gdcEnds.FieldName = "Ends";
            this.gdcEnds.Name = "gdcEnds";
            this.gdcEnds.Visible = true;
            this.gdcEnds.VisibleIndex = 5;
            this.gdcEnds.Width = 82;
            // 
            // gdcUpperLimitIfEquation
            // 
            this.gdcUpperLimitIfEquation.Caption = "结束值是否相等";
            this.gdcUpperLimitIfEquation.ColumnEdit = this.ddlUpperLimitIfEquation;
            this.gdcUpperLimitIfEquation.FieldName = "UpperLimitIfEquation";
            this.gdcUpperLimitIfEquation.Name = "gdcUpperLimitIfEquation";
            this.gdcUpperLimitIfEquation.Visible = true;
            this.gdcUpperLimitIfEquation.VisibleIndex = 6;
            this.gdcUpperLimitIfEquation.Width = 81;
            // 
            // ddlUpperLimitIfEquation
            // 
            this.ddlUpperLimitIfEquation.AutoHeight = false;
            this.ddlUpperLimitIfEquation.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlUpperLimitIfEquation.Name = "ddlUpperLimitIfEquation";
            this.ddlUpperLimitIfEquation.NullText = "";
            // 
            // gdcLowerLimitIfEquation
            // 
            this.gdcLowerLimitIfEquation.Caption = "起始值是否相等";
            this.gdcLowerLimitIfEquation.ColumnEdit = this.ddlLowerLimitIfEquation;
            this.gdcLowerLimitIfEquation.FieldName = "LowerLimitIfEquation";
            this.gdcLowerLimitIfEquation.Name = "gdcLowerLimitIfEquation";
            this.gdcLowerLimitIfEquation.Visible = true;
            this.gdcLowerLimitIfEquation.VisibleIndex = 4;
            this.gdcLowerLimitIfEquation.Width = 97;
            // 
            // ddlLowerLimitIfEquation
            // 
            this.ddlLowerLimitIfEquation.AutoHeight = false;
            this.ddlLowerLimitIfEquation.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlLowerLimitIfEquation.Name = "ddlLowerLimitIfEquation";
            this.ddlLowerLimitIfEquation.NullText = "";
            // 
            // gdcPositionBailTypeID
            // 
            this.gdcPositionBailTypeID.Caption = "持仓控制类型";
            this.gdcPositionBailTypeID.ColumnEdit = this.ddlPositionBailTypeID;
            this.gdcPositionBailTypeID.FieldName = "PositionBailTypeID";
            this.gdcPositionBailTypeID.Name = "gdcPositionBailTypeID";
            this.gdcPositionBailTypeID.Visible = true;
            this.gdcPositionBailTypeID.VisibleIndex = 2;
            this.gdcPositionBailTypeID.Width = 79;
            // 
            // ddlPositionBailTypeID
            // 
            this.ddlPositionBailTypeID.AutoHeight = false;
            this.ddlPositionBailTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlPositionBailTypeID.Name = "ddlPositionBailTypeID";
            this.ddlPositionBailTypeID.NullText = "";
            // 
            // gdcPositionValueTypeID
            // 
            this.gdcPositionValueTypeID.Caption = "持仓取值类型";
            this.gdcPositionValueTypeID.ColumnEdit = this.ddlPositionValueTypeID;
            this.gdcPositionValueTypeID.FieldName = "PositionValueTypeID";
            this.gdcPositionValueTypeID.Name = "gdcPositionValueTypeID";
            this.gdcPositionValueTypeID.Visible = true;
            this.gdcPositionValueTypeID.VisibleIndex = 7;
            this.gdcPositionValueTypeID.Width = 71;
            // 
            // ddlPositionValueTypeID
            // 
            this.ddlPositionValueTypeID.AutoHeight = false;
            this.ddlPositionValueTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlPositionValueTypeID.Name = "ddlPositionValueTypeID";
            this.ddlPositionValueTypeID.NullText = "";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "最小交割单位整数倍验证";
            this.gridColumn1.FieldName = "MinUnitLimit";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 9;
            this.gridColumn1.Width = 81;
            // 
            // gdPositionLimitValueResult
            // 
            this.gdPositionLimitValueResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdPositionLimitValueResult.EmbeddedNavigator.Name = "";
            this.gdPositionLimitValueResult.Location = new System.Drawing.Point(2, 2);
            this.gdPositionLimitValueResult.MainView = this.gdPositionLimitValueSelect;
            this.gdPositionLimitValueResult.Name = "gdPositionLimitValueResult";
            this.gdPositionLimitValueResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlDeliveryMonthTypeID,
            this.ddlPositionBailTypeID,
            this.ddlPositionValueTypeID,
            this.ddlUpperLimitIfEquation,
            this.ddlLowerLimitIfEquation,
            this.ddlBreedClassID});
            this.gdPositionLimitValueResult.Size = new System.Drawing.Size(788, 445);
            this.gdPositionLimitValueResult.TabIndex = 8;
            this.gdPositionLimitValueResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdPositionLimitValueSelect});
            this.gdPositionLimitValueResult.DoubleClick += new System.EventHandler(this.gdPositionLimitValueResult_DoubleClick);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdPositionLimitValueResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 82);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(792, 449);
            this.panelControl1.TabIndex = 64;
            this.panelControl1.Text = "panelControl1";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnModify);
            this.panelControl2.Controls.Add(this.btnDelete);
            this.panelControl2.Controls.Add(this.btnAdd);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 531);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(792, 35);
            this.panelControl2.TabIndex = 69;
            this.panelControl2.Text = "panelControl2";
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(622, 7);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 69;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(705, 7);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 70;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(541, 7);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 71;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // PositionLimitValueManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl3);
            this.MaximizeBox = false;
            this.Name = "PositionLimitValueManageUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "商品期货持仓管理";
            this.Load += new System.EventHandler(this.PositionRangeValManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPositionBailTypeID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDeliveryMonthTypeID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdPositionLimitValueSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDeliveryMonthTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUpperLimitIfEquation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlLowerLimitIfEquation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlPositionBailTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlPositionValueTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdPositionLimitValueResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn gdcStart;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPositionBailTypeID;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbDeliveryMonthTypeID;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraGrid.Views.Grid.GridView gdPositionLimitValueSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcDeliveryMonthTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcPositionValue;
        private DevExpress.XtraGrid.Columns.GridColumn gdcEnds;
        private DevExpress.XtraGrid.Columns.GridColumn gdcUpperLimitIfEquation;
        private DevExpress.XtraGrid.Columns.GridColumn gdcLowerLimitIfEquation;
        private DevExpress.XtraGrid.Columns.GridColumn gdcPositionBailTypeID;
        private DevExpress.XtraGrid.GridControl gdPositionLimitValueResult;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gdcPositionValueTypeID;
        private DevExpress.XtraEditors.TextEdit txtBreedClassName;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlDeliveryMonthTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlPositionBailTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlPositionValueTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlUpperLimitIfEquation;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlLowerLimitIfEquation;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBreedClassID;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;

    }
}