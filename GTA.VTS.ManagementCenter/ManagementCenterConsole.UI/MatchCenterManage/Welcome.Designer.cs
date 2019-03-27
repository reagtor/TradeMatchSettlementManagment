namespace ManagementCenterConsole.UI.MatchCenterManage
{
    partial class Welcome
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(82, 39);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(256, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "此向导将帮助您简单快速的配置撮合中心,撮合机";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(45, 69);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(136, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "以及代码撮合分配的关系.";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(82, 104);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(264, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "运用此向导将覆盖之前在撮合中心管理设置的信息";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(45, 139);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(228, 14);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "请谨慎使用,如果您是第一次配置,尽请放心!";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(82, 185);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(126, 14);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "要继续,请单击\"下一步\".";
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "Welcome";
            this.Size = new System.Drawing.Size(460, 240);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}
