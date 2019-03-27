using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagementCenter.Model.CommonClass;

namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 交易员管理左边导航控件
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public partial class LeftControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public LeftControl()
        {
            InitializeComponent();
        }
        #endregion

        #region
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
        /// 帐户管理超连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccountManage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.ButtonClick != null) ButtonClick(ButtonFunctionTypes.TransactionLeftControlType.AccountManage);
        }
        /// <summary>
        /// 追加资金超连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFundManage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.ButtonClick != null) ButtonClick(ButtonFunctionTypes.TransactionLeftControlType.AddFundManage);
        }
        /// <summary>
        /// 冻结/解冻超连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FreezeManage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.ButtonClick != null) ButtonClick(ButtonFunctionTypes.TransactionLeftControlType.FreezeManage);
        }

        /// <summary>
        /// 转账管理超连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferManage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.ButtonClick != null) ButtonClick(ButtonFunctionTypes.TransactionLeftControlType.TransferManageUI);

        }
        #endregion


    }
}
