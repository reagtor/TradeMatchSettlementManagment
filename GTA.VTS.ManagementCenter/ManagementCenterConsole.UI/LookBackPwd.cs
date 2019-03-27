using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI
{
    /// <summary>
    /// 虚拟交易系统后台管理中心找会密码界面
    /// 作者：熊晓凌
    /// 日期：2008-12-09
    /// </summary>
    public partial class LookBackPwd :DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public LookBackPwd()
        {
            InitializeComponent();
        }
        #endregion

        #region 确定事件
        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txt_LoginName.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入登录名称!");
                    return;
                }
                if (this.txt_Answer.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入提示问题的答案!");
                    return;
                }
                int QuestionID = ((UComboItem)this.dll_QuestionID.SelectedItem).ValueIndex;
                UM_UserInfoBLL UserInfoBLL=new UM_UserInfoBLL();
                UM_UserInfo UserInfo= UserInfoBLL.SeekForPassword(this.txt_LoginName.Text, this.txt_Answer.Text, QuestionID);
                if(UserInfo==null)
                {
                    ShowMessageBox.ShowInformation("验证失败,请重新输入!");
                    return;
                }
                ShowMessageBox.ShowInformation(string.Format("你的登录密码为：{0}", UtilityClass.DesDecrypt(UserInfo.Password.ToString(),string.Empty)));
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        #region 取消事件
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookBackPwd_Load(object sender, EventArgs e)
        {
            try
            {
                this.dll_QuestionID.Properties.Items.Clear();
                UComboItem item;
                List<UM_QuestionType> L_QuestionType = GetQuestionTypeList();
                if (L_QuestionType != null)
                {
                    foreach (UM_QuestionType QuestionType in L_QuestionType)
                    {
                        item = new UComboItem(QuestionType.Content, QuestionType.QuestionID);
                        dll_QuestionID.Properties.Items.Add(item);
                    }
                    dll_QuestionID.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                return;  
            }
        }
        #endregion

        #region 得到问题种类列表

        /// <summary>
        /// 得到问题种类列表
        /// </summary>
        /// <returns></returns>
        private static List<UM_QuestionType> GetQuestionTypeList()
        {
            try
            {
                UM_QuestionTypeBLL QuestionTypeBLL = new UM_QuestionTypeBLL();
                List<UM_QuestionType> L_QuestionType = QuestionTypeBLL.GetListArray(string.Empty);
                if (L_QuestionType != null)
                {
                    return L_QuestionType;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

    }
}
