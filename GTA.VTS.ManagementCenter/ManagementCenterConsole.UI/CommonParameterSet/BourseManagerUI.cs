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
    /// 描述：交易所类型管理窗体  错误编码范围:4400-4409  (4419
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// </summary>
    public partial class BourseManagerUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public BourseManagerUI()
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

        #region 根据查询条件，获取交易所类型

        /// <summary>
        /// 根据查询条件，获取交易所类型
        /// </summary>
        /// <returns></returns>
        private bool QueryCMBourseType()
        {
            try
            {
                string bourseTypeName = this.txtCondition.Text;
                DataSet _dsCMBourseType = CommonParameterSetCommon.GetAllCMBourseType(bourseTypeName,
                                                                                      m_pageNo,
                                                                                      m_pageSize,
                                                                                      out m_rowCount);
                DataTable _dtCMBourseType;
                if (_dsCMBourseType == null || _dsCMBourseType.Tables[0].Rows.Count == 0)
                {
                    _dtCMBourseType = new DataTable();
                }
                else
                {
                    _dtCMBourseType = _dsCMBourseType.Tables[0];
                }
                this.gdBourseTypeResult.DataSource = _dtCMBourseType;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4404";
                string errMsg = "根据查询条件，获取交易所类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }

            return true;
        }

        #endregion

        #region 获取需要更新的交易所类型数据 UpdateCMBourseType

        /// <summary>
        /// 获取需要更新的交易所类型数据 UpdateCMBourseType
        /// </summary>
        /// <param name="handle"></param>
        private void UpdateCMBourseType(int handle)
        {
            try
            {
                if (handle < 0)
                {
                    return;
                }
                //// 显示添加交易所类型UI
                //AddBourseUI addBourseUI = new AddBourseUI();
                //addBourseUI.EditType = (int) UITypes.EditTypeEnum.UpdateUI;
                //显示添加交易时间窗体
                TradeTimeManagerUI tradeTimeManagerUI = new TradeTimeManagerUI();
                tradeTimeManagerUI.EditType = (int)UITypes.EditTypeEnum.UpdateUI;
                DataRow _dr = this.gdvBourseTypeSelect.GetDataRow(handle);
                int bourseTypeID = Convert.ToInt32(_dr["BourseTypeID"]);
                CM_BourseType cM_BourseType = CommonParameterSetCommon.GetCMBourseTypeModel(bourseTypeID);
                tradeTimeManagerUI.CMBourseType = cM_BourseType;

                if (tradeTimeManagerUI.ShowDialog(this) == DialogResult.OK)
                {
                    this.QueryCMBourseType();
                    this.gdvBourseTypeSelect.FocusedRowHandle = handle;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4402";
                string errMsg = "获取需要更新的现货交易费用数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 交易所类型管理窗体 BourseManagerUI_Load

        /// <summary>
        /// 交易所类型管理窗体 BourseManagerUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BourseManagerUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定查询结果
                this.m_pageNo = 1;
                this.gdBourseTypeResult.DataSource = this.QueryCMBourseType();
                this.ShowDataPage();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4400";
                string errMsg = "交易所类型管理窗体加载失败!";
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
                this.QueryCMBourseType();
            }
        }

        #endregion

        #region 查询交易所类型 btnQuery_Click

        /// <summary>
        /// 查询交易所类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryCMBourseType();
                this.ShowDataPage(); //显示数据分页
                this.gdTradeTimeResult.DataSource = null;//当执行查询时，交易时间的GridView数据为空
                //DataTable _dtCMBourseType = (DataTable) this.gdvBourseTypeSelect.DataSource;
                DataView _dVCMBourseT = (DataView)this.gdvBourseTypeSelect.DataSource;
                DataTable _dtCMBourseType = _dVCMBourseT.Table;
                if (_dtCMBourseType == null || _dtCMBourseType.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4405";
                string errMsg = "查询交易所类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 显示添加交易所类型UI

        /// <summary>
        /// 显示添加交易所类型UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //AddBourseUI addBourseUI = new AddBourseUI();
                //addBourseUI.ShowDialog();
                //显示添加交易时间窗体
                TradeTimeManagerUI tradeTimeManagerUI = new TradeTimeManagerUI();
                tradeTimeManagerUI.EditType = (int)UITypes.EditTypeEnum.AddUI;
                tradeTimeManagerUI.ShowDialog();
                this.QueryCMBourseType();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4401";
                string errMsg = "显示添加交易所类型UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 修改交易所类型 btnModify_Click

        /// <summary>
        /// 修改交易所类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gdvBourseTypeSelect != null && this.gdvBourseTypeSelect.DataSource != null &&
                    this.gdvBourseTypeSelect.RowCount > 0 && this.gdvBourseTypeSelect.FocusedRowHandle >= 0)
                {
                    m_CurRow = this.gdvBourseTypeSelect.FocusedRowHandle;
                    if (m_CurRow != AppGlobalVariable.INIT_INT)
                    {
                        this.UpdateCMBourseType(m_CurRow);
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择记录!");
                }
                this.QueryCMBourseType();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4403";
                string errMsg = "修改交易所类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 交易所类型GridView的双击事件 gdBourseTypeResult_DoubleClick

        /// <summary>
        /// 交易所类型GridView的双击事件 gdBourseTypeResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdBourseTypeResult_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                    this.gdvBourseTypeSelect.CalcHitInfo(((Control) sender).PointToClient(Control.MousePosition));

                if (this.gdvBourseTypeSelect != null && this.gdvBourseTypeSelect.FocusedRowHandle >= 0 &&
                    hi.RowHandle >= 0)
                {
                    m_CurRow = this.gdvBourseTypeSelect.FocusedRowHandle;
                    this.UpdateCMBourseType(m_CurRow);
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

        #region 根据交易所类型获取交易所时间 gdBourseTypeResult_Click
        /// <summary>
        /// 根据交易所类型获取交易所时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdBourseTypeResult_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
               this.gdvBourseTypeSelect.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

                if (this.gdvBourseTypeSelect != null && this.gdvBourseTypeSelect.FocusedRowHandle >= 0 &&
                    hi.RowHandle >= 0)
                {
                    int CurRow = this.gdvBourseTypeSelect.FocusedRowHandle;
                    if (CurRow < 0)
                    {
                        return;
                    }
                    DataRow _dr = this.gdvBourseTypeSelect.GetDataRow(CurRow);
                    int bourseTypeID = Convert.ToInt32(_dr["BourseTypeID"]);
                    DataSet _dsTradeTime = CommonParameterSetCommon.GetTradeTimeByBourseTypeID(bourseTypeID);//GetTradeTimeByBourseTypeID(bourseTypeID);
                    DataTable _dtTradeTime;
                    if (_dsTradeTime == null || _dsTradeTime.Tables[0].Rows.Count == 0)
                    {
                        _dtTradeTime = new DataTable();
                    }
                    else
                    {
                        _dtTradeTime = _dsTradeTime.Tables[0];
                    }

                    ////绑定交易所类型_交易时间中的交易所类型ID对应的交易所名称
                    this.ddlBourseTypeID.DataSource = CommonParameterSetCommon.GetBourseTypeNameByBourseTypeID().Tables[0];
                    this.ddlBourseTypeID.ValueMember =
                        CommonParameterSetCommon.GetBourseTypeNameByBourseTypeID().Tables[0].Columns["BourseTypeID"].
                            ToString();
                    this.ddlBourseTypeID.DisplayMember =
                        CommonParameterSetCommon.GetBourseTypeNameByBourseTypeID().Tables[0].Columns["BourseTypeName"].
                            ToString();
                    this.gdTradeTimeResult.DataSource = _dtTradeTime;
                }
                //else
                //{
                //    ShowMessageBox.ShowInformation("请选中记录行!");
                //}
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }
        #endregion

        #region 鼠标移到gdBourseTypeResult_MouseMove控件上的任一地方时，显示提示gdBourseTypeResult_MouseMove事件
        /// <summary>
        /// 鼠标移到gdBourseTypeResult_MouseMove控件上的任一地方时，显示提示gdBourseTypeResult_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdBourseTypeResult_MouseMove(object sender, MouseEventArgs e)
        {
            this.m_tip.SetToolTip(this.gdBourseTypeResult,"双击查看详细信息");
            this.m_tip.Active = true;
        }
        #endregion

        #region 删除交易所类型
        /// <summary>
        /// 删除交易所类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdvBourseTypeSelect.GetDataRow(this.gdvBourseTypeSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                int BourseTypeID= Convert.ToInt32(_dr["BourseTypeID"]);
                bool Result = false;//结果变量
                //int breedClassID = Convert.ToInt32(_dr["BreedClassID"]);//品种ID
                if (BourseTypeID != AppGlobalVariable.INIT_INT)
                {
                    int ISSysDefaultBourseType = Convert.ToInt32(_dr["ISSysDefaultBourseType"]);
                    if (ISSysDefaultBourseType == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No)
                    {
                        Result = CommonParameterSetCommon.DeleteCMBourseTypeAbout(BourseTypeID);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("系统默认交易所不能删除!");
                        return;
                    }
                }
                if (Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryCMBourseType();
            }
            catch (Exception ex)
            {
                string errCode = "GL-";
                string errMsg = "删除交易所类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return;
            }
        }

        #endregion


    }
}