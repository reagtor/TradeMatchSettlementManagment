namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    partial class SpotUnitConversionManageUI
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
            this.gdSpotUnitConversionRelult = new DevExpress.XtraGrid.GridControl();
            this.gdSpotUnitConversionSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBreedClassID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBreedClassID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcUnitIDFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlUnitIDFrom = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcUnitIDTo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlUnitIDTo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.cmbUnitIDTo = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cmbUnitIDFrom = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBreedClassID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtValue = new DevExpress.XtraEditors.TextEdit();
            this.txtBreedClassName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotUnitConversionRelult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotUnitConversionSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUnitIDFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUnitIDTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnitIDTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnitIDFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdSpotUnitConversionRelult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 45);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(817, 449);
            this.panelControl1.TabIndex = 63;
            this.panelControl1.Text = "panelControl1";
            // 
            // gdSpotUnitConversionRelult
            // 
            this.gdSpotUnitConversionRelult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSpotUnitConversionRelult.EmbeddedNavigator.Name = "";
            this.gdSpotUnitConversionRelult.Location = new System.Drawing.Point(2, 2);
            this.gdSpotUnitConversionRelult.MainView = this.gdSpotUnitConversionSelect;
            this.gdSpotUnitConversionRelult.Name = "gdSpotUnitConversionRelult";
            this.gdSpotUnitConversionRelult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBreedClassID,
            this.ddlUnitIDFrom,
            this.ddlUnitIDTo});
            this.gdSpotUnitConversionRelult.Size = new System.Drawing.Size(813, 445);
            this.gdSpotUnitConversionRelult.TabIndex = 0;
            this.gdSpotUnitConversionRelult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdSpotUnitConversionSelect});
            this.gdSpotUnitConversionRelult.DoubleClick += new System.EventHandler(this.gdSpotUnitConversionRelult_DoubleClick);
            // 
            // gdSpotUnitConversionSelect
            // 
            this.gdSpotUnitConversionSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBreedClassID,
            this.gdcValue,
            this.gdcUnitIDFrom,
            this.gdcUnitIDTo});
            this.gdSpotUnitConversionSelect.GridControl = this.gdSpotUnitConversionRelult;
            this.gdSpotUnitConversionSelect.Name = "gdSpotUnitConversionSelect";
            this.gdSpotUnitConversionSelect.OptionsBehavior.Editable = false;
            this.gdSpotUnitConversionSelect.OptionsView.ShowGroupPanel = false;
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
            // gdcValue
            // 
            this.gdcValue.Caption = "比例";
            this.gdcValue.FieldName = "Value";
            this.gdcValue.Name = "gdcValue";
            this.gdcValue.Visible = true;
            this.gdcValue.VisibleIndex = 3;
            this.gdcValue.Width = 185;
            // 
            // gdcUnitIDFrom
            // 
            this.gdcUnitIDFrom.Caption = "从单位";
            this.gdcUnitIDFrom.ColumnEdit = this.ddlUnitIDFrom;
            this.gdcUnitIDFrom.FieldName = "UnitIDFrom";
            this.gdcUnitIDFrom.Name = "gdcUnitIDFrom";
            this.gdcUnitIDFrom.Visible = true;
            this.gdcUnitIDFrom.VisibleIndex = 1;
            // 
            // ddlUnitIDFrom
            // 
            this.ddlUnitIDFrom.AutoHeight = false;
            this.ddlUnitIDFrom.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlUnitIDFrom.Name = "ddlUnitIDFrom";
            this.ddlUnitIDFrom.NullText = "";
            // 
            // gdcUnitIDTo
            // 
            this.gdcUnitIDTo.Caption = "到单位";
            this.gdcUnitIDTo.ColumnEdit = this.ddlUnitIDTo;
            this.gdcUnitIDTo.FieldName = "UnitIDTo";
            this.gdcUnitIDTo.Name = "gdcUnitIDTo";
            this.gdcUnitIDTo.Visible = true;
            this.gdcUnitIDTo.VisibleIndex = 2;
            // 
            // ddlUnitIDTo
            // 
            this.ddlUnitIDTo.AutoHeight = false;
            this.ddlUnitIDTo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlUnitIDTo.Name = "ddlUnitIDTo";
            this.ddlUnitIDTo.NullText = "";
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.panel1);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(0, 494);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(817, 94);
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
            this.panel1.Controls.Add(this.cmbUnitIDTo);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.cmbUnitIDFrom);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.cmbBreedClassID);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.txtValue);
            this.panel1.Location = new System.Drawing.Point(2, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(810, 85);
            this.panel1.TabIndex = 21;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(719, 54);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 23);
            this.btnOK.TabIndex = 33;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.Location = new System.Drawing.Point(453, 22);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(7, 14);
            this.labelControl14.TabIndex = 32;
            this.labelControl14.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(719, 22);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(485, 55);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(641, 55);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(563, 55);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(72, 23);
            this.btnModify.TabIndex = 5;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // cmbUnitIDTo
            // 
            this.cmbUnitIDTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUnitIDTo.Location = new System.Drawing.Point(467, 17);
            this.cmbUnitIDTo.Name = "cmbUnitIDTo";
            this.cmbUnitIDTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUnitIDTo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbUnitIDTo.Size = new System.Drawing.Size(87, 21);
            this.cmbUnitIDTo.TabIndex = 3;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(338, 17);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(12, 19);
            this.labelControl3.TabIndex = 27;
            this.labelControl3.Text = "=";
            // 
            // cmbUnitIDFrom
            // 
            this.cmbUnitIDFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUnitIDFrom.Location = new System.Drawing.Point(240, 16);
            this.cmbUnitIDFrom.Name = "cmbUnitIDFrom";
            this.cmbUnitIDFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUnitIDFrom.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbUnitIDFrom.Size = new System.Drawing.Size(87, 21);
            this.cmbUnitIDFrom.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(222, 18);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(9, 19);
            this.labelControl2.TabIndex = 26;
            this.labelControl2.Text = "1";
            // 
            // cmbBreedClassID
            // 
            this.cmbBreedClassID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBreedClassID.Location = new System.Drawing.Point(119, 15);
            this.cmbBreedClassID.Name = "cmbBreedClassID";
            this.cmbBreedClassID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBreedClassID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBreedClassID.Size = new System.Drawing.Size(92, 21);
            this.cmbBreedClassID.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Location = new System.Drawing.Point(62, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 25;
            this.labelControl1.Text = "品种名称:";
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.Location = new System.Drawing.Point(360, 17);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(87, 21);
            this.txtValue.TabIndex = 2;
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
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.txtBreedClassName);
            this.panelControl3.Controls.Add(this.labelControl9);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.UCPageNavig);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(817, 45);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
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
            this.UCPageNavig.Location = new System.Drawing.Point(493, 5);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 2;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // SpotUnitConversionManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 588);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl3);
            this.Name = "SpotUnitConversionManageUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "现货单位换算管理";
            this.Load += new System.EventHandler(this.SpotUnitConversionManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotUnitConversionRelult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSpotUnitConversionSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUnitIDFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUnitIDTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnitIDTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnitIDFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gdSpotUnitConversionRelult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdSpotUnitConversionSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBreedClassID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcValue;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.TextEdit txtBreedClassName;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraGrid.Columns.GridColumn gdcUnitIDFrom;
        private DevExpress.XtraGrid.Columns.GridColumn gdcUnitIDTo;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlUnitIDFrom;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlUnitIDTo;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.ComboBoxEdit cmbUnitIDTo;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbUnitIDFrom;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBreedClassID;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtValue;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}