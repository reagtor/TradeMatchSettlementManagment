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
    /// 描述：交易商品品种管理窗体  错误编码范围:4000-4020
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// 修改：叶振东
    /// 日期：2010-04-02
    /// 描述：修改交易商品品种管理窗体中交易按钮事件
    /// </summary>
    public partial class BreedClassManagerUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public BreedClassManagerUI()
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
        /// 交易商品品种ID
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result;

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
                    UCPageNavig.PageCount = 0;
                }
                else
                {
                    if (m_rowCount % m_pageSize == 0)
                    {
                        UCPageNavig.PageCount = Convert.ToInt32(m_rowCount / m_pageSize);
                    }
                    else
                    {
                        UCPageNavig.PageCount = Convert.ToInt32(m_rowCount / m_pageSize) + 1;
                    }
                }

                UCPageNavig.CurrentPage = m_pageNo;
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
            cmbBourseType.Properties.Items.Clear();
            cmbBourseTypeIDQ.Properties.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                _item = new UComboItem(ds.Tables[0].Rows[i]["BourseTypeName"].ToString(),
                                       Convert.ToInt32(ds.Tables[0].Rows[i]["BourseTypeID"]));
                cmbBourseType.Properties.Items.Add(_item);
                cmbBourseTypeIDQ.Properties.Items.Add(_item);
            }
        }

        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            //组合框的值全清空?
            //this.txtBreedClassName.Text = string.Empty;
            //this.cmbBourseTypeID.Text = string.Empty;
            //this.tmTradeStartTime.EditValue = "00:00";
            //this.tmTradeEndTime.EditValue = "00:00";
            this.txtBreedClassName.Enabled = false;
            this.cmbAccountTypeIDFund.Enabled = false;
            this.cmbBreedClassType.Enabled = false;
            this.cmbAccountTypeIDHold.Enabled = false;
            this.cmbBourseType.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnAdd.Enabled = true;
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnAdd.Text = "添加";
            this.btnModify.Text = "修改";
        }

        #endregion

        #region 根据查询条件，获取交易商品品种

        /// <summary>
        /// 根据查询条件，获取交易商品品种
        /// </summary>
        /// <returns></returns>
        private bool QueryCMBreedClass()
        {
            try
            {
                int BourseTypeID = AppGlobalVariable.INIT_INT;
                int BreedClassTypeID = AppGlobalVariable.INIT_INT;
                if (!string.IsNullOrEmpty(cmbBourseTypeIDQ.Text))
                {
                    BourseTypeID = ((UComboItem)cmbBourseTypeIDQ.SelectedItem).ValueIndex;
                }
                if (!string.IsNullOrEmpty(cmbBreedClassTypeIDQ.Text))
                {
                    BreedClassTypeID = ((UComboItem)cmbBreedClassTypeIDQ.SelectedItem).ValueIndex;
                }
                DataSet _dsCMBreedClass = CommonParameterSetCommon.GetAllCMBreedClass(BreedClassTypeID, BourseTypeID,
                                                                                      m_pageNo,
                                                                                      m_pageSize,
                                                                                      out m_rowCount);
                DataTable _dtCMBreedClass;
                if (_dsCMBreedClass == null || _dsCMBreedClass.Tables[0].Rows.Count == 0)
                {
                    _dtCMBreedClass = new DataTable();
                }
                else
                {
                    _dtCMBreedClass = _dsCMBreedClass.Tables[0];
                }

                //绑定交易商品品种的交易所类型ID对应的交易所名称
                ddlBourseTypeID.DataSource = CommonParameterSetCommon.GetCMBreedClassBourseTypeName().Tables[0];
                ddlBourseTypeID.ValueMember =
                    CommonParameterSetCommon.GetCMBreedClassBourseTypeName().Tables[0].Columns["BourseTypeID"].
                        ToString();
                ddlBourseTypeID.DisplayMember =
                    CommonParameterSetCommon.GetCMBreedClassBourseTypeName().Tables[0].Columns["BourseTypeName"].
                        ToString();
                //绑定品种类型
                ddlBreedClassTypeID.DataSource = BindData.GetBindListBreedClassType();
                ddlBreedClassTypeID.ValueMember = "ValueIndex";
                ddlBreedClassTypeID.DisplayMember = "TextTitleValue";

                //资金账号类型
                ddlAccountTypeIDFund.DataSource = ComboBoxDataSource.GetAccountTypeList(1);
                ddlAccountTypeIDFund.ValueMember = "ValueIndex";
                ddlAccountTypeIDFund.DisplayMember = "TextTitleValue";
                //持仓账号类型
                ddlAccountTypeIDHold.DataSource = ComboBoxDataSource.GetAccountTypeList(2);
                ddlAccountTypeIDHold.ValueMember = "ValueIndex";
                ddlAccountTypeIDHold.DisplayMember = "TextTitleValue";


                gdBreedClassResult.DataSource = _dtCMBreedClass;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4004";
                string errMsg = "数据不存在，获取交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                //throw exception;
                return false;
            }
            return true;
        }

        #endregion

        #region 修改交易商品品种

        /// <summary>
        /// 修改交易商品品种
        /// </summary>
        private void ModifyCMBreedClass()
        {
            try
            {
                //btnModify.Enabled = true;
                if (this.gdvBreedClassSelect != null && this.gdvBreedClassSelect.DataSource != null &&
                    this.gdvBreedClassSelect.RowCount > 0 && this.gdvBreedClassSelect.FocusedRowHandle >= 0)
                {
                    //btnModify.Enabled = true;
                    DataRow _dr = this.gdvBreedClassSelect.GetDataRow(this.gdvBreedClassSelect.FocusedRowHandle);
                    m_BreedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                    this.txtBreedClassName.Text = _dr["BreedClassName"].ToString();

                    foreach (object item in this.cmbBreedClassType.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BreedClassTypeID"]))
                        {
                            this.cmbBreedClassType.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in this.cmbBourseType.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BourseTypeID"]))
                        {
                            this.cmbBourseType.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in this.cmbAccountTypeIDFund.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["AccountTypeIDFund"]))
                        {
                            this.cmbAccountTypeIDFund.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in this.cmbAccountTypeIDHold.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["AccountTypeIDHold"]))
                        {
                            this.cmbAccountTypeIDHold.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4006";
                string errMsg = "获取需要修改的交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 交易商品品种管理窗体 BreedClassManagerUI_Load

        /// <summary>
        /// 交易商品品种管理窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BreedClassManagerUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定交易所类型
                cmbBourseType.Properties.Items.Clear();
                GetBindBourseTypeName();
                cmbBourseType.SelectedIndex = 0;

                //绑定查询条件交易所类型
                //cmbBourseTypeIDQ.Text = "";
                cmbBourseTypeIDQ.Properties.Items.Clear();
                GetBindBourseTypeName();
                //cmbBourseTypeIDQ.SelectedIndex = 0;

                //绑定品种类型
                cmbBreedClassType.Properties.Items.Clear();
                cmbBreedClassType.Properties.Items.AddRange(BindData.GetBindListBreedClassType());
                cmbBreedClassType.SelectedIndex = 0;

                //绑定查询条件品种类型
                //cmbBreedClassTypeIDQ.Text = "";
                cmbBreedClassTypeIDQ.Properties.Items.Clear();
                cmbBreedClassTypeIDQ.Properties.Items.AddRange(BindData.GetBindListBreedClassType());
                //cmbBreedClassTypeIDQ.SelectedIndex = 0;

                //资金账号类型
                cmbAccountTypeIDFund.Properties.Items.Clear();
                cmbAccountTypeIDFund.Properties.Items.AddRange(ComboBoxDataSource.GetAccountTypeList(1));
                cmbAccountTypeIDFund.SelectedIndex = 0;

                //持仓账号类型
                cmbAccountTypeIDHold.Properties.Items.Clear();
                cmbAccountTypeIDHold.Properties.Items.AddRange(ComboBoxDataSource.GetAccountTypeList(2));
                cmbAccountTypeIDHold.SelectedIndex = 0;

                //绑定查询结果
                m_pageNo = 1;
                gdBreedClassResult.DataSource = QueryCMBreedClass();
                ShowDataPage();

                //设置文本框和按钮的状态
                this.txtBreedClassName.Enabled = false;
                this.cmbAccountTypeIDFund.Enabled = false;
                this.cmbBreedClassType.Enabled = false;
                this.cmbAccountTypeIDHold.Enabled = false;
                this.cmbBourseType.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4000";
                string errMsg = "交易商品品种管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 添加交易商品品种

        /// <summary>
        /// 添加交易商品品种
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
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
                this.EnabledTrue();
                this.txtBreedClassName.Text = string.Empty;
            }
            else if (Name.Equals("取消"))
            {
                this.btnAdd.Text = "添加";
                this.ClearAll();
            }
        }

        #endregion
        private void EnabledTrue()
        {
            this.txtBreedClassName.Enabled = true;
            this.cmbAccountTypeIDFund.Enabled = true;
            this.cmbBreedClassType.Enabled = true;
            this.cmbAccountTypeIDHold.Enabled = true;
            this.cmbBourseType.Enabled = true;
            this.btnOK.Enabled = true;
        }
        #region 修改交易商品品种

        /// <summary>
        /// 修改交易商品品种
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
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
                this.EnabledTrue();
            }
            else if (Name.Equals("取消"))
            {
                this.btnModify.Text = "修改";
                this.ClearAll();
            }
        }

        #endregion

        #region 删除交易商品品种

        /// <summary>
        /// 删除交易商品品种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdvBreedClassSelect.GetDataRow(this.gdvBreedClassSelect.FocusedRowHandle);
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
                    //m_Result = CommonParameterSetCommon.DeleteCMBreedClass(m_BreedClassID);
                    int _ISSysDefaultBreed = Convert.ToInt32(_dr["ISSysDefaultBreed"]);
                    if (_ISSysDefaultBreed == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No)
                    {
                        m_Result = CommonParameterSetCommon.DeleteCMBreedClassALLAbout(m_BreedClassID);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("系统默认品种不能删除!");
                        return;
                    }
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
                this.QueryCMBreedClass();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4002";
                string errMsg = "删除交易商品品种失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return;
            }
        }

        #endregion

        #region 查询交易商品品种

        /// <summary>
        /// 查询交易商品品种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                m_pageNo = 1; //设当前页是第一页
                QueryCMBreedClass();
                ShowDataPage(); //显示数据分页
                //var _dtCMBreedClass = (DataTable) gdvBreedClassSelect.DataSource;
                DataView _dvCMBreedC = (DataView)gdvBreedClassSelect.DataSource;
                var _dtCMBreedClass = _dvCMBreedC.Table;
                if (_dtCMBreedClass == null || _dtCMBreedClass.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4005";
                string errMsg = "数据不存在，查询交易商品品种失败!";
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
                m_pageNo = page;
                QueryCMBreedClass();
            }
        }

        #endregion

        #region  交易商品品种GridView的双击事件 gdBreedClassResult_DoubleClick

        /// <summary>
        /// 交易商品品种GridView的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdBreedClassResult_DoubleClick(object sender, EventArgs e)
        {
            //btnAdd.Enabled = false;
            this.ModifyCMBreedClass();
            this.ClearAll();
        }

        #endregion

        #region  显示添加交易商品UI

        /// <summary>
        /// 显示添加交易商品UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCommodity_Click(object sender, EventArgs e)
        {
            try
            {
                CommodityManagerUI commodityManagerUI = new CommodityManagerUI();
                commodityManagerUI.ShowDialog();
            }
            catch
            {
                return;
            }
        }

        #endregion

        #region 取消按钮事件 btnCancel_Click
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }
        #endregion

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //通过判断状态来执行不同的操作 1：添加  2：修改
            if (m_EditType == 1)
            {
                #region 添加

                try
                {
                    CM_BreedClass cM_BreedClas = new CM_BreedClass();
                    if (!string.IsNullOrEmpty(txtBreedClassName.Text))
                    {
                        if (!CommonParameterSetCommon.IsExistBreedClassName(txtBreedClassName.Text))
                        {
                            ShowMessageBox.ShowInformation("品种名称已存在!");
                            return;
                        }
                        cM_BreedClas.BreedClassName = txtBreedClassName.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("品种名称不能为空!");
                        return;
                    }

                    int _BreedClassMaxId = CommonParameterSetCommon.GetCMBreedClassMaxId();
                    if (_BreedClassMaxId != AppGlobalVariable.INIT_INT)
                    {
                        if (_BreedClassMaxId > 1500) //交易商品品种表中的最大ID大于系统默认的ID，1500时
                        {
                            cM_BreedClas.BreedClassID = _BreedClassMaxId; //因为在DAL层返回的最大ID已经加1
                        }
                        else
                        {
                            cM_BreedClas.BreedClassID = 1500 + 1;
                        }
                    }
                    if (!string.IsNullOrEmpty(cmbBourseType.Text))
                    {
                        cM_BreedClas.BourseTypeID = ((UComboItem)cmbBourseType.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        cM_BreedClas.BourseTypeID = AppGlobalVariable.INIT_INT;
                    }

                    if (!string.IsNullOrEmpty(cmbBreedClassType.Text))
                    {
                        cM_BreedClas.BreedClassTypeID = ((UComboItem)cmbBreedClassType.SelectedItem).ValueIndex;
                        ;
                    }
                    else
                    {
                        cM_BreedClas.BreedClassTypeID = AppGlobalVariable.INIT_INT;
                    }

                    if (!string.IsNullOrEmpty(cmbAccountTypeIDFund.Text))
                    {
                        cM_BreedClas.AccountTypeIDFund =
                            ((UComboItem)cmbAccountTypeIDFund.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        cM_BreedClas.AccountTypeIDFund = AppGlobalVariable.INIT_INT;
                    }
                    if (!string.IsNullOrEmpty(cmbAccountTypeIDHold.Text))
                    {
                        cM_BreedClas.AccountTypeIDHold =
                            ((UComboItem)cmbAccountTypeIDHold.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        cM_BreedClas.AccountTypeIDHold = AppGlobalVariable.INIT_INT;
                    }
                    cM_BreedClas.ISSysDefaultBreed = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                    //用户添加的代码不是系统默认代码
                    if (cM_BreedClas.BreedClassTypeID ==
                        (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.HKStock)
                    {
                        cM_BreedClas.ISHKBreedClassType = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes;
                        //当品种类型是港股时，标识为港股
                    }
                    else
                    {
                        cM_BreedClas.ISHKBreedClassType = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                        //当其它品种类型是时，标识为否

                    }
                    bool result = CommonParameterSetCommon.AddCMBreedClass(cM_BreedClas);
                    if (result) // != AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        ClearAll();
                        this.QueryCMBreedClass();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-4001";
                    string errMsg = "添加交易商品品种失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                #endregion
            }
            else if (m_EditType == 2)
            {
                #region 修改

                try
                {
                    CM_BreedClass cM_BreedClas = new CM_BreedClass();
                    if (m_BreedClassID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    cM_BreedClas.BreedClassID = m_BreedClassID;

                    if (!string.IsNullOrEmpty(txtBreedClassName.Text))
                    {
                        cM_BreedClas.BreedClassName = txtBreedClassName.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("品种名称不能为空!");
                        return;
                    }
                    cM_BreedClas.BourseTypeID = ((UComboItem)this.cmbBourseType.SelectedItem).ValueIndex;
                    cM_BreedClas.BreedClassTypeID = ((UComboItem)this.cmbBreedClassType.SelectedItem).ValueIndex;
                    cM_BreedClas.AccountTypeIDFund = ((UComboItem)this.cmbAccountTypeIDFund.SelectedItem).ValueIndex;
                    cM_BreedClas.AccountTypeIDHold = ((UComboItem)this.cmbAccountTypeIDHold.SelectedItem).ValueIndex;

                    m_Result = CommonParameterSetCommon.UpdateCMBreedClass(cM_BreedClas);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                    this.QueryCMBreedClass();
                }
                catch (Exception ex)
                {
                    string errCode = "GL-4003";
                    string errMsg = "修改交易商品品种失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                #endregion
            }
        }

    }
}