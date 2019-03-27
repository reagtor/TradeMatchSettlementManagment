namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    partial class CommodityFuseManageUI
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
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gdCommodityFuseResult = new DevExpress.XtraGrid.GridControl();
            this.gdCommodityFuseSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcCommodityCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTriggeringScale = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcFuseTimeOfDay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTriggeringDuration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcFuseDurationLimit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtCommodityCode = new DevExpress.XtraEditors.TextEdit();
            this.UCPageNavig = new ManagementCenterConsole.UI.CommonControl.UCPageNavigation();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddFuseTimesection = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtFuseDurationLimit = new DevExpress.XtraEditors.TextEdit();
            this.txtTriggeringDuration = new DevExpress.XtraEditors.TextEdit();
            this.cmbCommodityCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtFuseTimeOfDay = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txTriggeringScale = new DevExpress.XtraEditors.TextEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdCommodityFuseResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdCommodityFuseSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommodityCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFuseDurationLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTriggeringDuration.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommodityCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFuseTimeOfDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txTriggeringScale.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(13, 14);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(28, 14);
            this.labelControl9.TabIndex = 16;
            this.labelControl9.Text = "代码:";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdCommodityFuseResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 39);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(792, 361);
            this.panelControl1.TabIndex = 44;
            this.panelControl1.Text = "panelControl1";
            // 
            // gdCommodityFuseResult
            // 
            this.gdCommodityFuseResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdCommodityFuseResult.EmbeddedNavigator.Name = "";
            this.gdCommodityFuseResult.Location = new System.Drawing.Point(2, 2);
            this.gdCommodityFuseResult.MainView = this.gdCommodityFuseSelect;
            this.gdCommodityFuseResult.Name = "gdCommodityFuseResult";
            this.gdCommodityFuseResult.Size = new System.Drawing.Size(788, 357);
            this.gdCommodityFuseResult.TabIndex = 0;
            this.gdCommodityFuseResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdCommodityFuseSelect});
            this.gdCommodityFuseResult.DoubleClick += new System.EventHandler(this.gdCommodityFuseResult_DoubleClick);
            this.gdCommodityFuseResult.Click += new System.EventHandler(this.gdCommodityFuseResult_Click);
            // 
            // gdCommodityFuseSelect
            // 
            this.gdCommodityFuseSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcCommodityCode,
            this.gdcTriggeringScale,
            this.gdcFuseTimeOfDay,
            this.gdcTriggeringDuration,
            this.gdcFuseDurationLimit});
            this.gdCommodityFuseSelect.GridControl = this.gdCommodityFuseResult;
            this.gdCommodityFuseSelect.Name = "gdCommodityFuseSelect";
            this.gdCommodityFuseSelect.OptionsBehavior.Editable = false;
            this.gdCommodityFuseSelect.OptionsView.ShowGroupPanel = false;
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
            // gdcTriggeringScale
            // 
            this.gdcTriggeringScale.Caption = "触发比例(%)";
            this.gdcTriggeringScale.FieldName = "TriggeringScale";
            this.gdcTriggeringScale.Name = "gdcTriggeringScale";
            this.gdcTriggeringScale.Visible = true;
            this.gdcTriggeringScale.VisibleIndex = 1;
            this.gdcTriggeringScale.Width = 169;
            // 
            // gdcFuseTimeOfDay
            // 
            this.gdcFuseTimeOfDay.Caption = "熔断次数(次/天)";
            this.gdcFuseTimeOfDay.FieldName = "FuseTimeOfDay";
            this.gdcFuseTimeOfDay.Name = "gdcFuseTimeOfDay";
            this.gdcFuseTimeOfDay.Visible = true;
            this.gdcFuseTimeOfDay.VisibleIndex = 2;
            this.gdcFuseTimeOfDay.Width = 123;
            // 
            // gdcTriggeringDuration
            // 
            this.gdcTriggeringDuration.Caption = "触发持续时间限制(分钟)";
            this.gdcTriggeringDuration.FieldName = "TriggeringDuration";
            this.gdcTriggeringDuration.Name = "gdcTriggeringDuration";
            this.gdcTriggeringDuration.Visible = true;
            this.gdcTriggeringDuration.VisibleIndex = 3;
            this.gdcTriggeringDuration.Width = 151;
            // 
            // gdcFuseDurationLimit
            // 
            this.gdcFuseDurationLimit.Caption = "熔断持续时间限制(分钟)";
            this.gdcFuseDurationLimit.FieldName = "FuseDurationLimit";
            this.gdcFuseDurationLimit.Name = "gdcFuseDurationLimit";
            this.gdcFuseDurationLimit.Visible = true;
            this.gdcFuseDurationLimit.VisibleIndex = 4;
            this.gdcFuseDurationLimit.Width = 173;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(153, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.txtCommodityCode);
            this.panelControl3.Controls.Add(this.labelControl9);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.UCPageNavig);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(792, 39);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
            // 
            // txtCommodityCode
            // 
            this.txtCommodityCode.Location = new System.Drawing.Point(47, 11);
            this.txtCommodityCode.Name = "txtCommodityCode";
            this.txtCommodityCode.Size = new System.Drawing.Size(100, 21);
            this.txtCommodityCode.TabIndex = 0;
            // 
            // UCPageNavig
            // 
            this.UCPageNavig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UCPageNavig.CurrentPage = 0;
            this.UCPageNavig.Location = new System.Drawing.Point(461, 5);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 2;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.panel1);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(0, 400);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(792, 166);
            this.panelControl4.TabIndex = 1;
            this.panelControl4.Text = "panelControl4";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.labelControl10);
            this.panel1.Controls.Add(this.labelControl8);
            this.panel1.Controls.Add(this.labelControl7);
            this.panel1.Controls.Add(this.labelControl6);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnAddFuseTimesection);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.labelControl5);
            this.panel1.Controls.Add(this.txtFuseDurationLimit);
            this.panel1.Controls.Add(this.txtTriggeringDuration);
            this.panel1.Controls.Add(this.cmbCommodityCode);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.txtFuseTimeOfDay);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.txTriggeringScale);
            this.panel1.Controls.Add(this.labelControl12);
            this.panel1.Controls.Add(this.labelControl14);
            this.panel1.Controls.Add(this.labelControl15);
            this.panel1.Location = new System.Drawing.Point(5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 157);
            this.panel1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(582, 127);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(66, 23);
            this.btnOK.TabIndex = 61;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl10.Appearance.Options.UseForeColor = true;
            this.labelControl10.Location = new System.Drawing.Point(327, 80);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(7, 14);
            this.labelControl10.TabIndex = 60;
            this.labelControl10.Text = "*";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Location = new System.Drawing.Point(346, 48);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(7, 14);
            this.labelControl8.TabIndex = 59;
            this.labelControl8.Text = "*";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl7.Appearance.Options.UseForeColor = true;
            this.labelControl7.Location = new System.Drawing.Point(604, 48);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(7, 14);
            this.labelControl7.TabIndex = 58;
            this.labelControl7.Text = "*";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl6.Appearance.Options.UseForeColor = true;
            this.labelControl6.Location = new System.Drawing.Point(604, 13);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(7, 14);
            this.labelControl6.TabIndex = 57;
            this.labelControl6.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(582, 98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(66, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddFuseTimesection
            // 
            this.btnAddFuseTimesection.Location = new System.Drawing.Point(667, 127);
            this.btnAddFuseTimesection.Name = "btnAddFuseTimesection";
            this.btnAddFuseTimesection.Size = new System.Drawing.Size(108, 23);
            this.btnAddFuseTimesection.TabIndex = 9;
            this.btnAddFuseTimesection.Text = "设置熔断时间段";
            this.btnAddFuseTimesection.Click += new System.EventHandler(this.btnAddFuseTimesection_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(507, 127);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(66, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(434, 127);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(66, 23);
            this.btnModify.TabIndex = 6;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(362, 127);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(66, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(328, 47);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(12, 14);
            this.labelControl5.TabIndex = 34;
            this.labelControl5.Text = "%";
            // 
            // txtFuseDurationLimit
            // 
            this.txtFuseDurationLimit.Location = new System.Drawing.Point(468, 41);
            this.txtFuseDurationLimit.Name = "txtFuseDurationLimit";
            this.txtFuseDurationLimit.Size = new System.Drawing.Size(100, 21);
            this.txtFuseDurationLimit.TabIndex = 4;
            // 
            // txtTriggeringDuration
            // 
            this.txtTriggeringDuration.Location = new System.Drawing.Point(468, 6);
            this.txtTriggeringDuration.Name = "txtTriggeringDuration";
            this.txtTriggeringDuration.Size = new System.Drawing.Size(100, 21);
            this.txtTriggeringDuration.TabIndex = 3;
            // 
            // cmbCommodityCode
            // 
            this.cmbCommodityCode.Location = new System.Drawing.Point(221, 7);
            this.cmbCommodityCode.Name = "cmbCommodityCode";
            this.cmbCommodityCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCommodityCode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbCommodityCode.Size = new System.Drawing.Size(100, 21);
            this.cmbCommodityCode.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(163, 10);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(28, 14);
            this.labelControl4.TabIndex = 30;
            this.labelControl4.Text = "代码:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(574, 44);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 29;
            this.labelControl3.Text = "分钟";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(574, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 28;
            this.labelControl1.Text = "分钟";
            // 
            // txtFuseTimeOfDay
            // 
            this.txtFuseTimeOfDay.Location = new System.Drawing.Point(221, 73);
            this.txtFuseTimeOfDay.Name = "txtFuseTimeOfDay";
            this.txtFuseTimeOfDay.Size = new System.Drawing.Size(100, 21);
            this.txtFuseTimeOfDay.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(362, 44);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(100, 14);
            this.labelControl2.TabIndex = 26;
            this.labelControl2.Text = "熔断持续时间限制:";
            // 
            // txTriggeringScale
            // 
            this.txTriggeringScale.Location = new System.Drawing.Point(221, 41);
            this.txTriggeringScale.Name = "txTriggeringScale";
            this.txTriggeringScale.Size = new System.Drawing.Size(100, 21);
            this.txTriggeringScale.TabIndex = 1;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(124, 76);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(91, 14);
            this.labelControl12.TabIndex = 24;
            this.labelControl12.Text = "熔断次数(次/天):";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(362, 9);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(100, 14);
            this.labelControl14.TabIndex = 23;
            this.labelControl14.Text = "触发持续时间限制:";
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(163, 44);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(52, 14);
            this.labelControl15.TabIndex = 22;
            this.labelControl15.Text = "触发比例:";
            // 
            // CommodityFuseManageUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommodityFuseManageUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "熔断管理";
            this.Load += new System.EventHandler(this.CommodityFuseManageUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdCommodityFuseResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdCommodityFuseSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommodityCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFuseDurationLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTriggeringDuration.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommodityCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFuseTimeOfDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txTriggeringScale.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gdCommodityFuseResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdCommodityFuseSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCommodityCode;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTriggeringScale;
        private DevExpress.XtraGrid.Columns.GridColumn gdcFuseTimeOfDay;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTriggeringDuration;
        private DevExpress.XtraGrid.Columns.GridColumn gdcFuseDurationLimit;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        private DevExpress.XtraEditors.TextEdit txtCommodityCode;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnAddFuseTimesection;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtFuseDurationLimit;
        private DevExpress.XtraEditors.TextEdit txtTriggeringDuration;
        private DevExpress.XtraEditors.ComboBoxEdit cmbCommodityCode;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtFuseTimeOfDay;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txTriggeringScale;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton btnOK;
    }
}