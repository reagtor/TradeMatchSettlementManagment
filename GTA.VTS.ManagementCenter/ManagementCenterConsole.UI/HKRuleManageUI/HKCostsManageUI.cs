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

namespace ManagementCenterConsole.UI.HKRuleManageUI
{
    /// <summary>
    /// 描述：港股交易费用管理窗体  错误编码范围:7800-7819
    /// 作者：刘书伟
    /// 日期：2009-10-23
    /// 修改：叶振东
    /// 时间：2010-04-06
    /// 描述：修改港股交易费用管理窗体中按钮事件
    /// </summary>
    public partial class HKCostsManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public HKCostsManageUI()
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
        /// 品种ID
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;


        #region 操作类型　 1:添加,2:修改
        /// <summary>
        /// 操作类型　 1:添加,2:修改
        /// </summary>
        private int m_EditType = (int)UITypes.EditTypeEnum.AddUI;
        #endregion

        #endregion

        //================================  私有  方法 ================================

        #region 获取港股品种名称 GetBindHKBreedClassName

        /// <summary>
        /// 获取港股品种名称
        /// </summary>
        private void GetBindHKBreedClassName()
        {
            try
            {
                string strWhere = " BreedClassTypeID=4 and DeleteState is not null and DeleteState<>1 ";
                DataSet ds = CommonParameterSetCommon.GetList(strWhere); //从交易商品品种表中获取品种类型是港股的品种名称
                UComboItem _item;
                if (ds != null && ds.Tables[0].Rows.Count != 0)
                {
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
                string errCode = "GL-7807";
                string errMsg = "获取港股品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
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

        #region 根据查询条件，获取港股交易费用

        /// <summary>
        /// 根据查询条件，获取港股交易费用
        /// </summary>
        /// <returns></returns>
        private bool QueryHKCosts()
        {
            try
            {
                string breedClassName = this.txtBreedClassName.Text;
                DataSet _dsHKCosts = HKManageCommon.GetAllHKSpotCosts(breedClassName,
                                                                        m_pageNo,
                                                                        m_pageSize,
                                                                        out m_rowCount);
                DataTable _dtHKCosts;
                if (_dsHKCosts == null || _dsHKCosts.Tables[0].Rows.Count == 0)
                {
                    _dtHKCosts = new DataTable();
                }
                else
                {
                    _dtHKCosts = _dsHKCosts.Tables[0];
                }

                //绑定品种名称
                this.ddlBreedClassID.DataSource = HKManageCommon.GetHKSpotCostsBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                     HKManageCommon.GetHKSpotCostsBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                     HKManageCommon.GetHKSpotCostsBreedClassName().Tables[0].Columns["BreedClassName"].ToString();

                //绑定币种类型
                this.ddlCurrencyTypeID.DataSource = BindData.GetBindListCurrencyType();
                this.ddlCurrencyTypeID.ValueMember = "ValueIndex";
                this.ddlCurrencyTypeID.DisplayMember = "TextTitleValue";

                //绑定印花税收取方式
                this.ddlStampDutyTypeID.DataSource = BindData.GetBindListStampDutyType();
                this.ddlStampDutyTypeID.ValueMember = "ValueIndex";
                this.ddlStampDutyTypeID.DisplayMember = "TextTitleValue";

                this.gdHKCostsResult.DataSource = _dtHKCosts;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7801";
                string errMsg = "根据查询条件，获取港股交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }

            return true;
        }

        #endregion

        #region 修改港股交易费用

        /// <summary>
        /// 修改港股交易费用
        /// </summary>
        private void ModifyHKCosts()
        {
            try
            {
                if (this.gdvHKCostsSelect != null && this.gdvHKCostsSelect.DataSource != null &&
                    this.gdvHKCostsSelect.RowCount > 0 && this.gdvHKCostsSelect.FocusedRowHandle >= 0)
                {
                    DataRow _dr = this.gdvHKCostsSelect.GetDataRow(this.gdvHKCostsSelect.FocusedRowHandle);
                    foreach (object item in this.cmbBreedClassID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BreedClassID"]))
                        {
                            this.cmbBreedClassID.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in this.cmbCurrencyType.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["CurrencyTypeID"]))
                        {
                            this.cmbCurrencyType.SelectedItem = item;
                            break;
                            ;
                        }
                    }

                    foreach (object item in this.cmbStampDutyType.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["StampDutyTypeID"]))
                        {
                            this.cmbStampDutyType.SelectedItem = item;
                            break;
                            ;
                        }
                    }
                    this.txtStampDuty.Text = Convert.ToString(_dr["StampDuty"]);
                    this.txtStampDutyStartpoint.Text = Convert.ToString(_dr["StampDutyStartingpoint"]);
                    this.txtCommision.Text = Convert.ToString(_dr["Commision"]);
                    this.txtCommisionStartpoint.Text = Convert.ToString(_dr["CommisionStartingpoint"]);
                    this.txtPoundageValue.Text = Convert.ToString(_dr["PoundageValue"]);
                    this.txtMonitoringFee.Text = Convert.ToString(_dr["MonitoringFee"]);
                    this.txtTransferToll.Text = Convert.ToString(_dr["TransferToll"]);
                    this.txtSystemToll.Text = Convert.ToString(_dr["SystemToll"]);

                    this.cmbBreedClassID.Enabled = false;//品种名称不能修改
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7805";
                string errMsg = "修改港股交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return;
            }
        }

        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            //this.cmbBreedClassID.Text = string.Empty; //清空品种ID
            //this.cmbCurrencyType.Text = string.Empty; //清空货币类型
            //this.cmbStampDutyType.Text = string.Empty; //清空印花税收取方式

            //按钮的禁用
            this.cmbBreedClassID.Enabled = false;
            this.cmbCurrencyType.Enabled = false;
            this.cmbStampDutyType.Enabled = false;
            this.txtStampDuty.Enabled = false;
            this.txtStampDutyStartpoint.Enabled = false;
            this.txtCommision.Enabled = false;
            this.txtCommisionStartpoint.Enabled = false;
            this.txtPoundageValue.Enabled = false;
            this.txtMonitoringFee.Enabled = false;
            this.txtSystemToll.Enabled = false;
            this.txtTransferToll.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnAdd.Text = "添加";
            this.btnAdd.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnOK.Enabled = false;
        }

        #endregion

        //================================  事件 ================================

        #region 港股交易费UI HKCostsManageUI_Load
        /// <summary>
        /// 港股交易费UI HKCostsManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HKCostsManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定港股交易费用表中的品种ID对应的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindHKBreedClassName(); //获取港股交易费用表中的品种ID对应的品种名称
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定币种类型
                this.cmbCurrencyType.Properties.Items.Clear();
                this.cmbCurrencyType.Properties.Items.AddRange(BindData.GetBindListCurrencyType());
                this.cmbCurrencyType.SelectedIndex = 1; //0;

                //绑定印花税收取方式
                this.cmbStampDutyType.Properties.Items.Clear();
                this.cmbStampDutyType.Properties.Items.AddRange(BindData.GetBindListStampDutyType());
                this.cmbStampDutyType.SelectedIndex = 2; //0;

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdHKCostsResult.DataSource = this.QueryHKCosts();
                this.ShowDataPage();

                //窗体加载文本框和确定按钮禁用
                this.cmbBreedClassID.Enabled = false;
                this.cmbCurrencyType.Enabled = false;
                this.cmbStampDutyType.Enabled = false;
                this.txtStampDuty.Enabled = false;
                this.txtStampDutyStartpoint.Enabled = false;
                this.txtCommision.Enabled = false;
                this.txtCommisionStartpoint.Enabled = false;
                this.txtPoundageValue.Enabled = false;
                this.txtMonitoringFee.Enabled = false;
                this.txtSystemToll.Enabled = false;
                this.txtTransferToll.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7800";
                string errMsg = "港股交易费用UI加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
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
                this.QueryHKCosts();
            }
        }

        #endregion

        #region 查询港股交易费用
        /// <summary>
        /// 查询港股交易费用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryHKCosts();
                this.ShowDataPage(); //显示数据分页

                DataView _dvHKCosts = (DataView)this.gdvHKCostsSelect.DataSource;
                DataTable _dtHKCosts = _dvHKCosts.Table;
                if (_dtHKCosts == null || _dtHKCosts.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7802";
                string errMsg = "查询港股交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return;
            }
        }
        #endregion

        #region 添加港股交易费用
        /// <summary>
        /// 添加港股交易费用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            m_EditType = (int)UITypes.EditTypeEnum.AddUI;
            string Name = this.btnAdd.Text.ToString().Trim();
            if(Name.Equals("添加"))
            {
                #region 按钮禁用
                this.btnAdd.Text = "取消";
                this.cmbBreedClassID.Enabled = true;
                this.cmbCurrencyType.Enabled = true;
                this.cmbStampDutyType.Enabled = true;
                this.txtStampDuty.Enabled = true;
                this.txtStampDutyStartpoint.Enabled = true;
                this.txtCommision.Enabled = true;
                this.txtCommisionStartpoint.Enabled = true;
                this.txtPoundageValue.Enabled = true;
                this.txtMonitoringFee.Enabled = true;
                this.txtSystemToll.Enabled = true;
                this.txtTransferToll.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
                #endregion
                #region 文本框清空
                this.txtStampDuty.Text = string.Empty; //清空印花税
                this.txtStampDutyStartpoint.Text = string.Empty; //清空印花税起点
                this.txtCommision.Text = string.Empty; //清空佣金
                this.txtCommisionStartpoint.Text = string.Empty; //清空佣金起点
                this.txtPoundageValue.Text = string.Empty;//清空交易费
                this.txtMonitoringFee.Text = string.Empty;//清空监管费
                this.txtTransferToll.Text = string.Empty; //清空过户费
                this.txtSystemToll.Text = string.Empty;//清空交易系统使用费
                #endregion
            }
            else if(Name.Equals("取消"))
            {
                this.ClearAll();
            }
        }
        #endregion

        #region 修改港股交易费用
        /// <summary>
        /// 修改港股交易费用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            m_EditType = (int)UITypes.EditTypeEnum.UpdateUI;
            string Name = this.btnModify.Text.ToString().Trim();
            if (Name.Equals("修改"))
            {
                #region 按钮禁用
                this.btnModify.Text = "取消";
                this.cmbCurrencyType.Enabled = true;
                this.cmbStampDutyType.Enabled = true;
                this.txtStampDuty.Enabled = true;
                this.txtStampDutyStartpoint.Enabled = true;
                this.txtCommision.Enabled = true;
                this.txtCommisionStartpoint.Enabled = true;
                this.txtPoundageValue.Enabled = true;
                this.txtMonitoringFee.Enabled = true;
                this.txtSystemToll.Enabled = true;
                this.txtTransferToll.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
                #endregion
            }
            else if (Name.Equals("取消"))
            {
                this.ClearAll();
            }
        }
        #endregion

        #region 修改港股交易费用的GridView的双击事件 gdHKCostsResult_DoubleClick
        /// <summary>
        /// 修改港股交易费用的GridView的双击事件 gdHKCostsResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdHKCostsResult_DoubleClick(object sender, EventArgs e)
        {
            this.ModifyHKCosts();
            this.ClearAll();
        }
        #endregion

        #region 删除港股交易费用
        /// <summary>
        /// 删除港股交易费用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;

                DataRow _dr = this.gdvHKCostsSelect.GetDataRow(this.gdvHKCostsSelect.FocusedRowHandle);
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
                    m_Result = HKManageCommon.DeleteHKSpotCosts(m_BreedClassID);
                }

                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_BreedClassID = AppGlobalVariable.INIT_INT;
                    this.QueryHKCosts();
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7806";
                string errMsg = "删除港股交易费用失败!";
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

        #region   确定按钮事件
        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //通过状态值来进行判断是进行那种操作 1:添加 2:修改
            if (m_EditType == 1)
            {
                #region 添加操作

                try
                {
                    if (
                        HKManageCommon.ExistsHKSpotCosts(
                            ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的交易费用已存在!");
                        return;
                    }
                    HK_SpotCosts hK_SpotCosts = new HK_SpotCosts();
                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        hK_SpotCosts.BreedClassID = ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("品种名称不能为空!");
                        return;
                    }
                    hK_SpotCosts.CurrencyTypeID = ((UComboItem) this.cmbCurrencyType.SelectedItem).ValueIndex;
                    hK_SpotCosts.StampDutyTypeID = ((UComboItem) this.cmbStampDutyType.SelectedItem).ValueIndex;
                    if (!string.IsNullOrEmpty(this.txtStampDuty.Text)) //印花税
                    {
                        if (InputTest.DecimalTest(this.txtStampDuty.Text))
                        {
                            hK_SpotCosts.StampDuty = Convert.ToDecimal(this.txtStampDuty.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //hK_SpotCosts.StampDuty = 0; // AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("印花税不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtStampDutyStartpoint.Text)) //印花税起点
                    {
                        if (InputTest.DecimalTest(this.txtStampDutyStartpoint.Text))
                        {
                            hK_SpotCosts.StampDutyStartingpoint = Convert.ToDecimal(this.txtStampDutyStartpoint.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        hK_SpotCosts.StampDutyStartingpoint = 0; //默认为0
                    }
                    if (!string.IsNullOrEmpty(this.txtCommision.Text)) //佣金
                    {
                        if (InputTest.DecimalTest(this.txtCommision.Text))
                        {
                            hK_SpotCosts.Commision = Convert.ToDecimal(this.txtCommision.Text); //null;
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //hK_SpotCosts.Commision = 0; //
                        ShowMessageBox.ShowInformation("佣金不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtCommisionStartpoint.Text)) //佣金起点
                    {
                        if (InputTest.DecimalTest(this.txtCommisionStartpoint.Text))
                        {
                            hK_SpotCosts.CommisionStartingpoint = Convert.ToDecimal(this.txtCommisionStartpoint.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        hK_SpotCosts.CommisionStartingpoint = 0; // //默认为0AppGlobalVariable.INIT_DECIMAL;
                    }
                    if (!string.IsNullOrEmpty(this.txtPoundageValue.Text)) //交易费
                    {
                        if (InputTest.DecimalTest(this.txtPoundageValue.Text))
                        {
                            hK_SpotCosts.PoundageValue = Convert.ToDecimal(this.txtPoundageValue.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("交易费不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtMonitoringFee.Text)) //监管费
                    {
                        if (InputTest.DecimalTest(this.txtMonitoringFee.Text))
                        {
                            hK_SpotCosts.MonitoringFee = Convert.ToDecimal(this.txtMonitoringFee.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("监管费不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtSystemToll.Text)) //交易系统使用费
                    {
                        if (InputTest.DecimalTest(this.txtSystemToll.Text))
                        {
                            hK_SpotCosts.SystemToll = Convert.ToDecimal(this.txtSystemToll.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("交易系统使用费不能为空!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtTransferToll.Text)) //过户费
                    {
                        if (InputTest.DecimalTest(this.txtTransferToll.Text))
                        {
                            hK_SpotCosts.TransferToll = Convert.ToDecimal(this.txtTransferToll.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        hK_SpotCosts.TransferToll = 0; //默认为0
                    }
                    m_Result = HKManageCommon.AddHKSpotCosts(hK_SpotCosts);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QueryHKCosts();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }

                }
                catch (Exception ex)
                {
                    string errCode = "GL-7803";
                    string errMsg = "添加港股交易费用失败!";
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
                    HK_SpotCosts hK_SpotCosts = new HK_SpotCosts();
                    hK_SpotCosts.BreedClassID = ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex; //品种名称不能修改
                    hK_SpotCosts.CurrencyTypeID = ((UComboItem) this.cmbCurrencyType.SelectedItem).ValueIndex;
                    hK_SpotCosts.StampDutyTypeID = ((UComboItem) this.cmbStampDutyType.SelectedItem).ValueIndex;
                    if (!string.IsNullOrEmpty(this.txtStampDuty.Text)) //印花税
                    {
                        if (InputTest.DecimalTest(this.txtStampDuty.Text))
                        {
                            hK_SpotCosts.StampDuty = Convert.ToDecimal(this.txtStampDuty.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //hK_SpotCosts.StampDuty = 0; // AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("印花税不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtStampDutyStartpoint.Text)) //印花税起点
                    {
                        if (InputTest.DecimalTest(this.txtStampDutyStartpoint.Text))
                        {
                            hK_SpotCosts.StampDutyStartingpoint = Convert.ToDecimal(this.txtStampDutyStartpoint.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        hK_SpotCosts.StampDutyStartingpoint = 0; //默认为0
                    }
                    if (!string.IsNullOrEmpty(this.txtCommision.Text)) //佣金
                    {
                        if (InputTest.DecimalTest(this.txtCommision.Text))
                        {
                            hK_SpotCosts.Commision = Convert.ToDecimal(this.txtCommision.Text); //null;
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        //hK_SpotCosts.Commision = 0; //
                        ShowMessageBox.ShowInformation("佣金不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtCommisionStartpoint.Text)) //佣金起点
                    {
                        if (InputTest.DecimalTest(this.txtCommisionStartpoint.Text))
                        {
                            hK_SpotCosts.CommisionStartingpoint = Convert.ToDecimal(this.txtCommisionStartpoint.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        hK_SpotCosts.CommisionStartingpoint = 0; ////默认为0 AppGlobalVariable.INIT_DECIMAL;
                    }
                    if (!string.IsNullOrEmpty(this.txtPoundageValue.Text)) //交易费
                    {
                        if (InputTest.DecimalTest(this.txtPoundageValue.Text))
                        {
                            hK_SpotCosts.PoundageValue = Convert.ToDecimal(this.txtPoundageValue.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("交易费不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtMonitoringFee.Text)) //监管费
                    {
                        if (InputTest.DecimalTest(this.txtMonitoringFee.Text))
                        {
                            hK_SpotCosts.MonitoringFee = Convert.ToDecimal(this.txtMonitoringFee.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("监管费不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtSystemToll.Text)) //交易系统使用费
                    {
                        if (InputTest.DecimalTest(this.txtSystemToll.Text))
                        {
                            hK_SpotCosts.SystemToll = Convert.ToDecimal(this.txtSystemToll.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("交易系统使用费不能为空!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtTransferToll.Text)) //过户费
                    {
                        if (InputTest.DecimalTest(this.txtTransferToll.Text))
                        {
                            hK_SpotCosts.TransferToll = Convert.ToDecimal(this.txtTransferToll.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        hK_SpotCosts.TransferToll = 0; //默认为0
                    }
                    m_Result = HKManageCommon.UpdateHKSpotCosts(hK_SpotCosts);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                        this.QueryHKCosts();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }

                }
                catch (Exception ex)
                {
                    string errCode = "GL-7804";
                    string errMsg = "修改港股交易费用失败!";
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
