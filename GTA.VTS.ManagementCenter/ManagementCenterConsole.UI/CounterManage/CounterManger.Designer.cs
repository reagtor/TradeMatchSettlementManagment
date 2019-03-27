namespace ManagementCenterConsole.UI.CounterManage
{
    partial class CounterManger
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
            this.btn_Refresh = new DevExpress.XtraEditors.SimpleButton();
            this.ucPageNavigation1 = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.btn_Query = new DevExpress.XtraEditors.SimpleButton();
            this.txt_IP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Name = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.Btn_Del = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Update = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridCounter = new DevExpress.XtraGrid.GridControl();
            this.ViewCounter = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridCol_CouterID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_IP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_Port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_MaxValues = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_State = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_XiaDanServiceName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_QueryServiceName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_AccountServiceName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcAccountServicePort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcQueryServicePort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcSendPort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcSendServiceName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_IP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewCounter)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Refresh);
            this.panelControl1.Controls.Add(this.ucPageNavigation1);
            this.panelControl1.Controls.Add(this.btn_Query);
            this.panelControl1.Controls.Add(this.txt_IP);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.txt_Name);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(794, 58);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(396, 20);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(69, 23);
            this.btn_Refresh.TabIndex = 3;
            this.btn_Refresh.Text = "刷新状态";
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // ucPageNavigation1
            // 
            this.ucPageNavigation1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPageNavigation1.CurrentPage = 0;
            this.ucPageNavigation1.Location = new System.Drawing.Point(471, 16);
            this.ucPageNavigation1.Name = "ucPageNavigation1";
            this.ucPageNavigation1.PageCount = 0;
            this.ucPageNavigation1.Size = new System.Drawing.Size(319, 28);
            this.ucPageNavigation1.TabIndex = 4;
            // 
            // btn_Query
            // 
            this.btn_Query.Location = new System.Drawing.Point(321, 20);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(69, 23);
            this.btn_Query.TabIndex = 2;
            this.btn_Query.Text = "查询";
            this.btn_Query.Click += new System.EventHandler(this.Btn_Query_Click);
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(215, 20);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(100, 21);
            this.txt_IP.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(171, 23);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(39, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "IP地址:";
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(71, 20);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(90, 21);
            this.txt_Name.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "柜台名称:";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.Btn_Del);
            this.panelControl2.Controls.Add(this.Btn_Update);
            this.panelControl2.Controls.Add(this.Btn_Add);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 517);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(794, 51);
            this.panelControl2.TabIndex = 1;
            this.panelControl2.Text = "panelControl2";
            // 
            // Btn_Del
            // 
            this.Btn_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Del.Location = new System.Drawing.Point(707, 16);
            this.Btn_Del.Name = "Btn_Del";
            this.Btn_Del.Size = new System.Drawing.Size(75, 23);
            this.Btn_Del.TabIndex = 2;
            this.Btn_Del.Text = "删除";
            this.Btn_Del.Click += new System.EventHandler(this.Btn_Del_Click);
            // 
            // Btn_Update
            // 
            this.Btn_Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Update.Location = new System.Drawing.Point(626, 16);
            this.Btn_Update.Name = "Btn_Update";
            this.Btn_Update.Size = new System.Drawing.Size(75, 23);
            this.Btn_Update.TabIndex = 1;
            this.Btn_Update.Text = "修改";
            this.Btn_Update.Click += new System.EventHandler(this.Btn_Update_Click);
            // 
            // Btn_Add
            // 
            this.Btn_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Add.Location = new System.Drawing.Point(545, 16);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(75, 23);
            this.Btn_Add.TabIndex = 0;
            this.Btn_Add.Text = "添加";
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridCounter);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 58);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(794, 459);
            this.panelControl3.TabIndex = 2;
            this.panelControl3.Text = "panelControl3";
            // 
            // gridCounter
            // 
            this.gridCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCounter.EmbeddedNavigator.Name = "";
            this.gridCounter.Location = new System.Drawing.Point(2, 2);
            this.gridCounter.MainView = this.ViewCounter;
            this.gridCounter.Name = "gridCounter";
            this.gridCounter.Size = new System.Drawing.Size(790, 455);
            this.gridCounter.TabIndex = 0;
            this.gridCounter.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewCounter});
            this.gridCounter.DoubleClick += new System.EventHandler(this.gridCounter_DoubleClick);
            this.gridCounter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridCounter_MouseMove);
            // 
            // ViewCounter
            // 
            this.ViewCounter.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridCol_CouterID,
            this.gridCol_name,
            this.gridCol_IP,
            this.gridCol_Port,
            this.gridCol_MaxValues,
            this.gridCol_State,
            this.gridCol_XiaDanServiceName,
            this.gridCol_QueryServiceName,
            this.gridCol_AccountServiceName,
            this.gdcAccountServicePort,
            this.gdcQueryServicePort,
            this.gdcSendPort,
            this.gdcSendServiceName});
            this.ViewCounter.GridControl = this.gridCounter;
            this.ViewCounter.Name = "ViewCounter";
            this.ViewCounter.OptionsBehavior.Editable = false;
            this.ViewCounter.OptionsCustomization.AllowGroup = false;
            this.ViewCounter.OptionsMenu.EnableColumnMenu = false;
            this.ViewCounter.OptionsMenu.EnableFooterMenu = false;
            this.ViewCounter.OptionsMenu.EnableGroupPanelMenu = false;
            this.ViewCounter.OptionsView.ShowGroupPanel = false;
            this.ViewCounter.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.ViewCounter_CustomDrawCell);
            // 
            // gridCol_CouterID
            // 
            this.gridCol_CouterID.Caption = "柜台编号";
            this.gridCol_CouterID.FieldName = "CouterID";
            this.gridCol_CouterID.Name = "gridCol_CouterID";
            this.gridCol_CouterID.Visible = true;
            this.gridCol_CouterID.VisibleIndex = 0;
            this.gridCol_CouterID.Width = 47;
            // 
            // gridCol_name
            // 
            this.gridCol_name.Caption = "柜台名称";
            this.gridCol_name.FieldName = "name";
            this.gridCol_name.Name = "gridCol_name";
            this.gridCol_name.Visible = true;
            this.gridCol_name.VisibleIndex = 1;
            this.gridCol_name.Width = 60;
            // 
            // gridCol_IP
            // 
            this.gridCol_IP.Caption = "IP地址";
            this.gridCol_IP.FieldName = "IP";
            this.gridCol_IP.Name = "gridCol_IP";
            this.gridCol_IP.Visible = true;
            this.gridCol_IP.VisibleIndex = 2;
            this.gridCol_IP.Width = 52;
            // 
            // gridCol_Port
            // 
            this.gridCol_Port.Caption = "端口";
            this.gridCol_Port.FieldName = "XiaDanServicePort";
            this.gridCol_Port.Name = "gridCol_Port";
            this.gridCol_Port.Visible = true;
            this.gridCol_Port.VisibleIndex = 5;
            this.gridCol_Port.Width = 37;
            // 
            // gridCol_MaxValues
            // 
            this.gridCol_MaxValues.Caption = "最大容量(人数)";
            this.gridCol_MaxValues.FieldName = "MaxValues";
            this.gridCol_MaxValues.Name = "gridCol_MaxValues";
            this.gridCol_MaxValues.Width = 89;
            // 
            // gridCol_State
            // 
            this.gridCol_State.Caption = "状态";
            this.gridCol_State.FieldName = "State";
            this.gridCol_State.Name = "gridCol_State";
            this.gridCol_State.Visible = true;
            this.gridCol_State.VisibleIndex = 3;
            this.gridCol_State.Width = 44;
            // 
            // gridCol_XiaDanServiceName
            // 
            this.gridCol_XiaDanServiceName.Caption = "下单服务名称";
            this.gridCol_XiaDanServiceName.FieldName = "XiaDanServiceName";
            this.gridCol_XiaDanServiceName.Name = "gridCol_XiaDanServiceName";
            this.gridCol_XiaDanServiceName.Visible = true;
            this.gridCol_XiaDanServiceName.VisibleIndex = 4;
            this.gridCol_XiaDanServiceName.Width = 88;
            // 
            // gridCol_QueryServiceName
            // 
            this.gridCol_QueryServiceName.Caption = "查询服务名称";
            this.gridCol_QueryServiceName.FieldName = "QueryServiceName";
            this.gridCol_QueryServiceName.Name = "gridCol_QueryServiceName";
            this.gridCol_QueryServiceName.Visible = true;
            this.gridCol_QueryServiceName.VisibleIndex = 8;
            this.gridCol_QueryServiceName.Width = 86;
            // 
            // gridCol_AccountServiceName
            // 
            this.gridCol_AccountServiceName.Caption = "帐号服务名称";
            this.gridCol_AccountServiceName.FieldName = "AccountServiceName";
            this.gridCol_AccountServiceName.Name = "gridCol_AccountServiceName";
            this.gridCol_AccountServiceName.Visible = true;
            this.gridCol_AccountServiceName.VisibleIndex = 10;
            this.gridCol_AccountServiceName.Width = 71;
            // 
            // gdcAccountServicePort
            // 
            this.gdcAccountServicePort.Caption = "端口";
            this.gdcAccountServicePort.FieldName = "AccountServicePort";
            this.gdcAccountServicePort.Name = "gdcAccountServicePort";
            this.gdcAccountServicePort.Visible = true;
            this.gdcAccountServicePort.VisibleIndex = 11;
            this.gdcAccountServicePort.Width = 39;
            // 
            // gdcQueryServicePort
            // 
            this.gdcQueryServicePort.Caption = "端口";
            this.gdcQueryServicePort.FieldName = "QueryServicePort";
            this.gdcQueryServicePort.Name = "gdcQueryServicePort";
            this.gdcQueryServicePort.Visible = true;
            this.gdcQueryServicePort.VisibleIndex = 9;
            this.gdcQueryServicePort.Width = 37;
            // 
            // gdcSendPort
            // 
            this.gdcSendPort.Caption = "端口";
            this.gdcSendPort.FieldName = "SendPort";
            this.gdcSendPort.Name = "gdcSendPort";
            this.gdcSendPort.Visible = true;
            this.gdcSendPort.VisibleIndex = 7;
            this.gdcSendPort.Width = 34;
            // 
            // gdcSendServiceName
            // 
            this.gdcSendServiceName.Caption = "回送服务名称";
            this.gdcSendServiceName.FieldName = "SendServiceName";
            this.gdcSendServiceName.Name = "gdcSendServiceName";
            this.gdcSendServiceName.Visible = true;
            this.gdcSendServiceName.VisibleIndex = 6;
            this.gdcSendServiceName.Width = 85;
            // 
            // CounterManger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 568);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CounterManger";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "清算柜台管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.CounterManger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_IP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewCounter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gridCounter;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewCounter;
        private DevExpress.XtraEditors.SimpleButton Btn_Del;
        private DevExpress.XtraEditors.SimpleButton Btn_Update;
        private DevExpress.XtraEditors.SimpleButton Btn_Add;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_CouterID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_name;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_IP;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_Port;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_MaxValues;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_State;
        private DevExpress.XtraEditors.TextEdit txt_Name;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Query;
        private DevExpress.XtraEditors.TextEdit txt_IP;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation ucPageNavigation1;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_XiaDanServiceName;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_QueryServiceName;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_AccountServiceName;
        private DevExpress.XtraEditors.SimpleButton btn_Refresh;
        private DevExpress.XtraGrid.Columns.GridColumn gdcAccountServicePort;
        private DevExpress.XtraGrid.Columns.GridColumn gdcQueryServicePort;
        private DevExpress.XtraGrid.Columns.GridColumn gdcSendPort;
        private DevExpress.XtraGrid.Columns.GridColumn gdcSendServiceName;
    }
}