namespace ManagementCenterConsole.UI.ManagerManage
{
    partial class RightEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RightEdit));
            this.listBoxAllRight = new DevExpress.XtraEditors.ListBoxControl();
            this.listBoxHasRight = new DevExpress.XtraEditors.ListBoxControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_GroupName = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_delAll = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Del = new DevExpress.XtraEditors.SimpleButton();
            this.btn_AddAll = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxAllRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxHasRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_GroupName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxAllRight
            // 
            this.listBoxAllRight.Location = new System.Drawing.Point(5, 25);
            this.listBoxAllRight.Name = "listBoxAllRight";
            this.listBoxAllRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAllRight.Size = new System.Drawing.Size(223, 383);
            this.listBoxAllRight.TabIndex = 0;
            // 
            // listBoxHasRight
            // 
            this.listBoxHasRight.Location = new System.Drawing.Point(295, 25);
            this.listBoxHasRight.Name = "listBoxHasRight";
            this.listBoxHasRight.Size = new System.Drawing.Size(218, 383);
            this.listBoxHasRight.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(164, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "权限组名称:";
            // 
            // txt_GroupName
            // 
            this.txt_GroupName.Location = new System.Drawing.Point(234, 7);
            this.txt_GroupName.Name = "txt_GroupName";
            this.txt_GroupName.Size = new System.Drawing.Size(127, 21);
            this.txt_GroupName.TabIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_delAll);
            this.panelControl1.Controls.Add(this.btn_Del);
            this.panelControl1.Controls.Add(this.btn_AddAll);
            this.panelControl1.Controls.Add(this.btn_Add);
            this.panelControl1.Location = new System.Drawing.Point(232, 25);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(58, 383);
            this.panelControl1.TabIndex = 4;
            this.panelControl1.Text = "panelControl1";
            // 
            // btn_delAll
            // 
            this.btn_delAll.Location = new System.Drawing.Point(5, 246);
            this.btn_delAll.Name = "btn_delAll";
            this.btn_delAll.Size = new System.Drawing.Size(43, 23);
            this.btn_delAll.TabIndex = 3;
            this.btn_delAll.Text = "<<";
            this.btn_delAll.Click += new System.EventHandler(this.btn_delAll_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Location = new System.Drawing.Point(5, 201);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(43, 23);
            this.btn_Del.TabIndex = 2;
            this.btn_Del.Text = "<";
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // btn_AddAll
            // 
            this.btn_AddAll.Location = new System.Drawing.Point(5, 152);
            this.btn_AddAll.Name = "btn_AddAll";
            this.btn_AddAll.Size = new System.Drawing.Size(43, 23);
            this.btn_AddAll.TabIndex = 1;
            this.btn_AddAll.Text = ">>";
            this.btn_AddAll.Click += new System.EventHandler(this.btn_AddAll_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(5, 102);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(43, 23);
            this.btn_Add.TabIndex = 0;
            this.btn_Add.Text = ">";
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(313, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "拥有权限";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(19, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "权限列表";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.listBoxAllRight);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.listBoxHasRight);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.panelControl1);
            this.panelControl2.Location = new System.Drawing.Point(12, 54);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(528, 413);
            this.panelControl2.TabIndex = 7;
            this.panelControl2.Text = "panelControl2";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.labelControl17);
            this.panelControl3.Controls.Add(this.txt_GroupName);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Location = new System.Drawing.Point(12, 12);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(528, 33);
            this.panelControl3.TabIndex = 8;
            this.panelControl3.Text = "panelControl3";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(383, 477);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 9;
            this.btn_OK.Text = "确定";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(464, 477);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 10;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // labelControl17
            // 
            this.labelControl17.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl17.Appearance.Options.UseForeColor = true;
            this.labelControl17.Location = new System.Drawing.Point(367, 13);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(7, 14);
            this.labelControl17.TabIndex = 52;
            this.labelControl17.Text = "*";
            // 
            // RightEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 512);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RightEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "权限组编辑";
            this.Load += new System.EventHandler(this.RightEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxAllRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxHasRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_GroupName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl listBoxAllRight;
        private DevExpress.XtraEditors.ListBoxControl listBoxHasRight;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_GroupName;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btn_Del;
        private DevExpress.XtraEditors.SimpleButton btn_AddAll;
        private DevExpress.XtraEditors.SimpleButton btn_Add;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.SimpleButton btn_Cancel;
        private DevExpress.XtraEditors.SimpleButton btn_delAll;
        private DevExpress.XtraEditors.LabelControl labelControl17;

    }
}