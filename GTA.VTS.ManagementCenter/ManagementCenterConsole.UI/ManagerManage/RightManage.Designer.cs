namespace ManagementCenterConsole.UI.ManagerManage
{
    partial class RightManage
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
            this.txt_RightName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ucPageNavigation1 = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.Btn_Query = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Del = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Update = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridRightGroup = new DevExpress.XtraGrid.GridControl();
            this.ViewRightGroup = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridCol_ManagerGroupID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_ManagerGroupName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_RightName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRightGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewRightGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txt_RightName);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.ucPageNavigation1);
            this.panelControl1.Controls.Add(this.Btn_Query);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(863, 61);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Text = "panelControl1";
            // 
            // txt_RightName
            // 
            this.txt_RightName.Location = new System.Drawing.Point(88, 22);
            this.txt_RightName.Name = "txt_RightName";
            this.txt_RightName.Size = new System.Drawing.Size(143, 21);
            this.txt_RightName.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 17;
            this.labelControl1.Text = "权限组名称:";
            // 
            // ucPageNavigation1
            // 
            this.ucPageNavigation1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPageNavigation1.CurrentPage = 0;
            this.ucPageNavigation1.Location = new System.Drawing.Point(539, 15);
            this.ucPageNavigation1.Name = "ucPageNavigation1";
            this.ucPageNavigation1.PageCount = 0;
            this.ucPageNavigation1.Size = new System.Drawing.Size(319, 28);
            this.ucPageNavigation1.TabIndex = 2;
            // 
            // Btn_Query
            // 
            this.Btn_Query.Location = new System.Drawing.Point(247, 20);
            this.Btn_Query.Name = "Btn_Query";
            this.Btn_Query.Size = new System.Drawing.Size(75, 23);
            this.Btn_Query.TabIndex = 1;
            this.Btn_Query.Text = "查询";
            this.Btn_Query.Click += new System.EventHandler(this.Btn_Query_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_Del);
            this.panelControl2.Controls.Add(this.btn_Update);
            this.panelControl2.Controls.Add(this.btn_Add);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 570);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(863, 57);
            this.panelControl2.TabIndex = 3;
            this.panelControl2.Text = "panelControl2";
            // 
            // btn_Del
            // 
            this.btn_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Del.Location = new System.Drawing.Point(776, 22);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(75, 23);
            this.btn_Del.TabIndex = 2;
            this.btn_Del.Text = "删除";
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // btn_Update
            // 
            this.btn_Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Update.Location = new System.Drawing.Point(695, 22);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(75, 23);
            this.btn_Update.TabIndex = 1;
            this.btn_Update.Text = "修改";
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Add.Location = new System.Drawing.Point(614, 22);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 0;
            this.btn_Add.Text = "添加";
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridRightGroup);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 61);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(863, 509);
            this.panelControl3.TabIndex = 4;
            this.panelControl3.Text = "panelControl3";
            // 
            // gridRightGroup
            // 
            this.gridRightGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRightGroup.EmbeddedNavigator.Name = "";
            this.gridRightGroup.Location = new System.Drawing.Point(2, 2);
            this.gridRightGroup.MainView = this.ViewRightGroup;
            this.gridRightGroup.Name = "gridRightGroup";
            this.gridRightGroup.Size = new System.Drawing.Size(859, 505);
            this.gridRightGroup.TabIndex = 0;
            this.gridRightGroup.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewRightGroup});
            this.gridRightGroup.DoubleClick += new System.EventHandler(this.gridRightGroup_DoubleClick);
            this.gridRightGroup.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridRightGroup_MouseMove);
            // 
            // ViewRightGroup
            // 
            this.ViewRightGroup.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridCol_ManagerGroupID,
            this.gridCol_ManagerGroupName});
            this.ViewRightGroup.GridControl = this.gridRightGroup;
            this.ViewRightGroup.Name = "ViewRightGroup";
            this.ViewRightGroup.OptionsBehavior.Editable = false;
            this.ViewRightGroup.OptionsView.ShowGroupPanel = false;
            // 
            // gridCol_ManagerGroupID
            // 
            this.gridCol_ManagerGroupID.Caption = "权限组编号";
            this.gridCol_ManagerGroupID.FieldName = "ManagerGroupID";
            this.gridCol_ManagerGroupID.Name = "gridCol_ManagerGroupID";
            this.gridCol_ManagerGroupID.Visible = true;
            this.gridCol_ManagerGroupID.VisibleIndex = 0;
            this.gridCol_ManagerGroupID.Width = 143;
            // 
            // gridCol_ManagerGroupName
            // 
            this.gridCol_ManagerGroupName.Caption = "权限组名称";
            this.gridCol_ManagerGroupName.FieldName = "ManagerGroupName";
            this.gridCol_ManagerGroupName.Name = "gridCol_ManagerGroupName";
            this.gridCol_ManagerGroupName.Visible = true;
            this.gridCol_ManagerGroupName.VisibleIndex = 1;
            this.gridCol_ManagerGroupName.Width = 695;
            // 
            // RightManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 627);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "RightManage";
            this.Text = "管理员权限组管理";
            this.Load += new System.EventHandler(this.RightManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_RightName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRightGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewRightGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txt_RightName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation ucPageNavigation1;
        private DevExpress.XtraEditors.SimpleButton Btn_Query;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_Del;
        private DevExpress.XtraEditors.SimpleButton btn_Update;
        private DevExpress.XtraEditors.SimpleButton btn_Add;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gridRightGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewRightGroup;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_ManagerGroupID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_ManagerGroupName;

    }
}