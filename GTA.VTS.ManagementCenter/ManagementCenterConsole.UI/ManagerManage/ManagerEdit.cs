using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.Model;
using ManagementCenterConsole.UI.CommonClass;
using Types = ManagementCenter.Model.CommonClass.Types;
using ManagementCenter.BLL;


namespace ManagementCenterConsole.UI.ManagerManage
{
    /// <summary>
    /// 管理员信息编辑  错误编码范围1021-1050
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-25
    /// </summary>
    public partial class ManagerEdit : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ManagerEdit()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// 检测用户信息消息
        /// </summary>
        private string JudgmentMessage = string.Empty;

        #region 操作类型　 1:添加,2:修改
        /// <summary>
        /// 是否是后台管理员
        /// </summary>
        public bool ispersonedit = false;

        private int m_EditType = 1;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get { return this.m_EditType; }
            set { this.m_EditType = value; }
        }

        #endregion

        #region 管理员实体
        /// <summary>
        /// 管理员实体
        /// </summary>
        private UM_UserInfo m_userInfo = null;

        /// <summary>
        /// 管理员实体属性
        /// </summary>
        public UM_UserInfo UserInfo
        {
            get { return this.m_userInfo; }
            set
            {
                this.m_userInfo = new UM_UserInfo();
                this.m_userInfo = value;
            }
        }

        #endregion

        #region 权限组ID　 

        private int m_rightGroupID;

        /// <summary>
        /// 权限组ID
        /// </summary>
        public int RightGroupID
        {
            get { return m_rightGroupID; }
            set { m_rightGroupID = value; }
        }

        #endregion

        #region 页面加载

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManagerEdit_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_EditType == 1)
                {
                    m_userInfo = new UM_UserInfo();
                }
                BinData();
                SetControl();
                if (m_EditType == 2)
                {
                    if (ispersonedit)
                    {
                        this.Text = "个人信息";
                        this.Icon = Icon.FromHandle(new Bitmap(imageList1.Images[0]).GetHicon());
                        if((int)UserInfo.RoleID!=(int)Types.RoleTypeEnum.Admin)
                        {
                            GetRightGroupByUserID(UserInfo.UserID);
                        }
                    }
                    InitManagerInfo();
                }
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("页面加载失败!");
                string errCode = "GL-1021";
                string errMsg = "页面加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 初始化用户信息

        /// <summary>
        /// 初始化用户信息
        /// </summary>
        public void InitManagerInfo()
        {
            if (this.m_userInfo != null)
            {
                this.txt_Answer.Text = this.m_userInfo.Answer;
                this.txt_Address.Text = this.m_userInfo.Address;
                this.txt_CertificateNo.Text = this.m_userInfo.CertificateNo;
                this.txt_Email.Text = this.m_userInfo.Email;
                this.txt_Name.Text = this.m_userInfo.UserName;
                this.txt_Password.Text = "******";
                this.txt_PassAgain.Text = "******";
                this.txt_Telephone.Text = this.m_userInfo.Telephone;
                this.txt_LoginName.Text = this.m_userInfo.LoginName;
                this.txt_Postalcode.Text = this.m_userInfo.Postalcode;

                foreach (object item in this.dll_rightgroup.Properties.Items)
                {
                    if (((UComboItem) item).ValueIndex == RightGroupID)
                    {
                        this.dll_rightgroup.SelectedItem = item;
                        break;
                    }
                }
                if (m_userInfo.CertificateStyle != null)
                {
                    foreach (object item in this.dll_CertificateStyle.Properties.Items)
                    {
                        if (((UComboItem) item).ValueIndex == m_userInfo.CertificateStyle)
                        {
                            this.dll_CertificateStyle.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    this.dll_CertificateStyle.SelectedIndex = 0;
                }
                if (m_userInfo.QuestionID != null)
                {
                    foreach (object item in this.dll_QuestionID.Properties.Items)
                    {
                        if (((UComboItem) item).ValueIndex == m_userInfo.QuestionID)
                        {
                            this.dll_QuestionID.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    this.dll_QuestionID.SelectedIndex = 0;
                }
            }
        }

        #endregion

        #region 绑定界面下拉框的数据

        /// <summary>
        /// 绑定界面下拉框的数据
        /// </summary>
        private void BinData()
        {
            try
            {
                this.dll_CertificateStyle.Properties.Items.Clear();
                this.dll_CertificateStyle.Properties.Items.AddRange(
                    CommonClass.ComboBoxDataSource.GetCertificateStyleList());
                dll_CertificateStyle.SelectedIndex = 0;

                this.dll_QuestionID.Properties.Items.Clear();
                this.dll_QuestionID.Properties.Items.AddRange(CommonClass.ComboBoxDataSource.GetQuestionTypeList());
                dll_QuestionID.SelectedIndex = 0;

                this.dll_rightgroup.Properties.Items.Clear();
                this.dll_rightgroup.Properties.Items.AddRange(CommonClass.ComboBoxDataSource.GetAllRightGroupList());
                dll_rightgroup.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-1022";
                string errMsg = "绑定界面下拉框的数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
        }

        #endregion

        #region 确定按纽

        /// <summary>
        /// 确定按纽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                ManagementCenter.BLL.UM_UserInfoBLL UserInfoBLL = new UM_UserInfoBLL();

                if (this.m_EditType == 1)
                {
                    if (CheckUserInfo())
                    {
                        if (UserInfoBLL.ManagerAdd(m_userInfo, m_rightGroupID))
                        {
                            ShowMessageBox.ShowInformation("添加成功!");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("添加失败!");
                        }
                    }
                }
                else
                {
                    if (this.m_userInfo.AddType == (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.FrontManager)
                    {
                        ShowMessageBox.ShowInformation("此用户为前台管理员,不允许修改!");
                        return;
                    }
                    if (CheckUserInfo())
                    {
                        if (UserInfoBLL.ManagerUpdate(m_userInfo, m_rightGroupID))
                        {
                            if (ispersonedit) CommonClass.ParameterSetting.Mananger = UserInfo;
                            ShowMessageBox.ShowInformation("修改成功!");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("修改失败!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-1023";
                string errMsg = "确定事件异常!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 取消按纽

        /// <summary>
        /// 取消按纽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 检测用户信息

        /// <summary>
        /// 检测用户信息
        /// </summary>
        /// <returns></returns>
        public bool CheckUserInfo()
        {
            try
            {
                JudgmentMessage = string.Empty;
                if (this.dll_rightgroup.Properties.Items.Count < 1)
                {
                    ShowMessageBox.ShowInformation("请先设置权限组!");
                    return false;
                }
                m_rightGroupID = ((UComboItem) this.dll_rightgroup.SelectedItem).ValueIndex;

                if(this.txt_LoginName.Text==string.Empty)
                {
                    ShowMessageBox.ShowInformation("管理员帐号不能为空!");//("登录名称不能为空!");
                    return false;
                }
                else
                {
                    if (!IsSuperLenth(this.txt_LoginName.Text, 20))
                    {
                        ShowMessageBox.ShowInformation("管理员帐号超出有效长度20!");//("登录名称不超出有效长度20!");
                        return false;
                    }
                    if(!InputTest.LoginTest(this.txt_LoginName.Text))
                    {
                        ShowMessageBox.ShowInformation("管理员帐号包含非法字符(只能包含数字、字母、以及下划线和－)!");//("登录名称包含非法字符(只能包含数字、字母、以及下划线和－)");
                        return false;
                    }
                    this.m_userInfo.LoginName = this.txt_LoginName.Text.ToString();
                }
                if (EditType == 1)
                {
                    ManagementCenter.BLL.UM_UserInfoBLL UserInfoBLL = new UM_UserInfoBLL();
                    if (!UserInfoBLL.CheckLoginName(this.m_userInfo.LoginName))
                    {
                        MessageBox.Show("管理员帐号已经存在，请重新输入!");//("登录名称已经存在，请重新输入!");
                        return false;
                    }
                }

                if(this.txt_Name.Text!=string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Name.Text, 20))
                    {
                        MessageBox.Show("真实姓名超出有效范围长度20!");//("用户姓名超出有效范围长度20!");
                        return false;
                    }
                    this.m_userInfo.UserName = this.txt_Name.Text.ToString();
                }
                else
                {
                    this.m_userInfo.UserName = null;
                }

                if (this.txt_CertificateNo.Text.ToString() != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_CertificateNo.Text.ToString(), 50))
                    {
                        ShowMessageBox.ShowInformation("证件号码超出有效范围长度50!");
                        return false;
                    }
                    this.m_userInfo.CertificateNo = this.txt_CertificateNo.Text.ToString();
                    this.m_userInfo.CertificateStyle = ((UComboItem)this.dll_CertificateStyle.SelectedItem).ValueIndex;
                }
                else
                {
                    this.m_userInfo.CertificateNo =null;
                    this.m_userInfo.CertificateStyle = null;
                }

                if (this.txt_Postalcode.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Postalcode.Text, 50))
                    {
                        ShowMessageBox.ShowInformation("邮政编码超出有效范围长度50!");
                        return false;
                    }
                    this.m_userInfo.Postalcode = this.txt_Postalcode.Text.ToString();
                }
                else
                {
                    this.m_userInfo.Postalcode = null;
                }
                if (this.txt_Email.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Email.Text, 50))
                    {
                        ShowMessageBox.ShowInformation("邮箱超出有效范围长度50!");
                        return false;
                    }
                    if (!InputTest.emailTest(this.txt_Email.Text))
                    {
                        ShowMessageBox.ShowInformation("邮箱格式错误,请输入正确的邮箱!");
                        return false;
                    }
                    this.m_userInfo.Email = this.txt_Email.Text;
                }
                else
                {
                    this.m_userInfo.Email = null;
                }

                if (this.txt_Telephone.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Telephone.Text, 20))
                    {
                        ShowMessageBox.ShowInformation("电话号码超出有效范围长度20!");
                        return false;
                    }
                    this.m_userInfo.Telephone = this.txt_Telephone.Text.ToString();
                }
                else
                {
                    this.m_userInfo.Telephone = null;
                }
                if (this.txt_Address.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Address.Text, 50))
                    {
                        ShowMessageBox.ShowInformation("地址超出有效范围长度50!");
                        return false;
                    }
                    this.m_userInfo.Address = this.txt_Address.Text.ToString();
                }
                else
                {
                    this.m_userInfo.Address = null;
                }

                if (this.txt_Answer.Text.ToString() != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Answer.Text, 200))
                    {
                        ShowMessageBox.ShowInformation("答案超出有效范围长度200!");
                        return false;
                    }
                    this.m_userInfo.Answer = this.txt_Answer.Text.ToString();
                    this.m_userInfo.QuestionID = ((UComboItem) this.dll_QuestionID.SelectedItem).ValueIndex;
                   
                }
                else
                {
                    this.m_userInfo.Answer = null;
                    this.m_userInfo.QuestionID =null;
                }
               
                this.m_userInfo.RoleID = (int) ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Manager;
                this.m_userInfo.AddType =(int) ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.BackManager;

                //登陆密码检测
                if (EditType == 1 || (EditType == 2 && this.che_UpdatePass.Checked == true))
                {
                    if (this.txt_Password.Text.ToString() == string.Empty || this.txt_PassAgain.Text.ToString() == string.Empty)
                    {
                        ShowMessageBox.ShowInformation("密码不能为空!");
                        return false;
                    }
                    if (!IsSuperLenth(this.txt_Password.Text.ToString(), 12))
                    {
                        MessageBox.Show("密码超出有效范围长度12!");
                        return false;
                    }
                    if (!InputTest.LoginTest(this.txt_Password.Text.ToString()))
                    {
                        MessageBox.Show("密码中包含非法字符!");
                        return false;
                    }
                    if (this.txt_Password.Text.ToString() != this.txt_PassAgain.Text.ToString())
                    {
                        MessageBox.Show("两次输入的密码不一致!");
                        return false;
                    }
                    this.m_userInfo.Password = this.txt_Password.Text.ToString();
                    if ((EditType == 2 && this.che_UpdatePass.Checked == true))
                    {
                        this.m_userInfo.Password =
                            ManagementCenter.Model.CommonClass.UtilityClass.DesEncrypt(
                                this.txt_Password.Text.ToString(), string.Empty);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("用户输入检测失败!");
                string errCode = "GL-1024";
                string errMsg = "用户输入检测失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 判断字符串是否为空或者长度大于指定长度

        /// <summary>
        /// 判断字符串是否为空或者长度大于指定长度
        /// </summary>
        /// <param name="Content">字段内容</param>
        /// <param name="Length">字段长度</param>
        /// <returns></returns>
        private bool LengthLessOrNull(string Content, int Length)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return true;
            }
            if (Length > 0)
            {
                if (Content.Length > Length)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsSuperLenth(string Content, int Lenth)
        {
            if (Content == string.Empty) return true;
            if (Content.Length <= Lenth) return true;
            return false;
        }

        #endregion

        #region 设置控件

        private void SetControl()
        {
            if (m_EditType == 1)
            {
                this.txt_LoginName.Enabled = true;
                this.che_UpdatePass.Visible = false;
            }
            else
            {
                this.txt_LoginName.Enabled = true; //false;
                this.che_UpdatePass.Visible = true;
                this.txt_Password.Enabled = false;
                this.txt_PassAgain.Enabled = false;
                if(ispersonedit) this.dll_rightgroup.Enabled = false;
            }
        }

        #endregion

        #region 修改密码事件

        private void che_UpdatePass_CheckedChanged(object sender, EventArgs e)
        {
            if (EditType == 1) return;
            if (this.che_UpdatePass.Checked == true)
            {
                this.txt_Password.Enabled = true;
                this.txt_PassAgain.Enabled = true;
                this.txt_Password.Text = string.Empty;
                this.txt_PassAgain.Text = string.Empty;
            }
            else
            {
                this.txt_Password.Enabled = false;
                this.txt_PassAgain.Enabled = false;
                this.txt_Password.Text = "********";
                this.txt_PassAgain.Text = "********";
            }
        }

        #endregion

        #region 根据管理员ID得到权限组ID
        /// <summary>
        /// 根据管理员ID得到权限组ID
        /// </summary>
        /// <param name="UserID">管理员ID</param>
        public void GetRightGroupByUserID(int UserID)
        {
            try
            {
                UM_ManagerBeloneToGroupBLL ManagerBeloneToGroupBLL = new UM_ManagerBeloneToGroupBLL();
                UM_ManagerBeloneToGroup ManagerBeloneToGroup = ManagerBeloneToGroupBLL.GetModel(UserID);
                RightGroupID = (int)ManagerBeloneToGroup.ManagerGroupID;
            }
            catch (Exception ex)
            {
                string errCode = "GL-1025";
                string errMsg = "根据管理员ID得到权限组ID失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }
        #endregion

    }
}