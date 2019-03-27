using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using ManagementCenterConsole.UI.CommonClass;
using Types = ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 帐户管理 错误编码范围0321-0330
    /// 作者：程序员：熊晓凌  刘书伟
    /// 日期：2008-11-18    新功能:2009-07-07
    /// </summary>
    public partial class AccountManage : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AccountManage()
        {
            InitializeComponent();
        }
        #endregion

        #region 变量

        /// <summary>
        /// 表格当前行号
        /// </summary>
        private int m_cutRow = -100;

        /// <summary>
        /// 当前页
        /// </summary>
        private int m_pageNo = int.MaxValue;

        /// <summary>
        /// 一页展示记录数
        /// </summary>
        private int m_pageSize = ParameterSetting.PageSize;

        /// <summary>
        /// 总记录数
        /// </summary>
        private int m_rowCount = int.MaxValue;

        /// <summary>
        /// 查询实体
        /// </summary>
        private UM_UserInfo QueryUserInfo;

        /// <summary>
        /// 用户管理
        /// </summary>
        private ManagementCenter.BLL.UM_UserInfoBLL UserInfoBLL;

        private bool isFirstInit = true;

        /// <summary>
        /// 提示信息的变量
        /// </summary>
        private ToolTip m_tip=new ToolTip();

        #endregion

        #region 添加交易员
        /// <summary>
        /// 添加交易员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Add_Click(object sender, EventArgs e)
        {
            UserInfoForm userInfoForm = new UserInfoForm();
            userInfoForm.EditType = 1;
            if (userInfoForm.ShowDialog(this) == DialogResult.OK)
            {
                //刷新列表
                InitUserList();
            }
        }
        #endregion

        #region 页面加载
        private void AccountManage_Load(object sender, EventArgs e)
        {
            try
            {
                UserInfoBLL=new UM_UserInfoBLL();
                SetQueryUserInfo();
                InitUserList();
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("窗体加载失败!");
                string errCode = "GL-0321";
                string errMsg = "窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
        }
        #endregion

        #region 初始化

        /// <summary>
        /// 初始化页面信息
        /// </summary>
        private void InitUserList()
        {
            try
            {
                this.m_pageNo = 1;
                //加载第一页数据
                LoadUser();
                //根据获取的页数初始化分页控件
                this.ucPageNavigation1.PageIndexChanged -=
                    new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(
                        ucPageNavigation1_PageIndexChanged);
                if (m_rowCount == 0)
                {
                    this.ucPageNavigation1.PageCount = 0;
                }
                else
                {
                    if (m_rowCount % this.m_pageSize == 0)
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize);
                    }
                    else
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize) + 1;
                    }
                }
                this.ucPageNavigation1.CurrentPage = this.m_pageNo;
                this.ucPageNavigation1.PageIndexChanged +=
                    new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(
                        ucPageNavigation1_PageIndexChanged);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0322";
                string errMsg = "表格数据初始化失败";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);  
            }
        }
        #endregion

        #region 加载用户列表

        /// <summary>
        /// 加载用户列表
        /// </summary>
        private void LoadUser()
        {
            try
            {
               
                DataSet ds = UserInfoBLL.GetPagingUser(QueryUserInfo, this.m_pageNo, this.m_pageSize,
                                                       out this.m_rowCount);
                DataTable allUser;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    allUser = new DataTable();
                   if(!isFirstInit) ShowMessageBox.ShowInformation("不存在记录!");
                }
                else
                {
                    allUser = ds.Tables[0];
                }
                ddlCertificateStyle.DataSource = CommonClass.ComboBoxDataSource.GetCertificateStyleList();
                ddlCertificateStyle.ValueMember = "ValueIndex";
                ddlCertificateStyle.DisplayMember = "TextTitleValue";

                this.gridUser.DataSource = allUser;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0323";
                string errMsg = "加载用户列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);  
            }
            isFirstInit = false;
        }

        #endregion

        #region 页码改变触发事件

        /// <summary>
        /// 页码改变触发事件
        /// </summary>
        /// <param name="page">查询的页数</param>
        private void ucPageNavigation1_PageIndexChanged(int page)
        {
            this.m_pageNo = page;
            LoadUser();
        }

        #endregion

        #region 设置查询实体对象

        /// <summary>
        /// 设置查询实体对象
        /// </summary>
        private bool SetQueryUserInfo()
        {
            if (QueryUserInfo == null)
            {
                QueryUserInfo = new UM_UserInfo();
            }
            if (this.txt_UserID.Text.Trim() == string.Empty)
            {
                QueryUserInfo.UserID = int.MaxValue;
            }
            else
            {
               if(InputTest.intTest(this.txt_UserID.Text.Trim()))
               {
                   QueryUserInfo.UserID = int.Parse(this.txt_UserID.Text.Trim());
               }
               else
               {
                   ShowMessageBox.ShowInformation("请输入正确的交易员编号!");
                   return false;
               }
            }
            QueryUserInfo.UserName = this.txt_Name.Text.Trim() != string.Empty
                                         ? this.txt_Name.Text.Trim()
                                         : string.Empty;

            //if (this.txt_CounterID.Text.Trim() == string.Empty)
            //{
            QueryUserInfo.CouterID = int.MaxValue;
            //}
            //else
            //{
            //    if (InputTest.intTest(this.txt_CounterID.Text.Trim()))
            //    {
            //        QueryUserInfo.CouterID = int.Parse(this.txt_CounterID.Text.Trim());
            //    }
            //    else
            //    {
            //        ShowMessageBox.ShowInformation("请输入正确的柜台编号!");
            //        return false;
            //    }
            //}
            //柜台名称
            QueryUserInfo.Name = this.txt_CounterName.Text.Trim() != string.Empty
                                         ? this.txt_CounterName.Text.Trim()
                                         : string.Empty;
            QueryUserInfo.LoginName = string.Empty;
            QueryUserInfo.RoleID = (int)Types.RoleTypeEnum.Transaction;
            return true;
        }

        #endregion

        #region 表格双击事件
        private void gridUser_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
               this.ViewUser.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

            if (this.ViewUser != null && this.ViewUser.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewUser.FocusedRowHandle;
                UserUpdate(m_cutRow);
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行!");
            }
        }
        #endregion

        #region 修改交易员

        /// <summary>
        /// 修改交易员
        /// </summary>
        /// <param name="handle">当前行</param>
        private void UserUpdate(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                UserInfoForm userInfoForm = new UserInfoForm();
                userInfoForm.EditType = 2;
                DataRow dw = ViewUser.GetDataRow(handle);
                int UserID = int.Parse(dw["UserID"].ToString());

                UM_UserInfo UserInfo = UserInfoBLL.GetModel(UserID);
                userInfoForm.CurrentUser = UserInfo;

                if (userInfoForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUser();
                    this.ViewUser.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0324";
                string errMsg = "修改交易员失败";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);  
            }
        }

        #endregion

        #region 修改按纽事件
        /// <summary>
        /// 修改按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Update_Click(object sender, EventArgs e)
        {
            if (this.ViewUser != null && this.ViewUser.FocusedRowHandle >= 0)
            {
                m_cutRow = this.ViewUser.FocusedRowHandle;
                UserUpdate(m_cutRow);
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行!");
            }
        }
        #endregion

        #region 查询按纽事件
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Query_Click(object sender, EventArgs e)
        {
            if(SetQueryUserInfo())
            {
                InitUserList();
            }
        }
        #endregion

        #region 删除按纽事件
        /// <summary>
        /// 删除按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                string mess;
                if (this.ViewUser != null && this.ViewUser.FocusedRowHandle >= 0)
                {
                    if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.Yes)
                    {
                        CT_CounterBLL CounterBLL = new CT_CounterBLL();

                        if (!CounterBLL.TestCenterConnection())
                        {
                            MessageBox.Show("柜台服务连接失败,请检查柜台服务是否开启!", "系统提示");
                            return;
                        }
                        m_cutRow = this.ViewUser.FocusedRowHandle;
                        DataRow dw = ViewUser.GetDataRow(m_cutRow);
                        int UserID = int.Parse(dw["UserID"].ToString());
                        ManagementCenter.BLL.UserManage.TransactionManage TransactionManage
                            = new ManagementCenter.BLL.UserManage.TransactionManage();
                        if (TransactionManage.DelTransaction(UserID, out mess))
                        {
                            ShowMessageBox.ShowInformation("删除成功!");
                            InitUserList();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation(mess);
                        }
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选中记录行!");
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0325";
                string errMsg = "删除交易员失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                ShowMessageBox.ShowInformation(exception.ToString());
            }
            
        }
        #endregion

        #region 鼠标移到gridUser_MouseMove控件上的任一地方时，显示提示 gridUser_MouseMove事件
        /// <summary>
        /// 鼠标移到gridUser_MouseMove控件上的任一地方时，显示提示 gridUser_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridUser_MouseMove(object sender, MouseEventArgs e)
        {
            m_tip.SetToolTip(this.gridUser, "双击查看详细信息");
            m_tip.Active = true;
        }
        #endregion
    }
}
