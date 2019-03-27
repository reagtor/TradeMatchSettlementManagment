namespace ManagementCenterConsole.UI.MatchCenterManage
{
    partial class CenterManage
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
            this.btn_Refresh = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.txt_CenterID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txt_cuoheService = new DevExpress.XtraEditors.TextEdit();
            this.txt_CenterIP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Port = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txt_xiadanService = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txt_CenterName = new DevExpress.XtraEditors.TextEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Modify = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Del = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridCenter = new DevExpress.XtraGrid.GridControl();
            this.ViewCenter = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridCol_MatchCenterID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_MatchCenterName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_IP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_Port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_State = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_XiaDanService = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_CuoHeService = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CenterID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_cuoheService.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CenterIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Port.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_xiadanService.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CenterName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewCenter)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Refresh);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(800, 43);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Text = "panelControl1";
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(110, 9);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_Refresh.TabIndex = 0;
            this.btn_Refresh.Text = "刷新状态";
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(16, 14);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(72, 14);
            this.labelControl6.TabIndex = 0;
            this.labelControl6.Text = "撮合中心列表";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panelControl5);
            this.panelControl2.Controls.Add(this.panelControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 467);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(800, 133);
            this.panelControl2.TabIndex = 1;
            this.panelControl2.Text = "panelControl2";
            // 
            // panelControl5
            // 
            this.panelControl5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl5.Controls.Add(this.labelControl9);
            this.panelControl5.Controls.Add(this.labelControl8);
            this.panelControl5.Controls.Add(this.labelControl14);
            this.panelControl5.Controls.Add(this.txt_CenterID);
            this.panelControl5.Controls.Add(this.labelControl7);
            this.panelControl5.Controls.Add(this.labelControl1);
            this.panelControl5.Controls.Add(this.labelControl3);
            this.panelControl5.Controls.Add(this.txt_cuoheService);
            this.panelControl5.Controls.Add(this.txt_CenterIP);
            this.panelControl5.Controls.Add(this.labelControl5);
            this.panelControl5.Controls.Add(this.txt_Port);
            this.panelControl5.Controls.Add(this.labelControl2);
            this.panelControl5.Controls.Add(this.txt_xiadanService);
            this.panelControl5.Controls.Add(this.labelControl4);
            this.panelControl5.Controls.Add(this.txt_CenterName);
            this.panelControl5.Location = new System.Drawing.Point(4, 7);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(792, 82);
            this.panelControl5.TabIndex = 1;
            this.panelControl5.Text = "panelControl5";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl9.Appearance.Options.UseForeColor = true;
            this.labelControl9.Location = new System.Drawing.Point(409, 53);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(7, 14);
            this.labelControl9.TabIndex = 43;
            this.labelControl9.Text = "*";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Location = new System.Drawing.Point(409, 16);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(7, 14);
            this.labelControl8.TabIndex = 42;
            this.labelControl8.Text = "*";
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.Location = new System.Drawing.Point(219, 16);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(7, 14);
            this.labelControl14.TabIndex = 41;
            this.labelControl14.Text = "*";
            // 
            // txt_CenterID
            // 
            this.txt_CenterID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_CenterID.Location = new System.Drawing.Point(94, 46);
            this.txt_CenterID.Name = "txt_CenterID";
            this.txt_CenterID.Size = new System.Drawing.Size(119, 21);
            this.txt_CenterID.TabIndex = 3;
            // 
            // labelControl7
            // 
            this.labelControl7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl7.Location = new System.Drawing.Point(12, 49);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(76, 14);
            this.labelControl7.TabIndex = 20;
            this.labelControl7.Text = "撮合中心编号:";
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(76, 14);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "撮合中心名称:";
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Location = new System.Drawing.Point(243, 49);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(28, 14);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "端口:";
            // 
            // txt_cuoheService
            // 
            this.txt_cuoheService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_cuoheService.Location = new System.Drawing.Point(508, 9);
            this.txt_cuoheService.Name = "txt_cuoheService";
            this.txt_cuoheService.Size = new System.Drawing.Size(126, 21);
            this.txt_cuoheService.TabIndex = 2;
            // 
            // txt_CenterIP
            // 
            this.txt_CenterIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_CenterIP.Location = new System.Drawing.Point(277, 9);
            this.txt_CenterIP.Name = "txt_CenterIP";
            this.txt_CenterIP.Size = new System.Drawing.Size(126, 21);
            this.txt_CenterIP.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl5.Location = new System.Drawing.Point(426, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(76, 14);
            this.labelControl5.TabIndex = 18;
            this.labelControl5.Text = "撮合服务名称:";
            // 
            // txt_Port
            // 
            this.txt_Port.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Port.Location = new System.Drawing.Point(277, 46);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(126, 21);
            this.txt_Port.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(232, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(39, 14);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "IP地址:";
            // 
            // txt_xiadanService
            // 
            this.txt_xiadanService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_xiadanService.Location = new System.Drawing.Point(508, 46);
            this.txt_xiadanService.Name = "txt_xiadanService";
            this.txt_xiadanService.Size = new System.Drawing.Size(126, 21);
            this.txt_xiadanService.TabIndex = 5;
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl4.Location = new System.Drawing.Point(426, 49);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(76, 14);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "下单服务名称:";
            // 
            // txt_CenterName
            // 
            this.txt_CenterName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_CenterName.Location = new System.Drawing.Point(94, 9);
            this.txt_CenterName.Name = "txt_CenterName";
            this.txt_CenterName.Size = new System.Drawing.Size(119, 21);
            this.txt_CenterName.TabIndex = 0;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.btn_Add);
            this.panelControl4.Controls.Add(this.btn_OK);
            this.panelControl4.Controls.Add(this.btn_Modify);
            this.panelControl4.Controls.Add(this.btn_Del);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(2, 90);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(796, 41);
            this.panelControl4.TabIndex = 24;
            this.panelControl4.Text = "panelControl4";
            // 
            // btn_Add
            // 
            this.btn_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Add.Location = new System.Drawing.Point(300, 5);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 0;
            this.btn_Add.Text = "添加";
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.Location = new System.Drawing.Point(559, 5);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 3;
            this.btn_OK.Text = " 确定";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Modify
            // 
            this.btn_Modify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Modify.Location = new System.Drawing.Point(383, 5);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(75, 23);
            this.btn_Modify.TabIndex = 1;
            this.btn_Modify.Text = "修改";
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Del.Location = new System.Drawing.Point(464, 5);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(75, 23);
            this.btn_Del.TabIndex = 2;
            this.btn_Del.Text = "删除";
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridCenter);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 43);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(800, 424);
            this.panelControl3.TabIndex = 2;
            this.panelControl3.Text = "panelControl3";
            // 
            // gridCenter
            // 
            this.gridCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCenter.EmbeddedNavigator.Name = "";
            this.gridCenter.Location = new System.Drawing.Point(2, 2);
            this.gridCenter.MainView = this.ViewCenter;
            this.gridCenter.Name = "gridCenter";
            this.gridCenter.Size = new System.Drawing.Size(796, 420);
            this.gridCenter.TabIndex = 0;
            this.gridCenter.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewCenter});
            this.gridCenter.Click += new System.EventHandler(this.gridCenter_Click);
            // 
            // ViewCenter
            // 
            this.ViewCenter.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridCol_MatchCenterID,
            this.gridCol_MatchCenterName,
            this.gridCol_IP,
            this.gridCol_Port,
            this.gridCol_State,
            this.gridCol_XiaDanService,
            this.gridCol_CuoHeService});
            this.ViewCenter.GridControl = this.gridCenter;
            this.ViewCenter.Name = "ViewCenter";
            this.ViewCenter.OptionsBehavior.Editable = false;
            this.ViewCenter.OptionsView.ShowGroupPanel = false;
            this.ViewCenter.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.ViewCenter_CustomDrawCell);
            // 
            // gridCol_MatchCenterID
            // 
            this.gridCol_MatchCenterID.Caption = "撮合中心编号";
            this.gridCol_MatchCenterID.FieldName = "MatchCenterID";
            this.gridCol_MatchCenterID.Name = "gridCol_MatchCenterID";
            this.gridCol_MatchCenterID.Visible = true;
            this.gridCol_MatchCenterID.VisibleIndex = 1;
            // 
            // gridCol_MatchCenterName
            // 
            this.gridCol_MatchCenterName.Caption = "撮合中心名称";
            this.gridCol_MatchCenterName.FieldName = "MatchCenterName";
            this.gridCol_MatchCenterName.Name = "gridCol_MatchCenterName";
            this.gridCol_MatchCenterName.Visible = true;
            this.gridCol_MatchCenterName.VisibleIndex = 0;
            // 
            // gridCol_IP
            // 
            this.gridCol_IP.Caption = "IP地址";
            this.gridCol_IP.FieldName = "IP";
            this.gridCol_IP.Name = "gridCol_IP";
            this.gridCol_IP.Visible = true;
            this.gridCol_IP.VisibleIndex = 2;
            // 
            // gridCol_Port
            // 
            this.gridCol_Port.Caption = "端口";
            this.gridCol_Port.FieldName = "Port";
            this.gridCol_Port.Name = "gridCol_Port";
            this.gridCol_Port.Visible = true;
            this.gridCol_Port.VisibleIndex = 3;
            // 
            // gridCol_State
            // 
            this.gridCol_State.Caption = "连接状态";
            this.gridCol_State.FieldName = "State";
            this.gridCol_State.Name = "gridCol_State";
            this.gridCol_State.Visible = true;
            this.gridCol_State.VisibleIndex = 4;
            // 
            // gridCol_XiaDanService
            // 
            this.gridCol_XiaDanService.Caption = "下单服务名称";
            this.gridCol_XiaDanService.FieldName = "XiaDanService";
            this.gridCol_XiaDanService.Name = "gridCol_XiaDanService";
            this.gridCol_XiaDanService.Visible = true;
            this.gridCol_XiaDanService.VisibleIndex = 6;
            // 
            // gridCol_CuoHeService
            // 
            this.gridCol_CuoHeService.Caption = "撮合服务名称";
            this.gridCol_CuoHeService.FieldName = "CuoHeService";
            this.gridCol_CuoHeService.Name = "gridCol_CuoHeService";
            this.gridCol_CuoHeService.Visible = true;
            this.gridCol_CuoHeService.VisibleIndex = 5;
            // 
            // CenterManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "CenterManage";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.CenterManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            this.panelControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CenterID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_cuoheService.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CenterIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Port.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_xiadanService.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CenterName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewCenter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.SimpleButton btn_Del;
        private DevExpress.XtraEditors.SimpleButton btn_Modify;
        private DevExpress.XtraEditors.SimpleButton btn_Add;
        private DevExpress.XtraGrid.GridControl gridCenter;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewCenter;
        private DevExpress.XtraEditors.SimpleButton btn_Refresh;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_MatchCenterID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_MatchCenterName;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_IP;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_Port;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_State;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_XiaDanService;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_CuoHeService;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.TextEdit txt_CenterID;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txt_cuoheService;
        private DevExpress.XtraEditors.TextEdit txt_CenterIP;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txt_Port;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txt_xiadanService;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txt_CenterName;
    }
}
