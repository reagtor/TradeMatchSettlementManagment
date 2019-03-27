namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    partial class AddLastTradingDayUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddLastTradingDayUI));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbLastTradingDayType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbSequence = new DevExpress.XtraEditors.ComboBoxEdit();
            this.speWhatDay = new DevExpress.XtraEditors.SpinEdit();
            this.speWhatWeek = new DevExpress.XtraEditors.SpinEdit();
            this.cmbWeek = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLastTradingDayType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSequence.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speWhatDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speWhatWeek.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWeek.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cmbLastTradingDayType);
            this.panelControl1.Controls.Add(this.cmbSequence);
            this.panelControl1.Controls.Add(this.speWhatDay);
            this.panelControl1.Controls.Add(this.speWhatWeek);
            this.panelControl1.Controls.Add(this.cmbWeek);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Location = new System.Drawing.Point(12, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(268, 205);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Text = "panelControl1";
            // 
            // cmbLastTradingDayType
            // 
            this.cmbLastTradingDayType.Location = new System.Drawing.Point(79, 12);
            this.cmbLastTradingDayType.Name = "cmbLastTradingDayType";
            this.cmbLastTradingDayType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLastTradingDayType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbLastTradingDayType.Size = new System.Drawing.Size(143, 21);
            this.cmbLastTradingDayType.TabIndex = 0;
            this.cmbLastTradingDayType.SelectedIndexChanged += new System.EventHandler(this.cmbLastTradingDayType_SelectedIndexChanged);
            // 
            // cmbSequence
            // 
            this.cmbSequence.Location = new System.Drawing.Point(79, 161);
            this.cmbSequence.Name = "cmbSequence";
            this.cmbSequence.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSequence.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbSequence.Size = new System.Drawing.Size(143, 21);
            this.cmbSequence.TabIndex = 4;
            // 
            // speWhatDay
            // 
            this.speWhatDay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.speWhatDay.Location = new System.Drawing.Point(79, 49);
            this.speWhatDay.Name = "speWhatDay";
            this.speWhatDay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speWhatDay.Properties.MaxValue = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.speWhatDay.Size = new System.Drawing.Size(143, 21);
            this.speWhatDay.TabIndex = 1;
            // 
            // speWhatWeek
            // 
            this.speWhatWeek.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.speWhatWeek.Location = new System.Drawing.Point(79, 86);
            this.speWhatWeek.Name = "speWhatWeek";
            this.speWhatWeek.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speWhatWeek.Properties.MaxValue = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.speWhatWeek.Size = new System.Drawing.Size(143, 21);
            this.speWhatWeek.TabIndex = 2;
            // 
            // cmbWeek
            // 
            this.cmbWeek.Location = new System.Drawing.Point(79, 123);
            this.cmbWeek.Name = "cmbWeek";
            this.cmbWeek.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbWeek.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbWeek.Size = new System.Drawing.Size(143, 21);
            this.cmbWeek.TabIndex = 3;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(13, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 28);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "最后交易日\r\n类型:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(13, 164);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(64, 14);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "顺数或倒数:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(13, 126);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "星期几:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(13, 89);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(40, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "第几周:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 52);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "第几日:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(124, 223);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(205, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddLastTradingDayUI
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 258);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddLastTradingDayUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "最后交易日";
            this.Load += new System.EventHandler(this.AddLastTradingDayUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLastTradingDayType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSequence.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speWhatDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speWhatWeek.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWeek.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLastTradingDayType;
        private DevExpress.XtraEditors.ComboBoxEdit cmbSequence;
        private DevExpress.XtraEditors.SpinEdit speWhatDay;
        private DevExpress.XtraEditors.SpinEdit speWhatWeek;
        private DevExpress.XtraEditors.ComboBoxEdit cmbWeek;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}