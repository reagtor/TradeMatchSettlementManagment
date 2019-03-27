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
    /// 描述：期货交易规则管理 错误编码范围:5820-5839
    /// 作者：刘书伟
    /// 日期：2008-12-08
    /// </summary>
    public partial class FuturesTradeRulesManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public FuturesTradeRulesManageUI()
        {
            InitializeComponent();
        }

        #endregion

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

        #region 变量

        /// <summary>
        /// 品种ID
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 最后交易日ID
        /// </summary>
        private int m_LastTradingDayID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 交易规则委托量ID
        /// </summary>
        private int m_ConsignQuantumID = AppGlobalVariable.INIT_INT;

        private bool m_Result = false;

        /// <summary>
        /// 当前gridView的行号
        /// </summary>
        private int m_CurRow = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 提示信息变量
        /// </summary>
        private ToolTip m_tip=new ToolTip();

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
                    if (m_rowCount%this.m_pageSize == 0)
                    {
                        this.UCPageNavig.PageCount = Convert.ToInt32(m_rowCount/this.m_pageSize);
                    }
                    else
                    {
                        this.UCPageNavig.PageCount = Convert.ToInt32(m_rowCount/this.m_pageSize) + 1;
                    }
                }

                this.UCPageNavig.CurrentPage = this.m_pageNo;
            }
        }

        #endregion

        #region 根据查询条件，获取期货交易规则

        /// <summary>
        /// 根据查询条件，获取期货交易规则
        /// </summary>
        /// <returns></returns>
        private bool QueryFuturesTradeRules()
        {
            try
            {
                string breedClassName = this.txtBreeClassName.Text;
                DataSet _dsFuturesTradeRules = FuturesManageCommon.GetAllFuturesTradeRules(breedClassName,
                                                                                           m_pageNo,
                                                                                           m_pageSize,
                                                                                           out m_rowCount);
                DataTable _dtFuturesTradeRules;
                if (_dsFuturesTradeRules == null || _dsFuturesTradeRules.Tables[0].Rows.Count == 0)
                {
                    _dtFuturesTradeRules = new DataTable();
                }
                else
                {
                    _dtFuturesTradeRules = _dsFuturesTradeRules.Tables[0];
                }

                //绑定品种名称
                this.ddlBreedClassID.DataSource = CommonParameterSetCommon.GetQHFutureCostsBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    CommonParameterSetCommon.GetQHFutureCostsBreedClassName().Tables[0].Columns["BreedClassID"].ToString
                        ();
                this.ddlBreedClassID.DisplayMember =
                    CommonParameterSetCommon.GetQHFutureCostsBreedClassName().Tables[0].Columns["BreedClassName"].
                        ToString();


                this.gdFuturesTradeRulesResult.DataSource = _dtFuturesTradeRules;
                foreach (DevExpress.XtraGrid.Columns.GridColumn _col in this.gdvFuturesTradeRulesSelect.Columns)
                {
                    _col.Width = this.gdFuturesTradeRulesResult.Size.Width/this.gdvFuturesTradeRulesSelect.Columns.Count + 50;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5826";
                string errMsg = "根据查询条件，获取期货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region  获取需要更新的期货交易规则数据

        /// <summary>
        /// 获取需要更新的期货交易规则数据
        /// </summary>
        /// <param name="handle"></param>
        private void UpdateFuturesTradeRules(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                //显示添加期货规则窗体
                AddFuturesTradeRulesUI addFuturesTradeRulesUI = new AddFuturesTradeRulesUI();
                addFuturesTradeRulesUI.EditType = (int) UITypes.EditTypeEnum.UpdateUI;
                DataRow _dr = this.gdvFuturesTradeRulesSelect.GetDataRow(handle);
                int breedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                QH_FuturesTradeRules qHFuturesTradeRules = FuturesManageCommon.GetFuturesTradeRulesModel(breedClassID);
                addFuturesTradeRulesUI.QHFuturesTradeRules = qHFuturesTradeRules;

                if (addFuturesTradeRulesUI.ShowDialog(this) == DialogResult.OK)
                {
                    this.QueryFuturesTradeRules();
                    this.gdvFuturesTradeRulesSelect.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5823";
                string errMsg = "获取需要更新的期货交易规则数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 期货交易规则管理UI FuturesTradeRulesManageUI_Load

        /// <summary>
        /// 期货交易规则管理UI FuturesTradeRulesManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuturesTradeRulesManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定是否允许回转
                this.ddlIsSlew.DataSource = BindData.GetBindListYesOrNo();
                this.ddlIsSlew.ValueMember = "ValueIndex";
                this.ddlIsSlew.DisplayMember = "TextTitleValue";

                //绑定合约交易月份是否包含春节
                this.ddlIfContainCNewYear.DataSource = BindData.GetBindListYesOrNo();
                this.ddlIfContainCNewYear.ValueMember = "ValueIndex";
                this.ddlIfContainCNewYear.DisplayMember = "TextTitleValue";

                //绑定涨跌停板幅度类型
                this.ddlHighLowStopScopeID.DataSource = BindData.GetBindListQHHighLowStopALLType();// GetBindListQHHighLowStopType();
                this.ddlHighLowStopScopeID.ValueMember = "ValueIndex";
                this.ddlHighLowStopScopeID.DisplayMember = "TextTitleValue";

                //绑定交易单位
                this.ddlUnitsID.DataSource = BindData.GetBindListQHAboutUnit();
                this.ddlUnitsID.ValueMember = "ValueIndex";
                this.ddlUnitsID.DisplayMember = "TextTitleValue";

                //绑定计价单位
                this.ddlPriceUnit.DataSource = BindData.GetBindListQHPriceUnit();
                this.ddlPriceUnit.ValueMember = "ValueIndex";
                this.ddlPriceUnit.DisplayMember = "TextTitleValue";


                //绑定行情成交量单位
                this.ddlMarketUnitID.DataSource = BindData.GetBindListQHAboutUnit();
                this.ddlMarketUnitID.ValueMember = "ValueIndex";
                this.ddlMarketUnitID.DisplayMember = "TextTitleValue";

                //绑定查询结果
                this.m_pageNo = 1;
                this.gdFuturesTradeRulesResult.DataSource = this.QueryFuturesTradeRules();
                this.ShowDataPage();

            }
            catch (Exception ex)
            {
                string errCode = "GL-5820";
                string errMsg = "期货交易规则管理UI加载失败!";
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
                this.QueryFuturesTradeRules();
            }
        }

        #endregion

        #region  查询期货交易规则

        /// <summary>
        /// 查询期货交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1;
                this.QueryFuturesTradeRules();
                this.ShowDataPage();
                //DataTable _dtFuturesTradeRules = (DataTable) this.gdvFuturesTradeRulesSelect.DataSource;
                DataView _dvFuturesTradeR = (DataView)this.gdvFuturesTradeRulesSelect.DataSource;
                DataTable _dtFuturesTradeRules = _dvFuturesTradeR.Table;
                if (_dtFuturesTradeRules == null || _dtFuturesTradeRules.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5825";
                string errMsg = "查询期货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加期货交易规则

        /// <summary>
        /// 添加期货交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //显示添加期货规则窗体
                AddFuturesTradeRulesUI addFuturesTradeRulesUI = new AddFuturesTradeRulesUI();
                addFuturesTradeRulesUI.ShowDialog();
                this.QueryFuturesTradeRules();

            }
            catch (Exception ex)
            {
                string errCode = "GL-5821";
                string errMsg = "显示添加期货交易规则UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 修改期货交易规则

        /// <summary>
        /// 修改期货交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gdvFuturesTradeRulesSelect != null && this.gdvFuturesTradeRulesSelect.DataSource != null &&
                    this.gdvFuturesTradeRulesSelect.RowCount > 0 &&
                    this.gdvFuturesTradeRulesSelect.FocusedRowHandle >= 0)
                {
                    m_CurRow = this.gdvFuturesTradeRulesSelect.FocusedRowHandle;
                    if (m_CurRow != AppGlobalVariable.INIT_INT)
                    {
                        this.UpdateFuturesTradeRules(m_CurRow);
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择记录!");
                }
                this.QueryFuturesTradeRules();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5822";
                string errMsg = "修改期货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 修改 期货交易规则UI的GridView双击事件 gdFuturesTradeRulesResult_DoubleClick

        /// <summary>
        ///修改 期货交易规则UI的GridView双击事件 gdFuturesTradeRulesResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdFuturesTradeRulesResult_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.gdvFuturesTradeRulesSelect.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

                if (this.gdvFuturesTradeRulesSelect != null && this.gdvFuturesTradeRulesSelect.FocusedRowHandle >= 0 &&
                    hi.RowHandle >= 0)
                {
                    m_CurRow = this.gdvFuturesTradeRulesSelect.FocusedRowHandle;
                    UpdateFuturesTradeRules(m_CurRow);
                }
                //else
                //{
                //    ShowMessageBox.ShowInformation("请选中记录行!");
                //}

            }
            catch 
            {
                return;
            }

        }

        #endregion

        #region 删除期货交易规则
        /// <summary>
        /// 删除期货交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除交易规则吗？") == DialogResult.No) return;

                DataRow _dr = this.gdvFuturesTradeRulesSelect.GetDataRow(this.gdvFuturesTradeRulesSelect.FocusedRowHandle);
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

                if (!string.IsNullOrEmpty(Convert.ToString(_dr["LastTradingDayID"])))
                {
                    m_LastTradingDayID = Convert.ToInt32(_dr["LastTradingDayID"]);
                }
                else
                {
                    m_LastTradingDayID = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(_dr["ConsignQuantumID"])))
                {
                    m_ConsignQuantumID = Convert.ToInt32(_dr["ConsignQuantumID"]);
                }
                else
                {
                    m_ConsignQuantumID = AppGlobalVariable.INIT_INT;
                }
                if (m_BreedClassID != AppGlobalVariable.INIT_INT)
                {
                    m_Result = FuturesManageCommon.DeleteFuturesTradeRulesAboutAll(m_BreedClassID);
                }

                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_BreedClassID = AppGlobalVariable.INIT_INT;
                    m_LastTradingDayID = AppGlobalVariable.INIT_INT;
                    m_ConsignQuantumID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryFuturesTradeRules();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5824";
                string errMsg = "删除期货交易规则数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 鼠标移到gdFuturesTradeRulesResult_MouseMove控件上的任一地方时，显示提示gdFuturesTradeRulesResult_MouseMove事件
        /// <summary>
        /// 鼠标移到gdFuturesTradeRulesResult_MouseMove控件上的任一地方时，显示提示gdFuturesTradeRulesResult_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdFuturesTradeRulesResult_MouseMove(object sender, MouseEventArgs e)
        {
            //this.m_tip.SetToolTip(this.gdFuturesTradeRulesResult,"双击查看详细信息");
            //this.m_tip.Active = true;
        }
        #endregion

    }
}