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
    /// 描述：现货单位换算管理窗体  错误编码范围:5700-5719
    /// 作者：刘书伟
    /// 日期：2008-12-26  修改：2009-08-01
    /// 修改：叶振东
    /// 日期：2010-04-02
    /// 描述：修改现货单位换算管理窗体操作的使用方法
    /// </summary>
    public partial class SpotUnitConversionManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public SpotUnitConversionManageUI()
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
        /// 现货单位换算ID
        /// </summary>
        private int m_UnitConversionID = AppGlobalVariable.INIT_INT;

        #region 操作类型　 1:添加,2:修改

        private int m_EditType = (int)UITypes.EditTypeEnum.AddUI;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get { return m_EditType; }
            set { m_EditType = value; }
        }

        #endregion

        #endregion

        //================================  私有  方法 ================================

        #region 获取现货品种名称 GetBindBreedClassName

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        private void GetBindBreedClassName()
        {
            DataSet ds = SpotManageCommon.GetBreedClassName(); //从交易商品品种表中获取
            UComboItem _item;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                _item = new UComboItem(ds.Tables[0].Rows[i]["BreedClassName"].ToString(),
                                       Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]));
                this.cmbBreedClassID.Properties.Items.Add(_item);
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

        #region 根据查询条件，获取现货单位换算

        /// <summary>
        /// 根据查询条件，获取现货单位换算
        /// </summary>
        /// <returns></returns>
        private bool QuerySpotUnitConversion()
        {
            try
            {
                string breedClassName = this.txtBreedClassName.Text;
                DataSet _dsSpotUnitConversion = SpotManageCommon.GetAllCMUnitConversion(breedClassName,
                                                                                        m_pageNo,
                                                                                        m_pageSize,
                                                                                        out m_rowCount);
                DataTable _dtSpotUnitConversion;
                if (_dsSpotUnitConversion == null || _dsSpotUnitConversion.Tables[0].Rows.Count == 0)
                {
                    _dtSpotUnitConversion = new DataTable();
                }
                else
                {
                    _dtSpotUnitConversion = _dsSpotUnitConversion.Tables[0];
                }

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetCMUnitConversionBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetCMUnitConversionBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetCMUnitConversionBreedClassName().Tables[0].Columns["BreedClassName"].ToString();

                //绑定从单位 From
                this.ddlUnitIDFrom.DataSource = BindData.GetBindListXHAboutUnit();
                this.ddlUnitIDFrom.ValueMember = "ValueIndex";
                this.ddlUnitIDFrom.DisplayMember = "TextTitleValue";
                //绑定到单位 To
                this.ddlUnitIDTo.DataSource = BindData.GetBindListXHAboutUnit();
                this.ddlUnitIDTo.ValueMember = "ValueIndex";
                this.ddlUnitIDTo.DisplayMember = "TextTitleValue";

                this.gdSpotUnitConversionRelult.DataSource = _dtSpotUnitConversion;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5704";
                string errMsg = "根据查询条件，获取现货单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
                // return false;
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
            this.txtValue.Text = string.Empty;
            this.cmbBreedClassID.Enabled = false;
            this.cmbUnitIDFrom.Enabled = false;
            this.cmbUnitIDTo.Enabled = false;
            this.txtValue.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnAdd.Enabled = true;
            this.btnAdd.Text = "添加";
            this.btnModify.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnDelete.Enabled = true;
        }

        #endregion

        #region 启动文本框编辑和确定按钮可用
        /// <summary>
        /// 启动文本框编辑和确定按钮可用
        /// </summary>
        private void EnabledTrue()
        {
            this.cmbBreedClassID.Enabled = true;
            this.cmbUnitIDFrom.Enabled = true;
            this.cmbUnitIDTo.Enabled = true;
            this.txtValue.Enabled = true;
            this.btnDelete.Enabled = false;
            this.btnOK.Enabled = true;
        }
        #endregion

        #region 获取需要修改的现货单位换算

        /// <summary>
        /// 获取需要修改的现货单位换算
        /// </summary>
        private void ModifyCMUnitConversion()
        {
            try
            {
                btnModify.Enabled = true;
                if (this.gdSpotUnitConversionSelect != null && this.gdSpotUnitConversionSelect.DataSource != null &&
                    this.gdSpotUnitConversionSelect.RowCount > 0 &&
                    this.gdSpotUnitConversionSelect.FocusedRowHandle >= 0)
                {
                    //  btnModify.Enabled = true;
                    DataRow _dr =
                        this.gdSpotUnitConversionSelect.GetDataRow(this.gdSpotUnitConversionSelect.FocusedRowHandle);
                    m_UnitConversionID = Convert.ToInt32(_dr["UnitConversionID"]);
                    foreach (object item in cmbBreedClassID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == int.Parse(_dr["BreedClassID"].ToString()))
                        {
                            cmbBreedClassID.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in cmbUnitIDFrom.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == int.Parse(_dr["UnitIDFrom"].ToString()))
                        {
                            cmbUnitIDFrom.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in cmbUnitIDTo.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == int.Parse(_dr["UnitIDTo"].ToString()))
                        {
                            cmbUnitIDTo.SelectedItem = item;
                            break;
                        }
                    }
                    txtValue.Text = Convert.ToString(_dr["Value"]);

                    this.cmbBreedClassID.Enabled = false;
                    this.cmbUnitIDFrom.Enabled = false;
                    this.cmbUnitIDTo.Enabled = false;
                    this.txtValue.Enabled = false;
                    this.btnOK.Enabled = false;
                    this.btnDelete.Enabled = true;
                    this.btnAdd.Text = "添加";
                    this.btnModify.Text = "修改";
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5706";
                string errMsg = "获取需要修改的现货单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 现货单位换算管理窗体 SpotUnitConversionManageUI_Load

        /// <summary>
        /// 现货单位换算管理窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpotUnitConversionManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //初次加载时，更新按钮禁用
                //btnModify.Enabled = false;
                //绑定交易规则表中的品种ID对应的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindBreedClassName(); //获取交易规则表中的品种ID对应的品种名称
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetCMUnitConversionBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetCMUnitConversionBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetCMUnitConversionBreedClassName().Tables[0].Columns["BreedClassName"].ToString();

                //绑定从单位 From
                this.cmbUnitIDFrom.Properties.Items.Clear();
                this.cmbUnitIDFrom.Properties.Items.AddRange(BindData.GetBindListXHAboutUnit());
                this.cmbUnitIDFrom.SelectedIndex = 0;

                //绑定到单位 To
                this.cmbUnitIDTo.Properties.Items.Clear();
                this.cmbUnitIDTo.Properties.Items.AddRange(BindData.GetBindListXHAboutUnit());
                this.cmbUnitIDTo.SelectedIndex = 0;

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdSpotUnitConversionRelult.DataSource = this.QuerySpotUnitConversion();
                this.ShowDataPage();

                //禁用文本框的编辑状态
                this.cmbBreedClassID.Enabled = false;
                this.cmbUnitIDFrom.Enabled = false;
                this.cmbUnitIDTo.Enabled = false;
                this.txtValue.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5700";
                string errMsg = "现货单位换算管理窗体加载失败!";
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
                this.QuerySpotUnitConversion();
            }
        }

        #endregion

        #region 查询现货单位换算 btnQuery_Click

        /// <summary>
        /// 查询现货单位换算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QuerySpotUnitConversion();
                this.ShowDataPage(); //显示数据分页
                //DataTable _dtSpotUnitConversion = (DataTable) this.gdSpotUnitConversionSelect.DataSource;
                DataView _dvSpotUnitC = (DataView)this.gdSpotUnitConversionSelect.DataSource;
                DataTable _dtSpotUnitConversion = _dvSpotUnitC.Table;
                if (_dtSpotUnitConversion == null || _dtSpotUnitConversion.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5705";
                string errMsg = "查询现货单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加现货单位换算 btnAdd_Click

        /// <summary>
        /// 添加现货单位换算
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
                this.EnabledTrue();
                this.txtValue.Text = string.Empty;
                this.btnModify.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnAdd.Text = "添加";
                this.ClearAll();
            }
        }

        #endregion

        #region 修改现货单位换算 btnModify_Click

        /// <summary>
        /// 修改现货单位换算
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
                this.EnabledTrue();
                this.btnAdd.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnModify.Text = "修改";
                this.ClearAll();
            }

        }

        #endregion

        #region 现货单位换算GridView的双击事件 gdSpotUnitConversionRelult_DoubleClick

        /// <summary>
        /// 现货单位换算GridView的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSpotUnitConversionRelult_DoubleClick(object sender, EventArgs e)
        {
            //btnAdd.Enabled = false;
            this.ModifyCMUnitConversion();
        }

        #endregion

        #region  删除现货单位换算 btnDelete_Click

        /// <summary>
        /// 删除现货单位换算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr =
                    this.gdSpotUnitConversionSelect.GetDataRow(this.gdSpotUnitConversionSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["UnitConversionID"])))
                {
                    m_UnitConversionID = Convert.ToInt32(_dr["UnitConversionID"]);
                }
                else
                {
                    m_UnitConversionID = AppGlobalVariable.INIT_INT;
                }
                if (m_UnitConversionID != AppGlobalVariable.INIT_INT)
                {
                    m_Result = SpotManageCommon.DeleteCMUnitConversion(m_UnitConversionID);
                }
                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_UnitConversionID = AppGlobalVariable.INIT_INT;

                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QuerySpotUnitConversion();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5702";
                string errMsg = "删除现货单位换算失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 现货单位换算数据输入检测
        /// <summary>
        /// 现货单位换算数据输入检测
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        private CM_UnitConversion VerifyUnitConversionInput(ref string msg)
        {
            try
            {
                msg = string.Empty;
                CM_UnitConversion cM_UnitConversion = new CM_UnitConversion();
                if (!string.IsNullOrEmpty(this.txtValue.Text))
                {
                    if (InputTest.intTest(this.txtValue.Text))
                    {
                        cM_UnitConversion.Value = Convert.ToInt32(this.txtValue.Text);
                    }
                    else
                    {
                        msg = "请输入数字且第一位数不能为0!";
                    }
                }
                else
                {
                    msg = "比例不能为空!";
                }
                cM_UnitConversion.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                cM_UnitConversion.UnitIDFrom = ((UComboItem)this.cmbUnitIDFrom.SelectedItem).ValueIndex;
                cM_UnitConversion.UnitIDTo = ((UComboItem)this.cmbUnitIDTo.SelectedItem).ValueIndex;
                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    cM_UnitConversion.UnitConversionID = m_UnitConversionID;
                }
                return cM_UnitConversion;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return null;
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
        ///  确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_EditType == 1)
            {
                #region 添加操作

                EditType = (int)UITypes.EditTypeEnum.AddUI;
                int result = AppGlobalVariable.INIT_INT;
                try
                {
                    CM_UnitConversion cM_UnitConversion = new CM_UnitConversion();
                    int breedClassID = AppGlobalVariable.INIT_INT; //品种ID
                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        breedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                        if (breedClassID != AppGlobalVariable.INIT_INT)
                        {
                            List<CM_UnitConversion> cMUnitConversionList =
                                SpotManageCommon.GetUnitConveByBreedClassID(breedClassID);
                            if (cMUnitConversionList.Count > 0)
                            {
                                int _curRow = 0; //当前记录行
                                foreach (CM_UnitConversion _UnitConversion in cMUnitConversionList)
                                {
                                    _curRow++;
                                    if (_UnitConversion.BreedClassID ==
                                        ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex)
                                    {
                                        if (_UnitConversion.UnitIDFrom ==
                                            ((UComboItem)this.cmbUnitIDFrom.SelectedItem).ValueIndex &&
                                            _UnitConversion.UnitIDTo ==
                                            ((UComboItem)this.cmbUnitIDTo.SelectedItem).ValueIndex)
                                        {
                                            ShowMessageBox.ShowInformation("同一品种，同一从单位到单位的转换只允许一条记录!");
                                            break;
                                        }
                                        else
                                        {
                                            if (_curRow == cMUnitConversionList.Count)
                                            {
                                                string msg = string.Empty;
                                                cM_UnitConversion = VerifyUnitConversionInput(ref msg);
                                                if (!string.IsNullOrEmpty(msg))
                                                {
                                                    ShowMessageBox.ShowInformation(msg);
                                                }
                                                else
                                                {
                                                    result = SpotManageCommon.AddCMUnitConversion(cM_UnitConversion);
                                                    if (result != AppGlobalVariable.INIT_INT)
                                                    {
                                                        ShowMessageBox.ShowInformation("添加成功!");
                                                        this.ClearAll();
                                                        this.QuerySpotUnitConversion();
                                                    }
                                                    else
                                                    {
                                                        ShowMessageBox.ShowInformation("添加失败!");
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                string msg = string.Empty;
                                cM_UnitConversion = VerifyUnitConversionInput(ref msg);
                                if (!string.IsNullOrEmpty(msg))
                                {
                                    ShowMessageBox.ShowInformation(msg);
                                }
                                else
                                {
                                    result = SpotManageCommon.AddCMUnitConversion(cM_UnitConversion);
                                    if (result != AppGlobalVariable.INIT_INT)
                                    {
                                        ShowMessageBox.ShowInformation("添加成功!");
                                        this.ClearAll();
                                        this.QuerySpotUnitConversion();
                                    }
                                    else
                                    {
                                        ShowMessageBox.ShowInformation("添加失败!");
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    string errCode = "GL-5701";
                    string errMsg = "添加现货单位换算失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                #endregion
            }
            else if (m_EditType==2)
            {
                #region 修改操作

                try
                {
                    EditType = (int) UITypes.EditTypeEnum.UpdateUI;
                    CM_UnitConversion cM_UnitConversion = new CM_UnitConversion();
                    int breedClassID = AppGlobalVariable.INIT_INT; //品种ID
                    if (m_UnitConversionID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        breedClassID = ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex;
                        if (breedClassID != AppGlobalVariable.INIT_INT)
                        {
                            List<CM_UnitConversion> cMUnitConversionList =
                                SpotManageCommon.GetUnitConveByBreedClassID(breedClassID);
                            if (cMUnitConversionList.Count > 0)
                            {
                                int _curRow = 0; //当前记录行
                                foreach (CM_UnitConversion _UnitConversion in cMUnitConversionList)
                                {
                                    if (m_UnitConversionID == _UnitConversion.UnitConversionID) //不与自己比较
                                    {
                                        _curRow++;
                                        if (_curRow == cMUnitConversionList.Count)
                                        {
                                            string msg = string.Empty;
                                            cM_UnitConversion = VerifyUnitConversionInput(ref msg);
                                            if (!string.IsNullOrEmpty(msg))
                                            {
                                                ShowMessageBox.ShowInformation(msg);
                                            }
                                            else
                                            {
                                                m_Result = SpotManageCommon.UpdateCMUnitConversion(cM_UnitConversion);
                                                if (m_Result)
                                                {
                                                    ShowMessageBox.ShowInformation("修改成功!");
                                                    this.ClearAll();
                                                    this.QuerySpotUnitConversion();
                                                    m_UnitConversionID = AppGlobalVariable.INIT_INT;
                                                }
                                                else
                                                {
                                                    ShowMessageBox.ShowInformation("修改失败!");
                                                }
                                            }
                                            break;
                                        }
                                        continue;
                                    }
                                    _curRow++;
                                    if (_UnitConversion.BreedClassID ==
                                        ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex)
                                    {
                                        if (_UnitConversion.UnitIDFrom ==
                                            ((UComboItem) this.cmbUnitIDFrom.SelectedItem).ValueIndex &&
                                            _UnitConversion.UnitIDTo ==
                                            ((UComboItem) this.cmbUnitIDTo.SelectedItem).ValueIndex)
                                        {
                                            ShowMessageBox.ShowInformation("同一品种，同一从单位到单位的转换只允许一条记录!");
                                            break;
                                        }
                                        else
                                        {
                                            if (_curRow == cMUnitConversionList.Count)
                                            {
                                                string msg = string.Empty;
                                                cM_UnitConversion = VerifyUnitConversionInput(ref msg);
                                                if (!string.IsNullOrEmpty(msg))
                                                {
                                                    ShowMessageBox.ShowInformation(msg);
                                                }
                                                else
                                                {
                                                    m_Result = SpotManageCommon.UpdateCMUnitConversion(cM_UnitConversion);
                                                    if (m_Result)
                                                    {
                                                        ShowMessageBox.ShowInformation("修改成功!");
                                                        this.ClearAll();
                                                        this.QuerySpotUnitConversion();
                                                        m_UnitConversionID = AppGlobalVariable.INIT_INT;
                                                    }
                                                    else
                                                    {
                                                        ShowMessageBox.ShowInformation("修改失败!");
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-5703";
                    string errMsg = "修改现货单位换算失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                #endregion
            }
            this.ClearAll();
        }
        #endregion
    }
}