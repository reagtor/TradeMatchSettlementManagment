#region Using Namespace

using System;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;

#endregion

namespace ReckoningCounter.PushBackTest
{
    public partial class ModifyOrderForm : Form
    {
        private HKMessage message;
        private float modifyAmount;

        private float modifyPrice;
        private Types.UnitType modifyUnitType;

        public ModifyOrderForm(HKMessage message)
        {
            InitializeComponent();

            this.message = message;

            Initialize();
        }

        public float ModifyAmount
        {
            get { return modifyAmount; }
            private set { modifyAmount = value; }
        }

        public float ModifyPrice
        {
            get { return modifyPrice; }
            private set { modifyPrice = value; }
        }

        public Types.UnitType ModifyUnitType
        {
            get
            {
                return modifyUnitType;
            }
            private set
            {
                modifyUnitType = value;
            }
        }

        private void Initialize()
        {
            txtEntrustNumber.Text = message.EntrustNumber;
            txtOldPrice.Text = message.EntrustPrice;
            txtOldAmount.Text = message.EntrustAmount;

            txtNewPrice.Text = message.EntrustPrice;
            //txtNewAmount.Text = message.EntrustAmount;

            cbHKUnit.SelectedIndex = 0;
        }

        private void ModifyOrderForm_Load(object sender, EventArgs e)
        {
            txtNewPrice.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckValid())
                return;

            this.DialogResult = DialogResult.OK;
        }

        private bool CheckValid()
        {
            if (string.IsNullOrEmpty(txtNewPrice.Text))
            {
                string msg = "Please input a new price!";
                MessageBox.Show(msg);
                return false;
            }

            if (string.IsNullOrEmpty(txtNewAmount.Text))
            {
                string msg = "Please input a new amount!";
                MessageBox.Show(msg);
                return false;
            }

            float price = 0;
            bool canParsePrice = float.TryParse(txtNewPrice.Text, out price);
            if (!canParsePrice)
            {
                string msg = "Please input a valid price!";
                MessageBox.Show(msg);
                return false;
            }

            ModifyPrice = price;

            float amount = 0;
            bool canParseAmount = float.TryParse(txtNewAmount.Text, out amount);
            if (!canParseAmount)
            {
                string msg = "Please input a valid amount!";
                MessageBox.Show(msg);
                return false;
            }

            ModifyAmount = amount;

            modifyUnitType = Utils.GetUnit(cbHKUnit.Text.Trim());

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
    }
}