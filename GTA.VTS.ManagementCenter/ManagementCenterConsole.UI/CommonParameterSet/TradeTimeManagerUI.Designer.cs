using ManagementCenterConsole.UI.CommonControl;

namespace ManagementCenterConsole.UI.CommonParameterSet
{
    partial class TradeTimeManagerUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradeTimeManagerUI));
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancelTradeTime = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpdateTradeTime = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.tmTradeEndTime = new DevExpress.XtraEditors.TimeEdit();
            this.tmTradeStartTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btnDeleteTradeTime = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddTradeTime = new DevExpress.XtraEditors.SimpleButton();
            this.gdTradeTimeResult = new DevExpress.XtraGrid.GridControl();
            this.gdvTradeTimeSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBourseTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTradeStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTradeEndTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBourseTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.tmCounFromSubmitEndTime = new DevExpress.XtraEditors.TimeEdit();
            this.tmCounFromSubmitStartTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.txtBourseTypeName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddNotTradeDate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmTradeEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmTradeStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdTradeTimeResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvTradeTimeSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmCounFromSubmitEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmCounFromSubmitStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBourseTypeName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btnCancelTradeTime);
            this.panelControl3.Controls.Add(this.btnUpdateTradeTime);
            this.panelControl3.Controls.Add(this.panelControl5);
            this.panelControl3.Controls.Add(this.btnDeleteTradeTime);
            this.panelControl3.Controls.Add(this.btnAddTradeTime);
            this.panelControl3.Controls.Add(this.gdTradeTimeResult);
            this.panelControl3.Location = new System.Drawing.Point(12, 126);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(470, 246);
            this.panelControl3.TabIndex = 1;
            this.panelControl3.Text = "panelControl3";
            // 
            // btnCancelTradeTime
            // 
            this.btnCancelTradeTime.Location = new System.Drawing.Point(406, 210);
            this.btnCancelTradeTime.Name = "btnCancelTradeTime";
            this.btnCancelTradeTime.Size = new System.Drawing.Size(56, 23);
            this.btnCancelTradeTime.TabIndex = 4;
            this.btnCancelTradeTime.Text = "取消";
            this.btnCancelTradeTime.Click += new System.EventHandler(this.btnCancelTradeTime_Click);
            // 
            // btnUpdateTradeTime
            // 
            this.btnUpdateTradeTime.Location = new System.Drawing.Point(282, 210);
            this.btnUpdateTradeTime.Name = "btnUpdateTradeTime";
            this.btnUpdateTradeTime.Size = new System.Drawing.Size(56, 23);
            this.btnUpdateTradeTime.TabIndex = 2;
            this.btnUpdateTradeTime.Text = "修改";
            this.btnUpdateTradeTime.Click += new System.EventHandler(this.btnUpdateTradeTime_Click);
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.tmTradeEndTime);
            this.panelControl5.Controls.Add(this.tmTradeStartTime);
            this.panelControl5.Controls.Add(this.labelControl8);
            this.panelControl5.Controls.Add(this.labelControl9);
            this.panelControl5.Location = new System.Drawing.Point(7, 5);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(458, 43);
            this.panelControl5.TabIndex = 0;
            this.panelControl5.Text = "panelControl5";
            // 
            // tmTradeEndTime
            // 
            this.tmTradeEndTime.EditValue = new System.DateTime(2008, 11, 29, 0, 0, 0, 0);
            this.tmTradeEndTime.Location = new System.Drawing.Point(337, 8);
            this.tmTradeEndTime.Name = "tmTradeEndTime";
            this.tmTradeEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.tmTradeEndTime.Properties.Mask.EditMask = "HH:mm";
            this.tmTradeEndTime.Size = new System.Drawing.Size(100, 21);
            this.tmTradeEndTime.TabIndex = 1;
            // 
            // tmTradeStartTime
            // 
            this.tmTradeStartTime.EditValue = new System.DateTime(2008, 11, 29, 0, 0, 0, 0);
            this.tmTradeStartTime.Location = new System.Drawing.Point(140, 9);
            this.tmTradeStartTime.Name = "tmTradeStartTime";
            this.tmTradeStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.tmTradeStartTime.Properties.Mask.EditMask = "HH:mm";
            this.tmTradeStartTime.Size = new System.Drawing.Size(100, 21);
            this.tmTradeStartTime.TabIndex = 0;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(255, 12);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(76, 14);
            this.labelControl8.TabIndex = 2;
            this.labelControl8.Text = "交易结束时间:";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(8, 12);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(76, 14);
            this.labelControl9.TabIndex = 1;
            this.labelControl9.Text = "交易开始时间:";
            // 
            // btnDeleteTradeTime
            // 
            this.btnDeleteTradeTime.Location = new System.Drawing.Point(344, 210);
            this.btnDeleteTradeTime.Name = "btnDeleteTradeTime";
            this.btnDeleteTradeTime.Size = new System.Drawing.Size(56, 23);
            this.btnDeleteTradeTime.TabIndex = 3;
            this.btnDeleteTradeTime.Text = "删除";
            this.btnDeleteTradeTime.Click += new System.EventHandler(this.btnDeleteTradeTime_Click);
            // 
            // btnAddTradeTime
            // 
            this.btnAddTradeTime.Location = new System.Drawing.Point(217, 210);
            this.btnAddTradeTime.Name = "btnAddTradeTime";
            this.btnAddTradeTime.Size = new System.Drawing.Size(56, 23);
            this.btnAddTradeTime.TabIndex = 1;
            this.btnAddTradeTime.Text = "添加";
            this.btnAddTradeTime.Click += new System.EventHandler(this.btnAddTradeTime_Click);
            // 
            // gdTradeTimeResult
            // 
            this.gdTradeTimeResult.EmbeddedNavigator.Name = "";
            this.gdTradeTimeResult.Location = new System.Drawing.Point(7, 54);
            this.gdTradeTimeResult.MainView = this.gdvTradeTimeSelect;
            this.gdTradeTimeResult.Name = "gdTradeTimeResult";
            this.gdTradeTimeResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBourseTypeID});
            this.gdTradeTimeResult.Size = new System.Drawing.Size(458, 140);
            this.gdTradeTimeResult.TabIndex = 8;
            this.gdTradeTimeResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvTradeTimeSelect});
            this.gdTradeTimeResult.DoubleClick += new System.EventHandler(this.gdTradeTimeResult_DoubleClick);
            // 
            // gdvTradeTimeSelect
            // 
            this.gdvTradeTimeSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBourseTypeName,
            this.gdcTradeStartTime,
            this.gdcTradeEndTime});
            this.gdvTradeTimeSelect.CustomizationFormBounds = new System.Drawing.Rectangle(319, 363, 208, 177);
            this.gdvTradeTimeSelect.GridControl = this.gdTradeTimeResult;
            this.gdvTradeTimeSelect.Name = "gdvTradeTimeSelect";
            this.gdvTradeTimeSelect.OptionsBehavior.Editable = false;
            this.gdvTradeTimeSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcBourseTypeName
            // 
            this.gdcBourseTypeName.Caption = "交易所名称";
            this.gdcBourseTypeName.FieldName = "BOURSETYPENAME";
            this.gdcBourseTypeName.Name = "gdcBourseTypeName";
            this.gdcBourseTypeName.Visible = true;
            this.gdcBourseTypeName.VisibleIndex = 0;
            // 
            // gdcTradeStartTime
            // 
            this.gdcTradeStartTime.Caption = "交易开始时间";
            this.gdcTradeStartTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcTradeStartTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcTradeStartTime.FieldName = "STARTTIME";
            this.gdcTradeStartTime.Name = "gdcTradeStartTime";
            this.gdcTradeStartTime.Visible = true;
            this.gdcTradeStartTime.VisibleIndex = 1;
            // 
            // gdcTradeEndTime
            // 
            this.gdcTradeEndTime.Caption = "交易结束时间";
            this.gdcTradeEndTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcTradeEndTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcTradeEndTime.FieldName = "ENDTIME";
            this.gdcTradeEndTime.Name = "gdcTradeEndTime";
            this.gdcTradeEndTime.Visible = true;
            this.gdcTradeEndTime.VisibleIndex = 2;
            // 
            // ddlBourseTypeID
            // 
            this.ddlBourseTypeID.AutoHeight = false;
            this.ddlBourseTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBourseTypeID.Name = "ddlBourseTypeID";
            this.ddlBourseTypeID.NullText = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(263, 386);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.labelControl14);
            this.panelControl4.Controls.Add(this.tmCounFromSubmitEndTime);
            this.panelControl4.Controls.Add(this.tmCounFromSubmitStartTime);
            this.panelControl4.Controls.Add(this.labelControl7);
            this.panelControl4.Controls.Add(this.labelControl10);
            this.panelControl4.Controls.Add(this.txtBourseTypeName);
            this.panelControl4.Controls.Add(this.labelControl5);
            this.panelControl4.Location = new System.Drawing.Point(12, 12);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(470, 108);
            this.panelControl4.TabIndex = 0;
            this.panelControl4.Text = "panelControl4";
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.Location = new System.Drawing.Point(251, 16);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(7, 14);
            this.labelControl14.TabIndex = 22;
            this.labelControl14.Text = "*";
            // 
            // tmCounFromSubmitEndTime
            // 
            this.tmCounFromSubmitEndTime.EditValue = new System.DateTime(2008, 11, 29, 0, 0, 0, 0);
            this.tmCounFromSubmitEndTime.Location = new System.Drawing.Point(145, 76);
            this.tmCounFromSubmitEndTime.Name = "tmCounFromSubmitEndTime";
            this.tmCounFromSubmitEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.tmCounFromSubmitEndTime.Properties.Mask.EditMask = "HH:mm";
            this.tmCounFromSubmitEndTime.Size = new System.Drawing.Size(100, 21);
            this.tmCounFromSubmitEndTime.TabIndex = 2;
            // 
            // tmCounFromSubmitStartTime
            // 
            this.tmCounFromSubmitStartTime.EditValue = new System.DateTime(2008, 11, 29, 0, 0, 0, 0);
            this.tmCounFromSubmitStartTime.Location = new System.Drawing.Point(145, 42);
            this.tmCounFromSubmitStartTime.Name = "tmCounFromSubmitStartTime";
            this.tmCounFromSubmitStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.tmCounFromSubmitStartTime.Properties.Mask.EditMask = "HH:mm";
            this.tmCounFromSubmitStartTime.Size = new System.Drawing.Size(100, 21);
            this.tmCounFromSubmitStartTime.TabIndex = 1;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(15, 79);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(124, 14);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "柜台结束接收委托时间:";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(15, 45);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(124, 14);
            this.labelControl10.TabIndex = 18;
            this.labelControl10.Text = "柜台开始接收委托时间:";
            // 
            // txtBourseTypeName
            // 
            this.txtBourseTypeName.Location = new System.Drawing.Point(145, 9);
            this.txtBourseTypeName.Name = "txtBourseTypeName";
            this.txtBourseTypeName.Size = new System.Drawing.Size(100, 21);
            this.txtBourseTypeName.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(15, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 14);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "交易所名称:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(151, 386);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(106, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "修改交易所类型";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnAddNotTradeDate
            // 
            this.btnAddNotTradeDate.Location = new System.Drawing.Point(376, 386);
            this.btnAddNotTradeDate.Name = "btnAddNotTradeDate";
            this.btnAddNotTradeDate.Size = new System.Drawing.Size(106, 23);
            this.btnAddNotTradeDate.TabIndex = 7;
            this.btnAddNotTradeDate.Text = "设置非交易日期";
            this.btnAddNotTradeDate.Click += new System.EventHandler(this.btnAddNotTradeDate_Click);
            // 
            // TradeTimeManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(495, 421);
            this.Controls.Add(this.btnAddNotTradeDate);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panelControl3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TradeTimeManagerUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "交易所类型和交易时间管理";
            this.Load += new System.EventHandler(this.TradeTimeManagerUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            this.panelControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmTradeEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmTradeStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdTradeTimeResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvTradeTimeSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmCounFromSubmitEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmCounFromSubmitStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBourseTypeName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gdTradeTimeResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvTradeTimeSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBourseTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTradeStartTime;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTradeEndTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBourseTypeID;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnDeleteTradeTime;
        private DevExpress.XtraEditors.SimpleButton btnAddTradeTime;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.TextEdit txtBourseTypeName;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.TimeEdit tmTradeEndTime;
        private DevExpress.XtraEditors.TimeEdit tmTradeStartTime;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnAddNotTradeDate;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TimeEdit tmCounFromSubmitEndTime;
        private DevExpress.XtraEditors.TimeEdit tmCounFromSubmitStartTime;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton btnUpdateTradeTime;
        private DevExpress.XtraEditors.SimpleButton btnCancelTradeTime;
        private DevExpress.XtraEditors.LabelControl labelControl14;
    }
}