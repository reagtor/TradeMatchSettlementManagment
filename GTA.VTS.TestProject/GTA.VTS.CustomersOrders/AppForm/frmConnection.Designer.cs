namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class frmConnection
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnection));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbXH = new System.Windows.Forms.CheckBox();
            this.lblEntrust = new System.Windows.Forms.Label();
            this.lblChannel = new System.Windows.Forms.Label();
            this.txtEnturstNo = new System.Windows.Forms.TextBox();
            this.txtChannelID = new System.Windows.Forms.TextBox();
            this.btnRegisterChannle = new System.Windows.Forms.Button();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.splitContainer1.Panel1.Controls.Add(this.cbXH);
            this.splitContainer1.Panel1.Controls.Add(this.lblEntrust);
            this.splitContainer1.Panel1.Controls.Add(this.lblChannel);
            this.splitContainer1.Panel1.Controls.Add(this.txtEnturstNo);
            this.splitContainer1.Panel1.Controls.Add(this.txtChannelID);
            this.splitContainer1.Panel1.Controls.Add(this.btnRegisterChannle);
            this.splitContainer1.Panel1.Controls.Add(this.btnDisConnect);
            this.splitContainer1.Panel1.Controls.Add(this.btnConnect);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox3);
            this.splitContainer1.Size = new System.Drawing.Size(1044, 642);
            this.splitContainer1.SplitterDistance = 105;
            this.splitContainer1.TabIndex = 16;
            // 
            // cbXH
            // 
            this.cbXH.AutoSize = true;
            this.cbXH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbXH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbXH.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbXH.Location = new System.Drawing.Point(783, 21);
            this.cbXH.Name = "cbXH";
            this.cbXH.Size = new System.Drawing.Size(33, 16);
            this.cbXH.TabIndex = 6;
            this.cbXH.Text = "XH";
            this.cbXH.UseVisualStyleBackColor = true;
            // 
            // lblEntrust
            // 
            this.lblEntrust.AutoSize = true;
            this.lblEntrust.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblEntrust.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblEntrust.Location = new System.Drawing.Point(781, 46);
            this.lblEntrust.Name = "lblEntrust";
            this.lblEntrust.Size = new System.Drawing.Size(59, 12);
            this.lblEntrust.TabIndex = 12;
            this.lblEntrust.Text = "EntrustNo";
            // 
            // lblChannel
            // 
            this.lblChannel.AutoSize = true;
            this.lblChannel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblChannel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblChannel.Location = new System.Drawing.Point(537, 46);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(59, 12);
            this.lblChannel.TabIndex = 13;
            this.lblChannel.Text = "ChannelID";
            // 
            // txtEnturstNo
            // 
            this.txtEnturstNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEnturstNo.Location = new System.Drawing.Point(846, 43);
            this.txtEnturstNo.Name = "txtEnturstNo";
            this.txtEnturstNo.Size = new System.Drawing.Size(155, 21);
            this.txtEnturstNo.TabIndex = 5;
            // 
            // txtChannelID
            // 
            this.txtChannelID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChannelID.Location = new System.Drawing.Point(602, 43);
            this.txtChannelID.Name = "txtChannelID";
            this.txtChannelID.Size = new System.Drawing.Size(155, 21);
            this.txtChannelID.TabIndex = 3;
            // 
            // btnRegisterChannle
            // 
            this.btnRegisterChannle.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnRegisterChannle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRegisterChannle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRegisterChannle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRegisterChannle.Location = new System.Drawing.Point(602, 14);
            this.btnRegisterChannle.Name = "btnRegisterChannle";
            this.btnRegisterChannle.Size = new System.Drawing.Size(145, 23);
            this.btnRegisterChannle.TabIndex = 4;
            this.btnRegisterChannle.Text = "ChangeEntrustChannel";
            this.btnRegisterChannle.UseVisualStyleBackColor = true;
            this.btnRegisterChannle.Click += new System.EventHandler(this.btnRegisterChannle_Click);
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnDisConnect.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnDisConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDisConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDisConnect.Location = new System.Drawing.Point(230, 27);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(131, 37);
            this.btnDisConnect.TabIndex = 2;
            this.btnDisConnect.Text = "DisConnect";
            this.btnDisConnect.UseVisualStyleBackColor = false;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackgroundImage = global::GTA.VTS.CustomersOrders.Properties.Resources.PressButton;
            this.btnConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConnect.Location = new System.Drawing.Point(33, 27);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(131, 37);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // listBox3
            // 
            this.listBox3.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 12;
            this.listBox3.Location = new System.Drawing.Point(0, 0);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(1044, 532);
            this.listBox3.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // frmConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1044, 642);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2000, 2000);
            this.MinimizeBox = false;
            this.Name = "frmConnection";
            this.Text = "柜台连接";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
      
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.CheckBox cbXH;
        private System.Windows.Forms.Label lblEntrust;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.TextBox txtEnturstNo;
        private System.Windows.Forms.TextBox txtChannelID;
        private System.Windows.Forms.Button btnRegisterChannle;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;

    }
}