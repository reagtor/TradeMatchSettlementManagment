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
    /// 描述：现货交易费用管理窗体  错误编码范围:5420-5439
    /// 作者：刘书伟
    /// 日期：2008-12-11
    /// </summary>
    public partial class SpotCostsManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public SpotCostsManageUI()
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
        /// 当前gridView的行号
        /// </summary>
        private int m_CurRow = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

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

        #region 根据查询条件，获取现货交易费用

        /// <summary>
        /// 根据查询条件，获取现货交易费用
        /// </summary>
        /// <returns></returns>
        private bool QuerySpotCosts()
        {
            try
            {
                string breedClassName = this.txtBreedClassName.Text;
                DataSet _dsSpotCosts = SpotManageCommon.GetAllSpotCosts(m_BreedClassID, breedClassName,
                                                                        m_pageNo,
                                                                        m_pageSize,
                                                                        out m_rowCount);
                DataTable _dtSpotCosts;
                if (_dsSpotCosts == null || _dsSpotCosts.Tables[0].Rows.Count == 0)
                {
                    _dtSpotCosts = new DataTable();
                }
                else
                {
                    _dtSpotCosts = _dsSpotCosts.Tables[0];
                }

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetSpotCostsBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetSpotCostsBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetSpotCostsBreedClassName().Tables[0].Columns["BreedClassName"].ToString();
                this.gdSpotCostsResult.DataSource = _dtSpotCosts;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5425";
                string errMsg = "根据查询条件，获取现货交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }

            return true;
        }

        #endregion

        #region 获取需要更新的现货交易费用数据 UpdateSpotCosts

        /// <summary>
        /// 获取需要更新的现货交易费用数据 UpdateSpotCosts
        /// </summary>
        /// <param name="handle"></param>
        private void UpdateSpotCosts(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                //显示添加现货交易费用窗体
                AddSpotCostsUI addSpotCostsUI = new AddSpotCostsUI();
                addSpotCostsUI.EditType = (int) UITypes.EditTypeEnum.UpdateUI;
                DataRow _dr = this.gdvSpotCostsSelect.GetDataRow(handle);
                int breedClassID = Convert.ToInt32(_dr["BreedClassID"]);
                XH_SpotCosts xH_SpotCosts = SpotManageCommon.GetXHSpotCostsModel(breedClassID);
                addSpotCostsUI.XHSpotCosts = xH_SpotCosts;

                if (addSpotCostsUI.ShowDialog(this) == DialogResult.OK)
                {
                    this.QuerySpotCosts();
                    this.gdvSpotCostsSelect.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5423";
                string errMsg = "获取需要更新的现货交易费用数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 现货交易费UI SpotCostsManageUI_Load

        /// <summary>
        /// 现货交易费UI SpotCostsManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpotCostsManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定币种类型
                this.ddlCurrencyTypeID.DataSource = BindData.GetBindListCurrencyType();
                this.ddlCurrencyTypeID.ValueMember = "ValueIndex";
                this.ddlCurrencyTypeID.DisplayMember = "TextTitleValue";

                //绑定印花税收取方式
                this.ddlStampDutyTypeID.DataSource = BindData.GetBindListStampDutyType();
                this.ddlStampDutyTypeID.ValueMember = "ValueIndex";
                this.ddlStampDutyTypeID.DisplayMember = "TextTitleValue";

                //绑定过户费取值类型
                this.ddlTransferTollTypeID.DataSource = BindData.GetBindListXHTransferTollType();
                this.ddlTransferTollTypeID.ValueMember = "ValueIndex";
                this.ddlTransferTollTypeID.DisplayMember = "TextTitleValue";

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetSpotCostsBreedClassName().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetSpotCostsBreedClassName().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetSpotCostsBreedClassName().Tables[0].Columns["BreedClassName"].ToString();
                //绑定查询结果
                this.m_pageNo = 1;
                this.gdSpotCostsResult.DataSource = this.QuerySpotCosts();
                this.ShowDataPage();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5420";
                string errMsg = "现货交易费UI加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

            }
        }

        #endregion

        #region 显示添加现货交易费UI btnAdd_Click

        /// <summary>
        /// 显示添加现货交易费UI btnAdd_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //显示添加现货交易费用窗体
                AddSpotCostsUI addSpotCostsUI = new AddSpotCostsUI();
                addSpotCostsUI.ShowDialog();
                this.QuerySpotCosts();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5421";
                string errMsg = "显示添加现货交易费UI加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 修改现货交易费 btnModify_Click

        /// <summary>
        /// 修改现货交易费 btnModify_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gdvSpotCostsSelect != null && this.gdvSpotCostsSelect.DataSource != null &&
                    this.gdvSpotCostsSelect.RowCount > 0 && this.gdvSpotCostsSelect.FocusedRowHandle >= 0)
                {
                    m_CurRow = this.gdvSpotCostsSelect.FocusedRowHandle;
                    if (m_CurRow != AppGlobalVariable.INIT_INT)
                    {
                        this.UpdateSpotCosts(m_CurRow);
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择记录!");
                }
                this.QuerySpotCosts();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5424";
                string errMsg = "修改现货交易费失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 查询现货交易费用 btnQuery_Click

        /// <summary>
        /// 查询现货交易费用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QuerySpotCosts();
                this.ShowDataPage(); //显示数据分页

                DataView _dvSpotCosts = (DataView)this.gdvSpotCostsSelect.DataSource;
                DataTable _dtSpotCosts = _dvSpotCosts.Table;
                if (_dtSpotCosts == null || _dtSpotCosts.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5426";
                string errMsg = "查询现货交易费用失败!";
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
                this.QuerySpotCosts();
            }
        }

        #endregion

        #region 现货交易费用GridView的双击事件 gdSpotCostsResult_DoubleClick

        /// <summary>
        /// 现货交易费用GridView的双击事件 gdSpotCostsResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSpotCostsResult_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                    this.gdvSpotCostsSelect.CalcHitInfo(((Control) sender).PointToClient(Control.MousePosition));

                if (this.gdvSpotCostsSelect != null && this.gdvSpotCostsSelect.FocusedRowHandle >= 0 &&
                    hi.RowHandle >= 0)
                {
                    m_CurRow = this.gdvSpotCostsSelect.FocusedRowHandle;
                    this.UpdateSpotCosts(m_CurRow);
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

        #region 删除现货交易费用 btnDelete_Click

        /// <summary>
        /// 删除现货交易费用 btnDelete_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;

                DataRow _dr = this.gdvSpotCostsSelect.GetDataRow(this.gdvSpotCostsSelect.FocusedRowHandle);
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
                    //注意需要判断多值是，把此品种ID下的所有多值交易手续费删除
                    m_Result = SpotManageCommon.DeleteSpotCosts(m_BreedClassID);
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
                this.QuerySpotCosts();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5422";
                string errMsg = "删除现货交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 鼠标移到gdSpotCostsResult_MouseMove控件上的任一地方时，显示提示gdSpotCostsResult_MouseMove事件
        /// <summary>
        /// 鼠标移到gdSpotCostsResult_MouseMove控件上的任一地方时，显示提示gdSpotCostsResult_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdSpotCostsResult_MouseMove(object sender, MouseEventArgs e)
        {
            this.m_tip.SetToolTip(this.gdSpotCostsResult,"双击查看详细信息");
            this.m_tip.Active = true;
        }
        #endregion
    }
}