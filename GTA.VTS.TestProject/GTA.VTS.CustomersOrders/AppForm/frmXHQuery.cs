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
    /// Desc:现货查询窗体
    /// Create by:董鹏
    /// Create Data:2010-03-02
    /// </summary>
    public partial class frmXHQuery : MdiFormBase
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
        public frmXHQuery()
        {
            pageSize = ServerConfig.QueryPageSize;
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();

            XHMessageLogic.OnEntrustSelected += new EntrustNoEventHandler(XHMessageLogic_OnEntrustSelected);
        }

        #endregion

        #region 窗体事件

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmXHQuery_Load(object sender, EventArgs e)
        {
            InitPageControls();
            cmbXHCurenyType.SelectedIndex = 0;
        }

        /// <summary>
        /// 现货历史成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageControlXH_HistoryTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindXHHistoryTradeList();
        }

        /// <summary>
        /// 现货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlXH_HistoryEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindXHHistoryEntrustList();
        }

        /// <summary>
        /// 现货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlXH_TodayEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindXHTodayEntrustList();
        }

        /// <summary>
        /// 现货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pageControlXH_ToadyTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindXHTodayTradeList();
        }

        /// <summary>
        /// 现货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHCapital_Click(object sender, EventArgs e)
        {
            this.dgXHCapital.DataSource = new SortableBindingList<XH_CapitalAccountTableInfo>(wcfLogic.XHCapital);
            #region 币种
            dgXHCapital.Columns["XHQueryCurrencyType"].DisplayIndex = 8;
            for (int i = 0; i < this.dgXHCapital.Rows.Count; i++)
            {
                string CurrencyType = dgXHCapital.Rows[i].Cells["XHTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dgXHCapital.Rows[i].Cells["XHQueryCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgXHCapital.Rows[i].Cells["XHQueryCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgXHCapital.Rows[i].Cells["XHQueryCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        /// <summary>
        /// 资金查询币种列的绑定翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgXHCapital_Sorted(object sender, EventArgs e)
        {
            #region 币种
            dgXHCapital.Columns["XHQueryCurrencyType"].DisplayIndex = 8;
            string CurrencyType = dgXHCapital.Rows[0].Cells["XHTradeCurrencyType"].Value.ToString();
            for (int i = 0; i < this.dgXHCapital.Rows.Count; i++)
            {
                if (CurrencyType.Equals("1"))
                {
                    dgXHCapital.Rows[i].Cells["XHQueryCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dgXHCapital.Rows[i].Cells["XHQueryCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dgXHCapital.Rows[i].Cells["XHQueryCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        /// <summary>
        /// 现货持仓查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHHold_Click(object sender, EventArgs e)
        {
            this.daXHHold.DataSource = new SortableBindingList<XH_AccountHoldTableInfo>(wcfLogic.XHHold);
            #region 币种
            daXHHold.Columns["XHHoldCurrencyType"].DisplayIndex = 7;
            daXHHold.Columns["XHAccountHoldLogoId"].DisplayIndex = 8;
            for (int i = 0; i < this.daXHHold.Rows.Count; i++)
            {
                string CurrencyType = daXHHold.Rows[i].Cells["XHCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    daXHHold.Rows[i].Cells["XHHoldCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    daXHHold.Rows[i].Cells["XHHoldCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    daXHHold.Rows[i].Cells["XHHoldCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }

        /// <summary>
        /// 现货当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHTodayEntrust_Click(object sender, EventArgs e)
        {
            BindXHTodayEntrustList();
            pageControlXH_TodayEntrust.Visible = true;
            pageControlXH_TodayEntrust.BindData();
        }

        private void BindXHTodayEntrustList()
        {
            CurrentQueryValue.QueryXHEntrustNO = txtQueryXHNumber.Text;
            int icount;
            string msg = "";
            List<XH_TodayEntrustTableInfo> list = wcfLogic.QueryXHTodayEntrust(out icount, pageControlXH_TodayEntrust.CurrentPage, pageControlXH_TodayEntrust.PageSize, true, ServerConfig.XHCapitalAccount, ref msg);
            pageControlXH_TodayEntrust.RecordsCount = icount;
            daXHTodayEntrust.DataSource = list;
            daXHTodayEntrust.Columns["XHTEBuySell"].DisplayIndex = 13;
            daXHTodayEntrust.Columns["XHTECurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.daXHTodayEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = daXHTodayEntrust.Rows[i].Cells["XHBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEBuySell"].Value = "买";
                }
                else
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = daXHTodayEntrust.Rows[i].Cells["XHCurrencyTypeIds"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTECurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = daXHTodayEntrust.Rows[i].Cells["XHTradeUnitId"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = daXHTodayEntrust.Rows[i].Cells["XHOrderStatusId"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    daXHTodayEntrust.Rows[i].Cells["XHTEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 现货当日成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryXHTodayTrade_Click(object sender, EventArgs e)
        {
            BindXHTodayTradeList();
            pageControlXH_ToadyTrade.Visible = true;
            pageControlXH_ToadyTrade.BindData();
        }

        private void BindXHTodayTradeList()
        {
            CurrentQueryValue.QueryXHTradeNO = txtQueryXHTradeNo.Text;
            int icount;
            string msg = "";
            List<XH_TodayTradeTableInfo> list = wcfLogic.QueryXHTodayTrade(out icount, pageControlXH_ToadyTrade.CurrentPage, pageControlXH_ToadyTrade.PageSize, true, ServerConfig.XHCapitalAccount, ref msg);
            pageControlXH_ToadyTrade.RecordsCount = icount;
            daXHTodayTrade.DataSource = list;


            daXHTodayTrade.Columns["XHTTBuySell"].DisplayIndex = 13;
            daXHTodayTrade.Columns["XHTTCurencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.daXHTodayTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = daXHTodayTrade.Rows[i].Cells["BuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTBuySell"].Value = "买";
                }
                else
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = daXHTodayTrade.Rows[i].Cells["CurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTCurencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTCurencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTCurencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = daXHTodayTrade.Rows[i].Cells["TradeUnitId"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = daXHTodayTrade.Rows[i].Cells["TradeTypeId"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    daXHTodayTrade.Rows[i].Cells["XHTTTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 现货综合查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHMarketValue_Click(object sender, EventArgs e)
        {
            //dgXHMarketValue.AutoGenerateColumns = false;
            string mess = "";
            dgXHMarketValue.DataSource = wcfLogic.QuerymarketValueXHHold(txtXHMarketValue.Text.Trim(), ref mess);

        }

        /// <summary>
        /// 现货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHTotalCapital_Click(object sender, EventArgs e)
        {
            string msg = "";
            List<SpotCapitalEntity> list = new List<SpotCapitalEntity>();
            // DoCommonQuery.TypesCurrencyType type =
            int x = cmbXHCurenyType.SelectedIndex;
            SpotCapitalEntity entry = new SpotCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryXHTotalCapital(Types.CurrencyType.RMB, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryXHTotalCapital(Types.CurrencyType.HK, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryXHTotalCapital(Types.CurrencyType.US, ref msg);
            }
            if (entry != null)
            {
                list.Add(entry);
            }
            dgvXHTotalCapital.DataSource = list;
        }

        /// <summary>
        /// 现货历史委托查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHHistoryEntrust_Click(object sender, EventArgs e)
        {
            BindXHHistoryEntrustList();
            pageControlXH_HistoryEntrust.Visible = true;
            pageControlXH_HistoryEntrust.BindData();
        }

        /// <summary>
        /// 现货历史成交查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXHHistoryTrade_Click(object sender, EventArgs e)
        {
            BindXHHistoryTradeList();
            pageControlXH_HistoryTrade.Visible = true;
            pageControlXH_HistoryTrade.BindData();
        }

        /// <summary>
        /// 双击列表中委托单号显示在对应的委托单号文本框中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void daXHTodayEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.daXHTodayEntrust.SelectedRows)
            {
                XH_TodayEntrustTableInfo TodayEntrust = row.DataBoundItem as XH_TodayEntrustTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtQueryXHNumber.Text = TodayEntrust.EntrustNumber;
            }
        }

        /// <summary>
        /// 双击现货单元个事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void daXHTodayTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.daXHTodayTrade.SelectedRows)
            {
                XH_TodayTradeTableInfo TodayTrade = row.DataBoundItem as XH_TodayTradeTableInfo;
                if (TodayTrade == null)
                {
                    return;
                }
                this.txtQueryXHTradeNo.Text = TodayTrade.EntrustNumber;
            }
        }

        /// <summary>
        /// 双击列表中数据，获取代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgXHMarketValue_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgXHMarketValue.SelectedRows)
            {
                HKMarketValue HKMarket = row.DataBoundItem as HKMarketValue;
                if (HKMarket == null)
                {
                    return;
                }
                this.txtXHMarketValue.Text = HKMarket.Code;
            }
        }

        /// <summary>
        /// 委托单号获取事件处理
        /// </summary>
        /// <param name="entrustNumber"></param>
        private void XHMessageLogic_OnEntrustSelected(string entrustNumber)
        {
            txtQueryXHNumber.Text = entrustNumber;
            txtQueryXHTradeNo.Text = entrustNumber;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化翻页控件
        /// </summary>
        private void InitPageControls()
        {
            pageControlXH_HistoryTrade.PageSize = pageSize;
            pageControlXH_HistoryTrade.CurrentPage = 1;
            pageControlXH_HistoryTrade.RecordsCount = 1;
            pageControlXH_HistoryTrade.OnPageChanged += new EventHandler(pageControlXH_HistoryTrade_OnPageChanged);
            pageControlXH_HistoryTrade.Visible = false;

            pageControlXH_HistoryEntrust.PageSize = pageSize;
            pageControlXH_HistoryEntrust.CurrentPage = 1;
            pageControlXH_HistoryEntrust.RecordsCount = 1;
            pageControlXH_HistoryEntrust.OnPageChanged += new EventHandler(pageControlXH_HistoryEntrust_OnPageChanged);
            pageControlXH_HistoryEntrust.Visible = false;

            pageControlXH_TodayEntrust.PageSize = pageSize;
            pageControlXH_TodayEntrust.CurrentPage = 1;
            pageControlXH_TodayEntrust.RecordsCount = 1;
            pageControlXH_TodayEntrust.OnPageChanged += new EventHandler(pageControlXH_TodayEntrust_OnPageChanged);
            pageControlXH_TodayEntrust.Visible = false;

            pageControlXH_ToadyTrade.PageSize = pageSize;
            pageControlXH_ToadyTrade.CurrentPage = 1;
            pageControlXH_ToadyTrade.RecordsCount = 1;
            pageControlXH_ToadyTrade.OnPageChanged += new EventHandler(pageControlXH_ToadyTrade_OnPageChanged);
            pageControlXH_ToadyTrade.Visible = false;
        }

        /// <summary>
        /// 绑定现货历史委托列表
        /// </summary>
        private void BindXHHistoryEntrustList()
        {
            int icount;
            string msg = "";

            DateTime? sDate = null;
            DateTime? eDate = null;
            if (chkXHDateTime_HistoryEntrust.Checked)
            {
                sDate = dtpXHStart_HistoryEntrust.Value;
                eDate = dtpXHEnd_HistoryEntrust.Value;
            }

            List<XH_HistoryEntrustTableInfo> list = wcfLogic.QueryXHHistoryEntrust(out icount, pageControlXH_HistoryEntrust.CurrentPage, pageControlXH_HistoryEntrust.PageSize, true, ServerConfig.XHCapitalAccount, ref msg, sDate, eDate);
            pageControlXH_HistoryEntrust.RecordsCount = icount;
            daXHHistoryEntrust.DataSource = list;

            daXHHistoryEntrust.Columns["XHHEBuySell"].DisplayIndex = 13;
            daXHHistoryEntrust.Columns["XHHECurrencyType"].DisplayIndex = 14;
            for (int i = 0; i < this.daXHHistoryEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = daXHHistoryEntrust.Rows[i].Cells["XHHistoryBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEBuySell"].Value = "买";
                }
                else
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = daXHHistoryEntrust.Rows[i].Cells["XHHistoryCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHECurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHECurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHECurrencyType"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = daXHHistoryEntrust.Rows[i].Cells["XHHistoryTradeUnitId"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = daXHHistoryEntrust.Rows[i].Cells["XHHistoryOrderStatusId"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHHEStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    daXHHistoryEntrust.Rows[i].Cells["XHTEStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 绑定现货历史成交列表
        /// </summary>
        private void BindXHHistoryTradeList()
        {
            int icount;
            string msg = "";

            DateTime? sDate = null;
            DateTime? eDate = null;

            if (chkXHDateTime_HistoryTrade.Checked)
            {
                sDate = dtpXHStart_HistoryTrade.Value;
                eDate = dtpXHEnd_HistoryTrade.Value;
            }

            List<XH_HistoryTradeTableInfo> list = wcfLogic.QueryXHHistoryTrade(out icount, pageControlXH_HistoryTrade.CurrentPage, pageControlXH_HistoryTrade.PageSize, true, ServerConfig.XHCapitalAccount, ref msg, sDate, eDate);
            pageControlXH_HistoryTrade.RecordsCount = icount;
            daXHHistoryTrade.DataSource = list;


            daXHHistoryTrade.Columns["XHHTBuySell"].DisplayIndex = 13;
            daXHHistoryTrade.Columns["XHHECurrencyTypr"].DisplayIndex = 14;
            for (int i = 0; i < this.daXHHistoryTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = daXHHistoryTrade.Rows[i].Cells["XHHistoryTrdeBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTBuySell"].Value = "买";
                }
                else
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = daXHHistoryTrade.Rows[i].Cells["XHHistoryTrdeCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHECurrencyTypr"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHECurrencyTypr"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHECurrencyTypr"].Value = "美元";
                }
                #endregion
                #region 单位
                string Unit = daXHHistoryTrade.Rows[i].Cells["XHHistoryTrdeTradeUnitId"].Value.ToString();
                if (Unit.Equals("1"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTUnit"].Value = "股";
                }
                else if (Unit.Equals("2"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = daXHHistoryTrade.Rows[i].Cells["XHHistoryTrdeTradeTypeId"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHHTTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHTTTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHTTTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHTTTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    daXHHistoryTrade.Rows[i].Cells["XHTTTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuXHQuery");
            #region 现货查询
            this.lblXHCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblXHEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblXHEntrustNumbers.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.gpgQueryXHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQueryXHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryXHTodayTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnXHMarketValue.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnXHTotalCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryXHTodayEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQueryXHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.tabPageXHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageXHEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageXHTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageXHMarket.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageXHTotal.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageXHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.tabPageXHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");
            this.btnXHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnXHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.lblXHstart.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblXHend.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.chkXHDateTime_HistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            this.lblXHTradestart.Text = ResourceOperate.Instanse.GetResourceByKey("start");
            this.lblXHTradeend.Text = ResourceOperate.Instanse.GetResourceByKey("end");
            this.chkXHDateTime_HistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("QueryTime");
            #endregion 现货查询
            #region 现货多语言
            #region 现货 资金dataGridView绑定
            for (int i = 0; i < this.dgXHCapital.ColumnCount; i++)
            {
                string XHCapitalName = dgXHCapital.Columns[i].HeaderText;
                dgXHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHCapitalName);
            }
            #endregion 现货 资金dataGridView绑定dgHKCapital
            #region 现货 持仓DataGridView绑定
            for (int i = 0; i < this.daXHHold.ColumnCount; i++)
            {
                string XHHoldName = daXHHold.Columns[i].HeaderText;
                daXHHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHHoldName);
            }
            #endregion 现货 持仓DataGridView绑定
            #region 现货 当日委托DataGridView绑定
            for (int i = 0; i < this.daXHTodayEntrust.ColumnCount; i++)
            {
                string XHTodayEntrustName = daXHTodayEntrust.Columns[i].HeaderText;
                daXHTodayEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHTodayEntrustName);
            }
            #endregion 现货 当日委托DataGridView绑定
            #region 现货 当日成交DataGridView绑定
            for (int i = 0; i < this.daXHTodayTrade.ColumnCount; i++)
            {
                string XHTodayTradeName = daXHTodayTrade.Columns[i].HeaderText;
                daXHTodayTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHTodayTradeName);
            }
            #endregion 现货 当日成交DataGridView绑定
            #region 现货 市值DataGridView绑定
            for (int i = 0; i < this.dgXHMarketValue.ColumnCount; i++)
            {
                string XHMarketValueName = dgXHMarketValue.Columns[i].HeaderText;
                dgXHMarketValue.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHMarketValueName);
            }
            #endregion 现货 市值成交DataGridView绑定
            #region 现货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dgvXHTotalCapital.ColumnCount; i++)
            {
                string XHTotalCapitalName = dgvXHTotalCapital.Columns[i].HeaderText;
                dgvXHTotalCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHTotalCapitalName);
            }
            #endregion 现货 资金汇总DataGridView绑定
            #region 现货 历史委托DataGridView绑定
            for (int i = 0; i < this.daXHHistoryEntrust.ColumnCount; i++)
            {
                string XHHistoryEntrustName = daXHHistoryEntrust.Columns[i].HeaderText;
                daXHHistoryEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHHistoryEntrustName);
            }
            #endregion 现货 历史委托DataGridView绑定
            #region 现货 历史成交DataGridView绑定
            for (int i = 0; i < this.daXHHistoryTrade.ColumnCount; i++)
            {
                string XHHistoryTradeName = daXHHistoryTrade.Columns[i].HeaderText;
                daXHHistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(XHHistoryTradeName);
            }
            #endregion 现货 历史成交DataGridView绑定
            #endregion 现货多语言
        }
        /// <summary>
        /// 对持仓列表中币种进行显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void daXHHold_Sorted(object sender, EventArgs e)
        {
            #region 币种
            daXHHold.Columns["XHHoldCurrencyType"].DisplayIndex = 7;
            daXHHold.Columns["XHAccountHoldLogoId"].DisplayIndex = 8;
            for (int i = 0; i < this.daXHHold.Rows.Count; i++)
            {
                string CurrencyType = daXHHold.Rows[i].Cells["XHCurrencyTypeId"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    daXHHold.Rows[i].Cells["XHHoldCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    daXHHold.Rows[i].Cells["XHHoldCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    daXHHold.Rows[i].Cells["XHHoldCurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        #endregion
    }
}
