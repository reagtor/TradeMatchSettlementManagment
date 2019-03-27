namespace ReckoningCapabilityTest
{
    partial class TransManageTestUI
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
            this.btnAddUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddFund = new DevExpress.XtraEditors.SimpleButton();
            this.btnTranMoney = new DevExpress.XtraEditors.SimpleButton();
            this.btnQueryMoney = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(68, 12);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(75, 23);
            this.btnAddUser.TabIndex = 0;
            this.btnAddUser.Text = "测试开户";
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnAddFund
            // 
            this.btnAddFund.Location = new System.Drawing.Point(57, 78);
            this.btnAddFund.Name = "btnAddFund";
            this.btnAddFund.Size = new System.Drawing.Size(119, 23);
            this.btnAddFund.TabIndex = 1;
            this.btnAddFund.Text = "测试追加资金";
            this.btnAddFund.Click += new System.EventHandler(this.btnAddFund_Click);
            // 
            // btnTranMoney
            // 
            this.btnTranMoney.Location = new System.Drawing.Point(68, 149);
            this.btnTranMoney.Name = "btnTranMoney";
            this.btnTranMoney.Size = new System.Drawing.Size(75, 23);
            this.btnTranMoney.TabIndex = 2;
            this.btnTranMoney.Text = "测试转账";
            this.btnTranMoney.Click += new System.EventHandler(this.btnTranMoney_Click);
            // 
            // btnQueryMoney
            // 
            this.btnQueryMoney.Location = new System.Drawing.Point(68, 210);
            this.btnQueryMoney.Name = "btnQueryMoney";
            this.btnQueryMoney.Size = new System.Drawing.Size(116, 23);
            this.btnQueryMoney.TabIndex = 3;
            this.btnQueryMoney.Text = "测试资金查询";
            this.btnQueryMoney.Click += new System.EventHandler(this.btnQueryMoney_Click);
            // 
            // TransManageTestUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 372);
            this.Controls.Add(this.btnQueryMoney);
            this.Controls.Add(this.btnTranMoney);
            this.Controls.Add(this.btnAddFund);
            this.Controls.Add(this.btnAddUser);
            this.Name = "TransManageTestUI";
            this.Text = "TransManageTestUI";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnAddUser;
        private DevExpress.XtraEditors.SimpleButton btnAddFund;
        private DevExpress.XtraEditors.SimpleButton btnTranMoney;
        private DevExpress.XtraEditors.SimpleButton btnQueryMoney;
    }
}