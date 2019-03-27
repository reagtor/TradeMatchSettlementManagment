using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ReckoningCounterTest.OrderTest;

namespace ReckoningCounterTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 现货下单测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XHOrderTest ot = new XHOrderTest();
            ot.MdiParent = this;
            ot.Show();
        }
    }
}
