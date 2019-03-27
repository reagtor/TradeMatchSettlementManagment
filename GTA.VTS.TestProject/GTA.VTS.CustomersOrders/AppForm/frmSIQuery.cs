using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.DoCommonQuery;
using GTA.VTS.Common.CommonObject;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// Desc:股指期货查询窗体
    /// Create by:董鹏
    /// Create Data:2010-03-02
    /// </summary>
    public partial class frmSIQuery : MdiFormBase
    {
        #region 变量

        /// <summary>
        /// WCF服务访问对象
        /// </summary>
        WCFServices wcfLogic;

        /// <summary>
        /// 列表每页记录数
        /// </summary>
        private int pageSize;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmSIQuery()
        {
            pageSize = ServerConfig.QueryPageSize;
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();

            GZQHMessageLogic.OnEntrustSelected += new EntrustNoEventHandler(GZQHMessageLogic_OnEntrustSelected);
        }

        #endregion

        #region 窗体事件

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSIQuery_Load(object sender, EventArgs e)
        {
            InitPageControls();

            cmbQHCureny.SelectedIndex = 0;
            cmbQHFlowCury.SelectedIndex = 0;
            this.txtPwd.Text = ServerConfig.PassWord;
        }

        /// <summary>
        /// 股指期货资金信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZCapital_Click(object sender, EventArgs e)
        {
            this.dgQHCapital.DataSource = new SortableBindingList<QH_CapitalAccountTableInfo>(wcfLogic.GZQHCapital);
            dgQHCapital.Columns["GZQHCurrency"].DisplayIndex = 9;
            dgQHCapital.Columns["GZQHCapitalAccountLogo"].DisplayIndex = 10;
            #region 币种
            for (int i = 0; i < this.dgQHCapital.Rows.Count; i++)
            {
                string CurrencyType = dgQHCapital.Rows[i].Cells["GZQHTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgQHCapital.Rows[i].Cells["GZQHCurrency"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgQHCapital.Rows[i].Cells["GZQHCurrency"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgQHCapital.Rows[i].Cells["GZQHCurrency"].Value = "美元";
                }
            }
            #endregion
        }

        /// <summary>
        ///  股指期货持仓查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZHold_Click(object sender, EventArgs e)
        {
            this.dgvGZQHHold.DataSource = new SortableBindingList<QH_HoldAccountTableInfo>(wcfLogic.GZQHHold);
            #region 列的数据绑定
            dgvGZQHHold.Columns["GZQHCurrencyType"].DisplayIndex = 9;
            for (int i = 0; i < this.dgvGZQHHold.Rows.Count; i++)
            {
                string CurrencyType = dgvGZQHHold.Rows[i].Cells["GZQHHoldTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHCurrencyType"].Value = "美元";
                }
                string BuySellType = dgvGZQHHold.Rows[i].Cells["GZQHHoldBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHBuySellType"].Value = "买";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHBuySellType"].Value = "卖";
                }
            }
            #endregion
        }

        /// <summary>
        /// 股指期货当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZTodayEntrust_Click(object sender, EventArgs e)
        {
            BindGZQHTodayEntrustList();
            pageControlGZQH_TodayEntrust.Visible = true;
            pageControlGZQH_TodayEntrust.BindData();
        }
        /// <summary>
        /// 股指期货当日委托查询
        /// </summary>
        private void BindGZQHTodayEntrustList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtSITodayEntrustQueryNo.Text.Trim();
            int icount;
            string msg = "";

            List<QH_TodayEntrustTableInfo> list = wcfLogic.QueryQHTodayEntrust(out icount, pageControlGZQH_TodayEntrust.CurrentPage, pageControlGZQH_TodayEntrust.PageSize, ServerConfig.GZQHCapitalAccount, 6, ref msg);
            pageControlGZQH_TodayEntrust.RecordsCount = icount;
            dgvGZQHToday.DataSource = list;

            dgvGZQHToday.Columns["GZQHTECurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dgvGZQHToday.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEBuySell"].Value = "买";
                }
                else
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTECurrencyType"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustOpenCloseTypeId"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEOpenClose"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEOpenClose"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEOpenClose"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustTradeUnitId"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustOrderStatusId"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 股指期货当日交易量查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryGZTodayTrade_Click(object sender, EventArgs e)
        {
            BindGZQHTodayTradeList();
            pageControlGZQH_TodayTrade.Visible = true;
            pageControlGZQH_TodayTrade.BindData();
        }

        /// <summary>
        /// 股指期货当日交易量查询
        /// </summary>
        private void BindGZQHTodayTradeList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtSITodayTradeQueryNo.Text.Trim();
            int icount;
            string msg = "";

            List<QH_TodayTradeTableInfo> list = wcfLogic.QueryGZQHTodayTrade(out icount, pageControlGZQH_TodayTrade.CurrentPage, pageControlGZQH_TodayTrade.PageSize, ServerConfig.GZQHCapitalAccount, 6, ref msg);
            pageControlGZQH_TodayTrade.RecordsCount = icount;
            dagGZQHTodayTrade.DataSource = list;


            dagGZQHTodayTrade.Columns["GZQHTTCurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dagGZQHTodayTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagGZQHTodayTrade.Rows[i].Cells["GZQHTodayTradeBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTBuySell"].Value = "买";
                }
                else
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagGZQHTodayTrade.Rows[i].Cells["GZQHTodayTradeCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTCurrencyType"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagGZQHTodayTrade.Rows[i].Cells["GZQHTodayTradeOpenCloseTypeId"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTOpenClose"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTOpenClose"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTOpenClose"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagGZQHTodayTrade.Rows[i].Cells["GZQHTodayTradeTradeUnitId"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTUnit"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTUnit"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTUnit"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dagGZQHTodayTrade.Rows[i].Cells["GZQHTodayTradeTradeTypeId"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dagGZQHTodayTrade.Rows[i].Cells["GZQHTTTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 期货查询市值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryMarketValu_Click(object sender, EventArgs e)
        {
            dgQHMarketValue.AutoGenerateColumns = false;
            string mess = "";
            dgQHMarketValue.DataSource = wcfLogic.QueryMarketValueQHHold(txtQHMarketValue.Text.Trim(), 6, ref mess);
        }

        /// <summary>
        ///  股指期货当日金额查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQHQueryTotalCapital_Click(object sender, EventArgs e)
        {
            dgvQHTotalCapital.AutoGenerateColumns = false;
            string msg = "";
            List<FuturesCapitalEntity> list = new List<FuturesCapitalEntity>();
            int x = cmbQHCureny.SelectedIndex;
            FuturesCapitalEntity entry = new FuturesCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.RMB, 6, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.HK, 6, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.US, 6, ref msg);
            }
            //FuturesCapitalEntity entry = wcfLogic.QueryQHTotalCapital((Types.CurrencyType)cmbQHCureny.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvQHTotalCapital.DataSource = list;

        }

        /// <summary>
        /// 查询期货相应盘后的资金清算流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryQHFlow_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            dagGZQHFlowDetail.AutoGenerateColumns = false;
            string msg = "";
            List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();

            // List<QH_TradeCapitalFlowDetailInfo> entry = wcfLogic.QueryQHCapitalFlowDetail((QueryTypeQueryCurrencyType)cmbQHFlowCury.SelectedIndex, txtPwd.Text.Trim(), out msg);
            int x = cmbQHFlowCury.SelectedIndex;
            List<QH_TradeCapitalFlowDetailInfo> entry = new List<QH_TradeCapitalFlowDetailInfo>();
            if (x == 0)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.ALL, txtPwd.Text.Trim(), out msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.RMB, txtPwd.Text.Trim(), out msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.HK, txtPwd.Text.Trim(), out msg);
            }
            else if (x == 3)
            {
                entry = wcfLogic.QueryQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.US, txtPwd.Text.Trim(), out msg);
            }
            if (!string.IsNullOrEmpty(msg))
            {
                errPro.SetError(txtPwd, msg);
            }

            if (entry != null)
            {
                list = entry;
            }
            dagGZQHFlowDetail.DataSource = list;
        }

        /// <summary>
        /// 期货历史成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //CurrentQueryValue.QueryQHHistoryTradeNO = txtQueryQHTradeNo.Text;

            BindGZQHHistoryTradeList();
            pageControlGZQH_HistoryTrade.Visible = true;
            pageControlGZQH_HistoryTrade.BindData();
            // this.daQHHistoryTrade.DataSource = new SortableBindingList<QH_HistoryTradeTableInfo>(wcfLogic.QHHistoryTrade);
        }

        /// <summary>
        /// 期货历史委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            BindGZQHHistoryEntrustList();
            pageControlGZQH_HistoryEntrust.Visible = true;
            pageControlGZQH_HistoryEntrust.BindData();
        }

        /// <summary>
        /// 股指期货历史成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        private void pageControlGZQH_HistoryTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindGZQHHistoryTradeList();
        }

        /// <summary>
        /// 股指期货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlGZQH_HistoryEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindGZQHHistoryEntrustList();
        }
        /// <summary>
        /// 股指期货当日委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlGZQH_TodayEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindGZQHTodayEntrustList();
        }

        /// <summary>
        /// 股指期货当日成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlGZQH_TodayTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindGZQHTodayTradeList();
        }

        /// <summary>
        /// 获取列表中数据的Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgQHMarketValue_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgQHMarketValue.SelectedRows)
            {
                HKMarketValue HKMarket = row.DataBoundItem as HKMarketValue;
                if (HKMarket == null)
                {
                    return;
                }
                this.txtQHMarketValue.Text = HKMarket.Code;
            }
        }

        /// <summary>
        /// 委托单号获取事件处理
        /// </summary>
        /// <param name="entrustNumber"></param>
        public void GZQHMessageLogic_OnEntrustSelected(string entrustNumber)
        {
            txtSITodayEntrustQueryNo.Text = entrustNumber;
            txtSITodayTradeQueryNo.Text = entrustNumber;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化翻页控件
        /// </summary>
        private void InitPageControls()
        {

            pageControlGZQH_HistoryTrade.PageSize = pageSize;
            pageControlGZQH_HistoryTrade.CurrentPage = 1;
            pageControlGZQH_HistoryTrade.RecordsCount = 1;
            pageControlGZQH_HistoryTrade.OnPageChanged += new EventHandler(pageControlGZQH_HistoryTrade_OnPageChanged);
            pageControlGZQH_HistoryTrade.Visible = false;

            pageControlGZQH_HistoryEntrust.PageSize = pageSize;
            pageControlGZQH_HistoryEntrust.CurrentPage = 1;
            pageControlGZQH_HistoryEntrust.RecordsCount = 1;
            pageControlGZQH_HistoryEntrust.OnPageChanged += new EventHandler(pageControlGZQH_HistoryEntrust_OnPageChanged);
            pageControlGZQH_HistoryEntrust.Visible = false;

            pageControlGZQH_TodayEntrust.PageSize = pageSize;
            pageControlGZQH_TodayEntrust.CurrentPage = 1;
            pageControlGZQH_TodayEntrust.RecordsCount = 1;
            pageControlGZQH_TodayEntrust.OnPageChanged += new EventHandler(pageControlGZQH_TodayEntrust_OnPageChanged);
            pageControlGZQH_TodayEntrust.Visible = false;


            pageControlGZQH_TodayTrade.PageSize = pageSize;
            pageControlGZQH_TodayTrade.CurrentPage = 1;
            pageControlGZQH_TodayTrade.RecordsCount = 1;
            pageControlGZQH_TodayTrade.OnPageChanged += new EventHandler(pageControlGZQH_TodayTrade_OnPageChanged);
            pageControlGZQH_TodayTrade.Visible = false;
        }


        /// <summary>
        /// 股指期货历史成交查询绑定列表
        /// </summary>
        private void BindGZQHHistoryTradeList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtSIHistoryTradeQueryNo.Text.Trim();
            int icount;
            string msg = "";

            DateTime? sDate = null;
            DateTime? eDate = null;
            if (chkSIDateTime_HistoryTrade.Checked)
            {
                sDate = dtpSIStart_HistoryTrade.Value;
                eDate = dtpSIEnd_HistoryTrade.Value;
            }

            List<QH_HistoryTradeTableInfo> list = wcfLogic.QueryQHHistoryTrade(out icount, pageControlGZQH_HistoryTrade.CurrentPage, pageControlGZQH_HistoryTrade.PageSize, ServerConfig.GZQHCapitalAccount, 6, sDate, eDate, ref msg);
            pageControlGZQH_HistoryTrade.RecordsCount = icount;
            dagGZQHHistoryTrade.DataSource = list;
            dagGZQHHistoryTrade.Columns["GZQHHTCurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.dagGZQHHistoryTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagGZQHHistoryTrade.Rows[i].Cells["GZQHHistoryTradeBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTBuySell"].Value = "买";
                }
                else
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagGZQHHistoryTrade.Rows[i].Cells["GZQHHistoryTradeCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTCurrencyType"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagGZQHHistoryTrade.Rows[i].Cells["GZQHHistoryTradeOpenCloseTypeId"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTOpenClose"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTOpenClose"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTOpenClose"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagGZQHHistoryTrade.Rows[i].Cells["GZQHHistoryTradeTradeUnitId"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTUnit"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTUnit"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTUnit"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dagGZQHHistoryTrade.Rows[i].Cells["GZQHHistoryTradeTradeTypeId"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dagGZQHHistoryTrade.Rows[i].Cells["GZQHHTTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 股指期货历史委托查询绑定列表
        /// </summary>
        private void BindGZQHHistoryEntrustList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtSIHistoryEntrustQueryNo.Text.Trim();
            int icount;
            string msg = "";

            DateTime? sDate = null;
            DateTime? eDate = null;
            if (chkSIDateTime_HistoryEntrust.Checked)
            {
                sDate = dtpSIStart_HistoryEntrust.Value;
                eDate = dtpSIEnd_HistoryEntrust.Value;
            }

            List<QH_HistoryEntrustTableInfo> list = wcfLogic.QueryQHHistoryEntrust(out icount, pageControlGZQH_HistoryEntrust.CurrentPage, pageControlGZQH_HistoryEntrust.PageSize, ServerConfig.GZQHCapitalAccount, 6, sDate, eDate, ref msg);
            pageControlGZQH_HistoryEntrust.RecordsCount = icount;
            dagGZQHHistoryEntrust.DataSource = list;
            dagGZQHHistoryEntrust.Columns["GZQHHEBugSell"].DisplayIndex = 14;
            dagGZQHHistoryEntrust.Columns["GZQHHECurrencyType"].DisplayIndex = 15;
            for (int i = 0; i < this.dagGZQHHistoryEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHistoryEntrustBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEBugSell"].Value = "买";
                }
                else
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEBugSell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHistoryEntrustCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHECurrencyType"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHistoryEntrustOpenCloseTypeId"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEOpenClose"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEOpenClose"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEOpenClose"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHistoryEntrustTradeUnitId"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEUnit"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEUnit"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEUnit"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHistoryEntrustOrderStatusId"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dagGZQHHistoryEntrust.Rows[i].Cells["GZQHHEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuGZQHQuery");
            #region 股指期货查询
            this.tabPageQHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.gpgQueryGZQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQueryGZCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryGZHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryGZTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryGZTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQHQueryTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryQHFlow.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.button6.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.button3.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryMarketValu.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.tabPageQHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageQHTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageQHTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageQHMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageQHTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageQHFlowDetail.Text = ResourceOperate.Instanse.GetResourceByKey("FlowDetail");
            this.tabPageQHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");
            this.lblQHCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblQHPassWord.Text = ResourceOperate.Instanse.GetResourceByKey("PassWords");

            this.lblGZTradeNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblGZEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblGZTodayTradeNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblGZTodayEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            #endregion 股指期货查询
            #region 股指期货多语言
            #region 股指期货 资金dataGridView绑定
            for (int i = 0; i < this.dgQHCapital.ColumnCount; i++)
            {
                string QHCapitalName = dgQHCapital.Columns[i].HeaderText;
                dgQHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(QHCapitalName);
            }
            #endregion 股指期货 资金dataGridView绑定
            #region 股指期货 持仓dataGridView绑定
            for (int i = 0; i < this.dgvGZQHHold.ColumnCount; i++)
            {
                string GZQHHoldName = dgvGZQHHold.Columns[i].HeaderText;
                dgvGZQHHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHHoldName);
            }
            #endregion 股指期货 持仓dataGridView绑定
            #region 股指期货 当日委托dataGridView绑定
            for (int i = 0; i < this.dgvGZQHToday.ColumnCount; i++)
            {
                string GZQHTodayName = dgvGZQHToday.Columns[i].HeaderText;
                dgvGZQHToday.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHTodayName);
            }
            #endregion 股指期货 当日委托dataGridView绑定
            #region 股指期货 当日成交dataGridView绑定
            for (int i = 0; i < this.dagGZQHTodayTrade.ColumnCount; i++)
            {
                string GZQHTodayTradeName = dagGZQHTodayTrade.Columns[i].HeaderText;
                dagGZQHTodayTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHTodayTradeName);
            }
            #endregion 股指期货 当日成交dataGridView绑定
            #region 股指期货 市值DataGridView绑定
            for (int i = 0; i < this.dgQHMarketValue.ColumnCount; i++)
            {
                string GZQHMarketValueValueName = dgQHMarketValue.Columns[i].HeaderText;
                dgQHMarketValue.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHMarketValueValueName);
            }
            #endregion 股指期货 市值成交DataGridView绑定
            #region 股指期货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dgvQHTotalCapital.ColumnCount; i++)
            {
                string GZQHTotalCapitalName = dgvQHTotalCapital.Columns[i].HeaderText;
                dgvQHTotalCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHTotalCapitalName);
            }
            #endregion 股指期货 资金汇总DataGridView绑定
            #region 股指期货 历史委托DataGridView绑定
            for (int i = 0; i < this.dagGZQHHistoryEntrust.ColumnCount; i++)
            {
                string GZQHHistoryEntrustName = dagGZQHHistoryEntrust.Columns[i].HeaderText;
                dagGZQHHistoryEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHHistoryEntrustName);
            }
            #endregion 股指期货 历史委托DataGridView绑定
            #region 股指期货 历史成交DataGridView绑定
            for (int i = 0; i < this.dagGZQHHistoryTrade.ColumnCount; i++)
            {
                string GZQHHistoryTradeName = dagGZQHHistoryTrade.Columns[i].HeaderText;
                dagGZQHHistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHHistoryTradeName);
            }
            #endregion 股指期货 历史成交DataGridView绑定
            #region 股指期货 资金流水DataGridView绑定
            for (int i = 0; i < this.dagGZQHFlowDetail.ColumnCount; i++)
            {
                string GZQHFlowDetailName = dagGZQHFlowDetail.Columns[i].HeaderText;
                dagGZQHFlowDetail.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(GZQHFlowDetailName);
            }
            #endregion 股指期货 资金流水DataGridView绑定
            #endregion 股指期货多语言
        }

        #endregion

        /// <summary>
        ///  股指期货资金信息查询列值绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgQHCapital_Sorted(object sender, EventArgs e)
        {
            #region 币种
            string CurrencyType = dgQHCapital.Rows[0].Cells["GZQHTradeCurrencyType"].Value.ToString();
            if (CurrencyType.Equals("1"))
            {
                dgQHCapital.Rows[0].Cells["GZQHCurrency"].Value = "人民币";
            }
            else if (CurrencyType.Equals("2"))
            {
                dgQHCapital.Rows[0].Cells["GZQHCurrency"].Value = "港币";
            }
            else if (CurrencyType.Equals("3"))
            {
                dgQHCapital.Rows[0].Cells["GZQHCurrency"].Value = "美元";
            }
            #endregion
        }

        /// <summary>
        /// 股指期货持仓查询列的排序绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGZQHHold_Sorted(object sender, EventArgs e)
        {
            #region 列的数据绑定
            for (int i = 0; i < this.dgvGZQHHold.Rows.Count; i++)
            {
                string CurrencyType = dgvGZQHHold.Rows[i].Cells["GZQHHoldTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHCurrencyType"].Value = "美元";
                }
                string BuySellType = dgvGZQHHold.Rows[i].Cells["GZQHHoldBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHBuySellType"].Value = "买";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvGZQHHold.Rows[i].Cells["GZQHBuySellType"].Value = "卖";
                }
            }
            #endregion
        }

        /// <summary>
        /// 股指期货当日委托查询列排序数据绑定翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGZQHToday_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgvGZQHToday.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEBuySell"].Value = "买";
                }
                else
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTECurrencyType"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustOpenCloseTypeId"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEOpenClose"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEOpenClose"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEOpenClose"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustTradeUnitId"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dgvGZQHToday.Rows[i].Cells["GZQHTodayEntrustOrderStatusId"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dgvGZQHToday.Rows[i].Cells["GZQHTEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 双击当日委托列表中数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGZQHToday_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvGZQHToday.SelectedRows)
            {
                QH_TodayEntrustTableInfo TodayEntrust = row.DataBoundItem as QH_TodayEntrustTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtSITodayEntrustQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }
        /// <summary>
        /// 双击当日成交列表中数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagGZQHTodayTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagGZQHTodayTrade.SelectedRows)
            {
                QH_TodayTradeTableInfo TodayEntrust = row.DataBoundItem as QH_TodayTradeTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtSITodayTradeQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }
        /// <summary>
        /// 双击历史成交列表中数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagGZQHHistoryTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagGZQHHistoryTrade.SelectedRows)
            {
                QH_HistoryTradeTableInfo TodayEntrust = row.DataBoundItem as QH_HistoryTradeTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtSIHistoryTradeQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }
        /// <summary>
        /// 双击历史委托列表中数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagGZQHHistoryEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagGZQHHistoryEntrust.SelectedRows)
            {
                QH_HistoryEntrustTableInfo TodayEntrust = row.DataBoundItem as QH_HistoryEntrustTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtSIHistoryEntrustQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }
      
    }
}
