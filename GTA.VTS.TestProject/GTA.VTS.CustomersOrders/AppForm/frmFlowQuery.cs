using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.DoCommonQuery;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// 作者：叶振东
    /// 时间：2010-03-02
    /// 描述：资金流水查询窗体
    /// </summary>
    public partial class frmFlowQuery : MdiFormBase
    {
        private WCFServices wcfLogic;
        public frmFlowQuery()
        {
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
        }
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuFlowQuery");
            #region 资金流水查询
            for (int i = 0; i < this.dagCapitalFlow.ColumnCount; i++)
            {
                string CapitalFlowName = dagCapitalFlow.Columns[i].HeaderText;
                dagCapitalFlow.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(CapitalFlowName);
            }
            #endregion 资金流水查询
            #region 资金流水查询
            this.butCapitalFlowQuery.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.grpQueryTerm.Text = ResourceOperate.Instanse.GetResourceByKey("grpQueryTerm");
            this.lblaccountType.Text = ResourceOperate.Instanse.GetResourceByKey("lblaccountType");
            this.lblCurrencyType.Text = ResourceOperate.Instanse.GetResourceByKey("lblCurrencyType");
            this.lblCapitalFlowType.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapitalFlowType");
            this.lblCapitalAmount.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapitalAmount");
            this.lblStartTime.Text = ResourceOperate.Instanse.GetResourceByKey("lblStartTime");
            this.lblEndTime.Text = ResourceOperate.Instanse.GetResourceByKey("lblEndTime");
            this.grpQueryresult.Text = ResourceOperate.Instanse.GetResourceByKey("grpQueryresult");
            #endregion 资金流水查询
        }

        private void frmFlowQuery_Load(object sender, EventArgs e)
        {
            this.dateTimePickerstartTime.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerstartTime.CustomFormat = "   ";

            this.dateTimePickerendTime.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerendTime.CustomFormat = "   ";
            cmbaccountType.SelectedIndex = 0;
            cmbCurrType.SelectedIndex = 0;
            cmbTransferType.SelectedIndex = 3;
        }

        #region 查询资金流水
        /// <summary>
        ///  查询资金流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCapitalFlowQuery_Click(object sender, EventArgs e)
        {
            #region QueryUA_CapitalFlowFilter类的查询条件
            QueryUA_CapitalFlowFilter CapitalFlowFilter = new QueryUA_CapitalFlowFilter();
            //转账货币类型
            string currencyType = this.cmbCurrType.SelectedIndex.ToString();
            if (currencyType.Equals("0"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.ALL;
            }
            else if (currencyType.Equals("1"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.RMB;
            }
            else if (currencyType.Equals("2"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.HK;
            }
            else if (currencyType.Equals("3"))
            {
                CapitalFlowFilter.CurrencyType = GTA.VTS.CustomersOrders.DoCommonQuery.QueryTypeQueryCurrencyType.US;
            }
            //转账金额
            string TransferAmount = this.txtTransferAmount.Text.ToString();
            decimal Amount = 0;
            if (!string.IsNullOrEmpty(TransferAmount))
            {
                try
                {
                    Amount = decimal.Parse(TransferAmount);
                    CapitalFlowFilter.CapitalAmount = Amount;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
            //转账时间
            //this.dateTimePickerstartTime.Text = "";
            string startTime = this.dateTimePickerstartTime.Value.ToString("yyyy-MM-dd");
            string endTime = this.dateTimePickerendTime.Value.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                try
                {
                    CapitalFlowFilter.StartTime = DateTime.Parse(startTime);
                    CapitalFlowFilter.EndTime = DateTime.Parse(endTime);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
            //转账类型
            string TransferType = this.cmbTransferType.SelectedIndex.ToString();
            if (TransferType.Equals("0"))
            {
                CapitalFlowFilter.CapitalFlowType = 1;
            }
            else if (TransferType.Equals("1"))
            {
                CapitalFlowFilter.CapitalFlowType = 2;
            }
            else if (TransferType.Equals("2"))
            {
                CapitalFlowFilter.CapitalFlowType = 3;
            }
            #endregion QueryUA_CapitalFlowFilter类的查询条件
            #region 帐号类型查询条件
            string accountType = this.cmbaccountType.SelectedIndex.ToString();
            int account = 0;
            if (accountType.Equals("0"))
            {
                account = 1;
            }
            else if (accountType.Equals("1"))
            {
                account = 2;
            }
            else if (accountType.Equals("2"))
            {
                account = 3;
            }
            else if (accountType.Equals("3"))
            {
                account = 4;
            }
            else if (accountType.Equals("4"))
            {
                account = 5;
            }
            else if (accountType.Equals("5"))
            {
                account = 6;
            }
            else if (accountType.Equals("6"))
            {
                account = 7;
            }
            else if (accountType.Equals("7"))
            {
                account = 8;
            }
            else if (accountType.Equals("8"))
            {
                account = 9;
            }
            else
            {
                account = 0;
            }
            #endregion 帐号类型查询条件
            string msg = "";
            this.dagCapitalFlow.DataSource = wcfLogic.QueryCapitalFlow(out msg, CapitalFlowFilter, account);
            #region 币种和转账列的翻译
            for (int i = 0; i < this.dagCapitalFlow.Rows.Count; i++)
            {
                string CurrencyType = dagCapitalFlow.Rows[i].Cells["CapitalFlowTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagCapitalFlow.Rows[i].Cells["CurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagCapitalFlow.Rows[i].Cells["CurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagCapitalFlow.Rows[i].Cells["CurrencyType"].Value = "美元";
                }

                string TransferTypes = dagCapitalFlow.Rows[i].Cells["CapitalFlowTransferTypeLogo"].Value.ToString();
                if (TransferTypes.Equals("1"))
                {
                    dagCapitalFlow.Rows[i].Cells["TransferTypeLogo"].Value = "自由转账";
                }
                else if (TransferTypes.Equals("2"))
                {
                    dagCapitalFlow.Rows[i].Cells["TransferTypeLogo"].Value = "分红转账";
                }
                else if (TransferTypes.Equals("3"))
                {
                    dagCapitalFlow.Rows[i].Cells["TransferTypeLogo"].Value = "追加资金";
                }
            }
            #endregion
        }
        #endregion 查询资金流水
        /// <summary>
        /// 开始时间值发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePickerstartTime_ValueChanged(object sender, EventArgs e)
        {
            //this.dateTimePickerstartTime.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerstartTime.CustomFormat = null;
        }
        /// <summary>
        /// 结束时间值发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePickerendTime_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePickerendTime.CustomFormat = null;
        }
    }
}
