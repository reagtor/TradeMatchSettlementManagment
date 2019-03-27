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
    /// 描述：商品期货_保证金管理窗体  错误编码范围:6440-6459
    /// 作者：刘书伟
    /// 日期：2008-12-06
    /// </summary>
    public partial class CFBailScaleValueManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public CFBailScaleValueManageUI()
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
        /// 商品期货-保证金比例标识
        /// </summary>
        private int m_CFBailScaleValueID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 当前gridView的行号
        /// </summary>
        private int m_CurRow = AppGlobalVariable.INIT_INT;

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

        #region 根据查询条件，获取商品期货_保证金比例

        /// <summary>
        /// 根据查询条件，获取商品期货_保证金比例
        /// </summary>
        /// <returns></returns>
        private bool QueryQHCFBailScaleValue()
        {
            try
            {
                string BreedClassName = this.txtBreedClassName.Text;
                int PositionBailTypeID = AppGlobalVariable.INIT_INT;
                int DeliveryMonthTypeID = AppGlobalVariable.INIT_INT;
                if (!string.IsNullOrEmpty(cmbPositionBailTypeID.Text))
                {
                    PositionBailTypeID = ((UComboItem)cmbPositionBailTypeID.SelectedItem).ValueIndex;
                }
                if (!string.IsNullOrEmpty(cmbDeliveryMonthTypeID.Text))
                {
                    ;
                    DeliveryMonthTypeID = ((UComboItem)cmbDeliveryMonthTypeID.SelectedItem).ValueIndex;
                }
                DataSet _dsQHCFBailScaleV = FuturesManageCommon.GetAllQHCFBailScaleValue(BreedClassName,
                                                                                         DeliveryMonthTypeID,
                                                                                         PositionBailTypeID,
                                                                                         m_pageNo,
                                                                                         m_pageSize,
                                                                                         out m_rowCount);
                DataTable _dtQHCFBailScaleV;
                if (_dsQHCFBailScaleV == null || _dsQHCFBailScaleV.Tables[0].Rows.Count == 0)
                {
                    _dtQHCFBailScaleV = new DataTable();
                }
                else
                {
                    _dtQHCFBailScaleV = _dsQHCFBailScaleV.Tables[0];

                    int _CFBailScaleValueID = 0;//商品期货保证金比例ID
                    for (int i = 0; i < _dtQHCFBailScaleV.Rows.Count; i++)
                    {
                        _CFBailScaleValueID = 0;
                        if (_dtQHCFBailScaleV.Rows[i][10] != System.DBNull.Value)
                        {
                            _CFBailScaleValueID = Convert.ToInt32(_dtQHCFBailScaleV.Rows[i][10]);
                        }

                        if (_CFBailScaleValueID != 0)
                        {
                            for (int j = 0; j < _dtQHCFBailScaleV.Rows.Count; j++)
                            {

                                if (Convert.ToInt32(_dtQHCFBailScaleV.Rows[j][1]) == _CFBailScaleValueID)
                                {
                                    _dtQHCFBailScaleV.Rows.RemoveAt(j);
                                    i++;
                                    break;
                                }
                            }
                        }
                    }
                }

                //绑定商品期货类型的品种名称
                this.ddlBreedClassID.DataSource = CommonParameterSetCommon.GetSpQhTypeBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    CommonParameterSetCommon.GetSpQhTypeBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    CommonParameterSetCommon.GetSpQhTypeBreedClassName().Tables[0].Columns["BreedClassName"].ToString();

                this.gdCFBailScaleValueResult.DataSource = _dtQHCFBailScaleV;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6445";
                string errMsg = "根据查询条件，获取商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }

            return true;
        }

        #endregion

        #region 获取需要更新的商品期货_保证金比例的实体

        /// <summary>
        /// 获取需要更新的商品期货_保证金比例的实体
        /// </summary>
        /// <param name="handle">行号</param>
        private void UpdateQHCFBailScaleValue(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                //显示添加 商品期货_保证金比例窗体
                AddCFBailScaleValueUI addCFBailScaleValueUI = new AddCFBailScaleValueUI();
                addCFBailScaleValueUI.EditType = (int)UITypes.EditTypeEnum.UpdateUI;
                DataRow _dr = this.gdCFBailScaleValueSelect.GetDataRow(handle);
                int cFBailScaleValueID = Convert.ToInt32(_dr["CFBailScaleValueID"]);
                QH_CFBailScaleValue qH_CFBailScaleValue = FuturesManageCommon.GetQHCFBailScaleValueModel(cFBailScaleValueID);
                addCFBailScaleValueUI.QHCFBailScaleValue = qH_CFBailScaleValue;

                if (addCFBailScaleValueUI.ShowDialog(this) == DialogResult.OK)
                {
                    this.QueryQHCFBailScaleValue();
                    this.gdCFBailScaleValueSelect.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6443";
                string errMsg = "获取需要更新的商品期货_保证金比例的实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 商品期货_保证金管理窗体 CFSingleValBailScaleManageUI_Load

        /// <summary>
        /// 商品期货_保证金管理窗体 CFSingleValBailScaleManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFSingleValBailScaleManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定查询条件交割月份类型
                this.cmbDeliveryMonthTypeID.Properties.Items.Clear();
                this.cmbDeliveryMonthTypeID.Properties.Items.AddRange(BindData.GetBindListQHCFPositionMonthType());
                //this.cmbDeliveryMonthTypeID.SelectedIndex = 0;

                //绑定查询条件持仓控制类型
                this.cmbPositionBailTypeID.Properties.Items.Clear();
                this.cmbPositionBailTypeID.Properties.Items.AddRange(BindData.GetBindListQHPositionBailType());
                // this.cmbPositionBailTypeID.SelectedIndex = 0;

                //绑定上限是否相等
                this.ddlUpperLimitIfEquation.DataSource = BindData.GetBindListYesOrNo();
                this.ddlUpperLimitIfEquation.ValueMember = "ValueIndex";
                this.ddlUpperLimitIfEquation.DisplayMember = "TextTitleValue";

                //绑定下限是否相等
                this.ddlLowerLimitIfEquation.DataSource = BindData.GetBindListYesOrNo();
                this.ddlLowerLimitIfEquation.ValueMember = "ValueIndex";
                this.ddlLowerLimitIfEquation.DisplayMember = "TextTitleValue";

                //绑定交割月份类型
                this.ddlDeliveryMonthType.DataSource = BindData.GetBindListQHCFPositionMonthType();
                this.ddlDeliveryMonthType.ValueMember = "ValueIndex";
                this.ddlDeliveryMonthType.DisplayMember = "TextTitleValue";

                //绑定持仓控制类型
                this.ddlPositionBailTypeID.DataSource = BindData.GetBindListQHPositionBailType();
                this.ddlPositionBailTypeID.ValueMember = "ValueIndex";
                this.ddlPositionBailTypeID.DisplayMember = "TextTitleValue";

                //#region 绑定保证金设置类型 add by 董鹏 2010-01-21
                //this.ddlScaleSetType.DataSource = BindData.GetBindListQHCFScaleSetType();
                //this.ddlScaleSetType.ValueMember = "ValueIndex";
                //this.ddlScaleSetType.DisplayMember = "TextTitleValue";
                //#endregion

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdCFBailScaleValueResult.DataSource = this.QueryQHCFBailScaleValue();
                this.ShowDataPage();

            }
            catch (Exception ex)
            {
                string errCode = "GL-6440";
                string errMsg = "商品期货_保证金管理窗体加载失败!";
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
                this.QueryQHCFBailScaleValue();
            }
        }

        #endregion

        #region 添加商品期货_保证金比例

        /// <summary>
        /// 添加 商品期货_保证金比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //显示添加 商品期货_保证金比例窗体
                AddCFBailScaleValueUI addCFBailScaleValueUI = new AddCFBailScaleValueUI();
                addCFBailScaleValueUI.OnSaved += new EventHandler(addCFBailScaleValueUI_OnSaved);
                addCFBailScaleValueUI.ShowDialog();
                //this.QueryQHCFBailScaleValue();

            }
            catch (Exception ex)
            {
                string errCode = "GL-6441";
                string errMsg = "添加 商品期货_保证金比例窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        void addCFBailScaleValueUI_OnSaved(object sender, EventArgs e)
        {
            this.QueryQHCFBailScaleValue();
        }

        #endregion

        #region 修改商品期货_保证金比例

        /// <summary>
        /// 修改 商品期货_保证金比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gdCFBailScaleValueSelect != null && this.gdCFBailScaleValueSelect.DataSource != null &&
                    this.gdCFBailScaleValueSelect.RowCount > 0 && this.gdCFBailScaleValueSelect.FocusedRowHandle >= 0)
                {
                    m_CurRow = this.gdCFBailScaleValueSelect.FocusedRowHandle;
                    if (m_CurRow != AppGlobalVariable.INIT_INT)
                    {
                        this.UpdateQHCFBailScaleValue(m_CurRow);
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择记录!");
                }
                this.QueryQHCFBailScaleValue();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6442";
                string errMsg = "修改 商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 查询商品期货_保证金比例

        /// <summary>
        /// 查询商品期货_保证金比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1;
                this.QueryQHCFBailScaleValue();
                this.ShowDataPage();
                // DataTable _dtQHCFBailScaleV = (DataTable) this.gdCFBailScaleValueSelect.DataSource;
                DataView _dvQHCFBailS = (DataView)this.gdCFBailScaleValueSelect.DataSource;
                DataTable _dtQHCFBailScaleV = _dvQHCFBailS.Table;
                if (_dtQHCFBailScaleV == null || _dtQHCFBailScaleV.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6444";
                string errMsg = "查询商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 删除商品期货_保证金比例

        /// <summary>
        /// 删除商品期货_保证金比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;

                DataRow _dr = this.gdCFBailScaleValueSelect.GetDataRow(this.gdCFBailScaleValueSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(_dr["CFBailScaleValueID"])))
                {
                    m_CFBailScaleValueID = Convert.ToInt32(_dr["CFBailScaleValueID"]);
                }
                else
                {
                    m_CFBailScaleValueID = AppGlobalVariable.INIT_INT;
                }

                if (m_CFBailScaleValueID != AppGlobalVariable.INIT_INT)
                {
                    QH_CFBailScaleValue qH_CFBailScaleVal = FuturesManageCommon.GetQHCFBailScaleValueModel(m_CFBailScaleValueID);
                    //当子ID的数据存在时，则删除子数据，无论子数据是否删除成功，都继续执行删除当前的记录
                    if (qH_CFBailScaleVal.RelationScaleID != AppGlobalVariable.INIT_INT ||
                        string.IsNullOrEmpty(qH_CFBailScaleVal.RelationScaleID.Value.ToString()))
                    {
                        FuturesManageCommon.DeleteQHCFBailScaleValue(Convert.ToInt32(qH_CFBailScaleVal.RelationScaleID));
                    }
                    m_Result = FuturesManageCommon.DeleteQHCFBailScaleValue(m_CFBailScaleValueID);
                }

                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_CFBailScaleValueID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryQHCFBailScaleValue();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6446";
                string errMsg = "删除商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 修改商品期货_保证金比例GridView的双击事件 gdCFBailScaleValueResult_DoubleClick

        /// <summary>
        /// 修改商品期货_保证金比例GridView的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdCFBailScaleValueResult_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.gdCFBailScaleValueSelect.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

                if (this.gdCFBailScaleValueSelect != null && this.gdCFBailScaleValueSelect.FocusedRowHandle >= 0 &&
                    hi.RowHandle >= 0)
                {
                    m_CurRow = this.gdCFBailScaleValueSelect.FocusedRowHandle;
                    this.UpdateQHCFBailScaleValue(m_CurRow);
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选中记录行!");
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }

        #endregion

        private void btnMinScale_Click(object sender, EventArgs e)
        {
            try
            {
                //显示添加 商品期货_保证金比例窗体
                AddCFMinScaleValueUI addCFMinScaleValueUI = new AddCFMinScaleValueUI();
                addCFMinScaleValueUI.OnSaved += new EventHandler(addCFBailScaleValueUI_OnSaved);
                addCFMinScaleValueUI.ShowDialog();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6441";
                string errMsg = "添加 商品期货_最低保证金比例窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
    }
}