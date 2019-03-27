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
    /// 描述：熔断管理窗体  错误编码范围:6800-6819
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// 修改：叶振东
    /// 日期：2010-04-06
    /// 描述：修改熔断管理窗体中按钮事件
    /// </summary>
    public partial class CommodityFuseManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public CommodityFuseManageUI()
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
        /// 商品代码(可交易商品_熔断) 
        /// </summary>
        private string m_CommodityCode = AppGlobalVariable.INIT_STRING;

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

        #region 获取品种类型股指期货的商品代码

        /// <summary>
        /// 获取品种类型股指期货的商品代码
        /// </summary>
        private void GetQHSIFCommodityCode()
        {
            try
            {
                DataSet ds = CommonParameterSetCommon.GetQHSIFCommodityCode(); //从交易商品表中获取
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        this.cmbCommodityCode.Properties.Items.Add(ds.Tables[0].Rows[i]["CommodityCode"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-6807";
                string errMsg = "获取品种类型股指期货的商品代码失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }

        }

        #endregion

        #region 根据查询条件，获取可交易商品_熔断

        /// <summary>
        /// 根据查询条件，获取可交易商品_熔断
        /// </summary>
        /// <returns></returns>
        private bool QueryCMCommodityFuse()
        {
            try
            {
                string CommodityCode = this.txtCommodityCode.Text;
                DataSet _dsCMCommodityFuse = CommonParameterSetCommon.GetAllCMCommodityFuse(CommodityCode,
                                                                                            m_pageNo,
                                                                                            m_pageSize,
                                                                                            out m_rowCount);
                DataTable _dtCMCommodityFuse;
                if (_dsCMCommodityFuse == null || _dsCMCommodityFuse.Tables[0].Rows.Count == 0)
                {
                    _dtCMCommodityFuse = new DataTable();
                }
                else
                {
                    _dtCMCommodityFuse = _dsCMCommodityFuse.Tables[0];
                }
                this.gdCommodityFuseResult.DataSource = _dtCMCommodityFuse;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6806";
                string errMsg = "根据查询条件，获取可交易商品_熔断失败!";
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
            //this.txtFuseTimeOfDay.Text = string.Empty;
            //this.txTriggeringScale.Text = string.Empty;
            //this.txtFuseDurationLimit.EditValue = string.Empty;
            //this.txtTriggeringDuration.EditValue = string.Empty;
            //this.tmFuseDurationLimit.EditValue = "00:00";
            //this.tmTriggeringDuration.EditValue = "00:00";
            this.cmbCommodityCode.Enabled = false;
            this.txTriggeringScale.Enabled = false;
            this.txtFuseTimeOfDay.Enabled = false;
            this.txtTriggeringDuration.Enabled = false;
            this.txtFuseDurationLimit.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnAdd.Text = "添加";
            this.btnAdd.Enabled = true;
            this.btnModify.Text = "修改";
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
        }

        #endregion

        #region 修改可交易商品_熔断

        /// <summary>
        /// 修改可交易商品_熔断
        /// </summary>
        private void ModifyCMCommodityFuse()
        {
            try
            {
                // btnModify.Enabled = true;
                if (this.gdCommodityFuseSelect != null && this.gdCommodityFuseSelect.DataSource != null &&
                    this.gdCommodityFuseSelect.RowCount > 0 && this.gdCommodityFuseSelect.FocusedRowHandle >= 0)
                {
                    btnModify.Enabled = true;
                    DataRow _dr = this.gdCommodityFuseSelect.GetDataRow(this.gdCommodityFuseSelect.FocusedRowHandle);
                    m_CommodityCode = _dr["CommodityCode"].ToString();
                    this.txtFuseTimeOfDay.Text = _dr["FuseTimeOfDay"].ToString();
                    this.txTriggeringScale.Text = _dr["TriggeringScale"].ToString();
                    this.txtFuseDurationLimit.EditValue = Convert.ToInt32(_dr["FuseDurationLimit"]);
                    this.txtTriggeringDuration.EditValue = Convert.ToInt32(_dr["TriggeringDuration"]);

                    //foreach (object item in this.cmbBourseTypeID.Properties.Items)
                    //{
                    //    if (((UComboItem)item).ValueIndex == Convert.ToInt32(_dr["BourseTypeID"]))
                    //    {
                    this.cmbCommodityCode.SelectedItem = m_CommodityCode;
                    //    break;
                    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6808";
                string errMsg = "修改可交易商品_熔断失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 熔断管理窗体 CommodityFuseManageUI_Load

        /// <summary>
        /// 熔断管理窗体 CommodityFuseManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommodityFuseManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定品种类型股指期货的商品代码
                this.cmbCommodityCode.Properties.Items.Clear();
                this.GetQHSIFCommodityCode(); //从交易商品表中获取
                this.cmbCommodityCode.SelectedIndex = 0;
                //初次加载时，更新按钮禁用
                //btnModify.Enabled = false;
                //绑定查询结果
                this.m_pageNo = 1;
                this.gdCommodityFuseResult.DataSource = this.QueryCMCommodityFuse();
                this.ShowDataPage();

                //窗体加载时禁用文本和确定按钮
                this.cmbCommodityCode.Enabled = false;
                this.txTriggeringScale.Enabled = false;
                this.txtFuseTimeOfDay.Enabled = false;
                this.txtTriggeringDuration.Enabled = false;
                this.txtFuseDurationLimit.Enabled = false;
                this.btnOK.Enabled = false;

            }
            catch (Exception ex)
            {
                string errCode = "GL-6800";
                string errMsg = " 熔断管理窗体加载失败!";
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
                this.QueryCMCommodityFuse();
            }
        }

        #endregion

        #region 查询可交易商品_熔断

        /// <summary>
        /// 查询可交易商品_熔断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryCMCommodityFuse();
                this.ShowDataPage(); //显示数据分页
                //DataTable _dtCMCommodityFuse = (DataTable) this.gdCommodityFuseSelect.DataSource;
                DataView _dvCMCommodityF = (DataView)this.gdCommodityFuseSelect.DataSource;
                DataTable _dtCMCommodityFuse = _dvCMCommodityF.Table;
                if (_dtCMCommodityFuse == null || _dtCMCommodityFuse.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6805";
                string errMsg = "查询可交易商品_熔断失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加可交易商品_熔断

        /// <summary>
        ///添加可交易商品_熔断 
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
                #region 文本框清空
                this.txtFuseTimeOfDay.Text = string.Empty;
                this.txTriggeringScale.Text = string.Empty;
                this.txtFuseDurationLimit.EditValue = string.Empty;
                this.txtTriggeringDuration.EditValue = string.Empty;
                #endregion
                #region 按钮禁用
                this.cmbCommodityCode.Enabled = true;
                this.txTriggeringScale.Enabled = true;
                this.txtFuseTimeOfDay.Enabled = true;
                this.txtTriggeringDuration.Enabled = true;
                this.txtFuseDurationLimit.Enabled = true;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnOK.Enabled = true;
                #endregion
            }
            else if (Name.Equals("取消"))
            {
                this.ClearAll();
            }
        }

        #endregion

        #region 修改可交易商品_熔断

        /// <summary>
        /// 修改可交易商品_熔断
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
                #region 按钮禁用
                this.txTriggeringScale.Enabled = true;
                this.txtFuseTimeOfDay.Enabled = true;
                this.txtTriggeringDuration.Enabled = true;
                this.txtFuseDurationLimit.Enabled = true;
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnOK.Enabled = true;
                #endregion
            }
            else if (Name.Equals("取消"))
            {
                this.ClearAll();
            }
        }

        #endregion

        #region 修改可交易商品_熔断的GridView双击事件

        /// <summary>
        ///  修改可交易商品_熔断的GridView双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdCommodityFuseResult_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //btnAdd.Enabled = false;
                this.cmbCommodityCode.Enabled = false;
                this.ModifyCMCommodityFuse();
                this.ClearAll();

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }

        #endregion

        #region 删除可交易商品_熔断

        /// <summary>
        /// 删除可交易商品_熔断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdCommodityFuseSelect.GetDataRow(this.gdCommodityFuseSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["CommodityCode"])))
                {
                    m_CommodityCode = _dr["CommodityCode"].ToString();
                }
                else
                {
                    m_CommodityCode = AppGlobalVariable.INIT_STRING;
                }
                if (m_CommodityCode != AppGlobalVariable.INIT_STRING)
                {
                    //  m_Result = CommonParameterSetCommon.DeleteCMCommodityFuse(m_CommodityCode);
                    m_Result = CommonParameterSetCommon.DeleteCMCommodityFuseAbout(m_CommodityCode);
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
                this.QueryCMCommodityFuse();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6803";
                string errMsg = "删除可交易商品_熔断失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 显示添加熔断_时间段标识UI

        /// <summary>
        /// 显示添加熔断_时间段标识UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFuseTimesection_Click(object sender, EventArgs e)
        {
            try
            {
                FuseTimesectionManageUI fuseTimesectionManageUI = new FuseTimesectionManageUI();

                if (m_CommodityCode != AppGlobalVariable.INIT_STRING)
                {
                    fuseTimesectionManageUI.m_CommodityCode = m_CommodityCode;
                    fuseTimesectionManageUI.ShowDialog();
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选中记录!");
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-6804";
                string errMsg = "显示添加熔断_时间段标识UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region  GridView的单击事件
        /// <summary>
        /// GridView的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdCommodityFuseResult_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                    this.gdCommodityFuseSelect.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

                if (this.gdCommodityFuseSelect != null && this.gdCommodityFuseSelect.FocusedRowHandle >= 0 &&
                    hi.RowHandle >= 0)
                {
                    int CurRow = this.gdCommodityFuseSelect.FocusedRowHandle;
                    if (CurRow < 0)
                    {
                        return;
                    }
                    DataRow _dr = this.gdCommodityFuseSelect.GetDataRow(CurRow);
                    m_CommodityCode = _dr["CommodityCode"].ToString();
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

        #region 取消按钮事件
        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearAll();
                this.cmbCommodityCode.Enabled = true;

            }
            catch
            {
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
            if (m_EditType == 1)
            {
                #region 添加操作

                try
                {
                    CM_CommodityFuse cM_CommodityFuse = new CM_CommodityFuse();
                    if (CommonParameterSetCommon.ExistsCommodityCode(this.cmbCommodityCode.Text))
                    {
                        ShowMessageBox.ShowInformation("此代码的熔断已存在!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.cmbCommodityCode.Text))
                    {
                        cM_CommodityFuse.CommodityCode = this.cmbCommodityCode.SelectedItem.ToString();
                    }
                    else
                    {
                        //cM_CommodityFuse.CommodityCode = AppGlobalVariable.INIT_STRING;
                        ShowMessageBox.ShowInformation("请选择代码!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txTriggeringScale.Text))
                    {
                        //cM_CommodityFuse.TriggeringScale = Convert.ToDecimal(this.txTriggeringScale.Text);
                        if (InputTest.DecimalTest(this.txTriggeringScale.Text))
                        {
                            cM_CommodityFuse.TriggeringScale = Convert.ToDecimal(this.txTriggeringScale.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入数字或小数点!");
                            return;
                        }
                    }
                    else
                    {
                        //cM_CommodityFuse.TriggeringScale = AppGlobalVariable.INIT_DECIMAL;
                        ShowMessageBox.ShowInformation("触发比例不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtFuseTimeOfDay.Text))
                    {
                        if (InputTest.intTest(this.txtFuseTimeOfDay.Text))
                        {
                            cM_CommodityFuse.FuseTimeOfDay = Convert.ToInt32(this.txtFuseTimeOfDay.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入整数!");
                            return;
                        }
                    }
                    else
                    {
                        // cM_CommodityFuse.FuseTimeOfDay = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("熔断次数不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtTriggeringDuration.Text))
                    {
                        //cM_CommodityFuse.TriggeringDuration = Convert.ToInt32(this.txtTriggeringDuration.Text);
                        if (InputTest.intTest(this.txtTriggeringDuration.Text))
                        {
                            cM_CommodityFuse.TriggeringDuration = Convert.ToInt32(this.txtTriggeringDuration.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入整数!");
                            return;
                        }
                    }
                    else
                    {
                        //cM_CommodityFuse.TriggeringDuration = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("触发持续时间限制不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtFuseDurationLimit.Text))
                    {
                        if (InputTest.intTest(this.txtFuseDurationLimit.Text))
                        {
                            cM_CommodityFuse.FuseDurationLimit = Convert.ToInt32(this.txtFuseDurationLimit.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入整数!");
                            return;
                        }
                    }
                    else
                    {
                        // cM_CommodityFuse.FuseDurationLimit = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("熔断持续时间限制不能为空!");
                        return;
                    }
                    m_Result = CommonParameterSetCommon.AddCMCommodityFuse(cM_CommodityFuse);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.QueryCMCommodityFuse();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-6801";
                    string errMsg = "添加可交易商品_熔断失败!";
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
                    CM_CommodityFuse cM_CommodityFuse = new CM_CommodityFuse();
                    if (m_CommodityCode == AppGlobalVariable.INIT_STRING)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    cM_CommodityFuse.CommodityCode = this.cmbCommodityCode.SelectedItem.ToString();
                    //cM_CommodityFuse.TriggeringScale = Convert.ToDecimal(this.txTriggeringScale.Text);
                    if (!string.IsNullOrEmpty(this.txTriggeringScale.Text))
                    {
                        if (InputTest.DecimalTest(this.txTriggeringScale.Text))
                        {
                            cM_CommodityFuse.TriggeringScale = Convert.ToDecimal(this.txTriggeringScale.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入数字或小数点!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("触发比例不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtFuseTimeOfDay.Text))
                    {
                        if (InputTest.intTest(this.txtFuseTimeOfDay.Text))
                        {
                            cM_CommodityFuse.FuseTimeOfDay = Convert.ToInt32(this.txtFuseTimeOfDay.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入整数!");
                            return;
                        }
                    }
                    else
                    {
                        //cM_CommodityFuse.FuseTimeOfDay = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("熔断次数不能为空!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtTriggeringDuration.Text))
                    {
                        if (InputTest.intTest(this.txtTriggeringDuration.Text))
                        {
                            cM_CommodityFuse.TriggeringDuration = Convert.ToInt32(this.txtTriggeringDuration.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入整数!");
                            return;
                        }
                    }
                    else
                    {
                        //cM_CommodityFuse.TriggeringDuration = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("触发持续时间限制不能为空!");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.txtFuseDurationLimit.Text))
                    {
                        if (InputTest.intTest(this.txtFuseDurationLimit.Text))
                        {
                            cM_CommodityFuse.FuseDurationLimit = Convert.ToInt32(this.txtFuseDurationLimit.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入整数!");
                            return;
                        }
                    }
                    else
                    {
                        //cM_CommodityFuse.FuseDurationLimit = AppGlobalVariable.INIT_INT;
                        ShowMessageBox.ShowInformation("熔断持续时间限制不能为空!");
                        return;
                    }
                    m_Result = CommonParameterSetCommon.UpdateCMCommodityFuse(cM_CommodityFuse);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.ClearAll();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                    this.QueryCMCommodityFuse();
                    this.cmbCommodityCode.Enabled = true;

                }
                catch (Exception ex)
                {
                    string errCode = "GL-6802";
                    string errMsg = "修改可交易商品_熔断失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }

                #endregion
            }
        }
        #endregion 确定按钮事件


    }
}