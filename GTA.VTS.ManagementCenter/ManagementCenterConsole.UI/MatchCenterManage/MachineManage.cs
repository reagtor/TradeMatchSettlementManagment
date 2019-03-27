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

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 撮合机管理 异常编码2016-2030
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class MachineManage : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MachineManage()
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
        private ManagementCenter.Model.RC_MatchMachine matchMachineQueryEntity;

        /// <summary>
        /// 用户管理
        /// </summary>
        private ManagementCenter.BLL.RC_MatchMachineBLL MatchMachineBLL;

        private RC_MatchMachine MatchMachine;
        private bool isFirstInit = true;

        #endregion

        #region 页面加载

        /// <summary>
        ///  页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MachineManage_Load(object sender, EventArgs e)
        {
            try
            {
                this.txt_MachineID.Enabled = false;//撮合机编号为不可编辑状态
                MatchMachineBLL = new RC_MatchMachineBLL();
                SetddlData();
                SetMachineQueryEntity();
                Init();
                SetMatchMachine(null);
                SetControlEnAbled();
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("撮合机管理窗体加载失败!");
                string errCode = "GL-2016";
                string errMsg = "撮合机管理窗体加载失败。";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化撮合机管理页面信息
        /// </summary>
        private void Init()
        {
            try
            {
                this.m_pageNo = 1;
                //加载第一页数据
                LoadMachineList();
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
                //写日志
                string errCode = "GL-2017";
                string errMsg = "初始化撮合机管理页面信息失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
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
            LoadMachineList();
        }

        #endregion

        #region 加载权限组列表

        /// <summary>
        /// 加载权限组列表
        /// </summary>
        private void LoadMachineList()
        {
            try
            {
                DataSet ds = MatchMachineBLL.GetPagingMachine(matchMachineQueryEntity, this.m_pageNo, this.m_pageSize,
                                                              out this.m_rowCount);
                DataTable Machinedt;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    Machinedt = new DataTable();
                    if (!isFirstInit) ShowMessageBox.ShowInformation("不存在记录!");
                }
                else
                {
                    Machinedt = ds.Tables[0];
                }

                //绑定撮合中心_撮合机表中的交易所类型ID对应的交易所类型名称
                ddlBourseTypeID.DataSource = MatchMachineBLL.GetRCMatchMachineBourseTypeName().Tables[0];
                ddlBourseTypeID.ValueMember =
                    MatchMachineBLL.GetRCMatchMachineBourseTypeName().Tables[0].Columns["BourseTypeID"].
                        ToString();
                ddlBourseTypeID.DisplayMember =
                    MatchMachineBLL.GetRCMatchMachineBourseTypeName().Tables[0].Columns["BourseTypeName"].
                        ToString();


                //绑定撮合中心_撮合机表中的撮合中心ID对应的撮合中心名称
                ddlMatchCenterID.DataSource = MatchMachineBLL.GetRCMatchMachineMatchCenterName().Tables[0];
                ddlMatchCenterID.ValueMember =
                    MatchMachineBLL.GetRCMatchMachineMatchCenterName().Tables[0].Columns["MatchCenterID"].
                        ToString();
                ddlMatchCenterID.DisplayMember =
                    MatchMachineBLL.GetRCMatchMachineMatchCenterName().Tables[0].Columns["MatchCenterName"].
                        ToString();

                this.gridMachine.DataSource = Machinedt;
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-2018";
                string errMsg = "加载撮合机列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw;
            }
            isFirstInit = false;
        }

        #endregion

        #region 设置查询实体对象

        /// <summary>
        /// 设置查询实体对象
        /// </summary>
        private void SetMachineQueryEntity()
        {
            try
            {
                if (matchMachineQueryEntity == null)
                {
                    matchMachineQueryEntity = new RC_MatchMachine();
                }
                if (!string.IsNullOrEmpty(ddl_Bourse.Text))
                {
                    matchMachineQueryEntity.BourseTypeID = ((UComboItem)this.ddl_Bourse.SelectedItem).ValueIndex;
                }
                else
                {
                    matchMachineQueryEntity.BourseTypeID = int.MaxValue;
                }
                matchMachineQueryEntity.MatchCenterID = int.MaxValue;
                matchMachineQueryEntity.MatchMachineID = int.MaxValue;

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }

        #endregion

        #region 设置下拉框的值
        /// <summary>
        /// 设置下拉框的值
        /// </summary>
        private void SetddlData()
        {
            try
            {
                UComboItem item;
                item = new UComboItem(string.Empty, int.MaxValue);
                ddl_Bourse.Properties.Items.Add(item);
                ddl_Bourse.Properties.Items.Clear();
                ddl_Bourse.Properties.Items.AddRange(CommonClass.ComboBoxDataSource.GetBourseTypeList());

                ddl_BourseType.Properties.Items.AddRange(CommonClass.ComboBoxDataSource.GetBourseTypeList());
                //ddl_Bourse.SelectedIndex = 0;
                ddl_Center.Properties.Items.AddRange(CommonClass.ComboBoxDataSource.GetMatchCenterList());
            }
            catch (Exception ex)
            {
                string errCode = "GL-2019";
                string errMsg = "设置下拉框的值失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw;
            }
        }

        #endregion

        #region 查询事件

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, EventArgs e)
        {
            SetMachineQueryEntity();
            Init();
        }

        #endregion

        #region 表格单击事件

        /// <summary>
        /// 表格单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCenter_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.ViewMachine.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

            if (this.ViewMachine != null && this.ViewMachine.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewMachine.FocusedRowHandle;
                DataRow dw = ViewMachine.GetDataRow(m_cutRow);
                int MatchMachineID = int.Parse(dw["MatchMachineID"].ToString());
                MatchMachine = MatchMachineBLL.GetModel(MatchMachineID);
                SetMatchMachine(MatchMachine);
            }
            //else
            //{
            //    ShowMessageBox.ShowInformation("请选中记录行");
            //}
        }

        #endregion

        /// <summary>
        /// 编辑类型 0 查看 1 添加 2修改
        /// </summary>
        private static int edit;

        #region 添加按纽事件

        /// <summary>
        /// 添加按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (this.btn_Add.Text == "添加")
            {
                this.btn_Add.Text = "取消";
                edit = 1;
                SetControlEnAbled();
            }
            else if (this.btn_Add.Text == "取消")
            {
                this.btn_Add.Text = "添加";
                edit = 0;
                SetMatchMachine(null);
                SetControlEnAbled();
            }
        }

        #endregion

        #region 修改按纽事件

        /// <summary>
        /// 修改按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, EventArgs e)
        {
            if (this.btn_Modify.Text == "修改")
            {
                this.btn_Modify.Text = "取消";
                edit = 2;
                SetControlEnAbled();
            }
            else if (this.btn_Modify.Text == "取消")
            {
                this.btn_Modify.Text = "修改";
                edit = 0;
                SetMatchMachine(null);
                SetControlEnAbled();
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
                if (this.ViewMachine != null && this.ViewMachine.FocusedRowHandle >= 0)
                {
                    m_cutRow = this.ViewMachine.FocusedRowHandle;
                    //MachineUpdate(m_cutRow);
                    DataRow dw = ViewMachine.GetDataRow(m_cutRow);
                    int MatchMachineID = int.Parse(dw["MatchMachineID"].ToString());

                    if (MatchMachineBLL.Delete(MatchMachineID))
                    {
                        ShowMessageBox.ShowInformation("删除成功!");
                        Init();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("删除失败!");
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选中记录行!");
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("删除失败!");
                string errCode = "GL-2020";
                string errMsg = "删除失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }

        #endregion

        #region 确定按纽事件

        /// <summary>
        /// 确定按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                RC_MatchMachine rcMatchMachine = GetMatchMachine();
                if (rcMatchMachine != null)
                {
                    if (edit == 1)
                    {
                        int i = MatchMachineBLL.Add(GetMatchMachine());
                        if (i > 0)
                        {
                            ShowMessageBox.ShowInformation("添加成功!");
                            this.btn_Add.Text = "添加";
                            this.btn_Modify.Text = "修改";
                            edit = 0;
                            SetMatchMachine(null);
                            SetControlEnAbled();
                            Init();
                            return;
                        }
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                    else if (edit == 2)
                    {

                        MatchMachineBLL.Update(GetMatchMachine());
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.btn_Add.Text = "添加";
                        this.btn_Modify.Text = "修改";
                        edit = 0;
                        SetMatchMachine(null);
                        SetControlEnAbled();
                        Init();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("保存失败!");
                string errCode = "GL-2021";
                string errMsg = "保存失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }

        #endregion

        #region 得到凑合机实体

        /// <summary>
        /// 得到凑合机实体
        /// </summary>
        /// <returns></returns>
        public RC_MatchMachine GetMatchMachine()
        {
            if (MatchMachine == null) MatchMachine = new RC_MatchMachine();
            MatchMachine.BourseTypeID = ((UComboItem)ddl_BourseType.SelectedItem).ValueIndex;
            MatchMachine.MatchCenterID = ((UComboItem)ddl_Center.SelectedItem).ValueIndex;
            MatchMachine.MatchMachineID = Convert.ToInt32(this.txt_MachineID.Text);
            if (!string.IsNullOrEmpty(this.txt_MachineName.Text))
            {
                MatchMachine.MatchMachineName = this.txt_MachineName.Text;
            }
            else
            {
                ShowMessageBox.ShowInformation("请填写撮合机名称!");
                return null;

            }
            return MatchMachine;
        }

        #endregion

        #region 设置对象值

        /// <summary>
        /// 设置对象值
        /// </summary>
        /// <param name="matchMachine"></param>
        public void SetMatchMachine(RC_MatchMachine matchMachine)
        {
            try
            {
                if (matchMachine == null)
                {
                    m_cutRow = this.ViewMachine.FocusedRowHandle;
                    if (m_cutRow < 0) return;
                    DataRow dw = ViewMachine.GetDataRow(m_cutRow);
                    int MatchMachineID = int.Parse(dw["MatchMachineID"].ToString());
                    matchMachine = MatchMachineBLL.GetModel(MatchMachineID);
                    if (matchMachine == null) return;
                }
                this.txt_MachineID.Text = matchMachine.MatchMachineID.ToString();
                this.txt_MachineName.Text = matchMachine.MatchMachineName;
                foreach (object item in this.ddl_BourseType.Properties.Items)
                {
                    if (((UComboItem)item).ValueIndex == matchMachine.BourseTypeID)
                    {
                        this.ddl_BourseType.SelectedItem = item;
                        break;
                    }
                }
                foreach (object item in this.ddl_Center.Properties.Items)
                {
                    if (((UComboItem)item).ValueIndex == matchMachine.MatchCenterID)
                    {
                        this.ddl_Center.SelectedItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-2022";
                string errMsg = "设置对象值失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw;
            }
        }

        #endregion

        #region 设置控件是否可用

        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        private void SetControlEnAbled()
        {
            if (edit == 0)
            {
                this.btn_OK.Enabled = false;
                this.btn_Add.Enabled = this.btn_Modify.Enabled = this.btn_Del.Enabled = true;
                this.txt_MachineID.Enabled =
                    this.txt_MachineName.Enabled =
                    this.ddl_Center.Enabled = this.ddl_BourseType.Enabled = false;
            }
            else if (edit == 1)
            {
                this.btn_OK.Enabled = this.btn_Add.Enabled = true;
                this.btn_Modify.Enabled = this.btn_Del.Enabled = false;

                this.txt_MachineName.Enabled =
                    this.ddl_Center.Enabled = this.ddl_BourseType.Enabled = true;
                this.txt_MachineID.Text = this.txt_MachineName.Text = string.Empty;
            }
            else if (edit == 2)
            {
                this.btn_OK.Enabled = this.btn_Modify.Enabled = true;
                this.btn_Add.Enabled = this.btn_Del.Enabled = false;

                this.txt_MachineID.Enabled =
                    this.ddl_Center.Enabled = this.ddl_BourseType.Enabled = false;
                this.txt_MachineName.Enabled = true;
            }
        }

        #endregion

        #region 代码分配按纽事件
        private void btn_AssignCode_Click(object sender, EventArgs e)
        {
            if (this.ViewMachine != null && this.ViewMachine.FocusedRowHandle >= 0)
            {
                m_cutRow = this.ViewMachine.FocusedRowHandle;
                DataRow dw = ViewMachine.GetDataRow(m_cutRow);
                int MatchMachineID = int.Parse(dw["MatchMachineID"].ToString());
                MatchMachine = MatchMachineBLL.GetModel(MatchMachineID);
                CodeAssign code = new CodeAssign();
                code.MatchMachine = MatchMachine;
                code.ShowDialog();
            }
            else
            {
                ShowMessageBox.ShowInformation("请选中记录行!");
            }
        }
        #endregion
    }
}