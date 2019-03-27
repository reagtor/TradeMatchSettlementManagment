namespace ManagementCenterConsole.UI.MatchCenterManage
{
    partial class MachineManage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ddl_Bourse = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ucPageNavigation1 = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.btn_Query = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.txt_MachineID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.ddl_BourseType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ddl_Center = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txt_MachineName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.btn_AssignCode = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Del = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Modify = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridMachine = new DevExpress.XtraGrid.GridControl();
            this.ViewMachine = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridCol_MatchMachineID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_MatchMachineName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_BourseTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBourseTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridCol_MatchCenterID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlMatchCenterID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_Bourse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_MachineID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_BourseType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_Center.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_MachineName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlMatchCenterID)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ddl_Bourse);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.ucPageNavigation1);
            this.panelControl1.Controls.Add(this.btn_Query);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(800, 50);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Text = "panelControl1";
            // 
            // ddl_Bourse
            // 
            this.ddl_Bourse.Location = new System.Drawing.Point(83, 15);
            this.ddl_Bourse.Name = "ddl_Bourse";
            this.ddl_Bourse.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddl_Bourse.Size = new System.Drawing.Size(129, 21);
            this.ddl_Bourse.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "所属交易所:";
            // 
            // ucPageNavigation1
            // 
            this.ucPageNavigation1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPageNavigation1.CurrentPage = 0;
            this.ucPageNavigation1.Location = new System.Drawing.Point(465, 13);
            this.ucPageNavigation1.Name = "ucPageNavigation1";
            this.ucPageNavigation1.PageCount = 0;
            this.ucPageNavigation1.Size = new System.Drawing.Size(319, 28);
            this.ucPageNavigation1.TabIndex = 2;
            // 
            // btn_Query
            // 
            this.btn_Query.Location = new System.Drawing.Point(227, 13);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(75, 23);
            this.btn_Query.TabIndex = 1;
            this.btn_Query.Text = "查询";
            this.btn_Query.Click += new System.EventHandler(this.btn_Query_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panel1);
            this.panelControl2.Controls.Add(this.btn_OK);
            this.panelControl2.Controls.Add(this.btn_AssignCode);
            this.panelControl2.Controls.Add(this.btn_Del);
            this.panelControl2.Controls.Add(this.btn_Modify);
            this.panelControl2.Controls.Add(this.btn_Add);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 495);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(800, 105);
            this.panelControl2.TabIndex = 1;
            this.panelControl2.Text = "panelControl2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelControl15);
            this.panel1.Controls.Add(this.txt_MachineID);
            this.panel1.Controls.Add(this.labelControl5);
            this.panel1.Controls.Add(this.ddl_BourseType);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.ddl_Center);
            this.panel1.Controls.Add(this.txt_MachineName);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(790, 50);
            this.panel1.TabIndex = 1;
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl15.Appearance.Options.UseForeColor = true;
            this.labelControl15.Location = new System.Drawing.Point(373, 18);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(7, 14);
            this.labelControl15.TabIndex = 32;
            this.labelControl15.Text = "*";
            // 
            // txt_MachineID
            // 
            this.txt_MachineID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_MachineID.Location = new System.Drawing.Point(82, 11);
            this.txt_MachineID.Name = "txt_MachineID";
            this.txt_MachineID.Size = new System.Drawing.Size(100, 21);
            this.txt_MachineID.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl5.Location = new System.Drawing.Point(12, 13);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 14);
            this.labelControl5.TabIndex = 30;
            this.labelControl5.Text = "撮合机编号:";
            // 
            // ddl_BourseType
            // 
            this.ddl_BourseType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddl_BourseType.Location = new System.Drawing.Point(665, 10);
            this.ddl_BourseType.Name = "ddl_BourseType";
            this.ddl_BourseType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddl_BourseType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ddl_BourseType.Size = new System.Drawing.Size(111, 21);
            this.ddl_BourseType.TabIndex = 3;
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl4.Location = new System.Drawing.Point(595, 14);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(64, 14);
            this.labelControl4.TabIndex = 28;
            this.labelControl4.Text = "所属交易所:";
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(197, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(64, 14);
            this.labelControl2.TabIndex = 24;
            this.labelControl2.Text = "撮合机名称:";
            // 
            // ddl_Center
            // 
            this.ddl_Center.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddl_Center.Location = new System.Drawing.Point(468, 10);
            this.ddl_Center.Name = "ddl_Center";
            this.ddl_Center.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddl_Center.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ddl_Center.Size = new System.Drawing.Size(111, 21);
            this.ddl_Center.TabIndex = 2;
            // 
            // txt_MachineName
            // 
            this.txt_MachineName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_MachineName.Location = new System.Drawing.Point(267, 11);
            this.txt_MachineName.Name = "txt_MachineName";
            this.txt_MachineName.Size = new System.Drawing.Size(100, 21);
            this.txt_MachineName.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Location = new System.Drawing.Point(386, 13);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(76, 14);
            this.labelControl3.TabIndex = 26;
            this.labelControl3.Text = "所属撮合中心:";
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.Location = new System.Drawing.Point(688, 70);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 6;
            this.btn_OK.Text = "确定";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_AssignCode
            // 
            this.btn_AssignCode.Location = new System.Drawing.Point(13, 70);
            this.btn_AssignCode.Name = "btn_AssignCode";
            this.btn_AssignCode.Size = new System.Drawing.Size(106, 23);
            this.btn_AssignCode.TabIndex = 2;
            this.btn_AssignCode.Text = "分配撮合代码";
            this.btn_AssignCode.Click += new System.EventHandler(this.btn_AssignCode_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Del.Location = new System.Drawing.Point(607, 70);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(75, 23);
            this.btn_Del.TabIndex = 5;
            this.btn_Del.Text = "删除";
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // btn_Modify
            // 
            this.btn_Modify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Modify.Location = new System.Drawing.Point(526, 70);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(75, 23);
            this.btn_Modify.TabIndex = 4;
            this.btn_Modify.Text = "修改";
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Add.Location = new System.Drawing.Point(445, 70);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 3;
            this.btn_Add.Text = "添加";
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridMachine);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 50);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(800, 445);
            this.panelControl3.TabIndex = 2;
            this.panelControl3.Text = "panelControl3";
            // 
            // gridMachine
            // 
            this.gridMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMachine.EmbeddedNavigator.Name = "";
            this.gridMachine.Location = new System.Drawing.Point(2, 2);
            this.gridMachine.MainView = this.ViewMachine;
            this.gridMachine.Name = "gridMachine";
            this.gridMachine.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBourseTypeID,
            this.ddlMatchCenterID});
            this.gridMachine.Size = new System.Drawing.Size(796, 441);
            this.gridMachine.TabIndex = 0;
            this.gridMachine.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewMachine});
            this.gridMachine.Click += new System.EventHandler(this.gridCenter_Click);
            // 
            // ViewMachine
            // 
            this.ViewMachine.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridCol_MatchMachineID,
            this.gridCol_MatchMachineName,
            this.gridCol_BourseTypeID,
            this.gridCol_MatchCenterID});
            this.ViewMachine.GridControl = this.gridMachine;
            this.ViewMachine.Name = "ViewMachine";
            this.ViewMachine.OptionsBehavior.Editable = false;
            this.ViewMachine.OptionsView.ShowGroupPanel = false;
            // 
            // gridCol_MatchMachineID
            // 
            this.gridCol_MatchMachineID.Caption = "撮合机编号";
            this.gridCol_MatchMachineID.FieldName = "MatchMachineID";
            this.gridCol_MatchMachineID.Name = "gridCol_MatchMachineID";
            this.gridCol_MatchMachineID.Visible = true;
            this.gridCol_MatchMachineID.VisibleIndex = 0;
            // 
            // gridCol_MatchMachineName
            // 
            this.gridCol_MatchMachineName.Caption = "撮合机名称";
            this.gridCol_MatchMachineName.FieldName = "MatchMachineName";
            this.gridCol_MatchMachineName.Name = "gridCol_MatchMachineName";
            this.gridCol_MatchMachineName.Visible = true;
            this.gridCol_MatchMachineName.VisibleIndex = 1;
            // 
            // gridCol_BourseTypeID
            // 
            this.gridCol_BourseTypeID.Caption = "所属交易所";
            this.gridCol_BourseTypeID.ColumnEdit = this.ddlBourseTypeID;
            this.gridCol_BourseTypeID.FieldName = "BourseTypeID";
            this.gridCol_BourseTypeID.Name = "gridCol_BourseTypeID";
            this.gridCol_BourseTypeID.Visible = true;
            this.gridCol_BourseTypeID.VisibleIndex = 3;
            // 
            // ddlBourseTypeID
            // 
            this.ddlBourseTypeID.AutoHeight = false;
            this.ddlBourseTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBourseTypeID.Name = "ddlBourseTypeID";
            this.ddlBourseTypeID.NullText = "";
            // 
            // gridCol_MatchCenterID
            // 
            this.gridCol_MatchCenterID.Caption = "所属撮合中心";
            this.gridCol_MatchCenterID.ColumnEdit = this.ddlMatchCenterID;
            this.gridCol_MatchCenterID.FieldName = "MatchCenterID";
            this.gridCol_MatchCenterID.Name = "gridCol_MatchCenterID";
            this.gridCol_MatchCenterID.Visible = true;
            this.gridCol_MatchCenterID.VisibleIndex = 2;
            // 
            // ddlMatchCenterID
            // 
            this.ddlMatchCenterID.AutoHeight = false;
            this.ddlMatchCenterID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlMatchCenterID.Name = "ddlMatchCenterID";
            this.ddlMatchCenterID.NullText = "";
            // 
            // MachineManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "MachineManage";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.MachineManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_Bourse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_MachineID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_BourseType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_Center.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_MachineName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlMatchCenterID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btn_Query;
        private DevExpress.XtraEditors.SimpleButton btn_AssignCode;
        private DevExpress.XtraEditors.SimpleButton btn_Del;
        private DevExpress.XtraEditors.SimpleButton btn_Modify;
        private DevExpress.XtraEditors.SimpleButton btn_Add;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation ucPageNavigation1;
        private DevExpress.XtraGrid.GridControl gridMachine;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewMachine;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_MatchMachineID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_MatchMachineName;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_BourseTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_MatchCenterID;
        private DevExpress.XtraEditors.ComboBoxEdit ddl_Bourse;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBourseTypeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlMatchCenterID;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.TextEdit txt_MachineID;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit ddl_BourseType;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit ddl_Center;
        private DevExpress.XtraEditors.TextEdit txt_MachineName;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}
