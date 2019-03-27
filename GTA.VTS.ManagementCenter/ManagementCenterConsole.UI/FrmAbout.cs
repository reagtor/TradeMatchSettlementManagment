using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ManagementCenterConsole.UI
{
    /// <summary>
    /// 描述：关于窗体 错误编码范围:
    /// 作者：刘书伟
    /// 日期：2009-07-21
    /// </summary>
    public partial class FrmAbout : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public FrmAbout()
        {
            InitializeComponent();
        }
        #endregion

        #region 确定按钮事件
        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 链接到公司网址 事件 linkGtahttp_LinkClicked
        /// <summary>
        /// 链接到公司网址 事件 linkGtahttp_LinkClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkGtahttp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.gtafe.com");
        }
        #endregion

    }
}
