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
using ManagementCenterConsole.UI.CommonClass;
using ManagementCenter.Model;

namespace ManagementCenterConsole.UI.CounterManage
{
    /// <summary>
    /// 柜台管理页面  编码范围 GL-3000-3030
    /// 作者：熊晓凌
    /// 日期：2008-11-20
    /// </summary>
    public partial class CounterManger : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CounterManger()
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
        private CT_Counter QueryCounter;

        private CT_CounterBLL CounterBLL;
        private bool isFirstInit = true;

        /// <summary>
        /// 提示信息变量
        /// </summary>
        private ToolTip m_tip=new ToolTip();

        #endregion

        #region 加载页面

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CounterManger_Load(object sender, EventArgs e)
        {
            //CT_CounterBLL CounterBLL = new CT_CounterBLL();
            //this.gridCounter.DataSource= CounterBLL.GetAllList().Tables[0];
            try
            {
                CounterBLL = new CT_CounterBLL();
                SetQueryCounter();
                InitCounterList();
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("清算柜台管理页面加载失败!");
                string errCode = "GL-3000";
                string errMsg = "清算柜台管理页面加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化页面信息
        /// </summary>
        private void InitCounterList()
        {
            try
            {
                this.m_pageNo = 1;
                //加载第一页数据
                LoadCounter();
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
                string errCode = "GL-3001";
                string errMsg = "初始化页面信息失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw;
            }
        }

        #endregion

        #region 加载柜台列表

        /// <summary>
        /// 加载柜台列表
        /// </summary>
        private void LoadCounter()
        {
            try
            {
                DataSet ds = CounterBLL.GetPagingCounter(QueryCounter, this.m_pageNo, this.m_pageSize,
                                                         out this.m_rowCount);
                DataTable allCounter;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    allCounter = new DataTable();
                    if (!isFirstInit) ShowMessageBox.ShowInformation("不存在记录!");
                }
                else
                {
                    allCounter = ds.Tables[0];
                }
                this.gridCounter.DataSource = allCounter;
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-3002";
                string errMsg = "加载柜台列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
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
            LoadCounter();
        }

        #endregion

        #region 设置查询实体对象

        /// <summary>
        /// 设置查询实体对象
        /// </summary>
        private bool SetQueryCounter()
        {
            try
            {
                if (QueryCounter == null)
                {
                    QueryCounter = new CT_Counter();
                }
                if (this.txt_Name.Text != string.Empty)
                {
                    QueryCounter.name = this.txt_Name.Text;
                }
                else
                {
                    QueryCounter.name = null;
                }
                if (this.txt_IP.Text == string.Empty)
                {
                    QueryCounter.IP = null;
                }
                else
                {
                    if (CommonClass.InputTest.IPTest(this.txt_IP.Text))
                    {
                        QueryCounter.IP = this.txt_IP.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入正确的IP地址!");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-3003";
                string errMsg = "设置查询实体对象失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
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
            if (SetQueryCounter())
            {
                InitCounterList();
            }
        }

        #endregion

        #region 添加按纽事件

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            CounterEdit counterEdit = new CounterEdit();
            counterEdit.EditType = 1;
            if (counterEdit.ShowDialog(this) == DialogResult.OK)
            {
                InitCounterList();
            }
        }

        #endregion

        #region 更新按纽事件

        private void Btn_Update_Click(object sender, EventArgs e)
        {
            if (this.ViewCounter != null && this.ViewCounter.FocusedRowHandle >= 0)
            {
                m_cutRow = this.ViewCounter.FocusedRowHandle;
                CounterUpdate(m_cutRow);
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
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                if (this.ViewCounter != null && this.ViewCounter.FocusedRowHandle >= 0)
                {
                    m_cutRow = this.ViewCounter.FocusedRowHandle;
                    DataRow dw = ViewCounter.GetDataRow(m_cutRow);
                    int CouterID = int.Parse(dw["CouterID"].ToString());

                    {
                        //检测该柜台下是否存在用户
                        UM_UserInfoBLL UserInfoBLL = new UM_UserInfoBLL();
                        List<UM_UserInfo> l = UserInfoBLL.GetListArray(string.Format("CouterID={0}", CouterID));
                        if (l != null && l.Count > 0)
                        {
                            ShowMessageBox.ShowInformation("该柜台下存在交易员，不允许删除此柜台!");
                            return;
                        }
                    }
                    CounterBLL.Delete(CouterID);
                    ShowMessageBox.ShowInformation("删除成功!");
                    InitCounterList();
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选中记录行!");
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("删除失败!");
                string errCode = "GL-3004";
                string errMsg = "删除失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 修改柜台信息

        /// <summary>
        /// 修改柜台信息
        /// </summary>
        /// <param name="handle">当前行</param>
        private void CounterUpdate(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                CounterEdit counterEdit = new CounterEdit();
                counterEdit.EditType = 2;
                DataRow dw = ViewCounter.GetDataRow(handle);
                int CouterID = int.Parse(dw["CouterID"].ToString());

                CT_Counter Counter = CounterBLL.GetModel(CouterID);
                counterEdit.Counter = Counter;
                if (counterEdit.ShowDialog(this) == DialogResult.OK)
                {
                    InitCounterList();
                    this.ViewCounter.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("修改柜台信息失败！");
                string errCode = "GL-3005";
                string errMsg = "修改柜台信息失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 表格双击事件

        private void gridCounter_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.ViewCounter.CalcHitInfo(((Control) sender).PointToClient(Control.MousePosition));

            if (this.ViewCounter != null && this.ViewCounter.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewCounter.FocusedRowHandle;
                CounterUpdate(m_cutRow);
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行!");
            }
        }

        #endregion

        #region 用户重画单元格

        private void ViewCounter_CustomDrawCell(object sender,
                                                DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gridCol_State)
            {
                if ((object) e.CellValue == (object) System.DBNull.Value)
                {
                    e.DisplayText = string.Empty;
                }
                else
                {
                    if ((int) e.CellValue == (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnSuccess)
                    {
                        e.DisplayText = "连接正常";
                    }
                    else if ((int) e.CellValue == (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat)
                    {
                        e.DisplayText = "连接失败";
                    }
                }
            }
        }

        #endregion

        #region 刷新状态

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                CounterBLL.CenterTestConnection();
                InitCounterList();
            }
            catch (Exception ex)
            {
                string errCode = "GL-3006";
                string errMsg = "刷新柜台连接状态失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 鼠标移到gridCounter_MouseMove控件上的任一地方时，显示提示gridCounter_MouseMove事件
        /// <summary>
        /// 鼠标移到gridCounter_MouseMove控件上的任一地方时，显示提示gridCounter_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCounter_MouseMove(object sender, MouseEventArgs e)
        {
            this.m_tip.SetToolTip(this.gridCounter,"双击查看详细信息");
            this.m_tip.Active = true;
        }
        #endregion
    }
}