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
    /// 描述：商品期货持仓限制管理窗体 错误编码范围:6460-6479
    /// 作者：刘书伟
    /// 日期：2008-12-06
    /// </summary>
    public partial class PositionLimitValueManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public PositionLimitValueManageUI()
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
        /// 期货-持仓限制ID
        /// </summary>
        private int m_PositionLimitValueID = AppGlobalVariable.INIT_INT;

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

        #region 根据查询条件，获取(商品)期货_持仓限制

        /// <summary>
        /// 根据查询条件，获取(商品)期货_持仓限制
        /// </summary>
        /// <returns></returns>
        private bool QueryQHPositionLimitValue()
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
                    DeliveryMonthTypeID = ((UComboItem)cmbDeliveryMonthTypeID.SelectedItem).ValueIndex;
                }
                DataSet _dsQHPositionLimitV = FuturesManageCommon.GetAllQHPositionLimitValue(BreedClassName, DeliveryMonthTypeID, PositionBailTypeID,
                                                                                  m_pageNo,
                                                                                  m_pageSize,
                                                                                  out m_rowCount);
                DataTable _dtQHPositionLimitV;
                if (_dsQHPositionLimitV == null || _dsQHPositionLimitV.Tables[0].Rows.Count == 0)
                {
                    _dtQHPositionLimitV = new DataTable();
                }
                else
                {
                    _dtQHPositionLimitV = _dsQHPositionLimitV.Tables[0];
                }

               //绑定商品期货类型的品种名称
                this.ddlBreedClassID.DataSource = CommonParameterSetCommon.GetSpQhTypeBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    CommonParameterSetCommon.GetSpQhTypeBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    CommonParameterSetCommon.GetSpQhTypeBreedClassName().Tables[0].Columns["BreedClassName"].ToString();

                this.gdPositionLimitValueResult.DataSource = _dtQHPositionLimitV;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6465";
                string errMsg = "根据查询条件，获取(商品)期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region 获取需要更新的(商品)期货_持仓限制的实体
        /// <summary>
        /// 获取需要更新的(商品)期货_持仓限制的实体
        /// </summary>
        /// <param name="handle">行号</param>
        private void UpdateQHPositionLimitValue(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                //显示添加（商品）期货_持仓限制窗体
                AddPositionLimitValueUI addPositionLimitValueUI = new AddPositionLimitValueUI();
                addPositionLimitValueUI.EditType = (int)UITypes.EditTypeEnum.UpdateUI;
                DataRow _dr = this.gdPositionLimitValueSelect.GetDataRow(handle);
                int positionLimitValueID = Convert.ToInt32(_dr["PositionLimitValueID"]);
                QH_PositionLimitValue qHPositionLimitValue = FuturesManageCommon.GetQHPositionLimitValueModel(positionLimitValueID);
                addPositionLimitValueUI.QHPositionLimitValue = qHPositionLimitValue;

                if (addPositionLimitValueUI.ShowDialog(this) == DialogResult.OK)
                {
                    this.QueryQHPositionLimitValue();
                    this.gdPositionLimitValueSelect.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6463";
                string errMsg = "获取需要更新的(商品)期货_持仓限制的实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        //================================  事件 ================================

        #region 商品期货持仓限制管理窗体 PositionRangeValManageUI_Load
        /// <summary>
        /// 商品期货持仓限制管理窗体 PositionRangeValManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionRangeValManageUI_Load(object sender, EventArgs e)
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
                this.ddlDeliveryMonthTypeID.DataSource = BindData.GetBindListQHCFPositionMonthType();
                this.ddlDeliveryMonthTypeID.ValueMember = "ValueIndex";
                this.ddlDeliveryMonthTypeID.DisplayMember = "TextTitleValue";

                //绑定持仓控制类型
                this.ddlPositionBailTypeID.DataSource = BindData.GetBindListQHPositionBailType();
                this.ddlPositionBailTypeID.ValueMember = "ValueIndex";
                this.ddlPositionBailTypeID.DisplayMember = "TextTitleValue";

                //绑定持仓取值类型
                this.ddlPositionValueTypeID.DataSource = BindData.GetBindListQHPositionValueType();
                this.ddlPositionValueTypeID.ValueMember = "ValueIndex";
                this.ddlPositionValueTypeID.DisplayMember = "TextTitleValue";

                //#region 绑定持仓类型 add by 董鹏 2010-01-21
                //this.ddlPositionLimitType.DataSource = BindData.GetBindListQHPositionLimitType();
                //this.ddlPositionLimitType.ValueMember = "ValueIndex";
                //this.ddlPositionLimitType.DisplayMember = "TextTitleValue";
                //#endregion

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdPositionLimitValueResult.DataSource = this.QueryQHPositionLimitValue();
                this.ShowDataPage();

            }
            catch (Exception ex)
            {
                string errCode = "GL-6460";
                string errMsg = "商品期货持仓限制管理窗体加载失败!";
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
                this.QueryQHPositionLimitValue();
            }
        }
        #endregion

        #region 添加（商品）期货_持仓限制
        /// <summary>
        ///添加（商品）期货_持仓限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //显示添加（商品）期货_持仓限制窗体
                AddPositionLimitValueUI addPositionLimitValueUI = new AddPositionLimitValueUI();
                addPositionLimitValueUI.OnSaved += new EventHandler(addPositionLimitValueUI_OnSaved);
                addPositionLimitValueUI.ShowDialog();
                

            }
            catch (Exception ex) 
            {
                string errCode = "GL-6461";
                string errMsg = "添加（商品）期货_持仓限制窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        void addPositionLimitValueUI_OnSaved(object sender, EventArgs e)
        {
            this.QueryQHPositionLimitValue();
        }

        #endregion

        #region 修改（商品）期货_持仓限制
        /// <summary>
        /// 修改（商品）期货_持仓限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gdPositionLimitValueSelect != null && this.gdPositionLimitValueSelect.DataSource != null &&
                    this.gdPositionLimitValueSelect.RowCount > 0 && this.gdPositionLimitValueSelect.FocusedRowHandle >= 0)
                {
                    m_CurRow = this.gdPositionLimitValueSelect.FocusedRowHandle;
                    if (m_CurRow != AppGlobalVariable.INIT_INT)
                    {
                        this.UpdateQHPositionLimitValue(m_CurRow);
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择记录!");
                }
                this.QueryQHPositionLimitValue();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6462";
                string errMsg = "修改（商品）期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

      
        #region 查询（商品）期货_持仓限制 btnQuery_Click
        /// <summary>
        /// 查询（商品）期货_持仓限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1;
                this.QueryQHPositionLimitValue();
                this.ShowDataPage();
                //DataTable _dtQHPositionLimitV = (DataTable)this.gdPositionLimitValueSelect.DataSource;
                DataView _dvQHPositionL = (DataView)this.gdPositionLimitValueSelect.DataSource;
                DataTable _dtQHPositionLimitV = _dvQHPositionL.Table;
                if (_dtQHPositionLimitV == null || _dtQHPositionLimitV.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6464";
                string errMsg = "查询（商品）期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 删除（商品）期货_持仓限制 btnDelete_Click
        /// <summary>
        /// 删除（商品）期货_持仓限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;

                DataRow _dr = this.gdPositionLimitValueSelect.GetDataRow(this.gdPositionLimitValueSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(_dr["PositionLimitValueID"])))
                {
                    m_PositionLimitValueID = Convert.ToInt32(_dr["PositionLimitValueID"]);
                }
                else
                {
                    m_PositionLimitValueID = AppGlobalVariable.INIT_INT;
                }

                if (m_PositionLimitValueID != AppGlobalVariable.INIT_INT)
                {
                    m_Result = FuturesManageCommon.DeleteQHPositionLimitValue(m_PositionLimitValueID);
                }

                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_PositionLimitValueID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryQHPositionLimitValue();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6466";
                string errMsg = "删除（商品）期货_持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 修改（商品）期货_持仓限制GridView双击事件 gdPositionLimitValueResult_DoubleClick
        /// <summary>
        ///  修改（商品）期货_持仓限制GridView双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdPositionLimitValueResult_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                   this.gdPositionLimitValueSelect.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

                if (this.gdPositionLimitValueSelect != null && this.gdPositionLimitValueSelect.FocusedRowHandle >= 0 &&
                    hi.RowHandle >= 0)
                {
                    m_CurRow = this.gdPositionLimitValueSelect.FocusedRowHandle;
                    this.UpdateQHPositionLimitValue(m_CurRow);
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


    }
}
