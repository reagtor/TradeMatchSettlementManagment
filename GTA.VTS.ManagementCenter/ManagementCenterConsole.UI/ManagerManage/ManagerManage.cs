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
using ManagementCenter.BLL;
using ManagementCenter.Model;
using ManagementCenter.Model.UserManage;
using ManagementCenterConsole.UI.CommonClass;
using Types = ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI.ManagerManage
{
    /// <summary>
    /// 管理员管理  错误编码范围1000-1020
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-25
    /// </summary>
    public partial class ManagerManage : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ManagerManage()
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
        private ManagementCenter.Model.UserManage.ManagerQueryEntity managerQueryEntity;

        /// <summary>
        /// 用户管理
        /// </summary>
        private ManagementCenter.BLL.UM_UserInfoBLL UserInfoBLL;

        private bool isFirstInit = true;

        /// <summary>
        /// 提示信息变量
        /// </summary>
        private ToolTip m_tip=new ToolTip();

        #endregion

        #region 页面加载

        private void ManagerManage_Load(object sender, EventArgs e)
        {
            try
            {
                UserInfoBLL = new UM_UserInfoBLL();
                SetManagerQueryEntity();
                Init();
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("窗体加载失败!");
                string errCode = "GL-1000";
                string errMsg = "窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化管理员管理页面信息
        /// </summary>
        private void Init()
        {
            try
            {
                this.m_pageNo = 1;
                //加载第一页数据
                LoadManagerList();
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
                    if (m_rowCount%this.m_pageSize == 0)
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount/this.m_pageSize);
                    }
                    else
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount/this.m_pageSize) + 1;
                    }
                }
                this.ucPageNavigation1.CurrentPage = this.m_pageNo;
                this.ucPageNavigation1.PageIndexChanged +=
                    new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(
                        ucPageNavigation1_PageIndexChanged);
            }
            catch (Exception ex)
            {
                string errCode = "GL-1001";
                string errMsg = "初始化管理员管理页面信息失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
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
            LoadManagerList();
        }

        #endregion

        #region 加载管理员列表

        /// <summary>
        /// 加载管理员列表
        /// </summary>
        private void LoadManagerList()
        {
            try
            {
                DataSet ds = UserInfoBLL.GetPagingManager(managerQueryEntity, this.m_pageNo, this.m_pageSize,
                                                          out this.m_rowCount);
                DataTable Managerdt;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    Managerdt = new DataTable();
                    if (!isFirstInit) ShowMessageBox.ShowInformation("不存在记录！");
                }
                else
                {
                    Managerdt = ds.Tables[0];
                }
                this.gridManage.DataSource = Managerdt;
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-1002";
                string errMsg = "加载管理员列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
            isFirstInit = false;
        }

        #endregion

        #region 设置查询实体对象

        /// <summary>
        /// 设置查询实体对象
        /// </summary>
        private bool SetManagerQueryEntity()
        {
            if (managerQueryEntity == null)
            {
                managerQueryEntity = new ManagerQueryEntity();
            }
            if (this.txt_UserID.Text.Trim() == string.Empty)
            {
                managerQueryEntity.UserID = int.MaxValue;
            }
            else
            {
                if (InputTest.intTest(this.txt_UserID.Text.Trim()))
                {
                    managerQueryEntity.UserID = int.Parse(this.txt_UserID.Text.Trim());
                }
                else
                {
                    ShowMessageBox.ShowInformation("请输入正确的交易员编号！");
                    return false;
                }
               
            }
            managerQueryEntity.LoginName = this.txt_LoginName.Text.Trim() != string.Empty
                                               ? this.txt_LoginName.Text.Trim()
                                               : string.Empty;

            managerQueryEntity.ManagerGroupName = this.txt_rightgroupname.Text.Trim() != string.Empty
                                                      ? this.txt_rightgroupname.Text.Trim()
                                                      : string.Empty;

            managerQueryEntity.ManagerGroupID = int.MaxValue;
            managerQueryEntity.UserName = string.Empty;
            managerQueryEntity.RoleID = (int) Types.RoleTypeEnum.Manager;
            return true;
        }

        #endregion

        #region 查询按纽事件

        private void Btn_Query_Click(object sender, EventArgs e)
        {
            try
            {
                if (SetManagerQueryEntity())
                { Init(); } 
            }
            catch (Exception ex)
            {
                string errCode = "GL-1003";
                string errMsg = "查询失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 添加按纽事件

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            ManagerEdit managerEdit = new ManagerEdit();
            managerEdit.EditType = 1;
            if (managerEdit.ShowDialog(this) == DialogResult.OK)
            {
                LoadManagerList();
            }
        }

        #endregion

        #region 修改按纽事件

        private void Btn_Update_Click(object sender, EventArgs e)
        {
            if (this.ViewManage != null && this.ViewManage.FocusedRowHandle >= 0)
            {
                m_cutRow = this.ViewManage.FocusedRowHandle;
                ManagerUpdate(m_cutRow);
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行!");
            }
        }

        #endregion

        #region 删除按纽事件

        private void Btn_Del_Click(object sender, EventArgs e)
        {
            if (this.ViewManage != null && this.ViewManage.FocusedRowHandle >= 0)
            {
                if(ShowMessageBox.ShowQuestion("确认删除此用户？")==DialogResult.No ) return;
                try
                {
                    m_cutRow = this.ViewManage.FocusedRowHandle;
                    DataRow dw = ViewManage.GetDataRow(m_cutRow);
                    int UserID = int.Parse(dw["UserID"].ToString());
                    if(UserInfoBLL.ManagerDelete(UserID))
                    {
                        ShowMessageBox.ShowInformation("删除成功！");
                        LoadManagerList();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("删除失败！");
                    }

                }
                catch (Exception ex)
                {
                    ShowMessageBox.ShowInformation("删除失败！");
                    string errCode = "GL-1004";
                    string errMsg = "删除失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                }
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行");
            }
        }

        #endregion

        #region 表格双击事件

        /// <summary>
        /// 表格双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridManage_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.ViewManage.CalcHitInfo(((Control) sender).PointToClient(Control.MousePosition));

            if (this.ViewManage != null && this.ViewManage.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewManage.FocusedRowHandle;
                ManagerUpdate(m_cutRow);
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行");
            }
        }

        #endregion

        #region 修改管理员

        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="handle">当前行</param>
        private void ManagerUpdate(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                ManagerEdit managerEdit = new ManagerEdit();
                managerEdit.EditType = 2;
                DataRow dw = ViewManage.GetDataRow(handle);
                int UserID = int.Parse(dw["UserID"].ToString());

                UM_UserInfo UserInfo = UserInfoBLL.GetModel(UserID);
                managerEdit.UserInfo = UserInfo;

                UM_ManagerBeloneToGroupBLL ManagerBeloneToGroupBLL =new UM_ManagerBeloneToGroupBLL();
                UM_ManagerBeloneToGroup ManagerBeloneToGroup=ManagerBeloneToGroupBLL.GetModel(UserID);
                managerEdit.RightGroupID = (int) ManagerBeloneToGroup.ManagerGroupID;
                if (managerEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadManagerList();
                    this.ViewManage.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("修改管理员失败！");
                string errCode = "GL-1005";
                string errMsg = "修改管理员异常!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region ViewManage_CustomDrawCell的样式设置
        /// <summary>
        /// ViewManage_CustomDrawCell的样式设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewManage_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gridCol_CertificateStyle)
            {
                if ((object)e.CellValue == (object)System.DBNull.Value)
                {
                    e.DisplayText =string.Empty;
                }
                else
                {
                    if ((int)e.CellValue == (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.Passport)
                    {
                        e.DisplayText = "护照";
                    }
                    else if ((int)e.CellValue == (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.ServicemanCard)
                    {
                        e.DisplayText = "军官证";
                    }
                    else if ((int)e.CellValue == (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.StatusCard)
                    {
                        e.DisplayText = "身份证";
                    }
                    else if ((int)e.CellValue == (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.StudentCard)
                    {
                        e.DisplayText = "学生证";
                    }
                }
                
            }
        }
        #endregion

        #region 鼠标移到gridManage_MouseMove控件上的任一地方时，显示提示gridManage_MouseMove事件
        /// <summary>
        /// 鼠标移到gridManage_MouseMove控件上的任一地方时，显示提示gridManage_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridManage_MouseMove(object sender, MouseEventArgs e)
        {
            this.m_tip.SetToolTip(this.gridManage,"双击查看详细信息");
            this.m_tip.Active = true;
        }
        #endregion
    }
}