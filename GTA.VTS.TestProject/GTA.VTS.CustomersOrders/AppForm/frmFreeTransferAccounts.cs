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
using GTA.VTS.CustomersOrders.BLL;
using FreeTransferEntity = GTA.VTS.CustomersOrders.DoAccountManager.FreeTransferEntity;

namespace GTA.VTS.CustomersOrders.AppForm
{
    public partial class ConvertTransfer : MdiFormBase
    {
        #region 变量
        internal WCFServices wcfLogic;
        #endregion 变量

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public ConvertTransfer()
        {
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            this.cmbFromCaption.SelectedIndex = 0;
            LocalhostResourcesFormText();
        }
        #endregion

        #region 窗体加载
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertTransfer_Load(object sender, EventArgs e)
        {
            this.cmbCurrencyType.SelectedIndex = 0;
            
        }
        #endregion

        #region 窗体事件
        #region 根据转出帐号获取转入帐号类型
        /// <summary>
        /// 根据选择转出帐号类型来获取转入帐号类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFromCaption_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Name = this.cmbFromCaption.SelectedItem.ToString();
            this.cmbToCaption.Items.Clear();
            this.cmbToCaption.Items.Add("银行帐号");
            this.cmbToCaption.Items.Add("证券资金帐号");
            this.cmbToCaption.Items.Add("商品期货资金帐号");
            this.cmbToCaption.Items.Add("股指期货资金帐号");
            this.cmbToCaption.Items.Add("港股资金帐号");
            this.cmbToCaption.Items.Remove(Name);
            this.cmbToCaption.SelectedIndex = 0;
        }
        #endregion

        #region 转账按钮
        /// <summary>
        /// 转账按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string FromCapital = this.cmbFromCaption.SelectedItem.ToString();
                int FromCapitalAccount = Caption(FromCapital);
                string ToCapital = this.cmbToCaption.SelectedItem.ToString();
                int ToCapitalAccount = Caption(ToCapital);
                decimal TransferAmount = 0;
                if (decimal.TryParse(this.txtCaptionNum.Text.ToString(), out TransferAmount))
                {
                    if (!string.IsNullOrEmpty(this.txtCaptionNum.Text.ToString()))
                    {
                        FreeTransferEntity freeTransferEntity = new FreeTransferEntity();
                        freeTransferEntity.TraderID = ServerConfig.TraderID.ToString();
                        freeTransferEntity.FromCapitalAccountType = FromCapitalAccount;
                        freeTransferEntity.ToCapitalAccountType = ToCapitalAccount;
                        freeTransferEntity.TransferAmount = TransferAmount;
                        string Currency = this.cmbCurrencyType.SelectedItem.ToString();
                        Types.CurrencyType currencyType = new Types.CurrencyType();
                        if (Currency.Equals("人民币"))
                        {
                            currencyType = Types.CurrencyType.RMB;
                        }
                        else if (Currency.Equals("港币"))
                        {
                            currencyType = Types.CurrencyType.HK;
                        }
                        else if (Currency.Equals("美元"))
                        {
                            currencyType = Types.CurrencyType.US;
                        }
                        if (TransferAmount > 0)
                        {
                            string errMsg = "";
                            bool x = wcfLogic.TwoAccountsFreeTransferFunds(freeTransferEntity, currencyType, out errMsg);
                            if (x == true)
                            {
                                MessageBox.Show("转账成功", "系统提示");
                                this.txtCaptionNum.Text = "";
                                if (this.cmbToCaption.SelectedItem.Equals("银行帐号") ||
                                    this.cmbFromCaption.SelectedItem.Equals("银行帐号"))
                                {
                                    QueryUA_BankAccountByUserID();
                                }
                            }
                            else
                            {
                                error.SetError(txtCaptionNum, "转账失败!" + errMsg);
                            }
                        }
                        else
                        {
                            error.SetError(txtCaptionNum, "转账金额必须大于零!");
                        }
                    }
                    else
                    {
                        error.SetError(txtCaptionNum, "转账金额不能为空!");
                    }
                }
                else
                {
                    error.SetError(txtCaptionNum, "转账金额输入错误!");
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        #region 根据选择不同的转出转入帐号获取对应的帐号类型编号
        /// <summary>
        /// 根据帐号类型获取对应的类型值
        /// </summary>
        /// <param name="Caption"></param>
        /// <returns></returns>
        public int Caption(string Caption)
        {
            int x = 0;
            if (Caption.Equals("银行帐号"))
            {
               // x = 1;
                x = (int)Types.AccountType.BankAccount;
            }
            else if (Caption.Equals("证券资金帐号"))
            {
                x = (int)Types.AccountType.StockSpotCapital;
            }
            else if (Caption.Equals("商品期货资金帐号"))
            {
                x = (int)Types.AccountType.CommodityFuturesCapital;
            }
            else if (Caption.Equals("股指期货资金帐号"))
            {
                x = (int)Types.AccountType.StockFuturesCapital;
            }
            else if (Caption.Equals("港股资金帐号"))
            {
                x = (int)Types.AccountType.HKSpotCapital;
            }
            return x;
        }
        #endregion

        #region 取消按钮时间
        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.txtCaptionNum.Text = "";
        }
        #endregion 取消按钮时间

        #region 资金查询
        /// <summary>
        /// 资金查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            QueryUA_BankAccountByUserID();
        }
        #endregion

        #region 多语言翻译
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.Text = ResourceOperate.Instanse.GetResourceByKey("Freetransfer");
            #region 资金dataGridView绑定
            for (int i = 0; i < this.dagCaption.ColumnCount; i++)
            {
                string QHCapitalName = dagCaption.Columns[i].HeaderText;
                dagCaption.Columns[i].HeaderCell.Value = ResourceOperate.Instanse.GetResourceByKey(QHCapitalName);
            }
            this.btnQuery.Text = ResourceOperate.Instanse.GetResourceByKey("Query");
            this.btnCancel.Text = ResourceOperate.Instanse.GetResourceByKey("Cancel");
            this.lblFromCaption.Text = ResourceOperate.Instanse.GetResourceByKey("FromCaption");
            this.lblToCaption.Text = ResourceOperate.Instanse.GetResourceByKey("ToCaption");
            this.lblCurrencyType.Text = ResourceOperate.Instanse.GetResourceByKey("CurrencyType");
            this.lblTransferAmount.Text = ResourceOperate.Instanse.GetResourceByKey("lblCapitalAmount");
            this.groupBox1.Text = ResourceOperate.Instanse.GetResourceByKey("Freetransfer");
            this.groupBox2.Text = ResourceOperate.Instanse.GetResourceByKey("grpQueryTerm");
            #endregion 资金dataGridView绑定
        }
        #endregion

        #region 查询绑定资金
        /// <summary>
        /// 查询资金并绑定资金
        /// </summary>
        private void QueryUA_BankAccountByUserID()
        {
            this.dagCaption.DataSource = wcfLogic.QueryUA_BankAccountByUserID();

            for (int i = 0; i < this.dagCaption.Rows.Count; i++)
            {
                string CurrencyType = dagCaption.Rows[i].Cells["TradeCurrencyTypeLogoFie"].Value.ToString();
                if (CurrencyType.Equals("1"))
                {
                    dagCaption.Rows[i].Cells["CurrencyType"].Value = "人民币";
                }
                else if (CurrencyType.Equals("2"))
                {
                    dagCaption.Rows[i].Cells["CurrencyType"].Value = "港币";
                }
                else if (CurrencyType.Equals("3"))
                {
                    dagCaption.Rows[i].Cells["CurrencyType"].Value = "美元";
                }
            }
        }
        #endregion
        #endregion
    }
}
