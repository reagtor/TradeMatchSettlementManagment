namespace GTA.VTS.CustomersOrders.AppForm
{
    partial class ModifyOrderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModifyOrderForm));
            this.gbOldOrder = new System.Windows.Forms.GroupBox();
            this.txtOldAmount = new System.Windows.Forms.TextBox();
            this.lbAmount = new System.Windows.Forms.Label();
            this.txtOldPrice = new System.Windows.Forms.TextBox();
            this.lbPrice = new System.Windows.Forms.Label();
            this.txtEntrustNumber = new System.Windows.Forms.TextBox();
            this.lbEntrustNumber = new System.Windows.Forms.Label();
            this.txtNewAmount = new System.Windows.Forms.TextBox();
            this.lbNewAmount = new System.Windows.Forms.Label();
            this.gbNewOrder = new System.Windows.Forms.GroupBox();
            this.txtNewPrice = new System.Windows.Forms.TextBox();
            this.lbNewPrice = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbHKUnit = new System.Windows.Forms.ComboBox();
            this.lbUnit = new System.Windows.Forms.Label();
            this.gbOldOrder.SuspendLayout();
            this.gbNewOrder.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOldOrder
            // 
            this.gbOldOrder.Controls.Add(this.txtOldAmount);
            this.gbOldOrder.Controls.Add(this.lbAmount);
            this.gbOldOrder.Controls.Add(this.txtOldPrice);
            this.gbOldOrder.Controls.Add(this.lbPrice);
            this.gbOldOrder.Controls.Add(this.txtEntrustNumber);
            this.gbOldOrder.Controls.Add(this.lbEntrustNumber);
            this.gbOldOrder.Location = new System.Drawing.Point(12, 12);
            this.gbOldOrder.Name = "gbOldOrder";
            this.gbOldOrder.Size = new System.Drawing.Size(335, 100);
            this.gbOldOrder.TabIndex = 0;
            this.gbOldOrder.TabStop = false;
            this.gbOldOrder.Text = "Original Order";
            // 
            // txtOldAmount
            // 
            this.txtOldAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldAmount.Location = new System.Drawing.Point(114, 70);
            this.txtOldAmount.Name = "txtOldAmount";
            this.txtOldAmount.ReadOnly = true;
            this.txtOldAmount.Size = new System.Drawing.Size(201, 21);
            this.txtOldAmount.TabIndex = 5;
            this.txtOldAmount.TabStop = false;
            // 
            // lbAmount
            // 
            this.lbAmount.AutoSize = true;
            this.lbAmount.Location = new System.Drawing.Point(19, 73);
            this.lbAmount.Name = "lbAmount";
            this.lbAmount.Size = new System.Drawing.Size(47, 12);
            this.lbAmount.TabIndex = 4;
            this.lbAmount.Text = "Amount:";
            // 
            // txtOldPrice
            // 
            this.txtOldPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldPrice.Location = new System.Drawing.Point(114, 44);
            this.txtOldPrice.Name = "txtOldPrice";
            this.txtOldPrice.ReadOnly = true;
            this.txtOldPrice.Size = new System.Drawing.Size(201, 21);
            this.txtOldPrice.TabIndex = 3;
            this.txtOldPrice.TabStop = false;
            // 
            // lbPrice
            // 
            this.lbPrice.AutoSize = true;
            this.lbPrice.Location = new System.Drawing.Point(19, 47);
            this.lbPrice.Name = "lbPrice";
            this.lbPrice.Size = new System.Drawing.Size(41, 12);
            this.lbPrice.TabIndex = 2;
            this.lbPrice.Text = "Price:";
            // 
            // txtEntrustNumber
            // 
            this.txtEntrustNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEntrustNumber.Location = new System.Drawing.Point(114, 19);
            this.txtEntrustNumber.Name = "txtEntrustNumber";
            this.txtEntrustNumber.ReadOnly = true;
            this.txtEntrustNumber.Size = new System.Drawing.Size(201, 21);
            this.txtEntrustNumber.TabIndex = 1;
            this.txtEntrustNumber.TabStop = false;
            // 
            // lbEntrustNumber
            // 
            this.lbEntrustNumber.AutoSize = true;
            this.lbEntrustNumber.Location = new System.Drawing.Point(19, 22);
            this.lbEntrustNumber.Name = "lbEntrustNumber";
            this.lbEntrustNumber.Size = new System.Drawing.Size(89, 12);
            this.lbEntrustNumber.TabIndex = 0;
            this.lbEntrustNumber.Text = "EntrustNumber:";
            // 
            // txtNewAmount
            // 
            this.txtNewAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewAmount.Location = new System.Drawing.Point(114, 44);
            this.txtNewAmount.Name = "txtNewAmount";
            this.txtNewAmount.Size = new System.Drawing.Size(201, 21);
            this.txtNewAmount.TabIndex = 5;
            // 
            // lbNewAmount
            // 
            this.lbNewAmount.AutoSize = true;
            this.lbNewAmount.Location = new System.Drawing.Point(19, 47);
            this.lbNewAmount.Name = "lbNewAmount";
            this.lbNewAmount.Size = new System.Drawing.Size(47, 12);
            this.lbNewAmount.TabIndex = 4;
            this.lbNewAmount.Text = "Amount:";
            // 
            // gbNewOrder
            // 
            this.gbNewOrder.Controls.Add(this.cbHKUnit);
            this.gbNewOrder.Controls.Add(this.lbUnit);
            this.gbNewOrder.Controls.Add(this.txtNewAmount);
            this.gbNewOrder.Controls.Add(this.lbNewAmount);
            this.gbNewOrder.Controls.Add(this.txtNewPrice);
            this.gbNewOrder.Controls.Add(this.lbNewPrice);
            this.gbNewOrder.Location = new System.Drawing.Point(12, 120);
            this.gbNewOrder.Name = "gbNewOrder";
            this.gbNewOrder.Size = new System.Drawing.Size(335, 105);
            this.gbNewOrder.TabIndex = 1;
            this.gbNewOrder.TabStop = false;
            this.gbNewOrder.Text = "New Order";
            // 
            // txtNewPrice
            // 
            this.txtNewPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewPrice.Location = new System.Drawing.Point(114, 18);
            this.txtNewPrice.Name = "txtNewPrice";
            this.txtNewPrice.Size = new System.Drawing.Size(201, 21);
            this.txtNewPrice.TabIndex = 3;
            // 
            // lbNewPrice
            // 
            this.lbNewPrice.AutoSize = true;
            this.lbNewPrice.Location = new System.Drawing.Point(19, 21);
            this.lbNewPrice.Name = "lbNewPrice";
            this.lbNewPrice.Size = new System.Drawing.Size(41, 12);
            this.lbNewPrice.TabIndex = 2;
            this.lbNewPrice.Text = "Price:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Location = new System.Drawing.Point(218, 238);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Location = new System.Drawing.Point(66, 238);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbHKUnit
            // 
            this.cbHKUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHKUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbHKUnit.FormattingEnabled = true;
            this.cbHKUnit.Items.AddRange(new object[] {
            "Thigh股1",
            "Hand手2",
            "Sheet张3",
            "Share份8"});
            this.cbHKUnit.Location = new System.Drawing.Point(114, 71);
            this.cbHKUnit.Name = "cbHKUnit";
            this.cbHKUnit.Size = new System.Drawing.Size(100, 20);
            this.cbHKUnit.TabIndex = 30;
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Location = new System.Drawing.Point(19, 74);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(35, 12);
            this.lbUnit.TabIndex = 29;
            this.lbUnit.Text = "Unit:";
            // 
            // ModifyOrderForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(359, 276);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbNewOrder);
            this.Controls.Add(this.gbOldOrder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyOrderForm";
            this.Text = "Modify Order";
            this.Load += new System.EventHandler(this.ModifyOrderForm_Load);
            this.gbOldOrder.ResumeLayout(false);
            this.gbOldOrder.PerformLayout();
            this.gbNewOrder.ResumeLayout(false);
            this.gbNewOrder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOldOrder;
        private System.Windows.Forms.TextBox txtOldAmount;
        private System.Windows.Forms.Label lbAmount;
        private System.Windows.Forms.TextBox txtOldPrice;
        private System.Windows.Forms.Label lbPrice;
        private System.Windows.Forms.TextBox txtEntrustNumber;
        private System.Windows.Forms.Label lbEntrustNumber;
        private System.Windows.Forms.TextBox txtNewAmount;
        private System.Windows.Forms.Label lbNewAmount;
        private System.Windows.Forms.GroupBox gbNewOrder;
        private System.Windows.Forms.TextBox txtNewPrice;
        private System.Windows.Forms.Label lbNewPrice;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbHKUnit;
        private System.Windows.Forms.Label lbUnit;
    }
}