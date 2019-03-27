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
    /// 描述：港股交易规则管理窗体  错误编码范围:7900-7919
    /// 作者：刘书伟
    /// 日期：2009-10-23
    /// 修改：叶振东
    /// 时间：2010-04-06
    /// 描述：修改港股交易规则管理窗体中按钮事件
    /// </summary>
    public partial class HKTradeRulesManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public HKTradeRulesManageUI()
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

        #region 操作类型　 1:添加,2:修改
        /// <summary>
        /// 操作类型　 1:添加,2:修改
        /// </summary>
        private int m_EditType = (int)UITypes.EditTypeEnum.AddUI;
        #endregion

        /// <summary>
        /// 品种ID
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

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
                DataSet ds = CommonParameterSetCommon.GetList(strWhere); //从交易商品品种表中获取港股品种类型的品种
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
                string errCode = "GL-7907";
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

        #region 根据查询条件，获取港股交易规则

        /// <summary>
        /// 根据查询条件，获取港股交易规则
        /// </summary>
        /// <returns></returns>
        private bool QueryHKTradeRules()
        {
            try
            {
                string breedClassName = this.txtBreeClassName.Text;
                DataSet _dsHKTradeRules = HKManageCommon.GetAllHKSpotTradeRules(breedClassName,
                                                                                  m_pageNo,
                                                                                  m_pageSize,
                                                                                  out m_rowCount);
                DataTable _dtHKTradeRule;
                if (_dsHKTradeRules == null || _dsHKTradeRules.Tables[0].Rows.Count == 0)
                {
                    _dtHKTradeRule = new DataTable();
                }
                else
                {
                    _dtHKTradeRule = _dsHKTradeRules.Tables[0];
                }

                //绑定品种名称
                this.ddlBreedClassID.DataSource = HKManageCommon.GetHKBreedClassNameByBreedClassID().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    HKManageCommon.GetHKBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    HKManageCommon.GetHKBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassName"].ToString();

                //绑定行情成交量单位
                this.ddlMarketUnitID.DataSource = BindData.GetBindListXHAboutUnit();
                this.ddlMarketUnitID.ValueMember = "ValueIndex";
                this.ddlMarketUnitID.DisplayMember = "TextTitleValue";

                //绑定计价单位
                this.ddlPriceUnit.DataSource = BindData.GetBindListXHAboutUnit();
                this.ddlPriceUnit.ValueMember = "ValueIndex";
                this.ddlPriceUnit.DisplayMember = "TextTitleValue";

                this.gdHKTradeRulesQResult.DataSource = _dtHKTradeRule;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7901";
                string errMsg = "根据查询条件，获取港股交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                //throw exception;
                return false;
            }
            return true;
        }

        #endregion

        #region 修改港股交易规则

        /// <summary>
        /// 修改港股交易规则
        /// </summary>
        private void ModifyHKTradeRules()
        {
            try
            {
                if (this.gdvHKTradeRulesSelect != null && this.gdvHKTradeRulesSelect.DataSource != null &&
                    this.gdvHKTradeRulesSelect.RowCount > 0 && this.gdvHKTradeRulesSelect.FocusedRowHandle >= 0)
                {
                    DataRow _dr = this.gdvHKTradeRulesSelect.GetDataRow(this.gdvHKTradeRulesSelect.FocusedRowHandle);
                    foreach (object item in this.cmbBreedClassID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BreedClassID"]))
                        {
                            this.cmbBreedClassID.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in this.cmbPriceUnit.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["PriceUnit"]))
                        {
                            this.cmbPriceUnit.SelectedItem = item;
                            break;
                            ;
                        }
                    }

                    foreach (object item in this.cmbMarketUnitID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["MarketUnitID"]))
                        {
                            this.cmbMarketUnitID.SelectedItem = item;
                            break;
                            ;
                        }
                    }
                    this.speFundDeliveryIns.EditValue = Convert.ToInt32(_dr["FundDeliveryInstitution"]);
                    this.speStockDeliveryIns.EditValue = Convert.ToInt32(_dr["StockDeliveryInstitution"]);
                    this.txtMaxLeaveQuantity.Text = Convert.ToString(_dr["MaxLeaveQuantity"]);
                    this.cmbBreedClassID.Enabled = false;//品种名称不能修改
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7905";
                string errMsg = "获取需要修改的港股交易商品失败!";
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
            //this.cmbBreedClassID.Text = string.Empty;
            //this.cmbPriceUnit.Text = string.Empty;
            //this.cmbMarketUnitID.Text = string.Empty;
            //this.speStockDeliveryIns.EditValue = 0;
            //this.speFundDeliveryIns.EditValue = 0;
            //this.txtMaxLeaveQuantity.Text = string.Empty;
            //this.cmbBreedClassID.Enabled = true;//// 取消后品种类型可选择状态

            this.cmbMarketUnitID.Enabled = false;
            this.speFundDeliveryIns.Enabled = false;
            this.speStockDeliveryIns.Enabled = false;
            this.txtMaxLeaveQuantity.Enabled = false;
            this.cmbBreedClassID.Enabled = false;
            this.cmbPriceUnit.Enabled = false;
            this.btnOK.Enabled = false;

            this.btnAdd.Text = "添加";
            this.btnAdd.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnOK.Enabled = false;
        }

        #endregion

        #region 检验港股品种交易规则的输入
        /// <summary>
        /// 检验港股品种交易规则的输入
        /// </summary>
        /// <param name="strMess">提示信息</param>
        /// <returns></returns>
        private HK_SpotTradeRules VerifyHKSpotTradeRules(ref string strMess)
        {
            try
            {
                HK_SpotTradeRules hK_SpotTradeRules = new HK_SpotTradeRules();
                strMess = string.Empty;
                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    hK_SpotTradeRules.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                }
                else
                {
                    strMess = "请选择品种类型!";
                }
                hK_SpotTradeRules.FundDeliveryInstitution = Convert.ToInt32(this.speFundDeliveryIns.EditValue);
                hK_SpotTradeRules.StockDeliveryInstitution = Convert.ToInt32(this.speStockDeliveryIns.EditValue);
                if (!string.IsNullOrEmpty(this.txtMaxLeaveQuantity.Text))
                {
                    if (InputTest.intTest(this.txtMaxLeaveQuantity.Text))
                    {
                        hK_SpotTradeRules.MaxLeaveQuantity = Convert.ToInt32(this.txtMaxLeaveQuantity.Text);
                    }
                    else
                    {
                        strMess = "请输入数字且第一位数不能为0!";
                    }
                }
                else
                {
                    strMess = "每笔最大委托量不能为空!";
                }
                if (!string.IsNullOrEmpty(this.cmbPriceUnit.Text))
                {
                    hK_SpotTradeRules.PriceUnit = ((UComboItem)this.cmbPriceUnit.SelectedItem).ValueIndex;
                }
                else
                {
                    strMess = "计价单位不能为空!";
                }
                if (!string.IsNullOrEmpty(this.cmbMarketUnitID.Text))
                {
                    hK_SpotTradeRules.MarketUnitID = ((UComboItem)this.cmbMarketUnitID.SelectedItem).ValueIndex;
                }
                else
                {
                    strMess = "行情成交量单位不能为空!";
                }
                return hK_SpotTradeRules;

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }
        #endregion

        //================================  事件 ================================

        #region 港股交易规则管理窗体 HKTradeRulesManageUI_Load
        /// <summary>
        ///港股交易规则管理窗体 HKTradeRulesManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HKTradeRulesManageUI_Load(object sender, EventArgs e)
        {
            try
            {

                //绑定交易规则表中的品种ID对应的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindHKBreedClassName(); //从交易商品品种表中获取港股品种类型的品种
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定计价单位
                this.cmbPriceUnit.Properties.Items.Clear();
                this.cmbPriceUnit.Properties.Items.AddRange(BindData.GetBindListHKAboutUnit());
                this.cmbPriceUnit.SelectedIndex = 0;

                //绑定行情成交量单位
                this.cmbMarketUnitID.Properties.Items.Clear();
                this.cmbMarketUnitID.Properties.Items.AddRange(BindData.GetBindListHKAboutUnit());
                this.cmbMarketUnitID.SelectedIndex = 0;

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdHKTradeRulesQResult.DataSource = this.QueryHKTradeRules();
                this.ShowDataPage();

                //窗体加载时禁用界面中文本框和确定按钮
                this.cmbMarketUnitID.Enabled = false;
                this.speFundDeliveryIns.Enabled = false;
                this.speStockDeliveryIns.Enabled = false;
                this.txtMaxLeaveQuantity.Enabled = false;
                this.cmbBreedClassID.Enabled = false;
                this.cmbPriceUnit.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7900";
                string errMsg = " 港股交易规则管理UI加载失败!";
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
                this.QueryHKTradeRules();
            }
        }

        #endregion

        #region 查询港股交易规则
        /// <summary>
        /// 查询港股交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryHKTradeRules();
                this.ShowDataPage(); //显示数据分页
                DataView _dvHKTradeRules = (DataView)this.gdvHKTradeRulesSelect.DataSource;
                DataTable _dtHKTradeRules = _dvHKTradeRules.Table;
                if (_dtHKTradeRules == null || _dtHKTradeRules.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7902";
                string errMsg = "查询港股交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 添加港股交易规则
        /// <summary>
        ///添加港股交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            m_EditType = (int)UITypes.EditTypeEnum.AddUI;
            string Name = this.btnAdd.Text;
            if (Name.Equals("添加"))
            {
                this.btnAdd.Text = "取消";
                this.cmbMarketUnitID.Enabled = true;
                this.speFundDeliveryIns.Enabled = true;
                this.speStockDeliveryIns.Enabled = true;
                this.txtMaxLeaveQuantity.Enabled = true;
                this.cmbBreedClassID.Enabled = true;
                this.cmbPriceUnit.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.ClearAll();
            }
        }
        #endregion

        #region 修改港股交易规则
        /// <summary>
        /// 修改港股交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            m_EditType = (int)UITypes.EditTypeEnum.UpdateUI;
            string Name = this.btnModify.Text;
            if (Name.Equals("修改"))
            {
                this.btnModify.Text = "取消";
                this.cmbMarketUnitID.Enabled = true;
                this.speFundDeliveryIns.Enabled = true;
                this.speStockDeliveryIns.Enabled = true;
                this.txtMaxLeaveQuantity.Enabled = true;
                this.cmbPriceUnit.Enabled = true;
                this.btnOK.Enabled = true;
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.ClearAll();
            }
        }
        #endregion

        #region 修改港股交易规则的GridView的双击事件 gdHKTradeRulesQResult_DoubleClick
        /// <summary>
        /// 修改港股交易商品的GridView的双击事件 gdHKTradeRulesQResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdHKTradeRulesQResult_DoubleClick(object sender, EventArgs e)
        {
            this.ModifyHKTradeRules();
            this.ClearAll();
        }
        #endregion

        #region 删除港股交易规则
        /// <summary>
        /// 删除港股交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除交易规则吗？") == DialogResult.No) return;

                DataRow _dr = this.gdvHKTradeRulesSelect.GetDataRow(this.gdvHKTradeRulesSelect.FocusedRowHandle);
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
                    m_Result = HKManageCommon.DeleteHKSpotTradeRulesAbout(m_BreedClassID);
                }

                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_BreedClassID = AppGlobalVariable.INIT_INT;
                    this.QueryHKTradeRules();
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7906";
                string errMsg = "删除港股交易规则失败!";
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
            if (m_EditType == 1)
            {
                #region 添加操作

                try
                {
                    if (
                        HKManageCommon.ExistsHKSpotTradeRules(
                            ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的交易规则已存在!");
                        return;
                    }
                    HK_SpotTradeRules hK_SpotTradeRules = new HK_SpotTradeRules();
                    string _strMess = string.Empty;
                    hK_SpotTradeRules = VerifyHKSpotTradeRules(ref _strMess);
                    if (!string.IsNullOrEmpty(_strMess))
                    {
                        ShowMessageBox.ShowInformation(_strMess);
                        return;
                    }
                    m_Result = HKManageCommon.AddHKSpotTradeRules(hK_SpotTradeRules);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QueryHKTradeRules();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-7903";
                    string errMsg = "添加港股交易规则失败!";
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
                    HK_SpotTradeRules hK_SpotTradeRules = new HK_SpotTradeRules();
                    string _strMess = string.Empty;
                    hK_SpotTradeRules = VerifyHKSpotTradeRules(ref _strMess);
                    if (!string.IsNullOrEmpty(_strMess))
                    {
                        ShowMessageBox.ShowInformation(_strMess);
                        return;
                    }
                    m_Result = HKManageCommon.UpdateHKSpotTradeRules(hK_SpotTradeRules);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                        this.QueryHKTradeRules();

                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-7904";
                    string errMsg = "修改港股交易规则失败!";
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
