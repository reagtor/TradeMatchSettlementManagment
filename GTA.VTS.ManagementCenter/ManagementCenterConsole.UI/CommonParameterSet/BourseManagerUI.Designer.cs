using ManagementCenterConsole.UI.CommonControl;

namespace ManagementCenterConsole.UI.CommonParameterSet
{
    partial class BourseManagerUI
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtCondition = new DevExpress.XtraEditors.TextEdit();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.gdBourseTypeResult = new DevExpress.XtraGrid.GridControl();
            this.gdvBourseTypeSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdcBourseTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcReceivingConsignStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcReceivingConsignEndTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcCounterFromSubmitStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcCounterFromSubmitEndTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gdTradeTimeResult = new DevExpress.XtraGrid.GridControl();
            this.gdvTradeTimeSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ddlBourseTypeID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gdcTradeStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdcTradeEndTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtCondition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdBourseTypeResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvBourseTypeSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdTradeTimeResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvTradeTimeSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).BeginInit();
            this.SuspendLayout();
            // 
            // UCPageNavig
            // 
            this.UCPageNavig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UCPageNavig.CurrentPage = 0;
            this.UCPageNavig.Location = new System.Drawing.Point(468, 8);
            this.UCPageNavig.Name = "UCPageNavig";
            this.UCPageNavig.PageCount = 0;
            this.UCPageNavig.Size = new System.Drawing.Size(319, 28);
            this.UCPageNavig.TabIndex = 5;
            this.UCPageNavig.PageIndexChanged += new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(this.UCPageNavig_PageIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "关键字:";
            // 
            // txtCondition
            // 
            this.txtCondition.Location = new System.Drawing.Point(51, 11);
            this.txtCondition.Name = "txtCondition";
            this.txtCondition.Size = new System.Drawing.Size(127, 21);
            this.txtCondition.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(184, 11);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(62, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(332, 11);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(62, 23);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(264, 11);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gdBourseTypeResult
            // 
            this.gdBourseTypeResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdBourseTypeResult.EmbeddedNavigator.Name = "";
            this.gdBourseTypeResult.Location = new System.Drawing.Point(2, 2);
            this.gdBourseTypeResult.MainView = this.gdvBourseTypeSelect;
            this.gdBourseTypeResult.Name = "gdBourseTypeResult";
            this.gdBourseTypeResult.Size = new System.Drawing.Size(788, 357);
            this.gdBourseTypeResult.TabIndex = 0;
            this.gdBourseTypeResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvBourseTypeSelect});
            this.gdBourseTypeResult.DoubleClick += new System.EventHandler(this.gdBourseTypeResult_DoubleClick);
            this.gdBourseTypeResult.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gdBourseTypeResult_MouseMove);
            this.gdBourseTypeResult.Click += new System.EventHandler(this.gdBourseTypeResult_Click);
            // 
            // gdvBourseTypeSelect
            // 
            this.gdvBourseTypeSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdcBourseTypeName,
            this.gdcReceivingConsignStartTime,
            this.gdcReceivingConsignEndTime,
            this.gdcCounterFromSubmitStartTime,
            this.gdcCounterFromSubmitEndTime});
            this.gdvBourseTypeSelect.GridControl = this.gdBourseTypeResult;
            this.gdvBourseTypeSelect.Name = "gdvBourseTypeSelect";
            this.gdvBourseTypeSelect.OptionsBehavior.Editable = false;
            this.gdvBourseTypeSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gdcBourseTypeName
            // 
            this.gdcBourseTypeName.Caption = "交易所名称";
            this.gdcBourseTypeName.FieldName = "BourseTypeName";
            this.gdcBourseTypeName.Name = "gdcBourseTypeName";
            this.gdcBourseTypeName.Visible = true;
            this.gdcBourseTypeName.VisibleIndex = 0;
            this.gdcBourseTypeName.Width = 111;
            // 
            // gdcReceivingConsignStartTime
            // 
            this.gdcReceivingConsignStartTime.Caption = "撮合中心开始接收申报时间";
            this.gdcReceivingConsignStartTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcReceivingConsignStartTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcReceivingConsignStartTime.FieldName = "ReceivingConsignStartTime";
            this.gdcReceivingConsignStartTime.Name = "gdcReceivingConsignStartTime";
            this.gdcReceivingConsignStartTime.Width = 163;
            // 
            // gdcReceivingConsignEndTime
            // 
            this.gdcReceivingConsignEndTime.Caption = "撮合中心结束接收申报时间";
            this.gdcReceivingConsignEndTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcReceivingConsignEndTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcReceivingConsignEndTime.FieldName = "ReceivingConsignEndTime";
            this.gdcReceivingConsignEndTime.Name = "gdcReceivingConsignEndTime";
            this.gdcReceivingConsignEndTime.Width = 163;
            // 
            // gdcCounterFromSubmitStartTime
            // 
            this.gdcCounterFromSubmitStartTime.Caption = "柜台开始接收委托时间";
            this.gdcCounterFromSubmitStartTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcCounterFromSubmitStartTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcCounterFromSubmitStartTime.FieldName = "CounterFromSubmitStartTime";
            this.gdcCounterFromSubmitStartTime.Name = "gdcCounterFromSubmitStartTime";
            this.gdcCounterFromSubmitStartTime.Visible = true;
            this.gdcCounterFromSubmitStartTime.VisibleIndex = 1;
            this.gdcCounterFromSubmitStartTime.Width = 163;
            // 
            // gdcCounterFromSubmitEndTime
            // 
            this.gdcCounterFromSubmitEndTime.Caption = "柜台结束接收委托时间";
            this.gdcCounterFromSubmitEndTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcCounterFromSubmitEndTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcCounterFromSubmitEndTime.FieldName = "CounterFromSubmitEndTime";
            this.gdcCounterFromSubmitEndTime.Name = "gdcCounterFromSubmitEndTime";
            this.gdcCounterFromSubmitEndTime.Visible = true;
            this.gdcCounterFromSubmitEndTime.VisibleIndex = 2;
            this.gdcCounterFromSubmitEndTime.Width = 167;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gdBourseTypeResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 45);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(792, 361);
            this.panelControl1.TabIndex = 14;
            this.panelControl1.Text = "panelControl1";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btnDelete);
            this.panelControl3.Controls.Add(this.btnModify);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.btnAdd);
            this.panelControl3.Controls.Add(this.txtCondition);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.UCPageNavig);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(792, 45);
            this.panelControl3.TabIndex = 0;
            this.panelControl3.Text = "panelControl3";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(400, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(62, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gdTradeTimeResult);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 412);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(792, 154);
            this.panelControl2.TabIndex = 16;
            this.panelControl2.Text = "panelControl2";
            // 
            // gdTradeTimeResult
            // 
            this.gdTradeTimeResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdTradeTimeResult.EmbeddedNavigator.Name = "";
            this.gdTradeTimeResult.Location = new System.Drawing.Point(2, 2);
            this.gdTradeTimeResult.MainView = this.gdvTradeTimeSelect;
            this.gdTradeTimeResult.Name = "gdTradeTimeResult";
            this.gdTradeTimeResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ddlBourseTypeID});
            this.gdTradeTimeResult.Size = new System.Drawing.Size(788, 150);
            this.gdTradeTimeResult.TabIndex = 9;
            this.gdTradeTimeResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvTradeTimeSelect});
            // 
            // gdvTradeTimeSelect
            // 
            this.gdvTradeTimeSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gdcTradeStartTime,
            this.gdcTradeEndTime});
            this.gdvTradeTimeSelect.GridControl = this.gdTradeTimeResult;
            this.gdvTradeTimeSelect.Name = "gdvTradeTimeSelect";
            this.gdvTradeTimeSelect.OptionsBehavior.Editable = false;
            this.gdvTradeTimeSelect.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "交易所名称";
            this.gridColumn1.ColumnEdit = this.ddlBourseTypeID;
            this.gridColumn1.FieldName = "BourseTypeID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // ddlBourseTypeID
            // 
            this.ddlBourseTypeID.AutoHeight = false;
            this.ddlBourseTypeID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlBourseTypeID.Name = "ddlBourseTypeID";
            this.ddlBourseTypeID.NullText = "";
            // 
            // gdcTradeStartTime
            // 
            this.gdcTradeStartTime.Caption = "交易开始时间";
            this.gdcTradeStartTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcTradeStartTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcTradeStartTime.FieldName = "StartTime";
            this.gdcTradeStartTime.Name = "gdcTradeStartTime";
            this.gdcTradeStartTime.Visible = true;
            this.gdcTradeStartTime.VisibleIndex = 1;
            // 
            // gdcTradeEndTime
            // 
            this.gdcTradeEndTime.Caption = "交易结束时间";
            this.gdcTradeEndTime.DisplayFormat.FormatString = "HH:mm";
            this.gdcTradeEndTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gdcTradeEndTime.FieldName = "EndTime";
            this.gdcTradeEndTime.Name = "gdcTradeEndTime";
            this.gdcTradeEndTime.Visible = true;
            this.gdcTradeEndTime.VisibleIndex = 2;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterControl1.Location = new System.Drawing.Point(0, 406);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(792, 6);
            this.splitterControl1.TabIndex = 17;
            this.splitterControl1.TabStop = false;
            // 
            // BourseManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl3);
            this.Name = "BourseManagerUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "交易所类型管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.BourseManagerUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtCondition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdBourseTypeResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvBourseTypeSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdTradeTimeResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvTradeTimeSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBourseTypeID)).EndInit();
            this.ResumeLayout(false);

        }

        private ManagementCenterConsole.UI.CommonControl.UCPageNavigation UCPageNavig;
        #endregion
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtCondition;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraGrid.GridControl gdBourseTypeResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvBourseTypeSelect;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.Columns.GridColumn gdcBourseTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn gdcReceivingConsignStartTime;
        private DevExpress.XtraGrid.Columns.GridColumn gdcReceivingConsignEndTime;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCounterFromSubmitStartTime;
        private DevExpress.XtraGrid.Columns.GridColumn gdcCounterFromSubmitEndTime;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gdTradeTimeResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvTradeTimeSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ddlBourseTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTradeStartTime;
        private DevExpress.XtraGrid.Columns.GridColumn gdcTradeEndTime;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.SimpleButton btnDelete;

    }
}