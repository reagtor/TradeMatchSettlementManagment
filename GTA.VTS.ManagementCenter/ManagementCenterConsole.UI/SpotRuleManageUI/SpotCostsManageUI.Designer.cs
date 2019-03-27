namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    partial class SpotCostsManageUI
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
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtBreedClassName = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gdSpotCostsResult = new DevExpress.XtraGrid.GridControl();
            this.gdvSpotCostsSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcStampDutyTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlStampDutyTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcStampDuty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTransferToll = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcBreedClassID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBreedClassID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcCommision = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcStampDutyStartpoint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTranTollStartpoint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTransferTollTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlTransferTollTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcCommisionStartpoint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcClearingFees = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcCurrencyTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlCurrencyTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.ddlGetValueTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotCostsResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvSpotCostsSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlStampDutyTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTransferTollTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCurrencyTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlGetValueTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(169, 5);
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
            this.UCPageNavig.Location = new System.Drawing.Point(478, 4);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 2;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(5, 8);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(52, 14);
            this.labelControl9.TabIndex = 16;
            this.labelControl9.Text = "品种名称:";
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
            this.panelControl3.Size = new System.Drawing.Size(805, 36);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
            // 
            // txtBreedClassName
            // 
            this.txtBreedClassName.Location = new System.Drawing.Point(63, 5);
            this.txtBreedClassName.Name = "txtBreedClassName";
            this.txtBreedClassName.Size = new System.Drawing.Size(100, 21);
            this.txtBreedClassName.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdSpotCostsResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 36);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(805, 480);
            this.panelControl1.TabIndex = 49;
            this.panelControl1.Text = "panelControl1";
            // 
            // gdSpotCostsResult
            // 
            this.gdSpotCostsResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSpotCostsResult.EmbeddedNavigator.Name = "";
            this.gdSpotCostsResult.Location = new System.Drawing.Point(2, 2);
            this.gdSpotCostsResult.MainView = this.gdvSpotCostsSelect;
            this.gdSpotCostsResult.Name = "gdSpotCostsResult";
            this.gdSpotCostsResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBreedClassID,
            this.ddlGetValueTypeID,
            this.ddlStampDutyTypeID,
            this.ddlTransferTollTypeID,
            this.ddlCurrencyTypeID});
            this.gdSpotCostsResult.Size = new System.Drawing.Size(801, 476);
            this.gdSpotCostsResult.TabIndex = 8;
            this.gdSpotCostsResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvSpotCostsSelect});
            this.gdSpotCostsResult.DoubleClick += new System.EventHandler(this.gdSpotCostsResult_DoubleClick);
            this.gdSpotCostsResult.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gdSpotCostsResult_MouseMove);
            // 
            // gdvSpotCostsSelect
            // 
            this.gdvSpotCostsSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcStampDutyTypeID,
            this.gdcStampDuty,
            this.gdcTransferToll,
            this.gdcBreedClassID,
            this.gdcCommision,
            this.gdcStampDutyStartpoint,
            this.gdcTranTollStartpoint,
            this.gdcTransferTollTypeID,
            this.gdcCommisionStartpoint,
            this.gdcClearingFees,
            this.gdcCurrencyTypeID});
            this.gdvSpotCostsSelect.GridControl = this.gdSpotCostsResult;
            this.gdvSpotCostsSelect.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gdvSpotCostsSelect.Name = "gdvSpotCostsSelect";
            this.gdvSpotCostsSelect.OptionsBehavior.Editable = false;
            this.gdvSpotCostsSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcStampDutyTypeID
            // 
            this.gdcStampDutyTypeID.Caption = "印花税收取方式";
            this.gdcStampDutyTypeID.ColumnEdit = this.ddlStampDutyTypeID;
            this.gdcStampDutyTypeID.FieldName = "StampDutyTypeID";
            this.gdcStampDutyTypeID.Name = "gdcStampDutyTypeID";
            this.gdcStampDutyTypeID.Visible = true;
            this.gdcStampDutyTypeID.VisibleIndex = 3;
            this.gdcStampDutyTypeID.Width = 78;
            // 
            // ddlStampDutyTypeID
            // 
            this.ddlStampDutyTypeID.AutoHeight = false;
            this.ddlStampDutyTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlStampDutyTypeID.Name = "ddlStampDutyTypeID";
            this.ddlStampDutyTypeID.NullText = "";
            // 
            // gdcStampDuty
            // 
            this.gdcStampDuty.Caption = "印花税(%)";
            this.gdcStampDuty.FieldName = "StampDuty";
            this.gdcStampDuty.Name = "gdcStampDuty";
            this.gdcStampDuty.Visible = true;
            this.gdcStampDuty.VisibleIndex = 1;
            this.gdcStampDuty.Width = 50;
            // 
            // gdcTransferToll
            // 
            this.gdcTransferToll.Caption = "过户费(%)";
            this.gdcTransferToll.FieldName = "TransferToll";
            this.gdcTransferToll.Name = "gdcTransferToll";
            this.gdcTransferToll.Visible = true;
            this.gdcTransferToll.VisibleIndex = 6;
            this.gdcTransferToll.Width = 69;
            // 
            // gdcBreedClassID
            // 
            this.gdcBreedClassID.Caption = "品种名称";
            this.gdcBreedClassID.ColumnEdit = this.ddlBreedClassID;
            this.gdcBreedClassID.FieldName = "BreedClassID";
            this.gdcBreedClassID.Name = "gdcBreedClassID";
            this.gdcBreedClassID.Visible = true;
            this.gdcBreedClassID.VisibleIndex = 0;
            this.gdcBreedClassID.Width = 58;
            // 
            // ddlBreedClassID
            // 
            this.ddlBreedClassID.AutoHeight = false;
            this.ddlBreedClassID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBreedClassID.Name = "ddlBreedClassID";
            this.ddlBreedClassID.NullText = "";
            // 
            // gdcCommision
            // 
            this.gdcCommision.Caption = "佣金(%)";
            this.gdcCommision.FieldName = "Commision";
            this.gdcCommision.Name = "gdcCommision";
            this.gdcCommision.Visible = true;
            this.gdcCommision.VisibleIndex = 4;
            this.gdcCommision.Width = 58;
            // 
            // gdcStampDutyStartpoint
            // 
            this.gdcStampDutyStartpoint.Caption = "印花税起点(元)";
            this.gdcStampDutyStartpoint.FieldName = "StampDutyStartingpoint";
            this.gdcStampDutyStartpoint.Name = "gdcStampDutyStartpoint";
            this.gdcStampDutyStartpoint.Visible = true;
            this.gdcStampDutyStartpoint.VisibleIndex = 2;
            this.gdcStampDutyStartpoint.Width = 79;
            // 
            // gdcTranTollStartpoint
            // 
            this.gdcTranTollStartpoint.Caption = "过户费起点(元)";
            this.gdcTranTollStartpoint.FieldName = "TransferTollStartingpoint";
            this.gdcTranTollStartpoint.Name = "gdcTranTollStartpoint";
            this.gdcTranTollStartpoint.Visible = true;
            this.gdcTranTollStartpoint.VisibleIndex = 7;
            this.gdcTranTollStartpoint.Width = 78;
            // 
            // gdcTransferTollTypeID
            // 
            this.gdcTransferTollTypeID.Caption = "过户费取值类型";
            this.gdcTransferTollTypeID.ColumnEdit = this.ddlTransferTollTypeID;
            this.gdcTransferTollTypeID.FieldName = "TransferTollTypeID";
            this.gdcTransferTollTypeID.Name = "gdcTransferTollTypeID";
            this.gdcTransferTollTypeID.Visible = true;
            this.gdcTransferTollTypeID.VisibleIndex = 8;
            this.gdcTransferTollTypeID.Width = 79;
            // 
            // ddlTransferTollTypeID
            // 
            this.ddlTransferTollTypeID.AutoHeight = false;
            this.ddlTransferTollTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlTransferTollTypeID.Name = "ddlTransferTollTypeID";
            this.ddlTransferTollTypeID.NullText = "";
            // 
            // gdcCommisionStartpoint
            // 
            this.gdcCommisionStartpoint.Caption = "佣金起点(元)";
            this.gdcCommisionStartpoint.FieldName = "CommisionStartingpoint";
            this.gdcCommisionStartpoint.Name = "gdcCommisionStartpoint";
            this.gdcCommisionStartpoint.Visible = true;
            this.gdcCommisionStartpoint.VisibleIndex = 5;
            this.gdcCommisionStartpoint.Width = 80;
            // 
            // gdcClearingFees
            // 
            this.gdcClearingFees.Caption = "结算费(%)";
            this.gdcClearingFees.FieldName = "ClearingFees";
            this.gdcClearingFees.Name = "gdcClearingFees";
            this.gdcClearingFees.Visible = true;
            this.gdcClearingFees.VisibleIndex = 9;
            this.gdcClearingFees.Width = 68;
            // 
            // gdcCurrencyTypeID
            // 
            this.gdcCurrencyTypeID.Caption = "交易货币类型";
            this.gdcCurrencyTypeID.ColumnEdit = this.ddlCurrencyTypeID;
            this.gdcCurrencyTypeID.FieldName = "CurrencyTypeID";
            this.gdcCurrencyTypeID.Name = "gdcCurrencyTypeID";
            this.gdcCurrencyTypeID.Visible = true;
            this.gdcCurrencyTypeID.VisibleIndex = 10;
            this.gdcCurrencyTypeID.Width = 84;
            // 
            // ddlCurrencyTypeID
            // 
            this.ddlCurrencyTypeID.AutoHeight = false;
            this.ddlCurrencyTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlCurrencyTypeID.Name = "ddlCurrencyTypeID";
            this.ddlCurrencyTypeID.NullText = "";
            // 
            // ddlGetValueTypeID
            // 
            this.ddlGetValueTypeID.AutoHeight = false;
            this.ddlGetValueTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlGetValueTypeID.Name = "ddlGetValueTypeID";
            this.ddlGetValueTypeID.NullText = "";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnModify);
            this.panelControl2.Controls.Add(this.btnDelete);
            this.panelControl2.Controls.Add(this.btnAdd);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 516);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(805, 50);
            this.panelControl2.TabIndex = 1;
            this.panelControl2.Text = "panelControl2";
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(636, 13);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 1;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(717, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(555, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // SpotCostsManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 566);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl3);
            this.Name = "SpotCostsManageUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "现货交易费用管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SpotCostsManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotCostsResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvSpotCostsSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlStampDutyTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTransferTollTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCurrencyTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlGetValueTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.TextEdit txtBreedClassName;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gdSpotCostsResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvSpotCostsSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcStampDutyTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlStampDutyTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlGetValueTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcStampDuty;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTransferToll;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBreedClassID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCommision;
        private DevExpress.XtraGrid.Columns.GridColumn gdcStampDutyStartpoint;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTranTollStartpoint;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTransferTollTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlTransferTollTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCommisionStartpoint;
        private DevExpress.XtraGrid.Columns.GridColumn gdcClearingFees;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCurrencyTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlCurrencyTypeID;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
    }
}