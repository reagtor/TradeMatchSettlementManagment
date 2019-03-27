using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReckoningCounterService.UI.RuntimeMessage
{
    public partial class RealtimeFrm : Form
    {
        public RealtimeFrm()
        {
            InitializeComponent();
        }

        public ListBox DisplayList
        {
            get
            {
                return this.listBox1;
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayList.Items.Clear();
        }

        private void RealtimeFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
