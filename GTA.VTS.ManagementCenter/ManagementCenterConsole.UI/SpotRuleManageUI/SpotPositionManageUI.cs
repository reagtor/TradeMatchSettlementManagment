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

namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    /// <summary>
    /// 描述：现货持仓限制管理窗体  错误编码范围:5600-5619
    /// 作者：刘书伟
    /// 日期：2008-12-08
    /// 修改：叶振东
    /// 日期：2010-04-01
    /// 描述：修改了现货持仓限制管理窗体操作的使用方法
    /// </summary>
    public partial class SpotPositionManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public SpotPositionManageUI()
        {
            InitializeComponent();
            this.cmbBreedClassID.Enabled = false;
            this.txtRate.Enabled = false;
            this.btnOK.Enabled = false;
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
        /// 品种ID
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 操作状态 1：添加  2：修改
        /// </summary>
        private int Status = 0;
        #endregion

        //================================  私有  方法 ================================

        #region 获取现货品种名称 GetBindBreedClassName

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        private void GetBindBreedClassName()
        {
            DataSet ds = CommonParameterSetCommon.GetXHAndHKBreedClassName();//获取现货普通和港股品种名称   //SpotManageCommon.GetBreedClassName(); //从交易商品品种表中获取
            UComboItem _item;
            int _BreedClassID = 46;//未分配品种ID=46，固定值
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                _item = new UComboItem(ds.Tables[0].Rows[i]["BreedClassName"].ToString(),
                                       Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]));
                //this.cmbBreedClassID.Properties.Items.Add(_item);
                if (_BreedClassID != Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]))
                {
                    this.cmbBreedClassID.Properties.Items.Add(_item);
                }
            }
        }

        #endregion

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

        #region 根据查询条件，获取现货_交易商品品种_持仓限制

        /// <summary>
        /// 根据查询条件，获取现货_交易商品品种_持仓限制
        /// </summary>
        /// <returns></returns>
        private bool QuerySpotPosition()
        {
            try
            {
                string breedClassName = this.txtBreedClassName.Text;
                DataSet _dsSpotPosition = SpotManageCommon.GetAllXHSpotPosition(breedClassName,
                                                                                m_pageNo,
                                                                                m_pageSize,
                                                                                out m_rowCount);
                DataTable _dtSpotPosition;
                if (_dsSpotPosition == null || _dsSpotPosition.Tables[0].Rows.Count == 0)
                {
                    _dtSpotPosition = new DataTable();
                }
                else
                {
                    _dtSpotPosition = _dsSpotPosition.Tables[0];
                }

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetSpotPositionBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetSpotPositionBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetSpotPositionBreedClassName().Tables[0].Columns["BreedClassName"].ToString();
                this.gdSpotPositionResult.DataSource = _dtSpotPosition;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5605";
                string errMsg = "根据查询条件，获取现货_交易商品品种_持仓限制失败!";
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
            this.txtRate.Text = string.Empty;
            this.btnAdd.Enabled = true;
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
            this.cmbBreedClassID.Enabled = false;
            this.txtRate.Enabled = false;
        }

        #endregion

        #region 获取需要修改的现货_交易商品品种_持仓限制

        /// <summary>
        /// 获取需要修改的现货_交易商品品种_持仓限制
        /// </summary>
        private void ModifyXHSpotPosition()
        {
            try
            {
                if (this.gdSpotPositionSelect != null && this.gdSpotPositionSelect.DataSource != null &&
                    this.gdSpotPositionSelect.RowCount > 0 && this.gdSpotPositionSelect.FocusedRowHandle >= 0)
                {
                    btnModify.Enabled = true;
                    DataRow _dr = this.gdSpotPositionSelect.GetDataRow(this.gdSpotPositionSelect.FocusedRowHandle);
                    m_BreedClassID = Convert.ToInt32(_dr["BreedClassID"]);

                    foreach (object item in cmbBreedClassID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == int.Parse(_dr["BreedClassID"].ToString()))
                        {
                            cmbBreedClassID.SelectedItem = item;
                            break;
                        }
                    }
                    txtRate.Text = Convert.ToString(_dr["Rate"]);
                }
                this.cmbBreedClassID.Enabled = false;
                this.txtRate.Enabled = false;
                this.btnAdd.Enabled = true;
                this.btnModify.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnAdd.Text = "添加";
                this.btnModify.Text = "修改";
            }
            catch (Exception ex)
            {
                string errCode = "GL-5603";
                string errMsg = "获取需要修改的现货_交易商品品种_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 现货_交易商品品种_持仓限制 管理UI SpotPositionManageUI_Load

        /// <summary>
        /// 现货_交易商品品种_持仓限制 管理UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpotPositionManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定交易规则表中的品种ID对应的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindBreedClassName(); //获取交易规则表中的品种ID对应的品种名称
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetSpotPositionBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetSpotPositionBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetSpotPositionBreedClassName().Tables[0].Columns["BreedClassName"].ToString();

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdSpotPositionResult.DataSource = this.QuerySpotPosition();
                this.ShowDataPage();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5600";
                string errMsg = "现货_交易商品品种_持仓限制窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 查询现货_交易商品品种_持仓限制 btnQuery_Click

        /// <summary>
        ///查询现货_交易商品品种_持仓限制 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QuerySpotPosition();
                this.ShowDataPage(); //显示数据分页
                // DataTable _dtSpotPosition = (DataTable) this.gdSpotPositionSelect.DataSource;
                DataView _dvSpotP = (DataView)this.gdSpotPositionSelect.DataSource;
                DataTable _dtSpotPosition = _dvSpotP.Table;
                if (_dtSpotPosition == null || _dtSpotPosition.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5606";
                string errMsg = "查询现货_交易商品品种_持仓限制失败!";
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
                this.QuerySpotPosition();
            }
        }

        #endregion

        #region 添加现货_交易商品品种_持仓限制 btnAdd_Click

        /// <summary>
        /// 添加现货_交易商品品种_持仓限制 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Status = 1;
            string name = this.btnAdd.Text.ToString();
            if (name.Equals("添加"))
            {
                this.btnAdd.Text = "取消";
                this.btnOK.Enabled = true;
                this.txtRate.Text = string.Empty;
                this.cmbBreedClassID.Enabled = true;
                this.txtRate.Enabled = true;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else
            {
                this.btnOK.Enabled = false;
                this.btnAdd.Text = "添加";
                this.ClearAll();
                Status = 0;
            }
        }

        #endregion

        #region 修改现货_交易商品品种_持仓限制

        /// <summary>
        /// 修改现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            Status = 2;
            string name = this.btnModify.Text.ToString();
            if (name.Equals("修改"))
            {
                this.btnModify.Text = "取消";
                this.btnOK.Enabled = true;
                this.cmbBreedClassID.Enabled = true;
                this.txtRate.Enabled = true;
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else
            {
                Status = 0;
                this.btnOK.Enabled = false;
                this.btnModify.Text = "修改";
                this.ClearAll();
            }
        }

        #endregion

        #region  现货_交易商品品种_持仓限制GridView双击事件 gdSpotPositionResult_DoubleClick

        /// <summary>
        /// 现货_交易商品品种_持仓限制GridView双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSpotPositionResult_DoubleClick(object sender, EventArgs e)
        {
            this.ModifyXHSpotPosition();
        }

        #endregion

        #region 删除现货_交易商品品种_持仓限制 btnDelete_Click

        /// <summary>
        /// 删除现货_交易商品品种_持仓限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdSpotPositionSelect.GetDataRow(this.gdSpotPositionSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["BreedClassID"])))
                {
                    m_BreedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                }
                else
                {
                    m_BreedClassID = AppGlobalVariable.INIT_INT;
                }
                if (m_BreedClassID != AppGlobalVariable.INIT_INT)
                {
                    m_Result = SpotManageCommon.DeleteXHSpotPosition(m_BreedClassID);
                }
                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_BreedClassID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QuerySpotPosition();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5602";
                string errMsg = "删除现货_交易商品品种_持仓限制失败!";
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

        /// <summary>
        /// 点击确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            int status = Status;
            if (status == 1)
            {
                #region 添加操作
                try
                {
                    if (
                        SpotManageCommon.ExistsXHSpotPosition(
                            ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的持仓比率已存在!");
                        return;
                    }
                    XH_SpotPosition xH_SpotPosition = new XH_SpotPosition();

                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        xH_SpotPosition.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        xH_SpotPosition.BreedClassID = AppGlobalVariable.INIT_INT;
                    }

                    if (!string.IsNullOrEmpty(this.txtRate.Text))
                    {
                        if (InputTest.DecimalTest(this.txtRate.Text))
                        {
                            xH_SpotPosition.Rate = Convert.ToDecimal(this.txtRate.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //xH_SpotPosition.Rate = AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("持仓比率不能为空!");
                        return;
                    }
                    m_Result = SpotManageCommon.AddXHSpotPosition(xH_SpotPosition);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QuerySpotPosition();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-5601";
                    string errMsg = "添加现货_交易商品品种_持仓限制失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion 添加操作
                this.btnAdd.Enabled = true;
                this.btnModify.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnAdd.Text = "添加";
                this.btnModify.Text = "修改";
                Status = 0;
            }
            else if (status == 2)
            {
                #region 修改操作
                try
                {
                    XH_SpotPosition xH_SpotPosition = new XH_SpotPosition();
                    if (m_BreedClassID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    xH_SpotPosition.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    // xH_SpotPosition.Rate = Convert.ToDecimal(this.txtRate.Text);
                    if (!string.IsNullOrEmpty(this.txtRate.Text))
                    {
                        if (InputTest.DecimalTest(this.txtRate.Text))
                        {
                            xH_SpotPosition.Rate = Convert.ToDecimal(this.txtRate.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //xH_SpotPosition.Rate = AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("持仓比率不能为空!");
                        return;
                    }
                    m_Result = SpotManageCommon.UpdateXHSpotPosition(xH_SpotPosition);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                    this.QuerySpotPosition();
                }
                catch (Exception ex)
                {
                    string errCode = "GL-5604";
                    string errMsg = "修改现货_交易商品品种_持仓限制失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion  修改操作
                this.btnAdd.Enabled = true;
                this.btnModify.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnAdd.Text = "添加";
                this.btnModify.Text = "修改";
                Status = 0;
            }
            this.btnOK.Enabled = false;
        }

        #endregion
      
    }
}