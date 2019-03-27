namespace ManagementCenterConsole.UI.CommonParameterSet
{
    partial class BreedClassManagerUI
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
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.gdcBreedClassTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBreedClassTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdBreedClassResult = new DevExpress.XtraGrid.GridControl();
            this.gdvBreedClassSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBreedClassName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcBourseTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBourseTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcAccountTypeIDFund = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlAccountTypeIDFund = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcAccountTypeIDHold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlAccountTypeIDHold = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddCommodity = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.cmbAccountTypeIDFund = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbAccountTypeIDHold = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbBourseType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbBreedClassType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtBreedClassName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.cmbBourseTypeIDQ = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbBreedClassTypeIDQ = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdBreedClassResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvBreedClassSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlAccountTypeIDFund)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlAccountTypeIDHold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountTypeIDFund.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountTypeIDHold.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBourseType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBourseTypeIDQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassTypeIDQ.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // UCPageNavig
            // 
            this.UCPageNavig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UCPageNavig.CurrentPage = 0;
            this.UCPageNavig.Location = new System.Drawing.Point(464, 7);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 3;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // gdcBreedClassTypeID
            // 
            this.gdcBreedClassTypeID.Caption = "品种类型";
            this.gdcBreedClassTypeID.ColumnEdit = this.ddlBreedClassTypeID;
            this.gdcBreedClassTypeID.FieldName = "BreedClassTypeID";
            this.gdcBreedClassTypeID.Name = "gdcBreedClassTypeID";
            this.gdcBreedClassTypeID.Visible = true;
            this.gdcBreedClassTypeID.VisibleIndex = 1;
            // 
            // ddlBreedClassTypeID
            // 
            this.ddlBreedClassTypeID.AutoHeight = false;
            this.ddlBreedClassTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBreedClassTypeID.Name = "ddlBreedClassTypeID";
            this.ddlBreedClassTypeID.NullText = "";
            // 
            // gdBreedClassResult
            // 
            this.gdBreedClassResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdBreedClassResult.EmbeddedNavigator.Name = "";
            this.gdBreedClassResult.Location = new System.Drawing.Point(2, 2);
            this.gdBreedClassResult.MainView = this.gdvBreedClassSelect;
            this.gdBreedClassResult.Name = "gdBreedClassResult";
            this.gdBreedClassResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBreedClassTypeID,
            this.ddlBourseTypeID,
            this.ddlAccountTypeIDFund,
            this.ddlAccountTypeIDHold});
            this.gdBreedClassResult.Size = new System.Drawing.Size(788, 361);
            this.gdBreedClassResult.TabIndex = 0;
            this.gdBreedClassResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvBreedClassSelect});
            this.gdBreedClassResult.DoubleClick += new System.EventHandler(this.gdBreedClassResult_DoubleClick);
            // 
            // gdvBreedClassSelect
            // 
            this.gdvBreedClassSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBreedClassName,
            this.gdcBreedClassTypeID,
            this.gdcBourseTypeID,
            this.gdcAccountTypeIDFund,
            this.gdcAccountTypeIDHold});
            this.gdvBreedClassSelect.GridControl = this.gdBreedClassResult;
            this.gdvBreedClassSelect.Name = "gdvBreedClassSelect";
            this.gdvBreedClassSelect.OptionsBehavior.Editable = false;
            this.gdvBreedClassSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcBreedClassName
            // 
            this.gdcBreedClassName.Caption = "品种名称";
            this.gdcBreedClassName.FieldName = "BreedClassName";
            this.gdcBreedClassName.Name = "gdcBreedClassName";
            this.gdcBreedClassName.Visible = true;
            this.gdcBreedClassName.VisibleIndex = 0;
            // 
            // gdcBourseTypeID
            // 
            this.gdcBourseTypeID.Caption = "交易所";
            this.gdcBourseTypeID.ColumnEdit = this.ddlBourseTypeID;
            this.gdcBourseTypeID.FieldName = "BourseTypeID";
            this.gdcBourseTypeID.Name = "gdcBourseTypeID";
            this.gdcBourseTypeID.Visible = true;
            this.gdcBourseTypeID.VisibleIndex = 2;
            // 
            // ddlBourseTypeID
            // 
            this.ddlBourseTypeID.AutoHeight = false;
            this.ddlBourseTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBourseTypeID.Name = "ddlBourseTypeID";
            this.ddlBourseTypeID.NullText = "";
            // 
            // gdcAccountTypeIDFund
            // 
            this.gdcAccountTypeIDFund.Caption = "资金账号类型";
            this.gdcAccountTypeIDFund.ColumnEdit = this.ddlAccountTypeIDFund;
            this.gdcAccountTypeIDFund.FieldName = "AccountTypeIDFund";
            this.gdcAccountTypeIDFund.Name = "gdcAccountTypeIDFund";
            this.gdcAccountTypeIDFund.Visible = true;
            this.gdcAccountTypeIDFund.VisibleIndex = 3;
            // 
            // ddlAccountTypeIDFund
            // 
            this.ddlAccountTypeIDFund.AutoHeight = false;
            this.ddlAccountTypeIDFund.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlAccountTypeIDFund.Name = "ddlAccountTypeIDFund";
            this.ddlAccountTypeIDFund.NullText = "";
            // 
            // gdcAccountTypeIDHold
            // 
            this.gdcAccountTypeIDHold.Caption = "持仓账号类型";
            this.gdcAccountTypeIDHold.ColumnEdit = this.ddlAccountTypeIDHold;
            this.gdcAccountTypeIDHold.FieldName = "AccountTypeIDHold";
            this.gdcAccountTypeIDHold.Name = "gdcAccountTypeIDHold";
            this.gdcAccountTypeIDHold.Visible = true;
            this.gdcAccountTypeIDHold.VisibleIndex = 4;
            // 
            // ddlAccountTypeIDHold
            // 
            this.ddlAccountTypeIDHold.AutoHeight = false;
            this.ddlAccountTypeIDHold.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlAccountTypeIDHold.Name = "ddlAccountTypeIDHold";
            this.ddlAccountTypeIDHold.NullText = "";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdBreedClassResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 40);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(792, 365);
            this.panelControl1.TabIndex = 19;
            this.panelControl1.Text = "panelControl1";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "交易所:";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panel1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 405);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(792, 161);
            this.panelControl2.TabIndex = 1;
            this.panelControl2.Text = "panelControl2";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.labelControl14);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnAddCommodity);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.cmbAccountTypeIDFund);
            this.panel1.Controls.Add(this.cmbAccountTypeIDHold);
            this.panel1.Controls.Add(this.cmbBourseType);
            this.panel1.Controls.Add(this.cmbBreedClassType);
            this.panel1.Controls.Add(this.txtBreedClassName);
            this.panel1.Controls.Add(this.labelControl5);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.labelControl6);
            this.panel1.Location = new System.Drawing.Point(5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 150);
            this.panel1.TabIndex = 25;
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.Location = new System.Drawing.Point(227, 10);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(7, 14);
            this.labelControl14.TabIndex = 40;
            this.labelControl14.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(583, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddCommodity
            // 
            this.btnAddCommodity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCommodity.Location = new System.Drawing.Point(676, 120);
            this.btnAddCommodity.Name = "btnAddCommodity";
            this.btnAddCommodity.Size = new System.Drawing.Size(99, 23);
            this.btnAddCommodity.TabIndex = 9;
            this.btnAddCommodity.Text = "代码配置";
            this.btnAddCommodity.Click += new System.EventHandler(this.btnAddCommodity_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(502, 120);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(421, 120);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 6;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(340, 120);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cmbAccountTypeIDFund
            // 
            this.cmbAccountTypeIDFund.Location = new System.Drawing.Point(330, 3);
            this.cmbAccountTypeIDFund.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.cmbAccountTypeIDFund.Name = "cmbAccountTypeIDFund";
            this.cmbAccountTypeIDFund.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAccountTypeIDFund.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbAccountTypeIDFund.Size = new System.Drawing.Size(122, 21);
            this.cmbAccountTypeIDFund.TabIndex = 3;
            // 
            // cmbAccountTypeIDHold
            // 
            this.cmbAccountTypeIDHold.Location = new System.Drawing.Point(330, 40);
            this.cmbAccountTypeIDHold.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.cmbAccountTypeIDHold.Name = "cmbAccountTypeIDHold";
            this.cmbAccountTypeIDHold.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAccountTypeIDHold.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbAccountTypeIDHold.Size = new System.Drawing.Size(122, 21);
            this.cmbAccountTypeIDHold.TabIndex = 4;
            // 
            // cmbBourseType
            // 
            this.cmbBourseType.Location = new System.Drawing.Point(101, 77);
            this.cmbBourseType.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.cmbBourseType.Name = "cmbBourseType";
            this.cmbBourseType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBourseType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBourseType.Size = new System.Drawing.Size(122, 21);
            this.cmbBourseType.TabIndex = 2;
            // 
            // cmbBreedClassType
            // 
            this.cmbBreedClassType.Location = new System.Drawing.Point(101, 41);
            this.cmbBreedClassType.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.cmbBreedClassType.Name = "cmbBreedClassType";
            this.cmbBreedClassType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBreedClassType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBreedClassType.Size = new System.Drawing.Size(122, 21);
            this.cmbBreedClassType.TabIndex = 1;
            // 
            // txtBreedClassName
            // 
            this.txtBreedClassName.Location = new System.Drawing.Point(101, 3);
            this.txtBreedClassName.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.txtBreedClassName.Name = "txtBreedClassName";
            this.txtBreedClassName.Size = new System.Drawing.Size(122, 21);
            this.txtBreedClassName.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(248, 44);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(76, 14);
            this.labelControl5.TabIndex = 34;
            this.labelControl5.Text = "持仓账号类型:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(40, 80);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(40, 14);
            this.labelControl4.TabIndex = 32;
            this.labelControl4.Text = "交易所:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(248, 6);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(76, 14);
            this.labelControl3.TabIndex = 29;
            this.labelControl3.Text = "资金账号类型:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(40, 43);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 27;
            this.labelControl2.Text = "品种类型:";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(40, 6);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(52, 14);
            this.labelControl6.TabIndex = 25;
            this.labelControl6.Text = "品种名称:";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(335, 11);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.cmbBourseTypeIDQ);
            this.panelControl3.Controls.Add(this.cmbBreedClassTypeIDQ);
            this.panelControl3.Controls.Add(this.labelControl8);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.UCPageNavig);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(792, 40);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
            // 
            // cmbBourseTypeIDQ
            // 
            this.cmbBourseTypeIDQ.Location = new System.Drawing.Point(58, 9);
            this.cmbBourseTypeIDQ.Name = "cmbBourseTypeIDQ";
            this.cmbBourseTypeIDQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBourseTypeIDQ.Size = new System.Drawing.Size(100, 21);
            this.cmbBourseTypeIDQ.TabIndex = 0;
            // 
            // cmbBreedClassTypeIDQ
            // 
            this.cmbBreedClassTypeIDQ.Location = new System.Drawing.Point(229, 11);
            this.cmbBreedClassTypeIDQ.Name = "cmbBreedClassTypeIDQ";
            this.cmbBreedClassTypeIDQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBreedClassTypeIDQ.Size = new System.Drawing.Size(100, 21);
            this.cmbBreedClassTypeIDQ.TabIndex = 1;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(171, 14);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(52, 14);
            this.labelControl8.TabIndex = 16;
            this.labelControl8.Text = "品种类型:";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(583, 120);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 41;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // BreedClassManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl3);
            this.Name = "BreedClassManagerUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "品种管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.BreedClassManagerUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ddlBreedClassTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdBreedClassResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvBreedClassSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlAccountTypeIDFund)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlAccountTypeIDHold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountTypeIDFund.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountTypeIDHold.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBourseType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBreedClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBourseTypeIDQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBreedClassTypeIDQ.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassTypeID;
        private DevExpress.XtraGrid.GridControl gdBreedClassResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvBreedClassSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBreedClassName;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBourseTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcAccountTypeIDFund;
        private DevExpress.XtraGrid.Columns.GridColumn gdcAccountTypeIDHold;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBreedClassTypeIDQ;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBreedClassTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBourseTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlAccountTypeIDFund;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlAccountTypeIDHold;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBourseTypeIDQ;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnAddCommodity;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.ComboBoxEdit cmbAccountTypeIDFund;
        private DevExpress.XtraEditors.ComboBoxEdit cmbAccountTypeIDHold;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBourseType;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBreedClassType;
        private DevExpress.XtraEditors.TextEdit txtBreedClassName;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}