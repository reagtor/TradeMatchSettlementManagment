using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagementCenter.Model.CommonClass;

namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 描述：交易员管理主界面
    /// 作者：程序员：熊晓凌  修改：刘书伟
    /// 日期：2008-11-18     2009-10-23 
    /// </summary>
    public partial class TransactionManageForm : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public TransactionManageForm()
        {
            InitializeComponent();
        }
        #endregion

        #region 页面加载事件 TransactionManageForm_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransactionManageForm_Load(object sender, EventArgs e)
        {
            try
            {
                LeftControl leftControl = new LeftControl();
                leftControl.ButtonClick += new LeftControl.OnButtonClickHandle(leftControl_ButtonClick);
                leftControl.Dock = DockStyle.Fill;
                this.panelLeft.Controls.Add(leftControl);
            
            }
            catch { }
        }
        #endregion

        #region 委托左边导航条按纽事件 leftControl_ButtonClick
        /// <summary>
        /// 委托左边导航条按纽事件
        /// </summary>
        /// <param name="TransactionLeftControlType"></param>
        private void leftControl_ButtonClick(ManagementCenter.Model.CommonClass.ButtonFunctionTypes.TransactionLeftControlType TransactionLeftControlType)
        {
            switch (TransactionLeftControlType)
            {
                case ButtonFunctionTypes.TransactionLeftControlType.AccountManage:
                    this.panelRight.Controls.Clear();
                    AccountManage accountManage = new AccountManage();
                    accountManage.Dock = DockStyle.Fill;
                    this.panelRight.Controls.Add(accountManage); 
                    break;
                case ButtonFunctionTypes.TransactionLeftControlType.AddFundManage:
                    this.panelRight.Controls.Clear();
                    FundQuery fundQuery = new FundQuery();
                    fundQuery.Dock = DockStyle.Fill;
                    this.panelRight.Controls.Add(fundQuery); 
                    //new AddFundForm().ShowDialog();
                    break;
                case ButtonFunctionTypes.TransactionLeftControlType.FreezeManage:

                    this.panelRight.Controls.Clear();
                    FreezeManage freezeManage = new FreezeManage();
                    freezeManage.Dock = DockStyle.Fill;
                    this.panelRight.Controls.Add(freezeManage); 
                    break;
                case ButtonFunctionTypes.TransactionLeftControlType.TransferManageUI:

                    this.panelRight.Controls.Clear();
                    TransferManageUI transferManageUI = new TransferManageUI();
                    transferManageUI.Dock = DockStyle.Fill;
                    this.panelRight.Controls.Add(transferManageUI);
                    break;
            }
        }
        #endregion
    }
}
