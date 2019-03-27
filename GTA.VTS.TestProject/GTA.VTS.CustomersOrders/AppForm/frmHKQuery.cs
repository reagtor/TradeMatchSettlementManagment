using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.HKCommonQuery;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// 作者：叶振东
    /// 时间：2010-03-02
    /// 描述：港股查询窗体
    /// </summary>
    public partial class frmHKQuery : MdiFormBase
    {
        #region 变量
        private WCFServices wcfLogic;

        /// <summary>
        /// 列表每页记录数
        /// </summary>
        private int pageSize;

        #endregion 变量

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public frmHKQuery()
        {
            pageSize = ServerConfig.QueryPageSize;
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
            cmbCurrencyType.SelectedIndex = 0;

            HKMessageLogic.OnEntrustSelected += new EntrustNoEventHandler(HKMessageLogic_OnEntrustSelected);
        }

        #endregion 构造函数

        #region 窗体事件
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmHKQuery_Load(object sender, EventArgs e)
        {
            InitPageControls();
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuHKQuery");
            #region 港股查询
            this.gpgQueryHKCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQueryHKCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHKHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHKTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHKTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryHK_HistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQHKHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnModifyQuery.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.tabPageHKHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageHKTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageHKTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageHKMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageHKTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageHKHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.tabPageHKHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");
            this.tabPageHKModifyOrder.Text = ResourceOperate.Instanse.GetResourceByKey("ModifyOrder");
            #region 港股详细信息
            this.lblHKEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber1.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber3.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber4.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblHKEntrustNumber5.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblstart.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblstart1.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblstart3.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblend.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.lblend1.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.lblend3.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.chkQueryHKHisTrade.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.chkModifyTime.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.chkHKDateTime.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.lblDo1.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblDo4.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblDo5.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblHKDo.Text = ResourceOperate.Instanse.GetResourceByKey("Do");
            this.lblHKCodes.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            #endregion 港股详细信息
            #endregion 港股查询
            #region 港股多语言
            #region 港股 资金dataGridView绑定
            for (int i = 0; i < this.dgHKCapital.ColumnCount; i++)
            {
                string KCapitalName = dgHKCapital.Columns[i].HeaderText;
                dgHKCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(KCapitalName);
            }
            #endregion 港股 资金dataGridView绑定dgHKCapital
            #region 港股 持仓DataGridView绑定
            for (int i = 0; i < this.dgHKHold.ColumnCount; i++)
            {
                string HKHoldName = dgHKHold.Columns[i].HeaderText;
                dgHKHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKHoldName);
            }
            #endregion 港股 持仓DataGridView绑定
            #region 现货 当日委托DataGridView绑定
            for (int i = 0; i < this.dgHKTodayEntrust.ColumnCount; i++)
            {
                string HKTodayEntrustName = dgHKTodayEntrust.Columns[i].HeaderText;
                dgHKTodayEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKTodayEntrustName);
            }
            #endregion 现货 当日委托DataGridView绑定
            #region 现货 当日成交DataGridView绑定
            for (int i = 0; i < this.dgHKTodayTrade.ColumnCount; i++)
            {
                string HKTodayTradeName = dgHKTodayTrade.Columns[i].HeaderText;
                dgHKTodayTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKTodayTradeName);
            }
            #endregion 现货 当日成交DataGridView绑定
            #region 港股 市值DataGridView绑定
            for (int i = 0; i < this.dgvMarketValue.ColumnCount; i++)
            {
                string HKMarketValueValueName = dgvMarketValue.Columns[i].HeaderText;
                dgvMarketValue.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKMarketValueValueName);
            }
            #endregion 港股 市值成交DataGridView绑定
            #region 现货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dgvTotalCapital.ColumnCount; i++)
            {
                string HKTotalCapitalName = dgvTotalCapital.Columns[i].HeaderText;
                dgvTotalCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKTotalCapitalName);
            }
            #endregion 现货 资金汇总DataGridView绑定
            #region 现货 历史委托DataGridView绑定
            for (int i = 0; i < this.dgvHK_historyEntrust.ColumnCount; i++)
            {
                string HKHistoryEntrustName = dgvHK_historyEntrust.Columns[i].HeaderText;
                dgvHK_historyEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKHistoryEntrustName);
            }
            #endregion 现货 历史委托DataGridView绑定
            #region 现货 历史成交DataGridView绑定
            for (int i = 0; i < this.dgvHK_HistoryTrade.ColumnCount; i++)
            {
                string HKHistoryTradeName = dgvHK_HistoryTrade.Columns[i].HeaderText;
                dgvHK_HistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(HKHistoryTradeName);
            }
            #endregion 现货 历史成交DataGridView绑定
            #endregion 港股多语言
        }
        /// <summary>
        /// 港股资金信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKCapital_Click(object sender, EventArgs e)
        {
            this.dgHKCapital.DataSource = new SortableBindingList<HK_CapitalAccountInfo>(wcfLogic.HKCapital);
            #region 币种
            dgHKCapital.Columns["HKCurrencyType"].DisplayIndex = 7;
            for (int i = 0; i < this.dgHKCapital.Rows.Count; i++)
            {
                string CurrencyType = dgHKCapital.Rows[i].Cells["HKTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKCapital.Rows[i].Cells["HKCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKCapital.Rows[i].Cells["HKCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKCapital.Rows[i].Cells["HKCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        /// <summary>
        /// 资金查询排序币种类型绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHKCapital_Sorted(object sender, EventArgs e)
        {
            #region 币种
            dgHKCapital.Columns["HKCurrencyType"].DisplayIndex = 7;
            for (int i = 0; i < this.dgHKCapital.Rows.Count; i++)
            {
                string CurrencyType = dgHKCapital.Rows[i].Cells["HKTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKCapital.Rows[i].Cells["HKCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKCapital.Rows[i].Cells["HKCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKCapital.Rows[i].Cells["HKCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        /// <summary>
        /// 港股持仓信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKHold_Click(object sender, EventArgs e)
        {
            this.dgHKHold.DataSource = new SortableBindingList<HK_AccountHoldInfo>(wcfLogic.HKHold);
            #region 币种
            dgHKHold.Columns["HKHoldCurrencyType"].DisplayIndex = 7;
            for (int i = 0; i < this.dgHKHold.Rows.Count; i++)
            {
                string CurrencyType = dgHKHold.Rows[i].Cells["HKCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKHold.Rows[i].Cells["HKHoldCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKHold.Rows[i].Cells["HKHoldCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKHold.Rows[i].Cells["HKHoldCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        /// <summary>
        /// 持仓排序币种显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHKHold_Sorted(object sender, EventArgs e)
        {
            #region 币种
            dgHKHold.Columns["HKHoldCurrencyType"].DisplayIndex = 7;
            for (int i = 0; i < this.dgHKHold.Rows.Count; i++)
            {
                string CurrencyType = dgHKHold.Rows[i].Cells["HKCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKHold.Rows[i].Cells["HKHoldCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKHold.Rows[i].Cells["HKHoldCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKHold.Rows[i].Cells["HKHoldCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        /// <summary>
        /// 港股当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKTodayEntrust_Click(object sender, EventArgs e)
        {
            BindHKTodayEntrustList();
            pageControlHK_TodayEntrust.Visible = true;
            pageControlHK_TodayEntrust.BindData();
        }
        /// <summary>
        /// 当日委托排列列的翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHKTodayEntrust_Sorted(object sender, EventArgs e)
        {
            dgHKTodayEntrust.Columns["HKTEBuySell"].DisplayIndex = 13;
            dgHKTodayEntrust.Columns["HKTECurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgHKTodayEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgHKTodayEntrust.Rows[i].Cells["HKTodayEnerustBuySellTypeID"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEBuySell"].Value = "买";
                }
                else
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgHKTodayEntrust.Rows[i].Cells["HKTodayEntrustCurrencyTypeID"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTECurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = dgHKTodayEntrust.Rows[i].Cells["HKTodayEntrustTradeUnitID"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dgHKTodayEntrust.Rows[i].Cells["HKTodayEntrustOrderStatusID"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }
        /// <summary>
        /// 港股当日成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHKTodayTrade_Click(object sender, EventArgs e)
        {
            BindHKTodayTradeList();
            pageControlHK_TodayTrade.Visible = true;
            pageControlHK_TodayTrade.BindData();
        }

        /// <summary>
        /// 港股历史委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHK_HistoryEntrust_Click(object sender, EventArgs e)
        {
            BindHKHistoryEntrustList();
            pageControlHK_HistoryEntrust.Visible = true;
            pageControlHK_HistoryEntrust.BindData();
        }

        /// <summary>
        /// 历史委托列表中列的绑定翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHK_historyEntrust_Sorted(object sender, EventArgs e)
        {
            dgvHK_historyEntrust.Columns["HKHEBuySell"].DisplayIndex = 13;
            dgvHK_historyEntrust.Columns["HKHECurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgvHK_historyEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn129"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEBuySell"].Value = "买";
                }
                else
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn142"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHECurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn157"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn151"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 港股历史成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQHKHistoryTrade_Click(object sender, EventArgs e)
        {
            BindHKHistoryTradeList();
            pageControlHK_HistoryTrade.Visible = true;
            pageControlHK_HistoryTrade.BindData();
        }

        /// <summary>
        ///  改单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyQuery_Click(object sender, EventArgs e)
        {
            dgvModifyList.AutoGenerateColumns = false;
            errPro.Clear();
            DateTime? start = null;
            DateTime? end = null;
            if (chkModifyTime.Checked)
            {
                start = dtpModifyStart.Value;
                end = dtpModifyEnd.Value;
            }
            string strMessage = "";
            if (cmbQueryType.SelectedIndex == -1)
            {
                cmbQueryType.SelectedIndex = 0;
            }
            int selectType = cmbQueryType.SelectedIndex;
            List<HK_HistoryModifyOrderRequestInfo> list = wcfLogic.QueryHKModifyOrderRequest(txtModifyEntrustNumber.Text, start, end, ref strMessage, selectType);
            if (list == null)
            {
                list = new List<HK_HistoryModifyOrderRequestInfo>();
            }

            dgvModifyList.DataSource = list;

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(btnModifyQuery, strMessage);
            }
        }

        /// <summary>
        /// 港股综合查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryMarketValue_Click(object sender, EventArgs e)
        {
            dgvMarketValue.AutoGenerateColumns = false;
            string password = ServerConfig.PassWord;
            string mess = "";
            dgvMarketValue.DataSource = wcfLogic.QueryHKHoldMarketValue(txtMarketValuCode.Text.Trim(), password);
            dgvMarketValue.DataSource = wcfLogic.QueryMarketValueHKHold(txtMarketValuCode.Text.Trim(), ref mess);

        }

        /// <summary>
        /// 现货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTotalCapital_Click(object sender, EventArgs e)
        {
            string msg = "";
            List<HKCapitalEntity> list = new List<HKCapitalEntity>();
            int x = cmbCurrencyType.SelectedIndex;
            HKCapitalEntity entry = new HKCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryHKTotalCapital(Types.CurrencyType.RMB, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryHKTotalCapital(Types.CurrencyType.HK, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryHKTotalCapital(Types.CurrencyType.US, ref msg);
            }
            //   HKCapitalEntity entry = wcfLogic.QueryHKTotalCapital((Types.CurrencyType)cmbCurrencyType.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvTotalCapital.DataSource = list;

        }

        /// <summary>
        /// 获取当日委托的委托单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHKTodayEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgHKTodayEntrust.SelectedRows)
            {
                HK_TodayEntrustInfo TodayEntrust = row.DataBoundItem as HK_TodayEntrustInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtQueryHKEnNO.Text = TodayEntrust.EntrustNumber;
            }
        }

        /// <summary>
        /// 获取当日成交单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHKTodayTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgHKTodayTrade.SelectedRows)
            {
                HK_TodayTradeInfo TodayTrade = row.DataBoundItem as HK_TodayTradeInfo;
                if (TodayTrade == null)
                {
                    return;
                }
                this.txtQueryHKTradeNO.Text = TodayTrade.EntrustNumber;
            }
        }

        /// <summary>
        /// 获取历史委托单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHK_historyEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvHK_historyEntrust.SelectedRows)
            {
                HK_HistoryEntrustInfo HistoryEntrust = row.DataBoundItem as HK_HistoryEntrustInfo;
                if (HistoryEntrust == null)
                {
                    return;
                }
                this.txtHKHistroyEnNo.Text = HistoryEntrust.EntrustNumber;
            }
        }

        /// <summary>
        /// 获取历史成交单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHK_HistoryTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvHK_HistoryTrade.SelectedRows)
            {
                HK_HistoryTradeInfo HistoryTrade = row.DataBoundItem as HK_HistoryTradeInfo;
                if (HistoryTrade == null)
                {
                    return;
                }
                this.txtHKHistroyTradeNo.Text = HistoryTrade.EntrustNumber;
            }
        }

        /// <summary>
        /// 获取港股市值的股票代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMarketValue_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            foreach (DataGridViewRow row in this.dgvMarketValue.SelectedRows)
            {
                HKMarketValue hkMarketValue = row.DataBoundItem as HKMarketValue;
                if (hkMarketValue == null)
                {
                    return;
                }
                this.txtMarketValuCode.Text = hkMarketValue.Code;
            }
        }

        /// <summary>
        /// 今日成交查询翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlHK_TodayTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindHKTodayTradeList();
        }

        /// <summary>
        /// 今日委托查询翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlHK_TodayEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindHKTodayEntrustList();
        }

        /// <summary>
        /// 历史委托查询翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlHK_HistoryEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindHKHistoryEntrustList();
        }

        /// <summary>
        /// 历史成交查询翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlHK_HistoryTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindHKHistoryTradeList();
        }

        /// <summary>
        /// 委托单号获取事件处理
        /// </summary>
        /// <param name="entrustNumber"></param>
        private void HKMessageLogic_OnEntrustSelected(string entrustNumber)
        {
            txtQueryHKEnNO.Text = entrustNumber;
            txtQueryHKTradeNO.Text = entrustNumber;
        }

        #endregion 窗体事件

        #region 私有方法

        /// <summary>
        /// 初始化翻页控件
        /// </summary>
        private void InitPageControls()
        {
            pageControlHK_TodayEntrust.PageSize = pageSize;
            pageControlHK_TodayEntrust.CurrentPage = 1;
            pageControlHK_TodayEntrust.RecordsCount = 1;
            pageControlHK_TodayEntrust.OnPageChanged += new EventHandler(pageControlHK_TodayEntrust_OnPageChanged);
            pageControlHK_TodayEntrust.Visible = false;

            pageControlHK_TodayTrade.PageSize = pageSize;
            pageControlHK_TodayTrade.CurrentPage = 1;
            pageControlHK_TodayTrade.RecordsCount = 1;
            pageControlHK_TodayTrade.OnPageChanged += new EventHandler(pageControlHK_TodayTrade_OnPageChanged);
            pageControlHK_TodayTrade.Visible = false;

            pageControlHK_HistoryTrade.PageSize = pageSize;
            pageControlHK_HistoryTrade.CurrentPage = 1;
            pageControlHK_HistoryTrade.RecordsCount = 1;
            pageControlHK_HistoryTrade.OnPageChanged += new EventHandler(pageControlHK_HistoryTrade_OnPageChanged);
            pageControlHK_HistoryTrade.Visible = false;

            pageControlHK_HistoryEntrust.PageSize = pageSize;
            pageControlHK_HistoryEntrust.CurrentPage = 1;
            pageControlHK_HistoryEntrust.RecordsCount = 1;
            pageControlHK_HistoryEntrust.OnPageChanged += new EventHandler(pageControlHK_HistoryEntrust_OnPageChanged);
            pageControlHK_HistoryEntrust.Visible = false;
        }


        /// <summary>
        /// 绑定今日委托数据列表
        /// </summary>
        private void BindHKTodayEntrustList()
        {
            int icount;
            string msg;
            CurrentQueryValue.QueryHKEnNO = txtQueryHKEnNO.Text;
            List<HK_TodayEntrustInfo> list = wcfLogic.QueryHKTodayEntrust(out icount, ServerConfig.HKCapitalAccount, txtQueryHKEnNO.Text, pageControlHK_TodayEntrust.CurrentPage, pageControlHK_TodayEntrust.PageSize, out msg);
            pageControlHK_TodayEntrust.RecordsCount = icount;
            //this.dgHKTodayEntrust.DataSource = new SortableBindingList<HK_TodayEntrustInfo>(wcfLogic.HKTodayEntrust);
            this.dgHKTodayEntrust.DataSource = new SortableBindingList<HK_TodayEntrustInfo>(list);
            dgHKTodayEntrust.Columns["HKTEBuySell"].DisplayIndex = 13;
            dgHKTodayEntrust.Columns["HKTECurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgHKTodayEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgHKTodayEntrust.Rows[i].Cells["HKTodayEnerustBuySellTypeID"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEBuySell"].Value = "买";
                }
                else
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgHKTodayEntrust.Rows[i].Cells["HKTodayEntrustCurrencyTypeID"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTECurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = dgHKTodayEntrust.Rows[i].Cells["HKTodayEntrustTradeUnitID"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dgHKTodayEntrust.Rows[i].Cells["HKTodayEntrustOrderStatusID"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dgHKTodayEntrust.Rows[i].Cells["HKTEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 当日成交列表中排序列的数据绑定翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHKTodayTrade_Sorted(object sender, EventArgs e)
        {
            dgHKTodayTrade.Columns["HKTTBuySell"].DisplayIndex = 13;
            dgHKTodayTrade.Columns["HKTTCurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgHKTodayTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTBuySell"].Value = "买";
                }
                else
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTCurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeTradeUnitId"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeTradeTypeId"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 绑定今日成交数据列表
        /// </summary>
        private void BindHKTodayTradeList()
        {
            int icount;
            string msg;

            CurrentQueryValue.QueryHKTradeNO = txtQueryHKTradeNO.Text;
            //this.wCFLogicBindingSource11.DataSource =
            //    new SortableBindingList<HK_TodayTradeInfo>(wcfLogic.HKTodayTrade);
            //this.wCFLogicBindingSource11.ResetBindings(false);

            List<HK_TodayTradeInfo> list = wcfLogic.QueryHKTodayTrade(out icount, ServerConfig.HKCapitalAccount, txtQueryHKTradeNO.Text, pageControlHK_TodayTrade.CurrentPage, pageControlHK_TodayTrade.PageSize, out msg);
            pageControlHK_TodayTrade.RecordsCount = icount;
            if (list == null)
            {
                list = new List<HK_TodayTradeInfo>();
            }
            SortableBindingList<HK_TodayTradeInfo> items = new SortableBindingList<HK_TodayTradeInfo>(list);
            this.dgHKTodayTrade.DataSource = items;

            //this.dgHKTodayTrade.DataSource = new SortableBindingList<HK_TodayTradeInfo>(wcfLogic.HKTodayTrade);
            dgHKTodayTrade.Columns["HKTTBuySell"].DisplayIndex = 13;
            dgHKTodayTrade.Columns["HKTTCurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgHKTodayTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTBuySell"].Value = "买";
                }
                else
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTCurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeTradeUnitId"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dgHKTodayTrade.Rows[i].Cells["HKTodayTradeTradeTypeId"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dgHKTodayTrade.Rows[i].Cells["HKTTTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 绑定历史成交查询列表
        /// </summary>
        private void BindHKHistoryTradeList()
        {
            int icount;

            errPro.Clear();
            DateTime? start = null;
            DateTime? end = null;
            if (chkQueryHKHisTrade.Checked)
            {
                start = dtpDateHKHisStart.Value;
                end = dtpDateHKHistEnd.Value;
            }
            string strMessage = "";
            //List<HK_HistoryTradeInfo> list = wcfLogic.QueryHKHisotryTrade(txtHKHistroyTradeNo.Text, start, end, ref strMessage);
            //if (list == null)
            //{
            //    list = new List<HK_HistoryTradeInfo>();
            //}
            List<HK_HistoryTradeInfo> list = wcfLogic.QueryHKHisotryTrade(out icount, ServerConfig.HKCapitalAccount, txtHKHistroyTradeNo.Text, pageControlHK_HistoryTrade.CurrentPage, pageControlHK_HistoryTrade.PageSize, start, end, out strMessage);
            pageControlHK_HistoryTrade.RecordsCount = icount;
            dgvHK_HistoryTrade.DataSource = new SortableBindingList<HK_HistoryTradeInfo>(list);

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(btnQHKHistoryTrade, strMessage);
            }

            dgvHK_HistoryTrade.Columns["HKHTBuySell"].DisplayIndex = 13;
            dgvHK_HistoryTrade.Columns["HKHTCurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgvHK_HistoryTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgvHK_HistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn62"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTBuySell"].Value = "买";
                }
                else
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgvHK_HistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn66"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTCurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = dgvHK_HistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn136"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dgvHK_HistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn135"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dgvHK_HistoryTrade.Rows[i].Cells["HKHTTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 绑定历史委托查询列表
        /// </summary>
        private void BindHKHistoryEntrustList()
        {
            int icount;
            errPro.Clear();
            DateTime? start = null;
            DateTime? end = null;
            if (chkHKDateTime.Checked)
            {
                start = dtpDateHKStart.Value;
                end = dtpDateHKEnd.Value;
            }
            string strMessage = "";
            //List<HK_HistoryEntrustInfo> list = wcfLogic.QueryHKHisotryEntrust(txtHKHistroyEnNo.Text, start, end, ref strMessage);
            //if (list == null)
            //{
            //    list = new List<HK_HistoryEntrustInfo>();
            //}
            List<HK_HistoryEntrustInfo> list = wcfLogic.QueryHKHisotryEntrust(out icount, ServerConfig.HKCapitalAccount, txtHKHistroyTradeNo.Text, pageControlHK_HistoryTrade.CurrentPage, pageControlHK_HistoryTrade.PageSize, start, end, out strMessage);
            pageControlHK_HistoryEntrust.RecordsCount = icount;
            dgvHK_historyEntrust.DataSource = new SortableBindingList<HK_HistoryEntrustInfo>(list);

            if (!string.IsNullOrEmpty(strMessage))
            {
                errPro.SetError(btnQueryHK_HistoryEntrust, strMessage);
            }

            dgvHK_historyEntrust.Columns["HKHEBuySell"].DisplayIndex = 13;
            dgvHK_historyEntrust.Columns["HKHECurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgvHK_historyEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn129"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEBuySell"].Value = "买";
                }
                else
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn142"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHECurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn157"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dgvHK_historyEntrust.Rows[i].Cells["dataGridViewTextBoxColumn151"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dgvHK_historyEntrust.Rows[i].Cells["HKHEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        #endregion

    }
}