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

namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    /// <summary>
    /// 描述：股指期货持仓限制和保证金管理 错误编码范围:6480-6499
    /// 作者：刘书伟
    /// 日期：2008-12-08
    /// 修改：叶振东
    /// 时间：2010-04-07
    /// 描述：修改股指期货持仓限制和保证金管理界面中按钮事件
    /// </summary>
    public partial class SIFPositionAndBailManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public SIFPositionAndBailManageUI()
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
        /// 品种ID
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

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

        #region 获取品种类型是股指期货的品种名称

        /// <summary>
        /// 获取品种类型是股指期货的品种名称
        /// </summary>
        private void GetBindQHSIFPositionBailBCName()
        {
            try
            {
                DataSet ds = CommonParameterSetCommon.GetQHSIFPositionAndBailBreedClassName(); //从交易商品品种表中获取
                if (ds != null)
                {
                    UComboItem _item;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _item = new UComboItem(ds.Tables[0].Rows[i]["BreedClassName"].ToString(),
                                               Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]));
                        this.cmbBreedClassID.Properties.Items.Add(_item);
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6486";
                string errMsg = "获取品种类型是股指期货的品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 根据查询条件，获取所有股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 根据查询条件，获取所有股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <returns></returns>
        private bool QueryQHSIFPositionAndBail()
        {
            try
            {
                string breedClassName = this.txtBreedClassID.Text;
                DataSet _dsQHSIFPositionAndBail = FuturesManageCommon.GetAllQHSIFPositionAndQHSIFBail(breedClassName,
                                                                                                      m_pageNo,
                                                                                                      m_pageSize,
                                                                                                      out m_rowCount);
                DataTable _dtQHSIFPositionAndBail;
                if (_dsQHSIFPositionAndBail == null || _dsQHSIFPositionAndBail.Tables[0].Rows.Count == 0)
                {
                    _dtQHSIFPositionAndBail = new DataTable();
                }
                else
                {
                    _dtQHSIFPositionAndBail = _dsQHSIFPositionAndBail.Tables[0];
                }

                //绑定品种类型是股指期货的品种名称
                this.ddlBreedClassID.DataSource =
                    CommonParameterSetCommon.GetQHSIFPositionAndBailBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    CommonParameterSetCommon.GetQHSIFPositionAndBailBreedClassName().Tables[0].Columns["BREEDCLASSID"].
                        ToString();
                this.ddlBreedClassID.DisplayMember =
                    CommonParameterSetCommon.GetQHSIFPositionAndBailBreedClassName().Tables[0].Columns["BREEDCLASSNAME"]
                        .
                        ToString();


                this.gdSIFPositionAndSIFBailResult.DataSource = _dtQHSIFPositionAndBail;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6485";
                string errMsg = "根据查询条件，获取所有股指期货持仓限制和品种_股指期货_保证金数据失败!";
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
            // this.cmbBreedClassID.Text = string.Empty;
            //this.cmbBreedClassID.Enabled = true;
            //this.txtBailScale.Text = string.Empty;
            //this.txtUnilateralPositions.Text = string.Empty;

            this.cmbBreedClassID.Enabled = false;
            this.txtUnilateralPositions.Enabled = false;
            this.txtBailScale.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnAdd.Text = "添加";
            this.btnAdd.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
        }

        #endregion

        #region 修改股指期货持仓限制和品种_股指期货_保证金

        /// <summary>
        /// 修改股指期货持仓限制和品种_股指期货_保证金
        /// </summary>
        private void ModifyQHSIFPositionAndBail()
        {
            try
            {
                if (this.gdSIFPositionAndSIFBailSelect != null && this.gdSIFPositionAndSIFBailSelect.DataSource != null &&
                    this.gdSIFPositionAndSIFBailSelect.RowCount > 0 &&
                    this.gdSIFPositionAndSIFBailSelect.FocusedRowHandle >= 0)
                {
                    DataRow _dr =
                        this.gdSIFPositionAndSIFBailSelect.GetDataRow(
                            this.gdSIFPositionAndSIFBailSelect.FocusedRowHandle);
                    m_BreedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                    this.txtBailScale.Text = _dr["BailScale"].ToString();
                    this.txtUnilateralPositions.Text = _dr["UnilateralPositions"].ToString();
                    foreach (object item in this.cmbBreedClassID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BreedClassID"]))
                        {
                            this.cmbBreedClassID.SelectedItem = item;
                            break;
                        }
                    }
                    this.cmbBreedClassID.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6484";
                string errMsg = "修改股指期货持仓限制和品种_股指期货_保证金失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 股指期货持仓限制和保证金管理UI SIFPositionAndBailManageUI_Load

        /// <summary>
        /// 股指期货持仓限制和保证金管理UI SIFPositionAndBailManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SIFPositionAndBailManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定品种类型是股指期货的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindQHSIFPositionBailBCName(); //从交易商品品种表中获取
                this.cmbBreedClassID.SelectedIndex = 0;

                //初次加载时，更新按钮禁用
                //btnModify.Enabled = false;

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdSIFPositionAndSIFBailResult.DataSource = this.QueryQHSIFPositionAndBail();
                this.ShowDataPage();

                //窗体加载禁用页面中文本框和确定按钮
                this.cmbBreedClassID.Enabled = false;
                this.txtUnilateralPositions.Enabled = false;
                this.txtBailScale.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6480";
                string errMsg = "股指期货持仓限制和保证金管理UI加载失败!";
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
                this.QueryQHSIFPositionAndBail();
            }
        }

        #endregion

        #region 查询股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 查询股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryQHSIFPositionAndBail();
                this.ShowDataPage(); //显示数据分页
                //DataTable _dtQHSIF = (DataTable) this.gdSIFPositionAndSIFBailSelect.DataSource;
                DataView _dvQHSIF = (DataView)this.gdSIFPositionAndSIFBailSelect.DataSource;
                DataTable _dtQHSIF = _dvQHSIF.Table;
                if (_dtQHSIF == null || _dtQHSIF.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6487";
                string errMsg = "查询股指期货持仓限制和品种_股指期货_保证金数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加股指期货持仓限制和品种_股指期货_保证金

        /// <summary>
        /// 添加股指期货持仓限制和品种_股指期货_保证金
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
                this.cmbBreedClassID.Enabled = true;
                this.txtBailScale.Text = string.Empty;
                this.txtUnilateralPositions.Text = string.Empty;

                this.cmbBreedClassID.Enabled = true;
                this.txtUnilateralPositions.Enabled = true;
                this.txtBailScale.Enabled = true;
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

        #region 修改股指期货持仓限制和品种_股指期货_保证金

        /// <summary>
        /// 修改股指期货持仓限制和品种_股指期货_保证金
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

                this.txtUnilateralPositions.Enabled = true;
                this.txtBailScale.Enabled = true;
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

        #region 更新股指期货持仓限制和保证金的GridView双击事件
        /// <summary>
        /// 更新股指期货持仓限制和保证金的GridView双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSIFPositionAndSIFBailResult_DoubleClick(object sender, EventArgs e)
        {
            //btnAdd.Enabled = false;
            this.ModifyQHSIFPositionAndBail();
            this.ClearAll();
        }
        #endregion

        #region 删除股指期货持仓限制和品种_股指期货_保证金

        /// <summary>
        /// 删除股指期货持仓限制和品种_股指期货_保证金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr =
                    this.gdSIFPositionAndSIFBailSelect.GetDataRow(this.gdSIFPositionAndSIFBailSelect.FocusedRowHandle);
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
                    m_Result = FuturesManageCommon.DeleteQHSIFPositionAndQHSIFBail(m_BreedClassID);
                }
                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_BreedClassID = AppGlobalVariable.INIT_INT;
                    this.ClearAll();
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryQHSIFPositionAndBail();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6483";
                string errMsg = "删除股指期货持仓限制和品种_股指期货_保证金失败!";
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
            ClearAll();
        }
        #endregion

        #region 确定按钮操作
        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_EditType == 1)
            {
                #region 添加操作
                try
                {
                    if (
                        FuturesManageCommon.ExistsSIFBail(
                            ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex)
                        && FuturesManageCommon.ExistsSIFPosition(
                            ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的持仓限制和保证金已存在!");
                        return;
                    }
                    QH_SIFBail qH_SIFBail = new QH_SIFBail();
                    QH_SIFPosition qH_SIFPosition = new QH_SIFPosition();
                    if (!string.IsNullOrEmpty(this.txtUnilateralPositions.Text))
                    {
                        if (InputTest.intTest(this.txtUnilateralPositions.Text))
                        {
                            qH_SIFPosition.UnilateralPositions = Convert.ToInt32(this.txtUnilateralPositions.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入数字且第一位数不能为0!");
                            return;
                        }
                    }
                    else
                    {
                        //qH_SIFPosition.UnilateralPositions = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("请填单边持仓量!");
                        return;

                    }

                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        qH_SIFBail.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        qH_SIFBail.BreedClassID = AppGlobalVariable.INIT_INT;
                    }
                    if (!string.IsNullOrEmpty(this.txtBailScale.Text))
                    {
                        if (InputTest.DecimalTest(this.txtBailScale.Text))
                        {
                            qH_SIFBail.BailScale = Convert.ToDecimal(this.txtBailScale.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请填写保证金!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        qH_SIFPosition.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        qH_SIFPosition.BreedClassID = AppGlobalVariable.INIT_INT;
                    }
                    m_Result = FuturesManageCommon.AddQHSIFPositionAndQHSIFBail(qH_SIFPosition, qH_SIFBail);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QueryQHSIFPositionAndBail();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-6481";
                    string errMsg = "添加股指期货持仓限制和品种_股指期货_保证金失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion
            }
            else if (m_EditType == 2)
            {
                #region 修改操作
                try
                {
                    this.cmbBreedClassID.Enabled = false;
                    QH_SIFBail qH_SIFBail = new QH_SIFBail();
                    QH_SIFPosition qH_SIFPosition = new QH_SIFPosition();
                    if (m_BreedClassID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    //qH_FutureCosts.BreedClassID = m_BreedClassID;
                    qH_SIFBail.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    qH_SIFPosition.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    if (!string.IsNullOrEmpty(this.txtUnilateralPositions.Text))
                    {
                        if (InputTest.intTest(this.txtUnilateralPositions.Text))
                        {
                            qH_SIFPosition.UnilateralPositions = Convert.ToInt32(this.txtUnilateralPositions.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入数字且第一位数不能为0!");
                            return;
                        }
                    }
                    else
                    {
                        // qH_SIFPosition.UnilateralPositions = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("请填单边持仓量!");
                        return;

                    }
                    if (!string.IsNullOrEmpty(this.txtBailScale.Text))
                    {
                        if (InputTest.DecimalTest(this.txtBailScale.Text))
                        {
                            qH_SIFBail.BailScale = Convert.ToDecimal(this.txtBailScale.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //qH_SIFBail.BailScale = AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("请填写保证金!");
                        return;
                    }
                    m_Result = FuturesManageCommon.UpdateQHSIFPositionAndQHSIFBail(qH_SIFPosition, qH_SIFBail);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                    this.QueryQHSIFPositionAndBail();
                }
                catch (Exception ex)
                {
                    string errCode = "GL-6482";
                    string errMsg = "修改股指期货持仓限制和品种_股指期货_保证金失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion
            }
        }
        #endregion
    }
}