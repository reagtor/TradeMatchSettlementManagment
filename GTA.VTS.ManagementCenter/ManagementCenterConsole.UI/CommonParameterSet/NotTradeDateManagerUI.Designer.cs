namespace ManagementCenterConsole.UI.CommonParameterSet
{
    partial class NotTradeDateManagerUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotTradeDateManagerUI));
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtQueryBourseName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.gdcNotTradeDay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.gdvNotTradeDateSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBourseTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBourseTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdNotTradeDateResult = new DevExpress.XtraGrid.GridControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.cmbBourseTypeID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.dtNotTradeDay = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryBourseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvNotTradeDateSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdNotTradeDateResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBourseTypeID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtNotTradeDay.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtQueryBourseName);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Controls.Add(this.btnQuery);
            this.panelControl2.Controls.Add(this.UCPageNavig);
            this.panelControl2.Location = new System.Drawing.Point(11, 12);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(608, 43);
            this.panelControl2.TabIndex = 0;
            this.panelControl2.Text = "panelControl2";
            // 
            // txtQueryBourseName
            // 
            this.txtQueryBourseName.Location = new System.Drawing.Point(78, 5);
            this.txtQueryBourseName.Name = "txtQueryBourseName";
            this.txtQueryBourseName.Size = new System.Drawing.Size(100, 21);
            this.txtQueryBourseName.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(8, 8);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(64, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "交易所名称:";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(185, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // UCPageNavig
            // 
            this.UCPageNavig.CurrentPage = 0;
            this.UCPageNavig.Location = new System.Drawing.Point(277, 5);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(326, 28);
            this.UCPageNavig.TabIndex = 6;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(493, 386);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gdcNotTradeDay
            // 
            this.gdcNotTradeDay.Caption = "非交易日";
            this.gdcNotTradeDay.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.gdcNotTradeDay.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcNotTradeDay.FieldName = "NotTradeDay";
            this.gdcNotTradeDay.Name = "gdcNotTradeDay";
            this.gdcNotTradeDay.Visible = true;
            this.gdcNotTradeDay.VisibleIndex = 1;
            this.gdcNotTradeDay.Width = 322;
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(426, 386);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(61, 23);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // gdvNotTradeDateSelect
            // 
            this.gdvNotTradeDateSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBourseTypeName,
            this.gdcNotTradeDay});
            this.gdvNotTradeDateSelect.GridControl = this.gdNotTradeDateResult;
            this.gdvNotTradeDateSelect.Name = "gdvNotTradeDateSelect";
            this.gdvNotTradeDateSelect.OptionsBehavior.Editable = false;
            this.gdvNotTradeDateSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcBourseTypeName
            // 
            this.gdcBourseTypeName.Caption = "交易所名称";
            this.gdcBourseTypeName.ColumnEdit = this.ddlBourseTypeID;
            this.gdcBourseTypeName.FieldName = "BourseTypeID";
            this.gdcBourseTypeName.Name = "gdcBourseTypeName";
            this.gdcBourseTypeName.Visible = true;
            this.gdcBourseTypeName.VisibleIndex = 0;
            this.gdcBourseTypeName.Width = 263;
            // 
            // ddlBourseTypeID
            // 
            this.ddlBourseTypeID.AutoHeight = false;
            this.ddlBourseTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBourseTypeID.Name = "ddlBourseTypeID";
            this.ddlBourseTypeID.NullText = "";
            // 
            // gdNotTradeDateResult
            // 
            this.gdNotTradeDateResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdNotTradeDateResult.EmbeddedNavigator.Name = "";
            this.gdNotTradeDateResult.Location = new System.Drawing.Point(2, 2);
            this.gdNotTradeDateResult.MainView = this.gdvNotTradeDateSelect;
            this.gdNotTradeDateResult.Name = "gdNotTradeDateResult";
            this.gdNotTradeDateResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBourseTypeID});
            this.gdNotTradeDateResult.Size = new System.Drawing.Size(606, 275);
            this.gdNotTradeDateResult.TabIndex = 8;
            this.gdNotTradeDateResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvNotTradeDateSelect});
            this.gdNotTradeDateResult.DoubleClick += new System.EventHandler(this.gdNotTradeDateResult_DoubleClick);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gdNotTradeDateResult);
            this.panelControl3.Location = new System.Drawing.Point(11, 61);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(610, 279);
            this.panelControl3.TabIndex = 21;
            this.panelControl3.Text = "panelControl3";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(359, 386);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(61, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cmbBourseTypeID
            // 
            this.cmbBourseTypeID.Location = new System.Drawing.Point(154, 5);
            this.cmbBourseTypeID.Name = "cmbBourseTypeID";
            this.cmbBourseTypeID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBourseTypeID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBourseTypeID.Size = new System.Drawing.Size(100, 21);
            this.cmbBourseTypeID.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(289, 8);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "非交易日:";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.dtNotTradeDay);
            this.panelControl1.Controls.Add(this.cmbBourseTypeID);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Location = new System.Drawing.Point(12, 346);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(609, 34);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.Text = "panelControl1";
            // 
            // dtNotTradeDay
            // 
            this.dtNotTradeDay.EditValue = null;
            this.dtNotTradeDay.Location = new System.Drawing.Point(347, 5);
            this.dtNotTradeDay.Name = "dtNotTradeDay";
            this.dtNotTradeDay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtNotTradeDay.Size = new System.Drawing.Size(100, 21);
            this.dtNotTradeDay.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(84, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "交易所名称:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(292, 386);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(561, 386);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 23);
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // NotTradeDateManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(632, 421);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NotTradeDateManagerUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "非交易日期管理";
            this.Load += new System.EventHandler(this.NotTradeDateManagerUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryBourseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvNotTradeDateSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdNotTradeDateResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBourseTypeID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtNotTradeDay.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraGrid.Columns.GridColumn gdcNotTradeDay;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvNotTradeDateSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBourseTypeName;
        private DevExpress.XtraGrid.GridControl gdNotTradeDateResult;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBourseTypeID;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dtNotTradeDay;
        private DevExpress.XtraEditors.TextEdit txtQueryBourseName;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBourseTypeID;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}