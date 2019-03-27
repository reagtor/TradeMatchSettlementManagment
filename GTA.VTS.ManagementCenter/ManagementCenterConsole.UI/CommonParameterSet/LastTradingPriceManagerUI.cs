using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ManagementCenter.BLL.QH;

namespace ManagementCenterConsole.UI.CommonParameterSet
{
    /// <summary>
    /// Desc: 结算价管理
    /// Create By: 董鹏
    /// Create Date:2010-03-11
    /// </summary>
    public partial class LastTradingPriceManagerUI : DevExpress.XtraEditors.XtraForm
    {
        QH_LastTradingPriceBLL qh_LastTradingPriceBLL;

        public LastTradingPriceManagerUI()
        {
            InitializeComponent();
            qh_LastTradingPriceBLL = new QH_LastTradingPriceBLL();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastTradingPriceManagerUI_Load(object sender, EventArgs e)
        {
            ManagementCenter.Model.Profession pro;
            List<ManagementCenter.Model.Profession> list=new List<ManagementCenter.Model.Profession>();
            pro =new ManagementCenter.Model.Profession();
            pro.EnNindnme="aa";
            pro.Nindcd="aa2";
            pro.Nindnme="aa3";
            list.Add(pro);
            pro =new ManagementCenter.Model.Profession();
            pro.EnNindnme="bb";
            pro.Nindcd="bb2";
            pro.Nindnme="bb3";
            list.Add(pro);

            gridControl1.DataSource = list;

            btnReckoning.Enabled = true;
        }

        /// <summary>
        /// 点击手动清算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReckoning_Click(object sender, EventArgs e)
        {
            List<ManagementCenter.Model.Profession> list = (List<ManagementCenter.Model.Profession>)gridControl1.DataSource;
            foreach (ManagementCenter.Model.Profession item in list)
            {
                if (item.Nindnme == "")
                {
                    MessageBox.Show("请输入结算价！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //            qh_LastTradingPriceBLL.DoManualReckoning();
        }
    }
}
