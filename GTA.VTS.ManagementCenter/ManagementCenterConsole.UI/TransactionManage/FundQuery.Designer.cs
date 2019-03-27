namespace ManagementCenterConsole.UI.TransactionManage
{
    partial class FundQuery
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
            this.txt_LoginName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btn_Query = new DevExpress.XtraEditors.SimpleButton();
            this.txt_TransID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ucPageNavigation1 = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_AddFund = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridFund = new DevExpress.XtraGrid.GridControl();
            this.ViewFund = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridCol_AddFundID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_UserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_ManagerID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_LoginName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_USNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_RMBNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_HKNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_AddTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol_Remark = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_LoginName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_TransID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFund)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewFund)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txt_LoginName);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.btn_Query);
            this.panelControl1.Controls.Add(this.txt_TransID);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.ucPageNavigation1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(807, 59);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Text = "panelControl1";
            // 
            // txt_LoginName
            // 
            this.txt_LoginName.Location = new System.Drawing.Point(254, 19);
            this.txt_LoginName.Name = "txt_LoginName";
            this.txt_LoginName.Size = new System.Drawing.Size(80, 21);
            this.txt_LoginName.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(176, 22);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "管理员账号：";
            // 
            // btn_Query
            // 
            this.btn_Query.Location = new System.Drawing.Point(343, 19);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(75, 23);
            this.btn_Query.TabIndex = 2;
            this.btn_Query.Text = "查询";
            this.btn_Query.Click += new System.EventHandler(this.btn_Query_Click);
            // 
            // txt_TransID
            // 
            this.txt_TransID.Location = new System.Drawing.Point(90, 19);
            this.txt_TransID.Name = "txt_TransID";
            this.txt_TransID.Size = new System.Drawing.Size(80, 21);
            this.txt_TransID.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "交易员账号：";
            // 
            // ucPageNavigation1
            // 
            this.ucPageNavigation1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPageNavigation1.CurrentPage = 0;
            this.ucPageNavigation1.Location = new System.Drawing.Point(465, 19);
            this.ucPageNavigation1.Name = "ucPageNavigation1";
            this.ucPageNavigation1.PageCount = 0;
            this.ucPageNavigation1.Size = new System.Drawing.Size(319, 28);
            this.ucPageNavigation1.TabIndex = 3;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_AddFund);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 502);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(807, 56);
            this.panelControl2.TabIndex = 1;
            this.panelControl2.Text = "panelControl2";
            // 
            // btn_AddFund
            // 
            this.btn_AddFund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_AddFund.Location = new System.Drawing.Point(669, 16);
            this.btn_AddFund.Name = "btn_AddFund";
            this.btn_AddFund.Size = new System.Drawing.Size(126, 23);
            this.btn_AddFund.TabIndex = 0;
            this.btn_AddFund.Text = "追加资金";
            this.btn_AddFund.Click += new System.EventHandler(this.btn_AddFund_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridFund);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 59);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(807, 443);
            this.panelControl3.TabIndex = 2;
            this.panelControl3.Text = "panelControl3";
            // 
            // gridFund
            // 
            this.gridFund.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFund.EmbeddedNavigator.Name = "";
            this.gridFund.Location = new System.Drawing.Point(2, 2);
            this.gridFund.MainView = this.ViewFund;
            this.gridFund.Name = "gridFund";
            this.gridFund.Size = new System.Drawing.Size(803, 439);
            this.gridFund.TabIndex = 0;
            this.gridFund.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewFund});
            this.gridFund.DoubleClick += new System.EventHandler(this.gridFund_DoubleClick);
            this.gridFund.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridFund_MouseMove);
            // 
            // ViewFund
            // 
            this.ViewFund.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridCol_AddFundID,
            this.gridCol_UserID,
            this.gridCol_ManagerID,
            this.gridCol_LoginName,
            this.gridCol_USNumber,
            this.gridCol_RMBNumber,
            this.gridCol_HKNumber,
            this.gridCol_AddTime,
            this.gridCol_Remark});
            this.ViewFund.GridControl = this.gridFund;
            this.ViewFund.Name = "ViewFund";
            this.ViewFund.OptionsBehavior.Editable = false;
            this.ViewFund.OptionsView.ShowGroupPanel = false;
            // 
            // gridCol_AddFundID
            // 
            this.gridCol_AddFundID.Caption = "资金流水号";
            this.gridCol_AddFundID.FieldName = "AddFundID";
            this.gridCol_AddFundID.Name = "gridCol_AddFundID";
            this.gridCol_AddFundID.Visible = true;
            this.gridCol_AddFundID.VisibleIndex = 0;
            // 
            // gridCol_UserID
            // 
            this.gridCol_UserID.Caption = "交易员账号";
            this.gridCol_UserID.FieldName = "UserID";
            this.gridCol_UserID.Name = "gridCol_UserID";
            this.gridCol_UserID.Visible = true;
            this.gridCol_UserID.VisibleIndex = 1;
            // 
            // gridCol_ManagerID
            // 
            this.gridCol_ManagerID.Caption = "管理员ID";
            this.gridCol_ManagerID.FieldName = "ManagerID";
            this.gridCol_ManagerID.Name = "gridCol_ManagerID";
            // 
            // gridCol_LoginName
            // 
            this.gridCol_LoginName.Caption = "管理员账号";
            this.gridCol_LoginName.FieldName = "LoginName";
            this.gridCol_LoginName.Name = "gridCol_LoginName";
            this.gridCol_LoginName.Visible = true;
            this.gridCol_LoginName.VisibleIndex = 2;
            // 
            // gridCol_USNumber
            // 
            this.gridCol_USNumber.Caption = "美元";
            this.gridCol_USNumber.FieldName = "USNumber";
            this.gridCol_USNumber.GroupFormat.FormatString = "f2";
            this.gridCol_USNumber.Name = "gridCol_USNumber";
            this.gridCol_USNumber.Visible = true;
            this.gridCol_USNumber.VisibleIndex = 5;
            // 
            // gridCol_RMBNumber
            // 
            this.gridCol_RMBNumber.Caption = "人民币";
            this.gridCol_RMBNumber.FieldName = "RMBNumber";
            this.gridCol_RMBNumber.Name = "gridCol_RMBNumber";
            this.gridCol_RMBNumber.Visible = true;
            this.gridCol_RMBNumber.VisibleIndex = 3;
            // 
            // gridCol_HKNumber
            // 
            this.gridCol_HKNumber.Caption = "港币";
            this.gridCol_HKNumber.FieldName = "HKNumber";
            this.gridCol_HKNumber.Name = "gridCol_HKNumber";
            this.gridCol_HKNumber.Visible = true;
            this.gridCol_HKNumber.VisibleIndex = 4;
            // 
            // gridCol_AddTime
            // 
            this.gridCol_AddTime.Caption = "追加时间";
            this.gridCol_AddTime.FieldName = "AddTime";
            this.gridCol_AddTime.Name = "gridCol_AddTime";
            this.gridCol_AddTime.Visible = true;
            this.gridCol_AddTime.VisibleIndex = 6;
            // 
            // gridCol_Remark
            // 
            this.gridCol_Remark.Caption = "备注";
            this.gridCol_Remark.FieldName = "Remark";
            this.gridCol_Remark.Name = "gridCol_Remark";
            // 
            // FundQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "FundQuery";
            this.Size = new System.Drawing.Size(807, 558);
            this.Load += new System.EventHandler(this.FundQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_LoginName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_TransID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFund)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewFund)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txt_TransID;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation ucPageNavigation1;
        private DevExpress.XtraEditors.SimpleButton btn_Query;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gridFund;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewFund;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_AddFundID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_ManagerID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_UserID;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_USNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_RMBNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_HKNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_AddTime;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_Remark;
        private DevExpress.XtraEditors.SimpleButton btn_AddFund;
        private DevExpress.XtraEditors.TextEdit txt_LoginName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol_LoginName;
    }
}