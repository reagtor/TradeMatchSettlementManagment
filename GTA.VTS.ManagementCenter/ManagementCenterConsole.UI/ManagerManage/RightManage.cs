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
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.ManagerManage
{
    /// <summary>
    /// 权限管理 错误编码范围1200-1220
    /// 作者：程序员：熊晓凌  刘书伟
    /// 日期：2008-11-29  修改：2009-07-03
    /// </summary>
    public partial class RightManage : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RightManage()
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
        /// 查询条件
        /// </summary>
        private string strwhere = string.Empty;

        private bool isFirstInit = true;

        /// <summary>
        /// 权限组管理
        /// </summary>
        private ManagementCenter.BLL.UM_ManagerGroupBLL ManagerGroupBLL;

        /// <summary>
        /// 提示信息的变量
        /// </summary>
        private ToolTip m_tip = new ToolTip();

        #endregion 查询事件

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Query_Click(object sender, EventArgs e)
        {
            if (this.txt_RightName.Text != string.Empty)
            {
                strwhere = " ManagerGroupName LIKE  '%" + this.txt_RightName.Text + "%'";
            }
            else
            {
                strwhere = string.Empty;
            }
            InitRightGroupList();
        }

        #region

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightManage_Load(object sender, EventArgs e)
        {
            try
            {
                ManagerGroupBLL = new UM_ManagerGroupBLL();

                InitRightGroupList();
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("页面加载失败!");
                string errCode = "GL-1200";
                string errMsg = "页面加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化权限组页面信息
        /// </summary>
        private void InitRightGroupList()
        {
            try
            {
                this.m_pageNo = 1;
                //加载第一页数据
                LoadRightGroup();
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
                //写日志
                string errCode = "GL-1201";
                string errMsg = "初始化权限组页面信息失败!";
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
            LoadRightGroup();
        }

        #endregion

        #region 加载权限组列表

        /// <summary>
        /// 加载权限组列表
        /// </summary>
        private void LoadRightGroup()
        {
            try
            {
                ManagerGroupBLL = new UM_ManagerGroupBLL();
                DataSet ds = ManagerGroupBLL.GetPagingManagerGroup(strwhere, this.m_pageNo, this.m_pageSize,
                                                                   out this.m_rowCount);
                DataTable RightGroup;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    RightGroup = new DataTable();
                    if (!isFirstInit) ShowMessageBox.ShowInformation("不存在记录！");
                }
                else
                {
                    RightGroup = ds.Tables[0];
                }
                this.gridRightGroup.DataSource = RightGroup;
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-1202";
                string errMsg = "加载权限组列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
            isFirstInit = false;
        }

        #endregion

        #region 添加事件

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            RightEdit rightEdit = new RightEdit();
            rightEdit.EditType = 1;
            if (rightEdit.ShowDialog(this) == DialogResult.OK)
            {
                LoadRightGroup();
            }
        }

        #endregion

        #region 表格双击事件

        /// <summary>
        /// 表格双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridRightGroup_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.ViewRightGroup.CalcHitInfo(((Control) sender).PointToClient(Control.MousePosition));

            if (this.ViewRightGroup != null && this.ViewRightGroup.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewRightGroup.FocusedRowHandle;
                RightGroupUpdate(m_cutRow);
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行!");
            }
        }

        #endregion

        #region 修改权限组

        /// <summary>
        /// 修改权限组
        /// </summary>
        /// <param name="handle">当前行</param>
        private void RightGroupUpdate(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                RightEdit rightEdit = new RightEdit();
                rightEdit.EditType = 2;
                DataRow dw = ViewRightGroup.GetDataRow(handle);
                int ManagerGroupID = int.Parse(dw["ManagerGroupID"].ToString());
                UM_ManagerGroup ManagerGroup = ManagerGroupBLL.GetModel(ManagerGroupID);
                rightEdit.ManagerGroup = ManagerGroup;

                if (rightEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadRightGroup();
                    this.ViewRightGroup.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-1203";
                string errMsg = "修改权限组失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 修改按纽事件

        /// <summary>
        /// 修改按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (this.ViewRightGroup != null && this.ViewRightGroup.FocusedRowHandle >= 0)
            {
                m_cutRow = this.ViewRightGroup.FocusedRowHandle;
                RightGroupUpdate(m_cutRow);
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行");
            }
        }

        #endregion

        #region 删除按纽事件

        /// <summary>
        /// 删除按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                if (this.ViewRightGroup != null && this.ViewRightGroup.FocusedRowHandle >= 0)
                {
                    m_cutRow = this.ViewRightGroup.FocusedRowHandle;
                    DataRow dw = ViewRightGroup.GetDataRow(m_cutRow);
                    int ManagerGroupID = int.Parse(dw["ManagerGroupID"].ToString());
                    ManagementCenter.BLL.UM_ManagerBeloneToGroupBLL ManagerBeloneToGroupBLL =
                        new UM_ManagerBeloneToGroupBLL();
                    List<UM_ManagerBeloneToGroup> l =
                        ManagerBeloneToGroupBLL.GetListArray(string.Format("ManagerGroupID={0}", ManagerGroupID));
                    if (l == null) return;
                    if (l.Count > 0)
                    {
                        ShowMessageBox.ShowInformation("该权限组下存在管理员，不允许删除！");
                        return;
                    }
                    if (ManagerGroupBLL.Delete(ManagerGroupID))
                    {
                        ShowMessageBox.ShowInformation("删除成功！");
                        LoadRightGroup();
                        return;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("删除失败！");
                        return;
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选中记录行");
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-1204";
                string errMsg = "删除权限组失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region  鼠标移到gridRightGroup控件上的任一地方时，显示提示 gridRightGroup_MouseMove事件
        /// <summary>
        /// 鼠标移到gridRightGroup控件上的任一地方时，显示提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridRightGroup_MouseMove(object sender, MouseEventArgs e)
        {
            m_tip.SetToolTip(this.gridRightGroup, "双击查看详细信息");
            m_tip.Active = true;
        }
        #endregion

    }
}