namespace CounterServiceManager
{
    partial class mainfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainfrm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLable = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.CounterIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRealtime = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBreakConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmtReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.运行时消息MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtMessageRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OfferMessageOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.McRptMessageMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLable,
            this.StatusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 590);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(820, 24);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLable
            // 
            this.StatusLable.Name = "StatusLable";
            this.StatusLable.Size = new System.Drawing.Size(116, 19);
            this.StatusLable.Tag = global::ReckoningCounterService.UI.Resource.UIResource.SSStautBarText_A;
            this.StatusLable.Text = "行情服务连接状态:";
            // 
            // StatusText
            // 
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(0, 19);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSystem,
            this.tsmiRealtime,
            this.运行时消息MToolStripMenuItem,
            this.tsmiAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(820, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiSystem
            // 
            this.tsmiSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CounterIDToolStripMenuItem,
            this.tsmiQuit});
            this.tsmiSystem.Name = "tsmiSystem";
            this.tsmiSystem.Size = new System.Drawing.Size(63, 23);
            this.tsmiSystem.Text = "系统(&S)";
            // 
            // CounterIDToolStripMenuItem
            // 
            this.CounterIDToolStripMenuItem.Name = "CounterIDToolStripMenuItem";
            this.CounterIDToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.CounterIDToolStripMenuItem.Text = "柜台标识";
            this.CounterIDToolStripMenuItem.Click += new System.EventHandler(this.CounterIDToolStripMenuItem_Click);
            // 
            // tsmiQuit
            // 
            this.tsmiQuit.Image = global::ReckoningCounterService.UI.Properties.Resources.Exit;
            this.tsmiQuit.Name = "tsmiQuit";
            this.tsmiQuit.Size = new System.Drawing.Size(152, 24);
            this.tsmiQuit.Text = "退出(&Q)";
            this.tsmiQuit.Visible = false;
            this.tsmiQuit.Click += new System.EventHandler(this.tsmiQuit_Click);
            // 
            // tsmiRealtime
            // 
            this.tsmiRealtime.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBreakConnection,
            this.tsmtReconnect});
            this.tsmiRealtime.Name = "tsmiRealtime";
            this.tsmiRealtime.Size = new System.Drawing.Size(89, 23);
            this.tsmiRealtime.Text = "实时行情(&R)";
            this.tsmiRealtime.Visible = false;
            // 
            // tsmiBreakConnection
            // 
            this.tsmiBreakConnection.Image = global::ReckoningCounterService.UI.Properties.Resources.DisConn;
            this.tsmiBreakConnection.Name = "tsmiBreakConnection";
            this.tsmiBreakConnection.Size = new System.Drawing.Size(147, 24);
            this.tsmiBreakConnection.Text = "断开连接(&B)";
            this.tsmiBreakConnection.Click += new System.EventHandler(this.tsmiBreakConnection_Click);
            // 
            // tsmtReconnect
            // 
            this.tsmtReconnect.Image = global::ReckoningCounterService.UI.Properties.Resources.ReConn;
            this.tsmtReconnect.Name = "tsmtReconnect";
            this.tsmtReconnect.Size = new System.Drawing.Size(147, 24);
            this.tsmtReconnect.Text = "重新连接(&C)";
            this.tsmtReconnect.Click += new System.EventHandler(this.tsmtReconnect_Click);
            // 
            // 运行时消息MToolStripMenuItem
            // 
            this.运行时消息MToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rtMessageRToolStripMenuItem,
            this.OfferMessageOToolStripMenuItem,
            this.McRptMessageMToolStripMenuItem});
            this.运行时消息MToolStripMenuItem.Name = "运行时消息MToolStripMenuItem";
            this.运行时消息MToolStripMenuItem.Size = new System.Drawing.Size(107, 23);
            this.运行时消息MToolStripMenuItem.Text = "运行时消息(&M)";
            // 
            // rtMessageRToolStripMenuItem
            // 
            this.rtMessageRToolStripMenuItem.Image = global::ReckoningCounterService.UI.Properties.Resources.RealMessage;
            this.rtMessageRToolStripMenuItem.Name = "rtMessageRToolStripMenuItem";
            this.rtMessageRToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.rtMessageRToolStripMenuItem.Tag = "行情消息";
            this.rtMessageRToolStripMenuItem.Text = "行情消息(&R)";
            this.rtMessageRToolStripMenuItem.Click += new System.EventHandler(this.rtMessageRToolStripMenuItem_Click);
            // 
            // OfferMessageOToolStripMenuItem
            // 
            this.OfferMessageOToolStripMenuItem.Name = "OfferMessageOToolStripMenuItem";
            this.OfferMessageOToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.OfferMessageOToolStripMenuItem.Tag = "报盘消息";
            this.OfferMessageOToolStripMenuItem.Text = "报盘消息(&O)";
            this.OfferMessageOToolStripMenuItem.Click += new System.EventHandler(this.OfferMessageOToolStripMenuItem_Click);
            // 
            // McRptMessageMToolStripMenuItem
            // 
            this.McRptMessageMToolStripMenuItem.Image = global::ReckoningCounterService.UI.Properties.Resources.MatchMessage;
            this.McRptMessageMToolStripMenuItem.Name = "McRptMessageMToolStripMenuItem";
            this.McRptMessageMToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.McRptMessageMToolStripMenuItem.Tag = "回报消息";
            this.McRptMessageMToolStripMenuItem.Text = "撮合中心回报消息(&M)";
            this.McRptMessageMToolStripMenuItem.Click += new System.EventHandler(this.McRptMessageMToolStripMenuItem_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(64, 23);
            this.tsmiAbout.Text = "关于(&A)";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // mainfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ReckoningCounterService.UI.Properties.Resources.CounterBack;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(820, 614);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainfrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "瑞尔格特清算柜台";
            this.Load += new System.EventHandler(this.mainfrm_Load);
            this.SizeChanged += new System.EventHandler(this.mainfrm_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainfrm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLable;
        private System.Windows.Forms.ToolStripStatusLabel StatusText;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSystem;
        private System.Windows.Forms.ToolStripMenuItem tsmiQuit;
        private System.Windows.Forms.ToolStripMenuItem tsmiRealtime;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiBreakConnection;
        private System.Windows.Forms.ToolStripMenuItem tsmtReconnect;
        private System.Windows.Forms.ListBox lstMessages;
        private System.Windows.Forms.ToolStripMenuItem 运行时消息MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rtMessageRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OfferMessageOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem McRptMessageMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CounterIDToolStripMenuItem;

    }
}

