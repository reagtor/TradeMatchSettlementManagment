using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagementCenter.Model.CommonClass;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 描述：左部导航控件 
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class LeftNavControl :DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LeftNavControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 委托事件
        /// </summary>
        /// <param name="TransactionLeftControlType"></param>
        public delegate void OnButtonClickHandle(ButtonFunctionTypes.TransactionLeftControlType TransactionLeftControlType);
        /// <summary>
        /// 事件名称
        /// </summary>
        public event OnButtonClickHandle ButtonClick;

        /// <summary>
        /// 撮合中心链接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterManage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.ButtonClick != null) ButtonClick(ButtonFunctionTypes.TransactionLeftControlType.CenterManage);
        }

        /// <summary>
        /// 撮合机链接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MachineManage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.ButtonClick != null) ButtonClick(ButtonFunctionTypes.TransactionLeftControlType.MachineManage);
        }
    }
}
