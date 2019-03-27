namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    partial class SpotPositionManageUI
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
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtBreedClassName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.gdSpotPositionResult = new DevExpress.XtraGrid.GridControl();
            this.gdSpotPositionSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBreedClassID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBreedClassID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.cmbBreedClassID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtRate = new DevExpress.XtraEditors.TextEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotPositionResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotPositionSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRate.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.panelControl3.Size = new System.Drawing.Size(785, 45);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
            // 
            // txtBreedClassName
            // 
            this.txtBreedClassName.Location = new System.Drawing.Point(70, 11);
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
            this.btnQuery.Location = new System.Drawing.Point(176, 9);
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
            this.UCPageNavig.Location = new System.Drawing.Point(456, 5);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 2;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // gdSpotPositionResult
            // 
            this.gdSpotPositionResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSpotPositionResult.EmbeddedNavigator.Name = "";
            this.gdSpotPositionResult.Location = new System.Drawing.Point(2, 2);
            this.gdSpotPositionResult.MainView = this.gdSpotPositionSelect;
            this.gdSpotPositionResult.Name = "gdSpotPositionResult";
            this.gdSpotPositionResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBreedClassID});
            this.gdSpotPositionResult.Size = new System.Drawing.Size(781, 484);
            this.gdSpotPositionResult.TabIndex = 0;
            this.gdSpotPositionResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdSpotPositionSelect});
            this.gdSpotPositionResult.DoubleClick += new System.EventHandler(this.gdSpotPositionResult_DoubleClick);
            // 
            // gdSpotPositionSelect
            // 
            this.gdSpotPositionSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBreedClassID,
            this.gdcRate});
            this.gdSpotPositionSelect.GridControl = this.gdSpotPositionResult;
            this.gdSpotPositionSelect.Name = "gdSpotPositionSelect";
            this.gdSpotPositionSelect.OptionsBehavior.Editable = false;
            this.gdSpotPositionSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcBreedClassID
            // 
            this.gdcBreedClassID.Caption = "品种名称";
            this.gdcBreedClassID.ColumnEdit = this.ddlBreedClassID;
            this.gdcBreedClassID.FieldName = "BreedClassID";
            this.gdcBreedClassID.Name = "gdcBreedClassID";
            this.gdcBreedClassID.Visible = true;
            this.gdcBreedClassID.VisibleIndex = 0;
            this.gdcBreedClassID.Width = 127;
            // 
            // ddlBreedClassID
            // 
            this.ddlBreedClassID.AutoHeight = false;
            this.ddlBreedClassID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBreedClassID.Name = "ddlBreedClassID";
            this.ddlBreedClassID.NullText = "";
            // 
            // gdcRate
            // 
            this.gdcRate.Caption = "持仓比率";
            this.gdcRate.FieldName = "Rate";
            this.gdcRate.Name = "gdcRate";
            this.gdcRate.Visible = true;
            this.gdcRate.VisibleIndex = 1;
            this.gdcRate.Width = 185;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdSpotPositionResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 45);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(785, 488);
            this.panelControl1.TabIndex = 57;
            this.panelControl1.Text = "panelControl1";
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.panel1);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(0, 533);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(785, 88);
            this.panelControl4.TabIndex = 1;
            this.panelControl4.Text = "panelControl4";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.labelControl14);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.cmbBreedClassID);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.txtRate);
            this.panel1.Controls.Add(this.labelControl12);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 78);
            this.panel1.TabIndex = 17;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(614, 48);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 25;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.Location = new System.Drawing.Point(532, 10);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(7, 14);
            this.labelControl14.TabIndex = 24;
            this.labelControl14.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(693, 48);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(376, 48);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(535, 48);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(456, 48);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // cmbBreedClassID
            // 
            this.cmbBreedClassID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBreedClassID.Location = new System.Drawing.Point(251, 3);
            this.cmbBreedClassID.Name = "cmbBreedClassID";
            this.cmbBreedClassID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBreedClassID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBreedClassID.Size = new System.Drawing.Size(100, 21);
            this.cmbBreedClassID.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Location = new System.Drawing.Point(193, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 19;
            this.labelControl1.Text = "品种名称:";
            // 
            // txtRate
            // 
            this.txtRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRate.Location = new System.Drawing.Point(426, 3);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(100, 21);
            this.txtRate.TabIndex = 1;
            // 
            // labelControl12
            // 
            this.labelControl12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl12.Location = new System.Drawing.Point(368, 6);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(52, 14);
            this.labelControl12.TabIndex = 18;
            this.labelControl12.Text = "持仓比率:";
            // 
            // SpotPositionManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 621);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl4);
            this.MaximizeBox = false;
            this.Name = "SpotPositionManageUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "持仓限制管理";
            this.Load += new System.EventHandler(this.SpotPositionManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotPositionResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotPositionSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraGrid.GridControl gdSpotPositionResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdSpotPositionSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcRate;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.TextEdit txtBreedClassName;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBreedClassID;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBreedClassID;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtRate;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}