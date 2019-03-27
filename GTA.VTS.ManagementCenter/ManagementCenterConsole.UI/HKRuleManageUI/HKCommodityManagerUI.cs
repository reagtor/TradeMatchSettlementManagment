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
    /// 描述：港股交易商品管理窗体  错误编码范围:7700-7719
    /// 作者：刘书伟
    /// 日期：2009-10-23
    /// 修改：叶振东
    /// 日期：2010-04-02
    /// 描述：修改港股交易商品管理窗体按钮操作
    /// </summary>
    public partial class HKCommodityManagerUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public HKCommodityManagerUI()
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
        /// 港股商品代码
        /// </summary>
        private string m_HKCommodityCode = AppGlobalVariable.INIT_STRING;

        /// <summary>
        /// 是否是系统默认代码
        /// </summary>
        private int m_ISSysDefaultCode = AppGlobalVariable.INIT_INT;

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
            try
            {
                string strWhere = " BreedClassTypeID=4 and DeleteState is not null and DeleteState<>1 ";
                DataSet ds = CommonParameterSetCommon.GetList(strWhere); //GetHKBreedClassName();//从交易商品品种中获取港股品种名称
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
                string errCode = "GL-7707";
                string errMsg = "获取品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 根据查询条件，获取港股交易商品

        /// <summary>
        /// 根据查询条件，获取港股交易商品
        /// </summary>
        /// <returns></returns>
        private bool QueryHKCommodity()
        {
            try
            {
                //港股商品代码
                string HKCommodityCode = AppGlobalVariable.INIT_STRING;
                //港股商品名称
                string HKCommodityName = AppGlobalVariable.INIT_STRING;
                if (!string.IsNullOrEmpty(txtCondition.Text))
                {
                    HKCommodityCode = txtCondition.Text;
                    HKCommodityName = txtCondition.Text;
                }

                DataSet _dsHKCommodity = HKManageCommon.GetAllHKCommodity(HKCommodityCode, HKCommodityName,
                                                                                    m_pageNo,
                                                                                    m_pageSize,
                                                                                    out m_rowCount);
                DataTable _dtHKCommodity;
                if (_dsHKCommodity == null || _dsHKCommodity.Tables[0].Rows.Count == 0)
                {
                    _dtHKCommodity = new DataTable();
                }
                else
                {
                    _dtHKCommodity = _dsHKCommodity.Tables[0];
                }

                //绑定交易商品品种表中的品种ID对应的品种名称
                string strWhere = " BreedClassTypeID=4 or DeleteState=1 ";
                this.ddlBreedClassID.DataSource = CommonParameterSetCommon.GetList(strWhere).Tables[0];
                this.ddlBreedClassID.ValueMember =
                    CommonParameterSetCommon.GetList(strWhere).Tables[0].Columns["BreedClassID"].ToString();

                this.ddlBreedClassID.DisplayMember =
                    CommonParameterSetCommon.GetList(strWhere).Tables[0].Columns["BreedClassName"].ToString();

                this.gdHKCommodityResult.DataSource = _dtHKCommodity;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7701";
                string errMsg = "根据查询条件，获取港股交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region 修改港股交易商品

        /// <summary>
        /// 修改港股交易商品
        /// </summary>
        private void ModifyHKCommodity()
        {
            try
            {
                if (this.gdvHKCommoditySelect != null && this.gdvHKCommoditySelect.DataSource != null &&
                    this.gdvHKCommoditySelect.RowCount > 0 && this.gdvHKCommoditySelect.FocusedRowHandle >= 0)
                {
                    DataRow _dr = this.gdvHKCommoditySelect.GetDataRow(this.gdvHKCommoditySelect.FocusedRowHandle);
                    m_HKCommodityCode = _dr["HKCommodityCode"].ToString();
                    this.txtHKCommodityCode.Text = _dr["HKCommodityCode"].ToString();
                    this.txtHKCommodityName.Text = _dr["HKCommodityName"].ToString();
                    this.dtMarketDate.EditValue = Convert.ToDateTime(_dr["MarketDate"]);
                    this.txtStockPinYin.Text = _dr["StockPinYin"].ToString();
                    this.txtturnovervolume.Text = _dr["turnovervolume"].ToString();
                    this.txtPerHandThighOrShare.Text = _dr["PerHandThighOrShare"].ToString();
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
                    this.txtHKCommodityCode.Enabled = false;//港股代码不允许修改
                    this.txtHKCommodityName.Enabled = false;// 港股商品名称不允许修改
                    this.dtMarketDate.Enabled = false;// 上市日期不允许修改
                    this.txtStockPinYin.Enabled = false;// 股票简称的拼音不允许修改
                    this.txtturnovervolume.Enabled = false;// 流通股数不允许修改
                    this.txtPerHandThighOrShare.Enabled = false;// 每手股份数不允许修改
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7705";
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
            this.txtHKCommodityCode.Enabled = false;
            this.txtHKCommodityName.Enabled = false;
            this.dtMarketDate.Enabled = false;
            this.txtStockPinYin.Enabled = false;
            this.txtturnovervolume.Enabled = false;
            this.txtPerHandThighOrShare.Enabled = false;
            this.cmbBreedClassID.Enabled = false;

            this.btnOK.Enabled = false;
            this.btnAdd.Enabled = true;
            this.btnAdd.Text = "添加";
            this.btnModify.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnDelete.Enabled = true;
        }
        #endregion

        #region 启用界面中文本编辑和按钮
        /// <summary>
        /// 启用界面中文本编辑和按钮
        /// </summary>
        private void EnabledTrue()
        {
            this.txtHKCommodityCode.Enabled = true;
            this.txtHKCommodityName.Enabled = true;
            this.dtMarketDate.Enabled = true;
            this.txtStockPinYin.Enabled = true;
            this.txtturnovervolume.Enabled = true;
            this.txtPerHandThighOrShare.Enabled = true;
            this.cmbBreedClassID.Enabled = true;
            this.btnOK.Enabled = true;
            this.btnDelete.Enabled = false;
        }
        #endregion
        //================================  事件 ================================

        #region 港股交易商品管理窗体 HKCommodityManagerUI_Load
        /// <summary>
        ///港股交易商品管理窗体 HKCommodityManagerUI_Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HKCommodityManagerUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定品种标识(品种名称)
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindBreedClassName();
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdHKCommodityResult.DataSource = this.QueryHKCommodity();
                this.ShowDataPage();

                this.txtHKCommodityCode.Enabled = false;
                this.txtHKCommodityName.Enabled = false;
                this.dtMarketDate.Enabled = false;
                this.txtStockPinYin.Enabled = false;
                this.txtturnovervolume.Enabled = false;
                this.txtPerHandThighOrShare.Enabled = false;
                this.cmbBreedClassID.Enabled = false;
                this.cmbBreedClassID.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-7700";
                string errMsg = "港股交易商品管理窗体加载失败!";
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
                this.QueryHKCommodity();
            }
        }

        #endregion

        #region 查询港股交易商品

        /// <summary>
        /// 查询港股交易商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryHKCommodity();
                this.ShowDataPage(); //显示数据分页
                DataView _dvHKComm = (DataView)this.gdvHKCommoditySelect.DataSource;
                DataTable _dtHKCommodity = _dvHKComm.Table;
                if (_dtHKCommodity == null || _dtHKCommodity.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-7702";
                string errMsg = "查询港股交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加港股交易商品
        /// <summary>
        /// 添加港股交易商品
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
                this.txtHKCommodityCode.Text = string.Empty;
                this.txtHKCommodityName.Text = string.Empty;
                this.txtStockPinYin.Text = string.Empty;
                this.txtturnovervolume.Text = string.Empty;
                this.dtMarketDate.Text = string.Empty;
                this.txtPerHandThighOrShare.Text = string.Empty;
                this.btnModify.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnAdd.Text = "添加";
                this.ClearAll();
            }
        }
        #endregion

        #region  修改港股交易商品
        /// <summary>
        /// 修改港股交易商品
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
                this.cmbBreedClassID.Enabled = true;
                this.btnDelete.Enabled = false;
                this.btnOK.Enabled = true;
                this.btnAdd.Enabled = false;
            }
            else if (Name.Equals("取消"))
            {
                this.btnModify.Text = "修改";
                this.ClearAll();
            }
        }
        #endregion

        #region 修改港股交易商品的GridView的双击事件 gdHKCommodityResult_DoubleClick
        /// <summary>
        /// 修改港股交易商品的GridView的双击事件 gdHKCommodityResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdHKCommodityResult_DoubleClick(object sender, EventArgs e)
        {
            this.ModifyHKCommodity();
            this.ClearAll();
        }
        #endregion

        #region 删除港股交易商品
        /// <summary>
        /// 删除港股交易商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdvHKCommoditySelect.GetDataRow(this.gdvHKCommoditySelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["HKCommodityCode"])))
                {
                    m_HKCommodityCode = Convert.ToString(_dr["HKCommodityCode"]);
                }
                else
                {
                    m_HKCommodityCode = AppGlobalVariable.INIT_STRING;
                }
                //int breedClassID = Convert.ToInt32(_dr["BreedClassID"]);//品种ID
                if (m_HKCommodityCode != AppGlobalVariable.INIT_STRING)
                {
                    int _ISSysDefaultCode = Convert.ToInt32(_dr["ISSysDefaultCode"]);
                    if (_ISSysDefaultCode == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No)
                    {
                        m_Result = HKManageCommon.DeleteHKCommodity(m_HKCommodityCode);
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
                    m_HKCommodityCode = AppGlobalVariable.INIT_STRING;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryHKCommodity();
            }
            catch (Exception ex)
            {
                string errCode = "GL-7706";
                string errMsg = "删除港股交易商品失败!";
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

        /// <summary>
        /// 确定按钮操作
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
                    HK_Commodity hK_Commodity = new HK_Commodity();

                    if (!string.IsNullOrEmpty(this.txtHKCommodityCode.Text))
                    {
                        if (!HKManageCommon.IsExistHKCommodityCode(this.txtHKCommodityCode.Text))
                        {
                            ShowMessageBox.ShowInformation("代码已经存在!");
                            return;
                        }
                        if (InputTest.zeroStartIntTest(this.txtHKCommodityCode.Text))
                        {
                            if (this.txtHKCommodityCode.Text.Length == 5)
                            {
                                hK_Commodity.HKCommodityCode = this.txtHKCommodityCode.Text;
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("港股代码是五位数!");
                                return;
                            }

                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入数字!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请填写代码!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtHKCommodityName.Text))
                    {
                        if (!HKManageCommon.IsExistHKCommodityName(this.txtHKCommodityName.Text))
                        {
                            ShowMessageBox.ShowInformation("代码名称已经存在!");
                            return;
                        }
                        hK_Commodity.HKCommodityName = this.txtHKCommodityName.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请填写代码名称!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtStockPinYin.Text))
                    {
                        hK_Commodity.StockPinYin = this.txtStockPinYin.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("拼音简称不能为空!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        hK_Commodity.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        //cM_Commodity.BreedClassID = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("请选择品种类型!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.dtMarketDate.Text))
                    {
                        hK_Commodity.MarketDate = Convert.ToDateTime(this.dtMarketDate.Text);
                    }
                    else
                    {
                        //cM_Commodity.MarketDate = AppGlobalVariable.INIT_DATETIME;
                        ShowMessageBox.ShowInformation("请选择上市日期!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtturnovervolume.Text))
                    {
                        if (InputTest.intTest(this.txtturnovervolume.Text))
                        {
                            hK_Commodity.turnovervolume = Convert.ToDouble(this.txtturnovervolume.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入正整数!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("流通股数不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtPerHandThighOrShare.Text))
                    {
                        if (InputTest.intTest(this.txtPerHandThighOrShare.Text))
                        {
                            hK_Commodity.PerHandThighOrShare = Convert.ToInt32(this.txtPerHandThighOrShare.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入正整数!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("每手股数不能为空!");
                        return;
                    }
                    hK_Commodity.ISSysDefaultCode = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                    //用户添加的代码不是系统默认代码
                    m_Result = HKManageCommon.AddHKCommodity(hK_Commodity);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QueryHKCommodity();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-7703";
                    string errMsg = "添加港股交易商品失败!";
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
                    HK_Commodity hK_Commodity = new HK_Commodity();
                    if (string.IsNullOrEmpty(this.txtHKCommodityCode.Text))
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    hK_Commodity.HKCommodityCode = this.txtHKCommodityCode.Text;
                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        hK_Commodity.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    }
                    else
                    {
                        //cM_Commodity.BreedClassID = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("请选择品种类型!");
                        return;
                    }
                    //注释掉修改港股代码的系统默认代码值
                    //hK_Commodity.ISSysDefaultCode = AppGlobalVariable.INIT_INT;//用户添加的代码不是系统默认代码
                    hK_Commodity.ISSysDefaultCode = m_ISSysDefaultCode; //当是修改时不改变原来的系统默认代码默认值
                    m_Result = HKManageCommon.UpdateHKCommodity(hK_Commodity);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                    this.QueryHKCommodity();
                    //this.txtHKCommodityCode.Enabled = true;//操作结束代码变成可输入状态
                    m_ISSysDefaultCode = AppGlobalVariable.INIT_INT;
                    this.ClearAll();
                }
                catch (Exception ex)
                {
                    string errCode = "GL-7704";
                    string errMsg = "修改港股交易商品失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                #endregion
            }
        }


    }
}
