namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    partial class FuseTimesectionManageUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuseTimesectionManageUI));
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labCommodityCode = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.tmEndTime = new DevExpress.XtraEditors.TimeEdit();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.tmStartTime = new DevExpress.XtraEditors.TimeEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gdFuseTimesectionResult = new DevExpress.XtraGrid.GridControl();
            this.gdFuseTimesectionSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcCommodityCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcEndTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdFuseTimesectionResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdFuseTimesectionSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(17, 14);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(76, 14);
            this.labelControl14.TabIndex = 1;
            this.labelControl14.Text = "允许起始时间:";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.labCommodityCode);
            this.panelControl3.Controls.Add(this.labelControl9);
            this.panelControl3.Location = new System.Drawing.Point(12, 12);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(437, 39);
            this.panelControl3.TabIndex = 51;
            this.panelControl3.Text = "panelControl3";
            // 
            // labCommodityCode
            // 
            this.labCommodityCode.Location = new System.Drawing.Point(49, 11);
            this.labCommodityCode.Name = "labCommodityCode";
            this.labCommodityCode.Size = new System.Drawing.Size(70, 14);
            this.labCommodityCode.TabIndex = 17;
            this.labCommodityCode.Text = "labelControl4";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(15, 11);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(28, 14);
            this.labelControl9.TabIndex = 16;
            this.labelControl9.Text = "代码:";
            // 
            // tmEndTime
            // 
            this.tmEndTime.EditValue = new System.DateTime(2008, 12, 5, 0, 0, 0, 0);
            this.tmEndTime.Location = new System.Drawing.Point(315, 11);
            this.tmEndTime.Name = "tmEndTime";
            this.tmEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.tmEndTime.Properties.Mask.EditMask = "HH:mm";
            this.tmEndTime.Size = new System.Drawing.Size(76, 21);
            this.tmEndTime.TabIndex = 16;
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(294, 387);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 2;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // tmStartTime
            // 
            this.tmStartTime.EditValue = new System.DateTime(2008, 12, 5, 0, 0, 0, 0);
            this.tmStartTime.Location = new System.Drawing.Point(99, 11);
            this.tmStartTime.Name = "tmStartTime";
            this.tmStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.tmStartTime.Properties.Mask.EditMask = "HH:mm";
            this.tmStartTime.Size = new System.Drawing.Size(76, 21);
            this.tmStartTime.TabIndex = 15;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.tmEndTime);
            this.panelControl4.Controls.Add(this.tmStartTime);
            this.panelControl4.Controls.Add(this.labelControl3);
            this.panelControl4.Controls.Add(this.labelControl1);
            this.panelControl4.Controls.Add(this.labelControl2);
            this.panelControl4.Controls.Add(this.labelControl14);
            this.panelControl4.Location = new System.Drawing.Point(10, 341);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(440, 40);
            this.panelControl4.TabIndex = 0;
            this.panelControl4.Text = "panelControl4";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(400, 14);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "分钟";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(181, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "分钟";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(233, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(76, 14);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "允许截止时间:";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(375, 387);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdFuseTimesectionResult);
            this.panelControl1.Location = new System.Drawing.Point(12, 57);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(437, 278);
            this.panelControl1.TabIndex = 50;
            this.panelControl1.Text = "panelControl1";
            // 
            // gdFuseTimesectionResult
            // 
            this.gdFuseTimesectionResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdFuseTimesectionResult.EmbeddedNavigator.Name = "";
            this.gdFuseTimesectionResult.Location = new System.Drawing.Point(2, 2);
            this.gdFuseTimesectionResult.MainView = this.gdFuseTimesectionSelect;
            this.gdFuseTimesectionResult.Name = "gdFuseTimesectionResult";
            this.gdFuseTimesectionResult.Size = new System.Drawing.Size(433, 274);
            this.gdFuseTimesectionResult.TabIndex = 8;
            this.gdFuseTimesectionResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdFuseTimesectionSelect});
            this.gdFuseTimesectionResult.DoubleClick += new System.EventHandler(this.gdFuseTimesectionResult_DoubleClick);
            // 
            // gdFuseTimesectionSelect
            // 
            this.gdFuseTimesectionSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcCommodityCode,
            this.gdcStartTime,
            this.gdcEndTime});
            this.gdFuseTimesectionSelect.GridControl = this.gdFuseTimesectionResult;
            this.gdFuseTimesectionSelect.Name = "gdFuseTimesectionSelect";
            this.gdFuseTimesectionSelect.OptionsBehavior.Editable = false;
            this.gdFuseTimesectionSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcCommodityCode
            // 
            this.gdcCommodityCode.Caption = "代码";
            this.gdcCommodityCode.FieldName = "CommodityCode";
            this.gdcCommodityCode.Name = "gdcCommodityCode";
            this.gdcCommodityCode.Visible = true;
            this.gdcCommodityCode.VisibleIndex = 0;
            this.gdcCommodityCode.Width = 127;
            // 
            // gdcStartTime
            // 
            this.gdcStartTime.Caption = "允许起始时间";
            this.gdcStartTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcStartTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcStartTime.FieldName = "StartTime";
            this.gdcStartTime.Name = "gdcStartTime";
            this.gdcStartTime.Visible = true;
            this.gdcStartTime.VisibleIndex = 1;
            this.gdcStartTime.Width = 169;
            // 
            // gdcEndTime
            // 
            this.gdcEndTime.Caption = "允许截止时间";
            this.gdcEndTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcEndTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcEndTime.FieldName = "EndTime";
            this.gdcEndTime.Name = "gdcEndTime";
            this.gdcEndTime.Visible = true;
            this.gdcEndTime.VisibleIndex = 2;
            this.gdcEndTime.Width = 185;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(213, 387);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // FuseTimesectionManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 422);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FuseTimesectionManageUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "熔断时间段管理";
            this.Load += new System.EventHandler(this.FuseTimesectionManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdFuseTimesectionResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdFuseTimesectionSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TimeEdit tmEndTime;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.TimeEdit tmStartTime;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gdFuseTimesectionResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdFuseTimesectionSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCommodityCode;
        private DevExpress.XtraGrid.Columns.GridColumn gdcStartTime;
        private DevExpress.XtraGrid.Columns.GridColumn gdcEndTime;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.LabelControl labCommodityCode;
    }
}