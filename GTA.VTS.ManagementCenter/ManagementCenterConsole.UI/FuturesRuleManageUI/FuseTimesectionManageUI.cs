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
    /// 描述：熔断_时间段标识管理窗体 错误编码范围:6820-6839
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// </summary>
    public partial class FuseTimesectionManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public FuseTimesectionManageUI()
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
        /// 时间段标识ID
        /// </summary>
        private int m_TimesectionID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 商品代码
        /// </summary>
        public string m_CommodityCode = AppGlobalVariable.INIT_STRING;

        /// <summary>
        /// 商品代码
        /// </summary>
        public string CommodityCode
        {
            set { m_CommodityCode = value; }
            get { return m_CommodityCode; }
        }

        #endregion

        //================================  私有  方法 ================================

        #region 根据商品代码，获取熔断_时间段标识

        /// <summary>
        /// 根据商品代码，获取熔断_时间段标识
        /// </summary>
        /// <returns></returns>
        private bool QueryCMFuseTimesection()
        {
            try
            {
                if (m_CommodityCode != AppGlobalVariable.INIT_STRING)
                {
                    DataSet _dsCMFuseTimesection =
                        CommonParameterSetCommon.GetCMFuseTimesectionByCommodityCode(m_CommodityCode);
                    DataTable _dtCMFuseTimesection;
                    if (_dsCMFuseTimesection == null || _dsCMFuseTimesection.Tables[0].Rows.Count == 0)
                    {
                        _dtCMFuseTimesection = new DataTable();
                    }
                    else
                    {
                        _dtCMFuseTimesection = _dsCMFuseTimesection.Tables[0];
                    }
                    this.gdFuseTimesectionResult.DataSource = _dtCMFuseTimesection;
                }
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6824";
                string errMsg = "根据商品代码，获取熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 修改熔断_时间段标识

        /// <summary>
        /// 修改熔断_时间段标识
        /// </summary>
        private void ModifyCMFuseTimesection()
        {
            try
            {
                //btnModify.Enabled = true;
                if (this.gdFuseTimesectionSelect != null && this.gdFuseTimesectionSelect.DataSource != null &&
                    this.gdFuseTimesectionSelect.RowCount > 0 && this.gdFuseTimesectionSelect.FocusedRowHandle >= 0)
                {
                    btnModify.Enabled = true;
                    DataRow _dr = this.gdFuseTimesectionSelect.GetDataRow(this.gdFuseTimesectionSelect.FocusedRowHandle);
                    m_TimesectionID = Convert.ToInt32(_dr["TimesectionID"]);
                    m_CommodityCode = _dr["CommodityCode"].ToString();
                    this.tmStartTime.EditValue = Convert.ToDateTime(_dr["StartTime"]);
                    this.tmEndTime.EditValue = Convert.ToDateTime(_dr["EndTime"]);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6825";
                string errMsg = "修改熔断_时间段标识失败!";
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
            this.tmStartTime.EditValue = "00:00";
            this.tmEndTime.EditValue = "00:00";
        }

        #endregion

        //================================  事件 ================================

        #region  熔断_时间段标识管理窗体加载事件 FuseTimesectionManageUI_Load
        /// <summary>
        /// 熔断_时间段标识管理窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuseTimesectionManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                this.labCommodityCode.Text = m_CommodityCode;
                this.QueryCMFuseTimesection();
                //初次加载时，更新按钮禁用
                //btnModify.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6820";
                string errMsg = " 熔断_时间段标识管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 添加熔断_时间段标识

        /// <summary>
        /// 添加熔断_时间段标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                CM_FuseTimesection cM_FuseTimesection = new CM_FuseTimesection();
                if (m_CommodityCode != AppGlobalVariable.INIT_STRING)
                {
                    cM_FuseTimesection.CommodityCode = m_CommodityCode;
                }
                else
                {
                    ShowMessageBox.ShowInformation("商品代码不能为空!");
                    return;
                }

                if (!string.IsNullOrEmpty(this.tmStartTime.Text))
                {
                    cM_FuseTimesection.StartTime = Convert.ToDateTime(this.tmStartTime.EditValue);
                }
                else
                {
                    cM_FuseTimesection.StartTime = AppGlobalVariable.INIT_DATETIME;
                }
                if (!string.IsNullOrEmpty(this.tmEndTime.Text))
                {
                    if (!(Convert.ToDateTime(this.tmEndTime.EditValue) < Convert.ToDateTime(this.tmStartTime.EditValue)))
                    {
                        cM_FuseTimesection.EndTime = Convert.ToDateTime(this.tmEndTime.EditValue);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("结束时间不能小于开始时间!");
                        return;
                    }
                }
                else
                {
                    cM_FuseTimesection.EndTime = AppGlobalVariable.INIT_DATETIME;
                }
                string msg = string.Empty; //错误信息提示变量
                //int Result = CommonParameterSetCommon.AddCMFuseTimesection(cM_FuseTimesection);
                int Result = CommonParameterSetCommon.AddCMFuseTimesection(cM_FuseTimesection, ref msg);

                if (Result != AppGlobalVariable.INIT_INT)
                {
                    ShowMessageBox.ShowInformation("添加成功!");
                    this.ClearAll();
                    this.QueryCMFuseTimesection();
                }
                else
                {
                    ShowMessageBox.ShowInformation(msg);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6821";
                string errMsg = " 添加熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 修改熔断_时间段标识

        /// <summary>
        /// 修改熔断_时间段标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                CM_FuseTimesection cM_FuseTimesection = new CM_FuseTimesection();
                if (m_TimesectionID == AppGlobalVariable.INIT_INT)
                {
                    ShowMessageBox.ShowInformation("请选择更新数据!");
                    return;
                }
                cM_FuseTimesection.TimesectionID = m_TimesectionID;
                cM_FuseTimesection.CommodityCode = m_CommodityCode;
                cM_FuseTimesection.StartTime = Convert.ToDateTime(this.tmStartTime.EditValue);
                cM_FuseTimesection.EndTime = Convert.ToDateTime(this.tmEndTime.EditValue);
                string msg = string.Empty; //错误信息提示变量
                // m_Result = CommonParameterSetCommon.UpdateCMFuseTimesection(cM_FuseTimesection);
                m_Result = CommonParameterSetCommon.UpdateCMFuseTimesection(cM_FuseTimesection, ref msg);

                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("修改成功!");
                    this.ClearAll();
                    //btnAdd.Enabled = true;
                    //btnModify.Enabled = false;
                }
                else
                {
                    //MessageBox.Show("修改失败!", "系统提示");
                    ShowMessageBox.ShowInformation(msg);
                }
                this.QueryCMFuseTimesection();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6822";
                string errMsg = " 修改熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 修改熔断_时间段标识的GridView双击事件

        /// <summary>
        ///  修改熔断_时间段标识的GridView双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdFuseTimesectionResult_DoubleClick(object sender, EventArgs e)
        {
            //btnAdd.Enabled = false;
            this.ModifyCMFuseTimesection();
        }

        #endregion

        #region 删除熔断_时间段标识

        /// <summary>
        /// 删除熔断_时间段标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdFuseTimesectionSelect.GetDataRow(this.gdFuseTimesectionSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["TimesectionID"])))
                {
                    m_TimesectionID = Convert.ToInt32(_dr["TimesectionID"]);
                }
                else
                {
                    m_TimesectionID = AppGlobalVariable.INIT_INT;
                }
                if (m_TimesectionID != AppGlobalVariable.INIT_INT)
                {
                    m_Result = CommonParameterSetCommon.DeleteCMFuseTimesection(m_TimesectionID);
                }
                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_TimesectionID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryCMFuseTimesection();
            }
            catch (Exception ex)
            {
                string errCode = "GL-6823";
                string errMsg = "删除熔断_时间段标识失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion
    }
}