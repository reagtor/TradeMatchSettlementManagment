using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.TransactionManageService;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// Desc: 数据重新初始化窗体
    /// Create by: 董鹏
    /// Create Date: 2010-03-02
    /// </summary>
    public partial class frmReinitCaptial : MdiFormBase
    {
        /// <summary>
        /// WCF服务访问对象
        /// </summary>
        WCFServices wcfLogic;

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmReinitCaptial()
        {
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
            cmbCapitalType.SelectedIndex = 0;
            cmbmoney.SelectedIndex = 0;
        }

        ///// <summary>
        ///// 窗体加载事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void frmReinitCaptial_Load(object sender, EventArgs e)
        //{
        //    LocalhostResourcesFormText();
        //}
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            #region 试玩测试界面
            this.lblTestTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestTradeID");
            this.lblTestCapitalType.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestCapitalType");
            this.lblTestCapitalCurrency.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestCapitalCurrency");
            this.lblTestCapitalAmount.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestCapitalAmount");
            this.lblTestDesc.Text = ResourceOperate.Instanse.GetResourceByKey("lblTestDesc");
            this.Text = ResourceOperate.Instanse.GetResourceByKey("gpgTest");
            this.butCleat.Text = ResourceOperate.Instanse.GetResourceByKey("butCleat");
            this.butPersonalization.Text = ResourceOperate.Instanse.GetResourceByKey("butPersonalization");
            #endregion 试玩测试界面
        }
        /// <summary>
        /// 清空数据按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCleat_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            List<string> list = new List<string>();
            string Prompted = ResourceOperate.Instanse.GetResourceByKey("Prompted");

            if (string.IsNullOrEmpty(this.txtUserId.Text))
            {
                errPro.Clear();
                string UserIdError = ResourceOperate.Instanse.GetResourceByKey("UserIdError");
                errPro.SetError(txtUserId, UserIdError);
                return;
            }
            else
            {
                foreach (var item in txtUserId.Text.Split(','))
                {
                    if (Utils.DecimalTest(item))
                    {
                        list.Add(item);
                    }
                }
                string CapitalType = this.cmbCapitalType.SelectedIndex.ToString();
                if (CapitalType.Equals("-1"))
                {
                    errPro.Clear();
                    string CapitalNull = ResourceOperate.Instanse.GetResourceByKey("CapitalNull");
                    errPro.SetError(cmbCapitalType, CapitalNull);
                    return;
                }
                string money = this.cmbmoney.SelectedIndex.ToString();
                if (money.Equals("-1"))
                {
                    errPro.Clear();
                    string CurrencyError = ResourceOperate.Instanse.GetResourceByKey("Currency");
                    errPro.SetError(cmbmoney, CurrencyError);
                    return;
                }
                decimal Capital;
                if (decimal.TryParse(this.txtCapitals.Text, out Capital))
                {
                    if (Capital < 0)
                    {
                        errPro.Clear();
                        string Capitalamount = ResourceOperate.Instanse.GetResourceByKey("Capitalamount");
                        errPro.SetError(txtCapitals, Capitalamount);
                        return;
                    }
                }
                else
                {
                    errPro.Clear();
                    string Capitalamountillegal = ResourceOperate.Instanse.GetResourceByKey("Capitalamountillegal");
                    errPro.SetError(txtCapitals, Capitalamountillegal);
                    return;
                }
                #region 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                string LoginName = "Admin";
                string pwd = "admin";
                string password = Encrypt.DesEncrypt(pwd, string.Empty);
                #endregion 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                PersonalizationCapital personalization = new PersonalizationCapital();
                personalization.TradeID = list;
                int CapitalCode = 0;
                if (CapitalType.Equals("0"))
                {
                    CapitalCode = 0;
                }
                else if (CapitalType.Equals("1"))
                {
                    CapitalCode = 1;
                }
                else if (CapitalType.Equals("2"))
                {
                    CapitalCode = 2;
                }
                else if (CapitalType.Equals("3"))
                {
                    CapitalCode = 3;
                }
                else if (CapitalType.Equals("4"))
                {
                    CapitalCode = 4;
                }
                else if (CapitalType.Equals("5"))
                {
                    CapitalCode = 5;
                }
                personalization.PersonalType = CapitalCode;

                if (money.Equals("0"))
                {
                    personalization.SetCurrencyType = 0;
                    personalization.RMBAmount = Capital;
                    personalization.HKAmount = Capital;
                    personalization.USAmount = Capital;
                }
                else if (money.Equals("1"))
                {
                    personalization.SetCurrencyType = 1;
                    personalization.RMBAmount = Capital;
                }
                else if (money.Equals("2"))
                {
                    personalization.SetCurrencyType = 2;
                    personalization.HKAmount = Capital;
                }
                else if (money.Equals("3"))
                {
                    personalization.SetCurrencyType = 3;
                    personalization.USAmount = Capital;
                }
                string message;
                // wcfLogic.GetHighLowRangeValueByCommodityCode("00001",50);
                //调用清空数据服务接口
                bool ClearTrialData = wcfLogic.ClearTrialData(personalization, LoginName, password, out message);
                if (ClearTrialData == true)
                {
                    string Emptysuccessfully = ResourceOperate.Instanse.GetResourceByKey("Emptysuccessfully");
                    MessageBox.Show(Emptysuccessfully, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    string Emptyfailed = ResourceOperate.Instanse.GetResourceByKey("Emptyfailed");
                    MessageBox.Show(Emptyfailed + message, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        /// <summary>
        /// 个性化资金按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butPersonalization_Click(object sender, EventArgs e)
        {
            errPro.Clear();
            List<string> list = new List<string>();
            string Prompted = ResourceOperate.Instanse.GetResourceByKey("Prompted");
            if (string.IsNullOrEmpty(this.txtUserId.Text))
            {
                errPro.Clear();
                string UserIdError = ResourceOperate.Instanse.GetResourceByKey("UserIdError");
                errPro.SetError(txtUserId, UserIdError);
                return;
            }
            else
            {
                foreach (var item in txtUserId.Text.Split(','))
                {
                    if (Utils.DecimalTest(item))
                    {
                        list.Add(item);
                    }
                }
                string CapitalType = this.cmbCapitalType.SelectedIndex.ToString();
                if (CapitalType.Equals("-1"))
                {
                    errPro.Clear();
                    string CapitalNull = ResourceOperate.Instanse.GetResourceByKey("CapitalNull");
                    errPro.SetError(cmbCapitalType, CapitalNull);
                    return;
                }
                string money = this.cmbmoney.SelectedIndex.ToString();
                if (money.Equals("-1"))
                {
                    errPro.Clear();
                    string CurrencyError = ResourceOperate.Instanse.GetResourceByKey("Currency");
                    errPro.SetError(cmbmoney, CurrencyError);
                    return;
                }
                decimal Capital;
                if (decimal.TryParse(this.txtCapitals.Text, out Capital))
                {
                    if (Capital < 0)
                    {
                        errPro.Clear();
                        string Capitalamount = ResourceOperate.Instanse.GetResourceByKey("Capitalamount");
                        errPro.SetError(txtCapitals, Capitalamount);
                        return;
                    }
                }
                else
                {
                    errPro.Clear();
                    string Capitalamountillegal = ResourceOperate.Instanse.GetResourceByKey("Capitalamountillegal");
                    errPro.SetError(txtCapitals, Capitalamountillegal);
                    return;
                }
                #region 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                string LoginName = "Admin";
                string pwd = "admin";
                string password = Encrypt.DesEncrypt(pwd, string.Empty);
                #endregion 因需要用户名和密码，所有使用默认的用户名和密码并对密码进行加密
                GTA.VTS.CustomersOrders.TransactionManageService.PersonalizationCapital personalization = new PersonalizationCapital();
                personalization.TradeID = list;
                int CapitalCode = 0;
                if (CapitalType.Equals("0"))
                {
                    CapitalCode = 0;
                }
                else if (CapitalType.Equals("1"))
                {
                    CapitalCode = 1;
                }
                else if (CapitalType.Equals("2"))
                {
                    CapitalCode = 2;
                }
                else if (CapitalType.Equals("3"))
                {
                    CapitalCode = 3;
                }
                else if (CapitalType.Equals("4"))
                {
                    CapitalCode = 4;
                }
                else if (CapitalType.Equals("5"))
                {
                    CapitalCode = 5;
                }
                personalization.PersonalType = CapitalCode;

                if (money.Equals("0"))
                {
                    personalization.SetCurrencyType = 0;
                    personalization.RMBAmount = Capital;
                    personalization.HKAmount = Capital;
                    personalization.USAmount = Capital;
                }
                else if (money.Equals("1"))
                {
                    personalization.SetCurrencyType = 1;
                    personalization.RMBAmount = Capital;
                }
                else if (money.Equals("2"))
                {
                    personalization.SetCurrencyType = 2;
                    personalization.HKAmount = Capital;
                }
                else if (money.Equals("3"))
                {
                    personalization.SetCurrencyType = 3;
                    personalization.USAmount = Capital;
                }
                string message;
                //调用个性
                bool PersonalizationCapital = wcfLogic.PersonalizationCapital(personalization, LoginName, password, out message);
                if (PersonalizationCapital == true)
                {
                    string Personalizedsuccessful = ResourceOperate.Instanse.GetResourceByKey("Personalizedsuccessful");
                    MessageBox.Show(Personalizedsuccessful, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    string Personalizedfailure = ResourceOperate.Instanse.GetResourceByKey("Personalizedfailure");
                    MessageBox.Show(Personalizedfailure + message, Prompted, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
