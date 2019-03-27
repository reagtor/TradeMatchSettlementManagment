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
    /// 描述：交易商品管理窗体  错误编码范围:4200-4219
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// 修改：叶振东
    /// 日期：2010-04-02
    /// 描述：修改交易商品管理窗体操作按钮事件
    /// </summary>
    public partial class CommodityManagerUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public CommodityManagerUI()
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
        /// 商品代码
        /// </summary>
        private string m_CommodityCode = AppGlobalVariable.INIT_STRING;

        /// <summary>
        /// 是否是系统默认代码
        /// </summary>
        private int m_ISSysDefaultCode = AppGlobalVariable.INIT_INT;

        /// <summary>
        ///区别现货和期货(股指期货和商品期);当为true时，是现货；为false时是期货 
        /// </summary>
        private bool m_DiffBreeadClassType;

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

        #region 获取品种名称 GetBindBreedClassName

        /// <summary>
        /// 获取品种名称
        /// </summary>
        private void GetBindBreedClassName()
        {
            string strWhere = " BreedClassTypeID<>4  and DeleteState<>1 ";
            DataSet ds = CommonParameterSetCommon.GetList(strWhere);//从交易商品品种中获取
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

        #endregion

        #region 获取查询品种名称 GetBindQueryBreedClassName

        /// <summary>
        /// 获取查询品种名称
        /// </summary>
        private void GetBindQueryBreedClassName()
        {
            DataSet ds = CommonParameterSetCommon.GetAllBreedClassName(); //从交易商品品种中获取
            UComboItem _item;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                _item = new UComboItem(ds.Tables[0].Rows[i]["BreedClassName"].ToString(),
                                       Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]));

                //绑定查询条件品种标识(品种名称)
                this.cmbBreedClassIDQ.Properties.Items.Add(_item);
            }

        }

        #endregion

        #region 根据查询条件，获取交易商品

        /// <summary>
        /// 根据查询条件，获取交易商品
        /// </summary>
        /// <returns></returns>
        private bool QueryCMCommodity()
        {
            try
            {
                //商品代码
                string CommodityCode = AppGlobalVariable.INIT_STRING;
                //商品名称
                string CommodityName = AppGlobalVariable.INIT_STRING;
                //品种ID
                int BreedClassID = AppGlobalVariable.INIT_INT;
                if (!string.IsNullOrEmpty(txtCondition.Text))
                {
                    CommodityCode = txtCondition.Text;
                    CommodityName = txtCondition.Text;
                }
                if (!string.IsNullOrEmpty(cmbBreedClassIDQ.Text))
                {
                    ;
                    BreedClassID = ((UComboItem)cmbBreedClassIDQ.SelectedItem).ValueIndex;
                }


                DataSet _dsCMCommodity = CommonParameterSetCommon.GetAllCMCommodity(CommodityCode, CommodityName,
                                                                                    BreedClassID,
                                                                                    m_pageNo,
                                                                                    m_pageSize,
                                                                                    out m_rowCount);
                DataTable _dtCMCommodity;
                if (_dsCMCommodity == null || _dsCMCommodity.Tables[0].Rows.Count == 0)
                {
                    _dtCMCommodity = new DataTable();
                }
                else
                {
                    _dtCMCommodity = _dsCMCommodity.Tables[0];
                }

                //绑定交易商品品种表中的品种ID对应的品种名称
                this.ddlBreedClassID.DataSource = CommonParameterSetCommon.GetAllBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    CommonParameterSetCommon.GetAllBreedClassName().Tables[0].Columns["BreedClassID"].ToString();

                this.ddlBreedClassID.DisplayMember =
                    CommonParameterSetCommon.GetAllBreedClassName().Tables[0].Columns["BreedClassName"].ToString();

                this.gdCommodityResult.DataSource = _dtCMCommodity;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4204";
                string errMsg = "根据查询条件，获取交易商品失败!";
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
            //this.txtCommodityCode.Text = string.Empty;
            //this.txtCommodityName.Text = string.Empty;
            //this.txtGoerScale.Text = string.Empty;
            //this.txtLabelCommodityCode.Text = string.Empty;
            //this.txtStockPinYin.Text = string.Empty;
            //this.txtturnovervolume.Text = string.Empty;
            //this.dtMarketDate.Text = string.Empty;

            //说明：只允许修改品种类型
            this.txtCommodityCode.Enabled = false;//取消后代码变为可输入状态
            this.txtCommodityName.Enabled = false;// 取消后商品名称变为可输入状态
            this.dtMarketDate.Enabled = false;// 取消后上市日期变为可输入状态
            this.txtStockPinYin.Enabled = false;// 取消后简称的拼音变为可输入状态
            this.txtturnovervolume.Enabled = false;// 取消后流通股数变为可输入状态
            this.txtGoerScale.Enabled = false;// 取消后行权比例变为可输入状态
            this.txtLabelCommodityCode.Enabled = false;//取消后标的代码变为可输入状态
            this.cmbBreedClassID.Enabled = false;
            this.btnAdd.Enabled = true;
            this.btnAdd.Text = "添加";
            this.btnModify.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnDelete.Enabled = true;
            this.btnOK.Enabled = false;
        }

        #endregion
        #region  文本框启用事件
        /// <summary>
        /// 文本框启用事件
        /// </summary>
        private void EnabledTrue()
        {
            this.txtCommodityCode.Enabled = true;//取消后代码变为可输入状态
            this.txtCommodityName.Enabled = true;// 取消后商品名称变为可输入状态
            this.dtMarketDate.Enabled = true;// 取消后上市日期变为可输入状态
            this.txtStockPinYin.Enabled = true;// 取消后简称的拼音变为可输入状态
            this.txtturnovervolume.Enabled = true;// 取消后流通股数变为可输入状态
            this.txtGoerScale.Enabled = true;// 取消后行权比例变为可输入状态
            this.txtLabelCommodityCode.Enabled = true;//取消后标的代码变为可输入状态
            this.cmbBreedClassID.Enabled = true;
            this.btnOK.Enabled = true;
            this.btnDelete.Enabled = false;
        }
        #endregion
        #region 修改交易商品

        /// <summary>
        /// 修改交易商品
        /// </summary>
        private void ModifyCMCommodity()
        {
            try
            {
                if (this.gdvCommoditySelect != null && this.gdvCommoditySelect.DataSource != null &&
                    this.gdvCommoditySelect.RowCount > 0 && this.gdvCommoditySelect.FocusedRowHandle >= 0)
                {
                    DataRow _dr = this.gdvCommoditySelect.GetDataRow(this.gdvCommoditySelect.FocusedRowHandle);
                    m_CommodityCode = _dr["CommodityCode"].ToString();
                    this.txtCommodityCode.Text = _dr["CommodityCode"].ToString();
                    this.txtCommodityName.Text = _dr["CommodityName"].ToString();
                    this.txtGoerScale.Text = _dr["GoerScale"].ToString(); //?
                    this.txtLabelCommodityCode.Text = _dr["LabelCommodityCode"].ToString();
                    this.dtMarketDate.EditValue = Convert.ToDateTime(_dr["MarketDate"]);
                    this.txtStockPinYin.Text = _dr["StockPinYin"].ToString();
                    this.txtturnovervolume.Text = _dr["turnovervolume"].ToString();
                    m_ISSysDefaultCode = Convert.ToInt32(_dr["ISSysDefaultCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(_dr["BreedClassID"])))
                    {
                        foreach (object item in this.cmbBreedClassID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BreedClassID"]))
                            {
                                this.cmbBreedClassID.SelectedItem = item;
                                break;
                            }
                        }
                    }
                    //说明：只允许修改品种类型
                    this.txtCommodityCode.Enabled = false;//代码不允许修改
                    this.txtCommodityName.Enabled = false;// 商品名称不允许修改
                    this.dtMarketDate.Enabled = false;// 上市日期不允许修改
                    this.txtStockPinYin.Enabled = false;// 简称的拼音不允许修改
                    this.txtturnovervolume.Enabled = false;// 流通股数不允许修改
                    this.txtGoerScale.Enabled = false;// 行权比例不允许修改
                    this.txtLabelCommodityCode.Enabled = false;//标的代码不允许修改
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4206";
                string errMsg = "获取需要修改的交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return;
            }
        }

        #endregion

        #region  根据品种类型（现货或股指期货，商品期货）检验流通股份的输入
        /// <summary>
        /// 根据品种类型（现货或股指期货，商品期货）检验流通股份的输入
        /// </summary>
        private void TestInputTurnovervolumeText()
        {
            try
            {
                int breedClassID = AppGlobalVariable.INIT_INT;//品种ID
                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    breedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;

                    CM_BreedClass cM_BreedClass = new CM_BreedClass();
                    cM_BreedClass = CommonParameterSetCommon.GetBreedClassByBClassID(breedClassID);
                    if (cM_BreedClass != null)
                    {
                        int breedClassTypeID = Convert.ToInt32(cM_BreedClass.BreedClassTypeID);//品种类型ID
                        if (breedClassTypeID == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.Stock)
                        {
                            m_DiffBreeadClassType = true;
                        }
                        else if (breedClassTypeID != (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.Stock)
                        {
                            m_DiffBreeadClassType = false;

                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
        #endregion

        //================================  事件 ================================

        #region 交易商品管理窗体 CommodityManagerUI_Load

        /// <summary>
        /// 交易商品管理窗体 CommodityManagerUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommodityManagerUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定品种标识(品种名称)
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindBreedClassName();
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定查询条件品种标识(品种名称)
                this.cmbBreedClassIDQ.Properties.Items.Clear();
                this.GetBindQueryBreedClassName();
                //this.cmbBreedClassIDQ.SelectedIndex = 0;

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdCommodityResult.DataSource = this.QueryCMCommodity();
                this.ShowDataPage();

                TestInputTurnovervolumeText();

                //窗体加载禁用文本编辑
                this.btnOK.Enabled = false;
                this.txtCommodityCode.Enabled = false;
                this.txtCommodityName.Enabled = false;
                this.txtStockPinYin.Enabled = false;
                this.cmbBreedClassID.Enabled = false;
                this.dtMarketDate.Enabled = false;
                this.txtturnovervolume.Enabled = false;
                this.txtLabelCommodityCode.Enabled = false;
                this.txtGoerScale.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4200";
                string errMsg = "交易商品管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 查询交易商品

        /// <summary>
        /// 查询交易商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryCMCommodity();
                this.ShowDataPage(); //显示数据分页
                //DataTable _dtCMCommodity = (DataTable) this.gdvCommoditySelect.DataSource;
                DataView _dvCMComm = (DataView)this.gdvCommoditySelect.DataSource;
                DataTable _dtCMCommodity = _dvCMComm.Table;
                if (_dtCMCommodity == null || _dtCMCommodity.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4205";
                string errMsg = "查询交易商品失败!";
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
                this.QueryCMCommodity();
            }
        }

        #endregion

        #region 添加交易商品

        /// <summary>
        /// 添加交易商品
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
                this.txtCommodityCode.Text = string.Empty;
                this.txtCommodityName.Text = string.Empty;
                this.txtGoerScale.Text = string.Empty;
                this.txtLabelCommodityCode.Text = string.Empty;
                this.txtStockPinYin.Text = string.Empty;
                this.txtturnovervolume.Text = string.Empty;
                this.dtMarketDate.Text = string.Empty;
                this.EnabledTrue();
                this.btnModify.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnAdd.Text = "添加";
                this.ClearAll();
            }
        }

        #endregion

        #region  修改交易商品

        /// <summary>
        /// 修改交易商品
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
                this.btnDelete.Enabled = false;
                this.btnOK.Enabled = true;
                this.cmbBreedClassID.Enabled = true;
                this.btnAdd.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnModify.Text = "修改";
                this.ClearAll();
            }
        }

        #endregion

        #region 修改交易商品的GridView的双击事件 gdCommodityResult_DoubleClick

        /// <summary>
        /// 修改交易商品的GridView的双击事件 gdCommodityResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdCommodityResult_DoubleClick(object sender, EventArgs e)
        {
            //btnAdd.Enabled = false;
            this.ModifyCMCommodity();
            this.ClearAll();
        }

        #endregion

        #region 删除交易商品

        /// <summary>
        /// 删除交易商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdvCommoditySelect.GetDataRow(this.gdvCommoditySelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["CommodityCode"])))
                {
                    m_CommodityCode = Convert.ToString(_dr["CommodityCode"]);
                }
                else
                {
                    m_CommodityCode = AppGlobalVariable.INIT_STRING;
                }
                int breedClassID = Convert.ToInt32(_dr["BreedClassID"]);//品种ID
                if (m_CommodityCode != AppGlobalVariable.INIT_STRING)
                {
                    //m_Result = CommonParameterSetCommon.DeleteCMCommodity(m_CommodityCode, breedClassID);
                    int _ISSysDefaultCode = Convert.ToInt32(_dr["ISSysDefaultCode"]);
                    if (_ISSysDefaultCode == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No)
                    {
                        m_Result = CommonParameterSetCommon.DeleteCMCommodity(m_CommodityCode, breedClassID);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("系统默认代码不能删除!");
                        return;
                    }
                }
                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_CommodityCode = AppGlobalVariable.INIT_STRING;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryCMCommodity();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4202";
                string errMsg = "删除交易商品失败!";
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

        #region 品种名称组合框索引值改变事件
        /// <summary>
        /// 品种名称组合框索引值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBreedClassID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TestInputTurnovervolumeText();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4207";
                string errMsg = "品种名称组合框索引值改变事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
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
            //根据状态进行操作 1：添加  2：修改
            if (m_EditType == 1)
            {
                #region 添加操作

                try
                {
                    CM_Commodity cM_Commodity = new CM_Commodity();

                    if (!string.IsNullOrEmpty(this.txtCommodityCode.Text))
                    {
                        if (!CommonParameterSetCommon.IsExistCommodityCode(this.txtCommodityCode.Text))
                        {
                            ShowMessageBox.ShowInformation("代码已经存在!");
                            return;
                        }
                        if (InputTest.zeroXHAndQHStartIntTest(this.txtCommodityCode.Text))
                        {
                            if (this.txtCommodityCode.Text.Length == 6 || this.txtCommodityCode.Text.Length == 5)
                            {
                                cM_Commodity.CommodityCode = this.txtCommodityCode.Text;
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("代码长度是5或6!");
                                return;
                            }
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入期货代码简称+数字或6个数字!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入代码!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtCommodityName.Text))
                    {
                        if (!CommonParameterSetCommon.IsExistCommodityName(this.txtCommodityName.Text))
                        {
                            ShowMessageBox.ShowInformation("代码名称已经存在!");
                            return;
                        }
                        cM_Commodity.CommodityName = this.txtCommodityName.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入代码名称!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        cM_Commodity.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        //cM_Commodity.BreedClassID = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("请选择品种类型!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtLabelCommodityCode.Text))
                    {
                        cM_Commodity.LabelCommodityCode = this.txtLabelCommodityCode.Text;
                    }
                    else
                    {
                        cM_Commodity.LabelCommodityCode = AppGlobalVariable.INIT_STRING;
                    }

                    if (!string.IsNullOrEmpty(this.txtGoerScale.Text))
                    {
                        cM_Commodity.GoerScale = Convert.ToDecimal(this.txtGoerScale.Text);
                    }
                    else
                    {
                        cM_Commodity.GoerScale = AppGlobalVariable.INIT_DECIMAL;
                    }
                    //if (!string.IsNullOrEmpty(this.txtStockPinYin.Text))
                    //{
                    //cM_Commodity.StockPinYin = this.txtStockPinYin.Text;
                    //}
                    //else
                    //{
                    //    cM_Commodity.StockPinYin = AppGlobalVariable.INIT_STRING;
                    //    //ShowMessageBox.ShowInformation("拼音简称不能为空!");
                    //    //return;
                    //}

                    if (!string.IsNullOrEmpty(this.txtStockPinYin.Text))
                    {
                        if (m_DiffBreeadClassType)
                        {
                            cM_Commodity.StockPinYin = this.txtStockPinYin.Text;

                        }
                        else
                        {
                            cM_Commodity.StockPinYin = AppGlobalVariable.INIT_STRING;
                        }
                    }
                    else
                    {
                        if (m_DiffBreeadClassType)
                        {
                            ShowMessageBox.ShowInformation("拼音简称不能为空!");
                            return;
                        }
                        else
                        {
                            cM_Commodity.StockPinYin = AppGlobalVariable.INIT_STRING; //期货时此值为空

                        }
                    }

                    if (!string.IsNullOrEmpty(this.dtMarketDate.Text))
                    {
                        cM_Commodity.MarketDate = Convert.ToDateTime(this.dtMarketDate.Text);
                    }
                    else
                    {
                        //cM_Commodity.MarketDate = AppGlobalVariable.INIT_DATETIME;
                        ShowMessageBox.ShowInformation("请选择上市日期!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtturnovervolume.Text))
                    {
                        if (m_DiffBreeadClassType)
                        {
                            if (InputTest.intTest(this.txtturnovervolume.Text))
                            {
                                cM_Commodity.turnovervolume = Convert.ToDouble(this.txtturnovervolume.Text);
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("请输入正整数!");
                                return;
                            }
                        }
                        else
                        {
                            cM_Commodity.turnovervolume = null; //期货时此值为空
                        }
                    }
                    else
                    {
                        if (m_DiffBreeadClassType)
                        {
                            ShowMessageBox.ShowInformation("请输入流通股数!");
                            return;
                        }
                        else
                        {
                            cM_Commodity.turnovervolume = null; //期货时此值为空
                        }
                    }
                    cM_Commodity.IsExpired = AppGlobalVariable.INIT_INT; //默认int初始值
                    // cM_Commodity.ISSysDefaultCode = AppGlobalVariable.INIT_INT;//用户添加的代码不是系统默认代码
                    cM_Commodity.ISSysDefaultCode = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                    //用户添加的代码不是系统默认代码
                    m_Result = CommonParameterSetCommon.AddCMCommodity(cM_Commodity);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QueryCMCommodity();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-4201";
                    string errMsg = "添加交易商品失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                #endregion
            }
            else if (m_EditType == 2)
            {
                #region 修改商品操作

                try
                {
                    CM_Commodity cM_Commodity = new CM_Commodity();

                    if (string.IsNullOrEmpty(this.txtCommodityCode.Text))
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    cM_Commodity.CommodityCode = this.txtCommodityCode.Text;
                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        cM_Commodity.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        //cM_Commodity.BreedClassID = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("请选择品种类型!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(dtMarketDate.Text))
                    {
                        cM_Commodity.MarketDate = dtMarketDate.DateTime;
                    }
                    cM_Commodity.IsExpired = AppGlobalVariable.INIT_INT; //默认int初始值
                    //cM_Commodity.ISSysDefaultCode = AppGlobalVariable.INIT_INT;//用户添加的代码不是系统默认代码
                    cM_Commodity.ISSysDefaultCode = m_ISSysDefaultCode; //当是修改时不改变原来的系统默认代码默认值
                    m_Result = CommonParameterSetCommon.UpdateCMCommodity(cM_Commodity);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                    m_ISSysDefaultCode = AppGlobalVariable.INIT_INT;
                    this.QueryCMCommodity();
                }
                catch (Exception ex)
                {
                    string errCode = "GL-4203";
                    string errMsg = "修改交易商品失败!";
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