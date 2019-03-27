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
using GTA.VTS.CustomersOrders.DoAccountManager;

namespace GTA.VTS.CustomersOrders.AppForm
{
    public partial class frmAdditionalCaption : MdiFormBase
    {
        #region 变量
        internal WCFServices wcfLogic;
        #endregion 变量

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public frmAdditionalCaption()
        {
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
        }
        #endregion 初始化

        #region 窗体事件
        #region 追加资金按钮事件
        /// <summary>
        /// 追加资金按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string ID = this.txtTradeID.Text.ToString();
                //if(string.IsNullOrEmpty(this.textBox2.Text) ||string.IsNullOrEmpty(this.textBox3.Text) ||string.IsNullOrEmpty(this.textBox4.Text))
                //{
                //    MessageBox.Show("xx", "xx");
                //}
                AddCapitalEntity addCapitalEntity = new AddCapitalEntity();
                if (!string.IsNullOrEmpty(this.txtRMB.Text))
                {
                    decimal RMB;
                    if (decimal.TryParse(this.txtRMB.Text.ToString(), out RMB))
                    {
                        addCapitalEntity.AddRMBAmount = RMB;
                    }
                    else
                    {
                        error.Clear();
                        error.SetError(txtRMB, "请输入正确的资金格式!");
                        return;
                    }

                }
                else
                {
                    addCapitalEntity.AddRMBAmount = 0;
                }
                if (!string.IsNullOrEmpty(this.txtHK.Text))
                {
                    decimal HK;
                    if (decimal.TryParse(this.txtHK.Text.ToString(), out HK))
                    {
                        addCapitalEntity.AddHKAmount = HK;
                    }
                    else
                    {
                        error.Clear();
                        error.SetError(txtHK, "请输入正确的资金格式!");
                        return;
                    }
                }
                else
                {
                    addCapitalEntity.AddHKAmount = 0;
                }
                if (!string.IsNullOrEmpty(this.txtUSA.Text))
                {
                    decimal US;
                    if (decimal.TryParse(this.txtUSA.Text.ToString(), out US))
                    {
                        addCapitalEntity.AddUSAmount = US;
                    }
                    else
                    {
                        error.Clear();
                        error.SetError(txtUSA, "请输入正确的资金格式!");
                        return;
                    }
                }
                else
                {
                    addCapitalEntity.AddUSAmount = 0;
                }
                addCapitalEntity.TraderID = ID;
                //addCapitalEntity.AddHKAmount = HK;
                //addCapitalEntity.AddRMBAmount = RMB;
                //addCapitalEntity.AddUSAmount = US;
                string message = "";
                //GTA.VTS.CustomersOrders.TransactionManageService.CT_Counter counter =
                //               wcfLogic.TransactionConfirm(UserID, "69ggKIQJgh4=", out message);
                List<GTA.VTS.CustomersOrders.DoAccountManager.AccountFindResultEntity> list = wcfLogic.FindAccount(ID);
                string BankCapitalAccount = "";
                foreach (AccountFindResultEntity accountFindResultEntity in list)
                {
                    string Name = accountFindResultEntity.AccountName;
                    if (Name.Equals("银行账号"))
                    {
                        BankCapitalAccount = accountFindResultEntity.AccountID;
                    }
                }
                //string BankCapitalAccount = ProductionAccount.FormationAccount(counter.CouterID, UserID, 1);
                addCapitalEntity.BankCapitalAccount = BankCapitalAccount;
                string errMsg = "";
                bool x = wcfLogic.AddCapital(addCapitalEntity, out errMsg);
                if (x == true)
                {
                    error.Clear();
                    MessageBox.Show("追加资金成功！", "系统提示");
                    this.txtTradeID.Text = "";
                    this.txtRMB.Text = "";
                    this.txtHK.Text = "";
                    this.txtUSA.Text = "";
                }
                else
                {
                    error.SetError(txtTradeID, errMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

        }
        #endregion

        #region 取消按钮事件
        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.txtTradeID.Text = "";
            this.txtRMB.Text = "";
            this.txtHK.Text = "";
            this.txtUSA.Text = "";
        }
        #endregion

        #region 多语言翻译
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            #region 资金dataGridView绑定

            this.lblTradeId.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestTradeID");
            this.btnCancel.Text = ResourceOperate.Instanse.GetResourceByKey("Cancel");
            this.lblRMB.Text = ResourceOperate.Instanse.GetResourceByKey("RMB");
            this.lblHK.Text = ResourceOperate.Instanse.GetResourceByKey("HK");
            this.lblUSA.Text = ResourceOperate.Instanse.GetResourceByKey("USA");
            this.btnOK.Text = ResourceOperate.Instanse.GetResourceByKey("OK");
            this.grbCaption.Text = ResourceOperate.Instanse.GetResourceByKey("Additional");
            this.Text = ResourceOperate.Instanse.GetResourceByKey("Additional");
            #endregion 资金dataGridView绑定
        }
        #endregion

        /// <summary>
        /// 清算是否完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                error.Clear();
                //DateTime z = DateTime.Parse(this.dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                DateTime x = DateTime.Parse(this.txtTime.Text);
                if (!string.IsNullOrEmpty(this.txtTime.Text))
                {
                    bool y = wcfLogic.IsReckoningDone(x);
                    if (y == true)
                    {
                        this.label5.Text = "是";
                    }
                    else
                    {
                        this.label5.Text = "否";
                    }
                }
                else
                {
                    error.Clear();
                    error.SetError(txtTime, "请输入查询时间！");
                }
            }
            catch (Exception ex)
            {
                error.Clear();
                error.SetError(txtTime, ex.Message);
                LogHelper.WriteError(ex.Message,ex);
            }
        }

        /// <summary>
        /// 判断是否正在进行清算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            error.Clear();
            bool y = wcfLogic.IsReckoning();
            if (y == true)
            {
                this.label6.Text = "是";
            }
            else
            {
                this.label6.Text = "否";
            }
        }
        #endregion


    }
}
