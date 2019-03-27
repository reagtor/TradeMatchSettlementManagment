using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// Desc: 多窗体界面主窗体
    /// Create by: 董鹏
    /// Create Date: 2010-04-26
    /// </summary>
    public partial class MdiMainForm2 : Form
    {
        Dictionary<Type, TabPage> MdiForms;

        string title;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MdiMainForm2()
        {
            MdiForms = new Dictionary<Type, TabPage>();

            InitializeComponent();

            tabControl1.MouseDoubleClick += new MouseEventHandler(tabControl1_MouseDoubleClick);

            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            title = this.Text;
        }

        /// <summary>
        /// 切换TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = title;
            if (tabControl1.SelectedTab != null)
            {
                this.Text = title + " - " + tabControl1.SelectedTab.Text;
            }
        }

        /// <summary>
        /// 双击Tab控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                foreach (var item in MdiForms)
                {
                    if (item.Value == tabControl1.SelectedTab)
                    {
                        MdiForms.Remove(item.Key);
                        tabControl1.TabPages.Remove(item.Value);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 打开子窗体
        /// </summary>
        /// <param name="type"></param>
        private void OpenMdiForm(Type type)
        {
            if (!MdiForms.ContainsKey(type))
            {
                Form frm = (Form)System.Reflection.Assembly.GetAssembly(type).CreateInstance(type.ToString());
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.TopLevel = false;

                TabPage tp = new TabPage();
                tabControl1.TabPages.Add(tp);
                tp.Text = frm.Text;
                tp.Show();

                tp.Controls.Add(frm);
                frm.Show();

                tabControl1.SelectedTab = tp;

                MdiForms.Add(type, tp);

                this.Text = title + " - " + tp.Text;
            }
            else
            {
                tabControl1.SelectedTab = MdiForms[type];
            }
        }

        #region 菜单点击事件

        /// <summary>
        /// 现货下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuXHOrder_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmXHOrder));
        }

        /// <summary>
        /// 港股下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHKOrder_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmHKOrder));
        }

        private void menuGZQHOrder_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmSIOrder));
        }

        private void menuSPQHOrder_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmCFOrder));
        }

        private void menuXHQuery_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmXHQuery));
        }

        private void menuHKQuery_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmHKQuery));
        }

        private void menuGZQHQuery_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmSIQuery));
        }

        private void menuSPQHQuery_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmCFQuery));
        }

        private void menuFlowQuery_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmFlowQuery));
        }

        private void menuConnection_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmConnection));
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(ConvertTransfer));
        }

        private void Update_Click(object sender, EventArgs e)
        {

        }

        private void menuExit_Click(object sender, EventArgs e)
        {           
        }

        private void menuReInitData_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmReinitCaptial));
        }

        private void ToolStripMenuItemAddCaption_Click(object sender, EventArgs e)
        {
            OpenMdiForm(typeof(frmAdditionalCaption));
        }

        #endregion

    }
}