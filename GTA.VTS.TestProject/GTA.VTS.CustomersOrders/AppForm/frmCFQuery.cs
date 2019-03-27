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
    /// Desc:商品期货查询窗体
    /// Create by:董鹏
    /// Create Data:2010-03-02
    /// </summary>
    public partial class frmCFQuery : MdiFormBase
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
        public frmCFQuery()
        {
            pageSize = ServerConfig.QueryPageSize;
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
            SPQHMessageLogic.OnEntrustSelected += new EntrustNoEventHandler(SPQHMessageLogic_OnEntrustSelected);
        }

        #endregion

        #region 窗体事件
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCFQuery_Load(object sender, EventArgs e)
        {
            InitPageControls();

            cmbSPQHCureny.SelectedIndex = 0;
            cmbSPQHFlowCury.SelectedIndex = 0;
            this.txtSPQHPwd.Text = ServerConfig.PassWord;
        }

        /// <summary>
        /// 商品期货资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuerySPCapital_Click(object sender, EventArgs e)
        {
            SortableBindingList<QH_CapitalAccountTableInfo> list = new SortableBindingList<QH_CapitalAccountTableInfo>(wcfLogic.SPQHCapital);
            this.dagSPQHCapital.DataSource = list;
            #region 币种
            for (int i = 0; i < this.dagSPQHFlowDetail.Rows.Count; i++)
            {
                string CurrencyType = dagSPQHCapital.Rows[i].Cells["SPQHTradeCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHCapital.Rows[i].Cells["TradeCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "美元";
                }
            }
            #endregion
        }
        /// <summary>
        /// 商品期货持仓查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuerySPHold_Click(object sender, EventArgs e)
        {

            SortableBindingList<QH_HoldAccountTableInfo> list = new SortableBindingList<QH_HoldAccountTableInfo>(wcfLogic.SPQHHold);
            this.dagSPQHHold.DataSource = list;
            //for (int i = 0; i < dagSPQHHold.Columns.Count; i++)
            //{
            //    dagSPQHHold.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
            dagSPQHHold.Columns["SPQHCurrencyType"].DisplayIndex = 12;
            dagSPQHHold.Columns["BuySellType"].DisplayIndex = 11;
            #region 对列表中的币种和买卖类型进行转换
            for (int i = 0; i < this.dagSPQHHold.Rows.Count; i++)
            {
                string BuySellType = dagSPQHHold.Rows[i].Cells["SPQHBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHHold.Rows[i].Cells["BuySellType"].Value = "买";
                }
                else
                {
                    dagSPQHHold.Rows[i].Cells["BuySellType"].Value = "卖";
                }

                string CurrencyType = dagSPQHHold.Rows[i].Cells["dataGridViewTextBoxColumn170"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHHold.Rows[i].Cells["SPQHCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "美元";
                }
            }
            #endregion 对列表中的币种和买卖类型进行转换
        }
        /// <summary>
        /// 商品期货当日委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuerySPQHEntrust_Click(object sender, EventArgs e)
        {
            BindSPQHTodayEntrustList();
            pageControlSPQH_TodayEntrust.Visible = true;
            pageControlSPQH_TodayEntrust.BindData();
        }
        /// <summary>
        ///商品期货当日委托量查询数据绑定 
        /// </summary>
        private void BindSPQHTodayEntrustList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtCFTodayEntrustQueryNo.Text.Trim();
            int icount;
            string msg = "";
            List<DoCommonQuery.QH_TodayEntrustTableInfo> list = wcfLogic.QueryQHTodayEntrust(out icount, pageControlSPQH_TodayEntrust.CurrentPage, pageControlSPQH_TodayEntrust.PageSize, ServerConfig.SPQHCapitalAccount, 4, ref msg);
            pageControlSPQH_TodayEntrust.RecordsCount = icount;
            this.dagSPQHEntrust.DataSource = new SortableBindingList<QH_TodayEntrustTableInfo>(list);
            dagSPQHEntrust.Columns["Column2"].DisplayIndex = 12;
            for (int i = 0; i < this.dagSPQHEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn172"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column1"].Value = "买";
                }
                else
                {
                    dagSPQHEntrust.Rows[i].Cells["Column1"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn180"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column2"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column2"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column2"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn187"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column3"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column3"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column3"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn194"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn189"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 商品期货当日成交量查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHTrade_Click(object sender, EventArgs e)
        {
            BindSPQHTodayTradeList();
            pageControlSPQH_TodayTrade.Visible = true;
            pageControlSPQH_TodayTrade.BindData();
        }
        /// <summary>
        /// 商品期货当日成交查询数据绑定
        /// </summary>
        private void BindSPQHTodayTradeList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtCFTodayTradeQueryNo.Text.Trim();
            int icount;
            string msg = "";
            List<DoCommonQuery.QH_TodayTradeTableInfo> list = wcfLogic.QueryQHTodayTrade(out icount, pageControlSPQH_TodayTrade.CurrentPage, pageControlSPQH_TodayTrade.PageSize, ServerConfig.SPQHCapitalAccount, 4, ref msg);
            pageControlSPQH_TodayTrade.RecordsCount = icount;
            this.dagSPQHQueryTrade.DataSource = new SortableBindingList<QH_TodayTradeTableInfo>(list);

            // this.dagSPQHQueryTrade.DataSource = new SortableBindingList<QH_TodayTradeTableInfo>(wcfLogic.SPQHTodayTrade);
            dagSPQHQueryTrade.Columns["Column7"].DisplayIndex = 12;
            for (int i = 0; i < this.dagSPQHQueryTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagSPQHQueryTrade.Rows[i].Cells["dataGridViewTextBoxColumn195"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column6"].Value = "买";
                }
                else
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column6"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagSPQHQueryTrade.Rows[i].Cells["dataGridViewTextBoxColumn198"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column7"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column7"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column7"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagSPQHQueryTrade.Rows[i].Cells["dataGridViewTextBoxColumn203"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column8"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column8"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column8"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagSPQHQueryTrade.Rows[i].Cells["dataGridViewTextBoxColumn212"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dagSPQHQueryTrade.Rows[i].Cells["dataGridViewTextBoxColumn211"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 商品获取市价查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHMarket_Click(object sender, EventArgs e)
        {
            dgvQuerySPQHMarket.AutoGenerateColumns = false;
            string mess = "";
            dgvQuerySPQHMarket.DataSource = wcfLogic.QueryMarketValueQHHold(txtQuerySPQHCode.Text.Trim(), 4, ref mess);
        }

        /// <summary>
        /// 商品期货当日金额查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHCapital_Click(object sender, EventArgs e)
        {
            dagQuerySPQHCapital.AutoGenerateColumns = false;
            string msg = "";
            List<FuturesCapitalEntity> list = new List<FuturesCapitalEntity>();
            int x = cmbSPQHCureny.SelectedIndex;
            FuturesCapitalEntity entry = new FuturesCapitalEntity();

            if (x == 0)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.RMB, 4, ref msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.HK, 4, ref msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QueryQHTotalCapital(Types.CurrencyType.US, 4, ref msg);
            }
            //FuturesCapitalEntity entry = wcfLogic.QueryQHTotalCapital((Types.CurrencyType)cmbQHCureny.SelectedIndex + 1, ref msg);
            if (entry != null)
            {
                list.Add(entry);
            }
            dagQuerySPQHCapital.DataSource = list;
        }

        /// <summary>
        /// 商品期货盘后清算流水查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQueryFlowDetail_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            dagSPQHFlowDetail.AutoGenerateColumns = true;
            string msg = "";
            List<QH_TradeCapitalFlowDetailInfo> list = new List<QH_TradeCapitalFlowDetailInfo>();

            // List<QH_TradeCapitalFlowDetailInfo> entry = wcfLogic.QueryQHCapitalFlowDetail((QueryTypeQueryCurrencyType)cmbQHFlowCury.SelectedIndex, txtPwd.Text.Trim(), out msg);
            int x = cmbSPQHFlowCury.SelectedIndex;
            List<QH_TradeCapitalFlowDetailInfo> entry = new List<QH_TradeCapitalFlowDetailInfo>();
            if (x == 0)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.ALL, txtSPQHPwd.Text.Trim(), out msg);
            }
            else if (x == 1)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.RMB, txtSPQHPwd.Text.Trim(), out msg);
            }
            else if (x == 2)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.HK, txtSPQHPwd.Text.Trim(), out msg);
            }
            else if (x == 3)
            {
                entry = wcfLogic.QuerySPQHCapitalFlowDetail(DoCommonQuery.QueryTypeQueryCurrencyType.US, txtSPQHPwd.Text.Trim(), out msg);
            }
            if (!string.IsNullOrEmpty(msg))
            {
                errPro.SetError(txtSPQHPwd, msg);
            }

            if (entry != null)
            {
                list = entry;
            }
            dagSPQHFlowDetail.DataSource = list;
            #region 对币种类型进行修改
            for (int i = 0; i < this.dagSPQHFlowDetail.Rows.Count; i++)
            {
                string CurrencyType = dagSPQHFlowDetail.Rows[i].Cells["SPQHFlowCurrencyType"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "美元";
                }
            }
            #endregion 对币种类型进行修改
        }

        /// <summary>
        /// 商品期货历史成交查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHHistoryTrade_Click(object sender, EventArgs e)
        {
            BindSPQHHistoryTradeList();
            pageControlSPQH_HistoryTrade.Visible = true;
            pageControlSPQH_HistoryTrade.BindData();

        }

        /// <summary>
        /// 商品期货历史委托查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuerySPQHHistoryEntrust_Click(object sender, EventArgs e)
        {
            BindSPQHHistoryEntrustList();
            pageControlSPQH_HistoryEntrust.Visible = true;
            pageControlSPQH_HistoryEntrust.BindData();
        }

        /// <summary>
        /// 商品期货历史委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageControlSPQH_HistoryEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindSPQHHistoryEntrustList();
        }

        /// 商品期货历史成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        private void pageControlSPQH_HistoryTrade_OnPageChanged_1(object sender, EventArgs e)
        {
            BindSPQHHistoryTradeList();
        }

        /// <summary>
        /// 商品期货当日委托查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageControlSPQH_TodayEntrust_OnPageChanged(object sender, EventArgs e)
        {
            BindSPQHTodayEntrustList();
        }
        /// <summary>
        /// 商品期货当日成交查询翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageControlSPQH_TodayTrade_OnPageChanged(object sender, EventArgs e)
        {
            BindSPQHTodayTradeList();
        }
        /// <summary>
        /// 双击类表中数据，并期货对应的Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvQuerySPQHMarket_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvQuerySPQHMarket.SelectedRows)
            {
                HKMarketValue HKMarket = row.DataBoundItem as HKMarketValue;
                if (HKMarket == null)
                {
                    return;
                }
                this.txtQuerySPQHCode.Text = HKMarket.Code;
            }
        }


        /// <summary>
        /// 商品期货持仓查询排序绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHHold_Sorted(object sender, EventArgs e)
        {
            #region 对列表中的币种和买卖类型进行转换
            for (int i = 0; i < this.dagSPQHHold.Rows.Count; i++)
            {
                string BuySellType = dagSPQHHold.Rows[i].Cells["SPQHBuySellTypeId"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHHold.Rows[i].Cells["BuySellType"].Value = "买";
                }
                else
                {
                    dagSPQHHold.Rows[i].Cells["BuySellType"].Value = "卖";
                }

                string CurrencyType = dagSPQHHold.Rows[i].Cells["dataGridViewTextBoxColumn170"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHHold.Rows[i].Cells["SPQHCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "美元";
                }
            }
            #endregion 对列表中的币种和买卖类型进行转换
        }

        /// <summary>
        /// 商品期货资金查询排序绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHCapital_Sorted(object sender, EventArgs e)
        {
            #region 币种
            if (dagSPQHCapital.Rows.Count> 0)
            {
                for (int i = 0; i < this.dagSPQHCapital.Rows.Count; i++)
                {
                    string CurrencyType = dagSPQHCapital.Rows[i].Cells["SPQHTradeCurrencyType"].Value.ToString();
                    if (CurrencyType.Equals("1"))
                    {
                        dagSPQHCapital.Rows[i].Cells["TradeCurrencyType"].Value = "人民币";
                    }
                    else if (CurrencyType.Equals("2"))
                    {
                        dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "港币";
                    }
                    else if (CurrencyType.Equals("3"))
                    {
                        dagSPQHFlowDetail.Rows[i].Cells["CurrencyType"].Value = "美元";
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// 商品期货当日委托查询排序绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHEntrust_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dagSPQHEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn172"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column1"].Value = "买";
                }
                else
                {
                    dagSPQHEntrust.Rows[i].Cells["Column1"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn180"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column2"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column2"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column2"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn187"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column3"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column3"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column3"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn194"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column4"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dagSPQHEntrust.Rows[i].Cells["dataGridViewTextBoxColumn189"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dagSPQHEntrust.Rows[i].Cells["Column5"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 商品期货当日交易量查询排序绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHQueryTrade_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dagSPQHQueryTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagSPQHQueryTrade.Rows[i].Cells["dataGridViewTextBoxColumn195"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column6"].Value = "买";
                }
                else
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column6"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagSPQHQueryTrade.Rows[0].Cells["dataGridViewTextBoxColumn198"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column7"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column7"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column7"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagSPQHQueryTrade.Rows[0].Cells["dataGridViewTextBoxColumn203"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column8"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column8"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column8"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagSPQHQueryTrade.Rows[0].Cells["dataGridViewTextBoxColumn212"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column9"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dagSPQHQueryTrade.Rows[0].Cells["dataGridViewTextBoxColumn211"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dagSPQHQueryTrade.Rows[i].Cells["Column10"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 用户双击当日委托单元格内容发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagSPQHEntrust.SelectedRows)
            {
                QH_TodayEntrustTableInfo TodayEntrust = row.DataBoundItem as QH_TodayEntrustTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtCFTodayEntrustQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }
        /// <summary>
        /// 用户双击当日成交单元格内容发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHQueryTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagSPQHQueryTrade.SelectedRows)
            {
                QH_TodayTradeTableInfo TodayEntrust = row.DataBoundItem as QH_TodayTradeTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtCFTodayTradeQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }

        /// <summary>
        /// 用户双击历史成交单元格内容事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHHistoryTrade_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagSPQHHistoryTrade.SelectedRows)
            {
                QH_HistoryTradeTableInfo TodayEntrust = row.DataBoundItem as QH_HistoryTradeTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtCFHistoryTradeQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }
        /// <summary>
        /// 用户双击历史委托单元格内容事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dagSPQHHistoryEntrust_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in this.dagSPQHHistoryEntrust.SelectedRows)
            {
                QH_HistoryEntrustTableInfo TodayEntrust = row.DataBoundItem as QH_HistoryEntrustTableInfo;
                if (TodayEntrust == null)
                {
                    return;
                }
                this.txtCFHistoryEntrustQueryNo.Text = TodayEntrust.EntrustNumber;
            }
        }

        /// <summary>
        /// 委托单号获取事件处理
        /// </summary>
        /// <param name="entrustNumber"></param>
        public void SPQHMessageLogic_OnEntrustSelected(string entrustNumber)
        {
            txtCFTodayEntrustQueryNo.Text = entrustNumber;
            txtCFTodayTradeQueryNo.Text = entrustNumber;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 商品期货历史成交查询绑定列表
        /// </summary>
        private void BindSPQHHistoryTradeList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtCFHistoryTradeQueryNo.Text.Trim();
            int icount;
            string msg = "";
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (chkCFDateTime_HistoryTrade.Checked)
            {
                sDate = dtpCFStart_HistoryTrade.Value;
                eDate = dtpCFEnd_HistoryTrade.Value;
            }

            List<QH_HistoryTradeTableInfo> list = wcfLogic.QueryQHHistoryTrade(out icount, pageControlSPQH_HistoryTrade.CurrentPage, pageControlSPQH_HistoryTrade.PageSize, ServerConfig.SPQHCapitalAccount, 4, sDate, eDate, ref msg);
            pageControlSPQH_HistoryTrade.RecordsCount = icount;
            dagSPQHHistoryTrade.DataSource = list;
            dagSPQHHistoryTrade.Columns["HistoryTradeCurrencyType"].DisplayIndex = 12;
            for (int i = 0; i < this.dagSPQHHistoryTrade.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagSPQHHistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn234"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["SPQHTradeBuySellType"].Value = "买";
                }
                else
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["SPQHTradeBuySellType"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagSPQHHistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn237"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeCurrencyType"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagSPQHHistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn242"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeOpenClose"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeOpenClose"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeOpenClose"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagSPQHHistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn251"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeUnit"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeUnit"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeUnit"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeUnit"].Value = "份";
                }
                #endregion
                #region 成交类型
                string TradeType = dagSPQHHistoryTrade.Rows[i].Cells["dataGridViewTextBoxColumn250"].Value.ToString();
                if (TradeType.Equals("1"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "买卖成交";
                }
                else if (TradeType.Equals("2"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "撤单成交";
                }
                else if (TradeType.Equals("3"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "分红成交";
                }
                else if (TradeType.Equals("4"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "撤单成交（内部）";
                }
                else if (TradeType.Equals("5"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "废单";
                }
                else if (TradeType.Equals("6"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "保证金不足强行平仓成交";
                }
                else if (TradeType.Equals("7"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "超出最后交易日强行平仓成交";
                }
                else if (TradeType.Equals("8"))
                {
                    dagSPQHHistoryTrade.Rows[i].Cells["HistoryTradeType"].Value = "违法持仓限制强行平仓";
                }
                #endregion
            }
        }

        /// <summary>
        /// 商品期货历史委托查询绑定列表
        /// </summary>
        private void BindSPQHHistoryEntrustList()
        {
            CurrentQueryValue.QueryQHEntrustNO = txtCFHistoryEntrustQueryNo.Text.Trim();
            int icount;
            string msg = "";

            DateTime? sDate = null;
            DateTime? eDate = null;
            if (chkCFDateTime_HistoryEntrust.Checked)
            {
                sDate = dtpCFStart_HistoryEntrust.Value;
                eDate = dtpCFEnd_HistoryEntrust.Value;
            }

            List<QH_HistoryEntrustTableInfo> list = wcfLogic.QueryQHHistoryEntrust(out icount, pageControlSPQH_HistoryTrade.CurrentPage, pageControlSPQH_HistoryTrade.PageSize, ServerConfig.SPQHCapitalAccount, 4, sDate, eDate, ref msg);
            pageControlSPQH_HistoryTrade.RecordsCount = icount;
            dagSPQHHistoryEntrust.DataSource = list;
            dagSPQHHistoryEntrust.Columns["HistoryEntrustCurrencyType"].DisplayIndex = 12;
            for (int i = 0; i < this.dagSPQHHistoryEntrust.Rows.Count; i++)
            {
                #region 买卖类型
                string BuySellType = dagSPQHHistoryEntrust.Rows[i].Cells["dataGridViewTextBoxColumn213"].Value.ToString();
                if (BuySellType.Equals("1"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustBuySell"].Value = "买";
                }
                else
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustBuySell"].Value = "卖";
                }
                #endregion
                #region 币种类型
                string CurrencyType = dagSPQHHistoryEntrust.Rows[i].Cells["dataGridViewTextBoxColumn219"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustCurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustCurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustCurrencyType"].Value = "美元";
                }
                #endregion
                #region 开平方向
                string Open = dagSPQHHistoryEntrust.Rows[i].Cells["dataGridViewTextBoxColumn226"].Value.ToString();
                if (Open.Equals("1"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustOpenClose"].Value = "开仓";
                }
                else if (Open.Equals("2"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustOpenClose"].Value = "平历史";
                }
                else if (Open.Equals("3"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustOpenClose"].Value = "平今";
                }
                #endregion
                #region 单位
                string Unit = dagSPQHHistoryEntrust.Rows[i].Cells["dataGridViewTextBoxColumn233"].Value.ToString();
                if (Unit.Equals("2"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustUnit"].Value = "手";
                }
                else if (Unit.Equals("3"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustUnit"].Value = "张";
                }
                else if (Unit.Equals("4"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustUnit"].Value = "点";
                }
                else if (Unit.Equals("5"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustUnit"].Value = "吨";
                }
                else if (Unit.Equals("6"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustUnit"].Value = "克";
                }
                else if (Unit.Equals("7"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustUnit"].Value = "份";
                }
                #endregion
                #region 委托状态
                string OrderStatusId = dagSPQHHistoryEntrust.Rows[i].Cells["dataGridViewTextBoxColumn228"].Value.ToString();
                if (OrderStatusId.Equals("1"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "无状态";
                }
                else if (OrderStatusId.Equals("2"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "未报";
                }
                else if (OrderStatusId.Equals("3"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "待报";
                }
                else if (OrderStatusId.Equals("4"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "已报待撤";
                }
                else if (OrderStatusId.Equals("5"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "已报";
                }
                else if (OrderStatusId.Equals("6"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "废单";
                }
                else if (OrderStatusId.Equals("7"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "已撤";
                }
                else if (OrderStatusId.Equals("8"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "部撤";
                }
                else if (OrderStatusId.Equals("9"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "部成";
                }
                else if (OrderStatusId.Equals("10"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "已成";
                }
                else if (OrderStatusId.Equals("11"))
                {
                    dagSPQHHistoryEntrust.Rows[i].Cells["HistoryEntrustStatus"].Value = "部成已撤";
                }
                #endregion
            }
        }

        /// <summary>
        /// 初始化翻页控件
        /// </summary>
        private void InitPageControls()
        {
            pageControlSPQH_HistoryEntrust.PageSize = pageSize;
            pageControlSPQH_HistoryEntrust.CurrentPage = 1;
            pageControlSPQH_HistoryEntrust.RecordsCount = 1;
            pageControlSPQH_HistoryEntrust.OnPageChanged += new EventHandler(pageControlSPQH_HistoryEntrust_OnPageChanged);
            pageControlSPQH_HistoryEntrust.Visible = false;

            pageControlSPQH_HistoryTrade.PageSize = pageSize;
            pageControlSPQH_HistoryTrade.CurrentPage = 1;
            pageControlSPQH_HistoryTrade.RecordsCount = 1;
            pageControlSPQH_HistoryTrade.OnPageChanged += new EventHandler(pageControlSPQH_HistoryTrade_OnPageChanged_1);
            pageControlSPQH_HistoryTrade.Visible = false;

            pageControlSPQH_TodayEntrust.PageSize = pageSize;
            pageControlSPQH_TodayEntrust.CurrentPage = 1;
            pageControlSPQH_TodayEntrust.RecordsCount = 1;
            pageControlSPQH_TodayEntrust.OnPageChanged += new EventHandler(pageControlSPQH_TodayEntrust_OnPageChanged);
            pageControlSPQH_TodayEntrust.Visible = false;

            pageControlSPQH_TodayTrade.PageSize = pageSize;
            pageControlSPQH_TodayTrade.CurrentPage = 1;
            pageControlSPQH_TodayTrade.RecordsCount = 1;
            pageControlSPQH_TodayTrade.OnPageChanged += new EventHandler(pageControlSPQH_TodayTrade_OnPageChanged);
            pageControlSPQH_TodayTrade.Visible = false;
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuSPQHQuery");
            #region 商品期货查询
            this.tabPageSPQHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryEntrust");
            this.gpQuerySPQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("gpgCapital");
            this.btnQuerySPHold.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQuerySPQHEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHMarket.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQueryFlowDetail.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.bntQuerySPQHHistoryEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnQuerySPCapital.Text = ResourceOperate.Instanse.GetResourceByKey("Query");

            this.tabPageSPQHHold.Text = ResourceOperate.Instanse.GetResourceByKey("Hold");
            this.tabPageSPQHEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("Entrust");
            this.tabPageSPQHTrade.Text = ResourceOperate.Instanse.GetResourceByKey("Trade");
            this.tabPageSPQHMarket.Text = ResourceOperate.Instanse.GetResourceByKey("MarkeValue");
            this.tabPageSPQHTotal.Text = ResourceOperate.Instanse.GetResourceByKey("TotalCapital");
            this.tabPageSPQHFlowDetail.Text = ResourceOperate.Instanse.GetResourceByKey("FlowDetail");
            this.tabPageSPQHHistoryTrade.Text = ResourceOperate.Instanse.GetResourceByKey("HistoryTrade");

            this.lblSPQHTodayEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblSPQHTodayTradeNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblSPQHEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");
            this.lblSPQHEntrustNumber.Text = ResourceOperate.Instanse.GetResourceByKey("lblXHEntrustNumber");

            this.lblSPQHCode.Text = ResourceOperate.Instanse.GetResourceByKey("lblCode");
            this.lblSPQHPassWord.Text = ResourceOperate.Instanse.GetResourceByKey("PassWords");
            #endregion 商品期货查询
            #region 商品期货多语言
            #region 商品期货 资金dataGridView绑定
            for (int i = 0; i < this.dagSPQHCapital.ColumnCount; i++)
            {
                string SPQHCapitalName = dagSPQHCapital.Columns[i].HeaderText;
                dagSPQHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHCapitalName);
            }
            #endregion 商品期货 资金dataGridView绑定
            #region 股指期货 持仓dataGridView绑定
            for (int i = 0; i < this.dagSPQHHold.ColumnCount; i++)
            {
                string SPQHHoldName = dagSPQHHold.Columns[i].HeaderText;
                dagSPQHHold.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHHoldName);
            }
            #endregion 股指期货 持仓dataGridView绑定
            #region 股指期货 当日委托dataGridView绑定
            for (int i = 0; i < this.dagSPQHEntrust.ColumnCount; i++)
            {
                string SPQHTodayName = dagSPQHEntrust.Columns[i].HeaderText;
                dagSPQHEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHTodayName);
            }
            #endregion 股指期货 当日委托dataGridView绑定
            #region 股指期货 当日成交dataGridView绑定
            for (int i = 0; i < this.dagSPQHQueryTrade.ColumnCount; i++)
            {
                string SPQHTodayTradeName = dagSPQHQueryTrade.Columns[i].HeaderText;
                dagSPQHQueryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHTodayTradeName);
            }
            #endregion 股指期货 当日成交dataGridView绑定
            #region 商品期货 市值DataGridView绑定
            for (int i = 0; i < this.dgvQuerySPQHMarket.ColumnCount; i++)
            {
                string SPQHMarketValueValueName = dgvQuerySPQHMarket.Columns[i].HeaderText;
                dgvQuerySPQHMarket.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHMarketValueValueName);
            }
            #endregion 商品期货 市值成交DataGridView绑定
            #region 商品期货 资金汇总DataGridView绑定
            for (int i = 0; i < this.dagQuerySPQHCapital.ColumnCount; i++)
            {
                string SPQHTotalCapitalName = dagQuerySPQHCapital.Columns[i].HeaderText;
                dagQuerySPQHCapital.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHTotalCapitalName);
            }
            #endregion 商品期货 资金汇总DataGridView绑定
            #region 商品期货 历史委托DataGridView绑定
            for (int i = 0; i < this.dagSPQHHistoryEntrust.ColumnCount; i++)
            {
                string SPQHHistoryEntrustName = dagSPQHHistoryEntrust.Columns[i].HeaderText;
                dagSPQHHistoryEntrust.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHHistoryEntrustName);
            }
            #endregion 商品期货 历史委托DataGridView绑定
            #region 商品期货 历史成交DataGridView绑定
            for (int i = 0; i < this.dagSPQHHistoryTrade.ColumnCount; i++)
            {
                string SPQHHistoryTradeName = dagSPQHHistoryTrade.Columns[i].HeaderText;
                dagSPQHHistoryTrade.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHHistoryTradeName);
            }
            #endregion 商品期货 历史成交DataGridView绑定
            #region 商品期货 资金流水DataGridView绑定
            for (int i = 0; i < this.dagSPQHFlowDetail.ColumnCount; i++)
            {
                string SPQHFlowDetailName = dagSPQHFlowDetail.Columns[i].HeaderText;
                dagSPQHFlowDetail.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(SPQHFlowDetailName);
            }
            #endregion 商品期货 资金流水DataGridView绑定
            #endregion 商品期货多语言
        }

        #endregion
    }
}
