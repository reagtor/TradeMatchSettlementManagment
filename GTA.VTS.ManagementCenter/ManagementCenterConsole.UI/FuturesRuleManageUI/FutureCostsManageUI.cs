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
    /// 描述：期货交易费用管理窗体  错误编码范围:6200-6219
    /// 作者：刘书伟
    /// 日期：2008-12-06
    /// 修改：叶振东
    /// 时间：2010-04-07
    /// 描述：修改期货交易费用管理窗体中按钮事件、修改双击列表中数据数据未显示
    /// </summary>
    public partial class FutureCostsManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public FutureCostsManageUI()
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

        #region 获取品种类型是商品期货或股指期货的品种名称

        /// <summary>
        /// 获取品种类型是商品期货或股指期货的品种名称
        /// </summary>
        private void GetBindQHFutureCostsBreedClassName()
        {
            try
            {
                DataSet ds = CommonParameterSetCommon.GetQHFutureCostsBreedClassName(); //从交易商品品种表中获取
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
                LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }

        #endregion

        #region 根据查询条件，获取品种_期货_交易费用

        /// <summary>
        /// 根据查询条件，获取品种_期货_交易费用
        /// </summary>
        /// <returns></returns>
        private bool QueryQHFutureCosts()
        {
            try
            {
                string breedClassName = this.txtBreedClassID.Text;
                DataSet _dsQHFutureCosts = FuturesManageCommon.GetAllQHFutureCosts(breedClassName,
                                                                                   m_pageNo,
                                                                                   m_pageSize,
                                                                                   out m_rowCount);
                DataTable _dtQHFutureCosts;
                if (_dsQHFutureCosts == null || _dsQHFutureCosts.Tables[0].Rows.Count == 0)
                {
                    _dtQHFutureCosts = new DataTable();
                }
                else
                {
                    _dtQHFutureCosts = _dsQHFutureCosts.Tables[0];
                }

                //绑定绑定品种类型是商品期货或股指期货的品种名称
                this.ddlBreedClassID.DataSource = CommonParameterSetCommon.GetQHFutureCostsBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    CommonParameterSetCommon.GetQHFutureCostsBreedClassName().Tables[0].Columns["BreedClassID"].
                        ToString();
                this.ddlBreedClassID.DisplayMember =
                    CommonParameterSetCommon.GetQHFutureCostsBreedClassName().Tables[0].Columns["BreedClassName"].
                        ToString();

                //绑定货币类型
                this.ddlCurrencyTypeID.DataSource = BindData.GetBindListCurrencyType();
                this.ddlCurrencyTypeID.ValueMember = "ValueIndex";
                this.ddlCurrencyTypeID.DisplayMember = "TextTitleValue";

                //绑定手续费类型
                this.ddlCostType.DataSource = BindData.GetBindListFutrueCostType();
                this.ddlCostType.ValueMember = "ValueIndex";
                this.ddlCostType.DisplayMember = "TextTitleValue";

                this.gdFutureCostsResult.DataSource = _dtQHFutureCosts;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6204";
                string errMsg = "根据查询条件，获取品种_期货_交易费用失败!";
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
            //this.cmbBreedClassID.Text = string.Empty;
            //this.cmbCurrencyTypeID.Text = string.Empty;
            //this.txtTradeUnitCharge.Text = string.Empty;
            //this.txtTurnoverRateOfSerCha.Text = string.Empty;
            //this.cmbBreedClassID.Enabled = true;
            //this.txtCost.Text = string.Empty;

            this.cmbBreedClassID.Enabled = false;
            this.cmbCurrencyTypeID.Enabled = false;
            this.cmbCostType.Enabled = false;
            this.txtCost.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnAdd.Text = "添加";
            this.btnAdd.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
        }

        #endregion

        #region 修改品种_期货_交易费用

        /// <summary>
        /// 修改品种_期货_交易费用
        /// </summary>
        private void ModifyQHFutureCosts()
        {
            try
            {
                if (this.gdvFutureCostsSelect != null && this.gdvFutureCostsSelect.DataSource != null &&
                    this.gdvFutureCostsSelect.RowCount > 0 && this.gdvFutureCostsSelect.FocusedRowHandle >= 0)
                {
                    DataRow _dr = this.gdvFutureCostsSelect.GetDataRow(this.gdvFutureCostsSelect.FocusedRowHandle);
                    m_BreedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                    //if (!string.IsNullOrEmpty(_dr["TradeUnitCharge"].ToString()))
                    //{
                    //    this.txtCost.Text = _dr["TradeUnitCharge"].ToString();
                    //}
                    if (!string.IsNullOrEmpty(_dr["TurnoverRateOfServiceCharge"].ToString()))
                    {
                        this.txtCost.Text = _dr["TurnoverRateOfServiceCharge"].ToString();
                    }
                    foreach (object item in this.cmbBreedClassID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BreedClassID"]))
                        {
                            this.cmbBreedClassID.SelectedItem = item;
                            break;
                        }
                    }
                    foreach (object item in this.cmbCurrencyTypeID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["CurrencyTypeID"]))
                        {
                            this.cmbCurrencyTypeID.SelectedItem = item;
                            break;
                        }
                    }
                    foreach (object item in this.cmbCostType.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["CostType"]))
                        {
                            this.cmbCostType.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6205";
                string errMsg = "修改品种_期货_交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region  期货交易费用管理窗体 FutureCostsManageUI_Load

        /// <summary>
        /// 期货交易费用管理窗体 FutureCostsManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FutureCostsManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定品种类型是商品期货或股指期货的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindQHFutureCostsBreedClassName(); //从交易商品品种表中获取
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定货币类型
                this.cmbCurrencyTypeID.Properties.Items.Clear();
                this.cmbCurrencyTypeID.Properties.Items.AddRange(BindData.GetBindListCurrencyType());
                this.cmbCurrencyTypeID.SelectedIndex = 0;

                //初次加载时，更新按钮禁用
                //btnModify.Enabled = false;
                //绑定期货手续费类型
                this.cmbCostType.Properties.Items.Clear();
                this.cmbCostType.Properties.Items.AddRange(BindData.GetBindListFutrueCostType());
                this.cmbCostType.SelectedIndex = 0;
                //绑定查询结果
                this.m_pageNo = 1;
                this.gdFutureCostsResult.DataSource = this.QueryQHFutureCosts();
                this.ShowDataPage();

                //窗体加载时禁用文本框和确定按钮
                this.cmbBreedClassID.Enabled = false;
                this.cmbCurrencyTypeID.Enabled = false;
                this.cmbCostType.Enabled = false;
                this.txtCost.Enabled = false;
                this.btnOK.Enabled = false;

            }
            catch (Exception ex)
            {
                string errCode = "GL-6200";
                string errMsg = "期货交易费用管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 查询品种_期货_交易费用

        /// <summary>
        /// 查询品种_期货_交易费用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryQHFutureCosts();
                this.ShowDataPage(); //显示数据分页
                DataView _dvQHFutureC = (DataView)this.gdvFutureCostsSelect.DataSource;
                DataTable _dtQHFutureCosts = _dvQHFutureC.Table;
                if (_dtQHFutureCosts == null || _dtQHFutureCosts.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6206";
                string errMsg = "查询品种_期货_交易费用失败!";
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
                this.QueryQHFutureCosts();
            }
        }

        #endregion

        #region 添加品种_期货_交易费用

        /// <summary>
        /// 添加品种_期货_交易费用
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
                this.txtCost.Text = string.Empty;
                this.cmbBreedClassID.Enabled = true;
                this.cmbCurrencyTypeID.Enabled = true;
                this.cmbCostType.Enabled = true;
                this.txtCost.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;

            }
            else
            {
                this.ClearAll();
            }
        }

        #endregion

        #region 更新品种_期货_交易费用

        /// <summary>
        /// 更新品种_期货_交易费用
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
                this.cmbCurrencyTypeID.Enabled = true;
                this.cmbCostType.Enabled = true;
                this.txtCost.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;

            }
            else
            {
                this.ClearAll();
            }
        }

        #endregion

        #region 更新品种_期货_交易费用GridView的双击事件

        /// <summary>
        /// 更新品种_期货_交易费用GridView的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdFutureCostsResult_DoubleClick(object sender, EventArgs e)
        {
            cmbBreedClassID.Enabled = false;
            this.ModifyQHFutureCosts();
        }

        #endregion

        #region 删除品种_期货_交易费用

        /// <summary>
        /// 删除品种_期货_交易费用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdvFutureCostsSelect.GetDataRow(this.gdvFutureCostsSelect.FocusedRowHandle);
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
                    m_Result = FuturesManageCommon.DeleteQHFutureCosts(m_BreedClassID);
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
                this.QueryQHFutureCosts();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6203";
                string errMsg = "删除品种_期货_交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 期货手续费类型 cmbCostType_SelectedIndexChanged事件
        /// <summary>
        /// 期货手续费类型 cmbCostType_SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbCostType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.FutrueCostType.TradeUnitCharge - 1)
            {
                this.labCostTypeUnit.Text = "元/手";
            }
            else if (this.cmbCostType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.FutrueCostType.TurnoverRateOfSerCha - 1)
            {
                this.labCostTypeUnit.Text = "%";
            }
        }
        #endregion

        #region 取消按钮事件 btnCancel_Click
        /// <summary>
        /// 取消按钮事件 btnCancel_Click
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
            if (m_EditType == 1)
            {
                #region 添加操作
                try
                {
                    if (
                        FuturesManageCommon.ExistsFutureCosts(
                            ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的交易费用已存在!");
                        return;
                    }
                    QH_FutureCosts qH_FutureCosts = new QH_FutureCosts();

                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        qH_FutureCosts.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        qH_FutureCosts.BreedClassID = AppGlobalVariable.INIT_INT;
                    }
                    qH_FutureCosts.CurrencyTypeID = ((UComboItem)this.cmbCurrencyTypeID.SelectedItem).ValueIndex;
                    qH_FutureCosts.CostType = ((UComboItem)this.cmbCostType.SelectedItem).ValueIndex;
                    //if (this.cmbCostType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.FutrueCostType.TradeUnitCharge - 1)
                    //{
                    //    if (!string.IsNullOrEmpty(this.txtCost.Text))
                    //    {
                    //        if (InputTest.DecimalTest(this.txtCost.Text))
                    //        {
                    //            qH_FutureCosts.TradeUnitCharge = Convert.ToDecimal(this.txtCost.Text);
                    //        }
                    //        else
                    //        {
                    //            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //qH_FutureCosts.TradeUnitCharge = AppGlobalVariable.INIT_DECIMAL;
                    //        ShowMessageBox.ShowInformation("请填写手续费!");
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    if (!string.IsNullOrEmpty(this.txtCost.Text))
                    {
                        if (InputTest.DecimalTest(this.txtCost.Text))
                        {
                            qH_FutureCosts.TurnoverRateOfServiceCharge = Convert.ToDecimal(this.txtCost.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //qH_FutureCosts.TurnoverRateOfServiceCharge = AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("请填写手续费!");
                        return;
                    }
                    //}
                    m_Result = FuturesManageCommon.AddQHFutureCosts(qH_FutureCosts);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QueryQHFutureCosts();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-6201";
                    string errMsg = "添加品种_期货_交易费用失败!";
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
                    QH_FutureCosts qH_FutureCosts = new QH_FutureCosts();
                    if (m_BreedClassID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    this.cmbBreedClassID.Enabled = false;
                    qH_FutureCosts.BreedClassID = m_BreedClassID;
                    qH_FutureCosts.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    qH_FutureCosts.CurrencyTypeID = ((UComboItem)this.cmbCurrencyTypeID.SelectedItem).ValueIndex;
                    qH_FutureCosts.CostType = ((UComboItem)this.cmbCostType.SelectedItem).ValueIndex;

                    //if (this.cmbCostType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.FutrueCostType.TradeUnitCharge - 1)
                    //{
                    //    if (!string.IsNullOrEmpty(this.txtCost.Text))
                    //    {
                    //        if (InputTest.DecimalTest(this.txtCost.Text))
                    //        {
                    //            qH_FutureCosts.TradeUnitCharge = Convert.ToDecimal(this.txtCost.Text);
                    //        }
                    //        else
                    //        {
                    //            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //qH_FutureCosts.TradeUnitCharge = AppGlobalVariable.INIT_DECIMAL;
                    //        ShowMessageBox.ShowInformation("请填写手续费!");
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    if (!string.IsNullOrEmpty(this.txtCost.Text))
                    {
                        if (InputTest.DecimalTest(this.txtCost.Text))
                        {
                            qH_FutureCosts.TurnoverRateOfServiceCharge = Convert.ToDecimal(this.txtCost.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //qH_FutureCosts.TurnoverRateOfServiceCharge = AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("请填写手续费!");
                        return;
                    }
                    //}
                    m_Result = FuturesManageCommon.UpdateQHFutureCosts(qH_FutureCosts);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.cmbBreedClassID.Enabled = true;
                        this.ClearAll();
                        m_BreedClassID = AppGlobalVariable.INIT_INT;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                    this.QueryQHFutureCosts();
                }
                catch (Exception ex)
                {
                    string errCode = "GL-6202";
                    string errMsg = "更新品种_期货_交易费用失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion
            }
        }
        #endregion 确定按钮事件
    }
}