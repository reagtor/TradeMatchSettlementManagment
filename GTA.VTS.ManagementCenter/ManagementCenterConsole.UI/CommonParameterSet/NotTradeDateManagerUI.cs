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
using ManagementCenter.Model.CommonClass;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.CommonParameterSet
{
    /// <summary>
    /// 描述：交易所类型_非交易日期管理窗体  错误编码范围:4420-4439
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// </summary>
    public partial class NotTradeDateManagerUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public NotTradeDateManagerUI()
        {
            InitializeComponent();
        }
        #endregion

        #region 变量

        #region  当前页

        /// <summary>
        /// 当前页
        /// </summary>
        public int m_pageNo = int.MaxValue;

        #endregion

        #region 一页展示记录数

        /// <summary>
        /// 一页展示记录数
        /// </summary>
        public int m_pageSize = ParameterSetting.PageSize;

        #endregion

        #region  总记录数

        /// <summary>
        /// 总记录数
        /// </summary>
        public int m_rowCount = int.MaxValue;

        #endregion

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

        /// <summary>
        /// 非交易日期ID
        /// </summary>
        private int m_NotTradeDateID = AppGlobalVariable.INIT_INT;


        #region 操作类型　 1:添加,2:修改
        /// <summary>
        /// 操作类型　 1:添加,2:修改
        /// </summary>
        private int m_EditType = (int)UITypes.EditTypeEnum.AddUI;
        #endregion

        #endregion
        //================================  私有  方法 ================================
        #region 显示数据页数

        /// <summary>
        /// 显示数据页数
        /// </summary>
        private void ShowDataPage()
        {
            if (m_rowCount != AppGlobalVariable.INIT_INT)
            {
                if (m_rowCount == 0)
                {
                    this.UCPageNavig.PageCount = 0;
                }
                else
                {
                    if (m_rowCount % this.m_pageSize == 0)
                    {
                        this.UCPageNavig.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize);
                    }
                    else
                    {
                        this.UCPageNavig.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize) + 1;
                    }
                }

                this.UCPageNavig.CurrentPage = this.m_pageNo;
            }
        }

        #endregion

        #region 获取交易所类型名称 GetBindBourseTypeName

        /// <summary>
        /// 获取交易所类型名称
        /// </summary>
        private void GetBindBourseTypeName()
        {
            DataSet ds = CommonParameterSetCommon.GetBourseTypeName(); //从交易所类型中获取
            UComboItem _item;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                _item = new UComboItem(ds.Tables[0].Rows[i]["BourseTypeName"].ToString(),
                                       Convert.ToInt32(ds.Tables[0].Rows[i]["BourseTypeID"]));
                this.cmbBourseTypeID.Properties.Items.Add(_item);
            }
        }

        #endregion

        #region 根据查询条件，获取交易所类型_非交易日期

        /// <summary>
        /// 根据查询条件，获取交易所类型_非交易日期
        /// </summary>
        /// <returns></returns>
        private bool QueryCMNotTradeDate()
        {
            try
            {
                string bourseTypeName = this.txtQueryBourseName.Text;
                DataSet _dsCMNotTradeDate = CommonParameterSetCommon.GetAllCMNotTradeDate(bourseTypeName,
                                                                                     m_pageNo,
                                                                                     m_pageSize,
                                                                                     out m_rowCount);
                DataTable _dtCMNotTradeDate;
                if (_dsCMNotTradeDate == null || _dsCMNotTradeDate.Tables[0].Rows.Count == 0)
                {
                    _dtCMNotTradeDate = new DataTable();
                }
                else
                {
                    _dtCMNotTradeDate = _dsCMNotTradeDate.Tables[0];
                }

                //绑定交易所类型_非交易日期中的交易所类型ID对应的交易所名称
                this.ddlBourseTypeID.DataSource = CommonParameterSetCommon.GetCMNotTradeDateBourseTypeName().Tables[0];
                this.ddlBourseTypeID.ValueMember =
                    CommonParameterSetCommon.GetCMNotTradeDateBourseTypeName().Tables[0].Columns["BourseTypeID"].
                        ToString();
                this.ddlBourseTypeID.DisplayMember =
                    CommonParameterSetCommon.GetCMNotTradeDateBourseTypeName().Tables[0].Columns["BourseTypeName"].
                        ToString();

                this.gdNotTradeDateResult.DataSource = _dtCMNotTradeDate;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4425";
                string errMsg = "根据查询条件，获取交易所类型_非交易日期失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            //组合框的值全清空?
            //this.cmbBourseTypeID.Text = string.Empty;
            //this.dtNotTradeDay.Text = string.Empty;

            this.cmbBourseTypeID.Enabled = false;
            this.dtNotTradeDay.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnAdd.Text = "添加";
            this.btnAdd.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
        }

        #endregion

        #region 修改交易所类型_非交易日期

        /// <summary>
        /// 修改交易所类型_非交易日期
        /// </summary>
        private void ModifyCMNotTradeDate()
        {
            try
            {
                btnModify.Enabled = true;
                if (this.gdvNotTradeDateSelect != null && this.gdvNotTradeDateSelect.DataSource != null &&
                    this.gdvNotTradeDateSelect.RowCount > 0 && this.gdvNotTradeDateSelect.FocusedRowHandle >= 0)
                {
                    btnModify.Enabled = true;
                    DataRow _dr = this.gdvNotTradeDateSelect.GetDataRow(this.gdvNotTradeDateSelect.FocusedRowHandle);
                    m_NotTradeDateID = Convert.ToInt32(_dr["NotTradeDateID"]);
                    this.dtNotTradeDay.EditValue = Convert.ToDateTime(_dr["NotTradeDay"]);

                    foreach (object item in this.cmbBourseTypeID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BourseTypeID"]))
                        {
                            this.cmbBourseTypeID.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4424";
                string errMsg = " 获取需要修改的交易所类型_非交易日期失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 交易所类型_非交易日期管理窗体 NotTradeDateManagerUI_Load
        /// <summary>
        /// 交易所类型_非交易日期管理窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotTradeDateManagerUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定交易所类型名称
                this.cmbBourseTypeID.Properties.Items.Clear();
                this.GetBindBourseTypeName(); //从交易所类型表中获取
                this.cmbBourseTypeID.SelectedIndex = 0;
                //初次加载时，更新按钮禁用
                //btnModify.Enabled = false;

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdNotTradeDateResult.DataSource = this.QueryCMNotTradeDate();
                this.ShowDataPage();

                //窗体加载禁用文本框和确定按钮
                this.cmbBourseTypeID.Enabled = false;
                this.dtNotTradeDay.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4420";
                string errMsg = "交易所类型_非交易日期管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 查询交易所类型_非交易日期
        /// <summary>
        /// 查询交易所类型_非交易日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryCMNotTradeDate();
                this.ShowDataPage(); //显示数据分页
                //DataTable _dtCMNotTradeDate = (DataTable)this.gdvNotTradeDateSelect.DataSource;
                DataView _dvCMNotT = (DataView)this.gdvNotTradeDateSelect.DataSource;
                DataTable _dtCMNotTradeDate = _dvCMNotT.Table;
                if (_dtCMNotTradeDate == null || _dtCMNotTradeDate.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4426";
                string errMsg = "查询交易所类型_非交易日期失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }

        }
        #endregion

        #region 分页控件事件 UCPageNavig_PageIndexChanged

        /// <summary>
        /// 分页控件事件 UCPageNavig_PageIndexChanged
        /// </summary>
        /// <param name="page">当前页</param>
        private void UCPageNavig_PageIndexChanged(int page)
        {
            if (m_pageNo != page)
            {
                this.m_pageNo = page;
                this.QueryCMNotTradeDate();
            }
        }
        #endregion

        #region 添加交易所类型_非交易日期
        /// <summary>
        /// 添加交易所类型_非交易日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            m_EditType = (int)UITypes.EditTypeEnum.AddUI;
            string Name = this.btnAdd.Text.ToString().Trim();
            if (Name.Equals("添加"))
            {
                this.btnAdd.Text = "取消";
                this.dtNotTradeDay.Text = string.Empty;
                this.cmbBourseTypeID.Enabled = true;
                this.dtNotTradeDay.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnAdd.Text = "添加";
                this.ClearAll();
            }
        }
        #endregion

        #region 修改交易所类型_非交易日期
        /// <summary>
        /// 修改交易所类型_非交易日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            m_EditType = (int)UITypes.EditTypeEnum.UpdateUI;
            string Name = this.btnModify.Text.ToString().Trim();
            if (Name.Equals("修改"))
            {
                this.btnModify.Text = "取消";
                //交易所是否可以进行修改？
                // this.cmbBourseTypeID.Enabled = true;
                this.dtNotTradeDay.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnModify.Text = "修改";
                this.ClearAll();
            }
        }
        #endregion

        #region 修改交易所类型_非交易日期的GridView双击事件
        /// <summary>
        ///修改交易所类型_非交易日期的GridView双击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdNotTradeDateResult_DoubleClick(object sender, EventArgs e)
        {
            //btnAdd.Enabled = false;
            this.ModifyCMNotTradeDate();
            this.ClearAll();
        }
        #endregion

        #region 删除交易所类型_非交易日期
        /// <summary>
        /// 删除交易所类型_非交易日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdvNotTradeDateSelect.GetDataRow(this.gdvNotTradeDateSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["NotTradeDateID"])))
                {
                    m_NotTradeDateID = Convert.ToInt32(_dr["NotTradeDateID"]);
                }
                else
                {
                    m_NotTradeDateID = AppGlobalVariable.INIT_INT;
                }
                if (m_NotTradeDateID != AppGlobalVariable.INIT_INT)
                {
                    m_Result = CommonParameterSetCommon.DeleteCMNotTradeDate(m_NotTradeDateID);
                }
                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_NotTradeDateID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryCMNotTradeDate();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4422";
                string errMsg = " 删除交易所类型_非交易日期失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return;
            }
        }
        #endregion

        #region 取消按钮事件 btnCancel_Click
        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }
        #endregion

        #region 确定按钮事件
        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //通过判断操作类型来进行操作 1：添加操作 2：修改操作
            if (m_EditType == 1)
            {
                #region 添加操作
                try
                {
                    CM_NotTradeDate cM_NotTradeDate = new CM_NotTradeDate();
                    int BourseTypeID;
                    DateTime NotTradeDay;
                    if (!string.IsNullOrEmpty(this.cmbBourseTypeID.Text))
                    {
                        BourseTypeID = ((UComboItem)this.cmbBourseTypeID.SelectedItem).ValueIndex;
                        cM_NotTradeDate.BourseTypeID = BourseTypeID;
                    }
                    else
                    {
                        //cM_NotTradeDate.BourseTypeID = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("请选择交易所名称!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.dtNotTradeDay.Text))
                    {
                        NotTradeDay = Convert.ToDateTime(this.dtNotTradeDay.Text);
                        cM_NotTradeDate.NotTradeDay = NotTradeDay;
                    }
                    else
                    {
                        // cM_NotTradeDate.NotTradeDay = AppGlobalVariable.INIT_DATETIME;
                        ShowMessageBox.ShowInformation("请选择非交易日!");
                        return;
                    }
                    ManagementCenter.Model.CM_NotTradeDate NotTradeDate = CommonParameterSetCommon.GetNotTradeDate(BourseTypeID, NotTradeDay);
                    if (NotTradeDate == null)
                    {
                        int result = CommonParameterSetCommon.AddCMNotTradeDate(cM_NotTradeDate);
                        if (result != AppGlobalVariable.INIT_INT)
                        {
                            ShowMessageBox.ShowInformation("添加成功!");
                            this.ClearAll();
                            this.QueryCMNotTradeDate();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("添加失败!");
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("该非交易日时间已经存在,请勿添加重复数据!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-4421";
                    string errMsg = " 添加交易所类型_非交易日期失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion 添加操作
            }
            else if (m_EditType == 2)
            {
                #region 修改操作
                try
                {
                    CM_NotTradeDate cM_NotTradeDate = new CM_NotTradeDate();
                    if (m_NotTradeDateID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    cM_NotTradeDate.NotTradeDateID = m_NotTradeDateID;
                    //cM_NotTradeDate.BourseTypeID = ((UComboItem)this.cmbBourseTypeID.SelectedItem).ValueIndex;
                    //cM_NotTradeDate.NotTradeDay = Convert.ToDateTime(this.dtNotTradeDay.Text);
                    int BourseTypeID;
                    DateTime NotTradeDay;
                    if (!string.IsNullOrEmpty(this.cmbBourseTypeID.Text))
                    {
                        BourseTypeID = ((UComboItem)this.cmbBourseTypeID.SelectedItem).ValueIndex;
                        cM_NotTradeDate.BourseTypeID = BourseTypeID;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请选择交易所名称!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.dtNotTradeDay.Text))
                    {
                        NotTradeDay = Convert.ToDateTime(this.dtNotTradeDay.Text);
                        cM_NotTradeDate.NotTradeDay = NotTradeDay;
                    }
                    else
                    {
                        // cM_NotTradeDate.NotTradeDay = AppGlobalVariable.INIT_DATETIME;
                        ShowMessageBox.ShowInformation("请选择非交易日期!");
                        return;
                    }
                    ManagementCenter.Model.CM_NotTradeDate NotTradeDate = CommonParameterSetCommon.GetNotTradeDate(BourseTypeID, NotTradeDay);
                    if (NotTradeDate == null)
                    {
                        m_Result = CommonParameterSetCommon.UpdateCMNotTradeDate(cM_NotTradeDate);
                        if (m_Result)
                        {
                            ShowMessageBox.ShowInformation("修改成功!");
                            this.ClearAll();
                            //btnAdd.Enabled = true;
                            //btnModify.Enabled = false;
                            m_NotTradeDateID = AppGlobalVariable.INIT_INT;
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("修改失败!");
                        }
                        this.QueryCMNotTradeDate();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("该非交易日时间已经存在,无法修改成重复数据!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-4423";
                    string errMsg = "修改交易所类型_非交易日期失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion 修改操作
            }
        }
        #endregion 确定按钮事件

    }
}
