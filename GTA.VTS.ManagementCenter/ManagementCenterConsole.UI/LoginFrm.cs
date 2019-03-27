using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using DevExpress.DevLocalizer;
using ManagementCenter.Model;
using ManagementCenterConsole.UI.CommonClass;
using ManagementCenter.BLL;
using ManagementCenter.Model.CommonClass;
using Types = ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI
{
    public partial class LoginFrm : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 虚拟交易系统后台管理中心登陆界面
        /// 作者：熊晓凌
        /// 日期：2008-12-09
        /// </summary>
        #region 构造函数
        public LoginFrm()
        {
            InitializeComponent();
        }
        #endregion

        #region 登录窗体加载事件 LoginFrm_Load
        private void LoginFrm_Load(object sender, EventArgs e)
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            //第三方控件汉化加载
            DevEditLocalizer myeditLocalizer = new DevEditLocalizer();
            DevGridLocalizer mygridLocalizer = new DevGridLocalizer();
            DevBarLocalizer mybarLocalizer = new DevBarLocalizer();
            //DevPrintLocalizer myprintLocalizer = new DevPrintLocalizer();
            DevExpress.XtraEditors.Controls.Localizer.Active = myeditLocalizer;
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = mygridLocalizer;
            DevExpress.XtraBars.Localization.BarResLocalizer.Active = mybarLocalizer;
        }
        /// <summary>
        /// 异常捕捉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        //{
        //    if (e.Exception is System.IO.FileLoadException)
        //    {
        //        Application.ExitThread();
        //        return;
        //    }
        //}

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // LogHelper.WriteError(e.Exception.Message, e.Exception);

            LogHelper.WriteError("********************管理中心控制台Application异常:*******************\r\n", e.Exception);
        } 

        #endregion

        #region 确定按钮事件 btn_OK_Click
        /// <summary>
        /// 确定按钮事件 btn_OK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txt_LoginName.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("登陆名称不能为空!");
                    return;
                }
                if (this.txt_PassWord.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("密码不能为空!");
                    return;
                }
                UM_UserInfoBLL UserInfoBLL = new UM_UserInfoBLL();
                UM_UserInfo UserInfo = UserInfoBLL.ManagerLoginConfirm(this.txt_LoginName.Text, UtilityClass.DesEncrypt(this.txt_PassWord.Text, string.Empty),
                                                                       (int)Types.AddTpyeEnum.BackManager);
                if (UserInfo != null)
                {
                    ParameterSetting.Mananger = UserInfo;
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowMessageBox.ShowInformation("用户名或密码错误,请重新输入!");

                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation(ex.Message.ToString());
                return;
            }
            this.txt_LoginName.Focus();
        }
        #endregion

        #region  取消按钮事件 btn_Cancel_Click
        /// <summary>
        /// 取消按钮事件 btn_Cancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 找回按钮连接事件 hyperLinkEdit1_Click
        /// <summary>
        /// 找回按钮连接事件 hyperLinkEdit1_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hyperLinkEdit1_Click(object sender, EventArgs e)
        {
            LookBackPwd lookbackpwd = new LookBackPwd();
            lookbackpwd.ShowDialog();
        }
        #endregion

        #region 用户名文本框事件  txt_LoginName_KeyUp
        /// <summary>
        /// 用户名文本框事件  txt_LoginName_KeyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_LoginName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_PassWord.Focus();
            }
        }
        #endregion

        #region 密码文本框事件 txt_PassWord_KeyUp
        /// <summary>
        /// 密码文本框事件 txt_PassWord_KeyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_PassWord_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_OK_Click(null, null);
            }
        }
        #endregion

    }
}
