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
using Types = ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    /// <summary>
    /// 描述：现货交易规则管理窗体  错误编码范围:5020-5039
    /// 作者：刘书伟
    /// 日期：2008-12-11
    /// </summary>
    public partial class SpotTradeRulesManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public SpotTradeRulesManageUI()
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
        /// 品种有效申报ID
        /// </summary>
        private int m_BreedClassValidID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种涨跌幅ID
        /// </summary>
        private int m_BreedClassHighLowID = AppGlobalVariable.INIT_INT;

        private bool m_Result = false;

        /// <summary>
        /// 当前gridView的行号
        /// </summary>
        private int m_CurRow = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 提示信息变量
        /// </summary>
        private ToolTip m_tip = new ToolTip();

        #endregion

        //================================  私有  方法 ================================

        #region 获取查询条件(根据交易规则表中的品种ID获取对应的品种名称) GetQueryBreedClassName

        /// <summary>
        /// 获取查询条件(根据交易规则表中的品种ID获取对应的品种名称)
        /// </summary>
        //private void GetQueryBreedClassName()
        //{
        //    DataSet ds = SpotManageCommon.GetBreedClassNameByBreedClassID();
        //    UComboItem _item;
        //    _item = new UComboItem("", Int32.MaxValue);
        //    this.cmbBreedClassIDQ.Properties.Items.Add(_item);
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {
        //        _item = new UComboItem(ds.Tables[0].Rows[i]["BreedClassName"].ToString(),
        //                               Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]));
        //        this.cmbBreedClassIDQ.Properties.Items.Add(_item);
        //    }
        //}

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

        #region 根据查询条件，获取现货交易规则

        /// <summary>
        /// 根据查询条件，获取现货交易规则
        /// </summary>
        /// <returns></returns>
        private bool QuerySpotTradeRules()
        {
            try
            {
                string breedClassName = this.txtBreeClassName.Text;
                DataSet _dsSpotTradeRules = SpotManageCommon.GetAllSpotTradeRules(breedClassName,
                                                                                  m_pageNo,
                                                                                  m_pageSize,
                                                                                  out m_rowCount);
                DataTable _dtSpotTradeRule;
                if (_dsSpotTradeRules == null || _dsSpotTradeRules.Tables[0].Rows.Count == 0)
                {
                    _dtSpotTradeRule = new DataTable();
                }
                else
                {
                    _dtSpotTradeRule = _dsSpotTradeRules.Tables[0];
                }

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetBreedClassNameByBreedClassID().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassName"].ToString();


                this.gdSpotTradeRulesQResult.DataSource = _dtSpotTradeRule;
                //设置GridView显示效果
                foreach (DevExpress.XtraGrid.Columns.GridColumn _gridColumn in this.gdvSpotTradeRulesSelect.Columns)
                {
                    //switch (_gridColumn.FieldName.ToUpper())
                    //{
                    //    case "VALUETYPEMINCHANGEPRICE":
                    //        _gridColumn.Width = 90;
                    //        break;

                    //}
                    _gridColumn.Width = this.gdSpotTradeRulesQResult.Size.Width /
                                        this.gdvSpotTradeRulesSelect.Columns.Count + 6;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5025";
                string errMsg = "根据查询条件，获取现货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
            return true;
        }

        #endregion

        #region 获取当前行需要修改的数据 UpdateSpotTradeRules

        /// <summary>
        /// 获取当前行需要修改的数据
        /// </summary>
        /// <param name="handle">当前行号</param>
        private void UpdateSpotTradeRules(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                //显示添加现货规则窗体
                AddSpotTradeRulesUI addSpotTradeRulesUI = new AddSpotTradeRulesUI();
                addSpotTradeRulesUI.EditType = (int)UITypes.EditTypeEnum.UpdateUI;
                DataRow _dr = this.gdvSpotTradeRulesSelect.GetDataRow(handle);
                int breedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                XH_SpotTradeRules xHSpotTradeRules = SpotManageCommon.GetModel(breedClassID);
                addSpotTradeRulesUI.XHSpotTradeRules = xHSpotTradeRules;

                if (addSpotTradeRulesUI.ShowDialog(this) == DialogResult.OK)
                {
                    this.QuerySpotTradeRules();
                    this.gdvSpotTradeRulesSelect.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5023";
                string errMsg = "获取当前行需要修改的数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 现货交易规则管理UI SpotTradeRulesManageUI_Load

        /// <summary>
        /// 现货交易规则管理UI SpotTradeRulesManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpotTradeRulesManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定最小变动价位
                this.ddlValueTypeMinChangePrice.DataSource = BindData.GetBindListValueType();
                this.ddlValueTypeMinChangePrice.ValueMember = "ValueIndex";
                this.ddlValueTypeMinChangePrice.DisplayMember = "TextTitleValue";

                //绑定每笔最大委托量单位
                this.ddlMaxLeaveQuantityUnit.DataSource = BindData.GetBindListXHAboutUnit();
                this.ddlMaxLeaveQuantityUnit.ValueMember = "ValueIndex";
                this.ddlMaxLeaveQuantityUnit.DisplayMember = "TextTitleValue";

                //绑定是否允许回转
                this.ddlIsSlew.DataSource = BindData.GetBindListYesOrNo();
                this.ddlIsSlew.ValueMember = "ValueIndex";
                this.ddlIsSlew.DisplayMember = "TextTitleValue";


                //绑定行情成交量单位
                this.ddlMarketUnitID.DataSource = BindData.GetBindListXHAboutUnit();
                this.ddlMarketUnitID.ValueMember = "ValueIndex";
                this.ddlMarketUnitID.DisplayMember = "TextTitleValue";

                //绑定计价单位
                this.ddlPriceUnit.DataSource = BindData.GetBindListXHAboutUnit();
                this.ddlPriceUnit.ValueMember = "ValueIndex";
                this.ddlPriceUnit.DisplayMember = "TextTitleValue";

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetBreedClassNameByBreedClassID().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassName"].ToString();
                //绑定查询结果
                this.m_pageNo = 1;
                this.gdSpotTradeRulesQResult.DataSource = this.QuerySpotTradeRules();
                this.ShowDataPage();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5020";
                string errMsg = " 现货交易规则管理UI加载失败!";
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
                this.QuerySpotTradeRules();
            }
        }

        #endregion

        #region 显示添加现货交易规则UI btnAdd_Click

        /// <summary>
        /// 显示添加现货交易规则UI btnAdd_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //显示添加现货规则窗体
                AddSpotTradeRulesUI addSpotTradeRulesUI = new AddSpotTradeRulesUI();
                addSpotTradeRulesUI.ShowDialog();
                this.QuerySpotTradeRules();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5021";
                string errMsg = "显示添加现货交易规则UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 显示修改现货交易规则UI btnModify_Click

        /// <summary>
        /// 显示修改现货交易规则UI btnModify_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gdvSpotTradeRulesSelect != null && this.gdvSpotTradeRulesSelect.DataSource != null &&
                    this.gdvSpotTradeRulesSelect.RowCount > 0 && this.gdvSpotTradeRulesSelect.FocusedRowHandle >= 0)
                {
                    m_CurRow = this.gdvSpotTradeRulesSelect.FocusedRowHandle;
                    if (m_CurRow != AppGlobalVariable.INIT_INT)
                    {
                        this.UpdateSpotTradeRules(m_CurRow);
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择记录!");
                }
                this.QuerySpotTradeRules();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5024";
                string errMsg = "显示修改现货交易规则UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 现货交易规则GridView的双击事件 gdSpotTradeRulesQResult_DoubleClick

        /// <summary>
        /// 现货交易规则GridView的双击事件 gdSpotTradeRulesQResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSpotTradeRulesQResult_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.gdvSpotTradeRulesSelect.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

            if (this.gdvSpotTradeRulesSelect != null && this.gdvSpotTradeRulesSelect.FocusedRowHandle >= 0 &&
                hi.RowHandle >= 0)
            {
                m_CurRow = this.gdvSpotTradeRulesSelect.FocusedRowHandle;
                UpdateSpotTradeRules(m_CurRow);
            }
            //else
            //{
            //    ShowMessageBox.ShowInformation("请选中记录行!");
            //}
        }

        #endregion

        #region 查询现货交易规则 btnQuery_Click

        /// <summary>
        /// 查询现货交易规则 btnQuery_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1;
                this.QuerySpotTradeRules();
                this.ShowDataPage();
                this.gdSpotTRulesDetailResult.DataSource = null;//当执行查询时，现货明细GridView不显示数据
                //DataTable _dtSpotTradeRules = (DataTable) this.gdSpotTradeRulesQResult.DataSource;
                DataView _dvSpotTradeR = (DataView)this.gdvSpotTradeRulesSelect.DataSource;
                DataTable _dtSpotTradeRules = _dvSpotTradeR.Table;
                if (_dtSpotTradeRules == null || _dtSpotTradeRules.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5026";
                string errMsg = "查询现货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 删除现货交易规则 btnDelete_Click

        /// <summary>
        /// 删除现货交易规则 btnDelete_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除交易规则吗？") == DialogResult.No) return;

                DataRow _dr = this.gdvSpotTradeRulesSelect.GetDataRow(this.gdvSpotTradeRulesSelect.FocusedRowHandle);
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

                if (!string.IsNullOrEmpty(Convert.ToString(_dr["BreedClassValidID"])))
                {
                    m_BreedClassValidID = Convert.ToInt32(_dr["BreedClassValidID"]);
                }
                else
                {
                    m_BreedClassValidID = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(_dr["BreedClassHighLowID"])))
                {
                    m_BreedClassHighLowID = Convert.ToInt32(_dr["BreedClassHighLowID"]);
                }
                else
                {
                    m_BreedClassHighLowID = AppGlobalVariable.INIT_INT;
                }
                //删除需要实现当交易规则删除时，港股的最小变动价位的删除
                if (m_BreedClassID != AppGlobalVariable.INIT_INT)
                {
                    // m_Result = SpotManageCommon.DeleteSpotTradeRules(m_BreedClassID, m_BreedClassHighLowID,
                    // m_BreedClassValidID);
                    m_Result = SpotManageCommon.DeleteSpotTradeRulesAboutAll(m_BreedClassID);
                }

                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_BreedClassID = AppGlobalVariable.INIT_INT;
                    m_BreedClassHighLowID = AppGlobalVariable.INIT_INT;
                    m_BreedClassValidID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QuerySpotTradeRules();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5022";
                string errMsg = "删除现货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region  gdSpotTradeRulesQResult的单击事件 gdSpotTradeRulesQResult_Click
        /// <summary>
        /// gdSpotTradeRulesQResult的单击事件 gdSpotTradeRulesQResult_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSpotTradeRulesQResult_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
               this.gdvSpotTradeRulesSelect.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));
            if (this.gdvSpotTradeRulesSelect != null && this.gdvSpotTradeRulesSelect.FocusedRowHandle >= 0 &&
                hi.RowHandle >= 0)
            {
                int CurRow = this.gdvSpotTradeRulesSelect.FocusedRowHandle;
                if (CurRow < 0)
                {
                    return;
                }
                DataRow _dr = this.gdvSpotTradeRulesSelect.GetDataRow(CurRow);
                int breedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                DataSet _dsSpotTRulesD = SpotManageCommon.GetSpotTradeRulesDetail(breedClassID);
                DataTable _dtSpotTrulesD;
                if (_dsSpotTRulesD == null || _dsSpotTRulesD.Tables[0].Rows.Count == 0)
                {
                    _dtSpotTrulesD = new DataTable();
                }
                else
                {
                    _dtSpotTrulesD = _dsSpotTRulesD.Tables[0];
                }
                //if (_dtSpotTrulesD != null)
                if(_dtSpotTrulesD.Rows.Count>0)
                {
                    if (_dtSpotTrulesD.Rows[0]["VALIDDECLARETYPEID"] != DBNull.Value)
                    {
                        int validDTypeID = Convert.ToInt32(_dtSpotTrulesD.Rows[0]["VALIDDECLARETYPEID"]);
                        //当有效申报类型是：价位时，涨跌幅类型显示为“港股”
                        if (validDTypeID == (int)GTA.VTS.Common.CommonObject.Types.XHValidDeclareType.DownBuyOneAndSaleOne)
                        {
                            _dtSpotTrulesD.Rows[0]["HIGHLOWTYPEID"] = 5;//改变涨跌幅界面显示的类型值是5
                            //绑定涨跌幅类型
                            this.ddlHighLowTypeID.DataSource = BindData.GetBindListXHHighLowTypeDisplayAll();
                            this.ddlHighLowTypeID.ValueMember = "ValueIndex";
                            this.ddlHighLowTypeID.DisplayMember = "TextTitleValue";
                        }
                        else
                        {
                            //绑定涨跌幅类型
                            this.ddlHighLowTypeID.DataSource = BindData.GetBindListXHHighLowTypeDisplayAll();
                            this.ddlHighLowTypeID.ValueMember = "ValueIndex";
                            this.ddlHighLowTypeID.DisplayMember = "TextTitleValue";
                        }
                    }

                }
                //绑定有效申报类型
                this.ddlValidDeclareTypeID.DataSource = BindData.GetBindListXHValidDeclareTypeDisplayAll();
                this.ddlValidDeclareTypeID.ValueMember = "ValueIndex";
                this.ddlValidDeclareTypeID.DisplayMember = "TextTitleValue";
                this.gdSpotTRulesDetailResult.DataSource = _dtSpotTrulesD;
            }
            //else
            //{
            //    ShowMessageBox.ShowInformation("请选择记录!");
            //}
        }
        #endregion

        #region 鼠标移到gdSpotTradeRulesQResult_MouseMove控件上的任一地方时，显示提示gdSpotTradeRulesQResult_MouseMove事件
        /// <summary>
        /// 鼠标移到gdSpotTradeRulesQResult_MouseMove控件上的任一地方时，显示提示gdSpotTradeRulesQResult_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSpotTradeRulesQResult_MouseMove(object sender, MouseEventArgs e)
        {
            this.m_tip.SetToolTip(this.gdSpotTradeRulesQResult, "双击查看详细信息");
            this.m_tip.Active = true;
        }
        #endregion

    }
}