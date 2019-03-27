using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagementCenter.Model.CommonClass;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 撮合中心管理框架页
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class MatchCenterManage :DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatchCenterManage()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MatchCenterManage_Load(object sender, EventArgs e)
        {
            LeftNavControl leftNavControl = new LeftNavControl();
            leftNavControl.ButtonClick += new LeftNavControl.OnButtonClickHandle(leftNavControl_ButtonClick);
            leftNavControl.Dock = DockStyle.Fill;
            this.panelLeft.Controls.Add(leftNavControl);
        }

        /// <summary>
        /// 按纽事件
        /// </summary>
        /// <param name="TransactionLeftControlType"></param>
        private void leftNavControl_ButtonClick(ManagementCenter.Model.CommonClass.ButtonFunctionTypes.TransactionLeftControlType TransactionLeftControlType)
        {
            switch (TransactionLeftControlType)
            {
                case ButtonFunctionTypes.TransactionLeftControlType.CenterManage:
                    this.panelRight.Controls.Clear();
                    CenterManage centerManage = new CenterManage();
                    centerManage.Dock = DockStyle.Fill;
                    this.panelRight.Controls.Add(centerManage);
                    break;
                case ButtonFunctionTypes.TransactionLeftControlType.MachineManage:
                    this.panelRight.Controls.Clear();
                    MachineManage machineManage = new MachineManage();
                    machineManage.Dock = DockStyle.Fill;
                    this.panelRight.Controls.Add(machineManage);
                    //new AddFundForm().ShowDialog();
                    break;
            }
        }
    }
}
